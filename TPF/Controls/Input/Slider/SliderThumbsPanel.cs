using System;
using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls
{
    public class SliderThumbsPanel : Panel
    {
        private Slider _slider;
        internal Slider Slider
        {
            get
            {
                if (_slider == null) _slider = this.ParentOfType<Slider>();

                return _slider;
            }
        }

        private SliderThumbsControl _sliderThumbsControl;
        internal SliderThumbsControl SliderThumbsControl
        {
            get
            {
                if (_sliderThumbsControl == null) _sliderThumbsControl = this.ParentOfType<SliderThumbsControl>();

                return _sliderThumbsControl;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var finalSize = new Size();

            var isVertical = SliderThumbsControl?.Orientation == Orientation.Vertical;

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i];

                if (child is SliderThumbBase thumb)
                {
                    thumb.ParentThumbsPanel = this;
                    thumb.Measure(availableSize);

                    finalSize.Height = Math.Max(finalSize.Height, child.DesiredSize.Height);
                    finalSize.Width = Math.Max(finalSize.Width, child.DesiredSize.Width);
                }
            }

            return finalSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var minimum = Slider?.Minimum ?? 0;
            var maximum = Slider?.Maximum ?? 10;
            var range = Math.Max(0.0, maximum - minimum);

            if (range == 0) return finalSize;

            var isVertical = SliderThumbsControl?.Orientation == Orientation.Vertical;
            var isDirectionReversed = SliderThumbsControl?.IsDirectionReversed ?? false;

            var marginThumb = GetMarginThumb();

            var centerPoint = new Point(finalSize.Width / 2, finalSize.Height / 2);

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i];

                if (child is SliderThumb sliderThumb)
                {
                    var value = sliderThumb.Value;
                    var normalizedValue = Internal.Utility.NormalizeValue(value, minimum, maximum);

                    if (isVertical)
                    {
                        var factor = isDirectionReversed ? normalizedValue : 1 - normalizedValue;

                        var x = Math.Max(0, centerPoint.X - sliderThumb.DesiredSize.Width / 2);
                        var y = Math.Max(0, factor * (finalSize.Height - sliderThumb.DesiredSize.Height));

                        sliderThumb.Arrange(new Rect(x, y, sliderThumb.DesiredSize.Width, sliderThumb.DesiredSize.Height));
                    }
                    else
                    {
                        var factor = isDirectionReversed ? 1 - normalizedValue : normalizedValue;

                        var x = Math.Max(0, factor * (finalSize.Width - sliderThumb.DesiredSize.Width));
                        var y = Math.Max(0, centerPoint.Y - sliderThumb.DesiredSize.Height / 2);

                        sliderThumb.Arrange(new Rect(x, y, sliderThumb.DesiredSize.Width, sliderThumb.DesiredSize.Height));
                    }
                }
                else if (child is RangeSliderThumb rangeThumb)
                {
                    var normalizedStart = Internal.Utility.NormalizeValue(rangeThumb.RangeStart, minimum, maximum);
                    var normalizedEnd = Internal.Utility.NormalizeValue(rangeThumb.RangeEnd, minimum, maximum);

                    if (isVertical)
                    {
                        var startFactor = isDirectionReversed ? normalizedStart : 1 - normalizedStart;
                        var endFactor = isDirectionReversed ? normalizedEnd : 1 - normalizedEnd;

                        var marginThumbHeight = (marginThumb ?? rangeThumb).DesiredSize.Height;
                        var remainingTrackLength = finalSize.Height - marginThumbHeight;

                        var x = Math.Max(0, centerPoint.X - rangeThumb.DesiredSize.Width / 2);
                        var y = Math.Max(0, Math.Round((isDirectionReversed ? startFactor : endFactor) * remainingTrackLength));

                        if (rangeThumb.StartThumb != null && rangeThumb.MiddleThumb != null && rangeThumb.EndThumb != null)
                        {
                            var startThumbHeight = rangeThumb.StartThumb.DesiredSize.Height;
                            var endThumbHeight = rangeThumb.EndThumb.DesiredSize.Height;

                            var y2 = Math.Max(0, Math.Round((isDirectionReversed ? endFactor : startFactor) * (finalSize.Height - endThumbHeight)));

                            var middleThumbHeight = y2 - y;
                            var middleThumbX = Math.Max(0, centerPoint.X - rangeThumb.MiddleThumb.DesiredSize.Width / 2);

                            // Wenn beide Werte auf Maximum sind liegt der EndThumb über dem StartThumb und keiner kann mehr bewegt werden
                            // Um das zu verhindern, setzen wir hier den ZIndex einmal anders herum, wenn dieser Fall eintritt
                            if (y == y2 && normalizedStart == 1)
                            {
                                SetZIndex(rangeThumb.EndThumb, 1);
                                SetZIndex(rangeThumb.StartThumb, 2);
                            }
                            else
                            {
                                SetZIndex(rangeThumb.StartThumb, 1);
                                SetZIndex(rangeThumb.EndThumb, 2);
                            }

                            if (isDirectionReversed)
                            {
                                rangeThumb.StartThumb.Arrange(new Rect(x, y, rangeThumb.DesiredSize.Width, startThumbHeight));
                                rangeThumb.EndThumb.Arrange(new Rect(x, y2, rangeThumb.DesiredSize.Width, endThumbHeight));
                            }
                            else
                            {
                                rangeThumb.StartThumb.Arrange(new Rect(x, y2, rangeThumb.DesiredSize.Width, startThumbHeight));
                                rangeThumb.EndThumb.Arrange(new Rect(x, y, rangeThumb.DesiredSize.Width, endThumbHeight));
                            }

                            rangeThumb.MiddleThumb.Arrange(new Rect(middleThumbX, y + startThumbHeight, rangeThumb.MiddleThumb.DesiredSize.Width, middleThumbHeight));
                        }
                        else
                        {
                            var height = Math.Max(marginThumbHeight, Math.Round(((isDirectionReversed ? startFactor : endFactor) * remainingTrackLength) - y + marginThumbHeight));

                            rangeThumb.Arrange(new Rect(x, y, rangeThumb.DesiredSize.Width, height));
                        }
                    }
                    else
                    {
                        var startFactor = isDirectionReversed ? 1 - normalizedStart : normalizedStart;
                        var endFactor = isDirectionReversed ? 1 - normalizedEnd : normalizedEnd;

                        var marginThumbWidth = (marginThumb ?? rangeThumb).DesiredSize.Width;
                        var remainingTrackLength = finalSize.Width - marginThumbWidth;

                        var x = Math.Max(0, Math.Round((isDirectionReversed ? endFactor : startFactor) * remainingTrackLength));
                        var y = Math.Max(0, centerPoint.Y - rangeThumb.DesiredSize.Height / 2);

                        if (rangeThumb.StartThumb != null && rangeThumb.MiddleThumb != null && rangeThumb.EndThumb != null)
                        {
                            var startThumbWidth = rangeThumb.StartThumb.DesiredSize.Width;
                            var endThumbWidth = rangeThumb.EndThumb.DesiredSize.Width;

                            var x2 = Math.Max(0, Math.Round((isDirectionReversed ? startFactor : endFactor) * (finalSize.Width - endThumbWidth)));

                            var middleThumbWidth = x2 - x;
                            var middleThumbY = Math.Max(0, centerPoint.Y - rangeThumb.MiddleThumb.DesiredSize.Height / 2);

                            // Wenn beide Werte auf Maximum sind liegt der EndThumb über dem StartThumb und keiner kann mehr bewegt werden
                            // Um das zu verhindern, setzen wir hier den ZIndex einmal anders herum, wenn dieser Fall eintritt
                            if (x == x2 && normalizedStart == 1)
                            {
                                SetZIndex(rangeThumb.EndThumb, 1);
                                SetZIndex(rangeThumb.StartThumb, 2);
                            }
                            else
                            {
                                SetZIndex(rangeThumb.StartThumb, 1);
                                SetZIndex(rangeThumb.EndThumb, 2);
                            }

                            if (isDirectionReversed)
                            {
                                rangeThumb.StartThumb.Arrange(new Rect(x2, y, startThumbWidth, rangeThumb.DesiredSize.Height));
                                rangeThumb.EndThumb.Arrange(new Rect(x, y, endThumbWidth, rangeThumb.DesiredSize.Height));
                            }
                            else
                            {
                                rangeThumb.StartThumb.Arrange(new Rect(x, y, startThumbWidth, rangeThumb.DesiredSize.Height));
                                rangeThumb.EndThumb.Arrange(new Rect(x2, y, endThumbWidth, rangeThumb.DesiredSize.Height));
                            }

                            rangeThumb.MiddleThumb.Arrange(new Rect(x + startThumbWidth, middleThumbY, middleThumbWidth, rangeThumb.MiddleThumb.DesiredSize.Height));
                        }
                        else
                        {
                            var width = Math.Max(marginThumbWidth, Math.Round(((isDirectionReversed ? startFactor : endFactor) * remainingTrackLength) - x + marginThumbWidth));
                            
                            rangeThumb.Arrange(new Rect(x, y, width, rangeThumb.DesiredSize.Height));
                        }
                    }
                }
            }

            if (marginThumb != null && _slider != null && _slider.ThumbMode == SliderThumbMode.Custom)
            {
                double thumbLength;

                if (isVertical)
                {
                    thumbLength = marginThumb.DesiredSize.Height;
                }
                else
                {
                    thumbLength = marginThumb.DesiredSize.Width;
                }

                var topLeftMargin = thumbLength / 2;
                var bottomRightMargin = thumbLength / 2;

                _slider.UpdateMargins(topLeftMargin, bottomRightMargin);
            }

            return finalSize;
        }

        private SliderThumbBase GetMarginThumb()
        {
            if (SliderThumbsControl == null) return null;

            return SliderThumbsControl.GetThumbForMeasure();
        }
    }
}