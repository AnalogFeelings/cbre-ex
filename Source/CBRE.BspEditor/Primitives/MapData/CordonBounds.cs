using System.ComponentModel.Composition;
using System.Numerics;
using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Primitives.MapData
{
    public class CordonBounds : IMapData
    {
        public bool AffectsRendering => true;

        public bool Enabled { get; set; }
        public Box Box { get; set; }

        public CordonBounds()
        {
            Enabled = false;
            Box = new Box(Vector3.One * -1024, Vector3.One * 1024);
        }

        public CordonBounds(SerialisedObject obj)
        {
            Enabled = obj.Get<bool>("Enabled");
            Vector3 start = obj.Get<Vector3>("Start");
            Vector3 end = obj.Get<Vector3>("End");
            Box = new Box(start, end);
        }

        [Export(typeof(IMapElementFormatter))]
        public class CordonBoundsFormatter : StandardMapElementFormatter<CordonBounds> { }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Meh
        }

        public IMapElement Clone()
        {
            return new CordonBounds
            {
                Box = Box.Clone(),
                Enabled = Enabled
            };
        }

        public IMapElement Copy(UniqueNumberGenerator numberGenerator)
        {
            return Clone();
        }

        public SerialisedObject ToSerialisedObject()
        {
            SerialisedObject so = new SerialisedObject("CordonBounds");
            so.Set("Enabled", Enabled);
            so.Set("Start", Box.Start);
            so.Set("End", Box.End);
            return so;
        }
    }
}
