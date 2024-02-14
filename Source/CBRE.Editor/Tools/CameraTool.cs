using CBRE.Common.Mediator;
using CBRE.DataStructures.Geometric;
using CBRE.Editor.Properties;
using CBRE.Editor.UI;
using CBRE.Graphics;
using CBRE.Localization;
using CBRE.Settings;
using CBRE.UI;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Camera = CBRE.DataStructures.MapObjects.Camera;

namespace CBRE.Editor.Tools
{
    public class CameraTool : BaseTool
    {
        private enum State
        {
            None,
            MovingPosition,
            MovingLook
        }

        private State _state;
        private Camera _stateCamera;

        public override void ToolSelected(bool preventHistory)
        {
            _state = State.None;
            Mediator.Subscribe(HotkeysMediator.CameraNext, this);
            Mediator.Subscribe(HotkeysMediator.CameraPrevious, this);
        }

        private void CameraNext()
        {
            if (_state != State.None || Document.Map.Cameras.Count < 2) return;
            int idx = Document.Map.Cameras.IndexOf(Document.Map.ActiveCamera);
            idx = (idx + 1) % Document.Map.Cameras.Count;
            Document.Map.ActiveCamera = Document.Map.Cameras[idx];
            SetViewportCamera(Document.Map.ActiveCamera.EyePosition, Document.Map.ActiveCamera.LookPosition);
        }

        private void CameraPrevious()
        {
            if (_state != State.None || Document.Map.Cameras.Count < 2) return;
            int idx = Document.Map.Cameras.IndexOf(Document.Map.ActiveCamera);
            idx = (idx + Document.Map.Cameras.Count - 1) % Document.Map.Cameras.Count;
            Document.Map.ActiveCamera = Document.Map.Cameras[idx];
            SetViewportCamera(Document.Map.ActiveCamera.EyePosition, Document.Map.ActiveCamera.LookPosition);
        }

        private void CameraDelete()
        {
            if (_state != State.None || Document.Map.Cameras.Count < 2) return;
            Camera del = Document.Map.ActiveCamera;
            CameraPrevious();
            if (del != Document.Map.ActiveCamera) Document.Map.Cameras.Remove(del);
        }

        public override Image GetIcon()
        {
            return Resources.Tool_Camera;
        }

        public override string GetName()
        {
            return Local.LocalString("tool.camera");
        }

        public override HotkeyTool? GetHotkeyToolType()
        {
            return HotkeyTool.Camera;
        }

        public override string GetContextualHelp()
        {
            return Local.LocalString("tool.camera.help");
        }

        private Tuple<Coordinate, Coordinate> GetViewportCamera()
        {
            Graphics.Camera cam = ViewportManager.Viewports.OfType<Viewport3D>().Select(x => x.Camera).FirstOrDefault();
            if (cam == null) return null;

            Coordinate pos = new Coordinate((decimal)cam.Location.X, (decimal)cam.Location.Y, (decimal)cam.Location.Z);
            Coordinate look = new Coordinate((decimal)cam.LookAt.X, (decimal)cam.LookAt.Y, (decimal)cam.LookAt.Z);

            Coordinate dir = (look - pos).Normalise() * 20;
            return Tuple.Create(pos, pos + dir);
        }

        private void SetViewportCamera(Coordinate position, Coordinate look)
        {
            Graphics.Camera cam = ViewportManager.Viewports.OfType<Viewport3D>().Select(x => x.Camera).FirstOrDefault();
            if (cam == null) return;

            look = (look - position).Normalise() + position;
            cam.Location = new Vector3((float)position.X, (float)position.Y, (float)position.Z);
            cam.LookAt = new Vector3((float)look.X, (float)look.Y, (float)look.Z);
        }

        private State GetStateAtPoint(int x, int y, Viewport2D viewport, out Camera activeCamera)
        {
            decimal d = 5 / viewport.Zoom;

            foreach (Camera cam in GetCameras())
            {
                Coordinate p = viewport.ScreenToWorld(x, y);
                Coordinate pos = viewport.Flatten(cam.EyePosition);
                Coordinate look = viewport.Flatten(cam.LookPosition);
                activeCamera = cam;
                if (p.X >= pos.X - d && p.X <= pos.X + d && p.Y >= pos.Y - d && p.Y <= pos.Y + d) return State.MovingPosition;
                if (p.X >= look.X - d && p.X <= look.X + d && p.Y >= look.Y - d && p.Y <= look.Y + d) return State.MovingLook;
            }

            activeCamera = null;
            return State.None;
        }

