using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public class ClipBorder : Border
    {
        private StreamGeometry _backgroundGeometryCache;
        private StreamGeometry _borderGeometryCache;

        protected override Size MeasureOverride(Size constraint)
        {
            var desiredSize = new Size();

            // Mindestgröße ausrechnen
            var borderSize = BorderThickness.CalculateSize();
            var paddingSize = Padding.CalculateSize();
            var combinedSize = new Size(borderSize.Width + paddingSize.Width, borderSize.Height + paddingSize.Height);

            // Haben wir ein Child-Element?
            if (Child != null)
            {
                // Das Child-Element kriegt nur die mögliche Größe abzuglich unserer BorderThickness und des Paddings zur verfügung
                var childConstraint = new Size(Math.Max(0.0, constraint.Width - combinedSize.Width),
                                               Math.Max(0.0, constraint.Height - combinedSize.Height));

                Child.Measure(childConstraint);

                var childSize = Child.DesiredSize;

                // Um die finale Größe zu bestimmen muss nun unser Child um die Werte aus combinedSize erweitert werden
                desiredSize.Width = childSize.Width + combinedSize.Width;
                desiredSize.Height = childSize.Height + combinedSize.Height;
            }
            else desiredSize = combinedSize;

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // Da die Properties im folgenden Code mehrmals abgerufen werden holen wir uns die Werte hier direkt am Anfang einmal
            // Weil es sich um DependencyProperties handelt, können wir so mehrere Zugriffe auf die GetValue-Methode sparen
            var borderThickness = BorderThickness;
            var cornerRadius = CornerRadius;

            var boundRect = new Rect(finalSize);
            var innerRect = boundRect.Deflate(borderThickness);
            var childRect = innerRect.Deflate(Padding);

            // Border
            if (!boundRect.Width.IsZero() && !boundRect.Height.IsZero())
            {
                var outerBorderInfo = new BorderInfo(cornerRadius, borderThickness, new Thickness(), true);
                var borderGeometry = new StreamGeometry();

                using (var context = borderGeometry.Open())
                {
                    CreateGeometry(context, boundRect, outerBorderInfo);
                }

                borderGeometry.Freeze();
                _borderGeometryCache = borderGeometry;
            }
            else _borderGeometryCache = null;

            // Background
            if (!innerRect.Width.IsZero() && !innerRect.Height.IsZero())
            {
                var innerBorderInfo = new BorderInfo(cornerRadius, borderThickness, new Thickness(), false);
                var backgroundGeometry = new StreamGeometry();

                using (var context = backgroundGeometry.Open())
                {
                    CreateGeometry(context, innerRect, innerBorderInfo);
                }

                backgroundGeometry.Freeze();
                _backgroundGeometryCache = backgroundGeometry;
            }
            else _backgroundGeometryCache = null;

            if (Child != null)
            {
                Child.Arrange(childRect);

                var clipGeometry = new StreamGeometry();
                var childBorderInfo = new BorderInfo(cornerRadius, borderThickness, Padding, false);

                using (var context = clipGeometry.Open())
                {
                    CreateGeometry(context, new Rect(0, 0, childRect.Width, childRect.Height), childBorderInfo);
                }

                clipGeometry.Freeze();

                Child.Clip = clipGeometry;
            }

            return finalSize;
        }

        protected override void OnRender(DrawingContext dc)
        {
            // Da die Properties im folgenden Code mehrmals abgerufen werden holen wir uns die Werte hier direkt am Anfang einmal
            // Weil es sich um DependencyProperties handelt, können wir so mehrere Zugriffe auf die GetValue-Methode sparen
            var borderThickness = BorderThickness;
            var borderBrush = BorderBrush;
            var background = Background;

            var borderGeometry = _borderGeometryCache;
            var backgroundGeometry = _backgroundGeometryCache;

            var drawBorder = borderBrush != null && !borderThickness.IsZero();
            var drawBackground = background != null;

            // Müssen wir Border und Background zeichnen?
            if (drawBorder && drawBackground)
            {
                // Sind BorderBrush und Background gleich? 
                if (Comparer.IsEqualTo(borderBrush, background))
                {
                    // Nur ein Objekt malen
                    dc.DrawGeometry(borderBrush, null, borderGeometry);
                } // Sind beide Brushes undurchsichtige SolidColorBrushes?
                else if (borderBrush.IsOpaqueSolidColorBrush() && background.IsOpaqueSolidColorBrush())
                {
                    // Zuerst eine Schicht mit dem BorderBrush malen und darüber dann eine Schicht mit dem Background
                    dc.DrawGeometry(borderBrush, null, borderGeometry);
                    dc.DrawGeometry(background, null, backgroundGeometry);
                }
                else
                {
                    // Hier brauchen wir auf jeden Fall beide Geometry-Objekte
                    if (borderGeometry == null || backgroundGeometry == null) return;

                    // Hier brauchen wir ein Geometry-Objekt das nur den Rahmen darstellt
                    // Um das zu bekommen schneiden wir über ein Exclude den inneren Bereich raus
                    var borderOutlinePath = borderGeometry.GetOutlinedPathGeometry();
                    var backgroundOutlinePath = backgroundGeometry.GetOutlinedPathGeometry();
                    var borderOutlineGeometry = Geometry.Combine(borderOutlinePath, backgroundOutlinePath, GeometryCombineMode.Exclude, null);

                    // Ist wenigstens der BorderBrush ein undurchsichtiger SolidColorBrush?
                    if (borderBrush.IsOpaqueSolidColorBrush())
                    {
                        // Zuerst den Hintergrund malen und danach den Rand drüber malen
                        dc.DrawGeometry(background, null, borderGeometry);
                        dc.DrawGeometry(borderBrush, null, borderOutlineGeometry);
                    }
                    else
                    {
                        // Da der Rahmen nicht einfach über den Hintergrund drüber gemalt werden kann muss beides getrennt gemalt werden
                        dc.DrawGeometry(borderBrush, null, borderOutlineGeometry);
                        dc.DrawGeometry(background, null, backgroundGeometry);
                    }
                }

            } // Müssen wir nur Border zeichnen?
            else if (drawBorder)
            {
                if ((borderGeometry != null) && (backgroundGeometry != null))
                {
                    var borderOutlinePath = borderGeometry.GetOutlinedPathGeometry();
                    var backgroundOutlinePath = backgroundGeometry.GetOutlinedPathGeometry();
                    var borderOutlineGeometry = Geometry.Combine(borderOutlinePath, backgroundOutlinePath, GeometryCombineMode.Exclude, null);

                    dc.DrawGeometry(borderBrush, null, borderOutlineGeometry);
                }
                else dc.DrawGeometry(borderBrush, null, borderGeometry);
            } // Müssen wir nur Background zeichnen?
            else if (drawBackground) dc.DrawGeometry(background, null, backgroundGeometry);
        }

        private static void CreateGeometry(StreamGeometryContext context, Rect rect, BorderInfo borderInfo)
        {
            // Koordinaten berechnen
            var leftTop = new Point(borderInfo.LeftTop, 0);
            var rightTop = new Point(rect.Width - borderInfo.RightTop, 0);
            var topRight = new Point(rect.Width, borderInfo.TopRight);
            var bottomRight = new Point(rect.Width, rect.Height - borderInfo.BottomRight);
            var rightBottom = new Point(rect.Width - borderInfo.RightBottom, rect.Height);
            var leftBottom = new Point(borderInfo.LeftBottom, rect.Height);
            var bottomLeft = new Point(0, rect.Height - borderInfo.BottomLeft);
            var topLeft = new Point(0, borderInfo.TopLeft);

            // Auf mögliches überlappen von Punkten prüfen und beheben

            // Oben
            if (leftTop.X > rightTop.X)
            {
                var v = borderInfo.LeftTop / (borderInfo.LeftTop + borderInfo.RightTop) * rect.Width;
                leftTop.X = v;
                rightTop.X = v;
            }

            // Rechts
            if (topRight.Y > bottomRight.Y)
            {
                var v = borderInfo.TopRight / (borderInfo.TopRight + borderInfo.BottomRight) * rect.Height;
                topRight.Y = v;
                bottomRight.Y = v;
            }

            // Unten
            if (leftBottom.X > rightBottom.X)
            {
                var v = borderInfo.LeftBottom / (borderInfo.LeftBottom + borderInfo.RightBottom) * rect.Width;
                rightBottom.X = v;
                leftBottom.X = v;
            }

            // Links
            if (topLeft.Y > bottomLeft.Y)
            {
                var v = borderInfo.TopLeft / (borderInfo.TopLeft + borderInfo.BottomLeft) * rect.Height;
                bottomLeft.Y = v;
                topLeft.Y = v;
            }

            // Offset anwenden
            var offset = new Vector(rect.TopLeft.X, rect.TopLeft.Y);

            leftTop += offset;
            rightTop += offset;
            topRight += offset;
            bottomRight += offset;
            rightBottom += offset;
            leftBottom += offset;
            bottomLeft += offset;
            topLeft += offset;

            // Oben links anfangen zu zeichnen
            context.BeginFigure(leftTop, true, true);

            // Linie nach oben rechts
            context.LineTo(rightTop, true, false);

            // Ecke oben rechts berechnen
            var radiusX = rect.TopRight.X - rightTop.X;
            var radiusY = topRight.Y - rect.TopRight.Y;

            // Müssen wir eine abgerundete Ecke malen?
            if (!radiusX.IsZero() || !radiusY.IsZero())
            {
                context.ArcTo(topRight, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

            // Linie nach unten rechts
            context.LineTo(bottomRight, true, false);

            // Ecke unten rechts berechnen
            radiusX = rect.BottomRight.X - rightBottom.X;
            radiusY = rect.BottomRight.Y - bottomRight.Y;

            // Müssen wir eine abgerundete Ecke malen?
            if (!radiusX.IsZero() || !radiusY.IsZero())
            {
                context.ArcTo(rightBottom, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

            // Linie nach unten links
            context.LineTo(leftBottom, true, false);

            // Ecke unten links berechnen
            radiusX = leftBottom.X - rect.BottomLeft.X;
            radiusY = rect.BottomLeft.Y - bottomLeft.Y;

            // Müssen wir eine abgerundete Ecke malen?
            if (!radiusX.IsZero() || !radiusY.IsZero())
            {
                context.ArcTo(bottomLeft, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

            // Linie nach oben links
            context.LineTo(topLeft, true, false);

            // Ecke oben links berechnen
            radiusX = leftTop.X - rect.TopLeft.X;
            radiusY = topLeft.Y - rect.TopLeft.Y;

            // Müssen wir eine abgerundete Ecke malen?
            if (!radiusX.IsZero() || !radiusY.IsZero())
            {
                context.ArcTo(leftTop, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }
        }

        private struct BorderInfo
        {
            internal BorderInfo(CornerRadius corners, Thickness borders, Thickness padding, bool isOuterBorder)
            {
                var left = (0.5 * borders.Left) + padding.Left;
                var top = (0.5 * borders.Top) + padding.Top;
                var right = (0.5 * borders.Right) + padding.Right;
                var bottom = (0.5 * borders.Bottom) + padding.Bottom;

                if (isOuterBorder)
                {
                    if (corners.TopLeft.IsZero())
                    {
                        LeftTop = TopLeft = 0.0;
                    }
                    else
                    {
                        LeftTop = corners.TopLeft + left;
                        TopLeft = corners.TopLeft + top;
                    }

                    if (corners.TopRight.IsZero())
                    {
                        TopRight = RightTop = 0.0;
                    }
                    else
                    {
                        TopRight = corners.TopRight + top;
                        RightTop = corners.TopRight + right;
                    }

                    if (corners.BottomRight.IsZero())
                    {
                        RightBottom = BottomRight = 0.0;
                    }
                    else
                    {
                        RightBottom = corners.BottomRight + right;
                        BottomRight = corners.BottomRight + bottom;
                    }

                    if (corners.BottomLeft.IsZero())
                    {
                        BottomLeft = LeftBottom = 0.0;
                    }
                    else
                    {
                        BottomLeft = corners.BottomLeft + bottom;
                        LeftBottom = corners.BottomLeft + left;
                    }
                }
                else
                {
                    LeftTop = Math.Max(0.0, corners.TopLeft - left);
                    TopLeft = Math.Max(0.0, corners.TopLeft - top);
                    TopRight = Math.Max(0.0, corners.TopRight - top);
                    RightTop = Math.Max(0.0, corners.TopRight - right);
                    RightBottom = Math.Max(0.0, corners.BottomRight - right);
                    BottomRight = Math.Max(0.0, corners.BottomRight - bottom);
                    BottomLeft = Math.Max(0.0, corners.BottomLeft - bottom);
                    LeftBottom = Math.Max(0.0, corners.BottomLeft - left);
                }
            }

            internal readonly double LeftTop;
            internal readonly double TopLeft;
            internal readonly double TopRight;
            internal readonly double RightTop;
            internal readonly double RightBottom;
            internal readonly double BottomRight;
            internal readonly double BottomLeft;
            internal readonly double LeftBottom;
        }
    }
}