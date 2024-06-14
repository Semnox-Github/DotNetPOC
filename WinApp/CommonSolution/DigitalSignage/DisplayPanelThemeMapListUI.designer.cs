namespace Semnox.Parafait.DigitalSignage
{
    partial class DisplayPanelThemeMapListUI
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
            if(disposing && (components != null))
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gbSchedule = new System.Windows.Forms.GroupBox();
            this.dtpScheduleEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpScheduleEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblScheduleEndDate = new System.Windows.Forms.Label();
            this.dtpScheduleTime = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdbWeekDay = new System.Windows.Forms.RadioButton();
            this.rdbDay = new System.Windows.Forms.RadioButton();
            this.dtpRecurEndDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.rdbMonthly = new System.Windows.Forms.RadioButton();
            this.rdbWeekly = new System.Windows.Forms.RadioButton();
            this.rdbDaily = new System.Windows.Forms.RadioButton();
            this.chbRecurFlag = new System.Windows.Forms.CheckBox();
            this.dtpScheduleDate = new System.Windows.Forms.DateTimePicker();
            this.lblScheduleDate = new System.Windows.Forms.Label();
            this.chbActive = new System.Windows.Forms.CheckBox();
            this.txtScheduleName = new System.Windows.Forms.TextBox();
            this.lblScheduleName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvDisplayPanelThemeMapDTOList = new System.Windows.Forms.DataGridView();
            this.dgvDisplayPanelThemeMapListIdTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDisplayPanelThemeMapListPanelIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvDisplayPanelThemeMapListThemeIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvDisplayPanelThemeMapListIsActiveCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.displayPanelThemeMapDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbTheme = new System.Windows.Forms.ComboBox();
            this.lblTheme = new System.Windows.Forms.Label();
            this.cbPanel = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblPanel = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnInclExclDays = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbSchedule.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayPanelThemeMapDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.displayPanelThemeMapDTOListBS)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gbSchedule, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 13);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(958, 393);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gbSchedule
            // 
            this.gbSchedule.Controls.Add(this.dtpScheduleEndTime);
            this.gbSchedule.Controls.Add(this.dtpScheduleEndDate);
            this.gbSchedule.Controls.Add(this.lblScheduleEndDate);
            this.gbSchedule.Controls.Add(this.dtpScheduleTime);
            this.gbSchedule.Controls.Add(this.groupBox2);
            this.gbSchedule.Controls.Add(this.dtpScheduleDate);
            this.gbSchedule.Controls.Add(this.lblScheduleDate);
            this.gbSchedule.Controls.Add(this.chbActive);
            this.gbSchedule.Controls.Add(this.txtScheduleName);
            this.gbSchedule.Controls.Add(this.lblScheduleName);
            this.gbSchedule.Location = new System.Drawing.Point(0, 0);
            this.gbSchedule.Margin = new System.Windows.Forms.Padding(0);
            this.gbSchedule.Name = "gbSchedule";
            this.gbSchedule.Padding = new System.Windows.Forms.Padding(5);
            this.gbSchedule.Size = new System.Drawing.Size(958, 117);
            this.gbSchedule.TabIndex = 0;
            this.gbSchedule.TabStop = false;
            this.gbSchedule.Text = "Schedule";
            // 
            // dtpScheduleEndTime
            // 
            this.dtpScheduleEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpScheduleEndTime.Location = new System.Drawing.Point(230, 79);
            this.dtpScheduleEndTime.Name = "dtpScheduleEndTime";
            this.dtpScheduleEndTime.ShowUpDown = true;
            this.dtpScheduleEndTime.Size = new System.Drawing.Size(100, 20);
            this.dtpScheduleEndTime.TabIndex = 10;
            // 
            // dtpScheduleEndDate
            // 
            this.dtpScheduleEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpScheduleEndDate.Location = new System.Drawing.Point(116, 79);
            this.dtpScheduleEndDate.Name = "dtpScheduleEndDate";
            this.dtpScheduleEndDate.Size = new System.Drawing.Size(100, 20);
            this.dtpScheduleEndDate.TabIndex = 9;
            // 
            // lblScheduleEndDate
            // 
            this.lblScheduleEndDate.AutoSize = true;
            this.lblScheduleEndDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblScheduleEndDate.Location = new System.Drawing.Point(45, 82);
            this.lblScheduleEndDate.Name = "lblScheduleEndDate";
            this.lblScheduleEndDate.Size = new System.Drawing.Size(65, 15);
            this.lblScheduleEndDate.TabIndex = 8;
            this.lblScheduleEndDate.Text = "End Time :";
            // 
            // dtpScheduleTime
            // 
            this.dtpScheduleTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpScheduleTime.Location = new System.Drawing.Point(230, 46);
            this.dtpScheduleTime.Name = "dtpScheduleTime";
            this.dtpScheduleTime.ShowUpDown = true;
            this.dtpScheduleTime.Size = new System.Drawing.Size(100, 20);
            this.dtpScheduleTime.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.dtpRecurEndDate);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.rdbMonthly);
            this.groupBox2.Controls.Add(this.rdbWeekly);
            this.groupBox2.Controls.Add(this.rdbDaily);
            this.groupBox2.Controls.Add(this.chbRecurFlag);
            this.groupBox2.Location = new System.Drawing.Point(398, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(549, 80);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recurrence";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdbWeekDay);
            this.groupBox3.Controls.Add(this.rdbDay);
            this.groupBox3.Location = new System.Drawing.Point(398, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(145, 50);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Recurrence Type";
            // 
            // rdbWeekDay
            // 
            this.rdbWeekDay.AutoSize = true;
            this.rdbWeekDay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rdbWeekDay.Location = new System.Drawing.Point(56, 19);
            this.rdbWeekDay.Name = "rdbWeekDay";
            this.rdbWeekDay.Size = new System.Drawing.Size(79, 19);
            this.rdbWeekDay.TabIndex = 1;
            this.rdbWeekDay.TabStop = true;
            this.rdbWeekDay.Text = "WeekDay";
            this.rdbWeekDay.UseVisualStyleBackColor = true;
            // 
            // rdbDay
            // 
            this.rdbDay.AutoSize = true;
            this.rdbDay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rdbDay.Location = new System.Drawing.Point(6, 19);
            this.rdbDay.Name = "rdbDay";
            this.rdbDay.Size = new System.Drawing.Size(46, 19);
            this.rdbDay.TabIndex = 0;
            this.rdbDay.TabStop = true;
            this.rdbDay.Text = "Day";
            this.rdbDay.UseVisualStyleBackColor = true;
            // 
            // dtpRecurEndDate
            // 
            this.dtpRecurEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpRecurEndDate.Location = new System.Drawing.Point(113, 46);
            this.dtpRecurEndDate.Name = "dtpRecurEndDate";
            this.dtpRecurEndDate.Size = new System.Drawing.Size(100, 20);
            this.dtpRecurEndDate.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(11, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Recur End Date :";
            // 
            // rdbMonthly
            // 
            this.rdbMonthly.AutoSize = true;
            this.rdbMonthly.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rdbMonthly.Location = new System.Drawing.Point(234, 19);
            this.rdbMonthly.Name = "rdbMonthly";
            this.rdbMonthly.Size = new System.Drawing.Size(69, 19);
            this.rdbMonthly.TabIndex = 9;
            this.rdbMonthly.TabStop = true;
            this.rdbMonthly.Text = "Monthly";
            this.rdbMonthly.UseVisualStyleBackColor = true;
            this.rdbMonthly.CheckedChanged += new System.EventHandler(this.rdbMonthly_CheckedChanged);
            // 
            // rdbWeekly
            // 
            this.rdbWeekly.AutoSize = true;
            this.rdbWeekly.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rdbWeekly.Location = new System.Drawing.Point(167, 19);
            this.rdbWeekly.Name = "rdbWeekly";
            this.rdbWeekly.Size = new System.Drawing.Size(67, 19);
            this.rdbWeekly.TabIndex = 8;
            this.rdbWeekly.TabStop = true;
            this.rdbWeekly.Text = "Weekly";
            this.rdbWeekly.UseVisualStyleBackColor = true;
            this.rdbWeekly.CheckedChanged += new System.EventHandler(this.rdbWeekly_CheckedChanged);
            // 
            // rdbDaily
            // 
            this.rdbDaily.AutoSize = true;
            this.rdbDaily.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rdbDaily.Location = new System.Drawing.Point(113, 19);
            this.rdbDaily.Name = "rdbDaily";
            this.rdbDaily.Size = new System.Drawing.Size(52, 19);
            this.rdbDaily.TabIndex = 7;
            this.rdbDaily.TabStop = true;
            this.rdbDaily.Text = "Daily";
            this.rdbDaily.UseVisualStyleBackColor = true;
            this.rdbDaily.CheckedChanged += new System.EventHandler(this.rdbDaily_CheckedChanged);
            // 
            // chbRecurFlag
            // 
            this.chbRecurFlag.AutoSize = true;
            this.chbRecurFlag.Checked = true;
            this.chbRecurFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbRecurFlag.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbRecurFlag.Location = new System.Drawing.Point(14, 19);
            this.chbRecurFlag.Name = "chbRecurFlag";
            this.chbRecurFlag.Size = new System.Drawing.Size(86, 19);
            this.chbRecurFlag.TabIndex = 6;
            this.chbRecurFlag.Text = "Recur Flag";
            this.chbRecurFlag.UseVisualStyleBackColor = true;
            this.chbRecurFlag.CheckStateChanged += new System.EventHandler(this.chbRecurFlag_CheckStateChanged);
            // 
            // dtpScheduleDate
            // 
            this.dtpScheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpScheduleDate.Location = new System.Drawing.Point(116, 46);
            this.dtpScheduleDate.Name = "dtpScheduleDate";
            this.dtpScheduleDate.Size = new System.Drawing.Size(100, 20);
            this.dtpScheduleDate.TabIndex = 4;
            this.dtpScheduleDate.ValueChanged += new System.EventHandler(this.dtpScheduleDate_ValueChanged);
            // 
            // lblScheduleDate
            // 
            this.lblScheduleDate.AutoSize = true;
            this.lblScheduleDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblScheduleDate.Location = new System.Drawing.Point(38, 51);
            this.lblScheduleDate.Name = "lblScheduleDate";
            this.lblScheduleDate.Size = new System.Drawing.Size(72, 15);
            this.lblScheduleDate.TabIndex = 3;
            this.lblScheduleDate.Text = "Start Time :";
            // 
            // chbActive
            // 
            this.chbActive.AutoSize = true;
            this.chbActive.Checked = true;
            this.chbActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbActive.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbActive.Location = new System.Drawing.Point(230, 16);
            this.chbActive.Name = "chbActive";
            this.chbActive.Size = new System.Drawing.Size(61, 19);
            this.chbActive.TabIndex = 2;
            this.chbActive.Text = "Active";
            this.chbActive.UseVisualStyleBackColor = true;
            // 
            // txtScheduleName
            // 
            this.txtScheduleName.Location = new System.Drawing.Point(116, 15);
            this.txtScheduleName.Name = "txtScheduleName";
            this.txtScheduleName.Size = new System.Drawing.Size(100, 20);
            this.txtScheduleName.TabIndex = 1;
            // 
            // lblScheduleName
            // 
            this.lblScheduleName.AutoSize = true;
            this.lblScheduleName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblScheduleName.Location = new System.Drawing.Point(8, 18);
            this.lblScheduleName.Name = "lblScheduleName";
            this.lblScheduleName.Size = new System.Drawing.Size(102, 15);
            this.lblScheduleName.TabIndex = 0;
            this.lblScheduleName.Text = "Schedule Name :";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgvDisplayPanelThemeMapDTOList);
            this.groupBox1.Location = new System.Drawing.Point(0, 156);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(958, 237);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Panel Theme Mappings";
            this.groupBox1.UseCompatibleTextRendering = true;
            // 
            // dgvDisplayPanelThemeMapDTOList
            // 
            this.dgvDisplayPanelThemeMapDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDisplayPanelThemeMapDTOList.AutoGenerateColumns = false;
            this.dgvDisplayPanelThemeMapDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDisplayPanelThemeMapDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvDisplayPanelThemeMapListIdTextBoxColumn,
            this.dgvDisplayPanelThemeMapListPanelIdComboBoxColumn,
            this.dgvDisplayPanelThemeMapListThemeIdComboBoxColumn,
            this.dgvDisplayPanelThemeMapListIsActiveCheckBoxColumn});
            this.dgvDisplayPanelThemeMapDTOList.DataSource = this.displayPanelThemeMapDTOListBS;
            this.dgvDisplayPanelThemeMapDTOList.Location = new System.Drawing.Point(8, 21);
            this.dgvDisplayPanelThemeMapDTOList.Name = "dgvDisplayPanelThemeMapDTOList";
            this.dgvDisplayPanelThemeMapDTOList.Size = new System.Drawing.Size(942, 208);
            this.dgvDisplayPanelThemeMapDTOList.TabIndex = 0;
            this.dgvDisplayPanelThemeMapDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDisplayPanelThemeMapDTOList_DataError);
            // 
            // dgvDisplayPanelThemeMapListIdTextBoxColumn
            // 
            this.dgvDisplayPanelThemeMapListIdTextBoxColumn.DataPropertyName = "Id";
            this.dgvDisplayPanelThemeMapListIdTextBoxColumn.HeaderText = "SI#";
            this.dgvDisplayPanelThemeMapListIdTextBoxColumn.Name = "dgvDisplayPanelThemeMapListIdTextBoxColumn";
            this.dgvDisplayPanelThemeMapListIdTextBoxColumn.ReadOnly = true;
            // 
            // dgvDisplayPanelThemeMapListPanelIdComboBoxColumn
            // 
            this.dgvDisplayPanelThemeMapListPanelIdComboBoxColumn.DataPropertyName = "PanelId";
            this.dgvDisplayPanelThemeMapListPanelIdComboBoxColumn.HeaderText = "Panel";
            this.dgvDisplayPanelThemeMapListPanelIdComboBoxColumn.Name = "dgvDisplayPanelThemeMapListPanelIdComboBoxColumn";
            this.dgvDisplayPanelThemeMapListPanelIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDisplayPanelThemeMapListPanelIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvDisplayPanelThemeMapListThemeIdComboBoxColumn
            // 
            this.dgvDisplayPanelThemeMapListThemeIdComboBoxColumn.DataPropertyName = "ThemeId";
            this.dgvDisplayPanelThemeMapListThemeIdComboBoxColumn.HeaderText = "Theme";
            this.dgvDisplayPanelThemeMapListThemeIdComboBoxColumn.Name = "dgvDisplayPanelThemeMapListThemeIdComboBoxColumn";
            this.dgvDisplayPanelThemeMapListThemeIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDisplayPanelThemeMapListThemeIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvDisplayPanelThemeMapListIsActiveCheckBoxColumn
            // 
            this.dgvDisplayPanelThemeMapListIsActiveCheckBoxColumn.DataPropertyName = "IsActive";
            this.dgvDisplayPanelThemeMapListIsActiveCheckBoxColumn.HeaderText = "Active";
            this.dgvDisplayPanelThemeMapListIsActiveCheckBoxColumn.Name = "dgvDisplayPanelThemeMapListIsActiveCheckBoxColumn";
            this.dgvDisplayPanelThemeMapListIsActiveCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDisplayPanelThemeMapListIsActiveCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // displayPanelThemeMapDTOListBS
            // 
            this.displayPanelThemeMapDTOListBS.DataSource = typeof(DisplayPanelThemeMapDTO);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbTheme);
            this.groupBox4.Controls.Add(this.lblTheme);
            this.groupBox4.Controls.Add(this.cbPanel);
            this.groupBox4.Controls.Add(this.btnSearch);
            this.groupBox4.Controls.Add(this.lblPanel);
            this.groupBox4.Controls.Add(this.chbShowActiveEntries);
            this.groupBox4.Location = new System.Drawing.Point(0, 117);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(958, 39);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Filter";
            // 
            // cbTheme
            // 
            this.cbTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTheme.FormattingEnabled = true;
            this.cbTheme.Location = new System.Drawing.Point(447, 12);
            this.cbTheme.Name = "cbTheme";
            this.cbTheme.Size = new System.Drawing.Size(121, 21);
            this.cbTheme.TabIndex = 11;
            // 
            // lblTheme
            // 
            this.lblTheme.AutoSize = true;
            this.lblTheme.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblTheme.Location = new System.Drawing.Point(395, 15);
            this.lblTheme.Name = "lblTheme";
            this.lblTheme.Size = new System.Drawing.Size(52, 15);
            this.lblTheme.TabIndex = 10;
            this.lblTheme.Text = "Theme :";
            // 
            // cbPanel
            // 
            this.cbPanel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPanel.FormattingEnabled = true;
            this.cbPanel.Location = new System.Drawing.Point(243, 12);
            this.cbPanel.Name = "cbPanel";
            this.cbPanel.Size = new System.Drawing.Size(121, 21);
            this.cbPanel.TabIndex = 9;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(606, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblPanel
            // 
            this.lblPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblPanel.Location = new System.Drawing.Point(141, 12);
            this.lblPanel.Name = "lblPanel";
            this.lblPanel.Size = new System.Drawing.Size(96, 20);
            this.lblPanel.TabIndex = 6;
            this.lblPanel.Text = "Panel :";
            this.lblPanel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(11, 14);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(124, 19);
            this.chbShowActiveEntries.TabIndex = 5;
            this.chbShowActiveEntries.Text = "Show Active Only";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // btnInclExclDays
            // 
            this.btnInclExclDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInclExclDays.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnInclExclDays.Location = new System.Drawing.Point(453, 425);
            this.btnInclExclDays.Name = "btnInclExclDays";
            this.btnInclExclDays.Size = new System.Drawing.Size(75, 23);
            this.btnInclExclDays.TabIndex = 25;
            this.btnInclExclDays.Text = "Incl/Excl Days";
            this.btnInclExclDays.UseVisualStyleBackColor = true;
            this.btnInclExclDays.Click += new System.EventHandler(this.btnInclExclDays_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(13, 425);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(343, 425);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(233, 425);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 22;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(123, 425);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 21;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // DisplayPanelThemeMapListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 461);
            this.Controls.Add(this.btnInclExclDays);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DisplayPanelThemeMapListUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Panel Theme Mappings";
            this.Load += new System.EventHandler(this.DisplayPanelThemeMapListUI_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gbSchedule.ResumeLayout(false);
            this.gbSchedule.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayPanelThemeMapDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.displayPanelThemeMapDTOListBS)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnInclExclDays;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox gbSchedule;
        private System.Windows.Forms.TextBox txtScheduleName;
        private System.Windows.Forms.Label lblScheduleName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvDisplayPanelThemeMapDTOList;
        private System.Windows.Forms.CheckBox chbActive;
        private System.Windows.Forms.Label lblScheduleDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtpScheduleDate;
        private System.Windows.Forms.DateTimePicker dtpRecurEndDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdbMonthly;
        private System.Windows.Forms.RadioButton rdbWeekly;
        private System.Windows.Forms.RadioButton rdbDaily;
        private System.Windows.Forms.CheckBox chbRecurFlag;
        private System.Windows.Forms.BindingSource displayPanelThemeMapDTOListBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayPanelThemeMapListIdTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDisplayPanelThemeMapListPanelIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDisplayPanelThemeMapListThemeIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDisplayPanelThemeMapListIsActiveCheckBoxColumn;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblPanel;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Label lblTheme;
        private System.Windows.Forms.ComboBox cbPanel;
        private System.Windows.Forms.ComboBox cbTheme;
        private System.Windows.Forms.DateTimePicker dtpScheduleTime;
        private System.Windows.Forms.DateTimePicker dtpScheduleEndDate;
        private System.Windows.Forms.Label lblScheduleEndDate;
        private System.Windows.Forms.DateTimePicker dtpScheduleEndTime;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdbWeekDay;
        private System.Windows.Forms.RadioButton rdbDay;
    }
}