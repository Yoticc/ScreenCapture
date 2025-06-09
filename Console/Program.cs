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