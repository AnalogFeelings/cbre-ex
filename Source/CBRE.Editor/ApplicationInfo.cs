using System;
using System.ComponentModel.Composition;
using System.IO;
using CBRE.Common.Shell;

namespace CBRE.Editor
{
    [Export(typeof(IApplicationInfo))]
    public class ApplicationInfo : IApplicationInfo
    {
        private string Name => "CBRE-EX";

        public string GetApplicationSettingsFolder(string subfolder)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Name);
            if (String.IsNullOrWhiteSpace(subfolder)) return path;
            return Path.Combine(path, subfolder);
        }
    }
}
