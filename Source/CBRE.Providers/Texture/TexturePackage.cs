﻿using CBRE.Graphics.Helpers;
using System;
using System.Collections.Generic;

namespace CBRE.Providers.Texture
{
    public class TexturePackage : IDisposable
    {
        internal TextureProvider Provider { get; private set; }
        public string PackageRoot { get; private set; }
        public string PackageRelativePath { get; private set; }
        public Dictionary<string, TextureItem> Items { get; private set; }
        private readonly Dictionary<string, TextureItem> _loadedItems;
        public bool IsBrowsable { get; set; }

        public TexturePackage(string packageRoot, string packageRelativePath, TextureProvider provider)
        {
            Provider = provider;
            PackageRoot = packageRoot;
            PackageRelativePath = packageRelativePath;
            Items = new Dictionary<string, TextureItem>();
            _loadedItems = new Dictionary<string, TextureItem>();
            IsBrowsable = true;
        }

        public void AddTexture(TextureItem item)
        {
            if (Items.ContainsKey(item.Name.ToLowerInvariant())) return;
            Items.Add(item.Name.ToLowerInvariant(), item);
        }

        public bool HasTexture(string name)
        {
            return Items.ContainsKey(name.ToLowerInvariant());
        }

        public override string ToString()
        {
            return PackageRelativePath;
        }

        public void Dispose()
        {
            foreach (KeyValuePair<string, TextureItem> kv in _loadedItems)
            {
                TextureHelper.Delete(kv.Value.Name.ToLowerInvariant());
            }
        }
    }
}
