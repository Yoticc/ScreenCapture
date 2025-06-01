using ScreenCapture;
using ScreenCapture.Internal;

unsafe
{
    using var capturer = new ScreenCapturer();

    int counter = 0;
    Frame frame;
    HResult result;
    while (true)
    {
        counter++;
        if (result = capturer.CaptureFrame(&frame))
            Console.WriteLine($"Captured {counter}-th frame!");
        else Console.WriteLine($"Failed to capture {counter}-th frame. HResult: {result}");
    }
}