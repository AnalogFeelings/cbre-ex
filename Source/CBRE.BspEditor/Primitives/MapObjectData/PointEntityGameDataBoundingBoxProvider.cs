using System;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;
using CBRE.DataStructures.GameData;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Primitives.MapObjectData
{
    public class PointEntityGameDataBoundingBoxProvider : IBoundingBoxProvider
    {
        private readonly GameData _data;

        public PointEntityGameDataBoundingBoxProvider(GameData data)
        {
            _data = data;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Meh
        }

        public IMapElement Clone()
        {
            return new PointEntityGameDataBoundingBoxProvider(_data);
        }

        public Box GetBoundingBox(IMapObject obj)
        {
            // Try and get a bounding box for point entities
            string name = obj.Data.GetOne<EntityData>()?.Name;
            Vector3 origin = obj.Data.GetOne<Origin>()?.Location ?? Vector3.Zero;
            if (name == null) return null;

            // Get the class (must be point)
            GameDataObject cls = _data.Classes.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase) && x.ClassType == ClassType.Point);
            if (cls == null) return null;

            // Default to 16x16
            Vector3 sub = new Vector3(-8, -8, -8);
            Vector3 add = new Vector3(8, 8, 8);

            // Get the size behaviour
            Behaviour behav = cls.Behaviours.SingleOrDefault(x => x.Name == "size");
            if (behav != null && behav.Values.Count >= 6)
            {
                sub = behav.GetVector3(0) ?? Vector3.Zero;
                add = behav.GetVector3(1) ?? Vector3.Zero;
            }
            else if (cls.Name == "infodecal")
            {
                // Special handling for infodecal if it's not specified
                sub = Vector3.One * -4;
                add = Vector3.One * 4;
            }
            return new Box(origin + sub, origin + add);
        }

        public IMapElement Copy(UniqueNumberGenerator numberGenerator)
        {
            return Clone();
        }

        public SerialisedObject ToSerialisedObject()
        {
            SerialisedObject so = new SerialisedObject(nameof(PointEntityGameDataBoundingBoxProvider));
            return so;
        }
    }
}