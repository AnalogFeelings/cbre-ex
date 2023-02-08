using CBRE.FileSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CBRE.Providers.Texture.Generic
{
    public class GenericTexturePackage : TexturePackage
    {
        private readonly IFile _file;

        protected override IEqualityComparer<string> GetComparer => StringComparer.InvariantCultureIgnoreCase;

        public GenericTexturePackage(string name, TexturePackageReference reference) : base(name, "Generic")
        {
            _file = reference.File;

            IEnumerable<string> allTextures = _file.GetFiles(@".*\.png|.*\.jpg|.*\.jpeg", true).Select(x => x.Name);

            Textures.UnionWith(allTextures);
        }

        private Size GetSize(IFile file)
        {
            using (Stream stream = file.Open())
            {
                using (Image image = Image.FromStream(stream, false, false))
                {
                    return new Size(image.Width, image.Height);
                }
            }
        }

        private bool IsTransparent(IFile file)
        {
            using (Stream stream = file.Open())
            {
                using (Bitmap image = (Bitmap)Image.FromStream(stream))
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        for (int y = 0; y < image.Height; y++)
                        {
                            if (image.GetPixel(x, y).A < 255) return true;
                        }
                    }
                }
            }

            return false;
        }

        public override async Task<IEnumerable<TextureItem>> GetTextures(IEnumerable<string> names)
        {
            HashSet<string> textures = new HashSet<string>(names, GetComparer);
            textures.IntersectWith(Textures);

            if (!textures.Any()) return Array.Empty<TextureItem>();

            List<TextureItem> list = new List<TextureItem>();
            foreach (var name in textures)
            {
                IFile entry = _file.TraversePath(name);
                if (entry == null || !entry.Exists) continue;

                Size size = GetSize(entry);
                TextureFlags flags = IsTransparent(entry) ? TextureFlags.Transparent : TextureFlags.None;

                TextureItem item = new TextureItem(name, flags, size.Width, size.Height);

                list.Add(item);
            }

            return list;
        }

        public override async Task<TextureItem> GetTexture(string name)
        {
            if (!Textures.Contains(name)) return null;

            IEnumerable<TextureItem> textures = await GetTextures(new[] { name });

            return textures.FirstOrDefault();
        }

        public override ITextureStreamSource GetStreamSource()
        {
            return new GenericStreamSource(_file);
        }
    }
}
