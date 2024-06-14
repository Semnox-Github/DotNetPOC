namespace Parafait_POS.Subscription
{
    partial class usrCtlSubscriptions
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(usrCtlSubscriptions));
            this.pnlPage = new System.Windows.Forms.Panel();
            this.dgvSubscriptionHeader = new System.Windows.Forms.DataGridView();
            this.dgvColumnViewDetails = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvColumnCustomerId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.SubscriptionNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastBillOnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextBillOnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubscriptionExpiryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastPaymentStatus = new System.Windows.Forms.DataGridViewImageColumn();
            this.CreditCardStatus = new System.Windows.Forms.DataGridViewImageColumn();
            this.productSubscriptionNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColumnSubscriptionHeaderId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollDGVSubscriptionHeader = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.hScrollDGVSubscriptionHeader = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.gbxSearchOptions = new System.Windows.Forms.GroupBox();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.btnCustomerLookup = new System.Windows.Forms.Button();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbxCreditCardExpired = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cbxHasPaymentFailure = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cbxPaymentRetryLimitCrossed = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cbxCardExpiresBeforeNextBilling = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cmbSubscriptionStatus = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCustFirstName = new System.Windows.Forms.TextBox();
            this.lblCustomerFirstName = new System.Windows.Forms.Label();
            this.cmbSubscriptionProducts = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblSubscriptionProduct = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubscriptionHeader)).BeginInit();
            this.gbxSearchOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPage
            // 
            this.pnlPage.BackColor = System.Drawing.Color.White;
            this.pnlPage.Controls.Add(this.dgvSubscriptionHeader);
            this.pnlPage.Controls.Add(this.vScrollDGVSubscriptionHeader);
            this.pnlPage.Controls.Add(this.hScrollDGVSubscriptionHeader);
            this.pnlPage.Controls.Add(this.gbxSearchOptions);
            this.pnlPage.Location = new System.Drawing.Point(0, 0);
            this.pnlPage.Name = "pnlPage";
            this.pnlPage.Size = new System.Drawing.Size(1210, 557);
            this.pnlPage.TabIndex = 0;
            // 
            // dgvSubscriptionHeader
            // 
            this.dgvSubscriptionHeader.AllowUserToAddRows = false;
            this.dgvSubscriptionHeader.AllowUserToDeleteRows = false;
            this.dgvSubscriptionHeader.AllowUserToResizeColumns = false;
            this.dgvSubscriptionHeader.AllowUserToResizeRows = false;
            this.dgvSubscriptionHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubscriptionHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvColumnViewDetails,
            this.dgvColumnCustomerId,
            this.SubscriptionNumber,
            this.LastBillOnDate,
            this.NextBillOnDate,
            this.SubscriptionExpiryDate,
            this.statusDataGridViewTextBoxColumn,
            this.LastPaymentStatus,
            this.CreditCardStatus,
            this.productSubscriptionNameDataGridViewTextBoxColumn,
            this.dgvColumnSubscriptionHeaderId});
            this.dgvSubscriptionHeader.Location = new System.Drawing.Point(3, 119);
            this.dgvSubscriptionHeader.MultiSelect = false;
            this.dgvSubscriptionHeader.Name = "dgvSubscriptionHeader";
            this.dgvSubscriptionHeader.ReadOnly = true;
            this.dgvSubscriptionHeader.RowHeadersVisible = false;
            this.dgvSubscriptionHeader.RowTemplate.Height = 30;
            this.dgvSubscriptionHeader.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvSubscriptionHeader.Size = new System.Drawing.Size(1158, 386);
            this.dgvSubscriptionHeader.TabIndex = 6;
            this.dgvSubscriptionHeader.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSubscriptionHeader_CellClick);
            // 
            // dgvColumnViewDetails
            // 
            this.dgvColumnViewDetails.HeaderText = "...";
            this.dgvColumnViewDetails.MinimumWidth = 50;
            this.dgvColumnViewDetails.Name = "dgvColumnViewDetails";
            this.dgvColumnViewDetails.ReadOnly = true;
            this.dgvColumnViewDetails.Text = "...";
            this.dgvColumnViewDetails.UseColumnTextForButtonValue = true;
            this.dgvColumnViewDetails.Width = 50;
            // 
            // dgvColumnCustomerId
            // 
            this.dgvColumnCustomerId.HeaderText = "Customer";
            this.dgvColumnCustomerId.MinimumWidth = 200;
            this.dgvColumnCustomerId.Name = "dgvColumnCustomerId";
            this.dgvColumnCustomerId.ReadOnly = true;
            this.dgvColumnCustomerId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvColumnCustomerId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvColumnCustomerId.Width = 200;
            // 
            // SubscriptionNumber
            // 
            this.SubscriptionNumber.HeaderText = "Subscription No";
            this.SubscriptionNumber.Name = "SubscriptionNumber";
            this.SubscriptionNumber.ReadOnly = true;
            // 
            // LastBillOnDate
            // 
            this.LastBillOnDate.HeaderText = "Last Bill On Date";
            this.LastBillOnDate.MinimumWidth = 120;
            this.LastBillOnDate.Name = "LastBillOnDate";
            this.LastBillOnDate.ReadOnly = true;
            this.LastBillOnDate.Width = 120;
            // 
            // NextBillOnDate
            // 
            this.NextBillOnDate.HeaderText = "Next Bill On Date";
            this.NextBillOnDate.MinimumWidth = 120;
            this.NextBillOnDate.Name = "NextBillOnDate";
            this.NextBillOnDate.ReadOnly = true;
            this.NextBillOnDate.Width = 120;
            // 
            // SubscriptionExpiryDate
            // 
            this.SubscriptionExpiryDate.HeaderText = "Expiry Date";
            this.SubscriptionExpiryDate.MinimumWidth = 120;
            this.SubscriptionExpiryDate.Name = "SubscriptionExpiryDate";
            this.SubscriptionExpiryDate.ReadOnly = true;
            this.SubscriptionExpiryDate.Width = 120;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.HeaderText = "Subscription Status";
            this.statusDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // LastPaymentStatus
            // 
            this.LastPaymentStatus.HeaderText = "Last Payment Status";
            this.LastPaymentStatus.MinimumWidth = 90;
            this.LastPaymentStatus.Name = "LastPaymentStatus";
            this.LastPaymentStatus.ReadOnly = true;
            this.LastPaymentStatus.Width = 90;
            // 
            // CreditCardStatus
            // 
            this.CreditCardStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CreditCardStatus.HeaderText = "Credit Card Status";
            this.CreditCardStatus.MinimumWidth = 100;
            this.CreditCardStatus.Name = "CreditCardStatus";
            this.CreditCardStatus.ReadOnly = true;
            // 
            // productSubscriptionNameDataGridViewTextBoxColumn
            // 
            this.productSubscriptionNameDataGridViewTextBoxColumn.HeaderText = "Subscription Name";
            this.productSubscriptionNameDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.productSubscriptionNameDataGridViewTextBoxColumn.Name = "productSubscriptionNameDataGridViewTextBoxColumn";
            this.productSubscriptionNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.productSubscriptionNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // dgvColumnSubscriptionHeaderId
            // 
            this.dgvColumnSubscriptionHeaderId.HeaderText = "Subscription Header Id";
            this.dgvColumnSubscriptionHeaderId.Name = "dgvColumnSubscriptionHeaderId";
            this.dgvColumnSubscriptionHeaderId.ReadOnly = true;
            this.dgvColumnSubscriptionHeaderId.Visible = false;
            // 
            // vScrollDGVSubscriptionHeader
            // 
            this.vScrollDGVSubscriptionHeader.AutoHide = false;
            this.vScrollDGVSubscriptionHeader.DataGridView = this.dgvSubscriptionHeader;
            this.vScrollDGVSubscriptionHeader.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollDGVSubscriptionHeader.DownButtonBackgroundImage")));
            this.vScrollDGVSubscriptionHeader.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollDGVSubscriptionHeader.DownButtonDisabledBackgroundImage")));
            this.vScrollDGVSubscriptionHeader.Location = new System.Drawing.Point(1163, 119);
            this.vScrollDGVSubscriptionHeader.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollDGVSubscriptionHeader.Name = "vScrollDGVSubscriptionHeader";
            this.vScrollDGVSubscriptionHeader.ScrollableControl = null;
            this.vScrollDGVSubscriptionHeader.ScrollViewer = null;
            this.vScrollDGVSubscriptionHeader.Size = new System.Drawing.Size(47, 386);
            this.vScrollDGVSubscriptionHeader.TabIndex = 5;
            this.vScrollDGVSubscriptionHeader.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollDGVSubscriptionHeader.UpButtonBackgroundImage")));
            this.vScrollDGVSubscriptionHeader.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollDGVSubscriptionHeader.UpButtonDisabledBackgroundImage")));
            // 
            // hScrollDGVSubscriptionHeader
            // 
            this.hScrollDGVSubscriptionHeader.AutoHide = false;
            this.hScrollDGVSubscriptionHeader.DataGridView = this.dgvSubscriptionHeader;
            this.hScrollDGVSubscriptionHeader.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollDGVSubscriptionHeader.LeftButtonBackgroundImage")));
            this.hScrollDGVSubscriptionHeader.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollDGVSubscriptionHeader.LeftButtonDisabledBackgroundImage")));
            this.hScrollDGVSubscriptionHeader.Location = new System.Drawing.Point(3, 508);
            this.hScrollDGVSubscriptionHeader.Margin = new System.Windows.Forms.Padding(0);
            this.hScrollDGVSubscriptionHeader.Name = "hScrollDGVSubscriptionHeader";
            this.hScrollDGVSubscriptionHeader.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollDGVSubscriptionHeader.RightButtonBackgroundImage")));
            this.hScrollDGVSubscriptionHeader.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollDGVSubscriptionHeader.RightButtonDisabledBackgroundImage")));
            this.hScrollDGVSubscriptionHeader.ScrollableControl = null;
            this.hScrollDGVSubscriptionHeader.ScrollViewer = null;
            this.hScrollDGVSubscriptionHeader.Size = new System.Drawing.Size(1158, 46);
            this.hScrollDGVSubscriptionHeader.TabIndex = 4;
            // 
            // gbxSearchOptions
            // 
            this.gbxSearchOptions.Controls.Add(this.btnShowKeyPad);
            this.gbxSearchOptions.Controls.Add(this.btnCustomerLookup);
            this.gbxSearchOptions.Controls.Add(this.btnClearSearch);
            this.gbxSearchOptions.Controls.Add(this.btnSearch);
            this.gbxSearchOptions.Controls.Add(this.cbxCreditCardExpired);
            this.gbxSearchOptions.Controls.Add(this.cbxHasPaymentFailure);
            this.gbxSearchOptions.Controls.Add(this.cbxPaymentRetryLimitCrossed);
            this.gbxSearchOptions.Controls.Add(this.cbxCardExpiresBeforeNextBilling);
            this.gbxSearchOptions.Controls.Add(this.cmbSubscriptionStatus);
            this.gbxSearchOptions.Controls.Add(this.label1);
            this.gbxSearchOptions.Controls.Add(this.txtCustFirstName);
            this.gbxSearchOptions.Controls.Add(this.lblCustomerFirstName);
            this.gbxSearchOptions.Controls.Add(this.cmbSubscriptionProducts);
            this.gbxSearchOptions.Controls.Add(this.lblSubscriptionProduct);
            this.gbxSearchOptions.Location = new System.Drawing.Point(3, 3);
            this.gbxSearchOptions.Name = "gbxSearchOptions";
            this.gbxSearchOptions.Size = new System.Drawing.Size(1207, 113);
            this.gbxSearchOptions.TabIndex = 1;
            this.gbxSearchOptions.TabStop = false;
            this.gbxSearchOptions.Text = "Search";
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnShowKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Location = new System.Drawing.Point(1140, 54);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(42, 47);
            this.btnShowKeyPad.TabIndex = 34;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            // 
            // btnCustomerLookup
            // 
            this.btnCustomerLookup.BackColor = System.Drawing.Color.Transparent;
            this.btnCustomerLookup.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCustomerLookup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCustomerLookup.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCustomerLookup.FlatAppearance.BorderSize = 0;
            this.btnCustomerLookup.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCustomerLookup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCustomerLookup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCustomerLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerLookup.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnCustomerLookup.ForeColor = System.Drawing.Color.White;
            this.btnCustomerLookup.Location = new System.Drawing.Point(348, 65);
            this.btnCustomerLookup.Name = "btnCustomerLookup";
            this.btnCustomerLookup.Size = new System.Drawing.Size(82, 34);
            this.btnCustomerLookup.TabIndex = 33;
            this.btnCustomerLookup.Text = "LookUp";
            this.btnCustomerLookup.UseVisualStyleBackColor = false;
            this.btnCustomerLookup.Click += new System.EventHandler(this.btnCustomerLookup_Click);
            this.btnCustomerLookup.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnCustomerLookup.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClearSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClearSearch.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClearSearch.FlatAppearance.BorderSize = 0;
            this.btnClearSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClearSearch.ForeColor = System.Drawing.Color.White;
            this.btnClearSearch.Location = new System.Drawing.Point(1033, 65);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(82, 34);
            this.btnClearSearch.TabIndex = 32;
            this.btnClearSearch.Text = "Clear";
            this.btnClearSearch.UseVisualStyleBackColor = false;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            this.btnClearSearch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnClearSearch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
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
            this.btnSearch.Location = new System.Drawing.Point(926, 65);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(82, 34);
            this.btnSearch.TabIndex = 31;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnSearch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnSearch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // cbxCreditCardExpired
            // 
            this.cbxCreditCardExpired.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxCreditCardExpired.AutoSize = true;
            this.cbxCreditCardExpired.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxCreditCardExpired.FlatAppearance.BorderSize = 0;
            this.cbxCreditCardExpired.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxCreditCardExpired.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cbxCreditCardExpired.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxCreditCardExpired.ImageIndex = 1;
            this.cbxCreditCardExpired.Location = new System.Drawing.Point(683, 69);
            this.cbxCreditCardExpired.Name = "cbxCreditCardExpired";
            this.cbxCreditCardExpired.Size = new System.Drawing.Size(117, 26);
            this.cbxCreditCardExpired.TabIndex = 30;
            this.cbxCreditCardExpired.Text = "Card Expired?";
            this.cbxCreditCardExpired.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxCreditCardExpired.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cbxCreditCardExpired.UseVisualStyleBackColor = true;
            // 
            // cbxHasPaymentFailure
            // 
            this.cbxHasPaymentFailure.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxHasPaymentFailure.AutoSize = true;
            this.cbxHasPaymentFailure.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxHasPaymentFailure.FlatAppearance.BorderSize = 0;
            this.cbxHasPaymentFailure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxHasPaymentFailure.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cbxHasPaymentFailure.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxHasPaymentFailure.ImageIndex = 1;
            this.cbxHasPaymentFailure.Location = new System.Drawing.Point(683, 22);
            this.cbxHasPaymentFailure.Name = "cbxHasPaymentFailure";
            this.cbxHasPaymentFailure.Size = new System.Drawing.Size(160, 26);
            this.cbxHasPaymentFailure.TabIndex = 29;
            this.cbxHasPaymentFailure.Text = "Has Payment Failure?";
            this.cbxHasPaymentFailure.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxHasPaymentFailure.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cbxHasPaymentFailure.UseVisualStyleBackColor = true;
            // 
            // cbxPaymentRetryLimitCrossed
            // 
            this.cbxPaymentRetryLimitCrossed.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxPaymentRetryLimitCrossed.AutoSize = true;
            this.cbxPaymentRetryLimitCrossed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxPaymentRetryLimitCrossed.FlatAppearance.BorderSize = 0;
            this.cbxPaymentRetryLimitCrossed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxPaymentRetryLimitCrossed.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cbxPaymentRetryLimitCrossed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxPaymentRetryLimitCrossed.ImageIndex = 1;
            this.cbxPaymentRetryLimitCrossed.Location = new System.Drawing.Point(447, 22);
            this.cbxPaymentRetryLimitCrossed.Name = "cbxPaymentRetryLimitCrossed";
            this.cbxPaymentRetryLimitCrossed.Size = new System.Drawing.Size(209, 26);
            this.cbxPaymentRetryLimitCrossed.TabIndex = 28;
            this.cbxPaymentRetryLimitCrossed.Text = "Payment Retry Limit Crossed?";
            this.cbxPaymentRetryLimitCrossed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxPaymentRetryLimitCrossed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cbxPaymentRetryLimitCrossed.UseVisualStyleBackColor = true;
            // 
            // cbxCardExpiresBeforeNextBilling
            // 
            this.cbxCardExpiresBeforeNextBilling.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxCardExpiresBeforeNextBilling.AutoSize = true;
            this.cbxCardExpiresBeforeNextBilling.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxCardExpiresBeforeNextBilling.FlatAppearance.BorderSize = 0;
            this.cbxCardExpiresBeforeNextBilling.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxCardExpiresBeforeNextBilling.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cbxCardExpiresBeforeNextBilling.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxCardExpiresBeforeNextBilling.ImageIndex = 1;
            this.cbxCardExpiresBeforeNextBilling.Location = new System.Drawing.Point(447, 69);
            this.cbxCardExpiresBeforeNextBilling.Name = "cbxCardExpiresBeforeNextBilling";
            this.cbxCardExpiresBeforeNextBilling.Size = new System.Drawing.Size(224, 26);
            this.cbxCardExpiresBeforeNextBilling.TabIndex = 27;
            this.cbxCardExpiresBeforeNextBilling.Text = "Card Expires Before Next Billing?";
            this.cbxCardExpiresBeforeNextBilling.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxCardExpiresBeforeNextBilling.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cbxCardExpiresBeforeNextBilling.UseVisualStyleBackColor = true;
            // 
            // cmbSubscriptionStatus
            // 
            this.cmbSubscriptionStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSubscriptionStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSubscriptionStatus.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbSubscriptionStatus.FormattingEnabled = true;
            this.cmbSubscriptionStatus.Location = new System.Drawing.Point(927, 20);
            this.cmbSubscriptionStatus.Name = "cmbSubscriptionStatus";
            this.cmbSubscriptionStatus.Size = new System.Drawing.Size(263, 31);
            this.cmbSubscriptionStatus.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(823, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 28);
            this.label1.TabIndex = 25;
            this.label1.Text = "Status: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCustFirstName
            // 
            this.txtCustFirstName.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustFirstName.Location = new System.Drawing.Point(166, 67);
            this.txtCustFirstName.MaxLength = 5;
            this.txtCustFirstName.Name = "txtCustFirstName";
            this.txtCustFirstName.Size = new System.Drawing.Size(165, 30);
            this.txtCustFirstName.TabIndex = 24;
            // 
            // lblCustomerFirstName
            // 
            this.lblCustomerFirstName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerFirstName.Location = new System.Drawing.Point(7, 68);
            this.lblCustomerFirstName.Name = "lblCustomerFirstName";
            this.lblCustomerFirstName.Size = new System.Drawing.Size(163, 28);
            this.lblCustomerFirstName.TabIndex = 23;
            this.lblCustomerFirstName.Text = "First Name: ";
            this.lblCustomerFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbSubscriptionProducts
            // 
            this.cmbSubscriptionProducts.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSubscriptionProducts.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSubscriptionProducts.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbSubscriptionProducts.FormattingEnabled = true;
            this.cmbSubscriptionProducts.Location = new System.Drawing.Point(166, 20);
            this.cmbSubscriptionProducts.Name = "cmbSubscriptionProducts";
            this.cmbSubscriptionProducts.Size = new System.Drawing.Size(263, 31);
            this.cmbSubscriptionProducts.TabIndex = 22;
            // 
            // lblSubscriptionProduct
            // 
            this.lblSubscriptionProduct.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblSubscriptionProduct.Location = new System.Drawing.Point(7, 21);
            this.lblSubscriptionProduct.Name = "lblSubscriptionProduct";
            this.lblSubscriptionProduct.Size = new System.Drawing.Size(163, 28);
            this.lblSubscriptionProduct.TabIndex = 6;
            this.lblSubscriptionProduct.Text = "Subscription Product: ";
            this.lblSubscriptionProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Subscription Name";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 200;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Last Bill On Date";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 130;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 130;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Next Bill On Date";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 130;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 130;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Expiry Date";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 130;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 130;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Subscription Status";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "SubscriptionHeaderId";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // usrCtlSubscriptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlPage);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Name = "usrCtlSubscriptions";
            this.Size = new System.Drawing.Size(1215, 593);
            this.pnlPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubscriptionHeader)).EndInit();
            this.gbxSearchOptions.ResumeLayout(false);
            this.gbxSearchOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxSearchOptions;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Button btnCustomerLookup;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.Button btnSearch;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxCreditCardExpired;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxHasPaymentFailure;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxPaymentRetryLimitCrossed;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxCardExpiresBeforeNextBilling;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSubscriptionStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCustFirstName;
        private System.Windows.Forms.Label lblCustomerFirstName;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSubscriptionProducts;
        private System.Windows.Forms.Label lblSubscriptionProduct;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView hScrollDGVSubscriptionHeader;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vScrollDGVSubscriptionHeader;
        private System.Windows.Forms.DataGridView dgvSubscriptionHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.Panel pnlPage;
        private System.Windows.Forms.DataGridViewButtonColumn dgvColumnViewDetails;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvColumnCustomerId;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastBillOnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn NextBillOnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubscriptionExpiryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn LastPaymentStatus;
        private System.Windows.Forms.DataGridViewImageColumn CreditCardStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn productSubscriptionNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColumnSubscriptionHeaderId;
        //private System.Windows.Forms.DataGridViewTextBoxColumn transactionIdDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn transactionLineIdDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn customerContactIdDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn customerCreditCardsIdDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn productsIdDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn productSubscriptionIdDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn productSubscriptionDescriptionDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionPriceDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn taxInclusivePriceDataGridViewCheckBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionCycleDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn unitOfSubscriptionCycleDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionCycleValidityDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn seasonalSubscriptionDataGridViewCheckBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn seasonStartDateDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn seasonEndDateDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn freeTrialPeriodInDaysDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn billInAdvanceDataGridViewCheckBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn subscriptionPaymentCollectionModeDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn selectedPaymentCollectionModeDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn autoRenewDataGridViewCheckBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn autoRenewalMarkupPercentDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn renewalGracePeriodInDaysDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn noOfRenewalRemindersDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn reminderFrequencyInDaysDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn sendFirstReminderBeforeXDaysDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn lastRenewalReminderSentOnDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn renewalReminderCountDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn lastPaymentRetryLimitReminderSentOnDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn paymentRetryLimitReminderCountDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn allowPauseDataGridViewCheckBoxColumn;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn siteIdDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedDataGridViewCheckBoxColumn;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedRecursiveDataGridViewCheckBoxColumn;
    }
}
