namespace Parafait_POS.Redemption
{
    partial class frmCapillaryRedemption
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
            this.grpCouponPayment = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCouponNumber = new System.Windows.Forms.TextBox();
            this.grpPointsPayment = new System.Windows.Forms.GroupBox();
            this.btnOtherPayment = new System.Windows.Forms.Button();
            this.btnPointOK = new System.Windows.Forms.Button();
            this.lblValidationCode = new System.Windows.Forms.Label();
            this.txtValidationCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPoints = new System.Windows.Forms.TextBox();
            this.rdbtnCoupon = new System.Windows.Forms.RadioButton();
            this.rdbtnPoints = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkboxRedemption = new System.Windows.Forms.CheckBox();
            this.lblApplyRedemption = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblAvailablePoints = new System.Windows.Forms.Label();
            this.btnShowNumPad = new System.Windows.Forms.Button();
            this.grpCouponPayment.SuspendLayout();
            this.grpPointsPayment.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCouponPayment
            // 
            this.grpCouponPayment.Controls.Add(this.label2);
            this.grpCouponPayment.Controls.Add(this.txtCouponNumber);
            this.grpCouponPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCouponPayment.Location = new System.Drawing.Point(17, 90);
            this.grpCouponPayment.Name = "grpCouponPayment";
            this.grpCouponPayment.Size = new System.Drawing.Size(278, 108);
            this.grpCouponPayment.TabIndex = 21;
            this.grpCouponPayment.TabStop = false;
            this.grpCouponPayment.Text = "Coupon Payment";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enter Coupon Number :";
            // 
            // txtCouponNumber
            // 
            this.txtCouponNumber.Location = new System.Drawing.Point(153, 34);
            this.txtCouponNumber.Name = "txtCouponNumber";
            this.txtCouponNumber.Size = new System.Drawing.Size(116, 20);
            this.txtCouponNumber.TabIndex = 1;
            // 
            // grpPointsPayment
            // 
            this.grpPointsPayment.Controls.Add(this.btnOtherPayment);
            this.grpPointsPayment.Controls.Add(this.btnPointOK);
            this.grpPointsPayment.Controls.Add(this.lblValidationCode);
            this.grpPointsPayment.Controls.Add(this.txtValidationCode);
            this.grpPointsPayment.Controls.Add(this.label1);
            this.grpPointsPayment.Controls.Add(this.txtPoints);
            this.grpPointsPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPointsPayment.Location = new System.Drawing.Point(301, 91);
            this.grpPointsPayment.Name = "grpPointsPayment";
            this.grpPointsPayment.Size = new System.Drawing.Size(310, 107);
            this.grpPointsPayment.TabIndex = 22;
            this.grpPointsPayment.TabStop = false;
            this.grpPointsPayment.Text = "Points Payment";
            // 
            // btnOtherPayment
            // 
            this.btnOtherPayment.BackColor = System.Drawing.Color.Transparent;
            this.btnOtherPayment.BackgroundImage = global::Parafait_POS.Properties.Resources.Keypad;
            this.btnOtherPayment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOtherPayment.CausesValidation = false;
            this.btnOtherPayment.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnOtherPayment.FlatAppearance.BorderSize = 0;
            this.btnOtherPayment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOtherPayment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOtherPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOtherPayment.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOtherPayment.ForeColor = System.Drawing.Color.White;
            this.btnOtherPayment.Location = new System.Drawing.Point(252, 31);
            this.btnOtherPayment.Name = "btnOtherPayment";
            this.btnOtherPayment.Size = new System.Drawing.Size(21, 22);
            this.btnOtherPayment.TabIndex = 31;
            this.btnOtherPayment.UseVisualStyleBackColor = false;
            this.btnOtherPayment.Click += new System.EventHandler(this.btnOtherPayment_Click);
            // 
            // btnPointOK
            // 
            this.btnPointOK.BackColor = System.Drawing.Color.Transparent;
            this.btnPointOK.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPointOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPointOK.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPointOK.FlatAppearance.BorderSize = 0;
            this.btnPointOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPointOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPointOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPointOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPointOK.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPointOK.ForeColor = System.Drawing.Color.White;
            this.btnPointOK.Location = new System.Drawing.Point(276, 30);
            this.btnPointOK.Name = "btnPointOK";
            this.btnPointOK.Size = new System.Drawing.Size(34, 24);
            this.btnPointOK.TabIndex = 30;
            this.btnPointOK.Text = "OK";
            this.btnPointOK.UseVisualStyleBackColor = false;
            this.btnPointOK.Click += new System.EventHandler(this.btnPointOK_Click);
            // 
            // lblValidationCode
            // 
            this.lblValidationCode.AutoSize = true;
            this.lblValidationCode.Location = new System.Drawing.Point(18, 70);
            this.lblValidationCode.Name = "lblValidationCode";
            this.lblValidationCode.Size = new System.Drawing.Size(104, 13);
            this.lblValidationCode.TabIndex = 3;
            this.lblValidationCode.Text = "Validation Code :";
            // 
            // txtValidationCode
            // 
            this.txtValidationCode.Location = new System.Drawing.Point(130, 67);
            this.txtValidationCode.Name = "txtValidationCode";
            this.txtValidationCode.Size = new System.Drawing.Size(116, 20);
            this.txtValidationCode.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter Points :";
            // 
            // txtPoints
            // 
            this.txtPoints.Location = new System.Drawing.Point(130, 31);
            this.txtPoints.Name = "txtPoints";
            this.txtPoints.Size = new System.Drawing.Size(116, 20);
            this.txtPoints.TabIndex = 0;
            this.txtPoints.Enter += new System.EventHandler(this.txtPoints_Enter);
            this.txtPoints.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPoints_KeyPress);
            this.txtPoints.Leave += new System.EventHandler(this.txtPoints_Leave);
            // 
            // rdbtnCoupon
            // 
            this.rdbtnCoupon.AutoSize = true;
            this.rdbtnCoupon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbtnCoupon.Location = new System.Drawing.Point(17, 51);
            this.rdbtnCoupon.Name = "rdbtnCoupon";
            this.rdbtnCoupon.Size = new System.Drawing.Size(100, 17);
            this.rdbtnCoupon.TabIndex = 23;
            this.rdbtnCoupon.TabStop = true;
            this.rdbtnCoupon.Text = "Coupon Type";
            this.rdbtnCoupon.UseVisualStyleBackColor = true;
            this.rdbtnCoupon.CheckedChanged += new System.EventHandler(this.rdbtnCoupon_CheckedChanged);
            // 
            // rdbtnPoints
            // 
            this.rdbtnPoints.AutoSize = true;
            this.rdbtnPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbtnPoints.Location = new System.Drawing.Point(301, 51);
            this.rdbtnPoints.Name = "rdbtnPoints";
            this.rdbtnPoints.Size = new System.Drawing.Size(60, 17);
            this.rdbtnPoints.TabIndex = 24;
            this.rdbtnPoints.TabStop = true;
            this.rdbtnPoints.Text = "Points";
            this.rdbtnPoints.UseVisualStyleBackColor = true;
            this.rdbtnPoints.CheckedChanged += new System.EventHandler(this.rdbtnPoints_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(170, 204);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(103, 30);
            this.btnOk.TabIndex = 25;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(322, 204);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(103, 30);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkboxRedemption
            // 
            this.chkboxRedemption.AutoSize = true;
            this.chkboxRedemption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkboxRedemption.Location = new System.Drawing.Point(301, 17);
            this.chkboxRedemption.Name = "chkboxRedemption";
            this.chkboxRedemption.Size = new System.Drawing.Size(15, 14);
            this.chkboxRedemption.TabIndex = 27;
            this.chkboxRedemption.UseVisualStyleBackColor = true;
            this.chkboxRedemption.CheckedChanged += new System.EventHandler(this.chkboxRedemption_CheckedChanged);
            // 
            // lblApplyRedemption
            // 
            this.lblApplyRedemption.AutoSize = true;
            this.lblApplyRedemption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplyRedemption.Location = new System.Drawing.Point(16, 15);
            this.lblApplyRedemption.Name = "lblApplyRedemption";
            this.lblApplyRedemption.Size = new System.Drawing.Size(277, 16);
            this.lblApplyRedemption.TabIndex = 28;
            this.lblApplyRedemption.Text = "Do you want to apply the Redemption ?";
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.BackColor = System.Drawing.Color.LightYellow;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(-1, 247);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(626, 29);
            this.txtMessage.TabIndex = 29;
            this.txtMessage.TabStop = false;
            // 
            // lblAvailablePoints
            // 
            this.lblAvailablePoints.AutoSize = true;
            this.lblAvailablePoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblAvailablePoints.Location = new System.Drawing.Point(428, 75);
            this.lblAvailablePoints.Name = "lblAvailablePoints";
            this.lblAvailablePoints.Size = new System.Drawing.Size(106, 13);
            this.lblAvailablePoints.TabIndex = 30;
            this.lblAvailablePoints.Text = "Available Points :";
            // 
            // btnShowNumPad
            // 
            this.btnShowNumPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowNumPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.CausesValidation = false;
            this.btnShowNumPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowNumPad.FlatAppearance.BorderSize = 0;
            this.btnShowNumPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowNumPad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowNumPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowNumPad.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnShowNumPad.Location = new System.Drawing.Point(575, 207);
            this.btnShowNumPad.Name = "btnShowNumPad";
            this.btnShowNumPad.Size = new System.Drawing.Size(36, 34);
            this.btnShowNumPad.TabIndex = 31;
            this.btnShowNumPad.UseVisualStyleBackColor = false;
            this.btnShowNumPad.Click += new System.EventHandler(this.btnShowNumPad_Click);
            // 
            // frmMainRedemption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 276);
            this.Controls.Add(this.btnShowNumPad);
            this.Controls.Add(this.lblAvailablePoints);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblApplyRedemption);
            this.Controls.Add(this.chkboxRedemption);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.rdbtnPoints);
            this.Controls.Add(this.rdbtnCoupon);
            this.Controls.Add(this.grpPointsPayment);
            this.Controls.Add(this.grpCouponPayment);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMainRedemption";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Redemption";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMainRedemption_FormClosing);
            this.grpCouponPayment.ResumeLayout(false);
            this.grpCouponPayment.PerformLayout();
            this.grpPointsPayment.ResumeLayout(false);
            this.grpPointsPayment.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCouponPayment;
        private System.Windows.Forms.GroupBox grpPointsPayment;
        private System.Windows.Forms.RadioButton rdbtnCoupon;
        private System.Windows.Forms.RadioButton rdbtnPoints;
        private System.Windows.Forms.TextBox txtCouponNumber;
        private System.Windows.Forms.TextBox txtPoints;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkboxRedemption;
        private System.Windows.Forms.Label lblValidationCode;
        private System.Windows.Forms.TextBox txtValidationCode;
        private System.Windows.Forms.Label lblApplyRedemption;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnPointOK;
        private System.Windows.Forms.Label lblAvailablePoints;
        private System.Windows.Forms.Button btnShowNumPad;
        private System.Windows.Forms.Button btnOtherPayment;
    }
}