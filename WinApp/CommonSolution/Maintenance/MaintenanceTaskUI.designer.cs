namespace Semnox.Parafait.Maintenance
{
    partial class MaintenanceTaskUI
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblAssetName = new System.Windows.Forms.Label();
            this.cmbGroupName = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblGroupName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.maintenanceTaskDataGridView = new System.Windows.Forms.DataGridView();
            this.maintTaskIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maintTaskGroupIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.validateTagDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksMandatoryDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.maintenanceTaskDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.maintenanceTaskGroupDeleteBtn = new System.Windows.Forms.Button();
            this.maintenanceTaskGroupCloseBtn = new System.Windows.Forms.Button();
            this.maintenanceTaskGroupRefreshBtn = new System.Windows.Forms.Button();
            this.maintenanceTaskGroupSaveBtn = new System.Windows.Forms.Button();
            this.dgvGroupSearch = new System.Windows.Forms.DataGridView();
            this.txtGroupSearch = new System.Windows.Forms.TextBox();
            this.btnPublishToSite = new System.Windows.Forms.Button();
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceTaskDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceTaskDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.txtName);
            this.gpFilter.Controls.Add(this.lblAssetName);
            this.gpFilter.Controls.Add(this.cmbGroupName);
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.lblGroupName);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(12, 3);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(905, 46);
            this.gpFilter.TabIndex = 16;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(218, 15);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 6;
            // 
            // lblAssetName
            // 
            this.lblAssetName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssetName.Location = new System.Drawing.Point(150, 15);
            this.lblAssetName.Name = "lblAssetName";
            this.lblAssetName.Size = new System.Drawing.Size(62, 20);
            this.lblAssetName.TabIndex = 5;
            this.lblAssetName.Text = "Name:";
            this.lblAssetName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbGroupName
            // 
            this.cmbGroupName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbGroupName.FormattingEnabled = true;
            this.cmbGroupName.Location = new System.Drawing.Point(473, 15);
            this.cmbGroupName.Name = "cmbGroupName";
            this.cmbGroupName.Size = new System.Drawing.Size(136, 23);
            this.cmbGroupName.TabIndex = 4;
            this.cmbGroupName.SelectedValueChanged += new System.EventHandler(this.cmbGroupName_SelectedValueChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(618, 14);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblGroupName
            // 
            this.lblGroupName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblGroupName.Location = new System.Drawing.Point(362, 15);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(105, 20);
            this.lblGroupName.TabIndex = 1;
            this.lblGroupName.Text = "Group Name:";
            this.lblGroupName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(12, 17);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 0;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // maintenanceTaskDataGridView
            // 
            this.maintenanceTaskDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maintenanceTaskDataGridView.AutoGenerateColumns = false;
            this.maintenanceTaskDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.maintenanceTaskDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.maintTaskIdDataGridViewTextBoxColumn,
            this.taskNameDataGridViewTextBoxColumn,
            this.maintTaskGroupIdDataGridViewTextBoxColumn,
            this.validateTagDataGridViewCheckBoxColumn,
            this.cardNumberDataGridViewTextBoxColumn,
            this.remarksMandatoryDataGridViewCheckBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.maintenanceTaskDataGridView.DataSource = this.maintenanceTaskDTOBindingSource;
            this.maintenanceTaskDataGridView.Location = new System.Drawing.Point(12, 52);
            this.maintenanceTaskDataGridView.Name = "maintenanceTaskDataGridView";
            this.maintenanceTaskDataGridView.Size = new System.Drawing.Size(905, 148);
            this.maintenanceTaskDataGridView.TabIndex = 17;
            this.maintenanceTaskDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.maintenanceTaskDataGridView_DataError);
            // 
            // maintTaskIdDataGridViewTextBoxColumn
            // 
            this.maintTaskIdDataGridViewTextBoxColumn.DataPropertyName = "JobTaskId";
            this.maintTaskIdDataGridViewTextBoxColumn.HeaderText = "Task Id";
            this.maintTaskIdDataGridViewTextBoxColumn.Name = "maintTaskIdDataGridViewTextBoxColumn";
            this.maintTaskIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // taskNameDataGridViewTextBoxColumn
            // 
            this.taskNameDataGridViewTextBoxColumn.DataPropertyName = "TaskName";
            this.taskNameDataGridViewTextBoxColumn.HeaderText = "Task Name";
            this.taskNameDataGridViewTextBoxColumn.Name = "taskNameDataGridViewTextBoxColumn";
            // 
            // maintTaskGroupIdDataGridViewTextBoxColumn
            // 
            this.maintTaskGroupIdDataGridViewTextBoxColumn.DataPropertyName = "JobTaskGroupId";
            this.maintTaskGroupIdDataGridViewTextBoxColumn.HeaderText = "Task Group";
            this.maintTaskGroupIdDataGridViewTextBoxColumn.Name = "maintTaskGroupIdDataGridViewTextBoxColumn";
            this.maintTaskGroupIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.maintTaskGroupIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // validateTagDataGridViewCheckBoxColumn
            // 
            this.validateTagDataGridViewCheckBoxColumn.DataPropertyName = "ValidateTag";
            this.validateTagDataGridViewCheckBoxColumn.FalseValue = "False";
            this.validateTagDataGridViewCheckBoxColumn.HeaderText = "Validate Tag?";
            this.validateTagDataGridViewCheckBoxColumn.Name = "validateTagDataGridViewCheckBoxColumn";
            this.validateTagDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // cardNumberDataGridViewTextBoxColumn
            // 
            this.cardNumberDataGridViewTextBoxColumn.DataPropertyName = "CardNumber";
            this.cardNumberDataGridViewTextBoxColumn.HeaderText = "Card Number";
            this.cardNumberDataGridViewTextBoxColumn.Name = "cardNumberDataGridViewTextBoxColumn";
            // 
            // remarksMandatoryDataGridViewCheckBoxColumn
            // 
            this.remarksMandatoryDataGridViewCheckBoxColumn.DataPropertyName = "RemarksMandatory";
            this.remarksMandatoryDataGridViewCheckBoxColumn.FalseValue = "False";
            this.remarksMandatoryDataGridViewCheckBoxColumn.HeaderText = "Remarks Mandatory?";
            this.remarksMandatoryDataGridViewCheckBoxColumn.Name = "remarksMandatoryDataGridViewCheckBoxColumn";
            this.remarksMandatoryDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "False";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // maintenanceTaskDTOBindingSource
            // 
            this.maintenanceTaskDTOBindingSource.DataSource = typeof(JobTaskDTO);
            // 
            // maintenanceTaskGroupDeleteBtn
            // 
            this.maintenanceTaskGroupDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maintenanceTaskGroupDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.maintenanceTaskGroupDeleteBtn.Location = new System.Drawing.Point(269, 209);
            this.maintenanceTaskGroupDeleteBtn.Name = "maintenanceTaskGroupDeleteBtn";
            this.maintenanceTaskGroupDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.maintenanceTaskGroupDeleteBtn.TabIndex = 28;
            this.maintenanceTaskGroupDeleteBtn.Text = "Delete";
            this.maintenanceTaskGroupDeleteBtn.UseVisualStyleBackColor = true;
            this.maintenanceTaskGroupDeleteBtn.Click += new System.EventHandler(this.maintenanceTaskGroupDeleteBtn_Click);
            // 
            // maintenanceTaskGroupCloseBtn
            // 
            this.maintenanceTaskGroupCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maintenanceTaskGroupCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.maintenanceTaskGroupCloseBtn.Location = new System.Drawing.Point(397, 209);
            this.maintenanceTaskGroupCloseBtn.Name = "maintenanceTaskGroupCloseBtn";
            this.maintenanceTaskGroupCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.maintenanceTaskGroupCloseBtn.TabIndex = 27;
            this.maintenanceTaskGroupCloseBtn.Text = "Close";
            this.maintenanceTaskGroupCloseBtn.UseVisualStyleBackColor = true;
            this.maintenanceTaskGroupCloseBtn.Click += new System.EventHandler(this.maintenanceTaskGroupCloseBtn_Click);
            // 
            // maintenanceTaskGroupRefreshBtn
            // 
            this.maintenanceTaskGroupRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maintenanceTaskGroupRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.maintenanceTaskGroupRefreshBtn.Location = new System.Drawing.Point(141, 209);
            this.maintenanceTaskGroupRefreshBtn.Name = "maintenanceTaskGroupRefreshBtn";
            this.maintenanceTaskGroupRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.maintenanceTaskGroupRefreshBtn.TabIndex = 26;
            this.maintenanceTaskGroupRefreshBtn.Text = "Refresh";
            this.maintenanceTaskGroupRefreshBtn.UseVisualStyleBackColor = true;
            this.maintenanceTaskGroupRefreshBtn.Click += new System.EventHandler(this.maintenanceTaskGroupRefreshBtn_Click);
            // 
            // maintenanceTaskGroupSaveBtn
            // 
            this.maintenanceTaskGroupSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maintenanceTaskGroupSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.maintenanceTaskGroupSaveBtn.Location = new System.Drawing.Point(13, 209);
            this.maintenanceTaskGroupSaveBtn.Name = "maintenanceTaskGroupSaveBtn";
            this.maintenanceTaskGroupSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.maintenanceTaskGroupSaveBtn.TabIndex = 25;
            this.maintenanceTaskGroupSaveBtn.Text = "Save";
            this.maintenanceTaskGroupSaveBtn.UseVisualStyleBackColor = true;
            this.maintenanceTaskGroupSaveBtn.Click += new System.EventHandler(this.maintenanceTaskSaveBtn_Click);
            // 
            // dgvGroupSearch
            // 
            this.dgvGroupSearch.AllowUserToAddRows = false;
            this.dgvGroupSearch.AllowUserToDeleteRows = false;
            this.dgvGroupSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvGroupSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGroupSearch.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvGroupSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGroupSearch.ColumnHeadersVisible = false;
            this.dgvGroupSearch.Location = new System.Drawing.Point(485, 42);
            this.dgvGroupSearch.Name = "dgvGroupSearch";
            this.dgvGroupSearch.ReadOnly = true;
            this.dgvGroupSearch.RowHeadersVisible = false;
            this.dgvGroupSearch.RowTemplate.Height = 35;
            this.dgvGroupSearch.Size = new System.Drawing.Size(139, 181);
            this.dgvGroupSearch.TabIndex = 66;
            this.dgvGroupSearch.Visible = false;
            this.dgvGroupSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGroupSearch_CellClick);
            this.dgvGroupSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvGroupSearch_KeyDown);
            // 
            // txtGroupSearch
            // 
            this.txtGroupSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.txtGroupSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGroupSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtGroupSearch.Location = new System.Drawing.Point(489, 23);
            this.txtGroupSearch.Name = "txtGroupSearch";
            this.txtGroupSearch.Size = new System.Drawing.Size(113, 14);
            this.txtGroupSearch.TabIndex = 65;
            this.txtGroupSearch.TextChanged += new System.EventHandler(this.txtGroupSearch_TextChanged);
            this.txtGroupSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGroupSearch_KeyDown);
            // 
            // btnPublishToSite
            // 
            this.btnPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPublishToSite.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPublishToSite.Location = new System.Drawing.Point(525, 209);
            this.btnPublishToSite.Name = "btnPublishToSite";
            this.btnPublishToSite.Size = new System.Drawing.Size(109, 23);
            this.btnPublishToSite.TabIndex = 67;
            this.btnPublishToSite.Text = "Publish To Sites";
            this.btnPublishToSite.UseVisualStyleBackColor = true;
            this.btnPublishToSite.Click += new System.EventHandler(this.btnPublishToSite_Click);
            // 
            // MaintenanceTaskUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 249);
            this.Controls.Add(this.dgvGroupSearch);
            this.Controls.Add(this.txtGroupSearch);
            this.Controls.Add(this.maintenanceTaskGroupDeleteBtn);
            this.Controls.Add(this.maintenanceTaskGroupCloseBtn);
            this.Controls.Add(this.maintenanceTaskGroupRefreshBtn);
            this.Controls.Add(this.maintenanceTaskGroupSaveBtn);
            this.Controls.Add(this.maintenanceTaskDataGridView);
            this.Controls.Add(this.gpFilter);
            this.Controls.Add(this.btnPublishToSite);
            this.Name = "MaintenanceTaskUI";
            this.Text = "Maintenance Task";
            this.Load += new System.EventHandler(this.MaintenanceTaskUI_Load);
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceTaskDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceTaskDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblAssetName;
        private System.Windows.Forms.ComboBox cmbGroupName;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblGroupName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.DataGridView maintenanceTaskDataGridView;        
        private System.Windows.Forms.Button maintenanceTaskGroupDeleteBtn;
        private System.Windows.Forms.Button maintenanceTaskGroupCloseBtn;
        private System.Windows.Forms.Button maintenanceTaskGroupRefreshBtn;
        private System.Windows.Forms.Button maintenanceTaskGroupSaveBtn;
        private System.Windows.Forms.BindingSource maintenanceTaskDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn maintTaskIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn maintTaskGroupIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn validateTagDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn remarksMandatoryDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridView dgvGroupSearch;
        private System.Windows.Forms.TextBox txtGroupSearch;
        private System.Windows.Forms.Button btnPublishToSite;
    }
}