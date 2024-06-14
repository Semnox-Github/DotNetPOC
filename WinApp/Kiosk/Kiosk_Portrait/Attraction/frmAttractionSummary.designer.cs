namespace Parafait_Kiosk
{
    partial class frmAttractionSummary
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
            this.lblSiteName = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            this.btnProceed = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.flpComboProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.bigVerticalScrollCardProducts = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.pnlProductInfo = new System.Windows.Forms.Panel();
            this.lblBookingInfo = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.panelButtons.SuspendLayout();
            this.pnlProductInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(26, 1670);
            this.btnPrev.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(377, 1670);
            // 
            // lblSiteName
            // 
            this.lblSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSiteName.BackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.BorderSize = 0;
            this.lblSiteName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSiteName.Font = new System.Drawing.Font("Verdana", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(0, 0);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1196, 77);
            this.lblSiteName.TabIndex = 136;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            this.lblSiteName.Visible = false;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1851);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 50);
            this.txtMessage.TabIndex = 147;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // btnProceed
            // 
            this.btnProceed.BackColor = System.Drawing.Color.Transparent;
            this.btnProceed.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnProceed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProceed.FlatAppearance.BorderSize = 0;
            this.btnProceed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProceed.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F);
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProceed.Location = new System.Drawing.Point(702, 0);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(325, 164);
            this.btnProceed.TabIndex = 1078;
            this.btnProceed.Text = "Confirm Booking";
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.btnProceed);
            this.panelButtons.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(26, 1670);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1037, 180);
            this.panelButtons.TabIndex = 20005;
            // 
            // flpComboProducts
            // 
            this.flpComboProducts.AutoScroll = true;
            this.flpComboProducts.BackColor = System.Drawing.Color.Transparent;
            this.flpComboProducts.Location = new System.Drawing.Point(196, 426);
            this.flpComboProducts.Name = "flpComboProducts";
            this.flpComboProducts.Size = new System.Drawing.Size(723, 1092);
            this.flpComboProducts.TabIndex = 20008;
            // 
            // bigVerticalScrollCardProducts
            // 
            this.bigVerticalScrollCardProducts.AutoHide = true;
            this.bigVerticalScrollCardProducts.BackColor = System.Drawing.Color.White;
            this.bigVerticalScrollCardProducts.DataGridView = null;
            this.bigVerticalScrollCardProducts.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollCardProducts.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollCardProducts.Location = new System.Drawing.Point(934, 426);
            this.bigVerticalScrollCardProducts.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollCardProducts.Name = "bigVerticalScrollCardProducts";
            this.bigVerticalScrollCardProducts.ScrollableControl = this.flpComboProducts;
            this.bigVerticalScrollCardProducts.ScrollViewer = null;
            this.bigVerticalScrollCardProducts.Size = new System.Drawing.Size(63, 1093);
            this.bigVerticalScrollCardProducts.TabIndex = 167;
            this.bigVerticalScrollCardProducts.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollCardProducts.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            // 
            // pnlProductInfo
            // 
            this.pnlProductInfo.BackColor = System.Drawing.Color.Transparent;
            this.pnlProductInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlProductInfo.Controls.Add(this.lblBookingInfo);
            this.pnlProductInfo.Controls.Add(this.lblProductName);
            this.pnlProductInfo.Controls.Add(this.lblQty);
            this.pnlProductInfo.Location = new System.Drawing.Point(10, 182);
            this.pnlProductInfo.Name = "pnlProductInfo";
            this.pnlProductInfo.Size = new System.Drawing.Size(1070, 189);
            this.pnlProductInfo.TabIndex = 20015;
            // 
            // lblBookingInfo
            // 
            this.lblBookingInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblBookingInfo.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBookingInfo.ForeColor = System.Drawing.Color.White;
            this.lblBookingInfo.Location = new System.Drawing.Point(24, 2);
            this.lblBookingInfo.MinimumSize = new System.Drawing.Size(1027, 0);
            this.lblBookingInfo.Name = "lblBookingInfo";
            this.lblBookingInfo.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblBookingInfo.Size = new System.Drawing.Size(1027, 45);
            this.lblBookingInfo.TabIndex = 20011;
            this.lblBookingInfo.Text = "Showing booking information for";
            this.lblBookingInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProductName
            // 
            this.lblProductName.BackColor = System.Drawing.Color.Transparent;
            this.lblProductName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblProductName.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName.ForeColor = System.Drawing.Color.White;
            this.lblProductName.Location = new System.Drawing.Point(25, 53);
            this.lblProductName.MinimumSize = new System.Drawing.Size(1027, 0);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblProductName.Size = new System.Drawing.Size(1027, 63);
            this.lblProductName.TabIndex = 132;
            this.lblProductName.Text = "Product Name";
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblQty
            // 
            this.lblQty.BackColor = System.Drawing.Color.Transparent;
            this.lblQty.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQty.ForeColor = System.Drawing.Color.White;
            this.lblQty.Location = new System.Drawing.Point(24, 118);
            this.lblQty.MinimumSize = new System.Drawing.Size(1027, 0);
            this.lblQty.Name = "lblQty";
            this.lblQty.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblQty.Size = new System.Drawing.Size(1027, 42);
            this.lblQty.TabIndex = 20012;
            this.lblQty.Text = "Quantity:  ";
            this.lblQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmAttractionSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1901);
            this.Controls.Add(this.pnlProductInfo);
            this.Controls.Add(this.bigVerticalScrollCardProducts);
            this.Controls.Add(this.flpComboProducts);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblSiteName);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Bango Pro", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmAttractionSummary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmChooseProduct";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAttractionSummary_FormClosed);
            this.Load += new System.EventHandler(this.frmAttractionSummary_Load);
            this.Controls.SetChildIndex(this.lblSiteName, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.flpComboProducts, 0);
            this.Controls.SetChildIndex(this.bigVerticalScrollCardProducts, 0);
            this.Controls.SetChildIndex(this.pnlProductInfo, 0);
            this.panelButtons.ResumeLayout(false);
            this.pnlProductInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button btnProceed;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.FlowLayoutPanel flpComboProducts;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollCardProducts;
        private System.Windows.Forms.Panel pnlProductInfo;
        internal System.Windows.Forms.Label lblProductName;
        internal System.Windows.Forms.Label lblBookingInfo;
        internal System.Windows.Forms.Label lblQty;
    }
}