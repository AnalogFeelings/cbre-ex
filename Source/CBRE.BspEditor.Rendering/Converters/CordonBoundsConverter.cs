using System.ComponentModel.Composition;
using System.Numerics;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Rendering.Resources;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Pipelines;
using CBRE.Rendering.Primitives;
using CBRE.Rendering.Resources;
using Plane = CBRE.DataStructures.Geometric.Plane;

namespace CBRE.BspEditor.Rendering.Converters
{
    [Export(typeof(IMapObjectSceneConverter))]
    public class CordonBoundsConverter : IMapObjectSceneConverter
    {
        public MapObjectSceneConverterPriority Priority => MapObjectSceneConverterPriority.OverrideLow;

        private CordonBounds GetCordon(MapDocument doc)
        {
            return doc.Map.Data.GetOne<CordonBounds>() ?? new CordonBounds {Enabled = false};
        }

        public bool ShouldStopProcessing(MapDocument document, IMapObject obj)
        {
            return false;
        }

        public bool Supports(IMapObject obj)
        {
            return obj is Root;
        }

        public Task Convert(BufferBuilder builder, MapDocument document, IMapObject obj, ResourceCollector resourceCollector)
        {
            CordonBounds c = GetCordon(document);
            if (!c.Enabled) return Task.FromResult(0);

            // It's always a box, these numbers are known
            const uint numVertices = 4 * 6;
            const uint numWireframeIndices = numVertices * 2;

            VertexStandard[] points = new VertexStandard[numVertices];
            uint[] indices = new uint[numWireframeIndices];

            Vector4 colour = new Vector4(1, 0, 0, 1);

            uint vi = 0u;
            uint wi = 0u;
            foreach (Vector3[] face in c.Box.GetBoxFaces())
            {
                uint offs = vi;

                Vector3 normal = new Plane(face[0], face[1], face[2]).Normal;
                foreach (Vector3 v in face)
                {
                    points[vi++] = new VertexStandard
                    {
                        Position = v,
                        Colour = colour,
                        Normal = normal,
                        Texture = Vector2.Zero,
                        Tint = Vector4.One
                    };
                }

                // Lines - [0 1] ... [n-1 n] [n 0]
                for (uint i = 0; i < 4; i++)
                {
                    indices[wi++] = offs + i;
                    indices[wi++] = offs + (i == 4 - 1 ? 0 : i + 1);
                }
            }

            BufferGroup[] groups = new[]
            {
                new BufferGroup(PipelineType.Wireframe, CameraType.Both, 0, numWireframeIndices)
            };

            builder.Append(points, indices, groups);

            return Task.FromResult(0);
        }
    }
}