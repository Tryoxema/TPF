using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    [TemplatePart(Name = "PART_HexStringTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_SaturationPad", Type = typeof(Pad))]
    [TemplatePart(Name = "PART_HuePad", Type = typeof(Pad))]
    public class ColorEditor : Control
    {
        static ColorEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorEditor), new FrameworkPropertyMetadata(typeof(ColorEditor)));

            EventManager.RegisterClassHandler(typeof(ColorEditor), Thumb.DragStartedEvent, new DragStartedEventHandler(OnDragStarted));
            EventManager.RegisterClassHandler(typeof(ColorEditor), Thumb.DragCompletedEvent, new DragCompletedEventHandler(OnDragCompleted));

            EventManager.RegisterClassHandler(typeof(ColorEditor), EyeDropper.BeginColorPickingEvent, new RoutedEventHandler(OnBeginColorPicking));
            EventManager.RegisterClassHandler(typeof(ColorEditor), EyeDropper.ColorChangedEvent, new ColorChangeEventHandler(OnColorPicking));
            EventManager.RegisterClassHandler(typeof(ColorEditor), EyeDropper.CancelColorPickingEvent, new RoutedEventHandler(OnCancelColorPicking));
            EventManager.RegisterClassHandler(typeof(ColorEditor), EyeDropper.EndColorPickingEvent, new ColorChangeEventHandler(OnEndColorPicking));
        }

        bool _suppressPropertyChanged;
        bool _suppressSelectorUpdate;
        bool _suppressHexStringChanged;
        bool _selectionMoving;

        #region SelectedColorChanged RoutedEvent
        public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent("SelectedColorChanged",
            RoutingStrategy.Bubble,
            typeof(ColorChangeEventHandler),
            typeof(ColorEditor));

        public event ColorChangeEventHandler SelectedColorChanged
        {
            add => AddHandler(SelectedColorChangedEvent, value);
            remove => RemoveHandler(SelectedColorChangedEvent, value);
        }
        #endregion

        #region SelectedColor DependencyProperty
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor",
            typeof(Color),
            typeof(ColorEditor),
            new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));

        private static void OnSelectedColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ColorEditor)sender;

            instance.OnSelectedColorChanged();
        }

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
        #endregion

        #region Red DependencyProperty
        public static readonly DependencyProperty RedProperty = DependencyProperty.Register("Red",
            typeof(byte),
            typeof(ColorEditor),
            new PropertyMetadata((byte)255, OnColorValueChanged));

        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }
        #endregion

        #region Green DependencyProperty
        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register("Green",
            typeof(byte),
            typeof(ColorEditor),
            new PropertyMetadata((byte)255, OnColorValueChanged));

        public byte Green
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }
        #endregion

        #region Blue DependencyProperty
        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register("Blue",
            typeof(byte),
            typeof(ColorEditor),
            new PropertyMetadata((byte)255, OnColorValueChanged));

        public byte Blue
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }
        #endregion

        #region Alpha DependencyProperty
        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha",
            typeof(byte),
            typeof(ColorEditor),
            new PropertyMetadata((byte)255, OnColorValueChanged));

        public byte Alpha
        {
            get { return (byte)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }
        #endregion

        #region HexString DependencyProperty
        public static readonly DependencyProperty HexStringProperty = DependencyProperty.Register("HexString",
            typeof(string),
            typeof(ColorEditor),
            new PropertyMetadata(null, OnHexStringChanged, OnCoerceHexString));

        private static object OnCoerceHexString(DependencyObject d, object newValue)
        {
            var instance = (ColorEditor)d;

            var value = (string)newValue;

            try
            {
                if (string.IsNullOrWhiteSpace(value)) newValue = "FFFFFFFF";
                else
                {
                    if (int.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out var _)) value = $"#{value}";
                    ColorConverter.ConvertFromString(value);
                }
            }
            catch
            {
                // Wenn der Wert nicht Konvertiert werden kann, dann zurücksetzen auf den alten Wert
                newValue = instance.HexString;
            }

            return newValue;
        }

        private static void OnHexStringChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ColorEditor)sender;

            if (instance._suppressHexStringChanged) return;

            instance.OnHexStringChanged();
        }

        public string HexString
        {
            get { return (string)GetValue(HexStringProperty); }
            set { SetValue(HexStringProperty, value); }
        }
        #endregion

        private static void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            var instance = (ColorEditor)sender;

            instance._selectionMoving = true;
        }

        private static void OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            var instance = (ColorEditor)sender;

            instance._selectionMoving = false;
            instance.RaiseSelectedColorChanged();
        }

        private static void OnBeginColorPicking(object sender, RoutedEventArgs e)
        {
            var instance = (ColorEditor)sender;

            instance._selectionMoving = true;
        }

        private static void OnColorPicking(object sender, ColorChangeEventArgs e)
        {
            var instance = (ColorEditor)sender;

            instance.SelectedColor = e.Color;
        }

        private static void OnCancelColorPicking(object sender, RoutedEventArgs e)
        {
            var instance = (ColorEditor)sender;

            instance._selectionMoving = false;
        }

        private static void OnEndColorPicking(object sender, ColorChangeEventArgs e)
        {
            var instance = (ColorEditor)sender;

            instance._selectionMoving = false;
            instance.RaiseSelectedColorChanged();
        }

        private static void OnColorValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ColorEditor)sender;

            if (instance._suppressPropertyChanged) return;

            instance.OnColorValueChanged();
        }

        internal TextBox HexStringTextBox;
        internal Pad SaturationPad;
        internal Pad HuePad;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Falls das Template neu geladen wurde, alte Handler entfernen
            RemoveHandlers();

            HexStringTextBox = GetTemplateChild("PART_HexStringTextBox") as TextBox;
            SaturationPad = GetTemplateChild("PART_SaturationPad") as Pad;
            HuePad = GetTemplateChild("PART_HuePad") as Pad;

            // EventHandler hinzufügen
            AddHandlers();

            UpdateRGBValues();
            UpdateHexString();
            UpdateSelectorPositions(SelectedColor);
        }

        private void AddHandlers()
        {
            if (HexStringTextBox != null)
            {
                HexStringTextBox.KeyDown += TextBox_KeyDown;
            }

            if (SaturationPad != null)
            {
                SaturationPad.PositionPointChanging += Pad_PositionPointChanging;
                SaturationPad.PositionPointChanged += Pad_PositionPointChanged;
            }

            if (HuePad != null)
            {
                HuePad.PositionPointChanging += Pad_PositionPointChanging;
                HuePad.PositionPointChanged += Pad_PositionPointChanged;
            }
        }

        private void RemoveHandlers()
        {
            if (HexStringTextBox != null)
            {
                HexStringTextBox.KeyDown -= TextBox_KeyDown;
            }

            if (SaturationPad != null)
            {
                SaturationPad.PositionPointChanging -= Pad_PositionPointChanging;
                SaturationPad.PositionPointChanged -= Pad_PositionPointChanged;
            }

            if (HuePad != null)
            {
                HuePad.PositionPointChanging -= Pad_PositionPointChanging;
                HuePad.PositionPointChanged -= Pad_PositionPointChanged;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                HexString = HexStringTextBox.Text;
            }
        }

        private void Pad_PositionPointChanging(object sender, PositionPointChangeEventArgs e)
        {
            if (_suppressSelectorUpdate) return;
            _selectionMoving = true;
            UpdateColorFromPadPoints();
        }

        private void Pad_PositionPointChanged(object sender, PositionPointChangeEventArgs e)
        {
            _selectionMoving = false;
            RaiseSelectedColorChanged();
        }

        protected void RaiseSelectedColorChanged()
        {
            var eventArgs = new ColorChangeEventArgs(SelectedColor)
            {
                RoutedEvent = SelectedColorChangedEvent
            };

            RaiseEvent(eventArgs);
        }

        private void UpdateColorFromPadPoints()
        {
            var color = ColorHelper.ConvertHsvToRgb(360 * HuePad?.RelativePositionPoint.Y ?? 0, SaturationPad?.RelativePositionPoint.X ?? 1, 1 - SaturationPad?.RelativePositionPoint.Y ?? 1);
            _suppressSelectorUpdate = true;
            SelectedColor = color;
            _suppressSelectorUpdate = false;
        }

        // Wurde die SelectedColor geändert?
        protected virtual void OnSelectedColorChanged()
        {
            // HexString Updaten
            UpdateHexString();
            // RGBA Updaten
            UpdateRGBValues();
            // SelectorPosition Updaten
            UpdateSelectorPositions(SelectedColor);

            if (!_selectionMoving) RaiseSelectedColorChanged();
        }

        // Wurde einer der RGBA-Werte geändert?
        protected virtual void OnColorValueChanged()
        {
            UpdateSelectedColor();
        }

        // Wurde der HexString geändert?
        protected virtual void OnHexStringChanged()
        {
            var currentColorString = ColorToString(SelectedColor);

            if (HexString.StartsWith("#"))
            {
                _suppressHexStringChanged = true;
                HexString = HexString.Remove(0, 1);
                _suppressHexStringChanged = false;
            }

            if (!currentColorString.Equals(HexString))
            {
                try
                {
                    SelectedColor = (Color)ColorConverter.ConvertFromString($"#{HexString}");
                }
                catch (Exception)
                {
                    SelectedColor = Colors.White;
                }
            }
        }

        protected virtual void UpdateSelectedColor()
        {
            SelectedColor = Color.FromArgb(Alpha, Red, Green, Blue);
        }

        protected virtual void UpdateRGBValues()
        {
            //if (SelectedColor == null) return;

            _suppressPropertyChanged = true;

            Alpha = SelectedColor.A;
            Red = SelectedColor.R;
            Green = SelectedColor.G;
            Blue = SelectedColor.B;

            _suppressPropertyChanged = false;
        }

        protected virtual void UpdateHexString()
        {
            //if (SelectedColor == null) return;

            HexString = ColorToString(SelectedColor);
        }

        protected virtual void UpdateSelectorPositions(Color color)
        {
            if (_suppressSelectorUpdate) return;

            var hsvColor = ColorHelper.ConvertRgbToHsv(color.R, color.G, color.B);

            _suppressSelectorUpdate = true;

            if (HuePad != null)
            {
                var huePoint = new Point(0, hsvColor.H / 360);
                HuePad.RelativePositionPoint = huePoint; 
            }

            if (SaturationPad != null)
            {
                var saturationPoint = new Point(hsvColor.S, 1 - hsvColor.V);
                SaturationPad.RelativePositionPoint = saturationPoint; 
            }

            _suppressSelectorUpdate = false;
        }

        protected virtual string ColorToString(Color color)
        {
            return $"{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}