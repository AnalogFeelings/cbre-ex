using System.ComponentModel.Composition;
using System.Numerics;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Rendering.ChangeHandlers;
using CBRE.BspEditor.Rendering.ResourceManagement;
using CBRE.DataStructures.Geometric;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Engine;
using CBRE.Rendering.Pipelines;
using CBRE.Rendering.Primitives;
using CBRE.Rendering.Resources;

namespace CBRE.BspEditor.Rendering.Converters
{
    [Export(typeof(IMapObjectSceneConverter))]
    public class EntitySpriteConverter : IMapObjectSceneConverter
    {
        [Import] private EngineInterface _engine;

        public MapObjectSceneConverterPriority Priority => MapObjectSceneConverterPriority.DefaultLow;

        public bool ShouldStopProcessing(MapDocument document, IMapObject obj)
        {
            return false;
        }

        public bool Supports(IMapObject obj)
        {
            return obj is Entity e && GetSpriteData(e) != null;
        }

        private EntitySprite GetSpriteData(Entity e)
        {
            EntitySprite es = e.Data.GetOne<EntitySprite>();
            return es != null && es.ContentsReplaced ? es : null;
        }

        public async Task Convert(BufferBuilder builder, MapDocument document, IMapObject obj, ResourceCollector resourceCollector)
        {
            Entity entity = (Entity) obj;
            Environment.TextureCollection tc = await document.Environment.GetTextureCollection();

            EntitySprite sd = GetSpriteData(entity);
            if (sd == null || !sd.ContentsReplaced) return;

            string name = sd.Name;
            float scale = sd.Scale;

            float width = entity.BoundingBox.Width;
            float height = entity.BoundingBox.Height;

            CBRE.Providers.Texture.TextureItem t = await tc.GetTextureItem(name);

            string texture = $"{document.Environment.ID}::{name}";
            if (t != null) resourceCollector.RequireTexture(t.Name);

            Vector4 tint = sd.Color.ToVector4();

            VertexFlags flags = VertexFlags.None;
            if (entity.IsSelected) flags |= VertexFlags.SelectiveTransformed;

            builder.Append(
                new [] { new VertexStandard { Position = entity.Origin, Normal = new Vector3(width, height, 0), Colour = Vector4.One, Tint = tint, Flags = flags } },
                new [] { 0u },
                new [] { new BufferGroup(PipelineType.BillboardAlpha, CameraType.Perspective, entity.BoundingBox.Center, texture, 0, 1) }
            );

        }
    }
}