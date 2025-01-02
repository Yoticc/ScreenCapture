using ScreenCapture.DirectXModels;
using ScreenCapture.Internal;

namespace ScreenCapture;
public unsafe class GraphicDevice : IDisposable
{
    public GraphicDevice(uint index = 0)
    {
        DeviceID = index;

        ID3D11Device device;
        ID3D11DeviceContext context;
        FeatureLevel featureLevel;

        DirectX.D3D11CreateDevice(
            driverType: DriverType.Hardware,
            featureLevel: &featureLevel,
            context: &context,
            device: &device
        ).CheckResult();
        Device = device;
        Context = context;
        FeatureLevel = featureLevel;

        IDXGIFactory1 factory;
        DirectX.CreateDXGIFactory1(&factory).CheckResult();
        Factory = factory;

        IDXGIAdapter adapter;
        Factory.EnumAdapters(index, &adapter).CheckResult();
        Adapter = adapter;
    }

    public readonly uint DeviceID;

    public readonly ID3D11Device Device;
    public readonly ID3D11DeviceContext Context;
    public readonly FeatureLevel FeatureLevel;
    public readonly IDXGIFactory1 Factory;
    public readonly IDXGIAdapter Adapter;

    bool disposed;
    public void Dispose()
    {
        if (disposed)
            return;
        disposed = true;

        Device.Release();
        Context.Release();
        Factory.Release();
        Adapter.Release();
    }

    ~GraphicDevice() => Dispose();
}