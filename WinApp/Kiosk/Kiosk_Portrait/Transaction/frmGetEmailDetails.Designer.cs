using System;

namespace Parafait_Kiosk
{
    partial class frmGetEmailDetails
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
            this.lblGreeting1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.flpCustomerDetails = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.panelUserEntry = new System.Windows.Forms.Panel();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.pbxUserEntry = new System.Windows.Forms.PictureBox();
            this.panelEmail = new System.Windows.Forms.Panel();
            this.txtUserEntryEmail = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.Button();
            this.bigVerticalScrollCustomerDetails = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.lblSkipDetails = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.panelUserEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxUserEntry)).BeginInit();
            this.panelEmail.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.Location = new System.Drawing.Point(34, 27);
            this.btnHome.Size = new System.Drawing.Size(167, 145);
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(14, 217);
            this.btnPrev.Size = new System.Drawing.Size(82, 22);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(140, 1670);
            this.btnCancel.TabIndex = 1055;
            this.btnCancel.Text = "Skip";
            // 
            // btnCart
            // 
            this.btnCart.Location = new System.Drawing.Point(971, 27);
            this.btnCart.Size = new System.Drawing.Size(167, 145);
            // 
            // lblGreeting1
            // 
            this.lblGreeting1.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting1.Font = new System.Drawing.Font("Gotham Rounded Bold", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.White;
            this.lblGreeting1.Location = new System.Drawing.Point(34, 212);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(1034, 82);
            this.lblGreeting1.TabIndex = 149;
            this.lblGreeting1.Text = "Email Details";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Gotham Rounded Bold", 36.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(298, 66);
            this.label1.TabIndex = 1043;
            this.label1.Text = "Enter:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(605, 1670);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(325, 164);
            this.btnOk.TabIndex = 1044;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // flpCustomerDetails
            // 
            this.flpCustomerDetails.AutoScroll = true;
            this.flpCustomerDetails.BackColor = System.Drawing.Color.Transparent;
            this.flpCustomerDetails.Location = new System.Drawing.Point(310, 690);
            this.flpCustomerDetails.Name = "flpCustomerDetails";
            this.flpCustomerDetails.Size = new System.Drawing.Size(730, 635);
            this.flpCustomerDetails.TabIndex = 1056;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Gotham Rounded Bold", 36.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(305, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(571, 86);
            this.label2.TabIndex = 1057;
            this.label2.Text = "OR ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelUserEntry
            // 
            this.panelUserEntry.BackColor = System.Drawing.Color.Transparent;
            this.panelUserEntry.Controls.Add(this.btnShowKeyPad);
            this.panelUserEntry.Controls.Add(this.pbxUserEntry);
            this.panelUserEntry.Controls.Add(this.label1);
            this.panelUserEntry.Controls.Add(this.label2);
            this.panelUserEntry.Controls.Add(this.panelEmail);
            this.panelUserEntry.Location = new System.Drawing.Point(7, 341);
            this.panelUserEntry.Name = "panelUserEntry";
            this.panelUserEntry.Size = new System.Drawing.Size(1049, 330);
            this.panelUserEntry.TabIndex = 1059;
            this.panelUserEntry.Tag = "-1";
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
            this.btnShowKeyPad.Location = new System.Drawing.Point(882, 173);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(87, 83);
            this.btnShowKeyPad.TabIndex = 20001;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.btnShowKeyPad_Click);
            // 
            // pbxUserEntry
            // 
            this.pbxUserEntry.BackColor = System.Drawing.Color.Transparent;
            this.pbxUserEntry.Image = global::Parafait_Kiosk.Properties.Resources.tick_box_unchecked;
            this.pbxUserEntry.Location = new System.Drawing.Point(311, 24);
            this.pbxUserEntry.Name = "pbxUserEntry";
            this.pbxUserEntry.Size = new System.Drawing.Size(111, 111);
            this.pbxUserEntry.TabIndex = 1059;
            this.pbxUserEntry.TabStop = false;
            this.pbxUserEntry.Click += new System.EventHandler(this.pbxCustCheckBox_Click);
            // 
            // panelEmail
            // 
            this.panelEmail.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelEmail.Controls.Add(this.txtUserEntryEmail);
            this.panelEmail.Location = new System.Drawing.Point(425, 35);
            this.panelEmail.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmail.Name = "panelEmail";
            this.panelEmail.Size = new System.Drawing.Size(549, 80);
            this.panelEmail.TabIndex = 1604;
            // 
            // txtUserEntryEmail
            // 
            this.txtUserEntryEmail.BackColor = System.Drawing.Color.White;
            this.txtUserEntryEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserEntryEmail.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserEntryEmail.ForeColor = System.Drawing.Color.Black;
            this.txtUserEntryEmail.Location = new System.Drawing.Point(35, 12);
            this.txtUserEntryEmail.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtUserEntryEmail.Name = "txtUserEntryEmail";
            this.txtUserEntryEmail.Size = new System.Drawing.Size(505, 42);
            this.txtUserEntryEmail.TabIndex = 7;
            this.txtUserEntryEmail.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtUserEntryEmail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Gotham Rounded Bold", 36.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 715);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(298, 66);
            this.label3.TabIndex = 1061;
            this.label3.Text = "Select:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.txtMessage.Location = new System.Drawing.Point(0, 1870);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 50);
            this.txtMessage.TabIndex = 149;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // bigVerticalScrollCustomerDetails
            // 
            this.bigVerticalScrollCustomerDetails.AutoHide = true;
            this.bigVerticalScrollCustomerDetails.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollCustomerDetails.DataGridView = null;
            this.bigVerticalScrollCustomerDetails.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollCustomerDetails.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollCustomerDetails.Location = new System.Drawing.Point(997, 690);
            this.bigVerticalScrollCustomerDetails.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollCustomerDetails.Name = "bigVerticalScrollCustomerDetails";
            this.bigVerticalScrollCustomerDetails.ScrollableControl = this.flpCustomerDetails;
            this.bigVerticalScrollCustomerDetails.ScrollViewer = null;
            this.bigVerticalScrollCustomerDetails.Size = new System.Drawing.Size(63, 635);
            this.bigVerticalScrollCustomerDetails.TabIndex = 1058;
            this.bigVerticalScrollCustomerDetails.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollCustomerDetails.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollCustomerDetails.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollCustomerDetails.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // lblSkipDetails
            // 
            this.lblSkipDetails.BackColor = System.Drawing.Color.Transparent;
            this.lblSkipDetails.Font = new System.Drawing.Font("Gotham Rounded Bold", 28F);
            this.lblSkipDetails.ForeColor = System.Drawing.Color.Transparent;
            this.lblSkipDetails.Location = new System.Drawing.Point(20, 1368);
            this.lblSkipDetails.Name = "lblSkipDetails";
            this.lblSkipDetails.Size = new System.Drawing.Size(1048, 112);
            this.lblSkipDetails.TabIndex = 1062;
            this.lblSkipDetails.Text = "Press \'\'Skip\'\' to proceed without providing your email ";
            this.lblSkipDetails.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblNote
            // 
            this.lblNote.BackColor = System.Drawing.Color.Transparent;
            this.lblNote.Font = new System.Drawing.Font("Gotham Rounded Bold", 28F);
            this.lblNote.ForeColor = System.Drawing.Color.Transparent;
            this.lblNote.Location = new System.Drawing.Point(20, 1494);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(1048, 123);
            this.lblNote.TabIndex = 1063;
            this.lblNote.Text = "Note: if you skip, the receipt will not be emailed";
            this.lblNote.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmGetEmailDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblSkipDetails);
            this.Controls.Add(this.bigVerticalScrollCustomerDetails);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panelUserEntry);
            this.Controls.Add(this.flpCustomerDetails);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblGreeting1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Gotham Rounded Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmGetEmailDetails";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmGetEmailDetails";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmGetEmailDetails_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GetEmailDetails_FormClosed);
            this.Controls.SetChildIndex(this.lblGreeting1, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.flpCustomerDetails, 0);
            this.Controls.SetChildIndex(this.panelUserEntry, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.bigVerticalScrollCustomerDetails, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.lblSkipDetails, 0);
            this.Controls.SetChildIndex(this.lblNote, 0);
            this.panelUserEntry.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxUserEntry)).EndInit();
            this.panelEmail.ResumeLayout(false);
            this.panelEmail.PerformLayout();
            this.ResumeLayout(false);

        } 
        #endregion

        internal System.Windows.Forms.Label lblGreeting1;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.FlowLayoutPanel flpCustomerDetails;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelUserEntry;
        public System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.PictureBox pbxUserEntry;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollCustomerDetails;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Panel panelEmail;
        private System.Windows.Forms.TextBox txtUserEntryEmail;
        private System.Windows.Forms.Label lblSkipDetails;
        private System.Windows.Forms.Label lblNote;
    }
}