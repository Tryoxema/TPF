using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public class SliderTrack : FrameworkElement
    {
        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(SliderTrack),
            new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region ThumbMode DependencyProperty
        public static readonly DependencyProperty ThumbModeProperty = Slider.ThumbModeProperty.AddOwner(typeof(SliderTrack),
            new FrameworkPropertyMetadata(SliderThumbMode.Single, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public SliderThumbMode ThumbMode
        {
            get { return (SliderThumbMode)GetValue(ThumbModeProperty); }
            set { SetValue(ThumbModeProperty, value); }
        }
        #endregion

        #region Minimum DependencyProperty
        public static readonly DependencyProperty MinimumProperty = RangeBase.MinimumProperty.AddOwner(typeof(SliderTrack),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        #endregion

        #region Maximum DependencyProperty
        public static readonly DependencyProperty MaximumProperty = RangeBase.MaximumProperty.AddOwner(typeof(SliderTrack),
            new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        #endregion

        #region Value DependencyProperty
        public static readonly DependencyProperty ValueProperty = RangeBase.ValueProperty.AddOwner(typeof(SliderTrack),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region RangeStart DependencyProperty
        public static readonly DependencyProperty RangeStartProperty = Slider.RangeStartProperty.AddOwner(typeof(SliderTrack),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double RangeStart
        {
            get { return (double)GetValue(RangeStartProperty); }
            set { SetValue(RangeStartProperty, value); }
        }
        #endregion

        #region RangeEnd DependencyProperty
        public static readonly DependencyProperty RangeEndProperty = Slider.RangeEndProperty.AddOwner(typeof(SliderTrack),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double RangeEnd
        {
            get { return (double)GetValue(RangeEndProperty); }
            set { SetValue(RangeEndProperty, value); }
        }
        #endregion

        #region IsDirectionReversed DependencyProperty
        public static readonly DependencyProperty IsDirectionReversedProperty = Slider.IsDirectionReversedProperty.AddOwner(typeof(SliderTrack),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsArrange));

        public bool IsDirectionReversed
        {
            get { return (bool)GetValue(IsDirectionReversedProperty); }
            set { SetValue(IsDirectionReversedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

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

        Border _valueIndicator;
        public Border ValueIndicator
        {
            get { return _valueIndicator; }
            set
            {
                UpdateComponent(_valueIndicator, value);
                _valueIndicator = value;
            }
        }

        SliderThumb _thumb;
        public SliderThumb Thumb
        {
            get { return _thumb; }
            set
            {
                if (_thumb != null) ClearThumbBindings();
                UpdateComponent(_thumb, value);
                _thumb = value;
                BindThumbToSlider();
            }
        }

        RangeSliderThumb _rangeThumb;
        public RangeSliderThumb RangeThumb
        {
            get { return _rangeThumb; }
            set
            {
                if (_rangeThumb != null) ClearRangeThumbBindings();
                UpdateComponent(_rangeThumb, value);
                _rangeThumb = value;
                BindRangeThumbToSlider();
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

        SliderThumbsControl _thumbsControl;
        public SliderThumbsControl ThumbsControl
        {
            get { return _thumbsControl; }
            set
            {
                UpdateComponent(_thumbsControl, value);
                _thumbsControl = value;
            }
        }

        private Slider _slider;

        private double Density { get; set; }

        private double ThumbCenterOffset { get; set; }

        #region VisualChildren Verwaltung
        private Visual[] _children;

        private void UpdateComponent(Visual oldValue, Visual newValue)
        {
            if (oldValue != newValue)
            {
                if (_children == null) _children = new Visual[6];
                if (oldValue != null) RemoveVisualChild(oldValue);

                int i = 0;
                while (i < 6)
                {
                    // Array ist nicht gefüllt, break
                    if (_children[i] == null) break;

                    // Alten Wert gefunden
                    if (_children[i] == oldValue)
                    {
                        // Alle Werte um eins nach oben verschieben, damit das neue an letzter Stelle landet
                        while (i < 5 && _children[i + 1] != null)
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

        public virtual double ValueFromPoint(Point point)
        {
            double value;
            // Distanz von Mitte des Thumbs zum Punkt finden
            if (Orientation == Orientation.Horizontal)
            {
                value = Value + ValueFromDistance(point.X - ThumbCenterOffset, point.Y - (RenderSize.Height * 0.5));
            }
            else
            {
                value = Value + ValueFromDistance(point.X - (RenderSize.Width * 0.5), point.Y - ThumbCenterOffset);
            }

            return Math.Max(Minimum, Math.Min(Maximum, value));
        }

        public virtual double ValueFromDistance(double horizontal, double vertical)
        {
            double scale = IsDirectionReversed ? -1 : 1;

            if (Orientation == Orientation.Horizontal)
            {
                return scale * horizontal * Density;
            }
            else
            {
                return -1 * scale * vertical * Density;
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _slider = this.ParentOfType<Slider>();

            BindTrackToSlider();
            BindThumbToSlider();
            BindRangeThumbToSlider();
        }

        private void BindTrackToSlider()
        {
            if (_slider == null) return;

            SetBinding(MinimumProperty, new Binding("Minimum") { Source = _slider });
            SetBinding(MaximumProperty, new Binding("Maximum") { Source = _slider });
            SetBinding(ValueProperty, new Binding("Value") { Source = _slider });
            SetBinding(RangeStartProperty, new Binding("RangeStart") { Source = _slider });
            SetBinding(RangeEndProperty, new Binding("RangeEnd") { Source = _slider });
            SetBinding(OrientationProperty, new Binding("Orientation") { Source = _slider });
            SetBinding(ThumbModeProperty, new Binding("ThumbMode") { Source = _slider });
            SetBinding(IsDirectionReversedProperty, new Binding("IsDirectionReversed") { Source = _slider });
        }

        private void BindThumbToSlider()
        {
            if (Thumb == null || _slider == null) return;

            Thumb.SetBinding(SliderThumb.ValueProperty, new Binding("Value") { Source = _slider, Mode = BindingMode.TwoWay });
        }

        private void ClearThumbBindings()
        {
            BindingOperations.ClearBinding(Thumb, SliderThumb.ValueProperty);
        }

        private void BindRangeThumbToSlider()
        {
            if (RangeThumb == null || _slider == null) return;

            RangeThumb.SetBinding(RangeSliderThumb.RangeStartProperty, new Binding("RangeStart") { Source = _slider, Mode = BindingMode.TwoWay });
            RangeThumb.SetBinding(RangeSliderThumb.RangeEndProperty, new Binding("RangeEnd") { Source = _slider, Mode = BindingMode.TwoWay });
            RangeThumb.SetBinding(RangeSliderThumb.MinimumRangeSpanProperty, new Binding("MinimumRangeSpan") { Source = _slider });
            RangeThumb.SetBinding(RangeSliderThumb.MaximumRangeSpanProperty, new Binding("MaximumRangeSpan") { Source = _slider });
        }

        private void ClearRangeThumbBindings()
        {
            BindingOperations.ClearBinding(RangeThumb, RangeSliderThumb.RangeStartProperty);
            BindingOperations.ClearBinding(RangeThumb, RangeSliderThumb.RangeEndProperty);
            BindingOperations.ClearBinding(RangeThumb, RangeSliderThumb.MinimumRangeSpanProperty);
            BindingOperations.ClearBinding(RangeThumb, RangeSliderThumb.MaximumRangeSpanProperty);
        }

        // Sicherstellen, dass die Länge nicht < 0 oder > trackLength ist
        private static void CoerceLength(ref double componentLength, double trackLength)
        {
            if (componentLength < 0) componentLength = 0.0;
            else if (componentLength > trackLength || double.IsNaN(componentLength)) componentLength = trackLength;
        }

        private SliderThumbBase GetCurrentlyRelevantThumb()
        {
            switch (ThumbMode)
            {
                case SliderThumbMode.Single:
                {
                    return Thumb;
                }
                case SliderThumbMode.Range:
                {
                    return RangeThumb;
                }
                case SliderThumbMode.Custom:
                {
                    return ThumbsControl?.GetThumbForMeasure();
                }
            }

            return null;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var desiredSize = new Size(0.0, 0.0);

            var thumb = GetCurrentlyRelevantThumb();

            if (ThumbMode == SliderThumbMode.Custom && ThumbsControl != null)
            {
                ThumbsControl.Measure(availableSize);
                desiredSize = ThumbsControl.DesiredSize;
            }
            else if (thumb != null)
            {
                thumb.Measure(availableSize);
                desiredSize = thumb.DesiredSize;
            }

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            ResetArrangements();

            if (ThumbMode == SliderThumbMode.Custom) CustomArrange(finalSize);
            else NormalArrange(finalSize);

            return finalSize;
        }

        private void ResetArrangements()
        {
            if (Thumb != null) Thumb.Arrange(new Rect());
            if (RangeThumb != null) RangeThumb.Arrange(new Rect());
            if (ValueIndicator != null) ValueIndicator.Arrange(new Rect());
            if (DecreaseRepeatButton != null) DecreaseRepeatButton.Arrange(new Rect());
            if (IncreaseRepeatButton != null) IncreaseRepeatButton.Arrange(new Rect());
            if (ThumbsControl != null) ThumbsControl.Arrange(new Rect());
        }

        private void NormalArrange(Size finalSize)
        {
            double decreaseButtonLength, thumbLength, increaseButtonLength, additionalThumbLength;

            var isVertical = Orientation == Orientation.Vertical;

            var range = Math.Max(0.0, Maximum - Minimum);
            var offset = Math.Min(range, Value - Minimum);
            var offsetStart = Math.Min(range, RangeStart - Minimum);
            var offsetEnd = Math.Min(range, RangeEnd - Minimum);

            double trackLength;

            var thumb = GetCurrentlyRelevantThumb();

            // Größe des Thumb festlegen
            if (isVertical)
            {
                trackLength = finalSize.Height;
                thumbLength = thumb == null ? 0 : thumb.DesiredSize.Height;
            }
            else
            {
                trackLength = finalSize.Width;
                thumbLength = thumb == null ? 0 : thumb.DesiredSize.Width;
            }

            if (ThumbMode == SliderThumbMode.Range)
            {
                offset = offsetStart;
            }

            CoerceLength(ref thumbLength, trackLength);

            var remainingTrackLength = trackLength - thumbLength;

            decreaseButtonLength = remainingTrackLength * offset / range;
            CoerceLength(ref decreaseButtonLength, remainingTrackLength);

            if (ThumbMode == SliderThumbMode.Range)
            {
                additionalThumbLength = remainingTrackLength * offsetEnd / range - decreaseButtonLength;
                CoerceLength(ref additionalThumbLength, trackLength);
            }
            else additionalThumbLength = 0;

            var fullThumbLength = thumbLength + additionalThumbLength;

            increaseButtonLength = remainingTrackLength - decreaseButtonLength - additionalThumbLength;
            CoerceLength(ref increaseButtonLength, remainingTrackLength);

            Density = range / remainingTrackLength;

            var offsetPoint = new Point();
            var pieceSize = finalSize;
            // Damit nicht jedes mal die DependencyProperty ausgelesen werden muss wird der Wert zwischengespeichert
            var isDirectionReversed = IsDirectionReversed;

            double topLeftMargin, bottomRightMargin;

            if (isVertical)
            {
                CoerceLength(ref decreaseButtonLength, finalSize.Height);
                CoerceLength(ref increaseButtonLength, finalSize.Height);
                CoerceLength(ref fullThumbLength, finalSize.Height);

                offsetPoint.Y = isDirectionReversed ? decreaseButtonLength + fullThumbLength : 0.0;
                pieceSize.Height = increaseButtonLength;

                if (IncreaseRepeatButton != null) IncreaseRepeatButton.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.Y = isDirectionReversed ? 0.0 : increaseButtonLength + fullThumbLength;
                pieceSize.Height = decreaseButtonLength;

                if (DecreaseRepeatButton != null) DecreaseRepeatButton.Arrange(new Rect(offsetPoint, pieceSize));
                if (ValueIndicator != null && ThumbMode == SliderThumbMode.Single) ValueIndicator.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.Y = isDirectionReversed ? decreaseButtonLength : increaseButtonLength;
                pieceSize.Height = fullThumbLength;

                if (thumb != null) thumb.Arrange(new Rect(offsetPoint, pieceSize));

                ThumbCenterOffset = offsetPoint.Y + (thumbLength * 0.5);

                topLeftMargin = thumbLength / 2;
                bottomRightMargin = thumbLength / 2;
            }
            else
            {
                CoerceLength(ref decreaseButtonLength, finalSize.Width);
                CoerceLength(ref increaseButtonLength, finalSize.Width);
                CoerceLength(ref fullThumbLength, finalSize.Width);

                offsetPoint.X = isDirectionReversed ? increaseButtonLength + fullThumbLength : 0.0;
                pieceSize.Width = decreaseButtonLength;

                if (DecreaseRepeatButton != null) DecreaseRepeatButton.Arrange(new Rect(offsetPoint, pieceSize));
                if (ValueIndicator != null && ThumbMode == SliderThumbMode.Single) ValueIndicator.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.X = isDirectionReversed ? 0.0 : decreaseButtonLength + fullThumbLength;
                pieceSize.Width = increaseButtonLength;

                if (IncreaseRepeatButton != null) IncreaseRepeatButton.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.X = isDirectionReversed ? increaseButtonLength : decreaseButtonLength;
                pieceSize.Width = fullThumbLength;

                if (thumb != null) thumb.Arrange(new Rect(offsetPoint, pieceSize));

                ThumbCenterOffset = offsetPoint.X + (thumbLength * 0.5);

                topLeftMargin = thumbLength / 2;
                bottomRightMargin = thumbLength / 2;
            }

            if (_slider != null)
            {
                _slider.UpdateMargins(topLeftMargin, bottomRightMargin);
            }
        }

        private void CustomArrange(Size finalSize)
        {
            ThumbCenterOffset = 0;

            if (ThumbsControl != null)
            {
                ThumbsControl.Arrange(new Rect(finalSize));

                var isVertical = Orientation == Orientation.Vertical;
                var thumb = GetCurrentlyRelevantThumb();

                // Größe des Thumb festlegen
                double thumbLength, trackLength;

                if (isVertical)
                {
                    trackLength = finalSize.Height;
                    thumbLength = thumb == null ? 0 : thumb.DesiredSize.Height;
                }
                else
                {
                    trackLength = finalSize.Width;
                    thumbLength = thumb == null ? 0 : thumb.DesiredSize.Width;
                }

                var remainingTrackLength = trackLength - thumbLength;
                var range = Math.Max(0.0, Maximum - Minimum);

                Density = range / remainingTrackLength;

                // Wenn wir ein ThumbsPanel haben, kriegt das die Anweisung neu anzuordnen, da sonst im Fall von IsDirectionReversed hier nicht neu angeordnet wird
                if (thumb?.ParentThumbsPanel != null) thumb.ParentThumbsPanel.InvalidateArrange();
            }
            else
            {
                Density = 0;
            }
        }
    }
}