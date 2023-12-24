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

            Initialize();
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

        #region Properties
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

        double _sliderValue;
        public double SliderValue
        {
            get { return _sliderValue; }
            set { SetProperty(ref _sliderValue, value); }
        }

        ObservableCollection<ColorSkin> _skins;
        public ObservableCollection<ColorSkin> Skins
        {
            get { return _skins ?? (_skins = new ObservableCollection<ColorSkin>()); }
        }
        #endregion

        private void Initialize()
        {
            Notifications.Manager = Manager;

            SearchManager.Create();

            Skins.Add(new ColorSkin(VS2013LightSkin.Instance, "VS2013 Hell"));
            Skins.Add(new ColorSkin(VS2013DarkSkin.Instance, "VS2013 Dunkel"));
            Skins.Add(new ColorSkin(SmoothLightSkin.Instance, "Smooth Hell"));
            Skins.Add(new ColorSkin(SmoothDarkSkin.Instance, "Smooth Dunkel"));
            DesignComboBox.SelectedItem = Skins.FirstOrDefault();
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

        private void SmoothThemeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var dictionary = Application.Current.Resources.MergedDictionaries.First();

            dictionary.MergedDictionaries.Remove(dictionary.MergedDictionaries.Last());

            dictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/TPF.Demo.Net461;component/Themes/SmoothStyles.xaml", UriKind.RelativeOrAbsolute) });

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        private void SmoothThemeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var dictionary = Application.Current.Resources.MergedDictionaries.First();

            dictionary.MergedDictionaries.Remove(dictionary.MergedDictionaries.Last());

            dictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/TPF.Demo.Net461;component/Themes/VS2013Styles.xaml", UriKind.RelativeOrAbsolute) });

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        private void ShowSplashScreenButton_Click(object sender, RoutedEventArgs e)
        {
            var data = SplashScreenManager.CreateDataContext();

            data.Title = "SplashScreen";
            data.SubTitle = "Jetzt neu";
            data.StatusText = "Anfrage wird ignoriert...";
            data.Footer = "Das liest doch eh keiner";

            SplashScreenManager.DataContext = data;
            SplashScreenManager.Show();
        }

        private void CloseSplashScreenButton_Click(object sender, RoutedEventArgs e)
        {
            SplashScreenManager.Close();
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
}