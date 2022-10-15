using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace TPF.Controls.Specialized.DataAxis
{
    public class DataAxisTicksControl : FrameworkElement
    {
        public DataAxisTicksControl()
        {
            Loaded += DataAxisTicksControl_Loaded;
        }

        #region Ticks DependencyProperty
        public static readonly DependencyProperty TicksProperty = DependencyProperty.Register("Ticks",
            typeof(List<DataAxisTick>),
            typeof(DataAxisTicksControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public List<DataAxisTick> Ticks
        {
            get { return (List<DataAxisTick>)GetValue(TicksProperty); }
            set { SetValue(TicksProperty, value); }
        }
        #endregion

        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(DataAxisTicksControl),
            new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region MajorTickLength DependencyProperty
        public static readonly DependencyProperty MajorTickLengthProperty = DependencyProperty.Register("MajorTickLength",
            typeof(double),
            typeof(DataAxisTicksControl),
            new FrameworkPropertyMetadata(6.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double MajorTickLength
        {
            get { return (double)GetValue(MajorTickLengthProperty); }
            set { SetValue(MajorTickLengthProperty, value); }
        }
        #endregion

        #region MinorTickLength DependencyProperty
        public static readonly DependencyProperty MinorTickLengthProperty = DependencyProperty.Register("MinorTickLength",
            typeof(double),
            typeof(DataAxisTicksControl),
            new FrameworkPropertyMetadata(4.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double MinorTickLength
        {
            get { return (double)GetValue(MinorTickLengthProperty); }
            set { SetValue(MinorTickLengthProperty, value); }
        }
        #endregion

        #region TickBrush DependencyProperty
        public static readonly DependencyProperty TickBrushProperty = DependencyProperty.Register("TickBrush",
            typeof(Brush),
            typeof(DataAxisTicksControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush TickBrush
        {
            get { return (Brush)GetValue(TickBrushProperty); }
            set { SetValue(TickBrushProperty, value); }
        }
        #endregion

        private Controls.DataAxis _dataAxis;

        private void DataAxisTicksControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= DataAxisTicksControl_Loaded;

            var dataAxis = this.ParentOfType<Controls.DataAxis>();

            if (dataAxis == null) return;

            BindToDataAxis(dataAxis);

            _dataAxis = dataAxis;
        }

        private void BindToDataAxis(Controls.DataAxis dataAxis)
        {
            SetBinding(TicksProperty, new Binding("Ticks") { Source = dataAxis });
            SetBinding(OrientationProperty, new Binding("Orientation") { Source = dataAxis });
            SetBinding(MajorTickLengthProperty, new Binding("MajorTickLength") { Source = dataAxis });
            SetBinding(MinorTickLengthProperty, new Binding("MinorTickLength") { Source = dataAxis });
            SetBinding(TickBrushProperty, new Binding("TickBrush") { Source = dataAxis });
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_dataAxis == null || Ticks == null || Ticks.Count == 0) return;

            var size = new Size(ActualWidth, ActualHeight);

            var pen = new Pen(TickBrush, 1.0d);

            Point startPoint, endPoint;

            if (Orientation == Orientation.Horizontal)
            {
                if (size.Width >= 1) size.Width--;
                startPoint = new Point(0, 0);
                endPoint = new Point(size.Width - 0.5, 0);
            }
            else
            {
                if (size.Height >= 1) size.Height--;
                startPoint = new Point(size.Width, size.Height);
                endPoint = new Point(size.Width, 0);
            }

            var snapsToDevicePixels = SnapsToDevicePixels;
            var xLines = snapsToDevicePixels ? new DoubleCollection() : null;
            var yLines = snapsToDevicePixels ? new DoubleCollection() : null;

            var ticks = Ticks;

            if (Orientation == Orientation.Horizontal)
            {
                if (snapsToDevicePixels)
                {
                    xLines.Add(startPoint.X - 0.5);
                    xLines.Add(endPoint.X - 0.5);
                    yLines.Add(startPoint.Y);
                    yLines.Add(endPoint.Y + MinorTickLength);
                    yLines.Add(endPoint.Y + MajorTickLength);
                }

                for (int i = 0; i < ticks.Count; i++)
                {
                    var tick = ticks[i];

                    var x = tick.NormalizedValue * size.Width;

                    // Aus irgendwelchen unbekannten Gründen muss der letzte Tick für das Zeichnen um 0.5 verschoben werden, da er sonst nicht korrekt dargestellt wird
                    if (tick.NormalizedValue == 1) x += 0.5;

                    var tickSize = tick.IsMajorTick ? MajorTickLength : MinorTickLength;

                    drawingContext.DrawLine(pen, new Point(x, startPoint.Y), new Point(x, startPoint.Y + tickSize));

                    if (snapsToDevicePixels) xLines.Add(x - 0.5);
                }
            }
            else
            {
                if (snapsToDevicePixels)
                {
                    xLines.Add(startPoint.X);
                    xLines.Add(startPoint.X - MajorTickLength);
                    xLines.Add(startPoint.X - MinorTickLength);
                    yLines.Add(startPoint.Y - 0.5);
                    yLines.Add(endPoint.Y - 0.5);
                }

                for (int i = 0; i < ticks.Count; i++)
                {
                    var tick = ticks[i];

                    var y = size.Height - tick.NormalizedValue * size.Height;

                    // Aus irgendwelchen unbekannten Gründen muss der erste Tick für das Zeichnen um 0.5 verschoben werden, da er sonst nicht korrekt dargestellt wird
                    if (tick.NormalizedValue == 0) y += 0.5;

                    var tickSize = tick.IsMajorTick ? MajorTickLength : MinorTickLength;

                    drawingContext.DrawLine(pen, new Point(startPoint.X, y), new Point(startPoint.X - tickSize, y));

                    if (snapsToDevicePixels) yLines.Add(y - 0.5);
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

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Ticks == null || Ticks.Count == 0) return base.MeasureOverride(availableSize);

            var hasMajorTick = Ticks.Any(x => x.IsMajorTick);

            var resultSize = new Size(availableSize.Width, availableSize.Height);

            if (Orientation == Orientation.Horizontal)
            {
                resultSize.Height = hasMajorTick ? MajorTickLength : MinorTickLength;
            }
            else
            {
                resultSize.Width = hasMajorTick ? MajorTickLength : MinorTickLength;
            }

            if (double.IsInfinity(resultSize.Width)) resultSize.Width = 0;
            if (double.IsInfinity(resultSize.Height)) resultSize.Height = 0;

            return resultSize;
        }
    }
}