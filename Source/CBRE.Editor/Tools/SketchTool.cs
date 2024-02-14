using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.DataStructures.Transformations;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Editor.Actions.MapObjects.Selection;
using CBRE.Editor.Brushes;
using CBRE.Editor.Extensions;
using CBRE.Editor.Properties;
using CBRE.Editor.Rendering.Immediate;
using CBRE.Localization;
using CBRE.Settings;
using CBRE.UI;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Select = CBRE.Settings.Select;

namespace CBRE.Editor.Tools
{
    public class SketchTool : BaseTool
    {
        public enum SketchState
        {
            None,
            Ready,
            DrawingBase,
            DrawingVolume
        }

        private SketchState _state;
        private Face _currentFace;
        private Face _cloneFace;
        private Coordinate _intersection;
        private Polygon _base;
        private decimal _depth;
        private Plane _volumePlane;

        public SketchTool()
        {
            Usage = ToolUsage.View3D;
        }

        public override void ToolSelected(bool preventHistory)
        {
            _state = SketchState.None;
            _currentFace = _cloneFace = null;
            _intersection = null;
            _base = null;
            _depth = 0;
            _volumePlane = null;
        }

        public override void ToolDeselected(bool preventHistory)
        {
            _state = SketchState.None;
            _currentFace = _cloneFace = null;
            _intersection = null;
            _base = null;
            _depth = 0;
            _volumePlane = null;
        }

        public override Image GetIcon()
        {
            return Resources.Tool_Translate;
        }

        public override string GetName()
        {
            return Local.LocalString("tool.sketch");
        }

        public override HotkeyTool? GetHotkeyToolType()
        {
            return HotkeyTool.Sketch;
        }

        public override string GetContextualHelp()
        {
            return Local.LocalString("tool.sketch.help");
        }

