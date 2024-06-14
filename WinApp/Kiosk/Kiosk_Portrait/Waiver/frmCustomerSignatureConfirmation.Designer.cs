using System.Windows.Forms;

namespace Parafait_Kiosk.Waiver
{
    partial class frmCustomerSignatureConfirmation
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkSignConfirm = new System.Windows.Forms.CheckBox();
            this.pbCheckBox = new System.Windows.Forms.PictureBox();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheckBox)).BeginInit();
            this.SuspendLayout();
            // 
            // chkSignConfirm
            // 
            this.chkSignConfirm.AutoSize = true;
            this.chkSignConfirm.BackColor = System.Drawing.Color.Transparent;
            this.chkSignConfirm.Font = new System.Drawing.Font("Gotham Rounded Bold", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSignConfirm.ForeColor = System.Drawing.Color.White;
            this.chkSignConfirm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSignConfirm.Location = new System.Drawing.Point(187, 222);
            this.chkSignConfirm.Name = "chkSignConfirm";
            this.chkSignConfirm.Size = new System.Drawing.Size(639, 45);
            this.chkSignConfirm.TabIndex = 1030;
            this.chkSignConfirm.Text = "I agree to the terms and conditions";
            this.chkSignConfirm.UseVisualStyleBackColor = false;
            this.chkSignConfirm.CheckedChanged += new System.EventHandler(this.chkSignConfirm_CheckedChanged);
            // 
            // pbCheckBox
            // 
            this.pbCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.pbCheckBox.Image = global::Parafait_Kiosk.Properties.Resources.tick_box_unchecked;
            this.pbCheckBox.Location = new System.Drawing.Point(97, 190);
            this.pbCheckBox.Name = "pbCheckBox";
            this.pbCheckBox.Size = new System.Drawing.Size(110, 98);
            this.pbCheckBox.TabIndex = 1031;
            this.pbCheckBox.TabStop = false;
            this.pbCheckBox.Click += new System.EventHandler(this.pbCheckBox_Click);
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomerName.Font = new System.Drawing.Font("Gotham Rounded Bold", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.ForeColor = System.Drawing.Color.White;
            this.lblCustomerName.Location = new System.Drawing.Point(89, 59);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(194, 41);
            this.lblCustomerName.TabIndex = 1032;
            this.lblCustomerName.Text = "Guardian: ";
            // 
            // btnOkay
            // 
            this.btnOkay.BackColor = System.Drawing.Color.Transparent;
            this.btnOkay.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnOkay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOkay.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnOkay.FlatAppearance.BorderSize = 0;
            this.btnOkay.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkay.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOkay.ForeColor = System.Drawing.Color.White;
            this.btnOkay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOkay.Location = new System.Drawing.Point(550, 395);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(365, 135);
            this.btnOkay.TabIndex = 1028;
            this.btnOkay.Text = "Ok";
            this.btnOkay.UseVisualStyleBackColor = false;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(88, 395);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(365, 135);
            this.btnCancel.TabIndex = 1033;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // frmCustomerSignatureConfirmation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(993, 637);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.pbCheckBox);
            this.Controls.Add(this.chkSignConfirm);
            this.Controls.Add(this.btnOkay);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCustomerSignatureConfirmation";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Confirm";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCustomerSignatureConfirmation_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pbCheckBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox chkSignConfirm;
        private PictureBox pbCheckBox;
        private Label lblCustomerName;
        private Button btnOkay;
        private Button btnCancel;
    }
}
