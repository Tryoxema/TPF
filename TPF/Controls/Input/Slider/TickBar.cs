using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using TPF.Internal;

namespace TPF.Controls
{
    public class TickBar : FrameworkElement
    {
        static TickBar()
        {
            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(TickBar), new FrameworkPropertyMetadata(true));
        }

        #region TickBrush DependencyProperty
        public static readonly DependencyProperty TickBrushProperty = DependencyProperty.Register("TickBrush",
            typeof(Brush),
            typeof(TickBar),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush TickBrush
        {
            get { return (Brush)GetValue(TickBrushProperty); }
            set { SetValue(TickBrushProperty, value); }
        }
        #endregion

        #region Minimum DependencyProperty
        public static readonly DependencyProperty MinimumProperty = RangeBase.MinimumProperty.AddOwner(typeof(TickBar),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        #endregion

        #region Maximum DependencyProperty
        public static readonly DependencyProperty MaximumProperty = RangeBase.MaximumProperty.AddOwner(typeof(TickBar),
            new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        #endregion

        #region TickFrequency DependencyProperty
        public static readonly DependencyProperty TickFrequencyProperty = Slider.TickFrequencyProperty.AddOwner(typeof(TickBar),
            new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double TickFrequency
        {
            get { return (double)GetValue(TickFrequencyProperty); }
            set { SetValue(TickFrequencyProperty, value); }
        }
        #endregion

        #region IsDirectionReversed DependencyProperty
        public static readonly DependencyProperty IsDirectionReversedProperty = Slider.IsDirectionReversedProperty.AddOwner(typeof(TickBar),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool IsDirectionReversed
        {
            get { return (bool)GetValue(IsDirectionReversedProperty); }
            set { SetValue(IsDirectionReversedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region UseNumbersAsTicksProperty DependencyProperty
        public static readonly DependencyProperty UseNumbersAsTicksProperty = Slider.UseNumbersAsTicksProperty.AddOwner(typeof(TickBar),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool UseNumbersAsTicks
        {
            get { return (bool)GetValue(UseNumbersAsTicksProperty); }
            set { SetValue(UseNumbersAsTicksProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region Placement DependencyProperty
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement",
            typeof(TickBarPlacement),
            typeof(Slider),
            new FrameworkPropertyMetadata(TickBarPlacement.Top, FrameworkPropertyMetadataOptions.AffectsRender));

        public TickBarPlacement Placement
        {
            get { return (TickBarPlacement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }
        #endregion

        #region ReservedSpace DependencyProperty
        public static readonly DependencyProperty ReservedSpaceProperty = DependencyProperty.Register("ReservedSpace",
            typeof(double),
            typeof(Slider),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double ReservedSpace
        {
            get { return (double)GetValue(ReservedSpaceProperty); }
            set { SetValue(ReservedSpaceProperty, value); }
        }
        #endregion

        private void BindDoubleToTemplatedParent(DependencyProperty target, DependencyProperty source)
        {
            if ((double)GetValue(target) == (double)target.GetMetadata(this).DefaultValue)
            {
                var binding = new Binding
                {
                    RelativeSource = RelativeSource.TemplatedParent,
                    Path = new PropertyPath(source)
                };
                SetBinding(target, binding);
            }
        }

        private void BindBoolToTemplatedParent(DependencyProperty target, DependencyProperty source)
        {
            if ((bool)GetValue(target) == (bool)target.GetMetadata(this).DefaultValue)
            {
                var binding = new Binding
                {
                    RelativeSource = RelativeSource.TemplatedParent,
                    Path = new PropertyPath(source)
                };
                SetBinding(target, binding);
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // Wenn die TickBar in einem Slider verbaut wurde (wofür sie gemacht wurde), dann Properties übernehmen, wenn die nicht schon eigene Werte haben
            if (TemplatedParent is Slider parent)
            {
                BindDoubleToTemplatedParent(TickFrequencyProperty, Slider.TickFrequencyProperty);
                BindDoubleToTemplatedParent(MinimumProperty, RangeBase.MinimumProperty);
                BindDoubleToTemplatedParent(MaximumProperty, RangeBase.MaximumProperty);
                BindBoolToTemplatedParent(IsDirectionReversedProperty, Slider.IsDirectionReversedProperty);
                BindBoolToTemplatedParent(UseNumbersAsTicksProperty, Slider.UseNumbersAsTicksProperty);
                if (parent.Track != null && ReservedSpace == (double)ReservedSpaceProperty.GetMetadata(this).DefaultValue)
                {
                    var binding = new Binding()
                    {
                        Source = parent.Track.Thumb,
                        Path = parent.Orientation == Orientation.Horizontal ? new PropertyPath(WidthProperty) : new PropertyPath(HeightProperty)
                    };
                    SetBinding(ReservedSpaceProperty, binding);
                }
            }

            if (UseNumbersAsTicks) DrawTickNumbers(drawingContext);
            else DrawTickLines(drawingContext);
        }

        private void DrawTickLines(DrawingContext drawingContext)
        {
            var size = new Size(ActualWidth, ActualHeight);
            var range = Maximum - Minimum;
            var tickLength = 0.0;
            double tickLength2;
            var logicalToPhysical = 1.0;
            var startPoint = new Point(0.0, 0.0);
            var endPoint = new Point(0.0, 0.0);

            var halfReservedSpace = ReservedSpace * 0.5;

            switch (Placement)
            {
                case TickBarPlacement.Left:
                    if (ReservedSpace >= size.Height) return;
                    size.Height -= ReservedSpace;
                    tickLength = -size.Width;
                    startPoint = new Point(size.Width, size.Height + halfReservedSpace);
                    endPoint = new Point(size.Width, halfReservedSpace);
                    logicalToPhysical = size.Height / range * -1;
                    break;
                case TickBarPlacement.Top:
                    if (ReservedSpace >= size.Width) return;
                    size.Width -= ReservedSpace;
                    tickLength = -size.Height;
                    startPoint = new Point(halfReservedSpace, size.Height);
                    endPoint = new Point(halfReservedSpace + size.Width, size.Height);
                    logicalToPhysical = size.Width / range;
                    break;
                case TickBarPlacement.Right:
                    if (ReservedSpace >= size.Height) return;
                    size.Height -= ReservedSpace;
                    tickLength = size.Width;
                    startPoint = new Point(0d, size.Height + halfReservedSpace);
                    endPoint = new Point(0d, halfReservedSpace);
                    logicalToPhysical = size.Height / range * -1;
                    break;
                case TickBarPlacement.Bottom:
                    if (ReservedSpace >= size.Width) return;
                    size.Width -= ReservedSpace;
                    tickLength = size.Height;
                    startPoint = new Point(halfReservedSpace, 0d);
                    endPoint = new Point(halfReservedSpace + size.Width, 0d);
                    logicalToPhysical = size.Width / range;
                    break;
            }

            tickLength2 = tickLength * 0.75;

            // Richtung Invertieren
            if (IsDirectionReversed)
            {
                logicalToPhysical *= -1;

                // startPoint & endPoint tauschen
                var point = startPoint;
                startPoint = endPoint;
                endPoint = point;
            }

            var pen = new Pen(TickBrush, 1.0d);

            var snapsToDevicePixels = SnapsToDevicePixels;
            var xLines = snapsToDevicePixels ? new DoubleCollection() : null;
            var yLines = snapsToDevicePixels ? new DoubleCollection() : null;

            if ((Placement == TickBarPlacement.Left) || (Placement == TickBarPlacement.Right))
            {
                // Interval verringern, wenn der verfügbare Platz überschritten wird
                var interval = TickFrequency;
                if (interval > 0.0)
                {
                    var minInterval = (Maximum - Minimum) / size.Height;
                    if (interval < minInterval)
                    {
                        interval = minInterval;
                    }
                }

                // Start & Ende zeichnen
                drawingContext.DrawLine(pen, startPoint, new Point(startPoint.X + tickLength, startPoint.Y));
                drawingContext.DrawLine(pen, new Point(startPoint.X, endPoint.Y), new Point(startPoint.X + tickLength, endPoint.Y));

                if (snapsToDevicePixels)
                {
                    xLines.Add(startPoint.X);
                    yLines.Add(startPoint.Y - 0.5);
                    xLines.Add(startPoint.X + tickLength);
                    yLines.Add(endPoint.Y - 0.5);
                    xLines.Add(startPoint.X + tickLength2);
                }

                for (var i = interval; i < range; i += interval)
                {
                    var y = (i * logicalToPhysical) + startPoint.Y;

                    drawingContext.DrawLine(pen, new Point(startPoint.X, y), new Point(startPoint.X + tickLength2, y));

                    if (snapsToDevicePixels) yLines.Add(y - 0.5);
                }
            }
            else
            {
                // Interval verringern, wenn der verfügbare Platz überschritten wird
                var interval = TickFrequency;
                if (interval > 0.0)
                {
                    var minInterval = (Maximum - Minimum) / size.Width;
                    if (interval < minInterval)
                    {
                        interval = minInterval;
                    }
                }

                // Start & Ende zeichnen
                drawingContext.DrawLine(pen, startPoint, new Point(startPoint.X, startPoint.Y + tickLength));
                drawingContext.DrawLine(pen, new Point(endPoint.X, startPoint.Y), new Point(endPoint.X, startPoint.Y + tickLength));

                if (snapsToDevicePixels)
                {
                    xLines.Add(startPoint.X - 0.5);
                    yLines.Add(startPoint.Y);
                    xLines.Add(startPoint.X - 0.5);
                    yLines.Add(endPoint.Y + tickLength);
                    yLines.Add(endPoint.Y + tickLength2);
                }

                for (var i = interval; i < range; i += interval)
                {
                    var x = (i * logicalToPhysical) + startPoint.X;
                    drawingContext.DrawLine(pen, new Point(x, startPoint.Y), new Point(x, startPoint.Y + tickLength2));

                    if (snapsToDevicePixels) xLines.Add(x - 0.5);
                }
            }

            if (snapsToDevicePixels)
            {
                xLines.Add(ActualWidth);
                yLines.Add(ActualHeight);
                VisualXSnappingGuidelines = xLines;
                VisualYSnappingGuidelines = yLines;
            }
        }

        private void DrawTickNumbers(DrawingContext drawingContext)
        {
            var size = new Size(ActualWidth, ActualHeight);
            var range = Maximum - Minimum;
            var logicalToPhysical = 1.0;
            var startPoint = new Point(0.0, 0.0);
            var endPoint = new Point(0.0, 0.0);
            var halfReservedSpace = ReservedSpace * 0.5;

            switch (Placement)
            {
                case TickBarPlacement.Left:
                    if (ReservedSpace >= size.Height) return;
                    size.Height -= ReservedSpace;
                    //tickLength = -size.Width;
                    startPoint = new Point(size.Width, size.Height + halfReservedSpace);
                    endPoint = new Point(size.Width, halfReservedSpace);
                    logicalToPhysical = size.Height / range * -1;
                    break;
                case TickBarPlacement.Top:
                    if (ReservedSpace >= size.Width) return;
                    size.Width -= ReservedSpace;
                    //tickLength = -size.Height;
                    startPoint = new Point(halfReservedSpace, size.Height);
                    endPoint = new Point(halfReservedSpace + size.Width, size.Height);
                    logicalToPhysical = size.Width / range;
                    break;
                case TickBarPlacement.Right:
                    if (ReservedSpace >= size.Height) return;
                    size.Height -= ReservedSpace;
                    //tickLength = size.Width;
                    startPoint = new Point(0d, size.Height + halfReservedSpace);
                    endPoint = new Point(0d, halfReservedSpace);
                    logicalToPhysical = size.Height / range * -1;
                    break;
                case TickBarPlacement.Bottom:
                    if (ReservedSpace >= size.Width) return;
                    size.Width -= ReservedSpace;
                    //tickLength = size.Height;
                    startPoint = new Point(halfReservedSpace, 0d);
                    endPoint = new Point(halfReservedSpace + size.Width, 0d);
                    logicalToPhysical = size.Width / range;
                    break;
            }

            // Richtung Invertieren
            if (IsDirectionReversed)
            {
                logicalToPhysical *= -1;

                // startPoint & endPoint tauschen
                var point = startPoint;
                startPoint = endPoint;
                endPoint = point;
            }

            FormattedText text = null;
            var typeFace = new Typeface("Verdana");

#pragma warning disable CS0618
            if ((Placement == TickBarPlacement.Left) || (Placement == TickBarPlacement.Right))
            {
                // Interval verringern, wenn der verfügbare Platz überschritten wird
                var interval = TickFrequency;
                if (interval > 0.0)
                {
                    var minInterval = (Maximum - Minimum) / size.Height;
                    if (interval < minInterval)
                    {
                        interval = minInterval;
                    }
                }

                // Start & Ende zeichnen
                text = new FormattedText(Minimum.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeFace, 8, TickBrush);
                drawingContext.DrawText(text, new Point(startPoint.X - text.Width * 0.5, startPoint.Y - text.Height * 0.5));
                text = new FormattedText(Maximum.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeFace, 8, TickBrush);
                drawingContext.DrawText(text, new Point(startPoint.X - text.Width * 0.5, endPoint.Y - text.Height * 0.5));

                for (var i = interval; i < range; i += interval)
                {
                    var y = i * logicalToPhysical + startPoint.Y;
                    text = new FormattedText(i.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeFace, 8, TickBrush);
                    drawingContext.DrawText(text, new Point(startPoint.X - text.Width * 0.5, y - text.Height * 0.5));
                }
            }
            else
            {
                // Interval verringern, wenn der verfügbare Platz überschritten wird
                var interval = TickFrequency;
                if (interval > 0.0)
                {
                    var minInterval = (Maximum - Minimum) / size.Width;
                    if (interval < minInterval)
                    {
                        interval = minInterval;
                    }
                }

                // Start & Ende zeichnen
                text = new FormattedText(Minimum.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeFace, 8, TickBrush);
                drawingContext.DrawText(text, new Point(startPoint.X - text.Width * 0.5, -2));
                text = new FormattedText(Maximum.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeFace, 8, TickBrush);
                drawingContext.DrawText(text, new Point(endPoint.X - text.Width * 0.5, -2));

                for (var i = interval; i < range; i += interval)
                {
                    var x = (i * logicalToPhysical) + startPoint.X;
                    text = new FormattedText(i.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeFace, 8, TickBrush);
                    drawingContext.DrawText(text, new Point(x - text.Width * 0.5, -2));
                }
            }
#pragma warning restore CS0618
        }
    }
}