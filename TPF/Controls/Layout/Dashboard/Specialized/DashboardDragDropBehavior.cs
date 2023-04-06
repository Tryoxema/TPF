using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TPF.DragDrop.Behaviors;
using TPF.Internal;

namespace TPF.Controls.Specialized.Dashboard
{
    public class DashboardDragDropBehavior : DragDropBehavior<DragDropState>
    {
        private double _originalOpacity = -1;

        private Point _dragStartPoint;

        private readonly Dictionary<Widget, DashboardSlot> _movedWidgetOriginalPositions = new Dictionary<Widget, DashboardSlot>();

        public override bool CanStartDrag(DragDropState state)
        {
            var item = state.DraggedItems.OfType<Widget>().FirstOrDefault();

            var canStartDrag = item?.Dashboard.AllowDrag == true;

            _dragStartPoint = Mouse.GetPosition(item);

            return canStartDrag;
        }

        public override bool CanDrop(DragDropState state)
        {
            if (state.DraggedItems == null) return false;

            var item = state.DraggedItems.OfType<Widget>().FirstOrDefault();

            // Die Abfrage zum Unsichtbar machen darf erst nach Anfang des Vorgangs gemacht werden, weil der DragAdorner sonst auch unsichtbar ist
            if (item != null && _originalOpacity < 0 && ShouldHideDraggedItem(state))
            {
                _originalOpacity = item.Opacity;
                item.Opacity = 0;
            }

            if (item?.Dashboard != null)
            {
                var dashboard = item.Dashboard;

                var panel = dashboard.ChildOfType<DashboardPanel>();

                if (panel != null)
                {
                    MoveDashboardItems(state, dashboard, panel, item);
                }
            }

            return true;
        }

        public override void DragDropCanceled(DragDropState state)
        {
            var item = state.DraggedItems.OfType<Widget>().FirstOrDefault();

            if (item != null)
            {
                if (_originalOpacity >= 0)
                {
                    item.Opacity = _originalOpacity;
                    _originalOpacity = -1;
                }

                item.Dashboard?.CancelEdit();
            }

            CleanUp(state);

            base.DragDropCanceled(state);
        }

        public override void DragDropCompleted(DragDropState state)
        {
            var item = state.DraggedItems.OfType<Widget>().FirstOrDefault();

            if (item != null)
            {
                if (_originalOpacity >= 0)
                {
                    item.Opacity = _originalOpacity;
                    _originalOpacity = -1;
                }

                item.Dashboard?.CommitEdit();
            }

            CleanUp(state);

            if (!ShouldRemoveItemsFromSource(state)) return;

            if (item == null) return;

            state.SourceItemsSource.Remove(item.Content);
        }

        private void CleanUp(DragDropState state)
        {
            var item = state.DraggedItems.OfType<Widget>().FirstOrDefault();

            if (item?.Dashboard != null) item.Dashboard.DraggingWidget = null;

            _dragStartPoint = new Point();
            _movedWidgetOriginalPositions.Clear();
        }

        protected virtual bool ShouldHideDraggedItem(DragDropState state)
        {
            return true;
        }

