namespace Parafait_POS.Subscription
{
    partial class frmSubscriptionHistoryView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSubscriptionHistoryView));
            this.pnlPage = new System.Windows.Forms.Panel();
            this.lblBillCycle = new System.Windows.Forms.Label();
            this.cmbBillingCycles = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.hScrollBillCycleHistory = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.dgvBillCycleHistory = new System.Windows.Forms.DataGridView();
            this.subscriptionBillingScheduleHistoryIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subscriptionBillingScheduleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subscriptionHeaderIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvBillingCycleTransactionId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.transactionLineIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billFromDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billToDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billOnDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overridedBillAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overrideReasonDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overrideByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overrideApprovedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.createdByDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.guidDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isChangedDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.subscriptionBillingScheduleHistoryDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vScrollBillCycleHistory = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.hScrollHeaderHistory = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.dgvSubscriptionHeaderHistory = new System.Windows.Forms.DataGridView();
            this.subscriptionHeaderHistoryDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vScrollHeader = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.SubscriptionHeaderHistoryId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subscriptionHeaderIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubscriptionNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productSubscriptionNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubscriptionStartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubscriptionEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subscriptionPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvHeaderTransactionId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.transactionLineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvHeaderCustomerId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvHeaderCustomerContactId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvHeaderCustomerCreditCardsID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.productSubscriptionDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subscriptionCycleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitOfSubscriptionCycleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subscriptionCycleValidityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taxInclusivePriceDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.selectedPaymentCollectionModeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subscriptionPaymentCollectionModeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.autoRenewDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.autoRenewalMarkupPercentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.allowPauseDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FreeTrialPeriodCycle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RenewalGracePeriodCycle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SeasonStartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PausedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PauseApprovedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnPausedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnPauseApprovedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CancellationOption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CancelledBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CancellationApprovedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billInAdvanceDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.noOfRenewalRemindersDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reminderFrequencyInDaysDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sendFirstReminderBeforeXDaysDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastRenewalReminderSentOnDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.renewalReminderCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastPaymentRetryLimitReminderSentOnDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentRetryLimitReminderCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.productsIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productSubscriptionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SourceSubscriptionHeaderId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.guidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isChangedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pnlPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillCycleHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subscriptionBillingScheduleHistoryDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubscriptionHeaderHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subscriptionHeaderHistoryDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlPage
            // 
            this.pnlPage.BackColor = System.Drawing.Color.Transparent;
            this.pnlPage.Controls.Add(this.lblBillCycle);
            this.pnlPage.Controls.Add(this.cmbBillingCycles);
            this.pnlPage.Controls.Add(this.hScrollBillCycleHistory);
            this.pnlPage.Controls.Add(this.vScrollBillCycleHistory);
            this.pnlPage.Controls.Add(this.dgvBillCycleHistory);
            this.pnlPage.Controls.Add(this.hScrollHeaderHistory);
            this.pnlPage.Controls.Add(this.vScrollHeader);
            this.pnlPage.Controls.Add(this.dgvSubscriptionHeaderHistory);
            this.pnlPage.Location = new System.Drawing.Point(0, 0);
            this.pnlPage.Name = "pnlPage";
            this.pnlPage.Size = new System.Drawing.Size(1258, 625);
            this.pnlPage.TabIndex = 0;
            // 
            // lblBillCycle
            // 
            this.lblBillCycle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblBillCycle.Location = new System.Drawing.Point(13, 305);
            this.lblBillCycle.Name = "lblBillCycle";
            this.lblBillCycle.Size = new System.Drawing.Size(124, 24);
            this.lblBillCycle.TabIndex = 7;
            this.lblBillCycle.Text = "Bill Cycle: ";
            this.lblBillCycle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbBillingCycles
            // 
            this.cmbBillingCycles.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBillingCycles.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBillingCycles.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbBillingCycles.FormattingEnabled = true;
            this.cmbBillingCycles.Location = new System.Drawing.Point(137, 301);
            this.cmbBillingCycles.Name = "cmbBillingCycles";
            this.cmbBillingCycles.Size = new System.Drawing.Size(205, 31);
            this.cmbBillingCycles.TabIndex = 6;
            this.cmbBillingCycles.SelectedIndexChanged += new System.EventHandler(this.cmbBillingCycles_SelectedIndexChanged);
            // 
            // hScrollBillCycleHistory
            // 
            this.hScrollBillCycleHistory.AutoHide = false;
            this.hScrollBillCycleHistory.DataGridView = this.dgvBillCycleHistory;
            this.hScrollBillCycleHistory.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollBillCycleHistory.LeftButtonBackgroundImage")));
            this.hScrollBillCycleHistory.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollBillCycleHistory.LeftButtonDisabledBackgroundImage")));
            this.hScrollBillCycleHistory.Location = new System.Drawing.Point(13, 578);
            this.hScrollBillCycleHistory.Margin = new System.Windows.Forms.Padding(0);
            this.hScrollBillCycleHistory.Name = "hScrollBillCycleHistory";
            this.hScrollBillCycleHistory.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollBillCycleHistory.RightButtonBackgroundImage")));
            this.hScrollBillCycleHistory.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollBillCycleHistory.RightButtonDisabledBackgroundImage")));
            this.hScrollBillCycleHistory.ScrollableControl = null;
            this.hScrollBillCycleHistory.ScrollViewer = null;
            this.hScrollBillCycleHistory.Size = new System.Drawing.Size(1196, 40);
            this.hScrollBillCycleHistory.TabIndex = 5;
            // 
            // dgvBillCycleHistory
            // 
            this.dgvBillCycleHistory.AllowUserToAddRows = false;
            this.dgvBillCycleHistory.AllowUserToDeleteRows = false;
            this.dgvBillCycleHistory.AutoGenerateColumns = false;
            this.dgvBillCycleHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBillCycleHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.subscriptionBillingScheduleHistoryIdDataGridViewTextBoxColumn,
            this.subscriptionBillingScheduleIdDataGridViewTextBoxColumn,
            this.LineType,
            this.subscriptionHeaderIdDataGridViewTextBoxColumn1,
            this.dgvBillingCycleTransactionId,
            this.transactionLineIdDataGridViewTextBoxColumn1,
            this.billFromDateDataGridViewTextBoxColumn,
            this.billToDateDataGridViewTextBoxColumn,
            this.billOnDateDataGridViewTextBoxColumn,
            this.billAmountDataGridViewTextBoxColumn,
            this.overridedBillAmountDataGridViewTextBoxColumn,
            this.overrideReasonDataGridViewTextBoxColumn,
            this.overrideByDataGridViewTextBoxColumn,
            this.overrideApprovedByDataGridViewTextBoxColumn,
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.isActiveDataGridViewCheckBoxColumn1,
            this.createdByDataGridViewTextBoxColumn1,
            this.creationDateDataGridViewTextBoxColumn1,
            this.lastUpdatedByDataGridViewTextBoxColumn1,
            this.lastUpdateDateDataGridViewTextBoxColumn1,
            this.siteIdDataGridViewTextBoxColumn1,
            this.masterEntityIdDataGridViewTextBoxColumn1,
            this.synchStatusDataGridViewCheckBoxColumn1,
            this.guidDataGridViewTextBoxColumn1,
            this.isChangedDataGridViewCheckBoxColumn1});
            this.dgvBillCycleHistory.DataSource = this.subscriptionBillingScheduleHistoryDTOBindingSource;
            this.dgvBillCycleHistory.Location = new System.Drawing.Point(13, 338);
            this.dgvBillCycleHistory.Name = "dgvBillCycleHistory";
            this.dgvBillCycleHistory.ReadOnly = true;
            this.dgvBillCycleHistory.RowHeadersVisible = false;
            this.dgvBillCycleHistory.RowTemplate.Height = 30;
            this.dgvBillCycleHistory.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvBillCycleHistory.Size = new System.Drawing.Size(1193, 237);
            this.dgvBillCycleHistory.TabIndex = 3;
            this.dgvBillCycleHistory.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvBillCycleHistory_CellFormatting);
            this.dgvBillCycleHistory.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvBillCycleHistory_DataError);
            // 
            // subscriptionBillingScheduleHistoryIdDataGridViewTextBoxColumn
            // 
            this.subscriptionBillingScheduleHistoryIdDataGridViewTextBoxColumn.DataPropertyName = "SubscriptionBillingScheduleHistoryId";
            this.subscriptionBillingScheduleHistoryIdDataGridViewTextBoxColumn.HeaderText = "History Id";
            this.subscriptionBillingScheduleHistoryIdDataGridViewTextBoxColumn.Name = "subscriptionBillingScheduleHistoryIdDataGridViewTextBoxColumn";
            this.subscriptionBillingScheduleHistoryIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // subscriptionBillingScheduleIdDataGridViewTextBoxColumn
            // 
            this.subscriptionBillingScheduleIdDataGridViewTextBoxColumn.DataPropertyName = "SubscriptionBillingScheduleId";
            this.subscriptionBillingScheduleIdDataGridViewTextBoxColumn.HeaderText = "Billing Schedule Id";
            this.subscriptionBillingScheduleIdDataGridViewTextBoxColumn.Name = "subscriptionBillingScheduleIdDataGridViewTextBoxColumn";
            this.subscriptionBillingScheduleIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // LineType
            // 
            this.LineType.DataPropertyName = "LineType";
            this.LineType.HeaderText = "Line Type";
            this.LineType.Name = "LineType";
            this.LineType.ReadOnly = true;
            // 
            // subscriptionHeaderIdDataGridViewTextBoxColumn1
            // 
            this.subscriptionHeaderIdDataGridViewTextBoxColumn1.DataPropertyName = "SubscriptionHeaderId";
            this.subscriptionHeaderIdDataGridViewTextBoxColumn1.HeaderText = "SubscriptionHeaderId";
            this.subscriptionHeaderIdDataGridViewTextBoxColumn1.Name = "subscriptionHeaderIdDataGridViewTextBoxColumn1";
            this.subscriptionHeaderIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.subscriptionHeaderIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // dgvBillingCycleTransactionId
            // 
            this.dgvBillingCycleTransactionId.DataPropertyName = "TransactionId";
            this.dgvBillingCycleTransactionId.HeaderText = "Transaction No";
            this.dgvBillingCycleTransactionId.Name = "dgvBillingCycleTransactionId";
            this.dgvBillingCycleTransactionId.ReadOnly = true;
            this.dgvBillingCycleTransactionId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBillingCycleTransactionId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // transactionLineIdDataGridViewTextBoxColumn1
            // 
            this.transactionLineIdDataGridViewTextBoxColumn1.DataPropertyName = "TransactionLineId";
            this.transactionLineIdDataGridViewTextBoxColumn1.HeaderText = "Line Id";
            this.transactionLineIdDataGridViewTextBoxColumn1.Name = "transactionLineIdDataGridViewTextBoxColumn1";
            this.transactionLineIdDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // billFromDateDataGridViewTextBoxColumn
            // 
            this.billFromDateDataGridViewTextBoxColumn.DataPropertyName = "BillFromDate";
            this.billFromDateDataGridViewTextBoxColumn.HeaderText = "Bill From Date";
            this.billFromDateDataGridViewTextBoxColumn.Name = "billFromDateDataGridViewTextBoxColumn";
            this.billFromDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // billToDateDataGridViewTextBoxColumn
            // 
            this.billToDateDataGridViewTextBoxColumn.DataPropertyName = "BillToDate";
            this.billToDateDataGridViewTextBoxColumn.HeaderText = "Bill To Date";
            this.billToDateDataGridViewTextBoxColumn.Name = "billToDateDataGridViewTextBoxColumn";
            this.billToDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // billOnDateDataGridViewTextBoxColumn
            // 
            this.billOnDateDataGridViewTextBoxColumn.DataPropertyName = "BillOnDate";
            this.billOnDateDataGridViewTextBoxColumn.HeaderText = "Bill On Date";
            this.billOnDateDataGridViewTextBoxColumn.Name = "billOnDateDataGridViewTextBoxColumn";
            this.billOnDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // billAmountDataGridViewTextBoxColumn
            // 
            this.billAmountDataGridViewTextBoxColumn.DataPropertyName = "BillAmount";
            this.billAmountDataGridViewTextBoxColumn.HeaderText = "Bill Amount";
            this.billAmountDataGridViewTextBoxColumn.Name = "billAmountDataGridViewTextBoxColumn";
            this.billAmountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // overridedBillAmountDataGridViewTextBoxColumn
            // 
            this.overridedBillAmountDataGridViewTextBoxColumn.DataPropertyName = "OverridedBillAmount";
            this.overridedBillAmountDataGridViewTextBoxColumn.HeaderText = "Overrided Bill Amount";
            this.overridedBillAmountDataGridViewTextBoxColumn.Name = "overridedBillAmountDataGridViewTextBoxColumn";
            this.overridedBillAmountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // overrideReasonDataGridViewTextBoxColumn
            // 
            this.overrideReasonDataGridViewTextBoxColumn.DataPropertyName = "OverrideReason";
            this.overrideReasonDataGridViewTextBoxColumn.HeaderText = "Override Reason";
            this.overrideReasonDataGridViewTextBoxColumn.Name = "overrideReasonDataGridViewTextBoxColumn";
            this.overrideReasonDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // overrideByDataGridViewTextBoxColumn
            // 
            this.overrideByDataGridViewTextBoxColumn.DataPropertyName = "OverrideBy";
            this.overrideByDataGridViewTextBoxColumn.HeaderText = "Override By";
            this.overrideByDataGridViewTextBoxColumn.Name = "overrideByDataGridViewTextBoxColumn";
            this.overrideByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // overrideApprovedByDataGridViewTextBoxColumn
            // 
            this.overrideApprovedByDataGridViewTextBoxColumn.DataPropertyName = "OverrideApprovedBy";
            this.overrideApprovedByDataGridViewTextBoxColumn.HeaderText = "Override Approved By";
            this.overrideApprovedByDataGridViewTextBoxColumn.Name = "overrideApprovedByDataGridViewTextBoxColumn";
            this.overrideApprovedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // paymentProcessingFailureCountDataGridViewTextBoxColumn
            // 
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.DataPropertyName = "PaymentProcessingFailureCount";
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.HeaderText = "Payment Processing Failure Count";
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.Name = "paymentProcessingFailureCountDataGridViewTextBoxColumn";
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // statusDataGridViewTextBoxColumn1
            // 
            this.statusDataGridViewTextBoxColumn1.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn1.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn1.Name = "statusDataGridViewTextBoxColumn1";
            this.statusDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CancelledBy";
            this.dataGridViewTextBoxColumn1.HeaderText = "Cancelled By";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "CancellationApprovedBy";
            this.dataGridViewTextBoxColumn2.HeaderText = "Cancellation Approved By";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // isActiveDataGridViewCheckBoxColumn1
            // 
            this.isActiveDataGridViewCheckBoxColumn1.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn1.HeaderText = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn1.Name = "isActiveDataGridViewCheckBoxColumn1";
            this.isActiveDataGridViewCheckBoxColumn1.ReadOnly = true;
            // 
            // createdByDataGridViewTextBoxColumn1
            // 
            this.createdByDataGridViewTextBoxColumn1.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn1.HeaderText = "Created By";
            this.createdByDataGridViewTextBoxColumn1.Name = "createdByDataGridViewTextBoxColumn1";
            this.createdByDataGridViewTextBoxColumn1.ReadOnly = true;
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
            // siteIdDataGridViewTextBoxColumn1
            // 
            this.siteIdDataGridViewTextBoxColumn1.DataPropertyName = "SiteId";
            this.siteIdDataGridViewTextBoxColumn1.HeaderText = "SiteId";
            this.siteIdDataGridViewTextBoxColumn1.Name = "siteIdDataGridViewTextBoxColumn1";
            this.siteIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.siteIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // masterEntityIdDataGridViewTextBoxColumn1
            // 
            this.masterEntityIdDataGridViewTextBoxColumn1.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn1.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn1.Name = "masterEntityIdDataGridViewTextBoxColumn1";
            this.masterEntityIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.masterEntityIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn1
            // 
            this.synchStatusDataGridViewCheckBoxColumn1.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn1.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn1.Name = "synchStatusDataGridViewCheckBoxColumn1";
            this.synchStatusDataGridViewCheckBoxColumn1.ReadOnly = true;
            this.synchStatusDataGridViewCheckBoxColumn1.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn1
            // 
            this.guidDataGridViewTextBoxColumn1.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn1.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn1.Name = "guidDataGridViewTextBoxColumn1";
            this.guidDataGridViewTextBoxColumn1.ReadOnly = true;
            this.guidDataGridViewTextBoxColumn1.Visible = false;
            // 
            // isChangedDataGridViewCheckBoxColumn1
            // 
            this.isChangedDataGridViewCheckBoxColumn1.DataPropertyName = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn1.HeaderText = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn1.Name = "isChangedDataGridViewCheckBoxColumn1";
            this.isChangedDataGridViewCheckBoxColumn1.ReadOnly = true;
            this.isChangedDataGridViewCheckBoxColumn1.Visible = false;
            // 
            // subscriptionBillingScheduleHistoryDTOBindingSource
            // 
            this.subscriptionBillingScheduleHistoryDTOBindingSource.DataSource = typeof(Semnox.Parafait.Transaction.SubscriptionBillingScheduleHistoryDTO);
            // 
            // vScrollBillCycleHistory
            // 
            this.vScrollBillCycleHistory.AutoHide = false;
            this.vScrollBillCycleHistory.DataGridView = this.dgvBillCycleHistory;
            this.vScrollBillCycleHistory.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBillCycleHistory.DownButtonBackgroundImage")));
            this.vScrollBillCycleHistory.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBillCycleHistory.DownButtonDisabledBackgroundImage")));
            this.vScrollBillCycleHistory.Location = new System.Drawing.Point(1211, 338);
            this.vScrollBillCycleHistory.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollBillCycleHistory.Name = "vScrollBillCycleHistory";
            this.vScrollBillCycleHistory.ScrollableControl = null;
            this.vScrollBillCycleHistory.ScrollViewer = null;
            this.vScrollBillCycleHistory.Size = new System.Drawing.Size(40, 237);
            this.vScrollBillCycleHistory.TabIndex = 4;
            this.vScrollBillCycleHistory.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBillCycleHistory.UpButtonBackgroundImage")));
            this.vScrollBillCycleHistory.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBillCycleHistory.UpButtonDisabledBackgroundImage")));
            // 
            // hScrollHeaderHistory
            // 
            this.hScrollHeaderHistory.AutoHide = false;
            this.hScrollHeaderHistory.DataGridView = this.dgvSubscriptionHeaderHistory;
            this.hScrollHeaderHistory.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollHeaderHistory.LeftButtonBackgroundImage")));
            this.hScrollHeaderHistory.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollHeaderHistory.LeftButtonDisabledBackgroundImage")));
            this.hScrollHeaderHistory.Location = new System.Drawing.Point(13, 254);
            this.hScrollHeaderHistory.Margin = new System.Windows.Forms.Padding(0);
            this.hScrollHeaderHistory.Name = "hScrollHeaderHistory";
            this.hScrollHeaderHistory.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollHeaderHistory.RightButtonBackgroundImage")));
            this.hScrollHeaderHistory.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollHeaderHistory.RightButtonDisabledBackgroundImage")));
            this.hScrollHeaderHistory.ScrollableControl = null;
            this.hScrollHeaderHistory.ScrollViewer = null;
            this.hScrollHeaderHistory.Size = new System.Drawing.Size(1196, 40);
            this.hScrollHeaderHistory.TabIndex = 2;
            // 
            // dgvSubscriptionHeaderHistory
            // 
            this.dgvSubscriptionHeaderHistory.AllowUserToAddRows = false;
            this.dgvSubscriptionHeaderHistory.AllowUserToDeleteRows = false;
            this.dgvSubscriptionHeaderHistory.AutoGenerateColumns = false;
            this.dgvSubscriptionHeaderHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubscriptionHeaderHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SubscriptionHeaderHistoryId,
            this.subscriptionHeaderIdDataGridViewTextBoxColumn,
            this.SubscriptionNumber,
            this.productSubscriptionNameDataGridViewTextBoxColumn,
            this.SubscriptionStartDate,
            this.SubscriptionEndDate,
            this.subscriptionPriceDataGridViewTextBoxColumn,
            this.dgvHeaderTransactionId,
            this.transactionLineIdDataGridViewTextBoxColumn,
            this.dgvHeaderCustomerId,
            this.dgvHeaderCustomerContactId,
            this.dgvHeaderCustomerCreditCardsID,
            this.productSubscriptionDescriptionDataGridViewTextBoxColumn,
            this.subscriptionCycleDataGridViewTextBoxColumn,
            this.unitOfSubscriptionCycleDataGridViewTextBoxColumn,
            this.subscriptionCycleValidityDataGridViewTextBoxColumn,
            this.taxInclusivePriceDataGridViewCheckBoxColumn,
            this.selectedPaymentCollectionModeDataGridViewTextBoxColumn,
            this.subscriptionPaymentCollectionModeDataGridViewTextBoxColumn,
            this.autoRenewDataGridViewCheckBoxColumn,
            this.autoRenewalMarkupPercentDataGridViewTextBoxColumn,
            this.allowPauseDataGridViewCheckBoxColumn,
            this.FreeTrialPeriodCycle,
            this.RenewalGracePeriodCycle,
            this.SeasonStartDate,
            this.statusDataGridViewTextBoxColumn,
            this.PausedBy,
            this.PauseApprovedBy,
            this.UnPausedBy,
            this.UnPauseApprovedBy,
            this.CancellationOption,
            this.CancelledBy,
            this.CancellationApprovedBy,
            this.billInAdvanceDataGridViewCheckBoxColumn,
            this.noOfRenewalRemindersDataGridViewTextBoxColumn,
            this.reminderFrequencyInDaysDataGridViewTextBoxColumn,
            this.sendFirstReminderBeforeXDaysDataGridViewTextBoxColumn,
            this.lastRenewalReminderSentOnDataGridViewTextBoxColumn,
            this.renewalReminderCountDataGridViewTextBoxColumn,
            this.lastPaymentRetryLimitReminderSentOnDataGridViewTextBoxColumn,
            this.paymentRetryLimitReminderCountDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.productsIdDataGridViewTextBoxColumn,
            this.productSubscriptionIdDataGridViewTextBoxColumn,
            this.SourceSubscriptionHeaderId,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn,
            this.siteIdDataGridViewTextBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn,
            this.synchStatusDataGridViewCheckBoxColumn,
            this.guidDataGridViewTextBoxColumn,
            this.isChangedDataGridViewCheckBoxColumn});
            this.dgvSubscriptionHeaderHistory.DataSource = this.subscriptionHeaderHistoryDTOBindingSource;
            this.dgvSubscriptionHeaderHistory.Location = new System.Drawing.Point(13, 13);
            this.dgvSubscriptionHeaderHistory.Name = "dgvSubscriptionHeaderHistory";
            this.dgvSubscriptionHeaderHistory.ReadOnly = true;
            this.dgvSubscriptionHeaderHistory.RowHeadersVisible = false;
            this.dgvSubscriptionHeaderHistory.RowTemplate.Height = 30;
            this.dgvSubscriptionHeaderHistory.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvSubscriptionHeaderHistory.Size = new System.Drawing.Size(1193, 239);
            this.dgvSubscriptionHeaderHistory.TabIndex = 0;
            this.dgvSubscriptionHeaderHistory.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSubscriptionHeaderHistory_CellFormatting);
            this.dgvSubscriptionHeaderHistory.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvSubscriptionHeaderHistory_DataError);
            // 
            // subscriptionHeaderHistoryDTOBindingSource
            // 
            this.subscriptionHeaderHistoryDTOBindingSource.DataSource = typeof(Semnox.Parafait.Transaction.SubscriptionHeaderHistoryDTO);
            // 
            // vScrollHeader
            // 
            this.vScrollHeader.AutoHide = false;
            this.vScrollHeader.DataGridView = this.dgvSubscriptionHeaderHistory;
            this.vScrollHeader.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollHeader.DownButtonBackgroundImage")));
            this.vScrollHeader.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollHeader.DownButtonDisabledBackgroundImage")));
            this.vScrollHeader.Location = new System.Drawing.Point(1208, 13);
            this.vScrollHeader.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollHeader.Name = "vScrollHeader";
            this.vScrollHeader.ScrollableControl = null;
            this.vScrollHeader.ScrollViewer = null;
            this.vScrollHeader.Size = new System.Drawing.Size(40, 239);
            this.vScrollHeader.TabIndex = 1;
            this.vScrollHeader.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollHeader.UpButtonBackgroundImage")));
            this.vScrollHeader.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollHeader.UpButtonDisabledBackgroundImage")));
            // 
            // SubscriptionHeaderHistoryId
            // 
            this.SubscriptionHeaderHistoryId.DataPropertyName = "SubscriptionHeaderHistoryId";
            this.SubscriptionHeaderHistoryId.HeaderText = "History Id";
            this.SubscriptionHeaderHistoryId.Name = "SubscriptionHeaderHistoryId";
            this.SubscriptionHeaderHistoryId.ReadOnly = true;
            // 
            // subscriptionHeaderIdDataGridViewTextBoxColumn
            // 
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.DataPropertyName = "SubscriptionHeaderId";
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.HeaderText = "Id";
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.Name = "subscriptionHeaderIdDataGridViewTextBoxColumn";
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // SubscriptionNumber
            // 
            this.SubscriptionNumber.DataPropertyName = "SubscriptionNumber";
            this.SubscriptionNumber.HeaderText = "Subscription No";
            this.SubscriptionNumber.Name = "SubscriptionNumber";
            this.SubscriptionNumber.ReadOnly = true;
            // 
            // productSubscriptionNameDataGridViewTextBoxColumn
            // 
            this.productSubscriptionNameDataGridViewTextBoxColumn.DataPropertyName = "ProductSubscriptionName";
            this.productSubscriptionNameDataGridViewTextBoxColumn.HeaderText = "Subscription Name";
            this.productSubscriptionNameDataGridViewTextBoxColumn.Name = "productSubscriptionNameDataGridViewTextBoxColumn";
            this.productSubscriptionNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // SubscriptionStartDate
            // 
            this.SubscriptionStartDate.DataPropertyName = "SubscriptionStartDate";
            this.SubscriptionStartDate.HeaderText = "Subscription Start Date";
            this.SubscriptionStartDate.Name = "SubscriptionStartDate";
            this.SubscriptionStartDate.ReadOnly = true;
            // 
            // SubscriptionEndDate
            // 
            this.SubscriptionEndDate.DataPropertyName = "SubscriptionEndDate";
            this.SubscriptionEndDate.HeaderText = "Subscription End Date";
            this.SubscriptionEndDate.Name = "SubscriptionEndDate";
            this.SubscriptionEndDate.ReadOnly = true;
            // 
            // subscriptionPriceDataGridViewTextBoxColumn
            // 
            this.subscriptionPriceDataGridViewTextBoxColumn.DataPropertyName = "SubscriptionPrice";
            this.subscriptionPriceDataGridViewTextBoxColumn.HeaderText = "Subscription Price";
            this.subscriptionPriceDataGridViewTextBoxColumn.Name = "subscriptionPriceDataGridViewTextBoxColumn";
            this.subscriptionPriceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dgvHeaderTransactionId
            // 
            this.dgvHeaderTransactionId.DataPropertyName = "TransactionId";
            this.dgvHeaderTransactionId.HeaderText = "Transaction No";
            this.dgvHeaderTransactionId.Name = "dgvHeaderTransactionId";
            this.dgvHeaderTransactionId.ReadOnly = true;
            this.dgvHeaderTransactionId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHeaderTransactionId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // transactionLineIdDataGridViewTextBoxColumn
            // 
            this.transactionLineIdDataGridViewTextBoxColumn.DataPropertyName = "TransactionLineId";
            this.transactionLineIdDataGridViewTextBoxColumn.HeaderText = "LineId";
            this.transactionLineIdDataGridViewTextBoxColumn.Name = "transactionLineIdDataGridViewTextBoxColumn";
            this.transactionLineIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dgvHeaderCustomerId
            // 
            this.dgvHeaderCustomerId.DataPropertyName = "CustomerId";
            this.dgvHeaderCustomerId.HeaderText = "Customer ";
            this.dgvHeaderCustomerId.Name = "dgvHeaderCustomerId";
            this.dgvHeaderCustomerId.ReadOnly = true;
            this.dgvHeaderCustomerId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHeaderCustomerId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvHeaderCustomerContactId
            // 
            this.dgvHeaderCustomerContactId.DataPropertyName = "CustomerContactId";
            this.dgvHeaderCustomerContactId.HeaderText = "Customer Contact";
            this.dgvHeaderCustomerContactId.Name = "dgvHeaderCustomerContactId";
            this.dgvHeaderCustomerContactId.ReadOnly = true;
            this.dgvHeaderCustomerContactId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHeaderCustomerContactId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvHeaderCustomerCreditCardsID
            // 
            this.dgvHeaderCustomerCreditCardsID.DataPropertyName = "CustomerCreditCardsID";
            this.dgvHeaderCustomerCreditCardsID.HeaderText = "Credit Card";
            this.dgvHeaderCustomerCreditCardsID.Name = "dgvHeaderCustomerCreditCardsID";
            this.dgvHeaderCustomerCreditCardsID.ReadOnly = true;
            // 
            // productSubscriptionDescriptionDataGridViewTextBoxColumn
            // 
            this.productSubscriptionDescriptionDataGridViewTextBoxColumn.DataPropertyName = "ProductSubscriptionDescription";
            this.productSubscriptionDescriptionDataGridViewTextBoxColumn.HeaderText = "Subscription Description";
            this.productSubscriptionDescriptionDataGridViewTextBoxColumn.Name = "productSubscriptionDescriptionDataGridViewTextBoxColumn";
            this.productSubscriptionDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // subscriptionCycleDataGridViewTextBoxColumn
            // 
            this.subscriptionCycleDataGridViewTextBoxColumn.DataPropertyName = "SubscriptionCycle";
            this.subscriptionCycleDataGridViewTextBoxColumn.HeaderText = "Subscription Cycle";
            this.subscriptionCycleDataGridViewTextBoxColumn.Name = "subscriptionCycleDataGridViewTextBoxColumn";
            this.subscriptionCycleDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // unitOfSubscriptionCycleDataGridViewTextBoxColumn
            // 
            this.unitOfSubscriptionCycleDataGridViewTextBoxColumn.DataPropertyName = "UnitOfSubscriptionCycle";
            this.unitOfSubscriptionCycleDataGridViewTextBoxColumn.HeaderText = "Unit Of Subscription Cycle";
            this.unitOfSubscriptionCycleDataGridViewTextBoxColumn.Name = "unitOfSubscriptionCycleDataGridViewTextBoxColumn";
            this.unitOfSubscriptionCycleDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // subscriptionCycleValidityDataGridViewTextBoxColumn
            // 
            this.subscriptionCycleValidityDataGridViewTextBoxColumn.DataPropertyName = "SubscriptionCycleValidity";
            this.subscriptionCycleValidityDataGridViewTextBoxColumn.HeaderText = "Subscription Cycle Validity";
            this.subscriptionCycleValidityDataGridViewTextBoxColumn.Name = "subscriptionCycleValidityDataGridViewTextBoxColumn";
            this.subscriptionCycleValidityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // taxInclusivePriceDataGridViewCheckBoxColumn
            // 
            this.taxInclusivePriceDataGridViewCheckBoxColumn.DataPropertyName = "TaxInclusivePrice";
            this.taxInclusivePriceDataGridViewCheckBoxColumn.HeaderText = "Tax Inclusive Price";
            this.taxInclusivePriceDataGridViewCheckBoxColumn.Name = "taxInclusivePriceDataGridViewCheckBoxColumn";
            this.taxInclusivePriceDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // selectedPaymentCollectionModeDataGridViewTextBoxColumn
            // 
            this.selectedPaymentCollectionModeDataGridViewTextBoxColumn.DataPropertyName = "SelectedPaymentCollectionMode";
            this.selectedPaymentCollectionModeDataGridViewTextBoxColumn.HeaderText = "Selected Payment Collection Mode";
            this.selectedPaymentCollectionModeDataGridViewTextBoxColumn.Name = "selectedPaymentCollectionModeDataGridViewTextBoxColumn";
            this.selectedPaymentCollectionModeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // subscriptionPaymentCollectionModeDataGridViewTextBoxColumn
            // 
            this.subscriptionPaymentCollectionModeDataGridViewTextBoxColumn.DataPropertyName = "SubscriptionPaymentCollectionMode";
            this.subscriptionPaymentCollectionModeDataGridViewTextBoxColumn.HeaderText = "SubscriptionPaymentCollectionMode";
            this.subscriptionPaymentCollectionModeDataGridViewTextBoxColumn.Name = "subscriptionPaymentCollectionModeDataGridViewTextBoxColumn";
            this.subscriptionPaymentCollectionModeDataGridViewTextBoxColumn.ReadOnly = true;
            this.subscriptionPaymentCollectionModeDataGridViewTextBoxColumn.Visible = false;
            // 
            // autoRenewDataGridViewCheckBoxColumn
            // 
            this.autoRenewDataGridViewCheckBoxColumn.DataPropertyName = "AutoRenew";
            this.autoRenewDataGridViewCheckBoxColumn.HeaderText = "Auto Renew";
            this.autoRenewDataGridViewCheckBoxColumn.Name = "autoRenewDataGridViewCheckBoxColumn";
            this.autoRenewDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // autoRenewalMarkupPercentDataGridViewTextBoxColumn
            // 
            this.autoRenewalMarkupPercentDataGridViewTextBoxColumn.DataPropertyName = "AutoRenewalMarkupPercent";
            this.autoRenewalMarkupPercentDataGridViewTextBoxColumn.HeaderText = "Auto Renewal Markup Percent";
            this.autoRenewalMarkupPercentDataGridViewTextBoxColumn.Name = "autoRenewalMarkupPercentDataGridViewTextBoxColumn";
            this.autoRenewalMarkupPercentDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // allowPauseDataGridViewCheckBoxColumn
            // 
            this.allowPauseDataGridViewCheckBoxColumn.DataPropertyName = "AllowPause";
            this.allowPauseDataGridViewCheckBoxColumn.HeaderText = "Allow Pause";
            this.allowPauseDataGridViewCheckBoxColumn.Name = "allowPauseDataGridViewCheckBoxColumn";
            this.allowPauseDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // FreeTrialPeriodCycle
            // 
            this.FreeTrialPeriodCycle.DataPropertyName = "FreeTrialPeriodCycle";
            this.FreeTrialPeriodCycle.HeaderText = "Free Trial Period Cycle";
            this.FreeTrialPeriodCycle.Name = "FreeTrialPeriodCycle";
            this.FreeTrialPeriodCycle.ReadOnly = true;
            // 
            // RenewalGracePeriodCycle
            // 
            this.RenewalGracePeriodCycle.DataPropertyName = "RenewalGracePeriodCycle";
            this.RenewalGracePeriodCycle.HeaderText = "Renewal Grace Period Cycle";
            this.RenewalGracePeriodCycle.Name = "RenewalGracePeriodCycle";
            this.RenewalGracePeriodCycle.ReadOnly = true;
            // 
            // SeasonStartDate
            // 
            this.SeasonStartDate.DataPropertyName = "SeasonStartDate";
            this.SeasonStartDate.HeaderText = "Season Start Date";
            this.SeasonStartDate.Name = "SeasonStartDate";
            this.SeasonStartDate.ReadOnly = true;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // PausedBy
            // 
            this.PausedBy.DataPropertyName = "PausedBy";
            this.PausedBy.HeaderText = "Paused By";
            this.PausedBy.Name = "PausedBy";
            this.PausedBy.ReadOnly = true;
            // 
            // PauseApprovedBy
            // 
            this.PauseApprovedBy.DataPropertyName = "PauseApprovedBy";
            this.PauseApprovedBy.HeaderText = "Pause Approved By";
            this.PauseApprovedBy.Name = "PauseApprovedBy";
            this.PauseApprovedBy.ReadOnly = true;
            // 
            // UnPausedBy
            // 
            this.UnPausedBy.DataPropertyName = "UnPausedBy";
            this.UnPausedBy.HeaderText = "Unpaused By";
            this.UnPausedBy.Name = "UnPausedBy";
            this.UnPausedBy.ReadOnly = true;
            // 
            // UnPauseApprovedBy
            // 
            this.UnPauseApprovedBy.DataPropertyName = "UnPauseApprovedBy";
            this.UnPauseApprovedBy.HeaderText = "Unpause Approved By";
            this.UnPauseApprovedBy.Name = "UnPauseApprovedBy";
            this.UnPauseApprovedBy.ReadOnly = true;
            // 
            // CancellationOption
            // 
            this.CancellationOption.DataPropertyName = "CancellationOption";
            this.CancellationOption.HeaderText = "Cancellation Option";
            this.CancellationOption.Name = "CancellationOption";
            this.CancellationOption.ReadOnly = true;
            // 
            // CancelledBy
            // 
            this.CancelledBy.DataPropertyName = "CancelledBy";
            this.CancelledBy.HeaderText = "Cancelled By";
            this.CancelledBy.Name = "CancelledBy";
            this.CancelledBy.ReadOnly = true;
            // 
            // CancellationApprovedBy
            // 
            this.CancellationApprovedBy.DataPropertyName = "CancellationApprovedBy";
            this.CancellationApprovedBy.HeaderText = "Cancellation Approved By";
            this.CancellationApprovedBy.Name = "CancellationApprovedBy";
            this.CancellationApprovedBy.ReadOnly = true;
            // 
            // billInAdvanceDataGridViewCheckBoxColumn
            // 
            this.billInAdvanceDataGridViewCheckBoxColumn.DataPropertyName = "BillInAdvance";
            this.billInAdvanceDataGridViewCheckBoxColumn.HeaderText = "Bill In Advance";
            this.billInAdvanceDataGridViewCheckBoxColumn.Name = "billInAdvanceDataGridViewCheckBoxColumn";
            this.billInAdvanceDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // noOfRenewalRemindersDataGridViewTextBoxColumn
            // 
            this.noOfRenewalRemindersDataGridViewTextBoxColumn.DataPropertyName = "NoOfRenewalReminders";
            this.noOfRenewalRemindersDataGridViewTextBoxColumn.HeaderText = "No Of Renewal Reminders";
            this.noOfRenewalRemindersDataGridViewTextBoxColumn.Name = "noOfRenewalRemindersDataGridViewTextBoxColumn";
            this.noOfRenewalRemindersDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // reminderFrequencyInDaysDataGridViewTextBoxColumn
            // 
            this.reminderFrequencyInDaysDataGridViewTextBoxColumn.DataPropertyName = "ReminderFrequencyInDays";
            this.reminderFrequencyInDaysDataGridViewTextBoxColumn.HeaderText = "Reminder Frequency";
            this.reminderFrequencyInDaysDataGridViewTextBoxColumn.Name = "reminderFrequencyInDaysDataGridViewTextBoxColumn";
            this.reminderFrequencyInDaysDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sendFirstReminderBeforeXDaysDataGridViewTextBoxColumn
            // 
            this.sendFirstReminderBeforeXDaysDataGridViewTextBoxColumn.DataPropertyName = "SendFirstReminderBeforeXDays";
            this.sendFirstReminderBeforeXDaysDataGridViewTextBoxColumn.HeaderText = "Send First Reminder Before X Days";
            this.sendFirstReminderBeforeXDaysDataGridViewTextBoxColumn.Name = "sendFirstReminderBeforeXDaysDataGridViewTextBoxColumn";
            this.sendFirstReminderBeforeXDaysDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastRenewalReminderSentOnDataGridViewTextBoxColumn
            // 
            this.lastRenewalReminderSentOnDataGridViewTextBoxColumn.DataPropertyName = "LastRenewalReminderSentOn";
            this.lastRenewalReminderSentOnDataGridViewTextBoxColumn.HeaderText = "Last Renewal Reminder Sent On";
            this.lastRenewalReminderSentOnDataGridViewTextBoxColumn.Name = "lastRenewalReminderSentOnDataGridViewTextBoxColumn";
            this.lastRenewalReminderSentOnDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // renewalReminderCountDataGridViewTextBoxColumn
            // 
            this.renewalReminderCountDataGridViewTextBoxColumn.DataPropertyName = "RenewalReminderCount";
            this.renewalReminderCountDataGridViewTextBoxColumn.HeaderText = "Renewal Reminder Count";
            this.renewalReminderCountDataGridViewTextBoxColumn.Name = "renewalReminderCountDataGridViewTextBoxColumn";
            this.renewalReminderCountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastPaymentRetryLimitReminderSentOnDataGridViewTextBoxColumn
            // 
            this.lastPaymentRetryLimitReminderSentOnDataGridViewTextBoxColumn.DataPropertyName = "LastPaymentRetryLimitReminderSentOn";
            this.lastPaymentRetryLimitReminderSentOnDataGridViewTextBoxColumn.HeaderText = "Last Payment Retry Limit Reminder";
            this.lastPaymentRetryLimitReminderSentOnDataGridViewTextBoxColumn.Name = "lastPaymentRetryLimitReminderSentOnDataGridViewTextBoxColumn";
            this.lastPaymentRetryLimitReminderSentOnDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // paymentRetryLimitReminderCountDataGridViewTextBoxColumn
            // 
            this.paymentRetryLimitReminderCountDataGridViewTextBoxColumn.DataPropertyName = "PaymentRetryLimitReminderCount";
            this.paymentRetryLimitReminderCountDataGridViewTextBoxColumn.HeaderText = "Payment Retry Limit Reminder Count";
            this.paymentRetryLimitReminderCountDataGridViewTextBoxColumn.Name = "paymentRetryLimitReminderCountDataGridViewTextBoxColumn";
            this.paymentRetryLimitReminderCountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // productsIdDataGridViewTextBoxColumn
            // 
            this.productsIdDataGridViewTextBoxColumn.DataPropertyName = "ProductsId";
            this.productsIdDataGridViewTextBoxColumn.HeaderText = "ProductsId";
            this.productsIdDataGridViewTextBoxColumn.Name = "productsIdDataGridViewTextBoxColumn";
            this.productsIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.productsIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // productSubscriptionIdDataGridViewTextBoxColumn
            // 
            this.productSubscriptionIdDataGridViewTextBoxColumn.DataPropertyName = "ProductSubscriptionId";
            this.productSubscriptionIdDataGridViewTextBoxColumn.HeaderText = "ProductSubscriptionId";
            this.productSubscriptionIdDataGridViewTextBoxColumn.Name = "productSubscriptionIdDataGridViewTextBoxColumn";
            this.productSubscriptionIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.productSubscriptionIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // SourceSubscriptionHeaderId
            // 
            this.SourceSubscriptionHeaderId.DataPropertyName = "SourceSubscriptionHeaderId";
            this.SourceSubscriptionHeaderId.HeaderText = "Source Subscription Header Id";
            this.SourceSubscriptionHeaderId.Name = "SourceSubscriptionHeaderId";
            this.SourceSubscriptionHeaderId.ReadOnly = true;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "Created By";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "Creation Date";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Last Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "Last Update Date";
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // siteIdDataGridViewTextBoxColumn
            // 
            this.siteIdDataGridViewTextBoxColumn.DataPropertyName = "SiteId";
            this.siteIdDataGridViewTextBoxColumn.HeaderText = "SiteId";
            this.siteIdDataGridViewTextBoxColumn.Name = "siteIdDataGridViewTextBoxColumn";
            this.siteIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.siteIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn
            // 
            this.synchStatusDataGridViewCheckBoxColumn.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.Name = "synchStatusDataGridViewCheckBoxColumn";
            this.synchStatusDataGridViewCheckBoxColumn.ReadOnly = true;
            this.synchStatusDataGridViewCheckBoxColumn.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn
            // 
            this.guidDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn.Name = "guidDataGridViewTextBoxColumn";
            this.guidDataGridViewTextBoxColumn.ReadOnly = true;
            this.guidDataGridViewTextBoxColumn.Visible = false;
            // 
            // isChangedDataGridViewCheckBoxColumn
            // 
            this.isChangedDataGridViewCheckBoxColumn.DataPropertyName = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.HeaderText = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.Name = "isChangedDataGridViewCheckBoxColumn";
            this.isChangedDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isChangedDataGridViewCheckBoxColumn.Visible = false;
            // 
            // frmSubscriptionHistoryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 627);
            this.Controls.Add(this.pnlPage);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSubscriptionHistoryView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Subscription History";
            this.Load += new System.EventHandler(this.frmSubscriptionHistoryView_Load);
            this.pnlPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillCycleHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subscriptionBillingScheduleHistoryDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubscriptionHeaderHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subscriptionHeaderHistoryDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSubscriptionHeaderHistory;
        private System.Windows.Forms.BindingSource subscriptionHeaderHistoryDTOBindingSource;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vScrollHeader;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView hScrollHeaderHistory;
        private System.Windows.Forms.DataGridView dgvBillCycleHistory;
        private System.Windows.Forms.BindingSource subscriptionBillingScheduleHistoryDTOBindingSource;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vScrollBillCycleHistory;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView hScrollBillCycleHistory;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbBillingCycles;
        private System.Windows.Forms.Label lblBillCycle;
        private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionBillingScheduleHistoryIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionBillingScheduleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineType;
        private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionHeaderIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvBillingCycleTransactionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionLineIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn billFromDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billToDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billOnDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overridedBillAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overrideReasonDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overrideByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overrideApprovedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentProcessingFailureCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.Panel pnlPage;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionHeaderHistoryId;
        private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionHeaderIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn productSubscriptionNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvHeaderTransactionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionLineIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvHeaderCustomerId;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvHeaderCustomerContactId;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvHeaderCustomerCreditCardsID;
        private System.Windows.Forms.DataGridViewTextBoxColumn productSubscriptionDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionCycleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitOfSubscriptionCycleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionCycleValidityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn taxInclusivePriceDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn selectedPaymentCollectionModeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionPaymentCollectionModeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn autoRenewDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn autoRenewalMarkupPercentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn allowPauseDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FreeTrialPeriodCycle;
        private System.Windows.Forms.DataGridViewTextBoxColumn RenewalGracePeriodCycle;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeasonStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PausedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn PauseApprovedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnPausedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnPauseApprovedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CancellationOption;
        private System.Windows.Forms.DataGridViewTextBoxColumn CancelledBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CancellationApprovedBy;
        private System.Windows.Forms.DataGridViewCheckBoxColumn billInAdvanceDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn noOfRenewalRemindersDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reminderFrequencyInDaysDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sendFirstReminderBeforeXDaysDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastRenewalReminderSentOnDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn renewalReminderCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastPaymentRetryLimitReminderSentOnDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentRetryLimitReminderCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productsIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productSubscriptionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SourceSubscriptionHeaderId;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedDataGridViewCheckBoxColumn;
    }
}