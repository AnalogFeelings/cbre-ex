using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CBRE.BspEditor.Environment;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common;
using CBRE.Common.Extensions;
using CBRE.Common.Shell.Documents;
using CBRE.DataStructures.Geometric;
using Plane = CBRE.DataStructures.Geometric.Plane;

namespace CBRE.BspEditor.Providers
{
    [Export(typeof(IBspSourceProvider))]
    public class L3dwBspSourceProvider : IBspSourceProvider
    {
        private static readonly IEnumerable<Type> SupportedTypes = new List<Type>
        {
            typeof(IMapObject),
            typeof(IMapObjectData),
            typeof(IMapData),
        };

        public IEnumerable<Type> SupportedDataTypes => SupportedTypes;

        public IEnumerable<FileExtensionInfo> SupportedFileExtensions { get; } = new[]
        {
            new FileExtensionInfo("Leadwerks 3D World Studio", ".3dw"),
        };

        public async Task<BspFileLoadResult> Load(Stream stream, IEnvironment environment)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (BinaryReader br = new BinaryReader(stream, Encoding.ASCII, true))
                {
                    BspFileLoadResult result = new BspFileLoadResult();

                    ushort mapVersion = br.ReadUInt16();

                    // Map version should ALWAYS be 14.
                    if (mapVersion != 14)
                        throw new NotSupportedException("Incorrect 3DW version number. Expected 14, got " + mapVersion + ".");

                    byte mapFlags = br.ReadByte();
                    int nameCount = br.ReadInt32();
                    int nameOffset = br.ReadInt32();
                    int objectCount = br.ReadInt32();
                    int objectOffset = br.ReadInt32();

                    // Get names, needed to understand the objects
                    List<string> nameList = new List<string>();

                    br.BaseStream.Seek(nameOffset, SeekOrigin.Begin);

                    for (int i = 0; i < nameCount; i++)
                    {
                        string name = br.ReadNullTerminatedString();

                        nameList.Add(name);
                    }

                    Map map = new Map();

                    // Now we can parse the object table.
                    List<string> materialList = new List<string>();
                    Dictionary<int, string> meshTable = new Dictionary<int, string>();
                    Dictionary<int, Group> groupTable = new Dictionary<int, Group>();
                    Dictionary<int, long> visgroupTable = new Dictionary<int, long>();

                    br.BaseStream.Seek(objectOffset, SeekOrigin.Begin);

                    long objectStartPos = br.BaseStream.Position;

                    for (int i = 0; i < objectCount; i++)
                    {
                        int index = br.ReadInt32() - 1;
                        int size = br.ReadInt32();
                        string name = null;

                        if (index >= 0 && index < nameList.Count)
                        {
                            name = nameList[index];
                        }

                        if (name == "group")
                        {
                            // Both of these are unused.
                            byte flags = br.ReadByte();
                            int groupIndex = br.ReadInt32();

                            Group newGroup = new Group(map.NumberGenerator.Next("MapObject"));
                            newGroup.Hierarchy.Parent = map.Root;

                            groupTable.Add(i, newGroup);
                        }
                        else if (name == "visgroup")
                        {
                            byte flags = br.ReadByte(); // Unused.
                            string groupName = nameList[br.ReadInt32() - 1];

                            Visgroup newGroup = new Visgroup()
                            {
                                Name = groupName, 
                                ID = visgroupTable.Count + 1,
                                Colour = br.ReadRGBColour()
                            };

                            map.Data.Add(newGroup);

                            visgroupTable.Add(i, newGroup.ID);
                        }
                        else if (name == "meshreference")
                        {
                            // Both of these are unused.
                            byte flags = br.ReadByte();
                            int groupNameInd = br.ReadInt32() - 1;

                            int objectNameInd = br.ReadInt32() - 1;

                            byte limbCount = br.ReadByte(); // Unused.

                            meshTable.Add(i, nameList[objectNameInd]);
                        }
                        else if (name == "material")
                        {
                            byte materialFlags = br.ReadByte();
                            int groupIndex = br.ReadInt32(); // Unused.
                            string objectName = nameList[br.ReadInt32() - 1];

                            if ((materialFlags & 2) != 0)
                            {
                                int extensionNameIndex = br.ReadInt32(); // Unknown, unused.
                            }

                            materialList.Add(objectName);
                        }
                        else
                        {
                            br.BaseStream.Seek(size, SeekOrigin.Current);
                        }
                    }

                    br.BaseStream.Position = objectStartPos;

                    for (int i = 0; i < objectCount; i++)
                    {
                        int index = br.ReadInt32() - 1;
                        int size = br.ReadInt32();
                        string name = null;

                        if (index >= 0 && index < nameList.Count)
                        {
                            name = nameList[index];
                        }

                        if (name == "mesh")
                        {
                            long startPos = br.BaseStream.Position;

                            byte flags = br.ReadByte();

                            Entity entity = new Entity(map.NumberGenerator.Next("MapObject"));
                            entity.Data.Add(new EntityData()
                            {
                                Name = "model"
                            });

                            int keyCount = br.ReadInt32();
                            for (int j = 0; j < keyCount; j++)
                            {
                                int keyNameInd = br.ReadInt32() - 1;
                                int keyValueInd = br.ReadInt32() - 1;
                                if (nameList[keyNameInd] != "classname")
                                {
                                    string key = nameList[keyNameInd];
                                    string value = nameList[keyValueInd];

                                    entity.EntityData.Properties.Add(key, value);

                                    if (key == "file")
                                    {
                                        entity.EntityData.Properties[key] = System.IO.Path.GetFileNameWithoutExtension(value);
                                    }
                                }
                            }

                            int groupIndex = br.ReadInt32() - 1;
                            int visgroupIndex = br.ReadInt32() - 1;

                            if (visgroupTable.ContainsKey(visgroupIndex))
                            {
                                // Add the entity to the visgroup.
                                entity.Data.Add(new VisgroupID(visgroupTable[visgroupIndex]));
                            }

                            // These are unused.
                            byte red = br.ReadByte(); 
                            byte green = br.ReadByte(); 
                            byte blue = br.ReadByte();

                            int meshTableIndex = br.ReadInt32() - 1;

                            // 3dw stores vectors as XZY and not XYZ.
                            float x = br.ReadSingle();
                            float z = br.ReadSingle();
                            float y = br.ReadSingle();

                            if (entity != null) entity.Origin = new Vector3(x, y, z);

                            if (!entity.EntityData.Properties.ContainsKey("file"))
                            {
                                entity.EntityData.Properties.Add("file", meshTable.FirstOrDefault(q => q.Key == meshTableIndex).Value);
                            }

                            float pitch = br.ReadSingle();
                            float yaw = br.ReadSingle();
                            float roll = br.ReadSingle();

                            entity.EntityData.Properties.Add("angles", pitch + " " + yaw + " " + roll);

                            float xScale = 1.0f;
                            float yScale = 1.0f;
                            float zScale = 1.0f;

                            if ((flags & 1) == 0)
                            {
                                // For whatever reason, it now uses XYZ all of the sudden.
                                xScale = br.ReadSingle();
                                yScale = br.ReadSingle();
                                zScale = br.ReadSingle();
                            }
                            
                            entity.EntityData.Properties.Add("scale", xScale + " " + yScale + " " + zScale);

                            br.BaseStream.Position += size - (br.BaseStream.Position - startPos);

                            // Check group and set parent.
                            if (groupTable.ContainsKey(groupIndex))
                            {
                                entity.Hierarchy.Parent = groupTable[groupIndex];
                            }
                            else
                            {
                                entity.Hierarchy.Parent = map.Root;
                            }
                        }
                        else if (name == "entity")
                        {
                            byte flags = br.ReadByte(); // Unused.

                            // Aaaand we are back to XZY again.
                            float x = br.ReadSingle();
                            float z = br.ReadSingle();
                            float y = br.ReadSingle();

                            Entity entity = new Entity(map.NumberGenerator.Next("MapObject"))
                            {
                                Origin = new Vector3(x, y, z)
                            };

                            int keyCount = br.ReadInt32();
                            for (int j = 0; j < keyCount; j++)
                            {
                                int keyNameInd = br.ReadInt32() - 1;
                                int keyValueInd = br.ReadInt32() - 1;
                                if (nameList[keyNameInd] == "classname")
                                {
                                    entity.Data.Add(new EntityData()
                                    {
                                        Name = nameList[keyValueInd]
                                    });
                                }
                                else
                                {
                                    string key = nameList[keyNameInd];
                                    string value = nameList[keyValueInd];
                                    
                                    entity.EntityData.Properties.Add(key, value);
                                }
                            }

                            int groupIndex = br.ReadInt32() - 1;
                            int visgroupIndex = br.ReadInt32() - 1;

                            if (visgroupTable.ContainsKey(visgroupIndex))
                            {
                                // Add the entity to the visgroup.
                                entity.Data.Add(new VisgroupID(visgroupTable[visgroupIndex]));
                            }
                            
                            // Check group and set parent.
                            if (groupTable.ContainsKey(groupIndex))
                            {
                                entity.Hierarchy.Parent = groupTable[groupIndex];
                            }
                            else
                            {
                                entity.Hierarchy.Parent = map.Root;
                            }
                        }
                        else if (name == "brush")
                        {
                            bool invisibleCollision = false;

                            byte brushFlags = br.ReadByte(); //Unknown, unused.

                            int keys = br.ReadInt32();
                            for (int j = 0; j < keys; j++)
                            {
                                int keyNameInd = br.ReadInt32();
                                int keyValueInd = br.ReadInt32();
                                string keyName = nameList[keyNameInd - 1];

                                if (keyName.Equals("classname", StringComparison.OrdinalIgnoreCase))
                                {
                                    string keyValue = nameList[keyValueInd - 1];
                                    if (keyValue.Equals("field_hit", StringComparison.OrdinalIgnoreCase))
                                    {
                                        invisibleCollision = true;
                                    }
                                }
                            }

                            int groupIndex = br.ReadInt32() - 1;
                            int visgroupIndex = br.ReadInt32() - 1;

                            // These are unused.
                            byte red = br.ReadByte(); 
                            byte green = br.ReadByte(); 
                            byte blue = br.ReadByte();

                            // Read vertices.
                            List<Vector3> vertices = new List<Vector3>();
                            byte vertexCount = br.ReadByte();
                            for (int j = 0; j < vertexCount; j++)
                            {
                                float x = br.ReadSingle(); 
                                float z = br.ReadSingle(); 
                                float y = br.ReadSingle();

                                vertices.Add(new Vector3(x, y, z));
                            }

                            List<Face> faces = new List<Face>();
                            byte faceCount = br.ReadByte();
                            for (int j = 0; j < faceCount; j++)
                            {
                                byte faceFlags = br.ReadByte();

                                //TODO: maybe we need these unused bits for something idk
                                float planeEq0 = br.ReadSingle(); 
                                float planeEq1 = br.ReadSingle(); 
                                float planeEq2 = br.ReadSingle(); 
                                float planeEq3 = br.ReadSingle(); // Unused.

                                float texPosX = br.ReadSingle(); 
                                float texPosY = br.ReadSingle();

                                float texScaleX = br.ReadSingle(); 
                                float texScaleY = br.ReadSingle();

                                float texRotX = br.ReadSingle(); 
                                float texRotY = br.ReadSingle();

                                float uTexPlane0 = br.ReadSingle(); 
                                float uTexPlane1 = br.ReadSingle(); 
                                float uTexPlane2 = br.ReadSingle(); 
                                float uTexPlane3 = br.ReadSingle(); // Unused.

                                float vTexPlane0 = br.ReadSingle(); 
                                float vTexPlane1 = br.ReadSingle(); 
                                float vTexPlane2 = br.ReadSingle(); 
                                float vTexPlane3 = br.ReadSingle(); // Unused.

                                float luxelSize = br.ReadSingle(); // Unused too.

                                int smoothGroupInd = br.ReadInt32(); // This is ALSO unused.

                                int materialInd = br.ReadInt32() - 1;
                                
                                if ((faceFlags & 16) != 0)
                                {
                                    int lightmapInd = br.ReadInt32(); // Unused.
                                }

                                byte indexCount = br.ReadByte();
                                List<byte> vertsInFace = new List<byte>();
                                for (int k = 0; k < indexCount; k++)
                                {
                                    byte vertIndex = br.ReadByte();

                                    vertsInFace.Add(vertIndex);

                                    // Unused.
                                    float texCoordX = br.ReadSingle(); 
                                    float texCoordY = br.ReadSingle();
                                    
                                    // Unused.
                                    if ((faceFlags & 16) != 0)
                                    {
                                        float lmCoordX = br.ReadSingle(); 
                                        float lmCoordY = br.ReadSingle();
                                    }
                                }

                                Vector3 planeNormal = new Vector3(planeEq0, planeEq2, planeEq1);

                                if (Math.Abs(planeNormal.LengthSquared()) > 0.001f)
                                {
                                    if (Math.Abs((double)planeNormal.LengthSquared() - 1) > 0.001) throw new Exception(planeNormal.LengthSquared().ToString());

                                    Face newFace = new Face(map.NumberGenerator.Next("Face"));

                                    foreach (byte vertInd in vertsInFace)
                                    {
                                        newFace.Vertices.Insert(0, vertices[vertInd]);
                                    }

                                    newFace.Plane = new Plane(newFace.Vertices[0], newFace.Vertices[1], newFace.Vertices[2]);

                                    Vector3 uNorm = new Vector3(uTexPlane0, uTexPlane2, uTexPlane1).Normalise();
                                    Vector3 vNorm = new Vector3(vTexPlane0, vTexPlane2, vTexPlane1).Normalise();
                                    if (Math.Abs((double)(uNorm.LengthSquared() - vNorm.LengthSquared())) > 0.001) throw new Exception(uNorm.LengthSquared() + " " + vNorm.LengthSquared());

                                    newFace.Texture.Name = (faceFlags & 4) != 0 ? "tooltextures/remove_face" :
                                                            invisibleCollision ? "tooltextures/invisible_collision" :
                                                                                  materialList[materialInd];

                                    newFace.Texture.UAxis = uNorm * (float)Math.Cos(-texRotX * Math.PI / 180.0) + vNorm * (float)Math.Sin(-texRotX * Math.PI / 180.0);
                                    newFace.Texture.VAxis = vNorm * (float)Math.Cos(-texRotX * Math.PI / 180.0) - uNorm * (float)Math.Sin(-texRotX * Math.PI / 180.0);

                                    // What the fuck?
                                    if (Math.Abs(texScaleX) < 0.0001f)
                                    {
                                        if (Math.Abs(texScaleY) < 0.0001f)
                                        {
                                            texScaleX = 1f;
                                            texScaleY = 1f;
                                        }
                                        else
                                        {
                                            texScaleX = texScaleY;
                                        }
                                    }
                                    else if (Math.Abs(texScaleY) < 0.0001f)
                                    {
                                        texScaleY = texScaleX;
                                    }
                                    newFace.Texture.XScale = texScaleX / 2;
                                    newFace.Texture.YScale = texScaleY / 2;
                                    newFace.Texture.XShift = -texPosX * 2 / texScaleX;
                                    newFace.Texture.YShift = texPosY * 2 / texScaleY;
                                    newFace.Texture.Rotation = texRotX;

                                    // Seriously, what the FUCK???????????
                                    if ((texRotX - texRotY) > 120.0f)
                                    {
                                        newFace.Texture.XScale *= -1f;
                                        newFace.Texture.YScale *= -1f;
                                        newFace.Texture.Rotation -= 180f;
                                        newFace.Texture.UAxis = -newFace.Texture.UAxis;
                                    }
                                    else if ((texRotY - texRotX) > 120.0f)
                                    {
                                        newFace.Texture.XScale *= -1f;
                                        newFace.Texture.YScale *= -1f;
                                        newFace.Texture.Rotation -= 180f;
                                        newFace.Texture.VAxis = -newFace.Texture.VAxis;
                                    }

                                    faces.Add(newFace);
                                }
                            }

                            Solid newSolid = new Solid(map.NumberGenerator.Next("MapObject"));

                            foreach (Face face in faces)
                            {
                                newSolid.Data.Add(face); // Add the face to the solid.
                            }

                            if (visgroupTable.ContainsKey(visgroupIndex))
                            {
                                // Add the solid to the visgroup.
                                newSolid.Data.Add(new VisgroupID(visgroupTable[visgroupIndex]));
                            }

                            // Add a random brush color.
                            newSolid.Data.Add(new ObjectColor(Colour.GetRandomBrushColour()));

                            IMapObject parent = map.Root;
                            if (groupTable.ContainsKey(groupIndex))
                            {
                                parent = groupTable[groupIndex];
                            }

                            if (newSolid.IsValid())
                            {
                                newSolid.Hierarchy.Parent = parent;
                            }
                            else
                            {
                                // Not a valid solid, decompose into tetrahedrons/etc
                                foreach (Face face in faces)
                                {
                                    Polygon polygon = face.ToPolygon();
                                    if (!polygon.IsValid() || !polygon.IsConvex())
                                    {
                                        // tetrahedrons
                                        foreach (Vector3[] triangle in GetTriangles(face))
                                        {
                                            Face tf = new Face(map.NumberGenerator.Next("Face"))
                                            {
                                                Plane = new Plane(triangle[0], triangle[1], triangle[2])
                                            };
                                            tf.Vertices.AddRange(triangle);

                                            newSolid = SolidifyFace(map, tf);
                                            newSolid.Hierarchy.Parent = parent;
                                        }
                                    }
                                    else
                                    {
                                        // cone/pyramid/whatever
                                        newSolid = SolidifyFace(map, face);
                                        newSolid.Hierarchy.Parent = parent;
                                    }
                                }
                            }
                        }
                        else
                        {
                            br.BaseStream.Seek(size, SeekOrigin.Current);
                        }
                    }

                    result.Map = map;

                    return result;
                }
            });
        }

        public Task Save(Stream stream, Map map)
        {
            throw new NotImplementedException("Saving to .3dw is unsupported.");
        }

        private IEnumerable<Vector3[]> GetTriangles(Face face)
        {
            for (var i = 1; i < face.Vertices.Count - 1; i++)
            {
                yield return new[]
                {
                    face.Vertices[0],
                    face.Vertices[i],
                    face.Vertices[i + 1]
                };
            }
        }

        private Solid SolidifyFace(Map map, Face face)
        {
            Solid solid = new Solid(map.NumberGenerator.Next("MapObject"));
            solid.Data.Add(new ObjectColor(Colour.GetRandomBrushColour()));
            solid.Data.Add(face);

            Vector3 center = face.Vertices.Aggregate(Vector3.Zero, (sum, v) => sum + v) / face.Vertices.Count;
            Vector3 offset = center - face.Plane.Normal * 5;
            for (var i = 0; i < face.Vertices.Count; i++)
            {
                Vector3 v1 = face.Vertices[i];
                Vector3 v2 = face.Vertices[(i + 1) % face.Vertices.Count];

                Face newFace = new Face(map.NumberGenerator.Next("Face"))
                {
                    Plane = new Plane(v1, offset, v2)
                };

                newFace.Vertices.Add(offset);
                newFace.Vertices.Add(v2);
                newFace.Vertices.Add(v1);

                solid.Data.Add(newFace);
            }

            solid.DescendantsChanged();

            return solid;
        }
    }
}
