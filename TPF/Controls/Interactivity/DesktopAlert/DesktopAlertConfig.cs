using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TPF.Controls
{
    public class DesktopAlertConfig
    {
        public object Header { get; set; }

        public DataTemplate HeaderTemplate { get; set; }

        public object Content { get; set; }

        public DataTemplate ContentTemplate { get; set; }

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        public TimeSpan? Duration { get; set; }

        public bool IsClosable { get; set; } = true;

        public Style Style { get; set; }

        public Brush Background { get; set; }

        public Brush Foreground { get; set; }

        public Action OnClick { get; set; }

        public Action OnClosed { get; set; }
    }
}