using System;

namespace TPF.Internal.Interop
{
    internal class MouseHookEventArgs : EventArgs
    {
        public MouseHookMessage Message { get; set; }

        public Win32Point Point { get; set; }
    }
}