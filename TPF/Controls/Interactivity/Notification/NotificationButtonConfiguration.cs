using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TPF.Controls
{
    public class NotificationButtonConfiguration
    {
        object _content;
        public object Content
        {
            get { return _content; }
            set
            {
                _content = value;
                _applyContent = true;
            }
        }

        DataTemplate _contentTemplate;
        public DataTemplate ContentTemplate
        {
            get { return _contentTemplate; }
            set
            {
                _contentTemplate = value;
                _applyContentTemplate = true;
            }
        }

        Brush _foreground;
        public Brush Foreground
        {
            get { return _foreground; }
            set
            {
                _foreground = value;
                _applyForeground = true;
            }
        }

        Brush _background;
        public Brush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                _applyBackground = true;
            }
        }

        bool _applyContent;
        bool _applyContentTemplate;
        bool _applyForeground;
        bool _applyBackground;

        public virtual void Apply(ButtonBase button)
        {
            if (_applyContent) button.Content = Content;
            if (_applyContentTemplate) button.ContentTemplate = ContentTemplate;
            if (_applyForeground) button.Foreground = Foreground;
            if (_applyBackground) button.Background = Background;
        }
    }
}