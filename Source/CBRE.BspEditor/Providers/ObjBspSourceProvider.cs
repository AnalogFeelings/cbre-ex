using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CBRE.BspEditor.Environment;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common;
using CBRE.Common.Shell.Documents;
using CBRE.DataStructures.Geometric;
using Plane = CBRE.DataStructures.Geometric.Plane;

namespace CBRE.BspEditor.Providers
{
    [Export(typeof(IBspSourceProvider))]
    public class ObjBspSourceProvider : IBspSourceProvider
    {
        private static readonly IEnumerable<Type> SupportedTypes = new List<Type>
        {
            // CBRE only supports solids in the OBJ format
            typeof(Solid),
        };

        public IEnumerable<Type> SupportedDataTypes => SupportedTypes;

        public IEnumerable<FileExtensionInfo> SupportedFileExtensions { get; } = new[]
        {
            new FileExtensionInfo("Wavefront model format", ".obj")
        };

        public Task<BspFileLoadResult> Load(Stream stream, IEnvironment environment)
        {
            return Task.Run(() =>
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.ASCII, true, 1024, false))
                {
                    BspFileLoadResult result = new BspFileLoadResult();

                    Map map = new Map();

                    Read(map, reader);

                    result.Map = map;
                    return result;
                }
            });
        }

        public Task Save(Stream stream, Map map)
        {
            return Task.Run(() =>
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.ASCII, 1024, true))
                {
                    Write(map, writer);
                }
            });
        }

        #region Reading

        private struct ObjFace
        {
            public string Group { get; }
            public List<int> Vertices { get; }

            public ObjFace(string group, IEnumerable<int> vertices) : this()
            {
                Group = group;
                Vertices = vertices.ToList();
            }
        }

        private string CleanLine(string line)
        {
            if (line == null) return null;
            return line.StartsWith("#") ? "" : line.Trim();
        }

        private void Read(Map map, StreamReader reader)
        {
            const NumberStyles ns = NumberStyles.Float;

            List<Vector3> points = new List<Vector3>();
            List<ObjFace> faces = new List<ObjFace>();
            string currentGroup = "default";
            float scale = 100f;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("# Scale: "))
                {
                    string num = line.Substring(9);
                    if (float.TryParse(num, NumberStyles.Float, CultureInfo.InvariantCulture, out float s))
                    {
                        scale = s;
                    }
                }

                line = CleanLine(line);
                SplitLine(line, out string keyword, out string values);
                if (String.IsNullOrWhiteSpace(keyword)) continue;

                string[] vals = (values ?? "").Split(' ').Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
                switch (keyword.ToLower())
                {
                    // Things I care about
                    case "v": // geometric vertices
                        Vector3 vec = NumericsExtensions.Parse(vals[0], vals[1], vals[2], ns, CultureInfo.InvariantCulture);
                        points.Add(vec * scale);
                        break;
                    case "f": // face
                        faces.Add(new ObjFace(currentGroup, vals.Select(x => ParseFaceIndex(points, x))));
                        break;
                    case "g": // group name
                        currentGroup = (values ?? "").Trim();
                        break;

                    // Things I don't care about
                    #region Not Implemented

                    // Vertex data
                    // "v"
                    case "vt": // texture vertices
                        break;
                    case "vn": // vertex normals
                        break;
                    case "vp": // parameter space vertices
                    case "cstype": // rational or non-rational forms of curve or surface type: basis matrix, Bezier, B-spline, Cardinal, Taylor
                    case "degree": // degree
                    case "bmat": // basis matrix
                    case "step": // step size
                        // not supported
                        break;

                    // Elements
                    // "f"
                    case "p": // point
                    case "l": // line
                    case "curv": // curve
                    case "curv2": // 2D curve
                    case "surf": // surface
                        // not supported
                        break;

                    // Free-form curve/surface body statements
                    case "parm": // parameter name
                    case "trim": // outer trimming loop (trim)
                    case "hole": // inner trimming loop (hole)
                    case "scrv": // special curve (scrv)
                    case "sp":  // special point (sp)
                    case "end": // end statement (end)
                        // not supported
                        break;

                    // Connectivity between free-form surfaces
                    case "con": // connect
                        // not supported
                        break;

                    // Grouping
                    // "g"
                    case "s": // smoothing group
                        break;
                    case "mg": // merging group
                        break;
                    case "o": // object name
                        // not supported
                        break;

                    // Display/render attributes
                    case "mtllib": // material library
                    case "usemtl": // material name
                    case "usemap": // texture map name
                    case "bevel": // bevel interpolation
                    case "c_interp": // color interpolation
                    case "d_interp": // dissolve interpolation
                    case "lod": // level of detail
                    case "shadow_obj": // shadow casting
                    case "trace_obj": // ray tracing
                    case "ctech": // curve approximation technique
                    case "stech": // surface approximation technique
                        // not relevant
                        break;

                        #endregion
                }
            }

            List<Solid> solids = new List<Solid>();

            // Try and see if we have a valid solid per-group
            foreach (IGrouping<string, ObjFace> g in faces.GroupBy(x => x.Group))
            {
                solids.AddRange(CreateSolids(map, points, g));
            }

            foreach (Solid solid in solids)
            {
                foreach (Face face in solid.Faces)
                {
                    face.Texture.AlignToNormal(face.Plane.Normal);
                }

                solid.Hierarchy.Parent = map.Root;
            }

            map.Root.DescendantsChanged();
        }

        private IEnumerable<Solid> CreateSolids(Map map, List<Vector3> points, IEnumerable<ObjFace> objFaces)
        {
            List<Face> faces = objFaces.Select(x => CreateFace(map, points, x)).ToList();

            // See if the solid is valid
            Solid solid = new Solid(map.NumberGenerator.Next("MapObject"));
            solid.Data.Add(new ObjectColor(Colour.GetRandomBrushColour()));
            solid.Data.AddRange(faces);
            if (solid.IsValid())
            {
                // Do an additional check to ensure that all edges are shared
                List<Line> edges = solid.Faces.SelectMany(x => x.GetEdges()).ToList();
                if (edges.All(x => edges.Count(y => x.EquivalentTo(y)) == 2))
                {
                    // Valid! let's get out of here!
                    yield return solid;
                    yield break;
                }
            }

            // Not a valid solid, decompose into tetrahedrons/etc
            foreach (Face face in faces)
            {
                Polygon polygon = face.ToPolygon();
                if (!polygon.IsValid() || !polygon.IsConvex())
                {
                    // tetrahedrons
                    foreach (Vector3[] triangle in GetTriangles(face))
                    {
                        Face tf = new Face(map.NumberGenerator.Next("Face"))
                        {
                            Plane = new Plane(triangle[0], triangle[1], triangle[2])
                        };
                        tf.Vertices.AddRange(triangle);
                        yield return SolidifyFace(map, tf);
                    }
                }
                else
                {
                    // cone/pyramid/whatever
                    yield return SolidifyFace(map, face);
                }
            }
        }

        private IEnumerable<Vector3[]> GetTriangles(Face face)
        {
            for (int i = 1; i < face.Vertices.Count - 1; i++)
            {
                yield return new[]
                {
                    face.Vertices[0],
                    face.Vertices[i],
                    face.Vertices[i + 1]
                };
            }
        }

        private Solid SolidifyFace(Map map, Face face)
        {
            Solid solid = new Solid(map.NumberGenerator.Next("MapObject"));
            solid.Data.Add(new ObjectColor(Colour.GetRandomBrushColour()));
            solid.Data.Add(face);

            Vector3 center = face.Vertices.Aggregate(Vector3.Zero, (sum, v) => sum + v) / face.Vertices.Count;
            Vector3 offset = center - face.Plane.Normal * 5;
            for (int i = 0; i < face.Vertices.Count; i++)
            {
                Vector3 v1 = face.Vertices[i];
                Vector3 v2 = face.Vertices[(i + 1) % face.Vertices.Count];

                Face f = new Face(map.NumberGenerator.Next("Face"))
                {
                    Plane = new Plane(v1, offset, v2)
                };

                f.Vertices.Add(offset);
                f.Vertices.Add(v2);
                f.Vertices.Add(v1);

                solid.Data.Add(f);
            }

            solid.DescendantsChanged();
            return solid;
        }


        private Face CreateFace(Map map, List<Vector3> points, ObjFace objFace)
        {
            List<Vector3> verts = objFace.Vertices.Select(x => points[x]).ToList();

            Face f = new Face(map.NumberGenerator.Next("Face"))
            {
                Plane = new Plane(verts[2], verts[1], verts[0])
            };

            verts.Reverse();
            f.Vertices.AddRange(verts);

            return f;
        }

        private int ParseFaceIndex(List<Vector3> list, string index)
        {
            if (index.Contains('/')) index = index.Substring(0, index.IndexOf('/'));
            int idx = int.Parse(index);

            if (idx < 0)
            {
                idx = list.Count + idx;
            }
            else
            {
                idx -= 1;
            }
            //
            return idx;
        }

        private static void SplitLine(string line, out string keyword, out string arguments)
        {
            int idx = line.IndexOf(' ');
            if (idx < 0)
            {
                keyword = line;
                arguments = null;
                return;
            }

            keyword = line.Substring(0, idx);
            arguments = line.Substring(idx + 1);
        }

        #endregion

        #region Writing

        private void Write(Map map, StreamWriter writer)
        {
            writer.WriteLine("# CBRE Object Export");
            writer.WriteLine("# Scale: 1");
            writer.WriteLine();

            foreach (Solid solid in map.Root.Find(x => x is Solid).OfType<Solid>())
            {
                writer.Write("g solid_");
                writer.Write(solid.ID);
                writer.WriteLine();

                foreach (Face face in solid.Faces)
                {
                    foreach (Vector3 v in face.Vertices)
                    {
                        writer.Write("v ");
                        writer.Write(v.X.ToString("0.0000", CultureInfo.InvariantCulture));
                        writer.Write(' ');
                        writer.Write(v.Y.ToString("0.0000", CultureInfo.InvariantCulture));
                        writer.Write(' ');
                        writer.Write(v.Z.ToString("0.0000", CultureInfo.InvariantCulture));
                        writer.WriteLine();
                    }

                    writer.Write("f ");
                    for (int i = 1; i <= face.Vertices.Count; i++)
                    {
                        writer.Write(-i);
                        writer.Write(' ');
                    }
                    writer.WriteLine();
                    writer.WriteLine();
                }
            }
        }

        #endregion
    }
}
