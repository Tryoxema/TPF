using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class IntervalSelectionThumb : Control
    {
        static IntervalSelectionThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IntervalSelectionThumb), new FrameworkPropertyMetadata(typeof(IntervalSelectionThumb)));
        }

        #region OverlayBrush DependencyProperty
        public static readonly DependencyProperty OverlayBrushProperty = DependencyProperty.Register("OverlayBrush",
            typeof(Brush),
            typeof(IntervalSelectionThumb),
            new PropertyMetadata(null));

        public Brush OverlayBrush
        {
            get { return (Brush)GetValue(OverlayBrushProperty); }
            set { SetValue(OverlayBrushProperty, value); }
        }
        #endregion

        internal Controls.DateTimeRangeNavigator Owner { get; set; }

        private IntervalSelectionThumbTrack _track;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _track = GetTemplateChild("PART_Track") as IntervalSelectionThumbTrack;

            InvalidateThumbs();
        }

        internal void InvalidateThumbs()
        {
            if (_track != null) _track.InvalidateArrange();
        }
    }
}