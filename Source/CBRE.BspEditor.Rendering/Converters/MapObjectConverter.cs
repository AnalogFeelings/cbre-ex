using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Rendering.Resources;
using CBRE.BspEditor.Rendering.Scene;

namespace CBRE.BspEditor.Rendering.Converters
{
    [Export]
    public class MapObjectConverter
    {
        private readonly IEnumerable<Lazy<IMapObjectSceneConverter>> _converters;
        private readonly IEnumerable<Lazy<IMapObjectGroupSceneConverter>> _groupConverters;

        [ImportingConstructor]
        public MapObjectConverter(
            [ImportMany] IEnumerable<Lazy<IMapObjectSceneConverter>> converters,
            [ImportMany] IEnumerable<Lazy<IMapObjectGroupSceneConverter>> groupConverters
        )
        {
            _converters = converters;
            _groupConverters = groupConverters;
        }

        public async Task Convert(MapDocument document, SceneBuilder builder, IEnumerable<IMapObject> affected, ResourceCollector resourceCollector)
        {
            List<IMapObject> objs = document.Map.Root.FindAll();
            if (affected != null)
            {
                HashSet<long> groups = affected.Select(x => x.ID / 200).ToHashSet();
                foreach (long g in groups)
                {
                    resourceCollector.RemoveRenderables(builder.GetRenderablesForGroup(g));
                    builder.DeleteGroup(g);
                }
                objs = objs.Where(x => groups.Contains(x.ID / 200)).ToList();
            }

            List<IMapObjectSceneConverter> converters = _converters.Select(x => x.Value).OrderBy(x => (int) x.Priority).ToList();
            List<IMapObjectGroupSceneConverter> groupConverters = _groupConverters.Select(x => x.Value).OrderBy(x => (int) x.Priority).ToList();

            foreach (IGrouping<long, IMapObject> g in objs.GroupBy(x => x.ID / 200))
            {
                builder.EnsureGroupExists(g.Key);
                CBRE.Rendering.Resources.BufferBuilder buffer = builder.GetBufferForGroup(g.Key);
                ResourceCollector collector = new ResourceCollector();

                foreach (IMapObjectGroupSceneConverter gc in groupConverters)
                {
                    gc.Convert(buffer, document, g, collector);
                }

                foreach (IMapObject obj in g)
                {
                    foreach (IMapObjectSceneConverter converter in converters)
                    {
                        if (!converter.Supports(obj)) continue;
                        await converter.Convert(buffer, document, obj, collector);
                        if (converter.ShouldStopProcessing(document, obj)) break;
                    }
                }

                builder.RemoveRenderablesFromGroup(g.Key, collector.GetRenderablesToRemove());
                builder.AddRenderablesToGroup(g.Key, collector.GetRenderablesToAdd());

                resourceCollector.Merge(collector);
            }

            builder.Complete();
        }
    }
}