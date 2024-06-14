namespace Parafait_POS.OnlineFunctions
{
    partial class ExecuteOnlineTransactionView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecuteOnlineTransactionView));
            this.btnFindCustomer = new System.Windows.Forms.Button();
            this.btnChooseProduct = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.ChkBxSelectAll = new System.Windows.Forms.CheckBox();
            this.btnReopen = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblTransactionOTP = new System.Windows.Forms.Label();
            this.txtTransactionOTP = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvTrxLines = new System.Windows.Forms.DataGridView();
            this.productNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productDetailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selectedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.transactionLineDTOBS = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.btnShowNumPad = new System.Windows.Forms.Button();
            this.dgvTrxHeader = new System.Windows.Forms.DataGridView();
            this.transactionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionNetAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentReferenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnMapWaiver = new System.Windows.Forms.Button();
            this.btnGetDetails = new System.Windows.Forms.Button();
            this.lblTransactionId = new System.Windows.Forms.Label();
            this.txtTransactionId = new System.Windows.Forms.TextBox();
            this.flpCommandButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.verticalScrollBarView1 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionLineDTOBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionDTOListBS)).BeginInit();
            this.flpCommandButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFindCustomer
            // 
            this.btnFindCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnFindCustomer.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnFindCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindCustomer.FlatAppearance.BorderSize = 0;
            this.btnFindCustomer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFindCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFindCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindCustomer.Font = new System.Drawing.Font("Arial", 12F);
            this.btnFindCustomer.ForeColor = System.Drawing.Color.White;
            this.btnFindCustomer.Location = new System.Drawing.Point(715, 10);
            this.btnFindCustomer.Name = "btnFindCustomer";
            this.btnFindCustomer.Size = new System.Drawing.Size(143, 40);
            this.btnFindCustomer.TabIndex = 43;
            this.btnFindCustomer.Text = "Lookup Customer";
            this.btnFindCustomer.UseVisualStyleBackColor = false;
            this.btnFindCustomer.Click += new System.EventHandler(this.btnFindCustomer_Click);
            // 
            // btnChooseProduct
            // 
            this.btnChooseProduct.BackColor = System.Drawing.Color.Transparent;
            this.btnChooseProduct.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnChooseProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnChooseProduct.FlatAppearance.BorderSize = 0;
            this.btnChooseProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChooseProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChooseProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseProduct.Font = new System.Drawing.Font("Arial", 12F);
            this.btnChooseProduct.ForeColor = System.Drawing.Color.White;
            this.btnChooseProduct.Location = new System.Drawing.Point(14, 163);
            this.btnChooseProduct.Name = "btnChooseProduct";
            this.btnChooseProduct.Size = new System.Drawing.Size(123, 40);
            this.btnChooseProduct.TabIndex = 41;
            this.btnChooseProduct.Text = "Select Quantity";
            this.btnChooseProduct.UseVisualStyleBackColor = false;
            this.btnChooseProduct.Click += new System.EventHandler(this.btnChooseProduct_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(10, 440);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(111, 22);
            this.lblStatus.TabIndex = 40;
            this.lblStatus.Text = "Pass Status";
            // 
            // ChkBxSelectAll
            // 
            this.ChkBxSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChkBxSelectAll.AutoSize = true;
            this.ChkBxSelectAll.BackColor = System.Drawing.Color.Teal;
            this.ChkBxSelectAll.FlatAppearance.BorderSize = 0;
            this.ChkBxSelectAll.ForeColor = System.Drawing.Color.White;
            this.ChkBxSelectAll.Location = new System.Drawing.Point(937, 225);
            this.ChkBxSelectAll.Name = "ChkBxSelectAll";
            this.ChkBxSelectAll.Size = new System.Drawing.Size(15, 14);
            this.ChkBxSelectAll.TabIndex = 39;
            this.ChkBxSelectAll.UseVisualStyleBackColor = false;
            this.ChkBxSelectAll.CheckedChanged += new System.EventHandler(this.ChkBxSelectAll_CheckedChanged);
            // 
            // btnReopen
            // 
            this.btnReopen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReopen.BackColor = System.Drawing.Color.Transparent;
            this.btnReopen.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnReopen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReopen.FlatAppearance.BorderSize = 0;
            this.btnReopen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnReopen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnReopen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReopen.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReopen.ForeColor = System.Drawing.Color.White;
            this.btnReopen.Location = new System.Drawing.Point(3, 3);
            this.btnReopen.Name = "btnReopen";
            this.btnReopen.Size = new System.Drawing.Size(139, 40);
            this.btnReopen.TabIndex = 38;
            this.btnReopen.Text = "Reopen";
            this.btnReopen.UseVisualStyleBackColor = false;
            this.btnReopen.Click += new System.EventHandler(this.btnReopen_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.Enabled = false;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Arial", 12F);
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(519, 3);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(139, 40);
            this.btnPrint.TabIndex = 37;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblTransactionOTP
            // 
            this.lblTransactionOTP.AutoSize = true;
            this.lblTransactionOTP.Font = new System.Drawing.Font("Arial", 9F);
            this.lblTransactionOTP.Location = new System.Drawing.Point(292, 23);
            this.lblTransactionOTP.Name = "lblTransactionOTP";
            this.lblTransactionOTP.Size = new System.Drawing.Size(102, 15);
            this.lblTransactionOTP.TabIndex = 36;
            this.lblTransactionOTP.Text = "Transaction OTP:";
            // 
            // txtTransactionOTP
            // 
            this.txtTransactionOTP.Font = new System.Drawing.Font("Arial", 15F);
            this.txtTransactionOTP.Location = new System.Drawing.Point(401, 15);
            this.txtTransactionOTP.Margin = new System.Windows.Forms.Padding(4);
            this.txtTransactionOTP.Name = "txtTransactionOTP";
            this.txtTransactionOTP.Size = new System.Drawing.Size(153, 30);
            this.txtTransactionOTP.TabIndex = 35;
            this.txtTransactionOTP.Enter += new System.EventHandler(this.txtTransactionOTP_Enter);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 12F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(347, 3);
            this.btnClose.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(139, 40);
            this.btnClose.TabIndex = 34;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // dgvTrxLines
            // 
            this.dgvTrxLines.AllowUserToAddRows = false;
            this.dgvTrxLines.AllowUserToDeleteRows = false;
            this.dgvTrxLines.AllowUserToResizeColumns = false;
            this.dgvTrxLines.AllowUserToResizeRows = false;
            this.dgvTrxLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTrxLines.AutoGenerateColumns = false;
            this.dgvTrxLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTrxLines.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvTrxLines.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvTrxLines.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTrxLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTrxLines.ColumnHeadersHeight = 40;
            this.dgvTrxLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productNameDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.amountDataGridViewTextBoxColumn,
            this.cardNumberDataGridViewTextBoxColumn,
            this.productDetailDataGridViewTextBoxColumn,
            this.selectedDataGridViewCheckBoxColumn});
            this.dgvTrxLines.DataSource = this.transactionLineDTOBS;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTrxLines.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTrxLines.EnableHeadersVisualStyles = false;
            this.dgvTrxLines.Location = new System.Drawing.Point(15, 210);
            this.dgvTrxLines.Name = "dgvTrxLines";
            this.dgvTrxLines.RowHeadersVisible = false;
            this.dgvTrxLines.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvTrxLines.Size = new System.Drawing.Size(980, 227);
            this.dgvTrxLines.TabIndex = 33;
            this.dgvTrxLines.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrxLines_CellClick);
            // 
            // productNameDataGridViewTextBoxColumn
            // 
            this.productNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.productNameDataGridViewTextBoxColumn.DataPropertyName = "ProductName";
            this.productNameDataGridViewTextBoxColumn.HeaderText = "Product";
            this.productNameDataGridViewTextBoxColumn.Name = "productNameDataGridViewTextBoxColumn";
            this.productNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            this.quantityDataGridViewTextBoxColumn.Width = 80;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            this.amountDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            this.amountDataGridViewTextBoxColumn.ReadOnly = true;
            this.amountDataGridViewTextBoxColumn.Width = 77;
            // 
            // cardNumberDataGridViewTextBoxColumn
            // 
            this.cardNumberDataGridViewTextBoxColumn.DataPropertyName = "CardNumber";
            this.cardNumberDataGridViewTextBoxColumn.HeaderText = "Card Number";
            this.cardNumberDataGridViewTextBoxColumn.Name = "cardNumberDataGridViewTextBoxColumn";
            this.cardNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.cardNumberDataGridViewTextBoxColumn.Width = 103;
            // 
            // productDetailDataGridViewTextBoxColumn
            // 
            this.productDetailDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.productDetailDataGridViewTextBoxColumn.DataPropertyName = "ProductDetail";
            this.productDetailDataGridViewTextBoxColumn.HeaderText = "Details";
            this.productDetailDataGridViewTextBoxColumn.Name = "productDetailDataGridViewTextBoxColumn";
            this.productDetailDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // selectedDataGridViewCheckBoxColumn
            // 
            this.selectedDataGridViewCheckBoxColumn.DataPropertyName = "Selected";
            this.selectedDataGridViewCheckBoxColumn.HeaderText = "";
            this.selectedDataGridViewCheckBoxColumn.MinimumWidth = 100;
            this.selectedDataGridViewCheckBoxColumn.Name = "selectedDataGridViewCheckBoxColumn";
            this.selectedDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // transactionLineDTOBS
            // 
            this.transactionLineDTOBS.DataSource = typeof(Semnox.Parafait.Transaction.TransactionLineDTO);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F);
            this.label2.Location = new System.Drawing.Point(15, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 15);
            this.label2.TabIndex = 32;
            this.label2.Text = "Transaction Details";
            // 
            // btnShowNumPad
            // 
            this.btnShowNumPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.BackgroundImage = global::Parafait_POS.Properties.Resources.Keypad;
            this.btnShowNumPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnShowNumPad.CausesValidation = false;
            this.btnShowNumPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowNumPad.FlatAppearance.BorderSize = 0;
            this.btnShowNumPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowNumPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnShowNumPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowNumPad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnShowNumPad.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnShowNumPad.Location = new System.Drawing.Point(898, 10);
            this.btnShowNumPad.Name = "btnShowNumPad";
            this.btnShowNumPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowNumPad.TabIndex = 42;
            this.btnShowNumPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowNumPad.UseVisualStyleBackColor = false;
            this.btnShowNumPad.Click += new System.EventHandler(this.btnShowNumPad_Click);
            // 
            // dgvTrxHeader
            // 
            this.dgvTrxHeader.AllowUserToAddRows = false;
            this.dgvTrxHeader.AllowUserToDeleteRows = false;
            this.dgvTrxHeader.AllowUserToResizeColumns = false;
            this.dgvTrxHeader.AllowUserToResizeRows = false;
            this.dgvTrxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTrxHeader.AutoGenerateColumns = false;
            this.dgvTrxHeader.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTrxHeader.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvTrxHeader.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvTrxHeader.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTrxHeader.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTrxHeader.ColumnHeadersHeight = 40;
            this.dgvTrxHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.transactionIdDataGridViewTextBoxColumn,
            this.transactionNumberDataGridViewTextBoxColumn,
            this.transactionDateDataGridViewTextBoxColumn,
            this.transactionNetAmountDataGridViewTextBoxColumn,
            this.CustomerName,
            this.statusDataGridViewTextBoxColumn,
            this.paymentReferenceDataGridViewTextBoxColumn});
            this.dgvTrxHeader.DataSource = this.transactionDTOListBS;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTrxHeader.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTrxHeader.EnableHeadersVisualStyles = false;
            this.dgvTrxHeader.Location = new System.Drawing.Point(15, 79);
            this.dgvTrxHeader.Name = "dgvTrxHeader";
            this.dgvTrxHeader.ReadOnly = true;
            this.dgvTrxHeader.RowHeadersVisible = false;
            this.dgvTrxHeader.Size = new System.Drawing.Size(1011, 76);
            this.dgvTrxHeader.TabIndex = 31;
            // 
            // transactionIdDataGridViewTextBoxColumn
            // 
            this.transactionIdDataGridViewTextBoxColumn.DataPropertyName = "TransactionId";
            this.transactionIdDataGridViewTextBoxColumn.HeaderText = "TrxId";
            this.transactionIdDataGridViewTextBoxColumn.Name = "transactionIdDataGridViewTextBoxColumn";
            this.transactionIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionIdDataGridViewTextBoxColumn.Width = 62;
            // 
            // transactionNumberDataGridViewTextBoxColumn
            // 
            this.transactionNumberDataGridViewTextBoxColumn.DataPropertyName = "TransactionNumber";
            this.transactionNumberDataGridViewTextBoxColumn.HeaderText = "TrxNo";
            this.transactionNumberDataGridViewTextBoxColumn.Name = "transactionNumberDataGridViewTextBoxColumn";
            this.transactionNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionNumberDataGridViewTextBoxColumn.Width = 69;
            // 
            // transactionDateDataGridViewTextBoxColumn
            // 
            this.transactionDateDataGridViewTextBoxColumn.DataPropertyName = "TransactionDate";
            this.transactionDateDataGridViewTextBoxColumn.HeaderText = "Date";
            this.transactionDateDataGridViewTextBoxColumn.Name = "transactionDateDataGridViewTextBoxColumn";
            this.transactionDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionDateDataGridViewTextBoxColumn.Width = 61;
            // 
            // transactionNetAmountDataGridViewTextBoxColumn
            // 
            this.transactionNetAmountDataGridViewTextBoxColumn.DataPropertyName = "TransactionNetAmount";
            this.transactionNetAmountDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.transactionNetAmountDataGridViewTextBoxColumn.Name = "transactionNetAmountDataGridViewTextBoxColumn";
            this.transactionNetAmountDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionNetAmountDataGridViewTextBoxColumn.Width = 77;
            // 
            // CustomerName
            // 
            this.CustomerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CustomerName.DataPropertyName = "CustomerName";
            this.CustomerName.HeaderText = "Customer";
            this.CustomerName.Name = "CustomerName";
            this.CustomerName.ReadOnly = true;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            this.statusDataGridViewTextBoxColumn.Width = 69;
            // 
            // paymentReferenceDataGridViewTextBoxColumn
            // 
            this.paymentReferenceDataGridViewTextBoxColumn.DataPropertyName = "PaymentReference";
            this.paymentReferenceDataGridViewTextBoxColumn.HeaderText = "PaymentReference";
            this.paymentReferenceDataGridViewTextBoxColumn.Name = "paymentReferenceDataGridViewTextBoxColumn";
            this.paymentReferenceDataGridViewTextBoxColumn.ReadOnly = true;
            this.paymentReferenceDataGridViewTextBoxColumn.Width = 148;
            // 
            // transactionDTOListBS
            // 
            this.transactionDTOListBS.DataSource = typeof(Semnox.Parafait.Transaction.TransactionDTO);
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExecute.BackColor = System.Drawing.Color.Transparent;
            this.btnExecute.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnExecute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExecute.FlatAppearance.BorderSize = 0;
            this.btnExecute.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecute.Font = new System.Drawing.Font("Arial", 12F);
            this.btnExecute.ForeColor = System.Drawing.Color.White;
            this.btnExecute.Location = new System.Drawing.Point(175, 3);
            this.btnExecute.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(139, 40);
            this.btnExecute.TabIndex = 30;
            this.btnExecute.Text = "Issue Pass";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnMapWaiver
            // 
            this.btnMapWaiver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMapWaiver.BackColor = System.Drawing.Color.Transparent;
            this.btnMapWaiver.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnMapWaiver.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMapWaiver.Enabled = false;
            this.btnMapWaiver.FlatAppearance.BorderSize = 0;
            this.btnMapWaiver.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMapWaiver.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMapWaiver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMapWaiver.Font = new System.Drawing.Font("Arial", 12F);
            this.btnMapWaiver.ForeColor = System.Drawing.Color.White;
            this.btnMapWaiver.Location = new System.Drawing.Point(691, 3);
            this.btnMapWaiver.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.btnMapWaiver.Name = "btnMapWaiver";
            this.btnMapWaiver.Size = new System.Drawing.Size(139, 40);
            this.btnMapWaiver.TabIndex = 39;
            this.btnMapWaiver.Text = "Map Waiver";
            this.btnMapWaiver.UseVisualStyleBackColor = false;
            this.btnMapWaiver.Click += new System.EventHandler(this.btnMapWaiver_Click);
            // 
            // btnGetDetails
            // 
            this.btnGetDetails.BackColor = System.Drawing.Color.Transparent;
            this.btnGetDetails.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnGetDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGetDetails.FlatAppearance.BorderSize = 0;
            this.btnGetDetails.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGetDetails.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGetDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetDetails.Font = new System.Drawing.Font("Arial", 12F);
            this.btnGetDetails.ForeColor = System.Drawing.Color.White;
            this.btnGetDetails.Location = new System.Drawing.Point(600, 10);
            this.btnGetDetails.Name = "btnGetDetails";
            this.btnGetDetails.Size = new System.Drawing.Size(75, 40);
            this.btnGetDetails.TabIndex = 29;
            this.btnGetDetails.Text = "Get";
            this.btnGetDetails.UseVisualStyleBackColor = false;
            this.btnGetDetails.Click += new System.EventHandler(this.btnGetDetails_Click);
            // 
            // lblTransactionId
            // 
            this.lblTransactionId.AutoSize = true;
            this.lblTransactionId.Font = new System.Drawing.Font("Arial", 9F);
            this.lblTransactionId.Location = new System.Drawing.Point(15, 23);
            this.lblTransactionId.Name = "lblTransactionId";
            this.lblTransactionId.Size = new System.Drawing.Size(110, 15);
            this.lblTransactionId.TabIndex = 28;
            this.lblTransactionId.Text = "Transaction Ref Id:";
            // 
            // txtTransactionId
            // 
            this.txtTransactionId.Font = new System.Drawing.Font("Arial", 15F);
            this.txtTransactionId.Location = new System.Drawing.Point(132, 15);
            this.txtTransactionId.Margin = new System.Windows.Forms.Padding(4);
            this.txtTransactionId.Name = "txtTransactionId";
            this.txtTransactionId.Size = new System.Drawing.Size(153, 30);
            this.txtTransactionId.TabIndex = 27;
            this.txtTransactionId.Enter += new System.EventHandler(this.txtTransactionId_Enter);
            // 
            // flpCommandButtons
            // 
            this.flpCommandButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.flpCommandButtons.AutoSize = true;
            this.flpCommandButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpCommandButtons.Controls.Add(this.btnReopen);
            this.flpCommandButtons.Controls.Add(this.btnExecute);
            this.flpCommandButtons.Controls.Add(this.btnClose);
            this.flpCommandButtons.Controls.Add(this.btnPrint);
            this.flpCommandButtons.Controls.Add(this.btnMapWaiver);
            this.flpCommandButtons.Location = new System.Drawing.Point(99, 478);
            this.flpCommandButtons.Name = "flpCommandButtons";
            this.flpCommandButtons.Size = new System.Drawing.Size(833, 46);
            this.flpCommandButtons.TabIndex = 45;
            // 
            // verticalScrollBarView1
            // 
            this.verticalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView1.AutoHide = false;
            this.verticalScrollBarView1.DataGridView = this.dgvTrxLines;
            this.verticalScrollBarView1.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.DownButtonBackgroundImage")));
            this.verticalScrollBarView1.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarView1.Location = new System.Drawing.Point(998, 210);
            this.verticalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView1.Name = "verticalScrollBarView1";
            this.verticalScrollBarView1.ScrollableControl = null;
            this.verticalScrollBarView1.ScrollViewer = null;
            this.verticalScrollBarView1.Size = new System.Drawing.Size(40, 227);
            this.verticalScrollBarView1.TabIndex = 44;
            this.verticalScrollBarView1.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.UpButtonBackgroundImage")));
            this.verticalScrollBarView1.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.UpButtonDisabledBackgroundImage")));
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "TransactionId";
            this.dataGridViewTextBoxColumn1.HeaderText = "TrxId";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 62;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TransactionNumber";
            this.dataGridViewTextBoxColumn2.HeaderText = "TrxNo";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 69;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "TransactionDate";
            this.dataGridViewTextBoxColumn3.HeaderText = "Date";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 61;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "CustomerName";
            this.dataGridViewTextBoxColumn4.HeaderText = "Customer";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "TransactionNetAmount";
            this.dataGridViewTextBoxColumn5.HeaderText = "Amount";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 77;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Status";
            this.dataGridViewTextBoxColumn6.HeaderText = "Status";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 69;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn7.DataPropertyName = "PaymentReference";
            this.dataGridViewTextBoxColumn7.HeaderText = "Payment Reference";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 160;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.DataPropertyName = "ProductName";
            this.dataGridViewTextBoxColumn8.HeaderText = "Product";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 300;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Quantity";
            this.dataGridViewTextBoxColumn9.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 85;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 85;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "Amount";
            this.dataGridViewTextBoxColumn10.HeaderText = "Amount";
            this.dataGridViewTextBoxColumn10.MinimumWidth = 77;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Width = 77;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn11.DataPropertyName = "CardNumber";
            this.dataGridViewTextBoxColumn11.HeaderText = "Card Number";
            this.dataGridViewTextBoxColumn11.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Width = 120;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn12.DataPropertyName = "ProductDetail";
            this.dataGridViewTextBoxColumn12.HeaderText = "Details";
            this.dataGridViewTextBoxColumn12.MinimumWidth = 300;
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // ExecuteOnlineTransactionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 536);
            this.Controls.Add(this.flpCommandButtons);
            this.Controls.Add(this.verticalScrollBarView1);
            this.Controls.Add(this.btnFindCustomer);
            this.Controls.Add(this.btnChooseProduct);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.ChkBxSelectAll);
            this.Controls.Add(this.lblTransactionOTP);
            this.Controls.Add(this.txtTransactionOTP);
            this.Controls.Add(this.dgvTrxLines);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnShowNumPad);
            this.Controls.Add(this.dgvTrxHeader);
            this.Controls.Add(this.btnGetDetails);
            this.Controls.Add(this.lblTransactionId);
            this.Controls.Add(this.txtTransactionId);
            this.Name = "ExecuteOnlineTransactionView";
            this.Text = "Online Transaction - Card Transfers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExecuteOnlineTransactionView_FormClosing);
            this.Load += new System.EventHandler(this.frmExecuteOnlineTransaction_Load);
            this.Resize += new System.EventHandler(this.frmExecuteOnlineTransaction_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionLineDTOBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionDTOListBS)).EndInit();
            this.flpCommandButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFindCustomer;
        private System.Windows.Forms.Button btnChooseProduct;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox ChkBxSelectAll;
        private System.Windows.Forms.Button btnReopen;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblTransactionOTP;
        private System.Windows.Forms.TextBox txtTransactionOTP;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvTrxLines;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnShowNumPad;
        private System.Windows.Forms.DataGridView dgvTrxHeader;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnGetDetails;
        private System.Windows.Forms.Label lblTransactionId;
        private System.Windows.Forms.TextBox txtTransactionId;
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
        private System.Windows.Forms.BindingSource transactionDTOListBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource transactionLineDTOBS;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView1;
        private System.Windows.Forms.FlowLayoutPanel flpCommandButtons;
        private System.Windows.Forms.Button btnMapWaiver;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productDetailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionNetAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentReferenceDataGridViewTextBoxColumn;
    }
}