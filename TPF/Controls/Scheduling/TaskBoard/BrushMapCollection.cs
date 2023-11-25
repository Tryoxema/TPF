using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public class BrushMapCollection : ObservableCollection<BrushMap>
    {
        public BrushMapCollection() { }

        public BrushMapCollection(IEnumerable<BrushMap> maps) : base(maps) { }

        public BrushMapCollection(List<BrushMap> maps) : base(maps) { }

        public Brush GetBrushFromKey(object key)
        {
            return this.FirstOrDefault(x => ValueComparer.IsEqualTo(x.Key, key))?.Brush;
        }
    }
}