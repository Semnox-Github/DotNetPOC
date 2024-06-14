namespace Semnox.Parafait.Report.Reports
{
    partial class CustomReportviewer
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.flpParameters1 = new System.Windows.Forms.FlowLayoutPanel();
            this.CalFromDate = new System.Windows.Forms.DateTimePicker();
            this.CalToDate = new System.Windows.Forms.DateTimePicker();
            this.labelFromDate = new System.Windows.Forms.Label();
            this.labelToDate = new System.Windows.Forms.Label();
            this.dtpTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTimeTo = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnEmailReport = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // reportViewer
            // 
            this.reportViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.reportViewer.Location = new System.Drawing.Point(1, 194);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.ParametersAreaVisible = false;
            this.reportViewer.ShowRefreshButton = false;
            this.reportViewer.Size = new System.Drawing.Size(1221, 355);
            this.reportViewer.TabIndex = 48;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox5.Controls.Add(this.flpParameters1);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(11, 37);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1180, 105);
            this.groupBox5.TabIndex = 38;
            this.groupBox5.TabStop = false;
            // 
            // flpParameters1
            // 
            this.flpParameters1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpParameters1.AutoScroll = true;
            this.flpParameters1.Location = new System.Drawing.Point(19, 19);
            this.flpParameters1.Name = "flpParameters1";
            this.flpParameters1.Size = new System.Drawing.Size(1155, 80);
            this.flpParameters1.TabIndex = 0;
            // 
            // CalFromDate
            // 
            this.CalFromDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.CalFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.CalFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CalFromDate.Location = new System.Drawing.Point(82, 13);
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
            this.CalToDate.Location = new System.Drawing.Point(455, 13);
            this.CalToDate.Name = "CalToDate";
            this.CalToDate.Size = new System.Drawing.Size(183, 21);
            this.CalToDate.TabIndex = 22;
            // 
            // labelFromDate
            // 
            this.labelFromDate.AutoSize = true;
            this.labelFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.labelFromDate.Location = new System.Drawing.Point(11, 16);
            this.labelFromDate.Name = "labelFromDate";
            this.labelFromDate.Size = new System.Drawing.Size(68, 15);
            this.labelFromDate.TabIndex = 23;
            this.labelFromDate.Text = "From Date:";
            this.labelFromDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelToDate
            // 
            this.labelToDate.AutoSize = true;
            this.labelToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.labelToDate.Location = new System.Drawing.Point(399, 16);
            this.labelToDate.Name = "labelToDate";
            this.labelToDate.Size = new System.Drawing.Size(52, 15);
            this.labelToDate.TabIndex = 24;
            this.labelToDate.Text = "To Date:";
            this.labelToDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.dtpTimeFrom.TabIndex = 25;
            // 
            // dtpTimeTo
            // 
            this.dtpTimeTo.CustomFormat = "h:mm tt";
            this.dtpTimeTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dtpTimeTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeTo.Location = new System.Drawing.Point(643, 13);
            this.dtpTimeTo.Name = "dtpTimeTo";
            this.dtpTimeTo.ShowUpDown = true;
            this.dtpTimeTo.Size = new System.Drawing.Size(74, 21);
            this.dtpTimeTo.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.pnlButtons);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.dtpTimeTo);
            this.groupBox1.Controls.Add(this.dtpTimeFrom);
            this.groupBox1.Controls.Add(this.labelToDate);
            this.groupBox1.Controls.Add(this.labelFromDate);
            this.groupBox1.Controls.Add(this.CalToDate);
            this.groupBox1.Controls.Add(this.CalFromDate);
            this.groupBox1.Location = new System.Drawing.Point(1, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1221, 188);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(241, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 34);
            this.btnClose.TabIndex = 41;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGo.Location = new System.Drawing.Point(8, 0);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(97, 34);
            this.btnGo.TabIndex = 40;
            this.btnGo.Text = "Refresh";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnEmailReport
            // 
            this.btnEmailReport.Location = new System.Drawing.Point(126, 0);
            this.btnEmailReport.Name = "btnEmailReport";
            this.btnEmailReport.Size = new System.Drawing.Size(97, 34);
            this.btnEmailReport.TabIndex = 42;
            this.btnEmailReport.Text = "Email Report";
            this.btnEmailReport.UseVisualStyleBackColor = true;
            this.btnEmailReport.Click += new System.EventHandler(this.btnEmailReport_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnEmailReport);
            this.pnlButtons.Controls.Add(this.btnGo);
            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Location = new System.Drawing.Point(6, 148);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(353, 34);
            this.pnlButtons.TabIndex = 42;
            // 
            // CustomReportviewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1224, 561);
            this.Controls.Add(this.reportViewer);
            this.Controls.Add(this.groupBox1);
            this.Name = "CustomReportviewer";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomReportviewer";
            this.Load += new System.EventHandler(this.CustomReportviewer_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.ReportViewer.WinForms.ReportViewer reportViewer;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DateTimePicker CalFromDate;
        private System.Windows.Forms.DateTimePicker CalToDate;
        private System.Windows.Forms.Label labelFromDate;
        private System.Windows.Forms.Label labelToDate;
        private System.Windows.Forms.DateTimePicker dtpTimeFrom;
        private System.Windows.Forms.DateTimePicker dtpTimeTo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnEmailReport;
        private System.Windows.Forms.FlowLayoutPanel flpParameters1;
        private System.Windows.Forms.Panel pnlButtons;
    }
}