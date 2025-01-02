using System.Runtime.InteropServices;

namespace ScreenCapture;
[StructLayout(LayoutKind.Sequential, Size = 0x0C, Pack = 1)]
public struct OutDuplPointerPosition
{
    public int PositionX;
    public int PositionY;
    public bool Visible;
}