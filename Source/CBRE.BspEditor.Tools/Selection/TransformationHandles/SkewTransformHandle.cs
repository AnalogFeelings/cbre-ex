using System.Numerics;
using System.Windows.Forms;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Grid;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Rendering.Viewport;
using CBRE.BspEditor.Tools.Draggable;
using CBRE.DataStructures.Geometric;
using CBRE.Rendering.Cameras;
using KeyboardState = CBRE.Shell.Input.KeyboardState;

namespace CBRE.BspEditor.Tools.Selection.TransformationHandles
{
    public class SkewTransformHandle : BoxResizeHandle, ITransformationHandle
    {
        private Vector3? _skewStart;
        private Vector3? _skewEnd;

        public string Name => "Skew";

        public SkewTransformHandle(BoxDraggableState state, ResizeHandle handle)
            : base(state, handle)
        {
        }

        protected override void SetCursorForHandle(MapViewport viewport, ResizeHandle handle)
        {
            Cursor ct = handle.GetCursorType();
            switch (handle)
            {
                case ResizeHandle.Top:
                case ResizeHandle.Bottom:
                    ct = Cursors.SizeWE;
                    break;
                case ResizeHandle.Left:
                case ResizeHandle.Right:
                    ct = Cursors.SizeNS;
                    break;
            }
            viewport.Control.Cursor = ct;
        }

        public override void StartDrag(MapDocument document, MapViewport viewport, OrthographicCamera camera,
            ViewportEvent e,
            Vector3 position)
        {
            _skewStart = _skewEnd = position;
            base.StartDrag(document, viewport, camera, e, position);
        }

        public override void Drag(MapDocument document, MapViewport viewport, OrthographicCamera camera,
            ViewportEvent e,
            Vector3 lastPosition, Vector3 position)
        {
            _skewEnd = position;
        }

        public override void EndDrag(MapDocument document, MapViewport viewport, OrthographicCamera camera,
            ViewportEvent e, Vector3 position)
        {
            _skewStart = _skewEnd = null;
            base.EndDrag(document, viewport, camera, e, position);
        }

        public Matrix4x4? GetTransformationMatrix(MapViewport viewport, OrthographicCamera camera, BoxState state, MapDocument doc)
        {
            bool shearUpDown = Handle == ResizeHandle.Left || Handle == ResizeHandle.Right;
            bool shearTopRight = Handle == ResizeHandle.Top || Handle == ResizeHandle.Right;

            if (!_skewStart.HasValue || !_skewEnd.HasValue) return null;

            Vector3 nsmd = _skewEnd.Value - _skewStart.Value;
            Vector3 mouseDiff = State.Tool.SnapIfNeeded(nsmd);
            if (KeyboardState.Shift && !KeyboardState.Alt)
            {
                // todo post-beta: this is hard-coded to only work on the square grid
                GridData gridData = doc.Map.Data.GetOne<GridData>();
                if (gridData?.Grid is SquareGrid sg && gridData?.SnapToGrid == true)
                {
                    mouseDiff = nsmd.Snap(sg.Step / 2);
                }
            }

            Vector3 relative = camera.Flatten(state.OrigEnd - state.OrigStart);
            Vector3 shearOrigin = (shearTopRight) ? state.OrigStart : state.OrigEnd;

            Vector3 shearAmount = new Vector3(mouseDiff.X / relative.Y, mouseDiff.Y / relative.X, 0);
            if (!shearTopRight) shearAmount *= -1;

            Matrix4x4 shearMatrix = Matrix4x4.Identity;
            float sax = shearAmount.X;
            float say = shearAmount.Y;

            switch (camera.ViewType)
            {
                case OrthographicCamera.OrthographicType.Top:
                    if (shearUpDown) shearMatrix.M12 = say;
                    else shearMatrix.M21 = sax;
                    break;
                case OrthographicCamera.OrthographicType.Front:
                    if (shearUpDown) shearMatrix.M23 = say;
                    else shearMatrix.M32 = sax;
                    break;
                case OrthographicCamera.OrthographicType.Side:
                    if (shearUpDown) shearMatrix.M13 = say;
                    else shearMatrix.M31 = sax;
                    break;
            }

            Matrix4x4 stran = Matrix4x4.CreateTranslation(-shearOrigin.X, -shearOrigin.Y, -shearOrigin.Z);
            Matrix4x4 shear = Matrix4x4.Multiply(stran, shearMatrix);
            Matrix4x4 inv = Matrix4x4.Invert(stran, out Matrix4x4 i) ? i : Matrix4x4.Identity;
            return Matrix4x4.Multiply(shear, inv);
        }

        public TextureTransformationType GetTextureTransformationType(MapDocument doc)
        {
            // Never transform textures on skew
            return TextureTransformationType.None;
        }
    }
}