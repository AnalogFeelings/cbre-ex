using CBRE.DataStructures.Geometric;
using CBRE.Editor.Documents;
using CBRE.Editor.Tools.Widgets;
using CBRE.Localization;
using CBRE.UI;
using OpenTK;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CBRE.Editor.Tools.SelectTool.TransformationTools
{
    /// <summary>
    /// Allows the selected objects to be skewed
    /// </summary>
    class SkewTool : TransformationTool
    {
        public override bool RenderCircleHandles
        {
            get { return false; }
        }

        public override bool FilterHandle(BaseBoxTool.ResizeHandle handle)
        {
            return handle == BaseBoxTool.ResizeHandle.Bottom
                   || handle == BaseBoxTool.ResizeHandle.Left
                   || handle == BaseBoxTool.ResizeHandle.Top
                   || handle == BaseBoxTool.ResizeHandle.Right;
        }

        public override string GetTransformName()
        {
            return Local.LocalString("tool.skew");
        }

        public override Cursor CursorForHandle(BaseBoxTool.ResizeHandle handle)
        {
            return (handle == BaseBoxTool.ResizeHandle.Top || handle == BaseBoxTool.ResizeHandle.Bottom)
                       ? Cursors.SizeWE
                       : Cursors.SizeNS;
        }

        #region 2D Transformation Matrix
        public override Matrix4? GetTransformationMatrix(Viewport2D viewport, ViewportEvent e, BaseBoxTool.BoxState state, Document doc, IEnumerable<Widget> activeWidgets)
        {
            bool shearUpDown = state.Handle == BaseBoxTool.ResizeHandle.Left || state.Handle == BaseBoxTool.ResizeHandle.Right;
            bool shearTopRight = state.Handle == BaseBoxTool.ResizeHandle.Top || state.Handle == BaseBoxTool.ResizeHandle.Right;

            Coordinate nsmd = viewport.ScreenToWorld(e.X, viewport.Height - e.Y) - state.MoveStart;
            Coordinate mouseDiff = SnapIfNeeded(nsmd, doc);
            if (KeyboardState.Shift)
            {
                mouseDiff = doc.Snap(nsmd, doc.Map.GridSpacing / 2);
            }

            Coordinate relative = viewport.Flatten(state.PreTransformBoxEnd - state.PreTransformBoxStart);
            Coordinate shearOrigin = (shearTopRight) ? state.PreTransformBoxStart : state.PreTransformBoxEnd;

            Coordinate shearAmount = new Coordinate(mouseDiff.X / relative.Y, mouseDiff.Y / relative.X, 0);
            if (!shearTopRight) shearAmount *= -1;

            Matrix4 shearMatrix = Matrix4.Identity;
            float sax = (float)shearAmount.X;
            float say = (float)shearAmount.Y;

            switch (viewport.Direction)
            {
                case Viewport2D.ViewDirection.Top:
                    if (shearUpDown) shearMatrix.M12 = say;
                    else shearMatrix.M21 = sax;
                    break;
                case Viewport2D.ViewDirection.Front:
                    if (shearUpDown) shearMatrix.M23 = say;
                    else shearMatrix.M32 = sax;
                    break;
                case Viewport2D.ViewDirection.Side:
                    if (shearUpDown) shearMatrix.M13 = say;
                    else shearMatrix.M31 = sax;
                    break;
            }


            Matrix4 stran = Matrix4.CreateTranslation((float)-shearOrigin.X, (float)-shearOrigin.Y, (float)-shearOrigin.Z);
            Matrix4 shear = Matrix4.Mult(stran, shearMatrix);
            return Matrix4.Mult(shear, Matrix4.Invert(stran));
        }
        #endregion 2D Transformation Matrix

        public override IEnumerable<Widget> GetWidgets(Document document)
        {
            yield break;
        }
    }
}
