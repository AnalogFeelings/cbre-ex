using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Rendering.Resources;
using CBRE.BspEditor.Rendering.Viewport;
using CBRE.BspEditor.Tools.Vertex.Controls;
using CBRE.BspEditor.Tools.Vertex.Selection;
using CBRE.Common.Translations;
using CBRE.DataStructures.Geometric;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Pipelines;
using CBRE.Rendering.Primitives;
using CBRE.Rendering.Resources;
using CBRE.Shell.Input;

namespace CBRE.BspEditor.Tools.Vertex.Tools
{
    [AutoTranslate]
    [Export(typeof(VertexSubtool))]
    public class VertexFaceEditTool : VertexSubtool
    {
        private readonly VertexEditFaceControl _control;

        public override string OrderHint => "F";
        public override string GetName() => "Face editing";
        public override Control Control => _control;
        
        private readonly List<SolidFace> _selectedFaces;

        [ImportingConstructor]
        public VertexFaceEditTool(
            [Import] Lazy<VertexEditFaceControl> control
        )
        {
            _control = control.Value;
            _selectedFaces = new List<SolidFace>();
        }

        protected override IEnumerable<Subscription> Subscribe()
        {
            yield return Oy.Subscribe<string>("VertexTool:DeselectAll", _ => ClearSelection());
            yield return Oy.Subscribe<int>("VertexEditFaceTool:Poke", v => Poke(v));
            yield return Oy.Subscribe<int>("VertexEditFaceTool:Bevel", v => Bevel(v));
        }
        
        private void Poke(int num)
        {
            foreach (SolidFace solidFace in _selectedFaces)
            {
                PokeFace(solidFace, num);
            }
            UpdateSolids(_selectedFaces.Select(x => x.Solid).ToList());
        }

        private void Bevel(int num)
        {
            foreach (SolidFace solidFace in _selectedFaces)
            {
                BevelFace(solidFace, num);
            }
            UpdateSolids(_selectedFaces.Select(x => x.Solid).ToList());
        }

        private IList<MutableFace> GetSelectedFaces()
        {
            return _selectedFaces.Select(x => x.Face).ToList();
        }

        private void ClearSelection()
        {
            _selectedFaces.Clear();
            Invalidate();
        }

        #region Edit faces

        private void PokeFace(SolidFace solidFace, int num)
        {
            MutableFace face = solidFace.Face;
            MutableSolid solid = solidFace.Solid.Copy;

            // Remove the face
            solid.Faces.Remove(face);

            Vector3 center = face.Origin + face.Plane.Normal * num;
            foreach (Line edge in face.GetEdges())
            {
                MutableVertex v1 = face.Vertices.First(x => x.Position.EquivalentTo(edge.Start));
                MutableVertex v2 = face.Vertices.First(x => x.Position.EquivalentTo(edge.End));
                Vector3[] verts = new[] { v1.Position, v2.Position, center };
                MutableFace f = new MutableFace(verts, face.Texture.Clone());
                solid.Faces.Add(f);
            }
        }

        private void BevelFace(SolidFace solidFace, int num)
        {
            MutableFace face = solidFace.Face;
            MutableSolid solid = solidFace.Solid.Copy;

            // Remember the original positions
            List<Vector3> vertexPositions = face.Vertices.Select(x => x.Position).ToList();

            // Scale the face a bit and move it away by the bevel distance
            Vector3 origin = face.Origin;
            face.Transform(Matrix4x4.CreateScale(Vector3.One * 0.9f, origin));
            face.Transform(Matrix4x4.CreateTranslation(face.Plane.Normal * num));

            List<MutableVertex> vertList = face.Vertices.ToList();

            // Create a face for each new edge -> old edge
            foreach (Line edge in face.GetEdges())
            {
                int startIndex = vertList.FindIndex(x => x.Position.EquivalentTo(edge.Start));
                int endIndex = vertList.FindIndex(x => x.Position.EquivalentTo(edge.End));
                Vector3[] verts = new[] { vertexPositions[startIndex], vertexPositions[endIndex], edge.End, edge.Start };
                MutableFace f = new MutableFace(verts, face.Texture.Clone());
                solid.Faces.Add(f);
            }
        }

