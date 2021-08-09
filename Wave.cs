using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class Wave
    {

        public string ChunkID { get; protected set; }

        public int ChunkSize { get; protected set; }

        public string Format { get; protected set; }

        public string Subchunk1ID { get; protected set; }

        public int SubChunk1Size { get; protected set; }

        public short AudioFormat { get; protected set; }

        public short NumChannels { get; protected set; }

        public int SampleRate { get; protected set; }

        public int ByteRate { get; protected set; }

        public short BlockAlign { get; protected set; }

        public short BitsPerSample { get; protected set; }

        public string SubChunk2ID { get; protected set; }

        public int SubChunk2Size { get; protected set; }

        public double[] LeftChannel { get; set; }

        public double[] RightChannel { get; set; }

        public short[] LeftChannelShort { get; set; }

        public short[] RightChannelShort { get; set; }

        public byte[] HeaderBytes { get; set; }

        public int Samples { get; protected set; }

        public Wave() { }

        public Wave(CompressedWave wav)
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

        static double BytesToDouble(byte firstByte, byte secondByte)
        {
            short s = ByteToShort(firstByte, secondByte);
            return s / 32768.0;
        }

        static short ByteToShort(byte firstByte, byte secondByte)
        {
            return (short)((secondByte << 8) | firstByte);
        }

        public string Save(string dir, string filename)
        {
            var path = $"{dir}\\{filename}.wav";
            using (var fs = File.Open(path, FileMode.Create))
            {
                using (var headerstream = new MemoryStream(HeaderBytes))
                {
                    headerstream.CopyTo(fs);
                    for(int i = 0; i < Samples; ++i)
                    {
                        var left = LeftChannelShort[i];
                        fs.Write(BitConverter.GetBytes(left), 0, 2);

                        if(NumChannels == 2)
                        {
                            var right = RightChannelShort[i];
                            fs.Write(BitConverter.GetBytes(right), 0, 2);
                        }
                    }
                }
            }
            return path;
        }

        public static Wave OpenWav(string filename)
        {
            var wave = new Wave();

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

            wave.LeftChannel = new double[wave.Samples];

            wave.LeftChannelShort = new short[wave.Samples];

            wave.HeaderBytes = new byte[44];

            Buffer.BlockCopy(wav, 0, wave.HeaderBytes, 0, 44);

            if(wave.NumChannels == 2)
            {
                wave.RightChannel = new double[wave.Samples];

                wave.RightChannelShort = new short[wave.Samples];
            }

            for (int i = 0, pos = 44;  i < wave.Samples; ++i)
            {
                wave.LeftChannel[i] = BytesToDouble(wav[pos], wav[pos + 1]);
                wave.LeftChannelShort[i] = ByteToShort(wav[pos], wav[pos + 1]);
                pos += 2;
                if (wave.NumChannels == 2)
                {
                    wave.RightChannel[i] = BytesToDouble(wav[pos], wav[pos + 1]);
                    wave.RightChannelShort[i] = ByteToShort(wav[pos], wav[pos + 1]);
                    pos += 2;
                }
            }

            return wave;
        }
    }
}
