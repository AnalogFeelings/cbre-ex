using Assimp;
using CBRE.Localization;
using CBRE.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CBRE.SMFConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Log(Local.LocalString("error.converter.no_input"), ConsoleColor.Red);
                Log(Local.LocalString("log.converter.press_any_key"));
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
                                UInt16 mapVersion = reader.ReadUInt16();
                                if (mapVersion != 1)
                                {
                                    Log(Local.LocalString("warning.converter.mapversion", file, mapVersion), ConsoleColor.Yellow);
                                }
                                byte mapFlags = reader.ReadByte();

                                ReadNode(file, directory, reader, scene, rootNode);

                                string resultFilename = Path.GetFileNameWithoutExtension(file) + ".x";

                                context.ExportFile(scene, resultFilename, "x");

                                Log(Local.LocalString("warning.converter.complete", file), ConsoleColor.Green);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log(Local.LocalString("error.converter", file, e.Message, e.StackTrace), ConsoleColor.Red);
                    }
                }

                Log(Local.LocalString("log.converter.done"), ConsoleColor.Cyan);
            }
            Console.ReadKey();
        }

        static void ReadNode(string file, string directory, BinaryReader reader, Scene scene, Node parentNode)
        {
            Int32 childCount = reader.ReadInt32();

            if (childCount > 0)
            {
                Log(Local.LocalString("log.converter.processing", childCount));
            }

            for (int i = 0; i < childCount; i++)
            {
                Node node = new Node($"{parentNode.Name}_child{i}");
                Mesh mesh = new Mesh($"mesh{i}", PrimitiveType.Triangle);

                Vector3D position = reader.ReadVector3D();

                Vector3D rotation = reader.ReadVector3D();

                Vector3D scale = reader.ReadVector3D();

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
                    Assimp.TextureWrapMode.Wrap,
                    Assimp.TextureWrapMode.Wrap,
                    0);
                material.AddMaterialTexture(ref textureSlot);
                scene.Materials.Add(material);

                mesh.MaterialIndex = scene.MaterialCount - 1;

                UInt16 vertexCount = reader.ReadUInt16();
                Log($"{vertexCount} vertices");
                for (int j = 0; j < vertexCount; j++)
                {
                    Vector3D vertexPosition = reader.ReadVector3D();
                    mesh.Vertices.Add(vertexPosition);
                }
                for (int j = 0; j < vertexCount; j++)
                {
                    Vector3D vertexNormal = reader.ReadVector3D();
                    mesh.Normals.Add(vertexNormal);
                }
                for (int j = 0; j < vertexCount; j++)
                {
                    Vector2D vertexTexCoords = reader.ReadVector2D();
                    mesh.TextureCoordinateChannels[0].Add(new Vector3D(vertexTexCoords, 0.0f));
                }
                mesh.UVComponentCount[0] = 2;

                UInt16 triangleCount = reader.ReadUInt16();
                List<int> indices = new List<int>();
                for (int j = 0; j < triangleCount; j++)
                {
                    UInt16 ind0 = reader.ReadUInt16();
                    UInt16 ind1 = reader.ReadUInt16();
                    UInt16 ind2 = reader.ReadUInt16();
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
