using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TPF.Demo.Views
{
    public class ViewBase : ContentControl, System.ComponentModel.INotifyPropertyChanged
    {
        static ViewBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewBase), new FrameworkPropertyMetadata(typeof(ViewBase)));
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

        #region Settings DependencyProperty
        public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register("Settings",
            typeof(object),
            typeof(ViewBase),
            new PropertyMetadata(null));

        public object Settings
        {
            get { return GetValue(SettingsProperty); }
            set { SetValue(SettingsProperty, value); }
        }
        #endregion
    }
}