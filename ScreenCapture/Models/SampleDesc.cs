using System.Runtime.InteropServices;

namespace ScreenCapture.DirectXModels;
[StructLayout(LayoutKind.Sequential)]
public struct SampleDesc
{
    public SampleDesc(uint count, uint quality) => (Count, Quality) = (count, quality);

    public uint Count;
    public uint Quality;
}