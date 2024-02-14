using CBRE.Common.Mediator;
using CBRE.Editor.Documents;
using CBRE.Localization;
using CBRE.Settings;
using CBRE.UI;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Windows.Forms;
using EnableCap = OpenTK.Graphics.OpenGL.EnableCap;
using GL = OpenTK.Graphics.OpenGL.GL;
using View = CBRE.Settings.View;
#pragma warning disable 0612
namespace CBRE.Editor.Rendering
{
    // ReSharper disable CSharpWarnings::CS0612
    // OpenTK's TextPrinter is marked as obsolete but no suitable replacement exists
    public class ViewportLabelListener : IViewportEventListener, IDisposable
    {
        public ViewportBase Viewport { get; set; }
        private TextPrinter _printer;
        private RectangleF _rect;
        private string _text;
        private bool _showing;
        private ContextMenu _menu;

        public ViewportLabelListener(ViewportBase viewport)
        {
            Viewport = viewport;
            _printer = new TextPrinter(TextQuality.High);
            Rebuild();
        }

        public void Rebuild()
        {
            _rect = RectangleF.Empty;
            _text = "";
            if (Viewport is Viewport2D)
            {
                Viewport2D.ViewDirection dir = ((Viewport2D)Viewport).Direction;
                _text = "";
                switch (dir)
                {
                    case Viewport2D.ViewDirection.Top:
                        _text = Local.LocalString("viewpoint.top_xz");
                        break;
                    case Viewport2D.ViewDirection.Front:
                        _text = Local.LocalString("viewpoint.front_zy");
                        break;
                    case Viewport2D.ViewDirection.Side:
                        _text = Local.LocalString("viewpoint.side_xy");
                        break;
                }
            }
            else if (Viewport is Viewport3D)
            {
                Viewport3D.ViewType type = ((Viewport3D)Viewport).Type;
                switch (type)
                {
                    case Viewport3D.ViewType.Lightmapped:
                        _text = Local.LocalString("viewpoint.lightmapped");
                        break;
                    case Viewport3D.ViewType.Textured:
                        _text = Local.LocalString("viewpoint.textured");
                        break;
                    case Viewport3D.ViewType.Wireframe:
                        _text = Local.LocalString("viewpoint.wireframe");
                        break;
                    case Viewport3D.ViewType.Flat:
                        _text = Local.LocalString("viewpoint.flat");
                        break;
                    case Viewport3D.ViewType.Shaded:
                        _text = Local.LocalString("viewpoint.shaded");
                        break;
                };
            }
            if (_menu != null) _menu.Dispose();
            _menu = new ContextMenu(new[]
                                        {
                                            CreateMenu(Local.LocalString("menu.viewpoint.3d_lightmap"), Viewport3D.ViewType.Lightmapped, null),
                                            CreateMenu(Local.LocalString("menu.viewpoint.3d_textured"), Viewport3D.ViewType.Textured, null),
                                            CreateMenu(Local.LocalString("menu.viewpoint.3d_shaded"), Viewport3D.ViewType.Shaded, null),
                                            CreateMenu(Local.LocalString("menu.viewpoint.3d_flat"), Viewport3D.ViewType.Flat, null),
                                            CreateMenu(Local.LocalString("menu.viewpoint.3d_wireframe"), Viewport3D.ViewType.Wireframe, null),
                                            new MenuItem("-"),
                                            CreateMenu(Local.LocalString("menu.viewpoint.2d_top"), null, Viewport2D.ViewDirection.Top),
                                            CreateMenu(Local.LocalString("menu.viewpoint.2d_side"), null, Viewport2D.ViewDirection.Side),
                                            CreateMenu(Local.LocalString("menu.viewpoint.2d_front"), null, Viewport2D.ViewDirection.Front),
                                            new MenuItem("-"),
                                            ScreenshotMenuItem()
                                        });
        }

