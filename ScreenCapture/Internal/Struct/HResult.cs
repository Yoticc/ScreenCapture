using System.Runtime.InteropServices;

namespace ScreenCapture.Internal;
[StructLayout(LayoutKind.Sequential, Size = 4)]
public unsafe struct HResult
{
    public uint Code;

    public bool Success => Code == 0;

    public void CheckResult()
    {
        if (!Success)
            throw new Exception($"HResult: result is not success, code: {Code:X8}");
    }

    public override string ToString() => $"{Code:X8}";

    public static implicit operator bool(HResult self) => self.Success;
    public static implicit operator uint(HResult self) => self.Code;
}
