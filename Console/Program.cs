using ScreenCapture;

var device = new GraphicDevice();
var screen = new Screen(device);

int counter = 0;
while (true)
{
    if (screen.CaptureFrame(out Frame frame))
    {
        Console.WriteLine($"Got {++counter}-th frame!");
    }
}
