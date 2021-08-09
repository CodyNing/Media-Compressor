using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project2
{
    public partial class AudioForm : Form
    {
        private Wave _wave;

        private ImageForm _imageForm;

        public AudioForm()
        {
            _imageForm = new ImageForm(this);
            InitializeComponent();

            this.FormClosed += AudioForm_FormClosed;
        }

        private void AudioForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(1);
        }


        private async void CompressBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "wav File (*.wav*)|*.wav*;"
            };
            if (open.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _wave = Wave.OpenWav(open.FileName);

            //_wave.Save($"{open.FileName}-1");

            MsgLabel.Text = "Compressing...";
            var pair = await Compress();
            var compressed = pair.Key;
            var ratio = pair.Value;

            CompressRatioLabel.Text = $"Compression Ratio: {(ratio * 100):00.0}%";

            var dir = Path.GetDirectoryName(open.FileName);
            var filname = $"{Path.GetFileNameWithoutExtension(open.FileName)}-compressed";
            var outpath = compressed.Save(dir, filname);

            MsgLabel.Text = $"Compression completed!";
            MessageBox.Show($"Saved to {outpath}");

        }


        private async Task<KeyValuePair<CompressedWave, double>> Compress()
        {
            double ratio = 0;
            CompressedWave wav = null;
            await Task.Run(() =>
            {
                wav = WaveCompressor.Compress(_wave, out ratio);
            });
            return new KeyValuePair<CompressedWave, double>(wav, ratio);
        }

        private async Task<Wave> Decompress(CompressedWave wav)
        {
            Wave decompressed = null;
            await Task.Run(() =>
            {
                decompressed = WaveCompressor.Decompress(wav);
            });
            return decompressed;
        }

        private void ImageFormBtn_Click(object sender, EventArgs e)
        {
            Hide();
            _imageForm.Show();
        }

        private async void DecompressBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "compressed wav File (*.cwav*)|*.cwav*;"
            };
            if (open.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var compressed = CompressedWave.OpenWav(open.FileName);

            CompressRatioLabel.Text = "Compression Ratio:";

            MsgLabel.Text = "Decompressing...";
            var decompressed = await Decompress(compressed);

            var dir = Path.GetDirectoryName(open.FileName);
            var filname = $"{Path.GetFileNameWithoutExtension(open.FileName)}-decompressed";
            var outpath = decompressed.Save(dir, filname);

            MsgLabel.Text = $"Decompression completed!";

            MessageBox.Show($"Saved to {outpath}");

            //bool same = true;
            //for (int i = 0; i < compressed.Samples; i++)
            //{
            //    if (decompressed.LeftChannelShort[i] != _wave.LeftChannelShort[i]
            //        || decompressed.RightChannelShort[i] != _wave.RightChannelShort[i])
            //    {
            //        var asd = $"dl: {decompressed.LeftChannelShort[i]} vs nl: {_wave.LeftChannelShort[i]}, dr: {decompressed.RightChannelShort[i]} vs nr: {_wave.RightChannelShort[i]}";
            //        same = false;
            //    }
            //}
            //MsgLabel.Text = $"Result: {same}";
        }
    }
}
