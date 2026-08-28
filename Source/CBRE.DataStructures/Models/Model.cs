using CBRE.DataStructures.Geometric;
using CBRE.Localization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace CBRE.DataStructures.Models
{
    public class Model : IDisposable
    {
        public string Name { get; set; }
        public List<Bone> Bones { get; private set; }
        public List<BodyPart> BodyParts { get; private set; }
        public List<Animation> Animations { get; private set; }
        public List<Texture> Textures { get; set; }
        public bool BonesTransformMesh { get; set; }
        private bool _preprocessed;

        private Box _boundingBox;

        public Model()
        {
            Bones = new List<Bone>();
            BodyParts = new List<BodyPart>();
            Animations = new List<Animation>();
            Textures = new List<Texture>();
            _preprocessed = false;

        }

        public Box GetBoundingBox()
        {
            if (_boundingBox == null)
            {
                List<MatrixF> transforms = GetTransforms();
                IEnumerable<Coordinate> list =
                    from mesh in GetActiveMeshes()
                    from vertex in mesh.Vertices
                    let transform = transforms[vertex.BoneWeightings.First().Bone.BoneIndex]
                    let cf = vertex.Location * transform
                    select new Coordinate((decimal)cf.X, (decimal)cf.Y, (decimal)cf.Z);
                _boundingBox = new Box(list);
            }
            return _boundingBox;
        }

        public IEnumerable<Mesh> GetActiveMeshes()
        {
            return BodyParts.SelectMany(x => x.GetActiveGroup());
        }

        public void AddMesh(string bodyPartName, int groupid, Mesh mesh)
        {
            BodyPart g = BodyParts.FirstOrDefault(x => x.Name == bodyPartName);
            if (g == null)
            {
                g = new BodyPart(bodyPartName);
                BodyParts.Add(g);
            }
            g.AddMesh(groupid, mesh);
        }

        public List<MatrixF> GetTransforms(int animation = 0, int frame = 0)
        {
            if (Animations.Count > animation && animation >= 0)
            {
                Animation ani = Animations[animation];
                if (ani.Frames.Count > 0)
                {
                    if (frame < 0 || frame >= ani.Frames.Count) frame = 0;
                    AnimationFrame frm = ani.Frames[frame];
                    return frm.GetBoneTransforms(BonesTransformMesh, !BonesTransformMesh);
                }
            }
            return Bones.Select(x => x.Transform).ToList();
        }

        /// <summary>
        /// Preprocess the model for rendering purposes.
        /// Normalises the texture coordinates,
        /// pre-computes chrome texture values, and
        /// combines all the textures into a single bitmap.
        /// </summary>
        public void PreprocessModel()
        {
            if (_preprocessed) return;
            _preprocessed = true;

            PreCalculateChromeCoordinates();
            CombineTextures();
            NormaliseTextureCoordinates();
        }

        /// <summary>
        /// Combines the textures in this model into one bitmap and modifies all the referenced skins and texture coordinates to use the combined texture.
        /// This modifies the model object.
        /// </summary>
        private void CombineTextures()
        {
            if (Textures.Count < 1) return;
            // Calculate the dimension of the combined texture
            int width = 0;
            int height = 0;
            Dictionary<int, int> heightList = new Dictionary<int, int>();
            foreach (Texture texture in Textures)
            {
                width = Math.Max(texture.Width, width);
                heightList.Add(texture.Index, height);
                height += texture.Height;
            }

            // Create the combined texture and draw all the textures onto it
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int y = 0;
            foreach (Texture texture in Textures)
            {
                BitmapData bmpData2 = texture.Image.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] bytes = new byte[bmpData2.Width * bmpData2.Height * Bitmap.GetPixelFormatSize(PixelFormat.Format32bppArgb) / 8];
                Marshal.Copy(bmpData2.Scan0, bytes, 0, bytes.Length);
                Marshal.Copy(bytes, 0, bmpData.Scan0 + (bmpData.Width * y * Bitmap.GetPixelFormatSize(PixelFormat.Format32bppArgb) / 8), bytes.Length);
                y += texture.Height;
                texture.Image.UnlockBits(bmpData2);
            }
            bmp.UnlockBits(bmpData);

            // Create the texture object and replace the existing textures
            Texture tex = new Texture
            {
                Flags = Textures[0].Flags,
                Height = height,
                Width = width,
                Image = bmp,
                Index = 0,
                Name = Local.LocalString("data.combined_texture")
            };
            foreach (Texture texture in Textures)
            {
                texture.Image.Dispose();
            }
            Textures.Clear();
            Textures.Insert(0, tex);

            // Update all the meshes with the new texture and alter the texture coordinates as needed
            foreach (Mesh mesh in GetActiveMeshes())
            {
                if (!heightList.ContainsKey(mesh.SkinRef))
                {
                    mesh.SkinRef = -1;
                    continue;
                }
                int i = mesh.SkinRef;
                int yVal = heightList[i];
                foreach (MeshVertex v in mesh.Vertices)
                {
                    v.TextureV += yVal;
                }
                mesh.SkinRef = 0;
            }
            // Reset the texture indices
            for (int i = 0; i < Textures.Count; i++)
            {
                Textures[i].Index = i;
            }
        }

        /// <summary>
        /// Pre-calculates chrome texture values for the model.
        /// This operation modifies the model vertices.
        /// </summary>
        private void PreCalculateChromeCoordinates()
        {
            List<MatrixF> transforms = Bones.Select(x => x.Transform).ToList();
            foreach (IGrouping<int, Mesh> g in GetActiveMeshes().GroupBy(x => x.SkinRef))
            {
                Texture skin = Textures.FirstOrDefault(x => x.Index == g.Key);
                if (skin == null || (skin.Flags & 0x02) == 0) continue;
                foreach (MeshVertex v in g.SelectMany(m => m.Vertices))
                {
                    MatrixF transform = transforms[v.BoneWeightings.First().Bone.BoneIndex];

                    // Borrowed from HLMV's StudioModel::Chrome function
                    CoordinateF tmp = transform.Shift.Normalise();

                    // Using unitx for the "player right" vector
                    CoordinateF up = tmp.Cross(CoordinateF.UnitX).Normalise();
                    CoordinateF right = tmp.Cross(up).Normalise();

                    // HLMV is doing an inverse rotate (no translation),
                    // so we set the shift values to zero after inverting
                    MatrixF inv = transform.Inverse();
                    inv[12] = inv[13] = inv[14] = 0;
                    up = up * inv;
                    right = right * inv;

                    v.TextureU = (v.Normal.Dot(right) + 1) * 32;
                    v.TextureV = (v.Normal.Dot(up) + 1) * 32;
                }
            }
        }

        /// <summary>
        /// Normalises vertex texture coordinates to be between 0 and 1.
        /// This operation modifies the model vertices.
        /// </summary>
        private void NormaliseTextureCoordinates()
        {
            foreach (IGrouping<int, Mesh> g in GetActiveMeshes().GroupBy(x => x.SkinRef))
            {
                Texture skin = Textures.FirstOrDefault(x => x.Index == g.Key);
                if (skin == null) continue;
                foreach (MeshVertex v in g.SelectMany(m => m.Vertices))
                {
                    v.TextureU /= skin.Width;
                    v.TextureV /= skin.Height;
                }
            }
        }

        public void Dispose()
        {
            foreach (Texture t in Textures)
            {
                if (t.Image != null) t.Image.Dispose();
                if (t.TextureObject != null) t.TextureObject.Dispose();
            }
        }
    }
}