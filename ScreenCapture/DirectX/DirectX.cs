using ScreenCapture.DirectXModels;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.LayoutKind;

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
namespace ScreenCapture.Internal;
public unsafe static partial class DirectX
{
    const int D3D11_SDK_VERSION = 7;

    [LibraryImport("d3d11")]
    public static partial HResult D3D11CreateDevice(
        IDXGIAdapter* adapter = null,
        DriverType driverType = DriverType.Null,
        nint software = 0,
        uint flags = 0,
        FeatureLevel* featureLevels = null,
        uint featureLevelsCount = 0,
        uint sdkVersion = D3D11_SDK_VERSION,
        ID3D11Device* device = null,
        FeatureLevel* featureLevel = null,
        ID3D11DeviceContext* context = null
    );

    [LibraryImport("dxgi")]
    public static partial HResult CreateDXGIFactory1(IID iid, IDXGIFactory1* factory);

    public static HResult CreateDXGIFactory1(IDXGIFactory1* factory) => CreateDXGIFactory1(IDXGIFactory1.IID, factory);
}

public unsafe static partial class Extensions
{
    public static T Cast<T>(this IIUnknown self) where T : IIUnknown => *(T*)(*(nint**)&self + 1);
}

public unsafe interface IIUnknown
{
    public static abstract IID IID { get; }

    nint* VirtualTable
    {
        get
        {
            var unknown = AsUnknown;
            return **(nint***)&unknown;
        }
    }
    IUnknown AsUnknown => this.Cast<IUnknown>();
    nint this[int index] => VirtualTable[index];
}
[StructLayout(Sequential, Size = 8)]
public unsafe struct IUnknown : IIUnknown
{
    static IID iid = "0000000000000000C000000000000046"u8;
    public static IID IID => iid;
}
unsafe partial class Extensions
{
    public static HResult QueryInterface(this IIUnknown self, IID iid, void* obj) => ((delegate* unmanaged<IUnknown, IID*, void*, HResult>)self[0])(self.AsUnknown, &iid, obj);
    public static HResult QueryInterface<T>(this IIUnknown self, void* obj) where T : IIUnknown => self.QueryInterface(T.IID, obj);
    public static HResult Release(this IIUnknown self)  => ((delegate* unmanaged<IUnknown, HResult>)self[2])(self.AsUnknown);
}

public interface IIDXGIObject : IIUnknown;
[StructLayout(Sequential, Size = 8)]
public struct IDXGIObject : IIDXGIObject
{
    static IID iid = "2411e7e112ac4ccfbd149798e8534dc0"u8;
    public static IID IID => iid;
}

public unsafe interface IIDXGIAdapter : IIDXGIObject
{
    IDXGIAdapter AsAdapter => this.Cast<IDXGIAdapter>();
}
[StructLayout(Sequential, Size = 8)]
public struct IDXGIAdapter : IIDXGIAdapter 
{
    static IID iid = "aec22fb876f346399be028eb43a67a2e"u8;
    public static IID IID => iid;
}
unsafe partial class Extensions
{
    public static HResult EnumOutputs(this IIDXGIAdapter self, uint outputIndex, IDXGIOutput* output) 
        => ((delegate* unmanaged<IDXGIAdapter, uint, IDXGIOutput*, HResult>)self[7])(self.AsAdapter, outputIndex, output);
}

public interface IID3D11Device : IIUnknown
{
    ID3D11Device AsDevice => this.Cast<ID3D11Device>();
}
[StructLayout(Sequential, Size = 8)]
public struct ID3D11Device : IID3D11Device 
{
    static IID iid = "db6f6ddbac774e888253819df9bbf140"u8;
    public static IID IID => iid;
}
unsafe partial class Extensions
{
    public static HResult CreateTexture2D(this IID3D11Device self, Texture2DDescription* description, ID3D11Texture2D* texture) 
        => CreateTexture2D(self.AsDevice, description, (SubresourceData*)null, texture);

    public static HResult CreateTexture2D(this IID3D11Device self, Texture2DDescription* description, SubresourceData* data, ID3D11Texture2D* texture)
        => ((delegate* unmanaged<ID3D11Device, Texture2DDescription*, SubresourceData*, ID3D11Texture2D*, HResult>)self[5])(self.AsDevice, description, data, texture);
}

public interface IID3D11DeviceChild : IIUnknown;
[StructLayout(Sequential, Size = 8)]
public struct ID3D11DeviceChild : IID3D11DeviceChild
{
    static IID iid = "1841e5c816b0489bbcc844cfb0d5deae"u8;
    public static IID IID => iid;
}

public interface IID3D11DeviceContext : IID3D11DeviceChild
{
    ID3D11DeviceContext AsDeviceContext => this.Cast<ID3D11DeviceContext>();
}
[StructLayout(Sequential, Size = 8)]
public struct ID3D11DeviceContext : IID3D11DeviceContext
{
    static IID iid = "c0bfa96ce08944fb8eaf26f8796190da"u8;
    public static IID IID => iid;
}
unsafe partial class Extensions
{
    public static HResult CopyResource(this IID3D11DeviceContext self, IIDXGIResource source, IIDXGIResource destination)
        => self.CopyResource(source.AsResource, destination.AsResource);

    public static HResult CopyResource(this IID3D11DeviceContext self, IDXGIResource source, IDXGIResource destination)
        => ((delegate* unmanaged<ID3D11DeviceContext, IDXGIResource, IDXGIResource, HResult>)self[47])(self.AsDeviceContext, destination, source);

