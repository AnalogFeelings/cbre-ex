using System.Numerics;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Rendering.Viewport;
using CBRE.BspEditor.Tools.Draggable;
using CBRE.Rendering.Cameras;

namespace CBRE.BspEditor.Tools.Selection.TransformationHandles
{
    public interface ITransformationHandle : IDraggable
    {
        string Name { get; }
        Matrix4x4? GetTransformationMatrix(MapViewport viewport, OrthographicCamera camera, BoxState state, MapDocument doc);
        TextureTransformationType GetTextureTransformationType(MapDocument doc);
    }
}
