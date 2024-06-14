namespace Semnox.Parafait.Transaction
{
    partial class frmEditPaymentMode
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
            this.lblExpiry = new System.Windows.Forms.Label();
            this.txtCardExpiry = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblAuthorization = new System.Windows.Forms.Label();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.lblCCName = new System.Windows.Forms.Label();
            this.lblNameOnCC = new System.Windows.Forms.Label();
            this.lblPaymentMode = new System.Windows.Forms.Label();
            this.txtAuthorization = new System.Windows.Forms.TextBox();
            this.txtCCName = new System.Windows.Forms.TextBox();
            this.txtNameOnCC = new System.Windows.Forms.TextBox();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.lblReference = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.cmbPaymentMode = new System.Windows.Forms.ComboBox();
            this.btnShowNumPad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblExpiry
            // 
            this.lblExpiry.AutoSize = true;
            this.lblExpiry.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblExpiry.Location = new System.Drawing.Point(127, 110);
            this.lblExpiry.Name = "lblExpiry";
            this.lblExpiry.Size = new System.Drawing.Size(97, 20);
            this.lblExpiry.TabIndex = 31;
            this.lblExpiry.Text = "Card Expiry :";
            // 
            // txtCardExpiry
            // 
            this.txtCardExpiry.Enabled = false;
            this.txtCardExpiry.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtCardExpiry.Location = new System.Drawing.Point(226, 106);
            this.txtCardExpiry.Name = "txtCardExpiry";
            this.txtCardExpiry.Size = new System.Drawing.Size(164, 29);
            this.txtCardExpiry.TabIndex = 3;
            this.txtCardExpiry.Enter += new System.EventHandler(this.txtCardExpiry_Enter);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(287, 336);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(103, 34);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(114, 336);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(103, 34);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblAuthorization
            // 
            this.lblAuthorization.AutoSize = true;
            this.lblAuthorization.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblAuthorization.Location = new System.Drawing.Point(113, 235);
            this.lblAuthorization.Name = "lblAuthorization";
            this.lblAuthorization.Size = new System.Drawing.Size(111, 20);
            this.lblAuthorization.TabIndex = 27;
            this.lblAuthorization.Text = "Authorization :";
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.AutoSize = true;
            this.lblCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.Location = new System.Drawing.Point(113, 67);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(111, 20);
            this.lblCardNumber.TabIndex = 26;
            this.lblCardNumber.Text = "Card Number :";
            // 
            // lblCCName
            // 
            this.lblCCName.AutoSize = true;
            this.lblCCName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCCName.Location = new System.Drawing.Point(139, 192);
            this.lblCCName.Name = "lblCCName";
            this.lblCCName.Size = new System.Drawing.Size(85, 20);
            this.lblCCName.TabIndex = 25;
            this.lblCCName.Text = "CC Name :";
            // 
            // lblNameOnCC
            // 
            this.lblNameOnCC.AutoSize = true;
            this.lblNameOnCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblNameOnCC.Location = new System.Drawing.Point(114, 151);
            this.lblNameOnCC.Name = "lblNameOnCC";
            this.lblNameOnCC.Size = new System.Drawing.Size(110, 20);
            this.lblNameOnCC.TabIndex = 24;
            this.lblNameOnCC.Text = "Name On CC :";
            // 
            // lblPaymentMode
            // 
            this.lblPaymentMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentMode.Location = new System.Drawing.Point(51, 26);
            this.lblPaymentMode.Name = "lblPaymentMode";
            this.lblPaymentMode.Size = new System.Drawing.Size(180, 24);
            this.lblPaymentMode.TabIndex = 23;
            this.lblPaymentMode.Text = "Select Payment Mode :";
            // 
            // txtAuthorization
            // 
            this.txtAuthorization.Enabled = false;
            this.txtAuthorization.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtAuthorization.Location = new System.Drawing.Point(226, 232);
            this.txtAuthorization.Name = "txtAuthorization";
            this.txtAuthorization.Size = new System.Drawing.Size(164, 29);
            this.txtAuthorization.TabIndex = 6;
            this.txtAuthorization.Enter += new System.EventHandler(this.txtAuthorization_Enter);
            // 
            // txtCCName
            // 
            this.txtCCName.Enabled = false;
            this.txtCCName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtCCName.Location = new System.Drawing.Point(226, 190);
            this.txtCCName.Name = "txtCCName";
            this.txtCCName.Size = new System.Drawing.Size(164, 29);
            this.txtCCName.TabIndex = 5;
            this.txtCCName.Enter += new System.EventHandler(this.txtCCName_Enter);
            // 
            // txtNameOnCC
            // 
            this.txtNameOnCC.Enabled = false;
            this.txtNameOnCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtNameOnCC.Location = new System.Drawing.Point(226, 148);
            this.txtNameOnCC.Name = "txtNameOnCC";
            this.txtNameOnCC.Size = new System.Drawing.Size(164, 29);
            this.txtNameOnCC.TabIndex = 4;
            this.txtNameOnCC.Enter += new System.EventHandler(this.txtNameOnCC_Enter);
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Enabled = false;
            this.txtCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.Location = new System.Drawing.Point(226, 64);
            this.txtCardNumber.MaxLength = 4;
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(164, 29);
            this.txtCardNumber.TabIndex = 2;
            this.txtCardNumber.Enter += new System.EventHandler(this.txtCardNumber_Enter);
            // 
            // txtReference
            // 
            this.txtReference.Enabled = false;
            this.txtReference.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtReference.Location = new System.Drawing.Point(226, 274);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(164, 29);
            this.txtReference.TabIndex = 7;
            this.txtReference.Enter += new System.EventHandler(this.txtReference_Enter);
            // 
            // lblReference
            // 
            this.lblReference.AutoSize = true;
            this.lblReference.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblReference.Location = new System.Drawing.Point(132, 277);
            this.lblReference.Name = "lblReference";
            this.lblReference.Size = new System.Drawing.Size(92, 20);
            this.lblReference.TabIndex = 33;
            this.lblReference.Text = "Reference :";
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.BackColor = System.Drawing.Color.LightYellow;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 388);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(493, 29);
            this.txtMessage.TabIndex = 34;
            this.txtMessage.TabStop = false;
            // 
            // cmbPaymentMode
            // 
            this.cmbPaymentMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.cmbPaymentMode.FormattingEnabled = true;
            this.cmbPaymentMode.ItemHeight = 24;
            this.cmbPaymentMode.Location = new System.Drawing.Point(226, 22);
            this.cmbPaymentMode.Name = "cmbPaymentMode";
            this.cmbPaymentMode.Size = new System.Drawing.Size(164, 32);
            this.cmbPaymentMode.TabIndex = 35;
            this.cmbPaymentMode.SelectedIndexChanged += new System.EventHandler(this.cmbPaymentMode_SelectedIndexChanged);
            // 
            // btnShowNumPad
            // 
            this.btnShowNumPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.CausesValidation = false;
            this.btnShowNumPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowNumPad.FlatAppearance.BorderSize = 0;
            this.btnShowNumPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowNumPad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowNumPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowNumPad.Image = Semnox.Parafait.Transaction.Properties.Resources.keyboard;
            this.btnShowNumPad.Location = new System.Drawing.Point(433, 330);
            this.btnShowNumPad.Name = "btnShowNumPad";
            this.btnShowNumPad.Size = new System.Drawing.Size(36, 40);
            this.btnShowNumPad.TabIndex = 36;
            this.btnShowNumPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowNumPad.UseVisualStyleBackColor = false;
            this.btnShowNumPad.Click += new System.EventHandler(this.btnShowNumPad_Click);
            // 
            // frmEditPaymentMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 417);
            this.Controls.Add(this.btnShowNumPad);
            this.Controls.Add(this.cmbPaymentMode);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblReference);
            this.Controls.Add(this.txtReference);
            this.Controls.Add(this.lblExpiry);
            this.Controls.Add(this.txtCardExpiry);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblAuthorization);
            this.Controls.Add(this.lblCardNumber);
            this.Controls.Add(this.lblCCName);
            this.Controls.Add(this.lblNameOnCC);
            this.Controls.Add(this.lblPaymentMode);
            this.Controls.Add(this.txtAuthorization);
            this.Controls.Add(this.txtCCName);
            this.Controls.Add(this.txtNameOnCC);
            this.Controls.Add(this.txtCardNumber);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditPaymentMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Payment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEditPaymentMode_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblExpiry;
        private System.Windows.Forms.TextBox txtCardExpiry;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblAuthorization;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.Label lblCCName;
        private System.Windows.Forms.Label lblNameOnCC;
        private System.Windows.Forms.Label lblPaymentMode;
        private System.Windows.Forms.TextBox txtAuthorization;
        private System.Windows.Forms.TextBox txtCCName;
        private System.Windows.Forms.TextBox txtNameOnCC;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.Label lblReference;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.ComboBox cmbPaymentMode;
        private System.Windows.Forms.Button btnShowNumPad;
    }
}