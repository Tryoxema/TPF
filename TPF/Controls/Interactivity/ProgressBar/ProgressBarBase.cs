using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public abstract class ProgressBarBase : ContentControl
    {
        #region Minimum DependencyProperty
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum",
            typeof(double),
            typeof(ProgressBarBase),
            new PropertyMetadata(0.0, OnMinimumChanged, ConstrainMinimum));

        private static object ConstrainMinimum(DependencyObject sender, object value)
        {
            var instance = (ProgressBarBase)sender;

            var doubleValue = (double)value;

            if (doubleValue > instance.Maximum) doubleValue = instance.Maximum;

            return doubleValue;
        }

        private static void OnMinimumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ProgressBarBase)sender;

            instance.OnMinimumChanged((double)e.OldValue, (double)e.NewValue);
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        #endregion

        #region Maximum DependencyProperty
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum",
            typeof(double),
            typeof(ProgressBarBase),
            new PropertyMetadata(100.0, OnMaximumChanged, ConstrainMaximum));

        private static object ConstrainMaximum(DependencyObject sender, object value)
        {
            var instance = (ProgressBarBase)sender;

            var doubleValue = (double)value;

            if (doubleValue < instance.Minimum) doubleValue = instance.Minimum;

            return doubleValue;
        }

        private static void OnMaximumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ProgressBarBase)sender;

            instance.OnMaximumChanged((double)e.OldValue, (double)e.NewValue);
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        #endregion

        #region Progress DependencyProperty
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress",
            typeof(double),
            typeof(ProgressBarBase),
            new PropertyMetadata(0.0, OnProgressChanged, ConstrainProgress));

        private static object ConstrainProgress(DependencyObject sender, object value)
        {
            var instance = (ProgressBarBase)sender;

            var doubleValue = (double)value;

            if (doubleValue < instance.Minimum) doubleValue = instance.Minimum;
            else if (doubleValue > instance.Maximum) doubleValue = instance.Maximum;

            return doubleValue;
        }

        private static void OnProgressChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ProgressBarBase)sender;

            instance.OnProgressChanged((double)e.OldValue, (double)e.NewValue);
        }

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        #endregion

        #region SecondaryProgress DependencyProperty
        public static readonly DependencyProperty SecondaryProgressProperty = DependencyProperty.Register("SecondaryProgress",
            typeof(double),
            typeof(ProgressBarBase),
            new PropertyMetadata(0.0, OnSecondaryProgressChanged, ConstrainProgress));

        private static void OnSecondaryProgressChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ProgressBarBase)sender;

            instance.OnSecondaryProgressChanged((double)e.OldValue, (double)e.NewValue);
        }

        public double SecondaryProgress
        {
            get { return (double)GetValue(SecondaryProgressProperty); }
            set { SetValue(SecondaryProgressProperty, value); }
        }
        #endregion

        #region ProgressBrush DependencyProperty
        public static readonly DependencyProperty ProgressBrushProperty = DependencyProperty.Register("ProgressBrush",
            typeof(Brush),
            typeof(ProgressBarBase),
            new PropertyMetadata(null));

        public Brush ProgressBrush
        {
            get { return (Brush)GetValue(ProgressBrushProperty); }
            set { SetValue(ProgressBrushProperty, value); }
        }
        #endregion

        #region SecondaryProgressBrush DependencyProperty
        public static readonly DependencyProperty SecondaryProgressBrushProperty = DependencyProperty.Register("SecondaryProgressBrush",
            typeof(Brush),
            typeof(ProgressBarBase),
            new PropertyMetadata(null));

        public Brush SecondaryProgressBrush
        {
            get { return (Brush)GetValue(SecondaryProgressBrushProperty); }
            set { SetValue(SecondaryProgressBrushProperty, value); }
        }
        #endregion

        #region IsIndeterminate DependencyProperty
        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate",
            typeof(bool),
            typeof(ProgressBarBase),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsIndeterminateChanged));

        private static void OnIsIndeterminateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ProgressBarBase)sender;

            instance.UpdateVisualState(true);
            instance.OnIsIndeterminateChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region SegmentCount DependencyProperty
        public static readonly DependencyProperty SegmentCountProperty = DependencyProperty.Register("SegmentCount",
            typeof(int),
            typeof(ProgressBarBase),
            new PropertyMetadata(1, OnSegmentCountPropertyChanged));

        private static void OnSegmentCountPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ProgressBarBase)sender;

            instance.OnSegmentCountChanged((int)e.OldValue, (int)e.NewValue);
        }

        public int SegmentCount
        {
            get { return (int)GetValue(SegmentCountProperty); }
            set { SetValue(SegmentCountProperty, value); }
        }
        #endregion

        #region GapWidth DependencyProperty
        public static readonly DependencyProperty GapWidthProperty = DependencyProperty.Register("GapWidth",
            typeof(double),
            typeof(ProgressBarBase),
            new PropertyMetadata(5.0, OnGapWidthPropertyChanged));

        private static void OnGapWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ProgressBarBase)sender;

            instance.OnGapWidthChanged((double)e.OldValue, (double)e.NewValue);
        }

        public double GapWidth
        {
            get { return (double)GetValue(GapWidthProperty); }
            set { SetValue(GapWidthProperty, value); }
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateVisualState(false);
        }

        protected virtual void OnMinimumChanged(double oldValue, double newValue)
        {
            if (Progress < newValue) Progress = newValue;
            if (SecondaryProgress < newValue) SecondaryProgress = newValue;
        }

        protected virtual void OnMaximumChanged(double oldValue, double newValue)
        {
            if (Progress > newValue) Progress = newValue;
            if (SecondaryProgress > newValue) SecondaryProgress = newValue;
        }

        protected virtual void OnProgressChanged(double oldValue, double newValue) { }

        protected virtual void OnSecondaryProgressChanged(double oldValue, double newValue) { }

        protected virtual void OnIsIndeterminateChanged(bool oldValue, bool newValue) { }

        protected virtual void OnSegmentCountChanged(int oldValue, int newValue) { }

        protected virtual void OnGapWidthChanged(double oldValue, double newValue) { }

        protected virtual void UpdateVisualState(bool useTransitions)
        {
            if (IsIndeterminate) VisualStateManager.GoToState(this, "Indeterminate", useTransitions);
            else VisualStateManager.GoToState(this, "Determinate", useTransitions);
        }
    }
}