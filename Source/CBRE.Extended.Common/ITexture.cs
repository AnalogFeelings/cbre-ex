namespace CBRE.Extended.Common;

public interface ITexture : IDisposable
{
    TextureFlags Flags { get; }
    string Name { get; }
    int Width { get; }
    int Height { get; }
}