namespace CBRE.Extended.Common;

[Flags]
public enum TextureFlags : uint 
{
    None = 1u << 0,
    Transparent = 1u << 1,

    Missing = 1u << 31
}