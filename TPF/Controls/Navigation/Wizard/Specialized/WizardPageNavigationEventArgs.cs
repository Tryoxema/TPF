using System.Windows;

namespace TPF.Controls.Specialized.Wizard
{
    public class WizardPageNavigationEventArgs : RoutedEventArgs
    {
        public WizardPageNavigationEventArgs(RoutedEvent routedEvent, int oldIndex, int newIndex) : base(routedEvent)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
        }

        public int OldIndex { get; }

        public int NewIndex { get; set; }

        public bool Cancel { get; set; }
    }

    public delegate void WizardPageNavigationEventHandler(object sender, WizardPageNavigationEventArgs e);
}