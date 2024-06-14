namespace Semnox.Parafait.Deployment
{
    partial class DeploymentPlanUI
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
            this.deploymentVersionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minimumVersionRequiredDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.patchApplicationTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.upgradeTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.patchFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deploymentStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.patchApplicationDeploymentPlanDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.deploymentPlanDeleteBtn = new System.Windows.Forms.Button();
            this.deploymentPlanCloseBtn = new System.Windows.Forms.Button();
            this.deploymentPlanRefreshBtn = new System.Windows.Forms.Button();
            this.deploymentPlanSaveBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.deploymentPlanDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.patchApplicationDeploymentPlanDTOBindingSource)).BeginInit();
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
            this.deploymentVersionDataGridViewTextBoxColumn,
            this.minimumVersionRequiredDataGridViewTextBoxColumn,
            this.patchApplicationTypeIdDataGridViewTextBoxColumn,
            this.upgradeTypeDataGridViewTextBoxColumn,
            this.patchFileNameDataGridViewTextBoxColumn,
            this.deploymentStatusDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn});
            this.deploymentPlanDataGridView.DataSource = this.patchApplicationDeploymentPlanDTOBindingSource;
            this.deploymentPlanDataGridView.Location = new System.Drawing.Point(12, 12);
            this.deploymentPlanDataGridView.Name = "deploymentPlanDataGridView";
            this.deploymentPlanDataGridView.Size = new System.Drawing.Size(817, 225);
            this.deploymentPlanDataGridView.TabIndex = 4;
            this.deploymentPlanDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.deploymentPlanDataGridView_DataError);
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
            // patchApplicationTypeIdDataGridViewTextBoxColumn
            // 
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.DataPropertyName = "PatchApplicationTypeId";
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.HeaderText = "Application Type";
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.Name = "patchApplicationTypeIdDataGridViewTextBoxColumn";
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // upgradeTypeDataGridViewTextBoxColumn
            // 
            this.upgradeTypeDataGridViewTextBoxColumn.DataPropertyName = "UpgradeType";
            this.upgradeTypeDataGridViewTextBoxColumn.HeaderText = "Upgrade Type";
            this.upgradeTypeDataGridViewTextBoxColumn.Name = "upgradeTypeDataGridViewTextBoxColumn";
            this.upgradeTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.upgradeTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
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
            // 
            // isActiveDataGridViewTextBoxColumn
            // 
            this.isActiveDataGridViewTextBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn.FalseValue = "N";
            this.isActiveDataGridViewTextBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewTextBoxColumn.Name = "isActiveDataGridViewTextBoxColumn";
            this.isActiveDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isActiveDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // patchApplicationDeploymentPlanDTOBindingSource
            // 
            this.patchApplicationDeploymentPlanDTOBindingSource.DataSource = typeof(PatchApplicationDeploymentPlanDTO);
            // 
            // deploymentPlanDeleteBtn
            // 
            this.deploymentPlanDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deploymentPlanDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.deploymentPlanDeleteBtn.Location = new System.Drawing.Point(269, 255);
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
            this.deploymentPlanCloseBtn.Location = new System.Drawing.Point(397, 255);
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
            this.deploymentPlanRefreshBtn.Location = new System.Drawing.Point(141, 255);
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
            this.deploymentPlanSaveBtn.Location = new System.Drawing.Point(13, 255);
            this.deploymentPlanSaveBtn.Name = "deploymentPlanSaveBtn";
            this.deploymentPlanSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.deploymentPlanSaveBtn.TabIndex = 8;
            this.deploymentPlanSaveBtn.Text = "Save";
            this.deploymentPlanSaveBtn.UseVisualStyleBackColor = true;
            this.deploymentPlanSaveBtn.Click += new System.EventHandler(this.deploymentPlanSaveBtn_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(525, 255);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Apply Patch to Site";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // DeploymentPlanUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 295);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.deploymentPlanDeleteBtn);
            this.Controls.Add(this.deploymentPlanCloseBtn);
            this.Controls.Add(this.deploymentPlanRefreshBtn);
            this.Controls.Add(this.deploymentPlanSaveBtn);
            this.Controls.Add(this.deploymentPlanDataGridView);
            this.Name = "DeploymentPlanUI";
            this.Text = "Deployment Plan";
            ((System.ComponentModel.ISupportInitialize)(this.deploymentPlanDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.patchApplicationDeploymentPlanDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView deploymentPlanDataGridView;
        private System.Windows.Forms.Button deploymentPlanDeleteBtn;
        private System.Windows.Forms.Button deploymentPlanCloseBtn;
        private System.Windows.Forms.Button deploymentPlanRefreshBtn;
        private System.Windows.Forms.Button deploymentPlanSaveBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn patchDeploymentPlanIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deploymentPlanNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deploymentPlannedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deploymentVersionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minimumVersionRequiredDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn patchApplicationTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn upgradeTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn patchFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deploymentStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource patchApplicationDeploymentPlanDTOBindingSource;
        private System.Windows.Forms.Button button1;
    }
}

