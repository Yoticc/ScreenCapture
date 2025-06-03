using ScreenCapture;
using ScreenCapture.Extensions;
using ScreenCapture.Internal;

unsafe
{
    var samplesDirectory = @"C:\samples";
    Directory.CreateDirectory(samplesDirectory);

    using var capturer = new ScreenCapturer();

    int counter = 0;
    Frame frame;
    HResult result;
    while (true)
    {
        counter++;
        if (result = capturer.CaptureFrame(&frame))
        {
            frame.Bitmap.Save(Path.Combine(samplesDirectory, $"sample-{counter}.png"));
            Console.WriteLine($"Captured and saved {counter}-th frame!");
        }
        else if (!result.NoResult)
            Console.WriteLine($"Failed to capture {counter}-th frame. HResult: {result}");
    }
}