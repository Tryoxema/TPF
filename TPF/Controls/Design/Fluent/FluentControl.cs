using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using TPF.Internal;

namespace TPF.Controls
{
    public class FluentControl : MaterialControl
    {
        static FluentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FluentControl), new FrameworkPropertyMetadata(typeof(FluentControl)));
        }

        #region FluentEffectMode DependencyProperty
        public static readonly DependencyProperty FluentEffectModeProperty = DependencyProperty.Register("FluentEffectMode",
            typeof(FluentEffectMode),
            typeof(FluentControl),
            new PropertyMetadata(FluentEffectModeBoxes.RippleBox));

        public FluentEffectMode FluentEffectMode
        {
            get { return (FluentEffectMode)GetValue(FluentEffectModeProperty); }
            set { SetValue(FluentEffectModeProperty, FluentEffectModeBoxes.Box(value)); }
        }
        #endregion

        #region BorderGradient DependencyProperty
        public static readonly DependencyProperty BorderGradientProperty = DependencyProperty.Register("BorderGradient",
            typeof(Brush),
            typeof(FluentControl),
            new PropertyMetadata(null));

        public Brush BorderGradient
        {
            get { return (Brush)GetValue(BorderGradientProperty); }
            set { SetValue(BorderGradientProperty, value); }
        }
        #endregion

        #region PressedGradient DependencyProperty
        public static readonly DependencyProperty PressedGradientProperty = DependencyProperty.Register("PressedGradient",
            typeof(Brush),
            typeof(FluentControl),
            new PropertyMetadata(null));

        public Brush PressedGradient
        {
            get { return (Brush)GetValue(PressedGradientProperty); }
            set { SetValue(PressedGradientProperty, value); }
        }
        #endregion

        internal override void OnMouseMoveExternal(MouseEventArgs e)
        {
            base.OnMouseMoveExternal(e);

            var relativePosition = Mouse.GetPosition(this);

            if (relativePosition.X >= 0 && relativePosition.Y >= 0 && relativePosition.X < ActualWidth && relativePosition.Y < ActualHeight)
            {
                if (BorderGradient is RadialGradientBrush gradient)
                {
                    // Der Gradient muss modifiziert werden können
                    if (gradient.IsFrozen)
                    {
                        gradient = gradient.Clone();
                        BorderGradient = gradient;
                    }

                    // Aktuellen Punkt in relativen Wert umwandeln
                    var calculationWidth = ActualWidth - 1;
                    var calculationHeight = ActualHeight - 1;

                    var relativePoint = new Point()
                    {
                        X = Math.Round(relativePosition.X / calculationWidth, 3),
                        Y = Math.Round(relativePosition.Y / calculationHeight, 3)
                    };

                    gradient.Center = relativePoint;
                    gradient.GradientOrigin = relativePoint;
                }
            }
        }

        internal override void OnMouseButtonUp()
        {
            base.OnMouseButtonUp();

            var relativePosition = Mouse.GetPosition(this);

            if (relativePosition.X >= 0 && relativePosition.Y >= 0 && relativePosition.X < ActualWidth && relativePosition.Y < ActualHeight)
            {
                VisualStateManager.GoToState(this, "MouseIn", false);
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            VisualStateManager.GoToState(this, "MouseIn", false);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var point = e.GetPosition(this);

            if (!IsRippleCentered)
            {
                RippleX = point.X - RippleSize / 2;
                RippleY = point.Y - RippleSize / 2;
            }

            if (BorderGradient is RadialGradientBrush gradient)
            {
                // Der Gradient muss modifiziert werden können
                if (gradient.IsFrozen)
                {
                    gradient = gradient.Clone();
                    BorderGradient = gradient;
                }

                // Aktuellen Punkt in relativen Wert umwandeln
                var calculationWidth = ActualWidth - 1;
                var calculationHeight = ActualHeight - 1;

                var relativePoint = new Point()
                {
                    X = Math.Round(point.X / calculationWidth, 3),
                    Y = Math.Round(point.Y / calculationHeight, 3)
                };

                gradient.Center = relativePoint;
                gradient.GradientOrigin = relativePoint;
            }

            base.OnMouseMove(e);
        }

        // Wenn sich die Größe ändert, muss die Größe des Ripple-Effekts neu berechnet werden
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            // Zuerst das MaterialControl seine Größe berechnen lassen
            base.OnRenderSizeChanged(sizeInfo);

            // Und dann verdoppeln
            RippleSize = 2 * RippleSize;
        }
    }
}