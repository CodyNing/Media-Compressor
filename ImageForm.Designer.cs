namespace Project2
{
    partial class ImageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AudioFormBtn = new System.Windows.Forms.Button();
            this.OpenBmpBtn = new System.Windows.Forms.Button();
            this.OriginalImg = new System.Windows.Forms.PictureBox();
            this.OriginalImgTitle = new System.Windows.Forms.Label();
            this.CompressedImg = new System.Windows.Forms.PictureBox();
            this.CompressedImgTitle = new System.Windows.Forms.Label();
            this.SaveBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OriginalImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompressedImg)).BeginInit();
            this.SuspendLayout();
            // 
            // AudioFormBtn
            // 
            this.AudioFormBtn.Location = new System.Drawing.Point(1340, 12);
            this.AudioFormBtn.Name = "AudioFormBtn";
            this.AudioFormBtn.Size = new System.Drawing.Size(131, 43);
            this.AudioFormBtn.TabIndex = 0;
            this.AudioFormBtn.Text = "To AudioForm";
            this.AudioFormBtn.UseVisualStyleBackColor = true;
            this.AudioFormBtn.Click += new System.EventHandler(this.AudioFormBtn_Click);
            // 
            // OpenBmpBtn
            // 
            this.OpenBmpBtn.Location = new System.Drawing.Point(12, 12);
            this.OpenBmpBtn.Name = "OpenBmpBtn";
            this.OpenBmpBtn.Size = new System.Drawing.Size(75, 23);
            this.OpenBmpBtn.TabIndex = 1;
            this.OpenBmpBtn.Text = "Open Bmp";
            this.OpenBmpBtn.UseVisualStyleBackColor = true;
            this.OpenBmpBtn.Click += new System.EventHandler(this.OpenBmpBtn_Click);
            // 
            // OriginalImg
            // 
            this.OriginalImg.Location = new System.Drawing.Point(80, 93);
            this.OriginalImg.Name = "OriginalImg";
            this.OriginalImg.Size = new System.Drawing.Size(632, 486);
            this.OriginalImg.TabIndex = 2;
            this.OriginalImg.TabStop = false;
            // 
            // OriginalImgTitle
            // 
            this.OriginalImgTitle.AutoSize = true;
            this.OriginalImgTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OriginalImgTitle.Location = new System.Drawing.Point(330, 48);
            this.OriginalImgTitle.Name = "OriginalImgTitle";
            this.OriginalImgTitle.Size = new System.Drawing.Size(146, 42);
            this.OriginalImgTitle.TabIndex = 5;
            this.OriginalImgTitle.Text = "Original";
            // 
            // CompressedImg
            // 
            this.CompressedImg.Location = new System.Drawing.Point(790, 93);
            this.CompressedImg.Name = "CompressedImg";
            this.CompressedImg.Size = new System.Drawing.Size(632, 486);
            this.CompressedImg.TabIndex = 6;
            this.CompressedImg.TabStop = false;
            // 
            // CompressedImgTitle
            // 
            this.CompressedImgTitle.AutoSize = true;
            this.CompressedImgTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CompressedImgTitle.Location = new System.Drawing.Point(1014, 48);
            this.CompressedImgTitle.Name = "CompressedImgTitle";
            this.CompressedImgTitle.Size = new System.Drawing.Size(230, 42);
            this.CompressedImgTitle.TabIndex = 7;
            this.CompressedImgTitle.Text = "Compressed";
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(13, 42);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 8;
            this.SaveBtn.Text = "Save Result";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // ImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1500, 645);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.CompressedImgTitle);
            this.Controls.Add(this.CompressedImg);
            this.Controls.Add(this.OriginalImgTitle);
            this.Controls.Add(this.OriginalImg);
            this.Controls.Add(this.OpenBmpBtn);
            this.Controls.Add(this.AudioFormBtn);
            this.Name = "ImageForm";
            this.Text = "ImageForm";
            this.Load += new System.EventHandler(this.ImageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OriginalImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CompressedImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AudioFormBtn;
        private System.Windows.Forms.Button OpenBmpBtn;
        private System.Windows.Forms.PictureBox OriginalImg;
        private System.Windows.Forms.Label OriginalImgTitle;
        private System.Windows.Forms.PictureBox CompressedImg;
        private System.Windows.Forms.Label CompressedImgTitle;
        private System.Windows.Forms.Button SaveBtn;
    }
}