using Semnox.Parafait.Inventory;
using Semnox.Parafait.Product;
using Semnox.Parafait.Vendor;

namespace Semnox.Parafait.Inventory
{
    partial class frmOrderReceipts
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
            this.dgvReceiptLines = new System.Windows.Forms.DataGridView();
            this.purchaseTaxDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.locationDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.inventoryReceiveLinesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.productDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtPONumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVendorBillNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVendorName = new System.Windows.Forms.TextBox();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGRN = new System.Windows.Forms.TextBox();
            this.dgvReceipt = new System.Windows.Forms.DataGridView();
            this.receiptIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiptAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorBillNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gatePassNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gRNDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchaseOrderIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiveDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inventoryReceiptDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.vendorDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnPrint = new System.Windows.Forms.Button();
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vendorItemCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TaxPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isReceivedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LotDetails = new System.Windows.Forms.DataGridViewButtonColumn();
            this.PurchaseOrderLineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequisitionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequisitionLineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReceiptLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseTaxDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryReceiveLinesDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productDTOBindingSource)).BeginInit();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReceipt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryReceiptDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorDTOBindingSource)).BeginInit();
            this.SuspendLayout();
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
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn,
            this.productIdDataGridViewTextBoxColumn,
            this.ProductId,
            this.descriptionDataGridViewTextBoxColumn,
            this.vendorItemCodeDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.Price,
            this.amount,
            this.TaxId,
            this.TaxPercentage,
            this.locationIdDataGridViewTextBoxColumn,
            this.UOM,
            this.isReceivedDataGridViewTextBoxColumn,
            this.CurrentStock,
            this.LotDetails,
            this.PurchaseOrderLineId,
            this.POQuantity,
            this.UnitPrice,
            this.TaxAmount,
            this.SubTotal,
            this.RequisitionId,
            this.RequisitionLineId
            });
            this.dgvReceiptLines.DataSource = this.inventoryReceiveLinesDTOBindingSource;
            this.dgvReceiptLines.Location = new System.Drawing.Point(12, 240);
            this.dgvReceiptLines.Name = "dgvReceiptLines";
            this.dgvReceiptLines.Size = new System.Drawing.Size(994, 224);
            this.dgvReceiptLines.TabIndex = 0;
            this.dgvReceiptLines.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReceipts_CellContentClick);
            this.dgvReceiptLines.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReceipts_CellMouseEnter);
            this.dgvReceiptLines.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReceipts_CellMouseLeave);
            // 
            // purchaseTaxDTOBindingSource
            // 
            this.purchaseTaxDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.TaxDTO);
            // 
            // locationDTOBindingSource
            // 
            this.locationDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.LocationDTO);
            // 
            // inventoryReceiveLinesDTOBindingSource
            // 
            this.inventoryReceiveLinesDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.InventoryReceiveLinesDTO);
            // 
            // productDTOBindingSource
            // 
            this.productDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.ProductDTO);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(12, 470);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(222, 470);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(866, 14);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtPONumber
            // 
            this.txtPONumber.Location = new System.Drawing.Point(79, 14);
            this.txtPONumber.Name = "txtPONumber";
            this.txtPONumber.Size = new System.Drawing.Size(100, 20);
            this.txtPONumber.TabIndex = 4;
            this.txtPONumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPONumber_KeyPress);
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
            this.txtVendorBillNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVendorBillNumber_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(447, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Vendor Name:";
            // 
            // txtVendorName
            // 
            this.txtVendorName.Location = new System.Drawing.Point(523, 14);
            this.txtVendorName.Name = "txtVendorName";
            this.txtVendorName.Size = new System.Drawing.Size(100, 20);
            this.txtVendorName.TabIndex = 8;
            this.txtVendorName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVendorName_KeyPress);
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
            this.grpSearch.Location = new System.Drawing.Point(12, 0);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(848, 42);
            this.grpSearch.TabIndex = 10;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Search";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(666, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "GRN:";
            // 
            // txtGRN
            // 
            this.txtGRN.Location = new System.Drawing.Point(702, 14);
            this.txtGRN.Name = "txtGRN";
            this.txtGRN.Size = new System.Drawing.Size(100, 20);
            this.txtGRN.TabIndex = 10;
            this.txtGRN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGRN_KeyPress);
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
            this.OrderNumber,
            this.OrderDate,
            this.VendorId,
            this.ReceiptAmount,
            this.VendorBillNumber,
            this.gatePassNumberDataGridViewTextBoxColumn,
            this.gRNDataGridViewTextBoxColumn,
            this.purchaseOrderIdDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.ReceiveDate,
            this.receivedByDataGridViewTextBoxColumn});
            this.dgvReceipt.DataSource = this.inventoryReceiptDTOBindingSource;
            this.dgvReceipt.Location = new System.Drawing.Point(13, 64);
            this.dgvReceipt.Name = "dgvReceipt";
            this.dgvReceipt.Size = new System.Drawing.Size(993, 151);
            this.dgvReceipt.TabIndex = 11;
            this.dgvReceipt.SelectionChanged += new System.EventHandler(this.dgvReceipt_SelectionChanged);
            // 
            // receiptIdDataGridViewTextBoxColumn
            // 
            this.receiptIdDataGridViewTextBoxColumn.DataPropertyName = "ReceiptId";
            this.receiptIdDataGridViewTextBoxColumn.HeaderText = "ReceiptId";
            this.receiptIdDataGridViewTextBoxColumn.Name = "receiptIdDataGridViewTextBoxColumn";
            this.receiptIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // OrderNumber
            // 
            this.OrderNumber.DataPropertyName = "OrderNumber";
            this.OrderNumber.HeaderText = "PO Number";
            this.OrderNumber.Name = "OrderNumber";
            this.OrderNumber.ReadOnly = true;
            // 
            // OrderDate
            // 
            this.OrderDate.DataPropertyName = "OrderDate";
            this.OrderDate.HeaderText = "PO Date";
            this.OrderDate.Name = "OrderDate";
            this.OrderDate.ReadOnly = true;
            // 
            // VendorId
            // 
            this.VendorId.DataPropertyName = "VendorName";
            this.VendorId.HeaderText = "Vendor Name";
            this.VendorId.Name = "VendorId";
            this.VendorId.ReadOnly = true;
            this.VendorId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ReceiptAmount
            // 
            this.ReceiptAmount.DataPropertyName = "ReceiptAmount";
            this.ReceiptAmount.HeaderText = "Total Amount";
            this.ReceiptAmount.Name = "ReceiptAmount";
            this.ReceiptAmount.ReadOnly = true;
            // 
            // VendorBillNumber
            // 
            this.VendorBillNumber.DataPropertyName = "VendorBillNumber";
            this.VendorBillNumber.HeaderText = "VendorBillNumber";
            this.VendorBillNumber.Name = "VendorBillNumber";
            // 
            // gatePassNumberDataGridViewTextBoxColumn
            // 
            this.gatePassNumberDataGridViewTextBoxColumn.DataPropertyName = "GatePassNumber";
            this.gatePassNumberDataGridViewTextBoxColumn.HeaderText = "Gate Pass Number";
            this.gatePassNumberDataGridViewTextBoxColumn.Name = "gatePassNumberDataGridViewTextBoxColumn";
            // 
            // gRNDataGridViewTextBoxColumn
            // 
            this.gRNDataGridViewTextBoxColumn.DataPropertyName = "GRN";
            this.gRNDataGridViewTextBoxColumn.HeaderText = "GRN";
            this.gRNDataGridViewTextBoxColumn.Name = "gRNDataGridViewTextBoxColumn";
            this.gRNDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // purchaseOrderIdDataGridViewTextBoxColumn
            // 
            this.purchaseOrderIdDataGridViewTextBoxColumn.DataPropertyName = "PurchaseOrderId";
            this.purchaseOrderIdDataGridViewTextBoxColumn.HeaderText = "PurchaseOrderId";
            this.purchaseOrderIdDataGridViewTextBoxColumn.Name = "purchaseOrderIdDataGridViewTextBoxColumn";
            this.purchaseOrderIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            // 
            // ReceiveDate
            // 
            this.ReceiveDate.DataPropertyName = "ReceiveDate";
            this.ReceiveDate.HeaderText = "Receive Date";
            this.ReceiveDate.Name = "ReceiveDate";
            this.ReceiveDate.ReadOnly = true;
            // 
            // receivedByDataGridViewTextBoxColumn
            // 
            this.receivedByDataGridViewTextBoxColumn.DataPropertyName = "ReceivedBy";
            this.receivedByDataGridViewTextBoxColumn.HeaderText = "Received By";
            this.receivedByDataGridViewTextBoxColumn.Name = "receivedByDataGridViewTextBoxColumn";
            this.receivedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // inventoryReceiptDTOBindingSource
            // 
            this.inventoryReceiptDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.InventoryReceiptDTO);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Receipt Lines";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Receipts";
            // 
            // vendorDTOBindingSource
            // 
            this.vendorDTOBindingSource.DataSource = typeof(Semnox.Parafait.Vendor.VendorDTO);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.Location = new System.Drawing.Point(117, 470);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 14;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // purchaseOrderReceiveLineIdDataGridViewTextBoxColumn
            // 
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.DataPropertyName = "PurchaseOrderReceiveLineId";
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.HeaderText = "PurchaseOrderReceiveLineId";
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.Name = "purchaseOrderReceiveLineIdDataGridViewTextBoxColumn";
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // productIdDataGridViewTextBoxColumn
            // 
            this.productIdDataGridViewTextBoxColumn.DataPropertyName = "ProductCode";
            this.productIdDataGridViewTextBoxColumn.HeaderText = "Product";
            this.productIdDataGridViewTextBoxColumn.Name = "productIdDataGridViewTextBoxColumn";
            this.productIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.productIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ProductId
            // 
            this.ProductId.DataPropertyName = "ProductId";
            this.ProductId.HeaderText = "ProductId";
            this.ProductId.Name = "ProductId";
            this.ProductId.Visible = false;
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
            this.vendorItemCodeDataGridViewTextBoxColumn.HeaderText = "Vendor Item";
            this.vendorItemCodeDataGridViewTextBoxColumn.Name = "vendorItemCodeDataGridViewTextBoxColumn";
            this.vendorItemCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            this.Price.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // amount
            // 
            this.amount.DataPropertyName = "Amount";
            this.amount.HeaderText = "Amount";
            this.amount.Name = "amount";
            this.amount.ReadOnly = true;
            // 
            // TaxId
            // 
            this.TaxId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TaxId.DataPropertyName = "PurchaseTaxId";
            this.TaxId.DataSource = this.purchaseTaxDTOBindingSource;
            this.TaxId.DisplayMember = "TaxName";
            this.TaxId.DisplayStyleForCurrentCellOnly = true;
            this.TaxId.HeaderText = "Tax";
            this.TaxId.Name = "TaxId";
            this.TaxId.ReadOnly = true;
            this.TaxId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TaxId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TaxId.ValueMember = "TaxId";
            // 
            // TaxPercentage
            // 
            this.TaxPercentage.DataPropertyName = "TaxPercentage";
            this.TaxPercentage.HeaderText = "Tax %";
            this.TaxPercentage.Name = "TaxPercentage";
            this.TaxPercentage.ReadOnly = true;
            // 
            // locationIdDataGridViewTextBoxColumn
            // 
            this.locationIdDataGridViewTextBoxColumn.DataPropertyName = "LocationId";
            this.locationIdDataGridViewTextBoxColumn.DataSource = this.locationDTOBindingSource;
            this.locationIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            this.locationIdDataGridViewTextBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.locationIdDataGridViewTextBoxColumn.HeaderText = "Location";
            this.locationIdDataGridViewTextBoxColumn.Name = "locationIdDataGridViewTextBoxColumn";
            this.locationIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.locationIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.locationIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.locationIdDataGridViewTextBoxColumn.ValueMember = "LocationId";
            // 
            // isReceivedDataGridViewTextBoxColumn
            // 
            this.isReceivedDataGridViewTextBoxColumn.DataPropertyName = "IsReceived";
            this.isReceivedDataGridViewTextBoxColumn.HeaderText = "IsReceived";
            this.isReceivedDataGridViewTextBoxColumn.Name = "isReceivedDataGridViewTextBoxColumn";
            this.isReceivedDataGridViewTextBoxColumn.ReadOnly = true;
            this.isReceivedDataGridViewTextBoxColumn.Visible = false;
            // 
            // CurrentStock
            // 
            this.CurrentStock.DataPropertyName = "CurrentStock";
            this.CurrentStock.HeaderText = "Current Stock";
            this.CurrentStock.Name = "CurrentStock";
            this.CurrentStock.ReadOnly = true;
            // 
            // LotDetails
            // 
            this.LotDetails.HeaderText = "Lot Details";
            this.LotDetails.Name = "LotDetails";
            this.LotDetails.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LotDetails.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.LotDetails.Text = "...";
            this.LotDetails.UseColumnTextForButtonValue = true;
            // 
            // PurchaseOrderLineId
            // 
            this.PurchaseOrderLineId.DataPropertyName = "PurchaseOrderLineId";
            this.PurchaseOrderLineId.HeaderText = "PO Line Id";
            this.PurchaseOrderLineId.Name = "PurchaseOrderLineId";
            this.PurchaseOrderLineId.ReadOnly = true;
            // 
            // POQuantity
            // 
            this.POQuantity.DataPropertyName = "POQuantity";
            this.POQuantity.HeaderText = "PO Quantity";
            this.POQuantity.Name = "POQuantity";
            // 
            // UnitPrice
            // 
            this.UnitPrice.DataPropertyName = "POUnitPrice";
            this.UnitPrice.HeaderText = "PO Price";
            this.UnitPrice.Name = "UnitPrice";
            this.UnitPrice.ReadOnly = true;
            // 
            // TaxAmount
            // 
            this.TaxAmount.DataPropertyName = "POTaxAmount";
            this.TaxAmount.HeaderText = "PO Tax";
            this.TaxAmount.Name = "TaxAmount";
            this.TaxAmount.ReadOnly = true;
            // 
            // SubTotal
            // 
            this.SubTotal.DataPropertyName = "POSubtotal";
            this.SubTotal.HeaderText = "PO Sub Total";
            this.SubTotal.Name = "SubTotal";
            this.SubTotal.ReadOnly = true;
            // 
            // RequisitionId
            // 
            this.RequisitionId.DataPropertyName = "RequisitionId";
            this.RequisitionId.HeaderText = "Requisition ID";
            this.RequisitionId.Name = "RequisitionId";
            this.RequisitionId.ReadOnly = true;
            // 
            // RequisitionLineId
            // 
            this.RequisitionLineId.DataPropertyName = "RequisitionLineId";
            this.RequisitionLineId.HeaderText = "Requisition Line ID";
            this.RequisitionLineId.Name = "RequisitionLineId";
            this.RequisitionLineId.ReadOnly = true;
            // 
            // UOM
            // 
            this.UOM.DataPropertyName = "UOM";
            this.UOM.HeaderText = "UOM";
            this.UOM.Name = "UOM";
            this.UOM.ReadOnly = true;
            // 
            // frmOrderReceipts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Beige;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1018, 499);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvReceipt);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvReceiptLines);
            this.Controls.Add(this.grpSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmOrderReceipts";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Receipts for PO:";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_receipts_FormClosed);
            this.Load += new System.EventHandler(this.frm_receipts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReceiptLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseTaxDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryReceiveLinesDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productDTOBindingSource)).EndInit();
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReceipt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryReceiptDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvReceiptLines;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtPONumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtVendorBillNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVendorName;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.DataGridView dgvReceipt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.BindingSource receiptPurchaseOrderReceiveLineBindingSource;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGRN;
        private System.Windows.Forms.BindingSource inventoryReceiptDTOBindingSource;
        private System.Windows.Forms.BindingSource inventoryReceiveLinesDTOBindingSource;
        private System.Windows.Forms.BindingSource productDTOBindingSource;
        private System.Windows.Forms.BindingSource purchaseTaxDTOBindingSource;
        private System.Windows.Forms.BindingSource locationDTOBindingSource;
        private System.Windows.Forms.BindingSource vendorDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn tax_percentage;
        private System.Windows.Forms.DataGridViewCheckBoxColumn tax_inclusive;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiptIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiptAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorBillNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn gatePassNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gRNDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchaseOrderIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiveDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchaseOrderReceiveLineIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vendorItemCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn amount;
        private System.Windows.Forms.DataGridViewComboBoxColumn TaxId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxPercentage;
        private System.Windows.Forms.DataGridViewComboBoxColumn locationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn isReceivedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentStock;
        private System.Windows.Forms.DataGridViewButtonColumn LotDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn PurchaseOrderLineId;
        private System.Windows.Forms.DataGridViewTextBoxColumn POQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequisitionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequisitionLineId;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOM;
    }
}