        public override IEnumerable<KeyValuePair<string, Control>> GetSidebarControls()
        {
            yield return new KeyValuePair<string, Control>(GetName(), BrushManager.SidebarControl);
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
            //
            switch (_state)
            {
                case SketchState.None:
                    // nothin
                    break;
                case SketchState.Ready:
                    if (e.Button != MouseButtons.Left) break;
                    _base = new Polygon(_currentFace.Plane, 1);
                    _base.Transform(new UnitTranslate(_intersection - _base.Vertices[0]));
                    _state = SketchState.DrawingBase;
                    break;
                case SketchState.DrawingBase:
                    if (e.Button == MouseButtons.Right)
                    {
                        // Cancel
                        _state = SketchState.None;
                        _base = null;
                    }
                    else if (e.Button == MouseButtons.Left)
                    {
                        ExpandBase(_intersection);
                        _volumePlane = new Plane(_base.Vertices[1], _base.Vertices[2], _base.Vertices[2] + _base.Plane.Normal);
                        _state = SketchState.DrawingVolume;
                    }
                    break;
                case SketchState.DrawingVolume:
                    if (e.Button == MouseButtons.Right)
                    {
                        _state = SketchState.DrawingBase;
                        _volumePlane = null;
                    }
                    else if (e.Button == MouseButtons.Left)
                    {
                        Coordinate diff = _intersection - _base.Vertices[2];
                        int sign = _base.Plane.OnPlane(_intersection) < 0 ? -1 : 1;
                        _depth = diff.VectorMagnitude() * sign;
                        CreateBrush(_base, _depth);
                        _base = null;
                        _volumePlane = null;
                        _state = SketchState.None;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CreateBrush(Polygon poly, decimal depth)
        {
            MapObject brush = GetBrush(poly, depth, Document.Map.IDGenerator);
            if (brush == null) return;
            IAction action = new Create(Document.Map.WorldSpawn.ID, brush);
            if (Select.SelectCreatedBrush)
            {
                brush.IsSelected = true;
                if (Select.DeselectOthersWhenSelectingCreation)
                {
                    action = new ActionCollection(new ChangeSelection(new MapObject[0], Document.Selection.GetSelectedObjects()), action);
                }
            }
            Document.PerformAction(Local.LocalString("tool.brush.create", BrushManager.CurrentBrush.Name.ToLower()), action);
        }

        private MapObject GetBrush(Polygon bounds, decimal depth, IDGenerator idGenerator)
        {
            return null;
        }

        public override void MouseClick(ViewportBase viewport, ViewportEvent e)
        {
            //
        }

        public override void MouseDoubleClick(ViewportBase viewport, ViewportEvent e)
        {
            //
        }

        public override void MouseUp(ViewportBase viewport, ViewportEvent e)
        {
            switch (_state)
            {
                case SketchState.None:
                case SketchState.Ready:
                    // nothin
                    break;
                case SketchState.DrawingBase:
                    // IF dragging base
                    // left: go to volume mode
                    break;
                case SketchState.DrawingVolume:
                    // nothin
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public override void MouseWheel(ViewportBase viewport, ViewportEvent e)
        {
            //
        }

        private void ExpandBase(Coordinate endPoint)
        {
            Coordinate axis = _base.Plane.GetClosestAxisToNormal();
            Coordinate start = _base.Vertices[0] - _base.Vertices[0].ComponentMultiply(axis);
            Coordinate end = endPoint - endPoint.ComponentMultiply(axis);
            Coordinate diff = end - start;
            Coordinate addx, addy;
            if (axis == Coordinate.UnitX)
            {
                addx = diff.ComponentMultiply(Coordinate.UnitY);
                addy = diff.ComponentMultiply(Coordinate.UnitZ);
            }
            else if (axis == Coordinate.UnitY)
            {
                addx = diff.ComponentMultiply(Coordinate.UnitX);
                addy = diff.ComponentMultiply(Coordinate.UnitZ);
            }
            else
            {
                addx = diff.ComponentMultiply(Coordinate.UnitX);
                addy = diff.ComponentMultiply(Coordinate.UnitY);
            }
            Line linex = new Line(start + addx, start + addx + axis);
            Line liney = new Line(start + addy, start + addy + axis);
            _base.Vertices[1] = _base.Plane.GetIntersectionPoint(linex, true, true);
            _base.Vertices[2] = endPoint;
            _base.Vertices[3] = _base.Plane.GetIntersectionPoint(liney, true, true);
        }

        public override void MouseMove(ViewportBase viewport, ViewportEvent e)
        {
            Viewport3D vp = viewport as Viewport3D;
            if (vp == null) return;

            UpdateCurrentFace(vp, e);

            switch (_state)
            {
                case SketchState.None:
                case SketchState.Ready:
                    // face detect
                    break;
                case SketchState.DrawingBase:
                    ExpandBase(_intersection);
                    break;
                case SketchState.DrawingVolume:
                    Coordinate diff = _intersection - _base.Vertices[2];
                    diff = diff.ComponentMultiply(_base.Plane.GetClosestAxisToNormal());
                    int sign = _base.Plane.OnPlane(_intersection) < 0 ? -1 : 1;
                    _depth = diff.VectorMagnitude() * sign;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateCurrentFace(Viewport3D viewport, ViewportEvent e)
        {
            Line ray = viewport.CastRayFromScreen(e.X, e.Y);

            // The face doesn't change when drawing, just update the intersection
            if (_state == SketchState.DrawingBase || _state == SketchState.DrawingVolume)
            {
                _intersection = (_state == SketchState.DrawingBase ? _currentFace.Plane : _volumePlane).GetIntersectionPoint(ray, true, true);
                return;
            }

            var isect = Document.Map.WorldSpawn.GetAllNodesIntersectingWith(ray)
                .OfType<Solid>()
                .SelectMany(x => x.Faces)
                .Select(x => new { Item = x, Intersection = x.GetIntersectionPoint(ray) })
                .Where(x => x.Intersection != null)
                .OrderBy(x => (x.Intersection - ray.Start).VectorMagnitude())
                .FirstOrDefault();

            if (isect != null)
            {
                if (_currentFace != isect.Item)
                {
                    _cloneFace = isect.Item.Clone();
                    _cloneFace.Transform(new UnitTranslate(isect.Item.Plane.Normal * 0.1m), TransformFlags.None);
                }

                _currentFace = isect.Item;
                _intersection = isect.Intersection;
                _state = SketchState.Ready;
            }
            else
            {
                _cloneFace = null;
                _currentFace = null;
                _intersection = null;
                _state = SketchState.None;
            }
        }


        public override void KeyPress(ViewportBase viewport, ViewportEvent e)
        {
            switch (_state)
            {
                case SketchState.None:
                case SketchState.Ready:
                    // nothin
                    break;
                case SketchState.DrawingBase:
                case SketchState.DrawingVolume:
                    // esc: cancel
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        private IEnumerable<Face> GetSides()
        {
            if (_state == SketchState.None || _state == SketchState.Ready || _base == null) yield break;

            Face b = new Face(0) { Plane = _base.Plane };
            b.Vertices.AddRange(_base.Vertices.Select(x => new Vertex(x, b)));
            b.UpdateBoundingBox();
            yield return b;

            if (_state != SketchState.DrawingVolume) yield break;

            Face t = new Face(0) { Plane = new Plane(_base.Plane.Normal, _base.Plane.PointOnPlane + _base.Plane.Normal * _depth) };
            t.Vertices.AddRange(_base.Vertices.Select(x => new Vertex(x + _base.Plane.Normal * _depth, t)));
            t.UpdateBoundingBox();
            yield return t;
        }

        public override void Render(ViewportBase viewport)
        {
            // Render
            if (_base != null)
            {/*
                var faces = _drawing.GetBoxFaces().Select(x =>
                {
                    var f = new Face(0) { Plane = new Plane(x[0], x[1], x[2])};
                    f.Vertices.AddRange(x.Select(v => new Vertex(v + f.Plane.Normal * 0.1m, f)));
                    return f;
                });*
              */
                Viewport3D vp3 = viewport as Viewport3D;
                if (vp3 == null) return;

                GL.Disable(EnableCap.CullFace);
                List<Face> faces = GetSides().OrderByDescending(x => (vp3.Camera.LookAt.ToCoordinate() - x.BoundingBox.Center).LengthSquared()).ToList();
                MapObjectRenderer.DrawFilled(faces, Color.FromArgb(64, Color.DodgerBlue), false, false);
                GL.Enable(EnableCap.CullFace);
            }
            else if (_cloneFace != null)
            {
                MapObjectRenderer.DrawFilled(new[] { _cloneFace }, Color.FromArgb(64, Color.Orange), false, false);
            }
        }

        public override HotkeyInterceptResult InterceptHotkey(HotkeysMediator hotkeyMessage, object parameters)
        {
            switch (hotkeyMessage)
            {
                case HotkeysMediator.OperationsPasteSpecial:
                case HotkeysMediator.OperationsPaste:
                    return HotkeyInterceptResult.SwitchToSelectTool;
            }
            return HotkeyInterceptResult.Continue;
        }
    }
}
