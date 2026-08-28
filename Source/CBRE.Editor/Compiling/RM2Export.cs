using CBRE.Common;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Documents;
using CBRE.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.Editor.Compiling
{
    public class RM2Export
    {
        public class Waypoint
        {
            public Waypoint(Entity ent)
            {
                Location = new CoordinateF(ent.Origin);
                Connections = new List<int>();
            }

            public List<int> Connections;
            public CoordinateF Location;
        }

        enum RM2Chunks
        {
            Textures = 1,
            VisibleGeometry = 2,
            InvisibleGeometry = 3,
            Waypoint = 4,
            PointLight = 5,
            Spotlight = 6,
            Prop = 7
        };

        enum RM2TextureLoadFlag
        {
            Opaque = 1,
            Alpha = 2
        };

        public static void SaveToFile(string filename, Document document, ExportForm form)
        {
            Map map = document.Map;
            string filepath = System.IO.Path.GetDirectoryName(filename);
            filename = System.IO.Path.GetFileName(filename);
            filename = System.IO.Path.GetFileNameWithoutExtension(filename) + ".rm2";
            string lmPath = System.IO.Path.GetFileNameWithoutExtension(filename) + "_lm";

            List<Lightmap.LMFace> faces; int lmCount;
            List<Lightmap.Light> lights;
            Lightmap.Lightmapper.Render(document, form, out faces, out lmCount);
            Lightmap.Light.FindLights(map, out lights);

            IEnumerable<Face> transparentFaces = map.WorldSpawn.Find(x => x is Solid).OfType<Solid>().SelectMany(x => x.Faces).Where(x =>
            {
                if (x.Texture?.Texture == null) return false;
                if (!x.Texture.Texture.HasTransparency()) return false;
                if (x.Texture.Name.Contains("tooltextures")) return false;

                return true;
            });

            IEnumerable<Face> invisibleCollisionFaces = map.WorldSpawn.Find(x => x is Solid).OfType<Solid>().SelectMany(x => x.Faces).Where(x => x.Texture.Name.ToLowerInvariant() == "tooltextures/invisible_collision");

            Lightmap.Lightmapper.SaveLightmaps(document, lmCount, filepath + "/" + lmPath, true);
            lmPath = System.IO.Path.GetFileName(lmPath);

            List<Waypoint> waypoints = map.WorldSpawn.Find(x => x.ClassName != null && x.ClassName.ToLower() == "waypoint").OfType<Entity>().Select(x => new Waypoint(x)).ToList();

            IEnumerable<Entity> props = map.WorldSpawn.Find(x => x.ClassName != null && x.ClassName.ToLower() == "model").OfType<Entity>();

            form.ProgressLog.Invoke((MethodInvoker)(() => form.ProgressLog.AppendText("\n" + Local.LocalString("progress.export.determining_waypoint"))));
            form.ProgressBar.Invoke((MethodInvoker)(() => form.ProgressBar.Value = 9100));

            for (int i = 0; i < waypoints.Count; i++)
            {
                for (int j = 0; j < waypoints.Count; j++)
                {
                    if (j > i)
                    {
                        waypoints[i].Connections.Add(j);
                    }
                    else if (j < i)
                    {
                        if (waypoints[j].Connections.Contains(i)) waypoints[i].Connections.Add(j);
                    }
                }
                foreach (Lightmap.LMFace face in faces)
                {
                    for (int j = 0; j < waypoints[i].Connections.Count; j++)
                    {
                        int connection = waypoints[i].Connections[j];
                        if (connection < i) continue;
                        LineF line1 = new LineF(waypoints[i].Location, waypoints[connection].Location);
                        LineF line2 = new LineF(waypoints[connection].Location, waypoints[i].Location);
                        if (face.GetIntersectionPoint(line1) != null || face.GetIntersectionPoint(line2) != null)
                        {
                            waypoints[i].Connections.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }

            FileStream stream = new FileStream(filepath + "/" + filename, FileMode.Create);
            BinaryWriter br = new BinaryWriter(stream);

            //header
            br.Write((byte)'.');
            br.Write((byte)'R');
            br.Write((byte)'M');
            br.Write((byte)'2');

            //textures
            List<Tuple<string, byte>> textures = new List<Tuple<string, byte>>();
            byte flag = (byte)RM2TextureLoadFlag.Opaque;
            foreach (Lightmap.LMFace face in faces)
            {
                if (!textures.Any(x => x.Item1 == face.Texture)) textures.Add(new Tuple<string, byte>(face.Texture, flag));
            }
            flag = (byte)RM2TextureLoadFlag.Alpha;
            foreach (Face face in transparentFaces)
            {
                if (!textures.Any(x => x.Item1 == face.Texture.Name)) textures.Add(new Tuple<string, byte>(face.Texture.Name, flag));
            }

            br.Write((byte)RM2Chunks.Textures);
            br.Write((byte)lmCount);
            br.Write((byte)textures.Count);
            foreach (Tuple<string, byte> tex in textures)
            {
                br.WriteByteString(tex.Item1);
                br.Write(tex.Item2);
            }

            //mesh
            int vertCount;
            int vertOffset;
            int triCount;

            for (int i = 0; i < textures.Count; i++)
            {
                for (int lmInd = 0; lmInd < lmCount; lmInd++)
                {
                    IEnumerable<Lightmap.LMFace> tLmFaces = faces.FindAll(x => x.Texture == textures[i].Item1 && x.LmIndex == lmInd);
                    vertCount = 0;
                    vertOffset = 0;
                    triCount = 0;

                    if (tLmFaces.Count() > 0)
                    {
                        foreach (Lightmap.LMFace face in tLmFaces)
                        {
                            vertCount += face.Vertices.Count;
                            triCount += face.GetTriangleIndices().Count() / 3;
                        }

                        br.Write((byte)RM2Chunks.VisibleGeometry);
                        br.Write((byte)i);
                        if (lmCount > 1)
                        {
                            br.Write((byte)lmInd);
                        }

                        if (vertCount > UInt16.MaxValue) throw new Exception(Local.LocalString("error.export.vertex_overflow"));
                        br.Write((UInt16)vertCount);
                        foreach (Lightmap.LMFace face in tLmFaces)
                        {
                            for (int j = 0; j < face.Vertices.Count; j++)
                            {
                                br.Write(face.Vertices[j].Location.X);
                                br.Write(face.Vertices[j].Location.Z);
                                br.Write(face.Vertices[j].Location.Y);

                                //br.Write((byte)255); //r
                                //br.Write((byte)255); //g
                                //br.Write((byte)255); //b

                                br.Write(face.Vertices[j].DiffU);
                                br.Write(face.Vertices[j].DiffV);

                                float lmMul = (lmCount > 1) ? 2.0f : 1.0f;
                                float uSub = ((lmInd % 2) > 0) ? 0.5f : 0.0f;
                                float vSub = ((lmInd / 2) > 0) ? 0.5f : 0.0f;

                                br.Write((face.Vertices[j].LMU - uSub) * lmMul);
                                br.Write((face.Vertices[j].LMV - vSub) * lmMul);
                            }
                        }
                        br.Write((UInt16)triCount);
                        foreach (Lightmap.LMFace face in tLmFaces)
                        {
                            foreach (uint ind in face.GetTriangleIndices())
                            {
                                br.Write((UInt16)(ind + vertOffset));
                            }

                            vertOffset += face.Vertices.Count;
                        }
                    }
                }

                IEnumerable<Face> tTrptFaces = transparentFaces.Where(x => x.Texture.Name == textures[i].Item1);
                vertCount = 0;
                vertOffset = 0;
                triCount = 0;

                if (tTrptFaces.Count() > 0)
                {
                    foreach (Face face in tTrptFaces)
                    {
                        vertCount += face.Vertices.Count;
                        triCount += face.GetTriangleIndices().Count() / 3;
                    }

                    br.Write((byte)RM2Chunks.VisibleGeometry);
                    br.Write((byte)i);

                    if (vertCount > UInt16.MaxValue) throw new Exception(Local.LocalString("error.export.vertex_overflow"));
                    br.Write((UInt16)vertCount);
                    foreach (Face face in tTrptFaces)
                    {
                        for (int j = 0; j < face.Vertices.Count; j++)
                        {
                            br.Write((float)face.Vertices[j].Location.X);
                            br.Write((float)face.Vertices[j].Location.Z);
                            br.Write((float)face.Vertices[j].Location.Y);

                            //vertex color is not used since we don't do vertex lighting anymore
                            //br.Write((byte)255); //r
                            //br.Write((byte)255); //g
                            //br.Write((byte)255); //b

                            br.Write((float)face.Vertices[j].TextureU);
                            br.Write((float)face.Vertices[j].TextureV);
                            br.Write(0.0f);
                            br.Write(0.0f);
                        }
                    }
                    br.Write((UInt16)triCount);
                    foreach (Face face in tTrptFaces)
                    {
                        foreach (uint ind in face.GetTriangleIndices())
                        {
                            br.Write((UInt16)(ind + vertOffset));
                        }

                        vertOffset += face.Vertices.Count;
                    }
                }
            }

            vertCount = 0;
            vertOffset = 0;
            triCount = 0;
            if (invisibleCollisionFaces.Count() > 0)
            {
                foreach (Face face in invisibleCollisionFaces)
                {
                    vertCount += face.Vertices.Count;
                    triCount += face.GetTriangleIndices().Count() / 3;
                }

                br.Write((byte)RM2Chunks.InvisibleGeometry);

                if (vertCount > UInt16.MaxValue) throw new Exception(Local.LocalString("error.export.vertex_overflow"));
                br.Write((UInt16)vertCount);
                foreach (Face face in invisibleCollisionFaces)
                {
                    for (int j = 0; j < face.Vertices.Count; j++)
                    {
                        br.Write((float)face.Vertices[j].Location.X);
                        br.Write((float)face.Vertices[j].Location.Z);
                        br.Write((float)face.Vertices[j].Location.Y);
                    }
                }
                br.Write((UInt16)triCount);
                foreach (Face face in invisibleCollisionFaces)
                {
                    foreach (uint ind in face.GetTriangleIndices())
                    {
                        br.Write((UInt16)(ind + vertOffset));
                    }

                    vertOffset += face.Vertices.Count;
                }
            }

            foreach (Lightmap.Light light in lights)
            {
                br.Write((byte)RM2Chunks.PointLight);

                br.Write(light.Origin.X);
                br.Write(light.Origin.Z);
                br.Write(light.Origin.Y);

                br.Write(light.Range);

                br.Write((byte)light.Color.X);
                br.Write((byte)light.Color.Y);
                br.Write((byte)light.Color.Z);
                br.Write(light.Intensity);
            }

            foreach (Waypoint wp in waypoints)
            {
                br.Write((byte)RM2Chunks.Waypoint);

                br.Write(wp.Location.X);
                br.Write(wp.Location.Z);
                br.Write(wp.Location.Y);

                for (int i = 0; i < wp.Connections.Count; i++)
                {
                    br.Write((byte)(wp.Connections[i] + 1));
                }
                br.Write((byte)0);
            }

            foreach (Entity prop in props)
            {
                br.Write((byte)RM2Chunks.Prop);

                br.WriteByteString(System.IO.Path.GetFileNameWithoutExtension(prop.EntityData.GetPropertyValue("file")));

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

            br.Dispose();
            stream.Dispose();

            form.ProgressLog.Invoke((MethodInvoker)(() => form.ProgressLog.AppendText("\n" + Local.LocalString("progress.export.done"))));
            form.ProgressBar.Invoke((MethodInvoker)(() => form.ProgressBar.Value = 10000));
        }
    }
}
