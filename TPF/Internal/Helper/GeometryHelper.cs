using System;
using System.Windows;
using System.Windows.Media;

namespace TPF.Internal
{
    internal static class GeometryHelper
    {
        internal static Geometry GetRoundedRectangle(Rect baseRectangle, Thickness borderThickness, CornerRadius cornerRadius)
        {
            // Mögliche Fehler bei sehr kleinem CornerRadius ausgleichen
            if (cornerRadius.TopLeft < double.Epsilon) cornerRadius.TopLeft = 0.0;
            if (cornerRadius.TopRight < double.Epsilon) cornerRadius.TopRight = 0.0;
            if (cornerRadius.BottomLeft < double.Epsilon) cornerRadius.BottomLeft = 0.0;
            if (cornerRadius.BottomRight < double.Epsilon) cornerRadius.BottomRight = 0.0;

            // BorderThickness berücksichtigen
            var leftHalf = borderThickness.Left * 0.5;
            if (leftHalf < double.Epsilon) leftHalf = 0.0;

            var topHalf = borderThickness.Top * 0.5;
            if (topHalf < double.Epsilon) topHalf = 0.0;

            var rightHalf = borderThickness.Right * 0.5;
            if (rightHalf < double.Epsilon) rightHalf = 0.0;

            var bottomHalf = borderThickness.Bottom * 0.5;
            if (bottomHalf < double.Epsilon) bottomHalf = 0.0;

            // Die Rectangles für die Ecken erstellen 
            var topLeftRectangle = new Rect(baseRectangle.Location.X, baseRectangle.Location.Y,
                Math.Max(0.0, cornerRadius.TopLeft - leftHalf),
                Math.Max(0.0, cornerRadius.TopLeft - rightHalf));

            var topRightRectangle = new Rect(baseRectangle.Location.X + baseRectangle.Width - cornerRadius.TopRight + rightHalf, 
                baseRectangle.Location.Y,
                Math.Max(0.0, cornerRadius.TopRight - rightHalf),
                Math.Max(0.0, cornerRadius.TopRight - topHalf));

            var bottomRightRectangle = new Rect(
                baseRectangle.Location.X + baseRectangle.Width - cornerRadius.BottomRight + rightHalf,
                baseRectangle.Location.Y + baseRectangle.Height - cornerRadius.BottomRight + bottomHalf,
                Math.Max(0.0, cornerRadius.BottomRight - rightHalf),
                Math.Max(0.0, cornerRadius.BottomRight - bottomHalf));

            var bottomLeftRectangle = new Rect(baseRectangle.Location.X,
                baseRectangle.Location.Y + baseRectangle.Height - cornerRadius.BottomLeft + bottomHalf,
                Math.Max(0.0, cornerRadius.BottomLeft - leftHalf),
                Math.Max(0.0, cornerRadius.BottomLeft - bottomHalf));

            // Die Breite von den oberen beiden Rectangles proportional zu der Breite von baseRectangle anpassen 
            if (topLeftRectangle.Right > topRightRectangle.Left)
            {
                var newWidth = (topLeftRectangle.Width / (topLeftRectangle.Width + topRightRectangle.Width)) * baseRectangle.Width;
                topLeftRectangle = new Rect(topLeftRectangle.Location.X, topLeftRectangle.Location.Y, newWidth, topLeftRectangle.Height);
                topRightRectangle = new Rect(baseRectangle.Left + newWidth, topRightRectangle.Location.Y, Math.Max(0.0, baseRectangle.Width - newWidth), topRightRectangle.Height);
            }

            // Die Höhe von den oberen beiden Rectangles proportional zu der Höhe von baseRectangle anpassen 
            if (topRightRectangle.Bottom > bottomRightRectangle.Top)
            {
                var newHeight = (topRightRectangle.Height / (topRightRectangle.Height + bottomRightRectangle.Height)) * baseRectangle.Height;
                topRightRectangle = new Rect(topRightRectangle.Location.X, topRightRectangle.Location.Y, topRightRectangle.Width, newHeight);
                bottomRightRectangle = new Rect(bottomRightRectangle.Location.X, baseRectangle.Top + newHeight, bottomRightRectangle.Width, Math.Max(0.0, baseRectangle.Height - newHeight));
            }

            // Die Breite von den unteren beiden Rectangles proportional zu der Breite von baseRectangle anpassen 
            if (bottomRightRectangle.Left < bottomLeftRectangle.Right)
            {
                var newWidth = (bottomLeftRectangle.Width / (bottomLeftRectangle.Width + bottomRightRectangle.Width)) * baseRectangle.Width;
                bottomLeftRectangle = new Rect(bottomLeftRectangle.Location.X, bottomLeftRectangle.Location.Y, newWidth, bottomLeftRectangle.Height);
                bottomRightRectangle = new Rect(baseRectangle.Left + newWidth, bottomRightRectangle.Location.Y, Math.Max(0.0, baseRectangle.Width - newWidth), bottomRightRectangle.Height);
            }

            // Die Höhe von den unteren beiden Rectangles proportional zu der Höhe von baseRectangle anpassen 
            if (bottomLeftRectangle.Top < topLeftRectangle.Bottom)
            {
                var newHeight = (topLeftRectangle.Height / (topLeftRectangle.Height + bottomLeftRectangle.Height)) * baseRectangle.Height;
                topLeftRectangle = new Rect(topLeftRectangle.Location.X, topLeftRectangle.Location.Y, topLeftRectangle.Width, newHeight);
                bottomLeftRectangle = new Rect(bottomLeftRectangle.Location.X, baseRectangle.Top + newHeight, bottomLeftRectangle.Width, Math.Max(0.0, baseRectangle.Height - newHeight));
            }

            var geometry = new StreamGeometry();

            using (var context = geometry.Open())
            {
                // Am unteren Ende der Ecke links oben anfangen
                context.BeginFigure(topLeftRectangle.BottomLeft, true, true);
                // Ecke links oben
                context.ArcTo(topLeftRectangle.TopRight, topLeftRectangle.Size, 0, false, SweepDirection.Clockwise, true, true);
                // Oben
                context.LineTo(topRightRectangle.TopLeft, true, true);
                // Ecke rechts oben
                context.ArcTo(topRightRectangle.BottomRight, topRightRectangle.Size, 0, false, SweepDirection.Clockwise, true, true);
                // Rechte Seite
                context.LineTo(bottomRightRectangle.TopRight, true, true);
                // Ecke rechts unten
                context.ArcTo(bottomRightRectangle.BottomLeft, bottomRightRectangle.Size, 0, false, SweepDirection.Clockwise, true, true);
                // Unten
                context.LineTo(bottomLeftRectangle.BottomRight, true, true);
                // Ecke unten links
                context.ArcTo(bottomLeftRectangle.TopLeft, bottomLeftRectangle.Size, 0, false, SweepDirection.Clockwise, true, true);
            }

            return geometry;
        }
    }
}