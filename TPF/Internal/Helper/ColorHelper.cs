using System;
using System.Windows.Media;

namespace TPF.Internal
{
    internal static class ColorHelper
    {
        internal static Color ConvertHsvToRgb(double h, double s, double v)
        {
            double r = 0, g = 0, b = 0;

            if (s == 0)
            {
                r = v;
                g = v;
                b = v;
            }
            else
            {
                int i;
                double f, p, q, t;

                if (h == 360) h = 0;
                else h /= 60;

                i = (int)Math.Truncate(h);
                f = h - i;

                p = v * (1.0 - s);
                q = v * (1.0 - (s * f));
                t = v * (1.0 - (s * (1.0 - f)));

                switch (i)
                {
                    case 0:
                    {
                        r = v;
                        g = t;
                        b = p;
                        break;
                    }
                    case 1:
                    {
                        r = q;
                        g = v;
                        b = p;
                        break;
                    }
                    case 2:
                    {
                        r = p;
                        g = v;
                        b = t;
                        break;
                    }
                    case 3:
                    {
                        r = p;
                        g = q;
                        b = v;
                        break;
                    }
                    case 4:
                    {
                        r = t;
                        g = p;
                        b = v;
                        break;
                    }
                    default:
                    {
                        r = v;
                        g = p;
                        b = q;
                        break;
                    }
                }
            }

            return Color.FromArgb(255, (byte)(Math.Round(r * 255)), (byte)(Math.Round(g * 255)), (byte)(Math.Round(b * 255)));
        }

        internal static HsvColor ConvertRgbToHsv(int r, int g, int b)
        {
            double delta, min;
            double h = 0, s, v;

            min = Math.Min(Math.Min(r, g), b);
            v = Math.Max(Math.Max(r, g), b);
            delta = v - min;

            if (v == 0.0) s = 0;
            else s = delta / v;

            if (s == 0) h = 0.0;

            else
            {
                if (r == v) h = (g - b) / delta;
                else if (g == v) h = 2 + (b - r) / delta;
                else if (b == v) h = 4 + (r - g) / delta;

                h *= 60;

                if (h < 0.0) h += 360;
            }

            return new HsvColor
            {
                H = h,
                S = s,
                V = v / 255
            };
        }

        internal static bool UseBrightForegroundForBackgroundColor(Color color)
        {
            return UseBrightForegroundForBackgroundColor(color.R, color.G, color.B);
        }

        internal static bool UseBrightForegroundForBackgroundColor(int red, int green, int blue)
        {
            if (red * 0.299 + green * 0.587 + blue * 0.114 > 149) return false;
            else return true;
        }

        internal static Color GetTextColorForBackgroundColor(Color color)
        {
            return GetTextColorForBackgroundColor(color.R, color.G, color.B);
        }

        internal static Color GetTextColorForBackgroundColor(int red, int green, int blue)
        {
            if (UseBrightForegroundForBackgroundColor(red, green, blue)) return Colors.White;
            else return Colors.Black;
        }
    }
}