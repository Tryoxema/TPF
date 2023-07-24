using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TPF.Internal;

namespace TPF.Controls
{
    public class SliderLabelsControl : Control
    {
        static SliderLabelsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SliderLabelsControl), new FrameworkPropertyMetadata(typeof(SliderLabelsControl)));
        }

        #region Ticks DependencyProperty
        public static readonly DependencyProperty TicksProperty = DependencyProperty.Register("Ticks",
            typeof(List<SliderTick>),
            typeof(SliderLabelsControl),
            new PropertyMetadata(null, TicksPropertyChanged));

        private static void TicksPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SliderLabelsControl)sender;

            instance.OnTicksChanged();
        }

        public List<SliderTick> Ticks
        {
            get { return (List<SliderTick>)GetValue(TicksProperty); }
            set { SetValue(TicksProperty, value); }
        }
        #endregion

        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(SliderLabelsControl),
            new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region IsDirectionReversed DependencyProperty
        public static readonly DependencyProperty IsDirectionReversedProperty = Slider.IsDirectionReversedProperty.AddOwner(typeof(SliderLabelsControl),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool IsDirectionReversed
        {
            get { return (bool)GetValue(IsDirectionReversedProperty); }
            set { SetValue(IsDirectionReversedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        private Panel _panel;
        internal Panel Panel
        {
            get { return _panel ?? (_panel = new SliderLabelsPanel(this)); }
        }

        internal UIElementCollection Children
        {
            get { return Panel.Children; }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var root = GetTemplateChild("PART_Root") as Border;

            root.Child = Panel;

            var slider = this.ParentOfType<Slider>();

            if (slider == null) return;

            SetBinding(TicksProperty, new Binding("Ticks") { Source = slider });
            SetBinding(OrientationProperty, new Binding("Orientation") { Source = slider });
            SetBinding(IsDirectionReversedProperty, new Binding("IsDirectionReversed") { Source = slider });
        }

        private void OnTicksChanged()
        {
            Children.Clear();

            var ticks = Ticks;

            if (ticks == null || ticks.Count == 0) return;

            for (int i = 0; i < ticks.Count; i++)
            {
                var tick = ticks[i];

                // Es werden nur Labels für MajorTicks generiert
                if (!tick.IsMajorTick || string.IsNullOrWhiteSpace(tick.LabelText)) continue;

                var textBlock = new TextBlock()
                {
                    Text = tick.LabelText,
                    DataContext = tick
                };

                Children.Add(textBlock);
            }
        }
    }
}