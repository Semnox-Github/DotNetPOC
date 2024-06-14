namespace Redemption_Kiosk
{
    partial class frmRedemptionKioskSplashScreen
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
            this.panelHeader.Size = new System.Drawing.Size(1076, 267);
            this.panelHeader.TabIndex = 3;
            // 
            // pbBackground
            // 
            this.pbBackground.Location = new System.Drawing.Point(0, 0);
            this.pbBackground.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.pbBackground.Name = "pbBackground";
            this.pbBackground.Size = new System.Drawing.Size(75, 65);
            this.pbBackground.TabIndex = 0;
            this.pbBackground.TabStop = false;
            // 
            // panelSplashImage
            // 
            this.panelSplashImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelSplashImage.Controls.Add(this.pbSplashLogo);
            this.panelSplashImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSplashImage.Location = new System.Drawing.Point(0, 267);
            this.panelSplashImage.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panelSplashImage.Name = "panelSplashImage";
            this.panelSplashImage.Size = new System.Drawing.Size(1076, 1586);
            this.panelSplashImage.TabIndex = 4;
            //this.panelSplashImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelSplashImage_MouseClick);
            // 
            // pbSplashLogo
            // 
            this.pbSplashLogo.Location = new System.Drawing.Point(0, 0);
            this.pbSplashLogo.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.pbSplashLogo.Name = "pbSplashLogo";
            this.pbSplashLogo.Size = new System.Drawing.Size(75, 65);
            this.pbSplashLogo.TabIndex = 0;
            this.pbSplashLogo.TabStop = false;
            // 
            // frmSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1076, 1893);
            this.Controls.Add(this.panelSplashImage);
            this.Controls.Add(this.panelHeader);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "frmRedemptionKioskSplashScreen";
            this.ShowInTaskbar = true;
            this.Text = "Redemption Kiosk";
            this.Load += new System.EventHandler(this.FrmSplash_Load);
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