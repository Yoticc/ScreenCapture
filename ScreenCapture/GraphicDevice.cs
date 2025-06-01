using ScreenCapture.DirectXModels;
using ScreenCapture.Internal;

namespace ScreenCapture;
public unsafe struct GraphicDevice
{
    public ID3D11Device Device;
    public ID3D11DeviceContext Context;
    public FeatureLevel FeatureLevel;
    public IDXGIFactory1 Factory;
    public IDXGIAdapter Adapter;

    public void Release()
    {
        Device.Release();
        Context.Release();
        Factory.Release();
        Adapter.Release();
    }

    public static HResult EnumDevice(GraphicDevice* device, uint index = 0)
    {
        HResult result;
        
        _ = (result = DirectX.D3D11CreateDevice(driverType: DriverType.Hardware, featureLevel: &device->FeatureLevel, context: &device->Context, device: &device->Device)) &&
            (result = DirectX.CreateDXGIFactory1(&device->Factory)) &&
            (result = device->Factory.EnumAdapters(index, &device->Adapter));

        return result;
    }
}

public unsafe struct Screen
{
    public IDXGIOutput Output0;
    public IDXGIOutput1 Output;
    public OutputDescription Descriptor;

    public void Release()
    {
        Output0.Release();
        Output.Release();
    }

    public static HResult EnumScreen(Screen* screen, GraphicDevice* device, uint index = 0)
    {
        HResult result;
       
        _ = (result = device->Adapter.EnumOutputs(index, &screen->Output0)) &&
            (result = screen->Output0.QueryInterface<IDXGIOutput>(&screen->Output)) &&
            (result = screen->Output.GetDescription(&screen->Descriptor));

        return result;
    }
}