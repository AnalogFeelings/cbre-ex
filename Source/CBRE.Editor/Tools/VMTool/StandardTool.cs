using CBRE.Common.Mediator;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Localization;
using CBRE.Settings;
using CBRE.UI;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.Editor.Tools.VMTool
{
    public class StandardTool : VMSubTool
    {
        private enum VMState
        {
            None,
            Down,
            Moving
        }

        private VMState _state;

        public StandardTool(VMTool mainTool) : base(mainTool)
        {
            StandardControl sc = new StandardControl();
            sc.Merge += Merge;
            sc.Split += Split;
            Control = sc;
        }

        private bool AutomaticallyMerge()
        {
            return ((StandardControl)Control).AutomaticallyMerge;
        }

        private void UpdateSplitEnabled()
        {
            ((StandardControl)Control).SplitEnabled = CanSplit();
        }

        private bool CanSplit()
        {
            return GetSplitFace() != null;
        }

        private Face GetSplitFace()
        {
            if (MainTool.Points == null) return null;

            List<VMPoint> selected = MainTool.Points.Where(x => x.IsSelected).ToList();

            // Must have two points selected
            if (selected.Count != 2) return null;

            // Selection must share a face
            List<Face> commonFace = selected[0].GetAdjacentFaces().Intersect(selected[1].GetAdjacentFaces()).ToList();
            if (commonFace.Count != 1) return null;

            Face face = commonFace[0];
            Coordinate s = selected[0].Coordinate;
            Coordinate e = selected[1].Coordinate;
            IEnumerable<Line> edges = face.GetEdges();

            // The points cannot be adjacent
            return edges.Any(x => (x.Start == s && x.End == e) || (x.Start == e && x.End == s))
                       ? null
                       : face;
        }

        private void Merge(object sender)
        {
            CheckMergedVertices();
        }

        private void Split(object sender)
        {
            Face face = GetSplitFace();
            if (face == null) return;

            Solid solid = face.Parent;

            List<VMPoint> sel = MainTool.Points.Where(x => x.IsSelected).ToList();
            VMPoint p1 = sel[0];
            VMPoint p2 = sel[1];

            if (p1.IsMidPoint) AddAdjacentPoint(face, p1);
            if (p2.IsMidPoint) AddAdjacentPoint(face, p2);

            Polygon polygon = new Polygon(face.Vertices.Select(x => x.Location));
            Plane clip = new Plane(p1.Coordinate, p2.Coordinate, p1.Coordinate + face.Plane.Normal * 10);
            Polygon back, front;
            polygon.Split(clip, out back, out front);
            if (back == null || front == null) return;

            solid.Faces.Remove(face);
            face.Parent = null;

            CreateFace(back, solid, face);
            CreateFace(front, solid, face);

            solid.UpdateBoundingBox();

            MainTool.SetDirty(true, true);
        }

        private void VMSplitFace()
        {
            if (CanSplit())
            {
                Split(null);
            }
        }

        private void CreateFace(Polygon polygon, Solid parent, Face original)
        {
            List<Coordinate> verts = polygon.Vertices;
            Face f = new Face(Document.Map.IDGenerator.GetNextFaceID())
            {
                Parent = parent,
                Plane = new Plane(verts[0], verts[1], verts[2]),
                Colour = parent.Colour,
                Texture = original.Texture.Clone()
            };
            f.Vertices.AddRange(verts.Select(x => new Vertex(x, f)));
            f.UpdateBoundingBox();
            f.CalculateTextureCoordinates(true);
            parent.Faces.Add(f);
        }

        private void AddAdjacentPoint(Face face, VMPoint point)
        {
            Solid solid = face.Parent;
            Coordinate s = point.MidpointStart.Coordinate;
            Coordinate e = point.MidpointEnd.Coordinate;

            foreach (Face f in solid.Faces.Where(x => x != face))
            {
                foreach (Line edge in f.GetEdges())
                {
                    if (edge.Start == s && edge.End == e)
                    {
                        int idx = f.Vertices.FindIndex(x => x.Location == e);
                        f.Vertices.Insert(idx, new Vertex(point.Coordinate, f));
                        return;
                    }
                    if (edge.Start == e && edge.End == s)
                    {
                        int idx = f.Vertices.FindIndex(x => x.Location == s);
                        f.Vertices.Insert(idx, new Vertex(point.Coordinate, f));
                        return;
                    }
                }
            }
        }

        public override string GetName()
        {
            return Local.LocalString("tool.standard");
        }

        public override string GetContextualHelp()
        {
            return Local.LocalString("tool.standard.help");
        }

        public override void ToolSelected(bool preventHistory)
        {
            _state = VMState.None;
            UpdateSplitEnabled();
            Mediator.Subscribe(HotkeysMediator.VMSplitFace, this);
        }

        public override void ToolDeselected(bool preventHistory)
        {
            _state = VMState.None;
            Mediator.UnsubscribeAll(this);
        }

        public override List<VMPoint> GetVerticesAtPoint(int x, int y, Viewport2D viewport)
        {
            return MainTool.GetVerticesAtPoint(x, y, viewport);
        }

        public override List<VMPoint> GetVerticesAtPoint(int x, int y, Viewport3D viewport)
        {
            return MainTool.GetVerticesAtPoint(x, y, viewport);
        }

        public override void DragStart(List<VMPoint> clickedPoints)
        {
            _state = VMState.Down;
            Editor.Instance.CaptureAltPresses = true;
        }

        public override void DragMove(Coordinate distance)
        {
            _state = VMState.Moving;
            // Move each selected point by the delta value
            foreach (VMPoint p in MainTool.GetSelectedPoints())
            {
                p.Move(distance);
            }

            //MainTool.SetDirty(false, false);
        }

        public override void DragEnd()
        {
            if (_state == VMState.Moving)
            {
                if (AutomaticallyMerge()) CheckMergedVertices();
                else if (CanMerge() && ConfirmMerge()) CheckMergedVertices();
                else MainTool.SetDirty(true, true);
            }
            _state = VMState.None;
            Editor.Instance.CaptureAltPresses = false;
        }

        public override void MouseClick(ViewportBase viewport, ViewportEvent e)
        {

        }

        public override void MouseDoubleClick(ViewportBase viewport, ViewportEvent e)
        {
            // Not used
        }

        private bool CanMerge()
        {
            foreach (Solid solid in MainTool.GetCopies())
            {
                foreach (Face face in solid.Faces)
                {
                    for (int i = 0; i < face.Vertices.Count; i++)
                    {
                        int j = (i + 1) % face.Vertices.Count;
                        Vertex v1 = face.Vertices[i];
                        Vertex v2 = face.Vertices[j];

                        if (!v1.Location.EquivalentTo(v2.Location, 0.01m)) continue;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ConfirmMerge()
        {
            return MessageBox.Show(Local.LocalString("tool.merge_vertices"), Local.LocalString("tool.overlap_vertices_detected"), MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        private void CheckMergedVertices()
        {
            int mergedVertices = 0;
            int removedFaces = 0;
            foreach (Solid solid in MainTool.GetCopies())
            {
                foreach (Face face in solid.Faces)
                {
                    // Remove adjacent duplicates
                    for (int i = 0; i < face.Vertices.Count; i++)
                    {
                        // Loop through to the start to cater for when the first & last vertices are equal
                        int j = (i + 1) % face.Vertices.Count;
                        Vertex v1 = face.Vertices[i];
                        Vertex v2 = face.Vertices[j];

                        if (!v1.Location.EquivalentTo(v2.Location, 0.01m)) continue;

                        // Two adjacent vertices are equivalent, remove the latter...
                        face.Vertices.RemoveAt(j);
                        mergedVertices++;

                        // Check i again with its new neighbour
                        i--;
                    }
                }

                // Remove empty faces from the solid
                removedFaces += solid.Faces.RemoveAll(x => x.Vertices.Count < 3);
            }

            ((StandardControl)Control).ShowMergeResult(mergedVertices, removedFaces);
            MainTool.SetDirty(true, true);
        }

        public override void MouseEnter(ViewportBase viewport, ViewportEvent e)
        {

        }

        public override void MouseLeave(ViewportBase viewport, ViewportEvent e)
        {

        }

        public override void MouseDown(ViewportBase viewport, ViewportEvent e)
        {

        }

        public override void MouseUp(ViewportBase viewport, ViewportEvent e)
        {

        }

        public override void MouseWheel(ViewportBase viewport, ViewportEvent e)
        {

        }

        public override void MouseMove(ViewportBase viewport, ViewportEvent e)
        {

        }

        public override void KeyPress(ViewportBase viewport, ViewportEvent e)
        {

        }

        public override void KeyDown(ViewportBase viewport, ViewportEvent e)
        {
            Coordinate nudge = GetNudgeValue(e.KeyCode);
            Viewport2D vp = viewport as Viewport2D;
            List<VMPoint> sel = MainTool.GetSelectedPoints();
            if (nudge != null && vp != null && _state == VMState.None && sel.Any())
            {
                Coordinate translate = vp.Expand(nudge);
                foreach (VMPoint p in sel)
                {
                    p.Move(translate);
                }
                CheckMergedVertices();
            }
        }

        public override void KeyUp(ViewportBase viewport, ViewportEvent e)
        {

        }

        public override void UpdateFrame(ViewportBase viewport, FrameInfo frame)
        {

        }

        public override void Render(ViewportBase viewport)
        {

        }

        public override void Render2D(Viewport2D viewport)
        {

        }

        public override void Render3D(Viewport3D viewport)
        {

        }

        public override void SelectionChanged()
        {
            UpdateSplitEnabled();
        }

        public override bool ShouldDeselect(List<VMPoint> vtxs)
        {
            return true;
        }

        public override bool NoSelection()
        {
            return false;
        }

        public override bool No3DSelection()
        {
            return false;
        }

        public override bool DrawVertices()
        {
            return true;
        }
    }
}
