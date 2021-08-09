namespace Project2
{
    partial class AudioForm
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
            this.CompressBtn = new System.Windows.Forms.Button();
            this.ImageFormBtn = new System.Windows.Forms.Button();
            this.DecompressBtn = new System.Windows.Forms.Button();
            this.MsgLabel = new System.Windows.Forms.Label();
            this.CompressRatioLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CompressBtn
            // 
            this.CompressBtn.Location = new System.Drawing.Point(73, 56);
            this.CompressBtn.Name = "CompressBtn";
            this.CompressBtn.Size = new System.Drawing.Size(143, 51);
            this.CompressBtn.TabIndex = 0;
            this.CompressBtn.Text = "Wav to CompressedWav";
            this.CompressBtn.UseVisualStyleBackColor = true;
            this.CompressBtn.Click += new System.EventHandler(this.CompressBtn_Click);
            // 
            // ImageFormBtn
            // 
            this.ImageFormBtn.Location = new System.Drawing.Point(470, 12);
            this.ImageFormBtn.Name = "ImageFormBtn";
            this.ImageFormBtn.Size = new System.Drawing.Size(125, 39);
            this.ImageFormBtn.TabIndex = 5;
            this.ImageFormBtn.Text = "To Image Form";
            this.ImageFormBtn.UseVisualStyleBackColor = true;
            this.ImageFormBtn.Click += new System.EventHandler(this.ImageFormBtn_Click);
            // 
            // DecompressBtn
            // 
            this.DecompressBtn.Location = new System.Drawing.Point(291, 56);
            this.DecompressBtn.Name = "DecompressBtn";
            this.DecompressBtn.Size = new System.Drawing.Size(143, 51);
            this.DecompressBtn.TabIndex = 6;
            this.DecompressBtn.Text = "CompressedWav to Wav";
            this.DecompressBtn.UseVisualStyleBackColor = true;
            this.DecompressBtn.Click += new System.EventHandler(this.DecompressBtn_Click);
            // 
            // MsgLabel
            // 
            this.MsgLabel.AutoSize = true;
            this.MsgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.MsgLabel.Location = new System.Drawing.Point(44, 141);
            this.MsgLabel.Name = "MsgLabel";
            this.MsgLabel.Size = new System.Drawing.Size(0, 25);
            this.MsgLabel.TabIndex = 7;
            // 
            // CompressRatioLabel
            // 
            this.CompressRatioLabel.AutoSize = true;
            this.CompressRatioLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.CompressRatioLabel.Location = new System.Drawing.Point(44, 183);
            this.CompressRatioLabel.Name = "CompressRatioLabel";
            this.CompressRatioLabel.Size = new System.Drawing.Size(157, 25);
            this.CompressRatioLabel.TabIndex = 8;
            this.CompressRatioLabel.Text = "Compress Ratio:";
            // 
            // AudioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 243);
            this.Controls.Add(this.CompressRatioLabel);
            this.Controls.Add(this.MsgLabel);
            this.Controls.Add(this.DecompressBtn);
            this.Controls.Add(this.ImageFormBtn);
            this.Controls.Add(this.CompressBtn);
            this.Name = "AudioForm";
            this.Text = "AudioForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CompressBtn;
        private System.Windows.Forms.Button ImageFormBtn;
        private System.Windows.Forms.Button DecompressBtn;
        private System.Windows.Forms.Label MsgLabel;
        private System.Windows.Forms.Label CompressRatioLabel;
    }
}

