using System;
using System.Runtime.InteropServices;

namespace Stub.Help.Native
{
    class NativeMethods
    {
        #region For ClipBoard GetText
        [DllImport("user32.dll")]
        internal static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        public static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        internal static extern bool EmptyClipboard();

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        internal static extern bool GlobalUnlock(IntPtr hMem);
        #endregion   
    }
}
