namespace Parafait_Kiosk
{
    partial class frmLoyalty
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
            this.lblLoyaltyText = new System.Windows.Forms.Label();
            this.btnLoyaltyNo = new System.Windows.Forms.Button();
            this.lblPhoneNo = new System.Windows.Forms.Label();
            this.txtPhoneNo = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnProceedWithoutLoyalty = new System.Windows.Forms.Button();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.pbLoyalty = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtFirstName = new System.Windows.Forms.Label();
            this.flpInfo = new System.Windows.Forms.FlowLayoutPanel();
            this.panelLoyaltyInfo = new System.Windows.Forms.Panel();
            this.btnLoyaltyYes = new System.Windows.Forms.Button();
            this.panelPhoneNoInfo = new System.Windows.Forms.Panel();
            this.panelFirstNameInfo = new System.Windows.Forms.Panel();
            this.listboxNames = new System.Windows.Forms.ListBox();
            this.txtMessage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoyalty)).BeginInit();
            this.flpInfo.SuspendLayout();
            this.panelLoyaltyInfo.SuspendLayout();
            this.panelPhoneNoInfo.SuspendLayout();
            this.panelFirstNameInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblLoyaltyText
            // 
            this.lblLoyaltyText.BackColor = System.Drawing.Color.Transparent;
            this.lblLoyaltyText.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoyaltyText.ForeColor = System.Drawing.Color.White;
            this.lblLoyaltyText.Location = new System.Drawing.Point(3, 10);
            this.lblLoyaltyText.Name = "lblLoyaltyText";
            this.lblLoyaltyText.Size = new System.Drawing.Size(854, 121);
            this.lblLoyaltyText.TabIndex = 0;
            this.lblLoyaltyText.Text = "Are you a More Cheese™ Rewards member?";
            this.lblLoyaltyText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnLoyaltyNo
            // 
            this.btnLoyaltyNo.BackColor = System.Drawing.Color.Transparent;
            this.btnLoyaltyNo.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnLoyaltyNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLoyaltyNo.FlatAppearance.BorderSize = 0;
            this.btnLoyaltyNo.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnLoyaltyNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLoyaltyNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLoyaltyNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoyaltyNo.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoyaltyNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnLoyaltyNo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoyaltyNo.Location = new System.Drawing.Point(515, 134);
            this.btnLoyaltyNo.Name = "btnLoyaltyNo";
            this.btnLoyaltyNo.Size = new System.Drawing.Size(225, 96);
            this.btnLoyaltyNo.TabIndex = 1052;
            this.btnLoyaltyNo.Text = "No";
            this.btnLoyaltyNo.UseVisualStyleBackColor = false;
            this.btnLoyaltyNo.Click += new System.EventHandler(this.btnLoyaltyNo_Click);
            // 
            // lblPhoneNo
            // 
            this.lblPhoneNo.BackColor = System.Drawing.Color.Transparent;
            this.lblPhoneNo.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhoneNo.ForeColor = System.Drawing.Color.White;
            this.lblPhoneNo.Location = new System.Drawing.Point(6, 18);
            this.lblPhoneNo.Name = "lblPhoneNo";
            this.lblPhoneNo.Size = new System.Drawing.Size(851, 49);
            this.lblPhoneNo.TabIndex = 1053;
            this.lblPhoneNo.Text = "Enter Phone Number : ";
            this.lblPhoneNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPhoneNo
            // 
            this.txtPhoneNo.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhoneNo.Location = new System.Drawing.Point(282, 89);
            this.txtPhoneNo.Multiline = true;
            this.txtPhoneNo.Name = "txtPhoneNo";
            this.txtPhoneNo.Size = new System.Drawing.Size(285, 50);
            this.txtPhoneNo.TabIndex = 1054;
            this.txtPhoneNo.Text = "9876543210";
            this.txtPhoneNo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtPhoneNo_MouseClick);
            this.txtPhoneNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPhoneNo_KeyPress);
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGo.BackColor = System.Drawing.Color.Transparent;
            this.btnGo.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnGo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGo.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnGo.FlatAppearance.BorderSize = 0;
            this.btnGo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.Font = new System.Drawing.Font("Gotham Rounded Bold", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.ForeColor = System.Drawing.Color.White;
            this.btnGo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnGo.Location = new System.Drawing.Point(629, 78);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(175, 69);
            this.btnGo.TabIndex = 1055;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnProceedWithoutLoyalty
            // 
            this.btnProceedWithoutLoyalty.BackColor = System.Drawing.Color.Transparent;
            this.btnProceedWithoutLoyalty.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnProceedWithoutLoyalty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProceedWithoutLoyalty.FlatAppearance.BorderSize = 0;
            this.btnProceedWithoutLoyalty.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnProceedWithoutLoyalty.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProceedWithoutLoyalty.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProceedWithoutLoyalty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProceedWithoutLoyalty.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceedWithoutLoyalty.ForeColor = System.Drawing.Color.White;
            this.btnProceedWithoutLoyalty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProceedWithoutLoyalty.Location = new System.Drawing.Point(366, 907);
            this.btnProceedWithoutLoyalty.Name = "btnProceedWithoutLoyalty";
            this.btnProceedWithoutLoyalty.Size = new System.Drawing.Size(225, 96);
            this.btnProceedWithoutLoyalty.TabIndex = 1058;
            this.btnProceedWithoutLoyalty.Text = "Proceed Without Loyalty";
            this.btnProceedWithoutLoyalty.UseVisualStyleBackColor = false;
            this.btnProceedWithoutLoyalty.Click += new System.EventHandler(this.btnProceedWithoutLoyalty_Click);
            // 
            // lblFirstName
            // 
            this.lblFirstName.BackColor = System.Drawing.Color.Transparent;
            this.lblFirstName.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstName.ForeColor = System.Drawing.Color.White;
            this.lblFirstName.Location = new System.Drawing.Point(0, -5);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(872, 75);
            this.lblFirstName.TabIndex = 1056;
            this.lblFirstName.Text = "First Name ";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(600, 907);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(225, 96);
            this.btnOk.TabIndex = 1058;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pbLoyalty
            // 
            this.pbLoyalty.BackColor = System.Drawing.Color.Transparent;
            this.pbLoyalty.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.LoyaltyFormLogoImage;
            this.pbLoyalty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbLoyalty.Location = new System.Drawing.Point(47, 52);
            this.pbLoyalty.Name = "pbLoyalty";
            this.pbLoyalty.Size = new System.Drawing.Size(875, 172);
            this.pbLoyalty.TabIndex = 1059;
            this.pbLoyalty.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(135, 912);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(225, 96);
            this.btnCancel.TabIndex = 1061;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtFirstName
            // 
            this.txtFirstName.BackColor = System.Drawing.SystemColors.Window;
            this.txtFirstName.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.Location = new System.Drawing.Point(282, 156);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(285, 49);
            this.txtFirstName.TabIndex = 0;
            this.txtFirstName.Text = "XXXXXXX";
            this.txtFirstName.Visible = false;
            // 
            // flpInfo
            // 
            this.flpInfo.BackColor = System.Drawing.Color.Transparent;
            this.flpInfo.Controls.Add(this.panelLoyaltyInfo);
            this.flpInfo.Controls.Add(this.panelPhoneNoInfo);
            this.flpInfo.Controls.Add(this.panelFirstNameInfo);
            this.flpInfo.Location = new System.Drawing.Point(47, 230);
            this.flpInfo.Name = "flpInfo";
            this.flpInfo.Size = new System.Drawing.Size(890, 671);
            this.flpInfo.TabIndex = 1063;
            // 
            // panelLoyaltyInfo
            // 
            this.panelLoyaltyInfo.Controls.Add(this.btnLoyaltyYes);
            this.panelLoyaltyInfo.Controls.Add(this.lblLoyaltyText);
            this.panelLoyaltyInfo.Controls.Add(this.btnLoyaltyNo);
            this.panelLoyaltyInfo.Location = new System.Drawing.Point(3, 3);
            this.panelLoyaltyInfo.Name = "panelLoyaltyInfo";
            this.panelLoyaltyInfo.Size = new System.Drawing.Size(872, 243);
            this.panelLoyaltyInfo.TabIndex = 0;
            // 
            // btnLoyaltyYes
            // 
            this.btnLoyaltyYes.BackColor = System.Drawing.Color.Transparent;
            this.btnLoyaltyYes.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnLoyaltyYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLoyaltyYes.FlatAppearance.BorderSize = 0;
            this.btnLoyaltyYes.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnLoyaltyYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLoyaltyYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLoyaltyYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoyaltyYes.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoyaltyYes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnLoyaltyYes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoyaltyYes.Location = new System.Drawing.Point(142, 134);
            this.btnLoyaltyYes.Name = "btnLoyaltyYes";
            this.btnLoyaltyYes.Size = new System.Drawing.Size(225, 96);
            this.btnLoyaltyYes.TabIndex = 1053;
            this.btnLoyaltyYes.Text = "Yes";
            this.btnLoyaltyYes.UseVisualStyleBackColor = false;
            this.btnLoyaltyYes.Click += new System.EventHandler(this.btnLoyaltyYes_Click);
            // 
            // panelPhoneNoInfo
            // 
            this.panelPhoneNoInfo.BackColor = System.Drawing.Color.Transparent;
            this.panelPhoneNoInfo.Controls.Add(this.lblPhoneNo);
            this.panelPhoneNoInfo.Controls.Add(this.txtPhoneNo);
            this.panelPhoneNoInfo.Controls.Add(this.btnGo);
            this.panelPhoneNoInfo.Location = new System.Drawing.Point(3, 252);
            this.panelPhoneNoInfo.Name = "panelPhoneNoInfo";
            this.panelPhoneNoInfo.Size = new System.Drawing.Size(872, 179);
            this.panelPhoneNoInfo.TabIndex = 1064;
            // 
            // panelFirstNameInfo
            // 
            this.panelFirstNameInfo.BackColor = System.Drawing.Color.Transparent;
            this.panelFirstNameInfo.Controls.Add(this.listboxNames);
            this.panelFirstNameInfo.Controls.Add(this.txtFirstName);
            this.panelFirstNameInfo.Controls.Add(this.lblFirstName);
            this.panelFirstNameInfo.Location = new System.Drawing.Point(3, 437);
            this.panelFirstNameInfo.Name = "panelFirstNameInfo";
            this.panelFirstNameInfo.Size = new System.Drawing.Size(872, 226);
            this.panelFirstNameInfo.TabIndex = 1064;
            // 
            // listboxNames
            // 
            this.listboxNames.Font = new System.Drawing.Font("Gotham Rounded Bold", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listboxNames.FormattingEnabled = true;
            this.listboxNames.ItemHeight = 32;
            this.listboxNames.Location = new System.Drawing.Point(282, 73);
            this.listboxNames.Name = "listboxNames";
            this.listboxNames.Size = new System.Drawing.Size(285, 68);
            this.listboxNames.Sorted = true;
            this.listboxNames.TabIndex = 1057;
            this.listboxNames.SelectedValueChanged += new System.EventHandler(this.listboxNames_SelectedValueChanged);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(26, 1024);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(922, 44);
            this.txtMessage.TabIndex = 1065;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // frmLoyalty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.LoyaltyFormBackgroundImage;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(949, 1080);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.flpInfo);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnProceedWithoutLoyalty);
            this.Controls.Add(this.pbLoyalty);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLoyalty";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLoyalty_FormClosing);
            this.Load += new System.EventHandler(this.frmLoyalty_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmLoyalty_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoyalty)).EndInit();
            this.flpInfo.ResumeLayout(false);
            this.panelLoyaltyInfo.ResumeLayout(false);
            this.panelPhoneNoInfo.ResumeLayout(false);
            this.panelPhoneNoInfo.PerformLayout();
            this.panelFirstNameInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblLoyaltyText;
        private System.Windows.Forms.Button btnLoyaltyNo;
        private System.Windows.Forms.Label lblPhoneNo;
        private System.Windows.Forms.TextBox txtPhoneNo;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.PictureBox pbLoyalty;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FlowLayoutPanel flpInfo;
        private System.Windows.Forms.Panel panelLoyaltyInfo;
        private System.Windows.Forms.Panel panelPhoneNoInfo;
        private System.Windows.Forms.Panel panelFirstNameInfo;
        private System.Windows.Forms.Label txtFirstName;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button btnLoyaltyYes;
        private System.Windows.Forms.Button btnProceedWithoutLoyalty;
        private System.Windows.Forms.ListBox listboxNames;
    }
}
