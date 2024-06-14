namespace Semnox.Parafait.Maintenance
{
    partial class AssetGroupAssetMapUI
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
            this.cmbAssetName = new System.Windows.Forms.ComboBox();
            this.lblAssetName = new System.Windows.Forms.Label();
            this.cmbGroupName = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblGroupName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.assetGroupAssetDataGridView = new System.Windows.Forms.DataGridView();
            this.assetGroupAssetDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.assetGroupDeleteBtn = new System.Windows.Forms.Button();
            this.assetGroupCloseBtn = new System.Windows.Forms.Button();
            this.assetGroupRefreshBtn = new System.Windows.Forms.Button();
            this.assetGroupSaveBtn = new System.Windows.Forms.Button();
            this.dgvAssetSearch = new System.Windows.Forms.DataGridView();
            this.txtAssetSearch = new System.Windows.Forms.TextBox();
            this.dgvAssetGroupSearch = new System.Windows.Forms.DataGridView();
            this.txtAssetGroupSearch = new System.Windows.Forms.TextBox();
            this.btnPublishToSite = new System.Windows.Forms.Button();
            this.assetGroupAssetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetGroupIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.assetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.assetGroupAssetDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assetGroupAssetDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssetSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssetGroupSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.cmbAssetName);
            this.gpFilter.Controls.Add(this.lblAssetName);
            this.gpFilter.Controls.Add(this.cmbGroupName);
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.lblGroupName);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(12, 4);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(844, 47);
            this.gpFilter.TabIndex = 15;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // cmbAssetName
            // 
            this.cmbAssetName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssetName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbAssetName.FormattingEnabled = true;
            this.cmbAssetName.Location = new System.Drawing.Point(264, 16);
            this.cmbAssetName.Name = "cmbAssetName";
            this.cmbAssetName.Size = new System.Drawing.Size(136, 23);
            this.cmbAssetName.TabIndex = 6;
            this.cmbAssetName.SelectedIndexChanged += new System.EventHandler(this.cmbAssetName_SelectedIndexChanged);
            // 
            // lblAssetName
            // 
            this.lblAssetName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssetName.Location = new System.Drawing.Point(157, 17);
            this.lblAssetName.Name = "lblAssetName";
            this.lblAssetName.Size = new System.Drawing.Size(101, 20);
            this.lblAssetName.TabIndex = 5;
            this.lblAssetName.Text = "Asset Name:";
            this.lblAssetName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbGroupName
            // 
            this.cmbGroupName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbGroupName.FormattingEnabled = true;
            this.cmbGroupName.Location = new System.Drawing.Point(520, 16);
            this.cmbGroupName.Name = "cmbGroupName";
            this.cmbGroupName.Size = new System.Drawing.Size(136, 23);
            this.cmbGroupName.TabIndex = 4;
            this.cmbGroupName.SelectedIndexChanged += new System.EventHandler(this.cmbGroupName_SelectedIndexChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(668, 16);
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
            this.lblGroupName.Location = new System.Drawing.Point(411, 17);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(103, 20);
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
            this.chbShowActiveEntries.Location = new System.Drawing.Point(12, 18);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 0;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // assetGroupAssetDataGridView
            // 
            this.assetGroupAssetDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetGroupAssetDataGridView.AutoGenerateColumns = false;
            this.assetGroupAssetDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.assetGroupAssetDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.assetGroupAssetIdDataGridViewTextBoxColumn,
            this.assetGroupIdDataGridViewTextBoxColumn,
            this.assetIdDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.assetGroupAssetDataGridView.DataSource = this.assetGroupAssetDTOBindingSource;
            this.assetGroupAssetDataGridView.Location = new System.Drawing.Point(12, 54);
            this.assetGroupAssetDataGridView.Name = "assetGroupAssetDataGridView";
            this.assetGroupAssetDataGridView.Size = new System.Drawing.Size(845, 258);
            this.assetGroupAssetDataGridView.TabIndex = 16;
            this.assetGroupAssetDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.assetGroupAssetDataGridView_DataError);
            // 
            // assetGroupAssetDTOBindingSource
            // 
            this.assetGroupAssetDTOBindingSource.DataSource = typeof(Semnox.Parafait.Maintenance.AssetGroupAssetDTO);
            // 
            // assetGroupDeleteBtn
            // 
            this.assetGroupDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetGroupDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetGroupDeleteBtn.Location = new System.Drawing.Point(269, 320);
            this.assetGroupDeleteBtn.Name = "assetGroupDeleteBtn";
            this.assetGroupDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.assetGroupDeleteBtn.TabIndex = 20;
            this.assetGroupDeleteBtn.Text = "Delete";
            this.assetGroupDeleteBtn.UseVisualStyleBackColor = true;
            this.assetGroupDeleteBtn.Click += new System.EventHandler(this.assetGroupDeleteBtn_Click);
            // 
            // assetGroupCloseBtn
            // 
            this.assetGroupCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetGroupCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetGroupCloseBtn.Location = new System.Drawing.Point(397, 320);
            this.assetGroupCloseBtn.Name = "assetGroupCloseBtn";
            this.assetGroupCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.assetGroupCloseBtn.TabIndex = 19;
            this.assetGroupCloseBtn.Text = "Close";
            this.assetGroupCloseBtn.UseVisualStyleBackColor = true;
            this.assetGroupCloseBtn.Click += new System.EventHandler(this.assetGroupCloseBtn_Click);
            // 
            // assetGroupRefreshBtn
            // 
            this.assetGroupRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetGroupRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetGroupRefreshBtn.Location = new System.Drawing.Point(141, 320);
            this.assetGroupRefreshBtn.Name = "assetGroupRefreshBtn";
            this.assetGroupRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.assetGroupRefreshBtn.TabIndex = 18;
            this.assetGroupRefreshBtn.Text = "Refresh";
            this.assetGroupRefreshBtn.UseVisualStyleBackColor = true;
            this.assetGroupRefreshBtn.Click += new System.EventHandler(this.assetGroupRefreshBtn_Click);
            // 
            // assetGroupSaveBtn
            // 
            this.assetGroupSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetGroupSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetGroupSaveBtn.Location = new System.Drawing.Point(13, 320);
            this.assetGroupSaveBtn.Name = "assetGroupSaveBtn";
            this.assetGroupSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.assetGroupSaveBtn.TabIndex = 17;
            this.assetGroupSaveBtn.Text = "Save";
            this.assetGroupSaveBtn.UseVisualStyleBackColor = true;
            this.assetGroupSaveBtn.Click += new System.EventHandler(this.assetGroupSaveBtn_Click);
            // 
            // dgvAssetSearch
            // 
            this.dgvAssetSearch.AllowUserToAddRows = false;
            this.dgvAssetSearch.AllowUserToDeleteRows = false;
            this.dgvAssetSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvAssetSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAssetSearch.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvAssetSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssetSearch.ColumnHeadersVisible = false;
            this.dgvAssetSearch.Location = new System.Drawing.Point(278, 43);
            this.dgvAssetSearch.Name = "dgvAssetSearch";
            this.dgvAssetSearch.ReadOnly = true;
            this.dgvAssetSearch.RowHeadersVisible = false;
            this.dgvAssetSearch.RowTemplate.Height = 35;
            this.dgvAssetSearch.Size = new System.Drawing.Size(134, 181);
            this.dgvAssetSearch.TabIndex = 20;
            this.dgvAssetSearch.Visible = false;
            this.dgvAssetSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAssetSearch_CellClick);
            this.dgvAssetSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvAssetSearch_KeyDown);
            // 
            // txtAssetSearch
            // 
            this.txtAssetSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.txtAssetSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAssetSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtAssetSearch.Location = new System.Drawing.Point(279, 25);
            this.txtAssetSearch.Name = "txtAssetSearch";
            this.txtAssetSearch.Size = new System.Drawing.Size(115, 14);
            this.txtAssetSearch.TabIndex = 19;
            this.txtAssetSearch.TextChanged += new System.EventHandler(this.txtAssetSearch_TextChanged);
            this.txtAssetSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAssetSearch_KeyDown);
            // 
            // dgvAssetGroupSearch
            // 
            this.dgvAssetGroupSearch.AllowUserToAddRows = false;
            this.dgvAssetGroupSearch.AllowUserToDeleteRows = false;
            this.dgvAssetGroupSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvAssetGroupSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAssetGroupSearch.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvAssetGroupSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssetGroupSearch.ColumnHeadersVisible = false;
            this.dgvAssetGroupSearch.Location = new System.Drawing.Point(534, 43);
            this.dgvAssetGroupSearch.Name = "dgvAssetGroupSearch";
            this.dgvAssetGroupSearch.ReadOnly = true;
            this.dgvAssetGroupSearch.RowHeadersVisible = false;
            this.dgvAssetGroupSearch.RowTemplate.Height = 35;
            this.dgvAssetGroupSearch.Size = new System.Drawing.Size(134, 181);
            this.dgvAssetGroupSearch.TabIndex = 22;
            this.dgvAssetGroupSearch.Visible = false;
            this.dgvAssetGroupSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAssetGroupSearch_CellClick);
            this.dgvAssetGroupSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvAssetGroupSearch_KeyDown);
            // 
            // txtAssetGroupSearch
            // 
            this.txtAssetGroupSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.txtAssetGroupSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAssetGroupSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtAssetGroupSearch.Location = new System.Drawing.Point(536, 25);
            this.txtAssetGroupSearch.Name = "txtAssetGroupSearch";
            this.txtAssetGroupSearch.Size = new System.Drawing.Size(114, 14);
            this.txtAssetGroupSearch.TabIndex = 21;
            this.txtAssetGroupSearch.TextChanged += new System.EventHandler(this.txtAssetGroupSearch_TextChanged);
            this.txtAssetGroupSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAssetGroupSearch_KeyDown);
            // 
            // btnPublishToSite
            // 
            this.btnPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPublishToSite.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPublishToSite.Location = new System.Drawing.Point(532, 320);
            this.btnPublishToSite.Name = "btnPublishToSite";
            this.btnPublishToSite.Size = new System.Drawing.Size(109, 23);
            this.btnPublishToSite.TabIndex = 23;
            this.btnPublishToSite.Text = "Publish To Sites";
            this.btnPublishToSite.UseVisualStyleBackColor = true;
            this.btnPublishToSite.Click += new System.EventHandler(this.btnPublishToSite_Click);
            // 
            // assetGroupAssetIdDataGridViewTextBoxColumn
            // 
            this.assetGroupAssetIdDataGridViewTextBoxColumn.DataPropertyName = "AssetGroupAssetId";
            this.assetGroupAssetIdDataGridViewTextBoxColumn.HeaderText = "Asset Group Asset Id";
            this.assetGroupAssetIdDataGridViewTextBoxColumn.Name = "assetGroupAssetIdDataGridViewTextBoxColumn";
            this.assetGroupAssetIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // assetGroupIdDataGridViewTextBoxColumn
            // 
            this.assetGroupIdDataGridViewTextBoxColumn.DataPropertyName = "AssetGroupId";
            this.assetGroupIdDataGridViewTextBoxColumn.HeaderText = "Asset Group";
            this.assetGroupIdDataGridViewTextBoxColumn.Name = "assetGroupIdDataGridViewTextBoxColumn";
            this.assetGroupIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assetGroupIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // assetIdDataGridViewTextBoxColumn
            // 
            this.assetIdDataGridViewTextBoxColumn.DataPropertyName = "AssetId";
            this.assetIdDataGridViewTextBoxColumn.HeaderText = "Asset";
            this.assetIdDataGridViewTextBoxColumn.Name = "assetIdDataGridViewTextBoxColumn";
            this.assetIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assetIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "false";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // AssetGroupAssetMapUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 361);
            this.Controls.Add(this.btnPublishToSite);
            this.Controls.Add(this.dgvAssetGroupSearch);
            this.Controls.Add(this.txtAssetGroupSearch);
            this.Controls.Add(this.dgvAssetSearch);
            this.Controls.Add(this.txtAssetSearch);
            this.Controls.Add(this.assetGroupDeleteBtn);
            this.Controls.Add(this.assetGroupCloseBtn);
            this.Controls.Add(this.assetGroupRefreshBtn);
            this.Controls.Add(this.assetGroupSaveBtn);
            this.Controls.Add(this.assetGroupAssetDataGridView);
            this.Controls.Add(this.gpFilter);
            this.Name = "AssetGroupAssetMapUI";
            this.Text = "Asset Group Asset";
            this.Load += new System.EventHandler(this.AssetGroupAssetMapUI_Load);
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.assetGroupAssetDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assetGroupAssetDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssetSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssetGroupSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblGroupName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.DataGridView assetGroupAssetDataGridView;
        private System.Windows.Forms.Button assetGroupDeleteBtn;
        private System.Windows.Forms.Button assetGroupCloseBtn;
        private System.Windows.Forms.Button assetGroupRefreshBtn;
        private System.Windows.Forms.Button assetGroupSaveBtn;
        private System.Windows.Forms.ComboBox cmbAssetName;
        private System.Windows.Forms.Label lblAssetName;
        private System.Windows.Forms.ComboBox cmbGroupName;
        private System.Windows.Forms.BindingSource assetGroupAssetDTOBindingSource;
        private System.Windows.Forms.DataGridView dgvAssetSearch;
        private System.Windows.Forms.TextBox txtAssetSearch;
        private System.Windows.Forms.DataGridView dgvAssetGroupSearch;
        private System.Windows.Forms.TextBox txtAssetGroupSearch;
        private System.Windows.Forms.Button btnPublishToSite;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetGroupAssetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetGroupIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}