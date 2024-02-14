using Assimp;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.DataStructures.Models;
using CBRE.FileSystem;
using CBRE.Localization;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Face = CBRE.DataStructures.MapObjects.Face;
using Mesh = Assimp.Mesh;
using Path = System.IO.Path;

namespace CBRE.Providers.Model
{
    public class AssimpProvider : ModelProvider
    {
        protected static AssimpContext importer = null;

        protected override bool IsValidForFile(IFile file)
        {
            return file.Extension.ToLowerInvariant() == "b3d" ||
                   file.Extension.ToLowerInvariant() == "fbx" ||
                   file.Extension.ToLowerInvariant() == "x";
        }

        protected static void AddNode(Scene scene, Node node, DataStructures.Models.Model model, DataStructures.Models.Texture tex, Matrix4x4 parentMatrix)
        {
            Matrix4x4 selfMatrix = node.Transform * parentMatrix;
            foreach (int meshIndex in node.MeshIndices)
            {
                DataStructures.Models.Mesh sledgeMesh = AddMesh(model, scene.Meshes[meshIndex], selfMatrix);
                foreach (MeshVertex v in sledgeMesh.Vertices)
                {
                    v.TextureU *= tex.Width;
                    v.TextureV *= tex.Height;
                }
                model.AddMesh("mesh", 0, sledgeMesh);
            }

            foreach (Node subNode in node.Children)
            {
                AddNode(scene, subNode, model, tex, selfMatrix);
            }
        }

        protected static DataStructures.Models.Mesh AddMesh(DataStructures.Models.Model sledgeModel, Assimp.Mesh assimpMesh, Matrix4x4 selfMatrix)
        {
            DataStructures.Models.Mesh sledgeMesh = new DataStructures.Models.Mesh(0);
            List<MeshVertex> vertices = new List<MeshVertex>();
            List<Vector3D> normals = new List<Vector3D>();
            if (assimpMesh.HasNormals)
            {
                normals.AddRange(assimpMesh.Normals);
            }
            else
            {
                List<Vector3D> assimpVertices = assimpMesh.Vertices;
                for (int i = 0; i < assimpMesh.VertexCount; i++)
                {
                    normals.Add(new Vector3D(0, 0, 0));
                }

                foreach (Assimp.Face face in assimpMesh.Faces)
                {
                    List<int> triInds = face.Indices;
                    for (int i = 1; i < triInds.Count - 1; i++)
                    {
                        Vector3D normal = Vector3D.Cross(assimpVertices[triInds[0]] - assimpVertices[triInds[i]], assimpVertices[triInds[0]] - assimpVertices[triInds[i + 1]]);
                        normal.Normalize();

                        normals[triInds[0]] += normal;
                        normals[triInds[i]] += normal;
                        normals[triInds[i + 1]] += normal;
                    }
                }

                for (int i = 0; i < assimpMesh.VertexCount; i++)
                {
                    normals[i].Normalize();
                }
            }

            for (int i = 0; i < assimpMesh.VertexCount; i++)
            {
                Vector3D assimpVertex = assimpMesh.Vertices[i];
                assimpVertex = selfMatrix * assimpVertex;
                Vector3D assimpNormal = normals[i];
                assimpNormal = selfMatrix * assimpNormal;
                Vector3D assimpUv = assimpMesh.TextureCoordinateChannels[0][i];

                vertices.Add(new MeshVertex(new CoordinateF(assimpVertex.X, -assimpVertex.Z, assimpVertex.Y),
                                            new CoordinateF(assimpNormal.X, -assimpNormal.Z, assimpNormal.Y),
                                            sledgeModel.Bones[0], assimpUv.X, -assimpUv.Y));
            }

            foreach (Assimp.Face face in assimpMesh.Faces)
            {
                List<int> triInds = face.Indices;
                for (int i = 1; i < triInds.Count - 1; i++)
                {
                    sledgeMesh.Vertices.Add(new MeshVertex(vertices[triInds[0]].Location, vertices[triInds[0]].Normal, vertices[triInds[0]].BoneWeightings, vertices[triInds[0]].TextureU, vertices[triInds[0]].TextureV));
                    sledgeMesh.Vertices.Add(new MeshVertex(vertices[triInds[i + 1]].Location, vertices[triInds[i + 1]].Normal, vertices[triInds[i + 1]].BoneWeightings, vertices[triInds[i + 1]].TextureU, vertices[triInds[i + 1]].TextureV));
                    sledgeMesh.Vertices.Add(new MeshVertex(vertices[triInds[i]].Location, vertices[triInds[i]].Normal, vertices[triInds[i]].BoneWeightings, vertices[triInds[i]].TextureU, vertices[triInds[i]].TextureV));
                }
            }

            return sledgeMesh;
        }

