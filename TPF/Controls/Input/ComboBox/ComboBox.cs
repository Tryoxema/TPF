using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TPF.Internal;

namespace TPF.Controls
{
    [TemplatePart(Name = "PART_DropDownButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_EditableTextBox", Type = typeof(TextBox))]
    public class ComboBox : MultiSelector
    {
        static ComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(typeof(ComboBox)));

            EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(OnPreviewMouseButtonDown));
            EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.MouseDownEvent, new MouseButtonEventHandler(OnMouseButtonDown), true);
        }

        #region IsEditable DependencyProperty
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable",
            typeof(bool),
            typeof(ComboBox),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsEditableChanged));

        private static void OnIsEditableChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ComboBox)sender;

            instance.UpdateDisplayedSelectionText();
        }

        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region EditableTemplate DependencyProperty
        public static readonly DependencyProperty EditableTemplateProperty = DependencyProperty.Register("EditableTemplate",
            typeof(ControlTemplate),
            typeof(ComboBox),
            new PropertyMetadata(null));

        public ControlTemplate EditableTemplate
        {
            get { return (ControlTemplate)GetValue(EditableTemplateProperty); }
            set { SetValue(EditableTemplateProperty, value); }
        }
        #endregion

        #region AllowMultiSelection DependencyProperty
        public static readonly DependencyProperty AllowMultiSelectionProperty = DependencyProperty.Register("AllowMultiSelection",
            typeof(bool),
            typeof(ComboBox),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnAllowMultiSelectionChanged));

        private static void OnAllowMultiSelectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ComboBox)sender;

            instance.CanSelectMultipleItems = (bool)e.NewValue;
        }

        public bool AllowMultiSelection
        {
            get { return (bool)GetValue(AllowMultiSelectionProperty); }
            set { SetValue(AllowMultiSelectionProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region SelectionBoxItem ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey SelectionBoxItemPropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItem",
            typeof(object),
            typeof(ComboBox),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectionBoxItemProperty = SelectionBoxItemPropertyKey.DependencyProperty;

        public object SelectionBoxItem
        {
            get { return GetValue(SelectionBoxItemProperty); }
            private set { SetValue(SelectionBoxItemPropertyKey, value); }
        }
        #endregion

        #region SelectionBoxItemTemplate ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey SelectionBoxItemTemplatePropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItemTemplate",
            typeof(DataTemplate),
            typeof(ComboBox),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectionBoxItemTemplateProperty = SelectionBoxItemTemplatePropertyKey.DependencyProperty;

        public DataTemplate SelectionBoxItemTemplate
        {
            get { return (DataTemplate)GetValue(SelectionBoxItemTemplateProperty); }
            private set { SetValue(SelectionBoxItemTemplatePropertyKey, value); }
        }
        #endregion

        #region SelectionBoxTemplate DependencyProperty
        public static readonly DependencyProperty SelectionBoxTemplateProperty = DependencyProperty.Register("SelectionBoxTemplate",
            typeof(DataTemplate),
            typeof(ComboBox),
            new PropertyMetadata(null, OnSelectionBoxPropertiesChanged));

        public DataTemplate SelectionBoxTemplate
        {
            get { return (DataTemplate)GetValue(SelectionBoxTemplateProperty); }
            set { SetValue(SelectionBoxTemplateProperty, value); }
        }
        #endregion

        #region MultiSelectionBoxTemplate DependencyProperty
        public static readonly DependencyProperty MultiSelectionBoxTemplateProperty = DependencyProperty.Register("MultiSelectionBoxTemplate",
            typeof(DataTemplate),
            typeof(ComboBox),
            new PropertyMetadata(null, OnSelectionBoxPropertiesChanged));

        public DataTemplate MultiSelectionBoxTemplate
        {
            get { return (DataTemplate)GetValue(MultiSelectionBoxTemplateProperty); }
            set { SetValue(MultiSelectionBoxTemplateProperty, value); }
        }
        #endregion

        #region EmptySelectionBoxTemplate DependencyProperty
        public static readonly DependencyProperty EmptySelectionBoxTemplateProperty = DependencyProperty.Register("EmptySelectionBoxTemplate",
            typeof(DataTemplate),
            typeof(ComboBox),
            new PropertyMetadata(null, OnSelectionBoxPropertiesChanged));

        public DataTemplate EmptySelectionBoxTemplate
        {
            get { return (DataTemplate)GetValue(EmptySelectionBoxTemplateProperty); }
            set { SetValue(EmptySelectionBoxTemplateProperty, value); }
        }
        #endregion

        #region MultiSelectionSeparator DependencyProperty
        public static readonly DependencyProperty MultiSelectionSeparatorProperty = DependencyProperty.Register("MultiSelectionSeparator",
            typeof(char),
            typeof(ComboBox),
            new PropertyMetadata(',', OnSelectionBoxPropertiesChanged));

        public char MultiSelectionSeparator
        {
            get { return (char)GetValue(MultiSelectionSeparatorProperty); }
            set { SetValue(MultiSelectionSeparatorProperty, value); }
        }
        #endregion

        #region MultiSelectionSeparatorStringFormat DependencyProperty
        public static readonly DependencyProperty MultiSelectionSeparatorStringFormatProperty = DependencyProperty.Register("MultiSelectionSeparatorStringFormat",
            typeof(string),
            typeof(ComboBox),
            new PropertyMetadata("{0} ", OnSelectionBoxPropertiesChanged));

        public string MultiSelectionSeparatorStringFormat
        {
            get { return (string)GetValue(MultiSelectionSeparatorStringFormatProperty); }
            set { SetValue(MultiSelectionSeparatorStringFormatProperty, value); }
        }
        #endregion

        #region EmptyText DependencyProperty
        public static readonly DependencyProperty EmptyTextProperty = DependencyProperty.Register("EmptyText",
            typeof(string),
            typeof(ComboBox),
            new PropertyMetadata(null, OnSelectionBoxPropertiesChanged));

        public string EmptyText
        {
            get { return (string)GetValue(EmptyTextProperty); }
            set { SetValue(EmptyTextProperty, value); }
        }
        #endregion

        #region Watermark DependencyProperty
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark",
            typeof(string),
            typeof(ComboBox),
            new PropertyMetadata(null));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        #endregion

        #region Text DependencyProperty
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(ComboBox),
            new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region TextSearchMode DependencyProperty
        public static readonly DependencyProperty TextSearchModeProperty = DependencyProperty.Register("TextSearchMode",
            typeof(TextSearchMode),
            typeof(ComboBox),
            new PropertyMetadata(TextSearchMode.StartsWith));

        public TextSearchMode TextSearchMode
        {
            get { return (TextSearchMode)GetValue(TextSearchModeProperty); }
            set { SetValue(TextSearchModeProperty, value); }
        }
        #endregion

        #region TextBoxStyle DependencyProperty
        public static readonly DependencyProperty TextBoxStyleProperty = DependencyProperty.Register("TextBoxStyle",
            typeof(Style),
            typeof(ComboBox),
            new PropertyMetadata(null));

        public Style TextBoxStyle
        {
            get { return (Style)GetValue(TextBoxStyleProperty); }
            set { SetValue(TextBoxStyleProperty, value); }
        }
        #endregion

        #region IsReadOnly DependencyProperty
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly",
            typeof(bool),
            typeof(ComboBox),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsDropDownOpen DependencyProperty
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen",
            typeof(bool),
            typeof(ComboBox),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged));

        private static void OnIsDropDownOpenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ComboBox)sender;

            var open = (bool)e.NewValue;

            if (open)
            {
                Mouse.Capture(instance, CaptureMode.SubTree);

                instance.OnDropDownOpened(EventArgs.Empty);
            }
            else
            {
                if (Mouse.Captured == instance) Mouse.Capture(null);

                instance.OnDropDownClosed(EventArgs.Empty);
            }
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region MaxDropDownHeight DependencyProperty
        public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register("MaxDropDownHeight",
            typeof(double),
            typeof(ComboBox),
            new PropertyMetadata(200.0));

        public double MaxDropDownHeight
        {
            get { return (double)GetValue(MaxDropDownHeightProperty); }
            set { SetValue(MaxDropDownHeightProperty, value); }
        }
        #endregion

        #region ClearSelectionButtonVisibility DependencyProperty
        public static readonly DependencyProperty ClearSelectionButtonVisibilityProperty = DependencyProperty.Register("ClearSelectionButtonVisibility",
            typeof(Visibility),
            typeof(ComboBox),
            new PropertyMetadata(VisibilityBoxes.CollapsedBox));

        public Visibility ClearSelectionButtonVisibility
        {
            get { return (Visibility)GetValue(ClearSelectionButtonVisibilityProperty); }
            set { SetValue(ClearSelectionButtonVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region ClearSelectionButtonContent DependencyProperty
        public static readonly DependencyProperty ClearSelectionButtonContentProperty = DependencyProperty.Register("ClearSelectionButtonContent",
            typeof(object),
            typeof(ComboBox),
            new PropertyMetadata(null));

        public object ClearSelectionButtonContent
        {
            get { return GetValue(ClearSelectionButtonContentProperty); }
            set { SetValue(ClearSelectionButtonContentProperty, value); }
        }
        #endregion

        private static void OnSelectionBoxPropertiesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ComboBox)sender;

            instance.UpdateDisplayedSelectionText();
        }

        private static void OnPreviewMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var instance = (ComboBox)sender;

            if (!instance.IsEditable) return;

            if (e.OriginalSource is Visual visual && instance.EditableTextBox != null && instance.EditableTextBox.IsAncestorOf(visual))
            {
                if (instance.IsDropDownOpen) instance.IsDropDownOpen = false;
            }
        }

        private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var instance = (ComboBox)sender;

            e.Handled = true;

            if (Mouse.Captured == instance && e.OriginalSource == instance)
            {
                instance.IsDropDownOpen = false;
            }
        }

        public event EventHandler DropDownOpened;
        public event EventHandler DropDownClosed;

        internal ToggleButton DropDownButton;
        internal Button ClearButton;
        internal TextBox EditableTextBox;

        private UIElement _clonedElement;
        private bool _supressTextChanged;
        private bool _supressUpdateText;
        private string _internalSearchString;

        ComboBoxItem _highlightedItem;
        private ComboBoxItem HighlightedItem
        {
            get { return _highlightedItem; }
            set
            {
                if (_highlightedItem == value) return;
                if (_highlightedItem != null) _highlightedItem.SetIsHighlighted(false);
                _highlightedItem = value;
                if (_highlightedItem != null) _highlightedItem.SetIsHighlighted(true);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ClearButton != null)
            {
                ClearButton.Click -= ClearButton_Click;
            }

            if (EditableTextBox != null)
            {
                EditableTextBox.PreviewTextInput -= EditableTextBox_PreviewTextInput;
                EditableTextBox.TextChanged -= EditableTextBox_TextChanged;
            }

            DropDownButton = GetTemplateChild("PART_DropDownButton") as ToggleButton;
            ClearButton = GetTemplateChild("PART_ClearButton") as Button;
            EditableTextBox = GetTemplateChild("PART_EditableTextBox") as TextBox;

            if (ClearButton != null)
            {
                ClearButton.Click += ClearButton_Click;
            }

            if (EditableTextBox != null)
            {
                EditableTextBox.PreviewTextInput += EditableTextBox_PreviewTextInput;
                EditableTextBox.TextChanged += EditableTextBox_TextChanged;
            }

            UpdateDisplayedSelectionText();
        }

        // Der Button zum löschen der Auswahl
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            UnselectAll();

            IsDropDownOpen = false;
        }

        // Input der TextBox vorverarbeiten für Multi-Select usw.
        private void EditableTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Ist Mehrfachauswahl erlaubt und der Separator wurde angegeben?
            if (AllowMultiSelection && e.Text.Length == 1 && e.Text.First() == MultiSelectionSeparator)
            {
                // Sind wir beim letzten Item oder mitten drin?
                if (EditableTextBox.CaretIndex + EditableTextBox.SelectedText.Length == EditableTextBox.Text.Length)
                {
                    // Caret an letzte Stelle setzen, damit der Separator dort eingefügt wird
                    EditableTextBox.CaretIndex += EditableTextBox.SelectedText.Length;
                }
                // SelectedItems Updaten
                CommitSelectionFromText(EditableTextBox.Text);
            }
        }

        // Input der TextBox verarbeiten
        private void EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_supressTextChanged) return;

            if (string.IsNullOrEmpty(EditableTextBox.Text)) Text = EditableTextBox.Text;

            // Bei Contains-Suche wirt kein AutoComplete angewendet
            if (TextSearchMode == TextSearchMode.Contains || TextSearchMode == TextSearchMode.ContainsCaseSensitive)
            {
                CommitSelectionFromText(EditableTextBox.Text);
                return;
            }

            var change = e.Changes.FirstOrDefault();

            // Ich glaube change kann gar nicht null sein aber sicher ist sicher
            if (change == null || change.AddedLength == 0)
            {
                CommitSelectionFromText(EditableTextBox.Text);
                return;
            }

            if (AllowMultiSelection)
            {
                // Index des Letzten Separators suchen
                var lastSeparatorIndex = EditableTextBox.Text.LastIndexOf(MultiSelectionSeparator);
                // Wenn der momentane Punkt an dem geschrieben wird hinter dem letzten Separator liegt, wollen wir die Suche starten
                if (change.Offset > lastSeparatorIndex)
                {
                    // Den eingegebenen Text hinter dem letzten Separator extrahieren
                    var searchText = lastSeparatorIndex > -1 ? EditableTextBox.Text.Substring(lastSeparatorIndex + 1) : EditableTextBox.Text;
                    // Erstes Ergebnis suchen
                    var result = GetFirstSearchResult(searchText);

                    _supressTextChanged = true;

                    if (result != null)
                    {
                        // Die Differenz in Länge zwischen dem Ergebnis und dem bis jetzt eingetippten Text ermitteln
                        var difference = result.Length - searchText.Length;

                        // Suchergebnis am Ende einfügen und vorgeschlagenen Wert markieren, um den Schreibefluss nicht zu stören
                        EditableTextBox.Text = EditableTextBox.Text.Substring(0, lastSeparatorIndex + 1) + result;
                        EditableTextBox.Select(change.Offset + 1, difference);
                        Text = EditableTextBox.Text;
                        // SelectedItems Updaten
                        CommitSelectionFromText(Text);
                    }

                    _supressTextChanged = false;
                }
            }
            else
            {
                // Erstes Ergebnis suchen
                var result = GetFirstSearchResult(EditableTextBox.Text);

                _supressTextChanged = true;

                if (result == null)
                {
                    SelectedItem = null;
                    _supressTextChanged = false;
                    return;
                }

                // Die Differenz in Länge zwischen dem Ergebnis und dem bis jetzt eingetippten Text ermitteln
                var difference = result.Length - EditableTextBox.Text.Length;

                // Suchergebnis am Ende einfügen und vorgeschlagenen Wert markieren, um den Schreibefluss nicht zu stören
                EditableTextBox.Text = result;
                EditableTextBox.Select(change.Offset + 1, difference);
                Text = result;
                // SelectedItems Updaten
                CommitSelectionFromText(Text);

                _supressTextChanged = false;
            }
        }

        // SelectedItems anhand des Textes Selecten
        private void CommitSelectionFromText(string text)
        {
            if (Items.Count == 0 || string.IsNullOrEmpty(text))
            {
                SelectedItem = null;
                return;
            }

            // Pfad der Property suchen
            var propertyPath = GetSearchPropertyPath();

            if (AllowMultiSelection)
            {
                var parts = text.Split(MultiSelectionSeparator);

                BeginUpdateSelectedItems();

                var selectedItems = SelectedItems;
                // Zuerst SelectedItems leeren
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    var selectedItem = selectedItems[i];

                    SelectedItems.Remove(selectedItem);
                }
                // Dann alle Teile durchgehen
                for (int i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];

                    var item = GetFirstContainerFromText(part, false);
                    // Wenn ein Container mit einem passenden Inhalt gefunden wurde, dann selecten
                    if (item != null)
                    {
                        item.IsSelected = true;
                        if (ItemsSource == null) SelectedItems.Add(item);
                        else SelectedItems.Add(item.Content);
                    }
                }

                _supressUpdateText = true;
                EndUpdateSelectedItems();
                _supressUpdateText = false;
            }
            else
            {
                // Container mit einem passenden Inhalt suchen
                var item = GetFirstContainerFromText(text, false);

                BeginUpdateSelectedItems();

                var selectedItems = SelectedItems;
                // Zuerst SelectedItems leeren
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    var selectedItem = selectedItems[i];

                    SelectedItems.Remove(selectedItem);
                }

                if (item != null)
                {
                    item.IsSelected = true;
                    if (ItemsSource == null) SelectedItems.Add(item);
                    else SelectedItems.Add(item.Content);
                }

                EndUpdateSelectedItems();
            }

            Text = text;
        }

        // Das Item das momentan IsHighlighted auf true gesetzt hat selecten
        private void CommitSelectionFromHighlight()
        {
            _internalSearchString = null;
            Select(HighlightedItem);
        }

        // KeyDown-Event der TextBox handlen
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            // Wenn das DropDownMenü auf ist frisst der ToggleButton die Enter-Taste. Deshalb falls das passieren würde, kommen wir ihm zuvor
            if (e.OriginalSource == DropDownButton && e.Key == Key.Enter && DropDownButton.IsChecked == true) KeyDownHandler(e);
            // Wenn die TextBox den Focus hat kommen wir nur so an die Tasten
            if (IsEditable && e.OriginalSource == EditableTextBox) KeyDownHandler(e);
        }

        // KeyDown-Event des Controls handlen
        protected override void OnKeyDown(KeyEventArgs e)
        {
            KeyDownHandler(e);
        }

        // Der KeyDown-Handler
        private void KeyDownHandler(KeyEventArgs e)
        {
            if (!IsEnabled) return;

            switch (e.Key)
            {
                case Key.Up:
                {
                    if (IsDropDownOpen)
                    {
                        HighlightPrevious();
                        e.Handled = true;
                    }
                    else
                    {
                        SelectPrevious();
                        e.Handled = true;
                    }
                    break;
                }
                case Key.Down:
                {
                    if (IsDropDownOpen)
                    {
                        HighlightNext();
                        e.Handled = true;
                    }
                    else
                    {
                        SelectNext();
                        e.Handled = true;
                    }
                    break;
                }
                case Key.Enter:
                {
                    if (IsDropDownOpen)
                    {
                        CommitSelectionFromHighlight();
                        e.Handled = true;
                    }
                    else
                    {
                        IsDropDownOpen = true;
                        e.Handled = true;
                    }
                    break;
                }
                case Key.Escape:
                {
                    _internalSearchString = null;
                    if (IsDropDownOpen)
                    {
                        IsDropDownOpen = false;
                        e.Handled = true;
                    }
                    break;
                }
                case Key.Delete:
                {
                    _internalSearchString = null;
                    break;
                }
                case Key.Back:
                {
                    if (_internalSearchString != null)
                    {
                        if (_internalSearchString == string.Empty || _internalSearchString.Length == 1) _internalSearchString = null;
                        else _internalSearchString = _internalSearchString.Remove(_internalSearchString.Length - 1);
                    }
                    break;
                }
            }
        }

        // TextInput des Controls im IsEditable = false Modus für Suchvorschläge
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);

            // Wenn die ComboBox im Multi-Modus ist und der Separator gedrückt wurde, dann String resetten
            if (AllowMultiSelection && e.Text == MultiSelectionSeparator.ToString())
            {
                _internalSearchString = null;
                return;
            }

            if (_internalSearchString == null) _internalSearchString = e.Text;
            else _internalSearchString += e.Text;

            // Ist das Popup offen?
            if (IsDropDownOpen)
            {
                // Ersten Container suchen
                var container = GetFirstContainerFromText(_internalSearchString, false);
                // Wurde einer gefunden?
                if (container == null)
                {
                    // Suche resetten und neu versuchen
                    _internalSearchString = e.Text;
                    container = GetFirstContainerFromText(_internalSearchString, false);
                    // Wenn immer noch kein Ergebnis gefunden wurde, dann abbrechen
                    if (container == null)
                    {
                        _internalSearchString = null;
                        return;
                    }
                }
                // Container Highlighten
                Highlight(container);
            }
            else if (!IsEditable)
            {
                // Erstes Item suchen
                var item = GetFirstItemFromText(_internalSearchString, true);
                // Wurde eine Item gefunden?
                if (item == null)
                {
                    // Suche resetten und neu versuchen
                    _internalSearchString = e.Text;
                    item = GetFirstItemFromText(_internalSearchString, true);
                    // Wenn immer noch kein Ergebnis gefunden wurde, dann abbrechen
                    if (item == null)
                    {
                        _internalSearchString = null;
                        return;
                    }
                }
                // Im Multi-Modus Item zu SelectedItems hinzufügen und im normalen Modus Item selecten
                if (AllowMultiSelection) SelectedItems.Add(item);
                else SelectedItem = item;
            }
        }

        // String im IsEditable-Modus formatieren
        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            // Ist die ComboBox im Edit-Modus?
            if (IsEditable)
            {
                if ((bool)e.NewValue)
                {
                    // String in Edit-Modus formatieren
                    UpdateText(false);
                }
                else
                {
                    // SelectedItems aktualisieren
                    CommitSelectionFromText(Text);
                    // String in Anzeige-Modus formatieren
                    UpdateText(true);
                }
            }
            base.OnIsKeyboardFocusWithinChanged(e);
        }

        // Pfad der Property die für die Suche verwendet werden soll bestimmen
        private string GetSearchPropertyPath()
        {
            // Wenn die TextPath-Property von TextSearch gesetzt wurde, diese nehmen
            var propertyPath = TextSearch.GetTextPath(this);
            // Sonst DisplayMemberPath nehmen
            if (string.IsNullOrWhiteSpace(propertyPath)) propertyPath = DisplayMemberPath;
            // Wenn das auch nicht vorhanden ist, dann haben wir keinen Referenzpunkt
            if (string.IsNullOrWhiteSpace(propertyPath)) propertyPath = null;

            return propertyPath;
        }

        // Die Suchlogik, um ein Item auf einen Text zu durchsuchen
        private bool SearchItem(object item, string propertyPath, string text)
        {
            var content = item;

            if (item is ContentControl contentControl) content = contentControl.Content;

            // Wert des Items zum vergleichen raussuchen
            var value = propertyPath != null ? PropertyHelper.GetPropertyValueFromPath(content, propertyPath)?.ToString() : content?.ToString();
            // Wenn kein Wert gefunden wurde, wird dieses Item ignoriert
            if (value == null) return false;

            var match = false;

            switch (TextSearchMode)
            {
                case TextSearchMode.StartsWith:
                {
                    if (value.StartsWith(text, StringComparison.InvariantCultureIgnoreCase)) match = true;
                    break;
                }
                case TextSearchMode.Contains:
                {
                    var regex = new Regex($"({text})", RegexOptions.IgnoreCase);

                    if (regex.Match(value).Success) match = true;
                    break;
                }
                case TextSearchMode.StartsWithCaseSensitive:
                {
                    if (value.StartsWith(text)) match = true;
                    break;
                }
                case TextSearchMode.ContainsCaseSensitive:
                {
                    if (value.Contains(text)) match = true;
                    break;
                }
            }

            return match;
        }

        // Holt das erste Item für einen bestimmten Text
        private object GetFirstItemFromText(string text, bool skipSelected)
        {
            // Wenn wir keine Items haben oder der Text leer ist, brauchen wir gar nicht erst anfangen
            if (Items.Count == 0 || string.IsNullOrEmpty(text)) return null;

            // Pfad der Property suchen
            var propertyPath = GetSearchPropertyPath();

            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                if (skipSelected && SelectedItems.Contains(item)) continue;

                if (SearchItem(item, propertyPath, text)) return item;
            }

            return null;
        }

        // Holt den ersten Container für einen bestimmten Text
        private ComboBoxItem GetFirstContainerFromText(string text, bool skipSelected)
        {
            // Wenn wir keine Items haben oder der Text leer ist, brauchen wir gar nicht erst anfangen
            if (Items.Count == 0 || string.IsNullOrEmpty(text)) return null;

            // Pfad der Property suchen
            var propertyPath = GetSearchPropertyPath();

            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                if (skipSelected && SelectedItems.Contains(item)) continue;

                if (SearchItem(item, propertyPath, text))
                {
                    if (item is ComboBoxItem comboBoxItem) return comboBoxItem;
                    else return ItemContainerGenerator.ContainerFromItem(item) as ComboBoxItem;
                }
            }

            return null;
        }

        // Holt den ersten Text der für die Suche verwendeten Property, der den Suchkriterien und dem angegebenen Text entspricht
        private string GetFirstSearchResult(string text)
        {
            // Wenn wir keine Items haben oder der Text leer ist, brauchen wir gar nicht erst anfangen
            if (Items.Count == 0 || string.IsNullOrEmpty(text)) return null;

            // Pfad der Property suchen
            var propertyPath = GetSearchPropertyPath();

            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                if (item is ContentControl contentControl) item = contentControl.Content;

                // Wert des Items zum vergleichen raussuchen
                var value = propertyPath != null ? PropertyHelper.GetPropertyValueFromPath(item, propertyPath)?.ToString() : item?.ToString();
                // Wenn kein Wert gefunden wurde, wird dieses Item ignoriert
                if (value == null) continue;

                var match = false;

                switch (TextSearchMode)
                {
                    case TextSearchMode.StartsWith:
                    {
                        if (value.StartsWith(text, StringComparison.InvariantCultureIgnoreCase)) match = true;
                        break;
                    }
                    case TextSearchMode.Contains:
                    {
                        var regex = new Regex($"({text})", RegexOptions.IgnoreCase);

                        if (regex.Match(value).Success) match = true;
                        break;
                    }
                    case TextSearchMode.StartsWithCaseSensitive:
                    {
                        if (value.StartsWith(text)) match = true;
                        break;
                    }
                    case TextSearchMode.ContainsCaseSensitive:
                    {
                        if (value.Contains(text)) match = true;
                        break;
                    }
                }

                if (match && (Text == null || !Text.Contains(value))) return value;
            }

            return null;
        }

        // Aktualisiert die Anzeige der ausgewählten Items
        internal void UpdateDisplayedSelectionText()
        {
            if (IsEditable) UpdateText(false);
            else UpdateSelectionBoxItem();
        }

        // Aktualisiert die Anzeige der ausgewählten Items, wenn die ComboBox nicht im Edit-Modus ist
        private void UpdateSelectionBoxItem()
        {
            if (_clonedElement != null)
            {
                _clonedElement.LayoutUpdated -= ClonedElementLayoutUpdated;
                _clonedElement = null;
            }

            Text = null;

            // Wenn kein Item ausgewählt ist, dann EmptyText anzeigen
            if (SelectedItems.Count == 0)
            {
                SelectionBoxItem = EmptyText;
                SelectionBoxItemTemplate = EmptySelectionBoxTemplate ?? DataTemplates.StringTemplate;
            }
            // Wenn MultiSelection aktiv ist, dann String aus den zugehörigen Properties zusammenbauen
            else if (AllowMultiSelection)
            {
                var resultString = GetMultiSelectionResultString(true);

                SelectionBoxItem = resultString;
                SelectionBoxItemTemplate = DataTemplates.StringTemplate;
            }
            else
            {
                var item = SelectedItem;
                var itemTemplate = SelectionBoxTemplate;

                // Wenn das Item ein ContentControl ist, dann nehmen wir dessen Content
                if (item is ContentControl contentControl)
                {
                    item = contentControl.Content;
                    itemTemplate = contentControl.ContentTemplate;
                }

                if (itemTemplate == null && ItemTemplateSelector == null)
                {
                    // Ist das Item ein DependencyObject?
                    if (item is DependencyObject logicalElement)
                    {
                        // Wenn es ein UIElement ist, dann müssen wir einen Visuellen Klon des Elements erstellen, damit wir keine Probleme kriegen
                        if (logicalElement is UIElement uiElement)
                        {
                            _clonedElement = uiElement;

                            // Eine Kopie des Elements als VisualBrush erstellen
                            var visualBrush = new VisualBrush(_clonedElement)
                            {
                                Stretch = Stretch.None,
                                ViewboxUnits = BrushMappingMode.Absolute,
                                Viewbox = new Rect(_clonedElement.RenderSize),
                                ViewportUnits = BrushMappingMode.Absolute,
                                Viewport = new Rect(_clonedElement.RenderSize)
                            };

                            DependencyObject parent = VisualTreeHelper.GetParent(_clonedElement);
                            var parentFlowDirection = parent == null ? FlowDirection.LeftToRight : (FlowDirection)parent.GetValue(FlowDirectionProperty);
                            if (FlowDirection != parentFlowDirection)
                            {
                                visualBrush.Transform = new MatrixTransform(new Matrix(-1.0, 0.0, 0.0, 1.0, _clonedElement.RenderSize.Width, 0.0));
                            }

                            // VisualBrush des geklonten Elements auf ein Rectangle-Objekt anwenden
                            var rectangle = new Rectangle
                            {
                                Fill = visualBrush,
                                Width = _clonedElement.RenderSize.Width,
                                Height = _clonedElement.RenderSize.Height
                            };

                            _clonedElement.LayoutUpdated += ClonedElementLayoutUpdated;

                            item = rectangle;
                            itemTemplate = null;
                        }
                        else
                        {
                            // Sonst versuchen einen String aus dem Element zu extrahieren, um den anzeigen zu können
                            item = ExtractString(logicalElement);
                            itemTemplate = null;
                        }
                    }
                }

                SelectionBoxItem = item;
                SelectionBoxItemTemplate = itemTemplate;
            }
        }

        // Aktualisiert die Anzeige der ausgewählten Items, wenn die ComboBox im Edit-Modus ist
        private void UpdateText(bool useFormatString)
        {
            // Wenn die ComboBox nicht im Edit-Modus ist oder das Update unterdrückt wird, dann nichts machen
            if (!IsEditable || _supressUpdateText) return;
            // SelectionBoxItem ist im Edit-Modus null
            SelectionBoxItem = null;
            SelectionBoxItemTemplate = null;

            if (SelectedItems.Count == 0) Text = null;
            else if (AllowMultiSelection)
            {
                // Text auf den String aus SelectedItems setzen
                var resultString = GetMultiSelectionResultString(useFormatString);
                Text = resultString;
            }
            else
            {
                var item = SelectedItem;
                // Wenn das SelectedItem ein ContentControl ist, dann suchen wir in dessen Content
                if (item is ContentControl contentControl) item = contentControl.Content;

                var type = item.GetType();
                // Ist das item ein Primitiver Datentyp oder string?
                if (type.IsPrimitive || type == typeof(string)) Text = item.ToString();
                else
                {
                    var resultString = GetSelectionResultString();
                    Text = resultString;
                }
            }
            // Text in der TextBox setzen, falls sie vorhanden ist
            if (EditableTextBox != null)
            {
                _supressTextChanged = true;
                EditableTextBox.Text = Text;
                _supressTextChanged = false;
            }
        }

        // Holt den anzuzeigenden String im SingleSelect-Modus
        private string GetSelectionResultString()
        {
            if (SelectedItem == null) return null;

            // Pfad der Property suchen
            var propertyPath = GetSearchPropertyPath();

            // Wenn wir keinen PropertyPath haben, dann nur .ToString() nehmen
            if (propertyPath == null) return SelectedItem.ToString();
            // Wenn ein PropertyPath vorhanden ist, dann Wert extrahieren und in die Liste hinzufügen
            else return PropertyHelper.GetPropertyValueFromPath(SelectedItem, propertyPath)?.ToString();
        }

        // Holt den anzuzeigenden String im MultiSelect-Modus
        private string GetMultiSelectionResultString(bool useFormatString)
        {
            // Pfad der Property suchen
            var propertyPath = GetSearchPropertyPath();

            var propertyValues = new List<string>();

            for (int i = 0; i < SelectedItems.Count; i++)
            {
                var item = SelectedItems[i];

                if (item is ContentControl contentControl) item = contentControl.Content;

                // Wenn wir keinen PropertyPath haben, dann nur .ToString() nehmen
                if (propertyPath == null) propertyValues.Add(item.ToString());
                // Wenn ein PropertyPath vorhanden ist, dann Wert extrahieren und in die Liste hinzufügen
                else propertyValues.Add(PropertyHelper.GetPropertyValueFromPath(item, propertyPath)?.ToString());
            }

            // ResultString anhand angegebenen Properties zusammenbauen
            string resultString;

            // Soll der FormatString benutzt werden?
            if (useFormatString)
            {
                resultString = string.Join(string.Format(MultiSelectionSeparatorStringFormat ?? "{0} ", MultiSelectionSeparator), propertyValues);
            }
            else
            {
                resultString = string.Join(MultiSelectionSeparator.ToString(), propertyValues);
            }

            return resultString;
        }

        // Extrahiert einen String aus einem DependencyObject, falls einer zu finden ist
        private static string ExtractString(DependencyObject dependencyObject)
        {
            Visual visual;
            string result = string.Empty;

            if (dependencyObject is TextBlock text)
            {
                result = text.Text;
            }
            else if ((visual = dependencyObject as Visual) != null)
            {
                int count = VisualTreeHelper.GetChildrenCount(visual);

                for (int i = 0; i < count; i++)
                {
                    result += ExtractString((VisualTreeHelper.GetChild(visual, i)));
                }
            }

            return result;
        }

        // Updated das Layout des geklonten Elements, falls dieses für die Anzeige nötig war
        private void ClonedElementLayoutUpdated(object sender, EventArgs e)
        {
            var rectangle = (Rectangle)SelectionBoxItem;
            rectangle.Width = _clonedElement.RenderSize.Width;
            rectangle.Height = _clonedElement.RenderSize.Height;

            var visualBrush = (VisualBrush)rectangle.Fill;
            visualBrush.Viewbox = new Rect(_clonedElement.RenderSize);
            visualBrush.Viewport = new Rect(_clonedElement.RenderSize);
        }

        // Setzt IsHighlighted bei dem angegebenen Container
        internal void Highlight(ComboBoxItem item)
        {
            HighlightedItem = item;
        }

        // Selected den angegebenen Container
        internal void Select(ComboBoxItem item)
        {
            if (item == null || IsReadOnly) return;

            if (AllowMultiSelection)
            {
                item.IsSelected = !item.IsSelected;
            }
            else
            {
                var selectedItems = SelectedItems;

                BeginUpdateSelectedItems();

                for (int i = 0; i < selectedItems.Count; i++)
                {
                    var selectedItem = selectedItems[i];

                    SelectedItems.Remove(selectedItem);
                }

                item.IsSelected = true;
                // Wenn keine ItemsSource angegeben ist, sondern die ComboBoxItems so erstellt wurden, 
                // dann muss das item in die SelectedItems-Liste, an sonsten dessen Content
                // Wenn man das nicht macht, wird von EndUpdateSelectedItems() SelectedItems geleert, warum weiß ich nicht
                if (ItemsSource == null) SelectedItems.Add(item);
                else SelectedItems.Add(item.Content);

                EndUpdateSelectedItems();

                IsDropDownOpen = false;
            }
        }

        // Selected das nächste Item
        private void SelectNext()
        {
            if (Items.Count == 0 || IsReadOnly) return;

            if (SelectedIndex < Items.Count - 1) SelectedIndex++;
        }

        // Selected das vorherige Item
        private void SelectPrevious()
        {
            if (Items.Count == 0 || IsReadOnly) return;

            if (SelectedIndex > 0) SelectedIndex--;
        }

        // Setzt IsHighlighted beim nächsten Item
        private void HighlightNext()
        {
            if (Items.Count == 0) return;

            if (HighlightedItem == null)
            {
                var container = ItemContainerGenerator.ContainerFromItem(Items[0]) as ComboBoxItem;

                Highlight(container);
            }
            else
            {
                var index = ItemContainerGenerator.IndexFromContainer(HighlightedItem);

                if (index == Items.Count - 1) return;

                var container = ItemContainerGenerator.ContainerFromIndex(index + 1) as ComboBoxItem;

                Highlight(container);
            }
        }

        // Setzt IsHighlighted beim vorherigen Item
        private void HighlightPrevious()
        {
            if (Items.Count == 0) return;

            if (HighlightedItem == null)
            {
                var container = ItemContainerGenerator.ContainerFromItem(Items[0]) as ComboBoxItem;

                Highlight(container);
            }
            else
            {
                var index = ItemContainerGenerator.IndexFromContainer(HighlightedItem);

                if (index <= 0) return;

                var container = ItemContainerGenerator.ContainerFromIndex(index - 1) as ComboBoxItem;

                Highlight(container);
            }
        }

        // Löst das DropDownOpened-Event aus
        protected virtual void OnDropDownOpened(EventArgs e)
        {
            DropDownOpened?.Invoke(this, e);
        }

        // Löst das DropDownClosed-Event aus
        protected virtual void OnDropDownClosed(EventArgs e)
        {
            DropDownClosed?.Invoke(this, e);
        }

        // Aktualisiert den Angezeigten Text, wenn sich die Auswahl ändert
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            UpdateDisplayedSelectionText();
        }

        #region ItemsControl-Container-Logic
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ComboBoxItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ComboBoxItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (item is Separator separator)
            {
                separator.IsEnabled = false;
                separator.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            }
        }
        #endregion
    }
}