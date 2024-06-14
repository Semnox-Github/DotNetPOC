namespace Semnox.Parafait.JobUtils
{
    partial class ConcurrentProgramUI
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
            this.concurrentProgramsDataGridView = new System.Windows.Forms.DataGridView();
            this.programIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.programNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExecutionMethodDataGridViewComboBox = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ExecutableNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.keepRunningDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.errorNotificationMailIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.successNotificationMailIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executionMethodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executableNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedUserDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArgumentsDataGridButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ScheduleDatagridButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.concurrentProgramsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtProgramName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.searchByProgramsParametersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.concurrentProgramSchedulesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramsDTOBindingSource)).BeginInit();
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchByProgramsParametersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramSchedulesDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // concurrentProgramsDataGridView
            // 
            this.concurrentProgramsDataGridView.AllowUserToDeleteRows = false;
            this.concurrentProgramsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.concurrentProgramsDataGridView.AutoGenerateColumns = false;
            this.concurrentProgramsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.concurrentProgramsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.programIdDataGridViewTextBoxColumn,
            this.programNameDataGridViewTextBoxColumn,
            this.ExecutionMethodDataGridViewComboBox,
            this.ExecutableNameDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.keepRunningDataGridViewCheckBoxColumn,
            this.errorNotificationMailIdDataGridViewTextBoxColumn,
            this.successNotificationMailIdDataGridViewTextBoxColumn,
            this.executionMethodDataGridViewTextBoxColumn,
            this.executableNameDataGridViewTextBoxColumn1,
            this.lastUpdatedDateDataGridViewTextBoxColumn,
            this.lastUpdatedUserDataGridViewTextBoxColumn,
            this.ArgumentsDataGridButton,
            this.ScheduleDatagridButton});
            this.concurrentProgramsDataGridView.DataSource = this.concurrentProgramsDTOBindingSource;
            this.concurrentProgramsDataGridView.EnableHeadersVisualStyles = false;
            this.concurrentProgramsDataGridView.Location = new System.Drawing.Point(12, 70);
            this.concurrentProgramsDataGridView.MultiSelect = false;
            this.concurrentProgramsDataGridView.Name = "concurrentProgramsDataGridView";
            this.concurrentProgramsDataGridView.ShowCellErrors = false;
            this.concurrentProgramsDataGridView.Size = new System.Drawing.Size(1069, 405);
            this.concurrentProgramsDataGridView.TabIndex = 0;
            this.concurrentProgramsDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.concurrentProgramsDataGridView_CellContentClick);
            this.concurrentProgramsDataGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.concurrentProgramsDataGridView_CellEnter);
            this.concurrentProgramsDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.concurrentProgramsDataGridView_DataError);
            this.concurrentProgramsDataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.concurrentProgramsDataGridView_DefaultValuesNeeded);
            // 
            // programIdDataGridViewTextBoxColumn
            // 
            this.programIdDataGridViewTextBoxColumn.DataPropertyName = "ProgramId";
            this.programIdDataGridViewTextBoxColumn.HeaderText = "Program Id";
            this.programIdDataGridViewTextBoxColumn.Name = "programIdDataGridViewTextBoxColumn";
            this.programIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // programNameDataGridViewTextBoxColumn
            // 
            this.programNameDataGridViewTextBoxColumn.DataPropertyName = "ProgramName";
            this.programNameDataGridViewTextBoxColumn.HeaderText = "Program Name";
            this.programNameDataGridViewTextBoxColumn.Name = "programNameDataGridViewTextBoxColumn";
            // 
            // ExecutionMethodDataGridViewComboBox
            // 
            this.ExecutionMethodDataGridViewComboBox.DataPropertyName = "ExecutionMethod";
            this.ExecutionMethodDataGridViewComboBox.HeaderText = "Execution Method";
            this.ExecutionMethodDataGridViewComboBox.Name = "ExecutionMethodDataGridViewComboBox";
            this.ExecutionMethodDataGridViewComboBox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ExecutableNameDataGridViewTextBoxColumn
            // 
            this.ExecutableNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ExecutableNameDataGridViewTextBoxColumn.DataPropertyName = "ExecutableName";
            this.ExecutableNameDataGridViewTextBoxColumn.HeaderText = "Executable Name";
            this.ExecutableNameDataGridViewTextBoxColumn.Name = "ExecutableNameDataGridViewTextBoxColumn";
            this.ExecutableNameDataGridViewTextBoxColumn.Width = 106;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // keepRunningDataGridViewCheckBoxColumn
            // 
            this.keepRunningDataGridViewCheckBoxColumn.DataPropertyName = "KeepRunning";
            this.keepRunningDataGridViewCheckBoxColumn.HeaderText = "Keep Running";
            this.keepRunningDataGridViewCheckBoxColumn.Name = "keepRunningDataGridViewCheckBoxColumn";
            // 
            // errorNotificationMailIdDataGridViewTextBoxColumn
            // 
            this.errorNotificationMailIdDataGridViewTextBoxColumn.DataPropertyName = "ErrorNotificationMailId";
            this.errorNotificationMailIdDataGridViewTextBoxColumn.HeaderText = "Error Notification Email Id";
            this.errorNotificationMailIdDataGridViewTextBoxColumn.Name = "errorNotificationMailIdDataGridViewTextBoxColumn";
            // 
            // successNotificationMailIdDataGridViewTextBoxColumn
            // 
            this.successNotificationMailIdDataGridViewTextBoxColumn.DataPropertyName = "SuccessNotificationMailId";
            this.successNotificationMailIdDataGridViewTextBoxColumn.HeaderText = "Success Notification Email Id";
            this.successNotificationMailIdDataGridViewTextBoxColumn.Name = "successNotificationMailIdDataGridViewTextBoxColumn";
            // 
            // executionMethodDataGridViewTextBoxColumn
            // 
            this.executionMethodDataGridViewTextBoxColumn.DataPropertyName = "ExecutionMethod";
            this.executionMethodDataGridViewTextBoxColumn.HeaderText = "Execution Method";
            this.executionMethodDataGridViewTextBoxColumn.Name = "executionMethodDataGridViewTextBoxColumn";
            this.executionMethodDataGridViewTextBoxColumn.Visible = false;
            // 
            // executableNameDataGridViewTextBoxColumn1
            // 
            this.executableNameDataGridViewTextBoxColumn1.DataPropertyName = "ExecutableName";
            this.executableNameDataGridViewTextBoxColumn1.HeaderText = "Executable Name";
            this.executableNameDataGridViewTextBoxColumn1.Name = "executableNameDataGridViewTextBoxColumn1";
            this.executableNameDataGridViewTextBoxColumn1.Visible = false;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn.HeaderText = "Last Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn.Name = "lastUpdatedDateDataGridViewTextBoxColumn";
            this.lastUpdatedDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedUserDataGridViewTextBoxColumn
            // 
            this.lastUpdatedUserDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedUser";
            this.lastUpdatedUserDataGridViewTextBoxColumn.HeaderText = "Last Updated User";
            this.lastUpdatedUserDataGridViewTextBoxColumn.Name = "lastUpdatedUserDataGridViewTextBoxColumn";
            this.lastUpdatedUserDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ArgumentsDataGridButton
            // 
            this.ArgumentsDataGridButton.HeaderText = "Arguments";
            this.ArgumentsDataGridButton.Name = "ArgumentsDataGridButton";
            this.ArgumentsDataGridButton.Text = "...";
            this.ArgumentsDataGridButton.UseColumnTextForButtonValue = true;
            this.ArgumentsDataGridButton.Width = 70;
            // 
            // ScheduleDatagridButton
            // 
            this.ScheduleDatagridButton.HeaderText = "Schedule";
            this.ScheduleDatagridButton.Name = "ScheduleDatagridButton";
            this.ScheduleDatagridButton.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ScheduleDatagridButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ScheduleDatagridButton.Text = "...";
            this.ScheduleDatagridButton.UseColumnTextForButtonValue = true;
            // 
            // concurrentProgramsDTOBindingSource
            // 
            this.concurrentProgramsDTOBindingSource.DataSource = typeof(Semnox.Parafait.JobUtils.ConcurrentProgramsDTO);
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.txtProgramName);
            this.gpFilter.Controls.Add(this.lblName);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(10, 11);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(1071, 45);
            this.gpFilter.TabIndex = 16;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(411, 14);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtProgramName
            // 
            this.txtProgramName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtProgramName.Location = new System.Drawing.Point(269, 16);
            this.txtProgramName.Name = "txtProgramName";
            this.txtProgramName.Size = new System.Drawing.Size(136, 21);
            this.txtProgramName.TabIndex = 2;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(154, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(109, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Program Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(12, 18);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 0;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            this.chbShowActiveEntries.CheckedChanged += new System.EventHandler(this.chbShowActiveEntries_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(263, 493);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(134, 493);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 18;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(10, 493);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // searchByProgramsParametersBindingSource
            // 
            this.searchByProgramsParametersBindingSource.DataSource = typeof(Semnox.Parafait.JobUtils.ConcurrentProgramsDTO.SearchByProgramsParameters);
            // 
            // concurrentProgramSchedulesDTOBindingSource
            // 
            this.concurrentProgramSchedulesDTOBindingSource.DataSource = typeof(Semnox.Parafait.JobUtils.ConcurrentProgramSchedulesDTO);
            // 
            // ConcurrentProgramUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1093, 528);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gpFilter);
            this.Controls.Add(this.concurrentProgramsDataGridView);
            this.KeyPreview = true;
            this.Name = "ConcurrentProgramUI";
            this.Text = "Concurrent Programs";
            this.Load += new System.EventHandler(this.ConcurrentProgramUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramsDTOBindingSource)).EndInit();
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchByProgramsParametersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramSchedulesDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView concurrentProgramsDataGridView;
        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtProgramName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.BindingSource searchByProgramsParametersBindingSource;
        private System.Windows.Forms.BindingSource concurrentProgramsDTOBindingSource;
        private System.Windows.Forms.BindingSource concurrentProgramSchedulesDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn programIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn programNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ExecutionMethodDataGridViewComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExecutableNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn keepRunningDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn errorNotificationMailIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn successNotificationMailIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn executionMethodDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn executableNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedUserDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn ArgumentsDataGridButton;
        private System.Windows.Forms.DataGridViewButtonColumn ScheduleDatagridButton;
    }
}