        #endregion
        
        #region 3D interaction
        
        private Vector3? GetIntersectionPoint(MutableFace face, Line line)
        {
            return new Polygon(face.Vertices.Select(x => x.Position)).GetIntersectionPoint(line);
        }
        
        protected override void MouseDown(MapDocument document, MapViewport viewport, PerspectiveCamera camera,
            ViewportEvent e)
        {
            if (e.Button != MouseButtons.Left) return;

            e.Handled = true;

            // First, get the ray that is cast from the clicked point along the viewport frustrum
            (Vector3 start, Vector3 end) = camera.CastRayFromScreen(new Vector3(e.X, e.Y, 0));
            Line ray = new Line(start, end);

            // Grab all the elements that intersect with the ray
            IEnumerable<VertexSolid> hits = Selection.Where(x => x.Copy.BoundingBox.IntersectsWith(ray));

            // Sort the list of intersecting elements by distance from ray origin
            var clickedFace = hits
                .SelectMany(x => x.Copy.Faces.Select(f => new { Solid = x, Face = f}))
                .Select(x => new { Item = x, Intersection = GetIntersectionPoint(x.Face, ray) })
                .Where(x => x.Intersection != null)
                .OrderBy(x => (x.Intersection.Value - ray.Start).Length())
                .Select(x => x.Item)
                .FirstOrDefault();


            List<SolidFace> faces = new List<SolidFace>();
            if (clickedFace != null)
            {
                if (KeyboardState.Shift) faces.AddRange(clickedFace.Solid.Copy.Faces.Select(x => new SolidFace(clickedFace.Solid, x)));
                else faces.Add(new SolidFace(clickedFace.Solid, clickedFace.Face));
            }
            
            if (!KeyboardState.Ctrl) ClearSelection();
            _selectedFaces.AddRange(faces);

            Invalidate();
        }

        #endregion

        public override async Task SelectionChanged()
        {
            ClearSelection();
        }

        public override async Task ToolSelected()
        {
            ClearSelection();
            await base.ToolSelected();
        }

        public override async Task ToolDeselected()
        {
            ClearSelection();
            await base.ToolDeselected();
        }

        public override void Update()
        {
            ClearSelection();
        }

        private void UpdateSolids(List<VertexSolid> solids)
        {
            if (!solids.Any()) return;

            foreach (VertexSolid solid in solids)
            {
                solid.IsDirty = true;
            }

            Invalidate();
        }

        protected override void Render(MapDocument document, BufferBuilder builder, ResourceCollector resourceCollector)
        {
            base.Render(document, builder, resourceCollector);

            List<VertexStandard> verts = new List<VertexStandard>();
            List<int> indices = new List<int>();
            List<BufferGroup> groups = new List<BufferGroup>();

            Vector4 col = Vector4.One;
            Vector4 tintCol = Color.FromArgb(128, Color.OrangeRed).ToVector4();

            foreach (SolidFace face in _selectedFaces)
            {
                int vo = verts.Count;
                int io = indices.Count;

                verts.AddRange(face.Face.Vertices.Select(x => new VertexStandard
                {
                    Position = x.Position,
                    Colour = col,
                    Tint = tintCol,
                    Flags = VertexFlags.FlatColour
                }));

                for (int i = 2; i < face.Face.Vertices.Count; i++)
                {
                    indices.Add(vo);
                    indices.Add(vo + i - 1);
                    indices.Add(vo + i);
                }

                groups.Add(new BufferGroup(PipelineType.TexturedAlpha, CameraType.Perspective, face.Face.Origin, (uint) io, (uint)(indices.Count - io)));
            }

            builder.Append(verts, indices.Select(x => (uint) x), groups);
        }

        private class SolidFace
        {
            public VertexSolid Solid { get; set; }
            public MutableFace Face { get; set; }

            public SolidFace(VertexSolid solid, MutableFace face)
            {
                Solid = solid;
                Face = face;
            }
        }
    }
}