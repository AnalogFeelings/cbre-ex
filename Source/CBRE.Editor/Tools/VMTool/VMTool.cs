using CBRE.Common.Mediator;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Editor.Actions.MapObjects.Selection;
using CBRE.Editor.Properties;
using CBRE.Editor.Rendering.Immediate;
using CBRE.Graphics;
using CBRE.Graphics.Helpers;
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
using Matrix = CBRE.Graphics.Helpers.Matrix;
using Select = CBRE.Settings.Select;
using View = CBRE.Settings.View;

namespace CBRE.Editor.Tools.VMTool
{
    public class VMTool : BaseBoxTool
    {
        private enum ShowPoints
        {
            All,
            Vertices,
            Midpoints
        }

        //private readonly VMForm _form;
        private readonly VMSidebarPanel _controlPanel;
        private readonly VMErrorsSidebarPanel _errorPanel;
        private readonly List<VMSubTool> _tools;
        private VMSubTool _currentTool;

        /// <summary>
        /// Key = copy, Value = original
        /// </summary>
        private Dictionary<Solid, Solid> _copies;

        public List<VMPoint> Points { get; private set; }
        public List<VMPoint> MoveSelection { get; private set; }

        private VMPoint _movingPoint;
        private Coordinate _snapPointOffset;
        private bool _dirty;

        private ShowPoints _showPoints;

        public VMTool()
        {
            _controlPanel = new VMSidebarPanel();
            _controlPanel.ToolSelected += VMToolSelected;
            _controlPanel.DeselectAll += DeselectAll;
            _controlPanel.Reset += Reset;

            _errorPanel = new VMErrorsSidebarPanel();
            _errorPanel.SelectError += SelectError;
            _errorPanel.FixError += FixError;
            _errorPanel.FixAllErrors += FixAllErrors;

            _tools = new List<VMSubTool>();

            AddTool(new StandardTool(this));
            AddTool(new ScaleTool(this));
            AddTool(new EditFaceTool(this));
            _currentTool = _tools.FirstOrDefault();

            _showPoints = ShowPoints.All;
        }

        private void SelectError(object sender, VMError error)
        {
            if (error != null)
            {
                VMToolSelected(null, _tools.First(x => x is StandardTool));
                Points.ForEach(x => x.IsSelected = x.Vertices != null && error.Vertices.Any(y => x.Vertices.Contains(y)));
                foreach (Face f in _copies.SelectMany(x => x.Key.Faces))
                {
                    f.IsSelected = error.Faces.Contains(f);
                }
            }
            else
            {
                foreach (Face f in _copies.SelectMany(x => x.Key.Faces))
                {
                    f.IsSelected = false;
                }
            }
        }

        private void FixError(object sender, object error)
        {

        }

        private void FixAllErrors(object sender)
        {

        }

        private void Reset(object sender)
        {
            _dirty = false;
            Commit(_copies.Values.ToList());
            _copies.Clear();
            SelectionChanged();
        }

        private void DeselectAll(object sender)
        {
            Points.ForEach(x => x.IsSelected = false);
            VertexSelectionChanged();
        }

        private void VMToolSelected(object sender, VMSubTool tool)
        {
            if (_currentTool == tool) return;
            _controlPanel.SetSelectedTool(tool);
            if (_currentTool != null) _currentTool.ToolDeselected(false);
            _currentTool = tool;
            if (_currentTool != null) _currentTool.ToolSelected(false);

            Mediator.Publish(EditorMediator.ContextualHelpChanged);
        }

        private void AddTool(VMSubTool tool)
        {
            _controlPanel.AddTool(tool);
            _tools.Add(tool);
        }

        private void VMStandardMode()
        {
            VMToolSelected(this, _tools.First(x => x is StandardTool));
        }

        private void VMScalingMode()
        {
            VMToolSelected(this, _tools.First(x => x is ScaleTool));
        }

        private void VMFaceEditMode()
        {
            VMToolSelected(this, _tools.First(x => x is EditFaceTool));
        }

        public override void DocumentChanged()
        {
            _controlPanel.Document = Document;
            _tools.ForEach(x => x.SetDocument(Document));
        }

        public override Image GetIcon()
        {
            return Resources.Tool_VM;
        }

        public override string GetName()
        {
            return Local.LocalString("tool.vertex_manipulation");
        }

        public override HotkeyTool? GetHotkeyToolType()
        {
            return HotkeyTool.VM;
        }

