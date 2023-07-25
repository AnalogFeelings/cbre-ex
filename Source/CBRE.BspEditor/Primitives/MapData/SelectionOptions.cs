using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Primitives.MapData
{
    public class SelectionOptions : IMapData
    {
        public bool AffectsRendering => false;

        public bool IgnoreGrouping { get; set; }

        public SelectionOptions()
        {
        }

        public SelectionOptions(SerialisedObject obj)
        {
            IgnoreGrouping = obj.Get<bool>("IgnoreGrouping");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("IgnoreGrouping", IgnoreGrouping);
        }

        public SerialisedObject ToSerialisedObject()
        {
            SerialisedObject so = new SerialisedObject("SelectionOptions");
            so.Set("IgnoreGrouping", IgnoreGrouping);
            return so;
        }

        public IMapElement Copy(UniqueNumberGenerator numberGenerator)
        {
            return Clone();
        }

        public IMapElement Clone()
        {
            return new SelectionOptions
            {
                IgnoreGrouping = IgnoreGrouping
            };
        }
    }
}
