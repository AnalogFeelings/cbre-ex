using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CBRE.BspEditor.Tools.Vertex.Selection;
using CBRE.DataStructures.Geometric;

namespace CBRE.BspEditor.Tools.Vertex.Errors
{
    [Export(typeof(IVertexErrorCheck))]
    public class OverlappingVertices : IVertexErrorCheck
    {
        private const string Key = "CBRE.BspEditor.Tools.Vertex.Errors.OverlappingVertices";

        public IEnumerable<VertexError> GetErrors(VertexSolid solid)
        {
            foreach (MutableFace face in solid.Copy.Faces)
            {
                List<IGrouping<System.Numerics.Vector3, MutableVertex>> overlapping = face.Vertices.GroupBy(x => x.Position.Round(2)).Where(x => x.Count() > 1).ToList();
                foreach (IGrouping<System.Numerics.Vector3, MutableVertex> ol in overlapping)
                {
                    yield return new VertexError(Key, solid).Add(face).Add(ol);
                }
            }
        }
    }
}