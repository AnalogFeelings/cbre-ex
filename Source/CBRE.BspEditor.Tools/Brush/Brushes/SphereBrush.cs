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
    [OrderHint("I")]
    [AutoTranslate]
    public class SphereBrush : IBrush, IInitialiseHook
    {
        private NumericControl _numSides;
        
        public string NumberOfSides { get; set; }

        public Task OnInitialise()
        {
            _numSides = new NumericControl(this) { LabelText = NumberOfSides };
            return Task.CompletedTask;
        }

        public string Name { get; set; } = "Sphere";

        public bool CanRound => false;

        public IEnumerable<BrushControl> GetControls()
        {
            yield return _numSides;
        }

        private Solid MakeSolid(UniqueNumberGenerator generator, IEnumerable<Vector3[]> faces, string texture, Color col)
        {
            Solid solid = new Solid(generator.Next("MapObject"));
            solid.Data.Add(new ObjectColor(Colour.GetRandomBrushColour()));

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
            int numSides = (int)_numSides.GetValue();
            if (numSides < 3) yield break;

            roundDecimals = 2; // don't support rounding

            float width = box.Width;
            float length = box.Length;
            float height = box.Height;
            float major = width / 2;
            float minor = length / 2;
            float heightRadius = height / 2;

            float angleV = (float) MathHelper.DegreesToRadians(180f) / numSides;
            float angleH = (float) MathHelper.DegreesToRadians(360f) / numSides;

            List<Vector3[]> faces = new List<Vector3[]>();
            Vector3 bottom = new Vector3(box.Center.X, box.Center.Y, box.Start.Z).Round(roundDecimals);
            Vector3 top = new Vector3(box.Center.X, box.Center.Y, box.End.Z).Round(roundDecimals);
            
            for (int i = 0; i < numSides; i++)
            {
                // Top -> bottom
                float zAngleStart = angleV * i;
                float zAngleEnd = angleV * (i + 1);
                float zStart = heightRadius * (float) Math.Cos(zAngleStart);
                float zEnd = heightRadius * (float) Math.Cos(zAngleEnd);
                float zMultStart = (float) Math.Sin(zAngleStart);
                float zMultEnd = (float) Math.Sin(zAngleEnd);
                for (int j = 0; j < numSides; j++)
                {
                    // Go around the circle in X/Y
                    float xyAngleStart = angleH * j;
                    float xyAngleEnd = angleH * ((j + 1) % numSides);
                    float xyStartX = major * (float) Math.Cos(xyAngleStart);
                    float xyStartY = minor * (float) Math.Sin(xyAngleStart);
                    float xyEndX = major * (float) Math.Cos(xyAngleEnd);
                    float xyEndY = minor * (float) Math.Sin(xyAngleEnd);
                    Vector3 one = (new Vector3(xyStartX * zMultStart, xyStartY * zMultStart, zStart) + box.Center).Round(roundDecimals);
                    Vector3 two = (new Vector3(xyEndX * zMultStart, xyEndY * zMultStart, zStart) + box.Center).Round(roundDecimals);
                    Vector3 three = (new Vector3(xyEndX * zMultEnd, xyEndY * zMultEnd, zEnd) + box.Center).Round(roundDecimals);
                    Vector3 four = (new Vector3(xyStartX * zMultEnd, xyStartY * zMultEnd, zEnd) + box.Center).Round(roundDecimals);
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
