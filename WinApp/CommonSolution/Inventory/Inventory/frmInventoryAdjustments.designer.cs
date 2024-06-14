namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Required Designer Variables
    /// </summary>
    partial class frmInventoryAdjustments
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
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.cbToLocation = new System.Windows.Forms.ComboBox();
            this.locationDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTransferRemarks = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAdvancedSearch = new System.Windows.Forms.Button();
            this.txtProdBarcode = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.chkPurchaseable = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cbInvLocation = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.inventoryAdjustmentsSummaryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtProdCode = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbAdjustmentType = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAdjRemarks = new System.Windows.Forms.TextBox();
            this.UpDownAdjQty = new System.Windows.Forms.NumericUpDown();
            this.btnAdjust = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.rbFromLocation = new System.Windows.Forms.RadioButton();
            this.rbToLocation = new System.Windows.Forms.RadioButton();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tcInvAdjTransfer = new System.Windows.Forms.TabControl();
            this.tpManual = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.txtTotalCost = new System.Windows.Forms.TextBox();
            this.tpAutomated = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtBarCodeTransferRemarks = new System.Windows.Forms.TextBox();
            this.btnExitForm = new System.Windows.Forms.Button();
            this.btnClearProducts = new System.Windows.Forms.Button();
            this.btnSaveTransfers = new System.Windows.Forms.Button();
            this.dgvTransferProducts = new System.Windows.Forms.DataGridView();
            this.btnRemoveProduct = new System.Windows.Forms.DataGridViewButtonColumn();
            this.BarCodeLotDetails = new System.Windows.Forms.DataGridViewButtonColumn();
            this.grpScanType = new System.Windows.Forms.GroupBox();
            this.btnScanProduct = new System.Windows.Forms.Button();
            this.btnScanLocation = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblLocationId = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblAvlblToSell = new System.Windows.Forms.Label();
            this.lblActive = new System.Windows.Forms.Label();
            this.lblBarCode = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbScannedLocation = new System.Windows.Forms.ComboBox();
            this.productDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.chkSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barcodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductBarcode = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lotIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransferQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtUOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LotDetails = new System.Windows.Forms.DataGridViewButtonColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avlQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sKUDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.allocatedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reorderQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reorderPointDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotControlledDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UOMId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.locationDTOBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryAdjustmentsSummaryBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownAdjQty)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tcInvAdjTransfer.SuspendLayout();
            this.tpManual.SuspendLayout();
            this.tpAutomated.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransferProducts)).BeginInit();
            this.grpScanType.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(138, 41);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(115, 20);
            this.txtDescription.TabIndex = 2;
            this.txtDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDescription_KeyPress);
            // 
            // cbToLocation
            // 
            this.cbToLocation.DataSource = this.locationDTOBindingSource;
            this.cbToLocation.DisplayMember = "Name";
            this.cbToLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbToLocation.FormattingEnabled = true;
            this.cbToLocation.Location = new System.Drawing.Point(232, 23);
            this.cbToLocation.Name = "cbToLocation";
            this.cbToLocation.Size = new System.Drawing.Size(121, 21);
            this.cbToLocation.TabIndex = 20;
            this.cbToLocation.ValueMember = "LocationId";
            this.cbToLocation.SelectedIndexChanged += new System.EventHandler(this.cbToLocation_SelectedIndexChanged);
            // 
            // locationDTOBindingSource
            // 
            this.locationDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.LocationDTO);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Product Code";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Description";
            // 
            // txtTransferRemarks
            // 
            this.txtTransferRemarks.Location = new System.Drawing.Point(90, 54);
            this.txtTransferRemarks.Name = "txtTransferRemarks";
            this.txtTransferRemarks.Size = new System.Drawing.Size(263, 20);
            this.txtTransferRemarks.TabIndex = 21;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnAdvancedSearch);
            this.groupBox1.Controls.Add(this.txtProdBarcode);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.chkPurchaseable);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbInvLocation);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.dgvProducts);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtProdCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(6, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1013, 400);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Product Search";
            // 
            // btnAdvancedSearch
            // 
            this.btnAdvancedSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdvancedSearch.Location = new System.Drawing.Point(750, 39);
            this.btnAdvancedSearch.Name = "btnAdvancedSearch";
            this.btnAdvancedSearch.Size = new System.Drawing.Size(75, 23);
            this.btnAdvancedSearch.TabIndex = 7;
            this.btnAdvancedSearch.Text = "Adv Search";
            this.btnAdvancedSearch.UseVisualStyleBackColor = true;
            this.btnAdvancedSearch.Click += new System.EventHandler(this.btnAdvancedSearch_Click);
            // 
            // txtProdBarcode
            // 
            this.txtProdBarcode.Location = new System.Drawing.Point(264, 41);
            this.txtProdBarcode.Name = "txtProdBarcode";
            this.txtProdBarcode.Size = new System.Drawing.Size(115, 20);
            this.txtProdBarcode.TabIndex = 3;
            this.txtProdBarcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProdBarcode_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(261, 25);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(87, 13);
            this.label15.TabIndex = 18;
            this.label15.Text = "Product Barcode";
            // 
            // chkPurchaseable
            // 
            this.chkPurchaseable.AutoSize = true;
            this.chkPurchaseable.Checked = true;
            this.chkPurchaseable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPurchaseable.Location = new System.Drawing.Point(516, 44);
            this.chkPurchaseable.Name = "chkPurchaseable";
            this.chkPurchaseable.Size = new System.Drawing.Size(122, 17);
            this.chkPurchaseable.TabIndex = 5;
            this.chkPurchaseable.Text = "Inventory Items Only";
            this.chkPurchaseable.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Image = global::Semnox.Parafait.Inventory.Properties.Resources.cancel;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(846, 39);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Exit";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(385, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Location";
            // 
            // cbInvLocation
            // 
            this.cbInvLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbInvLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbInvLocation.DataSource = this.locationDTOBindingSource;
            this.cbInvLocation.DisplayMember = "Name";
            this.cbInvLocation.FormattingEnabled = true;
            this.cbInvLocation.Location = new System.Drawing.Point(388, 41);
            this.cbInvLocation.Name = "cbInvLocation";
            this.cbInvLocation.Size = new System.Drawing.Size(121, 21);
            this.cbInvLocation.TabIndex = 4;
            this.cbInvLocation.ValueMember = "LocationId";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Search Result";
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProducts.AutoGenerateColumns = false;
            this.dgvProducts.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkSelect,
            this.codeDataGridViewTextBoxColumn,
            this.barcodeDataGridViewTextBoxColumn,
            this.ProductBarcode,
            this.lotIdDataGridViewTextBoxColumn,
            this.TransferQuantity,
            this.txtUOM,
            this.lotNumberDataGridViewTextBoxColumn,
            this.LotDetails,
            this.descriptionDataGridViewTextBoxColumn,
            this.locationNameDataGridViewTextBoxColumn,
            this.avlQuantityDataGridViewTextBoxColumn,
            this.sKUDataGridViewTextBoxColumn,
            this.allocatedDataGridViewTextBoxColumn,
            this.totalCostDataGridViewTextBoxColumn,
            this.reorderQuantityDataGridViewTextBoxColumn,
            this.reorderPointDataGridViewTextBoxColumn,
            this.productIdDataGridViewTextBoxColumn,
            this.locationIdDataGridViewTextBoxColumn,
            this.lotControlledDataGridViewCheckBoxColumn,
            this.UOMId});
            this.dgvProducts.DataSource = this.inventoryAdjustmentsSummaryBindingSource;
            this.dgvProducts.Location = new System.Drawing.Point(11, 85);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.RowHeadersVisible = false;
            this.dgvProducts.Size = new System.Drawing.Size(995, 296);
            this.dgvProducts.TabIndex = 9;
            this.dgvProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellContentClick);
            this.dgvProducts.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProducts_CellFormat);
            this.dgvProducts.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellMouseEnter);
            this.dgvProducts.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellMouseLeave);
            this.dgvProducts.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellValidated);
            this.dgvProducts.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProducts_ColumnHeaderMouseClick);
            this.dgvProducts.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvProducts_DataError);
            this.dgvProducts.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dgvProducts_RowStateChanged);
            // 
            // inventoryAdjustmentsSummaryBindingSource
            // 
            this.inventoryAdjustmentsSummaryBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.InventoryAdjustmentsSummaryDTO);
            // 
            // btnSearch
            // 
            this.btnSearch.Image = global::Semnox.Parafait.Inventory.Properties.Resources.search;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(652, 39);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(92, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtProdCode
            // 
            this.txtProdCode.Location = new System.Drawing.Point(11, 41);
            this.txtProdCode.Name = "txtProdCode";
            this.txtProdCode.Size = new System.Drawing.Size(115, 20);
            this.txtProdCode.TabIndex = 1;
            this.txtProdCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProdCode_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cbAdjustmentType);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtAdjRemarks);
            this.groupBox2.Controls.Add(this.UpDownAdjQty);
            this.groupBox2.Controls.Add(this.btnAdjust);
            this.groupBox2.Location = new System.Drawing.Point(604, 414);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(415, 148);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Adjustments";
            // 
            // cbAdjustmentType
            // 
            this.cbAdjustmentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAdjustmentType.FormattingEnabled = true;
            this.cbAdjustmentType.Location = new System.Drawing.Point(82, 85);
            this.cbAdjustmentType.Name = "cbAdjustmentType";
            this.cbAdjustmentType.Size = new System.Drawing.Size(121, 21);
            this.cbAdjustmentType.TabIndex = 26;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(24, 89);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(54, 13);
            this.label16.TabIndex = 18;
            this.label16.Text = "Type";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(24, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Remarks";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(21, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Quantity";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAdjRemarks
            // 
            this.txtAdjRemarks.Location = new System.Drawing.Point(81, 54);
            this.txtAdjRemarks.Name = "txtAdjRemarks";
            this.txtAdjRemarks.Size = new System.Drawing.Size(263, 20);
            this.txtAdjRemarks.TabIndex = 25;
            // 
            // UpDownAdjQty
            // 
            this.UpDownAdjQty.DecimalPlaces = 4;
            this.UpDownAdjQty.Location = new System.Drawing.Point(81, 23);
            this.UpDownAdjQty.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            262144});
            this.UpDownAdjQty.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147221504});
            this.UpDownAdjQty.Name = "UpDownAdjQty";
            this.UpDownAdjQty.Size = new System.Drawing.Size(89, 20);
            this.UpDownAdjQty.TabIndex = 24;
            // 
            // btnAdjust
            // 
            this.btnAdjust.Image = global::Semnox.Parafait.Inventory.Properties.Resources.autoorder;
            this.btnAdjust.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdjust.Location = new System.Drawing.Point(81, 118);
            this.btnAdjust.Name = "btnAdjust";
            this.btnAdjust.Size = new System.Drawing.Size(89, 23);
            this.btnAdjust.TabIndex = 27;
            this.btnAdjust.Text = "Adjust";
            this.btnAdjust.UseVisualStyleBackColor = true;
            this.btnAdjust.Click += new System.EventHandler(this.btnAdjust_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.rbFromLocation);
            this.groupBox3.Controls.Add(this.rbToLocation);
            this.groupBox3.Controls.Add(this.btnTransfer);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtTransferRemarks);
            this.groupBox3.Controls.Add(this.cbToLocation);
            this.groupBox3.Location = new System.Drawing.Point(6, 414);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(415, 108);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Transfers";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(174, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Location";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbFromLocation
            // 
            this.rbFromLocation.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbFromLocation.Checked = true;
            this.rbFromLocation.Location = new System.Drawing.Point(60, 34);
            this.rbFromLocation.Name = "rbFromLocation";
            this.rbFromLocation.Size = new System.Drawing.Size(108, 17);
            this.rbFromLocation.TabIndex = 19;
            this.rbFromLocation.TabStop = true;
            this.rbFromLocation.Text = "Transfer From";
            this.rbFromLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbFromLocation.UseVisualStyleBackColor = true;
            this.rbFromLocation.CheckedChanged += new System.EventHandler(this.rbFromLocation_CheckedChanged);
            // 
            // rbToLocation
            // 
            this.rbToLocation.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbToLocation.Location = new System.Drawing.Point(60, 16);
            this.rbToLocation.Name = "rbToLocation";
            this.rbToLocation.Size = new System.Drawing.Size(108, 17);
            this.rbToLocation.TabIndex = 18;
            this.rbToLocation.Text = "Transfer To";
            this.rbToLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbToLocation.UseVisualStyleBackColor = true;
            this.rbToLocation.CheckedChanged += new System.EventHandler(this.rbToLocation_CheckedChanged);
            // 
            // btnTransfer
            // 
            this.btnTransfer.Image = global::Semnox.Parafait.Inventory.Properties.Resources.AddSelected;
            this.btnTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTransfer.Location = new System.Drawing.Point(90, 80);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(117, 23);
            this.btnTransfer.TabIndex = 22;
            this.btnTransfer.Text = "Transfer";
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(26, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Remarks";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tcInvAdjTransfer
            // 
            this.tcInvAdjTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcInvAdjTransfer.Controls.Add(this.tpManual);
            this.tcInvAdjTransfer.Controls.Add(this.tpAutomated);
            this.tcInvAdjTransfer.Location = new System.Drawing.Point(0, 0);
            this.tcInvAdjTransfer.Name = "tcInvAdjTransfer";
            this.tcInvAdjTransfer.SelectedIndex = 0;
            this.tcInvAdjTransfer.Size = new System.Drawing.Size(1033, 594);
            this.tcInvAdjTransfer.TabIndex = 4;
            this.tcInvAdjTransfer.SelectedIndexChanged += new System.EventHandler(this.tcInvAdjTransfer_SelectedIndexChanged);
            // 
            // tpManual
            // 
            this.tpManual.BackColor = System.Drawing.Color.White;
            this.tpManual.Controls.Add(this.label14);
            this.tpManual.Controls.Add(this.txtTotalCost);
            this.tpManual.Controls.Add(this.groupBox1);
            this.tpManual.Controls.Add(this.groupBox2);
            this.tpManual.Controls.Add(this.groupBox3);
            this.tpManual.Location = new System.Drawing.Point(4, 22);
            this.tpManual.Name = "tpManual";
            this.tpManual.Padding = new System.Windows.Forms.Padding(3);
            this.tpManual.Size = new System.Drawing.Size(1025, 568);
            this.tpManual.TabIndex = 0;
            this.tpManual.Text = "Manual";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.Location = new System.Drawing.Point(428, 422);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(55, 13);
            this.label14.TabIndex = 18;
            this.label14.Text = "Cost Total";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotalCost
            // 
            this.txtTotalCost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtTotalCost.Location = new System.Drawing.Point(488, 419);
            this.txtTotalCost.Name = "txtTotalCost";
            this.txtTotalCost.ReadOnly = true;
            this.txtTotalCost.Size = new System.Drawing.Size(105, 20);
            this.txtTotalCost.TabIndex = 23;
            this.txtTotalCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tpAutomated
            // 
            this.tpAutomated.BackColor = System.Drawing.Color.SkyBlue;
            this.tpAutomated.Controls.Add(this.groupBox5);
            this.tpAutomated.Controls.Add(this.grpScanType);
            this.tpAutomated.Controls.Add(this.groupBox4);
            this.tpAutomated.Location = new System.Drawing.Point(4, 22);
            this.tpAutomated.Name = "tpAutomated";
            this.tpAutomated.Padding = new System.Windows.Forms.Padding(3);
            this.tpAutomated.Size = new System.Drawing.Size(1025, 568);
            this.tpAutomated.TabIndex = 1;
            this.tpAutomated.Text = "Bar Code Scan";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.txtBarCodeTransferRemarks);
            this.groupBox5.Controls.Add(this.btnExitForm);
            this.groupBox5.Controls.Add(this.btnClearProducts);
            this.groupBox5.Controls.Add(this.btnSaveTransfers);
            this.groupBox5.Controls.Add(this.dgvTransferProducts);
            this.groupBox5.Location = new System.Drawing.Point(272, 8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(753, 552);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Products To Transfer";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(114, 500);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 13);
            this.label12.TabIndex = 50;
            this.label12.Text = "Remarks";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBarCodeTransferRemarks
            // 
            this.txtBarCodeTransferRemarks.Location = new System.Drawing.Point(190, 496);
            this.txtBarCodeTransferRemarks.Name = "txtBarCodeTransferRemarks";
            this.txtBarCodeTransferRemarks.Size = new System.Drawing.Size(340, 20);
            this.txtBarCodeTransferRemarks.TabIndex = 5;
            // 
            // btnExitForm
            // 
            this.btnExitForm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExitForm.Image = global::Semnox.Parafait.Inventory.Properties.Resources.cancel;
            this.btnExitForm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExitForm.Location = new System.Drawing.Point(440, 522);
            this.btnExitForm.Name = "btnExitForm";
            this.btnExitForm.Size = new System.Drawing.Size(90, 23);
            this.btnExitForm.TabIndex = 8;
            this.btnExitForm.Text = "Exit";
            this.btnExitForm.UseVisualStyleBackColor = true;
            this.btnExitForm.Click += new System.EventHandler(this.btnExitForm_Click);
            // 
            // btnClearProducts
            // 
            this.btnClearProducts.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClearProducts.Image = global::Semnox.Parafait.Inventory.Properties.Resources.ordercancel1;
            this.btnClearProducts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearProducts.Location = new System.Drawing.Point(315, 522);
            this.btnClearProducts.Name = "btnClearProducts";
            this.btnClearProducts.Size = new System.Drawing.Size(90, 23);
            this.btnClearProducts.TabIndex = 7;
            this.btnClearProducts.Text = "Clear";
            this.btnClearProducts.UseVisualStyleBackColor = true;
            this.btnClearProducts.Click += new System.EventHandler(this.btnClearProducts_Click);
            // 
            // btnSaveTransfers
            // 
            this.btnSaveTransfers.Image = global::Semnox.Parafait.Inventory.Properties.Resources.save;
            this.btnSaveTransfers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveTransfers.Location = new System.Drawing.Point(190, 522);
            this.btnSaveTransfers.Name = "btnSaveTransfers";
            this.btnSaveTransfers.Size = new System.Drawing.Size(90, 23);
            this.btnSaveTransfers.TabIndex = 6;
            this.btnSaveTransfers.Text = "Transfer";
            this.btnSaveTransfers.UseVisualStyleBackColor = true;
            this.btnSaveTransfers.Click += new System.EventHandler(this.btnSaveTransfers_Click);
            // 
            // dgvTransferProducts
            // 
            this.dgvTransferProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTransferProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransferProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnRemoveProduct,
            this.BarCodeLotDetails});
            this.dgvTransferProducts.Location = new System.Drawing.Point(6, 20);
            this.dgvTransferProducts.Name = "dgvTransferProducts";
            this.dgvTransferProducts.Size = new System.Drawing.Size(741, 466);
            this.dgvTransferProducts.TabIndex = 4;
            this.dgvTransferProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTransferProdcuts_CellContentClick);
            // 
            // btnRemoveProduct
            // 
            this.btnRemoveProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveProduct.HeaderText = "Remove";
            this.btnRemoveProduct.Name = "btnRemoveProduct";
            this.btnRemoveProduct.Text = "X";
            this.btnRemoveProduct.UseColumnTextForButtonValue = true;
            // 
            // BarCodeLotDetails
            // 
            this.BarCodeLotDetails.HeaderText = "Lot Details";
            this.BarCodeLotDetails.Name = "BarCodeLotDetails";
            this.BarCodeLotDetails.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BarCodeLotDetails.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.BarCodeLotDetails.Text = "...";
            this.BarCodeLotDetails.UseColumnTextForButtonValue = true;
            // 
            // grpScanType
            // 
            this.grpScanType.Controls.Add(this.btnScanProduct);
            this.grpScanType.Controls.Add(this.btnScanLocation);
            this.grpScanType.Location = new System.Drawing.Point(9, 460);
            this.grpScanType.Name = "grpScanType";
            this.grpScanType.Size = new System.Drawing.Size(257, 100);
            this.grpScanType.TabIndex = 1;
            this.grpScanType.TabStop = false;
            this.grpScanType.Text = "Choose Bar Code Scan Type";
            // 
            // btnScanProduct
            // 
            this.btnScanProduct.BackColor = System.Drawing.Color.CadetBlue;
            this.btnScanProduct.FlatAppearance.BorderColor = System.Drawing.Color.MediumTurquoise;
            this.btnScanProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGreen;
            this.btnScanProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.CadetBlue;
            this.btnScanProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanProduct.ForeColor = System.Drawing.Color.White;
            this.btnScanProduct.Location = new System.Drawing.Point(150, 22);
            this.btnScanProduct.Name = "btnScanProduct";
            this.btnScanProduct.Size = new System.Drawing.Size(94, 63);
            this.btnScanProduct.TabIndex = 3;
            this.btnScanProduct.Text = "Product";
            this.btnScanProduct.UseVisualStyleBackColor = false;
            // 
            // btnScanLocation
            // 
            this.btnScanLocation.BackColor = System.Drawing.Color.CadetBlue;
            this.btnScanLocation.FlatAppearance.BorderColor = System.Drawing.Color.MediumTurquoise;
            this.btnScanLocation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGreen;
            this.btnScanLocation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.CadetBlue;
            this.btnScanLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanLocation.ForeColor = System.Drawing.Color.White;
            this.btnScanLocation.Location = new System.Drawing.Point(11, 22);
            this.btnScanLocation.Name = "btnScanLocation";
            this.btnScanLocation.Size = new System.Drawing.Size(94, 63);
            this.btnScanLocation.TabIndex = 2;
            this.btnScanLocation.Text = "Location";
            this.btnScanLocation.UseVisualStyleBackColor = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblLocationId);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.lblAvlblToSell);
            this.groupBox4.Controls.Add(this.lblActive);
            this.groupBox4.Controls.Add(this.lblBarCode);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.cmbScannedLocation);
            this.groupBox4.Location = new System.Drawing.Point(9, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(257, 446);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Transfer To Location";
            // 
            // lblLocationId
            // 
            this.lblLocationId.AutoSize = true;
            this.lblLocationId.Location = new System.Drawing.Point(108, 61);
            this.lblLocationId.Name = "lblLocationId";
            this.lblLocationId.Size = new System.Drawing.Size(33, 13);
            this.lblLocationId.TabIndex = 8;
            this.lblLocationId.Text = "None";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(12, 61);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "Location Id:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAvlblToSell
            // 
            this.lblAvlblToSell.AutoSize = true;
            this.lblAvlblToSell.Location = new System.Drawing.Point(108, 144);
            this.lblAvlblToSell.Name = "lblAvlblToSell";
            this.lblAvlblToSell.Size = new System.Drawing.Size(33, 13);
            this.lblAvlblToSell.TabIndex = 6;
            this.lblAvlblToSell.Text = "None";
            // 
            // lblActive
            // 
            this.lblActive.AutoSize = true;
            this.lblActive.Location = new System.Drawing.Point(108, 116);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(33, 13);
            this.lblActive.TabIndex = 5;
            this.lblActive.Text = "None";
            // 
            // lblBarCode
            // 
            this.lblBarCode.AutoSize = true;
            this.lblBarCode.Location = new System.Drawing.Point(108, 88);
            this.lblBarCode.Name = "lblBarCode";
            this.lblBarCode.Size = new System.Drawing.Size(33, 13);
            this.lblBarCode.TabIndex = 4;
            this.lblBarCode.Text = "None";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(9, 116);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Active?:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 144);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Available To Sell?:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(11, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Bar Code:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbScannedLocation
            // 
            this.cmbScannedLocation.BackColor = System.Drawing.Color.Azure;
            this.cmbScannedLocation.DisplayMember = "Name";
            this.cmbScannedLocation.DropDownHeight = 196;
            this.cmbScannedLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScannedLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbScannedLocation.ForeColor = System.Drawing.Color.DarkGreen;
            this.cmbScannedLocation.FormattingEnabled = true;
            this.cmbScannedLocation.IntegralHeight = false;
            this.cmbScannedLocation.Location = new System.Drawing.Point(41, 20);
            this.cmbScannedLocation.Name = "cmbScannedLocation";
            this.cmbScannedLocation.Size = new System.Drawing.Size(203, 24);
            this.cmbScannedLocation.TabIndex = 0;
            this.cmbScannedLocation.ValueMember = "LocationId";
            this.cmbScannedLocation.SelectedIndexChanged += new System.EventHandler(this.cmbScannedLocation_SelectedIndexChanged);
            // 
            // chkSelect
            // 
            this.chkSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.chkSelect.FalseValue = "N";
            this.chkSelect.HeaderText = "Select";
            this.chkSelect.Name = "chkSelect";
            this.chkSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chkSelect.TrueValue = "Y";
            this.chkSelect.Width = 53;
            // 
            // codeDataGridViewTextBoxColumn
            // 
            this.codeDataGridViewTextBoxColumn.DataPropertyName = "Code";
            this.codeDataGridViewTextBoxColumn.HeaderText = "Code";
            this.codeDataGridViewTextBoxColumn.Name = "codeDataGridViewTextBoxColumn";
            this.codeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // barcodeDataGridViewTextBoxColumn
            // 
            this.barcodeDataGridViewTextBoxColumn.DataPropertyName = "Barcode";
            this.barcodeDataGridViewTextBoxColumn.HeaderText = "Barcode";
            this.barcodeDataGridViewTextBoxColumn.Name = "barcodeDataGridViewTextBoxColumn";
            this.barcodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ProductBarcode
            // 
            this.ProductBarcode.HeaderText = "";
            this.ProductBarcode.Name = "ProductBarcode";
            this.ProductBarcode.Text = "...";
            this.ProductBarcode.UseColumnTextForButtonValue = true;
            // 
            // lotIdDataGridViewTextBoxColumn
            // 
            this.lotIdDataGridViewTextBoxColumn.DataPropertyName = "LotId";
            this.lotIdDataGridViewTextBoxColumn.HeaderText = "LotId";
            this.lotIdDataGridViewTextBoxColumn.Name = "lotIdDataGridViewTextBoxColumn";
            this.lotIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.lotIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // TransferQuantity
            // 
            this.TransferQuantity.HeaderText = "Transfer Quantity";
            this.TransferQuantity.Name = "TransferQuantity";
            // 
            // txtUOM
            // 
            this.txtUOM.HeaderText = "UOM";
            this.txtUOM.Name = "txtUOM";
            this.txtUOM.ReadOnly = true;
            this.txtUOM.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.txtUOM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // lotNumberDataGridViewTextBoxColumn
            // 
            this.lotNumberDataGridViewTextBoxColumn.DataPropertyName = "LotNumber";
            this.lotNumberDataGridViewTextBoxColumn.HeaderText = "LotNumber";
            this.lotNumberDataGridViewTextBoxColumn.Name = "lotNumberDataGridViewTextBoxColumn";
            this.lotNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // LotDetails
            // 
            this.LotDetails.HeaderText = "Lot Details";
            this.LotDetails.Name = "LotDetails";
            this.LotDetails.Text = "...";
            this.LotDetails.UseColumnTextForButtonValue = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // locationNameDataGridViewTextBoxColumn
            // 
            this.locationNameDataGridViewTextBoxColumn.DataPropertyName = "LocationName";
            this.locationNameDataGridViewTextBoxColumn.HeaderText = "Location Name";
            this.locationNameDataGridViewTextBoxColumn.Name = "locationNameDataGridViewTextBoxColumn";
            this.locationNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // avlQuantityDataGridViewTextBoxColumn
            // 
            this.avlQuantityDataGridViewTextBoxColumn.DataPropertyName = "AvlQuantity";
            this.avlQuantityDataGridViewTextBoxColumn.HeaderText = "Avl Qty";
            this.avlQuantityDataGridViewTextBoxColumn.Name = "avlQuantityDataGridViewTextBoxColumn";
            this.avlQuantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sKUDataGridViewTextBoxColumn
            // 
            this.sKUDataGridViewTextBoxColumn.DataPropertyName = "SKU";
            this.sKUDataGridViewTextBoxColumn.HeaderText = "SKU";
            this.sKUDataGridViewTextBoxColumn.Name = "sKUDataGridViewTextBoxColumn";
            this.sKUDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // allocatedDataGridViewTextBoxColumn
            // 
            this.allocatedDataGridViewTextBoxColumn.DataPropertyName = "Allocated";
            this.allocatedDataGridViewTextBoxColumn.HeaderText = "Allocated";
            this.allocatedDataGridViewTextBoxColumn.Name = "allocatedDataGridViewTextBoxColumn";
            this.allocatedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // totalCostDataGridViewTextBoxColumn
            // 
            this.totalCostDataGridViewTextBoxColumn.DataPropertyName = "TotalCost";
            this.totalCostDataGridViewTextBoxColumn.HeaderText = "Total Cost";
            this.totalCostDataGridViewTextBoxColumn.Name = "totalCostDataGridViewTextBoxColumn";
            this.totalCostDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // reorderQuantityDataGridViewTextBoxColumn
            // 
            this.reorderQuantityDataGridViewTextBoxColumn.DataPropertyName = "ReorderQuantity";
            this.reorderQuantityDataGridViewTextBoxColumn.HeaderText = "Reorder Quantity";
            this.reorderQuantityDataGridViewTextBoxColumn.Name = "reorderQuantityDataGridViewTextBoxColumn";
            this.reorderQuantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // reorderPointDataGridViewTextBoxColumn
            // 
            this.reorderPointDataGridViewTextBoxColumn.DataPropertyName = "ReorderPoint";
            this.reorderPointDataGridViewTextBoxColumn.HeaderText = "Reorder Point";
            this.reorderPointDataGridViewTextBoxColumn.Name = "reorderPointDataGridViewTextBoxColumn";
            this.reorderPointDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // productIdDataGridViewTextBoxColumn
            // 
            this.productIdDataGridViewTextBoxColumn.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn.HeaderText = "ProductId";
            this.productIdDataGridViewTextBoxColumn.Name = "productIdDataGridViewTextBoxColumn";
            this.productIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.productIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // locationIdDataGridViewTextBoxColumn
            // 
            this.locationIdDataGridViewTextBoxColumn.DataPropertyName = "LocationId";
            this.locationIdDataGridViewTextBoxColumn.HeaderText = "LocationId";
            this.locationIdDataGridViewTextBoxColumn.Name = "locationIdDataGridViewTextBoxColumn";
            this.locationIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.locationIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // lotControlledDataGridViewCheckBoxColumn
            // 
            this.lotControlledDataGridViewCheckBoxColumn.DataPropertyName = "LotControlled";
            this.lotControlledDataGridViewCheckBoxColumn.HeaderText = "LotControlled";
            this.lotControlledDataGridViewCheckBoxColumn.Name = "lotControlledDataGridViewCheckBoxColumn";
            this.lotControlledDataGridViewCheckBoxColumn.ReadOnly = true;
            this.lotControlledDataGridViewCheckBoxColumn.Visible = false;
            // 
            // UOMId
            // 
            this.UOMId.DataPropertyName = "UOMId";
            this.UOMId.HeaderText = "UOMId";
            this.UOMId.Name = "UOMId";
            this.UOMId.ReadOnly = true;
            this.UOMId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.UOMId.Visible = false;
            // 
            // frmInventoryAdjustments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1037, 594);
            this.Controls.Add(this.tcInvAdjTransfer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmInventoryAdjustments";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Inventory Adjustments and Transfers";
            this.Activated += new System.EventHandler(this.frmInventoryAdjustments_Activated);
            this.Load += new System.EventHandler(this.frmInventoryAdjustments_Load);
            ((System.ComponentModel.ISupportInitialize)(this.locationDTOBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryAdjustmentsSummaryBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownAdjQty)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tcInvAdjTransfer.ResumeLayout(false);
            this.tpManual.ResumeLayout(false);
            this.tpManual.PerformLayout();
            this.tpAutomated.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransferProducts)).EndInit();
            this.grpScanType.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.ComboBox cbToLocation;
        private System.Windows.Forms.Button btnAdjust;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTransferRemarks;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtProdCode;
        //private System.Windows.Forms.ComboBox cbInvLocation;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cbInvLocation; 
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown UpDownAdjQty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAdjRemarks;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkPurchaseable;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rbFromLocation;
        private System.Windows.Forms.RadioButton rbToLocation;
        private System.Windows.Forms.TabControl tcInvAdjTransfer;
        private System.Windows.Forms.TabPage tpManual;
        private System.Windows.Forms.TabPage tpAutomated;
        private System.Windows.Forms.GroupBox grpScanType;
        private System.Windows.Forms.Button btnScanProduct;
        private System.Windows.Forms.Button btnScanLocation;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cmbScannedLocation;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblAvlblToSell;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.Label lblBarCode;
        private System.Windows.Forms.Label lblLocationId;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DataGridView dgvTransferProducts;
        private System.Windows.Forms.Button btnSaveTransfers;
        private System.Windows.Forms.Button btnClearProducts;
        private System.Windows.Forms.Button btnExitForm;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtBarCodeTransferRemarks;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtTotalCost;
        private System.Windows.Forms.TextBox txtProdBarcode;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnAdvancedSearch;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cbAdjustmentType;
        private System.Windows.Forms.DataGridViewButtonColumn btnRemoveProduct;
        private System.Windows.Forms.DataGridViewButtonColumn BarCodeLotDetails;
        private System.Windows.Forms.BindingSource locationDTOBindingSource;
        private System.Windows.Forms.BindingSource productDTOBindingSource;
        private System.Windows.Forms.BindingSource inventoryAdjustmentsSummaryBindingSource;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn barcodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn ProductBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransferQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn LotDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn locationNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn avlQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sKUDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn allocatedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reorderQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reorderPointDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn locationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn lotControlledDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOMId;
    }
}

