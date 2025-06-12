using ScreenCapture.DirectXModels;
using ScreenCapture.Internal;

namespace ScreenCapture;
public unsafe struct GraphicDevice
{
    public D3D11Device Device;
    public D3D11DeviceContext Context;
    public FeatureLevel FeatureLevel;
    public DXGIFactory1 Factory;
    public DXGIAdapter Adapter;

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
    public DXGIOutput Output0;
    public DXGIOutput1 Output;
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
            (result = screen->Output0.QueryInterface<DXGIOutput>(&screen->Output)) &&
            (result = screen->Output.GetDescription(&screen->Descriptor));

        return result;
    }
}