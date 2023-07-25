using System.ComponentModel.Composition;
using System.Drawing;
using System.Numerics;
using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Rendering.ChangeHandlers
{
    public class EntitySprite : IMapObjectData, IContentsReplaced, IBoundingBoxProvider
    {
        public string Name { get; set; }
        public float Scale { get; }
        public Color Color { get; set; }

        public bool ContentsReplaced => !string.IsNullOrWhiteSpace(Name);

        public EntitySprite(string name, float scale, Color color)
        {
            Name = name;
            Scale = scale;
            Color = color;
        }

        public EntitySprite(SerialisedObject obj)
        {
            Name = obj.Get<string>("Name");
            Scale = obj.Get<float>("Scale");
            Color = obj.GetColor("Color");
        }

        [Export(typeof(IMapElementFormatter))]
        public class ActiveTextureFormatter : StandardMapElementFormatter<EntitySprite> { }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Scale", Scale);
            info.AddValue("Color", Color);
        }

        public Box GetBoundingBox(IMapObject obj)
        {
            if (string.IsNullOrWhiteSpace(Name)) return null;

            SizeF size = new SizeF(64, 64);

            Vector3 origin = obj.Data.GetOne<Origin>()?.Location ?? Vector3.Zero;
            Vector3 half = new Vector3(size.Width, size.Width, size.Height) * Scale / 2;

            return new Box(origin - half, origin + half);
        }

        public IMapElement Copy(UniqueNumberGenerator numberGenerator)
        {
            return Clone();
        }

        public IMapElement Clone()
        {
            return new EntitySprite(Name, Scale, Color);
        }

        public SerialisedObject ToSerialisedObject()
        {
            SerialisedObject so = new SerialisedObject(nameof(EntitySprite));

            so.Set(nameof(Name), Name);
            so.Set(nameof(Scale), Scale);
            so.SetColor(nameof(Color), Color);

            return so;
        }
    }
}