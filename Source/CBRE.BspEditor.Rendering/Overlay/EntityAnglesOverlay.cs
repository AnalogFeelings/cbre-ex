using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Numerics;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.DataStructures.Geometric;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Overlay;
using CBRE.Rendering.Viewports;

namespace CBRE.BspEditor.Rendering.Overlay
{
    [Export(typeof(IMapObject2DOverlay))]
    public class EntityAnglesOverlay : IMapObject2DOverlay
    {
        public void Render(IViewport viewport, ICollection<IMapObject> objects, OrthographicCamera camera, Vector3 worldMin, Vector3 worldMax, I2DRenderer im)
        {
            if (camera.Zoom < 0.5f) return;

            foreach (Entity ed in objects.OfType<Entity>().Where(x => x.EntityData != null).Where(x => !x.Data.OfType<IObjectVisibility>().Any(v => v.IsHidden)))
            {
                Vector3? ang = ed.EntityData.GetVector3("angles");
                if (!ang.HasValue) continue;

                Color c = ed.Color?.Color ?? Color.White;

                Vector3 angRad = ang.Value * (float) Math.PI / 180f;
                float min = Math.Min(ed.BoundingBox.Width, Math.Min(ed.BoundingBox.Height, ed.BoundingBox.Length));
                Matrix4x4 tform = Matrix4x4.CreateFromYawPitchRoll(angRad.X, angRad.Z, angRad.Y);

                Vector3 origin = ed.BoundingBox.Center;

                Vector2 start = camera.WorldToScreen(origin).ToVector2();
                Vector2 end = camera.WorldToScreen(origin + Vector3.Transform(Vector3.UnitX, tform) * 0.4f * min).ToVector2();

                im.AddLine(start, end, c, 2);
            }
        }
    }
}