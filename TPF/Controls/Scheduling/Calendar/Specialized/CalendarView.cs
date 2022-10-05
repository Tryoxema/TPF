using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TPF.Internal;

namespace TPF.Controls.Specialized.Calendar
{
    public class CalendarView : HeaderedItemsControl
    {
        static CalendarView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarView), new FrameworkPropertyMetadata(typeof(CalendarView)));

            EventManager.RegisterClassHandler(typeof(CalendarButton), MouseLeftButtonUpEvent, new MouseButtonEventHandler(CalendarButton_MouseLeftButtonUp), true);
        }

        public CalendarView()
        {
            Loaded += CalendarView_Loaded;
        }

        public CalendarView(Controls.Calendar calendar) : this()
        {
            ParentCalendar = calendar;
        }

        #region HeaderVisibility DependencyProperty
        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register("HeaderVisibility",
            typeof(Visibility),
            typeof(CalendarView),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public Visibility HeaderVisibility
        {
            get { return (Visibility)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region HideFirstColumn DependencyProperty
        public static readonly DependencyProperty HideFirstColumnProperty = DependencyProperty.Register("HideFirstColumn",
            typeof(bool),
            typeof(CalendarView),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnHideFirstColumnChanged));

        static void OnHideFirstColumnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (CalendarView)sender;

            if (instance.CalendarPanel != null) instance.CalendarPanel.HideFirstColumn = (bool)e.NewValue;
        }

        public bool HideFirstColumn
        {
            get { return (bool)GetValue(HideFirstColumnProperty); }
            set { SetValue(HideFirstColumnProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region HideFirstRow DependencyProperty
        public static readonly DependencyProperty HideFirstRowProperty = DependencyProperty.Register("HideFirstRow",
            typeof(bool),
            typeof(CalendarView),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnHideFirstRowChanged));

        static void OnHideFirstRowChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (CalendarView)sender;

            if (instance.CalendarPanel != null) instance.CalendarPanel.HideFirstRow = (bool)e.NewValue;
        }

        public bool HideFirstRow
        {
            get { return (bool)GetValue(HideFirstRowProperty); }
            set { SetValue(HideFirstRowProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        internal Controls.Calendar ParentCalendar;
        internal UniformGrid CalendarPanel;

        private void CalendarView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= CalendarView_Loaded;

            CalendarPanel = this.ChildOfType<UniformGrid>();

            if (CalendarPanel != null)
            {
                CalendarPanel.HideFirstColumn = HideFirstColumn;
                CalendarPanel.HideFirstRow = HideFirstRow;
            }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CalendarButton;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CalendarButton();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (item is CalendarButtonContent content && element is CalendarButton button)
            {
                button.ParentCalendar = ParentCalendar;
                button.CalendarButtonType = content.DisplayMode;
                button.IsFromCurrentView = content.IsFromCurrentView;
                button.SetBinding(CalendarButton.CalendarButtonTypeProperty, new Binding("DisplayMode") { Source = content });
                button.SetBinding(IsEnabledProperty, new Binding("IsEnabled") { Source = content });
                button.SetBinding(CalendarButton.IsSelectedProperty, new Binding("IsSelected") { Source = content });
                button.SetBinding(VisibilityProperty, new Binding("Hide") { Source = content, Converter = new Converter.InvertedBooleanToVisibilityConverter() });
                button.SetBinding(ToolTipProperty, new Binding("ToolTip") { Source = content });
                button.SetBinding(ContentControl.ContentTemplateProperty, new Binding("ContentTemplate") { Source = content });

                if (button.CalendarButtonType != CalendarButtonType.DayOfWeek && button.CalendarButtonType != CalendarButtonType.WeekNumber)
                {
                    button.MouseLeftButtonDown += Button_MouseLeftButtonDown;
                    button.MouseEnter += Button_MouseEnter;
                }
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            // Bindings entfernen
            BindingOperations.ClearBinding(element, CalendarButton.CalendarButtonTypeProperty);
            BindingOperations.ClearBinding(element, IsEnabledProperty);
            BindingOperations.ClearBinding(element, CalendarButton.IsSelectedProperty);
            BindingOperations.ClearBinding(element, VisibilityProperty);
            BindingOperations.ClearBinding(element, ToolTipProperty);
            BindingOperations.ClearBinding(element, ContentControl.ContentTemplateProperty);

            if (element is CalendarButton button)
            {
                button.MouseLeftButtonDown -= Button_MouseLeftButtonDown;
                button.MouseEnter -= Button_MouseEnter;
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            Mouse.Capture(null);

            if (ParentCalendar != null) ParentCalendar.FinishSelection();
        }

        private static void CalendarButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Da MouseLeftButtonUp beim klicken auf ein Element außerhalb der aktuellen View (z.B. Vormonat) nicht auf der View getriggert wird,
            // sondern nur auf dem schon weggeräumten Container, müssen wir das hier machen
            if (sender is CalendarButton button && button.ParentCalendar != null) button.ParentCalendar.FinishSelection();
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ParentCalendar == null ||ParentCalendar.IsReadOnly) return;

            if (sender is CalendarButton button && button.Content is CalendarButtonContent content)
            {
                switch (ParentCalendar.DisplayMode)
                {
                    case DisplayMode.MonthView:
                    {
                        if (ParentCalendar.DateSelectionMode == DateSelectionMode.Day)
                        {
                            HandleMouseDownSelection(content);
                        }
                        break;
                    }
                    case DisplayMode.YearView:
                    {
                        if (ParentCalendar.DateSelectionMode == DateSelectionMode.Month)
                        {
                            HandleMouseDownSelection(content);
                        }
                        else
                        {
                            ParentCalendar.ChangeViewAndDate(DisplayMode.MonthView, content.Date);
                        }
                        break;
                    }
                    case DisplayMode.DecadeView:
                    {
                        if (ParentCalendar.DateSelectionMode == DateSelectionMode.Year)
                        {
                            HandleMouseDownSelection(content);
                        }
                        else
                        {
                            ParentCalendar.ChangeViewAndDate(DisplayMode.YearView, content.Date);
                        }
                        break;
                    }
                    case DisplayMode.CenturyView:
                    {
                        ParentCalendar.ChangeViewAndDate(DisplayMode.DecadeView, content.Date);
                        break;
                    }
                }
            }
        }

        private void HandleMouseDownSelection(CalendarButtonContent content)
        {
            switch (ParentCalendar.SelectionMode)
            {
                case SelectionMode.Single:
                {
                    Mouse.Capture(this);
                    content.IsSelected = !content.IsSelected;
                    if (content.IsSelected) ParentCalendar.SelectDate(content.Date, !content.IsFromCurrentView);
                    else ParentCalendar.UnSelectDate(content.Date, !content.IsFromCurrentView);
                    break;
                }
                case SelectionMode.Multiple:
                {
                    ParentCalendar.StartSelection();
                    content.IsSelected = !content.IsSelected;
                    if (content.IsSelected) ParentCalendar.SelectDate(content.Date, !content.IsFromCurrentView);
                    else ParentCalendar.UnSelectDate(content.Date, !content.IsFromCurrentView);
                    break;
                }
                case SelectionMode.Extended:
                {
                    var isControlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                    var isShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

                    if (!isControlDown) ParentCalendar.SelectedDates.Clear();

                    ParentCalendar.StartSelection();

                    if (isShiftDown)
                    {
                        ParentCalendar.SelectDateRange(ParentCalendar.SelectedDate ?? ParentCalendar.DisplayDate, content.Date, !isControlDown);
                        if (isControlDown) ParentCalendar.SelectDate(content.Date, !content.IsFromCurrentView);
                    }
                    else ParentCalendar.SelectDate(content.Date, !content.IsFromCurrentView);

                    break;
                }
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ParentCalendar == null || !ParentCalendar.IsSelectionInProgress()) return;

            if (sender is CalendarButton button && button.Content is CalendarButtonContent content)
            {
                // Wir behandeln hier nur Modul Multiple und Extended, da es im Modus Single unmöglich ist diesen Punkt zu erreichen
                switch (ParentCalendar.SelectionMode)
                {
                    case SelectionMode.Multiple:
                    {
                        content.IsSelected = !content.IsSelected;
                        if (content.IsSelected) ParentCalendar.SelectDate(content.Date, false);
                        else ParentCalendar.UnSelectDate(content.Date, false);
                        break;
                    }
                    case SelectionMode.Extended:
                    {
                        var isControlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                        var isShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

                        ParentCalendar.SelectDateRange(ParentCalendar.SelectedDate ?? ParentCalendar.DisplayDate, content.Date, !isControlDown);
                        if (isControlDown) ParentCalendar.SelectDate(content.Date, !content.IsFromCurrentView);
                        break;
                    }
                }
            }
        }
    }
}