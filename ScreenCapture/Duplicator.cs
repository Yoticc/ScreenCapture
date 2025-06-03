using ScreenCapture.DirectXModels;
using ScreenCapture.Extensions;
using ScreenCapture.Internal;

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
namespace ScreenCapture;
public unsafe struct Duplicator
{
    ID3D11Device device;
    IDXGIOutput1 output;
    ID3D11Texture2D texture;
    ID3D11DeviceContext deviceContext;
    Texture2DDescription textureDescription;
    IDXGIOutputDuplication duplication;
    bool hasInitializedDuplication;
    bool hasInitializedFrame;

    long lastPresentTime;
    public HResult CaptureFrame(Frame* frame)
    {
        const int FrameWaitInterval = 1000;

        HResult result;
        if (result = EnsureDuplicationIsInitialized())
        {
            OutDuplFrameInfo frameInfo;
            IDXGIResource frameResource;

            if (result = ReleasePreviousFrame())
            {
                if (result = duplication.AcquireNextFrame(FrameWaitInterval, &frameInfo, &frameResource))
                {
                    hasInitializedFrame = true;
                    if (frameInfo.LastPresentTime != 0 && lastPresentTime != frameInfo.LastPresentTime)
                    {
                        lastPresentTime = frameInfo.LastPresentTime;

                        ID3D11Texture2D frameTexture;
                        if (result = frameResource.QueryInterface<ID3D11Texture2D>(&frameTexture))
                        {
                            if (result = deviceContext.CopyResource(frameTexture, texture))
                            {
                                SubresourceData subresource;
                                if (result = deviceContext.Map(texture, 0, MapType.Read, MapFlags.None, &subresource))
                                {
                                    frame->FrameInfo = frameInfo;
                                    frame->Bitmap = new MemoryBitmap((Color*)subresource.Data, (int)textureDescription.Width, (int)textureDescription.Height);
                                }
                            }
                        }
                    }
                    else result = new HResult { Code = unchecked((uint)-1) };
                }
                else
                {
                    const uint DXGI_ERROR_ACCESS_LOST = 0x887A0026;

                    if (result == DXGI_ERROR_ACCESS_LOST)
                        ReinitializeDublication();
                }
            }
        }

        return result;
    }

    HResult EnsureDuplicationIsInitialized() => hasInitializedDuplication ? default : InitializeDuplication();

    HResult ReinitializeDublication()
    {
        ReleaseDublication();
        return InitializeDuplication();
    }

    void ReleaseDublication()
    {
        if (hasInitializedDuplication)
        {
            hasInitializedDuplication = false;

            ReleasePreviousFrame(); // skip hresult check
            duplication.Release(); // skip hresult check
        }
    }

    HResult InitializeDuplication()
    {
        hasInitializedDuplication = true;
        fixed (Duplicator* self = &this)
            return output.DuplicateOutput(device, &self->duplication);
    }

    HResult ReleasePreviousFrame()
    {
        if (!hasInitializedFrame)
            return default;
        hasInitializedFrame = false;

        HResult result;
        _ = (result = deviceContext.Unmap(texture, 0)) &&
            (result = duplication.ReleaseFrame());

        return default;
    }

    public void Release() => ReleaseDublication();

    public static HResult Create(GraphicDevice* device, Screen* screen, Duplicator* duplicator)
    {
        duplicator->device = device->Device;
        duplicator->output = screen->Output;
        duplicator->deviceContext = device->Context;

        var bounds = screen->Descriptor.DesktopBounds;
        duplicator->textureDescription = new Texture2DDescription
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

        return device->Device.CreateTexture2D(&duplicator->textureDescription, &duplicator->texture);
    }
}