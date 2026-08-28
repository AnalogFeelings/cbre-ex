using CBRE.Common;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Brushes.Controls;
using CBRE.Localization;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Editor.Brushes
{
    public class WedgeBrush : IBrush
    {
        public string Name
        {
            get { return Local.LocalString("brush.wedge"); }
        }

        public bool CanRound { get { return true; } }

        public IEnumerable<BrushControl> GetControls()
        {
            return new List<BrushControl>();
        }

        public IEnumerable<MapObject> Create(IDGenerator generator, Box box, ITexture texture, int roundDecimals)
        {
            Solid solid = new Solid(generator.GetNextObjectID()) { Colour = Colour.GetRandomBrushColour() };
            // The lower Z plane will be base, the x planes will be triangles
            Coordinate c1 = new Coordinate(box.Start.X, box.Start.Y, box.Start.Z).Round(roundDecimals);
            Coordinate c2 = new Coordinate(box.End.X, box.Start.Y, box.Start.Z).Round(roundDecimals);
            Coordinate c3 = new Coordinate(box.End.X, box.End.Y, box.Start.Z).Round(roundDecimals);
            Coordinate c4 = new Coordinate(box.Start.X, box.End.Y, box.Start.Z).Round(roundDecimals);
            Coordinate c5 = new Coordinate(box.Center.X, box.Start.Y, box.End.Z).Round(roundDecimals);
            Coordinate c6 = new Coordinate(box.Center.X, box.End.Y, box.End.Z).Round(roundDecimals);
            Coordinate[][] faces = new[]
                            {
                                new[] { c1, c2, c3, c4 },
                                new[] { c2, c1, c5 },
                                new[] { c5, c6, c3, c2 },
                                new[] { c4, c3, c6 },
                                new[] { c6, c5, c1, c4 }
                            };
            foreach (Coordinate[] arr in faces)
            {
                Face face = new Face(generator.GetNextFaceID())
                {
                    Parent = solid,
                    Plane = new Plane(arr[0], arr[1], arr[2]),
                    Colour = solid.Colour,
                    Texture = { Texture = texture }
                };
                face.Vertices.AddRange(arr.Select(x => new Vertex(x, face)));
                face.UpdateBoundingBox();
                face.AlignTextureToFace();
                solid.Faces.Add(face);
            }
            solid.UpdateBoundingBox();
            yield return solid;
        }
    }
}