using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using TPF.Internal;

namespace TPF.Controls
{
    public class Shadow : Border
    {
        static Shadow()
        {
            CreateShadowEffects();
        }

        #region ShadowDepth DependencyProperty
        public static readonly DependencyProperty ShadowDepthProperty = DependencyProperty.Register("ShadowDepth",
            typeof(ShadowDepth),
            typeof(Shadow),
            new FrameworkPropertyMetadata(ShadowDepth.None, FrameworkPropertyMetadataOptions.AffectsRender, OnShadowDepthChanged));

        private static void OnShadowDepthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Shadow)sender;

            switch (instance.ShadowDepth)
            {
                case ShadowDepth.Depth0:
                case ShadowDepth.Depth1:
                case ShadowDepth.Depth2:
                case ShadowDepth.Depth3:
                case ShadowDepth.Depth4:
                case ShadowDepth.Depth5:
                {
                    // Effekt aus Dictionary holen
                    var effect = Shadows[instance.ShadowDepth];
                    // Effekt klonen, damit er animiert werden kann
                    instance.Effect = effect.Clone();
                }
                break;
                case ShadowDepth.None:
                default: instance.Effect = null; break;
            }
        }

        public ShadowDepth ShadowDepth
        {
            get { return (ShadowDepth)GetValue(ShadowDepthProperty); }
            set { SetValue(ShadowDepthProperty, value); }
        }
        #endregion

        #region AnimationDuration DependencyProperty
        public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register("AnimationDuration",
            typeof(double),
            typeof(Shadow),
            new PropertyMetadata(250.0));

        public double AnimationDuration
        {
            get { return (double)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }
        #endregion

        #region Darken DependencyProperty
        public static readonly DependencyProperty DarkenProperty = DependencyProperty.Register("Darken",
            typeof(bool),
            typeof(Shadow),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender, OnDarkenChanged));

        private static void OnDarkenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Shadow)sender;

            if (!(instance.Effect is DropShadowEffect effect)) return;

            if (instance.Darken)
            {
                var animation = new DoubleAnimation(1, new Duration(TimeSpan.FromMilliseconds(instance.AnimationDuration)))
                {
                    FillBehavior = FillBehavior.HoldEnd
                };
                effect.BeginAnimation(DropShadowEffect.OpacityProperty, animation);
            }
            else
            {
                var animation = new DoubleAnimation(_defaultOpacity, new Duration(TimeSpan.FromMilliseconds(instance.AnimationDuration)))
                {
                    FillBehavior = FillBehavior.HoldEnd
                };
                effect.BeginAnimation(DropShadowEffect.OpacityProperty, animation);
            }
        }

        public bool Darken
        {
            get { return (bool)GetValue(DarkenProperty); }
            set { SetValue(DarkenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        private static readonly double _defaultOpacity = 0.42;

        internal static readonly Dictionary<ShadowDepth, DropShadowEffect> Shadows = new Dictionary<ShadowDepth, DropShadowEffect>();

        // Erstellt die Effekte für Tiefe 1-5 und legt sie im internen Dictionary ab
        private static void CreateShadowEffects()
        {
            var color = (Color)ColorConverter.ConvertFromString("#AA000000");

            var depth0Effect = new DropShadowEffect()
            {
                BlurRadius = 2,
                ShadowDepth = 0,
                Direction = 270,
                Opacity = _defaultOpacity,
                RenderingBias = RenderingBias.Performance,
                Color = color
            };

            var depth1Effect = new DropShadowEffect()
            {
                BlurRadius = 5,
                ShadowDepth = 1,
                Direction = 270,
                Opacity = _defaultOpacity,
                RenderingBias = RenderingBias.Performance,
                Color = color
            };

            var depth2Effect = new DropShadowEffect()
            {
                BlurRadius = 8,
                ShadowDepth = 1.5,
                Direction = 270,
                Opacity = _defaultOpacity,
                RenderingBias = RenderingBias.Performance,
                Color = color
            };

            var depth3Effect = new DropShadowEffect()
            {
                BlurRadius = 14,
                ShadowDepth = 4.5,
                Direction = 270,
                Opacity = _defaultOpacity,
                RenderingBias = RenderingBias.Performance,
                Color = color
            };

            var depth4Effect = new DropShadowEffect()
            {
                BlurRadius = 25,
                ShadowDepth = 8,
                Direction = 270,
                Opacity = _defaultOpacity,
                RenderingBias = RenderingBias.Performance,
                Color = color
            };

            var depth5Effect = new DropShadowEffect()
            {
                BlurRadius = 35,
                ShadowDepth = 13,
                Direction = 270,
                Opacity = _defaultOpacity,
                RenderingBias = RenderingBias.Performance,
                Color = color
            };

            // Die Vorlagen können alle eingefroren werden, da sie nie bearbeitet werden sollten
            depth0Effect.Freeze();
            depth1Effect.Freeze();
            depth2Effect.Freeze();
            depth3Effect.Freeze();
            depth4Effect.Freeze();
            depth5Effect.Freeze();

            Shadows.Add(ShadowDepth.Depth0, depth0Effect);
            Shadows.Add(ShadowDepth.Depth1, depth1Effect);
            Shadows.Add(ShadowDepth.Depth2, depth2Effect);
            Shadows.Add(ShadowDepth.Depth3, depth3Effect);
            Shadows.Add(ShadowDepth.Depth4, depth4Effect);
            Shadows.Add(ShadowDepth.Depth5, depth5Effect);
        }
    }
}