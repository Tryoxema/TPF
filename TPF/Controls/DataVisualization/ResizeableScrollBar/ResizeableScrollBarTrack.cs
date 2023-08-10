using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace TPF.Controls
{
    public class ResizeableScrollBarTrack : FrameworkElement
    {
        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(ResizeableScrollBarTrack),
            new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region Minimum DependencyProperty
        public static readonly DependencyProperty MinimumProperty = ResizeableScrollBar.MinimumProperty.AddOwner(typeof(ResizeableScrollBarTrack),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        #endregion

        #region Maximum DependencyProperty
        public static readonly DependencyProperty MaximumProperty = ResizeableScrollBar.MaximumProperty.AddOwner(typeof(ResizeableScrollBarTrack),
            new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        #endregion

        #region RangeStart DependencyProperty
        public static readonly DependencyProperty RangeStartProperty = ResizeableScrollBar.RangeStartProperty.AddOwner(typeof(ResizeableScrollBarTrack),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double RangeStart
        {
            get { return (double)GetValue(RangeStartProperty); }
            set { SetValue(RangeStartProperty, value); }
        }
        #endregion

        #region RangeEnd DependencyProperty
        public static readonly DependencyProperty RangeEndProperty = ResizeableScrollBar.RangeEndProperty.AddOwner(typeof(ResizeableScrollBarTrack),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double RangeEnd
        {
            get { return (double)GetValue(RangeEndProperty); }
            set { SetValue(RangeEndProperty, value); }
        }
        #endregion

        private ResizeableScrollBar _scrollBar;

        RepeatButton _decreaseRepeatButton;
        public RepeatButton DecreaseRepeatButton
        {
            get { return _decreaseRepeatButton; }
            set
            {
                if (_increaseRepeatButton == value) throw new NotSupportedException("Increase and Decrease can't be the same Button.");
                UpdateComponent(_decreaseRepeatButton, value);
                _decreaseRepeatButton = value;
            }
        }

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

        RepeatButton _increaseRepeatButton;
        public RepeatButton IncreaseRepeatButton
        {
            get { return _increaseRepeatButton; }
            set
            {
                if (_decreaseRepeatButton == value) throw new NotSupportedException("Increase and Decrease can't be the same Button.");
                UpdateComponent(_increaseRepeatButton, value);
                _increaseRepeatButton = value;
            }
        }

        private double Density { get; set; }

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

            _scrollBar = this.ParentOfType<ResizeableScrollBar>();

            BindTrackToScrollBar();
        }

        private void BindTrackToScrollBar()
        {
            if (_scrollBar == null) return;

            SetBinding(MinimumProperty, new Binding("Minimum") { Source = _scrollBar });
            SetBinding(MaximumProperty, new Binding("Maximum") { Source = _scrollBar });
            SetBinding(RangeStartProperty, new Binding("RangeStart") { Source = _scrollBar });
            SetBinding(RangeEndProperty, new Binding("RangeEnd") { Source = _scrollBar });
            SetBinding(OrientationProperty, new Binding("Orientation") { Source = _scrollBar });
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var width = 0d;
            var height = 0d;

            var isVertical = Orientation == Orientation.Vertical;

            if (_startThumb != null)
            {
                _startThumb.Measure(availableSize);
                if (isVertical)
                {
                    height += _startThumb.DesiredSize.Height;
                    width = Math.Max(width, _startThumb.DesiredSize.Width);
                }
                else
                {
                    width += _startThumb.DesiredSize.Width;
                    height = Math.Max(height, _startThumb.DesiredSize.Height);
                }
            }

            if (_middleThumb != null)
            {
                _middleThumb.Measure(availableSize);
                if (isVertical)
                {
                    height += _middleThumb.DesiredSize.Height;
                    width = Math.Max(width, _middleThumb.DesiredSize.Width);
                }
                else
                {
                    width += _middleThumb.DesiredSize.Width;
                    height = Math.Max(height, _middleThumb.DesiredSize.Height);
                }
            }

            if (_endThumb != null)
            {
                _endThumb.Measure(availableSize);
                if (isVertical)
                {
                    height += _endThumb.DesiredSize.Height;
                    width = Math.Max(width, _endThumb.DesiredSize.Width);
                }
                else
                {
                    width += _endThumb.DesiredSize.Width;
                    height = Math.Max(height, _endThumb.DesiredSize.Height);
                }
            }

            var desiredSize = new Size(width, height);

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double trackLength, decreaseButtonLength, startThumbLength, middleThumbLength, endThumbLength, increaseButtonLength;

            var isVertical = Orientation == Orientation.Vertical;

            var range = Math.Max(0.0, Maximum - Minimum);
            var offsetStart = Math.Min(range, RangeStart - Minimum);
            var offsetEnd = Math.Min(range, RangeEnd - Minimum);

            if (isVertical)
            {
                trackLength = finalSize.Height;
                startThumbLength = _startThumb == null ? 0 : _startThumb.DesiredSize.Height;
                endThumbLength = _endThumb == null ? 0 : _endThumb.DesiredSize.Height;
            }
            else
            {
                trackLength = finalSize.Width;
                startThumbLength = _startThumb == null ? 0 : _startThumb.DesiredSize.Width;
                endThumbLength = _endThumb == null ? 0 : _endThumb.DesiredSize.Width;
            }

            CoerceLength(ref startThumbLength, trackLength);
            CoerceLength(ref endThumbLength, trackLength);

            var remainingTrackLength = trackLength - startThumbLength - endThumbLength;

            decreaseButtonLength = remainingTrackLength * offsetStart / range;
            CoerceLength(ref decreaseButtonLength, remainingTrackLength);

            middleThumbLength = remainingTrackLength * offsetEnd / range - decreaseButtonLength;
            CoerceLength(ref middleThumbLength, trackLength);

            var fullThumbLength = startThumbLength + middleThumbLength + endThumbLength;

            increaseButtonLength = remainingTrackLength - decreaseButtonLength - middleThumbLength;
            CoerceLength(ref increaseButtonLength, remainingTrackLength);

            Density = range / remainingTrackLength;

            var offsetPoint = new Point();
            var pieceSize = finalSize;

            if (isVertical)
            {
                CoerceLength(ref decreaseButtonLength, finalSize.Height);
                CoerceLength(ref increaseButtonLength, finalSize.Height);
                CoerceLength(ref fullThumbLength, finalSize.Height);

                offsetPoint.Y = 0.0;
                pieceSize.Height = decreaseButtonLength;

                if (DecreaseRepeatButton != null) DecreaseRepeatButton.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.Y = decreaseButtonLength + fullThumbLength;
                pieceSize.Height = increaseButtonLength;

                if (IncreaseRepeatButton != null) IncreaseRepeatButton.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.Y = decreaseButtonLength;
                pieceSize.Height = startThumbLength;
                if (_startThumb != null) _startThumb.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.Y = decreaseButtonLength + startThumbLength;
                pieceSize.Height = middleThumbLength;
                if (_middleThumb != null) _middleThumb.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.Y = decreaseButtonLength + startThumbLength + middleThumbLength;
                pieceSize.Height = endThumbLength;
                if (_endThumb != null) _endThumb.Arrange(new Rect(offsetPoint, pieceSize));
            }
            else
            {
                CoerceLength(ref decreaseButtonLength, finalSize.Width);
                CoerceLength(ref increaseButtonLength, finalSize.Width);
                CoerceLength(ref fullThumbLength, finalSize.Width);

                offsetPoint.X = 0.0;
                pieceSize.Width = decreaseButtonLength;

                if (DecreaseRepeatButton != null) DecreaseRepeatButton.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.X = decreaseButtonLength + fullThumbLength;
                pieceSize.Width = increaseButtonLength;

                if (IncreaseRepeatButton != null) IncreaseRepeatButton.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.X = decreaseButtonLength;
                pieceSize.Width = startThumbLength;
                if (_startThumb != null) _startThumb.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.X = decreaseButtonLength + startThumbLength;
                pieceSize.Width = middleThumbLength;
                if (_middleThumb != null) _middleThumb.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.X = decreaseButtonLength + startThumbLength + middleThumbLength;
                pieceSize.Width = endThumbLength;
                if (_endThumb != null) _endThumb.Arrange(new Rect(offsetPoint, pieceSize));
            }

            return finalSize;
        }

        private static void CoerceLength(ref double componentLength, double trackLength)
        {
            if (componentLength < 0) componentLength = 0.0;
            else if (componentLength > trackLength || double.IsNaN(componentLength)) componentLength = trackLength;
        }

        public virtual double ValueFromDistance(double horizontal, double vertical)
        {
            if (Orientation == Orientation.Horizontal)
            {
                return horizontal * Density;
            }
            else
            {
                return vertical * Density;
            }
        }

        private void HookupStartThumb(Thumb thumb)
        {
            thumb.DragDelta += StartThumb_DragDelta;
        }

        private void HookupMiddleThumb(Thumb thumb)
        {
            thumb.DragDelta += MiddleThumb_DragDelta;
        }

        private void HookupEndThumb(Thumb thumb)
        {
            thumb.DragDelta += EndThumb_DragDelta;
        }

        private void UnhookupStartThumb(Thumb thumb)
        {
            thumb.DragDelta -= StartThumb_DragDelta;
        }

        private void UnhookupMiddleThumb(Thumb thumb)
        {
            thumb.DragDelta -= MiddleThumb_DragDelta;
        }

        private void UnhookupEndThumb(Thumb thumb)
        {
            thumb.DragDelta -= EndThumb_DragDelta;
        }

        private void StartThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_scrollBar == null) return;
            
            var delta = ValueFromDistance(e.HorizontalChange, e.VerticalChange);

            _scrollBar.MoveRangeStart(delta);
        }

        private void MiddleThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_scrollBar == null) return;

            var delta = ValueFromDistance(e.HorizontalChange, e.VerticalChange);

            _scrollBar.MoveRange(delta);
        }

        private void EndThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_scrollBar == null) return;

            var delta = ValueFromDistance(e.HorizontalChange, e.VerticalChange);

            _scrollBar.MoveRangeEnd(delta);
        }
    }
}