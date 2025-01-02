using ScreenCapture.Internal;

namespace ScreenCapture;
public unsafe class Screen : IDisposable
{
    public Screen(GraphicDevice device, uint index = 0)
    {
        ScreenID = index;
        Device = device;

        IDXGIOutput output;
        Device.Adapter.EnumOutputs(index, &output).CheckResult();
        Output0 = output;

        IDXGIOutput1 output1;
        Output0.QueryInterface<IDXGIOutput1>(&output1).CheckResult();
        Output = output1;

        Initialize();
    }

    public readonly uint ScreenID;
    public readonly GraphicDevice Device;
    readonly IDXGIOutput Output0;
    public readonly IDXGIOutput1 Output;

    public uint FrameWaitInterval = 1000;

    bool initialized;
    public IDXGIOutputDuplication Duplicator { get; private set; }

    public void Initialize()
    {
        if (initialized)
        {
            if (hasPreviousFrame)
            {
                hasPreviousFrame = false;
                Duplicator.ReleaseFrame().CheckResult();
            }

            Duplicator.Release();
        }
        initialized = true;

        IDXGIOutputDuplication duplicator;
        Output.DuplicateOutput(Device.Device, &duplicator).CheckResult();
        Duplicator = duplicator;
    }

    long lastPresentTime;
    bool hasPreviousFrame;
    public bool CaptureFrame(out Frame outputFrame)
    {
        const uint DXGI_ERROR_ACCESS_LOST = 0x887A0026;
        const uint DXGI_ERROR_WAIT_TIMEOUT = 0x887A0027;

        if (hasPreviousFrame)
        {
            hasPreviousFrame = false;
            Duplicator.ReleaseFrame().CheckResult();
        }

        OutDuplFrameInfo frame;
        IDXGIResource resource;
        var result = Duplicator.AcquireNextFrame(FrameWaitInterval, &frame, &resource);
        if (result)
        {
            hasPreviousFrame = true;
            if (frame.LastPresentTime != 0 && lastPresentTime != frame.LastPresentTime)
            {
                lastPresentTime = frame.LastPresentTime;

                outputFrame = new(frame);
                return true;
            }
        }
        else
        {
            if (result == DXGI_ERROR_WAIT_TIMEOUT)
            {
                // pass through
            }
            else if (result == DXGI_ERROR_ACCESS_LOST)
            {
                // lost access
                Initialize();
            }
            else Console.WriteLine($"Failed to acquire frame, result: {result}");
        }

        outputFrame = null!;
        return false;
    }

    #region IDispose
    bool disposed;
    public void Dispose()
    {
        if (disposed)
            return;
        disposed = true;

        Output.Release();

        if (initialized)
        {
            initialized = false;
            Duplicator.Release();
        }
    }

    ~Screen() => Dispose();
    #endregion
}