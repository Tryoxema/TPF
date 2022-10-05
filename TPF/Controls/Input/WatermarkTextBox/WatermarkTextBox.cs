using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TPF.Internal;

namespace TPF.Controls
{
    public class WatermarkTextBox : TextBox
    {
        static WatermarkTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextBox), new FrameworkPropertyMetadata(typeof(WatermarkTextBox)));
        }

        #region Watermark DependencyProperty
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark",
            typeof(object),
            typeof(WatermarkTextBox),
            new PropertyMetadata(null));

        public object Watermark
        {
            get { return GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        #endregion

        #region WatermarkTemplate DependencyProperty
        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register("WatermarkTemplate",
            typeof(DataTemplate),
            typeof(WatermarkTextBox),
            new PropertyMetadata(null));

        public DataTemplate WatermarkTemplate
        {
            get { return (DataTemplate)GetValue(WatermarkTemplateProperty); }
            set { SetValue(WatermarkTemplateProperty, value); }
        }
        #endregion

        #region WatermarkTemplateSelector DependencyProperty
        public static readonly DependencyProperty WatermarkTemplateSelectorProperty = DependencyProperty.Register("WatermarkTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(WatermarkTextBox),
            new PropertyMetadata(null));

        public DataTemplateSelector WatermarkTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(WatermarkTemplateSelectorProperty); }
            set { SetValue(WatermarkTemplateSelectorProperty, value); }
        }
        #endregion

        #region WatermarkBehavior DependencyProperty
        public static readonly DependencyProperty WatermarkBehaviorProperty = DependencyProperty.Register("WatermarkBehavior",
            typeof(WatermarkBehavior),
            typeof(WatermarkTextBox),
            new PropertyMetadata(WatermarkBehavior.HideOnTextEntered));

        public WatermarkBehavior WatermarkBehavior
        {
            get { return (WatermarkBehavior)GetValue(WatermarkBehaviorProperty); }
            set { SetValue(WatermarkBehaviorProperty, value); }
        }
        #endregion

        #region IsWatermarkVisible ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey IsWatermarkVisiblePropertyKey = DependencyProperty.RegisterReadOnly("IsWatermarkVisible",
            typeof(bool),
            typeof(WatermarkTextBox),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public static readonly DependencyProperty IsWatermarkVisibleProperty = IsWatermarkVisiblePropertyKey.DependencyProperty;

        public bool IsWatermarkVisible
        {
            get { return (bool)GetValue(IsWatermarkVisibleProperty); }
            protected set { SetValue(IsWatermarkVisiblePropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region SelectionOnFocus DependencyProperty
        public static readonly DependencyProperty SelectionOnFocusProperty = DependencyProperty.Register("SelectionOnFocus",
            typeof(SelectionOnFocus),
            typeof(WatermarkTextBox),
            new PropertyMetadata(SelectionOnFocus.Default));

        public SelectionOnFocus SelectionOnFocus
        {
            get { return (SelectionOnFocus)GetValue(SelectionOnFocusProperty); }
            set { SetValue(SelectionOnFocusProperty, value); }
        }
        #endregion

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            switch (SelectionOnFocus)
            {
                case SelectionOnFocus.Default: break;
                case SelectionOnFocus.CaretAtBeginning:
                    CaretIndex = 0;
                    break;
                case SelectionOnFocus.CaretAtEnd:
                    CaretIndex = Text.Length;
                    break;
                case SelectionOnFocus.SelectAll:
                    SelectAll();
                    break;
            }

            if (WatermarkBehavior == WatermarkBehavior.HideOnFocus) IsWatermarkVisible = false;
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (WatermarkBehavior == WatermarkBehavior.HideOnFocus) IsWatermarkVisible = true;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (WatermarkBehavior != WatermarkBehavior.HideOnTextEntered) return;

            if (string.IsNullOrWhiteSpace(Text)) IsWatermarkVisible = true;
            else IsWatermarkVisible = false;
        }
    }
}