using System;
using Veldrid;

namespace CBRE.Rendering.Viewports
{
    public interface IRenderTarget : IDisposable
    {
        Swapchain Swapchain { get; }
        bool ShouldRender(long frame);
    }
}