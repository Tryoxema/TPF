using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class IntervalLabelsPanel : VirtualizingPanel
    {
        double _pixelsPerTick;
        internal double PixelsPerTick 
        { 
            get { return _pixelsPerTick; }
            private set
            {
                _pixelsPerTick = value;
                if (Owner != null) Owner.IntervalManager.PixelsPerTick = value;
            }
        }

        Controls.DateTimeRangeNavigator _owner;
        private Controls.DateTimeRangeNavigator Owner
        {
            get
            {
                if (_owner == null)
                {
                    _owner = this.ParentOfType<Controls.DateTimeRangeNavigator>();
                    if (_owner != null) _owner.VisibleRangeChanged += Owner_VisibleRangeChanged;
                }

                return _owner;
            }
        }

        IntervalLabelsControl _itemsControl;
        private IntervalLabelsControl ItemsControl
        {
            get
            {
                if (_itemsControl == null) _itemsControl = this.ParentOfType<IntervalLabelsControl>();

                return _itemsControl;
            }
        }

        private static readonly Size InfiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

        private void Owner_VisibleRangeChanged(object sender, RangeChangedEventArgs<DateTime> e)
        {
            InvalidateMeasure();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Owner == null) return base.MeasureOverride(availableSize);
            
            var validSize = EnsureValidSize(availableSize);

            // Wenn wir keinen Platz haben können wir auch nichts machen
            if (validSize.Width == 0d) return validSize;

            PixelsPerTick = CalculatePixelsPerTick(validSize.Width);

            UpdateVirtualChildren();

            var areLabelMeasurementsEnsured = false;

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i] as FrameworkElement;

                var period = (IntervalPeriod)child?.DataContext;

                if (period == null) continue;

                if (!areLabelMeasurementsEnsured)
                {
                    if (ItemsControl != null)
                    {
                        var interval = period.Interval;
                        var labelType = ItemsControl.LabelType;

                        var requiresLabelMeasuring = Owner.IntervalManager.RequiresIntervalMeasuring(interval, labelType);

                        if (requiresLabelMeasuring)
                        {
                            var measurementMatrix = interval.CreateLabelMeasurementsMatrix(interval.StringFormatters);

                            for (int row = 0; row < measurementMatrix.GetLength(0); row++)
                            {
                                var results = new List<double>();

                                for (int column = 0; column < measurementMatrix.GetLength(1); column++)
                                {
                                    period.Label = measurementMatrix[row, column];
                                    child.Measure(new Size(0, 0));
                                    child.Measure(InfiniteSize);
                                    results.Add(child.DesiredSize.Width);
                                }

                                var maxWidth = results.Max();
                                Owner.IntervalManager.SaveLabelMeasurement(interval, labelType, row, maxWidth);
                            }
                        }
                    }
                    areLabelMeasurementsEnsured = true;
                }

                var width = CalculateSize(period.Duration.Ticks);

                child.Measure(new Size(width, availableSize.Height));

                validSize.Height = Math.Max(validSize.Height, child.DesiredSize.Height);
            }

            return validSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Owner == null) return base.ArrangeOverride(finalSize);

            var arrangedSize = base.ArrangeOverride(finalSize);
            PixelsPerTick = CalculatePixelsPerTick(arrangedSize.Width);
            Owner.IntervalManager.UpdateLabels();

            var currentOffset = 0d;

            if (InternalChildren.Count > 0)
            {
                var period = (IntervalPeriod)(InternalChildren[0] as FrameworkElement).DataContext;
                var ticks = (period.Start - Owner.VisibleStart).Ticks;
                var itemSize = CalculateSize(ticks);

                currentOffset = itemSize;
            }

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i] as FrameworkElement;

                var period = (IntervalPeriod)child?.DataContext;

                if (period == null) continue;

                var width = CalculateSize(period.Duration.Ticks);

                child.Arrange(new Rect(currentOffset, 0d, width, arrangedSize.Height));
                currentOffset += width;
            }

            return arrangedSize;
        }

        private static Size EnsureValidSize(Size size)
        {
            var width = double.IsInfinity(size.Width) ? 0d : size.Width;
            var height = double.IsInfinity(size.Height) ? 0d : size.Height;

            return new Size(width, height);
        }

        private double CalculatePixelsPerTick(double size)
        {
            var ticks = Owner.VisibleEnd.Ticks - Owner.VisibleStart.Ticks;

            var pixelsPerTick = size / ticks;

            if (double.IsInfinity(pixelsPerTick) || double.IsNaN(pixelsPerTick)) pixelsPerTick = 0;

            return pixelsPerTick;
        }

        private double CalculateSize(long ticks)
        {
            return ticks * PixelsPerTick;
        }

        private IntervalPeriod[] GetIntervalPeriods()
        {
            var itemsControl = ItemsControl;
            
            if (itemsControl == null || itemsControl.ItemsSource == null) return null;
            else if (itemsControl.ItemsSource is IntervalPeriod[] array) return array;
            else if (itemsControl.ItemsSource is List<IntervalPeriod> list) return list.ToArray();
            else return itemsControl.ItemsSource.Cast<IntervalPeriod>().ToArray();
        }

        private void UpdateVirtualChildren()
        {
            var items = GetIntervalPeriods();

            if (items == null) return;

            int startIndex, endIndex;

            if (items.Length == 1)
            {
                startIndex = 0;
                endIndex = 0;
            }
            else if (!TryGetVisibleRange(out startIndex, out endIndex)) return;
            
            var children = InternalChildren;

            var generator = ItemContainerGenerator;
            var position = generator.GeneratorPositionFromIndex(startIndex);

            var currentIndex = (position.Offset == 0) ? position.Index : position.Index + 1;

            using (generator.StartAt(position, GeneratorDirection.Forward, true))
            {
                for (int i = startIndex; i <= endIndex; i++, currentIndex++)
                {
                    var child = generator.GenerateNext(out var isNewlyRealized) as UIElement;

                    if (isNewlyRealized)
                    {
                        if (currentIndex >= children.Count) AddInternalChild(child);
                        else InsertInternalChild(currentIndex, child);

                        generator.PrepareItemContainer(child);
                    }
                    else if (!children.Contains(child))
                    {
                        InsertInternalChild(currentIndex, child);

                        generator.PrepareItemContainer(child);
                    }
                }
            }

            RecycleContainers(startIndex, endIndex);
        }

        private bool TryGetVisibleRange(out int startIndex, out int endIndex)
        {
            startIndex = -1;
            endIndex = -1;
            var items = GetIntervalPeriods();

            if (items == null) return false;

            startIndex = Array.BinarySearch(items, Owner.VisibleStart);

            if (startIndex < 0) startIndex = ~startIndex;

            if (startIndex >= items.Length) return false;

            endIndex = Array.BinarySearch(items, Owner.VisibleEnd);

            if (endIndex < 0) endIndex = ~endIndex;

            if (endIndex >= items.Length) endIndex = items.Length - 1;

            return true;
        }

        private void RecycleContainers(int startIndex, int endIndex)
        {
            var generator = ItemContainerGenerator as IRecyclingItemContainerGenerator;

            for (int i = InternalChildren.Count - 1; i >= 0; i--)
            {
                var position = new GeneratorPosition(i, 0);
                var itemIndex = generator.IndexFromGeneratorPosition(position);

                if (itemIndex < startIndex || itemIndex > endIndex)
                {
                    RemoveInternalChildRange(i, 1);
                    generator.Recycle(position, 1);
                }
            }
        }
    }
}