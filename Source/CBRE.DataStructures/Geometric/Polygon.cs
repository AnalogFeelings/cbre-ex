using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;

namespace CBRE.DataStructures.Geometric
{
    /// <summary>
    /// Represents a coplanar, directed polygon with at least 3 vertices.
    /// </summary>
    [Serializable]
    public class Polygon : ISerializable
    {
        public IReadOnlyList<Vector3> Vertices { get; }

        /// <summary>
        /// Returns the origin of this polygon.
        /// </summary>
        /// <returns></returns>
        public Vector3 Origin => Vertices.Aggregate(Vector3.Zero, (x, y) => x + y) / Vertices.Count;

        public Plane Plane => new Plane(Vertices[0], Vertices[1], Vertices[2]);

        /// <summary>
        /// Creates a polygon from a list of points
        /// </summary>
        /// <param name="vertices">The vertices of the polygon</param>
        public Polygon(IEnumerable<Vector3> vertices)
        {
            List<Vector3> verts = vertices.ToList();

            // Remove colinear vertices
            for (int i = 0; i < verts.Count - 2; i++)
            {
                Vector3 v1 = verts[i];
                Vector3 v2 = verts[i + 2];
                Vector3 p = verts[i + 1];
                Line line = new Line(v1, v2);
                // If the midpoint is on the line, remove it
                if (line.ClosestPoint(p).EquivalentTo(p))
                {
                    verts.RemoveAt(i + 1);
                }
            }

            Vertices = verts;
        }

        /// <summary>
        /// Creates a polygon from a plane and a radius.
        /// Expands the plane to the radius size to create a large polygon with 4 vertices.
        /// This constructor uses high-precision operations.
        /// </summary>
        /// <param name="plane">The polygon plane</param>
        /// <param name="radius">The polygon radius</param>
        public Polygon(Plane plane, float radius = 100000f)
        {
            Vertices = new Precision.Polygon(plane.ToPrecisionPlane(), radius).Vertices.Select(x => x.ToStandardVector3()).ToList();
        }

        protected Polygon(SerializationInfo info, StreamingContext context)
        {
            Vertices = ((Vector3[]) info.GetValue("Vertices", typeof (Vector3[]))).ToList();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Vertices", Vertices.ToArray());
        }

        /// <summary>
        /// Get the lines representing the edges of this polygon.
        /// </summary>
        /// <returns>A list of lines</returns>
        public IEnumerable<Line> GetLines()
        {
            for (int i = 1; i < Vertices.Count; i++)
            {
                yield return new Line(Vertices[i - 1], Vertices[i]);
            }
        }

        /// <summary>
        /// Checks that all the points in this polygon are valid.
        /// </summary>
        /// <returns>True if the polygon is valid</returns>
        public bool IsValid()
        {
            if (Vertices.Count < 3) return false;
            Plane plane = Plane;
            return Vertices.All(x => plane.OnPlane(x) == 0);
        }

        public bool IsConvex(float epsilon = 0.001f)
        {
            if (Vertices.Count < 3) return false;

            Plane plane = Plane;
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vector3 v1 = Vertices[i];
                Vector3 v2 = Vertices[(i + 1) % Vertices.Count];
                Vector3 v3 = Vertices[(i + 2) % Vertices.Count];
                Vector3 l1 = (v1 - v2).Normalise();
                Vector3 l2 = (v3 - v2).Normalise();
                Vector3 cross = l1.Cross(l2);
                if (plane.OnPlane(v2 + cross, epsilon) < 0.0001m) return false;
            }
            return true;
        }

        /// <summary>
        /// Determines if this polygon is behind, in front, or spanning a plane.
        /// </summary>
        /// <param name="p">The plane to test against</param>
        /// <returns>A PlaneClassification value.</returns>
        public PlaneClassification ClassifyAgainstPlane(Plane p)
        {
            int count = Vertices.Count;
            int front = 0;
            int back = 0;
            int onplane = 0;

            foreach (Vector3 t in Vertices)
            {
                int test = p.OnPlane(t);

                // Vertices on the plane are both in front and behind the plane in this context
                if (test <= 0) back++;
                if (test >= 0) front++;
                if (test == 0) onplane++;
            }

            if (onplane == count) return PlaneClassification.OnPlane;
            if (front == count) return PlaneClassification.Front;
            if (back == count) return PlaneClassification.Back;
            return PlaneClassification.Spanning;
        }

        public Polygon Flip()
        {
            return new Polygon(Vertices.Reverse());
        }

        public Vector3? GetIntersectionPoint(Line line, bool ignoreDirection = false)
        {
            if (Vertices.Count < 3) return null;

            Plane plane = Plane;
            Vector3? isect = plane.GetIntersectionPoint(line, ignoreDirection);
            if (isect == null) return null;

            Vector3 intersect = isect.Value;

            IReadOnlyList<Vector3> vectors = Vertices;

            // http://paulbourke.net/geometry/insidepoly/

            // The angle sum will be 2 * PI if the point is inside the face
            double sum = 0;
            for (int i = 0; i < vectors.Count; i++)
            {
                int i1 = i;
                int i2 = (i + 1) % vectors.Count;

                // Translate the vertices so that the intersect point is on the origin
                Vector3 v1 = vectors[i1] - intersect;
                Vector3 v2 = vectors[i2] - intersect;

                float m1 = v1.Length();
                float m2 = v2.Length();
                float nom = m1 * m2;
                if (nom < 0.001f)
                {
                    // intersection is at a vertex
                    return intersect;
                }
                sum += Math.Acos(v1.Dot(v2) / nom);
            }

            double delta = Math.Abs(sum - Math.PI * 2);
            return (delta < 0.001d) ? intersect : (Vector3?) null;
        }

        public Precision.Polygon ToPrecisionPolygon()
        {
            return new Precision.Polygon(Vertices.Select(x => x.ToPrecisionVector3()));
        }
    }
}
