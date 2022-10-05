using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using TPF.Controls;
using TPF.Collections;
using TPF.Controls.Specialized.TaskBoard;

namespace TPF.Demo.Net461.Views
{
    public partial class TaskBoardView : UserControl
    {
        public TaskBoardView()
        {
            InitializeComponent();

            Initialize();
        }

        ObservableCollection<string> _tags;
        public ObservableCollection<string> Tags
        {
            get { return _tags ?? (_tags = new ObservableCollection<string>()); }
        }

        RangeObservableCollection<ITaskBoardCardData> _cards;
        public RangeObservableCollection<ITaskBoardCardData> Cards
        {
            get { return _cards ?? (_cards = new RangeObservableCollection<ITaskBoardCardData>()); }
        }

        BrushMapCollection _colorPallet;
        public BrushMapCollection ColorPallet
        {
            get { return _colorPallet ?? (_colorPallet = new BrushMapCollection()); }
        }

        private void Initialize()
        {
            Tags.Add("Tag 1");
            Tags.Add("Tag 2");

            ColorPallet.Add(new BrushMap() { Key = "Feature", Brush = Brushes.DodgerBlue });
            ColorPallet.Add(new BrushMap() { Key = "Bug", Brush = Brushes.Crimson });
            ColorPallet.Add(new BrushMap() { Key = "Service", Brush = Brushes.ForestGreen });
            ColorPallet.Add(new BrushMap() { Key = "Internal", Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5B436")) });

            Cards.Clear();

            var categories = new List<string>()
            {
                "Planned",
                "Working",
                "Testing",
                "Done"
            };

            var random = new Random();

            for (int i = 0; i < 10; i++)
            {
                var data = new TaskBoardCardData()
                {
                    Id = i,
                    Assignee = "None",
                    Title = $"Task {i + 1}",
                    Description = $"Dies ist die Aufgabe Nummer {i + 1}",
                    State = categories[random.Next(0, categories.Count)],
                    Type = ColorPallet[random.Next(0, ColorPallet.Count)].Key
                };

                if (random.Next(2) > 0)
                {
                    var count = random.Next(3);

                    var tags = new ObservableCollection<string>();

                    for (int j = 0; j < count; j++)
                    {
                        tags.Add($"Tag {j + 1}");
                    }

                    data.Tags = tags;
                }

                Cards.Add(data);
            }
        }
    }
}