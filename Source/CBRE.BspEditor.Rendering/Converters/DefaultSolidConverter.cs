using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Numerics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Rendering.ResourceManagement;
using CBRE.DataStructures.Geometric;
using CBRE.Providers.Texture;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Pipelines;
using CBRE.Rendering.Primitives;
using CBRE.Rendering.Resources;

namespace CBRE.BspEditor.Rendering.Converters
{
    [Export(typeof(IMapObjectSceneConverter))]
    public class DefaultSolidConverter : IMapObjectSceneConverter
    {
        public MapObjectSceneConverterPriority Priority => MapObjectSceneConverterPriority.DefaultLowest;

        public bool ShouldStopProcessing(MapDocument document, IMapObject obj)
        {
            return false;
        }

        public bool Supports(IMapObject obj)
        {
            return obj.Data.OfType<Face>().Any();
        }

        public Task Convert(BufferBuilder builder, MapDocument document, IMapObject obj, ResourceCollector resourceCollector)
        {
            return ConvertFaces(builder, document, obj, obj.Data.Get<Face>().ToList(), resourceCollector);
        }

        internal static async Task ConvertFaces(BufferBuilder builder, MapDocument document, IMapObject obj, List<Face> faces, ResourceCollector resourceCollector)
        {
            faces = faces.Where(x => x.Vertices.Count > 2).ToList();

            DisplayFlags displayFlags = document.Map.Data.GetOne<DisplayFlags>();
            bool hideNull = displayFlags?.HideNullTextures == true;

            // Pack the vertices like this [ f1v1 ... f1vn ] ... [ fnv1 ... fnvn ]
            uint numVertices = (uint) faces.Sum(x => x.Vertices.Count);

            // Pack the indices like this [ solid1 ... solidn ] [ wireframe1 ... wireframe n ]
            uint numSolidIndices = (uint) faces.Sum(x => (x.Vertices.Count - 2) * 3);
            uint numWireframeIndices = numVertices * 2;

            VertexStandard[] points = new VertexStandard[numVertices];
            uint[] indices = new uint[numSolidIndices + numWireframeIndices];

            Vector4 colour = (obj.IsSelected ? Color.Red : obj.Data.GetOne<ObjectColor>()?.Color ?? Color.White).ToVector4();

            //var c = obj.IsSelected ? Color.FromArgb(255, 128, 128) : Color.White;
            //var tint = new Vector4(c.R, c.G, c.B, c.A) / 255f;
            Vector4 tint = Vector4.One;

            Environment.TextureCollection tc = await document.Environment.GetTextureCollection();

            PipelineType pipeline = PipelineType.TexturedOpaque;
            bool entityHasTransparency = false;
            VertexFlags flags = obj.IsSelected ? VertexFlags.SelectiveTransformed : VertexFlags.None;

            // try and find the parent entity for render flags
            // TODO: this code is extremely specific to Goldsource and should be abstracted away
            Entity parentEntity = obj.FindClosestParent(x => x is Entity) as Entity;
            if (parentEntity?.EntityData != null)
            {
                const int renderModeColor = 1;
                const int renderModeTexture = 2;
                const int renderModeGlow = 3; // same as texture for brushes
                const int renderModeSolid = 4;
                const int renderModeAdditive = 5;

                int rendermode = parentEntity.EntityData.Get("rendermode", 0);
                float renderamt = parentEntity.EntityData.Get("renderamt", 255f) / 255;
                entityHasTransparency = renderamt < 0.99;

                switch (rendermode)
                {
                    case renderModeColor:
                        // Flat colour, use render colour and force it to run through the alpha tested pipeline
                        Vector3 rendercolor = parentEntity.EntityData.GetVector3("rendercolor") / 255f ?? Vector3.One;
                        tint = new Vector4(rendercolor, renderamt);
                        flags |= VertexFlags.FlatColour | VertexFlags.AlphaTested;
                        pipeline = PipelineType.TexturedAlpha;
                        entityHasTransparency = true;
                        break;
                    case renderModeTexture:
                    case renderModeGlow:
                        // Texture is alpha tested and can be transparent
                        tint = new Vector4(1, 1, 1, renderamt);
                        flags |= VertexFlags.AlphaTested;
                        if (entityHasTransparency) pipeline = PipelineType.TexturedAlpha;
                        break;
                    case renderModeSolid:
                        // Texture is alpha tested only
                        flags |= VertexFlags.AlphaTested;
                        entityHasTransparency = false;
                        break;
                    case renderModeAdditive:
                        // Texture is alpha tested and transparent, force through the additive pipeline
                        tint = new Vector4(renderamt, renderamt, renderamt, 1);
                        pipeline = PipelineType.TexturedAdditive;
                        entityHasTransparency = true;
                        break;
                    default:
                        entityHasTransparency = false;
                        break;
                }
            }

            if (obj.IsSelected) tint *= new Vector4(1, 0.5f, 0.5f, 1);

            uint vi = 0u;
            uint si = 0u;
            uint wi = numSolidIndices;
            foreach (Face face in faces)
            {
                float opacity = tc.GetOpacity(face.Texture.Name);
                TextureItem t = await tc.GetTextureItem(face.Texture.Name);
                int w = t?.Width ?? 0;
                int h = t?.Height ?? 0;

                Vector4 tintModifier = new Vector4(1, 1, 1, opacity);
                VertexFlags extraFlags = t == null ? VertexFlags.FlatColour : VertexFlags.None;

                uint offs = vi;
                uint numFaceVerts = (uint)face.Vertices.Count;

                List<System.Tuple<Vector3, float, float>> textureCoords = face.GetTextureCoordinates(w, h).ToList();

                Vector3 normal = face.Plane.Normal;
                for (int i = 0; i < face.Vertices.Count; i++)
                {
                    Vector3 v = face.Vertices[i];
                    points[vi++] = new VertexStandard
                    {
                        Position = v,
                        Colour = colour,
                        Normal = normal,
                        Texture = new Vector2(textureCoords[i].Item2, textureCoords[i].Item3),
                        Tint = tint * tintModifier,
                        Flags = flags | extraFlags
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
            foreach (Face f in faces)
            {
                uint texInd = (uint)(f.Vertices.Count - 2) * 3;

                if (hideNull && tc.IsToolTexture(f.Texture.Name))
                {
                    texOffset += texInd;
                    continue;
                }

                float opacity = tc.GetOpacity(f.Texture.Name);
                TextureItem t = await tc.GetTextureItem(f.Texture.Name);
                bool transparent = entityHasTransparency || opacity < 0.95f || t?.Flags.HasFlag(TextureFlags.Transparent) == true;

                string texture = t == null ? string.Empty : $"{document.Environment.ID}::{f.Texture.Name}";

                BufferGroup group = new BufferGroup(
                    pipeline == PipelineType.TexturedOpaque && transparent ? PipelineType.TexturedAlpha : pipeline,
                    CameraType.Perspective, transparent, f.Origin, texture, texOffset, texInd
                );
                groups.Add(group);

                texOffset += texInd;

                if (t != null) resourceCollector.RequireTexture(t.Name);
            }

            groups.Add(new BufferGroup(PipelineType.Wireframe, obj.IsSelected ? CameraType.Both : CameraType.Orthographic, numSolidIndices, numWireframeIndices));
            
            builder.Append(points, indices, groups);

            // Also push the untransformed wireframe when selected
            if (obj.IsSelected)
            {
                for (int i = 0; i < points.Length; i++) points[i].Flags = VertexFlags.None;
                IEnumerable<uint> untransformedIndices = indices.Skip((int) numSolidIndices);
                builder.Append(points, untransformedIndices, new[]
                {
                    new BufferGroup(PipelineType.Wireframe, CameraType.Both, 0, numWireframeIndices)
                });
            }
        }
    }
}