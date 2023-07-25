using System.Linq;
using System.Numerics;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.DataStructures.Geometric;
using Path = System.IO.Path;

namespace CBRE.BspEditor.Documents
{
    /// <summary>
    /// Common extensions for map documents
    /// </summary>
    public static class MapDocumentExtensions
    {
        /// <summary>
        /// Clone a document, retaining only objects contained within a cordon area, and create a box around the cordon bounds to seal the level.
        /// </summary>
        /// <param name="doc">This document</param>
        /// <param name="cordonBounds">The cordon bounds</param>
        /// <param name="cordonTextureName">The name of the texture to use for the sealing box</param>
        /// <returns>A cloned document</returns>
        public static MapDocument CloneWithCordon(this MapDocument doc, Box cordonBounds, string cordonTextureName)
        {
            // If we're exporting cordon then we need to ensure that only objects in the bounds are exported.
            // Additionally a surrounding box needs to be added to enclose the map.
            Map cloneMap = new Map();

            // Copy the map data
            cloneMap.Data.Clear();
            cloneMap.Data.AddRange(doc.Map.Data.Copy(cloneMap.NumberGenerator));

            // Copy the root data
            cloneMap.Root.Data.Clear();
            cloneMap.Root.Data.AddRange(doc.Map.Root.Data.Copy(cloneMap.NumberGenerator));

            // Add copies of all the matching child objects (and their children, etc)
            cloneMap.Root.Hierarchy.Clear();
            foreach (IMapObject obj in doc.Map.Root.Hierarchy.Where(x => x.BoundingBox.IntersectsWith(cordonBounds)))
            {
                IMapObject copy = (IMapObject)obj.Copy(cloneMap.NumberGenerator);
                copy.Hierarchy.Parent = cloneMap.Root;
            }

            // Add a hollow box around the cordon bounds
            Box outside = new Box(cloneMap.Root.Hierarchy.Select(x => x.BoundingBox).Union(new[] { cordonBounds }));
            outside = new Box(outside.Start - Vector3.One * 10, outside.End + Vector3.One * 10);
            Box inside = cordonBounds;

            Solid outsideBox = new Solid(cloneMap.NumberGenerator.Next("MapObject"));
            foreach (Vector3[] arr in outside.GetBoxFaces())
            {
                Face face = new Face(cloneMap.NumberGenerator.Next("Face"))
                {
                    Plane = new DataStructures.Geometric.Plane(arr[0], arr[1], arr[2]),
                    Texture = { Name = cordonTextureName }
                };
                face.Vertices.AddRange(arr.Select(x => x.Round(0)));
                outsideBox.Data.Add(face);
            }

            outsideBox.DescendantsChanged();

            Solid insideBox = new Solid(cloneMap.NumberGenerator.Next("MapObject"));
            foreach (Vector3[] arr in inside.GetBoxFaces())
            {
                Face face = new Face(cloneMap.NumberGenerator.Next("Face"))
                {
                    Plane = new DataStructures.Geometric.Plane(arr[0], arr[1], arr[2]),
                    Texture = { Name = cordonTextureName }
                };
                face.Vertices.AddRange(arr.Select(x => x.Round(0)));
                insideBox.Data.Add(face);
            }

            insideBox.DescendantsChanged();

            // Carve the inside box into the outside box and add the front solids to the map
            foreach (Face face in insideBox.Faces)
            {
                // Carve the box
                if (!outsideBox.Split(cloneMap.NumberGenerator, face.Plane, out Solid back, out Solid front)) continue;

                // Align texture to face
                foreach (Face f in front.Faces)
                {
                    f.Texture.XScale = f.Texture.YScale = 1;
                    f.Texture.AlignToNormal(f.Plane.Normal);
                }

                // Add to map
                front.Hierarchy.Parent = cloneMap.Root;

                // Continue carving
                outsideBox = back;
            }

            // Now we're ready to export this map
            return new MapDocument(cloneMap, doc.Environment)
            {
                FileName = Path.GetFileName(doc.FileName),
                Name = doc.Name
            };
        }
    }
}
