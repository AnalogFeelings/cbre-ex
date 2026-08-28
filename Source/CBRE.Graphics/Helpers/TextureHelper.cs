using CBRE.Common;
using CBRE.Localization;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace CBRE.Graphics.Helpers
{
    /// <summary>
    /// Texture utilities. Borrows large amounts of code from
    /// the Texture Utility Library project for OpenTK:
    /// http://www.opentk.com/project/TexLib
    /// </summary>
    public static class TextureHelper
    {
        public static readonly Dictionary<string, GLTexture> Textures;

        private static PixelInternalFormat PixelInternalFormat { get; set; }

        public static bool EnableTransparency
        {
            get { return PixelInternalFormat == PixelInternalFormat.Rgba; }
            set { PixelInternalFormat = value ? PixelInternalFormat.Rgba : PixelInternalFormat.Rgb; }
        }

        public static bool ForceNonPowerOfTwoResize { get; set; }

        public static bool DisableTextureFiltering { get; set; }

        static TextureHelper()
        {
            Textures = new Dictionary<string, GLTexture>();
            PixelInternalFormat = PixelInternalFormat.Rgba;
        }

        public static void ClearLoadedTextures()
        {
            Textures.Values.ToList().ForEach(x => x.Dispose());
            Textures.Clear();
        }

        private static bool? _supportsNpot;

        public static bool SupportsNonPowerOfTwo()
        {
            if (!_supportsNpot.HasValue)
            {
                string extensions = GL.GetString(StringName.Extensions);
                _supportsNpot = extensions.Contains("GL_ARB_texture_non_power_of_two");
            }
            return _supportsNpot.Value;
        }


        #region Enable/Disable

        public static void EnableTexturing()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
        }

        public static void DisableTexturing()
        {
            GL.Disable(EnableCap.Texture2D);
        }

        #endregion

        #region Create

        // http://stackoverflow.com/questions/5525122/c-sharp-math-question-smallest-power-of-2-bigger-than-x
        // http://aggregate.org/MAGIC/#Next%20Largest%20Power%20of%202
        private static int NextPowerOfTwo(int num)
        {
            uint x = (uint)num;
            x--;
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (int)(x + 1);
        }

        private static List<ITexture> texturesToDispose = new List<ITexture>();

        public static void EnqueueDisposal(ITexture tex)
        {
            lock (texturesToDispose)
            {
                texturesToDispose.Add(tex);
            }
        }

        public static void DisposeQueuedTextures()
        {
            lock (texturesToDispose)
            {
                texturesToDispose.ForEach(t => t.Dispose());
                texturesToDispose.Clear();
            }
        }

        public static GLTexture Create(string name, Bitmap bitmap, int width, int height, TextureFlags flags)
        {
            DisposeQueuedTextures();

            if (Exists(name))
            {
                Delete(name);
            }
            Bitmap actualBitmap = bitmap;
            if (ForceNonPowerOfTwoResize || !SupportsNonPowerOfTwo())
            {
                int w = NextPowerOfTwo(bitmap.Width);
                int h = NextPowerOfTwo(bitmap.Height);
                if (w != bitmap.Width || h != bitmap.Height) actualBitmap = new Bitmap(bitmap, w, h);
            }
            BitmapData data = actualBitmap.LockBits(
                new Rectangle(0, 0, actualBitmap.Width, actualBitmap.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
                );
            int tex = CreateAndBindTexture();
            SetTextureParameters();
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat,
                data.Width,
                data.Height,
                0,
                PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0
                );

            actualBitmap.UnlockBits(data);
            if (actualBitmap != bitmap)
            {
                actualBitmap.Dispose();
            }
            GLTexture texobj = new GLTexture(tex, name, flags, 0) { Width = width, Height = height };
            Textures.Add(name, texobj);
            return texobj;
        }

        private static int LastRenderTargetID = 0;
        public static GLTexture CreateRenderTarget(int width, int height)
        {
            TextureFlags flags = TextureFlags.None;

            byte[] tempBuffer = new byte[width * height * Bitmap.GetPixelFormatSize(System.Drawing.Imaging.PixelFormat.Format32bppArgb)];

            int tex = CreateAndBindTexture();
            SetTextureParameters();
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat,
                width,
                height,
                0,
                PixelFormat.Bgra,
                PixelType.UnsignedByte,
                tempBuffer
                );
            int fbo = CreateFrameBuffer();
            GLTexture texobj = new GLTexture(tex, "RENDERTARGET" + LastRenderTargetID.ToString(), flags, fbo) { Width = width, Height = height };
            Textures.Add("RENDERTARGET" + LastRenderTargetID.ToString(), texobj); LastRenderTargetID++;
            return texobj;
        }

        private static int CreateAndBindTexture()
        {
            int tex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, tex);
            return tex;
        }

        private static int CreateFrameBuffer()
        {
            int fbo = GL.GenFramebuffer();
            return fbo;
        }

        private static void SetTextureParameters()
        {
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Modulate);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)(DisableTextureFiltering ? TextureMinFilter.Linear : TextureMinFilter.LinearMipmapLinear));
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);
        }

        #endregion

        #region Bind

        public static void Bind(string name)
        {
            if (!Exists(name))
            {
                throw new Exception(Local.LocalString("exception.texture_doesnt_exist", name));
            }
            Bind(Get(name).Reference);
        }

        public static void Bind(int reference)
        {
            GL.BindTexture(TextureTarget.Texture2D, reference);
        }

        public static void Unbind()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
        }

        public static void SetRenderTarget(GLTexture target)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, target.FrameBufferObject);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, target.Reference, 0);
        }

        public static void ResetRenderTarget()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        #endregion

        #region Get

        public static bool Exists(string name)
        {
            return Textures.ContainsKey(name);
        }

        public static GLTexture Get(string name)
        {
            return Exists(name) ? Textures[name] : null;
        }

        #endregion

        #region Delete

        public static void Delete(string name)
        {
            if (Textures.ContainsKey(name))
            {
                DeleteTexture(Get(name).Reference);
                Textures.Remove(name);
            }
        }

        public static void DeleteAll()
        {
            foreach (KeyValuePair<string, GLTexture> e in Textures)
            {
                DeleteTexture(e.Value.Reference);
            }
            Textures.Clear();
        }

        public static void DeleteTexture(int num)
        {
            GL.DeleteTexture(num);
        }

        #endregion
    }
}
