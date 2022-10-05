using System.Windows;

namespace TPF.DragDrop.Behaviors
{
    public interface IDropVisualProvider
    {
        FrameworkElement CreateDropVisual();

        Point GetPosition(DropInfo dropInfo);
    }
}