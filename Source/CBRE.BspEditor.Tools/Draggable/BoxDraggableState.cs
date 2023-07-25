using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Rendering.Viewport;
using CBRE.DataStructures.Geometric;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Overlay;
using CBRE.Rendering.Pipelines;
using CBRE.Rendering.Primitives;
using CBRE.Rendering.Resources;
using CBRE.Rendering.Viewports;

namespace CBRE.BspEditor.Tools.Draggable
{
    public class BoxDraggableState : BaseDraggable, IDraggableState
    {
        public BaseDraggableTool Tool { get; set; }

        public Color BoxColour { get; set; }
        public Color FillColour { get; set; }
        public Box RememberedDimensions { get; set; }
        internal BoxState State { get; set; }

        public bool RenderBoxText { get; set; } = true;
        public bool RenderBox { get; set; } = true;

        protected IDraggable[] BoxHandles { get; set; }

        public override Vector3 Origin => (State.Start + State.End) / 2;

        public BoxDraggableState(BaseDraggableTool tool)
        {
            Tool = tool;
            State = new BoxState();
            State.Changed += BoxStateChanged;

            RememberedDimensions = null;

            CreateBoxHandles();
        }

        private void BoxStateChanged(object sender, EventArgs e)
        {
            Box box = State.Action == BoxAction.Idle || State.Start == null || State.End == null ? Box.Empty : new Box(State.Start, State.End);

            string label = "";
            if (box != null && !box.IsEmpty()) label = box.Width.ToString("0") + " x " + box.Length.ToString("0") + " x " + box.Height.ToString("0");
            Oy.Publish("MapDocument:ToolStatus:UpdateText", label);
        }

        protected virtual void CreateBoxHandles()
        {
            BoxHandles = new[]
            {
                new BoxResizeHandle(this, ResizeHandle.TopLeft),
                new BoxResizeHandle(this, ResizeHandle.TopRight),
                new BoxResizeHandle(this, ResizeHandle.BottomLeft),
                new BoxResizeHandle(this, ResizeHandle.BottomRight),
                
                new BoxResizeHandle(this, ResizeHandle.Top),
                new BoxResizeHandle(this, ResizeHandle.Left),
                new BoxResizeHandle(this, ResizeHandle.Right),
                new BoxResizeHandle(this, ResizeHandle.Bottom),

                new InternalBoxResizeHandle(this, ResizeHandle.Center), 
            };
        }

        public virtual IEnumerable<IDraggable> GetDraggables()
        {
            if (State.Action == BoxAction.Idle || State.Action == BoxAction.Drawing) yield break;
            foreach (IDraggable draggable in BoxHandles.ToList())
            {
                yield return draggable;
            }
            // 
        }

        public override void Click(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e, Vector3 position)
        {
            State.Action = BoxAction.Idle;
        }

        public override bool CanDrag(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e, Vector3 position)
        {
            return true;
        }

        public override void Highlight(MapDocument document, MapViewport viewport)
        {
            //
        }

        public override void Unhighlight(MapDocument document, MapViewport viewport)
        {
            //
        }

        public override void StartDrag(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e, Vector3 position)
        {
            State.Viewport = viewport;
            State.Action = BoxAction.Drawing;
            State.OrigStart = State.Start;
            State.OrigEnd = State.End;
            Vector3 st = RememberedDimensions == null ? Vector3.Zero : camera.GetUnusedCoordinate(RememberedDimensions.Start);
            Vector3 wid = RememberedDimensions == null ? Vector3.Zero : camera.GetUnusedCoordinate(RememberedDimensions.End - RememberedDimensions.Start);
            State.Start = Tool.SnapIfNeeded(camera.Expand(position) + st);
            State.End = State.Start + wid;
            base.StartDrag(document, viewport, camera, e, position);
        }

        public override void Drag(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e, Vector3 lastPosition, Vector3 position)
        {
            State.End = Tool.SnapIfNeeded(camera.Expand(position)) + camera.GetUnusedCoordinate(State.End);
            base.Drag(document, viewport, camera, e, lastPosition, position);
        }

        public override void EndDrag(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e, Vector3 position)
        {
            State.Viewport = null;
            State.Action = BoxAction.Drawn;
            State.End = Tool.SnapIfNeeded(camera.Expand(position)) + camera.GetUnusedCoordinate(State.End);
            State.FixBounds();
            base.EndDrag(document, viewport, camera, e, position);
        }

