using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Engine;
using CBRE.Rendering.Pipelines;
using CBRE.Rendering.Renderables;
using CBRE.Rendering.Resources;
using CBRE.Rendering.Viewports;
using Veldrid;

namespace CBRE.BspEditor.Rendering.Scene
{
    public class SceneBuilderRenderable : IRenderable
    {
        private static readonly uint IndSize = (uint) Unsafe.SizeOf<IndirectDrawIndexedArguments>();

        private readonly SceneBuilder _sceneBuilder;

        public SceneBuilderRenderable(SceneBuilder sceneBuilder)
        {
            _sceneBuilder = sceneBuilder;
        }

        public bool ShouldRender(IPipeline pipeline, IViewport viewport)
        {
            return true;
        }

        public void Render(RenderContext context, IPipeline pipeline, IViewport viewport, CommandList cl)
        {
            List<BufferBuilder> builders = _sceneBuilder.BufferBuilders.ToList();
            foreach (BufferBuilder buffer in builders)
            {
                for (int i = 0; i < buffer.NumBuffers; i++)
                {
                    List<BufferGroup> groups = buffer.IndirectBufferGroups[i].Where(x => x.Pipeline == pipeline.Type && !x.HasTransparency).Where(x => x.Camera == CameraType.Both || x.Camera == viewport.Camera.Type).ToList();
                    if (!groups.Any()) continue;

                    cl.SetVertexBuffer(0, buffer.VertexBuffers[i]);
                    cl.SetIndexBuffer(buffer.IndexBuffers[i], IndexFormat.UInt32);
                    foreach (BufferGroup bg in groups)
                    {
                        pipeline.Bind(context, cl, bg.Binding);
                        buffer.IndirectBuffers[i].DrawIndexed(cl, bg.Offset * IndSize, bg.Count, 20);
                    }
                }
            }
        }

        public IEnumerable<ILocation> GetLocationObjects(IPipeline pipeline, IViewport viewport)
        {
            foreach (BufferBuilder buffer in _sceneBuilder.BufferBuilders)
            {
                for (int i = 0; i < buffer.NumBuffers; i++)
                {
                    foreach (BufferGroup group in buffer.IndirectBufferGroups[i])
                    {
                        if (group.Pipeline != pipeline.Type || !group.HasTransparency) continue;
                        if (group.Camera != CameraType.Both && group.Camera != viewport.Camera.Type) continue;
                        yield return new GroupLocation(buffer, i, group);
                    }
                }
            }
        }

        public void Render(RenderContext context, IPipeline pipeline, IViewport viewport, CommandList cl, ILocation locationObject)
        {
            GroupLocation groupLocation = (GroupLocation) locationObject;

            BufferBuilder buffer = groupLocation.Builder;
            int i = groupLocation.Index;
            BufferGroup bg = groupLocation.Group;

            cl.SetVertexBuffer(0, buffer.VertexBuffers[i]);
            cl.SetIndexBuffer(buffer.IndexBuffers[i], IndexFormat.UInt32);
            pipeline.Bind(context, cl, bg.Binding);
            buffer.IndirectBuffers[i].DrawIndexed(cl, bg.Offset * IndSize, bg.Count, 20);
        }

        public void Dispose()
        {
            //
        }

        private class GroupLocation : ILocation
        {
            public Vector3 Location => Group.Location;
            public BufferBuilder Builder { get; }
            public int Index { get; }
            public BufferGroup Group { get; }

            public GroupLocation(BufferBuilder builder, int index, BufferGroup group)
            {
                Builder = builder;
                Index = index;
                Group = group;
            }
        }
    }
}