namespace Semnox.Parafait.Customer.Membership
{
    partial class MembershipUI
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvMembership = new System.Windows.Forms.DataGridView();
            this.membershipIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.membershipNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vIPDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.autoApplyDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.baseMembershipIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.membershipRuleIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.redemptionDiscountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceListIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.membershipDTOBindingSource = new System.Windows.Forms.BindingSource();
            this.gpMembership = new System.Windows.Forms.GroupBox();
            this.gpMembershipRewards = new System.Windows.Forms.GroupBox();
            this.dgvMembershipRewards = new System.Windows.Forms.DataGridView();
            this.membershipRewardsIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rewardNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rewardProductIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rewardAttributeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rewardAttributePercentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rewardFunctionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rewardFunctionPeriodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitOfRewardFunctionPeriodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rewardFrequencyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitOfRewardFrequencyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.expireWithMembershipDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.membershipRewardsDTOBindingSource = new System.Windows.Forms.BindingSource();
            this.btnExclusionRules = new System.Windows.Forms.Button();
            this.btnMembershipRule = new System.Windows.Forms.Button();
            this.btnPublishToSite = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembership)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.membershipDTOBindingSource)).BeginInit();
            this.gpMembership.SuspendLayout();
            this.gpMembershipRewards.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembershipRewards)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.membershipRewardsDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(248, 397);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 22;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(361, 397);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(135, 397);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 20;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(22, 397);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvMembership
            // 
            this.dgvMembership.AutoGenerateColumns = false;
            this.dgvMembership.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMembership.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.membershipIDDataGridViewTextBoxColumn,
            this.membershipNameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.vIPDataGridViewCheckBoxColumn,
            this.autoApplyDataGridViewCheckBoxColumn,
            this.baseMembershipIDDataGridViewTextBoxColumn,
            this.membershipRuleIDDataGridViewTextBoxColumn,
            this.redemptionDiscountDataGridViewTextBoxColumn,
            this.priceListIdDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvMembership.DataSource = this.membershipDTOBindingSource;
            this.dgvMembership.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMembership.Location = new System.Drawing.Point(3, 16);
            this.dgvMembership.Name = "dgvMembership";
            this.dgvMembership.Size = new System.Drawing.Size(1038, 213);
            this.dgvMembership.TabIndex = 18;
            this.dgvMembership.Enter += new System.EventHandler(this.dgvMembership_Enter);
            // 
            // membershipIDDataGridViewTextBoxColumn
            // 
            this.membershipIDDataGridViewTextBoxColumn.DataPropertyName = "MembershipID";
            this.membershipIDDataGridViewTextBoxColumn.HeaderText = "Membership ID";
            this.membershipIDDataGridViewTextBoxColumn.Name = "membershipIDDataGridViewTextBoxColumn";
            this.membershipIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // membershipNameDataGridViewTextBoxColumn
            // 
            this.membershipNameDataGridViewTextBoxColumn.DataPropertyName = "MembershipName";
            this.membershipNameDataGridViewTextBoxColumn.HeaderText = "Membership Name";
            this.membershipNameDataGridViewTextBoxColumn.Name = "membershipNameDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // vIPDataGridViewCheckBoxColumn
            // 
            this.vIPDataGridViewCheckBoxColumn.DataPropertyName = "VIP";
            this.vIPDataGridViewCheckBoxColumn.HeaderText = "VIP";
            this.vIPDataGridViewCheckBoxColumn.Name = "vIPDataGridViewCheckBoxColumn";
            // 
            // autoApplyDataGridViewCheckBoxColumn
            // 
            this.autoApplyDataGridViewCheckBoxColumn.DataPropertyName = "AutoApply";
            this.autoApplyDataGridViewCheckBoxColumn.HeaderText = "Auto Apply";
            this.autoApplyDataGridViewCheckBoxColumn.Name = "autoApplyDataGridViewCheckBoxColumn";
            this.autoApplyDataGridViewCheckBoxColumn.Visible = false;
            // 
            // baseMembershipIDDataGridViewTextBoxColumn
            // 
            this.baseMembershipIDDataGridViewTextBoxColumn.DataPropertyName = "BaseMembershipID";
            this.baseMembershipIDDataGridViewTextBoxColumn.HeaderText = "Base Membership";
            this.baseMembershipIDDataGridViewTextBoxColumn.Name = "baseMembershipIDDataGridViewTextBoxColumn";
            this.baseMembershipIDDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.baseMembershipIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // membershipRuleIDDataGridViewTextBoxColumn
            // 
            this.membershipRuleIDDataGridViewTextBoxColumn.DataPropertyName = "MembershipRuleID";
            this.membershipRuleIDDataGridViewTextBoxColumn.HeaderText = "Membership Rule";
            this.membershipRuleIDDataGridViewTextBoxColumn.Name = "membershipRuleIDDataGridViewTextBoxColumn";
            this.membershipRuleIDDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.membershipRuleIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // redemptionDiscountDataGridViewTextBoxColumn
            // 
            this.redemptionDiscountDataGridViewTextBoxColumn.DataPropertyName = "RedemptionDiscount";
            this.redemptionDiscountDataGridViewTextBoxColumn.HeaderText = "Redemption Discount";
            this.redemptionDiscountDataGridViewTextBoxColumn.Name = "redemptionDiscountDataGridViewTextBoxColumn";
            // 
            // priceListIdDataGridViewTextBoxColumn
            // 
            this.priceListIdDataGridViewTextBoxColumn.DataPropertyName = "PriceListId";
            this.priceListIdDataGridViewTextBoxColumn.HeaderText = "Price List";
            this.priceListIdDataGridViewTextBoxColumn.Name = "priceListIdDataGridViewTextBoxColumn";
            this.priceListIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.priceListIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // membershipDTOBindingSource
            // 
            this.membershipDTOBindingSource.DataSource = typeof(Semnox.Parafait.Customer.Membership.MembershipDTO);
            this.membershipDTOBindingSource.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.membershipDTOBindingSource_AddingNew);
            this.membershipDTOBindingSource.CurrentChanged += new System.EventHandler(this.Membership_CurrentChanged);
            // 
            // gpMembership
            // 
            this.gpMembership.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpMembership.Controls.Add(this.dgvMembership);
            this.gpMembership.Location = new System.Drawing.Point(8, 5);
            this.gpMembership.Name = "gpMembership";
            this.gpMembership.Size = new System.Drawing.Size(1044, 232);
            this.gpMembership.TabIndex = 23;
            this.gpMembership.TabStop = false;
            this.gpMembership.Text = "Membership";
            // 
            // gpMembershipRewards
            // 
            this.gpMembershipRewards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpMembershipRewards.Controls.Add(this.dgvMembershipRewards);
            this.gpMembershipRewards.Location = new System.Drawing.Point(8, 243);
            this.gpMembershipRewards.Name = "gpMembershipRewards";
            this.gpMembershipRewards.Size = new System.Drawing.Size(1044, 140);
            this.gpMembershipRewards.TabIndex = 24;
            this.gpMembershipRewards.TabStop = false;
            this.gpMembershipRewards.Text = "Membership Rewards";
            // 
            // dgvMembershipRewards
            // 
            this.dgvMembershipRewards.AutoGenerateColumns = false;
            this.dgvMembershipRewards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMembershipRewards.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.membershipRewardsIdDataGridViewTextBoxColumn,
            this.rewardNameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn1,
            this.rewardProductIDDataGridViewTextBoxColumn,
            this.rewardAttributeDataGridViewTextBoxColumn,
            this.rewardAttributePercentDataGridViewTextBoxColumn,
            this.rewardFunctionDataGridViewTextBoxColumn,
            this.rewardFunctionPeriodDataGridViewTextBoxColumn,
            this.unitOfRewardFunctionPeriodDataGridViewTextBoxColumn,
            this.rewardFrequencyDataGridViewTextBoxColumn,
            this.unitOfRewardFrequencyDataGridViewTextBoxColumn,
            this.expireWithMembershipDataGridViewCheckBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn1});
            this.dgvMembershipRewards.DataSource = this.membershipRewardsDTOBindingSource;
            this.dgvMembershipRewards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMembershipRewards.Location = new System.Drawing.Point(3, 16);
            this.dgvMembershipRewards.Name = "dgvMembershipRewards";
            this.dgvMembershipRewards.Size = new System.Drawing.Size(1038, 121);
            this.dgvMembershipRewards.TabIndex = 18;
            this.dgvMembershipRewards.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvMembershipRewards_EditingControlShowing);
            this.dgvMembershipRewards.Enter += new System.EventHandler(this.dgvMembershipRewards_Enter);
            // 
            // membershipRewardsIdDataGridViewTextBoxColumn
            // 
            this.membershipRewardsIdDataGridViewTextBoxColumn.DataPropertyName = "MembershipRewardsId";
            this.membershipRewardsIdDataGridViewTextBoxColumn.HeaderText = "Rewards Id";
            this.membershipRewardsIdDataGridViewTextBoxColumn.Name = "membershipRewardsIdDataGridViewTextBoxColumn";
            this.membershipRewardsIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // rewardNameDataGridViewTextBoxColumn
            // 
            this.rewardNameDataGridViewTextBoxColumn.DataPropertyName = "RewardName";
            this.rewardNameDataGridViewTextBoxColumn.HeaderText = "Reward Name";
            this.rewardNameDataGridViewTextBoxColumn.Name = "rewardNameDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn1
            // 
            this.descriptionDataGridViewTextBoxColumn1.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn1.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn1.Name = "descriptionDataGridViewTextBoxColumn1";
            // 
            // rewardProductIDDataGridViewTextBoxColumn
            // 
            this.rewardProductIDDataGridViewTextBoxColumn.DataPropertyName = "RewardProductID";
            this.rewardProductIDDataGridViewTextBoxColumn.HeaderText = "Reward Product";
            this.rewardProductIDDataGridViewTextBoxColumn.Name = "rewardProductIDDataGridViewTextBoxColumn";
            this.rewardProductIDDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.rewardProductIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // rewardAttributeDataGridViewTextBoxColumn
            // 
            this.rewardAttributeDataGridViewTextBoxColumn.DataPropertyName = "RewardAttribute";
            this.rewardAttributeDataGridViewTextBoxColumn.HeaderText = "Reward Attribute";
            this.rewardAttributeDataGridViewTextBoxColumn.Name = "rewardAttributeDataGridViewTextBoxColumn";
            this.rewardAttributeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.rewardAttributeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // rewardAttributePercentDataGridViewTextBoxColumn
            // 
            this.rewardAttributePercentDataGridViewTextBoxColumn.DataPropertyName = "RewardAttributePercent";
            this.rewardAttributePercentDataGridViewTextBoxColumn.HeaderText = "Reward Attribute Percent";
            this.rewardAttributePercentDataGridViewTextBoxColumn.Name = "rewardAttributePercentDataGridViewTextBoxColumn";
            // 
            // rewardFunctionDataGridViewTextBoxColumn
            // 
            this.rewardFunctionDataGridViewTextBoxColumn.DataPropertyName = "RewardFunction";
            this.rewardFunctionDataGridViewTextBoxColumn.HeaderText = "Reward Function";
            this.rewardFunctionDataGridViewTextBoxColumn.Name = "rewardFunctionDataGridViewTextBoxColumn";
            this.rewardFunctionDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.rewardFunctionDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // rewardFunctionPeriodDataGridViewTextBoxColumn
            // 
            this.rewardFunctionPeriodDataGridViewTextBoxColumn.DataPropertyName = "RewardFunctionPeriod";
            this.rewardFunctionPeriodDataGridViewTextBoxColumn.HeaderText = "Reward Function Period";
            this.rewardFunctionPeriodDataGridViewTextBoxColumn.Name = "rewardFunctionPeriodDataGridViewTextBoxColumn";
            this.rewardFunctionPeriodDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // unitOfRewardFunctionPeriodDataGridViewTextBoxColumn
            // 
            this.unitOfRewardFunctionPeriodDataGridViewTextBoxColumn.DataPropertyName = "UnitOfRewardFunctionPeriod";
            this.unitOfRewardFunctionPeriodDataGridViewTextBoxColumn.HeaderText = "Unit Of Reward Function Period";
            this.unitOfRewardFunctionPeriodDataGridViewTextBoxColumn.Name = "unitOfRewardFunctionPeriodDataGridViewTextBoxColumn";
            this.unitOfRewardFunctionPeriodDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.unitOfRewardFunctionPeriodDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // rewardFrequencyDataGridViewTextBoxColumn
            // 
            this.rewardFrequencyDataGridViewTextBoxColumn.DataPropertyName = "RewardFrequency";
            this.rewardFrequencyDataGridViewTextBoxColumn.HeaderText = "Reward Frequency";
            this.rewardFrequencyDataGridViewTextBoxColumn.Name = "rewardFrequencyDataGridViewTextBoxColumn";
            // 
            // unitOfRewardFrequencyDataGridViewTextBoxColumn
            // 
            this.unitOfRewardFrequencyDataGridViewTextBoxColumn.DataPropertyName = "UnitOfRewardFrequency";
            this.unitOfRewardFrequencyDataGridViewTextBoxColumn.HeaderText = "Unit Of Reward Frequency";
            this.unitOfRewardFrequencyDataGridViewTextBoxColumn.Name = "unitOfRewardFrequencyDataGridViewTextBoxColumn";
            this.unitOfRewardFrequencyDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.unitOfRewardFrequencyDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // expireWithMembershipDataGridViewCheckBoxColumn
            // 
            this.expireWithMembershipDataGridViewCheckBoxColumn.DataPropertyName = "ExpireWithMembership";
            this.expireWithMembershipDataGridViewCheckBoxColumn.HeaderText = "Expire With Membership";
            this.expireWithMembershipDataGridViewCheckBoxColumn.Name = "expireWithMembershipDataGridViewCheckBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn1
            // 
            this.isActiveDataGridViewCheckBoxColumn1.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn1.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn1.Name = "isActiveDataGridViewCheckBoxColumn1";
            // 
            // membershipRewardsDTOBindingSource
            // 
            this.membershipRewardsDTOBindingSource.DataSource = typeof(Semnox.Parafait.Customer.Membership.MembershipRewardsDTO);
            // 
            // btnExclusionRules
            // 
            this.btnExclusionRules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExclusionRules.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnExclusionRules.Location = new System.Drawing.Point(633, 397);
            this.btnExclusionRules.Name = "btnExclusionRules";
            this.btnExclusionRules.Size = new System.Drawing.Size(106, 23);
            this.btnExclusionRules.TabIndex = 25;
            this.btnExclusionRules.Text = "Exclusion Rules";
            this.btnExclusionRules.UseVisualStyleBackColor = true;
            this.btnExclusionRules.Click += new System.EventHandler(this.btnExclusionRules_Click);
            // 
            // btnMembershipRule
            // 
            this.btnMembershipRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMembershipRule.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnMembershipRule.Location = new System.Drawing.Point(474, 397);
            this.btnMembershipRule.Name = "btnMembershipRule";
            this.btnMembershipRule.Size = new System.Drawing.Size(121, 22);
            this.btnMembershipRule.TabIndex = 26;
            this.btnMembershipRule.Text = "Membership Rule";
            this.btnMembershipRule.UseVisualStyleBackColor = true;
            this.btnMembershipRule.Click += new System.EventHandler(this.btnMembershipRule_Click);
            // 
            // btnPublishToSite
            // 
            this.btnPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPublishToSite.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPublishToSite.Location = new System.Drawing.Point(777, 397);
            this.btnPublishToSite.Name = "btnPublishToSite";
            this.btnPublishToSite.Size = new System.Drawing.Size(109, 23);
            this.btnPublishToSite.TabIndex = 56;
            this.btnPublishToSite.Text = "Publish To Sites";
            this.btnPublishToSite.UseVisualStyleBackColor = true;
            this.btnPublishToSite.Click += new System.EventHandler(this.btnPublishToSite_Click);
            // 
            // MembershipUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 440);
            this.Controls.Add(this.btnPublishToSite);
            this.Controls.Add(this.btnMembershipRule);
            this.Controls.Add(this.btnExclusionRules);
            this.Controls.Add(this.gpMembershipRewards);
            this.Controls.Add(this.gpMembership);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.Name = "MembershipUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Membership ";
            this.Load += new System.EventHandler(this.MembershipUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembership)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.membershipDTOBindingSource)).EndInit();
            this.gpMembership.ResumeLayout(false);
            this.gpMembershipRewards.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembershipRewards)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.membershipRewardsDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvMembership;
        private System.Windows.Forms.GroupBox gpMembership;
        private System.Windows.Forms.GroupBox gpMembershipRewards;
        private System.Windows.Forms.DataGridView dgvMembershipRewards;
        private System.Windows.Forms.BindingSource membershipRewardsDTOBindingSource;
        private System.Windows.Forms.BindingSource membershipDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn membershipIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn membershipNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn vIPDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn autoApplyDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn baseMembershipIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn membershipRuleIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn redemptionDiscountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn priceListIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.Button btnExclusionRules;
        private System.Windows.Forms.Button btnMembershipRule;
        private System.Windows.Forms.DataGridViewTextBoxColumn membershipRewardsIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rewardNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn rewardProductIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn rewardAttributeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rewardAttributePercentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn rewardFunctionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rewardFunctionPeriodDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn unitOfRewardFunctionPeriodDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rewardFrequencyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn unitOfRewardFrequencyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn expireWithMembershipDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.Button btnPublishToSite;
    }
}
