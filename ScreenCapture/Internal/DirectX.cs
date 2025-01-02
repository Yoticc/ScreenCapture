using ScreenCapture.DirectXModels;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.LayoutKind;

namespace ScreenCapture.Internal;
public unsafe static class DirectX
{
    const string dxgi = "dxgi";
    const string d3d11 = "d3d11";

    const int D3D11_SDK_VERSION = 7;

    [DllImport(d3d11)]
    public static extern HResult D3D11CreateDevice(
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

    [DllImport(dxgi)]
    public static extern HResult CreateDXGIFactory1(IID iid, IDXGIFactory1* factory);

    public static HResult CreateDXGIFactory1(IDXGIFactory1* factory) => CreateDXGIFactory1(GUID.IDXGIFactory1, factory);
}

public static class GUID
{
    public static readonly IID
        IUnknown = "00000000-0000-0000-C000-000000000046",
        IDXGIObject = "aec22fb8-76f3-4639-9be0-28eb43a67a2e",
        IDXGIAdapter = "2411e7e1-12ac-4ccf-bd14-9798e8534dc0",
        ID3D11Device = "db6f6ddb-ac77-4e88-8253-819df9bbf140",
        ID3D11DeviceContext = "c0bfa96c-e089-44fb-8eaf-26f8796190da",
        IDXGIOutput = "ae02eedb-c735-4690-8d52-5a8dc20213aa",
        IDXGIOutput1 = "00cddea8-939b-4b83-a340-a685226666cc",
        IDXGIFactory = "7b7166ec-21c7-44ae-b21a-c9ae321ae369",
        IDXGIFactory1 = "770aae78-f26f-4dba-a829-253c83d1b387",
        IDXGIOutputDuplication = "191cfac3-a341-470d-b26e-a864f428319c",
        IDXGIDeviceSubObject = "3d3e0379-f9de-4d58-bb6c-18d62992f1a6",
        IDXGIResource = "035f3ab4-482e-4e50-b41f-8a7f8bd8960b",
        ID3D11Texture2D = "6f15aaf2-d208-4e89-9ab4-489535d34f9c";

    public static readonly Dictionary<Type, IID> TypeToIIDDictionary = 
        typeof(GUID)
        .GetFields()
        .Where(field => field.FieldType == typeof(IID))
        .ToDictionary(field => Type.GetType($"ScreenCapture.Internal.{field.Name}")!, field => (IID)field.GetValue(null)!);
}

public unsafe static partial class Extensions
{
    public static T Cast<T>(this IIUnknown self) where T : IIUnknown => *(T*)((nint*)*(nint*)&self + 1);
}

public unsafe interface IIUnknown
{
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
public unsafe struct IUnknown : IIUnknown, IDisposable
{
    public nint* InternalIUnknown;

    public void Dispose() => this.Release();
}
unsafe partial class Extensions
{
    public static HResult QueryInterface(this IIUnknown self, IID iid, void* obj)
        => ((delegate* unmanaged<IUnknown, IID*, void*, HResult>)self[0])(self.AsUnknown, &iid, obj);

    public static HResult QueryInterface<T>(this IIUnknown self, void* obj) where T : IIUnknown
        => self.QueryInterface(GUID.TypeToIIDDictionary[typeof(T)], obj);

    public static HResult Release(this IIUnknown self)
        => ((delegate* unmanaged<IUnknown, HResult>)self[2])(self.AsUnknown);
}

public interface IIDXGIObject : IIUnknown;
[StructLayout(Sequential, Size = 8)]
public struct IDXGIObject : IIDXGIObject;

public unsafe interface IIDXGIAdapter : IIDXGIObject
{
    IDXGIAdapter AsAdapter => this.Cast<IDXGIAdapter>();
}
[StructLayout(Sequential, Size = 8)]
public struct IDXGIAdapter : IIDXGIAdapter { }
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
public struct ID3D11Device : IID3D11Device;
unsafe partial class Extensions
{
    public static HResult CreateTexture2D(this IID3D11Device self, Texture2DDescription* description, ID3D11Texture2D* texture) 
        => CreateTexture2D(self.AsDevice, description, (SubresourceData*)null, texture);

    public static HResult CreateTexture2D(this IID3D11Device self, Texture2DDescription* description, SubresourceData[] data, ID3D11Texture2D* texture)
    {
        fixed (SubresourceData* dataPointer = data)
            return CreateTexture2D(self.AsDevice, description, dataPointer, texture);
    }

    public static HResult CreateTexture2D(this IID3D11Device self, Texture2DDescription* description, SubresourceData* data, ID3D11Texture2D* texture)
        => ((delegate* unmanaged<ID3D11Device, Texture2DDescription*, SubresourceData*, ID3D11Texture2D*, HResult>)self[5])(self.AsDevice, description, data, texture);
}

public interface IID3D11DeviceChild : IIUnknown;
[StructLayout(Sequential, Size = 8)]
public struct ID3D11DeviceChild : IID3D11DeviceChild;

public interface IID3D11DeviceContext : IID3D11DeviceChild
{
    ID3D11DeviceContext AsDeviceContext => this.Cast<ID3D11DeviceContext>();
}
[StructLayout(Sequential, Size = 8)]
public struct ID3D11DeviceContext : IID3D11DeviceContext;
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
public struct IDXGIOutput : IIDXGIOutput;
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
public struct IDXGIOutput1 : IIDXGIOutput1;
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
public struct IDXGIFactory : IIDXGIFactory;
unsafe partial class Extensions
{
    public static HResult EnumAdapters(this IIDXGIFactory self, uint adapterIndex, IDXGIAdapter* adapter) 
        => ((delegate* unmanaged<IDXGIFactory, uint, IDXGIAdapter*, HResult>)self[7])(self.AsFactory, adapterIndex, adapter);
}

public interface IIDXGIFactory1 : IIDXGIFactory;
[StructLayout(Sequential, Size = 8)]
public struct IDXGIFactory1 : IIDXGIFactory1;

public interface IIDXGIOutputDuplication : IIDXGIObject
{
    IDXGIOutputDuplication AsOutputDuplication => this.Cast<IDXGIOutputDuplication>();
}
[StructLayout(Sequential, Size = 8)]
public struct IDXGIOutputDuplication : IIDXGIOutputDuplication;
unsafe partial class Extensions
{
    public static HResult AcquireNextFrame(this IIDXGIOutputDuplication self, uint timeout, OutDuplFrameInfo* frameInfo, IDXGIResource* resource)
        => ((delegate* unmanaged<IDXGIOutputDuplication, uint, OutDuplFrameInfo*, IDXGIResource*, HResult>)self[8])(self.AsOutputDuplication, timeout, frameInfo, resource);

    public static HResult ReleaseFrame(this IIDXGIOutputDuplication self) 
        => ((delegate* unmanaged<IDXGIOutputDuplication, HResult>)self[14])(self.AsOutputDuplication);
}

public interface IIDXGIDeviceSubObject : IIDXGIObject;
[StructLayout(Sequential, Size = 8)]
public struct IDXGIDeviceSubObject : IIDXGIDeviceSubObject;

public interface IIDXGIResource : IIDXGIDeviceSubObject
{
    IDXGIResource AsResource => this.Cast<IDXGIResource>();
}
[StructLayout(Sequential, Size = 8)]
public struct IDXGIResource : IIDXGIResource;

public interface IID3D11Texture2D : IIDXGIResource;
[StructLayout(Sequential, Size = 8)]
public struct ID3D11Texture2D : IID3D11Texture2D;