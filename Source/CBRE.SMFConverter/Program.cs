using Assimp;
using CBRE.Common.Extensions;
using CBRE.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CBRE.SMFConverter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!args.Any())
            {
                Log("No input files defined! Just drag and drop files onto CBRE.SMFConverter.exe", ConsoleColor.Red);
                Log("Press any key to exit...");
            }
            else
            {
                AssimpContext context = new AssimpContext();

                foreach (string file in args)
                {
                    try
                    {
                        string directory = Path.GetDirectoryName(file).Replace('\\', '/');
                        if (directory.Length > 0 && directory.Last() != '/') { directory += "/"; }

                        Scene scene = new Scene();
                        Node rootNode = new Node("rootnode");
                        scene.RootNode = rootNode;

                        using (FileStream fileStream = new FileStream(file, FileMode.Open))
                        {
                            using (BinaryReader reader = new BinaryReader(fileStream))
                            {
                                // header
                                ushort mapVersion = reader.ReadUInt16();
                                if (mapVersion != 1)
                                {
                                    Log($"[{file}] Warning: mapVersion != 1 ({mapVersion})", ConsoleColor.Yellow);
                                }
                                byte mapFlags = reader.ReadByte();

                                ReadNode(file, directory, reader, scene, rootNode);

                                string resultFilename = Path.GetFileNameWithoutExtension(file) + ".x";

                                context.ExportFile(scene, resultFilename, "x");

                                Log($"[{file}] Complete!", ConsoleColor.Green);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log($"[{file}] Error: {e.Message}\n{e.StackTrace}", ConsoleColor.Red);
                    }
                }

                Log("Done! Press any key to exit...", ConsoleColor.Cyan);
            }
            Console.ReadKey();
        }

        static void ReadNode(string file, string directory, BinaryReader reader, Scene scene, Node parentNode)
        {
            int childCount = reader.ReadInt32();

            if (childCount > 0)
            {
                Log($"Processing {childCount} node{(childCount == 1 ? "" : "s")}");
            }

            for (int i = 0; i < childCount; i++)
            {
                Node node = new Node($"{parentNode.Name}_child{i}");
                Mesh mesh = new Mesh($"mesh{i}", PrimitiveType.Triangle);

                Vector3D position = reader.ReadAssimpVector3();

                Vector3D rotation = reader.ReadAssimpVector3();

                Vector3D scale = reader.ReadAssimpVector3();

                string textureGroup = reader.ReadNullTerminatedString();

                string textureName = Path.GetFileNameWithoutExtension(reader.ReadNullTerminatedString());

                Matrix4x4 transform = Matrix4x4.FromScaling(scale) * Matrix4x4.FromEulerAnglesXYZ(rotation) * Matrix4x4.FromTranslation(position);

                node.Transform = transform;

                Material material = new Material();
                material.Name = textureName;
                TextureSlot textureSlot = new TextureSlot(textureName +
                    (File.Exists(directory + textureName + ".png") ? ".png" : (File.Exists(directory + textureName + ".jpeg") ? ".jpeg" : ".jpg")),
                    TextureType.Diffuse,
                    0,
                    TextureMapping.Plane,
                    0,
                    1.0f,
                    TextureOperation.Multiply,
                    TextureWrapMode.Wrap,
                    TextureWrapMode.Wrap,
                    0);
                material.AddMaterialTexture(ref textureSlot);
                scene.Materials.Add(material);

                mesh.MaterialIndex = scene.MaterialCount - 1;

                ushort vertexCount = reader.ReadUInt16();
                Log($"{vertexCount} vertices");
                for (int j = 0; j < vertexCount; j++)
                {
                    Vector3D vertexPosition = reader.ReadAssimpVector3();
                    mesh.Vertices.Add(vertexPosition);
                }
                for (int j = 0; j < vertexCount; j++)
                {
                    Vector3D vertexNormal = reader.ReadAssimpVector3();
                    mesh.Normals.Add(vertexNormal);
                }
                for (int j = 0; j < vertexCount; j++)
                {
                    Vector2D vertexTexCoords = reader.ReadAssimpVector2();
                    mesh.TextureCoordinateChannels[0].Add(new Vector3D(vertexTexCoords, 0.0f));
                }
                mesh.UVComponentCount[0] = 2;

                ushort triangleCount = reader.ReadUInt16();
                List<int> indices = new List<int>();
                for (int j = 0; j < triangleCount; j++)
                {
                    ushort ind0 = reader.ReadUInt16();
                    ushort ind1 = reader.ReadUInt16();
                    ushort ind2 = reader.ReadUInt16();
                    indices.Add(ind2);
                    indices.Add(ind1);
                    indices.Add(ind0);
                }
                mesh.SetIndices(indices.ToArray(), 3);
                scene.Meshes.Add(mesh);

                node.MeshIndices.Add(scene.MeshCount - 1);

                parentNode.Children.Add(node);

                ReadNode(file, directory, reader, scene, node);
            }
        }

        static void Log(string msg, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
        }
    }
}
