namespace Parafait_POS
{
    partial class MasterCardManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterCardManagement));
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvMasterCardDetails = new System.Windows.Forms.DataGridView();
            this.dcCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcSiteId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcMachineAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcPlaysSaved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcUpload = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Clear = new System.Windows.Forms.Button();
            this.btnGetDetails = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblPlayDate = new System.Windows.Forms.Label();
            this.dateTimeGamePlay = new System.Windows.Forms.DateTimePicker();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMasterCardDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.AutoSize = true;
            this.lblCardNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblCardNumber.Location = new System.Drawing.Point(35, 24);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(92, 16);
            this.lblCardNumber.TabIndex = 0;
            this.lblCardNumber.Text = "Card Number";
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Location = new System.Drawing.Point(127, 21);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.ReadOnly = true;
            this.txtCardNumber.Size = new System.Drawing.Size(90, 22);
            this.txtCardNumber.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(114, 410);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 60);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "Upload ";
            this.btnOK.UseVisualStyleBackColor = false;
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
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(460, 410);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 60);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvMasterCardDetails
            // 
            this.dgvMasterCardDetails.AllowUserToAddRows = false;
            this.dgvMasterCardDetails.AllowUserToDeleteRows = false;
            this.dgvMasterCardDetails.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dgvMasterCardDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMasterCardDetails.ColumnHeadersHeight = 30;
            this.dgvMasterCardDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMasterCardDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcCardNumber,
            this.dcSiteId,
            this.dcMachineAddress,
            this.dcPlaysSaved,
            this.dcStatus,
            this.dcUpload});
            this.dgvMasterCardDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvMasterCardDetails.Location = new System.Drawing.Point(38, 51);
            this.dgvMasterCardDetails.Name = "dgvMasterCardDetails";
            this.dgvMasterCardDetails.RowHeadersVisible = false;
            this.dgvMasterCardDetails.Size = new System.Drawing.Size(600, 307);
            this.dgvMasterCardDetails.TabIndex = 4;
            // 
            // dcCardNumber
            // 
            this.dcCardNumber.HeaderText = "Card Number";
            this.dcCardNumber.Name = "dcCardNumber";
            // 
            // dcSiteId
            // 
            this.dcSiteId.HeaderText = "Site";
            this.dcSiteId.Name = "dcSiteId";
            // 
            // dcMachineAddress
            // 
            this.dcMachineAddress.HeaderText = "Machine";
            this.dcMachineAddress.Name = "dcMachineAddress";
            // 
            // dcPlaysSaved
            // 
            this.dcPlaysSaved.HeaderText = "Play Index";
            this.dcPlaysSaved.Name = "dcPlaysSaved";
            // 
            // dcStatus
            // 
            this.dcStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dcStatus.HeaderText = "Status";
            this.dcStatus.Name = "dcStatus";
            // 
            // dcUpload
            // 
            this.dcUpload.HeaderText = "Upload";
            this.dcUpload.Name = "dcUpload";
            this.dcUpload.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dcUpload.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dcUpload.Width = 60;
            // 
            // Clear
            // 
            this.Clear.BackColor = System.Drawing.Color.Transparent;
            this.Clear.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.Clear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Clear.Enabled = false;
            this.Clear.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.Clear.FlatAppearance.BorderSize = 0;
            this.Clear.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.Clear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Clear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Clear.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.Clear.ForeColor = System.Drawing.Color.White;
            this.Clear.Location = new System.Drawing.Point(287, 410);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(100, 60);
            this.Clear.TabIndex = 10;
            this.Clear.Text = "Clear Card";
            this.Clear.UseVisualStyleBackColor = false;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // btnGetDetails
            // 
            this.btnGetDetails.BackColor = System.Drawing.Color.Transparent;
            this.btnGetDetails.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnGetDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGetDetails.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnGetDetails.FlatAppearance.BorderSize = 0;
            this.btnGetDetails.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnGetDetails.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGetDetails.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGetDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetDetails.ForeColor = System.Drawing.Color.White;
            this.btnGetDetails.Location = new System.Drawing.Point(223, 19);
            this.btnGetDetails.Name = "btnGetDetails";
            this.btnGetDetails.Size = new System.Drawing.Size(110, 25);
            this.btnGetDetails.TabIndex = 11;
            this.btnGetDetails.Text = "Get Details";
            this.btnGetDetails.UseVisualStyleBackColor = false;
            this.btnGetDetails.Click += new System.EventHandler(this.btnGetDetails_Click);
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(38, 378);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(600, 22);
            this.txtLog.TabIndex = 12;
            // 
            // lblPlayDate
            // 
            this.lblPlayDate.AutoSize = true;
            this.lblPlayDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblPlayDate.Location = new System.Drawing.Point(342, 24);
            this.lblPlayDate.Name = "lblPlayDate";
            this.lblPlayDate.Size = new System.Drawing.Size(37, 16);
            this.lblPlayDate.TabIndex = 14;
            this.lblPlayDate.Text = "Date";
            // 
            // dateTimeGamePlay
            // 
            this.dateTimeGamePlay.CustomFormat = "dd-MMM-yyyy";
            this.dateTimeGamePlay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimeGamePlay.Location = new System.Drawing.Point(379, 21);
            this.dateTimeGamePlay.Name = "dateTimeGamePlay";
            this.dateTimeGamePlay.Size = new System.Drawing.Size(128, 22);
            this.dateTimeGamePlay.TabIndex = 15;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(38, 363);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(600, 10);
            this.progressBar.TabIndex = 16;
            // 
            // MasterCardManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 478);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.dateTimeGamePlay);
            this.Controls.Add(this.lblPlayDate);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnGetDetails);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgvMasterCardDetails);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.lblCardNumber);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MasterCardManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Master Card Management";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MasterCardManagement_FormClosed);
            this.Load += new System.EventHandler(this.MasterCardManagement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMasterCardDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvMasterCardDetails;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.Button btnGetDetails;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblPlayDate;
        private System.Windows.Forms.DateTimePicker dateTimeGamePlay;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcCardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcSiteId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcMachineAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcPlaysSaved;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcStatus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dcUpload;
    }
}