using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Numerics;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.DataStructures.Geometric;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Overlay;
using CBRE.Rendering.Viewports;

namespace CBRE.BspEditor.Rendering.Overlay
{
    [Export(typeof(IMapDocumentOverlayRenderable))]
    public class MapObject2DOverlayManager : IMapDocumentOverlayRenderable
    {
        private readonly IEnumerable<Lazy<IMapObject2DOverlay>> _overlays;

        private readonly WeakReference<MapDocument> _document = new WeakReference<MapDocument>(null);

        [ImportingConstructor]
        public MapObject2DOverlayManager(
            [ImportMany] IEnumerable<Lazy<IMapObject2DOverlay>> overlays
        )
        {
            _overlays = overlays;
        }

        public void SetActiveDocument(MapDocument doc)
        {
            _document.SetTarget(doc);
        }

        public void Render(IViewport viewport, OrthographicCamera camera, Vector3 worldMin, Vector3 worldMax, I2DRenderer im)
        {
            if (!_overlays.Any()) return;
            if (!_document.TryGetTarget(out var doc)) return;

            // Determine which objects are visible
            var padding = Vector3.One * 100;
            var box = new Box(worldMin - padding, worldMax + padding);
            var objects = doc.Map.Root.Find(x => x.BoundingBox.IntersectsWith(box)).ToList();

            // Render the overlay for each object
            foreach (var overlay in _overlays)
            {
                overlay.Value.Render(viewport, objects, camera, worldMin, worldMax, im);
            }
        }

        public void Render(IViewport viewport, PerspectiveCamera camera, I2DRenderer im)
        {
            // 2D only
        }
    }
}