namespace Parafait_POS.Subscription
{
    partial class frmSubscriptionStaffView
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
            this.tcDashboard = new System.Windows.Forms.TabControl();
            this.tpDashboardView = new System.Windows.Forms.TabPage();
            this.gbxExpiryStats = new System.Windows.Forms.GroupBox();
            this.btnPaymentErrorCount = new System.Windows.Forms.Button();
            this.btnExpiryingCreditCards = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbCardExpiryInDays = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnExpiryingSubscriptions = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSubscriptionExpiryInDays = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbxKeyStats = new System.Windows.Forms.GroupBox();
            this.txtSubscriptionChurnRate = new System.Windows.Forms.TextBox();
            this.lblMonthlySubscriptionChurn = new System.Windows.Forms.Label();
            this.txtTotalActiveSubscriptions = new System.Windows.Forms.TextBox();
            this.lblTotalActiveSubscriptions = new System.Windows.Forms.Label();
            this.txtMonthlyCustomerChurnRate = new System.Windows.Forms.TextBox();
            this.lblMonthlyCustomerChurnRate = new System.Windows.Forms.Label();
            this.txtMonthlyRecurringRevenue = new System.Windows.Forms.TextBox();
            this.lblMonthlyRecurringRevenue = new System.Windows.Forms.Label();
            this.txtTotalActiveCustomers = new System.Windows.Forms.TextBox();
            this.lblTotalActiveCustomers = new System.Windows.Forms.Label();
            this.gbxTodayStats = new System.Windows.Forms.GroupBox();
            this.gbxActionsAndCancellations = new System.Windows.Forms.GroupBox();
            this.txtCancellations = new System.Windows.Forms.TextBox();
            this.lblCancellations = new System.Windows.Forms.Label();
            this.txtActivations = new System.Windows.Forms.TextBox();
            this.lblActivations = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblTodate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.cmbSearchDateOption = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.tpSubscriptions = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbBillingFailureInDays = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tcDashboard.SuspendLayout();
            this.tpDashboardView.SuspendLayout();
            this.gbxExpiryStats.SuspendLayout();
            this.gbxKeyStats.SuspendLayout();
            this.gbxTodayStats.SuspendLayout();
            this.gbxActionsAndCancellations.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcDashboard
            // 
            this.tcDashboard.Controls.Add(this.tpDashboardView);
            this.tcDashboard.Controls.Add(this.tpSubscriptions);
            this.tcDashboard.Location = new System.Drawing.Point(1, 1);
            this.tcDashboard.Name = "tcDashboard";
            this.tcDashboard.SelectedIndex = 0;
            this.tcDashboard.Size = new System.Drawing.Size(1242, 600);
            this.tcDashboard.TabIndex = 0;
            // 
            // tpDashboardView
            // 
            this.tpDashboardView.Controls.Add(this.gbxExpiryStats);
            this.tpDashboardView.Controls.Add(this.gbxKeyStats);
            this.tpDashboardView.Controls.Add(this.gbxTodayStats);
            this.tpDashboardView.Location = new System.Drawing.Point(4, 24);
            this.tpDashboardView.Name = "tpDashboardView";
            this.tpDashboardView.Padding = new System.Windows.Forms.Padding(3);
            this.tpDashboardView.Size = new System.Drawing.Size(1234, 572);
            this.tpDashboardView.TabIndex = 0;
            this.tpDashboardView.Text = "Dashboard";
            this.tpDashboardView.UseVisualStyleBackColor = true;
            // 
            // gbxExpiryStats
            // 
            this.gbxExpiryStats.Controls.Add(this.label7);
            this.gbxExpiryStats.Controls.Add(this.label8);
            this.gbxExpiryStats.Controls.Add(this.cmbBillingFailureInDays);
            this.gbxExpiryStats.Controls.Add(this.label9);
            this.gbxExpiryStats.Controls.Add(this.btnPaymentErrorCount);
            this.gbxExpiryStats.Controls.Add(this.btnExpiryingCreditCards);
            this.gbxExpiryStats.Controls.Add(this.label4);
            this.gbxExpiryStats.Controls.Add(this.label5);
            this.gbxExpiryStats.Controls.Add(this.cmbCardExpiryInDays);
            this.gbxExpiryStats.Controls.Add(this.label6);
            this.gbxExpiryStats.Controls.Add(this.btnExpiryingSubscriptions);
            this.gbxExpiryStats.Controls.Add(this.label3);
            this.gbxExpiryStats.Controls.Add(this.label2);
            this.gbxExpiryStats.Controls.Add(this.cmbSubscriptionExpiryInDays);
            this.gbxExpiryStats.Controls.Add(this.label1);
            this.gbxExpiryStats.Location = new System.Drawing.Point(99, 277);
            this.gbxExpiryStats.Name = "gbxExpiryStats";
            this.gbxExpiryStats.Size = new System.Drawing.Size(668, 278);
            this.gbxExpiryStats.TabIndex = 2;
            this.gbxExpiryStats.TabStop = false;
            this.gbxExpiryStats.Text = "Expiry and Payment Failures";
            // 
            // btnPaymentErrorCount
            // 
            this.btnPaymentErrorCount.BackgroundImage = global::Parafait_POS.Properties.Resources.LightBlueBtn648X83_Normal;
            this.btnPaymentErrorCount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPaymentErrorCount.FlatAppearance.BorderSize = 0;
            this.btnPaymentErrorCount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPaymentErrorCount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPaymentErrorCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPaymentErrorCount.Font = new System.Drawing.Font("Arial", 15F);
            this.btnPaymentErrorCount.Location = new System.Drawing.Point(377, 216);
            this.btnPaymentErrorCount.Name = "btnPaymentErrorCount";
            this.btnPaymentErrorCount.Size = new System.Drawing.Size(176, 30);
            this.btnPaymentErrorCount.TabIndex = 41;
            this.btnPaymentErrorCount.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnPaymentErrorCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaymentErrorCount.UseVisualStyleBackColor = true;
            this.btnPaymentErrorCount.Click += new System.EventHandler(this.btnPaymentErrorCount_Click);
            this.btnPaymentErrorCount.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lighBlueBtn_MouseDown);
            this.btnPaymentErrorCount.MouseLeave += new System.EventHandler(this.lightBlueBtn_MouseLeave);
            this.btnPaymentErrorCount.MouseHover += new System.EventHandler(this.lightBlueBtn_MouseHover);
            this.btnPaymentErrorCount.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lightBlueBtn_MouseUp);
            // 
            // btnExpiryingCreditCards
            // 
            this.btnExpiryingCreditCards.BackgroundImage = global::Parafait_POS.Properties.Resources.LightBlueBtn648X83_Normal;
            this.btnExpiryingCreditCards.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExpiryingCreditCards.FlatAppearance.BorderSize = 0;
            this.btnExpiryingCreditCards.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExpiryingCreditCards.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExpiryingCreditCards.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpiryingCreditCards.Font = new System.Drawing.Font("Arial", 15F);
            this.btnExpiryingCreditCards.Location = new System.Drawing.Point(376, 133);
            this.btnExpiryingCreditCards.Name = "btnExpiryingCreditCards";
            this.btnExpiryingCreditCards.Size = new System.Drawing.Size(176, 30);
            this.btnExpiryingCreditCards.TabIndex = 40;
            this.btnExpiryingCreditCards.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExpiryingCreditCards.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnExpiryingCreditCards.UseVisualStyleBackColor = true;
            this.btnExpiryingCreditCards.Click += new System.EventHandler(this.btnExpiryingCreditCards_Click);
            this.btnExpiryingCreditCards.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lighBlueBtn_MouseDown);
            this.btnExpiryingCreditCards.MouseLeave += new System.EventHandler(this.lightBlueBtn_MouseLeave);
            this.btnExpiryingCreditCards.MouseHover += new System.EventHandler(this.lightBlueBtn_MouseHover);
            this.btnExpiryingCreditCards.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lightBlueBtn_MouseUp);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(363, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 24);
            this.label4.TabIndex = 39;
            this.label4.Text = ":";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(287, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 24);
            this.label5.TabIndex = 38;
            this.label5.Text = "Days";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbCardExpiryInDays
            // 
            this.cmbCardExpiryInDays.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCardExpiryInDays.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCardExpiryInDays.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbCardExpiryInDays.FormattingEnabled = true;
            this.cmbCardExpiryInDays.Location = new System.Drawing.Point(285, 133);
            this.cmbCardExpiryInDays.Name = "cmbCardExpiryInDays";
            this.cmbCardExpiryInDays.Size = new System.Drawing.Size(76, 31);
            this.cmbCardExpiryInDays.TabIndex = 37;
            this.cmbCardExpiryInDays.SelectedIndexChanged += new System.EventHandler(this.cmbCardExpiryInDays_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(11, 136);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(271, 24);
            this.label6.TabIndex = 36;
            this.label6.Text = "Credit Cards due for expiry in next ";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnExpiryingSubscriptions
            // 
            this.btnExpiryingSubscriptions.BackgroundImage = global::Parafait_POS.Properties.Resources.LightBlueBtn648X83_Normal;
            this.btnExpiryingSubscriptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExpiryingSubscriptions.FlatAppearance.BorderSize = 0;
            this.btnExpiryingSubscriptions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExpiryingSubscriptions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExpiryingSubscriptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpiryingSubscriptions.Font = new System.Drawing.Font("Arial", 15F);
            this.btnExpiryingSubscriptions.Location = new System.Drawing.Point(376, 50);
            this.btnExpiryingSubscriptions.Name = "btnExpiryingSubscriptions";
            this.btnExpiryingSubscriptions.Size = new System.Drawing.Size(176, 30);
            this.btnExpiryingSubscriptions.TabIndex = 35;
            this.btnExpiryingSubscriptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExpiryingSubscriptions.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnExpiryingSubscriptions.UseVisualStyleBackColor = true;
            this.btnExpiryingSubscriptions.Click += new System.EventHandler(this.btnExpiryingSubscriptions_Click);
            this.btnExpiryingSubscriptions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lighBlueBtn_MouseDown);
            this.btnExpiryingSubscriptions.MouseLeave += new System.EventHandler(this.lightBlueBtn_MouseLeave);
            this.btnExpiryingSubscriptions.MouseHover += new System.EventHandler(this.lightBlueBtn_MouseHover);
            this.btnExpiryingSubscriptions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lightBlueBtn_MouseUp);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(363, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 24);
            this.label3.TabIndex = 34;
            this.label3.Text = ":";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(287, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 24);
            this.label2.TabIndex = 33;
            this.label2.Text = "Days";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbSubscriptionExpiryInDays
            // 
            this.cmbSubscriptionExpiryInDays.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSubscriptionExpiryInDays.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSubscriptionExpiryInDays.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbSubscriptionExpiryInDays.FormattingEnabled = true;
            this.cmbSubscriptionExpiryInDays.Location = new System.Drawing.Point(285, 50);
            this.cmbSubscriptionExpiryInDays.Name = "cmbSubscriptionExpiryInDays";
            this.cmbSubscriptionExpiryInDays.Size = new System.Drawing.Size(76, 31);
            this.cmbSubscriptionExpiryInDays.TabIndex = 32;
            this.cmbSubscriptionExpiryInDays.SelectedIndexChanged += new System.EventHandler(this.cmbSubscriptionExpiryInDays_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 24);
            this.label1.TabIndex = 31;
            this.label1.Text = "Subscriptions due for expiry in next ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxKeyStats
            // 
            this.gbxKeyStats.Controls.Add(this.txtSubscriptionChurnRate);
            this.gbxKeyStats.Controls.Add(this.lblMonthlySubscriptionChurn);
            this.gbxKeyStats.Controls.Add(this.txtTotalActiveSubscriptions);
            this.gbxKeyStats.Controls.Add(this.lblTotalActiveSubscriptions);
            this.gbxKeyStats.Controls.Add(this.txtMonthlyCustomerChurnRate);
            this.gbxKeyStats.Controls.Add(this.lblMonthlyCustomerChurnRate);
            this.gbxKeyStats.Controls.Add(this.txtMonthlyRecurringRevenue);
            this.gbxKeyStats.Controls.Add(this.lblMonthlyRecurringRevenue);
            this.gbxKeyStats.Controls.Add(this.txtTotalActiveCustomers);
            this.gbxKeyStats.Controls.Add(this.lblTotalActiveCustomers);
            this.gbxKeyStats.Location = new System.Drawing.Point(474, 7);
            this.gbxKeyStats.Name = "gbxKeyStats";
            this.gbxKeyStats.Size = new System.Drawing.Size(405, 262);
            this.gbxKeyStats.TabIndex = 1;
            this.gbxKeyStats.TabStop = false;
            this.gbxKeyStats.Text = "Key Stats";
            // 
            // txtSubscriptionChurnRate
            // 
            this.txtSubscriptionChurnRate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubscriptionChurnRate.Location = new System.Drawing.Point(214, 165);
            this.txtSubscriptionChurnRate.MaxLength = 5;
            this.txtSubscriptionChurnRate.Name = "txtSubscriptionChurnRate";
            this.txtSubscriptionChurnRate.ReadOnly = true;
            this.txtSubscriptionChurnRate.Size = new System.Drawing.Size(176, 30);
            this.txtSubscriptionChurnRate.TabIndex = 41;
            // 
            // lblMonthlySubscriptionChurn
            // 
            this.lblMonthlySubscriptionChurn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonthlySubscriptionChurn.Location = new System.Drawing.Point(2, 168);
            this.lblMonthlySubscriptionChurn.Name = "lblMonthlySubscriptionChurn";
            this.lblMonthlySubscriptionChurn.Size = new System.Drawing.Size(207, 24);
            this.lblMonthlySubscriptionChurn.TabIndex = 40;
            this.lblMonthlySubscriptionChurn.Text = "Monthly Subscription Churn Rate:";
            this.lblMonthlySubscriptionChurn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotalActiveSubscriptions
            // 
            this.txtTotalActiveSubscriptions.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalActiveSubscriptions.Location = new System.Drawing.Point(215, 67);
            this.txtTotalActiveSubscriptions.MaxLength = 5;
            this.txtTotalActiveSubscriptions.Name = "txtTotalActiveSubscriptions";
            this.txtTotalActiveSubscriptions.ReadOnly = true;
            this.txtTotalActiveSubscriptions.Size = new System.Drawing.Size(176, 30);
            this.txtTotalActiveSubscriptions.TabIndex = 39;
            // 
            // lblTotalActiveSubscriptions
            // 
            this.lblTotalActiveSubscriptions.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalActiveSubscriptions.Location = new System.Drawing.Point(18, 70);
            this.lblTotalActiveSubscriptions.Name = "lblTotalActiveSubscriptions";
            this.lblTotalActiveSubscriptions.Size = new System.Drawing.Size(192, 24);
            this.lblTotalActiveSubscriptions.TabIndex = 38;
            this.lblTotalActiveSubscriptions.Text = "Total Active Subscriptions:";
            this.lblTotalActiveSubscriptions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMonthlyCustomerChurnRate
            // 
            this.txtMonthlyCustomerChurnRate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMonthlyCustomerChurnRate.Location = new System.Drawing.Point(215, 116);
            this.txtMonthlyCustomerChurnRate.MaxLength = 5;
            this.txtMonthlyCustomerChurnRate.Name = "txtMonthlyCustomerChurnRate";
            this.txtMonthlyCustomerChurnRate.ReadOnly = true;
            this.txtMonthlyCustomerChurnRate.Size = new System.Drawing.Size(176, 30);
            this.txtMonthlyCustomerChurnRate.TabIndex = 37;
            // 
            // lblMonthlyCustomerChurnRate
            // 
            this.lblMonthlyCustomerChurnRate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonthlyCustomerChurnRate.Location = new System.Drawing.Point(18, 119);
            this.lblMonthlyCustomerChurnRate.Name = "lblMonthlyCustomerChurnRate";
            this.lblMonthlyCustomerChurnRate.Size = new System.Drawing.Size(192, 24);
            this.lblMonthlyCustomerChurnRate.TabIndex = 36;
            this.lblMonthlyCustomerChurnRate.Text = "Monthly Customer Churn Rate:";
            this.lblMonthlyCustomerChurnRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMonthlyRecurringRevenue
            // 
            this.txtMonthlyRecurringRevenue.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMonthlyRecurringRevenue.Location = new System.Drawing.Point(215, 214);
            this.txtMonthlyRecurringRevenue.MaxLength = 5;
            this.txtMonthlyRecurringRevenue.Name = "txtMonthlyRecurringRevenue";
            this.txtMonthlyRecurringRevenue.ReadOnly = true;
            this.txtMonthlyRecurringRevenue.Size = new System.Drawing.Size(176, 30);
            this.txtMonthlyRecurringRevenue.TabIndex = 35;
            // 
            // lblMonthlyRecurringRevenue
            // 
            this.lblMonthlyRecurringRevenue.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonthlyRecurringRevenue.Location = new System.Drawing.Point(18, 217);
            this.lblMonthlyRecurringRevenue.Name = "lblMonthlyRecurringRevenue";
            this.lblMonthlyRecurringRevenue.Size = new System.Drawing.Size(192, 24);
            this.lblMonthlyRecurringRevenue.TabIndex = 34;
            this.lblMonthlyRecurringRevenue.Text = "Monthly Recurring Revenue:";
            this.lblMonthlyRecurringRevenue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotalActiveCustomers
            // 
            this.txtTotalActiveCustomers.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalActiveCustomers.Location = new System.Drawing.Point(215, 18);
            this.txtTotalActiveCustomers.MaxLength = 5;
            this.txtTotalActiveCustomers.Name = "txtTotalActiveCustomers";
            this.txtTotalActiveCustomers.ReadOnly = true;
            this.txtTotalActiveCustomers.Size = new System.Drawing.Size(176, 30);
            this.txtTotalActiveCustomers.TabIndex = 33;
            // 
            // lblTotalActiveCustomers
            // 
            this.lblTotalActiveCustomers.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalActiveCustomers.Location = new System.Drawing.Point(18, 21);
            this.lblTotalActiveCustomers.Name = "lblTotalActiveCustomers";
            this.lblTotalActiveCustomers.Size = new System.Drawing.Size(192, 24);
            this.lblTotalActiveCustomers.TabIndex = 32;
            this.lblTotalActiveCustomers.Text = "Total Active Customers:";
            this.lblTotalActiveCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxTodayStats
            // 
            this.gbxTodayStats.Controls.Add(this.gbxActionsAndCancellations);
            this.gbxTodayStats.Controls.Add(this.btnSearch);
            this.gbxTodayStats.Controls.Add(this.lblTodate);
            this.gbxTodayStats.Controls.Add(this.dtpToDate);
            this.gbxTodayStats.Controls.Add(this.lblFromDate);
            this.gbxTodayStats.Controls.Add(this.dtpFromDate);
            this.gbxTodayStats.Controls.Add(this.cmbSearchDateOption);
            this.gbxTodayStats.Location = new System.Drawing.Point(7, 7);
            this.gbxTodayStats.Name = "gbxTodayStats";
            this.gbxTodayStats.Size = new System.Drawing.Size(462, 262);
            this.gbxTodayStats.TabIndex = 0;
            this.gbxTodayStats.TabStop = false;
            this.gbxTodayStats.Text = "Period";
            // 
            // gbxActionsAndCancellations
            // 
            this.gbxActionsAndCancellations.Controls.Add(this.txtCancellations);
            this.gbxActionsAndCancellations.Controls.Add(this.lblCancellations);
            this.gbxActionsAndCancellations.Controls.Add(this.txtActivations);
            this.gbxActionsAndCancellations.Controls.Add(this.lblActivations);
            this.gbxActionsAndCancellations.Location = new System.Drawing.Point(12, 126);
            this.gbxActionsAndCancellations.Name = "gbxActionsAndCancellations";
            this.gbxActionsAndCancellations.Size = new System.Drawing.Size(435, 130);
            this.gbxActionsAndCancellations.TabIndex = 48;
            this.gbxActionsAndCancellations.TabStop = false;
            this.gbxActionsAndCancellations.Text = "Activations and Cancellations";
            // 
            // txtCancellations
            // 
            this.txtCancellations.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCancellations.Location = new System.Drawing.Point(233, 63);
            this.txtCancellations.MaxLength = 5;
            this.txtCancellations.Name = "txtCancellations";
            this.txtCancellations.ReadOnly = true;
            this.txtCancellations.Size = new System.Drawing.Size(176, 30);
            this.txtCancellations.TabIndex = 33;
            // 
            // lblCancellations
            // 
            this.lblCancellations.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCancellations.Location = new System.Drawing.Point(256, 34);
            this.lblCancellations.Name = "lblCancellations";
            this.lblCancellations.Size = new System.Drawing.Size(131, 24);
            this.lblCancellations.TabIndex = 32;
            this.lblCancellations.Text = "Cancellations";
            this.lblCancellations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtActivations
            // 
            this.txtActivations.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtActivations.Location = new System.Drawing.Point(14, 63);
            this.txtActivations.MaxLength = 5;
            this.txtActivations.Name = "txtActivations";
            this.txtActivations.ReadOnly = true;
            this.txtActivations.Size = new System.Drawing.Size(176, 30);
            this.txtActivations.TabIndex = 31;
            // 
            // lblActivations
            // 
            this.lblActivations.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActivations.Location = new System.Drawing.Point(37, 34);
            this.lblActivations.Name = "lblActivations";
            this.lblActivations.Size = new System.Drawing.Size(131, 24);
            this.lblActivations.TabIndex = 30;
            this.lblActivations.Text = "Activations";
            this.lblActivations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(247, 89);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(95, 30);
            this.btnSearch.TabIndex = 47;
            this.btnSearch.Text = "View";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnSearch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnSearch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // lblTodate
            // 
            this.lblTodate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblTodate.Location = new System.Drawing.Point(247, 11);
            this.lblTodate.Name = "lblTodate";
            this.lblTodate.Size = new System.Drawing.Size(137, 28);
            this.lblTodate.TabIndex = 9;
            this.lblTodate.Text = "To Date";
            this.lblTodate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd-MM-yyyy";
            this.dtpToDate.Font = new System.Drawing.Font("Arial", 15F);
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(247, 42);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(200, 30);
            this.dtpToDate.TabIndex = 8;
            // 
            // lblFromDate
            // 
            this.lblFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblFromDate.Location = new System.Drawing.Point(12, 11);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(137, 28);
            this.lblFromDate.TabIndex = 7;
            this.lblFromDate.Text = "From Date";
            this.lblFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd-MM-yyyy";
            this.dtpFromDate.Font = new System.Drawing.Font("Arial", 15F);
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(12, 42);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(200, 30);
            this.dtpFromDate.TabIndex = 1;
            // 
            // cmbSearchDateOption
            // 
            this.cmbSearchDateOption.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSearchDateOption.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSearchDateOption.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbSearchDateOption.FormattingEnabled = true;
            this.cmbSearchDateOption.Location = new System.Drawing.Point(12, 88);
            this.cmbSearchDateOption.Name = "cmbSearchDateOption";
            this.cmbSearchDateOption.Size = new System.Drawing.Size(200, 31);
            this.cmbSearchDateOption.TabIndex = 0;
            this.cmbSearchDateOption.SelectedIndexChanged += new System.EventHandler(this.cmbSearchDateOption_SelectedIndexChanged);
            // 
            // tpSubscriptions
            // 
            this.tpSubscriptions.Location = new System.Drawing.Point(4, 24);
            this.tpSubscriptions.Name = "tpSubscriptions";
            this.tpSubscriptions.Padding = new System.Windows.Forms.Padding(3);
            this.tpSubscriptions.Size = new System.Drawing.Size(1234, 572);
            this.tpSubscriptions.TabIndex = 1;
            this.tpSubscriptions.Text = "Subscriptions";
            this.tpSubscriptions.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(363, 219);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 24);
            this.label7.TabIndex = 45;
            this.label7.Text = ":";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(287, 189);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 24);
            this.label8.TabIndex = 44;
            this.label8.Text = "Days";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbBillingFailureInDays
            // 
            this.cmbBillingFailureInDays.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBillingFailureInDays.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBillingFailureInDays.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbBillingFailureInDays.FormattingEnabled = true;
            this.cmbBillingFailureInDays.Location = new System.Drawing.Point(285, 216);
            this.cmbBillingFailureInDays.Name = "cmbBillingFailureInDays";
            this.cmbBillingFailureInDays.Size = new System.Drawing.Size(76, 31);
            this.cmbBillingFailureInDays.TabIndex = 43;
            this.cmbBillingFailureInDays.SelectedIndexChanged += new System.EventHandler(this.cmbBillingFailureInDays_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(11, 219);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(271, 24);
            this.label9.TabIndex = 42;
            this.label9.Text = "Subscriptions with billing failures for last ";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmSubscriptionStaffView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 600);
            this.Controls.Add(this.tcDashboard);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSubscriptionStaffView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Subscription Dashboard";
            this.Load += new System.EventHandler(this.frmSubscriptionStaffView_Load);
            this.tcDashboard.ResumeLayout(false);
            this.tpDashboardView.ResumeLayout(false);
            this.gbxExpiryStats.ResumeLayout(false);
            this.gbxKeyStats.ResumeLayout(false);
            this.gbxKeyStats.PerformLayout();
            this.gbxTodayStats.ResumeLayout(false);
            this.gbxActionsAndCancellations.ResumeLayout(false);
            this.gbxActionsAndCancellations.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcDashboard;
        private System.Windows.Forms.TabPage tpDashboardView;
        private System.Windows.Forms.TabPage tpSubscriptions;
        private System.Windows.Forms.GroupBox gbxKeyStats;
        private System.Windows.Forms.GroupBox gbxTodayStats;
        private System.Windows.Forms.GroupBox gbxExpiryStats;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSearchDateOption;
        private System.Windows.Forms.Label lblTodate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.GroupBox gbxActionsAndCancellations;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtCancellations;
        private System.Windows.Forms.Label lblCancellations;
        private System.Windows.Forms.TextBox txtActivations;
        private System.Windows.Forms.Label lblActivations;
        private System.Windows.Forms.TextBox txtMonthlyCustomerChurnRate;
        private System.Windows.Forms.Label lblMonthlyCustomerChurnRate;
        private System.Windows.Forms.TextBox txtMonthlyRecurringRevenue;
        private System.Windows.Forms.Label lblMonthlyRecurringRevenue;
        private System.Windows.Forms.TextBox txtTotalActiveCustomers;
        private System.Windows.Forms.Label lblTotalActiveCustomers;
        private System.Windows.Forms.Button btnExpiryingSubscriptions;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSubscriptionExpiryInDays;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExpiryingCreditCards;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbCardExpiryInDays;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnPaymentErrorCount;
        private System.Windows.Forms.TextBox txtTotalActiveSubscriptions;
        private System.Windows.Forms.Label lblTotalActiveSubscriptions;
        private System.Windows.Forms.TextBox txtSubscriptionChurnRate;
        private System.Windows.Forms.Label lblMonthlySubscriptionChurn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbBillingFailureInDays;
        private System.Windows.Forms.Label label9;
    }
}