namespace Parafait_POS
{
    partial class frmWaiverWelcome
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
            this.pbWelcomeImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbWelcomeImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbWelcomeImage
            // 
            this.pbWelcomeImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbWelcomeImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbWelcomeImage.Image = global::Parafait_POS.Properties.Resources.WaiverWelcomeScreen;
            this.pbWelcomeImage.Location = new System.Drawing.Point(0, 0);
            this.pbWelcomeImage.Name = "pbWelcomeImage";
            this.pbWelcomeImage.Size = new System.Drawing.Size(346, 288);
            this.pbWelcomeImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbWelcomeImage.TabIndex = 0;
            this.pbWelcomeImage.TabStop = false;
            // 
            // frmWaiverWelcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(346, 288);
            this.Controls.Add(this.pbWelcomeImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmWaiverWelcome";
            this.Text = "Welcome";
            ((System.ComponentModel.ISupportInitialize)(this.pbWelcomeImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbWelcomeImage;

    }
}