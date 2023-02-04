using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Rendering.Resources;
using CBRE.Rendering.Resources;

namespace CBRE.BspEditor.Rendering.Dynamic
{
    public interface IMapObjectDynamicRenderable
    {
        void Render(MapDocument document, BufferBuilder builder, ResourceCollector resourceCollector);
    }
}