namespace CBRE.Extended.Common.Extensions;

public static class TextureExtensions
{
    public static bool HasTransparency(this ITexture Texture)
    {
        return Texture.Flags.HasFlag(TextureFlags.Transparent);
    }
}