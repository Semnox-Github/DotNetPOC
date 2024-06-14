namespace Semnox.Parafait.User
{
    partial class DataAccessControlUI
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
            this.gpDataAccessRule = new System.Windows.Forms.GroupBox();
            this.dgvDataAccessRule = new System.Windows.Forms.DataGridView();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gpDataAccessRuleDetail = new System.Windows.Forms.GroupBox();
            this.dgvAccessRuleDetail = new System.Windows.Forms.DataGridView();
            this.DetailIsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gpExclusionDetail = new System.Windows.Forms.GroupBox();
            this.dgvExclusionDetails = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.entityExclusionDetailDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ruleDetailIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.accessLevelIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.accessLimitIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataAccessDetailDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataAccessRuleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataAccessRuleDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.exclusionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableAttributeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.attribute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableAttributeGuidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ExclusionIsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gpDataAccessRule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataAccessRule)).BeginInit();
            this.gpDataAccessRuleDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccessRuleDetail)).BeginInit();
            this.gpExclusionDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExclusionDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityExclusionDetailDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataAccessDetailDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataAccessRuleDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gpDataAccessRule
            // 
            this.gpDataAccessRule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpDataAccessRule.Controls.Add(this.dgvDataAccessRule);
            this.gpDataAccessRule.Location = new System.Drawing.Point(6, 3);
            this.gpDataAccessRule.Name = "gpDataAccessRule";
            this.gpDataAccessRule.Size = new System.Drawing.Size(930, 130);
            this.gpDataAccessRule.TabIndex = 0;
            this.gpDataAccessRule.TabStop = false;
            this.gpDataAccessRule.Text = "Data Access Rule";
            // 
            // dgvDataAccessRule
            // 
            this.dgvDataAccessRule.AutoGenerateColumns = false;
            this.dgvDataAccessRule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataAccessRule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataAccessRuleIdDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.IsActive});
            this.dgvDataAccessRule.DataSource = this.dataAccessRuleDTOBindingSource;
            this.dgvDataAccessRule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDataAccessRule.Location = new System.Drawing.Point(3, 16);
            this.dgvDataAccessRule.Name = "dgvDataAccessRule";
            this.dgvDataAccessRule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDataAccessRule.Size = new System.Drawing.Size(924, 111);
            this.dgvDataAccessRule.TabIndex = 1;
            this.dgvDataAccessRule.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDataAccessRule_DataError);
            this.dgvDataAccessRule.Enter += new System.EventHandler(this.dgvDataAccessRule_Enter);
            // 
            // IsActive
            // 
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.HeaderText = "IsActive";
            this.IsActive.Name = "IsActive";
            this.IsActive.Visible = false;
            // 
            // gpDataAccessRuleDetail
            // 
            this.gpDataAccessRuleDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpDataAccessRuleDetail.Controls.Add(this.dgvAccessRuleDetail);
            this.gpDataAccessRuleDetail.Location = new System.Drawing.Point(6, 139);
            this.gpDataAccessRuleDetail.Name = "gpDataAccessRuleDetail";
            this.gpDataAccessRuleDetail.Size = new System.Drawing.Size(930, 245);
            this.gpDataAccessRuleDetail.TabIndex = 1;
            this.gpDataAccessRuleDetail.TabStop = false;
            this.gpDataAccessRuleDetail.Text = "Access Rule Details";
            // 
            // dgvAccessRuleDetail
            // 
            this.dgvAccessRuleDetail.AutoGenerateColumns = false;
            this.dgvAccessRuleDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccessRuleDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ruleDetailIdDataGridViewTextBoxColumn,
            this.entityIdDataGridViewTextBoxColumn,
            this.accessLevelIdDataGridViewTextBoxColumn,
            this.accessLimitIdDataGridViewTextBoxColumn,
            this.DetailIsActive});
            this.dgvAccessRuleDetail.DataSource = this.dataAccessDetailDTOBindingSource;
            this.dgvAccessRuleDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAccessRuleDetail.Location = new System.Drawing.Point(3, 16);
            this.dgvAccessRuleDetail.Name = "dgvAccessRuleDetail";
            this.dgvAccessRuleDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAccessRuleDetail.Size = new System.Drawing.Size(924, 226);
            this.dgvAccessRuleDetail.TabIndex = 1;
            this.dgvAccessRuleDetail.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAccessRuleDetail_DataError);
            this.dgvAccessRuleDetail.Enter += new System.EventHandler(this.dgvAccessRuleDetail_Enter);
            // 
            // DetailIsActive
            // 
            this.DetailIsActive.DataPropertyName = "IsActive";
            this.DetailIsActive.HeaderText = "IsActive";
            this.DetailIsActive.Name = "DetailIsActive";
            // 
            // gpExclusionDetail
            // 
            this.gpExclusionDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpExclusionDetail.Controls.Add(this.dgvExclusionDetails);
            this.gpExclusionDetail.Location = new System.Drawing.Point(6, 390);
            this.gpExclusionDetail.Name = "gpExclusionDetail";
            this.gpExclusionDetail.Size = new System.Drawing.Size(930, 115);
            this.gpExclusionDetail.TabIndex = 2;
            this.gpExclusionDetail.TabStop = false;
            this.gpExclusionDetail.Text = "Exclusion Detail";
            // 
            // dgvExclusionDetails
            // 
            this.dgvExclusionDetails.AutoGenerateColumns = false;
            this.dgvExclusionDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExclusionDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.exclusionIdDataGridViewTextBoxColumn,
            this.tableNameDataGridViewTextBoxColumn,
            this.tableAttributeIdDataGridViewTextBoxColumn,
            this.attribute,
            this.tableAttributeGuidDataGridViewTextBoxColumn,
            this.ExclusionIsActive,
            this.FieldName});
            this.dgvExclusionDetails.DataSource = this.entityExclusionDetailDTOBindingSource;
            this.dgvExclusionDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvExclusionDetails.Location = new System.Drawing.Point(3, 16);
            this.dgvExclusionDetails.Name = "dgvExclusionDetails";
            this.dgvExclusionDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvExclusionDetails.Size = new System.Drawing.Size(924, 96);
            this.dgvExclusionDetails.TabIndex = 0;
            this.dgvExclusionDetails.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvExclusionDetails_CellValueChanged);
            this.dgvExclusionDetails.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvExclusionDetails_DataError);
            this.dgvExclusionDetails.Enter += new System.EventHandler(this.dgvExclusionDetails_Enter);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(12, 522);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(117, 522);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(222, 522);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(327, 522);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // entityExclusionDetailDTOBindingSource
            // 
            this.entityExclusionDetailDTOBindingSource.DataSource = typeof(Semnox.Parafait.User.EntityExclusionDetailDTO);
            // 
            // ruleDetailIdDataGridViewTextBoxColumn
            // 
            this.ruleDetailIdDataGridViewTextBoxColumn.DataPropertyName = "RuleDetailId";
            this.ruleDetailIdDataGridViewTextBoxColumn.HeaderText = "RuleDetailId";
            this.ruleDetailIdDataGridViewTextBoxColumn.Name = "ruleDetailIdDataGridViewTextBoxColumn";
            this.ruleDetailIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // entityIdDataGridViewTextBoxColumn
            // 
            this.entityIdDataGridViewTextBoxColumn.DataPropertyName = "EntityId";
            this.entityIdDataGridViewTextBoxColumn.HeaderText = "Entity";
            this.entityIdDataGridViewTextBoxColumn.Name = "entityIdDataGridViewTextBoxColumn";
            this.entityIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.entityIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // accessLevelIdDataGridViewTextBoxColumn
            // 
            this.accessLevelIdDataGridViewTextBoxColumn.DataPropertyName = "AccessLevelId";
            this.accessLevelIdDataGridViewTextBoxColumn.HeaderText = "Access Level";
            this.accessLevelIdDataGridViewTextBoxColumn.Name = "accessLevelIdDataGridViewTextBoxColumn";
            this.accessLevelIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.accessLevelIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // accessLimitIdDataGridViewTextBoxColumn
            // 
            this.accessLimitIdDataGridViewTextBoxColumn.DataPropertyName = "AccessLimitId";
            this.accessLimitIdDataGridViewTextBoxColumn.HeaderText = "Access Limit";
            this.accessLimitIdDataGridViewTextBoxColumn.Name = "accessLimitIdDataGridViewTextBoxColumn";
            this.accessLimitIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.accessLimitIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataAccessDetailDTOBindingSource
            // 
            this.dataAccessDetailDTOBindingSource.DataSource = typeof(Semnox.Parafait.User.DataAccessDetailDTO);
            this.dataAccessDetailDTOBindingSource.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.dataAccessDetailDTOBindingSource_AddingNew);
            this.dataAccessDetailDTOBindingSource.CurrentChanged += new System.EventHandler(this.DataAccessRuleDetailBS_CurrentChanged);
            // 
            // dataAccessRuleIdDataGridViewTextBoxColumn
            // 
            this.dataAccessRuleIdDataGridViewTextBoxColumn.DataPropertyName = "DataAccessRuleId";
            this.dataAccessRuleIdDataGridViewTextBoxColumn.HeaderText = "DataAccessRuleId";
            this.dataAccessRuleIdDataGridViewTextBoxColumn.Name = "dataAccessRuleIdDataGridViewTextBoxColumn";
            this.dataAccessRuleIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // dataAccessRuleDTOBindingSource
            // 
            this.dataAccessRuleDTOBindingSource.DataSource = typeof(Semnox.Parafait.User.DataAccessRuleDTO);
            this.dataAccessRuleDTOBindingSource.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.dataAccessRuleDTOBindingSource_AddingNew);
            this.dataAccessRuleDTOBindingSource.CurrentChanged += new System.EventHandler(this.DataAccessRule_CurrentChanged);
            // 
            // exclusionIdDataGridViewTextBoxColumn
            // 
            this.exclusionIdDataGridViewTextBoxColumn.DataPropertyName = "ExclusionId";
            this.exclusionIdDataGridViewTextBoxColumn.HeaderText = "ExclusionId";
            this.exclusionIdDataGridViewTextBoxColumn.Name = "exclusionIdDataGridViewTextBoxColumn";
            this.exclusionIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tableNameDataGridViewTextBoxColumn
            // 
            this.tableNameDataGridViewTextBoxColumn.DataPropertyName = "TableName";
            this.tableNameDataGridViewTextBoxColumn.HeaderText = "Table Name";
            this.tableNameDataGridViewTextBoxColumn.Name = "tableNameDataGridViewTextBoxColumn";
            this.tableNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // tableAttributeIdDataGridViewTextBoxColumn
            // 
            this.tableAttributeIdDataGridViewTextBoxColumn.DataPropertyName = "TableAttributeId";
            this.tableAttributeIdDataGridViewTextBoxColumn.HeaderText = "Entity";
            this.tableAttributeIdDataGridViewTextBoxColumn.Name = "tableAttributeIdDataGridViewTextBoxColumn";
            this.tableAttributeIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tableAttributeIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // attribute
            // 
            this.attribute.HeaderText = "Attribute";
            this.attribute.Name = "attribute";
            this.attribute.ReadOnly = true;
            this.attribute.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.attribute.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.attribute.Visible = false;
            // 
            // tableAttributeGuidDataGridViewTextBoxColumn
            // 
            this.tableAttributeGuidDataGridViewTextBoxColumn.DataPropertyName = "TableAttributeGuid";
            this.tableAttributeGuidDataGridViewTextBoxColumn.HeaderText = "Attribute";
            this.tableAttributeGuidDataGridViewTextBoxColumn.Name = "tableAttributeGuidDataGridViewTextBoxColumn";
            this.tableAttributeGuidDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tableAttributeGuidDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ExclusionIsActive
            // 
            this.ExclusionIsActive.DataPropertyName = "IsActive";
            this.ExclusionIsActive.HeaderText = "IsActive";
            this.ExclusionIsActive.Name = "ExclusionIsActive";
            // 
            // FieldName
            // 
            this.FieldName.DataPropertyName = "FieldName";
            this.FieldName.HeaderText = "FieldName";
            this.FieldName.Name = "FieldName";
            this.FieldName.Visible = false;
            // 
            // DataAccessControlUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 568);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gpExclusionDetail);
            this.Controls.Add(this.gpDataAccessRuleDetail);
            this.Controls.Add(this.gpDataAccessRule);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataAccessControlUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Access Management";
            this.gpDataAccessRule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataAccessRule)).EndInit();
            this.gpDataAccessRuleDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccessRuleDetail)).EndInit();
            this.gpExclusionDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExclusionDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityExclusionDetailDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataAccessDetailDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataAccessRuleDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpDataAccessRule;
        private System.Windows.Forms.GroupBox gpDataAccessRuleDetail;
        private System.Windows.Forms.GroupBox gpExclusionDetail;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvDataAccessRule;
        private System.Windows.Forms.DataGridView dgvAccessRuleDetail;
        private System.Windows.Forms.DataGridView dgvExclusionDetails;
        private System.Windows.Forms.BindingSource dataAccessRuleDTOBindingSource;
        private System.Windows.Forms.BindingSource dataAccessDetailDTOBindingSource;
        private System.Windows.Forms.BindingSource entityExclusionDetailDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn ruleDetailIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn entityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn accessLevelIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn accessLimitIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DetailIsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataAccessRuleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn exclusionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tableNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn tableAttributeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attribute;
        private System.Windows.Forms.DataGridViewComboBoxColumn tableAttributeGuidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ExclusionIsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldName;
    }
}

