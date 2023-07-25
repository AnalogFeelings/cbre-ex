using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Tools.Brush.Brushes.Controls;
using CBRE.Common;
using CBRE.Common.Shell.Components;
using CBRE.Common.Translations;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Tools.Brush.Brushes
{
    [Export(typeof(IBrush))]
    [OrderHint("A")]
    [AutoTranslate]
    public class BlockBrush : IBrush
    {
        public string Name { get; set; } = "Block";
        public bool CanRound => true;

        public IEnumerable<BrushControl> GetControls()
        {
            yield break;
        }

        public IEnumerable<IMapObject> Create(UniqueNumberGenerator idGenerator, Box box, string texture, int roundDecimals)
        {
            Solid solid = new Solid(idGenerator.Next("MapObject"));
            solid.Data.Add(new ObjectColor(Colour.GetRandomBrushColour()));

            foreach (System.Numerics.Vector3[] arr in box.GetBoxFaces())
            {
                Face face = new Face(idGenerator.Next("Face"))
                {
                    Plane = new Plane(arr[0], arr[1], arr[2]),
                    Texture = { Name = texture }
                };
                face.Vertices.AddRange(arr.Select(x => x.Round(roundDecimals)));
                solid.Data.Add(face);
            }
            solid.DescendantsChanged();
            yield return solid;
        }
    }
}
