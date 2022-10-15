using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TPF.Controls.Specialized.DataAxis
{
    public class DataAxisLabelsControl : Control
    {
        static DataAxisLabelsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataAxisLabelsControl), new FrameworkPropertyMetadata(typeof(DataAxisLabelsControl)));
        }

        #region Ticks DependencyProperty
        public static readonly DependencyProperty TicksProperty = DependencyProperty.Register("Ticks",
            typeof(List<DataAxisTick>),
            typeof(DataAxisLabelsControl),
            new PropertyMetadata(null, TicksPropertyChanged));

        private static void TicksPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataAxisLabelsControl)sender;

            instance.OnTicksChanged();
        }

        public List<DataAxisTick> Ticks
        {
            get { return (List<DataAxisTick>)GetValue(TicksProperty); }
            set { SetValue(TicksProperty, value); }
        }
        #endregion

        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(DataAxisLabelsControl),
            new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        private Panel _panel;
        internal Panel Panel
        {
            get { return _panel ?? (_panel = new DataAxisLabelsPanel(this)); }
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

            var dataAxis = this.ParentOfType<Controls.DataAxis>();

            if (dataAxis == null) return;

            SetBinding(TicksProperty, new Binding("Ticks") { Source = dataAxis });
            SetBinding(OrientationProperty, new Binding("Orientation") { Source = dataAxis });
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
                if (!tick.IsMajorTick) continue;

                var textBlock = new TextBlock()
                {
                    DataContext = tick
                };

                textBlock.SetBinding(TextBlock.TextProperty, "Value");

                Children.Add(textBlock);
            }
        }
    }
}