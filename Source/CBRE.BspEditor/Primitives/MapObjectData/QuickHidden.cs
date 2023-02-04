using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Primitives.MapObjectData
{
    public class QuickHidden : IMapObjectData, IObjectVisibility
    {
        public bool IsHidden => true;

        public QuickHidden()
        {
            //
        }

        public QuickHidden(SerialisedObject obj)
        {
            //
        }

        [Export(typeof(IMapElementFormatter))]
        public class ActiveTextureFormatter : StandardMapElementFormatter<QuickHidden> { }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //
        }

        public IMapElement Clone()
        {
            return new QuickHidden();
        }

        public IMapElement Copy(UniqueNumberGenerator numberGenerator)
        {
            return Clone();
        }

        public SerialisedObject ToSerialisedObject()
        {
            var so = new SerialisedObject("QuickHidden");
            return so;
        }
    }
}