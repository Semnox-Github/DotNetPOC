namespace Semnox.Core.GenericUtilities
{
    partial class ScheduleExclusionUI
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
            this.txtScheduleName = new System.Windows.Forms.TextBox();
            this.lblScheduleName = new System.Windows.Forms.Label();
            this.scheduleExclusionDeleteBtn = new System.Windows.Forms.Button();
            this.scheduleExclusionCloseBtn = new System.Windows.Forms.Button();
            this.scheduleExclusionRefreshBtn = new System.Windows.Forms.Button();
            this.scheduleExclusionSaveBtn = new System.Windows.Forms.Button();
            this.scheduleExclusionDataGridView = new System.Windows.Forms.DataGridView();
            this.scheduleExclusionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exclusionDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.includeDateDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.scheduleExclusionDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleExclusionDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleExclusionDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gpFilter.Controls.Add(this.txtScheduleName);
            this.gpFilter.Controls.Add(this.lblScheduleName);
            this.gpFilter.Controls.Add(this.scheduleExclusionDataGridView);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(6, 3);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(925, 310);
            this.gpFilter.TabIndex = 18;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Schedule Exclusion";
            // 
            // txtScheduleName
            // 
            this.txtScheduleName.BackColor = System.Drawing.SystemColors.Control;
            this.txtScheduleName.Location = new System.Drawing.Point(154, 18);
            this.txtScheduleName.Name = "txtScheduleName";
            this.txtScheduleName.ReadOnly = true;
            this.txtScheduleName.Size = new System.Drawing.Size(292, 21);
            this.txtScheduleName.TabIndex = 1;
            // 
            // lblScheduleName
            // 
            this.lblScheduleName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblScheduleName.Location = new System.Drawing.Point(9, 16);
            this.lblScheduleName.Name = "lblScheduleName";
            this.lblScheduleName.Size = new System.Drawing.Size(141, 25);
            this.lblScheduleName.TabIndex = 0;
            this.lblScheduleName.Text = "Schedule Name:";
            this.lblScheduleName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // scheduleExclusionDeleteBtn
            // 
            this.scheduleExclusionDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scheduleExclusionDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.scheduleExclusionDeleteBtn.Location = new System.Drawing.Point(269, 326);
            this.scheduleExclusionDeleteBtn.Name = "scheduleExclusionDeleteBtn";
            this.scheduleExclusionDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.scheduleExclusionDeleteBtn.TabIndex = 48;
            this.scheduleExclusionDeleteBtn.Text = "Delete";
            this.scheduleExclusionDeleteBtn.UseVisualStyleBackColor = true;
            this.scheduleExclusionDeleteBtn.Click += new System.EventHandler(this.scheduleExclusionDeleteBtn_Click);
            // 
            // scheduleExclusionCloseBtn
            // 
            this.scheduleExclusionCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scheduleExclusionCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.scheduleExclusionCloseBtn.Location = new System.Drawing.Point(397, 326);
            this.scheduleExclusionCloseBtn.Name = "scheduleExclusionCloseBtn";
            this.scheduleExclusionCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.scheduleExclusionCloseBtn.TabIndex = 47;
            this.scheduleExclusionCloseBtn.Text = "Close";
            this.scheduleExclusionCloseBtn.UseVisualStyleBackColor = true;
            this.scheduleExclusionCloseBtn.Click += new System.EventHandler(this.scheduleExclusionCloseBtn_Click);
            // 
            // scheduleExclusionRefreshBtn
            // 
            this.scheduleExclusionRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scheduleExclusionRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.scheduleExclusionRefreshBtn.Location = new System.Drawing.Point(141, 326);
            this.scheduleExclusionRefreshBtn.Name = "scheduleExclusionRefreshBtn";
            this.scheduleExclusionRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.scheduleExclusionRefreshBtn.TabIndex = 46;
            this.scheduleExclusionRefreshBtn.Text = "Refresh";
            this.scheduleExclusionRefreshBtn.UseVisualStyleBackColor = true;
            this.scheduleExclusionRefreshBtn.Click += new System.EventHandler(this.scheduleExclusionRefreshBtn_Click);
            // 
            // scheduleExclusionSaveBtn
            // 
            this.scheduleExclusionSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scheduleExclusionSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.scheduleExclusionSaveBtn.Location = new System.Drawing.Point(13, 326);
            this.scheduleExclusionSaveBtn.Name = "scheduleExclusionSaveBtn";
            this.scheduleExclusionSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.scheduleExclusionSaveBtn.TabIndex = 45;
            this.scheduleExclusionSaveBtn.Text = "Save";
            this.scheduleExclusionSaveBtn.UseVisualStyleBackColor = true;
            this.scheduleExclusionSaveBtn.Click += new System.EventHandler(this.scheduleExclusionSaveBtn_Click);
            // 
            // scheduleExclusionDataGridView
            // 
            this.scheduleExclusionDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scheduleExclusionDataGridView.AutoGenerateColumns = false;
            this.scheduleExclusionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scheduleExclusionDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.scheduleExclusionIdDataGridViewTextBoxColumn,
            this.scheduleIdDataGridViewTextBoxColumn,
            this.exclusionDateDataGridViewTextBoxColumn,
            this.dayDataGridViewTextBoxColumn,
            this.includeDateDataGridViewCheckBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.scheduleExclusionDataGridView.DataSource = this.scheduleExclusionDTOBindingSource;
            this.scheduleExclusionDataGridView.Location = new System.Drawing.Point(6, 45);
            this.scheduleExclusionDataGridView.Name = "scheduleExclusionDataGridView";
            this.scheduleExclusionDataGridView.Size = new System.Drawing.Size(925, 260);
            this.scheduleExclusionDataGridView.TabIndex = 44;
            this.scheduleExclusionDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.scheduleExclusionDataGridView_DataError);
            this.scheduleExclusionDataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.scheduleExclusionDataGridView_DefaultValuesNeeded);
            // 
            // scheduleExclusionIdDataGridViewTextBoxColumn
            // 
            this.scheduleExclusionIdDataGridViewTextBoxColumn.DataPropertyName = "ScheduleExclusionId";
            this.scheduleExclusionIdDataGridViewTextBoxColumn.HeaderText = "Exclusion Id";
            this.scheduleExclusionIdDataGridViewTextBoxColumn.Name = "scheduleExclusionIdDataGridViewTextBoxColumn";
            this.scheduleExclusionIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // scheduleIdDataGridViewTextBoxColumn
            // 
            this.scheduleIdDataGridViewTextBoxColumn.DataPropertyName = "ScheduleId";
            this.scheduleIdDataGridViewTextBoxColumn.HeaderText = "Schedule Id";
            this.scheduleIdDataGridViewTextBoxColumn.Name = "scheduleIdDataGridViewTextBoxColumn";
            // 
            // exclusionDateDataGridViewTextBoxColumn
            // 
            this.exclusionDateDataGridViewTextBoxColumn.DataPropertyName = "ExclusionDate";
            this.exclusionDateDataGridViewTextBoxColumn.HeaderText = "Exclusion Date";
            this.exclusionDateDataGridViewTextBoxColumn.Name = "exclusionDateDataGridViewTextBoxColumn";
            // 
            // dayDataGridViewTextBoxColumn
            // 
            this.dayDataGridViewTextBoxColumn.DataPropertyName = "Day";
            this.dayDataGridViewTextBoxColumn.HeaderText = "Day";
            this.dayDataGridViewTextBoxColumn.Name = "dayDataGridViewTextBoxColumn";
            this.dayDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dayDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // includeDateDataGridViewCheckBoxColumn
            // 
            this.includeDateDataGridViewCheckBoxColumn.DataPropertyName = "IncludeDate";
            this.includeDateDataGridViewCheckBoxColumn.FalseValue = "False";
            this.includeDateDataGridViewCheckBoxColumn.HeaderText = "Include Date?";
            this.includeDateDataGridViewCheckBoxColumn.Name = "includeDateDataGridViewCheckBoxColumn";
            this.includeDateDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "False";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // scheduleExclusionDTOBindingSource
            // 
            this.scheduleExclusionDTOBindingSource.DataSource = typeof(ScheduleCalendarExclusionDTO);
            // 
            // ScheduleExclusionUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 366);
            this.Controls.Add(this.scheduleExclusionDeleteBtn);
            this.Controls.Add(this.scheduleExclusionCloseBtn);
            this.Controls.Add(this.scheduleExclusionRefreshBtn);
            this.Controls.Add(this.scheduleExclusionSaveBtn);
            this.Controls.Add(this.gpFilter);
            this.Name = "ScheduleExclusionUI";
            this.Text = "Schedule Exclusion";
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleExclusionDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleExclusionDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.TextBox txtScheduleName;
        private System.Windows.Forms.Label lblScheduleName;
        private System.Windows.Forms.Button scheduleExclusionDeleteBtn;
        private System.Windows.Forms.Button scheduleExclusionCloseBtn;
        private System.Windows.Forms.Button scheduleExclusionRefreshBtn;
        private System.Windows.Forms.Button scheduleExclusionSaveBtn;
        private System.Windows.Forms.DataGridView scheduleExclusionDataGridView;
        private System.Windows.Forms.BindingSource scheduleExclusionDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleExclusionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exclusionDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn includeDateDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}