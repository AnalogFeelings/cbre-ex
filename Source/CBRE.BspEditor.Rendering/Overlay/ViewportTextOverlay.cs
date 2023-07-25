using System.ComponentModel.Composition;
using System.Drawing;
using System.Numerics;
using CBRE.BspEditor.Documents;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Overlay;
using CBRE.Rendering.Viewports;

namespace CBRE.BspEditor.Rendering.Overlay
{
    [Export(typeof(IMapDocumentOverlayRenderable))]
    public class ViewportTextOverlay : IMapDocumentOverlayRenderable
    {
        public void SetActiveDocument(MapDocument doc)
        {
            //
        }

        public void Render(IViewport viewport, OrthographicCamera camera, Vector3 worldMin, Vector3 worldMax, I2DRenderer im)
        {
            string str = $"2D {camera.ViewType}";
            Vector2 size = im.CalcTextSize(FontType.Normal, str);

            im.AddRectFilled(Vector2.Zero, size + new Vector2(4, 4), Color.FromArgb(128, Color.Black));
            im.AddText(new Vector2(2, 2), Color.White, FontType.Normal, str);
        }

        public void Render(IViewport viewport, PerspectiveCamera camera, I2DRenderer im)
        {
            string str = $"3D View";
            Vector2 size = im.CalcTextSize(FontType.Normal, str);

            im.AddRectFilled(Vector2.Zero, size + new Vector2(4, 4), Color.FromArgb(128, Color.Black));
            im.AddText(new Vector2(2, 2), Color.White, FontType.Normal, str);
        }
    }
}
