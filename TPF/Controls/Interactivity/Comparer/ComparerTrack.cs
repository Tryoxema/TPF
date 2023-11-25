using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TPF.Controls
{
    public class ComparerTrack : FrameworkElement
    {
        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = Comparer.OrientationProperty.AddOwner(typeof(ComparerTrack),
            new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region Value DependencyProperty
        public static readonly DependencyProperty ValueProperty = Comparer.ValueProperty.AddOwner(typeof(ComparerTrack),
            new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        private Comparer _comparer;

        private double Density { get; set; }

        ContentPresenter _firstContentPresenter;
        public ContentPresenter FirstContentPresenter
        {
            get { return _firstContentPresenter; }
            set
            {
                if (_secondContentPresenter == value) throw new NotSupportedException("First and Second can't be the same ContentPresenter.");
                UpdateComponent(_firstContentPresenter, value);
                _firstContentPresenter = value;
            }
        }

        Thumb _thumb;
        public Thumb Thumb
        {
            get { return _thumb; }
            set
            {
                if (_thumb != null) UnhookupThumb(_thumb);
                UpdateComponent(_thumb, value);
                _thumb = value;
                if (_thumb != null) HookupThumb(_thumb);
            }
        }

        ContentPresenter _secondContentPresenter;
        public ContentPresenter SecondContentPresenter
        {
            get { return _secondContentPresenter; }
            set
            {
                if (_firstContentPresenter == value) throw new NotSupportedException("First and Second can't be the same ContentPresenter.");
                UpdateComponent(_secondContentPresenter, value);
                _secondContentPresenter = value;
            }
        }

        #region VisualChildren Verwaltung
        private Visual[] _children;

        private void UpdateComponent(Visual oldValue, Visual newValue)
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

            _comparer = this.ParentOfType<Comparer>();

            BindTrackToComparer();
        }

        private void BindTrackToComparer()
        {
            if (_comparer == null) return;

            SetBinding(ValueProperty, new Binding("Value") { Source = _comparer });
            SetBinding(OrientationProperty, new Binding("Orientation") { Source = _comparer });
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var desiredSize = new Size(0, 0);

            if (_thumb != null)
            {
                _thumb.Measure(availableSize);
                desiredSize = _thumb.DesiredSize;
            }

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double totalLength, thumbLength, firstLength, secondLength;

            var isVertical = Orientation == Orientation.Vertical;
            var value = Value;

            if (isVertical)
            {
                totalLength = finalSize.Height;
                thumbLength = _thumb == null ? 0 : _thumb.DesiredSize.Height;
            }
            else
            {
                totalLength = finalSize.Width;
                thumbLength = _thumb == null ? 0 : _thumb.DesiredSize.Width;
            }

            Density = 1 / totalLength;

            CoerceLength(ref thumbLength, totalLength);

            firstLength = totalLength * value;
            secondLength = totalLength * (1 - value);

            var presenterRect = new Rect(0, 0, finalSize.Width, finalSize.Height);
            // Alle Elemente werden je nach Ausrichtung positioniert
            // Die ContentPresenter kriegen beide den vollen Platz und werden über die Clip-Property entsprechend dargestellt
            if (isVertical)
            {
                if (_firstContentPresenter != null)
                {
                    _firstContentPresenter.Arrange(presenterRect);
                    var clipRect = new Rect(0, 0, finalSize.Width, firstLength);
                    _firstContentPresenter.Clip = new RectangleGeometry(clipRect);
                }

                if (_thumb != null)
                {
                    var halfLength = thumbLength / 2;
                    _thumb.Arrange(new Rect(0, firstLength - halfLength, finalSize.Width, thumbLength));
                }

                if (_secondContentPresenter != null)
                {
                    _secondContentPresenter.Arrange(presenterRect);
                    var clipRect = new Rect(0, firstLength, finalSize.Width, secondLength);
                    _secondContentPresenter.Clip = new RectangleGeometry(clipRect);
                }
            }
            else
            {
                if (_firstContentPresenter != null)
                {
                    _firstContentPresenter.Arrange(presenterRect);
                    var clipRect = new Rect(0, 0, firstLength, finalSize.Height);
                    _firstContentPresenter.Clip = new RectangleGeometry(clipRect);
                }

                if (_thumb != null)
                {
                    var halfLength = thumbLength / 2;
                    _thumb.Arrange(new Rect(firstLength - halfLength, 0, thumbLength, finalSize.Height));
                }

                if (_secondContentPresenter != null)
                {
                    _secondContentPresenter.Arrange(presenterRect);
                    var clipRect = new Rect(firstLength, 0, secondLength, finalSize.Height);
                    _secondContentPresenter.Clip = new RectangleGeometry(clipRect);
                }
            }

            return base.ArrangeOverride(finalSize);
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

        private void HookupThumb(Thumb thumb)
        {
            thumb.DragDelta += Thumb_DragDelta;
        }

        private void UnhookupThumb(Thumb thumb)
        {
            thumb.DragDelta -= Thumb_DragDelta;
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_comparer == null) return;

            var delta = ValueFromDistance(e.HorizontalChange, e.VerticalChange);

            _comparer.Value += delta;
        }
    }
}