        private void MoveDashboardItems(DragDropState state, Controls.Dashboard dashboard, DashboardPanel panel, Widget item)
        {
            if (!dashboard.IsEditing) dashboard.BeginEdit();

            dashboard.DraggingWidget = item;

            var dashboardPoint = state.GetPosition(dashboard);

            if (dashboardPoint.X < 0 || dashboardPoint.Y < 0 || dashboardPoint.X > dashboard.ActualWidth || dashboardPoint.Y > dashboard.ActualHeight) return;

            var panelPoint = state.GetPosition(panel);

            // Den Slot berechnen den die Maus gerade belegt
            var horizontalSlot = (int)(panelPoint.X / (dashboard.SlotWidth + dashboard.Gap));
            var verticalSlot = (int)(panelPoint.Y / (dashboard.SlotHeight + dashboard.Gap));

            // Den Slot innerhalb des Widgets bestimmen in dem der Vorgang gestartet wurde
            var horizontalItemMouseSlot = (int)(_dragStartPoint.X / (dashboard.SlotWidth + dashboard.Gap));
            var verticalItemMouseSlot = (int)(_dragStartPoint.Y / (dashboard.SlotHeight + dashboard.Gap));

            var top = Math.Max(0, verticalSlot - verticalItemMouseSlot);
            var left = Math.Max(0, horizontalSlot - horizontalItemMouseSlot);
            // Falls sich unserer Slot nicht geändert hat, machen wir auch nichts
            if (left == item.Left && top == item.Top) return;

            var matrix = panel.LastMatrix;

            // Wir verlassen uns hier auf die letzte Anordnungs-Matrix des Panels um nicht alles selber nochmal auswerten zu müssen
            if (matrix == null) return;

            var oldSlot = new DashboardSlot(item);
            var targetSlot = new DashboardSlot(top, left, item.HorizontalSlots, item.VerticalSlots);

            var blockingWidgets = new HashSet<Widget>();

            // Das bewegte Widget aus der Matrix entfernen, damit der Platz als frei angesehen wird
            SetMatrixSlot(ref matrix, oldSlot, null);

            EnsureMatrixLargeEnough(ref matrix, targetSlot);

            // Ermitteln ob neue Position bereits belegt ist
            for (int x = targetSlot.Left; x <= targetSlot.Right; x++)
            {
                for (int y = targetSlot.Top; y <= targetSlot.Bottom; y++)
                {
                    if (matrix[x, y] != null)
                    {
                        blockingWidgets.Add(matrix[x, y]);
                    }
                }
            }

            if (blockingWidgets.Count > 0)
            {
                // Zuerst alle Widgets die wir jetzt bewegen müssen aus der Matrix entfernen
                foreach (var widget in blockingWidgets)
                {
                    var slot = new DashboardSlot(widget);

                    SetMatrixSlot(ref matrix, slot, null);

                    if (!_movedWidgetOriginalPositions.ContainsKey(widget)) _movedWidgetOriginalPositions.Add(widget, slot);
                }
            }

            // Bewegtes Widget auf Position setzen
            item.SetPosition(top, left);
            SetMatrixSlot(ref matrix, targetSlot, item);

            if (blockingWidgets.Count > 0)
            {
                foreach (var widget in blockingWidgets)
                {
                    var movementQueue = new Queue<MovingDirection>(4);

                    movementQueue.Enqueue(MovingDirection.Left);
                    movementQueue.Enqueue(MovingDirection.Right);
                    movementQueue.Enqueue(MovingDirection.Up);
                    movementQueue.Enqueue(MovingDirection.Down);

                    var foundSlot = false;

                    while (movementQueue.Count > 0)
                    {
                        var currentDirection = movementQueue.Dequeue();

                        var slot = new DashboardSlot(widget);

                        switch (currentDirection)
                        {
                            case MovingDirection.Left:
                            {
                                slot.Left--;
                                break;
                            }
                            case MovingDirection.Right:
                            {
                                slot.Left++;
                                break;
                            }
                            case MovingDirection.Up:
                            {
                                slot.Top--;
                                break;
                            }
                            case MovingDirection.Down:
                            {
                                slot.Top++;
                                break;
                            }
                        }

                        if (slot.Left < 0) slot.Left = 0;
                        if (slot.Top < 0) slot.Top = 0;

                        if (IsSlotFree(ref matrix, slot))
                        {
                            widget.SetPosition(slot.Top, slot.Left);
                            SetMatrixSlot(ref matrix, slot, widget);
                            foundSlot = true;
                            break;
                        }
                    }

                    // Wenn das Widget in keine der 4 Richtungen geschoben werden konnte, wird es vom Panel komplett neu positioniert
                    if (!foundSlot) widget.InvalidPosition = true;
                }
            }

            // Nach dem Verschieben bei allen bis dann verschobenen Widgets testen, ob sie auf ihre Originalposition zurück können
            var originalPositions = _movedWidgetOriginalPositions.ToList();

            // Der Vorgang muss rückwärts durchgeführt werden weil die Slots sich dann gegenseitig wieder freigeben können
            for (var i = originalPositions.Count; i > 0; i--)
            {
                var pair = originalPositions[i-1];

                var widget = pair.Key;

                var originalSlot = pair.Value;

                if (IsSlotFree(ref matrix, originalSlot, widget))
                {
                    var currentSlot = new DashboardSlot(widget);

                    // Aktuellen Slot leeren
                    SetMatrixSlot(ref matrix, currentSlot, null);
                    // Alten Slot füllen
                    SetMatrixSlot(ref matrix, originalSlot, widget);
                    // Widget wieder auf Position setzen
                    widget.SetPosition(originalSlot.Top, originalSlot.Left);
                    // Widget ist jetzt wieder auf Originalposition, kann also aus der Liste raus
                    _movedWidgetOriginalPositions.Remove(widget);
                }
            }

            dashboard.InvalidateWidgets();
        }

        private static void SetMatrixSlot(ref Widget[,] matrix, DashboardSlot slot, Widget widget)
        {
            EnsureMatrixLargeEnough(ref matrix, slot);

            for (int x = slot.Left; x <= slot.Right; x++)
            {
                for (int y = slot.Top; y <= slot.Bottom; y++)
                {
                    matrix[x, y] = widget;
                }
            }
        }

        private static bool IsSlotFree(ref Widget[,] matrix, DashboardSlot slot, Widget widgetToIgnore = null)
        {
            EnsureMatrixLargeEnough(ref matrix, slot);

            for (int x = slot.Left; x <= slot.Right; x++)
            {
                for (int y = slot.Top; y <= slot.Bottom; y++)
                {
                    if (matrix[x, y] != null && matrix[x, y] != widgetToIgnore) return false;
                }
            }

            return true;
        }

        private static void EnsureMatrixLargeEnough(ref Widget[,] matrix, DashboardSlot slot)
        {
            var matrixWidth = matrix.GetLength(0);
            var matrixHeight = matrix.GetLength(1);

            int widthIncrease = 0, heightIncrease = 0;

            if (matrixWidth < slot.Right + 1) widthIncrease = slot.Right + 1 - matrixWidth;
            if (matrixHeight < slot.Bottom + 1) heightIncrease = slot.Bottom + 1 - matrixHeight;

            if (widthIncrease > 0 || heightIncrease > 0) matrix = ArrayHelper.CreateLargerCopy(matrix, widthIncrease, heightIncrease);
        }

        private enum MovingDirection
        {
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3
        }
    }
}