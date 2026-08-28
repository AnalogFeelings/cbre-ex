using CBRE.Localization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CBRE.Providers.GameData
{
    public abstract class GameDataProvider
    {
        private static readonly List<GameDataProvider> RegisteredProviders;

        static GameDataProvider()
        {
            RegisteredProviders = new List<GameDataProvider>();
        }

        public static void Register(GameDataProvider provider)
        {
            RegisteredProviders.Add(provider);
        }

        public static void Deregister(GameDataProvider provider)
        {
            RegisteredProviders.Remove(provider);
        }

        public static DataStructures.GameData.GameData GetGameDataFromFiles(IEnumerable<string> files)
        {
            DataStructures.GameData.GameData gd = new DataStructures.GameData.GameData();
            foreach (DataStructures.GameData.GameData d in files.Select(GetGameDataFromFile))
            {
                gd.MapSizeHigh = d.MapSizeHigh;
                gd.MapSizeLow = d.MapSizeLow;
                gd.Classes.AddRange(d.Classes);
                gd.MaterialExclusions.AddRange(d.MaterialExclusions);
            }
            gd.CreateDependencies();
            gd.RemoveDuplicates();
            return gd;
        }

        public static DataStructures.GameData.GameData GetGameDataFromFile(string fileName)
        {
            GameDataProvider provider = RegisteredProviders.FirstOrDefault(p => p.IsValidForFile(fileName));
            if (provider != null)
            {
                DataStructures.GameData.GameData gd = provider.GetFromFile(fileName);
                return gd;
            }
            throw new ProviderNotFoundException(Local.LocalString("exception.no_gamedata_provider_file"));
        }

        public static DataStructures.GameData.GameData GetGameDataFromString(string contents)
        {
            GameDataProvider provider = RegisteredProviders.FirstOrDefault(p => p.IsValidForString(contents));
            if (provider != null)
            {
                DataStructures.GameData.GameData gd = provider.GetFromString(contents);
                return gd;
            }
            throw new ProviderNotFoundException(Local.LocalString("exception.no_gamedata_provider_string"));
        }

        public static DataStructures.GameData.GameData GetGameDataFromStream(Stream stream)
        {
            GameDataProvider provider = RegisteredProviders.FirstOrDefault(p => p.IsValidForStream(stream));
            if (provider != null)
            {
                DataStructures.GameData.GameData gd = provider.GetFromStream(stream);
                return gd;
            }
            throw new ProviderNotFoundException(Local.LocalString("exception.no_gamedata_provider_stream"));
        }

        protected virtual bool IsValidForFile(string filename)
        {
            Stream strm = new FileStream(filename, FileMode.Open, FileAccess.Read);
            return IsValidForStream(strm);
        }

        protected virtual bool IsValidForString(string contents)
        {
            int length = Encoding.UTF8.GetByteCount(contents);
            Stream strm = new MemoryStream(length);
            strm.Write(Encoding.UTF8.GetBytes(contents), 0, length);
            return IsValidForStream(strm);
        }

        protected abstract bool IsValidForStream(Stream stream);

        protected virtual DataStructures.GameData.GameData GetFromFile(string filename)
        {
            using (FileStream strm = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                return GetFromStream(strm);
            }
        }

        protected virtual DataStructures.GameData.GameData GetFromString(string contents)
        {
            int length = Encoding.UTF8.GetByteCount(contents);
            using (MemoryStream strm = new MemoryStream(length))
            {
                strm.Write(Encoding.UTF8.GetBytes(contents), 0, length);
                return GetFromStream(strm);
            }
        }

        protected abstract DataStructures.GameData.GameData GetFromStream(Stream stream);
    }
}
