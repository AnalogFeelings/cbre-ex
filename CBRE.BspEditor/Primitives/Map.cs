using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjects;

namespace CBRE.BspEditor.Primitives
{
    /// <summary>
    /// A map. Has a root node and some metadata.
    /// </summary>
    public class Map
    {
        public MapDataCollection Data { get; }
        public Root Root { get; }
        public UniqueNumberGenerator NumberGenerator { get; set; }

        public Map()
        {
            NumberGenerator = new UniqueNumberGenerator();
            Data = new MapDataCollection();
            Root = new Root(NumberGenerator.Next("MapObject"));
        }
    }
}