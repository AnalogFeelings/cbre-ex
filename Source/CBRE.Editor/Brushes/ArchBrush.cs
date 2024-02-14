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
    public class ArchBrush : IBrush
    {
        private readonly NumericControl _numSides;
        private readonly NumericControl _wallWidth;
        private readonly NumericControl _arc;
        private readonly NumericControl _startAngle;
        private readonly NumericControl _addHeight;
        private readonly BooleanControl _curvedRamp;
        private readonly NumericControl _tiltAngle;
        private readonly BooleanControl _tiltInterp;

        private const decimal Atan2 = 63.4m;

        public ArchBrush()
        {
            _numSides = new NumericControl(this) { LabelText = Local.LocalString("brush.sides") };
            _wallWidth = new NumericControl(this) { LabelText = Local.LocalString("brush.width"), Minimum = 1, Maximum = 1024, Value = 32, Precision = 1 };
            _arc = new NumericControl(this) { LabelText = Local.LocalString("brush.arc"), Minimum = 1, Maximum = 360 * 4, Value = 360 };
            _startAngle = new NumericControl(this) { LabelText = Local.LocalString("brush.angle"), Minimum = 0, Maximum = 359, Value = 0 };
            _addHeight = new NumericControl(this) { LabelText = Local.LocalString("brush.height"), Minimum = -1024, Maximum = 1024, Value = 0, Precision = 1 };
            _curvedRamp = new BooleanControl(this) { LabelText = Local.LocalString("brush.ramp"), Checked = false };
            _tiltAngle = new NumericControl(this) { LabelText = Local.LocalString("brush.tilt_angle"), Minimum = -Atan2, Maximum = Atan2, Value = 0, Enabled = false, Precision = 1 };
            _tiltInterp = new BooleanControl(this) { LabelText = Local.LocalString("brush.tilt_interpolation"), Checked = false, Enabled = false };

            _curvedRamp.ValuesChanged += (s, b) => _tiltAngle.Enabled = _tiltInterp.Enabled = _curvedRamp.GetValue();
        }

        public string Name
        {
            get { return Local.LocalString("brush.arch"); }
        }

        public bool CanRound { get { return true; } }

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
            decimal wallWidth = _wallWidth.GetValue();
            if (wallWidth < 1) yield break;
            decimal arc = _arc.GetValue();
            if (arc < 1) yield break;
            decimal startAngle = _startAngle.GetValue();
            if (startAngle < 0 || startAngle > 359) yield break;
            decimal addHeight = _addHeight.GetValue();
            bool curvedRamp = _curvedRamp.GetValue();
            decimal tiltAngle = curvedRamp ? _tiltAngle.GetValue() : 0;
            if (DMath.Abs(tiltAngle % 180) == 90) yield break;
            bool tiltInterp = curvedRamp && _tiltInterp.GetValue();

            // Very similar to the pipe brush, except with options for start angle, arc, height and tilt
            decimal width = box.Width;
            decimal length = box.Length;
            decimal height = box.Height;

            decimal majorOut = width / 2;
            decimal majorIn = majorOut - wallWidth;
            decimal minorOut = length / 2;
            decimal minorIn = minorOut - wallWidth;

            decimal start = DMath.DegreesToRadians(startAngle);
            decimal tilt = DMath.DegreesToRadians(tiltAngle);
            decimal angle = DMath.DegreesToRadians(arc) / numSides;

            // Calculate the coordinates of the inner and outer ellipses' points
            Coordinate[] outer = new Coordinate[numSides + 1];
            Coordinate[] inner = new Coordinate[numSides + 1];
            for (int i = 0; i < numSides + 1; i++)
            {
                decimal a = start + i * angle;
                decimal h = i * addHeight;
                decimal interp = tiltInterp ? DMath.Cos(DMath.PI / numSides * (i - numSides / 2M)) : 1;
                decimal tiltHeight = wallWidth / 2 * interp * DMath.Tan(tilt);

                decimal xval = box.Center.X + majorOut * DMath.Cos(a);
                decimal yval = box.Center.Y + minorOut * DMath.Sin(a);
                decimal zval = box.Start.Z + (curvedRamp ? h + tiltHeight : 0);
                outer[i] = new Coordinate(xval, yval, zval).Round(roundDecimals);

                xval = box.Center.X + majorIn * DMath.Cos(a);
                yval = box.Center.Y + minorIn * DMath.Sin(a);
                zval = box.Start.Z + (curvedRamp ? h - tiltHeight : 0);
                inner[i] = new Coordinate(xval, yval, zval).Round(roundDecimals);
            }

            // Create the solids
            Color colour = Colour.GetRandomBrushColour();
            Coordinate z = new Coordinate(0, 0, height).Round(roundDecimals);
            for (int i = 0; i < numSides; i++)
            {
                List<Coordinate[]> faces = new List<Coordinate[]>();

                // Since we are triangulating/splitting each arch segment, we need to generate 2 brushes per side
                if (curvedRamp)
                {
                    // The splitting orientation depends on the curving direction of the arch
                    if (addHeight >= 0)
                    {
                        faces.Add(new[] { outer[i], outer[i] + z, outer[i + 1] + z, outer[i + 1] });
                        faces.Add(new[] { outer[i + 1], outer[i + 1] + z, inner[i] + z, inner[i] });
                        faces.Add(new[] { inner[i], inner[i] + z, outer[i] + z, outer[i] });
                        faces.Add(new[] { outer[i] + z, inner[i] + z, outer[i + 1] + z });
                        faces.Add(new[] { outer[i + 1], inner[i], outer[i] });
                    }
                    else
                    {
                        faces.Add(new[] { inner[i + 1], inner[i + 1] + z, inner[i] + z, inner[i] });
                        faces.Add(new[] { outer[i], outer[i] + z, inner[i + 1] + z, inner[i + 1] });
                        faces.Add(new[] { inner[i], inner[i] + z, outer[i] + z, outer[i] });
                        faces.Add(new[] { inner[i + 1] + z, outer[i] + z, inner[i] + z });
                        faces.Add(new[] { inner[i], outer[i], inner[i + 1] });
                    }
                    yield return MakeSolid(generator, faces, texture, colour);

                    faces.Clear();

                    if (addHeight >= 0)
                    {
                        faces.Add(new[] { inner[i + 1], inner[i + 1] + z, inner[i] + z, inner[i] });
                        faces.Add(new[] { inner[i], inner[i] + z, outer[i + 1] + z, outer[i + 1] });
                        faces.Add(new[] { outer[i + 1], outer[i + 1] + z, inner[i + 1] + z, inner[i + 1] });
                        faces.Add(new[] { inner[i + 1] + z, outer[i + 1] + z, inner[i] + z });
                        faces.Add(new[] { inner[i], outer[i + 1], inner[i + 1] });
                    }
                    else
                    {
                        faces.Add(new[] { outer[i], outer[i] + z, outer[i + 1] + z, outer[i + 1] });
                        faces.Add(new[] { inner[i + 1], inner[i + 1] + z, outer[i] + z, outer[i] });
                        faces.Add(new[] { outer[i + 1], outer[i + 1] + z, inner[i + 1] + z, inner[i + 1] });
                        faces.Add(new[] { outer[i] + z, inner[i + 1] + z, outer[i + 1] + z });
                        faces.Add(new[] { outer[i + 1], inner[i + 1], outer[i] });
                    }
                    yield return MakeSolid(generator, faces, texture, colour);
                }
                else
                {
                    Coordinate h = i * addHeight * Coordinate.UnitZ;
                    faces.Add(new[] { outer[i], outer[i] + z, outer[i + 1] + z, outer[i + 1] }.Select(x => x + h).ToArray());
                    faces.Add(new[] { inner[i + 1], inner[i + 1] + z, inner[i] + z, inner[i] }.Select(x => x + h).ToArray());
                    faces.Add(new[] { outer[i + 1], outer[i + 1] + z, inner[i + 1] + z, inner[i + 1] }.Select(x => x + h).ToArray());
                    faces.Add(new[] { inner[i], inner[i] + z, outer[i] + z, outer[i] }.Select(x => x + h).ToArray());
                    faces.Add(new[] { inner[i + 1] + z, outer[i + 1] + z, outer[i] + z, inner[i] + z }.Select(x => x + h).ToArray());
                    faces.Add(new[] { inner[i], outer[i], outer[i + 1], inner[i + 1] }.Select(x => x + h).ToArray());
                    yield return MakeSolid(generator, faces, texture, colour);
                }
            }
        }
    }
}
