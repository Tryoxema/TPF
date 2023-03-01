using System.Windows;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public class ProgressButton : Button
    {
        static ProgressButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressButton), new FrameworkPropertyMetadata(typeof(ProgressButton)));
        }

        #region Minimum DependencyProperty
        public static readonly DependencyProperty MinimumProperty = ProgressBarBase.MinimumProperty.AddOwner(typeof(ProgressButton),
            new PropertyMetadata(0.0, null, ConstrainMinimum));

        private static object ConstrainMinimum(DependencyObject sender, object value)
        {
            var instance = (ProgressButton)sender;

            var doubleValue = (double)value;

            if (doubleValue > instance.Maximum) doubleValue = instance.Maximum;

            return doubleValue;
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        #endregion

        #region Maximum DependencyProperty
        public static readonly DependencyProperty MaximumProperty = ProgressBarBase.MaximumProperty.AddOwner(typeof(ProgressButton),
            new PropertyMetadata(100.0, null, ConstrainMaximum));

        private static object ConstrainMaximum(DependencyObject sender, object value)
        {
            var instance = (ProgressButton)sender;

            var doubleValue = (double)value;

            if (doubleValue < instance.Minimum) doubleValue = instance.Minimum;

            return doubleValue;
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        #endregion

        #region Progress DependencyProperty
        public static readonly DependencyProperty ProgressProperty = ProgressBarBase.ProgressProperty.AddOwner(typeof(ProgressButton),
            new PropertyMetadata(0.0, null, ConstrainProgress));

        private static object ConstrainProgress(DependencyObject sender, object value)
        {
            var instance = (ProgressButton)sender;

            var doubleValue = (double)value;

            if (doubleValue < instance.Minimum) doubleValue = instance.Minimum;
            else if (doubleValue > instance.Maximum) doubleValue = instance.Maximum;

            return doubleValue;
        }

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        #endregion

        #region SecondaryProgress DependencyProperty
        public static readonly DependencyProperty SecondaryProgressProperty = ProgressBarBase.SecondaryProgressProperty.AddOwner(typeof(ProgressButton),
            new PropertyMetadata(0.0, null, ConstrainProgress));

        public double SecondaryProgress
        {
            get { return (double)GetValue(SecondaryProgressProperty); }
            set { SetValue(SecondaryProgressProperty, value); }
        }
        #endregion

        #region ProgressBrush DependencyProperty
        public static readonly DependencyProperty ProgressBrushProperty = ProgressBarBase.ProgressBrushProperty.AddOwner(typeof(ProgressButton),
            new PropertyMetadata(null));

        public Brush ProgressBrush
        {
            get { return (Brush)GetValue(ProgressBrushProperty); }
            set { SetValue(ProgressBrushProperty, value); }
        }
        #endregion

        #region SecondaryProgressBrush DependencyProperty
        public static readonly DependencyProperty SecondaryProgressBrushProperty = ProgressBarBase.SecondaryProgressBrushProperty.AddOwner(typeof(ProgressButton),
            new PropertyMetadata(null));

        public Brush SecondaryProgressBrush
        {
            get { return (Brush)GetValue(SecondaryProgressBrushProperty); }
            set { SetValue(SecondaryProgressBrushProperty, value); }
        }
        #endregion

        #region IsIndeterminate DependencyProperty
        public static readonly DependencyProperty IsIndeterminateProperty = ProgressBarBase.IsIndeterminateProperty.AddOwner(typeof(ProgressButton),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, BooleanBoxes.Box(value)); }
        }
        #endregion
    }
}