        public override void Render(MapDocument document, BufferBuilder builder)
        {
            if (ShouldDrawBox())
            {
                // Draw a box around the point
                Box c = new Box(State.Start, State.End);

                const uint numVertices = 4 * 6;
                const uint numWireframeIndices = numVertices * 2;

                VertexStandard[] points = new VertexStandard[numVertices];
                uint[] indices = new uint[numWireframeIndices];

                Color col = GetRenderBoxColour();
                Vector4 colour = new Vector4(col.R, col.G, col.B, 255) / 255;

                uint vi = 0u;
                uint wi = 0u;
                foreach (Vector3[] face in c.GetBoxFaces())
                {
                    uint offs = vi;

                    foreach (Vector3 v in face)
                    {
                        points[vi++] = new VertexStandard
                        {
                            Position = v,
                            Colour = colour,
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
                    new BufferGroup(PipelineType.Wireframe, CameraType.Perspective, 0, numWireframeIndices)
                };

                builder.Append(points, indices, groups);
            }
        }

        #region Rendering

        protected virtual bool ShouldDrawBox()
        {
            if (!RenderBox) return false;
            return State.Action == BoxAction.Drawing || State.Action == BoxAction.Drawn || State.Action == BoxAction.Resizing;
        }

        protected virtual bool ShouldDrawBoxText()
        {
            if (!RenderBoxText) return false;
            return ShouldDrawBox();
        }

        protected virtual Color GetRenderFillColour()
        {
            return FillColour;
        }

        protected virtual Color GetRenderBoxColour()
        {
            return BoxColour;
        }

        public override void Render(IViewport viewport, OrthographicCamera camera, Vector3 worldMin, Vector3 worldMax, I2DRenderer im)
        {
            if (ShouldDrawBox())
            {
                Vector3 start = camera.WorldToScreen(Vector3.Min(State.Start, State.End));
                Vector3 end = camera.WorldToScreen(Vector3.Max(State.Start, State.End));
                DrawBox(viewport, camera, im, start, end);
            }

            if (ShouldDrawBoxText())
            {
                Vector3 start = camera.WorldToScreen(Vector3.Min(State.Start, State.End));
                Vector3 end = camera.WorldToScreen(Vector3.Max(State.Start, State.End));
                DrawBoxText(viewport, camera, im, start, end);
            }
        }

        protected virtual void DrawBox(IViewport viewport, OrthographicCamera camera, I2DRenderer im, Vector3 start, Vector3 end)
        {
            start = Vector3.Max(start, new Vector3(-100, -100, -100));
            end = Vector3.Min(end, new Vector3(viewport.Width + 100, viewport.Height + 100, 100));
            
            im.AddRectFilled(start.ToVector2(), end.ToVector2(), GetRenderFillColour());
            im.AddRect(start.ToVector2(), end.ToVector2(), GetRenderBoxColour());
        }

        protected virtual void DrawBoxText(IViewport viewport, OrthographicCamera camera, I2DRenderer im, Vector3 start, Vector3 end)
        {
            // Don't draw the text at all if the rectangle is entirely outside the viewport
            if (start.X > camera.Width || end.X < 0) return;
            if (start.Y < 0 || end.Y > camera.Height) return;

            // Find the width and height for the given projection
            Vector3 st = camera.Flatten(State.Start);
            Vector3 en = camera.Flatten(State.End);

            string widthText = (Math.Abs(Math.Round(en.X - st.X, 1))).ToString("0.##");
            string heightText = (Math.Abs(Math.Round(en.Y - st.Y, 1))).ToString("0.##");

            // Determine the size of the value strings
            Vector2 mWidth = im.CalcTextSize(FontType.Large, widthText);
            Vector2 mHeight = im.CalcTextSize(FontType.Large, heightText);
            
            const int padding = 6;

            // Ensure the text is clamped inside the viewport
            Vector3 vWidth = new Vector3((end.X + start.X - mWidth.X) / 2, end.Y - mWidth.Y - padding, 0);
            vWidth = Vector3.Clamp(vWidth, Vector3.Zero, new Vector3(camera.Width - mWidth.X - padding, camera.Height - mHeight.Y - padding, 0));

            Vector3 vHeight = new Vector3(end.X + padding, (end.Y + start.Y - mHeight.Y) / 2, 0); 
            vHeight = Vector3.Clamp(vHeight, new Vector3(0, mWidth.Y + padding, 0), new Vector3(camera.Width - mWidth.X - padding, camera.Height - mHeight.Y - padding, 0));
            
            // Draw the strings
            im.AddText(vWidth.ToVector2(), GetRenderBoxColour(), FontType.Large, widthText);
            im.AddText(vHeight.ToVector2(), GetRenderBoxColour(), FontType.Large, heightText);
            
        }

        public override void Render(IViewport viewport, PerspectiveCamera camera, I2DRenderer im)
        {
            //
        }

        #endregion
    }
}