using TPF.Controls;

namespace TPF.Internal
{
    internal static class FluentEffectModeBoxes
    {
        internal static object GlowBox = FluentEffectMode.Glow;

        internal static object RippleBox = FluentEffectMode.Ripple;

        internal static object Box(FluentEffectMode value)
        {
            switch (value)
            {
                case FluentEffectMode.Glow: return GlowBox;
                case FluentEffectMode.Ripple:
                default: return RippleBox;
            }
        }
    }
}