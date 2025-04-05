using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TPF.Demo.Views;
using TPF.Collections;
using TPF.Controls;
using TPF.Skins;

namespace TPF.Demo
{
    public partial class MainWindow : ChromelessWindow, System.ComponentModel.INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            Initialize();
        }

        #region SetProperty
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            return true;
        }
        #endregion

        public ObservableCollection<ColorSkin> Skins { get; } = new ObservableCollection<ColorSkin>();

        public RangeObservableCollection<DemoItem> DemoItems { get; } = new RangeObservableCollection<DemoItem>();

        private readonly Dictionary<string, Controls.TabItem> _existingTabItems = new Dictionary<string, Controls.TabItem>();

        private void Initialize()
        {
            Skins.Add(new ColorSkin(VS2013LightSkin.Instance, "VS2013 Hell"));
            Skins.Add(new ColorSkin(VS2013DarkSkin.Instance, "VS2013 Dunkel"));
            Skins.Add(new ColorSkin(SmoothLightSkin.Instance, "Smooth Hell"));
            Skins.Add(new ColorSkin(SmoothDarkSkin.Instance, "Smooth Dunkel"));

            DesignComboBox.SelectedItem = Skins.FirstOrDefault();

            var demoItems = new List<DemoItem>()
            {
                new DemoItem("DataBar", "DataVisualization", () => CreateTabItem("DataBar", typeof(DataBarDemoView))),
                new DemoItem("Sparkline", "DataVisualization", () => CreateTabItem("Sparkline", typeof(SparklineDemoView))),
                
                new DemoItem("FluentControl", "Design", () => CreateTabItem("FluentControl", typeof(FluentControlDemoView))),
                new DemoItem("MaterialControl", "Design", () => CreateTabItem("MaterialControl", typeof(MaterialControlDemoView))),
                new DemoItem("Shadow", "Design", () => CreateTabItem("Shadow", typeof(ShadowDemoView))),

                new DemoItem("Calculator", "Input", () => CreateTabItem("Calculator", typeof(CalculatorDemoView))),
                new DemoItem("ColorEditor", "Input", () => CreateTabItem("ColorEditor", typeof(ColorEditorDemoView))),
                new DemoItem("HighlightingTextBlock", "Input", () => CreateTabItem("HighlightingTextBlock", typeof(HighlightingTextBlockDemoView))),
                new DemoItem("NumericTextBox", "Input", () => CreateTabItem("NumericTextBox", typeof(NumericTextBoxDemoView))),
                new DemoItem("PasswordBox", "Input", () => CreateTabItem("PasswordBox", typeof(PasswordBoxDemoView))),
                new DemoItem("Rating", "Input", () => CreateTabItem("Rating", typeof(RatingDemoView))),
                new DemoItem("SearchTextBox", "Input", () => CreateTabItem("SearchTextBox", typeof(SearchTextBoxDemoView))),
                new DemoItem("Slider", "Input", () => CreateTabItem("Slider", typeof(SliderDemoView))),
                new DemoItem("WatermarkTextBox", "Input", () => CreateTabItem("WatermarkTextBox", typeof(WatermarkTextBoxDemoView))),

                new DemoItem("Badge", "Interaction", () => CreateTabItem("Badge", typeof(BadgeDemoView))),
                new DemoItem("BusyIndicator", "Interaction", () => CreateTabItem("BusyIndicator", typeof(BusyIndicatorDemoView))),
                new DemoItem("Buttons", "Interaction", () => CreateTabItem("Buttons", typeof(ButtonsDemoView))),
                new DemoItem("Comparer", "Interaction", () => CreateTabItem("Comparer", typeof(ComparerDemoView))),
                new DemoItem("DialogHost", "Interaction", () => CreateTabItem("DialogHost", typeof(DialogHostDemoView))),
                new DemoItem("DragDrop", "Interaction", () => CreateTabItem("DragDrop", typeof(DragDropDemoView))),
                new DemoItem("Notification", "Interaction", () => CreateTabItem("Notification", typeof(NotificationDemoView))),
                new DemoItem("Poptip", "Interaction", () => CreateTabItem("Poptip", typeof(PoptipDemoView))),
                new DemoItem("ProgressBar", "Interaction", () => CreateTabItem("ProgressBar", typeof(ProgressBarDemoView))),
                new DemoItem("StepProgressBar", "Interaction", () => CreateTabItem("StepProgressBar", typeof(StepProgressBarDemoView))),

                new DemoItem("Dashboard", "Layout", () => CreateTabItem("Dashboard", typeof(DashboardDemoView))),
                new DemoItem("Divider", "Layout", () => CreateTabItem("Divider", typeof(DividerDemoView))),
                new DemoItem("SplitView", "Layout", () => CreateTabItem("SplitView", typeof(SplitViewDemoView))),

                new DemoItem("Banner", "Misc", () => CreateTabItem("Banner", typeof(BannerDemoView))),
                new DemoItem("RevealBrush", "Misc", () => CreateTabItem("RevealBrush", typeof(RevealBrushDemoView))),
                new DemoItem("Shield", "Misc", () => CreateTabItem("Shield", typeof(ShieldDemoView))),
                new DemoItem("Skeleton", "Misc", () => CreateTabItem("Skeleton", typeof(SkeletonDemoView))),
                new DemoItem("SplashScreen", "Misc", () => CreateTabItem("SplashScreen", typeof(SplashScreenDemoView))),

                new DemoItem("HamburgerMenu", "Navigation", () => CreateTabItem("HamburgerMenu", typeof(HamburgerMenuDemoView))),
                new DemoItem("TabControl", "Navigation", () => CreateTabItem("TabControl", typeof(TabControlDemoView))),
                new DemoItem("Wizard", "Navigation", () => CreateTabItem("Wizard", typeof(WizardDemoView))),

                new DemoItem("Calendar", "Scheduling", () => CreateTabItem("Calendar", typeof(CalendarDemoView))),
                new DemoItem("Clock", "Scheduling", () => CreateTabItem("Clock", typeof(ClockDemoView))),
                new DemoItem("DateTimePicker", "Scheduling", () => CreateTabItem("DateTimePicker", typeof(DateTimePickerDemoView))),
                new DemoItem("DateTimeRangeNavigator", "Scheduling", () => CreateTabItem("DateTimeRangeNavigator", typeof(DateTimeRangeNavigatorDemoView))),
                new DemoItem("TaskBoard", "Scheduling", () => CreateTabItem("TaskBoard", typeof(TaskBoardDemoView))),
            };

            DemoItems.AddRange(demoItems.OrderBy(x => x.Name));
        }

        private void CreateTabItem(string key, Type contentType)
        {
            if (_existingTabItems.TryGetValue(key, out var tabItem))
            {
                HostTabControl.SelectedItem = tabItem;
            }
            else
            {
                var content = Activator.CreateInstance(contentType);

                tabItem = new Controls.TabItem()
                {
                    Header = key,
                    Tag = key,
                    Content = content
                };

                _existingTabItems[key] = tabItem;

                HostTabControl.Items.Add(tabItem);
                HostTabControl.SelectedItem = tabItem;
            }
        }

        private void HostTabControl_Closed(object sender, ClosedEventArgs<Controls.TabItem> e)
        {
            if (e.Item.Tag is string key)
            {
                _existingTabItems.Remove(key);
            }
        }

        private void DemoItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Controls.Button)sender;

            if (button.DataContext is DemoItem demoItem)
            {
                demoItem.OpenDemo?.Invoke();
            }
        }

        #region StatusBar
        private void DesignComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var skin = (SkinBase)DesignComboBox.SelectedValue;

            ResourceManager.ChangeSkin(skin);
        }

        private void OpenSkinEditorWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new TPF.Demo.Windows.SkinEditorWindow()
            {
                Owner = this
            };

            window.Show();
        }

        private void SmoothThemeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var dictionary = Application.Current.Resources.MergedDictionaries.First();

            dictionary.MergedDictionaries.Remove(dictionary.MergedDictionaries.Last());

            dictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/TPF.Demo;component/Themes/SmoothStyles.xaml", UriKind.RelativeOrAbsolute) });

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        private void SmoothThemeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var dictionary = Application.Current.Resources.MergedDictionaries.First();

            dictionary.MergedDictionaries.Remove(dictionary.MergedDictionaries.Last());

            dictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/TPF.Demo;component/Themes/VS2013Styles.xaml", UriKind.RelativeOrAbsolute) });

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }
        #endregion
    }

    public class ColorSkin
    {
        public ColorSkin(SkinBase skin, string name)
        {
            Name = name;
            Skin = skin;
        }

        public string Name { get; }

        public SkinBase Skin { get; }
    }

    public class DemoItem
    {
        public DemoItem(string name, string group, Action openDemo)
        {
            Name = name;
            Group = group;
            OpenDemo = openDemo;
        }

        public string Name { get; set; }

        public string Group { get; set; }

        public Action OpenDemo { get; set; }
    }
}