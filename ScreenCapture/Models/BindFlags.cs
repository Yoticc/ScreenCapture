namespace ScreenCapture.DirectXModels;
[Flags]
public enum BindFlags
{
    None = 0x00,
    VertexBuffer = 0x01,
    IndexBuffer = 0x02,
    ConstantBuffer = 0x04,
    ShaderResource = 0x08,
    StreamOutput = 0x10,
    RenderTarget = 0x20,
    DepthStencil = 0x40,
    UnorderedAccess = 0x80,
    Decoder = 0x200,
    VideoEncoder = 0x400
}