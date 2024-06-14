namespace Parafait_POS.Reservation
{
    partial class frmBookingPackageDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBookingPackageDetails));
            this.pnlProductDetails = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.productVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.SuspendLayout();
            // 
            // pnlProductDetails
            // 
            this.pnlProductDetails.AutoScroll = true;
            this.pnlProductDetails.Location = new System.Drawing.Point(4, 3);
            this.pnlProductDetails.Name = "pnlProductDetails";
            this.pnlProductDetails.Size = new System.Drawing.Size(307, 458);
            this.pnlProductDetails.TabIndex = 126;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(116, 467);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(116, 34);
            this.btnClose.TabIndex = 129;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // productVScrollBar
            // 
            this.productVScrollBar.AutoHide = false;
            this.productVScrollBar.DataGridView = null;
            this.productVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("productVScrollBar.DownButtonBackgroundImage")));
            this.productVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("productVScrollBar.DownButtonDisabledBackgroundImage")));
            this.productVScrollBar.Location = new System.Drawing.Point(291, 3);
            this.productVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.productVScrollBar.Name = "productVScrollBar";
            this.productVScrollBar.ScrollableControl = this.pnlProductDetails;
            this.productVScrollBar.ScrollViewer = null;
            this.productVScrollBar.Size = new System.Drawing.Size(40, 458);
            this.productVScrollBar.TabIndex = 130;
            this.productVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("productVScrollBar.UpButtonBackgroundImage")));
            this.productVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("productVScrollBar.UpButtonDisabledBackgroundImage")));
            this.productVScrollBar.UpButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            this.productVScrollBar.DownButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            // 
            // frmBookingPackageDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(336, 506);
            this.Controls.Add(this.productVScrollBar);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlProductDetails);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmBookingPackageDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Booking Package Details";
            this.Load += new System.EventHandler(this.frmBookingPackageDetails_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlProductDetails;
        private System.Windows.Forms.Button btnClose;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView productVScrollBar;
    }
}