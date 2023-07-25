using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Rendering.Viewport;
using CBRE.DataStructures.Geometric;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Overlay;
using CBRE.Rendering.Viewports;

namespace CBRE.BspEditor.Tools.Draggable
{
    public class InternalBoxResizeHandle : BoxResizeHandle
    {
        public InternalBoxResizeHandle(BoxDraggableState state, ResizeHandle handle) : base(state, handle)
        {
        }

        private Box GetRectangle(ICamera camera)
        {
            Vector3 start = camera.Flatten(BoxState.Start);
            Vector3 end = camera.Flatten(BoxState.End);
            Box box = new Box(start, end);
            float wid = Math.Min(box.Width / 10, camera.PixelsToUnits(20));
            float len = Math.Min(box.Length / 10, camera.PixelsToUnits(20));
            switch (Handle)
            {
                case ResizeHandle.TopLeft:
                    return new Box(new Vector3(start.X, end.Y - len, 0), new Vector3(start.X + wid, end.Y, 0));
                case ResizeHandle.Top:
                    return new Box(new Vector3(start.X, end.Y - len, 0), end);
                case ResizeHandle.TopRight:
                    return new Box(new Vector3(end.X - wid, end.Y - len, 0), new Vector3(end.X, end.Y, 0));
                case ResizeHandle.Left:
                    return new Box(start, new Vector3(start.X + wid, end.Y, 0));
                case ResizeHandle.Center:
                    return box;
                case ResizeHandle.Right:
                    return new Box(new Vector3(end.X - wid, start.Y, 0), end);
                case ResizeHandle.BottomLeft:
                    return new Box(new Vector3(start.X, start.Y, 0), new Vector3(start.X + wid, start.Y + len, 0));
                case ResizeHandle.Bottom:
                    return new Box(start, new Vector3(end.X, start.Y + len, 0));
                case ResizeHandle.BottomRight:
                    return new Box(new Vector3(end.X - wid, start.Y, 0), new Vector3(end.X, start.Y + len, 0));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override bool CanDrag(MapDocument document, MapViewport viewport, OrthographicCamera camera,
            ViewportEvent e, Vector3 position)
        {
            const int padding = 2;
            Box box = GetRectangle(viewport.Viewport.Camera);
            Vector3 c = position;
            return c.X >= box.Start.X - padding && c.Y >= box.Start.Y - padding && c.Z >= box.Start.Z - padding
                   && c.X <= box.End.X + padding && c.Y <= box.End.Y + padding && c.Z <= box.End.Z + padding;
        
        }

        protected override Vector3 GetResizeOrigin(MapViewport viewport, OrthographicCamera camera, Vector3 position)
        {
            Vector3 st = camera.Flatten(BoxState.Start);
            Vector3 ed = camera.Flatten(BoxState.End);
            Vector3[] points = new[] { st, ed, new Vector3(st.X, ed.Y, 0), new Vector3(ed.X, st.Y, 0) };
            return points.OrderBy(x => (position - x).LengthSquared()).First();
        }

        public override void Render(IViewport viewport, OrthographicCamera camera, Vector3 worldMin, Vector3 worldMax, I2DRenderer im)
        {
            if (HighlightedViewport != viewport) return;

            Box b = GetRectangle(camera);
            Vector3 start = camera.WorldToScreen(camera.Expand(b.Start));
            Vector3 end = camera.WorldToScreen(camera.Expand(b.End));

            im.AddRectFilled(start.ToVector2(), end.ToVector2(), State.FillColour);

            if (Handle == ResizeHandle.Center && SnappedMoveOrigin != null)
            {
                const int size = 4;
                Vector3 orig = camera.WorldToScreen(camera.Expand(SnappedMoveOrigin.Value));

                im.AddLine(new Vector2(orig.X - size, orig.Y - size), new Vector2(orig.X + size, orig.Y + size), Color.Yellow, 1, false);
                im.AddLine(new Vector2(orig.X + size, orig.Y - size), new Vector2(orig.X - size, orig.Y + size), Color.Yellow, 1, false);
            }
        }
    }
}