        public override IEnumerable<KeyValuePair<string, Control>> GetSidebarControls()
        {
            yield return new KeyValuePair<string, Control>(GetName(), _controlPanel);
            yield return new KeyValuePair<string, Control>(Local.LocalString("error.vm"), _errorPanel);
        }

        public override string GetContextualHelp()
        {
            if (_currentTool != null) return _currentTool.GetContextualHelp();
            return Local.LocalString("tool.vertex_manipulation.help");
        }

        protected override Color BoxColour
        {
            get { return Color.Orange; }
        }

        protected override Color FillColour
        {
            get { return Color.FromArgb(View.SelectionBoxBackgroundOpacity, Color.DodgerBlue); }
        }

        public void SetDirty(bool points, bool midpoints)
        {
            UpdateEditedFaces();
            if (points) RefreshPoints();
            if (midpoints) RefreshMidpoints();
            _errorPanel.SetErrorList(GetErrors());
            _dirty = true;
        }

        public IEnumerable<VMError> GetErrors()
        {
            foreach (KeyValuePair<Solid, Solid> kv in _copies)
            {
                Solid s = kv.Key;
                foreach (IGrouping<Plane, Face> g in s.GetCoplanarFaces().GroupBy(x => x.Plane))
                {
                    yield return new VMError(Local.LocalString("error.vm.coplanar_faces"), s, g);
                }
                foreach (Face f in s.GetBackwardsFaces(0.5m))
                {
                    yield return new VMError(Local.LocalString("error.vm.backwards_face"), s, new[] { f });
                }
                foreach (Face f in s.Faces)
                {
                    List<DataStructures.MapObjects.Vertex> np = f.GetNonPlanarVertices(0.5m).ToList();
                    bool found = false;
                    if (np.Any())
                    {
                        yield return new VMError(Local.LocalString("error.vm.nonplanar_vertex"), s, new[] { f }, np);
                        found = true;
                    }
                    foreach (IGrouping<Coordinate, DataStructures.MapObjects.Vertex> g in f.Vertices.GroupBy(x => x.Location).Where(x => x.Count() > 1))
                    {
                        yield return new VMError(Local.LocalString("error.vm.overlap_vertices"), s, new[] { f }, g);
                        found = true;
                    }
                    if (!f.IsConvex() && !found)
                    {
                        yield return new VMError(Local.LocalString("error.vm.concave_face"), s, new[] { f });
                    }
                }
            }
        }

        /// <summary>
        /// Get the VM points at the provided coordinate, ordered from top to bottom (for the supplied viewport).
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="viewport">The viewport</param>
        /// <returns>The points ordered from top to bottom, or an empty set if no points were found</returns>
        public List<VMPoint> GetVerticesAtPoint(int x, int y, Viewport2D viewport)
        {
            Coordinate p = viewport.ScreenToWorld(x, y);
            decimal d = 5 / viewport.Zoom; // Tolerance value = 5 pixels

            // Order by the unused coordinate in the view (which is the up axis) descending to get the "closest" point
            return (from point in Points
                    let c = viewport.Flatten(point.Coordinate)
                    where p.X >= c.X - d && p.X <= c.X + d && p.Y >= c.Y - d && p.Y <= c.Y + d
                    let unused = viewport.GetUnusedCoordinate(point.Coordinate)
                    orderby unused.X + unused.Y + unused.Z descending
                    select point).ToList();
        }

        public List<VMPoint> GetVerticesAtPoint(int x, int y, Viewport3D viewport)
        {
            Vector3 l = viewport.Camera.Location;
            Coordinate pos = new Coordinate((decimal)l.X, (decimal)l.Y, (decimal)l.Z);
            Coordinate p = new Coordinate(x, y, 0);
            const int d = 5;
            return (from point in Points
                    let c = viewport.WorldToScreen(point.Coordinate)
                    where c != null && c.Z <= 1
                    where p.X >= c.X - d && p.X <= c.X + d && p.Y >= c.Y - d && p.Y <= c.Y + d
                    orderby (pos - point.Coordinate).LengthSquared()
                    select point).ToList();
        }

        public IEnumerable<Solid> GetCopies()
        {
            return _copies.Keys;
        }

        public IEnumerable<Solid> GetOriginals()
        {
            return _copies.Values;
        }

