using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TPF.Controls.Specialized.DialogHost;

namespace TPF.Controls
{
    public static class DialogHostExtensions
    {
        public static async Task<object> Show(this DialogHost dialogHost, object content)
        {
            return await Show(dialogHost, content, null, null, null);
        }

        public static async Task<object> Show(this DialogHost dialogHost, object content, DialogOpenedEventHandler openedEventHandler)
        {
            return await Show(dialogHost, content, openedEventHandler, null, null);
        }

        public static async Task<object> Show(this DialogHost dialogHost, object content, DialogClosingEventHandler closingEventHandler)
        {
            return await Show(dialogHost, content, null, closingEventHandler, null);
        }

        public static async Task<object> Show(this DialogHost dialogHost, object content, DialogClosedEventHandler closedEventHandler)
        {
            return await Show(dialogHost, content, null, null, closedEventHandler);
        }

        public static async Task<object> Show(this DialogHost dialogHost, object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
        {
            return await Show(dialogHost, content, openedEventHandler, closingEventHandler, null);
        }

        public static async Task<object> Show(this DialogHost dialogHost, object content, DialogOpenedEventHandler openedEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await Show(dialogHost, content, openedEventHandler, null, closedEventHandler);
        }

        public static async Task<object> Show(this DialogHost dialogHost, object content, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await Show(dialogHost, content, null, closingEventHandler, closedEventHandler);
        }

        public static async Task<object> Show(this DialogHost dialogHost, object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            return await dialogHost.ShowInternal(content, openedEventHandler, closingEventHandler, closedEventHandler);
        }

        public static void Close(this DialogHost dialogHost)
        {
            Close(dialogHost, null);
        }

        public static void Close(this DialogHost dialogHost, object resultValue)
        {
            if (dialogHost.CurrentHandle != null && !dialogHost.CurrentHandle.IsClosed)
            {
                dialogHost.CurrentHandle.Close(resultValue);
            }
        }

        public static async Task<object> ShowParentDialog(this DependencyObject dependencyObject, object content)
        {
            return await ShowParentDialog(dependencyObject, content, null, null, null);
        }

        public static async Task<object> ShowParentDialog(this DependencyObject dependencyObject, object content, DialogOpenedEventHandler openedEventHandler)
        {
            return await ShowParentDialog(dependencyObject, content, openedEventHandler, null, null);
        }

        public static async Task<object> ShowParentDialog(this DependencyObject dependencyObject, object content, DialogClosingEventHandler closingEventHandler)
        {
            return await ShowParentDialog(dependencyObject, content, null, closingEventHandler, null);
        }

        public static async Task<object> ShowParentDialog(this DependencyObject dependencyObject, object content, DialogClosedEventHandler closedEventHandler)
        {
            return await ShowParentDialog(dependencyObject, content, null, null, closedEventHandler);
        }

        public static async Task<object> ShowParentDialog(this DependencyObject dependencyObject, object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
        {
            return await ShowParentDialog(dependencyObject, content, openedEventHandler, closingEventHandler, null);
        }

        public static async Task<object> ShowParentDialog(this DependencyObject dependencyObject, object content, DialogOpenedEventHandler openedEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await ShowParentDialog(dependencyObject, content, openedEventHandler, null, closedEventHandler);
        }

        public static async Task<object> ShowParentDialog(this DependencyObject dependencyObject, object content, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await ShowParentDialog(dependencyObject, content, null, closingEventHandler, closedEventHandler);
        }

        public static async Task<object> ShowParentDialog(this DependencyObject dependencyObject, object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var dialogHost = GetParentDialogHost(dependencyObject);

            return await dialogHost?.ShowInternal(content, openedEventHandler, closingEventHandler, closedEventHandler);
        }

        public static void CloseParentDialog(this DependencyObject dependencyObject)
        {
            CloseParentDialog(dependencyObject, null);
        }

        public static void CloseParentDialog(this DependencyObject dependencyObject, object resultValue)
        {
            var dialogHost = GetParentDialogHost(dependencyObject);

            if (dialogHost.CurrentHandle != null && !dialogHost.CurrentHandle.IsClosed)
            {
                dialogHost.CurrentHandle.Close(resultValue);
            }
        }

        public static DialogHandle GetParentDialogHandle(this DependencyObject dependencyObject)
        {
            var dialogHost = GetParentDialogHost(dependencyObject);

            return dialogHost?.CurrentHandle;
        }

        public static bool HasOpenParentDialog(this DependencyObject dependencyObject)
        {
            return GetParentDialogHandle(dependencyObject)?.IsClosed == false;
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content)
        {
            return await ShowChildDialog(dependencyObject, content, null, null, null, null);
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, DialogOpenedEventHandler openedEventHandler)
        {
            return await ShowChildDialog(dependencyObject, content, null, openedEventHandler, null, null);
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, DialogClosingEventHandler closingEventHandler)
        {
            return await ShowChildDialog(dependencyObject, content, null, null, closingEventHandler, null);
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, DialogClosedEventHandler closedEventHandler)
        {
            return await ShowChildDialog(dependencyObject, content, null, null, null, closedEventHandler);
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
        {
            return await ShowChildDialog(dependencyObject, content, null, openedEventHandler, closingEventHandler, null);
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, DialogOpenedEventHandler openedEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await ShowChildDialog(dependencyObject, content, null, openedEventHandler, null, closedEventHandler);
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await ShowChildDialog(dependencyObject, content, null, null, closingEventHandler, closedEventHandler);
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await ShowChildDialog(dependencyObject, content, null, openedEventHandler, closingEventHandler, closedEventHandler);
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, object hostId)
        {
            return await ShowChildDialog(dependencyObject, content, hostId, null, null, null);
        }

        public static Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, object hostId, DialogOpenedEventHandler openedEventHandler)
        {
            return ShowChildDialog(dependencyObject, content, hostId, openedEventHandler, null, null);
        }

        public static Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, object hostId, DialogClosingEventHandler closingEventHandler)
        {
            return ShowChildDialog(dependencyObject, content, hostId, null, closingEventHandler, null);
        }

        public static Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, object hostId, DialogClosedEventHandler closedEventHandler)
        {
            return ShowChildDialog(dependencyObject, content, hostId, null, null, closedEventHandler);
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, object hostId, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
        {
            return await ShowChildDialog(dependencyObject, content, hostId, openedEventHandler, closingEventHandler, null);
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, object hostId, DialogOpenedEventHandler openedEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await ShowChildDialog(dependencyObject, content, hostId, openedEventHandler, null, closedEventHandler);
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, object hostId, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await ShowChildDialog(dependencyObject, content, hostId, null, closingEventHandler, closedEventHandler);
        }

        public static async Task<object> ShowChildDialog(this DependencyObject dependencyObject, object content, object hostId, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var dialogHost = GetChildDialogHost(dependencyObject, hostId);

            return await dialogHost?.ShowInternal(content, openedEventHandler, closingEventHandler, closedEventHandler);
        }

        public static void CloseChildDialog(this DependencyObject dependencyObject)
        {
            CloseChildDialog(dependencyObject, null, null);
        }

        public static void CloseChildDialog(this DependencyObject dependencyObject, object hostId)
        {
            CloseChildDialog(dependencyObject, hostId, null);
        }

        public static void CloseChildDialog(this DependencyObject dependencyObject, object hostId, object resultValue)
        {
            var dialogHost = GetChildDialogHost(dependencyObject, hostId);

            if (dialogHost.CurrentHandle != null && !dialogHost.CurrentHandle.IsClosed)
            {
                dialogHost.CurrentHandle.Close(resultValue);
            }
        }

        public static DialogHandle GetChildDialogHandle(this DependencyObject dependencyObject)
        {
            return GetChildDialogHandle(dependencyObject, null);
        }

        public static DialogHandle GetChildDialogHandle(this DependencyObject dependencyObject, object hostId)
        {
            var dialogHost = GetChildDialogHost(dependencyObject, hostId);

            return dialogHost?.CurrentHandle;
        }

        public static bool HasOpenChildDialog(this DependencyObject dependencyObject, object hostId)
        {
            return GetChildDialogHandle(dependencyObject, hostId)?.IsClosed == false;
        }

        private static DialogHost GetParentDialogHost(DependencyObject dependencyObject)
        {
            var dialogHost = dependencyObject.ParentOfType<DialogHost>();

            if (dialogHost == null) System.Diagnostics.Trace.WriteLine("ERROR: Could not find parent DialogHost");

            return dialogHost;
        }

        private static DialogHost GetChildDialogHost(DependencyObject dependencyObject, object hostId)
        {
            var dialogHost = dependencyObject.ChildrenOfType<DialogHost>().FirstOrDefault(x => hostId == null || Equals(x.Id, hostId));

            if (dialogHost == null) System.Diagnostics.Trace.WriteLine("ERROR: Could not find child DialogHost");

            return dialogHost;
        }
    }
}