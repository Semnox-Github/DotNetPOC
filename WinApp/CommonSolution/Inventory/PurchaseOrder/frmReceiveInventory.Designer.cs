namespace Semnox.Parafait.Inventory
{
    partial class frmReceiveInventory
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.receive_dgv = new System.Windows.Forms.DataGridView();
            this.txt_remarks = new System.Windows.Forms.RichTextBox();
            this.cb_complete = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Rsearch_dgv = new System.Windows.Forms.DataGridView();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProdID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_prodcode = new System.Windows.Forms.TextBox();
            this.txt_qty = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dd_qty = new System.Windows.Forms.ComboBox();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.cb_search = new System.Windows.Forms.Button();
            this.gb_receive = new System.Windows.Forms.GroupBox();
            this.lb_orderid = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lnkReceiveAll = new System.Windows.Forms.LinkLabel();
            this.lblViewRequisitions = new System.Windows.Forms.LinkLabel();
            this.txtReceiptId = new System.Windows.Forms.Label();
            this.lblOrderDocumentType = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lnkApplyTaxToAllLines = new System.Windows.Forms.LinkLabel();
            this.tb_total = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tb_status = new System.Windows.Forms.TextBox();
            this.dtpReceiveDate = new System.Windows.Forms.DateTimePicker();
            this.label19 = new System.Windows.Forms.Label();
            this.cmbDefaultTax = new System.Windows.Forms.ComboBox();
            this.cmbVendor = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.tb_contact = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tb_phone = new System.Windows.Forms.TextBox();
            this.txtGRN = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tb_date = new System.Windows.Forms.TextBox();
            this.tb_order = new System.Windows.Forms.TextBox();
            this.txtGatePassNo = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtVendorBillNo = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_exit = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gbfilter = new System.Windows.Forms.GroupBox();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.cb_POsearch = new System.Windows.Forms.Button();
            this.tb_searchorder = new System.Windows.Forms.TextBox();
            this.cb_searchstatus = new System.Windows.Forms.ComboBox();
            this.cmbSearchVendor = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.searchorder_dgv = new System.Windows.Forms.DataGridView();
            this.PurchaseorderID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cb_create = new System.Windows.Forms.Button();
            this.btnReceipts = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtUOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbUOM = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TaxPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxInclusive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TaxAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequiredByDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsReceived = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PurchaseOrderLineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LowerLimitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpperLimitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostVariancePercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrigPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isLottable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpiryType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpiryDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequisitionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequisitionLineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lot = new System.Windows.Forms.DataGridViewButtonColumn();
            this.recvLocation = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.PriceInTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockLink = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.receive_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rsearch_dgv)).BeginInit();
            this.gbSearch.SuspendLayout();
            this.gb_receive.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gbfilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchorder_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // receive_dgv
            // 
            this.receive_dgv.AllowUserToDeleteRows = false;
            this.receive_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.receive_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.receive_dgv.ColumnHeadersHeight = 50;
            this.receive_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.receive_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductCode,
            this.Description,
            this.txtUOM,
            this.Qty,
            this.cmbUOM,
            this.Price,
            this.TaxId,
            this.TaxPercentage,
            this.TaxInclusive,
            this.TaxAmount,
            this.Amount,
            this.RequiredByDate,
            this.IsReceived,
            this.OrderedQty,
            this.PurchaseOrderLineId,
            this.LowerLimitCost,
            this.UpperLimitCost,
            this.CostVariancePercent,
            this.OrigPrice,
            this.isLottable,
            this.ExpiryType,
            this.ExpiryDays,
            this.ProductId,
            this.RequisitionId,
            this.RequisitionLineId,
            this.Lot,
            this.recvLocation,
            this.PriceInTickets,
            this.StockLink});
            this.receive_dgv.GridColor = System.Drawing.Color.Khaki;
            this.receive_dgv.Location = new System.Drawing.Point(9, 200);
            this.receive_dgv.Name = "receive_dgv";
            this.receive_dgv.RowHeadersVisible = false;
            this.receive_dgv.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.receive_dgv.Size = new System.Drawing.Size(787, 246);
            this.receive_dgv.TabIndex = 37;
            this.receive_dgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.receive_dgv_CellContentClick);
            this.receive_dgv.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.receive_dgv_CellValidating);
            this.receive_dgv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.receive_dgv_CellValueChanged);
            this.receive_dgv.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.receive_dgv_DataError);
            this.receive_dgv.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.receive_dgv_DefaultValuesNeeded);
            this.receive_dgv.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.receive_dgv_EditingControlShowing);
            this.receive_dgv.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.receive_dgv_UserDeletingRow);
            // 
            // txt_remarks
            // 
            this.txt_remarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_remarks.Location = new System.Drawing.Point(343, 501);
            this.txt_remarks.Name = "txt_remarks";
            this.txt_remarks.Size = new System.Drawing.Size(531, 31);
            this.txt_remarks.TabIndex = 40;
            this.txt_remarks.Text = "";
            // 
            // cb_complete
            // 
            this.cb_complete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_complete.Image = global::Semnox.Parafait.Inventory.Properties.Resources.save;
            this.cb_complete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_complete.Location = new System.Drawing.Point(914, 561);
            this.cb_complete.Name = "cb_complete";
            this.cb_complete.Size = new System.Drawing.Size(86, 23);
            this.cb_complete.TabIndex = 42;
            this.cb_complete.Tag = "Complete";
            this.cb_complete.Text = "Complete";
            this.cb_complete.UseVisualStyleBackColor = true;
            this.cb_complete.Click += new System.EventHandler(this.cb_complete_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(41, 481);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Remarks:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Rsearch_dgv
            // 
            this.Rsearch_dgv.AllowUserToAddRows = false;
            this.Rsearch_dgv.AllowUserToDeleteRows = false;
            this.Rsearch_dgv.BackgroundColor = System.Drawing.SystemColors.Control;
            this.Rsearch_dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Rsearch_dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.Rsearch_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Rsearch_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.Quantity,
            this.Desc,
            this.ProdID});
            this.Rsearch_dgv.GridColor = System.Drawing.Color.Khaki;
            this.Rsearch_dgv.Location = new System.Drawing.Point(10, 100);
            this.Rsearch_dgv.MultiSelect = false;
            this.Rsearch_dgv.Name = "Rsearch_dgv";
            this.Rsearch_dgv.RowHeadersVisible = false;
            this.Rsearch_dgv.RowHeadersWidth = 4;
            this.Rsearch_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Rsearch_dgv.Size = new System.Drawing.Size(203, 102);
            this.Rsearch_dgv.TabIndex = 16;
            this.Rsearch_dgv.TabStop = false;
            // 
            // Code
            // 
            this.Code.DataPropertyName = "Code";
            this.Code.HeaderText = "Product Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "Quantity";
            this.Quantity.HeaderText = "Available Quantity";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            // 
            // Desc
            // 
            this.Desc.DataPropertyName = "Desc.";
            this.Desc.HeaderText = "Desc.";
            this.Desc.Name = "Desc";
            this.Desc.ReadOnly = true;
            // 
            // ProdID
            // 
            this.ProdID.DataPropertyName = "ProductID";
            this.ProdID.HeaderText = "ProdID";
            this.ProdID.Name = "ProdID";
            this.ProdID.Visible = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Product Code:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_prodcode
            // 
            this.txt_prodcode.Location = new System.Drawing.Point(92, 19);
            this.txt_prodcode.Name = "txt_prodcode";
            this.txt_prodcode.Size = new System.Drawing.Size(119, 20);
            this.txt_prodcode.TabIndex = 11;
            // 
            // txt_qty
            // 
            this.txt_qty.Location = new System.Drawing.Point(136, 45);
            this.txt_qty.Name = "txt_qty";
            this.txt_qty.Size = new System.Drawing.Size(75, 20);
            this.txt_qty.TabIndex = 14;
            this.txt_qty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_qty_KeyPress);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Available Qty:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dd_qty
            // 
            this.dd_qty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dd_qty.FormattingEnabled = true;
            this.dd_qty.Items.AddRange(new object[] {
            ">",
            "=",
            "<"});
            this.dd_qty.Location = new System.Drawing.Point(92, 44);
            this.dd_qty.Name = "dd_qty";
            this.dd_qty.Size = new System.Drawing.Size(38, 21);
            this.dd_qty.TabIndex = 13;
            // 
            // gbSearch
            // 
            this.gbSearch.BackColor = System.Drawing.Color.Transparent;
            this.gbSearch.Controls.Add(this.cb_search);
            this.gbSearch.Controls.Add(this.Rsearch_dgv);
            this.gbSearch.Controls.Add(this.txt_prodcode);
            this.gbSearch.Controls.Add(this.label2);
            this.gbSearch.Controls.Add(this.label3);
            this.gbSearch.Controls.Add(this.txt_qty);
            this.gbSearch.Controls.Add(this.dd_qty);
            this.gbSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gbSearch.Location = new System.Drawing.Point(4, 335);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(217, 208);
            this.gbSearch.TabIndex = 9;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Search Products";
            // 
            // cb_search
            // 
            this.cb_search.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_search.Image = global::Semnox.Parafait.Inventory.Properties.Resources.search;
            this.cb_search.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_search.Location = new System.Drawing.Point(118, 70);
            this.cb_search.Name = "cb_search";
            this.cb_search.Size = new System.Drawing.Size(95, 23);
            this.cb_search.TabIndex = 15;
            this.cb_search.Text = "Search";
            this.cb_search.UseVisualStyleBackColor = true;
            this.cb_search.Click += new System.EventHandler(this.cb_search_Click);
            // 
            // gb_receive
            // 
            this.gb_receive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_receive.BackColor = System.Drawing.Color.Transparent;
            this.gb_receive.Controls.Add(this.lb_orderid);
            this.gb_receive.Controls.Add(this.label12);
            this.gb_receive.Controls.Add(this.label1);
            this.gb_receive.Controls.Add(this.receive_dgv);
            this.gb_receive.Controls.Add(this.panel2);
            this.gb_receive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_receive.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gb_receive.Location = new System.Drawing.Point(227, 4);
            this.gb_receive.Name = "gb_receive";
            this.gb_receive.Size = new System.Drawing.Size(805, 539);
            this.gb_receive.TabIndex = 17;
            this.gb_receive.TabStop = false;
            this.gb_receive.Text = "Receive Products";
            // 
            // lb_orderid
            // 
            this.lb_orderid.AutoSize = true;
            this.lb_orderid.BackColor = System.Drawing.Color.White;
            this.lb_orderid.Location = new System.Drawing.Point(438, 17);
            this.lb_orderid.Name = "lb_orderid";
            this.lb_orderid.Size = new System.Drawing.Size(81, 13);
            this.lb_orderid.TabIndex = 55;
            this.lb_orderid.Text = "Hidden Order id";
            this.lb_orderid.Visible = false;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(316, 60);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 13);
            this.label12.TabIndex = 33;
            this.label12.Text = "PO Date:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.lnkReceiveAll);
            this.panel2.Controls.Add(this.lblViewRequisitions);
            this.panel2.Controls.Add(this.txtReceiptId);
            this.panel2.Controls.Add(this.lblOrderDocumentType);
            this.panel2.Controls.Add(this.cmbLocation);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.lnkApplyTaxToAllLines);
            this.panel2.Controls.Add(this.tb_total);
            this.panel2.Controls.Add(this.label20);
            this.panel2.Controls.Add(this.label18);
            this.panel2.Controls.Add(this.tb_status);
            this.panel2.Controls.Add(this.dtpReceiveDate);
            this.panel2.Controls.Add(this.label19);
            this.panel2.Controls.Add(this.cmbDefaultTax);
            this.panel2.Controls.Add(this.cmbVendor);
            this.panel2.Controls.Add(this.tb_contact);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.tb_phone);
            this.panel2.Controls.Add(this.txtGRN);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.tb_date);
            this.panel2.Controls.Add(this.tb_order);
            this.panel2.Controls.Add(this.txtGatePassNo);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.txtVendorBillNo);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Location = new System.Drawing.Point(3, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(796, 516);
            this.panel2.TabIndex = 34;
            // 
            // lnkReceiveAll
            // 
            this.lnkReceiveAll.AutoSize = true;
            this.lnkReceiveAll.Location = new System.Drawing.Point(681, 160);
            this.lnkReceiveAll.Name = "lnkReceiveAll";
            this.lnkReceiveAll.Size = new System.Drawing.Size(61, 13);
            this.lnkReceiveAll.TabIndex = 34;
            this.lnkReceiveAll.TabStop = true;
            this.lnkReceiveAll.Text = "Receive All";
            this.lnkReceiveAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkReceiveAll_LinkClicked);
            // 
            // lblViewRequisitions
            // 
            this.lblViewRequisitions.AutoSize = true;
            this.lblViewRequisitions.Location = new System.Drawing.Point(72, 131);
            this.lblViewRequisitions.Name = "lblViewRequisitions";
            this.lblViewRequisitions.Size = new System.Drawing.Size(94, 13);
            this.lblViewRequisitions.TabIndex = 35;
            this.lblViewRequisitions.TabStop = true;
            this.lblViewRequisitions.Text = "Show Requisitions";
            this.lblViewRequisitions.Visible = false;
            this.lblViewRequisitions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblViewRequisitions_LinkClicked);
            // 
            // txtReceiptId
            // 
            this.txtReceiptId.AutoSize = true;
            this.txtReceiptId.Location = new System.Drawing.Point(98, 146);
            this.txtReceiptId.Name = "txtReceiptId";
            this.txtReceiptId.Size = new System.Drawing.Size(0, 13);
            this.txtReceiptId.TabIndex = 78;
            this.txtReceiptId.Visible = false;
            // 
            // lblOrderDocumentType
            // 
            this.lblOrderDocumentType.AutoSize = true;
            this.lblOrderDocumentType.Location = new System.Drawing.Point(37, 139);
            this.lblOrderDocumentType.Name = "lblOrderDocumentType";
            this.lblOrderDocumentType.Size = new System.Drawing.Size(0, 13);
            this.lblOrderDocumentType.TabIndex = 77;
            this.lblOrderDocumentType.Visible = false;
            // 
            // cmbLocation
            // 
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(83, 94);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(188, 21);
            this.cmbLocation.TabIndex = 24;
            this.cmbLocation.SelectedIndexChanged += new System.EventHandler(this.cmbLocation_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(7, 98);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 75;
            this.label10.Text = "Location:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lnkApplyTaxToAllLines
            // 
            this.lnkApplyTaxToAllLines.AutoSize = true;
            this.lnkApplyTaxToAllLines.Location = new System.Drawing.Point(567, 160);
            this.lnkApplyTaxToAllLines.Name = "lnkApplyTaxToAllLines";
            this.lnkApplyTaxToAllLines.Size = new System.Drawing.Size(86, 13);
            this.lnkApplyTaxToAllLines.TabIndex = 33;
            this.lnkApplyTaxToAllLines.TabStop = true;
            this.lnkApplyTaxToAllLines.Text = "Apply to all Lines";
            this.lnkApplyTaxToAllLines.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkApplyTaxToAllLines_LinkClicked);
            // 
            // tb_total
            // 
            this.tb_total.Enabled = false;
            this.tb_total.Location = new System.Drawing.Point(642, 438);
            this.tb_total.Name = "tb_total";
            this.tb_total.ReadOnly = true;
            this.tb_total.Size = new System.Drawing.Size(102, 20);
            this.tb_total.TabIndex = 73;
            this.tb_total.Text = "0";
            this.tb_total.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(550, 441);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(86, 13);
            this.label20.TabIndex = 72;
            this.label20.Text = "Total:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(313, 69);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(64, 13);
            this.label18.TabIndex = 57;
            this.label18.Text = "Status:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_status
            // 
            this.tb_status.Enabled = false;
            this.tb_status.Location = new System.Drawing.Point(388, 66);
            this.tb_status.Name = "tb_status";
            this.tb_status.ReadOnly = true;
            this.tb_status.Size = new System.Drawing.Size(83, 20);
            this.tb_status.TabIndex = 27;
            // 
            // dtpReceiveDate
            // 
            this.dtpReceiveDate.CustomFormat = "dd-MMM-yyyy";
            this.dtpReceiveDate.Enabled = false;
            this.dtpReceiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpReceiveDate.Location = new System.Drawing.Point(627, 15);
            this.dtpReceiveDate.Name = "dtpReceiveDate";
            this.dtpReceiveDate.Size = new System.Drawing.Size(117, 20);
            this.dtpReceiveDate.TabIndex = 28;
            this.dtpReceiveDate.ValueChanged += new System.EventHandler(this.dtpReceiveDate_ValueChanged);
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(531, 127);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(86, 13);
            this.label19.TabIndex = 70;
            this.label19.Text = "Default Tax:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbDefaultTax
            // 
            this.cmbDefaultTax.DisplayMember = "TaxId";
            this.cmbDefaultTax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefaultTax.FormattingEnabled = true;
            this.cmbDefaultTax.Location = new System.Drawing.Point(627, 124);
            this.cmbDefaultTax.Name = "cmbDefaultTax";
            this.cmbDefaultTax.Size = new System.Drawing.Size(117, 21);
            this.cmbDefaultTax.TabIndex = 32;
            this.cmbDefaultTax.ValueMember = "TaxId";
            // 
            // cmbVendor
            // 
            this.cmbVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbVendor.FormattingEnabled = true;
            this.cmbVendor.Location = new System.Drawing.Point(82, 14);
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.Size = new System.Drawing.Size(189, 21);
            this.cmbVendor.TabIndex = 19;
            this.cmbVendor.SelectionChangeCommitted += new System.EventHandler(this.cb_vendor_SelectionChangeCommitted);
            // 
            // tb_contact
            // 
            this.tb_contact.Enabled = false;
            this.tb_contact.Location = new System.Drawing.Point(82, 39);
            this.tb_contact.Name = "tb_contact";
            this.tb_contact.ReadOnly = true;
            this.tb_contact.Size = new System.Drawing.Size(189, 20);
            this.tb_contact.TabIndex = 21;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(531, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 13);
            this.label13.TabIndex = 63;
            this.label13.Text = "Receipt Date:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_phone
            // 
            this.tb_phone.Enabled = false;
            this.tb_phone.Location = new System.Drawing.Point(82, 66);
            this.tb_phone.Name = "tb_phone";
            this.tb_phone.ReadOnly = true;
            this.tb_phone.Size = new System.Drawing.Size(189, 20);
            this.tb_phone.TabIndex = 23;
            // 
            // txtGRN
            // 
            this.txtGRN.Location = new System.Drawing.Point(627, 41);
            this.txtGRN.Name = "txtGRN";
            this.txtGRN.ReadOnly = true;
            this.txtGRN.Size = new System.Drawing.Size(117, 20);
            this.txtGRN.TabIndex = 29;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(531, 45);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(86, 13);
            this.label16.TabIndex = 61;
            this.label16.Text = "GRN:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_date
            // 
            this.tb_date.Enabled = false;
            this.tb_date.Location = new System.Drawing.Point(388, 39);
            this.tb_date.Name = "tb_date";
            this.tb_date.ReadOnly = true;
            this.tb_date.Size = new System.Drawing.Size(83, 20);
            this.tb_date.TabIndex = 26;
            // 
            // tb_order
            // 
            this.tb_order.Enabled = false;
            this.tb_order.Location = new System.Drawing.Point(388, 14);
            this.tb_order.Name = "tb_order";
            this.tb_order.ReadOnly = true;
            this.tb_order.Size = new System.Drawing.Size(83, 20);
            this.tb_order.TabIndex = 25;
            // 
            // txtGatePassNo
            // 
            this.txtGatePassNo.Location = new System.Drawing.Point(627, 70);
            this.txtGatePassNo.Name = "txtGatePassNo";
            this.txtGatePassNo.Size = new System.Drawing.Size(117, 20);
            this.txtGatePassNo.TabIndex = 30;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(531, 74);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(86, 13);
            this.label15.TabIndex = 59;
            this.label15.Text = "Gate Pass No:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtVendorBillNo
            // 
            this.txtVendorBillNo.Location = new System.Drawing.Point(627, 96);
            this.txtVendorBillNo.Name = "txtVendorBillNo";
            this.txtVendorBillNo.Size = new System.Drawing.Size(117, 20);
            this.txtVendorBillNo.TabIndex = 31;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(527, 100);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(90, 13);
            this.label14.TabIndex = 57;
            this.label14.Text = "Bill / Invoice No:*";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(313, 17);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Order #:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(7, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Vendor:*";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(7, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Phone:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(7, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Contact:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cb_exit
            // 
            this.cb_exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cb_exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_exit.Image = global::Semnox.Parafait.Inventory.Properties.Resources.cancel;
            this.cb_exit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_exit.Location = new System.Drawing.Point(691, 561);
            this.cb_exit.Name = "cb_exit";
            this.cb_exit.Size = new System.Drawing.Size(85, 23);
            this.cb_exit.TabIndex = 45;
            this.cb_exit.Text = "Exit";
            this.cb_exit.UseVisualStyleBackColor = true;
            this.cb_exit.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Semnox.Parafait.Inventory.Properties.Resources.parsmallest;
            this.pictureBox1.Location = new System.Drawing.Point(9, 549);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(81, 38);
            this.pictureBox1.TabIndex = 32;
            this.pictureBox1.TabStop = false;
            // 
            // gbfilter
            // 
            this.gbfilter.Controls.Add(this.txtProductCode);
            this.gbfilter.Controls.Add(this.label21);
            this.gbfilter.Controls.Add(this.cb_POsearch);
            this.gbfilter.Controls.Add(this.tb_searchorder);
            this.gbfilter.Controls.Add(this.cb_searchstatus);
            this.gbfilter.Controls.Add(this.cmbSearchVendor);
            this.gbfilter.Controls.Add(this.label4);
            this.gbfilter.Controls.Add(this.searchorder_dgv);
            this.gbfilter.Controls.Add(this.label5);
            this.gbfilter.Controls.Add(this.label6);
            this.gbfilter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gbfilter.Location = new System.Drawing.Point(3, 4);
            this.gbfilter.Name = "gbfilter";
            this.gbfilter.Size = new System.Drawing.Size(218, 325);
            this.gbfilter.TabIndex = 0;
            this.gbfilter.TabStop = false;
            this.gbfilter.Text = "PO Search";
            // 
            // txtProductCode
            // 
            this.txtProductCode.BackColor = System.Drawing.SystemColors.Window;
            this.txtProductCode.Location = new System.Drawing.Point(60, 90);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(151, 20);
            this.txtProductCode.TabIndex = 7;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(6, 94);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(48, 13);
            this.label21.TabIndex = 9;
            this.label21.Text = "Product:";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cb_POsearch
            // 
            this.cb_POsearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_POsearch.Image = global::Semnox.Parafait.Inventory.Properties.Resources.search;
            this.cb_POsearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_POsearch.Location = new System.Drawing.Point(119, 117);
            this.cb_POsearch.Name = "cb_POsearch";
            this.cb_POsearch.Size = new System.Drawing.Size(93, 23);
            this.cb_POsearch.TabIndex = 8;
            this.cb_POsearch.Text = "Search";
            this.cb_POsearch.UseVisualStyleBackColor = true;
            this.cb_POsearch.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // tb_searchorder
            // 
            this.tb_searchorder.BackColor = System.Drawing.SystemColors.Window;
            this.tb_searchorder.Location = new System.Drawing.Point(60, 17);
            this.tb_searchorder.Name = "tb_searchorder";
            this.tb_searchorder.Size = new System.Drawing.Size(151, 20);
            this.tb_searchorder.TabIndex = 2;
            // 
            // cb_searchstatus
            // 
            this.cb_searchstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_searchstatus.FormattingEnabled = true;
            this.cb_searchstatus.Items.AddRange(new object[] {
            "",
            "Open",
            "InProgress"});
            this.cb_searchstatus.Location = new System.Drawing.Point(60, 40);
            this.cb_searchstatus.Name = "cb_searchstatus";
            this.cb_searchstatus.Size = new System.Drawing.Size(151, 21);
            this.cb_searchstatus.TabIndex = 4;
            // 
            // cmbSearchVendor
            // 
            this.cmbSearchVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSearchVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSearchVendor.FormattingEnabled = true;
            this.cmbSearchVendor.Location = new System.Drawing.Point(60, 65);
            this.cmbSearchVendor.Name = "cmbSearchVendor";
            this.cmbSearchVendor.Size = new System.Drawing.Size(151, 21);
            this.cmbSearchVendor.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Vendor:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // searchorder_dgv
            // 
            this.searchorder_dgv.AllowUserToAddRows = false;
            this.searchorder_dgv.AllowUserToDeleteRows = false;
            this.searchorder_dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.searchorder_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.searchorder_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PurchaseorderID,
            this.OrderNumber,
            this.OrderStatus,
            this.DocumentStatus});
            this.searchorder_dgv.GridColor = System.Drawing.Color.Khaki;
            this.searchorder_dgv.Location = new System.Drawing.Point(10, 148);
            this.searchorder_dgv.Name = "searchorder_dgv";
            this.searchorder_dgv.RowHeadersVisible = false;
            this.searchorder_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.searchorder_dgv.Size = new System.Drawing.Size(201, 171);
            this.searchorder_dgv.TabIndex = 9;
            this.searchorder_dgv.SelectionChanged += new System.EventHandler(this.searchorder_dgv_SelectionChanged);
            // 
            // PurchaseorderID
            // 
            this.PurchaseorderID.DataPropertyName = "PurchaseorderID";
            this.PurchaseorderID.HeaderText = "PurchaseorderID";
            this.PurchaseorderID.Name = "PurchaseorderID";
            this.PurchaseorderID.Visible = false;
            // 
            // OrderNumber
            // 
            this.OrderNumber.DataPropertyName = "OrderNumber";
            this.OrderNumber.HeaderText = "PO Number";
            this.OrderNumber.Name = "OrderNumber";
            this.OrderNumber.ReadOnly = true;
            // 
            // OrderStatus
            // 
            this.OrderStatus.DataPropertyName = "OrderStatus";
            this.OrderStatus.HeaderText = "PO Status";
            this.OrderStatus.Name = "OrderStatus";
            this.OrderStatus.ReadOnly = true;
            // 
            // DocumentStatus
            // 
            this.DocumentStatus.DataPropertyName = "DocumentStatus";
            this.DocumentStatus.HeaderText = "Document Status";
            this.DocumentStatus.Name = "DocumentStatus";
            this.DocumentStatus.ReadOnly = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Order #:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Status:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cb_create
            // 
            this.cb_create.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_create.Image = global::Semnox.Parafait.Inventory.Properties.Resources._new;
            this.cb_create.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_create.Location = new System.Drawing.Point(800, 561);
            this.cb_create.Name = "cb_create";
            this.cb_create.Size = new System.Drawing.Size(95, 23);
            this.cb_create.TabIndex = 46;
            this.cb_create.Text = "Auto PO";
            this.cb_create.UseVisualStyleBackColor = true;
            this.cb_create.Click += new System.EventHandler(this.cb_create_Click);
            // 
            // btnReceipts
            // 
            this.btnReceipts.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReceipts.Image = global::Semnox.Parafait.Inventory.Properties.Resources.duplicate;
            this.btnReceipts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReceipts.Location = new System.Drawing.Point(233, 561);
            this.btnReceipts.Name = "btnReceipts";
            this.btnReceipts.Size = new System.Drawing.Size(121, 23);
            this.btnReceipts.TabIndex = 43;
            this.btnReceipts.Text = "View Receipts";
            this.btnReceipts.UseVisualStyleBackColor = true;
            this.btnReceipts.Click += new System.EventHandler(this.btnReceipts_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::Semnox.Parafait.Inventory.Properties.Resources.Refresh;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(557, 561);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(97, 23);
            this.btnRefresh.TabIndex = 44;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::Semnox.Parafait.Inventory.Properties.Resources.printer;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(397, 561);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(90, 23);
            this.btnPrint.TabIndex = 47;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // ProductCode
            // 
            this.ProductCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ProductCode.FillWeight = 32.92284F;
            this.ProductCode.HeaderText = "Product Code";
            this.ProductCode.Name = "ProductCode";
            this.ProductCode.Width = 89;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Description.FillWeight = 41.15355F;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Description.Width = 85;
            // 
            // txtUOM
            // 
            this.txtUOM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.txtUOM.HeaderText = "PO UOM";
            this.txtUOM.Name = "txtUOM";
            this.txtUOM.ReadOnly = true;
            this.txtUOM.Width = 70;
            // 
            // Qty
            // 
            this.Qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Qty.FillWeight = 24.69213F;
            this.Qty.HeaderText = "Qty";
            this.Qty.Name = "Qty";
            this.Qty.Width = 48;
            // 
            // cmbUOM
            // 
            this.cmbUOM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.cmbUOM.HeaderText = "UOM";
            this.cmbUOM.Name = "cmbUOM";
            this.cmbUOM.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cmbUOM.Width = 70;
            // 
            // Price
            // 
            this.Price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Price.FillWeight = 41.15355F;
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.Width = 56;
            // 
            // TaxId
            // 
            this.TaxId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.TaxId.FillWeight = 41.15355F;
            this.TaxId.HeaderText = "Tax";
            this.TaxId.Name = "TaxId";
            this.TaxId.Width = 39;
            // 
            // TaxPercentage
            // 
            this.TaxPercentage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.TaxPercentage.FillWeight = 32.92284F;
            this.TaxPercentage.HeaderText = "Tax %";
            this.TaxPercentage.Name = "TaxPercentage";
            this.TaxPercentage.ReadOnly = true;
            this.TaxPercentage.Width = 50;
            // 
            // TaxInclusive
            // 
            this.TaxInclusive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.TaxInclusive.FalseValue = "N";
            this.TaxInclusive.FillWeight = 24.69213F;
            this.TaxInclusive.HeaderText = "Tax Incl";
            this.TaxInclusive.Name = "TaxInclusive";
            this.TaxInclusive.TrueValue = "Y";
            this.TaxInclusive.Width = 31;
            // 
            // TaxAmount
            // 
            this.TaxAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.TaxAmount.FillWeight = 37.0382F;
            this.TaxAmount.HeaderText = "Tax Amount";
            this.TaxAmount.Name = "TaxAmount";
            this.TaxAmount.Width = 82;
            // 
            // Amount
            // 
            this.Amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            this.Amount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Amount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Amount.Visible = false;
            this.Amount.Width = 49;
            // 
            // RequiredByDate
            // 
            this.RequiredByDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.RequiredByDate.FillWeight = 41.15355F;
            this.RequiredByDate.HeaderText = "Required Date";
            this.RequiredByDate.Name = "RequiredByDate";
            this.RequiredByDate.ReadOnly = true;
            this.RequiredByDate.Width = 93;
            // 
            // IsReceived
            // 
            this.IsReceived.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.IsReceived.FillWeight = 32.92284F;
            this.IsReceived.HeaderText = "Receive?";
            this.IsReceived.Name = "IsReceived";
            this.IsReceived.ReadOnly = true;
            this.IsReceived.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsReceived.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IsReceived.Width = 59;
            // 
            // OrderedQty
            // 
            this.OrderedQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.OrderedQty.HeaderText = "OrderedQty";
            this.OrderedQty.Name = "OrderedQty";
            this.OrderedQty.Visible = false;
            this.OrderedQty.Width = 86;
            // 
            // PurchaseOrderLineId
            // 
            this.PurchaseOrderLineId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.PurchaseOrderLineId.HeaderText = "PurchaseOrderLineId";
            this.PurchaseOrderLineId.Name = "PurchaseOrderLineId";
            this.PurchaseOrderLineId.Visible = false;
            // 
            // LowerLimitCost
            // 
            this.LowerLimitCost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.LowerLimitCost.HeaderText = "LowerLimitCost";
            this.LowerLimitCost.Name = "LowerLimitCost";
            this.LowerLimitCost.Visible = false;
            // 
            // UpperLimitCost
            // 
            this.UpperLimitCost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.UpperLimitCost.HeaderText = "UpperLimitCost";
            this.UpperLimitCost.Name = "UpperLimitCost";
            this.UpperLimitCost.Visible = false;
            // 
            // CostVariancePercent
            // 
            this.CostVariancePercent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CostVariancePercent.HeaderText = "CostVariancePercent";
            this.CostVariancePercent.Name = "CostVariancePercent";
            this.CostVariancePercent.Visible = false;
            // 
            // OrigPrice
            // 
            this.OrigPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.OrigPrice.HeaderText = "OrigPrice";
            this.OrigPrice.Name = "OrigPrice";
            this.OrigPrice.Visible = false;
            // 
            // isLottable
            // 
            this.isLottable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.isLottable.HeaderText = "isLottable?";
            this.isLottable.Name = "isLottable";
            this.isLottable.Visible = false;
            // 
            // ExpiryType
            // 
            this.ExpiryType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ExpiryType.HeaderText = "ExpiryType";
            this.ExpiryType.Name = "ExpiryType";
            this.ExpiryType.Visible = false;
            // 
            // ExpiryDays
            // 
            this.ExpiryDays.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle1.NullValue = "0";
            this.ExpiryDays.DefaultCellStyle = dataGridViewCellStyle1;
            this.ExpiryDays.HeaderText = "ExpiryDays";
            this.ExpiryDays.Name = "ExpiryDays";
            this.ExpiryDays.Visible = false;
            // 
            // ProductId
            // 
            this.ProductId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ProductId.HeaderText = "ProductId";
            this.ProductId.Name = "ProductId";
            this.ProductId.Visible = false;
            // 
            // RequisitionId
            // 
            this.RequisitionId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.RequisitionId.HeaderText = "RequisitionId";
            this.RequisitionId.Name = "RequisitionId";
            this.RequisitionId.Visible = false;
            // 
            // RequisitionLineId
            // 
            this.RequisitionLineId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.RequisitionLineId.HeaderText = "RequisitionLineId";
            this.RequisitionLineId.Name = "RequisitionLineId";
            this.RequisitionLineId.Visible = false;
            // 
            // Lot
            // 
            this.Lot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Lot.FillWeight = 20.57677F;
            this.Lot.HeaderText = "Lot";
            this.Lot.Name = "Lot";
            this.Lot.Text = "...";
            this.Lot.UseColumnTextForButtonValue = true;
            // 
            // recvLocation
            // 
            this.recvLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.recvLocation.FillWeight = 41.15355F;
            this.recvLocation.HeaderText = "Location";
            this.recvLocation.Name = "recvLocation";
            // 
            // PriceInTickets
            // 
            this.PriceInTickets.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.PriceInTickets.HeaderText = "PriceInTickets";
            this.PriceInTickets.Name = "PriceInTickets";
            this.PriceInTickets.Visible = false;
            // 
            // StockLink
            // 
            this.StockLink.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.StockLink.FillWeight = 30F;
            this.StockLink.HeaderText = "Stock";
            this.StockLink.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.StockLink.Name = "StockLink";
            this.StockLink.ReadOnly = true;
            this.StockLink.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.StockLink.Text = "";
            this.StockLink.Width = 41;
            // 
            // frmReceiveInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1028, 591);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnReceipts);
            this.Controls.Add(this.cb_create);
            this.Controls.Add(this.cb_complete);
            this.Controls.Add(this.gbfilter);
            this.Controls.Add(this.cb_exit);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txt_remarks);
            this.Controls.Add(this.gbSearch);
            this.Controls.Add(this.gb_receive);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmReceiveInventory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ReceiveInventory";
            this.Activated += new System.EventHandler(this.frm_receive_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_receive_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_receive_FormClosed);
            this.Load += new System.EventHandler(this.frm_receive_Load);
            ((System.ComponentModel.ISupportInitialize)(this.receive_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rsearch_dgv)).EndInit();
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.gb_receive.ResumeLayout(false);
            this.gb_receive.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gbfilter.ResumeLayout(false);
            this.gbfilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchorder_dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView receive_dgv;
        private System.Windows.Forms.RichTextBox txt_remarks;
        private System.Windows.Forms.Button cb_complete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView Rsearch_dgv;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_prodcode;
        private System.Windows.Forms.TextBox txt_qty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox dd_qty;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.GroupBox gb_receive;
        private System.Windows.Forms.Button cb_exit;
        private System.Windows.Forms.Button cb_search;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox gbfilter;
        private System.Windows.Forms.Button cb_POsearch;
        private System.Windows.Forms.TextBox tb_searchorder;
        private System.Windows.Forms.ComboBox cb_searchstatus;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSearchVendor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView searchorder_dgv;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbVendor;
        private System.Windows.Forms.TextBox tb_date;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tb_order;
        //private RedemptionDataSet redemptionDataSet;
        //private Redemption.RedemptionDataSetTableAdapters.VendorTableAdapter vendorTableAdapter;
        private System.Windows.Forms.Label lb_orderid;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_contact;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_phone;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button cb_create;
        private System.Windows.Forms.TextBox txtVendorBillNo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnReceipts;
        private System.Windows.Forms.TextBox txtGRN;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtGatePassNo;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox cmbDefaultTax;
        private System.Windows.Forms.DateTimePicker dtpReceiveDate;
        //private RedemptionDataSetTableAdapters.TaxTableAdapter taxTableAdapter;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tb_status;
        private System.Windows.Forms.TextBox tb_total;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.LinkLabel lnkApplyTaxToAllLines;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Label lblOrderDocumentType;
        private System.Windows.Forms.Label txtReceiptId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProdID;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.LinkLabel lblViewRequisitions;
        private System.Windows.Forms.DataGridViewTextBoxColumn PurchaseorderID;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentStatus;
        private System.Windows.Forms.LinkLabel lnkReceiveAll;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewComboBoxColumn TaxId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxPercentage;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TaxInclusive;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequiredByDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsReceived;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderedQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn PurchaseOrderLineId;
        private System.Windows.Forms.DataGridViewTextBoxColumn LowerLimitCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpperLimitCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostVariancePercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrigPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn isLottable;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpiryType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpiryDays;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequisitionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequisitionLineId;
        private System.Windows.Forms.DataGridViewButtonColumn Lot;
        private System.Windows.Forms.DataGridViewComboBoxColumn recvLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceInTickets;
        private System.Windows.Forms.DataGridViewLinkColumn StockLink;
    }
}
