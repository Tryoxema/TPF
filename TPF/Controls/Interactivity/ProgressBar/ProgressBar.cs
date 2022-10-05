using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace TPF.Controls
{
    public class ProgressBar : ProgressBarBase
    {
        public ProgressBar()
        {
            SizeChanged += (s, e) =>
            {
                var instance = (ProgressBar)s;
                instance.UpdateIndicators();
            };
        }

        static ProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(typeof(ProgressBar)));
        }

        #region GlowBrush DependencyProperty
        public static readonly DependencyProperty GlowBrushProperty = DependencyProperty.Register("GlowBrush",
            typeof(Brush),
            typeof(ProgressBar),
            new PropertyMetadata(Brushes.Transparent));

        public Brush GlowBrush
        {
            get { return (Brush)GetValue(GlowBrushProperty); }
            set { SetValue(GlowBrushProperty, value); }
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(ProgressBar),
            new PropertyMetadata(null));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region ContentPosition DependencyProperty
        public static readonly DependencyProperty ContentPositionProperty = DependencyProperty.Register("ContentPosition",
            typeof(ProgressBarContentPosition),
            typeof(ProgressBar),
            new PropertyMetadata(ProgressBarContentPosition.Center));

        public ProgressBarContentPosition ContentPosition
        {
            get { return (ProgressBarContentPosition)GetValue(ContentPositionProperty); }
            set { SetValue(ContentPositionProperty, value); }
        }
        #endregion

        internal Border Track;
        internal Border ProgressIndicator;
        internal Border SecondaryProgressIndicator;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Track = GetTemplateChild("PART_Track") as Border;
            ProgressIndicator = GetTemplateChild("PART_ProgressIndicator") as Border;
            SecondaryProgressIndicator = GetTemplateChild("PART_SecondaryProgressIndicator") as Border;

            UpdateIndicators();
        }

        protected override void OnMinimumChanged(double oldValue, double newValue)
        {
            base.OnMinimumChanged(oldValue, newValue);

            UpdateIndicators();
        }

        protected override void OnMaximumChanged(double oldValue, double newValue)
        {
            base.OnMaximumChanged(oldValue, newValue);

            UpdateIndicators();
        }

        protected override void OnProgressChanged(double oldValue, double newValue)
        {
            base.OnProgressChanged(oldValue, newValue);

            UpdateIndicators();
        }

        protected override void OnSecondaryProgressChanged(double oldValue, double newValue)
        {
            base.OnSecondaryProgressChanged(oldValue, newValue);

            UpdateIndicators();
        }

        private void UpdateIndicators()
        {
            if (Track == null || IsIndeterminate) return;

            // Gesamtwert zwischen Minimum und Maximum berechnen
            var totalRange = Maximum - Minimum;

            if (ProgressIndicator != null)
            {
                // Relativen Wert berechnen
                var relativeValue = Progress + Math.Abs(Minimum);
                // Faktor berechnen
                var factor = relativeValue / totalRange;

                ProgressIndicator.Width = Track.ActualWidth * factor;
            }

            if (SecondaryProgressIndicator != null)
            {
                // Relativen Wert berechnen
                var relativeValue = SecondaryProgress + Math.Abs(Minimum);
                // Faktor berechnen
                var factor = relativeValue / totalRange;

                SecondaryProgressIndicator.Width = Track.ActualWidth * factor;
            }
        }
    }
}