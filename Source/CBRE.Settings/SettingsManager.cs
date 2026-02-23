using CBRE.Providers;
using CBRE.Settings.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CBRE.Settings
{
    public static class SettingsManager
    {
        public static Build Build { get; private set; }
        public static Game Game { get; private set; }
        public static List<RecentFile> RecentFiles { get; set; }
        public static List<Setting> Settings { get; set; }
        public static List<Hotkey> Hotkeys { get; set; }
        private static readonly Dictionary<string, GenericStructure> AdditionalSettings;
        public static List<FavouriteTextureFolder> FavouriteTextureFolders { get; set; }

        public static string SettingsFile { get; set; }

        static SettingsManager()
        {
            Build = new Build();
            Game = new Game();
            RecentFiles = new List<RecentFile>();
            Settings = new List<Setting>();
            Hotkeys = new List<Hotkey>();
            SpecialTextureOpacities = new Dictionary<string, float>
                                          {
                                              {"null", 0},
                                              {"bevel", 0},
                                              {"tools/toolsnodraw", 0},
                                              {"aaatrigger", 0.5f},
                                              {"clip", 0.5f},
                                              {"hint", 0.5f},
                                              {"origin", 0.5f},
                                              {"skip", 0.5f},
                                              {"tooltextures/remove_face", 0.5f},
                                              {"tooltextures/invisible_collision", 0.5f},
                                              {"tooltextures/triggerbox", 0.5f},
                                              {"tooltextures/block_light", 0.5f},
                                          };
            AdditionalSettings = new Dictionary<string, GenericStructure>();
            FavouriteTextureFolders = new List<FavouriteTextureFolder>();

            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string sledge = Path.Combine(appdata, "CBRE-EX");
            if (!Directory.Exists(sledge)) Directory.CreateDirectory(sledge);
            SettingsFile = Path.Combine(sledge, "Settings.vdf");
        }

        public static string GetTextureCachePath()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string sledge = Path.Combine(appdata, "CBRE-EX");
            if (!Directory.Exists(sledge)) Directory.CreateDirectory(sledge);
            string cache = Path.Combine(sledge, "TextureCache");
            if (!Directory.Exists(cache)) Directory.CreateDirectory(cache);
            return cache;
        }

        public static float GetSpecialTextureOpacity(string name)
        {
            name = name.ToLowerInvariant();
            float val = SpecialTextureOpacities.ContainsKey(name) ? SpecialTextureOpacities[name] : 1;
            if (View.DisableToolTextureTransparency || View.GloballyDisableTransparency)
            {
                return val < 0.1 ? 0 : 1;
            }
            return val;
        }

        private static readonly IDictionary<string, float> SpecialTextureOpacities;

        private static GenericStructure ReadSettingsFile()
        {
            if (File.Exists(SettingsFile)) return GenericStructure.Parse(SettingsFile).FirstOrDefault();

            string exec = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(exec, "Settings.vdf");
            if (File.Exists(path)) return GenericStructure.Parse(path).FirstOrDefault();

            return null;
        }

        public static void Read()
        {
            RecentFiles.Clear();
            Settings.Clear();
            Hotkeys.Clear();
            AdditionalSettings.Clear();
            FavouriteTextureFolders.Clear();

            GenericStructure root = ReadSettingsFile();

            if (root == null) return;

            GenericStructure settings = root.Children.FirstOrDefault(x => x.Name == "Settings");
            if (settings != null)
            {
                foreach (string key in settings.GetPropertyKeys())
                {
                    Settings.Add(new Setting { Key = key, Value = settings[key] });
                }
            }
            GenericStructure recents = root.Children.FirstOrDefault(x => x.Name == "RecentFiles");
            if (recents != null)
            {
                foreach (string key in recents.GetPropertyKeys())
                {
                    int i;
                    if (int.TryParse(key, out i))
                    {
                        RecentFiles.Add(new RecentFile { Location = recents[key], Order = i });
                    }
                }
            }
            GenericStructure hotkeys = root.Children.FirstOrDefault(x => x.Name == "Hotkeys");
            if (hotkeys != null)
            {
                foreach (string key in hotkeys.GetPropertyKeys())
                {
                    string[] spl = key.Split(':');
                    Hotkeys.Add(new Hotkey { ID = spl[0], HotkeyString = hotkeys[key] });
                }
            }

            Serialise.DeserialiseSettings(Settings.ToDictionary(x => x.Key, x => x.Value));
            CBRE.Settings.Hotkeys.SetupHotkeys(Hotkeys);

            GenericStructure additionalSettings = root.Children.FirstOrDefault(x => x.Name == "AdditionalSettings");
            if (additionalSettings != null)
            {
                foreach (GenericStructure child in additionalSettings.Children)
                {
                    if (child.Children.Count > 0) AdditionalSettings.Add(child.Name, child.Children[0]);
                }
            }

            GenericStructure favTextures = root.Children.FirstOrDefault(x => x.Name == "FavouriteTextures");
            if (favTextures != null && favTextures.Children.Any())
            {
                try
                {
                    List<FavouriteTextureFolder> ft = GenericStructure.Deserialise<List<FavouriteTextureFolder>>(favTextures.Children[0]);
                    if (ft != null) FavouriteTextureFolders.AddRange(ft);
                    FixFavouriteNames(FavouriteTextureFolders);
                }
                catch
                {
                    // Nope
                }
            }

            if (!File.Exists(SettingsFile))
            {
                Write();
            }
        }

        private static void FixFavouriteNames(IEnumerable<FavouriteTextureFolder> folders)
        {
            foreach (FavouriteTextureFolder f in folders)
            {
                FixFavouriteNames(f.Children);
                f.Items = f.Items.Select(x =>
                {
                    int i = x.IndexOf(':');
                    if (i >= 0) x = x.Substring(i + 1);
                    return x;
                }).ToList();
            }
        }

        public static void Write()
        {
            IEnumerable<Setting> newSettings = Serialise.SerialiseSettings().Select(s => new Setting { Key = s.Key, Value = s.Value });
            Settings.Clear();
            Settings.AddRange(newSettings);

            GenericStructure root = new GenericStructure("CBRE-EX");

            // Settings
            GenericStructure settings = new GenericStructure("Settings");
            foreach (Setting setting in Settings)
            {
                settings.AddProperty(setting.Key, setting.Value);
            }
            root.Children.Add(settings);

            // Recent Files
            GenericStructure recents = new GenericStructure("RecentFiles");
            int i = 1;
            foreach (string file in RecentFiles.OrderBy(x => x.Order).Select(x => x.Location).Where(File.Exists))
            {
                recents.AddProperty(i.ToString(), file);
                i++;
            }
            root.Children.Add(recents);

            // Hotkeys
            Hotkeys = CBRE.Settings.Hotkeys.GetHotkeys().ToList();
            GenericStructure hotkeys = new GenericStructure("Hotkeys");
            foreach (IGrouping<string, Hotkey> g in Hotkeys.GroupBy(x => x.ID))
            {
                int count = 0;
                foreach (Hotkey hotkey in g)
                {
                    hotkeys.AddProperty(hotkey.ID + ":" + count, hotkey.HotkeyString);
                    count++;
                }
            }
            root.Children.Add(hotkeys);

            // Additional
            GenericStructure additional = new GenericStructure("AdditionalSettings");
            foreach (KeyValuePair<string, GenericStructure> kv in AdditionalSettings)
            {
                GenericStructure child = new GenericStructure(kv.Key);
                child.Children.Add(kv.Value);
                additional.Children.Add(child);
            }
            root.Children.Add(additional);

            // Favourite textures
            GenericStructure favTextures = new GenericStructure("FavouriteTextures");
            favTextures.Children.Add(GenericStructure.Serialise(FavouriteTextureFolders));
            root.Children.Add(favTextures);

            File.WriteAllText(SettingsFile, root.ToString());
        }

        private static string GetSessionFile()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string sledge = Path.Combine(appdata, "CBRE-EX");
            if (!Directory.Exists(sledge)) Directory.CreateDirectory(sledge);
            return Path.Combine(sledge, "session");
        }

        public static void SaveSession(IEnumerable<Tuple<string, Game>> files)
        {
            File.WriteAllLines(GetSessionFile(), files.Select(x => x.Item1 + ":" + x.Item2.ID));
        }

        public static IEnumerable<string> LoadSession()
        {
            string sf = GetSessionFile();
            if (!File.Exists(sf)) return new List<string>();
            return File.ReadAllLines(sf)
                .Select(x =>
                {
                    int i = x.LastIndexOf(":", StringComparison.Ordinal);
                    string file = x.Substring(0, i);
                    string num = x.Substring(i + 1);
                    int id;
                    int.TryParse(num, out id);
                    return file;
                })
                .Where(x => File.Exists(x));
        }

        public static T GetAdditionalData<T>(string key)
        {
            if (!AdditionalSettings.ContainsKey(key)) return default(T);
            GenericStructure additional = AdditionalSettings[key];
            try
            {
                return GenericStructure.Deserialise<T>(additional);
            }
            catch
            {
                return default(T); // Deserialisation failure
            }
        }

        public static void SetAdditionalData<T>(string key, T obj)
        {
            if (AdditionalSettings.ContainsKey(key)) AdditionalSettings.Remove(key);
            AdditionalSettings.Add(key, GenericStructure.Serialise(obj));
        }
    }
}
