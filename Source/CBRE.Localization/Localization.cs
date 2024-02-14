using System;
using System.Globalization;
using System.IO;
using System.Text.Json.Nodes;

namespace CBRE.Localization
{
    public partial class Local
    {
        private static readonly JsonObject LocalizationFile;
        private static readonly JsonObject FallbackFile;

        static Local()
        {
            FallbackFile = JsonNode.Parse(File.ReadAllText("Localization\\en_US.json")).AsObject();
            try
            {
                LocalizationFile = JsonNode.Parse(File.ReadAllText("Localization\\" + CultureInfo.CurrentUICulture.Name.Replace('-', '_') + ".json")).AsObject();
            }
            catch (Exception)
            {
                LocalizationFile = FallbackFile;
            }
        }

        public static string LocalString(string key)
        {
            return LocalizationFile.ContainsKey(key) ? LocalizationFile[key].ToString() : FallbackFile[key].ToString();
        }

        public static string LocalString(string key, params object[] values)
        {
            return String.Format(LocalString(key), values);
        }

    }
}
