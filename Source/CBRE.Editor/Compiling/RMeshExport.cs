using CBRE.Common;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Compiling.Lightmap;
using CBRE.Editor.Documents;
using CBRE.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CBRE.Common.Extensions;
using CBRE.Localization;

namespace CBRE.Editor.Compiling
{
    public class RMeshExport
    {
        public class Waypoint
        {
            public Waypoint(Entity ent)
            {
                Location = new CoordinateF(ent.Origin);
            }

            public CoordinateF Location;
        }

        enum RMeshLoadFlags
        {
            COLOR = 1,
            ALPHA = 2
        };

        enum RMeshBlendFlags
        {
            NORMAL = 0,
            DIFFUSE = 1,
            LM = 2
        };

        public static void SaveToFile(string filename, Document document, ExportForm form)
        {
            Map map = document.Map;
            string filepath = System.IO.Path.GetDirectoryName(filename);
            filename = System.IO.Path.GetFileName(filename);
            filename = System.IO.Path.GetFileNameWithoutExtension(filename) + ".rmesh";
            string lmPath = System.IO.Path.GetFileNameWithoutExtension(filename) + "_lm";

            List<LMFace> faces; int lmCount;
            List<Light> lights;
            Lightmapper.Render(document, form, out faces, out lmCount);
            Light.FindLights(map, out lights);
            lights.RemoveAll(l => !l.HasSprite);

            IEnumerable<Face> transparentFaces = map.WorldSpawn.Find(x => x is Solid).OfType<Solid>().SelectMany(x => x.Faces).Where(x =>
            {
                if (x.Texture?.Texture == null) return false;
                if (!x.Texture.Texture.HasTransparency()) return false;
                if (x.Texture.Name.Contains("tooltextures")) return false;

                return true;
            });

            IEnumerable<Face> invisibleCollisionFaces = map.WorldSpawn.Find(x => x is Solid).OfType<Solid>().SelectMany(x => x.Faces).Where(x => x.Texture.Name.ToLowerInvariant() == "tooltextures/invisible_collision");

            Lightmapper.SaveLightmaps(document, lmCount, filepath + "/" + lmPath, false);
            lmPath = System.IO.Path.GetFileName(lmPath);

            List<Waypoint> waypoints = map.WorldSpawn.Find(x => x.ClassName != null && x.ClassName.ToLower() == "waypoint").OfType<Entity>()
                .Select(x => new Waypoint(x)).ToList();

            IEnumerable<Entity> soundEmitters = map.WorldSpawn.Find(x => x.ClassName != null && x.ClassName.ToLower() == "soundemitter").OfType<Entity>();

            IEnumerable<Entity> props = map.WorldSpawn.Find(x => x.ClassName != null && x.ClassName.ToLower() == "model").OfType<Entity>();

            IEnumerable<Entity> screens = map.WorldSpawn.Find(x => x.ClassName != null && x.ClassName.ToLower() == "screen").OfType<Entity>();

            IEnumerable<Entity> customEntities = map.WorldSpawn.Find(x => x.ClassName != null).OfType<Entity>()
                .Where(x => x.GameData != null && x.GameData.IsCustom);

            FileStream stream = new FileStream(filepath + "/" + filename, FileMode.Create);
            BinaryWriter br = new BinaryWriter(stream);

            //header
            br.WriteB3DString("RoomMesh");

            //textures
            List<Tuple<string, RMeshLoadFlags, RMeshBlendFlags, byte>> textures = new List<Tuple<string, RMeshLoadFlags, RMeshBlendFlags, byte>>();
            RMeshLoadFlags loadFlag = RMeshLoadFlags.COLOR; RMeshBlendFlags blendFlag = RMeshBlendFlags.DIFFUSE;
            foreach (LMFace face in faces)
            {
                if (!textures.Any(x => x.Item1 == face.Texture)) textures.Add(new Tuple<string, RMeshLoadFlags, RMeshBlendFlags, byte>(face.Texture, loadFlag, blendFlag, 0));
            }
            loadFlag = RMeshLoadFlags.ALPHA; blendFlag = RMeshBlendFlags.NORMAL;
            foreach (Face face in transparentFaces)
            {
                if (!textures.Any(x => x.Item1 == face.Texture.Name)) textures.Add(new Tuple<string, RMeshLoadFlags, RMeshBlendFlags, byte>(face.Texture.Name, loadFlag, blendFlag, 0));
            }

            //mesh

            int vertCount;
            int vertOffset;
            int triCount;

            //TODO: find a clever way of splitting up meshes with the same texture
            //into several for collision optimization.
            //Making each face its own collision object is too slow, and merging all of
            //them together is not optimal either.

            int texCount = 0;
            for (int i = 0; i < textures.Count; i++)
            {
                texCount += faces.Where(x => x.Texture == textures[i].Item1).Select(x => x.LmIndex).Distinct().Count();
                texCount += transparentFaces.Any(x => x.Texture.Name == textures[i].Item1) ? 1 : 0;
            }

            br.Write(texCount);

            for (int i = 0; i < textures.Count; i++)
            {
                string texName = Directories.GetTextureExtension(textures[i].Item1);

                for (int lmInd = 0; lmInd < lmCount; lmInd++)
                {
                    LMFace[] tLmFaces = faces.FindAll(x => x.Texture == textures[i].Item1 && x.LmIndex == lmInd).ToArray();
                    Face[] tTrptFaces = transparentFaces.Where(x => x.Texture.Name == textures[i].Item1).ToArray();
                    vertCount = 0;
                    vertOffset = 0;
                    triCount = 0;

                    if (tLmFaces.Length > 0)
                    {
                        foreach (LMFace face in tLmFaces)
                        {
                            vertCount += face.Vertices.Count;
                            triCount += face.GetTriangleIndices().Count() / 3;
                        }

                        byte flag = 1;
                        br.Write(flag);
                        string currLmPath = lmPath + (lmCount > 1 ? "_" + lmInd.ToString() : "");
                        currLmPath += ".png";
                        br.WriteB3DString(currLmPath);

                        flag = 1;
                        br.Write(flag);
                        br.WriteB3DString(texName);

                        if (vertCount > UInt16.MaxValue) throw new Exception(Local.LocalString("error.export.vertex_overflow_2", texName));
                        br.Write((Int32)vertCount);
                        foreach (LMFace face in tLmFaces)
                        {
                            for (int j = 0; j < face.Vertices.Count; j++)
                            {
                                br.Write(face.Vertices[j].Location.X);
                                br.Write(face.Vertices[j].Location.Z);
                                br.Write(face.Vertices[j].Location.Y);

                                br.Write(face.Vertices[j].DiffU);
                                br.Write(face.Vertices[j].DiffV);

                                float lmMul = (lmCount > 1) ? 2.0f : 1.0f;
                                float uSub = ((lmInd % 2) > 0) ? 0.5f : 0.0f;
                                float vSub = ((lmInd / 2) > 0) ? 0.5f : 0.0f;

                                br.Write((face.Vertices[j].LMU - uSub) * lmMul);
                                br.Write((face.Vertices[j].LMV - vSub) * lmMul);

                                br.Write((byte)255); //r
                                br.Write((byte)255); //g
                                br.Write((byte)255); //b
                            }
                        }
                        br.Write((Int32)triCount);
                        foreach (LMFace face in tLmFaces)
                        {
                            foreach (uint ind in face.GetTriangleIndices())
                            {
                                br.Write((Int32)(ind + vertOffset));
                            }

                            vertOffset += face.Vertices.Count;
                        }
                    }
                    else if (lmInd == 0 && tTrptFaces.Length > 0)
                    {
                        foreach (Face face in tTrptFaces)
                        {
                            vertCount += face.Vertices.Count;
                            triCount += face.GetTriangleIndices().Count() / 3;
                        }

                        byte flag = 0;
                        br.Write(flag);
                        flag = 3;
                        br.Write(flag);
                        br.WriteB3DString(texName);

                        if (vertCount > UInt16.MaxValue) throw new Exception(Local.LocalString("error.export.vertex_overflow"));
                        br.Write((Int32)vertCount);
                        foreach (Face face in tTrptFaces)
                        {
                            for (int j = 0; j < face.Vertices.Count; j++)
                            {
                                br.Write((float)face.Vertices[j].Location.X);
                                br.Write((float)face.Vertices[j].Location.Z);
                                br.Write((float)face.Vertices[j].Location.Y);

                                br.Write((float)face.Vertices[j].TextureU);
                                br.Write((float)face.Vertices[j].TextureV);
                                br.Write(0.0f);
                                br.Write(0.0f);

                                br.Write((byte)255); //r
                                br.Write((byte)255); //g
                                br.Write((byte)255); //b
                            }
                        }
                        br.Write((Int32)triCount);
                        foreach (Face face in tTrptFaces)
                        {
                            foreach (uint ind in face.GetTriangleIndices())
                            {
                                br.Write((Int32)(ind + vertOffset));
                            }

                            vertOffset += face.Vertices.Count;
                        }
                    }
                }
            }

            vertCount = 0;
            vertOffset = 0;
            triCount = 0;
            if (invisibleCollisionFaces.Count() > 0)
            {
                br.Write((Int32)1);

                foreach (Face face in invisibleCollisionFaces)
                {
                    vertCount += face.Vertices.Count;
                    triCount += face.GetTriangleIndices().Count() / 3;
                }

                if (vertCount > UInt16.MaxValue) throw new Exception(Local.LocalString("error.export.vertex_overflow"));
                br.Write((Int32)vertCount);
                foreach (Face face in invisibleCollisionFaces)
                {
                    for (int j = 0; j < face.Vertices.Count; j++)
                    {
                        br.Write((float)face.Vertices[j].Location.X);
                        br.Write((float)face.Vertices[j].Location.Z);
                        br.Write((float)face.Vertices[j].Location.Y);
                    }
                }
                br.Write((Int32)triCount);
                foreach (Face face in invisibleCollisionFaces)
                {
                    foreach (uint ind in face.GetTriangleIndices())
                    {
                        br.Write((Int32)(ind + vertOffset));
                    }

                    vertOffset += face.Vertices.Count;
                }
            }
            else
            {
                br.Write((Int32)0);
            }

            br.Write((lights.Count + waypoints.Count + soundEmitters.Count() + props.Count() + screens.Count()
                + customEntities.Count()));

            foreach (Light light in lights)
            {
                br.WriteB3DString("light");

                br.Write(light.Origin.X);
                br.Write(light.Origin.Z);
                br.Write(light.Origin.Y);

                br.Write(light.Range);

                string lcolor = light.Color.X + " " + light.Color.Y + " " + light.Color.Z;
                br.Write((Int32)lcolor.Length);
                for (int k = 0; k < lcolor.Length; k++)
                {
                    br.Write((byte)lcolor[k]);
                }

                br.Write(light.Intensity);
            }

            foreach (Waypoint waypoint in waypoints)
            {
                br.WriteB3DString("waypoint");

                br.Write(waypoint.Location.X);
                br.Write(waypoint.Location.Z);
                br.Write(waypoint.Location.Y);
            }

            foreach (Entity soundEmitter in soundEmitters)
            {
                br.WriteB3DString("soundemitter");

                br.Write((float)soundEmitter.Origin.X);
                br.Write((float)soundEmitter.Origin.Z);
                br.Write((float)soundEmitter.Origin.Y);

                br.Write(int.Parse(soundEmitter.EntityData.GetPropertyValue("sound")));

                br.Write(float.Parse(soundEmitter.EntityData.GetPropertyValue("range")));
            }

            foreach (Entity prop in props)
            {
                br.WriteB3DString("model");

                string modelName = prop.EntityData.GetPropertyValue("file") ?? "";
                if (!modelName.Contains('.'))
                {
                    modelName = System.IO.Path.GetFileName(Directories.GetModelPath(modelName)) ?? (modelName + ".x");
                }
                br.WriteB3DString(modelName);

                br.Write((float)prop.Origin.X);
                br.Write((float)prop.Origin.Z);
                br.Write((float)prop.Origin.Y);

                Coordinate rotation = prop.EntityData.GetPropertyCoordinate("angles");
                br.Write((float)rotation.X);
                br.Write((float)rotation.Y);
                br.Write((float)rotation.Z);

                Coordinate scale = prop.EntityData.GetPropertyCoordinate("scale");
                br.Write((float)scale.X);
                br.Write((float)scale.Y);
                br.Write((float)scale.Z);
            }

            foreach (Entity screen in screens)
            {
                br.WriteB3DString("screen");

                br.Write((float)screen.Origin.X);
                br.Write((float)screen.Origin.Z);
                br.Write((float)screen.Origin.Y);

                br.WriteB3DString(screen.EntityData.GetPropertyValue("imgpath"));
            }

            foreach (Entity customEntity in customEntities)
            {
                br.WriteB3DString(customEntity.ClassName);

                //Write position
                br.Write((float)customEntity.Origin.X);
                br.Write((float)customEntity.Origin.Z);
                br.Write((float)customEntity.Origin.Y);

                IEnumerable<DataStructures.GameData.Property> customEntityProperties = customEntity.GameData.Properties.Where(x => x.Name != "position");

                foreach (DataStructures.GameData.Property customEntityProperty in customEntityProperties)
                {
                    WriteCustomEntityProperty(customEntityProperty, ref br, customEntity);
                }
            }

            br.Dispose();
            stream.Dispose();

            form.ProgressLog.Invoke((MethodInvoker)(() => form.ProgressLog.AppendText("\n" + Local.LocalString("progress.export.done"))));
            form.ProgressBar.Invoke((MethodInvoker)(() => form.ProgressBar.Value = 10000));
        }

