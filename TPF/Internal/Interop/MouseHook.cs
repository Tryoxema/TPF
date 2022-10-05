using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TPF.Internal.Interop
{
    internal class MouseHook
    {
        internal readonly static int HookType = 14;

        private static IntPtr _hookId = IntPtr.Zero;

        private static int _count;

        private static readonly HookCallback _callback = HookCallback;

        public static event EventHandler<MouseHookEventArgs> StatusChanged;

        private static IntPtr SetHook(HookCallback callback)
        {
            using (var process = Process.GetCurrentProcess())
            {
                using (var module = process.MainModule)
                {
                    if (module != null)
                    {
                        return NativeMethods.SetWindowsHookEx(HookType, callback, NativeMethods.GetModuleHandle(module.ModuleName), 0);
                    }
                    return IntPtr.Zero;
                }
            }
        }

        public static void Start()
        {
            if (_hookId == IntPtr.Zero) _hookId = SetHook(_callback);
            if (_hookId != IntPtr.Zero) _count++;
        }

        public static void Stop()
        {
            _count--;
            if (_count < 1)
            {
                NativeMethods.UnhookWindowsHookEx(_hookId);
                _hookId = IntPtr.Zero;
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0) return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);

            var hook = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));

            StatusChanged?.Invoke(null, new MouseHookEventArgs
            {
                Message = (MouseHookMessage)wParam,
                Point = new Win32Point(hook.pt.X, hook.pt.Y)
            });

            return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
    }
}