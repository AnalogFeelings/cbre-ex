using System.ComponentModel.Composition;
using System.Drawing;
using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Primitives.MapObjectData
{
    public class ObjectColor : IMapObjectData
    {
        public Color Color { get; set; }

        public ObjectColor(Color color)
        {
            Color = color;
        }

        public ObjectColor(SerialisedObject obj)
        {
            Color = obj.GetColor("Color");
        }

        [Export(typeof(IMapElementFormatter))]
        public class ActiveTextureFormatter : StandardMapElementFormatter<ObjectColor> { }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Color", Color);
        }

        public IMapElement Clone()
        {
            return new ObjectColor(Color);
        }

        public IMapElement Copy(UniqueNumberGenerator numberGenerator)
        {
            return Clone();
        }

        public SerialisedObject ToSerialisedObject()
        {
            var so = new SerialisedObject("ObjectColor");
            so.SetColor("Color", Color);
            return so;
        }
    }
}