namespace ScreenCapture.DirectXModels;
[Flags]
public enum CpuAccessFlags
{
    None = 0x00,
    Read = 0x20000,
    Write = 0x10000,
    ReadWrite = Read | Write
}