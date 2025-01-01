using ScreenCapture.Internal;

namespace ScreenCapture;
public unsafe class Screen
{
    public Screen(int index = 0)
    {
        Index = index;
    }

    public readonly int Index;

    public ID3D11Device Device;
    public ID3D11DeviceContext Context;
    public IDXGIAdapter Adapter;

    public void Initialize()
    {

    }
}