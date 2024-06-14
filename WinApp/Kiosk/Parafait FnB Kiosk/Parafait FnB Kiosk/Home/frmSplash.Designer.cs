namespace Parafait_FnB_Kiosk
{
    partial class frmSplash
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.pbBackground = new System.Windows.Forms.PictureBox();
            this.panelSplashImage = new System.Windows.Forms.Panel();
            this.pbSplashLogo = new System.Windows.Forms.PictureBox();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackground)).BeginInit();
            this.panelSplashImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplashLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelHeader.Controls.Add(this.pbBackground);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1080, 277);
            this.panelHeader.TabIndex = 2;
            // 
            // pbBackground
            // 
            this.pbBackground.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Header_Bg;
            this.pbBackground.Location = new System.Drawing.Point(0, 0);
            this.pbBackground.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.pbBackground.Name = "pbBackground";
            this.pbBackground.Size = new System.Drawing.Size(75, 67);
            this.pbBackground.TabIndex = 0;
            this.pbBackground.TabStop = false;
            // 
            // panelSplashImage
            // 
            this.panelSplashImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelSplashImage.Controls.Add(this.pbSplashLogo);
            this.panelSplashImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSplashImage.Location = new System.Drawing.Point(0, 277);
            this.panelSplashImage.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panelSplashImage.Name = "panelSplashImage";
            this.panelSplashImage.Size = new System.Drawing.Size(1080, 1643);
            this.panelSplashImage.TabIndex = 3;
            // 
            // pbSplashLogo
            // 
            this.pbSplashLogo.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Header_Bg;
            this.pbSplashLogo.Location = new System.Drawing.Point(0, 0);
            this.pbSplashLogo.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.pbSplashLogo.Name = "pbSplashLogo";
            this.pbSplashLogo.Size = new System.Drawing.Size(75, 67);
            this.pbSplashLogo.TabIndex = 0;
            this.pbSplashLogo.TabStop = false;
            // 
            // frmSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.panelSplashImage);
            this.Controls.Add(this.panelHeader);
            this.Margin = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.Name = "frmSplash";
            this.Text = "Parafait F&B Kiosk";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ShowInTaskbar = true;
            this.Load += new System.EventHandler(this.frmSplash_Load);
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbBackground)).EndInit();
            this.panelSplashImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSplashLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox pbBackground;
        private System.Windows.Forms.Panel panelSplashImage;
        private System.Windows.Forms.PictureBox pbSplashLogo;
    }
}