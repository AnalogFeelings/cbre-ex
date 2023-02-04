using System.Collections.Generic;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Tools.Brush.Brushes.Controls;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Tools.Brush
{
    public interface IBrush
    {
        string Name { get; }
        bool CanRound { get; }
        IEnumerable<BrushControl> GetControls();
        IEnumerable<IMapObject> Create(UniqueNumberGenerator idGenerator, Box box, string texture, int roundDecimals);
    }
}
