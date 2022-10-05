using System;
using System.Windows;
using TPF.Internal;

namespace TPF.Controls
{
    public class ShadowElement
    {
        #region ShadowDepth Attached DependencyProperty
        public static readonly DependencyProperty ShadowDepthProperty = DependencyProperty.RegisterAttached("ShadowDepth",
            typeof(ShadowDepth),
            typeof(ShadowElement),
            new PropertyMetadata(ShadowDepth.Depth0));

        public static ShadowDepth GetShadowDepth(DependencyObject element)
        {
            return (ShadowDepth)element.GetValue(ShadowDepthProperty);
        }

        public static void SetShadowDepth(DependencyObject element, ShadowDepth value)
        {
            element.SetValue(ShadowDepthProperty, value);
        }
        #endregion

        #region AnimationDuration Attached DependencyProperty
        public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.RegisterAttached("AnimationDuration",
            typeof(double),
            typeof(ShadowElement),
            new PropertyMetadata(250.0));

        public static double GetAnimationDuration(DependencyObject element)
        {
            return (double)element.GetValue(AnimationDurationProperty);
        }

        public static void SetAnimationDuration(DependencyObject element, double value)
        {
            element.SetValue(AnimationDurationProperty, value);
        }
        #endregion

        #region Darken Attached DependencyProperty
        public static readonly DependencyProperty DarkenProperty = DependencyProperty.RegisterAttached("Darken",
            typeof(bool),
            typeof(ShadowElement),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static bool GetDarken(DependencyObject element)
        {
            return (bool)element.GetValue(DarkenProperty);
        }

        public static void SetDarken(DependencyObject element, bool value)
        {
            element.SetValue(DarkenProperty, BooleanBoxes.Box(value));
        }
        #endregion
    }
}