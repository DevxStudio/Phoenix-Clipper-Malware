using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Stub.Help
{
    class ClipboardNative
    {
        private const uint CF_UNICODETEXT = 0xD;

        public static string GetText()
        {

            if (NativeMethods.IsClipboardFormatAvailable(CF_UNICODETEXT) && NativeMethods.OpenClipboard(IntPtr.Zero))
            {
                string data = string.Empty;
                IntPtr hGlobal = NativeMethods.GetClipboardData(CF_UNICODETEXT);
                if (!hGlobal.Equals(IntPtr.Zero))
                {
                    IntPtr lpwcstr = NativeMethods.GlobalLock(hGlobal);
                    if (!lpwcstr.Equals(IntPtr.Zero))
                    {
                        try
                        {
                            data = Marshal.PtrToStringUni(lpwcstr);
                            NativeMethods.GlobalUnlock(lpwcstr);
                        }
                        catch { }
                    }
                }
                NativeMethods.CloseClipboard();
                return data;
            }
            return null;
        }


        public static void SetText(string txt)
        {
            Thread STAThread = new Thread(
                delegate ()
                {
                    try
                    {
                        System.Windows.Forms.Clipboard.SetText(txt);
                    }
                    catch { }
                });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();
        }
    }
}
