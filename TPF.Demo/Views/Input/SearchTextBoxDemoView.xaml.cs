using System.Collections.ObjectModel;

namespace TPF.Demo.Views
{
    public partial class SearchTextBoxDemoView : ViewBase
    {
        public SearchTextBoxDemoView()
        {
            InitializeComponent();

            FillDemoData();
        }

        public ObservableCollection<string> DemoData { get; } = new ObservableCollection<string>();

        private void FillDemoData()
        {
            DemoData.Add("Steve");
            DemoData.Add("John");
            DemoData.Add("Pablo Jesus Esteban Maria Eduardo Mercedes");
            DemoData.Add("Dave");
            DemoData.Add("Martin");
            DemoData.Add("James Bond");
            DemoData.Add("Jane");
            DemoData.Add("Nancy");
            DemoData.Add("Paul");
            DemoData.Add("Daisy");
            DemoData.Add("Mike");
            DemoData.Add("Laura");
            DemoData.Add("Anna");
            DemoData.Add("Nico");
            DemoData.Add("Eric");
            DemoData.Add("Alex");
            DemoData.Add("Mathew");
            DemoData.Add("Theodore");
            DemoData.Add("Jack");
        }
    }
}