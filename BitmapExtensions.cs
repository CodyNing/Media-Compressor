using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public static class BitmapExtensions
    {
        public static Bitmap ToGrayscale(this Bitmap bitmap)
        {
            var res = new Bitmap(bitmap.Width, bitmap.Height);

            for(int x = 0; x < bitmap.Width; ++x)
            {
                for(int y = 0; y < bitmap.Height; ++y)
                {
                    var pix = bitmap.GetPixel(x, y);
                    int luma = (int)(pix.R * 0.3f + pix.G * 0.59f + pix.B * 0.11f);
                    res.SetPixel(x, y, Color.FromArgb(pix.A, luma, luma, luma));
                }
            }

            return res;
        }

        public static int[,] generateMatrix(int n)
        {
            if((n & (n - 1)) != 0)
            {
                return null;
            }
            int len = 2;
            var bas = new int[2, 2] { { 0, 2 }, { 3, 1 } };
            var res = new int[2, 2] { { 0, 2 }, { 3, 1 } };

            while ( (n >> 1) > 1 )
            {
                int newlen = len * 2;
                int[, ] next = new int[newlen, newlen];

                for(int i = 0; i < newlen; ++i)
                {
                    for(int j = 0; j < newlen; ++j)
                    {
                        int facr = i / len;
                        int facc = j / len;
                        int r = i % len;
                        int c = j % len;

                        next[i, j] = 4 * res[r, c] + bas[facr, facc];
                    }
                }

                res = next;
                len = newlen;
                n >>= 1;
            }

            return res;
        }


        public static Bitmap OrderedDithering(this Bitmap bitmap, int n)
        {
            var res = new Bitmap(bitmap.Width, bitmap.Height);

            var d = generateMatrix(n);

            float fac = 256f / (n * n + 1);

            for (int x = 0; x < bitmap.Width; ++x)
            {
                for (int y = 0; y < bitmap.Height; ++y)
                {
                    int i = x % n;
                    int j = y % n;
                    var pix = bitmap.GetPixel(x, y);
                    if(pix.R / fac > d[i, j])
                    {
                        res.SetPixel(x, y, Color.FromArgb(pix.A, 255, 255, 255));
                    }
                    else
                    {
                        res.SetPixel(x, y, Color.FromArgb(pix.A, 0, 0, 0));
                    }
                    
                }
            }

            return res;
        }

        private static byte getRGBbyIndex(int index, Color pixel)
        {
            switch (index)
            {
                case 0:
                    return pixel.R;
                case 1:
                    return pixel.G;
                case 2:
                    return pixel.B;
            }
            return 0;
        }

        private static Color setRGBbyIndex(int index, Color pixel, int A, int value)
        {
            switch (index)
            {
                case 0:
                    return Color.FromArgb(A, value, pixel.G, pixel.B);
                case 1:
                    return Color.FromArgb(A, pixel.R, value, pixel.B);
                case 2:
                    return Color.FromArgb(A, pixel.R, pixel.G, value);
            }
            return pixel;
        }

        public static Bitmap AutoLevel(this Bitmap bitmap)
        {
            var res = new Bitmap(bitmap.Width, bitmap.Height);

            for (int c = 0; c < 3; c++)
            {
                float[] hist = new float[256];

                for (int x = 0; x < bitmap.Width; ++x)
                {
                    for (int y = 0; y < bitmap.Height; ++y)
                    {
                        var pix = bitmap.GetPixel(x, y);
                        var val = getRGBbyIndex(c, pix);
                        hist[val]++;
                    }
                }

                hist[0] = hist[0] / (bitmap.Width * bitmap.Height);
                for (int i = 1; i < hist.Length; i++)
                {
                    hist[i] = hist[i - 1] + hist[i] / (bitmap.Width * bitmap.Height);
                }

                for (int x = 0; x < bitmap.Width; ++x)
                {
                    for (int y = 0; y < bitmap.Height; ++y)
                    {
                        var ocolor = bitmap.GetPixel(x, y);
                        var ncolor = res.GetPixel(x, y);
                        var oval = getRGBbyIndex(c, ocolor);
                        var nval = hist[oval] * 255;
                        var newpix = setRGBbyIndex(c, ncolor, ocolor.A, (int)nval);
                        res.SetPixel(x, y, newpix);
                    }
                }
            }

            return res;
        }

    }
}
