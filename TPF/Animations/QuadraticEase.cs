using System;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace TPF.Animations
{
    [MarkupExtensionReturnType(typeof(System.Windows.Media.Animation.QuadraticEase))]
    public class QuadraticEase : MarkupExtension
    {
        public EasingMode EasingMode { get; set; }

        public QuadraticEase()
        {
            EasingMode = EasingMode.EaseIn;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new System.Windows.Media.Animation.QuadraticEase()
            {
                EasingMode = EasingMode
            };
        }
    }
}