using System;
using System.Windows;
using TPF.Internal.Interop;

namespace TPF.DragDrop
{
    internal class DragAdornerWindow : Window
    {
        internal DragAdornerWindow(object content)
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            AllowDrop = false;
            Background = null;
            IsHitTestVisible = false;
            SizeToContent = SizeToContent.WidthAndHeight;
            Topmost = true;
            ShowInTaskbar = false;
            Content = content;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            NativeMethods.MakeWindowTransparent(this);

            base.OnSourceInitialized(e);
        }
    }
}