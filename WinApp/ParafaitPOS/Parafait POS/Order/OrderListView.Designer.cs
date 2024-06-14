namespace Parafait_POS
{
    partial class OrderListView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderListView));
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.dcOrderRightClick = new System.Windows.Forms.DataGridViewButtonColumn();
            this.selectOrderDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.facilityTableNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waiterNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pOSMachineNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionIDsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReservationCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderHeaderDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.txtTableNumber = new System.Windows.Forms.TextBox();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ctxOrderContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergeOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printKOTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelCardsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTableDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewTransactionLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeStaffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapTransactionWaiverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtTransactionId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTransactionNumber = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.verticalScrollBarView1 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.btnKeyboard = new System.Windows.Forms.Button();
            this.txtReservationCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderHeaderDTOListBS)).BeginInit();
            this.ctxOrderContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvOrders
            // 
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.AllowUserToDeleteRows = false;
            this.dgvOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrders.AutoGenerateColumns = false;
            this.dgvOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvOrders.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvOrders.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcOrderRightClick,
            this.selectOrderDataGridViewCheckBoxColumn,
            this.facilityTableNumberDataGridViewTextBoxColumn,
            this.waiterNameDataGridViewTextBoxColumn,
            this.customerNameDataGridViewTextBoxColumn,
            this.cardNumberDataGridViewTextBoxColumn,
            this.pOSMachineNameDataGridViewTextBoxColumn,
            this.orderDateDataGridViewTextBoxColumn,
            this.amountDataGridViewTextBoxColumn,
            this.transactionIDsDataGridViewTextBoxColumn,
            this.transactionNoDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.ReservationCode});
            this.dgvOrders.DataSource = this.orderHeaderDTOListBS;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Turquoise;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOrders.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvOrders.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvOrders.EnableHeadersVisualStyles = false;
            this.dgvOrders.Location = new System.Drawing.Point(0, 93);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOrders.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvOrders.RowHeadersVisible = false;
            this.dgvOrders.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvOrders.RowTemplate.Height = 40;
            this.dgvOrders.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrders.Size = new System.Drawing.Size(953, 407);
            this.dgvOrders.TabIndex = 2;
            this.dgvOrders.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrders_CellClick);
            this.dgvOrders.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrders_CellDoubleClick);
            this.dgvOrders.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvOrders_CellMouseClick);
            this.dgvOrders.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvOrders_DataError);
            this.dgvOrders.Sorted += new System.EventHandler(this.dgvOrders_Sorted);
            // 
            // dcOrderRightClick
            // 
            this.dcOrderRightClick.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcOrderRightClick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.selectOrderDataGridViewCheckBoxColumn.HeaderText = "";
            this.selectOrderDataGridViewCheckBoxColumn.Name = "selectOrderDataGridViewCheckBoxColumn";
            this.selectOrderDataGridViewCheckBoxColumn.ReadOnly = true;
            this.selectOrderDataGridViewCheckBoxColumn.Width = 40;
            // 
            // facilityTableNumberDataGridViewTextBoxColumn
            // 
            this.facilityTableNumberDataGridViewTextBoxColumn.DataPropertyName = "FacilityTableNumber";
            this.facilityTableNumberDataGridViewTextBoxColumn.HeaderText = "Table#";
            this.facilityTableNumberDataGridViewTextBoxColumn.Name = "facilityTableNumberDataGridViewTextBoxColumn";
            this.facilityTableNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.facilityTableNumberDataGridViewTextBoxColumn.Width = 65;
            // 
            // waiterNameDataGridViewTextBoxColumn
            // 
            this.waiterNameDataGridViewTextBoxColumn.DataPropertyName = "WaiterName";
            this.waiterNameDataGridViewTextBoxColumn.HeaderText = "Waiter";
            this.waiterNameDataGridViewTextBoxColumn.Name = "waiterNameDataGridViewTextBoxColumn";
            this.waiterNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.waiterNameDataGridViewTextBoxColumn.Width = 62;
            // 
            // customerNameDataGridViewTextBoxColumn
            // 
            this.customerNameDataGridViewTextBoxColumn.DataPropertyName = "CustomerName";
            this.customerNameDataGridViewTextBoxColumn.HeaderText = "Customer";
            this.customerNameDataGridViewTextBoxColumn.Name = "customerNameDataGridViewTextBoxColumn";
            this.customerNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerNameDataGridViewTextBoxColumn.Width = 75;
            // 
            // cardNumberDataGridViewTextBoxColumn
            // 
            this.cardNumberDataGridViewTextBoxColumn.DataPropertyName = "CardNumber";
            this.cardNumberDataGridViewTextBoxColumn.HeaderText = "Card Number";
            this.cardNumberDataGridViewTextBoxColumn.Name = "cardNumberDataGridViewTextBoxColumn";
            this.cardNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.cardNumberDataGridViewTextBoxColumn.Width = 93;
            // 
            // pOSMachineNameDataGridViewTextBoxColumn
            // 
            this.pOSMachineNameDataGridViewTextBoxColumn.DataPropertyName = "POSMachineName";
            this.pOSMachineNameDataGridViewTextBoxColumn.HeaderText = "POS";
            this.pOSMachineNameDataGridViewTextBoxColumn.Name = "pOSMachineNameDataGridViewTextBoxColumn";
            this.pOSMachineNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.pOSMachineNameDataGridViewTextBoxColumn.Width = 53;
            // 
            // orderDateDataGridViewTextBoxColumn
            // 
            this.orderDateDataGridViewTextBoxColumn.DataPropertyName = "OrderDate";
            this.orderDateDataGridViewTextBoxColumn.HeaderText = "Date";
            this.orderDateDataGridViewTextBoxColumn.Name = "orderDateDataGridViewTextBoxColumn";
            this.orderDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.orderDateDataGridViewTextBoxColumn.Width = 54;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            this.amountDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            this.amountDataGridViewTextBoxColumn.ReadOnly = true;
            this.amountDataGridViewTextBoxColumn.Width = 67;
            // 
            // transactionIDsDataGridViewTextBoxColumn
            // 
            this.transactionIDsDataGridViewTextBoxColumn.DataPropertyName = "TransactionIDs";
            this.transactionIDsDataGridViewTextBoxColumn.HeaderText = "TrxId(s)";
            this.transactionIDsDataGridViewTextBoxColumn.Name = "transactionIDsDataGridViewTextBoxColumn";
            this.transactionIDsDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionIDsDataGridViewTextBoxColumn.Width = 66;

            // 
            // transactionNoDataGridViewTextBoxColumn
            // 
            this.transactionNoDataGridViewTextBoxColumn.DataPropertyName = "TrxNo";
            this.transactionNoDataGridViewTextBoxColumn.HeaderText = "TrxNo";
            this.transactionNoDataGridViewTextBoxColumn.Name = "transactionNoDataGridViewTextBoxColumn";
            this.transactionNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionNoDataGridViewTextBoxColumn.Width = 66;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            this.remarksDataGridViewTextBoxColumn.ReadOnly = true;
            this.remarksDataGridViewTextBoxColumn.Width = 73;
            // 
            // ReservationCode
            // 
            this.ReservationCode.DataPropertyName = "ReservationCode";
            this.ReservationCode.HeaderText = "ReservationCode";
            this.ReservationCode.Name = "ReservationCode";
            this.ReservationCode.ReadOnly = true;
            this.ReservationCode.Width = 113;
            // 
            // orderHeaderDTOListBS
            // 
            this.orderHeaderDTOListBS.DataSource = typeof(Semnox.Parafait.Transaction.OrderHeaderDTO);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(3, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "Table #:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTableNumber
            // 
            this.txtTableNumber.Font = new System.Drawing.Font("Arial", 15F);
            this.txtTableNumber.Location = new System.Drawing.Point(78, 8);
            this.txtTableNumber.Name = "txtTableNumber";
            this.txtTableNumber.Size = new System.Drawing.Size(119, 30);
            this.txtTableNumber.TabIndex = 6;
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Font = new System.Drawing.Font("Arial", 15F);
            this.txtCardNumber.Location = new System.Drawing.Point(274, 8);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(119, 30);
            this.txtCardNumber.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(203, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "Card #:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCustomer
            // 
            this.txtCustomer.Font = new System.Drawing.Font("Arial", 15F);
            this.txtCustomer.Location = new System.Drawing.Point(507, 9);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(119, 30);
            this.txtCustomer.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(411, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 19);
            this.label3.TabIndex = 9;
            this.label3.Text = "Customer:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ctxOrderContextMenu
            // 
            this.ctxOrderContextMenu.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctxOrderContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectOrderToolStripMenuItem,
            this.cancelOrderToolStripMenuItem,
            this.mergeOrderToolStripMenuItem,
            this.splitToolStripMenuItem,
            this.printKOTToolStripMenuItem,
            this.cancelCardsToolStripMenuItem,
            this.moveToTableToolStripMenuItem,
            this.editTableDetailsToolStripMenuItem,
            this.viewTransactionLogsToolStripMenuItem,
            this.changeStaffToolStripMenuItem,
            this.mapTransactionWaiverToolStripMenuItem});
            this.ctxOrderContextMenu.Name = "ctxOrderContextMenu";
            this.ctxOrderContextMenu.Size = new System.Drawing.Size(291, 378);
            this.ctxOrderContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ctxOrderContextMenu_ItemClicked);
            // 
            // selectOrderToolStripMenuItem
            // 
            this.selectOrderToolStripMenuItem.Name = "selectOrderToolStripMenuItem";
            this.selectOrderToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.selectOrderToolStripMenuItem.Text = "Select";
            // 
            // cancelOrderToolStripMenuItem
            // 
            this.cancelOrderToolStripMenuItem.Name = "cancelOrderToolStripMenuItem";
            this.cancelOrderToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.cancelOrderToolStripMenuItem.Text = "Cancel";
            // 
            // mergeOrderToolStripMenuItem
            // 
            this.mergeOrderToolStripMenuItem.Name = "mergeOrderToolStripMenuItem";
            this.mergeOrderToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.mergeOrderToolStripMenuItem.Text = "Merge";
            // 
            // splitToolStripMenuItem
            // 
            this.splitToolStripMenuItem.Name = "splitToolStripMenuItem";
            this.splitToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.splitToolStripMenuItem.Text = "Split";
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
            // moveToTableToolStripMenuItem
            // 
            this.moveToTableToolStripMenuItem.Name = "moveToTableToolStripMenuItem";
            this.moveToTableToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.moveToTableToolStripMenuItem.Text = "Move to Table";
            // 
            // editTableDetailsToolStripMenuItem
            // 
            this.editTableDetailsToolStripMenuItem.Name = "editTableDetailsToolStripMenuItem";
            this.editTableDetailsToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.editTableDetailsToolStripMenuItem.Text = "Edit Table Details";
            // 
            // viewTransactionLogsToolStripMenuItem
            // 
            this.viewTransactionLogsToolStripMenuItem.Name = "viewTransactionLogsToolStripMenuItem";
            this.viewTransactionLogsToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.viewTransactionLogsToolStripMenuItem.Text = "View Transaction Logs";
            // 
            // changeStaffToolStripMenuItem
            // 
            this.changeStaffToolStripMenuItem.Name = "changeStaffToolStripMenuItem";
            this.changeStaffToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.changeStaffToolStripMenuItem.Text = "Change Staff";
            // 
            // mapTransactionWaiverToolStripMenuItem
            // 
            this.mapTransactionWaiverToolStripMenuItem.Name = "mapTransactionWaiverToolStripMenuItem";
            this.mapTransactionWaiverToolStripMenuItem.Size = new System.Drawing.Size(290, 34);
            this.mapTransactionWaiverToolStripMenuItem.Text = "Map Waiver";
            // 
            // txtTransactionId
            // 
            this.txtTransactionId.Font = new System.Drawing.Font("Arial", 15F);
            this.txtTransactionId.Location = new System.Drawing.Point(78, 51);
            this.txtTransactionId.Name = "txtTransactionId";
            this.txtTransactionId.Size = new System.Drawing.Size(119, 30);
            this.txtTransactionId.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(13, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 19);
            this.label4.TabIndex = 14;
            this.label4.Text = "Trx ID:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTransactionNumber
            // 
            this.txtTransactionNumber.Font = new System.Drawing.Font("Arial", 15F);
            this.txtTransactionNumber.Location = new System.Drawing.Point(274, 51);
            this.txtTransactionNumber.Name = "txtTransactionNumber";
            this.txtTransactionNumber.Size = new System.Drawing.Size(119, 30);
            this.txtTransactionNumber.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(216, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 19);
            this.label5.TabIndex = 16;
            this.label5.Text = "Trx #:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.Font = new System.Drawing.Font("Arial", 15F);
            this.txtPhoneNumber.Location = new System.Drawing.Point(507, 53);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(119, 30);
            this.txtPhoneNumber.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(423, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 19);
            this.label6.TabIndex = 18;
            this.label6.Text = "Phone #:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(823, 49);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(102, 41);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(698, 49);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(102, 41);
            this.btnClear.TabIndex = 20;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // verticalScrollBarView1
            // 
            this.verticalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView1.AutoHide = false;
            this.verticalScrollBarView1.DataGridView = this.dgvOrders;
            this.verticalScrollBarView1.Location = new System.Drawing.Point(956, 93);
            this.verticalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView1.Name = "verticalScrollBarView1";
            this.verticalScrollBarView1.ScrollableControl = null;
            this.verticalScrollBarView1.Size = new System.Drawing.Size(40, 407);
            this.verticalScrollBarView1.TabIndex = 13;
            // 
            // btnKeyboard
            // 
            this.btnKeyboard.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnKeyboard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnKeyboard.FlatAppearance.BorderSize = 0;
            this.btnKeyboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyboard.Location = new System.Drawing.Point(952, 48);
            this.btnKeyboard.Name = "btnKeyboard";
            this.btnKeyboard.Size = new System.Drawing.Size(39, 36);
            this.btnKeyboard.TabIndex = 21;
            this.btnKeyboard.TabStop = false;
            this.btnKeyboard.UseVisualStyleBackColor = false;
            // 
            // txtReservationCode
            // 
            this.txtReservationCode.Font = new System.Drawing.Font("Arial", 15F);
            this.txtReservationCode.Location = new System.Drawing.Point(806, 8);
            this.txtReservationCode.Name = "txtReservationCode";
            this.txtReservationCode.Size = new System.Drawing.Size(119, 30);
            this.txtReservationCode.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(648, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 19);
            this.label7.TabIndex = 22;
            this.label7.Text = "Reservation Code:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OrderListView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtReservationCode);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnKeyboard);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtPhoneNumber);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtTransactionNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtTransactionId);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.verticalScrollBarView1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtCustomer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTableNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvOrders);
            this.Name = "OrderListView";
            this.Size = new System.Drawing.Size(1000, 500);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderHeaderDTOListBS)).EndInit();
            this.ctxOrderContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTableNumber;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource orderHeaderDTOListBS;
        private System.Windows.Forms.ContextMenuStrip ctxOrderContextMenu;
        private System.Windows.Forms.ToolStripMenuItem selectOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mergeOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printKOTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelCardsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editTableDetailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewTransactionLogsToolStripMenuItem;
        private System.Windows.Forms.Button btnSearch;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView1;
        private System.Windows.Forms.TextBox txtTransactionId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTransactionNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem splitToolStripMenuItem;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ToolStripMenuItem changeStaffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapTransactionWaiverToolStripMenuItem;        
        private System.Windows.Forms.DataGridViewButtonColumn dcOrderRightClick;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectOrderDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn facilityTableNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiterNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pOSMachineNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionIDsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnKeyboard;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReservationCode;
        private System.Windows.Forms.TextBox txtReservationCode;
        private System.Windows.Forms.Label label7;
    }
}
