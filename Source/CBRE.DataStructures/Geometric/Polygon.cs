using CBRE.DataStructures.Transformations;
using CBRE.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CBRE.DataStructures.Geometric
{
    /// <summary>
    /// Represents a coplanar, directed polygon with at least 3 vertices.
    /// </summary>
    [Serializable]
    public class Polygon : ISerializable
    {
        public List<Coordinate> Vertices { get; set; }
        public Plane Plane { get; set; }

        /// <summary>
        /// Creates a polygon from a list of points
        /// </summary>
        /// <param name="vertices">The vertices of the polygon</param>
        public Polygon(IEnumerable<Coordinate> vertices)
        {
            Vertices = vertices.ToList();
            Plane = new Plane(Vertices[0], Vertices[1], Vertices[2]);
            Simplify();
        }

        /// <summary>
        /// Creates a polygon from a plane and a radius.
        /// Expands the plane to the radius size to create a large polygon with 4 vertices.
        /// </summary>
        /// <param name="plane">The polygon plane</param>
        /// <param name="radius">The polygon radius</param>
        public Polygon(Plane plane, decimal radius = 1000000m)
        {
            Plane = plane;

            // Get aligned up and right axes to the plane
            Coordinate direction = Plane.GetClosestAxisToNormal();
            Coordinate tempV = direction == Coordinate.UnitZ ? -Coordinate.UnitY : -Coordinate.UnitZ;
            Coordinate up = tempV.Cross(Plane.Normal).Normalise();
            Coordinate right = Plane.Normal.Cross(up).Normalise();

            Vertices = new List<Coordinate>
                           {
                               plane.PointOnPlane + right + up, // Top right
                               plane.PointOnPlane - right + up, // Top left
                               plane.PointOnPlane - right - up, // Bottom left
                               plane.PointOnPlane + right - up, // Bottom right
                           };
            Expand(radius);
        }

        protected Polygon(SerializationInfo info, StreamingContext context)
        {
            Vertices = ((Coordinate[])info.GetValue("Vertices", typeof(Coordinate[]))).ToList();
            Plane = (Plane)info.GetValue("Plane", typeof(Plane));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Vertices", Vertices.ToArray());
            info.AddValue("Plane", Plane);
        }

        public Polygon Clone()
        {
            return new Polygon(new List<Coordinate>(Vertices));
        }

        public void Unclone(Polygon polygon)
        {
            Vertices = new List<Coordinate>(polygon.Vertices);
            Plane = polygon.Plane.Clone();
        }

        /// <summary>
        /// Returns the origin of this polygon.
        /// </summary>
        /// <returns></returns>
        public Coordinate GetOrigin()
        {
            return Vertices.Aggregate(Coordinate.Zero, (x, y) => x + y) / Vertices.Count;
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
        /// <returns>True if the plane is valid</returns>
        public bool IsValid()
        {
            return Vertices.All(x => Plane.OnPlane(x) == 0);
        }

        /// <summary>
        /// Removes any colinear vertices in the polygon
        /// </summary>
        public void Simplify()
        {
            // Remove colinear vertices
            for (int i = 0; i < Vertices.Count - 2; i++)
            {
                Coordinate v1 = Vertices[i];
                Coordinate v2 = Vertices[i + 2];
                Coordinate p = Vertices[i + 1];
                Line line = new Line(v1, v2);
                // If the midpoint is on the line, remove it
                if (line.ClosestPoint(p).EquivalentTo(p))
                {
                    Vertices.RemoveAt(i + 1);
                }
            }
        }

        /// <summary>
        /// Transforms all the points in the polygon.
        /// </summary>
        /// <param name="transform">The transformation to perform</param>
        public void Transform(IUnitTransformation transform)
        {
            Vertices = Vertices.Select(transform.Transform).ToList();
            Plane = new Plane(Vertices[0], Vertices[1], Vertices[2]);
        }

        public bool IsConvex(decimal epsilon = 0.001m)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Coordinate v1 = Vertices[i];
                Coordinate v2 = Vertices[(i + 1) % Vertices.Count];
                Coordinate v3 = Vertices[(i + 2) % Vertices.Count];
                Coordinate l1 = (v1 - v2).Normalise();
                Coordinate l2 = (v3 - v2).Normalise();
                Coordinate cross = l1.Cross(l2);
                if (Plane.OnPlane(v2 + cross, epsilon) < 0.0001m) return false;
            }
            return true;
        }

        /// <summary>
        /// Expands this plane's points outwards from the origin by a radius value.
        /// </summary>
        /// <param name="radius">The distance the points will be from the origin after expanding</param>
        public void Expand(decimal radius)
        {
            // 1. Center the polygon at the world origin
            // 2. Normalise all the vertices
            // 3. Multiply them by the radius
            // 4. Move the polygon back to the original origin
            Coordinate origin = GetOrigin();
            Vertices = Vertices.Select(x => (x - origin).Normalise() * radius + origin).ToList();
            Plane = new Plane(Vertices[0], Vertices[1], Vertices[2]);
        }

        /// <summary>
        /// Determines if this polygon is behind, in front, or spanning a plane.
        /// </summary>
        /// <param name="p">The plane to test against</param>
        /// <returns>A PlaneClassification value.</returns>
        public PlaneClassification ClassifyAgainstPlane(Plane p)
        {
            int front = 0, back = 0, onplane = 0, count = Vertices.Count;

            foreach (int test in Vertices.Select(x => p.OnPlane(x)))
            {
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

        /// <summary>
        /// Splits this polygon by a clipping plane, discarding the front side.
        /// The original polygon is modified to be the back side of the split.
        /// </summary>
        /// <param name="clip">The clipping plane</param>
        public void Split(Plane clip)
        {
            Polygon front, back;
            if (Split(clip, out back, out front))
            {
                Unclone(back);
            }
        }

        /// <summary>
        /// Splits this polygon by a clipping plane, returning the back and front planes.
        /// The original polygon is not modified.
        /// </summary>
        /// <param name="clip">The clipping plane</param>
        /// <param name="back">The back polygon</param>
        /// <param name="front">The front polygon</param>
        /// <returns>True if the split was successful</returns>
        public bool Split(Plane clip, out Polygon back, out Polygon front)
        {
            Polygon cFront, cBack;
            return Split(clip, out back, out front, out cBack, out cFront);
        }

        /// <summary>
        /// Splits this polygon by a clipping plane, returning the back and front planes.
        /// The original polygon is not modified.
        /// </summary>
        /// <param name="clip">The clipping plane</param>
        /// <param name="back">The back polygon</param>
        /// <param name="front">The front polygon</param>
        /// <param name="coplanarBack">If the polygon rests on the plane and points backward, this will not be null</param>
        /// <param name="coplanarFront">If the polygon rests on the plane and points forward, this will not be null</param>
        /// <returns>True if the split was successful</returns>
        public bool Split(Plane clip, out Polygon back, out Polygon front, out Polygon coplanarBack, out Polygon coplanarFront)
        {
            // If the polygon doesn't span the plane, return false.
            PlaneClassification classify = ClassifyAgainstPlane(clip);
            if (classify != PlaneClassification.Spanning)
            {
                back = front = null;
                coplanarBack = coplanarFront = null;
                if (classify == PlaneClassification.Back) back = this;
                else if (classify == PlaneClassification.Front) front = this;
                else if (Plane.Normal.Dot(clip.Normal) > 0) coplanarFront = this;
                else coplanarBack = this;
                return false;
            }

            // Get the new front and back vertices
            List<Coordinate> backVerts = new List<Coordinate>();
            List<Coordinate> frontVerts = new List<Coordinate>();
            int prev = 0;

            for (int i = 0; i <= Vertices.Count; i++)
            {
                Coordinate end = Vertices[i % Vertices.Count];
                int cls = clip.OnPlane(end);

                // Check plane crossing
                if (i > 0 && cls != 0 && prev != 0 && prev != cls)
                {
                    // This line end point has crossed the plane
                    // Add the line intersect to the 
                    Coordinate start = Vertices[i - 1];
                    Line line = new Line(start, end);
                    Coordinate isect = clip.GetIntersectionPoint(line, true);
                    if (isect == null) throw new Exception(Local.LocalString("exception.null_intersection"));
                    frontVerts.Add(isect);
                    backVerts.Add(isect);
                }

                // Add original points
                if (i < Vertices.Count)
                {
                    // OnPlane points get put in both polygons, doesn't generate split
                    if (cls >= 0) frontVerts.Add(end);
                    if (cls <= 0) backVerts.Add(end);
                }

                prev = cls;
            }

            back = new Polygon(backVerts);
            front = new Polygon(frontVerts);
            coplanarBack = coplanarFront = null;

            return true;
        }

        public void Flip()
        {
            Vertices.Reverse();
            Plane = new Plane(-Plane.Normal, Plane.PointOnPlane);
        }
    }
}
