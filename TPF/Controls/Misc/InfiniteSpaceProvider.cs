using System;
using System.Windows;
using System.Windows.Controls;
using TPF.Internal;

namespace TPF.Controls
{
    public class InfiniteSpaceProvider : Border
    {
        protected override Size MeasureOverride(Size constraint)
        {
            var child = Child;
            var borderThickness = BorderThickness;
            var padding = Padding;

            if (UseLayoutRounding)
            {
                var dpiX = DpiHelper.DpiX;
                var dpiY = DpiHelper.DpiY;

                borderThickness = new Thickness(RoundLayoutValue(borderThickness.Left, dpiX), RoundLayoutValue(borderThickness.Top, dpiY), RoundLayoutValue(borderThickness.Right, dpiX), RoundLayoutValue(borderThickness.Bottom, dpiY));
            }

            var borderSize = CollapseThickness(borderThickness);
            var paddingSize = CollapseThickness(padding);

            var size = new Size(borderSize.Width + paddingSize.Width, borderSize.Height + paddingSize.Height);

            if (child != null)
            {
                var realConstraint = new Size(Math.Max(0.0, constraint.Width - size.Width), Math.Max(0.0, constraint.Height - size.Height));
                var fakeConstraint = new Size(double.PositiveInfinity, double.PositiveInfinity);

                child.Measure(realConstraint);

                var childSize = child.DesiredSize;

                size.Width += childSize.Width;
                size.Height += childSize.Height;

                child.Measure(fakeConstraint);
            }

            return size;
        }

        private static double RoundLayoutValue(double value, double dpi)
        {
            double roundedValue;

            if (!dpi.IsCloseOrEqual(1d))
            {
                roundedValue = Math.Round(value * dpi) / dpi;

                if (double.IsNaN(roundedValue) || double.IsInfinity(roundedValue) || roundedValue.IsCloseOrEqual(double.MaxValue)) roundedValue = value;
            }
            else roundedValue = Math.Round(value);

            return roundedValue;
        }

        private static Size CollapseThickness(Thickness thickness)
        {
            return new Size(thickness.Left + thickness.Right, thickness.Top + thickness.Bottom);
        }
    }
}