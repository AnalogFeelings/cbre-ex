using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Primitives.MapObjectData
{
    /// <summary>
    /// Assigns a visgroup ID to a map object.
    /// The visgroups themselves are stored as map data.
    /// </summary>
    public class VisgroupID : IMapObjectData
    {
        public long ID { get; set; }

        public VisgroupID(long id)
        {
            ID = id;
        }

        public VisgroupID(SerialisedObject obj)
        {
            ID = obj.Get<long>("ID");
        }

        [Export(typeof(IMapElementFormatter))]
        public class ActiveTextureFormatter : StandardMapElementFormatter<VisgroupID> { }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", ID);
        }

        public IMapElement Clone()
        {
            return new VisgroupID(ID);
        }

        public IMapElement Copy(UniqueNumberGenerator numberGenerator)
        {
            return Clone();
        }

        public SerialisedObject ToSerialisedObject()
        {
            var so = new SerialisedObject("VisgroupID");
            so.Set("ID", ID);
            return so;
        }
    }
}