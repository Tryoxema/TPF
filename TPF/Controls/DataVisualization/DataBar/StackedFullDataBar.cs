using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using TPF.Controls.Specialized.DataBar;
using TPF.Collections;
using TPF.Internal;

namespace TPF.Controls
{
    public class StackedFullDataBar : Control
    {
        static StackedFullDataBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StackedFullDataBar), new FrameworkPropertyMetadata(typeof(StackedFullDataBar)));
        }

        public StackedFullDataBar()
        {
            // Hiermit wird nur verhindert das es eine NullReferenceException gibt wenn jemand in die BarBrushes-Collection direkt einen Wert einfügt
            SetCurrentValue(BarBrushesProperty, new BrushCollection());
            SetCurrentValue(BarBorderBrushesProperty, new BrushCollection());
        }

        #region BarHeightFactor DependencyProperty
        public static readonly DependencyProperty BarHeightFactorProperty = DependencyProperty.Register("BarHeightFactor",
            typeof(double),
            typeof(StackedFullDataBar),
            new PropertyMetadata(0.6, null, ConstrainBarHeightFactor));

        private static object ConstrainBarHeightFactor(DependencyObject d, object baseValue)
        {
            var doubleValue = (double)baseValue;

            if (doubleValue < 0) doubleValue = 0;
            else if (doubleValue > 1) doubleValue = 1;

            return doubleValue;
        }

        public double BarHeightFactor
        {
            get { return (double)GetValue(BarHeightFactorProperty); }
            set { SetValue(BarHeightFactorProperty, value); }
        }
        #endregion

        #region BarStrokeThickness DependencyProperty
        public static readonly DependencyProperty BarStrokeThicknessProperty = DependencyProperty.Register("BarStrokeThickness",
            typeof(double),
            typeof(StackedFullDataBar),
            new PropertyMetadata(0.0));

        public double BarStrokeThickness
        {
            get { return (double)GetValue(BarStrokeThicknessProperty); }
            set { SetValue(BarStrokeThicknessProperty, value); }
        }
        #endregion

        #region BarStyle DependencyProperty
        public static readonly DependencyProperty BarStyleProperty = DependencyProperty.Register("BarStyle",
            typeof(Style),
            typeof(StackedFullDataBar),
            new PropertyMetadata(null));

        public Style BarStyle
        {
            get { return (Style)GetValue(BarStyleProperty); }
            set { SetValue(BarStyleProperty, value); }
        }
        #endregion

        #region ItemsSource DependencyProperty
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
            typeof(IEnumerable),
            typeof(StackedFullDataBar),
            new PropertyMetadata(null, ItemsSourcePropertyChanged));

        private static void ItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StackedFullDataBar)sender;

            instance.OnItemsSourceChanged(e);
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        #endregion

        #region ValuePath DependencyProperty
        public static readonly DependencyProperty ValuePathProperty = DependencyProperty.Register("ValuePath",
            typeof(string),
            typeof(StackedFullDataBar),
            new PropertyMetadata(null, ValuePathPropertyChanged));

        private static void ValuePathPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StackedFullDataBar)sender;

            instance.OnValuePathChanged();
        }

        public string ValuePath
        {
            get { return (string)GetValue(ValuePathProperty); }
            set { SetValue(ValuePathProperty, value); }
        }
        #endregion

        #region ToolTipTemplate DependencyProperty
        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register("ToolTipTemplate",
            typeof(DataTemplate),
            typeof(StackedFullDataBar),
            new PropertyMetadata(null));

        public DataTemplate ToolTipTemplate
        {
            get { return (DataTemplate)GetValue(ToolTipTemplateProperty); }
            set { SetValue(ToolTipTemplateProperty, value); }
        }
        #endregion

        #region BarBrushes DependencyProperty
        public static readonly DependencyProperty BarBrushesProperty = DependencyProperty.Register("BarBrushes",
            typeof(BrushCollection),
            typeof(StackedFullDataBar),
            new PropertyMetadata(null, null, ConstrainBarBrushesValue));

        private static object ConstrainBarBrushesValue(DependencyObject sender, object value)
        {
            return value ?? new BrushCollection();
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
            typeof(StackedFullDataBar),
            new PropertyMetadata(null, null, ConstrainBarBorderBrushesValue));

        private static object ConstrainBarBorderBrushesValue(DependencyObject sender, object value)
        {
            return value ?? new BrushCollection();
        }

        public BrushCollection BarBorderBrushes
        {
            get { return (BrushCollection)GetValue(BarBorderBrushesProperty); }
            set { SetValue(BarBorderBrushesProperty, value); }
        }
        #endregion

        private StackedDataBarItemsPresenter _presenter;

        private RangeObservableCollection<DataBarDataItem> _dataBarDataItems;
        internal RangeObservableCollection<DataBarDataItem> DataBarDataItems
        {
            get { return _dataBarDataItems ?? (_dataBarDataItems = new RangeObservableCollection<DataBarDataItem>() { ResetOnChange = false }); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _presenter = GetTemplateChild("PART_ItemsPresenter") as StackedDataBarItemsPresenter;

            if (_presenter != null) _presenter.ItemsSource = DataBarDataItems;

            CalculateBars();
        }

        private void CalculateBars()
        {
            var totalValue = 0.0;

            for (int i = 0; i < DataBarDataItems.Count; i++)
            {
                var item = DataBarDataItems[i];

                var value = item.Value;

                if (!Utility.IsANumber(value)) value = 0;

                totalValue += Math.Abs(value);
            }

            if (totalValue == 0) totalValue = 1;

            var current = 0.0;

            for (int i = 0; i < DataBarDataItems.Count; i++)
            {
                var item = DataBarDataItems[i];

                var value = item.Value;

                if (!Utility.IsANumber(value)) value = 0;

                item.Start = Utility.CoerceValue(Utility.NormalizeValue(current, 0, totalValue), 0, 1);
                current += Math.Abs(value);
                item.End = Utility.CoerceValue(Utility.NormalizeValue(current, 0, totalValue), 0, 1);
            }
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

            DataBarDataItems.Clear();
            AddDataBarDataItems(ItemsSource);
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    AddDataBarDataItems(e.NewItems);
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    RemoveDataBarDataItems(e.OldItems);
                    break;
                }
                case NotifyCollectionChangedAction.Replace:
                {
                    RemoveDataBarDataItems(e.OldItems);
                    AddDataBarDataItems(e.NewItems);
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    DataBarDataItems.Clear();
                    AddDataBarDataItems(e.NewItems);
                    break;
                }
            }
        }

        private void AddDataBarDataItems(IEnumerable items)
        {
            if (items == null) return;

            foreach (var item in items)
            {
                var dataItem = new DataBarDataItem()
                {
                    DataItem = item,
                    ValuePath = ValuePath
                };

                dataItem.PropertyChanged += DataItem_PropertyChanged;

                DataBarDataItems.Add(dataItem);
            }
        }

        private void RemoveDataBarDataItems(IEnumerable items)
        {
            if (items == null) return;

            foreach (var item in items)
            {
                var dataItem = DataBarDataItems.FirstOrDefault(x => x.DataItem == item);

                if (dataItem != null)
                {
                    dataItem.PropertyChanged -= DataItem_PropertyChanged;
                    DataBarDataItems.Remove(dataItem);
                }
            }
        }

        private void DataItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                CalculateBars();
            }
        }

        private void OnValuePathChanged()
        {
            for (int i = 0; i < DataBarDataItems.Count; i++)
            {
                var item = DataBarDataItems[i];

                item.ValuePath = ValuePath;
            }
        }
    }
}