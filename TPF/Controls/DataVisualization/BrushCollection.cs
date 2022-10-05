using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace TPF.Controls
{
    public class BrushCollection : ObservableCollection<Brush>
    {
        public BrushCollection() { }

        public BrushCollection(IEnumerable<Brush> brushes) : base(brushes) { }

        public BrushCollection(List<Brush> brushes) : base(brushes) { }
    }
}