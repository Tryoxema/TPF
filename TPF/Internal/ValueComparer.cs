using System.Windows.Media;

namespace TPF.Internal
{
    internal static class ValueComparer
    {
        internal static bool IsEqualTo(object first, object second)
        {
            if (first == null && second == null) return true;
            if (first == null && second != null) return false;
            if (first != null && second == null) return false;
            if (ReferenceEquals(first, second)) return true;

            var firstType = first.GetType();
            var secondType = second.GetType();

            return firstType == secondType && first.Equals(second);
        }

        internal static bool IsEqualTo(Brush first, Brush second)
        {
            if (first.GetType() != second.GetType()) return false;

            if (ReferenceEquals(first, second)) return true;

            // Sind beide vom Typ SolidColorBrush?
            if ((first is SolidColorBrush solidBrushA) && (second is SolidColorBrush solidBrushB))
            {
                return IsEqualTo(solidBrushA, solidBrushB);
            }

            // Sind beide vom Typ LinearGradientBrush?
            if ((first is LinearGradientBrush linearGradientBrushA) && (second is LinearGradientBrush linearGradientBrushB))
            {
                return IsEqualTo(linearGradientBrushA, linearGradientBrushB);
            }

            // Sind beide vom Typ RadialGradientBrush?
            if ((first is RadialGradientBrush radialGradientBrushA) && (second is RadialGradientBrush radialGradientBrushB))
            {
                return IsEqualTo(radialGradientBrushA, radialGradientBrushB);
            }

            // Sind beide vom Typ ImageBrush?
            if ((first is ImageBrush imageBrushA) && (second is ImageBrush imageBrushB))
            {
                return IsEqualTo(imageBrushA, imageBrushB);
            }

            return false;
        }

        internal static bool IsEqualTo(SolidColorBrush first, SolidColorBrush second)
        {
            return (first.Color == second.Color) && first.Opacity.IsCloseOrEqual(second.Opacity);
        }

        internal static bool IsEqualTo(LinearGradientBrush first, LinearGradientBrush second)
        {
            var result = (first.ColorInterpolationMode == second.ColorInterpolationMode)
                         && (first.EndPoint == second.EndPoint)
                         && (first.MappingMode == second.MappingMode)
                         && first.Opacity.IsCloseOrEqual(second.Opacity)
                         && (first.StartPoint == second.StartPoint)
                         && (first.SpreadMethod == second.SpreadMethod)
                         && (first.GradientStops.Count == second.GradientStops.Count);

            if (!result) return false;

            for (var i = 0; i < first.GradientStops.Count; i++)
            {
                var firstGradientStop = first.GradientStops[i];
                var secondGradientStop = second.GradientStops[i];

                result = (firstGradientStop.Color == secondGradientStop.Color) && firstGradientStop.Offset.IsCloseOrEqual(secondGradientStop.Offset);

                if (!result) break;
            }

            return result;
        }

        internal static bool IsEqualTo(RadialGradientBrush first, RadialGradientBrush second)
        {
            var result = (first.ColorInterpolationMode == second.ColorInterpolationMode)
                         && (first.GradientOrigin == second.GradientOrigin)
                         && (first.MappingMode == second.MappingMode)
                         && first.Opacity.IsCloseOrEqual(second.Opacity)
                         && first.RadiusX.IsCloseOrEqual(second.RadiusX)
                         && first.RadiusY.IsCloseOrEqual(second.RadiusY)
                         && (first.SpreadMethod == second.SpreadMethod)
                         && (first.GradientStops.Count == second.GradientStops.Count);

            if (!result) return false;

            for (var i = 0; i < first.GradientStops.Count; i++)
            {
                var firstGradientStop = first.GradientStops[i];
                var secondGradientStop = second.GradientStops[i];

                result = (firstGradientStop.Color == secondGradientStop.Color) && firstGradientStop.Offset.IsCloseOrEqual(secondGradientStop.Offset);

                if (!result) break;
            }

            return result;
        }

        internal static bool IsEqualTo(ImageBrush first, ImageBrush second)
        {
            var result = (first.AlignmentX == second.AlignmentX)
                         && (first.AlignmentY == second.AlignmentY)
                         && first.Opacity.IsCloseOrEqual(second.Opacity)
                         && (first.Stretch == second.Stretch)
                         && (first.TileMode == second.TileMode)
                         && (first.Viewbox == second.Viewbox)
                         && (first.ViewboxUnits == second.ViewboxUnits)
                         && (first.Viewport == second.Viewport)
                         && (first.ViewportUnits == second.ViewportUnits)
                         && (first.ImageSource == second.ImageSource);

            return result;
        }
    }
}