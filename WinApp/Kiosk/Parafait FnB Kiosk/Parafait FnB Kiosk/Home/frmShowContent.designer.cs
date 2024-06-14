namespace Parafait_FnB_Kiosk
{
    partial class frmShowContent
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
            this.btnClose = new System.Windows.Forms.Button();
            this.lblScreenTitle = new System.Windows.Forms.Button();
            this.wbTerms = new System.Windows.Forms.WebBrowser();
            this.panelBrowser = new System.Windows.Forms.Panel();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.panelBrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Close_Btn;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClose.Location = new System.Drawing.Point(814, 47);
            this.btnClose.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 84);
            this.btnClose.TabIndex = 10;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblScreenTitle
            // 
            this.lblScreenTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblScreenTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblScreenTitle.FlatAppearance.BorderSize = 0;
            this.lblScreenTitle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblScreenTitle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblScreenTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblScreenTitle.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenTitle.ForeColor = System.Drawing.Color.White;
            this.lblScreenTitle.Location = new System.Drawing.Point(77, 16);
            this.lblScreenTitle.Name = "lblScreenTitle";
            this.lblScreenTitle.Size = new System.Drawing.Size(740, 140);
            this.lblScreenTitle.TabIndex = 13;
            this.lblScreenTitle.Text = "<<Terms and Conditions>>";
            this.lblScreenTitle.UseVisualStyleBackColor = false;
            // 
            // wbTerms
            // 
            this.wbTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wbTerms.Location = new System.Drawing.Point(1, 0);
            this.wbTerms.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.wbTerms.MinimumSize = new System.Drawing.Size(50, 45);
            this.wbTerms.Name = "wbTerms";
            this.wbTerms.ScrollBarsEnabled = false;
            this.wbTerms.Size = new System.Drawing.Size(897, 1186);
            this.wbTerms.TabIndex = 15;
            // 
            // panelBrowser
            // 
            this.panelBrowser.BackColor = System.Drawing.Color.Transparent;
            this.panelBrowser.Controls.Add(this.hScrollBar);
            this.panelBrowser.Controls.Add(this.vScrollBar);
            this.panelBrowser.Controls.Add(this.wbTerms);
            this.panelBrowser.Location = new System.Drawing.Point(1, 168);
            this.panelBrowser.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.panelBrowser.Name = "panelBrowser";
            this.panelBrowser.Size = new System.Drawing.Size(952, 1186);
            this.panelBrowser.TabIndex = 17;
            this.panelBrowser.Visible = false;
            // 
            // hScrollBar
            // 
            this.hScrollBar.LargeChange = 50;
            this.hScrollBar.Location = new System.Drawing.Point(1, 1152);
            this.hScrollBar.Maximum = 1100;
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(896, 34);
            this.hScrollBar.SmallChange = 5;
            this.hScrollBar.TabIndex = 17;
            this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Scroll);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.LargeChange = 300;
            this.vScrollBar.Location = new System.Drawing.Point(897, 0);
            this.vScrollBar.Maximum = 9000;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(55, 1186);
            this.vScrollBar.SmallChange = 30;
            this.vScrollBar.TabIndex = 16;
            this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
            // 
            // frmShowContent
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Pop_up_Purple;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(955, 1437);
            this.Controls.Add(this.panelBrowser);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblScreenTitle);
            this.DoubleBuffered = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.Name = "frmShowContent";
            this.ShowIcon = false;
            this.Text = "frmYesNo";
            this.TransparencyKey = System.Drawing.Color.CornflowerBlue;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRegisterTnC_FormClosing);
            this.Load += new System.EventHandler(this.frmShowContent_Load);
            this.panelBrowser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button lblScreenTitle;
        private System.Windows.Forms.WebBrowser wbTerms;
        private System.Windows.Forms.Panel panelBrowser;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.HScrollBar hScrollBar;
    }
}