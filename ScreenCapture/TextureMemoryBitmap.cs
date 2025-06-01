using System.Runtime.InteropServices;

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
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

    public static TexturePixel FromArgb(byte a, byte r, byte g, byte b) => new() { A = a, R = r, G = g, B = b };

    public static bool operator ==(TexturePixel left, TexturePixel right) => *(int*)&left == *(int*)&right;
    public static bool operator !=(TexturePixel left, TexturePixel right) => *(int*)&left != *(int*)&right;
}