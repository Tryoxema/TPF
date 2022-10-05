using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

namespace TPF.Controls
{
    public static class DependencyObjectExtensions
    {
        public static T ParentOfType<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null) return null;

            var parent = GetParent(dependencyObject);

            // Kein Parent im VisualTree vorhanden
            if (parent == null) return null;

            if (parent is T) return (T)parent;
            else return parent.ParentOfType<T>();
        }

        public static DependencyObject ParentFromType(this DependencyObject dependencyObject, Type type)
        {
            if (type == null) throw new ArgumentNullException("Type");
            if (dependencyObject == null) return null;

            var parent = GetParent(dependencyObject);

            // Kein Parent im VisualTree vorhanden
            if (parent == null) return null;

            if (parent.GetType() == type) return parent;
            else return parent.ParentFromType(type);
        }

        public static T FindParentByName<T>(this DependencyObject dependencyObject, string name) where T : FrameworkElement
        {
            if (dependencyObject == null) return null;

            var parent = GetParent(dependencyObject);

            // Kein Parent im VisualTree vorhanden
            if (parent == null) return null;

            if (parent is T element && element.Name == name) return element;
            else return parent.FindParentByName<T>(name);
        }

        private static DependencyObject GetParent(DependencyObject dependencyObject)
        {
            // VisualTreeHelper.GetParent kann nur mit Visual oder Visual3D arbeiten
            if (dependencyObject is FrameworkContentElement contentElement) return contentElement.Parent;

            DependencyObject parent;

            try
            {
                // Um ungewollte Abstürze zu verhindern das hier absichern
                parent = VisualTreeHelper.GetParent(dependencyObject);
            }
            catch (InvalidOperationException)
            {
                parent = null;
            }

            // Manchmal kann der VisualTreeHelper nichts finden
            if (parent == null)
            {
                // Wenn der VisualTreeHelper nicht gefunden hat oder einen Fehler geworfen hat, prüfen wir die Parent-Property, sofern vorhanden
                if (dependencyObject is FrameworkElement element) parent = element.Parent;
            }

            return parent;
        }

        public static T ChildOfType<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null) return null;

            var childCount = VisualTreeHelper.GetChildrenCount(dependencyObject);

            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                if (child is T target) return target;

                var found = ChildOfType<T>(child);

                if (found != null) return found;
            }

            return null;
        }

        public static IEnumerable<T> ChildrenOfType<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    var child = VisualTreeHelper.GetChild(dependencyObject, i);

                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (var childOfChild in ChildrenOfType<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static T FindChildByName<T>(this DependencyObject dependencyObject, string name) where T : FrameworkElement
        {
            if (dependencyObject == null) return null;

            var childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);

            var elements = new List<T>();

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                var element = child as T;

                if (element != null && element.Name == name) return element;

                elements.Add(element);
            }

            for (int i = 0; i < elements.Count; i++)
            {
                var element = elements[i];

                var result = element.FindChildByName<T>(name);

                if (result != null) return result;
            }

            return null;
        }
    }
}