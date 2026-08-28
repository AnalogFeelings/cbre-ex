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
    public class PipeBrush : IBrush
    {
        private readonly NumericControl _numSides;
        private readonly NumericControl _wallWidth;

        public PipeBrush()
        {
            _numSides = new NumericControl(this) { LabelText = Local.LocalString("brush.sides") };
            _wallWidth = new NumericControl(this) { LabelText = Local.LocalString("brush.width"), Minimum = 1, Maximum = 1024, Value = 32, Precision = 1 };
        }

        public string Name
        {
            get { return Local.LocalString("brush.pipe"); }
        }

        public bool CanRound { get { return true; } }

        public IEnumerable<BrushControl> GetControls()
        {
            yield return _numSides;
            yield return _wallWidth;
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
                face.AlignTextureToFace();
                solid.Faces.Add(face);
            }
            solid.UpdateBoundingBox();
            return solid;
        }

        public IEnumerable<MapObject> Create(IDGenerator generator, Box box, ITexture texture, int roundDecimals)
        {
            decimal wallWidth = _wallWidth.GetValue();
            if (wallWidth < 1) yield break;
            int numSides = (int)_numSides.GetValue();
            if (numSides < 3) yield break;

            // Very similar to the cylinder, except we have multiple solids this time
            decimal width = box.Width;
            decimal length = box.Length;
            decimal height = box.Height;
            decimal majorOut = width / 2;
            decimal majorIn = majorOut - wallWidth;
            decimal minorOut = length / 2;
            decimal minorIn = minorOut - wallWidth;
            decimal angle = 2 * DMath.PI / numSides;

            // Calculate the X and Y points for the inner and outer ellipses
            Coordinate[] outer = new Coordinate[numSides];
            Coordinate[] inner = new Coordinate[numSides];
            for (int i = 0; i < numSides; i++)
            {
                decimal a = i * angle;
                decimal xval = box.Center.X + majorOut * DMath.Cos(a);
                decimal yval = box.Center.Y + minorOut * DMath.Sin(a);
                decimal zval = box.Start.Z;
                outer[i] = new Coordinate(xval, yval, zval).Round(roundDecimals);
                xval = box.Center.X + majorIn * DMath.Cos(a);
                yval = box.Center.Y + minorIn * DMath.Sin(a);
                inner[i] = new Coordinate(xval, yval, zval).Round(roundDecimals);
            }

            // Create the solids
            Color colour = Colour.GetRandomBrushColour();
            Coordinate z = new Coordinate(0, 0, height).Round(roundDecimals);
            for (int i = 0; i < numSides; i++)
            {
                List<Coordinate[]> faces = new List<Coordinate[]>();
                int next = (i + 1) % numSides;
                faces.Add(new[] { outer[i], outer[i] + z, outer[next] + z, outer[next] });
                faces.Add(new[] { inner[next], inner[next] + z, inner[i] + z, inner[i] });
                faces.Add(new[] { outer[next], outer[next] + z, inner[next] + z, inner[next] });
                faces.Add(new[] { inner[i], inner[i] + z, outer[i] + z, outer[i] });
                faces.Add(new[] { inner[next] + z, outer[next] + z, outer[i] + z, inner[i] + z });
                faces.Add(new[] { inner[i], outer[i], outer[next], inner[next] });
                yield return MakeSolid(generator, faces, texture, colour);
            }
        }
    }
}
