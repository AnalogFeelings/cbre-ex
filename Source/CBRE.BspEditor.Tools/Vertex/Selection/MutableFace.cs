using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.Common.Threading;
using CBRE.DataStructures.Geometric;
using Plane = CBRE.DataStructures.Geometric.Plane;

namespace CBRE.BspEditor.Tools.Vertex.Selection
{
    public class MutableFace
    {
        public IList<MutableVertex> Vertices { get; }
        public Primitives.Texture Texture { get; set; }
        public Plane Plane => new Plane(Vertices[0].Position, Vertices[1].Position, Vertices[2].Position);
        public Vector3 Origin => Vertices.Aggregate(Vector3.Zero, (a, b) => a + b.Position) / Vertices.Count;

        public MutableFace(Face face)
        {
            Vertices = new ThreadSafeList<MutableVertex>(face.Vertices.Select(x => new MutableVertex(x)));
            Texture = face.Texture.Clone();
        }

        public MutableFace(IEnumerable<Vector3> vertices, Primitives.Texture texture)
        {
            Vertices = new ThreadSafeList<MutableVertex>(vertices.Select(x => new MutableVertex(x)));
            Texture = texture;
        }

        public void Transform(Matrix4x4 matrix)
        {
            foreach (MutableVertex v in Vertices)
            {
                v.Set(Vector3.Transform(v.Position, matrix));
            }
        }

        public virtual IEnumerable<Line> GetEdges()
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                yield return new Line(Vertices[i].Position, Vertices[(i + 1) % Vertices.Count].Position);
            }
        }

        public virtual IEnumerable<Tuple<Vector3, float, float>> GetTextureCoordinates(int width, int height)
        {
            if (width <= 0 || height <= 0 || Texture.XScale == 0 || Texture.YScale == 0)
            {
                return Vertices.Select(x => Tuple.Create(x.Position, 0f, 0f));
            }

            float udiv = width * Texture.XScale;
            float uadd = Texture.XShift / width;
            float vdiv = height * Texture.YScale;
            float vadd = Texture.YShift / height;

            return Vertices.Select(x => Tuple.Create(x.Position, x.Position.Dot(Texture.UAxis) / udiv + uadd, x.Position.Dot(Texture.VAxis) / vdiv + vadd));
        }

        public Face ToFace(UniqueNumberGenerator idGenerator)
        {
            Face f = new Face(idGenerator.Next("Face"))
            {
                Texture = Texture.Clone()
            };

            f.Vertices.AddRange(Vertices.Select(x => x.Position));

            return f;
        }
    }
}