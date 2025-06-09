Screen capturing via IDXGIOutputDuplication
-------------------------------------------
Implementation of screen capture via DirectX using IDXGIOutputDuplication in C# without any dependencies. \
Emphasis on perfomance, but unfortunately due to a language limitation is completely covered by boxing structures into interfaces.

Samples
-------
Both samples can be found in [Program.cs](Console/Program.cs) \
But for illustrative purposes:
```csharp
using var capturer = new ScreenCapturer();

Frame frame;
while (true)
  if (capturer.CaptureFrame(&frame))
     …
```
or with the choice of device and screen:
```csharp
GraphicDevice device;
GraphicDevice.EnumDevice(&device, index: 0);

Screen screen;
Screen.EnumScreen(&screen, &device, index: 0);

Duplicator duplicator;
Duplicator.Create(&device, &screen, &duplicator);

Frame frame;
while (true)
   if (duplicator.CaptureFrame(&frame))
      …
```
