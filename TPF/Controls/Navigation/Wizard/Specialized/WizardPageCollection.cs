using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TPF.Controls.Specialized.Wizard
{
    public class WizardPageCollection : ObservableCollection<WizardPage>
    {
        public WizardPageCollection() { }

        public WizardPageCollection(IEnumerable<WizardPage> pages) : base(pages) { }

        public WizardPageCollection(List<WizardPage> pages) : base(pages) { }
    }
}