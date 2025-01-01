using ScreenCapture.Internal;

Thread.Sleep(5000);

unsafe
{
    ID3D11Device device;
    ID3D11DeviceContext context;
    D3DFeatureLevel featureLevel;

    DirectX.D3D11CreateDevice(
        driverType: D3DDriverType.Hardware,        
        featureLevel: &featureLevel,
        context: &context,
        device: &device
    ).CheckResult();

    IDXGIFactory1 factory;
    DirectX.CreateDXGIFactory1(GUID.IDXGIFactory1, &factory).CheckResult();

    IDXGIAdapter adapter;
    factory.EnumAdapters(0, &adapter).CheckResult();

    IDXGIOutput output;
    adapter.EnumOutputs(0, &output).CheckResult();

    IDXGIOutput1 output1;
    output.QueryInterface(GUID.IDXGIOutput1, &output1).CheckResult();

    IDXGIOutputDuplication duplicator;
    output1.DuplicateOutput(device, &duplicator).CheckResult();

    long lastPresentTime = 0;
    while (true)
    {
        const uint DXGI_ERROR_ACCESS_LOST = 0x887A0026;
        const uint DXGI_ERROR_WAIT_TIMEOUT = 0x887A0027;

        OutDuplFrameInfo frame;
        IDXGIResource resource;

        var result = duplicator.AcquireNextFrame(0, &frame, &resource);
        if (result)
        {
            if (lastPresentTime != frame.LastPresentTime)
            {
                lastPresentTime = frame.LastPresentTime;

                Console.WriteLine($"Success. time: {lastPresentTime}");
            }

            duplicator.ReleaseFrame().CheckResult();
        }
        else
        {
            if (result.Code == DXGI_ERROR_WAIT_TIMEOUT)
            {
                // pass through
            }
            else if (result.Code == DXGI_ERROR_ACCESS_LOST)
                Console.WriteLine("Lost access");
            else Console.WriteLine($"Failed to acquire frame, result: {result}");
        }

        Thread.Sleep(50);
    }
}