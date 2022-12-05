using System.Drawing;

namespace CBRE.Extended.Common;

public static class ColorUtils
{
    private static readonly Random _Random;

    static ColorUtils()
    {
        _Random = new Random();
    }

    public static Color GetRandomColor()
    {
        return Color.FromArgb(255, _Random.Next(0, 256), _Random.Next(0, 256), _Random.Next(0, 256));
    }

    /// <summary>
    /// Brush colors only vary from shades of green and blue
    /// </summary>
    public static Color GetRandomBrushColor()
    {
        return Color.FromArgb(255, 0, _Random.Next(128, 256), _Random.Next(128, 256));
    }

    /// <summary>
    /// Group colors only vary from shades of green and red
    /// </summary>
    public static Color GetRandomGroupColor()
    {
        return Color.FromArgb(255, _Random.Next(128, 256), _Random.Next(128, 256), 0);
    }

    public static Color GetRandomLightColour()
    {
        return Color.FromArgb(255, _Random.Next(128, 256), _Random.Next(128, 256), _Random.Next(128, 256));
    }

    public static Color GetRandomDarkColor()
    {
        return Color.FromArgb(255, _Random.Next(0, 128), _Random.Next(0, 128), _Random.Next(0, 128));
    }

    public static Color GetDefaultEntityColor()
    {
        return Color.FromArgb(255, 255, 0, 255);
    }

    public static Color Vary(this Color Color, int By = 10)
    {
        By = _Random.Next(-By, By);
        return Color.FromArgb(Color.A, Math.Min(255, Math.Max(0, Color.R + By)), Math.Min(255, Math.Max(0, Color.G + By)), Math.Min(255, Math.Max(0, Color.B + By)));
    }

    public static Color Darken(this Color Color, int By = 20)
    {
        return Color.FromArgb(Color.A, Math.Max(0, Color.R - By), Math.Max(0, Color.G - By), Math.Max(0, Color.B - By));
    }

    public static Color Lighten(this Color Color, int By = 20)
    {
        return Color.FromArgb(Color.A, Math.Min(255, Color.R + By), Math.Min(255, Color.G + By), Math.Min(255, Color.B + By));
    }

    public static Color Blend(this Color Color, Color Other)
    {
        return Color.FromArgb(
            (byte)((Color.A) / 255f * (Other.A / 255f) * 255),
            (byte)((Color.R) / 255f * (Other.R / 255f) * 255),
            (byte)((Color.G) / 255f * (Other.G / 255f) * 255),
            (byte)((Color.B) / 255f * (Other.B / 255f) * 255)
        );
    }
}