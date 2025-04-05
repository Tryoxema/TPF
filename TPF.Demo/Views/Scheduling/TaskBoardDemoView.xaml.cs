using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using TPF.Controls;
using TPF.Controls.Specialized.TaskBoard;
using TPF.Collections;

namespace TPF.Demo.Views
{
    public partial class TaskBoardDemoView : ViewBase
    {
        public TaskBoardDemoView()
        {
            InitializeComponent();

            Initialize();
        }

        public ObservableCollection<string> Tags { get; } = new ObservableCollection<string>();

        public RangeObservableCollection<ITaskBoardCardData> Cards { get; } = new RangeObservableCollection<ITaskBoardCardData>();

        public BrushMapCollection ColorPallet { get; } = new BrushMapCollection();

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

            for (int i = 0; i < 15; i++)
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