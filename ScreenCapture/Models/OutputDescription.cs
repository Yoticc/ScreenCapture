using ScreenCapture.DirectXModels;
using ScreenCapture.DirectXModels.Mathematic;
using System.Runtime.InteropServices;

namespace ScreenCapture;
[StructLayout(LayoutKind.Explicit)]
public unsafe struct OutputDescription
{
    [FieldOffset(0x00)] public fixed char DeviceName[32];
    [FieldOffset(0x40)] public Rectangle DesktopBounds;
    [FieldOffset(0x50)] public bool IsAttachedToDesktop;
    [FieldOffset(0x54)] public DisplayModeRotation DisplayMode;
    [FieldOffset(0x58)] public nint MonitorHandle;
}