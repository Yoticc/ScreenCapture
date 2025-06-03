#pragma warning disable CA1416 // Validate platform compatibility
using ScreenCapture.Internal;
using System;
using System.Drawing.Imaging;
using System.Runtime.Intrinsics.X86;

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
namespace ScreenCapture;
public unsafe struct MemoryBitmap
{
    public MemoryBitmap(void* pixels, int width, int height)
    {
        Pixels = (Color*)pixels;
        (Width, Height) = (width, height);
    }

    public readonly Color* Pixels;
    public readonly int Width, Height;

    public Color this[int x, int y]
    {
        get => Pixels[y * Width + x];
        set => Pixels[y * Width + x] = value;
    }

    public SlicedMemoryBitmap Slice() => new(this, 0, 0, Width, Height);
    public SlicedMemoryBitmap Slice(int x, int y, int width, int height) => new(this, x, y, width, height);

    public Bitmap GetGDIBitmap()
    {
        var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
        var bitmapData = bitmap.LockBits(new(default, bitmap.Size), ImageLockMode.ReadWrite, bitmap.PixelFormat);

        var length = Width * Height / 2;
        var source = (ulong*)Pixels;
        var source_end = source + length;
        var destination = (ulong*)bitmapData.Scan0;

        if (Avx512F.IsSupported)
        {
            for (; source < source_end; source += 8, destination += 8)
            {
                var vector = Avx512F.LoadVector512(source);
                Avx512F.Store(destination, vector);
            }
        }
        else if (Avx.IsSupported)
        {
            for (; source < source_end; source += 4, destination += 4)
            {
                var vector = Avx.LoadVector256(source);
                Avx.Store(destination, vector);
            }
        }
        else if (Sse2.IsSupported)
        {
            for (; source < source_end; source += 2, destination += 2)
            {
                var vector = Sse2.LoadVector128(source);
                Sse2.Store(destination, vector);
            }
        }
        else
        {
            for (; source < source_end; source++, destination++)
                *destination = *source;
        }

        bitmap.UnlockBits(bitmapData);
        return bitmap;
    }

    public void Save(string path)
    {
        using var bitmap = GetGDIBitmap();
        bitmap.Save(path);
    }
}