using System.Collections.Generic;
using System.Linq;

namespace CBRE.DataStructures.Geometric.Precision
{
    /// <summary>
    /// Represents a convex polyhedron with at least 4 sides. Uses high-precision value types.
    /// </summary>
    public class Polyhedron
    {
        public IReadOnlyList<Polygon> Polygons { get; }

        public Vector3 Origin => Polygons.Aggregate(Vector3.Zero, (x, y) => x + y.Origin) / Polygons.Count;

        /// <summary>
        /// Creates a polyhedron from a list of polygons which are assumed to be valid.
        /// </summary>
        public Polyhedron(IEnumerable<Polygon> polygons)
        {
            Polygons = polygons.ToList();
        }

        /// <summary>
        /// Creates a polyhedron by intersecting a set of at least 4 planes.
        /// </summary>
        public Polyhedron(IEnumerable<Plane> planes)
        {
            List<Polygon> polygons = new List<Polygon>();

            List<Plane> list = planes.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                // Split the polygon by all the other planes
                Polygon poly = new Polygon(list[i]);
                for (int j = 0; j < list.Count; j++)
                {
                    if (i != j && poly.Split(list[j], out Polygon back, out _))
                    {
                        poly = back;
                    }
                }
                polygons.Add(poly);
            }

            // Ensure all the faces point outwards
            Vector3 origin = polygons.Aggregate(Vector3.Zero, (x, y) => x + y.Origin) / polygons.Count;
            for (int i = 0; i < polygons.Count; i++)
            {
                Polygon face = polygons[i];
                if (face.Plane.OnPlane(origin) >= 0) polygons[i] = new Polygon(face.Vertices.Reverse());
            }

            Polygons = polygons;
        }

        /// <summary>
        /// Splits this polyhedron into two polyhedron by intersecting against a plane.
        /// </summary>
        /// <param name="plane">The splitting plane</param>
        /// <param name="back">The back side of the polyhedron</param>
        /// <param name="front">The front side of the polyhedron</param>
        /// <returns>True if the plane splits the polyhedron, false if the plane doesn't intersect</returns>
        public bool Split(Plane plane, out Polyhedron back, out Polyhedron front)
        {
            back = front = null;

            // Check that this solid actually spans the plane
            List<PlaneClassification> classify = Polygons.Select(x => x.ClassifyAgainstPlane(plane)).Distinct().ToList();
            if (classify.All(x => x != PlaneClassification.Spanning))
            {
                if (classify.Any(x => x == PlaneClassification.Back)) back = this;
                else if (classify.Any(x => x == PlaneClassification.Front)) front = this;
                return false;
            }

            List<Plane> backPlanes = new List<Plane> { plane };
            List<Plane> frontPlanes = new List<Plane> { new Plane(-plane.Normal, -plane.DistanceFromOrigin) };

            foreach (Polygon face in Polygons)
            {
                PlaneClassification classification = face.ClassifyAgainstPlane(plane);
                if (classification != PlaneClassification.Back) frontPlanes.Add(face.Plane);
                if (classification != PlaneClassification.Front) backPlanes.Add(face.Plane);
            }

            back = new Polyhedron(backPlanes);
            front = new Polyhedron(frontPlanes);
            
            return true;
        }

        public Geometric.Polyhedron ToStandardPolyhedron()
        {
            return new Geometric.Polyhedron(Polygons.Select(x => x.ToStandardPolygon()));
        }
    }
}
