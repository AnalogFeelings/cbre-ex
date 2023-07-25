using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Numerics;
using CBRE.BspEditor.Tools.Vertex.Selection;

namespace CBRE.BspEditor.Tools.Vertex.Errors
{
    [Export(typeof(IVertexErrorCheck))]
    public class BackwardsFace : IVertexErrorCheck
    {
        private const string Key = "CBRE.BspEditor.Tools.Vertex.Errors.BackwardsFace";

        private Vector3 GetOrigin(IEnumerable<MutableFace> faces)
        {
            List<MutableVertex> points = faces.SelectMany(x => x.Vertices).ToList();
            Vector3 origin = points.Aggregate(Vector3.Zero, (x, y) => x + y.Position) / points.Count;
            return origin;
        }

        private IEnumerable<MutableFace> GetBackwardsFaces(MutableSolid solid, float epsilon = 0.001f)
        {
            List<MutableFace> faces = solid.Faces.ToList();
            Vector3 origin = GetOrigin(faces);
            return faces.Where(x => x.Plane.OnPlane(origin, epsilon) > 0);
        }

        public IEnumerable<VertexError> GetErrors(VertexSolid solid)
        {
            foreach (MutableFace face in GetBackwardsFaces(solid.Copy, 0.5f))
            {
                yield return new VertexError(Key, solid).Add(face);
            }
        }
    }
}