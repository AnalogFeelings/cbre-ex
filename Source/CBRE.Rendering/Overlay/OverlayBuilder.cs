using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using LogicAndTrick.Oy;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Engine;
using CBRE.Rendering.Interfaces;
using CBRE.Rendering.Pipelines;
using CBRE.Rendering.Renderables;
using CBRE.Rendering.Viewports;
using Veldrid;

namespace CBRE.Rendering.Overlay
{
    public class ViewportOverlay : IRenderable, IUpdateable
    {
        private readonly IViewport _viewport;

        private int _width;
        private int _height;

        private readonly ImGuiController _controller;

        internal ViewportOverlay(IViewport viewport)
        {
            _viewport = viewport;
            _width = -1;
            _height = -1;
            _controller = new ImGuiController(Engine.Engine.Instance.Device, viewport.Swapchain.Framebuffer.OutputDescription, viewport.Width, viewport.Height);
        }

        private void Resize()
        {
            int vpw = Math.Max(1, _viewport.Width);
            int vph = Math.Max(1, _viewport.Height);

            _width = vpw;
            _height = vph;
            
            _controller.WindowResized(_width, _height);
        }

        public void Build(IEnumerable<IOverlayRenderable> builders)
        {
            Resize();

            Vector3 min = Vector3.Zero;
            Vector3 max = Vector3.Zero;

            OrthographicCamera oc = _viewport.Camera as OrthographicCamera;
            PerspectiveCamera pc = _viewport.Camera as PerspectiveCamera;

            if (oc != null)
            {
                Vector3 up = (Vector3.One - oc.Expand(new Vector3(1, 1, 0))) * 1000;
                Vector3 tl = oc.ScreenToWorld(Vector3.Zero) + up;
                Vector3 br = oc.ScreenToWorld(new Vector3(_width, _height, 0)) - up;
                min = Vector3.Min(tl, br);
                max = Vector3.Max(tl, br);
            }

            using (ImGui2DRenderer renderer = new ImGui2DRenderer(_viewport, _controller))
            {
                foreach (IOverlayRenderable b in builders)
                {
                    try
                    {
                        if (pc != null) b.Render(_viewport, pc, renderer);
                        if (oc != null) b.Render(_viewport, oc, min, max, renderer);
                    }
                    catch (Exception ex)
                    {
                        Oy.Publish("Shell:UnhandledExceptionOnce", ex);
                    }
                }
            }
        }

        private long _lastFrame = -1;
        public void Update(long frame)
        {
            if (_lastFrame < 0)
            {
                _lastFrame = frame;
                return;
            }

            float diff = (frame - _lastFrame) / 1000f;
            ImGui.SetCurrentContext(_controller.Context);
            _controller.Update(diff);
            _lastFrame = frame;
        }

        public bool ShouldRender(IPipeline pipeline, IViewport viewport)
        {
            return pipeline.Type == PipelineType.Overlay && viewport == _viewport && _lastFrame >= 0;
        }

        public void Render(RenderContext context, IPipeline pipeline, IViewport viewport, CommandList cl)
        {
            if (_lastFrame < 0) return;
            
            ImGui.SetCurrentContext(_controller.Context);
            _controller.Render(context.Device, cl);
        }

        public IEnumerable<ILocation> GetLocationObjects(IPipeline pipeline, IViewport viewport)
        {
            yield break;
        }

        public void Render(RenderContext context, IPipeline pipeline, IViewport viewport, CommandList cl, ILocation locationObject)
        {
            //
        }

        public void Dispose()
        {

        }
    }
}
