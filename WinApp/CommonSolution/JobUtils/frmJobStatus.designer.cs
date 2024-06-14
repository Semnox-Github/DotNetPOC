namespace Semnox.Parafait.JobUtils
{
    partial class frmJobStatus
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
            this.cmbPhase = new System.Windows.Forms.ComboBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DgvJobSchedule = new System.Windows.Forms.DataGridView();
            this.requestedIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.programNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executableNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phaseDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.argumentsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErrorNotificationMailId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuccessNotificationMailId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErrorCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.concurrentProgramJobStatusDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.BtnSearch = new System.Windows.Forms.Button();
            this.BtnRefresh = new System.Windows.Forms.Button();
            this.BtnClose = new System.Windows.Forms.Button();
            this.dtrStartTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtrEndTime = new System.Windows.Forms.DateTimePicker();
            this.grpFilter = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.DgvJobSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramJobStatusDTOBindingSource)).BeginInit();
            this.grpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbPhase
            // 
            this.cmbPhase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPhase.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPhase.FormattingEnabled = true;
            this.cmbPhase.Items.AddRange(new object[] {
            "-All-",
            "Complete",
            "Pending",
            "Running"});
            this.cmbPhase.Location = new System.Drawing.Point(111, 57);
            this.cmbPhase.Name = "cmbPhase";
            this.cmbPhase.Size = new System.Drawing.Size(147, 23);
            this.cmbPhase.TabIndex = 0;
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "-All-",
            "Normal",
            "Error",
            "Aborted"});
            this.cmbStatus.Location = new System.Drawing.Point(398, 57);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(147, 23);
            this.cmbStatus.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(56, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Phase :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(347, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Status :";
            // 
            // DgvJobSchedule
            // 
            this.DgvJobSchedule.AllowUserToAddRows = false;
            this.DgvJobSchedule.AllowUserToDeleteRows = false;
            this.DgvJobSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvJobSchedule.AutoGenerateColumns = false;
            this.DgvJobSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvJobSchedule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.requestedIdDataGridViewTextBoxColumn,
            this.programNameDataGridViewTextBoxColumn,
            this.executableNameDataGridViewTextBoxColumn,
            this.startTimeDataGridViewTextBoxColumn,
            this.endTimeDataGridViewTextBoxColumn,
            this.phaseDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.argumentsDataGridViewTextBoxColumn,
            this.ErrorNotificationMailId,
            this.SuccessNotificationMailId,
            this.ErrorCount});
            this.DgvJobSchedule.DataSource = this.concurrentProgramJobStatusDTOBindingSource;
            this.DgvJobSchedule.Location = new System.Drawing.Point(10, 97);
            this.DgvJobSchedule.Name = "DgvJobSchedule";
            this.DgvJobSchedule.ReadOnly = true;
            this.DgvJobSchedule.Size = new System.Drawing.Size(1002, 366);
            this.DgvJobSchedule.TabIndex = 5;
            this.DgvJobSchedule.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DgvJobSchedule_DataBindingComplete);
            // 
            // requestedIdDataGridViewTextBoxColumn
            // 
            this.requestedIdDataGridViewTextBoxColumn.DataPropertyName = "RequestedId";
            this.requestedIdDataGridViewTextBoxColumn.HeaderText = "Argument Id";
            this.requestedIdDataGridViewTextBoxColumn.Name = "requestedIdDataGridViewTextBoxColumn";
            this.requestedIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.requestedIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // programNameDataGridViewTextBoxColumn
            // 
            this.programNameDataGridViewTextBoxColumn.DataPropertyName = "ProgramName";
            this.programNameDataGridViewTextBoxColumn.HeaderText = "Program Name";
            this.programNameDataGridViewTextBoxColumn.Name = "programNameDataGridViewTextBoxColumn";
            this.programNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // executableNameDataGridViewTextBoxColumn
            // 
            this.executableNameDataGridViewTextBoxColumn.DataPropertyName = "ExecutableName";
            this.executableNameDataGridViewTextBoxColumn.HeaderText = "Executable Name";
            this.executableNameDataGridViewTextBoxColumn.Name = "executableNameDataGridViewTextBoxColumn";
            this.executableNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // startTimeDataGridViewTextBoxColumn
            // 
            this.startTimeDataGridViewTextBoxColumn.DataPropertyName = "StartTime";
            this.startTimeDataGridViewTextBoxColumn.HeaderText = "StartTime";
            this.startTimeDataGridViewTextBoxColumn.Name = "startTimeDataGridViewTextBoxColumn";
            this.startTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // endTimeDataGridViewTextBoxColumn
            // 
            this.endTimeDataGridViewTextBoxColumn.DataPropertyName = "EndTime";
            this.endTimeDataGridViewTextBoxColumn.HeaderText = "EndTime";
            this.endTimeDataGridViewTextBoxColumn.Name = "endTimeDataGridViewTextBoxColumn";
            this.endTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // phaseDataGridViewTextBoxColumn
            // 
            this.phaseDataGridViewTextBoxColumn.DataPropertyName = "Phase";
            this.phaseDataGridViewTextBoxColumn.HeaderText = "Phase";
            this.phaseDataGridViewTextBoxColumn.Name = "phaseDataGridViewTextBoxColumn";
            this.phaseDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // argumentsDataGridViewTextBoxColumn
            // 
            this.argumentsDataGridViewTextBoxColumn.DataPropertyName = "Arguments";
            this.argumentsDataGridViewTextBoxColumn.HeaderText = "Arguments";
            this.argumentsDataGridViewTextBoxColumn.Name = "argumentsDataGridViewTextBoxColumn";
            this.argumentsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ErrorNotificationMailId
            // 
            this.ErrorNotificationMailId.DataPropertyName = "ErrorNotificationMailId";
            this.ErrorNotificationMailId.HeaderText = "Error Notification MailId";
            this.ErrorNotificationMailId.Name = "ErrorNotificationMailId";
            this.ErrorNotificationMailId.ReadOnly = true;
            // 
            // SuccessNotificationMailId
            // 
            this.SuccessNotificationMailId.DataPropertyName = "SuccessNotificationMailId";
            this.SuccessNotificationMailId.HeaderText = "Success Notification MailId";
            this.SuccessNotificationMailId.Name = "SuccessNotificationMailId";
            this.SuccessNotificationMailId.ReadOnly = true;
            // 
            // ErrorCount
            // 
            this.ErrorCount.DataPropertyName = "ErrorCount";
            this.ErrorCount.HeaderText = "Error Count";
            this.ErrorCount.Name = "ErrorCount";
            this.ErrorCount.ReadOnly = true;
            // 
            // concurrentProgramJobStatusDTOBindingSource
            // 
            this.concurrentProgramJobStatusDTOBindingSource.DataSource = typeof(Semnox.Parafait.JobUtils.ConcurrentProgramJobStatusDTO);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(555, 44);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 23);
            this.BtnSearch.TabIndex = 6;
            this.BtnSearch.Text = "Search";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // BtnRefresh
            // 
            this.BtnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnRefresh.Location = new System.Drawing.Point(10, 472);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(75, 23);
            this.BtnRefresh.TabIndex = 7;
            this.BtnRefresh.Text = "Refresh";
            this.BtnRefresh.UseVisualStyleBackColor = true;
            this.BtnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnClose.Location = new System.Drawing.Point(125, 472);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 8;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // dtrStartTime
            // 
            this.dtrStartTime.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtrStartTime.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtrStartTime.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtrStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtrStartTime.Location = new System.Drawing.Point(111, 29);
            this.dtrStartTime.Name = "dtrStartTime";
            this.dtrStartTime.Size = new System.Drawing.Size(147, 21);
            this.dtrStartTime.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Start Date :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(331, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "End Date:";
            // 
            // dtrEndTime
            // 
            this.dtrEndTime.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtrEndTime.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtrEndTime.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtrEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtrEndTime.Location = new System.Drawing.Point(398, 27);
            this.dtrEndTime.Name = "dtrEndTime";
            this.dtrEndTime.Size = new System.Drawing.Size(147, 21);
            this.dtrEndTime.TabIndex = 11;
            // 
            // grpFilter
            // 
            this.grpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFilter.Controls.Add(this.BtnSearch);
            this.grpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpFilter.Location = new System.Drawing.Point(10, 12);
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.Size = new System.Drawing.Size(1004, 79);
            this.grpFilter.TabIndex = 14;
            this.grpFilter.TabStop = false;
            this.grpFilter.Text = "Filter";
            // 
            // frmJobStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 507);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtrEndTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtrStartTime);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnRefresh);
            this.Controls.Add(this.DgvJobSchedule);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.cmbPhase);
            this.Controls.Add(this.grpFilter);
            this.Name = "frmJobStatus";
            this.Text = "Concurrent Program Job Status";
            this.Load += new System.EventHandler(this.frmJobStatus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvJobSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramJobStatusDTOBindingSource)).EndInit();
            this.grpFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPhase;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView DgvJobSchedule;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Button BtnRefresh;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.DateTimePicker dtrStartTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtrEndTime;
        private System.Windows.Forms.GroupBox grpFilter;
        private System.Windows.Forms.BindingSource concurrentProgramJobStatusDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn requestedIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn programNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn executableNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn phaseDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn argumentsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrorNotificationMailId;
        private System.Windows.Forms.DataGridViewTextBoxColumn SuccessNotificationMailId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrorCount;
    }
}