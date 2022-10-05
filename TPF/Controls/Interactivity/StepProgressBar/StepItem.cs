using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.ComponentModel;

namespace TPF.Controls
{
    public class StepItem : ContentControl
    {
        static StepItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StepItem), new FrameworkPropertyMetadata(typeof(StepItem)));
        }

        #region ConnectorBrush DependencyProperty
        public static readonly DependencyProperty ConnectorBrushProperty = DependencyProperty.Register("ConnectorBrush",
            typeof(Brush),
            typeof(StepItem),
            new PropertyMetadata(null));

        public Brush ConnectorBrush
        {
            get { return (Brush)GetValue(ConnectorBrushProperty); }
            set { SetValue(ConnectorBrushProperty, value); }
        }
        #endregion

        #region ConnectorProgressBrush DependencyProperty
        public static readonly DependencyProperty ConnectorProgressBrushProperty = DependencyProperty.Register("ConnectorProgressBrush",
            typeof(Brush),
            typeof(StepItem),
            new PropertyMetadata(null));

        public Brush ConnectorProgressBrush
        {
            get { return (Brush)GetValue(ConnectorProgressBrushProperty); }
            set { SetValue(ConnectorProgressBrushProperty, value); }
        }
        #endregion

        #region ConnectorThickness DependencyProperty
        public static readonly DependencyProperty ConnectorThicknessProperty = DependencyProperty.Register("ConnectorThickness",
            typeof(double),
            typeof(StepItem),
            new PropertyMetadata(2.0));

        public double ConnectorThickness
        {
            get { return (double)GetValue(ConnectorThicknessProperty); }
            set { SetValue(ConnectorThicknessProperty, value); }
        }
        #endregion

        #region Orientation ReadOnly DependencyProperty
        internal static readonly DependencyPropertyKey OrientationPropertyKey = DependencyProperty.RegisterReadOnly("Orientation",
            typeof(Orientation),
            typeof(StepItem),
            new PropertyMetadata(Orientation.Horizontal));

        public static readonly DependencyProperty OrientationProperty = OrientationPropertyKey.DependencyProperty;

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            internal set { SetValue(OrientationPropertyKey, value); }
        }
        #endregion

        #region IndicatorTemplate DependencyProperty
        public static readonly DependencyProperty IndicatorTemplateProperty = DependencyProperty.Register("IndicatorTemplate",
            typeof(DataTemplate),
            typeof(StepItem),
            new PropertyMetadata(null));

        public DataTemplate IndicatorTemplate
        {
            get { return (DataTemplate)GetValue(IndicatorTemplateProperty); }
            set { SetValue(IndicatorTemplateProperty, value); }
        }
        #endregion

        #region IndicatorHeight DependencyProperty
        public static readonly DependencyProperty IndicatorHeightProperty = DependencyProperty.Register("IndicatorHeight",
            typeof(double),
            typeof(StepItem),
            new PropertyMetadata(24.0));

        public double IndicatorHeight
        {
            get { return (double)GetValue(IndicatorHeightProperty); }
            set { SetValue(IndicatorHeightProperty, value); }
        }
        #endregion

        #region IndicatorWidth DependencyProperty
        public static readonly DependencyProperty IndicatorWidthProperty = DependencyProperty.Register("IndicatorWidth",
            typeof(double),
            typeof(StepItem),
            new PropertyMetadata(24.0));

        public double IndicatorWidth
        {
            get { return (double)GetValue(IndicatorWidthProperty); }
            set { SetValue(IndicatorWidthProperty, value); }
        }
        #endregion

        #region Progress DependencyProperty
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress",
            typeof(double),
            typeof(StepItem),
            new PropertyMetadata(0.0, null, ConstrainProgress));

        internal static object ConstrainProgress(DependencyObject sender, object value)
        {
            var doubleValue = (double)value;

            if (doubleValue < 0) doubleValue = 0;
            else if (doubleValue > 100) doubleValue = 100;

            return doubleValue;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        #endregion

        #region ItemSpacing DependencyProperty
        public static readonly DependencyProperty ItemSpacingProperty = DependencyProperty.Register("ItemSpacing",
            typeof(double),
            typeof(StepItem),
            new PropertyMetadata(80.0));

        public double ItemSpacing
        {
            get { return (double)GetValue(ItemSpacingProperty); }
            set { SetValue(ItemSpacingProperty, value); }
        }
        #endregion

        #region SecondaryContent DependencyProperty
        public static readonly DependencyProperty SecondaryContentProperty = DependencyProperty.Register("SecondaryContent",
            typeof(object),
            typeof(StepItem),
            new PropertyMetadata(null));

        public object SecondaryContent
        {
            get { return GetValue(SecondaryContentProperty); }
            set { SetValue(SecondaryContentProperty, value); }
        }
        #endregion

        #region SecondaryContentTemplate DependencyProperty
        public static readonly DependencyProperty SecondaryContentTemplateProperty = DependencyProperty.Register("SecondaryContentTemplate",
            typeof(DataTemplate),
            typeof(StepItem),
            new PropertyMetadata(null));

        public DataTemplate SecondaryContentTemplate
        {
            get { return (DataTemplate)GetValue(SecondaryContentTemplateProperty); }
            set { SetValue(SecondaryContentTemplateProperty, value); }
        }
        #endregion

        #region SecondaryContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty SecondaryContentTemplateSelectorProperty = DependencyProperty.Register("SecondaryContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(StepItem),
            new PropertyMetadata(null));

        public DataTemplateSelector SecondaryContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(SecondaryContentTemplateSelectorProperty); }
            set { SetValue(SecondaryContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region StepStatus DependencyProperty
        public static readonly DependencyProperty StepStatusProperty = DependencyProperty.Register("StepStatus",
            typeof(StepStatus),
            typeof(StepItem),
            new PropertyMetadata(StepStatus.Inactive, OnStepStatusChanged));

        static void OnStepStatusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StepItem)sender;

            var parent = instance.ParentStepProgressBar;

            if (parent != null)
            {
                instance.IndicatorTemplate = parent.IndicatorTemplateSelector?.SelectTemplate(instance.Content, instance);
            }
        }

        public StepStatus StepStatus
        {
            get { return (StepStatus)GetValue(StepStatusProperty); }
            set { SetValue(StepStatusProperty, value); }
        }
        #endregion

        internal StepProgressBar ParentStepProgressBar
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this) as StepProgressBar; }
        }

        internal ContentPresenter Indicator;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (Indicator != null)
            {
                Indicator.MouseLeftButtonUp -= Indicator_MouseLeftButtonUp;
            }

            Indicator = (ContentPresenter)GetTemplateChild("PART_Indicator");

            if (Indicator != null)
            {
                Indicator.MouseLeftButtonUp += Indicator_MouseLeftButtonUp;
            }
        }

        private void Indicator_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ParentStepProgressBar != null) ParentStepProgressBar.RaiseIndicatorClicked(this);
        }
    }
}