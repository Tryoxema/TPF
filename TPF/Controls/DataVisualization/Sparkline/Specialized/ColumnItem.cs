using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls.Specialized.Sparkline
{
    public class ColumnItem : Control
    {
        static ColumnItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColumnItem), new FrameworkPropertyMetadata(typeof(ColumnItem)));
        }

        #region RelativeX DependencyProperty
        public static readonly DependencyProperty RelativeXProperty = DependencyProperty.Register("RelativeX",
            typeof(double),
            typeof(ColumnItem),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange, null, ConstrainDouble));

        public double RelativeX
        {
            get { return (double)GetValue(RelativeXProperty); }
            set { SetValue(RelativeXProperty, value); }
        }
        #endregion

        #region RelativeYTop DependencyProperty
        public static readonly DependencyProperty RelativeYTopProperty = DependencyProperty.Register("RelativeYTop",
            typeof(double),
            typeof(ColumnItem),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange, null, ConstrainDouble));

        public double RelativeYTop
        {
            get { return (double)GetValue(RelativeYTopProperty); }
            set { SetValue(RelativeYTopProperty, value); }
        }
        #endregion

        #region RelativeYBottom DependencyProperty
        public static readonly DependencyProperty RelativeYBottomProperty = DependencyProperty.Register("RelativeYBottom",
            typeof(double),
            typeof(ColumnItem),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange, null, ConstrainDouble));

        public double RelativeYBottom
        {
            get { return (double)GetValue(RelativeYBottomProperty); }
            set { SetValue(RelativeYBottomProperty, value); }
        }
        #endregion

        #region Type DependencyProperty
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type",
            typeof(IndicatorType),
            typeof(ColumnItem),
            new PropertyMetadata(IndicatorType.Normal));

        public IndicatorType Type
        {
            get { return (IndicatorType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        #endregion

        #region ToolTipTemplate DependencyProperty
        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register("ToolTipTemplate",
            typeof(DataTemplate),
            typeof(ColumnItem),
            new PropertyMetadata(null));

        public DataTemplate ToolTipTemplate
        {
            get { return (DataTemplate)GetValue(ToolTipTemplateProperty); }
            set { SetValue(ToolTipTemplateProperty, value); }
        }
        #endregion

        private static object ConstrainDouble(DependencyObject d, object baseValue)
        {
            var doubleValue = (double)baseValue;

            if (doubleValue < 0) doubleValue = 0;
            else if (doubleValue > 1) doubleValue = 1;

            return doubleValue;
        }
    }
}