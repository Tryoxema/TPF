using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TPF.Internal;

namespace TPF.Controls
{
    [TemplatePart(Name = "PART_BackgroundFigure", Type = typeof(PathFigure))]
    [TemplatePart(Name = "PART_BackgroundArc", Type = typeof(ArcSegment))]
    [TemplatePart(Name = "PART_GlowFigure", Type = typeof(PathFigure))]
    [TemplatePart(Name = "PART_GlowArc", Type = typeof(ArcSegment))]
    [TemplatePart(Name = "PART_ValueFigure", Type = typeof(PathFigure))]
    [TemplatePart(Name = "PART_ValueArc", Type = typeof(ArcSegment))]
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
            // Alles über 360 Grad ergibt keinen sinn, also kürzen
            while (availableAngle > 360.0) availableAngle -= 360.0;

            var startAngle = StartAngle;

            // Bei weniger als 360 Grad müssen wir 25% des verfügbaren Bereichs vor dem Anfang simulieren, damit der Balken nicht einfach komplett sichtbar startet
            if (availableAngle < 360.0)
            {
                availableAngle *= 1.25;
                startAngle -= availableAngle * 0.25;
            }

            // Start und Ende berechnen
            var start = startAngle + availableAngle * AnimationProgressFactor;
            var end = start + availableAngle * 0.25;

            // Bei weniger als 360 Grad start und ende an den verfügbaren Bereich angleichen
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

            // Ist es ein vollständiger Kreis?
            if (angleDelta >= 360)
            {
                // Da ArcTo bei 360 Grad gar nicht zeichnet, müssen 2 halbe Kreise gemalt werden
                var outerArcTopPoint = Helper.ComputeCartesianCoordinate(arcCenter, calculationStartAngle, calculationOuterRadius);
                var outerArcBottomPoint = Helper.ComputeCartesianCoordinate(arcCenter, calculationStartAngle + 180, calculationOuterRadius);
                var innerArcTopPoint = Helper.ComputeCartesianCoordinate(arcCenter, calculationStartAngle, calculationInnerRadius);
                var innerArcBottomPoint = Helper.ComputeCartesianCoordinate(arcCenter, calculationStartAngle + 180, calculationInnerRadius);

                context.BeginFigure(innerArcTopPoint, true, true);
                context.LineTo(outerArcTopPoint, true, true);
                context.ArcTo(outerArcBottomPoint, outerArcSize, 0, false, SweepDirection.Clockwise, true, true);
                context.ArcTo(outerArcTopPoint, outerArcSize, 0, false, SweepDirection.Clockwise, true, true);
                context.LineTo(innerArcTopPoint, true, true);
                context.ArcTo(innerArcBottomPoint, innerArcSize, 0, false, SweepDirection.Counterclockwise, true, true);
                context.ArcTo(innerArcTopPoint, innerArcSize, 0, false, SweepDirection.Counterclockwise, true, true);
            }
            else
            {
                var outerArcStartPoint = Helper.ComputeCartesianCoordinate(arcCenter, calculationStartAngle, calculationOuterRadius);
                var outerArcEndPoint = Helper.ComputeCartesianCoordinate(arcCenter, calculationStartAngle + angleDelta, calculationOuterRadius);
                var innerArcStartPoint = Helper.ComputeCartesianCoordinate(arcCenter, calculationStartAngle, calculationInnerRadius);
                var innerArcEndPoint = Helper.ComputeCartesianCoordinate(arcCenter, calculationStartAngle + angleDelta, calculationInnerRadius);

                var largeArc = angleDelta > 180.0;

                context.BeginFigure(innerArcStartPoint, true, true);
                context.LineTo(outerArcStartPoint, true, true);
                context.ArcTo(outerArcEndPoint, outerArcSize, 0, largeArc, SweepDirection.Clockwise, true, true);
                context.LineTo(innerArcEndPoint, true, true);
                context.ArcTo(innerArcStartPoint, innerArcSize, 0, largeArc, SweepDirection.Counterclockwise, true, true);
            }
        }
    }
}