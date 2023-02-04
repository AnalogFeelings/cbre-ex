using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace CBRE.BspEditor.Rendering.Viewport
{
    [Export(typeof(IViewportEventListenerFactory))]
    public class RightClickViewportEventListenerFactory : IViewportEventListenerFactory
    {
        public IEnumerable<IViewportEventListener> Create(MapViewport viewport)
        {
            yield return new RightClickViewportListener(viewport);
        }
    }
}