namespace Semnox.Parafait.Schedule
{
    partial class ScheduleCalendarUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScheduleCalendarUI));
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblAssetName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.tabCalendar = new System.Windows.Forms.TabControl();
            this.tabPageDay = new System.Windows.Forms.TabPage();
            this.dgvDay = new System.Windows.Forms.DataGridView();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageWeek = new System.Windows.Forms.TabPage();
            this.dgvWeek = new System.Windows.Forms.DataGridView();
            this.dayOne = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayTwo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayThree = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayFour = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayFive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.daySix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.daySeven = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageAll = new System.Windows.Forms.TabPage();
            this.dgvAll = new System.Windows.Forms.DataGridView();
            this.scheduleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recurFlagDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.recurFrequencyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recurEndDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recurTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.scheduleDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dgvSearch = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuschedue = new System.Windows.Forms.ToolStripMenuItem();
            this.gpFilter.SuspendLayout();
            this.tabCalendar.SuspendLayout();
            this.tabPageDay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDay)).BeginInit();
            this.tabPageWeek.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeek)).BeginInit();
            this.tabPageAll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.txtName);
            this.gpFilter.Controls.Add(this.lblAssetName);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Controls.Add(this.btnPrev);
            this.gpFilter.Controls.Add(this.btnNext);
            this.gpFilter.Controls.Add(this.btnClose);
            this.gpFilter.Controls.Add(this.label1);
            this.gpFilter.Controls.Add(this.datePicker);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(9, 3);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(1029, 46);
            this.gpFilter.TabIndex = 18;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(733, 15);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 21);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(450, 15);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(134, 21);
            this.txtName.TabIndex = 14;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // lblAssetName
            // 
            this.lblAssetName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssetName.Location = new System.Drawing.Point(377, 15);
            this.lblAssetName.Name = "lblAssetName";
            this.lblAssetName.Size = new System.Drawing.Size(67, 20);
            this.lblAssetName.TabIndex = 13;
            this.lblAssetName.Text = "Name:";
            this.lblAssetName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(590, 18);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 12;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPrev.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrev.Location = new System.Drawing.Point(301, 13);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(30, 25);
            this.btnPrev.TabIndex = 11;
            this.btnPrev.Text = "<<";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.SystemColors.Control;
            this.btnNext.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNext.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnNext.Location = new System.Drawing.Point(338, 13);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(30, 25);
            this.btnNext.TabIndex = 10;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(826, 16);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 21);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Change Date:";
            // 
            // datePicker
            // 
            this.datePicker.CustomFormat = "dddd, dd-MMM-yyyy";
            this.datePicker.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datePicker.Location = new System.Drawing.Point(100, 15);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(184, 21);
            this.datePicker.TabIndex = 7;
            this.datePicker.ValueChanged += new System.EventHandler(this.DatePicker_ValueChanged);
            // 
            // tabCalendar
            // 
            this.tabCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCalendar.Controls.Add(this.tabPageDay);
            this.tabCalendar.Controls.Add(this.tabPageWeek);
            this.tabCalendar.Controls.Add(this.tabPageAll);
            this.tabCalendar.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCalendar.ItemSize = new System.Drawing.Size(42, 23);
            this.tabCalendar.Location = new System.Drawing.Point(12, 55);
            this.tabCalendar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabCalendar.Name = "tabCalendar";
            this.tabCalendar.SelectedIndex = 0;
            this.tabCalendar.Size = new System.Drawing.Size(1026, 511);
            this.tabCalendar.TabIndex = 19;
            this.tabCalendar.SelectedIndexChanged += new System.EventHandler(this.tabCalendar_SelectedIndexChanged);
            // 
            // tabPageDay
            // 
            this.tabPageDay.AutoScroll = true;
            this.tabPageDay.Controls.Add(this.dgvDay);
            this.tabPageDay.Location = new System.Drawing.Point(4, 27);
            this.tabPageDay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageDay.Name = "tabPageDay";
            this.tabPageDay.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageDay.Size = new System.Drawing.Size(1018, 480);
            this.tabPageDay.TabIndex = 0;
            this.tabPageDay.Text = "Day";
            this.tabPageDay.UseVisualStyleBackColor = true;
            // 
            // dgvDay
            // 
            this.dgvDay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.time});
            this.dgvDay.EnableHeadersVisualStyles = false;
            this.dgvDay.Location = new System.Drawing.Point(0, 0);
            this.dgvDay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvDay.Name = "dgvDay";
            this.dgvDay.Size = new System.Drawing.Size(734, 474);
            this.dgvDay.TabIndex = 0;
            this.dgvDay.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvDay_CellPainting);
            this.dgvDay.DoubleClick += new System.EventHandler(this.dgvDay_DoubleClick);
            // 
            // time
            // 
            this.time.HeaderText = "";
            this.time.Name = "time";
            // 
            // tabPageWeek
            // 
            this.tabPageWeek.AutoScroll = true;
            this.tabPageWeek.Controls.Add(this.dgvWeek);
            this.tabPageWeek.Location = new System.Drawing.Point(4, 27);
            this.tabPageWeek.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageWeek.Name = "tabPageWeek";
            this.tabPageWeek.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageWeek.Size = new System.Drawing.Size(1018, 480);
            this.tabPageWeek.TabIndex = 1;
            this.tabPageWeek.Text = "Week";
            this.tabPageWeek.UseVisualStyleBackColor = true;
            // 
            // dgvWeek
            // 
            this.dgvWeek.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWeek.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dayOne,
            this.dayTwo,
            this.dayThree,
            this.dayFour,
            this.dayFive,
            this.daySix,
            this.daySeven});
            this.dgvWeek.Location = new System.Drawing.Point(0, 0);
            this.dgvWeek.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvWeek.Name = "dgvWeek";
            this.dgvWeek.Size = new System.Drawing.Size(1000, 609);
            this.dgvWeek.TabIndex = 0;
            this.dgvWeek.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWeek_CellDoubleClick);
            this.dgvWeek.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvWeek_CellPainting);
            // 
            // dayOne
            // 
            this.dayOne.HeaderText = "dayOne";
            this.dayOne.Name = "dayOne";
            // 
            // dayTwo
            // 
            this.dayTwo.HeaderText = "dayTwo";
            this.dayTwo.Name = "dayTwo";
            // 
            // dayThree
            // 
            this.dayThree.HeaderText = "dayThree";
            this.dayThree.Name = "dayThree";
            // 
            // dayFour
            // 
            this.dayFour.HeaderText = "dayFour";
            this.dayFour.Name = "dayFour";
            // 
            // dayFive
            // 
            this.dayFive.HeaderText = "dayFive";
            this.dayFive.Name = "dayFive";
            // 
            // daySix
            // 
            this.daySix.HeaderText = "daySix";
            this.daySix.Name = "daySix";
            // 
            // daySeven
            // 
            this.daySeven.HeaderText = "daySeven";
            this.daySeven.Name = "daySeven";
            // 
            // tabPageAll
            // 
            this.tabPageAll.Controls.Add(this.dgvAll);
            this.tabPageAll.Location = new System.Drawing.Point(4, 27);
            this.tabPageAll.Name = "tabPageAll";
            this.tabPageAll.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAll.Size = new System.Drawing.Size(1018, 480);
            this.tabPageAll.TabIndex = 2;
            this.tabPageAll.Text = "All";
            this.tabPageAll.UseVisualStyleBackColor = true;
            // 
            // dgvAll
            // 
            this.dgvAll.AllowUserToAddRows = false;
            this.dgvAll.AllowUserToDeleteRows = false;
            this.dgvAll.AutoGenerateColumns = false;
            this.dgvAll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAll.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.scheduleIdDataGridViewTextBoxColumn,
            this.scheduleNameDataGridViewTextBoxColumn,
            this.scheduleTimeDataGridViewTextBoxColumn,
            this.recurFlagDataGridViewCheckBoxColumn,
            this.recurFrequencyDataGridViewTextBoxColumn,
            this.recurEndDateDataGridViewTextBoxColumn,
            this.recurTypeDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvAll.DataSource = this.scheduleDTOBindingSource;
            this.dgvAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAll.Location = new System.Drawing.Point(3, 3);
            this.dgvAll.Name = "dgvAll";
            this.dgvAll.ReadOnly = true;
            this.dgvAll.Size = new System.Drawing.Size(1012, 474);
            this.dgvAll.TabIndex = 0;
            this.dgvAll.DoubleClick += new System.EventHandler(this.dgvAll_DoubleClick);
            // 
            // scheduleIdDataGridViewTextBoxColumn
            // 
            this.scheduleIdDataGridViewTextBoxColumn.DataPropertyName = "ScheduleId";
            this.scheduleIdDataGridViewTextBoxColumn.HeaderText = "Schedule Id";
            this.scheduleIdDataGridViewTextBoxColumn.Name = "scheduleIdDataGridViewTextBoxColumn";
            this.scheduleIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // scheduleNameDataGridViewTextBoxColumn
            // 
            this.scheduleNameDataGridViewTextBoxColumn.DataPropertyName = "ScheduleName";
            this.scheduleNameDataGridViewTextBoxColumn.HeaderText = "Schedule Name";
            this.scheduleNameDataGridViewTextBoxColumn.Name = "scheduleNameDataGridViewTextBoxColumn";
            this.scheduleNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // scheduleTimeDataGridViewTextBoxColumn
            // 
            this.scheduleTimeDataGridViewTextBoxColumn.DataPropertyName = "ScheduleTime";
            this.scheduleTimeDataGridViewTextBoxColumn.HeaderText = "Schedule Time";
            this.scheduleTimeDataGridViewTextBoxColumn.Name = "scheduleTimeDataGridViewTextBoxColumn";
            this.scheduleTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // recurFlagDataGridViewCheckBoxColumn
            // 
            this.recurFlagDataGridViewCheckBoxColumn.DataPropertyName = "RecurFlag";
            this.recurFlagDataGridViewCheckBoxColumn.FalseValue = "N";
            this.recurFlagDataGridViewCheckBoxColumn.HeaderText = "Recur Flag";
            this.recurFlagDataGridViewCheckBoxColumn.Name = "recurFlagDataGridViewCheckBoxColumn";
            this.recurFlagDataGridViewCheckBoxColumn.ReadOnly = true;
            this.recurFlagDataGridViewCheckBoxColumn.TrueValue = "Y";
            // 
            // recurFrequencyDataGridViewTextBoxColumn
            // 
            this.recurFrequencyDataGridViewTextBoxColumn.DataPropertyName = "RecurFrequency";
            this.recurFrequencyDataGridViewTextBoxColumn.HeaderText = "Recur Frequency";
            this.recurFrequencyDataGridViewTextBoxColumn.Name = "recurFrequencyDataGridViewTextBoxColumn";
            this.recurFrequencyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // recurEndDateDataGridViewTextBoxColumn
            // 
            this.recurEndDateDataGridViewTextBoxColumn.DataPropertyName = "RecurEndDate";
            this.recurEndDateDataGridViewTextBoxColumn.HeaderText = "End Date";
            this.recurEndDateDataGridViewTextBoxColumn.Name = "recurEndDateDataGridViewTextBoxColumn";
            this.recurEndDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // recurTypeDataGridViewTextBoxColumn
            // 
            this.recurTypeDataGridViewTextBoxColumn.DataPropertyName = "RecurType";
            this.recurTypeDataGridViewTextBoxColumn.HeaderText = "Recur Type";
            this.recurTypeDataGridViewTextBoxColumn.Name = "recurTypeDataGridViewTextBoxColumn";
            this.recurTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "N";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "Y";
            // 
            // scheduleDTOBindingSource
            // 
            this.scheduleDTOBindingSource.DataSource = typeof(Semnox.Core.GenericUtilities.ScheduleCalendarDTO);
            // 
            // dgvSearch
            // 
            this.dgvSearch.AllowUserToAddRows = false;
            this.dgvSearch.AllowUserToDeleteRows = false;
            this.dgvSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSearch.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearch.ColumnHeadersVisible = false;
            this.dgvSearch.Location = new System.Drawing.Point(459, 40);
            this.dgvSearch.Name = "dgvSearch";
            this.dgvSearch.ReadOnly = true;
            this.dgvSearch.RowHeadersVisible = false;
            this.dgvSearch.RowTemplate.Height = 35;
            this.dgvSearch.Size = new System.Drawing.Size(134, 181);
            this.dgvSearch.TabIndex = 16;
            this.dgvSearch.Visible = false;
            this.dgvSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSearch_CellClick);
            this.dgvSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSearch_KeyDown);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuschedue});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(123, 26);
            // 
            // menuschedue
            // 
            this.menuschedue.Image = ((System.Drawing.Image)(resources.GetObject("menuschedue.Image")));
            this.menuschedue.Name = "menuschedue";
            this.menuschedue.Size = new System.Drawing.Size(122, 22);
            this.menuschedue.Text = "Schedule";
            // 
            // ScheduleCalendarUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 569);
            this.Controls.Add(this.dgvSearch);
            this.Controls.Add(this.tabCalendar);
            this.Controls.Add(this.gpFilter);
            this.Name = "ScheduleCalendarUI";
            this.Text = "Schedule Calendar";
            this.Load += new System.EventHandler(this.SchedueCalendarUI_Load);
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            this.tabCalendar.ResumeLayout(false);
            this.tabPageDay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDay)).EndInit();
            this.tabPageWeek.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeek)).EndInit();
            this.tabPageAll.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.Label lblAssetName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.TabControl tabCalendar;
        private System.Windows.Forms.TabPage tabPageDay;
        private System.Windows.Forms.DataGridView dgvDay;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.TabPage tabPageWeek;
        private System.Windows.Forms.DataGridView dgvWeek;
        private System.Windows.Forms.DataGridViewTextBoxColumn dayOne;
        private System.Windows.Forms.DataGridViewTextBoxColumn dayTwo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dayThree;
        private System.Windows.Forms.DataGridViewTextBoxColumn dayFour;
        private System.Windows.Forms.DataGridViewTextBoxColumn dayFive;
        private System.Windows.Forms.DataGridViewTextBoxColumn daySix;
        private System.Windows.Forms.DataGridViewTextBoxColumn daySeven;
        private System.Windows.Forms.TabPage tabPageAll;
        private System.Windows.Forms.DataGridView dgvAll;       
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.DataGridView dgvSearch;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuschedue;
        private System.Windows.Forms.BindingSource scheduleDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn recurFlagDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recurFrequencyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recurEndDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recurTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}

