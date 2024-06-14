using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    partial class frmOrder
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
            this.label1 = new System.Windows.Forms.Label();
            this.tb_searchorder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_searchstatus = new System.Windows.Forms.ComboBox();
            this.cb_searchvendor = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.vendorBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.vendorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.searchorder_dgv = new System.Windows.Forms.DataGridView();
            this.PurchaseOrderId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gb_search = new System.Windows.Forms.GroupBox();
            this.cmbOrderType = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtProductPOSearch = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cb_search = new System.Windows.Forms.Button();
            this.cb_submit = new System.Windows.Forms.Button();
            this.cb_amendment = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gb_order = new System.Windows.Forms.GroupBox();
            this.lb_orderid = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtTotalTax = new System.Windows.Forms.TextBox();
            this.lblViewTotalTax = new System.Windows.Forms.LinkLabel();
            this.lblTosite = new System.Windows.Forms.Label();
            this.cmbToSite = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.tb_MarkupPercent = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.tb_TotalRetail = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.tb_TotalQuantity = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.tb_totalLogisticsCost = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tb_TotalLandedCost = new System.Windows.Forms.TextBox();
            this.order_dgv = new System.Windows.Forms.DataGridView();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbUOM = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequiredByDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitLogisticsCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxCode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TaxAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LowerLimitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpperLimitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostVariancePercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrigPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiveQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stockLink = new System.Windows.Forms.DataGridViewLinkColumn();
            this.CancelledDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MarketListItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequisitionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequisitionLineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceInTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taxInclusiveCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taxPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_order = new System.Windows.Forms.TextBox();
            this.tb_date = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tb_phone = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tb_contact = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_vendor = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblViewRequisitions = new System.Windows.Forms.LinkLabel();
            this.lnkRemarks = new System.Windows.Forms.LinkLabel();
            this.lblReprintCount = new System.Windows.Forms.Label();
            this.lblDocumentStatus = new System.Windows.Forms.Label();
            this.lblAmendmentNumber = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.TextBox();
            this.cmbDocumentType = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.label20 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.chkIsCreditPO = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tb_status = new System.Windows.Forms.ComboBox();
            this.tb_cancelledDate = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tb_total = new System.Windows.Forms.TextBox();
            this.cb_autoorder = new System.Windows.Forms.Button();
            this.btnEmail = new System.Windows.Forms.Button();
            this.btnReceipts = new System.Windows.Forms.Button();
            this.cb_print = new System.Windows.Forms.Button();
            this.cb_create = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cb_prodsearch = new System.Windows.Forms.Button();
            this.dd_qty = new System.Windows.Forms.ComboBox();
            this.txt_qty = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Rsearch_dgv = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Product_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_prodcode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.taxId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.vendorBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchorder_dgv)).BeginInit();
            this.gb_search.SuspendLayout();
            this.gb_order.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.order_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rsearch_dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Order #:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_searchorder
            // 
            this.tb_searchorder.Location = new System.Drawing.Point(59, 16);
            this.tb_searchorder.Name = "tb_searchorder";
            this.tb_searchorder.Size = new System.Drawing.Size(151, 20);
            this.tb_searchorder.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Status:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cb_searchstatus
            // 
            this.cb_searchstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_searchstatus.FormattingEnabled = true;
            this.cb_searchstatus.Items.AddRange(new object[] {
            "",
            "Open",
            "InProgress",
            "Cancelled",
            "Received",
            "ShortClose",
            "Drop Ship"});
            this.cb_searchstatus.Location = new System.Drawing.Point(59, 40);
            this.cb_searchstatus.Name = "cb_searchstatus";
            this.cb_searchstatus.Size = new System.Drawing.Size(151, 21);
            this.cb_searchstatus.TabIndex = 4;
            // 
            // cb_searchvendor
            // 
            this.cb_searchvendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cb_searchvendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cb_searchvendor.DataSource = this.vendorBindingSource1;
            this.cb_searchvendor.DisplayMember = "Name";
            this.cb_searchvendor.FormattingEnabled = true;
            this.cb_searchvendor.Location = new System.Drawing.Point(59, 65);
            this.cb_searchvendor.Name = "cb_searchvendor";
            this.cb_searchvendor.Size = new System.Drawing.Size(151, 21);
            this.cb_searchvendor.TabIndex = 6;
            this.cb_searchvendor.ValueMember = "VendorId";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Vendor:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // searchorder_dgv
            // 
            this.searchorder_dgv.AllowUserToAddRows = false;
            this.searchorder_dgv.AllowUserToDeleteRows = false;
            this.searchorder_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.searchorder_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PurchaseOrderId,
            this.OrderNumber,
            this.Status});
            this.searchorder_dgv.GridColor = System.Drawing.Color.LightSlateGray;
            this.searchorder_dgv.Location = new System.Drawing.Point(7, 165);
            this.searchorder_dgv.MultiSelect = false;
            this.searchorder_dgv.Name = "searchorder_dgv";
            this.searchorder_dgv.ReadOnly = true;
            this.searchorder_dgv.RowHeadersVisible = false;
            this.searchorder_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.searchorder_dgv.Size = new System.Drawing.Size(206, 136);
            this.searchorder_dgv.TabIndex = 10;
            this.searchorder_dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.searchorder_dgv_CellClick);
            this.searchorder_dgv.SelectionChanged += new System.EventHandler(this.searchorder_dgv_SelectionChanged);
            // 
            // PurchaseOrderId
            // 
            this.PurchaseOrderId.DataPropertyName = "PurchaseOrderId";
            this.PurchaseOrderId.HeaderText = "ID";
            this.PurchaseOrderId.Name = "PurchaseOrderId";
            this.PurchaseOrderId.ReadOnly = true;
            this.PurchaseOrderId.Visible = false;
            // 
            // OrderNumber
            // 
            this.OrderNumber.DataPropertyName = "OrderNumber";
            this.OrderNumber.HeaderText = "PO Number";
            this.OrderNumber.Name = "OrderNumber";
            this.OrderNumber.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "OrderStatus";
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // gb_search
            // 
            this.gb_search.Controls.Add(this.cmbOrderType);
            this.gb_search.Controls.Add(this.label22);
            this.gb_search.Controls.Add(this.txtProductPOSearch);
            this.gb_search.Controls.Add(this.label15);
            this.gb_search.Controls.Add(this.cb_search);
            this.gb_search.Controls.Add(this.tb_searchorder);
            this.gb_search.Controls.Add(this.cb_searchstatus);
            this.gb_search.Controls.Add(this.cb_searchvendor);
            this.gb_search.Controls.Add(this.label3);
            this.gb_search.Controls.Add(this.searchorder_dgv);
            this.gb_search.Controls.Add(this.label1);
            this.gb_search.Controls.Add(this.label2);
            this.gb_search.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gb_search.Location = new System.Drawing.Point(6, 3);
            this.gb_search.Name = "gb_search";
            this.gb_search.Size = new System.Drawing.Size(219, 307);
            this.gb_search.TabIndex = 0;
            this.gb_search.TabStop = false;
            this.gb_search.Text = "PO Search";
            // 
            // cmbOrderType
            // 
            this.cmbOrderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrderType.FormattingEnabled = true;
            this.cmbOrderType.Items.AddRange(new object[] {
            "",
            "Regular Purchase Order",
            "Contract Purchase Order"});
            this.cmbOrderType.Location = new System.Drawing.Point(59, 114);
            this.cmbOrderType.Name = "cmbOrderType";
            this.cmbOrderType.Size = new System.Drawing.Size(151, 21);
            this.cmbOrderType.TabIndex = 8;
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(6, 117);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(48, 13);
            this.label22.TabIndex = 10;
            this.label22.Text = "Type:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProductPOSearch
            // 
            this.txtProductPOSearch.Location = new System.Drawing.Point(59, 90);
            this.txtProductPOSearch.Name = "txtProductPOSearch";
            this.txtProductPOSearch.Size = new System.Drawing.Size(151, 20);
            this.txtProductPOSearch.TabIndex = 7;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(6, 94);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(48, 13);
            this.label15.TabIndex = 9;
            this.label15.Text = "Product:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cb_search
            // 
            this.cb_search.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_search.Image = global::Semnox.Parafait.Inventory.Properties.Resources.search;
            this.cb_search.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_search.Location = new System.Drawing.Point(122, 139);
            this.cb_search.Name = "cb_search";
            this.cb_search.Size = new System.Drawing.Size(88, 23);
            this.cb_search.TabIndex = 9;
            this.cb_search.Text = "Search";
            this.cb_search.UseVisualStyleBackColor = true;
            this.cb_search.Click += new System.EventHandler(this.cb_search_Click);
            // 
            // cb_submit
            // 
            this.cb_submit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_submit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_submit.Image = global::Semnox.Parafait.Inventory.Properties.Resources.save;
            this.cb_submit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_submit.Location = new System.Drawing.Point(917, 557);
            this.cb_submit.Name = "cb_submit";
            this.cb_submit.Size = new System.Drawing.Size(90, 23);
            this.cb_submit.TabIndex = 82;
            this.cb_submit.Text = "Save PO";
            this.cb_submit.Tag = "Save PO";
            this.cb_submit.UseVisualStyleBackColor = true;
            this.cb_submit.Click += new System.EventHandler(this.cb_submit_Click);
            // 
            // cb_amendment
            // 
            this.cb_amendment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_amendment.Enabled = false;
            this.cb_amendment.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_amendment.Image = global::Semnox.Parafait.Inventory.Properties.Resources.add1;
            this.cb_amendment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_amendment.Location = new System.Drawing.Point(797, 558);
            this.cb_amendment.Name = "cb_amendment";
            this.cb_amendment.Size = new System.Drawing.Size(102, 23);
            this.cb_amendment.TabIndex = 88;
            this.cb_amendment.Text = "    Amendment";
            this.cb_amendment.UseVisualStyleBackColor = true;
            this.cb_amendment.Click += new System.EventHandler(this.cb_amendment_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClose.Image = global::Semnox.Parafait.Inventory.Properties.Resources.cancel;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(368, 557);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 23);
            this.btnClose.TabIndex = 84;
            this.btnClose.Text = "Exit";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.button3_Click);
            // 
            // gb_order
            // 
            this.gb_order.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_order.Controls.Add(this.lb_orderid);
            this.gb_order.Controls.Add(this.panel2);
            this.gb_order.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gb_order.Location = new System.Drawing.Point(232, 3);
            this.gb_order.Name = "gb_order";
            this.gb_order.Size = new System.Drawing.Size(931, 546);
            this.gb_order.TabIndex = 17;
            this.gb_order.TabStop = false;
            this.gb_order.Text = "Order";
            // 
            // lb_orderid
            // 
            this.lb_orderid.AutoSize = true;
            this.lb_orderid.BackColor = System.Drawing.Color.White;
            this.lb_orderid.Location = new System.Drawing.Point(347, 16);
            this.lb_orderid.Name = "lb_orderid";
            this.lb_orderid.Size = new System.Drawing.Size(0, 13);
            this.lb_orderid.TabIndex = 43;
            this.lb_orderid.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.txtTotalTax);
            this.panel2.Controls.Add(this.lblViewTotalTax);
            this.panel2.Controls.Add(this.lblTosite);
            this.panel2.Controls.Add(this.cmbToSite);
            this.panel2.Controls.Add(this.label26);
            this.panel2.Controls.Add(this.tb_MarkupPercent);
            this.panel2.Controls.Add(this.label25);
            this.panel2.Controls.Add(this.tb_TotalRetail);
            this.panel2.Controls.Add(this.label24);
            this.panel2.Controls.Add(this.tb_TotalQuantity);
            this.panel2.Controls.Add(this.label23);
            this.panel2.Controls.Add(this.tb_totalLogisticsCost);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.tb_TotalLandedCost);
            this.panel2.Controls.Add(this.order_dgv);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.tb_order);
            this.panel2.Controls.Add(this.tb_date);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.tb_phone);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.tb_contact);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.cb_vendor);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.lblViewRequisitions);
            this.panel2.Controls.Add(this.lnkRemarks);
            this.panel2.Controls.Add(this.lblReprintCount);
            this.panel2.Controls.Add(this.lblDocumentStatus);
            this.panel2.Controls.Add(this.lblAmendmentNumber);
            this.panel2.Controls.Add(this.lblEmail);
            this.panel2.Controls.Add(this.cmbDocumentType);
            this.panel2.Controls.Add(this.label21);
            this.panel2.Controls.Add(this.dtpToDate);
            this.panel2.Controls.Add(this.label20);
            this.panel2.Controls.Add(this.dtpFromDate);
            this.panel2.Controls.Add(this.chkIsCreditPO);
            this.panel2.Controls.Add(this.label19);
            this.panel2.Controls.Add(this.label18);
            this.panel2.Controls.Add(this.tb_status);
            this.panel2.Controls.Add(this.tb_cancelledDate);
            this.panel2.Controls.Add(this.label17);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.tb_total);
            this.panel2.Controls.Add(this.cb_autoorder);
            this.panel2.Location = new System.Drawing.Point(7, 15);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(916, 527);
            this.panel2.TabIndex = 18;
            this.panel2.TabStop = true;
            // 
            // txtTotalTax
            // 
            this.txtTotalTax.Location = new System.Drawing.Point(563, 463);
            this.txtTotalTax.Name = "txtTotalTax";
            this.txtTotalTax.ReadOnly = true;
            this.txtTotalTax.Size = new System.Drawing.Size(102, 20);
            this.txtTotalTax.TabIndex = 204;
            this.txtTotalTax.Text = "0";
            this.txtTotalTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblViewTotalTax
            // 
            this.lblViewTotalTax.AutoSize = true;
            this.lblViewTotalTax.Location = new System.Drawing.Point(501, 467);
            this.lblViewTotalTax.Name = "lblViewTotalTax";
            this.lblViewTotalTax.Size = new System.Drawing.Size(55, 13);
            this.lblViewTotalTax.TabIndex = 203;
            this.lblViewTotalTax.TabStop = true;
            this.lblViewTotalTax.Text = "Total Tax:";
            this.lblViewTotalTax.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblViewTotalTax_LinkClicked);
            // 
            // lblTosite
            // 
            this.lblTosite.Location = new System.Drawing.Point(677, 15);
            this.lblTosite.Name = "lblTosite";
            this.lblTosite.Size = new System.Drawing.Size(93, 12);
            this.lblTosite.TabIndex = 208;
            this.lblTosite.Text = "Ship To :*";
            this.lblTosite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbToSite
            // 
            this.cmbToSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToSite.FormattingEnabled = true;
            this.cmbToSite.Location = new System.Drawing.Point(771, 11);
            this.cmbToSite.Name = "cmbToSite";
            this.cmbToSite.Size = new System.Drawing.Size(128, 21);
            this.cmbToSite.TabIndex = 209;
            this.cmbToSite.SelectedIndexChanged += new System.EventHandler(this.cmbToSite_SelectedIndexChanged);
            // 
            // label26
            // 
            this.label26.BackColor = System.Drawing.Color.White;
            this.label26.Location = new System.Drawing.Point(668, 467);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(77, 13);
            this.label26.TabIndex = 90;
            this.label26.Text = "Markup%:";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_MarkupPercent
            // 
            this.tb_MarkupPercent.Enabled = false;
            this.tb_MarkupPercent.Location = new System.Drawing.Point(749, 463);
            this.tb_MarkupPercent.Name = "tb_MarkupPercent";
            this.tb_MarkupPercent.ReadOnly = true;
            this.tb_MarkupPercent.Size = new System.Drawing.Size(102, 20);
            this.tb_MarkupPercent.TabIndex = 202;
            this.tb_MarkupPercent.Text = "0";
            this.tb_MarkupPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.White;
            this.label25.Location = new System.Drawing.Point(491, 495);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(68, 20);
            this.label25.TabIndex = 88;
            this.label25.Text = "Total Retail:";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_TotalRetail
            // 
            this.tb_TotalRetail.Enabled = false;
            this.tb_TotalRetail.Location = new System.Drawing.Point(563, 495);
            this.tb_TotalRetail.Name = "tb_TotalRetail";
            this.tb_TotalRetail.ReadOnly = true;
            this.tb_TotalRetail.Size = new System.Drawing.Size(102, 20);
            this.tb_TotalRetail.TabIndex = 207;
            this.tb_TotalRetail.Text = "0";
            this.tb_TotalRetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label24
            // 
            this.label24.BackColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(26, 463);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(80, 21);
            this.label24.TabIndex = 86;
            this.label24.Text = "Total Quantity:";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_TotalQuantity
            // 
            this.tb_TotalQuantity.Enabled = false;
            this.tb_TotalQuantity.Location = new System.Drawing.Point(112, 463);
            this.tb_TotalQuantity.Name = "tb_TotalQuantity";
            this.tb_TotalQuantity.ReadOnly = true;
            this.tb_TotalQuantity.Size = new System.Drawing.Size(102, 20);
            this.tb_TotalQuantity.TabIndex = 200;
            this.tb_TotalQuantity.Text = "0";
            this.tb_TotalQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(218, 497);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(109, 16);
            this.label23.TabIndex = 84;
            this.label23.Text = "Total Logistics Cost:";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_totalLogisticsCost
            // 
            this.tb_totalLogisticsCost.Enabled = false;
            this.tb_totalLogisticsCost.Location = new System.Drawing.Point(333, 495);
            this.tb_totalLogisticsCost.Name = "tb_totalLogisticsCost";
            this.tb_totalLogisticsCost.ReadOnly = true;
            this.tb_totalLogisticsCost.Size = new System.Drawing.Size(102, 20);
            this.tb_totalLogisticsCost.TabIndex = 206;
            this.tb_totalLogisticsCost.Text = "0";
            this.tb_totalLogisticsCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(10, 497);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(97, 16);
            this.label14.TabIndex = 82;
            this.label14.Text = "Total Landed Cost:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_TotalLandedCost
            // 
            this.tb_TotalLandedCost.Enabled = false;
            this.tb_TotalLandedCost.Location = new System.Drawing.Point(112, 495);
            this.tb_TotalLandedCost.Name = "tb_TotalLandedCost";
            this.tb_TotalLandedCost.ReadOnly = true;
            this.tb_TotalLandedCost.Size = new System.Drawing.Size(102, 20);
            this.tb_TotalLandedCost.TabIndex = 205;
            this.tb_TotalLandedCost.Text = "0";
            this.tb_TotalLandedCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // order_dgv
            // 
            this.order_dgv.AllowUserToOrderColumns = true;
            this.order_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.order_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.order_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.Description,
            this.cmbUOM,
            this.Quantity,
            this.RequiredByDate,
            this.Cost,
            this.UnitLogisticsCost,
            this.TaxCode,
            this.TaxAmount,
            this.DiscountPercentage,
            this.SubTotal,
            this.LowerLimitCost,
            this.UpperLimitCost,
            this.CostVariancePercent,
            this.OrigPrice,
            this.LineId,
            this.ReceiveQuantity,
            this.stockLink,
            this.CancelledDate,
            this.isActive,
            this.MarketListItem,
            this.ProductId,
            this.RequisitionId,
            this.RequisitionLineId,
            this.PriceInTickets,
            this.taxInclusiveCost,
            this.taxPercentage});
            this.order_dgv.GridColor = System.Drawing.Color.LightSlateGray;
            this.order_dgv.Location = new System.Drawing.Point(3, 192);
            this.order_dgv.Name = "order_dgv";
            this.order_dgv.RowHeadersVisible = false;
            this.order_dgv.RowHeadersWidth = 20;
            this.order_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.order_dgv.Size = new System.Drawing.Size(896, 246);
            this.order_dgv.TabIndex = 80;
            this.order_dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.order_dgv_CellClick);
            this.order_dgv.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.order_dgv_CellEndEdit);
            this.order_dgv.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.order_dgv_CellValidating);
            this.order_dgv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.order_dgv_CellValueChanged);
            this.order_dgv.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.order_dgv_UserDeletedRow);
            this.order_dgv.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.order_dgv_UserDeletingRow);
            // 
            // Code
            // 
            this.Code.FillWeight = 112.3439F;
            this.Code.HeaderText = "Product Code";
            this.Code.Name = "Code";
            this.Code.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Code.Width = 89;
            // 
            // Description
            // 
            this.Description.FillWeight = 192.3043F;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Description.Width = 185;
            // 
            // cmbUOM
            // 
            this.cmbUOM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cmbUOM.DataPropertyName = "UOMId";
            this.cmbUOM.FillWeight = 50F;
            this.cmbUOM.HeaderText = "UOM";
            this.cmbUOM.Name = "cmbUOM";
            this.cmbUOM.Width = 80;
            // 
            // Quantity
            // 
            this.Quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Quantity.DataPropertyName = "Quantity";
            this.Quantity.FillWeight = 60F;
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.Name = "Quantity";
            this.Quantity.Width = 55;
            // 
            // RequiredByDate
            // 
            this.RequiredByDate.FillWeight = 49.38443F;
            this.RequiredByDate.HeaderText = "Required By Date";
            this.RequiredByDate.Name = "RequiredByDate";
            this.RequiredByDate.Width = 86;
            // 
            // Cost
            // 
            this.Cost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Cost.DataPropertyName = "Cost";
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.Cost.DefaultCellStyle = dataGridViewCellStyle1;
            this.Cost.FillWeight = 50F;
            this.Cost.HeaderText = "Unit Price";
            this.Cost.Name = "Cost";
            this.Cost.Width = 50;
            // 
            // UnitLogisticsCost
            // 
            this.UnitLogisticsCost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.UnitLogisticsCost.FillWeight = 75F;
            this.UnitLogisticsCost.HeaderText = "Unit Logistics Cost";
            this.UnitLogisticsCost.Name = "UnitLogisticsCost";
            this.UnitLogisticsCost.Width = 68;
            // 
            // TaxCode
            // 
            this.TaxCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TaxCode.FillWeight = 80F;
            this.TaxCode.HeaderText = "Tax Code";
            this.TaxCode.Name = "TaxCode";
            this.TaxCode.Width = 74;
            // 
            // TaxAmount
            // 
            this.TaxAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.TaxAmount.DefaultCellStyle = dataGridViewCellStyle2;
            this.TaxAmount.FillWeight = 75F;
            this.TaxAmount.HeaderText = "Tax Amount";
            this.TaxAmount.Name = "TaxAmount";
            this.TaxAmount.Width = 74;
            // 
            // DiscountPercentage
            // 
            this.DiscountPercentage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DiscountPercentage.FillWeight = 70F;
            this.DiscountPercentage.HeaderText = "Discount %";
            this.DiscountPercentage.Name = "DiscountPercentage";
            this.DiscountPercentage.Width = 60;
            // 
            // SubTotal
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.SubTotal.DefaultCellStyle = dataGridViewCellStyle3;
            this.SubTotal.FillWeight = 36.14199F;
            this.SubTotal.HeaderText = "Sub Total";
            this.SubTotal.Name = "SubTotal";
            this.SubTotal.ReadOnly = true;
            this.SubTotal.Width = 72;
            // 
            // LowerLimitCost
            // 
            this.LowerLimitCost.HeaderText = "LowerLimitCost";
            this.LowerLimitCost.Name = "LowerLimitCost";
            this.LowerLimitCost.Visible = false;
            // 
            // UpperLimitCost
            // 
            this.UpperLimitCost.HeaderText = "UpperLimitCost";
            this.UpperLimitCost.Name = "UpperLimitCost";
            this.UpperLimitCost.Visible = false;
            // 
            // CostVariancePercent
            // 
            this.CostVariancePercent.HeaderText = "CostVariancePercent";
            this.CostVariancePercent.Name = "CostVariancePercent";
            this.CostVariancePercent.Visible = false;
            // 
            // OrigPrice
            // 
            this.OrigPrice.HeaderText = "OrigPrice";
            this.OrigPrice.Name = "OrigPrice";
            this.OrigPrice.Visible = false;
            // 
            // LineId
            // 
            this.LineId.HeaderText = "LineId";
            this.LineId.Name = "LineId";
            this.LineId.Visible = false;
            // 
            // ReceiveQuantity
            // 
            this.ReceiveQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ReceiveQuantity.FillWeight = 60F;
            this.ReceiveQuantity.HeaderText = "Received Quantity";
            this.ReceiveQuantity.Name = "ReceiveQuantity";
            this.ReceiveQuantity.ReadOnly = true;
            this.ReceiveQuantity.Width = 80;
            // 
            // stockLink
            // 
            this.stockLink.FillWeight = 18.44329F;
            this.stockLink.HeaderText = "Stock";
            this.stockLink.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.stockLink.Name = "stockLink";
            this.stockLink.ReadOnly = true;
            this.stockLink.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.stockLink.Text = "";
            this.stockLink.Width = 41;
            // 
            // CancelledDate
            // 
            this.CancelledDate.FillWeight = 40.75768F;
            this.CancelledDate.HeaderText = "Cancelled Date";
            this.CancelledDate.Name = "CancelledDate";
            this.CancelledDate.Width = 96;
            // 
            // isActive
            // 
            this.isActive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.isActive.FalseValue = "Y";
            this.isActive.FillWeight = 50F;
            this.isActive.HeaderText = "Cancel";
            this.isActive.Name = "isActive";
            this.isActive.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isActive.TrueValue = "N";
            this.isActive.Width = 50;
            // 
            // MarketListItem
            // 
            this.MarketListItem.HeaderText = "MarketListItem";
            this.MarketListItem.Name = "MarketListItem";
            this.MarketListItem.Visible = false;
            // 
            // ProductId
            // 
            this.ProductId.HeaderText = "ProductId";
            this.ProductId.Name = "ProductId";
            this.ProductId.Visible = false;
            // 
            // RequisitionId
            // 
            this.RequisitionId.HeaderText = "RequisitionId";
            this.RequisitionId.Name = "RequisitionId";
            this.RequisitionId.Visible = false;
            // 
            // RequisitionLineId
            // 
            this.RequisitionLineId.HeaderText = "RequisitionLineId";
            this.RequisitionLineId.Name = "RequisitionLineId";
            this.RequisitionLineId.Visible = false;
            // 
            // PriceInTickets
            // 
            this.PriceInTickets.HeaderText = "PriceInTickets";
            this.PriceInTickets.Name = "PriceInTickets";
            this.PriceInTickets.Visible = false;
            // 
            // taxInclusiveCost
            // 
            this.taxInclusiveCost.HeaderText = "taxInclusiveCost";
            this.taxInclusiveCost.Name = "taxInclusiveCost";
            this.taxInclusiveCost.Visible = false;
            // 
            // taxPercentage
            // 
            this.taxPercentage.HeaderText = "taxPercentage";
            this.taxPercentage.Name = "taxPercentage";
            this.taxPercentage.Visible = false;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(424, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 13);
            this.label11.TabIndex = 78;
            this.label11.Text = "Order #:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_order
            // 
            this.tb_order.Enabled = false;
            this.tb_order.Location = new System.Drawing.Point(518, 11);
            this.tb_order.Name = "tb_order";
            this.tb_order.ReadOnly = true;
            this.tb_order.Size = new System.Drawing.Size(137, 20);
            this.tb_order.TabIndex = 67;
            // 
            // tb_date
            // 
            this.tb_date.Enabled = false;
            this.tb_date.Location = new System.Drawing.Point(518, 40);
            this.tb_date.Name = "tb_date";
            this.tb_date.ReadOnly = true;
            this.tb_date.Size = new System.Drawing.Size(137, 20);
            this.tb_date.TabIndex = 68;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(424, 44);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 13);
            this.label12.TabIndex = 76;
            this.label12.Text = "Date:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(424, 72);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 13);
            this.label13.TabIndex = 75;
            this.label13.Text = "Order Status:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(267, 466);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 15);
            this.label7.TabIndex = 74;
            this.label7.Text = "Total:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(22, 164);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 13);
            this.label10.TabIndex = 73;
            this.label10.Text = "Email:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_phone
            // 
            this.tb_phone.Enabled = false;
            this.tb_phone.Location = new System.Drawing.Point(113, 130);
            this.tb_phone.Name = "tb_phone";
            this.tb_phone.ReadOnly = true;
            this.tb_phone.Size = new System.Drawing.Size(189, 20);
            this.tb_phone.TabIndex = 64;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(22, 133);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 13);
            this.label9.TabIndex = 71;
            this.label9.Text = "Phone:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_contact
            // 
            this.tb_contact.Enabled = false;
            this.tb_contact.Location = new System.Drawing.Point(113, 99);
            this.tb_contact.Name = "tb_contact";
            this.tb_contact.ReadOnly = true;
            this.tb_contact.Size = new System.Drawing.Size(189, 20);
            this.tb_contact.TabIndex = 63;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(22, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 69;
            this.label8.Text = "Contact:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cb_vendor
            // 
            this.cb_vendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cb_vendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cb_vendor.FormattingEnabled = true;
            this.cb_vendor.Location = new System.Drawing.Point(113, 68);
            this.cb_vendor.Name = "cb_vendor";
            this.cb_vendor.Size = new System.Drawing.Size(189, 21);
            this.cb_vendor.TabIndex = 62;
            this.cb_vendor.SelectionChangeCommitted += new System.EventHandler(this.cb_vendor_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(22, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 67;
            this.label4.Text = "Vendor:*";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblViewRequisitions
            // 
            this.lblViewRequisitions.AutoSize = true;
            this.lblViewRequisitions.Location = new System.Drawing.Point(720, 167);
            this.lblViewRequisitions.Name = "lblViewRequisitions";
            this.lblViewRequisitions.Size = new System.Drawing.Size(94, 13);
            this.lblViewRequisitions.TabIndex = 73;
            this.lblViewRequisitions.TabStop = true;
            this.lblViewRequisitions.Text = "Show Requisitions";
            this.lblViewRequisitions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblViewRequisitions_LinkClicked);
            // 
            // lnkRemarks
            // 
            this.lnkRemarks.AutoSize = true;
            this.lnkRemarks.Location = new System.Drawing.Point(746, 499);
            this.lnkRemarks.Name = "lnkRemarks";
            this.lnkRemarks.Size = new System.Drawing.Size(49, 13);
            this.lnkRemarks.TabIndex = 81;
            this.lnkRemarks.TabStop = true;
            this.lnkRemarks.Text = "Remarks";
            this.lnkRemarks.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRemarks_LinkClicked);
            // 
            // lblReprintCount
            // 
            this.lblReprintCount.AutoSize = true;
            this.lblReprintCount.Location = new System.Drawing.Point(349, 47);
            this.lblReprintCount.Name = "lblReprintCount";
            this.lblReprintCount.Size = new System.Drawing.Size(0, 13);
            this.lblReprintCount.TabIndex = 64;
            this.lblReprintCount.Visible = false;
            // 
            // lblDocumentStatus
            // 
            this.lblDocumentStatus.AutoSize = true;
            this.lblDocumentStatus.Location = new System.Drawing.Point(354, 30);
            this.lblDocumentStatus.Name = "lblDocumentStatus";
            this.lblDocumentStatus.Size = new System.Drawing.Size(0, 13);
            this.lblDocumentStatus.TabIndex = 63;
            this.lblDocumentStatus.Visible = false;
            // 
            // lblAmendmentNumber
            // 
            this.lblAmendmentNumber.AutoSize = true;
            this.lblAmendmentNumber.Location = new System.Drawing.Point(354, 15);
            this.lblAmendmentNumber.Name = "lblAmendmentNumber";
            this.lblAmendmentNumber.Size = new System.Drawing.Size(0, 13);
            this.lblAmendmentNumber.TabIndex = 62;
            this.lblAmendmentNumber.Visible = false;
            // 
            // lblEmail
            // 
            this.lblEmail.BackColor = System.Drawing.SystemColors.Control;
            this.lblEmail.Enabled = false;
            this.lblEmail.Location = new System.Drawing.Point(113, 160);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(189, 20);
            this.lblEmail.TabIndex = 65;
            // 
            // cmbDocumentType
            // 
            this.cmbDocumentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDocumentType.FormattingEnabled = true;
            this.cmbDocumentType.Location = new System.Drawing.Point(113, 11);
            this.cmbDocumentType.Name = "cmbDocumentType";
            this.cmbDocumentType.Size = new System.Drawing.Size(188, 21);
            this.cmbDocumentType.TabIndex = 60;
            this.cmbDocumentType.SelectedIndexChanged += new System.EventHandler(this.cmbDocumentType_SelectedIndexChanged);
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(636, 133);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(86, 13);
            this.label21.TabIndex = 59;
            this.label21.Text = "To Date:*";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd-MMM-yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(723, 130);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(106, 20);
            this.dtpToDate.TabIndex = 72;
            this.dtpToDate.CloseUp += new System.EventHandler(this.dtpToDate_CloseUp);
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(427, 133);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(86, 13);
            this.label20.TabIndex = 57;
            this.label20.Text = "From Date:*";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd-MMM-yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(519, 130);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(106, 20);
            this.dtpFromDate.TabIndex = 71;
            this.dtpFromDate.CloseUp += new System.EventHandler(this.dtpFromDate_CloseUp);
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            // 
            // chkIsCreditPO
            // 
            this.chkIsCreditPO.AutoSize = true;
            this.chkIsCreditPO.Location = new System.Drawing.Point(113, 42);
            this.chkIsCreditPO.Name = "chkIsCreditPO";
            this.chkIsCreditPO.Size = new System.Drawing.Size(15, 14);
            this.chkIsCreditPO.TabIndex = 61;
            this.chkIsCreditPO.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(22, 42);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(86, 13);
            this.label19.TabIndex = 54;
            this.label19.Text = "Is Credit PO:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(13, 13);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(96, 13);
            this.label18.TabIndex = 52;
            this.label18.Text = "Document Type:*";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_status
            // 
            this.tb_status.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tb_status.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tb_status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tb_status.FormattingEnabled = true;
            this.tb_status.Items.AddRange(new object[] {
            "Open",
            "InProgress",
            "ShortClose",
            "Received",
            "Cancelled",
            "Drop Ship"});
            this.tb_status.Location = new System.Drawing.Point(518, 69);
            this.tb_status.Name = "tb_status";
            this.tb_status.Size = new System.Drawing.Size(189, 21);
            this.tb_status.TabIndex = 69;
            this.tb_status.SelectedIndexChanged += new System.EventHandler(this.tb_status_SelectedIndexChanged);
            // 
            // tb_cancelledDate
            // 
            this.tb_cancelledDate.Enabled = false;
            this.tb_cancelledDate.Location = new System.Drawing.Point(518, 99);
            this.tb_cancelledDate.Name = "tb_cancelledDate";
            this.tb_cancelledDate.ReadOnly = true;
            this.tb_cancelledDate.Size = new System.Drawing.Size(137, 20);
            this.tb_cancelledDate.TabIndex = 70;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(424, 102);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(86, 13);
            this.label17.TabIndex = 49;
            this.label17.Text = "Cancelled Date:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(442, 261);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(137, 20);
            this.textBox1.TabIndex = 48;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(393, 264);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(40, 13);
            this.label16.TabIndex = 47;
            this.label16.Text = "Status:";
            // 
            // tb_total
            // 
            this.tb_total.Enabled = false;
            this.tb_total.Location = new System.Drawing.Point(332, 463);
            this.tb_total.Name = "tb_total";
            this.tb_total.ReadOnly = true;
            this.tb_total.Size = new System.Drawing.Size(102, 20);
            this.tb_total.TabIndex = 201;
            this.tb_total.Text = "0";
            this.tb_total.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cb_autoorder
            // 
            this.cb_autoorder.BackColor = System.Drawing.Color.White;
            this.cb_autoorder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_autoorder.Image = global::Semnox.Parafait.Inventory.Properties.Resources.autoorder;
            this.cb_autoorder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_autoorder.Location = new System.Drawing.Point(319, 66);
            this.cb_autoorder.Name = "cb_autoorder";
            this.cb_autoorder.Size = new System.Drawing.Size(89, 23);
            this.cb_autoorder.TabIndex = 32;
            this.cb_autoorder.Text = "Auto Order";
            this.cb_autoorder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cb_autoorder.UseVisualStyleBackColor = false;
            this.cb_autoorder.Visible = false;
            // 
            // btnEmail
            // 
            this.btnEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEmail.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnEmail.Image = global::Semnox.Parafait.Inventory.Properties.Resources.emailButton;
            this.btnEmail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmail.Location = new System.Drawing.Point(579, 558);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(90, 23);
            this.btnEmail.TabIndex = 86;
            this.btnEmail.Text = "eMail";
            this.btnEmail.UseVisualStyleBackColor = true;
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            // 
            // btnReceipts
            // 
            this.btnReceipts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReceipts.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReceipts.Image = global::Semnox.Parafait.Inventory.Properties.Resources.duplicate;
            this.btnReceipts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReceipts.Location = new System.Drawing.Point(256, 557);
            this.btnReceipts.Name = "btnReceipts";
            this.btnReceipts.Size = new System.Drawing.Size(95, 23);
            this.btnReceipts.TabIndex = 83;
            this.btnReceipts.Text = "Receipts";
            this.btnReceipts.UseVisualStyleBackColor = true;
            this.btnReceipts.Click += new System.EventHandler(this.btnReceipts_Click);
            // 
            // cb_print
            // 
            this.cb_print.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_print.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_print.Image = global::Semnox.Parafait.Inventory.Properties.Resources.printer;
            this.cb_print.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_print.Location = new System.Drawing.Point(473, 557);
            this.cb_print.Name = "cb_print";
            this.cb_print.Size = new System.Drawing.Size(90, 23);
            this.cb_print.TabIndex = 85;
            this.cb_print.Text = "Print";
            this.cb_print.UseVisualStyleBackColor = true;
            this.cb_print.Click += new System.EventHandler(this.cb_print_Click);
            // 
            // cb_create
            // 
            this.cb_create.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_create.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_create.Image = global::Semnox.Parafait.Inventory.Properties.Resources._new;
            this.cb_create.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_create.Location = new System.Drawing.Point(687, 557);
            this.cb_create.Name = "cb_create";
            this.cb_create.Size = new System.Drawing.Size(90, 23);
            this.cb_create.TabIndex = 87;
            this.cb_create.Text = "New";
            this.cb_create.UseVisualStyleBackColor = true;
            this.cb_create.Click += new System.EventHandler(this.cb_create_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Semnox.Parafait.Inventory.Properties.Resources.parsmallest;
            this.pictureBox1.Location = new System.Drawing.Point(9, 549);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(81, 38);
            this.pictureBox1.TabIndex = 33;
            this.pictureBox1.TabStop = false;
            // 
            // cb_prodsearch
            // 
            this.cb_prodsearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_prodsearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_prodsearch.Image = global::Semnox.Parafait.Inventory.Properties.Resources.search;
            this.cb_prodsearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_prodsearch.Location = new System.Drawing.Point(123, 68);
            this.cb_prodsearch.Name = "cb_prodsearch";
            this.cb_prodsearch.Size = new System.Drawing.Size(88, 23);
            this.cb_prodsearch.TabIndex = 15;
            this.cb_prodsearch.Text = "Search";
            this.cb_prodsearch.UseVisualStyleBackColor = true;
            this.cb_prodsearch.Click += new System.EventHandler(this.cb_prodsearch_Click);
            // 
            // dd_qty
            // 
            this.dd_qty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dd_qty.FormattingEnabled = true;
            this.dd_qty.Items.AddRange(new object[] {
            ">",
            "=",
            "<"});
            this.dd_qty.Location = new System.Drawing.Point(91, 42);
            this.dd_qty.Name = "dd_qty";
            this.dd_qty.Size = new System.Drawing.Size(38, 21);
            this.dd_qty.TabIndex = 13;
            // 
            // txt_qty
            // 
            this.txt_qty.Location = new System.Drawing.Point(137, 43);
            this.txt_qty.Name = "txt_qty";
            this.txt_qty.Size = new System.Drawing.Size(75, 20);
            this.txt_qty.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Available Qty:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Rsearch_dgv
            // 
            this.Rsearch_dgv.AllowUserToAddRows = false;
            this.Rsearch_dgv.AllowUserToDeleteRows = false;
            this.Rsearch_dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.Rsearch_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Rsearch_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.AvailQty,
            this.Product_Id});
            this.Rsearch_dgv.GridColor = System.Drawing.Color.LightSlateGray;
            this.Rsearch_dgv.Location = new System.Drawing.Point(6, 95);
            this.Rsearch_dgv.MultiSelect = false;
            this.Rsearch_dgv.Name = "Rsearch_dgv";
            this.Rsearch_dgv.ReadOnly = true;
            this.Rsearch_dgv.RowHeadersVisible = false;
            this.Rsearch_dgv.RowHeadersWidth = 4;
            this.Rsearch_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Rsearch_dgv.Size = new System.Drawing.Size(208, 136);
            this.Rsearch_dgv.TabIndex = 16;
            this.Rsearch_dgv.TabStop = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Code";
            this.dataGridViewTextBoxColumn1.HeaderText = "Product Code";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // AvailQty
            // 
            this.AvailQty.DataPropertyName = "Quantity";
            this.AvailQty.HeaderText = "Available Quantity";
            this.AvailQty.Name = "AvailQty";
            this.AvailQty.ReadOnly = true;
            // 
            // Product_Id
            // 
            this.Product_Id.DataPropertyName = "ProductId";
            this.Product_Id.HeaderText = "ProductId";
            this.Product_Id.Name = "Product_Id";
            this.Product_Id.ReadOnly = true;
            this.Product_Id.Visible = false;
            // 
            // txt_prodcode
            // 
            this.txt_prodcode.Location = new System.Drawing.Point(91, 17);
            this.txt_prodcode.Name = "txt_prodcode";
            this.txt_prodcode.Size = new System.Drawing.Size(119, 20);
            this.txt_prodcode.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Product Code:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.dd_qty);
            this.groupBox1.Controls.Add(this.cb_prodsearch);
            this.groupBox1.Controls.Add(this.txt_qty);
            this.groupBox1.Controls.Add(this.Rsearch_dgv);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txt_prodcode);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(4, 314);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(219, 235);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Product Search";
            // 
            // taxId
            // 
            this.taxId.HeaderText = "taxId";
            this.taxId.Name = "taxId";
            this.taxId.Visible = false;
            this.taxId.Width = 55;
            // 
            // frmOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1175, 591);
            this.Controls.Add(this.btnEmail);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cb_print);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnReceipts);
            this.Controls.Add(this.gb_search);
            this.Controls.Add(this.cb_create);
            this.Controls.Add(this.cb_submit);
            this.Controls.Add(this.cb_amendment);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gb_order);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Order";
            this.Activated += new System.EventHandler(this.frm_Order_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Order_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_Order_FormClosed);
            this.Load += new System.EventHandler(this.frm_Order_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vendorBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchorder_dgv)).EndInit();
            this.gb_search.ResumeLayout(false);
            this.gb_search.PerformLayout();
            this.gb_order.ResumeLayout(false);
            this.gb_order.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.order_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rsearch_dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_searchorder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_searchstatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView searchorder_dgv;
        private System.Windows.Forms.GroupBox gb_search;
        private System.Windows.Forms.Button cb_search;
        private System.Windows.Forms.Button cb_submit;
        private System.Windows.Forms.Button cb_amendment;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gb_order;
        private System.Windows.Forms.PictureBox pictureBox1;
        // private RedemptionDataSet redemptionDataSet;
        private System.Windows.Forms.BindingSource vendorBindingSource;
        //private Redemption.RedemptionDataSetTableAdapters.VendorTableAdapter vendorTableAdapter;
        private System.Windows.Forms.Button cb_prodsearch;
        private System.Windows.Forms.ComboBox dd_qty;
        private System.Windows.Forms.TextBox txt_qty;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView Rsearch_dgv;
        private System.Windows.Forms.TextBox txt_prodcode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_total;
        private System.Windows.Forms.Label lb_orderid;
        private System.Windows.Forms.Button cb_create;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button cb_print;
        private System.Windows.Forms.Button cb_autoorder;
        private System.Windows.Forms.BindingSource vendorBindingSource1;
        private System.Windows.Forms.Button btnReceipts;
        private System.Windows.Forms.Button btnEmail;
        private System.Windows.Forms.TextBox txtProductPOSearch;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tb_cancelledDate;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox chkIsCreditPO;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox tb_status;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.ComboBox cmbDocumentType;
        private System.Windows.Forms.TextBox lblEmail;
        private System.Windows.Forms.Label lblAmendmentNumber;
        private System.Windows.Forms.Label lblDocumentStatus;
        private System.Windows.Forms.Label lblReprintCount;
        private System.Windows.Forms.ComboBox cmbOrderType;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.LinkLabel lnkRemarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn PurchaseOrderId;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Product_Id;
        private System.Windows.Forms.LinkLabel lblViewRequisitions;
        private System.Windows.Forms.Label label4;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cb_vendor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_contact;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_phone;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tb_date;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tb_order;
        private System.Windows.Forms.DataGridView order_dgv;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tb_TotalLandedCost;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tb_totalLogisticsCost;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tb_TotalQuantity;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox tb_TotalRetail;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox tb_MarkupPercent;
        //private System.Windows.Forms.DataGridViewComboBoxColumn cbTaxCode;
        private System.Windows.Forms.Label lblTosite;
        private System.Windows.Forms.ComboBox cmbToSite;
        private System.Windows.Forms.TextBox txtTotalTax;
        private System.Windows.Forms.LinkLabel lblViewTotalTax;

        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cb_searchvendor;
        private DataGridViewTextBoxColumn taxId;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewComboBoxColumn cmbUOM;
        private DataGridViewTextBoxColumn Quantity;
        private DataGridViewTextBoxColumn RequiredByDate;
        private DataGridViewTextBoxColumn Cost;
        private DataGridViewTextBoxColumn UnitLogisticsCost;
        private DataGridViewComboBoxColumn TaxCode;
        private DataGridViewTextBoxColumn TaxAmount;
        private DataGridViewTextBoxColumn DiscountPercentage;
        private DataGridViewTextBoxColumn SubTotal;
        private DataGridViewTextBoxColumn LowerLimitCost;
        private DataGridViewTextBoxColumn UpperLimitCost;
        private DataGridViewTextBoxColumn CostVariancePercent;
        private DataGridViewTextBoxColumn OrigPrice;
        private DataGridViewTextBoxColumn LineId;
        private DataGridViewTextBoxColumn CancelledDate;
        private DataGridViewCheckBoxColumn isActive;
        private DataGridViewTextBoxColumn ReceiveQuantity;
        private DataGridViewTextBoxColumn MarketListItem;
        private DataGridViewTextBoxColumn ProductId;
        private DataGridViewTextBoxColumn RequisitionId;
        private DataGridViewTextBoxColumn RequisitionLineId;
        private DataGridViewTextBoxColumn PriceInTickets;
        private DataGridViewTextBoxColumn taxInclusiveCost;
        private DataGridViewTextBoxColumn taxPercentage;
        private DataGridViewLinkColumn stockLink;
    }
}