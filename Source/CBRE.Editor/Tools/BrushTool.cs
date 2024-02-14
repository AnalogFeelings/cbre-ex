using CBRE.Common.Mediator;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Editor.Actions.MapObjects.Selection;
using CBRE.Editor.Brushes;
using CBRE.Editor.Properties;
using CBRE.Editor.Rendering.Immediate;
using CBRE.Editor.UI;
using CBRE.Graphics.Helpers;
using CBRE.Localization;
using CBRE.Settings;
using CBRE.UI;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Select = CBRE.Settings.Select;

namespace CBRE.Editor.Tools
{
    public class BrushTool : BaseBoxTool
    {
        private Box _lastBox;
        private bool _updatePreview;
        private List<Face> _preview;

        public override Image GetIcon()
        {
            return Resources.Tool_Brush;
        }

        public override string GetName()
        {
            return Local.LocalString("tool.brush");
        }

        public override HotkeyTool? GetHotkeyToolType()
        {
            return HotkeyTool.Brush;
        }

        public override IEnumerable<KeyValuePair<string, Control>> GetSidebarControls()
        {
            yield return new KeyValuePair<string, Control>(GetName(), BrushManager.SidebarControl);
        }

        public override string GetContextualHelp()
        {
            return Local.LocalString("tool.brush.help");
        }

        protected override Color BoxColour
        {
            get { return Color.Turquoise; }
        }

        protected override Color FillColour
        {
            get { return Color.FromArgb(CBRE.Settings.View.SelectionBoxBackgroundOpacity, Color.Green); }
        }

        public override void ToolSelected(bool preventHistory)
        {
            BrushManager.ValuesChanged += ValuesChanged;
            List<Solid> sel = Document.Selection.GetSelectedObjects().OfType<Solid>().ToList();
            if (sel.Any())
            {
                _lastBox = new Box(sel.Select(x => x.BoundingBox));
            }
            else if (_lastBox == null)
            {
                decimal gs = Document.Map.GridSpacing;
                _lastBox = new Box(Coordinate.Zero, new Coordinate(gs, gs, gs));
            }
            _updatePreview = true;
        }

        public override void ToolDeselected(bool preventHistory)
        {
            BrushManager.ValuesChanged -= ValuesChanged;
            _updatePreview = false;
        }

        private void ValuesChanged(IBrush brush)
        {
            if (BrushManager.CurrentBrush == brush) _updatePreview = true;
        }

        protected override void OnBoxChanged()
        {
            _updatePreview = true;
            base.OnBoxChanged();
        }

        protected override void LeftMouseDownToDraw(Viewport2D viewport, ViewportEvent e)
        {
            base.LeftMouseDownToDraw(viewport, e);
            if (_lastBox == null) return;
            State.BoxStart += viewport.GetUnusedCoordinate(_lastBox.Start);
            State.BoxEnd += viewport.GetUnusedCoordinate(_lastBox.End);
            _updatePreview = true;
        }

        private void CreateBrush(Box bounds)
        {
            MapObject brush = GetBrush(bounds, Document.Map.IDGenerator);
            if (brush == null) return;

            brush.IsSelected = Select.SelectCreatedBrush;
            IAction action = new Create(Document.Map.WorldSpawn.ID, brush);
            if (Select.SelectCreatedBrush && Select.DeselectOthersWhenSelectingCreation)
            {
                action = new ActionCollection(new ChangeSelection(new MapObject[0], Document.Selection.GetSelectedObjects()), action);
            }

            Document.PerformAction(Local.LocalString("tool.brush.create", BrushManager.CurrentBrush.Name.ToLower()), action);
        }

