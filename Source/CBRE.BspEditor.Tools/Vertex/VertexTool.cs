using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Selection;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Rendering.Resources;
using CBRE.BspEditor.Rendering.Viewport;
using CBRE.BspEditor.Tools.Draggable;
using CBRE.BspEditor.Tools.Properties;
using CBRE.BspEditor.Tools.Vertex.Selection;
using CBRE.BspEditor.Tools.Vertex.Tools;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Documents;
using CBRE.Common.Shell.Hooks;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Translations;
using CBRE.DataStructures.Geometric;
using CBRE.Providers.Texture;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Pipelines;
using CBRE.Rendering.Primitives;
using CBRE.Rendering.Resources;
using CBRE.Shell.Input;

namespace CBRE.BspEditor.Tools.Vertex
{
    [Export(typeof(ITool))]
    [Export(typeof(IInitialiseHook))]
    [Export]
    [OrderHint("P")]
    [AutoTranslate]
    [DefaultHotkey("Shift+V")]
    public class VertexTool : BaseDraggableTool, IInitialiseHook
    {
        private readonly IEnumerable<Lazy<VertexSubtool>> _subTools;

        private readonly VertexSelection _selection;

        [ImportingConstructor]
        public VertexTool(
            [ImportMany] IEnumerable<Lazy<VertexSubtool>> subTools
        )
        {
            _subTools = subTools;

            Usage = ToolUsage.Both;

            _selection = new VertexSelection();
        }

        public Task OnInitialise()
        {
            foreach (Lazy<VertexSubtool> st in _subTools.OrderBy(x => x.Value.OrderHint))
            {
                st.Value.Selection = _selection;
                st.Value.Active = Children.Count == 0;
                Children.Add(st.Value);
            }

            return Task.FromResult(false);
        }

        public override Image GetIcon()
        {
            return Resources.Tool_VM;
        }

        public override string GetName()
        {
            return "Vertex Manipulation Tool";
        }

        protected override IEnumerable<Subscription> Subscribe()
        {
            yield return Oy.Subscribe<IDocument>("MapDocument:SelectionChanged", x =>
            {
                if (x == GetDocument()) SelectionChanged();
            });
            yield return Oy.Subscribe<Type>("VertexTool:SetSubTool", t =>
            {
                CurrentSubTool = _subTools.Select(x => x.Value).FirstOrDefault(x => x.GetType() == t);
            });
            yield return Oy.Subscribe<string>("VertexTool:Reset", async _ =>
            {
                MapDocument document = GetDocument();
                if (document != null) await _selection.Reset(document);
                CurrentSubTool?.Update();
                Invalidate();
            });
        }

        public override async Task ToolSelected()
        {
            await SelectionChanged();
            VertexSubtool ct = CurrentSubTool;
            if (ct != null) await ct.ToolSelected();
            await base.ToolSelected();
        }

        public override async Task ToolDeselected()
        {
            MapDocument document = GetDocument();
            if (document != null)
            {
                await _selection.Commit(document);
                await _selection.Clear(document);
            }

            VertexSubtool ct = CurrentSubTool;
            if (ct != null) await ct.ToolDeselected();
            await base.ToolDeselected();
        }

        private async Task SelectionChanged()
        {
            MapDocument document = GetDocument();
            if (document != null)
            {
                await _selection.Commit(document);
                await _selection.Update(document);
            }

            VertexSubtool ct = CurrentSubTool;
            if (ct != null) await ct.SelectionChanged();
            Invalidate();
        }

        #region Tool switching
        
        internal VertexSubtool CurrentSubTool
        {
            get { return Children.OfType<VertexSubtool>().FirstOrDefault(x => x.Active); }
            set
            {
                foreach (BaseTool tool in Children.Where(x => x != value && x.Active))
                {
                    tool.ToolDeselected();
                    tool.Active = false;
                }
                if (value != null)
                {
                    value.Active = true;
                    value.ToolSelected();
                }

                Oy.Publish("VertexTool:SubToolChanged", value?.GetType());
            }
        }

