namespace Semnox.Parafait.Transaction
{
    
    partial class ViewTransactions
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
            this.dgvTrxHeader = new System.Windows.Forms.DataGridView();
            this.btnCancelTrx = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnPrint = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnTrxReopen = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgvTrxDetails = new System.Windows.Forms.DataGridView();
            this.btnCancelLine = new System.Windows.Forms.DataGridViewImageColumn();
            this.grpHeader = new System.Windows.Forms.GroupBox();
            this.filterPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.rdToday = new System.Windows.Forms.RadioButton();
            this.rdPast = new System.Windows.Forms.RadioButton();
            this.rdFuture3 = new System.Windows.Forms.RadioButton();
            this.rdFutureAll = new System.Windows.Forms.RadioButton();
            this.btnExcel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.splitContainerTrx = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbUser = new System.Windows.Forms.ComboBox();
            this.cmbPOS = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbDetailOnly = new System.Windows.Forms.RadioButton();
            this.rbHeaderDetail = new System.Windows.Forms.RadioButton();
            this.panelViewTransactions = new System.Windows.Forms.Panel();
            this.grpCriteria = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbTrxStatus = new System.Windows.Forms.ComboBox();
            this.txtTrxId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.verticalScrollBarView1 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.horizontalScrollBarView1 = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.verticalScrollBarView2 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.horizontalScrollBarView2 = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxDetails)).BeginInit();
            this.grpHeader.SuspendLayout();
            this.filterPanel.SuspendLayout();
            this.grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTrx)).BeginInit();
            this.splitContainerTrx.Panel1.SuspendLayout();
            this.splitContainerTrx.Panel2.SuspendLayout();
            this.splitContainerTrx.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelViewTransactions.SuspendLayout();
            this.grpCriteria.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTrxHeader
            // 
            this.dgvTrxHeader.AllowUserToAddRows = false;
            this.dgvTrxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTrxHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnCancelTrx,
            this.btnPrint,
            this.btnTrxReopen});
            this.dgvTrxHeader.Location = new System.Drawing.Point(8, 31);
            this.dgvTrxHeader.Name = "dgvTrxHeader";
            this.dgvTrxHeader.RowHeadersWidth = 45;
            this.dgvTrxHeader.RowTemplate.Height = 35;
            this.dgvTrxHeader.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvTrxHeader.Size = new System.Drawing.Size(1015, 165);
            this.dgvTrxHeader.TabIndex = 0;
            this.dgvTrxHeader.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrxHeader_CellClick);
            this.dgvTrxHeader.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrxHeader_CellMouseEnter);
            this.dgvTrxHeader.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrxHeader_CellMouseLeave);
            this.dgvTrxHeader.SelectionChanged += new System.EventHandler(this.dgvTrxHeader_SelectionChanged);
            // 
            // btnCancelTrx
            // 
            this.btnCancelTrx.HeaderText = "X";
            this.btnCancelTrx.Image = global::Semnox.Parafait.Transaction.Properties.Resources.cancelIcon;
            this.btnCancelTrx.MinimumWidth = 35;
            this.btnCancelTrx.Name = "btnCancelTrx";
            this.btnCancelTrx.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnCancelTrx.ToolTipText = "Reverse Transaction";
            this.btnCancelTrx.Width = 40;
            // 
            // btnPrint
            // 
            this.btnPrint.HeaderText = "P";
            this.btnPrint.Image = global::Semnox.Parafait.Transaction.Properties.Resources.printer;
            this.btnPrint.MinimumWidth = 35;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnPrint.ToolTipText = "Print Transaction";
            this.btnPrint.Width = 40;
            // 
            // btnTrxReopen
            // 
            this.btnTrxReopen.HeaderText = "O";
            this.btnTrxReopen.Image = global::Semnox.Parafait.Transaction.Properties.Resources.Trx_Reopen;
            this.btnTrxReopen.MinimumWidth = 35;
            this.btnTrxReopen.Name = "btnTrxReopen";
            this.btnTrxReopen.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnTrxReopen.ToolTipText = "Reopen Transaction";
            this.btnTrxReopen.Width = 40;
            // 
            // dgvTrxDetails
            // 
            this.dgvTrxDetails.AllowUserToAddRows = false;
            this.dgvTrxDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTrxDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrxDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnCancelLine});
            this.dgvTrxDetails.Location = new System.Drawing.Point(6, 20);
            this.dgvTrxDetails.Name = "dgvTrxDetails";
            this.dgvTrxDetails.RowTemplate.Height = 35;
            this.dgvTrxDetails.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvTrxDetails.Size = new System.Drawing.Size(1015, 120);
            this.dgvTrxDetails.TabIndex = 1;
            this.dgvTrxDetails.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrxDetails_CellContentClick);
            this.dgvTrxDetails.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrxDetails_CellMouseEnter);
            this.dgvTrxDetails.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrxDetails_CellMouseLeave);
            // 
            // btnCancelLine
            // 
            this.btnCancelLine.HeaderText = "X";
            this.btnCancelLine.Image = global::Semnox.Parafait.Transaction.Properties.Resources.cancelIcon;
            this.btnCancelLine.MinimumWidth = 35;
            this.btnCancelLine.Name = "btnCancelLine";
            this.btnCancelLine.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnCancelLine.ToolTipText = "Reverse Line";
            this.btnCancelLine.Width = 40;
            // 
            // grpHeader
            // 
            this.grpHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpHeader.Controls.Add(this.horizontalScrollBarView1);
            this.grpHeader.Controls.Add(this.verticalScrollBarView1);
            this.grpHeader.Controls.Add(this.dgvTrxHeader);
            this.grpHeader.Controls.Add(this.filterPanel);
            this.grpHeader.Location = new System.Drawing.Point(3, 3);
            this.grpHeader.Name = "grpHeader";
            this.grpHeader.Size = new System.Drawing.Size(1066, 239);
            this.grpHeader.TabIndex = 2;
            this.grpHeader.TabStop = false;
            this.grpHeader.Text = "Transaction Header";
            // 
            // filterPanel
            // 
            this.filterPanel.AutoScroll = true;
            this.filterPanel.BackColor = System.Drawing.Color.Ivory;
            this.filterPanel.Controls.Add(this.rdToday);
            this.filterPanel.Controls.Add(this.rdPast);
            this.filterPanel.Controls.Add(this.rdFuture3);
            this.filterPanel.Controls.Add(this.rdFutureAll);
            this.filterPanel.Location = new System.Drawing.Point(6, 0);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Size = new System.Drawing.Size(1015, 28);
            this.filterPanel.TabIndex = 0;
            this.filterPanel.Visible = false;
            // 
            // rdToday
            // 
            this.rdToday.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdToday.AutoEllipsis = true;
            this.rdToday.Location = new System.Drawing.Point(3, 3);
            this.rdToday.Name = "rdToday";
            this.rdToday.Size = new System.Drawing.Size(130, 22);
            this.rdToday.TabIndex = 1;
            this.rdToday.TabStop = true;
            this.rdToday.Text = "Today";
            this.rdToday.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdToday.UseVisualStyleBackColor = true;
            this.rdToday.CheckedChanged += new System.EventHandler(this.rdToday_CheckedChanged);
            // 
            // rdPast
            // 
            this.rdPast.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdPast.AutoEllipsis = true;
            this.rdPast.Location = new System.Drawing.Point(139, 3);
            this.rdPast.Name = "rdPast";
            this.rdPast.Size = new System.Drawing.Size(130, 22);
            this.rdPast.TabIndex = 2;
            this.rdPast.Text = "Past 3 days";
            this.rdPast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdPast.UseVisualStyleBackColor = true;
            this.rdPast.CheckedChanged += new System.EventHandler(this.rdPast_Click);
            // 
            // rdFuture3
            // 
            this.rdFuture3.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdFuture3.AutoEllipsis = true;
            this.rdFuture3.Location = new System.Drawing.Point(275, 3);
            this.rdFuture3.Name = "rdFuture3";
            this.rdFuture3.Size = new System.Drawing.Size(130, 22);
            this.rdFuture3.TabIndex = 3;
            this.rdFuture3.Text = "Future 3 Days";
            this.rdFuture3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdFuture3.UseVisualStyleBackColor = true;
            this.rdFuture3.CheckedChanged += new System.EventHandler(this.rdFuture3_Click);
            // 
            // rdFutureAll
            // 
            this.rdFutureAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdFutureAll.AutoEllipsis = true;
            this.rdFutureAll.Location = new System.Drawing.Point(411, 3);
            this.rdFutureAll.Name = "rdFutureAll";
            this.rdFutureAll.Size = new System.Drawing.Size(130, 22);
            this.rdFutureAll.TabIndex = 3;
            this.rdFutureAll.Text = "All Future";
            this.rdFutureAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdFutureAll.UseVisualStyleBackColor = true;
            this.rdFutureAll.CheckedChanged += new System.EventHandler(this.rdFutureAll_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnExcel.FlatAppearance.BorderSize = 0;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcel.Location = new System.Drawing.Point(808, 43);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(120, 24);
            this.btnExcel.TabIndex = 6;
            this.btnExcel.Text = "Export To Excel";
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(293, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "To:";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(808, 17);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "From:";
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(323, 17);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(161, 21);
            this.dtpTo.TabIndex = 2;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(55, 17);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(161, 21);
            this.dtpFrom.TabIndex = 1;
            // 
            // grpDetails
            // 
            this.grpDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDetails.Controls.Add(this.horizontalScrollBarView2);
            this.grpDetails.Controls.Add(this.verticalScrollBarView2);
            this.grpDetails.Controls.Add(this.dgvTrxDetails);
            this.grpDetails.Location = new System.Drawing.Point(3, 3);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new System.Drawing.Size(1066, 186);
            this.grpDetails.TabIndex = 3;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Details";
            // 
            // splitContainerTrx
            // 
            this.splitContainerTrx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerTrx.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerTrx.Location = new System.Drawing.Point(9, 81);
            this.splitContainerTrx.Name = "splitContainerTrx";
            this.splitContainerTrx.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTrx.Panel1
            // 
            this.splitContainerTrx.Panel1.Controls.Add(this.grpHeader);
            // 
            // splitContainerTrx.Panel2
            // 
            this.splitContainerTrx.Panel2.Controls.Add(this.grpDetails);
            this.splitContainerTrx.Size = new System.Drawing.Size(1076, 443);
            this.splitContainerTrx.SplitterDistance = 249;
            this.splitContainerTrx.SplitterWidth = 3;
            this.splitContainerTrx.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "User:";
            // 
            // cmbUser
            // 
            this.cmbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.Location = new System.Drawing.Point(55, 44);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(161, 23);
            this.cmbUser.TabIndex = 9;
            // 
            // cmbPOS
            // 
            this.cmbPOS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPOS.FormattingEnabled = true;
            this.cmbPOS.Location = new System.Drawing.Point(323, 44);
            this.cmbPOS.Name = "cmbPOS";
            this.cmbPOS.Size = new System.Drawing.Size(161, 23);
            this.cmbPOS.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(280, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "POS:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbDetailOnly);
            this.groupBox1.Controls.Add(this.rbHeaderDetail);
            this.groupBox1.Location = new System.Drawing.Point(934, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 58);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // rbDetailOnly
            // 
            this.rbDetailOnly.AutoSize = true;
            this.rbDetailOnly.Location = new System.Drawing.Point(7, 34);
            this.rbDetailOnly.Name = "rbDetailOnly";
            this.rbDetailOnly.Size = new System.Drawing.Size(85, 19);
            this.rbDetailOnly.TabIndex = 1;
            this.rbDetailOnly.Text = "Detail Only";
            this.rbDetailOnly.UseVisualStyleBackColor = true;
            // 
            // rbHeaderDetail
            // 
            this.rbHeaderDetail.AutoSize = true;
            this.rbHeaderDetail.Checked = true;
            this.rbHeaderDetail.Location = new System.Drawing.Point(7, 9);
            this.rbHeaderDetail.Name = "rbHeaderDetail";
            this.rbHeaderDetail.Size = new System.Drawing.Size(102, 19);
            this.rbHeaderDetail.TabIndex = 0;
            this.rbHeaderDetail.TabStop = true;
            this.rbHeaderDetail.Text = "Header-Detail";
            this.rbHeaderDetail.UseVisualStyleBackColor = true;
            // 
            // panelViewTransactions
            // 
            this.panelViewTransactions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelViewTransactions.Controls.Add(this.splitContainerTrx);
            this.panelViewTransactions.Controls.Add(this.grpCriteria);
            this.panelViewTransactions.Location = new System.Drawing.Point(0, 0);
            this.panelViewTransactions.Name = "panelViewTransactions";
            this.panelViewTransactions.Size = new System.Drawing.Size(1088, 534);
            this.panelViewTransactions.TabIndex = 13;
            // 
            // grpCriteria
            // 
            this.grpCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCriteria.Controls.Add(this.label6);
            this.grpCriteria.Controls.Add(this.cmbTrxStatus);
            this.grpCriteria.Controls.Add(this.txtTrxId);
            this.grpCriteria.Controls.Add(this.label5);
            this.grpCriteria.Controls.Add(this.btnCancel);
            this.grpCriteria.Controls.Add(this.groupBox1);
            this.grpCriteria.Controls.Add(this.btnExcel);
            this.grpCriteria.Controls.Add(this.cmbUser);
            this.grpCriteria.Controls.Add(this.label4);
            this.grpCriteria.Controls.Add(this.label3);
            this.grpCriteria.Controls.Add(this.dtpFrom);
            this.grpCriteria.Controls.Add(this.cmbPOS);
            this.grpCriteria.Controls.Add(this.btnRefresh);
            this.grpCriteria.Controls.Add(this.dtpTo);
            this.grpCriteria.Controls.Add(this.label2);
            this.grpCriteria.Controls.Add(this.label1);
            this.grpCriteria.Location = new System.Drawing.Point(9, 4);
            this.grpCriteria.Name = "grpCriteria";
            this.grpCriteria.Size = new System.Drawing.Size(1076, 74);
            this.grpCriteria.TabIndex = 13;
            this.grpCriteria.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(512, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "Status:";
            // 
            // cmbTrxStatus
            // 
            this.cmbTrxStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTrxStatus.FormattingEnabled = true;
            this.cmbTrxStatus.Items.AddRange(new object[] {
            "",
            "INITIATED",
            "CLOSED",
            "OPEN",
            "ORDERED",
            "PREPARED",
            "CANCELLED",
            "PENDING"});
            this.cmbTrxStatus.Location = new System.Drawing.Point(561, 44);
            this.cmbTrxStatus.Name = "cmbTrxStatus";
            this.cmbTrxStatus.Size = new System.Drawing.Size(120, 23);
            this.cmbTrxStatus.TabIndex = 16;
            // 
            // txtTrxId
            // 
            this.txtTrxId.Location = new System.Drawing.Point(561, 17);
            this.txtTrxId.Name = "txtTrxId";
            this.txtTrxId.Size = new System.Drawing.Size(120, 21);
            this.txtTrxId.TabIndex = 14;
            this.txtTrxId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTrxId_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(517, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "Trx Id:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(712, 17);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 50);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "X";
            this.dataGridViewImageColumn1.Image = global::Semnox.Parafait.Transaction.Properties.Resources.cancel;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.ToolTipText = "Reverse Transaction";
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.HeaderText = "P";
            this.dataGridViewImageColumn2.Image = global::Semnox.Parafait.Transaction.Properties.Resources.printer;
            this.dataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn2.ToolTipText = "Print Transaction";
            // 
            // dataGridViewImageColumn3
            // 
            this.dataGridViewImageColumn3.HeaderText = "X";
            this.dataGridViewImageColumn3.Image = global::Semnox.Parafait.Transaction.Properties.Resources.cancel;
            this.dataGridViewImageColumn3.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn3.Name = "dataGridViewImageColumn3";
            this.dataGridViewImageColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn3.ToolTipText = "Reverse Line";
            // 
            // dataGridViewImageColumn4
            // 
            this.dataGridViewImageColumn4.HeaderText = "X";
            this.dataGridViewImageColumn4.Image = global::Semnox.Parafait.Transaction.Properties.Resources.cancelIcon;
            this.dataGridViewImageColumn4.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn4.Name = "dataGridViewImageColumn4";
            this.dataGridViewImageColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn4.ToolTipText = "Reverse Line";
            // 
            // verticalScrollBarView1
            // 
            this.verticalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView1.AutoHide = false;
            this.verticalScrollBarView1.DataGridView = this.dgvTrxHeader;
            this.verticalScrollBarView1.Location = new System.Drawing.Point(1026, 20);
            this.verticalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView1.Name = "verticalScrollBarView1";
            this.verticalScrollBarView1.ScrollableControl = null;
            this.verticalScrollBarView1.Size = new System.Drawing.Size(40, 176);
            this.verticalScrollBarView1.TabIndex = 1;
            // 
            // horizontalScrollBarView1
            // 
            this.horizontalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBarView1.AutoHide = false;
            this.horizontalScrollBarView1.DataGridView = this.dgvTrxHeader;
            this.horizontalScrollBarView1.Location = new System.Drawing.Point(6, 199);
            this.horizontalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarView1.Name = "horizontalScrollBarView1";
            this.horizontalScrollBarView1.ScrollableControl = null;
            this.horizontalScrollBarView1.Size = new System.Drawing.Size(1015, 40);
            this.horizontalScrollBarView1.TabIndex = 2;
            // 
            // verticalScrollBarView2
            // 
            this.verticalScrollBarView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView2.AutoHide = false;
            this.verticalScrollBarView2.DataGridView = this.dgvTrxDetails;
            this.verticalScrollBarView2.Location = new System.Drawing.Point(1026, 20);
            this.verticalScrollBarView2.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView2.Name = "verticalScrollBarView2";
            this.verticalScrollBarView2.ScrollableControl = null;
            this.verticalScrollBarView2.Size = new System.Drawing.Size(40, 111);
            this.verticalScrollBarView2.TabIndex = 2;
            // 
            // horizontalScrollBarView2
            // 
            this.horizontalScrollBarView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBarView2.AutoHide = false;
            this.horizontalScrollBarView2.DataGridView = this.dgvTrxDetails;
            this.horizontalScrollBarView2.Location = new System.Drawing.Point(6, 142);
            this.horizontalScrollBarView2.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarView2.Name = "horizontalScrollBarView2";
            this.horizontalScrollBarView2.ScrollableControl = null;
            this.horizontalScrollBarView2.Size = new System.Drawing.Size(1015, 40);
            this.horizontalScrollBarView2.TabIndex = 3;
            // 
            // ViewTransactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 536);
            this.Controls.Add(this.panelViewTransactions);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ViewTransactions";
            this.Text = "View Transactions";
            this.Load += new System.EventHandler(this.ViewTransactions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxDetails)).EndInit();
            this.grpHeader.ResumeLayout(false);
            this.filterPanel.ResumeLayout(false);
            this.grpDetails.ResumeLayout(false);
            this.splitContainerTrx.Panel1.ResumeLayout(false);
            this.splitContainerTrx.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTrx)).EndInit();
            this.splitContainerTrx.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelViewTransactions.ResumeLayout(false);
            this.grpCriteria.ResumeLayout(false);
            this.grpCriteria.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTrxHeader;
        private System.Windows.Forms.DataGridView dgvTrxDetails;
        private System.Windows.Forms.GroupBox grpHeader;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.SplitContainer splitContainerTrx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.ComboBox cmbPOS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbDetailOnly;
        private System.Windows.Forms.RadioButton rbHeaderDetail;
        private System.Windows.Forms.Panel panelViewTransactions;
        private System.Windows.Forms.GroupBox grpCriteria;
        private System.Windows.Forms.TextBox txtTrxId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbTrxStatus;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn4;
        private System.Windows.Forms.DataGridViewImageColumn btnCancelTrx;
        private System.Windows.Forms.DataGridViewImageColumn btnPrint;
        private System.Windows.Forms.DataGridViewImageColumn btnTrxReopen;
        private System.Windows.Forms.DataGridViewImageColumn btnCancelLine;
        private Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView1;
        private Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarView1;
        private Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarView2;
        private Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView2;

        private System.Windows.Forms.FlowLayoutPanel filterPanel;
        private System.Windows.Forms.RadioButton rdToday;
        private System.Windows.Forms.RadioButton rdPast;
        private System.Windows.Forms.RadioButton rdFuture3;
        private System.Windows.Forms.RadioButton rdFutureAll;
    }
}