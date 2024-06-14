namespace Semnox.Parafait.Maintenance
{
    partial class MaintenanceGroupUI
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
            this.maintenanceTaskGroupDeleteBtn = new System.Windows.Forms.Button();
            this.maintenanceTaskGroupCloseBtn = new System.Windows.Forms.Button();
            this.maintenanceTaskGroupRefreshBtn = new System.Windows.Forms.Button();
            this.maintenanceTaskGroupSaveBtn = new System.Windows.Forms.Button();
            this.maintenanceTaskGroupDataGridView = new System.Windows.Forms.DataGridView();
            this.maintenanceTaskGroupDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblAssetName = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnPublishToSite = new System.Windows.Forms.Button();
            this.maintTaskGroupIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskGroupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceTaskGroupDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceTaskGroupDTOBindingSource)).BeginInit();
            this.gpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // maintenanceTaskGroupDeleteBtn
            // 
            this.maintenanceTaskGroupDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maintenanceTaskGroupDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.maintenanceTaskGroupDeleteBtn.Location = new System.Drawing.Point(269, 228);
            this.maintenanceTaskGroupDeleteBtn.Name = "maintenanceTaskGroupDeleteBtn";
            this.maintenanceTaskGroupDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.maintenanceTaskGroupDeleteBtn.TabIndex = 38;
            this.maintenanceTaskGroupDeleteBtn.Text = "Delete";
            this.maintenanceTaskGroupDeleteBtn.UseVisualStyleBackColor = true;
            this.maintenanceTaskGroupDeleteBtn.Click += new System.EventHandler(this.maintenanceTaskGroupDeleteBtn_Click);
            // 
            // maintenanceTaskGroupCloseBtn
            // 
            this.maintenanceTaskGroupCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maintenanceTaskGroupCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.maintenanceTaskGroupCloseBtn.Location = new System.Drawing.Point(397, 228);
            this.maintenanceTaskGroupCloseBtn.Name = "maintenanceTaskGroupCloseBtn";
            this.maintenanceTaskGroupCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.maintenanceTaskGroupCloseBtn.TabIndex = 37;
            this.maintenanceTaskGroupCloseBtn.Text = "Close";
            this.maintenanceTaskGroupCloseBtn.UseVisualStyleBackColor = true;
            this.maintenanceTaskGroupCloseBtn.Click += new System.EventHandler(this.maintenanceTaskGroupCloseBtn_Click);
            // 
            // maintenanceTaskGroupRefreshBtn
            // 
            this.maintenanceTaskGroupRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maintenanceTaskGroupRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.maintenanceTaskGroupRefreshBtn.Location = new System.Drawing.Point(141, 228);
            this.maintenanceTaskGroupRefreshBtn.Name = "maintenanceTaskGroupRefreshBtn";
            this.maintenanceTaskGroupRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.maintenanceTaskGroupRefreshBtn.TabIndex = 36;
            this.maintenanceTaskGroupRefreshBtn.Text = "Refresh";
            this.maintenanceTaskGroupRefreshBtn.UseVisualStyleBackColor = true;
            this.maintenanceTaskGroupRefreshBtn.Click += new System.EventHandler(this.maintenanceTaskGroupRefreshBtn_Click);
            // 
            // maintenanceTaskGroupSaveBtn
            // 
            this.maintenanceTaskGroupSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maintenanceTaskGroupSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.maintenanceTaskGroupSaveBtn.Location = new System.Drawing.Point(13, 228);
            this.maintenanceTaskGroupSaveBtn.Name = "maintenanceTaskGroupSaveBtn";
            this.maintenanceTaskGroupSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.maintenanceTaskGroupSaveBtn.TabIndex = 35;
            this.maintenanceTaskGroupSaveBtn.Text = "Save";
            this.maintenanceTaskGroupSaveBtn.UseVisualStyleBackColor = true;
            this.maintenanceTaskGroupSaveBtn.Click += new System.EventHandler(this.maintenanceTaskGroupSaveBtn_Click);
            // 
            // maintenanceTaskGroupDataGridView
            // 
            this.maintenanceTaskGroupDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maintenanceTaskGroupDataGridView.AutoGenerateColumns = false;
            this.maintenanceTaskGroupDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.maintenanceTaskGroupDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.maintTaskGroupIdDataGridViewTextBoxColumn,
            this.taskGroupNameDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.maintenanceTaskGroupDataGridView.DataSource = this.maintenanceTaskGroupDTOBindingSource;
            this.maintenanceTaskGroupDataGridView.Location = new System.Drawing.Point(9, 62);
            this.maintenanceTaskGroupDataGridView.Name = "maintenanceTaskGroupDataGridView";
            this.maintenanceTaskGroupDataGridView.Size = new System.Drawing.Size(745, 156);
            this.maintenanceTaskGroupDataGridView.TabIndex = 34;
            this.maintenanceTaskGroupDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.maintenanceTaskGroupDataGridView_DataError);
            // 
            // maintenanceTaskGroupDTOBindingSource
            // 
            this.maintenanceTaskGroupDTOBindingSource.DataSource = typeof(Semnox.Parafait.Maintenance.JobTaskGroupDTO);
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.txtName);
            this.gpFilter.Controls.Add(this.lblAssetName);
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(9, 4);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(745, 53);
            this.gpFilter.TabIndex = 17;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(261, 20);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 6;
            // 
            // lblAssetName
            // 
            this.lblAssetName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssetName.Location = new System.Drawing.Point(162, 20);
            this.lblAssetName.Name = "lblAssetName";
            this.lblAssetName.Size = new System.Drawing.Size(93, 20);
            this.lblAssetName.TabIndex = 5;
            this.lblAssetName.Text = "Group Name:";
            this.lblAssetName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(412, 19);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 22);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(12, 22);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 0;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // btnPublishToSite
            // 
            this.btnPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPublishToSite.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPublishToSite.Location = new System.Drawing.Point(523, 228);
            this.btnPublishToSite.Name = "btnPublishToSite";
            this.btnPublishToSite.Size = new System.Drawing.Size(109, 23);
            this.btnPublishToSite.TabIndex = 39;
            this.btnPublishToSite.Text = "Publish To Sites";
            this.btnPublishToSite.UseVisualStyleBackColor = true;
            this.btnPublishToSite.Click += new System.EventHandler(this.btnPublishToSite_Click);
            // 
            // maintTaskGroupIdDataGridViewTextBoxColumn
            // 
            this.maintTaskGroupIdDataGridViewTextBoxColumn.DataPropertyName = "JobTaskGroupId";
            this.maintTaskGroupIdDataGridViewTextBoxColumn.HeaderText = "Group Id";
            this.maintTaskGroupIdDataGridViewTextBoxColumn.Name = "maintTaskGroupIdDataGridViewTextBoxColumn";
            this.maintTaskGroupIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // taskGroupNameDataGridViewTextBoxColumn
            // 
            this.taskGroupNameDataGridViewTextBoxColumn.DataPropertyName = "TaskGroupName";
            this.taskGroupNameDataGridViewTextBoxColumn.HeaderText = "Group Name";
            this.taskGroupNameDataGridViewTextBoxColumn.Name = "taskGroupNameDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "False";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // MaintenanceGroupUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 268);
            this.Controls.Add(this.btnPublishToSite);
            this.Controls.Add(this.maintenanceTaskGroupDeleteBtn);
            this.Controls.Add(this.maintenanceTaskGroupCloseBtn);
            this.Controls.Add(this.maintenanceTaskGroupRefreshBtn);
            this.Controls.Add(this.maintenanceTaskGroupSaveBtn);
            this.Controls.Add(this.maintenanceTaskGroupDataGridView);
            this.Controls.Add(this.gpFilter);
            this.Name = "MaintenanceGroupUI";
            this.Text = "Maintenance Task Group";
            this.Load += new System.EventHandler(this.MaintenanceGroupUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceTaskGroupDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceTaskGroupDTOBindingSource)).EndInit();
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button maintenanceTaskGroupDeleteBtn;
        private System.Windows.Forms.Button maintenanceTaskGroupCloseBtn;
        private System.Windows.Forms.Button maintenanceTaskGroupRefreshBtn;
        private System.Windows.Forms.Button maintenanceTaskGroupSaveBtn;
        private System.Windows.Forms.DataGridView maintenanceTaskGroupDataGridView;
        private System.Windows.Forms.BindingSource maintenanceTaskGroupDTOBindingSource;
        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblAssetName;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Button btnPublishToSite;
        private System.Windows.Forms.DataGridViewTextBoxColumn maintTaskGroupIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskGroupNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}