namespace Semnox.Parafait.Report.Reports
{

    /// <summary>
    /// GenericReportViewer class
    /// </summary>
    partial class GenericReportViewer
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
            this.reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.dtpTimeTo = new System.Windows.Forms.DateTimePicker();
            this.dtpTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.labelFromDate = new System.Windows.Forms.Label();
            this.labelToDate = new System.Windows.Forms.Label();
            this.CalFromDate = new System.Windows.Forms.DateTimePicker();
            this.CalToDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlUsers = new System.Windows.Forms.Panel();
            this.cmbUsers = new Telerik.WinControls.UI.RadCheckedDropDownList();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlCategory = new System.Windows.Forms.Panel();
            this.cmbCategory = new Telerik.WinControls.UI.RadCheckedDropDownList();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlLocation = new System.Windows.Forms.Panel();
            this.cmbLocation = new Telerik.WinControls.UI.RadCheckedDropDownList();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlVendor = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlTechnicianCards = new System.Windows.Forms.Panel();
            this.cmbTechnicianCards = new Telerik.WinControls.UI.RadCheckedDropDownList();
            this.Cards = new System.Windows.Forms.Label();
            this.pnlStockUsage = new System.Windows.Forms.Panel();
            this.radCheckedDropDownList1 = new Telerik.WinControls.UI.RadCheckedDropDownList();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnEmailReport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmbVendor = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsers)).BeginInit();
            this.pnlCategory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCategory)).BeginInit();
            this.pnlLocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocation)).BeginInit();
            this.pnlVendor.SuspendLayout();
            this.pnlTechnicianCards.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTechnicianCards)).BeginInit();
            this.pnlStockUsage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckedDropDownList1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // reportViewer
            // 
            this.reportViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer.Location = new System.Drawing.Point(3, 241);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.ParametersAreaVisible = false;
            this.reportViewer.ShowRefreshButton = false;
            this.reportViewer.Size = new System.Drawing.Size(1225, 308);
            this.reportViewer.TabIndex = 46;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.flowLayoutPanel2);
            this.groupBox1.Controls.Add(this.dtpTimeTo);
            this.groupBox1.Controls.Add(this.dtpTimeFrom);
            this.groupBox1.Controls.Add(this.labelFromDate);
            this.groupBox1.Controls.Add(this.labelToDate);
            this.groupBox1.Controls.Add(this.CalFromDate);
            this.groupBox1.Controls.Add(this.CalToDate);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1222, 55);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(36, 40);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanel2.TabIndex = 48;
            // 
            // dtpTimeTo
            // 
            this.dtpTimeTo.CustomFormat = "h:mm tt";
            this.dtpTimeTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dtpTimeTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeTo.Location = new System.Drawing.Point(662, 13);
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
            this.dtpTimeFrom.Location = new System.Drawing.Point(289, 13);
            this.dtpTimeFrom.Name = "dtpTimeFrom";
            this.dtpTimeFrom.ShowUpDown = true;
            this.dtpTimeFrom.Size = new System.Drawing.Size(74, 21);
            this.dtpTimeFrom.TabIndex = 25;
            // 
            // labelFromDate
            // 
            this.labelFromDate.AutoSize = true;
            this.labelFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.labelFromDate.Location = new System.Drawing.Point(30, 16);
            this.labelFromDate.Name = "labelFromDate";
            this.labelFromDate.Size = new System.Drawing.Size(68, 15);
            this.labelFromDate.TabIndex = 23;
            this.labelFromDate.Text = "From Date:";
            // 
            // labelToDate
            // 
            this.labelToDate.AutoSize = true;
            this.labelToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.labelToDate.Location = new System.Drawing.Point(418, 16);
            this.labelToDate.Name = "labelToDate";
            this.labelToDate.Size = new System.Drawing.Size(52, 15);
            this.labelToDate.TabIndex = 24;
            this.labelToDate.Text = "To Date:";
            // 
            // CalFromDate
            // 
            this.CalFromDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.CalFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.CalFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CalFromDate.Location = new System.Drawing.Point(101, 13);
            this.CalFromDate.Name = "CalFromDate";
            this.CalFromDate.Size = new System.Drawing.Size(183, 21);
            this.CalFromDate.TabIndex = 21;
            this.CalFromDate.TabStop = false;
            // 
            // CalToDate
            // 
            this.CalToDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.CalToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.CalToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CalToDate.Location = new System.Drawing.Point(474, 13);
            this.CalToDate.Name = "CalToDate";
            this.CalToDate.Size = new System.Drawing.Size(183, 21);
            this.CalToDate.TabIndex = 22;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.flowLayoutPanel1);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(12, 73);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1222, 168);
            this.groupBox5.TabIndex = 47;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Parameters";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.pnlUsers);
            this.flowLayoutPanel1.Controls.Add(this.pnlCategory);
            this.flowLayoutPanel1.Controls.Add(this.pnlLocation);
            this.flowLayoutPanel1.Controls.Add(this.pnlVendor);
            this.flowLayoutPanel1.Controls.Add(this.pnlTechnicianCards);
            this.flowLayoutPanel1.Controls.Add(this.pnlStockUsage);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1010, 143);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // pnlUsers
            // 
            this.pnlUsers.Controls.Add(this.cmbUsers);
            this.pnlUsers.Controls.Add(this.label2);
            this.pnlUsers.Location = new System.Drawing.Point(3, 3);
            this.pnlUsers.Name = "pnlUsers";
            this.pnlUsers.Size = new System.Drawing.Size(245, 38);
            this.pnlUsers.TabIndex = 0;
            this.pnlUsers.Visible = false;
            // 
            // cmbUsers
            // 
            this.cmbUsers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cmbUsers.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cmbUsers.Location = new System.Drawing.Point(61, 9);
            this.cmbUsers.Name = "cmbUsers";
            // 
            // 
            // 
            this.cmbUsers.RootElement.ControlBounds = new System.Drawing.Rectangle(61, 9, 125, 20);
            this.cmbUsers.RootElement.StretchVertically = true;
            this.cmbUsers.ShowCheckAllItems = true;
            this.cmbUsers.ShowImageInEditorArea = false;
            this.cmbUsers.Size = new System.Drawing.Size(168, 20);
            this.cmbUsers.TabIndex = 42;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(11, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 15);
            this.label2.TabIndex = 41;
            this.label2.Text = "Users:";
            // 
            // pnlCategory
            // 
            this.pnlCategory.Controls.Add(this.cmbCategory);
            this.pnlCategory.Controls.Add(this.label4);
            this.pnlCategory.Location = new System.Drawing.Point(254, 3);
            this.pnlCategory.Name = "pnlCategory";
            this.pnlCategory.Size = new System.Drawing.Size(264, 38);
            this.pnlCategory.TabIndex = 1;
            this.pnlCategory.Visible = false;
            // 
            // cmbCategory
            // 
            this.cmbCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cmbCategory.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cmbCategory.Location = new System.Drawing.Point(76, 9);
            this.cmbCategory.Name = "cmbCategory";
            // 
            // 
            // 
            this.cmbCategory.RootElement.ControlBounds = new System.Drawing.Rectangle(76, 9, 125, 20);
            this.cmbCategory.RootElement.StretchVertically = true;
            this.cmbCategory.ShowCheckAllItems = true;
            this.cmbCategory.ShowImageInEditorArea = false;
            this.cmbCategory.Size = new System.Drawing.Size(168, 20);
            this.cmbCategory.TabIndex = 42;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(11, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 15);
            this.label4.TabIndex = 41;
            this.label4.Text = "Category:";
            // 
            // pnlLocation
            // 
            this.pnlLocation.Controls.Add(this.cmbLocation);
            this.pnlLocation.Controls.Add(this.label5);
            this.pnlLocation.Location = new System.Drawing.Point(524, 3);
            this.pnlLocation.Name = "pnlLocation";
            this.pnlLocation.Size = new System.Drawing.Size(255, 38);
            this.pnlLocation.TabIndex = 2;
            this.pnlLocation.Visible = false;
            // 
            // cmbLocation
            // 
            this.cmbLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cmbLocation.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cmbLocation.Location = new System.Drawing.Point(76, 9);
            this.cmbLocation.Name = "cmbLocation";
            // 
            // 
            // 
            this.cmbLocation.RootElement.ControlBounds = new System.Drawing.Rectangle(76, 9, 125, 20);
            this.cmbLocation.RootElement.StretchVertically = true;
            this.cmbLocation.ShowCheckAllItems = true;
            this.cmbLocation.ShowImageInEditorArea = false;
            this.cmbLocation.Size = new System.Drawing.Size(168, 20);
            this.cmbLocation.TabIndex = 42;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(11, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 15);
            this.label5.TabIndex = 41;
            this.label5.Text = "Location:";
            // 
            // pnlVendor
            // 
            this.pnlVendor.Controls.Add(this.cmbVendor);
            this.pnlVendor.Controls.Add(this.label1);
            this.pnlVendor.Location = new System.Drawing.Point(3, 47);
            this.pnlVendor.Name = "pnlVendor";
            this.pnlVendor.Size = new System.Drawing.Size(255, 38);
            this.pnlVendor.TabIndex = 48;
            this.pnlVendor.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(11, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 15);
            this.label1.TabIndex = 41;
            this.label1.Text = "Vendor:";
            // 
            // pnlTechnicianCards
            // 
            this.pnlTechnicianCards.Controls.Add(this.cmbTechnicianCards);
            this.pnlTechnicianCards.Controls.Add(this.Cards);
            this.pnlTechnicianCards.Location = new System.Drawing.Point(264, 47);
            this.pnlTechnicianCards.Name = "pnlTechnicianCards";
            this.pnlTechnicianCards.Size = new System.Drawing.Size(255, 38);
            this.pnlTechnicianCards.TabIndex = 49;
            this.pnlTechnicianCards.Visible = false;
            // 
            // cmbTechnicianCards
            // 
            this.cmbTechnicianCards.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cmbTechnicianCards.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cmbTechnicianCards.Location = new System.Drawing.Point(76, 9);
            this.cmbTechnicianCards.Name = "cmbTechnicianCards";
            // 
            // 
            // 
            this.cmbTechnicianCards.RootElement.ControlBounds = new System.Drawing.Rectangle(76, 9, 125, 20);
            this.cmbTechnicianCards.RootElement.StretchVertically = true;
            this.cmbTechnicianCards.ShowCheckAllItems = true;
            this.cmbTechnicianCards.ShowImageInEditorArea = false;
            this.cmbTechnicianCards.Size = new System.Drawing.Size(168, 20);
            this.cmbTechnicianCards.TabIndex = 42;
            // 
            // Cards
            // 
            this.Cards.AutoSize = true;
            this.Cards.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Cards.Location = new System.Drawing.Point(11, 8);
            this.Cards.Name = "Cards";
            this.Cards.Size = new System.Drawing.Size(44, 15);
            this.Cards.TabIndex = 41;
            this.Cards.Text = "Cards:";
            // 
            // pnlStockUsage
            // 
            this.pnlStockUsage.Controls.Add(this.radCheckedDropDownList1);
            this.pnlStockUsage.Controls.Add(this.label3);
            this.pnlStockUsage.Location = new System.Drawing.Point(525, 47);
            this.pnlStockUsage.Name = "pnlStockUsage";
            this.pnlStockUsage.Size = new System.Drawing.Size(255, 38);
            this.pnlStockUsage.TabIndex = 50;
            this.pnlStockUsage.Visible = false;
            // 
            // radCheckedDropDownList1
            // 
            this.radCheckedDropDownList1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.radCheckedDropDownList1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.radCheckedDropDownList1.Location = new System.Drawing.Point(76, 9);
            this.radCheckedDropDownList1.Name = "radCheckedDropDownList1";
            // 
            // 
            // 
            this.radCheckedDropDownList1.RootElement.ControlBounds = new System.Drawing.Rectangle(76, 9, 125, 20);
            this.radCheckedDropDownList1.RootElement.StretchVertically = true;
            this.radCheckedDropDownList1.ShowCheckAllItems = true;
            this.radCheckedDropDownList1.ShowImageInEditorArea = false;
            this.radCheckedDropDownList1.Size = new System.Drawing.Size(168, 20);
            this.radCheckedDropDownList1.TabIndex = 42;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(11, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 15);
            this.label3.TabIndex = 41;
            this.label3.Text = "Cards:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnGo);
            this.panel1.Controls.Add(this.btnEmailReport);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Location = new System.Drawing.Point(3, 91);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1033, 44);
            this.panel1.TabIndex = 47;
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGo.Location = new System.Drawing.Point(8, 8);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(101, 31);
            this.btnGo.TabIndex = 8;
            this.btnGo.Text = "Refresh";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnEmailReport
            // 
            this.btnEmailReport.Location = new System.Drawing.Point(123, 8);
            this.btnEmailReport.Name = "btnEmailReport";
            this.btnEmailReport.Size = new System.Drawing.Size(101, 31);
            this.btnEmailReport.TabIndex = 39;
            this.btnEmailReport.Text = "Email Report";
            this.btnEmailReport.UseVisualStyleBackColor = true;
            this.btnEmailReport.Click += new System.EventHandler(this.btnEmailReport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(238, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 31);
            this.btnClose.TabIndex = 38;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmbVendor
            // 
            this.cmbVendor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbVendor.FormattingEnabled = true;
            this.cmbVendor.Location = new System.Drawing.Point(90, 8);
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.Size = new System.Drawing.Size(140, 21);
            this.cmbVendor.TabIndex = 42;
            // 
            // GenericReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 561);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.reportViewer);
            this.Controls.Add(this.groupBox1);
            this.Name = "GenericReportViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "GenericReportViewer";
            this.Load += new System.EventHandler(this.GenericReportViewer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.pnlUsers.ResumeLayout(false);
            this.pnlUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsers)).EndInit();
            this.pnlCategory.ResumeLayout(false);
            this.pnlCategory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCategory)).EndInit();
            this.pnlLocation.ResumeLayout(false);
            this.pnlLocation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocation)).EndInit();
            this.pnlVendor.ResumeLayout(false);
            this.pnlVendor.PerformLayout();
            this.pnlTechnicianCards.ResumeLayout(false);
            this.pnlTechnicianCards.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTechnicianCards)).EndInit();
            this.pnlStockUsage.ResumeLayout(false);
            this.pnlStockUsage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckedDropDownList1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.ReportViewer.WinForms.ReportViewer reportViewer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpTimeTo;
        private System.Windows.Forms.DateTimePicker dtpTimeFrom;
        private System.Windows.Forms.Label labelFromDate;
        private System.Windows.Forms.Label labelToDate;
        private System.Windows.Forms.DateTimePicker CalFromDate;
        private System.Windows.Forms.DateTimePicker CalToDate;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel pnlUsers;
        private Telerik.WinControls.UI.RadCheckedDropDownList cmbUsers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlCategory;
        private Telerik.WinControls.UI.RadCheckedDropDownList cmbCategory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlLocation;
        private Telerik.WinControls.UI.RadCheckedDropDownList cmbLocation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlTechnicianCards;
        private Telerik.WinControls.UI.RadCheckedDropDownList cmbTechnicianCards;
        private System.Windows.Forms.Label Cards;
        private System.Windows.Forms.Panel pnlStockUsage;
        private Telerik.WinControls.UI.RadCheckedDropDownList radCheckedDropDownList1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnEmailReport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlVendor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbVendor;
    }
}