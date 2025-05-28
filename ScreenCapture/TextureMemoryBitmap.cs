using System.Runtime.InteropServices;

namespace ScreenCapture;
public unsafe struct TextureMemoryBitmap
{
    public TextureMemoryBitmap(TexturePixel* pixels, int width, int height)
    {
        Pixels = pixels;
        Width = width;
        Height = height;
    }

    public readonly TexturePixel* Pixels;
    public readonly int Width;
    public readonly int Height;
}

[StructLayout(LayoutKind.Sequential, Size = 4)]
public unsafe struct TexturePixel
{
    public byte B, G, R, A;
}