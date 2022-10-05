using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TPF.Controls
{
    public class TaskBoardCard : Control
    {
        static TaskBoardCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TaskBoardCard), new FrameworkPropertyMetadata(typeof(TaskBoardCard)));
        }

        #region Id DependencyProperty
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id",
            typeof(object),
            typeof(TaskBoardCard),
            new PropertyMetadata(null));

        public object Id
        {
            get { return GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        #endregion

        #region Assignee DependencyProperty
        public static readonly DependencyProperty AssigneeProperty = DependencyProperty.Register("Assignee",
            typeof(string),
            typeof(TaskBoardCard),
            new PropertyMetadata(null));

        public string Assignee
        {
            get { return (string)GetValue(AssigneeProperty); }
            set { SetValue(AssigneeProperty, value); }
        }
        #endregion

        #region Title DependencyProperty
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string),
            typeof(TaskBoardCard),
            new PropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion

        #region Description DependencyProperty
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description",
            typeof(string),
            typeof(TaskBoardCard),
            new PropertyMetadata(null));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        #endregion

        #region State DependencyProperty
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State",
            typeof(object),
            typeof(TaskBoardCard),
            new PropertyMetadata(null));

        public object State
        {
            get { return GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }
        #endregion

        #region Type DependencyProperty
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type",
            typeof(object),
            typeof(TaskBoardCard),
            new PropertyMetadata(null, OnTypeChanged));

        private static void OnTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TaskBoardCard)sender;

            instance.ResolveIndicatorBrush();
        }

        public object Type
        {
            get { return GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        #endregion

        #region IndicatorBrush DependencyProperty
        public static readonly DependencyProperty IndicatorBrushProperty = DependencyProperty.Register("IndicatorBrush",
            typeof(Brush),
            typeof(TaskBoardCard),
            new PropertyMetadata(null));

        public Brush IndicatorBrush
        {
            get { return (Brush)GetValue(IndicatorBrushProperty); }
            set { SetValue(IndicatorBrushProperty, value); }
        }
        #endregion

        #region Icon DependencyProperty
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon",
            typeof(object),
            typeof(TaskBoardCard),
            new PropertyMetadata(null));

        public object Icon
        {
            get { return GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        #endregion

        #region IconTemplate DependencyProperty
        public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register("IconTemplate",
            typeof(DataTemplate),
            typeof(TaskBoardCard),
            new PropertyMetadata(null));

        public DataTemplate IconTemplate
        {
            get { return (DataTemplate)GetValue(IconTemplateProperty); }
            set { SetValue(IconTemplateProperty, value); }
        }
        #endregion

        #region IconTemplateSelector DependencyProperty
        public static readonly DependencyProperty IconTemplateSelectorProperty = DependencyProperty.Register("IconTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(TaskBoardCard),
            new PropertyMetadata(null));

        public DataTemplateSelector IconTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(IconTemplateSelectorProperty); }
            set { SetValue(IconTemplateSelectorProperty, value); }
        }
        #endregion

        #region Tags DependencyProperty
        public static readonly DependencyProperty TagsProperty = DependencyProperty.Register("Tags",
            typeof(IEnumerable<object>),
            typeof(TaskBoardCard),
            new PropertyMetadata(null));

        public IEnumerable<object> Tags
        {
            get { return (IEnumerable<object>)GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }
        #endregion

        #region TagTemplate DependencyProperty
        public static readonly DependencyProperty TagTemplateProperty = DependencyProperty.Register("TagTemplate",
            typeof(DataTemplate),
            typeof(TaskBoardCard),
            new PropertyMetadata(null));

        public DataTemplate TagTemplate
        {
            get { return (DataTemplate)GetValue(TagTemplateProperty); }
            set { SetValue(TagTemplateProperty, value); }
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(TaskBoardCard),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        private TaskBoard _taskBoard;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _taskBoard = this.ParentOfType<TaskBoard>();

            ResolveIndicatorBrush();
        }

        protected virtual void ResolveIndicatorBrush()
        {
            if (_taskBoard == null || Type == null) return;

            var brush = _taskBoard.IndicatorMapping?.GetBrushFromKey(Type);

            SetCurrentValue(IndicatorBrushProperty, brush);
        }
    }
}