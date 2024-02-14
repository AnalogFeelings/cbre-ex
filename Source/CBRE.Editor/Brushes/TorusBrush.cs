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
    public class TorusBrush : IBrush
    {
        private readonly NumericControl _crossSides;
        private readonly NumericControl _crossRadius;
        private readonly BooleanControl _crossMakeHollow;
        private readonly NumericControl _crossArc;
        private readonly NumericControl _crossStartAngle;
        private readonly NumericControl _crossWallWidth;

        private readonly NumericControl _ringSides;
        private readonly NumericControl _ringArc;
        private readonly NumericControl _ringStartAngle;

        private readonly NumericControl _rotationHeight;

        public TorusBrush()
        {
            _crossSides = new NumericControl(this) { LabelText = Local.LocalString("brush.cross_sides") };
            _crossRadius = new NumericControl(this) { LabelText = Local.LocalString("brush.ring_width"), Minimum = 16, Maximum = 1024, Value = 32, Precision = 1 };
            _crossStartAngle = new NumericControl(this) { LabelText = Local.LocalString("brush.cross_start"), Minimum = 0, Maximum = 359, Value = 0 };
            _crossMakeHollow = new BooleanControl(this) { LabelText = Local.LocalString("brush.make_hollow"), Checked = false };
            _crossArc = new NumericControl(this) { LabelText = Local.LocalString("brush.cross_arc"), Minimum = 1, Maximum = 360, Value = 360, Enabled = false };
            _crossWallWidth = new NumericControl(this) { LabelText = Local.LocalString("brush.hollow_wall_width"), Minimum = 1, Maximum = 1024, Value = 16, Precision = 1, Enabled = false };
            _ringSides = new NumericControl(this) { LabelText = Local.LocalString("brush.ring_sides") };
            _ringArc = new NumericControl(this) { LabelText = Local.LocalString("brush.ring_arc"), Minimum = 1, Maximum = 1080, Value = 360 };
            _ringStartAngle = new NumericControl(this) { LabelText = Local.LocalString("brush.ring_start"), Minimum = 0, Maximum = 359, Value = 0 };
            _rotationHeight = new NumericControl(this) { LabelText = Local.LocalString("brush.rotation_height"), Minimum = -1024, Maximum = 1024, Value = 0, Precision = 1 };
            _crossMakeHollow.ValuesChanged += (s, b) => _crossWallWidth.Enabled = _crossArc.Enabled = _crossMakeHollow.GetValue();
        }

        public string Name
        {
            get { return Local.LocalString("brush.torus"); }
        }

        public bool CanRound { get { return true; } }

        public IEnumerable<BrushControl> GetControls()
        {
            yield return _crossSides;
            yield return _crossRadius;
            yield return _crossStartAngle;
            yield return _crossMakeHollow;
            yield return _crossArc;
            yield return _crossWallWidth;
            yield return _ringSides;
            yield return _ringArc;
            yield return _ringStartAngle;
            yield return _rotationHeight;
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
            int crossSides = (int)_crossSides.GetValue();
            if (crossSides < 3) yield break;
            decimal crossWidth = _crossRadius.GetValue() * 2;
            if (crossWidth < 1) yield break;
            bool crossMakeHollow = _crossMakeHollow.GetValue();
            int crossArc = !crossMakeHollow ? 360 : (int)_crossArc.GetValue();
            if (crossArc < 1) yield break;
            int crossStartAngle = (int)_crossStartAngle.GetValue();
            if (crossStartAngle < 0 || crossStartAngle > 359) yield break;
            decimal crossWallWidth = _crossWallWidth.GetValue();
            if (crossWallWidth < 1) yield break;
            int ringSides = (int)_ringSides.GetValue();
            if (ringSides < 3) yield break;
            int ringArc = (int)_ringArc.GetValue();
            if (ringArc < 1) yield break;
            int ringStartAngle = (int)_ringStartAngle.GetValue();
            if (ringStartAngle < 0 || ringStartAngle > 359) yield break;
            decimal rotationHeight = _rotationHeight.GetValue();

            // Sort of a combination of cylinder and pipe brushes
            decimal width = box.Width;
            decimal length = box.Length;
            decimal height = box.Height;
            decimal majorPrimary = (width - crossWidth) / 2; // Primary = donut circle
            decimal minorPrimary = (length - crossWidth) / 2;
            decimal majorSecondaryOuter = crossWidth / 2; // Secondary = cross section circle
            decimal minorSecondaryOuter = height / 2; // Outer = Outer ring
            decimal majorSecondaryInner = (crossWidth - crossWallWidth) / 2; // Inner = inner ring (hollow only)
            decimal minorSecondaryInner = (height - crossWallWidth) / 2;

            decimal ringStart = DMath.DegreesToRadians(ringStartAngle);
            decimal ringAngle = DMath.DegreesToRadians(ringArc) / ringSides;
            decimal crossStart = DMath.DegreesToRadians(crossStartAngle);
            decimal crossAngle = DMath.DegreesToRadians(crossArc) / crossSides;
            decimal heightAdd = rotationHeight / ringSides;

            // Rotate around the ring, generating each cross section
            List<Coordinate[]> ringOuterSections = new List<Coordinate[]>();
            List<Coordinate[]> ringInnerSections = new List<Coordinate[]>();
            for (int i = 0; i < ringSides + 1; i++)
            {
                decimal ring = ringStart + i * ringAngle;
                decimal rxval = box.Center.X + majorPrimary * DMath.Cos(ring);
                decimal ryval = box.Center.Y + minorPrimary * DMath.Sin(ring);
                decimal rzval = box.Center.Z;
                Coordinate[] crossSecOuter = new Coordinate[crossSides + 1];
                Coordinate[] crossSecInner = new Coordinate[crossSides + 1];
                for (int j = 0; j < crossSides + 1; j++)
                {
                    decimal cross = crossStart + j * crossAngle;
                    decimal xval = majorSecondaryOuter * DMath.Cos(cross) * DMath.Cos(ring);
                    decimal yval = majorSecondaryOuter * DMath.Cos(cross) * DMath.Sin(ring);
                    decimal zval = minorSecondaryOuter * DMath.Sin(cross);
                    crossSecOuter[j] = new Coordinate(xval + rxval, yval + ryval, zval + rzval).Round(roundDecimals);
                    if (!crossMakeHollow) continue;
                    xval = majorSecondaryInner * DMath.Cos(cross) * DMath.Cos(ring);
                    yval = majorSecondaryInner * DMath.Cos(cross) * DMath.Sin(ring);
                    zval = minorSecondaryInner * DMath.Sin(cross);
                    crossSecInner[j] = new Coordinate(xval + rxval, yval + ryval, zval + rzval).Round(roundDecimals);
                }
                ringOuterSections.Add(crossSecOuter);
                ringInnerSections.Add(crossSecInner);
            }

            // Create the solids
            Color colour = Colour.GetRandomBrushColour();
            for (int i = 0; i < ringSides; i++)
            {
                Coordinate vertical = Coordinate.UnitZ * heightAdd * i;
                int nexti = i + 1;
                if (crossMakeHollow)
                {
                    // Use pipe cross sections
                    Coordinate[] outerPoints = ringOuterSections[i];
                    Coordinate[] nextOuterPoints = ringOuterSections[nexti];
                    Coordinate[] innerPoints = ringInnerSections[i];
                    Coordinate[] nextInnerPoints = ringInnerSections[nexti];
                    for (int j = 0; j < crossSides; j++)
                    {
                        int nextj = j + 1;
                        List<Coordinate[]> faces = new List<Coordinate[]>
                        {
                            new[] { outerPoints[j], outerPoints[nextj], nextOuterPoints[nextj], nextOuterPoints[j] }.Select(x => x + vertical).ToArray(),
                            new[] { nextInnerPoints[j], nextInnerPoints[nextj], innerPoints[nextj], innerPoints[j] }.Select(x => x + vertical).ToArray(),
                            new[] { innerPoints[nextj], nextInnerPoints[nextj], nextOuterPoints[nextj], outerPoints[nextj] }.Select(x => x + vertical).ToArray(),
                            new[] { outerPoints[j], nextOuterPoints[j], nextInnerPoints[j], innerPoints[j] }.Select(x => x + vertical).ToArray(),
                            new[] { innerPoints[j], innerPoints[nextj], outerPoints[nextj], outerPoints[j] }.Select(x => x + vertical).ToArray(),
                            new[] { nextOuterPoints[j], nextOuterPoints[nextj], nextInnerPoints[nextj], nextInnerPoints[j] }.Select(x => x + vertical).ToArray()
                        };
                        yield return MakeSolid(generator, faces, texture, colour);
                    }
                }
                else
                {
                    // Use cylindrical cross sections
                    List<Coordinate[]> faces = new List<Coordinate[]>();
                    Coordinate[] points = ringOuterSections[i];
                    Coordinate[] nextPoints = ringOuterSections[nexti];
                    // Add the outer faces
                    for (int j = 0; j < crossSides; j++)
                    {
                        int nextj = (j + 1) % crossSides;
                        faces.Add(new[] { points[j], points[nextj], nextPoints[nextj], nextPoints[j] }.Select(x => x + vertical).ToArray());
                    }
                    // Add the cross section faces
                    faces.Add(points.Reverse().Select(x => x + vertical).ToArray());
                    faces.Add(nextPoints.Select(x => x + vertical).ToArray());
                    yield return MakeSolid(generator, faces, texture, colour);
                }
            }
        }
    }
}
