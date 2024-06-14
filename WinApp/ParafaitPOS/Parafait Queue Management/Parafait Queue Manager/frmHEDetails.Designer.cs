namespace ParafaitQueueManagement
{
    partial class frmHEDetails
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
            this.lblHEUserID = new System.Windows.Forms.Label();
            this.lblHEUsername = new System.Windows.Forms.Label();
            this.lblMobileNo = new System.Windows.Forms.Label();
            this.lblDOB = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.txtMobNo = new System.Windows.Forms.TextBox();
            this.txtDOB = new System.Windows.Forms.TextBox();
            this.TxtUserName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblHEUserID
            // 
            this.lblHEUserID.AutoSize = true;
            this.lblHEUserID.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHEUserID.Location = new System.Drawing.Point(48, 36);
            this.lblHEUserID.Name = "lblHEUserID";
            this.lblHEUserID.Size = new System.Drawing.Size(50, 13);
            this.lblHEUserID.TabIndex = 0;
            this.lblHEUserID.Text = "User ID: ";
            // 
            // lblHEUsername
            // 
            this.lblHEUsername.AutoSize = true;
            this.lblHEUsername.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHEUsername.Location = new System.Drawing.Point(48, 85);
            this.lblHEUsername.Name = "lblHEUsername";
            this.lblHEUsername.Size = new System.Drawing.Size(66, 13);
            this.lblHEUsername.TabIndex = 1;
            this.lblHEUsername.Text = "User Name: ";
            // 
            // lblMobileNo
            // 
            this.lblMobileNo.AutoSize = true;
            this.lblMobileNo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMobileNo.Location = new System.Drawing.Point(48, 134);
            this.lblMobileNo.Name = "lblMobileNo";
            this.lblMobileNo.Size = new System.Drawing.Size(60, 13);
            this.lblMobileNo.TabIndex = 2;
            this.lblMobileNo.Text = "Mobile No: ";
            // 
            // lblDOB
            // 
            this.lblDOB.AutoSize = true;
            this.lblDOB.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDOB.Location = new System.Drawing.Point(51, 183);
            this.lblDOB.Name = "lblDOB";
            this.lblDOB.Size = new System.Drawing.Size(35, 13);
            this.lblDOB.TabIndex = 3;
            this.lblDOB.Text = "DOB: ";
            // 
            // txtUserID
            // 
            this.txtUserID.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserID.Location = new System.Drawing.Point(164, 29);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.ReadOnly = true;
            this.txtUserID.Size = new System.Drawing.Size(161, 21);
            this.txtUserID.TabIndex = 4;
            // 
            // txtMobNo
            // 
            this.txtMobNo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMobNo.Location = new System.Drawing.Point(164, 126);
            this.txtMobNo.Name = "txtMobNo";
            this.txtMobNo.ReadOnly = true;
            this.txtMobNo.Size = new System.Drawing.Size(161, 21);
            this.txtMobNo.TabIndex = 5;
            // 
            // txtDOB
            // 
            this.txtDOB.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDOB.Location = new System.Drawing.Point(164, 175);
            this.txtDOB.Name = "txtDOB";
            this.txtDOB.ReadOnly = true;
            this.txtDOB.Size = new System.Drawing.Size(161, 21);
            this.txtDOB.TabIndex = 6;
            // 
            // TxtUserName
            // 
            this.TxtUserName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtUserName.Location = new System.Drawing.Point(164, 77);
            this.TxtUserName.Name = "TxtUserName";
            this.TxtUserName.ReadOnly = true;
            this.TxtUserName.Size = new System.Drawing.Size(161, 21);
            this.TxtUserName.TabIndex = 7;
            // 
            // frmHEDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 244);
            this.Controls.Add(this.TxtUserName);
            this.Controls.Add(this.txtDOB);
            this.Controls.Add(this.txtMobNo);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.lblDOB);
            this.Controls.Add(this.lblMobileNo);
            this.Controls.Add(this.lblHEUsername);
            this.Controls.Add(this.lblHEUserID);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmHEDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "H-E Details";
            this.Load += new System.EventHandler(this.frmHEDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHEUserID;
        private System.Windows.Forms.Label lblHEUsername;
        private System.Windows.Forms.Label lblMobileNo;
        private System.Windows.Forms.Label lblDOB;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.TextBox txtMobNo;
        private System.Windows.Forms.TextBox txtDOB;
        private System.Windows.Forms.TextBox TxtUserName;
    }
}