using System;
using System.Windows;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public class MaterialElement
    {
        #region IsRippleEnabled Attached DependencyProperty
        public static readonly DependencyProperty IsRippleEnabledProperty = DependencyProperty.RegisterAttached("IsRippleEnabled",
            typeof(bool),
            typeof(MaterialElement),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public static bool GetIsRippleEnabled(DependencyObject element)
        {
            return (bool)element.GetValue(IsRippleEnabledProperty);
        }

        public static void SetIsRippleEnabled(DependencyObject element, bool value)
        {
            element.SetValue(IsRippleEnabledProperty, BooleanBoxes.Box(value));
        }
        #endregion

        #region IsRippleCentered Attached DependencyProperty
        public static readonly DependencyProperty IsRippleCenteredProperty = DependencyProperty.RegisterAttached("IsRippleCentered",
            typeof(bool),
            typeof(MaterialElement),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static bool GetIsRippleCentered(DependencyObject element)
        {
            return (bool)element.GetValue(IsRippleCenteredProperty);
        }

        public static void SetIsRippleCentered(DependencyObject element, bool value)
        {
            element.SetValue(IsRippleCenteredProperty, BooleanBoxes.Box(value));
        }
        #endregion

        #region IsRippleOnTop Attached DependencyProperty
        public static readonly DependencyProperty IsRippleOnTopProperty = DependencyProperty.RegisterAttached("IsRippleOnTop",
            typeof(bool),
            typeof(MaterialElement),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static bool GetIsRippleOnTop(DependencyObject element)
        {
            return (bool)element.GetValue(IsRippleOnTopProperty);
        }

        public static void SetIsRippleOnTop(DependencyObject element, bool value)
        {
            element.SetValue(IsRippleOnTopProperty, BooleanBoxes.Box(value));
        }
        #endregion

        #region RippleBrush Attached DependencyProperty
        public static readonly DependencyProperty RippleBrushProperty = DependencyProperty.RegisterAttached("RippleBrush",
            typeof(Brush),
            typeof(MaterialElement),
            new PropertyMetadata(null));

        public static Brush GetRippleBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(RippleBrushProperty);
        }

        public static void SetRippleBrush(DependencyObject element, Brush value)
        {
            element.SetValue(RippleBrushProperty, value);
        }
        #endregion

        #region RippleOpacity Attached DependencyProperty
        public static readonly DependencyProperty RippleOpacityProperty = DependencyProperty.RegisterAttached("RippleOpacity",
            typeof(double),
            typeof(MaterialElement),
            new PropertyMetadata(0.5));

        public static double GetRippleOpacity(DependencyObject element)
        {
            return (double)element.GetValue(RippleOpacityProperty);
        }

        public static void SetRippleOpacity(DependencyObject element, double value)
        {
            element.SetValue(RippleOpacityProperty, value);
        }
        #endregion
    }
}