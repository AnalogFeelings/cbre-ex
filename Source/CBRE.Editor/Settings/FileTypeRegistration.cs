using CBRE.Localization;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CBRE.Editor.Settings
{
    public static class FileTypeRegistration
    {
        public const string ProgramId = "CBREEditor";
        public const string ProgramIdVer = "1";

        public static FileType[] GetSupportedExtensions()
        {
            return new[]
            {
                new FileType(".cbr", Local.LocalString("filetype.cbr"), false, true),
                new FileType(".vmf", Local.LocalString("filetype.vmf"), true, true),
                new FileType(".3dw", Local.LocalString("filetype.3dw"), false, true),
                new FileType(".msl", Local.LocalString("filetype.msl"), false, true),
            };
        }

        private static string ExecutableLocation()
        {
            return Assembly.GetEntryAssembly().Location;
        }

        private static void AddExtensionHandler(string extension, string description)
        {
            using (RegistryKey root = Registry.CurrentUser.OpenSubKey("Software\\Classes", true))
            {
                if (root == null) return;

                using (RegistryKey progId = root.CreateSubKey(ProgramId + extension + "." + ProgramIdVer))
                {
                    if (progId == null) return;
                    progId.SetValue("", description);

                    using (RegistryKey di = progId.CreateSubKey("DefaultIcon"))
                    {
                        if (di != null) di.SetValue("", ExecutableLocation() + ",-40001");
                    }

                    using (RegistryKey comm = progId.CreateSubKey("shell\\open\\command"))
                    {
                        if (comm != null) comm.SetValue("", "\"" + ExecutableLocation() + "\" \"%1\"");
                    }

                    progId.SetValue("AppUserModelID", ProgramId);
                }
            }
        }

        private static void AssociateExtensionHandler(string extension)
        {
            using (RegistryKey root = Registry.CurrentUser.OpenSubKey("Software\\Classes", true))
            {
                if (root == null) return;

                using (RegistryKey ext = root.CreateSubKey(extension))
                {
                    if (ext == null) return;
                    ext.SetValue("", ProgramId + extension + "." + ProgramIdVer);
                    ext.SetValue("PerceivedType", "Document");

                    using (RegistryKey openWith = ext.CreateSubKey("OpenWithProgIds"))
                    {
                        if (openWith != null) openWith.SetValue(ProgramId + extension + "." + ProgramIdVer, string.Empty);
                    }
                }
            }
        }

        public static IEnumerable<string> GetRegisteredDefaultFileTypes()
        {
            using (RegistryKey root = Registry.CurrentUser.OpenSubKey("Software\\Classes"))
            {
                if (root == null) yield break;

                foreach (FileType ft in GetSupportedExtensions())
                {
                    using (RegistryKey ext = root.OpenSubKey(ft.Extension))
                    {
                        if (ext == null) continue;
                        if (Convert.ToString(ext.GetValue("")) == ProgramId + ft.Extension + "." + ProgramIdVer)
                        {
                            yield return ft.Extension;
                        }
                    }
                }
            }
        }

        public static void RegisterDefaultFileTypes(IEnumerable<string> extensions)
        {
#if DEBUG
            return;
#else
            foreach (string e in extensions)
            {
                string extension = e;
                if (!extension.StartsWith(".")) extension = "." + extension;
                AssociateExtensionHandler(extension);
            }
#endif
        }

        public static void RegisterFileTypes()
        {
#if DEBUG
            return;
#else
            try
            {
                foreach (FileType ft in GetSupportedExtensions())
                {
                    AddExtensionHandler(ft.Extension, ft.Description);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // security exception or some such
            }
#endif
        }
    }
}
