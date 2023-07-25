using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using CBRE.Common.Shell;

namespace CBRE.Common.Translations
{
    /// <summary>
    /// A catalog which loads and stores all languages and translation strings.
    /// </summary>
    [Export]
    public class TranslationStringsCatalog
    {
        private readonly IApplicationInfo _appInfo;
        private readonly List<string> _loaded;

        public Dictionary<string, Language> Languages { get; }

        [ImportingConstructor]
        public TranslationStringsCatalog(
            [Import(AllowDefault = true)] IApplicationInfo appInfo
        )
        {
            _appInfo = appInfo;
            Languages = new Dictionary<string, Language>();
            _loaded = new List<string>();

#if DEBUG
            Languages["debug_en"] = new Language("debug_en") { Description = "Debug (english fallback)", Inherit = "en" };
            Languages["debug_doubler"] = new Language("debug_doubler") { Description = "Debug (english doubler)", Inherit = "en" };
            Languages["debug_keys"] = new Language("debug_keys") { Description = "Debug (keys)" };
            Languages["debug_keys_long"] = new Language("debug_keys_long") { Description = "Debug (long keys)" };
            Languages["debug_blank"] = new Language("debug_blank") { Description = "Debug (no fallback)" };
#endif
        }

        public string GetString(string language, string key)
        {
            // Basic loop prevention
            for (int i = 0; i < 4; i++)
            {
                if (!Languages.ContainsKey(language)) break;
                Language lang = Languages[language];
                if (lang.Collection.Strings.ContainsKey(key)) return lang.Collection.Strings[key];
                if (String.IsNullOrWhiteSpace(lang.Inherit)) break;
                language = lang.Inherit;
            }
            return null;
        }

        public string GetSetting(string language, string key)
        {
            // Basic loop prevention
            for (int i = 0; i < 4; i++)
            {
                if (!Languages.ContainsKey(language)) break;
                Language lang = Languages[language];
                if (lang.Collection.Settings.ContainsKey(key)) return lang.Collection.Settings[key];
                if (String.IsNullOrWhiteSpace(lang.Inherit)) break;
                language = lang.Inherit;
            }
            return null;
        }

        public void Load(Type type)
        {
            string loc = type.Assembly.Location ?? "";
            if (_loaded.Contains(loc)) return;
            _loaded.Add(loc);

            // Load from the translations sub-directory of the assembly's directory first
            if (File.Exists(loc))
            {
                string dir = Path.Combine(Path.GetDirectoryName(loc) ?? "", "Translations");
                if (Directory.Exists(dir))
                {
                    foreach (string file in Directory.GetFiles(dir, "*.json", SearchOption.TopDirectoryOnly))
                    {
                        LoadFile(file);
                    }
                }
            }

            // Override these values with translations from the appdata directory
            string appdataTranslations = _appInfo?.GetApplicationSettingsFolder("Translations");
            if (appdataTranslations != null && Directory.Exists(appdataTranslations))
            {
                foreach (string file in Directory.GetFiles(appdataTranslations, "*.json", SearchOption.TopDirectoryOnly))
                {
                    LoadFile(file);
                }
            }
        }

        private void LoadFile(string file)
        {
            Language data = LoadLanguageFromFile(file);
            if (data == null) return;

            if (!Languages.ContainsKey(data.Code))
            {
                Languages.Add(data.Code, data);
                return;
            }

            Language language = Languages[data.Code];

            foreach (KeyValuePair<string, string> kv in data.Collection.Settings)
            {
                if (!String.IsNullOrWhiteSpace(kv.Value)) language.Collection.Settings[kv.Key] = kv.Value;
#if DEBUG
                if (data.Code == "en") Languages["debug_doubler"].Collection.Settings[kv.Key] = kv.Value + " | " + kv.Value;
                Languages["debug_blank"].Collection.Settings[kv.Key] = "--";
                Languages["debug_keys"].Collection.Settings[kv.Key] = "[" + kv.Key.Split('.').LastOrDefault() + "]";
                Languages["debug_keys_long"].Collection.Settings[kv.Key] = "[" + kv.Key + "]";
#endif
            }

            foreach (KeyValuePair<string, string> kv in data.Collection.Strings)
            {
                if (!String.IsNullOrWhiteSpace(kv.Value)) language.Collection.Strings[kv.Key] = kv.Value;
#if DEBUG
                if (data.Code == "en") Languages["debug_doubler"].Collection.Strings[kv.Key] = kv.Value + " | " + kv.Value;
                Languages["debug_blank"].Collection.Strings[kv.Key] = "--";
                Languages["debug_keys"].Collection.Strings[kv.Key] = "[" + kv.Key.Split('.').LastOrDefault() + "]";
                Languages["debug_keys_long"].Collection.Strings[kv.Key] = "[" + kv.Key + "]";
#endif
            }
        }

        public static Language LoadLanguageFromFile(string file)
        {
            if (!File.Exists(file)) return null;

            string text = File.ReadAllText(file, Encoding.UTF8);
            JObject obj = JObject.Parse(text);

            JToken meta = obj["@Meta"];
            if (meta == null) return null;

            string lang = Convert.ToString(meta["Language"]);
            if (String.IsNullOrWhiteSpace(lang)) return null;

            string basePath = Convert.ToString(meta["Base"]) ?? "";
            if (!String.IsNullOrWhiteSpace(basePath)) basePath += ".";

            Language language = new Language(lang);

            string langDesc = Convert.ToString(meta["LanguageDescription"]);
            if (!string.IsNullOrWhiteSpace(langDesc) && string.IsNullOrWhiteSpace(language.Description)) language.Description = langDesc;

            string inherit = Convert.ToString(meta["Inherit"]);
            if (!string.IsNullOrWhiteSpace(inherit) && string.IsNullOrWhiteSpace(language.Inherit)) language.Inherit = inherit;

            IEnumerable<JProperty> strings = obj.Descendants()
                .OfType<JProperty>()
                .Where(x => x.Path[0] != '@')
                .Where(x => x.Value.Type == JTokenType.String);

            foreach (JProperty st in strings)
            {
                language.Collection.Strings[basePath + FixedPath(st)] = st.Value?.ToString();
            }

            if (obj["@Settings"] is JObject settingsNode)
            {
                IEnumerable<JProperty> settings = settingsNode.Descendants()
                    .OfType<JProperty>()
                    .Where(x => x.Value.Type == JTokenType.String);
                foreach (JProperty se in settings)
                {
                    if (se.Name.StartsWith("@")) language.Collection.Settings[se.Name] = se.Value?.ToString();
                    else language.Collection.Settings[basePath + GetSettingPath(se)] = se.Value?.ToString();
                }
            }

            return language;
        }

        private static string GetSettingPath(JToken token)
        {
            List<string> l = new List<string>();
            while (token != null)
            {
                if (token is JProperty)
                {
                    string name = ((JProperty) token).Name;
                    if (name.StartsWith("@")) break;
                    l.Add(name);
                }
                token = token.Parent;
            }
            l.Reverse();
            return String.Join(".", l);
        }

        private static string FixedPath(JToken tok)
        {
            List<string> l = new List<string>();
            JToken par = tok;
            while (par != null)
            {
                if (par is JProperty p) l.Add(p.Name);
                par = par.Parent;
            }
            l.Reverse();
            return String.Join(".", l);
        }
    }
}