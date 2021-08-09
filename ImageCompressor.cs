using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    static class ImageCompressor
    {
        private static readonly int[,] QuantizationTable = {
            { 1, 1, 2, 4, 8, 16, 32, 64 },
            { 1, 1, 2, 4, 8, 16, 32, 64 },
            { 2, 2, 2, 4, 8, 16, 32, 64 },
            { 4, 4, 4, 4, 8, 16, 32, 64 },
            { 8, 8, 8, 8, 8, 16, 32, 64 },
            { 16, 16, 16, 16, 16, 16, 32, 64 },
            { 32, 32, 32, 32, 32, 32, 32, 64 },
            { 64, 64, 64, 64, 64, 64, 64, 64 }
        };

        //private static readonly int[,] QuantizationTable = {
        //    { 16, 11, 10, 16, 24, 40, 51, 61 },
        //    { 12, 12, 14, 19, 26, 58, 60, 55 },
        //    { 14, 13, 16, 24, 40, 57, 69, 56 },
        //    { 14, 17, 22, 29, 51, 87, 80, 62 },
        //    { 18, 22, 37, 56, 68, 109, 103, 77 },
        //    { 24, 35, 55, 64, 81, 104, 113, 92 },
        //    { 40, 64, 78, 87, 103, 121, 120, 101 },
        //    { 72, 92, 95, 98, 112, 100, 103, 99 }
        //};

        static double[,] Transpose(double[,] matrix)
        {
            int len = (int)Math.Sqrt(matrix.Length);
            double[,] res = new double[len, len];
            for (int i = 0; i < len; ++i)
            {
                for (int j = 0; j < len; ++j)
                {
                    res[i, j] = matrix[j, i];
                }
            }
            return res;
        }

        static double[,] Multiply(double[,] A, double[,] B)
        {
            int len = (int)Math.Sqrt(A.Length);
            double[,] res = new double[len, len];

            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    res[i, j] = 0;
                    for (int k = 0; k < len; k++)
                    {
                        res[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return res;
        }

        static double[,] GetDCTTransformMatrix(int n)
        {
            double[,] dct = new double[n, n];

            for (int i = 0; i < n; ++i)
            {
                double a = Math.Sqrt((i == 0 ? 1d : 2d) / n);

                for (int j = 0; j < n; ++j)
                {
                    dct[i, j] = a * Math.Cos(((2d * j + 1d) * i * Math.PI) / (2d * n));
                }
            }
            return dct;
        }

        static int[,] Round(double[,] matrix, int n)
        {
            int[,] res = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    res[i, j] = (int)Math.Round(matrix[i, j]);
                }
            }
            return res;
        }

        public static string MSave(double[,] m, int width, int height, string dir, string filename)
        {
            var path = $"{dir}\\{filename}.txt";
            using (var fs = File.Open(path, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs))
                {
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            sw.Write(m[i, j].ToString());
                            sw.Write(" ");
                        }
                        sw.Write("\n");
                    }
                }
            }
            return path;
        }

        static string MPrint(double[,] matrix, int width, int height)
        {
            //string res = "{\n";
            string res = "";
            for (int i = 0; i < width; i++)
            {
                //res += "\t{";
                for (int j = 0; j < height; j++)
                {
                    res += matrix[i, j];
                    //res += ",\t";
                    res += " ";
                }
                //res += "\t},\n";
            }
            //res += "}";
            return res;
        }


        static double[,] DCT(double[,] input, int n)
        {
            var dct = GetDCTTransformMatrix(n);

            var A = Multiply(dct, input);
            var TT = Transpose(dct);

            return Multiply(A, TT);
        }

        static double[,] DCTInv(double[,] input, int n)
        {
            var dct = GetDCTTransformMatrix(n);

            var TT = Transpose(dct);

            var A = Multiply(TT, input);

            return Multiply(A, dct);
        }

        static void Quantize(double[,] input)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    input[i, j] = Math.Round(input[i, j] / QuantizationTable[i, j]);
                }
            }
        }

        static void DeQuantize(double[,] input)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    input[i, j] = input[i, j] * QuantizationTable[i, j];
                }
            }
        }

        static void RGB2YUV(Bitmap bitmap, out double[,] Y, out double[,] U, out double[,] V)
        {
            Y = new double[bitmap.Width, bitmap.Height];
            U = new double[bitmap.Width, bitmap.Height];
            V = new double[bitmap.Width, bitmap.Height];


            for (int x = 0; x < bitmap.Width; ++x)
            {
                for (int y = 0; y < bitmap.Height; ++y)
                {
                    var rgb = bitmap.GetPixel(x, y);
                    double luma = rgb.R * .299000 + rgb.G * .587000 + rgb.B * .114000;
                    double u = rgb.R * -.168736 + rgb.G * -.331264 + rgb.B * .500000 + 128;
                    double v = rgb.R * .500000 + rgb.G * -.418688 + rgb.B * -.081312 + 128;
                    Y[x, y] = luma;
                    U[x, y] = u;
                    V[x, y] = v;
                }
            }
        }

        static Bitmap YUV2RGB(double[,] Y, double[,] U, double[,] V, int width, int height)
        {
            var res = new Bitmap(width, height);


            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    double luma = Y[x, y];
                    double u = U[x, y];
                    double v = V[x, y];
                    int r = (int)Math.Round(luma + 1.4075d * (v - 128d));
                    int g = (int)Math.Round(luma - 0.3455d * (u - 128d) - 0.7169d * (v - 128d));
                    int b = (int)Math.Round(luma + 1.7790d * (u - 128d));
                    if (r < 0) r = 0;
                    if (g < 0) g = 0;
                    if (b < 0) b = 0;
                    if (r > 255) r = 255;
                    if (g > 255) g = 255;
                    if (b > 255) b = 255;
                    res.SetPixel(x, y, Color.FromArgb(255, r, g, b));
                }
            }
            return res;
        }

        static void DCTMatrix(double[,] matrix, int width, int height)
        {
            int wn = width / 8;
            int wh = height / 8;

            for(int i = 0; i < wn; ++i)
            {
                for (int j = 0; j < wh; ++j)
                {
                    var part = new double[8, 8];
                    for (int k = 0; k < 8; ++k)
                    {
                        for (int l = 0; l < 8; ++l)
                        {
                            part[k, l] = matrix[i * 8 + k, j * 8 + l];
                        }
                    }
                    part = DCT(part, 8);
                    Quantize(part);
                    for (int k = 0; k < 8; ++k)
                    {
                        for (int l = 0; l < 8; ++l)
                        {
                            matrix[i * 8 + k, j * 8 + l] = part[k, l];
                        }
                    }
                }
            }
        }

        static void DCTInvMatrix(double[,] matrix, int width, int height)
        {
            int wn = width / 8;
            int wh = height / 8;

            for (int i = 0; i < wn; ++i)
            {
                for (int j = 0; j < wh; ++j)
                {
                    var part = new double[8, 8];
                    for (int k = 0; k < 8; ++k)
                    {
                        for (int l = 0; l < 8; ++l)
                        {
                            part[k, l] = matrix[i * 8 + k, j * 8 + l];
                        }
                    }
                    DeQuantize(part);
                    part = DCTInv(part, 8);
                    for (int k = 0; k < 8; ++k)
                    {
                        for (int l = 0; l < 8; ++l)
                        {
                            matrix[i * 8 + k, j * 8 + l] = part[k, l];
                        }
                    }
                }
            }

        }

        public static void Compress(Bitmap bitmap, out double[,] Y, out double[,] U, out double[,] V)
        {
            if (bitmap.Width % 8 != 0 || bitmap.Height % 8 != 0)
            {
                throw new InvalidOperationException("Input image size is incorrect.");
            }

            int width = bitmap.Width, height = bitmap.Height;
            RGB2YUV(bitmap, out Y, out U, out V);

            DCTMatrix(Y, width, height);
            DCTMatrix(U, width, height);
            DCTMatrix(V, width, height);

        }

        public static Bitmap Decompress(double[,] Y, double[,] U, double[,] V, int width, int height)
        {

            DCTInvMatrix(Y, width, height);
            DCTInvMatrix(U, width, height);
            DCTInvMatrix(V, width, height);

            return YUV2RGB(Y, U, V, width, height);
        }
    }
}
