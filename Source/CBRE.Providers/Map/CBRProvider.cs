using CBRE.Common;
using CBRE.DataStructures.GameData;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Localization;
using CBRE.Providers.Texture;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace CBRE.Providers.Map
{
    public class CBRProvider : MapProvider
    {
        private const uint revision = 0;

        // Hierarchy control bytes
        // bytes to avoid enum cast
        private const byte HIERARCHY_PROCCEED = 0;
        private const byte HIERARCHY_DOWN = 1;
        private const byte HIERARCHY_UP = 2; // Should be highest hierarchy byte, smaller than identifiers

        private const byte IDENTIFIER_SOLID = 3;
        private const byte IDENTIFIER_ENTITY = 4;

        private enum Lightmapped : byte
        {
            No = 0,
            Fully = 1,
            Outdated = 2,
        }

        private struct Property
        {
            public string name;
            public VariableType type;
        }

        protected override IEnumerable<MapFeature> GetFormatFeatures()
        {
            return new[] {
                MapFeature.Solids,
                MapFeature.Entities,
                MapFeature.Displacements, // TODO
                MapFeature.Groups,
                MapFeature.SingleVisgroups,
                MapFeature.MultipleVisgroups,
            };
        }

        protected override bool IsValidForFileName(string filename)
        {
            return filename.EndsWith(".cbr", StringComparison.OrdinalIgnoreCase);
        }

        protected override DataStructures.MapObjects.Map GetFromStream(Stream stream, IEnumerable<string> modelDirs, out Image[] lightmaps)
        {
            BinaryReader reader = new BinaryReader(stream);

            if (reader.ReadFixedLengthString(Encoding.ASCII, 3) != "CBR")
            {
                throw new ProviderException(Local.LocalString("exception.cbr_corrupted"));
            }
            uint revision = reader.ReadUInt32();

            // Lightmaps
            bool lightmapped = reader.ReadByte() > (byte)Lightmapped.No;
            if (lightmapped)
            {
                lightmaps = new Image[4];
                for (int i = 0; i < 4; i++)
                {
                    lightmaps[i] = Image.FromStream(new MemoryStream(reader.ReadBytes(reader.ReadInt32())));
                }
            }
            else
            {
                lightmaps = null;
            }

            // Texture dictionary
            int texSize = reader.ReadInt32();
            string[] textures = new string[texSize];
            for (int i = 0; i < texSize; i++)
            {
                textures[i] = reader.ReadNullTerminatedString();
            }

            DataStructures.MapObjects.Map map = new DataStructures.MapObjects.Map();
            map.WorldSpawn = new World(map.IDGenerator.GetNextObjectID());

            // Solids
            List<MapObject> solids = new List<MapObject>();
            int solidCount = reader.ReadInt32();
            for (int i = 0; i < solidCount; i++)
            {
                Solid s = new Solid(map.IDGenerator.GetNextObjectID());
                s.Colour = Colour.GetRandomBrushColour();
                int faceCount = reader.ReadInt32();
                for (int j = 0; j < faceCount; j++)
                {
                    Face f = new Face(map.IDGenerator.GetNextFaceID());
                    f.Colour = s.Colour;
                    f.Texture.Name = textures[reader.ReadInt32()];
                    f.Texture.UAxis = reader.ReadCoordinate();
                    f.Texture.VAxis = reader.ReadCoordinate();
                    f.Texture.XShift = reader.ReadDecimal();
                    f.Texture.YShift = reader.ReadDecimal();
                    f.Texture.XScale = reader.ReadDecimal();
                    f.Texture.YScale = reader.ReadDecimal();
                    f.Texture.Rotation = reader.ReadDecimal();
                    int vertexCount = reader.ReadInt32();
                    for (int k = 0; k < vertexCount; k++)
                    {
                        Vertex v = new Vertex(reader.ReadCoordinate(), f);
                        if (lightmapped)
                        {
                            v.LMU = reader.ReadSingle();
                            v.LMV = reader.ReadSingle();
                            v.TextureU = (decimal)reader.ReadSingle();
                            v.TextureV = (decimal)reader.ReadSingle();
                        }
                        f.Vertices.Add(v);
                    }
                    f.Plane = new Plane(f.Vertices[0].Location, f.Vertices[1].Location, f.Vertices[2].Location);
                    f.Parent = s;
                    s.Faces.Add(f);
                    f.UpdateBoundingBox();
                }
                s.SetParent(map.WorldSpawn, false);
                s.UpdateBoundingBox(false);
                solids.Add(s);
            }

            // Entities
            List<MapObject> entities = new List<MapObject>(0);
            string read;
            bool isStillSolid = true;
            for (int i = 0; i < 2; i++)
            {
                while ((read = reader.ReadNullTerminatedString()) != "")
                {
                    List<Property> properties = new List<Property>();
                    byte propertyType;
                    while ((propertyType = reader.ReadByte()) != 255)
                    {
                        properties.Add(new Property()
                        {
                            name = reader.ReadNullTerminatedString(),
                            type = (VariableType)propertyType
                        });
                    }

                    // Entries
                    int entitiesOfType = reader.ReadInt32();
                    entities.Capacity += entitiesOfType;
                    for (int j = 0; j < entitiesOfType; j++)
                    {
                        Entity e = new Entity(map.IDGenerator.GetNextObjectID());
                        e.Colour = Colour.GetDefaultEntityColour();
                        e.ClassName = read;
                        e.EntityData.Name = read;
                        if (isStillSolid)
                        {
                            int entitySolids = reader.ReadInt32();
                            for (int k = 0; k < entitySolids; k++)
                            {
                                solids[reader.ReadInt32()].SetParent(e);
                            }
                        }
                        e.SetParent(map.WorldSpawn);
                        foreach (Property property in properties)
                        {
                            string propertyVal;
                            switch (property.type)
                            {
                                case VariableType.Bool:
                                    propertyVal = reader.ReadBoolean() ? "Yes" : "No";
                                    break;
                                case VariableType.Color255:
                                    propertyVal = DataStructures.MapObjects.Property.FromColor(reader.ReadRGBAColour());
                                    break;
                                case VariableType.Float:
                                    propertyVal = reader.ReadSingle().ToString();
                                    break;
                                case VariableType.Integer:
                                    propertyVal = reader.ReadInt32().ToString();
                                    break;
                                case VariableType.String:
                                    propertyVal = reader.ReadNullTerminatedString();
                                    break;
                                case VariableType.Vector:
                                    propertyVal = DataStructures.MapObjects.Property.FromCoordinate(reader.ReadCoordinate());
                                    break;
                                case VariableType.Choices:
                                    // TODO: Bullshit
                                    throw new NotImplementedException();
                                default:
                                    propertyVal = "";
                                    break;
                            }
                            e.EntityData.SetPropertyValue(property.name, propertyVal);
                        }
                        e.UpdateBoundingBox();
                        entities.Add(e);
                    }
                }
                isStillSolid = false;
            }

            // CBRE ONLY

            // Visgroup dictionary
            Visgroup currentParentVisgroup = null;
            while (true)
            {
                byte hierarchyControl;
                Visgroup newGroup = null;
                while ((hierarchyControl = reader.ReadByte()) == HIERARCHY_PROCCEED)
                {
                    newGroup = new Visgroup();
                    newGroup.Colour = Colour.GetRandomBrushColour();
                    newGroup.ID = reader.ReadInt32();
                    newGroup.Name = reader.ReadNullTerminatedString();
                    if (currentParentVisgroup != null)
                    {
                        newGroup.Parent = currentParentVisgroup;
                        currentParentVisgroup.Children.Add(newGroup);
                    }
                    else
                    {
                        map.Visgroups.Add(newGroup);
                    }
                }
                if (hierarchyControl == HIERARCHY_DOWN)
                {
                    currentParentVisgroup = newGroup;
                }
                else if (currentParentVisgroup != null)
                {
                    currentParentVisgroup = currentParentVisgroup.Parent;
                }
                else
                {
                    break;
                }
            }

            // Solid visgroups
            foreach (Solid mo in solids)
            {
                ReadVisgroups(reader, mo);
            }

            // Entity visgroups
            foreach (Entity e in entities)
            {
                ReadVisgroups(reader, e);
            }

            // Groups
            int directWorldGroups = reader.ReadInt32();
            for (int i = 0; i < directWorldGroups; i++)
            {
                Group currentParentGroup = new Group(map.IDGenerator.GetNextObjectID());
                currentParentGroup.SetParent(map.WorldSpawn);
                while (true)
                {
                    byte hierarchyControl;
                    while ((hierarchyControl = reader.ReadByte()) > HIERARCHY_UP)
                    {
                        if (hierarchyControl == IDENTIFIER_ENTITY)
                        {
                            entities[reader.ReadInt32()].SetParent(currentParentGroup);
                        }
                        else
                        {
                            solids[reader.ReadInt32()].SetParent(currentParentGroup);
                        }
                    }
                    if (hierarchyControl == HIERARCHY_DOWN)
                    {
                        Group newGroup = new Group(map.IDGenerator.GetNextObjectID());
                        newGroup.SetParent(currentParentGroup);
                        currentParentGroup = newGroup;
                    }
                    else if (currentParentGroup.Parent != map.WorldSpawn)
                    {
                        currentParentGroup = (Group)currentParentGroup.Parent;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            stream.Close();
            return map;
        }

        private void ReadVisgroups(BinaryReader reader, MapObject mo)
        {
            int visNum = reader.ReadInt32();
            for (int i = 0; i < visNum; i++)
            {
                mo.Visgroups.Add(reader.ReadInt32());
            }
        }

        protected override void SaveToStream(Stream stream, DataStructures.MapObjects.Map map, DataStructures.GameData.GameData gameData, TextureCollection textureCollection)
        {
            BinaryWriter writer = new BinaryWriter(stream);

            writer.WriteFixedLengthString(Encoding.ASCII, 3, "CBR");
            writer.Write(revision);

            // Lightmaps
            bool lightmapped = textureCollection != null && textureCollection.Lightmaps[0] != null;
            if (lightmapped)
            {
                writer.Write((byte)Lightmapped.Fully); // TODO: Determine changes from last render
                for (int i = 0; i < 4; i++)
                {
                    long prevPos = writer.Seek(0, SeekOrigin.Current);
                    writer.Write(0);
                    writer.Flush();
                    textureCollection.Lightmaps[i].Save(stream, ImageFormat.Png);
                    int imageOffset = (int)(writer.Seek(0, SeekOrigin.Current) - prevPos);
                    writer.Seek(-imageOffset, SeekOrigin.Current);
                    writer.Write(imageOffset - sizeof(int));
                    writer.Seek(0, SeekOrigin.End);
                }
            }
            else
            {
                writer.Write((byte)Lightmapped.No);
            }

            // Texture dictionary
            Dictionary<string, int> texDic = new Dictionary<string, int>();
            StringBuilder texBuilder = new StringBuilder();
            int texCount = 0;
            IEnumerator<string> textures = map.GetAllTextures().GetEnumerator();
            while (textures.MoveNext())
            {
                texBuilder.Append(textures.Current);
                texBuilder.Append('\0');
                texDic.Add(textures.Current, texCount);
                texCount++;
            }
            writer.Write(texCount);
            writer.WriteFixedLengthString(Encoding.UTF8, texBuilder.Length, texBuilder.ToString());

            // Solids
            List<MapObject> solids = map.WorldSpawn.Find(x => x is Solid);
            writer.Write(solids.Count);
            foreach (Solid s in solids)
            {
                writer.Write(s.Faces.Count);
                foreach (Face f in s.Faces)
                {
                    writer.Write(texDic[f.Texture.Name]);
                    writer.WriteCoordinate(f.Texture.UAxis);
                    writer.WriteCoordinate(f.Texture.VAxis);
                    writer.Write(f.Texture.XShift);
                    writer.Write(f.Texture.YShift);
                    writer.Write(f.Texture.XScale);
                    writer.Write(f.Texture.YScale);
                    writer.Write(f.Texture.Rotation);
                    writer.Write(f.Vertices.Count);
                    foreach (Vertex v in f.Vertices)
                    {
                        writer.WriteCoordinate(v.Location);
                        if (lightmapped)
                        {
                            writer.Write(v.LMU);
                            writer.Write(v.LMV);
                            writer.Write((float)v.TextureU);
                            writer.Write((float)v.TextureV);
                        }
                    }
                }
            }

            // Entities
            ISet<string> foundEntityTypes = new HashSet<string>();
            List<GameDataObject> entityTypes = new List<GameDataObject>();
            List<MapObject> entites = map.WorldSpawn.Find(x => x is Entity && x.ClassName != "");
            foreach (Entity e in entites)
            {
                Console.WriteLine(e.ClassName);
                Console.WriteLine(e.GameData.Name);
                if (!foundEntityTypes.Contains(e.ClassName))
                {
                    GameDataObject gdo = gameData.Classes.Find(x => x.Name == e.ClassName);
                    entityTypes.Add(gdo);
                    foundEntityTypes.Add(gdo.Name);
                }
            }
            // Move brush entities to front
            entityTypes.Sort((x, y) => (x.ClassType == ClassType.Solid ? -1 : 0));
            // For later use with groups
            Dictionary<Entity, int> entityIndices = new Dictionary<Entity, int>();
            int entityIndicesCounter = 0;
            bool reachedRegular = false;
            foreach (GameDataObject gdo in entityTypes)
            {
                if (!reachedRegular && gdo.ClassType != ClassType.Solid)
                {
                    reachedRegular = true;
                    writer.WriteNullTerminatedString("");
                }
                writer.WriteNullTerminatedString(gdo.Name);
                foreach (DataStructures.GameData.Property p in gdo.Properties)
                {
                    writer.Write((byte)p.VariableType);
                    writer.WriteNullTerminatedString(p.Name); // Switch from brush to regular entities
                }
                writer.Write((byte)255); // Property end byte

                // Entries
                List<MapObject> entitiesOfType = entites.FindAll(x => x.ClassName == gdo.Name);
                writer.Write(entitiesOfType.Count);
                foreach (Entity e in entitiesOfType)
                {
                    entityIndices.Add(e, entityIndicesCounter++);
                    if (e.GameData.ClassType == ClassType.Solid)
                    {
                        IEnumerable<MapObject> children = e.GetChildren();
                        writer.Write(children.Count());
                        foreach (MapObject mo in children)
                        {
                            int index = solids.FindIndex(x => x == mo);
                            writer.Write(index);
                        }
                    }
                    for (int i = 0; i < gdo.Properties.Count; i++)
                    {
                        DataStructures.MapObjects.Property property;
                        if (i < e.EntityData.Properties.Count && gdo.Properties[i].Name == e.EntityData.Properties[i].Key)
                        {
                            property = e.EntityData.Properties[i];
                        }
                        else
                        {
                            property = e.EntityData.Properties.Find(x => x.Key == gdo.Properties[i].Name);
                            if (property == null)
                            {
                                property = new DataStructures.MapObjects.Property();
                                property.Key = gdo.Properties[i].Name;
                                property.Value = gdo.Properties[i].DefaultValue;
                            }
                        }
                        switch (gdo.Properties[i].VariableType)
                        {
                            case VariableType.Bool:
                                writer.Write(property.Value == "Yes");
                                break;
                            case VariableType.Color255:
                                writer.WriteRGBAColour(property.GetColour(Color.Black));
                                break;
                            case VariableType.Float:
                                writer.Write(float.Parse(property.Value));
                                break;
                            case VariableType.Integer:
                                writer.Write(int.Parse(property.Value));
                                break;
                            case VariableType.String:
                                // TODO: Implement dictionary.
                                writer.WriteNullTerminatedString(property.Value);
                                break;
                            case VariableType.Vector:
                                writer.WriteCoordinate(property.GetCoordinate(Coordinate.Zero));
                                break;
                            case VariableType.Choices:
                                bool found = false;
                                for (int j = 0; j < gdo.Properties[i].Options.Count; j++)
                                {
                                    if (property.Value == gdo.Properties[i].Options[j].Key)
                                    {
                                        writer.Write((byte)j);
                                        found = true;
                                        break;
                                    }
                                }
                                if (!found)
                                {
                                    writer.Write((byte)255);
                                }
                                break;
                        }
                    }
                }
            }
            writer.WriteNullTerminatedString("");

            // CBRE ONLY

            // Visgroup dictionary
            // TODO: Visgroup IDs to indices
            Stack<IEnumerator<Visgroup>> visStack = new Stack<IEnumerator<Visgroup>>();
            visStack.Push(map.Visgroups.GetEnumerator());
            while (visStack.Count > 0)
            {
                IEnumerator<Visgroup> v = visStack.Pop();
                while (v.MoveNext())
                {
                    if (!v.Current.IsAutomatic)
                    {
                        writer.Write(HIERARCHY_PROCCEED);
                        writer.Write(v.Current.ID);
                        writer.WriteNullTerminatedString(v.Current.Name);
                        if (v.Current.Children.Count != 0)
                        {
                            writer.Write(HIERARCHY_DOWN);
                            visStack.Push(v);
                            v = v.Current.Children.GetEnumerator();
                        }
                    }
                }
                writer.Write(HIERARCHY_UP);
            }

            // Solid visgroups
            foreach (Solid s in map.WorldSpawn.FindAll().OfType<Solid>())
            {
                WriteVisgroups(writer, s);
            }

            // Entity visgroups
            foreach (GameDataObject entityType in entityTypes)
            {
                foreach (Entity e in entites.FindAll(x => x.ClassName == entityType.Name))
                {
                    WriteVisgroups(writer, e);
                }
            }

            // Groups
            IEnumerable<Group> groups = map.WorldSpawn.GetChildren().OfType<Group>();
            writer.Write(groups.Count());
            foreach (Group g in groups)
            {
                Stack<IEnumerator<MapObject>> groupStack = new Stack<IEnumerator<MapObject>>();
                groupStack.Push(g.GetChildren().GetEnumerator());
                while (groupStack.Count > 0)
                {
                    IEnumerator<MapObject> gg = groupStack.Pop();
                    while (gg.MoveNext())
                    {
                        if (gg.Current is Group)
                        {
                            writer.Write(HIERARCHY_DOWN);
                            groupStack.Push(gg);
                            gg = gg.Current.GetChildren().GetEnumerator();
                        }
                        else if (gg.Current is Entity)
                        {
                            writer.Write(IDENTIFIER_ENTITY);
                            writer.Write(entityIndices[(Entity)gg.Current]);
                        }
                        else
                        {
                            writer.Write(IDENTIFIER_SOLID);
                            writer.Write(solids.FindIndex(x => x == gg.Current));
                        }
                    }
                    writer.Write(HIERARCHY_UP);
                }
            }

            stream.Close();
        }

        private void WriteVisgroups(BinaryWriter writer, MapObject mo)
        {
            IEnumerable<int> visgroupIDs = mo.Visgroups.Except(mo.AutoVisgroups);
            writer.Write(visgroupIDs.Count());
            foreach (int v in visgroupIDs)
            {
                writer.Write(v);
            }
        }
    }
}
