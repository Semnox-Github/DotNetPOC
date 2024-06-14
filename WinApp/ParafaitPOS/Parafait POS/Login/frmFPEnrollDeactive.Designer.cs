namespace Parafait_POS.Login
{
    partial class frmFPEnrollDeactive
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
            this.lblLoginId = new System.Windows.Forms.Label();
            this.lblFingerPosition = new System.Windows.Forms.Label();
            this.cmbUserRole = new System.Windows.Forms.ComboBox();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.gbThumbImpression = new System.Windows.Forms.GroupBox();
            this.pbThumbImpresion = new System.Windows.Forms.PictureBox();
            this.lnkDeactivate = new System.Windows.Forms.LinkLabel();
            this.gbEnrolFingerPrintInfo = new System.Windows.Forms.GroupBox();
            this.cmbFingerPosition = new System.Windows.Forms.ComboBox();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.prbStatus = new System.Windows.Forms.ProgressBar();
            this.btnScan = new System.Windows.Forms.Button();
            this.gbUserFingerDetails = new System.Windows.Forms.GroupBox();
            this.gblblLoginId = new System.Windows.Forms.Label();
            this.gblabelLoginId = new System.Windows.Forms.Label();
            this.gblblUserName = new System.Windows.Forms.Label();
            this.gblabelUserName = new System.Windows.Forms.Label();
            this.dgvUserFingerDetails = new System.Windows.Forms.DataGridView();
            this.btnDeactivation = new System.Windows.Forms.Button();
            this.btnRefesh = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FingerPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActiveFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbThumbImpression.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbThumbImpresion)).BeginInit();
            this.gbEnrolFingerPrintInfo.SuspendLayout();
            this.gbUserFingerDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserFingerDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLoginId
            // 
            this.lblLoginId.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginId.Location = new System.Drawing.Point(19, 74);
            this.lblLoginId.Name = "lblLoginId";
            this.lblLoginId.Size = new System.Drawing.Size(95, 17);
            this.lblLoginId.TabIndex = 1;
            this.lblLoginId.Text = "LoginId:";
            this.lblLoginId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFingerPosition
            // 
            this.lblFingerPosition.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFingerPosition.Location = new System.Drawing.Point(16, 110);
            this.lblFingerPosition.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFingerPosition.Name = "lblFingerPosition";
            this.lblFingerPosition.Size = new System.Drawing.Size(100, 17);
            this.lblFingerPosition.TabIndex = 57;
            this.lblFingerPosition.Text = "Finger Position: ";
            this.lblFingerPosition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbUserRole
            // 
            this.cmbUserRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserRole.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUserRole.FormattingEnabled = true;
            this.cmbUserRole.Location = new System.Drawing.Point(116, 38);
            this.cmbUserRole.Name = "cmbUserRole";
            this.cmbUserRole.Size = new System.Drawing.Size(278, 24);
            this.cmbUserRole.TabIndex = 1;
            this.cmbUserRole.SelectedIndexChanged += new System.EventHandler(this.cmbUserRole_SelectedIndexChanged);
            // 
            // lblUserRole
            // 
            this.lblUserRole.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserRole.Location = new System.Drawing.Point(19, 38);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(95, 20);
            this.lblUserRole.TabIndex = 73;
            this.lblUserRole.Text = "User Role:";
            this.lblUserRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.Navy;
            this.btnRegister.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(128, 168);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(97, 39);
            this.btnRegister.TabIndex = 13;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Navy;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(125, 441);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 39);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.White;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 485);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(641, 22);
            this.txtMessage.TabIndex = 79;
            // 
            // gbThumbImpression
            // 
            this.gbThumbImpression.Controls.Add(this.pbThumbImpresion);
            this.gbThumbImpression.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbThumbImpression.Location = new System.Drawing.Point(444, 32);
            this.gbThumbImpression.Name = "gbThumbImpression";
            this.gbThumbImpression.Size = new System.Drawing.Size(186, 175);
            this.gbThumbImpression.TabIndex = 5;
            this.gbThumbImpression.TabStop = false;
            this.gbThumbImpression.Text = "Finger Impression";
            // 
            // pbThumbImpresion
            // 
            this.pbThumbImpresion.BackColor = System.Drawing.Color.White;
            this.pbThumbImpresion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbThumbImpresion.Location = new System.Drawing.Point(6, 21);
            this.pbThumbImpresion.Name = "pbThumbImpresion";
            this.pbThumbImpresion.Size = new System.Drawing.Size(175, 149);
            this.pbThumbImpresion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbThumbImpresion.TabIndex = 89;
            this.pbThumbImpresion.TabStop = false;
            // 
            // lnkDeactivate
            // 
            this.lnkDeactivate.Location = new System.Drawing.Point(0, 0);
            this.lnkDeactivate.Name = "lnkDeactivate";
            this.lnkDeactivate.Size = new System.Drawing.Size(100, 23);
            this.lnkDeactivate.TabIndex = 89;
            // 
            // gbEnrolFingerPrintInfo
            // 
            this.gbEnrolFingerPrintInfo.Controls.Add(this.cmbFingerPosition);
            this.gbEnrolFingerPrintInfo.Controls.Add(this.cmbUserName);
            this.gbEnrolFingerPrintInfo.Controls.Add(this.lblLoginId);
            this.gbEnrolFingerPrintInfo.Controls.Add(this.lblFingerPosition);
            this.gbEnrolFingerPrintInfo.Controls.Add(this.cmbUserRole);
            this.gbEnrolFingerPrintInfo.Controls.Add(this.lblUserRole);
            this.gbEnrolFingerPrintInfo.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbEnrolFingerPrintInfo.Location = new System.Drawing.Point(12, 12);
            this.gbEnrolFingerPrintInfo.Name = "gbEnrolFingerPrintInfo";
            this.gbEnrolFingerPrintInfo.Size = new System.Drawing.Size(426, 142);
            this.gbEnrolFingerPrintInfo.TabIndex = 88;
            this.gbEnrolFingerPrintInfo.TabStop = false;
            this.gbEnrolFingerPrintInfo.Text = "Enrol Finger Print";
            // 
            // cmbFingerPosition
            // 
            this.cmbFingerPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFingerPosition.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFingerPosition.FormattingEnabled = true;
            this.cmbFingerPosition.Location = new System.Drawing.Point(116, 109);
            this.cmbFingerPosition.Name = "cmbFingerPosition";
            this.cmbFingerPosition.Size = new System.Drawing.Size(278, 24);
            this.cmbFingerPosition.TabIndex = 106;
            // 
            // cmbUserName
            // 
            this.cmbUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbUserName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbUserName.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Location = new System.Drawing.Point(116, 74);
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(278, 24);
            this.cmbUserName.TabIndex = 105;
            this.cmbUserName.SelectedIndexChanged += new System.EventHandler(this.cmbUserName_SelectedIndexChanged);
            // 
            // prbStatus
            // 
            this.prbStatus.Location = new System.Drawing.Point(444, 19);
            this.prbStatus.Name = "prbStatus";
            this.prbStatus.Size = new System.Drawing.Size(186, 10);
            this.prbStatus.TabIndex = 0;
            // 
            // btnScan
            // 
            this.btnScan.BackColor = System.Drawing.Color.Navy;
            this.btnScan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnScan.FlatAppearance.BorderSize = 0;
            this.btnScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScan.ForeColor = System.Drawing.Color.White;
            this.btnScan.Location = new System.Drawing.Point(12, 168);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(97, 39);
            this.btnScan.TabIndex = 110;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = false;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // gbUserFingerDetails
            // 
            this.gbUserFingerDetails.Controls.Add(this.gblblLoginId);
            this.gbUserFingerDetails.Controls.Add(this.gblabelLoginId);
            this.gbUserFingerDetails.Controls.Add(this.gblblUserName);
            this.gbUserFingerDetails.Controls.Add(this.gblabelUserName);
            this.gbUserFingerDetails.Controls.Add(this.dgvUserFingerDetails);
            this.gbUserFingerDetails.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbUserFingerDetails.Location = new System.Drawing.Point(12, 214);
            this.gbUserFingerDetails.Name = "gbUserFingerDetails";
            this.gbUserFingerDetails.Size = new System.Drawing.Size(618, 221);
            this.gbUserFingerDetails.TabIndex = 111;
            this.gbUserFingerDetails.TabStop = false;
            this.gbUserFingerDetails.Text = "User Finger Details";
            // 
            // gblblLoginId
            // 
            this.gblblLoginId.AutoSize = true;
            this.gblblLoginId.Location = new System.Drawing.Point(369, 19);
            this.gblblLoginId.Name = "gblblLoginId";
            this.gblblLoginId.Size = new System.Drawing.Size(0, 16);
            this.gblblLoginId.TabIndex = 4;
            // 
            // gblabelLoginId
            // 
            this.gblabelLoginId.AutoSize = true;
            this.gblabelLoginId.Location = new System.Drawing.Point(303, 19);
            this.gblabelLoginId.Name = "gblabelLoginId";
            this.gblabelLoginId.Size = new System.Drawing.Size(57, 16);
            this.gblabelLoginId.TabIndex = 3;
            this.gblabelLoginId.Text = "LoginId :";
            // 
            // gblblUserName
            // 
            this.gblblUserName.AutoSize = true;
            this.gblblUserName.Location = new System.Drawing.Point(211, 19);
            this.gblblUserName.Name = "gblblUserName";
            this.gblblUserName.Size = new System.Drawing.Size(0, 16);
            this.gblblUserName.TabIndex = 2;
            // 
            // gblabelUserName
            // 
            this.gblabelUserName.AutoSize = true;
            this.gblabelUserName.Location = new System.Drawing.Point(118, 19);
            this.gblabelUserName.Name = "gblabelUserName";
            this.gblabelUserName.Size = new System.Drawing.Size(71, 16);
            this.gblabelUserName.TabIndex = 1;
            this.gblabelUserName.Text = "UserName : ";
            // 
            // dgvUserFingerDetails
            // 
            this.dgvUserFingerDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUserFingerDetails.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvUserFingerDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserFingerDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FingerPosition,
            this.ActiveFlag});
            this.dgvUserFingerDetails.Location = new System.Drawing.Point(7, 45);
            this.dgvUserFingerDetails.Name = "dgvUserFingerDetails";
            this.dgvUserFingerDetails.RowTemplate.ReadOnly = true;
            this.dgvUserFingerDetails.Size = new System.Drawing.Size(605, 170);
            this.dgvUserFingerDetails.TabIndex = 0;
            // 
            // btnDeactivation
            // 
            this.btnDeactivation.BackColor = System.Drawing.Color.Navy;
            this.btnDeactivation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDeactivation.FlatAppearance.BorderSize = 0;
            this.btnDeactivation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeactivation.ForeColor = System.Drawing.Color.White;
            this.btnDeactivation.Location = new System.Drawing.Point(12, 441);
            this.btnDeactivation.Name = "btnDeactivation";
            this.btnDeactivation.Size = new System.Drawing.Size(97, 39);
            this.btnDeactivation.TabIndex = 112;
            this.btnDeactivation.Text = "Deactivation";
            this.btnDeactivation.UseVisualStyleBackColor = false;
            this.btnDeactivation.Click += new System.EventHandler(this.btnDeactivation_Click);
            // 
            // btnRefesh
            // 
            this.btnRefesh.BackColor = System.Drawing.Color.Navy;
            this.btnRefesh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefesh.FlatAppearance.BorderSize = 0;
            this.btnRefesh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefesh.ForeColor = System.Drawing.Color.White;
            this.btnRefesh.Location = new System.Drawing.Point(341, 168);
            this.btnRefesh.Name = "btnRefesh";
            this.btnRefesh.Size = new System.Drawing.Size(97, 39);
            this.btnRefesh.TabIndex = 113;
            this.btnRefesh.Text = "Refresh";
            this.btnRefesh.UseVisualStyleBackColor = false;
            this.btnRefesh.Click += new System.EventHandler(this.btnRefesh_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "FingerName";
            this.dataGridViewTextBoxColumn1.HeaderText = "Finger Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 281;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ActiveFlag";
            this.dataGridViewTextBoxColumn2.HeaderText = "Active Flag";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 281;
            // 
            // FingerPosition
            // 
            this.FingerPosition.DataPropertyName = "Key";
            this.FingerPosition.HeaderText = "Finger Name";
            this.FingerPosition.Name = "FingerPosition";
            // 
            // ActiveFlag
            // 
            this.ActiveFlag.DataPropertyName = "Value";
            this.ActiveFlag.HeaderText = "Active Flag";
            this.ActiveFlag.Name = "ActiveFlag";
            // 
            // frmFPEnrollDeactive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Ivory;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(641, 507);
            this.Controls.Add(this.btnRefesh);
            this.Controls.Add(this.btnDeactivation);
            this.Controls.Add(this.gbUserFingerDetails);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.prbStatus);
            this.Controls.Add(this.gbEnrolFingerPrintInfo);
            this.Controls.Add(this.gbThumbImpression);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.lnkDeactivate);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmFPEnrollDeactive";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Finger Print Enroll/Deactive";
            this.Load += new System.EventHandler(this.frmFPEnrollDeactive_Load);
            this.gbThumbImpression.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbThumbImpresion)).EndInit();
            this.gbEnrolFingerPrintInfo.ResumeLayout(false);
            this.gbUserFingerDetails.ResumeLayout(false);
            this.gbUserFingerDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserFingerDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLoginId;
        private System.Windows.Forms.Label lblFingerPosition;
        private System.Windows.Forms.ComboBox cmbUserRole;
        private System.Windows.Forms.Label lblUserRole;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.GroupBox gbThumbImpression;
        private System.Windows.Forms.LinkLabel lnkDeactivate;
        private System.Windows.Forms.GroupBox gbEnrolFingerPrintInfo;
        private System.Windows.Forms.ComboBox cmbFingerPosition;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.PictureBox pbThumbImpresion;
        private System.Windows.Forms.ProgressBar prbStatus;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.GroupBox gbUserFingerDetails;
        private System.Windows.Forms.DataGridView dgvUserFingerDetails;
        private System.Windows.Forms.Button btnDeactivation;
        private System.Windows.Forms.Label gblblLoginId;
        private System.Windows.Forms.Label gblabelLoginId;
        private System.Windows.Forms.Label gblblUserName;
        private System.Windows.Forms.Label gblabelUserName;
        private System.Windows.Forms.Button btnRefesh;
        private System.Windows.Forms.DataGridViewTextBoxColumn FingerPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActiveFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}