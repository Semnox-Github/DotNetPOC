namespace Semnox.Parafait.Maintenance
{
    partial class AssetTypeUI
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
            this.assetTypeDataGridView = new System.Windows.Forms.DataGridView();
            this.assetTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.assetTypeDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.assetTypeDeleteBtn = new System.Windows.Forms.Button();
            this.assetTypeCloseBtn = new System.Windows.Forms.Button();
            this.assetTypeRefreshBtn = new System.Windows.Forms.Button();
            this.assetTypeSaveBtn = new System.Windows.Forms.Button();
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnPublishToSite = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.assetTypeDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assetTypeDTOBindingSource)).BeginInit();
            this.gpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // assetTypeDataGridView
            // 
            this.assetTypeDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetTypeDataGridView.AutoGenerateColumns = false;
            this.assetTypeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.assetTypeDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.assetTypeIdDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.assetTypeDataGridView.DataSource = this.assetTypeDTOBindingSource;
            this.assetTypeDataGridView.Location = new System.Drawing.Point(12, 56);
            this.assetTypeDataGridView.Name = "assetTypeDataGridView";
            this.assetTypeDataGridView.Size = new System.Drawing.Size(743, 201);
            this.assetTypeDataGridView.TabIndex = 0;
            this.assetTypeDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.assetTypeDataGridView_DataError);
            // 
            // assetTypeIdDataGridViewTextBoxColumn
            // 
            this.assetTypeIdDataGridViewTextBoxColumn.DataPropertyName = "AssetTypeId";
            this.assetTypeIdDataGridViewTextBoxColumn.HeaderText = "Type Id";
            this.assetTypeIdDataGridViewTextBoxColumn.Name = "assetTypeIdDataGridViewTextBoxColumn";
            this.assetTypeIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Type Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "false";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // assetTypeDTOBindingSource
            // 
            this.assetTypeDTOBindingSource.DataSource = typeof(AssetTypeDTO);
            // 
            // assetTypeDeleteBtn
            // 
            this.assetTypeDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetTypeDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetTypeDeleteBtn.Location = new System.Drawing.Point(269, 273);
            this.assetTypeDeleteBtn.Name = "assetTypeDeleteBtn";
            this.assetTypeDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.assetTypeDeleteBtn.TabIndex = 12;
            this.assetTypeDeleteBtn.Text = "Delete";
            this.assetTypeDeleteBtn.UseVisualStyleBackColor = true;
            this.assetTypeDeleteBtn.Click += new System.EventHandler(this.assetTypeDeleteBtn_Click);
            // 
            // assetTypeCloseBtn
            // 
            this.assetTypeCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetTypeCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetTypeCloseBtn.Location = new System.Drawing.Point(397, 273);
            this.assetTypeCloseBtn.Name = "assetTypeCloseBtn";
            this.assetTypeCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.assetTypeCloseBtn.TabIndex = 11;
            this.assetTypeCloseBtn.Text = "Close";
            this.assetTypeCloseBtn.UseVisualStyleBackColor = true;
            this.assetTypeCloseBtn.Click += new System.EventHandler(this.assetTypeCloseBtn_Click);
            // 
            // assetTypeRefreshBtn
            // 
            this.assetTypeRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetTypeRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetTypeRefreshBtn.Location = new System.Drawing.Point(141, 273);
            this.assetTypeRefreshBtn.Name = "assetTypeRefreshBtn";
            this.assetTypeRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.assetTypeRefreshBtn.TabIndex = 10;
            this.assetTypeRefreshBtn.Text = "Refresh";
            this.assetTypeRefreshBtn.UseVisualStyleBackColor = true;
            this.assetTypeRefreshBtn.Click += new System.EventHandler(this.assetTypeRefreshBtn_Click);
            // 
            // assetTypeSaveBtn
            // 
            this.assetTypeSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetTypeSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetTypeSaveBtn.Location = new System.Drawing.Point(13, 273);
            this.assetTypeSaveBtn.Name = "assetTypeSaveBtn";
            this.assetTypeSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.assetTypeSaveBtn.TabIndex = 9;
            this.assetTypeSaveBtn.Text = "Save";
            this.assetTypeSaveBtn.UseVisualStyleBackColor = true;
            this.assetTypeSaveBtn.Click += new System.EventHandler(this.assetTypeSaveBtn_Click);
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
            this.gpFilter.Location = new System.Drawing.Point(12, 3);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(743, 47);
            this.gpFilter.TabIndex = 13;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(395, 16);
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
            this.txtName.Location = new System.Drawing.Point(253, 17);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 2;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(151, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(96, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // btnPublishToSite
            // 
            this.btnPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPublishToSite.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPublishToSite.Location = new System.Drawing.Point(525, 273);
            this.btnPublishToSite.Name = "btnPublishToSite";
            this.btnPublishToSite.Size = new System.Drawing.Size(109, 23);
            this.btnPublishToSite.TabIndex = 14;
            this.btnPublishToSite.Text = "Publish To Sites";
            this.btnPublishToSite.UseVisualStyleBackColor = true;
            this.btnPublishToSite.Click += new System.EventHandler(this.btnPublishToSite_Click);
            // 
            // AssetTypeUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 313);
            this.Controls.Add(this.btnPublishToSite);
            this.Controls.Add(this.gpFilter);
            this.Controls.Add(this.assetTypeDeleteBtn);
            this.Controls.Add(this.assetTypeCloseBtn);
            this.Controls.Add(this.assetTypeRefreshBtn);
            this.Controls.Add(this.assetTypeSaveBtn);
            this.Controls.Add(this.assetTypeDataGridView);
            this.Name = "AssetTypeUI";
            this.Text = "Asset Type";
            this.Load += new System.EventHandler(this.AssetTypeUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.assetTypeDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assetTypeDTOBindingSource)).EndInit();
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView assetTypeDataGridView;
        private System.Windows.Forms.BindingSource assetTypeDTOBindingSource;
        private System.Windows.Forms.Button assetTypeDeleteBtn;
        private System.Windows.Forms.Button assetTypeCloseBtn;
        private System.Windows.Forms.Button assetTypeRefreshBtn;
        private System.Windows.Forms.Button assetTypeSaveBtn;
        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.Button btnPublishToSite;
    }
}