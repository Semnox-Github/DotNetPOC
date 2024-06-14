namespace Semnox.Parafait.Inventory
{
    partial class frmVendorReturns
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
            this.txtPONumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGRN = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVendorName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVendorBillNumber = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvReceipt = new System.Windows.Forms.DataGridView();
            this.receiptIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PONumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PODate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiptAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorBillNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gatePassNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gRNDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiveDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchaseOrderIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceSystemIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiveToLocationIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.documentTypeIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inventoryReceiptDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dgvReceiptLines = new System.Windows.Forms.DataGridView();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOMId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vendorItemCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtUOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorReturnedQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbUOM = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taxPercentageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocationId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchaseOrderLineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SelectLots = new System.Windows.Forms.DataGridViewButtonColumn();
            this.POPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isReceivedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taxInclusiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taxIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiptIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vendorBillNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivedByDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceSystemIDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timestampDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inventoryReceiveLinesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.Activated += new System.EventHandler(frmVendorReturns_Activated); 
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReceipt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryReceiptDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReceiptLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryReceiveLinesDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPONumber
            // 
            this.txtPONumber.Location = new System.Drawing.Point(79, 14);
            this.txtPONumber.Name = "txtPONumber";
            this.txtPONumber.Size = new System.Drawing.Size(100, 20);
            this.txtPONumber.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "PO Number:";
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.label6);
            this.grpSearch.Controls.Add(this.txtGRN);
            this.grpSearch.Controls.Add(this.label3);
            this.grpSearch.Controls.Add(this.txtVendorName);
            this.grpSearch.Controls.Add(this.txtPONumber);
            this.grpSearch.Controls.Add(this.label2);
            this.grpSearch.Controls.Add(this.label1);
            this.grpSearch.Controls.Add(this.txtVendorBillNumber);
            this.grpSearch.Location = new System.Drawing.Point(12, 12);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(783, 42);
            this.grpSearch.TabIndex = 1;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Search";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(634, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "GRN:";
            // 
            // txtGRN
            // 
            this.txtGRN.Location = new System.Drawing.Point(674, 14);
            this.txtGRN.Name = "txtGRN";
            this.txtGRN.Size = new System.Drawing.Size(100, 20);
            this.txtGRN.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(429, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Vendor Name:";
            // 
            // txtVendorName
            // 
            this.txtVendorName.Location = new System.Drawing.Point(507, 14);
            this.txtVendorName.Name = "txtVendorName";
            this.txtVendorName.Size = new System.Drawing.Size(100, 20);
            this.txtVendorName.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Vendor Bill Number:";
            // 
            // txtVendorBillNumber
            // 
            this.txtVendorBillNumber.Location = new System.Drawing.Point(297, 14);
            this.txtVendorBillNumber.Name = "txtVendorBillNumber";
            this.txtVendorBillNumber.Size = new System.Drawing.Size(100, 20);
            this.txtVendorBillNumber.TabIndex = 6;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(889, 25);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 12;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Receipts";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Receipt Lines";
            // 
            // dgvReceipt
            // 
            this.dgvReceipt.AllowUserToAddRows = false;
            this.dgvReceipt.AllowUserToDeleteRows = false;
            this.dgvReceipt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvReceipt.AutoGenerateColumns = false;
            this.dgvReceipt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReceipt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.receiptIdDataGridViewTextBoxColumn,
            this.PONumber,
            this.PODate,
            this.VendorName,
            this.ReceiptAmount,
            this.VendorBillNumber,
            this.gatePassNumberDataGridViewTextBoxColumn,
            this.gRNDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.receiveDate,
            this.receivedByDataGridViewTextBoxColumn,
            this.purchaseOrderIdDataGridViewTextBoxColumn,
            this.sourceSystemIDDataGridViewTextBoxColumn,
            this.receiveToLocationIDDataGridViewTextBoxColumn,
            this.documentTypeIDDataGridViewTextBoxColumn});
            this.dgvReceipt.DataSource = this.inventoryReceiptDTOBindingSource;
            this.dgvReceipt.Location = new System.Drawing.Point(12, 83);
            this.dgvReceipt.Name = "dgvReceipt";
            this.dgvReceipt.Size = new System.Drawing.Size(993, 151);
            this.dgvReceipt.TabIndex = 15;
            this.dgvReceipt.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dgvReceipt_RowStateChanged);
            this.dgvReceipt.SelectionChanged += new System.EventHandler(this.dgvReceipt_SelectionChanged);
            // 
            // receiptIdDataGridViewTextBoxColumn
            // 
            this.receiptIdDataGridViewTextBoxColumn.DataPropertyName = "ReceiptId";
            this.receiptIdDataGridViewTextBoxColumn.HeaderText = "Receipt Id";
            this.receiptIdDataGridViewTextBoxColumn.Name = "receiptIdDataGridViewTextBoxColumn";
            this.receiptIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // PONumber
            // 
            this.PONumber.HeaderText = "PO No.";
            this.PONumber.Name = "PONumber";
            this.PONumber.ReadOnly = true;
            // 
            // PODate
            // 
            this.PODate.HeaderText = "PO Date";
            this.PODate.Name = "PODate";
            this.PODate.ReadOnly = true;
            // 
            // VendorName
            // 
            this.VendorName.HeaderText = "Vendor Name";
            this.VendorName.Name = "VendorName";
            this.VendorName.ReadOnly = true;
            // 
            // ReceiptAmount
            // 
            this.ReceiptAmount.HeaderText = "Total Amount";
            this.ReceiptAmount.Name = "ReceiptAmount";
            this.ReceiptAmount.ReadOnly = true;
            // 
            // VendorBillNumber
            // 
            this.VendorBillNumber.DataPropertyName = "VendorBillNumber";
            this.VendorBillNumber.HeaderText = "Vendor Bill Number";
            this.VendorBillNumber.Name = "VendorBillNumber";
            this.VendorBillNumber.ReadOnly = true;
            // 
            // gatePassNumberDataGridViewTextBoxColumn
            // 
            this.gatePassNumberDataGridViewTextBoxColumn.DataPropertyName = "GatePassNumber";
            this.gatePassNumberDataGridViewTextBoxColumn.HeaderText = "Gate Pass Number";
            this.gatePassNumberDataGridViewTextBoxColumn.Name = "gatePassNumberDataGridViewTextBoxColumn";
            this.gatePassNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // gRNDataGridViewTextBoxColumn
            // 
            this.gRNDataGridViewTextBoxColumn.DataPropertyName = "GRN";
            this.gRNDataGridViewTextBoxColumn.HeaderText = "GRN";
            this.gRNDataGridViewTextBoxColumn.Name = "gRNDataGridViewTextBoxColumn";
            this.gRNDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            this.remarksDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // receiveDate
            // 
            this.receiveDate.DataPropertyName = "ReceiveDate";
            this.receiveDate.HeaderText = "Receive Date";
            this.receiveDate.Name = "receiveDate";
            this.receiveDate.ReadOnly = true;
            // 
            // receivedByDataGridViewTextBoxColumn
            // 
            this.receivedByDataGridViewTextBoxColumn.DataPropertyName = "ReceivedBy";
            this.receivedByDataGridViewTextBoxColumn.HeaderText = "Received By";
            this.receivedByDataGridViewTextBoxColumn.Name = "receivedByDataGridViewTextBoxColumn";
            this.receivedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // purchaseOrderIdDataGridViewTextBoxColumn
            // 
            this.purchaseOrderIdDataGridViewTextBoxColumn.DataPropertyName = "PurchaseOrderId";
            this.purchaseOrderIdDataGridViewTextBoxColumn.HeaderText = "Purchase Order Id";
            this.purchaseOrderIdDataGridViewTextBoxColumn.Name = "purchaseOrderIdDataGridViewTextBoxColumn";
            this.purchaseOrderIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.purchaseOrderIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // sourceSystemIDDataGridViewTextBoxColumn
            // 
            this.sourceSystemIDDataGridViewTextBoxColumn.DataPropertyName = "SourceSystemID";
            this.sourceSystemIDDataGridViewTextBoxColumn.HeaderText = "Source System ID";
            this.sourceSystemIDDataGridViewTextBoxColumn.Name = "sourceSystemIDDataGridViewTextBoxColumn";
            this.sourceSystemIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.sourceSystemIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // receiveToLocationIDDataGridViewTextBoxColumn
            // 
            this.receiveToLocationIDDataGridViewTextBoxColumn.DataPropertyName = "ReceiveToLocationID";
            this.receiveToLocationIDDataGridViewTextBoxColumn.HeaderText = "Receive To Location ID";
            this.receiveToLocationIDDataGridViewTextBoxColumn.Name = "receiveToLocationIDDataGridViewTextBoxColumn";
            this.receiveToLocationIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.receiveToLocationIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // documentTypeIDDataGridViewTextBoxColumn
            // 
            this.documentTypeIDDataGridViewTextBoxColumn.DataPropertyName = "DocumentTypeID";
            this.documentTypeIDDataGridViewTextBoxColumn.HeaderText = "Document Type ID";
            this.documentTypeIDDataGridViewTextBoxColumn.Name = "documentTypeIDDataGridViewTextBoxColumn";
            this.documentTypeIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.documentTypeIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // inventoryReceiptDTOBindingSource
            // 
            this.inventoryReceiptDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.InventoryReceiptDTO);
            // 
            // dgvReceiptLines
            // 
            this.dgvReceiptLines.AllowUserToAddRows = false;
            this.dgvReceiptLines.AllowUserToDeleteRows = false;
            this.dgvReceiptLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvReceiptLines.AutoGenerateColumns = false;
            this.dgvReceiptLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReceiptLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.UOMId,
            this.productIdDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.vendorItemCodeDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.Price,
            this.VendorReturnedQuantity,
            this.amountDataGridViewTextBoxColumn,
            this.Tax,
            this.taxPercentageDataGridViewTextBoxColumn,
            this.LocationName,
            this.LocationId,
            this.CurrentStock,
            this.purchaseOrderLineIdDataGridViewTextBoxColumn,
            this.txtUOM,
            this.ReturnQuantity,
            this.cmbUOM,
            this.SelectLots,
            this.POPrice,
            this.isReceivedDataGridViewTextBoxColumn,
            this.taxInclusiveDataGridViewTextBoxColumn,
            this.taxIdDataGridViewTextBoxColumn,
            this.receiptIdDataGridViewTextBoxColumn1,
            this.vendorBillNumberDataGridViewTextBoxColumn,
            this.receivedByDataGridViewTextBoxColumn1,
            this.sourceSystemIDDataGridViewTextBoxColumn1,
            this.timestampDataGridViewTextBoxColumn,
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn});
            this.dgvReceiptLines.DataSource = this.inventoryReceiveLinesDTOBindingSource;
            this.dgvReceiptLines.Location = new System.Drawing.Point(11, 259);
            this.dgvReceiptLines.Name = "dgvReceiptLines";
            this.dgvReceiptLines.Size = new System.Drawing.Size(994, 220);
            this.dgvReceiptLines.TabIndex = 16;
            this.dgvReceiptLines.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReceiptLines_CellContentClick);
            this.dgvReceiptLines.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReceiptLines_CellMouseEnter);
            this.dgvReceiptLines.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReceiptLines_CellMouseLeave);
            this.dgvReceiptLines.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvReceiptLines_CellValidating);
            this.dgvReceiptLines.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvReceiptLines_ColumnHeaderMouseClick);
            this.dgvReceiptLines.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dgvReceiptLines_RowStateChanged);
            // 
            // Code
            // 
            this.Code.HeaderText = "Product Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            // 
            // UOMId
            // 
            this.UOMId.DataPropertyName = "UOMId";
            this.UOMId.HeaderText = "UOMId";
            this.UOMId.Name = "UOMId";
            this.UOMId.Visible = false;
            // 
            // productIdDataGridViewTextBoxColumn
            // 
            this.productIdDataGridViewTextBoxColumn.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn.HeaderText = "Product";
            this.productIdDataGridViewTextBoxColumn.Name = "productIdDataGridViewTextBoxColumn";
            this.productIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.productIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // vendorItemCodeDataGridViewTextBoxColumn
            // 
            this.vendorItemCodeDataGridViewTextBoxColumn.DataPropertyName = "VendorItemCode";
            this.vendorItemCodeDataGridViewTextBoxColumn.HeaderText = "Vendor Item Code";
            this.vendorItemCodeDataGridViewTextBoxColumn.Name = "vendorItemCodeDataGridViewTextBoxColumn";
            this.vendorItemCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Actual Received Quantity";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            // 
            // VendorReturnedQuantity
            // 
            this.VendorReturnedQuantity.HeaderText = "Vendor Returned Quantity";
            this.VendorReturnedQuantity.Name = "VendorReturnedQuantity";
            this.VendorReturnedQuantity.ReadOnly = true;
            this.VendorReturnedQuantity.Width = 50;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            this.amountDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            this.amountDataGridViewTextBoxColumn.ReadOnly = true;
            this.amountDataGridViewTextBoxColumn.Visible = false;
            // 
            // Tax
            // 
            this.Tax.DataPropertyName = "PurchaseOrderReceiveLineId";
            this.Tax.HeaderText = "Tax";
            this.Tax.Name = "Tax";
            this.Tax.ReadOnly = true;
            this.Tax.Visible = false;
            // 
            // taxPercentageDataGridViewTextBoxColumn
            // 
            this.taxPercentageDataGridViewTextBoxColumn.DataPropertyName = "TaxPercentage";
            this.taxPercentageDataGridViewTextBoxColumn.HeaderText = "TaxPercentage";
            this.taxPercentageDataGridViewTextBoxColumn.Name = "taxPercentageDataGridViewTextBoxColumn";
            this.taxPercentageDataGridViewTextBoxColumn.ReadOnly = true;
            this.taxPercentageDataGridViewTextBoxColumn.Visible = false;
            // 
            // LocationName
            // 
            this.LocationName.HeaderText = "Location";
            this.LocationName.Name = "LocationName";
            this.LocationName.ReadOnly = true;
            // 
            // LocationId
            // 
            this.LocationId.DataPropertyName = "LocationId";
            this.LocationId.HeaderText = "Location";
            this.LocationId.Name = "LocationId";
            this.LocationId.ReadOnly = true;
            this.LocationId.Visible = false;
            // 
            // CurrentStock
            // 
            this.CurrentStock.HeaderText = "Current Stock";
            this.CurrentStock.Name = "CurrentStock";
            this.CurrentStock.ReadOnly = true;
            this.CurrentStock.Visible = false;
            // 
            // purchaseOrderLineIdDataGridViewTextBoxColumn
            // 
            this.purchaseOrderLineIdDataGridViewTextBoxColumn.DataPropertyName = "PurchaseOrderLineId";
            this.purchaseOrderLineIdDataGridViewTextBoxColumn.HeaderText = "PO Line Id";
            this.purchaseOrderLineIdDataGridViewTextBoxColumn.Name = "purchaseOrderLineIdDataGridViewTextBoxColumn";
            this.purchaseOrderLineIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // txtUOM
            // 
            this.txtUOM.DataPropertyName = "UOM";
            this.txtUOM.HeaderText = "PO UOM";
            this.txtUOM.Name = "txtUOM";
            this.txtUOM.ReadOnly = true;
            // 
            // cmbUOM
            // 
            this.cmbUOM.HeaderText = "UOM";
            this.cmbUOM.MinimumWidth = 20;
            this.cmbUOM.Name = "cmbUOM";
            this.cmbUOM.Width = 80;
            // 
            // ReturnQuantity
            // 
            this.ReturnQuantity.HeaderText = "Return Quantity";
            this.ReturnQuantity.Name = "ReturnQuantity";
            this.ReturnQuantity.Width = 80;
            // 
            // SelectLots
            // 
            this.SelectLots.HeaderText = "Select Lots with Quantity";
            this.SelectLots.Name = "SelectLots";
            this.SelectLots.ReadOnly = true;
            this.SelectLots.Text = "....";
            // 
            // POPrice
            // 
            this.POPrice.DataPropertyName = "PurchaseOrderReceiveLineId";
            this.POPrice.HeaderText = "PO Price";
            this.POPrice.Name = "POPrice";
            this.POPrice.ReadOnly = true;
            this.POPrice.Visible = false;
            // 
            // isReceivedDataGridViewTextBoxColumn
            // 
            this.isReceivedDataGridViewTextBoxColumn.DataPropertyName = "IsReceived";
            this.isReceivedDataGridViewTextBoxColumn.HeaderText = "IsReceived?";
            this.isReceivedDataGridViewTextBoxColumn.Name = "isReceivedDataGridViewTextBoxColumn";
            this.isReceivedDataGridViewTextBoxColumn.Visible = false;
            // 
            // taxInclusiveDataGridViewTextBoxColumn
            // 
            this.taxInclusiveDataGridViewTextBoxColumn.DataPropertyName = "TaxInclusive";
            this.taxInclusiveDataGridViewTextBoxColumn.HeaderText = "TaxInclusive?";
            this.taxInclusiveDataGridViewTextBoxColumn.Name = "taxInclusiveDataGridViewTextBoxColumn";
            this.taxInclusiveDataGridViewTextBoxColumn.Visible = false;
            // 
            // taxIdDataGridViewTextBoxColumn
            // 
            this.taxIdDataGridViewTextBoxColumn.DataPropertyName = "TaxId";
            this.taxIdDataGridViewTextBoxColumn.HeaderText = "TaxId";
            this.taxIdDataGridViewTextBoxColumn.Name = "taxIdDataGridViewTextBoxColumn";
            this.taxIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // receiptIdDataGridViewTextBoxColumn1
            // 
            this.receiptIdDataGridViewTextBoxColumn1.DataPropertyName = "ReceiptId";
            this.receiptIdDataGridViewTextBoxColumn1.HeaderText = "ReceiptId";
            this.receiptIdDataGridViewTextBoxColumn1.Name = "receiptIdDataGridViewTextBoxColumn1";
            this.receiptIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // vendorBillNumberDataGridViewTextBoxColumn
            // 
            this.vendorBillNumberDataGridViewTextBoxColumn.DataPropertyName = "VendorBillNumber";
            this.vendorBillNumberDataGridViewTextBoxColumn.HeaderText = "Vendor Bill Number";
            this.vendorBillNumberDataGridViewTextBoxColumn.Name = "vendorBillNumberDataGridViewTextBoxColumn";
            this.vendorBillNumberDataGridViewTextBoxColumn.Visible = false;
            // 
            // receivedByDataGridViewTextBoxColumn1
            // 
            this.receivedByDataGridViewTextBoxColumn1.DataPropertyName = "ReceivedBy";
            this.receivedByDataGridViewTextBoxColumn1.HeaderText = "Received By";
            this.receivedByDataGridViewTextBoxColumn1.Name = "receivedByDataGridViewTextBoxColumn1";
            this.receivedByDataGridViewTextBoxColumn1.Visible = false;
            // 
            // sourceSystemIDDataGridViewTextBoxColumn1
            // 
            this.sourceSystemIDDataGridViewTextBoxColumn1.DataPropertyName = "SourceSystemID";
            this.sourceSystemIDDataGridViewTextBoxColumn1.HeaderText = "Source System ID";
            this.sourceSystemIDDataGridViewTextBoxColumn1.Name = "sourceSystemIDDataGridViewTextBoxColumn1";
            this.sourceSystemIDDataGridViewTextBoxColumn1.Visible = false;
            // 
            // timestampDataGridViewTextBoxColumn
            // 
            this.timestampDataGridViewTextBoxColumn.DataPropertyName = "Timestamp";
            this.timestampDataGridViewTextBoxColumn.HeaderText = "Time stamp";
            this.timestampDataGridViewTextBoxColumn.Name = "timestampDataGridViewTextBoxColumn";
            this.timestampDataGridViewTextBoxColumn.Visible = false;
            // 
            // purchaseOrderReceiveLineIdDataGridViewTextBoxColumn
            // 
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.DataPropertyName = "PurchaseOrderReceiveLineId";
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.HeaderText = "purchase Order Receive Line Id";
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.Name = "purchaseOrderReceiveLineIdDataGridViewTextBoxColumn";
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // inventoryReceiveLinesDTOBindingSource
            // 
            this.inventoryReceiveLinesDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.InventoryDTO);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(473, 485);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(357, 485);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "Submit";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(808, 25);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // frmVendorReturns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 514);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvReceipt);
            this.Controls.Add(this.dgvReceiptLines);
            this.Controls.Add(this.grpSearch);
            this.Controls.Add(this.btnRefresh);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmVendorReturns";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Return To Vendor";
            this.Load += new System.EventHandler(this.frmVendorReturns_Load);
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReceipt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryReceiptDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReceiptLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryReceiveLinesDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPONumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGRN;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVendorName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtVendorBillNumber;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvReceipt;
        private System.Windows.Forms.DataGridView dgvReceiptLines;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.BindingSource inventoryReceiptDTOBindingSource;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.BindingSource inventoryReceiveLinesDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiptIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PONumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn PODate;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiptAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorBillNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn gatePassNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gRNDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiveDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchaseOrderIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceSystemIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiveToLocationIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn documentTypeIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOMId;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vendorItemCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorReturnedQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxPercentageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationId;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchaseOrderLineIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtUOM;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnQuantity;
        private System.Windows.Forms.DataGridViewButtonColumn SelectLots;
        private System.Windows.Forms.DataGridViewTextBoxColumn POPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn isReceivedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxInclusiveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiptIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn vendorBillNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivedByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceSystemIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn timestampDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchaseOrderReceiveLineIdDataGridViewTextBoxColumn;
    }
}

