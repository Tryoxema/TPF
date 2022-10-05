using System;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace TPF.Animations
{
    [MarkupExtensionReturnType(typeof(System.Windows.Media.Animation.BackEase))]
    public class BackEase : MarkupExtension
    {
        public double Amplitude { get; set; }

        public EasingMode EasingMode { get; set; }

        public BackEase()
        {
            Amplitude = 1;
            EasingMode = EasingMode.EaseIn;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new System.Windows.Media.Animation.BackEase()
            {
                Amplitude = Amplitude,
                EasingMode = EasingMode
            };
        }
    }
}