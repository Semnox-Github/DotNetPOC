namespace Parafait_Kiosk
{
    partial class frmScanCoupon
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
            this.lblCoupon = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.txtCouponNo = new System.Windows.Forms.Label();
            this.panelCouponNo = new System.Windows.Forms.Panel();
            this.panelCouponNo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCoupon
            // 
            this.lblCoupon.BackColor = System.Drawing.Color.Transparent;
            this.lblCoupon.Font = new System.Drawing.Font("Bango Pro", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCoupon.ForeColor = System.Drawing.Color.White;
            this.lblCoupon.Location = new System.Drawing.Point(136, 110);
            this.lblCoupon.Name = "lblCoupon";
            this.lblCoupon.Size = new System.Drawing.Size(344, 54);
            this.lblCoupon.TabIndex = 0;
            this.lblCoupon.Text = "Coupon Number:";
            this.lblCoupon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClose.Location = new System.Drawing.Point(533, 194);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(347, 110);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.Transparent;
            this.btnApply.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnApply.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnApply.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnApply.FlatAppearance.BorderSize = 0;
            this.btnApply.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnApply.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnApply.Location = new System.Drawing.Point(125, 194);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(347, 110);
            this.btnApply.TabIndex = 16;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // label1
            // 
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Bango Pro", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(44, 9);
            this.lblHeader.Name = "label1";
            this.lblHeader.Size = new System.Drawing.Size(909, 64);
            this.lblHeader.TabIndex = 17;
            this.lblHeader.Text = "Scan Your Coupon";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCouponNo
            // 
            this.txtCouponNo.BackColor = System.Drawing.Color.Transparent;
            this.txtCouponNo.Font = new System.Drawing.Font("Bango Pro", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCouponNo.Location = new System.Drawing.Point(3, 0);
            this.txtCouponNo.Name = "txtCouponNo";
            this.txtCouponNo.Size = new System.Drawing.Size(385, 54);
            this.txtCouponNo.TabIndex = 18;
            this.txtCouponNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelCouponNo
            // 
            this.panelCouponNo.BackColor = System.Drawing.Color.Transparent;
            this.panelCouponNo.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.CouponNoPanel;
            this.panelCouponNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelCouponNo.Controls.Add(this.txtCouponNo);
            this.panelCouponNo.Location = new System.Drawing.Point(464, 110);
            this.panelCouponNo.Name = "panelCouponNo";
            this.panelCouponNo.Size = new System.Drawing.Size(394, 64);
            this.panelCouponNo.TabIndex = 19;
            // 
            // frmScanCoupon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(986, 377);
            this.Controls.Add(this.panelCouponNo);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblCoupon);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmScanCoupon";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmScanCoupon";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmScanCoupon_FormClosing);
            this.Load += new System.EventHandler(this.frmScanCoupon_Load);
            this.panelCouponNo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCoupon;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label txtCouponNo;
        private System.Windows.Forms.Panel panelCouponNo;
    }
}