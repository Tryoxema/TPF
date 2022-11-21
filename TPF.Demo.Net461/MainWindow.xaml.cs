using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TPF.Demo.Net461.Controls;
using TPF.Controls;
using TPF.Skins;

namespace TPF.Demo.Net461
{
    public partial class MainWindow : ChromelessWindow, System.ComponentModel.INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            Instance = this;

            Manager = new NotificationManager();
            Notifications.Manager = Manager;

            SearchManager.Create();

            Skins.Add(new ColorSkin(VS2013LightSkin.Instance, "Hell"));
            Skins.Add(new ColorSkin(VS2013DarkSkin.Instance, "Dunkel"));
            DesignComboBox.SelectedItem = Skins.FirstOrDefault();

            DataBarTests.Add(new DataBarTest(5));
            DataBarTests.Add(new DataBarTest(15));
            DataBarTests.Add(new DataBarTest(5));
            DataBarTests.Add(new DataBarTest(3));
            DataBarTests.Add(new DataBarTest(18));
            DataBarTests.Add(new DataBarTest(22));
            DataBarTests.Add(new DataBarTest(10));
            DataBarTests.Add(new DataBarTest(5));
            DataBarTests.Add(new DataBarTest(9));
            DataBarTests.Add(new DataBarTest(6));

            var random = new Random();

            for (int i = 0; i < 20; i++)
            {
                SparklineTests.Add(new SparklineTest(i, random.Next(-10, 10)));
            }
        }

        #region SetProperty
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            return true;
        }
        #endregion

        public static MainWindow Instance { get; private set; }

        public readonly NotificationManager Manager;

        int? _badgeCounter;
        public int? BadgeCounter
        {
            get { return _badgeCounter; }
            set
            {
                if (SetProperty(ref _badgeCounter, value))
                {
                    if (value == null) BadgeVisibility = Visibility.Collapsed;
                    else BadgeVisibility = Visibility.Visible;
                }
            }
        }

        Visibility _badgeVisibility = Visibility.Collapsed;
        public Visibility BadgeVisibility
        {
            get { return _badgeVisibility; }
            set { SetProperty(ref _badgeVisibility, value); }
        }

        ObservableCollection<ColorSkin> _skins;
        public ObservableCollection<ColorSkin> Skins
        {
            get { return _skins ?? (_skins = new ObservableCollection<ColorSkin>()); }
        }

        ObservableCollection<DataBarTest> _dataBarTests;
        public ObservableCollection<DataBarTest> DataBarTests
        {
            get { return _dataBarTests ?? (_dataBarTests = new ObservableCollection<DataBarTest>()); }
        }

        ObservableCollection<SparklineTest> _sparklineTests;
        public ObservableCollection<SparklineTest> SparklineTests
        {
            get { return _sparklineTests ?? (_sparklineTests = new ObservableCollection<SparklineTest>()); }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowFactory.CloseAllWindows();
        }

        private void DesignComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var skin = (SkinBase)DesignComboBox.SelectedValue;
            ResourceManager.ChangeSkin(skin);
        }

        private void SearchTextBox_Searching(object sender, SearchingEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Value))
            {
                var results = SearchManager.Search(e.Value);

                e.Results = results;
            }
        }

        private void SearchTextBox_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            var result = (SearchResult)e.SelectedItem;
            WindowFactory.CreateWindow(result.InternalName);
        }

        private void ReceiveButton_Click(object sender, RoutedEventArgs e)
        {
            var count = new Random().Next(0, 10);
            if (BadgeCounter == null) BadgeCounter = count == 0 ? (int?)null : count;
            else BadgeCounter += count;

            Manager.CreateNotification()
                .Header("Empfangen")
                .Message($"Es wurden {count} neue Nachrichten empfangen")
                .Dismiss().WithButton("Ja und?")
                .Dismiss().WithButton(new NotificationButtonConfiguration() { Content = "X", Background = Brushes.Crimson })
                .Queue();
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            if (BadgeCounter == null) return;

            Manager.CreateNotification()
                .Header("Gelesen")
                .Message($"Es wurden {BadgeCounter} Nachrichten gelesen")
                .UseAnimation(true)
                .AnimationInDuration(0.5)
                .AnimationOutDuration(1)
                .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                .Dismiss().WithButton("OK")
                .Queue();

            BadgeCounter = null;
        }
    }

    public class ColorSkin
    {
        public string Name { get; set; }

        public SkinBase Skin { get; set; }

        public ColorSkin(SkinBase skin) : this(skin, skin.Name) { }

        public ColorSkin(SkinBase skin, string name)
        {
            Name = name;
            Skin = skin;
        }
    }

    public class DataBarTest : NotifyObject
    {
        public DataBarTest(double value)
        {
            Value = value;
            Name = "Test";
        }

        double _value;
        public double Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    }

    public class SparklineTest : NotifyObject
    {
        public SparklineTest(double x, double y)
        {
            X = x;
            Y = y;
        }

        double _x;
        public double X
        {
            get { return _x; }
            set { SetProperty(ref _x, value); }
        }

        double _y;
        public double Y
        {
            get { return _y; }
            set { SetProperty(ref _y, value); }
        }
    }
}