        private MapObject GetBrush(Box bounds, IDGenerator idg)
        {
            Box _bounds = new Box(bounds.Start, bounds.End);
            if ((_bounds.Start - _bounds.End).VectorMagnitude() > 1000000m)
            {
                _bounds = new Box(bounds.Start, ((bounds.End - bounds.Start).Normalise() * 1000000m) + bounds.Start);
            }
            IBrush brush = BrushManager.CurrentBrush;
            Providers.Texture.TextureItem ti = Document.TextureCollection.SelectedTexture;
            Common.ITexture texture = ti != null ? ti.GetTexture() : null;
            List<MapObject> created = brush.Create(idg, bounds, texture, BrushManager.RoundCreatedVertices ? 0 : 2).ToList();
            if (created.Count > 1)
            {
                Group g = new Group(idg.GetNextObjectID());
                created.ForEach(x => x.SetParent(g));
                g.UpdateBoundingBox();
                return g;
            }
            return created.FirstOrDefault();
        }

        public override void BoxDrawnConfirm(ViewportBase viewport)
        {
            Box box = new Box(State.BoxStart, State.BoxEnd);
            if (box.Start.X != box.End.X && box.Start.Y != box.End.Y && box.Start.Z != box.End.Z)
            {
                CreateBrush(box);
                _lastBox = box;
            }
            _preview = null;
            base.BoxDrawnConfirm(viewport);
            if (Select.SwitchToSelectAfterCreation)
            {
                Mediator.Publish(HotkeysMediator.SwitchTool, HotkeyTool.Selection);
            }
            if (Select.ResetBrushTypeOnCreation)
            {
                Mediator.Publish(EditorMediator.ResetSelectedBrushType);
            }
        }

        public override void BoxDrawnCancel(ViewportBase viewport)
        {
            _lastBox = new Box(State.BoxStart, State.BoxEnd);
            _preview = null;
            base.BoxDrawnCancel(viewport);
        }

        public override void UpdateFrame(ViewportBase viewport, FrameInfo frame)
        {
            if (_updatePreview && ShouldDrawBox(viewport))
            {
                Box box = new Box(State.BoxStart, State.BoxEnd);
                MapObject brush = GetBrush(box, new IDGenerator());
                _preview = new List<Face>();
                CollectFaces(_preview, new[] { brush });
                Color color = GetRenderBoxColour();
                _preview.ForEach(x => { x.Colour = color; });
            }
            _updatePreview = false;
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

        public override void OverrideViewportContextMenu(ViewportContextMenu menu, Viewport2D vp, ViewportEvent e)
        {
            menu.Items.Clear();
            if (State.Handle == ResizeHandle.Center)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(Local.LocalString("menu.tools.create_object"));
                item.Click += (sender, args) => BoxDrawnConfirm(vp);
                menu.Items.Add(item);
            }
        }

        private Color GetRenderColour()
        {
            Color col = GetRenderBoxColour();
            return Color.FromArgb(128, col);
        }

        protected override void Render2D(Viewport2D viewport)
        {
            base.Render2D(viewport);
            if (ShouldDrawBox(viewport) && _preview != null)
            {
                GL.Color3(GetRenderColour());
                Graphics.Helpers.Matrix.Push();
                OpenTK.Matrix4 matrix = viewport.GetModelViewMatrix();
                GL.MultMatrix(ref matrix);
                MapObjectRenderer.DrawWireframe(_preview, true, false);
                Graphics.Helpers.Matrix.Pop();
            }
        }

        protected override void Render3D(Viewport3D viewport)
        {
            base.Render3D(viewport);
            if (ShouldDraw3DBox() && _preview != null)
            {
                GL.Disable(EnableCap.CullFace);
                TextureHelper.Unbind();
                if (viewport.Type != Viewport3D.ViewType.Flat) MapObjectRenderer.EnableLighting();
                MapObjectRenderer.DrawFilled(_preview, GetRenderColour(), false);
                MapObjectRenderer.DisableLighting();
                GL.Color4(Color.GreenYellow);
                MapObjectRenderer.DrawWireframe(_preview, true, false);
            }
        }

        private static void CollectFaces(List<Face> faces, IEnumerable<MapObject> list)
        {
            foreach (MapObject mo in list)
            {
                if (mo is Solid)
                {
                    faces.AddRange(((Solid)mo).Faces);
                }
                else if (mo is Entity || mo is Group)
                {
                    CollectFaces(faces, mo.GetChildren());
                }
            }
        }
    }
}
