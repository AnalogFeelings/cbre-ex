using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Primitives.MapObjectData
{
    public interface IBoundingBoxProvider : IMapObjectData
    {
        Box GetBoundingBox(IMapObject obj);
    }
}
