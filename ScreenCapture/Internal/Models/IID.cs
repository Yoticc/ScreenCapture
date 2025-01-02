namespace ScreenCapture.Internal;
public unsafe struct IID
{
    public IID(Guid guid) => GUID = guid;
    public IID(string guid) : this(new Guid(guid)) { }

    public Guid GUID;

    public static implicit operator IID(string guid) => new(guid);
}