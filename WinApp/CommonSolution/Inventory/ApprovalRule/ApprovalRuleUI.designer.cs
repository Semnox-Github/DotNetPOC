namespace Semnox.Parafait.Inventory
{
    partial class ApprovalRuleUI
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvApprovalRule = new System.Windows.Forms.DataGridView();
            this.approvalRuleIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.roleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.numberOfApprovalLevelsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.approvalRuleDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gpApprovalRule = new System.Windows.Forms.GroupBox();
            this.gpDocumentType = new System.Windows.Forms.GroupBox();
            this.dgvDocumentType = new System.Windows.Forms.DataGridView();
            this.documentTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.applicabilityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.inventoryDocumentTypeDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvApprovalRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.approvalRuleDTOBindingSource)).BeginInit();
            this.gpApprovalRule.SuspendLayout();
            this.gpDocumentType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocumentType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryDocumentTypeDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(602, 558);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 17;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(730, 558);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(474, 558);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvApprovalRule
            // 
            this.dgvApprovalRule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvApprovalRule.AutoGenerateColumns = false;
            this.dgvApprovalRule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvApprovalRule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.approvalRuleIDDataGridViewTextBoxColumn,
            this.roleIdDataGridViewTextBoxColumn,
            this.numberOfApprovalLevelsDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvApprovalRule.DataSource = this.approvalRuleDTOBindingSource;
            this.dgvApprovalRule.Location = new System.Drawing.Point(6, 19);
            this.dgvApprovalRule.Name = "dgvApprovalRule";
            this.dgvApprovalRule.Size = new System.Drawing.Size(803, 507);
            this.dgvApprovalRule.TabIndex = 15;
            // 
            // approvalRuleIDDataGridViewTextBoxColumn
            // 
            this.approvalRuleIDDataGridViewTextBoxColumn.DataPropertyName = "ApprovalRuleID";
            this.approvalRuleIDDataGridViewTextBoxColumn.HeaderText = "Approval Rule ID";
            this.approvalRuleIDDataGridViewTextBoxColumn.Name = "approvalRuleIDDataGridViewTextBoxColumn";
            this.approvalRuleIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // roleIdDataGridViewTextBoxColumn
            // 
            this.roleIdDataGridViewTextBoxColumn.DataPropertyName = "RoleId";
            this.roleIdDataGridViewTextBoxColumn.HeaderText = "Role";
            this.roleIdDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.roleIdDataGridViewTextBoxColumn.Name = "roleIdDataGridViewTextBoxColumn";
            this.roleIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.roleIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.roleIdDataGridViewTextBoxColumn.Width = 120;
            // 
            // numberOfApprovalLevelsDataGridViewTextBoxColumn
            // 
            this.numberOfApprovalLevelsDataGridViewTextBoxColumn.DataPropertyName = "NumberOfApprovalLevels";
            this.numberOfApprovalLevelsDataGridViewTextBoxColumn.HeaderText = "Number Of Approval Levels";
            this.numberOfApprovalLevelsDataGridViewTextBoxColumn.Name = "numberOfApprovalLevelsDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // approvalRuleDTOBindingSource
            // 
            this.approvalRuleDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.ApprovalRuleDTO);
            // 
            // gpApprovalRule
            // 
            this.gpApprovalRule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpApprovalRule.Controls.Add(this.dgvApprovalRule);
            this.gpApprovalRule.Location = new System.Drawing.Point(354, 8);
            this.gpApprovalRule.Name = "gpApprovalRule";
            this.gpApprovalRule.Size = new System.Drawing.Size(815, 532);
            this.gpApprovalRule.TabIndex = 19;
            this.gpApprovalRule.TabStop = false;
            this.gpApprovalRule.Text = "Approval Rule";
            // 
            // gpDocumentType
            // 
            this.gpDocumentType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gpDocumentType.Controls.Add(this.dgvDocumentType);
            this.gpDocumentType.Location = new System.Drawing.Point(12, 8);
            this.gpDocumentType.Name = "gpDocumentType";
            this.gpDocumentType.Size = new System.Drawing.Size(336, 532);
            this.gpDocumentType.TabIndex = 20;
            this.gpDocumentType.TabStop = false;
            this.gpDocumentType.Text = "Document Type";
            // 
            // dgvDocumentType
            // 
            this.dgvDocumentType.AllowUserToAddRows = false;
            this.dgvDocumentType.AllowUserToDeleteRows = false;
            this.dgvDocumentType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDocumentType.AutoGenerateColumns = false;
            this.dgvDocumentType.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDocumentType.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.documentTypeIdDataGridViewTextBoxColumn,
            this.applicabilityDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.codeDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn1});
            this.dgvDocumentType.DataSource = this.inventoryDocumentTypeDTOBindingSource;
            this.dgvDocumentType.Location = new System.Drawing.Point(6, 19);
            this.dgvDocumentType.Name = "dgvDocumentType";
            this.dgvDocumentType.ReadOnly = true;
            this.dgvDocumentType.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDocumentType.Size = new System.Drawing.Size(324, 507);
            this.dgvDocumentType.TabIndex = 14;
            this.dgvDocumentType.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentType_RowEnter);
            // 
            // documentTypeIdDataGridViewTextBoxColumn
            // 
            this.documentTypeIdDataGridViewTextBoxColumn.DataPropertyName = "DocumentTypeId";
            this.documentTypeIdDataGridViewTextBoxColumn.HeaderText = "Document Type Id";
            this.documentTypeIdDataGridViewTextBoxColumn.Name = "documentTypeIdDataGridViewTextBoxColumn";
            this.documentTypeIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.documentTypeIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // applicabilityDataGridViewTextBoxColumn
            // 
            this.applicabilityDataGridViewTextBoxColumn.DataPropertyName = "Applicability";
            this.applicabilityDataGridViewTextBoxColumn.HeaderText = "Applicability";
            this.applicabilityDataGridViewTextBoxColumn.Name = "applicabilityDataGridViewTextBoxColumn";
            this.applicabilityDataGridViewTextBoxColumn.ReadOnly = true;
            this.applicabilityDataGridViewTextBoxColumn.Width = 80;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // codeDataGridViewTextBoxColumn
            // 
            this.codeDataGridViewTextBoxColumn.DataPropertyName = "Code";
            this.codeDataGridViewTextBoxColumn.HeaderText = "Code";
            this.codeDataGridViewTextBoxColumn.Name = "codeDataGridViewTextBoxColumn";
            this.codeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Visible = false;
            // 
            // isActiveDataGridViewCheckBoxColumn1
            // 
            this.isActiveDataGridViewCheckBoxColumn1.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn1.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn1.Name = "isActiveDataGridViewCheckBoxColumn1";
            this.isActiveDataGridViewCheckBoxColumn1.ReadOnly = true;
            this.isActiveDataGridViewCheckBoxColumn1.Visible = false;
            // 
            // inventoryDocumentTypeDTOBindingSource
            // 
            this.inventoryDocumentTypeDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.InventoryDocumentTypeDTO);
            // 
            // ApprovalRuleUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 591);
            this.Controls.Add(this.gpDocumentType);
            this.Controls.Add(this.gpApprovalRule);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "ApprovalRuleUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Approval Rule";
            ((System.ComponentModel.ISupportInitialize)(this.dgvApprovalRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.approvalRuleDTOBindingSource)).EndInit();
            this.gpApprovalRule.ResumeLayout(false);
            this.gpDocumentType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocumentType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryDocumentTypeDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvApprovalRule;
        private System.Windows.Forms.GroupBox gpApprovalRule;
        private System.Windows.Forms.GroupBox gpDocumentType;
        private System.Windows.Forms.DataGridView dgvDocumentType;
        private System.Windows.Forms.BindingSource approvalRuleDTOBindingSource;
        private System.Windows.Forms.BindingSource inventoryDocumentTypeDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn documentTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicabilityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn approvalRuleIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn roleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfApprovalLevelsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}

