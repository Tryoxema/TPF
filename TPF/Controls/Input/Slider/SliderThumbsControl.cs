using System.Windows;
using System.Windows.Controls;
using TPF.Internal;

namespace TPF.Controls
{
    public class SliderThumbsControl : ItemsControl
    {
        static SliderThumbsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SliderThumbsControl), new FrameworkPropertyMetadata(typeof(SliderThumbsControl)));
        }

        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(SliderThumbsControl),
            new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region IsDirectionReversed DependencyProperty
        public static readonly DependencyProperty IsDirectionReversedProperty = Slider.IsDirectionReversedProperty.AddOwner(typeof(SliderThumbsControl),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsArrange));

        public bool IsDirectionReversed
        {
            get { return (bool)GetValue(IsDirectionReversedProperty); }
            set { SetValue(IsDirectionReversedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        private Slider _slider;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _slider = this.ParentOfType<Slider>();
        }

        internal SliderThumbBase GetThumbForMeasure()
        {
            SliderThumbBase resultThumb = null;

            foreach (SliderThumbBase thumb in Items)
            {
                if (thumb is SliderThumb) return thumb;
                else if (thumb is RangeSliderThumb rangeThumb && rangeThumb.StartThumb != null) return rangeThumb.StartThumb;

                resultThumb = thumb;
            }

            return resultThumb;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is SliderThumbBase;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is SliderThumbBase thumb)
            {
                thumb.ParentThumbsControl = this;

                if (thumb is SliderThumb sliderThumb)
                {
                    if (sliderThumb.Style == null) sliderThumb.Style = _slider?.ThumbStyle;
                }
                else if (thumb is RangeSliderThumb rangeThumb)
                {
                    if (rangeThumb.Style == null) rangeThumb.Style = _slider?.RangeThumbStyle;
                }
            }
        }
    }
}