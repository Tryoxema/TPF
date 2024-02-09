using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public class SplitView : Control
    {
        static SplitView()
        {
            InitializeCommands();

            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitView), new FrameworkPropertyMetadata(typeof(SplitView)));
        }

        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(SplitView),
            new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region FirstContent DependencyProperty
        public static readonly DependencyProperty FirstContentProperty = DependencyProperty.Register("FirstContent",
            typeof(object),
            typeof(SplitView),
            new PropertyMetadata(null));

        public object FirstContent
        {
            get { return GetValue(FirstContentProperty); }
            set { SetValue(FirstContentProperty, value); }
        }
        #endregion

        #region FirstContentTemplate DependencyProperty
        public static readonly DependencyProperty FirstContentTemplateProperty = DependencyProperty.Register("FirstContentTemplate",
            typeof(DataTemplate),
            typeof(SplitView),
            new PropertyMetadata(null));

        public DataTemplate FirstContentTemplate
        {
            get { return (DataTemplate)GetValue(FirstContentTemplateProperty); }
            set { SetValue(FirstContentTemplateProperty, value); }
        }
        #endregion

        #region FirstContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty FirstContentTemplateSelectorProperty = DependencyProperty.Register("FirstContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(SplitView),
            new PropertyMetadata(null));

        public DataTemplateSelector FirstContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(FirstContentTemplateSelectorProperty); }
            set { SetValue(FirstContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region SecondContent DependencyProperty
        public static readonly DependencyProperty SecondContentProperty = DependencyProperty.Register("SecondContent",
            typeof(object),
            typeof(SplitView),
            new PropertyMetadata(null));

        public object SecondContent
        {
            get { return GetValue(SecondContentProperty); }
            set { SetValue(SecondContentProperty, value); }
        }
        #endregion

        #region SecondContentTemplate DependencyProperty
        public static readonly DependencyProperty SecondContentTemplateProperty = DependencyProperty.Register("SecondContentTemplate",
            typeof(DataTemplate),
            typeof(SplitView),
            new PropertyMetadata(null));

        public DataTemplate SecondContentTemplate
        {
            get { return (DataTemplate)GetValue(SecondContentTemplateProperty); }
            set { SetValue(SecondContentTemplateProperty, value); }
        }
        #endregion

        #region SecondContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty SecondContentTemplateSelectorProperty = DependencyProperty.Register("SecondContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(SplitView),
            new PropertyMetadata(null));

        public DataTemplateSelector SecondContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(SecondContentTemplateSelectorProperty); }
            set { SetValue(SecondContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region IsContentSwaped DependencyProperty
        public static readonly DependencyProperty IsContentSwapedProperty = DependencyProperty.Register("IsContentSwaped",
            typeof(bool),
            typeof(SplitView),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsContentSwaped
        {
            get { return (bool)GetValue(IsContentSwapedProperty); }
            set { SetValue(IsContentSwapedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region SplitBarBackgroundBrush DependencyProperty
        public static readonly DependencyProperty SplitBarBackgroundBrushProperty = DependencyProperty.Register("SplitBarBackgroundBrush",
            typeof(Brush),
            typeof(SplitView),
            new PropertyMetadata(null));

        public Brush SplitBarBackgroundBrush
        {
            get { return (Brush)GetValue(SplitBarBackgroundBrushProperty); }
            set { SetValue(SplitBarBackgroundBrushProperty, value); }
        }
        #endregion

        #region SplitBarForegroundBrush DependencyProperty
        public static readonly DependencyProperty SplitBarForegroundBrushProperty = DependencyProperty.Register("SplitBarForegroundBrush",
            typeof(Brush),
            typeof(SplitView),
            new PropertyMetadata(null));

        public Brush SplitBarForegroundBrush
        {
            get { return (Brush)GetValue(SplitBarForegroundBrushProperty); }
            set { SetValue(SplitBarForegroundBrushProperty, value); }
        }
        #endregion

        public static RoutedCommand SwapContent { get; private set; }
        public static RoutedCommand VerticalLayout { get; private set; }
        public static RoutedCommand HorizontalLayout { get; private set; }

        private static void InitializeCommands()
        {
            var type = typeof(SplitView);

            SwapContent = new RoutedCommand("SwapContent", type);
            VerticalLayout = new RoutedCommand("VerticalLayout", type);
            HorizontalLayout = new RoutedCommand("HorizontalLayout", type);

            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(SwapContent, OnSwapContentCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(VerticalLayout, OnVerticalLayoutCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(HorizontalLayout, OnHorizontalLayoutCommand));
        }

        private static void OnSwapContentCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is SplitView slider) slider.OnSwapContent();
        }

        private static void OnVerticalLayoutCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is SplitView slider) slider.OnVerticalLayout();
        }

        private static void OnHorizontalLayoutCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is SplitView slider) slider.OnHorizontalLayout();
        }

        protected virtual void OnSwapContent()
        {
            IsContentSwaped = !IsContentSwaped;
        }

        protected virtual void OnVerticalLayout()
        {
            Orientation = Orientation.Vertical;
        }

        protected virtual void OnHorizontalLayout()
        {
            Orientation = Orientation.Horizontal;
        }
    }
}