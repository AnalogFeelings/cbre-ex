using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Rendering.ChangeHandlers;
using CBRE.BspEditor.Rendering.Resources;
using CBRE.Rendering.Resources;

namespace CBRE.BspEditor.Rendering.Converters
{
    [Export(typeof(IMapObjectSceneConverter))]
    public class EntityModelConverter : IMapObjectSceneConverter
    {
        public MapObjectSceneConverterPriority Priority => MapObjectSceneConverterPriority.DefaultLow;

        public bool ShouldStopProcessing(MapDocument document, IMapObject obj)
        {
            return false;
        }

        public bool Supports(IMapObject obj)
        {
            return obj is Entity e && GetModelData(e) != null;
        }

        private EntityModel GetModelData(Entity e)
        {
            var em = e.Data.GetOne<EntityModel>();
            return em != null && em.ContentsReplaced ? em : null;
        }

        public Task Convert(BufferBuilder builder, MapDocument document, IMapObject obj, ResourceCollector resourceCollector)
        {
            var em = obj.Data.GetOne<EntityModel>();

            if (em.ContentsReplaced && em.Renderable != null)
            {
                resourceCollector.AddRenderables(new []{ em.Renderable });
            }

            return Task.CompletedTask;
        }
    }
}