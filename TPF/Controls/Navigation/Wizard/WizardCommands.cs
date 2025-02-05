using System.Windows.Input;

namespace TPF.Controls
{
    public static class WizardCommands
    {
        static WizardCommands()
        {
            var type = typeof(WizardCommands);

            GoToPrevious = new RoutedCommand(nameof(GoToPrevious), type);
            GoToNext = new RoutedCommand(nameof(GoToNext), type);
            Finish = new RoutedCommand(nameof(Finish), type);
            Cancel = new RoutedCommand(nameof(Cancel), type);
        }

        public static RoutedCommand GoToPrevious { get; private set; }
        public static RoutedCommand GoToNext { get; private set; }
        public static RoutedCommand Finish { get; private set; }
        public static RoutedCommand Cancel { get; private set; }
    }
}