        //If juan sees this he would probably crush my neck
        private static void WriteCustomEntityProperty(DataStructures.GameData.Property property, ref BinaryWriter binaryWriter, Entity entity)
        {
            Property mapProperty = entity.EntityData.Properties.FirstOrDefault(x => x.Key == property.Name);

            if (mapProperty == default(Property))
                throw new Exception(Local.LocalString("error.export.property_not_found", property.Name, entity.ClassName, entity.Origin.X, entity.Origin.Y, entity.Origin.Z));
            
            switch (property.VariableType)
            {
                case DataStructures.GameData.VariableType.Bool:
                    binaryWriter.Write(mapProperty.Value?.ToBool() ?? false);
                    
                    break;
                case DataStructures.GameData.VariableType.Integer:
                    int.TryParse(mapProperty.Value, out int parsedInt);

                    binaryWriter.Write(parsedInt);
                    
                    break;
                case DataStructures.GameData.VariableType.Color255:
                    Coordinate colorCoord = entity.EntityData.GetPropertyCoordinate(property.Name, Coordinate.Zero);
                    string color = colorCoord.X + " " + colorCoord.Y + " " + colorCoord.Z;
                    
                    binaryWriter.WriteB3DString(color);
                    
                    break;
                case DataStructures.GameData.VariableType.Float:
                    float.TryParse(mapProperty.Value, out float parsedFloat);

                    binaryWriter.Write(parsedFloat);
                    
                    break;
                case DataStructures.GameData.VariableType.String:
                    binaryWriter.WriteB3DString(mapProperty.Value ?? string.Empty);
                    
                    break;
                case DataStructures.GameData.VariableType.Vector:
                    Coordinate coord = entity.EntityData.GetPropertyCoordinate(property.Name, Coordinate.Zero);
                    
                    binaryWriter.Write((float)coord.X);
                    binaryWriter.Write((float)coord.Y);
                    binaryWriter.Write((float)coord.Z);
                    
                    break;
            }
        }
    }
}