        public List<VMPoint> GetSelectedPoints()
        {
            List<VMPoint> list = new List<VMPoint>();
            foreach (VMPoint point in Points.Where(point => point.IsSelected))
            {
                if (point.IsMidPoint && _showPoints != ShowPoints.Vertices)
                {
                    if (!list.Contains(point.MidpointStart)) list.Add(point.MidpointStart);
                    if (!list.Contains(point.MidpointEnd)) list.Add(point.MidpointEnd);
                }
                else if (!point.IsMidPoint && _showPoints != ShowPoints.Midpoints)
                {
                    if (!list.Contains(point)) list.Add(point);
                }
            }
            return list;
        }

        private void Commit(IList<Solid> solids)
        {
            if (!solids.Any()) return;

            // Unhide the solids
            foreach (Solid solid in solids)
            {
                solid.IsCodeHidden = false;
            }
            List<KeyValuePair<Solid, Solid>> kvs = _copies.Where(x => solids.Contains(x.Value)).ToList();
            foreach (KeyValuePair<Solid, Solid> kv in kvs)
            {
                _copies.Remove(kv.Key);
                foreach (Face f in kv.Key.Faces) f.IsSelected = false;
                foreach (Face f in kv.Value.Faces) f.IsSelected = false;
            }
            if (_dirty)
            {
                // Commit the changes
                ReplaceObjects edit = new ReplaceObjects(kvs.Select(x => x.Value), kvs.Select(x => x.Key));
                Document.PerformAction(Local.LocalString("tool.vm"), edit);
            }
        }

        private void SelectionChanged()
        {
            List<Solid> selectedSolids = Document.Selection.GetSelectedObjects().OfType<Solid>().ToList();
            List<Solid> commit = _copies.Values.Where(x => !selectedSolids.Contains(x)).ToList();
            Commit(commit);
            if (!_copies.Any()) _dirty = false;
            foreach (Solid solid in selectedSolids.Where(x => !_copies.ContainsValue(x)))
            {
                Solid copy = (Solid)solid.Clone();
                copy.IsSelected = false;
                foreach (Face f in copy.Faces) f.IsSelected = false;
                _copies.Add(copy, solid);

                // Set all the original solids to hidden
                // (do this after we clone it so the clones aren't hidden too)
                solid.IsCodeHidden = true;
            }
            RefreshPoints();
            RefreshMidpoints();
        }

        public override void ToolSelected(bool preventHistory)
        {
            Editor.Instance.Focus();

            // Init the points and copy caches
            _copies = new Dictionary<Solid, Solid>();
            Points = new List<VMPoint>();

            SelectionChanged();

            _snapPointOffset = null;
            _movingPoint = null;
            MoveSelection = null;

            if (_currentTool != null) _currentTool.ToolSelected(preventHistory);

            Mediator.Subscribe(EditorMediator.SelectionChanged, this);
            Mediator.Subscribe(HotkeysMediator.VMStandardMode, this);
            Mediator.Subscribe(HotkeysMediator.VMScalingMode, this);
            Mediator.Subscribe(HotkeysMediator.VMFaceEditMode, this);
        }

        public override void ToolDeselected(bool preventHistory)
        {
            Mediator.UnsubscribeAll(this);

            if (_currentTool != null) _currentTool.ToolDeselected(preventHistory);

            // Commit the changes
            Commit(_copies.Values.ToList());

            _copies = null;
            Points = null;
            _snapPointOffset = null;
            _movingPoint = null;
            MoveSelection = null;
        }

        private void VertexSelectionChanged()
        {
            _currentTool.SelectionChanged();
        }

        /// <summary>
        /// Updates the points list (does not update midpoints)
        /// </summary>
        public void RefreshPoints()
        {
            var selected = Points.Where(x => !x.IsMidPoint && x.IsSelected).Select(x => new { x.Coordinate, x.Solid }).ToList();
            Points.RemoveAll(x => !x.IsMidPoint);
            foreach (Solid copy in _copies.Keys)
            {
                // Add the vertex points
                // Group by location per solid, duplicate coordinates are "attached" and moved at the same time
                foreach (IGrouping<Coordinate, DataStructures.MapObjects.Vertex> group in copy.Faces.SelectMany(x => x.Vertices).GroupBy(x => x.Location.Round(2)))
                {
                    Points.Add(new VMPoint
                    {
                        Solid = copy, // ten four, solid copy
                        Coordinate = group.First().Location,
                        Vertices = group.ToList(),
                        IsSelected = selected.Any(x => x.Solid == copy && x.Coordinate == group.First().Location)
                    });
                }
            }
        }

