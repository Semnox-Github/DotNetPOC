namespace Semnox.Parafait.DigitalSignage
{
    partial class DisplayPanelScheduleListUI
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpSchedule = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPanel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtScheduleName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvDisplayPanelScheduleList = new System.Windows.Forms.DataGridView();
            this.scheduleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleEndDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recurFlagDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.recurFrequencyDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.recurEndDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recurTypeDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.scheduleDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.lnkPublish = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayPanelScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpSchedule);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbPanel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtScheduleName);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.chbShowActiveEntries);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(964, 56);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // dtpSchedule
            // 
            this.dtpSchedule.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSchedule.Location = new System.Drawing.Point(691, 19);
            this.dtpSchedule.Name = "dtpSchedule";
            this.dtpSchedule.Size = new System.Drawing.Size(136, 21);
            this.dtpSchedule.TabIndex = 9;
            this.dtpSchedule.Value = new System.DateTime(2017, 3, 13, 10, 29, 2, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(646, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Date :";
            // 
            // cbPanel
            // 
            this.cbPanel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPanel.FormattingEnabled = true;
            this.cbPanel.Location = new System.Drawing.Point(503, 17);
            this.cbPanel.Name = "cbPanel";
            this.cbPanel.Size = new System.Drawing.Size(121, 23);
            this.cbPanel.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(452, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Panel :";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(862, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtScheduleName
            // 
            this.txtScheduleName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtScheduleName.Location = new System.Drawing.Point(298, 19);
            this.txtScheduleName.Name = "txtScheduleName";
            this.txtScheduleName.Size = new System.Drawing.Size(136, 21);
            this.txtScheduleName.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(189, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(103, 20);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Schedule Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(6, 20);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(167, 19);
            this.chbShowActiveEntries.TabIndex = 1;
            this.chbShowActiveEntries.Text = "Show Only Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNew.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnNew.Location = new System.Drawing.Point(16, 264);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 23;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(236, 264);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(126, 264);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 20;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dgvDisplayPanelScheduleList
            // 
            this.dgvDisplayPanelScheduleList.AllowUserToAddRows = false;
            this.dgvDisplayPanelScheduleList.AllowUserToDeleteRows = false;
            this.dgvDisplayPanelScheduleList.AutoGenerateColumns = false;
            this.dgvDisplayPanelScheduleList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDisplayPanelScheduleList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.scheduleIdDataGridViewTextBoxColumn,
            this.scheduleNameDataGridViewTextBoxColumn,
            this.scheduleTimeDataGridViewTextBoxColumn,
            this.scheduleEndDateDataGridViewTextBoxColumn,
            this.recurFlagDataGridViewCheckBoxColumn,
            this.recurFrequencyDataGridViewComboBoxColumn,
            this.recurEndDateDataGridViewTextBoxColumn,
            this.recurTypeDataGridViewComboBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvDisplayPanelScheduleList.DataSource = this.scheduleDTOListBS;
            this.dgvDisplayPanelScheduleList.Location = new System.Drawing.Point(10, 72);
            this.dgvDisplayPanelScheduleList.Name = "dgvDisplayPanelScheduleList";
            this.dgvDisplayPanelScheduleList.ReadOnly = true;
            this.dgvDisplayPanelScheduleList.Size = new System.Drawing.Size(961, 177);
            this.dgvDisplayPanelScheduleList.TabIndex = 24;
            this.dgvDisplayPanelScheduleList.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDisplayPanelScheduleList_CellContentDoubleClick);
            this.dgvDisplayPanelScheduleList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDisplayPanelScheduleList_DataError);
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
            this.scheduleTimeDataGridViewTextBoxColumn.HeaderText = "Start Time";
            this.scheduleTimeDataGridViewTextBoxColumn.Name = "scheduleTimeDataGridViewTextBoxColumn";
            this.scheduleTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // scheduleEndDateDataGridViewTextBoxColumn
            // 
            this.scheduleEndDateDataGridViewTextBoxColumn.DataPropertyName = "ScheduleEndDate";
            this.scheduleEndDateDataGridViewTextBoxColumn.HeaderText = "End Time";
            this.scheduleEndDateDataGridViewTextBoxColumn.Name = "scheduleEndDateDataGridViewTextBoxColumn";
            this.scheduleEndDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // recurFlagDataGridViewCheckBoxColumn
            // 
            this.recurFlagDataGridViewCheckBoxColumn.DataPropertyName = "RecurFlag";
            this.recurFlagDataGridViewCheckBoxColumn.FalseValue = "N";
            this.recurFlagDataGridViewCheckBoxColumn.HeaderText = "Recur Flag";
            this.recurFlagDataGridViewCheckBoxColumn.IndeterminateValue = "N";
            this.recurFlagDataGridViewCheckBoxColumn.Name = "recurFlagDataGridViewCheckBoxColumn";
            this.recurFlagDataGridViewCheckBoxColumn.ReadOnly = true;
            this.recurFlagDataGridViewCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.recurFlagDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.recurFlagDataGridViewCheckBoxColumn.TrueValue = "Y";
            // 
            // recurFrequencyDataGridViewComboBoxColumn
            // 
            this.recurFrequencyDataGridViewComboBoxColumn.DataPropertyName = "RecurFrequency";
            this.recurFrequencyDataGridViewComboBoxColumn.HeaderText = "Recur Frequency";
            this.recurFrequencyDataGridViewComboBoxColumn.Name = "recurFrequencyDataGridViewComboBoxColumn";
            this.recurFrequencyDataGridViewComboBoxColumn.ReadOnly = true;
            this.recurFrequencyDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.recurFrequencyDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // recurEndDateDataGridViewTextBoxColumn
            // 
            this.recurEndDateDataGridViewTextBoxColumn.DataPropertyName = "RecurEndDate";
            this.recurEndDateDataGridViewTextBoxColumn.HeaderText = "Recur End Date";
            this.recurEndDateDataGridViewTextBoxColumn.Name = "recurEndDateDataGridViewTextBoxColumn";
            this.recurEndDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // recurTypeDataGridViewComboBoxColumn
            // 
            this.recurTypeDataGridViewComboBoxColumn.DataPropertyName = "RecurType";
            this.recurTypeDataGridViewComboBoxColumn.HeaderText = "Recur Type";
            this.recurTypeDataGridViewComboBoxColumn.Name = "recurTypeDataGridViewComboBoxColumn";
            this.recurTypeDataGridViewComboBoxColumn.ReadOnly = true;
            this.recurTypeDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.recurTypeDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "N";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.IndeterminateValue = "N";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "Y";
            // 
            // scheduleDTOListBS
            // 
            this.scheduleDTOListBS.DataSource = typeof(Semnox.Core.GenericUtilities.ScheduleCalendarDTO);
            // 
            // lnkPublish
            // 
            this.lnkPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublish.AutoSize = true;
            this.lnkPublish.BackColor = System.Drawing.Color.Blue;
            this.lnkPublish.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublish.ForeColor = System.Drawing.Color.White;
            this.lnkPublish.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkPublish.LinkColor = System.Drawing.Color.White;
            this.lnkPublish.Location = new System.Drawing.Point(875, 268);
            this.lnkPublish.Name = "lnkPublish";
            this.lnkPublish.Size = new System.Drawing.Size(96, 15);
            this.lnkPublish.TabIndex = 38;
            this.lnkPublish.TabStop = true;
            this.lnkPublish.Text = "Publish To Sites";
            this.lnkPublish.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublish_LinkClicked);
            // 
            // DisplayPanelScheduleListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 300);
            this.Controls.Add(this.lnkPublish);
            this.Controls.Add(this.dgvDisplayPanelScheduleList);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.groupBox1);
            this.Name = "DisplayPanelScheduleListUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Signage Schedules";
            this.Load += new System.EventHandler(this.DisplayPanelScheduleListUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayPanelScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtScheduleName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpSchedule;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvDisplayPanelScheduleList;
        private System.Windows.Forms.BindingSource scheduleDTOListBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleEndDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn recurFlagDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn recurFrequencyDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recurEndDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn recurTypeDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.LinkLabel lnkPublish;
    }
}