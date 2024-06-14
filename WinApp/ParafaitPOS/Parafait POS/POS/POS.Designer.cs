using System;
using System.Windows.Forms;

namespace Parafait_POS
{
    partial class POS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainerPOS = new System.Windows.Forms.SplitContainer();
            this.tabControlSelection = new System.Windows.Forms.TabControl();
            this.tabPageProducts = new System.Windows.Forms.TabPage();
            this.btnPrevProductGroup = new System.Windows.Forms.Button();
            this.panelProductSearch = new System.Windows.Forms.Panel();
            this.btnQuantity = new System.Windows.Forms.Button();
            this.nudQuantity = new System.Windows.Forms.NumericUpDown();
            this.btnProductLookup = new System.Windows.Forms.Button();
            this.txtProductSearch = new System.Windows.Forms.TextBox();
            this.btnDisplayGroupDropDown = new System.Windows.Forms.Button();
            this.lblTabText = new System.Windows.Forms.Label();
            this.btnNextProductGroup = new System.Windows.Forms.Button();
            this.tabControlProducts = new System.Windows.Forms.TabControl();
            this.tabPageProductGroups = new System.Windows.Forms.TabPage();
            this.flowLayoutPanelCardProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.SampleButtonCardProduct = new System.Windows.Forms.Button();
            this.SampleButtonOtherProduct = new System.Windows.Forms.Button();
            this.sampleButtonAttraction = new System.Windows.Forms.Button();
            this.sampleButtonCardSaleProduct = new System.Windows.Forms.Button();
            this.SampleButtonGameTime = new System.Windows.Forms.Button();
            this.btnSampleProductRecharge = new System.Windows.Forms.Button();
            this.btnSampleVariableRecharge = new System.Windows.Forms.Button();
            this.btnSampleCheckInCheckOut = new System.Windows.Forms.Button();
            this.btnSampleCombo = new System.Windows.Forms.Button();
            this.sampleButtonMainMenu = new System.Windows.Forms.Button();
            this.SampleButtonVoucher = new System.Windows.Forms.Button();
            this.panelProductButtons = new System.Windows.Forms.Panel();
            this.btnRefundCard = new System.Windows.Forms.Button();
            this.btnProductDetails = new System.Windows.Forms.Button();
            this.tabPageDiscounts = new System.Windows.Forms.TabPage();
            this.flowLayoutPanelDiscounts = new System.Windows.Forms.FlowLayoutPanel();
            this.SampleButtonDiscount = new System.Windows.Forms.Button();
            this.tabPageFunctions = new System.Windows.Forms.TabPage();
            this.flowLayoutPanelFunctions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTransferCard = new System.Windows.Forms.Button();
            this.btnTokenForCredit = new System.Windows.Forms.Button();
            this.btnCreditForToken = new System.Windows.Forms.Button();
            this.btnLoadTickets = new System.Windows.Forms.Button();
            this.btnConsolidateCards = new System.Windows.Forms.Button();
            this.btnLoadMultiple = new System.Windows.Forms.Button();
            this.btnRealEticket = new System.Windows.Forms.Button();
            this.btnLoadBonus = new System.Windows.Forms.Button();
            this.btnApplyDiscount = new System.Windows.Forms.Button();
            this.btnRedeemLoyalty = new System.Windows.Forms.Button();
            this.btnSpecialPricing = new System.Windows.Forms.Button();
            this.btnRedeemTicketsForBonus = new System.Windows.Forms.Button();
            this.btnSetChildSite = new System.Windows.Forms.Button();
            this.btnGetMiFareDetails = new System.Windows.Forms.Button();
            this.btnRefundCardTask = new System.Windows.Forms.Button();
            this.btnTransferBalance = new System.Windows.Forms.Button();
            this.btnSalesReturnExchange = new System.Windows.Forms.Button();
            this.btnRedeemBonusForTicket = new System.Windows.Forms.Button();
            this.btnPauseTime = new System.Windows.Forms.Button();
            this.btnConvertPointsToTime = new System.Windows.Forms.Button();
            this.btnConvertTimeToPoints = new System.Windows.Forms.Button();
            this.btnholdEntitilements = new System.Windows.Forms.Button();
            this.btnRedeemVirtualPoint = new System.Windows.Forms.Button();
            this.tabPageRedeem = new System.Windows.Forms.TabPage();
            this.pbRedeem = new System.Windows.Forms.PictureBox();
            this.tabPageSystem = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonReConnectCardReader = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.panelSkinColor = new System.Windows.Forms.Panel();
            this.buttonChangeSkinColor = new System.Windows.Forms.Button();
            this.buttonSkinColorReset = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.panelPassword = new System.Windows.Forms.Panel();
            this.buttonChangePassword = new System.Windows.Forms.Button();
            this.textBoxReenterNewPassword = new System.Windows.Forms.TextBox();
            this.textBoxNewPassword = new System.Windows.Forms.TextBox();
            this.textBoxCurrentPassword = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbPaymentMode = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.textBoxMessageLine = new System.Windows.Forms.TextBox();
            this.tabControlCardAction = new System.Windows.Forms.TabControl();
            this.tabPageTrx = new System.Windows.Forms.TabPage();
            this.flpTrxProfiles = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTrxProfileDefault = new System.Windows.Forms.Button();
            this.dgvTrxSummary = new System.Windows.Forms.DataGridView();
            this.ProductType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTrxButtons = new System.Windows.Forms.Panel();
            this.flpTrxButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonSaveTransaction = new System.Windows.Forms.Button();
            this.btnPayment = new System.Windows.Forms.Button();
            this.buttonCancelTransaction = new System.Windows.Forms.Button();
            this.buttonCancelLine = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.flpOrderButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPlaceOrder = new System.Windows.Forms.Button();
            this.btnViewOrders = new System.Windows.Forms.Button();
            this.btnRedeemCoupon = new System.Windows.Forms.Button();
            this.btnVariableRefund = new System.Windows.Forms.Button();
            this.btnSendPaymentLink = new System.Windows.Forms.Button();
            this.dataGridViewTransaction = new System.Windows.Forms.DataGridView();
            this.Product_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Card_Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Product_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Line_Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Line_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttractionDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TrxProfileId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageActivities = new System.Windows.Forms.TabPage();
            this.btnCardSearch = new System.Windows.Forms.Button();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.TxtCardNumber = new System.Windows.Forms.TextBox();
            this.lnkPrintCardActivity = new System.Windows.Forms.LinkLabel();
            this.lnkShowHideExtended = new System.Windows.Forms.LinkLabel();
            this.btnConsolidatedCardActivity = new System.Windows.Forms.Button();
            this.lnkConsolidatedView = new System.Windows.Forms.LinkLabel();
            this.lnkZoomCardActivity = new System.Windows.Forms.LinkLabel();
            this.dataGridViewGamePlay = new System.Windows.Forms.DataGridView();
            this.ReverseGamePlay = new System.Windows.Forms.DataGridViewButtonColumn();
            this.labelGamePlay = new System.Windows.Forms.Label();
            this.labelPurchases = new System.Windows.Forms.Label();
            this.dataGridViewPurchases = new System.Windows.Forms.DataGridView();
            this.dcBtnCardActivityTrxPrint = new System.Windows.Forms.DataGridViewImageColumn();
            this.tabPageCardInfo = new System.Windows.Forms.TabPage();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.dgvCardGames = new System.Windows.Forms.DataGridView();
            this.dgvCardDiscounts = new System.Windows.Forms.DataGridView();
            this.dgvCreditPlus = new System.Windows.Forms.DataGridView();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.btnRefreshGraph = new System.Windows.Forms.Button();
            this.rdAmount = new System.Windows.Forms.RadioButton();
            this.lblTicketMode = new System.Windows.Forms.Label();
            this.rdCount = new System.Windows.Forms.RadioButton();
            this.lblTicketAllowed = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.dtpGraphFrom = new System.Windows.Forms.DateTimePicker();
            this.lblRoamingCard = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.zedGraphCardInfo = new ZedGraph.ZedGraphControl();
            this.dtpGraphTo = new System.Windows.Forms.DateTimePicker();
            this.tabPageMyTrx = new System.Windows.Forms.TabPage();
            this.tpBookings = new System.Windows.Forms.TabPage();
            this.tcAttractionBookings = new System.Windows.Forms.TabControl();
            this.tpAttractionBooking = new System.Windows.Forms.TabPage();
            this.dgvBookings = new System.Windows.Forms.DataGridView();
            this.filterBookingPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.rdBookingToday = new System.Windows.Forms.RadioButton();
            this.rdBookingPast = new System.Windows.Forms.RadioButton();
            this.rdBookingFuture3 = new System.Windows.Forms.RadioButton();
            this.rdBookingFutureAll = new System.Windows.Forms.RadioButton();
            this.btnSearchBookingAttraction = new System.Windows.Forms.Button();
            this.verticalScrollBarViewAB = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.horizontalScrollBarViewAB = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.tpAttractionScedules = new System.Windows.Forms.TabPage();
            this.dtpAttractionDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbAttractionFacility = new System.Windows.Forms.ComboBox();
            this.chkShowPast = new System.Windows.Forms.CheckBox();
            this.btnRescheduleAttraction = new System.Windows.Forms.Button();
            this.dgvAttractionSchedules = new System.Windows.Forms.DataGridView();
            this.verticalScrollBarViewAS = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.horizontalScrollBarViewAS = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.tpReservations = new System.Windows.Forms.TabPage();
            this.lnkRefreshReservations = new System.Windows.Forms.LinkLabel();
            this.lnkNewReservation = new System.Windows.Forms.LinkLabel();
            this.dgvAllReservations = new System.Windows.Forms.DataGridView();
            this.dcSelectBooking = new System.Windows.Forms.DataGridViewButtonColumn();
            this.horizontalScrollBarViewRB = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.verticalScrollBarViewRB = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.tpOpenOrders = new System.Windows.Forms.TabPage();
            this.panelOrders = new System.Windows.Forms.Panel();
            this.btnCloseOrderPanel = new System.Windows.Forms.Button();
            this.tcOrderView = new System.Windows.Forms.TabControl();
            this.tpOrderTableView = new System.Windows.Forms.TabPage();
            this.panelTables = new System.Windows.Forms.Panel();
            this.tableLayoutVerticalScrollBarView = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.tableLayoutHorizontalScrollBarView = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.tblPanelTables = new System.Windows.Forms.Integration.ElementHost();
            this.flpFacilities = new System.Windows.Forms.FlowLayoutPanel();
            this.tpOrderOrderView = new System.Windows.Forms.TabPage();
            this.orderListView = new Parafait_POS.OrderListView();
            this.tpOrderBookingView = new System.Windows.Forms.TabPage();
            this.dgvBookingDetails = new System.Windows.Forms.DataGridView();
            this.tpOrderPendingTrxView = new System.Windows.Forms.TabPage();
            this.dgvPendingTransactions = new System.Windows.Forms.DataGridView();
            this.dcPendingTrxRightClick = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnCloseProductDetails = new System.Windows.Forms.Button();
            this.panelProductDetails = new System.Windows.Forms.Panel();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripSiteName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripPOSMachine = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLoginID = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripPOSUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripRole = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLANStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelCardSwipe = new System.Windows.Forms.GroupBox();
            this.btnCardBalancePrint = new System.Windows.Forms.Button();
            this.btnRegisterCustomer = new System.Windows.Forms.Button();
            this.btnParentChild = new System.Windows.Forms.Button();
            this.txtVIPStatus = new System.Windows.Forms.TextBox();
            this.labelCardNo = new System.Windows.Forms.TextBox();
            this.textBoxCustomerInfo = new System.Windows.Forms.TextBox();
            this.labelCardStatuslbl = new System.Windows.Forms.Label();
            this.labelCardStatus = new System.Windows.Forms.Label();
            this.labelCardNumber = new System.Windows.Forms.Label();
            this.dataGridViewCardDetails = new System.Windows.Forms.DataGridView();
            this.ColumnHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnGetServerCard = new System.Windows.Forms.Button();
            this.txtChangeAmount = new System.Windows.Forms.TextBox();
            this.textBoxTendered = new System.Windows.Forms.TextBox();
            this.textBoxTransactionTotal = new System.Windows.Forms.TextBox();
            this.labelBalance = new System.Windows.Forms.Label();
            this.labelTransactionTotal = new System.Windows.Forms.Label();
            this.panelAmounts = new System.Windows.Forms.Panel();
            this.label24 = new System.Windows.Forms.Label();
            this.txtTipAmount = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtBalanceAmount = new System.Windows.Forms.TextBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.btnRefreshPOS = new System.Windows.Forms.Button();
            this.buttonLogout = new System.Windows.Forms.Button();
            this.btnShowNumPad = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.btnLaunchApps = new System.Windows.Forms.Button();
            this.btnTasks = new System.Windows.Forms.Button();
            this.POSTasksContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.poolKaraokeLightControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenCashDrawer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddToShift = new System.Windows.Forms.ToolStripMenuItem();
            this.lockerFunctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lockerLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lockerUtilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuOnlineFunctions = new System.Windows.Forms.ToolStripMenuItem();
            this.executeTransactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMenuCreditCardGatewayFunctions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNewCompleteFDTransactions = new System.Windows.Forms.ToolStripMenuItem();
            this.gameManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cardFunctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTechCard = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewTasks = new System.Windows.Forms.ToolStripMenuItem();
            this.menuParentChildCards = new System.Windows.Forms.ToolStripMenuItem();
            this.deactivateCardsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masterCardsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tokenCardInventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legacyTransferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStaffFunctions = new System.Windows.Forms.ToolStripMenuItem();
            this.fpEnrollDeactiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attendanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timeSheetDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lockScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeLoginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enterMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteShiftOpenCloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productAvailabilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cashdrawerAssignmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFiscalPrinterReports = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVariableCashRefund = new System.Windows.Forms.ToolStripMenuItem();
            this.achievements = new System.Windows.Forms.ToolStripMenuItem();
            this.couponStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRunReport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFlagVoucher = new System.Windows.Forms.ToolStripMenuItem();
            this.notificationDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.receiveStock = new System.Windows.Forms.ToolStripMenuItem();
            this.retailInventoryLookUp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAccessControl = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTransactionFunctions = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionLookupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendTransactionReceiptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionRemarksMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SignedWaiversMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkInCheckOutUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subscriptionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFoodDeliveryFunctions = new System.Windows.Forms.ToolStripMenuItem();
            this.deliveryOrderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCompleteFDTransactions = new System.Windows.Forms.ToolStripMenuItem();
            this.TrxDGVContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuEnterRemarks = new System.Windows.Forms.ToolStripMenuItem();
            this.menuChangeQuantity = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuChangePrice = new System.Windows.Forms.ToolStripMenuItem();
            this.menuResetPrice = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProductModifiers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuApplyDiscount = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExemptTax = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewProductDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuLaunchApps = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.managementStudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxPendingTrxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SelectPendingTrx = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxOrderContextTableMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tblOrderCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.tblOrderPrintKOT = new System.Windows.Forms.ToolStripMenuItem();
            this.tblOrderMoveToTable = new System.Windows.Forms.ToolStripMenuItem();
            this.lblAlerts = new System.Windows.Forms.Button();
            this.panelPOSButtons = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ctxProductButtonContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuApplyProductToAllCards = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPOS)).BeginInit();
            this.splitContainerPOS.Panel1.SuspendLayout();
            this.splitContainerPOS.Panel2.SuspendLayout();
            this.splitContainerPOS.SuspendLayout();
            this.tabControlSelection.SuspendLayout();
            this.tabPageProducts.SuspendLayout();
            this.panelProductSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).BeginInit();
            this.tabControlProducts.SuspendLayout();
            this.tabPageProductGroups.SuspendLayout();
            this.flowLayoutPanelCardProducts.SuspendLayout();
            this.panelProductButtons.SuspendLayout();
            this.tabPageDiscounts.SuspendLayout();
            this.flowLayoutPanelDiscounts.SuspendLayout();
            this.tabPageFunctions.SuspendLayout();
            this.flowLayoutPanelFunctions.SuspendLayout();
            this.tabPageRedeem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRedeem)).BeginInit();
            this.tabPageSystem.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelSkinColor.SuspendLayout();
            this.panelPassword.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabControlCardAction.SuspendLayout();
            this.tabPageTrx.SuspendLayout();
            this.flpTrxProfiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxSummary)).BeginInit();
            this.panelTrxButtons.SuspendLayout();
            this.flpTrxButtons.SuspendLayout();
            this.flpOrderButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTransaction)).BeginInit();
            this.tabPageActivities.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGamePlay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPurchases)).BeginInit();
            this.tabPageCardInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardGames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardDiscounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCreditPlus)).BeginInit();
            this.grpMisc.SuspendLayout();
            this.tpBookings.SuspendLayout();
            this.tcAttractionBookings.SuspendLayout();
            this.tpAttractionBooking.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookings)).BeginInit();
            this.filterBookingPanel.SuspendLayout();
            this.tpAttractionScedules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttractionSchedules)).BeginInit();
            this.tpReservations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllReservations)).BeginInit();
            this.tpOpenOrders.SuspendLayout();
            this.panelOrders.SuspendLayout();
            this.tcOrderView.SuspendLayout();
            this.tpOrderTableView.SuspendLayout();
            this.panelTables.SuspendLayout();
            this.tpOrderOrderView.SuspendLayout();
            this.tpOrderBookingView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookingDetails)).BeginInit();
            this.tpOrderPendingTrxView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendingTransactions)).BeginInit();
            this.panelProductDetails.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panelCardSwipe.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCardDetails)).BeginInit();
            this.panelAmounts.SuspendLayout();
            this.POSTasksContextMenu.SuspendLayout();
            this.TrxDGVContextMenu.SuspendLayout();
            this.ctxMenuLaunchApps.SuspendLayout();
            this.ctxPendingTrxContextMenu.SuspendLayout();
            this.ctxOrderContextTableMenu.SuspendLayout();
            this.panelPOSButtons.SuspendLayout();
            this.ctxProductButtonContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerPOS
            // 
            this.splitContainerPOS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerPOS.BackColor = System.Drawing.Color.Transparent;
            this.splitContainerPOS.Location = new System.Drawing.Point(0, 0);
            this.splitContainerPOS.Name = "splitContainerPOS";
            // 
            // splitContainerPOS.Panel1
            // 
            this.splitContainerPOS.Panel1.Controls.Add(this.tabControlSelection);
            // 
            // splitContainerPOS.Panel2
            // 
            this.splitContainerPOS.Panel2.AutoScroll = true;
            this.splitContainerPOS.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainerPOS.Panel2.Controls.Add(this.textBoxMessageLine);
            this.splitContainerPOS.Panel2.Controls.Add(this.tabControlCardAction);
            this.splitContainerPOS.Size = new System.Drawing.Size(940, 708);
            this.splitContainerPOS.SplitterDistance = 387;
            this.splitContainerPOS.TabIndex = 16;
            // 
            // tabControlSelection
            // 
            this.tabControlSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlSelection.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlSelection.Controls.Add(this.tabPageProducts);
            this.tabControlSelection.Controls.Add(this.tabPageDiscounts);
            this.tabControlSelection.Controls.Add(this.tabPageFunctions);
            this.tabControlSelection.Controls.Add(this.tabPageRedeem);
            this.tabControlSelection.Controls.Add(this.tabPageSystem);
            this.tabControlSelection.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold);
            this.tabControlSelection.ItemSize = new System.Drawing.Size(65, 45);
            this.tabControlSelection.Location = new System.Drawing.Point(-1, 0);
            this.tabControlSelection.Name = "tabControlSelection";
            this.tabControlSelection.SelectedIndex = 0;
            this.tabControlSelection.ShowToolTips = true;
            this.tabControlSelection.Size = new System.Drawing.Size(387, 708);
            this.tabControlSelection.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControlSelection.TabIndex = 0;
            this.tabControlSelection.SelectedIndexChanged += new System.EventHandler(this.tabControlSelection_SelectedIndexChanged);
            // 
            // tabPageProducts
            // 
            this.tabPageProducts.BackColor = System.Drawing.Color.Gray;
            this.tabPageProducts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageProducts.Controls.Add(this.btnPrevProductGroup);
            this.tabPageProducts.Controls.Add(this.panelProductSearch);
            this.tabPageProducts.Controls.Add(this.btnDisplayGroupDropDown);
            this.tabPageProducts.Controls.Add(this.lblTabText);
            this.tabPageProducts.Controls.Add(this.btnNextProductGroup);
            this.tabPageProducts.Controls.Add(this.tabControlProducts);
            this.tabPageProducts.Controls.Add(this.panelProductButtons);
            this.tabPageProducts.ImageIndex = 3;
            this.tabPageProducts.Location = new System.Drawing.Point(4, 49);
            this.tabPageProducts.Name = "tabPageProducts";
            this.tabPageProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProducts.Size = new System.Drawing.Size(379, 655);
            this.tabPageProducts.TabIndex = 0;
            this.tabPageProducts.Text = "Products";
            this.tabPageProducts.ToolTipText = "Choose Products";
            this.tabPageProducts.UseVisualStyleBackColor = true;
            // 
            // btnPrevProductGroup
            // 
            this.btnPrevProductGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevProductGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevProductGroup.BackgroundImage = global::Parafait_POS.Properties.Resources.left_arrow_normal;
            this.btnPrevProductGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPrevProductGroup.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPrevProductGroup.FlatAppearance.BorderSize = 0;
            this.btnPrevProductGroup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrevProductGroup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrevProductGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevProductGroup.Location = new System.Drawing.Point(242, 3);
            this.btnPrevProductGroup.Name = "btnPrevProductGroup";
            this.btnPrevProductGroup.Size = new System.Drawing.Size(35, 35);
            this.btnPrevProductGroup.TabIndex = 9;
            this.btnPrevProductGroup.UseVisualStyleBackColor = false;
            this.btnPrevProductGroup.Click += new System.EventHandler(this.btnPrevProductGroup_Click);
            // 
            // panelProductSearch
            // 
            this.panelProductSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelProductSearch.Controls.Add(this.btnQuantity);
            this.panelProductSearch.Controls.Add(this.nudQuantity);
            this.panelProductSearch.Controls.Add(this.btnProductLookup);
            this.panelProductSearch.Controls.Add(this.txtProductSearch);
            this.panelProductSearch.Location = new System.Drawing.Point(0, 561);
            this.panelProductSearch.Name = "panelProductSearch";
            this.panelProductSearch.Size = new System.Drawing.Size(375, 31);
            this.panelProductSearch.TabIndex = 21;
            // 
            // btnQuantity
            // 
            this.btnQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnQuantity.BackColor = System.Drawing.Color.White;
            this.btnQuantity.FlatAppearance.BorderSize = 0;
            this.btnQuantity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuantity.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnQuantity.ForeColor = System.Drawing.Color.Black;
            this.btnQuantity.Image = ((System.Drawing.Image)(resources.GetObject("btnQuantity.Image")));
            this.btnQuantity.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQuantity.Location = new System.Drawing.Point(4, 5);
            this.btnQuantity.Name = "btnQuantity";
            this.btnQuantity.Size = new System.Drawing.Size(84, 22);
            this.btnQuantity.TabIndex = 22;
            this.btnQuantity.Text = "Quantity";
            this.btnQuantity.UseVisualStyleBackColor = false;
            this.btnQuantity.Click += new System.EventHandler(this.btnQuantity_Click);
            // 
            // nudQuantity
            // 
            this.nudQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudQuantity.Font = new System.Drawing.Font("Arial", 9.75F);
            this.nudQuantity.Location = new System.Drawing.Point(93, 5);
            this.nudQuantity.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudQuantity.Name = "nudQuantity";
            this.nudQuantity.Size = new System.Drawing.Size(50, 22);
            this.nudQuantity.TabIndex = 1;
            this.nudQuantity.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.nudQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQuantity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.General_KeyDown);
            // 
            // btnProductLookup
            // 
            this.btnProductLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnProductLookup.BackColor = System.Drawing.Color.White;
            this.btnProductLookup.FlatAppearance.BorderSize = 0;
            this.btnProductLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductLookup.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnProductLookup.ForeColor = System.Drawing.Color.Black;
            this.btnProductLookup.Image = ((System.Drawing.Image)(resources.GetObject("btnProductLookup.Image")));
            this.btnProductLookup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProductLookup.Location = new System.Drawing.Point(147, 5);
            this.btnProductLookup.Name = "btnProductLookup";
            this.btnProductLookup.Size = new System.Drawing.Size(84, 22);
            this.btnProductLookup.TabIndex = 20;
            this.btnProductLookup.Text = "Product";
            this.btnProductLookup.UseVisualStyleBackColor = false;
            this.btnProductLookup.Click += new System.EventHandler(this.btnProductLookup_Click);
            // 
            // txtProductSearch
            // 
            this.txtProductSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProductSearch.Font = new System.Drawing.Font("Arial", 9.75F);
            this.txtProductSearch.Location = new System.Drawing.Point(237, 5);
            this.txtProductSearch.Name = "txtProductSearch";
            this.txtProductSearch.Size = new System.Drawing.Size(137, 22);
            this.txtProductSearch.TabIndex = 2;
            this.txtProductSearch.Enter += new System.EventHandler(this.txtProductSearch_Enter);
            this.txtProductSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProductSearch_KeyDown);
            this.txtProductSearch.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtProductSearch_MouseDoubleClick);
            // 
            // btnDisplayGroupDropDown
            // 
            this.btnDisplayGroupDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplayGroupDropDown.BackColor = System.Drawing.Color.Transparent;
            this.btnDisplayGroupDropDown.BackgroundImage = global::Parafait_POS.Properties.Resources.option_button_normal;
            this.btnDisplayGroupDropDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDisplayGroupDropDown.CausesValidation = false;
            this.btnDisplayGroupDropDown.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnDisplayGroupDropDown.FlatAppearance.BorderSize = 0;
            this.btnDisplayGroupDropDown.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDisplayGroupDropDown.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDisplayGroupDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplayGroupDropDown.Location = new System.Drawing.Point(291, 3);
            this.btnDisplayGroupDropDown.Name = "btnDisplayGroupDropDown";
            this.btnDisplayGroupDropDown.Size = new System.Drawing.Size(35, 35);
            this.btnDisplayGroupDropDown.TabIndex = 10;
            this.btnDisplayGroupDropDown.UseVisualStyleBackColor = false;
            this.btnDisplayGroupDropDown.Click += new System.EventHandler(this.btnDisplayGroupDropDown_Click);
            // 
            // lblTabText
            // 
            this.lblTabText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTabText.BackColor = System.Drawing.SystemColors.Control;
            this.lblTabText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTabText.Location = new System.Drawing.Point(0, 0);
            this.lblTabText.MinimumSize = new System.Drawing.Size(150, 43);
            this.lblTabText.Name = "lblTabText";
            this.lblTabText.Size = new System.Drawing.Size(236, 43);
            this.lblTabText.TabIndex = 1;
            this.lblTabText.Text = "TabText";
            this.lblTabText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNextProductGroup
            // 
            this.btnNextProductGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextProductGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnNextProductGroup.BackgroundImage = global::Parafait_POS.Properties.Resources.right_arrow_normal;
            this.btnNextProductGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNextProductGroup.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnNextProductGroup.FlatAppearance.BorderSize = 0;
            this.btnNextProductGroup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextProductGroup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextProductGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextProductGroup.Location = new System.Drawing.Point(338, 3);
            this.btnNextProductGroup.Name = "btnNextProductGroup";
            this.btnNextProductGroup.Size = new System.Drawing.Size(35, 35);
            this.btnNextProductGroup.TabIndex = 1;
            this.btnNextProductGroup.UseVisualStyleBackColor = false;
            this.btnNextProductGroup.Click += new System.EventHandler(this.btnNextProductGroup_Click);
            // 
            // tabControlProducts
            // 
            this.tabControlProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlProducts.Controls.Add(this.tabPageProductGroups);
            this.tabControlProducts.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControlProducts.ItemSize = new System.Drawing.Size(550, 40);
            this.tabControlProducts.Location = new System.Drawing.Point(-3, -1);
            this.tabControlProducts.Name = "tabControlProducts";
            this.tabControlProducts.SelectedIndex = 0;
            this.tabControlProducts.Size = new System.Drawing.Size(382, 560);
            this.tabControlProducts.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlProducts.TabIndex = 0;
            this.tabControlProducts.SelectedIndexChanged += new System.EventHandler(this.tabControlProducts_SelectedIndexChanged);
            // 
            // tabPageProductGroups
            // 
            this.tabPageProductGroups.BackColor = System.Drawing.Color.Gray;
            this.tabPageProductGroups.Controls.Add(this.flowLayoutPanelCardProducts);
            this.tabPageProductGroups.Location = new System.Drawing.Point(4, 44);
            this.tabPageProductGroups.Name = "tabPageProductGroups";
            this.tabPageProductGroups.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProductGroups.Size = new System.Drawing.Size(374, 512);
            this.tabPageProductGroups.TabIndex = 0;
            this.tabPageProductGroups.Text = "Group 1";
            // 
            // flowLayoutPanelCardProducts
            // 
            this.flowLayoutPanelCardProducts.AutoScroll = true;
            this.flowLayoutPanelCardProducts.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanelCardProducts.Controls.Add(this.SampleButtonCardProduct);
            this.flowLayoutPanelCardProducts.Controls.Add(this.SampleButtonOtherProduct);
            this.flowLayoutPanelCardProducts.Controls.Add(this.sampleButtonAttraction);
            this.flowLayoutPanelCardProducts.Controls.Add(this.sampleButtonCardSaleProduct);
            this.flowLayoutPanelCardProducts.Controls.Add(this.SampleButtonGameTime);
            this.flowLayoutPanelCardProducts.Controls.Add(this.btnSampleProductRecharge);
            this.flowLayoutPanelCardProducts.Controls.Add(this.btnSampleVariableRecharge);
            this.flowLayoutPanelCardProducts.Controls.Add(this.btnSampleCheckInCheckOut);
            this.flowLayoutPanelCardProducts.Controls.Add(this.btnSampleCombo);
            this.flowLayoutPanelCardProducts.Controls.Add(this.sampleButtonMainMenu);
            this.flowLayoutPanelCardProducts.Controls.Add(this.SampleButtonVoucher);
            this.flowLayoutPanelCardProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelCardProducts.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelCardProducts.Name = "flowLayoutPanelCardProducts";
            this.flowLayoutPanelCardProducts.Size = new System.Drawing.Size(368, 506);
            this.flowLayoutPanelCardProducts.TabIndex = 0;
            // 
            // SampleButtonCardProduct
            // 
            this.SampleButtonCardProduct.BackColor = System.Drawing.Color.Transparent;
            this.SampleButtonCardProduct.BackgroundImage = global::Parafait_POS.Properties.Resources.NewCard;
            this.SampleButtonCardProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SampleButtonCardProduct.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.SampleButtonCardProduct.FlatAppearance.BorderSize = 0;
            this.SampleButtonCardProduct.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonCardProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonCardProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonCardProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SampleButtonCardProduct.Font = new System.Drawing.Font("Tahoma", 9F);
            this.SampleButtonCardProduct.ForeColor = System.Drawing.Color.Black;
            this.SampleButtonCardProduct.Location = new System.Drawing.Point(3, 3);
            this.SampleButtonCardProduct.Name = "SampleButtonCardProduct";
            this.SampleButtonCardProduct.Size = new System.Drawing.Size(120, 96);
            this.SampleButtonCardProduct.TabIndex = 0;
            this.SampleButtonCardProduct.Text = "New Card";
            this.SampleButtonCardProduct.UseVisualStyleBackColor = false;
            this.SampleButtonCardProduct.Visible = false;
            // 
            // SampleButtonOtherProduct
            // 
            this.SampleButtonOtherProduct.BackColor = System.Drawing.Color.Transparent;
            this.SampleButtonOtherProduct.BackgroundImage = global::Parafait_POS.Properties.Resources.ManualProduct;
            this.SampleButtonOtherProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SampleButtonOtherProduct.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.SampleButtonOtherProduct.FlatAppearance.BorderSize = 0;
            this.SampleButtonOtherProduct.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonOtherProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonOtherProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonOtherProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SampleButtonOtherProduct.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.SampleButtonOtherProduct.ForeColor = System.Drawing.Color.White;
            this.SampleButtonOtherProduct.Location = new System.Drawing.Point(129, 3);
            this.SampleButtonOtherProduct.Name = "SampleButtonOtherProduct";
            this.SampleButtonOtherProduct.Size = new System.Drawing.Size(80, 60);
            this.SampleButtonOtherProduct.TabIndex = 1;
            this.SampleButtonOtherProduct.Text = "Manual";
            this.SampleButtonOtherProduct.UseVisualStyleBackColor = false;
            this.SampleButtonOtherProduct.Visible = false;
            // 
            // sampleButtonAttraction
            // 
            this.sampleButtonAttraction.BackColor = System.Drawing.Color.Transparent;
            this.sampleButtonAttraction.BackgroundImage = global::Parafait_POS.Properties.Resources.Attraction;
            this.sampleButtonAttraction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sampleButtonAttraction.FlatAppearance.BorderSize = 0;
            this.sampleButtonAttraction.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonAttraction.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonAttraction.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonAttraction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sampleButtonAttraction.Font = new System.Drawing.Font("Tahoma", 9F);
            this.sampleButtonAttraction.ForeColor = System.Drawing.Color.Black;
            this.sampleButtonAttraction.Location = new System.Drawing.Point(215, 3);
            this.sampleButtonAttraction.Name = "sampleButtonAttraction";
            this.sampleButtonAttraction.Size = new System.Drawing.Size(120, 96);
            this.sampleButtonAttraction.TabIndex = 2;
            this.sampleButtonAttraction.Text = "Attraction";
            this.sampleButtonAttraction.UseVisualStyleBackColor = false;
            this.sampleButtonAttraction.Visible = false;
            // 
            // sampleButtonCardSaleProduct
            // 
            this.sampleButtonCardSaleProduct.BackColor = System.Drawing.Color.Transparent;
            this.sampleButtonCardSaleProduct.BackgroundImage = global::Parafait_POS.Properties.Resources.CardSale;
            this.sampleButtonCardSaleProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sampleButtonCardSaleProduct.FlatAppearance.BorderSize = 0;
            this.sampleButtonCardSaleProduct.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonCardSaleProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonCardSaleProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonCardSaleProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sampleButtonCardSaleProduct.Font = new System.Drawing.Font("Tahoma", 9F);
            this.sampleButtonCardSaleProduct.ForeColor = System.Drawing.Color.White;
            this.sampleButtonCardSaleProduct.Location = new System.Drawing.Point(3, 105);
            this.sampleButtonCardSaleProduct.Name = "sampleButtonCardSaleProduct";
            this.sampleButtonCardSaleProduct.Size = new System.Drawing.Size(120, 96);
            this.sampleButtonCardSaleProduct.TabIndex = 3;
            this.sampleButtonCardSaleProduct.Text = "Card Sale";
            this.sampleButtonCardSaleProduct.UseVisualStyleBackColor = false;
            this.sampleButtonCardSaleProduct.Visible = false;
            // 
            // SampleButtonGameTime
            // 
            this.SampleButtonGameTime.BackColor = System.Drawing.Color.Transparent;
            this.SampleButtonGameTime.BackgroundImage = global::Parafait_POS.Properties.Resources.GameTime;
            this.SampleButtonGameTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SampleButtonGameTime.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.SampleButtonGameTime.FlatAppearance.BorderSize = 0;
            this.SampleButtonGameTime.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonGameTime.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonGameTime.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonGameTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SampleButtonGameTime.Font = new System.Drawing.Font("Tahoma", 9F);
            this.SampleButtonGameTime.ForeColor = System.Drawing.Color.Black;
            this.SampleButtonGameTime.Location = new System.Drawing.Point(129, 105);
            this.SampleButtonGameTime.Name = "SampleButtonGameTime";
            this.SampleButtonGameTime.Size = new System.Drawing.Size(120, 96);
            this.SampleButtonGameTime.TabIndex = 4;
            this.SampleButtonGameTime.Text = "Game Time";
            this.SampleButtonGameTime.UseVisualStyleBackColor = false;
            this.SampleButtonGameTime.Visible = false;
            // 
            // btnSampleProductRecharge
            // 
            this.btnSampleProductRecharge.BackColor = System.Drawing.Color.Transparent;
            this.btnSampleProductRecharge.BackgroundImage = global::Parafait_POS.Properties.Resources.Recharge;
            this.btnSampleProductRecharge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSampleProductRecharge.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSampleProductRecharge.FlatAppearance.BorderSize = 0;
            this.btnSampleProductRecharge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSampleProductRecharge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSampleProductRecharge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSampleProductRecharge.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnSampleProductRecharge.ForeColor = System.Drawing.Color.Black;
            this.btnSampleProductRecharge.Location = new System.Drawing.Point(3, 207);
            this.btnSampleProductRecharge.Name = "btnSampleProductRecharge";
            this.btnSampleProductRecharge.Size = new System.Drawing.Size(120, 96);
            this.btnSampleProductRecharge.TabIndex = 5;
            this.btnSampleProductRecharge.Text = "Recharge";
            this.btnSampleProductRecharge.UseVisualStyleBackColor = false;
            this.btnSampleProductRecharge.Visible = false;
            // 
            // btnSampleVariableRecharge
            // 
            this.btnSampleVariableRecharge.BackColor = System.Drawing.Color.Transparent;
            this.btnSampleVariableRecharge.BackgroundImage = global::Parafait_POS.Properties.Resources.VariableRecharge;
            this.btnSampleVariableRecharge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSampleVariableRecharge.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSampleVariableRecharge.FlatAppearance.BorderSize = 0;
            this.btnSampleVariableRecharge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSampleVariableRecharge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSampleVariableRecharge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSampleVariableRecharge.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnSampleVariableRecharge.ForeColor = System.Drawing.Color.LavenderBlush;
            this.btnSampleVariableRecharge.Location = new System.Drawing.Point(129, 207);
            this.btnSampleVariableRecharge.Name = "btnSampleVariableRecharge";
            this.btnSampleVariableRecharge.Size = new System.Drawing.Size(120, 96);
            this.btnSampleVariableRecharge.TabIndex = 6;
            this.btnSampleVariableRecharge.Text = "Variable Recharge";
            this.btnSampleVariableRecharge.UseVisualStyleBackColor = false;
            this.btnSampleVariableRecharge.Visible = false;
            // 
            // btnSampleCheckInCheckOut
            // 
            this.btnSampleCheckInCheckOut.BackColor = System.Drawing.Color.Transparent;
            this.btnSampleCheckInCheckOut.BackgroundImage = global::Parafait_POS.Properties.Resources.CheckInCheckOut;
            this.btnSampleCheckInCheckOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSampleCheckInCheckOut.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSampleCheckInCheckOut.FlatAppearance.BorderSize = 0;
            this.btnSampleCheckInCheckOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSampleCheckInCheckOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSampleCheckInCheckOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSampleCheckInCheckOut.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnSampleCheckInCheckOut.ForeColor = System.Drawing.Color.LavenderBlush;
            this.btnSampleCheckInCheckOut.Location = new System.Drawing.Point(3, 309);
            this.btnSampleCheckInCheckOut.Name = "btnSampleCheckInCheckOut";
            this.btnSampleCheckInCheckOut.Size = new System.Drawing.Size(120, 96);
            this.btnSampleCheckInCheckOut.TabIndex = 7;
            this.btnSampleCheckInCheckOut.Text = "CheckIn-CheckOut";
            this.btnSampleCheckInCheckOut.UseVisualStyleBackColor = false;
            this.btnSampleCheckInCheckOut.Visible = false;
            // 
            // btnSampleCombo
            // 
            this.btnSampleCombo.BackColor = System.Drawing.Color.Transparent;
            this.btnSampleCombo.BackgroundImage = global::Parafait_POS.Properties.Resources.ComboProduct;
            this.btnSampleCombo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSampleCombo.FlatAppearance.BorderSize = 0;
            this.btnSampleCombo.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSampleCombo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSampleCombo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSampleCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSampleCombo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnSampleCombo.ForeColor = System.Drawing.Color.SeaShell;
            this.btnSampleCombo.Location = new System.Drawing.Point(129, 309);
            this.btnSampleCombo.Name = "btnSampleCombo";
            this.btnSampleCombo.Size = new System.Drawing.Size(100, 79);
            this.btnSampleCombo.TabIndex = 8;
            this.btnSampleCombo.Text = "Combo";
            this.btnSampleCombo.UseVisualStyleBackColor = false;
            this.btnSampleCombo.Visible = false;
            // 
            // sampleButtonMainMenu
            // 
            this.sampleButtonMainMenu.BackColor = System.Drawing.Color.Transparent;
            this.sampleButtonMainMenu.BackgroundImage = global::Parafait_POS.Properties.Resources.DiplayGroupButton;
            this.sampleButtonMainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sampleButtonMainMenu.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.sampleButtonMainMenu.FlatAppearance.BorderSize = 0;
            this.sampleButtonMainMenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonMainMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.sampleButtonMainMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sampleButtonMainMenu.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sampleButtonMainMenu.ForeColor = System.Drawing.Color.White;
            this.sampleButtonMainMenu.Location = new System.Drawing.Point(235, 309);
            this.sampleButtonMainMenu.Name = "sampleButtonMainMenu";
            this.sampleButtonMainMenu.Size = new System.Drawing.Size(120, 96);
            this.sampleButtonMainMenu.TabIndex = 9;
            this.sampleButtonMainMenu.Text = "Main Menu";
            this.sampleButtonMainMenu.UseVisualStyleBackColor = false;
            this.sampleButtonMainMenu.Visible = false;
            // 
            // SampleButtonVoucher
            // 
            this.SampleButtonVoucher.BackColor = System.Drawing.Color.Transparent;
            this.SampleButtonVoucher.BackgroundImage = global::Parafait_POS.Properties.Resources.VoucherProduct;
            this.SampleButtonVoucher.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SampleButtonVoucher.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.SampleButtonVoucher.FlatAppearance.BorderSize = 0;
            this.SampleButtonVoucher.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonVoucher.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonVoucher.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonVoucher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SampleButtonVoucher.Font = new System.Drawing.Font("Tahoma", 9F);
            this.SampleButtonVoucher.ForeColor = System.Drawing.Color.Black;
            this.SampleButtonVoucher.Location = new System.Drawing.Point(3, 411);
            this.SampleButtonVoucher.Name = "SampleButtonVoucher";
            this.SampleButtonVoucher.Size = new System.Drawing.Size(120, 96);
            this.SampleButtonVoucher.TabIndex = 10;
            this.SampleButtonVoucher.Text = "Voucher";
            this.SampleButtonVoucher.UseVisualStyleBackColor = false;
            this.SampleButtonVoucher.Visible = false;
            // 
            // panelProductButtons
            // 
            this.panelProductButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelProductButtons.Controls.Add(this.btnRefundCard);
            this.panelProductButtons.Controls.Add(this.btnProductDetails);
            this.panelProductButtons.Location = new System.Drawing.Point(0, 592);
            this.panelProductButtons.Name = "panelProductButtons";
            this.panelProductButtons.Padding = new System.Windows.Forms.Padding(3);
            this.panelProductButtons.Size = new System.Drawing.Size(375, 58);
            this.panelProductButtons.TabIndex = 23;
            this.panelProductButtons.Resize += new System.EventHandler(this.panelProductButtons_Resize);
            // 
            // btnRefundCard
            // 
            this.btnRefundCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefundCard.BackColor = System.Drawing.Color.Transparent;
            this.btnRefundCard.BackgroundImage = global::Parafait_POS.Properties.Resources.RefundCard_Normal;
            this.btnRefundCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefundCard.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRefundCard.FlatAppearance.BorderSize = 0;
            this.btnRefundCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefundCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefundCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefundCard.Font = new System.Drawing.Font("Arial", 11.25F);
            this.btnRefundCard.ForeColor = System.Drawing.Color.White;
            this.btnRefundCard.Location = new System.Drawing.Point(19, 3);
            this.btnRefundCard.Name = "btnRefundCard";
            this.btnRefundCard.Size = new System.Drawing.Size(149, 52);
            this.btnRefundCard.TabIndex = 8;
            this.btnRefundCard.Tag = "REFUNDCARD";
            this.btnRefundCard.Text = "Refund Card";
            this.btnRefundCard.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRefundCard.UseVisualStyleBackColor = false;
            this.btnRefundCard.Click += new System.EventHandler(this.CallTask);
            this.btnRefundCard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnRefundCard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnProductDetails
            // 
            this.btnProductDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnProductDetails.BackColor = System.Drawing.Color.Transparent;
            this.btnProductDetails.BackgroundImage = global::Parafait_POS.Properties.Resources.ProductDetails_Normal;
            this.btnProductDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProductDetails.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnProductDetails.FlatAppearance.BorderSize = 0;
            this.btnProductDetails.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProductDetails.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProductDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductDetails.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProductDetails.ForeColor = System.Drawing.Color.White;
            this.btnProductDetails.Location = new System.Drawing.Point(210, 3);
            this.btnProductDetails.Name = "btnProductDetails";
            this.btnProductDetails.Size = new System.Drawing.Size(149, 52);
            this.btnProductDetails.TabIndex = 22;
            this.btnProductDetails.Text = "Product Details";
            this.btnProductDetails.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnProductDetails.UseVisualStyleBackColor = false;
            this.btnProductDetails.Click += new System.EventHandler(this.btnProductDetails_Click);
            // 
            // tabPageDiscounts
            // 
            this.tabPageDiscounts.BackColor = System.Drawing.Color.Gray;
            this.tabPageDiscounts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageDiscounts.Controls.Add(this.flowLayoutPanelDiscounts);
            this.tabPageDiscounts.Location = new System.Drawing.Point(4, 49);
            this.tabPageDiscounts.Name = "tabPageDiscounts";
            this.tabPageDiscounts.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDiscounts.Size = new System.Drawing.Size(379, 655);
            this.tabPageDiscounts.TabIndex = 1;
            this.tabPageDiscounts.Text = "Discounts";
            this.tabPageDiscounts.ToolTipText = "Apply Transaction Discounts";
            this.tabPageDiscounts.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelDiscounts
            // 
            this.flowLayoutPanelDiscounts.AutoScroll = true;
            this.flowLayoutPanelDiscounts.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanelDiscounts.Controls.Add(this.SampleButtonDiscount);
            this.flowLayoutPanelDiscounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelDiscounts.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelDiscounts.Name = "flowLayoutPanelDiscounts";
            this.flowLayoutPanelDiscounts.Size = new System.Drawing.Size(369, 645);
            this.flowLayoutPanelDiscounts.TabIndex = 0;
            // 
            // SampleButtonDiscount
            // 
            this.SampleButtonDiscount.BackgroundImage = global::Parafait_POS.Properties.Resources.discount_button;
            this.SampleButtonDiscount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SampleButtonDiscount.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.SampleButtonDiscount.FlatAppearance.BorderSize = 0;
            this.SampleButtonDiscount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonDiscount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.SampleButtonDiscount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SampleButtonDiscount.Font = new System.Drawing.Font("Tahoma", 9F);
            this.SampleButtonDiscount.ForeColor = System.Drawing.Color.White;
            this.SampleButtonDiscount.Location = new System.Drawing.Point(3, 3);
            this.SampleButtonDiscount.Name = "SampleButtonDiscount";
            this.SampleButtonDiscount.Size = new System.Drawing.Size(135, 90);
            this.SampleButtonDiscount.TabIndex = 0;
            this.SampleButtonDiscount.Text = "Discount";
            // 
            // tabPageFunctions
            // 
            this.tabPageFunctions.BackColor = System.Drawing.Color.Gray;
            this.tabPageFunctions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageFunctions.Controls.Add(this.flowLayoutPanelFunctions);
            this.tabPageFunctions.Location = new System.Drawing.Point(4, 49);
            this.tabPageFunctions.Name = "tabPageFunctions";
            this.tabPageFunctions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFunctions.Size = new System.Drawing.Size(379, 655);
            this.tabPageFunctions.TabIndex = 2;
            this.tabPageFunctions.Text = "Tasks";
            this.tabPageFunctions.ToolTipText = "Card Tasks";
            this.tabPageFunctions.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelFunctions
            // 
            this.flowLayoutPanelFunctions.AutoScroll = true;
            this.flowLayoutPanelFunctions.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanelFunctions.Controls.Add(this.btnTransferCard);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnTokenForCredit);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnCreditForToken);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnLoadTickets);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnConsolidateCards);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnLoadMultiple);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnRealEticket);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnLoadBonus);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnApplyDiscount);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnRedeemLoyalty);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnSpecialPricing);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnRedeemTicketsForBonus);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnSetChildSite);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnGetMiFareDetails);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnRefundCardTask);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnTransferBalance);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnSalesReturnExchange);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnRedeemBonusForTicket);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnPauseTime);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnConvertPointsToTime);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnConvertTimeToPoints);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnholdEntitilements);
            this.flowLayoutPanelFunctions.Controls.Add(this.btnRedeemVirtualPoint);
            this.flowLayoutPanelFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelFunctions.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelFunctions.Name = "flowLayoutPanelFunctions";
            this.flowLayoutPanelFunctions.Size = new System.Drawing.Size(369, 645);
            this.flowLayoutPanelFunctions.TabIndex = 0;
            // 
            // btnTransferCard
            // 
            this.btnTransferCard.BackColor = System.Drawing.Color.Transparent;
            this.btnTransferCard.BackgroundImage = global::Parafait_POS.Properties.Resources.TransferCard;
            this.btnTransferCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTransferCard.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnTransferCard.FlatAppearance.BorderSize = 0;
            this.btnTransferCard.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTransferCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTransferCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTransferCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransferCard.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnTransferCard.ForeColor = System.Drawing.Color.Snow;
            this.btnTransferCard.Location = new System.Drawing.Point(3, 3);
            this.btnTransferCard.Name = "btnTransferCard";
            this.btnTransferCard.Size = new System.Drawing.Size(125, 80);
            this.btnTransferCard.TabIndex = 0;
            this.btnTransferCard.Tag = "TRANSFERCARD";
            this.btnTransferCard.Text = "Transfer Card";
            this.btnTransferCard.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTransferCard.UseVisualStyleBackColor = false;
            this.btnTransferCard.Click += new System.EventHandler(this.CallTask);
            this.btnTransferCard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnTransferCard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnTokenForCredit
            // 
            this.btnTokenForCredit.BackColor = System.Drawing.Color.Transparent;
            this.btnTokenForCredit.BackgroundImage = global::Parafait_POS.Properties.Resources.ExchangeTokenForCredit;
            this.btnTokenForCredit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTokenForCredit.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnTokenForCredit.FlatAppearance.BorderSize = 0;
            this.btnTokenForCredit.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTokenForCredit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTokenForCredit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTokenForCredit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTokenForCredit.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnTokenForCredit.ForeColor = System.Drawing.Color.Snow;
            this.btnTokenForCredit.Location = new System.Drawing.Point(134, 3);
            this.btnTokenForCredit.Name = "btnTokenForCredit";
            this.btnTokenForCredit.Size = new System.Drawing.Size(125, 80);
            this.btnTokenForCredit.TabIndex = 1;
            this.btnTokenForCredit.Tag = "EXCHANGETOKENFORCREDIT";
            this.btnTokenForCredit.Text = "Exchange Token for Credit";
            this.btnTokenForCredit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTokenForCredit.UseVisualStyleBackColor = false;
            this.btnTokenForCredit.Click += new System.EventHandler(this.CallNewTask);
            this.btnTokenForCredit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnTokenForCredit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnCreditForToken
            // 
            this.btnCreditForToken.BackColor = System.Drawing.Color.Transparent;
            this.btnCreditForToken.BackgroundImage = global::Parafait_POS.Properties.Resources.ExchangeCreditForToken;
            this.btnCreditForToken.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCreditForToken.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnCreditForToken.FlatAppearance.BorderSize = 0;
            this.btnCreditForToken.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCreditForToken.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCreditForToken.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCreditForToken.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreditForToken.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnCreditForToken.ForeColor = System.Drawing.Color.Snow;
            this.btnCreditForToken.Location = new System.Drawing.Point(3, 89);
            this.btnCreditForToken.Name = "btnCreditForToken";
            this.btnCreditForToken.Size = new System.Drawing.Size(125, 80);
            this.btnCreditForToken.TabIndex = 2;
            this.btnCreditForToken.Tag = "EXCHANGECREDITFORTOKEN";
            this.btnCreditForToken.Text = "Exchange Credit for Token";
            this.btnCreditForToken.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCreditForToken.UseVisualStyleBackColor = false;
            this.btnCreditForToken.Click += new System.EventHandler(this.CallNewTask);
            this.btnCreditForToken.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnCreditForToken.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnLoadTickets
            // 
            this.btnLoadTickets.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadTickets.BackgroundImage = global::Parafait_POS.Properties.Resources.LoadTickets;
            this.btnLoadTickets.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLoadTickets.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnLoadTickets.FlatAppearance.BorderSize = 0;
            this.btnLoadTickets.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnLoadTickets.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLoadTickets.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLoadTickets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadTickets.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnLoadTickets.ForeColor = System.Drawing.Color.Snow;
            this.btnLoadTickets.Location = new System.Drawing.Point(134, 89);
            this.btnLoadTickets.Name = "btnLoadTickets";
            this.btnLoadTickets.Size = new System.Drawing.Size(125, 80);
            this.btnLoadTickets.TabIndex = 3;
            this.btnLoadTickets.Tag = "LOADTICKETS";
            this.btnLoadTickets.Text = "Load Tickets";
            this.btnLoadTickets.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoadTickets.UseVisualStyleBackColor = false;
            this.btnLoadTickets.Click += new System.EventHandler(this.CallNewTask);
            this.btnLoadTickets.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnLoadTickets.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnConsolidateCards
            // 
            this.btnConsolidateCards.BackColor = System.Drawing.Color.Transparent;
            this.btnConsolidateCards.BackgroundImage = global::Parafait_POS.Properties.Resources.Consolidate;
            this.btnConsolidateCards.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnConsolidateCards.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnConsolidateCards.FlatAppearance.BorderSize = 0;
            this.btnConsolidateCards.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnConsolidateCards.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConsolidateCards.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConsolidateCards.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsolidateCards.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnConsolidateCards.ForeColor = System.Drawing.Color.Snow;
            this.btnConsolidateCards.Location = new System.Drawing.Point(3, 175);
            this.btnConsolidateCards.Name = "btnConsolidateCards";
            this.btnConsolidateCards.Size = new System.Drawing.Size(125, 80);
            this.btnConsolidateCards.TabIndex = 4;
            this.btnConsolidateCards.Tag = "CONSOLIDATE";
            this.btnConsolidateCards.Text = "Consolidate Multiple Cards";
            this.btnConsolidateCards.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnConsolidateCards.UseVisualStyleBackColor = false;
            this.btnConsolidateCards.Click += new System.EventHandler(this.CallTask);
            this.btnConsolidateCards.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnConsolidateCards.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnLoadMultiple
            // 
            this.btnLoadMultiple.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadMultiple.BackgroundImage = global::Parafait_POS.Properties.Resources.LoadMultiple;
            this.btnLoadMultiple.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLoadMultiple.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnLoadMultiple.FlatAppearance.BorderSize = 0;
            this.btnLoadMultiple.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnLoadMultiple.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLoadMultiple.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLoadMultiple.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadMultiple.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnLoadMultiple.ForeColor = System.Drawing.Color.Snow;
            this.btnLoadMultiple.Location = new System.Drawing.Point(134, 175);
            this.btnLoadMultiple.Name = "btnLoadMultiple";
            this.btnLoadMultiple.Size = new System.Drawing.Size(125, 80);
            this.btnLoadMultiple.TabIndex = 5;
            this.btnLoadMultiple.Tag = "LOADMULTIPLE";
            this.btnLoadMultiple.Text = "Load Multiple";
            this.btnLoadMultiple.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoadMultiple.UseVisualStyleBackColor = false;
            this.btnLoadMultiple.Click += new System.EventHandler(this.CallTask);
            this.btnLoadMultiple.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnLoadMultiple.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnRealEticket
            // 
            this.btnRealEticket.BackColor = System.Drawing.Color.Transparent;
            this.btnRealEticket.BackgroundImage = global::Parafait_POS.Properties.Resources.RealETicket;
            this.btnRealEticket.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRealEticket.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRealEticket.FlatAppearance.BorderSize = 0;
            this.btnRealEticket.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRealEticket.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRealEticket.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRealEticket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRealEticket.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnRealEticket.ForeColor = System.Drawing.Color.Snow;
            this.btnRealEticket.Location = new System.Drawing.Point(3, 261);
            this.btnRealEticket.Name = "btnRealEticket";
            this.btnRealEticket.Size = new System.Drawing.Size(125, 80);
            this.btnRealEticket.TabIndex = 6;
            this.btnRealEticket.Tag = "REALETICKET";
            this.btnRealEticket.Text = "Real / E-Ticket";
            this.btnRealEticket.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRealEticket.UseVisualStyleBackColor = false;
            this.btnRealEticket.Click += new System.EventHandler(this.CallNewTask);
            this.btnRealEticket.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnRealEticket.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnLoadBonus
            // 
            this.btnLoadBonus.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadBonus.BackgroundImage = global::Parafait_POS.Properties.Resources.LoadBonus;
            this.btnLoadBonus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLoadBonus.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnLoadBonus.FlatAppearance.BorderSize = 0;
            this.btnLoadBonus.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnLoadBonus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLoadBonus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLoadBonus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadBonus.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnLoadBonus.ForeColor = System.Drawing.Color.Snow;
            this.btnLoadBonus.Location = new System.Drawing.Point(134, 261);
            this.btnLoadBonus.Name = "btnLoadBonus";
            this.btnLoadBonus.Size = new System.Drawing.Size(125, 80);
            this.btnLoadBonus.TabIndex = 8;
            this.btnLoadBonus.Tag = "LOADBONUS";
            this.btnLoadBonus.Text = "Load Bonus";
            this.btnLoadBonus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoadBonus.UseVisualStyleBackColor = false;
            this.btnLoadBonus.Click += new System.EventHandler(this.CallNewTask);
            this.btnLoadBonus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnLoadBonus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnApplyDiscount
            // 
            this.btnApplyDiscount.BackColor = System.Drawing.Color.Transparent;
            this.btnApplyDiscount.BackgroundImage = global::Parafait_POS.Properties.Resources.ApplyDiscount;
            this.btnApplyDiscount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnApplyDiscount.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnApplyDiscount.FlatAppearance.BorderSize = 0;
            this.btnApplyDiscount.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnApplyDiscount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnApplyDiscount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnApplyDiscount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyDiscount.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnApplyDiscount.ForeColor = System.Drawing.Color.Snow;
            this.btnApplyDiscount.Location = new System.Drawing.Point(3, 347);
            this.btnApplyDiscount.Name = "btnApplyDiscount";
            this.btnApplyDiscount.Size = new System.Drawing.Size(125, 80);
            this.btnApplyDiscount.TabIndex = 9;
            this.btnApplyDiscount.Tag = "DISCOUNT";
            this.btnApplyDiscount.Text = "Apply Discount";
            this.btnApplyDiscount.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnApplyDiscount.UseVisualStyleBackColor = false;
            this.btnApplyDiscount.Click += new System.EventHandler(this.CallTask);
            this.btnApplyDiscount.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnApplyDiscount.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnRedeemLoyalty
            // 
            this.btnRedeemLoyalty.BackColor = System.Drawing.Color.Transparent;
            this.btnRedeemLoyalty.BackgroundImage = global::Parafait_POS.Properties.Resources.RedeemLoyalty;
            this.btnRedeemLoyalty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRedeemLoyalty.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRedeemLoyalty.FlatAppearance.BorderSize = 0;
            this.btnRedeemLoyalty.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemLoyalty.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemLoyalty.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemLoyalty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedeemLoyalty.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnRedeemLoyalty.ForeColor = System.Drawing.Color.Snow;
            this.btnRedeemLoyalty.Location = new System.Drawing.Point(134, 347);
            this.btnRedeemLoyalty.Name = "btnRedeemLoyalty";
            this.btnRedeemLoyalty.Size = new System.Drawing.Size(125, 80);
            this.btnRedeemLoyalty.TabIndex = 10;
            this.btnRedeemLoyalty.Tag = "REDEEMLOYALTY";
            this.btnRedeemLoyalty.Text = "Redeem Loyalty Points";
            this.btnRedeemLoyalty.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRedeemLoyalty.UseVisualStyleBackColor = false;
            this.btnRedeemLoyalty.Click += new System.EventHandler(this.CallNewTask);
            this.btnRedeemLoyalty.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnRedeemLoyalty.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnSpecialPricing
            // 
            this.btnSpecialPricing.BackColor = System.Drawing.Color.Transparent;
            this.btnSpecialPricing.BackgroundImage = global::Parafait_POS.Properties.Resources.SpecialPricing;
            this.btnSpecialPricing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSpecialPricing.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSpecialPricing.FlatAppearance.BorderSize = 0;
            this.btnSpecialPricing.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSpecialPricing.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSpecialPricing.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSpecialPricing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSpecialPricing.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnSpecialPricing.ForeColor = System.Drawing.Color.Snow;
            this.btnSpecialPricing.Location = new System.Drawing.Point(3, 433);
            this.btnSpecialPricing.Name = "btnSpecialPricing";
            this.btnSpecialPricing.Size = new System.Drawing.Size(125, 80);
            this.btnSpecialPricing.TabIndex = 11;
            this.btnSpecialPricing.Tag = "SPECIALPRICING";
            this.btnSpecialPricing.Text = "Special Pricing";
            this.btnSpecialPricing.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSpecialPricing.UseVisualStyleBackColor = false;
            this.btnSpecialPricing.Click += new System.EventHandler(this.CallTask);
            this.btnSpecialPricing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnSpecialPricing.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnRedeemTicketsForBonus
            // 
            this.btnRedeemTicketsForBonus.BackColor = System.Drawing.Color.Transparent;
            this.btnRedeemTicketsForBonus.BackgroundImage = global::Parafait_POS.Properties.Resources.RedeemTickets;
            this.btnRedeemTicketsForBonus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRedeemTicketsForBonus.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRedeemTicketsForBonus.FlatAppearance.BorderSize = 0;
            this.btnRedeemTicketsForBonus.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemTicketsForBonus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemTicketsForBonus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemTicketsForBonus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedeemTicketsForBonus.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnRedeemTicketsForBonus.ForeColor = System.Drawing.Color.Snow;
            this.btnRedeemTicketsForBonus.Location = new System.Drawing.Point(134, 433);
            this.btnRedeemTicketsForBonus.Name = "btnRedeemTicketsForBonus";
            this.btnRedeemTicketsForBonus.Size = new System.Drawing.Size(125, 80);
            this.btnRedeemTicketsForBonus.TabIndex = 12;
            this.btnRedeemTicketsForBonus.Tag = "REDEEMTICKETSFORBONUS";
            this.btnRedeemTicketsForBonus.Text = "Redeem Tickets";
            this.btnRedeemTicketsForBonus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRedeemTicketsForBonus.UseVisualStyleBackColor = false;
            this.btnRedeemTicketsForBonus.Click += new System.EventHandler(this.CallNewTask);
            this.btnRedeemTicketsForBonus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnRedeemTicketsForBonus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnSetChildSite
            // 
            this.btnSetChildSite.BackColor = System.Drawing.Color.IndianRed;
            this.btnSetChildSite.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSetChildSite.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSetChildSite.FlatAppearance.BorderSize = 0;
            this.btnSetChildSite.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSetChildSite.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSetChildSite.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSetChildSite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetChildSite.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnSetChildSite.ForeColor = System.Drawing.Color.Snow;
            this.btnSetChildSite.Location = new System.Drawing.Point(3, 519);
            this.btnSetChildSite.Name = "btnSetChildSite";
            this.btnSetChildSite.Size = new System.Drawing.Size(125, 80);
            this.btnSetChildSite.TabIndex = 13;
            this.btnSetChildSite.Tag = "SETCHILDSITECODE";
            this.btnSetChildSite.Text = "Set MiFare Site Code";
            this.btnSetChildSite.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSetChildSite.UseVisualStyleBackColor = false;
            this.btnSetChildSite.Click += new System.EventHandler(this.CallTask);
            this.btnSetChildSite.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnSetChildSite.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnGetMiFareDetails
            // 
            this.btnGetMiFareDetails.BackColor = System.Drawing.Color.IndianRed;
            this.btnGetMiFareDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGetMiFareDetails.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnGetMiFareDetails.FlatAppearance.BorderSize = 0;
            this.btnGetMiFareDetails.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnGetMiFareDetails.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGetMiFareDetails.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGetMiFareDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetMiFareDetails.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnGetMiFareDetails.ForeColor = System.Drawing.Color.Snow;
            this.btnGetMiFareDetails.Location = new System.Drawing.Point(134, 519);
            this.btnGetMiFareDetails.Name = "btnGetMiFareDetails";
            this.btnGetMiFareDetails.Size = new System.Drawing.Size(125, 80);
            this.btnGetMiFareDetails.TabIndex = 14;
            this.btnGetMiFareDetails.Tag = "GETMIFAREGAMEPLAY";
            this.btnGetMiFareDetails.Text = "Get MiFare Gameplay Details";
            this.btnGetMiFareDetails.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGetMiFareDetails.UseVisualStyleBackColor = false;
            this.btnGetMiFareDetails.Click += new System.EventHandler(this.CallTask);
            this.btnGetMiFareDetails.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnGetMiFareDetails.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnRefundCardTask
            // 
            this.btnRefundCardTask.BackColor = System.Drawing.Color.Transparent;
            this.btnRefundCardTask.BackgroundImage = global::Parafait_POS.Properties.Resources.refund_card_task_normal;
            this.btnRefundCardTask.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefundCardTask.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRefundCardTask.FlatAppearance.BorderSize = 0;
            this.btnRefundCardTask.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRefundCardTask.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefundCardTask.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefundCardTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefundCardTask.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnRefundCardTask.ForeColor = System.Drawing.Color.Snow;
            this.btnRefundCardTask.Location = new System.Drawing.Point(3, 605);
            this.btnRefundCardTask.Name = "btnRefundCardTask";
            this.btnRefundCardTask.Size = new System.Drawing.Size(125, 80);
            this.btnRefundCardTask.TabIndex = 15;
            this.btnRefundCardTask.Tag = "REFUNDCARDTASK";
            this.btnRefundCardTask.Text = "Refund Card";
            this.btnRefundCardTask.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRefundCardTask.UseVisualStyleBackColor = false;
            this.btnRefundCardTask.Click += new System.EventHandler(this.CallTask);
            this.btnRefundCardTask.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnRefundCardTask.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnTransferBalance
            // 
            this.btnTransferBalance.BackColor = System.Drawing.Color.Transparent;
            this.btnTransferBalance.BackgroundImage = global::Parafait_POS.Properties.Resources.transfer_balance_normal;
            this.btnTransferBalance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTransferBalance.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnTransferBalance.FlatAppearance.BorderSize = 0;
            this.btnTransferBalance.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTransferBalance.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTransferBalance.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTransferBalance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransferBalance.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnTransferBalance.ForeColor = System.Drawing.Color.Snow;
            this.btnTransferBalance.Location = new System.Drawing.Point(134, 605);
            this.btnTransferBalance.Name = "btnTransferBalance";
            this.btnTransferBalance.Size = new System.Drawing.Size(125, 80);
            this.btnTransferBalance.TabIndex = 16;
            this.btnTransferBalance.Tag = "BALANCETRANSFER";
            this.btnTransferBalance.Text = "Transfer Balance";
            this.btnTransferBalance.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTransferBalance.UseVisualStyleBackColor = false;
            this.btnTransferBalance.Click += new System.EventHandler(this.CallNewTask);
            this.btnTransferBalance.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnTransferBalance.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnSalesReturnExchange
            // 
            this.btnSalesReturnExchange.BackColor = System.Drawing.Color.Transparent;
            this.btnSalesReturnExchange.BackgroundImage = global::Parafait_POS.Properties.Resources.return_normal;
            this.btnSalesReturnExchange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSalesReturnExchange.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSalesReturnExchange.FlatAppearance.BorderSize = 0;
            this.btnSalesReturnExchange.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSalesReturnExchange.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSalesReturnExchange.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSalesReturnExchange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalesReturnExchange.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnSalesReturnExchange.ForeColor = System.Drawing.Color.Snow;
            this.btnSalesReturnExchange.Location = new System.Drawing.Point(3, 691);
            this.btnSalesReturnExchange.Name = "btnSalesReturnExchange";
            this.btnSalesReturnExchange.Size = new System.Drawing.Size(125, 80);
            this.btnSalesReturnExchange.TabIndex = 17;
            this.btnSalesReturnExchange.Tag = "SALESRETURNEXCHANGE";
            this.btnSalesReturnExchange.Text = "Sales Return/Exchange";
            this.btnSalesReturnExchange.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSalesReturnExchange.UseVisualStyleBackColor = false;
            this.btnSalesReturnExchange.Click += new System.EventHandler(this.CallTask);
            // 
            // btnRedeemBonusForTicket
            // 
            this.btnRedeemBonusForTicket.BackColor = System.Drawing.Color.Transparent;
            this.btnRedeemBonusForTicket.BackgroundImage = global::Parafait_POS.Properties.Resources.DollarToTicket_normal;
            this.btnRedeemBonusForTicket.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRedeemBonusForTicket.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRedeemBonusForTicket.FlatAppearance.BorderSize = 0;
            this.btnRedeemBonusForTicket.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemBonusForTicket.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemBonusForTicket.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemBonusForTicket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedeemBonusForTicket.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnRedeemBonusForTicket.ForeColor = System.Drawing.Color.Snow;
            this.btnRedeemBonusForTicket.Location = new System.Drawing.Point(134, 691);
            this.btnRedeemBonusForTicket.Name = "btnRedeemBonusForTicket";
            this.btnRedeemBonusForTicket.Size = new System.Drawing.Size(125, 80);
            this.btnRedeemBonusForTicket.TabIndex = 18;
            this.btnRedeemBonusForTicket.Tag = "REDEEMBONUSFORTICKET";
            this.btnRedeemBonusForTicket.Text = "Redeem Bonus for Ticket ";
            this.btnRedeemBonusForTicket.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRedeemBonusForTicket.UseVisualStyleBackColor = false;
            this.btnRedeemBonusForTicket.Click += new System.EventHandler(this.CallNewTask);
            this.btnRedeemBonusForTicket.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnRedeemBonusForTicket.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnPauseTime
            // 
            this.btnPauseTime.BackColor = System.Drawing.Color.Transparent;
            this.btnPauseTime.BackgroundImage = global::Parafait_POS.Properties.Resources.Pause_Time_normal;
            this.btnPauseTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPauseTime.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnPauseTime.FlatAppearance.BorderSize = 0;
            this.btnPauseTime.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPauseTime.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPauseTime.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPauseTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPauseTime.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnPauseTime.ForeColor = System.Drawing.Color.Snow;
            this.btnPauseTime.Location = new System.Drawing.Point(3, 777);
            this.btnPauseTime.Name = "btnPauseTime";
            this.btnPauseTime.Size = new System.Drawing.Size(125, 80);
            this.btnPauseTime.TabIndex = 19;
            this.btnPauseTime.Tag = "PAUSETIMEENTITLEMENT";
            this.btnPauseTime.Text = "Pause Card";
            this.btnPauseTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPauseTime.UseVisualStyleBackColor = false;
            this.btnPauseTime.Click += new System.EventHandler(this.CallNewTask);
            this.btnPauseTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnPauseTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnConvertPointsToTime
            // 
            this.btnConvertPointsToTime.BackColor = System.Drawing.Color.Transparent;
            this.btnConvertPointsToTime.BackgroundImage = global::Parafait_POS.Properties.Resources.Points_to_Time_normal;
            this.btnConvertPointsToTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnConvertPointsToTime.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnConvertPointsToTime.FlatAppearance.BorderSize = 0;
            this.btnConvertPointsToTime.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnConvertPointsToTime.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConvertPointsToTime.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConvertPointsToTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConvertPointsToTime.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnConvertPointsToTime.ForeColor = System.Drawing.Color.Snow;
            this.btnConvertPointsToTime.Location = new System.Drawing.Point(134, 777);
            this.btnConvertPointsToTime.Name = "btnConvertPointsToTime";
            this.btnConvertPointsToTime.Size = new System.Drawing.Size(125, 80);
            this.btnConvertPointsToTime.TabIndex = 20;
            this.btnConvertPointsToTime.Tag = "EXCHANGECREDITFORTIME";
            this.btnConvertPointsToTime.Text = "Convert Points to Time";
            this.btnConvertPointsToTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnConvertPointsToTime.UseVisualStyleBackColor = false;
            this.btnConvertPointsToTime.Click += new System.EventHandler(this.CallNewTask);
            this.btnConvertPointsToTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnConvertPointsToTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnConvertTimeToPoints
            // 
            this.btnConvertTimeToPoints.BackColor = System.Drawing.Color.Transparent;
            this.btnConvertTimeToPoints.BackgroundImage = global::Parafait_POS.Properties.Resources.Time_to_Points_Normal;
            this.btnConvertTimeToPoints.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnConvertTimeToPoints.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnConvertTimeToPoints.FlatAppearance.BorderSize = 0;
            this.btnConvertTimeToPoints.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnConvertTimeToPoints.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConvertTimeToPoints.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConvertTimeToPoints.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConvertTimeToPoints.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnConvertTimeToPoints.ForeColor = System.Drawing.Color.Snow;
            this.btnConvertTimeToPoints.Location = new System.Drawing.Point(3, 863);
            this.btnConvertTimeToPoints.Name = "btnConvertTimeToPoints";
            this.btnConvertTimeToPoints.Size = new System.Drawing.Size(125, 80);
            this.btnConvertTimeToPoints.TabIndex = 20;
            this.btnConvertTimeToPoints.Tag = "EXCHANGETIMEFORCREDIT";
            this.btnConvertTimeToPoints.Text = "Convert Time to Points";
            this.btnConvertTimeToPoints.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnConvertTimeToPoints.UseVisualStyleBackColor = false;
            this.btnConvertTimeToPoints.Click += new System.EventHandler(this.CallNewTask);
            this.btnConvertTimeToPoints.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnConvertTimeToPoints.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnholdEntitilements
            // 
            this.btnholdEntitilements.BackColor = System.Drawing.Color.Transparent;
            this.btnholdEntitilements.BackgroundImage = global::Parafait_POS.Properties.Resources.HoldEntitlement;
            this.btnholdEntitilements.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnholdEntitilements.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnholdEntitilements.FlatAppearance.BorderSize = 0;
            this.btnholdEntitilements.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnholdEntitilements.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnholdEntitilements.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnholdEntitilements.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnholdEntitilements.ForeColor = System.Drawing.Color.Snow;
            this.btnholdEntitilements.Location = new System.Drawing.Point(134, 863);
            this.btnholdEntitilements.Name = "btnholdEntitilements";
            this.btnholdEntitilements.Size = new System.Drawing.Size(125, 80);
            this.btnholdEntitilements.TabIndex = 20;
            this.btnholdEntitilements.Tag = "HOLDENTITLEMENTS";
            this.btnholdEntitilements.Text = "Hold Entitlements";
            this.btnholdEntitilements.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnholdEntitilements.UseVisualStyleBackColor = false;
            this.btnholdEntitilements.Click += new System.EventHandler(this.CallTask);
            this.btnholdEntitilements.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnholdEntitilements.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // btnRedeemVirtualPoint
            // 
            this.btnRedeemVirtualPoint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRedeemVirtualPoint.BackgroundImage")));
            this.btnRedeemVirtualPoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRedeemVirtualPoint.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnRedeemVirtualPoint.FlatAppearance.BorderSize = 0;
            this.btnRedeemVirtualPoint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemVirtualPoint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemVirtualPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedeemVirtualPoint.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnRedeemVirtualPoint.ForeColor = System.Drawing.Color.Snow;
            this.btnRedeemVirtualPoint.Location = new System.Drawing.Point(3, 949);
            this.btnRedeemVirtualPoint.Name = "btnRedeemVirtualPoint";
            this.btnRedeemVirtualPoint.Size = new System.Drawing.Size(125, 80);
            this.btnRedeemVirtualPoint.TabIndex = 21;
            this.btnRedeemVirtualPoint.Tag = "REDEEMVIRTUALPOINTS";
            this.btnRedeemVirtualPoint.Text = "Redeem Virtual Points";
            this.btnRedeemVirtualPoint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRedeemVirtualPoint.UseVisualStyleBackColor = true;
            this.btnRedeemVirtualPoint.Click += new System.EventHandler(this.CallTask);
            this.btnRedeemVirtualPoint.MouseDown += new System.Windows.Forms.MouseEventHandler(this.functionButtonMouseDown);
            this.btnRedeemVirtualPoint.MouseUp += new System.Windows.Forms.MouseEventHandler(this.functionButtionMouseUp);
            // 
            // tabPageRedeem
            // 
            this.tabPageRedeem.BackColor = System.Drawing.Color.Gray;
            this.tabPageRedeem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageRedeem.Controls.Add(this.pbRedeem);
            this.tabPageRedeem.Location = new System.Drawing.Point(4, 49);
            this.tabPageRedeem.Name = "tabPageRedeem";
            this.tabPageRedeem.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRedeem.Size = new System.Drawing.Size(379, 655);
            this.tabPageRedeem.TabIndex = 4;
            this.tabPageRedeem.Text = "Redeem";
            this.tabPageRedeem.ToolTipText = "Redeem Tickets for Gifts";
            // 
            // pbRedeem
            // 
            this.pbRedeem.Image = global::Parafait_POS.Properties.Resources.redeem;
            this.pbRedeem.Location = new System.Drawing.Point(110, 28);
            this.pbRedeem.Name = "pbRedeem";
            this.pbRedeem.Size = new System.Drawing.Size(130, 130);
            this.pbRedeem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbRedeem.TabIndex = 0;
            this.pbRedeem.TabStop = false;
            this.pbRedeem.Click += new System.EventHandler(this.pbRedeem_Click);
            this.pbRedeem.MouseEnter += new System.EventHandler(this.pbRedeem_MouseEnter);
            this.pbRedeem.MouseLeave += new System.EventHandler(this.pbRedeem_MouseLeave);
            // 
            // tabPageSystem
            // 
            this.tabPageSystem.AutoScroll = true;
            this.tabPageSystem.BackColor = System.Drawing.Color.Gray;
            this.tabPageSystem.BackgroundImage = global::Parafait_POS.Properties.Resources.box1;
            this.tabPageSystem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPageSystem.Controls.Add(this.panel1);
            this.tabPageSystem.Controls.Add(this.panelSkinColor);
            this.tabPageSystem.Controls.Add(this.panelPassword);
            this.tabPageSystem.Controls.Add(this.panel3);
            this.tabPageSystem.Controls.Add(this.pictureBox2);
            this.tabPageSystem.Location = new System.Drawing.Point(4, 49);
            this.tabPageSystem.Name = "tabPageSystem";
            this.tabPageSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSystem.Size = new System.Drawing.Size(379, 655);
            this.tabPageSystem.TabIndex = 3;
            this.tabPageSystem.Text = "Tools";
            this.tabPageSystem.ToolTipText = "System Functions such as Logout, Change Password";
            this.tabPageSystem.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Parafait_POS.Properties.Resources.box3;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.buttonReConnectCardReader);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Location = new System.Drawing.Point(8, 258);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(317, 63);
            this.panel1.TabIndex = 6;
            // 
            // buttonReConnectCardReader
            // 
            this.buttonReConnectCardReader.BackColor = System.Drawing.Color.Transparent;
            this.buttonReConnectCardReader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonReConnectCardReader.FlatAppearance.BorderSize = 0;
            this.buttonReConnectCardReader.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonReConnectCardReader.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonReConnectCardReader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReConnectCardReader.Image = global::Parafait_POS.Properties.Resources.re_connect_button;
            this.buttonReConnectCardReader.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonReConnectCardReader.Location = new System.Drawing.Point(158, 8);
            this.buttonReConnectCardReader.Name = "buttonReConnectCardReader";
            this.buttonReConnectCardReader.Size = new System.Drawing.Size(152, 43);
            this.buttonReConnectCardReader.TabIndex = 3;
            this.buttonReConnectCardReader.Text = "    Re-Connect";
            this.buttonReConnectCardReader.UseVisualStyleBackColor = false;
            this.buttonReConnectCardReader.Click += new System.EventHandler(this.buttonReConnectCardReader_Click);
            this.buttonReConnectCardReader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonReConnectCardReader_MouseDown);
            this.buttonReConnectCardReader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonReConnectCardReader_MouseUp);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(4, 4);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(88, 16);
            this.label22.TabIndex = 1;
            this.label22.Text = "Card Reader";
            // 
            // panelSkinColor
            // 
            this.panelSkinColor.BackgroundImage = global::Parafait_POS.Properties.Resources.box3;
            this.panelSkinColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelSkinColor.Controls.Add(this.buttonChangeSkinColor);
            this.panelSkinColor.Controls.Add(this.buttonSkinColorReset);
            this.panelSkinColor.Controls.Add(this.label21);
            this.panelSkinColor.Location = new System.Drawing.Point(8, 174);
            this.panelSkinColor.Name = "panelSkinColor";
            this.panelSkinColor.Size = new System.Drawing.Size(317, 80);
            this.panelSkinColor.TabIndex = 2;
            // 
            // buttonChangeSkinColor
            // 
            this.buttonChangeSkinColor.BackColor = System.Drawing.Color.Transparent;
            this.buttonChangeSkinColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonChangeSkinColor.FlatAppearance.BorderSize = 0;
            this.buttonChangeSkinColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonChangeSkinColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonChangeSkinColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChangeSkinColor.Image = global::Parafait_POS.Properties.Resources.change_color_button;
            this.buttonChangeSkinColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonChangeSkinColor.Location = new System.Drawing.Point(7, 28);
            this.buttonChangeSkinColor.Name = "buttonChangeSkinColor";
            this.buttonChangeSkinColor.Size = new System.Drawing.Size(152, 43);
            this.buttonChangeSkinColor.TabIndex = 5;
            this.buttonChangeSkinColor.Text = "   Change";
            this.buttonChangeSkinColor.UseVisualStyleBackColor = false;
            this.buttonChangeSkinColor.Click += new System.EventHandler(this.buttonChangeSkinColor_Click);
            this.buttonChangeSkinColor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonChangeSkinColor_MouseDown);
            this.buttonChangeSkinColor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonChangeSkinColor_MouseUp);
            // 
            // buttonSkinColorReset
            // 
            this.buttonSkinColorReset.BackColor = System.Drawing.Color.Transparent;
            this.buttonSkinColorReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonSkinColorReset.FlatAppearance.BorderSize = 0;
            this.buttonSkinColorReset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonSkinColorReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonSkinColorReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSkinColorReset.Image = global::Parafait_POS.Properties.Resources.reset_button;
            this.buttonSkinColorReset.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSkinColorReset.Location = new System.Drawing.Point(158, 28);
            this.buttonSkinColorReset.Name = "buttonSkinColorReset";
            this.buttonSkinColorReset.Size = new System.Drawing.Size(152, 43);
            this.buttonSkinColorReset.TabIndex = 3;
            this.buttonSkinColorReset.Text = "   Reset";
            this.buttonSkinColorReset.UseVisualStyleBackColor = false;
            this.buttonSkinColorReset.Click += new System.EventHandler(this.buttonSkinColorReset_Click);
            this.buttonSkinColorReset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSkinColorReset_MouseDown);
            this.buttonSkinColorReset.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSkinColorReset_MouseUp);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(4, 4);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(159, 16);
            this.label21.TabIndex = 1;
            this.label21.Text = "Change POS Skin Color";
            // 
            // panelPassword
            // 
            this.panelPassword.BackgroundImage = global::Parafait_POS.Properties.Resources.box3;
            this.panelPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelPassword.Controls.Add(this.buttonChangePassword);
            this.panelPassword.Controls.Add(this.textBoxReenterNewPassword);
            this.panelPassword.Controls.Add(this.textBoxNewPassword);
            this.panelPassword.Controls.Add(this.textBoxCurrentPassword);
            this.panelPassword.Controls.Add(this.label20);
            this.panelPassword.Controls.Add(this.label19);
            this.panelPassword.Controls.Add(this.label18);
            this.panelPassword.Controls.Add(this.label17);
            this.panelPassword.Location = new System.Drawing.Point(8, 9);
            this.panelPassword.Name = "panelPassword";
            this.panelPassword.Size = new System.Drawing.Size(317, 161);
            this.panelPassword.TabIndex = 1;
            // 
            // buttonChangePassword
            // 
            this.buttonChangePassword.BackColor = System.Drawing.Color.Transparent;
            this.buttonChangePassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonChangePassword.FlatAppearance.BorderSize = 0;
            this.buttonChangePassword.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonChangePassword.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonChangePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChangePassword.Image = global::Parafait_POS.Properties.Resources.ChangePassword;
            this.buttonChangePassword.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonChangePassword.Location = new System.Drawing.Point(158, 112);
            this.buttonChangePassword.Name = "buttonChangePassword";
            this.buttonChangePassword.Size = new System.Drawing.Size(152, 43);
            this.buttonChangePassword.TabIndex = 7;
            this.buttonChangePassword.Text = "    Change";
            this.buttonChangePassword.UseVisualStyleBackColor = false;
            this.buttonChangePassword.Click += new System.EventHandler(this.buttonChangePassword_Click);
            this.buttonChangePassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonChangePassword_MouseDown);
            this.buttonChangePassword.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonChangePassword_MouseUp);
            // 
            // textBoxReenterNewPassword
            // 
            this.textBoxReenterNewPassword.Location = new System.Drawing.Point(163, 87);
            this.textBoxReenterNewPassword.Name = "textBoxReenterNewPassword";
            this.textBoxReenterNewPassword.PasswordChar = 'x';
            this.textBoxReenterNewPassword.Size = new System.Drawing.Size(143, 22);
            this.textBoxReenterNewPassword.TabIndex = 6;
            this.textBoxReenterNewPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.General_KeyDown);
            // 
            // textBoxNewPassword
            // 
            this.textBoxNewPassword.Location = new System.Drawing.Point(163, 61);
            this.textBoxNewPassword.Name = "textBoxNewPassword";
            this.textBoxNewPassword.PasswordChar = 'x';
            this.textBoxNewPassword.Size = new System.Drawing.Size(143, 22);
            this.textBoxNewPassword.TabIndex = 5;
            this.textBoxNewPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.General_KeyDown);
            // 
            // textBoxCurrentPassword
            // 
            this.textBoxCurrentPassword.Location = new System.Drawing.Point(163, 35);
            this.textBoxCurrentPassword.Name = "textBoxCurrentPassword";
            this.textBoxCurrentPassword.PasswordChar = 'x';
            this.textBoxCurrentPassword.Size = new System.Drawing.Size(143, 22);
            this.textBoxCurrentPassword.TabIndex = 4;
            this.textBoxCurrentPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.General_KeyDown);
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("Arial", 9.25F);
            this.label20.ForeColor = System.Drawing.Color.Snow;
            this.label20.Location = new System.Drawing.Point(10, 90);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(151, 16);
            this.label20.TabIndex = 3;
            this.label20.Text = "Re-enter New Password:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Arial", 9.25F);
            this.label19.ForeColor = System.Drawing.Color.Snow;
            this.label19.Location = new System.Drawing.Point(10, 64);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(151, 16);
            this.label19.TabIndex = 2;
            this.label19.Text = "New Password:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Arial", 9.25F);
            this.label18.ForeColor = System.Drawing.Color.Snow;
            this.label18.Location = new System.Drawing.Point(10, 37);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(151, 16);
            this.label18.TabIndex = 1;
            this.label18.Text = "Current Password:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(4, 4);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(122, 16);
            this.label17.TabIndex = 0;
            this.label17.Text = "Change Password";
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = global::Parafait_POS.Properties.Resources.box3;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Controls.Add(this.cmbPaymentMode);
            this.panel3.Controls.Add(this.label23);
            this.panel3.Location = new System.Drawing.Point(8, 394);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(317, 59);
            this.panel3.TabIndex = 8;
            // 
            // cmbPaymentMode
            // 
            this.cmbPaymentMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMode.Font = new System.Drawing.Font("Arial", 14.25F);
            this.cmbPaymentMode.FormattingEnabled = true;
            this.cmbPaymentMode.Location = new System.Drawing.Point(163, 13);
            this.cmbPaymentMode.Name = "cmbPaymentMode";
            this.cmbPaymentMode.Size = new System.Drawing.Size(143, 30);
            this.cmbPaymentMode.TabIndex = 14;
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(3, 16);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(154, 22);
            this.label23.TabIndex = 13;
            this.label23.Text = "Default Pay Mode";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::Parafait_POS.Properties.Resources.SemnoxParafaitLogos;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Location = new System.Drawing.Point(6, 462);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(319, 133);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // textBoxMessageLine
            // 
            this.textBoxMessageLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessageLine.BackColor = System.Drawing.Color.White;
            this.textBoxMessageLine.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBoxMessageLine.Font = new System.Drawing.Font("Arial", 15.75F);
            this.textBoxMessageLine.ForeColor = System.Drawing.Color.Red;
            this.textBoxMessageLine.Location = new System.Drawing.Point(-2, 677);
            this.textBoxMessageLine.Name = "textBoxMessageLine";
            this.textBoxMessageLine.ReadOnly = true;
            this.textBoxMessageLine.Size = new System.Drawing.Size(552, 32);
            this.textBoxMessageLine.TabIndex = 4;
            this.textBoxMessageLine.TabStop = false;
            this.textBoxMessageLine.Text = "Swipe Card";
            // 
            // tabControlCardAction
            // 
            this.tabControlCardAction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlCardAction.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlCardAction.Controls.Add(this.tabPageTrx);
            this.tabControlCardAction.Controls.Add(this.tabPageActivities);
            this.tabControlCardAction.Controls.Add(this.tabPageCardInfo);
            this.tabControlCardAction.Controls.Add(this.tabPageMyTrx);
            this.tabControlCardAction.Controls.Add(this.tpBookings);
            this.tabControlCardAction.Controls.Add(this.tpOpenOrders);
            this.tabControlCardAction.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.tabControlCardAction.ItemSize = new System.Drawing.Size(21, 40);
            this.tabControlCardAction.Location = new System.Drawing.Point(0, -2);
            this.tabControlCardAction.Name = "tabControlCardAction";
            this.tabControlCardAction.SelectedIndex = 0;
            this.tabControlCardAction.Size = new System.Drawing.Size(550, 679);
            this.tabControlCardAction.TabIndex = 11;
            this.tabControlCardAction.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlCardAction_Selected);
            // 
            // tabPageTrx
            // 
            this.tabPageTrx.BackColor = System.Drawing.Color.Transparent;
            this.tabPageTrx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tabPageTrx.Controls.Add(this.flpTrxProfiles);
            this.tabPageTrx.Controls.Add(this.dgvTrxSummary);
            this.tabPageTrx.Controls.Add(this.panelTrxButtons);
            this.tabPageTrx.Controls.Add(this.dataGridViewTransaction);
            this.tabPageTrx.Font = new System.Drawing.Font("Arial", 8.25F);
            this.tabPageTrx.ImageIndex = 1;
            this.tabPageTrx.Location = new System.Drawing.Point(4, 44);
            this.tabPageTrx.Name = "tabPageTrx";
            this.tabPageTrx.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTrx.Size = new System.Drawing.Size(542, 631);
            this.tabPageTrx.TabIndex = 0;
            this.tabPageTrx.Text = "Transaction";
            // 
            // flpTrxProfiles
            // 
            this.flpTrxProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flpTrxProfiles.BackColor = System.Drawing.Color.DarkGray;
            this.flpTrxProfiles.Controls.Add(this.btnTrxProfileDefault);
            this.flpTrxProfiles.Location = new System.Drawing.Point(421, 429);
            this.flpTrxProfiles.Name = "flpTrxProfiles";
            this.flpTrxProfiles.Size = new System.Drawing.Size(118, 79);
            this.flpTrxProfiles.TabIndex = 19;
            // 
            // btnTrxProfileDefault
            // 
            this.btnTrxProfileDefault.BackColor = System.Drawing.Color.Transparent;
            this.btnTrxProfileDefault.BackgroundImage = global::Parafait_POS.Properties.Resources.RoyalBlueBox;
            this.btnTrxProfileDefault.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTrxProfileDefault.FlatAppearance.BorderColor = System.Drawing.Color.Azure;
            this.btnTrxProfileDefault.FlatAppearance.BorderSize = 0;
            this.btnTrxProfileDefault.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTrxProfileDefault.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTrxProfileDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrxProfileDefault.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrxProfileDefault.ForeColor = System.Drawing.Color.Black;
            this.btnTrxProfileDefault.Location = new System.Drawing.Point(4, 4);
            this.btnTrxProfileDefault.Margin = new System.Windows.Forms.Padding(4, 4, 2, 1);
            this.btnTrxProfileDefault.Name = "btnTrxProfileDefault";
            this.btnTrxProfileDefault.Size = new System.Drawing.Size(109, 71);
            this.btnTrxProfileDefault.TabIndex = 0;
            this.btnTrxProfileDefault.Tag = "-1";
            this.btnTrxProfileDefault.Text = "Transaction Profile Options";
            this.btnTrxProfileDefault.UseVisualStyleBackColor = false;
            // 
            // dgvTrxSummary
            // 
            this.dgvTrxSummary.AllowUserToAddRows = false;
            this.dgvTrxSummary.AllowUserToDeleteRows = false;
            this.dgvTrxSummary.AllowUserToOrderColumns = true;
            this.dgvTrxSummary.AllowUserToResizeColumns = false;
            this.dgvTrxSummary.AllowUserToResizeRows = false;
            this.dgvTrxSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTrxSummary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTrxSummary.BackgroundColor = System.Drawing.Color.DarkGray;
            this.dgvTrxSummary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTrxSummary.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Arial", 8.25F);
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTrxSummary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle20;
            this.dgvTrxSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrxSummary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductType,
            this.ProductQuantity,
            this.Amount});
            this.dgvTrxSummary.EnableHeadersVisualStyles = false;
            this.dgvTrxSummary.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvTrxSummary.Location = new System.Drawing.Point(4, 429);
            this.dgvTrxSummary.Name = "dgvTrxSummary";
            this.dgvTrxSummary.ReadOnly = true;
            this.dgvTrxSummary.RowHeadersVisible = false;
            this.dgvTrxSummary.Size = new System.Drawing.Size(416, 79);
            this.dgvTrxSummary.TabIndex = 0;
            // 
            // ProductType
            // 
            this.ProductType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ProductType.HeaderText = "Summary";
            this.ProductType.Name = "ProductType";
            this.ProductType.ReadOnly = true;
            // 
            // ProductQuantity
            // 
            this.ProductQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ProductQuantity.HeaderText = "Quantity";
            this.ProductQuantity.Name = "ProductQuantity";
            this.ProductQuantity.ReadOnly = true;
            this.ProductQuantity.Width = 70;
            // 
            // Amount
            // 
            this.Amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            this.Amount.Width = 67;
            // 
            // panelTrxButtons
            // 
            this.panelTrxButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTrxButtons.Controls.Add(this.flpTrxButtons);
            this.panelTrxButtons.Controls.Add(this.flpOrderButtons);
            this.panelTrxButtons.Location = new System.Drawing.Point(3, 511);
            this.panelTrxButtons.Name = "panelTrxButtons";
            this.panelTrxButtons.Size = new System.Drawing.Size(536, 117);
            this.panelTrxButtons.TabIndex = 17;
            this.panelTrxButtons.Resize += new System.EventHandler(this.panelTrxButtons_Resize);
            // 
            // flpTrxButtons
            // 
            this.flpTrxButtons.Controls.Add(this.buttonSaveTransaction);
            this.flpTrxButtons.Controls.Add(this.btnPayment);
            this.flpTrxButtons.Controls.Add(this.buttonCancelTransaction);
            this.flpTrxButtons.Controls.Add(this.buttonCancelLine);
            this.flpTrxButtons.Controls.Add(this.btnPrint);
            this.flpTrxButtons.Location = new System.Drawing.Point(3, 2);
            this.flpTrxButtons.Name = "flpTrxButtons";
            this.flpTrxButtons.Size = new System.Drawing.Size(530, 53);
            this.flpTrxButtons.TabIndex = 8;
            // 
            // buttonSaveTransaction
            // 
            this.buttonSaveTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSaveTransaction.AutoEllipsis = true;
            this.buttonSaveTransaction.BackgroundImage = global::Parafait_POS.Properties.Resources.NewTrx;
            this.buttonSaveTransaction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSaveTransaction.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonSaveTransaction.FlatAppearance.BorderSize = 0;
            this.buttonSaveTransaction.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonSaveTransaction.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonSaveTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveTransaction.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSaveTransaction.ForeColor = System.Drawing.Color.White;
            this.buttonSaveTransaction.Location = new System.Drawing.Point(3, 3);
            this.buttonSaveTransaction.Name = "buttonSaveTransaction";
            this.buttonSaveTransaction.Size = new System.Drawing.Size(99, 49);
            this.buttonSaveTransaction.TabIndex = 0;
            this.buttonSaveTransaction.Text = "Complete";
            this.buttonSaveTransaction.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSaveTransaction.UseVisualStyleBackColor = false;
            this.buttonSaveTransaction.Click += new System.EventHandler(this.buttonSaveTransaction_Click);
            this.buttonSaveTransaction.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseDown);
            this.buttonSaveTransaction.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseUp);
            // 
            // btnPayment
            // 
            this.btnPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPayment.AutoEllipsis = true;
            this.btnPayment.BackgroundImage = global::Parafait_POS.Properties.Resources.payment_buttonRound;
            this.btnPayment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPayment.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPayment.FlatAppearance.BorderSize = 0;
            this.btnPayment.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPayment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPayment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayment.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayment.ForeColor = System.Drawing.Color.White;
            this.btnPayment.Location = new System.Drawing.Point(108, 3);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(99, 49);
            this.btnPayment.TabIndex = 1;
            this.btnPayment.Text = "Payment";
            this.btnPayment.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPayment.UseVisualStyleBackColor = false;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            this.btnPayment.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseDown);
            this.btnPayment.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseUp);
            // 
            // buttonCancelTransaction
            // 
            this.buttonCancelTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancelTransaction.BackgroundImage = global::Parafait_POS.Properties.Resources.ClearTrx;
            this.buttonCancelTransaction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCancelTransaction.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonCancelTransaction.FlatAppearance.BorderSize = 0;
            this.buttonCancelTransaction.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonCancelTransaction.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonCancelTransaction.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonCancelTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancelTransaction.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancelTransaction.ForeColor = System.Drawing.Color.White;
            this.buttonCancelTransaction.Location = new System.Drawing.Point(213, 3);
            this.buttonCancelTransaction.Name = "buttonCancelTransaction";
            this.buttonCancelTransaction.Size = new System.Drawing.Size(99, 49);
            this.buttonCancelTransaction.TabIndex = 2;
            this.buttonCancelTransaction.Text = "Clear Trx";
            this.buttonCancelTransaction.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonCancelTransaction.UseVisualStyleBackColor = false;
            this.buttonCancelTransaction.Click += new System.EventHandler(this.buttonCancelTransaction_Click);
            this.buttonCancelTransaction.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseDown);
            this.buttonCancelTransaction.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseUp);
            // 
            // buttonCancelLine
            // 
            this.buttonCancelLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancelLine.AutoEllipsis = true;
            this.buttonCancelLine.BackgroundImage = global::Parafait_POS.Properties.Resources.CancelLine;
            this.buttonCancelLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCancelLine.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonCancelLine.FlatAppearance.BorderSize = 0;
            this.buttonCancelLine.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonCancelLine.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonCancelLine.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonCancelLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancelLine.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancelLine.ForeColor = System.Drawing.Color.White;
            this.buttonCancelLine.Location = new System.Drawing.Point(318, 3);
            this.buttonCancelLine.Name = "buttonCancelLine";
            this.buttonCancelLine.Size = new System.Drawing.Size(99, 49);
            this.buttonCancelLine.TabIndex = 4;
            this.buttonCancelLine.Text = "Cancel Line";
            this.buttonCancelLine.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonCancelLine.UseVisualStyleBackColor = false;
            this.buttonCancelLine.Click += new System.EventHandler(this.buttonCancelLine_Click);
            this.buttonCancelLine.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseDown);
            this.buttonCancelLine.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseUp);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.AutoEllipsis = true;
            this.btnPrint.BackgroundImage = global::Parafait_POS.Properties.Resources.PrintTrx;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(423, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(99, 49);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.btnPrint.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseDown);
            this.btnPrint.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseUp);
            // 
            // flpOrderButtons
            // 
            this.flpOrderButtons.Controls.Add(this.btnPlaceOrder);
            this.flpOrderButtons.Controls.Add(this.btnViewOrders);
            this.flpOrderButtons.Controls.Add(this.btnRedeemCoupon);
            this.flpOrderButtons.Controls.Add(this.btnVariableRefund);
            this.flpOrderButtons.Controls.Add(this.btnSendPaymentLink);
            this.flpOrderButtons.Location = new System.Drawing.Point(3, 59);
            this.flpOrderButtons.Name = "flpOrderButtons";
            this.flpOrderButtons.Size = new System.Drawing.Size(525, 53);
            this.flpOrderButtons.TabIndex = 7;
            // 
            // btnPlaceOrder
            // 
            this.btnPlaceOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPlaceOrder.AutoEllipsis = true;
            this.btnPlaceOrder.BackgroundImage = global::Parafait_POS.Properties.Resources.OrderSuspend;
            this.btnPlaceOrder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlaceOrder.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPlaceOrder.FlatAppearance.BorderSize = 0;
            this.btnPlaceOrder.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPlaceOrder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPlaceOrder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPlaceOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlaceOrder.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlaceOrder.ForeColor = System.Drawing.Color.White;
            this.btnPlaceOrder.Location = new System.Drawing.Point(3, 3);
            this.btnPlaceOrder.Name = "btnPlaceOrder";
            this.btnPlaceOrder.Size = new System.Drawing.Size(99, 49);
            this.btnPlaceOrder.TabIndex = 3;
            this.btnPlaceOrder.Text = "Order / Suspend";
            this.btnPlaceOrder.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPlaceOrder.UseVisualStyleBackColor = false;
            this.btnPlaceOrder.Click += new System.EventHandler(this.btnPlaceOrder_Click);
            this.btnPlaceOrder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseDown);
            this.btnPlaceOrder.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseUp);
            // 
            // btnViewOrders
            // 
            this.btnViewOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnViewOrders.AutoEllipsis = true;
            this.btnViewOrders.BackgroundImage = global::Parafait_POS.Properties.Resources.ViewOrderNormal;
            this.btnViewOrders.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnViewOrders.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnViewOrders.FlatAppearance.BorderSize = 0;
            this.btnViewOrders.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnViewOrders.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnViewOrders.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnViewOrders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewOrders.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewOrders.ForeColor = System.Drawing.Color.White;
            this.btnViewOrders.Location = new System.Drawing.Point(108, 3);
            this.btnViewOrders.Name = "btnViewOrders";
            this.btnViewOrders.Size = new System.Drawing.Size(99, 49);
            this.btnViewOrders.TabIndex = 6;
            this.btnViewOrders.Text = "View Orders";
            this.btnViewOrders.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnViewOrders.UseVisualStyleBackColor = false;
            this.btnViewOrders.Click += new System.EventHandler(this.btnViewOrders_Click);
            this.btnViewOrders.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseDown);
            this.btnViewOrders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trxButtonMouseUp);
            // 
            // btnRedeemCoupon
            // 
            this.btnRedeemCoupon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRedeemCoupon.AutoEllipsis = true;
            this.btnRedeemCoupon.BackgroundImage = global::Parafait_POS.Properties.Resources.ScanTicket;
            this.btnRedeemCoupon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRedeemCoupon.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnRedeemCoupon.FlatAppearance.BorderSize = 0;
            this.btnRedeemCoupon.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemCoupon.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemCoupon.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemCoupon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedeemCoupon.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRedeemCoupon.ForeColor = System.Drawing.Color.White;
            this.btnRedeemCoupon.Location = new System.Drawing.Point(213, 3);
            this.btnRedeemCoupon.Name = "btnRedeemCoupon";
            this.btnRedeemCoupon.Size = new System.Drawing.Size(99, 49);
            this.btnRedeemCoupon.TabIndex = 8;
            this.btnRedeemCoupon.Text = "Redeem Coupon";
            this.btnRedeemCoupon.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRedeemCoupon.UseVisualStyleBackColor = false;
            this.btnRedeemCoupon.Click += new System.EventHandler(this.btnRedeemCoupon_Click);
            this.btnRedeemCoupon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRedeemCoupon_MouseDown);
            this.btnRedeemCoupon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRedeemCoupon_MouseUp);
            // 
            // btnVariableRefund
            // 
            this.btnVariableRefund.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVariableRefund.BackColor = System.Drawing.Color.Transparent;
            this.btnVariableRefund.BackgroundImage = global::Parafait_POS.Properties.Resources.Variable_Refund_Btn_Normal;
            this.btnVariableRefund.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVariableRefund.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnVariableRefund.FlatAppearance.BorderSize = 0;
            this.btnVariableRefund.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnVariableRefund.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnVariableRefund.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVariableRefund.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.btnVariableRefund.ForeColor = System.Drawing.Color.White;
            this.btnVariableRefund.Location = new System.Drawing.Point(318, 3);
            this.btnVariableRefund.Name = "btnVariableRefund";
            this.btnVariableRefund.Size = new System.Drawing.Size(99, 49);
            this.btnVariableRefund.TabIndex = 24;
            this.btnVariableRefund.Text = "Item Refund";
            this.btnVariableRefund.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVariableRefund.UseVisualStyleBackColor = false;
            this.btnVariableRefund.Click += new System.EventHandler(this.btnVariableRefund_Click);
            this.btnVariableRefund.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnVariableRefund_MouseDown);
            this.btnVariableRefund.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnVariableRefund_MouseUp);
            // 
            // btnSendPaymentLink
            // 
            this.btnSendPaymentLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSendPaymentLink.AutoEllipsis = true;
            this.btnSendPaymentLink.BackgroundImage = global::Parafait_POS.Properties.Resources.SendPaymentLink;
            this.btnSendPaymentLink.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSendPaymentLink.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnSendPaymentLink.FlatAppearance.BorderSize = 0;
            this.btnSendPaymentLink.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSendPaymentLink.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSendPaymentLink.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSendPaymentLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendPaymentLink.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendPaymentLink.ForeColor = System.Drawing.Color.White;
            this.btnSendPaymentLink.Location = new System.Drawing.Point(423, 3);
            this.btnSendPaymentLink.Name = "btnSendPaymentLink";
            this.btnSendPaymentLink.Size = new System.Drawing.Size(99, 49);
            this.btnSendPaymentLink.TabIndex = 25;
            this.btnSendPaymentLink.Text = "Payment Link";
            this.btnSendPaymentLink.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSendPaymentLink.UseVisualStyleBackColor = false;
            this.btnSendPaymentLink.Click += new System.EventHandler(this.btnSendPaymentLink_Click);
            this.btnSendPaymentLink.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSendPaymentLink_MouseDown);
            this.btnSendPaymentLink.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSendPaymentLink_MouseUp);
            // 
            // dataGridViewTransaction
            // 
            this.dataGridViewTransaction.AllowUserToAddRows = false;
            this.dataGridViewTransaction.AllowUserToDeleteRows = false;
            this.dataGridViewTransaction.AllowUserToResizeColumns = false;
            this.dataGridViewTransaction.AllowUserToResizeRows = false;
            this.dataGridViewTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTransaction.BackgroundColor = System.Drawing.Color.Gray;
            this.dataGridViewTransaction.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Arial", 8.25F);
            dataGridViewCellStyle21.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTransaction.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle21;
            this.dataGridViewTransaction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewTransaction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Product_Type,
            this.Card_Number,
            this.Product_Name,
            this.Quantity,
            this.Price,
            this.Tax,
            this.TaxName,
            this.Line_Amount,
            this.LineId,
            this.Line_Type,
            this.Remarks,
            this.AttractionDetails,
            this.TrxProfileId});
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle29.Font = new System.Drawing.Font("Arial", 8.25F);
            dataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle29.SelectionBackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTransaction.DefaultCellStyle = dataGridViewCellStyle29;
            this.dataGridViewTransaction.EnableHeadersVisualStyles = false;
            this.dataGridViewTransaction.Location = new System.Drawing.Point(4, 3);
            this.dataGridViewTransaction.Name = "dataGridViewTransaction";
            this.dataGridViewTransaction.RowHeadersVisible = false;
            this.dataGridViewTransaction.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewTransaction.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewTransaction.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridViewTransaction.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSteelBlue;
            this.dataGridViewTransaction.RowTemplate.Height = 35;
            this.dataGridViewTransaction.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewTransaction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTransaction.Size = new System.Drawing.Size(535, 423);
            this.dataGridViewTransaction.TabIndex = 4;
            this.dataGridViewTransaction.TabStop = false;
            this.dataGridViewTransaction.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTransaction_CellClick);
            this.dataGridViewTransaction.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTransaction_CellDoubleClick);
            this.dataGridViewTransaction.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewTransaction_CellMouseClick);
            this.dataGridViewTransaction.SelectionChanged += new System.EventHandler(this.dataGridViewTransaction_SelectionChanged);
            // 
            // Product_Type
            // 
            this.Product_Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Product_Type.DefaultCellStyle = dataGridViewCellStyle22;
            this.Product_Type.HeaderText = "Type";
            this.Product_Type.Name = "Product_Type";
            this.Product_Type.ReadOnly = true;
            this.Product_Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Product_Type.Width = 35;
            // 
            // Card_Number
            // 
            this.Card_Number.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Card_Number.DefaultCellStyle = dataGridViewCellStyle23;
            this.Card_Number.HeaderText = "Card Number";
            this.Card_Number.Name = "Card_Number";
            this.Card_Number.ReadOnly = true;
            this.Card_Number.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Card_Number.Visible = false;
            // 
            // Product_Name
            // 
            this.Product_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Product_Name.DefaultCellStyle = dataGridViewCellStyle24;
            this.Product_Name.FillWeight = 500F;
            this.Product_Name.HeaderText = "Product";
            this.Product_Name.MinimumWidth = 30;
            this.Product_Name.Name = "Product_Name";
            this.Product_Name.ReadOnly = true;
            this.Product_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Quantity
            // 
            this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Quantity.DefaultCellStyle = dataGridViewCellStyle25;
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Quantity.Width = 52;
            // 
            // Price
            // 
            this.Price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle26.Format = "N2";
            dataGridViewCellStyle26.NullValue = null;
            this.Price.DefaultCellStyle = dataGridViewCellStyle26;
            this.Price.HeaderText = "Price";
            this.Price.MinimumWidth = 30;
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            this.Price.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Tax
            // 
            this.Tax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle27.Format = "N2";
            this.Tax.DefaultCellStyle = dataGridViewCellStyle27;
            this.Tax.HeaderText = "Tax";
            this.Tax.MinimumWidth = 30;
            this.Tax.Name = "Tax";
            this.Tax.ReadOnly = true;
            this.Tax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TaxName
            // 
            this.TaxName.HeaderText = "Tax Name";
            this.TaxName.Name = "TaxName";
            this.TaxName.Visible = false;
            // 
            // Line_Amount
            // 
            this.Line_Amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle28.Format = "N2";
            dataGridViewCellStyle28.NullValue = null;
            this.Line_Amount.DefaultCellStyle = dataGridViewCellStyle28;
            this.Line_Amount.HeaderText = "Amount";
            this.Line_Amount.MinimumWidth = 100;
            this.Line_Amount.Name = "Line_Amount";
            this.Line_Amount.ReadOnly = true;
            this.Line_Amount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LineId
            // 
            this.LineId.HeaderText = "LineId";
            this.LineId.Name = "LineId";
            this.LineId.Visible = false;
            // 
            // Line_Type
            // 
            this.Line_Type.HeaderText = "Line_Type";
            this.Line_Type.Name = "Line_Type";
            this.Line_Type.Visible = false;
            // 
            // Remarks
            // 
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            this.Remarks.Visible = false;
            // 
            // AttractionDetails
            // 
            this.AttractionDetails.HeaderText = "AttractionDetails";
            this.AttractionDetails.Name = "AttractionDetails";
            this.AttractionDetails.Visible = false;
            // 
            // TrxProfileId
            // 
            this.TrxProfileId.HeaderText = "TrxProfileId";
            this.TrxProfileId.Name = "TrxProfileId";
            this.TrxProfileId.Visible = false;
            // 
            // tabPageActivities
            // 
            this.tabPageActivities.AutoScroll = true;
            this.tabPageActivities.BackColor = System.Drawing.Color.Gray;
            this.tabPageActivities.Controls.Add(this.btnCardSearch);
            this.tabPageActivities.Controls.Add(this.lblCardNumber);
            this.tabPageActivities.Controls.Add(this.TxtCardNumber);
            this.tabPageActivities.Controls.Add(this.lnkPrintCardActivity);
            this.tabPageActivities.Controls.Add(this.lnkShowHideExtended);
            this.tabPageActivities.Controls.Add(this.btnConsolidatedCardActivity);
            this.tabPageActivities.Controls.Add(this.lnkConsolidatedView);
            this.tabPageActivities.Controls.Add(this.lnkZoomCardActivity);
            this.tabPageActivities.Controls.Add(this.dataGridViewGamePlay);
            this.tabPageActivities.Controls.Add(this.labelGamePlay);
            this.tabPageActivities.Controls.Add(this.labelPurchases);
            this.tabPageActivities.Controls.Add(this.dataGridViewPurchases);
            this.tabPageActivities.Location = new System.Drawing.Point(4, 44);
            this.tabPageActivities.Name = "tabPageActivities";
            this.tabPageActivities.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageActivities.Size = new System.Drawing.Size(542, 631);
            this.tabPageActivities.TabIndex = 1;
            this.tabPageActivities.Text = "Activities";
            // 
            // btnCardSearch
            // 
            this.btnCardSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnCardSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.box3;
            this.btnCardSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCardSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCardSearch.ForeColor = System.Drawing.Color.White;
            this.btnCardSearch.Location = new System.Drawing.Point(466, 3);
            this.btnCardSearch.Name = "btnCardSearch";
            this.btnCardSearch.Size = new System.Drawing.Size(69, 23);
            this.btnCardSearch.TabIndex = 16;
            this.btnCardSearch.Text = "GO";
            this.btnCardSearch.UseVisualStyleBackColor = false;
            this.btnCardSearch.Click += new System.EventHandler(this.btnCardSearch_Click);
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.AutoSize = true;
            this.lblCardNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblCardNumber.ForeColor = System.Drawing.Color.Snow;
            this.lblCardNumber.Location = new System.Drawing.Point(168, 4);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(96, 16);
            this.lblCardNumber.TabIndex = 15;
            this.lblCardNumber.Text = "Card Number:";
            // 
            // TxtCardNumber
            // 
            this.TxtCardNumber.BackColor = System.Drawing.Color.AliceBlue;
            this.TxtCardNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtCardNumber.Font = new System.Drawing.Font("Arial", 14.25F);
            this.TxtCardNumber.ForeColor = System.Drawing.Color.Black;
            this.TxtCardNumber.Location = new System.Drawing.Point(269, 3);
            this.TxtCardNumber.Name = "TxtCardNumber";
            this.TxtCardNumber.Size = new System.Drawing.Size(192, 22);
            this.TxtCardNumber.TabIndex = 14;
            // 
            // lnkPrintCardActivity
            // 
            this.lnkPrintCardActivity.AutoSize = true;
            this.lnkPrintCardActivity.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lnkPrintCardActivity.LinkColor = System.Drawing.Color.DarkBlue;
            this.lnkPrintCardActivity.Location = new System.Drawing.Point(425, 340);
            this.lnkPrintCardActivity.Name = "lnkPrintCardActivity";
            this.lnkPrintCardActivity.Size = new System.Drawing.Size(34, 15);
            this.lnkPrintCardActivity.TabIndex = 12;
            this.lnkPrintCardActivity.TabStop = true;
            this.lnkPrintCardActivity.Text = "Print";
            this.lnkPrintCardActivity.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPrintCardActivity_LinkClicked);
            // 
            // lnkShowHideExtended
            // 
            this.lnkShowHideExtended.AutoSize = true;
            this.lnkShowHideExtended.LinkColor = System.Drawing.Color.DarkBlue;
            this.lnkShowHideExtended.Location = new System.Drawing.Point(92, 340);
            this.lnkShowHideExtended.Name = "lnkShowHideExtended";
            this.lnkShowHideExtended.Size = new System.Drawing.Size(95, 15);
            this.lnkShowHideExtended.TabIndex = 11;
            this.lnkShowHideExtended.TabStop = true;
            this.lnkShowHideExtended.Tag = "0";
            this.lnkShowHideExtended.Text = "Show Extended";
            this.lnkShowHideExtended.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkShowHideExtended_LinkClicked);
            // 
            // btnConsolidatedCardActivity
            // 
            this.btnConsolidatedCardActivity.BackColor = System.Drawing.Color.Transparent;
            this.btnConsolidatedCardActivity.BackgroundImage = global::Parafait_POS.Properties.Resources.Globe_Icon;
            this.btnConsolidatedCardActivity.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnConsolidatedCardActivity.FlatAppearance.BorderColor = System.Drawing.Color.PaleTurquoise;
            this.btnConsolidatedCardActivity.FlatAppearance.BorderSize = 0;
            this.btnConsolidatedCardActivity.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnConsolidatedCardActivity.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConsolidatedCardActivity.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConsolidatedCardActivity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsolidatedCardActivity.Location = new System.Drawing.Point(206, 331);
            this.btnConsolidatedCardActivity.Name = "btnConsolidatedCardActivity";
            this.btnConsolidatedCardActivity.Size = new System.Drawing.Size(29, 26);
            this.btnConsolidatedCardActivity.TabIndex = 10;
            this.btnConsolidatedCardActivity.UseVisualStyleBackColor = false;
            this.btnConsolidatedCardActivity.Click += new System.EventHandler(this.btnConsolidatedCardActivity_Click);
            // 
            // lnkConsolidatedView
            // 
            this.lnkConsolidatedView.AutoSize = true;
            this.lnkConsolidatedView.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lnkConsolidatedView.LinkColor = System.Drawing.Color.DarkBlue;
            this.lnkConsolidatedView.Location = new System.Drawing.Point(236, 340);
            this.lnkConsolidatedView.Name = "lnkConsolidatedView";
            this.lnkConsolidatedView.Size = new System.Drawing.Size(112, 15);
            this.lnkConsolidatedView.TabIndex = 7;
            this.lnkConsolidatedView.TabStop = true;
            this.lnkConsolidatedView.Text = "Consolidated View";
            this.lnkConsolidatedView.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkConsolidatedView_LinkClicked);
            // 
            // lnkZoomCardActivity
            // 
            this.lnkZoomCardActivity.AutoSize = true;
            this.lnkZoomCardActivity.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lnkZoomCardActivity.LinkColor = System.Drawing.Color.DarkBlue;
            this.lnkZoomCardActivity.Location = new System.Drawing.Point(369, 340);
            this.lnkZoomCardActivity.Name = "lnkZoomCardActivity";
            this.lnkZoomCardActivity.Size = new System.Drawing.Size(39, 15);
            this.lnkZoomCardActivity.TabIndex = 5;
            this.lnkZoomCardActivity.TabStop = true;
            this.lnkZoomCardActivity.Text = "Zoom";
            this.lnkZoomCardActivity.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkZoomCardActivity_LinkClicked);
            // 
            // dataGridViewGamePlay
            // 
            this.dataGridViewGamePlay.AllowUserToAddRows = false;
            this.dataGridViewGamePlay.AllowUserToDeleteRows = false;
            this.dataGridViewGamePlay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewGamePlay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewGamePlay.BackgroundColor = System.Drawing.Color.Gray;
            this.dataGridViewGamePlay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGamePlay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReverseGamePlay});
            this.dataGridViewGamePlay.Location = new System.Drawing.Point(4, 358);
            this.dataGridViewGamePlay.Name = "dataGridViewGamePlay";
            this.dataGridViewGamePlay.ReadOnly = true;
            this.dataGridViewGamePlay.RowHeadersVisible = false;
            this.dataGridViewGamePlay.RowTemplate.ReadOnly = true;
            this.dataGridViewGamePlay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGamePlay.Size = new System.Drawing.Size(535, 270);
            this.dataGridViewGamePlay.TabIndex = 3;
            this.dataGridViewGamePlay.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewGamePlay_CellClick);
            this.dataGridViewGamePlay.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridGamePlay_Scroll);
            this.dataGridViewGamePlay.VisibleChanged += DataGridViewGamePlay_VisibleChanged;
            // 
            // ReverseGamePlay
            // 
            this.ReverseGamePlay.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ReverseGamePlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReverseGamePlay.HeaderText = "X";
            this.ReverseGamePlay.Name = "ReverseGamePlay";
            this.ReverseGamePlay.ReadOnly = true;
            this.ReverseGamePlay.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ReverseGamePlay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ReverseGamePlay.Text = "X";
            this.ReverseGamePlay.UseColumnTextForButtonValue = true;
            this.ReverseGamePlay.Width = 20;
            // 
            // labelGamePlay
            // 
            this.labelGamePlay.AutoSize = true;
            this.labelGamePlay.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelGamePlay.ForeColor = System.Drawing.Color.White;
            this.labelGamePlay.Location = new System.Drawing.Point(6, 339);
            this.labelGamePlay.Name = "labelGamePlay";
            this.labelGamePlay.Size = new System.Drawing.Size(84, 16);
            this.labelGamePlay.TabIndex = 2;
            this.labelGamePlay.Text = "Game Plays";
            // 
            // labelPurchases
            // 
            this.labelPurchases.AutoSize = true;
            this.labelPurchases.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelPurchases.ForeColor = System.Drawing.Color.White;
            this.labelPurchases.Location = new System.Drawing.Point(6, 3);
            this.labelPurchases.Name = "labelPurchases";
            this.labelPurchases.Size = new System.Drawing.Size(139, 16);
            this.labelPurchases.TabIndex = 1;
            this.labelPurchases.Text = "Purchases and Tasks";
            // 
            // dataGridViewPurchases
            // 
            this.dataGridViewPurchases.AllowUserToAddRows = false;
            this.dataGridViewPurchases.AllowUserToDeleteRows = false;
            dataGridViewCellStyle30.BackColor = System.Drawing.Color.Gainsboro;
            this.dataGridViewPurchases.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle30;
            this.dataGridViewPurchases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPurchases.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewPurchases.BackgroundColor = System.Drawing.Color.Gray;
            this.dataGridViewPurchases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPurchases.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcBtnCardActivityTrxPrint});
            this.dataGridViewPurchases.Location = new System.Drawing.Point(4, 32);
            this.dataGridViewPurchases.Name = "dataGridViewPurchases";
            this.dataGridViewPurchases.ReadOnly = true;
            this.dataGridViewPurchases.RowHeadersVisible = false;
            this.dataGridViewPurchases.RowTemplate.ReadOnly = true;
            this.dataGridViewPurchases.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPurchases.Size = new System.Drawing.Size(536, 297);
            this.dataGridViewPurchases.TabIndex = 0;
            this.dataGridViewPurchases.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPurchases_CellContentClick);
            this.dataGridViewPurchases.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridViewPurchases_Scroll);
            // 
            // dcBtnCardActivityTrxPrint
            // 
            this.dcBtnCardActivityTrxPrint.HeaderText = "P";
            this.dcBtnCardActivityTrxPrint.Image = global::Parafait_POS.Properties.Resources.printer;
            this.dcBtnCardActivityTrxPrint.Name = "dcBtnCardActivityTrxPrint";
            this.dcBtnCardActivityTrxPrint.ReadOnly = true;
            this.dcBtnCardActivityTrxPrint.Width = 21;
            // 
            // tabPageCardInfo
            // 
            this.tabPageCardInfo.AutoScroll = true;
            this.tabPageCardInfo.BackColor = System.Drawing.Color.Gray;
            this.tabPageCardInfo.Controls.Add(this.label33);
            this.tabPageCardInfo.Controls.Add(this.label32);
            this.tabPageCardInfo.Controls.Add(this.label31);
            this.tabPageCardInfo.Controls.Add(this.dgvCardGames);
            this.tabPageCardInfo.Controls.Add(this.dgvCardDiscounts);
            this.tabPageCardInfo.Controls.Add(this.dgvCreditPlus);
            this.tabPageCardInfo.Controls.Add(this.grpMisc);
            this.tabPageCardInfo.Location = new System.Drawing.Point(4, 44);
            this.tabPageCardInfo.Name = "tabPageCardInfo";
            this.tabPageCardInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCardInfo.Size = new System.Drawing.Size(542, 631);
            this.tabPageCardInfo.TabIndex = 3;
            this.tabPageCardInfo.Text = "Card Details";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.ForeColor = System.Drawing.Color.White;
            this.label33.Location = new System.Drawing.Point(338, 2);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(69, 15);
            this.label33.TabIndex = 8;
            this.label33.Text = "Credit Plus";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.ForeColor = System.Drawing.Color.White;
            this.label32.Location = new System.Drawing.Point(338, 133);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(64, 15);
            this.label32.TabIndex = 7;
            this.label32.Text = "Discounts";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.ForeColor = System.Drawing.Color.White;
            this.label31.Location = new System.Drawing.Point(3, 2);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(96, 15);
            this.label31.TabIndex = 6;
            this.label31.Text = "Games On Card";
            // 
            // dgvCardGames
            // 
            this.dgvCardGames.AllowUserToAddRows = false;
            this.dgvCardGames.AllowUserToDeleteRows = false;
            this.dgvCardGames.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCardGames.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvCardGames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCardGames.Location = new System.Drawing.Point(4, 17);
            this.dgvCardGames.Name = "dgvCardGames";
            this.dgvCardGames.ReadOnly = true;
            dataGridViewCellStyle31.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle31.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCardGames.RowsDefaultCellStyle = dataGridViewCellStyle31;
            this.dgvCardGames.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvCardGames.Size = new System.Drawing.Size(330, 210);
            this.dgvCardGames.TabIndex = 0;
            // 
            // dgvCardDiscounts
            // 
            this.dgvCardDiscounts.AllowUserToAddRows = false;
            this.dgvCardDiscounts.AllowUserToDeleteRows = false;
            this.dgvCardDiscounts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCardDiscounts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCardDiscounts.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvCardDiscounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCardDiscounts.Location = new System.Drawing.Point(340, 148);
            this.dgvCardDiscounts.Name = "dgvCardDiscounts";
            this.dgvCardDiscounts.ReadOnly = true;
            dataGridViewCellStyle32.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle32.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCardDiscounts.RowsDefaultCellStyle = dataGridViewCellStyle32;
            this.dgvCardDiscounts.Size = new System.Drawing.Size(200, 79);
            this.dgvCardDiscounts.TabIndex = 1;
            // 
            // dgvCreditPlus
            // 
            this.dgvCreditPlus.AllowUserToAddRows = false;
            this.dgvCreditPlus.AllowUserToDeleteRows = false;
            this.dgvCreditPlus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCreditPlus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCreditPlus.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvCreditPlus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCreditPlus.Location = new System.Drawing.Point(340, 17);
            this.dgvCreditPlus.Name = "dgvCreditPlus";
            this.dgvCreditPlus.ReadOnly = true;
            dataGridViewCellStyle33.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle33.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCreditPlus.RowsDefaultCellStyle = dataGridViewCellStyle33;
            this.dgvCreditPlus.Size = new System.Drawing.Size(200, 115);
            this.dgvCreditPlus.TabIndex = 1;
            // 
            // grpMisc
            // 
            this.grpMisc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMisc.Controls.Add(this.btnRefreshGraph);
            this.grpMisc.Controls.Add(this.rdAmount);
            this.grpMisc.Controls.Add(this.lblTicketMode);
            this.grpMisc.Controls.Add(this.rdCount);
            this.grpMisc.Controls.Add(this.lblTicketAllowed);
            this.grpMisc.Controls.Add(this.label14);
            this.grpMisc.Controls.Add(this.dtpGraphFrom);
            this.grpMisc.Controls.Add(this.lblRoamingCard);
            this.grpMisc.Controls.Add(this.label25);
            this.grpMisc.Controls.Add(this.zedGraphCardInfo);
            this.grpMisc.Controls.Add(this.dtpGraphTo);
            this.grpMisc.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMisc.ForeColor = System.Drawing.Color.White;
            this.grpMisc.Location = new System.Drawing.Point(4, 227);
            this.grpMisc.Name = "grpMisc";
            this.grpMisc.Size = new System.Drawing.Size(537, 389);
            this.grpMisc.TabIndex = 5;
            this.grpMisc.TabStop = false;
            this.grpMisc.Text = "Game Play Chart";
            // 
            // btnRefreshGraph
            // 
            this.btnRefreshGraph.BackColor = System.Drawing.Color.Transparent;
            this.btnRefreshGraph.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnRefreshGraph.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefreshGraph.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnRefreshGraph.FlatAppearance.BorderSize = 0;
            this.btnRefreshGraph.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefreshGraph.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefreshGraph.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshGraph.Font = new System.Drawing.Font("Arial", 8F);
            this.btnRefreshGraph.ForeColor = System.Drawing.Color.White;
            this.btnRefreshGraph.Location = new System.Drawing.Point(470, 63);
            this.btnRefreshGraph.Name = "btnRefreshGraph";
            this.btnRefreshGraph.Size = new System.Drawing.Size(60, 23);
            this.btnRefreshGraph.TabIndex = 23;
            this.btnRefreshGraph.Text = "Refresh";
            this.btnRefreshGraph.UseVisualStyleBackColor = false;
            this.btnRefreshGraph.Click += new System.EventHandler(this.btnRefreshGraph_Click);
            // 
            // rdAmount
            // 
            this.rdAmount.AutoSize = true;
            this.rdAmount.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdAmount.Location = new System.Drawing.Point(409, 64);
            this.rdAmount.Name = "rdAmount";
            this.rdAmount.Size = new System.Drawing.Size(62, 18);
            this.rdAmount.TabIndex = 22;
            this.rdAmount.TabStop = true;
            this.rdAmount.Text = "Amount";
            this.rdAmount.UseVisualStyleBackColor = true;
            // 
            // lblTicketMode
            // 
            this.lblTicketMode.AutoSize = true;
            this.lblTicketMode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketMode.ForeColor = System.Drawing.Color.Orange;
            this.lblTicketMode.Location = new System.Drawing.Point(96, 20);
            this.lblTicketMode.Name = "lblTicketMode";
            this.lblTicketMode.Size = new System.Drawing.Size(70, 15);
            this.lblTicketMode.TabIndex = 2;
            this.lblTicketMode.Text = "ticket mode";
            // 
            // rdCount
            // 
            this.rdCount.AutoSize = true;
            this.rdCount.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdCount.Location = new System.Drawing.Point(347, 64);
            this.rdCount.Name = "rdCount";
            this.rdCount.Size = new System.Drawing.Size(53, 18);
            this.rdCount.TabIndex = 21;
            this.rdCount.TabStop = true;
            this.rdCount.Text = "Count";
            this.rdCount.UseVisualStyleBackColor = true;
            // 
            // lblTicketAllowed
            // 
            this.lblTicketAllowed.AutoSize = true;
            this.lblTicketAllowed.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketAllowed.ForeColor = System.Drawing.Color.Orange;
            this.lblTicketAllowed.Location = new System.Drawing.Point(7, 20);
            this.lblTicketAllowed.Name = "lblTicketAllowed";
            this.lblTicketAllowed.Size = new System.Drawing.Size(81, 15);
            this.lblTicketAllowed.TabIndex = 1;
            this.lblTicketAllowed.Text = "ticket allowed";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(181, 67);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(21, 14);
            this.label14.TabIndex = 20;
            this.label14.Text = "To:";
            // 
            // dtpGraphFrom
            // 
            this.dtpGraphFrom.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpGraphFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpGraphFrom.Location = new System.Drawing.Point(41, 64);
            this.dtpGraphFrom.Name = "dtpGraphFrom";
            this.dtpGraphFrom.Size = new System.Drawing.Size(135, 20);
            this.dtpGraphFrom.TabIndex = 17;
            // 
            // lblRoamingCard
            // 
            this.lblRoamingCard.AutoSize = true;
            this.lblRoamingCard.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRoamingCard.ForeColor = System.Drawing.Color.Orange;
            this.lblRoamingCard.Location = new System.Drawing.Point(7, 41);
            this.lblRoamingCard.Name = "lblRoamingCard";
            this.lblRoamingCard.Size = new System.Drawing.Size(80, 15);
            this.lblRoamingCard.TabIndex = 0;
            this.lblRoamingCard.Text = "roaming card";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(2, 67);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(34, 14);
            this.label25.TabIndex = 19;
            this.label25.Text = "From:";
            // 
            // zedGraphCardInfo
            // 
            this.zedGraphCardInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraphCardInfo.Location = new System.Drawing.Point(5, 96);
            this.zedGraphCardInfo.Name = "zedGraphCardInfo";
            this.zedGraphCardInfo.ScrollGrace = 0D;
            this.zedGraphCardInfo.ScrollMaxX = 0D;
            this.zedGraphCardInfo.ScrollMaxY = 0D;
            this.zedGraphCardInfo.ScrollMaxY2 = 0D;
            this.zedGraphCardInfo.ScrollMinX = 0D;
            this.zedGraphCardInfo.ScrollMinY = 0D;
            this.zedGraphCardInfo.ScrollMinY2 = 0D;
            this.zedGraphCardInfo.Size = new System.Drawing.Size(526, 287);
            this.zedGraphCardInfo.TabIndex = 0;
            // 
            // dtpGraphTo
            // 
            this.dtpGraphTo.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpGraphTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpGraphTo.Location = new System.Drawing.Point(207, 64);
            this.dtpGraphTo.Name = "dtpGraphTo";
            this.dtpGraphTo.Size = new System.Drawing.Size(135, 20);
            this.dtpGraphTo.TabIndex = 18;
            // 
            // tabPageMyTrx
            // 
            this.tabPageMyTrx.Location = new System.Drawing.Point(4, 44);
            this.tabPageMyTrx.Name = "tabPageMyTrx";
            this.tabPageMyTrx.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMyTrx.Size = new System.Drawing.Size(542, 631);
            this.tabPageMyTrx.TabIndex = 4;
            this.tabPageMyTrx.Text = "My Transactions";
            this.tabPageMyTrx.UseVisualStyleBackColor = true;
            // 
            // tpBookings
            // 
            this.tpBookings.Controls.Add(this.tcAttractionBookings);
            this.tpBookings.Location = new System.Drawing.Point(4, 44);
            this.tpBookings.Name = "tpBookings";
            this.tpBookings.Padding = new System.Windows.Forms.Padding(3);
            this.tpBookings.Size = new System.Drawing.Size(542, 631);
            this.tpBookings.TabIndex = 5;
            this.tpBookings.Text = "Bookings";
            this.tpBookings.UseVisualStyleBackColor = true;
            // 
            // tcAttractionBookings
            // 
            this.tcAttractionBookings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcAttractionBookings.Controls.Add(this.tpAttractionBooking);
            this.tcAttractionBookings.Controls.Add(this.tpAttractionScedules);
            this.tcAttractionBookings.Controls.Add(this.tpReservations);
            this.tcAttractionBookings.Location = new System.Drawing.Point(0, 0);
            this.tcAttractionBookings.Name = "tcAttractionBookings";
            this.tcAttractionBookings.SelectedIndex = 0;
            this.tcAttractionBookings.Size = new System.Drawing.Size(543, 631);
            this.tcAttractionBookings.TabIndex = 1;
            this.tcAttractionBookings.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabBookingControl_Selected);
            // 
            // tpAttractionBooking
            // 
            this.tpAttractionBooking.Controls.Add(this.dgvBookings);
            this.tpAttractionBooking.Controls.Add(this.filterBookingPanel);
            this.tpAttractionBooking.Controls.Add(this.verticalScrollBarViewAB);
            this.tpAttractionBooking.Controls.Add(this.horizontalScrollBarViewAB);
            this.tpAttractionBooking.Location = new System.Drawing.Point(4, 24);
            this.tpAttractionBooking.Name = "tpAttractionBooking";
            this.tpAttractionBooking.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tpAttractionBooking.Size = new System.Drawing.Size(535, 603);
            this.tpAttractionBooking.TabIndex = 0;
            this.tpAttractionBooking.Text = "Attraction Bookings";
            this.tpAttractionBooking.UseVisualStyleBackColor = true;
            this.tpAttractionBooking.Enter += new System.EventHandler(this.tpAttractionBooking_OnEnter);
            // 
            // dgvBookings
            // 
            this.dgvBookings.AllowUserToAddRows = false;
            this.dgvBookings.AllowUserToDeleteRows = false;
            this.dgvBookings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBookings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBookings.Location = new System.Drawing.Point(3, 34);
            this.dgvBookings.MultiSelect = false;
            this.dgvBookings.Name = "dgvBookings";
            this.dgvBookings.ReadOnly = true;
            this.dgvBookings.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvBookings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBookings.Size = new System.Drawing.Size(489, 504);
            this.dgvBookings.TabIndex = 0;
            this.dgvBookings.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBookingsColumnHeaderClick);
            this.dgvBookings.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridViewBookings_Scroll);
            // 
            // filterBookingPanel
            // 
            this.filterBookingPanel.Controls.Add(this.rdBookingToday);
            this.filterBookingPanel.Controls.Add(this.rdBookingPast);
            this.filterBookingPanel.Controls.Add(this.rdBookingFuture3);
            this.filterBookingPanel.Controls.Add(this.rdBookingFutureAll);
            this.filterBookingPanel.Controls.Add(this.btnSearchBookingAttraction);
            this.filterBookingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterBookingPanel.Location = new System.Drawing.Point(3, 0);
            this.filterBookingPanel.Name = "filterBookingPanel";
            this.filterBookingPanel.Size = new System.Drawing.Size(529, 34);
            this.filterBookingPanel.TabIndex = 0;
            // 
            // rdBookingToday
            // 
            this.rdBookingToday.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdBookingToday.AutoEllipsis = true;
            this.rdBookingToday.Checked = true;
            this.rdBookingToday.Location = new System.Drawing.Point(3, 3);
            this.rdBookingToday.Name = "rdBookingToday";
            this.rdBookingToday.Size = new System.Drawing.Size(130, 30);
            this.rdBookingToday.TabIndex = 1;
            this.rdBookingToday.TabStop = true;
            this.rdBookingToday.Text = "Today";
            this.rdBookingToday.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdBookingToday.UseVisualStyleBackColor = true;
            this.rdBookingToday.Click += new System.EventHandler(this.rdBookingToday_Click);
            // 
            // rdBookingPast
            // 
            this.rdBookingPast.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdBookingPast.AutoEllipsis = true;
            this.rdBookingPast.Location = new System.Drawing.Point(139, 3);
            this.rdBookingPast.Name = "rdBookingPast";
            this.rdBookingPast.Size = new System.Drawing.Size(130, 30);
            this.rdBookingPast.TabIndex = 2;
            this.rdBookingPast.Text = "Past 3 Days";
            this.rdBookingPast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdBookingPast.UseVisualStyleBackColor = true;
            this.rdBookingPast.Click += new System.EventHandler(this.rdBookingPast_Click);
            // 
            // rdBookingFuture3
            // 
            this.rdBookingFuture3.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdBookingFuture3.AutoEllipsis = true;
            this.rdBookingFuture3.Location = new System.Drawing.Point(275, 3);
            this.rdBookingFuture3.Name = "rdBookingFuture3";
            this.rdBookingFuture3.Size = new System.Drawing.Size(130, 30);
            this.rdBookingFuture3.TabIndex = 3;
            this.rdBookingFuture3.Text = "Future 3 Days";
            this.rdBookingFuture3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdBookingFuture3.UseVisualStyleBackColor = true;
            this.rdBookingFuture3.Click += new System.EventHandler(this.rdBookingFuture3_Click);
            // 
            // rdBookingFutureAll
            // 
            this.rdBookingFutureAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdBookingFutureAll.AutoEllipsis = true;
            this.rdBookingFutureAll.Location = new System.Drawing.Point(3, 39);
            this.rdBookingFutureAll.Name = "rdBookingFutureAll";
            this.rdBookingFutureAll.Size = new System.Drawing.Size(130, 30);
            this.rdBookingFutureAll.TabIndex = 3;
            this.rdBookingFutureAll.Text = "All Future";
            this.rdBookingFutureAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdBookingFutureAll.UseVisualStyleBackColor = true;
            this.rdBookingFutureAll.Click += new System.EventHandler(this.rdBookingFutureAll_Click);
            // 
            // btnSearchBookingAttraction
            // 
            this.btnSearchBookingAttraction.BackgroundImage = global::Parafait_POS.Properties.Resources.Magnify_Icon_Hover;
            this.btnSearchBookingAttraction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearchBookingAttraction.FlatAppearance.BorderSize = 0;
            this.btnSearchBookingAttraction.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearchBookingAttraction.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearchBookingAttraction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchBookingAttraction.ForeColor = System.Drawing.Color.White;
            this.btnSearchBookingAttraction.Location = new System.Drawing.Point(139, 39);
            this.btnSearchBookingAttraction.Name = "btnSearchBookingAttraction";
            this.btnSearchBookingAttraction.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnSearchBookingAttraction.Size = new System.Drawing.Size(30, 30);
            this.btnSearchBookingAttraction.TabIndex = 4;
            this.btnSearchBookingAttraction.UseVisualStyleBackColor = true;
            this.btnSearchBookingAttraction.Click += new System.EventHandler(this.btnSearchBookingAttraction_Click);
            this.btnSearchBookingAttraction.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSearchBookingAttraction_MouseDown);
            this.btnSearchBookingAttraction.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSearchBookingAttraction_MouseUp);
            // 
            // verticalScrollBarViewAB
            // 
            this.verticalScrollBarViewAB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarViewAB.AutoHide = false;
            this.verticalScrollBarViewAB.DataGridView = this.dgvBookings;
            this.verticalScrollBarViewAB.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarViewAB.DownButtonBackgroundImage")));
            this.verticalScrollBarViewAB.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarViewAB.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarViewAB.Location = new System.Drawing.Point(491, 34);
            this.verticalScrollBarViewAB.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarViewAB.Name = "verticalScrollBarViewAB";
            this.verticalScrollBarViewAB.ScrollableControl = null;
            this.verticalScrollBarViewAB.ScrollViewer = null;
            this.verticalScrollBarViewAB.Size = new System.Drawing.Size(40, 501);
            this.verticalScrollBarViewAB.TabIndex = 1;
            this.verticalScrollBarViewAB.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarViewAB.UpButtonBackgroundImage")));
            this.verticalScrollBarViewAB.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarViewAB.UpButtonDisabledBackgroundImage")));
            // 
            // horizontalScrollBarViewAB
            // 
            this.horizontalScrollBarViewAB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBarViewAB.AutoHide = false;
            this.horizontalScrollBarViewAB.DataGridView = this.dgvBookings;
            this.horizontalScrollBarViewAB.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarViewAB.LeftButtonBackgroundImage")));
            this.horizontalScrollBarViewAB.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarViewAB.LeftButtonDisabledBackgroundImage")));
            this.horizontalScrollBarViewAB.Location = new System.Drawing.Point(3, 538);
            this.horizontalScrollBarViewAB.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarViewAB.Name = "horizontalScrollBarViewAB";
            this.horizontalScrollBarViewAB.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarViewAB.RightButtonBackgroundImage")));
            this.horizontalScrollBarViewAB.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarViewAB.RightButtonDisabledBackgroundImage")));
            this.horizontalScrollBarViewAB.ScrollableControl = null;
            this.horizontalScrollBarViewAB.ScrollViewer = null;
            this.horizontalScrollBarViewAB.Size = new System.Drawing.Size(489, 40);
            this.horizontalScrollBarViewAB.TabIndex = 2;
            // 
            // tpAttractionScedules
            // 
            this.tpAttractionScedules.Controls.Add(this.dtpAttractionDate);
            this.tpAttractionScedules.Controls.Add(this.label3);
            this.tpAttractionScedules.Controls.Add(this.cmbAttractionFacility);
            this.tpAttractionScedules.Controls.Add(this.chkShowPast);
            this.tpAttractionScedules.Controls.Add(this.btnRescheduleAttraction);
            this.tpAttractionScedules.Controls.Add(this.dgvAttractionSchedules);
            this.tpAttractionScedules.Controls.Add(this.verticalScrollBarViewAS);
            this.tpAttractionScedules.Controls.Add(this.horizontalScrollBarViewAS);
            this.tpAttractionScedules.Location = new System.Drawing.Point(4, 24);
            this.tpAttractionScedules.Name = "tpAttractionScedules";
            this.tpAttractionScedules.Padding = new System.Windows.Forms.Padding(3);
            this.tpAttractionScedules.Size = new System.Drawing.Size(535, 603);
            this.tpAttractionScedules.TabIndex = 1;
            this.tpAttractionScedules.Text = "Attraction Schedules";
            this.tpAttractionScedules.UseVisualStyleBackColor = true;
            // 
            // dtpAttractionDate
            // 
            this.dtpAttractionDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpAttractionDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpAttractionDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAttractionDate.Location = new System.Drawing.Point(400, 4);
            this.dtpAttractionDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
            this.dtpAttractionDate.Name = "dtpAttractionDate";
            this.dtpAttractionDate.Size = new System.Drawing.Size(142, 21);
            this.dtpAttractionDate.TabIndex = 1;
            this.dtpAttractionDate.ValueChanged += new System.EventHandler(this.dtpAttractionDate_ValueChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(158, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 18);
            this.label3.TabIndex = 25;
            this.label3.Text = "Attraction:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbAttractionFacility
            // 
            this.cmbAttractionFacility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttractionFacility.FormattingEnabled = true;
            this.cmbAttractionFacility.Location = new System.Drawing.Point(255, 2);
            this.cmbAttractionFacility.Name = "cmbAttractionFacility";
            this.cmbAttractionFacility.Size = new System.Drawing.Size(139, 23);
            this.cmbAttractionFacility.TabIndex = 24;
            // 
            // chkShowPast
            // 
            this.chkShowPast.AutoSize = true;
            this.chkShowPast.Location = new System.Drawing.Point(4, 4);
            this.chkShowPast.Name = "chkShowPast";
            this.chkShowPast.Size = new System.Drawing.Size(150, 19);
            this.chkShowPast.TabIndex = 23;
            this.chkShowPast.Text = "Show Past Schedules";
            this.chkShowPast.UseVisualStyleBackColor = true;
            this.chkShowPast.CheckedChanged += new System.EventHandler(this.chkShowPast_CheckedChanged);
            // 
            // btnRescheduleAttraction
            // 
            this.btnRescheduleAttraction.BackgroundImage = global::Parafait_POS.Properties.Resources.Reschedule_Icon_Normal;
            this.btnRescheduleAttraction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRescheduleAttraction.FlatAppearance.BorderSize = 0;
            this.btnRescheduleAttraction.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRescheduleAttraction.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRescheduleAttraction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRescheduleAttraction.ForeColor = System.Drawing.Color.White;
            this.btnRescheduleAttraction.Location = new System.Drawing.Point(555, 2);
            this.btnRescheduleAttraction.Name = "btnRescheduleAttraction";
            this.btnRescheduleAttraction.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnRescheduleAttraction.Size = new System.Drawing.Size(30, 30);
            this.btnRescheduleAttraction.TabIndex = 25;
            this.btnRescheduleAttraction.UseVisualStyleBackColor = true;
            this.btnRescheduleAttraction.Click += new System.EventHandler(this.btnRescheduleAttraction_Click);
            this.btnRescheduleAttraction.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRescheduleAttraction_MouseDown);
            this.btnRescheduleAttraction.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRescheduleAttraction_MouseUp);
            // 
            // dgvAttractionSchedules
            // 
            this.dgvAttractionSchedules.AllowUserToAddRows = false;
            this.dgvAttractionSchedules.AllowUserToDeleteRows = false;
            this.dgvAttractionSchedules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAttractionSchedules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttractionSchedules.Location = new System.Drawing.Point(3, 34);
            this.dgvAttractionSchedules.MultiSelect = false;
            this.dgvAttractionSchedules.Name = "dgvAttractionSchedules";
            this.dgvAttractionSchedules.ReadOnly = true;
            this.dgvAttractionSchedules.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvAttractionSchedules.Size = new System.Drawing.Size(489, 550);
            this.dgvAttractionSchedules.TabIndex = 0;
            this.dgvAttractionSchedules.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAttractionSchedules_CellMouseClick);
            // 
            // verticalScrollBarViewAS
            // 
            this.verticalScrollBarViewAS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarViewAS.AutoHide = false;
            this.verticalScrollBarViewAS.DataGridView = this.dgvAttractionSchedules;
            this.verticalScrollBarViewAS.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarViewAS.DownButtonBackgroundImage")));
            this.verticalScrollBarViewAS.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarViewAS.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarViewAS.Location = new System.Drawing.Point(491, 34);
            this.verticalScrollBarViewAS.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarViewAS.Name = "verticalScrollBarViewAS";
            this.verticalScrollBarViewAS.ScrollableControl = null;
            this.verticalScrollBarViewAS.ScrollViewer = null;
            this.verticalScrollBarViewAS.Size = new System.Drawing.Size(40, 547);
            this.verticalScrollBarViewAS.TabIndex = 1;
            this.verticalScrollBarViewAS.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarViewAS.UpButtonBackgroundImage")));
            this.verticalScrollBarViewAS.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarViewAS.UpButtonDisabledBackgroundImage")));
            // 
            // horizontalScrollBarViewAS
            // 
            this.horizontalScrollBarViewAS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBarViewAS.AutoHide = false;
            this.horizontalScrollBarViewAS.DataGridView = this.dgvAttractionSchedules;
            this.horizontalScrollBarViewAS.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarViewAS.LeftButtonBackgroundImage")));
            this.horizontalScrollBarViewAS.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarViewAS.LeftButtonDisabledBackgroundImage")));
            this.horizontalScrollBarViewAS.Location = new System.Drawing.Point(3, 584);
            this.horizontalScrollBarViewAS.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarViewAS.Name = "horizontalScrollBarViewAS";
            this.horizontalScrollBarViewAS.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarViewAS.RightButtonBackgroundImage")));
            this.horizontalScrollBarViewAS.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarViewAS.RightButtonDisabledBackgroundImage")));
            this.horizontalScrollBarViewAS.ScrollableControl = null;
            this.horizontalScrollBarViewAS.ScrollViewer = null;
            this.horizontalScrollBarViewAS.Size = new System.Drawing.Size(489, 40);
            this.horizontalScrollBarViewAS.TabIndex = 2;
            // 
            // tpReservations
            // 
            this.tpReservations.Controls.Add(this.lnkRefreshReservations);
            this.tpReservations.Controls.Add(this.lnkNewReservation);
            this.tpReservations.Controls.Add(this.dgvAllReservations);
            this.tpReservations.Controls.Add(this.horizontalScrollBarViewRB);
            this.tpReservations.Controls.Add(this.verticalScrollBarViewRB);
            this.tpReservations.Location = new System.Drawing.Point(4, 24);
            this.tpReservations.Name = "tpReservations";
            this.tpReservations.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tpReservations.Size = new System.Drawing.Size(535, 603);
            this.tpReservations.TabIndex = 3;
            this.tpReservations.Text = "Reservations";
            this.tpReservations.UseVisualStyleBackColor = true;
            // 
            // lnkRefreshReservations
            // 
            this.lnkRefreshReservations.AutoSize = true;
            this.lnkRefreshReservations.LinkColor = System.Drawing.Color.Maroon;
            this.lnkRefreshReservations.Location = new System.Drawing.Point(10, 10);
            this.lnkRefreshReservations.Name = "lnkRefreshReservations";
            this.lnkRefreshReservations.Size = new System.Drawing.Size(52, 15);
            this.lnkRefreshReservations.TabIndex = 4;
            this.lnkRefreshReservations.TabStop = true;
            this.lnkRefreshReservations.Text = "Refresh";
            this.lnkRefreshReservations.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRefreshReservations_LinkClicked);
            // 
            // lnkNewReservation
            // 
            this.lnkNewReservation.AutoSize = true;
            this.lnkNewReservation.LinkColor = System.Drawing.Color.Maroon;
            this.lnkNewReservation.Location = new System.Drawing.Point(68, 10);
            this.lnkNewReservation.Name = "lnkNewReservation";
            this.lnkNewReservation.Size = new System.Drawing.Size(32, 15);
            this.lnkNewReservation.TabIndex = 5;
            this.lnkNewReservation.TabStop = true;
            this.lnkNewReservation.Text = "New";
            this.lnkNewReservation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNewReservation_LinkClicked);
            // 
            // dgvAllReservations
            // 
            this.dgvAllReservations.AllowUserToAddRows = false;
            this.dgvAllReservations.AllowUserToDeleteRows = false;
            this.dgvAllReservations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAllReservations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllReservations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcSelectBooking});
            this.dgvAllReservations.Location = new System.Drawing.Point(3, 34);
            this.dgvAllReservations.Name = "dgvAllReservations";
            this.dgvAllReservations.ReadOnly = true;
            this.dgvAllReservations.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvAllReservations.Size = new System.Drawing.Size(489, 550);
            this.dgvAllReservations.TabIndex = 2;
            this.dgvAllReservations.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAllReservations_CellClick);
            this.dgvAllReservations.DoubleClick += new System.EventHandler(this.dgvAllReservations_DoubleClick);
            // 
            // dcSelectBooking
            // 
            this.dcSelectBooking.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcSelectBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dcSelectBooking.HeaderText = "...";
            this.dcSelectBooking.Name = "dcSelectBooking";
            this.dcSelectBooking.ReadOnly = true;
            this.dcSelectBooking.Text = "...";
            this.dcSelectBooking.ToolTipText = "Edit";
            this.dcSelectBooking.UseColumnTextForButtonValue = true;
            this.dcSelectBooking.Width = 22;
            // 
            // horizontalScrollBarViewRB
            // 
            this.horizontalScrollBarViewRB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBarViewRB.AutoHide = false;
            this.horizontalScrollBarViewRB.DataGridView = this.dgvAllReservations;
            this.horizontalScrollBarViewRB.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarViewRB.LeftButtonBackgroundImage")));
            this.horizontalScrollBarViewRB.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarViewRB.LeftButtonDisabledBackgroundImage")));
            this.horizontalScrollBarViewRB.Location = new System.Drawing.Point(3, 584);
            this.horizontalScrollBarViewRB.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarViewRB.Name = "horizontalScrollBarViewRB";
            this.horizontalScrollBarViewRB.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarViewRB.RightButtonBackgroundImage")));
            this.horizontalScrollBarViewRB.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarViewRB.RightButtonDisabledBackgroundImage")));
            this.horizontalScrollBarViewRB.ScrollableControl = null;
            this.horizontalScrollBarViewRB.ScrollViewer = null;
            this.horizontalScrollBarViewRB.Size = new System.Drawing.Size(489, 40);
            this.horizontalScrollBarViewRB.TabIndex = 2;
            // 
            // verticalScrollBarViewRB
            // 
            this.verticalScrollBarViewRB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarViewRB.AutoHide = false;
            this.verticalScrollBarViewRB.DataGridView = this.dgvAllReservations;
            this.verticalScrollBarViewRB.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarViewRB.DownButtonBackgroundImage")));
            this.verticalScrollBarViewRB.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarViewRB.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarViewRB.Location = new System.Drawing.Point(492, 34);
            this.verticalScrollBarViewRB.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarViewRB.Name = "verticalScrollBarViewRB";
            this.verticalScrollBarViewRB.ScrollableControl = null;
            this.verticalScrollBarViewRB.ScrollViewer = null;
            this.verticalScrollBarViewRB.Size = new System.Drawing.Size(40, 550);
            this.verticalScrollBarViewRB.TabIndex = 1;
            this.verticalScrollBarViewRB.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarViewRB.UpButtonBackgroundImage")));
            this.verticalScrollBarViewRB.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarViewRB.UpButtonDisabledBackgroundImage")));
            // 
            // tpOpenOrders
            // 
            this.tpOpenOrders.Controls.Add(this.panelOrders);
            this.tpOpenOrders.Location = new System.Drawing.Point(4, 44);
            this.tpOpenOrders.Name = "tpOpenOrders";
            this.tpOpenOrders.Padding = new System.Windows.Forms.Padding(3);
            this.tpOpenOrders.Size = new System.Drawing.Size(542, 631);
            this.tpOpenOrders.TabIndex = 6;
            this.tpOpenOrders.Text = "Open Orders";
            this.tpOpenOrders.UseVisualStyleBackColor = true;
            // 
            // panelOrders
            // 
            this.panelOrders.BackColor = System.Drawing.Color.SkyBlue;
            this.panelOrders.Controls.Add(this.btnCloseOrderPanel);
            this.panelOrders.Controls.Add(this.tcOrderView);
            this.panelOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOrders.Location = new System.Drawing.Point(3, 3);
            this.panelOrders.Name = "panelOrders";
            this.panelOrders.Size = new System.Drawing.Size(536, 625);
            this.panelOrders.TabIndex = 18;
            // 
            // btnCloseOrderPanel
            // 
            this.btnCloseOrderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCloseOrderPanel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCloseOrderPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCloseOrderPanel.FlatAppearance.BorderSize = 0;
            this.btnCloseOrderPanel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCloseOrderPanel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCloseOrderPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseOrderPanel.ForeColor = System.Drawing.Color.White;
            this.btnCloseOrderPanel.Location = new System.Drawing.Point(7, 579);
            this.btnCloseOrderPanel.Name = "btnCloseOrderPanel";
            this.btnCloseOrderPanel.Size = new System.Drawing.Size(132, 42);
            this.btnCloseOrderPanel.TabIndex = 4;
            this.btnCloseOrderPanel.Text = "Close";
            this.btnCloseOrderPanel.UseVisualStyleBackColor = true;
            this.btnCloseOrderPanel.Click += new System.EventHandler(this.btnCloseOrderPanel_Click);
            // 
            // tcOrderView
            // 
            this.tcOrderView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcOrderView.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tcOrderView.Controls.Add(this.tpOrderTableView);
            this.tcOrderView.Controls.Add(this.tpOrderOrderView);
            this.tcOrderView.Controls.Add(this.tpOrderBookingView);
            this.tcOrderView.Controls.Add(this.tpOrderPendingTrxView);
            this.tcOrderView.ItemSize = new System.Drawing.Size(75, 22);
            this.tcOrderView.Location = new System.Drawing.Point(3, 4);
            this.tcOrderView.Multiline = true;
            this.tcOrderView.Name = "tcOrderView";
            this.tcOrderView.SelectedIndex = 0;
            this.tcOrderView.Size = new System.Drawing.Size(530, 570);
            this.tcOrderView.TabIndex = 3;
            // 
            // tpOrderTableView
            // 
            this.tpOrderTableView.BackColor = System.Drawing.Color.Transparent;
            this.tpOrderTableView.Controls.Add(this.panelTables);
            this.tpOrderTableView.Controls.Add(this.flpFacilities);
            this.tpOrderTableView.Location = new System.Drawing.Point(4, 26);
            this.tpOrderTableView.Name = "tpOrderTableView";
            this.tpOrderTableView.Padding = new System.Windows.Forms.Padding(3);
            this.tpOrderTableView.Size = new System.Drawing.Size(522, 540);
            this.tpOrderTableView.TabIndex = 1;
            this.tpOrderTableView.Text = "Table View";
            // 
            // panelTables
            // 
            this.panelTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTables.AutoScroll = true;
            this.panelTables.Controls.Add(this.tableLayoutVerticalScrollBarView);
            this.panelTables.Controls.Add(this.tableLayoutHorizontalScrollBarView);
            this.panelTables.Controls.Add(this.tblPanelTables);
            this.panelTables.Location = new System.Drawing.Point(3, 22);
            this.panelTables.Name = "panelTables";
            this.panelTables.Size = new System.Drawing.Size(515, 515);
            this.panelTables.TabIndex = 5;
            // 
            // tableLayoutVerticalScrollBarView
            // 
            this.tableLayoutVerticalScrollBarView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutVerticalScrollBarView.AutoHide = false;
            this.tableLayoutVerticalScrollBarView.DataGridView = null;
            this.tableLayoutVerticalScrollBarView.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutVerticalScrollBarView.DownButtonBackgroundImage")));
            this.tableLayoutVerticalScrollBarView.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutVerticalScrollBarView.DownButtonDisabledBackgroundImage")));
            this.tableLayoutVerticalScrollBarView.Location = new System.Drawing.Point(472, 4);
            this.tableLayoutVerticalScrollBarView.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutVerticalScrollBarView.Name = "tableLayoutVerticalScrollBarView";
            this.tableLayoutVerticalScrollBarView.ScrollableControl = null;
            this.tableLayoutVerticalScrollBarView.ScrollViewer = null;
            this.tableLayoutVerticalScrollBarView.Size = new System.Drawing.Size(40, 460);
            this.tableLayoutVerticalScrollBarView.TabIndex = 2;
            this.tableLayoutVerticalScrollBarView.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutVerticalScrollBarView.UpButtonBackgroundImage")));
            this.tableLayoutVerticalScrollBarView.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutVerticalScrollBarView.UpButtonDisabledBackgroundImage")));
            // 
            // tableLayoutHorizontalScrollBarView
            // 
            this.tableLayoutHorizontalScrollBarView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutHorizontalScrollBarView.AutoHide = false;
            this.tableLayoutHorizontalScrollBarView.DataGridView = null;
            this.tableLayoutHorizontalScrollBarView.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutHorizontalScrollBarView.LeftButtonBackgroundImage")));
            this.tableLayoutHorizontalScrollBarView.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutHorizontalScrollBarView.LeftButtonDisabledBackgroundImage")));
            this.tableLayoutHorizontalScrollBarView.Location = new System.Drawing.Point(4, 466);
            this.tableLayoutHorizontalScrollBarView.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutHorizontalScrollBarView.Name = "tableLayoutHorizontalScrollBarView";
            this.tableLayoutHorizontalScrollBarView.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutHorizontalScrollBarView.RightButtonBackgroundImage")));
            this.tableLayoutHorizontalScrollBarView.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutHorizontalScrollBarView.RightButtonDisabledBackgroundImage")));
            this.tableLayoutHorizontalScrollBarView.ScrollableControl = null;
            this.tableLayoutHorizontalScrollBarView.ScrollViewer = null;
            this.tableLayoutHorizontalScrollBarView.Size = new System.Drawing.Size(467, 40);
            this.tableLayoutHorizontalScrollBarView.TabIndex = 1;
            // 
            // tblPanelTables
            // 
            this.tblPanelTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblPanelTables.Location = new System.Drawing.Point(3, 4);
            this.tblPanelTables.Name = "tblPanelTables";
            this.tblPanelTables.Size = new System.Drawing.Size(468, 460);
            this.tblPanelTables.TabIndex = 0;
            this.tblPanelTables.Text = "elementHost1";
            this.tblPanelTables.Child = null;
            // 
            // flpFacilities
            // 
            this.flpFacilities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpFacilities.Location = new System.Drawing.Point(6, 0);
            this.flpFacilities.Margin = new System.Windows.Forms.Padding(2, 0, 4, 1);
            this.flpFacilities.Name = "flpFacilities";
            this.flpFacilities.Size = new System.Drawing.Size(509, 22);
            this.flpFacilities.TabIndex = 3;
            // 
            // tpOrderOrderView
            // 
            this.tpOrderOrderView.BackColor = System.Drawing.Color.Transparent;
            this.tpOrderOrderView.Controls.Add(this.orderListView);
            this.tpOrderOrderView.Location = new System.Drawing.Point(4, 26);
            this.tpOrderOrderView.Name = "tpOrderOrderView";
            this.tpOrderOrderView.Padding = new System.Windows.Forms.Padding(3);
            this.tpOrderOrderView.Size = new System.Drawing.Size(522, 540);
            this.tpOrderOrderView.TabIndex = 0;
            this.tpOrderOrderView.Text = "Order View";
            // 
            // orderListView
            // 
            this.orderListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderListView.Location = new System.Drawing.Point(3, 3);
            this.orderListView.Name = "orderListView";
            this.orderListView.Size = new System.Drawing.Size(516, 534);
            this.orderListView.TabIndex = 0;
            // 
            // tpOrderBookingView
            // 
            this.tpOrderBookingView.Controls.Add(this.dgvBookingDetails);
            this.tpOrderBookingView.Location = new System.Drawing.Point(4, 26);
            this.tpOrderBookingView.Name = "tpOrderBookingView";
            this.tpOrderBookingView.Size = new System.Drawing.Size(522, 540);
            this.tpOrderBookingView.TabIndex = 2;
            this.tpOrderBookingView.Text = "Booking View";
            this.tpOrderBookingView.UseVisualStyleBackColor = true;
            // 
            // dgvBookingDetails
            // 
            this.dgvBookingDetails.AllowUserToAddRows = false;
            this.dgvBookingDetails.AllowUserToDeleteRows = false;
            this.dgvBookingDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvBookingDetails.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvBookingDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvBookingDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle34.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle34.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle34.ForeColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle34.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle34.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle34.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBookingDetails.DefaultCellStyle = dataGridViewCellStyle34;
            this.dgvBookingDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBookingDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvBookingDetails.EnableHeadersVisualStyles = false;
            this.dgvBookingDetails.Location = new System.Drawing.Point(0, 0);
            this.dgvBookingDetails.Name = "dgvBookingDetails";
            this.dgvBookingDetails.ReadOnly = true;
            this.dgvBookingDetails.RowHeadersVisible = false;
            this.dgvBookingDetails.RowTemplate.Height = 35;
            this.dgvBookingDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBookingDetails.Size = new System.Drawing.Size(522, 540);
            this.dgvBookingDetails.TabIndex = 2;
            this.dgvBookingDetails.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBookingDetails_CellClick);
            // 
            // tpOrderPendingTrxView
            // 
            this.tpOrderPendingTrxView.Controls.Add(this.dgvPendingTransactions);
            this.tpOrderPendingTrxView.Location = new System.Drawing.Point(4, 26);
            this.tpOrderPendingTrxView.Name = "tpOrderPendingTrxView";
            this.tpOrderPendingTrxView.Size = new System.Drawing.Size(522, 540);
            this.tpOrderPendingTrxView.TabIndex = 2;
            this.tpOrderPendingTrxView.Text = "Pending Transactions View";
            this.tpOrderPendingTrxView.UseVisualStyleBackColor = true;
            // 
            // dgvPendingTransactions
            // 
            this.dgvPendingTransactions.AllowUserToAddRows = false;
            this.dgvPendingTransactions.AllowUserToDeleteRows = false;
            this.dgvPendingTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvPendingTransactions.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvPendingTransactions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvPendingTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPendingTransactions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcPendingTrxRightClick});
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle35.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle35.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle35.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle35.SelectionBackColor = System.Drawing.Color.Turquoise;
            dataGridViewCellStyle35.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle35.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPendingTransactions.DefaultCellStyle = dataGridViewCellStyle35;
            this.dgvPendingTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPendingTransactions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvPendingTransactions.EnableHeadersVisualStyles = false;
            this.dgvPendingTransactions.Location = new System.Drawing.Point(0, 0);
            this.dgvPendingTransactions.Name = "dgvPendingTransactions";
            this.dgvPendingTransactions.ReadOnly = true;
            this.dgvPendingTransactions.RowHeadersVisible = false;
            this.dgvPendingTransactions.RowTemplate.Height = 40;
            this.dgvPendingTransactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPendingTransactions.Size = new System.Drawing.Size(522, 540);
            this.dgvPendingTransactions.TabIndex = 1;
            this.dgvPendingTransactions.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPendingTransactions_CellClick);
            this.dgvPendingTransactions.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPendingTransactions_CellDoubleClick);
            // 
            // dcPendingTrxRightClick
            // 
            this.dcPendingTrxRightClick.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcPendingTrxRightClick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dcPendingTrxRightClick.HeaderText = "...";
            this.dcPendingTrxRightClick.Name = "dcPendingTrxRightClick";
            this.dcPendingTrxRightClick.ReadOnly = true;
            this.dcPendingTrxRightClick.Text = "...";
            this.dcPendingTrxRightClick.UseColumnTextForButtonValue = true;
            this.dcPendingTrxRightClick.Width = 40;
            // 
            // btnCloseProductDetails
            // 
            this.btnCloseProductDetails.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseProductDetails.BackgroundImage = global::Parafait_POS.Properties.Resources.customer_button_normal;
            this.btnCloseProductDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCloseProductDetails.FlatAppearance.BorderSize = 0;
            this.btnCloseProductDetails.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCloseProductDetails.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCloseProductDetails.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCloseProductDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseProductDetails.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseProductDetails.ForeColor = System.Drawing.Color.White;
            this.btnCloseProductDetails.Location = new System.Drawing.Point(387, 567);
            this.btnCloseProductDetails.Name = "btnCloseProductDetails";
            this.btnCloseProductDetails.Size = new System.Drawing.Size(100, 44);
            this.btnCloseProductDetails.TabIndex = 20003;
            this.btnCloseProductDetails.Text = "Close";
            this.btnCloseProductDetails.UseVisualStyleBackColor = false;
            this.btnCloseProductDetails.Click += new System.EventHandler(this.btnCloseProductDetails_Click);
            // 
            // panelProductDetails
            // 
            this.panelProductDetails.BackColor = System.Drawing.Color.SlateGray;
            this.panelProductDetails.Controls.Add(this.btnCloseProductDetails);
            this.panelProductDetails.Location = new System.Drawing.Point(3, 2);
            this.panelProductDetails.Name = "panelProductDetails";
            this.panelProductDetails.Size = new System.Drawing.Size(875, 618);
            this.panelProductDetails.TabIndex = 20004;
            // 
            // timerClock
            // 
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            // 
            // statusStrip
            // 
            this.statusStrip.AllowItemReorder = true;
            this.statusStrip.Font = new System.Drawing.Font("Arial", 8.25F);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSiteName,
            this.toolStripVersion,
            this.toolStripPOSMachine,
            this.toolStripLoginID,
            this.toolStripPOSUser,
            this.toolStripRole,
            this.toolStripMessage,
            this.toolStripLANStatus,
            this.toolStripDateTime});
            this.statusStrip.Location = new System.Drawing.Point(0, 708);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1208, 25);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripSiteName
            // 
            this.toolStripSiteName.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSiteName.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripSiteName.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.toolStripSiteName.Name = "toolStripSiteName";
            this.toolStripSiteName.Size = new System.Drawing.Size(59, 20);
            this.toolStripSiteName.Text = "Site Name";
            // 
            // toolStripVersion
            // 
            this.toolStripVersion.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripVersion.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripVersion.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.toolStripVersion.Name = "toolStripVersion";
            this.toolStripVersion.Size = new System.Drawing.Size(48, 20);
            this.toolStripVersion.Text = "Version";
            // 
            // toolStripPOSMachine
            // 
            this.toolStripPOSMachine.AutoSize = false;
            this.toolStripPOSMachine.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripPOSMachine.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripPOSMachine.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.toolStripPOSMachine.Name = "toolStripPOSMachine";
            this.toolStripPOSMachine.Size = new System.Drawing.Size(350, 20);
            this.toolStripPOSMachine.Text = "Machine";
            this.toolStripPOSMachine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripLoginID
            // 
            this.toolStripLoginID.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripLoginID.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripLoginID.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.toolStripLoginID.Name = "toolStripLoginID";
            this.toolStripLoginID.Size = new System.Drawing.Size(46, 20);
            this.toolStripLoginID.Text = "LoginID";
            // 
            // toolStripPOSUser
            // 
            this.toolStripPOSUser.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripPOSUser.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripPOSUser.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.toolStripPOSUser.Name = "toolStripPOSUser";
            this.toolStripPOSUser.Size = new System.Drawing.Size(58, 20);
            this.toolStripPOSUser.Text = "POS User";
            // 
            // toolStripRole
            // 
            this.toolStripRole.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripRole.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripRole.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.toolStripRole.Name = "toolStripRole";
            this.toolStripRole.Size = new System.Drawing.Size(55, 20);
            this.toolStripRole.Text = "UserRole";
            this.toolStripRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripMessage
            // 
            this.toolStripMessage.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMessage.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripMessage.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.toolStripMessage.Name = "toolStripMessage";
            this.toolStripMessage.Size = new System.Drawing.Size(484, 20);
            this.toolStripMessage.Spring = true;
            this.toolStripMessage.Text = "Message";
            this.toolStripMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripLANStatus
            // 
            this.toolStripLANStatus.AutoSize = false;
            this.toolStripLANStatus.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripLANStatus.BackgroundImage = global::Parafait_POS.Properties.Resources.LanStatusImage;
            this.toolStripLANStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripLANStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripLANStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.toolStripLANStatus.Name = "toolStripLANStatus";
            this.toolStripLANStatus.Size = new System.Drawing.Size(60, 20);
            this.toolStripLANStatus.Text = "Online";
            this.toolStripLANStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripDateTime
            // 
            this.toolStripDateTime.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripDateTime.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripDateTime.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripDateTime.Name = "toolStripDateTime";
            this.toolStripDateTime.Size = new System.Drawing.Size(33, 20);
            this.toolStripDateTime.Text = "Date";
            // 
            // panelCardSwipe
            // 
            this.panelCardSwipe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCardSwipe.BackColor = System.Drawing.Color.Gray;
            this.panelCardSwipe.Controls.Add(this.btnCardBalancePrint);
            this.panelCardSwipe.Controls.Add(this.btnRegisterCustomer);
            this.panelCardSwipe.Controls.Add(this.btnParentChild);
            this.panelCardSwipe.Controls.Add(this.txtVIPStatus);
            this.panelCardSwipe.Controls.Add(this.labelCardNo);
            this.panelCardSwipe.Controls.Add(this.textBoxCustomerInfo);
            this.panelCardSwipe.Controls.Add(this.labelCardStatuslbl);
            this.panelCardSwipe.Controls.Add(this.labelCardStatus);
            this.panelCardSwipe.Controls.Add(this.labelCardNumber);
            this.panelCardSwipe.Controls.Add(this.dataGridViewCardDetails);
            this.panelCardSwipe.Controls.Add(this.btnGetServerCard);
            this.panelCardSwipe.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.panelCardSwipe.Location = new System.Drawing.Point(941, -8);
            this.panelCardSwipe.Name = "panelCardSwipe";
            this.panelCardSwipe.Size = new System.Drawing.Size(265, 394);
            this.panelCardSwipe.TabIndex = 2;
            this.panelCardSwipe.TabStop = false;
            // 
            // btnCardBalancePrint
            // 
            this.btnCardBalancePrint.BackColor = System.Drawing.Color.White;
            this.btnCardBalancePrint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCardBalancePrint.BackgroundImage")));
            this.btnCardBalancePrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCardBalancePrint.FlatAppearance.BorderSize = 0;
            this.btnCardBalancePrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnCardBalancePrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnCardBalancePrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardBalancePrint.Location = new System.Drawing.Point(198, 60);
            this.btnCardBalancePrint.Name = "btnCardBalancePrint";
            this.btnCardBalancePrint.Size = new System.Drawing.Size(32, 28);
            this.btnCardBalancePrint.TabIndex = 20;
            this.btnCardBalancePrint.UseVisualStyleBackColor = false;
            this.btnCardBalancePrint.Visible = false;
            this.btnCardBalancePrint.Click += new System.EventHandler(this.btnCardBalancePrint_Click);
            // 
            // btnRegisterCustomer
            // 
            this.btnRegisterCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnRegisterCustomer.BackgroundImage = global::Parafait_POS.Properties.Resources.Register_Icon;
            this.btnRegisterCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRegisterCustomer.FlatAppearance.BorderSize = 0;
            this.btnRegisterCustomer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRegisterCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRegisterCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegisterCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnRegisterCustomer.ForeColor = System.Drawing.Color.Snow;
            this.btnRegisterCustomer.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRegisterCustomer.Location = new System.Drawing.Point(222, 93);
            this.btnRegisterCustomer.Name = "btnRegisterCustomer";
            this.btnRegisterCustomer.Size = new System.Drawing.Size(40, 50);
            this.btnRegisterCustomer.TabIndex = 11;
            this.btnRegisterCustomer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRegisterCustomer.UseVisualStyleBackColor = false;
            this.btnRegisterCustomer.Click += new System.EventHandler(this.btnRegisterCustomer_Click);
            // 
            // btnParentChild
            // 
            this.btnParentChild.BackColor = System.Drawing.Color.White;
            this.btnParentChild.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnParentChild.BackgroundImage")));
            this.btnParentChild.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnParentChild.FlatAppearance.BorderSize = 0;
            this.btnParentChild.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnParentChild.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnParentChild.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParentChild.Location = new System.Drawing.Point(230, 60);
            this.btnParentChild.Name = "btnParentChild";
            this.btnParentChild.Size = new System.Drawing.Size(32, 28);
            this.btnParentChild.TabIndex = 10;
            this.btnParentChild.UseVisualStyleBackColor = false;
            this.btnParentChild.Click += new System.EventHandler(this.btnParentChild_Click);
            // 
            // txtVIPStatus
            // 
            this.txtVIPStatus.BackColor = System.Drawing.Color.White;
            this.txtVIPStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVIPStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txtVIPStatus.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVIPStatus.ForeColor = System.Drawing.Color.Red;
            this.txtVIPStatus.Location = new System.Drawing.Point(-1, 60);
            this.txtVIPStatus.Name = "txtVIPStatus";
            this.txtVIPStatus.ReadOnly = true;
            this.txtVIPStatus.Size = new System.Drawing.Size(231, 28);
            this.txtVIPStatus.TabIndex = 8;
            this.txtVIPStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVIPStatus.Click += new System.EventHandler(this.txtVIPStatus_Click);
            // 
            // labelCardNo
            // 
            this.labelCardNo.BackColor = System.Drawing.Color.MidnightBlue;
            this.labelCardNo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labelCardNo.Font = new System.Drawing.Font("Arial", 14.25F);
            this.labelCardNo.ForeColor = System.Drawing.Color.Yellow;
            this.labelCardNo.Location = new System.Drawing.Point(128, 12);
            this.labelCardNo.Name = "labelCardNo";
            this.labelCardNo.ReadOnly = true;
            this.labelCardNo.Size = new System.Drawing.Size(134, 22);
            this.labelCardNo.TabIndex = 7;
            this.labelCardNo.Text = "343245s34";
            this.labelCardNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxCustomerInfo
            // 
            this.textBoxCustomerInfo.BackColor = System.Drawing.Color.DarkRed;
            this.textBoxCustomerInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCustomerInfo.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBoxCustomerInfo.Font = new System.Drawing.Font("Arial", 9.75F);
            this.textBoxCustomerInfo.Location = new System.Drawing.Point(3, 90);
            this.textBoxCustomerInfo.Multiline = true;
            this.textBoxCustomerInfo.Name = "textBoxCustomerInfo";
            this.textBoxCustomerInfo.ReadOnly = true;
            this.textBoxCustomerInfo.Size = new System.Drawing.Size(216, 54);
            this.textBoxCustomerInfo.TabIndex = 6;
            // 
            // labelCardStatuslbl
            // 
            this.labelCardStatuslbl.AutoSize = true;
            this.labelCardStatuslbl.Font = new System.Drawing.Font("Arial", 12F);
            this.labelCardStatuslbl.ForeColor = System.Drawing.Color.Snow;
            this.labelCardStatuslbl.Location = new System.Drawing.Point(2, 36);
            this.labelCardStatuslbl.Name = "labelCardStatuslbl";
            this.labelCardStatuslbl.Size = new System.Drawing.Size(95, 18);
            this.labelCardStatuslbl.TabIndex = 5;
            this.labelCardStatuslbl.Text = "Card Status:";
            // 
            // labelCardStatus
            // 
            this.labelCardStatus.BackColor = System.Drawing.Color.MidnightBlue;
            this.labelCardStatus.Font = new System.Drawing.Font("Arial", 12F);
            this.labelCardStatus.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelCardStatus.Location = new System.Drawing.Point(128, 36);
            this.labelCardStatus.Name = "labelCardStatus";
            this.labelCardStatus.Size = new System.Drawing.Size(110, 18);
            this.labelCardStatus.TabIndex = 2;
            this.labelCardStatus.Text = "Card Status";
            this.labelCardStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCardNumber
            // 
            this.labelCardNumber.AutoSize = true;
            this.labelCardNumber.Font = new System.Drawing.Font("Arial", 14.25F);
            this.labelCardNumber.ForeColor = System.Drawing.Color.Snow;
            this.labelCardNumber.Location = new System.Drawing.Point(1, 11);
            this.labelCardNumber.Name = "labelCardNumber";
            this.labelCardNumber.Size = new System.Drawing.Size(128, 22);
            this.labelCardNumber.TabIndex = 0;
            this.labelCardNumber.Text = "Card Number:";
            // 
            // dataGridViewCardDetails
            // 
            this.dataGridViewCardDetails.AllowUserToAddRows = false;
            this.dataGridViewCardDetails.AllowUserToDeleteRows = false;
            this.dataGridViewCardDetails.AllowUserToResizeColumns = false;
            this.dataGridViewCardDetails.AllowUserToResizeRows = false;
            this.dataGridViewCardDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewCardDetails.BackgroundColor = System.Drawing.Color.Brown;
            this.dataGridViewCardDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle36.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle36.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle36.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle36.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle36.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle36.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCardDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle36;
            this.dataGridViewCardDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewCardDetails.ColumnHeadersVisible = false;
            this.dataGridViewCardDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnHeader,
            this.Value});
            this.dataGridViewCardDetails.EnableHeadersVisualStyles = false;
            this.dataGridViewCardDetails.Location = new System.Drawing.Point(0, 140);
            this.dataGridViewCardDetails.Name = "dataGridViewCardDetails";
            this.dataGridViewCardDetails.RowHeadersVisible = false;
            this.dataGridViewCardDetails.RowTemplate.ReadOnly = true;
            this.dataGridViewCardDetails.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCardDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCardDetails.Size = new System.Drawing.Size(265, 253);
            this.dataGridViewCardDetails.TabIndex = 3;
            this.dataGridViewCardDetails.TabStop = false;
            this.dataGridViewCardDetails.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCardDetails_CellContentClick);
            // 
            // ColumnHeader
            // 
            dataGridViewCellStyle37.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle37.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle37.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle37.SelectionBackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle37.SelectionForeColor = System.Drawing.Color.White;
            this.ColumnHeader.DefaultCellStyle = dataGridViewCellStyle37;
            this.ColumnHeader.HeaderText = "ColumnHeader";
            this.ColumnHeader.Name = "ColumnHeader";
            this.ColumnHeader.Width = 5;
            // 
            // Value
            // 
            dataGridViewCellStyle38.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle38.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle38.SelectionForeColor = System.Drawing.Color.Black;
            this.Value.DefaultCellStyle = dataGridViewCellStyle38;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.Width = 5;
            // 
            // btnGetServerCard
            // 
            this.btnGetServerCard.BackColor = System.Drawing.Color.Transparent;
            this.btnGetServerCard.BackgroundImage = global::Parafait_POS.Properties.Resources.Globe_Icon;
            this.btnGetServerCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGetServerCard.FlatAppearance.BorderSize = 0;
            this.btnGetServerCard.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnGetServerCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGetServerCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGetServerCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetServerCard.Location = new System.Drawing.Point(238, 34);
            this.btnGetServerCard.Name = "btnGetServerCard";
            this.btnGetServerCard.Size = new System.Drawing.Size(26, 24);
            this.btnGetServerCard.TabIndex = 9;
            this.btnGetServerCard.UseVisualStyleBackColor = false;
            this.btnGetServerCard.Click += new System.EventHandler(this.btnGetServerCard_Click);
            // 
            // txtChangeAmount
            // 
            this.txtChangeAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChangeAmount.BackColor = System.Drawing.Color.Gainsboro;
            this.txtChangeAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChangeAmount.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChangeAmount.Location = new System.Drawing.Point(98, 82);
            this.txtChangeAmount.Name = "txtChangeAmount";
            this.txtChangeAmount.ReadOnly = true;
            this.txtChangeAmount.Size = new System.Drawing.Size(163, 27);
            this.txtChangeAmount.TabIndex = 10;
            this.txtChangeAmount.TabStop = false;
            this.txtChangeAmount.Text = "0.00";
            this.txtChangeAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxTendered
            // 
            this.textBoxTendered.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTendered.BackColor = System.Drawing.Color.Gainsboro;
            this.textBoxTendered.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTendered.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTendered.Location = new System.Drawing.Point(98, 56);
            this.textBoxTendered.Name = "textBoxTendered";
            this.textBoxTendered.ReadOnly = true;
            this.textBoxTendered.Size = new System.Drawing.Size(163, 27);
            this.textBoxTendered.TabIndex = 9;
            this.textBoxTendered.TabStop = false;
            this.textBoxTendered.Text = "0.00";
            this.textBoxTendered.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxTransactionTotal
            // 
            this.textBoxTransactionTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTransactionTotal.BackColor = System.Drawing.Color.Silver;
            this.textBoxTransactionTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTransactionTotal.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTransactionTotal.Location = new System.Drawing.Point(98, 4);
            this.textBoxTransactionTotal.Name = "textBoxTransactionTotal";
            this.textBoxTransactionTotal.ReadOnly = true;
            this.textBoxTransactionTotal.Size = new System.Drawing.Size(163, 27);
            this.textBoxTransactionTotal.TabIndex = 8;
            this.textBoxTransactionTotal.TabStop = false;
            this.textBoxTransactionTotal.Text = "0.00";
            this.textBoxTransactionTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelBalance
            // 
            this.labelBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBalance.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBalance.ForeColor = System.Drawing.Color.Snow;
            this.labelBalance.Location = new System.Drawing.Point(3, 86);
            this.labelBalance.Name = "labelBalance";
            this.labelBalance.Size = new System.Drawing.Size(93, 22);
            this.labelBalance.TabIndex = 7;
            this.labelBalance.Text = "Change";
            this.labelBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelTransactionTotal
            // 
            this.labelTransactionTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTransactionTotal.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTransactionTotal.ForeColor = System.Drawing.Color.Snow;
            this.labelTransactionTotal.Location = new System.Drawing.Point(3, 8);
            this.labelTransactionTotal.Name = "labelTransactionTotal";
            this.labelTransactionTotal.Size = new System.Drawing.Size(93, 22);
            this.labelTransactionTotal.TabIndex = 5;
            this.labelTransactionTotal.Text = "Total";
            this.labelTransactionTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panelAmounts
            // 
            this.panelAmounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAmounts.BackColor = System.Drawing.Color.Gray;
            this.panelAmounts.Controls.Add(this.label24);
            this.panelAmounts.Controls.Add(this.txtTipAmount);
            this.panelAmounts.Controls.Add(this.label16);
            this.panelAmounts.Controls.Add(this.label13);
            this.panelAmounts.Controls.Add(this.txtBalanceAmount);
            this.panelAmounts.Controls.Add(this.textBoxTransactionTotal);
            this.panelAmounts.Controls.Add(this.labelTransactionTotal);
            this.panelAmounts.Controls.Add(this.labelBalance);
            this.panelAmounts.Controls.Add(this.textBoxTendered);
            this.panelAmounts.Controls.Add(this.txtChangeAmount);
            this.panelAmounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.panelAmounts.Location = new System.Drawing.Point(941, 389);
            this.panelAmounts.Name = "panelAmounts";
            this.panelAmounts.Size = new System.Drawing.Size(265, 138);
            this.panelAmounts.TabIndex = 5;
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label24.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.Snow;
            this.label24.Location = new System.Drawing.Point(3, 111);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(93, 22);
            this.label24.TabIndex = 14;
            this.label24.Text = "Tip Amount";
            this.label24.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtTipAmount
            // 
            this.txtTipAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTipAmount.BackColor = System.Drawing.Color.Gainsboro;
            this.txtTipAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTipAmount.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTipAmount.Location = new System.Drawing.Point(98, 108);
            this.txtTipAmount.Name = "txtTipAmount";
            this.txtTipAmount.ReadOnly = true;
            this.txtTipAmount.Size = new System.Drawing.Size(163, 27);
            this.txtTipAmount.TabIndex = 15;
            this.txtTipAmount.TabStop = false;
            this.txtTipAmount.Text = "0.00";
            this.txtTipAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Snow;
            this.label16.Location = new System.Drawing.Point(3, 34);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(93, 22);
            this.label16.TabIndex = 13;
            this.label16.Text = "Balance";
            this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Snow;
            this.label13.Location = new System.Drawing.Point(3, 61);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(93, 22);
            this.label13.TabIndex = 11;
            this.label13.Text = "Tendered";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtBalanceAmount
            // 
            this.txtBalanceAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBalanceAmount.BackColor = System.Drawing.Color.Silver;
            this.txtBalanceAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBalanceAmount.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBalanceAmount.Location = new System.Drawing.Point(98, 30);
            this.txtBalanceAmount.Name = "txtBalanceAmount";
            this.txtBalanceAmount.ReadOnly = true;
            this.txtBalanceAmount.Size = new System.Drawing.Size(163, 27);
            this.txtBalanceAmount.TabIndex = 12;
            this.txtBalanceAmount.TabStop = false;
            this.txtBalanceAmount.Text = "0.00";
            this.txtBalanceAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // colorDialog
            // 
            this.colorDialog.Color = System.Drawing.Color.Gray;
            // 
            // btnRefreshPOS
            // 
            this.btnRefreshPOS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshPOS.BackColor = System.Drawing.Color.Transparent;
            this.btnRefreshPOS.FlatAppearance.BorderSize = 0;
            this.btnRefreshPOS.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRefreshPOS.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefreshPOS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefreshPOS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshPOS.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnRefreshPOS.ForeColor = System.Drawing.Color.White;
            this.btnRefreshPOS.Image = global::Parafait_POS.Properties.Resources.refresh;
            this.btnRefreshPOS.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRefreshPOS.Location = new System.Drawing.Point(-2, 3);
            this.btnRefreshPOS.Name = "btnRefreshPOS";
            this.btnRefreshPOS.Size = new System.Drawing.Size(60, 60);
            this.btnRefreshPOS.TabIndex = 19;
            this.btnRefreshPOS.Text = "Refresh";
            this.btnRefreshPOS.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRefreshPOS.UseVisualStyleBackColor = true;
            this.btnRefreshPOS.Click += new System.EventHandler(this.btnPOSRefresh_Click);
            // 
            // buttonLogout
            // 
            this.buttonLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogout.BackColor = System.Drawing.Color.Transparent;
            this.buttonLogout.FlatAppearance.BorderSize = 0;
            this.buttonLogout.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonLogout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogout.Font = new System.Drawing.Font("Arial", 8.25F);
            this.buttonLogout.ForeColor = System.Drawing.Color.White;
            this.buttonLogout.Image = global::Parafait_POS.Properties.Resources.logout;
            this.buttonLogout.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonLogout.Location = new System.Drawing.Point(204, 3);
            this.buttonLogout.Name = "buttonLogout";
            this.buttonLogout.Size = new System.Drawing.Size(60, 60);
            this.buttonLogout.TabIndex = 17;
            this.buttonLogout.Text = "Logout";
            this.buttonLogout.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonLogout.UseVisualStyleBackColor = false;
            this.buttonLogout.Click += new System.EventHandler(this.buttonLogout_Click);
            // 
            // btnShowNumPad
            // 
            this.btnShowNumPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowNumPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatAppearance.BorderSize = 0;
            this.btnShowNumPad.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowNumPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnShowNumPad.ForeColor = System.Drawing.Color.White;
            this.btnShowNumPad.Image = global::Parafait_POS.Properties.Resources.Keypad;
            this.btnShowNumPad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnShowNumPad.Location = new System.Drawing.Point(153, 3);
            this.btnShowNumPad.Name = "btnShowNumPad";
            this.btnShowNumPad.Size = new System.Drawing.Size(60, 60);
            this.btnShowNumPad.TabIndex = 18;
            this.btnShowNumPad.Text = "Key Pad";
            this.btnShowNumPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowNumPad.UseVisualStyleBackColor = false;
            this.btnShowNumPad.Click += new System.EventHandler(this.btnShowNumPad_Click);
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label26.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label26.Location = new System.Drawing.Point(1190, 645);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(255, 1);
            this.label26.TabIndex = 22;
            // 
            // btnLaunchApps
            // 
            this.btnLaunchApps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLaunchApps.BackColor = System.Drawing.Color.Transparent;
            this.btnLaunchApps.FlatAppearance.BorderSize = 0;
            this.btnLaunchApps.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnLaunchApps.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLaunchApps.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLaunchApps.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLaunchApps.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnLaunchApps.ForeColor = System.Drawing.Color.White;
            this.btnLaunchApps.Image = global::Parafait_POS.Properties.Resources.launch;
            this.btnLaunchApps.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLaunchApps.Location = new System.Drawing.Point(100, 3);
            this.btnLaunchApps.Name = "btnLaunchApps";
            this.btnLaunchApps.Size = new System.Drawing.Size(60, 60);
            this.btnLaunchApps.TabIndex = 23;
            this.btnLaunchApps.Text = "Launch";
            this.btnLaunchApps.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLaunchApps.UseVisualStyleBackColor = false;
            this.btnLaunchApps.Click += new System.EventHandler(this.btnLaunchApps_Click);
            // 
            // btnTasks
            // 
            this.btnTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTasks.BackColor = System.Drawing.Color.Transparent;
            this.btnTasks.FlatAppearance.BorderSize = 0;
            this.btnTasks.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTasks.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTasks.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTasks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTasks.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnTasks.ForeColor = System.Drawing.Color.White;
            this.btnTasks.Image = global::Parafait_POS.Properties.Resources.tasks;
            this.btnTasks.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnTasks.Location = new System.Drawing.Point(48, 3);
            this.btnTasks.Name = "btnTasks";
            this.btnTasks.Size = new System.Drawing.Size(60, 60);
            this.btnTasks.TabIndex = 24;
            this.btnTasks.Text = "Tasks";
            this.btnTasks.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTasks.UseVisualStyleBackColor = false;
            this.btnTasks.Click += new System.EventHandler(this.btnTasks_Click);
            // 
            // POSTasksContextMenu
            // 
            this.POSTasksContextMenu.BackColor = System.Drawing.Color.Gray;
            this.POSTasksContextMenu.Font = new System.Drawing.Font("Arial", 14F);
            this.POSTasksContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.poolKaraokeLightControlToolStripMenuItem,
            this.menuOpenCashDrawer,
            this.menuAddToShift,
            this.lockerFunctionsToolStripMenuItem,
            this.toolStripMenuOnlineFunctions,
            this.tsMenuCreditCardGatewayFunctions,
            this.gameManagementToolStripMenuItem,
            this.cardFunctionsToolStripMenuItem,
            this.menuStaffFunctions,
            this.menuItemFiscalPrinterReports,
            this.menuVariableCashRefund,
            this.achievements,
            this.couponStatusToolStripMenuItem,
            this.menuRunReport,
            this.menuFlagVoucher,
            this.notificationDevice,
            this.receiveStock,
            this.retailInventoryLookUp,
            this.menuItemAccessControl,
            this.menuTransactionFunctions,
            this.menuFoodDeliveryFunctions});
            this.POSTasksContextMenu.Name = "POSTasksContextMenu";
            this.POSTasksContextMenu.Size = new System.Drawing.Size(309, 676);
            this.POSTasksContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.POSTasksContextMenu_ItemClicked);
            // 
            // poolKaraokeLightControlToolStripMenuItem
            // 
            this.poolKaraokeLightControlToolStripMenuItem.AutoSize = false;
            this.poolKaraokeLightControlToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.poolKaraokeLightControlToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.poolKaraokeLightControlToolStripMenuItem.Font = new System.Drawing.Font("Gadugi", 14F);
            this.poolKaraokeLightControlToolStripMenuItem.Name = "poolKaraokeLightControlToolStripMenuItem";
            this.poolKaraokeLightControlToolStripMenuItem.Size = new System.Drawing.Size(306, 32);
            this.poolKaraokeLightControlToolStripMenuItem.Text = "Pool/Karaoke Light Control";
            // 
            // menuOpenCashDrawer
            // 
            this.menuOpenCashDrawer.AutoSize = false;
            this.menuOpenCashDrawer.BackColor = System.Drawing.Color.PaleTurquoise;
            this.menuOpenCashDrawer.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.menuOpenCashDrawer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuOpenCashDrawer.Font = new System.Drawing.Font("Gadugi", 14F);
            this.menuOpenCashDrawer.ForeColor = System.Drawing.Color.Black;
            this.menuOpenCashDrawer.Name = "menuOpenCashDrawer";
            this.menuOpenCashDrawer.Size = new System.Drawing.Size(306, 32);
            this.menuOpenCashDrawer.Text = "Open Cash Drawer";
            // 
            // menuAddToShift
            // 
            this.menuAddToShift.AutoSize = false;
            this.menuAddToShift.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.menuAddToShift.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuAddToShift.Font = new System.Drawing.Font("Gadugi", 14F);
            this.menuAddToShift.ForeColor = System.Drawing.Color.Black;
            this.menuAddToShift.Name = "menuAddToShift";
            this.menuAddToShift.Size = new System.Drawing.Size(306, 32);
            this.menuAddToShift.Text = "Add Cash / Cards to Shift";
            // 
            // lockerFunctionsToolStripMenuItem
            // 
            this.lockerFunctionsToolStripMenuItem.AutoSize = false;
            this.lockerFunctionsToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.lockerFunctionsToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lockerFunctionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lockerLayoutToolStripMenuItem,
            this.lockerUtilityToolStripMenuItem});
            this.lockerFunctionsToolStripMenuItem.Font = new System.Drawing.Font("Gadugi", 14F);
            this.lockerFunctionsToolStripMenuItem.Name = "lockerFunctionsToolStripMenuItem";
            this.lockerFunctionsToolStripMenuItem.Size = new System.Drawing.Size(306, 32);
            this.lockerFunctionsToolStripMenuItem.Text = "Locker Functions";
            // 
            // lockerLayoutToolStripMenuItem
            // 
            this.lockerLayoutToolStripMenuItem.AutoSize = false;
            this.lockerLayoutToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.lockerLayoutToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lockerLayoutToolStripMenuItem.Name = "lockerLayoutToolStripMenuItem";
            this.lockerLayoutToolStripMenuItem.Size = new System.Drawing.Size(199, 32);
            this.lockerLayoutToolStripMenuItem.Text = "Locker Layout";
            this.lockerLayoutToolStripMenuItem.Click += new System.EventHandler(this.lockerLayoutToolStripMenuItem_Click);
            // 
            // lockerUtilityToolStripMenuItem
            // 
            this.lockerUtilityToolStripMenuItem.AutoSize = false;
            this.lockerUtilityToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.lockerUtilityToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lockerUtilityToolStripMenuItem.Name = "lockerUtilityToolStripMenuItem";
            this.lockerUtilityToolStripMenuItem.Size = new System.Drawing.Size(199, 32);
            this.lockerUtilityToolStripMenuItem.Text = "Locker Utility";
            this.lockerUtilityToolStripMenuItem.Click += new System.EventHandler(this.lockerUtilityToolStripMenuItem_Click);
            // 
            // toolStripMenuOnlineFunctions
            // 
            this.toolStripMenuOnlineFunctions.AutoSize = false;
            this.toolStripMenuOnlineFunctions.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.toolStripMenuOnlineFunctions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripMenuOnlineFunctions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeTransactionToolStripMenuItem});
            this.toolStripMenuOnlineFunctions.Font = new System.Drawing.Font("Gadugi", 14F);
            this.toolStripMenuOnlineFunctions.Name = "toolStripMenuOnlineFunctions";
            this.toolStripMenuOnlineFunctions.Size = new System.Drawing.Size(306, 32);
            this.toolStripMenuOnlineFunctions.Text = "Online Functions";
            // 
            // executeTransactionToolStripMenuItem
            // 
            this.executeTransactionToolStripMenuItem.AutoSize = false;
            this.executeTransactionToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.executeTransactionToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.executeTransactionToolStripMenuItem.Name = "executeTransactionToolStripMenuItem";
            this.executeTransactionToolStripMenuItem.Size = new System.Drawing.Size(251, 32);
            this.executeTransactionToolStripMenuItem.Text = "Execute Transaction";
            this.executeTransactionToolStripMenuItem.Click += new System.EventHandler(this.executeTransactionToolStripMenuItem_Click);
            // 
            // tsMenuCreditCardGatewayFunctions
            // 
            this.tsMenuCreditCardGatewayFunctions.AutoSize = false;
            this.tsMenuCreditCardGatewayFunctions.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.tsMenuCreditCardGatewayFunctions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tsMenuCreditCardGatewayFunctions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNewCompleteFDTransactions});
            this.tsMenuCreditCardGatewayFunctions.Font = new System.Drawing.Font("Gadugi", 14F);
            this.tsMenuCreditCardGatewayFunctions.Name = "tsMenuCreditCardGatewayFunctions";
            this.tsMenuCreditCardGatewayFunctions.Size = new System.Drawing.Size(306, 32);
            this.tsMenuCreditCardGatewayFunctions.Text = "CC Gateway Tasks";
            // 
            // menuNewCompleteFDTransactions
            // 
            this.menuNewCompleteFDTransactions.AutoSize = false;
            this.menuNewCompleteFDTransactions.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.menuNewCompleteFDTransactions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuNewCompleteFDTransactions.Name = "menuNewCompleteFDTransactions";
            this.menuNewCompleteFDTransactions.Size = new System.Drawing.Size(375, 32);
            this.menuNewCompleteFDTransactions.Text = "Complete Credit Card Transactions";
            this.menuNewCompleteFDTransactions.Click += new System.EventHandler(this.menuNewCompleteFDTransactions_Click);
            // 
            // gameManagementToolStripMenuItem
            // 
            this.gameManagementToolStripMenuItem.AutoSize = false;
            this.gameManagementToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.gameManagementToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gameManagementToolStripMenuItem.Font = new System.Drawing.Font("Gadugi", 14F);
            this.gameManagementToolStripMenuItem.Name = "gameManagementToolStripMenuItem";
            this.gameManagementToolStripMenuItem.Size = new System.Drawing.Size(306, 32);
            this.gameManagementToolStripMenuItem.Text = "Game Management";
            // 
            // cardFunctionsToolStripMenuItem
            // 
            this.cardFunctionsToolStripMenuItem.AutoSize = false;
            this.cardFunctionsToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.cardFunctionsToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cardFunctionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTechCard,
            this.menuViewTasks,
            this.menuParentChildCards,
            this.deactivateCardsToolStripMenuItem,
            this.masterCardsToolStripMenuItem,
            this.tokenCardInventoryToolStripMenuItem,
            this.legacyTransferToolStripMenuItem});
            this.cardFunctionsToolStripMenuItem.Font = new System.Drawing.Font("Gadugi", 14F);
            this.cardFunctionsToolStripMenuItem.Name = "cardFunctionsToolStripMenuItem";
            this.cardFunctionsToolStripMenuItem.Size = new System.Drawing.Size(306, 32);
            this.cardFunctionsToolStripMenuItem.Text = "Card Functions";
            // 
            // menuTechCard
            // 
            this.menuTechCard.AutoSize = false;
            this.menuTechCard.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.menuTechCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuTechCard.Name = "menuTechCard";
            this.menuTechCard.Size = new System.Drawing.Size(269, 32);
            this.menuTechCard.Text = "Manage Staff Cards";
            this.menuTechCard.Click += new System.EventHandler(this.menuTechCard_Click);
            // 
            // menuViewTasks
            // 
            this.menuViewTasks.AutoSize = false;
            this.menuViewTasks.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.menuViewTasks.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuViewTasks.Name = "menuViewTasks";
            this.menuViewTasks.Size = new System.Drawing.Size(269, 32);
            this.menuViewTasks.Text = "View Card Tasks";
            this.menuViewTasks.Click += new System.EventHandler(this.menuViewTasks_Click);
            // 
            // menuParentChildCards
            // 
            this.menuParentChildCards.AutoSize = false;
            this.menuParentChildCards.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.menuParentChildCards.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuParentChildCards.Name = "menuParentChildCards";
            this.menuParentChildCards.Size = new System.Drawing.Size(269, 32);
            this.menuParentChildCards.Text = "Parent-Child Cards";
            this.menuParentChildCards.Click += new System.EventHandler(this.menuParentChildCards_Click);
            // 
            // deactivateCardsToolStripMenuItem
            // 
            this.deactivateCardsToolStripMenuItem.AutoSize = false;
            this.deactivateCardsToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.deactivateCardsToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.deactivateCardsToolStripMenuItem.Name = "deactivateCardsToolStripMenuItem";
            this.deactivateCardsToolStripMenuItem.Size = new System.Drawing.Size(269, 32);
            this.deactivateCardsToolStripMenuItem.Text = "Deactivate Cards";
            this.deactivateCardsToolStripMenuItem.Click += new System.EventHandler(this.deactivateCardsToolStripMenuItem_Click);
            // 
            // masterCardsToolStripMenuItem
            // 
            this.masterCardsToolStripMenuItem.AutoSize = false;
            this.masterCardsToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.masterCardsToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.masterCardsToolStripMenuItem.Name = "masterCardsToolStripMenuItem";
            this.masterCardsToolStripMenuItem.Size = new System.Drawing.Size(269, 32);
            this.masterCardsToolStripMenuItem.Text = "Master Cards";
            this.masterCardsToolStripMenuItem.Click += new System.EventHandler(this.masterCardsToolStripMenuItem_Click);
            // 
            // tokenCardInventoryToolStripMenuItem
            // 
            this.tokenCardInventoryToolStripMenuItem.AutoSize = false;
            this.tokenCardInventoryToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.tokenCardInventoryToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tokenCardInventoryToolStripMenuItem.Name = "tokenCardInventoryToolStripMenuItem";
            this.tokenCardInventoryToolStripMenuItem.Size = new System.Drawing.Size(269, 32);
            this.tokenCardInventoryToolStripMenuItem.Text = "Token / Card Inventory";
            this.tokenCardInventoryToolStripMenuItem.Click += new System.EventHandler(this.tokenCardInventoryToolStripMenuItem_Click);
            // 
            // legacyTransferToolStripMenuItem
            // 
            this.legacyTransferToolStripMenuItem.AutoSize = false;
            this.legacyTransferToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.legacyTransferToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.legacyTransferToolStripMenuItem.Name = "legacyTransferToolStripMenuItem";
            this.legacyTransferToolStripMenuItem.Size = new System.Drawing.Size(269, 32);
            this.legacyTransferToolStripMenuItem.Text = "Legacy Card Transfer";
            this.legacyTransferToolStripMenuItem.Click += new System.EventHandler(this.legacyTransferToolStripMenuItem_Click);
            // 
            // menuStaffFunctions
            // 
            this.menuStaffFunctions.AutoSize = false;
            this.menuStaffFunctions.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.menuStaffFunctions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuStaffFunctions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fpEnrollDeactiveToolStripMenuItem,
            this.attendanceToolStripMenuItem,
            this.timeSheetDetailsToolStripMenuItem,
            this.lockScreenToolStripMenuItem,
            this.changeLoginToolStripMenuItem,
            this.enterMessageToolStripMenuItem,
            this.changeLayoutToolStripMenuItem,
            this.remoteShiftOpenCloseToolStripMenuItem,
            this.productAvailabilityToolStripMenuItem,
            this.cashdrawerAssignmentToolStripMenuItem});
            this.menuStaffFunctions.Font = new System.Drawing.Font("Gadugi", 14F);
            this.menuStaffFunctions.Name = "menuStaffFunctions";
            this.menuStaffFunctions.Size = new System.Drawing.Size(306, 32);
            this.menuStaffFunctions.Text = "Staff Functions";
            // 
            // fpEnrollDeactiveToolStripMenuItem
            // 
            this.fpEnrollDeactiveToolStripMenuItem.AutoSize = false;
            this.fpEnrollDeactiveToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.fpEnrollDeactiveToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.fpEnrollDeactiveToolStripMenuItem.Name = "fpEnrollDeactiveToolStripMenuItem";
            this.fpEnrollDeactiveToolStripMenuItem.Size = new System.Drawing.Size(313, 32);
            this.fpEnrollDeactiveToolStripMenuItem.Text = "FingerPrint Enroll/Deactive";
            this.fpEnrollDeactiveToolStripMenuItem.Click += new System.EventHandler(this.fpEnrollDeactiveToolStripMenuItem_Click);
            // 
            // attendanceToolStripMenuItem
            // 
            this.attendanceToolStripMenuItem.AutoSize = false;
            this.attendanceToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.attendanceToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.attendanceToolStripMenuItem.Name = "attendanceToolStripMenuItem";
            this.attendanceToolStripMenuItem.Size = new System.Drawing.Size(313, 32);
            this.attendanceToolStripMenuItem.Text = "Record Attendance";
            this.attendanceToolStripMenuItem.Click += new System.EventHandler(this.attendanceToolStripMenuItem_Click);
            // 
            // timeSheetDetailsToolStripMenuItem
            // 
            this.timeSheetDetailsToolStripMenuItem.AutoSize = false;
            this.timeSheetDetailsToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.timeSheetDetailsToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.timeSheetDetailsToolStripMenuItem.Name = "timeSheetDetailsToolStripMenuItem";
            this.timeSheetDetailsToolStripMenuItem.Size = new System.Drawing.Size(313, 32);
            this.timeSheetDetailsToolStripMenuItem.Text = "Manage POS Attendance";
            this.timeSheetDetailsToolStripMenuItem.Click += new System.EventHandler(this.timeSheetDetailsToolStripMenuItem_Click);
            // 
            // lockScreenToolStripMenuItem
            // 
            this.lockScreenToolStripMenuItem.AutoSize = false;
            this.lockScreenToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.lockScreenToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lockScreenToolStripMenuItem.Name = "lockScreenToolStripMenuItem";
            this.lockScreenToolStripMenuItem.Size = new System.Drawing.Size(313, 32);
            this.lockScreenToolStripMenuItem.Text = "Lock Screen";
            this.lockScreenToolStripMenuItem.Click += new System.EventHandler(this.lockScreenToolStripMenuItem_Click);
            // 
            // changeLoginToolStripMenuItem
            // 
            this.changeLoginToolStripMenuItem.AutoSize = false;
            this.changeLoginToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.changeLoginToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.changeLoginToolStripMenuItem.Name = "changeLoginToolStripMenuItem";
            this.changeLoginToolStripMenuItem.Size = new System.Drawing.Size(313, 32);
            this.changeLoginToolStripMenuItem.Text = "Change Login";
            this.changeLoginToolStripMenuItem.Click += new System.EventHandler(this.changeLoginToolStripMenuItem_Click);
            // 
            // enterMessageToolStripMenuItem
            // 
            this.enterMessageToolStripMenuItem.AutoSize = false;
            this.enterMessageToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.enterMessageToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.enterMessageToolStripMenuItem.Name = "enterMessageToolStripMenuItem";
            this.enterMessageToolStripMenuItem.Size = new System.Drawing.Size(313, 32);
            this.enterMessageToolStripMenuItem.Text = "Enter Message";
            this.enterMessageToolStripMenuItem.Click += new System.EventHandler(this.enterMessageToolStripMenuItem_Click);
            // 
            // changeLayoutToolStripMenuItem
            // 
            this.changeLayoutToolStripMenuItem.AutoSize = false;
            this.changeLayoutToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.changeLayoutToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.changeLayoutToolStripMenuItem.Name = "changeLayoutToolStripMenuItem";
            this.changeLayoutToolStripMenuItem.Size = new System.Drawing.Size(313, 32);
            this.changeLayoutToolStripMenuItem.Text = "Change Layout";
            this.changeLayoutToolStripMenuItem.Click += new System.EventHandler(this.changeLayoutToolStripMenuItem_Click);
            // 
            // remoteShiftOpenCloseToolStripMenuItem
            // 
            this.remoteShiftOpenCloseToolStripMenuItem.AutoSize = false;
            this.remoteShiftOpenCloseToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.remoteShiftOpenCloseToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.remoteShiftOpenCloseToolStripMenuItem.Name = "remoteShiftOpenCloseToolStripMenuItem";
            this.remoteShiftOpenCloseToolStripMenuItem.Size = new System.Drawing.Size(313, 32);
            this.remoteShiftOpenCloseToolStripMenuItem.Text = "Remote Shift Open/Close";
            this.remoteShiftOpenCloseToolStripMenuItem.Click += new System.EventHandler(this.RemoteShiftOpenCloseToolStripMenuItem_Click);
            // 
            // productAvailabilityToolStripMenuItem
            // 
            this.productAvailabilityToolStripMenuItem.AutoSize = false;
            this.productAvailabilityToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.productAvailabilityToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.productAvailabilityToolStripMenuItem.Name = "productAvailabilityToolStripMenuItem";
            this.productAvailabilityToolStripMenuItem.Size = new System.Drawing.Size(313, 32);
            this.productAvailabilityToolStripMenuItem.Text = "Change Product Availability";
            this.productAvailabilityToolStripMenuItem.Click += new System.EventHandler(this.productAvailabilityToolStripMenuItem_Click);
            // 
            // cashdrawerAssignmentToolStripMenuItem
            // 
            this.cashdrawerAssignmentToolStripMenuItem.AutoSize = false;
            this.cashdrawerAssignmentToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Drop_Down_Normal1;
            this.cashdrawerAssignmentToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cashdrawerAssignmentToolStripMenuItem.Name = "cashdrawerAssignmentToolStripMenuItem";
            this.cashdrawerAssignmentToolStripMenuItem.Size = new System.Drawing.Size(313, 32);
            this.cashdrawerAssignmentToolStripMenuItem.Text = "Assign Cash Drawer";
            this.cashdrawerAssignmentToolStripMenuItem.Click += new System.EventHandler(this.CashdrawerAssignmentToolStripMenuItem_Click);
            // 
            // menuItemFiscalPrinterReports
            // 
            this.menuItemFiscalPrinterReports.AutoSize = false;
            this.menuItemFiscalPrinterReports.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.menuItemFiscalPrinterReports.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuItemFiscalPrinterReports.Font = new System.Drawing.Font("Gadugi", 14F);
            this.menuItemFiscalPrinterReports.Name = "menuItemFiscalPrinterReports";
            this.menuItemFiscalPrinterReports.Size = new System.Drawing.Size(306, 32);
            this.menuItemFiscalPrinterReports.Text = "Fiscal Printer Reports";
            this.menuItemFiscalPrinterReports.Visible = false;
            // 
            // menuVariableCashRefund
            // 
            this.menuVariableCashRefund.AutoSize = false;
            this.menuVariableCashRefund.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.menuVariableCashRefund.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuVariableCashRefund.Font = new System.Drawing.Font("Gadugi", 14F);
            this.menuVariableCashRefund.Name = "menuVariableCashRefund";
            this.menuVariableCashRefund.Size = new System.Drawing.Size(306, 32);
            this.menuVariableCashRefund.Text = "Variable Cash Refund";
            // 
            // achievements
            // 
            this.achievements.AutoSize = false;
            this.achievements.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.achievements.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.achievements.Font = new System.Drawing.Font("Gadugi", 14F);
            this.achievements.Name = "achievements";
            this.achievements.Size = new System.Drawing.Size(306, 32);
            this.achievements.Text = "Achievements";
            // 
            // couponStatusToolStripMenuItem
            // 
            this.couponStatusToolStripMenuItem.AutoSize = false;
            this.couponStatusToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.couponStatusToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.couponStatusToolStripMenuItem.Font = new System.Drawing.Font("Gadugi", 14F);
            this.couponStatusToolStripMenuItem.Name = "couponStatusToolStripMenuItem";
            this.couponStatusToolStripMenuItem.Size = new System.Drawing.Size(308, 32);
            this.couponStatusToolStripMenuItem.Text = "Coupon Status";
            // 
            // menuRunReport
            // 
            this.menuRunReport.AutoSize = false;
            this.menuRunReport.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.menuRunReport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuRunReport.Font = new System.Drawing.Font("Gadugi", 14F);
            this.menuRunReport.Name = "menuRunReport";
            this.menuRunReport.Size = new System.Drawing.Size(308, 32);
            this.menuRunReport.Text = "Run Reports";
            // 
            // menuFlagVoucher
            // 
            this.menuFlagVoucher.AutoSize = false;
            this.menuFlagVoucher.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.menuFlagVoucher.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuFlagVoucher.Font = new System.Drawing.Font("Gadugi", 14F);
            this.menuFlagVoucher.Name = "menuFlagVoucher";
            this.menuFlagVoucher.Size = new System.Drawing.Size(308, 32);
            this.menuFlagVoucher.Text = "Flag Voucher";
            // 
            // notificationDevice
            // 
            this.notificationDevice.AutoSize = false;
            this.notificationDevice.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.notificationDevice.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.notificationDevice.Font = new System.Drawing.Font("Gadugi", 14F);
            this.notificationDevice.Name = "notificationDevice";
            this.notificationDevice.Size = new System.Drawing.Size(308, 32);
            this.notificationDevice.Text = "Notification Device";
            // 
            // receiveStock
            // 
            this.receiveStock.AutoSize = false;
            this.receiveStock.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.receiveStock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.receiveStock.Font = new System.Drawing.Font("Gadugi", 14F);
            this.receiveStock.Name = "receiveStock";
            this.receiveStock.Size = new System.Drawing.Size(308, 32);
            this.receiveStock.Text = "Receive Stock";
            // 
            // retailInventoryLookUp
            // 
            this.retailInventoryLookUp.AutoSize = false;
            this.retailInventoryLookUp.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.retailInventoryLookUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.retailInventoryLookUp.Font = new System.Drawing.Font("Gadugi", 14F);
            this.retailInventoryLookUp.Name = "retailInventoryLookUp";
            this.retailInventoryLookUp.Size = new System.Drawing.Size(308, 32);
            this.retailInventoryLookUp.Text = "Inventory LookUp";
            // 
            // menuItemAccessControl
            // 
            this.menuItemAccessControl.AutoSize = false;
            this.menuItemAccessControl.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.menuItemAccessControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuItemAccessControl.Font = new System.Drawing.Font("Gadugi", 14F);
            this.menuItemAccessControl.Name = "menuItemAccessControl";
            this.menuItemAccessControl.Size = new System.Drawing.Size(308, 32);
            this.menuItemAccessControl.Text = "Access Control";
            this.menuItemAccessControl.Click += new System.EventHandler(this.menuItemAccessControl_Click);
            // 
            // menuTransactionFunctions
            // 
            this.menuTransactionFunctions.AutoSize = false;
            this.menuTransactionFunctions.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.menuTransactionFunctions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuTransactionFunctions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.transactionLookupMenuItem,
            this.sendTransactionReceiptMenuItem,
            this.transactionRemarksMenuItem,
            this.SignedWaiversMenuItem,
            this.checkInCheckOutUIToolStripMenuItem,
            this.subscriptionsMenuItem});
            this.menuTransactionFunctions.Font = new System.Drawing.Font("Gadugi", 14F);
            this.menuTransactionFunctions.Name = "menuTransactionFunctions";
            this.menuTransactionFunctions.Size = new System.Drawing.Size(308, 32);
            this.menuTransactionFunctions.Text = "Transaction Functions";
            // 
            // transactionLookupMenuItem
            // 
            this.transactionLookupMenuItem.AutoSize = false;
            this.transactionLookupMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.transactionLookupMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.transactionLookupMenuItem.Name = "transactionLookupMenuItem";
            this.transactionLookupMenuItem.Size = new System.Drawing.Size(314, 26);
            this.transactionLookupMenuItem.Text = "Transaction Lookup";
            this.transactionLookupMenuItem.Click += new System.EventHandler(this.transactionLookupMenuItem_Click);
            // 
            // sendTransactionReceiptMenuItem
            // 
            this.sendTransactionReceiptMenuItem.AutoSize = false;
            this.sendTransactionReceiptMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.sendTransactionReceiptMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.sendTransactionReceiptMenuItem.Name = "sendTransactionReceiptMenuItem";
            this.sendTransactionReceiptMenuItem.Size = new System.Drawing.Size(314, 26);
            this.sendTransactionReceiptMenuItem.Text = "Send Transaction Receipt";
            this.sendTransactionReceiptMenuItem.Click += new System.EventHandler(this.sendTransactionReceiptMenuItem_Click);
            // 
            // transactionRemarksMenuItem
            // 
            this.transactionRemarksMenuItem.AutoSize = false;
            this.transactionRemarksMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.transactionRemarksMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.transactionRemarksMenuItem.Name = "transactionRemarksMenuItem";
            this.transactionRemarksMenuItem.Size = new System.Drawing.Size(314, 26);
            this.transactionRemarksMenuItem.Text = "Transaction Remarks";
            this.transactionRemarksMenuItem.Click += new System.EventHandler(this.transactionRemarksMenuItem_Click);
            // 
            // SignedWaiversMenuItem
            // 
            this.SignedWaiversMenuItem.AutoSize = false;
            this.SignedWaiversMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.SignedWaiversMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SignedWaiversMenuItem.Name = "SignedWaiversMenuItem";
            this.SignedWaiversMenuItem.Size = new System.Drawing.Size(314, 26);
            this.SignedWaiversMenuItem.Text = "Signed Waivers";
            this.SignedWaiversMenuItem.Click += new System.EventHandler(this.SignedWaiversMenuItem_Click);
            // 
            // checkInCheckOutUIToolStripMenuItem
            // 
            this.checkInCheckOutUIToolStripMenuItem.AutoSize = false;
            this.checkInCheckOutUIToolStripMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.checkInCheckOutUIToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkInCheckOutUIToolStripMenuItem.Name = "checkInCheckOutUIToolStripMenuItem";
            this.checkInCheckOutUIToolStripMenuItem.Size = new System.Drawing.Size(314, 26);
            this.checkInCheckOutUIToolStripMenuItem.Text = "Check-In/Check-Out Details";
            this.checkInCheckOutUIToolStripMenuItem.Click += new System.EventHandler(this.checkInCheckOutUIToolStripMenuItem_Click);
            // 
            // subscriptionsMenuItem
            // 
            this.subscriptionsMenuItem.AutoSize = false;
            this.subscriptionsMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.subscriptionsMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.subscriptionsMenuItem.Name = "subscriptionsMenuItem";
            this.subscriptionsMenuItem.Size = new System.Drawing.Size(314, 26);
            this.subscriptionsMenuItem.Text = "Subscription Details";
            this.subscriptionsMenuItem.Click += new System.EventHandler(this.subscriptionsMenuItem_Click);
            // 
            // menuFoodDeliveryFunctions
            // 
            this.menuFoodDeliveryFunctions.AutoSize = false;
            this.menuFoodDeliveryFunctions.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.menuFoodDeliveryFunctions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuFoodDeliveryFunctions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deliveryOrderMenuItem});
            this.menuFoodDeliveryFunctions.Font = new System.Drawing.Font("Gadugi", 14F);
            this.menuFoodDeliveryFunctions.Name = "menuFoodDeliveryFunctions";
            this.menuFoodDeliveryFunctions.Size = new System.Drawing.Size(308, 32);
            this.menuFoodDeliveryFunctions.Text = "Food Delivery Functions";
            // 
            // deliveryOrderMenuItem
            // 
            this.deliveryOrderMenuItem.AutoSize = false;
            this.deliveryOrderMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
            this.deliveryOrderMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.deliveryOrderMenuItem.Name = "deliveryOrderMenuItem";
            this.deliveryOrderMenuItem.Size = new System.Drawing.Size(314, 26);
            this.deliveryOrderMenuItem.Text = "Delivery Orders";
            this.deliveryOrderMenuItem.Click += new System.EventHandler(this.deliveryOrderMenuItem_Click);
            // 
            // menuCompleteFDTransactions
            // 
            this.menuCompleteFDTransactions.Name = "menuCompleteFDTransactions";
            this.menuCompleteFDTransactions.Size = new System.Drawing.Size(32, 19);
            // 
            // TrxDGVContextMenu
            // 
            this.TrxDGVContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEnterRemarks,
            this.menuChangeQuantity,
            this.menuReOrder,
            this.toolStripSeparator2,
            this.menuChangePrice,
            this.menuResetPrice,
            this.menuProductModifiers,
            this.menuApplyDiscount,
            this.menuExemptTax,
            this.menuViewProductDetails});
            this.TrxDGVContextMenu.Name = "TrxDGVContextMenu";
            this.TrxDGVContextMenu.Size = new System.Drawing.Size(183, 208);
            this.TrxDGVContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.TrxDGVContextMenu_ItemClicked);
            // 
            // menuEnterRemarks
            // 
            this.menuEnterRemarks.Name = "menuEnterRemarks";
            this.menuEnterRemarks.Size = new System.Drawing.Size(182, 22);
            this.menuEnterRemarks.Text = "Enter Remarks";
            // 
            // menuChangeQuantity
            // 
            this.menuChangeQuantity.Name = "menuChangeQuantity";
            this.menuChangeQuantity.Size = new System.Drawing.Size(182, 22);
            this.menuChangeQuantity.Text = "Change Quantity";
            // 
            // menuReOrder
            // 
            this.menuReOrder.Name = "menuReOrder";
            this.menuReOrder.Size = new System.Drawing.Size(182, 22);
            this.menuReOrder.Text = "ReOrder";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(179, 6);
            // 
            // menuChangePrice
            // 
            this.menuChangePrice.Name = "menuChangePrice";
            this.menuChangePrice.Size = new System.Drawing.Size(182, 22);
            this.menuChangePrice.Text = "Change Price";
            // 
            // menuResetPrice
            // 
            this.menuResetPrice.Name = "menuResetPrice";
            this.menuResetPrice.Size = new System.Drawing.Size(182, 22);
            this.menuResetPrice.Text = "Reset Price";
            // 
            // menuProductModifiers
            // 
            this.menuProductModifiers.Name = "menuProductModifiers";
            this.menuProductModifiers.Size = new System.Drawing.Size(182, 22);
            this.menuProductModifiers.Text = "Modifiers";
            // 
            // menuApplyDiscount
            // 
            this.menuApplyDiscount.Name = "menuApplyDiscount";
            this.menuApplyDiscount.Size = new System.Drawing.Size(182, 22);
            this.menuApplyDiscount.Text = "Apply Discount";
            // 
            // menuExemptTax
            // 
            this.menuExemptTax.Name = "menuExemptTax";
            this.menuExemptTax.Size = new System.Drawing.Size(182, 22);
            this.menuExemptTax.Text = "Apply Tax Rule";
            // 
            // menuViewProductDetails
            // 
            this.menuViewProductDetails.Name = "menuViewProductDetails";
            this.menuViewProductDetails.Size = new System.Drawing.Size(182, 22);
            this.menuViewProductDetails.Text = "View Product Details";
            // 
            // ctxMenuLaunchApps
            // 
            this.ctxMenuLaunchApps.BackColor = System.Drawing.Color.PaleTurquoise;
            this.ctxMenuLaunchApps.Font = new System.Drawing.Font("Arial", 14.25F);
            this.ctxMenuLaunchApps.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.managementStudioToolStripMenuItem,
            this.reportsToolStripMenuItem,
            this.inventoryManagementToolStripMenuItem});
            this.ctxMenuLaunchApps.Name = "ctxMenuLaunchApps";
            this.ctxMenuLaunchApps.Size = new System.Drawing.Size(272, 82);
            this.ctxMenuLaunchApps.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ctxMenuLaunchApps_ItemClicked);
            // 
            // managementStudioToolStripMenuItem
            // 
            this.managementStudioToolStripMenuItem.Name = "managementStudioToolStripMenuItem";
            this.managementStudioToolStripMenuItem.Size = new System.Drawing.Size(271, 26);
            this.managementStudioToolStripMenuItem.Text = "Management Studio";
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(271, 26);
            this.reportsToolStripMenuItem.Text = "Reports";
            // 
            // inventoryManagementToolStripMenuItem
            // 
            this.inventoryManagementToolStripMenuItem.Name = "inventoryManagementToolStripMenuItem";
            this.inventoryManagementToolStripMenuItem.Size = new System.Drawing.Size(271, 26);
            this.inventoryManagementToolStripMenuItem.Text = "Inventory Management";
            // 
            // ctxPendingTrxContextMenu
            // 
            this.ctxPendingTrxContextMenu.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctxPendingTrxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectPendingTrx});
            this.ctxPendingTrxContextMenu.Name = "ctxPendingTrxContextMenu";
            this.ctxPendingTrxContextMenu.Size = new System.Drawing.Size(142, 38);
            this.ctxPendingTrxContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ctxPendingTrxContextMenu_ItemClicked);
            // 
            // SelectPendingTrx
            // 
            this.SelectPendingTrx.Name = "SelectPendingTrx";
            this.SelectPendingTrx.Size = new System.Drawing.Size(141, 34);
            this.SelectPendingTrx.Text = "Select";
            // 
            // ctxOrderContextTableMenu
            // 
            this.ctxOrderContextTableMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tblOrderCancel,
            this.tblOrderPrintKOT,
            this.tblOrderMoveToTable});
            this.ctxOrderContextTableMenu.Name = "ctxOrderContextMenu";
            this.ctxOrderContextTableMenu.Size = new System.Drawing.Size(149, 70);
            this.ctxOrderContextTableMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ctxOrderContextTableMenu_ItemClicked);
            // 
            // tblOrderCancel
            // 
            this.tblOrderCancel.Name = "tblOrderCancel";
            this.tblOrderCancel.Size = new System.Drawing.Size(148, 22);
            this.tblOrderCancel.Text = "Cancel Order";
            // 
            // tblOrderPrintKOT
            // 
            this.tblOrderPrintKOT.Name = "tblOrderPrintKOT";
            this.tblOrderPrintKOT.Size = new System.Drawing.Size(148, 22);
            this.tblOrderPrintKOT.Text = "Print KOT";
            // 
            // tblOrderMoveToTable
            // 
            this.tblOrderMoveToTable.Name = "tblOrderMoveToTable";
            this.tblOrderMoveToTable.Size = new System.Drawing.Size(148, 22);
            this.tblOrderMoveToTable.Text = "Move to Table";
            // 
            // lblAlerts
            // 
            this.lblAlerts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAlerts.AutoEllipsis = true;
            this.lblAlerts.BackColor = System.Drawing.Color.Transparent;
            this.lblAlerts.BackgroundImage = global::Parafait_POS.Properties.Resources.AlertPanelMiddle;
            this.lblAlerts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lblAlerts.CausesValidation = false;
            this.lblAlerts.FlatAppearance.BorderSize = 0;
            this.lblAlerts.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblAlerts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblAlerts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAlerts.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlerts.ForeColor = System.Drawing.Color.Black;
            this.lblAlerts.Location = new System.Drawing.Point(942, 543);
            this.lblAlerts.Name = "lblAlerts";
            this.lblAlerts.Size = new System.Drawing.Size(264, 87);
            this.lblAlerts.TabIndex = 25;
            this.lblAlerts.Text = "45 Gameplay Credits expire on 12-May-2012";
            this.lblAlerts.UseVisualStyleBackColor = false;
            // 
            // panelPOSButtons
            // 
            this.panelPOSButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPOSButtons.Controls.Add(this.btnRefreshPOS);
            this.panelPOSButtons.Controls.Add(this.btnTasks);
            this.panelPOSButtons.Controls.Add(this.btnLaunchApps);
            this.panelPOSButtons.Controls.Add(this.buttonLogout);
            this.panelPOSButtons.Controls.Add(this.btnShowNumPad);
            this.panelPOSButtons.Location = new System.Drawing.Point(942, 643);
            this.panelPOSButtons.Name = "panelPOSButtons";
            this.panelPOSButtons.Size = new System.Drawing.Size(265, 65);
            this.panelPOSButtons.TabIndex = 26;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackgroundImage = global::Parafait_POS.Properties.Resources.AlertPanelBottom;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(942, 630);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(264, 16);
            this.panel2.TabIndex = 27;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackgroundImage = global::Parafait_POS.Properties.Resources.AlertPanelTop;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Location = new System.Drawing.Point(942, 528);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(264, 15);
            this.panel4.TabIndex = 28;
            // 
            // ctxProductButtonContextMenu
            // 
            this.ctxProductButtonContextMenu.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctxProductButtonContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuApplyProductToAllCards});
            this.ctxProductButtonContextMenu.Name = "ctxProductButtonContextMenu";
            this.ctxProductButtonContextMenu.Size = new System.Drawing.Size(255, 38);
            this.ctxProductButtonContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ctxProductButtonContextMenu_ItemClicked);
            // 
            // menuApplyProductToAllCards
            // 
            this.menuApplyProductToAllCards.Name = "menuApplyProductToAllCards";
            this.menuApplyProductToAllCards.Size = new System.Drawing.Size(254, 34);
            this.menuApplyProductToAllCards.Text = "Apply To All Cards";
            // 
            // POS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1208, 733);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelPOSButtons);
            this.Controls.Add(this.lblAlerts);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.panelAmounts);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panelCardSwipe);
            this.Controls.Add(this.splitContainerPOS);
            this.Font = new System.Drawing.Font("Times New Roman", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "POS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parafait POS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Deactivate += new System.EventHandler(this.POS_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.POS_FormClosing);
            this.Load += new System.EventHandler(this.POS_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.POS_KeyPress);
            this.splitContainerPOS.Panel1.ResumeLayout(false);
            this.splitContainerPOS.Panel2.ResumeLayout(false);
            this.splitContainerPOS.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPOS)).EndInit();
            this.splitContainerPOS.ResumeLayout(false);
            this.tabControlSelection.ResumeLayout(false);
            this.tabPageProducts.ResumeLayout(false);
            this.panelProductSearch.ResumeLayout(false);
            this.panelProductSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).EndInit();
            this.tabControlProducts.ResumeLayout(false);
            this.tabPageProductGroups.ResumeLayout(false);
            this.flowLayoutPanelCardProducts.ResumeLayout(false);
            this.panelProductButtons.ResumeLayout(false);
            this.tabPageDiscounts.ResumeLayout(false);
            this.flowLayoutPanelDiscounts.ResumeLayout(false);
            this.tabPageFunctions.ResumeLayout(false);
            this.flowLayoutPanelFunctions.ResumeLayout(false);
            this.tabPageRedeem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbRedeem)).EndInit();
            this.tabPageSystem.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelSkinColor.ResumeLayout(false);
            this.panelSkinColor.PerformLayout();
            this.panelPassword.ResumeLayout(false);
            this.panelPassword.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabControlCardAction.ResumeLayout(false);
            this.tabPageTrx.ResumeLayout(false);
            this.flpTrxProfiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxSummary)).EndInit();
            this.panelTrxButtons.ResumeLayout(false);
            this.flpTrxButtons.ResumeLayout(false);
            this.flpOrderButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTransaction)).EndInit();
            this.tabPageActivities.ResumeLayout(false);
            this.tabPageActivities.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGamePlay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPurchases)).EndInit();
            this.tabPageCardInfo.ResumeLayout(false);
            this.tabPageCardInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardGames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardDiscounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCreditPlus)).EndInit();
            this.grpMisc.ResumeLayout(false);
            this.grpMisc.PerformLayout();
            this.tpBookings.ResumeLayout(false);
            this.tcAttractionBookings.ResumeLayout(false);
            this.tpAttractionBooking.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookings)).EndInit();
            this.filterBookingPanel.ResumeLayout(false);
            this.tpAttractionScedules.ResumeLayout(false);
            this.tpAttractionScedules.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttractionSchedules)).EndInit();
            this.tpReservations.ResumeLayout(false);
            this.tpReservations.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllReservations)).EndInit();
            this.tpOpenOrders.ResumeLayout(false);
            this.panelOrders.ResumeLayout(false);
            this.tcOrderView.ResumeLayout(false);
            this.tpOrderTableView.ResumeLayout(false);
            this.panelTables.ResumeLayout(false);
            this.tpOrderOrderView.ResumeLayout(false);
            this.tpOrderBookingView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookingDetails)).EndInit();
            this.tpOrderPendingTrxView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendingTransactions)).EndInit();
            this.panelProductDetails.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelCardSwipe.ResumeLayout(false);
            this.panelCardSwipe.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCardDetails)).EndInit();
            this.panelAmounts.ResumeLayout(false);
            this.panelAmounts.PerformLayout();
            this.POSTasksContextMenu.ResumeLayout(false);
            this.TrxDGVContextMenu.ResumeLayout(false);
            this.ctxMenuLaunchApps.ResumeLayout(false);
            this.ctxPendingTrxContextMenu.ResumeLayout(false);
            this.ctxOrderContextTableMenu.ResumeLayout(false);
            this.panelPOSButtons.ResumeLayout(false);
            this.ctxProductButtonContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void DataGridViewGamePlay_VisibleChanged(object sender, EventArgs e)
        {
            POSUtils.ResetDGVGamePlayRowBackColor(dataGridViewGamePlay, 0);
        }


        #endregion

        private System.Windows.Forms.TabControl tabControlSelection;
        private System.Windows.Forms.TabPage tabPageProducts;
        private System.Windows.Forms.TabPage tabPageDiscounts;
        private System.Windows.Forms.TabPage tabPageFunctions;
        private System.Windows.Forms.Button SampleButtonCardProduct;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCardProducts;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDiscounts;
        private System.Windows.Forms.Button SampleButtonDiscount;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelFunctions;
        private System.Windows.Forms.Button btnTransferCard;
        private System.Windows.Forms.Button btnTokenForCredit;
        private System.Windows.Forms.Button btnCreditForToken;
        private System.Windows.Forms.Button btnLoadTickets;
        private System.Windows.Forms.Button btnConsolidateCards;
        private System.Windows.Forms.Button btnLoadMultiple;
        private System.Windows.Forms.Button btnRealEticket;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripPOSMachine;
        private System.Windows.Forms.GroupBox panelCardSwipe;
        private System.Windows.Forms.Label labelCardNumber;
        private System.Windows.Forms.Label labelCardStatus;
        private System.Windows.Forms.DataGridView dataGridViewCardDetails;
        private System.Windows.Forms.TextBox textBoxTransactionTotal;
        private System.Windows.Forms.Label labelBalance;
        private System.Windows.Forms.Label labelTransactionTotal;
        private System.Windows.Forms.TextBox txtChangeAmount;
        private System.Windows.Forms.TextBox textBoxTendered;
        private System.Windows.Forms.Button buttonSaveTransaction;
        private System.Windows.Forms.Button buttonCancelLine;
        private System.Windows.Forms.Button btnPlaceOrder;
        private System.Windows.Forms.Button buttonCancelTransaction;
        private System.Windows.Forms.TextBox textBoxMessageLine;
        private System.Windows.Forms.ToolStripStatusLabel toolStripDateTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripPOSUser;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSiteName;
        private System.Windows.Forms.TabControl tabControlCardAction;
        private System.Windows.Forms.TabPage tabPageTrx;
        private System.Windows.Forms.TabPage tabPageActivities;
        private System.Windows.Forms.DataGridView dataGridViewPurchases;
        private System.Windows.Forms.Label labelPurchases;
        private System.Windows.Forms.DataGridView dataGridViewGamePlay;
        private System.Windows.Forms.Label labelGamePlay;
        private System.Windows.Forms.Panel panelAmounts;
        private System.Windows.Forms.DataGridView dataGridViewTransaction;
        private System.Windows.Forms.Label labelCardStatuslbl;
        private System.Windows.Forms.TextBox textBoxCustomerInfo;
        private System.Windows.Forms.TabControl tabControlProducts;
        private System.Windows.Forms.TabPage tabPageProductGroups;
        private System.Windows.Forms.ToolStripStatusLabel toolStripLoginID;
        private System.Windows.Forms.ToolStripStatusLabel toolStripRole;
        private System.Windows.Forms.TabPage tabPageSystem;
        private System.Windows.Forms.Panel panelPassword;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button buttonChangePassword;
        private System.Windows.Forms.TextBox textBoxReenterNewPassword;
        private System.Windows.Forms.TextBox textBoxNewPassword;
        private System.Windows.Forms.TextBox textBoxCurrentPassword;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Panel panelSkinColor;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button buttonSkinColorReset;
        private System.Windows.Forms.Button buttonChangeSkinColor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonReConnectCardReader;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnLoadBonus;
        private System.Windows.Forms.TabPage tabPageRedeem;
        private System.Windows.Forms.Button btnRefundCard;
        private System.Windows.Forms.Button btnNextProductGroup;
        private System.Windows.Forms.Button btnPrevProductGroup;
        private System.Windows.Forms.Button SampleButtonOtherProduct;
        private System.Windows.Forms.Label lblTabText;
        private System.Windows.Forms.Button btnPayment;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cmbPaymentMode;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TabPage tabPageCardInfo;
        private System.Windows.Forms.DataGridView dgvCardGames;
        private System.Windows.Forms.GroupBox grpMisc;
        private System.Windows.Forms.Label lblTicketMode;
        private System.Windows.Forms.Label lblTicketAllowed;
        private System.Windows.Forms.Label lblRoamingCard;
        private ZedGraph.ZedGraphControl zedGraphCardInfo;
        private System.Windows.Forms.RadioButton rdAmount;
        private System.Windows.Forms.RadioButton rdCount;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dtpGraphFrom;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.DateTimePicker dtpGraphTo;
        private System.Windows.Forms.Button btnRefreshGraph;
        private System.Windows.Forms.DataGridView dgvCardDiscounts;
        private System.Windows.Forms.Button btnApplyDiscount;
        private System.Windows.Forms.DataGridView dgvCreditPlus;
        private System.Windows.Forms.TextBox labelCardNo;
        private System.Windows.Forms.SplitContainer splitContainerPOS;
        private System.Windows.Forms.Panel panelTrxButtons;
        private System.Windows.Forms.Timer timerClock;
        private System.Windows.Forms.TextBox txtProductSearch;
        private System.Windows.Forms.Button btnProductLookup;
        private System.Windows.Forms.Panel panelProductSearch;
        private System.Windows.Forms.NumericUpDown nudQuantity;
        private System.Windows.Forms.Button btnQuantity;
        private System.Windows.Forms.TextBox txtVIPStatus;
        private System.Windows.Forms.Button buttonLogout;
        private System.Windows.Forms.Button btnShowNumPad;
        private System.Windows.Forms.Button btnRefreshPOS;
        private System.Windows.Forms.Button sampleButtonAttraction;
        private System.Windows.Forms.Button btnRedeemLoyalty;
        private System.Windows.Forms.Button btnSpecialPricing;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button btnDisplayGroupDropDown;
        private System.Windows.Forms.TabPage tabPageMyTrx;
        private System.Windows.Forms.PictureBox pbRedeem;
        private System.Windows.Forms.Button btnRedeemTicketsForBonus;
        private System.Windows.Forms.Button sampleButtonCardSaleProduct;
        private System.Windows.Forms.Button SampleButtonGameTime;
        private System.Windows.Forms.Button btnLaunchApps;
        private System.Windows.Forms.Button btnSampleProductRecharge;
        private System.Windows.Forms.Button btnGetServerCard;
        private System.Windows.Forms.Button btnSetChildSite;
        private System.Windows.Forms.Button btnTasks;
        private System.Windows.Forms.ContextMenuStrip POSTasksContextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuAddToShift;
        private System.Windows.Forms.Button btnGetMiFareDetails;
        private System.Windows.Forms.LinkLabel lnkZoomCardActivity;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.ToolStripMenuItem menuOpenCashDrawer;
        private System.Windows.Forms.DataGridView dgvTrxSummary;
        private System.Windows.Forms.Button btnConsolidatedCardActivity;
        private System.Windows.Forms.LinkLabel lnkConsolidatedView;
        private System.Windows.Forms.ToolStripMenuItem menuViewTasks;
        private System.Windows.Forms.ContextMenuStrip TrxDGVContextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuEnterRemarks;
        private System.Windows.Forms.ToolStripMenuItem menuReOrder;
        private System.Windows.Forms.ToolStripMenuItem menuChangeQuantity;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuChangePrice;
        private System.Windows.Forms.ToolStripMenuItem menuResetPrice;
        private System.Windows.Forms.ToolStripMenuItem menuProductModifiers;
        private System.Windows.Forms.LinkLabel lnkShowHideExtended;
        private System.Windows.Forms.ContextMenuStrip ctxMenuLaunchApps;
        private System.Windows.Forms.ToolStripMenuItem managementStudioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryManagementToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripMessage;
        private System.Windows.Forms.Panel panelOrders;
        private System.Windows.Forms.DataGridView dgvPendingTransactions;
        private System.Windows.Forms.ContextMenuStrip ctxPendingTrxContextMenu;
        private System.Windows.Forms.FlowLayoutPanel flpTrxProfiles;
        private System.Windows.Forms.Button btnTrxProfileDefault;
        private System.Windows.Forms.TabControl tcOrderView;
        private System.Windows.Forms.TabPage tpOrderOrderView;
        private System.Windows.Forms.TabPage tpOrderTableView;
        private System.Windows.Forms.FlowLayoutPanel flpFacilities;
        private System.Windows.Forms.Panel panelTables;
        private System.Windows.Forms.ContextMenuStrip ctxOrderContextTableMenu;
        private System.Windows.Forms.ToolStripMenuItem tblOrderCancel;
        private System.Windows.Forms.ToolStripMenuItem tblOrderPrintKOT;
        private System.Windows.Forms.ToolStripMenuItem poolKaraokeLightControlToolStripMenuItem;
        private System.Windows.Forms.DataGridViewButtonColumn ReverseGamePlay;
        private System.Windows.Forms.ToolStripMenuItem tblOrderMoveToTable;
        private System.Windows.Forms.TabPage tpBookings;
        private System.Windows.Forms.DataGridView dgvBookings;
        private System.Windows.Forms.TabControl tcAttractionBookings;
        private System.Windows.Forms.TabPage tpAttractionBooking;
        private System.Windows.Forms.TabPage tpReservations;
        private System.Windows.Forms.TabPage tpAttractionScedules;
        private System.Windows.Forms.DataGridView dgvAttractionSchedules;
        private System.Windows.Forms.DataGridView dgvAllReservations;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarViewAB;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarViewAB;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarViewAS;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarViewAS;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarViewRB;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarViewRB;
        private System.Windows.Forms.LinkLabel lnkRefreshReservations;
        private System.Windows.Forms.LinkLabel lnkNewReservation;
        private System.Windows.Forms.Button btnSampleVariableRecharge;
        private System.Windows.Forms.Button btnSampleCheckInCheckOut;
        private System.Windows.Forms.Button btnSampleCombo;
        private System.Windows.Forms.Button lblAlerts;
        private System.Windows.Forms.ToolStripMenuItem tsMenuCreditCardGatewayFunctions;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuOnlineFunctions;
        private System.Windows.Forms.ToolStripMenuItem executeTransactionToolStripMenuItem;
        private System.Windows.Forms.Button sampleButtonMainMenu;
        private System.Windows.Forms.Panel panelPOSButtons;
        private System.Windows.Forms.Button btnRefundCardTask;
        private System.Windows.Forms.ToolStripMenuItem menuStaffFunctions;
        private System.Windows.Forms.ToolStripMenuItem menuParentChildCards;
        private System.Windows.Forms.ToolStripMenuItem menuTechCard;
        private System.Windows.Forms.DataGridViewButtonColumn dcSelectBooking;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnTransferBalance;
        private System.Windows.Forms.ContextMenuStrip ctxProductButtonContextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuApplyProductToAllCards;
        private System.Windows.Forms.ToolStripMenuItem cardFunctionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deactivateCardsToolStripMenuItem;
        private System.Windows.Forms.Button btnParentChild;
        private System.Windows.Forms.CheckBox chkShowPast;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbAttractionFacility;
        private System.Windows.Forms.Button btnRescheduleAttraction;
        private System.Windows.Forms.DateTimePicker dtpAttractionDate;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtBalanceAmount;
        private System.Windows.Forms.ToolStripMenuItem lockerFunctionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gameManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemFiscalPrinterReports;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtTipAmount;
        private System.Windows.Forms.ToolStripMenuItem lockerLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lockerUtilityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuVariableCashRefund;
        private System.Windows.Forms.Button SampleButtonVoucher;
        private System.Windows.Forms.LinkLabel lnkPrintCardActivity;
        private System.Windows.Forms.ToolStripMenuItem masterCardsToolStripMenuItem;
        private System.Windows.Forms.TabPage tpOrderBookingView;
        private System.Windows.Forms.TabPage tpOrderPendingTrxView;
        private System.Windows.Forms.DataGridView dgvBookingDetails;
        private System.Windows.Forms.TabPage tpOpenOrders;
        private System.Windows.Forms.Button btnCloseOrderPanel;
        private System.Windows.Forms.ToolStripMenuItem SelectPendingTrx;
        private System.Windows.Forms.DataGridViewImageColumn dcBtnCardActivityTrxPrint;
        private System.Windows.Forms.Button btnRegisterCustomer;
        private System.Windows.Forms.Button btnViewOrders;
        private System.Windows.Forms.DataGridViewButtonColumn dcPendingTrxRightClick;
        private System.Windows.Forms.FlowLayoutPanel flpOrderButtons;
        private System.Windows.Forms.FlowLayoutPanel flpTrxButtons;
        private System.Windows.Forms.Button btnSalesReturnExchange;
        private System.Windows.Forms.Button btnRedeemCoupon;
        private System.Windows.Forms.ToolStripMenuItem menuCompleteFDTransactions;
        private System.Windows.Forms.ToolStripMenuItem menuNewCompleteFDTransactions;
        private System.Windows.Forms.Button btnCardSearch;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.TextBox TxtCardNumber;
        private System.Windows.Forms.ToolStripMenuItem achievements;
        private System.Windows.Forms.Button btnRedeemBonusForTicket;
        private System.Windows.Forms.ToolStripMenuItem tokenCardInventoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem couponStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripLANStatus;
        private System.Windows.Forms.ToolStripMenuItem menuApplyDiscount;
        private System.Windows.Forms.ToolStripMenuItem menuExemptTax;
        private System.Windows.Forms.DataGridViewTextBoxColumn Product_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Card_Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn Product_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Line_Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Line_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttractionDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrxProfileId;
        private System.Windows.Forms.ToolStripMenuItem menuRunReport;
        private System.Windows.Forms.Button btnPauseTime;
        private System.Windows.Forms.Button btnConvertPointsToTime;
        private System.Windows.Forms.Button btnConvertTimeToPoints;
        private System.Windows.Forms.ToolStripMenuItem notificationDevice;
        private System.Windows.Forms.ToolStripMenuItem receiveStock;
        private System.Windows.Forms.ToolStripMenuItem retailInventoryLookUp;
        private System.Windows.Forms.ToolStripMenuItem cashdrawerAssignmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuFlagVoucher;
        private System.Windows.Forms.ToolStripStatusLabel toolStripVersion;
        private System.Windows.Forms.ToolStripMenuItem menuItemAccessControl;
        private System.Windows.Forms.Button btnCardBalancePrint;
        private System.Windows.Forms.ToolStripMenuItem attendanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timeSheetDetailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enterMessageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeLoginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lockScreenToolStripMenuItem;
        private OrderListView orderListView;
        private System.Windows.Forms.ToolStripMenuItem changeStaff;
        private System.Windows.Forms.Integration.ElementHost tblPanelTables;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView tableLayoutVerticalScrollBarView;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView tableLayoutHorizontalScrollBarView;

        private System.Windows.Forms.Button btnProductDetails;
        private System.Windows.Forms.Button btnCloseProductDetails;
        private System.Windows.Forms.Panel panelProductButtons;
        private System.Windows.Forms.Panel panelProductDetails;
        private System.Windows.Forms.ToolStripMenuItem menuViewProductDetails;
        private System.Windows.Forms.ToolStripMenuItem remoteShiftOpenCloseToolStripMenuItem;
        private ToolStripMenuItem productAvailabilityToolStripMenuItem;
        private ToolStripMenuItem menuTransactionFunctions;
        private ToolStripMenuItem menuFoodDeliveryFunctions;
        private ToolStripMenuItem transactionLookupMenuItem;
        private ToolStripMenuItem sendTransactionReceiptMenuItem;
        private ToolStripMenuItem deliveryOrderMenuItem;
        private ToolStripMenuItem transactionRemarksMenuItem;
        private ToolStripMenuItem SignedWaiversMenuItem;
        private ToolStripMenuItem subscriptionsMenuItem;
        private ToolStripMenuItem checkInCheckOutUIToolStripMenuItem;
        private ToolStripMenuItem legacyTransferToolStripMenuItem;

        private System.Windows.Forms.FlowLayoutPanel filterBookingPanel;
        private System.Windows.Forms.RadioButton rdBookingToday;
        private System.Windows.Forms.RadioButton rdBookingPast;
        private System.Windows.Forms.RadioButton rdBookingFuture3;
        private System.Windows.Forms.RadioButton rdBookingFutureAll;
        private System.Windows.Forms.Button btnSearchBookingAttraction;
        private Button btnVariableRefund;
        private Button btnSendPaymentLink;
        private ToolStripMenuItem fpEnrollDeactiveToolStripMenuItem;
        private Button btnholdEntitilements;
        private Button btnRedeemVirtualPoint;
    }
}

