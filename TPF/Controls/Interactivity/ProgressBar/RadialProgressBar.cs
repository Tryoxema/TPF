using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TPF.Internal;

namespace TPF.Controls
{
    [TemplatePart(Name = "PART_Track", Type = typeof(Path))]
    [TemplatePart(Name = "PART_SecondaryProgress", Type = typeof(Path))]
    [TemplatePart(Name = "PART_Progress", Type = typeof(Path))]
    public class RadialProgressBar : ProgressBarBase
    {
        public RadialProgressBar()
        {
            SizeChanged += (s, e) =>
            {
                var instance = (RadialProgressBar)s;
                instance.RenderAll();
            };
        }

        static RadialProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadialProgressBar), new FrameworkPropertyMetadata(typeof(RadialProgressBar)));
        }

        #region StartAngle DependencyProperty
        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle",
            typeof(double),
            typeof(RadialProgressBar),
            new PropertyMetadata(180.0, OnAngleChanged, ConstrainStartAngle));

        private static object ConstrainStartAngle(DependencyObject sender, object value)
        {
            var instance = (RadialProgressBar)sender;

            var doubleValue = (double)value;

            if (doubleValue < 0.0) doubleValue = 0.0;
            if (doubleValue > instance.EndAngle) doubleValue = instance.EndAngle;

            return doubleValue;
        }

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }
        #endregion

        #region EndAngle DependencyProperty
        public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register("EndAngle",
            typeof(double),
            typeof(RadialProgressBar),
            new PropertyMetadata(540.0, OnAngleChanged, ConstrainEndAngle));

        private static object ConstrainEndAngle(DependencyObject sender, object value)
        {
            var instance = (RadialProgressBar)sender;

            var doubleValue = (double)value;

            if (doubleValue < 0.0) doubleValue = 0.0;
            if (doubleValue < instance.StartAngle) doubleValue = instance.StartAngle;

            return doubleValue;
        }

        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }
        #endregion

        #region TrackOuterRadius DependencyProperty
        public static readonly DependencyProperty TrackOuterRadiusProperty = DependencyProperty.Register("TrackOuterRadius",
            typeof(double),
            typeof(RadialProgressBar),
            new PropertyMetadata(1.0, OnTrackChanged, ConstrainTrackOuterRadius));

        private static object ConstrainTrackOuterRadius(DependencyObject sender, object value)
        {
            var instance = (RadialProgressBar)sender;

            var doubleValue = (double)value;

            if (doubleValue < 0.0) doubleValue = 0.0;
            else if (doubleValue > 1.0) doubleValue = 1.0;

            if (doubleValue < instance.TrackInnerRadius) doubleValue = instance.TrackInnerRadius;

            return doubleValue;
        }

        public double TrackOuterRadius
        {
            get { return (double)GetValue(TrackOuterRadiusProperty); }
            set { SetValue(TrackOuterRadiusProperty, value); }
        }
        #endregion

        #region TrackInnerRadius DependencyProperty
        public static readonly DependencyProperty TrackInnerRadiusProperty = DependencyProperty.Register("TrackInnerRadius",
            typeof(double),
            typeof(RadialProgressBar),
            new PropertyMetadata(0.8, OnTrackChanged, ConstrainTrackInnerRadius));

        private static object ConstrainTrackInnerRadius(DependencyObject sender, object value)
        {
            var instance = (RadialProgressBar)sender;

            var doubleValue = (double)value;

            if (doubleValue < 0.0) doubleValue = 0.0;
            else if (doubleValue > 1.0) doubleValue = 1.0;

            if (doubleValue > instance.TrackOuterRadius) doubleValue = instance.TrackOuterRadius;

            return doubleValue;
        }

        public double TrackInnerRadius
        {
            get { return (double)GetValue(TrackInnerRadiusProperty); }
            set { SetValue(TrackInnerRadiusProperty, value); }
        }
        #endregion

        #region IndicatorOuterRadius DependencyProperty
        public static readonly DependencyProperty IndicatorOuterRadiusProperty = DependencyProperty.Register("IndicatorOuterRadius",
            typeof(double),
            typeof(RadialProgressBar),
            new PropertyMetadata(1.0, OnIndicatorChanged, ConstrainIndicatorOuterRadius));

        private static object ConstrainIndicatorOuterRadius(DependencyObject sender, object value)
        {
            var instance = (RadialProgressBar)sender;

            var doubleValue = (double)value;

            if (doubleValue < 0.0) doubleValue = 0.0;
            else if (doubleValue > 1.0) doubleValue = 1.0;

            if (doubleValue < instance.IndicatorInnerRadius) doubleValue = instance.IndicatorInnerRadius;

            return doubleValue;
        }

        public double IndicatorOuterRadius
        {
            get { return (double)GetValue(IndicatorOuterRadiusProperty); }
            set { SetValue(IndicatorOuterRadiusProperty, value); }
        }
        #endregion

        #region IndicatorInnerRadius DependencyProperty
        public static readonly DependencyProperty IndicatorInnerRadiusProperty = DependencyProperty.Register("IndicatorInnerRadius",
            typeof(double),
            typeof(RadialProgressBar),
            new PropertyMetadata(0.8, OnIndicatorChanged, ConstrainIndicatorInnerRadius));

        private static object ConstrainIndicatorInnerRadius(DependencyObject sender, object value)
        {
            var instance = (RadialProgressBar)sender;

            var doubleValue = (double)value;

            if (doubleValue < 0.0) doubleValue = 0.0;
            else if (doubleValue > 1.0) doubleValue = 1.0;

            if (doubleValue > instance.IndicatorOuterRadius) doubleValue = instance.IndicatorOuterRadius;

            return doubleValue;
        }

        public double IndicatorInnerRadius
        {
            get { return (double)GetValue(IndicatorInnerRadiusProperty); }
            set { SetValue(IndicatorInnerRadiusProperty, value); }
        }
        #endregion

        #region AnimationProgressFactor DependencyProperty
        private static readonly DependencyProperty AnimationProgressFactorProperty = DependencyProperty.Register("AnimationProgressFactor",
            typeof(double),
            typeof(RadialProgressBar),
            new PropertyMetadata(0.0, OnAnimationProgressFactorChanged));

        private static void OnAnimationProgressFactorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (RadialProgressBar)sender;

            instance.OnAnimationProgressFactorChanged();
        }

        private double AnimationProgressFactor
        {
            get { return (double)GetValue(AnimationProgressFactorProperty); }
            set { SetValue(AnimationProgressFactorProperty, value); }
        }
        #endregion

        private static void OnAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (RadialProgressBar)sender;

            instance.RenderAll();
        }

        private static void OnTrackChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (RadialProgressBar)sender;

            instance.RenderTrack();
        }

        private static void OnIndicatorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (RadialProgressBar)sender;

            instance.RenderAll();
        }

        private Path TrackPath;
        private Path ProgressPath;
        private Path SecondaryProgressPath;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TrackPath = (Path)GetTemplateChild("PART_Track");
            ProgressPath = (Path)GetTemplateChild("PART_Progress");
            SecondaryProgressPath = (Path)GetTemplateChild("PART_SecondaryProgress");

            RenderAll();
            if (IsIndeterminate) StartAnimation();
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            RenderProgress();
            RenderSecondaryProgress();
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            RenderProgress();
            RenderSecondaryProgress();
        }

        protected override void OnProgressChanged(double oldValue, double newValue)
        {
            base.OnProgressChanged(oldValue, newValue);
            RenderProgress();
        }

        protected override void OnSecondaryProgressChanged(double oldValue, double newValue)
        {
            base.OnSecondaryProgressChanged(oldValue, newValue);
            RenderSecondaryProgress();
        }

        protected override void OnIsIndeterminateChanged(bool oldValue, bool newValue)
        {
            base.OnIsIndeterminateChanged(oldValue, newValue);

            if (oldValue) StopAnimation();

            if (newValue) StartAnimation();
        }

        protected override void OnSegmentCountChanged(int oldValue, int newValue)
        {
            base.OnSegmentCountChanged(oldValue, newValue);

            RenderAll();
        }

        protected override void OnGapWidthChanged(double oldValue, double newValue)
        {
            base.OnGapWidthChanged(oldValue, newValue);

            RenderAll();
        }

        private void StartAnimation()
        {
            // Falls aus irgend einem Grund eine Animation läuft, diese entfernen
            BeginAnimation(AnimationProgressFactorProperty, null);

            if (SecondaryProgressPath != null) SecondaryProgressPath.Data = null;

            var animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1))
            {
                RepeatBehavior = RepeatBehavior.Forever
            };

            BeginAnimation(AnimationProgressFactorProperty, animation);
        }

        private void StopAnimation()
        {
            BeginAnimation(AnimationProgressFactorProperty, null);
            RenderProgress();
            RenderSecondaryProgress();
        }

        private void OnAnimationProgressFactorChanged()
        {
            if (ProgressPath == null) return;

            var availableAngle = EndAngle - StartAngle;
            // Alles über 360 Grad ergibt keinen Sinn, also kürzen
            while (availableAngle > 360.0) availableAngle -= 360.0;

            // Für die Berechnung wird bei weniger als 360 Grad ein zweiter Wert benötigt
            // Wenn man alles mit einem Wert rechnet ist entweder der Anfang oder das Ende nicht korrekt
            var calculationAngle = availableAngle;

            var startAngle = StartAngle;

            // Bei weniger als 360 Grad müssen wir 25% des verfügbaren Bereichs vor dem Anfang simulieren, damit der Balken nicht einfach komplett sichtbar startet
            if (availableAngle < 360.0)
            {
                calculationAngle *= 1.25;
                startAngle -= availableAngle * 0.25;
            }

            // Start und Ende berechnen
            var start = startAngle + calculationAngle * AnimationProgressFactor;
            var end = start + availableAngle * 0.25;

            // Bei weniger als 360 Grad Start und Ende an den verfügbaren Bereich angleichen
            if (availableAngle < 360.0)
            {
                start = Math.Max(StartAngle, start);
                end = Math.Min(EndAngle, end);
            }

            var geometry = new StreamGeometry { FillRule = FillRule.EvenOdd };

            using (var context = geometry.Open())
            {
                DrawFigure(context, IndicatorOuterRadius, IndicatorInnerRadius, start, end);
            }

            // Freeze für Performance
            geometry.Freeze();

            ProgressPath.Data = geometry;
        }

        private void RenderAll()
        {
            RenderTrack();
            RenderProgress();
            RenderSecondaryProgress();
        }

        private void RenderTrack()
        {
            if (TrackPath == null) return;

            var geometry = new StreamGeometry { FillRule = FillRule.EvenOdd };

            using (var context = geometry.Open())
            {
                DrawFigure(context, TrackOuterRadius, TrackInnerRadius, StartAngle, EndAngle);
            }

            // Freeze für Performance
            geometry.Freeze();

            TrackPath.Data = geometry;
        }

        private void RenderProgress()
        {
            if (ProgressPath == null || IsIndeterminate) return;

            var endAngle = CalculateEndAngle(Progress);

            var geometry = new StreamGeometry { FillRule = FillRule.EvenOdd };

            using (var context = geometry.Open())
            {
                DrawFigure(context, IndicatorOuterRadius, IndicatorInnerRadius, StartAngle, endAngle);
            }

            // Freeze für Performance
            geometry.Freeze();

            ProgressPath.Data = geometry;
        }

        private void RenderSecondaryProgress()
        {
            if (SecondaryProgressPath == null || IsIndeterminate) return;

            var endAngle = CalculateEndAngle(SecondaryProgress);

            var geometry = new StreamGeometry { FillRule = FillRule.EvenOdd };

            using (var context = geometry.Open())
            {
                DrawFigure(context, IndicatorOuterRadius, IndicatorInnerRadius, StartAngle, endAngle);
            }

            // Freeze für Performance
            geometry.Freeze();

            SecondaryProgressPath.Data = geometry;
        }

        private double CalculateEndAngle(double progress)
        {
            // Gesamtwert zwischen Minimum und Maximum berechnen
            var totalRange = Maximum - Minimum;

            // Relativen Wert berechnen
            var relativeValue = progress + Math.Abs(Minimum);
            // Faktor berechnen
            var factor = relativeValue / totalRange;

            var availableAngle = EndAngle - StartAngle;

            // Alles über 360 Grad ergibt keinen sinn, also kürzen
            while (availableAngle > 360.0) availableAngle -= 360.0;

            var endAngle = StartAngle + availableAngle * factor;

            return endAngle;
        }

        private static double GetCalculationAngle(double angle)
        {
            // Für das Zeichnen wird oben Mitte als 0 angesehen, für die Angle-Properties unten Mitte
            // Der Wert muss also um 180 Grad gedreht werden
            angle += 180.0;

            return angle;
        }

        private void DrawFigure(StreamGeometryContext context, double outerRadius, double innerRadius, double startAngle, double endAngle)
        {
            if (startAngle == endAngle) return;

            // Immer mindestens ein Segment generieren
            var segmentCount = Math.Max(1, SegmentCount);

            var barStartAngle = GetCalculationAngle(StartAngle);
            var calculationStartAngle = GetCalculationAngle(startAngle);
            var calculationEndAngle = GetCalculationAngle(endAngle);
            var angleDelta = calculationEndAngle - calculationStartAngle;

            // Alles über 360 Grad verkompliziert nur die Berechnung, also wird es auf 360 Grad beschränkt
            while (angleDelta > 360.0) angleDelta -= 360;

            var radius = ActualWidth / 2.0;
            var calculationOuterRadius = radius * outerRadius;
            var calculationInnerRadius = radius * innerRadius;

            var arcCenter = new Point(radius, radius);
            var outerArcSize = new Size(calculationOuterRadius, calculationOuterRadius);
            var innerArcSize = new Size(calculationInnerRadius, calculationInnerRadius);

            // Ist es ein vollständiger Kreis in einem Segment?
            if (angleDelta >= 360 && segmentCount == 1)
            {
                // Da ArcTo bei 360 Grad gar nicht zeichnet, müssen 2 halbe Kreise gemalt werden
                var outerArcTopPoint = Helper.ComputeCartesianCoordinate(arcCenter, calculationStartAngle, calculationOuterRadius);
                var outerArcBottomPoint = Helper.ComputeCartesianCoordinate(arcCenter, calculationStartAngle + 180, calculationOuterRadius);
                var innerArcTopPoint = Helper.ComputeCartesianCoordinate(arcCenter, calculationStartAngle, calculationInnerRadius);
                var innerArcBottomPoint = Helper.ComputeCartesianCoordinate(arcCenter, calculationStartAngle + 180, calculationInnerRadius);

                // Zeichnen
                context.BeginFigure(innerArcTopPoint, true, true);
                context.LineTo(outerArcTopPoint, false, true);
                context.ArcTo(outerArcBottomPoint, outerArcSize, 0, false, SweepDirection.Clockwise, true, true);
                context.ArcTo(outerArcTopPoint, outerArcSize, 0, false, SweepDirection.Clockwise, true, true);
                context.LineTo(innerArcTopPoint, false, true);
                context.ArcTo(innerArcBottomPoint, innerArcSize, 0, false, SweepDirection.Counterclockwise, true, true);
                context.ArcTo(innerArcTopPoint, innerArcSize, 0, false, SweepDirection.Counterclockwise, true, true);
            }
            else
            {
                var totalAngleDelta = EndAngle - StartAngle;
                // Alles über 360 Grad verkompliziert nur die Berechnung, also wird es auf 360 Grad beschränkt
                while (totalAngleDelta > 360.0) totalAngleDelta -= 360;

                var angleDeltaPerSegment = totalAngleDelta / segmentCount;
                var halfGapAngle = 0d;

                // Bei mehr als einem Segment den Abstand ausrechnen
                if (segmentCount > 1)
                {
                    var angleFactor = angleDelta / 360;
                    var circumference = ActualWidth * angleFactor * Math.PI;
                    var gapWidth = Math.Max(0, GapWidth);
                    var gapFactor = gapWidth / circumference;
                    var gapAngle = angleDelta * gapFactor;
                    halfGapAngle = gapAngle / 2;
                }

                for (int i = 0; i < segmentCount; i++)
                {
                    var segmentStart = barStartAngle + angleDeltaPerSegment * i;
                    var segmentEnd = segmentStart + angleDeltaPerSegment;

                    double valueStart;
                    var valueEnd = calculationEndAngle < segmentEnd ? calculationEndAngle : segmentEnd;

                    if (IsIndeterminate)
                    {
                        // Wenn der zu zeichnende Bereich noch nicht erreicht wurde, können wir das Segment überspringen
                        if (calculationEndAngle <= segmentStart) continue;

                        valueStart = calculationStartAngle > segmentStart ? calculationStartAngle : segmentStart;

                        if (calculationEndAngle > barStartAngle + 360 && totalAngleDelta == 360)
                        {
                            // -= 360 funktioniert bei nur einem Segment
                            // bei mehreren Segmenten funktioniert muss anders gerechnet werden
                            if (segmentCount == 1)
                            {
                                valueStart -= 360;
                                valueEnd = calculationEndAngle - 360 < segmentEnd ? calculationEndAngle - 360 : segmentEnd;
                            }
                            else if (valueStart > segmentEnd)
                            {
                                valueStart = Math.Max(segmentStart, valueStart - 360);
                                valueEnd = calculationEndAngle - 360 < segmentEnd ? calculationEndAngle - 360 : segmentEnd;
                            }
                        }
                        // Wenn der Start nach unserer Umrechnung nach dem Ende des aktuellen Segments kommt, können wir das überspringen
                        if (valueStart > segmentEnd) continue;
                    }
                    else
                    {
                        // Wenn der zu zeichnende Bereich noch nicht erreicht wurde oder überschritten wurde, können wir das Segment überspringen
                        if (calculationStartAngle > segmentEnd || calculationEndAngle <= segmentStart) continue;

                        valueStart = segmentStart;
                    }

                    var adjustedValueStart = valueStart + halfGapAngle;
                    // Faktor der Distanz zwischen Start und Ende
                    var factor = (valueEnd - valueStart) / angleDeltaPerSegment;
                    // Anhand des tatsächlichen Starts und des Faktors des Segments den korrekten Endpunkt bestimmen
                    var adjustedValueEnd = Math.Min(adjustedValueStart + (factor * (angleDeltaPerSegment - (2 * halfGapAngle))), segmentEnd - halfGapAngle);

                    // Wenn durch die Rechnung ein Ende vor dem Start liegt, können wir das Segment überspringen
                    if (adjustedValueEnd < adjustedValueStart) continue;

                    var outerArcTopPoint = Helper.ComputeCartesianCoordinate(arcCenter, adjustedValueStart, calculationOuterRadius);
                    var outerArcBottomPoint = Helper.ComputeCartesianCoordinate(arcCenter, adjustedValueEnd, calculationOuterRadius);
                    var innerArcTopPoint = Helper.ComputeCartesianCoordinate(arcCenter, adjustedValueStart, calculationInnerRadius);
                    var innerArcBottomPoint = Helper.ComputeCartesianCoordinate(arcCenter, adjustedValueEnd, calculationInnerRadius);

                    // Wenn der Wert mehr als 180 Grad ist, muss ArcTo gesagt werden, dass es sich um einen großen Bogen handelt
                    var isLargeArc = adjustedValueEnd - adjustedValueStart > 180.0;

                    // Zeichnen
                    context.BeginFigure(outerArcTopPoint, true, true);
                    context.ArcTo(outerArcBottomPoint, outerArcSize, 0, isLargeArc, SweepDirection.Clockwise, true, true);
                    context.LineTo(innerArcBottomPoint, true, true);
                    context.ArcTo(innerArcTopPoint, innerArcSize, 0, isLargeArc, SweepDirection.Counterclockwise, true, true);
                    context.LineTo(outerArcTopPoint, true, true);
                }
            }
        }
    }
}