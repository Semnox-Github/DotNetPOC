namespace Parafait_POS.SalesReturn
{
    partial class frmSalesReturn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSalesReturn));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tcReturnExchange = new System.Windows.Forms.TabControl();
            this.tpTrxLookup = new System.Windows.Forms.TabPage();
            this.btnShowNumPad = new System.Windows.Forms.Button();
            this.btnSearchTrx = new System.Windows.Forms.Button();
            this.btnTLClose = new System.Windows.Forms.Button();
            this.buttonTLNext = new System.Windows.Forms.Button();
            this.txtTrxID = new System.Windows.Forms.TextBox();
            this.dtToTrxDate = new System.Windows.Forms.DateTimePicker();
            this.txtToTrxDate = new System.Windows.Forms.TextBox();
            this.dtFromTrxDate = new System.Windows.Forms.DateTimePicker();
            this.txtFromTrxDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvTransaction = new System.Windows.Forms.DataGridView();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.btnChooseSchedules = new System.Windows.Forms.Button();
            this.btnCheckAvailability = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.tpProductLookup = new System.Windows.Forms.TabPage();
            this.btnInitiateExchange = new System.Windows.Forms.Button();
            this.btnInitiateReturn = new System.Windows.Forms.Button();
            this.btnPLClose = new System.Windows.Forms.Button();
            this.btnPLBack = new System.Windows.Forms.Button();
            this.gbPLSearchCriteria = new System.Windows.Forms.GroupBox();
            this.btnKeyboardProductLookup = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btnClearProductSearch = new System.Windows.Forms.Button();
            this.btnProductSearch = new System.Windows.Forms.Button();
            this.btnPLSKUSearch = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dgvProductDetails = new System.Windows.Forms.DataGridView();
            this.chkSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.product_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tax_inclusive_price = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.discount_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.last_sale_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MoreInfo = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.tpReturn = new System.Windows.Forms.TabPage();
            this.btnCloseReturn = new System.Windows.Forms.Button();
            this.btnCancelReturn = new System.Windows.Forms.Button();
            this.btnConfirmReturn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.dgvReturnProducts = new System.Windows.Forms.DataGridView();
            this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnProduct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxIncl = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DiscAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastSalePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalReturnLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductDetails = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ReturnLineRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpExchange = new System.Windows.Forms.TabPage();
            this.btnCloseExchange = new System.Windows.Forms.Button();
            this.btnCancelExchange = new System.Windows.Forms.Button();
            this.btnConfirmExchange = new System.Windows.Forms.Button();
            this.dgvExchangeProducts = new System.Windows.Forms.DataGridView();
            this.SelectProduct = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ExchangeProductID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExchangeProduct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExchangeQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExchangePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExchangeAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExchangeTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxInclv = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ExchangeLastSalePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExchangePdtLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExchangeMoreInfo = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ExchangeProductRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbExchangeSelect = new System.Windows.Forms.GroupBox();
            this.btnExchangeProductLookup = new System.Windows.Forms.Button();
            this.btnKeyboardExchangbtnExchangeProductLookup = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.btnClearExchangeProductSearch = new System.Windows.Forms.Button();
            this.btnExchangeProductSearch = new System.Windows.Forms.Button();
            this.btnExchangeSKUSearch = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtExchangeProduct = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dgvExchangeReturnProducts = new System.Windows.Forms.DataGridView();
            this.ReturnProductID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnPdtPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnTaxAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxInclusive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.discountAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SalePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExchangeLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalExchangeLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MoreInformation = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ExchangeReturnLineRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxMessageLine = new System.Windows.Forms.TextBox();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Trx_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Net_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxdiscount_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tcReturnExchange.SuspendLayout();
            this.tpTrxLookup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransaction)).BeginInit();
            this.tpProductLookup.SuspendLayout();
            this.gbPLSearchCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductDetails)).BeginInit();
            this.tpReturn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturnProducts)).BeginInit();
            this.tpExchange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExchangeProducts)).BeginInit();
            this.gbExchangeSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExchangeReturnProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // tcReturnExchange
            // 
            this.tcReturnExchange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcReturnExchange.Controls.Add(this.tpTrxLookup);
            this.tcReturnExchange.Controls.Add(this.tpProductLookup);
            this.tcReturnExchange.Controls.Add(this.tpReturn);
            this.tcReturnExchange.Controls.Add(this.tpExchange);
            this.tcReturnExchange.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcReturnExchange.ItemSize = new System.Drawing.Size(93, 40);
            this.tcReturnExchange.Location = new System.Drawing.Point(1, 0);
            this.tcReturnExchange.Name = "tcReturnExchange";
            this.tcReturnExchange.SelectedIndex = 0;
            this.tcReturnExchange.Size = new System.Drawing.Size(1119, 566);
            this.tcReturnExchange.TabIndex = 68;
            // 
            // tpTrxLookup
            // 
            this.tpTrxLookup.Controls.Add(this.btnShowNumPad);
            this.tpTrxLookup.Controls.Add(this.btnSearchTrx);
            this.tpTrxLookup.Controls.Add(this.btnTLClose);
            this.tpTrxLookup.Controls.Add(this.buttonTLNext);
            this.tpTrxLookup.Controls.Add(this.txtTrxID);
            this.tpTrxLookup.Controls.Add(this.dtToTrxDate);
            this.tpTrxLookup.Controls.Add(this.txtToTrxDate);
            this.tpTrxLookup.Controls.Add(this.dtFromTrxDate);
            this.tpTrxLookup.Controls.Add(this.txtFromTrxDate);
            this.tpTrxLookup.Controls.Add(this.label3);
            this.tpTrxLookup.Controls.Add(this.dgvTransaction);
            this.tpTrxLookup.Controls.Add(this.label25);
            this.tpTrxLookup.Controls.Add(this.label24);
            this.tpTrxLookup.Controls.Add(this.btnChooseSchedules);
            this.tpTrxLookup.Controls.Add(this.btnCheckAvailability);
            this.tpTrxLookup.Controls.Add(this.label1);
            this.tpTrxLookup.Controls.Add(this.btnFind);
            this.tpTrxLookup.Location = new System.Drawing.Point(4, 44);
            this.tpTrxLookup.Name = "tpTrxLookup";
            this.tpTrxLookup.Padding = new System.Windows.Forms.Padding(3);
            this.tpTrxLookup.Size = new System.Drawing.Size(1111, 518);
            this.tpTrxLookup.TabIndex = 0;
            this.tpTrxLookup.Text = "Transaction Lookup";
            this.tpTrxLookup.UseVisualStyleBackColor = true;
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
            this.btnShowNumPad.Location = new System.Drawing.Point(294, 18);
            this.btnShowNumPad.Name = "btnShowNumPad";
            this.btnShowNumPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowNumPad.TabIndex = 109;
            this.btnShowNumPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowNumPad.UseVisualStyleBackColor = false;
            this.btnShowNumPad.Click += new System.EventHandler(this.btnShowNumPad_Click);
            // 
            // btnSearchTrx
            // 
            this.btnSearchTrx.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchTrx.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearchTrx.BackgroundImage")));
            this.btnSearchTrx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearchTrx.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnSearchTrx.FlatAppearance.BorderSize = 0;
            this.btnSearchTrx.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSearchTrx.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearchTrx.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearchTrx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchTrx.ForeColor = System.Drawing.Color.White;
            this.btnSearchTrx.Location = new System.Drawing.Point(854, 19);
            this.btnSearchTrx.Name = "btnSearchTrx";
            this.btnSearchTrx.Size = new System.Drawing.Size(181, 34);
            this.btnSearchTrx.TabIndex = 82;
            this.btnSearchTrx.Text = "OK";
            this.btnSearchTrx.UseVisualStyleBackColor = false;
            this.btnSearchTrx.Click += new System.EventHandler(this.btnSearchTrx_Click);
            // 
            // btnTLClose
            // 
            this.btnTLClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTLClose.BackColor = System.Drawing.Color.Transparent;
            this.btnTLClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnTLClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTLClose.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnTLClose.FlatAppearance.BorderSize = 0;
            this.btnTLClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTLClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTLClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTLClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTLClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTLClose.ForeColor = System.Drawing.Color.White;
            this.btnTLClose.Location = new System.Drawing.Point(601, 447);
            this.btnTLClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTLClose.Name = "btnTLClose";
            this.btnTLClose.Size = new System.Drawing.Size(181, 55);
            this.btnTLClose.TabIndex = 81;
            this.btnTLClose.Text = "Close";
            this.btnTLClose.UseVisualStyleBackColor = false;
            this.btnTLClose.Click += new System.EventHandler(this.btnTLClose_Click);
            // 
            // buttonTLNext
            // 
            this.buttonTLNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonTLNext.BackColor = System.Drawing.Color.Transparent;
            this.buttonTLNext.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.buttonTLNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonTLNext.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.buttonTLNext.FlatAppearance.BorderSize = 0;
            this.buttonTLNext.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonTLNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonTLNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonTLNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTLNext.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTLNext.ForeColor = System.Drawing.Color.White;
            this.buttonTLNext.Location = new System.Drawing.Point(320, 449);
            this.buttonTLNext.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTLNext.Name = "buttonTLNext";
            this.buttonTLNext.Size = new System.Drawing.Size(181, 55);
            this.buttonTLNext.TabIndex = 80;
            this.buttonTLNext.Text = "Next";
            this.buttonTLNext.UseVisualStyleBackColor = false;
            this.buttonTLNext.Click += new System.EventHandler(this.buttonTLNext_Click);
            // 
            // txtTrxID
            // 
            this.txtTrxID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTrxID.Location = new System.Drawing.Point(78, 24);
            this.txtTrxID.Name = "txtTrxID";
            this.txtTrxID.Size = new System.Drawing.Size(206, 22);
            this.txtTrxID.TabIndex = 79;
            this.txtTrxID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTrxID_KeyPress);
            // 
            // dtToTrxDate
            // 
            this.dtToTrxDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.dtToTrxDate.Location = new System.Drawing.Point(776, 25);
            this.dtToTrxDate.Name = "dtToTrxDate";
            this.dtToTrxDate.Size = new System.Drawing.Size(19, 22);
            this.dtToTrxDate.TabIndex = 78;
            this.dtToTrxDate.ValueChanged += new System.EventHandler(this.dtToTrxDate_ValueChanged);
            // 
            // txtToTrxDate
            // 
            this.txtToTrxDate.Location = new System.Drawing.Point(636, 25);
            this.txtToTrxDate.Name = "txtToTrxDate";
            this.txtToTrxDate.Size = new System.Drawing.Size(139, 22);
            this.txtToTrxDate.TabIndex = 77;
            // 
            // dtFromTrxDate
            // 
            this.dtFromTrxDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.dtFromTrxDate.Location = new System.Drawing.Point(560, 24);
            this.dtFromTrxDate.Name = "dtFromTrxDate";
            this.dtFromTrxDate.Size = new System.Drawing.Size(19, 22);
            this.dtFromTrxDate.TabIndex = 76;
            this.dtFromTrxDate.ValueChanged += new System.EventHandler(this.dtFromTrxDate_ValueChanged);
            // 
            // txtFromTrxDate
            // 
            this.txtFromTrxDate.Location = new System.Drawing.Point(420, 24);
            this.txtFromTrxDate.Name = "txtFromTrxDate";
            this.txtFromTrxDate.Size = new System.Drawing.Size(139, 22);
            this.txtFromTrxDate.TabIndex = 75;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 16);
            this.label3.TabIndex = 68;
            this.label3.Text = "Transaction Details";
            // 
            // dgvTransaction
            // 
            this.dgvTransaction.AllowUserToAddRows = false;
            this.dgvTransaction.AllowUserToDeleteRows = false;
            this.dgvTransaction.AllowUserToResizeColumns = false;
            this.dgvTransaction.AllowUserToResizeRows = false;
            this.dgvTransaction.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTransaction.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvTransaction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvTransaction.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTransaction.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTransaction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransaction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Trx_no,
            this.Date,
            this.trxAmount,
            this.trxTax,
            this.Net_amount,
            this.trxdiscount_amount,
            this.trxStatus,
            this.trxRemarks});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransaction.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTransaction.EnableHeadersVisualStyles = false;
            this.dgvTransaction.Location = new System.Drawing.Point(23, 81);
            this.dgvTransaction.MultiSelect = false;
            this.dgvTransaction.Name = "dgvTransaction";
            this.dgvTransaction.ReadOnly = true;
            this.dgvTransaction.RowHeadersVisible = false;
            this.dgvTransaction.RowTemplate.Height = 40;
            this.dgvTransaction.Size = new System.Drawing.Size(1032, 350);
            this.dgvTransaction.TabIndex = 67;
            this.dgvTransaction.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTransaction_CellContentClick);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(22, 27);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(49, 16);
            this.label25.TabIndex = 64;
            this.label25.Text = "Trx ID:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(601, 28);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(27, 16);
            this.label24.TabIndex = 63;
            this.label24.Text = "To:";
            // 
            // btnChooseSchedules
            // 
            this.btnChooseSchedules.BackColor = System.Drawing.Color.Transparent;
            this.btnChooseSchedules.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnChooseSchedules.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnChooseSchedules.FlatAppearance.BorderSize = 0;
            this.btnChooseSchedules.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnChooseSchedules.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChooseSchedules.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChooseSchedules.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseSchedules.ForeColor = System.Drawing.Color.White;
            this.btnChooseSchedules.Location = new System.Drawing.Point(563, 131);
            this.btnChooseSchedules.Name = "btnChooseSchedules";
            this.btnChooseSchedules.Size = new System.Drawing.Size(139, 30);
            this.btnChooseSchedules.TabIndex = 61;
            this.btnChooseSchedules.Text = "Choose Schedule";
            this.btnChooseSchedules.UseVisualStyleBackColor = false;
            // 
            // btnCheckAvailability
            // 
            this.btnCheckAvailability.BackColor = System.Drawing.Color.Transparent;
            this.btnCheckAvailability.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCheckAvailability.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCheckAvailability.FlatAppearance.BorderSize = 0;
            this.btnCheckAvailability.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCheckAvailability.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCheckAvailability.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCheckAvailability.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckAvailability.ForeColor = System.Drawing.Color.White;
            this.btnCheckAvailability.Location = new System.Drawing.Point(205, 232);
            this.btnCheckAvailability.Name = "btnCheckAvailability";
            this.btnCheckAvailability.Size = new System.Drawing.Size(135, 29);
            this.btnCheckAvailability.TabIndex = 58;
            this.btnCheckAvailability.Text = "Check Availability";
            this.btnCheckAvailability.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(369, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "From:";
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.Color.Transparent;
            this.btnFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFind.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnFind.FlatAppearance.BorderSize = 0;
            this.btnFind.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnFind.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFind.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.ForeColor = System.Drawing.Color.White;
            this.btnFind.Location = new System.Drawing.Point(421, 87);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(121, 25);
            this.btnFind.TabIndex = 3;
            this.btnFind.Text = "Product Details";
            this.btnFind.UseVisualStyleBackColor = false;
            // 
            // tpProductLookup
            // 
            this.tpProductLookup.Controls.Add(this.btnInitiateExchange);
            this.tpProductLookup.Controls.Add(this.btnInitiateReturn);
            this.tpProductLookup.Controls.Add(this.btnPLClose);
            this.tpProductLookup.Controls.Add(this.btnPLBack);
            this.tpProductLookup.Controls.Add(this.gbPLSearchCriteria);
            this.tpProductLookup.Controls.Add(this.dgvProductDetails);
            this.tpProductLookup.Controls.Add(this.label6);
            this.tpProductLookup.Location = new System.Drawing.Point(4, 44);
            this.tpProductLookup.Name = "tpProductLookup";
            this.tpProductLookup.Padding = new System.Windows.Forms.Padding(3);
            this.tpProductLookup.Size = new System.Drawing.Size(1111, 518);
            this.tpProductLookup.TabIndex = 1;
            this.tpProductLookup.Text = "Product Lookup";
            this.tpProductLookup.UseVisualStyleBackColor = true;
            // 
            // btnInitiateExchange
            // 
            this.btnInitiateExchange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInitiateExchange.BackColor = System.Drawing.Color.Transparent;
            this.btnInitiateExchange.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnInitiateExchange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnInitiateExchange.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnInitiateExchange.FlatAppearance.BorderSize = 0;
            this.btnInitiateExchange.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnInitiateExchange.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnInitiateExchange.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnInitiateExchange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInitiateExchange.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInitiateExchange.ForeColor = System.Drawing.Color.White;
            this.btnInitiateExchange.Location = new System.Drawing.Point(293, 448);
            this.btnInitiateExchange.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInitiateExchange.Name = "btnInitiateExchange";
            this.btnInitiateExchange.Size = new System.Drawing.Size(181, 55);
            this.btnInitiateExchange.TabIndex = 85;
            this.btnInitiateExchange.Text = "Exchange";
            this.btnInitiateExchange.UseVisualStyleBackColor = false;
            this.btnInitiateExchange.Click += new System.EventHandler(this.btnInitiateExchange_Click);
            // 
            // btnInitiateReturn
            // 
            this.btnInitiateReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInitiateReturn.BackColor = System.Drawing.Color.Transparent;
            this.btnInitiateReturn.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnInitiateReturn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnInitiateReturn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnInitiateReturn.FlatAppearance.BorderSize = 0;
            this.btnInitiateReturn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnInitiateReturn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnInitiateReturn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnInitiateReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInitiateReturn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInitiateReturn.ForeColor = System.Drawing.Color.White;
            this.btnInitiateReturn.Location = new System.Drawing.Point(24, 448);
            this.btnInitiateReturn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInitiateReturn.Name = "btnInitiateReturn";
            this.btnInitiateReturn.Size = new System.Drawing.Size(181, 55);
            this.btnInitiateReturn.TabIndex = 84;
            this.btnInitiateReturn.Text = "Return";
            this.btnInitiateReturn.UseVisualStyleBackColor = false;
            this.btnInitiateReturn.Click += new System.EventHandler(this.btnInitiateReturn_Click);
            // 
            // btnPLClose
            // 
            this.btnPLClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPLClose.BackColor = System.Drawing.Color.Transparent;
            this.btnPLClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPLClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPLClose.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPLClose.FlatAppearance.BorderSize = 0;
            this.btnPLClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPLClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPLClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPLClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPLClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPLClose.ForeColor = System.Drawing.Color.White;
            this.btnPLClose.Location = new System.Drawing.Point(850, 448);
            this.btnPLClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPLClose.Name = "btnPLClose";
            this.btnPLClose.Size = new System.Drawing.Size(181, 55);
            this.btnPLClose.TabIndex = 83;
            this.btnPLClose.Text = "Close";
            this.btnPLClose.UseVisualStyleBackColor = false;
            this.btnPLClose.Click += new System.EventHandler(this.btnPLClose_Click);
            // 
            // btnPLBack
            // 
            this.btnPLBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPLBack.BackColor = System.Drawing.Color.Transparent;
            this.btnPLBack.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPLBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPLBack.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPLBack.FlatAppearance.BorderSize = 0;
            this.btnPLBack.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPLBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPLBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPLBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPLBack.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPLBack.ForeColor = System.Drawing.Color.White;
            this.btnPLBack.Location = new System.Drawing.Point(569, 448);
            this.btnPLBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPLBack.Name = "btnPLBack";
            this.btnPLBack.Size = new System.Drawing.Size(181, 55);
            this.btnPLBack.TabIndex = 82;
            this.btnPLBack.Text = "Back";
            this.btnPLBack.UseVisualStyleBackColor = false;
            this.btnPLBack.Click += new System.EventHandler(this.btnPLBack_Click);
            // 
            // gbPLSearchCriteria
            // 
            this.gbPLSearchCriteria.Controls.Add(this.btnKeyboardProductLookup);
            this.gbPLSearchCriteria.Controls.Add(this.label11);
            this.gbPLSearchCriteria.Controls.Add(this.btnClearProductSearch);
            this.gbPLSearchCriteria.Controls.Add(this.btnProductSearch);
            this.gbPLSearchCriteria.Controls.Add(this.btnPLSKUSearch);
            this.gbPLSearchCriteria.Controls.Add(this.label5);
            this.gbPLSearchCriteria.Controls.Add(this.txtProduct);
            this.gbPLSearchCriteria.Controls.Add(this.label9);
            this.gbPLSearchCriteria.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPLSearchCriteria.Location = new System.Drawing.Point(26, 12);
            this.gbPLSearchCriteria.Name = "gbPLSearchCriteria";
            this.gbPLSearchCriteria.Size = new System.Drawing.Size(1045, 172);
            this.gbPLSearchCriteria.TabIndex = 81;
            this.gbPLSearchCriteria.TabStop = false;
            this.gbPLSearchCriteria.Text = "Search Criteria";
            // 
            // btnKeyboardProductLookup
            // 
            this.btnKeyboardProductLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKeyboardProductLookup.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyboardProductLookup.CausesValidation = false;
            this.btnKeyboardProductLookup.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnKeyboardProductLookup.FlatAppearance.BorderSize = 0;
            this.btnKeyboardProductLookup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnKeyboardProductLookup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnKeyboardProductLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyboardProductLookup.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKeyboardProductLookup.ForeColor = System.Drawing.Color.Black;
            this.btnKeyboardProductLookup.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnKeyboardProductLookup.Location = new System.Drawing.Point(587, 18);
            this.btnKeyboardProductLookup.Name = "btnKeyboardProductLookup";
            this.btnKeyboardProductLookup.Size = new System.Drawing.Size(42, 40);
            this.btnKeyboardProductLookup.TabIndex = 106;
            this.btnKeyboardProductLookup.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnKeyboardProductLookup.UseVisualStyleBackColor = false;
            this.btnKeyboardProductLookup.Click += new System.EventHandler(this.btnKeyboardProductLookup_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(140, 57);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(335, 16);
            this.label11.TabIndex = 105;
            this.label11.Text = "(Search by Name/ Description/ Inventory Code/ Barcode)";
            // 
            // btnClearProductSearch
            // 
            this.btnClearProductSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnClearProductSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClearProductSearch.BackgroundImage")));
            this.btnClearProductSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClearProductSearch.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClearProductSearch.FlatAppearance.BorderSize = 0;
            this.btnClearProductSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClearProductSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClearProductSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClearProductSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearProductSearch.ForeColor = System.Drawing.Color.White;
            this.btnClearProductSearch.Location = new System.Drawing.Point(308, 130);
            this.btnClearProductSearch.Name = "btnClearProductSearch";
            this.btnClearProductSearch.Size = new System.Drawing.Size(181, 34);
            this.btnClearProductSearch.TabIndex = 104;
            this.btnClearProductSearch.Text = "Clear";
            this.btnClearProductSearch.UseVisualStyleBackColor = false;
            this.btnClearProductSearch.Click += new System.EventHandler(this.btnClearProductSearch_Click);
            // 
            // btnProductSearch
            // 
            this.btnProductSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnProductSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnProductSearch.BackgroundImage")));
            this.btnProductSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProductSearch.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnProductSearch.FlatAppearance.BorderSize = 0;
            this.btnProductSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnProductSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProductSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProductSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductSearch.ForeColor = System.Drawing.Color.White;
            this.btnProductSearch.Location = new System.Drawing.Point(69, 130);
            this.btnProductSearch.Name = "btnProductSearch";
            this.btnProductSearch.Size = new System.Drawing.Size(181, 34);
            this.btnProductSearch.TabIndex = 103;
            this.btnProductSearch.Text = "OK";
            this.btnProductSearch.UseVisualStyleBackColor = false;
            this.btnProductSearch.Click += new System.EventHandler(this.btnProductSearch_Click);
            // 
            // btnPLSKUSearch
            // 
            this.btnPLSKUSearch.Location = new System.Drawing.Point(138, 86);
            this.btnPLSKUSearch.Name = "btnPLSKUSearch";
            this.btnPLSKUSearch.Size = new System.Drawing.Size(79, 35);
            this.btnPLSKUSearch.TabIndex = 102;
            this.btnPLSKUSearch.Text = "...";
            this.btnPLSKUSearch.UseVisualStyleBackColor = true;
            this.btnPLSKUSearch.Click += new System.EventHandler(this.btnPLSKUSearch_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(91, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 16);
            this.label5.TabIndex = 101;
            this.label5.Text = "SKU:";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(140, 33);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(440, 22);
            this.txtProduct.TabIndex = 91;
            this.txtProduct.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProduct_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(71, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 16);
            this.label9.TabIndex = 90;
            this.label9.Text = "Product:";
            // 
            // dgvProductDetails
            // 
            this.dgvProductDetails.AllowUserToAddRows = false;
            this.dgvProductDetails.AllowUserToDeleteRows = false;
            this.dgvProductDetails.AllowUserToResizeColumns = false;
            this.dgvProductDetails.AllowUserToResizeRows = false;
            this.dgvProductDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvProductDetails.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvProductDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvProductDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProductDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvProductDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkSelect,
            this.product_id,
            this.product,
            this.quantity,
            this.price,
            this.amount,
            this.Tax,
            this.tax_inclusive_price,
            this.discount_amount,
            this.last_sale_price,
            this.Line,
            this.OriginalLine,
            this.Remarks,
            this.MoreInfo});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProductDetails.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvProductDetails.EnableHeadersVisualStyles = false;
            this.dgvProductDetails.Location = new System.Drawing.Point(24, 212);
            this.dgvProductDetails.Name = "dgvProductDetails";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProductDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvProductDetails.RowHeadersVisible = false;
            this.dgvProductDetails.RowTemplate.Height = 40;
            this.dgvProductDetails.Size = new System.Drawing.Size(1050, 210);
            this.dgvProductDetails.TabIndex = 75;
            this.dgvProductDetails.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductDetails_CellContentClick);
            // 
            // chkSelect
            // 
            this.chkSelect.FalseValue = "N";
            this.chkSelect.HeaderText = "Select";
            this.chkSelect.Name = "chkSelect";
            this.chkSelect.TrueValue = "Y";
            this.chkSelect.Width = 53;
            // 
            // product_id
            // 
            this.product_id.HeaderText = "Product Id";
            this.product_id.Name = "product_id";
            this.product_id.ReadOnly = true;
            this.product_id.Visible = false;
            this.product_id.Width = 97;
            // 
            // product
            // 
            this.product.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.product.HeaderText = "Product";
            this.product.Name = "product";
            this.product.ReadOnly = true;
            // 
            // quantity
            // 
            this.quantity.HeaderText = "Quantity";
            this.quantity.Name = "quantity";
            this.quantity.ReadOnly = true;
            this.quantity.Width = 85;
            // 
            // price
            // 
            this.price.HeaderText = "Price";
            this.price.Name = "price";
            this.price.ReadOnly = true;
            this.price.Width = 65;
            // 
            // amount
            // 
            this.amount.HeaderText = "Amount";
            this.amount.Name = "amount";
            this.amount.ReadOnly = true;
            this.amount.Width = 81;
            // 
            // Tax
            // 
            this.Tax.HeaderText = "Tax";
            this.Tax.Name = "Tax";
            this.Tax.ReadOnly = true;
            this.Tax.Width = 55;
            // 
            // tax_inclusive_price
            // 
            this.tax_inclusive_price.FalseValue = "N";
            this.tax_inclusive_price.HeaderText = "Tax Incl.";
            this.tax_inclusive_price.Name = "tax_inclusive_price";
            this.tax_inclusive_price.ReadOnly = true;
            this.tax_inclusive_price.TrueValue = "Y";
            this.tax_inclusive_price.Width = 60;
            // 
            // discount_amount
            // 
            this.discount_amount.HeaderText = "Discount Amount";
            this.discount_amount.Name = "discount_amount";
            this.discount_amount.ReadOnly = true;
            this.discount_amount.Width = 127;
            // 
            // last_sale_price
            // 
            this.last_sale_price.HeaderText = "Least Sale Price";
            this.last_sale_price.Name = "last_sale_price";
            this.last_sale_price.ReadOnly = true;
            this.last_sale_price.Width = 124;
            // 
            // Line
            // 
            this.Line.HeaderText = "Line";
            this.Line.Name = "Line";
            this.Line.Visible = false;
            this.Line.Width = 60;
            // 
            // OriginalLine
            // 
            this.OriginalLine.HeaderText = "OriginalLine";
            this.OriginalLine.Name = "OriginalLine";
            this.OriginalLine.Visible = false;
            this.OriginalLine.Width = 111;
            // 
            // Remarks
            // 
            this.Remarks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            this.Remarks.ReadOnly = true;
            this.Remarks.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Remarks.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MoreInfo
            // 
            this.MoreInfo.HeaderText = "More Info";
            this.MoreInfo.Name = "MoreInfo";
            this.MoreInfo.Text = "...";
            this.MoreInfo.UseColumnTextForButtonValue = true;
            this.MoreInfo.Width = 66;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 16);
            this.label6.TabIndex = 74;
            this.label6.Text = "Product Details";
            // 
            // tpReturn
            // 
            this.tpReturn.Controls.Add(this.btnCloseReturn);
            this.tpReturn.Controls.Add(this.btnCancelReturn);
            this.tpReturn.Controls.Add(this.btnConfirmReturn);
            this.tpReturn.Controls.Add(this.label10);
            this.tpReturn.Controls.Add(this.dgvReturnProducts);
            this.tpReturn.Location = new System.Drawing.Point(4, 44);
            this.tpReturn.Name = "tpReturn";
            this.tpReturn.Padding = new System.Windows.Forms.Padding(3);
            this.tpReturn.Size = new System.Drawing.Size(1111, 518);
            this.tpReturn.TabIndex = 2;
            this.tpReturn.Text = "Return";
            this.tpReturn.UseVisualStyleBackColor = true;
            // 
            // btnCloseReturn
            // 
            this.btnCloseReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCloseReturn.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseReturn.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCloseReturn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCloseReturn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCloseReturn.FlatAppearance.BorderSize = 0;
            this.btnCloseReturn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCloseReturn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCloseReturn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCloseReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseReturn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseReturn.ForeColor = System.Drawing.Color.White;
            this.btnCloseReturn.Location = new System.Drawing.Point(717, 449);
            this.btnCloseReturn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCloseReturn.Name = "btnCloseReturn";
            this.btnCloseReturn.Size = new System.Drawing.Size(181, 55);
            this.btnCloseReturn.TabIndex = 88;
            this.btnCloseReturn.Text = "Close";
            this.btnCloseReturn.UseVisualStyleBackColor = false;
            this.btnCloseReturn.Click += new System.EventHandler(this.btnCloseReturn_Click);
            // 
            // btnCancelReturn
            // 
            this.btnCancelReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelReturn.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelReturn.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancelReturn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelReturn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCancelReturn.FlatAppearance.BorderSize = 0;
            this.btnCancelReturn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancelReturn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancelReturn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancelReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelReturn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelReturn.ForeColor = System.Drawing.Color.White;
            this.btnCancelReturn.Location = new System.Drawing.Point(461, 449);
            this.btnCancelReturn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancelReturn.Name = "btnCancelReturn";
            this.btnCancelReturn.Size = new System.Drawing.Size(181, 55);
            this.btnCancelReturn.TabIndex = 87;
            this.btnCancelReturn.Text = "Cancel";
            this.btnCancelReturn.UseVisualStyleBackColor = false;
            this.btnCancelReturn.Click += new System.EventHandler(this.btnCancelReturn_Click);
            // 
            // btnConfirmReturn
            // 
            this.btnConfirmReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConfirmReturn.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirmReturn.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnConfirmReturn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConfirmReturn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnConfirmReturn.FlatAppearance.BorderSize = 0;
            this.btnConfirmReturn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnConfirmReturn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConfirmReturn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConfirmReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmReturn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmReturn.ForeColor = System.Drawing.Color.White;
            this.btnConfirmReturn.Location = new System.Drawing.Point(192, 449);
            this.btnConfirmReturn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfirmReturn.Name = "btnConfirmReturn";
            this.btnConfirmReturn.Size = new System.Drawing.Size(181, 55);
            this.btnConfirmReturn.TabIndex = 86;
            this.btnConfirmReturn.Text = "Confirm";
            this.btnConfirmReturn.UseVisualStyleBackColor = false;
            this.btnConfirmReturn.Click += new System.EventHandler(this.btnConfirmReturn_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(27, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(150, 16);
            this.label10.TabIndex = 79;
            this.label10.Text = "Return Product Details";
            // 
            // dgvReturnProducts
            // 
            this.dgvReturnProducts.AllowUserToAddRows = false;
            this.dgvReturnProducts.AllowUserToDeleteRows = false;
            this.dgvReturnProducts.AllowUserToResizeColumns = false;
            this.dgvReturnProducts.AllowUserToResizeRows = false;
            this.dgvReturnProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvReturnProducts.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvReturnProducts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvReturnProducts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReturnProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvReturnProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReturnProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductId,
            this.ReturnProduct,
            this.Qty,
            this.ReturnPrice,
            this.ReturnAmount,
            this.ReturnTax,
            this.TaxIncl,
            this.DiscAmount,
            this.LastSalePrice,
            this.ReturnLine,
            this.OriginalReturnLine,
            this.OriginalPrice,
            this.ProductDetails,
            this.ReturnLineRemarks});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReturnProducts.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvReturnProducts.EnableHeadersVisualStyles = false;
            this.dgvReturnProducts.Location = new System.Drawing.Point(26, 46);
            this.dgvReturnProducts.Name = "dgvReturnProducts";
            this.dgvReturnProducts.RowHeadersVisible = false;
            this.dgvReturnProducts.RowTemplate.Height = 40;
            this.dgvReturnProducts.Size = new System.Drawing.Size(1050, 380);
            this.dgvReturnProducts.TabIndex = 78;
            this.dgvReturnProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReturnProducts_CellClick);
            this.dgvReturnProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReturnProducts_CellContentClick);
            this.dgvReturnProducts.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvReturnProducts_CellMouseClick);
            // 
            // ProductId
            // 
            this.ProductId.HeaderText = "Product ID";
            this.ProductId.Name = "ProductId";
            this.ProductId.Visible = false;
            this.ProductId.Width = 79;
            // 
            // ReturnProduct
            // 
            this.ReturnProduct.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ReturnProduct.HeaderText = "Product";
            this.ReturnProduct.Name = "ReturnProduct";
            // 
            // Qty
            // 
            this.Qty.HeaderText = "Qty";
            this.Qty.Name = "Qty";
            this.Qty.Width = 53;
            // 
            // ReturnPrice
            // 
            this.ReturnPrice.HeaderText = "Price";
            this.ReturnPrice.Name = "ReturnPrice";
            this.ReturnPrice.Width = 65;
            // 
            // ReturnAmount
            // 
            this.ReturnAmount.HeaderText = "Amount";
            this.ReturnAmount.Name = "ReturnAmount";
            this.ReturnAmount.Width = 81;
            // 
            // ReturnTax
            // 
            this.ReturnTax.HeaderText = "Tax";
            this.ReturnTax.Name = "ReturnTax";
            this.ReturnTax.Width = 55;
            // 
            // TaxIncl
            // 
            this.TaxIncl.FalseValue = "N";
            this.TaxIncl.HeaderText = "Tax Incl.";
            this.TaxIncl.Name = "TaxIncl";
            this.TaxIncl.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TaxIncl.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TaxIncl.TrueValue = "Y";
            this.TaxIncl.Width = 79;
            // 
            // DiscAmount
            // 
            this.DiscAmount.HeaderText = "Discount Amount";
            this.DiscAmount.Name = "DiscAmount";
            this.DiscAmount.Width = 127;
            // 
            // LastSalePrice
            // 
            this.LastSalePrice.HeaderText = "Least Sale Price";
            this.LastSalePrice.Name = "LastSalePrice";
            this.LastSalePrice.Width = 124;
            // 
            // ReturnLine
            // 
            this.ReturnLine.HeaderText = "Line";
            this.ReturnLine.Name = "ReturnLine";
            this.ReturnLine.Visible = false;
            this.ReturnLine.Width = 60;
            // 
            // OriginalReturnLine
            // 
            this.OriginalReturnLine.HeaderText = "OriginalReturnLine";
            this.OriginalReturnLine.Name = "OriginalReturnLine";
            this.OriginalReturnLine.Visible = false;
            this.OriginalReturnLine.Width = 153;
            // 
            // OriginalPrice
            // 
            this.OriginalPrice.HeaderText = "OriginalPrice";
            this.OriginalPrice.Name = "OriginalPrice";
            this.OriginalPrice.Visible = false;
            this.OriginalPrice.Width = 116;
            // 
            // ProductDetails
            // 
            this.ProductDetails.HeaderText = "More Info";
            this.ProductDetails.Name = "ProductDetails";
            this.ProductDetails.Text = "...";
            this.ProductDetails.UseColumnTextForButtonValue = true;
            this.ProductDetails.Width = 66;
            // 
            // ReturnLineRemarks
            // 
            this.ReturnLineRemarks.HeaderText = "Remarks";
            this.ReturnLineRemarks.Name = "ReturnLineRemarks";
            this.ReturnLineRemarks.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ReturnLineRemarks.Width = 87;
            // 
            // tpExchange
            // 
            this.tpExchange.Controls.Add(this.btnCloseExchange);
            this.tpExchange.Controls.Add(this.btnCancelExchange);
            this.tpExchange.Controls.Add(this.btnConfirmExchange);
            this.tpExchange.Controls.Add(this.dgvExchangeProducts);
            this.tpExchange.Controls.Add(this.gbExchangeSelect);
            this.tpExchange.Controls.Add(this.dgvExchangeReturnProducts);
            this.tpExchange.Controls.Add(this.label2);
            this.tpExchange.Controls.Add(this.label4);
            this.tpExchange.Location = new System.Drawing.Point(4, 44);
            this.tpExchange.Name = "tpExchange";
            this.tpExchange.Padding = new System.Windows.Forms.Padding(3);
            this.tpExchange.Size = new System.Drawing.Size(1111, 518);
            this.tpExchange.TabIndex = 3;
            this.tpExchange.Text = "Exchange";
            this.tpExchange.UseVisualStyleBackColor = true;
            // 
            // btnCloseExchange
            // 
            this.btnCloseExchange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCloseExchange.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseExchange.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCloseExchange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCloseExchange.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCloseExchange.FlatAppearance.BorderSize = 0;
            this.btnCloseExchange.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCloseExchange.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCloseExchange.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCloseExchange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseExchange.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseExchange.ForeColor = System.Drawing.Color.White;
            this.btnCloseExchange.Location = new System.Drawing.Point(682, 454);
            this.btnCloseExchange.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCloseExchange.Name = "btnCloseExchange";
            this.btnCloseExchange.Size = new System.Drawing.Size(181, 55);
            this.btnCloseExchange.TabIndex = 111;
            this.btnCloseExchange.Text = "Close";
            this.btnCloseExchange.UseVisualStyleBackColor = false;
            this.btnCloseExchange.Click += new System.EventHandler(this.btnCloseExchange_Click);
            // 
            // btnCancelExchange
            // 
            this.btnCancelExchange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelExchange.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelExchange.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancelExchange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelExchange.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCancelExchange.FlatAppearance.BorderSize = 0;
            this.btnCancelExchange.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancelExchange.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancelExchange.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancelExchange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelExchange.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelExchange.ForeColor = System.Drawing.Color.White;
            this.btnCancelExchange.Location = new System.Drawing.Point(423, 454);
            this.btnCancelExchange.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancelExchange.Name = "btnCancelExchange";
            this.btnCancelExchange.Size = new System.Drawing.Size(181, 55);
            this.btnCancelExchange.TabIndex = 110;
            this.btnCancelExchange.Text = "Cancel";
            this.btnCancelExchange.UseVisualStyleBackColor = false;
            this.btnCancelExchange.Click += new System.EventHandler(this.btnCancelExchange_Click);
            // 
            // btnConfirmExchange
            // 
            this.btnConfirmExchange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConfirmExchange.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirmExchange.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnConfirmExchange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConfirmExchange.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnConfirmExchange.FlatAppearance.BorderSize = 0;
            this.btnConfirmExchange.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnConfirmExchange.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConfirmExchange.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConfirmExchange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmExchange.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmExchange.ForeColor = System.Drawing.Color.White;
            this.btnConfirmExchange.Location = new System.Drawing.Point(160, 454);
            this.btnConfirmExchange.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfirmExchange.Name = "btnConfirmExchange";
            this.btnConfirmExchange.Size = new System.Drawing.Size(181, 55);
            this.btnConfirmExchange.TabIndex = 109;
            this.btnConfirmExchange.Text = "Confirm";
            this.btnConfirmExchange.UseVisualStyleBackColor = false;
            this.btnConfirmExchange.Click += new System.EventHandler(this.btnConfirmExchange_Click);
            // 
            // dgvExchangeProducts
            // 
            this.dgvExchangeProducts.AllowUserToAddRows = false;
            this.dgvExchangeProducts.AllowUserToDeleteRows = false;
            this.dgvExchangeProducts.AllowUserToResizeColumns = false;
            this.dgvExchangeProducts.AllowUserToResizeRows = false;
            this.dgvExchangeProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvExchangeProducts.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvExchangeProducts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvExchangeProducts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvExchangeProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvExchangeProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExchangeProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectProduct,
            this.ExchangeProductID,
            this.ExchangeProduct,
            this.ExchangeQuantity,
            this.ExchangePrice,
            this.ExchangeAmount,
            this.ExchangeTax,
            this.TaxInclv,
            this.ExchangeLastSalePrice,
            this.ExchangePdtLine,
            this.ExchangeMoreInfo,
            this.ExchangeProductRemarks});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvExchangeProducts.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvExchangeProducts.EnableHeadersVisualStyles = false;
            this.dgvExchangeProducts.Location = new System.Drawing.Point(37, 296);
            this.dgvExchangeProducts.Name = "dgvExchangeProducts";
            this.dgvExchangeProducts.RowHeadersVisible = false;
            this.dgvExchangeProducts.RowTemplate.Height = 40;
            this.dgvExchangeProducts.Size = new System.Drawing.Size(1051, 145);
            this.dgvExchangeProducts.TabIndex = 108;
            this.dgvExchangeProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvExchangeProducts_CellClick);
            this.dgvExchangeProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvExchangeProducts_CellContentClick);
            this.dgvExchangeProducts.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvExchangeProducts_CellMouseClick);
            this.dgvExchangeProducts.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvExchangeProducts_CellValueChanged);
            // 
            // SelectProduct
            // 
            this.SelectProduct.FalseValue = "N";
            this.SelectProduct.HeaderText = "Select";
            this.SelectProduct.Name = "SelectProduct";
            this.SelectProduct.TrueValue = "Y";
            this.SelectProduct.Width = 53;
            // 
            // ExchangeProductID
            // 
            this.ExchangeProductID.HeaderText = "Product ID";
            this.ExchangeProductID.Name = "ExchangeProductID";
            this.ExchangeProductID.Visible = false;
            this.ExchangeProductID.Width = 98;
            // 
            // ExchangeProduct
            // 
            this.ExchangeProduct.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ExchangeProduct.HeaderText = "Product";
            this.ExchangeProduct.Name = "ExchangeProduct";
            // 
            // ExchangeQuantity
            // 
            this.ExchangeQuantity.HeaderText = "Quantity";
            this.ExchangeQuantity.Name = "ExchangeQuantity";
            this.ExchangeQuantity.Width = 85;
            // 
            // ExchangePrice
            // 
            this.ExchangePrice.HeaderText = "Price";
            this.ExchangePrice.Name = "ExchangePrice";
            this.ExchangePrice.Width = 65;
            // 
            // ExchangeAmount
            // 
            this.ExchangeAmount.HeaderText = "Amount";
            this.ExchangeAmount.Name = "ExchangeAmount";
            this.ExchangeAmount.Width = 81;
            // 
            // ExchangeTax
            // 
            this.ExchangeTax.HeaderText = "Tax";
            this.ExchangeTax.Name = "ExchangeTax";
            this.ExchangeTax.Width = 55;
            // 
            // TaxInclv
            // 
            this.TaxInclv.FalseValue = "N";
            this.TaxInclv.HeaderText = "Tax Incl.";
            this.TaxInclv.Name = "TaxInclv";
            this.TaxInclv.TrueValue = "Y";
            this.TaxInclv.Width = 60;
            // 
            // ExchangeLastSalePrice
            // 
            this.ExchangeLastSalePrice.HeaderText = "Least Sale Price";
            this.ExchangeLastSalePrice.Name = "ExchangeLastSalePrice";
            this.ExchangeLastSalePrice.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ExchangeLastSalePrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ExchangeLastSalePrice.Width = 105;
            // 
            // ExchangePdtLine
            // 
            this.ExchangePdtLine.HeaderText = "Line";
            this.ExchangePdtLine.Name = "ExchangePdtLine";
            this.ExchangePdtLine.Visible = false;
            this.ExchangePdtLine.Width = 60;
            // 
            // ExchangeMoreInfo
            // 
            this.ExchangeMoreInfo.HeaderText = "More Info";
            this.ExchangeMoreInfo.Name = "ExchangeMoreInfo";
            this.ExchangeMoreInfo.Text = "...";
            this.ExchangeMoreInfo.UseColumnTextForButtonValue = true;
            this.ExchangeMoreInfo.Width = 66;
            // 
            // ExchangeProductRemarks
            // 
            this.ExchangeProductRemarks.HeaderText = "Remarks";
            this.ExchangeProductRemarks.Name = "ExchangeProductRemarks";
            this.ExchangeProductRemarks.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ExchangeProductRemarks.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ExchangeProductRemarks.Width = 68;
            // 
            // gbExchangeSelect
            // 
            this.gbExchangeSelect.Controls.Add(this.btnExchangeProductLookup);
            this.gbExchangeSelect.Controls.Add(this.btnKeyboardExchangbtnExchangeProductLookup);
            this.gbExchangeSelect.Controls.Add(this.label12);
            this.gbExchangeSelect.Controls.Add(this.btnClearExchangeProductSearch);
            this.gbExchangeSelect.Controls.Add(this.btnExchangeProductSearch);
            this.gbExchangeSelect.Controls.Add(this.btnExchangeSKUSearch);
            this.gbExchangeSelect.Controls.Add(this.label7);
            this.gbExchangeSelect.Controls.Add(this.txtExchangeProduct);
            this.gbExchangeSelect.Controls.Add(this.label8);
            this.gbExchangeSelect.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbExchangeSelect.Location = new System.Drawing.Point(37, 151);
            this.gbExchangeSelect.Name = "gbExchangeSelect";
            this.gbExchangeSelect.Size = new System.Drawing.Size(1051, 120);
            this.gbExchangeSelect.TabIndex = 106;
            this.gbExchangeSelect.TabStop = false;
            this.gbExchangeSelect.Text = "Exchange Product Search Criteria";
            // 
            // btnExchangeProductLookup
            // 
            this.btnExchangeProductLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExchangeProductLookup.BackColor = System.Drawing.Color.Transparent;
            this.btnExchangeProductLookup.CausesValidation = false;
            this.btnExchangeProductLookup.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnExchangeProductLookup.FlatAppearance.BorderSize = 0;
            this.btnExchangeProductLookup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExchangeProductLookup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExchangeProductLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExchangeProductLookup.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExchangeProductLookup.ForeColor = System.Drawing.Color.Black;
            this.btnExchangeProductLookup.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnExchangeProductLookup.Location = new System.Drawing.Point(586, 16);
            this.btnExchangeProductLookup.Name = "btnExchangeProductLookup";
            this.btnExchangeProductLookup.Size = new System.Drawing.Size(42, 40);
            this.btnExchangeProductLookup.TabIndex = 109;
            this.btnExchangeProductLookup.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExchangeProductLookup.UseVisualStyleBackColor = false;
            this.btnExchangeProductLookup.Click += new System.EventHandler(this.btnExchangeProductLookup_Click);
            // 
            // btnKeyboardExchangbtnExchangeProductLookup
            // 
            this.btnKeyboardExchangbtnExchangeProductLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKeyboardExchangbtnExchangeProductLookup.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyboardExchangbtnExchangeProductLookup.CausesValidation = false;
            this.btnKeyboardExchangbtnExchangeProductLookup.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnKeyboardExchangbtnExchangeProductLookup.FlatAppearance.BorderSize = 0;
            this.btnKeyboardExchangbtnExchangeProductLookup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnKeyboardExchangbtnExchangeProductLookup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnKeyboardExchangbtnExchangeProductLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyboardExchangbtnExchangeProductLookup.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKeyboardExchangbtnExchangeProductLookup.ForeColor = System.Drawing.Color.Black;
            this.btnKeyboardExchangbtnExchangeProductLookup.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnKeyboardExchangbtnExchangeProductLookup.Location = new System.Drawing.Point(639, -36);
            this.btnKeyboardExchangbtnExchangeProductLookup.Name = "btnKeyboardExchangbtnExchangeProductLookup";
            this.btnKeyboardExchangbtnExchangeProductLookup.Size = new System.Drawing.Size(42, 40);
            this.btnKeyboardExchangbtnExchangeProductLookup.TabIndex = 108;
            this.btnKeyboardExchangbtnExchangeProductLookup.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnKeyboardExchangbtnExchangeProductLookup.UseVisualStyleBackColor = false;
            this.btnKeyboardExchangbtnExchangeProductLookup.Click += new System.EventHandler(this.btnExchangeProductLookup_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(140, 52);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(335, 16);
            this.label12.TabIndex = 107;
            this.label12.Text = "(Search by Name/ Description/ Inventory Code/ Barcode)";
            // 
            // btnClearExchangeProductSearch
            // 
            this.btnClearExchangeProductSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnClearExchangeProductSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClearExchangeProductSearch.BackgroundImage")));
            this.btnClearExchangeProductSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClearExchangeProductSearch.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClearExchangeProductSearch.FlatAppearance.BorderSize = 0;
            this.btnClearExchangeProductSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClearExchangeProductSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClearExchangeProductSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClearExchangeProductSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearExchangeProductSearch.ForeColor = System.Drawing.Color.White;
            this.btnClearExchangeProductSearch.Location = new System.Drawing.Point(852, 21);
            this.btnClearExchangeProductSearch.Name = "btnClearExchangeProductSearch";
            this.btnClearExchangeProductSearch.Size = new System.Drawing.Size(170, 34);
            this.btnClearExchangeProductSearch.TabIndex = 106;
            this.btnClearExchangeProductSearch.Text = "Clear";
            this.btnClearExchangeProductSearch.UseVisualStyleBackColor = false;
            this.btnClearExchangeProductSearch.Click += new System.EventHandler(this.btnClearExchangeProductSearch_Click);
            // 
            // btnExchangeProductSearch
            // 
            this.btnExchangeProductSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnExchangeProductSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExchangeProductSearch.BackgroundImage")));
            this.btnExchangeProductSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExchangeProductSearch.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnExchangeProductSearch.FlatAppearance.BorderSize = 0;
            this.btnExchangeProductSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExchangeProductSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExchangeProductSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExchangeProductSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExchangeProductSearch.ForeColor = System.Drawing.Color.White;
            this.btnExchangeProductSearch.Location = new System.Drawing.Point(670, 21);
            this.btnExchangeProductSearch.Name = "btnExchangeProductSearch";
            this.btnExchangeProductSearch.Size = new System.Drawing.Size(170, 34);
            this.btnExchangeProductSearch.TabIndex = 105;
            this.btnExchangeProductSearch.Text = "OK";
            this.btnExchangeProductSearch.UseVisualStyleBackColor = false;
            this.btnExchangeProductSearch.Click += new System.EventHandler(this.btnExchangeProductSearch_Click);
            // 
            // btnExchangeSKUSearch
            // 
            this.btnExchangeSKUSearch.Location = new System.Drawing.Point(138, 78);
            this.btnExchangeSKUSearch.Name = "btnExchangeSKUSearch";
            this.btnExchangeSKUSearch.Size = new System.Drawing.Size(79, 35);
            this.btnExchangeSKUSearch.TabIndex = 102;
            this.btnExchangeSKUSearch.Text = "...";
            this.btnExchangeSKUSearch.UseVisualStyleBackColor = true;
            this.btnExchangeSKUSearch.Click += new System.EventHandler(this.btnExchangeSKUSearch_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(91, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 16);
            this.label7.TabIndex = 101;
            this.label7.Text = "SKU:";
            // 
            // txtExchangeProduct
            // 
            this.txtExchangeProduct.Location = new System.Drawing.Point(140, 28);
            this.txtExchangeProduct.Name = "txtExchangeProduct";
            this.txtExchangeProduct.Size = new System.Drawing.Size(440, 22);
            this.txtExchangeProduct.TabIndex = 91;
            this.txtExchangeProduct.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtExchangeProduct_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(71, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 16);
            this.label8.TabIndex = 90;
            this.label8.Text = "Product:";
            // 
            // dgvExchangeReturnProducts
            // 
            this.dgvExchangeReturnProducts.AllowUserToAddRows = false;
            this.dgvExchangeReturnProducts.AllowUserToDeleteRows = false;
            this.dgvExchangeReturnProducts.AllowUserToResizeColumns = false;
            this.dgvExchangeReturnProducts.AllowUserToResizeRows = false;
            this.dgvExchangeReturnProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvExchangeReturnProducts.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvExchangeReturnProducts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvExchangeReturnProducts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvExchangeReturnProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvExchangeReturnProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExchangeReturnProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReturnProductID,
            this.ReturnProductName,
            this.ReturnQuantity,
            this.ReturnPdtPrice,
            this.ReturnAmt,
            this.ReturnTaxAmt,
            this.TaxInclusive,
            this.discountAmt,
            this.SalePrice,
            this.ExchangeLine,
            this.OriginalExchangeLine,
            this.MoreInformation,
            this.ExchangeReturnLineRemarks});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvExchangeReturnProducts.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgvExchangeReturnProducts.EnableHeadersVisualStyles = false;
            this.dgvExchangeReturnProducts.Location = new System.Drawing.Point(37, 29);
            this.dgvExchangeReturnProducts.Name = "dgvExchangeReturnProducts";
            this.dgvExchangeReturnProducts.RowHeadersVisible = false;
            this.dgvExchangeReturnProducts.RowTemplate.Height = 40;
            this.dgvExchangeReturnProducts.Size = new System.Drawing.Size(1050, 110);
            this.dgvExchangeReturnProducts.TabIndex = 105;
            this.dgvExchangeReturnProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvExchangeReturnProducts_CellClick);
            this.dgvExchangeReturnProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvExchangeReturnProducts_CellContentClick);
            this.dgvExchangeReturnProducts.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvExchangeReturnProducts_CellMouseClick);
            // 
            // ReturnProductID
            // 
            this.ReturnProductID.HeaderText = "Product ID";
            this.ReturnProductID.Name = "ReturnProductID";
            this.ReturnProductID.Visible = false;
            this.ReturnProductID.Width = 79;
            // 
            // ReturnProductName
            // 
            this.ReturnProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ReturnProductName.HeaderText = "Product";
            this.ReturnProductName.Name = "ReturnProductName";
            // 
            // ReturnQuantity
            // 
            this.ReturnQuantity.HeaderText = "Quantity";
            this.ReturnQuantity.Name = "ReturnQuantity";
            this.ReturnQuantity.Width = 85;
            // 
            // ReturnPdtPrice
            // 
            this.ReturnPdtPrice.HeaderText = "Price";
            this.ReturnPdtPrice.Name = "ReturnPdtPrice";
            this.ReturnPdtPrice.Width = 65;
            // 
            // ReturnAmt
            // 
            this.ReturnAmt.HeaderText = "Amount";
            this.ReturnAmt.Name = "ReturnAmt";
            this.ReturnAmt.Width = 81;
            // 
            // ReturnTaxAmt
            // 
            this.ReturnTaxAmt.HeaderText = "Tax";
            this.ReturnTaxAmt.Name = "ReturnTaxAmt";
            this.ReturnTaxAmt.Width = 55;
            // 
            // TaxInclusive
            // 
            this.TaxInclusive.FalseValue = "N";
            this.TaxInclusive.HeaderText = "Tax Incl.";
            this.TaxInclusive.Name = "TaxInclusive";
            this.TaxInclusive.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TaxInclusive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TaxInclusive.TrueValue = "Y";
            this.TaxInclusive.Width = 79;
            // 
            // discountAmt
            // 
            this.discountAmt.HeaderText = "Discount Amount";
            this.discountAmt.Name = "discountAmt";
            this.discountAmt.Width = 127;
            // 
            // SalePrice
            // 
            this.SalePrice.HeaderText = "Least Sale Price";
            this.SalePrice.Name = "SalePrice";
            this.SalePrice.Width = 124;
            // 
            // ExchangeLine
            // 
            this.ExchangeLine.HeaderText = "Line";
            this.ExchangeLine.Name = "ExchangeLine";
            this.ExchangeLine.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ExchangeLine.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ExchangeLine.Visible = false;
            this.ExchangeLine.Width = 41;
            // 
            // OriginalExchangeLine
            // 
            this.OriginalExchangeLine.HeaderText = "OriginalExchangeLine";
            this.OriginalExchangeLine.Name = "OriginalExchangeLine";
            this.OriginalExchangeLine.Visible = false;
            this.OriginalExchangeLine.Width = 174;
            // 
            // MoreInformation
            // 
            this.MoreInformation.HeaderText = "More Info";
            this.MoreInformation.Name = "MoreInformation";
            this.MoreInformation.Text = "...";
            this.MoreInformation.UseColumnTextForButtonValue = true;
            this.MoreInformation.Width = 66;
            // 
            // ExchangeReturnLineRemarks
            // 
            this.ExchangeReturnLineRemarks.HeaderText = "Remarks";
            this.ExchangeReturnLineRemarks.Name = "ExchangeReturnLineRemarks";
            this.ExchangeReturnLineRemarks.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ExchangeReturnLineRemarks.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ExchangeReturnLineRemarks.Width = 68;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 16);
            this.label2.TabIndex = 104;
            this.label2.Text = "Return Product Details";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(171, 16);
            this.label4.TabIndex = 100;
            this.label4.Text = "Exchange Product Details";
            // 
            // textBoxMessageLine
            // 
            this.textBoxMessageLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessageLine.BackColor = System.Drawing.Color.PapayaWhip;
            this.textBoxMessageLine.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessageLine.ForeColor = System.Drawing.Color.Firebrick;
            this.textBoxMessageLine.Location = new System.Drawing.Point(0, 568);
            this.textBoxMessageLine.Name = "textBoxMessageLine";
            this.textBoxMessageLine.Size = new System.Drawing.Size(1119, 26);
            this.textBoxMessageLine.TabIndex = 69;
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ID.HeaderText = "Trx ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 69;
            // 
            // Trx_no
            // 
            this.Trx_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Trx_no.HeaderText = "Trx no";
            this.Trx_no.Name = "Trx_no";
            this.Trx_no.ReadOnly = true;
            this.Trx_no.Width = 72;
            // 
            // Date
            // 
            this.Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 61;
            // 
            // trxAmount
            // 
            this.trxAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.trxAmount.HeaderText = "Price";
            this.trxAmount.Name = "trxAmount";
            this.trxAmount.ReadOnly = true;
            // 
            // trxTax
            // 
            this.trxTax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.trxTax.HeaderText = "Tax";
            this.trxTax.Name = "trxTax";
            this.trxTax.ReadOnly = true;
            // 
            // Net_amount
            // 
            this.Net_amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Net_amount.HeaderText = "Net Amount";
            this.Net_amount.Name = "Net_amount";
            this.Net_amount.ReadOnly = true;
            // 
            // trxdiscount_amount
            // 
            this.trxdiscount_amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.trxdiscount_amount.HeaderText = "Discount Amount";
            this.trxdiscount_amount.Name = "trxdiscount_amount";
            this.trxdiscount_amount.ReadOnly = true;
            // 
            // trxStatus
            // 
            this.trxStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.trxStatus.HeaderText = "Status";
            this.trxStatus.Name = "trxStatus";
            this.trxStatus.ReadOnly = true;
            // 
            // trxRemarks
            // 
            this.trxRemarks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.trxRemarks.HeaderText = "Remarks";
            this.trxRemarks.Name = "trxRemarks";
            this.trxRemarks.ReadOnly = true;
            // 
            // frmSalesReturn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Linen;
            this.ClientSize = new System.Drawing.Size(1120, 595);
            this.ControlBox = false;
            this.Controls.Add(this.textBoxMessageLine);
            this.Controls.Add(this.tcReturnExchange);
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "frmSalesReturn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Return/Exchange";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSalesReturn_FormClosing);
            this.Load += new System.EventHandler(this.frmSalesReturn_Load);
            this.tcReturnExchange.ResumeLayout(false);
            this.tpTrxLookup.ResumeLayout(false);
            this.tpTrxLookup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransaction)).EndInit();
            this.tpProductLookup.ResumeLayout(false);
            this.tpProductLookup.PerformLayout();
            this.gbPLSearchCriteria.ResumeLayout(false);
            this.gbPLSearchCriteria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductDetails)).EndInit();
            this.tpReturn.ResumeLayout(false);
            this.tpReturn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturnProducts)).EndInit();
            this.tpExchange.ResumeLayout(false);
            this.tpExchange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExchangeProducts)).EndInit();
            this.gbExchangeSelect.ResumeLayout(false);
            this.gbExchangeSelect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExchangeReturnProducts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcReturnExchange;
        private System.Windows.Forms.TabPage tpTrxLookup;
        private System.Windows.Forms.DateTimePicker dtToTrxDate;
        private System.Windows.Forms.TextBox txtToTrxDate;
        private System.Windows.Forms.DateTimePicker dtFromTrxDate;
        private System.Windows.Forms.TextBox txtFromTrxDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvTransaction;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button btnChooseSchedules;
        private System.Windows.Forms.Button btnCheckAvailability;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TabPage tpProductLookup;
        private System.Windows.Forms.GroupBox gbPLSearchCriteria;
        private System.Windows.Forms.Button btnPLSKUSearch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView dgvProductDetails;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tpReturn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dgvReturnProducts;
        private System.Windows.Forms.TabPage tpExchange;
        private System.Windows.Forms.DataGridView dgvExchangeProducts;
        private System.Windows.Forms.GroupBox gbExchangeSelect;
        private System.Windows.Forms.Button btnExchangeSKUSearch;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtExchangeProduct;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dgvExchangeReturnProducts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTrxID;
        private System.Windows.Forms.Button buttonTLNext;
        private System.Windows.Forms.Button btnTLClose;
        private System.Windows.Forms.Button btnSearchTrx;
        private System.Windows.Forms.TextBox textBoxMessageLine;
        private System.Windows.Forms.Button btnClearProductSearch;
        private System.Windows.Forms.Button btnProductSearch;
        private System.Windows.Forms.Button btnInitiateExchange;
        private System.Windows.Forms.Button btnInitiateReturn;
        private System.Windows.Forms.Button btnPLClose;
        private System.Windows.Forms.Button btnPLBack;
        private System.Windows.Forms.Button btnCancelReturn;
        private System.Windows.Forms.Button btnConfirmReturn;
        private System.Windows.Forms.Button btnClearExchangeProductSearch;
        private System.Windows.Forms.Button btnExchangeProductSearch;
        private System.Windows.Forms.Button btnCancelExchange;
        private System.Windows.Forms.Button btnConfirmExchange;
        private System.Windows.Forms.Button btnCloseReturn;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnKeyboardProductLookup;
        private System.Windows.Forms.Button btnShowNumPad;
        private System.Windows.Forms.Button btnKeyboardExchangbtnExchangeProductLookup;
        private System.Windows.Forms.Button btnCloseExchange;
        private System.Windows.Forms.Button btnExchangeProductLookup;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnTax;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TaxIncl;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastSalePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalReturnLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalPrice;
        private System.Windows.Forms.DataGridViewButtonColumn ProductDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnLineRemarks;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExchangeProductID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExchangeProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExchangeQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExchangePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExchangeAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExchangeTax;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TaxInclv;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExchangeLastSalePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExchangePdtLine;
        private System.Windows.Forms.DataGridViewButtonColumn ExchangeMoreInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExchangeProductRemarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnProductID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnPdtPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnTaxAmt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TaxInclusive;
        private System.Windows.Forms.DataGridViewTextBoxColumn discountAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn SalePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExchangeLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalExchangeLine;
        private System.Windows.Forms.DataGridViewButtonColumn MoreInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExchangeReturnLineRemarks;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn price;
        private System.Windows.Forms.DataGridViewTextBoxColumn amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tax;
        private System.Windows.Forms.DataGridViewCheckBoxColumn tax_inclusive_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn discount_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn last_sale_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Line;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewButtonColumn MoreInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Trx_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxTax;
        private System.Windows.Forms.DataGridViewTextBoxColumn Net_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxdiscount_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxRemarks;

    }
}