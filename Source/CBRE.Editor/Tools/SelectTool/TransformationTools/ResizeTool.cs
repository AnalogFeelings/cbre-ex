using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Documents;
using CBRE.Editor.Tools.Widgets;
using CBRE.Localization;
using CBRE.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.Editor.Tools.SelectTool.TransformationTools
{
    /// <summary>
    /// Allows the selected objects to be scaled and translated
    /// </summary>
    class ResizeTool : TransformationTool
    {

        public override bool RenderCircleHandles
        {
            get { return false; }
        }

        public override bool FilterHandle(BaseBoxTool.ResizeHandle handle)
        {
            return true;
        }

        public override string GetTransformName()
        {
            return Local.LocalString("tool.resize");
        }

        public override Cursor CursorForHandle(BaseBoxTool.ResizeHandle handle)
        {
            return null;
        }

        #region 2D Transformation Matrix
        public override Matrix4? GetTransformationMatrix(Viewport2D viewport, ViewportEvent e, BaseBoxTool.BoxState state, Document doc, IEnumerable<Widget> activeWidgets)
        {
            Tuple<Coordinate, Coordinate> coords = GetBoxCoordinatesForSelectionResize(viewport, e, state, doc);
            state.BoxStart = coords.Item1;
            state.BoxEnd = coords.Item2;
            Matrix4 resizeMatrix;
            if (state.Handle == BaseBoxTool.ResizeHandle.Center)
            {
                Coordinate movement = state.BoxStart - state.PreTransformBoxStart;
                resizeMatrix = Matrix4.CreateTranslation((float)movement.X, (float)movement.Y, (float)movement.Z);
            }
            else
            {
                Coordinate resize = (state.PreTransformBoxStart - state.BoxStart) +
                             (state.BoxEnd - state.PreTransformBoxEnd);
                resize = resize.ComponentDivide(state.PreTransformBoxEnd - state.PreTransformBoxStart);
                resize += new Coordinate(1, 1, 1);
                Coordinate offset = -GetOriginForTransform(viewport, state);
                Matrix4 trans = Matrix4.CreateTranslation((float)offset.X, (float)offset.Y, (float)offset.Z);
                Matrix4 scale = Matrix4.Mult(trans, Matrix4.CreateScale((float)resize.X, (float)resize.Y, (float)resize.Z));
                resizeMatrix = Matrix4.Mult(scale, Matrix4.Invert(trans));
            }
            return resizeMatrix;
        }

        private Coordinate GetResizeOrigin(Viewport2D viewport, BaseBoxTool.BoxState state, Document document)
        {
            if (state.Action != BaseBoxTool.BoxAction.Resizing || state.Handle != BaseBoxTool.ResizeHandle.Center) return null;
            List<MapObject> sel = document.Selection.GetSelectedParents().ToList();
            if (sel.Count == 1 && sel[0] is Entity && !sel[0].HasChildren)
            {
                return viewport.Flatten(((Entity)sel[0]).Origin);
            }
            Coordinate st = viewport.Flatten(state.PreTransformBoxStart);
            Coordinate ed = viewport.Flatten(state.PreTransformBoxEnd);
            Coordinate[] points = new[] { st, ed, new Coordinate(st.X, ed.Y, 0), new Coordinate(ed.X, st.Y, 0) };
            return points.OrderBy(x => (state.MoveStart - x).LengthSquared()).First();
        }

        private Coordinate GetResizeDistance(Viewport2D viewport, ViewportEvent e, BaseBoxTool.BoxState state, Document document)
        {
            Coordinate origin = GetResizeOrigin(viewport, state, document);
            if (origin == null) return null;
            Coordinate before = state.MoveStart;
            Coordinate after = viewport.ScreenToWorld(e.X, viewport.Height - e.Y);
            return SnapIfNeeded(origin + after - before, document) - origin;
        }

        private Tuple<Coordinate, Coordinate> GetBoxCoordinatesForSelectionResize(Viewport2D viewport, ViewportEvent e, BaseBoxTool.BoxState state, Document document)
        {
            if (state.Action != BaseBoxTool.BoxAction.Resizing) return Tuple.Create(state.BoxStart, state.BoxEnd);
            Coordinate now = SnapIfNeeded(viewport.ScreenToWorld(e.X, viewport.Height - e.Y), document);
            Coordinate cstart = viewport.Flatten(state.BoxStart);
            Coordinate cend = viewport.Flatten(state.BoxEnd);

            // Proportional scaling
            Coordinate ostart = viewport.Flatten(state.PreTransformBoxStart ?? Coordinate.Zero);
            Coordinate oend = viewport.Flatten(state.PreTransformBoxEnd ?? Coordinate.Zero);
            decimal owidth = oend.X - ostart.X;
            decimal oheight = oend.Y - ostart.Y;
            bool proportional = KeyboardState.Ctrl && state.Action == BaseBoxTool.BoxAction.Resizing && owidth != 0 && oheight != 0;

            switch (state.Handle)
            {
                case BaseBoxTool.ResizeHandle.TopLeft:
                    cstart.X = Math.Min(now.X, cend.X - 1);
                    cend.Y = Math.Max(now.Y, cstart.Y + 1);
                    break;
                case BaseBoxTool.ResizeHandle.Top:
                    cend.Y = Math.Max(now.Y, cstart.Y + 1);
                    break;
                case BaseBoxTool.ResizeHandle.TopRight:
                    cend.X = Math.Max(now.X, cstart.X + 1);
                    cend.Y = Math.Max(now.Y, cstart.Y + 1);
                    break;
                case BaseBoxTool.ResizeHandle.Left:
                    cstart.X = Math.Min(now.X, cend.X - 1);
                    break;
                case BaseBoxTool.ResizeHandle.Center:
                    Coordinate cdiff = cend - cstart;

                    Coordinate distance = GetResizeDistance(viewport, e, state, document);
                    if (distance == null) cstart = viewport.Flatten(state.PreTransformBoxStart) + now - SnapIfNeeded(state.MoveStart, document);
                    else cstart = viewport.Flatten(state.PreTransformBoxStart) + distance;
                    cend = cstart + cdiff;
                    break;
                case BaseBoxTool.ResizeHandle.Right:
                    cend.X = Math.Max(now.X, cstart.X + 1);
                    break;
                case BaseBoxTool.ResizeHandle.BottomLeft:
                    cstart.X = Math.Min(now.X, cend.X - 1);
                    cstart.Y = Math.Min(now.Y, cend.Y - 1);
                    break;
                case BaseBoxTool.ResizeHandle.Bottom:
                    cstart.Y = Math.Min(now.Y, cend.Y - 1);
                    break;
                case BaseBoxTool.ResizeHandle.BottomRight:
                    cend.X = Math.Max(now.X, cstart.X + 1);
                    cstart.Y = Math.Min(now.Y, cend.Y - 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (proportional)
            {
                decimal nwidth = cend.X - cstart.X;
                decimal nheight = cend.Y - cstart.Y;
                decimal mult = Math.Max(nwidth / owidth, nheight / oheight);
                decimal pwidth = owidth * mult;
                decimal pheight = oheight * mult;
                decimal wdiff = pwidth - nwidth;
                decimal hdiff = pheight - nheight;
                switch (state.Handle)
                {
                    case BaseBoxTool.ResizeHandle.TopLeft:
                        cstart.X -= wdiff;
                        cend.Y += hdiff;
                        break;
                    case BaseBoxTool.ResizeHandle.TopRight:
                        cend.X += wdiff;
                        cend.Y += hdiff;
                        break;
                    case BaseBoxTool.ResizeHandle.BottomLeft:
                        cstart.X -= wdiff;
                        cstart.Y -= hdiff;
                        break;
                    case BaseBoxTool.ResizeHandle.BottomRight:
                        cend.X += wdiff;
                        cstart.Y -= hdiff;
                        break;
                }
            }

            cstart = viewport.Expand(cstart) + viewport.GetUnusedCoordinate(state.BoxStart);
            cend = viewport.Expand(cend) + viewport.GetUnusedCoordinate(state.BoxEnd);
            return Tuple.Create(cstart, cend);
        }

        private static Coordinate GetOriginForTransform(Viewport2D viewport, BaseBoxTool.BoxState state)
        {
            decimal x = 0;
            decimal y = 0;
            Coordinate cstart = viewport.Flatten(state.PreTransformBoxStart);
            Coordinate cend = viewport.Flatten(state.PreTransformBoxEnd);
            switch (state.Handle)
            {
                case BaseBoxTool.ResizeHandle.TopLeft:
                case BaseBoxTool.ResizeHandle.Top:
                case BaseBoxTool.ResizeHandle.TopRight:
                case BaseBoxTool.ResizeHandle.Left:
                case BaseBoxTool.ResizeHandle.Right:
                    y = cstart.Y;
                    break;
                case BaseBoxTool.ResizeHandle.BottomLeft:
                case BaseBoxTool.ResizeHandle.Bottom:
                case BaseBoxTool.ResizeHandle.BottomRight:
                    y = cend.Y;
                    break;
            }
            switch (state.Handle)
            {
                case BaseBoxTool.ResizeHandle.Top:
                case BaseBoxTool.ResizeHandle.TopRight:
                case BaseBoxTool.ResizeHandle.Right:
                case BaseBoxTool.ResizeHandle.BottomRight:
                case BaseBoxTool.ResizeHandle.Bottom:
                    x = cstart.X;
                    break;
                case BaseBoxTool.ResizeHandle.TopLeft:
                case BaseBoxTool.ResizeHandle.Left:
                case BaseBoxTool.ResizeHandle.BottomLeft:
                    x = cend.X;
                    break;
            }
            return viewport.Expand(new Coordinate(x, y, 0));
        }
        #endregion 2D Transformation Matrix

        public override IEnumerable<Widget> GetWidgets(Document document)
        {
            yield break;
        }
    }
}
