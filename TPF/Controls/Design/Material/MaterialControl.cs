using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Markup;
using System.Windows.Input;
using TPF.Internal;

namespace TPF.Controls
{
    [ContentProperty("Content")]
    public class MaterialControl : Control
    {
        static MaterialControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialControl), new FrameworkPropertyMetadata(typeof(MaterialControl)));

            // Die Eventhandler müssen an die Control-Klasse angehangen werden statt an das MaterialControl, um im Fall von Mouse.Capture anderer Elemente noch zu funktionieren
            EventManager.RegisterClassHandler(typeof(Control), Mouse.PreviewMouseUpEvent, new MouseButtonEventHandler(OnMouseButtonUp), true);
            EventManager.RegisterClassHandler(typeof(Control), Mouse.MouseMoveEvent, new MouseEventHandler(OnMouseMove), true);
        }

        #region Content DependencyProperty
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content",
            typeof(object),
            typeof(MaterialControl),
            new PropertyMetadata(null));

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(MaterialControl),
            new PropertyMetadata(default(CornerRadius), OnCornerRadiusChanged));

        private static void OnCornerRadiusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (MaterialControl)sender;

            instance.RefreshClip();
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region IsRippleEnabled DependencyProperty
        public static readonly DependencyProperty IsRippleEnabledProperty = DependencyProperty.Register("IsRippleEnabled",
            typeof(bool),
            typeof(MaterialControl),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool IsRippleEnabled
        {
            get { return (bool)GetValue(IsRippleEnabledProperty); }
            set { SetValue(IsRippleEnabledProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsRippleCentered DependencyProperty
        public static readonly DependencyProperty IsRippleCenteredProperty = DependencyProperty.Register("IsRippleCentered",
            typeof(bool),
            typeof(MaterialControl),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsRippleCentered
        {
            get { return (bool)GetValue(IsRippleCenteredProperty); }
            set { SetValue(IsRippleCenteredProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsRippleOnTop DependencyProperty
        public static readonly DependencyProperty IsRippleOnTopProperty = DependencyProperty.Register("IsRippleOnTop",
            typeof(bool),
            typeof(MaterialControl),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsRippleOnTop
        {
            get { return (bool)GetValue(IsRippleOnTopProperty); }
            set { SetValue(IsRippleOnTopProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsSmartClipped DependencyProperty
        public static readonly DependencyProperty IsSmartClippedProperty = DependencyProperty.Register("IsSmartClipped",
            typeof(bool),
            typeof(MaterialControl),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsSmartClippedChanged));

        private static void OnIsSmartClippedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (MaterialControl)sender;

            instance.RefreshClip();
        }

        public bool IsSmartClipped
        {
            get { return (bool)GetValue(IsSmartClippedProperty); }
            set { SetValue(IsSmartClippedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region RippleBrush DependencyProperty
        public static readonly DependencyProperty RippleBrushProperty = DependencyProperty.Register("RippleBrush",
            typeof(Brush),
            typeof(MaterialControl),
            new PropertyMetadata(null));

        public Brush RippleBrush
        {
            get { return (Brush)GetValue(RippleBrushProperty); }
            set { SetValue(RippleBrushProperty, value); }
        }
        #endregion

        #region RippleOpacity DependencyProperty
        public static readonly DependencyProperty RippleOpacityProperty = DependencyProperty.Register("RippleOpacity",
            typeof(double),
            typeof(MaterialControl),
            new PropertyMetadata(0.5, null, ConstrainRippleOpacity));

        internal static object ConstrainRippleOpacity(DependencyObject sender, object value)
        {
            var doubleValue = (double)value;

            if (doubleValue < 0.0) doubleValue = 0.0;
            else if (doubleValue > 1.0) doubleValue = 1.0;

            return doubleValue;
        }

        public double RippleOpacity
        {
            get { return (double)GetValue(RippleOpacityProperty); }
            set { SetValue(RippleOpacityProperty, value); }
        }
        #endregion

        #region RippleSize ReadOnly DependencyProperty
        internal static readonly DependencyPropertyKey RippleSizePropertyKey = DependencyProperty.RegisterReadOnly("RippleSize",
            typeof(double),
            typeof(MaterialControl),
            new PropertyMetadata(0.0));

        public static readonly DependencyProperty RippleSizeProperty = RippleSizePropertyKey.DependencyProperty;

        public double RippleSize
        {
            get { return (double)GetValue(RippleSizeProperty); }
            protected set { SetValue(RippleSizePropertyKey, value); }
        }
        #endregion

        #region RippleX ReadOnly DependencyProperty
        internal static readonly DependencyPropertyKey RippleXPropertyKey = DependencyProperty.RegisterReadOnly("RippleX",
            typeof(double),
            typeof(MaterialControl),
            new PropertyMetadata(0.0));

        public static readonly DependencyProperty RippleXProperty = RippleXPropertyKey.DependencyProperty;

        public double RippleX
        {
            get { return (double)GetValue(RippleXProperty); }
            protected set { SetValue(RippleXPropertyKey, value); }
        }
        #endregion

        #region RippleY ReadOnly DependencyProperty
        internal static readonly DependencyPropertyKey RippleYPropertyKey = DependencyProperty.RegisterReadOnly("RippleY",
            typeof(double),
            typeof(MaterialControl),
            new PropertyMetadata(0.0));

        public static readonly DependencyProperty RippleYProperty = RippleYPropertyKey.DependencyProperty;

        public double RippleY
        {
            get { return (double)GetValue(RippleYProperty); }
            protected set { SetValue(RippleYPropertyKey, value); }
        }
        #endregion

        #region IsPressed ReadOnly DependencyProperty
        internal static readonly DependencyPropertyKey IsPressedPropertyKey = DependencyProperty.RegisterReadOnly("IsPressed",
            typeof(bool),
            typeof(MaterialControl),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static readonly DependencyProperty IsPressedProperty = IsPressedPropertyKey.DependencyProperty;

        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            protected set { SetValue(IsPressedPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        bool _templateApplied;

        internal static readonly HashSet<MaterialControl> PressedInstances = new HashSet<MaterialControl>();

        // Clip aktualisieren
        private void RefreshClip()
        {
            // Wenn das Element keine Größe besitzt oder nicht geclipped werden soll, dann abbrechen
            if (!_templateApplied || !IsSmartClipped || ActualWidth < 1.0 || ActualHeight < 1.0)
            {
                Clip = null;
                return;
            }

            // Clip-Geometry erstellen
            var clip = GeometryHelper.GetRoundedRectangle(new Rect(0, 0, ActualWidth, ActualHeight), BorderThickness, CornerRadius);

            clip.Freeze();

            Clip = clip;
        }

        // Ripple-Effekt entfernen, wenn Maus losgelassen wird
        private static void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (PressedInstances.Count == 0) return;

            foreach (var instance in PressedInstances)
            {
                instance.OnMouseButtonUp();
            }

            PressedInstances.Clear();
        }

        // Handled das loslassen der linken Maustaste
        internal virtual void OnMouseButtonUp()
        {
            if (GetTemplateChild("PART_ScaleTransform") is ScaleTransform transform)
            {
                var currentScale = transform.ScaleX;

                var newTime = TimeSpan.FromMilliseconds(300 * (1.0 - currentScale));

                if (GetTemplateChild("PART_MousePressedToNormalScaleXKeyFrame") is EasingDoubleKeyFrame scaleXKeyFrame)
                {
                    scaleXKeyFrame.KeyTime = KeyTime.FromTimeSpan(newTime);
                }

                if (GetTemplateChild("PART_MousePressedToNormalScaleYKeyFrame") is EasingDoubleKeyFrame scaleYKeyFrame)
                {
                    scaleYKeyFrame.KeyTime = KeyTime.FromTimeSpan(newTime);
                }
            }

            IsPressed = false;
            VisualStateManager.GoToState(this, "Normal", true);
        }

        // Ripple-Effekt entfernen, wenn die Maus das Control verlässt
        private static void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (PressedInstances.Count == 0) return;

            var list = PressedInstances.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                var instance = list[i];

                if (sender != instance) instance.OnMouseMoveExternal(e);
            }
        }

        // Handled Mausbewegungen außerhalb unserer Instanz
        internal virtual void OnMouseMoveExternal(MouseEventArgs e)
        {
            var relativePosition = Mouse.GetPosition(this);

            if (relativePosition.X < 0 || relativePosition.Y < 0 || relativePosition.X >= ActualWidth || relativePosition.Y >= ActualHeight)
            {
                IsPressed = false;
                VisualStateManager.GoToState(this, "MouseOut", true);
                PressedInstances.Remove(this);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _templateApplied = true;

            // Control in Anfangszustand versetzen
            VisualStateManager.GoToState(this, "Normal", false);
        }

        // Ripple-Effekt bei Click beginnen
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            // Ist der Effekt zentriert?
            if (IsRippleCentered)
            {
                if (Content is FrameworkElement element)
                {
                    var position = element.TransformToAncestor(this).Transform(new Point(0, 0));

                    if (FlowDirection == FlowDirection.RightToLeft) RippleX = position.X - element.ActualWidth / 2 - RippleSize / 2;
                    else RippleX = position.X + element.ActualWidth / 2 - RippleSize / 2;

                    RippleY = position.Y + element.ActualHeight / 2 - RippleSize / 2;
                }
                else
                {
                    RippleX = ActualWidth / 2 - RippleSize / 2;
                    RippleY = ActualHeight / 2 - RippleSize / 2;
                }
            }
            else
            {
                var point = e.GetPosition(this);
                RippleX = point.X - RippleSize / 2;
                RippleY = point.Y - RippleSize / 2;
            }

            if (IsRippleEnabled)
            {
                IsPressed = true;
                VisualStateManager.GoToState(this, "MousePressed", true);
                PressedInstances.Add(this);
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            VisualStateManager.GoToState(this, "Normal", false);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            var relativePosition = Mouse.GetPosition(this);

            if (relativePosition.X < 0 || relativePosition.Y < 0 || relativePosition.X >= ActualWidth || relativePosition.Y >= ActualHeight)
            {
                if (IsPressed)
                {
                    IsPressed = false;
                    VisualStateManager.GoToState(this, "MouseOut", true);
                    PressedInstances.Remove(this);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Normal", true);
                }
            }

            base.OnMouseLeave(e);
        }

        // Wenn sich die Größe ändert, muss die Größe des Ripple-Effekts neu berechnet werden
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            double width, height;

            if (IsRippleCentered && Content is FrameworkElement element)
            {
                width = element.ActualWidth;
                height = element.ActualHeight;
            }
            else
            {
                width = sizeInfo.NewSize.Width;
                height = sizeInfo.NewSize.Height;
            }

            var radius = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));

            RippleSize = 2 * radius;

            RefreshClip();
        }
    }
}