        protected override DataStructures.Models.Model LoadFromFile(IFile file)
        {
            if (importer == null)
            {
                importer = new AssimpContext();
                //importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
            }

            DataStructures.Models.Model model = new DataStructures.Models.Model();
            DataStructures.Models.Bone bone = new DataStructures.Models.Bone(0, -1, null, "rootBone", CoordinateF.Zero, CoordinateF.Zero, CoordinateF.One, CoordinateF.One);
            model.Bones.Add(bone);

            Scene scene = importer.ImportFile(file.FullPathName);

            DataStructures.Models.Texture tex = null;

            if (scene.MaterialCount > 0)
            {
                //TODO: handle several textures
                for (int i = 0; i < scene.MaterialCount; i++)
                {
                    if (string.IsNullOrEmpty(scene.Materials[i].TextureDiffuse.FilePath)) { continue; }
                    string path = Path.Combine(Path.GetDirectoryName(file.FullPathName), scene.Materials[i].TextureDiffuse.FilePath);
                    if (!File.Exists(path)) { path = scene.Materials[i].TextureDiffuse.FilePath; }
                    if (File.Exists(path))
                    {
                        Bitmap bmp = new Bitmap(path);
                        tex = new DataStructures.Models.Texture
                        {
                            Name = path,
                            Index = 0,
                            Width = bmp.Width,
                            Height = bmp.Height,
                            Flags = 0,
                            Image = bmp
                        };
                    }
                    break;
                }
            }

            if (tex == null)
            {
                Bitmap bmp = new Bitmap(64, 64);
                for (int i = 0; i < 64; i++)
                {
                    for (int j = 0; j < 64; j++)
                    {
                        bmp.SetPixel(i, j, Color.DarkGray);
                    }
                }
                tex = new DataStructures.Models.Texture
                {
                    Name = Local.LocalString("texture.blank"),
                    Index = 0,
                    Width = 64,
                    Height = 64,
                    Flags = 0,
                    Image = bmp
                };
            }

            model.Textures.Add(tex);

            AddNode(scene, scene.RootNode, model, tex, Matrix4x4.Identity);

            return model;
        }

        public static void SaveToFile(string filename, DataStructures.MapObjects.Map map, string format)
        {
            Scene scene = new Scene();

            Node rootNode = new Node();
            rootNode.Name = "root";
            scene.RootNode = rootNode;

            Node newNode = new Node();

            Mesh mesh;
            int vertOffset;
            string[] textures = map.GetAllTextures().ToArray();
            foreach (string texture in textures)
            {
                if (texture == "tooltextures/remove_face") { continue; }

                Material material = new Material();
                material.Name = texture;
                TextureSlot textureSlot = new TextureSlot(texture +
                    (File.Exists(texture + ".png") ? ".png" : (File.Exists(texture + ".jpeg") ? ".jpeg" : ".jpg")),
                    TextureType.Diffuse,
                    0,
                    TextureMapping.Plane,
                    0,
                    1.0f,
                    TextureOperation.Multiply,
                    Assimp.TextureWrapMode.Wrap,
                    Assimp.TextureWrapMode.Wrap,
                    0);
                material.AddMaterialTexture(ref textureSlot);
                scene.Materials.Add(material);

                mesh = new Mesh();
                if (format != "obj") // .obj files should have no mesh names so they are one proper mesh
                {
                    mesh.Name = texture + "_mesh";
                }
                mesh.MaterialIndex = scene.MaterialCount - 1;
                vertOffset = 0;

                List<int> indices = new List<int>();

                IEnumerable<Face> faces = map.WorldSpawn.Find(x => x is Solid).
                    OfType<Solid>().
                    SelectMany(x => x.Faces).
                    Where(x => x.Texture.Name == texture);

                foreach (Face face in faces)
                {
                    foreach (Vertex v in face.Vertices)
                    {
                        mesh.Vertices.Add(new Vector3D((float)v.Location.X, (float)v.Location.Z, (float)v.Location.Y));
                        mesh.Normals.Add(new Vector3D((float)face.Plane.Normal.X, (float)face.Plane.Normal.Z, (float)face.Plane.Normal.Y));
                        mesh.TextureCoordinateChannels[0].Add(new Vector3D((float)v.TextureU, (float)v.TextureV, 0));
                    }
                    mesh.UVComponentCount[0] = 2;
                    foreach (uint ind in face.GetTriangleIndices())
                    {
                        indices.Add((int)ind + vertOffset);
                    }

                    vertOffset += face.Vertices.Count;
                }

                mesh.SetIndices(indices.ToArray(), 3);
                scene.Meshes.Add(mesh);

                newNode.MeshIndices.Add(scene.MeshCount - 1);
            }

            rootNode.Children.Add(newNode);

            new AssimpContext().ExportFile(scene, filename, format);
        }
    }
}
