namespace Parafait_POS.Subscription
{
    partial class frmSubscriptionInput
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
            this.components = new System.ComponentModel.Container();
            this.txtSubscriptionName = new System.Windows.Forms.TextBox();
            this.lblSubscriptionName = new System.Windows.Forms.Label();
            this.txtSubscriptionPrice = new System.Windows.Forms.TextBox();
            this.lblSubscriptionPrice = new System.Windows.Forms.Label();
            this.txtSubscriptionCycle = new System.Windows.Forms.TextBox();
            this.lblSubscriptionCycle = new System.Windows.Forms.Label();
            this.txtUnitOfSubscriptionCycle = new System.Windows.Forms.TextBox();
            this.lblUnitOfSubscriptionCycle = new System.Windows.Forms.Label();
            this.txtSubscriptionCycleValidity = new System.Windows.Forms.TextBox();
            this.lblSubscriptionCycleValidity = new System.Windows.Forms.Label();
            this.lblPaymentCollectionMode = new System.Windows.Forms.Label();
            this.lblBillInAdvance = new System.Windows.Forms.Label();
            this.lblAutoRenew = new System.Windows.Forms.Label();
            this.cbxBillInAdvance = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cbxAutoRenew = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cmbPaymentCollectionMode = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSubscriptionName
            // 
            this.txtSubscriptionName.Font = new System.Drawing.Font("Arial", 15F);
            this.txtSubscriptionName.Location = new System.Drawing.Point(148, 12);
            this.txtSubscriptionName.MaxLength = 10;
            this.txtSubscriptionName.Name = "txtSubscriptionName";
            this.txtSubscriptionName.ReadOnly = true;
            this.txtSubscriptionName.Size = new System.Drawing.Size(226, 30);
            this.txtSubscriptionName.TabIndex = 5;
            // 
            // lblSubscriptionName
            // 
            this.lblSubscriptionName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblSubscriptionName.Location = new System.Drawing.Point(12, 15);
            this.lblSubscriptionName.Name = "lblSubscriptionName";
            this.lblSubscriptionName.Size = new System.Drawing.Size(131, 24);
            this.lblSubscriptionName.TabIndex = 4;
            this.lblSubscriptionName.Text = "Subscription Name: ";
            this.lblSubscriptionName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSubscriptionPrice
            // 
            this.txtSubscriptionPrice.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubscriptionPrice.Location = new System.Drawing.Point(148, 49);
            this.txtSubscriptionPrice.MaxLength = 5;
            this.txtSubscriptionPrice.Name = "txtSubscriptionPrice";
            this.txtSubscriptionPrice.ReadOnly = true;
            this.txtSubscriptionPrice.Size = new System.Drawing.Size(130, 30);
            this.txtSubscriptionPrice.TabIndex = 7;
            // 
            // lblSubscriptionPrice
            // 
            this.lblSubscriptionPrice.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubscriptionPrice.Location = new System.Drawing.Point(12, 52);
            this.lblSubscriptionPrice.Name = "lblSubscriptionPrice";
            this.lblSubscriptionPrice.Size = new System.Drawing.Size(131, 24);
            this.lblSubscriptionPrice.TabIndex = 6;
            this.lblSubscriptionPrice.Text = "Subscription Price:";
            this.lblSubscriptionPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSubscriptionCycle
            // 
            this.txtSubscriptionCycle.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubscriptionCycle.Location = new System.Drawing.Point(609, 12);
            this.txtSubscriptionCycle.MaxLength = 5;
            this.txtSubscriptionCycle.Name = "txtSubscriptionCycle";
            this.txtSubscriptionCycle.ReadOnly = true;
            this.txtSubscriptionCycle.Size = new System.Drawing.Size(130, 30);
            this.txtSubscriptionCycle.TabIndex = 9;
            // 
            // lblSubscriptionCycle
            // 
            this.lblSubscriptionCycle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubscriptionCycle.Location = new System.Drawing.Point(472, 15);
            this.lblSubscriptionCycle.Name = "lblSubscriptionCycle";
            this.lblSubscriptionCycle.Size = new System.Drawing.Size(131, 24);
            this.lblSubscriptionCycle.TabIndex = 8;
            this.lblSubscriptionCycle.Text = "Subscription Cycle:";
            this.lblSubscriptionCycle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUnitOfSubscriptionCycle
            // 
            this.txtUnitOfSubscriptionCycle.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnitOfSubscriptionCycle.Location = new System.Drawing.Point(609, 49);
            this.txtUnitOfSubscriptionCycle.MaxLength = 5;
            this.txtUnitOfSubscriptionCycle.Name = "txtUnitOfSubscriptionCycle";
            this.txtUnitOfSubscriptionCycle.ReadOnly = true;
            this.txtUnitOfSubscriptionCycle.Size = new System.Drawing.Size(130, 30);
            this.txtUnitOfSubscriptionCycle.TabIndex = 11;
            // 
            // lblUnitOfSubscriptionCycle
            // 
            this.lblUnitOfSubscriptionCycle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnitOfSubscriptionCycle.Location = new System.Drawing.Point(402, 52);
            this.lblUnitOfSubscriptionCycle.Name = "lblUnitOfSubscriptionCycle";
            this.lblUnitOfSubscriptionCycle.Size = new System.Drawing.Size(201, 24);
            this.lblUnitOfSubscriptionCycle.TabIndex = 10;
            this.lblUnitOfSubscriptionCycle.Text = "Unit Of Subscription Cycle:";
            this.lblUnitOfSubscriptionCycle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSubscriptionCycleValidity
            // 
            this.txtSubscriptionCycleValidity.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubscriptionCycleValidity.Location = new System.Drawing.Point(609, 88);
            this.txtSubscriptionCycleValidity.MaxLength = 5;
            this.txtSubscriptionCycleValidity.Name = "txtSubscriptionCycleValidity";
            this.txtSubscriptionCycleValidity.ReadOnly = true;
            this.txtSubscriptionCycleValidity.Size = new System.Drawing.Size(130, 30);
            this.txtSubscriptionCycleValidity.TabIndex = 13;
            // 
            // lblSubscriptionCycleValidity
            // 
            this.lblSubscriptionCycleValidity.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubscriptionCycleValidity.Location = new System.Drawing.Point(402, 91);
            this.lblSubscriptionCycleValidity.Name = "lblSubscriptionCycleValidity";
            this.lblSubscriptionCycleValidity.Size = new System.Drawing.Size(201, 24);
            this.lblSubscriptionCycleValidity.TabIndex = 12;
            this.lblSubscriptionCycleValidity.Text = "Subscription Cycle Validity:";
            this.lblSubscriptionCycleValidity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPaymentCollectionMode
            // 
            this.lblPaymentCollectionMode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentCollectionMode.Location = new System.Drawing.Point(402, 128);
            this.lblPaymentCollectionMode.Name = "lblPaymentCollectionMode";
            this.lblPaymentCollectionMode.Size = new System.Drawing.Size(201, 24);
            this.lblPaymentCollectionMode.TabIndex = 14;
            this.lblPaymentCollectionMode.Text = "Payment Collection Mode:";
            this.lblPaymentCollectionMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBillInAdvance
            // 
            this.lblBillInAdvance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillInAdvance.Location = new System.Drawing.Point(12, 91);
            this.lblBillInAdvance.Name = "lblBillInAdvance";
            this.lblBillInAdvance.Size = new System.Drawing.Size(131, 24);
            this.lblBillInAdvance.TabIndex = 16;
            this.lblBillInAdvance.Text = "Bill In Advance?:";
            this.lblBillInAdvance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAutoRenew
            // 
            this.lblAutoRenew.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoRenew.Location = new System.Drawing.Point(12, 128);
            this.lblAutoRenew.Name = "lblAutoRenew";
            this.lblAutoRenew.Size = new System.Drawing.Size(131, 24);
            this.lblAutoRenew.TabIndex = 18;
            this.lblAutoRenew.Text = "Auto Renew?:";
            this.lblAutoRenew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxBillInAdvance
            // 
            this.cbxBillInAdvance.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxBillInAdvance.AutoSize = true;
            this.cbxBillInAdvance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxBillInAdvance.Enabled = false;
            this.cbxBillInAdvance.FlatAppearance.BorderSize = 0;
            this.cbxBillInAdvance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxBillInAdvance.Font = new System.Drawing.Font("Arial", 15F);
            this.cbxBillInAdvance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxBillInAdvance.ImageIndex = 1;
            this.cbxBillInAdvance.Location = new System.Drawing.Point(145, 90);
            this.cbxBillInAdvance.Name = "cbxBillInAdvance";
            this.cbxBillInAdvance.Size = new System.Drawing.Size(26, 26);
            this.cbxBillInAdvance.TabIndex = 19;
            this.cbxBillInAdvance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxBillInAdvance.UseVisualStyleBackColor = true;
            // 
            // cbxAutoRenew
            // 
            this.cbxAutoRenew.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxAutoRenew.AutoSize = true;
            this.cbxAutoRenew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxAutoRenew.FlatAppearance.BorderSize = 0;
            this.cbxAutoRenew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxAutoRenew.Font = new System.Drawing.Font("Arial", 15F);
            this.cbxAutoRenew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxAutoRenew.ImageIndex = 1;
            this.cbxAutoRenew.Location = new System.Drawing.Point(145, 127);
            this.cbxAutoRenew.Name = "cbxAutoRenew";
            this.cbxAutoRenew.Size = new System.Drawing.Size(26, 26);
            this.cbxAutoRenew.TabIndex = 20;
            this.cbxAutoRenew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAutoRenew.UseVisualStyleBackColor = true;
            // 
            // cmbPaymentCollectionMode
            // 
            this.cmbPaymentCollectionMode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbPaymentCollectionMode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbPaymentCollectionMode.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbPaymentCollectionMode.FormattingEnabled = true;
            this.cmbPaymentCollectionMode.Location = new System.Drawing.Point(609, 125);
            this.cmbPaymentCollectionMode.Name = "cmbPaymentCollectionMode";
            this.cmbPaymentCollectionMode.Size = new System.Drawing.Size(226, 31);
            this.cmbPaymentCollectionMode.TabIndex = 21;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(439, 213);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 34);
            this.btnCancel.TabIndex = 124;
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
            this.btnOkay.Location = new System.Drawing.Point(296, 212);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(104, 36);
            this.btnOkay.TabIndex = 123;
            this.btnOkay.Text = "Ok";
            this.btnOkay.UseVisualStyleBackColor = false;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            this.btnOkay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnOkay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // frmSubscriptionInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 265);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.cmbPaymentCollectionMode);
            this.Controls.Add(this.cbxAutoRenew);
            this.Controls.Add(this.cbxBillInAdvance);
            this.Controls.Add(this.lblAutoRenew);
            this.Controls.Add(this.lblBillInAdvance);
            this.Controls.Add(this.lblPaymentCollectionMode);
            this.Controls.Add(this.txtSubscriptionCycleValidity);
            this.Controls.Add(this.lblSubscriptionCycleValidity);
            this.Controls.Add(this.txtUnitOfSubscriptionCycle);
            this.Controls.Add(this.lblUnitOfSubscriptionCycle);
            this.Controls.Add(this.txtSubscriptionCycle);
            this.Controls.Add(this.lblSubscriptionCycle);
            this.Controls.Add(this.txtSubscriptionPrice);
            this.Controls.Add(this.lblSubscriptionPrice);
            this.Controls.Add(this.txtSubscriptionName);
            this.Controls.Add(this.lblSubscriptionName);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSubscriptionInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Subscription Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSubscriptionName;
        private System.Windows.Forms.Label lblSubscriptionName;
        private System.Windows.Forms.TextBox txtSubscriptionPrice;
        private System.Windows.Forms.Label lblSubscriptionPrice;
        private System.Windows.Forms.TextBox txtSubscriptionCycle;
        private System.Windows.Forms.Label lblSubscriptionCycle;
        private System.Windows.Forms.TextBox txtUnitOfSubscriptionCycle;
        private System.Windows.Forms.Label lblUnitOfSubscriptionCycle;
        private System.Windows.Forms.TextBox txtSubscriptionCycleValidity;
        private System.Windows.Forms.Label lblSubscriptionCycleValidity;
        private System.Windows.Forms.Label lblPaymentCollectionMode;
        private System.Windows.Forms.Label lblBillInAdvance;
        private System.Windows.Forms.Label lblAutoRenew;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxBillInAdvance;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxAutoRenew;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbPaymentCollectionMode;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOkay;
    }
}