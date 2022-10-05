using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace TPF.Demo.Net461.Views
{
    public partial class SearchTextBoxView : UserControl
    {
        public SearchTextBoxView()
        {
            InitializeComponent();

            FillDemoData();
        }

        ObservableCollection<string> _demoData;
        public ObservableCollection<string> DemoData
        {
            get { return _demoData ?? (_demoData = new ObservableCollection<string>()); }
        }

        private void FillDemoData()
        {
            DemoData.Add("Steve");
            DemoData.Add("John");
            DemoData.Add("Pablo Jesus Esteban Maria Eduardo Mercedes");
            DemoData.Add("Dave");
            DemoData.Add("Martin");
            DemoData.Add("Jane");
            DemoData.Add("Nancy");
            DemoData.Add("Paul");
            DemoData.Add("Daisy");
            DemoData.Add("Mike");
            DemoData.Add("Laura");
            DemoData.Add("Anna");
            DemoData.Add("Nico");
            DemoData.Add("Eric");
            DemoData.Add("Mathew");
            DemoData.Add("Theodore");
            DemoData.Add("Jack");
        }
    }
}