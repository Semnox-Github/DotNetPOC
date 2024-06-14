namespace Parafait_POS.Subscription
{
    partial class frmCustomerSubscription
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomerSubscription));
            this.tabCustomerCards = new System.Windows.Forms.TabPage();
            this.dgvCreditCards = new System.Windows.Forms.DataGridView();
            this.btnAddCustomerCreditCard = new System.Windows.Forms.Button();
            this.tabSubscriptions = new System.Windows.Forms.TabPage();
            this.tabCtrlSubscriptions = new System.Windows.Forms.TabControl();
            this.vScrollCreditCards = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.hScrollCreditCards = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.customerCreditCardsIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardProfileIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColumnPaymentModeId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tokenIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameOnCardDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creditCardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardExpiryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastCreditCardExpiryReminderSentOnDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creditCardExpiryReminderCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCardColumnIsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.createdByDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.guidDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isChangedDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.customerCreditCardsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabCustomerCards.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCreditCards)).BeginInit();
            this.tabCtrlSubscriptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customerCreditCardsDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tabCustomerCards
            // 
            this.tabCustomerCards.Controls.Add(this.vScrollCreditCards);
            this.tabCustomerCards.Controls.Add(this.hScrollCreditCards);
            this.tabCustomerCards.Controls.Add(this.dgvCreditCards);
            this.tabCustomerCards.Controls.Add(this.btnAddCustomerCreditCard);
            this.tabCustomerCards.Location = new System.Drawing.Point(4, 24);
            this.tabCustomerCards.Name = "tabCustomerCards";
            this.tabCustomerCards.Padding = new System.Windows.Forms.Padding(3);
            this.tabCustomerCards.Size = new System.Drawing.Size(1270, 611);
            this.tabCustomerCards.TabIndex = 1;
            this.tabCustomerCards.Text = "Customer Cards";
            this.tabCustomerCards.UseVisualStyleBackColor = true;
            // 
            // dgvCreditCards
            // 
            this.dgvCreditCards.AllowUserToAddRows = false;
            this.dgvCreditCards.AllowUserToDeleteRows = false;
            this.dgvCreditCards.AutoGenerateColumns = false;
            this.dgvCreditCards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCreditCards.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.customerCreditCardsIdDataGridViewTextBoxColumn1,
            this.customerIdDataGridViewTextBoxColumn1,
            this.cardProfileIdDataGridViewTextBoxColumn,
            this.dgvColumnPaymentModeId,
            this.tokenIdDataGridViewTextBoxColumn,
            this.customerNameOnCardDataGridViewTextBoxColumn,
            this.creditCardNumberDataGridViewTextBoxColumn,
            this.cardExpiryDataGridViewTextBoxColumn,
            this.lastCreditCardExpiryReminderSentOnDataGridViewTextBoxColumn,
            this.creditCardExpiryReminderCountDataGridViewTextBoxColumn,
            this.dgvCardColumnIsActive,
            this.createdByDataGridViewTextBoxColumn1,
            this.creationDateDataGridViewTextBoxColumn1,
            this.lastUpdatedByDataGridViewTextBoxColumn1,
            this.lastUpdateDateDataGridViewTextBoxColumn1,
            this.siteIdDataGridViewTextBoxColumn1,
            this.masterEntityIdDataGridViewTextBoxColumn1,
            this.synchStatusDataGridViewCheckBoxColumn1,
            this.guidDataGridViewTextBoxColumn1,
            this.isChangedDataGridViewCheckBoxColumn1});
            this.dgvCreditCards.DataSource = this.customerCreditCardsDTOBindingSource;
            this.dgvCreditCards.Location = new System.Drawing.Point(15, 76);
            this.dgvCreditCards.Name = "dgvCreditCards";
            this.dgvCreditCards.RowHeadersVisible = false;
            this.dgvCreditCards.RowTemplate.Height = 30;
            this.dgvCreditCards.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvCreditCards.Size = new System.Drawing.Size(1174, 399);
            this.dgvCreditCards.TabIndex = 33;
            this.dgvCreditCards.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCreditCards_CellClick);
            // 
            // btnAddCustomerCreditCard
            // 
            this.btnAddCustomerCreditCard.BackColor = System.Drawing.Color.Transparent;
            this.btnAddCustomerCreditCard.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnAddCustomerCreditCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddCustomerCreditCard.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnAddCustomerCreditCard.FlatAppearance.BorderSize = 0;
            this.btnAddCustomerCreditCard.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAddCustomerCreditCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddCustomerCreditCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddCustomerCreditCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCustomerCreditCard.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddCustomerCreditCard.ForeColor = System.Drawing.Color.White;
            this.btnAddCustomerCreditCard.Location = new System.Drawing.Point(15, 19);
            this.btnAddCustomerCreditCard.Name = "btnAddCustomerCreditCard";
            this.btnAddCustomerCreditCard.Size = new System.Drawing.Size(70, 30);
            this.btnAddCustomerCreditCard.TabIndex = 32;
            this.btnAddCustomerCreditCard.Text = "Add Card";
            this.btnAddCustomerCreditCard.UseVisualStyleBackColor = false;
            this.btnAddCustomerCreditCard.Click += new System.EventHandler(this.btnAddCustomerCreditCard_Click);
            this.btnAddCustomerCreditCard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseDown);
            this.btnAddCustomerCreditCard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blueButton_MouseUp);
            // 
            // tabSubscriptions
            // 
            this.tabSubscriptions.Location = new System.Drawing.Point(4, 24);
            this.tabSubscriptions.Name = "tabSubscriptions";
            this.tabSubscriptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabSubscriptions.Size = new System.Drawing.Size(1238, 611);
            this.tabSubscriptions.TabIndex = 2;
            this.tabSubscriptions.Text = "Subscriptions";
            this.tabSubscriptions.UseVisualStyleBackColor = true;
            // 
            // tabCtrlSubscriptions
            // 
            this.tabCtrlSubscriptions.Controls.Add(this.tabSubscriptions);
            this.tabCtrlSubscriptions.Controls.Add(this.tabCustomerCards);
            this.tabCtrlSubscriptions.Location = new System.Drawing.Point(2, 2);
            this.tabCtrlSubscriptions.Name = "tabCtrlSubscriptions";
            this.tabCtrlSubscriptions.SelectedIndex = 0;
            this.tabCtrlSubscriptions.Size = new System.Drawing.Size(1246, 639);
            this.tabCtrlSubscriptions.TabIndex = 0;
            // 
            // vScrollCreditCards
            // 
            this.vScrollCreditCards.AutoHide = false;
            this.vScrollCreditCards.DataGridView = this.dgvCreditCards;
            this.vScrollCreditCards.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollCreditCards.DownButtonBackgroundImage")));
            this.vScrollCreditCards.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollCreditCards.DownButtonDisabledBackgroundImage")));
            this.vScrollCreditCards.Location = new System.Drawing.Point(1192, 78);
            this.vScrollCreditCards.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollCreditCards.Name = "vScrollCreditCards";
            this.vScrollCreditCards.ScrollableControl = null;
            this.vScrollCreditCards.ScrollViewer = null;
            this.vScrollCreditCards.Size = new System.Drawing.Size(40, 397);
            this.vScrollCreditCards.TabIndex = 35;
            this.vScrollCreditCards.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollCreditCards.UpButtonBackgroundImage")));
            this.vScrollCreditCards.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollCreditCards.UpButtonDisabledBackgroundImage")));
            // 
            // hScrollCreditCards
            // 
            this.hScrollCreditCards.AutoHide = false;
            this.hScrollCreditCards.DataGridView = this.dgvCreditCards;
            this.hScrollCreditCards.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollCreditCards.LeftButtonBackgroundImage")));
            this.hScrollCreditCards.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollCreditCards.LeftButtonDisabledBackgroundImage")));
            this.hScrollCreditCards.Location = new System.Drawing.Point(16, 475);
            this.hScrollCreditCards.Margin = new System.Windows.Forms.Padding(0);
            this.hScrollCreditCards.Name = "hScrollCreditCards";
            this.hScrollCreditCards.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollCreditCards.RightButtonBackgroundImage")));
            this.hScrollCreditCards.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollCreditCards.RightButtonDisabledBackgroundImage")));
            this.hScrollCreditCards.ScrollableControl = null;
            this.hScrollCreditCards.ScrollViewer = null;
            this.hScrollCreditCards.Size = new System.Drawing.Size(1173, 40);
            this.hScrollCreditCards.TabIndex = 34;
            // 
            // customerCreditCardsIdDataGridViewTextBoxColumn1
            // 
            this.customerCreditCardsIdDataGridViewTextBoxColumn1.DataPropertyName = "CustomerCreditCardsId";
            this.customerCreditCardsIdDataGridViewTextBoxColumn1.HeaderText = "Id";
            this.customerCreditCardsIdDataGridViewTextBoxColumn1.Name = "customerCreditCardsIdDataGridViewTextBoxColumn1";
            this.customerCreditCardsIdDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // customerIdDataGridViewTextBoxColumn1
            // 
            this.customerIdDataGridViewTextBoxColumn1.DataPropertyName = "CustomerId";
            this.customerIdDataGridViewTextBoxColumn1.HeaderText = "Customer";
            this.customerIdDataGridViewTextBoxColumn1.MinimumWidth = 200;
            this.customerIdDataGridViewTextBoxColumn1.Name = "customerIdDataGridViewTextBoxColumn1";
            this.customerIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.customerIdDataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.customerIdDataGridViewTextBoxColumn1.Visible = false;
            this.customerIdDataGridViewTextBoxColumn1.Width = 200;
            // 
            // cardProfileIdDataGridViewTextBoxColumn
            // 
            this.cardProfileIdDataGridViewTextBoxColumn.DataPropertyName = "CardProfileId";
            this.cardProfileIdDataGridViewTextBoxColumn.HeaderText = "CardProfileId";
            this.cardProfileIdDataGridViewTextBoxColumn.Name = "cardProfileIdDataGridViewTextBoxColumn";
            this.cardProfileIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // dgvColumnPaymentModeId
            // 
            this.dgvColumnPaymentModeId.DataPropertyName = "PaymentModeId";
            this.dgvColumnPaymentModeId.HeaderText = "Payment Mode";
            this.dgvColumnPaymentModeId.MinimumWidth = 200;
            this.dgvColumnPaymentModeId.Name = "dgvColumnPaymentModeId";
            this.dgvColumnPaymentModeId.ReadOnly = true;
            this.dgvColumnPaymentModeId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvColumnPaymentModeId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvColumnPaymentModeId.Width = 200;
            // 
            // tokenIdDataGridViewTextBoxColumn
            // 
            this.tokenIdDataGridViewTextBoxColumn.DataPropertyName = "TokenId";
            this.tokenIdDataGridViewTextBoxColumn.HeaderText = "TokenId";
            this.tokenIdDataGridViewTextBoxColumn.Name = "tokenIdDataGridViewTextBoxColumn";
            this.tokenIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // customerNameOnCardDataGridViewTextBoxColumn
            // 
            this.customerNameOnCardDataGridViewTextBoxColumn.DataPropertyName = "CustomerNameOnCard";
            this.customerNameOnCardDataGridViewTextBoxColumn.HeaderText = "Customer Name On Card";
            this.customerNameOnCardDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.customerNameOnCardDataGridViewTextBoxColumn.Name = "customerNameOnCardDataGridViewTextBoxColumn";
            this.customerNameOnCardDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerNameOnCardDataGridViewTextBoxColumn.Width = 200;
            // 
            // creditCardNumberDataGridViewTextBoxColumn
            // 
            this.creditCardNumberDataGridViewTextBoxColumn.DataPropertyName = "CreditCardNumber";
            this.creditCardNumberDataGridViewTextBoxColumn.HeaderText = "Credit Card Number";
            this.creditCardNumberDataGridViewTextBoxColumn.MinimumWidth = 180;
            this.creditCardNumberDataGridViewTextBoxColumn.Name = "creditCardNumberDataGridViewTextBoxColumn";
            this.creditCardNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.creditCardNumberDataGridViewTextBoxColumn.Width = 180;
            // 
            // cardExpiryDataGridViewTextBoxColumn
            // 
            this.cardExpiryDataGridViewTextBoxColumn.DataPropertyName = "CardExpiry";
            this.cardExpiryDataGridViewTextBoxColumn.HeaderText = "Card Expiry";
            this.cardExpiryDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.cardExpiryDataGridViewTextBoxColumn.Name = "cardExpiryDataGridViewTextBoxColumn";
            // 
            // lastCreditCardExpiryReminderSentOnDataGridViewTextBoxColumn
            // 
            this.lastCreditCardExpiryReminderSentOnDataGridViewTextBoxColumn.DataPropertyName = "LastCreditCardExpiryReminderSentOn";
            this.lastCreditCardExpiryReminderSentOnDataGridViewTextBoxColumn.HeaderText = "Last Expiry Reminder Sent On";
            this.lastCreditCardExpiryReminderSentOnDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.lastCreditCardExpiryReminderSentOnDataGridViewTextBoxColumn.Name = "lastCreditCardExpiryReminderSentOnDataGridViewTextBoxColumn";
            this.lastCreditCardExpiryReminderSentOnDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastCreditCardExpiryReminderSentOnDataGridViewTextBoxColumn.Width = 150;
            // 
            // creditCardExpiryReminderCountDataGridViewTextBoxColumn
            // 
            this.creditCardExpiryReminderCountDataGridViewTextBoxColumn.DataPropertyName = "CreditCardExpiryReminderCount";
            this.creditCardExpiryReminderCountDataGridViewTextBoxColumn.HeaderText = "Expiry Reminder Count";
            this.creditCardExpiryReminderCountDataGridViewTextBoxColumn.MinimumWidth = 140;
            this.creditCardExpiryReminderCountDataGridViewTextBoxColumn.Name = "creditCardExpiryReminderCountDataGridViewTextBoxColumn";
            this.creditCardExpiryReminderCountDataGridViewTextBoxColumn.ReadOnly = true;
            this.creditCardExpiryReminderCountDataGridViewTextBoxColumn.Width = 140;
            // 
            // dgvCardColumnIsActive
            // 
            this.dgvCardColumnIsActive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCardColumnIsActive.DataPropertyName = "IsActive";
            this.dgvCardColumnIsActive.HeaderText = "IsActive";
            this.dgvCardColumnIsActive.MinimumWidth = 80;
            this.dgvCardColumnIsActive.Name = "dgvCardColumnIsActive";
            this.dgvCardColumnIsActive.ReadOnly = true;
            // 
            // createdByDataGridViewTextBoxColumn1
            // 
            this.createdByDataGridViewTextBoxColumn1.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn1.HeaderText = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn1.Name = "createdByDataGridViewTextBoxColumn1";
            this.createdByDataGridViewTextBoxColumn1.Visible = false;
            // 
            // creationDateDataGridViewTextBoxColumn1
            // 
            this.creationDateDataGridViewTextBoxColumn1.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn1.HeaderText = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn1.Name = "creationDateDataGridViewTextBoxColumn1";
            this.creationDateDataGridViewTextBoxColumn1.Visible = false;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn1
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn1.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn1.HeaderText = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn1.Name = "lastUpdatedByDataGridViewTextBoxColumn1";
            this.lastUpdatedByDataGridViewTextBoxColumn1.Visible = false;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn1
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn1.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn1.HeaderText = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn1.Name = "lastUpdateDateDataGridViewTextBoxColumn1";
            this.lastUpdateDateDataGridViewTextBoxColumn1.Visible = false;
            // 
            // siteIdDataGridViewTextBoxColumn1
            // 
            this.siteIdDataGridViewTextBoxColumn1.DataPropertyName = "SiteId";
            this.siteIdDataGridViewTextBoxColumn1.HeaderText = "SiteId";
            this.siteIdDataGridViewTextBoxColumn1.Name = "siteIdDataGridViewTextBoxColumn1";
            this.siteIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // masterEntityIdDataGridViewTextBoxColumn1
            // 
            this.masterEntityIdDataGridViewTextBoxColumn1.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn1.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn1.Name = "masterEntityIdDataGridViewTextBoxColumn1";
            this.masterEntityIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn1
            // 
            this.synchStatusDataGridViewCheckBoxColumn1.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn1.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn1.Name = "synchStatusDataGridViewCheckBoxColumn1";
            this.synchStatusDataGridViewCheckBoxColumn1.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn1
            // 
            this.guidDataGridViewTextBoxColumn1.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn1.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn1.Name = "guidDataGridViewTextBoxColumn1";
            this.guidDataGridViewTextBoxColumn1.Visible = false;
            // 
            // isChangedDataGridViewCheckBoxColumn1
            // 
            this.isChangedDataGridViewCheckBoxColumn1.DataPropertyName = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn1.HeaderText = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn1.Name = "isChangedDataGridViewCheckBoxColumn1";
            this.isChangedDataGridViewCheckBoxColumn1.Visible = false;
            // 
            // customerCreditCardsDTOBindingSource
            // 
            this.customerCreditCardsDTOBindingSource.DataSource = typeof(Semnox.Parafait.Transaction.CustomerCreditCardsDTO);
            // 
            // frmCustomerSubscription
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1247, 641);
            this.Controls.Add(this.tabCtrlSubscriptions);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCustomerSubscription";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Subscriptions";
            this.Load += new System.EventHandler(this.frmCustomerSubscription_Load);
            this.tabCustomerCards.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCreditCards)).EndInit();
            this.tabCtrlSubscriptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customerCreditCardsDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource customerCreditCardsDTOBindingSource; 
        private System.Windows.Forms.TabPage tabCustomerCards;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vScrollCreditCards;
        private System.Windows.Forms.DataGridView dgvCreditCards;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView hScrollCreditCards;
        private System.Windows.Forms.Button btnAddCustomerCreditCard;
        private System.Windows.Forms.TabPage tabSubscriptions;
        private System.Windows.Forms.TabControl tabCtrlSubscriptions;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerCreditCardsIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardProfileIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvColumnPaymentModeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn tokenIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameOnCardDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creditCardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardExpiryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastCreditCardExpiryReminderSentOnDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creditCardExpiryReminderCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCardColumnIsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedDataGridViewCheckBoxColumn1;
    }
}