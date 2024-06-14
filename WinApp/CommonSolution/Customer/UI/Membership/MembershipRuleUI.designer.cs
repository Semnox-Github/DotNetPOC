namespace Semnox.Parafait.Customer.Membership
{
    partial class MembershipRuleUI
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvMembershipRule = new System.Windows.Forms.DataGridView();
            this.membershipRuleDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.membershipRuleIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ruleNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qualifyingPointsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qualificationWindowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitOfQualificationWindowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.retentionPointsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.retentionWindowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitOfRetentionWindowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembershipRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.membershipRuleDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(269, 236);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(397, 236);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(141, 236);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 15;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(13, 236);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvMembershipRule
            // 
            this.dgvMembershipRule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMembershipRule.AutoGenerateColumns = false;
            this.dgvMembershipRule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMembershipRule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.membershipRuleIDDataGridViewTextBoxColumn,
            this.ruleNameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.qualifyingPointsDataGridViewTextBoxColumn,
            this.qualificationWindowDataGridViewTextBoxColumn,
            this.unitOfQualificationWindowDataGridViewTextBoxColumn,
            this.retentionPointsDataGridViewTextBoxColumn,
            this.retentionWindowDataGridViewTextBoxColumn,
            this.unitOfRetentionWindowDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvMembershipRule.DataSource = this.membershipRuleDTOBindingSource;
            this.dgvMembershipRule.Location = new System.Drawing.Point(12, 12);
            this.dgvMembershipRule.Name = "dgvMembershipRule";
            this.dgvMembershipRule.Size = new System.Drawing.Size(752, 206);
            this.dgvMembershipRule.TabIndex = 13;
            this.dgvMembershipRule.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvMembershipRule_DataError);
            // 
            // membershipRuleDTOBindingSource
            // 
            this.membershipRuleDTOBindingSource.DataSource = typeof(Semnox.Parafait.Customer.Membership.MembershipRuleDTO);
            // 
            // membershipRuleIDDataGridViewTextBoxColumn
            // 
            this.membershipRuleIDDataGridViewTextBoxColumn.DataPropertyName = "MembershipRuleID";
            this.membershipRuleIDDataGridViewTextBoxColumn.HeaderText = "Rule ID";
            this.membershipRuleIDDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.membershipRuleIDDataGridViewTextBoxColumn.Name = "membershipRuleIDDataGridViewTextBoxColumn";
            this.membershipRuleIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.membershipRuleIDDataGridViewTextBoxColumn.Width = 50;
            // 
            // ruleNameDataGridViewTextBoxColumn
            // 
            this.ruleNameDataGridViewTextBoxColumn.DataPropertyName = "RuleName";
            this.ruleNameDataGridViewTextBoxColumn.HeaderText = "Rule Name";
            this.ruleNameDataGridViewTextBoxColumn.Name = "ruleNameDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // qualifyingPointsDataGridViewTextBoxColumn
            // 
            this.qualifyingPointsDataGridViewTextBoxColumn.DataPropertyName = "QualifyingPoints";
            this.qualifyingPointsDataGridViewTextBoxColumn.HeaderText = "Qualifying Points";
            this.qualifyingPointsDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.qualifyingPointsDataGridViewTextBoxColumn.Name = "qualifyingPointsDataGridViewTextBoxColumn";
            this.qualifyingPointsDataGridViewTextBoxColumn.Width = 120;
            // 
            // qualificationWindowDataGridViewTextBoxColumn
            // 
            this.qualificationWindowDataGridViewTextBoxColumn.DataPropertyName = "QualificationWindow";
            this.qualificationWindowDataGridViewTextBoxColumn.HeaderText = "Qualification Window";
            this.qualificationWindowDataGridViewTextBoxColumn.MinimumWidth = 140;
            this.qualificationWindowDataGridViewTextBoxColumn.Name = "qualificationWindowDataGridViewTextBoxColumn";
            this.qualificationWindowDataGridViewTextBoxColumn.Width = 140;
            // 
            // unitOfQualificationWindowDataGridViewTextBoxColumn
            // 
            this.unitOfQualificationWindowDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.unitOfQualificationWindowDataGridViewTextBoxColumn.DataPropertyName = "UnitOfQualificationWindow";
            this.unitOfQualificationWindowDataGridViewTextBoxColumn.HeaderText = "Unit Of Qualification Window";
            this.unitOfQualificationWindowDataGridViewTextBoxColumn.MinimumWidth = 170;
            this.unitOfQualificationWindowDataGridViewTextBoxColumn.Name = "unitOfQualificationWindowDataGridViewTextBoxColumn";
            this.unitOfQualificationWindowDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.unitOfQualificationWindowDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.unitOfQualificationWindowDataGridViewTextBoxColumn.Width = 170;
            // 
            // retentionPointsDataGridViewTextBoxColumn
            // 
            this.retentionPointsDataGridViewTextBoxColumn.DataPropertyName = "RetentionPoints";
            this.retentionPointsDataGridViewTextBoxColumn.HeaderText = "Retention Points";
            this.retentionPointsDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.retentionPointsDataGridViewTextBoxColumn.Name = "retentionPointsDataGridViewTextBoxColumn";
            this.retentionPointsDataGridViewTextBoxColumn.Width = 120;
            // 
            // retentionWindowDataGridViewTextBoxColumn
            // 
            this.retentionWindowDataGridViewTextBoxColumn.DataPropertyName = "RetentionWindow";
            this.retentionWindowDataGridViewTextBoxColumn.HeaderText = "Retention Window";
            this.retentionWindowDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.retentionWindowDataGridViewTextBoxColumn.Name = "retentionWindowDataGridViewTextBoxColumn";
            this.retentionWindowDataGridViewTextBoxColumn.Width = 120;
            // 
            // unitOfRetentionWindowDataGridViewTextBoxColumn
            // 
            this.unitOfRetentionWindowDataGridViewTextBoxColumn.DataPropertyName = "UnitOfRetentionWindow";
            this.unitOfRetentionWindowDataGridViewTextBoxColumn.HeaderText = "Unit Of Retention Window";
            this.unitOfRetentionWindowDataGridViewTextBoxColumn.MinimumWidth = 160;
            this.unitOfRetentionWindowDataGridViewTextBoxColumn.Name = "unitOfRetentionWindowDataGridViewTextBoxColumn";
            this.unitOfRetentionWindowDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.unitOfRetentionWindowDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.unitOfRetentionWindowDataGridViewTextBoxColumn.Width = 160;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // MembershipRuleUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 277);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvMembershipRule);
            this.MaximizeBox = false;
            this.Name = "MembershipRuleUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Membership Rules";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembershipRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.membershipRuleDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvMembershipRule;
        private System.Windows.Forms.BindingSource membershipRuleDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn membershipRuleIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ruleNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn qualifyingPointsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn qualificationWindowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn unitOfQualificationWindowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn retentionPointsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn retentionWindowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn unitOfRetentionWindowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}

