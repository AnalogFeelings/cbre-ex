﻿using CBRE.Common;
using System.Drawing;
using System.Drawing.Imaging;

namespace CBRE.Providers.Texture
{
    public class NullTextureStreamSource : ITextureStreamSource
    {
        private static readonly Bitmap PlaceholderImage;
        static NullTextureStreamSource()
        {
            PlaceholderImage = new Bitmap(64, 64, PixelFormat.Format32bppArgb);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(PlaceholderImage))
            {
                g.FillRectangle(Brushes.Black, 0, 0, 64, 64);
                for (int i = 0; i < 64; i++)
                {
                    int x = i % 8;
                    int y = i / 8;
                    if (y % 2 == x % 2) continue;
                    g.FillRectangle(Brushes.Magenta, x * 8, y * 8, 8, 8);
                }
            }
        }

        private readonly int _maxWidth;
        private readonly int _maxHeight;

        public NullTextureStreamSource(int maxWidth, int maxHeight)
        {
            _maxWidth = maxWidth;
            _maxHeight = maxHeight;
        }

        public bool HasImage(TextureItem item)
        {
            return item.Flags.HasFlag(TextureFlags.Missing);
        }

        public BitmapRef GetImage(TextureItem item)
        {
            lock (PlaceholderImage)
            {
                return new BitmapRef(PlaceholderImage);
            }
        }

        public void Dispose()
        {

        }
    }
}
