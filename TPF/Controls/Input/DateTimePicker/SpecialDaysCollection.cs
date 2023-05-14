using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TPF.Controls
{
    public class SpecialDaysCollection : ObservableCollection<SpecialDay>
    {
        public SpecialDaysCollection() { }

        public SpecialDaysCollection(IEnumerable<SpecialDay> days) : base(days) { }

        public SpecialDaysCollection(List<SpecialDay> days) : base(days) { }
    }
}