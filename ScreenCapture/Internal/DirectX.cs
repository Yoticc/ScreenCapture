using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
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
        D3DDriverType driverType = D3DDriverType.Null,
        nint software = 0,
        uint flags = 0,
        D3DFeatureLevel* featureLevels = null,
        uint featureLevelsCount = 0,
        uint sdkVersion = D3D11_SDK_VERSION,
        ID3D11Device* device = null,
        D3DFeatureLevel* featureLevel = null,
        ID3D11DeviceContext* context = null
    );

    [DllImport(dxgi)]
    public static extern HResult CreateDXGIFactory1(IID iid, IDXGIFactory1* factory);    
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
        IDXGIResource = "035f3ab4-482e-4e50-b41f-8a7f8bd8960b";
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
public unsafe struct IUnknown : IIUnknown
{
    public nint* InternalIUnknown;
}
unsafe partial class Extensions
{
    public static HResult QueryInterface(this IIUnknown self, IID iid, void* obj) 
        => ((delegate* unmanaged[Stdcall]<IUnknown, IID*, void*, HResult>)self[0])(self.AsUnknown, &iid, obj);
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
        => ((delegate* unmanaged[Stdcall]<IDXGIAdapter, uint, IDXGIOutput*, HResult>)self[7])(self.AsAdapter, outputIndex, output);
}

public interface IID3D11Device : IIUnknown;
[StructLayout(Sequential, Size = 8)]
public struct ID3D11Device : IID3D11Device;

public interface IID3D11DeviceChild : IIUnknown;
[StructLayout(Sequential, Size = 8)]
public struct ID3D11DeviceChild : IID3D11DeviceChild;

public interface IID3D11DeviceContext : IID3D11DeviceChild;
[StructLayout(Sequential, Size = 8)]
public struct ID3D11DeviceContext : IID3D11DeviceContext;

public interface IIDXGIOutput : IIDXGIObject;
[StructLayout(Sequential, Size = 8)]
public struct IDXGIOutput : IIDXGIOutput;

public interface IIDXGIOutput1 : IIDXGIOutput 
{
    IDXGIOutput1 AsOutput1 => this.Cast<IDXGIOutput1>();
}
[StructLayout(Sequential, Size = 8)]
public struct IDXGIOutput1 : IIDXGIOutput1;
unsafe partial class Extensions
{
    public static HResult DuplicateOutput(this IIDXGIOutput1 self, ID3D11Device device, IDXGIOutputDuplication* duplicator) 
        => ((delegate* unmanaged[Stdcall]<IDXGIOutput1, ID3D11Device, IDXGIOutputDuplication*, HResult>)self[22])(self.AsOutput1, device, duplicator);
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
        => ((delegate* unmanaged[Stdcall]<IDXGIFactory, uint, IDXGIAdapter*, HResult>)self[7])(self.AsFactory, adapterIndex, adapter);
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
        => ((delegate* unmanaged[Stdcall]<IDXGIOutputDuplication, uint, OutDuplFrameInfo*, IDXGIResource*, HResult>)self[8])(self.AsOutputDuplication, timeout, frameInfo, resource);

    public static HResult ReleaseFrame(this IIDXGIOutputDuplication self) 
        => ((delegate* unmanaged[Stdcall]<IDXGIOutputDuplication, HResult>)self[14])(self.AsOutputDuplication);
}

public interface IIDXGIDeviceSubObject : IIDXGIObject;
[StructLayout(Sequential, Size = 8)]
public struct IDXGIDeviceSubObject : IIDXGIDeviceSubObject;

public interface IIDXGIResource : IIDXGIDeviceSubObject;
[StructLayout(Sequential, Size = 8)]
public struct IDXGIResource : IIDXGIResource
{
    
}

public enum D3DDriverType
{
    Unknown,
    Hardware,
    Reference,
    Null,
    Software,
    Warp,
}

public enum D3DFeatureLevel
{
    Level1_0_Generic = 0x100,
    Level1_0_Core = 0x1000,
    Level9_1 = 0x9100,
    Level9_2 = 0x9200,
    Level9_3 = 0x9300,
    Level10_0 = 0xa000,
    Level10_1 = 0xa100,
    Level11_0 = 0xb000,
    Level11_1 = 0xb100,
    Level12_0 = 0xc000,
    Level12_1 = 0xc100,
    Level12_2 = 0xc200
}