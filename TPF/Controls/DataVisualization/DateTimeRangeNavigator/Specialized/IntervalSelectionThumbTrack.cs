using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class IntervalSelectionThumbTrack : FrameworkElement
    {
        private Controls.DateTimeRangeNavigator _rangeNavigator;
        private bool _isDragging;
        private DateTime? _dragStartDate;
        private DateTime? _dragEndDate;

        Thumb _startThumb;
        public Thumb StartThumb
        {
            get { return _startThumb; }
            set
            {
                if (_startThumb != null) UnhookupStartThumb(_startThumb);
                if (_middleThumb == value || _endThumb == value) throw new NotSupportedException("One Thumb can only be in one place at a time.");
                UpdateComponent(_startThumb, value);
                _startThumb = value;
                if (_startThumb != null) HookupStartThumb(_startThumb);
            }
        }

        Thumb _middleThumb;
        public Thumb MiddleThumb
        {
            get { return _middleThumb; }
            set
            {
                if (_middleThumb != null) UnhookupMiddleThumb(_middleThumb);
                if (_startThumb == value || _endThumb == value) throw new NotSupportedException("One Thumb can only be in one place at a time.");
                UpdateComponent(_middleThumb, value);
                _middleThumb = value;
                if (_middleThumb != null) HookupMiddleThumb(_middleThumb);
            }
        }

        Thumb _endThumb;
        public Thumb EndThumb
        {
            get { return _endThumb; }
            set
            {
                if (_endThumb != null) UnhookupEndThumb(_endThumb);
                if (_startThumb == value || _middleThumb == value) throw new NotSupportedException("One Thumb can only be in one place at a time.");
                UpdateComponent(_endThumb, value);
                _endThumb = value;
                if (_endThumb != null) HookupEndThumb(_endThumb);
            }
        }

        Border _preceedingOverlay;
        public Border PreceedingOverlay
        {
            get { return _preceedingOverlay; }
            set
            {
                if (_followingOverlay == value) throw new NotSupportedException("One Border can only be in one place at a time.");
                UpdateComponent(_preceedingOverlay, value);
                _preceedingOverlay = value;
            }
        }

        Border _followingOverlay;
        public Border FollowingOverlay
        {
            get { return _followingOverlay; }
            set
            {
                if (_preceedingOverlay == value) throw new NotSupportedException("One Border can only be in one place at a time.");
                UpdateComponent(_followingOverlay, value);
                _followingOverlay = value;
            }
        }

        #region VisualChildren Verwaltung
        private Visual[] _children;

        private void UpdateComponent(Visual oldValue, Visual newValue)
        {
            if (oldValue != newValue)
            {
                if (_children == null) _children = new Visual[5];
                if (oldValue != null) RemoveVisualChild(oldValue);

                int i = 0;
                while (i < 5)
                {
                    // Array ist nicht gefüllt, break
                    if (_children[i] == null) break;

                    // Alten Wert gefunden
                    if (_children[i] == oldValue)
                    {
                        // Alle Werte um eins nach oben verschieben, damit das neue an letzter Stelle landet
                        while (i < 4 && _children[i + 1] != null)
                        {
                            _children[i] = _children[i + 1];
                            i++;
                        }
                    }
                    else i++;
                }

                _children[i] = newValue;

                AddVisualChild(newValue);

                InvalidateMeasure();
                InvalidateArrange();
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (_children == null || _children[index] == null) throw new ArgumentOutOfRangeException(nameof(index));
            return _children[index];
        }

        protected override int VisualChildrenCount
        {
            get
            {
                if (_children == null) return 0;

                for (var i = 0; i < _children.Length; i++)
                {
                    if (_children[i] == null) return i;
                }

                return _children.Length;
            }
        }
        #endregion

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _rangeNavigator = this.ParentOfType<Controls.DateTimeRangeNavigator>();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var width = 0d;
            var height = 0d;

            if (_startThumb != null)
            {
                _startThumb.Measure(availableSize);
                width += _startThumb.DesiredSize.Width;
                height = Math.Max(height, _startThumb.DesiredSize.Height);
            }

            if (_middleThumb != null)
            {
                _middleThumb.Measure(availableSize);
                width += _middleThumb.DesiredSize.Width;
                height = Math.Max(height, _middleThumb.DesiredSize.Height);
            }

            if (_endThumb != null)
            {
                _endThumb.Measure(availableSize);
                width += _endThumb.DesiredSize.Width;
                height = Math.Max(height, _endThumb.DesiredSize.Height);
            }

            var desiredSize = new Size(width, height);

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_rangeNavigator == null) return finalSize;

            var startDate = _dragStartDate.GetValueOrDefault(_rangeNavigator.SelectedStart);
            var endDate = _dragEndDate.GetValueOrDefault(_rangeNavigator.SelectedEnd);
            var duration = endDate - startDate;
            var pixelsPerTick = _rangeNavigator.IntervalManager.PixelsPerTick;

            var ticks = (startDate - _rangeNavigator.VisibleStart).Ticks;
            var x = ticks * pixelsPerTick;
            var width = duration.Ticks * pixelsPerTick;

            var rect = new Rect(x, 0, width, finalSize.Height);

            if (_middleThumb != null)
            {
                _middleThumb.Arrange(rect);
            }

            if (_startThumb != null)
            {
                var thumbWidth = _startThumb.DesiredSize.Width;
                _startThumb.Arrange(new Rect(rect.X - (thumbWidth / 2), rect.Y, thumbWidth, rect.Height));
            }

            if (_endThumb != null)
            {
                var thumbWidth = _endThumb.DesiredSize.Width;
                _endThumb.Arrange(new Rect(rect.Right - (thumbWidth / 2), rect.Y, thumbWidth, rect.Height));
            }

            if (_preceedingOverlay != null)
            {
                var overlayWidth = Math.Max(x, 0);
                _preceedingOverlay.Arrange(new Rect(0, 0, overlayWidth, rect.Height));
            }

            if (_followingOverlay != null)
            {
                var left = rect.Right;
                var overlayWidth = Math.Max(finalSize.Width - left, 0);
                _followingOverlay.Arrange(new Rect(left, 0, overlayWidth, rect.Height));
            }

            return finalSize;
        }

        private void HookupStartThumb(Thumb thumb)
        {
            thumb.DragStarted += Thumb_DragStarted;
            thumb.DragDelta += StartThumb_DragDelta;
            thumb.DragCompleted += Thumb_DragCompleted;
        }

        private void HookupMiddleThumb(Thumb thumb)
        {
            thumb.DragStarted += Thumb_DragStarted;
            thumb.DragDelta += MiddleThumb_DragDelta;
            thumb.DragCompleted += Thumb_DragCompleted;
        }

        private void HookupEndThumb(Thumb thumb)
        {
            thumb.DragStarted += Thumb_DragStarted;
            thumb.DragDelta += EndThumb_DragDelta;
            thumb.DragCompleted += Thumb_DragCompleted;
        }

        private void UnhookupStartThumb(Thumb thumb)
        {
            thumb.DragStarted -= Thumb_DragStarted;
            thumb.DragDelta -= StartThumb_DragDelta;
            thumb.DragCompleted -= Thumb_DragCompleted;
        }

        private void UnhookupMiddleThumb(Thumb thumb)
        {
            thumb.DragStarted -= Thumb_DragStarted;
            thumb.DragDelta -= MiddleThumb_DragDelta;
            thumb.DragCompleted -= Thumb_DragCompleted;
        }

        private void UnhookupEndThumb(Thumb thumb)
        {
            thumb.DragStarted -= Thumb_DragStarted;
            thumb.DragDelta -= EndThumb_DragDelta;
            thumb.DragCompleted -= Thumb_DragCompleted;
        }

        private DateTime SnapToInterval(DateTime value)
        {
            if (_rangeNavigator == null || _rangeNavigator.ItemIntervalPeriods.Count == 0) return value;

            var snappingValues = new List<DateTime>();

            snappingValues.AddRange(_rangeNavigator.ItemIntervalPeriods.Select(x => x.Start));
            snappingValues.AddRange(_rangeNavigator.ItemIntervalPeriods.Select(x => x.End));
            snappingValues.Sort();

            var previous = _rangeNavigator.Start;
            var next = _rangeNavigator.End;

            for (int i = 0; i < snappingValues.Count; i++)
            {
                var snappingValue = snappingValues[i];

                if (value == snappingValue) return value;
                else if (snappingValue < value && snappingValue > previous) previous = snappingValue;
                else if (snappingValue > value && snappingValue < next) next = snappingValue;
            }

            value = (value.Ticks > (previous.Ticks + next.Ticks) * 0.5) ? next : previous;

            return value;
        }

        private TimeSpan GetIntervalDuration()
        {
            if (_rangeNavigator == null || _rangeNavigator.CurrentItemInterval == null) return TimeSpan.Zero;

            return _rangeNavigator.CurrentItemInterval.MinimumIntervalLength;
        }

        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (_rangeNavigator == null) return;

            _isDragging = true;
            _dragStartDate = _rangeNavigator.SelectedStart;
            _dragEndDate = _rangeNavigator.SelectedEnd;
        }

        private void StartThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!_isDragging) return;

            var minimumDate = _rangeNavigator.Start;
            var pixelsPerTick = _rangeNavigator.IntervalManager.PixelsPerTick;
            var ticks = (long)Math.Round(e.HorizontalChange / pixelsPerTick);

            var startDate = _dragStartDate.GetValueOrDefault(_rangeNavigator.SelectedStart);
            var endDate = _dragEndDate.GetValueOrDefault(_rangeNavigator.SelectedEnd);
            var snappedValue = SnapToInterval(startDate.AddTicks(ticks));

            if (endDate - snappedValue > TimeSpan.Zero) startDate = snappedValue;

            if (startDate <= endDate && startDate >= minimumDate) _dragStartDate = startDate;

            InvalidateArrange();
        }

        private void MiddleThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!_isDragging) return;

            var minimumDate = _rangeNavigator.Start;
            var maximumDate = _rangeNavigator.End;
            var pixelsPerTick = _rangeNavigator.IntervalManager.PixelsPerTick;
            var ticks = (long)Math.Round(e.HorizontalChange / pixelsPerTick);

            var startDate = _dragStartDate.GetValueOrDefault(_rangeNavigator.SelectedStart);
            var endDate = _dragEndDate.GetValueOrDefault(_rangeNavigator.SelectedEnd);
            var duration = endDate - startDate;
            var minimumDuration = GetIntervalDuration();
            var snappedStart = SnapToInterval(startDate.AddTicks(ticks));
            var snappedEnd = SnapToInterval(snappedStart.Add(duration < minimumDuration ? minimumDuration : duration));

            startDate = snappedStart;
            endDate = snappedEnd;

            if (startDate >= minimumDate && endDate <= maximumDate)
            {
                _dragStartDate = startDate;
                _dragEndDate = endDate;
            }

            InvalidateArrange();
        }

        private void EndThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!_isDragging) return;

            var maximumDate = _rangeNavigator.End;
            var pixelsPerTick = _rangeNavigator.IntervalManager.PixelsPerTick;
            var ticks = (long)Math.Round(e.HorizontalChange / pixelsPerTick);

            var startDate = _dragStartDate.GetValueOrDefault(_rangeNavigator.SelectedStart);
            var endDate = _dragEndDate.GetValueOrDefault(_rangeNavigator.SelectedEnd);
            var snappedValue = SnapToInterval(endDate.AddTicks(ticks));

            if (snappedValue - startDate > TimeSpan.Zero) endDate = snappedValue;

            if (endDate >= startDate && endDate <= maximumDate) _dragEndDate = endDate;

            InvalidateArrange();
        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (!_isDragging) return;

            if (e.Canceled)
            {
                _isDragging = false;
                _dragStartDate = null;
                _dragEndDate = null;
                InvalidateArrange();
                return;
            }

            var startDate = _dragStartDate.GetValueOrDefault(_rangeNavigator.SelectedStart);
            var endDate = _dragEndDate.GetValueOrDefault(_rangeNavigator.SelectedEnd);

            _isDragging = false;
            _dragStartDate = null;
            _dragEndDate = null;

            _rangeNavigator.SetSelectedInterval(startDate, endDate);
        }
    }
}