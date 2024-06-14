namespace Semnox.Parafait.Maintenance
{
    partial class AssetGroupUI
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
            this.assetGroupsDataGridView = new System.Windows.Forms.DataGridView();
            this.assetGroupSaveBtn = new System.Windows.Forms.Button();
            this.assetGroupRefreshBtn = new System.Windows.Forms.Button();
            this.assetGroupCloseBtn = new System.Windows.Forms.Button();
            this.assetGroupDeleteBtn = new System.Windows.Forms.Button();
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnPublishToSite = new System.Windows.Forms.Button();
            this.assetGroupDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.assetGroupIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetGroupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.assetGroupsDataGridView)).BeginInit();
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.assetGroupDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // assetGroupsDataGridView
            // 
            this.assetGroupsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetGroupsDataGridView.AutoGenerateColumns = false;
            this.assetGroupsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.assetGroupsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.assetGroupIdDataGridViewTextBoxColumn,
            this.assetGroupNameDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.assetGroupsDataGridView.DataSource = this.assetGroupDTOBindingSource;
            this.assetGroupsDataGridView.Location = new System.Drawing.Point(9, 50);
            this.assetGroupsDataGridView.Name = "assetGroupsDataGridView";
            this.assetGroupsDataGridView.Size = new System.Drawing.Size(741, 241);
            this.assetGroupsDataGridView.TabIndex = 2;
            this.assetGroupsDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.assetGroupsDataGridView_DataError);
            // 
            // assetGroupSaveBtn
            // 
            this.assetGroupSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetGroupSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetGroupSaveBtn.Location = new System.Drawing.Point(13, 308);
            this.assetGroupSaveBtn.Name = "assetGroupSaveBtn";
            this.assetGroupSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.assetGroupSaveBtn.TabIndex = 3;
            this.assetGroupSaveBtn.Text = "Save";
            this.assetGroupSaveBtn.UseVisualStyleBackColor = true;
            this.assetGroupSaveBtn.Click += new System.EventHandler(this.assetGroupSave_Click);
            // 
            // assetGroupRefreshBtn
            // 
            this.assetGroupRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetGroupRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetGroupRefreshBtn.Location = new System.Drawing.Point(141, 308);
            this.assetGroupRefreshBtn.Name = "assetGroupRefreshBtn";
            this.assetGroupRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.assetGroupRefreshBtn.TabIndex = 4;
            this.assetGroupRefreshBtn.Text = "Refresh";
            this.assetGroupRefreshBtn.UseVisualStyleBackColor = true;
            this.assetGroupRefreshBtn.Click += new System.EventHandler(this.assetGroupRefreshBtn_Click);
            // 
            // assetGroupCloseBtn
            // 
            this.assetGroupCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetGroupCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetGroupCloseBtn.Location = new System.Drawing.Point(397, 308);
            this.assetGroupCloseBtn.Name = "assetGroupCloseBtn";
            this.assetGroupCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.assetGroupCloseBtn.TabIndex = 5;
            this.assetGroupCloseBtn.Text = "Close";
            this.assetGroupCloseBtn.UseVisualStyleBackColor = true;
            this.assetGroupCloseBtn.Click += new System.EventHandler(this.assetGroupCloseBtn_Click);
            // 
            // assetGroupDeleteBtn
            // 
            this.assetGroupDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetGroupDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetGroupDeleteBtn.Location = new System.Drawing.Point(269, 308);
            this.assetGroupDeleteBtn.Name = "assetGroupDeleteBtn";
            this.assetGroupDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.assetGroupDeleteBtn.TabIndex = 8;
            this.assetGroupDeleteBtn.Text = "Delete";
            this.assetGroupDeleteBtn.UseVisualStyleBackColor = true;
            this.assetGroupDeleteBtn.Click += new System.EventHandler(this.assetGroupDeleteBtn_Click);
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.txtName);
            this.gpFilter.Controls.Add(this.lblName);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(9, 3);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(741, 44);
            this.gpFilter.TabIndex = 14;
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
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(269, 15);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 2;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(154, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(109, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Group Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(12, 16);
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
            this.btnPublishToSite.Location = new System.Drawing.Point(520, 308);
            this.btnPublishToSite.Name = "btnPublishToSite";
            this.btnPublishToSite.Size = new System.Drawing.Size(109, 23);
            this.btnPublishToSite.TabIndex = 15;
            this.btnPublishToSite.Text = "Publish To Sites";
            this.btnPublishToSite.UseVisualStyleBackColor = true;
            this.btnPublishToSite.Click += new System.EventHandler(this.btnPublishToSite_Click);
            // 
            // assetGroupDTOBindingSource
            // 
            this.assetGroupDTOBindingSource.DataSource = typeof(Semnox.Parafait.Maintenance.AssetGroupDTO);
            // 
            // assetGroupIdDataGridViewTextBoxColumn
            // 
            this.assetGroupIdDataGridViewTextBoxColumn.DataPropertyName = "AssetGroupId";
            this.assetGroupIdDataGridViewTextBoxColumn.HeaderText = "Group Id";
            this.assetGroupIdDataGridViewTextBoxColumn.Name = "assetGroupIdDataGridViewTextBoxColumn";
            this.assetGroupIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // assetGroupNameDataGridViewTextBoxColumn
            // 
            this.assetGroupNameDataGridViewTextBoxColumn.DataPropertyName = "AssetGroupName";
            this.assetGroupNameDataGridViewTextBoxColumn.HeaderText = "Group Name";
            this.assetGroupNameDataGridViewTextBoxColumn.Name = "assetGroupNameDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "false";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // AssetGroupUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 348);
            this.Controls.Add(this.btnPublishToSite);
            this.Controls.Add(this.gpFilter);
            this.Controls.Add(this.assetGroupDeleteBtn);
            this.Controls.Add(this.assetGroupCloseBtn);
            this.Controls.Add(this.assetGroupRefreshBtn);
            this.Controls.Add(this.assetGroupSaveBtn);
            this.Controls.Add(this.assetGroupsDataGridView);
            this.Name = "AssetGroupUI";
            this.Text = "Asset Group";
            this.Load += new System.EventHandler(this.AssetGroupUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.assetGroupsDataGridView)).EndInit();
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.assetGroupDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView assetGroupsDataGridView;
        private System.Windows.Forms.Button assetGroupSaveBtn;
        private System.Windows.Forms.Button assetGroupRefreshBtn;
        private System.Windows.Forms.Button assetGroupCloseBtn;
        private System.Windows.Forms.Button assetGroupDeleteBtn;
        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.BindingSource assetGroupDTOBindingSource;
        private System.Windows.Forms.Button btnPublishToSite;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetGroupIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetGroupNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}