        /// <summary>
        /// Updates the positions of all midpoints.
        /// </summary>
        private void RefreshMidpoints(bool recreate = true)
        {
            var selected = Points.Where(x => x.IsMidPoint && x.IsSelected).Select(x => new { Start = x.MidpointStart.Coordinate, End = x.MidpointEnd.Coordinate, x.Solid }).ToList();
            if (recreate) Points.RemoveAll(x => x.IsMidPoint);
            foreach (Solid copy in _copies.Keys)
            {
                foreach (var group in copy.Faces.SelectMany(x => x.GetLines()).GroupBy(x => new { x.Start, x.End }))
                {
                    Coordinate s = group.Key.Start;
                    Coordinate e = group.Key.End;
                    Coordinate coord = (s + e) / 2;
                    VMPoint mpStart = Points.First(x => !x.IsMidPoint && x.Coordinate == s);
                    VMPoint mpEnd = Points.First(x => !x.IsMidPoint && x.Coordinate == e);
                    List<VMPoint> existingPoints = Points.Where(x => x.IsMidPointFor(mpStart, mpEnd)).ToList();
                    if (recreate && !existingPoints.Any())
                    {
                        Points.Add(new VMPoint
                        {
                            Solid = copy,
                            Coordinate = coord,
                            IsMidPoint = true,
                            MidpointStart = mpStart,
                            MidpointEnd = mpEnd,
                            IsSelected = selected.Any(x => x.Solid == copy && x.Start == mpStart.Coordinate && x.End == mpEnd.Coordinate)
                        });
                    }
                    else
                    {
                        foreach (VMPoint point in Points.Where(x => x.IsMidPoint && x.MidpointStart.Coordinate == s && x.MidpointEnd.Coordinate == e))
                        {
                            point.Coordinate = coord;
                        }
                    }
                }
            }
        }

        private void UpdateMidpoints()
        {
            foreach (VMPoint mp in Points.Where(x => x.IsMidPoint))
            {
                mp.Coordinate = (mp.MidpointStart.Coordinate + mp.MidpointEnd.Coordinate) / 2;
            }
        }

        public void UpdateEditedFaces()
        {
            foreach (Face face in GetCopies().SelectMany(x => x.Faces))
            {
                if (face.Vertices.Count >= 3) face.Plane = new Plane(face.Vertices[0].Location, face.Vertices[1].Location, face.Vertices[2].Location);
                face.CalculateTextureCoordinates(true);
                face.UpdateBoundingBox();
            }
        }

        private Coordinate GetIntersectionPoint(MapObject obj, Line line)
        {
            if (obj == null) return null;

            Solid solid = obj as Solid;
            if (solid == null) return obj.GetIntersectionPoint(line);

            return solid.Faces.Where(x => x.Opacity > 0 && !x.IsHidden)
                .Select(x => x.GetIntersectionPoint(line))
                .Where(x => x != null)
                .OrderBy(x => (x - line.Start).VectorMagnitude())
                .FirstOrDefault();
        }

        private void MouseDown(Viewport3D vp, ViewportEvent e)
        {
            if (!_currentTool.NoSelection())
            {
                List<VMPoint> vtxs = _currentTool.GetVerticesAtPoint(e.X, vp.Height - e.Y, vp);

                if (vtxs.Any())
                {
                    // Use the topmost vertex as the control point
                    VMPoint vtx = vtxs.First();

                    // Mouse down on a point
                    if (vtx.IsSelected && KeyboardState.Ctrl && _currentTool.ShouldDeselect(vtxs))
                    {
                        // If the vertex is selected and ctrl is down, deselect the vertices
                        vtxs.ForEach(x => x.IsSelected = false);
                    }
                    else
                    {
                        if (!vtx.IsSelected && !KeyboardState.Ctrl && _currentTool.ShouldDeselect(vtxs))
                        {
                            // If we aren't clicking on a selected point and ctrl is not down, deselect the others
                            Points.ForEach(x => x.IsSelected = false);
                            // If this point is already selected, don't deselect others. This is the same behaviour as 2D selection.
                        }
                        vtxs.ForEach(x => x.IsSelected = true);
                    }
                    VertexSelectionChanged();

                    // Don't do other click operations
                    return;
                }

                // Nothing clicked
                if (!KeyboardState.Ctrl)
                {
                    // Deselect all the points if not ctrl-ing
                    Points.ForEach(x => x.IsSelected = false);
                }
            }
            if (!_currentTool.No3DSelection())
            {
                // Do selection
                Line ray = vp.CastRayFromScreen(e.X, e.Y);
                IEnumerable<MapObject> hits = Document.Map.WorldSpawn.GetAllNodesIntersectingWith(ray, true);
                Solid solid = hits
                    .OfType<Solid>()
                    .Select(x => new { Item = x, Intersection = GetIntersectionPoint(x, ray) })
                    .Where(x => x.Intersection != null)
                    .OrderBy(x => (x.Intersection - ray.Start).VectorMagnitude())
                    .Select(x => x.Item)
                    .FirstOrDefault();

                if (solid != null)
                {
                    if (solid.IsSelected && KeyboardState.Ctrl)
                    {
                        // deselect solid
                        MapObject[] select = new MapObject[0];
                        Solid[] deselect = new[] { solid };
                        Document.PerformAction(Local.LocalString("tool.vm.deselect"), new ChangeSelection(select, deselect));
                    }
                    else if (!solid.IsSelected)
                    {
                        // select solid
                        Solid[] select = new[] { solid };
                        IEnumerable<MapObject> deselect = !KeyboardState.Ctrl ? Document.Selection.GetSelectedObjects() : new MapObject[0];
                        Document.PerformAction(Local.LocalString("tool.vm.select"), new ChangeSelection(select, deselect));
                    }

                    // Don't do other click operations
                    return;
                }
            }

            base.MouseDown(vp, e);
        }

