using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TPF.Internal;

namespace TPF.Controls
{
    public class BusyIndicator : ContentControl
    {
        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyIndicator), new FrameworkPropertyMetadata(typeof(BusyIndicator)));
        }

        public BusyIndicator()
        {
            DisplayTimer.Tick += (s, e) =>
            {
                DisplayTimer.Stop();
                IsBusyContentVisible = true;
            };
        }

        #region IsBusy DependencyProperty
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsBusyChanged));

        private static void OnIsBusyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (BusyIndicator)sender;

            instance.OnIsBusyChanged();
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsBusyContentVisible ReadOnly DependencyProperty
        internal static readonly DependencyPropertyKey IsBusyContentVisiblePropertyKey = DependencyProperty.RegisterReadOnly("IsBusyContentVisible",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static readonly DependencyProperty IsBusyContentVisibleProperty = IsBusyContentVisiblePropertyKey.DependencyProperty;

        public bool IsBusyContentVisible
        {
            get { return (bool)GetValue(IsBusyContentVisibleProperty); }
            protected set { SetValue(IsBusyContentVisiblePropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region DisplayDelay DependencyProperty
        public static readonly DependencyProperty DisplayAfterProperty = DependencyProperty.Register("DisplayAfter",
            typeof(TimeSpan),
            typeof(BusyIndicator),
            new PropertyMetadata(TimeSpan.FromSeconds(0.1)));

        public TimeSpan DisplayAfter
        {
            get { return (TimeSpan)GetValue(DisplayAfterProperty); }
            set { SetValue(DisplayAfterProperty, value); }
        }
        #endregion

        #region OverlayStyle DependencyProperty
        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register("OverlayStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        public Style OverlayStyle
        {
            get { return (Style)GetValue(OverlayStyleProperty); }
            set { SetValue(OverlayStyleProperty, value); }
        }
        #endregion

        #region ProgressBarStyle DependencyProperty
        public static readonly DependencyProperty ProgressBarStyleProperty = DependencyProperty.Register("ProgressBarStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        public Style ProgressBarStyle
        {
            get { return (Style)GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }
        #endregion

        #region ProgressBarValue DependencyProperty
        public static readonly DependencyProperty ProgressBarValueProperty = DependencyProperty.Register("ProgressBarValue",
            typeof(double),
            typeof(BusyIndicator),
            new PropertyMetadata(0.0));

        public double ProgressBarValue
        {
            get { return (double)GetValue(ProgressBarValueProperty); }
            set { SetValue(ProgressBarValueProperty, value); }
        }
        #endregion

        #region IsIndeterminate DependencyProperty
        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region BusyContent DependencyProperty
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register("BusyContent",
            typeof(object),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        public object BusyContent
        {
            get { return GetValue(BusyContentProperty); }
            set { SetValue(BusyContentProperty, value); }
        }
        #endregion

        #region BusyContentTemplate DependencyProperty
        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register("BusyContentTemplate",
            typeof(DataTemplate),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        public DataTemplate BusyContentTemplate
        {
            get { return (DataTemplate)GetValue(BusyContentTemplateProperty); }
            set { SetValue(BusyContentTemplateProperty, value); }
        }
        #endregion

        private readonly DispatcherTimer DisplayTimer = new DispatcherTimer();

        protected virtual void OnIsBusyChanged()
        {
            if (IsBusy)
            {
                if (DisplayAfter.Equals(TimeSpan.Zero))
                {
                    IsBusyContentVisible = true;
                }
                else
                {
                    DisplayTimer.Interval = DisplayAfter;
                    DisplayTimer.Start();
                }
            }
            else
            {
                DisplayTimer.Stop();
                IsBusyContentVisible = false;
            }
        }
    }
}