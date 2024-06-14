namespace Semnox.Parafait.User
{
    partial class ChangeStaffUI
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.lblUsers = new System.Windows.Forms.Label();
            this.ddlUserRoles = new System.Windows.Forms.ComboBox();
            this.ddlUsers = new System.Windows.Forms.ComboBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblEmployeeNo = new System.Windows.Forms.Label();
            this.cbClockedInUsers = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Semnox.Parafait.User.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkTurquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(216, 206);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 49);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundImage = global::Semnox.Parafait.User.Properties.Resources.normal2;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.DarkTurquoise;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(96, 206);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 49);
            this.btnOK.TabIndex = 22;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // lblUserRole
            // 
            this.lblUserRole.AutoSize = true;
            this.lblUserRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserRole.Location = new System.Drawing.Point(52, 45);
            this.lblUserRole.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(83, 18);
            this.lblUserRole.TabIndex = 25;
            this.lblUserRole.Text = "User Roles";
            // 
            // lblUsers
            // 
            this.lblUsers.AutoSize = true;
            this.lblUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsers.Location = new System.Drawing.Point(52, 98);
            this.lblUsers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(48, 18);
            this.lblUsers.TabIndex = 26;
            this.lblUsers.Text = "Users";
            // 
            // ddlUserRoles
            // 
            this.ddlUserRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlUserRoles.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlUserRoles.FormattingEnabled = true;
            this.ddlUserRoles.Location = new System.Drawing.Point(143, 37);
            this.ddlUserRoles.Name = "ddlUserRoles";
            this.ddlUserRoles.Size = new System.Drawing.Size(203, 33);
            this.ddlUserRoles.TabIndex = 27;
            this.ddlUserRoles.SelectedIndexChanged += new System.EventHandler(this.DdlUserRoles_SelectedIndexChanged);
            // 
            // ddlUsers
            // 
            this.ddlUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlUsers.FormattingEnabled = true;
            this.ddlUsers.Location = new System.Drawing.Point(143, 90);
            this.ddlUsers.Margin = new System.Windows.Forms.Padding(4);
            this.ddlUsers.Name = "ddlUsers";
            this.ddlUsers.Size = new System.Drawing.Size(203, 33);
            this.ddlUsers.TabIndex = 28;
            this.ddlUsers.SelectedIndexChanged += new System.EventHandler(this.DdlUsers_SelectedIndexChanged);
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Location = new System.Drawing.Point(52, 137);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(0, 18);
            this.lblUserName.TabIndex = 29;
            // 
            // lblEmployeeNo
            // 
            this.lblEmployeeNo.AutoSize = true;
            this.lblEmployeeNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeNo.Location = new System.Drawing.Point(52, 168);
            this.lblEmployeeNo.Name = "lblEmployeeNo";
            this.lblEmployeeNo.Size = new System.Drawing.Size(0, 18);
            this.lblEmployeeNo.TabIndex = 30;
            // 
            // cbClockedInUsers
            // 
            this.cbClockedInUsers.AutoSize = true;
            this.cbClockedInUsers.Location = new System.Drawing.Point(26, 12);
            this.cbClockedInUsers.Name = "cbClockedInUsers";
            this.cbClockedInUsers.Size = new System.Drawing.Size(152, 19);
            this.cbClockedInUsers.TabIndex = 31;
            this.cbClockedInUsers.Text = "Show Clocked In Users";
            this.cbClockedInUsers.UseVisualStyleBackColor = true;
            this.cbClockedInUsers.CheckedChanged += new System.EventHandler(this.cbClockedInUsers_CheckedChanged);
            // 
            // ChangeStaffUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MintCream;
            this.ClientSize = new System.Drawing.Size(401, 275);
            this.Controls.Add(this.cbClockedInUsers);
            this.Controls.Add(this.lblEmployeeNo);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.ddlUsers);
            this.Controls.Add(this.ddlUserRoles);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.lblUserRole);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "ChangeStaffUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Staff";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblUserRole;
        private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.ComboBox ddlUserRoles;
        private System.Windows.Forms.ComboBox ddlUsers;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblEmployeeNo;
        private System.Windows.Forms.CheckBox cbClockedInUsers;
    }
}