using ScreenCapture;
using ScreenCapture.Extensions;
using ScreenCapture.Internal;

unsafe
{
    var samplesDirectory = @"C:\samples";
    Directory.CreateDirectory(samplesDirectory);

    using var capturer = new ScreenCapturer();

    int counter = 1;
    Frame frame;
    HResult result;
    while (true)
    {
        if (result = capturer.CaptureFrame(&frame))
        {
            frame.Bitmap.Save(Path.Combine(samplesDirectory, $"sample-{counter}.png"));
            Console.WriteLine($"Captured and saved {counter}-th frame!");
            counter++;
        }
        else if (result.NoResult)
        {
            // skip, there's no new frame
            Thread.Sleep(25);
        }
        else Console.WriteLine($"Failed to capture {counter}-th frame. HResult: {result}");
    }
}

unsafe
{
    var samplesDirectory = @"C:\samples";
    Directory.CreateDirectory(samplesDirectory);

    GraphicDevice device;
    GraphicDevice.EnumDevice(&device, index: 0);

    Screen screen;
    Screen.EnumScreen(&screen, &device, index: 0);

    Duplicator duplicator;
    Duplicator.Create(&device, &screen, &duplicator);

    int counter = 1;
    Frame frame;
    HResult result;
    while (true)
    {
        if (result = duplicator.CaptureFrame(&frame))
        {
            frame.Bitmap.Save(Path.Combine(samplesDirectory, $"sample-{counter}.png"));
            Console.WriteLine($"Captured and saved {counter}-th frame!");
            counter++;
        }
        else if (result.NoResult)
        {
            // skip, there's no new frame
            Thread.Sleep(25);
        }
        else Console.WriteLine($"Failed to capture {counter}-th frame. HResult: {result}");
    }

    device.Release();
    screen.Release();
    duplicator.Release();
}