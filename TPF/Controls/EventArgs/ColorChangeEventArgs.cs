using System;
using System.Windows;
using System.Windows.Media;

namespace TPF.Controls
{
    public class ColorChangeEventArgs : RoutedEventArgs
    {
        public Color Color { get; }

        public ColorChangeEventArgs(Color color)
        {
            Color = color;
        }
    }

    public delegate void ColorChangeEventHandler(object sender, ColorChangeEventArgs e);
}