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
    public partial class ImageForm : Form
    {

        private AudioForm _audioForm;

        private string _imgDir;

        private string _imgName;

        private Bitmap _oImg;

        private Bitmap _cImg;


        public ImageForm(AudioForm audioForm)
        {
            _audioForm = audioForm;

            InitializeComponent();

            this.FormClosed += ImageForm_FormClosed;
        }

        private void ImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void AudioFormBtn_Click(object sender, EventArgs e)
        {
            Hide();
            _audioForm.Show();
        }

        private void ImageForm_Load(object sender, EventArgs e)
        {
            
        }

        private void OpenBmpBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "bmp File (*.bmp)|*.bmp;"
            };
            if (open.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _oImg = new Bitmap(open.FileName, true);

            _imgDir = Path.GetDirectoryName(open.FileName);

            _imgName = Path.GetFileNameWithoutExtension(open.FileName);

            try
            {
                int width = _oImg.Width, height = _oImg.Height;
                double[,] Y, U, V;
                ImageCompressor.Compress(_oImg, out Y, out U, out V);
                ImageCompressor.MSave(Y, width, height, _imgDir, $"{_imgName}-Y");
                ImageCompressor.MSave(U, width, height, _imgDir, $"{_imgName}-U");
                ImageCompressor.MSave(V, width, height, _imgDir, $"{_imgName}-V");

                _cImg = ImageCompressor.Decompress(Y, U, V, width, height);


            } catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                _imgDir = null;
                _imgName = null;
                return;
            }

            OriginalImg.Image = _oImg;

            CompressedImg.Image = _cImg;

        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (_cImg != null)
            {
                var path = $"{_imgDir}\\{_imgName}-quantized.bmp";
                _cImg.Save(path);
                MessageBox.Show($"Saved to {path}");
            }
        }
    }
}
