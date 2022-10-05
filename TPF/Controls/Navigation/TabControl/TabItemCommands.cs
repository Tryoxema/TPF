using System.Windows.Input;

namespace TPF.Controls
{
    public static class TabItemCommands
    {
        static TabItemCommands()
        {
            var type = typeof(TabItemCommands);

            Close = new RoutedCommand(nameof(Close), type);
            TogglePin = new RoutedCommand(nameof(TogglePin), type);
        }

        public static RoutedCommand Close { get; private set; }
        public static RoutedCommand TogglePin { get; private set; }
    }
}