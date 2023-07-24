using System.Windows;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using TPF.Internal;

namespace TPF.Controls
{
    public class TickBar : FrameworkElement
    {
        static TickBar()
        {
            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(TickBar), new FrameworkPropertyMetadata(true));
        }

        #region Ticks DependencyProperty
        public static readonly DependencyProperty TicksProperty = DependencyProperty.Register("Ticks",
            typeof(List<SliderTick>),
            typeof(TickBar),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public List<SliderTick> Ticks
        {
            get { return (List<SliderTick>)GetValue(TicksProperty); }
            set { SetValue(TicksProperty, value); }
        }
        #endregion

        #region TickBrush DependencyProperty
        public static readonly DependencyProperty TickBrushProperty = Slider.TickBrushProperty.AddOwner(typeof(TickBar),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush TickBrush
        {
            get { return (Brush)GetValue(TickBrushProperty); }
            set { SetValue(TickBrushProperty, value); }
        }
        #endregion

        #region MinorTickBrush DependencyProperty
        public static readonly DependencyProperty MinorTickBrushProperty = Slider.MinorTickBrushProperty.AddOwner(typeof(TickBar),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush MinorTickBrush
        {
            get { return (Brush)GetValue(MinorTickBrushProperty); }
            set { SetValue(MinorTickBrushProperty, value); }
        }
        #endregion

        #region ActiveTickBrush DependencyProperty
        public static readonly DependencyProperty ActiveTickBrushProperty = Slider.ActiveTickBrushProperty.AddOwner(typeof(TickBar),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ActiveTickBrush
        {
            get { return (Brush)GetValue(ActiveTickBrushProperty); }
            set { SetValue(ActiveTickBrushProperty, value); }
        }
        #endregion

        #region ActiveMinorTickBrush DependencyProperty
        public static readonly DependencyProperty ActiveMinorTickBrushProperty = Slider.ActiveMinorTickBrushProperty.AddOwner(typeof(TickBar),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ActiveMinorTickBrush
        {
            get { return (Brush)GetValue(ActiveMinorTickBrushProperty); }
            set { SetValue(ActiveMinorTickBrushProperty, value); }
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

        #region Placement DependencyProperty
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement",
            typeof(TickBarPlacement),
            typeof(TickBar),
            new FrameworkPropertyMetadata(TickBarPlacement.Top, FrameworkPropertyMetadataOptions.AffectsRender));

        public TickBarPlacement Placement
        {
            get { return (TickBarPlacement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }
        #endregion

        private readonly Dictionary<Brush, Pen> _penCache = new Dictionary<Brush, Pen>();

        protected override void OnRender(DrawingContext drawingContext)
        {
            DrawTicks(drawingContext);
        }

        private void DrawTicks(DrawingContext drawingContext)
        {
            var ticks = Ticks;

            if (ticks == null || ticks.Count == 0) return;

            var size = new Size(ActualWidth, ActualHeight);
            var tickLength = 0.0;
            double minorTickLength;
            var startPoint = new Point(0.0, 0.0);
            var endPoint = new Point(0.0, 0.0);

            switch (Placement)
            {
                case TickBarPlacement.Left:
                {
                    tickLength = -size.Width;
                    startPoint = new Point(size.Width, size.Height);
                    endPoint = new Point(size.Width, 0);
                    break;
                }
                case TickBarPlacement.Top:
                {
                    tickLength = -size.Height;
                    startPoint = new Point(0, size.Height);
                    endPoint = new Point(0 + size.Width, size.Height);
                    break;
                }
                case TickBarPlacement.Right:
                {
                    tickLength = size.Width;
                    startPoint = new Point(0d, size.Height + 0);
                    endPoint = new Point(0d, 0);
                    break;
                }
                case TickBarPlacement.Bottom:
                {
                    tickLength = size.Height;
                    startPoint = new Point(0, 0d);
                    endPoint = new Point(0 + size.Width, 0d);
                    break;
                }
            }

            minorTickLength = tickLength * 0.5;

            // Richtung Invertieren
            if (IsDirectionReversed)
            {
                // startPoint & endPoint tauschen
                var point = startPoint;
                startPoint = endPoint;
                endPoint = point;
            }

            var snapsToDevicePixels = SnapsToDevicePixels;
            var xLines = snapsToDevicePixels ? new DoubleCollection() : null;
            var yLines = snapsToDevicePixels ? new DoubleCollection() : null;

            if ((Placement == TickBarPlacement.Left) || (Placement == TickBarPlacement.Right))
            {
                if (snapsToDevicePixels)
                {
                    xLines.Add(startPoint.X);
                    xLines.Add(startPoint.X + tickLength);
                    //yLines.Add(startPoint.Y - 0.5);
                    //yLines.Add(endPoint.Y - 0.5);
                    xLines.Add(startPoint.X + minorTickLength);
                }

                for (int i = 0; i < ticks.Count; i++)
                {
                    var tick = ticks[i];

                    var pen = GetPenForTick(tick);

                    if (pen == null) continue;

                    var y = size.Height - ((IsDirectionReversed ? 1 - tick.NormalizedValue : tick.NormalizedValue) * size.Height);

                    // Aus irgendwelchen unbekannten Gründen muss der erste Tick für das Zeichnen um 0.5 verschoben werden, da er sonst nicht korrekt dargestellt wird
                    if (IsDirectionReversed && tick.NormalizedValue == 1 || tick.NormalizedValue == 0) y += 0.5;

                    var tickSize = tick.IsMajorTick ? tickLength : minorTickLength;

                    drawingContext.DrawLine(pen, new Point(startPoint.X, y), new Point(startPoint.X + tickSize, y));

                    if (snapsToDevicePixels) yLines.Add(y - 0.5);
                }
            }
            else
            {
                if (snapsToDevicePixels)
                {
                    yLines.Add(startPoint.Y);
                    yLines.Add(endPoint.Y + tickLength);
                    yLines.Add(endPoint.Y + minorTickLength);
                }

                for (int i = 0; i < ticks.Count; i++)
                {
                    var tick = ticks[i];

                    var pen = GetPenForTick(tick);

                    if (pen == null) continue;

                    var x = (IsDirectionReversed ? 1 - tick.NormalizedValue : tick.NormalizedValue) * size.Width;

                    // Aus irgendwelchen unbekannten Gründen muss der letzte Tick für das Zeichnen um 0.5 verschoben werden, da er sonst nicht korrekt dargestellt wird
                    if (IsDirectionReversed && tick.NormalizedValue == 0 || tick.NormalizedValue == 1) x -= 0.5;

                    var tickSize = tick.IsMajorTick ? tickLength : minorTickLength;

                    drawingContext.DrawLine(pen, new Point(x, startPoint.Y), new Point(x, startPoint.Y + tickSize));

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

        private Pen GetPenForTick(SliderTick tick)
        {
            var tickBrush = TickBrush;

            if (!tick.IsMajorTick)
            {
                if (tick.IsActive) tickBrush = ActiveMinorTickBrush ?? ActiveTickBrush ?? MinorTickBrush;
                else tickBrush = MinorTickBrush;
            }
            else if (tick.IsActive) tickBrush = ActiveTickBrush;

            if (tickBrush == null) tickBrush = TickBrush;

            if (tickBrush == null) return null;

            if (!_penCache.TryGetValue(tickBrush, out var pen))
            {
                pen = new Pen(tickBrush, 1);
                _penCache.Add(tickBrush, pen);
            }

            return pen;
        }
    }
}