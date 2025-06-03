using ScreenCapture.Internal;

namespace ScreenCapture.Extensions;
public unsafe class ScreenCapturer : IDisposable
{
    public ScreenCapturer(uint screenIndex = 0)
    {
        fixed (GraphicDevice* device = &this.device)
        {
            GraphicDevice.EnumDevice(device, 0);
            fixed (Screen* screen = &this.screen)
            {
                Screen.EnumScreen(screen, device, screenIndex);
                fixed (Duplicator* duplicator = &this.duplicator)
                {
                    Duplicator.Create(device, screen, duplicator);
                }
            }
        }
    }

    GraphicDevice device;
    Screen screen;
    Duplicator duplicator;

    public HResult CaptureFrame(Frame* frame) => duplicator.CaptureFrame(frame);

    bool disposed;
    public void Dispose()
    {
        if (disposed)
            return;
        disposed = true;

        duplicator.Release();
        screen.Release();
        device.Release();
    }
}