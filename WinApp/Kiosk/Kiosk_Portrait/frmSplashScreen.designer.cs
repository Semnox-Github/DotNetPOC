namespace Parafait_Kiosk
{
    partial class frmSplashScreen
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
            this.pbForeImage = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbForeImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbForeImage
            // 
            this.pbForeImage.BackColor = System.Drawing.Color.Transparent;
            this.pbForeImage.Image = global::Parafait_Kiosk.Properties.Resources.touch_screen_semnox_logo;
            this.pbForeImage.Location = new System.Drawing.Point(193, 326);
            this.pbForeImage.Name = "pbForeImage";
            this.pbForeImage.Size = new System.Drawing.Size(695, 770);
            this.pbForeImage.TabIndex = 0;
            this.pbForeImage.TabStop = false;
            this.pbForeImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmSplashScreen_MouseClick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Gotham Rounded Bold", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(9, 1256);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1061, 235);
            this.label1.TabIndex = 1;
            this.label1.Text = "TOUCH SCREEN TO BEGIN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmSplashScreen_MouseClick);
            // 
            // frmSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbForeImage);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSplashScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmSplashScreen_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.pbForeImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbForeImage;
        private System.Windows.Forms.Label label1;
    }
}