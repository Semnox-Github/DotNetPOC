using System;
using System.Windows.Forms;
using Semnox.Parafait.Inventory;

namespace Parafait_POS.Redemption
{
    partial class frmScanAndRedeem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScanAndRedeem));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlProductLookup = new System.Windows.Forms.Panel();
            this.gb_search = new System.Windows.Forms.GroupBox();
            this.txtPriceInTicketLessThan = new System.Windows.Forms.TextBox();
            this.pnlProductLookupButtons = new System.Windows.Forms.Panel();
            this.btnKeyPad = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.btnExitProductLookup = new System.Windows.Forms.Button();
            this.tcProducts = new System.Windows.Forms.TabControl();
            this.tpProductButtons = new System.Windows.Forms.TabPage();
            this.flpProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSampleProduct = new System.Windows.Forms.Button();
            this.tpProductList = new System.Windows.Forms.TabPage();
            this.productInventoryInfo = new System.Windows.Forms.Panel();
            this.btnProdInvInfoClose = new System.Windows.Forms.Button();
            this.productInvDgv = new System.Windows.Forms.DataGridView();
            this.inventoryIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.locationDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timestampDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.allocatedQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivePriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isPurchasebleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sKUDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barcodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksMandatoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.massUpdateAllowedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stagingQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stagingRemarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inventoryDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.search_dgv = new System.Windows.Forms.DataGridView();
            this.selectGift = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.imgViewInventoryLocations = new System.Windows.Forms.DataGridViewImageColumn();
            this.lblPriceInTickets = new System.Windows.Forms.Label();
            this.txtPriceInTicketMoreThan = new System.Windows.Forms.TextBox();
            this.txt_prodcode = new System.Windows.Forms.TextBox();
            this.lbl_prodcode = new System.Windows.Forms.Label();
            this.lbl_proddesc = new System.Windows.Forms.Label();
            this.txt_proddesc = new System.Windows.Forms.TextBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnMoreOptions = new System.Windows.Forms.Button();
            this.btnProductSearch = new System.Windows.Forms.Button();
            this.btnTurnIn = new System.Windows.Forms.Button();
            this.btnLoadTickets = new System.Windows.Forms.Button();
            this.btnAddManual = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSuspend = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnScanTicketOrGift = new System.Windows.Forms.Button();
            this.btnFlagTicketReceipt = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.panelAddClose = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddScreen = new System.Windows.Forms.Button();
            this.dgvRedemption = new System.Windows.Forms.DataGridView();
            this.dummyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dcProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblScreenNumber = new System.Windows.Forms.Label();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblRedeemed = new System.Windows.Forms.Label();
            this.lblTotalTickets = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lnkTotalTickets = new System.Windows.Forms.LinkLabel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnShowSuspended = new System.Windows.Forms.Button();
            this.lblLoginId = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnAddTicketMenuItem = new System.Windows.Forms.Button();
            this.btnLoadTicketMenuItem = new System.Windows.Forms.Button();
            this.btnTurnInMenuItem = new System.Windows.Forms.Button();
            this.btnProductSearchMenuItem = new System.Windows.Forms.Button();
            this.btnFlagTicketReceiptMenuItem = new System.Windows.Forms.Button();
            this.btnScanTicketOrGiftMenuItem = new System.Windows.Forms.Button();
            this.btnSuspendMenuItem = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.pnlProductLookup.SuspendLayout();
            this.gb_search.SuspendLayout();
            this.pnlProductLookupButtons.SuspendLayout();
            this.tcProducts.SuspendLayout();
            this.tpProductButtons.SuspendLayout();
            this.flpProducts.SuspendLayout();
            this.tpProductList.SuspendLayout();
            this.productInventoryInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productInvDgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.search_dgv)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.panelAddClose.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemption)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pnlProductLookup);
            this.panel1.Controls.Add(this.panelButtons);
            this.panel1.Controls.Add(this.dgvRedemption);
            this.panel1.Controls.Add(this.panelTop);
            this.panel1.Controls.Add(this.lblMessage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(640, 543);
            this.panel1.TabIndex = 0;
            // 
            // pnlProductLookup
            // 
            this.pnlProductLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProductLookup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.pnlProductLookup.Controls.Add(this.gb_search);
            this.pnlProductLookup.Location = new System.Drawing.Point(0, 88);
            this.pnlProductLookup.Name = "pnlProductLookup";
            this.pnlProductLookup.Size = new System.Drawing.Size(640, 427);
            this.pnlProductLookup.TabIndex = 4;
            this.pnlProductLookup.Visible = false;
            // 
            // gb_search
            // 
            this.gb_search.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_search.BackColor = System.Drawing.Color.Transparent;
            this.gb_search.Controls.Add(this.txtPriceInTicketLessThan);
            this.gb_search.Controls.Add(this.pnlProductLookupButtons);
            this.gb_search.Controls.Add(this.tcProducts);
            this.gb_search.Controls.Add(this.lblPriceInTickets);
            this.gb_search.Controls.Add(this.txtPriceInTicketMoreThan);
            this.gb_search.Controls.Add(this.txt_prodcode);
            this.gb_search.Controls.Add(this.lbl_prodcode);
            this.gb_search.Controls.Add(this.lbl_proddesc);
            this.gb_search.Controls.Add(this.txt_proddesc);
            this.gb_search.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.gb_search.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gb_search.Location = new System.Drawing.Point(7, 6);
            this.gb_search.Name = "gb_search";
            this.gb_search.Size = new System.Drawing.Size(626, 416);
            this.gb_search.TabIndex = 31;
            this.gb_search.TabStop = false;
            this.gb_search.Text = "Search Gifts";
            // 
            // txtPriceInTicketLessThan
            // 
            this.txtPriceInTicketLessThan.Location = new System.Drawing.Point(232, 58);
            this.txtPriceInTicketLessThan.MaxLength = 14;
            this.txtPriceInTicketLessThan.Name = "txtPriceInTicketLessThan";
            this.txtPriceInTicketLessThan.Size = new System.Drawing.Size(94, 20);
            this.txtPriceInTicketLessThan.TabIndex = 17;
            this.txtPriceInTicketLessThan.Enter += new System.EventHandler(this.txtPriceInTicketLessThan_Enter);
            this.txtPriceInTicketLessThan.Leave += new System.EventHandler(this.txtPriceInTicketLessThan_Leave);
            // 
            // pnlProductLookupButtons
            // 
            this.pnlProductLookupButtons.Controls.Add(this.btnKeyPad);
            this.pnlProductLookupButtons.Controls.Add(this.btnSearch);
            this.pnlProductLookupButtons.Controls.Add(this.btnClearSearch);
            this.pnlProductLookupButtons.Controls.Add(this.btnExitProductLookup);
            this.pnlProductLookupButtons.Location = new System.Drawing.Point(350, 9);
            this.pnlProductLookupButtons.Name = "pnlProductLookupButtons";
            this.pnlProductLookupButtons.Size = new System.Drawing.Size(266, 97);
            this.pnlProductLookupButtons.TabIndex = 16;
            // 
            // btnKeyPad
            // 
            this.btnKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatAppearance.BorderSize = 0;
            this.btnKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyPad.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnKeyPad.Location = new System.Drawing.Point(88, 49);
            this.btnKeyPad.Name = "btnKeyPad";
            this.btnKeyPad.Size = new System.Drawing.Size(58, 47);
            this.btnKeyPad.TabIndex = 99;
            this.btnKeyPad.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.Search_Btn_Normal;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(7, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 45);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnSearch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSearch_MouseDown);
            this.btnSearch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSearch_MouseUp);
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
            this.btnClearSearch.Location = new System.Drawing.Point(82, 2);
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
            // btnExitProductLookup
            // 
            this.btnExitProductLookup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.btnExitProductLookup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExitProductLookup.BackgroundImage")));
            this.btnExitProductLookup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExitProductLookup.FlatAppearance.BorderSize = 0;
            this.btnExitProductLookup.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExitProductLookup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExitProductLookup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExitProductLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExitProductLookup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnExitProductLookup.ForeColor = System.Drawing.Color.White;
            this.btnExitProductLookup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExitProductLookup.Location = new System.Drawing.Point(7, 48);
            this.btnExitProductLookup.Name = "btnExitProductLookup";
            this.btnExitProductLookup.Size = new System.Drawing.Size(70, 45);
            this.btnExitProductLookup.TabIndex = 15;
            this.btnExitProductLookup.Text = "Exit";
            this.btnExitProductLookup.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExitProductLookup.UseVisualStyleBackColor = false;
            this.btnExitProductLookup.Click += new System.EventHandler(this.btnExitProductLookup_Click);
            this.btnExitProductLookup.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnExitProductLookup_MouseClick);
            this.btnExitProductLookup.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnExitProductLookup_MouseUp);
            // 
            // tcProducts
            // 
            this.tcProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcProducts.Controls.Add(this.tpProductButtons);
            this.tcProducts.Controls.Add(this.tpProductList);
            this.tcProducts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcProducts.Location = new System.Drawing.Point(3, 84);
            this.tcProducts.Name = "tcProducts";
            this.tcProducts.SelectedIndex = 0;
            this.tcProducts.Size = new System.Drawing.Size(617, 334);
            this.tcProducts.TabIndex = 12;
            this.tcProducts.SelectedIndexChanged += new System.EventHandler(this.tcProducts_SelectedIndexChanged);
            // 
            // tpProductButtons
            // 
            this.tpProductButtons.Controls.Add(this.flpProducts);
            this.tpProductButtons.Location = new System.Drawing.Point(4, 25);
            this.tpProductButtons.Name = "tpProductButtons";
            this.tpProductButtons.Padding = new System.Windows.Forms.Padding(3);
            this.tpProductButtons.Size = new System.Drawing.Size(609, 305);
            this.tpProductButtons.TabIndex = 0;
            this.tpProductButtons.Text = "Gifts";
            this.tpProductButtons.UseVisualStyleBackColor = true;
            // 
            // flpProducts
            // 
            this.flpProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpProducts.AutoScroll = true;
            this.flpProducts.Controls.Add(this.btnSampleProduct);
            this.flpProducts.Location = new System.Drawing.Point(3, 3);
            this.flpProducts.Name = "flpProducts";
            this.flpProducts.Size = new System.Drawing.Size(603, 296);
            this.flpProducts.TabIndex = 0;
            this.flpProducts.Scroll += new System.Windows.Forms.ScrollEventHandler(this.flpProducts_Scroll);
            this.flpProducts.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.flpProducts_MouseWheel);
            // 
            // btnSampleProduct
            // 
            this.btnSampleProduct.BackgroundImage = global::Parafait_POS.Properties.Resources.CheckInCheckOut;
            this.btnSampleProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSampleProduct.FlatAppearance.BorderSize = 0;
            this.btnSampleProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSampleProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSampleProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSampleProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnSampleProduct.ForeColor = System.Drawing.Color.White;
            this.btnSampleProduct.Location = new System.Drawing.Point(2, 2);
            this.btnSampleProduct.Margin = new System.Windows.Forms.Padding(2);
            this.btnSampleProduct.Name = "btnSampleProduct";
            this.btnSampleProduct.Size = new System.Drawing.Size(68, 55);
            this.btnSampleProduct.TabIndex = 0;
            this.btnSampleProduct.Text = "Small Teddy Bear";
            this.btnSampleProduct.UseVisualStyleBackColor = true;
            // 
            // tpProductList
            // 
            this.tpProductList.Controls.Add(this.productInventoryInfo);
            this.tpProductList.Controls.Add(this.search_dgv);
            this.tpProductList.Location = new System.Drawing.Point(4, 25);
            this.tpProductList.Name = "tpProductList";
            this.tpProductList.Padding = new System.Windows.Forms.Padding(3);
            this.tpProductList.Size = new System.Drawing.Size(609, 305);
            this.tpProductList.TabIndex = 1;
            this.tpProductList.Text = "List";
            this.tpProductList.UseVisualStyleBackColor = true;
            this.tpProductList.Enter += new System.EventHandler(this.tpProductList_Enter);
            // 
            // productInventoryInfo
            // 
            this.productInventoryInfo.AutoScroll = true;
            this.productInventoryInfo.AutoScrollMinSize = new System.Drawing.Size(625, 150);
            this.productInventoryInfo.AutoSize = true;
            this.productInventoryInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.productInventoryInfo.Controls.Add(this.btnProdInvInfoClose);
            this.productInventoryInfo.Controls.Add(this.productInvDgv);
            this.productInventoryInfo.Location = new System.Drawing.Point(16, 152);
            this.productInventoryInfo.Name = "productInventoryInfo";
            this.productInventoryInfo.Size = new System.Drawing.Size(627, 152);
            this.productInventoryInfo.TabIndex = 6;
            this.productInventoryInfo.Visible = false;
            this.productInventoryInfo.Leave += new System.EventHandler(this.productInventoryInfo_Leave);
            // 
            // btnProdInvInfoClose
            // 
            this.btnProdInvInfoClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnProdInvInfoClose.BackColor = System.Drawing.Color.Transparent;
            this.btnProdInvInfoClose.BackgroundImage = global::Parafait_POS.Properties.Resources.CancelLine;
            this.btnProdInvInfoClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProdInvInfoClose.FlatAppearance.BorderSize = 0;
            this.btnProdInvInfoClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProdInvInfoClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProdInvInfoClose.ForeColor = System.Drawing.Color.White;
            this.btnProdInvInfoClose.Location = new System.Drawing.Point(293, 106);
            this.btnProdInvInfoClose.Name = "btnProdInvInfoClose";
            this.btnProdInvInfoClose.Size = new System.Drawing.Size(70, 45);
            this.btnProdInvInfoClose.TabIndex = 14;
            this.btnProdInvInfoClose.Text = "Close";
            this.btnProdInvInfoClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnProdInvInfoClose.UseVisualStyleBackColor = false;
            this.btnProdInvInfoClose.Click += new System.EventHandler(this.btnProdInvInfoClose_Click);
            this.btnProdInvInfoClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnProdInvInfoClose_MouseDown);
            this.btnProdInvInfoClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnProdInvInfoClose_MouseUp);
            // 
            // productInvDgv
            // 
            this.productInvDgv.AllowUserToAddRows = false;
            this.productInvDgv.AllowUserToDeleteRows = false;
            this.productInvDgv.AutoGenerateColumns = false;
            this.productInvDgv.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.productInvDgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.productInvDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productInvDgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.inventoryIdDataGridViewTextBoxColumn,
            this.productIdDataGridViewTextBoxColumn,
            this.codeDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.locationIdDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.timestampDataGridViewTextBoxColumn,
            this.allocatedQuantityDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.lotIdDataGridViewTextBoxColumn,
            this.receivePriceDataGridViewTextBoxColumn,
            this.lotNumberDataGridViewTextBoxColumn,
            this.isPurchasebleDataGridViewTextBoxColumn,
            this.sKUDataGridViewTextBoxColumn,
            this.barcodeDataGridViewTextBoxColumn,
            this.remarksMandatoryDataGridViewTextBoxColumn,
            this.massUpdateAllowedDataGridViewTextBoxColumn,
            this.totalCostDataGridViewTextBoxColumn,
            this.stagingQuantityDataGridViewTextBoxColumn,
            this.stagingRemarksDataGridViewTextBoxColumn});
            this.productInvDgv.DataSource = this.inventoryDTOBindingSource;
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle25.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle25.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.productInvDgv.DefaultCellStyle = dataGridViewCellStyle25;
            this.productInvDgv.GridColor = System.Drawing.Color.White;
            this.productInvDgv.Location = new System.Drawing.Point(3, 3);
            this.productInvDgv.Name = "productInvDgv";
            this.productInvDgv.ReadOnly = true;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.productInvDgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle26;
            this.productInvDgv.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productInvDgv.Size = new System.Drawing.Size(617, 97);
            this.productInvDgv.TabIndex = 0;
            this.productInvDgv.Scroll += new System.Windows.Forms.ScrollEventHandler(this.search_dgv_Scroll);
            this.productInvDgv.Click += new System.EventHandler(this.SearchFieldEnter);
            // 
            // inventoryIdDataGridViewTextBoxColumn
            // 
            this.inventoryIdDataGridViewTextBoxColumn.DataPropertyName = "InventoryId";
            this.inventoryIdDataGridViewTextBoxColumn.HeaderText = "InventoryId";
            this.inventoryIdDataGridViewTextBoxColumn.Name = "inventoryIdDataGridViewTextBoxColumn";
            this.inventoryIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.inventoryIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // productIdDataGridViewTextBoxColumn
            // 
            this.productIdDataGridViewTextBoxColumn.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn.HeaderText = "ProductId";
            this.productIdDataGridViewTextBoxColumn.Name = "productIdDataGridViewTextBoxColumn";
            this.productIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.productIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // codeDataGridViewTextBoxColumn
            // 
            this.codeDataGridViewTextBoxColumn.DataPropertyName = "Code";
            this.codeDataGridViewTextBoxColumn.HeaderText = "Product Code";
            this.codeDataGridViewTextBoxColumn.Name = "codeDataGridViewTextBoxColumn";
            this.codeDataGridViewTextBoxColumn.ReadOnly = true;
            this.codeDataGridViewTextBoxColumn.Width = 120;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Width = 125;
            // 
            // locationIdDataGridViewTextBoxColumn
            // 
            this.locationIdDataGridViewTextBoxColumn.DataPropertyName = "LocationId";
            this.locationIdDataGridViewTextBoxColumn.DataSource = this.locationDTOBindingSource;
            this.locationIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            this.locationIdDataGridViewTextBoxColumn.HeaderText = "Location";
            this.locationIdDataGridViewTextBoxColumn.Name = "locationIdDataGridViewTextBoxColumn";
            this.locationIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.locationIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.locationIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.locationIdDataGridViewTextBoxColumn.ValueMember = "LocationId";
            // 
            // locationDTOBindingSource
            // 
            this.locationDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.LocationDTO);
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            this.quantityDataGridViewTextBoxColumn.Width = 60;
            // 
            // timestampDataGridViewTextBoxColumn
            // 
            this.timestampDataGridViewTextBoxColumn.DataPropertyName = "Timestamp";
            this.timestampDataGridViewTextBoxColumn.HeaderText = "Timestamp";
            this.timestampDataGridViewTextBoxColumn.Name = "timestampDataGridViewTextBoxColumn";
            this.timestampDataGridViewTextBoxColumn.ReadOnly = true;
            this.timestampDataGridViewTextBoxColumn.Visible = false;
            // 
            // allocatedQuantityDataGridViewTextBoxColumn
            // 
            this.allocatedQuantityDataGridViewTextBoxColumn.DataPropertyName = "AllocatedQuantity";
            this.allocatedQuantityDataGridViewTextBoxColumn.HeaderText = "Allocated Quantity";
            this.allocatedQuantityDataGridViewTextBoxColumn.Name = "allocatedQuantityDataGridViewTextBoxColumn";
            this.allocatedQuantityDataGridViewTextBoxColumn.ReadOnly = true;
            this.allocatedQuantityDataGridViewTextBoxColumn.Visible = false;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            this.remarksDataGridViewTextBoxColumn.ReadOnly = true;
            this.remarksDataGridViewTextBoxColumn.Visible = false;
            // 
            // lotIdDataGridViewTextBoxColumn
            // 
            this.lotIdDataGridViewTextBoxColumn.DataPropertyName = "LotId";
            this.lotIdDataGridViewTextBoxColumn.HeaderText = "LotId";
            this.lotIdDataGridViewTextBoxColumn.Name = "lotIdDataGridViewTextBoxColumn";
            this.lotIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.lotIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // receivePriceDataGridViewTextBoxColumn
            // 
            this.receivePriceDataGridViewTextBoxColumn.DataPropertyName = "ReceivePrice";
            this.receivePriceDataGridViewTextBoxColumn.HeaderText = "ReceivePrice";
            this.receivePriceDataGridViewTextBoxColumn.Name = "receivePriceDataGridViewTextBoxColumn";
            this.receivePriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.receivePriceDataGridViewTextBoxColumn.Visible = false;
            // 
            // lotNumberDataGridViewTextBoxColumn
            // 
            this.lotNumberDataGridViewTextBoxColumn.DataPropertyName = "LotNumber";
            this.lotNumberDataGridViewTextBoxColumn.HeaderText = "Inventory Lot#";
            this.lotNumberDataGridViewTextBoxColumn.Name = "lotNumberDataGridViewTextBoxColumn";
            this.lotNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.lotNumberDataGridViewTextBoxColumn.Width = 120;
            // 
            // isPurchasebleDataGridViewTextBoxColumn
            // 
            this.isPurchasebleDataGridViewTextBoxColumn.DataPropertyName = "IsPurchaseble";
            this.isPurchasebleDataGridViewTextBoxColumn.HeaderText = "IsPurchaseble";
            this.isPurchasebleDataGridViewTextBoxColumn.Name = "isPurchasebleDataGridViewTextBoxColumn";
            this.isPurchasebleDataGridViewTextBoxColumn.ReadOnly = true;
            this.isPurchasebleDataGridViewTextBoxColumn.Visible = false;
            // 
            // sKUDataGridViewTextBoxColumn
            // 
            this.sKUDataGridViewTextBoxColumn.DataPropertyName = "SKU";
            this.sKUDataGridViewTextBoxColumn.HeaderText = "SKU";
            this.sKUDataGridViewTextBoxColumn.Name = "sKUDataGridViewTextBoxColumn";
            this.sKUDataGridViewTextBoxColumn.ReadOnly = true;
            this.sKUDataGridViewTextBoxColumn.Visible = false;
            // 
            // barcodeDataGridViewTextBoxColumn
            // 
            this.barcodeDataGridViewTextBoxColumn.DataPropertyName = "Barcode";
            this.barcodeDataGridViewTextBoxColumn.HeaderText = "Barcode";
            this.barcodeDataGridViewTextBoxColumn.Name = "barcodeDataGridViewTextBoxColumn";
            this.barcodeDataGridViewTextBoxColumn.ReadOnly = true;
            this.barcodeDataGridViewTextBoxColumn.Visible = false;
            // 
            // remarksMandatoryDataGridViewTextBoxColumn
            // 
            this.remarksMandatoryDataGridViewTextBoxColumn.DataPropertyName = "RemarksMandatory";
            this.remarksMandatoryDataGridViewTextBoxColumn.HeaderText = "Remarks Mandatory";
            this.remarksMandatoryDataGridViewTextBoxColumn.Name = "remarksMandatoryDataGridViewTextBoxColumn";
            this.remarksMandatoryDataGridViewTextBoxColumn.ReadOnly = true;
            this.remarksMandatoryDataGridViewTextBoxColumn.Visible = false;
            // 
            // massUpdateAllowedDataGridViewTextBoxColumn
            // 
            this.massUpdateAllowedDataGridViewTextBoxColumn.DataPropertyName = "MassUpdateAllowed";
            this.massUpdateAllowedDataGridViewTextBoxColumn.HeaderText = "Mass Update Allowed";
            this.massUpdateAllowedDataGridViewTextBoxColumn.Name = "massUpdateAllowedDataGridViewTextBoxColumn";
            this.massUpdateAllowedDataGridViewTextBoxColumn.ReadOnly = true;
            this.massUpdateAllowedDataGridViewTextBoxColumn.Visible = false;
            // 
            // totalCostDataGridViewTextBoxColumn
            // 
            this.totalCostDataGridViewTextBoxColumn.DataPropertyName = "TotalCost";
            this.totalCostDataGridViewTextBoxColumn.HeaderText = "Total Cost";
            this.totalCostDataGridViewTextBoxColumn.Name = "totalCostDataGridViewTextBoxColumn";
            this.totalCostDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalCostDataGridViewTextBoxColumn.Visible = false;
            // 
            // stagingQuantityDataGridViewTextBoxColumn
            // 
            this.stagingQuantityDataGridViewTextBoxColumn.DataPropertyName = "StagingQuantity";
            this.stagingQuantityDataGridViewTextBoxColumn.HeaderText = "New Quantity";
            this.stagingQuantityDataGridViewTextBoxColumn.Name = "stagingQuantityDataGridViewTextBoxColumn";
            this.stagingQuantityDataGridViewTextBoxColumn.ReadOnly = true;
            this.stagingQuantityDataGridViewTextBoxColumn.Visible = false;
            // 
            // stagingRemarksDataGridViewTextBoxColumn
            // 
            this.stagingRemarksDataGridViewTextBoxColumn.DataPropertyName = "StagingRemarks";
            this.stagingRemarksDataGridViewTextBoxColumn.HeaderText = "Staging Remarks";
            this.stagingRemarksDataGridViewTextBoxColumn.Name = "stagingRemarksDataGridViewTextBoxColumn";
            this.stagingRemarksDataGridViewTextBoxColumn.ReadOnly = true;
            this.stagingRemarksDataGridViewTextBoxColumn.Visible = false;
            // 
            // inventoryDTOBindingSource
            // 
            this.inventoryDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.InventoryDTO);
            // 
            // search_dgv
            // 
            this.search_dgv.AllowUserToAddRows = false;
            this.search_dgv.AllowUserToDeleteRows = false;
            this.search_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.search_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.search_dgv.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.search_dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.search_dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.search_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.search_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectGift,
            this.imgViewInventoryLocations});
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.search_dgv.DefaultCellStyle = dataGridViewCellStyle27;
            this.search_dgv.EnableHeadersVisualStyles = false;
            this.search_dgv.GridColor = System.Drawing.Color.White;
            this.search_dgv.Location = new System.Drawing.Point(3, 3);
            this.search_dgv.Name = "search_dgv";
            this.search_dgv.RowHeadersVisible = false;
            dataGridViewCellStyle28.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle28.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.search_dgv.RowsDefaultCellStyle = dataGridViewCellStyle28;
            this.search_dgv.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.search_dgv.RowTemplate.Height = 40;
            this.search_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.search_dgv.Size = new System.Drawing.Size(603, 298);
            this.search_dgv.TabIndex = 5;
            this.search_dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.search_dgv_CellClick);
            this.search_dgv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.search_dgv_CellDoubleClick);
            this.search_dgv.Scroll += new System.Windows.Forms.ScrollEventHandler(this.search_dgv_Scroll);
            // 
            // selectGift
            // 
            this.selectGift.DataPropertyName = "selectGift";
            this.selectGift.FalseValue = "N";
            this.selectGift.HeaderText = "Select";
            this.selectGift.Name = "selectGift";
            this.selectGift.TrueValue = "Y";
            this.selectGift.Visible = false;
            // 
            // imgViewInventoryLocations
            // 
            this.imgViewInventoryLocations.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.imgViewInventoryLocations.HeaderText = "";
            this.imgViewInventoryLocations.Image = ((System.Drawing.Image)(resources.GetObject("imgViewInventoryLocations.Image")));
            this.imgViewInventoryLocations.Name = "imgViewInventoryLocations";
            this.imgViewInventoryLocations.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.imgViewInventoryLocations.Width = 50;
            // 
            // lblPriceInTickets
            // 
            this.lblPriceInTickets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblPriceInTickets.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPriceInTickets.Location = new System.Drawing.Point(8, 63);
            this.lblPriceInTickets.Name = "lblPriceInTickets";
            this.lblPriceInTickets.Size = new System.Drawing.Size(104, 13);
            this.lblPriceInTickets.TabIndex = 9;
            this.lblPriceInTickets.Text = "$$$ Range:";
            this.lblPriceInTickets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPriceInTicketMoreThan
            // 
            this.txtPriceInTicketMoreThan.Location = new System.Drawing.Point(119, 58);
            this.txtPriceInTicketMoreThan.MaxLength = 14;
            this.txtPriceInTicketMoreThan.Name = "txtPriceInTicketMoreThan";
            this.txtPriceInTicketMoreThan.Size = new System.Drawing.Size(94, 20);
            this.txtPriceInTicketMoreThan.TabIndex = 10;
            this.txtPriceInTicketMoreThan.Enter += new System.EventHandler(this.txtPriceInTicketMoreThan_Enter);
            this.txtPriceInTicketMoreThan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            this.txtPriceInTicketMoreThan.Leave += new System.EventHandler(this.txtPriceInTicketMoreThan_Leave);
            // 
            // txt_prodcode
            // 
            this.txt_prodcode.Location = new System.Drawing.Point(119, 14);
            this.txt_prodcode.Name = "txt_prodcode";
            this.txt_prodcode.Size = new System.Drawing.Size(154, 20);
            this.txt_prodcode.TabIndex = 1;
            this.txt_prodcode.Enter += new System.EventHandler(this.SearchFieldEnter);
            this.txt_prodcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            // 
            // lbl_prodcode
            // 
            this.lbl_prodcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbl_prodcode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_prodcode.Location = new System.Drawing.Point(8, 19);
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
            this.lbl_proddesc.Location = new System.Drawing.Point(8, 41);
            this.lbl_proddesc.Name = "lbl_proddesc";
            this.lbl_proddesc.Size = new System.Drawing.Size(104, 13);
            this.lbl_proddesc.TabIndex = 2;
            this.lbl_proddesc.Text = "Description:";
            this.lbl_proddesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_proddesc
            // 
            this.txt_proddesc.Location = new System.Drawing.Point(119, 36);
            this.txt_proddesc.Name = "txt_proddesc";
            this.txt_proddesc.Size = new System.Drawing.Size(154, 20);
            this.txt_proddesc.TabIndex = 3;
            this.txt_proddesc.Enter += new System.EventHandler(this.SearchFieldEnter);
            this.txt_proddesc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchFieldKeyPress);
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnMoreOptions);
            this.panelButtons.Controls.Add(this.btnProductSearch);
            this.panelButtons.Controls.Add(this.btnTurnIn);
            this.panelButtons.Controls.Add(this.btnLoadTickets);
            this.panelButtons.Controls.Add(this.btnAddManual);
            this.panelButtons.Controls.Add(this.btnPrint);
            this.panelButtons.Controls.Add(this.btnSuspend);
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Controls.Add(this.btnScanTicketOrGift);
            this.panelButtons.Controls.Add(this.btnFlagTicketReceipt);
            this.panelButtons.Controls.Add(this.btnNew);
            this.panelButtons.Controls.Add(this.panelAddClose);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 455);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(638, 60);
            this.panelButtons.TabIndex = 3;
            // 
            // btnMoreOptions
            // 
            this.btnMoreOptions.BackgroundImage = global::Parafait_POS.Properties.Resources.More_Options_Btn_Normal;
            this.btnMoreOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMoreOptions.FlatAppearance.BorderSize = 0;
            this.btnMoreOptions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMoreOptions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMoreOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoreOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoreOptions.ForeColor = System.Drawing.Color.White;
            this.btnMoreOptions.Location = new System.Drawing.Point(613, 5);
            this.btnMoreOptions.Name = "btnMoreOptions";
            this.btnMoreOptions.Size = new System.Drawing.Size(70, 45);
            this.btnMoreOptions.TabIndex = 7;
            this.btnMoreOptions.Text = "More ";
            this.btnMoreOptions.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMoreOptions.UseVisualStyleBackColor = true;
            this.btnMoreOptions.Click += new System.EventHandler(this.btnMoreOptions_Click);
            this.btnMoreOptions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnMoreOptions_MouseDown);
            this.btnMoreOptions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMoreOptions_MouseUp);
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
            this.btnProductSearch.Location = new System.Drawing.Point(563, 5);
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
            // btnTurnIn
            // 
            this.btnTurnIn.BackgroundImage = global::Parafait_POS.Properties.Resources.TurnInNormal;
            this.btnTurnIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTurnIn.FlatAppearance.BorderSize = 0;
            this.btnTurnIn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTurnIn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTurnIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurnIn.ForeColor = System.Drawing.Color.White;
            this.btnTurnIn.Location = new System.Drawing.Point(493, 5);
            this.btnTurnIn.Name = "btnTurnIn";
            this.btnTurnIn.Size = new System.Drawing.Size(70, 45);
            this.btnTurnIn.TabIndex = 9;
            this.btnTurnIn.Text = "Turn-In";
            this.btnTurnIn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTurnIn.UseVisualStyleBackColor = true;
            this.btnTurnIn.Click += new System.EventHandler(this.btnTurnIn_Click);
            this.btnTurnIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnIn_MouseDown);
            this.btnTurnIn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnIn_MouseUp);
            // 
            // btnLoadTickets
            // 
            this.btnLoadTickets.BackgroundImage = global::Parafait_POS.Properties.Resources.LoadTicket_Normal;
            this.btnLoadTickets.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLoadTickets.FlatAppearance.BorderSize = 0;
            this.btnLoadTickets.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLoadTickets.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLoadTickets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadTickets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadTickets.ForeColor = System.Drawing.Color.White;
            this.btnLoadTickets.Location = new System.Drawing.Point(423, 5);
            this.btnLoadTickets.Name = "btnLoadTickets";
            this.btnLoadTickets.Size = new System.Drawing.Size(70, 45);
            this.btnLoadTickets.TabIndex = 8;
            this.btnLoadTickets.Text = "Load Tickets";
            this.btnLoadTickets.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoadTickets.UseVisualStyleBackColor = true;
            this.btnLoadTickets.Click += new System.EventHandler(this.btnLoadTickets_Click);
            this.btnLoadTickets.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLoadTickets_MouseDown);
            this.btnLoadTickets.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLoadTickets_MouseUp);
            // 
            // btnAddManual
            // 
            this.btnAddManual.BackgroundImage = global::Parafait_POS.Properties.Resources.AddTicketNormal;
            this.btnAddManual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddManual.FlatAppearance.BorderSize = 0;
            this.btnAddManual.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddManual.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddManual.ForeColor = System.Drawing.Color.White;
            this.btnAddManual.Location = new System.Drawing.Point(353, 5);
            this.btnAddManual.Name = "btnAddManual";
            this.btnAddManual.Size = new System.Drawing.Size(70, 45);
            this.btnAddManual.TabIndex = 8;
            this.btnAddManual.Text = "Add";
            this.btnAddManual.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddManual.UseVisualStyleBackColor = true;
            this.btnAddManual.Click += new System.EventHandler(this.btnAddManual_Click);
            this.btnAddManual.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAddManual_MouseDown);
            this.btnAddManual.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAddManual_MouseUp);
            // 
            // btnPrint
            // 
            this.btnPrint.BackgroundImage = global::Parafait_POS.Properties.Resources.PrintTrx;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(143, 5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(70, 45);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSuspend
            // 
            this.btnSuspend.BackgroundImage = global::Parafait_POS.Properties.Resources.OrderSuspend;
            this.btnSuspend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSuspend.FlatAppearance.BorderSize = 0;
            this.btnSuspend.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSuspend.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSuspend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSuspend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuspend.ForeColor = System.Drawing.Color.White;
            this.btnSuspend.Location = new System.Drawing.Point(213, 5);
            this.btnSuspend.Name = "btnSuspend";
            this.btnSuspend.Size = new System.Drawing.Size(70, 45);
            this.btnSuspend.TabIndex = 4;
            this.btnSuspend.Text = "Suspend";
            this.btnSuspend.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSuspend.UseVisualStyleBackColor = true;
            this.btnSuspend.Click += new System.EventHandler(this.btnSuspend_Click);
            this.btnSuspend.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSuspend_MouseDown);
            this.btnSuspend.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSuspend_MouseUp);
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = global::Parafait_POS.Properties.Resources.OrderSave;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(73, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 45);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Complete";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnSave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSave_MouseDown);
            this.btnSave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSave_MouseUp);
            // 
            // btnScanTicketOrGift
            // 
            this.btnScanTicketOrGift.BackgroundImage = global::Parafait_POS.Properties.Resources.ScanGift;
            this.btnScanTicketOrGift.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnScanTicketOrGift.FlatAppearance.BorderSize = 0;
            this.btnScanTicketOrGift.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnScanTicketOrGift.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnScanTicketOrGift.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanTicketOrGift.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanTicketOrGift.ForeColor = System.Drawing.Color.White;
            this.btnScanTicketOrGift.Location = new System.Drawing.Point(283, 5);
            this.btnScanTicketOrGift.Name = "btnScanTicketOrGift";
            this.btnScanTicketOrGift.Size = new System.Drawing.Size(70, 45);
            this.btnScanTicketOrGift.TabIndex = 1;
            this.btnScanTicketOrGift.Text = "Scan Gift";
            this.btnScanTicketOrGift.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnScanTicketOrGift.UseVisualStyleBackColor = true;
            this.btnScanTicketOrGift.Click += new System.EventHandler(this.btnScanTicketOrGift_Click);
            this.btnScanTicketOrGift.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnScanTicketOrGift_MouseDown);
            this.btnScanTicketOrGift.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnScanTicketOrGift_MouseUp);
            // 
            // btnFlagTicketReceipt
            // 
            this.btnFlagTicketReceipt.BackgroundImage = global::Parafait_POS.Properties.Resources.FlagReceipt_Normal;
            this.btnFlagTicketReceipt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFlagTicketReceipt.FlatAppearance.BorderSize = 0;
            this.btnFlagTicketReceipt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFlagTicketReceipt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFlagTicketReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFlagTicketReceipt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFlagTicketReceipt.ForeColor = System.Drawing.Color.White;
            this.btnFlagTicketReceipt.Location = new System.Drawing.Point(633, 5);
            this.btnFlagTicketReceipt.Name = "btnFlagTicketReceipt";
            this.btnFlagTicketReceipt.Size = new System.Drawing.Size(70, 45);
            this.btnFlagTicketReceipt.TabIndex = 1;
            this.btnFlagTicketReceipt.Text = "Flag Voucher";
            this.btnFlagTicketReceipt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFlagTicketReceipt.UseVisualStyleBackColor = true;
            this.btnFlagTicketReceipt.Click += new System.EventHandler(this.btnFlagTicketReceipt_Click);
            this.btnFlagTicketReceipt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnFlagTicketReceipt_MouseDown);
            this.btnFlagTicketReceipt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnFlagTicketReceipt_MouseUp);
            // 
            // btnNew
            // 
            this.btnNew.BackgroundImage = global::Parafait_POS.Properties.Resources.NewTrx;
            this.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Location = new System.Drawing.Point(3, 5);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(70, 45);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "New";
            this.btnNew.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            this.btnNew.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNew_MouseDown);
            this.btnNew.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnNew_MouseUp);
            // 
            // panelAddClose
            // 
            this.panelAddClose.Controls.Add(this.btnClose);
            this.panelAddClose.Controls.Add(this.btnAddScreen);
            this.panelAddClose.Location = new System.Drawing.Point(523, 3);
            this.panelAddClose.Name = "panelAddClose";
            this.panelAddClose.Size = new System.Drawing.Size(115, 52);
            this.panelAddClose.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(60, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(50, 50);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "-";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddScreen
            // 
            this.btnAddScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddScreen.FlatAppearance.BorderSize = 0;
            this.btnAddScreen.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAddScreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddScreen.Location = new System.Drawing.Point(3, 2);
            this.btnAddScreen.Name = "btnAddScreen";
            this.btnAddScreen.Size = new System.Drawing.Size(50, 50);
            this.btnAddScreen.TabIndex = 5;
            this.btnAddScreen.Text = "+";
            this.btnAddScreen.UseVisualStyleBackColor = true;
            this.btnAddScreen.Visible = false;
            this.btnAddScreen.Click += new System.EventHandler(this.btnAddScreen_Click);
            // 
            // dgvRedemption
            // 
            this.dgvRedemption.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRedemption.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.dgvRedemption.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRedemption.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRedemption.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.dgvRedemption.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRedemption.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dummyColumn,
            this.dcRemove,
            this.dcProductId,
            this.dcQuantity,
            this.dcProductName,
            this.dcPrice,
            this.dcTotal});
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle23.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRedemption.DefaultCellStyle = dataGridViewCellStyle23;
            this.dgvRedemption.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvRedemption.EnableHeadersVisualStyles = false;
            this.dgvRedemption.GridColor = System.Drawing.Color.White;
            this.dgvRedemption.Location = new System.Drawing.Point(0, 88);
            this.dgvRedemption.Name = "dgvRedemption";
            this.dgvRedemption.RowHeadersVisible = false;
            dataGridViewCellStyle24.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvRedemption.RowsDefaultCellStyle = dataGridViewCellStyle24;
            this.dgvRedemption.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvRedemption.Size = new System.Drawing.Size(638, 364);
            this.dgvRedemption.TabIndex = 2;
            this.dgvRedemption.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRedemption_CellClick);
            this.dgvRedemption.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRedemption_CellValueChanged);
            // 
            // dummyColumn
            // 
            this.dummyColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dummyColumn.FillWeight = 1F;
            this.dummyColumn.HeaderText = "";
            this.dummyColumn.MinimumWidth = 2;
            this.dummyColumn.Name = "dummyColumn";
            this.dummyColumn.ReadOnly = true;
            this.dummyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dummyColumn.Width = 2;
            // 
            // dcRemove
            // 
            this.dcRemove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dcRemove.HeaderText = "X";
            this.dcRemove.Name = "dcRemove";
            this.dcRemove.Text = "X";
            this.dcRemove.UseColumnTextForButtonValue = true;
            this.dcRemove.Width = 30;
            // 
            // dcProductId
            // 
            this.dcProductId.HeaderText = "ProductId";
            this.dcProductId.Name = "dcProductId";
            this.dcProductId.Visible = false;
            // 
            // dcQuantity
            // 
            this.dcQuantity.HeaderText = "Qty";
            this.dcQuantity.Name = "dcQuantity";
            this.dcQuantity.ReadOnly = true;
            this.dcQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dcProductName
            // 
            this.dcProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dcProductName.HeaderText = "Gift";
            this.dcProductName.Name = "dcProductName";
            this.dcProductName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dcPrice
            // 
            this.dcPrice.HeaderText = "Price";
            this.dcPrice.Name = "dcPrice";
            this.dcPrice.ReadOnly = true;
            this.dcPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dcTotal
            // 
            this.dcTotal.HeaderText = "Total";
            this.dcTotal.Name = "dcTotal";
            this.dcTotal.ReadOnly = true;
            this.dcTotal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panelTop
            // 
            this.panelTop.BackgroundImage = global::Parafait_POS.Properties.Resources.blueGradient;
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTop.Controls.Add(this.lblScreenNumber);
            this.panelTop.Controls.Add(this.lblCardNumber);
            this.panelTop.Controls.Add(this.lblBalance);
            this.panelTop.Controls.Add(this.lblRedeemed);
            this.panelTop.Controls.Add(this.lblTotalTickets);
            this.panelTop.Controls.Add(this.label3);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.lnkTotalTickets);
            this.panelTop.Controls.Add(this.panel4);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelTop.ForeColor = System.Drawing.Color.White;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(638, 88);
            this.panelTop.TabIndex = 1;
            // 
            // lblScreenNumber
            // 
            this.lblScreenNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScreenNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblScreenNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 35F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenNumber.Location = new System.Drawing.Point(480, 5);
            this.lblScreenNumber.Margin = new System.Windows.Forms.Padding(0);
            this.lblScreenNumber.Name = "lblScreenNumber";
            this.lblScreenNumber.Size = new System.Drawing.Size(33, 73);
            this.lblScreenNumber.TabIndex = 9;
            this.lblScreenNumber.Text = "0";
            this.lblScreenNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCardNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.ForeColor = System.Drawing.Color.Black;
            this.lblCardNumber.Location = new System.Drawing.Point(214, 32);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(209, 23);
            this.lblCardNumber.TabIndex = 7;
            this.lblCardNumber.Text = "Card Number";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBalance
            // 
            this.lblBalance.BackColor = System.Drawing.Color.Transparent;
            this.lblBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalance.ForeColor = System.Drawing.Color.Black;
            this.lblBalance.Location = new System.Drawing.Point(133, 60);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(86, 23);
            this.lblBalance.TabIndex = 6;
            this.lblBalance.Text = "0";
            this.lblBalance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRedeemed
            // 
            this.lblRedeemed.BackColor = System.Drawing.Color.Transparent;
            this.lblRedeemed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRedeemed.ForeColor = System.Drawing.Color.Black;
            this.lblRedeemed.Location = new System.Drawing.Point(133, 32);
            this.lblRedeemed.Name = "lblRedeemed";
            this.lblRedeemed.Size = new System.Drawing.Size(86, 23);
            this.lblRedeemed.TabIndex = 5;
            this.lblRedeemed.Text = "0";
            this.lblRedeemed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalTickets
            // 
            this.lblTotalTickets.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalTickets.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalTickets.ForeColor = System.Drawing.Color.Black;
            this.lblTotalTickets.Location = new System.Drawing.Point(133, 4);
            this.lblTotalTickets.Name = "lblTotalTickets";
            this.lblTotalTickets.Size = new System.Drawing.Size(86, 23);
            this.lblTotalTickets.TabIndex = 4;
            this.lblTotalTickets.Text = "0";
            this.lblTotalTickets.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(35, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Balance:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(25, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Redeemed:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lnkTotalTickets
            // 
            this.lnkTotalTickets.BackColor = System.Drawing.Color.Transparent;
            this.lnkTotalTickets.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkTotalTickets.Location = new System.Drawing.Point(4, 4);
            this.lnkTotalTickets.Name = "lnkTotalTickets";
            this.lnkTotalTickets.Size = new System.Drawing.Size(123, 23);
            this.lnkTotalTickets.TabIndex = 1;
            this.lnkTotalTickets.TabStop = true;
            this.lnkTotalTickets.Text = "Total Tickets:";
            this.lnkTotalTickets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lnkTotalTickets.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTotalTickets_LinkClicked);
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnShowSuspended);
            this.panel4.Controls.Add(this.lblLoginId);
            this.panel4.Location = new System.Drawing.Point(542, -1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(100, 88);
            this.panel4.TabIndex = 0;
            // 
            // btnShowSuspended
            // 
            this.btnShowSuspended.BackColor = System.Drawing.Color.Transparent;
            this.btnShowSuspended.BackgroundImage = global::Parafait_POS.Properties.Resources.OrderSuspend;
            this.btnShowSuspended.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnShowSuspended.FlatAppearance.BorderSize = 0;
            this.btnShowSuspended.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowSuspended.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowSuspended.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowSuspended.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowSuspended.Location = new System.Drawing.Point(3, 28);
            this.btnShowSuspended.Name = "btnShowSuspended";
            this.btnShowSuspended.Size = new System.Drawing.Size(90, 56);
            this.btnShowSuspended.TabIndex = 7;
            this.btnShowSuspended.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnShowSuspended.UseVisualStyleBackColor = false;
            this.btnShowSuspended.Click += new System.EventHandler(this.btnSuspended_Click);
            // 
            // lblLoginId
            // 
            this.lblLoginId.BackColor = System.Drawing.Color.Gray;
            this.lblLoginId.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLoginId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginId.ForeColor = System.Drawing.Color.White;
            this.lblLoginId.Location = new System.Drawing.Point(0, 0);
            this.lblLoginId.Name = "lblLoginId";
            this.lblLoginId.Size = new System.Drawing.Size(98, 24);
            this.lblLoginId.TabIndex = 4;
            this.lblLoginId.Text = "loginId";
            this.lblLoginId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoEllipsis = true;
            this.lblMessage.BackColor = System.Drawing.Color.LightGray;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0, 515);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(638, 26);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "LabelMessage";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAddTicketMenuItem
            // 
            this.btnAddTicketMenuItem.Location = new System.Drawing.Point(0, 0);
            this.btnAddTicketMenuItem.Name = "btnAddTicketMenuItem";
            this.btnAddTicketMenuItem.Size = new System.Drawing.Size(75, 23);
            this.btnAddTicketMenuItem.TabIndex = 0;
            // 
            // btnLoadTicketMenuItem
            // 
            this.btnLoadTicketMenuItem.Location = new System.Drawing.Point(0, 0);
            this.btnLoadTicketMenuItem.Name = "btnLoadTicketMenuItem";
            this.btnLoadTicketMenuItem.Size = new System.Drawing.Size(75, 23);
            this.btnLoadTicketMenuItem.TabIndex = 0;
            // 
            // btnTurnInMenuItem
            // 
            this.btnTurnInMenuItem.Location = new System.Drawing.Point(0, 0);
            this.btnTurnInMenuItem.Name = "btnTurnInMenuItem";
            this.btnTurnInMenuItem.Size = new System.Drawing.Size(75, 23);
            this.btnTurnInMenuItem.TabIndex = 0;
            // 
            // btnProductSearchMenuItem
            // 
            this.btnProductSearchMenuItem.Location = new System.Drawing.Point(0, 0);
            this.btnProductSearchMenuItem.Name = "btnProductSearchMenuItem";
            this.btnProductSearchMenuItem.Size = new System.Drawing.Size(75, 23);
            this.btnProductSearchMenuItem.TabIndex = 0;
            // 
            // btnFlagTicketReceiptMenuItem
            // 
            this.btnFlagTicketReceiptMenuItem.Location = new System.Drawing.Point(0, 0);
            this.btnFlagTicketReceiptMenuItem.Name = "btnFlagTicketReceiptMenuItem";
            this.btnFlagTicketReceiptMenuItem.Size = new System.Drawing.Size(75, 23);
            this.btnFlagTicketReceiptMenuItem.TabIndex = 0;
            // 
            // btnScanTicketOrGiftMenuItem
            // 
            this.btnScanTicketOrGiftMenuItem.Location = new System.Drawing.Point(0, 0);
            this.btnScanTicketOrGiftMenuItem.Name = "btnScanTicketOrGiftMenuItem";
            this.btnScanTicketOrGiftMenuItem.Size = new System.Drawing.Size(75, 23);
            this.btnScanTicketOrGiftMenuItem.TabIndex = 0;
            // 
            // btnSuspendMenuItem
            // 
            this.btnSuspendMenuItem.Location = new System.Drawing.Point(0, 0);
            this.btnSuspendMenuItem.Name = "btnSuspendMenuItem";
            this.btnSuspendMenuItem.Size = new System.Drawing.Size(75, 23);
            this.btnSuspendMenuItem.TabIndex = 0;
            // 
            // frmScanAndRedeem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(640, 543);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmScanAndRedeem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmScanAndRedeem";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmScanAndRedeem_FormClosed);
            this.Load += new System.EventHandler(this.frmScanAndRedeem_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmScanAndRedeem_KeyUp);
            this.panel1.ResumeLayout(false);
            this.pnlProductLookup.ResumeLayout(false);
            this.gb_search.ResumeLayout(false);
            this.gb_search.PerformLayout();
            this.pnlProductLookupButtons.ResumeLayout(false);
            this.tcProducts.ResumeLayout(false);
            this.tpProductButtons.ResumeLayout(false);
            this.flpProducts.ResumeLayout(false);
            this.tpProductList.ResumeLayout(false);
            this.tpProductList.PerformLayout();
            this.productInventoryInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.productInvDgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.search_dgv)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelAddClose.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemption)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSuspend;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnScanTicketOrGift;
        private System.Windows.Forms.Button btnFlagTicketReceipt;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkTotalTickets;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblRedeemed;
        private System.Windows.Forms.Label lblTotalTickets;
        private System.Windows.Forms.DataGridView dgvRedemption;
        private System.Windows.Forms.Button btnAddScreen;
        private System.Windows.Forms.Label lblLoginId;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn dummyColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dcRemove;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcTotal;
        private System.Windows.Forms.Button btnShowSuspended;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.Button btnAddManual;
        private System.Windows.Forms.Button btnLoadTickets;
        private System.Windows.Forms.Button btnTurnIn;
        private System.Windows.Forms.Panel panelAddClose;
        private System.Windows.Forms.Label lblScreenNumber;
        private System.Windows.Forms.Button btnProductSearch;
        private System.Windows.Forms.Panel pnlProductLookup;
        private System.Windows.Forms.GroupBox gb_search;
        private System.Windows.Forms.TabControl tcProducts;
        private System.Windows.Forms.TabPage tpProductButtons;
        private System.Windows.Forms.FlowLayoutPanel flpProducts;
        private System.Windows.Forms.Button btnSampleProduct;
        private System.Windows.Forms.TabPage tpProductList;
        private System.Windows.Forms.DataGridView search_dgv;
        private System.Windows.Forms.Label lblPriceInTickets;
        private System.Windows.Forms.TextBox txtPriceInTicketMoreThan;
        private System.Windows.Forms.TextBox txt_prodcode;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lbl_prodcode;
        private System.Windows.Forms.Label lbl_proddesc;
        private System.Windows.Forms.TextBox txt_proddesc;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.Button btnExitProductLookup;
        private System.Windows.Forms.Button btnMoreOptions;
        //private System.Windows.Forms.ContextMenuStrip ctxMenuMoreOptions;
        //private System.Windows.Forms.ToolStripMenuItem addTicketsStripMenuItem;
        //private System.Windows.Forms.ToolStripMenuItem turnInStripMenuItem;
        //private System.Windows.Forms.ToolStripMenuItem productSearchStripMenuItem;
        //private System.Windows.Forms.ToolStripMenuItem loadTicketsStripMenuItem;
        private System.Windows.Forms.Panel pnlProductLookupButtons;
        //private System.Windows.Forms.ToolStripMenuItem flagTicketReceiptToolStripMenuItem;
        private System.Windows.Forms.Panel productInventoryInfo;
        private System.Windows.Forms.DataGridView productInvDgv;
        private System.Windows.Forms.BindingSource inventoryDTOBindingSource;
        private System.Windows.Forms.BindingSource locationDTOBindingSource;
        private System.Windows.Forms.Button btnProdInvInfoClose;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectGift;
        private System.Windows.Forms.DataGridViewImageColumn imgViewInventoryLocations;
        private System.Windows.Forms.DataGridViewTextBoxColumn inventoryIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn locationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timestampDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn allocatedQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivePriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn isPurchasebleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sKUDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn barcodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksMandatoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn massUpdateAllowedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stagingQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stagingRemarksDataGridViewTextBoxColumn;

        private System.Windows.Forms.FlowLayoutPanel fpnlMoreMenu;
        private System.Windows.Forms.Button btnAddTicketMenuItem;
        private System.Windows.Forms.Button btnLoadTicketMenuItem;
        private System.Windows.Forms.Button btnTurnInMenuItem;
        private System.Windows.Forms.Button btnProductSearchMenuItem;
        private System.Windows.Forms.Button btnFlagTicketReceiptMenuItem;
        private System.Windows.Forms.Button btnScanTicketOrGiftMenuItem;
        private System.Windows.Forms.Button btnSuspendMenuItem;
        //private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Timer timerClock;
        private Button btnKeyPad;
        private TextBox txtPriceInTicketLessThan;
    }
}