using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using CBRE.BspEditor.Grid;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Primitives.MapData
{
    public class GridData : IMapData
    {
        public bool AffectsRendering => false;

        public bool SnapToGrid { get; set; }
        public IGrid Grid { get; set; }

        public GridData(IGrid grid)
        {
            Grid = grid;
            SnapToGrid = true;
        }

        public GridData(SerialisedObject obj)
        {
            // todo deserialise grid
            SnapToGrid = obj.Get<bool>("SnapToGrid");
        }

        [Export(typeof(IMapElementFormatter))]
        public class GridDataFormatter : StandardMapElementFormatter<GridData> { }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Meh
        }

        public IMapElement Clone()
        {
            return new GridData(Grid) {SnapToGrid = SnapToGrid};
        }

        public IMapElement Copy(UniqueNumberGenerator numberGenerator)
        {
            return Clone();
        }

        public SerialisedObject ToSerialisedObject()
        {
            SerialisedObject so = new SerialisedObject("Grid");
            so.Set("SnapToGrid", SnapToGrid);
            return so;
        }
    }
}