        private bool _clickSelectionDone = false;
        public override void MouseDown(ViewportBase vp, ViewportEvent e)
        {
            _clickSelectionDone = false;
            if (_currentTool != null)
            {
                // If the current tool handles the event, we're done
                _currentTool.MouseDown(vp, e);
                if (e.Handled) return;
            }
            if (!(vp is Viewport2D))
            {
                MouseDown((Viewport3D)vp, e);
                return;
            }

            if (_currentTool == null) return;

            if (_currentTool.NoSelection()) return;

            Viewport2D viewport = (Viewport2D)vp;

            // Otherwise we try a selection
            // Find the clicked vertices
            List<VMPoint> vtxs = _currentTool.GetVerticesAtPoint(e.X, viewport.Height - e.Y, viewport);

            if (!vtxs.Any())
            {
                // Nothing clicked
                if (!KeyboardState.Ctrl)
                {
                    // Deselect all the points if not ctrl-ing
                    Points.ForEach(x => x.IsSelected = false);
                }

                // Try to select in 2D

                // Create a box to represent the click, with a tolerance level
                Coordinate unused = viewport.GetUnusedCoordinate(new Coordinate(100000, 100000, 100000));
                decimal tolerance = 4 / viewport.Zoom; // Selection tolerance of four pixels
                Coordinate used = viewport.Expand(new Coordinate(tolerance, tolerance, 0));
                Coordinate add = used + unused;
                Coordinate click = viewport.Expand(viewport.ScreenToWorld(e.X, viewport.Height - e.Y));
                Box box = new Box(click - add, click + add);

                bool centerHandles = Select.DrawCenterHandles;
                bool centerOnly = Select.ClickSelectByCenterHandlesOnly;
                // Get the first element that intersects with the box
                Solid solid = Document.Map.WorldSpawn.GetAllNodesIntersecting2DLineTest(box, centerHandles, centerOnly).OfType<Solid>().FirstOrDefault();

                if (solid != null)
                {
                    // select solid
                    Solid[] select = new[] { solid };
                    IEnumerable<MapObject> deselect = !KeyboardState.Ctrl ? Document.Selection.GetSelectedObjects() : new MapObject[0];
                    Document.PerformAction(Local.LocalString("tool.vm.select"), new ChangeSelection(select, deselect));

                    // Don't do other click operations
                    return;
                }

                base.MouseDown(vp, e);
                return;
            }

            VMPoint vtx = vtxs.First();

            // When clicking, only select vertices in a single solid
            vtxs = vtxs.Where(x => x.Solid == vtx.Solid).ToList();

            // If any vertices are selected, don't change the selection yet
            if (!vtxs.Any(x => x.IsSelected))
            {
                _clickSelectionDone = true;
                DoSelection(vtxs, viewport);
            }

            // Only move selected vertices
            vtxs = vtxs.Where(x => x.IsSelected).ToList();
            if (!vtxs.Any()) return;

            // Use the fist vertex as the control point
            _currentTool.DragStart(vtxs);
            MoveSelection = vtxs;
            _snapPointOffset = SnapIfNeeded(viewport.Expand(viewport.ScreenToWorld(e.X, viewport.Height - e.Y))) - viewport.ZeroUnusedCoordinate(vtx.Coordinate);
            _movingPoint = vtx;
        }

