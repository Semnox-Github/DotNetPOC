namespace Semnox.Parafait.Report.Reports
{
    /// <summary>
    ///  TransactionReportviewer Class
    /// </summary>
    partial class TransactionReportviewer
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
            this.pnlUser = new System.Windows.Forms.Panel();
            this.cmbUsers = new Telerik.WinControls.UI.RadCheckedDropDownList();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlReportType = new System.Windows.Forms.Panel();
            this.cmbReportType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlPOSSelection = new System.Windows.Forms.Panel();
            this.cmbPOS = new Telerik.WinControls.UI.RadCheckedDropDownList();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnEmailReport = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.dtpTimeTo = new System.Windows.Forms.DateTimePicker();
            this.dtpTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.labelFromDate = new System.Windows.Forms.Label();
            this.labelToDate = new System.Windows.Forms.Label();
            this.CalFromDate = new System.Windows.Forms.DateTimePicker();
            this.CalToDate = new System.Windows.Forms.DateTimePicker();
            this.reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsers)).BeginInit();
            this.pnlReportType.SuspendLayout();
            this.pnlPOSSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPOS)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Controls.Add(this.dtpTimeTo);
            this.groupBox1.Controls.Add(this.dtpTimeFrom);
            this.groupBox1.Controls.Add(this.labelFromDate);
            this.groupBox1.Controls.Add(this.labelToDate);
            this.groupBox1.Controls.Add(this.CalFromDate);
            this.groupBox1.Controls.Add(this.CalToDate);
            this.groupBox1.Location = new System.Drawing.Point(1, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1229, 134);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.pnlUser);
            this.flowLayoutPanel1.Controls.Add(this.pnlReportType);
            this.flowLayoutPanel1.Controls.Add(this.pnlPOSSelection);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(11, 40);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1210, 91);
            this.flowLayoutPanel1.TabIndex = 25;
            // 
            // pnlUser
            // 
            this.pnlUser.Controls.Add(this.cmbUsers);
            this.pnlUser.Controls.Add(this.label2);
            this.pnlUser.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.pnlUser.Location = new System.Drawing.Point(3, 3);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(332, 33);
            this.pnlUser.TabIndex = 59;
            // 
            // cmbUsers
            // 
            this.cmbUsers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cmbUsers.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cmbUsers.Location = new System.Drawing.Point(70, 4);
            this.cmbUsers.Name = "cmbUsers";
            // 
            // 
            // 
            this.cmbUsers.RootElement.ControlBounds = new System.Drawing.Rectangle(70, 4, 125, 20);
            this.cmbUsers.RootElement.StretchVertically = true;
            this.cmbUsers.ShowCheckAllItems = true;
            this.cmbUsers.ShowImageInEditorArea = false;
            this.cmbUsers.Size = new System.Drawing.Size(188, 20);
            this.cmbUsers.TabIndex = 58;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(30, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 57;
            this.label2.Text = "User:";
            // 
            // pnlReportType
            // 
            this.pnlReportType.Controls.Add(this.cmbReportType);
            this.pnlReportType.Controls.Add(this.label1);
            this.pnlReportType.Location = new System.Drawing.Point(341, 3);
            this.pnlReportType.Name = "pnlReportType";
            this.pnlReportType.Size = new System.Drawing.Size(332, 33);
            this.pnlReportType.TabIndex = 48;
            // 
            // cmbReportType
            // 
            this.cmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReportType.FormattingEnabled = true;
            this.cmbReportType.Location = new System.Drawing.Point(93, 7);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new System.Drawing.Size(184, 21);
            this.cmbReportType.TabIndex = 64;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(14, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 15);
            this.label1.TabIndex = 57;
            this.label1.Text = "Report Type:";
            // 
            // pnlPOSSelection
            // 
            this.pnlPOSSelection.Controls.Add(this.cmbPOS);
            this.pnlPOSSelection.Controls.Add(this.label4);
            this.pnlPOSSelection.Location = new System.Drawing.Point(679, 3);
            this.pnlPOSSelection.Name = "pnlPOSSelection";
            this.pnlPOSSelection.Size = new System.Drawing.Size(332, 33);
            this.pnlPOSSelection.TabIndex = 47;
            // 
            // cmbPOS
            // 
            this.cmbPOS.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cmbPOS.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cmbPOS.Location = new System.Drawing.Point(56, 7);
            this.cmbPOS.Name = "cmbPOS";
            // 
            // 
            // 
            this.cmbPOS.RootElement.ControlBounds = new System.Drawing.Rectangle(56, 7, 125, 20);
            this.cmbPOS.RootElement.StretchVertically = true;
            this.cmbPOS.ShowCheckAllItems = true;
            this.cmbPOS.ShowImageInEditorArea = false;
            this.cmbPOS.Size = new System.Drawing.Size(188, 20);
            this.cmbPOS.TabIndex = 58;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(18, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 15);
            this.label4.TabIndex = 57;
            this.label4.Text = "POS:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnEmailReport);
            this.panel1.Controls.Add(this.btnGo);
            this.panel1.Location = new System.Drawing.Point(3, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1071, 45);
            this.panel1.TabIndex = 60;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(289, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(105, 31);
            this.btnClose.TabIndex = 38;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEmailReport
            // 
            this.btnEmailReport.Location = new System.Drawing.Point(148, 6);
            this.btnEmailReport.Name = "btnEmailReport";
            this.btnEmailReport.Size = new System.Drawing.Size(105, 31);
            this.btnEmailReport.TabIndex = 39;
            this.btnEmailReport.Text = "Email Report";
            this.btnEmailReport.UseVisualStyleBackColor = true;
            this.btnEmailReport.Click += new System.EventHandler(this.btnEmailReport_Click);
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGo.Location = new System.Drawing.Point(7, 6);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(105, 31);
            this.btnGo.TabIndex = 8;
            this.btnGo.Text = "Refresh";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // dtpTimeTo
            // 
            this.dtpTimeTo.CustomFormat = "h:mm tt";
            this.dtpTimeTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dtpTimeTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeTo.Location = new System.Drawing.Point(630, 13);
            this.dtpTimeTo.Name = "dtpTimeTo";
            this.dtpTimeTo.ShowUpDown = true;
            this.dtpTimeTo.Size = new System.Drawing.Size(74, 21);
            this.dtpTimeTo.TabIndex = 31;
            this.dtpTimeTo.ValueChanged += new System.EventHandler(this.dtpTimeTo_ValueChanged);
            // 
            // dtpTimeFrom
            // 
            this.dtpTimeFrom.CustomFormat = "h:mm tt";
            this.dtpTimeFrom.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dtpTimeFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeFrom.Location = new System.Drawing.Point(270, 13);
            this.dtpTimeFrom.Name = "dtpTimeFrom";
            this.dtpTimeFrom.ShowUpDown = true;
            this.dtpTimeFrom.Size = new System.Drawing.Size(74, 21);
            this.dtpTimeFrom.TabIndex = 30;
            this.dtpTimeFrom.ValueChanged += new System.EventHandler(this.dtpTimeFrom_ValueChanged);
            // 
            // labelFromDate
            // 
            this.labelFromDate.AutoSize = true;
            this.labelFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.labelFromDate.Location = new System.Drawing.Point(11, 16);
            this.labelFromDate.Name = "labelFromDate";
            this.labelFromDate.Size = new System.Drawing.Size(68, 15);
            this.labelFromDate.TabIndex = 28;
            this.labelFromDate.Text = "From Date:";
            // 
            // labelToDate
            // 
            this.labelToDate.AutoSize = true;
            this.labelToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.labelToDate.Location = new System.Drawing.Point(386, 16);
            this.labelToDate.Name = "labelToDate";
            this.labelToDate.Size = new System.Drawing.Size(52, 15);
            this.labelToDate.TabIndex = 29;
            this.labelToDate.Text = "To Date:";
            // 
            // CalFromDate
            // 
            this.CalFromDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.CalFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.CalFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CalFromDate.Location = new System.Drawing.Point(82, 13);
            this.CalFromDate.Name = "CalFromDate";
            this.CalFromDate.Size = new System.Drawing.Size(183, 21);
            this.CalFromDate.TabIndex = 26;
            this.CalFromDate.TabStop = false;
            this.CalFromDate.ValueChanged += new System.EventHandler(this.CalFromDate_ValueChanged);
            // 
            // CalToDate
            // 
            this.CalToDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.CalToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.CalToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CalToDate.Location = new System.Drawing.Point(442, 13);
            this.CalToDate.Name = "CalToDate";
            this.CalToDate.Size = new System.Drawing.Size(183, 21);
            this.CalToDate.TabIndex = 27;
            this.CalToDate.ValueChanged += new System.EventHandler(this.CalToDate_ValueChanged);
            // 
            // reportViewer
            // 
            this.reportViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer.Location = new System.Drawing.Point(1, 134);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.ParametersAreaVisible = false;
            this.reportViewer.ShowRefreshButton = false;
            this.reportViewer.Size = new System.Drawing.Size(1229, 411);
            this.reportViewer.TabIndex = 43;
            // 
            // TransactionReportviewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 561);
            this.Controls.Add(this.reportViewer);
            this.Controls.Add(this.groupBox1);
            this.Name = "TransactionReportviewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TransactionReportviewer";
            this.Load += new System.EventHandler(this.TransactionReportviewer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.pnlUser.ResumeLayout(false);
            this.pnlUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsers)).EndInit();
            this.pnlReportType.ResumeLayout(false);
            this.pnlReportType.PerformLayout();
            this.pnlPOSSelection.ResumeLayout(false);
            this.pnlPOSSelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPOS)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Telerik.ReportViewer.WinForms.ReportViewer reportViewer;
        private System.Windows.Forms.DateTimePicker dtpTimeTo;
        private System.Windows.Forms.DateTimePicker dtpTimeFrom;
        private System.Windows.Forms.Label labelFromDate;
        private System.Windows.Forms.Label labelToDate;
        private System.Windows.Forms.DateTimePicker CalFromDate;
        private System.Windows.Forms.DateTimePicker CalToDate;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel pnlUser;
        private Telerik.WinControls.UI.RadCheckedDropDownList cmbUsers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlReportType;
        private System.Windows.Forms.ComboBox cmbReportType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlPOSSelection;
        private Telerik.WinControls.UI.RadCheckedDropDownList cmbPOS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnEmailReport;
        private System.Windows.Forms.Button btnGo;
    }
}