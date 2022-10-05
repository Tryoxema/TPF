using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TPF.Controls
{
    public class SpecialDatesCollection : ObservableCollection<SpecialDate>
    {
        public SpecialDatesCollection() { }

        public SpecialDatesCollection(IEnumerable<SpecialDate> dates) : base(dates) { }

        public SpecialDatesCollection(List<SpecialDate> dates) : base(dates) { }
    }
}