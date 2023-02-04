using System.Collections.Generic;
using CBRE.BspEditor.Tools.Vertex.Selection;

namespace CBRE.BspEditor.Tools.Vertex.Errors
{
    public interface IVertexErrorCheck
    {
        IEnumerable<VertexError> GetErrors(VertexSolid solid);
    }
}
