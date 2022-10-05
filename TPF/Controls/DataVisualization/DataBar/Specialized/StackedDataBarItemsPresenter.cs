using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace TPF.Controls.Specialized.DataBar
{
    public class StackedDataBarItemsPresenter : Control
    {
        static StackedDataBarItemsPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StackedDataBarItemsPresenter), new FrameworkPropertyMetadata(typeof(StackedDataBarItemsPresenter)));
        }

        #region ItemsSource DependencyProperty
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
            typeof(IList<DataBarDataItem>),
            typeof(StackedDataBarItemsPresenter),
            new PropertyMetadata(null, ItemsSourcePropertyChanged));

        private static void ItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StackedDataBarItemsPresenter)sender;

            instance.OnItemsSourceChanged(e);
        }

        public IList<DataBarDataItem> ItemsSource
        {
            get { return (IList<DataBarDataItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        #endregion

        #region BarHeightFactor DependencyProperty
        public static readonly DependencyProperty BarHeightFactorProperty = DependencyProperty.Register("BarHeightFactor",
            typeof(double),
            typeof(StackedDataBarItemsPresenter),
            new PropertyMetadata(1.0));

        public double BarHeightFactor
        {
            get { return (double)GetValue(BarHeightFactorProperty); }
            set { SetValue(BarHeightFactorProperty, value); }
        }
        #endregion

        #region BarStyle DependencyProperty
        public static readonly DependencyProperty BarStyleProperty = DependencyProperty.Register("BarStyle",
            typeof(Style),
            typeof(StackedDataBarItemsPresenter),
            new PropertyMetadata(null));

        public Style BarStyle
        {
            get { return (Style)GetValue(BarStyleProperty); }
            set { SetValue(BarStyleProperty, value); }
        }
        #endregion

        #region BarStrokeThickness DependencyProperty
        public static readonly DependencyProperty BarStrokeThicknessProperty = DependencyProperty.Register("BarStrokeThickness",
            typeof(double),
            typeof(StackedDataBarItemsPresenter),
            new PropertyMetadata(0.0));

        public double BarStrokeThickness
        {
            get { return (double)GetValue(BarStrokeThicknessProperty); }
            set { SetValue(BarStrokeThicknessProperty, value); }
        }
        #endregion

        #region BarBrushes DependencyProperty
        public static readonly DependencyProperty BarBrushesProperty = DependencyProperty.Register("BarBrushes",
            typeof(BrushCollection),
            typeof(StackedDataBarItemsPresenter),
            new PropertyMetadata(null, BarBrushesPropertyChanged));

        private static void BarBrushesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StackedDataBarItemsPresenter)sender;

            instance.OnBarBrushesChanged(e);
        }

        public BrushCollection BarBrushes
        {
            get { return (BrushCollection)GetValue(BarBrushesProperty); }
            set { SetValue(BarBrushesProperty, value); }
        }
        #endregion

        #region BarBorderBrushes DependencyProperty
        public static readonly DependencyProperty BarBorderBrushesProperty = DependencyProperty.Register("BarBorderBrushes",
            typeof(BrushCollection),
            typeof(StackedDataBarItemsPresenter),
            new PropertyMetadata(null, BarBorderBrushesPropertyChanged));

        private static void BarBorderBrushesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StackedDataBarItemsPresenter)sender;

            instance.OnBarBorderBrushesChanged(e);
        }

        public BrushCollection BarBorderBrushes
        {
            get { return (BrushCollection)GetValue(BarBorderBrushesProperty); }
            set { SetValue(BarBorderBrushesProperty, value); }
        }
        #endregion

        #region ToolTipTemplate DependencyProperty
        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register("ToolTipTemplate",
            typeof(DataTemplate),
            typeof(StackedDataBarItemsPresenter),
            new PropertyMetadata(null));

        public DataTemplate ToolTipTemplate
        {
            get { return (DataTemplate)GetValue(ToolTipTemplateProperty); }
            set { SetValue(ToolTipTemplateProperty, value); }
        }
        #endregion

        private Panel _panel;
        internal Panel Panel
        {
            get { return _panel ?? (_panel = new SimplePanel()); }
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
        }

        private void OnItemsSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyCollectionChanged oldItems)
            {
                oldItems.CollectionChanged -= ItemsSource_CollectionChanged;
            }

            if (e.NewValue is INotifyCollectionChanged newItems)
            {
                newItems.CollectionChanged += ItemsSource_CollectionChanged;
            }

            Children.Clear();
            AddItems(ItemsSource.ToList());
            UpdateBarBrushes();
            UpdateBarBorderBrushes();
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    AddItems(e.NewItems);
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    RemoveItems(e.OldItems);
                    break;
                }
                case NotifyCollectionChangedAction.Replace:
                {
                    RemoveItems(e.OldItems);
                    AddItems(e.NewItems);
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    Children.Clear();
                    AddItems(e.NewItems);
                    break;
                }
            }

            UpdateBarBrushes();
            UpdateBarBorderBrushes();
        }

        private void AddItems(IList items)
        {
            if (items == null) return;

            foreach (var item in items)
            {
                var container = new StackedDataBarItem
                {
                    DataContext = item
                };

                container.SetBinding(StackedDataBarItem.BarHeightFactorProperty, new Binding(nameof(BarHeightFactor)) { Source = this });
                container.SetBinding(StackedDataBarItem.BarStrokeThicknessProperty, new Binding(nameof(BarStrokeThickness)) { Source = this });
                container.SetBinding(StackedDataBarItem.BarStyleProperty, new Binding(nameof(BarStyle)) { Source = this });
                container.SetBinding(StackedDataBarItem.ToolTipTemplateProperty, new Binding(nameof(ToolTipTemplate)) { Source = this });
                container.SetBinding(StackedDataBarItem.StartProperty, new Binding("Start"));
                container.SetBinding(StackedDataBarItem.EndProperty, new Binding("End"));

                Children.Add(container);
            }
        }

        private void RemoveItems(IList items)
        {
            if (items == null) return;

            foreach (var item in items)
            {
                var visualItem = FindContainerFromDataItem(item);

                Children.Remove(visualItem);
            }
        }

        private StackedDataBarItem FindContainerFromDataItem(object dataItem)
        {
            return Children.Cast<StackedDataBarItem>().Where(child => child.DataContext == dataItem).First();
        }

        private void OnBarBrushesChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is BrushCollection oldBarBrushes)
            {
                oldBarBrushes.CollectionChanged -= BarBrushes_CollectionChanged;
            }

            if (e.NewValue is BrushCollection newBarBrushes)
            {
                newBarBrushes.CollectionChanged += BarBrushes_CollectionChanged;
            }

            UpdateBarBrushes();
        }

        private void BarBrushes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateBarBrushes();
        }

        private void UpdateBarBrushes()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var item = Children[i] as StackedDataBarItem;

                if (item != null) item.Background = GetBarBrushForIndex(i);
            }
        }

        private Brush GetBarBrushForIndex(int itemIndex)
        {
            if (BarBrushes == null || BarBrushes.Count == 0) return null;

            return BarBrushes[itemIndex % BarBrushes.Count];
        }

        private void OnBarBorderBrushesChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is BrushCollection oldBarBorderBrushes)
            {
                oldBarBorderBrushes.CollectionChanged -= BarBorderBrushes_CollectionChanged;
            }

            if (e.NewValue is BrushCollection newBarBorderBrushes)
            {
                newBarBorderBrushes.CollectionChanged += BarBorderBrushes_CollectionChanged;
            }

            UpdateBarBorderBrushes();
        }

        private void BarBorderBrushes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateBarBorderBrushes();
        }

        private void UpdateBarBorderBrushes()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var item = Children[i] as StackedDataBarItem;

                if (item != null) item.BorderBrush = GetBarBorderBrushForIndex(i);
            }
        }

        private Brush GetBarBorderBrushForIndex(int itemIndex)
        {
            if (BarBorderBrushes == null || BarBorderBrushes.Count == 0) return null;

            return BarBorderBrushes[itemIndex % BarBorderBrushes.Count];
        }
    }
}