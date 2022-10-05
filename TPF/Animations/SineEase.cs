using System;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace TPF.Animations
{
    [MarkupExtensionReturnType(typeof(System.Windows.Media.Animation.SineEase))]
    public class SineEase : MarkupExtension
    {
        public EasingMode EasingMode { get; set; }

        public SineEase()
        {
            EasingMode = EasingMode.EaseIn;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new System.Windows.Media.Animation.SineEase()
            {
                EasingMode = EasingMode
            };
        }
    }
}