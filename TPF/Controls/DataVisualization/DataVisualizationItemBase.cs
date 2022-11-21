using System.Collections.Generic;
using System.ComponentModel;

namespace TPF.Controls
{
    public abstract class DataVisualizationItemBase : NotifyObject
    {
        protected DataVisualizationItemBase()
        {
            _propertyPathMappings = new Dictionary<string, HashSet<string>>();
        }

        private readonly Dictionary<string, HashSet<string>> _propertyPathMappings;

        private object _dataItem;
        public object DataItem
        {
            get { return _dataItem; }
            set
            {
                if (_dataItem is INotifyPropertyChanged oldItem) oldItem.PropertyChanged -= DataItem_PropertyChanged;

                _dataItem = value;

                if (_dataItem is INotifyPropertyChanged newItem) newItem.PropertyChanged += DataItem_PropertyChanged;
            }
        }

        private void DataItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(sender, e);

            foreach (var mapping in _propertyPathMappings)
            {
                var propertyPath = mapping.Key;

                var isComplexPath = propertyPath.Contains(".") || propertyPath.Contains("[");

                if ((isComplexPath && propertyPath.StartsWith(e.PropertyName)) || propertyPath == e.PropertyName)
                {
                    foreach (var propertyName in mapping.Value)
                    {
                        OnPropertyChanged(propertyName);
                    }
                }
            }
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e) { }

        protected void RegisterPropertyMapping(string propertyPath, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyPath)) return;

            if (_propertyPathMappings.TryGetValue(propertyPath, out var propertyNames)) propertyNames.Add(propertyName);
            else _propertyPathMappings.Add(propertyPath, new HashSet<string>() { propertyName });
        }

        protected void UnregisterPropertyMapping(string propertyPath, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyPath)) return;

            if (!_propertyPathMappings.ContainsKey(propertyPath)) return;

            var propertyNames = _propertyPathMappings[propertyPath];

            propertyNames.Remove(propertyName);

            if (propertyNames.Count == 0) _propertyPathMappings.Remove(propertyPath);
        }

        protected object GetValueFromPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || DataItem == null) return DataItem;

            var accessExpression = MemberAccessExpressionCache.GetMemberAccessExpression(DataItem.GetType(), path);

            var value = accessExpression(DataItem);

            return value;
        }
    }
}