using System;
using System.Globalization;
using System.IO;
using System.Text.Json.Nodes;

namespace CBRE.Localization
{
    public static class Local
    {
        private static readonly JsonObject LocalizationFile;
        private static readonly JsonObject FallbackFile;

        static Local()
        {
            FallbackFile = JsonNode.Parse(File.ReadAllText("Localization\\en_US.json"))?.AsObject();
            try
            {
                LocalizationFile = JsonNode
                    .Parse(File.ReadAllText("Localization\\" + CultureInfo.CurrentUICulture.Name.Replace('-', '_') +
                                            ".json"))?.AsObject();
            }
            catch (Exception)
            {
                LocalizationFile = FallbackFile;
            }
        }

        public static string LocalString(string key)
        {
            if (LocalizationFile.ContainsKey(key)) return LocalizationFile[key]?.ToString();
            return FallbackFile.ContainsKey(key) ? FallbackFile[key]?.ToString() : key;
        }

        public static string LocalString(string key, params object[] values)
        {
            return string.Format(LocalString(key), values);
        }
    }
}