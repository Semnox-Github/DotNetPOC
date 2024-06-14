namespace Semnox.Parafait.Customer.Accounts
{
    partial class AccountActivityListUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountActivityListUI));
            this.label1 = new System.Windows.Forms.Label();
            this.dgvAccountActivityDTOList = new System.Windows.Forms.DataGridView();
            this.accountActivityDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.dgvGamePlayDTOList = new System.Windows.Forms.DataGridView();
            this.gamePlayDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportToExcelPurchases = new System.Windows.Forms.Button();
            this.btnExportToExcelGamePlay = new System.Windows.Forms.Button();
            this.lnkShowHideExtended = new System.Windows.Forms.LinkLabel();
            this.lnkConsolidated = new System.Windows.Forms.LinkLabel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.dateAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creditsAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.courtesyAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bonusAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tockensAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketsAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loyaltyPointsAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pOSAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userAccountActivityNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.refIdAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activityTypeAccountActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.playDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creditsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPCardBalanceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPCreditsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardGameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.courtesyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bonusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPBonusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manualTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketEarterTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountActivityDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountActivityDTOListBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGamePlayDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gamePlayDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Purchases and Tasks";
            // 
            // dgvAccountActivityDTOList
            // 
            this.dgvAccountActivityDTOList.AllowUserToAddRows = false;
            this.dgvAccountActivityDTOList.AllowUserToDeleteRows = false;
            this.dgvAccountActivityDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAccountActivityDTOList.AutoGenerateColumns = false;
            this.dgvAccountActivityDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccountActivityDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dateAccountActivityDataGridViewTextBoxColumn,
            this.productAccountActivityDataGridViewTextBoxColumn,
            this.amountAccountActivityDataGridViewTextBoxColumn,
            this.creditsAccountActivityDataGridViewTextBoxColumn,
            this.courtesyAccountActivityDataGridViewTextBoxColumn,
            this.bonusAccountActivityDataGridViewTextBoxColumn,
            this.timeAccountActivityDataGridViewTextBoxColumn,
            this.tockensAccountActivityDataGridViewTextBoxColumn,
            this.ticketsAccountActivityDataGridViewTextBoxColumn,
            this.loyaltyPointsAccountActivityDataGridViewTextBoxColumn,
            this.siteAccountActivityDataGridViewTextBoxColumn,
            this.pOSAccountActivityDataGridViewTextBoxColumn,
            this.userAccountActivityNameDataGridViewTextBoxColumn,
            this.quantityAccountActivityDataGridViewTextBoxColumn,
            this.priceAccountActivityDataGridViewTextBoxColumn,
            this.refIdAccountActivityDataGridViewTextBoxColumn,
            this.activityTypeAccountActivityDataGridViewTextBoxColumn});
            this.dgvAccountActivityDTOList.DataSource = this.accountActivityDTOListBS;
            this.dgvAccountActivityDTOList.Location = new System.Drawing.Point(16, 24);
            this.dgvAccountActivityDTOList.Name = "dgvAccountActivityDTOList";
            this.dgvAccountActivityDTOList.ReadOnly = true;
            this.dgvAccountActivityDTOList.Size = new System.Drawing.Size(896, 314);
            this.dgvAccountActivityDTOList.TabIndex = 1;
            // 
            // accountActivityDTOListBS
            // 
            this.accountActivityDTOListBS.DataSource = typeof(Semnox.Parafait.Customer.Accounts.AccountActivityDTO);
            // 
            // dgvGamePlayDTOList
            // 
            this.dgvGamePlayDTOList.AllowUserToAddRows = false;
            this.dgvGamePlayDTOList.AllowUserToDeleteRows = false;
            this.dgvGamePlayDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGamePlayDTOList.AutoGenerateColumns = false;
            this.dgvGamePlayDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGamePlayDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.playDateDataGridViewTextBoxColumn,
            this.gameDataGridViewTextBoxColumn,
            this.creditsDataGridViewTextBoxColumn,
            this.cPCardBalanceDataGridViewTextBoxColumn,
            this.cPCreditsDataGridViewTextBoxColumn,
            this.cardGameDataGridViewTextBoxColumn,
            this.courtesyDataGridViewTextBoxColumn,
            this.bonusDataGridViewTextBoxColumn,
            this.cPBonusDataGridViewTextBoxColumn,
            this.timeDataGridViewTextBoxColumn,
            this.ticketCountDataGridViewTextBoxColumn,
            this.eTicketsDataGridViewTextBoxColumn,
            this.manualTicketsDataGridViewTextBoxColumn,
            this.ticketEarterTicketsDataGridViewTextBoxColumn,
            this.modeDataGridViewTextBoxColumn,
            this.siteDataGridViewTextBoxColumn});
            this.dgvGamePlayDTOList.DataSource = this.gamePlayDTOListBS;
            this.dgvGamePlayDTOList.Location = new System.Drawing.Point(16, 372);
            this.dgvGamePlayDTOList.Name = "dgvGamePlayDTOList";
            this.dgvGamePlayDTOList.ReadOnly = true;
            this.dgvGamePlayDTOList.Size = new System.Drawing.Size(896, 329);
            this.dgvGamePlayDTOList.TabIndex = 2;
            this.dgvGamePlayDTOList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvGamePlayDTOList_CellFormatting);
            this.dgvGamePlayDTOList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvGamePlayDTOList_DataBindingComplete);
            // 
            // gamePlayDTOListBS
            // 
            this.gamePlayDTOListBS.DataSource = typeof(Semnox.Parafait.Game.GamePlayDTO);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 353);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Game Plays";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(343, 710);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(98, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(485, 710);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExportToExcelPurchases
            // 
            this.btnExportToExcelPurchases.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportToExcelPurchases.Location = new System.Drawing.Point(797, 344);
            this.btnExportToExcelPurchases.Name = "btnExportToExcelPurchases";
            this.btnExportToExcelPurchases.Size = new System.Drawing.Size(115, 25);
            this.btnExportToExcelPurchases.TabIndex = 6;
            this.btnExportToExcelPurchases.Text = "Export to Excel";
            this.btnExportToExcelPurchases.UseVisualStyleBackColor = true;
            this.btnExportToExcelPurchases.Click += new System.EventHandler(this.btnExportToExcelPurchases_Click);
            // 
            // btnExportToExcelGamePlay
            // 
            this.btnExportToExcelGamePlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportToExcelGamePlay.Location = new System.Drawing.Point(797, 709);
            this.btnExportToExcelGamePlay.Name = "btnExportToExcelGamePlay";
            this.btnExportToExcelGamePlay.Size = new System.Drawing.Size(115, 25);
            this.btnExportToExcelGamePlay.TabIndex = 7;
            this.btnExportToExcelGamePlay.Text = "Export to Excel";
            this.btnExportToExcelGamePlay.UseVisualStyleBackColor = true;
            this.btnExportToExcelGamePlay.Click += new System.EventHandler(this.btnExportToExcelGamePlay_Click);
            // 
            // lnkShowHideExtended
            // 
            this.lnkShowHideExtended.AutoSize = true;
            this.lnkShowHideExtended.Location = new System.Drawing.Point(103, 353);
            this.lnkShowHideExtended.Name = "lnkShowHideExtended";
            this.lnkShowHideExtended.Size = new System.Drawing.Size(99, 16);
            this.lnkShowHideExtended.TabIndex = 13;
            this.lnkShowHideExtended.TabStop = true;
            this.lnkShowHideExtended.Tag = "0";
            this.lnkShowHideExtended.Text = "Show Extended";
            this.lnkShowHideExtended.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkShowHideExtended_LinkClicked);
            // 
            // lnkConsolidated
            // 
            this.lnkConsolidated.AutoSize = true;
            this.lnkConsolidated.Location = new System.Drawing.Point(245, 353);
            this.lnkConsolidated.Name = "lnkConsolidated";
            this.lnkConsolidated.Size = new System.Drawing.Size(115, 16);
            this.lnkConsolidated.TabIndex = 14;
            this.lnkConsolidated.TabStop = true;
            this.lnkConsolidated.Tag = "0";
            this.lnkConsolidated.Text = "Consolidated View";
            this.lnkConsolidated.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkConsolidated_LinkClicked);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(18, 703);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(133, 15);
            this.lblStatus.TabIndex = 45;
            this.lblStatus.Text = "Loading.. Please wait..";
            // 
            // dateAccountActivityDataGridViewTextBoxColumn
            // 
            this.dateAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "Date";
            this.dateAccountActivityDataGridViewTextBoxColumn.HeaderText = "Date";
            this.dateAccountActivityDataGridViewTextBoxColumn.Name = "dateAccountActivityDataGridViewTextBoxColumn";
            this.dateAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // productAccountActivityDataGridViewTextBoxColumn
            // 
            this.productAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "Product";
            this.productAccountActivityDataGridViewTextBoxColumn.HeaderText = "Product";
            this.productAccountActivityDataGridViewTextBoxColumn.Name = "productAccountActivityDataGridViewTextBoxColumn";
            this.productAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // amountAccountActivityDataGridViewTextBoxColumn
            // 
            this.amountAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            this.amountAccountActivityDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.amountAccountActivityDataGridViewTextBoxColumn.Name = "amountAccountActivityDataGridViewTextBoxColumn";
            this.amountAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // creditsAccountActivityDataGridViewTextBoxColumn
            // 
            this.creditsAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "Credits";
            this.creditsAccountActivityDataGridViewTextBoxColumn.HeaderText = "Credits";
            this.creditsAccountActivityDataGridViewTextBoxColumn.Name = "creditsAccountActivityDataGridViewTextBoxColumn";
            this.creditsAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // courtesyAccountActivityDataGridViewTextBoxColumn
            // 
            this.courtesyAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "Courtesy";
            this.courtesyAccountActivityDataGridViewTextBoxColumn.HeaderText = "Courtesy";
            this.courtesyAccountActivityDataGridViewTextBoxColumn.Name = "courtesyAccountActivityDataGridViewTextBoxColumn";
            this.courtesyAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bonusAccountActivityDataGridViewTextBoxColumn
            // 
            this.bonusAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "Bonus";
            this.bonusAccountActivityDataGridViewTextBoxColumn.HeaderText = "Bonus";
            this.bonusAccountActivityDataGridViewTextBoxColumn.Name = "bonusAccountActivityDataGridViewTextBoxColumn";
            this.bonusAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // timeAccountActivityDataGridViewTextBoxColumn
            // 
            this.timeAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "Time";
            this.timeAccountActivityDataGridViewTextBoxColumn.HeaderText = "Time";
            this.timeAccountActivityDataGridViewTextBoxColumn.Name = "timeAccountActivityDataGridViewTextBoxColumn";
            this.timeAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tockensAccountActivityDataGridViewTextBoxColumn
            // 
            this.tockensAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "Tokens";
            this.tockensAccountActivityDataGridViewTextBoxColumn.HeaderText = "Tokens";
            this.tockensAccountActivityDataGridViewTextBoxColumn.Name = "tockensAccountActivityDataGridViewTextBoxColumn";
            this.tockensAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ticketsAccountActivityDataGridViewTextBoxColumn
            // 
            this.ticketsAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "Tickets";
            this.ticketsAccountActivityDataGridViewTextBoxColumn.HeaderText = "Tickets";
            this.ticketsAccountActivityDataGridViewTextBoxColumn.Name = "ticketsAccountActivityDataGridViewTextBoxColumn";
            this.ticketsAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // loyaltyPointsAccountActivityDataGridViewTextBoxColumn
            // 
            this.loyaltyPointsAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "LoyaltyPoints";
            this.loyaltyPointsAccountActivityDataGridViewTextBoxColumn.HeaderText = "Loyalty Points";
            this.loyaltyPointsAccountActivityDataGridViewTextBoxColumn.Name = "loyaltyPointsAccountActivityDataGridViewTextBoxColumn";
            this.loyaltyPointsAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // siteAccountActivityDataGridViewTextBoxColumn
            // 
            this.siteAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "Site";
            this.siteAccountActivityDataGridViewTextBoxColumn.HeaderText = "Site";
            this.siteAccountActivityDataGridViewTextBoxColumn.Name = "siteAccountActivityDataGridViewTextBoxColumn";
            this.siteAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pOSAccountActivityDataGridViewTextBoxColumn
            // 
            this.pOSAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "POS";
            this.pOSAccountActivityDataGridViewTextBoxColumn.HeaderText = "POS";
            this.pOSAccountActivityDataGridViewTextBoxColumn.Name = "pOSAccountActivityDataGridViewTextBoxColumn";
            this.pOSAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // userAccountActivityNameDataGridViewTextBoxColumn
            // 
            this.userAccountActivityNameDataGridViewTextBoxColumn.DataPropertyName = "UserName";
            this.userAccountActivityNameDataGridViewTextBoxColumn.HeaderText = "UserName";
            this.userAccountActivityNameDataGridViewTextBoxColumn.Name = "userAccountActivityNameDataGridViewTextBoxColumn";
            this.userAccountActivityNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // quantityAccountActivityDataGridViewTextBoxColumn
            // 
            this.quantityAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityAccountActivityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityAccountActivityDataGridViewTextBoxColumn.Name = "quantityAccountActivityDataGridViewTextBoxColumn";
            this.quantityAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // priceAccountActivityDataGridViewTextBoxColumn
            // 
            this.priceAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "Price";
            this.priceAccountActivityDataGridViewTextBoxColumn.HeaderText = "Price";
            this.priceAccountActivityDataGridViewTextBoxColumn.Name = "priceAccountActivityDataGridViewTextBoxColumn";
            this.priceAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // refIdAccountActivityDataGridViewTextBoxColumn
            // 
            this.refIdAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "RefId";
            this.refIdAccountActivityDataGridViewTextBoxColumn.HeaderText = "RefId";
            this.refIdAccountActivityDataGridViewTextBoxColumn.Name = "refIdAccountActivityDataGridViewTextBoxColumn";
            this.refIdAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // activityTypeAccountActivityDataGridViewTextBoxColumn
            // 
            this.activityTypeAccountActivityDataGridViewTextBoxColumn.DataPropertyName = "ActivityType";
            this.activityTypeAccountActivityDataGridViewTextBoxColumn.HeaderText = "Activity Type";
            this.activityTypeAccountActivityDataGridViewTextBoxColumn.Name = "activityTypeAccountActivityDataGridViewTextBoxColumn";
            this.activityTypeAccountActivityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // playDateDataGridViewTextBoxColumn
            // 
            this.playDateDataGridViewTextBoxColumn.DataPropertyName = "PlayDate";
            this.playDateDataGridViewTextBoxColumn.HeaderText = "Date";
            this.playDateDataGridViewTextBoxColumn.Name = "playDateDataGridViewTextBoxColumn";
            this.playDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // gameDataGridViewTextBoxColumn
            // 
            this.gameDataGridViewTextBoxColumn.DataPropertyName = "Game";
            this.gameDataGridViewTextBoxColumn.HeaderText = "Game";
            this.gameDataGridViewTextBoxColumn.Name = "gameDataGridViewTextBoxColumn";
            this.gameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // creditsDataGridViewTextBoxColumn
            // 
            this.creditsDataGridViewTextBoxColumn.DataPropertyName = "Credits";
            this.creditsDataGridViewTextBoxColumn.HeaderText = "Credits";
            this.creditsDataGridViewTextBoxColumn.Name = "creditsDataGridViewTextBoxColumn";
            this.creditsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cPCardBalanceDataGridViewTextBoxColumn
            // 
            this.cPCardBalanceDataGridViewTextBoxColumn.DataPropertyName = "CPCardBalance";
            this.cPCardBalanceDataGridViewTextBoxColumn.HeaderText = "CPCardBalance";
            this.cPCardBalanceDataGridViewTextBoxColumn.Name = "cPCardBalanceDataGridViewTextBoxColumn";
            this.cPCardBalanceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cPCreditsDataGridViewTextBoxColumn
            // 
            this.cPCreditsDataGridViewTextBoxColumn.DataPropertyName = "CPCredits";
            this.cPCreditsDataGridViewTextBoxColumn.HeaderText = "CPCredits";
            this.cPCreditsDataGridViewTextBoxColumn.Name = "cPCreditsDataGridViewTextBoxColumn";
            this.cPCreditsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cardGameDataGridViewTextBoxColumn
            // 
            this.cardGameDataGridViewTextBoxColumn.DataPropertyName = "CardGame";
            this.cardGameDataGridViewTextBoxColumn.HeaderText = "CardGame";
            this.cardGameDataGridViewTextBoxColumn.Name = "cardGameDataGridViewTextBoxColumn";
            this.cardGameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // courtesyDataGridViewTextBoxColumn
            // 
            this.courtesyDataGridViewTextBoxColumn.DataPropertyName = "Courtesy";
            this.courtesyDataGridViewTextBoxColumn.HeaderText = "Courtesy";
            this.courtesyDataGridViewTextBoxColumn.Name = "courtesyDataGridViewTextBoxColumn";
            this.courtesyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bonusDataGridViewTextBoxColumn
            // 
            this.bonusDataGridViewTextBoxColumn.DataPropertyName = "Bonus";
            this.bonusDataGridViewTextBoxColumn.HeaderText = "Bonus";
            this.bonusDataGridViewTextBoxColumn.Name = "bonusDataGridViewTextBoxColumn";
            this.bonusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cPBonusDataGridViewTextBoxColumn
            // 
            this.cPBonusDataGridViewTextBoxColumn.DataPropertyName = "CPBonus";
            this.cPBonusDataGridViewTextBoxColumn.HeaderText = "CPBonus";
            this.cPBonusDataGridViewTextBoxColumn.Name = "cPBonusDataGridViewTextBoxColumn";
            this.cPBonusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // timeDataGridViewTextBoxColumn
            // 
            this.timeDataGridViewTextBoxColumn.DataPropertyName = "Time";
            this.timeDataGridViewTextBoxColumn.HeaderText = "Time";
            this.timeDataGridViewTextBoxColumn.Name = "timeDataGridViewTextBoxColumn";
            this.timeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ticketCountDataGridViewTextBoxColumn
            // 
            this.ticketCountDataGridViewTextBoxColumn.DataPropertyName = "TicketCount";
            this.ticketCountDataGridViewTextBoxColumn.HeaderText = "Tickets";
            this.ticketCountDataGridViewTextBoxColumn.Name = "ticketCountDataGridViewTextBoxColumn";
            this.ticketCountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // eTicketsDataGridViewTextBoxColumn
            // 
            this.eTicketsDataGridViewTextBoxColumn.DataPropertyName = "ETickets";
            this.eTicketsDataGridViewTextBoxColumn.HeaderText = "e-Tickets";
            this.eTicketsDataGridViewTextBoxColumn.Name = "eTicketsDataGridViewTextBoxColumn";
            this.eTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // manualTicketsDataGridViewTextBoxColumn
            // 
            this.manualTicketsDataGridViewTextBoxColumn.DataPropertyName = "ManualTickets";
            this.manualTicketsDataGridViewTextBoxColumn.HeaderText = "Manual Tickets";
            this.manualTicketsDataGridViewTextBoxColumn.Name = "manualTicketsDataGridViewTextBoxColumn";
            this.manualTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ticketEarterTicketsDataGridViewTextBoxColumn
            // 
            this.ticketEarterTicketsDataGridViewTextBoxColumn.DataPropertyName = "TicketEaterTickets";
            this.ticketEarterTicketsDataGridViewTextBoxColumn.HeaderText = "T.Eater Tickets";
            this.ticketEarterTicketsDataGridViewTextBoxColumn.Name = "ticketEarterTicketsDataGridViewTextBoxColumn";
            this.ticketEarterTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // modeDataGridViewTextBoxColumn
            // 
            this.modeDataGridViewTextBoxColumn.DataPropertyName = "Mode";
            this.modeDataGridViewTextBoxColumn.HeaderText = "Mode";
            this.modeDataGridViewTextBoxColumn.Name = "modeDataGridViewTextBoxColumn";
            this.modeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // siteDataGridViewTextBoxColumn
            // 
            this.siteDataGridViewTextBoxColumn.DataPropertyName = "Site";
            this.siteDataGridViewTextBoxColumn.HeaderText = "Site";
            this.siteDataGridViewTextBoxColumn.Name = "siteDataGridViewTextBoxColumn";
            this.siteDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // AccountActivityListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(924, 741);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lnkConsolidated);
            this.Controls.Add(this.lnkShowHideExtended);
            this.Controls.Add(this.btnExportToExcelGamePlay);
            this.Controls.Add(this.btnExportToExcelPurchases);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvGamePlayDTOList);
            this.Controls.Add(this.dgvAccountActivityDTOList);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "AccountActivityListUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Card Activity for Card Number";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AccountActivityListUI_FormClosed);
            this.Load += new System.EventHandler(this.CardActivity_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountActivityDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountActivityDTOListBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGamePlayDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gamePlayDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvAccountActivityDTOList;
        private System.Windows.Forms.DataGridView dgvGamePlayDTOList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExportToExcelPurchases;
        private System.Windows.Forms.Button btnExportToExcelGamePlay;
        private System.Windows.Forms.LinkLabel lnkShowHideExtended;
        private System.Windows.Forms.LinkLabel lnkConsolidated;
        private System.Windows.Forms.BindingSource accountActivityDTOListBS;
        private System.Windows.Forms.BindingSource gamePlayDTOListBS;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creditsAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn courtesyAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bonusAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tockensAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticketsAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn loyaltyPointsAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pOSAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userAccountActivityNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn refIdAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn activityTypeAccountActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn playDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creditsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPCardBalanceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPCreditsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardGameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn courtesyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bonusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPBonusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticketCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn manualTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticketEarterTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteDataGridViewTextBoxColumn;
    }
}