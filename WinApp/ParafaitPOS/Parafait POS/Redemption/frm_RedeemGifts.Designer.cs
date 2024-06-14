using System;

namespace Parafait_POS
{
    partial class frm_redemption
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvTurnInCard = new System.Windows.Forms.DataGridView();
            this.cardNumberTICard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameTICard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vipTICard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eTicketsTICard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardIdTICard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerIdTICard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbTargetLocation = new System.Windows.Forms.ComboBox();
            this.txtFeedback = new System.Windows.Forms.TextBox();
            this.cmbTurninFromLocation = new System.Windows.Forms.ComboBox();
            this.dgvSelectedProducts = new System.Windows.Forms.DataGridView();
            this.dcSelectedProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcSelecctedProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcSelectedProductDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcSelectedQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblScreenNumber = new System.Windows.Forms.Label();
            this.txtTotalTickets = new System.Windows.Forms.Label();
            this.lnkTotalTickets = new System.Windows.Forms.LinkLabel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblLoginId = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnProductSearch = new System.Windows.Forms.Button();
            this.btnTurnInKeyPad = new System.Windows.Forms.Button();
            this.btnPrintTurnIn = new System.Windows.Forms.Button();
            this.btnTurnInSave = new System.Windows.Forms.Button();
            this.btnTurnInClear = new System.Windows.Forms.Button();
            this.pnlLocations = new System.Windows.Forms.Panel();
            this.pnlProductLookup = new System.Windows.Forms.Panel();
            this.gb_search = new System.Windows.Forms.GroupBox();
            this.btnTurnInSearchKeyPad = new System.Windows.Forms.Button();
            this.btnAddTurnInProduct = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.nudQuantity = new System.Windows.Forms.NumericUpDown();
            this.btnExitProductLookup = new System.Windows.Forms.Button();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.txtTurnInProdCode = new System.Windows.Forms.TextBox();
            this.btnTurnInProductSearch = new System.Windows.Forms.Button();
            this.lbl_prodcode = new System.Windows.Forms.Label();
            this.lbl_proddesc = new System.Windows.Forms.Label();
            this.txtTurnInProdDesc = new System.Windows.Forms.TextBox();
            this.dgvTurnInProducts = new System.Windows.Forms.DataGridView();
            this.selectGift = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tcRedemption = new System.Windows.Forms.TabControl();
            this.tpViewRedemptions = new System.Windows.Forms.TabPage();
            this.btnKeyPad = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblRedemptionHeader = new System.Windows.Forms.Label();
            this.lblRedemptionId = new System.Windows.Forms.Label();
            this.txtRedemptionId = new System.Windows.Forms.TextBox();
            this.cmbRedemptionStatus = new System.Windows.Forms.ComboBox();
            this.dtpRedeemedToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpRedeemedFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblRedeemedFrom = new System.Windows.Forms.Label();
            this.lblRedeemedTo = new System.Windows.Forms.Label();
            this.lblRedemptionIdNo = new System.Windows.Forms.Label();
            this.txtRedemptionNo = new System.Windows.Forms.TextBox();
            this.lblRedemptionStatus = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtProdInfo = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSearchRedemptions = new System.Windows.Forms.Button();
            this.txtRedemptionCard = new System.Windows.Forms.TextBox();
            this.sCViewRedemption = new System.Windows.Forms.SplitContainer();
            this.dgvRedemptionHeader = new System.Windows.Forms.DataGridView();
            this.graceTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redemptionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redemptionOrderNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redeemedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RedemptionStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.primaryCardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderCompletedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderDeliveredDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manualTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origRedemptionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiptTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redemptionDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnPrintRedemption = new System.Windows.Forms.Button();
            this.btnEditRedemption = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnReverseRedemption = new System.Windows.Forms.Button();
            this.dgvRedemptions = new System.Windows.Forms.DataGridView();
            this.redemptionGiftsId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReverseGiftLine = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.giftCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.graceTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.originalPriceInTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GiftLineIsReversed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.redemptionGiftsListDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tpTurnInGift = new System.Windows.Forms.TabPage();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn27 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn28 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn29 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn30 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn31 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn32 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn33 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn34 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn35 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn36 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn37 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn38 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn39 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn40 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn41 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn42 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn43 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn44 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn45 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnInCard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedProducts)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.pnlLocations.SuspendLayout();
            this.pnlProductLookup.SuspendLayout();
            this.gb_search.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnInProducts)).BeginInit();
            this.tcRedemption.SuspendLayout();
            this.tpViewRedemptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sCViewRedemption)).BeginInit();
            this.sCViewRedemption.Panel1.SuspendLayout();
            this.sCViewRedemption.Panel2.SuspendLayout();
            this.sCViewRedemption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptionHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionGiftsListDTOBindingSource)).BeginInit();
            this.tpTurnInGift.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTurnInCard
            // 
            this.dgvTurnInCard.AllowUserToAddRows = false;
            this.dgvTurnInCard.AllowUserToDeleteRows = false;
            this.dgvTurnInCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTurnInCard.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTurnInCard.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.dgvTurnInCard.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTurnInCard.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTurnInCard.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTurnInCard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTurnInCard.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cardNumberTICard,
            this.nameTICard,
            this.vipTICard,
            this.eTicketsTICard,
            this.cardIdTICard,
            this.customerIdTICard});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTurnInCard.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTurnInCard.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvTurnInCard.EnableHeadersVisualStyles = false;
            this.dgvTurnInCard.GridColor = System.Drawing.Color.White;
            this.dgvTurnInCard.Location = new System.Drawing.Point(3, 272);
            this.dgvTurnInCard.MultiSelect = false;
            this.dgvTurnInCard.Name = "dgvTurnInCard";
            this.dgvTurnInCard.ReadOnly = true;
            this.dgvTurnInCard.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTurnInCard.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTurnInCard.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTurnInCard.Size = new System.Drawing.Size(1206, 114);
            this.dgvTurnInCard.TabIndex = 33;
            // 
            // cardNumberTICard
            // 
            this.cardNumberTICard.HeaderText = "Card #";
            this.cardNumberTICard.Name = "cardNumberTICard";
            this.cardNumberTICard.ReadOnly = true;
            // 
            // nameTICard
            // 
            this.nameTICard.HeaderText = "Name";
            this.nameTICard.Name = "nameTICard";
            this.nameTICard.ReadOnly = true;
            // 
            // vipTICard
            // 
            this.vipTICard.HeaderText = "VIP?";
            this.vipTICard.Name = "vipTICard";
            this.vipTICard.ReadOnly = true;
            // 
            // eTicketsTICard
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N0";
            this.eTicketsTICard.DefaultCellStyle = dataGridViewCellStyle2;
            this.eTicketsTICard.HeaderText = "e-$$$";
            this.eTicketsTICard.Name = "eTicketsTICard";
            this.eTicketsTICard.ReadOnly = true;
            // 
            // cardIdTICard
            // 
            this.cardIdTICard.HeaderText = "CardId";
            this.cardIdTICard.Name = "cardIdTICard";
            this.cardIdTICard.ReadOnly = true;
            this.cardIdTICard.Visible = false;
            // 
            // customerIdTICard
            // 
            this.customerIdTICard.HeaderText = "CustomerId";
            this.customerIdTICard.Name = "customerIdTICard";
            this.customerIdTICard.ReadOnly = true;
            this.customerIdTICard.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label1.Location = new System.Drawing.Point(3, 254);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 18);
            this.label1.TabIndex = 34;
            this.label1.Text = "Card";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label13.Location = new System.Drawing.Point(3, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(87, 13);
            this.label13.TabIndex = 38;
            this.label13.Text = "Turn-in Location:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label15.Location = new System.Drawing.Point(283, 11);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(85, 13);
            this.label15.TabIndex = 42;
            this.label15.Text = "Target Location:";
            // 
            // cmbTargetLocation
            // 
            this.cmbTargetLocation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.cmbTargetLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTargetLocation.FormattingEnabled = true;
            this.cmbTargetLocation.Location = new System.Drawing.Point(371, 7);
            this.cmbTargetLocation.Name = "cmbTargetLocation";
            this.cmbTargetLocation.Size = new System.Drawing.Size(192, 24);
            this.cmbTargetLocation.TabIndex = 45;
            this.cmbTargetLocation.SelectedIndexChanged += new System.EventHandler(this.cmbTargetLocation_SelectedIndexChanged);
            // 
            // txtFeedback
            // 
            this.txtFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFeedback.BackColor = System.Drawing.Color.LightGray;
            this.txtFeedback.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFeedback.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFeedback.ForeColor = System.Drawing.Color.Black;
            this.txtFeedback.Location = new System.Drawing.Point(3, 424);
            this.txtFeedback.Name = "txtFeedback";
            this.txtFeedback.ReadOnly = true;
            this.txtFeedback.Size = new System.Drawing.Size(1206, 19);
            this.txtFeedback.TabIndex = 46;
            // 
            // cmbTurninFromLocation
            // 
            this.cmbTurninFromLocation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.cmbTurninFromLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTurninFromLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTurninFromLocation.FormattingEnabled = true;
            this.cmbTurninFromLocation.Location = new System.Drawing.Point(93, 6);
            this.cmbTurninFromLocation.Name = "cmbTurninFromLocation";
            this.cmbTurninFromLocation.Size = new System.Drawing.Size(171, 24);
            this.cmbTurninFromLocation.TabIndex = 47;
            this.cmbTurninFromLocation.SelectedIndexChanged += new System.EventHandler(this.cmbTargetLocation_SelectedIndexChanged);
            // 
            // dgvSelectedProducts
            // 
            this.dgvSelectedProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSelectedProducts.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.dgvSelectedProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSelectedProducts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSelectedProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSelectedProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelectedProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcSelectedProductId,
            this.dcSelecctedProductCode,
            this.dcSelectedProductDescription,
            this.dcSelectedQuantity});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelectedProducts.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvSelectedProducts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSelectedProducts.EnableHeadersVisualStyles = false;
            this.dgvSelectedProducts.GridColor = System.Drawing.Color.White;
            this.dgvSelectedProducts.Location = new System.Drawing.Point(3, 90);
            this.dgvSelectedProducts.Name = "dgvSelectedProducts";
            this.dgvSelectedProducts.RowHeadersVisible = false;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgvSelectedProducts.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvSelectedProducts.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSelectedProducts.Size = new System.Drawing.Size(1206, 161);
            this.dgvSelectedProducts.TabIndex = 49;
            this.dgvSelectedProducts.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSelectedProducts_CellValueChanged);
            this.dgvSelectedProducts.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvRedemptionHeader_Scroll);
            this.dgvSelectedProducts.Click += new System.EventHandler(this.SearchFieldEnter);
            // 
            // dcSelectedProductId
            // 
            this.dcSelectedProductId.HeaderText = "ProductId";
            this.dcSelectedProductId.Name = "dcSelectedProductId";
            this.dcSelectedProductId.Visible = false;
            // 
            // dcSelecctedProductCode
            // 
            this.dcSelecctedProductCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            this.dcSelecctedProductCode.DefaultCellStyle = dataGridViewCellStyle6;
            this.dcSelecctedProductCode.HeaderText = "Gift";
            this.dcSelecctedProductCode.Name = "dcSelecctedProductCode";
            this.dcSelecctedProductCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dcSelectedProductDescription
            // 
            this.dcSelectedProductDescription.HeaderText = "Description";
            this.dcSelectedProductDescription.Name = "dcSelectedProductDescription";
            // 
            // dcSelectedQuantity
            // 
            this.dcSelectedQuantity.HeaderText = "Quantity";
            this.dcSelectedQuantity.Name = "dcSelectedQuantity";
            this.dcSelectedQuantity.ReadOnly = true;
            this.dcSelectedQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panelTop
            // 
            this.panelTop.BackgroundImage = global::Parafait_POS.Properties.Resources.blueGradient;
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTop.Controls.Add(this.lblScreenNumber);
            this.panelTop.Controls.Add(this.txtTotalTickets);
            this.panelTop.Controls.Add(this.lnkTotalTickets);
            this.panelTop.Controls.Add(this.panel4);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelTop.ForeColor = System.Drawing.Color.White;
            this.panelTop.Location = new System.Drawing.Point(3, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1206, 88);
            this.panelTop.TabIndex = 48;
            // 
            // lblScreenNumber
            // 
            this.lblScreenNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScreenNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblScreenNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenNumber.Location = new System.Drawing.Point(1033, 1);
            this.lblScreenNumber.Name = "lblScreenNumber";
            this.lblScreenNumber.Size = new System.Drawing.Size(53, 73);
            this.lblScreenNumber.TabIndex = 9;
            this.lblScreenNumber.Text = "0";
            this.lblScreenNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTotalTickets
            // 
            this.txtTotalTickets.BackColor = System.Drawing.Color.Transparent;
            this.txtTotalTickets.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalTickets.ForeColor = System.Drawing.Color.Black;
            this.txtTotalTickets.Location = new System.Drawing.Point(133, 4);
            this.txtTotalTickets.Name = "txtTotalTickets";
            this.txtTotalTickets.Size = new System.Drawing.Size(86, 23);
            this.txtTotalTickets.TabIndex = 4;
            this.txtTotalTickets.Text = "0";
            this.txtTotalTickets.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lnkTotalTickets
            // 
            this.lnkTotalTickets.BackColor = System.Drawing.Color.Transparent;
            this.lnkTotalTickets.DisabledLinkColor = System.Drawing.Color.Black;
            this.lnkTotalTickets.Enabled = false;
            this.lnkTotalTickets.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkTotalTickets.LinkColor = System.Drawing.Color.Black;
            this.lnkTotalTickets.Location = new System.Drawing.Point(4, 4);
            this.lnkTotalTickets.Name = "lnkTotalTickets";
            this.lnkTotalTickets.Size = new System.Drawing.Size(123, 23);
            this.lnkTotalTickets.TabIndex = 1;
            this.lnkTotalTickets.TabStop = true;
            this.lnkTotalTickets.Text = "Total Tickets:";
            this.lnkTotalTickets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.lblLoginId);
            this.panel4.Location = new System.Drawing.Point(1090, -1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(115, 28);
            this.panel4.TabIndex = 0;
            // 
            // lblLoginId
            // 
            this.lblLoginId.BackColor = System.Drawing.Color.Gray;
            this.lblLoginId.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLoginId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginId.ForeColor = System.Drawing.Color.White;
            this.lblLoginId.Location = new System.Drawing.Point(0, 0);
            this.lblLoginId.Name = "lblLoginId";
            this.lblLoginId.Size = new System.Drawing.Size(113, 24);
            this.lblLoginId.TabIndex = 4;
            this.lblLoginId.Text = "loginId";
            this.lblLoginId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.panelButtons.Controls.Add(this.btnClose);
            this.panelButtons.Controls.Add(this.btnProductSearch);
            this.panelButtons.Controls.Add(this.btnTurnInKeyPad);
            this.panelButtons.Controls.Add(this.btnPrintTurnIn);
            this.panelButtons.Controls.Add(this.btnTurnInSave);
            this.panelButtons.Controls.Add(this.btnTurnInClear);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(3, 447);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1206, 57);
            this.panelButtons.TabIndex = 50;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.CancelLine;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1128, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 45);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Exit";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnProductSearch
            // 
            this.btnProductSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.Product_Search_Btn_Normal;
            this.btnProductSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProductSearch.FlatAppearance.BorderSize = 0;
            this.btnProductSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProductSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProductSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnProductSearch.ForeColor = System.Drawing.Color.White;
            this.btnProductSearch.Location = new System.Drawing.Point(213, 5);
            this.btnProductSearch.Name = "btnProductSearch";
            this.btnProductSearch.Size = new System.Drawing.Size(70, 45);
            this.btnProductSearch.TabIndex = 11;
            this.btnProductSearch.Text = "Gift";
            this.btnProductSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnProductSearch.UseVisualStyleBackColor = true;
            this.btnProductSearch.Click += new System.EventHandler(this.btnProductSearch_Click);
            this.btnProductSearch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnProductSearch_MouseDown);
            this.btnProductSearch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnProductSearch_MouseUp);
            // 
            // btnTurnInKeyPad
            // 
            this.btnTurnInKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTurnInKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnTurnInKeyPad.FlatAppearance.BorderSize = 0;
            this.btnTurnInKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTurnInKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTurnInKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnInKeyPad.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnTurnInKeyPad.Location = new System.Drawing.Point(295, 5);
            this.btnTurnInKeyPad.Name = "btnTurnInKeyPad";
            this.btnTurnInKeyPad.Size = new System.Drawing.Size(58, 47);
            this.btnTurnInKeyPad.TabIndex = 101;
            this.btnTurnInKeyPad.UseVisualStyleBackColor = false;
            // 
            // btnPrintTurnIn
            // 
            this.btnPrintTurnIn.BackgroundImage = global::Parafait_POS.Properties.Resources.PrintTrx;
            this.btnPrintTurnIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrintTurnIn.FlatAppearance.BorderSize = 0;
            this.btnPrintTurnIn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintTurnIn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintTurnIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintTurnIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintTurnIn.ForeColor = System.Drawing.Color.White;
            this.btnPrintTurnIn.Location = new System.Drawing.Point(143, 5);
            this.btnPrintTurnIn.Name = "btnPrintTurnIn";
            this.btnPrintTurnIn.Size = new System.Drawing.Size(70, 45);
            this.btnPrintTurnIn.TabIndex = 7;
            this.btnPrintTurnIn.Text = "Print";
            this.btnPrintTurnIn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrintTurnIn.UseVisualStyleBackColor = true;
            this.btnPrintTurnIn.Click += new System.EventHandler(this.btnPrintTurnIn_Click);
            this.btnPrintTurnIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPrintTurnIn_MouseDown);
            this.btnPrintTurnIn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPrintTurnIn_MouseUp);
            // 
            // btnTurnInSave
            // 
            this.btnTurnInSave.BackgroundImage = global::Parafait_POS.Properties.Resources.CompleteTrx;
            this.btnTurnInSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTurnInSave.FlatAppearance.BorderSize = 0;
            this.btnTurnInSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTurnInSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTurnInSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnInSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurnInSave.ForeColor = System.Drawing.Color.White;
            this.btnTurnInSave.Location = new System.Drawing.Point(73, 5);
            this.btnTurnInSave.Name = "btnTurnInSave";
            this.btnTurnInSave.Size = new System.Drawing.Size(70, 45);
            this.btnTurnInSave.TabIndex = 3;
            this.btnTurnInSave.Text = "Save";
            this.btnTurnInSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTurnInSave.UseVisualStyleBackColor = true;
            this.btnTurnInSave.Click += new System.EventHandler(this.btnTurnInSave_Click);
            this.btnTurnInSave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnInSave_MouseDown);
            this.btnTurnInSave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnInSave_MouseUp);
            // 
            // btnTurnInClear
            // 
            this.btnTurnInClear.BackgroundImage = global::Parafait_POS.Properties.Resources.ClearTrx;
            this.btnTurnInClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTurnInClear.FlatAppearance.BorderSize = 0;
            this.btnTurnInClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTurnInClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTurnInClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnInClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurnInClear.ForeColor = System.Drawing.Color.White;
            this.btnTurnInClear.Location = new System.Drawing.Point(3, 5);
            this.btnTurnInClear.Name = "btnTurnInClear";
            this.btnTurnInClear.Size = new System.Drawing.Size(70, 45);
            this.btnTurnInClear.TabIndex = 0;
            this.btnTurnInClear.Text = "Clear";
            this.btnTurnInClear.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTurnInClear.UseVisualStyleBackColor = true;
            this.btnTurnInClear.Click += new System.EventHandler(this.btnTurnInClear_Click);
            this.btnTurnInClear.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnInClear_MouseDown);
            this.btnTurnInClear.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnInClear_MouseUp);
            // 
            // pnlLocations
            // 
            this.pnlLocations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLocations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.pnlLocations.Controls.Add(this.label13);
            this.pnlLocations.Controls.Add(this.cmbTurninFromLocation);
            this.pnlLocations.Controls.Add(this.label15);
            this.pnlLocations.Controls.Add(this.cmbTargetLocation);
            this.pnlLocations.Location = new System.Drawing.Point(3, 386);
            this.pnlLocations.Name = "pnlLocations";
            this.pnlLocations.Size = new System.Drawing.Size(1206, 35);
            this.pnlLocations.TabIndex = 51;
            // 
            // pnlProductLookup
            // 
            this.pnlProductLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProductLookup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.pnlProductLookup.Controls.Add(this.gb_search);
            this.pnlProductLookup.Location = new System.Drawing.Point(3, 90);
            this.pnlProductLookup.Name = "pnlProductLookup";
            this.pnlProductLookup.Size = new System.Drawing.Size(1206, 414);
            this.pnlProductLookup.TabIndex = 52;
            this.pnlProductLookup.Visible = false;
            // 
            // gb_search
            // 
            this.gb_search.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_search.BackColor = System.Drawing.Color.Transparent;
            this.gb_search.Controls.Add(this.btnTurnInSearchKeyPad);
            this.gb_search.Controls.Add(this.btnAddTurnInProduct);
            this.gb_search.Controls.Add(this.label14);
            this.gb_search.Controls.Add(this.nudQuantity);
            this.gb_search.Controls.Add(this.btnExitProductLookup);
            this.gb_search.Controls.Add(this.btnClearSearch);
            this.gb_search.Controls.Add(this.txtTurnInProdCode);
            this.gb_search.Controls.Add(this.btnTurnInProductSearch);
            this.gb_search.Controls.Add(this.lbl_prodcode);
            this.gb_search.Controls.Add(this.lbl_proddesc);
            this.gb_search.Controls.Add(this.txtTurnInProdDesc);
            this.gb_search.Controls.Add(this.dgvTurnInProducts);
            this.gb_search.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.gb_search.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gb_search.Location = new System.Drawing.Point(9, 5);
            this.gb_search.Name = "gb_search";
            this.gb_search.Size = new System.Drawing.Size(1189, 406);
            this.gb_search.TabIndex = 31;
            this.gb_search.TabStop = false;
            this.gb_search.Text = "Search Gifts";
            // 
            // btnTurnInSearchKeyPad
            // 
            this.btnTurnInSearchKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnTurnInSearchKeyPad.FlatAppearance.BorderSize = 0;
            this.btnTurnInSearchKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTurnInSearchKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTurnInSearchKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnInSearchKeyPad.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnTurnInSearchKeyPad.Location = new System.Drawing.Point(334, 62);
            this.btnTurnInSearchKeyPad.Name = "btnTurnInSearchKeyPad";
            this.btnTurnInSearchKeyPad.Size = new System.Drawing.Size(58, 47);
            this.btnTurnInSearchKeyPad.TabIndex = 100;
            this.btnTurnInSearchKeyPad.UseVisualStyleBackColor = false;
            // 
            // btnAddTurnInProduct
            // 
            this.btnAddTurnInProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddTurnInProduct.BackgroundImage = global::Parafait_POS.Properties.Resources.Add_Btn_Normal;
            this.btnAddTurnInProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddTurnInProduct.FlatAppearance.BorderSize = 0;
            this.btnAddTurnInProduct.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAddTurnInProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddTurnInProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddTurnInProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTurnInProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTurnInProduct.ForeColor = System.Drawing.Color.White;
            this.btnAddTurnInProduct.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddTurnInProduct.Location = new System.Drawing.Point(116, 362);
            this.btnAddTurnInProduct.Name = "btnAddTurnInProduct";
            this.btnAddTurnInProduct.Size = new System.Drawing.Size(70, 45);
            this.btnAddTurnInProduct.TabIndex = 44;
            this.btnAddTurnInProduct.Text = "Add";
            this.btnAddTurnInProduct.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddTurnInProduct.UseVisualStyleBackColor = true;
            this.btnAddTurnInProduct.Click += new System.EventHandler(this.btnAddTurnInProduct_Click);
            this.btnAddTurnInProduct.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAddTurnInProduct_MouseDown);
            this.btnAddTurnInProduct.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAddTurnInProduct_MouseUp);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(9, 367);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 13);
            this.label14.TabIndex = 43;
            this.label14.Text = "Quantity:";
            // 
            // nudQuantity
            // 
            this.nudQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudQuantity.Location = new System.Drawing.Point(60, 365);
            this.nudQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQuantity.Name = "nudQuantity";
            this.nudQuantity.Size = new System.Drawing.Size(50, 20);
            this.nudQuantity.TabIndex = 42;
            this.nudQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnExitProductLookup
            // 
            this.btnExitProductLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExitProductLookup.BackgroundImage = global::Parafait_POS.Properties.Resources.CancelLine;
            this.btnExitProductLookup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExitProductLookup.FlatAppearance.BorderSize = 0;
            this.btnExitProductLookup.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExitProductLookup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExitProductLookup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExitProductLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExitProductLookup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnExitProductLookup.ForeColor = System.Drawing.Color.White;
            this.btnExitProductLookup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExitProductLookup.Location = new System.Drawing.Point(1095, 362);
            this.btnExitProductLookup.Name = "btnExitProductLookup";
            this.btnExitProductLookup.Size = new System.Drawing.Size(70, 45);
            this.btnExitProductLookup.TabIndex = 15;
            this.btnExitProductLookup.Text = "Exit";
            this.btnExitProductLookup.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExitProductLookup.UseVisualStyleBackColor = true;
            this.btnExitProductLookup.Click += new System.EventHandler(this.btnExitProductLookup_Click);
            this.btnExitProductLookup.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnExitProductLookup_MouseDown);
            this.btnExitProductLookup.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnExitProductLookup_MouseUp);
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.ClearTrx;
            this.btnClearSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClearSearch.FlatAppearance.BorderSize = 0;
            this.btnClearSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnClearSearch.ForeColor = System.Drawing.Color.White;
            this.btnClearSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearSearch.Location = new System.Drawing.Point(232, 64);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(70, 45);
            this.btnClearSearch.TabIndex = 13;
            this.btnClearSearch.Text = "Clear";
            this.btnClearSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClearSearch.UseVisualStyleBackColor = true;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            this.btnClearSearch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnClearSearch_MouseDown);
            this.btnClearSearch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnClearSearch_MouseUp);
            // 
            // txtTurnInProdCode
            // 
            this.txtTurnInProdCode.Location = new System.Drawing.Point(128, 14);
            this.txtTurnInProdCode.Name = "txtTurnInProdCode";
            this.txtTurnInProdCode.Size = new System.Drawing.Size(154, 20);
            this.txtTurnInProdCode.TabIndex = 1;
            this.txtTurnInProdCode.Enter += new System.EventHandler(this.SearchFieldEnter);
            this.txtTurnInProdCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            // 
            // btnTurnInProductSearch
            // 
            this.btnTurnInProductSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.Search_Btn_Normal;
            this.btnTurnInProductSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTurnInProductSearch.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnTurnInProductSearch.FlatAppearance.BorderSize = 0;
            this.btnTurnInProductSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTurnInProductSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTurnInProductSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTurnInProductSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnInProductSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnTurnInProductSearch.ForeColor = System.Drawing.Color.White;
            this.btnTurnInProductSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTurnInProductSearch.Location = new System.Drawing.Point(124, 64);
            this.btnTurnInProductSearch.Name = "btnTurnInProductSearch";
            this.btnTurnInProductSearch.Size = new System.Drawing.Size(70, 45);
            this.btnTurnInProductSearch.TabIndex = 4;
            this.btnTurnInProductSearch.Text = "Search";
            this.btnTurnInProductSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTurnInProductSearch.UseVisualStyleBackColor = true;
            this.btnTurnInProductSearch.Click += new System.EventHandler(this.btnTurnInProductSearch_Click);
            this.btnTurnInProductSearch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnInProductSearch_MouseDown);
            this.btnTurnInProductSearch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnInProductSearch_MouseUp);
            // 
            // lbl_prodcode
            // 
            this.lbl_prodcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbl_prodcode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_prodcode.Location = new System.Drawing.Point(17, 19);
            this.lbl_prodcode.Name = "lbl_prodcode";
            this.lbl_prodcode.Size = new System.Drawing.Size(104, 13);
            this.lbl_prodcode.TabIndex = 0;
            this.lbl_prodcode.Text = "Prod. Code:";
            this.lbl_prodcode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_proddesc
            // 
            this.lbl_proddesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbl_proddesc.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_proddesc.Location = new System.Drawing.Point(17, 41);
            this.lbl_proddesc.Name = "lbl_proddesc";
            this.lbl_proddesc.Size = new System.Drawing.Size(104, 13);
            this.lbl_proddesc.TabIndex = 2;
            this.lbl_proddesc.Text = "Description:";
            this.lbl_proddesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTurnInProdDesc
            // 
            this.txtTurnInProdDesc.Location = new System.Drawing.Point(128, 36);
            this.txtTurnInProdDesc.Name = "txtTurnInProdDesc";
            this.txtTurnInProdDesc.Size = new System.Drawing.Size(154, 20);
            this.txtTurnInProdDesc.TabIndex = 3;
            this.txtTurnInProdDesc.Enter += new System.EventHandler(this.SearchFieldEnter);
            this.txtTurnInProdDesc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            // 
            // dgvTurnInProducts
            // 
            this.dgvTurnInProducts.AllowUserToAddRows = false;
            this.dgvTurnInProducts.AllowUserToDeleteRows = false;
            this.dgvTurnInProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTurnInProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTurnInProducts.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.dgvTurnInProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTurnInProducts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTurnInProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTurnInProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectGift});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTurnInProducts.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvTurnInProducts.EnableHeadersVisualStyles = false;
            this.dgvTurnInProducts.GridColor = System.Drawing.Color.White;
            this.dgvTurnInProducts.Location = new System.Drawing.Point(9, 115);
            this.dgvTurnInProducts.Name = "dgvTurnInProducts";
            this.dgvTurnInProducts.ReadOnly = true;
            this.dgvTurnInProducts.RowHeadersVisible = false;
            this.dgvTurnInProducts.RowHeadersWidth = 4;
            this.dgvTurnInProducts.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.Blue;
            this.dgvTurnInProducts.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvTurnInProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTurnInProducts.Size = new System.Drawing.Size(1174, 242);
            this.dgvTurnInProducts.TabIndex = 5;
            this.dgvTurnInProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTurnInProducts_CellClick);
            this.dgvTurnInProducts.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvRedemptions_Scroll);
            // 
            // selectGift
            // 
            this.selectGift.DataPropertyName = "selectGift";
            this.selectGift.FalseValue = "N";
            this.selectGift.HeaderText = "Select";
            this.selectGift.Name = "selectGift";
            this.selectGift.ReadOnly = true;
            this.selectGift.TrueValue = "Y";
            this.selectGift.Visible = false;
            // 
            // tcRedemption
            // 
            this.tcRedemption.Controls.Add(this.tpViewRedemptions);
            this.tcRedemption.Controls.Add(this.tpTurnInGift);
            this.tcRedemption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcRedemption.ItemSize = new System.Drawing.Size(52, 28);
            this.tcRedemption.Location = new System.Drawing.Point(0, 0);
            this.tcRedemption.Name = "tcRedemption";
            this.tcRedemption.SelectedIndex = 0;
            this.tcRedemption.Size = new System.Drawing.Size(1220, 543);
            this.tcRedemption.TabIndex = 53;
            // 
            // tpViewRedemptions
            // 
            this.tpViewRedemptions.AutoScroll = true;
            this.tpViewRedemptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.tpViewRedemptions.Controls.Add(this.btnKeyPad);
            this.tpViewRedemptions.Controls.Add(this.label2);
            this.tpViewRedemptions.Controls.Add(this.lblRedemptionHeader);
            this.tpViewRedemptions.Controls.Add(this.lblRedemptionId);
            this.tpViewRedemptions.Controls.Add(this.txtRedemptionId);
            this.tpViewRedemptions.Controls.Add(this.cmbRedemptionStatus);
            this.tpViewRedemptions.Controls.Add(this.dtpRedeemedToDate);
            this.tpViewRedemptions.Controls.Add(this.dtpRedeemedFromDate);
            this.tpViewRedemptions.Controls.Add(this.lblRedeemedFrom);
            this.tpViewRedemptions.Controls.Add(this.lblRedeemedTo);
            this.tpViewRedemptions.Controls.Add(this.lblRedemptionIdNo);
            this.tpViewRedemptions.Controls.Add(this.txtRedemptionNo);
            this.tpViewRedemptions.Controls.Add(this.lblRedemptionStatus);
            this.tpViewRedemptions.Controls.Add(this.label9);
            this.tpViewRedemptions.Controls.Add(this.txtProdInfo);
            this.tpViewRedemptions.Controls.Add(this.label8);
            this.tpViewRedemptions.Controls.Add(this.btnSearchRedemptions);
            this.tpViewRedemptions.Controls.Add(this.txtRedemptionCard);
            this.tpViewRedemptions.Controls.Add(this.sCViewRedemption);
            this.tpViewRedemptions.Location = new System.Drawing.Point(4, 32);
            this.tpViewRedemptions.Name = "tpViewRedemptions";
            this.tpViewRedemptions.Padding = new System.Windows.Forms.Padding(3);
            this.tpViewRedemptions.Size = new System.Drawing.Size(1212, 507);
            this.tpViewRedemptions.TabIndex = 1;
            this.tpViewRedemptions.Text = "View Redemptions";
            // 
            // btnKeyPad
            // 
            this.btnKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatAppearance.BorderSize = 0;
            this.btnKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyPad.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnKeyPad.Location = new System.Drawing.Point(1011, 14);
            this.btnKeyPad.Name = "btnKeyPad";
            this.btnKeyPad.Size = new System.Drawing.Size(58, 47);
            this.btnKeyPad.TabIndex = 99;
            this.btnKeyPad.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label2.Location = new System.Drawing.Point(929, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 16);
            this.label2.TabIndex = 50;
            this.label2.Text = "Redemption Gifts";
            // 
            // lblRedemptionHeader
            // 
            this.lblRedemptionHeader.AutoSize = true;
            this.lblRedemptionHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lblRedemptionHeader.Location = new System.Drawing.Point(4, 76);
            this.lblRedemptionHeader.Name = "lblRedemptionHeader";
            this.lblRedemptionHeader.Size = new System.Drawing.Size(126, 16);
            this.lblRedemptionHeader.TabIndex = 49;
            this.lblRedemptionHeader.Text = "Redemption Orders";
            // 
            // lblRedemptionId
            // 
            this.lblRedemptionId.AutoSize = true;
            this.lblRedemptionId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lblRedemptionId.Location = new System.Drawing.Point(61, 15);
            this.lblRedemptionId.Name = "lblRedemptionId";
            this.lblRedemptionId.Size = new System.Drawing.Size(24, 16);
            this.lblRedemptionId.TabIndex = 47;
            this.lblRedemptionId.Text = "ID:";
            // 
            // txtRedemptionId
            // 
            this.txtRedemptionId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRedemptionId.Location = new System.Drawing.Point(87, 9);
            this.txtRedemptionId.MinimumSize = new System.Drawing.Size(4, 28);
            this.txtRedemptionId.Name = "txtRedemptionId";
            this.txtRedemptionId.Size = new System.Drawing.Size(130, 24);
            this.txtRedemptionId.TabIndex = 20;
            this.txtRedemptionId.Enter += new System.EventHandler(this.SearchFieldEnter);
            this.txtRedemptionId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            // 
            // cmbRedemptionStatus
            // 
            this.cmbRedemptionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRedemptionStatus.FormattingEnabled = true;
            this.cmbRedemptionStatus.Location = new System.Drawing.Point(748, 10);
            this.cmbRedemptionStatus.Name = "cmbRedemptionStatus";
            this.cmbRedemptionStatus.Size = new System.Drawing.Size(121, 26);
            this.cmbRedemptionStatus.TabIndex = 23;
            this.cmbRedemptionStatus.Enter += new System.EventHandler(this.SearchFieldEnter);
            this.cmbRedemptionStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            this.cmbRedemptionStatus.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SearchFieldMouseDown);
            // 
            // dtpRedeemedToDate
            // 
            this.dtpRedeemedToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpRedeemedToDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpRedeemedToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpRedeemedToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpRedeemedToDate.Location = new System.Drawing.Point(327, 47);
            this.dtpRedeemedToDate.Name = "dtpRedeemedToDate";
            this.dtpRedeemedToDate.Size = new System.Drawing.Size(165, 24);
            this.dtpRedeemedToDate.TabIndex = 25;
            this.dtpRedeemedToDate.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SearchFieldMouseDown);
            this.dtpRedeemedToDate.Enter += new System.EventHandler(this.dtpRedeemedToDate_Enter);
            this.dtpRedeemedToDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            // 
            // dtpRedeemedFromDate
            // 
            this.dtpRedeemedFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpRedeemedFromDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpRedeemedFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpRedeemedFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpRedeemedFromDate.Location = new System.Drawing.Point(87, 47);
            this.dtpRedeemedFromDate.Name = "dtpRedeemedFromDate";
            this.dtpRedeemedFromDate.Size = new System.Drawing.Size(165, 24);
            this.dtpRedeemedFromDate.TabIndex = 24;
            this.dtpRedeemedFromDate.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SearchFieldMouseDown);
            this.dtpRedeemedFromDate.Enter += new System.EventHandler(this.dtpRedeemedFromDate_Enter);
            this.dtpRedeemedFromDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            // 
            // lblRedeemedFrom
            // 
            this.lblRedeemedFrom.AutoSize = true;
            this.lblRedeemedFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lblRedeemedFrom.Location = new System.Drawing.Point(11, 51);
            this.lblRedeemedFrom.Name = "lblRedeemedFrom";
            this.lblRedeemedFrom.Size = new System.Drawing.Size(74, 16);
            this.lblRedeemedFrom.TabIndex = 45;
            this.lblRedeemedFrom.Text = "From Date:";
            // 
            // lblRedeemedTo
            // 
            this.lblRedeemedTo.AutoSize = true;
            this.lblRedeemedTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lblRedeemedTo.Location = new System.Drawing.Point(266, 51);
            this.lblRedeemedTo.Name = "lblRedeemedTo";
            this.lblRedeemedTo.Size = new System.Drawing.Size(60, 16);
            this.lblRedeemedTo.TabIndex = 44;
            this.lblRedeemedTo.Text = "To Date:";
            // 
            // lblRedemptionIdNo
            // 
            this.lblRedemptionIdNo.AutoSize = true;
            this.lblRedemptionIdNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lblRedemptionIdNo.Location = new System.Drawing.Point(230, 15);
            this.lblRedemptionIdNo.Name = "lblRedemptionIdNo";
            this.lblRedemptionIdNo.Size = new System.Drawing.Size(96, 16);
            this.lblRedemptionIdNo.TabIndex = 43;
            this.lblRedemptionIdNo.Text = "Order Number:";
            // 
            // txtRedemptionNo
            // 
            this.txtRedemptionNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRedemptionNo.Location = new System.Drawing.Point(327, 11);
            this.txtRedemptionNo.Name = "txtRedemptionNo";
            this.txtRedemptionNo.Size = new System.Drawing.Size(130, 24);
            this.txtRedemptionNo.TabIndex = 21;
            this.txtRedemptionNo.Enter += new System.EventHandler(this.SearchFieldEnter);
            this.txtRedemptionNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            // 
            // lblRedemptionStatus
            // 
            this.lblRedemptionStatus.AutoSize = true;
            this.lblRedemptionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lblRedemptionStatus.Location = new System.Drawing.Point(699, 15);
            this.lblRedemptionStatus.Name = "lblRedemptionStatus";
            this.lblRedemptionStatus.Size = new System.Drawing.Size(48, 16);
            this.lblRedemptionStatus.TabIndex = 41;
            this.lblRedemptionStatus.Text = "Status:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label9.Location = new System.Drawing.Point(562, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(185, 16);
            this.label9.TabIndex = 36;
            this.label9.Text = "Prod Code / Desc / Bar Code:";
            // 
            // txtProdInfo
            // 
            this.txtProdInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProdInfo.Location = new System.Drawing.Point(748, 47);
            this.txtProdInfo.Name = "txtProdInfo";
            this.txtProdInfo.Size = new System.Drawing.Size(121, 24);
            this.txtProdInfo.TabIndex = 26;
            this.txtProdInfo.Enter += new System.EventHandler(this.SearchFieldEnter);
            this.txtProdInfo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label8.Location = new System.Drawing.Point(472, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 16);
            this.label8.TabIndex = 34;
            this.label8.Text = "Card Number:";
            // 
            // btnSearchRedemptions
            // 
            this.btnSearchRedemptions.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchRedemptions.BackgroundImage = global::Parafait_POS.Properties.Resources.Search_Btn_Normal;
            this.btnSearchRedemptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearchRedemptions.FlatAppearance.BorderSize = 0;
            this.btnSearchRedemptions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearchRedemptions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearchRedemptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchRedemptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnSearchRedemptions.ForeColor = System.Drawing.Color.White;
            this.btnSearchRedemptions.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearchRedemptions.Location = new System.Drawing.Point(892, 15);
            this.btnSearchRedemptions.Name = "btnSearchRedemptions";
            this.btnSearchRedemptions.Size = new System.Drawing.Size(86, 45);
            this.btnSearchRedemptions.TabIndex = 27;
            this.btnSearchRedemptions.Text = "Search";
            this.btnSearchRedemptions.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearchRedemptions.UseVisualStyleBackColor = false;
            this.btnSearchRedemptions.Click += new System.EventHandler(this.btnSearchRedemptions_Click);
            this.btnSearchRedemptions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSearchRedemptions_MouseDown);
            this.btnSearchRedemptions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSearchRedemptions_MouseUp);
            // 
            // txtRedemptionCard
            // 
            this.txtRedemptionCard.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRedemptionCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRedemptionCard.Location = new System.Drawing.Point(563, 11);
            this.txtRedemptionCard.Name = "txtRedemptionCard";
            this.txtRedemptionCard.Size = new System.Drawing.Size(121, 24);
            this.txtRedemptionCard.TabIndex = 22;
            this.txtRedemptionCard.Enter += new System.EventHandler(this.SearchFieldEnter);
            this.txtRedemptionCard.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            // 
            // sCViewRedemption
            // 
            this.sCViewRedemption.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sCViewRedemption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sCViewRedemption.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.sCViewRedemption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.sCViewRedemption.Location = new System.Drawing.Point(7, 92);
            this.sCViewRedemption.Name = "sCViewRedemption";
            // 
            // sCViewRedemption.Panel1
            // 
            this.sCViewRedemption.Panel1.Controls.Add(this.dgvRedemptionHeader);
            this.sCViewRedemption.Panel1.Controls.Add(this.btnPrintRedemption);
            this.sCViewRedemption.Panel1.Controls.Add(this.btnEditRedemption);
            this.sCViewRedemption.Panel1.Controls.Add(this.btnExit);
            this.sCViewRedemption.Panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            // 
            // sCViewRedemption.Panel2
            // 
            this.sCViewRedemption.Panel2.Controls.Add(this.btnReverseRedemption);
            this.sCViewRedemption.Panel2.Controls.Add(this.dgvRedemptions);
            this.sCViewRedemption.Panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.sCViewRedemption.Size = new System.Drawing.Size(1197, 407);
            this.sCViewRedemption.SplitterDistance = 920;
            this.sCViewRedemption.TabIndex = 48;
            // 
            // dgvRedemptionHeader
            // 
            this.dgvRedemptionHeader.AllowUserToAddRows = false;
            this.dgvRedemptionHeader.AllowUserToDeleteRows = false;
            this.dgvRedemptionHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRedemptionHeader.AutoGenerateColumns = false;
            this.dgvRedemptionHeader.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRedemptionHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.dgvRedemptionHeader.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRedemptionHeader.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvRedemptionHeader.ColumnHeadersHeight = 38;
            this.dgvRedemptionHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.graceTicketsDataGridViewTextBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.redemptionIdDataGridViewTextBoxColumn,
            this.redemptionOrderNoDataGridViewTextBoxColumn,
            this.redeemedDateDataGridViewTextBoxColumn,
            this.RedemptionStatus,
            this.primaryCardNumberDataGridViewTextBoxColumn,
            this.CustomerName,
            this.orderCompletedDateDataGridViewTextBoxColumn,
            this.orderDeliveredDateDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.manualTicketsDataGridViewTextBoxColumn,
            this.eTicketsDataGridViewTextBoxColumn,
            this.cardIdDataGridViewTextBoxColumn,
            this.origRedemptionIdDataGridViewTextBoxColumn,
            this.receiptTicketsDataGridViewTextBoxColumn,
            this.currencyTicketsDataGridViewTextBoxColumn,
            this.sourceDataGridViewTextBoxColumn});
            this.dgvRedemptionHeader.DataSource = this.redemptionDTOBindingSource;
            this.dgvRedemptionHeader.EnableHeadersVisualStyles = false;
            this.dgvRedemptionHeader.GridColor = System.Drawing.Color.White;
            this.dgvRedemptionHeader.Location = new System.Drawing.Point(0, 0);
            this.dgvRedemptionHeader.MultiSelect = false;
            this.dgvRedemptionHeader.Name = "dgvRedemptionHeader";
            this.dgvRedemptionHeader.ReadOnly = true;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgvRedemptionHeader.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvRedemptionHeader.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgvRedemptionHeader.RowTemplate.Height = 40;
            this.dgvRedemptionHeader.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRedemptionHeader.Size = new System.Drawing.Size(918, 344);
            this.dgvRedemptionHeader.TabIndex = 0;
            this.dgvRedemptionHeader.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRedemptionHeader_CellClick);
            this.dgvRedemptionHeader.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvRedemptionHeader_Scroll);
            // 
            // graceTicketsDataGridViewTextBoxColumn
            // 
            this.graceTicketsDataGridViewTextBoxColumn.DataPropertyName = "GraceTickets";
            this.graceTicketsDataGridViewTextBoxColumn.HeaderText = "Grace Tickets";
            this.graceTicketsDataGridViewTextBoxColumn.Name = "graceTicketsDataGridViewTextBoxColumn";
            this.graceTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            this.graceTicketsDataGridViewTextBoxColumn.Visible = false;
            this.graceTicketsDataGridViewTextBoxColumn.Width = 91;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdateDateDataGridViewTextBoxColumn.Visible = false;
            this.lastUpdateDateDataGridViewTextBoxColumn.Width = 110;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.creationDateDataGridViewTextBoxColumn.Visible = false;
            this.creationDateDataGridViewTextBoxColumn.Width = 94;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdByDataGridViewTextBoxColumn.Visible = false;
            this.createdByDataGridViewTextBoxColumn.Width = 81;
            // 
            // redemptionIdDataGridViewTextBoxColumn
            // 
            this.redemptionIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.redemptionIdDataGridViewTextBoxColumn.DataPropertyName = "RedemptionId";
            this.redemptionIdDataGridViewTextBoxColumn.HeaderText = "ID";
            this.redemptionIdDataGridViewTextBoxColumn.Name = "redemptionIdDataGridViewTextBoxColumn";
            this.redemptionIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.redemptionIdDataGridViewTextBoxColumn.Width = 80;
            // 
            // redemptionOrderNoDataGridViewTextBoxColumn
            // 
            this.redemptionOrderNoDataGridViewTextBoxColumn.DataPropertyName = "RedemptionOrderNo";
            this.redemptionOrderNoDataGridViewTextBoxColumn.HeaderText = "Order Number";
            this.redemptionOrderNoDataGridViewTextBoxColumn.Name = "redemptionOrderNoDataGridViewTextBoxColumn";
            this.redemptionOrderNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.redemptionOrderNoDataGridViewTextBoxColumn.Width = 89;
            // 
            // redeemedDateDataGridViewTextBoxColumn
            // 
            this.redeemedDateDataGridViewTextBoxColumn.DataPropertyName = "RedeemedDate";
            this.redeemedDateDataGridViewTextBoxColumn.HeaderText = "Redeemed Date";
            this.redeemedDateDataGridViewTextBoxColumn.Name = "redeemedDateDataGridViewTextBoxColumn";
            this.redeemedDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // RedemptionStatus
            // 
            this.RedemptionStatus.DataPropertyName = "RedemptionStatus";
            this.RedemptionStatus.HeaderText = "Status";
            this.RedemptionStatus.Name = "RedemptionStatus";
            this.RedemptionStatus.ReadOnly = true;
            this.RedemptionStatus.Width = 61;
            // 
            // primaryCardNumberDataGridViewTextBoxColumn
            // 
            this.primaryCardNumberDataGridViewTextBoxColumn.DataPropertyName = "PrimaryCardNumber";
            this.primaryCardNumberDataGridViewTextBoxColumn.HeaderText = "Primary Card#";
            this.primaryCardNumberDataGridViewTextBoxColumn.Name = "primaryCardNumberDataGridViewTextBoxColumn";
            this.primaryCardNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.primaryCardNumberDataGridViewTextBoxColumn.Width = 89;
            // 
            // CustomerName
            // 
            this.CustomerName.DataPropertyName = "CustomerName";
            this.CustomerName.HeaderText = "Customer";
            this.CustomerName.Name = "CustomerName";
            this.CustomerName.ReadOnly = true;
            this.CustomerName.Width = 75;
            // 
            // orderCompletedDateDataGridViewTextBoxColumn
            // 
            this.orderCompletedDateDataGridViewTextBoxColumn.DataPropertyName = "OrderCompletedDate";
            this.orderCompletedDateDataGridViewTextBoxColumn.HeaderText = "Prepared Date";
            this.orderCompletedDateDataGridViewTextBoxColumn.Name = "orderCompletedDateDataGridViewTextBoxColumn";
            this.orderCompletedDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.orderCompletedDateDataGridViewTextBoxColumn.Width = 92;
            // 
            // orderDeliveredDateDataGridViewTextBoxColumn
            // 
            this.orderDeliveredDateDataGridViewTextBoxColumn.DataPropertyName = "OrderDeliveredDate";
            this.orderDeliveredDateDataGridViewTextBoxColumn.HeaderText = "Delivered Date";
            this.orderDeliveredDateDataGridViewTextBoxColumn.Name = "orderDeliveredDateDataGridViewTextBoxColumn";
            this.orderDeliveredDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.orderDeliveredDateDataGridViewTextBoxColumn.Width = 94;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            this.remarksDataGridViewTextBoxColumn.ReadOnly = true;
            this.remarksDataGridViewTextBoxColumn.Width = 73;
            // 
            // manualTicketsDataGridViewTextBoxColumn
            // 
            this.manualTicketsDataGridViewTextBoxColumn.DataPropertyName = "ManualTickets";
            this.manualTicketsDataGridViewTextBoxColumn.HeaderText = "Manual Tickets";
            this.manualTicketsDataGridViewTextBoxColumn.Name = "manualTicketsDataGridViewTextBoxColumn";
            this.manualTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            this.manualTicketsDataGridViewTextBoxColumn.Visible = false;
            this.manualTicketsDataGridViewTextBoxColumn.Width = 96;
            // 
            // eTicketsDataGridViewTextBoxColumn
            // 
            this.eTicketsDataGridViewTextBoxColumn.DataPropertyName = "ETickets";
            this.eTicketsDataGridViewTextBoxColumn.HeaderText = "e-Tickets";
            this.eTicketsDataGridViewTextBoxColumn.Name = "eTicketsDataGridViewTextBoxColumn";
            this.eTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            this.eTicketsDataGridViewTextBoxColumn.Visible = false;
            this.eTicketsDataGridViewTextBoxColumn.Width = 76;
            // 
            // cardIdDataGridViewTextBoxColumn
            // 
            this.cardIdDataGridViewTextBoxColumn.DataPropertyName = "CardId";
            this.cardIdDataGridViewTextBoxColumn.HeaderText = "Card Id";
            this.cardIdDataGridViewTextBoxColumn.Name = "cardIdDataGridViewTextBoxColumn";
            this.cardIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.cardIdDataGridViewTextBoxColumn.Visible = false;
            this.cardIdDataGridViewTextBoxColumn.Width = 61;
            // 
            // origRedemptionIdDataGridViewTextBoxColumn
            // 
            this.origRedemptionIdDataGridViewTextBoxColumn.DataPropertyName = "OrigRedemptionId";
            this.origRedemptionIdDataGridViewTextBoxColumn.HeaderText = "Original Redemption Id";
            this.origRedemptionIdDataGridViewTextBoxColumn.Name = "origRedemptionIdDataGridViewTextBoxColumn";
            this.origRedemptionIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.origRedemptionIdDataGridViewTextBoxColumn.Visible = false;
            this.origRedemptionIdDataGridViewTextBoxColumn.Width = 116;
            // 
            // receiptTicketsDataGridViewTextBoxColumn
            // 
            this.receiptTicketsDataGridViewTextBoxColumn.DataPropertyName = "ReceiptTickets";
            this.receiptTicketsDataGridViewTextBoxColumn.HeaderText = "Receipt Tickets";
            this.receiptTicketsDataGridViewTextBoxColumn.Name = "receiptTicketsDataGridViewTextBoxColumn";
            this.receiptTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            this.receiptTicketsDataGridViewTextBoxColumn.Visible = false;
            this.receiptTicketsDataGridViewTextBoxColumn.Width = 98;
            // 
            // currencyTicketsDataGridViewTextBoxColumn
            // 
            this.currencyTicketsDataGridViewTextBoxColumn.DataPropertyName = "CurrencyTickets";
            this.currencyTicketsDataGridViewTextBoxColumn.HeaderText = "Currency Tickets";
            this.currencyTicketsDataGridViewTextBoxColumn.Name = "currencyTicketsDataGridViewTextBoxColumn";
            this.currencyTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            this.currencyTicketsDataGridViewTextBoxColumn.Visible = false;
            this.currencyTicketsDataGridViewTextBoxColumn.Width = 103;
            // 
            // sourceDataGridViewTextBoxColumn
            // 
            this.sourceDataGridViewTextBoxColumn.DataPropertyName = "Source";
            this.sourceDataGridViewTextBoxColumn.HeaderText = "Source";
            this.sourceDataGridViewTextBoxColumn.Name = "sourceDataGridViewTextBoxColumn";
            this.sourceDataGridViewTextBoxColumn.ReadOnly = true;
            this.sourceDataGridViewTextBoxColumn.Visible = false;
            this.sourceDataGridViewTextBoxColumn.Width = 66;
            // 
            // redemptionDTOBindingSource
            // 
            this.redemptionDTOBindingSource.DataSource = typeof(Semnox.Parafait.Redemption.RedemptionDTO);
            this.redemptionDTOBindingSource.CurrentChanged += new System.EventHandler(this.redemptionDTOBindingSource_CurrentChanged);
            // 
            // btnPrintRedemption
            // 
            this.btnPrintRedemption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrintRedemption.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintRedemption.BackgroundImage = global::Parafait_POS.Properties.Resources.PrintTrx;
            this.btnPrintRedemption.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrintRedemption.FlatAppearance.BorderSize = 0;
            this.btnPrintRedemption.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrintRedemption.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintRedemption.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintRedemption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintRedemption.ForeColor = System.Drawing.Color.White;
            this.btnPrintRedemption.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintRedemption.Location = new System.Drawing.Point(419, 350);
            this.btnPrintRedemption.Name = "btnPrintRedemption";
            this.btnPrintRedemption.Size = new System.Drawing.Size(86, 45);
            this.btnPrintRedemption.TabIndex = 37;
            this.btnPrintRedemption.Text = "Print";
            this.btnPrintRedemption.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrintRedemption.UseVisualStyleBackColor = false;
            this.btnPrintRedemption.Click += new System.EventHandler(this.btnPrintRedemption_Click);
            this.btnPrintRedemption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPrintRedemption_MouseDown);
            this.btnPrintRedemption.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPrintRedemption_MouseUp);
            // 
            // btnEditRedemption
            // 
            this.btnEditRedemption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditRedemption.BackColor = System.Drawing.Color.Transparent;
            this.btnEditRedemption.BackgroundImage = global::Parafait_POS.Properties.Resources.OrderSave;
            this.btnEditRedemption.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEditRedemption.FlatAppearance.BorderSize = 0;
            this.btnEditRedemption.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEditRedemption.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEditRedemption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditRedemption.ForeColor = System.Drawing.Color.White;
            this.btnEditRedemption.Location = new System.Drawing.Point(256, 350);
            this.btnEditRedemption.Name = "btnEditRedemption";
            this.btnEditRedemption.Size = new System.Drawing.Size(86, 45);
            this.btnEditRedemption.TabIndex = 39;
            this.btnEditRedemption.Text = "Update Status";
            this.btnEditRedemption.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEditRedemption.UseVisualStyleBackColor = false;
            this.btnEditRedemption.Click += new System.EventHandler(this.btnEditRedemption_Click);
            this.btnEditRedemption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnEditRedemption_MouseDown);
            this.btnEditRedemption.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnEditRedemption_MouseUp);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = global::Parafait_POS.Properties.Resources.CancelLine;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(93, 350);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(86, 45);
            this.btnExit.TabIndex = 30;
            this.btnExit.Text = "Exit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            this.btnExit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnExit_MouseDown);
            this.btnExit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnExit_MouseUp);
            // 
            // btnReverseRedemption
            // 
            this.btnReverseRedemption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReverseRedemption.BackColor = System.Drawing.Color.Transparent;
            this.btnReverseRedemption.BackgroundImage = global::Parafait_POS.Properties.Resources.TurnInNormal;
            this.btnReverseRedemption.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReverseRedemption.FlatAppearance.BorderSize = 0;
            this.btnReverseRedemption.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnReverseRedemption.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnReverseRedemption.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnReverseRedemption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReverseRedemption.ForeColor = System.Drawing.Color.White;
            this.btnReverseRedemption.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReverseRedemption.Location = new System.Drawing.Point(51, 350);
            this.btnReverseRedemption.Name = "btnReverseRedemption";
            this.btnReverseRedemption.Size = new System.Drawing.Size(86, 45);
            this.btnReverseRedemption.TabIndex = 31;
            this.btnReverseRedemption.Text = "Reverse";
            this.btnReverseRedemption.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReverseRedemption.UseVisualStyleBackColor = false;
            this.btnReverseRedemption.Click += new System.EventHandler(this.btnReverseRedemption_Click);
            this.btnReverseRedemption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnReverseRedemption_MouseDown);
            this.btnReverseRedemption.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnReverseRedemption_MouseUp);
            // 
            // dgvRedemptions
            // 
            this.dgvRedemptions.AllowUserToAddRows = false;
            this.dgvRedemptions.AllowUserToDeleteRows = false;
            this.dgvRedemptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRedemptions.AutoGenerateColumns = false;
            this.dgvRedemptions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRedemptions.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.dgvRedemptions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRedemptions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvRedemptions.ColumnHeadersHeight = 39;
            this.dgvRedemptions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.redemptionGiftsId,
            this.ReverseGiftLine,
            this.giftCode,
            this.tickets,
            this.productDescription,
            this.productId,
            this.locationId,
            this.graceTickets,
            this.lotId,
            this.lastUpdatedBy,
            this.lastUpdateDate,
            this.creationDate,
            this.createdBy,
            this.originalPriceInTickets,
            this.productQuantity,
            this.productName,
            this.imageFileName,
            this.GiftLineIsReversed});
            this.dgvRedemptions.DataSource = this.redemptionGiftsListDTOBindingSource;
            this.dgvRedemptions.EnableHeadersVisualStyles = false;
            this.dgvRedemptions.GridColor = System.Drawing.Color.White;
            this.dgvRedemptions.Location = new System.Drawing.Point(0, 1);
            this.dgvRedemptions.MultiSelect = false;
            this.dgvRedemptions.Name = "dgvRedemptions";
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgvRedemptions.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvRedemptions.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgvRedemptions.RowTemplate.Height = 40;
            this.dgvRedemptions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRedemptions.Size = new System.Drawing.Size(271, 344);
            this.dgvRedemptions.TabIndex = 38;
            this.dgvRedemptions.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRedemptions_CellClick);
            this.dgvRedemptions.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRedemptions_CellEnter);
            this.dgvRedemptions.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRedemptions_CellFormatting);
            this.dgvRedemptions.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvRedemptions_Scroll);
            // 
            // redemptionGiftsId
            // 
            this.redemptionGiftsId.DataPropertyName = "RedemptionGiftsId";
            this.redemptionGiftsId.HeaderText = "Id";
            this.redemptionGiftsId.Name = "redemptionGiftsId";
            this.redemptionGiftsId.ReadOnly = true;
            this.redemptionGiftsId.Visible = false;
            this.redemptionGiftsId.Width = 41;
            // 
            // ReverseGiftLine
            // 
            this.ReverseGiftLine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ReverseGiftLine.FalseValue = "false";
            this.ReverseGiftLine.HeaderText = "       ";
            this.ReverseGiftLine.Name = "ReverseGiftLine";
            this.ReverseGiftLine.TrueValue = "true";
            this.ReverseGiftLine.Width = 52;
            // 
            // giftCode
            // 
            this.giftCode.DataPropertyName = "GiftCode";
            this.giftCode.HeaderText = "Code";
            this.giftCode.Name = "giftCode";
            this.giftCode.ReadOnly = true;
            this.giftCode.Width = 56;
            // 
            // tickets
            // 
            this.tickets.DataPropertyName = "Tickets";
            this.tickets.HeaderText = "Tickets";
            this.tickets.Name = "tickets";
            this.tickets.ReadOnly = true;
            this.tickets.Width = 66;
            // 
            // productDescription
            // 
            this.productDescription.DataPropertyName = "ProductDescription";
            this.productDescription.HeaderText = "Description";
            this.productDescription.Name = "productDescription";
            this.productDescription.ReadOnly = true;
            this.productDescription.Width = 84;
            // 
            // productId
            // 
            this.productId.DataPropertyName = "ProductId";
            this.productId.HeaderText = "Product";
            this.productId.Name = "productId";
            this.productId.Visible = false;
            this.productId.Width = 69;
            // 
            // locationId
            // 
            this.locationId.DataPropertyName = "LocationId";
            this.locationId.HeaderText = "Location";
            this.locationId.Name = "locationId";
            this.locationId.Visible = false;
            this.locationId.Width = 73;
            // 
            // graceTickets
            // 
            this.graceTickets.DataPropertyName = "GraceTickets";
            this.graceTickets.HeaderText = "Grace Tickets";
            this.graceTickets.Name = "graceTickets";
            this.graceTickets.Visible = false;
            this.graceTickets.Width = 91;
            // 
            // lotId
            // 
            this.lotId.DataPropertyName = "LotId";
            this.lotId.HeaderText = "Lot";
            this.lotId.Name = "lotId";
            this.lotId.Visible = false;
            this.lotId.Width = 47;
            // 
            // lastUpdatedBy
            // 
            this.lastUpdatedBy.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedBy.HeaderText = "Last Updated By";
            this.lastUpdatedBy.Name = "lastUpdatedBy";
            this.lastUpdatedBy.Visible = false;
            this.lastUpdatedBy.Width = 91;
            // 
            // lastUpdateDate
            // 
            this.lastUpdateDate.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDate.HeaderText = "Last Update Date";
            this.lastUpdateDate.Name = "lastUpdateDate";
            this.lastUpdateDate.Visible = false;
            this.lastUpdateDate.Width = 106;
            // 
            // creationDate
            // 
            this.creationDate.DataPropertyName = "CreationDate";
            this.creationDate.HeaderText = "Creation Date";
            this.creationDate.Name = "creationDate";
            this.creationDate.Visible = false;
            this.creationDate.Width = 89;
            // 
            // createdBy
            // 
            this.createdBy.DataPropertyName = "CreatedBy";
            this.createdBy.HeaderText = "Created By";
            this.createdBy.Name = "createdBy";
            this.createdBy.Visible = false;
            this.createdBy.Width = 78;
            // 
            // originalPriceInTickets
            // 
            this.originalPriceInTickets.DataPropertyName = "OriginalPriceInTickets";
            this.originalPriceInTickets.HeaderText = "Original Price In Tickets";
            this.originalPriceInTickets.Name = "originalPriceInTickets";
            this.originalPriceInTickets.Visible = false;
            // 
            // productQuantity
            // 
            this.productQuantity.DataPropertyName = "ProductQuantity";
            this.productQuantity.HeaderText = "ProductQuantity";
            this.productQuantity.Name = "productQuantity";
            this.productQuantity.Visible = false;
            this.productQuantity.Width = 108;
            // 
            // productName
            // 
            this.productName.DataPropertyName = "ProductName";
            this.productName.HeaderText = "ProductName";
            this.productName.Name = "productName";
            this.productName.Visible = false;
            this.productName.Width = 97;
            // 
            // imageFileName
            // 
            this.imageFileName.DataPropertyName = "ImageFileName";
            this.imageFileName.HeaderText = "ImageFileName";
            this.imageFileName.Name = "imageFileName";
            this.imageFileName.Visible = false;
            this.imageFileName.Width = 105;
            // 
            // GiftLineIsReversed
            // 
            this.GiftLineIsReversed.DataPropertyName = "GiftLineIsReversed";
            this.GiftLineIsReversed.HeaderText = "GiftLineIsReversed";
            this.GiftLineIsReversed.Name = "GiftLineIsReversed";
            this.GiftLineIsReversed.ReadOnly = true;
            this.GiftLineIsReversed.Visible = false;
            this.GiftLineIsReversed.Width = 103;
            // 
            // redemptionGiftsListDTOBindingSource
            // 
            this.redemptionGiftsListDTOBindingSource.DataMember = "RedemptionGiftsListDTO";
            this.redemptionGiftsListDTOBindingSource.DataSource = this.redemptionDTOBindingSource;
            // 
            // tpTurnInGift
            // 
            this.tpTurnInGift.AutoScroll = true;
            this.tpTurnInGift.Controls.Add(this.pnlProductLookup);
            this.tpTurnInGift.Controls.Add(this.pnlLocations);
            this.tpTurnInGift.Controls.Add(this.dgvSelectedProducts);
            this.tpTurnInGift.Controls.Add(this.panelTop);
            this.tpTurnInGift.Controls.Add(this.txtFeedback);
            this.tpTurnInGift.Controls.Add(this.label1);
            this.tpTurnInGift.Controls.Add(this.dgvTurnInCard);
            this.tpTurnInGift.Controls.Add(this.panelButtons);
            this.tpTurnInGift.Location = new System.Drawing.Point(4, 32);
            this.tpTurnInGift.Name = "tpTurnInGift";
            this.tpTurnInGift.Padding = new System.Windows.Forms.Padding(3);
            this.tpTurnInGift.Size = new System.Drawing.Size(1212, 507);
            this.tpTurnInGift.TabIndex = 2;
            this.tpTurnInGift.Text = "Turn-In Gifts";
            this.tpTurnInGift.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "RedemptionId";
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "RedemptionOrderNo";
            this.dataGridViewTextBoxColumn2.HeaderText = "Order Number";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 89;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "RedeemedDate";
            this.dataGridViewTextBoxColumn3.HeaderText = "Redeemed Date";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "RedemptionStatus";
            this.dataGridViewTextBoxColumn4.HeaderText = "Status";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 61;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "PrimaryCardNumber";
            this.dataGridViewTextBoxColumn5.HeaderText = "Primary Card#";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 89;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "CustomerName";
            this.dataGridViewTextBoxColumn6.HeaderText = "Customer";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 75;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "OrderCompletedDate";
            this.dataGridViewTextBoxColumn7.HeaderText = "Prepared Date";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 92;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "OrderDeliveredDate";
            this.dataGridViewTextBoxColumn8.HeaderText = "Delivered Date";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 94;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Remarks";
            this.dataGridViewTextBoxColumn9.HeaderText = "Remarks";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 73;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "ManualTickets";
            this.dataGridViewTextBoxColumn10.HeaderText = "Manual Tickets";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Visible = false;
            this.dataGridViewTextBoxColumn10.Width = 96;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "ETickets";
            this.dataGridViewTextBoxColumn11.HeaderText = "e-Tickets";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Visible = false;
            this.dataGridViewTextBoxColumn11.Width = 76;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "CardId";
            this.dataGridViewTextBoxColumn12.HeaderText = "Card Id";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.Visible = false;
            this.dataGridViewTextBoxColumn12.Width = 61;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "OrigRedemptionId";
            this.dataGridViewTextBoxColumn13.HeaderText = "Original Redemption Id";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.Visible = false;
            this.dataGridViewTextBoxColumn13.Width = 116;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "GraceTickets";
            this.dataGridViewTextBoxColumn14.HeaderText = "Grace Tickets";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.Visible = false;
            this.dataGridViewTextBoxColumn14.Width = 91;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "ReceiptTickets";
            this.dataGridViewTextBoxColumn15.HeaderText = "Receipt Tickets";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.Visible = false;
            this.dataGridViewTextBoxColumn15.Width = 98;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "CurrencyTickets";
            this.dataGridViewTextBoxColumn16.HeaderText = "Currency Tickets";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.Visible = false;
            this.dataGridViewTextBoxColumn16.Width = 103;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.DataPropertyName = "Source";
            this.dataGridViewTextBoxColumn17.HeaderText = "Source";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.Visible = false;
            this.dataGridViewTextBoxColumn17.Width = 66;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.DataPropertyName = "LastUpdateDate";
            this.dataGridViewTextBoxColumn18.HeaderText = "LastUpdateDate";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.Visible = false;
            this.dataGridViewTextBoxColumn18.Width = 110;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.DataPropertyName = "CreationDate";
            this.dataGridViewTextBoxColumn19.HeaderText = "CreationDate";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.Visible = false;
            this.dataGridViewTextBoxColumn19.Width = 94;
            // 
            // dataGridViewTextBoxColumn20
            // 
            this.dataGridViewTextBoxColumn20.DataPropertyName = "CreatedBy";
            this.dataGridViewTextBoxColumn20.HeaderText = "CreatedBy";
            this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
            this.dataGridViewTextBoxColumn20.Visible = false;
            this.dataGridViewTextBoxColumn20.Width = 81;
            // 
            // dataGridViewTextBoxColumn21
            // 
            this.dataGridViewTextBoxColumn21.DataPropertyName = "RedemptionGiftsId";
            this.dataGridViewTextBoxColumn21.HeaderText = "Id";
            this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
            this.dataGridViewTextBoxColumn21.Visible = false;
            this.dataGridViewTextBoxColumn21.Width = 41;
            // 
            // dataGridViewTextBoxColumn22
            // 
            this.dataGridViewTextBoxColumn22.DataPropertyName = "GiftCode";
            this.dataGridViewTextBoxColumn22.HeaderText = "Code";
            this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
            this.dataGridViewTextBoxColumn22.Width = 56;
            // 
            // dataGridViewTextBoxColumn23
            // 
            this.dataGridViewTextBoxColumn23.DataPropertyName = "ProductDescription";
            this.dataGridViewTextBoxColumn23.HeaderText = "Description";
            this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
            this.dataGridViewTextBoxColumn23.Width = 84;
            // 
            // dataGridViewTextBoxColumn24
            // 
            this.dataGridViewTextBoxColumn24.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn24.HeaderText = "Product";
            this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
            this.dataGridViewTextBoxColumn24.Visible = false;
            this.dataGridViewTextBoxColumn24.Width = 69;
            // 
            // dataGridViewTextBoxColumn25
            // 
            this.dataGridViewTextBoxColumn25.DataPropertyName = "LocationId";
            this.dataGridViewTextBoxColumn25.HeaderText = "Location";
            this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
            this.dataGridViewTextBoxColumn25.Visible = false;
            this.dataGridViewTextBoxColumn25.Width = 73;
            // 
            // dataGridViewTextBoxColumn26
            // 
            this.dataGridViewTextBoxColumn26.DataPropertyName = "Tickets";
            this.dataGridViewTextBoxColumn26.HeaderText = "Tickets";
            this.dataGridViewTextBoxColumn26.Name = "dataGridViewTextBoxColumn26";
            this.dataGridViewTextBoxColumn26.Width = 66;
            // 
            // dataGridViewTextBoxColumn27
            // 
            this.dataGridViewTextBoxColumn27.DataPropertyName = "GraceTickets";
            this.dataGridViewTextBoxColumn27.HeaderText = "Grace Tickets";
            this.dataGridViewTextBoxColumn27.Name = "dataGridViewTextBoxColumn27";
            this.dataGridViewTextBoxColumn27.Visible = false;
            this.dataGridViewTextBoxColumn27.Width = 91;
            // 
            // dataGridViewTextBoxColumn28
            // 
            this.dataGridViewTextBoxColumn28.DataPropertyName = "LotId";
            this.dataGridViewTextBoxColumn28.HeaderText = "Lot";
            this.dataGridViewTextBoxColumn28.Name = "dataGridViewTextBoxColumn28";
            this.dataGridViewTextBoxColumn28.Visible = false;
            this.dataGridViewTextBoxColumn28.Width = 47;
            // 
            // dataGridViewTextBoxColumn29
            // 
            this.dataGridViewTextBoxColumn29.DataPropertyName = "LastUpdatedBy";
            this.dataGridViewTextBoxColumn29.HeaderText = "Last Updated By";
            this.dataGridViewTextBoxColumn29.Name = "dataGridViewTextBoxColumn29";
            this.dataGridViewTextBoxColumn29.Visible = false;
            this.dataGridViewTextBoxColumn29.Width = 91;
            // 
            // dataGridViewTextBoxColumn30
            // 
            this.dataGridViewTextBoxColumn30.DataPropertyName = "LastUpdateDate";
            this.dataGridViewTextBoxColumn30.HeaderText = "Last Update Date";
            this.dataGridViewTextBoxColumn30.Name = "dataGridViewTextBoxColumn30";
            this.dataGridViewTextBoxColumn30.Visible = false;
            this.dataGridViewTextBoxColumn30.Width = 106;
            // 
            // dataGridViewTextBoxColumn31
            // 
            this.dataGridViewTextBoxColumn31.DataPropertyName = "CreationDate";
            this.dataGridViewTextBoxColumn31.HeaderText = "Creation Date";
            this.dataGridViewTextBoxColumn31.Name = "dataGridViewTextBoxColumn31";
            this.dataGridViewTextBoxColumn31.Visible = false;
            this.dataGridViewTextBoxColumn31.Width = 89;
            // 
            // dataGridViewTextBoxColumn32
            // 
            this.dataGridViewTextBoxColumn32.DataPropertyName = "CreatedBy";
            this.dataGridViewTextBoxColumn32.HeaderText = "Created By";
            this.dataGridViewTextBoxColumn32.Name = "dataGridViewTextBoxColumn32";
            this.dataGridViewTextBoxColumn32.Visible = false;
            this.dataGridViewTextBoxColumn32.Width = 78;
            // 
            // dataGridViewTextBoxColumn33
            // 
            this.dataGridViewTextBoxColumn33.DataPropertyName = "OriginalPriceInTickets";
            this.dataGridViewTextBoxColumn33.HeaderText = "Original Price In Tickets";
            this.dataGridViewTextBoxColumn33.Name = "dataGridViewTextBoxColumn33";
            this.dataGridViewTextBoxColumn33.Visible = false;
            // 
            // dataGridViewTextBoxColumn34
            // 
            this.dataGridViewTextBoxColumn34.DataPropertyName = "ProductQuantity";
            this.dataGridViewTextBoxColumn34.HeaderText = "ProductQuantity";
            this.dataGridViewTextBoxColumn34.Name = "dataGridViewTextBoxColumn34";
            this.dataGridViewTextBoxColumn34.Visible = false;
            this.dataGridViewTextBoxColumn34.Width = 108;
            // 
            // dataGridViewTextBoxColumn35
            // 
            this.dataGridViewTextBoxColumn35.DataPropertyName = "ProductName";
            this.dataGridViewTextBoxColumn35.HeaderText = "ProductName";
            this.dataGridViewTextBoxColumn35.Name = "dataGridViewTextBoxColumn35";
            this.dataGridViewTextBoxColumn35.Visible = false;
            this.dataGridViewTextBoxColumn35.Width = 97;
            // 
            // dataGridViewTextBoxColumn36
            // 
            this.dataGridViewTextBoxColumn36.DataPropertyName = "ImageFileName";
            this.dataGridViewTextBoxColumn36.HeaderText = "ImageFileName";
            this.dataGridViewTextBoxColumn36.Name = "dataGridViewTextBoxColumn36";
            this.dataGridViewTextBoxColumn36.Visible = false;
            this.dataGridViewTextBoxColumn36.Width = 105;
            // 
            // dataGridViewTextBoxColumn37
            // 
            this.dataGridViewTextBoxColumn37.HeaderText = "ProductId";
            this.dataGridViewTextBoxColumn37.Name = "dataGridViewTextBoxColumn37";
            this.dataGridViewTextBoxColumn37.Visible = false;
            // 
            // dataGridViewTextBoxColumn38
            // 
            this.dataGridViewTextBoxColumn38.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewTextBoxColumn38.DefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewTextBoxColumn38.HeaderText = "Gift";
            this.dataGridViewTextBoxColumn38.Name = "dataGridViewTextBoxColumn38";
            this.dataGridViewTextBoxColumn38.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn39
            // 
            this.dataGridViewTextBoxColumn39.HeaderText = "Description";
            this.dataGridViewTextBoxColumn39.Name = "dataGridViewTextBoxColumn39";
            // 
            // dataGridViewTextBoxColumn40
            // 
            this.dataGridViewTextBoxColumn40.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn40.Name = "dataGridViewTextBoxColumn40";
            this.dataGridViewTextBoxColumn40.ReadOnly = true;
            this.dataGridViewTextBoxColumn40.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn41
            // 
            this.dataGridViewTextBoxColumn41.HeaderText = "Card #";
            this.dataGridViewTextBoxColumn41.Name = "dataGridViewTextBoxColumn41";
            this.dataGridViewTextBoxColumn41.Width = 301;
            // 
            // dataGridViewTextBoxColumn42
            // 
            this.dataGridViewTextBoxColumn42.HeaderText = "Name";
            this.dataGridViewTextBoxColumn42.Name = "dataGridViewTextBoxColumn42";
            this.dataGridViewTextBoxColumn42.Width = 302;
            // 
            // dataGridViewTextBoxColumn43
            // 
            this.dataGridViewTextBoxColumn43.HeaderText = "VIP?";
            this.dataGridViewTextBoxColumn43.Name = "dataGridViewTextBoxColumn43";
            this.dataGridViewTextBoxColumn43.Width = 301;
            // 
            // dataGridViewTextBoxColumn44
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.Format = "N0";
            this.dataGridViewTextBoxColumn44.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewTextBoxColumn44.HeaderText = "e-$$$";
            this.dataGridViewTextBoxColumn44.Name = "dataGridViewTextBoxColumn44";
            this.dataGridViewTextBoxColumn44.Width = 301;
            // 
            // dataGridViewTextBoxColumn45
            // 
            this.dataGridViewTextBoxColumn45.HeaderText = "CardId";
            this.dataGridViewTextBoxColumn45.Name = "dataGridViewTextBoxColumn45";
            this.dataGridViewTextBoxColumn45.Visible = false;
            // 
            // frm_redemption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1220, 543);
            this.Controls.Add(this.tcRedemption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frm_redemption";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Redeem Gifts";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_redemption_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_redemption_FormClosed);
            this.Load += new System.EventHandler(this.frm_redemption_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnInCard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedProducts)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.pnlLocations.ResumeLayout(false);
            this.pnlLocations.PerformLayout();
            this.pnlProductLookup.ResumeLayout(false);
            this.gb_search.ResumeLayout(false);
            this.gb_search.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnInProducts)).EndInit();
            this.tcRedemption.ResumeLayout(false);
            this.tpViewRedemptions.ResumeLayout(false);
            this.tpViewRedemptions.PerformLayout();
            this.sCViewRedemption.Panel1.ResumeLayout(false);
            this.sCViewRedemption.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sCViewRedemption)).EndInit();
            this.sCViewRedemption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptionHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionGiftsListDTOBindingSource)).EndInit();
            this.tpTurnInGift.ResumeLayout(false);
            this.tpTurnInGift.PerformLayout();
            this.ResumeLayout(false);

        }
        

        #endregion
        private System.Windows.Forms.ComboBox cmbTurninFromLocation;
        private System.Windows.Forms.TextBox txtFeedback;
        private System.Windows.Forms.ComboBox cmbTargetLocation;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvTurnInCard;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardNumberTICard;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameTICard;
        private System.Windows.Forms.DataGridViewTextBoxColumn vipTICard;
        private System.Windows.Forms.DataGridViewTextBoxColumn eTicketsTICard;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardIdTICard;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerIdTICard;
        private System.Windows.Forms.DataGridView dgvSelectedProducts;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblScreenNumber;
        private System.Windows.Forms.Label txtTotalTickets;
        private System.Windows.Forms.LinkLabel lnkTotalTickets;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblLoginId;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnProductSearch;
        private System.Windows.Forms.Button btnPrintTurnIn;
        private System.Windows.Forms.Button btnTurnInSave;
        private System.Windows.Forms.Button btnTurnInClear;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlLocations;
        private System.Windows.Forms.Panel pnlProductLookup;
        private System.Windows.Forms.GroupBox gb_search;
        private System.Windows.Forms.Button btnExitProductLookup;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.DataGridView dgvTurnInProducts;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectGift;
        private System.Windows.Forms.TextBox txtTurnInProdCode;
        private System.Windows.Forms.Button btnTurnInProductSearch;
        private System.Windows.Forms.Label lbl_prodcode;
        private System.Windows.Forms.Label lbl_proddesc;
        private System.Windows.Forms.TextBox txtTurnInProdDesc;
        private System.Windows.Forms.Button btnAddTurnInProduct;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nudQuantity;
        private System.Windows.Forms.TabControl tcRedemption;
        private System.Windows.Forms.TabPage tpViewRedemptions;
        private System.Windows.Forms.Button btnPrintRedemption;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtProdInfo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSearchRedemptions;
        private System.Windows.Forms.TextBox txtRedemptionCard;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnReverseRedemption;
        private System.Windows.Forms.TabPage tpTurnInGift;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcSelectedProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcSelecctedProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcSelectedProductDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcSelectedQuantity;
        private System.Windows.Forms.DataGridView dgvRedemptions;
        private System.Windows.Forms.Button btnEditRedemption;
        private System.Windows.Forms.Label lblRedemptionIdNo;
        private System.Windows.Forms.TextBox txtRedemptionNo;
        private System.Windows.Forms.Label lblRedemptionStatus;
        private System.Windows.Forms.Label lblRedeemedFrom;
        private System.Windows.Forms.Label lblRedeemedTo;
        private System.Windows.Forms.DateTimePicker dtpRedeemedFromDate;
        private System.Windows.Forms.DateTimePicker dtpRedeemedToDate;
        private System.Windows.Forms.ComboBox cmbRedemptionStatus;
        private System.Windows.Forms.Label lblRedemptionId;
        private System.Windows.Forms.TextBox txtRedemptionId;
        private System.Windows.Forms.SplitContainer sCViewRedemption;
        private System.Windows.Forms.DataGridView dgvRedemptionHeader;
        private System.Windows.Forms.BindingSource redemptionDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn graceTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label lblRedemptionHeader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.BindingSource redemptionGiftsListDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn26;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn27;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn28;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn29;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn30;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn31;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn32;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn33;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn34;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn35;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn36;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn37;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn38;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn39;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn40;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn41;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn42;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn43;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn44;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn45;
        private System.Windows.Forms.DataGridViewTextBoxColumn redemptionGiftsId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ReverseGiftLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn giftCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn tickets;
        private System.Windows.Forms.DataGridViewTextBoxColumn productDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn productId;
        private System.Windows.Forms.DataGridViewTextBoxColumn locationId;
        private System.Windows.Forms.DataGridViewTextBoxColumn graceTickets;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotId;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn originalPriceInTickets;
        private System.Windows.Forms.DataGridViewTextBoxColumn productQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn productName;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageFileName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn GiftLineIsReversed;
        private System.Windows.Forms.DataGridViewTextBoxColumn redemptionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn redemptionOrderNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn redeemedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RedemptionStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn primaryCardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderCompletedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderDeliveredDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn manualTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn origRedemptionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiptTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnKeyPad;
        private System.Windows.Forms.Button btnTurnInSearchKeyPad;
        private System.Windows.Forms.Button btnTurnInKeyPad;
    }
}

