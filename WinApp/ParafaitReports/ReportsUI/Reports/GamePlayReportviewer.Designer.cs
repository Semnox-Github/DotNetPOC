namespace Semnox.Parafait.Report.Reports
{
    /// <summary>
    /// GamePlayReportviewer Class
    /// </summary>
    partial class GamePlayReportviewer
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbGameProfile = new Telerik.WinControls.UI.RadCheckedDropDownList();
            this.label3 = new System.Windows.Forms.Label();
            this.grpGamemetric = new System.Windows.Forms.GroupBox();
            this.chkGroupByGameProfile = new System.Windows.Forms.CheckBox();
            this.drpSortBy = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpGamePerformance = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkIncludeCardGame = new System.Windows.Forms.CheckBox();
            this.chkIncludeTime = new System.Windows.Forms.CheckBox();
            this.chkIncludeBonus = new System.Windows.Forms.CheckBox();
            this.chkIncludeCourstesy = new System.Windows.Forms.CheckBox();
            this.chkIncludeCredits = new System.Windows.Forms.CheckBox();
            this.cmbMachines = new Telerik.WinControls.UI.RadCheckedDropDownList();
            this.rbDaily = new System.Windows.Forms.RadioButton();
            this.rbMonthly = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.gbTrafficReport = new System.Windows.Forms.GroupBox();
            this.rbTrafficMonthly = new System.Windows.Forms.RadioButton();
            this.rbTrafficWeekly = new System.Windows.Forms.RadioButton();
            this.rbTrafficDaily = new System.Windows.Forms.RadioButton();
            this.rbTrafficHourly = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnChart = new System.Windows.Forms.Button();
            this.btnEmailReport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dtpTimeTo = new System.Windows.Forms.DateTimePicker();
            this.dtpTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.labelToDate = new System.Windows.Forms.Label();
            this.labelFromDate = new System.Windows.Forms.Label();
            this.CalToDate = new System.Windows.Forms.DateTimePicker();
            this.CalFromDate = new System.Windows.Forms.DateTimePicker();
            this.reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGameProfile)).BeginInit();
            this.grpGamemetric.SuspendLayout();
            this.grpGamePerformance.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMachines)).BeginInit();
            this.gbTrafficReport.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Controls.Add(this.dtpTimeTo);
            this.groupBox1.Controls.Add(this.dtpTimeFrom);
            this.groupBox1.Controls.Add(this.labelToDate);
            this.groupBox1.Controls.Add(this.labelFromDate);
            this.groupBox1.Controls.Add(this.CalToDate);
            this.groupBox1.Controls.Add(this.CalFromDate);
            this.groupBox1.Location = new System.Drawing.Point(2, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1231, 207);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.groupBox3);
            this.flowLayoutPanel1.Controls.Add(this.grpGamemetric);
            this.flowLayoutPanel1.Controls.Add(this.grpGamePerformance);
            this.flowLayoutPanel1.Controls.Add(this.gbTrafficReport);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 40);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1209, 161);
            this.flowLayoutPanel1.TabIndex = 25;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmbGameProfile);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(260, 42);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            // 
            // cmbGameProfile
            // 
            this.cmbGameProfile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cmbGameProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cmbGameProfile.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbGameProfile.Location = new System.Drawing.Point(95, 13);
            this.cmbGameProfile.Name = "cmbGameProfile";
            this.cmbGameProfile.ShowCheckAllItems = true;
            this.cmbGameProfile.ShowImageInEditorArea = false;
            this.cmbGameProfile.Size = new System.Drawing.Size(152, 20);
            this.cmbGameProfile.TabIndex = 40;
            this.cmbGameProfile.ItemCheckedChanged += new Telerik.WinControls.UI.RadCheckedListDataItemEventHandler(this.cmbGameProfile_ItemCheckedChanged);
            ((Telerik.WinControls.UI.RadCheckedDropDownListElement)(this.cmbGameProfile.GetChildAt(0))).DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
            ((Telerik.WinControls.UI.RadCheckedDropDownListElement)(this.cmbGameProfile.GetChildAt(0))).Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(6, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 15);
            this.label3.TabIndex = 23;
            this.label3.Text = "Game Profile:";
            // 
            // grpGamemetric
            // 
            this.grpGamemetric.Controls.Add(this.chkGroupByGameProfile);
            this.grpGamemetric.Controls.Add(this.drpSortBy);
            this.grpGamemetric.Controls.Add(this.label1);
            this.grpGamemetric.Location = new System.Drawing.Point(269, 3);
            this.grpGamemetric.Name = "grpGamemetric";
            this.grpGamemetric.Size = new System.Drawing.Size(213, 75);
            this.grpGamemetric.TabIndex = 16;
            this.grpGamemetric.TabStop = false;
            // 
            // chkGroupByGameProfile
            // 
            this.chkGroupByGameProfile.AutoSize = true;
            this.chkGroupByGameProfile.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkGroupByGameProfile.Checked = true;
            this.chkGroupByGameProfile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGroupByGameProfile.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chkGroupByGameProfile.Location = new System.Drawing.Point(47, 13);
            this.chkGroupByGameProfile.Name = "chkGroupByGameProfile";
            this.chkGroupByGameProfile.Size = new System.Drawing.Size(153, 19);
            this.chkGroupByGameProfile.TabIndex = 13;
            this.chkGroupByGameProfile.Text = "Group By Game Profile";
            this.chkGroupByGameProfile.UseVisualStyleBackColor = true;
            // 
            // drpSortBy
            // 
            this.drpSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpSortBy.FormattingEnabled = true;
            this.drpSortBy.Location = new System.Drawing.Point(60, 38);
            this.drpSortBy.Name = "drpSortBy";
            this.drpSortBy.Size = new System.Drawing.Size(140, 21);
            this.drpSortBy.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(8, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Sort By:";
            // 
            // grpGamePerformance
            // 
            this.grpGamePerformance.Controls.Add(this.panel1);
            this.grpGamePerformance.Controls.Add(this.cmbMachines);
            this.grpGamePerformance.Controls.Add(this.rbDaily);
            this.grpGamePerformance.Controls.Add(this.rbMonthly);
            this.grpGamePerformance.Controls.Add(this.label2);
            this.grpGamePerformance.Location = new System.Drawing.Point(488, 3);
            this.grpGamePerformance.Name = "grpGamePerformance";
            this.grpGamePerformance.Size = new System.Drawing.Size(401, 108);
            this.grpGamePerformance.TabIndex = 17;
            this.grpGamePerformance.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkIncludeCardGame);
            this.panel1.Controls.Add(this.chkIncludeTime);
            this.panel1.Controls.Add(this.chkIncludeBonus);
            this.panel1.Controls.Add(this.chkIncludeCourstesy);
            this.panel1.Controls.Add(this.chkIncludeCredits);
            this.panel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.panel1.Location = new System.Drawing.Point(291, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(96, 85);
            this.panel1.TabIndex = 33;
            // 
            // chkIncludeCardGame
            // 
            this.chkIncludeCardGame.AutoSize = true;
            this.chkIncludeCardGame.Checked = true;
            this.chkIncludeCardGame.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeCardGame.Location = new System.Drawing.Point(3, 65);
            this.chkIncludeCardGame.Name = "chkIncludeCardGame";
            this.chkIncludeCardGame.Size = new System.Drawing.Size(86, 19);
            this.chkIncludeCardGame.TabIndex = 4;
            this.chkIncludeCardGame.Text = "CardGame";
            this.chkIncludeCardGame.UseVisualStyleBackColor = true;
            // 
            // chkIncludeTime
            // 
            this.chkIncludeTime.AutoSize = true;
            this.chkIncludeTime.Checked = true;
            this.chkIncludeTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeTime.Location = new System.Drawing.Point(3, 50);
            this.chkIncludeTime.Name = "chkIncludeTime";
            this.chkIncludeTime.Size = new System.Drawing.Size(54, 19);
            this.chkIncludeTime.TabIndex = 3;
            this.chkIncludeTime.Text = "Time";
            this.chkIncludeTime.UseVisualStyleBackColor = true;
            // 
            // chkIncludeBonus
            // 
            this.chkIncludeBonus.AutoSize = true;
            this.chkIncludeBonus.Checked = true;
            this.chkIncludeBonus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeBonus.Location = new System.Drawing.Point(3, 34);
            this.chkIncludeBonus.Name = "chkIncludeBonus";
            this.chkIncludeBonus.Size = new System.Drawing.Size(62, 19);
            this.chkIncludeBonus.TabIndex = 2;
            this.chkIncludeBonus.Text = "Bonus";
            this.chkIncludeBonus.UseVisualStyleBackColor = true;
            // 
            // chkIncludeCourstesy
            // 
            this.chkIncludeCourstesy.AutoSize = true;
            this.chkIncludeCourstesy.Checked = true;
            this.chkIncludeCourstesy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeCourstesy.Location = new System.Drawing.Point(3, 18);
            this.chkIncludeCourstesy.Name = "chkIncludeCourstesy";
            this.chkIncludeCourstesy.Size = new System.Drawing.Size(77, 19);
            this.chkIncludeCourstesy.TabIndex = 1;
            this.chkIncludeCourstesy.Text = "Courtesy";
            this.chkIncludeCourstesy.UseVisualStyleBackColor = true;
            // 
            // chkIncludeCredits
            // 
            this.chkIncludeCredits.AutoSize = true;
            this.chkIncludeCredits.Checked = true;
            this.chkIncludeCredits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeCredits.Location = new System.Drawing.Point(3, 3);
            this.chkIncludeCredits.Name = "chkIncludeCredits";
            this.chkIncludeCredits.Size = new System.Drawing.Size(67, 19);
            this.chkIncludeCredits.TabIndex = 0;
            this.chkIncludeCredits.Text = "Credits";
            this.chkIncludeCredits.UseVisualStyleBackColor = true;
            // 
            // cmbMachines
            // 
            this.cmbMachines.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cmbMachines.Location = new System.Drawing.Point(55, 13);
            this.cmbMachines.Name = "cmbMachines";
            this.cmbMachines.ShowCheckAllItems = true;
            this.cmbMachines.ShowImageInEditorArea = false;
            this.cmbMachines.Size = new System.Drawing.Size(152, 20);
            this.cmbMachines.TabIndex = 32;
            // 
            // rbDaily
            // 
            this.rbDaily.AutoSize = true;
            this.rbDaily.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rbDaily.Location = new System.Drawing.Point(223, 32);
            this.rbDaily.Name = "rbDaily";
            this.rbDaily.Size = new System.Drawing.Size(52, 19);
            this.rbDaily.TabIndex = 29;
            this.rbDaily.Text = "Daily";
            this.rbDaily.UseVisualStyleBackColor = true;
            // 
            // rbMonthly
            // 
            this.rbMonthly.AutoSize = true;
            this.rbMonthly.Checked = true;
            this.rbMonthly.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rbMonthly.Location = new System.Drawing.Point(223, 8);
            this.rbMonthly.Name = "rbMonthly";
            this.rbMonthly.Size = new System.Drawing.Size(69, 19);
            this.rbMonthly.TabIndex = 28;
            this.rbMonthly.TabStop = true;
            this.rbMonthly.Text = "Monthly";
            this.rbMonthly.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(6, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 27;
            this.label2.Text = "Game:";
            // 
            // gbTrafficReport
            // 
            this.gbTrafficReport.Controls.Add(this.rbTrafficMonthly);
            this.gbTrafficReport.Controls.Add(this.rbTrafficWeekly);
            this.gbTrafficReport.Controls.Add(this.rbTrafficDaily);
            this.gbTrafficReport.Controls.Add(this.rbTrafficHourly);
            this.gbTrafficReport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gbTrafficReport.Location = new System.Drawing.Point(895, 3);
            this.gbTrafficReport.Name = "gbTrafficReport";
            this.gbTrafficReport.Size = new System.Drawing.Size(285, 40);
            this.gbTrafficReport.TabIndex = 19;
            this.gbTrafficReport.TabStop = false;
            // 
            // rbTrafficMonthly
            // 
            this.rbTrafficMonthly.AutoSize = true;
            this.rbTrafficMonthly.Checked = true;
            this.rbTrafficMonthly.Location = new System.Drawing.Point(218, 12);
            this.rbTrafficMonthly.Name = "rbTrafficMonthly";
            this.rbTrafficMonthly.Size = new System.Drawing.Size(69, 19);
            this.rbTrafficMonthly.TabIndex = 32;
            this.rbTrafficMonthly.TabStop = true;
            this.rbTrafficMonthly.Text = "Monthly";
            this.rbTrafficMonthly.UseVisualStyleBackColor = true;
            // 
            // rbTrafficWeekly
            // 
            this.rbTrafficWeekly.AutoSize = true;
            this.rbTrafficWeekly.Checked = true;
            this.rbTrafficWeekly.Location = new System.Drawing.Point(143, 12);
            this.rbTrafficWeekly.Name = "rbTrafficWeekly";
            this.rbTrafficWeekly.Size = new System.Drawing.Size(67, 19);
            this.rbTrafficWeekly.TabIndex = 31;
            this.rbTrafficWeekly.TabStop = true;
            this.rbTrafficWeekly.Text = "Weekly";
            this.rbTrafficWeekly.UseVisualStyleBackColor = true;
            // 
            // rbTrafficDaily
            // 
            this.rbTrafficDaily.AutoSize = true;
            this.rbTrafficDaily.Checked = true;
            this.rbTrafficDaily.Location = new System.Drawing.Point(77, 12);
            this.rbTrafficDaily.Name = "rbTrafficDaily";
            this.rbTrafficDaily.Size = new System.Drawing.Size(55, 19);
            this.rbTrafficDaily.TabIndex = 30;
            this.rbTrafficDaily.TabStop = true;
            this.rbTrafficDaily.Text = "Dailly";
            this.rbTrafficDaily.UseVisualStyleBackColor = true;
            // 
            // rbTrafficHourly
            // 
            this.rbTrafficHourly.AutoSize = true;
            this.rbTrafficHourly.Checked = true;
            this.rbTrafficHourly.Location = new System.Drawing.Point(7, 12);
            this.rbTrafficHourly.Name = "rbTrafficHourly";
            this.rbTrafficHourly.Size = new System.Drawing.Size(61, 19);
            this.rbTrafficHourly.TabIndex = 29;
            this.rbTrafficHourly.TabStop = true;
            this.rbTrafficHourly.Text = "Hourly";
            this.rbTrafficHourly.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnGo);
            this.flowLayoutPanel2.Controls.Add(this.btnChart);
            this.flowLayoutPanel2.Controls.Add(this.btnEmailReport);
            this.flowLayoutPanel2.Controls.Add(this.btnClose);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 117);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(442, 37);
            this.flowLayoutPanel2.TabIndex = 18;
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGo.Location = new System.Drawing.Point(3, 3);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(87, 27);
            this.btnGo.TabIndex = 8;
            this.btnGo.Text = "Refresh";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnChart
            // 
            this.btnChart.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnChart.Location = new System.Drawing.Point(96, 3);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(88, 24);
            this.btnChart.TabIndex = 41;
            this.btnChart.Text = "Bar Chart";
            this.btnChart.UseVisualStyleBackColor = true;
            this.btnChart.Click += new System.EventHandler(this.btnChart_Click);
            // 
            // btnEmailReport
            // 
            this.btnEmailReport.Location = new System.Drawing.Point(190, 3);
            this.btnEmailReport.Name = "btnEmailReport";
            this.btnEmailReport.Size = new System.Drawing.Size(115, 27);
            this.btnEmailReport.TabIndex = 39;
            this.btnEmailReport.Text = "Email Report";
            this.btnEmailReport.UseVisualStyleBackColor = true;
            this.btnEmailReport.Click += new System.EventHandler(this.btnEmailReport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(311, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 27);
            this.btnClose.TabIndex = 38;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dtpTimeTo
            // 
            this.dtpTimeTo.CustomFormat = "h:mm tt";
            this.dtpTimeTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dtpTimeTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeTo.Location = new System.Drawing.Point(657, 14);
            this.dtpTimeTo.Name = "dtpTimeTo";
            this.dtpTimeTo.ShowUpDown = true;
            this.dtpTimeTo.Size = new System.Drawing.Size(74, 21);
            this.dtpTimeTo.TabIndex = 20;
            // 
            // dtpTimeFrom
            // 
            this.dtpTimeFrom.CustomFormat = "h:mm tt";
            this.dtpTimeFrom.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dtpTimeFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeFrom.Location = new System.Drawing.Point(284, 14);
            this.dtpTimeFrom.Name = "dtpTimeFrom";
            this.dtpTimeFrom.ShowUpDown = true;
            this.dtpTimeFrom.Size = new System.Drawing.Size(74, 21);
            this.dtpTimeFrom.TabIndex = 19;
            // 
            // labelToDate
            // 
            this.labelToDate.AutoSize = true;
            this.labelToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.labelToDate.Location = new System.Drawing.Point(413, 17);
            this.labelToDate.Name = "labelToDate";
            this.labelToDate.Size = new System.Drawing.Size(52, 15);
            this.labelToDate.TabIndex = 5;
            this.labelToDate.Text = "To Date:";
            // 
            // labelFromDate
            // 
            this.labelFromDate.AutoSize = true;
            this.labelFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.labelFromDate.Location = new System.Drawing.Point(25, 17);
            this.labelFromDate.Name = "labelFromDate";
            this.labelFromDate.Size = new System.Drawing.Size(68, 15);
            this.labelFromDate.TabIndex = 4;
            this.labelFromDate.Text = "From Date:";
            // 
            // CalToDate
            // 
            this.CalToDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.CalToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.CalToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CalToDate.Location = new System.Drawing.Point(469, 14);
            this.CalToDate.Name = "CalToDate";
            this.CalToDate.Size = new System.Drawing.Size(183, 21);
            this.CalToDate.TabIndex = 1;
            // 
            // CalFromDate
            // 
            this.CalFromDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.CalFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.CalFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CalFromDate.Location = new System.Drawing.Point(96, 14);
            this.CalFromDate.Name = "CalFromDate";
            this.CalFromDate.Size = new System.Drawing.Size(183, 21);
            this.CalFromDate.TabIndex = 0;
            this.CalFromDate.TabStop = false;
            // 
            // reportViewer
            // 
            this.reportViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer.Location = new System.Drawing.Point(2, 208);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.ParametersAreaVisible = false;
            this.reportViewer.ShowRefreshButton = false;
            this.reportViewer.Size = new System.Drawing.Size(1231, 341);
            this.reportViewer.TabIndex = 43;
            // 
            // GamePlayReportviewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1234, 561);
            this.Controls.Add(this.reportViewer);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "GamePlayReportviewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GamePlayReportviewer";
            this.Load += new System.EventHandler(this.GamePlayReportviewer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGameProfile)).EndInit();
            this.grpGamemetric.ResumeLayout(false);
            this.grpGamemetric.PerformLayout();
            this.grpGamePerformance.ResumeLayout(false);
            this.grpGamePerformance.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMachines)).EndInit();
            this.gbTrafficReport.ResumeLayout(false);
            this.gbTrafficReport.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelToDate;
        private System.Windows.Forms.Label labelFromDate;
        private System.Windows.Forms.DateTimePicker CalToDate;
        private System.Windows.Forms.DateTimePicker CalFromDate;
        private System.Windows.Forms.DateTimePicker dtpTimeTo;
        private System.Windows.Forms.DateTimePicker dtpTimeFrom;
        private Telerik.ReportViewer.WinForms.ReportViewer reportViewer;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private Telerik.WinControls.UI.RadCheckedDropDownList cmbGameProfile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grpGamemetric;
        private System.Windows.Forms.CheckBox chkGroupByGameProfile;
        private System.Windows.Forms.ComboBox drpSortBy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpGamePerformance;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkIncludeCardGame;
        private System.Windows.Forms.CheckBox chkIncludeTime;
        private System.Windows.Forms.CheckBox chkIncludeBonus;
        private System.Windows.Forms.CheckBox chkIncludeCourstesy;
        private System.Windows.Forms.CheckBox chkIncludeCredits;
        private Telerik.WinControls.UI.RadCheckedDropDownList cmbMachines;
        private System.Windows.Forms.RadioButton rbDaily;
        private System.Windows.Forms.RadioButton rbMonthly;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbTrafficReport;
        private System.Windows.Forms.RadioButton rbTrafficMonthly;
        private System.Windows.Forms.RadioButton rbTrafficWeekly;
        private System.Windows.Forms.RadioButton rbTrafficDaily;
        private System.Windows.Forms.RadioButton rbTrafficHourly;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnChart;
        private System.Windows.Forms.Button btnEmailReport;
        private System.Windows.Forms.Button btnClose;
    }
}