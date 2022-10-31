using System.ComponentModel;

namespace TPF.Controls
{
    public abstract class DataVisualizationItemBase : NotifyObject
    {
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
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e) { }

        protected object GetValueFromPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || DataItem == null) return DataItem;

            var accessExpression = MemberAccessExpressionCache.GetMemberAccessExpression(DataItem.GetType(), path);

            var value = accessExpression(DataItem);

            return value;
        }
    }
}