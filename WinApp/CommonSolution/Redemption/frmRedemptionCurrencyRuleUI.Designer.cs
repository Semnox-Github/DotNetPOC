using System;
using System.Windows.Forms;

namespace Semnox.Parafait.Redemption
{
    partial class frmRedemptionCurrencyRuleUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRedemptionCurrencyRuleUI));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvRedemptionCurrencyRule = new System.Windows.Forms.DataGridView();
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redemptionCurrencyRuleNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.percentageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priorityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cumulativeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isChangedRecursiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.redemptionCurrencyRuleDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dgvRedemptionCurrencyRuleDetails = new System.Windows.Forms.DataGridView();
               this.redemptionCurrencyRuleDetailIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueInTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redemptionCurrencyRuleDetailDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptionCurrencyRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionCurrencyRuleDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptionCurrencyRuleDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionCurrencyRuleDetailDTOBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvRedemptionCurrencyRule
            // 
            this.dgvRedemptionCurrencyRule.AutoGenerateColumns = false;
            this.dgvRedemptionCurrencyRule.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvRedemptionCurrencyRule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRedemptionCurrencyRule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn,
            this.redemptionCurrencyRuleNameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.percentageDataGridViewTextBoxColumn,
            this.amountDataGridViewTextBoxColumn,
            this.priorityDataGridViewTextBoxColumn,
            this.cumulativeDataGridViewCheckBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn,
            this.isChangedRecursiveDataGridViewCheckBoxColumn});
            this.dgvRedemptionCurrencyRule.DataSource = this.redemptionCurrencyRuleDTOBindingSource;
            this.dgvRedemptionCurrencyRule.Location = new System.Drawing.Point(10, 19);
            this.dgvRedemptionCurrencyRule.MultiSelect = false;
            this.dgvRedemptionCurrencyRule.Name = "dgvRedemptionCurrencyRule";
            this.dgvRedemptionCurrencyRule.RowHeadersWidth = 20;
            this.dgvRedemptionCurrencyRule.Size = new System.Drawing.Size(1128, 281);
            this.dgvRedemptionCurrencyRule.TabIndex = 0;
            this.dgvRedemptionCurrencyRule.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRedemptionCurrencyRule_CellClick);
            this.dgvRedemptionCurrencyRule.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvRedemptionCurrencyRule_DataError);
            this.dgvRedemptionCurrencyRule.Enter += new System.EventHandler(this.dgvRedemptionCurrencyRule_Enter);
            //

            // redemptionCurrencyRuleIdDataGridViewTextBoxColumn
            // 
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn.DataPropertyName = "RedemptionCurrencyRuleId";
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn.HeaderText = "Rule Id";
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn.Name = "redemptionCurrencyRuleIdDataGridViewTextBoxColumn";
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn.Width = 66;
            // 
            // redemptionCurrencyRuleNameDataGridViewTextBoxColumn
            // 
            this.redemptionCurrencyRuleNameDataGridViewTextBoxColumn.DataPropertyName = "RedemptionCurrencyRuleName";
            this.redemptionCurrencyRuleNameDataGridViewTextBoxColumn.HeaderText = "Rule Name";
            this.redemptionCurrencyRuleNameDataGridViewTextBoxColumn.Name = "redemptionCurrencyRuleNameDataGridViewTextBoxColumn";
            this.redemptionCurrencyRuleNameDataGridViewTextBoxColumn.Width = 85;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.Width = 85;
            // 
            // percentageDataGridViewTextBoxColumn
            // 
            this.percentageDataGridViewTextBoxColumn.DataPropertyName = "Percentage";
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.percentageDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.percentageDataGridViewTextBoxColumn.HeaderText = "Bonus Percentage";
            this.percentageDataGridViewTextBoxColumn.Name = "percentageDataGridViewTextBoxColumn";
            this.percentageDataGridViewTextBoxColumn.Width = 110;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            this.amountDataGridViewTextBoxColumn.HeaderText = "Ticket";
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            this.amountDataGridViewTextBoxColumn.Width = 62;
            // 
            // priorityDataGridViewTextBoxColumn
            // 
            this.priorityDataGridViewTextBoxColumn.DataPropertyName = "Priority";
            this.priorityDataGridViewTextBoxColumn.HeaderText = "Priority";
            this.priorityDataGridViewTextBoxColumn.Name = "priorityDataGridViewTextBoxColumn";
            this.priorityDataGridViewTextBoxColumn.Width = 63;
            // 
            // cumulativeDataGridViewCheckBoxColumn
            // 
            this.cumulativeDataGridViewCheckBoxColumn.DataPropertyName = "Cumulative";
            this.cumulativeDataGridViewCheckBoxColumn.HeaderText = "Cumulative";
            this.cumulativeDataGridViewCheckBoxColumn.Name = "cumulativeDataGridViewCheckBoxColumn";
            this.cumulativeDataGridViewCheckBoxColumn.ReadOnly = true;
            this.cumulativeDataGridViewCheckBoxColumn.Width = 65;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewCheckBoxColumn.Width = 49;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "Created By";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.Width = 78;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "Creation Date";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.Width = 89;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Last Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.Width = 91;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "Last Update Date";
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.Width = 106;
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "Master Entity Id";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            this.masterEntityIdDataGridViewTextBoxColumn.Width = 88;
            // 
            // isChangedRecursiveDataGridViewCheckBoxColumn
            // 
            this.isChangedRecursiveDataGridViewCheckBoxColumn.DataPropertyName = "IsChangedRecursive";
            this.isChangedRecursiveDataGridViewCheckBoxColumn.HeaderText = "IsChangedRecursive";
            this.isChangedRecursiveDataGridViewCheckBoxColumn.Name = "isChangedRecursiveDataGridViewCheckBoxColumn";
            this.isChangedRecursiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isChangedRecursiveDataGridViewCheckBoxColumn.Visible = false;
            this.isChangedRecursiveDataGridViewCheckBoxColumn.Width = 112;
            // 
            // redemptionCurrencyRuleDTOBindingSource
            // 
            this.redemptionCurrencyRuleDTOBindingSource.DataSource = typeof(Semnox.Parafait.Redemption.RedemptionCurrencyRuleDTO);
            this.redemptionCurrencyRuleDTOBindingSource.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.redemptionCurrencyRulesDTOBindingSource_AddingNew);
            this.redemptionCurrencyRuleDTOBindingSource.CurrentItemChanged += new System.EventHandler(this.RedemptionCurrencyRules_CurrentChanged);
            //
            // dgvRedemptionCurrencyRuleDetails
            // 
            this.dgvRedemptionCurrencyRuleDetails.AutoGenerateColumns = false;
            this.dgvRedemptionCurrencyRuleDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRedemptionCurrencyRuleDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.redemptionCurrencyRuleDetailIdDataGridViewTextBoxColumn,
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn1,
            this.currencyIdDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn1,
            this.CreatedBy,
            this.creationDateDataGridViewTextBoxColumn1,
            this.lastUpdatedByDataGridViewTextBoxColumn1,
            this.lastUpdateDateDataGridViewTextBoxColumn1,
            this.masterEntityIdDataGridViewTextBoxColumn1,
            this.currencyNameDataGridViewTextBoxColumn,
            this.valueInTicketsDataGridViewTextBoxColumn});
            this.dgvRedemptionCurrencyRuleDetails.DataSource = this.redemptionCurrencyRuleDetailDTOBindingSource;
            this.dgvRedemptionCurrencyRuleDetails.Location = new System.Drawing.Point(10, 19);
            this.dgvRedemptionCurrencyRuleDetails.MultiSelect = false;
            this.dgvRedemptionCurrencyRuleDetails.Name = "dgvRedemptionCurrencyRuleDetails";
            this.dgvRedemptionCurrencyRuleDetails.RowHeadersWidth = 20;
            this.dgvRedemptionCurrencyRuleDetails.Size = new System.Drawing.Size(952, 184);
            this.dgvRedemptionCurrencyRuleDetails.TabIndex = 1;
            this.dgvRedemptionCurrencyRuleDetails.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRedemptionCurrencyRuleDetails_CellClick);
            this.dgvRedemptionCurrencyRuleDetails.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvRedemptionCurrencyRuleDetails_ComboDataError);
            this.dgvRedemptionCurrencyRuleDetails.Enter += new System.EventHandler(this.dgvRedemptionCurrencyRuleDetail_Enter);
            // 
            // redemptionCurrencyRuleDetailIdDataGridViewTextBoxColumn
            // 
            this.redemptionCurrencyRuleDetailIdDataGridViewTextBoxColumn.DataPropertyName = "RedemptionCurrencyRuleDetailId";
            this.redemptionCurrencyRuleDetailIdDataGridViewTextBoxColumn.HeaderText = "Id";
            this.redemptionCurrencyRuleDetailIdDataGridViewTextBoxColumn.Name = "redemptionCurrencyRuleDetailIdDataGridViewTextBoxColumn";
            // 
            // redemptionCurrencyRuleIdDataGridViewTextBoxColumn1
            // 
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn1.DataPropertyName = "RedemptionCurrencyRuleId";
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn1.HeaderText = "RedemptionCurrencyRuleId";
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn1.Name = "redemptionCurrencyRuleIdDataGridViewTextBoxColumn1";
            this.redemptionCurrencyRuleIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // currencyIdDataGridViewTextBoxColumn
            // 
            this.currencyIdDataGridViewTextBoxColumn.DataPropertyName = "CurrencyId";
            this.currencyIdDataGridViewTextBoxColumn.HeaderText = "Currency Name";
            this.currencyIdDataGridViewTextBoxColumn.Name = "currencyIdDataGridViewTextBoxColumn";
            this.currencyIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.currencyIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn1
            // 
            this.isActiveDataGridViewCheckBoxColumn1.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn1.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn1.Name = "isActiveDataGridViewCheckBoxColumn1";
            this.isActiveDataGridViewCheckBoxColumn1.ReadOnly = true;
            // 
            // CreatedBy
            // 
            this.CreatedBy.DataPropertyName = "CreatedBy";
            this.CreatedBy.HeaderText = "Created By";
            this.CreatedBy.Name = "CreatedBy";
            this.CreatedBy.ReadOnly = true;
            // 
            // creationDateDataGridViewTextBoxColumn1
            // 
            this.creationDateDataGridViewTextBoxColumn1.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn1.HeaderText = "Creation Date";
            this.creationDateDataGridViewTextBoxColumn1.Name = "creationDateDataGridViewTextBoxColumn1";
            this.creationDateDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn1
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn1.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn1.HeaderText = "Last Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn1.Name = "lastUpdatedByDataGridViewTextBoxColumn1";
            this.lastUpdatedByDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn1
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn1.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn1.HeaderText = "Last Update Date";
            this.lastUpdateDateDataGridViewTextBoxColumn1.Name = "lastUpdateDateDataGridViewTextBoxColumn1";
            this.lastUpdateDateDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // masterEntityIdDataGridViewTextBoxColumn1
            // 
            this.masterEntityIdDataGridViewTextBoxColumn1.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn1.HeaderText = "Master Entity Id";
            this.masterEntityIdDataGridViewTextBoxColumn1.Name = "masterEntityIdDataGridViewTextBoxColumn1";
            this.masterEntityIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // currencyNameDataGridViewTextBoxColumn
            // 
            this.currencyNameDataGridViewTextBoxColumn.DataPropertyName = "CurrencyName";
            this.currencyNameDataGridViewTextBoxColumn.HeaderText = "Currency Name";
            this.currencyNameDataGridViewTextBoxColumn.Name = "currencyNameDataGridViewTextBoxColumn";
            this.currencyNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // valueInTicketsDataGridViewTextBoxColumn
            // 
            this.valueInTicketsDataGridViewTextBoxColumn.DataPropertyName = "ValueInTickets";
            this.valueInTicketsDataGridViewTextBoxColumn.HeaderText = "Value In Tickets";
            this.valueInTicketsDataGridViewTextBoxColumn.Name = "valueInTicketsDataGridViewTextBoxColumn";
            this.valueInTicketsDataGridViewTextBoxColumn.Visible = false;
            // 
            // redemptionCurrencyRuleDetailDTOBindingSource
            // 
            this.redemptionCurrencyRuleDetailDTOBindingSource.DataSource = typeof(Semnox.Parafait.Redemption.RedemptionCurrencyRuleDetailDTO);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvRedemptionCurrencyRule);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1144, 306);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Redemption Currency Rule";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvRedemptionCurrencyRuleDetails);
            this.groupBox2.Location = new System.Drawing.Point(12, 336);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1144, 211);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rule Details";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(25, 557);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(108, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(167, 557);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(108, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh ";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(310, 557);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(108, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(453, 557);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(108, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmRedemptionCurrencyRuleUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1286, 749);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmRedemptionCurrencyRuleUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Redemption Currency Rule";
            this.Load += new System.EventHandler(this.frmRedemptionCurrencyRuleUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptionCurrencyRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionCurrencyRuleDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptionCurrencyRuleDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionCurrencyRuleDetailDTOBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.DataGridView dgvRedemptionCurrencyRule;
        private System.Windows.Forms.BindingSource redemptionCurrencyRuleDTOBindingSource;
        private System.Windows.Forms.DataGridView dgvRedemptionCurrencyRuleDetails;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private BindingSource redemptionCurrencyRuleDetailDTOBindingSource;
         private DataGridViewTextBoxColumn redemptionCurrencyRuleIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn redemptionCurrencyRuleNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn percentageDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn priorityDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn cumulativeDataGridViewCheckBoxColumn;
        private DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn isChangedRecursiveDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn redemptionCurrencyRuleDetailIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn redemptionCurrencyRuleIdDataGridViewTextBoxColumn1;
        private DataGridViewComboBoxColumn currencyIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn CreatedBy;
        private DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn currencyNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn valueInTicketsDataGridViewTextBoxColumn;
    }
}