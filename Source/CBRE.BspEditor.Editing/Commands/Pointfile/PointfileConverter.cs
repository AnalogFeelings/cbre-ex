using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Numerics;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Rendering.Converters;
using CBRE.BspEditor.Rendering.Resources;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Pipelines;
using CBRE.Rendering.Primitives;
using CBRE.Rendering.Resources;

namespace CBRE.BspEditor.Editing.Commands.Pointfile
{
    [Export(typeof(IMapObjectSceneConverter))]
    public class PointfileConverter : IMapObjectSceneConverter
    {
        public MapObjectSceneConverterPriority Priority => MapObjectSceneConverterPriority.OverrideLow;
        
        private Pointfile GetPointfile(MapDocument doc)
        {
            return doc.Map.Data.GetOne<Pointfile>();
        }

        public bool ShouldStopProcessing(MapDocument document, IMapObject obj)
        {
            return false;
        }

        public bool Supports(IMapObject obj)
        {
            return obj is Root;
        }

        public Task Convert(BufferBuilder builder, MapDocument document, IMapObject obj,
            ResourceCollector resourceCollector)
        {
            Pointfile pointfile = GetPointfile(document);
            if (pointfile == null) return Task.FromResult(0);

            float r = 1f;
            float g = 0.5f;
            float b = 0.5f;
            float change = 0.5f / pointfile.Lines.Count;

            List<VertexStandard> verts = new List<VertexStandard>();
            List<uint> index = new List<uint>();
            
            for (int i = 0; i < pointfile.Lines.Count; i++)
            {
                DataStructures.Geometric.Line line = pointfile.Lines[i];

                index.Add((uint) index.Count + 0);
                index.Add((uint) index.Count + 1);

                verts.Add(new VertexStandard
                {
                    Position = line.Start,
                    Colour = new Vector4(r, g, b, 1),
                    Tint = Vector4.One
                });
                verts.Add(new VertexStandard
                {
                    Position = line.End,
                    Colour = new Vector4(r, g, b, 1),
                    Tint = Vector4.One
                });
            
                r = 1f - (change * i);
                b = 0.5f + (change * i);
            }

            builder.Append(verts, index, new []
            {
                new BufferGroup(PipelineType.Wireframe, CameraType.Both, 0, (uint) index.Count)
            });

            return Task.FromResult(0);
        }
    }
}