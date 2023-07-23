using System;
using System.Collections.Generic;
using System.Linq;
using CBRE.Providers.Texture;

namespace CBRE.BspEditor.Environment.ContainmentBreach
{
    public class BlitzTextureCollection : TextureCollection
    {
        public BlitzTextureCollection(IEnumerable<TexturePackage> packages) : base(packages)
        {
        }

        public override IEnumerable<string> GetBrowsableTextures()
        {
            HashSet<string> hs = new HashSet<string>();

            foreach (TexturePackage pack in Packages.Where(x => x.Type == "Generic")) hs.UnionWith(pack.Textures);

            return hs;
        }

        public override IEnumerable<string> GetDecalTextures()
        {
            return Array.Empty<string>(); // TODO: Remove this, there are no decals in CB.
        }

        public override IEnumerable<string> GetSpriteTextures()
        {
            return Packages.Where(x => string.Equals(x.Location, "sprites", StringComparison.InvariantCultureIgnoreCase)).SelectMany(x => x.Textures);
        }

        public override bool IsToolTexture(string name)
        {
            switch (name?.ToLower())
            {
                case "block_light":
                case "remove_face":
                case "invisible_collision":
                    return true;
                default:
                    return false;
            }
        }

        public override float GetOpacity(string name)
        {
            switch (name?.ToLower())
            {
                case "block_light":
                case "remove_face":
                case "invisible_collision":
                    return 0.5f;
                default:
                    return 1;
            }
        }
    }
}
