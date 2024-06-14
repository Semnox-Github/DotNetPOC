using Semnox.Parafait.CardCore;

namespace Parafait_POS
{
    partial class FormCardTasks
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCardTasks));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControlTasks = new System.Windows.Forms.TabControl();
            this.tabPageTransferCard = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.sampleButtonCardSaleProduct = new System.Windows.Forms.Button();
            this.sampleButtonCardSaleSelectedProduct = new System.Windows.Forms.Button();
            this.btnCustomerLookUp = new System.Windows.Forms.Button();
            this.btnAlphanumericCancelndarTransferCard = new System.Windows.Forms.Button();
            this.rb10DFormat = new System.Windows.Forms.RadioButton();
            this.rb10HFormat = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonTransferCardGetDetails = new System.Windows.Forms.Button();
            this.textBoxTransferCardNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPageHoldEntitlement = new System.Windows.Forms.TabPage();
            this.buttonClear = new System.Windows.Forms.Button();
            this.getHoldCardDetails = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.btnAlphanumericCancelndarHoldEntitlement = new System.Windows.Forms.Button();
            this.textBoxHoldEntitlementNumber = new System.Windows.Forms.TextBox();
            this.tabPageExchangeTokenForCredit = new System.Windows.Forms.TabPage();
            this.txtCreditsAdded = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTokensExchanged = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPageExchangeCreditForToken = new System.Windows.Forms.TabPage();
            this.txtCreditsRequired = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTokensBought = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPageLoadTickets = new System.Windows.Forms.TabPage();
            this.lblScannedTickets = new System.Windows.Forms.Label();
            this.lblEnterTicketsToLoad = new System.Windows.Forms.Label();
            this.textBoxLoadTickets = new System.Windows.Forms.TextBox();
            this.tabPageConsolidate = new System.Windows.Forms.TabPage();
            this.tabPageBalanceTransfer = new System.Windows.Forms.TabPage();
            this.dgvBalanceTransferee = new System.Windows.Forms.DataGridView();
            this.dcTrToCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcTrToCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcTrToBonus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcTrToTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcTransferCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcTransferBonus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcTransferTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcTrToCardId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTransfereeCardDetails = new System.Windows.Forms.Label();
            this.lblTransfererCardDetails = new System.Windows.Forms.Label();
            this.tabPageLoadMultiple = new System.Windows.Forms.TabPage();
            this.lblEntitlement = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLoadCardSerialMapping = new System.Windows.Forms.Button();
            this.txtToCardSerialNumber = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtFromCardSerialNumber = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRemoveProduct = new System.Windows.Forms.Button();
            this.dgvProductsAdded = new System.Windows.Forms.DataGridView();
            this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Credits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bonus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Courtesy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnChooseProduct = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvMultipleCards = new System.Windows.Forms.DataGridView();
            this.SerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Card_Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BulkSerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageRealETicket = new System.Windows.Forms.TabPage();
            this.grpRealEticket = new System.Windows.Forms.GroupBox();
            this.radioButtoneTicket = new System.Windows.Forms.RadioButton();
            this.radioButtonRealTicket = new System.Windows.Forms.RadioButton();
            this.tabPageRefundCard = new System.Windows.Forms.TabPage();
            this.grpRefundTime = new System.Windows.Forms.GroupBox();
            this.cmbLoadTimeAttributes = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.txtRefundTime = new System.Windows.Forms.TextBox();
            this.dgvRefundTime = new System.Windows.Forms.DataGridView();
            this.chkRefundTime = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.grpRefundCredits = new System.Windows.Forms.GroupBox();
            this.dgvRefundCardData = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.lblRefundAmount = new System.Windows.Forms.Label();
            this.txtRefundAmount = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.chkMakeCardNewOnFullRefund = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.lblTaxAmount = new System.Windows.Forms.Label();
            this.grpRefundGames = new System.Windows.Forms.GroupBox();
            this.cmbLoadGameAttributes = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.lblLoadGameAttributes = new System.Windows.Forms.Label();
            this.dgvRefundBalanceGames = new System.Windows.Forms.DataGridView();
            this.SelectRefundGame = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.txtRefundGameAmount = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.lblRefundGameCount = new System.Windows.Forms.Label();
            this.tabPageLoadBonus = new System.Windows.Forms.TabPage();
            this.panelLoadBonusManualCard = new System.Windows.Forms.Panel();
            this.btnGetLoadBonusManualCard = new System.Windows.Forms.Button();
            this.btnLoadBonusAlphaKeypad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLoadBonusManualCardNumber = new System.Windows.Forms.TextBox();
            this.grpLoadBonusTypes = new System.Windows.Forms.GroupBox();
            this.flpLoadBonusTypes = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLoadBonus = new System.Windows.Forms.TextBox();
            this.tabPageDiscount = new System.Windows.Forms.TabPage();
            this.dgvCardDiscountsDTOList = new System.Windows.Forms.DataGridView();
            this.dgvCardDiscountsDTOListCardDiscountIdTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCardDiscountsDTOListDiscountIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvCardDiscountsDTOListExpiryDateTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCardDiscountsDTOListLastUpdatedUserTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCardDiscountsDTOListLastUpdatedDateTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCardDiscountsDTOListIsActiveCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cardDiscountsDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbExpires = new System.Windows.Forms.RadioButton();
            this.rbNever = new System.Windows.Forms.RadioButton();
            this.dtpDiscountExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.btnAddDiscount = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbDiscount = new System.Windows.Forms.ComboBox();
            this.tabPageRedeemLoyalty = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.labelVirtualPoint = new System.Windows.Forms.Label();
            this.btnGetRedemptionValues = new System.Windows.Forms.Button();
            this.btnVirtualGetRedemptionValues = new System.Windows.Forms.Button();
            this.dgvLoyaltyRedemption = new System.Windows.Forms.DataGridView();
            this.dgvVirtualPointRedemption = new System.Windows.Forms.DataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SelectedVirtualPoint = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.txtLoyaltyRedeemPoints = new System.Windows.Forms.TextBox();
            this.txtVirtualRedeemPoints = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.labelVirtualPointRedeem = new System.Windows.Forms.Label();
            this.txtLoyaltyPoints = new System.Windows.Forms.TextBox();
            this.txtVirtualPoints = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.labelVirtualPointAvailable = new System.Windows.Forms.Label();
            this.tabPageRedeemVirtualPoints = new System.Windows.Forms.TabPage();
            this.tpSpecialPricing = new System.Windows.Forms.TabPage();
            this.btnClearPricing = new System.Windows.Forms.Button();
            this.dgvSpecialPricing = new System.Windows.Forms.DataGridView();
            this.PricingId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PricingName = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Percentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequiresManagerApproval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpRedeemTicketsForBonus = new System.Windows.Forms.TabPage();
            this.grpBoxTicketType = new System.Windows.Forms.GroupBox();
            this.rbCardBalance = new System.Windows.Forms.RadioButton();
            this.rbBonus = new System.Windows.Forms.RadioButton();
            this.txtBonusEligible = new System.Windows.Forms.TextBox();
            this.lblBonusEligible = new System.Windows.Forms.Label();
            this.txtTicketsAvailable = new System.Windows.Forms.TextBox();
            this.lblTicketsAvlbl = new System.Windows.Forms.Label();
            this.btnRefreshBonus = new System.Windows.Forms.Button();
            this.txtTicketsToRedeem = new System.Windows.Forms.TextBox();
            this.lblTicketsToRedeem = new System.Windows.Forms.Label();
            this.tpSetSiteCode = new System.Windows.Forms.TabPage();
            this.cbSiteCode = new System.Windows.Forms.ComboBox();
            this.btnChangeSiteCode = new System.Windows.Forms.Button();
            this.txtCurrentSiteCode = new System.Windows.Forms.TextBox();
            this.lblNewSiteCode = new System.Windows.Forms.Label();
            this.lblCurrentSiteCode = new System.Windows.Forms.Label();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.tpGetMiFareGPDetails = new System.Windows.Forms.TabPage();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.txtNoOfGamePlays = new System.Windows.Forms.TextBox();
            this.lblNoOfGamePlays = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.dgvCardDetails = new System.Windows.Forms.DataGridView();
            this.dcSiteId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcMachineAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcStartBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcEndBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcComments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtCardNumberMiFare = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tpRedeemBonusForTicket = new System.Windows.Forms.TabPage();
            this.txtElgibleTickets = new System.Windows.Forms.TextBox();
            this.lblTicketsElgible = new System.Windows.Forms.Label();
            this.txtBonusAvailable = new System.Windows.Forms.TextBox();
            this.lblBonusAvl = new System.Windows.Forms.Label();
            this.btnBonusRefresh = new System.Windows.Forms.Button();
            this.txtBonusToRedeem = new System.Windows.Forms.TextBox();
            this.lblBonusToRedeem = new System.Windows.Forms.Label();
            this.tpPauseTime = new System.Windows.Forms.TabPage();
            this.txteTicket = new System.Windows.Forms.TextBox();
            this.lblPausetimeEticketBal = new System.Windows.Forms.Label();
            this.textTimeRemaining = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.textCardNo = new System.Windows.Forms.TextBox();
            this.tpConvertCreditsToTime = new System.Windows.Forms.TabPage();
            this.btnClearPointsToConvert = new System.Windows.Forms.Button();
            this.grpPointsToConvert = new System.Windows.Forms.GroupBox();
            this.dgvPointsToConvert = new System.Windows.Forms.DataGridView();
            this.dcAdditionalPoints = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcAdditionalTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcBalancePoints = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTappedCard = new System.Windows.Forms.DataGridView();
            this.ParentCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParentTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParentCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParentCardId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpConvertTimeToPoints = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dgvTimeToConvert = new System.Windows.Forms.DataGridView();
            this.dcNewTimeToConvert = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcNewPoints = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcNewTimeBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCardInfo = new System.Windows.Forms.DataGridView();
            this.dcParentCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcParentCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcParentPoints = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcParentTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcParentCardId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxMessageLine = new System.Windows.Forms.TextBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnShowNumPad = new System.Windows.Forms.Button();
            this.tabControlTasks.SuspendLayout();
            this.tabPageTransferCard.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabPageHoldEntitlement.SuspendLayout();
            this.tabPageExchangeTokenForCredit.SuspendLayout();
            this.tabPageExchangeCreditForToken.SuspendLayout();
            this.tabPageLoadTickets.SuspendLayout();
            this.tabPageBalanceTransfer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBalanceTransferee)).BeginInit();
            this.tabPageLoadMultiple.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductsAdded)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMultipleCards)).BeginInit();
            this.tabPageRealETicket.SuspendLayout();
            this.grpRealEticket.SuspendLayout();
            this.tabPageRefundCard.SuspendLayout();
            this.grpRefundTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRefundTime)).BeginInit();
            this.grpRefundCredits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRefundCardData)).BeginInit();
            this.grpRefundGames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRefundBalanceGames)).BeginInit();
            this.tabPageLoadBonus.SuspendLayout();
            this.panelLoadBonusManualCard.SuspendLayout();
            this.grpLoadBonusTypes.SuspendLayout();
            this.tabPageDiscount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardDiscountsDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardDiscountsDTOListBS)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tabPageRedeemLoyalty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoyaltyRedemption)).BeginInit();
            this.tabPageRedeemVirtualPoints.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVirtualPointRedemption)).BeginInit();
            this.tpSpecialPricing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpecialPricing)).BeginInit();
            this.tpRedeemTicketsForBonus.SuspendLayout();
            this.grpBoxTicketType.SuspendLayout();
            this.tpSetSiteCode.SuspendLayout();
            this.tpGetMiFareGPDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardDetails)).BeginInit();
            this.tpRedeemBonusForTicket.SuspendLayout();
            this.tpPauseTime.SuspendLayout();
            this.tpConvertCreditsToTime.SuspendLayout();
            this.grpPointsToConvert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPointsToConvert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTappedCard)).BeginInit();
            this.tpConvertTimeToPoints.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTimeToConvert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlTasks
            // 
            this.tabControlTasks.Controls.Add(this.tabPageTransferCard);
            this.tabControlTasks.Controls.Add(this.tabPageHoldEntitlement);
            this.tabControlTasks.Controls.Add(this.tabPageExchangeTokenForCredit);
            this.tabControlTasks.Controls.Add(this.tabPageExchangeCreditForToken);
            this.tabControlTasks.Controls.Add(this.tabPageLoadTickets);
            this.tabControlTasks.Controls.Add(this.tabPageConsolidate);
            this.tabControlTasks.Controls.Add(this.tabPageBalanceTransfer);
            this.tabControlTasks.Controls.Add(this.tabPageLoadMultiple);
            this.tabControlTasks.Controls.Add(this.tabPageRealETicket);
            this.tabControlTasks.Controls.Add(this.tabPageRefundCard);
            this.tabControlTasks.Controls.Add(this.tabPageLoadBonus);
            this.tabControlTasks.Controls.Add(this.tabPageDiscount);
            this.tabControlTasks.Controls.Add(this.tabPageRedeemLoyalty);
            this.tabControlTasks.Controls.Add(this.tabPageRedeemVirtualPoints);
            this.tabControlTasks.Controls.Add(this.tpSpecialPricing);
            this.tabControlTasks.Controls.Add(this.tpRedeemTicketsForBonus);
            this.tabControlTasks.Controls.Add(this.tpSetSiteCode);
            this.tabControlTasks.Controls.Add(this.tpGetMiFareGPDetails);
            this.tabControlTasks.Controls.Add(this.tpRedeemBonusForTicket);
            this.tabControlTasks.Controls.Add(this.tpPauseTime);
            this.tabControlTasks.Controls.Add(this.tpConvertCreditsToTime);
            this.tabControlTasks.Controls.Add(this.tpConvertTimeToPoints);
            this.tabControlTasks.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlTasks.Location = new System.Drawing.Point(0, 0);
            this.tabControlTasks.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControlTasks.Name = "tabControlTasks";
            this.tabControlTasks.SelectedIndex = 0;
            this.tabControlTasks.Size = new System.Drawing.Size(1162, 396);
            this.tabControlTasks.TabIndex = 0;
            // 
            // tabPageTransferCard
            // 
            this.tabPageTransferCard.Controls.Add(this.textBox1);
            this.tabPageTransferCard.Controls.Add(this.label28);
            this.tabPageTransferCard.Controls.Add(this.flowLayoutPanel1);
            this.tabPageTransferCard.Controls.Add(this.btnCustomerLookUp);
            this.tabPageTransferCard.Controls.Add(this.btnAlphanumericCancelndarTransferCard);
            this.tabPageTransferCard.Controls.Add(this.rb10DFormat);
            this.tabPageTransferCard.Controls.Add(this.rb10HFormat);
            this.tabPageTransferCard.Controls.Add(this.label10);
            this.tabPageTransferCard.Controls.Add(this.label9);
            this.tabPageTransferCard.Controls.Add(this.buttonTransferCardGetDetails);
            this.tabPageTransferCard.Controls.Add(this.textBoxTransferCardNumber);
            this.tabPageTransferCard.Controls.Add(this.label2);
            this.tabPageTransferCard.Location = new System.Drawing.Point(4, 25);
            this.tabPageTransferCard.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageTransferCard.Name = "tabPageTransferCard";
            this.tabPageTransferCard.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageTransferCard.Size = new System.Drawing.Size(1154, 367);
            this.tabPageTransferCard.TabIndex = 0;
            this.tabPageTransferCard.Tag = "TRANSFERCARD";
            this.tabPageTransferCard.Text = "Transfer Card";
            this.tabPageTransferCard.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox1.Location = new System.Drawing.Point(207, 160);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(177, 22);
            this.textBox1.TabIndex = 25;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(20, 176);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(105, 16);
            this.label28.TabIndex = 24;
            this.label28.Text = "Select Product:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Gray;
            this.flowLayoutPanel1.Controls.Add(this.sampleButtonCardSaleProduct);
            this.flowLayoutPanel1.Controls.Add(this.sampleButtonCardSaleSelectedProduct);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(23, 196);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(714, 164);
            this.flowLayoutPanel1.TabIndex = 23;
            // 
            // sampleButtonCardSaleProduct
            // 
            this.sampleButtonCardSaleProduct.BackColor = System.Drawing.Color.Transparent;
            this.sampleButtonCardSaleProduct.BackgroundImage = global::Parafait_POS.Properties.Resources.NewCard;
            this.sampleButtonCardSaleProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sampleButtonCardSaleProduct.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.sampleButtonCardSaleProduct.FlatAppearance.BorderSize = 0;
            this.sampleButtonCardSaleProduct.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonCardSaleProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonCardSaleProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonCardSaleProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sampleButtonCardSaleProduct.Font = new System.Drawing.Font("Tahoma", 9F);
            this.sampleButtonCardSaleProduct.ForeColor = System.Drawing.Color.Black;
            this.sampleButtonCardSaleProduct.Location = new System.Drawing.Point(3, 3);
            this.sampleButtonCardSaleProduct.Name = "sampleButtonCardSaleProduct";
            this.sampleButtonCardSaleProduct.Size = new System.Drawing.Size(120, 96);
            this.sampleButtonCardSaleProduct.TabIndex = 3;
            this.sampleButtonCardSaleProduct.Text = "Card Sale";
            this.sampleButtonCardSaleProduct.UseVisualStyleBackColor = false;
            this.sampleButtonCardSaleProduct.Visible = false;
            // 
            // sampleButtonCardSaleSelectedProduct
            // 
            this.sampleButtonCardSaleSelectedProduct.BackColor = System.Drawing.Color.Transparent;
            this.sampleButtonCardSaleSelectedProduct.BackgroundImage = global::Parafait_POS.Properties.Resources.Attraction;
            this.sampleButtonCardSaleSelectedProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sampleButtonCardSaleSelectedProduct.FlatAppearance.BorderSize = 0;
            this.sampleButtonCardSaleSelectedProduct.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonCardSaleSelectedProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonCardSaleSelectedProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonCardSaleSelectedProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sampleButtonCardSaleSelectedProduct.Font = new System.Drawing.Font("Tahoma", 9F);
            this.sampleButtonCardSaleSelectedProduct.ForeColor = System.Drawing.Color.White;
            this.sampleButtonCardSaleSelectedProduct.Location = new System.Drawing.Point(129, 3);
            this.sampleButtonCardSaleSelectedProduct.Name = "sampleButtonCardSaleSelectedProduct";
            this.sampleButtonCardSaleSelectedProduct.Size = new System.Drawing.Size(110, 70);
            this.sampleButtonCardSaleSelectedProduct.TabIndex = 3;
            this.sampleButtonCardSaleSelectedProduct.Text = "Card Sale";
            this.sampleButtonCardSaleSelectedProduct.UseVisualStyleBackColor = false;
            this.sampleButtonCardSaleSelectedProduct.Visible = false;
            // 
            // btnCustomerLookUp
            // 
            this.btnCustomerLookUp.BackColor = System.Drawing.Color.Transparent;
            this.btnCustomerLookUp.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnCustomerLookUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCustomerLookUp.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCustomerLookUp.FlatAppearance.BorderSize = 0;
            this.btnCustomerLookUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCustomerLookUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCustomerLookUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerLookUp.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerLookUp.ForeColor = System.Drawing.Color.White;
            this.btnCustomerLookUp.Location = new System.Drawing.Point(636, 39);
            this.btnCustomerLookUp.Name = "btnCustomerLookUp";
            this.btnCustomerLookUp.Size = new System.Drawing.Size(101, 23);
            this.btnCustomerLookUp.TabIndex = 22;
            this.btnCustomerLookUp.Text = "LookUp";
            this.btnCustomerLookUp.UseVisualStyleBackColor = false;
            this.btnCustomerLookUp.Click += new System.EventHandler(this.btnCustomerLookUp_Click);
            // 
            // btnAlphanumericCancelndarTransferCard
            // 
            this.btnAlphanumericCancelndarTransferCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAlphanumericCancelndarTransferCard.BackColor = System.Drawing.Color.Transparent;
            this.btnAlphanumericCancelndarTransferCard.CausesValidation = false;
            this.btnAlphanumericCancelndarTransferCard.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAlphanumericCancelndarTransferCard.FlatAppearance.BorderSize = 0;
            this.btnAlphanumericCancelndarTransferCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAlphanumericCancelndarTransferCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAlphanumericCancelndarTransferCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlphanumericCancelndarTransferCard.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlphanumericCancelndarTransferCard.ForeColor = System.Drawing.Color.Black;
            this.btnAlphanumericCancelndarTransferCard.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnAlphanumericCancelndarTransferCard.Location = new System.Drawing.Point(472, 27);
            this.btnAlphanumericCancelndarTransferCard.Name = "btnAlphanumericCancelndarTransferCard";
            this.btnAlphanumericCancelndarTransferCard.Size = new System.Drawing.Size(36, 40);
            this.btnAlphanumericCancelndarTransferCard.TabIndex = 21;
            this.btnAlphanumericCancelndarTransferCard.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAlphanumericCancelndarTransferCard.UseVisualStyleBackColor = false;
            this.btnAlphanumericCancelndarTransferCard.Click += new System.EventHandler(this.btnAlphanumericCancelndarTransferCard_Click);
            // 
            // rb10DFormat
            // 
            this.rb10DFormat.AutoSize = true;
            this.rb10DFormat.Location = new System.Drawing.Point(161, 12);
            this.rb10DFormat.Name = "rb10DFormat";
            this.rb10DFormat.Size = new System.Drawing.Size(52, 20);
            this.rb10DFormat.TabIndex = 6;
            this.rb10DFormat.Text = "Lost";
            this.rb10DFormat.UseVisualStyleBackColor = true;
            this.rb10DFormat.Click += new System.EventHandler(this.IsLostCardButton_click);
            // 
            // rb10HFormat
            // 
            this.rb10HFormat.AutoSize = true;
            this.rb10HFormat.Checked = true;
            this.rb10HFormat.Location = new System.Drawing.Point(23, 12);
            this.rb10HFormat.Name = "rb10HFormat";
            this.rb10HFormat.Size = new System.Drawing.Size(87, 20);
            this.rb10HFormat.TabIndex = 5;
            this.rb10HFormat.TabStop = true;
            this.rb10HFormat.Text = "Damaged";
            this.rb10HFormat.UseVisualStyleBackColor = true;
            this.rb10HFormat.Click += new System.EventHandler(this.IsDamagedCardButton_click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 160);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(181, 16);
            this.label10.TabIndex = 4;
            this.label10.Text = "To Card (Swipe NEW Card):";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 81);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 16);
            this.label9.TabIndex = 3;
            this.label9.Text = "From Card:";
            // 
            // buttonTransferCardGetDetails
            // 
            this.buttonTransferCardGetDetails.BackColor = System.Drawing.Color.Transparent;
            this.buttonTransferCardGetDetails.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.buttonTransferCardGetDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonTransferCardGetDetails.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonTransferCardGetDetails.FlatAppearance.BorderSize = 0;
            this.buttonTransferCardGetDetails.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonTransferCardGetDetails.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonTransferCardGetDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferCardGetDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTransferCardGetDetails.ForeColor = System.Drawing.Color.White;
            this.buttonTransferCardGetDetails.Location = new System.Drawing.Point(516, 39);
            this.buttonTransferCardGetDetails.Name = "buttonTransferCardGetDetails";
            this.buttonTransferCardGetDetails.Size = new System.Drawing.Size(114, 23);
            this.buttonTransferCardGetDetails.TabIndex = 2;
            this.buttonTransferCardGetDetails.Text = "Get Details";
            this.buttonTransferCardGetDetails.UseVisualStyleBackColor = false;
            this.buttonTransferCardGetDetails.Click += new System.EventHandler(this.buttonTransferCardGetDetails_Click);
            // 
            // textBoxTransferCardNumber
            // 
            this.textBoxTransferCardNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxTransferCardNumber.Location = new System.Drawing.Point(289, 40);
            this.textBoxTransferCardNumber.Name = "textBoxTransferCardNumber";
            this.textBoxTransferCardNumber.Size = new System.Drawing.Size(177, 22);
            this.textBoxTransferCardNumber.TabIndex = 1;
            this.textBoxTransferCardNumber.Enter += new System.EventHandler(this.textBoxTransferCardNumber_Enter);
            this.textBoxTransferCardNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxTransferCardNumber_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(263, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Enter Card Number/Swipe ISSUED Card:";
            // 
            // tabPageHoldEntitlement
            // 
            this.tabPageHoldEntitlement.Controls.Add(this.buttonClear);
            this.tabPageHoldEntitlement.Controls.Add(this.getHoldCardDetails);
            this.tabPageHoldEntitlement.Controls.Add(this.label27);
            this.tabPageHoldEntitlement.Controls.Add(this.btnAlphanumericCancelndarHoldEntitlement);
            this.tabPageHoldEntitlement.Controls.Add(this.textBoxHoldEntitlementNumber);
            this.tabPageHoldEntitlement.Location = new System.Drawing.Point(4, 25);
            this.tabPageHoldEntitlement.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageHoldEntitlement.Name = "tabPageHoldEntitlement";
            this.tabPageHoldEntitlement.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageHoldEntitlement.Size = new System.Drawing.Size(1154, 367);
            this.tabPageHoldEntitlement.TabIndex = 0;
            this.tabPageHoldEntitlement.Tag = "HOLDENTITLEMENTS";
            this.tabPageHoldEntitlement.Text = "Hold Entitlement";
            this.tabPageHoldEntitlement.UseVisualStyleBackColor = true;
            // 
            // buttonClear
            // 
            this.buttonClear.BackColor = System.Drawing.Color.Transparent;
            this.buttonClear.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.buttonClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonClear.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonClear.FlatAppearance.BorderSize = 0;
            this.buttonClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClear.ForeColor = System.Drawing.Color.White;
            this.buttonClear.Location = new System.Drawing.Point(634, 39);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(82, 23);
            this.buttonClear.TabIndex = 24;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = false;
            this.buttonClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // getHoldCardDetails
            // 
            this.getHoldCardDetails.BackColor = System.Drawing.Color.Transparent;
            this.getHoldCardDetails.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.getHoldCardDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.getHoldCardDetails.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.getHoldCardDetails.FlatAppearance.BorderSize = 0;
            this.getHoldCardDetails.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.getHoldCardDetails.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.getHoldCardDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.getHoldCardDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.getHoldCardDetails.ForeColor = System.Drawing.Color.White;
            this.getHoldCardDetails.Location = new System.Drawing.Point(514, 39);
            this.getHoldCardDetails.Name = "getHoldCardDetails";
            this.getHoldCardDetails.Size = new System.Drawing.Size(114, 23);
            this.getHoldCardDetails.TabIndex = 23;
            this.getHoldCardDetails.Text = "Get Details";
            this.getHoldCardDetails.UseVisualStyleBackColor = false;
            this.getHoldCardDetails.Click += new System.EventHandler(this.ButtonGetCardDetails_Click);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(20, 43);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(263, 16);
            this.label27.TabIndex = 22;
            this.label27.Text = "Enter Card Number/Swipe ISSUED Card:";
            // 
            // btnAlphanumericCancelndarHoldEntitlement
            // 
            this.btnAlphanumericCancelndarHoldEntitlement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAlphanumericCancelndarHoldEntitlement.BackColor = System.Drawing.Color.Transparent;
            this.btnAlphanumericCancelndarHoldEntitlement.CausesValidation = false;
            this.btnAlphanumericCancelndarHoldEntitlement.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAlphanumericCancelndarHoldEntitlement.FlatAppearance.BorderSize = 0;
            this.btnAlphanumericCancelndarHoldEntitlement.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAlphanumericCancelndarHoldEntitlement.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAlphanumericCancelndarHoldEntitlement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlphanumericCancelndarHoldEntitlement.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlphanumericCancelndarHoldEntitlement.ForeColor = System.Drawing.Color.Black;
            this.btnAlphanumericCancelndarHoldEntitlement.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnAlphanumericCancelndarHoldEntitlement.Location = new System.Drawing.Point(472, 25);
            this.btnAlphanumericCancelndarHoldEntitlement.Name = "btnAlphanumericCancelndarHoldEntitlement";
            this.btnAlphanumericCancelndarHoldEntitlement.Size = new System.Drawing.Size(36, 40);
            this.btnAlphanumericCancelndarHoldEntitlement.TabIndex = 21;
            this.btnAlphanumericCancelndarHoldEntitlement.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAlphanumericCancelndarHoldEntitlement.UseVisualStyleBackColor = false;
            this.btnAlphanumericCancelndarHoldEntitlement.Click += new System.EventHandler(this.btnAlphanumericCancelndarHoldEntitlement_Click);
            // 
            // textBoxHoldEntitlementNumber
            // 
            this.textBoxHoldEntitlementNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxHoldEntitlementNumber.Location = new System.Drawing.Point(289, 40);
            this.textBoxHoldEntitlementNumber.Name = "textBoxHoldEntitlementNumber";
            this.textBoxHoldEntitlementNumber.Size = new System.Drawing.Size(177, 22);
            this.textBoxHoldEntitlementNumber.TabIndex = 1;
            this.textBoxHoldEntitlementNumber.Enter += new System.EventHandler(this.textBoxHoldEntitlementNumber_Enter);
            this.textBoxHoldEntitlementNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxHoldEntitlementNumber_KeyPress);
            // 
            // tabPageExchangeTokenForCredit
            // 
            this.tabPageExchangeTokenForCredit.Controls.Add(this.txtCreditsAdded);
            this.tabPageExchangeTokenForCredit.Controls.Add(this.label7);
            this.tabPageExchangeTokenForCredit.Controls.Add(this.txtTokensExchanged);
            this.tabPageExchangeTokenForCredit.Controls.Add(this.label4);
            this.tabPageExchangeTokenForCredit.Location = new System.Drawing.Point(4, 25);
            this.tabPageExchangeTokenForCredit.Name = "tabPageExchangeTokenForCredit";
            this.tabPageExchangeTokenForCredit.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExchangeTokenForCredit.Size = new System.Drawing.Size(1154, 367);
            this.tabPageExchangeTokenForCredit.TabIndex = 3;
            this.tabPageExchangeTokenForCredit.Tag = "EXCHANGETOKENFORCREDIT";
            this.tabPageExchangeTokenForCredit.Text = "Exchange Token For Credit";
            this.tabPageExchangeTokenForCredit.UseVisualStyleBackColor = true;
            // 
            // txtCreditsAdded
            // 
            this.txtCreditsAdded.Location = new System.Drawing.Point(270, 177);
            this.txtCreditsAdded.Name = "txtCreditsAdded";
            this.txtCreditsAdded.ReadOnly = true;
            this.txtCreditsAdded.Size = new System.Drawing.Size(100, 22);
            this.txtCreditsAdded.TabIndex = 3;
            this.txtCreditsAdded.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(137, 180);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 16);
            this.label7.TabIndex = 2;
            this.label7.Text = "Credits Exchanged:";
            // 
            // txtTokensExchanged
            // 
            this.txtTokensExchanged.Location = new System.Drawing.Point(270, 135);
            this.txtTokensExchanged.Name = "txtTokensExchanged";
            this.txtTokensExchanged.Size = new System.Drawing.Size(100, 22);
            this.txtTokensExchanged.TabIndex = 1;
            this.txtTokensExchanged.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTokensExchanged.Enter += new System.EventHandler(this.txtTokensExchanged_Enter);
            this.txtTokensExchanged.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTokensExchanged_KeyPress);
            this.txtTokensExchanged.Validating += new System.ComponentModel.CancelEventHandler(this.txtTokensExchanged_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(76, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(190, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Number of Tokens Tendered:";
            // 
            // tabPageExchangeCreditForToken
            // 
            this.tabPageExchangeCreditForToken.Controls.Add(this.txtCreditsRequired);
            this.tabPageExchangeCreditForToken.Controls.Add(this.label5);
            this.tabPageExchangeCreditForToken.Controls.Add(this.txtTokensBought);
            this.tabPageExchangeCreditForToken.Controls.Add(this.label11);
            this.tabPageExchangeCreditForToken.Location = new System.Drawing.Point(4, 25);
            this.tabPageExchangeCreditForToken.Name = "tabPageExchangeCreditForToken";
            this.tabPageExchangeCreditForToken.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExchangeCreditForToken.Size = new System.Drawing.Size(1154, 367);
            this.tabPageExchangeCreditForToken.TabIndex = 4;
            this.tabPageExchangeCreditForToken.Tag = "EXCHANGECREDITFORTOKEN";
            this.tabPageExchangeCreditForToken.Text = "Exchange Credit For Token";
            this.tabPageExchangeCreditForToken.UseVisualStyleBackColor = true;
            // 
            // txtCreditsRequired
            // 
            this.txtCreditsRequired.Location = new System.Drawing.Point(270, 177);
            this.txtCreditsRequired.Name = "txtCreditsRequired";
            this.txtCreditsRequired.ReadOnly = true;
            this.txtCreditsRequired.Size = new System.Drawing.Size(100, 22);
            this.txtCreditsRequired.TabIndex = 7;
            this.txtCreditsRequired.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(150, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Credits Required:";
            // 
            // txtTokensBought
            // 
            this.txtTokensBought.Location = new System.Drawing.Point(270, 135);
            this.txtTokensBought.Name = "txtTokensBought";
            this.txtTokensBought.Size = new System.Drawing.Size(100, 22);
            this.txtTokensBought.TabIndex = 5;
            this.txtTokensBought.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTokensBought.Enter += new System.EventHandler(this.txtTokensBought_Enter);
            this.txtTokensBought.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTokensBought_KeyPress);
            this.txtTokensBought.Validating += new System.ComponentModel.CancelEventHandler(this.txtTokensBought_Validating);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(97, 138);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(170, 16);
            this.label11.TabIndex = 4;
            this.label11.Text = "Number of Tokens to Buy:";
            // 
            // tabPageLoadTickets
            // 
            this.tabPageLoadTickets.Controls.Add(this.lblScannedTickets);
            this.tabPageLoadTickets.Controls.Add(this.lblEnterTicketsToLoad);
            this.tabPageLoadTickets.Controls.Add(this.textBoxLoadTickets);
            this.tabPageLoadTickets.Location = new System.Drawing.Point(4, 25);
            this.tabPageLoadTickets.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageLoadTickets.Name = "tabPageLoadTickets";
            this.tabPageLoadTickets.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageLoadTickets.Size = new System.Drawing.Size(1154, 367);
            this.tabPageLoadTickets.TabIndex = 1;
            this.tabPageLoadTickets.Tag = "LOADTICKETS";
            this.tabPageLoadTickets.Text = "Load Tickets";
            this.tabPageLoadTickets.UseVisualStyleBackColor = true;
            // 
            // lblScannedTickets
            // 
            this.lblScannedTickets.AutoSize = true;
            this.lblScannedTickets.Location = new System.Drawing.Point(169, 227);
            this.lblScannedTickets.Name = "lblScannedTickets";
            this.lblScannedTickets.Size = new System.Drawing.Size(105, 16);
            this.lblScannedTickets.TabIndex = 4;
            this.lblScannedTickets.Text = "scannedTickets";
            // 
            // lblEnterTicketsToLoad
            // 
            this.lblEnterTicketsToLoad.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnterTicketsToLoad.Location = new System.Drawing.Point(20, 204);
            this.lblEnterTicketsToLoad.Name = "lblEnterTicketsToLoad";
            this.lblEnterTicketsToLoad.Size = new System.Drawing.Size(145, 39);
            this.lblEnterTicketsToLoad.TabIndex = 3;
            this.lblEnterTicketsToLoad.Text = "Enter Tickets to Load:\r\n[or Scan Receipts]";
            // 
            // textBoxLoadTickets
            // 
            this.textBoxLoadTickets.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLoadTickets.Location = new System.Drawing.Point(169, 201);
            this.textBoxLoadTickets.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxLoadTickets.Name = "textBoxLoadTickets";
            this.textBoxLoadTickets.Size = new System.Drawing.Size(116, 22);
            this.textBoxLoadTickets.TabIndex = 2;
            this.textBoxLoadTickets.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxLoadTickets.Enter += new System.EventHandler(this.textBoxLoadTickets_Enter);
            this.textBoxLoadTickets.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxLoadTickets_KeyPress);
            // 
            // tabPageConsolidate
            // 
            this.tabPageConsolidate.AutoScroll = true;
            this.tabPageConsolidate.Location = new System.Drawing.Point(4, 25);
            this.tabPageConsolidate.Name = "tabPageConsolidate";
            this.tabPageConsolidate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConsolidate.Size = new System.Drawing.Size(1154, 367);
            this.tabPageConsolidate.TabIndex = 2;
            this.tabPageConsolidate.Tag = "CONSOLIDATE";
            this.tabPageConsolidate.Text = "Consolidate Multiple Cards";
            this.tabPageConsolidate.UseVisualStyleBackColor = true;
            // 
            // tabPageBalanceTransfer
            // 
            this.tabPageBalanceTransfer.Controls.Add(this.dgvBalanceTransferee);
            this.tabPageBalanceTransfer.Controls.Add(this.lblTransfereeCardDetails);
            this.tabPageBalanceTransfer.Controls.Add(this.lblTransfererCardDetails);
            this.tabPageBalanceTransfer.Location = new System.Drawing.Point(4, 25);
            this.tabPageBalanceTransfer.Name = "tabPageBalanceTransfer";
            this.tabPageBalanceTransfer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBalanceTransfer.Size = new System.Drawing.Size(1154, 367);
            this.tabPageBalanceTransfer.TabIndex = 15;
            this.tabPageBalanceTransfer.Tag = "BALANCETRANSFER";
            this.tabPageBalanceTransfer.Text = "Balance Transfer";
            this.tabPageBalanceTransfer.UseVisualStyleBackColor = true;
            // 
            // dgvBalanceTransferee
            // 
            this.dgvBalanceTransferee.AllowUserToAddRows = false;
            this.dgvBalanceTransferee.AllowUserToDeleteRows = false;
            this.dgvBalanceTransferee.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBalanceTransferee.BackgroundColor = System.Drawing.Color.White;
            this.dgvBalanceTransferee.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvBalanceTransferee.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBalanceTransferee.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcTrToCardNumber,
            this.dcTrToCredits,
            this.dcTrToBonus,
            this.dcTrToTickets,
            this.dcTransferCredits,
            this.dcTransferBonus,
            this.dcTransferTickets,
            this.dcTrToCardId});
            this.dgvBalanceTransferee.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvBalanceTransferee.EnableHeadersVisualStyles = false;
            this.dgvBalanceTransferee.GridColor = System.Drawing.Color.DimGray;
            this.dgvBalanceTransferee.Location = new System.Drawing.Point(11, 128);
            this.dgvBalanceTransferee.Name = "dgvBalanceTransferee";
            this.dgvBalanceTransferee.RowHeadersVisible = false;
            this.dgvBalanceTransferee.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvBalanceTransferee.Size = new System.Drawing.Size(672, 185);
            this.dgvBalanceTransferee.TabIndex = 15;
            this.dgvBalanceTransferee.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBalanceTransferee_CellEnter);
            this.dgvBalanceTransferee.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvBalanceTransferee_EditingControlShowing);
            // 
            // dcTrToCardNumber
            // 
            this.dcTrToCardNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dcTrToCardNumber.HeaderText = "Card Number";
            this.dcTrToCardNumber.Name = "dcTrToCardNumber";
            this.dcTrToCardNumber.ReadOnly = true;
            // 
            // dcTrToCredits
            // 
            this.dcTrToCredits.HeaderText = "Credits";
            this.dcTrToCredits.Name = "dcTrToCredits";
            this.dcTrToCredits.ReadOnly = true;
            this.dcTrToCredits.Width = 76;
            // 
            // dcTrToBonus
            // 
            this.dcTrToBonus.HeaderText = "Bonus";
            this.dcTrToBonus.Name = "dcTrToBonus";
            this.dcTrToBonus.ReadOnly = true;
            this.dcTrToBonus.Width = 71;
            // 
            // dcTrToTickets
            // 
            this.dcTrToTickets.HeaderText = "Tickets";
            this.dcTrToTickets.Name = "dcTrToTickets";
            this.dcTrToTickets.ReadOnly = true;
            this.dcTrToTickets.Width = 76;
            // 
            // dcTransferCredits
            // 
            this.dcTransferCredits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dcTransferCredits.DefaultCellStyle = dataGridViewCellStyle1;
            this.dcTransferCredits.HeaderText = "Transfer Credits";
            this.dcTransferCredits.Name = "dcTransferCredits";
            // 
            // dcTransferBonus
            // 
            this.dcTransferBonus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dcTransferBonus.DefaultCellStyle = dataGridViewCellStyle2;
            this.dcTransferBonus.HeaderText = "Transfer Bonus";
            this.dcTransferBonus.Name = "dcTransferBonus";
            // 
            // dcTransferTickets
            // 
            this.dcTransferTickets.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.dcTransferTickets.DefaultCellStyle = dataGridViewCellStyle3;
            this.dcTransferTickets.HeaderText = "Transfer Tickets";
            this.dcTransferTickets.Name = "dcTransferTickets";
            // 
            // dcTrToCardId
            // 
            this.dcTrToCardId.HeaderText = "CardId";
            this.dcTrToCardId.Name = "dcTrToCardId";
            this.dcTrToCardId.Visible = false;
            this.dcTrToCardId.Width = 62;
            // 
            // lblTransfereeCardDetails
            // 
            this.lblTransfereeCardDetails.AutoSize = true;
            this.lblTransfereeCardDetails.Location = new System.Drawing.Point(8, 108);
            this.lblTransfereeCardDetails.Name = "lblTransfereeCardDetails";
            this.lblTransfereeCardDetails.Size = new System.Drawing.Size(165, 16);
            this.lblTransfereeCardDetails.TabIndex = 6;
            this.lblTransfereeCardDetails.Text = "Transferee\'s Card Details";
            // 
            // lblTransfererCardDetails
            // 
            this.lblTransfererCardDetails.AutoSize = true;
            this.lblTransfererCardDetails.Location = new System.Drawing.Point(8, 12);
            this.lblTransfererCardDetails.Name = "lblTransfererCardDetails";
            this.lblTransfererCardDetails.Size = new System.Drawing.Size(162, 16);
            this.lblTransfererCardDetails.TabIndex = 5;
            this.lblTransfererCardDetails.Text = "Transferer\'s Card Details";
            // 
            // tabPageLoadMultiple
            // 
            this.tabPageLoadMultiple.Controls.Add(this.lblEntitlement);
            this.tabPageLoadMultiple.Controls.Add(this.groupBox1);
            this.tabPageLoadMultiple.Controls.Add(this.groupBox2);
            this.tabPageLoadMultiple.Controls.Add(this.groupBox3);
            this.tabPageLoadMultiple.Location = new System.Drawing.Point(4, 25);
            this.tabPageLoadMultiple.Name = "tabPageLoadMultiple";
            this.tabPageLoadMultiple.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLoadMultiple.Size = new System.Drawing.Size(1154, 367);
            this.tabPageLoadMultiple.TabIndex = 5;
            this.tabPageLoadMultiple.Tag = "LOADMULTIPLE";
            this.tabPageLoadMultiple.Text = "Load Multiple";
            this.tabPageLoadMultiple.UseVisualStyleBackColor = true;
            // 
            // lblEntitlement
            // 
            this.lblEntitlement.AutoSize = true;
            this.lblEntitlement.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntitlement.ForeColor = System.Drawing.Color.Red;
            this.lblEntitlement.Location = new System.Drawing.Point(8, 11);
            this.lblEntitlement.Name = "lblEntitlement";
            this.lblEntitlement.Size = new System.Drawing.Size(0, 31);
            this.lblEntitlement.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLoadCardSerialMapping);
            this.groupBox1.Controls.Add(this.txtToCardSerialNumber);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.txtFromCardSerialNumber);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Location = new System.Drawing.Point(3, 318);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(685, 47);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Numbers";
            // 
            // btnLoadCardSerialMapping
            // 
            this.btnLoadCardSerialMapping.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadCardSerialMapping.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLoadCardSerialMapping.BackgroundImage")));
            this.btnLoadCardSerialMapping.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLoadCardSerialMapping.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnLoadCardSerialMapping.FlatAppearance.BorderSize = 0;
            this.btnLoadCardSerialMapping.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnLoadCardSerialMapping.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLoadCardSerialMapping.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLoadCardSerialMapping.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadCardSerialMapping.ForeColor = System.Drawing.Color.White;
            this.btnLoadCardSerialMapping.Location = new System.Drawing.Point(449, 11);
            this.btnLoadCardSerialMapping.Name = "btnLoadCardSerialMapping";
            this.btnLoadCardSerialMapping.Size = new System.Drawing.Size(76, 34);
            this.btnLoadCardSerialMapping.TabIndex = 12;
            this.btnLoadCardSerialMapping.Text = "Load";
            this.btnLoadCardSerialMapping.UseVisualStyleBackColor = false;
            this.btnLoadCardSerialMapping.Click += new System.EventHandler(this.btnLoadCardSerialMapping_Click);
            // 
            // txtToCardSerialNumber
            // 
            this.txtToCardSerialNumber.Location = new System.Drawing.Point(275, 20);
            this.txtToCardSerialNumber.Name = "txtToCardSerialNumber";
            this.txtToCardSerialNumber.Size = new System.Drawing.Size(117, 22);
            this.txtToCardSerialNumber.TabIndex = 11;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(245, 23);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(27, 16);
            this.label23.TabIndex = 10;
            this.label23.Text = "To:";
            // 
            // txtFromCardSerialNumber
            // 
            this.txtFromCardSerialNumber.Location = new System.Drawing.Point(61, 20);
            this.txtFromCardSerialNumber.Name = "txtFromCardSerialNumber";
            this.txtFromCardSerialNumber.Size = new System.Drawing.Size(117, 22);
            this.txtFromCardSerialNumber.TabIndex = 9;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(13, 23);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(45, 16);
            this.label22.TabIndex = 8;
            this.label22.Text = "From:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRemoveProduct);
            this.groupBox2.Controls.Add(this.dgvProductsAdded);
            this.groupBox2.Controls.Add(this.btnChooseProduct);
            this.groupBox2.Location = new System.Drawing.Point(185, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(503, 309);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Products Added";
            // 
            // btnRemoveProduct
            // 
            this.btnRemoveProduct.BackColor = System.Drawing.Color.Transparent;
            this.btnRemoveProduct.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRemoveProduct.BackgroundImage")));
            this.btnRemoveProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRemoveProduct.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnRemoveProduct.FlatAppearance.BorderSize = 0;
            this.btnRemoveProduct.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRemoveProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRemoveProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRemoveProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveProduct.ForeColor = System.Drawing.Color.White;
            this.btnRemoveProduct.Location = new System.Drawing.Point(267, 270);
            this.btnRemoveProduct.Name = "btnRemoveProduct";
            this.btnRemoveProduct.Size = new System.Drawing.Size(155, 34);
            this.btnRemoveProduct.TabIndex = 5;
            this.btnRemoveProduct.Text = "Remove Product";
            this.btnRemoveProduct.UseVisualStyleBackColor = false;
            this.btnRemoveProduct.Click += new System.EventHandler(this.btnRemoveProduct_Click);
            // 
            // dgvProductsAdded
            // 
            this.dgvProductsAdded.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProductsAdded.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProductsAdded.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvProductsAdded.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductsAdded.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductId,
            this.CardProductName,
            this.Price,
            this.Credits,
            this.Bonus,
            this.Courtesy,
            this.Time,
            this.ProductType});
            this.dgvProductsAdded.EnableHeadersVisualStyles = false;
            this.dgvProductsAdded.Location = new System.Drawing.Point(2, 18);
            this.dgvProductsAdded.Name = "dgvProductsAdded";
            this.dgvProductsAdded.ReadOnly = true;
            this.dgvProductsAdded.RowHeadersVisible = false;
            this.dgvProductsAdded.Size = new System.Drawing.Size(498, 238);
            this.dgvProductsAdded.TabIndex = 1;
            // 
            // ProductId
            // 
            this.ProductId.HeaderText = "ProductId";
            this.ProductId.Name = "ProductId";
            this.ProductId.ReadOnly = true;
            this.ProductId.Visible = false;
            // 
            // CardProductName
            // 
            this.CardProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CardProductName.HeaderText = "Product";
            this.CardProductName.Name = "CardProductName";
            this.CardProductName.ReadOnly = true;
            // 
            // Price
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Price.DefaultCellStyle = dataGridViewCellStyle5;
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            // 
            // Credits
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Credits.DefaultCellStyle = dataGridViewCellStyle6;
            this.Credits.HeaderText = "Credits";
            this.Credits.Name = "Credits";
            this.Credits.ReadOnly = true;
            // 
            // Bonus
            // 
            this.Bonus.HeaderText = "Bonus";
            this.Bonus.Name = "Bonus";
            this.Bonus.ReadOnly = true;
            // 
            // Courtesy
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Courtesy.DefaultCellStyle = dataGridViewCellStyle7;
            this.Courtesy.HeaderText = "Courtesy";
            this.Courtesy.Name = "Courtesy";
            this.Courtesy.ReadOnly = true;
            this.Courtesy.Visible = false;
            // 
            // Time
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Time.DefaultCellStyle = dataGridViewCellStyle8;
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.Visible = false;
            // 
            // ProductType
            // 
            this.ProductType.HeaderText = "ProductType";
            this.ProductType.Name = "ProductType";
            this.ProductType.ReadOnly = true;
            this.ProductType.Visible = false;
            // 
            // btnChooseProduct
            // 
            this.btnChooseProduct.BackColor = System.Drawing.Color.Transparent;
            this.btnChooseProduct.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnChooseProduct.BackgroundImage")));
            this.btnChooseProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnChooseProduct.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnChooseProduct.FlatAppearance.BorderSize = 0;
            this.btnChooseProduct.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnChooseProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChooseProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChooseProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseProduct.ForeColor = System.Drawing.Color.White;
            this.btnChooseProduct.Location = new System.Drawing.Point(93, 270);
            this.btnChooseProduct.Name = "btnChooseProduct";
            this.btnChooseProduct.Size = new System.Drawing.Size(155, 34);
            this.btnChooseProduct.TabIndex = 2;
            this.btnChooseProduct.Text = "Choose Product";
            this.btnChooseProduct.UseVisualStyleBackColor = false;
            this.btnChooseProduct.Click += new System.EventHandler(this.btnChooseProduct_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvMultipleCards);
            this.groupBox3.Location = new System.Drawing.Point(3, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(178, 309);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Cards";
            // 
            // dgvMultipleCards
            // 
            this.dgvMultipleCards.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMultipleCards.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMultipleCards.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvMultipleCards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMultipleCards.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SerialNumber,
            this.Card_Number,
            this.BulkSerialNumber});
            this.dgvMultipleCards.EnableHeadersVisualStyles = false;
            this.dgvMultipleCards.Location = new System.Drawing.Point(3, 18);
            this.dgvMultipleCards.Name = "dgvMultipleCards";
            this.dgvMultipleCards.ReadOnly = true;
            this.dgvMultipleCards.RowHeadersVisible = false;
            this.dgvMultipleCards.Size = new System.Drawing.Size(171, 289);
            this.dgvMultipleCards.TabIndex = 0;
            // 
            // SerialNumber
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.SerialNumber.DefaultCellStyle = dataGridViewCellStyle10;
            this.SerialNumber.HeaderText = "Serial#";
            this.SerialNumber.Name = "SerialNumber";
            this.SerialNumber.ReadOnly = true;
            this.SerialNumber.Width = 50;
            // 
            // Card_Number
            // 
            this.Card_Number.HeaderText = "Card Number";
            this.Card_Number.Name = "Card_Number";
            this.Card_Number.ReadOnly = true;
            this.Card_Number.Width = 120;
            // 
            // BulkSerialNumber
            // 
            this.BulkSerialNumber.HeaderText = "Bulk Serial";
            this.BulkSerialNumber.Name = "BulkSerialNumber";
            this.BulkSerialNumber.ReadOnly = true;
            this.BulkSerialNumber.Visible = false;
            // 
            // tabPageRealETicket
            // 
            this.tabPageRealETicket.Controls.Add(this.grpRealEticket);
            this.tabPageRealETicket.Location = new System.Drawing.Point(4, 25);
            this.tabPageRealETicket.Name = "tabPageRealETicket";
            this.tabPageRealETicket.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRealETicket.Size = new System.Drawing.Size(1154, 367);
            this.tabPageRealETicket.TabIndex = 6;
            this.tabPageRealETicket.Tag = "REALETICKET";
            this.tabPageRealETicket.Text = "Real / e-Ticket";
            this.tabPageRealETicket.UseVisualStyleBackColor = true;
            // 
            // grpRealEticket
            // 
            this.grpRealEticket.Controls.Add(this.radioButtoneTicket);
            this.grpRealEticket.Controls.Add(this.radioButtonRealTicket);
            this.grpRealEticket.Location = new System.Drawing.Point(204, 146);
            this.grpRealEticket.Name = "grpRealEticket";
            this.grpRealEticket.Size = new System.Drawing.Size(285, 124);
            this.grpRealEticket.TabIndex = 2;
            this.grpRealEticket.TabStop = false;
            this.grpRealEticket.Text = "Change Ticket Mode";
            // 
            // radioButtoneTicket
            // 
            this.radioButtoneTicket.Location = new System.Drawing.Point(107, 82);
            this.radioButtoneTicket.Name = "radioButtoneTicket";
            this.radioButtoneTicket.Size = new System.Drawing.Size(97, 20);
            this.radioButtoneTicket.TabIndex = 1;
            this.radioButtoneTicket.Text = "e-Ticket";
            this.radioButtoneTicket.UseVisualStyleBackColor = true;
            // 
            // radioButtonRealTicket
            // 
            this.radioButtonRealTicket.AutoSize = true;
            this.radioButtonRealTicket.BackColor = System.Drawing.Color.SkyBlue;
            this.radioButtonRealTicket.Checked = true;
            this.radioButtonRealTicket.Location = new System.Drawing.Point(107, 38);
            this.radioButtonRealTicket.Name = "radioButtonRealTicket";
            this.radioButtonRealTicket.Size = new System.Drawing.Size(97, 20);
            this.radioButtonRealTicket.TabIndex = 0;
            this.radioButtonRealTicket.TabStop = true;
            this.radioButtonRealTicket.Text = "Real Ticket";
            this.radioButtonRealTicket.UseVisualStyleBackColor = false;
            this.radioButtonRealTicket.CheckedChanged += new System.EventHandler(this.radioButtonRealTicket_CheckedChanged);
            // 
            // tabPageRefundCard
            // 
            this.tabPageRefundCard.AutoScroll = true;
            this.tabPageRefundCard.Controls.Add(this.grpRefundTime);
            this.tabPageRefundCard.Controls.Add(this.grpRefundCredits);
            this.tabPageRefundCard.Controls.Add(this.grpRefundGames);
            this.tabPageRefundCard.Location = new System.Drawing.Point(4, 25);
            this.tabPageRefundCard.Name = "tabPageRefundCard";
            this.tabPageRefundCard.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRefundCard.Size = new System.Drawing.Size(1154, 367);
            this.tabPageRefundCard.TabIndex = 7;
            this.tabPageRefundCard.Tag = "REFUNDCARD";
            this.tabPageRefundCard.Text = "Refund Card";
            this.tabPageRefundCard.UseVisualStyleBackColor = true;
            // 
            // grpRefundTime
            // 
            this.grpRefundTime.Controls.Add(this.cmbLoadTimeAttributes);
            this.grpRefundTime.Controls.Add(this.label26);
            this.grpRefundTime.Controls.Add(this.label25);
            this.grpRefundTime.Controls.Add(this.txtRefundTime);
            this.grpRefundTime.Controls.Add(this.dgvRefundTime);
            this.grpRefundTime.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRefundTime.Location = new System.Drawing.Point(765, 71);
            this.grpRefundTime.Name = "grpRefundTime";
            this.grpRefundTime.Size = new System.Drawing.Size(385, 236);
            this.grpRefundTime.TabIndex = 15;
            this.grpRefundTime.TabStop = false;
            this.grpRefundTime.Text = "Refund Time";
            // 
            // cmbLoadTimeAttributes
            // 
            this.cmbLoadTimeAttributes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoadTimeAttributes.FormattingEnabled = true;
            this.cmbLoadTimeAttributes.Location = new System.Drawing.Point(279, 192);
            this.cmbLoadTimeAttributes.Name = "cmbLoadTimeAttributes";
            this.cmbLoadTimeAttributes.Size = new System.Drawing.Size(95, 22);
            this.cmbLoadTimeAttributes.TabIndex = 20;
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(213, 193);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(63, 22);
            this.label26.TabIndex = 15;
            this.label26.Text = "Load As:";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(6, 197);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(135, 16);
            this.label25.TabIndex = 14;
            this.label25.Text = "Amount to Refund:";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRefundTime
            // 
            this.txtRefundTime.Location = new System.Drawing.Point(143, 193);
            this.txtRefundTime.Name = "txtRefundTime";
            this.txtRefundTime.Size = new System.Drawing.Size(60, 20);
            this.txtRefundTime.TabIndex = 13;
            this.txtRefundTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRefundTime.Enter += new System.EventHandler(this.txtRefundTime_Enter);
            // 
            // dgvRefundTime
            // 
            this.dgvRefundTime.AllowUserToAddRows = false;
            this.dgvRefundTime.AllowUserToDeleteRows = false;
            this.dgvRefundTime.AllowUserToResizeColumns = false;
            this.dgvRefundTime.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRefundTime.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRefundTime.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkRefundTime});
            this.dgvRefundTime.Location = new System.Drawing.Point(9, 21);
            this.dgvRefundTime.Name = "dgvRefundTime";
            this.dgvRefundTime.ReadOnly = true;
            this.dgvRefundTime.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRefundTime.Size = new System.Drawing.Size(365, 128);
            this.dgvRefundTime.TabIndex = 13;
            this.dgvRefundTime.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRefundTime_CellContentClick);
            // 
            // chkRefundTime
            // 
            this.chkRefundTime.FalseValue = "0";
            this.chkRefundTime.HeaderText = "√";
            this.chkRefundTime.Name = "chkRefundTime";
            this.chkRefundTime.ReadOnly = true;
            this.chkRefundTime.TrueValue = "1";
            // 
            // grpRefundCredits
            // 
            this.grpRefundCredits.Controls.Add(this.dgvRefundCardData);
            this.grpRefundCredits.Controls.Add(this.label8);
            this.grpRefundCredits.Controls.Add(this.lblRefundAmount);
            this.grpRefundCredits.Controls.Add(this.txtRefundAmount);
            this.grpRefundCredits.Controls.Add(this.label19);
            this.grpRefundCredits.Controls.Add(this.chkMakeCardNewOnFullRefund);
            this.grpRefundCredits.Controls.Add(this.label21);
            this.grpRefundCredits.Controls.Add(this.lblTaxAmount);
            this.grpRefundCredits.Location = new System.Drawing.Point(7, 71);
            this.grpRefundCredits.Name = "grpRefundCredits";
            this.grpRefundCredits.Size = new System.Drawing.Size(292, 294);
            this.grpRefundCredits.TabIndex = 13;
            this.grpRefundCredits.TabStop = false;
            // 
            // dgvRefundCardData
            // 
            this.dgvRefundCardData.AllowUserToAddRows = false;
            this.dgvRefundCardData.AllowUserToDeleteRows = false;
            this.dgvRefundCardData.AllowUserToResizeColumns = false;
            this.dgvRefundCardData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRefundCardData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRefundCardData.Location = new System.Drawing.Point(6, 21);
            this.dgvRefundCardData.Name = "dgvRefundCardData";
            this.dgvRefundCardData.ReadOnly = true;
            this.dgvRefundCardData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRefundCardData.Size = new System.Drawing.Size(271, 128);
            this.dgvRefundCardData.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(27, 220);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(163, 16);
            this.label8.TabIndex = 0;
            this.label8.Text = "Refund Amount:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRefundAmount
            // 
            this.lblRefundAmount.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRefundAmount.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblRefundAmount.Location = new System.Drawing.Point(186, 216);
            this.lblRefundAmount.Name = "lblRefundAmount";
            this.lblRefundAmount.Size = new System.Drawing.Size(96, 24);
            this.lblRefundAmount.TabIndex = 1;
            this.lblRefundAmount.Text = "0";
            this.lblRefundAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRefundAmount
            // 
            this.txtRefundAmount.Location = new System.Drawing.Point(194, 160);
            this.txtRefundAmount.Name = "txtRefundAmount";
            this.txtRefundAmount.Size = new System.Drawing.Size(83, 22);
            this.txtRefundAmount.TabIndex = 3;
            this.txtRefundAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRefundAmount.TextChanged += new System.EventHandler(this.txtRefundAmount_TextChanged);
            this.txtRefundAmount.Enter += new System.EventHandler(this.txtRefundAmount_Enter);
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(27, 163);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(163, 16);
            this.label19.TabIndex = 4;
            this.label19.Text = "Enter Amount to Refund:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkMakeCardNewOnFullRefund
            // 
            this.chkMakeCardNewOnFullRefund.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkMakeCardNewOnFullRefund.Location = new System.Drawing.Point(6, 250);
            this.chkMakeCardNewOnFullRefund.Name = "chkMakeCardNewOnFullRefund";
            this.chkMakeCardNewOnFullRefund.Size = new System.Drawing.Size(271, 20);
            this.chkMakeCardNewOnFullRefund.TabIndex = 7;
            this.chkMakeCardNewOnFullRefund.Text = "Make Card NEW on full refund";
            this.chkMakeCardNewOnFullRefund.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkMakeCardNewOnFullRefund.UseVisualStyleBackColor = true;
            this.chkMakeCardNewOnFullRefund.CheckedChanged += new System.EventHandler(this.chkMakeCardNewOnFullRefund_CheckedChanged);
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(106, 196);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(84, 16);
            this.label21.TabIndex = 5;
            this.label21.Text = "Tax:";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTaxAmount
            // 
            this.lblTaxAmount.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxAmount.Location = new System.Drawing.Point(197, 196);
            this.lblTaxAmount.Name = "lblTaxAmount";
            this.lblTaxAmount.Size = new System.Drawing.Size(83, 16);
            this.lblTaxAmount.TabIndex = 6;
            this.lblTaxAmount.Text = "0";
            this.lblTaxAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpRefundGames
            // 
            this.grpRefundGames.Controls.Add(this.cmbLoadGameAttributes);
            this.grpRefundGames.Controls.Add(this.label17);
            this.grpRefundGames.Controls.Add(this.lblLoadGameAttributes);
            this.grpRefundGames.Controls.Add(this.dgvRefundBalanceGames);
            this.grpRefundGames.Controls.Add(this.txtRefundGameAmount);
            this.grpRefundGames.Controls.Add(this.label16);
            this.grpRefundGames.Controls.Add(this.lblRefundGameCount);
            this.grpRefundGames.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRefundGames.Location = new System.Drawing.Point(306, 71);
            this.grpRefundGames.Name = "grpRefundGames";
            this.grpRefundGames.Size = new System.Drawing.Size(453, 236);
            this.grpRefundGames.TabIndex = 14;
            this.grpRefundGames.TabStop = false;
            this.grpRefundGames.Text = "Refund Games";
            // 
            // cmbLoadGameAttributes
            // 
            this.cmbLoadGameAttributes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoadGameAttributes.FormattingEnabled = true;
            this.cmbLoadGameAttributes.Location = new System.Drawing.Point(264, 191);
            this.cmbLoadGameAttributes.Name = "cmbLoadGameAttributes";
            this.cmbLoadGameAttributes.Size = new System.Drawing.Size(95, 22);
            this.cmbLoadGameAttributes.TabIndex = 19;
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(5, 195);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(135, 16);
            this.label17.TabIndex = 12;
            this.label17.Text = "Amount to Refund:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLoadGameAttributes
            // 
            this.lblLoadGameAttributes.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoadGameAttributes.Location = new System.Drawing.Point(200, 193);
            this.lblLoadGameAttributes.Name = "lblLoadGameAttributes";
            this.lblLoadGameAttributes.Size = new System.Drawing.Size(63, 22);
            this.lblLoadGameAttributes.TabIndex = 17;
            this.lblLoadGameAttributes.Text = "Load As:";
            this.lblLoadGameAttributes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvRefundBalanceGames
            // 
            this.dgvRefundBalanceGames.AllowUserToAddRows = false;
            this.dgvRefundBalanceGames.AllowUserToDeleteRows = false;
            this.dgvRefundBalanceGames.AllowUserToOrderColumns = true;
            this.dgvRefundBalanceGames.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRefundBalanceGames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRefundBalanceGames.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectRefundGame});
            this.dgvRefundBalanceGames.Location = new System.Drawing.Point(6, 21);
            this.dgvRefundBalanceGames.MultiSelect = false;
            this.dgvRefundBalanceGames.Name = "dgvRefundBalanceGames";
            this.dgvRefundBalanceGames.ReadOnly = true;
            this.dgvRefundBalanceGames.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRefundBalanceGames.Size = new System.Drawing.Size(444, 128);
            this.dgvRefundBalanceGames.TabIndex = 8;
            this.dgvRefundBalanceGames.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRefundBalanceGames_CellContentClick);
            // 
            // SelectRefundGame
            // 
            this.SelectRefundGame.FalseValue = "0";
            this.SelectRefundGame.HeaderText = "√";
            this.SelectRefundGame.Name = "SelectRefundGame";
            this.SelectRefundGame.ReadOnly = true;
            this.SelectRefundGame.TrueValue = "1";
            // 
            // txtRefundGameAmount
            // 
            this.txtRefundGameAmount.Location = new System.Drawing.Point(140, 192);
            this.txtRefundGameAmount.Name = "txtRefundGameAmount";
            this.txtRefundGameAmount.Size = new System.Drawing.Size(60, 20);
            this.txtRefundGameAmount.TabIndex = 11;
            this.txtRefundGameAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRefundGameAmount.Enter += new System.EventHandler(this.txtRefundGameAmount_Enter);
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(353, 195);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(72, 16);
            this.label16.TabIndex = 9;
            this.label16.Text = "#Games:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRefundGameCount
            // 
            this.lblRefundGameCount.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRefundGameCount.Location = new System.Drawing.Point(411, 195);
            this.lblRefundGameCount.Name = "lblRefundGameCount";
            this.lblRefundGameCount.Size = new System.Drawing.Size(36, 16);
            this.lblRefundGameCount.TabIndex = 10;
            this.lblRefundGameCount.Text = "0";
            this.lblRefundGameCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPageLoadBonus
            // 
            this.tabPageLoadBonus.Controls.Add(this.panelLoadBonusManualCard);
            this.tabPageLoadBonus.Controls.Add(this.grpLoadBonusTypes);
            this.tabPageLoadBonus.Controls.Add(this.label3);
            this.tabPageLoadBonus.Controls.Add(this.txtLoadBonus);
            this.tabPageLoadBonus.Location = new System.Drawing.Point(4, 25);
            this.tabPageLoadBonus.Name = "tabPageLoadBonus";
            this.tabPageLoadBonus.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLoadBonus.Size = new System.Drawing.Size(1154, 367);
            this.tabPageLoadBonus.TabIndex = 8;
            this.tabPageLoadBonus.Tag = "LOADBONUS";
            this.tabPageLoadBonus.Text = "Load Bonus";
            this.tabPageLoadBonus.UseVisualStyleBackColor = true;
            // 
            // panelLoadBonusManualCard
            // 
            this.panelLoadBonusManualCard.Controls.Add(this.btnGetLoadBonusManualCard);
            this.panelLoadBonusManualCard.Controls.Add(this.btnLoadBonusAlphaKeypad);
            this.panelLoadBonusManualCard.Controls.Add(this.label1);
            this.panelLoadBonusManualCard.Controls.Add(this.txtLoadBonusManualCardNumber);
            this.panelLoadBonusManualCard.Location = new System.Drawing.Point(27, 5);
            this.panelLoadBonusManualCard.Name = "panelLoadBonusManualCard";
            this.panelLoadBonusManualCard.Size = new System.Drawing.Size(619, 36);
            this.panelLoadBonusManualCard.TabIndex = 26;
            // 
            // btnGetLoadBonusManualCard
            // 
            this.btnGetLoadBonusManualCard.BackColor = System.Drawing.Color.Transparent;
            this.btnGetLoadBonusManualCard.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnGetLoadBonusManualCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGetLoadBonusManualCard.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnGetLoadBonusManualCard.FlatAppearance.BorderSize = 0;
            this.btnGetLoadBonusManualCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGetLoadBonusManualCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGetLoadBonusManualCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetLoadBonusManualCard.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetLoadBonusManualCard.ForeColor = System.Drawing.Color.White;
            this.btnGetLoadBonusManualCard.Location = new System.Drawing.Point(444, 3);
            this.btnGetLoadBonusManualCard.Name = "btnGetLoadBonusManualCard";
            this.btnGetLoadBonusManualCard.Size = new System.Drawing.Size(114, 23);
            this.btnGetLoadBonusManualCard.TabIndex = 24;
            this.btnGetLoadBonusManualCard.Text = "Get Details";
            this.btnGetLoadBonusManualCard.UseVisualStyleBackColor = false;
            this.btnGetLoadBonusManualCard.Click += new System.EventHandler(this.btnGetLoadBonusManualCard_Click);
            // 
            // btnLoadBonusAlphaKeypad
            // 
            this.btnLoadBonusAlphaKeypad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadBonusAlphaKeypad.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadBonusAlphaKeypad.CausesValidation = false;
            this.btnLoadBonusAlphaKeypad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnLoadBonusAlphaKeypad.FlatAppearance.BorderSize = 0;
            this.btnLoadBonusAlphaKeypad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLoadBonusAlphaKeypad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLoadBonusAlphaKeypad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadBonusAlphaKeypad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadBonusAlphaKeypad.ForeColor = System.Drawing.Color.Black;
            this.btnLoadBonusAlphaKeypad.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnLoadBonusAlphaKeypad.Location = new System.Drawing.Point(400, -5);
            this.btnLoadBonusAlphaKeypad.Name = "btnLoadBonusAlphaKeypad";
            this.btnLoadBonusAlphaKeypad.Size = new System.Drawing.Size(36, 33);
            this.btnLoadBonusAlphaKeypad.TabIndex = 25;
            this.btnLoadBonusAlphaKeypad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoadBonusAlphaKeypad.UseVisualStyleBackColor = false;
            this.btnLoadBonusAlphaKeypad.Click += new System.EventHandler(this.btnLoadBonusAlphaKeypad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 16);
            this.label1.TabIndex = 22;
            this.label1.Text = "Enter Card Number:";
            // 
            // txtLoadBonusManualCardNumber
            // 
            this.txtLoadBonusManualCardNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLoadBonusManualCardNumber.Location = new System.Drawing.Point(217, 4);
            this.txtLoadBonusManualCardNumber.Name = "txtLoadBonusManualCardNumber";
            this.txtLoadBonusManualCardNumber.Size = new System.Drawing.Size(177, 22);
            this.txtLoadBonusManualCardNumber.TabIndex = 23;
            // 
            // grpLoadBonusTypes
            // 
            this.grpLoadBonusTypes.Controls.Add(this.flpLoadBonusTypes);
            this.grpLoadBonusTypes.Location = new System.Drawing.Point(208, 112);
            this.grpLoadBonusTypes.Name = "grpLoadBonusTypes";
            this.grpLoadBonusTypes.Size = new System.Drawing.Size(261, 155);
            this.grpLoadBonusTypes.TabIndex = 6;
            this.grpLoadBonusTypes.TabStop = false;
            this.grpLoadBonusTypes.Text = "Choose Bonus Type (Non-Refundable)";
            // 
            // flpLoadBonusTypes
            // 
            this.flpLoadBonusTypes.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpLoadBonusTypes.Location = new System.Drawing.Point(6, 22);
            this.flpLoadBonusTypes.Name = "flpLoadBonusTypes";
            this.flpLoadBonusTypes.Size = new System.Drawing.Size(249, 127);
            this.flpLoadBonusTypes.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(211, 284);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Enter Bonus to Load:";
            // 
            // txtLoadBonus
            // 
            this.txtLoadBonus.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoadBonus.Location = new System.Drawing.Point(353, 281);
            this.txtLoadBonus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLoadBonus.Name = "txtLoadBonus";
            this.txtLoadBonus.Size = new System.Drawing.Size(116, 22);
            this.txtLoadBonus.TabIndex = 4;
            this.txtLoadBonus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLoadBonus.Enter += new System.EventHandler(this.txtLoadBonus_Enter);
            // 
            // tabPageDiscount
            // 
            this.tabPageDiscount.Controls.Add(this.dgvCardDiscountsDTOList);
            this.tabPageDiscount.Controls.Add(this.groupBox4);
            this.tabPageDiscount.Controls.Add(this.btnAddDiscount);
            this.tabPageDiscount.Controls.Add(this.label12);
            this.tabPageDiscount.Controls.Add(this.cmbDiscount);
            this.tabPageDiscount.Location = new System.Drawing.Point(4, 25);
            this.tabPageDiscount.Name = "tabPageDiscount";
            this.tabPageDiscount.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDiscount.Size = new System.Drawing.Size(1154, 367);
            this.tabPageDiscount.TabIndex = 9;
            this.tabPageDiscount.Tag = "DISCOUNT";
            this.tabPageDiscount.Text = "Discount";
            this.tabPageDiscount.UseVisualStyleBackColor = true;
            // 
            // dgvCardDiscountsDTOList
            // 
            this.dgvCardDiscountsDTOList.AllowUserToAddRows = false;
            this.dgvCardDiscountsDTOList.AllowUserToDeleteRows = false;
            this.dgvCardDiscountsDTOList.AutoGenerateColumns = false;
            this.dgvCardDiscountsDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCardDiscountsDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCardDiscountsDTOListCardDiscountIdTextBoxColumn,
            this.dgvCardDiscountsDTOListDiscountIdComboBoxColumn,
            this.dgvCardDiscountsDTOListExpiryDateTextBoxColumn,
            this.dgvCardDiscountsDTOListLastUpdatedUserTextBoxColumn,
            this.dgvCardDiscountsDTOListLastUpdatedDateTextBoxColumn,
            this.dgvCardDiscountsDTOListIsActiveCheckBoxColumn});
            this.dgvCardDiscountsDTOList.DataSource = this.cardDiscountsDTOListBS;
            this.dgvCardDiscountsDTOList.Location = new System.Drawing.Point(8, 154);
            this.dgvCardDiscountsDTOList.Name = "dgvCardDiscountsDTOList";
            this.dgvCardDiscountsDTOList.Size = new System.Drawing.Size(1138, 150);
            this.dgvCardDiscountsDTOList.TabIndex = 7;
            // 
            // dgvCardDiscountsDTOListCardDiscountIdTextBoxColumn
            // 
            this.dgvCardDiscountsDTOListCardDiscountIdTextBoxColumn.DataPropertyName = "CardDiscountId";
            this.dgvCardDiscountsDTOListCardDiscountIdTextBoxColumn.HeaderText = "Id";
            this.dgvCardDiscountsDTOListCardDiscountIdTextBoxColumn.Name = "dgvCardDiscountsDTOListCardDiscountIdTextBoxColumn";
            this.dgvCardDiscountsDTOListCardDiscountIdTextBoxColumn.ReadOnly = true;
            this.dgvCardDiscountsDTOListCardDiscountIdTextBoxColumn.Visible = false;
            // 
            // dgvCardDiscountsDTOListDiscountIdComboBoxColumn
            // 
            this.dgvCardDiscountsDTOListDiscountIdComboBoxColumn.DataPropertyName = "DiscountId";
            this.dgvCardDiscountsDTOListDiscountIdComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.dgvCardDiscountsDTOListDiscountIdComboBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvCardDiscountsDTOListDiscountIdComboBoxColumn.HeaderText = "Discount Name";
            this.dgvCardDiscountsDTOListDiscountIdComboBoxColumn.Name = "dgvCardDiscountsDTOListDiscountIdComboBoxColumn";
            this.dgvCardDiscountsDTOListDiscountIdComboBoxColumn.ReadOnly = true;
            this.dgvCardDiscountsDTOListDiscountIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCardDiscountsDTOListDiscountIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvCardDiscountsDTOListExpiryDateTextBoxColumn
            // 
            this.dgvCardDiscountsDTOListExpiryDateTextBoxColumn.DataPropertyName = "ExpiryDate";
            this.dgvCardDiscountsDTOListExpiryDateTextBoxColumn.HeaderText = "Expiry Date";
            this.dgvCardDiscountsDTOListExpiryDateTextBoxColumn.Name = "dgvCardDiscountsDTOListExpiryDateTextBoxColumn";
            this.dgvCardDiscountsDTOListExpiryDateTextBoxColumn.ReadOnly = true;
            // 
            // dgvCardDiscountsDTOListLastUpdatedUserTextBoxColumn
            // 
            this.dgvCardDiscountsDTOListLastUpdatedUserTextBoxColumn.DataPropertyName = "LastUpdatedUser";
            this.dgvCardDiscountsDTOListLastUpdatedUserTextBoxColumn.HeaderText = "Last Updated User";
            this.dgvCardDiscountsDTOListLastUpdatedUserTextBoxColumn.Name = "dgvCardDiscountsDTOListLastUpdatedUserTextBoxColumn";
            this.dgvCardDiscountsDTOListLastUpdatedUserTextBoxColumn.ReadOnly = true;
            this.dgvCardDiscountsDTOListLastUpdatedUserTextBoxColumn.Visible = false;
            // 
            // dgvCardDiscountsDTOListLastUpdatedDateTextBoxColumn
            // 
            this.dgvCardDiscountsDTOListLastUpdatedDateTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.dgvCardDiscountsDTOListLastUpdatedDateTextBoxColumn.HeaderText = "Last Updated Date";
            this.dgvCardDiscountsDTOListLastUpdatedDateTextBoxColumn.Name = "dgvCardDiscountsDTOListLastUpdatedDateTextBoxColumn";
            this.dgvCardDiscountsDTOListLastUpdatedDateTextBoxColumn.ReadOnly = true;
            this.dgvCardDiscountsDTOListLastUpdatedDateTextBoxColumn.Visible = false;
            // 
            // dgvCardDiscountsDTOListIsActiveCheckBoxColumn
            // 
            this.dgvCardDiscountsDTOListIsActiveCheckBoxColumn.DataPropertyName = "IsActive";
            this.dgvCardDiscountsDTOListIsActiveCheckBoxColumn.FalseValue = "N";
            this.dgvCardDiscountsDTOListIsActiveCheckBoxColumn.HeaderText = "Active";
            this.dgvCardDiscountsDTOListIsActiveCheckBoxColumn.IndeterminateValue = "N";
            this.dgvCardDiscountsDTOListIsActiveCheckBoxColumn.Name = "dgvCardDiscountsDTOListIsActiveCheckBoxColumn";
            this.dgvCardDiscountsDTOListIsActiveCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCardDiscountsDTOListIsActiveCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvCardDiscountsDTOListIsActiveCheckBoxColumn.TrueValue = "Y";
            // 
            // cardDiscountsDTOListBS
            // 
            this.cardDiscountsDTOListBS.DataSource = typeof(Semnox.Parafait.CardCore.CardDiscountsDTO);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbExpires);
            this.groupBox4.Controls.Add(this.rbNever);
            this.groupBox4.Controls.Add(this.dtpDiscountExpiryDate);
            this.groupBox4.Location = new System.Drawing.Point(381, 15);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(231, 75);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Expiry";
            // 
            // rbExpires
            // 
            this.rbExpires.AutoSize = true;
            this.rbExpires.Location = new System.Drawing.Point(10, 49);
            this.rbExpires.Name = "rbExpires";
            this.rbExpires.Size = new System.Drawing.Size(14, 13);
            this.rbExpires.TabIndex = 4;
            this.rbExpires.TabStop = true;
            this.rbExpires.UseVisualStyleBackColor = true;
            // 
            // rbNever
            // 
            this.rbNever.AutoSize = true;
            this.rbNever.Location = new System.Drawing.Point(10, 22);
            this.rbNever.Name = "rbNever";
            this.rbNever.Size = new System.Drawing.Size(63, 20);
            this.rbNever.TabIndex = 3;
            this.rbNever.TabStop = true;
            this.rbNever.Text = "Never";
            this.rbNever.UseVisualStyleBackColor = true;
            this.rbNever.CheckedChanged += new System.EventHandler(this.rbNever_CheckedChanged);
            // 
            // dtpDiscountExpiryDate
            // 
            this.dtpDiscountExpiryDate.CustomFormat = "dd-MMM-yyyy hh:mm tt";
            this.dtpDiscountExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDiscountExpiryDate.Location = new System.Drawing.Point(40, 44);
            this.dtpDiscountExpiryDate.Name = "dtpDiscountExpiryDate";
            this.dtpDiscountExpiryDate.ShowUpDown = true;
            this.dtpDiscountExpiryDate.Size = new System.Drawing.Size(185, 22);
            this.dtpDiscountExpiryDate.TabIndex = 2;
            // 
            // btnAddDiscount
            // 
            this.btnAddDiscount.BackColor = System.Drawing.Color.Transparent;
            this.btnAddDiscount.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnAddDiscount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddDiscount.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnAddDiscount.FlatAppearance.BorderSize = 0;
            this.btnAddDiscount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddDiscount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddDiscount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddDiscount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddDiscount.ForeColor = System.Drawing.Color.White;
            this.btnAddDiscount.Location = new System.Drawing.Point(176, 67);
            this.btnAddDiscount.Name = "btnAddDiscount";
            this.btnAddDiscount.Size = new System.Drawing.Size(108, 23);
            this.btnAddDiscount.TabIndex = 5;
            this.btnAddDiscount.Text = "Add";
            this.btnAddDiscount.UseVisualStyleBackColor = false;
            this.btnAddDiscount.Click += new System.EventHandler(this.btnAddDiscount_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(66, 15);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(66, 16);
            this.label12.TabIndex = 4;
            this.label12.Text = "Discount:";
            // 
            // cmbDiscount
            // 
            this.cmbDiscount.FormattingEnabled = true;
            this.cmbDiscount.Location = new System.Drawing.Point(69, 34);
            this.cmbDiscount.Name = "cmbDiscount";
            this.cmbDiscount.Size = new System.Drawing.Size(215, 24);
            this.cmbDiscount.TabIndex = 1;
            // 
            // tabPageRedeemLoyalty
            // 
            this.tabPageRedeemLoyalty.Controls.Add(this.label15);
            this.tabPageRedeemLoyalty.Controls.Add(this.btnGetRedemptionValues);
            this.tabPageRedeemLoyalty.Controls.Add(this.dgvLoyaltyRedemption);
            this.tabPageRedeemLoyalty.Controls.Add(this.txtLoyaltyRedeemPoints);
            this.tabPageRedeemLoyalty.Controls.Add(this.label14);
            this.tabPageRedeemLoyalty.Controls.Add(this.txtLoyaltyPoints);
            this.tabPageRedeemLoyalty.Controls.Add(this.label13);
            this.tabPageRedeemLoyalty.Location = new System.Drawing.Point(4, 25);
            this.tabPageRedeemLoyalty.Name = "tabPageRedeemLoyalty";
            this.tabPageRedeemLoyalty.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRedeemLoyalty.Size = new System.Drawing.Size(1154, 367);
            this.tabPageRedeemLoyalty.TabIndex = 10;
            this.tabPageRedeemLoyalty.Tag = "REDEEMLOYALTY";
            this.tabPageRedeemLoyalty.Text = "Redeem Loyalty Points";
            this.tabPageRedeemLoyalty.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(41, 119);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(113, 16);
            this.label15.TabIndex = 11;
            this.label15.Text = "Choose Attribute";
            // 
            // btnGetRedemptionValues
            // 
            this.btnGetRedemptionValues.BackColor = System.Drawing.Color.Transparent;
            this.btnGetRedemptionValues.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnGetRedemptionValues.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGetRedemptionValues.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnGetRedemptionValues.FlatAppearance.BorderSize = 0;
            this.btnGetRedemptionValues.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGetRedemptionValues.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGetRedemptionValues.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetRedemptionValues.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetRedemptionValues.ForeColor = System.Drawing.Color.White;
            this.btnGetRedemptionValues.Location = new System.Drawing.Point(260, 79);
            this.btnGetRedemptionValues.Name = "btnGetRedemptionValues";
            this.btnGetRedemptionValues.Size = new System.Drawing.Size(75, 23);
            this.btnGetRedemptionValues.TabIndex = 10;
            this.btnGetRedemptionValues.Text = "Refresh";
            this.btnGetRedemptionValues.UseVisualStyleBackColor = false;
            this.btnGetRedemptionValues.Click += new System.EventHandler(this.btnGetRedemptionValues_Click);
            // 
            // dgvLoyaltyRedemption
            // 
            this.dgvLoyaltyRedemption.AllowUserToAddRows = false;
            this.dgvLoyaltyRedemption.AllowUserToDeleteRows = false;
            this.dgvLoyaltyRedemption.AllowUserToOrderColumns = true;
            this.dgvLoyaltyRedemption.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLoyaltyRedemption.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected});
            this.dgvLoyaltyRedemption.Location = new System.Drawing.Point(44, 137);
            this.dgvLoyaltyRedemption.Name = "dgvLoyaltyRedemption";
            this.dgvLoyaltyRedemption.ReadOnly = true;
            this.dgvLoyaltyRedemption.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLoyaltyRedemption.Size = new System.Drawing.Size(637, 176);
            this.dgvLoyaltyRedemption.TabIndex = 9;
            this.dgvLoyaltyRedemption.SelectionChanged += new System.EventHandler(this.dgvLoyaltyRedemption_SelectionChanged);
            // 
            // Selected
            // 
            this.Selected.FalseValue = "N";
            this.Selected.HeaderText = "Select";
            this.Selected.Name = "Selected";
            this.Selected.ReadOnly = true;
            this.Selected.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Selected.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Selected.TrueValue = "Y";
            // 
            // txtLoyaltyRedeemPoints
            // 
            this.txtLoyaltyRedeemPoints.Location = new System.Drawing.Point(160, 79);
            this.txtLoyaltyRedeemPoints.Name = "txtLoyaltyRedeemPoints";
            this.txtLoyaltyRedeemPoints.Size = new System.Drawing.Size(94, 22);
            this.txtLoyaltyRedeemPoints.TabIndex = 8;
            this.txtLoyaltyRedeemPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLoyaltyRedeemPoints.Enter += new System.EventHandler(this.txtLoyaltyRedeemPoints_Enter);
            this.txtLoyaltyRedeemPoints.Validating += new System.ComponentModel.CancelEventHandler(this.txtLoyaltyRedeemPoints_Validating);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(34, 82);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(124, 16);
            this.label14.TabIndex = 7;
            this.label14.Text = "Points to Redeem:";
            // 
            // txtLoyaltyPoints
            // 
            this.txtLoyaltyPoints.Enabled = false;
            this.txtLoyaltyPoints.Location = new System.Drawing.Point(160, 40);
            this.txtLoyaltyPoints.Name = "txtLoyaltyPoints";
            this.txtLoyaltyPoints.Size = new System.Drawing.Size(94, 22);
            this.txtLoyaltyPoints.TabIndex = 6;
            this.txtLoyaltyPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(43, 43);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(115, 16);
            this.label13.TabIndex = 5;
            this.label13.Text = "Points Available:";
            // 
            // tabPageRedeemVirtualLoyalty
            // 
            this.tabPageRedeemVirtualPoints.Location = new System.Drawing.Point(4, 25);
            this.tabPageRedeemVirtualPoints.Name = "tabPageRedeemVirtualLoyalty";
            this.tabPageRedeemVirtualPoints.Size = new System.Drawing.Size(1154, 367);
            this.tabPageRedeemVirtualPoints.TabIndex = 16;
            // 

            // tabPageRedeemVirtualLoyalty
            // 
            this.tabPageRedeemVirtualPoints.Controls.Add(this.labelVirtualPoint);
            this.tabPageRedeemVirtualPoints.Controls.Add(this.btnVirtualGetRedemptionValues);
            this.tabPageRedeemVirtualPoints.Controls.Add(this.dgvVirtualPointRedemption);
            this.tabPageRedeemVirtualPoints.Controls.Add(this.txtVirtualRedeemPoints);
            this.tabPageRedeemVirtualPoints.Controls.Add(this.labelVirtualPointRedeem);
            this.tabPageRedeemVirtualPoints.Controls.Add(this.txtVirtualPoints);
            this.tabPageRedeemVirtualPoints.Controls.Add(this.labelVirtualPointAvailable);
            this.tabPageRedeemVirtualPoints.Location = new System.Drawing.Point(4, 25);
            this.tabPageRedeemVirtualPoints.Name = "tabPageRedeemVirtualLoyalty";
            this.tabPageRedeemVirtualPoints.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRedeemVirtualPoints.Size = new System.Drawing.Size(1154, 367);
            this.tabPageRedeemVirtualPoints.TabIndex = 10;
            this.tabPageRedeemVirtualPoints.Tag = "REDEEMVIRTUALPOINTS";
            this.tabPageRedeemVirtualPoints.Text = "Redeem Virtual Points";
            this.tabPageRedeemVirtualPoints.UseVisualStyleBackColor = true;

            // 
            // labelVirtualPoint
            // 
            this.labelVirtualPoint.AutoSize = true;
            this.labelVirtualPoint.Location = new System.Drawing.Point(41, 119);
            this.labelVirtualPoint.Name = "labelVirtualPoint";
            this.labelVirtualPoint.Size = new System.Drawing.Size(113, 16);
            this.labelVirtualPoint.TabIndex = 11;
            this.labelVirtualPoint.Text = "Choose Attribute";

            // 
            // btnVirtualGetRedemptionValues
            // 
            this.btnVirtualGetRedemptionValues.BackColor = System.Drawing.Color.Transparent;
            this.btnVirtualGetRedemptionValues.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnVirtualGetRedemptionValues.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVirtualGetRedemptionValues.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnVirtualGetRedemptionValues.FlatAppearance.BorderSize = 0;
            this.btnVirtualGetRedemptionValues.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnVirtualGetRedemptionValues.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnVirtualGetRedemptionValues.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVirtualGetRedemptionValues.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVirtualGetRedemptionValues.ForeColor = System.Drawing.Color.White;
            this.btnVirtualGetRedemptionValues.Location = new System.Drawing.Point(260, 79);
            this.btnVirtualGetRedemptionValues.Name = "btnVirtualGetRedemptionValues";
            this.btnVirtualGetRedemptionValues.Size = new System.Drawing.Size(75, 23);
            this.btnVirtualGetRedemptionValues.TabIndex = 10;
            this.btnVirtualGetRedemptionValues.Text = "Refresh";
            this.btnVirtualGetRedemptionValues.UseVisualStyleBackColor = false;
            this.btnVirtualGetRedemptionValues.Click += new System.EventHandler(this.btnVirtualGetRedemptionValues_Click);

            // 
            // dgvVirtualLoyaltyRedemption
            // 
            this.dgvVirtualPointRedemption.AllowUserToAddRows = false;
            this.dgvVirtualPointRedemption.AllowUserToDeleteRows = false;
            this.dgvVirtualPointRedemption.AllowUserToOrderColumns = true;
            this.dgvVirtualPointRedemption.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVirtualPointRedemption.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectedVirtualPoint});
            this.dgvVirtualPointRedemption.Location = new System.Drawing.Point(44, 137);
            this.dgvVirtualPointRedemption.Name = "dgvVirtualLoyaltyRedemption";
            this.dgvVirtualPointRedemption.ReadOnly = true;
            this.dgvVirtualPointRedemption.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVirtualPointRedemption.Size = new System.Drawing.Size(637, 176);
            this.dgvVirtualPointRedemption.TabIndex = 9;
            this.dgvVirtualPointRedemption.SelectionChanged += new System.EventHandler(this.dgvVirtualLoyaltyRedemption_SelectionChanged);

            // 
            // txtVirtualLoyaltyRedeemPoints
            // 
            this.txtVirtualRedeemPoints.Location = new System.Drawing.Point(160, 79);
            this.txtVirtualRedeemPoints.Name = "txtVirtualLoyaltyRedeemPoints";
            this.txtVirtualRedeemPoints.Size = new System.Drawing.Size(94, 22);
            this.txtVirtualRedeemPoints.TabIndex = 8;
            this.txtVirtualRedeemPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtVirtualRedeemPoints.Enter += new System.EventHandler(this.txtVirtualLoyaltyRedeemPoints_Enter);
            this.txtVirtualRedeemPoints.Validating += new System.ComponentModel.CancelEventHandler(this.txtVirtualLoyaltyRedeemPoints_Validating);

            // 
            // labelVirtualPointRedeem
            // 
            this.labelVirtualPointRedeem.AutoSize = true;
            this.labelVirtualPointRedeem.Location = new System.Drawing.Point(34, 82);
            this.labelVirtualPointRedeem.Name = "labelVirtualPointRedeem";
            this.labelVirtualPointRedeem.Size = new System.Drawing.Size(124, 16);
            this.labelVirtualPointRedeem.TabIndex = 7;
            this.labelVirtualPointRedeem.Text = "Points to Redeem:";
            // 
            // txtVirtualLoyaltyPoints
            // 
            this.txtVirtualPoints.Enabled = false;
            this.txtVirtualPoints.Location = new System.Drawing.Point(160, 40);
            this.txtVirtualPoints.Name = "txtVirtualLoyaltyPoints";
            this.txtVirtualPoints.Size = new System.Drawing.Size(94, 22);
            this.txtVirtualPoints.TabIndex = 6;
            this.txtVirtualPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            // 
            // labelVirtualPointAvailable
            // 
            this.labelVirtualPointAvailable.AutoSize = true;
            this.labelVirtualPointAvailable.Location = new System.Drawing.Point(43, 43);
            this.labelVirtualPointAvailable.Name = "labelVirtualPointAvailable";
            this.labelVirtualPointAvailable.Size = new System.Drawing.Size(115, 16);
            this.labelVirtualPointAvailable.TabIndex = 5;
            this.labelVirtualPointAvailable.Text = "Points Available:";

            // 
            // SelectedVirtualPoint
            // 
            this.SelectedVirtualPoint.FalseValue = "N";
            this.SelectedVirtualPoint.HeaderText = "Select";
            this.SelectedVirtualPoint.Name = "SelectedVirtualPoint";
            this.SelectedVirtualPoint.ReadOnly = true;
            this.SelectedVirtualPoint.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SelectedVirtualPoint.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SelectedVirtualPoint.TrueValue = "Y";
            // 
            // 
            // tpSpecialPricing
            // 
            this.tpSpecialPricing.Controls.Add(this.btnClearPricing);
            this.tpSpecialPricing.Controls.Add(this.dgvSpecialPricing);
            this.tpSpecialPricing.Location = new System.Drawing.Point(4, 25);
            this.tpSpecialPricing.Name = "tpSpecialPricing";
            this.tpSpecialPricing.Padding = new System.Windows.Forms.Padding(3);
            this.tpSpecialPricing.Size = new System.Drawing.Size(1154, 367);
            this.tpSpecialPricing.TabIndex = 11;
            this.tpSpecialPricing.Tag = "SPECIALPRICING";
            this.tpSpecialPricing.Text = "Special Pricing";
            this.tpSpecialPricing.UseVisualStyleBackColor = true;
            // 
            // btnClearPricing
            // 
            this.btnClearPricing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearPricing.BackColor = System.Drawing.Color.Transparent;
            this.btnClearPricing.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnClearPricing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClearPricing.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClearPricing.FlatAppearance.BorderSize = 0;
            this.btnClearPricing.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClearPricing.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClearPricing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearPricing.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearPricing.ForeColor = System.Drawing.Color.White;
            this.btnClearPricing.Location = new System.Drawing.Point(528, 258);
            this.btnClearPricing.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClearPricing.Name = "btnClearPricing";
            this.btnClearPricing.Size = new System.Drawing.Size(155, 55);
            this.btnClearPricing.TabIndex = 2;
            this.btnClearPricing.Text = "Clear Pricing";
            this.btnClearPricing.UseVisualStyleBackColor = false;
            this.btnClearPricing.Click += new System.EventHandler(this.btnClearPricing_Click);
            // 
            // dgvSpecialPricing
            // 
            this.dgvSpecialPricing.AllowUserToAddRows = false;
            this.dgvSpecialPricing.AllowUserToDeleteRows = false;
            this.dgvSpecialPricing.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSpecialPricing.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSpecialPricing.ColumnHeadersVisible = false;
            this.dgvSpecialPricing.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PricingId,
            this.PricingName,
            this.Percentage,
            this.RequiresManagerApproval});
            this.dgvSpecialPricing.GridColor = System.Drawing.Color.White;
            this.dgvSpecialPricing.Location = new System.Drawing.Point(198, 29);
            this.dgvSpecialPricing.Name = "dgvSpecialPricing";
            this.dgvSpecialPricing.RowHeadersVisible = false;
            this.dgvSpecialPricing.RowTemplate.Height = 50;
            this.dgvSpecialPricing.Size = new System.Drawing.Size(307, 284);
            this.dgvSpecialPricing.TabIndex = 0;
            // 
            // PricingId
            // 
            this.PricingId.HeaderText = "PricingId";
            this.PricingId.Name = "PricingId";
            this.PricingId.Visible = false;
            // 
            // PricingName
            // 
            this.PricingName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PricingName.DefaultCellStyle = dataGridViewCellStyle11;
            this.PricingName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PricingName.HeaderText = "Special Pricing Name";
            this.PricingName.Name = "PricingName";
            // 
            // Percentage
            // 
            this.Percentage.HeaderText = "Percentage";
            this.Percentage.Name = "Percentage";
            this.Percentage.Visible = false;
            // 
            // RequiresManagerApproval
            // 
            this.RequiresManagerApproval.HeaderText = "RequiresManagerApproval";
            this.RequiresManagerApproval.Name = "RequiresManagerApproval";
            this.RequiresManagerApproval.Visible = false;
            // 
            // tpRedeemTicketsForBonus
            // 
            this.tpRedeemTicketsForBonus.Controls.Add(this.grpBoxTicketType);
            this.tpRedeemTicketsForBonus.Controls.Add(this.txtBonusEligible);
            this.tpRedeemTicketsForBonus.Controls.Add(this.lblBonusEligible);
            this.tpRedeemTicketsForBonus.Controls.Add(this.txtTicketsAvailable);
            this.tpRedeemTicketsForBonus.Controls.Add(this.lblTicketsAvlbl);
            this.tpRedeemTicketsForBonus.Controls.Add(this.btnRefreshBonus);
            this.tpRedeemTicketsForBonus.Controls.Add(this.txtTicketsToRedeem);
            this.tpRedeemTicketsForBonus.Controls.Add(this.lblTicketsToRedeem);
            this.tpRedeemTicketsForBonus.Location = new System.Drawing.Point(4, 25);
            this.tpRedeemTicketsForBonus.Name = "tpRedeemTicketsForBonus";
            this.tpRedeemTicketsForBonus.Padding = new System.Windows.Forms.Padding(3);
            this.tpRedeemTicketsForBonus.Size = new System.Drawing.Size(1154, 367);
            this.tpRedeemTicketsForBonus.TabIndex = 12;
            this.tpRedeemTicketsForBonus.Tag = "REDEEMTICKETSFORBONUS";
            this.tpRedeemTicketsForBonus.Text = "Redeem Tickets";
            this.tpRedeemTicketsForBonus.UseVisualStyleBackColor = true;
            // 
            // grpBoxTicketType
            // 
            this.grpBoxTicketType.Controls.Add(this.rbCardBalance);
            this.grpBoxTicketType.Controls.Add(this.rbBonus);
            this.grpBoxTicketType.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpBoxTicketType.Location = new System.Drawing.Point(240, 87);
            this.grpBoxTicketType.Name = "grpBoxTicketType";
            this.grpBoxTicketType.Size = new System.Drawing.Size(218, 115);
            this.grpBoxTicketType.TabIndex = 19;
            this.grpBoxTicketType.TabStop = false;
            this.grpBoxTicketType.Text = "Choose Redeem Type ";
            // 
            // rbCardBalance
            // 
            this.rbCardBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCardBalance.Location = new System.Drawing.Point(43, 69);
            this.rbCardBalance.Name = "rbCardBalance";
            this.rbCardBalance.Size = new System.Drawing.Size(155, 35);
            this.rbCardBalance.TabIndex = 1;
            this.rbCardBalance.Text = "Card Balance";
            this.rbCardBalance.UseVisualStyleBackColor = true;
            this.rbCardBalance.CheckedChanged += new System.EventHandler(this.rbCardBalance_CheckedChanged);
            // 
            // rbBonus
            // 
            this.rbBonus.AutoSize = true;
            this.rbBonus.BackColor = System.Drawing.Color.Transparent;
            this.rbBonus.Checked = true;
            this.rbBonus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBonus.Location = new System.Drawing.Point(43, 34);
            this.rbBonus.Name = "rbBonus";
            this.rbBonus.Size = new System.Drawing.Size(78, 24);
            this.rbBonus.TabIndex = 0;
            this.rbBonus.TabStop = true;
            this.rbBonus.Text = "Bonus";
            this.rbBonus.UseVisualStyleBackColor = false;
            this.rbBonus.CheckedChanged += new System.EventHandler(this.rbBonus_CheckedChanged);
            // 
            // txtBonusEligible
            // 
            this.txtBonusEligible.Enabled = false;
            this.txtBonusEligible.Location = new System.Drawing.Point(364, 269);
            this.txtBonusEligible.Name = "txtBonusEligible";
            this.txtBonusEligible.Size = new System.Drawing.Size(94, 22);
            this.txtBonusEligible.TabIndex = 17;
            this.txtBonusEligible.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblBonusEligible
            // 
            this.lblBonusEligible.Location = new System.Drawing.Point(203, 272);
            this.lblBonusEligible.Name = "lblBonusEligible";
            this.lblBonusEligible.Size = new System.Drawing.Size(159, 19);
            this.lblBonusEligible.TabIndex = 16;
            this.lblBonusEligible.Text = "Bonus Eligible:";
            this.lblBonusEligible.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtTicketsAvailable
            // 
            this.txtTicketsAvailable.Enabled = false;
            this.txtTicketsAvailable.Location = new System.Drawing.Point(364, 212);
            this.txtTicketsAvailable.Name = "txtTicketsAvailable";
            this.txtTicketsAvailable.Size = new System.Drawing.Size(94, 22);
            this.txtTicketsAvailable.TabIndex = 15;
            this.txtTicketsAvailable.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTicketsAvlbl
            // 
            this.lblTicketsAvlbl.Location = new System.Drawing.Point(186, 214);
            this.lblTicketsAvlbl.Name = "lblTicketsAvlbl";
            this.lblTicketsAvlbl.Size = new System.Drawing.Size(176, 20);
            this.lblTicketsAvlbl.TabIndex = 14;
            this.lblTicketsAvlbl.Text = "Tickets Available:";
            this.lblTicketsAvlbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnRefreshBonus
            // 
            this.btnRefreshBonus.BackColor = System.Drawing.Color.Transparent;
            this.btnRefreshBonus.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnRefreshBonus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefreshBonus.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnRefreshBonus.FlatAppearance.BorderSize = 0;
            this.btnRefreshBonus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefreshBonus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefreshBonus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshBonus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshBonus.ForeColor = System.Drawing.Color.White;
            this.btnRefreshBonus.Location = new System.Drawing.Point(466, 240);
            this.btnRefreshBonus.Name = "btnRefreshBonus";
            this.btnRefreshBonus.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshBonus.TabIndex = 13;
            this.btnRefreshBonus.Text = "Refresh";
            this.btnRefreshBonus.UseVisualStyleBackColor = false;
            this.btnRefreshBonus.Click += new System.EventHandler(this.btnRefreshBonus_Click);
            // 
            // txtTicketsToRedeem
            // 
            this.txtTicketsToRedeem.Location = new System.Drawing.Point(364, 240);
            this.txtTicketsToRedeem.Name = "txtTicketsToRedeem";
            this.txtTicketsToRedeem.Size = new System.Drawing.Size(94, 22);
            this.txtTicketsToRedeem.TabIndex = 12;
            this.txtTicketsToRedeem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTicketsToRedeem.Enter += new System.EventHandler(this.txtTicketsToRedeem_Enter);
            this.txtTicketsToRedeem.Validating += new System.ComponentModel.CancelEventHandler(this.txtTicketsToRedeem_Validating);
            // 
            // lblTicketsToRedeem
            // 
            this.lblTicketsToRedeem.Location = new System.Drawing.Point(133, 242);
            this.lblTicketsToRedeem.Name = "lblTicketsToRedeem";
            this.lblTicketsToRedeem.Size = new System.Drawing.Size(229, 20);
            this.lblTicketsToRedeem.TabIndex = 11;
            this.lblTicketsToRedeem.Text = "Tickets to Redeem:";
            this.lblTicketsToRedeem.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tpSetSiteCode
            // 
            this.tpSetSiteCode.Controls.Add(this.cbSiteCode);
            this.tpSetSiteCode.Controls.Add(this.btnChangeSiteCode);
            this.tpSetSiteCode.Controls.Add(this.txtCurrentSiteCode);
            this.tpSetSiteCode.Controls.Add(this.lblNewSiteCode);
            this.tpSetSiteCode.Controls.Add(this.lblCurrentSiteCode);
            this.tpSetSiteCode.Controls.Add(this.lblCardNumber);
            this.tpSetSiteCode.Controls.Add(this.txtCardNumber);
            this.tpSetSiteCode.Location = new System.Drawing.Point(4, 25);
            this.tpSetSiteCode.Name = "tpSetSiteCode";
            this.tpSetSiteCode.Padding = new System.Windows.Forms.Padding(3);
            this.tpSetSiteCode.Size = new System.Drawing.Size(1154, 367);
            this.tpSetSiteCode.TabIndex = 13;
            this.tpSetSiteCode.Tag = "SETCHILDSITECODE";
            this.tpSetSiteCode.Text = "Set Child Site Code";
            this.tpSetSiteCode.UseVisualStyleBackColor = true;
            // 
            // cbSiteCode
            // 
            this.cbSiteCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSiteCode.FormattingEnabled = true;
            this.cbSiteCode.Location = new System.Drawing.Point(254, 152);
            this.cbSiteCode.Name = "cbSiteCode";
            this.cbSiteCode.Size = new System.Drawing.Size(170, 24);
            this.cbSiteCode.TabIndex = 14;
            // 
            // btnChangeSiteCode
            // 
            this.btnChangeSiteCode.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnChangeSiteCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnChangeSiteCode.Enabled = false;
            this.btnChangeSiteCode.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnChangeSiteCode.FlatAppearance.BorderSize = 0;
            this.btnChangeSiteCode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChangeSiteCode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChangeSiteCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeSiteCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeSiteCode.ForeColor = System.Drawing.Color.White;
            this.btnChangeSiteCode.Location = new System.Drawing.Point(275, 195);
            this.btnChangeSiteCode.Name = "btnChangeSiteCode";
            this.btnChangeSiteCode.Size = new System.Drawing.Size(127, 45);
            this.btnChangeSiteCode.TabIndex = 13;
            this.btnChangeSiteCode.Text = "Change Site Code";
            this.btnChangeSiteCode.UseVisualStyleBackColor = false;
            this.btnChangeSiteCode.Click += new System.EventHandler(this.btnChangeSiteCode_Click);
            // 
            // txtCurrentSiteCode
            // 
            this.txtCurrentSiteCode.Location = new System.Drawing.Point(254, 119);
            this.txtCurrentSiteCode.Name = "txtCurrentSiteCode";
            this.txtCurrentSiteCode.ReadOnly = true;
            this.txtCurrentSiteCode.Size = new System.Drawing.Size(170, 22);
            this.txtCurrentSiteCode.TabIndex = 12;
            // 
            // lblNewSiteCode
            // 
            this.lblNewSiteCode.AutoSize = true;
            this.lblNewSiteCode.Location = new System.Drawing.Point(146, 156);
            this.lblNewSiteCode.Name = "lblNewSiteCode";
            this.lblNewSiteCode.Size = new System.Drawing.Size(106, 16);
            this.lblNewSiteCode.TabIndex = 11;
            this.lblNewSiteCode.Text = "New Site Code:";
            // 
            // lblCurrentSiteCode
            // 
            this.lblCurrentSiteCode.AutoSize = true;
            this.lblCurrentSiteCode.Location = new System.Drawing.Point(127, 122);
            this.lblCurrentSiteCode.Name = "lblCurrentSiteCode";
            this.lblCurrentSiteCode.Size = new System.Drawing.Size(125, 16);
            this.lblCurrentSiteCode.TabIndex = 10;
            this.lblCurrentSiteCode.Text = "Current Site Code:";
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.AutoSize = true;
            this.lblCardNumber.Location = new System.Drawing.Point(156, 88);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(96, 16);
            this.lblCardNumber.TabIndex = 9;
            this.lblCardNumber.Text = "Card Number:";
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Location = new System.Drawing.Point(254, 85);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.ReadOnly = true;
            this.txtCardNumber.Size = new System.Drawing.Size(170, 22);
            this.txtCardNumber.TabIndex = 8;
            // 
            // tpGetMiFareGPDetails
            // 
            this.tpGetMiFareGPDetails.Controls.Add(this.btnUpload);
            this.tpGetMiFareGPDetails.Controls.Add(this.btnValidate);
            this.tpGetMiFareGPDetails.Controls.Add(this.txtNoOfGamePlays);
            this.tpGetMiFareGPDetails.Controls.Add(this.lblNoOfGamePlays);
            this.tpGetMiFareGPDetails.Controls.Add(this.progressBar);
            this.tpGetMiFareGPDetails.Controls.Add(this.dgvCardDetails);
            this.tpGetMiFareGPDetails.Controls.Add(this.txtCardNumberMiFare);
            this.tpGetMiFareGPDetails.Controls.Add(this.label20);
            this.tpGetMiFareGPDetails.Location = new System.Drawing.Point(4, 25);
            this.tpGetMiFareGPDetails.Name = "tpGetMiFareGPDetails";
            this.tpGetMiFareGPDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tpGetMiFareGPDetails.Size = new System.Drawing.Size(1154, 367);
            this.tpGetMiFareGPDetails.TabIndex = 14;
            this.tpGetMiFareGPDetails.Tag = "GETMIFAREGAMEPLAY";
            this.tpGetMiFareGPDetails.Text = "Get MiFare Gameplay Details";
            this.tpGetMiFareGPDetails.UseVisualStyleBackColor = true;
            // 
            // btnUpload
            // 
            this.btnUpload.BackColor = System.Drawing.Color.Transparent;
            this.btnUpload.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpload.BackgroundImage")));
            this.btnUpload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUpload.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnUpload.FlatAppearance.BorderSize = 0;
            this.btnUpload.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnUpload.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnUpload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpload.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.ForeColor = System.Drawing.Color.White;
            this.btnUpload.Location = new System.Drawing.Point(415, 22);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(100, 28);
            this.btnUpload.TabIndex = 28;
            this.btnUpload.Text = "Upload ";
            this.btnUpload.UseVisualStyleBackColor = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.BackColor = System.Drawing.Color.Transparent;
            this.btnValidate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnValidate.BackgroundImage")));
            this.btnValidate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnValidate.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnValidate.FlatAppearance.BorderSize = 0;
            this.btnValidate.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnValidate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnValidate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnValidate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidate.ForeColor = System.Drawing.Color.White;
            this.btnValidate.Location = new System.Drawing.Point(291, 22);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(100, 28);
            this.btnValidate.TabIndex = 27;
            this.btnValidate.Text = "Validate";
            this.btnValidate.UseVisualStyleBackColor = false;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // txtNoOfGamePlays
            // 
            this.txtNoOfGamePlays.BackColor = System.Drawing.SystemColors.Control;
            this.txtNoOfGamePlays.Location = new System.Drawing.Point(161, 28);
            this.txtNoOfGamePlays.Name = "txtNoOfGamePlays";
            this.txtNoOfGamePlays.ReadOnly = true;
            this.txtNoOfGamePlays.Size = new System.Drawing.Size(101, 22);
            this.txtNoOfGamePlays.TabIndex = 25;
            this.txtNoOfGamePlays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblNoOfGamePlays
            // 
            this.lblNoOfGamePlays.AutoSize = true;
            this.lblNoOfGamePlays.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblNoOfGamePlays.Location = new System.Drawing.Point(35, 31);
            this.lblNoOfGamePlays.Name = "lblNoOfGamePlays";
            this.lblNoOfGamePlays.Size = new System.Drawing.Size(124, 16);
            this.lblNoOfGamePlays.TabIndex = 24;
            this.lblNoOfGamePlays.Text = "No. of Gameplays:";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(35, 307);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(623, 10);
            this.progressBar.TabIndex = 23;
            // 
            // dgvCardDetails
            // 
            this.dgvCardDetails.AllowUserToAddRows = false;
            this.dgvCardDetails.AllowUserToDeleteRows = false;
            this.dgvCardDetails.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dgvCardDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCardDetails.ColumnHeadersHeight = 30;
            this.dgvCardDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCardDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcSiteId,
            this.dcMachineAddress,
            this.dcStartBalance,
            this.dcEndBalance,
            this.dcComments,
            this.dcStatus});
            this.dgvCardDetails.Location = new System.Drawing.Point(35, 56);
            this.dgvCardDetails.Name = "dgvCardDetails";
            this.dgvCardDetails.ReadOnly = true;
            this.dgvCardDetails.RowHeadersVisible = false;
            this.dgvCardDetails.Size = new System.Drawing.Size(623, 246);
            this.dgvCardDetails.TabIndex = 22;
            this.dgvCardDetails.Tag = "";
            // 
            // dcSiteId
            // 
            this.dcSiteId.HeaderText = "Site ID";
            this.dcSiteId.Name = "dcSiteId";
            this.dcSiteId.ReadOnly = true;
            // 
            // dcMachineAddress
            // 
            this.dcMachineAddress.HeaderText = "Machine ID";
            this.dcMachineAddress.Name = "dcMachineAddress";
            this.dcMachineAddress.ReadOnly = true;
            // 
            // dcStartBalance
            // 
            this.dcStartBalance.HeaderText = "Start Balance";
            this.dcStartBalance.Name = "dcStartBalance";
            this.dcStartBalance.ReadOnly = true;
            // 
            // dcEndBalance
            // 
            this.dcEndBalance.HeaderText = "End Balance";
            this.dcEndBalance.Name = "dcEndBalance";
            this.dcEndBalance.ReadOnly = true;
            // 
            // dcComments
            // 
            this.dcComments.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dcComments.HeaderText = "Comments";
            this.dcComments.Name = "dcComments";
            this.dcComments.ReadOnly = true;
            // 
            // dcStatus
            // 
            this.dcStatus.HeaderText = "Status";
            this.dcStatus.Name = "dcStatus";
            this.dcStatus.ReadOnly = true;
            // 
            // txtCardNumberMiFare
            // 
            this.txtCardNumberMiFare.BackColor = System.Drawing.SystemColors.Control;
            this.txtCardNumberMiFare.Location = new System.Drawing.Point(161, 3);
            this.txtCardNumberMiFare.Name = "txtCardNumberMiFare";
            this.txtCardNumberMiFare.ReadOnly = true;
            this.txtCardNumberMiFare.Size = new System.Drawing.Size(101, 22);
            this.txtCardNumberMiFare.TabIndex = 21;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.label20.Location = new System.Drawing.Point(63, 6);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(96, 16);
            this.label20.TabIndex = 20;
            this.label20.Text = "Card Number:";
            // 
            // tpRedeemBonusForTicket
            // 
            this.tpRedeemBonusForTicket.Controls.Add(this.txtElgibleTickets);
            this.tpRedeemBonusForTicket.Controls.Add(this.lblTicketsElgible);
            this.tpRedeemBonusForTicket.Controls.Add(this.txtBonusAvailable);
            this.tpRedeemBonusForTicket.Controls.Add(this.lblBonusAvl);
            this.tpRedeemBonusForTicket.Controls.Add(this.btnBonusRefresh);
            this.tpRedeemBonusForTicket.Controls.Add(this.txtBonusToRedeem);
            this.tpRedeemBonusForTicket.Controls.Add(this.lblBonusToRedeem);
            this.tpRedeemBonusForTicket.Location = new System.Drawing.Point(4, 25);
            this.tpRedeemBonusForTicket.Name = "tpRedeemBonusForTicket";
            this.tpRedeemBonusForTicket.Padding = new System.Windows.Forms.Padding(3);
            this.tpRedeemBonusForTicket.Size = new System.Drawing.Size(1154, 367);
            this.tpRedeemBonusForTicket.TabIndex = 16;
            this.tpRedeemBonusForTicket.Tag = "REDEEMBONUSFORTICKET";
            this.tpRedeemBonusForTicket.Text = "Redeem Bonus For Ticket";
            this.tpRedeemBonusForTicket.UseVisualStyleBackColor = true;
            // 
            // txtElgibleTickets
            // 
            this.txtElgibleTickets.Enabled = false;
            this.txtElgibleTickets.Location = new System.Drawing.Point(323, 162);
            this.txtElgibleTickets.Name = "txtElgibleTickets";
            this.txtElgibleTickets.Size = new System.Drawing.Size(94, 22);
            this.txtElgibleTickets.TabIndex = 24;
            this.txtElgibleTickets.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTicketsElgible
            // 
            this.lblTicketsElgible.AutoSize = true;
            this.lblTicketsElgible.Location = new System.Drawing.Point(211, 164);
            this.lblTicketsElgible.Name = "lblTicketsElgible";
            this.lblTicketsElgible.Size = new System.Drawing.Size(108, 16);
            this.lblTicketsElgible.TabIndex = 23;
            this.lblTicketsElgible.Text = "Tickets Eligible:";
            // 
            // txtBonusAvailable
            // 
            this.txtBonusAvailable.Enabled = false;
            this.txtBonusAvailable.Location = new System.Drawing.Point(323, 101);
            this.txtBonusAvailable.Name = "txtBonusAvailable";
            this.txtBonusAvailable.Size = new System.Drawing.Size(94, 22);
            this.txtBonusAvailable.TabIndex = 22;
            this.txtBonusAvailable.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblBonusAvl
            // 
            this.lblBonusAvl.AutoSize = true;
            this.lblBonusAvl.Location = new System.Drawing.Point(204, 103);
            this.lblBonusAvl.Name = "lblBonusAvl";
            this.lblBonusAvl.Size = new System.Drawing.Size(115, 16);
            this.lblBonusAvl.TabIndex = 21;
            this.lblBonusAvl.Text = "Bonus Available:";
            // 
            // btnBonusRefresh
            // 
            this.btnBonusRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnBonusRefresh.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnBonusRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBonusRefresh.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnBonusRefresh.FlatAppearance.BorderSize = 0;
            this.btnBonusRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBonusRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBonusRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBonusRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBonusRefresh.ForeColor = System.Drawing.Color.White;
            this.btnBonusRefresh.Location = new System.Drawing.Point(430, 131);
            this.btnBonusRefresh.Name = "btnBonusRefresh";
            this.btnBonusRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnBonusRefresh.TabIndex = 20;
            this.btnBonusRefresh.Text = "Refresh";
            this.btnBonusRefresh.UseVisualStyleBackColor = false;
            this.btnBonusRefresh.Click += new System.EventHandler(this.btnBonusRefresh_Click);
            // 
            // txtBonusToRedeem
            // 
            this.txtBonusToRedeem.Location = new System.Drawing.Point(323, 132);
            this.txtBonusToRedeem.Name = "txtBonusToRedeem";
            this.txtBonusToRedeem.Size = new System.Drawing.Size(94, 22);
            this.txtBonusToRedeem.TabIndex = 19;
            this.txtBonusToRedeem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBonusToRedeem.Validating += new System.ComponentModel.CancelEventHandler(this.txtBonusToRedeem_Validating);
            // 
            // lblBonusToRedeem
            // 
            this.lblBonusToRedeem.AutoSize = true;
            this.lblBonusToRedeem.Location = new System.Drawing.Point(195, 134);
            this.lblBonusToRedeem.Name = "lblBonusToRedeem";
            this.lblBonusToRedeem.Size = new System.Drawing.Size(124, 16);
            this.lblBonusToRedeem.TabIndex = 18;
            this.lblBonusToRedeem.Text = "Bonus to Redeem:";
            // 
            // tpPauseTime
            // 
            this.tpPauseTime.Controls.Add(this.txteTicket);
            this.tpPauseTime.Controls.Add(this.lblPausetimeEticketBal);
            this.tpPauseTime.Controls.Add(this.textTimeRemaining);
            this.tpPauseTime.Controls.Add(this.label18);
            this.tpPauseTime.Controls.Add(this.label24);
            this.tpPauseTime.Controls.Add(this.textCardNo);
            this.tpPauseTime.Location = new System.Drawing.Point(4, 25);
            this.tpPauseTime.Name = "tpPauseTime";
            this.tpPauseTime.Padding = new System.Windows.Forms.Padding(3);
            this.tpPauseTime.Size = new System.Drawing.Size(1154, 367);
            this.tpPauseTime.TabIndex = 17;
            this.tpPauseTime.Tag = "PAUSETIMEENTITLEMENT";
            this.tpPauseTime.Text = "Pause Time";
            this.tpPauseTime.UseVisualStyleBackColor = true;
            // 
            // txteTicket
            // 
            this.txteTicket.Location = new System.Drawing.Point(258, 158);
            this.txteTicket.Name = "txteTicket";
            this.txteTicket.ReadOnly = true;
            this.txteTicket.Size = new System.Drawing.Size(170, 22);
            this.txteTicket.TabIndex = 18;
            // 
            // lblPausetimeEticketBal
            // 
            this.lblPausetimeEticketBal.AutoSize = true;
            this.lblPausetimeEticketBal.Location = new System.Drawing.Point(135, 161);
            this.lblPausetimeEticketBal.Name = "lblPausetimeEticketBal";
            this.lblPausetimeEticketBal.Size = new System.Drawing.Size(114, 16);
            this.lblPausetimeEticketBal.TabIndex = 17;
            this.lblPausetimeEticketBal.Text = "eTicket Balance:";
            // 
            // textTimeRemaining
            // 
            this.textTimeRemaining.Location = new System.Drawing.Point(258, 121);
            this.textTimeRemaining.Name = "textTimeRemaining";
            this.textTimeRemaining.ReadOnly = true;
            this.textTimeRemaining.Size = new System.Drawing.Size(170, 22);
            this.textTimeRemaining.TabIndex = 16;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(135, 124);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(117, 16);
            this.label18.TabIndex = 15;
            this.label18.Text = "Time Remaining:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(156, 88);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(96, 16);
            this.label24.TabIndex = 14;
            this.label24.Text = "Card Number:";
            // 
            // textCardNo
            // 
            this.textCardNo.Location = new System.Drawing.Point(258, 85);
            this.textCardNo.Name = "textCardNo";
            this.textCardNo.ReadOnly = true;
            this.textCardNo.Size = new System.Drawing.Size(170, 22);
            this.textCardNo.TabIndex = 13;
            // 
            // tpConvertCreditsToTime
            // 
            this.tpConvertCreditsToTime.Controls.Add(this.btnClearPointsToConvert);
            this.tpConvertCreditsToTime.Controls.Add(this.grpPointsToConvert);
            this.tpConvertCreditsToTime.Controls.Add(this.dgvTappedCard);
            this.tpConvertCreditsToTime.Location = new System.Drawing.Point(4, 25);
            this.tpConvertCreditsToTime.Name = "tpConvertCreditsToTime";
            this.tpConvertCreditsToTime.Padding = new System.Windows.Forms.Padding(3);
            this.tpConvertCreditsToTime.Size = new System.Drawing.Size(1154, 367);
            this.tpConvertCreditsToTime.TabIndex = 18;
            this.tpConvertCreditsToTime.Tag = "EXCHANGECREDITFORTIME";
            this.tpConvertCreditsToTime.Text = "Convert Points To Time";
            this.tpConvertCreditsToTime.UseVisualStyleBackColor = true;
            // 
            // btnClearPointsToConvert
            // 
            this.btnClearPointsToConvert.BackColor = System.Drawing.Color.Transparent;
            this.btnClearPointsToConvert.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClearPointsToConvert.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClearPointsToConvert.FlatAppearance.BorderSize = 0;
            this.btnClearPointsToConvert.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClearPointsToConvert.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClearPointsToConvert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClearPointsToConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearPointsToConvert.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearPointsToConvert.ForeColor = System.Drawing.Color.White;
            this.btnClearPointsToConvert.Location = new System.Drawing.Point(296, 303);
            this.btnClearPointsToConvert.Name = "btnClearPointsToConvert";
            this.btnClearPointsToConvert.Size = new System.Drawing.Size(100, 33);
            this.btnClearPointsToConvert.TabIndex = 20023;
            this.btnClearPointsToConvert.Text = "Clear";
            this.btnClearPointsToConvert.UseVisualStyleBackColor = false;
            this.btnClearPointsToConvert.Visible = false;
            this.btnClearPointsToConvert.Click += new System.EventHandler(this.btnClearPointsToConvert_Click);
            // 
            // grpPointsToConvert
            // 
            this.grpPointsToConvert.BackColor = System.Drawing.Color.Transparent;
            this.grpPointsToConvert.Controls.Add(this.dgvPointsToConvert);
            this.grpPointsToConvert.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPointsToConvert.Location = new System.Drawing.Point(86, 122);
            this.grpPointsToConvert.Name = "grpPointsToConvert";
            this.grpPointsToConvert.Size = new System.Drawing.Size(586, 156);
            this.grpPointsToConvert.TabIndex = 20022;
            this.grpPointsToConvert.TabStop = false;
            this.grpPointsToConvert.Text = "Points To Convert";
            // 
            // dgvPointsToConvert
            // 
            this.dgvPointsToConvert.AllowUserToAddRows = false;
            this.dgvPointsToConvert.AllowUserToDeleteRows = false;
            this.dgvPointsToConvert.AllowUserToResizeRows = false;
            this.dgvPointsToConvert.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dgvPointsToConvert.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPointsToConvert.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvPointsToConvert.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPointsToConvert.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcAdditionalPoints,
            this.dcAdditionalTime,
            this.dcBalancePoints});
            this.dgvPointsToConvert.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvPointsToConvert.EnableHeadersVisualStyles = false;
            this.dgvPointsToConvert.Location = new System.Drawing.Point(6, 19);
            this.dgvPointsToConvert.Name = "dgvPointsToConvert";
            this.dgvPointsToConvert.RowHeadersVisible = false;
            this.dgvPointsToConvert.RowTemplate.Height = 30;
            this.dgvPointsToConvert.Size = new System.Drawing.Size(574, 51);
            this.dgvPointsToConvert.TabIndex = 0;
            this.dgvPointsToConvert.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPointsToConvert_CellClick);
            this.dgvPointsToConvert.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPointsToConvert_CellValidated);
            // 
            // dcAdditionalPoints
            // 
            this.dcAdditionalPoints.HeaderText = "Points To Convert";
            this.dcAdditionalPoints.Name = "dcAdditionalPoints";
            this.dcAdditionalPoints.Width = 191;
            // 
            // dcAdditionalTime
            // 
            this.dcAdditionalTime.HeaderText = "New Time";
            this.dcAdditionalTime.Name = "dcAdditionalTime";
            this.dcAdditionalTime.ReadOnly = true;
            this.dcAdditionalTime.Width = 191;
            // 
            // dcBalancePoints
            // 
            this.dcBalancePoints.HeaderText = "Balance Points";
            this.dcBalancePoints.Name = "dcBalancePoints";
            this.dcBalancePoints.ReadOnly = true;
            this.dcBalancePoints.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dcBalancePoints.Width = 191;
            // 
            // dgvTappedCard
            // 
            this.dgvTappedCard.AllowUserToAddRows = false;
            this.dgvTappedCard.AllowUserToDeleteRows = false;
            this.dgvTappedCard.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dgvTappedCard.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTappedCard.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTappedCard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTappedCard.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParentCardNumber,
            this.Customer,
            this.ParentTime,
            this.ParentCredits,
            this.ParentCardId});
            this.dgvTappedCard.EnableHeadersVisualStyles = false;
            this.dgvTappedCard.Location = new System.Drawing.Point(86, 47);
            this.dgvTappedCard.Name = "dgvTappedCard";
            this.dgvTappedCard.ReadOnly = true;
            this.dgvTappedCard.RowHeadersVisible = false;
            this.dgvTappedCard.RowTemplate.Height = 30;
            this.dgvTappedCard.Size = new System.Drawing.Size(587, 50);
            this.dgvTappedCard.TabIndex = 20021;
            // 
            // ParentCardNumber
            // 
            this.ParentCardNumber.HeaderText = "Card Number";
            this.ParentCardNumber.Name = "ParentCardNumber";
            this.ParentCardNumber.ReadOnly = true;
            this.ParentCardNumber.Width = 146;
            // 
            // Customer
            // 
            this.Customer.HeaderText = "Customer";
            this.Customer.Name = "Customer";
            this.Customer.ReadOnly = true;
            this.Customer.Width = 146;
            // 
            // ParentTime
            // 
            this.ParentTime.HeaderText = "Time";
            this.ParentTime.Name = "ParentTime";
            this.ParentTime.ReadOnly = true;
            this.ParentTime.Width = 146;
            // 
            // ParentCredits
            // 
            this.ParentCredits.HeaderText = "Points";
            this.ParentCredits.Name = "ParentCredits";
            this.ParentCredits.ReadOnly = true;
            this.ParentCredits.Width = 146;
            // 
            // ParentCardId
            // 
            this.ParentCardId.HeaderText = "ParentCardId";
            this.ParentCardId.Name = "ParentCardId";
            this.ParentCardId.ReadOnly = true;
            this.ParentCardId.Visible = false;
            // 
            // tpConvertTimeToPoints
            // 
            this.tpConvertTimeToPoints.Controls.Add(this.groupBox5);
            this.tpConvertTimeToPoints.Controls.Add(this.dgvCardInfo);
            this.tpConvertTimeToPoints.Location = new System.Drawing.Point(4, 25);
            this.tpConvertTimeToPoints.Name = "tpConvertTimeToPoints";
            this.tpConvertTimeToPoints.Padding = new System.Windows.Forms.Padding(3);
            this.tpConvertTimeToPoints.Size = new System.Drawing.Size(1154, 367);
            this.tpConvertTimeToPoints.TabIndex = 19;
            this.tpConvertTimeToPoints.Tag = "EXCHANGETIMEFORCREDIT";
            this.tpConvertTimeToPoints.Text = "Convert Time To Points";
            this.tpConvertTimeToPoints.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.dgvTimeToConvert);
            this.groupBox5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(124, 124);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(586, 156);
            this.groupBox5.TabIndex = 20024;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Time To Convert";
            // 
            // dgvTimeToConvert
            // 
            this.dgvTimeToConvert.AllowUserToAddRows = false;
            this.dgvTimeToConvert.AllowUserToDeleteRows = false;
            this.dgvTimeToConvert.AllowUserToResizeRows = false;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvTimeToConvert.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvTimeToConvert.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dgvTimeToConvert.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTimeToConvert.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTimeToConvert.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTimeToConvert.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcNewTimeToConvert,
            this.dcNewPoints,
            this.dcNewTimeBalance});
            this.dgvTimeToConvert.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvTimeToConvert.EnableHeadersVisualStyles = false;
            this.dgvTimeToConvert.Location = new System.Drawing.Point(6, 19);
            this.dgvTimeToConvert.Name = "dgvTimeToConvert";
            this.dgvTimeToConvert.RowHeadersVisible = false;
            this.dgvTimeToConvert.RowTemplate.Height = 30;
            this.dgvTimeToConvert.Size = new System.Drawing.Size(574, 51);
            this.dgvTimeToConvert.TabIndex = 0;
            this.dgvTimeToConvert.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTimeToConvert_CellClick);
            this.dgvTimeToConvert.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTimeToConvert_CellValidated);
            // 
            // dcNewTimeToConvert
            // 
            this.dcNewTimeToConvert.HeaderText = "Time To Convert";
            this.dcNewTimeToConvert.Name = "dcNewTimeToConvert";
            this.dcNewTimeToConvert.Width = 191;
            // 
            // dcNewPoints
            // 
            this.dcNewPoints.HeaderText = "New Points";
            this.dcNewPoints.Name = "dcNewPoints";
            this.dcNewPoints.ReadOnly = true;
            this.dcNewPoints.Width = 191;
            // 
            // dcNewTimeBalance
            // 
            this.dcNewTimeBalance.HeaderText = "Balance Time";
            this.dcNewTimeBalance.Name = "dcNewTimeBalance";
            this.dcNewTimeBalance.ReadOnly = true;
            this.dcNewTimeBalance.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dcNewTimeBalance.Width = 191;
            // 
            // dgvCardInfo
            // 
            this.dgvCardInfo.AllowUserToAddRows = false;
            this.dgvCardInfo.AllowUserToDeleteRows = false;
            this.dgvCardInfo.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dgvCardInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCardInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvCardInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCardInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcParentCardNumber,
            this.dcParentCustomer,
            this.dcParentPoints,
            this.dcParentTime,
            this.dcParentCardId});
            this.dgvCardInfo.EnableHeadersVisualStyles = false;
            this.dgvCardInfo.Location = new System.Drawing.Point(124, 49);
            this.dgvCardInfo.Name = "dgvCardInfo";
            this.dgvCardInfo.ReadOnly = true;
            this.dgvCardInfo.RowHeadersVisible = false;
            this.dgvCardInfo.RowTemplate.Height = 30;
            this.dgvCardInfo.Size = new System.Drawing.Size(587, 50);
            this.dgvCardInfo.TabIndex = 20023;
            // 
            // dcParentCardNumber
            // 
            this.dcParentCardNumber.HeaderText = "Card Number";
            this.dcParentCardNumber.Name = "dcParentCardNumber";
            this.dcParentCardNumber.ReadOnly = true;
            this.dcParentCardNumber.Width = 146;
            // 
            // dcParentCustomer
            // 
            this.dcParentCustomer.HeaderText = "Customer";
            this.dcParentCustomer.Name = "dcParentCustomer";
            this.dcParentCustomer.ReadOnly = true;
            this.dcParentCustomer.Width = 146;
            // 
            // dcParentPoints
            // 
            this.dcParentPoints.HeaderText = "Points";
            this.dcParentPoints.Name = "dcParentPoints";
            this.dcParentPoints.ReadOnly = true;
            this.dcParentPoints.Width = 146;
            // 
            // dcParentTime
            // 
            this.dcParentTime.HeaderText = "Time";
            this.dcParentTime.Name = "dcParentTime";
            this.dcParentTime.ReadOnly = true;
            this.dcParentTime.Width = 146;
            // 
            // dcParentCardId
            // 
            this.dcParentCardId.HeaderText = "ParentCardId";
            this.dcParentCardId.Name = "dcParentCardId";
            this.dcParentCardId.ReadOnly = true;
            this.dcParentCardId.Visible = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.BackColor = System.Drawing.Color.Transparent;
            this.buttonOK.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.buttonOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonOK.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.buttonOK.FlatAppearance.BorderSize = 0;
            this.buttonOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.White;
            this.buttonOK.Location = new System.Drawing.Point(352, 441);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(155, 55);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            this.buttonOK.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonOK_MouseDown);
            this.buttonOK.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonOK_MouseUp);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.Location = new System.Drawing.Point(656, 441);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(155, 55);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            this.buttonCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonOK_MouseDown);
            this.buttonCancel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonOK_MouseUp);
            // 
            // textBoxMessageLine
            // 
            this.textBoxMessageLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessageLine.BackColor = System.Drawing.Color.PapayaWhip;
            this.textBoxMessageLine.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessageLine.ForeColor = System.Drawing.Color.Firebrick;
            this.textBoxMessageLine.Location = new System.Drawing.Point(0, 505);
            this.textBoxMessageLine.Name = "textBoxMessageLine";
            this.textBoxMessageLine.ReadOnly = true;
            this.textBoxMessageLine.Size = new System.Drawing.Size(1162, 26);
            this.textBoxMessageLine.TabIndex = 3;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemarks.Location = new System.Drawing.Point(68, 412);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(1075, 22);
            this.txtRemarks.TabIndex = 4;
            this.txtRemarks.Enter += new System.EventHandler(this.txtRemarks_Enter);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1, 415);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Remarks:";
            // 
            // btnShowNumPad
            // 
            this.btnShowNumPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowNumPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.BackgroundImage = global::Parafait_POS.Properties.Resources.Keypad;
            this.btnShowNumPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnShowNumPad.CausesValidation = false;
            this.btnShowNumPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowNumPad.FlatAppearance.BorderSize = 0;
            this.btnShowNumPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowNumPad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowNumPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowNumPad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnShowNumPad.Location = new System.Drawing.Point(1126, 467);
            this.btnShowNumPad.Name = "btnShowNumPad";
            this.btnShowNumPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowNumPad.TabIndex = 21;
            this.btnShowNumPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowNumPad.UseVisualStyleBackColor = false;
            this.btnShowNumPad.Click += new System.EventHandler(this.btnShowNumPad_Click);
            // 
            // FormCardTasks
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(1162, 531);
            this.Controls.Add(this.btnShowNumPad);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.textBoxMessageLine);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.tabControlTasks);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormCardTasks";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Card Tasks";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCardTasks_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormCardTasks_FormClosed);
            this.Load += new System.EventHandler(this.FormCardTasks_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormCardTasks_KeyPress);
            this.tabControlTasks.ResumeLayout(false);
            this.tabPageTransferCard.ResumeLayout(false);
            this.tabPageTransferCard.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tabPageHoldEntitlement.ResumeLayout(false);
            this.tabPageHoldEntitlement.PerformLayout();
            this.tabPageExchangeTokenForCredit.ResumeLayout(false);
            this.tabPageExchangeTokenForCredit.PerformLayout();
            this.tabPageExchangeCreditForToken.ResumeLayout(false);
            this.tabPageExchangeCreditForToken.PerformLayout();
            this.tabPageLoadTickets.ResumeLayout(false);
            this.tabPageLoadTickets.PerformLayout();
            this.tabPageBalanceTransfer.ResumeLayout(false);
            this.tabPageBalanceTransfer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBalanceTransferee)).EndInit();
            this.tabPageLoadMultiple.ResumeLayout(false);
            this.tabPageLoadMultiple.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductsAdded)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMultipleCards)).EndInit();
            this.tabPageRealETicket.ResumeLayout(false);
            this.grpRealEticket.ResumeLayout(false);
            this.grpRealEticket.PerformLayout();
            this.tabPageRefundCard.ResumeLayout(false);
            this.grpRefundTime.ResumeLayout(false);
            this.grpRefundTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRefundTime)).EndInit();
            this.grpRefundCredits.ResumeLayout(false);
            this.grpRefundCredits.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRefundCardData)).EndInit();
            this.grpRefundGames.ResumeLayout(false);
            this.grpRefundGames.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRefundBalanceGames)).EndInit();
            this.tabPageLoadBonus.ResumeLayout(false);
            this.tabPageLoadBonus.PerformLayout();
            this.panelLoadBonusManualCard.ResumeLayout(false);
            this.panelLoadBonusManualCard.PerformLayout();
            this.grpLoadBonusTypes.ResumeLayout(false);
            this.tabPageDiscount.ResumeLayout(false);
            this.tabPageDiscount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardDiscountsDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardDiscountsDTOListBS)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPageRedeemLoyalty.ResumeLayout(false);
            this.tabPageRedeemLoyalty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoyaltyRedemption)).EndInit();
            this.tabPageRedeemVirtualPoints.ResumeLayout(false);
            this.tabPageRedeemVirtualPoints.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVirtualPointRedemption)).EndInit();
            this.tpSpecialPricing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSpecialPricing)).EndInit();
            this.tpRedeemTicketsForBonus.ResumeLayout(false);
            this.tpRedeemTicketsForBonus.PerformLayout();
            this.grpBoxTicketType.ResumeLayout(false);
            this.grpBoxTicketType.PerformLayout();
            this.tpSetSiteCode.ResumeLayout(false);
            this.tpSetSiteCode.PerformLayout();
            this.tpGetMiFareGPDetails.ResumeLayout(false);
            this.tpGetMiFareGPDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardDetails)).EndInit();
            this.tpRedeemBonusForTicket.ResumeLayout(false);
            this.tpRedeemBonusForTicket.PerformLayout();
            this.tpPauseTime.ResumeLayout(false);
            this.tpPauseTime.PerformLayout();
            this.tpConvertCreditsToTime.ResumeLayout(false);
            this.grpPointsToConvert.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPointsToConvert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTappedCard)).EndInit();
            this.tpConvertTimeToPoints.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTimeToConvert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlTasks;
        private System.Windows.Forms.TabPage tabPageTransferCard;
        private System.Windows.Forms.TabPage tabPageHoldEntitlement;
        private System.Windows.Forms.TabPage tabPageLoadTickets;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabPage tabPageConsolidate;
        private System.Windows.Forms.TabPage tabPageExchangeTokenForCredit;
        private System.Windows.Forms.TabPage tabPageExchangeCreditForToken;
        private System.Windows.Forms.TabPage tabPageLoadMultiple;
        private System.Windows.Forms.TabPage tabPageRealETicket;
        private System.Windows.Forms.TabPage tabPageRefundCard;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonTransferCardGetDetails;
        private System.Windows.Forms.TextBox textBoxTransferCardNumber;
        private System.Windows.Forms.TextBox textBoxHoldEntitlementNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblEnterTicketsToLoad;
        private System.Windows.Forms.TextBox textBoxLoadTickets;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxMessageLine;
        private System.Windows.Forms.Label lblRefundAmount;
        private System.Windows.Forms.RadioButton radioButtoneTicket;
        private System.Windows.Forms.RadioButton radioButtonRealTicket;
        private System.Windows.Forms.GroupBox grpRealEticket;
        private System.Windows.Forms.TextBox txtCreditsAdded;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTokensExchanged;
        private System.Windows.Forms.TextBox txtCreditsRequired;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTokensBought;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridView dgvProductsAdded;
        private System.Windows.Forms.DataGridView dgvMultipleCards;
        private System.Windows.Forms.Button btnChooseProduct;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRemoveProduct;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabPage tabPageLoadBonus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLoadBonus;
        private System.Windows.Forms.TabPage tabPageDiscount;
        private System.Windows.Forms.Button btnAddDiscount;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker dtpDiscountExpiryDate;
        private System.Windows.Forms.ComboBox cmbDiscount;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbExpires;
        private System.Windows.Forms.RadioButton rbNever;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPageRedeemLoyalty;
        private System.Windows.Forms.TabPage tabPageRedeemVirtualPoints;
        private System.Windows.Forms.TextBox txtLoyaltyPoints;
        private System.Windows.Forms.TextBox txtVirtualPoints;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelVirtualPointAvailable;
        private System.Windows.Forms.DataGridView dgvLoyaltyRedemption;
        private System.Windows.Forms.DataGridView dgvVirtualPointRedemption;
        private System.Windows.Forms.TextBox txtLoyaltyRedeemPoints;
        private System.Windows.Forms.TextBox txtVirtualRedeemPoints;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label labelVirtualPointRedeem;
        private System.Windows.Forms.Button btnGetRedemptionValues;
        private System.Windows.Forms.Button btnVirtualGetRedemptionValues;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label labelVirtualPoint;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectedVirtualPoint;
        private System.Windows.Forms.TabPage tpSpecialPricing;
        private System.Windows.Forms.DataGridView dgvSpecialPricing;
        private System.Windows.Forms.DataGridViewTextBoxColumn PricingId;
        private System.Windows.Forms.DataGridViewButtonColumn PricingName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Percentage;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequiresManagerApproval;
        private System.Windows.Forms.Button btnClearPricing;
        private System.Windows.Forms.TabPage tpRedeemTicketsForBonus;
        private System.Windows.Forms.TextBox txtTicketsAvailable;
        private System.Windows.Forms.Label lblTicketsAvlbl;
        private System.Windows.Forms.Button btnRefreshBonus;
        private System.Windows.Forms.TextBox txtTicketsToRedeem;
        private System.Windows.Forms.Label lblTicketsToRedeem;
        private System.Windows.Forms.TextBox txtBonusEligible;
        private System.Windows.Forms.Label lblBonusEligible;
        private System.Windows.Forms.DataGridView dgvRefundCardData;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtRefundAmount;
        private System.Windows.Forms.TabPage tpSetSiteCode;
        private System.Windows.Forms.ComboBox cbSiteCode;
        private System.Windows.Forms.Button btnChangeSiteCode;
        private System.Windows.Forms.TextBox txtCurrentSiteCode;
        private System.Windows.Forms.Label lblNewSiteCode;
        private System.Windows.Forms.Label lblCurrentSiteCode;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.TabPage tpGetMiFareGPDetails;
        private System.Windows.Forms.TextBox txtNoOfGamePlays;
        private System.Windows.Forms.Label lblNoOfGamePlays;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.DataGridView dgvCardDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcSiteId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcMachineAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcStartBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcEndBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcComments;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcStatus;
        private System.Windows.Forms.TextBox txtCardNumberMiFare;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnShowNumPad;
        private System.Windows.Forms.GroupBox grpLoadBonusTypes;
        private System.Windows.Forms.FlowLayoutPanel flpLoadBonusTypes;
        private System.Windows.Forms.Label lblTaxAmount;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Credits;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bonus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Courtesy;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductType;
        private System.Windows.Forms.RadioButton rb10DFormat;
        private System.Windows.Forms.RadioButton rb10HFormat;
        private System.Windows.Forms.Label lblScannedTickets;
        private System.Windows.Forms.CheckBox chkMakeCardNewOnFullRefund;
        private System.Windows.Forms.DataGridView dgvRefundBalanceGames;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtRefundGameAmount;
        private System.Windows.Forms.Label lblRefundGameCount;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectRefundGame;
        private System.Windows.Forms.GroupBox grpRefundCredits;
        private System.Windows.Forms.GroupBox grpRefundGames;
        private System.Windows.Forms.Button btnAlphanumericCancelndarTransferCard;
        private System.Windows.Forms.Button btnAlphanumericCancelndarHoldEntitlement;
        private System.Windows.Forms.Button btnLoadBonusAlphaKeypad;
        private System.Windows.Forms.Button btnGetLoadBonusManualCard;
        private System.Windows.Forms.TextBox txtLoadBonusManualCardNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelLoadBonusManualCard;
        private System.Windows.Forms.TabPage tabPageBalanceTransfer;
        private System.Windows.Forms.Label lblTransfereeCardDetails;
        private System.Windows.Forms.Label lblTransfererCardDetails;
        private System.Windows.Forms.DataGridView dgvBalanceTransferee;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcTrToCardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcTrToCredits;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcTrToBonus;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcTrToTickets;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcTransferCredits;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcTransferBonus;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcTransferTickets;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcTrToCardId;
        private System.Windows.Forms.GroupBox grpRefundTime;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txtRefundTime;
        private System.Windows.Forms.DataGridView dgvRefundTime;
        private System.Windows.Forms.Label lblLoadGameAttributes;
        private System.Windows.Forms.ComboBox cmbLoadTimeAttributes;
        private System.Windows.Forms.ComboBox cmbLoadGameAttributes;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkRefundTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLoadCardSerialMapping;
        private System.Windows.Forms.TextBox txtToCardSerialNumber;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtFromCardSerialNumber;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Card_Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn BulkSerialNumber;
        private System.Windows.Forms.GroupBox grpBoxTicketType;
        private System.Windows.Forms.RadioButton rbCardBalance;
        private System.Windows.Forms.RadioButton rbBonus;
        private System.Windows.Forms.TabPage tpRedeemBonusForTicket;
        private System.Windows.Forms.TextBox txtElgibleTickets;
        private System.Windows.Forms.Label lblTicketsElgible;
        private System.Windows.Forms.TextBox txtBonusAvailable;
        private System.Windows.Forms.Label lblBonusAvl;
        private System.Windows.Forms.Button btnBonusRefresh;
        private System.Windows.Forms.TextBox txtBonusToRedeem;
        private System.Windows.Forms.Label lblBonusToRedeem;
        private System.Windows.Forms.DataGridView dgvCardDiscountsDTOList;
        private System.Windows.Forms.BindingSource cardDiscountsDTOListBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCardDiscountsDTOListCardDiscountIdTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvCardDiscountsDTOListDiscountIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCardDiscountsDTOListExpiryDateTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCardDiscountsDTOListLastUpdatedUserTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCardDiscountsDTOListLastUpdatedDateTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCardDiscountsDTOListIsActiveCheckBoxColumn;
        private System.Windows.Forms.TabPage tpPauseTime;
        private System.Windows.Forms.TextBox textTimeRemaining;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox textCardNo;
        private System.Windows.Forms.TabPage tpConvertCreditsToTime;
        private System.Windows.Forms.GroupBox grpPointsToConvert;
        private System.Windows.Forms.DataGridView dgvPointsToConvert;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcAdditionalPoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcAdditionalTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcBalancePoints;
        private System.Windows.Forms.DataGridView dgvTappedCard;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentCardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentCredits;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentCardId;
        private System.Windows.Forms.Button btnClearPointsToConvert;
        private System.Windows.Forms.TabPage tpConvertTimeToPoints;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dgvTimeToConvert;
        private System.Windows.Forms.DataGridView dgvCardInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcNewTimeToConvert;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcNewPoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcNewTimeBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcParentCardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcParentCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcParentPoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcParentTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcParentCardId;
        private System.Windows.Forms.Label lblEntitlement;
        private System.Windows.Forms.TextBox txteTicket;
        private System.Windows.Forms.Label lblPausetimeEticketBal;
        private System.Windows.Forms.Button btnCustomerLookUp;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Button getHoldCardDetails;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button sampleButtonCardSaleProduct;
        private System.Windows.Forms.Button sampleButtonCardSaleSelectedProduct;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox textBox1;
    }
}
