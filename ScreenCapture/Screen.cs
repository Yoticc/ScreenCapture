using ScreenCapture.DirectXModels;
using ScreenCapture.Internal;

namespace ScreenCapture;
public unsafe class Screen : IDisposable
{
    public Screen(GraphicDevice device, uint index = 0)
    {
        ScreenID = index;
        Device = device;

        Initialize();
        InitializeDuplicator();
    }

    public readonly uint ScreenID;
    public readonly GraphicDevice Device;
    bool initialized;

    public uint FrameWaitInterval = 1000;

    IDXGIOutput Output0;
    ID3D11Texture2D texture;
    Texture2DDescription textureDescription;
    public IDXGIOutput1 Output { get; private set; }
    public IDXGIOutputDuplication Duplicator { get; private set; }

    void Initialize()
    {
        IDXGIOutput output;
        Device.Adapter.EnumOutputs(ScreenID, &output).CheckResult();
        Output0 = output;

        IDXGIOutput1 output1;
        Output0.QueryInterface<IDXGIOutput1>(&output1).CheckResult();
        Output = output1;

        OutputDescription outputDescription;
        Output.GetDescription(&outputDescription).CheckResult();
        var bounds = outputDescription.DesktopBounds;

        var textureDescription = new Texture2DDescription
        {
            CpuAccessFlags = CpuAccessFlags.ReadWrite,
            BindFlags = BindFlags.None,
            Format = FormatType.B8G8R8A8_UNorm,
            Width = bounds.Width,
            Height = bounds.Height,
            ResourceOptionFlags = ResourceOptionFlags.None,
            MipLevels = 1,
            ArraySize = 1,
            SampleDescription = new(1, 0),
            Usage = Usage.Staging
        };

        ID3D11Texture2D texture;
        Device.Device.CreateTexture2D(&textureDescription, &texture).CheckResult();
        this.texture = texture;
        this.textureDescription = textureDescription;
    }

    void InitializeDuplicator()
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

        DisposePreviousFrame();

        OutDuplFrameInfo frame;
        IDXGIResource frameResource;
        var result = Duplicator.AcquireNextFrame(FrameWaitInterval, &frame, &frameResource);
        if (result)
        {
            hasPreviousFrame = true;
            if (frame.LastPresentTime != 0 && lastPresentTime != frame.LastPresentTime)
            {
                lastPresentTime = frame.LastPresentTime;

                ID3D11Texture2D frameTexture;
                result = frameResource.QueryInterface<ID3D11Texture2D>(&frameTexture);
                if (result)
                {
                    Device.Context.CopyResource(frameTexture, texture).CheckResult();
                    SubresourceData subresource;
                    Device.Context.Map(texture, 0, MapType.Read, MapFlags.None, &subresource).CheckResult();

                    var bitmap = new TextureMemoryBitmap((TexturePixel*)subresource.Data, (int)textureDescription.Width, (int)textureDescription.Height);

                    outputFrame = new(frame, bitmap);
                    return true;
                }
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
                InitializeDuplicator();
            }
            else Console.WriteLine($"ScreenCapture->Screen: Failed to acquire frame, result: {result}");
        }

        outputFrame = null!;
        return false;
    }

    #region IDispose
    void DisposePreviousFrame()
    {
        if (!hasPreviousFrame)
            return;

        hasPreviousFrame = false;
        Device.Context.Unmap(texture, 0).CheckResult();

        Duplicator.ReleaseFrame().CheckResult();
    }

    bool disposed;
    public void Dispose()
    {
        if (disposed)
            return;
        disposed = true;

        DisposePreviousFrame();
        
        Output.Release();
        texture.Release();

        if (initialized)
        {
            initialized = false;
            Duplicator.Release();
        }
    }

    ~Screen() => Dispose();
    #endregion
}