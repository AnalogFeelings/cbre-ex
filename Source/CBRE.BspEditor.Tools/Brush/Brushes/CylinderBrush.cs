using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
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
    [OrderHint("E")]
    [AutoTranslate]
    public class CylinderBrush : IBrush, IInitialiseHook
    {
        private NumericControl _numSides;
        
        public string NumberOfSides { get; set; }

        public Task OnInitialise()
        {
            _numSides = new NumericControl(this) { LabelText = NumberOfSides };
            return Task.CompletedTask;
        }

        public string Name { get; set; } = "Cylinder";
        public bool CanRound => true;

        public IEnumerable<BrushControl> GetControls()
        {
            yield return _numSides;
        }

        public IEnumerable<IMapObject> Create(UniqueNumberGenerator generator, Box box, string texture, int roundDecimals)
        {
            int numSides = (int) _numSides.GetValue();
            if (numSides < 3) yield break;

            // Cylinders can be elliptical so use both major and minor rather than just the radius
            // NOTE: when a low number (< 10ish) of faces are selected this will cause the cylinder to not touch all the edges of the box.
            float width = box.Width;
            float length = box.Length;
            float height = box.Height;
            float major = width / 2;
            float minor = length / 2;
            float angle = 2 * (float) Math.PI / numSides;

            // Calculate the X and Y points for the ellipse
            Vector3[] points = new Vector3[numSides];
            for (int i = 0; i < numSides; i++)
            {
                float a = i * angle;
                float xval = box.Center.X + major * (float) Math.Cos(a);
                float yval = box.Center.Y + minor * (float) Math.Sin(a);
                float zval = box.Start.Z;
                points[i] = new Vector3(xval, yval, zval).Round(roundDecimals);
            }

            List<Vector3[]> faces = new List<Vector3[]>();

            // Add the vertical faces
            Vector3 z = new Vector3(0, 0, height).Round(roundDecimals);
            for (int i = 0; i < numSides; i++)
            {
                int next = (i + 1) % numSides;
                faces.Add(new[] {points[i], points[i] + z, points[next] + z, points[next]});
            }

            // Add the elliptical top and bottom faces
            faces.Add(points.ToArray());
            faces.Add(points.Select(x => x + z).Reverse().ToArray());

            // Nothing new here, move along
            Solid solid = new Solid(generator.Next("MapObject"));
            solid.Data.Add(new ObjectColor(Colour.GetRandomBrushColour()));

            foreach (Vector3[] arr in faces)
            {
                Face face = new Face(generator.Next("Face"))
                {
                    Plane = new Plane(arr[0], arr[1], arr[2]),
                    Texture = { Name = texture }
                };
                face.Vertices.AddRange(arr);
                solid.Data.Add(face);
            }
            solid.DescendantsChanged();
            yield return solid;
        }
    }
}
