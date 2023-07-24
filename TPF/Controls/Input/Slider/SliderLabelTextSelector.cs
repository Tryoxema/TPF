namespace TPF.Controls
{
    public class SliderLabelTextSelector
    {
        public virtual string SelectLabelText(Slider slider, double value)
        {
            return value.ToString();
        }
    }
}