    public static HResult Map(this IID3D11DeviceContext self, IIDXGIResource resource, uint subresource, MapType mapType, MapFlags mapFlags, SubresourceData* data)
        => self.Map(resource.AsResource, subresource, mapType, mapFlags, data);

    public static HResult Map(this IID3D11DeviceContext self, IDXGIResource resource, uint subresource, MapType mapType, MapFlags mapFlags, SubresourceData* data)
        => ((delegate* unmanaged<ID3D11DeviceContext, IDXGIResource, uint, MapType, MapFlags, SubresourceData*, HResult>)self[14])
           (self.AsDeviceContext, resource, subresource, mapType, mapFlags, data);

    public static HResult Unmap(this IID3D11DeviceContext self, IIDXGIResource resource, uint subresource)
        => Unmap(self, resource.AsResource, subresource);

    public static HResult Unmap(this IID3D11DeviceContext self, IDXGIResource resource, uint subresource)
        => ((delegate* unmanaged<ID3D11DeviceContext, IDXGIResource, uint, HResult>)self[15])(self.AsDeviceContext, resource, subresource);
}

public interface IIDXGIOutput : IIDXGIObject
{
    IDXGIOutput AsOutput => this.Cast<IDXGIOutput>();
}
[StructLayout(Sequential, Size = 8)]
public struct IDXGIOutput : IIDXGIOutput
{
    static IID iid = "ae02eedbc73546908d525a8dc20213aa"u8;
    public static IID IID => iid;
}
unsafe partial class Extensions
{
    public static HResult GetDescription(this IIDXGIOutput self, OutputDescription* description) 
        => ((delegate* unmanaged<IDXGIOutput, OutputDescription*, HResult>)self[7])(self.AsOutput, description);
}

public interface IIDXGIOutput1 : IIDXGIOutput 
{
    IDXGIOutput1 AsOutput1 => this.Cast<IDXGIOutput1>();
}
[StructLayout(Sequential, Size = 8)]
public struct IDXGIOutput1 : IIDXGIOutput1
{
    static IID iid = "00cddea8939b4b83a340a685226666cc"u8;
    public static IID IID => iid;
}
unsafe partial class Extensions
{
    public static HResult DuplicateOutput(this IIDXGIOutput1 self, ID3D11Device device, IDXGIOutputDuplication* duplicator) 
        => ((delegate* unmanaged<IDXGIOutput1, ID3D11Device, IDXGIOutputDuplication*, HResult>)self[22])(self.AsOutput1, device, duplicator);
}

public unsafe interface IIDXGIFactory : IIDXGIObject
{
    IDXGIFactory AsFactory => this.Cast<IDXGIFactory>();
}
[StructLayout(Sequential, Size = 8)]
public struct IDXGIFactory : IIDXGIFactory
{
    static IID iid = "7b7166ec21c744aeb21ac9ae321ae369"u8;
    public static IID IID => iid;
}
unsafe partial class Extensions
{
    public static HResult EnumAdapters(this IIDXGIFactory self, uint adapterIndex, IDXGIAdapter* adapter) 
        => ((delegate* unmanaged<IDXGIFactory, uint, IDXGIAdapter*, HResult>)self[7])(self.AsFactory, adapterIndex, adapter);
}

public interface IIDXGIFactory1 : IIDXGIFactory;
[StructLayout(Sequential, Size = 8)]
public struct IDXGIFactory1 : IIDXGIFactory1
{
    static IID iid = "770aae78f26f4dbaa829253c83d1b387"u8;
    public static IID IID => iid;
}
public interface IIDXGIOutputDuplication : IIDXGIObject
{
    IDXGIOutputDuplication AsOutputDuplication => this.Cast<IDXGIOutputDuplication>();
}
[StructLayout(Sequential, Size = 8)]
public struct IDXGIOutputDuplication : IIDXGIOutputDuplication
{
    static IID iid = "191cfac3a341470db26ea864f428319c"u8;
    public static IID IID => iid;
}
unsafe partial class Extensions
{
    public static HResult AcquireNextFrame(this IIDXGIOutputDuplication self, uint timeout, OutDuplFrameInfo* frameInfo, IDXGIResource* resource)
        => ((delegate* unmanaged<IDXGIOutputDuplication, uint, OutDuplFrameInfo*, IDXGIResource*, HResult>)self[8])(self.AsOutputDuplication, timeout, frameInfo, resource);

    public static HResult ReleaseFrame(this IIDXGIOutputDuplication self) 
        => ((delegate* unmanaged<IDXGIOutputDuplication, HResult>)self[14])(self.AsOutputDuplication);
}

public interface IIDXGIDeviceSubObject : IIDXGIObject;
[StructLayout(Sequential, Size = 8)]
public struct IDXGIDeviceSubObject : IIDXGIDeviceSubObject
{
    static IID iid = "3d3e0379f9de4d58bb6c18d62992f1a6"u8;
    public static IID IID => iid;
}
public interface IIDXGIResource : IIDXGIDeviceSubObject
{
    IDXGIResource AsResource => this.Cast<IDXGIResource>();
}
[StructLayout(Sequential, Size = 8)]
public struct IDXGIResource : IIDXGIResource
{
    static IID iid = "035f3ab4482e4e50b41f8a7f8bd8960b"u8;
    public static IID IID => iid;
}

public interface IID3D11Texture2D : IIDXGIResource;
[StructLayout(Sequential, Size = 8)]
public struct ID3D11Texture2D : IID3D11Texture2D
{
    static IID iid = "6f15aaf2d2084e899ab4489535d34f9c"u8;
    public static IID IID => iid;
}