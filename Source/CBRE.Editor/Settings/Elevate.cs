using CBRE.Localization;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CBRE.Editor.Settings
{
    public static class Elevation
    {
        public const string ProgramId = "CBREEditor";

        public static void RegisterFileType(string extension)
        {
            if (!extension.StartsWith(".")) extension = "." + extension;
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(Path.Combine(extension, "OpenWithProgIds"));
            if (key == null) return; // TODO etc etc
            bool registered = key.GetValue(ProgramId, null) != null;
            if (!registered)
            {
                //RegisterFileType(programId, execuatablePath, extension)
                Execute("RegisterFileType", ProgramId, '"' + Assembly.GetEntryAssembly().Location + '"', extension);
            }
        }

        private static void Execute(params string[] parameters)
        {
            ProcessStartInfo psi = new ProcessStartInfo("CBRE.Editor.Elevate.exe")
            {
                Arguments = String.Join(" ", parameters),
                UseShellExecute = true,
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Hidden
            };

            try
            {
                Process.Start(psi).WaitForExit();
                //TaskDialog.Show("File associations were " + (unregister ? "un" : "") + "registered");
            }
            catch (Win32Exception e)
            {
                if (e.NativeErrorCode == 1223) // 1223: The operation was canceled by the user. 
                    TaskDialog.Show(Local.LocalString("info.canceled"));
            }
        }
    }
}
