using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Documents;
using CBRE.Editor.Tools;
using CBRE.Graphics.Helpers;
using CBRE.Settings;
using CBRE.UI;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CBRE.Editor.Rendering.Helpers
{
    public class CordonHelper : IHelper
    {
        public Document Document { get; set; }
        public bool Is2DHelper { get { return false; } }
        public bool Is3DHelper { get { return false; } }
        public bool IsDocumentHelper { get { return true; } }
        public HelperType HelperType { get { return HelperType.None; } }

        public void AfterRender3D(Viewport3D viewport)
        {
            throw new NotImplementedException();
        }

        public void RenderDocument(ViewportBase viewport, Document document)
        {
            if (!document.Map.Cordon || document.Map.CordonBounds.IsEmpty()) return;
            if (ToolManager.ActiveTool != null && ToolManager.ActiveTool.GetHotkeyToolType() == HotkeyTool.Cordon) return;
            if (viewport is Viewport2D) Render2D((Viewport2D)viewport, document);
            if (viewport is Viewport3D) Render3D((Viewport3D)viewport, document);
        }

        public IEnumerable<MapObject> Order(ViewportBase viewport, IEnumerable<MapObject> mapObjects)
        {
            return mapObjects;
        }

        private void Render2D(Viewport2D viewport, Document document)
        {
            DataStructures.Geometric.Coordinate start = viewport.Flatten(document.Map.CordonBounds.Start);
            DataStructures.Geometric.Coordinate end = viewport.Flatten(document.Map.CordonBounds.End);

            DataStructures.Geometric.Coordinate min = viewport.ScreenToWorld(0, 0);
            DataStructures.Geometric.Coordinate max = viewport.ScreenToWorld(viewport.Width, viewport.Height);

            GL.Color4(Color.FromArgb(128, Color.Purple));
            GL.Begin(PrimitiveType.Quads);

            GL.Vertex3(min.DX, min.DY, 0);
            GL.Vertex3(max.DX, min.DY, 0);
            GL.Vertex3(max.DX, start.DY, 0);
            GL.Vertex3(min.DX, start.DY, 0);

            GL.Vertex3(min.DX, end.DY, 0);
            GL.Vertex3(max.DX, end.DY, 0);
            GL.Vertex3(max.DX, max.DY, 0);
            GL.Vertex3(min.DX, max.DY, 0);

            GL.Vertex3(min.DX, start.DY, 0);
            GL.Vertex3(start.DX, start.DY, 0);
            GL.Vertex3(start.DX, end.DY, 0);
            GL.Vertex3(min.DX, end.DY, 0);

            GL.Vertex3(end.DX, start.DY, 0);
            GL.Vertex3(max.DX, start.DY, 0);
            GL.Vertex3(max.DX, end.DY, 0);
            GL.Vertex3(end.DX, end.DY, 0);

            GL.End();


            GL.LineWidth(2);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Red);
            GL.Vertex3(start.DX, start.DY, start.DZ);
            GL.Vertex3(end.DX, start.DY, start.DZ);
            GL.Vertex3(end.DX, end.DY, start.DZ);
            GL.Vertex3(start.DX, end.DY, start.DZ);
            GL.End();
            GL.LineWidth(1);
        }

        private void Render3D(Viewport3D viewport, Document document)
        {
            DataStructures.Geometric.Box box = document.Map.CordonBounds;
            TextureHelper.Unbind();
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(Color.Red);
            foreach (DataStructures.Geometric.Line line in box.GetBoxLines())
            {
                GL.Vertex3(line.Start.DX, line.Start.DY, line.Start.DZ);
                GL.Vertex3(line.End.DX, line.End.DY, line.End.DZ);
            }
            GL.End();
        }

        public bool IsValidFor(MapObject o)
        {
            return false;
        }

        public void BeforeRender2D(Viewport2D viewport)
        {
            throw new NotImplementedException();
        }

        public void Render2D(Viewport2D viewport, MapObject o)
        {
            throw new NotImplementedException();
        }

        public void AfterRender2D(Viewport2D viewport)
        {
            throw new NotImplementedException();
        }

        public void BeforeRender3D(Viewport3D viewport)
        {
            throw new NotImplementedException();
        }

        public void Render3D(Viewport3D viewport, MapObject o)
        {
            throw new NotImplementedException();
        }
    }
}