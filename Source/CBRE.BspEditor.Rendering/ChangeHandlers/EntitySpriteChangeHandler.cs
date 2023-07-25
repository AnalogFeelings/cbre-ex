using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Environment;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.ChangeHandling;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.DataStructures.GameData;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Rendering.ChangeHandlers
{
    [Export(typeof(IMapDocumentChangeHandler))]
    public class EntitySpriteChangeHandler : IMapDocumentChangeHandler
    {
        public string OrderHint => "M";

        public async Task Changed(Change change)
        {
            GameData gd = await change.Document.Environment.GetGameData();
            TextureCollection tc = await change.Document.Environment.GetTextureCollection();
            foreach (Entity entity in change.Added.Union(change.Updated).OfType<Entity>())
            {
                string sn = GetSpriteName(entity, gd);
                EntitySprite sd = sn == null ? null : await CreateSpriteData(entity, change.Document, gd, tc, sn);
                if (sd == null) entity.Data.Remove(x => x is EntitySprite);
                else entity.Data.Replace(sd);
                entity.DescendantsChanged();
            }
        }

        private static async Task<EntitySprite> CreateSpriteData(Entity entity, MapDocument doc, GameData gd, TextureCollection tc, string name)
        {
            if (!tc.HasTexture(name)) return null;

            CBRE.Providers.Texture.TextureItem texture = await tc.GetTextureItem(name);
            if (texture == null) return null;

            GameDataObject cls = gd?.GetClass(entity.EntityData.Name);
            float scale = 1f;
            Color color = Color.White;

            if (cls != null)
            {
                if (cls.Properties.Any(x => string.Equals(x.Name, "scale", StringComparison.CurrentCultureIgnoreCase)))
                {
                    scale = entity.EntityData.Get<float>("scale", 1);
                    if (scale <= 0.1f) scale = 1;
                }

                Property colProp = cls.Properties.FirstOrDefault(x => x.VariableType == VariableType.Color255 || x.VariableType == VariableType.Color1);
                if (colProp != null)
                {
                    System.Numerics.Vector3? col = entity.EntityData.GetVector3(colProp.Name);
                    if (colProp.VariableType == VariableType.Color255) col /= 255f;
                    if (col.HasValue) color = col.Value.ToColor();
                }
            }

            return new EntitySprite(name, scale, color);
        }

        private static string GetSpriteName(Entity entity, GameData gd)
        {
            if (entity.Hierarchy.HasChildren || string.IsNullOrWhiteSpace(entity.EntityData.Name)) return null;
            GameDataObject cls = gd?.GetClass(entity.EntityData.Name);
            if (cls == null) return null;

            Behaviour spr = cls.Behaviours.FirstOrDefault(x => string.Equals(x.Name, "sprite", StringComparison.InvariantCultureIgnoreCase))
                      ?? cls.Behaviours.FirstOrDefault(x => string.Equals(x.Name, "iconsprite", StringComparison.InvariantCultureIgnoreCase));
            if (spr == null) return null;

            // First see if the studio behaviour forces a model...
            if (spr.Values.Count == 1 && !string.IsNullOrWhiteSpace(spr.Values[0]))
            {
                return spr.Values[0].Trim();
            }

            // Find the first property that is a studio type, or has a name of "sprite"...
            Property prop = cls.Properties.FirstOrDefault(x => x.VariableType == VariableType.Sprite) ??
                       cls.Properties.FirstOrDefault(x => string.Equals(x.Name, "sprite", StringComparison.InvariantCultureIgnoreCase));
            if (prop != null)
            {
                string val = entity.EntityData.Get(prop.Name, prop.DefaultValue);
                if (!string.IsNullOrWhiteSpace(val)) return val;
            }
            return null;
        }
    }
}
