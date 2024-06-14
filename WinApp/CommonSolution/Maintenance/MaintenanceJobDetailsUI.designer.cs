namespace Semnox.Parafait.Maintenance
{
    partial class MaintenanceJobDetailsUI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.adhocDeleteBtn = new System.Windows.Forms.Button();
            this.adhocCloseBtn = new System.Windows.Forms.Button();
            this.adhocRefreshBtn = new System.Windows.Forms.Button();
            this.adhocSaveBtn = new System.Windows.Forms.Button();
            this.adhocDataGridView = new System.Windows.Forms.DataGridView();
            this.JobTaskId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.btnRaiseServiceRequest = new System.Windows.Forms.DataGridViewButtonColumn();
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.txtScheduleTo = new System.Windows.Forms.TextBox();
            this.txtScheduleFrom = new System.Windows.Forms.TextBox();
            this.cmbTaskName = new System.Windows.Forms.ComboBox();
            this.cmbAssetName = new System.Windows.Forms.ComboBox();
            this.lblScheduleTime = new System.Windows.Forms.Label();
            this.dtscheduleDate = new System.Windows.Forms.DateTimePicker();
            this.lblCloseDate = new System.Windows.Forms.Label();
            this.dtpCloseDate = new System.Windows.Forms.DateTimePicker();
            this.chbPastDueDate = new System.Windows.Forms.CheckBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbAssignedTo = new System.Windows.Forms.ComboBox();
            this.lblAssignedTo = new System.Windows.Forms.Label();
            this.lblAssetName = new System.Windows.Forms.Label();
            this.dgvAssetNameSearch = new System.Windows.Forms.DataGridView();
            this.txtName = new System.Windows.Forms.TextBox();
            this.dgvAssignedToSearch = new System.Windows.Forms.DataGridView();
            this.txtAssignedTo = new System.Windows.Forms.TextBox();
            this.dgvTaskSearch = new System.Windows.Forms.DataGridView();
            this.txtTaskNameSearch = new System.Windows.Forms.TextBox();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.btnPublishToSite = new System.Windows.Forms.Button();
            this.maintChklstdetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maintJobNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chklstScheduleTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assignedToDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assignedUserIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.departmentIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.checklistCloseDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.validateTagDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cardIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskCardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.assetNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetGroupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chklistValueDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.chklistRemarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceSystemIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.durationToCompleteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requestTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priorityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requestDetailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requestedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contactPhoneDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contactEmailIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resolutionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.repairCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.docFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attribute1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.maintenanceJobDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.adhocDataGridView)).BeginInit();
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssetNameSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignedToSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaskSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceJobDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // adhocDeleteBtn
            // 
            this.adhocDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.adhocDeleteBtn.Enabled = false;
            this.adhocDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.adhocDeleteBtn.Location = new System.Drawing.Point(269, 281);
            this.adhocDeleteBtn.Name = "adhocDeleteBtn";
            this.adhocDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.adhocDeleteBtn.TabIndex = 6;
            this.adhocDeleteBtn.Text = "Delete";
            this.adhocDeleteBtn.UseVisualStyleBackColor = true;
            this.adhocDeleteBtn.Click += new System.EventHandler(this.adhocDeleteBtn_Click);
            // 
            // adhocCloseBtn
            // 
            this.adhocCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.adhocCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.adhocCloseBtn.Location = new System.Drawing.Point(397, 281);
            this.adhocCloseBtn.Name = "adhocCloseBtn";
            this.adhocCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.adhocCloseBtn.TabIndex = 7;
            this.adhocCloseBtn.Text = "Close";
            this.adhocCloseBtn.UseVisualStyleBackColor = true;
            this.adhocCloseBtn.Click += new System.EventHandler(this.adhocCloseBtn_Click);
            // 
            // adhocRefreshBtn
            // 
            this.adhocRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.adhocRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.adhocRefreshBtn.Location = new System.Drawing.Point(141, 281);
            this.adhocRefreshBtn.Name = "adhocRefreshBtn";
            this.adhocRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.adhocRefreshBtn.TabIndex = 5;
            this.adhocRefreshBtn.Text = "Refresh";
            this.adhocRefreshBtn.UseVisualStyleBackColor = true;
            this.adhocRefreshBtn.Click += new System.EventHandler(this.adhocRefreshBtn_Click);
            // 
            // adhocSaveBtn
            // 
            this.adhocSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.adhocSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.adhocSaveBtn.Location = new System.Drawing.Point(13, 281);
            this.adhocSaveBtn.Name = "adhocSaveBtn";
            this.adhocSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.adhocSaveBtn.TabIndex = 4;
            this.adhocSaveBtn.Text = "Save";
            this.adhocSaveBtn.UseVisualStyleBackColor = true;
            this.adhocSaveBtn.Click += new System.EventHandler(this.adhocSaveBtn_Click);
            // 
            // adhocDataGridView
            // 
            this.adhocDataGridView.AllowUserToAddRows = false;
            this.adhocDataGridView.AllowUserToDeleteRows = false;
            this.adhocDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adhocDataGridView.AutoGenerateColumns = false;
            this.adhocDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.adhocDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.maintChklstdetIdDataGridViewTextBoxColumn,
            this.JobTaskId,
            this.maintJobNameDataGridViewTextBoxColumn,
            this.chklstScheduleTimeDataGridViewTextBoxColumn,
            this.assignedToDataGridViewTextBoxColumn,
            this.assignedUserIdDataGridViewTextBoxColumn,
            this.departmentIdDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.checklistCloseDateDataGridViewTextBoxColumn,
            this.taskNameDataGridViewTextBoxColumn,
            this.validateTagDataGridViewCheckBoxColumn,
            this.cardIdDataGridViewTextBoxColumn,
            this.cardNumberDataGridViewTextBoxColumn,
            this.taskCardNumberDataGridViewTextBoxColumn,
            this.assetIdDataGridViewTextBoxColumn,
            this.assetNameDataGridViewTextBoxColumn,
            this.assetTypeDataGridViewTextBoxColumn,
            this.assetGroupNameDataGridViewTextBoxColumn,
            this.chklistValueDataGridViewCheckBoxColumn,
            this.chklistRemarksDataGridViewTextBoxColumn,
            this.sourceSystemIdDataGridViewTextBoxColumn,
            this.durationToCompleteDataGridViewTextBoxColumn,
            this.requestTypeDataGridViewTextBoxColumn,
            this.priorityDataGridViewTextBoxColumn,
            this.requestDetailDataGridViewTextBoxColumn,
            this.imageNameDataGridViewTextBoxColumn,
            this.requestedByDataGridViewTextBoxColumn,
            this.contactPhoneDataGridViewTextBoxColumn,
            this.contactEmailIdDataGridViewTextBoxColumn,
            this.resolutionDataGridViewTextBoxColumn,
            this.commentsDataGridViewTextBoxColumn,
            this.repairCostDataGridViewTextBoxColumn,
            this.docFileNameDataGridViewTextBoxColumn,
            this.attribute1DataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.btnRaiseServiceRequest});
            this.adhocDataGridView.DataSource = this.maintenanceJobDTOBindingSource;
            this.adhocDataGridView.Location = new System.Drawing.Point(5, 83);
            this.adhocDataGridView.Name = "adhocDataGridView";
            this.adhocDataGridView.Size = new System.Drawing.Size(1110, 189);
            this.adhocDataGridView.TabIndex = 3;
            this.adhocDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.adhocDataGridView_CellClick);
            this.adhocDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.adhocDataGridView_DataError);
            this.adhocDataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.adhocDataGridView_DefaultValuesNeeded);
            this.adhocDataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.adhocDataGridView_RowEnter);
            // 
            // JobTaskId
            // 
            this.JobTaskId.DataPropertyName = "JobTaskId";
            this.JobTaskId.HeaderText = "Job Task Id";
            this.JobTaskId.Name = "JobTaskId";
            this.JobTaskId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.JobTaskId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // btnRaiseServiceRequest
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "....";
            this.btnRaiseServiceRequest.DefaultCellStyle = dataGridViewCellStyle1;
            this.btnRaiseServiceRequest.HeaderText = "Raise Service Request";
            this.btnRaiseServiceRequest.Name = "btnRaiseServiceRequest";
            this.btnRaiseServiceRequest.Text = "";
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.txtScheduleTo);
            this.gpFilter.Controls.Add(this.txtScheduleFrom);
            this.gpFilter.Controls.Add(this.cmbTaskName);
            this.gpFilter.Controls.Add(this.cmbAssetName);
            this.gpFilter.Controls.Add(this.lblScheduleTime);
            this.gpFilter.Controls.Add(this.dtscheduleDate);
            this.gpFilter.Controls.Add(this.lblCloseDate);
            this.gpFilter.Controls.Add(this.dtpCloseDate);
            this.gpFilter.Controls.Add(this.chbPastDueDate);
            this.gpFilter.Controls.Add(this.cmbStatus);
            this.gpFilter.Controls.Add(this.lblStatus);
            this.gpFilter.Controls.Add(this.lblTaskName);
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.cmbAssignedTo);
            this.gpFilter.Controls.Add(this.lblAssignedTo);
            this.gpFilter.Controls.Add(this.lblAssetName);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(5, 3);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(1110, 76);
            this.gpFilter.TabIndex = 57;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // txtScheduleTo
            // 
            this.txtScheduleTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtScheduleTo.Location = new System.Drawing.Point(633, 48);
            this.txtScheduleTo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtScheduleTo.Name = "txtScheduleTo";
            this.txtScheduleTo.Size = new System.Drawing.Size(122, 21);
            this.txtScheduleTo.TabIndex = 32;
            // 
            // txtScheduleFrom
            // 
            this.txtScheduleFrom.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtScheduleFrom.Location = new System.Drawing.Point(371, 48);
            this.txtScheduleFrom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtScheduleFrom.Name = "txtScheduleFrom";
            this.txtScheduleFrom.Size = new System.Drawing.Size(122, 21);
            this.txtScheduleFrom.TabIndex = 31;
            // 
            // cmbTaskName
            // 
            this.cmbTaskName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTaskName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbTaskName.FormattingEnabled = true;
            this.cmbTaskName.Location = new System.Drawing.Point(371, 16);
            this.cmbTaskName.Name = "cmbTaskName";
            this.cmbTaskName.Size = new System.Drawing.Size(141, 23);
            this.cmbTaskName.TabIndex = 30;
            this.cmbTaskName.SelectedValueChanged += new System.EventHandler(this.cmbTaskName_SelectedValueChanged);
            this.cmbTaskName.Click += new System.EventHandler(this.cmbCommon_Click);
            // 
            // cmbAssetName
            // 
            this.cmbAssetName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssetName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbAssetName.FormattingEnabled = true;
            this.cmbAssetName.Location = new System.Drawing.Point(101, 16);
            this.cmbAssetName.Name = "cmbAssetName";
            this.cmbAssetName.Size = new System.Drawing.Size(141, 23);
            this.cmbAssetName.TabIndex = 29;
            this.cmbAssetName.SelectedValueChanged += new System.EventHandler(this.cmbAssetName_SelectedValueChanged);
            this.cmbAssetName.Click += new System.EventHandler(this.cmbCommon_Click);
            // 
            // lblScheduleTime
            // 
            this.lblScheduleTime.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblScheduleTime.Location = new System.Drawing.Point(249, 51);
            this.lblScheduleTime.Name = "lblScheduleTime";
            this.lblScheduleTime.Size = new System.Drawing.Size(116, 15);
            this.lblScheduleTime.TabIndex = 25;
            this.lblScheduleTime.Text = "Schedule From:";
            this.lblScheduleTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtscheduleDate
            // 
            this.dtscheduleDate.CustomFormat = "dd-MMM-yyyy";
            this.dtscheduleDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dtscheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtscheduleDate.Location = new System.Drawing.Point(493, 48);
            this.dtscheduleDate.Name = "dtscheduleDate";
            this.dtscheduleDate.Size = new System.Drawing.Size(19, 21);
            this.dtscheduleDate.TabIndex = 24;
            this.dtscheduleDate.Value = new System.DateTime(2016, 1, 21, 10, 13, 0, 0);
            this.dtscheduleDate.ValueChanged += new System.EventHandler(this.dtscheduleDate_ValueChanged);
            // 
            // lblCloseDate
            // 
            this.lblCloseDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCloseDate.Location = new System.Drawing.Point(524, 49);
            this.lblCloseDate.Name = "lblCloseDate";
            this.lblCloseDate.Size = new System.Drawing.Size(103, 18);
            this.lblCloseDate.TabIndex = 27;
            this.lblCloseDate.Text = "Schedule To:";
            this.lblCloseDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpCloseDate
            // 
            this.dtpCloseDate.CustomFormat = "dd-MMM-yyyy";
            this.dtpCloseDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dtpCloseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCloseDate.Location = new System.Drawing.Point(755, 48);
            this.dtpCloseDate.Name = "dtpCloseDate";
            this.dtpCloseDate.Size = new System.Drawing.Size(18, 21);
            this.dtpCloseDate.TabIndex = 26;
            this.dtpCloseDate.ValueChanged += new System.EventHandler(this.dtpCloseDate_ValueChanged);
            // 
            // chbPastDueDate
            // 
            this.chbPastDueDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbPastDueDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbPastDueDate.Location = new System.Drawing.Point(11, 46);
            this.chbPastDueDate.Name = "chbPastDueDate";
            this.chbPastDueDate.Size = new System.Drawing.Size(230, 24);
            this.chbPastDueDate.TabIndex = 28;
            this.chbPastDueDate.Text = "Jobs past due date";
            this.chbPastDueDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbPastDueDate.UseVisualStyleBackColor = true;
            this.chbPastDueDate.CheckedChanged += new System.EventHandler(this.chbPastDueDate_CheckedChanged);
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(868, 16);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(141, 23);
            this.cmbStatus.TabIndex = 22;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(782, 17);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(78, 20);
            this.lblStatus.TabIndex = 23;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTaskName
            // 
            this.lblTaskName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblTaskName.Location = new System.Drawing.Point(252, 17);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(113, 20);
            this.lblTaskName.TabIndex = 21;
            this.lblTaskName.Text = "Task Name:";
            this.lblTaskName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(868, 47);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbAssignedTo
            // 
            this.cmbAssignedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssignedTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbAssignedTo.FormattingEnabled = true;
            this.cmbAssignedTo.Location = new System.Drawing.Point(633, 16);
            this.cmbAssignedTo.Name = "cmbAssignedTo";
            this.cmbAssignedTo.Size = new System.Drawing.Size(141, 23);
            this.cmbAssignedTo.TabIndex = 1;
            this.cmbAssignedTo.SelectedValueChanged += new System.EventHandler(this.cmbAssignedTo_SelectedValueChanged);
            this.cmbAssignedTo.Click += new System.EventHandler(this.cmbCommon_Click);
            // 
            // lblAssignedTo
            // 
            this.lblAssignedTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssignedTo.Location = new System.Drawing.Point(521, 17);
            this.lblAssignedTo.Name = "lblAssignedTo";
            this.lblAssignedTo.Size = new System.Drawing.Size(106, 20);
            this.lblAssignedTo.TabIndex = 19;
            this.lblAssignedTo.Text = "Assigned To:";
            this.lblAssignedTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAssetName
            // 
            this.lblAssetName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssetName.Location = new System.Drawing.Point(8, 17);
            this.lblAssetName.Name = "lblAssetName";
            this.lblAssetName.Size = new System.Drawing.Size(87, 20);
            this.lblAssetName.TabIndex = 13;
            this.lblAssetName.Text = "Asset Name:";
            this.lblAssetName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvAssetNameSearch
            // 
            this.dgvAssetNameSearch.AllowUserToAddRows = false;
            this.dgvAssetNameSearch.AllowUserToDeleteRows = false;
            this.dgvAssetNameSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvAssetNameSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAssetNameSearch.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvAssetNameSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssetNameSearch.ColumnHeadersVisible = false;
            this.dgvAssetNameSearch.Location = new System.Drawing.Point(108, 44);
            this.dgvAssetNameSearch.Name = "dgvAssetNameSearch";
            this.dgvAssetNameSearch.ReadOnly = true;
            this.dgvAssetNameSearch.RowHeadersVisible = false;
            this.dgvAssetNameSearch.RowTemplate.Height = 35;
            this.dgvAssetNameSearch.Size = new System.Drawing.Size(139, 181);
            this.dgvAssetNameSearch.TabIndex = 60;
            this.dgvAssetNameSearch.Visible = false;
            this.dgvAssetNameSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAssetNameSearch_CellClick);
            this.dgvAssetNameSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvAssetNameSearch_KeyDown);
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(110, 23);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(119, 14);
            this.txtName.TabIndex = 59;
            this.txtName.Click += new System.EventHandler(this.cmbCommon_Click);
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // dgvAssignedToSearch
            // 
            this.dgvAssignedToSearch.AllowUserToAddRows = false;
            this.dgvAssignedToSearch.AllowUserToDeleteRows = false;
            this.dgvAssignedToSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvAssignedToSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAssignedToSearch.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvAssignedToSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssignedToSearch.ColumnHeadersVisible = false;
            this.dgvAssignedToSearch.Location = new System.Drawing.Point(640, 43);
            this.dgvAssignedToSearch.Name = "dgvAssignedToSearch";
            this.dgvAssignedToSearch.ReadOnly = true;
            this.dgvAssignedToSearch.RowHeadersVisible = false;
            this.dgvAssignedToSearch.RowTemplate.Height = 35;
            this.dgvAssignedToSearch.Size = new System.Drawing.Size(139, 181);
            this.dgvAssignedToSearch.TabIndex = 62;
            this.dgvAssignedToSearch.Visible = false;
            this.dgvAssignedToSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAssignedTo_CellClick);
            this.dgvAssignedToSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvAssignedToSearch_KeyDown);
            // 
            // txtAssignedTo
            // 
            this.txtAssignedTo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.txtAssignedTo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAssignedTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtAssignedTo.Location = new System.Drawing.Point(641, 23);
            this.txtAssignedTo.Name = "txtAssignedTo";
            this.txtAssignedTo.Size = new System.Drawing.Size(119, 14);
            this.txtAssignedTo.TabIndex = 61;
            this.txtAssignedTo.Click += new System.EventHandler(this.cmbCommon_Click);
            this.txtAssignedTo.TextChanged += new System.EventHandler(this.txtAssignedTo_TextChanged);
            this.txtAssignedTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAssignedTo_KeyDown);
            // 
            // dgvTaskSearch
            // 
            this.dgvTaskSearch.AllowUserToAddRows = false;
            this.dgvTaskSearch.AllowUserToDeleteRows = false;
            this.dgvTaskSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvTaskSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTaskSearch.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvTaskSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTaskSearch.ColumnHeadersVisible = false;
            this.dgvTaskSearch.Location = new System.Drawing.Point(378, 44);
            this.dgvTaskSearch.Name = "dgvTaskSearch";
            this.dgvTaskSearch.ReadOnly = true;
            this.dgvTaskSearch.RowHeadersVisible = false;
            this.dgvTaskSearch.RowTemplate.Height = 35;
            this.dgvTaskSearch.Size = new System.Drawing.Size(139, 181);
            this.dgvTaskSearch.TabIndex = 64;
            this.dgvTaskSearch.Visible = false;
            this.dgvTaskSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTaskSearch_CellClick);
            this.dgvTaskSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTaskSearch_KeyDown);
            // 
            // txtTaskNameSearch
            // 
            this.txtTaskNameSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.txtTaskNameSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTaskNameSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtTaskNameSearch.Location = new System.Drawing.Point(380, 23);
            this.txtTaskNameSearch.Name = "txtTaskNameSearch";
            this.txtTaskNameSearch.Size = new System.Drawing.Size(119, 14);
            this.txtTaskNameSearch.TabIndex = 63;
            this.txtTaskNameSearch.Click += new System.EventHandler(this.cmbCommon_Click);
            this.txtTaskNameSearch.TextChanged += new System.EventHandler(this.txtTaskNameSearch_TextChanged);
            this.txtTaskNameSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTaskNameSearch_KeyDown);
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCardNumber.Location = new System.Drawing.Point(1117, 301);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(1, 20);
            this.txtCardNumber.TabIndex = 65;
            this.txtCardNumber.Visible = false;
            this.txtCardNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCardNumber_KeyDown);
            this.txtCardNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCardNumber_KeyPress);
            // 
            // btnPublishToSite
            // 
            this.btnPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPublishToSite.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPublishToSite.Location = new System.Drawing.Point(523, 281);
            this.btnPublishToSite.Name = "btnPublishToSite";
            this.btnPublishToSite.Size = new System.Drawing.Size(109, 23);
            this.btnPublishToSite.TabIndex = 68;
            this.btnPublishToSite.Text = "Publish To Sites";
            this.btnPublishToSite.UseVisualStyleBackColor = true;
            this.btnPublishToSite.Visible = false;
            this.btnPublishToSite.Click += new System.EventHandler(this.btnPublishToSite_Click);
            // 
            // maintChklstdetIdDataGridViewTextBoxColumn
            // 
            this.maintChklstdetIdDataGridViewTextBoxColumn.DataPropertyName = "MaintChklstdetId";
            this.maintChklstdetIdDataGridViewTextBoxColumn.HeaderText = "Job ID";
            this.maintChklstdetIdDataGridViewTextBoxColumn.Name = "maintChklstdetIdDataGridViewTextBoxColumn";
            this.maintChklstdetIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // maintJobNameDataGridViewTextBoxColumn
            // 
            this.maintJobNameDataGridViewTextBoxColumn.DataPropertyName = "MaintJobName";
            this.maintJobNameDataGridViewTextBoxColumn.HeaderText = "Job Name";
            this.maintJobNameDataGridViewTextBoxColumn.Name = "maintJobNameDataGridViewTextBoxColumn";
            // 
            // chklstScheduleTimeDataGridViewTextBoxColumn
            // 
            this.chklstScheduleTimeDataGridViewTextBoxColumn.DataPropertyName = "ChklstScheduleTime";
            this.chklstScheduleTimeDataGridViewTextBoxColumn.HeaderText = "Schedule Date";
            this.chklstScheduleTimeDataGridViewTextBoxColumn.Name = "chklstScheduleTimeDataGridViewTextBoxColumn";
            // 
            // assignedToDataGridViewTextBoxColumn
            // 
            this.assignedToDataGridViewTextBoxColumn.DataPropertyName = "AssignedTo";
            this.assignedToDataGridViewTextBoxColumn.HeaderText = "Assigned To";
            this.assignedToDataGridViewTextBoxColumn.Name = "assignedToDataGridViewTextBoxColumn";
            // 
            // assignedUserIdDataGridViewTextBoxColumn
            // 
            this.assignedUserIdDataGridViewTextBoxColumn.DataPropertyName = "AssignedUserId";
            this.assignedUserIdDataGridViewTextBoxColumn.HeaderText = "Assigned User";
            this.assignedUserIdDataGridViewTextBoxColumn.Name = "assignedUserIdDataGridViewTextBoxColumn";
            this.assignedUserIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assignedUserIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // departmentIdDataGridViewTextBoxColumn
            // 
            this.departmentIdDataGridViewTextBoxColumn.DataPropertyName = "DepartmentId";
            this.departmentIdDataGridViewTextBoxColumn.HeaderText = "Department";
            this.departmentIdDataGridViewTextBoxColumn.Name = "departmentIdDataGridViewTextBoxColumn";
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.statusDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // checklistCloseDateDataGridViewTextBoxColumn
            // 
            this.checklistCloseDateDataGridViewTextBoxColumn.DataPropertyName = "ChecklistCloseDate";
            this.checklistCloseDateDataGridViewTextBoxColumn.HeaderText = "Close Date";
            this.checklistCloseDateDataGridViewTextBoxColumn.Name = "checklistCloseDateDataGridViewTextBoxColumn";
            // 
            // taskNameDataGridViewTextBoxColumn
            // 
            this.taskNameDataGridViewTextBoxColumn.DataPropertyName = "TaskName";
            this.taskNameDataGridViewTextBoxColumn.HeaderText = "Task Name";
            this.taskNameDataGridViewTextBoxColumn.Name = "taskNameDataGridViewTextBoxColumn";
            // 
            // validateTagDataGridViewCheckBoxColumn
            // 
            this.validateTagDataGridViewCheckBoxColumn.DataPropertyName = "ValidateTag";
            this.validateTagDataGridViewCheckBoxColumn.FalseValue = "False";
            this.validateTagDataGridViewCheckBoxColumn.HeaderText = "Validate?";
            this.validateTagDataGridViewCheckBoxColumn.Name = "validateTagDataGridViewCheckBoxColumn";
            this.validateTagDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // cardIdDataGridViewTextBoxColumn
            // 
            this.cardIdDataGridViewTextBoxColumn.DataPropertyName = "CardId";
            this.cardIdDataGridViewTextBoxColumn.HeaderText = "Card Id";
            this.cardIdDataGridViewTextBoxColumn.Name = "cardIdDataGridViewTextBoxColumn";
            // 
            // cardNumberDataGridViewTextBoxColumn
            // 
            this.cardNumberDataGridViewTextBoxColumn.DataPropertyName = "CardNumber";
            this.cardNumberDataGridViewTextBoxColumn.HeaderText = "Card Number";
            this.cardNumberDataGridViewTextBoxColumn.Name = "cardNumberDataGridViewTextBoxColumn";
            // 
            // taskCardNumberDataGridViewTextBoxColumn
            // 
            this.taskCardNumberDataGridViewTextBoxColumn.DataPropertyName = "TaskCardNumber";
            this.taskCardNumberDataGridViewTextBoxColumn.HeaderText = "Task Card Number";
            this.taskCardNumberDataGridViewTextBoxColumn.Name = "taskCardNumberDataGridViewTextBoxColumn";
            // 
            // assetIdDataGridViewTextBoxColumn
            // 
            this.assetIdDataGridViewTextBoxColumn.DataPropertyName = "AssetId";
            this.assetIdDataGridViewTextBoxColumn.HeaderText = "Asset";
            this.assetIdDataGridViewTextBoxColumn.Name = "assetIdDataGridViewTextBoxColumn";
            this.assetIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assetIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // assetNameDataGridViewTextBoxColumn
            // 
            this.assetNameDataGridViewTextBoxColumn.DataPropertyName = "AssetName";
            this.assetNameDataGridViewTextBoxColumn.HeaderText = "Asset Name";
            this.assetNameDataGridViewTextBoxColumn.Name = "assetNameDataGridViewTextBoxColumn";
            // 
            // assetTypeDataGridViewTextBoxColumn
            // 
            this.assetTypeDataGridViewTextBoxColumn.DataPropertyName = "AssetType";
            this.assetTypeDataGridViewTextBoxColumn.HeaderText = "Asset Type";
            this.assetTypeDataGridViewTextBoxColumn.Name = "assetTypeDataGridViewTextBoxColumn";
            // 
            // assetGroupNameDataGridViewTextBoxColumn
            // 
            this.assetGroupNameDataGridViewTextBoxColumn.DataPropertyName = "AssetGroupName";
            this.assetGroupNameDataGridViewTextBoxColumn.HeaderText = "Asset Group Name";
            this.assetGroupNameDataGridViewTextBoxColumn.Name = "assetGroupNameDataGridViewTextBoxColumn";
            // 
            // chklistValueDataGridViewCheckBoxColumn
            // 
            this.chklistValueDataGridViewCheckBoxColumn.DataPropertyName = "ChklistValue";
            this.chklistValueDataGridViewCheckBoxColumn.FalseValue = "False";
            this.chklistValueDataGridViewCheckBoxColumn.HeaderText = "Checked?";
            this.chklistValueDataGridViewCheckBoxColumn.Name = "chklistValueDataGridViewCheckBoxColumn";
            this.chklistValueDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // chklistRemarksDataGridViewTextBoxColumn
            // 
            this.chklistRemarksDataGridViewTextBoxColumn.DataPropertyName = "ChklistRemarks";
            this.chklistRemarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.chklistRemarksDataGridViewTextBoxColumn.Name = "chklistRemarksDataGridViewTextBoxColumn";
            // 
            // sourceSystemIdDataGridViewTextBoxColumn
            // 
            this.sourceSystemIdDataGridViewTextBoxColumn.DataPropertyName = "SourceSystemId";
            this.sourceSystemIdDataGridViewTextBoxColumn.HeaderText = "Source System Id";
            this.sourceSystemIdDataGridViewTextBoxColumn.Name = "sourceSystemIdDataGridViewTextBoxColumn";
            // 
            // durationToCompleteDataGridViewTextBoxColumn
            // 
            this.durationToCompleteDataGridViewTextBoxColumn.DataPropertyName = "DurationToComplete";
            this.durationToCompleteDataGridViewTextBoxColumn.HeaderText = "Duration To Complete";
            this.durationToCompleteDataGridViewTextBoxColumn.Name = "durationToCompleteDataGridViewTextBoxColumn";
            // 
            // requestTypeDataGridViewTextBoxColumn
            // 
            this.requestTypeDataGridViewTextBoxColumn.DataPropertyName = "RequestType";
            this.requestTypeDataGridViewTextBoxColumn.HeaderText = "Request Type";
            this.requestTypeDataGridViewTextBoxColumn.Name = "requestTypeDataGridViewTextBoxColumn";
            // 
            // priorityDataGridViewTextBoxColumn
            // 
            this.priorityDataGridViewTextBoxColumn.DataPropertyName = "Priority";
            this.priorityDataGridViewTextBoxColumn.HeaderText = "Priority";
            this.priorityDataGridViewTextBoxColumn.Name = "priorityDataGridViewTextBoxColumn";
            // 
            // requestDetailDataGridViewTextBoxColumn
            // 
            this.requestDetailDataGridViewTextBoxColumn.DataPropertyName = "RequestDetail";
            this.requestDetailDataGridViewTextBoxColumn.HeaderText = "Request Detail";
            this.requestDetailDataGridViewTextBoxColumn.Name = "requestDetailDataGridViewTextBoxColumn";
            // 
            // imageNameDataGridViewTextBoxColumn
            // 
            this.imageNameDataGridViewTextBoxColumn.DataPropertyName = "ImageName";
            this.imageNameDataGridViewTextBoxColumn.HeaderText = "Image Name";
            this.imageNameDataGridViewTextBoxColumn.Name = "imageNameDataGridViewTextBoxColumn";
            // 
            // requestedByDataGridViewTextBoxColumn
            // 
            this.requestedByDataGridViewTextBoxColumn.DataPropertyName = "RequestedBy";
            this.requestedByDataGridViewTextBoxColumn.HeaderText = "Requested By";
            this.requestedByDataGridViewTextBoxColumn.Name = "requestedByDataGridViewTextBoxColumn";
            // 
            // contactPhoneDataGridViewTextBoxColumn
            // 
            this.contactPhoneDataGridViewTextBoxColumn.DataPropertyName = "ContactPhone";
            this.contactPhoneDataGridViewTextBoxColumn.HeaderText = "Contact Phone";
            this.contactPhoneDataGridViewTextBoxColumn.Name = "contactPhoneDataGridViewTextBoxColumn";
            // 
            // contactEmailIdDataGridViewTextBoxColumn
            // 
            this.contactEmailIdDataGridViewTextBoxColumn.DataPropertyName = "ContactEmailId";
            this.contactEmailIdDataGridViewTextBoxColumn.HeaderText = "Contact EmailId";
            this.contactEmailIdDataGridViewTextBoxColumn.Name = "contactEmailIdDataGridViewTextBoxColumn";
            // 
            // resolutionDataGridViewTextBoxColumn
            // 
            this.resolutionDataGridViewTextBoxColumn.DataPropertyName = "Resolution";
            this.resolutionDataGridViewTextBoxColumn.HeaderText = "Resolution";
            this.resolutionDataGridViewTextBoxColumn.Name = "resolutionDataGridViewTextBoxColumn";
            // 
            // commentsDataGridViewTextBoxColumn
            // 
            this.commentsDataGridViewTextBoxColumn.DataPropertyName = "Comments";
            this.commentsDataGridViewTextBoxColumn.HeaderText = "Comments";
            this.commentsDataGridViewTextBoxColumn.Name = "commentsDataGridViewTextBoxColumn";
            // 
            // repairCostDataGridViewTextBoxColumn
            // 
            this.repairCostDataGridViewTextBoxColumn.DataPropertyName = "RepairCost";
            this.repairCostDataGridViewTextBoxColumn.HeaderText = "Repair Cost";
            this.repairCostDataGridViewTextBoxColumn.Name = "repairCostDataGridViewTextBoxColumn";
            // 
            // docFileNameDataGridViewTextBoxColumn
            // 
            this.docFileNameDataGridViewTextBoxColumn.DataPropertyName = "DocFileName";
            this.docFileNameDataGridViewTextBoxColumn.HeaderText = "Doc File Name";
            this.docFileNameDataGridViewTextBoxColumn.Name = "docFileNameDataGridViewTextBoxColumn";
            // 
            // attribute1DataGridViewTextBoxColumn
            // 
            this.attribute1DataGridViewTextBoxColumn.DataPropertyName = "Attribute1";
            this.attribute1DataGridViewTextBoxColumn.HeaderText = "Attribute";
            this.attribute1DataGridViewTextBoxColumn.Name = "attribute1DataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "False";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // maintenanceJobDTOBindingSource
            // 
            this.maintenanceJobDTOBindingSource.DataSource = typeof(Semnox.Parafait.Maintenance.UserJobItemsDTO);
            // 
            // MaintenanceJobDetailsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 322);
            this.Controls.Add(this.btnPublishToSite);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.dgvTaskSearch);
            this.Controls.Add(this.txtTaskNameSearch);
            this.Controls.Add(this.dgvAssignedToSearch);
            this.Controls.Add(this.txtAssignedTo);
            this.Controls.Add(this.dgvAssetNameSearch);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.adhocDeleteBtn);
            this.Controls.Add(this.adhocCloseBtn);
            this.Controls.Add(this.adhocRefreshBtn);
            this.Controls.Add(this.adhocSaveBtn);
            this.Controls.Add(this.adhocDataGridView);
            this.Controls.Add(this.gpFilter);
            this.Name = "MaintenanceJobDetailsUI";
            this.Text = "Maintenance Job Details";
            this.Load += new System.EventHandler(this.MaintenanceJobDetailsUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.adhocDataGridView)).EndInit();
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssetNameSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignedToSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaskSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceJobDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button adhocDeleteBtn;
        private System.Windows.Forms.Button adhocCloseBtn;
        private System.Windows.Forms.Button adhocRefreshBtn;
        private System.Windows.Forms.Button adhocSaveBtn;
        private System.Windows.Forms.DataGridView adhocDataGridView;
        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.ComboBox cmbAssignedTo;
        private System.Windows.Forms.Label lblAssignedTo;
        private System.Windows.Forms.Label lblAssetName;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblScheduleTime;
        private System.Windows.Forms.DateTimePicker dtscheduleDate;
        private System.Windows.Forms.Label lblCloseDate;
        private System.Windows.Forms.DateTimePicker dtpCloseDate;
        private System.Windows.Forms.CheckBox chbPastDueDate;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.ComboBox cmbTaskName;
        private System.Windows.Forms.ComboBox cmbAssetName;
        private System.Windows.Forms.TextBox txtScheduleFrom;
        private System.Windows.Forms.TextBox txtScheduleTo;
        private System.Windows.Forms.BindingSource maintenanceJobDTOBindingSource;
        // private System.Windows.Forms.DataGridViewTextBoxColumn requestTitleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView dgvAssetNameSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.DataGridView dgvAssignedToSearch;
        private System.Windows.Forms.TextBox txtAssignedTo;
        private System.Windows.Forms.DataGridView dgvTaskSearch;
        private System.Windows.Forms.TextBox txtTaskNameSearch;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Button btnPublishToSite;
        private System.Windows.Forms.DataGridViewTextBoxColumn maintChklstdetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn JobTaskId;
        private System.Windows.Forms.DataGridViewTextBoxColumn maintJobNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chklstScheduleTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assignedToDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assignedUserIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn departmentIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn checklistCloseDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn validateTagDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskCardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetGroupNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chklistValueDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chklistRemarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceSystemIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn durationToCompleteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn requestTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priorityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn requestDetailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn requestedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn contactPhoneDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn contactEmailIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resolutionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn repairCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn docFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attribute1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn btnRaiseServiceRequest;
    }
}