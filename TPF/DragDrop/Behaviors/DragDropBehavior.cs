using System;
using System.Collections;
using System.Linq;
using System.Windows;
using TPF.Internal;

namespace TPF.DragDrop.Behaviors
{
    public abstract class DragDropBehavior<TState> : DependencyObject where TState : DragDropState
    {
        public bool AllowReorder { get; set; } = true;

        public virtual bool CanStartDrag(TState state)
        {
            return true;
        }

        public virtual bool CanDrop(TState state)
        {
            if (state.DraggedItems == null) return false;

            if (state.TargetItemsSource != null)
            {
                var draggedType = TypeHelper.GetIEnumerableType(state.DraggedItems);

                if (draggedType == typeof(object))
                {
                    foreach (var item in state.DraggedItems)
                    {
                        draggedType = item.GetType();
                        break;
                    }
                }

                var targetType = TypeHelper.GetIListType(state.TargetItemsSource);

                if (targetType != null && !targetType.IsAssignableFrom(draggedType)) return false;
            }

            return true;
        }

        public virtual void DragDropCanceled(TState state)
        {

        }

        public virtual void Drop(TState state)
        {
            if (state.DraggedItems == null) return;

            // Sind Source und Target gleich?
            if (state.SourceControl == state.TargetControl)
            {
                // Wenn bewegen in der Collection erlaubt ist, dann Items bewegen
                if (AllowReorder) MoveItems(state.DraggedItems, state.TargetItemsSource, state.InsertIndex);
            }
            else
            {
                InsertItems(state.DraggedItems, state.TargetItemsSource, state.InsertIndex);
            }
        }

        public virtual void DragDropCompleted(TState state)
        {
            if (!ShouldRemoveItemsFromSource(state)) return;

            RemoveItems(state.DraggedItems, state.SourceItemsSource);
        }

        // Sollen die Items aus der Quelle entfernt werden?
        protected virtual bool ShouldRemoveItemsFromSource(TState state)
        {
            // Wenn es die gleiche Collection ist, dann nicht entfernen
            if (state.SourceControl == state.TargetControl) return false;

            return true;
        }

        // Items in neue Liste einfügen
        protected static void InsertItems(IEnumerable items, IList target, int index)
        {
            if (items == null) return;

            var type = TypeHelper.GetIListType(target);

            foreach (var item in items)
            {
                if (type != null && !type.IsAssignableFrom(item.GetType())) continue;

                if (index == -1) target.Add(item);
                else
                {
                    target.Insert(index, item);
                    index++;
                }
            }
        }

        // Items in Liste auf neue Position bewegen
        protected static void MoveItems(IEnumerable items, IList list, int index)
        {
            if (items == null) return;

            var itemsList = items.Cast<object>().ToList();

            for (int i = 0; i < itemsList.Count; i++)
            {
                var item = itemsList[i];

                var oldIndex = list.IndexOf(item);

                list.Remove(item);

                if (index == -1) list.Add(item);
                else
                {
                    if (oldIndex != -1 && oldIndex < index) index--;
                    list.Insert(index, item);
                    index++;
                }
            }
        }

        // Items aus Liste entfernen
        protected static void RemoveItems(IEnumerable items, IList source)
        {
            if (items == null) return;

            var itemsList = items.Cast<object>().ToList();

            for (int i = 0; i < itemsList.Count; i++)
            {
                var item = itemsList[i];

                source.Remove(item);
            }
        }
    }
}