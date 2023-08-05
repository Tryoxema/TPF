using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media.Animation;

namespace TPF.Controls
{
    [TemplatePart(Name = "PART_SegmentsPanel", Type = typeof(Canvas))]
    public class ProgressBar : ProgressBarBase
    {
        static ProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(typeof(ProgressBar)));
        }

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(ProgressBar),
            new PropertyMetadata(default));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region ContentPosition DependencyProperty
        public static readonly DependencyProperty ContentPositionProperty = DependencyProperty.Register("ContentPosition",
            typeof(ProgressBarContentPosition),
            typeof(ProgressBar),
            new PropertyMetadata(ProgressBarContentPosition.Center));

        public ProgressBarContentPosition ContentPosition
        {
            get { return (ProgressBarContentPosition)GetValue(ContentPositionProperty); }
            set { SetValue(ContentPositionProperty, value); }
        }
        #endregion

        #region AnimationProgressFactor DependencyProperty
        private static readonly DependencyProperty AnimationProgressFactorProperty = DependencyProperty.Register("AnimationProgressFactor",
            typeof(double),
            typeof(ProgressBar),
            new PropertyMetadata(0.0, OnAnimationProgressFactorChanged));

        private static void OnAnimationProgressFactorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ProgressBar)sender;

            instance.ResizeIndeterminateSegments();
        }

        private double AnimationProgressFactor
        {
            get { return (double)GetValue(AnimationProgressFactorProperty); }
            set { SetValue(AnimationProgressFactorProperty, value); }
        }
        #endregion

        private Canvas _segmentsPanel;
        private readonly Dictionary<int, Border> _backgroundSegmentsCache = new Dictionary<int, Border>();
        private readonly Dictionary<int, Border> _secondaryProgressSegmentsCache = new Dictionary<int, Border>();
        private readonly Dictionary<int, Border> _progressSegmentsCache = new Dictionary<int, Border>();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ClearSegmentsCache();

            if (_segmentsPanel != null)
            {
                _segmentsPanel.SizeChanged -= SegmentsPanel_SizeChanged;
            }

            _segmentsPanel = GetTemplateChild("PART_SegmentsPanel") as Canvas;

            if (_segmentsPanel != null)
            {
                _segmentsPanel.SizeChanged += SegmentsPanel_SizeChanged;
            }

            GenerateSegments();
            ResizeSegments();

            if (IsIndeterminate) ResizeIndeterminateSegments();
            else ResizeValueSegments();
        }

        private void SegmentsPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeSegments();
            if (IsIndeterminate) ResizeIndeterminateSegments();
            else ResizeValueSegments();
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);

            ResizeValueSegments();
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);

            ResizeValueSegments();
        }

        protected override void OnProgressChanged(double oldValue, double newValue)
        {
            base.OnProgressChanged(oldValue, newValue);

            ResizeValueSegments();
        }

        protected override void OnSecondaryProgressChanged(double oldValue, double newValue)
        {
            base.OnSecondaryProgressChanged(oldValue, newValue);

            ResizeValueSegments();
        }

        protected override void OnIsIndeterminateChanged(bool oldValue, bool newValue)
        {
            base.OnIsIndeterminateChanged(oldValue, newValue);

            if (oldValue) StopAnimation();

            if (newValue) StartAnimation();
        }

        protected override void OnSegmentCountChanged(int oldValue, int newValue)
        {
            base.OnSegmentCountChanged(oldValue, newValue);

            ClearSegmentsCache();

            GenerateSegments();
            ResizeSegments();

            if (IsIndeterminate) ResizeIndeterminateSegments();
            else ResizeValueSegments();
        }

        protected override void OnGapWidthChanged(double oldValue, double newValue)
        {
            base.OnGapWidthChanged(oldValue, newValue);

            ResizeSegments();

            if (IsIndeterminate) ResizeIndeterminateSegments();
            else ResizeValueSegments();
        }

        private void StartAnimation()
        {
            // Falls aus irgend einem Grund eine Animation läuft, diese entfernen
            BeginAnimation(AnimationProgressFactorProperty, null);

            // SecondaryProgress-Segmente ausblenden
            foreach (var segment in _secondaryProgressSegmentsCache)
            {
                segment.Value.Width = 0;
            }

            var animation = new DoubleAnimation(-0.25, 1, TimeSpan.FromSeconds(2))
            {
                RepeatBehavior = RepeatBehavior.Forever
            };

            BeginAnimation(AnimationProgressFactorProperty, animation);
        }

        private void StopAnimation()
        {
            BeginAnimation(AnimationProgressFactorProperty, null);

            ResizeValueSegments();
        }

        private void ClearSegmentsCache()
        {
            foreach (var segment in _backgroundSegmentsCache)
            {
                BindingOperations.ClearAllBindings(segment.Value);
            }

            foreach (var segment in _secondaryProgressSegmentsCache)
            {
                BindingOperations.ClearAllBindings(segment.Value);
            }

            foreach (var segment in _progressSegmentsCache)
            {
                BindingOperations.ClearAllBindings(segment.Value);
            }

            _backgroundSegmentsCache.Clear();
            _secondaryProgressSegmentsCache.Clear();
            _progressSegmentsCache.Clear();

            if (_segmentsPanel != null) _segmentsPanel.Children.Clear();
        }

        private void GenerateSegments()
        {
            if (_segmentsPanel == null) return;

            // Immer mindestens ein Segment generieren
            var segmentCount = Math.Max(1, SegmentCount);

            var width = _segmentsPanel.ActualWidth;
            var height = _segmentsPanel.ActualHeight;
            var gapCount = segmentCount - 1;
            var gapWidth = Math.Max(0, GapWidth);
            var totalSegmentWidth = Math.Max(0, width - gapCount * gapWidth);
            var segmentWidth = totalSegmentWidth / segmentCount;

            // Hintergrund-Segmente generieren
            for (int i = 0; i < segmentCount; i++)
            {
                var segment = new Border()
                {
                    Width = segmentWidth,
                    Height = height
                };

                segment.SetBinding(BackgroundProperty, new Binding(nameof(Background)) { Source = this });
                segment.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { Source = this });
                segment.SetBinding(Border.BorderBrushProperty, new Binding(nameof(BorderBrush)) { Source = this });
                segment.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { Source = this });

                var left = i * (segmentWidth + gapWidth);

                Canvas.SetTop(segment, 0);
                Canvas.SetLeft(segment, left);

                _backgroundSegmentsCache.Add(i, segment);
                _segmentsPanel.Children.Add(segment);
            }

            // SecondaryProgress-Segmente generieren
            for (int i = 0; i < segmentCount; i++)
            {
                var segment = new Border()
                {
                    Height = height
                };

                segment.SetBinding(BackgroundProperty, new Binding(nameof(SecondaryProgressBrush)) { Source = this });
                segment.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { Source = this });
                segment.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { Source = this });

                var left = i * (segmentWidth + gapWidth);

                Canvas.SetTop(segment, 0);
                Canvas.SetLeft(segment, left);

                _secondaryProgressSegmentsCache.Add(i, segment);
                _segmentsPanel.Children.Add(segment);
            }

            // Progress-Segmente generieren
            for (int i = 0; i < segmentCount; i++)
            {
                var segment = new Border()
                {
                    Height = height
                };

                segment.SetBinding(BackgroundProperty, new Binding(nameof(ProgressBrush)) { Source = this });
                segment.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { Source = this });
                segment.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { Source = this });

                var left = i * (segmentWidth + gapWidth);

                Canvas.SetTop(segment, 0);
                Canvas.SetLeft(segment, left);

                _progressSegmentsCache.Add(i, segment);
                _segmentsPanel.Children.Add(segment);
            }
        }

        private void ResizeSegments()
        {
            if (_segmentsPanel == null) return;

            var segmentCount = Math.Max(1, SegmentCount);

            var width = _segmentsPanel.ActualWidth;
            var height = _segmentsPanel.ActualHeight;
            var gapCount = segmentCount - 1;
            var gapWidth = GapWidth;
            var totalSegmentWidth = Math.Max(0, width - gapCount * gapWidth);
            var segmentWidth = totalSegmentWidth / segmentCount;

            foreach (var segment in _backgroundSegmentsCache)
            {
                segment.Value.Width = segmentWidth;
                segment.Value.Height = height;

                var left = segment.Key * (segmentWidth + gapWidth);

                Canvas.SetLeft(segment.Value, left);
            }

            foreach (var segment in _secondaryProgressSegmentsCache)
            {
                segment.Value.Height = height;

                var left = segment.Key * (segmentWidth + gapWidth);

                Canvas.SetLeft(segment.Value, left);
            }

            foreach (var segment in _progressSegmentsCache)
            {
                segment.Value.Height = height;

                var left = segment.Key * (segmentWidth + gapWidth);

                Canvas.SetLeft(segment.Value, left);
            }
        }

        private void ResizeValueSegments()
        {
            if (_segmentsPanel == null || IsIndeterminate) return;

            var segmentCount = Math.Max(1, SegmentCount);

            var width = _segmentsPanel.ActualWidth;
            var gapCount = segmentCount - 1;
            var gapWidth = GapWidth;
            var totalSegmentWidth = Math.Max(0, width - gapCount * gapWidth);
            var segmentWidth = totalSegmentWidth / segmentCount;

            var progress = Progress;
            var secondaryProgress = SecondaryProgress;
            var segmentValue = Maximum / segmentCount;

            foreach (var segment in _secondaryProgressSegmentsCache)
            {
                var remainingValue = secondaryProgress - segmentValue * segment.Key;

                var factor = remainingValue >= segmentValue ? 1d : Math.Max(0, remainingValue / segmentValue);

                segment.Value.Width = segmentWidth * factor;

                var left = segment.Key * (segmentWidth + gapWidth);

                Canvas.SetLeft(segment.Value, left);
            }

            foreach (var segment in _progressSegmentsCache)
            {
                var remainingValue = progress - segmentValue * segment.Key;

                var factor = remainingValue >= segmentValue ? 1d : Math.Max(0, remainingValue / segmentValue);

                segment.Value.Width = segmentWidth * factor;

                var left = segment.Key * (segmentWidth + gapWidth);

                Canvas.SetLeft(segment.Value, left);
            }
        }

        private void ResizeIndeterminateSegments()
        {
            if (!IsIndeterminate || _segmentsPanel == null) return;

            var segmentCount = Math.Max(1, SegmentCount);

            var width = _segmentsPanel.ActualWidth;
            var gapCount = segmentCount - 1;
            var gapWidth = GapWidth;
            var totalSegmentWidth = Math.Max(0, width - gapCount * gapWidth);
            var segmentWidth = totalSegmentWidth / segmentCount;

            var segmentValue = Maximum / segmentCount;

            var indeterminateProgressFactor = AnimationProgressFactor;

            var indeterminateProgressValue = Maximum * indeterminateProgressFactor;
            var indeterminateMarkerSize = totalSegmentWidth / 4;
            var startSegment = Math.Max(0, (int)(indeterminateProgressValue / segmentValue));
            var indeterminateStartValue = indeterminateProgressValue - startSegment * segmentValue;

            foreach (var segment in _progressSegmentsCache)
            {
                if (segment.Key < startSegment)
                {
                    segment.Value.Width = 0;
                }
                else
                {
                    var skipFactor = indeterminateStartValue / segmentValue;
                    var widthFactor = 1 - skipFactor;

                    var currentSegmentWidth = Math.Min(indeterminateMarkerSize, segmentWidth * widthFactor);
                    indeterminateMarkerSize -= currentSegmentWidth;

                    var left = segment.Key * (segmentWidth + gapWidth) + segmentWidth * skipFactor;

                    if (left < 0)
                    {
                        currentSegmentWidth += left;
                        left = 0;
                    }

                    segment.Value.Width = currentSegmentWidth;

                    Canvas.SetLeft(segment.Value, left);

                    indeterminateStartValue = 0;
                }
            }
        }
    }
}