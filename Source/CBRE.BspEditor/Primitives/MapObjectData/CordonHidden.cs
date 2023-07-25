using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Primitives.MapObjectData
{
    public class CordonHidden : IMapObjectData, IObjectVisibility
    {
        public bool IsHidden => true;

        public CordonHidden()
        {
        }

        public CordonHidden(SerialisedObject obj)
        {
        }

        [Export(typeof(IMapElementFormatter))]
        public class ActiveTextureFormatter : StandardMapElementFormatter<CordonHidden> { }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        public IMapElement Clone()
        {
            return new CordonHidden();
        }

        public IMapElement Copy(UniqueNumberGenerator numberGenerator)
        {
            return Clone();
        }

        public SerialisedObject ToSerialisedObject()
        {
            SerialisedObject so = new SerialisedObject("CordonHidden");
            return so;
        }
    }
}