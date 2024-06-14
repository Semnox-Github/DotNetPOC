namespace ParafaitQueueManagement
{
    partial class frmSetupTeam
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTeamName = new System.Windows.Forms.Label();
            this.lblCaptain = new System.Windows.Forms.Label();
            this.txtTeamName = new System.Windows.Forms.TextBox();
            this.captainCardNumber = new System.Windows.Forms.TextBox();
            this.btnValidateTeamName = new System.Windows.Forms.Button();
            this.bntGetTeamDetails = new System.Windows.Forms.Button();
            this.dgCaptainDetails = new System.Windows.Forms.DataGridView();
            this.teamMemberCardNumber = new System.Windows.Forms.TextBox();
            this.lblTeamMember = new System.Windows.Forms.Label();
            this.btnGetTeamMemberDetails = new System.Windows.Forms.Button();
            this.dgTeamMemberDetails = new System.Windows.Forms.DataGridView();
            this.btnSetup = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.captainDeleteButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgCaptainDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgTeamMemberDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTeamName
            // 
            this.lblTeamName.AutoSize = true;
            this.lblTeamName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTeamName.Location = new System.Drawing.Point(19, 20);
            this.lblTeamName.Name = "lblTeamName";
            this.lblTeamName.Size = new System.Drawing.Size(70, 13);
            this.lblTeamName.TabIndex = 0;
            this.lblTeamName.Text = "Team Name: ";
            // 
            // lblCaptain
            // 
            this.lblCaptain.AutoSize = true;
            this.lblCaptain.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaptain.Location = new System.Drawing.Point(19, 56);
            this.lblCaptain.Name = "lblCaptain";
            this.lblCaptain.Size = new System.Drawing.Size(51, 13);
            this.lblCaptain.TabIndex = 1;
            this.lblCaptain.Text = "Captain: ";
            // 
            // txtTeamName
            // 
            this.txtTeamName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTeamName.Location = new System.Drawing.Point(121, 16);
            this.txtTeamName.Name = "txtTeamName";
            this.txtTeamName.Size = new System.Drawing.Size(143, 21);
            this.txtTeamName.TabIndex = 2;
            // 
            // captainCardNumber
            // 
            this.captainCardNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captainCardNumber.Location = new System.Drawing.Point(121, 52);
            this.captainCardNumber.Name = "captainCardNumber";
            this.captainCardNumber.Size = new System.Drawing.Size(143, 21);
            this.captainCardNumber.TabIndex = 3;
            // 
            // btnValidateTeamName
            // 
            this.btnValidateTeamName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidateTeamName.Location = new System.Drawing.Point(278, 15);
            this.btnValidateTeamName.Name = "btnValidateTeamName";
            this.btnValidateTeamName.Size = new System.Drawing.Size(75, 23);
            this.btnValidateTeamName.TabIndex = 4;
            this.btnValidateTeamName.Text = "Validate";
            this.btnValidateTeamName.UseVisualStyleBackColor = true;
            this.btnValidateTeamName.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // bntGetTeamDetails
            // 
            this.bntGetTeamDetails.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntGetTeamDetails.Location = new System.Drawing.Point(278, 51);
            this.bntGetTeamDetails.Name = "bntGetTeamDetails";
            this.bntGetTeamDetails.Size = new System.Drawing.Size(75, 23);
            this.bntGetTeamDetails.TabIndex = 5;
            this.bntGetTeamDetails.Text = "Get Details";
            this.bntGetTeamDetails.UseVisualStyleBackColor = true;
            this.bntGetTeamDetails.Click += new System.EventHandler(this.bntGetCaptainDetails_Click);
            // 
            // dgCaptainDetails
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCaptainDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgCaptainDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgCaptainDetails.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgCaptainDetails.Location = new System.Drawing.Point(22, 83);
            this.dgCaptainDetails.Name = "dgCaptainDetails";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCaptainDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgCaptainDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgCaptainDetails.Size = new System.Drawing.Size(555, 62);
            this.dgCaptainDetails.TabIndex = 6;
            // 
            // teamMemberCardNumber
            // 
            this.teamMemberCardNumber.Location = new System.Drawing.Point(121, 184);
            this.teamMemberCardNumber.Name = "teamMemberCardNumber";
            this.teamMemberCardNumber.Size = new System.Drawing.Size(143, 20);
            this.teamMemberCardNumber.TabIndex = 7;
            // 
            // lblTeamMember
            // 
            this.lblTeamMember.AutoSize = true;
            this.lblTeamMember.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTeamMember.Location = new System.Drawing.Point(19, 188);
            this.lblTeamMember.Name = "lblTeamMember";
            this.lblTeamMember.Size = new System.Drawing.Size(81, 13);
            this.lblTeamMember.TabIndex = 8;
            this.lblTeamMember.Text = "Team Member: ";
            this.lblTeamMember.Click += new System.EventHandler(this.lblTeamMember_Click);
            // 
            // btnGetTeamMemberDetails
            // 
            this.btnGetTeamMemberDetails.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetTeamMemberDetails.Location = new System.Drawing.Point(278, 183);
            this.btnGetTeamMemberDetails.Name = "btnGetTeamMemberDetails";
            this.btnGetTeamMemberDetails.Size = new System.Drawing.Size(75, 23);
            this.btnGetTeamMemberDetails.TabIndex = 9;
            this.btnGetTeamMemberDetails.Text = "Get Details";
            this.btnGetTeamMemberDetails.UseVisualStyleBackColor = true;
            this.btnGetTeamMemberDetails.Click += new System.EventHandler(this.btnGetTeamMemberDetails_Click);
            // 
            // dgTeamMemberDetails
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTeamMemberDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgTeamMemberDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgTeamMemberDetails.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgTeamMemberDetails.Location = new System.Drawing.Point(22, 214);
            this.dgTeamMemberDetails.Name = "dgTeamMemberDetails";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTeamMemberDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgTeamMemberDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgTeamMemberDetails.Size = new System.Drawing.Size(555, 99);
            this.dgTeamMemberDetails.TabIndex = 10;
            this.dgTeamMemberDetails.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgTeamMemberDetails_CellContentClick);
            // 
            // btnSetup
            // 
            this.btnSetup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetup.Location = new System.Drawing.Point(22, 354);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(75, 23);
            this.btnSetup.TabIndex = 11;
            this.btnSetup.Text = "Setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSaveTeamName_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(128, 354);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClearForm_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(232, 354);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // captainDeleteButton
            // 
            this.captainDeleteButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captainDeleteButton.Location = new System.Drawing.Point(22, 152);
            this.captainDeleteButton.Name = "captainDeleteButton";
            this.captainDeleteButton.Size = new System.Drawing.Size(94, 23);
            this.captainDeleteButton.TabIndex = 14;
            this.captainDeleteButton.Text = "Delete Captain";
            this.captainDeleteButton.UseVisualStyleBackColor = true;
            this.captainDeleteButton.Click += new System.EventHandler(this.btnDeleteCaptain_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(22, 319);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Delete All";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnDeleteMember_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(128, 319);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Delete Member";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnDeleteSingleMember_Click);
            // 
            // frmSetupTeam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 389);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.captainDeleteButton);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.dgTeamMemberDetails);
            this.Controls.Add(this.btnGetTeamMemberDetails);
            this.Controls.Add(this.lblTeamMember);
            this.Controls.Add(this.teamMemberCardNumber);
            this.Controls.Add(this.dgCaptainDetails);
            this.Controls.Add(this.bntGetTeamDetails);
            this.Controls.Add(this.btnValidateTeamName);
            this.Controls.Add(this.captainCardNumber);
            this.Controls.Add(this.txtTeamName);
            this.Controls.Add(this.lblCaptain);
            this.Controls.Add(this.lblTeamName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetupTeam";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup Team";
            this.Load += new System.EventHandler(this.frmSetupTeam_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgCaptainDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgTeamMemberDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTeamName;
        private System.Windows.Forms.Label lblCaptain;
        private System.Windows.Forms.TextBox txtTeamName;
        private System.Windows.Forms.TextBox captainCardNumber;
        private System.Windows.Forms.Button btnValidateTeamName;
        private System.Windows.Forms.Button bntGetTeamDetails;
        private System.Windows.Forms.DataGridView dgCaptainDetails;
        private System.Windows.Forms.TextBox teamMemberCardNumber;
        private System.Windows.Forms.Label lblTeamMember;
        private System.Windows.Forms.Button btnGetTeamMemberDetails;
        private System.Windows.Forms.DataGridView dgTeamMemberDetails;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button captainDeleteButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;

    }
}