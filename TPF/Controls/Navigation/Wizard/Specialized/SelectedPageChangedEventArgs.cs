using System.Windows;

namespace TPF.Controls.Specialized.Wizard
{
    public class SelectedPageChangedEventArgs : RoutedEventArgs
    {
        public SelectedPageChangedEventArgs(RoutedEvent routedEvent, WizardPage oldPage, WizardPage newPage) : base(routedEvent)
        {
            OldPage = oldPage;
            NewPage = newPage;
        }

        public WizardPage OldPage { get; }

        public WizardPage NewPage { get; }
    }

    public delegate void SelectedPageChangedEventHandler(object sender, SelectedPageChangedEventArgs e);
}