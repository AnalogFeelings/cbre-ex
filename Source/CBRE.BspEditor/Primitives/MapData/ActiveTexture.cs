using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Primitives.MapData
{
    public class ActiveTexture : IMapData
    {
        public bool AffectsRendering => false;

        public string Name { get; set; }

        public ActiveTexture()
        {
            Name = null;
        }

        public ActiveTexture(SerialisedObject obj)
        {
            Name = obj.Get<string>("Name");
        }

        [Export(typeof(IMapElementFormatter))]
        public class ActiveTextureFormatter : StandardMapElementFormatter<ActiveTexture> { }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Meh
        }

        public IMapElement Clone()
        {
            return new ActiveTexture {Name = Name};
        }

        public IMapElement Copy(UniqueNumberGenerator numberGenerator)
        {
            return Clone();
        }

        public SerialisedObject ToSerialisedObject()
        {
            SerialisedObject so = new SerialisedObject("ActiveTexture");
            so.Set("Name", Name);
            return so;
        }
    }
}
