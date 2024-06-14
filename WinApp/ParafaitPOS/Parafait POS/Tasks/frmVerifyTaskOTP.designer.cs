using System.Windows.Forms;

namespace Parafait_POS
{
    partial class frmVerifyTaskOTP
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
            this.lblEnterOTP = new System.Windows.Forms.Label();
            this.lblContactMsg = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnResend = new System.Windows.Forms.Button();
            this.txtUserEnteredOTP = new System.Windows.Forms.TextBox();
            this.lblTimeValue = new System.Windows.Forms.Label();
            this.lblTimerUOM = new System.Windows.Forms.Label();
            this.btnOverrideOTP = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblEnterOTP
            // 
            this.lblEnterOTP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnterOTP.Location = new System.Drawing.Point(37, 65);
            this.lblEnterOTP.Name = "lblEnterOTP";
            this.lblEnterOTP.Size = new System.Drawing.Size(488, 19);
            this.lblEnterOTP.TabIndex = 15;
            this.lblEnterOTP.Text = "Please enter customer provided OTP to verify the Customer";
            this.lblEnterOTP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblContactMsg
            // 
            this.lblContactMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContactMsg.Location = new System.Drawing.Point(37, 13);
            this.lblContactMsg.Name = "lblContactMsg";
            this.lblContactMsg.Size = new System.Drawing.Size(488, 44);
            this.lblContactMsg.TabIndex = 16;
            this.lblContactMsg.Text = "A OTP code has been sent to customer contact 9876543210 and testmail@testmail.com" +
    "";
            this.lblContactMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(354, 354);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(155, 55);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnCancel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnResend
            // 
            this.btnResend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResend.BackColor = System.Drawing.Color.Transparent;
            this.btnResend.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnResend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnResend.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnResend.FlatAppearance.BorderSize = 0;
            this.btnResend.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnResend.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnResend.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnResend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResend.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResend.ForeColor = System.Drawing.Color.White;
            this.btnResend.Location = new System.Drawing.Point(354, 212);
            this.btnResend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnResend.Name = "btnResend";
            this.btnResend.Size = new System.Drawing.Size(155, 55);
            this.btnResend.TabIndex = 3;
            this.btnResend.Text = "Resend OTP";
            this.btnResend.UseVisualStyleBackColor = false;
            this.btnResend.Click += new System.EventHandler(this.btnResend_Click);
            this.btnResend.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnResend.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // txtUserEnteredOTP
            // 
            this.txtUserEnteredOTP.Location = new System.Drawing.Point(41, 92);
            this.txtUserEnteredOTP.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserEnteredOTP.MaximumSize = new System.Drawing.Size(200, 41);
            this.txtUserEnteredOTP.MaxLength = 7;
            this.txtUserEnteredOTP.MinimumSize = new System.Drawing.Size(200, 41);
            this.txtUserEnteredOTP.Multiline = true;
            this.txtUserEnteredOTP.Name = "txtUserEnteredOTP";
            this.txtUserEnteredOTP.ReadOnly = true;
            this.txtUserEnteredOTP.Size = new System.Drawing.Size(200, 41);
            this.txtUserEnteredOTP.TabIndex = 1;
            this.txtUserEnteredOTP.Visible = false;
            // 
            // lblTimeValue
            // 
            this.lblTimeValue.BackColor = System.Drawing.Color.MistyRose;
            this.lblTimeValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTimeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeValue.Location = new System.Drawing.Point(357, 108);
            this.lblTimeValue.Name = "lblTimeValue";
            this.lblTimeValue.Size = new System.Drawing.Size(54, 22);
            this.lblTimeValue.TabIndex = 20;
            this.lblTimeValue.Text = "02:00";
            this.lblTimeValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTimerUOM
            // 
            this.lblTimerUOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimerUOM.Location = new System.Drawing.Point(415, 110);
            this.lblTimerUOM.Name = "lblTimerUOM";
            this.lblTimerUOM.Size = new System.Drawing.Size(103, 19);
            this.lblTimerUOM.TabIndex = 21;
            this.lblTimerUOM.Text = "Minutes";
            this.lblTimerUOM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOverrideOTP
            // 
            this.btnOverrideOTP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOverrideOTP.BackColor = System.Drawing.Color.Transparent;
            this.btnOverrideOTP.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnOverrideOTP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOverrideOTP.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnOverrideOTP.FlatAppearance.BorderSize = 0;
            this.btnOverrideOTP.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOverrideOTP.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOverrideOTP.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOverrideOTP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOverrideOTP.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOverrideOTP.ForeColor = System.Drawing.Color.White;
            this.btnOverrideOTP.Location = new System.Drawing.Point(354, 283);
            this.btnOverrideOTP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOverrideOTP.Name = "btnOverrideOTP";
            this.btnOverrideOTP.Size = new System.Drawing.Size(155, 55);
            this.btnOverrideOTP.TabIndex = 4;
            this.btnOverrideOTP.Text = "Override OTP";
            this.btnOverrideOTP.UseVisualStyleBackColor = false;
            this.btnOverrideOTP.Click += new System.EventHandler(this.btnOverrideOTP_Click);
            this.btnOverrideOTP.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnOverrideOTP.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.MistyRose;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 412);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(552, 22);
            this.txtMessage.TabIndex = 22;
            // 
            // frmVerifyTaskOTP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(552, 434);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnOverrideOTP);
            this.Controls.Add(this.lblTimerUOM);
            this.Controls.Add(this.lblTimeValue);
            this.Controls.Add(this.txtUserEnteredOTP);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnResend);
            this.Controls.Add(this.lblContactMsg);
            this.Controls.Add(this.lblEnterOTP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVerifyTaskOTP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Verify Generic OTP";
            this.Load += new System.EventHandler(this.frmVerifyGenericOTP_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion 
        private System.Windows.Forms.Label lblEnterOTP;
        private System.Windows.Forms.Label lblContactMsg;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnResend;
        private System.Windows.Forms.TextBox txtUserEnteredOTP;
        private System.Windows.Forms.Label lblTimeValue;
        private System.Windows.Forms.Label lblTimerUOM;
        private System.Windows.Forms.Button btnOverrideOTP;
        private TextBox txtMessage;
    }
}