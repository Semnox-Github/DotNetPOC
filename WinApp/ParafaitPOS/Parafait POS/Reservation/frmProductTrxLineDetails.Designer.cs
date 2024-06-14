namespace Parafait_POS.Reservation
{
    partial class frmProductTrxLineDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProductTrxLineDetails));
            this.gpbxTrxProfile = new System.Windows.Forms.GroupBox();
            this.VScrollBarView = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.pnlTrxLineDetails = new System.Windows.Forms.Panel();
            this.usrCtrlProductTrxLineDetails1 = new Parafait_POS.Reservation.usrCtrlProductTrxLineDetails();
            this.lblPageSize = new System.Windows.Forms.Label();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.lblPageNo = new System.Windows.Forms.Label();
            this.txtPageSize = new System.Windows.Forms.TextBox();
            this.txtPageNo = new System.Windows.Forms.TextBox();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnEditModifiers = new System.Windows.Forms.Button();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.lblParentLine = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblLineId = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.gpbxTrxProfile.SuspendLayout();
            this.pnlTrxLineDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpbxTrxProfile
            // 
            this.gpbxTrxProfile.BackColor = System.Drawing.Color.White;
            this.gpbxTrxProfile.Controls.Add(this.VScrollBarView);
            this.gpbxTrxProfile.Controls.Add(this.lblPageSize);
            this.gpbxTrxProfile.Controls.Add(this.lblTotalCount);
            this.gpbxTrxProfile.Controls.Add(this.lblPageNo);
            this.gpbxTrxProfile.Controls.Add(this.txtPageSize);
            this.gpbxTrxProfile.Controls.Add(this.txtPageNo);
            this.gpbxTrxProfile.Controls.Add(this.btnRight);
            this.gpbxTrxProfile.Controls.Add(this.btnLeft);
            this.gpbxTrxProfile.Controls.Add(this.btnEditModifiers);
            this.gpbxTrxProfile.Controls.Add(this.lblRemarks);
            this.gpbxTrxProfile.Controls.Add(this.lblCardNumber);
            this.gpbxTrxProfile.Controls.Add(this.lblProductName);
            this.gpbxTrxProfile.Controls.Add(this.pnlTrxLineDetails);
            this.gpbxTrxProfile.Controls.Add(this.lblParentLine);
            this.gpbxTrxProfile.Controls.Add(this.btnClose);
            this.gpbxTrxProfile.Controls.Add(this.lblPrice);
            this.gpbxTrxProfile.Controls.Add(this.lblLineId);
            this.gpbxTrxProfile.Controls.Add(this.lblQty);
            this.gpbxTrxProfile.Location = new System.Drawing.Point(3, -2);
            this.gpbxTrxProfile.Name = "gpbxTrxProfile";
            this.gpbxTrxProfile.Size = new System.Drawing.Size(1181, 669);
            this.gpbxTrxProfile.TabIndex = 0;
            this.gpbxTrxProfile.TabStop = false;
            // 
            // VScrollBarView
            // 
            this.VScrollBarView.AutoHide = false;
            this.VScrollBarView.DataGridView = null;
            this.VScrollBarView.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("VScrollBarView.DownButtonBackgroundImage")));
            this.VScrollBarView.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("VScrollBarView.DownButtonDisabledBackgroundImage")));
            this.VScrollBarView.Location = new System.Drawing.Point(1131, 52);
            this.VScrollBarView.Margin = new System.Windows.Forms.Padding(0);
            this.VScrollBarView.Name = "VScrollBarView";
            this.VScrollBarView.ScrollableControl = this.pnlTrxLineDetails;
            this.VScrollBarView.ScrollViewer = null;
            this.VScrollBarView.Size = new System.Drawing.Size(40, 527);
            this.VScrollBarView.TabIndex = 133;
            this.VScrollBarView.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("VScrollBarView.UpButtonBackgroundImage")));
            this.VScrollBarView.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("VScrollBarView.UpButtonDisabledBackgroundImage")));
            this.VScrollBarView.UpButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            this.VScrollBarView.DownButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            // 
            // pnlTrxLineDetails
            // 
            this.pnlTrxLineDetails.AutoScroll = true;
            this.pnlTrxLineDetails.Controls.Add(this.usrCtrlProductTrxLineDetails1);
            this.pnlTrxLineDetails.Location = new System.Drawing.Point(3, 52);
            this.pnlTrxLineDetails.Name = "pnlTrxLineDetails";
            this.pnlTrxLineDetails.Size = new System.Drawing.Size(1150, 527);
            this.pnlTrxLineDetails.TabIndex = 124;
            // 
            // usrCtrlProductTrxLineDetails1
            // 
            this.usrCtrlProductTrxLineDetails1.BackColor = System.Drawing.Color.White;
            this.usrCtrlProductTrxLineDetails1.Location = new System.Drawing.Point(7, 3);
            this.usrCtrlProductTrxLineDetails1.Name = "usrCtrlProductTrxLineDetails1";
            this.usrCtrlProductTrxLineDetails1.Size = new System.Drawing.Size(1118, 36);
            this.usrCtrlProductTrxLineDetails1.TabIndex = 0;
            // 
            // lblPageSize
            // 
            this.lblPageSize.AutoSize = true;
            this.lblPageSize.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageSize.Location = new System.Drawing.Point(683, 591);
            this.lblPageSize.Name = "lblPageSize";
            this.lblPageSize.Size = new System.Drawing.Size(112, 22);
            this.lblPageSize.TabIndex = 132;
            this.lblPageSize.Text = "PAGE SIZE";
            this.lblPageSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCount.Location = new System.Drawing.Point(533, 591);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(64, 22);
            this.lblTotalCount.TabIndex = 132;
            this.lblTotalCount.Text = "OF 10";
            this.lblTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPageNo
            // 
            this.lblPageNo.AutoSize = true;
            this.lblPageNo.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageNo.Location = new System.Drawing.Point(420, 591);
            this.lblPageNo.Name = "lblPageNo";
            this.lblPageNo.Size = new System.Drawing.Size(63, 22);
            this.lblPageNo.TabIndex = 132;
            this.lblPageNo.Text = "PAGE";
            this.lblPageNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPageSize
            // 
            this.txtPageSize.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPageSize.Location = new System.Drawing.Point(798, 588);
            this.txtPageSize.MaxLength = 4;
            this.txtPageSize.Name = "txtPageSize";
            this.txtPageSize.ReadOnly = true;
            this.txtPageSize.Size = new System.Drawing.Size(40, 29);
            this.txtPageSize.TabIndex = 131;
            this.txtPageSize.Enter += new System.EventHandler(this.txtPageSize_Enter);
            // 
            // txtPageNo
            // 
            this.txtPageNo.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPageNo.Location = new System.Drawing.Point(487, 588);
            this.txtPageNo.MaxLength = 4;
            this.txtPageNo.Name = "txtPageNo";
            this.txtPageNo.ReadOnly = true;
            this.txtPageNo.Size = new System.Drawing.Size(40, 29);
            this.txtPageNo.TabIndex = 131;
            this.txtPageNo.Enter += new System.EventHandler(this.txtPageNo_Enter);
            // 
            // btnRight
            // 
            this.btnRight.BackgroundImage = global::Parafait_POS.Properties.Resources.R_Forward_Btn;
            this.btnRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRight.Location = new System.Drawing.Point(619, 585);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(40, 35);
            this.btnRight.TabIndex = 130;
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.BtnRight_Click);
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRight_MouseDown);
            this.btnRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRight_MouseUp);
            // 
            // btnLeft
            // 
            this.btnLeft.BackgroundImage = global::Parafait_POS.Properties.Resources.R_Backward_Btn;
            this.btnLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeft.Location = new System.Drawing.Point(365, 585);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(40, 35);
            this.btnLeft.TabIndex = 130;
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.BtnLeft_Click);
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLeft_MouseDown);
            this.btnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLeft_MouseUp);
            // 
            // btnEditModifiers
            // 
            this.btnEditModifiers.BackColor = System.Drawing.Color.Transparent;
            this.btnEditModifiers.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnEditModifiers.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEditModifiers.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnEditModifiers.FlatAppearance.BorderSize = 0;
            this.btnEditModifiers.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnEditModifiers.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEditModifiers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEditModifiers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditModifiers.ForeColor = System.Drawing.Color.White;
            this.btnEditModifiers.Location = new System.Drawing.Point(1017, 10);
            this.btnEditModifiers.Name = "btnEditModifiers";
            this.btnEditModifiers.Size = new System.Drawing.Size(104, 36);
            this.btnEditModifiers.TabIndex = 125;
            this.btnEditModifiers.Text = "Edit Modifiers";
            this.btnEditModifiers.UseVisualStyleBackColor = false;
            this.btnEditModifiers.Click += new System.EventHandler(this.btnEditModifiers_Click);
            this.btnEditModifiers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnEditModifiers_MouseDown);
            this.btnEditModifiers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnEditModifiers_MouseUp);
            // 
            // lblRemarks
            // 
            this.lblRemarks.Location = new System.Drawing.Point(867, 21);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(130, 30);
            this.lblRemarks.TabIndex = 126;
            this.lblRemarks.Text = "Remarks";
            this.lblRemarks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.Location = new System.Drawing.Point(596, 21);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(175, 30);
            this.lblCardNumber.TabIndex = 126;
            this.lblCardNumber.Text = "Card#";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProductName
            // 
            this.lblProductName.Location = new System.Drawing.Point(45, 21);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(375, 30);
            this.lblProductName.TabIndex = 129;
            this.lblProductName.Text = "Product";
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblParentLine
            // 
            this.lblParentLine.Location = new System.Drawing.Point(269, 21);
            this.lblParentLine.Name = "lblParentLine";
            this.lblParentLine.Size = new System.Drawing.Size(66, 30);
            this.lblParentLine.TabIndex = 127;
            this.lblParentLine.Text = "Parent Line";
            this.lblParentLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblParentLine.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(522, 630);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(116, 34);
            this.btnClose.TabIndex = 122;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // lblPrice
            // 
            this.lblPrice.Location = new System.Drawing.Point(495, 21);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(95, 30);
            this.lblPrice.TabIndex = 126;
            this.lblPrice.Text = "Price";
            this.lblPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLineId
            // 
            this.lblLineId.Location = new System.Drawing.Point(332, 21);
            this.lblLineId.Name = "lblLineId";
            this.lblLineId.Size = new System.Drawing.Size(65, 30);
            this.lblLineId.TabIndex = 127;
            this.lblLineId.Text = "LineId";
            this.lblLineId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLineId.Visible = false;
            // 
            // lblQty
            // 
            this.lblQty.Location = new System.Drawing.Point(425, 21);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(68, 30);
            this.lblQty.TabIndex = 127;
            this.lblQty.Text = "Qty";
            this.lblQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.MistyRose;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 669);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(1188, 22);
            this.txtMessage.TabIndex = 22;
            // 
            // frmProductTrxLineDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1188, 691);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.gpbxTrxProfile);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmProductTrxLineDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product details";
            this.Deactivate += new System.EventHandler(this.frmProductTrxLineDetails_Deactivate);
            this.Load += new System.EventHandler(this.frmProductTrxLineDetails_Load);
            this.gpbxTrxProfile.ResumeLayout(false);
            this.gpbxTrxProfile.PerformLayout();
            this.pnlTrxLineDetails.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbxTrxProfile;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlTrxLineDetails;
        private System.Windows.Forms.Button btnEditModifiers;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblParentLine;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.Label lblLineId;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.Label lblCardNumber;
        private usrCtrlProductTrxLineDetails usrCtrlProductTrxLineDetails1;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.Label lblPageNo;
        private System.Windows.Forms.TextBox txtPageNo;
        private System.Windows.Forms.Label lblPageSize;
        private System.Windows.Forms.TextBox txtPageSize;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView VScrollBarView;
    }
}