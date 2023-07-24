using System.Windows.Controls.Primitives;

namespace TPF.Controls
{
    public abstract class SliderThumbBase : Thumb
    {
        protected internal Slider ParentSlider { get; private set; }
        protected internal SliderTrack ParentTrack { get; private set; }
        protected internal SliderThumbsControl ParentThumbsControl { get; internal set; }
        protected internal SliderThumbsPanel ParentThumbsPanel { get; internal set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ParentSlider = this.ParentOfType<Slider>();
            ParentTrack = this.ParentOfType<SliderTrack>();
        }

        protected void OnThumbValueChanged()
        {
            if (ParentSlider != null)
            {
                ParentSlider.ThumbValueChanged();
            }

            if (ParentThumbsPanel != null)
            {
                ParentThumbsPanel.InvalidateMeasure();
                ParentThumbsPanel.InvalidateArrange();
            }
        }
    }
}