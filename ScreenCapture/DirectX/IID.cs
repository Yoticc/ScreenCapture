using System.Buffers.Text;

namespace ScreenCapture.Internal;
public unsafe struct IID
{
    public IID(Guid guid) => GUID = guid;
    public IID(ReadOnlySpan<byte> u8string)
    {
        if (!Utf8Parser.TryParse(u8string, out Guid guid, out _, 'N'))
            throw new Exception("Unable to parse guid in IID");

        GUID = guid;
    }

    public Guid GUID;

    public static implicit operator IID(ReadOnlySpan<byte> u8string) => new(u8string);
}