using System.Drawing;
using CBRE.Common;

namespace CBRE.DataStructures.Models
{
    public class Texture
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public ITexture TextureObject { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Flags { get; set; }
        public Bitmap Image { get; set; }
    }
}