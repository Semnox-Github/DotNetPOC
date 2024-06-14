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
            this.lblScanCoupon = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblCoupon = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtCouponNo = new System.Windows.Forms.Label();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblScanCoupon
            // 
            this.lblScanCoupon.BackColor = System.Drawing.Color.Transparent;
            this.lblScanCoupon.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScanCoupon.ForeColor = System.Drawing.Color.White;
            this.lblScanCoupon.Location = new System.Drawing.Point(26, 29);
            this.lblScanCoupon.Name = "lblScanCoupon";
            this.lblScanCoupon.Size = new System.Drawing.Size(716, 75);
            this.lblScanCoupon.TabIndex = 0;
            this.lblScanCoupon.Text = "Scan Your Coupon";
            this.lblScanCoupon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClose.Location = new System.Drawing.Point(416, 267);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(269, 80);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.Transparent;
            this.btnApply.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnApply.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnApply.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnApply.FlatAppearance.BorderSize = 0;
            this.btnApply.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnApply.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnApply.Location = new System.Drawing.Point(74, 267);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(269, 80);
            this.btnApply.TabIndex = 13;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lblCoupon
            // 
            this.lblCoupon.BackColor = System.Drawing.Color.Transparent;
            this.lblCoupon.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCoupon.ForeColor = System.Drawing.Color.White;
            this.lblCoupon.Location = new System.Drawing.Point(45, 150);
            this.lblCoupon.Name = "lblCoupon";
            this.lblCoupon.Size = new System.Drawing.Size(275, 42);
            this.lblCoupon.TabIndex = 14;
            this.lblCoupon.Text = "Coupon Number : ";
            this.lblCoupon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.CouponNoPanel;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.txtCouponNo);
            this.panel1.Location = new System.Drawing.Point(326, 141);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(416, 61);
            this.panel1.TabIndex = 15;
            // 
            // txtCouponNo
            // 
            this.txtCouponNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCouponNo.Location = new System.Drawing.Point(15, 9);
            this.txtCouponNo.Name = "txtCouponNo";
            this.txtCouponNo.Size = new System.Drawing.Size(386, 52);
            this.txtCouponNo.TabIndex = 0;
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Image = global::Parafait_Kiosk.Properties.Resources.Keyboard_1;
            this.btnShowKeyPad.Location = new System.Drawing.Point(659, 208);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(83, 44);
            this.btnShowKeyPad.TabIndex = 20002;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.btnShowKeyPad_Click);
            // 
            // frmScanCoupon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(781, 414);
            this.Controls.Add(this.btnShowKeyPad);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblCoupon);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblScanCoupon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmScanCoupon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmScanCoupon";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmScanCoupon_FormClosing);
            this.Load += new System.EventHandler(this.frmScanCoupon_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblScanCoupon;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lblCoupon;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label txtCouponNo;
        private System.Windows.Forms.Button btnShowKeyPad;
    }
}