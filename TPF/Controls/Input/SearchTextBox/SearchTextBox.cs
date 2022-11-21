using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_DropDownSizeElement", Type = typeof(FrameworkElement))]
    public class SearchTextBox : Control
    {
        static SearchTextBox()
        {
            InitializeCommands();

            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchTextBox), new FrameworkPropertyMetadata(typeof(SearchTextBox)));
        }

        public SearchTextBox()
        {
            SizeChanged += SearchTextBox_SizeChanged;
        }

        private bool _supressSearching;

        #region Watermark DependencyProperty
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark",
            typeof(string),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        #endregion

        #region Value DependencyProperty
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(string),
            typeof(SearchTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SearchTextBox)sender;

            if (instance._supressSearching || !instance.IsEnabled) return;

            instance.FilterItems(instance.Value);
        }

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region SearchMode DependencyProperty
        public static readonly DependencyProperty SearchModeProperty = DependencyProperty.Register("SearchMode",
            typeof(SearchTextBoxMode),
            typeof(SearchTextBox),
            new PropertyMetadata(SearchTextBoxMode.Integrated));

        public SearchTextBoxMode SearchMode
        {
            get { return (SearchTextBoxMode)GetValue(SearchModeProperty); }
            set { SetValue(SearchModeProperty, value); }
        }
        #endregion

        #region SearchPath DependencyProperty
        public static readonly DependencyProperty SearchPathProperty = DependencyProperty.Register("SearchPath",
            typeof(string),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public string SearchPath
        {
            get { return (string)GetValue(SearchPathProperty); }
            set { SetValue(SearchPathProperty, value); }
        }
        #endregion

        #region FilteringBehavior DependencyProperty
        public static readonly DependencyProperty FilteringBehaviorProperty = DependencyProperty.Register("FilteringBehavior",
            typeof(FilteringBehavior),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public FilteringBehavior FilteringBehavior
        {
            get { return (FilteringBehavior)GetValue(FilteringBehaviorProperty); }
            set { SetValue(FilteringBehaviorProperty, value); }
        }
        #endregion

        #region Delay DependencyProperty
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay",
            typeof(int),
            typeof(SearchTextBox),
            new PropertyMetadata(100, OnDelayChanged));

        private static void OnDelayChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SearchTextBox)sender;

            instance.UpdateDelay();
        }

        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }
        #endregion

        #region IsDropDownOpen DependencyProperty
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen",
            typeof(bool),
            typeof(SearchTextBox),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged));

        private static void OnIsDropDownOpenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SearchTextBox)sender;

            if (instance.IsDropDownOpen) instance.SelectFirst();
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region DropDownDirection DependencyProperty
        public static readonly DependencyProperty DropDownDirectionProperty = DependencyProperty.Register("DropDownDirection",
            typeof(DropDownDirection),
            typeof(SearchTextBox),
            new PropertyMetadata(DropDownDirection.Bottom, OnDropDownDirectionChanged));

        private static void OnDropDownDirectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SearchTextBox)sender;

            instance.UpdatePopupPlacement();
        }

        public DropDownDirection DropDownDirection
        {
            get { return (DropDownDirection)GetValue(DropDownDirectionProperty); }
            set { SetValue(DropDownDirectionProperty, value); }
        }
        #endregion

        #region DropDownWidth DependencyProperty
        public static readonly DependencyProperty DropDownWidthProperty = DependencyProperty.Register("DropDownWidth",
            typeof(GridLength),
            typeof(SearchTextBox),
            new PropertyMetadata(new GridLength(1, GridUnitType.Auto), OnDropDownWidthChanged));

        private static void OnDropDownWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SearchTextBox)sender;

            instance.UpdateDropDownWidth();
        }

        public GridLength DropDownWidth
        {
            get { return (GridLength)GetValue(DropDownWidthProperty); }
            set { SetValue(DropDownWidthProperty, value); }
        }
        #endregion

        #region DropDownMinWidth Readonly DependencyProperty
        private static readonly DependencyPropertyKey DropDownMinWidthPropertyKey = DependencyProperty.RegisterReadOnly("DropDownMinWidth",
            typeof(double),
            typeof(SearchTextBox),
            new PropertyMetadata(0.0));

        public static readonly DependencyProperty DropDownMinWidthProperty = DropDownMinWidthPropertyKey.DependencyProperty;

        public double DropDownMinWidth
        {
            get { return (double)GetValue(DropDownMinWidthProperty); }
            private set { SetValue(DropDownMinWidthPropertyKey, value); }
        }
        #endregion

        #region DropDownMaxHeight DependencyProperty
        public static readonly DependencyProperty DropDownMaxHeightProperty = DependencyProperty.Register("DropDownMaxHeight",
            typeof(double),
            typeof(SearchTextBox),
            new PropertyMetadata(double.NaN));

        public double DropDownMaxHeight
        {
            get { return (double)GetValue(DropDownMaxHeightProperty); }
            set { SetValue(DropDownMaxHeightProperty, value); }
        }
        #endregion

        #region DropDownMaxWidth DependencyProperty
        public static readonly DependencyProperty DropDownMaxWidthProperty = DependencyProperty.Register("DropDownMaxWidth",
            typeof(double),
            typeof(SearchTextBox),
            new PropertyMetadata(double.NaN));

        public double DropDownMaxWidth
        {
            get { return (double)GetValue(DropDownMaxWidthProperty); }
            set { SetValue(DropDownMaxWidthProperty, value); }
        }
        #endregion

        #region ItemsSource DependencyProperty
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
            typeof(IEnumerable),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        #endregion

        #region FilteredItems ReadOnly DependencyProperty
        internal static readonly DependencyPropertyKey FilteredItemsPropertyKey = DependencyProperty.RegisterReadOnly("FilteredItems",
            typeof(IEnumerable),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public static readonly DependencyProperty FilteredItemsProperty = FilteredItemsPropertyKey.DependencyProperty;

        public IEnumerable FilteredItems
        {
            get { return (IEnumerable)GetValue(FilteredItemsProperty); }
            protected set { SetValue(FilteredItemsPropertyKey, value); }
        }
        #endregion

        #region DisplayMemberPath DependencyProperty
        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath",
            typeof(string),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        #endregion

        #region ItemContainerStyle DependencyProperty
        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle",
            typeof(Style),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }
        #endregion

        #region ItemContainerStyleSelector DependencyProperty
        public static readonly DependencyProperty ItemContainerStyleSelectorProperty = DependencyProperty.Register("ItemContainerStyleSelector",
            typeof(StyleSelector),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public StyleSelector ItemContainerStyleSelector
        {
            get { return (StyleSelector)GetValue(ItemContainerStyleSelectorProperty); }
            set { SetValue(ItemContainerStyleSelectorProperty, value); }
        }
        #endregion

        #region ItemTemplate DependencyProperty
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate",
            typeof(DataTemplate),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        #endregion

        #region ItemTemplateSelector DependencyProperty
        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register("ItemTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }
        #endregion

        #region ItemsBackground DependencyProperty
        public static readonly DependencyProperty ItemsBackgroundProperty = DependencyProperty.Register("ItemsBackground",
            typeof(Brush),
            typeof(SearchTextBox),
            new PropertyMetadata(Brushes.White));

        public Brush ItemsBackground
        {
            get { return (Brush)GetValue(ItemsBackgroundProperty); }
            set { SetValue(ItemsBackgroundProperty, value); }
        }
        #endregion

        #region ItemsForeground DependencyProperty
        public static readonly DependencyProperty ItemsForegroundProperty = DependencyProperty.Register("ItemsForeground",
            typeof(Brush),
            typeof(SearchTextBox),
            new PropertyMetadata(Brushes.Black));

        public Brush ItemsForeground
        {
            get { return (Brush)GetValue(ItemsForegroundProperty); }
            set { SetValue(ItemsForegroundProperty, value); }
        }
        #endregion

        #region ItemsBorderBrush DependencyProperty
        public static readonly DependencyProperty ItemsBorderBrushProperty = DependencyProperty.Register("ItemsBorderBrush",
            typeof(Brush),
            typeof(SearchTextBox),
            new PropertyMetadata(Brushes.LightGray));

        public Brush ItemsBorderBrush
        {
            get { return (Brush)GetValue(ItemsBorderBrushProperty); }
            set { SetValue(ItemsBorderBrushProperty, value); }
        }
        #endregion

        #region NoResultsContent DependencyProperty
        public static readonly DependencyProperty NoResultsContentProperty = DependencyProperty.Register("NoResultsContent",
            typeof(object),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public object NoResultsContent
        {
            get { return GetValue(NoResultsContentProperty); }
            set { SetValue(NoResultsContentProperty, value); }
        }
        #endregion

        #region NoResultsContentTemplate DependencyProperty
        public static readonly DependencyProperty NoResultsContentTemplateProperty = DependencyProperty.Register("NoResultsContentTemplate",
            typeof(DataTemplate),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public DataTemplate NoResultsContentTemplate
        {
            get { return (DataTemplate)GetValue(NoResultsContentTemplateProperty); }
            set { SetValue(NoResultsContentTemplateProperty, value); }
        }
        #endregion

        #region QueryButtonCommand DependencyProperty
        public static readonly DependencyProperty QueryButtonCommandProperty = DependencyProperty.Register("QueryButtonCommand",
            typeof(ICommand),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public ICommand QueryButtonCommand
        {
            get { return (ICommand)GetValue(QueryButtonCommandProperty); }
            set { SetValue(QueryButtonCommandProperty, value); }
        }
        #endregion

        #region QueryButtonVisibility DependencyProperty
        public static readonly DependencyProperty QueryButtonVisibilityProperty = DependencyProperty.Register("QueryButtonVisibility",
            typeof(Visibility),
            typeof(SearchTextBox),
            new PropertyMetadata(VisibilityBoxes.CollapsedBox));

        public Visibility QueryButtonVisibility
        {
            get { return (Visibility)GetValue(QueryButtonVisibilityProperty); }
            set { SetValue(QueryButtonVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region QueryButtonContent DependencyProperty
        public static readonly DependencyProperty QueryButtonContentProperty = DependencyProperty.Register("QueryButtonContent",
            typeof(object),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public object QueryButtonContent
        {
            get { return GetValue(QueryButtonContentProperty); }
            set { SetValue(QueryButtonContentProperty, value); }
        }
        #endregion

        #region QueryButtonContentTemplate DependencyProperty
        public static readonly DependencyProperty QueryButtonContentTemplateProperty = DependencyProperty.Register("QueryButtonContentTemplate",
            typeof(DataTemplate),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public DataTemplate QueryButtonContentTemplate
        {
            get { return (DataTemplate)GetValue(QueryButtonContentTemplateProperty); }
            set { SetValue(QueryButtonContentTemplateProperty, value); }
        }
        #endregion

        #region ClearAfterSelection DependencyProperty
        public static readonly DependencyProperty ClearAfterSelectionProperty = DependencyProperty.Register("ClearAfterSelection",
            typeof(bool),
            typeof(SearchTextBox),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool ClearAfterSelection
        {
            get { return (bool)GetValue(ClearAfterSelectionProperty); }
            set { SetValue(ClearAfterSelectionProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IgnoreCase DependencyProperty
        public static readonly DependencyProperty IgnoreCaseProperty = DependencyProperty.Register("IgnoreCase",
            typeof(bool),
            typeof(SearchTextBox),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool IgnoreCase
        {
            get { return (bool)GetValue(IgnoreCaseProperty); }
            set { SetValue(IgnoreCaseProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region MaxLength DependencyProperty
        public static readonly DependencyProperty MaxLengthProperty = TextBox.MaxLengthProperty.AddOwner(typeof(SearchTextBox),
            new PropertyMetadata(0));

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
        #endregion

        #region CharacterCasing DependencyProperty
        public static readonly DependencyProperty CharacterCasingProperty = TextBox.CharacterCasingProperty.AddOwner(typeof(SearchTextBox),
            new PropertyMetadata(CharacterCasing.Normal));

        public CharacterCasing CharacterCasing
        {
            get { return (CharacterCasing)GetValue(CharacterCasingProperty); }
            set { SetValue(CharacterCasingProperty, value); }
        }
        #endregion

        internal TextBox TextBox;
        internal ListBox ItemsListBox;
        internal Popup ItemsPopup;
        internal ContentPresenter NoResultsContentPresenter;
        private FrameworkElement _dropDownSizeElement;

        public static RoutedCommand SubmitQuery { get; private set; }

        private static void InitializeCommands()
        {
            var type = typeof(SearchTextBox);

            SubmitQuery = new RoutedCommand("SubmitQuery", type);

            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(SubmitQuery, OnSubmitQueryCommand));
        }

        public event EventHandler<ItemSelectedEventArgs> ItemSelected;

        public event EventHandler<SearchingEventArgs> Searching;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ItemsListBox != null) ItemsListBox.MouseLeftButtonUp -= ItemsListBox_MouseLeftButtonUp;

            TextBox = GetTemplateChild("PART_TextBox") as TextBox;
            ItemsListBox = GetTemplateChild("PART_ListBox") as ListBox;
            ItemsPopup = GetTemplateChild("PART_Popup") as Popup;
            NoResultsContentPresenter = GetTemplateChild("PART_NoResultsContentPresenter") as ContentPresenter;
            _dropDownSizeElement = GetTemplateChild("PART_DropDownSizeElement") as FrameworkElement;

            UpdateDelay();
            UpdatePopupPlacement();

            if (ItemsListBox == null) return;

            ItemsListBox.MouseLeftButtonUp += ItemsListBox_MouseLeftButtonUp;
        }

        private void ItemsListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CommitSelection();
        }

        // Setzt die Value-Property und bestimmt, ob die Suche gestartet oder unterdrückt werden soll
        public void SetControlValue(string value, bool search = true)
        {
            if (!search) _supressSearching = true;
            Value = value;
            if (!search) _supressSearching = false;
        }

        private void FilterItems(string value)
        {
            var e = new SearchingEventArgs(value);

            // Ist eine eigene Suche gewünscht
            if (SearchMode == SearchTextBoxMode.Event)
            {
                // Eigene Suche starten
                OnSearching(e);
            }
            else if (string.IsNullOrEmpty(value)) e.Results = ItemsSource;
            else
            {
                // Interne Suche starten
                OnIntegratedFiltering(e);
            }

            // Resultate verarbeiten
            OnProcessResults(e);
        }

        protected virtual void OnSearching(SearchingEventArgs e)
        {
            Searching?.Invoke(this, e);
        }

        protected virtual void OnIntegratedFiltering(SearchingEventArgs e)
        {
            // Wenn kein eigenes FilteringBehavior festgelegt wurde, dann Default nehmen
            var filteringBehavior = FilteringBehavior ?? new FilteringBehavior();
            // Filtern
            e.Results = filteringBehavior.FilterItems(ItemsSource, e.Value, SearchPath, IgnoreCase, null);
        }

        protected virtual void OnProcessResults(SearchingEventArgs e)
        {
            FilteredItems = e.Results;

            var openDropDown = ItemsListBox?.HasItems == true;

            if (NoResultsContentPresenter != null)
            {
                if (ItemsListBox?.HasItems == true) NoResultsContentPresenter.Visibility = Visibility.Collapsed;
                else
                {
                    NoResultsContentPresenter.Visibility = Visibility.Visible;
                    if (NoResultsContent != null) openDropDown = true;
                }
            }

            IsDropDownOpen = openDropDown;

            SelectFirst();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            // Wenn der Focus in der TextBox liegt, wird OnKeyDown nicht für alle Tasten ausgelöst
            if (e.OriginalSource == TextBox) KeyDownHandler(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Wenn der Focus in der TextBox liegt, wird die Logik schon von OnPreviewKeyDown übernommen
            if (e.OriginalSource != TextBox) KeyDownHandler(e);
        }

        private void KeyDownHandler(KeyEventArgs e)
        {
            if (!IsEnabled) return;

            switch (e.Key)
            {
                case Key.Up:
                {
                    SelectPrevious();
                    e.Handled = true;
                    break;
                }
                case Key.Down:
                {
                    SelectNext();
                    e.Handled = true;
                    break;
                }
                case Key.Enter:
                {
                    if (IsDropDownOpen)
                    {
                        CommitSelection();
                        e.Handled = true;
                    }
                    else
                    {
                        Value = TextBox?.Text;
                        IsDropDownOpen = ItemsListBox?.HasItems ?? false;
                        //e.Handled = true;
                    }
                    break;
                }
                case Key.Tab:
                {
                    if (IsDropDownOpen)
                    {
                        CommitSelection();
                        e.Handled = true;
                    }
                    break;
                }
                case Key.Escape:
                {
                    if (IsDropDownOpen)
                    {
                        IsDropDownOpen = false;
                        e.Handled = true;
                    }
                    break;
                }
            }
        }

        private void SelectFirst()
        {
            if (ItemsListBox == null || ItemsListBox.Items.Count <= 0) return;

            ItemsListBox.SelectedIndex = 0;
            ItemsListBox.ScrollIntoView(ItemsListBox.SelectedItem);
        }

        private void SelectLast()
        {
            if (ItemsListBox == null || ItemsListBox.Items.Count <= 0) return;

            ItemsListBox.SelectedIndex = ItemsListBox.Items.Count - 1;
            ItemsListBox.ScrollIntoView(ItemsListBox.SelectedItem);
        }

        private void SelectNext()
        {
            if (ItemsListBox == null || ItemsListBox.Items.Count <= 0) return;

            if (ItemsListBox.SelectedIndex < ItemsListBox.Items.Count - 1) ItemsListBox.SelectedIndex++;
            ItemsListBox.ScrollIntoView(ItemsListBox.SelectedItem);
        }

        private void SelectPrevious()
        {
            if (ItemsListBox == null || ItemsListBox.Items.Count <= 0) return;

            if (ItemsListBox.SelectedIndex > 0) ItemsListBox.SelectedIndex--;
            ItemsListBox.ScrollIntoView(ItemsListBox.SelectedItem);
        }

        private void CommitSelection()
        {
            if (ItemsListBox?.SelectedItem == null) return;

            var selectedItem = ItemsListBox.SelectedItem;

            string value;

            var type = selectedItem.GetType();

            if (type.IsPrimitive || type == typeof(string))
            {
                value = selectedItem.ToString();
            }
            else value = PropertyHelper.GetPropertyValueFromPath<string>(selectedItem, SearchPath);

            ItemSelected?.Invoke(this, new ItemSelectedEventArgs(selectedItem, value));

            if (ClearAfterSelection) Value = null;
            else
            {
                SetControlValue(value, false);
                FilteredItems = null;
                IsDropDownOpen = false;
                if (TextBox != null && value != null) TextBox.CaretIndex = Math.Max(value.Length, 0);
            }
        }

        private void UpdateDelay()
        {
            if (TextBox == null) return;

            var binding = new Binding("Value")
            {
                Delay = Delay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Source = this
            };

            TextBox.SetBinding(TextBox.TextProperty, binding);
        }

        private void UpdatePopupPlacement()
        {
            if (ItemsPopup == null) return;

            switch (DropDownDirection)
            {
                case DropDownDirection.Bottom: ItemsPopup.Placement = PlacementMode.Bottom; break;
                case DropDownDirection.Left: ItemsPopup.Placement = PlacementMode.Left; break;
                case DropDownDirection.Right: ItemsPopup.Placement = PlacementMode.Right; break;
                case DropDownDirection.Top: ItemsPopup.Placement = PlacementMode.Top; break;
            }
        }

        private void UpdateDropDownWidth()
        {
            if (_dropDownSizeElement == null) return;

            var dropDownWidth = DropDownWidth;

            if (dropDownWidth.IsAbsolute)
            {
                _dropDownSizeElement.Width = dropDownWidth.Value;
            }
            else if (dropDownWidth.IsAuto)
            {
                _dropDownSizeElement.Width = double.NaN;
            }
            else if (dropDownWidth.IsStar)
            {
                _dropDownSizeElement.Width = ActualWidth;
            }

            if (DropDownMinWidth > _dropDownSizeElement.Width)
            {
                DropDownMinWidth = 0;
            }
        }

        private void SearchTextBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DesignerHelper.IsInDesignMode) return;

            var width = e.NewSize.Width;

            if (!double.IsNaN(width) && !double.IsInfinity(width) && width > 0) DropDownMinWidth = width;

            UpdateDropDownWidth();
        }

        private static void OnSubmitQueryCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (SearchTextBox)sender;

            if (instance == null) return;

            instance.FilterItems(instance.Value);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            // Versuchen Focus an die Textbox weiterzugeben wenn vorhanden
            TextBox?.Focus();
        }
    }

    public enum DropDownDirection
    {
        Bottom = 0,
        Left = 1,
        Right = 2,
        Top = 3
    }

    public enum SearchTextBoxMode
    {
        Integrated = 0,
        Event = 1
    }
}