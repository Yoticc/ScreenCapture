#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
namespace ScreenCapture;
public unsafe struct SlicedMemoryBitmap
{
    public SlicedMemoryBitmap(MemoryBitmap parent, int x, int y, int width, int height)
    {
        Parent = parent;
        Pixels = Parent.Pixels;
        (ParentWidth, ParentHeight) = (Parent.Width, Parent.Height);
        (X, Y) = (x, y);
        (Width, Height) = (width, height);
    }

    public readonly MemoryBitmap Parent;
    public readonly Bpp32Color* Pixels;
    public readonly int ParentWidth, ParentHeight;
    public readonly int X, Y;
    public readonly int Width, Height;

    public Bpp32Color this[int x, int y]
    {
        get => Pixels[(Y + y) * ParentWidth + X + x];
        set => Pixels[(Y + y) * ParentWidth + X + x] = value;
    }
}