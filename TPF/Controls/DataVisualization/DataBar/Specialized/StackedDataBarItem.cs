using System;
using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls.Specialized.DataBar
{
    public class StackedDataBarItem : Control
    {
        static StackedDataBarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StackedDataBarItem), new FrameworkPropertyMetadata(typeof(StackedDataBarItem)));
        }

        #region BarHeightFactor DependencyProperty
        public static readonly DependencyProperty BarHeightFactorProperty = DependencyProperty.Register("BarHeightFactor",
            typeof(double),
            typeof(StackedDataBarItem),
            new PropertyMetadata(1.0));

        public double BarHeightFactor
        {
            get { return (double)GetValue(BarHeightFactorProperty); }
            set { SetValue(BarHeightFactorProperty, value); }
        }
        #endregion

        #region BarStyle DependencyProperty
        public static readonly DependencyProperty BarStyleProperty = DependencyProperty.Register("BarStyle",
            typeof(Style),
            typeof(StackedDataBarItem),
            new PropertyMetadata(null));

        public Style BarStyle
        {
            get { return (Style)GetValue(BarStyleProperty); }
            set { SetValue(BarStyleProperty, value); }
        }
        #endregion

        #region BarStrokeThickness DependencyProperty
        public static readonly DependencyProperty BarStrokeThicknessProperty = DependencyProperty.Register("BarStrokeThickness",
            typeof(double),
            typeof(StackedDataBarItem),
            new PropertyMetadata(0.0));

        public double BarStrokeThickness
        {
            get { return (double)GetValue(BarStrokeThicknessProperty); }
            set { SetValue(BarStrokeThicknessProperty, value); }
        }
        #endregion

        #region ToolTipTemplate DependencyProperty
        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register("ToolTipTemplate",
            typeof(DataTemplate),
            typeof(StackedDataBarItem),
            new PropertyMetadata(null));

        public DataTemplate ToolTipTemplate
        {
            get { return (DataTemplate)GetValue(ToolTipTemplateProperty); }
            set { SetValue(ToolTipTemplateProperty, value); }
        }
        #endregion

        #region Start DependencyProperty
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start",
            typeof(double),
            typeof(StackedDataBarItem),
            new PropertyMetadata(0.0));

        public double Start
        {
            get { return (double)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }
        #endregion

        #region End DependencyProperty
        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End",
            typeof(double),
            typeof(StackedDataBarItem),
            new PropertyMetadata(0.0));

        public double End
        {
            get { return (double)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }
        #endregion
    }
}