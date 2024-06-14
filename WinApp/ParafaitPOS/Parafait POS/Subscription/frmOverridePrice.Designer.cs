namespace Parafait_POS.Subscription
{
    partial class frmOverridePrice
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
            this.txtBillToDate = new System.Windows.Forms.TextBox();
            this.lblBillToDate = new System.Windows.Forms.Label();
            this.txtBillFromDate = new System.Windows.Forms.TextBox();
            this.lblBillFromDate = new System.Windows.Forms.Label();
            this.txtBillAmount = new System.Windows.Forms.TextBox();
            this.lblBillAmount = new System.Windows.Forms.Label();
            this.txtOverrideAmount = new System.Windows.Forms.TextBox();
            this.lblOverrideAmount = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.txtOverrideReason = new System.Windows.Forms.TextBox();
            this.lblOverrideReason = new System.Windows.Forms.Label();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtBillToDate
            // 
            this.txtBillToDate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBillToDate.Location = new System.Drawing.Point(470, 13);
            this.txtBillToDate.MaxLength = 5;
            this.txtBillToDate.Name = "txtBillToDate";
            this.txtBillToDate.ReadOnly = true;
            this.txtBillToDate.Size = new System.Drawing.Size(160, 30);
            this.txtBillToDate.TabIndex = 31;
            // 
            // lblBillToDate
            // 
            this.lblBillToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillToDate.Location = new System.Drawing.Point(333, 16);
            this.lblBillToDate.Name = "lblBillToDate";
            this.lblBillToDate.Size = new System.Drawing.Size(131, 24);
            this.lblBillToDate.TabIndex = 30;
            this.lblBillToDate.Text = "Bill To Date:";
            this.lblBillToDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBillFromDate
            // 
            this.txtBillFromDate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBillFromDate.Location = new System.Drawing.Point(142, 13);
            this.txtBillFromDate.MaxLength = 5;
            this.txtBillFromDate.Name = "txtBillFromDate";
            this.txtBillFromDate.ReadOnly = true;
            this.txtBillFromDate.Size = new System.Drawing.Size(160, 30);
            this.txtBillFromDate.TabIndex = 29;
            // 
            // lblBillFromDate
            // 
            this.lblBillFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillFromDate.Location = new System.Drawing.Point(6, 16);
            this.lblBillFromDate.Name = "lblBillFromDate";
            this.lblBillFromDate.Size = new System.Drawing.Size(131, 24);
            this.lblBillFromDate.TabIndex = 28;
            this.lblBillFromDate.Text = "Bill From Date:";
            this.lblBillFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBillAmount
            // 
            this.txtBillAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBillAmount.Location = new System.Drawing.Point(142, 51);
            this.txtBillAmount.MaxLength = 5;
            this.txtBillAmount.Name = "txtBillAmount";
            this.txtBillAmount.ReadOnly = true;
            this.txtBillAmount.Size = new System.Drawing.Size(130, 30);
            this.txtBillAmount.TabIndex = 33;
            // 
            // lblBillAmount
            // 
            this.lblBillAmount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillAmount.Location = new System.Drawing.Point(6, 54);
            this.lblBillAmount.Name = "lblBillAmount";
            this.lblBillAmount.Size = new System.Drawing.Size(131, 24);
            this.lblBillAmount.TabIndex = 32;
            this.lblBillAmount.Text = "Bill Amount:";
            this.lblBillAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOverrideAmount
            // 
            this.txtOverrideAmount.Font = new System.Drawing.Font("Arial", 15F);
            this.txtOverrideAmount.Location = new System.Drawing.Point(470, 51);
            this.txtOverrideAmount.MaxLength = 10;
            this.txtOverrideAmount.Name = "txtOverrideAmount";
            this.txtOverrideAmount.Size = new System.Drawing.Size(130, 30);
            this.txtOverrideAmount.TabIndex = 43;
            this.txtOverrideAmount.Enter += new System.EventHandler(this.txtOverrideAmount_Enter);
            // 
            // lblOverrideAmount
            // 
            this.lblOverrideAmount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblOverrideAmount.Location = new System.Drawing.Point(337, 54);
            this.lblOverrideAmount.Name = "lblOverrideAmount";
            this.lblOverrideAmount.Size = new System.Drawing.Size(130, 24);
            this.lblOverrideAmount.TabIndex = 42;
            this.lblOverrideAmount.Text = "Override Amount: ";
            this.lblOverrideAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(383, 178);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 30);
            this.btnCancel.TabIndex = 49;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnCancel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // btnOkay
            // 
            this.btnOkay.BackColor = System.Drawing.Color.Transparent;
            this.btnOkay.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnOkay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOkay.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnOkay.FlatAppearance.BorderSize = 0;
            this.btnOkay.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnOkay.ForeColor = System.Drawing.Color.White;
            this.btnOkay.Location = new System.Drawing.Point(273, 178);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(70, 30);
            this.btnOkay.TabIndex = 48;
            this.btnOkay.Text = "Ok";
            this.btnOkay.UseVisualStyleBackColor = false;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            this.btnOkay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnOkay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // txtOverrideReason
            // 
            this.txtOverrideReason.Font = new System.Drawing.Font("Arial", 15F);
            this.txtOverrideReason.Location = new System.Drawing.Point(142, 93);
            this.txtOverrideReason.MaxLength = 1999;
            this.txtOverrideReason.Multiline = true;
            this.txtOverrideReason.Name = "txtOverrideReason";
            this.txtOverrideReason.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOverrideReason.Size = new System.Drawing.Size(488, 65);
            this.txtOverrideReason.TabIndex = 51;
            // 
            // lblOverrideReason
            // 
            this.lblOverrideReason.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblOverrideReason.Location = new System.Drawing.Point(9, 96);
            this.lblOverrideReason.Name = "lblOverrideReason";
            this.lblOverrideReason.Size = new System.Drawing.Size(130, 24);
            this.lblOverrideReason.TabIndex = 50;
            this.lblOverrideReason.Text = "Reason:";
            this.lblOverrideReason.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnShowKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Location = new System.Drawing.Point(654, 117);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(36, 41);
            this.btnShowKeyPad.TabIndex = 122;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            // 
            // frmOverridePrice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 230);
            this.Controls.Add(this.btnShowKeyPad);
            this.Controls.Add(this.txtOverrideReason);
            this.Controls.Add(this.lblOverrideReason);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.txtOverrideAmount);
            this.Controls.Add(this.lblOverrideAmount);
            this.Controls.Add(this.txtBillAmount);
            this.Controls.Add(this.lblBillAmount);
            this.Controls.Add(this.txtBillToDate);
            this.Controls.Add(this.lblBillToDate);
            this.Controls.Add(this.txtBillFromDate);
            this.Controls.Add(this.lblBillFromDate);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOverridePrice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Override Bill Amount";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBillToDate;
        private System.Windows.Forms.Label lblBillToDate;
        private System.Windows.Forms.TextBox txtBillFromDate;
        private System.Windows.Forms.Label lblBillFromDate;
        private System.Windows.Forms.TextBox txtBillAmount;
        private System.Windows.Forms.Label lblBillAmount;
        private System.Windows.Forms.TextBox txtOverrideAmount;
        private System.Windows.Forms.Label lblOverrideAmount;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.TextBox txtOverrideReason;
        private System.Windows.Forms.Label lblOverrideReason;
        private System.Windows.Forms.Button btnShowKeyPad;
    }
}