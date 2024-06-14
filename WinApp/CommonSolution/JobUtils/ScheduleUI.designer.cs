namespace Semnox.Parafait.JobUtils
{
    partial class ScheduleUI
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
            this.concurrentProgramsScheduleGridView = new System.Windows.Forms.DataGridView();
            this.programScheduleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.programIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.runAtDataGridViewComboBox = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.frequencyDataGridViewComboBox = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.endDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lastExecutedOnDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.concurrentProgramSchedulesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchByProgramSceduleParametersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gpSchedule = new System.Windows.Forms.GroupBox();
            this.lblProgramName = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.chbShowActiveSchedules = new System.Windows.Forms.CheckBox();
            this.gpFilter = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramsScheduleGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramSchedulesDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchByProgramSceduleParametersBindingSource)).BeginInit();
            this.gpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // concurrentProgramsScheduleGridView
            // 
            this.concurrentProgramsScheduleGridView.AllowUserToDeleteRows = false;
            this.concurrentProgramsScheduleGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.concurrentProgramsScheduleGridView.AutoGenerateColumns = false;
            this.concurrentProgramsScheduleGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.concurrentProgramsScheduleGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.programScheduleIdDataGridViewTextBoxColumn,
            this.programIdDataGridViewTextBoxColumn,
            this.startDateDataGridViewTextBoxColumn,
            this.runAtDataGridViewComboBox,
            this.frequencyDataGridViewComboBox,
            this.endDateDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.lastExecutedOnDataGridViewTextBoxColumn});
            this.concurrentProgramsScheduleGridView.DataSource = this.concurrentProgramSchedulesDTOBindingSource;
            this.concurrentProgramsScheduleGridView.Location = new System.Drawing.Point(19, 73);
            this.concurrentProgramsScheduleGridView.Name = "concurrentProgramsScheduleGridView";
            this.concurrentProgramsScheduleGridView.Size = new System.Drawing.Size(744, 234);
            this.concurrentProgramsScheduleGridView.TabIndex = 21;
            this.concurrentProgramsScheduleGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.concurrentProgramsScheduleGridView_DataError);
            this.concurrentProgramsScheduleGridView.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.concurrentProgramsScheduleGridView_RowValidating);
            // 
            // programScheduleIdDataGridViewTextBoxColumn
            // 
            this.programScheduleIdDataGridViewTextBoxColumn.DataPropertyName = "ProgramScheduleId";
            this.programScheduleIdDataGridViewTextBoxColumn.HeaderText = "Schedule Id";
            this.programScheduleIdDataGridViewTextBoxColumn.Name = "programScheduleIdDataGridViewTextBoxColumn";
            this.programScheduleIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // programIdDataGridViewTextBoxColumn
            // 
            this.programIdDataGridViewTextBoxColumn.DataPropertyName = "ProgramId";
            this.programIdDataGridViewTextBoxColumn.HeaderText = "Program Id";
            this.programIdDataGridViewTextBoxColumn.Name = "programIdDataGridViewTextBoxColumn";
            this.programIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // startDateDataGridViewTextBoxColumn
            // 
            this.startDateDataGridViewTextBoxColumn.DataPropertyName = "StartDate";
            this.startDateDataGridViewTextBoxColumn.HeaderText = "Start Date";
            this.startDateDataGridViewTextBoxColumn.Name = "startDateDataGridViewTextBoxColumn";
            // 
            // runAtDataGridViewComboBox
            // 
            this.runAtDataGridViewComboBox.DataPropertyName = "RunAt";
            this.runAtDataGridViewComboBox.HeaderText = "RunAt";
            this.runAtDataGridViewComboBox.Name = "runAtDataGridViewComboBox";
            this.runAtDataGridViewComboBox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.runAtDataGridViewComboBox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // frequencyDataGridViewComboBox
            // 
            this.frequencyDataGridViewComboBox.DataPropertyName = "Frequency";
            this.frequencyDataGridViewComboBox.HeaderText = "Frequency";
            this.frequencyDataGridViewComboBox.Name = "frequencyDataGridViewComboBox";
            this.frequencyDataGridViewComboBox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.frequencyDataGridViewComboBox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // endDateDataGridViewTextBoxColumn
            // 
            this.endDateDataGridViewTextBoxColumn.DataPropertyName = "EndDate";
            this.endDateDataGridViewTextBoxColumn.HeaderText = "End Date";
            this.endDateDataGridViewTextBoxColumn.Name = "endDateDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // lastExecutedOnDataGridViewTextBoxColumn
            // 
            this.lastExecutedOnDataGridViewTextBoxColumn.DataPropertyName = "LastExecutedOn";
            this.lastExecutedOnDataGridViewTextBoxColumn.HeaderText = "Last Executed On";
            this.lastExecutedOnDataGridViewTextBoxColumn.Name = "lastExecutedOnDataGridViewTextBoxColumn";
            this.lastExecutedOnDataGridViewTextBoxColumn.Visible = false;
            // 
            // concurrentProgramSchedulesDTOBindingSource
            // 
            this.concurrentProgramSchedulesDTOBindingSource.DataSource = typeof(Semnox.Parafait.JobUtils.ConcurrentProgramSchedulesDTO);
            // 
            // searchByProgramSceduleParametersBindingSource
            // 
            this.searchByProgramSceduleParametersBindingSource.DataSource = typeof(Semnox.Parafait.JobUtils.ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters);
            // 
            // gpSchedule
            // 
            this.gpSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpSchedule.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.gpSchedule.Location = new System.Drawing.Point(7, 52);
            this.gpSchedule.Name = "gpSchedule";
            this.gpSchedule.Size = new System.Drawing.Size(780, 264);
            this.gpSchedule.TabIndex = 22;
            this.gpSchedule.TabStop = false;
            this.gpSchedule.Text = "Schedule";
            // 
            // lblProgramName
            // 
            this.lblProgramName.AutoSize = true;
            this.lblProgramName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblProgramName.Location = new System.Drawing.Point(177, 16);
            this.lblProgramName.Name = "lblProgramName";
            this.lblProgramName.Size = new System.Drawing.Size(99, 15);
            this.lblProgramName.TabIndex = 26;
            this.lblProgramName.Text = "Program Name :";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(302, 333);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 23);
            this.btnRefresh.TabIndex = 27;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(142, 332);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(467, 333);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 23);
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chbShowActiveSchedules
            // 
            this.chbShowActiveSchedules.AutoSize = true;
            this.chbShowActiveSchedules.Checked = true;
            this.chbShowActiveSchedules.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveSchedules.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveSchedules.Location = new System.Drawing.Point(12, 16);
            this.chbShowActiveSchedules.Name = "chbShowActiveSchedules";
            this.chbShowActiveSchedules.Size = new System.Drawing.Size(159, 19);
            this.chbShowActiveSchedules.TabIndex = 30;
            this.chbShowActiveSchedules.Text = "Show Active Schedules";
            this.chbShowActiveSchedules.UseVisualStyleBackColor = true;
            this.chbShowActiveSchedules.CheckedChanged += new System.EventHandler(this.chbShowActiveSchedules_CheckedChanged);
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.chbShowActiveSchedules);
            this.gpFilter.Controls.Add(this.lblProgramName);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(7, 8);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(780, 38);
            this.gpFilter.TabIndex = 31;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // ScheduleUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 368);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.concurrentProgramsScheduleGridView);
            this.Controls.Add(this.gpSchedule);
            this.Controls.Add(this.gpFilter);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScheduleUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Program Schedules";
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramsScheduleGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.concurrentProgramSchedulesDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchByProgramSceduleParametersBindingSource)).EndInit();
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView concurrentProgramsScheduleGridView;
        private System.Windows.Forms.BindingSource searchByProgramSceduleParametersBindingSource;
        private System.Windows.Forms.BindingSource concurrentProgramSchedulesDTOBindingSource;
        private System.Windows.Forms.GroupBox gpSchedule;
        private System.Windows.Forms.Label lblProgramName;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chbShowActiveSchedules;
        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.DataGridViewTextBoxColumn programScheduleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn programIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn runAtDataGridViewComboBox;
        private System.Windows.Forms.DataGridViewComboBoxColumn frequencyDataGridViewComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn endDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastExecutedOnDataGridViewTextBoxColumn;
    }
}