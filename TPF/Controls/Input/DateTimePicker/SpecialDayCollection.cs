using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TPF.Controls
{
    public class SpecialDayCollection : ObservableCollection<SpecialDay>
    {
        public SpecialDayCollection() { }

        public SpecialDayCollection(IEnumerable<SpecialDay> days) : base(days) { }

        public SpecialDayCollection(List<SpecialDay> days) : base(days) { }
    }
}