        #endregion

        private void SelectObject(MapDocument document, Solid closestObject)
        {
            // Nothing was clicked, don't change the selection
            if (closestObject == null) return;

            Transaction operation = new Transaction();

            // Ctrl doesn't toggle selection, only adds to it.
            // Ctrl+clicking a selected solid will do nothing.

            if (!KeyboardState.Ctrl)
            {
                // Ctrl isn't down, so we want to clear the selection
                operation.Add(new Deselect(document.Selection.Where(x => !ReferenceEquals(x, closestObject)).ToList()));
            }

            if (!closestObject.IsSelected)
            {
                // The clicked object isn't selected yet, select it.
                operation.Add(new Select(closestObject));
            }

            if (!operation.IsEmpty)
            {
                MapDocumentOperation.Perform(document, operation);
            }
        }
        
        #region 3D interaction

        /// <summary>
        /// When the mouse is pressed in the 3D view, we want to select the clicked object.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="viewport">The viewport that was clicked</param>
        /// <param name="camera"></param>
        /// <param name="e">The click event</param>
        protected override void MouseDown(MapDocument document, MapViewport viewport, PerspectiveCamera camera, ViewportEvent e)
        {
            // First, get the ray that is cast from the clicked point along the viewport frustrum
            (Vector3 start, Vector3 end) = camera.CastRayFromScreen(new Vector3(e.X, e.Y, 0));
            Line ray = new Line(start, end);

            // Grab all the elements that intersect with the ray
            Solid closestObject = document.Map.Root.GetIntersectionsForVisibleObjects(ray)
                .Where(x => x.Object is Solid)
                .Select(x => (Solid) x.Object)
                .FirstOrDefault();

            SelectObject(document, closestObject);
        }

        #endregion

        #region 2D interaction

        private IEnumerable<IMapObject> GetLineIntersections(MapDocument document, Box box)
        {
            return document.Map.Root.Collect(
                x => x is Root || (x.BoundingBox != null && x.BoundingBox.IntersectsWith(box)),
                x => x.Hierarchy.Parent != null && !x.Hierarchy.HasChildren && x is Solid && x.GetPolygons().Any(p => p.GetLines().Any(box.IntersectsWith))
            );
        }

        private IMapObject SelectionTest(MapDocument document, OrthographicCamera camera, ViewportEvent e)
        {
            // Create a box to represent the click, with a tolerance level
            Vector3 unused = camera.GetUnusedCoordinate(new Vector3(100000, 100000, 100000));
            float tolerance = 4 / camera.Zoom; // Selection tolerance of four pixels
            Vector3 used = camera.Expand(new Vector3(tolerance, tolerance, 0));
            Vector3 add = used + unused;
            Vector3 click = camera.ScreenToWorld(e.X, e.Y);
            Box box = new Box(click - add, click + add);
            return GetLineIntersections(document, box).FirstOrDefault();
        }

        protected override void MouseDown(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e)
        {
            // Get the first element that intersects with the box, selecting or deselecting as needed
            Solid closestObject = SelectionTest(document, camera, e) as Solid;
            SelectObject(document, closestObject);
        }

        #endregion

        protected override void Render(MapDocument document, BufferBuilder builder, ResourceCollector resourceCollector)
        {
            base.Render(document, builder, resourceCollector);

            // Force this work to happen on a new thread so waiting on it won't block the context
            Task.Run(async () =>
            {
                foreach (VertexSolid obj in _selection)
                {
                    await Convert(builder, document, obj.Copy, resourceCollector);
                }
            }).Wait();
        }

        public void Invalidate()
        {
            Oy.Publish("VertexTool:Updated", _selection);
        }

