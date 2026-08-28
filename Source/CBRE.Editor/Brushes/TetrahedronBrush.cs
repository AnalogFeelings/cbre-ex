using CBRE.Common;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Brushes.Controls;
using CBRE.Localization;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Editor.Brushes
{
    public class TetrahedronBrush : IBrush
    {
        private readonly BooleanControl _useCentroid;

        public TetrahedronBrush()
        {
            _useCentroid = new BooleanControl(this) { LabelText = Local.LocalString("brush.top_vertex_centroid"), Checked = false };
        }

        public string Name
        {
            get { return Local.LocalString("brush.tetrahedron"); }
        }

        public bool CanRound { get { return true; } }

        public IEnumerable<BrushControl> GetControls()
        {
            yield return _useCentroid;
        }

        public IEnumerable<MapObject> Create(IDGenerator generator, Box box, ITexture texture, int roundDecimals)
        {
            bool useCentroid = _useCentroid.GetValue();

            // The lower Z plane will be the triangle, with the lower Y value getting the two corners
            Coordinate c1 = new Coordinate(box.Start.X, box.Start.Y, box.Start.Z).Round(roundDecimals);
            Coordinate c2 = new Coordinate(box.End.X, box.Start.Y, box.Start.Z).Round(roundDecimals);
            Coordinate c3 = new Coordinate(box.Center.X, box.End.Y, box.Start.Z).Round(roundDecimals);
            Coordinate centroid = new Coordinate((c1.X + c2.X + c3.X) / 3, (c1.Y + c2.Y + c3.Y) / 3, box.End.Z);
            Coordinate c4 = (useCentroid ? centroid : new Coordinate(box.Center.X, box.Center.Y, box.End.Z)).Round(roundDecimals);

            Coordinate[][] faces = new[] {
                new[] { c1, c2, c3 },
                new[] { c4, c1, c3 },
                new[] { c4, c3, c2 },
                new[] { c4, c2, c1 }
            };

            Solid solid = new Solid(generator.GetNextObjectID()) { Colour = Colour.GetRandomBrushColour() };
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
