using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Numerics;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Tools.Brush.Brushes.Controls;
using CBRE.Common;
using CBRE.Common.Shell.Components;
using CBRE.Common.Translations;
using CBRE.DataStructures.Geometric;
using Plane = CBRE.DataStructures.Geometric.Plane;

namespace CBRE.BspEditor.Tools.Brush.Brushes
{
    [Export(typeof(IBrush))]
    [OrderHint("D")]
    [AutoTranslate]
    public class WedgeBrush : IBrush
    {
        public string Name { get; set; } = "Wedge";
        public bool CanRound => true;

        public IEnumerable<BrushControl> GetControls()
        {
            return new List<BrushControl>();
        }

        public IEnumerable<IMapObject> Create(UniqueNumberGenerator generator, Box box, string texture, int roundDecimals)
        {
            var solid = new Solid(generator.Next("MapObject"));
            solid.Data.Add(new ObjectColor(Colour.GetRandomBrushColour()));

            // The lower Z plane will be base, the x planes will be triangles
            var c1 = new Vector3(box.Start.X, box.Start.Y, box.Start.Z).Round(roundDecimals);
            var c2 = new Vector3(box.End.X, box.Start.Y, box.Start.Z).Round(roundDecimals);
            var c3 = new Vector3(box.End.X, box.End.Y, box.Start.Z).Round(roundDecimals);
            var c4 = new Vector3(box.Start.X, box.End.Y, box.Start.Z).Round(roundDecimals);
            var c5 = new Vector3(box.Center.X, box.Start.Y, box.End.Z).Round(roundDecimals);
            var c6 = new Vector3(box.Center.X, box.End.Y, box.End.Z).Round(roundDecimals);
            var faces = new[]
                            {
                                new[] { c1, c2, c3, c4 },
                                new[] { c2, c1, c5 },
                                new[] { c5, c6, c3, c2 },
                                new[] { c4, c3, c6 },
                                new[] { c6, c5, c1, c4 }
                            };
            foreach (var arr in faces)
            {
                var face = new Face(generator.Next("Face"))
                {
                    Plane = new Plane(arr[0], arr[1], arr[2]),
                    Texture = {Name = texture }
                };
                face.Vertices.AddRange(arr);
                solid.Data.Add(face);
            }
            solid.DescendantsChanged();
            yield return solid;
        }
    }
}