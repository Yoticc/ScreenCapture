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
        DXGIAdapter* adapter = null,
        DriverType driverType = DriverType.Null,
        nint software = 0,
        uint flags = 0,
        FeatureLevel* featureLevels = null,
        uint featureLevelsCount = 0,
        uint sdkVersion = D3D11_SDK_VERSION,
        D3D11Device* device = null,
        FeatureLevel* featureLevel = null,
        D3D11DeviceContext* context = null
    );

    [LibraryImport("dxgi")]
    public static partial HResult CreateDXGIFactory1(IID iid, DXGIFactory1* factory);

    public static HResult CreateDXGIFactory1(DXGIFactory1* factory) => CreateDXGIFactory1(DXGIFactory1.IID, factory);
}

public unsafe static class Extensions
{
    public static T As<T>(this IUnknown.Interface self) => *(*(T**)&self + 1);

    public static HResult QueryInterface(this IUnknown.Interface self, IID iid, void* obj) 
        => ((delegate* unmanaged<IUnknown, IID*, void*, HResult>)self[0])(self.As<IUnknown>(), &iid, obj);

    public static HResult QueryInterface<T>(this IUnknown.Interface self, void* obj) where T : IUnknown.Interface
        => self.QueryInterface(T.IID, obj);

    public static HResult Release(this IUnknown.Interface self) 
        => ((delegate* unmanaged<IUnknown, HResult>)self[2])(self.As<IUnknown>());

    public static HResult EnumOutputs(this DXGIAdapter.Interface self, uint outputIndex, DXGIOutput* output)
        => ((delegate* unmanaged<DXGIAdapter, uint, DXGIOutput*, HResult>)self[7])(self.As<DXGIAdapter>(), outputIndex, output);

    public static HResult CreateTexture2D(this D3D11Device.Interface self, Texture2DDescription* description, SubresourceData* data, D3D11Texture2D* texture)
        => ((delegate* unmanaged<D3D11Device, Texture2DDescription*, SubresourceData*, D3D11Texture2D*, HResult>)self[5])(self.As<D3D11Device>(), description, data, texture);

    public static HResult CopyResource(this D3D11DeviceContext.Interface self, DXGIResource.Interface source, DXGIResource.Interface destination)
        => ((delegate* unmanaged<D3D11DeviceContext, DXGIResource, DXGIResource, HResult>)self[47])(self.As<D3D11DeviceContext>(), destination.As<DXGIResource>(), source.As<DXGIResource>());

    public static HResult Map(this D3D11DeviceContext.Interface self, DXGIResource.Interface resource, uint subresource, MapType mapType, MapFlags mapFlags, SubresourceData* data)
        => ((delegate* unmanaged<D3D11DeviceContext, DXGIResource, uint, MapType, MapFlags, SubresourceData*, HResult>)self[14])
        (self.As<D3D11DeviceContext>(), resource.As<DXGIResource>(), subresource, mapType, mapFlags, data);

    public static HResult Unmap(this D3D11DeviceContext.Interface self, DXGIResource.Interface resource, uint subresource)
        => ((delegate* unmanaged<D3D11DeviceContext, DXGIResource, uint, HResult>)self[15])(self.As<D3D11DeviceContext>(), resource.As<DXGIResource>(), subresource);

    public static HResult GetDescription(this DXGIOutput.Interface self, OutputDescription* description)
        => ((delegate* unmanaged<DXGIOutput, OutputDescription*, HResult>)self[7])(self.As<DXGIOutput>(), description);

    public static HResult DuplicateOutput(this DXGIOutput.Interface self, D3D11Device device, DXGIOutputDuplication* duplicator)
        => ((delegate* unmanaged<DXGIOutput1, D3D11Device, DXGIOutputDuplication*, HResult>)self[22])(self.As<DXGIOutput1>(), device, duplicator);

    public static HResult EnumAdapters(this DXGIFactory.Interface self, uint adapterIndex, DXGIAdapter* adapter)
        => ((delegate* unmanaged<DXGIFactory, uint, DXGIAdapter*, HResult>)self[7])(self.As<DXGIFactory>(), adapterIndex, adapter);

    public static HResult AcquireNextFrame(this DXGIOutputDuplication.Interface self, uint timeout, OutDuplFrameInfo* frameInfo, DXGIResource* resource)
        => ((delegate* unmanaged<DXGIOutputDuplication, uint, OutDuplFrameInfo*, DXGIResource*, HResult>)self[8])(self.As<DXGIOutputDuplication>(), timeout, frameInfo, resource);

