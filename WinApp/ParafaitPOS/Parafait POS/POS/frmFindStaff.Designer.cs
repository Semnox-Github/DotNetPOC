namespace Parafait_POS
{
    partial class frmFindStaff
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
            this.lblRole = new System.Windows.Forms.Label();
            this.cmbStaffRoles = new System.Windows.Forms.ComboBox();
            this.lblStaffName = new System.Windows.Forms.Label();
            this.cmbStaffName = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRole
            // 
            this.lblRole.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblRole.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRole.Location = new System.Drawing.Point(19, 37);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(90, 22);
            this.lblRole.TabIndex = 0;
            this.lblRole.Text = "Role:";
            this.lblRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbStaffRoles
            // 
            this.cmbStaffRoles.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbStaffRoles.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbStaffRoles.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbStaffRoles.FormattingEnabled = true;
            this.cmbStaffRoles.Location = new System.Drawing.Point(111, 36);
            this.cmbStaffRoles.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbStaffRoles.Name = "cmbStaffRoles";
            this.cmbStaffRoles.Size = new System.Drawing.Size(298, 24);
            this.cmbStaffRoles.TabIndex = 1;
            this.cmbStaffRoles.SelectedIndexChanged += new System.EventHandler(this.cmbStaffRoles_SelectedIndexChanged);
            // 
            // lblStaffName
            // 
            this.lblStaffName.Location = new System.Drawing.Point(19, 103);
            this.lblStaffName.Name = "lblStaffName";
            this.lblStaffName.Size = new System.Drawing.Size(86, 21);
            this.lblStaffName.TabIndex = 2;
            this.lblStaffName.Text = "Staff Name:";
            this.lblStaffName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbStaffName
            // 
            this.cmbStaffName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbStaffName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbStaffName.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbStaffName.FormattingEnabled = true;
            this.cmbStaffName.Location = new System.Drawing.Point(111, 102);
            this.cmbStaffName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbStaffName.Name = "cmbStaffName";
            this.cmbStaffName.Size = new System.Drawing.Size(298, 24);
            this.cmbStaffName.TabIndex = 3;
            this.cmbStaffName.SelectedIndexChanged += new System.EventHandler(this.cmbStaffName_SelectedIndexChanged);
            this.cmbStaffName.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.cmbStaffName_Format);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtMessage);
            this.panel1.Controls.Add(this.cmbStaffRoles);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.lblRole);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.lblStaffName);
            this.panel1.Controls.Add(this.cmbStaffName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(441, 280);
            this.panel1.TabIndex = 15;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.White;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 258);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(441, 22);
            this.txtMessage.TabIndex = 80;
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(262, 174);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 39);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(139, 174);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 39);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "Select";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // frmFindStaff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(441, 280);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmFindStaff";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search Staff Details";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cmbStaffRoles;
        private System.Windows.Forms.Label lblStaffName;
        private System.Windows.Forms.ComboBox cmbStaffName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtMessage;
    }
}