namespace ScreenCapture;
public class Frame
{
    public Frame(OutDuplFrameInfo frameInfo, TextureMemoryBitmap bitmap)
    {
        FrameInfo = frameInfo;
        Bitmap = bitmap;
    }

    public OutDuplFrameInfo FrameInfo;
    public TextureMemoryBitmap Bitmap;
}