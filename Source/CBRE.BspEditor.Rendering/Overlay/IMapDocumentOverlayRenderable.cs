using CBRE.BspEditor.Documents;
using CBRE.Rendering.Overlay;

namespace CBRE.BspEditor.Rendering.Overlay
{
    public interface IMapDocumentOverlayRenderable : IOverlayRenderable
    {
        void SetActiveDocument(MapDocument doc);
    }
}