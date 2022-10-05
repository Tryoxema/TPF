using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TPF.Internal;

namespace TPF.Controls.Specialized.Calendar
{
    public class CalendarButton : ContentControl
    {
        static CalendarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarButton), new FrameworkPropertyMetadata(typeof(CalendarButton)));
        }

        #region CalendarButtonType DependencyProperty
        public static readonly DependencyProperty CalendarButtonTypeProperty = DependencyProperty.Register("CalendarButtonType",
            typeof(CalendarButtonType),
            typeof(CalendarButton),
            new PropertyMetadata(CalendarButtonType.Day, OnCalendarButtonTypeChanged));

        static void OnCalendarButtonTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (CalendarButton)sender;

            if (instance._isTemplateApplied) instance.ChangeVisualState(true);
        }

        public CalendarButtonType CalendarButtonType
        {
            get { return (CalendarButtonType)GetValue(CalendarButtonTypeProperty); }
            set { SetValue(CalendarButtonTypeProperty, value); }
        }
        #endregion

        #region IsFromCurrentView DependencyProperty
        public static readonly DependencyProperty IsFromCurrentViewProperty = DependencyProperty.Register("IsFromCurrentView",
            typeof(bool),
            typeof(CalendarButton),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnIsFromCurrentViewChanged));

        static void OnIsFromCurrentViewChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (CalendarButton)sender;

            if (instance._isTemplateApplied) instance.ChangeVisualState(true);
        }

        public bool IsFromCurrentView
        {
            get { return (bool)GetValue(IsFromCurrentViewProperty); }
            set { SetValue(IsFromCurrentViewProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsSelected DependencyProperty
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected",
            typeof(bool),
            typeof(CalendarButton),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsSelectedChanged));

        static void OnIsSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (CalendarButton)sender;

            if (instance._isTemplateApplied) instance.ChangeVisualState(true);
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        private bool _isTemplateApplied;

        internal Controls.Calendar ParentCalendar;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _isTemplateApplied = true;

            ChangeVisualState(false);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            ChangeVisualState(true);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            ChangeVisualState(true);
        }

        private void ChangeVisualState(bool useTransitions)
        {
            if (!IsEnabled) VisualStateManager.GoToState(this, "Disabled", useTransitions);
            else if (!IsFromCurrentView) VisualStateManager.GoToState(this, "NotFromCurrentView", useTransitions);
            else VisualStateManager.GoToState(this, "Normal", useTransitions);

            switch (CalendarButtonType)
            {
                case CalendarButtonType.Day: VisualStateManager.GoToState(this, "Day", useTransitions); break;
                case CalendarButtonType.Month: VisualStateManager.GoToState(this, "Month", useTransitions); break;
                case CalendarButtonType.Year: VisualStateManager.GoToState(this, "Year", useTransitions); break;
                case CalendarButtonType.Decade: VisualStateManager.GoToState(this, "Decade", useTransitions); break;
                case CalendarButtonType.DayOfWeek: VisualStateManager.GoToState(this, "DayOfWeek", useTransitions); break;
                case CalendarButtonType.WeekNumber: VisualStateManager.GoToState(this, "WeekNumber", useTransitions); break;
                case CalendarButtonType.Today: VisualStateManager.GoToState(this, "Today", useTransitions); break;
            }

            if (IsSelected) VisualStateManager.GoToState(this, "Selected", useTransitions);
            else if (IsMouseOver) VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            else VisualStateManager.GoToState(this, "Unselected", useTransitions);
        }
    }
}