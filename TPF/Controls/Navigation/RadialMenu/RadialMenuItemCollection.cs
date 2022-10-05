using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TPF.Controls
{
    public class RadialMenuItemCollection : ObservableCollection<RadialMenuItem>
    {
        public RadialMenuItemCollection() { }

        public RadialMenuItemCollection(IEnumerable<RadialMenuItem> items) : base(items) { }

        public RadialMenuItemCollection(List<RadialMenuItem> items) : base(items) { }
    }
}