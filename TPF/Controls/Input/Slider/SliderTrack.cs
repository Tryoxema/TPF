using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsArrange));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region IsDirectionReversed DependencyProperty
        public static readonly DependencyProperty IsDirectionReversedProperty = Slider.IsDirectionReversedProperty.AddOwner(typeof(SliderTrack),
            new PropertyMetadata(BooleanBoxes.FalseBox));

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

        Thumb _thumb;
        public Thumb Thumb
        {
            get { return _thumb; }
            set
            {
                UpdateComponent(_thumb, value);
                _thumb = value;
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

        private double ThumbCenterOffset { get; set; }

        #region VisualChildren Verwaltung
        private Visual[] _children;

        private void UpdateComponent(Control oldValue, Control newValue)
        {
            if (oldValue != newValue)
            {
                if (_children == null) _children = new Visual[3];
                if (oldValue != null) RemoveVisualChild(oldValue);

                int i = 0;
                while (i < 3)
                {
                    // Array ist nicht gefüllt, break
                    if (_children[i] == null) break;

                    // Alten Wert gefunden
                    if (_children[i] == oldValue)
                    {
                        // Alle Werte um eins nach oben verschieben, damit das neue an letzter Stelle landet
                        while (i < 2 && _children[i + 1] != null)
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
                if (_children == null || _children[0] == null) return 0;
                else if (_children[1] == null) return 1;
                else return _children[2] == null ? 2 : 3;
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

        private void BindToTemplatedParent(DependencyProperty target, DependencyProperty source)
        {
            var binding = new Binding
            {
                RelativeSource = RelativeSource.TemplatedParent,
                Path = new PropertyPath(source)
            };
            SetBinding(target, binding);
        }

        private void BindChildToTemplatedParent(FrameworkElement element, DependencyProperty target, DependencyProperty source)
        {
            var binding = new Binding
            {
                Source = TemplatedParent,
                Path = new PropertyPath(source)
            };
            element.SetBinding(target, binding);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Properties vom übergeordneten Slider übernehmen
            if (TemplatedParent is Slider parent)
            {
                BindToTemplatedParent(MinimumProperty, RangeBase.MinimumProperty);
                BindToTemplatedParent(MaximumProperty, RangeBase.MaximumProperty);
                BindToTemplatedParent(ValueProperty, RangeBase.ValueProperty);
                BindToTemplatedParent(OrientationProperty, Slider.OrientationProperty);
                BindToTemplatedParent(IsDirectionReversedProperty, Slider.IsDirectionReversedProperty);

                BindChildToTemplatedParent(DecreaseRepeatButton, RepeatButton.DelayProperty, Slider.DelayProperty);
                BindChildToTemplatedParent(DecreaseRepeatButton, RepeatButton.IntervalProperty, Slider.IntervalProperty);
                BindChildToTemplatedParent(IncreaseRepeatButton, RepeatButton.DelayProperty, Slider.DelayProperty);
                BindChildToTemplatedParent(IncreaseRepeatButton, RepeatButton.IntervalProperty, Slider.IntervalProperty);
            }
        }

        // Sicherstellen, dass die Länge nicht < 0 oder > trackLength ist
        private static void CoerceLength(ref double componentLength, double trackLength)
        {
            if (componentLength < 0) componentLength = 0.0;
            else if (componentLength > trackLength || double.IsNaN(componentLength)) componentLength = trackLength;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var desiredSize = new Size(0.0, 0.0);

            if (Thumb != null)
            {
                Thumb.Measure(availableSize);
                desiredSize = Thumb.DesiredSize;
            }

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double decreaseButtonLength, thumbLength, increaseButtonLength;

            var isVertical = Orientation == Orientation.Vertical;

            var range = Math.Max(0.0, Maximum - Minimum);
            var offset = Math.Min(range, Value - Minimum);

            double trackLength;

            // Größe des Thumb festlegen
            if (isVertical)
            {
                trackLength = finalSize.Height;
                thumbLength = Thumb == null ? 0 : Thumb.DesiredSize.Height;
            }
            else
            {
                trackLength = finalSize.Width;
                thumbLength = Thumb == null ? 0 : Thumb.DesiredSize.Width;
            }

            CoerceLength(ref thumbLength, trackLength);

            var remainingTrackLength = trackLength - thumbLength;

            decreaseButtonLength = remainingTrackLength * offset / range;
            CoerceLength(ref decreaseButtonLength, remainingTrackLength);

            increaseButtonLength = remainingTrackLength - decreaseButtonLength;
            CoerceLength(ref increaseButtonLength, remainingTrackLength);

            Density = range / remainingTrackLength;

            var offsetPoint = new Point();
            var pieceSize = finalSize;
            // Damit nicht jedes mal die DependencyProperty ausgelesen werden muss wird der Wert zwischengespeichert
            var isDirectionReversed = IsDirectionReversed;

            if (isVertical)
            {
                CoerceLength(ref decreaseButtonLength, finalSize.Height);
                CoerceLength(ref increaseButtonLength, finalSize.Height);
                CoerceLength(ref thumbLength, finalSize.Height);

                offsetPoint.Y = isDirectionReversed ? decreaseButtonLength + thumbLength : 0.0;
                pieceSize.Height = increaseButtonLength;

                if (IncreaseRepeatButton != null) IncreaseRepeatButton.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.Y = isDirectionReversed ? 0.0 : increaseButtonLength + thumbLength;
                pieceSize.Height = decreaseButtonLength;

                if (DecreaseRepeatButton != null) DecreaseRepeatButton.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.Y = isDirectionReversed ? decreaseButtonLength : increaseButtonLength;
                pieceSize.Height = thumbLength;

                if (Thumb != null) Thumb.Arrange(new Rect(offsetPoint, pieceSize));

                ThumbCenterOffset = offsetPoint.Y + (thumbLength * 0.5);
            }
            else
            {
                CoerceLength(ref decreaseButtonLength, finalSize.Width);
                CoerceLength(ref increaseButtonLength, finalSize.Width);
                CoerceLength(ref thumbLength, finalSize.Width);

                offsetPoint.X = isDirectionReversed ? increaseButtonLength + thumbLength : 0.0;
                pieceSize.Width = decreaseButtonLength;

                if (DecreaseRepeatButton != null) DecreaseRepeatButton.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.X = isDirectionReversed ? 0.0 : decreaseButtonLength + thumbLength;
                pieceSize.Width = increaseButtonLength;

                if (IncreaseRepeatButton != null) IncreaseRepeatButton.Arrange(new Rect(offsetPoint, pieceSize));

                offsetPoint.X = isDirectionReversed ? increaseButtonLength : decreaseButtonLength;
                pieceSize.Width = thumbLength;

                if (Thumb != null) Thumb.Arrange(new Rect(offsetPoint, pieceSize));

                ThumbCenterOffset = offsetPoint.X + (thumbLength * 0.5);
            }

            return finalSize;
        }
    }
}