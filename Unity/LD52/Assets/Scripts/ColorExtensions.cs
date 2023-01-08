using UnityEngine;

public static class ColorExtensions
{
    public static Color Multiply(this Color color, float saturation = 1, float value = 1, float alpha = 1)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        Color c = Color.HSVToRGB(h, s * saturation, v * value);
        c.a = color.a * alpha;
        return c;
    }

    public static Color Add(this Color color, float hue = 0, float saturation = 0, float value = 0, float alpha = 0)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        Color c = Color.HSVToRGB(h + hue, s + saturation, v + value);
        c.a = color.a + alpha;
        return c;
    }

    public static Color Set(this Color color, float hue = -1, float saturation = -1, float value = -1, float alpha = -1)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        Color c = Color.HSVToRGB(hue >= 0 ? hue : h, saturation >= 0 ? saturation : s, value >= 0 ? value : v);
        c.a = alpha >= 0 ? alpha : color.a;
        return c;
    }

    public static Color Alpha(this Color color, float alpha) => Multiply(color, alpha: alpha);
}
