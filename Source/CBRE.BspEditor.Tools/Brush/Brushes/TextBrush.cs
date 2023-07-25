using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Poly2Tri;
using Poly2Tri.Triangulation.Polygon;
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
using Polygon = Poly2Tri.Triangulation.Polygon.Polygon;

namespace CBRE.BspEditor.Tools.Brush.Brushes
{
    [Export(typeof(IBrush))]
    [Export(typeof(IInitialiseHook))]
    [OrderHint("T")]
    [AutoTranslate]
    public class TextBrush : IBrush, IInitialiseHook
    {
        private FontChooserControl _fontChooser;
        private NumericControl _flattenFactor;
        private TextControl _text;
        
        public string Font { get; set; }
        public string AliasingFactor { get; set; }
        public string Text { get; set; }
        public string EnteredText { get; set; }

        public Task OnInitialise()
        {
            _fontChooser = new FontChooserControl(this) { LabelText = Font };
            _flattenFactor = new NumericControl(this) { LabelText = AliasingFactor, Minimum = 0.1m, Maximum = 10m, Value = 1, Precision = 1, Increment = 0.1m };
            _text = new TextControl(this) { EnteredText = EnteredText, LabelText = Text };
            return Task.CompletedTask;
        }

        public string Name { get; set; } = "Text";
        public bool CanRound => true;

        public IEnumerable<BrushControl> GetControls()
        {
            yield return _fontChooser;
            yield return _flattenFactor;
            yield return _text;
        }

        public IEnumerable<IMapObject> Create(UniqueNumberGenerator generator, Box box, string texture, int roundfloats)
        {
            int length = Math.Max(1, Math.Abs((int) box.Length));
            float height = box.Height;
            float flatten = (float) _flattenFactor.Value;
            string text = _text.GetValue();

            FontFamily family = _fontChooser.GetFontFamily();
            FontStyle style = Enum.GetValues(typeof (FontStyle)).OfType<FontStyle>().FirstOrDefault(fs => family.IsStyleAvailable(fs));
            if (!family.IsStyleAvailable(style)) family = FontFamily.GenericSansSerif;

            List<Polygon> set = new List<Polygon>();

            List<RectangleF> sizes = new List<RectangleF>();
            using (Bitmap bmp = new Bitmap(1,1))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    using (Font font = new Font(family, length, style, GraphicsUnit.Pixel))
                    {
                        for (int i = 0; i < text.Length; i += 32)
                        {
                            using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
                            {
                                int rem = Math.Min(text.Length, i + 32) - i;
                                CharacterRange[] range = Enumerable.Range(0, rem).Select(x => new CharacterRange(x, 1)).ToArray();
                                sf.SetMeasurableCharacterRanges(range);
                                Region[] reg = g.MeasureCharacterRanges(text.Substring(i, rem), font, new RectangleF(0, 0, float.MaxValue, float.MaxValue), sf);
                                sizes.AddRange(reg.Select(x => x.GetBounds(g)));
                            }
                        }
                    }
                }
            }

            float xOffset = box.Start.X;
            float yOffset = box.End.Y;

            for (int ci = 0; ci < text.Length; ci++)
            {
                char c = text[ci];
                RectangleF size = sizes[ci];

                GraphicsPath gp = new GraphicsPath();
                gp.AddString(c.ToString(CultureInfo.InvariantCulture), family, (int)style, length, new PointF(0, 0), StringFormat.GenericTypographic);
                gp.Flatten(new System.Drawing.Drawing2D.Matrix(), flatten);

                List<Polygon> polygons = new List<Polygon>();
                List<PolygonPoint> poly = new List<PolygonPoint>();

                for (int i = 0; i < gp.PointCount; i++)
                {
                    byte type = gp.PathTypes[i];
                    PointF point = gp.PathPoints[i];

                    poly.Add(new PolygonPoint(point.X + xOffset, -point.Y + yOffset));

                    if ((type & 0x80) == 0x80)
                    {
                        polygons.Add(new Polygon(poly));
                        poly.Clear();
                    }
                }

                List<Polygon> tri = new List<Polygon>();
                Polygon polygon = null;
                foreach (Polygon p in polygons)
                {
                    if (polygon == null)
                    {
                        polygon = p;
                        tri.Add(p);
                    }
                    else if (p.CalculateWindingOrder() != polygon.CalculateWindingOrder())
                    {
                        polygon.AddHole(p);
                    }
                    else
                    {
                        polygon = null;
                        tri.Add(p);
                    }
                }

                foreach (Polygon pp in tri)
                {
                    try
                    {
                        P2T.Triangulate(pp);
                        set.Add(pp);
                    }
                    catch
                    {
                        // Ignore
                    }
                }

                xOffset += size.Width;
            }

            float zOffset = box.Start.Z;

            foreach (Polygon polygon in set)
            {
                foreach (Poly2Tri.Triangulation.Delaunay.DelaunayTriangle t in polygon.Triangles)
                {
                    List<Vector3> points = t.Points.Select(x => new Vector3((float) x.X, (float) x.Y, zOffset).Round(roundfloats)).ToList();

                    List<Vector3[]> faces = new List<Vector3[]>();

                    // Add the vertical faces
                    Vector3 z = new Vector3(0, 0, height).Round(roundfloats);
                    for (int j = 0; j < points.Count; j++)
                    {
                        int next = (j + 1) % points.Count;
                        faces.Add(new[] {points[j], points[j] + z, points[next] + z, points[next]});
                    }
                    // Add the top and bottom faces
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
                        face.Vertices.AddRange(arr.Select(x => x.Round(roundfloats)));
                        solid.Data.Add(face);
                    }
                    solid.DescendantsChanged();
                    yield return solid;
                }
            }
        }
    }
}
