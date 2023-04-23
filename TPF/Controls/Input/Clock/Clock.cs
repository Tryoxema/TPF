using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Globalization;
using TPF.Internal;

namespace TPF.Controls
{
    public class Clock : Control
    {
        static Clock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Clock), new FrameworkPropertyMetadata(typeof(Clock)));
        }

        #region SelectedTimeChanged RoutedEvent
        public static readonly RoutedEvent SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedTimeChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<DateTime?>),
            typeof(NumericRangeBox));

        public event RoutedPropertyChangedEventHandler<DateTime?> SelectedTimeChanged
        {
            add => AddHandler(SelectedTimeChangedEvent, value);
            remove => RemoveHandler(SelectedTimeChangedEvent, value);
        }
        #endregion

        #region DisplayMode DependencyProperty
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode",
            typeof(ClockDisplayMode),
            typeof(Clock),
            new PropertyMetadata(ClockDisplayMode.Clock));

        public ClockDisplayMode DisplayMode
        {
            get { return (ClockDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }
        #endregion

        #region ActiveInputMode DependencyProperty
        public static readonly DependencyProperty ActiveInputModeProperty = DependencyProperty.Register("ActiveInputMode",
            typeof(ClockInputMode),
            typeof(Clock),
            new PropertyMetadata(ClockInputMode.Hours, OnActiveInputModeChanged));

        private static void OnActiveInputModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Clock)sender;

            instance.UpdateInputModeButtons();
        }

        public ClockInputMode ActiveInputMode
        {
            get { return (ClockInputMode)GetValue(ActiveInputModeProperty); }
            set { SetValue(ActiveInputModeProperty, value); }
        }
        #endregion

        #region SelectedTime DependencyProperty
        public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register("SelectedTime",
            typeof(DateTime?),
            typeof(Clock),
            new PropertyMetadata(null, OnSelectedTimeChanged));

        private static void OnSelectedTimeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Clock)sender;

            instance.UpdateSelectedTimeString();
            instance.SyncListBoxesWithValue();
            instance.SyncClockButtonsWithValue();
            instance.UpdateMeridiemButtons();
            instance.RaiseSelectedTimeChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue);
        }

        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }
        #endregion

        #region SelectedTimeString ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey SelectedTimeStringPropertyKey = DependencyProperty.RegisterReadOnly("SelectedTimeString",
            typeof(string),
            typeof(Clock),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedTimeStringProperty = SelectedTimeStringPropertyKey.DependencyProperty;

        public string SelectedTimeString
        {
            get { return (string)GetValue(SelectedTimeStringProperty); }
            protected set { SetValue(SelectedTimeStringPropertyKey, value); }
        }
        #endregion

        #region ShowSeconds DependencyProperty
        public static readonly DependencyProperty ShowSecondsProperty = DependencyProperty.Register("ShowSeconds",
            typeof(bool),
            typeof(Clock),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnShowSecondsChanged));

        private static void OnShowSecondsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Clock)sender;

            instance.UpdateSelectedTimeString();
        }

        public bool ShowSeconds
        {
            get { return (bool)GetValue(ShowSecondsProperty); }
            set { SetValue(ShowSecondsProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region Is24Hours DependencyProperty
        public static readonly DependencyProperty Is24HoursProperty = DependencyProperty.Register("Is24Hours",
            typeof(bool),
            typeof(Clock),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnIs24HoursChanged));

        private static void OnIs24HoursChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Clock)sender;

            instance.UpdateSelectedTimeString();
            instance.GenerateHoursListBoxItemsSource();
            instance.SyncListBoxesWithValue();
            instance.SyncClockButtonsWithValue();
        }

        public bool Is24Hours
        {
            get { return (bool)GetValue(Is24HoursProperty); }
            set { SetValue(Is24HoursProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region HeaderVisibility DependencyProperty
        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register("HeaderVisibility",
            typeof(Visibility),
            typeof(Clock),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public Visibility HeaderVisibility
        {
            get { return (Visibility)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region HeaderForeground DependencyProperty
        public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register("HeaderForeground",
            typeof(Brush),
            typeof(Clock),
            new PropertyMetadata(null));

        public Brush HeaderForeground
        {
            get { return (Brush)GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }
        #endregion

        #region HeaderBackground DependencyProperty
        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register("HeaderBackground",
            typeof(Brush),
            typeof(Clock),
            new PropertyMetadata(null));

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }
        #endregion

        #region HeaderPadding DependencyProperty
        public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register("HeaderPadding",
            typeof(Thickness),
            typeof(Clock),
            new PropertyMetadata(default(Thickness)));

        public Thickness HeaderPadding
        {
            get { return (Thickness)GetValue(HeaderPaddingProperty); }
            set { SetValue(HeaderPaddingProperty, value); }
        }
        #endregion

        #region MajorClockButtonStyle DependencyProperty
        public static readonly DependencyProperty MajorClockButtonStyleProperty = DependencyProperty.Register("MajorClockButtonStyle",
            typeof(Style),
            typeof(Clock),
            new PropertyMetadata(null));

        public Style MajorClockButtonStyle
        {
            get { return (Style)GetValue(MajorClockButtonStyleProperty); }
            set { SetValue(MajorClockButtonStyleProperty, value); }
        }
        #endregion

        #region MinorClockButtonStyle DependencyProperty
        public static readonly DependencyProperty MinorClockButtonStyleProperty = DependencyProperty.Register("MinorClockButtonStyle",
            typeof(Style),
            typeof(Clock),
            new PropertyMetadata(null));

        public Style MinorClockButtonStyle
        {
            get { return (Style)GetValue(MinorClockButtonStyleProperty); }
            set { SetValue(MinorClockButtonStyleProperty, value); }
        }
        #endregion

        private RadioButton _hoursButton;
        private RadioButton _minutesButton;
        private RadioButton _secondsButton;
        private TextBlock _meridiemTextBlock;

        private RadioButton _amButton;
        private RadioButton _pmButton;

        private ListBox _hoursListBox;
        private ListBox _minutesListBox;
        private ListBox _secondsListBox;

        private RadialPanel _normalHoursPanel;
        private RadialPanel _extendedHoursPanel;
        private RadialPanel _minutesPanel;
        private RadialPanel _secondsPanel;

        private readonly List<ToggleButton> _generatedClockButtons = new List<ToggleButton>();

        private bool _syncingListBoxes;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_hoursButton != null)
            {
                _hoursButton.Click -= InputModeButton_Click;
            }

            if (_minutesButton != null)
            {
                _minutesButton.Click -= InputModeButton_Click;
            }

            if (_secondsButton != null)
            {
                _secondsButton.Click -= InputModeButton_Click;
            }

            if (_amButton != null)
            {
                _amButton.Click -= MeridiemButton_Click;
            }

            if (_pmButton != null)
            {
                _pmButton.Click -= MeridiemButton_Click;
            }

            if (_hoursListBox != null)
            {
                _hoursListBox.SelectionChanged -= ListBox_SelectionChanged;
            }

            if (_minutesListBox != null)
            {
                _minutesListBox.SelectionChanged -= ListBox_SelectionChanged;
            }

            if (_secondsListBox != null)
            {
                _secondsListBox.SelectionChanged -= ListBox_SelectionChanged;
            }

            if (_normalHoursPanel != null)
            {
                _normalHoursPanel.Children.Clear();
            }

            if (_extendedHoursPanel != null)
            {
                _extendedHoursPanel.Children.Clear();
            }

            if (_minutesPanel != null)
            {
                _minutesPanel.Children.Clear();
            }

            if (_secondsPanel != null)
            {
                _secondsPanel.Children.Clear();
            }

            CleanUpClockButtons();

            _hoursButton = GetTemplateChild("PART_HoursButton") as RadioButton;
            _minutesButton = GetTemplateChild("PART_MinutesButton") as RadioButton;
            _secondsButton = GetTemplateChild("PART_SecondsButton") as RadioButton;
            _meridiemTextBlock = GetTemplateChild("PART_MeridiemTextBlock") as TextBlock;

            _amButton = GetTemplateChild("PART_AMButton") as RadioButton;
            _pmButton = GetTemplateChild("PART_PMButton") as RadioButton;

            _hoursListBox = GetTemplateChild("PART_HoursListBox") as ListBox;
            _minutesListBox = GetTemplateChild("PART_MinutesListBox") as ListBox;
            _secondsListBox = GetTemplateChild("PART_SecondsListBox") as ListBox;

            _normalHoursPanel = GetTemplateChild("PART_NormalHoursPanel") as RadialPanel;
            _extendedHoursPanel = GetTemplateChild("PART_ExtendedHoursPanel") as RadialPanel;
            _minutesPanel = GetTemplateChild("PART_MinutesPanel") as RadialPanel;
            _secondsPanel = GetTemplateChild("PART_SecondsPanel") as RadialPanel;

            if (_hoursButton != null)
            {
                _hoursButton.Click += InputModeButton_Click;
            }

            if (_minutesButton != null)
            {
                _minutesButton.Click += InputModeButton_Click;
            }

            if (_secondsButton != null)
            {
                _secondsButton.Click += InputModeButton_Click;
            }

            if (_amButton != null)
            {
                _amButton.Click += MeridiemButton_Click;
            }

            if (_pmButton != null)
            {
                _pmButton.Click += MeridiemButton_Click;
            }

            if (_hoursListBox != null)
            {
                _hoursListBox.SelectionChanged += ListBox_SelectionChanged;
                GenerateHoursListBoxItemsSource();
            }

            if (_minutesListBox != null)
            {
                _minutesListBox.SelectionChanged += ListBox_SelectionChanged;
                _minutesListBox.ItemsSource = GenerateClockValues(0, 59);
            }

            if (_secondsListBox != null)
            {
                _secondsListBox.SelectionChanged += ListBox_SelectionChanged;
                _secondsListBox.ItemsSource = GenerateClockValues(0, 59);
            }

            GenerateClockButtons();
            UpdateSelectedTimeString();
            SyncListBoxesWithValue();
            SyncClockButtonsWithValue();
            UpdateInputModeButtons();
            UpdateMeridiemButtons();
        }

        private void InputModeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_hoursButton != null && _hoursButton.IsChecked == true) ActiveInputMode = ClockInputMode.Hours;
            else if (_minutesButton != null && _minutesButton.IsChecked == true) ActiveInputMode = ClockInputMode.Minutes;
            else if (_secondsButton != null && _secondsButton.IsChecked == true) ActiveInputMode = ClockInputMode.Seconds;
        }

        private void UpdateInputModeButtons()
        {
            switch (ActiveInputMode)
            {
                case ClockInputMode.Hours:
                {
                    if (_hoursButton != null) _hoursButton.IsChecked = true;
                    break;
                }
                case ClockInputMode.Minutes:
                {
                    if (_minutesButton != null) _minutesButton.IsChecked = true;
                    break;
                }
                case ClockInputMode.Seconds:
                {
                    if (_secondsButton != null) _secondsButton.IsChecked = true;
                    break;
                }
            }
        }

        private void MeridiemButton_Click(object sender, RoutedEventArgs e)
        {
            if (_amButton == null || _pmButton == null || SelectedTime == null) return;

            var hour = SelectedTime.Value.Hour;
            var minute = SelectedTime.Value.Minute;
            var second = SelectedTime.Value.Second;

            if (_pmButton.IsChecked == true && hour < 12)
            {
                hour += 12;

                var time = new TimeSpan(hour, minute, second);
                var dateTime = DateTime.Now.Date.Add(time);

                SelectedTime = dateTime;
            }
            else if (_amButton.IsChecked == true && hour > 12)
            {
                hour -= 12;

                var time = new TimeSpan(hour, minute, second);
                var dateTime = DateTime.Now.Date.Add(time);

                SelectedTime = dateTime;
            }
        }

        private void UpdateMeridiemButtons()
        {
            var hour = SelectedTime.GetValueOrDefault().Hour;

            if (hour > 11)
            {
                if (_pmButton != null) _pmButton.IsChecked = true;
            }
            else
            {
                if (_amButton != null) _amButton.IsChecked = true;
            }
        }

        private void RaiseSelectedTimeChanged(DateTime? oldValue, DateTime? newValue)
        {
            var e = new RoutedPropertyChangedEventArgs<DateTime?>(oldValue, newValue, SelectedTimeChangedEvent);

            RaiseEvent(e);

            OnSelectedTimeChanged(oldValue, newValue);
        }

        protected virtual void OnSelectedTimeChanged(DateTime? oldValue, DateTime? newValue)
        {

        }

        protected virtual void UpdateSelectedTimeString()
        {
            var selectedValue = SelectedTime;

            string timeString;

            var culture = string.IsNullOrWhiteSpace(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture;

            if (selectedValue != null)
            {
                if (ShowSeconds)
                {
                    if (Is24Hours) timeString = selectedValue.Value.ToString("T");
                    else
                    {
                        timeString = selectedValue.Value.ToString("hh:mm:ss tt", culture);
                    }
                }
                else
                {
                    if (Is24Hours) timeString = selectedValue.Value.ToString("t");
                    else
                    {
                        timeString = selectedValue.Value.ToString("hh:mm tt", culture);
                    }
                }

                if (_hoursButton != null) _hoursButton.Content = selectedValue.Value.ToString(Is24Hours ? "HH" : "hh");
                if (_minutesButton != null) _minutesButton.Content = selectedValue.Value.ToString("mm");
                if (_secondsButton != null) _secondsButton.Content = selectedValue.Value.ToString("ss");
            }
            else
            {
                if (ShowSeconds) timeString = "--:--:--";
                else timeString = "--:--";

                if (_hoursButton != null) _hoursButton.Content = "--";
                if (_minutesButton != null) _minutesButton.Content = "--";
                if (_secondsButton != null) _secondsButton.Content = "--";
            }

            if (_meridiemTextBlock != null)
            {
                if (Is24Hours) _meridiemTextBlock.Text = "";
                else
                {
                    if (selectedValue != null) _meridiemTextBlock.Text = selectedValue.Value.ToString(" tt", culture);
                    else _meridiemTextBlock.Text = $" {culture.DateTimeFormat.AMDesignator}";
                }
            }

            SelectedTimeString = timeString;
        }

        private void SyncListBoxesWithValue()
        {
            var selectedValue = SelectedTime;

            _syncingListBoxes = true;

            if (selectedValue == null)
            {
                if (_hoursListBox != null) _hoursListBox.SelectedIndex = -1;
                if (_minutesListBox != null) _minutesListBox.SelectedIndex = -1;
                if (_secondsListBox != null) _secondsListBox.SelectedIndex = -1;
            }
            else
            {
                if (_hoursListBox != null)
                {
                    var hour = selectedValue.Value.Hour;

                    // Wenn keine 24 Stunden angezeigt werden dann müssen wir das auf den 12 Stunden Raum eingrenzen
                    if (!Is24Hours) hour %= 12;

                    _hoursListBox.SelectedIndex = hour;
                    _hoursListBox.ScrollIntoView(_hoursListBox.SelectedItem);
                }
                if (_minutesListBox != null)
                {
                    _minutesListBox.SelectedIndex = selectedValue.Value.Minute;
                    _minutesListBox.ScrollIntoView(_minutesListBox.SelectedItem);
                }
                if (_secondsListBox != null)
                {
                    _secondsListBox.SelectedIndex = selectedValue.Value.Second;
                    _secondsListBox.ScrollIntoView(_secondsListBox.SelectedItem);
                }
            }

            _syncingListBoxes = false;
        }

        private void SyncClockButtonsWithValue()
        {
            foreach (var checkedButton in _generatedClockButtons.Where(x => x.IsChecked == true))
            {
                checkedButton.IsChecked = false;
            }

            if (SelectedTime == null) return;

            var hour = SelectedTime.Value.Hour;
            var minute = SelectedTime.Value.Minute;
            var second = SelectedTime.Value.Second;

            // Wenn keine 24 Stunden angezeigt werden dann müssen wir das auf den 12 Stunden Raum eingrenzen
            if (!Is24Hours) hour %= 12;

            foreach (var button in _generatedClockButtons.Where(x => (ClockInputMode)x.Tag == ClockInputMode.Hours))
            {
                var value = (int)button.Content;

                // Entspricht der Wert des Buttons der aktuellen Stunde?
                // Bei nur 12 Stunden Anzeige ist die Stunde 0 als Button 12 zu interpretieren
                if ((Is24Hours && value == hour) || (!Is24Hours && (value == hour || (hour == 0 && value == 12))))
                {
                    button.IsChecked = true;
                    break;
                }
            }

            foreach (var button in _generatedClockButtons.Where(x => (ClockInputMode)x.Tag == ClockInputMode.Minutes))
            {
                var value = (int)button.Content;

                if (value == minute)
                {
                    button.IsChecked = true;
                    break;
                }
            }

            foreach (var button in _generatedClockButtons.Where(x => (ClockInputMode)x.Tag == ClockInputMode.Seconds))
            {
                var value = (int)button.Content;

                if (value == second)
                {
                    button.IsChecked = true;
                    break;
                }
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_syncingListBoxes) return;

            var hourItem = _hoursListBox?.SelectedItem as ClockValue?;
            var minuteItem = _minutesListBox?.SelectedItem as ClockValue?;
            var secondItem = _secondsListBox?.SelectedItem as ClockValue?;

            if (hourItem == null && minuteItem == null && secondItem == null)
            {
                SelectedTime = null;
            }
            else
            {
                var hour = hourItem?.Value ?? 0;
                var minute = minuteItem?.Value ?? 0;
                var second = secondItem?.Value ?? 0;

                // Wenn keine 24 Stunden angezeigt werden dann ist der Wert 12 eigentlich der Wert 0
                if (!Is24Hours)
                {
                    hour %= 12;

                    if (_pmButton?.IsChecked == true) hour += 12;
                }

                hour = Math.Min(Math.Max(0, hour), 23);
                minute = Math.Min(Math.Max(0, minute), 59);
                second = Math.Min(Math.Max(0, second), 59);

                var time = new TimeSpan(hour, minute, second);
                var dateTime = DateTime.Now.Date.Add(time);

                SelectedTime = dateTime;
            }
        }

        private void CleanUpClockButtons()
        {
            for (int i = 0; i < _generatedClockButtons.Count; i++)
            {
                var button = _generatedClockButtons[i];

                BindingOperations.ClearAllBindings(button);

                button.Click -= ClockButton_Click;
            }

            _generatedClockButtons.Clear();
        }

        private void GenerateClockButtons()
        {
            if (_normalHoursPanel != null)
            {
                for (int i = 1; i <= 12; i++)
                {
                    var button = new ClockButton()
                    {
                        Content = i,
                        Tag = ClockInputMode.Hours
                    };

                    button.SetBinding(StyleProperty, new Binding(nameof(MajorClockButtonStyle)) { Source = this });

                    button.Click += ClockButton_Click;

                    if (i == 12) _normalHoursPanel.Children.Insert(0, button);
                    else _normalHoursPanel.Children.Add(button);

                    _generatedClockButtons.Add(button);
                }
            }

            if (_extendedHoursPanel != null)
            {
                for (int i = 13; i <= 24; i++)
                {
                    var button = new ClockButton()
                    {
                        Content = i == 24 ? 0 : i,
                        Tag = ClockInputMode.Hours
                    };

                    button.SetBinding(StyleProperty, new Binding(nameof(MajorClockButtonStyle)) { Source = this });

                    button.Click += ClockButton_Click;

                    if (i == 24) _extendedHoursPanel.Children.Insert(0, button);
                    else _extendedHoursPanel.Children.Add(button);

                    _generatedClockButtons.Add(button);
                }
            }

            if (_minutesPanel != null)
            {
                for (int i = 0; i <= 59; i++)
                {
                    var button = new ClockButton()
                    {
                        Content = i,
                        Tag = ClockInputMode.Minutes
                    };

                    var styleName = i % 5 == 0 ? nameof(MajorClockButtonStyle) : nameof(MinorClockButtonStyle);

                    button.SetBinding(StyleProperty, new Binding(styleName) { Source = this });

                    button.Click += ClockButton_Click;

                    _minutesPanel.Children.Add(button);

                    _generatedClockButtons.Add(button);
                }
            }

            if (_secondsPanel != null)
            {
                for (int i = 0; i <= 59; i++)
                {
                    var button = new ClockButton()
                    {
                        Content = i,
                        Tag = ClockInputMode.Seconds
                    };

                    var styleName = i % 5 == 0 ? nameof(MajorClockButtonStyle) : nameof(MinorClockButtonStyle);

                    button.SetBinding(StyleProperty, new Binding(styleName) { Source = this });

                    button.Click += ClockButton_Click;

                    _secondsPanel.Children.Add(button);

                    _generatedClockButtons.Add(button);
                }
            }
        }

        private void ClockButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (ToggleButton)sender;

            var previousSelectedButton = _generatedClockButtons.FirstOrDefault(x => x != button && (ClockInputMode)x.Tag == (ClockInputMode)button.Tag && x.IsChecked == true);

            if (previousSelectedButton != null) previousSelectedButton.IsChecked = false;

            var hourButton = _generatedClockButtons.FirstOrDefault(x => (ClockInputMode)x.Tag == ClockInputMode.Hours && x.IsChecked == true);
            var minuteButton = _generatedClockButtons.FirstOrDefault(x => (ClockInputMode)x.Tag == ClockInputMode.Minutes && x.IsChecked == true);
            var secondButton = _generatedClockButtons.FirstOrDefault(x => (ClockInputMode)x.Tag == ClockInputMode.Seconds && x.IsChecked == true);

            var hour = hourButton?.Content as int? ?? 0;
            var minute = minuteButton?.Content as int? ?? 0;
            var second = secondButton?.Content as int? ?? 0;

            if (!Is24Hours && _pmButton != null && _pmButton.IsChecked == true) hour += 12;

            var time = new TimeSpan(hour, minute, second);
            var dateTime = DateTime.Now.Date.Add(time);

            SelectedTime = dateTime;
        }

        private void GenerateHoursListBoxItemsSource()
        {
            if (_hoursListBox == null) return;

            if (Is24Hours) _hoursListBox.ItemsSource = GenerateClockValues(0, 23);
            else
            {
                var items = GenerateClockValues(0, 11);

                items.RemoveAt(0);
                items.Insert(0, new ClockValue(12));

                _hoursListBox.ItemsSource = items;
            }
        }

        private static List<ClockValue> GenerateClockValues(int start, int end)
        {
            var list = new List<ClockValue>(end - start);

            for (var i = start; i <= end; i++)
            {
                list.Add(new ClockValue(i));
            }

            return list;
        }
    }
}