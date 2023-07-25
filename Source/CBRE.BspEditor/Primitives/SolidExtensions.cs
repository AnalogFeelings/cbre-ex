using System.Linq;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Primitives
{
    public static class SolidExtensions
    {
        public static bool Split(this Solid solid, UniqueNumberGenerator generator, Plane plane, out Solid back, out Solid front)
        {
            back = front = null;
            DataStructures.Geometric.Precision.Plane pln = plane.ToPrecisionPlane();
            DataStructures.Geometric.Precision.Polyhedron poly = solid.ToPolyhedron().ToPrecisionPolyhedron();

            if (!poly.Split(pln, out DataStructures.Geometric.Precision.Polyhedron backPoly, out DataStructures.Geometric.Precision.Polyhedron frontPoly))
            {
                if (backPoly != null) back = solid;
                else if (frontPoly != null) front = solid;
                return false;
            }

            front = MakeSolid(generator, solid, frontPoly);
            back = MakeSolid(generator, solid, backPoly);
            return true;
        }

        private static Solid MakeSolid(UniqueNumberGenerator generator, Solid original, DataStructures.Geometric.Precision.Polyhedron poly)
        {
            var originalFacePlanes = original.Faces.Select(x => new
            {
                Face = x,
                Plane = x.Plane.ToPrecisionPlane()
            }).ToList();

            Solid solid = new Solid(generator.Next("MapObject")) { IsSelected = original.IsSelected };
            foreach (DataStructures.Geometric.Precision.Polygon p in poly.Polygons)
            {
                // Try and find the face with the same plane, so we can duplicate the texture values
                Face originalFace = originalFacePlanes
                    .Where(x => p.ClassifyAgainstPlane(x.Plane) == PlaneClassification.OnPlane)
                    .Select(x => x.Face)
                    .FirstOrDefault();

                Face face = new Face(generator.Next("Face"));
                face.Vertices.AddRange(p.Vertices.Select(x => x.ToStandardVector3()));
                face.Plane = p.Plane.ToStandardPlane();

                if (originalFace != null)
                {
                    // The plane exists, so we can just apply the texture axes directly
                    face.Texture = originalFace.Texture.Clone();
                }
                else
                {
                    // No matching plane exists, so it's the clipping plane.
                    // Apply the first texture we find and align it with the face.
                    Face firstFace = originalFacePlanes[0].Face;
                    face.Texture = firstFace.Texture.Clone();
                    face.Texture.AlignToNormal(face.Plane.Normal);
                }

                solid.Data.Add(face);
            }

            // Add any extra data (visgroups, colour, etc)
            foreach (IMapObjectData data in original.Data.Where(x => !(x is Face)))
            {
                solid.Data.Add((IMapObjectData)data.Clone());
            }
            return solid;
        }

        public static bool IsValid(this Solid solid)
        {
            return solid.ToPolyhedron().IsValid();
        }
    }
}