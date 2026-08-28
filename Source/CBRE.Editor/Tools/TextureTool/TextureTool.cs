using CBRE.Common;
using CBRE.Common.Mediator;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Editor.Actions.MapObjects.Selection;
using CBRE.Editor.Documents;
using CBRE.Editor.History;
using CBRE.Editor.Properties;
using CBRE.Graphics.Helpers;
using CBRE.Localization;
using CBRE.Providers.Texture;
using CBRE.Settings;
using CBRE.UI;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.Editor.Tools.TextureTool
{
    public class TextureTool : BaseTool
    {

        #region Enums

        public enum SelectBehaviour
        {
            LiftSelect,
            Lift,
            Select,
            Apply,
            ApplyWithValues,
            AlignToView
        }

        public enum JustifyMode
        {
            Fit,
            Left,
            Right,
            Center,
            Top,
            Bottom
        }

        public enum AlignMode
        {
            Face,
            World
        }

        #endregion

        private readonly TextureApplicationForm _form;
        private readonly TextureToolSidebarPanel _sidebarPanel;

        public TextureTool()
        {
            Usage = ToolUsage.View3D;
            _form = new TextureApplicationForm();
            _form.PropertyChanged += TexturePropertyChanged;
            _form.TextureAlign += TextureAligned;
            _form.TextureApply += TextureApplied;
            _form.TextureJustify += TextureJustified;
            _form.HideMaskToggled += HideMaskToggled;
            _form.TextureChanged += TextureChanged;

            _sidebarPanel = new TextureToolSidebarPanel();
            _sidebarPanel.TileFit += TileFit;
            _sidebarPanel.RandomiseXShiftValues += RandomiseXShiftValues;
            _sidebarPanel.RandomiseYShiftValues += RandomiseYShiftValues;
        }

        public override void DocumentChanged()
        {
            _form.Document = Document;
        }

        private void HideMaskToggled(object sender, bool hide)
        {
            Document.Map.HideFaceMask = !hide;
            Mediator.Publish(HotkeysMediator.ToggleHideFaceMask);
        }

        private void RandomiseXShiftValues(object sender, int min, int max)
        {
            if (Document.Selection.IsEmpty()) return;

            Random rand = new Random();
            Action<Document, Face> action = (d, f) =>
            {
                f.Texture.XShift = rand.Next(min, max + 1); // Upper bound is exclusive
                f.CalculateTextureCoordinates(true);
            };
            Document.PerformAction(Local.LocalString("tool.random_x_shift"), new EditFace(Document.Selection.GetSelectedFaces(), action, false));
        }

        private void RandomiseYShiftValues(object sender, int min, int max)
        {
            if (Document.Selection.IsEmpty()) return;

            Random rand = new Random();
            Action<Document, Face> action = (d, f) =>
            {
                f.Texture.YShift = rand.Next(min, max + 1); // Upper bound is exclusive
                f.CalculateTextureCoordinates(true);
            };
            Document.PerformAction(Local.LocalString("tool.random_y_shift"), new EditFace(Document.Selection.GetSelectedFaces(), action, false));
        }

        private void TextureJustified(object sender, JustifyMode justifymode, bool treatasone)
        {
            TextureJustified(justifymode, treatasone, 1, 1);
        }

        private void TileFit(object sender, int tileX, int tileY)
        {
            TextureJustified(JustifyMode.Fit, _form.ShouldTreatAsOne(), tileX, tileY);
        }

        private void TextureJustified(JustifyMode justifymode, bool treatasone, int tileX, int tileY)
        {
            if (Document.Selection.IsEmpty()) return;
            Face.BoxAlignMode boxAlignMode = (justifymode == JustifyMode.Fit)
                                   ? Face.BoxAlignMode.Center // Don't care about the align mode when centering
                                   : (Face.BoxAlignMode)Enum.Parse(typeof(Face.BoxAlignMode), justifymode.ToString());
            Cloud cloud = null;
            Action<Document, Face> action;
            if (treatasone)
            {
                // If we treat as one, it means we want to align to one great big cloud
                cloud = new Cloud(Document.Selection.GetSelectedFaces().SelectMany(x => x.Vertices).Select(x => x.Location));
            }

            if (justifymode == JustifyMode.Fit)
            {
                action = (d, x) => x.FitTextureToPointCloud(cloud ?? new Cloud(x.Vertices.Select(y => y.Location)), tileX, tileY);
            }
            else
            {
                action = (d, x) => x.AlignTextureWithPointCloud(cloud ?? new Cloud(x.Vertices.Select(y => y.Location)), boxAlignMode);
            }

            Document.PerformAction("Align texture", new EditFace(Document.Selection.GetSelectedFaces(), action, false));
        }

        private void TextureApplied(object sender, TextureItem texture)
        {
            ITexture ti = texture.GetTexture();
            Action<Document, Face> action = (document, face) =>
            {
                face.Texture.Name = texture.Name;
                face.Texture.Texture = ti;
                face.CalculateTextureCoordinates(false);
            };
            // When the texture changes, the entire list needs to be regenerated, can't do a partial update.
            Document.PerformAction(Local.LocalString("tool.apply_texture"), new EditFace(Document.Selection.GetSelectedFaces(), action, true));

            Mediator.Publish(EditorMediator.TextureSelected, texture);
        }

        private void TextureAligned(object sender, AlignMode align)
        {
            Action<Document, Face> action = (document, face) =>
            {
                if (align == AlignMode.Face) face.AlignTextureToFace();
                else if (align == AlignMode.World) face.AlignTextureToWorld();
                face.CalculateTextureCoordinates(false);
            };

            Document.PerformAction(Local.LocalString("tool.align_texture"), new EditFace(Document.Selection.GetSelectedFaces(), action, false));
        }

        private void TexturePropertyChanged(object sender, TextureApplicationForm.CurrentTextureProperties properties)
        {
            if (Document.Selection.IsEmpty()) return;

            Action<Document, Face> action = (document, face) =>
            {
                if (!properties.DifferentXScaleValues) face.Texture.XScale = properties.XScale;
                if (!properties.DifferentYScaleValues) face.Texture.YScale = properties.YScale;
                if (!properties.DifferentXShiftValues) face.Texture.XShift = properties.XShift;
                if (!properties.DifferentYShiftValues) face.Texture.YShift = properties.YShift;
                if (!properties.DifferentRotationValues) face.SetTextureRotation(properties.Rotation);
                face.CalculateTextureCoordinates(false);
            };

            Document.PerformAction(Local.LocalString("tool.modify_texture_properties"), new EditFace(Document.Selection.GetSelectedFaces(), action, false));
        }

        private void TextureChanged(object sender, TextureItem texture)
        {
            Mediator.Publish(EditorMediator.TextureSelected, texture);
        }

        public override Image GetIcon()
        {
            return Resources.Tool_Texture;
        }

        public override string GetName()
        {
            return Local.LocalString("tool.texture.tool");
        }

        public override HotkeyTool? GetHotkeyToolType()
        {
            return HotkeyTool.Texture;
        }

        public override string GetContextualHelp()
        {
            return Local.LocalString("tool.texture.help");
        }

        public override IEnumerable<KeyValuePair<string, Control>> GetSidebarControls()
        {
            yield return new KeyValuePair<string, Control>(Local.LocalString("tool.texture.power"), _sidebarPanel);
        }

        public override void ToolSelected(bool preventHistory)
        {
            _form.Show(Editor.Instance);
            Editor.Instance.Focus();

            if (!preventHistory)
            {
                Document.History.AddHistoryItem(new HistoryAction(Local.LocalString("tool.texture.switch_selection"), new ChangeToFaceSelectionMode(GetType(), Document.Selection.GetSelectedObjects())));
                IEnumerable<MapObject> currentSelection = Document.Selection.GetSelectedObjects();
                Document.Selection.SwitchToFaceSelection();
                IEnumerable<Solid> newSelection = Document.Selection.GetSelectedFaces().Select(x => x.Parent);
                Document.RenderSelection(currentSelection.Union(newSelection));
            }

            _form.SelectionChanged();

            Face selection = Document.Selection.GetSelectedFaces().OrderBy(x => x.Texture.Texture == null ? 1 : 0).FirstOrDefault();
            if (selection != null)
            {
                TextureItem itemToSelect = Document.TextureCollection.GetItem(selection.Texture.Name)
                                   ?? new TextureItem(null, selection.Texture.Name, TextureFlags.Missing, 64, 64);
                Mediator.Publish(EditorMediator.TextureSelected, itemToSelect);
            }
            _form.SelectTexture(Document.TextureCollection.SelectedTexture);

            Mediator.Subscribe(EditorMediator.TextureSelected, this);
            Mediator.Subscribe(EditorMediator.DocumentTreeFacesChanged, this);
            Mediator.Subscribe(EditorMediator.SelectionChanged, this);
        }

        public override void ToolDeselected(bool preventHistory)
        {
            List<Face> selected = Document.Selection.GetSelectedFaces().ToList();

            if (!preventHistory)
            {
                Document.History.AddHistoryItem(new HistoryAction(Local.LocalString("tool.texture.switch_selection"), new ChangeToObjectSelectionMode(GetType(), selected)));
                IEnumerable<Solid> currentSelection = Document.Selection.GetSelectedFaces().Select(x => x.Parent);
                Document.Selection.SwitchToObjectSelection();
                IEnumerable<MapObject> newSelection = Document.Selection.GetSelectedObjects();
                Document.RenderSelection(currentSelection.Union(newSelection));
            }

            _form.Clear();
            _form.Hide();
            Mediator.UnsubscribeAll(this);
        }

        private void TextureSelected(TextureItem texture)
        {
            _form.SelectTexture(texture);
        }

        private void SelectionChanged()
        {
            _form.SelectionChanged();
        }

        private void DocumentTreeFacesChanged()
        {
            _form.SelectionChanged();
        }

        public override void MouseDown(ViewportBase viewport, ViewportEvent e)
        {
            Viewport3D vp = viewport as Viewport3D;
            if (vp == null || (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right)) return;

            SelectBehaviour behaviour = e.Button == MouseButtons.Left
                                ? _form.GetLeftClickBehaviour(KeyboardState.Ctrl, KeyboardState.Shift, KeyboardState.Alt)
                                : _form.GetRightClickBehaviour(KeyboardState.Ctrl, KeyboardState.Shift, KeyboardState.Alt);

            Line ray = vp.CastRayFromScreen(e.X, e.Y);
            IEnumerable<Solid> hits = Document.Map.WorldSpawn.GetAllNodesIntersectingWith(ray).OfType<Solid>();
            Face clickedFace = null;

            //Didnt want a linq mess, so I did this instead. God forgive me.
            if (Document.Map.HideToolTextures)
            {
                clickedFace = hits.SelectMany(f => f.Faces)
                    .Select(x => new { Item = x, Intersection = x.GetIntersectionPoint(ray) })
                    .Where(x => x.Intersection != null && x.Item.Texture.IsToolTexture == false)
                    .OrderBy(x => (x.Intersection - ray.Start).VectorMagnitude())
                    .Select(x => x.Item)
                    .FirstOrDefault();
            }
            else
            {
                clickedFace = hits.SelectMany(f => f.Faces)
                    .Select(x => new { Item = x, Intersection = x.GetIntersectionPoint(ray) })
                    .Where(x => x.Intersection != null)
                    .OrderBy(x => (x.Intersection - ray.Start).VectorMagnitude())
                    .Select(x => x.Item)
                    .FirstOrDefault();
            }

            if (clickedFace == null) return;

            List<Face> faces = new List<Face>();
            if (KeyboardState.Shift) faces.AddRange(clickedFace.Parent.Faces);
            else faces.Add(clickedFace);

            Face firstSelected = Document.Selection.GetSelectedFaces().FirstOrDefault();
            Face firstClicked = faces.FirstOrDefault(face => !String.IsNullOrWhiteSpace(face.Texture.Name));

            ActionCollection ac = new ActionCollection();

            ChangeFaceSelection select = new ChangeFaceSelection(
                KeyboardState.Ctrl ? faces.Where(x => !x.IsSelected) : faces,
                KeyboardState.Ctrl ? faces.Where(x => x.IsSelected) : Document.Selection.GetSelectedFaces().Where(x => !faces.Contains(x)));

            Action lift = () =>
            {
                if (firstClicked == null) return;
                TextureItem itemToSelect = Document.TextureCollection.GetItem(firstClicked.Texture.Name)
                                   ?? new TextureItem(null, firstClicked.Texture.Name, TextureFlags.Missing, 64, 64);
                Mediator.Publish(EditorMediator.TextureSelected, itemToSelect);
            };

            switch (behaviour)
            {
                case SelectBehaviour.Select:
                    ac.Add(select);
                    break;
                case SelectBehaviour.LiftSelect:
                    lift();
                    ac.Add(select);
                    break;
                case SelectBehaviour.Lift:
                    lift();
                    break;
                case SelectBehaviour.Apply:
                case SelectBehaviour.ApplyWithValues:
                    TextureItem item = _form.GetFirstSelectedTexture();
                    if (item != null)
                    {
                        ITexture texture = item.GetTexture();
                        ac.Add(new EditFace(faces, (document, face) =>
                        {
                            face.Texture.Name = item.Name;
                            face.Texture.Texture = texture;
                            if (behaviour == SelectBehaviour.ApplyWithValues && firstSelected != null)
                            {
                                // Calculates the texture coordinates
                                face.AlignTextureWithFace(firstSelected);
                            }
                            else if (behaviour == SelectBehaviour.ApplyWithValues)
                            {
                                face.Texture.XScale = _form.CurrentProperties.XScale;
                                face.Texture.YScale = _form.CurrentProperties.YScale;
                                face.Texture.XShift = _form.CurrentProperties.XShift;
                                face.Texture.YShift = _form.CurrentProperties.YShift;
                                face.SetTextureRotation(_form.CurrentProperties.Rotation);
                            }
                            else
                            {
                                face.CalculateTextureCoordinates(true);
                            }
                        }, true));
                    }
                    break;
                case SelectBehaviour.AlignToView:
                    OpenTK.Vector3 right = vp.Camera.GetRight();
                    OpenTK.Vector3 up = vp.Camera.GetUp();
                    OpenTK.Vector3 loc = vp.Camera.Location;
                    Coordinate point = new Coordinate((decimal)loc.X, (decimal)loc.Y, (decimal)loc.Z);
                    Coordinate uaxis = new Coordinate((decimal)right.X, (decimal)right.Y, (decimal)right.Z);
                    Coordinate vaxis = new Coordinate((decimal)up.X, (decimal)up.Y, (decimal)up.Z);
                    ac.Add(new EditFace(faces, (document, face) =>
                    {
                        face.Texture.XScale = 1;
                        face.Texture.YScale = 1;
                        face.Texture.UAxis = uaxis;
                        face.Texture.VAxis = vaxis;
                        face.Texture.XShift = face.Texture.UAxis.Dot(point);
                        face.Texture.YShift = face.Texture.VAxis.Dot(point);
                        face.Texture.Rotation = 0;
                        face.CalculateTextureCoordinates(true);
                    }, false));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (!ac.IsEmpty())
            {
                Document.PerformAction(Local.LocalString("tool.texture.selection"), ac);
            }
        }

        public override void KeyDown(ViewportBase viewport, ViewportEvent e)
        {
            //throw new NotImplementedException();
        }

        public override void Render(ViewportBase viewport)
        {
            if (Document.Map.HideFaceMask) return;

            TextureHelper.Unbind();
            GL.Begin(PrimitiveType.Lines);
            foreach (Face face in Document.Selection.GetSelectedFaces())
            {
                Coordinate lineStart = face.BoundingBox.Center + face.Plane.Normal * 0.5m;
                Coordinate uEnd = lineStart + face.Texture.UAxis * 20;
                Coordinate vEnd = lineStart + face.Texture.VAxis * 20;

                GL.Color3(Color.Yellow);
                GL.Vertex3(lineStart.DX, lineStart.DY, lineStart.DZ);
                GL.Vertex3(uEnd.DX, uEnd.DY, uEnd.DZ);

                GL.Color3(Color.FromArgb(0, 255, 0));
                GL.Vertex3(lineStart.DX, lineStart.DY, lineStart.DZ);
                GL.Vertex3(vEnd.DX, vEnd.DY, vEnd.DZ);
            }
            GL.End();
        }

        public override HotkeyInterceptResult InterceptHotkey(HotkeysMediator hotkeyMessage, object parameters)
        {
            switch (hotkeyMessage)
            {
                case HotkeysMediator.OperationsCopy:
                case HotkeysMediator.OperationsCut:
                case HotkeysMediator.OperationsPaste:
                case HotkeysMediator.OperationsPasteSpecial:
                    return HotkeyInterceptResult.Abort;
                case HotkeysMediator.OperationsDelete:
                    List<Face> faces = Document.Selection.GetSelectedFaces().ToList();
                    TextureItem removeFaceTexture = Document.TextureCollection.GetItem("tooltextures/remove_face");
                    if (faces.Count > 0 && removeFaceTexture != null)
                    {
                        Action<Document, Face> action = (doc, face) =>
                        {
                            face.Texture.Name = "tooltextures/remove_face";
                            face.Texture.Texture = removeFaceTexture.GetTexture();
                            face.CalculateTextureCoordinates(false);
                        };
                        Document.PerformAction(Local.LocalString("tool.apply_texture"), new EditFace(faces, action, true));
                        Mediator.Publish(EditorMediator.TextureSelected, faces[0]);
                    }
                    return HotkeyInterceptResult.Abort;
            }
            return HotkeyInterceptResult.Continue;
        }

        public void OperationsDelete()
        {

        }

        public override void MouseEnter(ViewportBase viewport, ViewportEvent e)
        {
            //
        }

        public override void MouseLeave(ViewportBase viewport, ViewportEvent e)
        {
            //
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
            //
        }

        public override void MouseWheel(ViewportBase viewport, ViewportEvent e)
        {
            //
        }

        public override void MouseMove(ViewportBase viewport, ViewportEvent e)
        {
            //
        }

        public override void KeyPress(ViewportBase viewport, ViewportEvent e)
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
    }
}
