using CBRE.Editor.Documents;
using CBRE.Editor.Tools.Widgets;
using CBRE.Extensions;
using CBRE.Localization;
using CBRE.Settings;
using CBRE.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.Editor.Tools.SelectTool.TransformationTools
{
    /// <summary>
    /// Allows the selected objects to be rotated
    /// </summary>
    class RotateTool : TransformationTool
    {
        public override bool RenderCircleHandles
        {
            get { return true; }
        }

        public override bool FilterHandle(BaseBoxTool.ResizeHandle handle)
        {
            return handle == BaseBoxTool.ResizeHandle.BottomLeft
                   || handle == BaseBoxTool.ResizeHandle.BottomRight
                   || handle == BaseBoxTool.ResizeHandle.TopLeft
                   || handle == BaseBoxTool.ResizeHandle.TopRight;
        }

        public override string GetTransformName()
        {
            return Local.LocalString("tool.rotate");
        }

        public override Cursor CursorForHandle(BaseBoxTool.ResizeHandle handle)
        {
            return CBRECursors.RotateCursor;
        }

        #region 2D Transformation Matrix
        public override Matrix4? GetTransformationMatrix(Viewport2D viewport, ViewportEvent e, BaseBoxTool.BoxState state, Document doc, IEnumerable<Widget> activeWidgets)
        {
            DataStructures.Geometric.Coordinate origin = viewport.ZeroUnusedCoordinate((state.PreTransformBoxStart + state.PreTransformBoxEnd) / 2);
            RotationWidget rw = activeWidgets.OfType<RotationWidget>().FirstOrDefault();
            if (rw != null) origin = rw.GetPivotPoint();

            DataStructures.Geometric.Coordinate forigin = viewport.Flatten(origin);

            DataStructures.Geometric.Coordinate origv = (state.MoveStart - forigin).Normalise();
            DataStructures.Geometric.Coordinate newv = (viewport.ScreenToWorld(e.X, viewport.Height - e.Y) - forigin).Normalise();

            decimal angle = DMath.Acos(Math.Max(-1, Math.Min(1, origv.Dot(newv))));
            if ((origv.Cross(newv).Z < 0)) angle = 2 * DMath.PI - angle;

            bool shf = KeyboardState.Shift;
            RotationStyle def = Select.RotationStyle;
            bool snap = (def == RotationStyle.SnapOnShift && shf) || (def == RotationStyle.SnapOffShift && !shf);
            if (snap)
            {
                decimal deg = angle * (180 / DMath.PI);
                decimal rnd = Math.Round(deg / 15) * 15;
                angle = rnd * (DMath.PI / 180);
            }

            Matrix4 rotm;
            if (viewport.Direction == Viewport2D.ViewDirection.Top) rotm = Matrix4.CreateRotationZ((float)angle);
            else if (viewport.Direction == Viewport2D.ViewDirection.Front) rotm = Matrix4.CreateRotationX((float)angle);
            else rotm = Matrix4.CreateRotationY((float)-angle); // The Y axis rotation goes in the reverse direction for whatever reason

            Matrix4 mov = Matrix4.CreateTranslation((float)-origin.X, (float)-origin.Y, (float)-origin.Z);
            Matrix4 rot = Matrix4.Mult(mov, rotm);
            return Matrix4.Mult(rot, Matrix4.Invert(mov));
        }
        #endregion 2D Transformation Matrix

        public override IEnumerable<Widget> GetWidgets(Document document)
        {
            yield return new RotationWidget(document);
        }
    }
}