        public override void MouseClick(ViewportBase viewport, ViewportEvent e)
        {
            Viewport2D vp = viewport as Viewport2D;
            if (vp == null || _clickSelectionDone) return;

            List<VMPoint> vtxs = _currentTool.GetVerticesAtPoint(e.X, viewport.Height - e.Y, vp);
            DoSelection(vtxs, vp);
        }

        private void DoSelection(List<VMPoint> vertices, Viewport2D vp)
        {
            if (!vertices.Any()) return;

            VMPoint vtx = vertices.First();
            // Shift selects only the topmost point
            if (KeyboardState.Shift)
            {
                vertices.Clear();
                vertices.Add(vtx);
            }

            // Vertex found, cancel the box if needed
            BoxDrawnCancel(vp);

            // Mouse down on a point
            if (vtx.IsSelected && KeyboardState.Ctrl && _currentTool.ShouldDeselect(vertices))
            {
                // If the vertex is selected and ctrl is down, deselect the vertices
                vertices.ForEach(x => x.IsSelected = false);
            }
            else
            {
                if (!vtx.IsSelected && !KeyboardState.Ctrl && _currentTool.ShouldDeselect(vertices))
                {
                    // If we aren't clicking on a selected point and ctrl is not down, deselect the others
                    Points.ForEach(x => x.IsSelected = false);
                    // If this point is already selected, don't deselect others. This is so we can move multiple points easily.
                }
                vertices.ForEach(x => x.IsSelected = true);
            }
            VertexSelectionChanged();
        }

        public override void MouseDoubleClick(ViewportBase viewport, ViewportEvent e)
        {
            // Not used
        }

        public override void MouseUp(ViewportBase viewport, ViewportEvent e)
        {
            base.MouseUp(viewport, e);

            if (_currentTool == null) return;
            _currentTool.MouseUp(viewport, e);

            if (!(viewport is Viewport2D)) return;
            if (_currentTool.NoSelection()) return;

            if (!e.Handled)
            {
                if (MoveSelection != null && !KeyboardState.Ctrl)
                {
                    // If we were clicking on a point, and the mouse hasn't moved yet,
                    // and ctrl is not down, deselect the other points.
                    Points.ForEach(x => x.IsSelected = false);
                    MoveSelection.ForEach(x => x.IsSelected = true);
                    VertexSelectionChanged();

                    _currentTool.MouseClick(viewport, e);
                }
                else
                {
                    _currentTool.DragEnd();
                }
            }

            RefreshMidpoints();
            _snapPointOffset = null;
            _movingPoint = null;
            MoveSelection = null;
        }

        protected override void LeftMouseUpDrawing(Viewport2D viewport, ViewportEvent e)
        {
            base.LeftMouseUpDrawing(viewport, e);
            if (Select.AutoSelectBox)
            {
                BoxDrawnConfirm(viewport);
            }
        }

        public override void MouseMove(ViewportBase vp, ViewportEvent e)
        {
            base.MouseMove(vp, e);

            if (_currentTool == null) return;

            _currentTool.MouseMove(vp, e);
            if (e.Handled) return;

            if (!(vp is Viewport2D)) return;
            if (_currentTool.NoSelection()) return;

            Viewport2D viewport = (Viewport2D)vp;

            if (_movingPoint == null)
            {
                // Not moving a point, just test for the cursor.
                List<VMPoint> vtxs = _currentTool.GetVerticesAtPoint(e.X, viewport.Height - e.Y, viewport);
                if (vtxs.Any()) viewport.Cursor = Cursors.Cross;
                else if (viewport.Cursor == Cursors.Cross) viewport.Cursor = Cursors.Default;
            }
            else
            {
                // Moving a point, get the delta moved
                Coordinate point = SnapIfNeeded(viewport.Expand(viewport.ScreenToWorld(e.X, viewport.Height - e.Y)));
                if (!KeyboardState.Alt && KeyboardState.Shift)
                {
                    // If shift is down, retain the offset the point was at before (relative to the grid)
                    point += _snapPointOffset;
                }
                Coordinate moveDistance = point - viewport.ZeroUnusedCoordinate(_movingPoint.Coordinate);
                _currentTool.DragMove(moveDistance);
                //RefreshMidpoints(false);
                UpdateMidpoints();
                MoveSelection = null;
            }
        }

