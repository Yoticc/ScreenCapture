namespace ScreenCapture.Internal;
public unsafe struct IID
{
    public IID(Guid guid) => GUID = guid;
    public IID(string u16string) : this(new Guid(u16string)) { }

    public Guid GUID;

    public static implicit operator IID(string guid) => new(guid);
}