        private IEnumerable<Camera> GetCameras()
        {
            Tuple<Coordinate, Coordinate> c = GetViewportCamera();
            if (!Document.Map.Cameras.Any())
            {
                Document.Map.Cameras.Add(new Camera { EyePosition = c.Item1, LookPosition = c.Item2 });
            }
            if (Document.Map.ActiveCamera == null || !Document.Map.Cameras.Contains(Document.Map.ActiveCamera))
            {
                Document.Map.ActiveCamera = Document.Map.Cameras.First();
            }
            decimal len = Document.Map.ActiveCamera.Length;
            Document.Map.ActiveCamera.EyePosition = c.Item1;
            Document.Map.ActiveCamera.LookPosition = c.Item1 + (c.Item2 - c.Item1).Normalise() * len;
            foreach (Camera camera in Document.Map.Cameras)
            {
                Coordinate dir = camera.LookPosition - camera.EyePosition;
                camera.LookPosition = camera.EyePosition + dir.Normalise() * Math.Max(Document.Map.GridSpacing * 1.5m, dir.VectorMagnitude());
                yield return camera;
            }
        }

        public override void MouseEnter(ViewportBase viewport, ViewportEvent e)
        {
            //
        }

        public override void MouseLeave(ViewportBase viewport, ViewportEvent e)
        {
            //
        }

        public override void MouseDown(ViewportBase viewport, ViewportEvent e)
        {
            Viewport2D vp = viewport as Viewport2D;
            if (vp == null) return;
            _state = GetStateAtPoint(e.X, vp.Height - e.Y, vp, out _stateCamera);
            if (_state == State.None && KeyboardState.Shift)
            {
                Coordinate p = SnapIfNeeded(vp.Expand(vp.ScreenToWorld(e.X, vp.Height - e.Y)));
                _stateCamera = new Camera { EyePosition = p, LookPosition = p + Coordinate.UnitX * 1.5m * Document.Map.GridSpacing };
                Document.Map.Cameras.Add(_stateCamera);
                _state = State.MovingLook;
            }
            if (_stateCamera != null)
            {
                SetViewportCamera(_stateCamera.EyePosition, _stateCamera.LookPosition);
                Document.Map.ActiveCamera = _stateCamera;
            }

        }

        public override void MouseClick(ViewportBase viewport, ViewportEvent e)
        {
            // Not used
        }

        public override void MouseDoubleClick(ViewportBase viewport, ViewportEvent e)
        {
            // Not used
        }

        public override void MouseUp(ViewportBase viewport, ViewportEvent e)
        {
            _state = State.None;
        }

        public override void MouseWheel(ViewportBase viewport, ViewportEvent e)
        {
            //
        }

        public override void MouseMove(ViewportBase viewport, ViewportEvent e)
        {
            Viewport2D vp = viewport as Viewport2D;
            if (vp == null) return;

            Coordinate p = SnapIfNeeded(vp.Expand(vp.ScreenToWorld(e.X, vp.Height - e.Y)));
            Cursor cursor = Cursors.Default;

            switch (_state)
            {
                case State.None:
                    State st = GetStateAtPoint(e.X, vp.Height - e.Y, vp, out _stateCamera);
                    if (st != State.None) cursor = Cursors.SizeAll;
                    break;
                case State.MovingPosition:
                    if (_stateCamera == null) break;
                    Coordinate newEye = vp.GetUnusedCoordinate(_stateCamera.EyePosition) + p;
                    if (KeyboardState.Ctrl) _stateCamera.LookPosition += (newEye - _stateCamera.EyePosition);
                    _stateCamera.EyePosition = newEye;
                    SetViewportCamera(_stateCamera.EyePosition, _stateCamera.LookPosition);
                    break;
                case State.MovingLook:
                    if (_stateCamera == null) break;
                    Coordinate newLook = vp.GetUnusedCoordinate(_stateCamera.LookPosition) + p;
                    if (KeyboardState.Ctrl) _stateCamera.EyePosition += (newLook - _stateCamera.LookPosition);
                    _stateCamera.LookPosition = newLook;
                    SetViewportCamera(_stateCamera.EyePosition, _stateCamera.LookPosition);
                    break;
            }
            vp.Cursor = cursor;
        }

