using System;
using System.Drawing;
using System.Numerics;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Rendering.Viewport;
using CBRE.BspEditor.Tools.Draggable;
using CBRE.DataStructures.Geometric;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Overlay;
using CBRE.Rendering.Viewports;
using KeyboardState = CBRE.Shell.Input.KeyboardState;

namespace CBRE.BspEditor.Tools.Selection.TransformationHandles
{
    public class RotateTransformHandle : BoxResizeHandle, ITransformationHandle
    {
        private readonly RotationOrigin _origin;
        private Vector3? _rotateStart;
        private Vector3? _rotateEnd;

        public string Name => "Rotate";

        public RotateTransformHandle(BoxDraggableState state, ResizeHandle handle, RotationOrigin origin) : base(state, handle)
        {
            _origin = origin;
        }

        protected override void SetCursorForHandle(MapViewport viewport, ResizeHandle handle)
        {
            System.Windows.Forms.Cursor ct = ToolCursors.RotateCursor;
            viewport.Control.Cursor = ct;
        }

        public override void StartDrag(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e, Vector3 position)
        {
            _rotateStart = _rotateEnd = position;
            base.StartDrag(document, viewport, camera, e, position);
        }

        public override void Drag(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e, Vector3 lastPosition, Vector3 position)
        {
            _rotateEnd = position;
        }

        public override void EndDrag(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e, Vector3 position)
        {
            _rotateStart = _rotateEnd = null;
            base.EndDrag(document, viewport, camera, e, position);
        }

        public override void Render(IViewport viewport, OrthographicCamera camera, Vector3 worldMin, Vector3 worldMax, I2DRenderer im)
        {
            (Vector3 wpos, Vector3 soff) = GetWorldPositionAndScreenOffset(camera);
            Vector3 spos = camera.WorldToScreen(wpos) + soff;

            const float radius = 4;

            im.AddCircleFilled(spos.ToVector2(), radius, Color.White);
            im.AddCircle(spos.ToVector2(), radius, Color.Black);
        }

        public Matrix4x4? GetTransformationMatrix(MapViewport viewport, OrthographicCamera camera, BoxState state, MapDocument doc)
        {
            Vector3 origin = camera.ZeroUnusedCoordinate((state.OrigStart + state.OrigEnd) / 2);
            if (_origin != null) origin = _origin.Position;

            if (!_rotateStart.HasValue || !_rotateEnd.HasValue) return null;

            Vector3 forigin = camera.Flatten(origin);

            Vector3 origv = Vector3.Normalize(_rotateStart.Value - forigin);
            Vector3 newv =  Vector3.Normalize(_rotateEnd.Value - forigin);

            double angle = Math.Acos(Math.Max(-1, Math.Min(1, origv.Dot(newv))));
            if ((origv.Cross(newv).Z < 0)) angle = 2 * Math.PI - angle;

            // TODO post-beta: configurable rotation snapping
            float roundingDegrees = 15f;
            if (KeyboardState.Alt) roundingDegrees = 1;

            double deg = angle * (180 / Math.PI);
            double rnd = Math.Round(deg / roundingDegrees) * roundingDegrees;
            angle = rnd * (Math.PI / 180);

            Matrix4x4 rotm;
            if (camera.ViewType == OrthographicCamera.OrthographicType.Top) rotm = Matrix4x4.CreateRotationZ((float)angle);
            else if (camera.ViewType == OrthographicCamera.OrthographicType.Front) rotm = Matrix4x4.CreateRotationX((float)angle);
            else rotm = Matrix4x4.CreateRotationY((float)-angle); // The Y axis rotation goes in the reverse direction for whatever reason

            Matrix4x4 mov = Matrix4x4.CreateTranslation(-origin.X, -origin.Y, -origin.Z);
            Matrix4x4 rot = Matrix4x4.Multiply(mov, rotm);
            Matrix4x4 inv = Matrix4x4.Invert(mov, out Matrix4x4 i) ? i : Matrix4x4.Identity;
            return Matrix4x4.Multiply(rot, inv);
        }

        public TextureTransformationType GetTextureTransformationType(MapDocument doc)
        {
            TransformationFlags tl = doc.Map.Data.GetOne<TransformationFlags>() ?? new TransformationFlags();
            return !tl.TextureLock ? TextureTransformationType.None : TextureTransformationType.Uniform;
        }
    }
}