        public override HotkeyInterceptResult InterceptHotkey(HotkeysMediator hotkeyMessage, object parameters)
        {
            switch (hotkeyMessage)
            {
                case HotkeysMediator.HistoryUndo:
                case HotkeysMediator.HistoryRedo:
                    MessageBox.Show(Local.LocalString("tool.vm.undo"));
                    return HotkeyInterceptResult.Abort;
                case HotkeysMediator.OperationsPaste:
                case HotkeysMediator.OperationsPasteSpecial:
                    return HotkeyInterceptResult.SwitchToSelectTool;
                case HotkeysMediator.SwitchTool:
                    if (parameters is HotkeyTool && (HotkeyTool)parameters == GetHotkeyToolType())
                    {
                        CycleShowPoints();
                        return HotkeyInterceptResult.Abort;
                    }
                    break;
            }
            return HotkeyInterceptResult.Continue;
        }

        private void CycleShowPoints()
        {
            int side = (int)_showPoints;
            side = (side + 1) % (Enum.GetValues(typeof(ShowPoints)).Length);
            _showPoints = (ShowPoints)side;
        }

        public override void BoxDrawnConfirm(ViewportBase viewport)
        {
            Box box;
            if (GetSelectionBox(out box))
            {
                foreach (VMPoint point in Points.Where(x => !x.IsMidPoint && box.CoordinateIsInside(x.Coordinate)))
                {
                    // Select all the points in the box
                    point.IsSelected = true;
                }
                VertexSelectionChanged();
            }

            base.BoxDrawnConfirm(viewport);
        }

        protected override void Render2D(Viewport2D vp)
        {
            base.Render2D(vp);

            if (_currentTool != null) _currentTool.Render2D(vp);

            // Render out the solid previews
            GL.Color3(Color.Pink);
            Matrix.Push();
            Matrix4 matrix = vp.GetModelViewMatrix();
            GL.MultMatrix(ref matrix);
            MapObjectRenderer.DrawWireframe(_copies.Keys.SelectMany(x => x.Faces), true, false);
            Matrix.Pop();

            // Draw in order by the unused coordinate (the up axis for this viewport)
            List<VMPoint> ordered = (from point in Points
                                     where (point.IsMidPoint && _showPoints != ShowPoints.Vertices) || (!point.IsMidPoint && _showPoints != ShowPoints.Midpoints)
                                     let unused = vp.GetUnusedCoordinate(point.Coordinate)
                                     orderby point.IsSelected, unused.X + unused.Y + unused.Z
                                     select point).ToList();
            // Render out the point handles
            double z = (double)vp.Zoom;
            GL.Begin(PrimitiveType.Quads);
            foreach (VMPoint point in ordered)
            {
                Coordinate c = vp.Flatten(point.Coordinate);
                GL.Color3(Color.Black);
                GLX.Square(new Vector2d(c.DX, c.DY), 4, z, true);
                GL.Color3(point.GetColour());
                GLX.Square(new Vector2d(c.DX, c.DY), 3, z, true);
            }
            GL.End();
        }

