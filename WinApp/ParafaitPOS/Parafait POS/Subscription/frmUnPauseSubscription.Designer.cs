namespace Parafait_POS.Subscription
{
    partial class frmUnPauseSubscription
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUnPauseSubscription));
            this.dgvUnPauseBillingSchedules = new System.Windows.Forms.DataGridView();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isChangedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.subscriptionBillingScheduleId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subscriptionHeaderIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionLineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billFromDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billToDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billOnDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.overridedBillAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overrideReasonDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overrideByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overrideApprovedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cancelledByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cancellationApprovedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newBillFromDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resumeBilling = new System.Windows.Forms.DataGridViewImageColumn();
            this.cancelBillingSchedule = new System.Windows.Forms.DataGridViewImageColumn();
            this.postponeBillingSchedule = new System.Windows.Forms.DataGridViewImageColumn();
            this.subscriptionBillingScheduleDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.horizontalScrollBarView1 = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.verticalScrollBarView1 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.btnClear = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnPauseBillingSchedules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subscriptionBillingScheduleDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvUnPauseBillingSchedules
            // 
            this.dgvUnPauseBillingSchedules.AutoGenerateColumns = false;
            this.dgvUnPauseBillingSchedules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnPauseBillingSchedules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.isActiveDataGridViewCheckBoxColumn,
            this.synchStatusDataGridViewCheckBoxColumn,
            this.isChangedDataGridViewCheckBoxColumn,
            this.subscriptionBillingScheduleId,
            this.subscriptionHeaderIdDataGridViewTextBoxColumn,
            this.transactionLineIdDataGridViewTextBoxColumn,
            this.billFromDate,
            this.billToDate,
            this.billOnDateDataGridViewTextBoxColumn,
            this.billAmount,
            this.transactionId,
            this.overridedBillAmountDataGridViewTextBoxColumn,
            this.overrideReasonDataGridViewTextBoxColumn,
            this.overrideByDataGridViewTextBoxColumn,
            this.overrideApprovedByDataGridViewTextBoxColumn,
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn,
            this.status,
            this.cancelledByDataGridViewTextBoxColumn,
            this.cancellationApprovedByDataGridViewTextBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn,
            this.siteIdDataGridViewTextBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn,
            this.guidDataGridViewTextBoxColumn,
            this.newBillFromDate,
            this.resumeBilling,
            this.cancelBillingSchedule,
            this.postponeBillingSchedule});
            this.dgvUnPauseBillingSchedules.DataSource = this.subscriptionBillingScheduleDTOBindingSource;
            this.dgvUnPauseBillingSchedules.Location = new System.Drawing.Point(12, 12);
            this.dgvUnPauseBillingSchedules.Name = "dgvUnPauseBillingSchedules";
            this.dgvUnPauseBillingSchedules.ReadOnly = true;
            this.dgvUnPauseBillingSchedules.RowHeadersVisible = false;
            this.dgvUnPauseBillingSchedules.RowTemplate.Height = 30;
            this.dgvUnPauseBillingSchedules.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvUnPauseBillingSchedules.Size = new System.Drawing.Size(963, 405);
            this.dgvUnPauseBillingSchedules.TabIndex = 1;
            this.dgvUnPauseBillingSchedules.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnPauseBillingSchedules_CellClick);
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewCheckBoxColumn.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn
            // 
            this.synchStatusDataGridViewCheckBoxColumn.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.Name = "synchStatusDataGridViewCheckBoxColumn";
            this.synchStatusDataGridViewCheckBoxColumn.ReadOnly = true;
            this.synchStatusDataGridViewCheckBoxColumn.Visible = false;
            // 
            // isChangedDataGridViewCheckBoxColumn
            // 
            this.isChangedDataGridViewCheckBoxColumn.DataPropertyName = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.HeaderText = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.Name = "isChangedDataGridViewCheckBoxColumn";
            this.isChangedDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isChangedDataGridViewCheckBoxColumn.Visible = false;
            // 
            // subscriptionBillingScheduleId
            // 
            this.subscriptionBillingScheduleId.DataPropertyName = "SubscriptionBillingScheduleId";
            this.subscriptionBillingScheduleId.HeaderText = "SubscriptionBillingScheduleId";
            this.subscriptionBillingScheduleId.Name = "subscriptionBillingScheduleId";
            this.subscriptionBillingScheduleId.ReadOnly = true;
            this.subscriptionBillingScheduleId.Visible = false;
            // 
            // subscriptionHeaderIdDataGridViewTextBoxColumn
            // 
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.DataPropertyName = "SubscriptionHeaderId";
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.HeaderText = "SubscriptionHeaderId";
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.Name = "subscriptionHeaderIdDataGridViewTextBoxColumn";
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // transactionLineIdDataGridViewTextBoxColumn
            // 
            this.transactionLineIdDataGridViewTextBoxColumn.DataPropertyName = "TransactionLineId";
            this.transactionLineIdDataGridViewTextBoxColumn.HeaderText = "TransactionLineId";
            this.transactionLineIdDataGridViewTextBoxColumn.Name = "transactionLineIdDataGridViewTextBoxColumn";
            this.transactionLineIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionLineIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // billFromDate
            // 
            this.billFromDate.DataPropertyName = "BillFromDate";
            this.billFromDate.HeaderText = "Bill From Date";
            this.billFromDate.MinimumWidth = 130;
            this.billFromDate.Name = "billFromDate";
            this.billFromDate.ReadOnly = true;
            this.billFromDate.Width = 130;
            // 
            // billToDate
            // 
            this.billToDate.DataPropertyName = "BillToDate";
            this.billToDate.HeaderText = "Bill To Date";
            this.billToDate.MinimumWidth = 130;
            this.billToDate.Name = "billToDate";
            this.billToDate.ReadOnly = true;
            this.billToDate.Width = 130;
            // 
            // billOnDateDataGridViewTextBoxColumn
            // 
            this.billOnDateDataGridViewTextBoxColumn.DataPropertyName = "BillOnDate";
            this.billOnDateDataGridViewTextBoxColumn.HeaderText = "BillOnDate";
            this.billOnDateDataGridViewTextBoxColumn.Name = "billOnDateDataGridViewTextBoxColumn";
            this.billOnDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.billOnDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // billAmount
            // 
            this.billAmount.DataPropertyName = "BillAmount";
            this.billAmount.HeaderText = "Bill Amount";
            this.billAmount.Name = "billAmount";
            this.billAmount.ReadOnly = true;
            // 
            // transactionId
            // 
            this.transactionId.DataPropertyName = "TransactionId";
            this.transactionId.HeaderText = "Transaction No";
            this.transactionId.MinimumWidth = 130;
            this.transactionId.Name = "transactionId";
            this.transactionId.ReadOnly = true;
            this.transactionId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.transactionId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.transactionId.Width = 130;
            // 
            // overridedBillAmountDataGridViewTextBoxColumn
            // 
            this.overridedBillAmountDataGridViewTextBoxColumn.DataPropertyName = "OverridedBillAmount";
            this.overridedBillAmountDataGridViewTextBoxColumn.HeaderText = "OverridedBillAmount";
            this.overridedBillAmountDataGridViewTextBoxColumn.Name = "overridedBillAmountDataGridViewTextBoxColumn";
            this.overridedBillAmountDataGridViewTextBoxColumn.ReadOnly = true;
            this.overridedBillAmountDataGridViewTextBoxColumn.Visible = false;
            // 
            // overrideReasonDataGridViewTextBoxColumn
            // 
            this.overrideReasonDataGridViewTextBoxColumn.DataPropertyName = "OverrideReason";
            this.overrideReasonDataGridViewTextBoxColumn.HeaderText = "OverrideReason";
            this.overrideReasonDataGridViewTextBoxColumn.Name = "overrideReasonDataGridViewTextBoxColumn";
            this.overrideReasonDataGridViewTextBoxColumn.ReadOnly = true;
            this.overrideReasonDataGridViewTextBoxColumn.Visible = false;
            // 
            // overrideByDataGridViewTextBoxColumn
            // 
            this.overrideByDataGridViewTextBoxColumn.DataPropertyName = "OverrideBy";
            this.overrideByDataGridViewTextBoxColumn.HeaderText = "OverrideBy";
            this.overrideByDataGridViewTextBoxColumn.Name = "overrideByDataGridViewTextBoxColumn";
            this.overrideByDataGridViewTextBoxColumn.ReadOnly = true;
            this.overrideByDataGridViewTextBoxColumn.Visible = false;
            // 
            // overrideApprovedByDataGridViewTextBoxColumn
            // 
            this.overrideApprovedByDataGridViewTextBoxColumn.DataPropertyName = "OverrideApprovedBy";
            this.overrideApprovedByDataGridViewTextBoxColumn.HeaderText = "OverrideApprovedBy";
            this.overrideApprovedByDataGridViewTextBoxColumn.Name = "overrideApprovedByDataGridViewTextBoxColumn";
            this.overrideApprovedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.overrideApprovedByDataGridViewTextBoxColumn.Visible = false;
            // 
            // paymentProcessingFailureCountDataGridViewTextBoxColumn
            // 
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.DataPropertyName = "PaymentProcessingFailureCount";
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.HeaderText = "PaymentProcessingFailureCount";
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.Name = "paymentProcessingFailureCountDataGridViewTextBoxColumn";
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.ReadOnly = true;
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.Visible = false;
            // 
            // status
            // 
            this.status.DataPropertyName = "Status";
            this.status.HeaderText = "Status";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // cancelledByDataGridViewTextBoxColumn
            // 
            this.cancelledByDataGridViewTextBoxColumn.DataPropertyName = "CancelledBy";
            this.cancelledByDataGridViewTextBoxColumn.HeaderText = "CancelledBy";
            this.cancelledByDataGridViewTextBoxColumn.Name = "cancelledByDataGridViewTextBoxColumn";
            this.cancelledByDataGridViewTextBoxColumn.ReadOnly = true;
            this.cancelledByDataGridViewTextBoxColumn.Visible = false;
            // 
            // cancellationApprovedByDataGridViewTextBoxColumn
            // 
            this.cancellationApprovedByDataGridViewTextBoxColumn.DataPropertyName = "CancellationApprovedBy";
            this.cancellationApprovedByDataGridViewTextBoxColumn.HeaderText = "CancellationApprovedBy";
            this.cancellationApprovedByDataGridViewTextBoxColumn.Name = "cancellationApprovedByDataGridViewTextBoxColumn";
            this.cancellationApprovedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.cancellationApprovedByDataGridViewTextBoxColumn.Visible = false;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdByDataGridViewTextBoxColumn.Visible = false;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.creationDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdatedByDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdateDateDataGridViewTextBoxColumn.Visible = false;
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
            // guidDataGridViewTextBoxColumn
            // 
            this.guidDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn.Name = "guidDataGridViewTextBoxColumn";
            this.guidDataGridViewTextBoxColumn.ReadOnly = true;
            this.guidDataGridViewTextBoxColumn.Visible = false;
            // 
            // newBillFromDate
            // 
            this.newBillFromDate.HeaderText = "New Bill From Date";
            this.newBillFromDate.MinimumWidth = 160;
            this.newBillFromDate.Name = "newBillFromDate";
            this.newBillFromDate.ReadOnly = true;
            this.newBillFromDate.Width = 160;
            // 
            // resumeBilling
            // 
            this.resumeBilling.HeaderText = "Resume";
            this.resumeBilling.Image = global::Parafait_POS.Properties.Resources.ResumeBillingActive;
            this.resumeBilling.MinimumWidth = 70;
            this.resumeBilling.Name = "resumeBilling";
            this.resumeBilling.ReadOnly = true;
            this.resumeBilling.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.resumeBilling.ToolTipText = "Resume Billing";
            this.resumeBilling.Width = 70;
            // 
            // cancelBillingSchedule
            // 
            this.cancelBillingSchedule.HeaderText = "Cancel";
            this.cancelBillingSchedule.Image = global::Parafait_POS.Properties.Resources.CancelSubscriptionCycleGrayed;
            this.cancelBillingSchedule.MinimumWidth = 70;
            this.cancelBillingSchedule.Name = "cancelBillingSchedule";
            this.cancelBillingSchedule.ReadOnly = true;
            this.cancelBillingSchedule.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cancelBillingSchedule.ToolTipText = "Cancel Billing";
            this.cancelBillingSchedule.Width = 70;
            // 
            // postponeBillingSchedule
            // 
            this.postponeBillingSchedule.HeaderText = "Postpone";
            this.postponeBillingSchedule.Image = global::Parafait_POS.Properties.Resources.PostponeSubscriptionCycleGrayed;
            this.postponeBillingSchedule.MinimumWidth = 70;
            this.postponeBillingSchedule.Name = "postponeBillingSchedule";
            this.postponeBillingSchedule.ReadOnly = true;
            this.postponeBillingSchedule.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.postponeBillingSchedule.ToolTipText = "Postpone";
            this.postponeBillingSchedule.Width = 70;
            // 
            // subscriptionBillingScheduleDTOBindingSource
            // 
            this.subscriptionBillingScheduleDTOBindingSource.DataSource = typeof(Semnox.Parafait.Transaction.SubscriptionBillingScheduleDTO);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(627, 489);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 34);
            this.btnCancel.TabIndex = 126;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.BackColor = System.Drawing.Color.Transparent;
            this.btnOkay.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnOkay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOkay.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnOkay.FlatAppearance.BorderSize = 0;
            this.btnOkay.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnOkay.ForeColor = System.Drawing.Color.White;
            this.btnOkay.Location = new System.Drawing.Point(315, 488);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(104, 36);
            this.btnOkay.TabIndex = 125;
            this.btnOkay.Text = "Ok";
            this.btnOkay.UseVisualStyleBackColor = false;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // horizontalScrollBarView1
            // 
            this.horizontalScrollBarView1.AutoHide = false;
            this.horizontalScrollBarView1.DataGridView = null;
            this.horizontalScrollBarView1.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.LeftButtonBackgroundImage")));
            this.horizontalScrollBarView1.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.LeftButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView1.Location = new System.Drawing.Point(13, 420);
            this.horizontalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarView1.Name = "horizontalScrollBarView1";
            this.horizontalScrollBarView1.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.RightButtonBackgroundImage")));
            this.horizontalScrollBarView1.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.RightButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView1.ScrollableControl = null;
            this.horizontalScrollBarView1.ScrollViewer = null;
            this.horizontalScrollBarView1.Size = new System.Drawing.Size(962, 40);
            this.horizontalScrollBarView1.TabIndex = 3;
            // 
            // verticalScrollBarView1
            // 
            this.verticalScrollBarView1.AutoHide = false;
            this.verticalScrollBarView1.DataGridView = null;
            this.verticalScrollBarView1.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.DownButtonBackgroundImage")));
            this.verticalScrollBarView1.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarView1.Location = new System.Drawing.Point(976, 12);
            this.verticalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView1.Name = "verticalScrollBarView1";
            this.verticalScrollBarView1.ScrollableControl = null;
            this.verticalScrollBarView1.ScrollViewer = null;
            this.verticalScrollBarView1.Size = new System.Drawing.Size(40, 405);
            this.verticalScrollBarView1.TabIndex = 2;
            this.verticalScrollBarView1.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.UpButtonBackgroundImage")));
            this.verticalScrollBarView1.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.UpButtonDisabledBackgroundImage")));
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClear.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(465, 489);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(116, 34);
            this.btnClear.TabIndex = 127;
            this.btnClear.Text = "Clear Actions";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Postpone To";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 160;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 160;
            // 
            // frmUnPauseSubscription
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 535);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.horizontalScrollBarView1);
            this.Controls.Add(this.verticalScrollBarView1);
            this.Controls.Add(this.dgvUnPauseBillingSchedules);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUnPauseSubscription";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Un Pause Subscription";
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnPauseBillingSchedules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subscriptionBillingScheduleDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvUnPauseBillingSchedules;
        private System.Windows.Forms.BindingSource subscriptionBillingScheduleDTOBindingSource;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView1;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarView1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionBillingScheduleId;
        private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionHeaderIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionLineIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billFromDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn billToDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn billOnDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billAmount;
        private System.Windows.Forms.DataGridViewComboBoxColumn transactionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn overridedBillAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overrideReasonDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overrideByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overrideApprovedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentProcessingFailureCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn cancelledByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cancellationApprovedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn newBillFromDate;
        private System.Windows.Forms.DataGridViewImageColumn resumeBilling;
        private System.Windows.Forms.DataGridViewImageColumn cancelBillingSchedule;
        private System.Windows.Forms.DataGridViewImageColumn postponeBillingSchedule;
    }
}