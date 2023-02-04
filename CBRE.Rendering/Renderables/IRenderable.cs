using System;
using System.Collections.Generic;
using System.Numerics;
using CBRE.Rendering.Engine;
using CBRE.Rendering.Pipelines;
using CBRE.Rendering.Viewports;
using Veldrid;

namespace CBRE.Rendering.Renderables
{
    public interface IRenderable : IDisposable
    {
        IEnumerable<ILocation> GetLocationObjects(IPipeline pipeline, IViewport viewport);
        bool ShouldRender(IPipeline pipeline, IViewport viewport);
        void Render(RenderContext context, IPipeline pipeline, IViewport viewport, CommandList cl);
        void Render(RenderContext context, IPipeline pipeline, IViewport viewport, CommandList cl, ILocation locationObject);
    }

    public interface ILocation
    {
        Vector3 Location { get; }
    }
}
