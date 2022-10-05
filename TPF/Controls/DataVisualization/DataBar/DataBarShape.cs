using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TPF.Controls.Specialized.DataBar
{
    public class DataBarShape : Shape
    {
        #region HeightFactor DependencyProperty
        public static readonly DependencyProperty HeightFactorProperty = DependencyProperty.Register("HeightFactor",
            typeof(double),
            typeof(DataBarShape),
            new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender, null, ConstrainDouble));

        public double HeightFactor
        {
            get { return (double)GetValue(HeightFactorProperty); }
            set { SetValue(HeightFactorProperty, value); }
        }
        #endregion

        #region Start DependencyProperty
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start",
            typeof(double),
            typeof(DataBarShape),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, null, ConstrainDouble));

        public double Start
        {
            get { return (double)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }
        #endregion

        #region End DependencyProperty
        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End",
            typeof(double),
            typeof(DataBarShape),
            new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender, null, ConstrainDouble));

        public double End
        {
            get { return (double)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }
        #endregion

        #region OutOfRangeState DependencyProperty
        public static readonly DependencyProperty OutOfRangeStateProperty = DependencyProperty.Register("OutOfRangeState",
            typeof(OutOfRangeState),
            typeof(DataBarShape),
            new FrameworkPropertyMetadata(OutOfRangeState.None, FrameworkPropertyMetadataOptions.AffectsRender));

        public OutOfRangeState OutOfRangeState
        {
            get { return (OutOfRangeState)GetValue(OutOfRangeStateProperty); }
            set { SetValue(OutOfRangeStateProperty, value); }
        }
        #endregion

        #region OutOfRangeMarkerType DependencyProperty
        public static readonly DependencyProperty OutOfRangeMarkerTypeProperty = DependencyProperty.Register("OutOfRangeMarkerType",
            typeof(OutOfRangeMarkerType),
            typeof(DataBarShape),
            new FrameworkPropertyMetadata(OutOfRangeMarkerType.Arrow, FrameworkPropertyMetadataOptions.AffectsRender));

        public OutOfRangeMarkerType OutOfRangeMarkerType
        {
            get { return (OutOfRangeMarkerType)GetValue(OutOfRangeMarkerTypeProperty); }
            set { SetValue(OutOfRangeMarkerTypeProperty, value); }
        }
        #endregion

        private static object ConstrainDouble(DependencyObject d, object baseValue)
        {
            var doubleValue = (double)baseValue;

            if (doubleValue < 0) doubleValue = 0;
            else if (doubleValue > 1) doubleValue = 1;

            return doubleValue;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            // Bei unendlich Platz gehen die Berechnungen kaputt, also hier einfach 0 nehmen
            if (double.IsInfinity(constraint.Height)) constraint.Height = 0;
            if (double.IsInfinity(constraint.Width)) constraint.Width = 0;

            return constraint;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                var geometry = new StreamGeometry();

                using (var context = geometry.Open())
                {
                    DrawGeometry(context);
                }

                // Freeze für Performance
                geometry.Freeze();

                return geometry;
            }
        }

        private void DrawGeometry(StreamGeometryContext context)
        {
            var heightFactor = HeightFactor;
            var start = Start;
            var end = End;

            var left = Math.Round(start * ActualWidth);
            var right = Math.Round(end * ActualWidth);
            var top = Math.Round((1 - heightFactor) / 2 * ActualHeight);
            var bottom = ActualHeight - top;

            left += StrokeThickness / 2.0;
            top += StrokeThickness / 2.0;
            bottom -= StrokeThickness / 2.0;
            right -= StrokeThickness / 2.0;

            var offset = (bottom - top) / 2;

            if (OutOfRangeState == OutOfRangeState.Overflow)
            {
                context.BeginFigure(new Point(left, top), true, true);
                // Schauen welche Art von OutOfRangeMarker gezeichnet werden soll
                if (OutOfRangeMarkerType == OutOfRangeMarkerType.Torn)
                {
                    var halfOffset = offset * 0.5;

                    context.LineTo(new Point(right, top), true, true);
                    if (left < right - halfOffset) context.LineTo(new Point(right - halfOffset, top + halfOffset), true, true);
                    context.LineTo(new Point(right, top + offset), true, true);
                    if (left < right - halfOffset) context.LineTo(new Point(right - halfOffset, top + (offset * 1.5)), true, true);
                    context.LineTo(new Point(right, bottom), true, true);
                }
                else
                {
                    if (left < right - offset) context.LineTo(new Point(right - offset, top), true, true);
                    context.LineTo(new Point(right, top + offset), true, true);
                    if (left < right - offset) context.LineTo(new Point(right - offset, bottom), true, true);
                }
                context.LineTo(new Point(left, bottom), true, true);
                context.LineTo(new Point(left, top), true, true);
            }
            else if (OutOfRangeState == OutOfRangeState.Underflow)
            {
                if (OutOfRangeMarkerType == OutOfRangeMarkerType.Torn)
                {
                    var halfOffset = offset * 0.5;

                    context.BeginFigure(new Point(left, bottom), true, true);
                    if (left + halfOffset < right) context.LineTo(new Point(left + halfOffset, bottom - halfOffset), true, true);
                    context.LineTo(new Point(left, top + offset), true, true);
                    if (left + halfOffset < right) context.LineTo(new Point(left + halfOffset, top + halfOffset), true, true);
                    context.LineTo(new Point(left, top), true, true);
                    context.LineTo(new Point(right, top), true, true);
                    context.LineTo(new Point(right, bottom), true, true);
                    context.LineTo(new Point(left, bottom), true, true);
                }
                else
                {
                    context.BeginFigure(new Point(left, top + offset), true, true);
                    if (left + offset < right) context.LineTo(new Point(left + offset, top), true, true);
                    context.LineTo(new Point(right, top), true, true);
                    context.LineTo(new Point(right, bottom), true, true);
                    if (left < right - offset) context.LineTo(new Point(left + offset, bottom), true, true);
                    context.LineTo(new Point(left, top + offset), true, true);
                }
            }
            else
            {
                context.BeginFigure(new Point(left, top), true, true);
                context.LineTo(new Point(right, top), true, true);
                context.LineTo(new Point(right, bottom), true, true);
                context.LineTo(new Point(left, bottom), true, true);
                context.LineTo(new Point(left, top), true, true);
            }
        }
    }
}