        private async Task Convert(BufferBuilder builder, MapDocument document, MutableSolid solid, ResourceCollector resourceCollector)
        {
            DisplayFlags displayFlags = document.Map.Data.GetOne<DisplayFlags>();
            bool hideNull = displayFlags?.HideNullTextures == true;

            List<MutableFace> faces = solid.Faces.Where(x => x.Vertices.Count > 2).ToList();

            // Pack the vertices like this [ f1v1 ... f1vn ] ... [ fnv1 ... fnvn ]
            uint numVertices = (uint)faces.Sum(x => x.Vertices.Count);

            // Pack the indices like this [ solid1 ... solidn ] [ wireframe1 ... wireframe n ]
            uint numSolidIndices = (uint)faces.Sum(x => (x.Vertices.Count - 2) * 3);
            uint numWireframeIndices = numVertices * 2;

            VertexStandard[] points = new VertexStandard[numVertices];
            uint[] indices = new uint[numSolidIndices + numWireframeIndices];

            Vector4 tint = Color.FromArgb(128, 255, 128).ToVector4();

            Environment.TextureCollection tc = await document.Environment.GetTextureCollection();

            uint vi = 0u;
            uint si = 0u;
            uint wi = numSolidIndices;
            foreach (MutableFace face in faces)
            {
                float opacity = tc.GetOpacity(face.Texture.Name);
                TextureItem t = await tc.GetTextureItem(face.Texture.Name);
                int w = t?.Width ?? 0;
                int h = t?.Height ?? 0;

                Vector4 tintModifier = new Vector4(1, 1, 1, opacity);

                uint offs = vi;
                uint numFaceVerts = (uint)face.Vertices.Count;

                List<Tuple<Vector3, float, float>> textureCoords = face.GetTextureCoordinates(w, h).ToList();

                Vector3 normal = face.Plane.Normal;
                for (int i = 0; i < face.Vertices.Count; i++)
                {
                    MutableVertex v = face.Vertices[i];
                    points[vi++] = new VertexStandard
                    {
                        Position = v.Position,
                        Colour = Vector4.One,
                        Normal = normal,
                        Texture = new Vector2(textureCoords[i].Item2, textureCoords[i].Item3),
                        Tint = tint * tintModifier,
                        Flags = VertexFlags.None
                    };
                }

                // Triangles - [0 1 2]  ... [0 n-1 n]
                for (uint i = 2; i < numFaceVerts; i++)
                {
                    indices[si++] = offs;
                    indices[si++] = offs + i - 1;
                    indices[si++] = offs + i;
                }

                // Lines - [0 1] ... [n-1 n] [n 0]
                for (uint i = 0; i < numFaceVerts; i++)
                {
                    indices[wi++] = offs + i;
                    indices[wi++] = offs + (i == numFaceVerts - 1 ? 0 : i + 1);
                }
            }

            List<BufferGroup> groups = new List<BufferGroup>();

            uint texOffset = 0;
            foreach (MutableFace f in faces)
            {
                uint texInd = (uint)(f.Vertices.Count - 2) * 3;

                if (hideNull && tc.IsToolTexture(f.Texture.Name))
                {
                    texOffset += texInd;
                    continue;
                }

                float opacity = tc.GetOpacity(f.Texture.Name);
                TextureItem t = await tc.GetTextureItem(f.Texture.Name);
                bool transparent = opacity < 0.95f || t?.Flags.HasFlag(TextureFlags.Transparent) == true;

                string texture = t == null ? string.Empty : $"{document.Environment.ID}::{f.Texture.Name}";

                groups.Add(transparent
                    ? new BufferGroup(PipelineType.TexturedAlpha, CameraType.Perspective, f.Origin, texture, texOffset, texInd)
                    : new BufferGroup(PipelineType.TexturedOpaque, CameraType.Perspective, texture, texOffset, texInd)
                );

                texOffset += texInd;

                if (t != null) resourceCollector.RequireTexture(t.Name);
            }

            groups.Add(new BufferGroup(PipelineType.Wireframe, CameraType.Both, numSolidIndices, numWireframeIndices));

            builder.Append(points, indices, groups);
        }
    }
}
