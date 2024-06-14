namespace Parafait_POS.Subscription
{
    partial class frmSubscriptionDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSubscriptionDetails));
            this.pnlPage = new System.Windows.Forms.Panel();
            this.fpnlHeaderActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancelSubscription = new System.Windows.Forms.Button();
            this.btnSendManualRenewalReminder = new System.Windows.Forms.Button();
            this.btnViewHistory = new System.Windows.Forms.Button();
            this.btnPauseSubscription = new System.Windows.Forms.Button();
            this.btnReactivateSubscription = new System.Windows.Forms.Button();
            this.grpBillingCycle = new System.Windows.Forms.GroupBox();
            this.fpnlBillingCycleActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOverridePrice = new System.Windows.Forms.Button();
            this.btnResetPaymentErrorCount = new System.Windows.Forms.Button();
            this.btnPrintReceipt = new System.Windows.Forms.Button();
            this.btnEmailReceipt = new System.Windows.Forms.Button();
            this.hScrollBillingCycle = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.dgvSubscriptionBillingSchedule = new System.Windows.Forms.DataGridView();
            this.dgvColumnbillCycleActions = new System.Windows.Forms.DataGridViewButtonColumn();
            this.subscriptionBillingScheduleId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subscriptionHeaderIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionLineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billFromDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billToDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billOnDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColumnTransactionId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overridedBillAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overrideByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overrideApprovedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CancelledBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CancellationApprovedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overrideReasonDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.guidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isChangedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.subscriptionBillingScheduleDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vScrollBillingCycle = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.grpHeader = new System.Windows.Forms.GroupBox();
            this.fpnlHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlMainHeader = new System.Windows.Forms.Panel();
            this.btnExpandCollapse1 = new System.Windows.Forms.Button();
            this.lblHeaderName = new System.Windows.Forms.Label();
            this.pnlMainHeaderBody = new System.Windows.Forms.Panel();
            this.txtSubscriptionEndDate = new System.Windows.Forms.TextBox();
            this.lblSubscriptionEndDate = new System.Windows.Forms.Label();
            this.txtSubscriptionStartDate = new System.Windows.Forms.TextBox();
            this.lblSubscriptionStartDate = new System.Windows.Forms.Label();
            this.btnDetails = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbSubscriptionStatus = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbCreditCardId = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblCreditCard = new System.Windows.Forms.Label();
            this.txtSubscriptionDescription = new System.Windows.Forms.TextBox();
            this.lblSubscriptionDescription = new System.Windows.Forms.Label();
            this.txtSubscriptionNo = new System.Windows.Forms.TextBox();
            this.lblSubscriptionNo = new System.Windows.Forms.Label();
            this.cmbCustomerContact = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblCustomerContact = new System.Windows.Forms.Label();
            this.txtSubscriptionCycleValidity = new System.Windows.Forms.TextBox();
            this.lblSubscriptionCycleValidity = new System.Windows.Forms.Label();
            this.txtUnitOfSubscriptionCycle = new System.Windows.Forms.TextBox();
            this.lblUnitOfSubscriptionCycle = new System.Windows.Forms.Label();
            this.txtSubscriptionPrice = new System.Windows.Forms.TextBox();
            this.lblSubscriptionPrice = new System.Windows.Forms.Label();
            this.txtSubscriptionCycle = new System.Windows.Forms.TextBox();
            this.lblSubscriptionCycle = new System.Windows.Forms.Label();
            this.pnlPaymentSeason = new System.Windows.Forms.Panel();
            this.btnExpandCollapse2 = new System.Windows.Forms.Button();
            this.lblPaymentSeasonHeader = new System.Windows.Forms.Label();
            this.pnlPaymentAndSeason = new System.Windows.Forms.Panel();
            this.txtUnpausedBy = new System.Windows.Forms.TextBox();
            this.lblUnpausedBy = new System.Windows.Forms.Label();
            this.txtPausedBy = new System.Windows.Forms.TextBox();
            this.lblPausedBy = new System.Windows.Forms.Label();
            this.txtCancellationApprovedBy = new System.Windows.Forms.TextBox();
            this.lblCancellationApprovedBy = new System.Windows.Forms.Label();
            this.txtCancelledBy = new System.Windows.Forms.TextBox();
            this.lblCancelledBy = new System.Windows.Forms.Label();
            this.txtUnpauseApprovedBy = new System.Windows.Forms.TextBox();
            this.lblUnpauseApprovedBy = new System.Windows.Forms.Label();
            this.txtPauseApprovedBy = new System.Windows.Forms.TextBox();
            this.lblPauseApprovedBy = new System.Windows.Forms.Label();
            this.cmbCancellationOption = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblCancellationOption = new System.Windows.Forms.Label();
            this.cbxBillInAdvance = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.lblBillInAdvance = new System.Windows.Forms.Label();
            this.cbxAllowPause = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.lblAllowPause = new System.Windows.Forms.Label();
            this.cmbSelectedPaymentCollectionMode = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblPaymentColletionMode = new System.Windows.Forms.Label();
            this.txtSourceSubscriptionHeaderId = new System.Windows.Forms.TextBox();
            this.lblSourceSubscriptionHeaderId = new System.Windows.Forms.Label();
            this.txtSeasonStartDate = new System.Windows.Forms.TextBox();
            this.lblSeasonStartDate = new System.Windows.Forms.Label();
            this.pnlRenewalHeader = new System.Windows.Forms.Panel();
            this.btnExpandCollapse3 = new System.Windows.Forms.Button();
            this.lblRenewalHeader = new System.Windows.Forms.Label();
            this.pnlRenewalAndReminders = new System.Windows.Forms.Panel();
            this.txtAutoRenewalMarkup = new System.Windows.Forms.TextBox();
            this.lblAutoRenewalMarkup = new System.Windows.Forms.Label();
            this.cbxAutoRenew = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.lblAutoRenew = new System.Windows.Forms.Label();
            this.txtFirstReminderBeforeXDays = new System.Windows.Forms.TextBox();
            this.lblFirstReminderBeforeXDays = new System.Windows.Forms.Label();
            this.txtRenewalReminderFrequency = new System.Windows.Forms.TextBox();
            this.lblRenewalReminderFrequency1 = new System.Windows.Forms.Label();
            this.txtNextReminderOn = new System.Windows.Forms.TextBox();
            this.lblNextReminderOn = new System.Windows.Forms.Label();
            this.txtLastReminderSentOn = new System.Windows.Forms.TextBox();
            this.lblLastReminderSentOn = new System.Windows.Forms.Label();
            this.txtRenewalReminderSent = new System.Windows.Forms.TextBox();
            this.lblRenewalReminders = new System.Windows.Forms.Label();
            this.txtFreeTrialPeriod = new System.Windows.Forms.TextBox();
            this.lblFreeTrialPeriod = new System.Windows.Forms.Label();
            this.txtRenewalGracePeriod = new System.Windows.Forms.TextBox();
            this.lblRenewalGracePeriod = new System.Windows.Forms.Label();
            this.pnlPage.SuspendLayout();
            this.fpnlHeaderActions.SuspendLayout();
            this.grpBillingCycle.SuspendLayout();
            this.fpnlBillingCycleActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubscriptionBillingSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subscriptionBillingScheduleDTOBindingSource)).BeginInit();
            this.grpHeader.SuspendLayout();
            this.fpnlHeader.SuspendLayout();
            this.pnlMainHeader.SuspendLayout();
            this.pnlMainHeaderBody.SuspendLayout();
            this.pnlPaymentSeason.SuspendLayout();
            this.pnlPaymentAndSeason.SuspendLayout();
            this.pnlRenewalHeader.SuspendLayout();
            this.pnlRenewalAndReminders.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPage
            // 
            this.pnlPage.BackColor = System.Drawing.Color.Transparent;
            this.pnlPage.Controls.Add(this.fpnlHeaderActions);
            this.pnlPage.Controls.Add(this.grpBillingCycle);
            this.pnlPage.Controls.Add(this.grpHeader);
            this.pnlPage.Location = new System.Drawing.Point(0, 0);
            this.pnlPage.Name = "pnlPage";
            this.pnlPage.Size = new System.Drawing.Size(1242, 625);
            this.pnlPage.TabIndex = 0;
            // 
            // fpnlHeaderActions
            // 
            this.fpnlHeaderActions.AutoSize = true;
            this.fpnlHeaderActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fpnlHeaderActions.BackColor = System.Drawing.Color.White;
            this.fpnlHeaderActions.BackgroundImage = global::Parafait_POS.Properties.Resources.whiteBackground;
            this.fpnlHeaderActions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.fpnlHeaderActions.Controls.Add(this.btnCancelSubscription);
            this.fpnlHeaderActions.Controls.Add(this.btnSendManualRenewalReminder);
            this.fpnlHeaderActions.Controls.Add(this.btnViewHistory);
            this.fpnlHeaderActions.Controls.Add(this.btnPauseSubscription);
            this.fpnlHeaderActions.Controls.Add(this.btnReactivateSubscription);
            this.fpnlHeaderActions.Location = new System.Drawing.Point(2, 387);
            this.fpnlHeaderActions.MaximumSize = new System.Drawing.Size(142, 240);
            this.fpnlHeaderActions.MinimumSize = new System.Drawing.Size(142, 131);
            this.fpnlHeaderActions.Name = "fpnlHeaderActions";
            this.fpnlHeaderActions.Padding = new System.Windows.Forms.Padding(3);
            this.fpnlHeaderActions.Size = new System.Drawing.Size(142, 236);
            this.fpnlHeaderActions.TabIndex = 52;
            this.fpnlHeaderActions.Leave += new System.EventHandler(this.pnlHeaderActions_Leave);
            // 
            // btnCancelSubscription
            // 
            this.btnCancelSubscription.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelSubscription.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancelSubscription.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelSubscription.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCancelSubscription.FlatAppearance.BorderSize = 0;
            this.btnCancelSubscription.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancelSubscription.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancelSubscription.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancelSubscription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelSubscription.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancelSubscription.ForeColor = System.Drawing.Color.White;
            this.btnCancelSubscription.Location = new System.Drawing.Point(6, 6);
            this.btnCancelSubscription.Name = "btnCancelSubscription";
            this.btnCancelSubscription.Size = new System.Drawing.Size(130, 40);
            this.btnCancelSubscription.TabIndex = 47;
            this.btnCancelSubscription.Text = "Cancel Subscription";
            this.btnCancelSubscription.UseVisualStyleBackColor = false;
            this.btnCancelSubscription.Click += new System.EventHandler(this.btnCancelSubscription_Click);
            this.btnCancelSubscription.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnCancelSubscription.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // btnSendManualRenewalReminder
            // 
            this.btnSendManualRenewalReminder.BackColor = System.Drawing.Color.Transparent;
            this.btnSendManualRenewalReminder.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSendManualRenewalReminder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSendManualRenewalReminder.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnSendManualRenewalReminder.FlatAppearance.BorderSize = 0;
            this.btnSendManualRenewalReminder.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSendManualRenewalReminder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSendManualRenewalReminder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSendManualRenewalReminder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendManualRenewalReminder.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSendManualRenewalReminder.ForeColor = System.Drawing.Color.White;
            this.btnSendManualRenewalReminder.Location = new System.Drawing.Point(6, 52);
            this.btnSendManualRenewalReminder.Name = "btnSendManualRenewalReminder";
            this.btnSendManualRenewalReminder.Size = new System.Drawing.Size(130, 40);
            this.btnSendManualRenewalReminder.TabIndex = 48;
            this.btnSendManualRenewalReminder.Text = "Send Renewal Reminder";
            this.btnSendManualRenewalReminder.UseVisualStyleBackColor = false;
            this.btnSendManualRenewalReminder.Click += new System.EventHandler(this.btnSendManualRenewalReminder_Click);
            this.btnSendManualRenewalReminder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnSendManualRenewalReminder.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // btnViewHistory
            // 
            this.btnViewHistory.BackColor = System.Drawing.Color.Transparent;
            this.btnViewHistory.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnViewHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnViewHistory.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnViewHistory.FlatAppearance.BorderSize = 0;
            this.btnViewHistory.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnViewHistory.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnViewHistory.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnViewHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewHistory.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnViewHistory.ForeColor = System.Drawing.Color.White;
            this.btnViewHistory.Location = new System.Drawing.Point(6, 98);
            this.btnViewHistory.Name = "btnViewHistory";
            this.btnViewHistory.Size = new System.Drawing.Size(130, 40);
            this.btnViewHistory.TabIndex = 49;
            this.btnViewHistory.Text = "View History";
            this.btnViewHistory.UseVisualStyleBackColor = false;
            this.btnViewHistory.Click += new System.EventHandler(this.btnViewHistory_Click);
            this.btnViewHistory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnViewHistory.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // btnPauseSubscription
            // 
            this.btnPauseSubscription.BackColor = System.Drawing.Color.Transparent;
            this.btnPauseSubscription.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPauseSubscription.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPauseSubscription.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPauseSubscription.FlatAppearance.BorderSize = 0;
            this.btnPauseSubscription.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPauseSubscription.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPauseSubscription.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPauseSubscription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPauseSubscription.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPauseSubscription.ForeColor = System.Drawing.Color.White;
            this.btnPauseSubscription.Location = new System.Drawing.Point(6, 144);
            this.btnPauseSubscription.Name = "btnPauseSubscription";
            this.btnPauseSubscription.Size = new System.Drawing.Size(130, 40);
            this.btnPauseSubscription.TabIndex = 50;
            this.btnPauseSubscription.Text = "Pause Subscription";
            this.btnPauseSubscription.UseVisualStyleBackColor = false;
            this.btnPauseSubscription.Click += new System.EventHandler(this.btnPauseSubscription_Click);
            this.btnPauseSubscription.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnPauseSubscription.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // btnReactivateSubscription
            // 
            this.btnReactivateSubscription.BackColor = System.Drawing.Color.Transparent;
            this.btnReactivateSubscription.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnReactivateSubscription.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReactivateSubscription.Enabled = false;
            this.btnReactivateSubscription.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnReactivateSubscription.FlatAppearance.BorderSize = 0;
            this.btnReactivateSubscription.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnReactivateSubscription.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnReactivateSubscription.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnReactivateSubscription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReactivateSubscription.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnReactivateSubscription.ForeColor = System.Drawing.Color.White;
            this.btnReactivateSubscription.Location = new System.Drawing.Point(6, 190);
            this.btnReactivateSubscription.Name = "btnReactivateSubscription";
            this.btnReactivateSubscription.Size = new System.Drawing.Size(130, 40);
            this.btnReactivateSubscription.TabIndex = 51;
            this.btnReactivateSubscription.Text = "Reactivate Subscription";
            this.btnReactivateSubscription.UseVisualStyleBackColor = false;
            this.btnReactivateSubscription.Visible = false;
            this.btnReactivateSubscription.Click += new System.EventHandler(this.btnReactivateSubscription_Click);
            this.btnReactivateSubscription.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnReactivateSubscription.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // grpBillingCycle
            // 
            this.grpBillingCycle.Controls.Add(this.fpnlBillingCycleActions);
            this.grpBillingCycle.Controls.Add(this.hScrollBillingCycle);
            this.grpBillingCycle.Controls.Add(this.vScrollBillingCycle);
            this.grpBillingCycle.Controls.Add(this.dgvSubscriptionBillingSchedule);
            this.grpBillingCycle.Location = new System.Drawing.Point(2, 326);
            this.grpBillingCycle.Name = "grpBillingCycle";
            this.grpBillingCycle.Size = new System.Drawing.Size(1236, 299);
            this.grpBillingCycle.TabIndex = 51;
            this.grpBillingCycle.TabStop = false;
            this.grpBillingCycle.Text = "Billing Cycles";
            // 
            // fpnlBillingCycleActions
            // 
            this.fpnlBillingCycleActions.AutoSize = true;
            this.fpnlBillingCycleActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fpnlBillingCycleActions.BackColor = System.Drawing.Color.White;
            this.fpnlBillingCycleActions.BackgroundImage = global::Parafait_POS.Properties.Resources.whiteBackground;
            this.fpnlBillingCycleActions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.fpnlBillingCycleActions.Controls.Add(this.btnOverridePrice);
            this.fpnlBillingCycleActions.Controls.Add(this.btnResetPaymentErrorCount);
            this.fpnlBillingCycleActions.Controls.Add(this.btnPrintReceipt);
            this.fpnlBillingCycleActions.Controls.Add(this.btnEmailReceipt);
            this.fpnlBillingCycleActions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.fpnlBillingCycleActions.Location = new System.Drawing.Point(189, 94);
            this.fpnlBillingCycleActions.MaximumSize = new System.Drawing.Size(142, 193);
            this.fpnlBillingCycleActions.MinimumSize = new System.Drawing.Size(142, 63);
            this.fpnlBillingCycleActions.Name = "fpnlBillingCycleActions";
            this.fpnlBillingCycleActions.Padding = new System.Windows.Forms.Padding(3);
            this.fpnlBillingCycleActions.Size = new System.Drawing.Size(142, 190);
            this.fpnlBillingCycleActions.TabIndex = 53;
            this.fpnlBillingCycleActions.Leave += new System.EventHandler(this.fpnlBillingCycleActions_Leave);
            // 
            // btnOverridePrice
            // 
            this.btnOverridePrice.BackColor = System.Drawing.Color.Transparent;
            this.btnOverridePrice.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnOverridePrice.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOverridePrice.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnOverridePrice.FlatAppearance.BorderSize = 0;
            this.btnOverridePrice.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOverridePrice.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOverridePrice.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOverridePrice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOverridePrice.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnOverridePrice.ForeColor = System.Drawing.Color.White;
            this.btnOverridePrice.Location = new System.Drawing.Point(6, 6);
            this.btnOverridePrice.Name = "btnOverridePrice";
            this.btnOverridePrice.Size = new System.Drawing.Size(130, 40);
            this.btnOverridePrice.TabIndex = 47;
            this.btnOverridePrice.Text = "Override";
            this.btnOverridePrice.UseVisualStyleBackColor = false;
            this.btnOverridePrice.Click += new System.EventHandler(this.btnOverridePrice_Click);
            this.btnOverridePrice.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnOverridePrice.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // btnResetPaymentErrorCount
            // 
            this.btnResetPaymentErrorCount.BackColor = System.Drawing.Color.Transparent;
            this.btnResetPaymentErrorCount.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnResetPaymentErrorCount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnResetPaymentErrorCount.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnResetPaymentErrorCount.FlatAppearance.BorderSize = 0;
            this.btnResetPaymentErrorCount.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnResetPaymentErrorCount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnResetPaymentErrorCount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnResetPaymentErrorCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetPaymentErrorCount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnResetPaymentErrorCount.ForeColor = System.Drawing.Color.White;
            this.btnResetPaymentErrorCount.Location = new System.Drawing.Point(6, 52);
            this.btnResetPaymentErrorCount.Name = "btnResetPaymentErrorCount";
            this.btnResetPaymentErrorCount.Size = new System.Drawing.Size(130, 40);
            this.btnResetPaymentErrorCount.TabIndex = 48;
            this.btnResetPaymentErrorCount.Text = "Reset Payment Failure Count";
            this.btnResetPaymentErrorCount.UseVisualStyleBackColor = false;
            this.btnResetPaymentErrorCount.Click += new System.EventHandler(this.btnResetPaymentErrorCount_Click);
            this.btnResetPaymentErrorCount.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnResetPaymentErrorCount.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // btnPrintReceipt
            // 
            this.btnPrintReceipt.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintReceipt.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrintReceipt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrintReceipt.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrintReceipt.FlatAppearance.BorderSize = 0;
            this.btnPrintReceipt.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrintReceipt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintReceipt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintReceipt.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrintReceipt.ForeColor = System.Drawing.Color.White;
            this.btnPrintReceipt.Location = new System.Drawing.Point(6, 98);
            this.btnPrintReceipt.Name = "btnPrintReceipt";
            this.btnPrintReceipt.Size = new System.Drawing.Size(130, 40);
            this.btnPrintReceipt.TabIndex = 49;
            this.btnPrintReceipt.Text = "Print Receipt";
            this.btnPrintReceipt.UseVisualStyleBackColor = false;
            this.btnPrintReceipt.Click += new System.EventHandler(this.btnPrintReceipt_Click);
            this.btnPrintReceipt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnPrintReceipt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // btnEmailReceipt
            // 
            this.btnEmailReceipt.BackColor = System.Drawing.Color.Transparent;
            this.btnEmailReceipt.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnEmailReceipt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEmailReceipt.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnEmailReceipt.FlatAppearance.BorderSize = 0;
            this.btnEmailReceipt.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnEmailReceipt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEmailReceipt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEmailReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmailReceipt.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnEmailReceipt.ForeColor = System.Drawing.Color.White;
            this.btnEmailReceipt.Location = new System.Drawing.Point(6, 144);
            this.btnEmailReceipt.Name = "btnEmailReceipt";
            this.btnEmailReceipt.Size = new System.Drawing.Size(130, 40);
            this.btnEmailReceipt.TabIndex = 50;
            this.btnEmailReceipt.Text = "Email Receipt";
            this.btnEmailReceipt.UseVisualStyleBackColor = false;
            this.btnEmailReceipt.Click += new System.EventHandler(this.btnEmailReceipt_Click);
            this.btnEmailReceipt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnEmailReceipt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // hScrollBillingCycle
            // 
            this.hScrollBillingCycle.AutoHide = false;
            this.hScrollBillingCycle.DataGridView = this.dgvSubscriptionBillingSchedule;
            this.hScrollBillingCycle.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollBillingCycle.LeftButtonBackgroundImage")));
            this.hScrollBillingCycle.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollBillingCycle.LeftButtonDisabledBackgroundImage")));
            this.hScrollBillingCycle.Location = new System.Drawing.Point(6, 250);
            this.hScrollBillingCycle.Margin = new System.Windows.Forms.Padding(0);
            this.hScrollBillingCycle.Name = "hScrollBillingCycle";
            this.hScrollBillingCycle.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollBillingCycle.RightButtonBackgroundImage")));
            this.hScrollBillingCycle.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollBillingCycle.RightButtonDisabledBackgroundImage")));
            this.hScrollBillingCycle.ScrollableControl = null;
            this.hScrollBillingCycle.ScrollViewer = null;
            this.hScrollBillingCycle.Size = new System.Drawing.Size(1183, 40);
            this.hScrollBillingCycle.TabIndex = 2;
            // 
            // dgvSubscriptionBillingSchedule
            // 
            this.dgvSubscriptionBillingSchedule.AutoGenerateColumns = false;
            this.dgvSubscriptionBillingSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubscriptionBillingSchedule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvColumnbillCycleActions,
            this.subscriptionBillingScheduleId,
            this.LineType,
            this.subscriptionHeaderIdDataGridViewTextBoxColumn,
            this.transactionLineIdDataGridViewTextBoxColumn,
            this.billFromDateDataGridViewTextBoxColumn,
            this.billToDateDataGridViewTextBoxColumn,
            this.billOnDateDataGridViewTextBoxColumn,
            this.billAmountDataGridViewTextBoxColumn,
            this.dgvColumnTransactionId,
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn,
            this.overridedBillAmountDataGridViewTextBoxColumn,
            this.overrideByDataGridViewTextBoxColumn,
            this.overrideApprovedByDataGridViewTextBoxColumn,
            this.CancelledBy,
            this.CancellationApprovedBy,
            this.overrideReasonDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn,
            this.siteIdDataGridViewTextBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn,
            this.synchStatusDataGridViewCheckBoxColumn,
            this.guidDataGridViewTextBoxColumn,
            this.isChangedDataGridViewCheckBoxColumn});
            this.dgvSubscriptionBillingSchedule.DataSource = this.subscriptionBillingScheduleDTOBindingSource;
            this.dgvSubscriptionBillingSchedule.Location = new System.Drawing.Point(7, 21);
            this.dgvSubscriptionBillingSchedule.Name = "dgvSubscriptionBillingSchedule";
            this.dgvSubscriptionBillingSchedule.RowHeadersVisible = false;
            this.dgvSubscriptionBillingSchedule.RowTemplate.Height = 30;
            this.dgvSubscriptionBillingSchedule.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvSubscriptionBillingSchedule.Size = new System.Drawing.Size(1182, 229);
            this.dgvSubscriptionBillingSchedule.TabIndex = 0;
            this.dgvSubscriptionBillingSchedule.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSubscriptionBillingSchedule_CellClick);
            this.dgvSubscriptionBillingSchedule.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSubscriptionBillingSchedule_CellFormatting);
            // 
            // dgvColumnbillCycleActions
            // 
            this.dgvColumnbillCycleActions.HeaderText = "...";
            this.dgvColumnbillCycleActions.MinimumWidth = 60;
            this.dgvColumnbillCycleActions.Name = "dgvColumnbillCycleActions";
            this.dgvColumnbillCycleActions.ReadOnly = true;
            this.dgvColumnbillCycleActions.Text = "...";
            this.dgvColumnbillCycleActions.ToolTipText = "Actions";
            this.dgvColumnbillCycleActions.UseColumnTextForButtonValue = true;
            this.dgvColumnbillCycleActions.Width = 60;
            // 
            // subscriptionBillingScheduleId
            // 
            this.subscriptionBillingScheduleId.DataPropertyName = "SubscriptionBillingScheduleId";
            this.subscriptionBillingScheduleId.HeaderText = "Id";
            this.subscriptionBillingScheduleId.Name = "subscriptionBillingScheduleId";
            this.subscriptionBillingScheduleId.ReadOnly = true;
            // 
            // LineType
            // 
            this.LineType.DataPropertyName = "LineType";
            this.LineType.HeaderText = "Line Type";
            this.LineType.Name = "LineType";
            this.LineType.ReadOnly = true;
            // 
            // subscriptionHeaderIdDataGridViewTextBoxColumn
            // 
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.DataPropertyName = "SubscriptionHeaderId";
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.HeaderText = "SubscriptionHeaderId";
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.Name = "subscriptionHeaderIdDataGridViewTextBoxColumn";
            this.subscriptionHeaderIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // transactionLineIdDataGridViewTextBoxColumn
            // 
            this.transactionLineIdDataGridViewTextBoxColumn.DataPropertyName = "TransactionLineId";
            this.transactionLineIdDataGridViewTextBoxColumn.HeaderText = "TransactionLineId";
            this.transactionLineIdDataGridViewTextBoxColumn.Name = "transactionLineIdDataGridViewTextBoxColumn";
            this.transactionLineIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // billFromDateDataGridViewTextBoxColumn
            // 
            this.billFromDateDataGridViewTextBoxColumn.DataPropertyName = "BillFromDate";
            this.billFromDateDataGridViewTextBoxColumn.HeaderText = "Bill From Date";
            this.billFromDateDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.billFromDateDataGridViewTextBoxColumn.Name = "billFromDateDataGridViewTextBoxColumn";
            this.billFromDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.billFromDateDataGridViewTextBoxColumn.Width = 120;
            // 
            // billToDateDataGridViewTextBoxColumn
            // 
            this.billToDateDataGridViewTextBoxColumn.DataPropertyName = "BillToDate";
            this.billToDateDataGridViewTextBoxColumn.HeaderText = "Bill To Date";
            this.billToDateDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.billToDateDataGridViewTextBoxColumn.Name = "billToDateDataGridViewTextBoxColumn";
            this.billToDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.billToDateDataGridViewTextBoxColumn.Width = 120;
            // 
            // billOnDateDataGridViewTextBoxColumn
            // 
            this.billOnDateDataGridViewTextBoxColumn.DataPropertyName = "BillOnDate";
            this.billOnDateDataGridViewTextBoxColumn.HeaderText = "Bill On Date";
            this.billOnDateDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.billOnDateDataGridViewTextBoxColumn.Name = "billOnDateDataGridViewTextBoxColumn";
            this.billOnDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.billOnDateDataGridViewTextBoxColumn.Width = 120;
            // 
            // billAmountDataGridViewTextBoxColumn
            // 
            this.billAmountDataGridViewTextBoxColumn.DataPropertyName = "BillAmount";
            this.billAmountDataGridViewTextBoxColumn.HeaderText = "Bill Amount";
            this.billAmountDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.billAmountDataGridViewTextBoxColumn.Name = "billAmountDataGridViewTextBoxColumn";
            this.billAmountDataGridViewTextBoxColumn.ReadOnly = true;
            this.billAmountDataGridViewTextBoxColumn.Width = 120;
            // 
            // dgvColumnTransactionId
            // 
            this.dgvColumnTransactionId.DataPropertyName = "TransactionId";
            this.dgvColumnTransactionId.HeaderText = "Transaction No";
            this.dgvColumnTransactionId.MinimumWidth = 120;
            this.dgvColumnTransactionId.Name = "dgvColumnTransactionId";
            this.dgvColumnTransactionId.ReadOnly = true;
            this.dgvColumnTransactionId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvColumnTransactionId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvColumnTransactionId.Width = 120;
            // 
            // paymentProcessingFailureCountDataGridViewTextBoxColumn
            // 
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.DataPropertyName = "PaymentProcessingFailureCount";
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.HeaderText = "Payment Failure Count";
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.Name = "paymentProcessingFailureCountDataGridViewTextBoxColumn";
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.ReadOnly = true;
            this.paymentProcessingFailureCountDataGridViewTextBoxColumn.Width = 120;
            // 
            // overridedBillAmountDataGridViewTextBoxColumn
            // 
            this.overridedBillAmountDataGridViewTextBoxColumn.DataPropertyName = "OverridedBillAmount";
            this.overridedBillAmountDataGridViewTextBoxColumn.HeaderText = "Overrided Bill Amount";
            this.overridedBillAmountDataGridViewTextBoxColumn.Name = "overridedBillAmountDataGridViewTextBoxColumn";
            this.overridedBillAmountDataGridViewTextBoxColumn.ReadOnly = true;
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
            // overrideReasonDataGridViewTextBoxColumn
            // 
            this.overrideReasonDataGridViewTextBoxColumn.DataPropertyName = "OverrideReason";
            this.overrideReasonDataGridViewTextBoxColumn.HeaderText = "Override Reason";
            this.overrideReasonDataGridViewTextBoxColumn.Name = "overrideReasonDataGridViewTextBoxColumn";
            this.overrideReasonDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.Visible = false;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // siteIdDataGridViewTextBoxColumn
            // 
            this.siteIdDataGridViewTextBoxColumn.DataPropertyName = "SiteId";
            this.siteIdDataGridViewTextBoxColumn.HeaderText = "SiteId";
            this.siteIdDataGridViewTextBoxColumn.Name = "siteIdDataGridViewTextBoxColumn";
            this.siteIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn
            // 
            this.synchStatusDataGridViewCheckBoxColumn.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.Name = "synchStatusDataGridViewCheckBoxColumn";
            this.synchStatusDataGridViewCheckBoxColumn.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn
            // 
            this.guidDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn.Name = "guidDataGridViewTextBoxColumn";
            this.guidDataGridViewTextBoxColumn.Visible = false;
            // 
            // isChangedDataGridViewCheckBoxColumn
            // 
            this.isChangedDataGridViewCheckBoxColumn.DataPropertyName = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.HeaderText = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.Name = "isChangedDataGridViewCheckBoxColumn";
            this.isChangedDataGridViewCheckBoxColumn.Visible = false;
            // 
            // subscriptionBillingScheduleDTOBindingSource
            // 
            this.subscriptionBillingScheduleDTOBindingSource.DataSource = typeof(Semnox.Parafait.Transaction.SubscriptionBillingScheduleDTO);
            // 
            // vScrollBillingCycle
            // 
            this.vScrollBillingCycle.AutoHide = false;
            this.vScrollBillingCycle.DataGridView = this.dgvSubscriptionBillingSchedule;
            this.vScrollBillingCycle.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBillingCycle.DownButtonBackgroundImage")));
            this.vScrollBillingCycle.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBillingCycle.DownButtonDisabledBackgroundImage")));
            this.vScrollBillingCycle.Location = new System.Drawing.Point(1189, 21);
            this.vScrollBillingCycle.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollBillingCycle.Name = "vScrollBillingCycle";
            this.vScrollBillingCycle.ScrollableControl = null;
            this.vScrollBillingCycle.ScrollViewer = null;
            this.vScrollBillingCycle.Size = new System.Drawing.Size(40, 229);
            this.vScrollBillingCycle.TabIndex = 1;
            this.vScrollBillingCycle.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBillingCycle.UpButtonBackgroundImage")));
            this.vScrollBillingCycle.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBillingCycle.UpButtonDisabledBackgroundImage")));
            // 
            // grpHeader
            // 
            this.grpHeader.Controls.Add(this.fpnlHeader);
            this.grpHeader.Location = new System.Drawing.Point(2, 5);
            this.grpHeader.Name = "grpHeader";
            this.grpHeader.Size = new System.Drawing.Size(1236, 315);
            this.grpHeader.TabIndex = 0;
            this.grpHeader.TabStop = false;
            this.grpHeader.Text = "Header";
            // 
            // fpnlHeader
            // 
            this.fpnlHeader.Controls.Add(this.pnlMainHeader);
            this.fpnlHeader.Controls.Add(this.pnlMainHeaderBody);
            this.fpnlHeader.Controls.Add(this.pnlPaymentSeason);
            this.fpnlHeader.Controls.Add(this.pnlPaymentAndSeason);
            this.fpnlHeader.Controls.Add(this.pnlRenewalHeader);
            this.fpnlHeader.Controls.Add(this.pnlRenewalAndReminders);
            this.fpnlHeader.Location = new System.Drawing.Point(6, 18);
            this.fpnlHeader.Name = "fpnlHeader";
            this.fpnlHeader.Size = new System.Drawing.Size(1224, 292);
            this.fpnlHeader.TabIndex = 0;
            // 
            // pnlMainHeader
            // 
            this.pnlMainHeader.BackColor = System.Drawing.Color.LightBlue;
            this.pnlMainHeader.Controls.Add(this.btnExpandCollapse1);
            this.pnlMainHeader.Controls.Add(this.lblHeaderName);
            this.pnlMainHeader.Location = new System.Drawing.Point(3, 3);
            this.pnlMainHeader.Name = "pnlMainHeader";
            this.pnlMainHeader.Size = new System.Drawing.Size(1220, 34);
            this.pnlMainHeader.TabIndex = 0;
            this.pnlMainHeader.Click += new System.EventHandler(this.pnlMainHeader_Click);
            // 
            // btnExpandCollapse1
            // 
            this.btnExpandCollapse1.BackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExpandCollapse1.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnExpandCollapse1.FlatAppearance.BorderSize = 0;
            this.btnExpandCollapse1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandCollapse1.ForeColor = System.Drawing.Color.White;
            this.btnExpandCollapse1.Image = global::Parafait_POS.Properties.Resources.CollapseArrow;
            this.btnExpandCollapse1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExpandCollapse1.Location = new System.Drawing.Point(905, 0);
            this.btnExpandCollapse1.Name = "btnExpandCollapse1";
            this.btnExpandCollapse1.Size = new System.Drawing.Size(317, 34);
            this.btnExpandCollapse1.TabIndex = 130;
            this.btnExpandCollapse1.Tag = "";
            this.btnExpandCollapse1.UseVisualStyleBackColor = false;
            this.btnExpandCollapse1.Click += new System.EventHandler(this.btnExpandCollapse_Click);
            // 
            // lblHeaderName
            // 
            this.lblHeaderName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderName.Location = new System.Drawing.Point(3, 0);
            this.lblHeaderName.Name = "lblHeaderName";
            this.lblHeaderName.Size = new System.Drawing.Size(602, 35);
            this.lblHeaderName.TabIndex = 128;
            this.lblHeaderName.Text = "Name";
            this.lblHeaderName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblHeaderName.Click += new System.EventHandler(this.lblHeaderName_Click);
            // 
            // pnlMainHeaderBody
            // 
            this.pnlMainHeaderBody.Controls.Add(this.txtSubscriptionEndDate);
            this.pnlMainHeaderBody.Controls.Add(this.lblSubscriptionEndDate);
            this.pnlMainHeaderBody.Controls.Add(this.txtSubscriptionStartDate);
            this.pnlMainHeaderBody.Controls.Add(this.lblSubscriptionStartDate);
            this.pnlMainHeaderBody.Controls.Add(this.btnDetails);
            this.pnlMainHeaderBody.Controls.Add(this.btnClear);
            this.pnlMainHeaderBody.Controls.Add(this.btnSave);
            this.pnlMainHeaderBody.Controls.Add(this.cmbSubscriptionStatus);
            this.pnlMainHeaderBody.Controls.Add(this.lblStatus);
            this.pnlMainHeaderBody.Controls.Add(this.cmbCreditCardId);
            this.pnlMainHeaderBody.Controls.Add(this.lblCreditCard);
            this.pnlMainHeaderBody.Controls.Add(this.txtSubscriptionDescription);
            this.pnlMainHeaderBody.Controls.Add(this.lblSubscriptionDescription);
            this.pnlMainHeaderBody.Controls.Add(this.txtSubscriptionNo);
            this.pnlMainHeaderBody.Controls.Add(this.lblSubscriptionNo);
            this.pnlMainHeaderBody.Controls.Add(this.cmbCustomerContact);
            this.pnlMainHeaderBody.Controls.Add(this.lblCustomerContact);
            this.pnlMainHeaderBody.Controls.Add(this.txtSubscriptionCycleValidity);
            this.pnlMainHeaderBody.Controls.Add(this.lblSubscriptionCycleValidity);
            this.pnlMainHeaderBody.Controls.Add(this.txtUnitOfSubscriptionCycle);
            this.pnlMainHeaderBody.Controls.Add(this.lblUnitOfSubscriptionCycle);
            this.pnlMainHeaderBody.Controls.Add(this.txtSubscriptionPrice);
            this.pnlMainHeaderBody.Controls.Add(this.lblSubscriptionPrice);
            this.pnlMainHeaderBody.Controls.Add(this.txtSubscriptionCycle);
            this.pnlMainHeaderBody.Controls.Add(this.lblSubscriptionCycle);
            this.pnlMainHeaderBody.Location = new System.Drawing.Point(3, 43);
            this.pnlMainHeaderBody.Name = "pnlMainHeaderBody";
            this.pnlMainHeaderBody.Size = new System.Drawing.Size(1220, 160);
            this.pnlMainHeaderBody.TabIndex = 1;
            this.pnlMainHeaderBody.Click += new System.EventHandler(this.pnlMainHeaderBody_Click);
            // 
            // txtSubscriptionEndDate
            // 
            this.txtSubscriptionEndDate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubscriptionEndDate.Location = new System.Drawing.Point(963, 4);
            this.txtSubscriptionEndDate.MaxLength = 5;
            this.txtSubscriptionEndDate.Name = "txtSubscriptionEndDate";
            this.txtSubscriptionEndDate.ReadOnly = true;
            this.txtSubscriptionEndDate.Size = new System.Drawing.Size(130, 30);
            this.txtSubscriptionEndDate.TabIndex = 52;
            // 
            // lblSubscriptionEndDate
            // 
            this.lblSubscriptionEndDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubscriptionEndDate.Location = new System.Drawing.Point(826, 7);
            this.lblSubscriptionEndDate.Name = "lblSubscriptionEndDate";
            this.lblSubscriptionEndDate.Size = new System.Drawing.Size(131, 24);
            this.lblSubscriptionEndDate.TabIndex = 51;
            this.lblSubscriptionEndDate.Text = "End Date:";
            this.lblSubscriptionEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSubscriptionStartDate
            // 
            this.txtSubscriptionStartDate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubscriptionStartDate.Location = new System.Drawing.Point(609, 4);
            this.txtSubscriptionStartDate.MaxLength = 5;
            this.txtSubscriptionStartDate.Name = "txtSubscriptionStartDate";
            this.txtSubscriptionStartDate.ReadOnly = true;
            this.txtSubscriptionStartDate.Size = new System.Drawing.Size(130, 30);
            this.txtSubscriptionStartDate.TabIndex = 50;
            // 
            // lblSubscriptionStartDate
            // 
            this.lblSubscriptionStartDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubscriptionStartDate.Location = new System.Drawing.Point(473, 7);
            this.lblSubscriptionStartDate.Name = "lblSubscriptionStartDate";
            this.lblSubscriptionStartDate.Size = new System.Drawing.Size(131, 24);
            this.lblSubscriptionStartDate.TabIndex = 49;
            this.lblSubscriptionStartDate.Text = "Start Date:";
            this.lblSubscriptionStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnDetails
            // 
            this.btnDetails.BackColor = System.Drawing.Color.Transparent;
            this.btnDetails.BackgroundImage = global::Parafait_POS.Properties.Resources.More_Options_Btn_Normal;
            this.btnDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDetails.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnDetails.FlatAppearance.BorderSize = 0;
            this.btnDetails.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnDetails.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDetails.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDetails.ForeColor = System.Drawing.Color.White;
            this.btnDetails.Location = new System.Drawing.Point(1122, 84);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(70, 30);
            this.btnDetails.TabIndex = 48;
            this.btnDetails.UseVisualStyleBackColor = false;
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            this.btnDetails.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnDetails_MouseDown);
            this.btnDetails.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnDetails_MouseUp);
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
            this.btnClear.Location = new System.Drawing.Point(1122, 44);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(70, 30);
            this.btnClear.TabIndex = 47;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnClear.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnClear.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(1122, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 30);
            this.btnSave.TabIndex = 46;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnSave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // cmbSubscriptionStatus
            // 
            this.cmbSubscriptionStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSubscriptionStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSubscriptionStatus.Enabled = false;
            this.cmbSubscriptionStatus.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbSubscriptionStatus.FormattingEnabled = true;
            this.cmbSubscriptionStatus.Location = new System.Drawing.Point(167, 84);
            this.cmbSubscriptionStatus.Name = "cmbSubscriptionStatus";
            this.cmbSubscriptionStatus.Size = new System.Drawing.Size(226, 31);
            this.cmbSubscriptionStatus.TabIndex = 45;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(72, 87);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(93, 24);
            this.lblStatus.TabIndex = 44;
            this.lblStatus.Text = "Status: ";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbCreditCardId
            // 
            this.cmbCreditCardId.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCreditCardId.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCreditCardId.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbCreditCardId.FormattingEnabled = true;
            this.cmbCreditCardId.Location = new System.Drawing.Point(609, 123);
            this.cmbCreditCardId.Name = "cmbCreditCardId";
            this.cmbCreditCardId.Size = new System.Drawing.Size(226, 31);
            this.cmbCreditCardId.TabIndex = 43;
            // 
            // lblCreditCard
            // 
            this.lblCreditCard.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreditCard.Location = new System.Drawing.Point(446, 126);
            this.lblCreditCard.Name = "lblCreditCard";
            this.lblCreditCard.Size = new System.Drawing.Size(158, 24);
            this.lblCreditCard.TabIndex = 42;
            this.lblCreditCard.Text = "Credit Card:";
            this.lblCreditCard.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSubscriptionDescription
            // 
            this.txtSubscriptionDescription.Font = new System.Drawing.Font("Arial", 15F);
            this.txtSubscriptionDescription.Location = new System.Drawing.Point(167, 4);
            this.txtSubscriptionDescription.MaxLength = 10;
            this.txtSubscriptionDescription.Name = "txtSubscriptionDescription";
            this.txtSubscriptionDescription.ReadOnly = true;
            this.txtSubscriptionDescription.Size = new System.Drawing.Size(226, 30);
            this.txtSubscriptionDescription.TabIndex = 39;
            // 
            // lblSubscriptionDescription
            // 
            this.lblSubscriptionDescription.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblSubscriptionDescription.Location = new System.Drawing.Point(5, 7);
            this.lblSubscriptionDescription.Name = "lblSubscriptionDescription";
            this.lblSubscriptionDescription.Size = new System.Drawing.Size(160, 24);
            this.lblSubscriptionDescription.TabIndex = 38;
            this.lblSubscriptionDescription.Text = "Subscription Desciption: ";
            this.lblSubscriptionDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSubscriptionNo
            // 
            this.txtSubscriptionNo.Font = new System.Drawing.Font("Arial", 15F);
            this.txtSubscriptionNo.Location = new System.Drawing.Point(167, 44);
            this.txtSubscriptionNo.MaxLength = 10;
            this.txtSubscriptionNo.Name = "txtSubscriptionNo";
            this.txtSubscriptionNo.ReadOnly = true;
            this.txtSubscriptionNo.Size = new System.Drawing.Size(226, 30);
            this.txtSubscriptionNo.TabIndex = 39;
            // 
            // lblSubscriptionNo
            // 
            this.lblSubscriptionNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubscriptionNo.Location = new System.Drawing.Point(31, 47);
            this.lblSubscriptionNo.Name = "lblSubscriptionNo";
            this.lblSubscriptionNo.Size = new System.Drawing.Size(131, 24);
            this.lblSubscriptionNo.TabIndex = 36;
            this.lblSubscriptionNo.Text = "Subscription No:";
            this.lblSubscriptionNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbCustomerContact
            // 
            this.cmbCustomerContact.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCustomerContact.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCustomerContact.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbCustomerContact.FormattingEnabled = true;
            this.cmbCustomerContact.Location = new System.Drawing.Point(167, 123);
            this.cmbCustomerContact.Name = "cmbCustomerContact";
            this.cmbCustomerContact.Size = new System.Drawing.Size(226, 31);
            this.cmbCustomerContact.TabIndex = 35;
            // 
            // lblCustomerContact
            // 
            this.lblCustomerContact.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerContact.Location = new System.Drawing.Point(4, 126);
            this.lblCustomerContact.Name = "lblCustomerContact";
            this.lblCustomerContact.Size = new System.Drawing.Size(158, 24);
            this.lblCustomerContact.TabIndex = 32;
            this.lblCustomerContact.Text = "Customer Contact:";
            this.lblCustomerContact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSubscriptionCycleValidity
            // 
            this.txtSubscriptionCycleValidity.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubscriptionCycleValidity.Location = new System.Drawing.Point(962, 84);
            this.txtSubscriptionCycleValidity.MaxLength = 5;
            this.txtSubscriptionCycleValidity.Name = "txtSubscriptionCycleValidity";
            this.txtSubscriptionCycleValidity.ReadOnly = true;
            this.txtSubscriptionCycleValidity.Size = new System.Drawing.Size(130, 30);
            this.txtSubscriptionCycleValidity.TabIndex = 31;
            // 
            // lblSubscriptionCycleValidity
            // 
            this.lblSubscriptionCycleValidity.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubscriptionCycleValidity.Location = new System.Drawing.Point(755, 87);
            this.lblSubscriptionCycleValidity.Name = "lblSubscriptionCycleValidity";
            this.lblSubscriptionCycleValidity.Size = new System.Drawing.Size(201, 24);
            this.lblSubscriptionCycleValidity.TabIndex = 30;
            this.lblSubscriptionCycleValidity.Text = "Subscription Cycle Validity:";
            this.lblSubscriptionCycleValidity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUnitOfSubscriptionCycle
            // 
            this.txtUnitOfSubscriptionCycle.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnitOfSubscriptionCycle.Location = new System.Drawing.Point(609, 84);
            this.txtUnitOfSubscriptionCycle.MaxLength = 5;
            this.txtUnitOfSubscriptionCycle.Name = "txtUnitOfSubscriptionCycle";
            this.txtUnitOfSubscriptionCycle.ReadOnly = true;
            this.txtUnitOfSubscriptionCycle.Size = new System.Drawing.Size(130, 30);
            this.txtUnitOfSubscriptionCycle.TabIndex = 29;
            // 
            // lblUnitOfSubscriptionCycle
            // 
            this.lblUnitOfSubscriptionCycle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnitOfSubscriptionCycle.Location = new System.Drawing.Point(403, 87);
            this.lblUnitOfSubscriptionCycle.Name = "lblUnitOfSubscriptionCycle";
            this.lblUnitOfSubscriptionCycle.Size = new System.Drawing.Size(201, 24);
            this.lblUnitOfSubscriptionCycle.TabIndex = 28;
            this.lblUnitOfSubscriptionCycle.Text = "Unit Of Subscription Cycle:";
            this.lblUnitOfSubscriptionCycle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSubscriptionPrice
            // 
            this.txtSubscriptionPrice.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubscriptionPrice.Location = new System.Drawing.Point(609, 44);
            this.txtSubscriptionPrice.MaxLength = 5;
            this.txtSubscriptionPrice.Name = "txtSubscriptionPrice";
            this.txtSubscriptionPrice.ReadOnly = true;
            this.txtSubscriptionPrice.Size = new System.Drawing.Size(130, 30);
            this.txtSubscriptionPrice.TabIndex = 25;
            // 
            // lblSubscriptionPrice
            // 
            this.lblSubscriptionPrice.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubscriptionPrice.Location = new System.Drawing.Point(473, 47);
            this.lblSubscriptionPrice.Name = "lblSubscriptionPrice";
            this.lblSubscriptionPrice.Size = new System.Drawing.Size(131, 24);
            this.lblSubscriptionPrice.TabIndex = 24;
            this.lblSubscriptionPrice.Text = "Subscription Price:";
            this.lblSubscriptionPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSubscriptionCycle
            // 
            this.txtSubscriptionCycle.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubscriptionCycle.Location = new System.Drawing.Point(962, 44);
            this.txtSubscriptionCycle.MaxLength = 5;
            this.txtSubscriptionCycle.Name = "txtSubscriptionCycle";
            this.txtSubscriptionCycle.ReadOnly = true;
            this.txtSubscriptionCycle.Size = new System.Drawing.Size(130, 30);
            this.txtSubscriptionCycle.TabIndex = 27;
            // 
            // lblSubscriptionCycle
            // 
            this.lblSubscriptionCycle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubscriptionCycle.Location = new System.Drawing.Point(825, 47);
            this.lblSubscriptionCycle.Name = "lblSubscriptionCycle";
            this.lblSubscriptionCycle.Size = new System.Drawing.Size(131, 24);
            this.lblSubscriptionCycle.TabIndex = 26;
            this.lblSubscriptionCycle.Text = "Subscription Cycle:";
            this.lblSubscriptionCycle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlPaymentSeason
            // 
            this.pnlPaymentSeason.BackColor = System.Drawing.Color.LightBlue;
            this.pnlPaymentSeason.Controls.Add(this.btnExpandCollapse2);
            this.pnlPaymentSeason.Controls.Add(this.lblPaymentSeasonHeader);
            this.pnlPaymentSeason.Location = new System.Drawing.Point(3, 209);
            this.pnlPaymentSeason.Name = "pnlPaymentSeason";
            this.pnlPaymentSeason.Size = new System.Drawing.Size(1220, 34);
            this.pnlPaymentSeason.TabIndex = 2;
            this.pnlPaymentSeason.Click += new System.EventHandler(this.pnlPaymentSeason_Click);
            // 
            // btnExpandCollapse2
            // 
            this.btnExpandCollapse2.BackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExpandCollapse2.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnExpandCollapse2.FlatAppearance.BorderSize = 0;
            this.btnExpandCollapse2.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandCollapse2.ForeColor = System.Drawing.Color.White;
            this.btnExpandCollapse2.Image = global::Parafait_POS.Properties.Resources.ExpandArrow;
            this.btnExpandCollapse2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExpandCollapse2.Location = new System.Drawing.Point(905, 0);
            this.btnExpandCollapse2.Name = "btnExpandCollapse2";
            this.btnExpandCollapse2.Size = new System.Drawing.Size(317, 34);
            this.btnExpandCollapse2.TabIndex = 131;
            this.btnExpandCollapse2.Tag = "";
            this.btnExpandCollapse2.UseVisualStyleBackColor = false;
            this.btnExpandCollapse2.Click += new System.EventHandler(this.btnExpandCollapse_Click);
            // 
            // lblPaymentSeasonHeader
            // 
            this.lblPaymentSeasonHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentSeasonHeader.Location = new System.Drawing.Point(3, 0);
            this.lblPaymentSeasonHeader.Name = "lblPaymentSeasonHeader";
            this.lblPaymentSeasonHeader.Size = new System.Drawing.Size(602, 35);
            this.lblPaymentSeasonHeader.TabIndex = 128;
            this.lblPaymentSeasonHeader.Text = "Payment, Season and Actions";
            this.lblPaymentSeasonHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPaymentSeasonHeader.Click += new System.EventHandler(this.lblPaymentSeasonHeader_Click);
            // 
            // pnlPaymentAndSeason
            // 
            this.pnlPaymentAndSeason.Controls.Add(this.txtUnpausedBy);
            this.pnlPaymentAndSeason.Controls.Add(this.lblUnpausedBy);
            this.pnlPaymentAndSeason.Controls.Add(this.txtPausedBy);
            this.pnlPaymentAndSeason.Controls.Add(this.lblPausedBy);
            this.pnlPaymentAndSeason.Controls.Add(this.txtCancellationApprovedBy);
            this.pnlPaymentAndSeason.Controls.Add(this.lblCancellationApprovedBy);
            this.pnlPaymentAndSeason.Controls.Add(this.txtCancelledBy);
            this.pnlPaymentAndSeason.Controls.Add(this.lblCancelledBy);
            this.pnlPaymentAndSeason.Controls.Add(this.txtUnpauseApprovedBy);
            this.pnlPaymentAndSeason.Controls.Add(this.lblUnpauseApprovedBy);
            this.pnlPaymentAndSeason.Controls.Add(this.txtPauseApprovedBy);
            this.pnlPaymentAndSeason.Controls.Add(this.lblPauseApprovedBy);
            this.pnlPaymentAndSeason.Controls.Add(this.cmbCancellationOption);
            this.pnlPaymentAndSeason.Controls.Add(this.lblCancellationOption);
            this.pnlPaymentAndSeason.Controls.Add(this.cbxBillInAdvance);
            this.pnlPaymentAndSeason.Controls.Add(this.lblBillInAdvance);
            this.pnlPaymentAndSeason.Controls.Add(this.cbxAllowPause);
            this.pnlPaymentAndSeason.Controls.Add(this.lblAllowPause);
            this.pnlPaymentAndSeason.Controls.Add(this.cmbSelectedPaymentCollectionMode);
            this.pnlPaymentAndSeason.Controls.Add(this.lblPaymentColletionMode);
            this.pnlPaymentAndSeason.Controls.Add(this.txtSourceSubscriptionHeaderId);
            this.pnlPaymentAndSeason.Controls.Add(this.lblSourceSubscriptionHeaderId);
            this.pnlPaymentAndSeason.Controls.Add(this.txtSeasonStartDate);
            this.pnlPaymentAndSeason.Controls.Add(this.lblSeasonStartDate);
            this.pnlPaymentAndSeason.Location = new System.Drawing.Point(3, 249);
            this.pnlPaymentAndSeason.Name = "pnlPaymentAndSeason";
            this.pnlPaymentAndSeason.Size = new System.Drawing.Size(1220, 160);
            this.pnlPaymentAndSeason.TabIndex = 3;
            this.pnlPaymentAndSeason.Click += new System.EventHandler(this.pnlPaymentAndSeason_Click);
            // 
            // txtUnpausedBy
            // 
            this.txtUnpausedBy.Font = new System.Drawing.Font("Arial", 15F);
            this.txtUnpausedBy.Location = new System.Drawing.Point(970, 129);
            this.txtUnpausedBy.MaxLength = 10;
            this.txtUnpausedBy.Name = "txtUnpausedBy";
            this.txtUnpausedBy.ReadOnly = true;
            this.txtUnpausedBy.Size = new System.Drawing.Size(226, 30);
            this.txtUnpausedBy.TabIndex = 47;
            // 
            // lblUnpausedBy
            // 
            this.lblUnpausedBy.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblUnpausedBy.Location = new System.Drawing.Point(817, 128);
            this.lblUnpausedBy.Name = "lblUnpausedBy";
            this.lblUnpausedBy.Size = new System.Drawing.Size(150, 24);
            this.lblUnpausedBy.TabIndex = 46;
            this.lblUnpausedBy.Text = "Unpaused By: ";
            this.lblUnpausedBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPausedBy
            // 
            this.txtPausedBy.Font = new System.Drawing.Font("Arial", 15F);
            this.txtPausedBy.Location = new System.Drawing.Point(167, 129);
            this.txtPausedBy.MaxLength = 10;
            this.txtPausedBy.Name = "txtPausedBy";
            this.txtPausedBy.ReadOnly = true;
            this.txtPausedBy.Size = new System.Drawing.Size(226, 30);
            this.txtPausedBy.TabIndex = 43;
            // 
            // lblPausedBy
            // 
            this.lblPausedBy.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblPausedBy.Location = new System.Drawing.Point(4, 128);
            this.lblPausedBy.Name = "lblPausedBy";
            this.lblPausedBy.Size = new System.Drawing.Size(160, 24);
            this.lblPausedBy.TabIndex = 42;
            this.lblPausedBy.Text = "Pause By: ";
            this.lblPausedBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCancellationApprovedBy
            // 
            this.txtCancellationApprovedBy.Font = new System.Drawing.Font("Arial", 15F);
            this.txtCancellationApprovedBy.Location = new System.Drawing.Point(588, 89);
            this.txtCancellationApprovedBy.MaxLength = 10;
            this.txtCancellationApprovedBy.Name = "txtCancellationApprovedBy";
            this.txtCancellationApprovedBy.ReadOnly = true;
            this.txtCancellationApprovedBy.Size = new System.Drawing.Size(226, 30);
            this.txtCancellationApprovedBy.TabIndex = 47;
            // 
            // lblCancellationApprovedBy
            // 
            this.lblCancellationApprovedBy.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCancellationApprovedBy.Location = new System.Drawing.Point(425, 88);
            this.lblCancellationApprovedBy.Name = "lblCancellationApprovedBy";
            this.lblCancellationApprovedBy.Size = new System.Drawing.Size(160, 24);
            this.lblCancellationApprovedBy.TabIndex = 46;
            this.lblCancellationApprovedBy.Text = "Cancellation ApprovedBy: ";
            this.lblCancellationApprovedBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCancelledBy
            // 
            this.txtCancelledBy.Font = new System.Drawing.Font("Arial", 15F);
            this.txtCancelledBy.Location = new System.Drawing.Point(588, 129);
            this.txtCancelledBy.MaxLength = 10;
            this.txtCancelledBy.Name = "txtCancelledBy";
            this.txtCancelledBy.ReadOnly = true;
            this.txtCancelledBy.Size = new System.Drawing.Size(226, 30);
            this.txtCancelledBy.TabIndex = 47;
            // 
            // lblCancelledBy
            // 
            this.lblCancelledBy.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCancelledBy.Location = new System.Drawing.Point(425, 128);
            this.lblCancelledBy.Name = "lblCancelledBy";
            this.lblCancelledBy.Size = new System.Drawing.Size(160, 24);
            this.lblCancelledBy.TabIndex = 46;
            this.lblCancelledBy.Text = "Cancelled By: ";
            this.lblCancelledBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUnpauseApprovedBy
            // 
            this.txtUnpauseApprovedBy.Font = new System.Drawing.Font("Arial", 15F);
            this.txtUnpauseApprovedBy.Location = new System.Drawing.Point(970, 89);
            this.txtUnpauseApprovedBy.MaxLength = 10;
            this.txtUnpauseApprovedBy.Name = "txtUnpauseApprovedBy";
            this.txtUnpauseApprovedBy.ReadOnly = true;
            this.txtUnpauseApprovedBy.Size = new System.Drawing.Size(226, 30);
            this.txtUnpauseApprovedBy.TabIndex = 45;
            // 
            // lblUnpauseApprovedBy
            // 
            this.lblUnpauseApprovedBy.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblUnpauseApprovedBy.Location = new System.Drawing.Point(807, 88);
            this.lblUnpauseApprovedBy.Name = "lblUnpauseApprovedBy";
            this.lblUnpauseApprovedBy.Size = new System.Drawing.Size(160, 24);
            this.lblUnpauseApprovedBy.TabIndex = 44;
            this.lblUnpauseApprovedBy.Text = "Unpause ApprovedBy: ";
            this.lblUnpauseApprovedBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPauseApprovedBy
            // 
            this.txtPauseApprovedBy.Font = new System.Drawing.Font("Arial", 15F);
            this.txtPauseApprovedBy.Location = new System.Drawing.Point(167, 89);
            this.txtPauseApprovedBy.MaxLength = 10;
            this.txtPauseApprovedBy.Name = "txtPauseApprovedBy";
            this.txtPauseApprovedBy.ReadOnly = true;
            this.txtPauseApprovedBy.Size = new System.Drawing.Size(226, 30);
            this.txtPauseApprovedBy.TabIndex = 43;
            // 
            // lblPauseApprovedBy
            // 
            this.lblPauseApprovedBy.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblPauseApprovedBy.Location = new System.Drawing.Point(4, 88);
            this.lblPauseApprovedBy.Name = "lblPauseApprovedBy";
            this.lblPauseApprovedBy.Size = new System.Drawing.Size(160, 24);
            this.lblPauseApprovedBy.TabIndex = 42;
            this.lblPauseApprovedBy.Text = "Pause Approved By: ";
            this.lblPauseApprovedBy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbCancellationOption
            // 
            this.cmbCancellationOption.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCancellationOption.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCancellationOption.Enabled = false;
            this.cmbCancellationOption.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbCancellationOption.FormattingEnabled = true;
            this.cmbCancellationOption.Location = new System.Drawing.Point(167, 47);
            this.cmbCancellationOption.Name = "cmbCancellationOption";
            this.cmbCancellationOption.Size = new System.Drawing.Size(226, 31);
            this.cmbCancellationOption.TabIndex = 54;
            // 
            // lblCancellationOption
            // 
            this.lblCancellationOption.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCancellationOption.Location = new System.Drawing.Point(4, 48);
            this.lblCancellationOption.Name = "lblCancellationOption";
            this.lblCancellationOption.Size = new System.Drawing.Size(158, 24);
            this.lblCancellationOption.TabIndex = 53;
            this.lblCancellationOption.Text = "Cancellation Option:";
            this.lblCancellationOption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxBillInAdvance
            // 
            this.cbxBillInAdvance.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxBillInAdvance.AutoSize = true;
            this.cbxBillInAdvance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxBillInAdvance.Enabled = false;
            this.cbxBillInAdvance.FlatAppearance.BorderSize = 0;
            this.cbxBillInAdvance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxBillInAdvance.Font = new System.Drawing.Font("Arial", 15F);
            this.cbxBillInAdvance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxBillInAdvance.ImageIndex = 1;
            this.cbxBillInAdvance.Location = new System.Drawing.Point(588, 7);
            this.cbxBillInAdvance.Name = "cbxBillInAdvance";
            this.cbxBillInAdvance.Size = new System.Drawing.Size(26, 26);
            this.cbxBillInAdvance.TabIndex = 34;
            this.cbxBillInAdvance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxBillInAdvance.UseVisualStyleBackColor = true;
            // 
            // lblBillInAdvance
            // 
            this.lblBillInAdvance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillInAdvance.Location = new System.Drawing.Point(450, 8);
            this.lblBillInAdvance.Name = "lblBillInAdvance";
            this.lblBillInAdvance.Size = new System.Drawing.Size(131, 24);
            this.lblBillInAdvance.TabIndex = 33;
            this.lblBillInAdvance.Text = "Bill In Advance?:";
            this.lblBillInAdvance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxAllowPause
            // 
            this.cbxAllowPause.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxAllowPause.AutoSize = true;
            this.cbxAllowPause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxAllowPause.Enabled = false;
            this.cbxAllowPause.FlatAppearance.BorderSize = 0;
            this.cbxAllowPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxAllowPause.Font = new System.Drawing.Font("Arial", 15F);
            this.cbxAllowPause.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxAllowPause.ImageIndex = 1;
            this.cbxAllowPause.Location = new System.Drawing.Point(970, 7);
            this.cbxAllowPause.Name = "cbxAllowPause";
            this.cbxAllowPause.Size = new System.Drawing.Size(26, 26);
            this.cbxAllowPause.TabIndex = 37;
            this.cbxAllowPause.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAllowPause.UseVisualStyleBackColor = true;
            // 
            // lblAllowPause
            // 
            this.lblAllowPause.Enabled = false;
            this.lblAllowPause.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllowPause.Location = new System.Drawing.Point(834, 8);
            this.lblAllowPause.Name = "lblAllowPause";
            this.lblAllowPause.Size = new System.Drawing.Size(131, 24);
            this.lblAllowPause.TabIndex = 36;
            this.lblAllowPause.Text = "Allow Pause?:";
            this.lblAllowPause.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbSelectedPaymentCollectionMode
            // 
            this.cmbSelectedPaymentCollectionMode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSelectedPaymentCollectionMode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSelectedPaymentCollectionMode.Enabled = false;
            this.cmbSelectedPaymentCollectionMode.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbSelectedPaymentCollectionMode.FormattingEnabled = true;
            this.cmbSelectedPaymentCollectionMode.Location = new System.Drawing.Point(167, 5);
            this.cmbSelectedPaymentCollectionMode.Name = "cmbSelectedPaymentCollectionMode";
            this.cmbSelectedPaymentCollectionMode.Size = new System.Drawing.Size(226, 31);
            this.cmbSelectedPaymentCollectionMode.TabIndex = 35;
            // 
            // lblPaymentColletionMode
            // 
            this.lblPaymentColletionMode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentColletionMode.Location = new System.Drawing.Point(3, 8);
            this.lblPaymentColletionMode.Name = "lblPaymentColletionMode";
            this.lblPaymentColletionMode.Size = new System.Drawing.Size(158, 24);
            this.lblPaymentColletionMode.TabIndex = 32;
            this.lblPaymentColletionMode.Text = "Payment Mode:";
            this.lblPaymentColletionMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSourceSubscriptionHeaderId
            // 
            this.txtSourceSubscriptionHeaderId.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSourceSubscriptionHeaderId.Location = new System.Drawing.Point(970, 45);
            this.txtSourceSubscriptionHeaderId.MaxLength = 5;
            this.txtSourceSubscriptionHeaderId.Name = "txtSourceSubscriptionHeaderId";
            this.txtSourceSubscriptionHeaderId.ReadOnly = true;
            this.txtSourceSubscriptionHeaderId.Size = new System.Drawing.Size(226, 30);
            this.txtSourceSubscriptionHeaderId.TabIndex = 29;
            // 
            // lblSourceSubscriptionHeaderId
            // 
            this.lblSourceSubscriptionHeaderId.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSourceSubscriptionHeaderId.Location = new System.Drawing.Point(820, 48);
            this.lblSourceSubscriptionHeaderId.Name = "lblSourceSubscriptionHeaderId";
            this.lblSourceSubscriptionHeaderId.Size = new System.Drawing.Size(145, 24);
            this.lblSourceSubscriptionHeaderId.TabIndex = 28;
            this.lblSourceSubscriptionHeaderId.Text = "Source Subscription:";
            this.lblSourceSubscriptionHeaderId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSeasonStartDate
            // 
            this.txtSeasonStartDate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSeasonStartDate.Location = new System.Drawing.Point(588, 45);
            this.txtSeasonStartDate.MaxLength = 5;
            this.txtSeasonStartDate.Name = "txtSeasonStartDate";
            this.txtSeasonStartDate.ReadOnly = true;
            this.txtSeasonStartDate.Size = new System.Drawing.Size(226, 30);
            this.txtSeasonStartDate.TabIndex = 25;
            // 
            // lblSeasonStartDate
            // 
            this.lblSeasonStartDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeasonStartDate.Location = new System.Drawing.Point(450, 48);
            this.lblSeasonStartDate.Name = "lblSeasonStartDate";
            this.lblSeasonStartDate.Size = new System.Drawing.Size(131, 24);
            this.lblSeasonStartDate.TabIndex = 24;
            this.lblSeasonStartDate.Text = "Season Start Date:";
            this.lblSeasonStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlRenewalHeader
            // 
            this.pnlRenewalHeader.BackColor = System.Drawing.Color.LightBlue;
            this.pnlRenewalHeader.Controls.Add(this.btnExpandCollapse3);
            this.pnlRenewalHeader.Controls.Add(this.lblRenewalHeader);
            this.pnlRenewalHeader.Location = new System.Drawing.Point(3, 415);
            this.pnlRenewalHeader.Name = "pnlRenewalHeader";
            this.pnlRenewalHeader.Size = new System.Drawing.Size(1220, 34);
            this.pnlRenewalHeader.TabIndex = 4;
            this.pnlRenewalHeader.Click += new System.EventHandler(this.pnlRenewalHeader_Click);
            // 
            // btnExpandCollapse3
            // 
            this.btnExpandCollapse3.BackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExpandCollapse3.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnExpandCollapse3.FlatAppearance.BorderSize = 0;
            this.btnExpandCollapse3.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapse3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandCollapse3.ForeColor = System.Drawing.Color.White;
            this.btnExpandCollapse3.Image = global::Parafait_POS.Properties.Resources.ExpandArrow;
            this.btnExpandCollapse3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExpandCollapse3.Location = new System.Drawing.Point(905, 0);
            this.btnExpandCollapse3.Name = "btnExpandCollapse3";
            this.btnExpandCollapse3.Size = new System.Drawing.Size(317, 34);
            this.btnExpandCollapse3.TabIndex = 131;
            this.btnExpandCollapse3.Tag = "";
            this.btnExpandCollapse3.UseVisualStyleBackColor = false;
            this.btnExpandCollapse3.Click += new System.EventHandler(this.btnExpandCollapse_Click);
            // 
            // lblRenewalHeader
            // 
            this.lblRenewalHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRenewalHeader.Location = new System.Drawing.Point(3, 0);
            this.lblRenewalHeader.Name = "lblRenewalHeader";
            this.lblRenewalHeader.Size = new System.Drawing.Size(602, 35);
            this.lblRenewalHeader.TabIndex = 128;
            this.lblRenewalHeader.Text = "Renewal and reminders";
            this.lblRenewalHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblRenewalHeader.Click += new System.EventHandler(this.lblRenewalHeader_Click);
            // 
            // pnlRenewalAndReminders
            // 
            this.pnlRenewalAndReminders.Controls.Add(this.txtAutoRenewalMarkup);
            this.pnlRenewalAndReminders.Controls.Add(this.lblAutoRenewalMarkup);
            this.pnlRenewalAndReminders.Controls.Add(this.cbxAutoRenew);
            this.pnlRenewalAndReminders.Controls.Add(this.lblAutoRenew);
            this.pnlRenewalAndReminders.Controls.Add(this.txtFirstReminderBeforeXDays);
            this.pnlRenewalAndReminders.Controls.Add(this.lblFirstReminderBeforeXDays);
            this.pnlRenewalAndReminders.Controls.Add(this.txtRenewalReminderFrequency);
            this.pnlRenewalAndReminders.Controls.Add(this.lblRenewalReminderFrequency1);
            this.pnlRenewalAndReminders.Controls.Add(this.txtNextReminderOn);
            this.pnlRenewalAndReminders.Controls.Add(this.lblNextReminderOn);
            this.pnlRenewalAndReminders.Controls.Add(this.txtLastReminderSentOn);
            this.pnlRenewalAndReminders.Controls.Add(this.lblLastReminderSentOn);
            this.pnlRenewalAndReminders.Controls.Add(this.txtRenewalReminderSent);
            this.pnlRenewalAndReminders.Controls.Add(this.lblRenewalReminders);
            this.pnlRenewalAndReminders.Controls.Add(this.txtFreeTrialPeriod);
            this.pnlRenewalAndReminders.Controls.Add(this.lblFreeTrialPeriod);
            this.pnlRenewalAndReminders.Controls.Add(this.txtRenewalGracePeriod);
            this.pnlRenewalAndReminders.Controls.Add(this.lblRenewalGracePeriod);
            this.pnlRenewalAndReminders.Location = new System.Drawing.Point(3, 455);
            this.pnlRenewalAndReminders.Name = "pnlRenewalAndReminders";
            this.pnlRenewalAndReminders.Size = new System.Drawing.Size(1220, 160);
            this.pnlRenewalAndReminders.TabIndex = 5;
            this.pnlRenewalAndReminders.Click += new System.EventHandler(this.pnlRenewalAndReminders_Click);
            // 
            // txtAutoRenewalMarkup
            // 
            this.txtAutoRenewalMarkup.Font = new System.Drawing.Font("Arial", 15F); 
            this.txtAutoRenewalMarkup.Location = new System.Drawing.Point(608, 86);

            this.txtAutoRenewalMarkup.MaxLength = 10;
            this.txtAutoRenewalMarkup.Name = "txtAutoRenewalMarkup";
            this.txtAutoRenewalMarkup.Size = new System.Drawing.Size(130, 30);
            this.txtAutoRenewalMarkup.TabIndex = 41;
            // 
            // lblAutoRenewalMarkup
            // 
            this.lblAutoRenewalMarkup.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAutoRenewalMarkup.Location = new System.Drawing.Point(447, 89);
            this.lblAutoRenewalMarkup.Name = "lblAutoRenewalMarkup";
            this.lblAutoRenewalMarkup.Size = new System.Drawing.Size(160, 24);
            this.lblAutoRenewalMarkup.TabIndex = 40;
            this.lblAutoRenewalMarkup.Text = "Auto Renewal Markup: ";
            this.lblAutoRenewalMarkup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.cbxAutoRenew.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxAutoRenew.AutoSize = true;
            this.cbxAutoRenew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxAutoRenew.FlatAppearance.BorderSize = 0;
            this.cbxAutoRenew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxAutoRenew.Font = new System.Drawing.Font("Arial", 15F);
            this.cbxAutoRenew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxAutoRenew.ImageIndex = 1;
            this.cbxAutoRenew.Location = new System.Drawing.Point(957, 86); 
            this.cbxAutoRenew.Name = "cbxAutoRenew";
            this.cbxAutoRenew.Size = new System.Drawing.Size(26, 26);
            this.cbxAutoRenew.TabIndex = 37;
            this.cbxAutoRenew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAutoRenew.UseVisualStyleBackColor = true;
            // 
            // lblAutoRenew
            // 
            this.lblAutoRenew.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))); 
            this.lblAutoRenew.Location = new System.Drawing.Point(828, 89);
            this.lblAutoRenew.Name = "lblAutoRenew";
            this.lblAutoRenew.Size = new System.Drawing.Size(131, 24);
            this.lblAutoRenew.TabIndex = 36;
            this.lblAutoRenew.Text = "Auto Renew?:";
            this.lblAutoRenew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;  
            // 
            // txtFirstReminderBeforeXDays
            // 
            this.txtFirstReminderBeforeXDays.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstReminderBeforeXDays.Location = new System.Drawing.Point(608, 45);
            this.txtFirstReminderBeforeXDays.MaxLength = 5;
            this.txtFirstReminderBeforeXDays.Name = "txtFirstReminderBeforeXDays";
            this.txtFirstReminderBeforeXDays.ReadOnly = true;
            this.txtFirstReminderBeforeXDays.Size = new System.Drawing.Size(130, 30);
            this.txtFirstReminderBeforeXDays.TabIndex = 39;
            // 
            // lblFirstReminderBeforeXDays
            // 
            this.lblFirstReminderBeforeXDays.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstReminderBeforeXDays.Location = new System.Drawing.Point(402, 48);
            this.lblFirstReminderBeforeXDays.Name = "lblFirstReminderBeforeXDays";
            this.lblFirstReminderBeforeXDays.Size = new System.Drawing.Size(201, 24);
            this.lblFirstReminderBeforeXDays.TabIndex = 38;
            this.lblFirstReminderBeforeXDays.Text = "First Reminder Before X Days:";
            this.lblFirstReminderBeforeXDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRenewalReminderFrequency
            // 
            this.txtRenewalReminderFrequency.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRenewalReminderFrequency.Location = new System.Drawing.Point(962, 45);
            this.txtRenewalReminderFrequency.MaxLength = 5;
            this.txtRenewalReminderFrequency.Name = "txtRenewalReminderFrequency";
            this.txtRenewalReminderFrequency.ReadOnly = true;
            this.txtRenewalReminderFrequency.Size = new System.Drawing.Size(130, 30);
            this.txtRenewalReminderFrequency.TabIndex = 37;
            // 
            // lblRenewalReminderFrequency1
            // 
            this.lblRenewalReminderFrequency1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRenewalReminderFrequency1.Location = new System.Drawing.Point(756, 48);
            this.lblRenewalReminderFrequency1.Name = "lblRenewalReminderFrequency1";
            this.lblRenewalReminderFrequency1.Size = new System.Drawing.Size(201, 24);
            this.lblRenewalReminderFrequency1.TabIndex = 36;
            this.lblRenewalReminderFrequency1.Text = "Renewal Reminder Frequency:";
            this.lblRenewalReminderFrequency1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNextReminderOn
            // 
            this.txtNextReminderOn.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNextReminderOn.Location = new System.Drawing.Point(167, 86);
            this.txtNextReminderOn.MaxLength = 5;
            this.txtNextReminderOn.Name = "txtNextReminderOn";
            this.txtNextReminderOn.ReadOnly = true;
            this.txtNextReminderOn.Size = new System.Drawing.Size(130, 30);
            this.txtNextReminderOn.TabIndex = 35;
            // 
            // lblNextReminderOn
            // 
            this.lblNextReminderOn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNextReminderOn.Location = new System.Drawing.Point(9, 89);
            this.lblNextReminderOn.Name = "lblNextReminderOn";
            this.lblNextReminderOn.Size = new System.Drawing.Size(156, 24);
            this.lblNextReminderOn.TabIndex = 34;
            this.lblNextReminderOn.Text = "Next Reminder On:";
            this.lblNextReminderOn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLastReminderSentOn
            // 
            this.txtLastReminderSentOn.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastReminderSentOn.Location = new System.Drawing.Point(167, 45);
            this.txtLastReminderSentOn.MaxLength = 5;
            this.txtLastReminderSentOn.Name = "txtLastReminderSentOn";
            this.txtLastReminderSentOn.ReadOnly = true;
            this.txtLastReminderSentOn.Size = new System.Drawing.Size(130, 30);
            this.txtLastReminderSentOn.TabIndex = 33;
            // 
            // lblLastReminderSentOn
            // 
            this.lblLastReminderSentOn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastReminderSentOn.Location = new System.Drawing.Point(3, 48);
            this.lblLastReminderSentOn.Name = "lblLastReminderSentOn";
            this.lblLastReminderSentOn.Size = new System.Drawing.Size(162, 24);
            this.lblLastReminderSentOn.TabIndex = 32;
            this.lblLastReminderSentOn.Text = "Last Reminder Sent On:";
            this.lblLastReminderSentOn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRenewalReminderSent
            // 
            this.txtRenewalReminderSent.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRenewalReminderSent.Location = new System.Drawing.Point(962, 5);
            this.txtRenewalReminderSent.MaxLength = 5;
            this.txtRenewalReminderSent.Name = "txtRenewalReminderSent";
            this.txtRenewalReminderSent.ReadOnly = true;
            this.txtRenewalReminderSent.Size = new System.Drawing.Size(130, 30);
            this.txtRenewalReminderSent.TabIndex = 31;
            // 
            // lblRenewalReminders
            // 
            this.lblRenewalReminders.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRenewalReminders.Location = new System.Drawing.Point(756, 8);
            this.lblRenewalReminders.Name = "lblRenewalReminders";
            this.lblRenewalReminders.Size = new System.Drawing.Size(201, 24);
            this.lblRenewalReminders.TabIndex = 30;
            this.lblRenewalReminders.Text = "Renewal Reminder Count:";
            this.lblRenewalReminders.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFreeTrialPeriod
            // 
            this.txtFreeTrialPeriod.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFreeTrialPeriod.Location = new System.Drawing.Point(608, 5);
            this.txtFreeTrialPeriod.MaxLength = 5;
            this.txtFreeTrialPeriod.Name = "txtFreeTrialPeriod";
            this.txtFreeTrialPeriod.ReadOnly = true;
            this.txtFreeTrialPeriod.Size = new System.Drawing.Size(130, 30);
            this.txtFreeTrialPeriod.TabIndex = 29;
            // 
            // lblFreeTrialPeriod
            // 
            this.lblFreeTrialPeriod.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFreeTrialPeriod.Location = new System.Drawing.Point(402, 8);
            this.lblFreeTrialPeriod.Name = "lblFreeTrialPeriod";
            this.lblFreeTrialPeriod.Size = new System.Drawing.Size(201, 24);
            this.lblFreeTrialPeriod.TabIndex = 28;
            this.lblFreeTrialPeriod.Text = "Free Trial Periods:";
            this.lblFreeTrialPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRenewalGracePeriod
            // 
            this.txtRenewalGracePeriod.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRenewalGracePeriod.Location = new System.Drawing.Point(167, 5);
            this.txtRenewalGracePeriod.MaxLength = 5;
            this.txtRenewalGracePeriod.Name = "txtRenewalGracePeriod";
            this.txtRenewalGracePeriod.ReadOnly = true;
            this.txtRenewalGracePeriod.Size = new System.Drawing.Size(130, 30);
            this.txtRenewalGracePeriod.TabIndex = 25;
            // 
            // lblRenewalGracePeriod
            // 
            this.lblRenewalGracePeriod.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRenewalGracePeriod.Location = new System.Drawing.Point(3, 8);
            this.lblRenewalGracePeriod.Name = "lblRenewalGracePeriod";
            this.lblRenewalGracePeriod.Size = new System.Drawing.Size(162, 24);
            this.lblRenewalGracePeriod.TabIndex = 24;
            this.lblRenewalGracePeriod.Text = "Renewal Grace Period:";
            this.lblRenewalGracePeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmSubscriptionDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1244, 627);
            this.Controls.Add(this.pnlPage);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSubscriptionDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Subscription Details";
            this.pnlPage.ResumeLayout(false);
            this.pnlPage.PerformLayout();
            this.fpnlHeaderActions.ResumeLayout(false);
            this.grpBillingCycle.ResumeLayout(false);
            this.grpBillingCycle.PerformLayout();
            this.fpnlBillingCycleActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubscriptionBillingSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subscriptionBillingScheduleDTOBindingSource)).EndInit();
            this.grpHeader.ResumeLayout(false);
            this.fpnlHeader.ResumeLayout(false);
            this.pnlMainHeader.ResumeLayout(false);
            this.pnlMainHeaderBody.ResumeLayout(false);
            this.pnlMainHeaderBody.PerformLayout();
            this.pnlPaymentSeason.ResumeLayout(false);
            this.pnlPaymentAndSeason.ResumeLayout(false);
            this.pnlPaymentAndSeason.PerformLayout();
            this.pnlRenewalHeader.ResumeLayout(false);
            this.pnlRenewalAndReminders.ResumeLayout(false);
            this.pnlRenewalAndReminders.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpHeader;
        private System.Windows.Forms.FlowLayoutPanel fpnlHeader;
        private System.Windows.Forms.Panel pnlMainHeader;
        private System.Windows.Forms.Panel pnlMainHeaderBody;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbCustomerContact;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxBillInAdvance;
        private System.Windows.Forms.Label lblBillInAdvance;
        private System.Windows.Forms.Label lblCustomerContact;
        private System.Windows.Forms.TextBox txtSubscriptionCycleValidity;
        private System.Windows.Forms.Label lblSubscriptionCycleValidity;
        private System.Windows.Forms.TextBox txtUnitOfSubscriptionCycle;
        private System.Windows.Forms.Label lblUnitOfSubscriptionCycle;
        private System.Windows.Forms.TextBox txtSubscriptionCycle;
        private System.Windows.Forms.Label lblSubscriptionCycle;
        private System.Windows.Forms.TextBox txtSubscriptionPrice;
        private System.Windows.Forms.Label lblSubscriptionPrice;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxAutoRenew;
        private System.Windows.Forms.Label lblAutoRenew;
        private System.Windows.Forms.Label lblSubscriptionNo;
        private System.Windows.Forms.TextBox txtSubscriptionNo;
        private System.Windows.Forms.TextBox txtSubscriptionDescription;
        private System.Windows.Forms.Label lblSubscriptionDescription;
        private System.Windows.Forms.TextBox txtAutoRenewalMarkup;
        private System.Windows.Forms.Label lblAutoRenewalMarkup;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbCreditCardId;
        private System.Windows.Forms.Label lblCreditCard;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSubscriptionStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnDetails;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblHeaderName;
        private System.Windows.Forms.Button btnExpandCollapse1;
        private System.Windows.Forms.Panel pnlPaymentSeason;
        private System.Windows.Forms.Label lblPaymentSeasonHeader;
        private System.Windows.Forms.Panel pnlPaymentAndSeason;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxAllowPause;
        private System.Windows.Forms.Label lblAllowPause;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSelectedPaymentCollectionMode;
        private System.Windows.Forms.Label lblPaymentColletionMode;
        private System.Windows.Forms.TextBox txtSourceSubscriptionHeaderId;
        private System.Windows.Forms.Label lblSourceSubscriptionHeaderId;
        private System.Windows.Forms.TextBox txtSeasonStartDate;
        private System.Windows.Forms.Label lblSeasonStartDate;
        private System.Windows.Forms.Button btnExpandCollapse2;
        private System.Windows.Forms.Panel pnlRenewalHeader;
        private System.Windows.Forms.Button btnExpandCollapse3;
        private System.Windows.Forms.Label lblRenewalHeader;
        private System.Windows.Forms.Panel pnlRenewalAndReminders;
        private System.Windows.Forms.TextBox txtFreeTrialPeriod;
        private System.Windows.Forms.Label lblFreeTrialPeriod;
        private System.Windows.Forms.TextBox txtRenewalGracePeriod;
        private System.Windows.Forms.Label lblRenewalGracePeriod;
        private System.Windows.Forms.TextBox txtRenewalReminderFrequency;
        private System.Windows.Forms.Label lblRenewalReminderFrequency1;
        private System.Windows.Forms.TextBox txtNextReminderOn;
        private System.Windows.Forms.Label lblNextReminderOn;
        private System.Windows.Forms.TextBox txtLastReminderSentOn;
        private System.Windows.Forms.Label lblLastReminderSentOn;
        private System.Windows.Forms.TextBox txtRenewalReminderSent;
        private System.Windows.Forms.Label lblRenewalReminders;
        private System.Windows.Forms.TextBox txtFirstReminderBeforeXDays;
        private System.Windows.Forms.Label lblFirstReminderBeforeXDays;
        private System.Windows.Forms.GroupBox grpBillingCycle;
        private System.Windows.Forms.DataGridView dgvSubscriptionBillingSchedule;
        private System.Windows.Forms.BindingSource subscriptionBillingScheduleDTOBindingSource;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView hScrollBillingCycle;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vScrollBillingCycle;
        private System.Windows.Forms.FlowLayoutPanel fpnlHeaderActions;
        private System.Windows.Forms.Button btnCancelSubscription;
        private System.Windows.Forms.Button btnSendManualRenewalReminder;
        private System.Windows.Forms.Button btnViewHistory;
        private System.Windows.Forms.Button btnPauseSubscription;
        private System.Windows.Forms.Button btnReactivateSubscription;
        private System.Windows.Forms.FlowLayoutPanel fpnlBillingCycleActions;
        private System.Windows.Forms.Button btnOverridePrice;
        private System.Windows.Forms.Button btnResetPaymentErrorCount;
        private System.Windows.Forms.Button btnPrintReceipt;
        private System.Windows.Forms.Button btnEmailReceipt;
        private System.Windows.Forms.TextBox txtSubscriptionEndDate;
        private System.Windows.Forms.Label lblSubscriptionEndDate;
        private System.Windows.Forms.TextBox txtSubscriptionStartDate;
        private System.Windows.Forms.Label lblSubscriptionStartDate;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbCancellationOption;
        private System.Windows.Forms.Label lblCancellationOption;
        private System.Windows.Forms.TextBox txtPauseApprovedBy;
        private System.Windows.Forms.Label lblPauseApprovedBy;
        private System.Windows.Forms.TextBox txtCancellationApprovedBy;
        private System.Windows.Forms.Label lblCancellationApprovedBy;
        private System.Windows.Forms.TextBox txtCancelledBy;
        private System.Windows.Forms.Label lblCancelledBy;
        private System.Windows.Forms.TextBox txtUnpauseApprovedBy;
        private System.Windows.Forms.Label lblUnpauseApprovedBy;
        private System.Windows.Forms.TextBox txtPausedBy;
        private System.Windows.Forms.Label lblPausedBy; 
        private System.Windows.Forms.TextBox txtUnpausedBy;
        private System.Windows.Forms.Label lblUnpausedBy;
        private System.Windows.Forms.DataGridViewButtonColumn dgvColumnbillCycleActions;
        private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionBillingScheduleId;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineType;
        private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionHeaderIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionLineIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billFromDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billToDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billOnDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvColumnTransactionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentProcessingFailureCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overridedBillAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overrideByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overrideApprovedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CancelledBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CancellationApprovedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn overrideReasonDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.Panel pnlPage;
    }
}