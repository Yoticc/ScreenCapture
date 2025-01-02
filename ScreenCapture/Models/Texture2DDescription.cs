using System.Runtime.InteropServices;

namespace ScreenCapture.DirectXModels;
[StructLayout(LayoutKind.Sequential)]
public struct Texture2DDescription
{
    public uint Width;
    public uint Height;
    public uint MipLevels;
    public uint ArraySize;
    public FormatType Format;
    public SampleDesc SampleDescription;
    public Usage Usage;
    public BindFlags BindFlags;
    public CpuAccessFlags CpuAccessFlags;
    public ResourceOptionFlags ResourceOptionFlags;
}