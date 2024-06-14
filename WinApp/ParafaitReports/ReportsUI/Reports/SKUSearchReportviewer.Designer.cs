namespace Semnox.Parafait.Report.Reports
{
    partial class SKUSearchReportviewer
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpTimeTo = new System.Windows.Forms.DateTimePicker();
            this.dtpTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.labelToDate = new System.Windows.Forms.Label();
            this.labelFromDate = new System.Windows.Forms.Label();
            this.CalToDate = new System.Windows.Forms.DateTimePicker();
            this.CalFromDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCommon = new System.Windows.Forms.Panel();
            this.cmbRecvCashCredit = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.pnlVendor = new System.Windows.Forms.Panel();
            this.cmbRecvVendor = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.pnlProductType = new System.Windows.Forms.Panel();
            this.cmbProductType = new System.Windows.Forms.ComboBox();
            this.lblPrdtype = new System.Windows.Forms.Label();
            this.pnlActive = new System.Windows.Forms.Panel();
            this.chkActiveProducts = new System.Windows.Forms.CheckBox();
            this.pnllocation = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.pnlStaus = new System.Windows.Forms.Panel();
            this.cmbPOStatus = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlCat = new System.Windows.Forms.Panel();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlPurchaseOrder = new System.Windows.Forms.Panel();
            this.cmbPOType = new System.Windows.Forms.ComboBox();
            this.lblPoType = new System.Windows.Forms.Label();
            this.cmbPOGroupBy = new System.Windows.Forms.ComboBox();
            this.lblGroupBy = new System.Windows.Forms.Label();
            this.pnlCashPo = new System.Windows.Forms.Panel();
            this.cmbCreditCashPO = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pnlPONum = new System.Windows.Forms.Panel();
            this.txtPONumber = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnAdvancedSearch = new System.Windows.Forms.Button();
            this.btnEmailReport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            this.pnlCategorySearch = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCategorySearch = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlCommon.SuspendLayout();
            this.pnlVendor.SuspendLayout();
            this.pnlProductType.SuspendLayout();
            this.pnlActive.SuspendLayout();
            this.pnllocation.SuspendLayout();
            this.pnlStaus.SuspendLayout();
            this.pnlCat.SuspendLayout();
            this.pnlPurchaseOrder.SuspendLayout();
            this.pnlCashPo.SuspendLayout();
            this.pnlPONum.SuspendLayout();
            this.pnlCategorySearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dtpTimeTo);
            this.groupBox1.Controls.Add(this.dtpTimeFrom);
            this.groupBox1.Controls.Add(this.labelToDate);
            this.groupBox1.Controls.Add(this.labelFromDate);
            this.groupBox1.Controls.Add(this.CalToDate);
            this.groupBox1.Controls.Add(this.CalFromDate);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(0, -4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1228, 235);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            // 
            // dtpTimeTo
            // 
            this.dtpTimeTo.CustomFormat = "h:mm tt";
            this.dtpTimeTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dtpTimeTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeTo.Location = new System.Drawing.Point(644, 17);
            this.dtpTimeTo.Name = "dtpTimeTo";
            this.dtpTimeTo.ShowUpDown = true;
            this.dtpTimeTo.Size = new System.Drawing.Size(74, 21);
            this.dtpTimeTo.TabIndex = 26;
            // 
            // dtpTimeFrom
            // 
            this.dtpTimeFrom.CustomFormat = "h:mm tt";
            this.dtpTimeFrom.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dtpTimeFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeFrom.Location = new System.Drawing.Point(271, 17);
            this.dtpTimeFrom.Name = "dtpTimeFrom";
            this.dtpTimeFrom.ShowUpDown = true;
            this.dtpTimeFrom.Size = new System.Drawing.Size(74, 21);
            this.dtpTimeFrom.TabIndex = 25;
            // 
            // labelToDate
            // 
            this.labelToDate.AutoSize = true;
            this.labelToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.labelToDate.Location = new System.Drawing.Point(400, 20);
            this.labelToDate.Name = "labelToDate";
            this.labelToDate.Size = new System.Drawing.Size(52, 15);
            this.labelToDate.TabIndex = 24;
            this.labelToDate.Text = "To Date:";
            // 
            // labelFromDate
            // 
            this.labelFromDate.AutoSize = true;
            this.labelFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.labelFromDate.Location = new System.Drawing.Point(12, 20);
            this.labelFromDate.Name = "labelFromDate";
            this.labelFromDate.Size = new System.Drawing.Size(68, 15);
            this.labelFromDate.TabIndex = 23;
            this.labelFromDate.Text = "From Date:";
            // 
            // CalToDate
            // 
            this.CalToDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.CalToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.CalToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CalToDate.Location = new System.Drawing.Point(456, 17);
            this.CalToDate.Name = "CalToDate";
            this.CalToDate.Size = new System.Drawing.Size(183, 21);
            this.CalToDate.TabIndex = 22;
            // 
            // CalFromDate
            // 
            this.CalFromDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.CalFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.CalFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CalFromDate.Location = new System.Drawing.Point(83, 17);
            this.CalFromDate.Name = "CalFromDate";
            this.CalFromDate.Size = new System.Drawing.Size(183, 21);
            this.CalFromDate.TabIndex = 21;
            this.CalFromDate.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.flowLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(12, 38);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1210, 150);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.pnlCommon);
            this.flowLayoutPanel1.Controls.Add(this.pnlVendor);
            this.flowLayoutPanel1.Controls.Add(this.pnlProductType);
            this.flowLayoutPanel1.Controls.Add(this.pnlActive);
            this.flowLayoutPanel1.Controls.Add(this.pnllocation);
            this.flowLayoutPanel1.Controls.Add(this.pnlStaus);
            this.flowLayoutPanel1.Controls.Add(this.pnlCat);
            this.flowLayoutPanel1.Controls.Add(this.pnlCategorySearch);
            this.flowLayoutPanel1.Controls.Add(this.pnlPurchaseOrder);
            this.flowLayoutPanel1.Controls.Add(this.pnlCashPo);
            this.flowLayoutPanel1.Controls.Add(this.pnlPONum);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1204, 133);
            this.flowLayoutPanel1.TabIndex = 25;
            // 
            // pnlCommon
            // 
            this.pnlCommon.Controls.Add(this.cmbRecvCashCredit);
            this.pnlCommon.Controls.Add(this.label14);
            this.pnlCommon.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCommon.Location = new System.Drawing.Point(3, 3);
            this.pnlCommon.Name = "pnlCommon";
            this.pnlCommon.Size = new System.Drawing.Size(290, 43);
            this.pnlCommon.TabIndex = 2;
            // 
            // cmbRecvCashCredit
            // 
            this.cmbRecvCashCredit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRecvCashCredit.FormattingEnabled = true;
            this.cmbRecvCashCredit.Items.AddRange(new object[] {
            "All",
            "Credit PO",
            "Cash PO"});
            this.cmbRecvCashCredit.Location = new System.Drawing.Point(96, 11);
            this.cmbRecvCashCredit.Name = "cmbRecvCashCredit";
            this.cmbRecvCashCredit.Size = new System.Drawing.Size(184, 21);
            this.cmbRecvCashCredit.TabIndex = 60;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(84, 13);
            this.label14.TabIndex = 59;
            this.label14.Text = "Credit/Cash PO:";
            // 
            // pnlVendor
            // 
            this.pnlVendor.Controls.Add(this.cmbRecvVendor);
            this.pnlVendor.Controls.Add(this.label15);
            this.pnlVendor.Location = new System.Drawing.Point(299, 3);
            this.pnlVendor.Name = "pnlVendor";
            this.pnlVendor.Size = new System.Drawing.Size(290, 43);
            this.pnlVendor.TabIndex = 54;
            // 
            // cmbRecvVendor
            // 
            this.cmbRecvVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRecvVendor.FormattingEnabled = true;
            this.cmbRecvVendor.Location = new System.Drawing.Point(68, 11);
            this.cmbRecvVendor.Name = "cmbRecvVendor";
            this.cmbRecvVendor.Size = new System.Drawing.Size(184, 21);
            this.cmbRecvVendor.TabIndex = 71;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(18, 14);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(44, 13);
            this.label15.TabIndex = 70;
            this.label15.Text = "Vendor:";
            // 
            // pnlProductType
            // 
            this.pnlProductType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProductType.AutoSize = true;
            this.pnlProductType.Controls.Add(this.cmbProductType);
            this.pnlProductType.Controls.Add(this.lblPrdtype);
            this.pnlProductType.Location = new System.Drawing.Point(595, 3);
            this.pnlProductType.Name = "pnlProductType";
            this.pnlProductType.Size = new System.Drawing.Size(290, 43);
            this.pnlProductType.TabIndex = 47;
            // 
            // cmbProductType
            // 
            this.cmbProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductType.FormattingEnabled = true;
            this.cmbProductType.Location = new System.Drawing.Point(103, 9);
            this.cmbProductType.Name = "cmbProductType";
            this.cmbProductType.Size = new System.Drawing.Size(184, 21);
            this.cmbProductType.TabIndex = 63;
            // 
            // lblPrdtype
            // 
            this.lblPrdtype.AutoSize = true;
            this.lblPrdtype.Location = new System.Drawing.Point(13, 14);
            this.lblPrdtype.Name = "lblPrdtype";
            this.lblPrdtype.Size = new System.Drawing.Size(74, 13);
            this.lblPrdtype.TabIndex = 58;
            this.lblPrdtype.Text = "Product Type:";
            // 
            // pnlActive
            // 
            this.pnlActive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlActive.AutoSize = true;
            this.pnlActive.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlActive.Controls.Add(this.chkActiveProducts);
            this.pnlActive.Location = new System.Drawing.Point(891, 3);
            this.pnlActive.Name = "pnlActive";
            this.pnlActive.Size = new System.Drawing.Size(144, 43);
            this.pnlActive.TabIndex = 48;
            // 
            // chkActiveProducts
            // 
            this.chkActiveProducts.AutoSize = true;
            this.chkActiveProducts.Checked = true;
            this.chkActiveProducts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActiveProducts.Location = new System.Drawing.Point(16, 11);
            this.chkActiveProducts.Name = "chkActiveProducts";
            this.chkActiveProducts.Size = new System.Drawing.Size(125, 17);
            this.chkActiveProducts.TabIndex = 63;
            this.chkActiveProducts.Text = "Active Products Only";
            this.chkActiveProducts.UseVisualStyleBackColor = true;
            // 
            // pnllocation
            // 
            this.pnllocation.Controls.Add(this.label3);
            this.pnllocation.Controls.Add(this.cmbLocation);
            this.pnllocation.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnllocation.Location = new System.Drawing.Point(3, 52);
            this.pnllocation.Name = "pnllocation";
            this.pnllocation.Size = new System.Drawing.Size(290, 29);
            this.pnllocation.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 55;
            this.label3.Text = "Location:";
            // 
            // cmbLocation
            // 
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(97, 5);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(184, 21);
            this.cmbLocation.TabIndex = 54;
            // 
            // pnlStaus
            // 
            this.pnlStaus.Controls.Add(this.cmbPOStatus);
            this.pnlStaus.Controls.Add(this.label1);
            this.pnlStaus.Location = new System.Drawing.Point(299, 52);
            this.pnlStaus.Name = "pnlStaus";
            this.pnlStaus.Size = new System.Drawing.Size(290, 29);
            this.pnlStaus.TabIndex = 49;
            // 
            // cmbPOStatus
            // 
            this.cmbPOStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPOStatus.FormattingEnabled = true;
            this.cmbPOStatus.Items.AddRange(new object[] {
            "All",
            "Open",
            "Received",
            "ShortClose",
            "Cancelled"});
            this.cmbPOStatus.Location = new System.Drawing.Point(70, 4);
            this.cmbPOStatus.Name = "cmbPOStatus";
            this.cmbPOStatus.Size = new System.Drawing.Size(184, 21);
            this.cmbPOStatus.TabIndex = 77;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 76;
            this.label1.Text = "Status:";
            // 
            // pnlCat
            // 
            this.pnlCat.Controls.Add(this.cmbCategory);
            this.pnlCat.Controls.Add(this.label4);
            this.pnlCat.Location = new System.Drawing.Point(595, 52);
            this.pnlCat.Name = "pnlCat";
            this.pnlCat.Size = new System.Drawing.Size(290, 29);
            this.pnlCat.TabIndex = 50;
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(104, 4);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(184, 21);
            this.cmbCategory.TabIndex = 58;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 59;
            this.label4.Text = "Category:";
            // 
            // pnlPurchaseOrder
            // 
            this.pnlPurchaseOrder.Controls.Add(this.cmbPOType);
            this.pnlPurchaseOrder.Controls.Add(this.lblPoType);
            this.pnlPurchaseOrder.Controls.Add(this.cmbPOGroupBy);
            this.pnlPurchaseOrder.Controls.Add(this.lblGroupBy);
            this.pnlPurchaseOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPurchaseOrder.Location = new System.Drawing.Point(3, 87);
            this.pnlPurchaseOrder.Name = "pnlPurchaseOrder";
            this.pnlPurchaseOrder.Size = new System.Drawing.Size(420, 43);
            this.pnlPurchaseOrder.TabIndex = 1;
            // 
            // cmbPOType
            // 
            this.cmbPOType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPOType.FormattingEnabled = true;
            this.cmbPOType.Location = new System.Drawing.Point(95, 10);
            this.cmbPOType.Name = "cmbPOType";
            this.cmbPOType.Size = new System.Drawing.Size(132, 21);
            this.cmbPOType.TabIndex = 79;
            // 
            // lblPoType
            // 
            this.lblPoType.AutoSize = true;
            this.lblPoType.Location = new System.Drawing.Point(37, 15);
            this.lblPoType.Name = "lblPoType";
            this.lblPoType.Size = new System.Drawing.Size(52, 13);
            this.lblPoType.TabIndex = 78;
            this.lblPoType.Text = "PO Type:";
            // 
            // cmbPOGroupBy
            // 
            this.cmbPOGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPOGroupBy.FormattingEnabled = true;
            this.cmbPOGroupBy.Items.AddRange(new object[] {
            "PO#",
            "Vendor"});
            this.cmbPOGroupBy.Location = new System.Drawing.Point(294, 10);
            this.cmbPOGroupBy.Name = "cmbPOGroupBy";
            this.cmbPOGroupBy.Size = new System.Drawing.Size(122, 21);
            this.cmbPOGroupBy.TabIndex = 73;
            // 
            // lblGroupBy
            // 
            this.lblGroupBy.AutoSize = true;
            this.lblGroupBy.Location = new System.Drawing.Point(236, 15);
            this.lblGroupBy.Name = "lblGroupBy";
            this.lblGroupBy.Size = new System.Drawing.Size(54, 13);
            this.lblGroupBy.TabIndex = 72;
            this.lblGroupBy.Text = "Group By:";
            // 
            // pnlCashPo
            // 
            this.pnlCashPo.Controls.Add(this.cmbCreditCashPO);
            this.pnlCashPo.Controls.Add(this.label10);
            this.pnlCashPo.Location = new System.Drawing.Point(429, 87);
            this.pnlCashPo.Name = "pnlCashPo";
            this.pnlCashPo.Size = new System.Drawing.Size(236, 43);
            this.pnlCashPo.TabIndex = 53;
            // 
            // cmbCreditCashPO
            // 
            this.cmbCreditCashPO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCreditCashPO.FormattingEnabled = true;
            this.cmbCreditCashPO.Items.AddRange(new object[] {
            "All",
            "Credit PO",
            "Cash PO"});
            this.cmbCreditCashPO.Location = new System.Drawing.Point(100, 11);
            this.cmbCreditCashPO.Name = "cmbCreditCashPO";
            this.cmbCreditCashPO.Size = new System.Drawing.Size(122, 21);
            this.cmbCreditCashPO.TabIndex = 69;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 13);
            this.label10.TabIndex = 68;
            this.label10.Text = "Credit/Cash PO:";
            // 
            // pnlPONum
            // 
            this.pnlPONum.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlPONum.Controls.Add(this.txtPONumber);
            this.pnlPONum.Controls.Add(this.label9);
            this.pnlPONum.Location = new System.Drawing.Point(671, 87);
            this.pnlPONum.Name = "pnlPONum";
            this.pnlPONum.Size = new System.Drawing.Size(192, 43);
            this.pnlPONum.TabIndex = 51;
            // 
            // txtPONumber
            // 
            this.txtPONumber.Location = new System.Drawing.Point(65, 12);
            this.txtPONumber.Name = "txtPONumber";
            this.txtPONumber.Size = new System.Drawing.Size(123, 20);
            this.txtPONumber.TabIndex = 79;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 78;
            this.label9.Text = "PO#:";
            // 
            // btnAdvancedSearch
            // 
            this.btnAdvancedSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdvancedSearch.Location = new System.Drawing.Point(148, 190);
            this.btnAdvancedSearch.Name = "btnAdvancedSearch";
            this.btnAdvancedSearch.Size = new System.Drawing.Size(112, 31);
            this.btnAdvancedSearch.TabIndex = 46;
            this.btnAdvancedSearch.Text = "SKU Search";
            this.btnAdvancedSearch.UseVisualStyleBackColor = true;
            this.btnAdvancedSearch.Click += new System.EventHandler(this.btnAdvancedSearch_Click);
            // 
            // btnEmailReport
            // 
            this.btnEmailReport.Location = new System.Drawing.Point(293, 190);
            this.btnEmailReport.Name = "btnEmailReport";
            this.btnEmailReport.Size = new System.Drawing.Size(134, 31);
            this.btnEmailReport.TabIndex = 39;
            this.btnEmailReport.Text = "Email Report";
            this.btnEmailReport.UseVisualStyleBackColor = true;
            this.btnEmailReport.Click += new System.EventHandler(this.btnEmailReport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(460, 190);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(105, 31);
            this.btnClose.TabIndex = 38;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGo.Location = new System.Drawing.Point(14, 190);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(101, 31);
            this.btnGo.TabIndex = 8;
            this.btnGo.Text = "Refresh";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // reportViewer
            // 
            this.reportViewer.AccessibilityKeyMap = null;
            this.reportViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer.Location = new System.Drawing.Point(2, 228);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.ParametersAreaVisible = false;
            this.reportViewer.ShowRefreshButton = false;
            this.reportViewer.Size = new System.Drawing.Size(1225, 322);
            this.reportViewer.TabIndex = 42;
            // 
            // pnlCategorySearch
            // 
            this.pnlCategorySearch.Controls.Add(this.txtCategorySearch);
            this.pnlCategorySearch.Controls.Add(this.label2);
            this.pnlCategorySearch.Location = new System.Drawing.Point(891, 52);
            this.pnlCategorySearch.Name = "pnlCategorySearch";
            this.pnlCategorySearch.Size = new System.Drawing.Size(238, 29);
            this.pnlCategorySearch.TabIndex = 55;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 59;
            this.label2.Text = "Category:";
            // 
            // txtCategorySearch
            // 
            this.txtCategorySearch.Location = new System.Drawing.Point(76, 5);
            this.txtCategorySearch.Name = "txtCategorySearch";
            this.txtCategorySearch.Size = new System.Drawing.Size(157, 20);
            this.txtCategorySearch.TabIndex = 60;
            // 
            // SKUSearchReportviewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 561);
            this.Controls.Add(this.btnAdvancedSearch);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.btnEmailReport);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.reportViewer);
            this.Controls.Add(this.groupBox1);
            this.Name = "SKUSearchReportviewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SKUSearchReportviewer";
            this.Load += new System.EventHandler(this.SKUSearchReportviewer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.pnlCommon.ResumeLayout(false);
            this.pnlCommon.PerformLayout();
            this.pnlVendor.ResumeLayout(false);
            this.pnlVendor.PerformLayout();
            this.pnlProductType.ResumeLayout(false);
            this.pnlProductType.PerformLayout();
            this.pnlActive.ResumeLayout(false);
            this.pnlActive.PerformLayout();
            this.pnllocation.ResumeLayout(false);
            this.pnllocation.PerformLayout();
            this.pnlStaus.ResumeLayout(false);
            this.pnlStaus.PerformLayout();
            this.pnlCat.ResumeLayout(false);
            this.pnlCat.PerformLayout();
            this.pnlPurchaseOrder.ResumeLayout(false);
            this.pnlPurchaseOrder.PerformLayout();
            this.pnlCashPo.ResumeLayout(false);
            this.pnlCashPo.PerformLayout();
            this.pnlPONum.ResumeLayout(false);
            this.pnlPONum.PerformLayout();
            this.pnlCategorySearch.ResumeLayout(false);
            this.pnlCategorySearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnEmailReport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnAdvancedSearch;
        private System.Windows.Forms.Label lblPrdtype;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbLocation;
        private Telerik.ReportViewer.WinForms.ReportViewer reportViewer;
        private System.Windows.Forms.Panel pnlPurchaseOrder;
        private System.Windows.Forms.ComboBox cmbProductType;
        private System.Windows.Forms.ComboBox cmbPOGroupBy;
        private System.Windows.Forms.Label lblGroupBy;
        private System.Windows.Forms.Panel pnlCommon;
        private System.Windows.Forms.ComboBox cmbRecvCashCredit;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel pnlProductType;
        private System.Windows.Forms.DateTimePicker dtpTimeTo;
        private System.Windows.Forms.DateTimePicker dtpTimeFrom;
        private System.Windows.Forms.Label labelToDate;
        private System.Windows.Forms.Label labelFromDate;
        private System.Windows.Forms.DateTimePicker CalToDate;
        private System.Windows.Forms.DateTimePicker CalFromDate;
        private System.Windows.Forms.Panel pnllocation;
        private System.Windows.Forms.Panel pnlActive;
        private System.Windows.Forms.CheckBox chkActiveProducts;
        private System.Windows.Forms.Panel pnlStaus;
        private System.Windows.Forms.ComboBox cmbPOStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPOType;
        private System.Windows.Forms.Label lblPoType;
        private System.Windows.Forms.Panel pnlCat;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlPONum;
        private System.Windows.Forms.TextBox txtPONumber;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel pnlCashPo;
        private System.Windows.Forms.ComboBox cmbCreditCashPO;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel pnlVendor;
        private System.Windows.Forms.ComboBox cmbRecvVendor;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel pnlCategorySearch;
        private System.Windows.Forms.TextBox txtCategorySearch;
        private System.Windows.Forms.Label label2;
    }
}