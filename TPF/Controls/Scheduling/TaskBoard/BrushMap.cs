using System.Windows.Media;

namespace TPF.Controls
{
    public class BrushMap : NotifyObject
    {
        object _key;
        public object Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        Brush _brush;
        public Brush Brush
        {
            get { return _brush; }
            set { SetProperty(ref _brush, value); }
        }
    }
}