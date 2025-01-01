using System.Runtime.InteropServices;

namespace ScreenCapture.Internal;
[StructLayout(LayoutKind.Sequential, Size = 0x0C, Pack = 1)]
public struct OutDuplPointerPosition
{
    public int PositionX;
    public int PositionY;
    public bool Visible;
}