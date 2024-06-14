namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// 
    /// </summary>
    partial class frmURLVideo
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
            this.VideoWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // VideoWebBrowser
            // 
            this.VideoWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VideoWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.VideoWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.VideoWebBrowser.Name = "VideoWebBrowser";
            this.VideoWebBrowser.Size = new System.Drawing.Size(598, 342);
            this.VideoWebBrowser.TabIndex = 0;
            // 
            // frmURLVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 342);
            this.Controls.Add(this.VideoWebBrowser);
            this.Name = "frmURLVideo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "URL Video";
            this.Load += new System.EventHandler(this.frmURLVideo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser VideoWebBrowser;

    }
}