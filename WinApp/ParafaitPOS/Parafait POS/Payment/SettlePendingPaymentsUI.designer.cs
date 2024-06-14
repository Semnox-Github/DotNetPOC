namespace Parafait_POS
{
    partial class SettlePendingPaymentsUI
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
            if(disposing && (components != null))
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
            this.gpFilterbox = new System.Windows.Forms.GroupBox();
            this.btnShowNumPad = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtTrxid = new System.Windows.Forms.TextBox();
            this.lblTrxid = new System.Windows.Forms.Label();
            this.cmbUser = new System.Windows.Forms.ComboBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.textBoxMessageLine = new System.Windows.Forms.TextBox();
            this.gpConpleteTransaction = new System.Windows.Forms.GroupBox();
            this.chbPrintMerchantCopy = new System.Windows.Forms.CheckBox();
            this.chbTrxSelect = new System.Windows.Forms.CheckBox();
            this.dgvUnsettledPayments = new System.Windows.Forms.DataGridView();
            this.selectedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CreditCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintMerchantReceipt = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnComplete = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creditCardNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedUserDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pendingTransactionPaymentDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.gpFilterbox.SuspendLayout();
            this.gpConpleteTransaction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnsettledPayments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pendingTransactionPaymentDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // gpFilterbox
            // 
            this.gpFilterbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilterbox.Controls.Add(this.btnShowNumPad);
            this.gpFilterbox.Controls.Add(this.btnSearch);
            this.gpFilterbox.Controls.Add(this.txtTrxid);
            this.gpFilterbox.Controls.Add(this.lblTrxid);
            this.gpFilterbox.Controls.Add(this.cmbUser);
            this.gpFilterbox.Controls.Add(this.lblUser);
            this.gpFilterbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.gpFilterbox.Location = new System.Drawing.Point(9, 3);
            this.gpFilterbox.Name = "gpFilterbox";
            this.gpFilterbox.Size = new System.Drawing.Size(953, 55);
            this.gpFilterbox.TabIndex = 6;
            this.gpFilterbox.TabStop = false;
            this.gpFilterbox.Text = "Filter";
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
            this.btnShowNumPad.Location = new System.Drawing.Point(470, 12);
            this.btnShowNumPad.Name = "btnShowNumPad";
            this.btnShowNumPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowNumPad.TabIndex = 23;
            this.btnShowNumPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowNumPad.UseVisualStyleBackColor = false;
            this.btnShowNumPad.Click += new System.EventHandler(this.btnShowNumPad_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(528, 16);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(119, 30);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtTrxid
            // 
            this.txtTrxid.Location = new System.Drawing.Point(364, 22);
            this.txtTrxid.Name = "txtTrxid";
            this.txtTrxid.Size = new System.Drawing.Size(100, 20);
            this.txtTrxid.TabIndex = 3;
            // 
            // lblTrxid
            // 
            this.lblTrxid.Location = new System.Drawing.Point(271, 22);
            this.lblTrxid.Name = "lblTrxid";
            this.lblTrxid.Size = new System.Drawing.Size(87, 17);
            this.lblTrxid.TabIndex = 2;
            this.lblTrxid.Text = "Trxid:";
            this.lblTrxid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbUser
            // 
            this.cmbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.Location = new System.Drawing.Point(111, 21);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(139, 21);
            this.cmbUser.TabIndex = 1;
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(18, 22);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(87, 17);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "Users:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxMessageLine
            // 
            this.textBoxMessageLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessageLine.BackColor = System.Drawing.Color.PapayaWhip;
            this.textBoxMessageLine.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessageLine.ForeColor = System.Drawing.Color.Firebrick;
            this.textBoxMessageLine.Location = new System.Drawing.Point(2, 364);
            this.textBoxMessageLine.Name = "textBoxMessageLine";
            this.textBoxMessageLine.Size = new System.Drawing.Size(964, 26);
            this.textBoxMessageLine.TabIndex = 5;
            // 
            // gpConpleteTransaction
            // 
            this.gpConpleteTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpConpleteTransaction.Controls.Add(this.chbPrintMerchantCopy);
            this.gpConpleteTransaction.Controls.Add(this.chbTrxSelect);
            this.gpConpleteTransaction.Controls.Add(this.dgvUnsettledPayments);
            this.gpConpleteTransaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpConpleteTransaction.Location = new System.Drawing.Point(3, 62);
            this.gpConpleteTransaction.Name = "gpConpleteTransaction";
            this.gpConpleteTransaction.Size = new System.Drawing.Size(959, 214);
            this.gpConpleteTransaction.TabIndex = 9;
            this.gpConpleteTransaction.TabStop = false;
            this.gpConpleteTransaction.Text = "Credit Card Transactions";
            // 
            // chbPrintMerchantCopy
            // 
            this.chbPrintMerchantCopy.AutoSize = true;
            this.chbPrintMerchantCopy.BackColor = System.Drawing.Color.Transparent;
            this.chbPrintMerchantCopy.Location = new System.Drawing.Point(735, 35);
            this.chbPrintMerchantCopy.Name = "chbPrintMerchantCopy";
            this.chbPrintMerchantCopy.Size = new System.Drawing.Size(15, 14);
            this.chbPrintMerchantCopy.TabIndex = 11;
            this.chbPrintMerchantCopy.UseVisualStyleBackColor = false;
            this.chbPrintMerchantCopy.CheckedChanged += new System.EventHandler(this.chbPrintMerchantCopy_CheckedChanged);
            // 
            // chbTrxSelect
            // 
            this.chbTrxSelect.AutoSize = true;
            this.chbTrxSelect.BackColor = System.Drawing.Color.Transparent;
            this.chbTrxSelect.Location = new System.Drawing.Point(26, 35);
            this.chbTrxSelect.Name = "chbTrxSelect";
            this.chbTrxSelect.Size = new System.Drawing.Size(15, 14);
            this.chbTrxSelect.TabIndex = 10;
            this.chbTrxSelect.UseVisualStyleBackColor = false;
            this.chbTrxSelect.CheckedChanged += new System.EventHandler(this.chbTrxSelect_CheckedChanged);
            // 
            // dgvUnsettledPayments
            // 
            this.dgvUnsettledPayments.AllowUserToAddRows = false;
            this.dgvUnsettledPayments.AllowUserToDeleteRows = false;
            this.dgvUnsettledPayments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUnsettledPayments.AutoGenerateColumns = false;
            this.dgvUnsettledPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnsettledPayments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectedDataGridViewCheckBoxColumn,
            this.transactionIdDataGridViewTextBoxColumn,
            this.paymentDateDataGridViewTextBoxColumn,
            this.CreditCardNumber,
            this.amountDataGridViewTextBoxColumn,
            this.TipAmount,
            this.creditCardNameDataGridViewTextBoxColumn,
            this.PrintMerchantReceipt,
            this.lastUpdatedUserDataGridViewTextBoxColumn});
            this.dgvUnsettledPayments.DataSource = this.pendingTransactionPaymentDTOListBS;
            this.dgvUnsettledPayments.Location = new System.Drawing.Point(9, 19);
            this.dgvUnsettledPayments.Name = "dgvUnsettledPayments";
            this.dgvUnsettledPayments.ReadOnly = true;
            this.dgvUnsettledPayments.RowTemplate.Height = 35;
            this.dgvUnsettledPayments.Size = new System.Drawing.Size(942, 189);
            this.dgvUnsettledPayments.TabIndex = 0;
            this.dgvUnsettledPayments.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnsettledPayments_CellContentClick);
            this.dgvUnsettledPayments.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvUnsettledPayments_DataBindingComplete);
            // 
            // selectedDataGridViewCheckBoxColumn
            // 
            this.selectedDataGridViewCheckBoxColumn.HeaderText = "Select";
            this.selectedDataGridViewCheckBoxColumn.Name = "selectedDataGridViewCheckBoxColumn";
            this.selectedDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // CreditCardNumber
            // 
            this.CreditCardNumber.DataPropertyName = "CreditCardNumber";
            this.CreditCardNumber.HeaderText = "Credit Card Number";
            this.CreditCardNumber.Name = "CreditCardNumber";
            this.CreditCardNumber.ReadOnly = true;
            // 
            // TipAmount
            // 
            this.TipAmount.DataPropertyName = "TipAmount";
            this.TipAmount.HeaderText = "Tip Amount";
            this.TipAmount.Name = "TipAmount";
            this.TipAmount.ReadOnly = true;
            // 
            // PrintMerchantReceipt
            // 
            this.PrintMerchantReceipt.FalseValue = "false";
            this.PrintMerchantReceipt.HeaderText = "PrintMerchantReceipt";
            this.PrintMerchantReceipt.Name = "PrintMerchantReceipt";
            this.PrintMerchantReceipt.ReadOnly = true;
            this.PrintMerchantReceipt.TrueValue = "true";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(388, 298);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(155, 55);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnComplete.BackColor = System.Drawing.Color.Transparent;
            this.btnComplete.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnComplete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnComplete.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnComplete.FlatAppearance.BorderSize = 0;
            this.btnComplete.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnComplete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnComplete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnComplete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComplete.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComplete.ForeColor = System.Drawing.Color.White;
            this.btnComplete.Location = new System.Drawing.Point(133, 298);
            this.btnComplete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(155, 55);
            this.btnComplete.TabIndex = 7;
            this.btnComplete.Text = "Complete";
            this.btnComplete.UseVisualStyleBackColor = false;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "TransactionId";
            this.dataGridViewTextBoxColumn1.HeaderText = "Transaction Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "PaymentDate";
            this.dataGridViewTextBoxColumn2.HeaderText = "Payment Date";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "CreditCardNumber";
            this.dataGridViewTextBoxColumn3.HeaderText = "Credit Card Number";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Amount";
            this.dataGridViewTextBoxColumn4.HeaderText = "Amount";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "TipAmount";
            this.dataGridViewTextBoxColumn5.HeaderText = "Tip Amount";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "CreditCardName";
            this.dataGridViewTextBoxColumn6.HeaderText = "Credit Card Name";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "LastUpdatedUser";
            this.dataGridViewTextBoxColumn7.HeaderText = "User Name";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // transactionIdDataGridViewTextBoxColumn
            // 
            this.transactionIdDataGridViewTextBoxColumn.DataPropertyName = "TransactionId";
            this.transactionIdDataGridViewTextBoxColumn.HeaderText = "Transaction Id";
            this.transactionIdDataGridViewTextBoxColumn.Name = "transactionIdDataGridViewTextBoxColumn";
            this.transactionIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // paymentDateDataGridViewTextBoxColumn
            // 
            this.paymentDateDataGridViewTextBoxColumn.DataPropertyName = "PaymentDate";
            this.paymentDateDataGridViewTextBoxColumn.HeaderText = "Payment Date";
            this.paymentDateDataGridViewTextBoxColumn.Name = "paymentDateDataGridViewTextBoxColumn";
            this.paymentDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            this.amountDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            this.amountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // creditCardNameDataGridViewTextBoxColumn
            // 
            this.creditCardNameDataGridViewTextBoxColumn.DataPropertyName = "CreditCardName";
            this.creditCardNameDataGridViewTextBoxColumn.HeaderText = "Credit Card Name";
            this.creditCardNameDataGridViewTextBoxColumn.Name = "creditCardNameDataGridViewTextBoxColumn";
            this.creditCardNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedUserDataGridViewTextBoxColumn
            // 
            this.lastUpdatedUserDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedUser";
            this.lastUpdatedUserDataGridViewTextBoxColumn.HeaderText = "User Name";
            this.lastUpdatedUserDataGridViewTextBoxColumn.Name = "lastUpdatedUserDataGridViewTextBoxColumn";
            this.lastUpdatedUserDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pendingTransactionPaymentDTOListBS
            // 
            this.pendingTransactionPaymentDTOListBS.DataSource = typeof(Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO);
            // 
            // SettlePendingPaymentsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 390);
            this.Controls.Add(this.gpConpleteTransaction);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.textBoxMessageLine);
            this.Controls.Add(this.gpFilterbox);
            this.MaximizeBox = false;
            this.Name = "SettlePendingPaymentsUI";
            this.Text = "Settle Pending Payments";
            this.Load += new System.EventHandler(this.SettlePendingPaymentsUI_Load);
            this.gpFilterbox.ResumeLayout(false);
            this.gpFilterbox.PerformLayout();
            this.gpConpleteTransaction.ResumeLayout(false);
            this.gpConpleteTransaction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnsettledPayments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pendingTransactionPaymentDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gpFilterbox;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtTrxid;
        private System.Windows.Forms.Label lblTrxid;
        private System.Windows.Forms.ComboBox cmbUser;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox textBoxMessageLine;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.GroupBox gpConpleteTransaction;
        private System.Windows.Forms.DataGridView dgvUnsettledPayments;
        private System.Windows.Forms.BindingSource pendingTransactionPaymentDTOListBS;
        private System.Windows.Forms.Button btnShowNumPad;
        private System.Windows.Forms.CheckBox chbTrxSelect;
        private System.Windows.Forms.CheckBox chbPrintMerchantCopy;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreditCardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn creditCardNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn PrintMerchantReceipt;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedUserDataGridViewTextBoxColumn;
    }
}