using CBRE.BspEditor.Rendering.ResourceManagement;
using CBRE.Rendering.Resources;

namespace CBRE.BspEditor.Rendering.Dynamic
{
    public interface IDynamicRenderable
    {
        void Render(BufferBuilder builder, ResourceCollector resourceCollector);
    }
}