    public static HResult ReleaseFrame(this DXGIOutputDuplication.Interface self)
        => ((delegate* unmanaged<DXGIOutputDuplication, HResult>)self[14])(self.As<DXGIOutputDuplication>());
}

[StructLayout(Sequential, Size = 8)]
public unsafe struct IUnknown : IUnknown.Interface
{ 
    public static IID IID { get; private set; } = "0000000000000000c000000000000046"u8; 
    public interface Interface
    {
        public static abstract IID IID { get; }

        void** VirtualTable => *(void***)this.As<nint>();

        void* this[int index] => VirtualTable[index];
    }
}
[StructLayout(Sequential, Size = 8)] 
public struct DXGIOutputDuplication : DXGIOutputDuplication.Interface
{ 
    public static IID IID { get; private set; } = "191cfac3a341470db26ea864f428319c"u8;
    public interface Interface : DXGIObject.Interface;
}

[StructLayout(Sequential, Size = 8)]
public struct DXGIDeviceSubObject : DXGIDeviceSubObject.Interface
{ 
    public static IID IID { get; private set; } = "3d3e0379f9de4d58bb6c18d62992f1a6"u8;
    public interface Interface : DXGIObject.Interface; 
}

[StructLayout(Sequential, Size = 8)] 
public struct D3D11DeviceContext : D3D11DeviceContext.Interface
{ 
    public static IID IID { get; private set; } = "c0bfa96ce08944fb8eaf26f8796190da"u8;
    public interface Interface : D3D11DeviceChild.Interface; 
}

[StructLayout(Sequential, Size = 8)] 
public struct D3D11DeviceChild : D3D11DeviceChild.Interface
{ 
    public static IID IID { get; private set; } = "1841e5c816b0489bbcc844cfb0d5deae"u8; 
    public interface Interface : IUnknown.Interface; 
}

[StructLayout(Sequential, Size = 8)] 
public struct D3D11Texture2D : D3D11Texture2D.Interface
{ 
    public static IID IID { get; private set; } = "6f15aaf2d2084e899ab4489535d34f9c"u8; 
    public interface Interface : DXGIResource.Interface;
}

[StructLayout(Sequential, Size = 8)] 
public struct DXGIResource : DXGIResource.Interface
{ 
    public static IID IID { get; private set; } = "035f3ab4482e4e50b41f8a7f8bd8960b"u8; 
    public interface Interface : DXGIDeviceSubObject.Interface; 
}

[StructLayout(Sequential, Size = 8)] 
public struct DXGIFactory1 : DXGIFactory1.Interface
{ 
    public static IID IID { get; private set; } = "770aae78f26f4dbaa829253c83d1b387"u8; 
    public interface Interface : DXGIFactory.Interface; 
}

[StructLayout(Sequential, Size = 8)] 
public struct DXGIAdapter : DXGIAdapter.Interface
{ 
    public static IID IID { get; private set; } = "aec22fb876f346399be028eb43a67a2e"u8; 
    public interface Interface : DXGIObject.Interface; 
}

[StructLayout(Sequential, Size = 8)] 
public struct D3D11Device : D3D11Device.Interface
{ 
    public static IID IID { get; private set; } = "db6f6ddbac774e888253819df9bbf140"u8; 
    public interface Interface : IUnknown.Interface; 
}

[StructLayout(Sequential, Size = 8)] 
public struct DXGIOutput1 : DXGIOutput1.Interface
{ 
    public static IID IID { get; private set; } = "00cddea8939b4b83a340a685226666cc"u8; 
    public interface Interface : DXGIOutput.Interface; 
}

[StructLayout(Sequential, Size = 8)] 
public struct DXGIFactory : DXGIFactory.Interface
{ 
    public static IID IID { get; private set; } = "7b7166ec21c744aeb21ac9ae321ae369"u8; 
    public interface Interface : DXGIObject.Interface; 
}

[StructLayout(Sequential, Size = 8)] 
public struct DXGIObject : DXGIObject.Interface
{ 
    public static IID IID { get; private set; } = "2411e7e112ac4ccfbd149798e8534dc0"u8; 
    public interface Interface : IUnknown.Interface; 
}

[StructLayout(Sequential, Size = 8)]
public struct DXGIOutput : DXGIOutput.Interface
{ 
    public static IID IID { get; private set; } = "ae02eedbc73546908d525a8dc20213aa"u8;
    public interface Interface : DXGIObject.Interface; 
}