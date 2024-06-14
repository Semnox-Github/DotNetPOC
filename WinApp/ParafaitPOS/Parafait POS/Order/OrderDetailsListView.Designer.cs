namespace Parafait_POS
{
    partial class OrderDetailsListView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvTransactionDTOList = new System.Windows.Forms.DataGridView();
            this.dcOrderRightClick = new System.Windows.Forms.DataGridViewButtonColumn();
            this.selectOrderDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.transactionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taxAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionNetAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionDiscountPercentageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentModeDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.posMachineDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cashAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creditCardAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gameCardAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otherPaymentModeAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentReferenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.ctxTransactionContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectTransactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelTransactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergeTransactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printKOTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelCardsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewTransactionLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verticalScrollBarView1 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.horizontalScrollBarView1 = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionDTOListBS)).BeginInit();
            this.ctxTransactionContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTransactionDTOList
            // 
            this.dgvTransactionDTOList.AllowUserToAddRows = false;
            this.dgvTransactionDTOList.AllowUserToDeleteRows = false;
            this.dgvTransactionDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTransactionDTOList.AutoGenerateColumns = false;
            this.dgvTransactionDTOList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvTransactionDTOList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvTransactionDTOList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTransactionDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransactionDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcOrderRightClick,
            this.selectOrderDataGridViewCheckBoxColumn,
            this.transactionIdDataGridViewTextBoxColumn,
            this.transactionNumberDataGridViewTextBoxColumn,
            this.tableNumberDataGridViewTextBoxColumn,
            this.transactionDateDataGridViewTextBoxColumn,
            this.transactionAmountDataGridViewTextBoxColumn,
            this.taxAmountDataGridViewTextBoxColumn,
            this.transactionNetAmountDataGridViewTextBoxColumn,
            this.paidDataGridViewTextBoxColumn,
            this.transactionDiscountPercentageDataGridViewTextBoxColumn,
            this.paymentModeDataGridViewComboBoxColumn,
            this.posMachineDataGridViewTextBoxColumn,
            this.userNameDataGridViewTextBoxColumn,
            this.cashAmountDataGridViewTextBoxColumn,
            this.creditCardAmountDataGridViewTextBoxColumn,
            this.gameCardAmountDataGridViewTextBoxColumn,
            this.otherPaymentModeAmountDataGridViewTextBoxColumn,
            this.paymentReferenceDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn});
            this.dgvTransactionDTOList.DataSource = this.transactionDTOListBS;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Turquoise;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransactionDTOList.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTransactionDTOList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvTransactionDTOList.EnableHeadersVisualStyles = false;
            this.dgvTransactionDTOList.Location = new System.Drawing.Point(12, 12);
            this.dgvTransactionDTOList.Name = "dgvTransactionDTOList";
            this.dgvTransactionDTOList.ReadOnly = true;
            this.dgvTransactionDTOList.RowHeadersVisible = false;
            this.dgvTransactionDTOList.RowTemplate.Height = 40;
            this.dgvTransactionDTOList.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvTransactionDTOList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransactionDTOList.Size = new System.Drawing.Size(827, 312);
            this.dgvTransactionDTOList.TabIndex = 3;
            this.dgvTransactionDTOList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTransactionDTOList_CellClick);
            this.dgvTransactionDTOList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTransactionDTOList_CellDoubleClick);
            this.dgvTransactionDTOList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTransactionDTOList_CellMouseClick);
            this.dgvTransactionDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvTransactionDTOList_DataError);
            // 
            // dcOrderRightClick
            // 
            this.dcOrderRightClick.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcOrderRightClick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dcOrderRightClick.Frozen = true;
            this.dcOrderRightClick.HeaderText = "...";
            this.dcOrderRightClick.Name = "dcOrderRightClick";
            this.dcOrderRightClick.ReadOnly = true;
            this.dcOrderRightClick.Text = "...";
            this.dcOrderRightClick.UseColumnTextForButtonValue = true;
            this.dcOrderRightClick.Width = 40;
            // 
            // selectOrderDataGridViewCheckBoxColumn
            // 
            this.selectOrderDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.selectOrderDataGridViewCheckBoxColumn.DataPropertyName = "Selected";
            this.selectOrderDataGridViewCheckBoxColumn.Frozen = true;
            this.selectOrderDataGridViewCheckBoxColumn.HeaderText = "";
            this.selectOrderDataGridViewCheckBoxColumn.Name = "selectOrderDataGridViewCheckBoxColumn";
            this.selectOrderDataGridViewCheckBoxColumn.ReadOnly = true;
            this.selectOrderDataGridViewCheckBoxColumn.Width = 40;
            // 
            // transactionIdDataGridViewTextBoxColumn
            // 
            this.transactionIdDataGridViewTextBoxColumn.DataPropertyName = "TransactionId";
            this.transactionIdDataGridViewTextBoxColumn.HeaderText = "ID";
            this.transactionIdDataGridViewTextBoxColumn.Name = "transactionIdDataGridViewTextBoxColumn";
            this.transactionIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionIdDataGridViewTextBoxColumn.Width = 42;
            // 
            // transactionNumberDataGridViewTextBoxColumn
            // 
            this.transactionNumberDataGridViewTextBoxColumn.DataPropertyName = "TransactionNumber";
            this.transactionNumberDataGridViewTextBoxColumn.HeaderText = "Trx No";
            this.transactionNumberDataGridViewTextBoxColumn.Name = "transactionNumberDataGridViewTextBoxColumn";
            this.transactionNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionNumberDataGridViewTextBoxColumn.Width = 46;
            // 
            // tableNumberDataGridViewTextBoxColumn
            // 
            this.tableNumberDataGridViewTextBoxColumn.DataPropertyName = "TableNumber";
            this.tableNumberDataGridViewTextBoxColumn.HeaderText = "Table#";
            this.tableNumberDataGridViewTextBoxColumn.Name = "tableNumberDataGridViewTextBoxColumn";
            this.tableNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.tableNumberDataGridViewTextBoxColumn.Width = 65;
            // 
            // transactionDateDataGridViewTextBoxColumn
            // 
            this.transactionDateDataGridViewTextBoxColumn.DataPropertyName = "TransactionDate";
            this.transactionDateDataGridViewTextBoxColumn.HeaderText = "Date";
            this.transactionDateDataGridViewTextBoxColumn.Name = "transactionDateDataGridViewTextBoxColumn";
            this.transactionDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionDateDataGridViewTextBoxColumn.Width = 54;
            // 
            // transactionAmountDataGridViewTextBoxColumn
            // 
            this.transactionAmountDataGridViewTextBoxColumn.DataPropertyName = "TransactionAmount";
            this.transactionAmountDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.transactionAmountDataGridViewTextBoxColumn.Name = "transactionAmountDataGridViewTextBoxColumn";
            this.transactionAmountDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionAmountDataGridViewTextBoxColumn.Width = 67;
            // 
            // taxAmountDataGridViewTextBoxColumn
            // 
            this.taxAmountDataGridViewTextBoxColumn.DataPropertyName = "TaxAmount";
            this.taxAmountDataGridViewTextBoxColumn.HeaderText = "Tax";
            this.taxAmountDataGridViewTextBoxColumn.Name = "taxAmountDataGridViewTextBoxColumn";
            this.taxAmountDataGridViewTextBoxColumn.ReadOnly = true;
            this.taxAmountDataGridViewTextBoxColumn.Width = 49;
            // 
            // transactionNetAmountDataGridViewTextBoxColumn
            // 
            this.transactionNetAmountDataGridViewTextBoxColumn.DataPropertyName = "TransactionNetAmount";
            this.transactionNetAmountDataGridViewTextBoxColumn.HeaderText = "Net Amount";
            this.transactionNetAmountDataGridViewTextBoxColumn.Name = "transactionNetAmountDataGridViewTextBoxColumn";
            this.transactionNetAmountDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionNetAmountDataGridViewTextBoxColumn.Width = 80;
            // 
            // paidDataGridViewTextBoxColumn
            // 
            this.paidDataGridViewTextBoxColumn.DataPropertyName = "Paid";
            this.paidDataGridViewTextBoxColumn.HeaderText = "Paid";
            this.paidDataGridViewTextBoxColumn.Name = "paidDataGridViewTextBoxColumn";
            this.paidDataGridViewTextBoxColumn.ReadOnly = true;
            this.paidDataGridViewTextBoxColumn.Width = 52;
            // 
            // transactionDiscountPercentageDataGridViewTextBoxColumn
            // 
            this.transactionDiscountPercentageDataGridViewTextBoxColumn.DataPropertyName = "TransactionDiscountPercentage";
            this.transactionDiscountPercentageDataGridViewTextBoxColumn.HeaderText = "Avg Disc %";
            this.transactionDiscountPercentageDataGridViewTextBoxColumn.Name = "transactionDiscountPercentageDataGridViewTextBoxColumn";
            this.transactionDiscountPercentageDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionDiscountPercentageDataGridViewTextBoxColumn.Width = 71;
            // 
            // paymentModeDataGridViewComboBoxColumn
            // 
            this.paymentModeDataGridViewComboBoxColumn.DataPropertyName = "PaymentMode";
            this.paymentModeDataGridViewComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.paymentModeDataGridViewComboBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.paymentModeDataGridViewComboBoxColumn.HeaderText = "Pay Mode";
            this.paymentModeDataGridViewComboBoxColumn.Name = "paymentModeDataGridViewComboBoxColumn";
            this.paymentModeDataGridViewComboBoxColumn.ReadOnly = true;
            this.paymentModeDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.paymentModeDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.paymentModeDataGridViewComboBoxColumn.Width = 73;
            // 
            // posMachineDataGridViewTextBoxColumn
            // 
            this.posMachineDataGridViewTextBoxColumn.DataPropertyName = "PosMachine";
            this.posMachineDataGridViewTextBoxColumn.HeaderText = "POS";
            this.posMachineDataGridViewTextBoxColumn.Name = "posMachineDataGridViewTextBoxColumn";
            this.posMachineDataGridViewTextBoxColumn.ReadOnly = true;
            this.posMachineDataGridViewTextBoxColumn.Width = 53;
            // 
            // userNameDataGridViewTextBoxColumn
            // 
            this.userNameDataGridViewTextBoxColumn.DataPropertyName = "UserName";
            this.userNameDataGridViewTextBoxColumn.HeaderText = "Cashier";
            this.userNameDataGridViewTextBoxColumn.Name = "userNameDataGridViewTextBoxColumn";
            this.userNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.userNameDataGridViewTextBoxColumn.Width = 66;
            // 
            // cashAmountDataGridViewTextBoxColumn
            // 
            this.cashAmountDataGridViewTextBoxColumn.DataPropertyName = "CashAmount";
            this.cashAmountDataGridViewTextBoxColumn.HeaderText = "Cash";
            this.cashAmountDataGridViewTextBoxColumn.Name = "cashAmountDataGridViewTextBoxColumn";
            this.cashAmountDataGridViewTextBoxColumn.ReadOnly = true;
            this.cashAmountDataGridViewTextBoxColumn.Width = 55;
            // 
            // creditCardAmountDataGridViewTextBoxColumn
            // 
            this.creditCardAmountDataGridViewTextBoxColumn.DataPropertyName = "CreditCardAmount";
            this.creditCardAmountDataGridViewTextBoxColumn.HeaderText = "C C Amount";
            this.creditCardAmountDataGridViewTextBoxColumn.Name = "creditCardAmountDataGridViewTextBoxColumn";
            this.creditCardAmountDataGridViewTextBoxColumn.ReadOnly = true;
            this.creditCardAmountDataGridViewTextBoxColumn.Width = 80;
            // 
            // gameCardAmountDataGridViewTextBoxColumn
            // 
            this.gameCardAmountDataGridViewTextBoxColumn.DataPropertyName = "GameCardAmount";
            this.gameCardAmountDataGridViewTextBoxColumn.HeaderText = "Game Card Amt";
            this.gameCardAmountDataGridViewTextBoxColumn.Name = "gameCardAmountDataGridViewTextBoxColumn";
            this.gameCardAmountDataGridViewTextBoxColumn.ReadOnly = true;
            this.gameCardAmountDataGridViewTextBoxColumn.Width = 80;
            // 
            // otherPaymentModeAmountDataGridViewTextBoxColumn
            // 
            this.otherPaymentModeAmountDataGridViewTextBoxColumn.DataPropertyName = "OtherPaymentModeAmount";
            this.otherPaymentModeAmountDataGridViewTextBoxColumn.HeaderText = "Other Mode Amt";
            this.otherPaymentModeAmountDataGridViewTextBoxColumn.Name = "otherPaymentModeAmountDataGridViewTextBoxColumn";
            this.otherPaymentModeAmountDataGridViewTextBoxColumn.ReadOnly = true;
            this.otherPaymentModeAmountDataGridViewTextBoxColumn.Width = 83;
            // 
            // paymentReferenceDataGridViewTextBoxColumn
            // 
            this.paymentReferenceDataGridViewTextBoxColumn.DataPropertyName = "PaymentReference";
            this.paymentReferenceDataGridViewTextBoxColumn.HeaderText = "Ref";
            this.paymentReferenceDataGridViewTextBoxColumn.Name = "paymentReferenceDataGridViewTextBoxColumn";
            this.paymentReferenceDataGridViewTextBoxColumn.ReadOnly = true;
            this.paymentReferenceDataGridViewTextBoxColumn.Width = 48;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            this.statusDataGridViewTextBoxColumn.Width = 61;
            // 
            // transactionDTOListBS
            // 
            this.transactionDTOListBS.DataSource = typeof(Semnox.Parafait.Transaction.TransactionDTO);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(12, 370);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 41);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ctxTransactionContextMenu
            // 
            this.ctxTransactionContextMenu.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctxTransactionContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectTransactionToolStripMenuItem,
            this.cancelTransactionToolStripMenuItem,
            this.mergeTransactionToolStripMenuItem,
            this.printKOTToolStripMenuItem,
            this.cancelCardsToolStripMenuItem,
            this.viewTransactionLogsToolStripMenuItem});
            this.ctxTransactionContextMenu.Name = "ctxOrderContextMenu";
            this.ctxTransactionContextMenu.Size = new System.Drawing.Size(291, 208);
            this.ctxTransactionContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ctxTransactionContextMenu_ItemClicked);
            // 
            // selectTransactionToolStripMenuItem
            // 
            this.selectTransactionToolStripMenuItem.Name = "selectTransactionToolStripMenuItem";
            this.selectTransactionToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.selectTransactionToolStripMenuItem.Text = "Select";
            // 
            // cancelTransactionToolStripMenuItem
            // 
            this.cancelTransactionToolStripMenuItem.Name = "cancelTransactionToolStripMenuItem";
            this.cancelTransactionToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.cancelTransactionToolStripMenuItem.Text = "Cancel";
            // 
            // mergeTransactionToolStripMenuItem
            // 
            this.mergeTransactionToolStripMenuItem.Name = "mergeTransactionToolStripMenuItem";
            this.mergeTransactionToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.mergeTransactionToolStripMenuItem.Text = "Merge";
            // 
            // printKOTToolStripMenuItem
            // 
            this.printKOTToolStripMenuItem.Name = "printKOTToolStripMenuItem";
            this.printKOTToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.printKOTToolStripMenuItem.Text = "Print KOT";
            // 
            // cancelCardsToolStripMenuItem
            // 
            this.cancelCardsToolStripMenuItem.Name = "cancelCardsToolStripMenuItem";
            this.cancelCardsToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.cancelCardsToolStripMenuItem.Text = "Cancel Cards";
            // 
            // viewTransactionLogsToolStripMenuItem
            // 
            this.viewTransactionLogsToolStripMenuItem.Name = "viewTransactionLogsToolStripMenuItem";
            this.viewTransactionLogsToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.viewTransactionLogsToolStripMenuItem.Text = "View Transaction Logs";
            // 
            // verticalScrollBarView1
            // 
            this.verticalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView1.AutoHide = false;
            this.verticalScrollBarView1.DataGridView = this.dgvTransactionDTOList;
            this.verticalScrollBarView1.Location = new System.Drawing.Point(842, 12);
            this.verticalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView1.Name = "verticalScrollBarView1";
            this.verticalScrollBarView1.ScrollableControl = null;
            this.verticalScrollBarView1.Size = new System.Drawing.Size(40, 312);
            this.verticalScrollBarView1.TabIndex = 14;
            // 
            // horizontalScrollBarView1
            // 
            this.horizontalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBarView1.AutoHide = false;
            this.horizontalScrollBarView1.DataGridView = this.dgvTransactionDTOList;
            this.horizontalScrollBarView1.Location = new System.Drawing.Point(12, 327);
            this.horizontalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarView1.Name = "horizontalScrollBarView1";
            this.horizontalScrollBarView1.ScrollableControl = null;
            this.horizontalScrollBarView1.Size = new System.Drawing.Size(827, 40);
            this.horizontalScrollBarView1.TabIndex = 15;
            // 
            // OrderDetailsListView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(884, 415);
            this.Controls.Add(this.horizontalScrollBarView1);
            this.Controls.Add(this.verticalScrollBarView1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvTransactionDTOList);
            this.Name = "OrderDetailsListView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order Details";
            this.Load += new System.EventHandler(this.OrderTransactionListView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionDTOListBS)).EndInit();
            this.ctxTransactionContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource transactionDTOListBS;
        private System.Windows.Forms.DataGridView dgvTransactionDTOList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip ctxTransactionContextMenu;
        private System.Windows.Forms.ToolStripMenuItem selectTransactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelTransactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mergeTransactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printKOTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelCardsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewTransactionLogsToolStripMenuItem;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView1;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarView1;
        private System.Windows.Forms.DataGridViewButtonColumn dcOrderRightClick;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectOrderDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tableNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionNetAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionDiscountPercentageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn paymentModeDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn posMachineDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cashAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creditCardAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gameCardAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn otherPaymentModeAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentReferenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
    }
}