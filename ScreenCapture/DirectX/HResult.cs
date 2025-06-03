using System.Runtime.InteropServices;

namespace ScreenCapture.Internal;
[StructLayout(LayoutKind.Sequential, Size = 4)]
public unsafe struct HResult
{
    public uint Code;

    public bool IsSuccess => Code == 0;
    public bool NoResult => Code == unchecked((uint)-1);

    public void CheckResult()
    {
        if (!IsSuccess)
            throw new Exception($"HResult: result is not success, code: {Code:X8}");
    }

    public override string ToString() => $"{Code:X8}";

    public static implicit operator bool(HResult self) => self.IsSuccess;
    public static implicit operator uint(HResult self) => self.Code;
}
