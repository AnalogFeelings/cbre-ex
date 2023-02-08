using System.Diagnostics;
using CBRE.FileSystem;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace CBRE.Providers.Texture.Generic
{
    public class GenericStreamSource : ITextureStreamSource
    {
        private readonly IFile _file;

        public GenericStreamSource(IFile file)
        {
            _file = file;
        }

        public bool HasImage(string item)
        {
            return _file.TraversePath(item) != null;
        }

        public async Task<Bitmap> GetImage(string item, int maxWidth, int maxHeight)
        {
            IFile file = _file.TraversePath(item);
            if (file == null || !file.Exists) return null;

            var temp = await Task.Factory.StartNew(() =>
            {
                using (Stream stream = file.Open())
                {
                    return new Bitmap(Image.FromStream(stream));
                }
            });

            return temp;
        }

        public void Dispose()
        {
            //
        }
    }
}
