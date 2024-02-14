using CBRE.Common;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Brushes.Controls;
using CBRE.Extensions;
using CBRE.Localization;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CBRE.Editor.Brushes
{
    public class SphereBrush : IBrush
    {
        private readonly NumericControl _numSides;

        public SphereBrush()
        {
            _numSides = new NumericControl(this) { LabelText = Local.LocalString("brush.sides") };
        }

        public string Name
        {
            get { return Local.LocalString("brush.sphere"); }
        }

        public bool CanRound { get { return false; } }

        public IEnumerable<BrushControl> GetControls()
        {
            yield return _numSides;
        }

        private Solid MakeSolid(IDGenerator generator, IEnumerable<Coordinate[]> faces, ITexture texture, Color col)
        {
            Solid solid = new Solid(generator.GetNextObjectID()) { Colour = col };
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
                face.AlignTextureToWorld();
                solid.Faces.Add(face);
            }
            solid.UpdateBoundingBox();
            return solid;
        }

        public IEnumerable<MapObject> Create(IDGenerator generator, Box box, ITexture texture, int roundDecimals)
        {
            int numSides = (int)_numSides.GetValue();
            if (numSides < 3) yield break;

            roundDecimals = 2; // don't support rounding

            decimal width = box.Width;
            decimal length = box.Length;
            decimal height = box.Height;
            decimal major = width / 2;
            decimal minor = length / 2;
            decimal heightRadius = height / 2;

            decimal angleV = DMath.DegreesToRadians(180) / numSides;
            decimal angleH = DMath.DegreesToRadians(360) / numSides;

            List<Coordinate[]> faces = new List<Coordinate[]>();
            Coordinate bottom = new Coordinate(box.Center.X, box.Center.Y, box.Start.Z).Round(roundDecimals);
            Coordinate top = new Coordinate(box.Center.X, box.Center.Y, box.End.Z).Round(roundDecimals);

            for (int i = 0; i < numSides; i++)
            {
                // Top -> bottom
                decimal zAngleStart = angleV * i;
                decimal zAngleEnd = angleV * (i + 1);
                decimal zStart = heightRadius * DMath.Cos(zAngleStart);
                decimal zEnd = heightRadius * DMath.Cos(zAngleEnd);
                decimal zMultStart = DMath.Sin(zAngleStart);
                decimal zMultEnd = DMath.Sin(zAngleEnd);
                for (int j = 0; j < numSides; j++)
                {
                    // Go around the circle in X/Y
                    decimal xyAngleStart = angleH * j;
                    decimal xyAngleEnd = angleH * ((j + 1) % numSides);
                    decimal xyStartX = major * DMath.Cos(xyAngleStart);
                    decimal xyStartY = minor * DMath.Sin(xyAngleStart);
                    decimal xyEndX = major * DMath.Cos(xyAngleEnd);
                    decimal xyEndY = minor * DMath.Sin(xyAngleEnd);
                    Coordinate one = (new Coordinate(xyStartX * zMultStart, xyStartY * zMultStart, zStart) + box.Center).Round(roundDecimals);
                    Coordinate two = (new Coordinate(xyEndX * zMultStart, xyEndY * zMultStart, zStart) + box.Center).Round(roundDecimals);
                    Coordinate three = (new Coordinate(xyEndX * zMultEnd, xyEndY * zMultEnd, zEnd) + box.Center).Round(roundDecimals);
                    Coordinate four = (new Coordinate(xyStartX * zMultEnd, xyStartY * zMultEnd, zEnd) + box.Center).Round(roundDecimals);
                    if (i == 0)
                    {
                        // Top faces are triangles
                        faces.Add(new[] { top, three, four });
                    }
                    else if (i == numSides - 1)
                    {
                        // Bottom faces are also triangles
                        faces.Add(new[] { bottom, one, two });
                    }
                    else
                    {
                        // Inner faces are quads
                        faces.Add(new[] { one, two, three, four });
                    }
                }
            }
            yield return MakeSolid(generator, faces, texture, Colour.GetRandomBrushColour());
        }
    }
}