        private MenuItem ScreenshotMenuItem()
        {
            MenuItem menu = new MenuItem(Local.LocalString("menu.viewpoint.screenshot"));
            menu.Click += (s, e) => Mediator.Publish(HotkeysMediator.ScreenshotViewport, Viewport);
            return menu;
        }

        private MenuItem CreateMenu(string text, Viewport3D.ViewType? type, Viewport2D.ViewDirection? dir)
        {
            MenuItem menu = new MenuItem(text);
            menu.Click += (sender, e) => SwitchType(type, dir);
            if (dir.HasValue && Viewport is Viewport2D)
            {
                Viewport2D.ViewDirection vpdir = ((Viewport2D)Viewport).Direction;
                menu.Checked = vpdir == dir.Value;
            }
            else if (type.HasValue && Viewport is Viewport3D)
            {
                Viewport3D.ViewType vptype = ((Viewport3D)Viewport).Type;
                menu.Checked = vptype == type.Value;
            }
            return menu;
        }

        private void SwitchType(Viewport3D.ViewType? type, Viewport2D.ViewDirection? dir)
        {
            Document doc = DocumentManager.CurrentDocument;
            if (doc == null) return;
            if (type.HasValue)
            {
                Viewport3D vp = Viewport as Viewport3D;
                if (vp == null)
                {
                    doc.Make3D(Viewport, type.Value);
                    return;
                }
                vp.Type = type.Value;
            }
            else if (dir.HasValue)
            {
                Viewport2D vp = Viewport as Viewport2D;
                if (vp == null)
                {
                    doc.Make2D(Viewport, dir.Value);
                    return;
                }
                vp.Direction = dir.Value;
            }
            Rebuild();
        }

        public void Render2D()
        {
            if (_rect.IsEmpty)
            {
                TextExtents te = _printer.Measure(_text, SystemFonts.MessageBoxFont, new RectangleF(0, 0, Viewport.Width, Viewport.Height));
                _rect = te.BoundingBox;
                _rect.X += 5;
                _rect.Y += 2;
                _rect.Width += 5;
                _rect.Height += 2;
            }

            GL.Disable(EnableCap.CullFace);

            _printer.Begin();
            if (_showing)
            {
                GL.Begin(PrimitiveType.Quads);
                GL.Color3(Viewport is Viewport3D ? View.ViewportBackground : Grid.Background);
                GL.Vertex2(0, 0);
                GL.Vertex2(_rect.Right, 0);
                GL.Vertex2(_rect.Right, _rect.Bottom);
                GL.Vertex2(0, _rect.Bottom);
                GL.End();
            }
            _printer.Print(_text, SystemFonts.MessageBoxFont, _showing ? Color.White : Grid.GridLines, _rect);
            _printer.End();

            GL.Enable(EnableCap.CullFace);
        }

        public void PostRender()
        {
            // 
        }

        public void Dispose()
        {
            _printer.Dispose();
        }

        public void KeyUp(ViewportEvent e)
        {

        }

        public void KeyDown(ViewportEvent e)
        {

        }

        public void KeyPress(ViewportEvent e)
        {

        }

        public void MouseMove(ViewportEvent e)
        {
            if (_rect.IsEmpty) return;
            _showing = _rect.Contains(e.X, e.Y);
        }

        public void MouseWheel(ViewportEvent e)
        {

        }

        public void MouseUp(ViewportEvent e)
        {

        }

        public void MouseDown(ViewportEvent e)
        {
            if (_showing)
            {
                _menu.Show(Viewport, new Point(e.X, e.Y));
                e.Handled = true;
            }
        }

        public void MouseClick(ViewportEvent e)
        {

        }

        public void MouseDoubleClick(ViewportEvent e)
        {

        }

        public void MouseEnter(ViewportEvent e)
        {

        }

        public void MouseLeave(ViewportEvent e)
        {
            _showing = false;
        }

        public void UpdateFrame(FrameInfo frame)
        {

        }

        public void PreRender()
        {

        }

        public void Render3D()
        {

        }
    }
}
#pragma warning restore 0612