        protected override void Render3D(Viewport3D vp)
        {
            base.Render3D(vp);

            if (_currentTool != null) _currentTool.Render3D(vp);

            TextureHelper.Unbind();

            if (_currentTool == null || _currentTool.DrawVertices())
            {
                // Get us into 2D rendering
                Matrix.Set(MatrixMode.Projection);
                Matrix.Identity();
                Graphics.Helpers.Viewport.Orthographic(0, 0, vp.Width, vp.Height);
                Matrix.Set(MatrixMode.Modelview);
                Matrix.Identity();

                Coordinate half = new Coordinate(vp.Width, vp.Height, 0) / 2;
                // Render out the point handles
                GL.Begin(PrimitiveType.Quads);
                foreach (VMPoint point in Points)
                {
                    if (point.IsMidPoint && _showPoints == ShowPoints.Vertices) continue;
                    if (!point.IsMidPoint && _showPoints == ShowPoints.Midpoints) continue;

                    Coordinate c = vp.WorldToScreen(point.Coordinate);
                    if (c == null || c.Z > 1) continue;
                    c -= half;

                    GL.Color3(Color.Black);
                    GL.Vertex2(c.DX - 4, c.DY - 4);
                    GL.Vertex2(c.DX - 4, c.DY + 4);
                    GL.Vertex2(c.DX + 4, c.DY + 4);
                    GL.Vertex2(c.DX + 4, c.DY - 4);

                    GL.Color3(point.GetColour());
                    GL.Vertex2(c.DX - 3, c.DY - 3);
                    GL.Vertex2(c.DX - 3, c.DY + 3);
                    GL.Vertex2(c.DX + 3, c.DY + 3);
                    GL.Vertex2(c.DX + 3, c.DY - 3);
                }
                GL.End();

                // Get back into 3D rendering
                Matrix.Set(MatrixMode.Projection);
                Matrix.Identity();
                Graphics.Helpers.Viewport.Perspective(0, 0, vp.Width, vp.Height, View.CameraFOV);
                Matrix.Set(MatrixMode.Modelview);
                Matrix.Identity();
                vp.Camera.Position();
            }

            Viewport3D.ViewType type = vp.Type;
            bool shaded = type == Viewport3D.ViewType.Shaded || type == Viewport3D.ViewType.Textured || type == Viewport3D.ViewType.Lightmapped,
                 textured = type == Viewport3D.ViewType.Textured || type == Viewport3D.ViewType.Lightmapped,
                 wireframe = type == Viewport3D.ViewType.Wireframe;

            // Render out the solid previews
            GL.Color3(Color.White);
            List<Face> faces = _copies.Keys.SelectMany(x => x.Faces).ToList();

            if (!wireframe)
            {
                if (shaded) MapObjectRenderer.EnableLighting();
                GL.Enable(EnableCap.Texture2D);
                MapObjectRenderer.DrawFilled(faces.Where(x => !x.IsSelected), Color.FromArgb(255, 64, 192, 64), textured);
                MapObjectRenderer.DrawFilled(faces.Where(x => x.IsSelected), Color.FromArgb(255, 255, 128, 128), textured);
                GL.Disable(EnableCap.Texture2D);
                MapObjectRenderer.DisableLighting();

                GL.Color3(Color.Pink);
                MapObjectRenderer.DrawWireframe(faces, true, false);
            }
            else
            {
                GL.Color4(Color.FromArgb(255, 64, 192, 64));
                MapObjectRenderer.DrawWireframe(faces.Where(x => !x.IsSelected), true, false);
                GL.Color4(Color.FromArgb(255, 255, 128, 128));
                MapObjectRenderer.DrawWireframe(faces.Where(x => x.IsSelected), true, false);
            }
        }

        public override void KeyDown(ViewportBase viewport, ViewportEvent e)
        {
            if (_currentTool != null) _currentTool.KeyDown(viewport, e);
            if (e.Handled) return;
            base.KeyDown(viewport, e);
        }

        public override void Render(ViewportBase viewport)
        {
            if (_currentTool != null) _currentTool.Render(viewport);
            base.Render(viewport);
        }

        public override void MouseEnter(ViewportBase viewport, ViewportEvent e)
        {
            if (_currentTool != null) _currentTool.MouseEnter(viewport, e);
            if (e.Handled) return;
            base.MouseEnter(viewport, e);
        }

        public override void MouseLeave(ViewportBase viewport, ViewportEvent e)
        {
            if (_currentTool != null) _currentTool.MouseLeave(viewport, e);
            if (e.Handled) return;
            base.MouseLeave(viewport, e);
        }

        public override void MouseWheel(ViewportBase viewport, ViewportEvent e)
        {
            if (_currentTool != null) _currentTool.MouseWheel(viewport, e);
            if (e.Handled) return;
            base.MouseWheel(viewport, e);
        }

        public override void KeyPress(ViewportBase viewport, ViewportEvent e)
        {
            if (_currentTool != null) _currentTool.KeyPress(viewport, e);
            if (e.Handled) return;
            base.KeyPress(viewport, e);
        }

        public override void KeyUp(ViewportBase viewport, ViewportEvent e)
        {
            if (_currentTool != null) _currentTool.KeyUp(viewport, e);
            if (e.Handled) return;
            base.KeyUp(viewport, e);
        }

        public override void UpdateFrame(ViewportBase viewport, FrameInfo frame)
        {
            if (_currentTool != null) _currentTool.UpdateFrame(viewport, frame);
            base.UpdateFrame(viewport, frame);
        }

        public override void PreRender(ViewportBase viewport)
        {
            if (_currentTool != null) _currentTool.PreRender(viewport);
            base.PreRender(viewport);
        }
    }
}
