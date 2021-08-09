using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class CompressedWave : Wave
    {
        public byte[] CompressedChannelOne { get; set; }

        public byte[] CompressedChannelTwo { get; set; }

        public int ChannelOneSize { get; set; }

        public int ChannelTwoSize { get; set; }

        public CompressedWave() { }

        public CompressedWave(Wave wav)
        {
            ChunkID = wav.ChunkID;
            ChunkSize = wav.ChunkSize;
            Format = wav.Format;
            Subchunk1ID = wav.Subchunk1ID;
            SubChunk1Size = wav.SubChunk1Size;
            AudioFormat = wav.AudioFormat;
            NumChannels = wav.NumChannels;
            SampleRate = wav.SampleRate;
            ByteRate = wav.ByteRate;
            BlockAlign = wav.BlockAlign;
            BitsPerSample = wav.BitsPerSample;
            SubChunk2ID = wav.SubChunk2ID;
            SubChunk2Size = wav.SubChunk2Size;
            Samples = SubChunk2Size / (BitsPerSample / 8) / NumChannels;
            HeaderBytes = new byte[44];
            Buffer.BlockCopy(wav.HeaderBytes, 0, HeaderBytes, 0, 44);
        }

        public void SetChannels(double[] left, double[] right)
        {
            LeftChannel = left;
            RightChannel = right;
        }

        public void SetCompressedChannels(byte[] one, byte[] two)
        {
            CompressedChannelOne = one;
            ChannelOneSize = CompressedChannelOne.Length;
            CompressedChannelTwo = two;
            ChannelTwoSize = CompressedChannelTwo == null ? 0 : CompressedChannelTwo.Length;
        }

        public new string Save(string dir, string filename)
        {
            var path = $"{dir}\\{filename}.cwav";
            using (var fs = File.Open(path, FileMode.Create))
            {
                using (var headerstream = new MemoryStream(HeaderBytes))
                {
                    headerstream.CopyTo(fs);
                    fs.Write(BitConverter.GetBytes(ChannelOneSize), 0, 4);
                    fs.Write(BitConverter.GetBytes(ChannelTwoSize), 0, 4);
                    for (int i = 0; i < ChannelOneSize; ++i)
                    {
                        fs.WriteByte(CompressedChannelOne[i]);
                    }
                    if (NumChannels == 2)
                    {
                        for (int i = 0; i < ChannelTwoSize; ++i)
                        {
                            fs.WriteByte(CompressedChannelTwo[i]);
                        }
                    }
                }
            }
            return path;
        }

        public static new CompressedWave OpenWav(string filename)
        {
            var wave = new CompressedWave();

            byte[] wav = File.ReadAllBytes(filename);

            wave.ChunkID = BitConverter.ToString(wav, 0, 4);

            wave.ChunkSize = BitConverter.ToInt32(wav, 4);

            wave.Format = BitConverter.ToString(wav, 8, 4);

            wave.Subchunk1ID = BitConverter.ToString(wav, 12, 4);

            wave.SubChunk1Size = BitConverter.ToInt32(wav, 16);

            wave.AudioFormat = BitConverter.ToInt16(wav, 20);

            wave.NumChannels = BitConverter.ToInt16(wav, 22);

            wave.SampleRate = BitConverter.ToInt32(wav, 24);

            wave.ByteRate = BitConverter.ToInt32(wav, 28);

            wave.BlockAlign = BitConverter.ToInt16(wav, 32);

            wave.BitsPerSample = BitConverter.ToInt16(wav, 34);

            wave.SubChunk2ID = BitConverter.ToString(wav, 36, 4);

            wave.SubChunk2Size = BitConverter.ToInt32(wav, 40);

            wave.Samples = wave.SubChunk2Size / (wave.BitsPerSample / 8) / wave.NumChannels;

            wave.CompressedChannelOne = new byte[wave.Samples * 2];

            wave.HeaderBytes = new byte[44];

            Buffer.BlockCopy(wav, 0, wave.HeaderBytes, 0, 44);

            var channelOneSize = BitConverter.ToInt32(wav, 44);

            var channelTwoSize = BitConverter.ToInt32(wav, 48);

            if (wave.NumChannels == 2)
            {
                wave.CompressedChannelTwo = new byte[wave.Samples * 2];
            }

            int pos = 52;

            for (int i = 0; i < channelOneSize; ++i, ++pos)
            {
                wave.CompressedChannelOne[i] = wav[pos];
            }

            for (int i = 0; i < channelTwoSize; ++i, ++pos)
            {
                wave.CompressedChannelTwo[i] = wav[pos];
            }

            return wave;
        }
    }
}
