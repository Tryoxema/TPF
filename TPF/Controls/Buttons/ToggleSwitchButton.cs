using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace TPF.Controls
{
    public class ToggleSwitchButton : ToggleButton
    {
        static ToggleSwitchButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitchButton), new FrameworkPropertyMetadata(typeof(ToggleSwitchButton)));
        }

        #region CheckedContent DependencyProperty
        public static readonly DependencyProperty CheckedContentProperty = DependencyProperty.Register("CheckedContent",
            typeof(object),
            typeof(ToggleSwitchButton),
            new PropertyMetadata(null));

        public object CheckedContent
        {
            get { return GetValue(CheckedContentProperty); }
            set { SetValue(CheckedContentProperty, value); }
        }
        #endregion

        #region CheckedContentTemplate DependencyProperty
        public static readonly DependencyProperty CheckedContentTemplateProperty = DependencyProperty.Register("CheckedContentTemplate",
            typeof(DataTemplate),
            typeof(ToggleSwitchButton),
            new PropertyMetadata(null));

        public DataTemplate CheckedContentTemplate
        {
            get { return (DataTemplate)GetValue(CheckedContentTemplateProperty); }
            set { SetValue(CheckedContentTemplateProperty, value); }
        }
        #endregion

        #region UncheckedContent DependencyProperty
        public static readonly DependencyProperty UncheckedContentProperty = DependencyProperty.Register("UncheckedContent",
            typeof(object),
            typeof(ToggleSwitchButton),
            new PropertyMetadata(null));

        public object UncheckedContent
        {
            get { return GetValue(UncheckedContentProperty); }
            set { SetValue(UncheckedContentProperty, value); }
        }
        #endregion

        #region UncheckedContentTemplate DependencyProperty
        public static readonly DependencyProperty UncheckedContentTemplateProperty = DependencyProperty.Register("UncheckedContentTemplate",
            typeof(DataTemplate),
            typeof(ToggleSwitchButton),
            new PropertyMetadata(null));

        public DataTemplate UncheckedContentTemplate
        {
            get { return (DataTemplate)GetValue(UncheckedContentTemplateProperty); }
            set { SetValue(UncheckedContentTemplateProperty, value); }
        }
        #endregion

        #region ContentPosition DependencyProperty
        public static readonly DependencyProperty ContentPositionProperty = DependencyProperty.Register("ContentPosition",
            typeof(ToggleSwitchContentPosition),
            typeof(ToggleSwitchButton),
            new PropertyMetadata(ToggleSwitchContentPosition.Both));

        public ToggleSwitchContentPosition ContentPosition
        {
            get { return (ToggleSwitchContentPosition)GetValue(ContentPositionProperty); }
            set { SetValue(ContentPositionProperty, value); }
        }
        #endregion

        #region TrackHeight DependencyProperty
        public static readonly DependencyProperty TrackHeightProperty = DependencyProperty.Register("TrackHeight",
            typeof(double),
            typeof(ToggleSwitchButton),
            new PropertyMetadata(double.NaN));

        public double TrackHeight
        {
            get { return (double)GetValue(TrackHeightProperty); }
            set { SetValue(TrackHeightProperty, value); }
        }
        #endregion

        #region TrackWidth DependencyProperty
        public static readonly DependencyProperty TrackWidthProperty = DependencyProperty.Register("TrackWidth",
            typeof(double),
            typeof(ToggleSwitchButton),
            new PropertyMetadata(double.NaN));

        public double TrackWidth
        {
            get { return (double)GetValue(TrackWidthProperty); }
            set { SetValue(TrackWidthProperty, value); }
        }
        #endregion

        #region SwitchHeight DependencyProperty
        public static readonly DependencyProperty SwitchHeightProperty = DependencyProperty.Register("SwitchHeight",
            typeof(double),
            typeof(ToggleSwitchButton),
            new PropertyMetadata(double.NaN));

        public double SwitchHeight
        {
            get { return (double)GetValue(SwitchHeightProperty); }
            set { SetValue(SwitchHeightProperty, value); }
        }
        #endregion

        #region SwitchWidth DependencyProperty
        public static readonly DependencyProperty SwitchWidthProperty = DependencyProperty.Register("SwitchWidth",
            typeof(double),
            typeof(ToggleSwitchButton),
            new PropertyMetadata(double.NaN));

        public double SwitchWidth
        {
            get { return (double)GetValue(SwitchWidthProperty); }
            set { SetValue(SwitchWidthProperty, value); }
        }
        #endregion
    }
}