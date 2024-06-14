using System;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    partial class frmSignWaiverFiles
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
            this.lblSample = new System.Windows.Forms.Label();
            this.lblSignWaiverFile = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.Button();
            this.pnlMaster = new System.Windows.Forms.Panel();
            this.bigVerticalScrollWaiver = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.pnlWaiver = new System.Windows.Forms.Panel();
            this.lblCustomerContact = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.btnOkay = new System.Windows.Forms.Button();
            this.pnlWaiverDisplay = new System.Windows.Forms.Panel();
            this.pbCheckBox = new System.Windows.Forms.PictureBox();
            this.chkSignConfirm = new System.Windows.Forms.CheckBox();
            this.pnlSignature = new System.Windows.Forms.Panel();
            this.pnlMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheckBox)).BeginInit();
            this.pnlSignature.SuspendLayout();
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
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //this.btnCancel.Location = new System.Drawing.Point(605, 1670);
            this.btnCancel.Location = new System.Drawing.Point(140, 1670);
            this.btnCancel.TabIndex = 1027;
            // 
            // lblSample
            // 
            this.lblSample.BackColor = System.Drawing.Color.Maroon;
            this.lblSample.Font = new System.Drawing.Font("Gotham Rounded Bold", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSample.ForeColor = System.Drawing.Color.White;
            this.lblSample.Location = new System.Drawing.Point(1, 1);
            this.lblSample.Name = "lblSample";
            this.lblSample.Size = new System.Drawing.Size(1, 1);
            this.lblSample.TabIndex = 1;
            this.lblSample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSample.Visible = false;
            // 
            // lblSignWaiverFile
            // 
            this.lblSignWaiverFile.BackColor = System.Drawing.Color.Transparent;
            this.lblSignWaiverFile.Font = new System.Drawing.Font("Gotham Rounded Bold", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignWaiverFile.ForeColor = System.Drawing.Color.White;
            this.lblSignWaiverFile.Location = new System.Drawing.Point(34, 182);
            this.lblSignWaiverFile.Name = "lblSignWaiverFile";
            this.lblSignWaiverFile.Size = new System.Drawing.Size(1016, 86);
            this.lblSignWaiverFile.TabIndex = 1;
            this.lblSignWaiverFile.Text = "Waiver file name";
            this.lblSignWaiverFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.txtMessage.TabIndex = 136;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // pnlMaster
            // 
            this.pnlMaster.BackColor = System.Drawing.Color.Transparent;
            this.pnlMaster.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Table1;
            this.pnlMaster.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlMaster.Controls.Add(this.bigVerticalScrollWaiver);
            this.pnlMaster.Controls.Add(this.pnlWaiver);
            this.pnlMaster.Controls.Add(this.lblCustomerContact);
            this.pnlMaster.Controls.Add(this.lblCustomerName);
            this.pnlMaster.Location = new System.Drawing.Point(60, 299);
            this.pnlMaster.Name = "pnlMaster";
            this.pnlMaster.Size = new System.Drawing.Size(959, 408);
            this.pnlMaster.TabIndex = 163;
            // 
            // bigVerticalScrollWaiver
            // 
            this.bigVerticalScrollWaiver.AutoHide = false;
            this.bigVerticalScrollWaiver.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollWaiver.DataGridView = null;
            this.bigVerticalScrollWaiver.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollWaiver.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollWaiver.Location = new System.Drawing.Point(880, 49);
            this.bigVerticalScrollWaiver.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollWaiver.Name = "bigVerticalScrollWaiver";
            this.bigVerticalScrollWaiver.ScrollableControl = this.pnlWaiver;
            this.bigVerticalScrollWaiver.ScrollViewer = null;
            this.bigVerticalScrollWaiver.Size = new System.Drawing.Size(63, 341);
            this.bigVerticalScrollWaiver.TabIndex = 165;
            this.bigVerticalScrollWaiver.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollWaiver.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollWaiver.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollWaiver.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // pnlWaiver
            // 
            this.pnlWaiver.AutoScroll = true;
            this.pnlWaiver.ForeColor = System.Drawing.Color.White;
            this.pnlWaiver.Location = new System.Drawing.Point(19, 49);
            this.pnlWaiver.MaximumSize = new System.Drawing.Size(874, 0);
            this.pnlWaiver.MinimumSize = new System.Drawing.Size(874, 338);
            this.pnlWaiver.Name = "pnlWaiver";
            this.pnlWaiver.Size = new System.Drawing.Size(874, 338);
            this.pnlWaiver.TabIndex = 167;
            // 
            // lblCustomerContact
            // 
            this.lblCustomerContact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCustomerContact.AutoEllipsis = true;
            this.lblCustomerContact.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomerContact.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerContact.ForeColor = System.Drawing.Color.Thistle;
            this.lblCustomerContact.Location = new System.Drawing.Point(414, 0);
            this.lblCustomerContact.Name = "lblCustomerContact";
            this.lblCustomerContact.Size = new System.Drawing.Size(407, 49);
            this.lblCustomerContact.TabIndex = 166;
            this.lblCustomerContact.Text = "Email ID";
            this.lblCustomerContact.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCustomerName.AutoEllipsis = true;
            this.lblCustomerName.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomerName.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.ForeColor = System.Drawing.Color.Thistle;
            this.lblCustomerName.Location = new System.Drawing.Point(3, 0);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(401, 49);
            this.lblCustomerName.TabIndex = 163;
            this.lblCustomerName.Text = "Name";
            this.lblCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOkay
            // 
            this.btnOkay.BackColor = System.Drawing.Color.Transparent;
            this.btnOkay.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnOkay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOkay.FlatAppearance.BorderSize = 0;
            this.btnOkay.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkay.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOkay.ForeColor = System.Drawing.Color.White;
            this.btnOkay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //this.btnOkay.Location = new System.Drawing.Point(140, 1670);
            this.btnOkay.Location = new System.Drawing.Point(605, 1670);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(325, 164);
            this.btnOkay.TabIndex = 1028;
            this.btnOkay.Text = "Ok";
            this.btnOkay.UseVisualStyleBackColor = false;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // pnlWaiverDisplay
            // 
            this.pnlWaiverDisplay.AutoScroll = true;
            this.pnlWaiverDisplay.BackColor = System.Drawing.Color.White;
            this.pnlWaiverDisplay.Location = new System.Drawing.Point(60, 730);
            this.pnlWaiverDisplay.Name = "pnlWaiverDisplay";
            this.pnlWaiverDisplay.Size = new System.Drawing.Size(959, 820);
            this.pnlWaiverDisplay.TabIndex = 1029;
            // 
            // pbCheckBox
            // 
            this.pbCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.pbCheckBox.Image = global::Parafait_Kiosk.Properties.Resources.tick_box_unchecked;
            this.pbCheckBox.Location = new System.Drawing.Point(140, 3);
            this.pbCheckBox.Name = "pbCheckBox";
            this.pbCheckBox.Size = new System.Drawing.Size(110, 98);
            this.pbCheckBox.TabIndex = 1031;
            this.pbCheckBox.TabStop = false;
            this.pbCheckBox.Click += new System.EventHandler(this.pbCheckBox_Click);
            // 
            // chkSignConfirm
            // 
            this.chkSignConfirm.AutoSize = true;
            this.chkSignConfirm.BackColor = System.Drawing.Color.Transparent;
            this.chkSignConfirm.Font = new System.Drawing.Font("Gotham Rounded Bold", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSignConfirm.ForeColor = System.Drawing.Color.White;
            this.chkSignConfirm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSignConfirm.Location = new System.Drawing.Point(216, 36);
            this.chkSignConfirm.Name = "chkSignConfirm";
            this.chkSignConfirm.Size = new System.Drawing.Size(639, 45);
            this.chkSignConfirm.TabIndex = 1030;
            this.chkSignConfirm.Text = "I agree to the terms and conditions";
            this.chkSignConfirm.UseVisualStyleBackColor = false;
            this.chkSignConfirm.CheckedChanged += new System.EventHandler(this.chkSignConfirm_CheckedChanged);
            // 
            // pnlSignature
            // 
            this.pnlSignature.BackColor = System.Drawing.Color.Transparent;
            this.pnlSignature.Controls.Add(this.pbCheckBox);
            this.pnlSignature.Controls.Add(this.chkSignConfirm);
            this.pnlSignature.Location = new System.Drawing.Point(12, 1558);
            this.pnlSignature.Name = "pnlSignature";
            this.pnlSignature.Size = new System.Drawing.Size(1056, 110);
            this.pnlSignature.TabIndex = 1032;
            // 
            // frmSignWaiverFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.pnlWaiverDisplay);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.pnlMaster);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblSignWaiverFile);
            this.Controls.Add(this.lblSample);
            this.Controls.Add(this.pnlSignature);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmSignWaiverFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sign Waiver File";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSelectWaiverOption_Closing);
            this.Load += new System.EventHandler(this.frmSelectWaiverOption_Load);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.pnlSignature, 0);
            this.Controls.SetChildIndex(this.lblSample, 0);
            this.Controls.SetChildIndex(this.lblSignWaiverFile, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.pnlMaster, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnOkay, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.pnlWaiverDisplay, 0);
            this.pnlMaster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCheckBox)).EndInit();
            this.pnlSignature.ResumeLayout(false);
            this.pnlSignature.PerformLayout();
            this.ResumeLayout(false);

        }
        

        #endregion

        private System.Windows.Forms.Label lblSignWaiverFile;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Panel pnlMaster;
        private System.Windows.Forms.Label lblSample;
        private System.Windows.Forms.Label lblCustomerName;
        //private System.Windows.Forms.Button btnCancel;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollWaiver;
        private Label lblCustomerContact;
        private Button btnOkay;
        private Panel pnlWaiver;
        private Panel pnlWaiverDisplay;
        private WebBrowser webBrowser;
        private PictureBox pbCheckBox;
        private CheckBox chkSignConfirm;
        private Panel pnlSignature;
    }
}