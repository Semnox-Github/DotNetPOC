﻿namespace Semnox.Parafait.Maintenance
{
    partial class AdhocJobUI
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
            this.adhocDeleteBtn = new System.Windows.Forms.Button();
            this.adhocCloseBtn = new System.Windows.Forms.Button();
            this.adhocRefreshBtn = new System.Windows.Forms.Button();
            this.adhocSaveBtn = new System.Windows.Forms.Button();
            this.adhocDataGridView = new System.Windows.Forms.DataGridView();
            this.maintenanceJobDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.txtAssignedTo = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cmbAssetName = new System.Windows.Forms.ComboBox();
            this.cmbAssignedTo = new System.Windows.Forms.ComboBox();
            this.lblDays = new System.Windows.Forms.Label();
            this.txtCompleted = new System.Windows.Forms.TextBox();
            this.lblCompleted = new System.Windows.Forms.Label();
            this.lblAssignedTo = new System.Windows.Forms.Label();
            this.lblAssetName = new System.Windows.Forms.Label();
            this.dgvAssignedTo = new System.Windows.Forms.DataGridView();
            this.dgvAssetNameSearch = new System.Windows.Forms.DataGridView();
            this.gpTaskDetail = new System.Windows.Forms.GroupBox();
            this.maintChklstdetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jobTaskId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.maintJobNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chklstScheduleTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assignedToDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assignedUserIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.departmentIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checklistCloseDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.validateTagDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cardIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskCardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksMandatoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.assetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.assetNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetGroupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chklistValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
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
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.adhocDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceJobDTOBindingSource)).BeginInit();
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignedTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssetNameSearch)).BeginInit();
            this.gpTaskDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // adhocDeleteBtn
            // 
            this.adhocDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.adhocDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.adhocDeleteBtn.Location = new System.Drawing.Point(269, 389);
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
            this.adhocCloseBtn.Location = new System.Drawing.Point(397, 389);
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
            this.adhocRefreshBtn.Location = new System.Drawing.Point(141, 389);
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
            this.adhocSaveBtn.Location = new System.Drawing.Point(13, 389);
            this.adhocSaveBtn.Name = "adhocSaveBtn";
            this.adhocSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.adhocSaveBtn.TabIndex = 4;
            this.adhocSaveBtn.Text = "Save";
            this.adhocSaveBtn.UseVisualStyleBackColor = true;
            this.adhocSaveBtn.Click += new System.EventHandler(this.adhocSaveBtn_Click);
            // 
            // adhocDataGridView
            // 
            this.adhocDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adhocDataGridView.AutoGenerateColumns = false;
            this.adhocDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.adhocDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.maintChklstdetIdDataGridViewTextBoxColumn,
            this.jobTaskId,
            this.maintJobNameDataGridViewTextBoxColumn,
            this.chklstScheduleTimeDataGridViewTextBoxColumn,
            this.assignedToDataGridViewTextBoxColumn,
            this.assignedUserIdDataGridViewTextBoxColumn,
            this.departmentIdDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.checklistCloseDateDataGridViewTextBoxColumn,
            this.taskNameDataGridViewTextBoxColumn,
            this.validateTagDataGridViewTextBoxColumn,
            this.cardIdDataGridViewTextBoxColumn,
            this.cardNumberDataGridViewTextBoxColumn,
            this.taskCardNumberDataGridViewTextBoxColumn,
            this.remarksMandatoryDataGridViewTextBoxColumn,
            this.assetIdDataGridViewTextBoxColumn,
            this.assetNameDataGridViewTextBoxColumn,
            this.assetTypeDataGridViewTextBoxColumn,
            this.assetGroupNameDataGridViewTextBoxColumn,
            this.chklistValueDataGridViewTextBoxColumn,
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
            this.isActiveDataGridViewTextBoxColumn});
            this.adhocDataGridView.DataSource = this.maintenanceJobDTOBindingSource;
            this.adhocDataGridView.Location = new System.Drawing.Point(1, 19);
            this.adhocDataGridView.Name = "adhocDataGridView";
            this.adhocDataGridView.Size = new System.Drawing.Size(850, 277);
            this.adhocDataGridView.TabIndex = 3;
            this.adhocDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.adhocDataGridView_DataError);
            // 
            // maintenanceJobDTOBindingSource
            // 
            this.maintenanceJobDTOBindingSource.DataSource = typeof(Semnox.Parafait.Maintenance.UserJobItemsDTO);
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.txtAssignedTo);
            this.gpFilter.Controls.Add(this.txtName);
            this.gpFilter.Controls.Add(this.cmbAssetName);
            this.gpFilter.Controls.Add(this.cmbAssignedTo);
            this.gpFilter.Controls.Add(this.lblDays);
            this.gpFilter.Controls.Add(this.txtCompleted);
            this.gpFilter.Controls.Add(this.lblCompleted);
            this.gpFilter.Controls.Add(this.lblAssignedTo);
            this.gpFilter.Controls.Add(this.lblAssetName);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(5, 3);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(857, 57);
            this.gpFilter.TabIndex = 57;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Asset and Assignment";
            // 
            // txtAssignedTo
            // 
            this.txtAssignedTo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.txtAssignedTo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAssignedTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtAssignedTo.Location = new System.Drawing.Point(410, 25);
            this.txtAssignedTo.Name = "txtAssignedTo";
            this.txtAssignedTo.Size = new System.Drawing.Size(119, 14);
            this.txtAssignedTo.TabIndex = 59;
            this.txtAssignedTo.TextChanged += new System.EventHandler(this.txtAssignedTo_TextChanged);
            this.txtAssignedTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAssignedTo_KeyDown);
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(137, 27);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(119, 14);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // cmbAssetName
            // 
            this.cmbAssetName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssetName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbAssetName.FormattingEnabled = true;
            this.cmbAssetName.Location = new System.Drawing.Point(134, 22);
            this.cmbAssetName.Name = "cmbAssetName";
            this.cmbAssetName.Size = new System.Drawing.Size(141, 23);
            this.cmbAssetName.TabIndex = 30;
            this.cmbAssetName.SelectedValueChanged += new System.EventHandler(this.cmbAssetName_SelectedValueChanged);
            this.cmbAssetName.Click += new System.EventHandler(this.cmbAssetName_Click);
            // 
            // cmbAssignedTo
            // 
            this.cmbAssignedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssignedTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbAssignedTo.FormattingEnabled = true;
            this.cmbAssignedTo.Location = new System.Drawing.Point(406, 20);
            this.cmbAssignedTo.Name = "cmbAssignedTo";
            this.cmbAssignedTo.Size = new System.Drawing.Size(141, 23);
            this.cmbAssignedTo.TabIndex = 1;
            this.cmbAssignedTo.SelectedValueChanged += new System.EventHandler(this.cmbAssignedTo_SelectedValueChanged);
            this.cmbAssignedTo.Click += new System.EventHandler(this.cmbAssignedTo_Click);
            // 
            // lblDays
            // 
            this.lblDays.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblDays.Location = new System.Drawing.Point(740, 22);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(48, 20);
            this.lblDays.TabIndex = 24;
            this.lblDays.Text = "days";
            this.lblDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCompleted
            // 
            this.txtCompleted.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtCompleted.Location = new System.Drawing.Point(676, 22);
            this.txtCompleted.Name = "txtCompleted";
            this.txtCompleted.Size = new System.Drawing.Size(59, 21);
            this.txtCompleted.TabIndex = 2;
            // 
            // lblCompleted
            // 
            this.lblCompleted.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCompleted.Location = new System.Drawing.Point(554, 22);
            this.lblCompleted.Name = "lblCompleted";
            this.lblCompleted.Size = new System.Drawing.Size(116, 20);
            this.lblCompleted.TabIndex = 22;
            this.lblCompleted.Text = "Complete in:";
            this.lblCompleted.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAssignedTo
            // 
            this.lblAssignedTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssignedTo.Location = new System.Drawing.Point(284, 22);
            this.lblAssignedTo.Name = "lblAssignedTo";
            this.lblAssignedTo.Size = new System.Drawing.Size(116, 20);
            this.lblAssignedTo.TabIndex = 19;
            this.lblAssignedTo.Text = "Assigned To:*";
            this.lblAssignedTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAssetName
            // 
            this.lblAssetName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssetName.Location = new System.Drawing.Point(8, 22);
            this.lblAssetName.Name = "lblAssetName";
            this.lblAssetName.Size = new System.Drawing.Size(120, 20);
            this.lblAssetName.TabIndex = 13;
            this.lblAssetName.Text = "Asset Name:*";
            this.lblAssetName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvAssignedTo
            // 
            this.dgvAssignedTo.AllowUserToAddRows = false;
            this.dgvAssignedTo.AllowUserToDeleteRows = false;
            this.dgvAssignedTo.BackgroundColor = System.Drawing.Color.White;
            this.dgvAssignedTo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAssignedTo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvAssignedTo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssignedTo.ColumnHeadersVisible = false;
            this.dgvAssignedTo.Location = new System.Drawing.Point(413, 48);
            this.dgvAssignedTo.Name = "dgvAssignedTo";
            this.dgvAssignedTo.ReadOnly = true;
            this.dgvAssignedTo.RowHeadersVisible = false;
            this.dgvAssignedTo.RowTemplate.Height = 35;
            this.dgvAssignedTo.Size = new System.Drawing.Size(139, 181);
            this.dgvAssignedTo.TabIndex = 60;
            this.dgvAssignedTo.Visible = false;
            this.dgvAssignedTo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAssignedTo_CellClick);
            this.dgvAssignedTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvAssignedTo_KeyDown);
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
            this.dgvAssetNameSearch.Location = new System.Drawing.Point(140, 48);
            this.dgvAssetNameSearch.Name = "dgvAssetNameSearch";
            this.dgvAssetNameSearch.ReadOnly = true;
            this.dgvAssetNameSearch.RowHeadersVisible = false;
            this.dgvAssetNameSearch.RowTemplate.Height = 35;
            this.dgvAssetNameSearch.Size = new System.Drawing.Size(139, 181);
            this.dgvAssetNameSearch.TabIndex = 58;
            this.dgvAssetNameSearch.Visible = false;
            this.dgvAssetNameSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAssetNameSearch_CellClick);
            this.dgvAssetNameSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvAssetNameSearch_KeyDown);
            // 
            // gpTaskDetail
            // 
            this.gpTaskDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpTaskDetail.Controls.Add(this.adhocDataGridView);
            this.gpTaskDetail.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpTaskDetail.Location = new System.Drawing.Point(5, 70);
            this.gpTaskDetail.Name = "gpTaskDetail";
            this.gpTaskDetail.Size = new System.Drawing.Size(857, 302);
            this.gpTaskDetail.TabIndex = 61;
            this.gpTaskDetail.TabStop = false;
            this.gpTaskDetail.Text = "Task Details";
            // 
            // maintChklstdetIdDataGridViewTextBoxColumn
            // 
            this.maintChklstdetIdDataGridViewTextBoxColumn.DataPropertyName = "MaintChklstdetId";
            this.maintChklstdetIdDataGridViewTextBoxColumn.HeaderText = "Job ID";
            this.maintChklstdetIdDataGridViewTextBoxColumn.Name = "maintChklstdetIdDataGridViewTextBoxColumn";
            this.maintChklstdetIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // jobTaskIdDataGridViewTextBoxColumn
            // 
            this.jobTaskId.DataPropertyName = "JobTaskId";
            this.jobTaskId.HeaderText = "Job Task";
            this.jobTaskId.Name = "jobTaskId";
            this.jobTaskId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.jobTaskId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
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
            // validateTagDataGridViewTextBoxColumn
            // 
            this.validateTagDataGridViewTextBoxColumn.DataPropertyName = "ValidateTag";
            this.validateTagDataGridViewTextBoxColumn.FalseValue = "False";
            this.validateTagDataGridViewTextBoxColumn.HeaderText = "Validate Tag?";
            this.validateTagDataGridViewTextBoxColumn.Name = "validateTagDataGridViewTextBoxColumn";
            this.validateTagDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.validateTagDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.validateTagDataGridViewTextBoxColumn.TrueValue = "True";
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
            // remarksMandatoryDataGridViewTextBoxColumn
            // 
            this.remarksMandatoryDataGridViewTextBoxColumn.DataPropertyName = "RemarksMandatory";
            this.remarksMandatoryDataGridViewTextBoxColumn.FalseValue = "False";
            this.remarksMandatoryDataGridViewTextBoxColumn.HeaderText = "Remarks Mandatory?";
            this.remarksMandatoryDataGridViewTextBoxColumn.Name = "remarksMandatoryDataGridViewTextBoxColumn";
            this.remarksMandatoryDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.remarksMandatoryDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.remarksMandatoryDataGridViewTextBoxColumn.TrueValue = "True";
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
            // chklistValueDataGridViewTextBoxColumn
            // 
            this.chklistValueDataGridViewTextBoxColumn.DataPropertyName = "ChklistValue";
            this.chklistValueDataGridViewTextBoxColumn.FalseValue = "False";
            this.chklistValueDataGridViewTextBoxColumn.HeaderText = "Checked?";
            this.chklistValueDataGridViewTextBoxColumn.Name = "chklistValueDataGridViewTextBoxColumn";
            this.chklistValueDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chklistValueDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chklistValueDataGridViewTextBoxColumn.TrueValue = "True";
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
            // isActiveDataGridViewTextBoxColumn
            // 
            this.isActiveDataGridViewTextBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn.FalseValue = "False";
            this.isActiveDataGridViewTextBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewTextBoxColumn.Name = "isActiveDataGridViewTextBoxColumn";
            this.isActiveDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isActiveDataGridViewTextBoxColumn.TrueValue = "True";
            // 
            // AdhocJobUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 429);
            this.Controls.Add(this.dgvAssignedTo);
            this.Controls.Add(this.dgvAssetNameSearch);
            this.Controls.Add(this.gpTaskDetail);
            this.Controls.Add(this.adhocDeleteBtn);
            this.Controls.Add(this.adhocCloseBtn);
            this.Controls.Add(this.adhocRefreshBtn);
            this.Controls.Add(this.adhocSaveBtn);
            this.Controls.Add(this.gpFilter);
            this.Name = "AdhocJobUI";
            this.Text = "Create Adhoc Job";
            ((System.ComponentModel.ISupportInitialize)(this.adhocDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceJobDTOBindingSource)).EndInit();
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignedTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssetNameSearch)).EndInit();
            this.gpTaskDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button adhocDeleteBtn;
        private System.Windows.Forms.Button adhocCloseBtn;
        private System.Windows.Forms.Button adhocRefreshBtn;
        private System.Windows.Forms.Button adhocSaveBtn;
        private System.Windows.Forms.DataGridView adhocDataGridView;
        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.ComboBox cmbAssignedTo;
        private System.Windows.Forms.Label lblDays;
        private System.Windows.Forms.TextBox txtCompleted;
        private System.Windows.Forms.Label lblCompleted;
        private System.Windows.Forms.Label lblAssignedTo;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblAssetName;
        private System.Windows.Forms.ComboBox cmbAssetName;
        private System.Windows.Forms.DataGridView dgvAssetNameSearch;
        private System.Windows.Forms.DataGridViewComboBoxColumn jobTaskId;
        private System.Windows.Forms.BindingSource maintenanceJobDTOBindingSource;
        private System.Windows.Forms.DataGridView dgvAssignedTo;
        private System.Windows.Forms.TextBox txtAssignedTo;
        private System.Windows.Forms.GroupBox gpTaskDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn maintChklstdetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maintJobNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chklstScheduleTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assignedToDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assignedUserIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn departmentIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn checklistCloseDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn validateTagDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskCardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn remarksMandatoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetGroupNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chklistValueDataGridViewTextBoxColumn;
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
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn;
    }
}