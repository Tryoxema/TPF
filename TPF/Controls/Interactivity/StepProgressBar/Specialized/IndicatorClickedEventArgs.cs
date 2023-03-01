using System.Windows;

namespace TPF.Controls.Specialized.StepProgressBar
{
    public class IndicatorClickedEventArgs : RoutedEventArgs
    {
        internal IndicatorClickedEventArgs(StepItem step) : base(Controls.StepProgressBar.IndicatorClickedEvent)
        {
            Step = step;
        }

        public StepItem Step { get; }
    }

    public delegate void IndicatorClickedEventHandler(object sender, IndicatorClickedEventArgs e);
}