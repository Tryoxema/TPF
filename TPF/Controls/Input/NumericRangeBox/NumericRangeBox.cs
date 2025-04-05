using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using TPF.Internal;
using TPF.Calculation;
using System.Windows.Controls.Primitives;

namespace TPF.Controls
{
    public class NumericRangeBox : Control
    {
        public NumericRangeBox()
        {
            NumberFormatInfo = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
        }

        static NumericRangeBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericRangeBox), new FrameworkPropertyMetadata(typeof(NumericRangeBox)));
        }

        #region ValueChanged RoutedEvent
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<double?>),
            typeof(NumericRangeBox));

        public event RoutedPropertyChangedEventHandler<double?> ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }
        #endregion

        #region Minimum DependencyProperty
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum",
            typeof(double),
            typeof(NumericRangeBox),
            new PropertyMetadata(double.MinValue));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        #endregion

        #region Maximum DependencyProperty
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum",
            typeof(double),
            typeof(NumericRangeBox),
            new PropertyMetadata(double.MaxValue));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        #endregion

        #region SmallChange DependencyProperty
        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register("SmallChange",
            typeof(double),
            typeof(NumericRangeBox),
            new PropertyMetadata(1.0));

        public double SmallChange
        {
            get { return (double)GetValue(SmallChangeProperty); }
            set { SetValue(SmallChangeProperty, value); }
        }
        #endregion

        #region LargeChange DependencyProperty
        public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register("LargeChange",
            typeof(double),
            typeof(NumericRangeBox),
            new PropertyMetadata(10.0));

        public double LargeChange
        {
            get { return (double)GetValue(LargeChangeProperty); }
            set { SetValue(LargeChangeProperty, value); }
        }
        #endregion

        #region NumberDecimalDigits DependencyProperty
        public static readonly DependencyProperty NumberDecimalDigitsProperty = DependencyProperty.Register("NumberDecimalDigits",
            typeof(int),
            typeof(NumericRangeBox),
            new PropertyMetadata(2, NumberDecimalDigitsPropertyChanged, ConstrainNumberDecimalDigits));

        private static void NumberDecimalDigitsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (NumericRangeBox)sender;

            instance.NumberFormatInfo.NumberDecimalDigits = instance.NumberDecimalDigits;
            // Text neu formatieren
            instance.FormatText();
        }

        private static object ConstrainNumberDecimalDigits(DependencyObject sender, object value)
        {
            var decimalDigits = (int)value;

            if (decimalDigits < 0) decimalDigits = 0;
            else if (decimalDigits > 99) decimalDigits = 99;

            return decimalDigits;
        }

        public int NumberDecimalDigits
        {
            get { return (int)GetValue(NumberDecimalDigitsProperty); }
            set { SetValue(NumberDecimalDigitsProperty, value); }
        }
        #endregion

        #region NumberDecimalSeparator DependencyProperty
        public static readonly DependencyProperty NumberDecimalSeparatorProperty = DependencyProperty.Register("NumberDecimalSeparator",
            typeof(string),
            typeof(NumericRangeBox),
            new PropertyMetadata(null, NumberDecimalSeparatorPropertyChanged));

        private static void NumberDecimalSeparatorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (NumericRangeBox)sender;
            // Wenn der Wert auf null gesetzt wird soll der Standardwert genutzt werden
            instance.NumberFormatInfo.NumberDecimalSeparator = instance.NumberDecimalSeparator ?? CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            // Text neu formatieren
            instance.FormatText();
        }

        public string NumberDecimalSeparator
        {
            get { return (string)GetValue(NumberDecimalSeparatorProperty); }
            set { SetValue(NumberDecimalSeparatorProperty, value); }
        }
        #endregion

        #region NumberGroupSeparator DependencyProperty
        public static readonly DependencyProperty NumberGroupSeparatorProperty = DependencyProperty.Register("NumberGroupSeparator",
            typeof(string),
            typeof(NumericRangeBox),
            new PropertyMetadata(null, NumberGroupSeparatorPropertyChanged));

        private static void NumberGroupSeparatorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (NumericRangeBox)sender;
            // Wenn der Wert auf null gesetzt wird soll der Standardwert genutzt werden
            instance.NumberFormatInfo.NumberGroupSeparator = instance.NumberGroupSeparator ?? CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            // Text neu formatieren
            instance.FormatText();
        }

        public string NumberGroupSeparator
        {
            get { return (string)GetValue(NumberGroupSeparatorProperty); }
            set { SetValue(NumberGroupSeparatorProperty, value); }
        }
        #endregion

        #region CustomUnit DependencyProperty
        public static readonly DependencyProperty CustomUnitProperty = DependencyProperty.Register("CustomUnit",
            typeof(string),
            typeof(NumericRangeBox),
            new PropertyMetadata(null, CustomUnitPropertyChanged));

        private static void CustomUnitPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (NumericRangeBox)sender;

            instance.FormatText();
        }

        public string CustomUnit
        {
            get { return (string)GetValue(CustomUnitProperty); }
            set { SetValue(CustomUnitProperty, value); }
        }
        #endregion

        #region Value DependencyProperty
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(double?),
            typeof(NumericRangeBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValuePropertyChanged, ConstrainValue));

        private static void ValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (NumericRangeBox)sender;
            // Text formatieren wenn gewünscht
            if (instance._setText) instance.FormatText();

            instance.OnValueChanged((double?)e.OldValue, (double?)e.NewValue);
        }

        private static object ConstrainValue(DependencyObject sender, object value)
        {
            var instance = (NumericRangeBox)sender;

            var doubleValue = (double?)value;

            if (doubleValue == null)
            {
                if (instance.TextBox != null) instance.TextBox.Text = string.Empty;
                return value;
            }
            doubleValue = Math.Round(doubleValue.Value, instance.NumberDecimalDigits, MidpointRounding.AwayFromZero);
            if (doubleValue < instance.Minimum) doubleValue = instance.Minimum;
            else if (doubleValue > instance.Maximum) doubleValue = instance.Maximum;

            return doubleValue;
        }

        public double? Value
        {
            get { return (double?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region NumericValueType ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey NumericValueTypePropertyKey = DependencyProperty.RegisterReadOnly("NumericValueType",
            typeof(NumericValueType),
            typeof(NumericRangeBox),
            new PropertyMetadata(NumericValueType.Null));

        public static readonly DependencyProperty NumericValueTypeProperty = NumericValueTypePropertyKey.DependencyProperty;

        public NumericValueType NumericValueType
        {
            get { return (NumericValueType)GetValue(NumericValueTypeProperty); }
            protected set { SetValue(NumericValueTypePropertyKey, value); }
        }
        #endregion

        #region ShowButtons DependencyProperty
        public static readonly DependencyProperty ShowButtonsProperty = DependencyProperty.Register("ShowButtons",
            typeof(bool),
            typeof(NumericRangeBox),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool ShowButtons
        {
            get { return (bool)GetValue(ShowButtonsProperty); }
            set { SetValue(ShowButtonsProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsReadOnly DependencyProperty
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly",
            typeof(bool),
            typeof(NumericRangeBox),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region AllowCalculations DependencyProperty
        public static readonly DependencyProperty AllowCalculationsProperty = DependencyProperty.Register("AllowCalculations",
            typeof(bool),
            typeof(NumericRangeBox),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool AllowCalculations
        {
            get { return (bool)GetValue(AllowCalculationsProperty); }
            set { SetValue(AllowCalculationsProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region SelectionOnFocus DependencyProperty
        public static readonly DependencyProperty SelectionOnFocusProperty = DependencyProperty.Register("SelectionOnFocus",
            typeof(SelectionOnFocus),
            typeof(NumericRangeBox),
            new PropertyMetadata(SelectionOnFocus.SelectAll));

        public SelectionOnFocus SelectionOnFocus
        {
            get { return (SelectionOnFocus)GetValue(SelectionOnFocusProperty); }
            set { SetValue(SelectionOnFocusProperty, value); }
        }
        #endregion

        #region HideTrailingZeros DependencyProperty
        public static readonly DependencyProperty HideTrailingZerosProperty = DependencyProperty.Register("HideTrailingZeros",
            typeof(bool),
            typeof(NumericRangeBox),
            new PropertyMetadata(BooleanBoxes.FalseBox, HideTrailingZerosPropertyChanged));

        private static void HideTrailingZerosPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (NumericRangeBox)sender;

            instance.FormatText();
        }

        public bool HideTrailingZeros
        {
            get { return (bool)GetValue(HideTrailingZerosProperty); }
            set { SetValue(HideTrailingZerosProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region PositiveForeground DependencyProperty
        public static readonly DependencyProperty PositiveForegroundProperty = DependencyProperty.Register("PositiveForeground",
            typeof(Brush),
            typeof(NumericRangeBox),
            new PropertyMetadata(null));

        public Brush PositiveForeground
        {
            get { return (Brush)GetValue(PositiveForegroundProperty); }
            set { SetValue(PositiveForegroundProperty, value); }
        }
        #endregion

        #region NegativeForeground DependencyProperty
        public static readonly DependencyProperty NegativeForegroundProperty = DependencyProperty.Register("NegativeForeground",
            typeof(Brush),
            typeof(NumericRangeBox),
            new PropertyMetadata(null));

        public Brush NegativeForeground
        {
            get { return (Brush)GetValue(NegativeForegroundProperty); }
            set { SetValue(NegativeForegroundProperty, value); }
        }
        #endregion

        #region ZeroForeground DependencyProperty
        public static readonly DependencyProperty ZeroForegroundProperty = DependencyProperty.Register("ZeroForeground",
            typeof(Brush),
            typeof(NumericRangeBox),
            new PropertyMetadata(null));

        public Brush ZeroForeground
        {
            get { return (Brush)GetValue(ZeroForegroundProperty); }
            set { SetValue(ZeroForegroundProperty, value); }
        }
        #endregion

        #region ApplyPositiveForeground DependencyProperty
        public static readonly DependencyProperty ApplyPositiveForegroundProperty = DependencyProperty.Register("ApplyPositiveForeground",
            typeof(bool),
            typeof(NumericRangeBox),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool ApplyPositiveForeground
        {
            get { return (bool)GetValue(ApplyPositiveForegroundProperty); }
            set { SetValue(ApplyPositiveForegroundProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ApplyNegativeForeground DependencyProperty
        public static readonly DependencyProperty ApplyNegativeForegroundProperty = DependencyProperty.Register("ApplyNegativeForeground",
            typeof(bool),
            typeof(NumericRangeBox),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool ApplyNegativeForeground
        {
            get { return (bool)GetValue(ApplyNegativeForegroundProperty); }
            set { SetValue(ApplyNegativeForegroundProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ApplyZeroForeground DependencyProperty
        public static readonly DependencyProperty ApplyZeroForegroundProperty = DependencyProperty.Register("ApplyZeroForeground",
            typeof(bool),
            typeof(NumericRangeBox),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool ApplyZeroForeground
        {
            get { return (bool)GetValue(ApplyZeroForegroundProperty); }
            set { SetValue(ApplyZeroForegroundProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        NumberFormatInfo _numberFormatInfo;
        public NumberFormatInfo NumberFormatInfo
        {
            get => _numberFormatInfo;
            set
            {
                // Sicherstellen das immer eine NumberFormatInfo existiert um das setzen der Properties ohne NullReferenceException zu garantieren
                if (value == null) _numberFormatInfo = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
                else _numberFormatInfo = value;
            }
        }

        private RepeatButton _increaseButton;
        private RepeatButton _decreaseButton;
        internal TextBox TextBox;

        private bool _setText = true;

        public static ReadOnlyCollection<char> AllowedOperators { get; } = Array.AsReadOnly(new[] { '+', '-', '*', '/', '^', '(', ')' });

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Wurde das Template schonmal geladen und die Events registriert?
            if (TextBox != null)
            {
                // Events der alten TextBox abmelden um MemoryLeak zu verhindern
                TextBox.PreviewTextInput -= TextBox_PreviewTextInput;
                TextBox.MouseDoubleClick -= TextBox_MouseDoubleClick;
                TextBox.PreviewKeyDown -= TextBox_PreviewKeyDown;
                TextBox.PreviewMouseDown -= TextBox_PreviewMouseDown;
                DataObject.RemovePastingHandler(TextBox, TextBoxPasting);
            }

            if (_increaseButton != null)
            {
                _increaseButton.Click -= IncreaseButton_Click;
                _increaseButton.LostMouseCapture -= Button_LostMouseCapture;
            }

            if (_decreaseButton != null)
            {
                _decreaseButton.Click -= DecreaseButton_Click;
                _decreaseButton.LostMouseCapture -= Button_LostMouseCapture;
            }

            TextBox = GetTemplateChild("PART_TextBox") as TextBox;
            _increaseButton = GetTemplateChild("PART_IncreaseButton") as RepeatButton;
            _decreaseButton = GetTemplateChild("PART_DecreaseButton") as RepeatButton;

            if (TextBox != null)
            {
                // Copy-Paste-Handler zum Verhindern von ungewolltem Inhalt
                DataObject.AddPastingHandler(TextBox, TextBoxPasting);

                // EventHandler hinzufügen
                TextBox.PreviewTextInput += TextBox_PreviewTextInput;
                TextBox.MouseDoubleClick += TextBox_MouseDoubleClick;
                TextBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                TextBox.PreviewMouseDown += TextBox_PreviewMouseDown;
            }

            if (_increaseButton != null)
            {
                _increaseButton.Click += IncreaseButton_Click;
                _increaseButton.LostMouseCapture += Button_LostMouseCapture;
            }

            if (_decreaseButton != null)
            {
                _decreaseButton.Click += DecreaseButton_Click;
                _decreaseButton.LostMouseCapture += Button_LostMouseCapture;
            }

            FormatText();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;

            for (int i = 0; i < e.Text.Length; i++)
            {
                var charItem = e.Text[i];
                // In string umwandeln zum einfachen Vergleichen
                var stringItem = charItem.ToString();

                if (AllowCalculations)
                {
                    // Handelt es sich um das Dezimal-Trennzeichen?
                    if (stringItem == NumberFormatInfo.NumberDecimalSeparator)
                    {
                        // Text vor dem Caret prüfen wenn nötig
                        if (textBox.CaretIndex > 0)
                        {
                            var text = textBox.Text.Substring(0, textBox.CaretIndex);

                            for (int j = text.Length - 1; j >= 0; j--)
                            {
                                var currentChar = text[j];
                                var currentString = currentChar.ToString();
                                // Wenn eine neue Zahl anfängt, prüfung beenden
                                if (AllowedOperators.Any(x => x == currentChar) || currentString == " ") break;
                                else if (currentString == NumberFormatInfo.NumberDecimalSeparator)
                                {
                                    // Wenn innerhalb der gleichen Zahl das Dezimal-Trennzeichen gefunden wurde, Input abbrechen
                                    e.Handled = true;
                                    break;
                                }
                            }
                        }
                        // Text nach dem Caret prüfen wenn nötig
                        if (!e.Handled && textBox.Text != null && textBox.CaretIndex < textBox.Text.Length)
                        {
                            var text = textBox.Text.Substring(textBox.CaretIndex, textBox.Text.Length - textBox.CaretIndex);

                            for (int j = 0; j < text.Length; j++)
                            {
                                var currentChar = text[j];
                                var currentString = currentChar.ToString();
                                // Wenn eine neue Zahl anfängt, prüfung beenden
                                if (AllowedOperators.Any(x => x == currentChar) || currentString == " ") break;
                                else if (currentString == NumberFormatInfo.NumberDecimalSeparator)
                                {
                                    // Wenn innerhalb der gleichen Zahl das Dezimal-Trennzeichen gefunden wurde, Input abbrechen
                                    e.Handled = true;
                                    break;
                                }
                            }
                        }
                    }
                    else if (AllowedOperators.Any(x => x == charItem)) e.Handled = false;
                    else if (!char.IsNumber(charItem) && stringItem != NumberFormatInfo.NumberDecimalSeparator) e.Handled = true;
                }
                else
                {
                    if (charItem == '-' && TextBox.CaretIndex == 0) e.Handled = false;
                    else if (!char.IsNumber(charItem) && stringItem != NumberFormatInfo.NumberDecimalSeparator) e.Handled = true;
                    else if (stringItem == NumberFormatInfo.NumberDecimalSeparator && textBox.Text.Contains(stringItem)) e.Handled = true;
                }
            }
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox.SelectAll();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyDown(e);
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!TextBox.IsFocused)
            {
                TextBox.Focus();
                // Event auf Handled setzen, damit SelectAll funktioniert
                e.Handled = true;
            }
        }

        private void HandleKeyDown(KeyEventArgs e)
        {
            if (IsReadOnly) return;

            switch (e.Key)
            {
                case Key.Enter:
                {
                    ParseInputText();
                    break;
                }
                case Key.Up:
                {
                    Value += SmallChange;
                    e.Handled = true;
                    break;
                }
                case Key.Down:
                {
                    Value -= SmallChange;
                    e.Handled = true;
                    break;
                }
                case Key.PageUp:
                {
                    Value += LargeChange;
                    e.Handled = true;
                    break;
                }
                case Key.PageDown:
                {
                    Value -= LargeChange;
                    e.Handled = true;
                    break;
                }
            }
        }

        private void SelectOnFocus()
        {
            if (TextBox == null) return;

            switch (SelectionOnFocus)
            {
                case SelectionOnFocus.Default: break;
                case SelectionOnFocus.CaretAtBeginning:
                    TextBox.CaretIndex = 0;
                    break;
                case SelectionOnFocus.CaretAtEnd:
                    TextBox.CaretIndex = TextBox.Text.Length;
                    break;
                case SelectionOnFocus.SelectAll:
                    TextBox.SelectAll();
                    break;
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            TextBox?.Focus();
            if (!IsReadOnly) FormatText();

            SelectOnFocus();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (!IsReadOnly) ParseInputText();

            base.OnLostFocus(e);
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusWithinChanged(e);

            var isFocused = (bool)e.NewValue;

            if (isFocused)
            {
                TextBox?.Focus();
                if (!IsReadOnly) FormatText();

                SelectOnFocus();
            }
            else
            {
                if (!IsReadOnly) ParseInputText();
            }
        }

        protected void ParseInputText()
        {
            double? value = null;

            var text = TextBox.Text;

            if (!string.IsNullOrWhiteSpace(text))
            {
                // Ist es eine Zahl die direkt gesetzt werden kann?
                if (double.TryParse(text, NumberStyles.Any, NumberFormatInfo, out var result))
                {
                    value = result;
                }
                // Wenn es keine einfache Zahl ist, prüfen ob Rechnungen erlaubt sind
                else if (AllowCalculations)
                {
                    MathParser parser;

                    // Parser mit besonderem Separator erstellen sofern ein gültiger angegeben ist
                    if (char.TryParse(NumberFormatInfo.NumberDecimalSeparator, out var separator)) parser = new MathParser(separator);
                    else parser = new MathParser();

                    try
                    {
                        // Formel Parsen
                        value = parser.Parse(text);
                    }
                    catch { }
                }
            }

            SetValueProperty(value);
        }

        protected void FormatText()
        {
            if (TextBox == null) return;

            // Wenn kein Wert vorhanden ist dann Text leeren
            if (Value == null)
            {
                TextBox.Text = string.Empty;
                return;
            }

            var formatString = "{0:n}";
            // Text formatieren
            var text = string.Format(NumberFormatInfo, formatString, Value);

            if (HideTrailingZeros)
            {
                text = text.TrimEnd('0');
                if (text.Length > 1)
                {
                    var lastChar = text[text.Length - 1];
                    if (!char.IsNumber(lastChar) && lastChar.ToString() == NumberFormatInfo.NumberDecimalSeparator) text = text.Remove(text.Length - 1);
                }
            }

            // Wenn die TextBox keinen Focus hat und eine CustomUnit angegeben ist, dann diese anhängen
            if (!IsKeyboardFocusWithin && !string.IsNullOrWhiteSpace(CustomUnit)) text += $" {CustomUnit}";

            TextBox.Text = text;

            if (IsKeyboardFocusWithin && TextBox.Text != null) TextBox.CaretIndex = TextBox.Text.Length;
        }

        protected void SetValueProperty(double? newValue)
        {
            var oldValue = Value;

            // Falls ein Wert angegeben wurde, dann sicherstellen das Minimum und Maximum eingehalten werden
            if (newValue != null) newValue = Math.Max(Minimum, Math.Min(Maximum, newValue.Value));

            var isEqual = oldValue == newValue;

            if (!isEqual)
            {
                // Verhindern das der Text doppelt gesetzt wird
                _setText = false;
                Value = newValue;
            }

            _setText = true;

            // Text Formatieren
            FormatText();
        }

        protected virtual void OnValueChanged(double? oldValue, double? newValue)
        {
            if (newValue == null) NumericValueType = NumericValueType.Null;
            else if (double.IsNaN(newValue.Value)) NumericValueType = NumericValueType.NaN;
            else if (double.IsPositiveInfinity(newValue.Value)) NumericValueType = NumericValueType.PositiveInfinity;
            else if (double.IsNegativeInfinity(newValue.Value)) NumericValueType = NumericValueType.NegativeInfinity;
            else if (newValue == 0) NumericValueType = NumericValueType.Zero;
            else if (newValue > 0) NumericValueType = NumericValueType.Positive;
            else if (newValue < 0) NumericValueType = NumericValueType.Negative;

            var eventArgs = new RoutedPropertyChangedEventArgs<double?>(oldValue, newValue) { RoutedEvent = ValueChangedEvent };

            RaiseEvent(eventArgs);
        }

        // Einfügen von Nicht-Zahlen-Werten verhindern
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                // Wenn der einzufügende Text keine Zahl ist dann nicht erlauben
                if (!double.TryParse(text, NumberStyles.Any, NumberFormatInfo, out var result)) e.CancelCommand();
            }
            else e.CancelCommand();
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsReadOnly) return;

            Value += SmallChange;
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsReadOnly) return;

            Value -= SmallChange;
        }

        private void Button_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if (TextBox == null) return;

            TextBox.Focus();
        }
    }
}