using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
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
    [OrderHint("H")]
    [AutoTranslate]
    public class ArchBrush : IBrush, IInitialiseHook
    {
        private NumericControl _numSides;
        private NumericControl _wallWidth;
        private NumericControl _arc;
        private NumericControl _startAngle;
        private NumericControl _addHeight;
        private BooleanControl _curvedRamp;
        private NumericControl _tiltAngle;
        private BooleanControl _tiltInterp;

        private const decimal Atan2 = 63.4m;

        public string NumberOfSides { get; set; }
        public string WallWidth { get; set; }
        public string Arc { get; set; }
        public string StartAngle { get; set; }
        public string AddHeight { get; set; }
        public string CurvedRamp { get; set; }
        public string TiltAngle { get; set; }
        public string TiltInterpolation { get; set; }

        public async Task OnInitialise()
        {
            _numSides = new NumericControl(this) { LabelText = NumberOfSides };
            _wallWidth = new NumericControl(this) { LabelText = WallWidth, Minimum = 1, Maximum = 1024, Value = 32, Precision = 1 };
            _arc = new NumericControl(this) { LabelText = Arc, Minimum = 1, Maximum = 360 * 4, Value = 360 };
            _startAngle = new NumericControl(this) { LabelText = StartAngle, Minimum = 0, Maximum = 359, Value = 0 };
            _addHeight = new NumericControl(this) { LabelText = AddHeight, Minimum = -1024, Maximum = 1024, Value = 0, Precision = 1 };
            _curvedRamp = new BooleanControl(this) { LabelText = CurvedRamp, Checked = false };
            _tiltAngle = new NumericControl(this) { LabelText = TiltAngle, Minimum = -Atan2, Maximum = Atan2, Value = 0, Enabled = false, Precision = 1 };
            _tiltInterp = new BooleanControl(this) { LabelText = TiltInterpolation, Checked = false, Enabled = false };

            _curvedRamp.ValuesChanged += (s, b) => _tiltAngle.Enabled = _tiltInterp.Enabled = _curvedRamp.GetValue();
        }

        public string Name { get; set; } = "Arch";
        public bool CanRound => true;

        public IEnumerable<BrushControl> GetControls()
        {
            yield return _numSides;
            yield return _wallWidth;
            yield return _arc;
            yield return _startAngle;
            yield return _addHeight;
            yield return _curvedRamp;
            yield return _tiltAngle;
            yield return _tiltInterp;
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
                    Texture = { Name = texture }
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
            float wallWidth = (float) _wallWidth.GetValue();
            if (wallWidth < 1) yield break;
            float arc = (float) _arc.GetValue();
            if (arc < 1) yield break;
            float startAngle = (float) _startAngle.GetValue();
            if (startAngle < 0 || startAngle > 359) yield break;
            float addHeight = (float) _addHeight.GetValue();
            bool curvedRamp = _curvedRamp.GetValue();
            float tiltAngle = curvedRamp ? (float) _tiltAngle.GetValue() : 0;
            if (Math.Abs(Math.Abs(tiltAngle % 180) - 90) < 0.001f) yield break;
            bool tiltInterp = curvedRamp && _tiltInterp.GetValue();

            // Very similar to the pipe brush, except with options for start angle, arc, height and tilt
            float width = box.Width;
            float length = box.Length;
            float height = box.Height;

            float majorOut = width / 2;
            float majorIn = majorOut - wallWidth;
            float minorOut = length / 2;
            float minorIn = minorOut - wallWidth;

            float start = (float) MathHelper.DegreesToRadians(startAngle);
            float tilt = (float) MathHelper.DegreesToRadians(tiltAngle);
            float angle = (float) MathHelper.DegreesToRadians(arc) / numSides;

            // Calculate the coordinates of the inner and outer ellipses' points
            Vector3[] outer = new Vector3[numSides + 1];
            Vector3[] inner = new Vector3[numSides + 1];
            for (int i = 0; i < numSides + 1; i++)
            {
                float a = start + i * angle;
                float h = i * addHeight;
                float interp = tiltInterp ? (float) Math.Cos(Math.PI / numSides * (i - numSides / 2f)) : 1;
                float tiltHeight = wallWidth / 2 * interp * (float) Math.Tan(tilt);

                float xval = box.Center.X + majorOut * (float) Math.Cos(a);
                float yval = box.Center.Y + minorOut * (float) Math.Sin(a);
                float zval = box.Start.Z + (curvedRamp ? h + tiltHeight : 0);
                outer[i] = new Vector3(xval, yval, zval).Round(roundDecimals);

                xval = box.Center.X + majorIn * (float) Math.Cos(a);
                yval = box.Center.Y + minorIn * (float) Math.Sin(a);
                zval = box.Start.Z + (curvedRamp ? h - tiltHeight : 0);
                inner[i] = new Vector3(xval, yval, zval).Round(roundDecimals);
            }

            // Create the solids
            Color colour = Colour.GetRandomBrushColour();
            Vector3 z = new Vector3(0, 0, height).Round(roundDecimals);
            for (int i = 0; i < numSides; i++)
            {
                List<Vector3[]> faces = new List<Vector3[]>();

                // Since we are triangulating/splitting each arch segment, we need to generate 2 brushes per side
                if (curvedRamp)
                {
                    // The splitting orientation depends on the curving direction of the arch
                    if (addHeight >= 0)
                    {
                        faces.Add(new[] { outer[i],       outer[i] + z,   outer[i+1] + z, outer[i+1] });
                        faces.Add(new[] { outer[i+1],     outer[i+1] + z, inner[i] + z,   inner[i]   });
                        faces.Add(new[] { inner[i],       inner[i] + z,   outer[i] + z,   outer[i]   });
                        faces.Add(new[] { outer[i] + z,   inner[i] + z,   outer[i+1] + z  });
                        faces.Add(new[] { outer[i+1],     inner[i],       outer[i]        });
                    }
                    else
                    {
                        faces.Add(new[] { inner[i+1],     inner[i+1] + z, inner[i] + z,   inner[i]   });
                        faces.Add(new[] { outer[i],       outer[i] + z,   inner[i+1] + z, inner[i+1] });
                        faces.Add(new[] { inner[i],       inner[i] + z,   outer[i] + z,   outer[i]   });
                        faces.Add(new[] { inner[i+1] + z, outer[i] + z,   inner[i] + z    });
                        faces.Add(new[] { inner[i],       outer[i],       inner[i+1]      });
                    }
                    yield return MakeSolid(generator, faces, texture, colour);

                    faces.Clear();

                    if (addHeight >= 0)
                    {
                        faces.Add(new[] { inner[i+1],     inner[i+1] + z, inner[i] + z,   inner[i]   });
                        faces.Add(new[] { inner[i],       inner[i] + z,   outer[i+1] + z, outer[i+1] });
                        faces.Add(new[] { outer[i+1],     outer[i+1] + z, inner[i+1] + z, inner[i+1] });
                        faces.Add(new[] { inner[i+1] + z, outer[i+1] + z, inner[i] + z    });
                        faces.Add(new[] { inner[i],       outer[i+1],     inner[i+1]      });
                    }
                    else
                    {
                        faces.Add(new[] { outer[i],       outer[i] + z,   outer[i+1] + z, outer[i+1] });
                        faces.Add(new[] { inner[i+1],     inner[i+1] + z, outer[i] + z,   outer[i]   });
                        faces.Add(new[] { outer[i+1],     outer[i+1] + z, inner[i+1] + z, inner[i+1] });
                        faces.Add(new[] { outer[i] + z,   inner[i+1] + z, outer[i+1] + z  });
                        faces.Add(new[] { outer[i+1],     inner[i+1],     outer[i]        });
                    }
                    yield return MakeSolid(generator, faces, texture, colour);
                }
                else
                {
                    Vector3 h = i * addHeight * Vector3.UnitZ;
                    faces.Add(new[] { outer[i],       outer[i] + z,   outer[i+1] + z, outer[i+1]   }.Select(x => x + h).ToArray());
                    faces.Add(new[] { inner[i+1],     inner[i+1] + z, inner[i] + z,   inner[i]     }.Select(x => x + h).ToArray());
                    faces.Add(new[] { outer[i+1],     outer[i+1] + z, inner[i+1] + z, inner[i+1]   }.Select(x => x + h).ToArray());
                    faces.Add(new[] { inner[i],       inner[i] + z,   outer[i] + z,   outer[i]     }.Select(x => x + h).ToArray());
                    faces.Add(new[] { inner[i+1] + z, outer[i+1] + z, outer[i] + z,   inner[i] + z }.Select(x => x + h).ToArray());
                    faces.Add(new[] { inner[i],       outer[i],       outer[i+1],     inner[i+1]   }.Select(x => x + h).ToArray());
                    yield return MakeSolid(generator, faces, texture, colour);
                }
            }
        }
    }
}
