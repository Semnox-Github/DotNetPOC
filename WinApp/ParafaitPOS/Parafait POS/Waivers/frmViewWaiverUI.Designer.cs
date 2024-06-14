namespace Parafait_POS.Waivers
{
    partial class frmViewWaiverUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewWaiverUI));
            this.lblWaiverName = new System.Windows.Forms.Label();
            this.pnlWaiverDisplay = new System.Windows.Forms.Panel();
            this.wBrowser = new System.Windows.Forms.WebBrowser();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlWaiverDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWaiverName
            // 
            this.lblWaiverName.Location = new System.Drawing.Point(12, 9);
            this.lblWaiverName.Name = "lblWaiverName";
            this.lblWaiverName.Size = new System.Drawing.Size(272, 27);
            this.lblWaiverName.TabIndex = 2;
            this.lblWaiverName.Text = "Waiver Name";
            this.lblWaiverName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlWaiverDisplay
            // 
            this.pnlWaiverDisplay.AutoScroll = true;
            this.pnlWaiverDisplay.Controls.Add(this.wBrowser);
            this.pnlWaiverDisplay.Location = new System.Drawing.Point(15, 39);
            this.pnlWaiverDisplay.Name = "pnlWaiverDisplay";
            this.pnlWaiverDisplay.Size = new System.Drawing.Size(1200, 485);
            this.pnlWaiverDisplay.TabIndex = 3;
            // 
            // wBrowser
            // 
            this.wBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wBrowser.Location = new System.Drawing.Point(0, 0);
            this.wBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.wBrowser.Name = "wBrowser";
            this.wBrowser.ScrollBarsEnabled = false;
            this.wBrowser.Size = new System.Drawing.Size(1200, 485);
            this.wBrowser.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(555, 574);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 36);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmViewWaiverUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1241, 644);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pnlWaiverDisplay);
            this.Controls.Add(this.lblWaiverName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmViewWaiverUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Waiver File";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmViewWaiverUI_FormClosing);
            this.pnlWaiverDisplay.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblWaiverName;
        private System.Windows.Forms.Panel pnlWaiverDisplay;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.WebBrowser wBrowser;
    }
}