        public override void KeyPress(ViewportBase viewport, ViewportEvent e)
        {
            //
        }

        public override void KeyDown(ViewportBase viewport, ViewportEvent e)
        {
            //
        }

        public override void KeyUp(ViewportBase viewport, ViewportEvent e)
        {
            //
        }

        public override void UpdateFrame(ViewportBase viewport, FrameInfo frame)
        {
            //
        }

        public override void Render(ViewportBase viewport)
        {
            Viewport2D vp = viewport as Viewport2D;
            if (vp == null) return;

            List<Camera> cams = GetCameras().ToList();
            if (!cams.Any()) return;

            double z = (double)vp.Zoom;

            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);

            // Draw lines between points and point outlines
            GL.Begin(PrimitiveType.Lines);

            foreach (Camera camera in cams)
            {
                Coordinate p1 = vp.Flatten(camera.EyePosition);
                Coordinate p2 = vp.Flatten(camera.LookPosition);

                GL.Color3(camera == Document.Map.ActiveCamera ? Color.Red : Color.Cyan);
                GL.Vertex2(p1.DX, p1.DY);
                GL.Vertex2(p2.DX, p2.DY);
                GL.Vertex2(p2.DX, p2.DY);
                GL.Vertex2(p1.DX, p1.DY);
            }

            GL.End();

            GL.Enable(EnableCap.PolygonSmooth);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);

            foreach (Camera camera in cams)
            {
                Coordinate p1 = vp.Flatten(camera.EyePosition);

                // Position circle
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(camera == Document.Map.ActiveCamera ? Color.DarkOrange : Color.LawnGreen);
                GLX.Circle(new Vector2d(p1.DX, p1.DY), 4, z, loop: true);
                GL.End();
            }
            foreach (Camera camera in cams)
            {
                Coordinate p1 = vp.Flatten(camera.EyePosition);
                Coordinate p2 = vp.Flatten(camera.LookPosition);

                decimal multiplier = 4 / vp.Zoom;
                Coordinate dir = (p2 - p1).Normalise();
                Coordinate cp = new Coordinate(-dir.Y, dir.X, 0).Normalise();

                // Direction Triangle
                GL.Begin(PrimitiveType.Triangles);
                GL.Color3(camera == Document.Map.ActiveCamera ? Color.Red : Color.Cyan);
                Coord(p2 - (dir - cp) * multiplier);
                Coord(p2 - (dir + cp) * multiplier);
                Coord(p2 + dir * 1.5m * multiplier);
                GL.End();
            }

            GL.Disable(EnableCap.PolygonSmooth);

            GL.Begin(PrimitiveType.Lines);

            foreach (Camera camera in cams)
            {
                Coordinate p1 = vp.Flatten(camera.EyePosition);
                Coordinate p2 = vp.Flatten(camera.LookPosition);

                decimal multiplier = 4 / vp.Zoom;
                Coordinate dir = (p2 - p1).Normalise();
                Coordinate cp = new Coordinate(-dir.Y, dir.X, 0).Normalise();

                GL.Color3(Color.Black);
                GLX.Circle(new Vector2d(p1.DX, p1.DY), 4, z);
                Coord(p2 + dir * 1.5m * multiplier);
                Coord(p2 - (dir + cp) * multiplier);
                Coord(p2 - (dir + cp) * multiplier);
                Coord(p2 - (dir - cp) * multiplier);
                Coord(p2 - (dir - cp) * multiplier);
                Coord(p2 + dir * 1.5m * multiplier);
            }

            GL.End();

            GL.Disable(EnableCap.LineSmooth);
        }

        protected static void Coord(Coordinate c)
        {
            GL.Vertex3(c.DX, c.DY, c.DZ);
        }

        public override HotkeyInterceptResult InterceptHotkey(HotkeysMediator hotkeyMessage, object parameters)
        {
            if (hotkeyMessage == HotkeysMediator.OperationsDelete)
            {
                CameraDelete();
                return HotkeyInterceptResult.Abort;
            }
            return HotkeyInterceptResult.Continue;
        }
    }
}
