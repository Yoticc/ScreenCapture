using System.Runtime.InteropServices;

namespace ScreenCapture;
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct OutDuplFrameInfo
{
    [FieldOffset(0x00)]
    public long LastPresentTime;
    [FieldOffset(0x08)]
    public long LastMouseUpdateTime;
    [FieldOffset(0x10)]
    public uint AccumulatedFrames;
    [FieldOffset(0x14)]
    public bool RectsCoalesced;
    [FieldOffset(0x18)]
    public bool ProtectedContentMaskedOut;
    [FieldOffset(0x1C)]
    public OutDuplPointerPosition PointerPosition;
    [FieldOffset(0x28)]
    public uint TotalMetadataBufferSize;
    [FieldOffset(0x2C)]
    public uint PointerShapeBufferSize;
}