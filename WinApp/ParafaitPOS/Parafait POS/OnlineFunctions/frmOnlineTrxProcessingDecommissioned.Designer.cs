namespace Parafait_POS
{
    partial class frmOnlineTrxProcessingDecommissioned
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtTransactionId = new System.Windows.Forms.TextBox();
            this.lblTransactionId = new System.Windows.Forms.Label();
            this.btnGetDetails = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.dgvTrxHeader = new System.Windows.Forms.DataGridView();
            this.TrxId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TrxNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentReference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvTrxLines = new System.Windows.Forms.DataGridView();
            this.Product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.issueCardColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtTransactionOTP = new System.Windows.Forms.TextBox();
            this.lblTransactionOTP = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnReopen = new System.Windows.Forms.Button();
            this.ChkBxSelectAll = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnChooseProduct = new System.Windows.Forms.Button();
            this.btnShowNumPad = new System.Windows.Forms.Button();
            this.btnFindCustomer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxLines)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTransactionId
            // 
            this.txtTransactionId.Location = new System.Drawing.Point(136, 13);
            this.txtTransactionId.Margin = new System.Windows.Forms.Padding(4);
            this.txtTransactionId.Name = "txtTransactionId";
            this.txtTransactionId.Size = new System.Drawing.Size(99, 22);
            this.txtTransactionId.TabIndex = 0;
            this.txtTransactionId.Enter += new System.EventHandler(this.txtTransactionId_Enter);
            // 
            // lblTransactionId
            // 
            this.lblTransactionId.AutoSize = true;
            this.lblTransactionId.Location = new System.Drawing.Point(13, 16);
            this.lblTransactionId.Name = "lblTransactionId";
            this.lblTransactionId.Size = new System.Drawing.Size(120, 16);
            this.lblTransactionId.TabIndex = 1;
            this.lblTransactionId.Text = "Transaction Ref Id:";
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
            this.btnGetDetails.ForeColor = System.Drawing.Color.White;
            this.btnGetDetails.Location = new System.Drawing.Point(500, 12);
            this.btnGetDetails.Name = "btnGetDetails";
            this.btnGetDetails.Size = new System.Drawing.Size(75, 24);
            this.btnGetDetails.TabIndex = 2;
            this.btnGetDetails.Text = "Get";
            this.btnGetDetails.UseVisualStyleBackColor = false;
            this.btnGetDetails.Click += new System.EventHandler(this.btnGetDetails_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.BackColor = System.Drawing.Color.Transparent;
            this.btnExecute.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnExecute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExecute.FlatAppearance.BorderSize = 0;
            this.btnExecute.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecute.ForeColor = System.Drawing.Color.White;
            this.btnExecute.Location = new System.Drawing.Point(339, 402);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(139, 32);
            this.btnExecute.TabIndex = 3;
            this.btnExecute.Text = "Issue Pass";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // dgvTrxHeader
            // 
            this.dgvTrxHeader.AllowUserToAddRows = false;
            this.dgvTrxHeader.AllowUserToDeleteRows = false;
            this.dgvTrxHeader.AllowUserToResizeColumns = false;
            this.dgvTrxHeader.AllowUserToResizeRows = false;
            this.dgvTrxHeader.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTrxHeader.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvTrxHeader.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvTrxHeader.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTrxHeader.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvTrxHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrxHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TrxId,
            this.TrxNo,
            this.Date,
            this.Customer,
            this.Amount,
            this.Status,
            this.PaymentReference});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTrxHeader.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvTrxHeader.EnableHeadersVisualStyles = false;
            this.dgvTrxHeader.Location = new System.Drawing.Point(13, 59);
            this.dgvTrxHeader.Name = "dgvTrxHeader";
            this.dgvTrxHeader.ReadOnly = true;
            this.dgvTrxHeader.RowHeadersVisible = false;
            this.dgvTrxHeader.Size = new System.Drawing.Size(802, 69);
            this.dgvTrxHeader.TabIndex = 4;
            // 
            // TrxId
            // 
            this.TrxId.HeaderText = "TrxId";
            this.TrxId.Name = "TrxId";
            this.TrxId.ReadOnly = true;
            this.TrxId.Width = 62;
            // 
            // TrxNo
            // 
            this.TrxNo.HeaderText = "TrxNo";
            this.TrxNo.Name = "TrxNo";
            this.TrxNo.ReadOnly = true;
            this.TrxNo.Width = 69;
            // 
            // Date
            // 
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 61;
            // 
            // Customer
            // 
            this.Customer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Customer.HeaderText = "Customer";
            this.Customer.Name = "Customer";
            this.Customer.ReadOnly = true;
            // 
            // Amount
            // 
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            this.Amount.Width = 77;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 69;
            // 
            // PaymentReference
            // 
            this.PaymentReference.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.PaymentReference.HeaderText = "Payment Reference";
            this.PaymentReference.Name = "PaymentReference";
            this.PaymentReference.ReadOnly = true;
            this.PaymentReference.Width = 160;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Transaction Details";
            // 
            // dgvTrxLines
            // 
            this.dgvTrxLines.AllowUserToAddRows = false;
            this.dgvTrxLines.AllowUserToDeleteRows = false;
            this.dgvTrxLines.AllowUserToResizeColumns = false;
            this.dgvTrxLines.AllowUserToResizeRows = false;
            this.dgvTrxLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTrxLines.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvTrxLines.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvTrxLines.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTrxLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvTrxLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrxLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Product,
            this.Quantity,
            this.LineAmount,
            this.CardNumber,
            this.ProductDetails,
            this.issueCardColumn});
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTrxLines.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgvTrxLines.EnableHeadersVisualStyles = false;
            this.dgvTrxLines.Location = new System.Drawing.Point(13, 155);
            this.dgvTrxLines.Name = "dgvTrxLines";
            this.dgvTrxLines.RowHeadersVisible = false;
            this.dgvTrxLines.Size = new System.Drawing.Size(980, 207);
            this.dgvTrxLines.TabIndex = 6;
            // 
            // Product
            // 
            this.Product.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Product.HeaderText = "Product";
            this.Product.MinimumWidth = 300;
            this.Product.Name = "Product";
            // 
            // Quantity
            // 
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.MinimumWidth = 85;
            this.Quantity.Name = "Quantity";
            this.Quantity.Width = 85;
            // 
            // LineAmount
            // 
            this.LineAmount.HeaderText = "Amount";
            this.LineAmount.MinimumWidth = 77;
            this.LineAmount.Name = "LineAmount";
            this.LineAmount.Width = 77;
            // 
            // CardNumber
            // 
            this.CardNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CardNumber.HeaderText = "Card Number";
            this.CardNumber.MinimumWidth = 120;
            this.CardNumber.Name = "CardNumber";
            this.CardNumber.Width = 120;
            // 
            // ProductDetails
            // 
            this.ProductDetails.HeaderText = "Details";
            this.ProductDetails.MinimumWidth = 300;
            this.ProductDetails.Name = "ProductDetails";
            this.ProductDetails.ReadOnly = true;
            this.ProductDetails.Width = 300;
            // 
            // issueCardColumn
            // 
            this.issueCardColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.issueCardColumn.HeaderText = "                           ";
            this.issueCardColumn.MinimumWidth = 50;
            this.issueCardColumn.Name = "issueCardColumn";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(534, 402);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(139, 32);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtTransactionOTP
            // 
            this.txtTransactionOTP.Location = new System.Drawing.Point(382, 13);
            this.txtTransactionOTP.Margin = new System.Windows.Forms.Padding(4);
            this.txtTransactionOTP.Name = "txtTransactionOTP";
            this.txtTransactionOTP.Size = new System.Drawing.Size(99, 22);
            this.txtTransactionOTP.TabIndex = 8;
            this.txtTransactionOTP.Enter += new System.EventHandler(this.txtTransactionOTP_Enter);
            // 
            // lblTransactionOTP
            // 
            this.lblTransactionOTP.AutoSize = true;
            this.lblTransactionOTP.Location = new System.Drawing.Point(266, 16);
            this.lblTransactionOTP.Name = "lblTransactionOTP";
            this.lblTransactionOTP.Size = new System.Drawing.Size(113, 16);
            this.lblTransactionOTP.TabIndex = 9;
            this.lblTransactionOTP.Text = "Transaction OTP:";
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.Enabled = false;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(729, 402);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(139, 32);
            this.btnPrint.TabIndex = 10;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnReopen
            // 
            this.btnReopen.BackColor = System.Drawing.Color.Transparent;
            this.btnReopen.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnReopen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReopen.FlatAppearance.BorderSize = 0;
            this.btnReopen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnReopen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnReopen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReopen.ForeColor = System.Drawing.Color.White;
            this.btnReopen.Location = new System.Drawing.Point(144, 402);
            this.btnReopen.Name = "btnReopen";
            this.btnReopen.Size = new System.Drawing.Size(139, 32);
            this.btnReopen.TabIndex = 11;
            this.btnReopen.Text = "Reopen";
            this.btnReopen.UseVisualStyleBackColor = false;
            this.btnReopen.Click += new System.EventHandler(this.btnReopen_Click);
            // 
            // ChkBxSelectAll
            // 
            this.ChkBxSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChkBxSelectAll.AutoSize = true;
            this.ChkBxSelectAll.BackColor = System.Drawing.Color.Teal;
            this.ChkBxSelectAll.ForeColor = System.Drawing.Color.White;
            this.ChkBxSelectAll.Location = new System.Drawing.Point(937, 159);
            this.ChkBxSelectAll.Name = "ChkBxSelectAll";
            this.ChkBxSelectAll.Size = new System.Drawing.Size(15, 14);
            this.ChkBxSelectAll.TabIndex = 14;
            this.ChkBxSelectAll.UseVisualStyleBackColor = false;
            this.ChkBxSelectAll.CheckedChanged += new System.EventHandler(this.ChkBxSelectAll_CheckedChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(13, 374);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(79, 16);
            this.lblStatus.TabIndex = 15;
            this.lblStatus.Text = "Pass Status";
            this.lblStatus.Visible = false;
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
            this.btnChooseProduct.ForeColor = System.Drawing.Color.White;
            this.btnChooseProduct.Location = new System.Drawing.Point(14, 130);
            this.btnChooseProduct.Name = "btnChooseProduct";
            this.btnChooseProduct.Size = new System.Drawing.Size(123, 24);
            this.btnChooseProduct.TabIndex = 16;
            this.btnChooseProduct.Text = "Select Quantity";
            this.btnChooseProduct.UseVisualStyleBackColor = false;
            this.btnChooseProduct.Click += new System.EventHandler(this.btnChooseProduct_Click);
            // 
            // btnShowNumPad
            // 
            this.btnShowNumPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.btnShowNumPad.Location = new System.Drawing.Point(779, 8);
            this.btnShowNumPad.Name = "btnShowNumPad";
            this.btnShowNumPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowNumPad.TabIndex = 23;
            this.btnShowNumPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowNumPad.UseVisualStyleBackColor = false;
            this.btnShowNumPad.Click += new System.EventHandler(this.btnShowNumPad_Click);
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
            this.btnFindCustomer.ForeColor = System.Drawing.Color.White;
            this.btnFindCustomer.Location = new System.Drawing.Point(615, 12);
            this.btnFindCustomer.Name = "btnFindCustomer";
            this.btnFindCustomer.Size = new System.Drawing.Size(125, 24);
            this.btnFindCustomer.TabIndex = 26;
            this.btnFindCustomer.Text = "Lookup Customer";
            this.btnFindCustomer.UseVisualStyleBackColor = false;
            this.btnFindCustomer.Click += new System.EventHandler(this.btnFindCustomer_Click);
            // 
            // frmOnlineTrxProcessingDecommissioned
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 447);
            this.Controls.Add(this.btnFindCustomer);
            this.Controls.Add(this.btnShowNumPad);
            this.Controls.Add(this.btnChooseProduct);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.ChkBxSelectAll);
            this.Controls.Add(this.btnReopen);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.lblTransactionOTP);
            this.Controls.Add(this.txtTransactionOTP);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvTrxLines);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvTrxHeader);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.btnGetDetails);
            this.Controls.Add(this.lblTransactionId);
            this.Controls.Add(this.txtTransactionId);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOnlineTrxProcessingDecommissioned";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Online Transaction - Card Transfers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmOnlineTrxProcessing_FormClosing);
            this.Load += new System.EventHandler(this.frmOnlineTrxProcessing_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTransactionId;
        private System.Windows.Forms.Label lblTransactionId;
        private System.Windows.Forms.Button btnGetDetails;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.DataGridView dgvTrxHeader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvTrxLines;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrxId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrxNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentReference;
        private System.Windows.Forms.TextBox txtTransactionOTP;
        private System.Windows.Forms.Label lblTransactionOTP;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnReopen;
        private System.Windows.Forms.CheckBox ChkBxSelectAll;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnChooseProduct;
        private System.Windows.Forms.Button btnShowNumPad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Product;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductDetails;
        private System.Windows.Forms.DataGridViewCheckBoxColumn issueCardColumn; 
        private System.Windows.Forms.Button btnFindCustomer;
    }
}