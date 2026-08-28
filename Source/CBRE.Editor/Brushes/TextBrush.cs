using CBRE.Common;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Brushes.Controls;
using CBRE.Localization;
using Poly2Tri;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Polygon = Poly2Tri.Polygon;

namespace CBRE.Editor.Brushes
{
    public class TextBrush : IBrush
    {
        private readonly FontChooserControl _fontChooser;
        private readonly NumericControl _flattenFactor;
        private readonly TextControl _text;

        public TextBrush()
        {
            _fontChooser = new FontChooserControl(this);
            _flattenFactor = new NumericControl(this) { LabelText = Local.LocalString("brush.aliasing_factor"), Minimum = 0.1m, Maximum = 10m, Value = 1, Precision = 1, Increment = 0.1m };
            _text = new TextControl(this) { EnteredText = Local.LocalString("brush.enter_text_here") };
        }

        public string Name { get { return Local.LocalString("brush.text"); } }

        public bool CanRound { get { return true; } }

        public IEnumerable<BrushControl> GetControls()
        {
            yield return _fontChooser;
            yield return _flattenFactor;
            yield return _text;
        }

        public IEnumerable<MapObject> Create(IDGenerator generator, Box box, ITexture texture, int roundDecimals)
        {
            decimal width = box.Width;
            int length = Math.Max(1, Math.Abs((int)box.Length));
            decimal height = box.Height;
            float flatten = (float)_flattenFactor.Value;
            string text = _text.GetValue();

            FontFamily family = _fontChooser.GetFontFamily();
            FontStyle style = Enum.GetValues(typeof(FontStyle)).OfType<FontStyle>().FirstOrDefault(fs => family.IsStyleAvailable(fs));
            if (!family.IsStyleAvailable(style)) family = FontFamily.GenericSansSerif;

            PolygonSet set = new PolygonSet();

            List<RectangleF> sizes = new List<RectangleF>();
            using (Bitmap bmp = new Bitmap(1, 1))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
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

            double xOffset = box.Start.DX;
            double yOffset = box.End.DY;

            for (int ci = 0; ci < text.Length; ci++)
            {
                char c = text[ci];
                RectangleF size = sizes[ci];

                GraphicsPath gp = new GraphicsPath();
                gp.AddString(c.ToString(), family, (int)style, length, new PointF(0, 0), StringFormat.GenericTypographic);
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

            decimal zOffset = box.Start.Z;

            foreach (Polygon polygon in set.Polygons)
            {
                foreach (DelaunayTriangle t in polygon.Triangles)
                {
                    List<Coordinate> points = t.Points.Select(x => new Coordinate((decimal)x.X, (decimal)x.Y, zOffset).Round(roundDecimals)).ToList();

                    List<Coordinate[]> faces = new List<Coordinate[]>();

                    // Add the vertical faces
                    Coordinate z = new Coordinate(0, 0, height).Round(roundDecimals);
                    for (int j = 0; j < points.Count; j++)
                    {
                        int next = (j + 1) % points.Count;
                        faces.Add(new[] { points[j], points[j] + z, points[next] + z, points[next] });
                    }
                    // Add the top and bottom faces
                    faces.Add(points.ToArray());
                    faces.Add(points.Select(x => x + z).Reverse().ToArray());

                    // Nothing new here, move along
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
    }
}
