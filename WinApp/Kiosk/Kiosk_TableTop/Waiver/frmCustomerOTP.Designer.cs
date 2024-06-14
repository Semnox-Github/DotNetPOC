namespace Parafait_Kiosk
{
    partial class frmCustomerOTP
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
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtOTP = new System.Windows.Forms.TextBox();
            this.lblOTP = new System.Windows.Forms.Label();
            this.lblOTPmsg = new System.Windows.Forms.Label();
            this.linkLblResendOTP = new System.Windows.Forms.LinkLabel();
            this.lblTimeRemaining = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.Controls.Add(this.txtOTP);
            this.panel2.Location = new System.Drawing.Point(710, 392);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(531, 180);
            this.panel2.TabIndex = 1046;
            // 
            // txtOTP
            // 
            this.txtOTP.BackColor = System.Drawing.Color.White;
            this.txtOTP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOTP.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOTP.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtOTP.Location = new System.Drawing.Point(77, 66);
            this.txtOTP.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtOTP.Name = "txtOTP";
            this.txtOTP.Size = new System.Drawing.Size(369, 44);
            this.txtOTP.TabIndex = 1050;
            this.txtOTP.Click += new System.EventHandler(this.textBox_Enter);
            this.txtOTP.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // lblOTP
            // 
            this.lblOTP.BackColor = System.Drawing.Color.Transparent;
            this.lblOTP.Font = new System.Drawing.Font("Gotham Rounded Bold", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOTP.Location = new System.Drawing.Point(200, 30);
            this.lblOTP.Name = "lblOTP";
            this.lblOTP.Size = new System.Drawing.Size(1522, 162);
            this.lblOTP.TabIndex = 1045;
            this.lblOTP.Text = "Enter OTP";
            this.lblOTP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOTPmsg
            // 
            this.lblOTPmsg.BackColor = System.Drawing.Color.Transparent;
            this.lblOTPmsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOTPmsg.Location = new System.Drawing.Point(24, 204);
            this.lblOTPmsg.Name = "lblOTPmsg";
            this.lblOTPmsg.Size = new System.Drawing.Size(1861, 174);
            this.lblOTPmsg.TabIndex = 1044;
            this.lblOTPmsg.Text = "(OTP will be sent to your registered Email ID)";
            this.lblOTPmsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLblResendOTP
            // 
            this.linkLblResendOTP.BackColor = System.Drawing.Color.Transparent;
            this.linkLblResendOTP.DisabledLinkColor = System.Drawing.Color.White;
            this.linkLblResendOTP.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLblResendOTP.LinkColor = System.Drawing.Color.White;
            this.linkLblResendOTP.Location = new System.Drawing.Point(430, 593);
            this.linkLblResendOTP.Name = "linkLblResendOTP";
            this.linkLblResendOTP.Size = new System.Drawing.Size(1078, 65);
            this.linkLblResendOTP.TabIndex = 1047;
            this.linkLblResendOTP.TabStop = true;
            this.linkLblResendOTP.Text = "Resend OTP";
            this.linkLblResendOTP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLblResendOTP.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblResendOTP_LinkClicked);
            // 
            // lblTimeRemaining
            // 
            this.lblTimeRemaining.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeRemaining.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemaining.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.timer_SmallBox;
            this.lblTimeRemaining.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lblTimeRemaining.FlatAppearance.BorderSize = 0;
            this.lblTimeRemaining.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemaining.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemaining.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeRemaining.Font = new System.Drawing.Font("Verdana", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeRemaining.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblTimeRemaining.Location = new System.Drawing.Point(1705, 38);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(180, 144);
            this.lblTimeRemaining.TabIndex = 1048;
            this.lblTimeRemaining.TabStop = false;
            this.lblTimeRemaining.Text = "8";
            this.lblTimeRemaining.UseVisualStyleBackColor = false;
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
            this.txtMessage.Location = new System.Drawing.Point(0, 1030);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 50);
            this.txtMessage.TabIndex = 1052;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // btnOkay
            // 
            this.btnOkay.BackColor = System.Drawing.Color.Transparent;
            this.btnOkay.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnOkay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOkay.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnOkay.FlatAppearance.BorderSize = 0;
            this.btnOkay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkay.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOkay.Location = new System.Drawing.Point(980, 825);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(250, 125);
            this.btnOkay.TabIndex = 1053;
            this.btnOkay.Text = "Ok";
            this.btnOkay.UseVisualStyleBackColor = false;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(680, 825);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(250, 125);
            this.btnCancel.TabIndex = 1055;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Image = global::Parafait_Kiosk.Properties.Resources.Keyboard_1;
            this.btnShowKeyPad.Location = new System.Drawing.Point(1254, 444);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(87, 83);
            this.btnShowKeyPad.TabIndex = 20002;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.btnShowKeyPad_Click);
            // 
            // frmCustomerOTP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.btnShowKeyPad);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblTimeRemaining);
            this.Controls.Add(this.linkLblResendOTP);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblOTP);
            this.Controls.Add(this.lblOTPmsg);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCustomerOTP";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCustomerOTP_Closing);
            this.Load += new System.EventHandler(this.frmCustomerOTP_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Button lblTimeRemaining;
        private System.Windows.Forms.TextBox txtOTP;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblOTP;
        private System.Windows.Forms.Label lblOTPmsg;
        private System.Windows.Forms.LinkLabel linkLblResendOTP;
        private System.Windows.Forms.Button btnShowKeyPad;
    }
}