using System;
using System.ComponentModel.Composition;
using System.Numerics;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Rendering.Dynamic;
using CBRE.BspEditor.Rendering.Resources;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Hooks;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Overlay;
using CBRE.Rendering.Resources;
using CBRE.Rendering.Viewports;

namespace CBRE.BspEditor.Tools
{
    [Export(typeof(IOverlayRenderable))]
    [Export(typeof(IDynamicRenderable))]
    [Export(typeof(IStartupHook))]
    public class ActiveToolRenderable : IOverlayRenderable, IDynamicRenderable, IStartupHook
    {
        private readonly WeakReference<BaseTool> _activeTool = new WeakReference<BaseTool>(null);
        private BaseTool ActiveTool => _activeTool.TryGetTarget(out BaseTool t) ? t : null;

        public Task OnStartup()
        {
            Oy.Subscribe<ITool>("Tool:Activated", ToolActivated);
            return Task.CompletedTask;
        }

        private Task ToolActivated(ITool tool)
        {
            _activeTool.SetTarget(tool as BaseTool);
            return Task.CompletedTask;
        }

        public void Render(BufferBuilder builder, ResourceCollector resourceCollector)
        {
            ActiveTool?.Render(builder, resourceCollector);
        }

        public void Render(IViewport viewport, OrthographicCamera camera, Vector3 worldMin, Vector3 worldMax, I2DRenderer im)
        {
            ActiveTool?.Render(viewport, camera, worldMin, worldMax, im);
        }

        public void Render(IViewport viewport, PerspectiveCamera camera, I2DRenderer im)
        {
            ActiveTool?.Render(viewport, camera, im);
        }
    }
}
