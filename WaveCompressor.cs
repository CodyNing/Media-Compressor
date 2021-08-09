using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace Project2
{
    static class WaveCompressor
    {
        static double[] AutoCorrelation(double[] samples, int maxOrder)
        {
            int len = maxOrder + 1;
            double[] autoc = new double[len];
            for(int i = 0; i < len; ++i)
            {
                for(int j = 0; j < samples.Length - i; ++j)
                {
                    autoc[i] += samples[j] * samples[i + j];
                }
            }
            return autoc;
        }

        static double[] LevinsonDurbinRecursion(double[] autoc, int maxOrder)
        {
            double[] lpc = new double[maxOrder + 1];
            double err = autoc[0];

            for(int i = 0; i < maxOrder; ++i)
            {
                double r = -autoc[i + 1];
                int j;
                for(j = 0; j < i; ++j)
                {
                    r -= lpc[j] * autoc[i - j];
                }
                r /= err;

                lpc[i] = r;
                for(j = 0; j < i / 2; ++j)
                {
                    double temp = lpc[j];
                    lpc[j] += r * lpc[i - j - 1];
                    lpc[i - j - 1] += r * temp;
                }

                if(i % 2 == 0)
                {
                    lpc[j] += lpc[j] * r;
                }

                err *= (1d - r * r);
            }

            return lpc;
        }

        static double[] LevinsonDurbinRecursion2(double[] autoc, int maxOrder)
        {
            double[] lpc = new double[maxOrder + 1];
            double[] tmp = new double[maxOrder + 1];
            double err = autoc[0];
            lpc[0] = 1;


            for (int i = 1; i <= maxOrder; i++)
            {
                double r = 0;

                for (int j = 1; j < i; j++)
                {
                    r += lpc[j] * autoc[i - j];
                }
                r = (autoc[i] - r) / err;

                lpc[i] = r;

                for (int j = 1; j < i; j++)
                {
                    tmp[j] = lpc[j] - r * lpc[i - j];
                }
                for (int j = 1; j < i; j++)
                {
                    lpc[j] = tmp[j];
                }

                err *= (1 - r * r);
            }

            return lpc;
        }

        static short[] LinearDeltaEncoding(short[] samples)
        {
            short[] delta = new short[samples.Length];
            delta[0] = samples[0];
            for(int i = 1; i < samples.Length; ++i)
            {
                delta[i] = (short)(samples[i] - samples[i - 1]);
            }

            return delta;
        }

        static short[] LinearDeltaDecoding(short[] delta)
        {
            short[] samples = new short[delta.Length];
            samples[0] = delta[0];
            for (int i = 1; i < samples.Length; ++i)
            {
                samples[i] = (short)(samples[i - 1] + delta[i]);
            }

            return samples;
        }

        static void ChannelCoupling(short[] left, short[] right, out short[] mid, out short[] side)
        {
            if(left.Length != right.Length)
            {
                throw new InvalidOperationException("Channel lengths are different.");
            }
            mid = new short[left.Length];
            side = new short[left.Length];
            for(int i = 0; i < left.Length; ++i)
            {
                mid[i] = (short)((left[i] + right[i]) / 2);
                side[i] = (short)((left[i] - right[i]) / 2);
            }
        }

        static void ChannelDecoupling(short[] mid, short[] side, out short[] left, out short[] right)
        {
            if (mid.Length != side.Length)
            {
                throw new InvalidOperationException("Channel lengths are different.");
            }
            left = new short[side.Length];
            right = new short[mid.Length];
            for (int i = 0; i < mid.Length; ++i)
            {
                int diff = side[i] * 2;
                int sum = mid[i] * 2;
                right[i] = (short)((sum - diff) / 2);
                left[i] = (short)(right[i] + diff);
            }
        }

        static byte[] GzipCompress(byte[] bytes)
        {
            var outStream = new MemoryStream();
            var inStream = new MemoryStream(bytes);
            using (var gzips = new DeflateStream(outStream, CompressionMode.Compress))
            {
                inStream.CopyTo(gzips);
            }
            return outStream.ToArray();
        }

        static byte[] GzipDecompress(byte[] bytes)
        {
            var outStream = new MemoryStream();
            var inStream = new MemoryStream(bytes);
            using (var gzips = new DeflateStream(inStream, CompressionMode.Decompress))
            {
                gzips.CopyTo(outStream);
            }
            return outStream.ToArray();
        }

        static byte[] ShortsToBytes(short[] doubles)
        {
            var bytes = new byte[doubles.Length * sizeof(short)];
            Buffer.BlockCopy(doubles, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static short[] BytesToShorts(byte[] bytes)
        {
            var doubles = new short[bytes.Length / sizeof(short)];
            Buffer.BlockCopy(bytes, 0, doubles, 0, bytes.Length);
            return doubles;
        }


        public static CompressedWave Compress(Wave wav, out double ratio)
        {
            var compressedWav = new CompressedWave(wav);

            var deltaLeft = LinearDeltaEncoding(wav.LeftChannelShort);

            if (wav.NumChannels == 2)
            {
                var deltaRight = LinearDeltaEncoding(wav.RightChannelShort);

                short[] mid, side;
                ChannelCoupling(deltaLeft, deltaRight, out mid, out side);

                byte[] byteMid = ShortsToBytes(mid);
                byte[] byteSide = ShortsToBytes(side);

                byte[] compressedByteMid = GzipCompress(byteMid);
                byte[] compressedByteSide = GzipCompress(byteSide);

                compressedWav.SetCompressedChannels(compressedByteMid, compressedByteSide);
                ratio = (double)(compressedByteMid.Length + compressedByteSide.Length) / (wav.LeftChannel.Length * 4);
            }
            else
            {
                byte[] byteLeft = ShortsToBytes(deltaLeft);
                byte[] compressedByteLeft = GzipCompress(byteLeft);

                compressedWav.SetCompressedChannels(compressedByteLeft, null);
                ratio = (double)compressedByteLeft.Length / (wav.LeftChannel.Length * 2);
            }
            //ratio = 1;
            //double[] mid, side;
            //var asd = ChannelCoupling(wav.LeftChannel, wav.RightChannel, out mid, out side);

            return compressedWav;
        }

        public static Wave Decompress(CompressedWave wav)
        {
            var decompressed = new Wave(wav);

            if (wav.NumChannels == 2)
            {
                byte[] byteMid = GzipDecompress(wav.CompressedChannelOne);
                byte[] byteSide = GzipDecompress(wav.CompressedChannelTwo);

                short[] mid = BytesToShorts(byteMid);
                short[] side = BytesToShorts(byteSide);

                short[] deltaLeft, deltaRight;
                ChannelDecoupling(mid, side, out deltaLeft, out deltaRight);

                var left = LinearDeltaDecoding(deltaLeft);
                var right = LinearDeltaDecoding(deltaRight);

                decompressed.LeftChannelShort = left;
                decompressed.RightChannelShort = right;
            }
            else
            {
                byte[] byteLeft = GzipDecompress(wav.CompressedChannelOne);
                short[] deltaLeft = BytesToShorts(byteLeft);
                var left = LinearDeltaDecoding(deltaLeft);
                decompressed.LeftChannelShort = left;
            }

            return decompressed;
        }
    }
}
