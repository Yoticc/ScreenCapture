using System.Runtime.InteropServices;

namespace ScreenCapture.DirectXModels.Mathematic;
[StructLayout(LayoutKind.Sequential)]
public struct Rectangle
{
    public int Left, Top, Right, Bottom;

    public uint Width => (uint)(Right - Left);
    public uint Height => (uint)(Bottom - Top);
}