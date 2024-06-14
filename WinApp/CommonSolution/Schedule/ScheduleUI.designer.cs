namespace Semnox.Parafait.Schedule
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
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.cmbAssignedTo = new System.Windows.Forms.ComboBox();
            this.lblDays = new System.Windows.Forms.Label();
            this.txtCompleted = new System.Windows.Forms.TextBox();
            this.lblCompleted = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblAssignedTo = new System.Windows.Forms.Label();
            this.chbActive = new System.Windows.Forms.CheckBox();
            this.gprecur = new System.Windows.Forms.GroupBox();
            this.gpRecurType = new System.Windows.Forms.GroupBox();
            this.rbRecurTypeWeekly = new System.Windows.Forms.RadioButton();
            this.rbRecurTypeDaily = new System.Windows.Forms.RadioButton();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.rbMonthly = new System.Windows.Forms.RadioButton();
            this.rbWeekly = new System.Windows.Forms.RadioButton();
            this.rbDaily = new System.Windows.Forms.RadioButton();
            this.chbRecur = new System.Windows.Forms.CheckBox();
            this.cmbTime = new System.Windows.Forms.ComboBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblAssetName = new System.Windows.Forms.Label();
            this.lblScheduleTime = new System.Windows.Forms.Label();
            this.dtscheduleDate = new System.Windows.Forms.DateTimePicker();
            this.scheduleDataGridView = new System.Windows.Forms.DataGridView();
            this.scheduleAssetTaskDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.scheduleDeleteBtn = new System.Windows.Forms.Button();
            this.scheduleCloseBtn = new System.Windows.Forms.Button();
            this.scheduleRefreshBtn = new System.Windows.Forms.Button();
            this.scheduleSaveBtn = new System.Windows.Forms.Button();
            this.exclusionDaysbtn = new System.Windows.Forms.Button();
            this.btnPublishToSite = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.maintSchAssetTaskIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maintScheduleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetGroupIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.assetTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.assetIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.maintTaskGroupIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.maintTaskIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gpFilter.SuspendLayout();
            this.gprecur.SuspendLayout();
            this.gpRecurType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleAssetTaskDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.cmbAssignedTo);
            this.gpFilter.Controls.Add(this.lblDays);
            this.gpFilter.Controls.Add(this.txtCompleted);
            this.gpFilter.Controls.Add(this.lblCompleted);
            this.gpFilter.Controls.Add(this.lblTime);
            this.gpFilter.Controls.Add(this.lblAssignedTo);
            this.gpFilter.Controls.Add(this.chbActive);
            this.gpFilter.Controls.Add(this.gprecur);
            this.gpFilter.Controls.Add(this.cmbTime);
            this.gpFilter.Controls.Add(this.txtName);
            this.gpFilter.Controls.Add(this.lblAssetName);
            this.gpFilter.Controls.Add(this.lblScheduleTime);
            this.gpFilter.Controls.Add(this.dtscheduleDate);
            this.gpFilter.Location = new System.Drawing.Point(12, 3);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(1131, 148);
            this.gpFilter.TabIndex = 19;
            this.gpFilter.TabStop = false;
            // 
            // cmbAssignedTo
            // 
            this.cmbAssignedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssignedTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbAssignedTo.FormattingEnabled = true;
            this.cmbAssignedTo.Location = new System.Drawing.Point(134, 111);
            this.cmbAssignedTo.Name = "cmbAssignedTo";
            this.cmbAssignedTo.Size = new System.Drawing.Size(186, 23);
            this.cmbAssignedTo.TabIndex = 25;
            // 
            // lblDays
            // 
            this.lblDays.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblDays.Location = new System.Drawing.Point(472, 110);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(56, 20);
            this.lblDays.TabIndex = 24;
            this.lblDays.Text = "days";
            this.lblDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCompleted
            // 
            this.txtCompleted.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtCompleted.Location = new System.Drawing.Point(407, 110);
            this.txtCompleted.Name = "txtCompleted";
            this.txtCompleted.Size = new System.Drawing.Size(59, 21);
            this.txtCompleted.TabIndex = 23;
            // 
            // lblCompleted
            // 
            this.lblCompleted.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCompleted.Location = new System.Drawing.Point(320, 110);
            this.lblCompleted.Name = "lblCompleted";
            this.lblCompleted.Size = new System.Drawing.Size(81, 20);
            this.lblCompleted.TabIndex = 22;
            this.lblCompleted.Text = "Complete in:";
            this.lblCompleted.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTime
            // 
            this.lblTime.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(320, 70);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(81, 15);
            this.lblTime.TabIndex = 21;
            this.lblTime.Text = "Time:";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAssignedTo
            // 
            this.lblAssignedTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssignedTo.Location = new System.Drawing.Point(12, 110);
            this.lblAssignedTo.Name = "lblAssignedTo";
            this.lblAssignedTo.Size = new System.Drawing.Size(116, 20);
            this.lblAssignedTo.TabIndex = 19;
            this.lblAssignedTo.Text = "Assigned To:";
            this.lblAssignedTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbActive
            // 
            this.chbActive.Checked = true;
            this.chbActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbActive.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbActive.Location = new System.Drawing.Point(407, 22);
            this.chbActive.Name = "chbActive";
            this.chbActive.Size = new System.Drawing.Size(106, 24);
            this.chbActive.TabIndex = 18;
            this.chbActive.Text = "Active";
            this.chbActive.UseVisualStyleBackColor = true;
            // 
            // gprecur
            // 
            this.gprecur.Controls.Add(this.gpRecurType);
            this.gprecur.Controls.Add(this.lblEndDate);
            this.gprecur.Controls.Add(this.dtpEndDate);
            this.gprecur.Controls.Add(this.rbMonthly);
            this.gprecur.Controls.Add(this.rbWeekly);
            this.gprecur.Controls.Add(this.rbDaily);
            this.gprecur.Controls.Add(this.chbRecur);
            this.gprecur.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gprecur.Location = new System.Drawing.Point(519, 9);
            this.gprecur.Name = "gprecur";
            this.gprecur.Size = new System.Drawing.Size(569, 103);
            this.gprecur.TabIndex = 17;
            this.gprecur.TabStop = false;
            this.gprecur.Text = "Recurrence";
            // 
            // gpRecurType
            // 
            this.gpRecurType.Controls.Add(this.rbRecurTypeWeekly);
            this.gpRecurType.Controls.Add(this.rbRecurTypeDaily);
            this.gpRecurType.Enabled = false;
            this.gpRecurType.Location = new System.Drawing.Point(344, 53);
            this.gpRecurType.Name = "gpRecurType";
            this.gpRecurType.Size = new System.Drawing.Size(214, 44);
            this.gpRecurType.TabIndex = 11;
            this.gpRecurType.TabStop = false;
            this.gpRecurType.Text = "Recurrence Type";
            // 
            // rbRecurTypeWeekly
            // 
            this.rbRecurTypeWeekly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbRecurTypeWeekly.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rbRecurTypeWeekly.Location = new System.Drawing.Point(109, 19);
            this.rbRecurTypeWeekly.Name = "rbRecurTypeWeekly";
            this.rbRecurTypeWeekly.Size = new System.Drawing.Size(97, 24);
            this.rbRecurTypeWeekly.TabIndex = 12;
            this.rbRecurTypeWeekly.TabStop = true;
            this.rbRecurTypeWeekly.Text = "Week Day";
            this.rbRecurTypeWeekly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbRecurTypeWeekly.UseVisualStyleBackColor = true;
            // 
            // rbRecurTypeDaily
            // 
            this.rbRecurTypeDaily.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbRecurTypeDaily.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rbRecurTypeDaily.Location = new System.Drawing.Point(9, 20);
            this.rbRecurTypeDaily.Name = "rbRecurTypeDaily";
            this.rbRecurTypeDaily.Size = new System.Drawing.Size(91, 23);
            this.rbRecurTypeDaily.TabIndex = 11;
            this.rbRecurTypeDaily.TabStop = true;
            this.rbRecurTypeDaily.Text = "Day";
            this.rbRecurTypeDaily.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbRecurTypeDaily.UseVisualStyleBackColor = true;
            // 
            // lblEndDate
            // 
            this.lblEndDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndDate.Location = new System.Drawing.Point(6, 64);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(120, 18);
            this.lblEndDate.TabIndex = 10;
            this.lblEndDate.Text = "End Date:";
            this.lblEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.dtpEndDate.Enabled = false;
            this.dtpEndDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(129, 61);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(197, 21);
            this.dtpEndDate.TabIndex = 9;
            // 
            // rbMonthly
            // 
            this.rbMonthly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbMonthly.Enabled = false;
            this.rbMonthly.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rbMonthly.Location = new System.Drawing.Point(448, 23);
            this.rbMonthly.Name = "rbMonthly";
            this.rbMonthly.Size = new System.Drawing.Size(104, 24);
            this.rbMonthly.TabIndex = 3;
            this.rbMonthly.TabStop = true;
            this.rbMonthly.Text = "Monthly";
            this.rbMonthly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbMonthly.UseVisualStyleBackColor = true;
            this.rbMonthly.CheckedChanged += new System.EventHandler(this.rbMonthly_CheckedChanged);
            // 
            // rbWeekly
            // 
            this.rbWeekly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbWeekly.Enabled = false;
            this.rbWeekly.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rbWeekly.Location = new System.Drawing.Point(335, 23);
            this.rbWeekly.Name = "rbWeekly";
            this.rbWeekly.Size = new System.Drawing.Size(104, 24);
            this.rbWeekly.TabIndex = 2;
            this.rbWeekly.TabStop = true;
            this.rbWeekly.Text = "Weekly";
            this.rbWeekly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbWeekly.UseVisualStyleBackColor = true;
            // 
            // rbDaily
            // 
            this.rbDaily.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbDaily.Enabled = false;
            this.rbDaily.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rbDaily.Location = new System.Drawing.Point(222, 23);
            this.rbDaily.Name = "rbDaily";
            this.rbDaily.Size = new System.Drawing.Size(104, 24);
            this.rbDaily.TabIndex = 1;
            this.rbDaily.TabStop = true;
            this.rbDaily.Text = "Daily";
            this.rbDaily.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbDaily.UseVisualStyleBackColor = true;
            // 
            // chbRecur
            // 
            this.chbRecur.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbRecur.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbRecur.Location = new System.Drawing.Point(13, 23);
            this.chbRecur.Name = "chbRecur";
            this.chbRecur.Size = new System.Drawing.Size(132, 24);
            this.chbRecur.TabIndex = 0;
            this.chbRecur.Text = "Recur Flag";
            this.chbRecur.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbRecur.UseVisualStyleBackColor = true;
            this.chbRecur.CheckedChanged += new System.EventHandler(this.chbRecur_CheckedChanged);
            // 
            // cmbTime
            // 
            this.cmbTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTime.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbTime.FormattingEnabled = true;
            this.cmbTime.Location = new System.Drawing.Point(407, 67);
            this.cmbTime.Name = "cmbTime";
            this.cmbTime.Size = new System.Drawing.Size(106, 23);
            this.cmbTime.TabIndex = 16;
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(134, 22);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(186, 21);
            this.txtName.TabIndex = 14;
            // 
            // lblAssetName
            // 
            this.lblAssetName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssetName.Location = new System.Drawing.Point(12, 22);
            this.lblAssetName.Name = "lblAssetName";
            this.lblAssetName.Size = new System.Drawing.Size(116, 20);
            this.lblAssetName.TabIndex = 13;
            this.lblAssetName.Text = "Schedule Name:";
            this.lblAssetName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblScheduleTime
            // 
            this.lblScheduleTime.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScheduleTime.Location = new System.Drawing.Point(12, 70);
            this.lblScheduleTime.Name = "lblScheduleTime";
            this.lblScheduleTime.Size = new System.Drawing.Size(116, 15);
            this.lblScheduleTime.TabIndex = 8;
            this.lblScheduleTime.Text = "Schedule Date:";
            this.lblScheduleTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtscheduleDate
            // 
            this.dtscheduleDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.dtscheduleDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtscheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtscheduleDate.Location = new System.Drawing.Point(134, 67);
            this.dtscheduleDate.Name = "dtscheduleDate";
            this.dtscheduleDate.Size = new System.Drawing.Size(186, 21);
            this.dtscheduleDate.TabIndex = 7;
            // 
            // scheduleDataGridView
            // 
            this.scheduleDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scheduleDataGridView.AutoGenerateColumns = false;
            this.scheduleDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scheduleDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.maintSchAssetTaskIdDataGridViewTextBoxColumn,
            this.maintScheduleIdDataGridViewTextBoxColumn,
            this.assetGroupIdDataGridViewTextBoxColumn,
            this.assetTypeIdDataGridViewTextBoxColumn,
            this.assetIDDataGridViewTextBoxColumn,
            this.maintTaskGroupIdDataGridViewTextBoxColumn,
            this.maintTaskIdDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.scheduleDataGridView.DataSource = this.scheduleAssetTaskDTOBindingSource;
            this.scheduleDataGridView.Location = new System.Drawing.Point(12, 157);
            this.scheduleDataGridView.Name = "scheduleDataGridView";
            this.scheduleDataGridView.Size = new System.Drawing.Size(1131, 160);
            this.scheduleDataGridView.TabIndex = 45;
            this.scheduleDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.scheduleDataGridView_CellEndEdit);
            this.scheduleDataGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.scheduleDataGridView_CellEnter);
            this.scheduleDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.scheduleDataGridView_DataError);
            this.scheduleDataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.scheduleDataGridView_DefaultValuesNeeded);
            // 
            // scheduleAssetTaskDTOBindingSource
            // 
            this.scheduleAssetTaskDTOBindingSource.DataSource = typeof(Semnox.Core.GenericUtilities.JobScheduleTasksDTO);
            // 
            // scheduleDeleteBtn
            // 
            this.scheduleDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scheduleDeleteBtn.Enabled = false;
            this.scheduleDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.scheduleDeleteBtn.Location = new System.Drawing.Point(269, 345);
            this.scheduleDeleteBtn.Name = "scheduleDeleteBtn";
            this.scheduleDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.scheduleDeleteBtn.TabIndex = 52;
            this.scheduleDeleteBtn.Text = "Delete";
            this.scheduleDeleteBtn.UseVisualStyleBackColor = true;
            this.scheduleDeleteBtn.Click += new System.EventHandler(this.scheduleDeleteBtn_Click);
            // 
            // scheduleCloseBtn
            // 
            this.scheduleCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scheduleCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.scheduleCloseBtn.Location = new System.Drawing.Point(397, 345);
            this.scheduleCloseBtn.Name = "scheduleCloseBtn";
            this.scheduleCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.scheduleCloseBtn.TabIndex = 51;
            this.scheduleCloseBtn.Text = "Close";
            this.scheduleCloseBtn.UseVisualStyleBackColor = true;
            this.scheduleCloseBtn.Click += new System.EventHandler(this.scheduleCloseBtn_Click);
            // 
            // scheduleRefreshBtn
            // 
            this.scheduleRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scheduleRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.scheduleRefreshBtn.Location = new System.Drawing.Point(141, 345);
            this.scheduleRefreshBtn.Name = "scheduleRefreshBtn";
            this.scheduleRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.scheduleRefreshBtn.TabIndex = 50;
            this.scheduleRefreshBtn.Text = "Refresh";
            this.scheduleRefreshBtn.UseVisualStyleBackColor = true;
            this.scheduleRefreshBtn.Click += new System.EventHandler(this.scheduleRefreshBtn_Click);
            // 
            // scheduleSaveBtn
            // 
            this.scheduleSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scheduleSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.scheduleSaveBtn.Location = new System.Drawing.Point(13, 345);
            this.scheduleSaveBtn.Name = "scheduleSaveBtn";
            this.scheduleSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.scheduleSaveBtn.TabIndex = 49;
            this.scheduleSaveBtn.Text = "Save";
            this.scheduleSaveBtn.UseVisualStyleBackColor = true;
            this.scheduleSaveBtn.Click += new System.EventHandler(this.scheduleSaveBtn_Click);
            // 
            // exclusionDaysbtn
            // 
            this.exclusionDaysbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exclusionDaysbtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.exclusionDaysbtn.Location = new System.Drawing.Point(525, 345);
            this.exclusionDaysbtn.Name = "exclusionDaysbtn";
            this.exclusionDaysbtn.Size = new System.Drawing.Size(107, 23);
            this.exclusionDaysbtn.TabIndex = 53;
            this.exclusionDaysbtn.Text = "Incl/Excl Days";
            this.exclusionDaysbtn.UseVisualStyleBackColor = true;
            this.exclusionDaysbtn.Click += new System.EventHandler(this.exclusionDaysbtn_Click);
            // 
            // btnPublishToSite
            // 
            this.btnPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPublishToSite.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPublishToSite.Location = new System.Drawing.Point(683, 345);
            this.btnPublishToSite.Name = "btnPublishToSite";
            this.btnPublishToSite.Size = new System.Drawing.Size(109, 23);
            this.btnPublishToSite.TabIndex = 54;
            this.btnPublishToSite.Text = "Publish To Sites";
            this.btnPublishToSite.UseVisualStyleBackColor = true;
            this.btnPublishToSite.Click += new System.EventHandler(this.btnPublishToSite_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblMessage.Location = new System.Drawing.Point(12, 320);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(496, 22);
            this.lblMessage.TabIndex = 55;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // maintSchAssetTaskIdDataGridViewTextBoxColumn
            // 
            this.maintSchAssetTaskIdDataGridViewTextBoxColumn.DataPropertyName = "JobScheduleTaskId";
            this.maintSchAssetTaskIdDataGridViewTextBoxColumn.HeaderText = "Job Schedule Task";
            this.maintSchAssetTaskIdDataGridViewTextBoxColumn.Name = "maintSchAssetTaskIdDataGridViewTextBoxColumn";
            this.maintSchAssetTaskIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // maintScheduleIdDataGridViewTextBoxColumn
            // 
            this.maintScheduleIdDataGridViewTextBoxColumn.DataPropertyName = "JobScheduleId";
            this.maintScheduleIdDataGridViewTextBoxColumn.HeaderText = "JobScheduleId";
            this.maintScheduleIdDataGridViewTextBoxColumn.Name = "maintScheduleIdDataGridViewTextBoxColumn";
            this.maintScheduleIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // assetGroupIdDataGridViewTextBoxColumn
            // 
            this.assetGroupIdDataGridViewTextBoxColumn.DataPropertyName = "AssetGroupId";
            this.assetGroupIdDataGridViewTextBoxColumn.HeaderText = "Asset Group";
            this.assetGroupIdDataGridViewTextBoxColumn.Name = "assetGroupIdDataGridViewTextBoxColumn";
            this.assetGroupIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assetGroupIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // assetTypeIdDataGridViewTextBoxColumn
            // 
            this.assetTypeIdDataGridViewTextBoxColumn.DataPropertyName = "AssetTypeId";
            this.assetTypeIdDataGridViewTextBoxColumn.HeaderText = "Asset Type";
            this.assetTypeIdDataGridViewTextBoxColumn.Name = "assetTypeIdDataGridViewTextBoxColumn";
            this.assetTypeIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assetTypeIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // assetIDDataGridViewTextBoxColumn
            // 
            this.assetIDDataGridViewTextBoxColumn.DataPropertyName = "AssetID";
            this.assetIDDataGridViewTextBoxColumn.HeaderText = "Asset";
            this.assetIDDataGridViewTextBoxColumn.Name = "assetIDDataGridViewTextBoxColumn";
            this.assetIDDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assetIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // maintTaskGroupIdDataGridViewTextBoxColumn
            // 
            this.maintTaskGroupIdDataGridViewTextBoxColumn.DataPropertyName = "JObTaskGroupId";
            this.maintTaskGroupIdDataGridViewTextBoxColumn.HeaderText = "Job Task Group";
            this.maintTaskGroupIdDataGridViewTextBoxColumn.Name = "maintTaskGroupIdDataGridViewTextBoxColumn";
            this.maintTaskGroupIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.maintTaskGroupIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // maintTaskIdDataGridViewTextBoxColumn
            // 
            this.maintTaskIdDataGridViewTextBoxColumn.DataPropertyName = "JobTaskId";
            this.maintTaskIdDataGridViewTextBoxColumn.HeaderText = "Job Task";
            this.maintTaskIdDataGridViewTextBoxColumn.Name = "maintTaskIdDataGridViewTextBoxColumn";
            this.maintTaskIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.maintTaskIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "False";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // ScheduleUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 385);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnPublishToSite);
            this.Controls.Add(this.exclusionDaysbtn);
            this.Controls.Add(this.scheduleDeleteBtn);
            this.Controls.Add(this.scheduleCloseBtn);
            this.Controls.Add(this.scheduleRefreshBtn);
            this.Controls.Add(this.scheduleSaveBtn);
            this.Controls.Add(this.scheduleDataGridView);
            this.Controls.Add(this.gpFilter);
            this.Name = "ScheduleUI";
            this.Text = "Schedule";
            this.Load += new System.EventHandler(this.ScheduleUI_Load);
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            this.gprecur.ResumeLayout(false);
            this.gpRecurType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleAssetTaskDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.GroupBox gprecur;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.RadioButton rbMonthly;
        private System.Windows.Forms.RadioButton rbWeekly;
        private System.Windows.Forms.RadioButton rbDaily;
        private System.Windows.Forms.CheckBox chbRecur;
        private System.Windows.Forms.ComboBox cmbTime;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblAssetName;
        private System.Windows.Forms.Label lblScheduleTime;
        private System.Windows.Forms.DateTimePicker dtscheduleDate;
        private System.Windows.Forms.CheckBox chbActive;
        private System.Windows.Forms.ComboBox cmbAssignedTo;
        private System.Windows.Forms.Label lblDays;
        private System.Windows.Forms.TextBox txtCompleted;
        private System.Windows.Forms.Label lblCompleted;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblAssignedTo;
        private System.Windows.Forms.DataGridView scheduleDataGridView;
        private System.Windows.Forms.Button scheduleDeleteBtn;
        private System.Windows.Forms.Button scheduleCloseBtn;
        private System.Windows.Forms.Button scheduleRefreshBtn;
        private System.Windows.Forms.Button scheduleSaveBtn;
        private System.Windows.Forms.Button exclusionDaysbtn;
        private System.Windows.Forms.RadioButton rbRecurTypeWeekly;
        private System.Windows.Forms.GroupBox gpRecurType;
        private System.Windows.Forms.RadioButton rbRecurTypeDaily;
        private System.Windows.Forms.BindingSource scheduleAssetTaskDTOBindingSource;
        private System.Windows.Forms.Button btnPublishToSite;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn maintSchAssetTaskIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maintScheduleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetGroupIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn maintTaskGroupIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn maintTaskIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}