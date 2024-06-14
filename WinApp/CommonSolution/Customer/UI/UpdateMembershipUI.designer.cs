namespace Semnox.Parafait.Customer
{
    partial class UpdateMembershipUI
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
            this.cmbCurrentMembership = new System.Windows.Forms.ComboBox();
            this.cmbNewMembeship = new System.Windows.Forms.ComboBox();
            this.lblCurrentMembership = new System.Windows.Forms.Label();
            this.lblNewMembership = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbCurrentMembership
            // 
            this.cmbCurrentMembership.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCurrentMembership.Enabled = false;
            this.cmbCurrentMembership.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCurrentMembership.FormattingEnabled = true;
            this.cmbCurrentMembership.Location = new System.Drawing.Point(153, 48);
            this.cmbCurrentMembership.Name = "cmbCurrentMembership";
            this.cmbCurrentMembership.Size = new System.Drawing.Size(140, 23);
            this.cmbCurrentMembership.TabIndex = 0;
            // 
            // cmbNewMembeship
            // 
            this.cmbNewMembeship.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNewMembeship.FormattingEnabled = true;
            this.cmbNewMembeship.Location = new System.Drawing.Point(153, 83);
            this.cmbNewMembeship.Name = "cmbNewMembeship";
            this.cmbNewMembeship.Size = new System.Drawing.Size(140, 23);
            this.cmbNewMembeship.TabIndex = 1;
            // 
            // lblCurrentMembership
            // 
            this.lblCurrentMembership.AutoSize = true;
            this.lblCurrentMembership.Location = new System.Drawing.Point(13, 52);
            this.lblCurrentMembership.Name = "lblCurrentMembership";
            this.lblCurrentMembership.Size = new System.Drawing.Size(127, 15);
            this.lblCurrentMembership.TabIndex = 2;
            this.lblCurrentMembership.Text = "Current Membership:";
            // 
            // lblNewMembership
            // 
            this.lblNewMembership.AutoSize = true;
            this.lblNewMembership.Location = new System.Drawing.Point(15, 87);
            this.lblNewMembership.Name = "lblNewMembership";
            this.lblNewMembership.Size = new System.Drawing.Size(123, 15);
            this.lblNewMembership.TabIndex = 3;
            this.lblNewMembership.Text = "Assign Membership:";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(66, 146);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(153, 146);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(39, 22);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(102, 15);
            this.lblName.TabIndex = 6;
            this.lblName.Text = "Customer Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Location = new System.Drawing.Point(153, 22);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(41, 15);
            this.lblCustomerName.TabIndex = 7;
            this.lblCustomerName.Text = "label1";
            // 
            // UpdateMembershipUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(331, 181);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblNewMembership);
            this.Controls.Add(this.lblCurrentMembership);
            this.Controls.Add(this.cmbNewMembeship);
            this.Controls.Add(this.cmbCurrentMembership);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateMembershipUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update Membership";
            this.Load += new System.EventHandler(this.UpdateMembershipUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCurrentMembership;
        private System.Windows.Forms.ComboBox cmbNewMembeship;
        private System.Windows.Forms.Label lblCurrentMembership;
        private System.Windows.Forms.Label lblNewMembership;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCustomerName;
    }
}