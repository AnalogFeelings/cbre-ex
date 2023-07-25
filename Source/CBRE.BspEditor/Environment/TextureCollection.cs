using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CBRE.Common.Logging;
using CBRE.Providers.Texture;

namespace CBRE.BspEditor.Environment
{
    public abstract class TextureCollection
    {
        public IEnumerable<TexturePackage> Packages => _packages;

        private readonly ConcurrentDictionary<string, TextureItem> _itemCache;

        private readonly List<TexturePackage> _packages;

        protected TextureCollection(IEnumerable<TexturePackage> packages)
        {
            _packages = packages.ToList();
            _itemCache = new ConcurrentDictionary<string, TextureItem>(StringComparer.InvariantCultureIgnoreCase);
            
            Log.Debug(nameof(TextureCollection), $"Reading textures from: {String.Join("; ", _packages.Select(x => x.Location))}");
        }

        public bool HasTexture(string name)
        {
            return Packages.Any(x => x.HasTexture(name));
        }

        private string GetDefaultSelection()
        {
            return
            (from item in _packages.SelectMany(x => x.Textures).OrderBy(x => x, StringComparer.CurrentCultureIgnoreCase)
                where item.Length > 0
                let c = Char.ToLower(item[0])
                where c >= 'a' && c <= 'z'
                select item).FirstOrDefault();
        }

        public async Task Precache(IEnumerable<string> textures)
        {
            HashSet<string> tex = new HashSet<string>(textures.Select(x => x.ToLower()));
            tex.ExceptWith(_itemCache.Keys);
            if (!tex.Any()) return;

            List<Task> tasks = new List<Task>();

            foreach (TexturePackage pack in _packages)
            {
                HashSet<string> found = new HashSet<string>(tex.Where(x => pack.HasTexture(x)));
                tex.ExceptWith(found);
                if (!found.Any()) continue;
                Task t = pack.GetTextures(found).ContinueWith(x =>
                {
                    foreach (TextureItem ti in x.Result) _itemCache[ti.Name] = ti;
                });
                tasks.Add(t);
            }

            if (tasks.Any()) await Task.WhenAll(tasks);
        }

        public IEnumerable<string> GetAllTextures()
        {
            HashSet<string> hs = new HashSet<string>();
            foreach (TexturePackage pack in _packages) hs.UnionWith(pack.Textures);
            return hs;
        }

        public abstract IEnumerable<string> GetBrowsableTextures();
        public abstract IEnumerable<string> GetDecalTextures();
        public abstract IEnumerable<string> GetSpriteTextures();

        public async Task<TextureItem> GetTextureItem(string name)
        {
            name = name.ToLower();
            if (_itemCache.ContainsKey(name)) return _itemCache[name];

            List<TexturePackage> packs = _packages.Where(x => x.HasTexture(name)).ToList();
            foreach (TexturePackage tp in packs)
            {
                TextureItem r = await tp.GetTexture(name);
                if (r == null) continue;

                _itemCache[r.Name.ToLower()] = r;
                return r;
            }
            return null;
        }

        public async Task<IEnumerable<TextureItem>> GetTextureItems(IEnumerable<string> names)
        {
            List<string> n = names.Select(x => x.ToLower()).ToList();
            IEnumerable<string> missing = n.Where(x => !_itemCache.ContainsKey(x));
            await Precache(missing);
            return n.Where(x => _itemCache.ContainsKey(x)).Select(x => _itemCache[x]);
        }

        public ITextureStreamSource GetStreamSource()
        {
            return GetStreamSource(_packages);
        }

        public ITextureStreamSource GetStreamSource(IEnumerable<TexturePackage> packages)
        {
            return new MultiTextureStreamSource(packages.Select(x => x.GetStreamSource()));
        }

        public abstract bool IsToolTexture(string name);
        public abstract float GetOpacity(string name);
    }
}
