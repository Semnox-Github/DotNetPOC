namespace Redemption_Kiosk
{
    partial class FrmRedemptionKioskAdmin
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
            this.lblRedemptionFromDate = new System.Windows.Forms.Label();
            this.lblRedemptionToDate = new System.Windows.Forms.Label();
            this.cbxRedemptionStatus = new System.Windows.Forms.ComboBox();
            this.lblRedemptionStatus = new System.Windows.Forms.Label();
            this.txtRedemptionOrderNo = new System.Windows.Forms.TextBox();
            this.lblRedemptionOrderNo = new System.Windows.Forms.Label();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.txtProdCode = new System.Windows.Forms.TextBox();
            this.lblProdCode = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvRedemptionOrders = new System.Windows.Forms.DataGridView();
            this.redemptionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redemptionOrderNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manualTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redeemedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redemptionStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.primaryCardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origRedemptionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.graceTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiptTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderCompletedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderDeliveredDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pOSMachineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.posMachineNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.viewDetailsDataGridViewButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.redemptionDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.btnReboot = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.dtRedemptionFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtRedemptionToDate = new System.Windows.Forms.DateTimePicker();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnKeyPad = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblHeaderText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptionOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionDTOBindingSource)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRedemptionFromDate
            // 
            this.lblRedemptionFromDate.BackColor = System.Drawing.Color.Transparent;
            this.lblRedemptionFromDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRedemptionFromDate.ForeColor = System.Drawing.Color.Black;
            this.lblRedemptionFromDate.Location = new System.Drawing.Point(4, 199);
            this.lblRedemptionFromDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRedemptionFromDate.Name = "lblRedemptionFromDate";
            this.lblRedemptionFromDate.Size = new System.Drawing.Size(196, 35);
            this.lblRedemptionFromDate.TabIndex = 2;
            this.lblRedemptionFromDate.Text = "Redemption From Date:";
            this.lblRedemptionFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRedemptionToDate
            // 
            this.lblRedemptionToDate.BackColor = System.Drawing.Color.Transparent;
            this.lblRedemptionToDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRedemptionToDate.ForeColor = System.Drawing.Color.Black;
            this.lblRedemptionToDate.Location = new System.Drawing.Point(31, 265);
            this.lblRedemptionToDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRedemptionToDate.Name = "lblRedemptionToDate";
            this.lblRedemptionToDate.Size = new System.Drawing.Size(169, 27);
            this.lblRedemptionToDate.TabIndex = 4;
            this.lblRedemptionToDate.Text = "Redemption To Date:";
            this.lblRedemptionToDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRedemptionStatus
            // 
            this.cbxRedemptionStatus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxRedemptionStatus.Location = new System.Drawing.Point(529, 263);
            this.cbxRedemptionStatus.Margin = new System.Windows.Forms.Padding(4);
            this.cbxRedemptionStatus.Name = "cbxRedemptionStatus";
            this.cbxRedemptionStatus.Size = new System.Drawing.Size(178, 26);
            this.cbxRedemptionStatus.TabIndex = 9;
            this.cbxRedemptionStatus.Click += new System.EventHandler(this.textBox_Leave);
            //this.cbxRedemptionStatus.Enter += new System.EventHandler(this.textBox_Enter);
            //this.cbxRedemptionStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtRedemptionFromDate_KeyPress);
            this.cbxRedemptionStatus.Leave += new System.EventHandler(this.textBox_Leave);
            this.cbxRedemptionStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dtRedemptionFromDate_MouseDown);
            this.cbxRedemptionStatus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dtRedemptionFromDate_MouseDown);
            // 
            // lblRedemptionStatus
            // 
            this.lblRedemptionStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblRedemptionStatus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRedemptionStatus.ForeColor = System.Drawing.Color.Black;
            this.lblRedemptionStatus.Location = new System.Drawing.Point(351, 265);
            this.lblRedemptionStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRedemptionStatus.Name = "lblRedemptionStatus";
            this.lblRedemptionStatus.Size = new System.Drawing.Size(171, 25);
            this.lblRedemptionStatus.TabIndex = 8;
            this.lblRedemptionStatus.Text = "Redemption Status:";
            this.lblRedemptionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRedemptionOrderNo
            // 
            this.txtRedemptionOrderNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRedemptionOrderNo.Location = new System.Drawing.Point(529, 210);
            this.txtRedemptionOrderNo.Margin = new System.Windows.Forms.Padding(4);
            this.txtRedemptionOrderNo.Name = "txtRedemptionOrderNo";
            this.txtRedemptionOrderNo.Size = new System.Drawing.Size(178, 26);
            this.txtRedemptionOrderNo.TabIndex = 7;
            this.txtRedemptionOrderNo.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtRedemptionOrderNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtRedemptionFromDate_KeyPress);
            this.txtRedemptionOrderNo.Leave += new System.EventHandler(this.textBox_Leave);
            this.txtRedemptionOrderNo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dtRedemptionFromDate_MouseDown);
            this.txtRedemptionOrderNo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dtRedemptionFromDate_MouseDown);
            // 
            // lblRedemptionOrderNo
            // 
            this.lblRedemptionOrderNo.BackColor = System.Drawing.Color.Transparent;
            this.lblRedemptionOrderNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRedemptionOrderNo.ForeColor = System.Drawing.Color.Black;
            this.lblRedemptionOrderNo.Location = new System.Drawing.Point(352, 209);
            this.lblRedemptionOrderNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRedemptionOrderNo.Name = "lblRedemptionOrderNo";
            this.lblRedemptionOrderNo.Size = new System.Drawing.Size(170, 25);
            this.lblRedemptionOrderNo.TabIndex = 6;
            this.lblRedemptionOrderNo.Text = "Redemption Order No:";
            this.lblRedemptionOrderNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.Location = new System.Drawing.Point(861, 263);
            this.txtCardNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(146, 26);
            this.txtCardNumber.TabIndex = 13;
            this.txtCardNumber.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtCardNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtRedemptionFromDate_KeyPress);
            this.txtCardNumber.Leave += new System.EventHandler(this.textBox_Leave);
            this.txtCardNumber.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dtRedemptionFromDate_MouseDown);
            this.txtCardNumber.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dtRedemptionFromDate_MouseDown);
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblCardNumber.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.ForeColor = System.Drawing.Color.Black;
            this.lblCardNumber.Location = new System.Drawing.Point(714, 266);
            this.lblCardNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(139, 25);
            this.lblCardNumber.TabIndex = 12;
            this.lblCardNumber.Text = "Card Number:";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProdCode
            // 
            this.txtProdCode.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProdCode.Location = new System.Drawing.Point(861, 210);
            this.txtProdCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtProdCode.Name = "txtProdCode";
            this.txtProdCode.Size = new System.Drawing.Size(146, 26);
            this.txtProdCode.TabIndex = 11;
            this.txtProdCode.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtProdCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmRedemptionKioskAdmin_KeyPress);
            this.txtProdCode.Leave += new System.EventHandler(this.textBox_Leave);
            this.txtProdCode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dtRedemptionFromDate_MouseDown);
            this.txtProdCode.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dtRedemptionFromDate_MouseDown);
            // 
            // lblProdCode
            // 
            this.lblProdCode.BackColor = System.Drawing.Color.Transparent;
            this.lblProdCode.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProdCode.ForeColor = System.Drawing.Color.Black;
            this.lblProdCode.Location = new System.Drawing.Point(714, 198);
            this.lblProdCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProdCode.Name = "lblProdCode";
            this.lblProdCode.Size = new System.Drawing.Size(139, 51);
            this.lblProdCode.TabIndex = 10;
            this.lblProdCode.Text = "Prod Code/ Desc/Barcode:";
            this.lblProdCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Search;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.GhostWhite;
            this.btnSearch.Location = new System.Drawing.Point(586, 299);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(177, 68);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvRedemptionOrders
            // 
            this.dgvRedemptionOrders.AllowUserToAddRows = false;
            this.dgvRedemptionOrders.AllowUserToDeleteRows = false;
            this.dgvRedemptionOrders.AllowUserToResizeColumns = false;
            this.dgvRedemptionOrders.AllowUserToResizeRows = false;
            this.dgvRedemptionOrders.AutoGenerateColumns = false;
            this.dgvRedemptionOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvRedemptionOrders.BackgroundColor = System.Drawing.Color.White;
            this.dgvRedemptionOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRedemptionOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.redemptionIdDataGridViewTextBoxColumn,
            this.redemptionOrderNoDataGridViewTextBoxColumn,
            this.manualTicketsDataGridViewTextBoxColumn,
            this.eTicketsDataGridViewTextBoxColumn,
            this.redeemedDateDataGridViewTextBoxColumn,
            this.redemptionStatusDataGridViewTextBoxColumn,
            this.primaryCardNumberDataGridViewTextBoxColumn,
            this.cardIdDataGridViewTextBoxColumn,
            this.origRedemptionIdDataGridViewTextBoxColumn,
            this.customerNameDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.graceTicketsDataGridViewTextBoxColumn,
            this.receiptTicketsDataGridViewTextBoxColumn,
            this.currencyTicketsDataGridViewTextBoxColumn,
            this.sourceDataGridViewTextBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn,
            this.orderCompletedDateDataGridViewTextBoxColumn,
            this.orderDeliveredDateDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.pOSMachineIdDataGridViewTextBoxColumn,
            this.customerIdDataGridViewTextBoxColumn,
            this.posMachineNameDataGridViewTextBoxColumn,
            this.viewDetailsDataGridViewButtonColumn});
            this.dgvRedemptionOrders.DataSource = this.redemptionDTOBindingSource;
            this.dgvRedemptionOrders.Location = new System.Drawing.Point(4, 375);
            this.dgvRedemptionOrders.Margin = new System.Windows.Forms.Padding(4);
            this.dgvRedemptionOrders.MultiSelect = false;
            this.dgvRedemptionOrders.Name = "dgvRedemptionOrders";
            this.dgvRedemptionOrders.ReadOnly = true;
            this.dgvRedemptionOrders.RowHeadersWidth = 9;
            this.dgvRedemptionOrders.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRedemptionOrders.RowTemplate.Height = 35;
            this.dgvRedemptionOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRedemptionOrders.Size = new System.Drawing.Size(1015, 825);
            this.dgvRedemptionOrders.TabIndex = 16;
            this.dgvRedemptionOrders.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRedemptionOrders_CellContentClick);
            this.dgvRedemptionOrders.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRedemptionOrders_CellFormatting);
            // 
            // redemptionIdDataGridViewTextBoxColumn
            // 
            this.redemptionIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.redemptionIdDataGridViewTextBoxColumn.DataPropertyName = "RedemptionId";
            this.redemptionIdDataGridViewTextBoxColumn.HeaderText = "Id";
            this.redemptionIdDataGridViewTextBoxColumn.Name = "redemptionIdDataGridViewTextBoxColumn";
            this.redemptionIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.redemptionIdDataGridViewTextBoxColumn.Width = 110;
            // 
            // redemptionOrderNoDataGridViewTextBoxColumn
            // 
            this.redemptionOrderNoDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.redemptionOrderNoDataGridViewTextBoxColumn.DataPropertyName = "RedemptionOrderNo";
            this.redemptionOrderNoDataGridViewTextBoxColumn.HeaderText = "Order Number";
            this.redemptionOrderNoDataGridViewTextBoxColumn.Name = "redemptionOrderNoDataGridViewTextBoxColumn";
            this.redemptionOrderNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.redemptionOrderNoDataGridViewTextBoxColumn.Width = 150;
            // 
            // manualTicketsDataGridViewTextBoxColumn
            // 
            this.manualTicketsDataGridViewTextBoxColumn.DataPropertyName = "ManualTickets";
            this.manualTicketsDataGridViewTextBoxColumn.HeaderText = "Manual Tickets";
            this.manualTicketsDataGridViewTextBoxColumn.Name = "manualTicketsDataGridViewTextBoxColumn";
            this.manualTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            this.manualTicketsDataGridViewTextBoxColumn.Visible = false;
            this.manualTicketsDataGridViewTextBoxColumn.Width = 105;
            // 
            // eTicketsDataGridViewTextBoxColumn
            // 
            this.eTicketsDataGridViewTextBoxColumn.DataPropertyName = "ETickets";
            this.eTicketsDataGridViewTextBoxColumn.HeaderText = "e-Tickets";
            this.eTicketsDataGridViewTextBoxColumn.Name = "eTicketsDataGridViewTextBoxColumn";
            this.eTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            this.eTicketsDataGridViewTextBoxColumn.Visible = false;
            this.eTicketsDataGridViewTextBoxColumn.Width = 76;
            // 
            // redeemedDateDataGridViewTextBoxColumn
            // 
            this.redeemedDateDataGridViewTextBoxColumn.DataPropertyName = "RedeemedDate";
            this.redeemedDateDataGridViewTextBoxColumn.HeaderText = "Redeemed Date";
            this.redeemedDateDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.redeemedDateDataGridViewTextBoxColumn.Name = "redeemedDateDataGridViewTextBoxColumn";
            this.redeemedDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.redeemedDateDataGridViewTextBoxColumn.Width = 195;
            // 
            // redemptionStatusDataGridViewTextBoxColumn
            // 
            this.redemptionStatusDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.redemptionStatusDataGridViewTextBoxColumn.DataPropertyName = "RedemptionStatus";
            this.redemptionStatusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.redemptionStatusDataGridViewTextBoxColumn.Name = "redemptionStatusDataGridViewTextBoxColumn";
            this.redemptionStatusDataGridViewTextBoxColumn.ReadOnly = true;
            this.redemptionStatusDataGridViewTextBoxColumn.Width = 130;
            // 
            // primaryCardNumberDataGridViewTextBoxColumn
            // 
            this.primaryCardNumberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.primaryCardNumberDataGridViewTextBoxColumn.DataPropertyName = "PrimaryCardNumber";
            this.primaryCardNumberDataGridViewTextBoxColumn.HeaderText = "Primary Card #";
            this.primaryCardNumberDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.primaryCardNumberDataGridViewTextBoxColumn.Name = "primaryCardNumberDataGridViewTextBoxColumn";
            this.primaryCardNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.primaryCardNumberDataGridViewTextBoxColumn.Width = 170;
            // 
            // cardIdDataGridViewTextBoxColumn
            // 
            this.cardIdDataGridViewTextBoxColumn.DataPropertyName = "CardId";
            this.cardIdDataGridViewTextBoxColumn.HeaderText = "Card Id";
            this.cardIdDataGridViewTextBoxColumn.Name = "cardIdDataGridViewTextBoxColumn";
            this.cardIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.cardIdDataGridViewTextBoxColumn.Visible = false;
            this.cardIdDataGridViewTextBoxColumn.Width = 61;
            // 
            // origRedemptionIdDataGridViewTextBoxColumn
            // 
            this.origRedemptionIdDataGridViewTextBoxColumn.DataPropertyName = "OrigRedemptionId";
            this.origRedemptionIdDataGridViewTextBoxColumn.HeaderText = "Original Redemption Id";
            this.origRedemptionIdDataGridViewTextBoxColumn.Name = "origRedemptionIdDataGridViewTextBoxColumn";
            this.origRedemptionIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.origRedemptionIdDataGridViewTextBoxColumn.Visible = false;
            this.origRedemptionIdDataGridViewTextBoxColumn.Width = 116;
            // 
            // customerNameDataGridViewTextBoxColumn
            // 
            this.customerNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.customerNameDataGridViewTextBoxColumn.DataPropertyName = "CustomerName";
            this.customerNameDataGridViewTextBoxColumn.HeaderText = "Customer ";
            this.customerNameDataGridViewTextBoxColumn.Name = "customerNameDataGridViewTextBoxColumn";
            this.customerNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerNameDataGridViewTextBoxColumn.Width = 148;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            this.remarksDataGridViewTextBoxColumn.ReadOnly = true;
            this.remarksDataGridViewTextBoxColumn.Visible = false;
            this.remarksDataGridViewTextBoxColumn.Width = 74;
            // 
            // graceTicketsDataGridViewTextBoxColumn
            // 
            this.graceTicketsDataGridViewTextBoxColumn.DataPropertyName = "GraceTickets";
            this.graceTicketsDataGridViewTextBoxColumn.HeaderText = "Grace Tickets";
            this.graceTicketsDataGridViewTextBoxColumn.Name = "graceTicketsDataGridViewTextBoxColumn";
            this.graceTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            this.graceTicketsDataGridViewTextBoxColumn.Visible = false;
            this.graceTicketsDataGridViewTextBoxColumn.Width = 91;
            // 
            // receiptTicketsDataGridViewTextBoxColumn
            // 
            this.receiptTicketsDataGridViewTextBoxColumn.DataPropertyName = "ReceiptTickets";
            this.receiptTicketsDataGridViewTextBoxColumn.HeaderText = "Receipt Tickets";
            this.receiptTicketsDataGridViewTextBoxColumn.Name = "receiptTicketsDataGridViewTextBoxColumn";
            this.receiptTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            this.receiptTicketsDataGridViewTextBoxColumn.Visible = false;
            this.receiptTicketsDataGridViewTextBoxColumn.Width = 98;
            // 
            // currencyTicketsDataGridViewTextBoxColumn
            // 
            this.currencyTicketsDataGridViewTextBoxColumn.DataPropertyName = "CurrencyTickets";
            this.currencyTicketsDataGridViewTextBoxColumn.HeaderText = "Currency Tickets";
            this.currencyTicketsDataGridViewTextBoxColumn.Name = "currencyTicketsDataGridViewTextBoxColumn";
            this.currencyTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            this.currencyTicketsDataGridViewTextBoxColumn.Visible = false;
            this.currencyTicketsDataGridViewTextBoxColumn.Width = 103;
            // 
            // sourceDataGridViewTextBoxColumn
            // 
            this.sourceDataGridViewTextBoxColumn.DataPropertyName = "Source";
            this.sourceDataGridViewTextBoxColumn.HeaderText = "Source";
            this.sourceDataGridViewTextBoxColumn.Name = "sourceDataGridViewTextBoxColumn";
            this.sourceDataGridViewTextBoxColumn.ReadOnly = true;
            this.sourceDataGridViewTextBoxColumn.Visible = false;
            this.sourceDataGridViewTextBoxColumn.Width = 66;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdateDateDataGridViewTextBoxColumn.Visible = false;
            this.lastUpdateDateDataGridViewTextBoxColumn.Width = 110;
            // 
            // orderCompletedDateDataGridViewTextBoxColumn
            // 
            this.orderCompletedDateDataGridViewTextBoxColumn.DataPropertyName = "OrderCompletedDate";
            this.orderCompletedDateDataGridViewTextBoxColumn.HeaderText = "OrderCompletedDate";
            this.orderCompletedDateDataGridViewTextBoxColumn.Name = "orderCompletedDateDataGridViewTextBoxColumn";
            this.orderCompletedDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.orderCompletedDateDataGridViewTextBoxColumn.Visible = false;
            this.orderCompletedDateDataGridViewTextBoxColumn.Width = 131;
            // 
            // orderDeliveredDateDataGridViewTextBoxColumn
            // 
            this.orderDeliveredDateDataGridViewTextBoxColumn.DataPropertyName = "OrderDeliveredDate";
            this.orderDeliveredDateDataGridViewTextBoxColumn.HeaderText = "OrderDeliveredDate";
            this.orderDeliveredDateDataGridViewTextBoxColumn.Name = "orderDeliveredDateDataGridViewTextBoxColumn";
            this.orderDeliveredDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.orderDeliveredDateDataGridViewTextBoxColumn.Visible = false;
            this.orderDeliveredDateDataGridViewTextBoxColumn.Width = 126;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.creationDateDataGridViewTextBoxColumn.Visible = false;
            this.creationDateDataGridViewTextBoxColumn.Width = 94;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdByDataGridViewTextBoxColumn.Visible = false;
            this.createdByDataGridViewTextBoxColumn.Width = 81;
            // 
            // pOSMachineIdDataGridViewTextBoxColumn
            // 
            this.pOSMachineIdDataGridViewTextBoxColumn.DataPropertyName = "POSMachineId";
            this.pOSMachineIdDataGridViewTextBoxColumn.HeaderText = "POS Machine Id";
            this.pOSMachineIdDataGridViewTextBoxColumn.Name = "pOSMachineIdDataGridViewTextBoxColumn";
            this.pOSMachineIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.pOSMachineIdDataGridViewTextBoxColumn.Visible = false;
            this.pOSMachineIdDataGridViewTextBoxColumn.Width = 93;
            // 
            // customerIdDataGridViewTextBoxColumn
            // 
            this.customerIdDataGridViewTextBoxColumn.DataPropertyName = "CustomerId";
            this.customerIdDataGridViewTextBoxColumn.HeaderText = "Customer Id";
            this.customerIdDataGridViewTextBoxColumn.Name = "customerIdDataGridViewTextBoxColumn";
            this.customerIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerIdDataGridViewTextBoxColumn.Visible = false;
            this.customerIdDataGridViewTextBoxColumn.Width = 81;
            // 
            // posMachineNameDataGridViewTextBoxColumn
            // 
            this.posMachineNameDataGridViewTextBoxColumn.DataPropertyName = "PosMachineName";
            this.posMachineNameDataGridViewTextBoxColumn.HeaderText = "POS machine Name";
            this.posMachineNameDataGridViewTextBoxColumn.Name = "posMachineNameDataGridViewTextBoxColumn";
            this.posMachineNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.posMachineNameDataGridViewTextBoxColumn.Visible = false;
            this.posMachineNameDataGridViewTextBoxColumn.Width = 117;
            // 
            // viewDetailsDataGridViewButtonColumn
            // 
            this.viewDetailsDataGridViewButtonColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            this.viewDetailsDataGridViewButtonColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.viewDetailsDataGridViewButtonColumn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.viewDetailsDataGridViewButtonColumn.HeaderText = "Details";
            this.viewDetailsDataGridViewButtonColumn.Name = "viewDetailsDataGridViewButtonColumn";
            this.viewDetailsDataGridViewButtonColumn.ReadOnly = true;
            this.viewDetailsDataGridViewButtonColumn.Text = "View Details";
            this.viewDetailsDataGridViewButtonColumn.UseColumnTextForButtonValue = true;
            this.viewDetailsDataGridViewButtonColumn.Width = 150;
            // 
            // redemptionDTOBindingSource
            // 
            this.redemptionDTOBindingSource.DataSource = typeof(Semnox.Parafait.Redemption.RedemptionDTO);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Blank_Btn;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(31, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(291, 145);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnReboot
            // 
            this.btnReboot.BackColor = System.Drawing.Color.Transparent;
            this.btnReboot.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Blank_Btn;
            this.btnReboot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReboot.FlatAppearance.BorderSize = 0;
            this.btnReboot.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnReboot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnReboot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReboot.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReboot.ForeColor = System.Drawing.Color.GhostWhite;
            this.btnReboot.Location = new System.Drawing.Point(352, 4);
            this.btnReboot.Margin = new System.Windows.Forms.Padding(4);
            this.btnReboot.Name = "btnReboot";
            this.btnReboot.Size = new System.Drawing.Size(291, 145);
            this.btnReboot.TabIndex = 18;
            this.btnReboot.Text = "REBOOT";
            this.btnReboot.UseVisualStyleBackColor = false;
            this.btnReboot.Click += new System.EventHandler(this.btnReboot_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Blank_Btn;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.GhostWhite;
            this.btnPrint.Location = new System.Drawing.Point(676, 4);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(291, 145);
            this.btnPrint.TabIndex = 19;
            this.btnPrint.Text = "PRINT";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // dtRedemptionFromDate
            // 
            this.dtRedemptionFromDate.CalendarFont = new System.Drawing.Font("Arial", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtRedemptionFromDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtRedemptionFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtRedemptionFromDate.Location = new System.Drawing.Point(207, 207);
            this.dtRedemptionFromDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtRedemptionFromDate.Name = "dtRedemptionFromDate";
            this.dtRedemptionFromDate.Size = new System.Drawing.Size(137, 26);
            this.dtRedemptionFromDate.TabIndex = 20;
            this.dtRedemptionFromDate.Value = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtRedemptionFromDate.ValueChanged += new System.EventHandler(this.dtRedemptionFromDate_ValueChanged);
            this.dtRedemptionFromDate.Enter += new System.EventHandler(this.dtRedemptionFromDate_Enter);
            this.dtRedemptionFromDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtRedemptionFromDate_KeyPress);
            this.dtRedemptionFromDate.Leave += new System.EventHandler(this.dtRedemptionFromDate_Leave);
            this.dtRedemptionFromDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dtRedemptionFromDate_MouseDown);
            this.dtRedemptionFromDate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dtRedemptionFromDate_MouseDown);
            // 
            // dtRedemptionToDate
            // 
            this.dtRedemptionToDate.CalendarFont = new System.Drawing.Font("Arial", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtRedemptionToDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtRedemptionToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtRedemptionToDate.Location = new System.Drawing.Point(207, 264);
            this.dtRedemptionToDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtRedemptionToDate.Name = "dtRedemptionToDate";
            this.dtRedemptionToDate.Size = new System.Drawing.Size(137, 26);
            this.dtRedemptionToDate.TabIndex = 21;
            this.dtRedemptionToDate.Value = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtRedemptionToDate.ValueChanged += new System.EventHandler(this.dtRedemptionFromDate_ValueChanged);
            this.dtRedemptionToDate.Enter += new System.EventHandler(this.dtRedemptionFromDate_Enter);
            this.dtRedemptionToDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtRedemptionFromDate_KeyPress);
            this.dtRedemptionToDate.Leave += new System.EventHandler(this.dtRedemptionFromDate_Leave);
            this.dtRedemptionToDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dtRedemptionFromDate_MouseDown);
            this.dtRedemptionToDate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dtRedemptionFromDate_MouseDown);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.Transparent;
            this.pnlBottom.Controls.Add(this.btnExit);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnReboot);
            this.pnlBottom.Controls.Add(this.btnPrint);
            this.pnlBottom.Location = new System.Drawing.Point(4, 1205);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1015, 316);
            this.pnlBottom.TabIndex = 22;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Blank_Btn;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.GhostWhite;
            this.btnExit.Location = new System.Drawing.Point(352, 162);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(291, 145);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "EXIT Kiosk";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnKeyPad
            // 
            this.btnKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.keyboard;
            this.btnKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnKeyPad.FlatAppearance.BorderSize = 0;
            this.btnKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyPad.Location = new System.Drawing.Point(785, 333);
            this.btnKeyPad.Margin = new System.Windows.Forms.Padding(4);
            this.btnKeyPad.Name = "btnKeyPad";
            this.btnKeyPad.Size = new System.Drawing.Size(60, 34);
            this.btnKeyPad.TabIndex = 15;
            this.btnKeyPad.UseVisualStyleBackColor = false;
            this.btnKeyPad.Click += new System.EventHandler(this.btnKeyPad_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Search;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(371, 299);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(177, 68);
            this.btnClear.TabIndex = 23;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblHeaderText
            // 
            this.lblHeaderText.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderText.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderText.ForeColor = System.Drawing.Color.White;
            this.lblHeaderText.Location = new System.Drawing.Point(46, 33);
            this.lblHeaderText.Name = "lblHeaderText";
            this.lblHeaderText.Size = new System.Drawing.Size(877, 81);
            this.lblHeaderText.TabIndex = 24;
            this.lblHeaderText.Text = "ADMIN SCREEN";
            this.lblHeaderText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmRedemptionKioskAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Redemption_Kiosk_AgeGate_Bg;
            this.ClientSize = new System.Drawing.Size(991, 1673);
            this.Controls.Add(this.lblHeaderText);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.dtRedemptionToDate);
            this.Controls.Add(this.dtRedemptionFromDate);
            this.Controls.Add(this.dgvRedemptionOrders);
            this.Controls.Add(this.btnKeyPad);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.lblCardNumber);
            this.Controls.Add(this.txtProdCode);
            this.Controls.Add(this.lblProdCode);
            this.Controls.Add(this.cbxRedemptionStatus);
            this.Controls.Add(this.lblRedemptionStatus);
            this.Controls.Add(this.txtRedemptionOrderNo);
            this.Controls.Add(this.lblRedemptionOrderNo);
            this.Controls.Add(this.lblRedemptionToDate);
            this.Controls.Add(this.lblRedemptionFromDate);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.Name = "FrmRedemptionKioskAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Text = "frmRedemptionKioskAdmin";
            this.Activated += new System.EventHandler(this.FrmRedemptionKioskAdmin_Activated);
            this.Deactivate += new System.EventHandler(this.FrmRedemptionKioskAdmin_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRedemptionKioskAdmin_FormClosing);
            this.Load += new System.EventHandler(this.frmRedemptionKioskAdmin_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmRedemptionKioskAdmin_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptionOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionDTOBindingSource)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblRedemptionFromDate;
        private System.Windows.Forms.Label lblRedemptionToDate;
        private System.Windows.Forms.ComboBox cbxRedemptionStatus;
        private System.Windows.Forms.Label lblRedemptionStatus;
        private System.Windows.Forms.TextBox txtRedemptionOrderNo;
        private System.Windows.Forms.Label lblRedemptionOrderNo;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.TextBox txtProdCode;
        private System.Windows.Forms.Label lblProdCode;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnKeyPad;
        private System.Windows.Forms.DataGridView dgvRedemptionOrders;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReboot;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DateTimePicker dtRedemptionFromDate;
        private System.Windows.Forms.DateTimePicker dtRedemptionToDate;
        private System.Windows.Forms.BindingSource redemptionDTOBindingSource;
        private System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1;
        private System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2;
        private System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3;
        private System.Windows.Forms.DataGridViewTextBoxColumn redemptionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn redemptionOrderNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn manualTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn redeemedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn redemptionStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn primaryCardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn origRedemptionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn graceTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiptTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderCompletedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderDeliveredDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pOSMachineIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn posMachineNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn viewDetailsDataGridViewButtonColumn;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblHeaderText;
    }
}