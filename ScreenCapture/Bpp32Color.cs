using System.Runtime.InteropServices;

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
namespace ScreenCapture;
[StructLayout(LayoutKind.Sequential, Size = 4)]
public unsafe struct Bpp32Color
{
    public byte B, G, R, A;

    public static Bpp32Color FromArgb(byte a, byte r, byte g, byte b)
    {
        var value = b | g << 0x08 | r << 0x10 | a << 0x18;
        return *(Bpp32Color*)&value;
    }

    public Color ToGdiColor()
    {
        fixed (Bpp32Color* self = &this)
            return Color.FromArgb(*(int*)self);
    }

    public static bool operator ==(Bpp32Color left, Bpp32Color right) => *(int*)&left == *(int*)&right;
    public static bool operator !=(Bpp32Color left, Bpp32Color right) => *(int*)&left != *(int*)&right;
}