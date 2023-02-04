using CBRE.BspEditor.Rendering.Resources;
using CBRE.Rendering.Resources;

namespace CBRE.BspEditor.Rendering.Dynamic
{
    public interface IDynamicRenderable
    {
        void Render(BufferBuilder builder, ResourceCollector resourceCollector);
    }
}