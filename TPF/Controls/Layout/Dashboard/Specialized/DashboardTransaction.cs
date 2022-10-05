using System;
using System.Collections.Generic;

namespace TPF.Controls.Specialized.Dashboard
{
    internal class DashboardTransaction
    {
        public DashboardTransaction(Controls.Dashboard dashboard)
        {
            Dashboard = dashboard;

            var originalWidgetPositions = new Dictionary<object, Tuple<int, int>>(Dashboard.Items.Count);

            for (int i = 0; i < Dashboard.Items.Count; i++)
            {
                var item = Dashboard.Items[i];

                Widget widget;

                if (item is Widget)
                {
                    widget = item as Widget;
                }
                else
                {
                    widget = Dashboard.ItemContainerGenerator.ContainerFromItem(item) as Widget;
                }

                if (widget == null) continue;

                var position = new Tuple<int, int>(widget.Top, widget.Left);

                originalWidgetPositions.Add(item, position);
            }

            _originalWidgetPositions = originalWidgetPositions;
        }

        public Controls.Dashboard Dashboard { get; private set; }

        public bool IsBulkEditing { get; private set; }

        private Dictionary<object, Tuple<int, int>> _originalWidgetPositions;

        public void Cancel()
        {
            IsBulkEditing = true;

            for (int i = 0; i < Dashboard.Items.Count; i++)
            {
                var item = Dashboard.Items[i];

                Widget widget;

                if (item is Widget)
                {
                    widget = item as Widget;
                }
                else
                {
                    widget = Dashboard.ItemContainerGenerator.ContainerFromItem(item) as Widget;
                }

                if (widget == null) continue;

                if (_originalWidgetPositions.TryGetValue(item, out var position))
                {
                    widget.Top = position.Item1;
                    widget.Left = position.Item2;
                }
            }

            IsBulkEditing = false;
        }
    }
}