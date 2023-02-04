using System.Collections.Generic;
using System.Linq;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Threading;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Tools.Vertex.Selection
{
    public class MutableSolid
    {
        public IList<MutableFace> Faces { get; }
        public Box BoundingBox => new Box(Faces.SelectMany(x => x.Vertices.Select(v => v.Position)));

        public MutableSolid(Solid solid)
        {
            Faces = new ThreadSafeList<MutableFace>(solid.Faces.Select(x => new MutableFace(x)));
        }
    }
}