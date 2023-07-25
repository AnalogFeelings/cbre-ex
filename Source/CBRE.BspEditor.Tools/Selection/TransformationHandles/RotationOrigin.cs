using System.Drawing;
using System.Numerics;
using System.Windows.Forms;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Rendering.Viewport;
using CBRE.BspEditor.Tools.Draggable;
using CBRE.DataStructures.Geometric;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Overlay;
using CBRE.Rendering.Viewports;

namespace CBRE.BspEditor.Tools.Selection.TransformationHandles
{
    public class RotationOrigin : DraggableVector3
    {
        private readonly BaseTool _tool;

        public RotationOrigin(BaseTool tool)
        {
            _tool = tool;
            Width = 8;
        }

        protected override void SetMoveCursor(MapViewport viewport)
        {
            viewport.Control.Cursor = Cursors.Cross;
        }

        public override void Drag(MapDocument document, MapViewport viewport, OrthographicCamera camera,
            ViewportEvent e, Vector3 lastPosition, Vector3 position)
        {
            Position = _tool.SnapToSelection(camera.Expand(position) + camera.GetUnusedCoordinate(Position), camera);
            OnDragMoved();
        }

        public override void Render(IViewport viewport, OrthographicCamera camera, Vector3 worldMin, Vector3 worldMax, I2DRenderer im)
        {
            Vector3 spos = camera.WorldToScreen(Position);

            const float inner = 4;
            const float outer = 8;

            Color col = Highlighted ? Color.Red : Color.White;
            im.AddCircle(spos.ToVector2(), inner, Color.Cyan);
            im.AddCircle(spos.ToVector2(), outer, col);
        }
    }
}