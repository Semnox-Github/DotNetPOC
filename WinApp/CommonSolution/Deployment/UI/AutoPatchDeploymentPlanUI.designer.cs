namespace Semnox.Parafait.Deployment
{
    partial class AutoPatchDeploymentPlanUI
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
            this.deploymentPlanDataGridView = new System.Windows.Forms.DataGridView();
            this.patchDeploymentPlanIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deploymentPlanNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deploymentPlannedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.patchFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deploymentStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.autoPatchDepPlanDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.deploymentPlanDeleteBtn = new System.Windows.Forms.Button();
            this.deploymentPlanCloseBtn = new System.Windows.Forms.Button();
            this.deploymentPlanRefreshBtn = new System.Windows.Forms.Button();
            this.deploymentPlanSaveBtn = new System.Windows.Forms.Button();
            this.btnApplyPatchToSite = new System.Windows.Forms.Button();
            this.deploymentPlanApplicationDataGridView = new System.Windows.Forms.DataGridView();
            this.patchDeploymentPlanApplicationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.patchDeploymentPlanIdApplDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.patchApplicationTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.deploymentVersionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minimumVersionRequiredDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.upgradeTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isActiveDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.autoPatchDepPlanApplicationDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gpDeploymentPlan = new System.Windows.Forms.GroupBox();
            this.gpApplication = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.deploymentPlanDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoPatchDepPlanDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deploymentPlanApplicationDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoPatchDepPlanApplicationDTOBindingSource)).BeginInit();
            this.gpDeploymentPlan.SuspendLayout();
            this.gpApplication.SuspendLayout();
            this.SuspendLayout();
            // 
            // deploymentPlanDataGridView
            // 
            this.deploymentPlanDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deploymentPlanDataGridView.AutoGenerateColumns = false;
            this.deploymentPlanDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.deploymentPlanDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.patchDeploymentPlanIdDataGridViewTextBoxColumn,
            this.deploymentPlanNameDataGridViewTextBoxColumn,
            this.deploymentPlannedDateDataGridViewTextBoxColumn,
            this.patchFileNameDataGridViewTextBoxColumn,
            this.deploymentStatusDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn});
            this.deploymentPlanDataGridView.DataSource = this.autoPatchDepPlanDTOBindingSource;
            this.deploymentPlanDataGridView.Location = new System.Drawing.Point(6, 19);
            this.deploymentPlanDataGridView.MultiSelect = false;
            this.deploymentPlanDataGridView.Name = "deploymentPlanDataGridView";
            this.deploymentPlanDataGridView.Size = new System.Drawing.Size(818, 158);
            this.deploymentPlanDataGridView.TabIndex = 4;
            this.deploymentPlanDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.deploymentPlanDataGridView_CellClick);
            this.deploymentPlanDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.deploymentPlanDataGridView_DataError);
            this.deploymentPlanDataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.deploymentPlanDataGridView_RowEnter);
            this.deploymentPlanDataGridView.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.deploymentPlanDataGridView_RowLeave);
            // 
            // patchDeploymentPlanIdDataGridViewTextBoxColumn
            // 
            this.patchDeploymentPlanIdDataGridViewTextBoxColumn.DataPropertyName = "PatchDeploymentPlanId";
            this.patchDeploymentPlanIdDataGridViewTextBoxColumn.HeaderText = "Plan Id";
            this.patchDeploymentPlanIdDataGridViewTextBoxColumn.Name = "patchDeploymentPlanIdDataGridViewTextBoxColumn";
            this.patchDeploymentPlanIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // deploymentPlanNameDataGridViewTextBoxColumn
            // 
            this.deploymentPlanNameDataGridViewTextBoxColumn.DataPropertyName = "DeploymentPlanName";
            this.deploymentPlanNameDataGridViewTextBoxColumn.HeaderText = "Plan Name";
            this.deploymentPlanNameDataGridViewTextBoxColumn.Name = "deploymentPlanNameDataGridViewTextBoxColumn";
            // 
            // deploymentPlannedDateDataGridViewTextBoxColumn
            // 
            this.deploymentPlannedDateDataGridViewTextBoxColumn.DataPropertyName = "DeploymentPlannedDate";
            this.deploymentPlannedDateDataGridViewTextBoxColumn.HeaderText = "Planned Date";
            this.deploymentPlannedDateDataGridViewTextBoxColumn.Name = "deploymentPlannedDateDataGridViewTextBoxColumn";
            // 
            // patchFileNameDataGridViewTextBoxColumn
            // 
            this.patchFileNameDataGridViewTextBoxColumn.DataPropertyName = "PatchFileName";
            this.patchFileNameDataGridViewTextBoxColumn.HeaderText = "Patch File Name";
            this.patchFileNameDataGridViewTextBoxColumn.Name = "patchFileNameDataGridViewTextBoxColumn";
            // 
            // deploymentStatusDataGridViewTextBoxColumn
            // 
            this.deploymentStatusDataGridViewTextBoxColumn.DataPropertyName = "DeploymentStatus";
            this.deploymentStatusDataGridViewTextBoxColumn.HeaderText = "Deployment Status";
            this.deploymentStatusDataGridViewTextBoxColumn.Name = "deploymentStatusDataGridViewTextBoxColumn";
            this.deploymentStatusDataGridViewTextBoxColumn.ReadOnly = true;
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
            // autoPatchDepPlanDTOBindingSource
            // 
            this.autoPatchDepPlanDTOBindingSource.DataSource = typeof(AutoPatchDepPlanDTO);
            // 
            // deploymentPlanDeleteBtn
            // 
            this.deploymentPlanDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deploymentPlanDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.deploymentPlanDeleteBtn.Location = new System.Drawing.Point(269, 384);
            this.deploymentPlanDeleteBtn.Name = "deploymentPlanDeleteBtn";
            this.deploymentPlanDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.deploymentPlanDeleteBtn.TabIndex = 10;
            this.deploymentPlanDeleteBtn.Text = "Delete";
            this.deploymentPlanDeleteBtn.UseVisualStyleBackColor = true;
            this.deploymentPlanDeleteBtn.Click += new System.EventHandler(this.deploymentPlanDeleteBtn_Click);
            // 
            // deploymentPlanCloseBtn
            // 
            this.deploymentPlanCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deploymentPlanCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.deploymentPlanCloseBtn.Location = new System.Drawing.Point(397, 384);
            this.deploymentPlanCloseBtn.Name = "deploymentPlanCloseBtn";
            this.deploymentPlanCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.deploymentPlanCloseBtn.TabIndex = 11;
            this.deploymentPlanCloseBtn.Text = "Close";
            this.deploymentPlanCloseBtn.UseVisualStyleBackColor = true;
            this.deploymentPlanCloseBtn.Click += new System.EventHandler(this.deploymentPlanCloseBtn_Click);
            // 
            // deploymentPlanRefreshBtn
            // 
            this.deploymentPlanRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deploymentPlanRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.deploymentPlanRefreshBtn.Location = new System.Drawing.Point(141, 384);
            this.deploymentPlanRefreshBtn.Name = "deploymentPlanRefreshBtn";
            this.deploymentPlanRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.deploymentPlanRefreshBtn.TabIndex = 9;
            this.deploymentPlanRefreshBtn.Text = "Refresh";
            this.deploymentPlanRefreshBtn.UseVisualStyleBackColor = true;
            this.deploymentPlanRefreshBtn.Click += new System.EventHandler(this.deploymentPlanRefreshBtn_Click);
            // 
            // deploymentPlanSaveBtn
            // 
            this.deploymentPlanSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deploymentPlanSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.deploymentPlanSaveBtn.Location = new System.Drawing.Point(13, 384);
            this.deploymentPlanSaveBtn.Name = "deploymentPlanSaveBtn";
            this.deploymentPlanSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.deploymentPlanSaveBtn.TabIndex = 8;
            this.deploymentPlanSaveBtn.Text = "Save";
            this.deploymentPlanSaveBtn.UseVisualStyleBackColor = true;
            this.deploymentPlanSaveBtn.Click += new System.EventHandler(this.deploymentPlanSaveBtn_Click);
            // 
            // btnApplyPatchToSite
            // 
            this.btnApplyPatchToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApplyPatchToSite.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnApplyPatchToSite.Location = new System.Drawing.Point(525, 384);
            this.btnApplyPatchToSite.Name = "btnApplyPatchToSite";
            this.btnApplyPatchToSite.Size = new System.Drawing.Size(126, 23);
            this.btnApplyPatchToSite.TabIndex = 12;
            this.btnApplyPatchToSite.Text = "Publish To Sites";
            this.btnApplyPatchToSite.UseVisualStyleBackColor = true;
            this.btnApplyPatchToSite.Visible = false;
            this.btnApplyPatchToSite.Click += new System.EventHandler(this.btnApplyPatchToSite_Click);
            // 
            // deploymentPlanApplicationDataGridView
            // 
            this.deploymentPlanApplicationDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deploymentPlanApplicationDataGridView.AutoGenerateColumns = false;
            this.deploymentPlanApplicationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.deploymentPlanApplicationDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.patchDeploymentPlanApplicationIdDataGridViewTextBoxColumn,
            this.patchDeploymentPlanIdApplDataGridViewTextBoxColumn,
            this.patchApplicationTypeIdDataGridViewTextBoxColumn,
            this.deploymentVersionDataGridViewTextBoxColumn,
            this.minimumVersionRequiredDataGridViewTextBoxColumn,
            this.upgradeTypeDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn1});
            this.deploymentPlanApplicationDataGridView.DataSource = this.autoPatchDepPlanApplicationDTOBindingSource;
            this.deploymentPlanApplicationDataGridView.Location = new System.Drawing.Point(6, 19);
            this.deploymentPlanApplicationDataGridView.MultiSelect = false;
            this.deploymentPlanApplicationDataGridView.Name = "deploymentPlanApplicationDataGridView";
            this.deploymentPlanApplicationDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.deploymentPlanApplicationDataGridView.Size = new System.Drawing.Size(819, 144);
            this.deploymentPlanApplicationDataGridView.TabIndex = 13;
            this.deploymentPlanApplicationDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.deploymentPlanApplicationDataGridView_DataError);
            this.deploymentPlanApplicationDataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.deploymentPlanApplicationDataGridView_DefaultValuesNeeded);
            // 
            // patchDeploymentPlanApplicationIdDataGridViewTextBoxColumn
            // 
            this.patchDeploymentPlanApplicationIdDataGridViewTextBoxColumn.DataPropertyName = "PatchDeploymentPlanApplicationId";
            this.patchDeploymentPlanApplicationIdDataGridViewTextBoxColumn.HeaderText = "Plan Application Id";
            this.patchDeploymentPlanApplicationIdDataGridViewTextBoxColumn.Name = "patchDeploymentPlanApplicationIdDataGridViewTextBoxColumn";
            this.patchDeploymentPlanApplicationIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // patchDeploymentPlanIdApplDataGridViewTextBoxColumn
            // 
            this.patchDeploymentPlanIdApplDataGridViewTextBoxColumn.DataPropertyName = "PatchDeploymentPlanId";
            this.patchDeploymentPlanIdApplDataGridViewTextBoxColumn.HeaderText = "Plan Id";
            this.patchDeploymentPlanIdApplDataGridViewTextBoxColumn.Name = "patchDeploymentPlanIdApplDataGridViewTextBoxColumn";
            this.patchDeploymentPlanIdApplDataGridViewTextBoxColumn.Visible = false;
            // 
            // patchApplicationTypeIdDataGridViewTextBoxColumn
            // 
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.DataPropertyName = "PatchApplicationTypeId";
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.HeaderText = "Application Type";
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.Name = "patchApplicationTypeIdDataGridViewTextBoxColumn";
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // deploymentVersionDataGridViewTextBoxColumn
            // 
            this.deploymentVersionDataGridViewTextBoxColumn.DataPropertyName = "DeploymentVersion";
            this.deploymentVersionDataGridViewTextBoxColumn.HeaderText = "Upgrade Version";
            this.deploymentVersionDataGridViewTextBoxColumn.Name = "deploymentVersionDataGridViewTextBoxColumn";
            // 
            // minimumVersionRequiredDataGridViewTextBoxColumn
            // 
            this.minimumVersionRequiredDataGridViewTextBoxColumn.DataPropertyName = "MinimumVersionRequired";
            this.minimumVersionRequiredDataGridViewTextBoxColumn.HeaderText = "Minimum Version";
            this.minimumVersionRequiredDataGridViewTextBoxColumn.Name = "minimumVersionRequiredDataGridViewTextBoxColumn";
            // 
            // upgradeTypeDataGridViewTextBoxColumn
            // 
            this.upgradeTypeDataGridViewTextBoxColumn.DataPropertyName = "UpgradeType";
            this.upgradeTypeDataGridViewTextBoxColumn.HeaderText = "Upgrade Type";
            this.upgradeTypeDataGridViewTextBoxColumn.Name = "upgradeTypeDataGridViewTextBoxColumn";
            this.upgradeTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.upgradeTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // isActiveDataGridViewTextBoxColumn1
            // 
            this.isActiveDataGridViewTextBoxColumn1.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn1.FalseValue = "N";
            this.isActiveDataGridViewTextBoxColumn1.HeaderText = "Active?";
            this.isActiveDataGridViewTextBoxColumn1.Name = "isActiveDataGridViewTextBoxColumn1";
            this.isActiveDataGridViewTextBoxColumn1.ReadOnly = true;
            this.isActiveDataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isActiveDataGridViewTextBoxColumn1.TrueValue = "Y";
            // 
            // autoPatchDepPlanApplicationDTOBindingSource
            // 
            this.autoPatchDepPlanApplicationDTOBindingSource.DataSource = typeof(AutoPatchDepPlanApplicationDTO);
            // 
            // gpDeploymentPlan
            // 
            this.gpDeploymentPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpDeploymentPlan.Controls.Add(this.deploymentPlanDataGridView);
            this.gpDeploymentPlan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpDeploymentPlan.Location = new System.Drawing.Point(6, 7);
            this.gpDeploymentPlan.Name = "gpDeploymentPlan";
            this.gpDeploymentPlan.Size = new System.Drawing.Size(831, 183);
            this.gpDeploymentPlan.TabIndex = 14;
            this.gpDeploymentPlan.TabStop = false;
            this.gpDeploymentPlan.Text = "Deployment Plan";
            // 
            // gpApplication
            // 
            this.gpApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpApplication.Controls.Add(this.deploymentPlanApplicationDataGridView);
            this.gpApplication.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpApplication.Location = new System.Drawing.Point(6, 200);
            this.gpApplication.Name = "gpApplication";
            this.gpApplication.Size = new System.Drawing.Size(831, 169);
            this.gpApplication.TabIndex = 15;
            this.gpApplication.TabStop = false;
            this.gpApplication.Text = "Deployment Application";
            // 
            // AutoPatchDeploymentPlanUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 424);
            this.Controls.Add(this.gpApplication);
            this.Controls.Add(this.gpDeploymentPlan);
            this.Controls.Add(this.btnApplyPatchToSite);
            this.Controls.Add(this.deploymentPlanDeleteBtn);
            this.Controls.Add(this.deploymentPlanCloseBtn);
            this.Controls.Add(this.deploymentPlanRefreshBtn);
            this.Controls.Add(this.deploymentPlanSaveBtn);
            this.Name = "AutoPatchDeploymentPlanUI";
            this.Text = "Deployment Plan";
            this.Load += new System.EventHandler(this.AutoPatchDeploymentPlanUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.deploymentPlanDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoPatchDepPlanDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deploymentPlanApplicationDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoPatchDepPlanApplicationDTOBindingSource)).EndInit();
            this.gpDeploymentPlan.ResumeLayout(false);
            this.gpApplication.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView deploymentPlanDataGridView;
        private System.Windows.Forms.Button deploymentPlanDeleteBtn;
        private System.Windows.Forms.Button deploymentPlanCloseBtn;
        private System.Windows.Forms.Button deploymentPlanRefreshBtn;
        private System.Windows.Forms.Button deploymentPlanSaveBtn;
        private System.Windows.Forms.Button btnApplyPatchToSite;
        private System.Windows.Forms.DataGridView deploymentPlanApplicationDataGridView;
        private System.Windows.Forms.GroupBox gpDeploymentPlan;
        private System.Windows.Forms.GroupBox gpApplication;
        private System.Windows.Forms.BindingSource autoPatchDepPlanDTOBindingSource;
        private System.Windows.Forms.BindingSource autoPatchDepPlanApplicationDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn patchDeploymentPlanIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deploymentPlanNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deploymentPlannedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn patchFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deploymentStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn patchDeploymentPlanApplicationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn patchDeploymentPlanIdApplDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn patchApplicationTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deploymentVersionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minimumVersionRequiredDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn upgradeTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn1;
    }
}

