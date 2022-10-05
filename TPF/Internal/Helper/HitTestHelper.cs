using System.Windows;
using System.Windows.Media;
using TPF.Controls;

namespace TPF.Internal
{
    internal static class HitTestHelper
    {
        internal static bool HitTestForType<T>(object sender, Point elementPosition) where T : UIElement
        {
            var uiElement = GetHitTestElementOfType<T>(sender, elementPosition);

            return uiElement != null && uiElement.Visibility == Visibility.Visible;
        }

        internal static T GetHitTestElementOfType<T>(object sender, Point elementPosition) where T : UIElement
        {
            if (!(sender is Visual visual)) return null;

            T resultElement = null;

            VisualTreeHelper.HitTest(visual, HitTestFilterInvisible, result =>
            {
                if (result == null) return HitTestResultBehavior.Continue;

                resultElement = result.VisualHit.ParentOfType<T>();

                return HitTestResultBehavior.Stop;
            }, new PointHitTestParameters(elementPosition));

            return resultElement;
        }

        internal static HitTestFilterBehavior HitTestFilterInvisible(DependencyObject potentialHitTestTarget)
        {
            bool isVisible = false;
            bool isHitTestVisible = false;

            if (potentialHitTestTarget is UIElement uiElement)
            {
                isVisible = uiElement.IsVisible;

                if (isVisible) isHitTestVisible = uiElement.IsHitTestVisible;
            }

            if (isVisible)
            {
                return isHitTestVisible ? HitTestFilterBehavior.Continue : HitTestFilterBehavior.ContinueSkipSelf;
            }

            return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
        }
    }
}