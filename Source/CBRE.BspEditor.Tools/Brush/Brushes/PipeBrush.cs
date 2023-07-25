using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Tools.Brush.Brushes.Controls;
using CBRE.Common;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Hooks;
using CBRE.Common.Translations;
using CBRE.DataStructures.Geometric;
using Plane = CBRE.DataStructures.Geometric.Plane;

namespace CBRE.BspEditor.Tools.Brush.Brushes
{
    [Export(typeof(IBrush))]
    [Export(typeof(IInitialiseHook))]
    [OrderHint("G")]
    [AutoTranslate]
    public class PipeBrush : IBrush, IInitialiseHook
    {
        private NumericControl _numSides;
        private NumericControl _wallWidth;
        
        public string NumberOfSides { get; set; }
        public string WallWidth { get; set; }

        public async Task OnInitialise()
        {
            _numSides = new NumericControl(this) { LabelText = NumberOfSides };
            _wallWidth = new NumericControl(this) { LabelText = WallWidth, Minimum = 1, Maximum = 1024, Value = 32, Precision = 1 };
        }

        public string Name { get; set; } = "Pipe";
        public bool CanRound => true;

        public IEnumerable<BrushControl> GetControls()
        {
            yield return _numSides;
            yield return _wallWidth;
        }

        private Solid MakeSolid(UniqueNumberGenerator generator, IEnumerable<Vector3[]> faces, string texture, Color col)
        {
            Solid solid = new Solid(generator.Next("MapObject"));
            solid.Data.Add(new ObjectColor(col));
            foreach (Vector3[] arr in faces)
            {
                Face face = new Face(generator.Next("Face"))
                {
                    Plane = new Plane(arr[0], arr[1], arr[2]),
                    Texture = { Name = texture  }
                };
                face.Vertices.AddRange(arr);
                solid.Data.Add(face);
            }
            solid.DescendantsChanged();
            return solid;
        }

        public IEnumerable<IMapObject> Create(UniqueNumberGenerator generator, Box box, string texture, int roundDecimals)
        {
            float wallWidth = (float) _wallWidth.GetValue();
            if (wallWidth < 1) yield break;
            int numSides = (int) _numSides.GetValue();
            if (numSides < 3) yield break;

            // Very similar to the cylinder, except we have multiple solids this time
            float width = box.Width;
            float length = box.Length;
            float height = box.Height;
            float majorOut = width / 2;
            float majorIn = majorOut - wallWidth;
            float minorOut = length / 2;
            float minorIn = minorOut - wallWidth;
            double angle = 2 * Math.PI / numSides;

            // Calculate the X and Y points for the inner and outer ellipses
            Vector3[] outer = new Vector3[numSides];
            Vector3[] inner = new Vector3[numSides];
            for (int i = 0; i < numSides; i++)
            {
                double a = i * angle;
                float xval = box.Center.X + majorOut * (float) Math.Cos(a);
                float yval = box.Center.Y + minorOut * (float) Math.Sin(a);
                float zval = box.Start.Z;
                outer[i] = new Vector3(xval, yval, zval).Round(roundDecimals);
                xval = box.Center.X + majorIn * (float) Math.Cos(a);
                yval = box.Center.Y + minorIn * (float) Math.Sin(a);
                inner[i] = new Vector3(xval, yval, zval).Round(roundDecimals);
            }

            // Create the solids
            Color colour = Colour.GetRandomBrushColour();
            Vector3 z = new Vector3(0, 0, height).Round(roundDecimals);
            for (int i = 0; i < numSides; i++)
            {
                List<Vector3[]> faces = new List<Vector3[]>();
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
