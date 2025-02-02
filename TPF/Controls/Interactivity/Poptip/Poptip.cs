using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TPF.Controls.Primitives;
using TPF.Internal;

namespace TPF.Controls
{
    public class Poptip : AttachableElement
    {
        static Poptip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Poptip), new FrameworkPropertyMetadata(typeof(Poptip)));
        }

        public Poptip()
        {
            // Popup erstellen und uns als Child setzen
            _popup = CreatePopup();
        }

        #region CornerRadius Attached DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius",
            typeof(CornerRadius),
            typeof(Poptip),
            new PropertyMetadata(default(CornerRadius)));

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region Content Attached DependencyProperty
        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached("Content",
            typeof(object),
            typeof(Poptip),
            new PropertyMetadata(null, OnContentChanged));

        static void OnContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null || e.NewValue is string text && string.IsNullOrWhiteSpace(text)) sender.SetValue(IsOpenProperty, false);

            if (sender is Poptip) return;

            if (GetInstance(sender) == null)
            {
                var poptip = new Poptip() { _isGeneratedInstance = true };
                SetInstance(sender, poptip);
            }
        }

        public static object GetContent(DependencyObject element)
        {
            return element.GetValue(ContentProperty);
        }

        public static void SetContent(DependencyObject element, object value)
        {
            element.SetValue(ContentProperty, value);
        }

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        #endregion

        #region ContentTemplate DependencyProperty
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate",
            typeof(DataTemplate),
            typeof(Poptip),
            new PropertyMetadata(null));

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }
        #endregion

        #region ContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register("ContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Poptip),
            new PropertyMetadata(null));

        public DataTemplateSelector ContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty); }
            set { SetValue(ContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region ContentStringFormat DependencyProperty
        public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat",
            typeof(string),
            typeof(Poptip),
            new PropertyMetadata(null));

        public string ContentStringFormat
        {
            get { return (string)GetValue(ContentStringFormatProperty); }
            set { SetValue(ContentStringFormatProperty, value); }
        }
        #endregion

        #region Offset Attached DependencyProperty
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.RegisterAttached("Offset",
            typeof(double),
            typeof(Poptip),
            new PropertyMetadata(5.0));

        public static object GetOffset(DependencyObject element)
        {
            return element.GetValue(OffsetProperty);
        }

        public static void SetOffset(DependencyObject element, double value)
        {
            element.SetValue(OffsetProperty, value);
        }

        public double Offset
        {
            get { return (double)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }
        #endregion

        #region Placement Attached DependencyProperty
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached("Placement",
            typeof(PoptipPlacement),
            typeof(Poptip),
            new PropertyMetadata(PoptipPlacement.Top));

        public static PoptipPlacement GetPlacement(DependencyObject element)
        {
            return (PoptipPlacement)element.GetValue(PlacementProperty);
        }

        public static void SetPlacement(DependencyObject element, PoptipPlacement value)
        {
            element.SetValue(PlacementProperty, value);
        }

        public PoptipPlacement Placement
        {
            get { return (PoptipPlacement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }
        #endregion

        #region Trigger Attached DependencyProperty
        public static readonly DependencyProperty TriggerProperty = DependencyProperty.RegisterAttached("Trigger",
            typeof(PoptipTrigger),
            typeof(Poptip),
            new PropertyMetadata(PoptipTrigger.Hover));

        public static PoptipTrigger GetTrigger(DependencyObject element)
        {
            return (PoptipTrigger)element.GetValue(TriggerProperty);
        }

        public static void SetTrigger(DependencyObject element, PoptipTrigger value)
        {
            element.SetValue(TriggerProperty, value);
        }

        public PoptipTrigger Trigger
        {
            get { return (PoptipTrigger)GetValue(TriggerProperty); }
            set { SetValue(TriggerProperty, value); }
        }
        #endregion

        #region IsOpen Attached DependencyProperty
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.RegisterAttached("IsOpen",
            typeof(bool),
            typeof(Poptip),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsOpenChanged));

        static void OnIsOpenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Poptip instance) instance.UpdateOpenState((bool)e.NewValue);
            else
            {
                instance = GetInstance(sender) as Poptip;
                if (instance != null) instance.IsOpen = (bool)e.NewValue;
            }
        }

        public static object GetIsOpen(DependencyObject element)
        {
            return element.GetValue(IsOpenProperty);
        }

        public static void SetIsOpen(DependencyObject element, bool value)
        {
            element.SetValue(IsOpenProperty, BooleanBoxes.Box(value));
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        private readonly Popup _popup;
        private bool _isGeneratedInstance;

        protected sealed override void OnTargetChanged(FrameworkElement oldValue, FrameworkElement newValue)
        {
            base.OnTargetChanged(oldValue, newValue);

            if (oldValue != null)
            {
                oldValue.MouseEnter -= TargetElement_MouseEnter;
                oldValue.PreviewMouseDown -= TargetElement_MouseDown;
                oldValue.PreviewMouseUp -= TargetElement_MouseUp;
                oldValue.MouseLeave -= TargetElement_MouseLeave;
                oldValue.GotFocus -= TargetElement_GotFocus;
                oldValue.LostFocus -= TargetElement_LostFocus;
            }

            if (newValue != null)
            {
                newValue.MouseEnter += TargetElement_MouseEnter;
                newValue.PreviewMouseDown += TargetElement_MouseDown;
                newValue.PreviewMouseUp += TargetElement_MouseUp;
                newValue.MouseLeave += TargetElement_MouseLeave;
                newValue.GotFocus += TargetElement_GotFocus;
                newValue.LostFocus += TargetElement_LostFocus;
                _popup.PlacementTarget = newValue;
            }
        }

        private Popup CreatePopup()
        {
            var popup = new Popup()
            {
                Placement = PlacementMode.Custom,
                AllowsTransparency = true,
                IsHitTestVisible = false,
                Child = this,
            };

            // Diese Property wird von ToolTip benutzt, ist aber leider internal
            // Da der Poptip quasi ein ToolTip ist mit anderen Triggern, sollte er sich auch so verhalten wie ein ToolTip
            var hitTestableProperty = typeof(Popup).GetProperty("HitTestable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            hitTestableProperty?.SetValue(popup, false);

            return popup;
        }

        private void TargetElement_MouseEnter(object sender, MouseEventArgs e)
        {
            var trigger = GetActualTrigger();

            if (trigger != PoptipTrigger.Hover) return;

            if (HasContent()) IsOpen = true;
        }

        private void TargetElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var trigger = GetActualTrigger();

            if (trigger != PoptipTrigger.Pressed) return;

            if (HasContent()) IsOpen = true;
        }

        private void TargetElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var trigger = GetActualTrigger();

            if (trigger != PoptipTrigger.Pressed) return;

            IsOpen = false;
        }

        private void TargetElement_MouseLeave(object sender, MouseEventArgs e)
        {
            var trigger = GetActualTrigger();

            if (trigger == PoptipTrigger.Focus || trigger == PoptipTrigger.None) return;

            IsOpen = false;
        }

        private void TargetElement_GotFocus(object sender, RoutedEventArgs e)
        {
            var trigger = GetActualTrigger();

            if (trigger != PoptipTrigger.Focus) return;

            if (HasContent()) IsOpen = true;
        }

        private void TargetElement_LostFocus(object sender, RoutedEventArgs e)
        {
            var trigger = GetActualTrigger();

            if (trigger != PoptipTrigger.Focus) return;

            IsOpen = false;
        }

        private PoptipTrigger GetActualTrigger()
        {
            return _isGeneratedInstance ? GetTrigger(Target) : Trigger;
        }

        private bool HasContent()
        {
            var content = _isGeneratedInstance ? GetContent(Target) : Content;

            if (content == null || (content is string text && string.IsNullOrWhiteSpace(text))) return false;
            else return true;
        }

        private void UpdateOpenState(bool isOpen)
        {
            if (isOpen)
            {
                // Wenn die Content-Property auf dem Target gesetzt wurde, werden die Properties vom Target kopiert
                if (_isGeneratedInstance)
                {
                    SetCurrentValue(ContentProperty, GetContent(Target));
                    SetCurrentValue(PlacementProperty, GetPlacement(Target));
                    SetCurrentValue(TriggerProperty, GetTrigger(Target));
                    SetCurrentValue(OffsetProperty, GetOffset(Target));
                }

                _popup.PlacementTarget = Target;
                UpdatePopupLocation();
            }
            // IsOpen von Popup und Target setzen
            _popup.IsOpen = isOpen;
            Target.SetCurrentValue(IsOpenProperty, isOpen);
        }

        private void UpdatePopupLocation()
        {
            var targetWidth = Target.RenderSize.Width;
            var targetHeight = Target.RenderSize.Height;

            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            var size = DesiredSize;

            var width = size.Width;
            var height = size.Height;
            var poptip = (Poptip)GetInstance(Target);
            var placement = poptip.Placement;
            var offset = poptip.Offset;

            double x = 0.0, y = 0.0, offsetX = 0.0, offsetY = 0.0;

            switch (placement)
            {
                case PoptipPlacement.TopLeft:
                {
                    offsetY = -offset;
                    y = -height;
                    break;
                }
                case PoptipPlacement.Top:
                {
                    offsetX = (targetWidth - width) / 2;
                    offsetY = -offset;
                    y = -height;
                    break;
                }
                case PoptipPlacement.TopRight:
                {
                    offsetX = targetWidth - width;
                    offsetY = -offset;
                    y = -height;
                    break;
                }
                case PoptipPlacement.BottomLeft:
                {
                    offsetY = offset;
                    y = targetHeight;
                    break;
                }
                case PoptipPlacement.Bottom:
                {
                    offsetX = (targetWidth - width) / 2;
                    offsetY = offset;
                    y = targetHeight;
                    break;
                }
                case PoptipPlacement.BottomRight:
                {
                    offsetX = targetWidth - width;
                    offsetY = offset;
                    y = targetHeight;
                    break;
                }
                case PoptipPlacement.LeftTop:
                {
                    offsetX = -offset;
                    x = -width;
                    break;
                }
                case PoptipPlacement.Left:
                {
                    offsetX = -offset;
                    offsetY = (targetHeight - height) / 2;
                    x = -width;
                    break;
                }
                case PoptipPlacement.LeftBottom:
                {
                    offsetX = -offset;
                    offsetY = targetHeight - height;
                    x = -width;
                    break;
                }
                case PoptipPlacement.RightTop:
                {
                    offsetX = offset;
                    x = targetWidth;
                    break;
                }
                case PoptipPlacement.Right:
                {
                    offsetX = offset;
                    offsetY = (targetHeight - height) / 2;
                    x = targetWidth;
                    break;
                }
                case PoptipPlacement.RightBottom:
                {
                    offsetX = offset;
                    offsetY = targetHeight - height;
                    x = targetWidth;
                    break;
                }
            }

            _popup.HorizontalOffset = offsetX;
            _popup.VerticalOffset = offsetY;
            // Wenn in Windows der Linkshänder-Modus aktiviert ist stimmt die Automatisch bestimmte Position nicht mehr, also müssen wir die hier selbst festlegen
            var popupPlacement = new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.None);
            _popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback((popupSize, targetSize, offsetPoint) => new[] { popupPlacement });
        }

        protected override void Dispose()
        {
            IsOpen = false;
        }
    }
}