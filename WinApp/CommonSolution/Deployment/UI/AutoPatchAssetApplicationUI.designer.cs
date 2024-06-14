 namespace Semnox.Parafait.Deployment
{
    partial class AutoPatchAssetApplicationUI
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
            this.assetApplicationDeleteBtn = new System.Windows.Forms.Button();
            this.assetApplicationCloseBtn = new System.Windows.Forms.Button();
            this.assetApplicationRefreshBtn = new System.Windows.Forms.Button();
            this.assetApplicationSaveBtn = new System.Windows.Forms.Button();
            this.assetApplicationDataGridView = new System.Windows.Forms.DataGridView();
            this.autoPatchAssetApplDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.patchAssetApplicationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.patchApplicationTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.patchVersionNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.patchUpgradeStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.applicationPathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpgradeDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorCounterDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.assetApplicationDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoPatchAssetApplDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // assetApplicationDeleteBtn
            // 
            this.assetApplicationDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetApplicationDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetApplicationDeleteBtn.Location = new System.Drawing.Point(269, 255);
            this.assetApplicationDeleteBtn.Name = "assetApplicationDeleteBtn";
            this.assetApplicationDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.assetApplicationDeleteBtn.TabIndex = 20;
            this.assetApplicationDeleteBtn.Text = "Delete";
            this.assetApplicationDeleteBtn.UseVisualStyleBackColor = true;
            this.assetApplicationDeleteBtn.Click += new System.EventHandler(this.assetApplicationDeleteBtn_Click);
            // 
            // assetApplicationCloseBtn
            // 
            this.assetApplicationCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetApplicationCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetApplicationCloseBtn.Location = new System.Drawing.Point(397, 255);
            this.assetApplicationCloseBtn.Name = "assetApplicationCloseBtn";
            this.assetApplicationCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.assetApplicationCloseBtn.TabIndex = 21;
            this.assetApplicationCloseBtn.Text = "Close";
            this.assetApplicationCloseBtn.UseVisualStyleBackColor = true;
            this.assetApplicationCloseBtn.Click += new System.EventHandler(this.assetApplicationCloseBtn_Click);
            // 
            // assetApplicationRefreshBtn
            // 
            this.assetApplicationRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetApplicationRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetApplicationRefreshBtn.Location = new System.Drawing.Point(141, 255);
            this.assetApplicationRefreshBtn.Name = "assetApplicationRefreshBtn";
            this.assetApplicationRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.assetApplicationRefreshBtn.TabIndex = 19;
            this.assetApplicationRefreshBtn.Text = "Refresh";
            this.assetApplicationRefreshBtn.UseVisualStyleBackColor = true;
            this.assetApplicationRefreshBtn.Click += new System.EventHandler(this.assetApplicationRefreshBtn_Click);
            // 
            // assetApplicationSaveBtn
            // 
            this.assetApplicationSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetApplicationSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetApplicationSaveBtn.Location = new System.Drawing.Point(13, 255);
            this.assetApplicationSaveBtn.Name = "assetApplicationSaveBtn";
            this.assetApplicationSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.assetApplicationSaveBtn.TabIndex = 18;
            this.assetApplicationSaveBtn.Text = "Save";
            this.assetApplicationSaveBtn.UseVisualStyleBackColor = true;
            this.assetApplicationSaveBtn.Click += new System.EventHandler(this.assetApplicationSaveBtn_Click);
            // 
            // assetApplicationDataGridView
            // 
            this.assetApplicationDataGridView.AllowUserToDeleteRows = false;
            this.assetApplicationDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetApplicationDataGridView.AutoGenerateColumns = false;
            this.assetApplicationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.assetApplicationDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.patchAssetApplicationIdDataGridViewTextBoxColumn,
            this.assetIdDataGridViewTextBoxColumn,
            this.patchApplicationTypeIdDataGridViewTextBoxColumn,
            this.patchVersionNumberDataGridViewTextBoxColumn,
            this.patchUpgradeStatusDataGridViewTextBoxColumn,
            this.applicationPathDataGridViewTextBoxColumn,
            this.lastUpgradeDateDataGridViewTextBoxColumn,
            this.errorCounterDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn});
            this.assetApplicationDataGridView.DataSource = this.autoPatchAssetApplDTOBindingSource;
            this.assetApplicationDataGridView.Location = new System.Drawing.Point(12, 12);
            this.assetApplicationDataGridView.Name = "assetApplicationDataGridView";
            this.assetApplicationDataGridView.Size = new System.Drawing.Size(817, 225);
            this.assetApplicationDataGridView.TabIndex = 17;
            // 
            // autoPatchAssetApplDTOBindingSource
            // 
            this.autoPatchAssetApplDTOBindingSource.DataSource = typeof(AutoPatchAssetApplDTO);
            // 
            // patchAssetApplicationIdDataGridViewTextBoxColumn
            // 
            this.patchAssetApplicationIdDataGridViewTextBoxColumn.DataPropertyName = "PatchAssetApplicationId";
            this.patchAssetApplicationIdDataGridViewTextBoxColumn.HeaderText = "Patch Asset Application Id";
            this.patchAssetApplicationIdDataGridViewTextBoxColumn.Name = "patchAssetApplicationIdDataGridViewTextBoxColumn";
            this.patchAssetApplicationIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // assetIdDataGridViewTextBoxColumn
            // 
            this.assetIdDataGridViewTextBoxColumn.DataPropertyName = "AssetId";
            this.assetIdDataGridViewTextBoxColumn.HeaderText = "Asset";
            this.assetIdDataGridViewTextBoxColumn.Name = "assetIdDataGridViewTextBoxColumn";
            this.assetIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assetIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // patchApplicationTypeIdDataGridViewTextBoxColumn
            // 
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.DataPropertyName = "PatchApplicationTypeId";
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.HeaderText = "Application Type";
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.Name = "patchApplicationTypeIdDataGridViewTextBoxColumn";
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // patchVersionNumberDataGridViewTextBoxColumn
            // 
            this.patchVersionNumberDataGridViewTextBoxColumn.DataPropertyName = "PatchVersionNumber";
            this.patchVersionNumberDataGridViewTextBoxColumn.HeaderText = "Version";
            this.patchVersionNumberDataGridViewTextBoxColumn.Name = "patchVersionNumberDataGridViewTextBoxColumn";
            this.patchVersionNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // patchUpgradeStatusDataGridViewTextBoxColumn
            // 
            this.patchUpgradeStatusDataGridViewTextBoxColumn.DataPropertyName = "PatchUpgradeStatus";
            this.patchUpgradeStatusDataGridViewTextBoxColumn.HeaderText = "Upgrade Status";
            this.patchUpgradeStatusDataGridViewTextBoxColumn.Name = "patchUpgradeStatusDataGridViewTextBoxColumn";
            this.patchUpgradeStatusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // applicationPathDataGridViewTextBoxColumn
            // 
            this.applicationPathDataGridViewTextBoxColumn.DataPropertyName = "ApplicationPath";
            this.applicationPathDataGridViewTextBoxColumn.HeaderText = "Application Path";
            this.applicationPathDataGridViewTextBoxColumn.Name = "applicationPathDataGridViewTextBoxColumn";
            // 
            // lastUpgradeDateDataGridViewTextBoxColumn
            // 
            this.lastUpgradeDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpgradeDate";
            this.lastUpgradeDateDataGridViewTextBoxColumn.HeaderText = "Last Upgraded On";
            this.lastUpgradeDateDataGridViewTextBoxColumn.Name = "lastUpgradeDateDataGridViewTextBoxColumn";
            this.lastUpgradeDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // errorCounterDataGridViewTextBoxColumn
            // 
            this.errorCounterDataGridViewTextBoxColumn.DataPropertyName = "ErrorCounter";
            this.errorCounterDataGridViewTextBoxColumn.HeaderText = "Error Counter";
            this.errorCounterDataGridViewTextBoxColumn.Name = "errorCounterDataGridViewTextBoxColumn";
            this.errorCounterDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isActiveDataGridViewTextBoxColumn
            // 
            this.isActiveDataGridViewTextBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn.FalseValue = "N";
            this.isActiveDataGridViewTextBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewTextBoxColumn.Name = "isActiveDataGridViewTextBoxColumn";
            this.isActiveDataGridViewTextBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isActiveDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // AutoPatchAssetApplicationUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 295);
            this.Controls.Add(this.assetApplicationDeleteBtn);
            this.Controls.Add(this.assetApplicationCloseBtn);
            this.Controls.Add(this.assetApplicationRefreshBtn);
            this.Controls.Add(this.assetApplicationSaveBtn);
            this.Controls.Add(this.assetApplicationDataGridView);
            this.Name = "AutoPatchAssetApplicationUI";
            this.Text = "Asset Application";
            ((System.ComponentModel.ISupportInitialize)(this.assetApplicationDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoPatchAssetApplDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button assetApplicationDeleteBtn;
        private System.Windows.Forms.Button assetApplicationCloseBtn;
        private System.Windows.Forms.Button assetApplicationRefreshBtn;
        private System.Windows.Forms.Button assetApplicationSaveBtn;
        private System.Windows.Forms.DataGridView assetApplicationDataGridView;
        private System.Windows.Forms.BindingSource autoPatchAssetApplDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn patchAssetApplicationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn patchApplicationTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn patchVersionNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn patchUpgradeStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicationPathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpgradeDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn errorCounterDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn;
    }
}