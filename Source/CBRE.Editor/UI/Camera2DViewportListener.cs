﻿using CBRE.Common.Mediator;
using CBRE.DataStructures.Geometric;
using CBRE.Editor.Documents;
using CBRE.Extensions;
using CBRE.UI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.Editor.UI
{
    public class Camera2DViewportListener : IViewportEventListener, IMediatorListener
    {
        public ViewportBase Viewport
        {
            get { return Viewport2D; }
            set { Viewport2D = (Viewport2D)value; }
        }

        public Viewport2D Viewport2D { get; set; }

        public Camera2DViewportListener(Viewport2D viewport)
        {
            Viewport = viewport;
            Viewport2D = viewport;
        }

        public void KeyUp(ViewportEvent e)
        {
            if (e.KeyCode == Keys.Space)
            {
                Viewport.Cursor = Cursors.Default;
                Viewport.Capture = false;
                e.Handled = true;
            }
        }

        public void KeyDown(ViewportEvent e)
        {
            if (e.KeyCode == Keys.Space)
            {
                Viewport.Cursor = Cursors.SizeAll;
                if (!CBRE.Settings.View.Camera2DPanRequiresMouseClick)
                {
                    Viewport.Capture = true;
                    System.Drawing.Point p = e.Sender.PointToClient(Cursor.Position);
                    _mouseDown = new Coordinate(p.X, Viewport2D.Height - p.Y, 0);
                }
                e.Handled = true;
            }

            bool moveAllowed = DocumentManager.CurrentDocument != null &&
                              (DocumentManager.CurrentDocument.Selection.IsEmpty()
                               || !CBRE.Settings.Select.ArrowKeysNudgeSelection);
            if (moveAllowed)
            {
                Coordinate shift = new Coordinate(0, 0, 0);

                switch (e.KeyCode)
                {
                    case Keys.Left:
                        shift.X = -Viewport.Width / Viewport2D.Zoom / 4;
                        break;
                    case Keys.Right:
                        shift.X = Viewport.Width / Viewport2D.Zoom / 4;
                        break;
                    case Keys.Up:
                        shift.Y = Viewport.Height / Viewport2D.Zoom / 4;
                        break;
                    case Keys.Down:
                        shift.Y = -Viewport.Height / Viewport2D.Zoom / 4;
                        break;
                }

                Viewport2D.Position += shift;
            }

            string str = e.KeyCode.ToString();
            if (str.StartsWith("NumPad") || str.StartsWith("D"))
            {
                char last = str.Last();
                if (Char.IsDigit(last))
                {
                    int press = (int)Char.GetNumericValue(last);
                    if (press >= 0 && press <= 9)
                    {
                        if (press == 0) press = 10;
                        int num = Math.Max(press - 6, 6 - press);
                        decimal pow = (decimal)Math.Pow(2, num);
                        decimal zoom = press < 6 ? 1 / pow : pow;
                        Viewport2D.Zoom = zoom;
                        Mediator.Publish(EditorMediator.ViewZoomChanged, Viewport2D.Zoom);
                    }
                }
            }
        }

        public void KeyPress(ViewportEvent e)
        {

        }

        public void MouseMove(ViewportEvent e)
        {
            bool lmouse = Control.MouseButtons.HasFlag(MouseButtons.Left);
            bool mmouse = Control.MouseButtons.HasFlag(MouseButtons.Middle);
            bool space = KeyboardState.IsKeyDown(Keys.Space);
            if (space || mmouse)
            {
                Viewport.Cursor = Cursors.SizeAll;
                if (lmouse || mmouse || !CBRE.Settings.View.Camera2DPanRequiresMouseClick)
                {
                    Coordinate point = new Coordinate(e.X, Viewport2D.Height - e.Y, 0);
                    if (_mouseDown != null)
                    {
                        Coordinate difference = _mouseDown - point;
                        Viewport2D.Position += difference / Viewport2D.Zoom;
                    }
                    _mouseDown = point;
                    e.Handled = true;
                }
            }

            Coordinate pt = Viewport2D.Expand(Viewport2D.ScreenToWorld(new Coordinate(e.X, Viewport2D.Height - e.Y, 0)));
            Mediator.Publish(EditorMediator.MouseCoordinatesChanged, pt);
        }

        public void MouseWheel(ViewportEvent e)
        {
            Coordinate before = Viewport2D.ScreenToWorld(e.X, Viewport2D.Height - e.Y);
            Viewport2D.Zoom *= DMath.Pow(CBRE.Settings.View.ScrollWheelZoomMultiplier, (e.Delta < 0 ? -1 : 1));
            Coordinate after = Viewport2D.ScreenToWorld(e.X, Viewport2D.Height - e.Y);
            Viewport2D.Position -= (after - before);

            Mediator.Publish(EditorMediator.ViewZoomChanged, Viewport2D.Zoom);
            if (KeyboardState.IsKeyDown(Keys.ControlKey))
            {
                Mediator.Publish(EditorMediator.SetZoomValue, Viewport2D.Zoom);
            }
        }

        public void MouseUp(ViewportEvent e)
        {
            bool space = KeyboardState.IsKeyDown(Keys.Space);
            bool req = CBRE.Settings.View.Camera2DPanRequiresMouseClick;
            if (space && (!req || e.Button == MouseButtons.Left))
            {
                e.Handled = true;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                e.Handled = true;
                Viewport.Cursor = Cursors.Default;
            }
        }

        private Coordinate _mouseDown;

        public void MouseDown(ViewportEvent e)
        {
            bool space = KeyboardState.IsKeyDown(Keys.Space);
            bool req = CBRE.Settings.View.Camera2DPanRequiresMouseClick;
            if (space && (!req || e.Button == MouseButtons.Left))
            {
                e.Handled = true;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                e.Handled = true;
                Viewport.Cursor = Cursors.SizeAll;
            }
            _mouseDown = new Coordinate(e.X, Viewport2D.Height - e.Y, 0);
        }

        public void MouseClick(ViewportEvent e)
        {

        }

        public void MouseDoubleClick(ViewportEvent e)
        {

        }

        public void MouseEnter(ViewportEvent e)
        {
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                Viewport.Cursor = Cursors.SizeAll;
            }
            Mediator.Publish(EditorMediator.ViewFocused);
            Mediator.Publish(EditorMediator.ViewZoomChanged, Viewport2D.Zoom);
        }

        public void MouseLeave(ViewportEvent e)
        {
            Viewport.Cursor = Cursors.Default;
            Mediator.Publish(EditorMediator.ViewUnfocused);
        }

        private const decimal ScrollStart = 1;
        private const decimal ScrollIncrement = 0.025m;
        private const int ScrollMaximum = 200;
        private const int ScrollPadding = 40;

        public void UpdateFrame(FrameInfo frame)
        {
            if (Viewport2D.IsFocused && _mouseDown != null && Control.MouseButtons.HasFlag(MouseButtons.Left) && !KeyboardState.IsKeyDown(Keys.Space))
            {
                System.Drawing.Point pt = Viewport2D.PointToClient(Control.MousePosition);
                if (pt.X < ScrollPadding)
                {
                    decimal mx = ScrollStart + ScrollIncrement * Math.Min(ScrollMaximum, ScrollPadding - pt.X);
                    mx = mx * mx + ScrollStart;
                    Viewport2D.Position.X -= mx / Viewport2D.Zoom;
                }
                else if (pt.X > Viewport2D.Width - ScrollPadding)
                {
                    decimal mx = ScrollStart + ScrollIncrement * Math.Min(ScrollMaximum, pt.X - (Viewport2D.Width - ScrollPadding));
                    mx = mx * mx + ScrollStart;
                    Viewport2D.Position.X += mx / Viewport2D.Zoom;
                }
                if (pt.Y < ScrollPadding)
                {
                    decimal my = ScrollStart + ScrollIncrement * Math.Min(ScrollMaximum, ScrollPadding - pt.Y);
                    my = my * my + ScrollStart;
                    Viewport2D.Position.Y += my / Viewport2D.Zoom;
                }
                else if (pt.Y > Viewport2D.Height - ScrollPadding)
                {
                    decimal my = ScrollStart + ScrollIncrement * Math.Min(ScrollMaximum, pt.Y - (Viewport2D.Height - ScrollPadding));
                    my = my * my + ScrollStart;
                    Viewport2D.Position.Y -= my / Viewport2D.Zoom;
                }
            }
        }

        public void PreRender()
        {

        }

        public void Render3D()
        {

        }

        public void Render2D()
        {

        }

        public void PostRender()
        {
            //
        }

        public void Notify(string message, object data)
        {
            Mediator.ExecuteDefault(this, message, data);
        }
    }
}
