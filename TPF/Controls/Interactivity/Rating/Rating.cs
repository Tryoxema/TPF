using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public class Rating : ItemsControl
    {
        static Rating()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Rating), new FrameworkPropertyMetadata(typeof(Rating)));
        }

        #region ValueChanged RoutedEvent
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<double?>),
            typeof(Rating));

        public event RoutedPropertyChangedEventHandler<double?> ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }
        #endregion

        #region Value DependencyProperty
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(double),
            typeof(Rating),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValuePropertyChanged));

        static void ValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Rating)sender;

            var eventArgs = new RoutedPropertyChangedEventArgs<double>((double)e.OldValue, (double)e.NewValue) { RoutedEvent = ValueChangedEvent };

            instance.RaiseEvent(eventArgs);

            instance.UpdateItemValues();
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region Precision DependencyProperty
        public static readonly DependencyProperty PrecisionProperty = DependencyProperty.Register("Precision",
            typeof(RatingPrecision),
            typeof(Rating),
            new PropertyMetadata(RatingPrecision.Full, OnPrecisionChanged));

        private static void OnPrecisionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Rating)sender;

            instance.OnPrecisionChanged((RatingPrecision)e.NewValue);
        }

        public RatingPrecision Precision
        {
            get { return (RatingPrecision)GetValue(PrecisionProperty); }
            set { SetValue(PrecisionProperty, value); }
        }
        #endregion

        #region ItemsToGenerateCount DependencyProperty
        public static readonly DependencyProperty ItemsToGenerateCountProperty = DependencyProperty.Register("ItemsToGenerateCount",
            typeof(int),
            typeof(Rating),
            new PropertyMetadata(5, ItemsToGenerateCountPropertyChanged, ConstrainItemsToGenerateCount));

        private static void ItemsToGenerateCountPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Rating)sender;

            // Items neu generieren
            instance.GenerateItems();
        }

        internal static object ConstrainItemsToGenerateCount(DependencyObject sender, object value)
        {
            var intValue = (int)value;

            // Werte unter 0 ergeben keinen Sinn
            if (intValue < 0) intValue = 0;

            return intValue;
        }

        public int ItemsToGenerateCount
        {
            get { return (int)GetValue(ItemsToGenerateCountProperty); }
            set { SetValue(ItemsToGenerateCountProperty, value); }
        }
        #endregion

        #region Geometry DependencyProperty
        public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register("Geometry",
            typeof(Geometry),
            typeof(Rating),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, GeometryPropertyChanged));

        static void GeometryPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Rating)sender;

            instance.UpdateGeometry();
        }

        public Geometry Geometry
        {
            get { return (Geometry)GetValue(GeometryProperty); }
            set { SetValue(GeometryProperty, value); }
        }
        #endregion

        #region IsReadOnly DependencyProperty
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly",
            typeof(bool),
            typeof(Rating),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (Items.Count == 0) GenerateItems();
        }

        private void GenerateItems()
        {
            // Existierende Items rauswerfen
            Items.Clear();

            for (int i = 1; i <= ItemsToGenerateCount; i++)
            {
                var item = new RatingItem
                {
                    ParentRating = this,
                    Index = i,
                    Geometry = Geometry,
                    IsReadOnly = IsReadOnly
                };

                Items.Add(item);
            }
            UpdateItemValues();
        }

        private void UpdateItemValues()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (!(Items[i] is RatingItem item)) return;

                var value = Value - i;

                if (value > 1) value = 1;
                else if (value < 0) value = 0;

                item.VisibleValue = value;
                item.Value = value;
            }
        }

        private void UpdateGeometry()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (!(Items[i] is RatingItem item)) return;

                item.Geometry = Geometry;
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new RatingItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is RatingItem;
        }

        protected virtual void OnPrecisionChanged(RatingPrecision newValue)
        {
            // Wenn die Präzision auf Exakt geändert wurde, muss nichts angepasst werden
            if (newValue == RatingPrecision.Exact) return;

            for (int i = 0; i < Items.Count; i++)
            {
                if (!(Items[i] is RatingItem item)) return;

                switch (newValue)
                {
                    case RatingPrecision.Full:
                    {
                        if (item.Value > 0) item.Value = 1;
                        break;
                    }
                    case RatingPrecision.Half:
                    {
                        if (item.Value > 0.5) item.Value = 1;
                        else if (item.Value > 0) item.Value = 0.5;
                        break;
                    }
                }
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (IsReadOnly) return;

            UpdateItemValues();
        }

        internal int GetIndex(RatingItem item)
        {
            // Der CollectionIndex fängt bei 0 an also rechnen wir +1, da dass für manche Funktionen benötigt wird
            return Items.IndexOf(item) + 1;
        }

        // Aktualisiert den Sichtbaren Bereich aller Items, wenn MouseEnter auf einem Item ausgelöst wurde
        internal void SelectionChanged(RatingItem selectedItem)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (!(Items[i] is RatingItem item)) return;

                if (item.Index < selectedItem.Index) item.VisibleValue = 1.0;
                else if (item.Index > selectedItem.Index) item.VisibleValue = 0.0;
            }
        }

        internal void SetValue(RatingItem item)
        {
            // Der Wert ist der Index - 1 + der Wert des Items, was angeklickt wurde
            Value = item.Index - 1.0 + item.Value;
        }
    }
}