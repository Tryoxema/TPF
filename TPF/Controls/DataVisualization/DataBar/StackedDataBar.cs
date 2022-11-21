using System;
using System.Linq;
using System.Windows;
using System.Collections;
using System.Collections.Specialized;
using TPF.Controls.Specialized.DataBar;
using TPF.Collections;
using TPF.Internal;

namespace TPF.Controls
{
    public class StackedDataBar : DataBarBase
    {
        static StackedDataBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StackedDataBar), new FrameworkPropertyMetadata(typeof(StackedDataBar)));
        }

        public StackedDataBar()
        {
            // Hiermit wird nur verhindert das es eine NullReferenceException gibt wenn jemand in die BarBrushes-Collection direkt einen Wert einfügt
            SetCurrentValue(BarBrushesProperty, new BrushCollection());
            SetCurrentValue(BarBorderBrushesProperty, new BrushCollection());
        }

        #region ItemsSource DependencyProperty
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
            typeof(IEnumerable),
            typeof(StackedDataBar),
            new PropertyMetadata(null, ItemsSourcePropertyChanged));

        private static void ItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StackedDataBar)sender;

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
            typeof(StackedDataBar),
            new PropertyMetadata(null, ValuePathPropertyChanged));

        private static void ValuePathPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StackedDataBar)sender;

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
            typeof(StackedDataBar),
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
            typeof(StackedDataBar),
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
            typeof(StackedDataBar),
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

        #region AppliedOverflowTemplate Readonly DependencyProperty
        internal static readonly DependencyPropertyKey AppliedOverflowTemplatePropertyKey = DependencyProperty.RegisterReadOnly("AppliedOverflowTemplate",
            typeof(DataTemplate),
            typeof(StackedDataBar),
            new PropertyMetadata(null));

        public static readonly DependencyProperty AppliedOverflowTemplateProperty = AppliedOverflowTemplatePropertyKey.DependencyProperty;

        public DataTemplate AppliedOverflowTemplate
        {
            get { return (DataTemplate)GetValue(AppliedOverflowTemplateProperty); }
            private set { SetValue(AppliedOverflowTemplatePropertyKey, value); }
        }
        #endregion

        #region AppliedUnderflowTemplate Readonly DependencyProperty
        internal static readonly DependencyPropertyKey AppliedUnderflowTemplatePropertyKey = DependencyProperty.RegisterReadOnly("AppliedUnderflowTemplate",
            typeof(DataTemplate),
            typeof(StackedDataBar),
            new PropertyMetadata(null));

        public static readonly DependencyProperty AppliedUnderflowTemplateProperty = AppliedUnderflowTemplatePropertyKey.DependencyProperty;

        public DataTemplate AppliedUnderflowTemplate
        {
            get { return (DataTemplate)GetValue(AppliedUnderflowTemplateProperty); }
            private set { SetValue(AppliedUnderflowTemplatePropertyKey, value); }
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

        protected override void OnMinimumChanged(double oldValue, double newValue)
        {
            CalculateBars();
        }

        protected override void OnMaximumChanged(double oldValue, double newValue)
        {
            CalculateBars();
        }

        protected override void UpdateOutOfRangeTemplates()
        {
            if (Minimum >= Maximum || !Utility.IsANumber(Minimum) || !Utility.IsANumber(Maximum)) return;

            var positiveSum = 0.0;
            var negativeSum = 0.0;
            var isItemsSourceEmpty = DataBarDataItems.Count == 0;

            for (int i = 0; i < DataBarDataItems.Count; i++)
            {
                var item = DataBarDataItems[i];

                var value = item.Value;
                if (double.IsNaN(value)) value = 0d;

                if (value >= 0) positiveSum += value;
                else negativeSum += value;
            }

            if (negativeSum < Minimum && !isItemsSourceEmpty) AppliedUnderflowTemplate = UnderflowTemplate;
            else AppliedUnderflowTemplate = null;

            if (Maximum < positiveSum && !isItemsSourceEmpty) AppliedOverflowTemplate = OverflowTemplate;
            else AppliedOverflowTemplate = null;
        }

        private void CalculateBars()
        {
            if (Minimum >= Maximum || !Utility.IsANumber(Minimum) || !Utility.IsANumber(Maximum))
            {
                for (int i = 0; i < DataBarDataItems.Count; i++)
                {
                    var item = DataBarDataItems[i];

                    item.Start = 0;
                    item.End = 0;
                }

                return;
            }

            var positive = 0.0;
            var negative = 0.0;

            for (int i = 0; i < DataBarDataItems.Count; i++)
            {
                var item = DataBarDataItems[i];

                var value = item.Value;

                if (!Utility.IsANumber(value)) value = 0;

                if (value >= 0)
                {
                    item.Start = Utility.CoerceValue(Utility.NormalizeValue(positive, Minimum, Maximum), 0, 1);
                    positive += value;
                    item.End = Utility.CoerceValue(Utility.NormalizeValue(positive, Minimum, Maximum), 0, 1);
                }
                else
                {
                    item.Start = Utility.CoerceValue(Utility.NormalizeValue(negative, Minimum, Maximum), 0, 1);
                    negative += value;
                    item.End = Utility.CoerceValue(Utility.NormalizeValue(negative, Minimum, Maximum), 0, 1);
                }
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