namespace Semnox.Parafait.Report.Reports
{
    partial class RetailReportviewer
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.drpCategory = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnEmailReport = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.drpSortBy = new System.Windows.Forms.ComboBox();
            this.dtpTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.flpSegments = new System.Windows.Forms.TableLayoutPanel();
            this.labelToDate = new System.Windows.Forms.Label();
            this.CalFromDate = new System.Windows.Forms.DateTimePicker();
            this.labelFromDate = new System.Windows.Forms.Label();
            this.CalToDate = new System.Windows.Forms.DateTimePicker();
            this.reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dtpTimeTo);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.dtpTimeFrom);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.labelToDate);
            this.groupBox1.Controls.Add(this.CalFromDate);
            this.groupBox1.Controls.Add(this.labelFromDate);
            this.groupBox1.Controls.Add(this.CalToDate);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1237, 195);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            // 
            // dtpTimeTo
            // 
            this.dtpTimeTo.CustomFormat = "h:mm tt";
            this.dtpTimeTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dtpTimeTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeTo.Location = new System.Drawing.Point(642, 15);
            this.dtpTimeTo.Name = "dtpTimeTo";
            this.dtpTimeTo.ShowUpDown = true;
            this.dtpTimeTo.Size = new System.Drawing.Size(74, 21);
            this.dtpTimeTo.TabIndex = 46;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.drpCategory);
            this.groupBox4.Controls.Add(this.btnClose);
            this.groupBox4.Controls.Add(this.btnGo);
            this.groupBox4.Controls.Add(this.btnEmailReport);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.drpSortBy);
            this.groupBox4.Location = new System.Drawing.Point(7, 51);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(270, 135);
            this.groupBox4.TabIndex = 26;
            this.groupBox4.TabStop = false;
            // 
            // drpCategory
            // 
            this.drpCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.drpCategory.FormattingEnabled = true;
            this.drpCategory.Location = new System.Drawing.Point(74, 10);
            this.drpCategory.Name = "drpCategory";
            this.drpCategory.Size = new System.Drawing.Size(140, 21);
            this.drpCategory.TabIndex = 40;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(11, 104);
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
            this.btnGo.Location = new System.Drawing.Point(11, 70);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(101, 31);
            this.btnGo.TabIndex = 8;
            this.btnGo.Text = "Refresh";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnEmailReport
            // 
            this.btnEmailReport.Location = new System.Drawing.Point(118, 70);
            this.btnEmailReport.Name = "btnEmailReport";
            this.btnEmailReport.Size = new System.Drawing.Size(134, 31);
            this.btnEmailReport.TabIndex = 39;
            this.btnEmailReport.Text = "Email Report";
            this.btnEmailReport.UseVisualStyleBackColor = true;
            this.btnEmailReport.Click += new System.EventHandler(this.btnEmailReport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(6, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 15);
            this.label3.TabIndex = 23;
            this.label3.Text = "Category:";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(8, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Sort By:";
            // 
            // drpSortBy
            // 
            this.drpSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpSortBy.FormattingEnabled = true;
            this.drpSortBy.Location = new System.Drawing.Point(73, 40);
            this.drpSortBy.Name = "drpSortBy";
            this.drpSortBy.Size = new System.Drawing.Size(163, 21);
            this.drpSortBy.TabIndex = 9;
            // 
            // dtpTimeFrom
            // 
            this.dtpTimeFrom.CustomFormat = "h:mm tt";
            this.dtpTimeFrom.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dtpTimeFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeFrom.Location = new System.Drawing.Point(269, 15);
            this.dtpTimeFrom.Name = "dtpTimeFrom";
            this.dtpTimeFrom.ShowUpDown = true;
            this.dtpTimeFrom.Size = new System.Drawing.Size(74, 21);
            this.dtpTimeFrom.TabIndex = 45;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Location = new System.Drawing.Point(3, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1228, 150);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.flpSegments);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(280, 9);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(942, 135);
            this.groupBox5.TabIndex = 38;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Segment Selection";
            // 
            // flpSegments
            // 
            this.flpSegments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpSegments.AutoScroll = true;
            this.flpSegments.ColumnCount = 1;
            this.flpSegments.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.flpSegments.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.flpSegments.Location = new System.Drawing.Point(3, 16);
            this.flpSegments.Name = "flpSegments";
            this.flpSegments.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.flpSegments.RowCount = 1;
            this.flpSegments.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.flpSegments.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.flpSegments.Size = new System.Drawing.Size(933, 116);
            this.flpSegments.TabIndex = 0;
            // 
            // labelToDate
            // 
            this.labelToDate.AutoSize = true;
            this.labelToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.labelToDate.Location = new System.Drawing.Point(398, 18);
            this.labelToDate.Name = "labelToDate";
            this.labelToDate.Size = new System.Drawing.Size(52, 15);
            this.labelToDate.TabIndex = 44;
            this.labelToDate.Text = "To Date:";
            // 
            // CalFromDate
            // 
            this.CalFromDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.CalFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.CalFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CalFromDate.Location = new System.Drawing.Point(81, 15);
            this.CalFromDate.Name = "CalFromDate";
            this.CalFromDate.Size = new System.Drawing.Size(183, 21);
            this.CalFromDate.TabIndex = 41;
            this.CalFromDate.TabStop = false;
            // 
            // labelFromDate
            // 
            this.labelFromDate.AutoSize = true;
            this.labelFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.labelFromDate.Location = new System.Drawing.Point(10, 18);
            this.labelFromDate.Name = "labelFromDate";
            this.labelFromDate.Size = new System.Drawing.Size(68, 15);
            this.labelFromDate.TabIndex = 43;
            this.labelFromDate.Text = "From Date:";
            // 
            // CalToDate
            // 
            this.CalToDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.CalToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.CalToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.CalToDate.Location = new System.Drawing.Point(454, 15);
            this.CalToDate.Name = "CalToDate";
            this.CalToDate.Size = new System.Drawing.Size(183, 21);
            this.CalToDate.TabIndex = 42;
            // 
            // reportViewer
            // 
            this.reportViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer.Location = new System.Drawing.Point(3, 201);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.ParametersAreaVisible = false;
            this.reportViewer.ShowRefreshButton = false;
            this.reportViewer.Size = new System.Drawing.Size(1228, 348);
            this.reportViewer.TabIndex = 44;
            // 
            // RetailReportviewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 561);
            this.Controls.Add(this.reportViewer);
            this.Controls.Add(this.groupBox1);
            this.Name = "RetailReportviewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "RetailReportviewer";
            this.Load += new System.EventHandler(this.RetailReportviewer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnEmailReport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox drpSortBy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TableLayoutPanel flpSegments;
        private System.Windows.Forms.ComboBox drpCategory;
        private Telerik.ReportViewer.WinForms.ReportViewer reportViewer;
        private System.Windows.Forms.DateTimePicker dtpTimeTo;
        private System.Windows.Forms.DateTimePicker dtpTimeFrom;
        private System.Windows.Forms.Label labelToDate;
        private System.Windows.Forms.DateTimePicker CalFromDate;
        private System.Windows.Forms.Label labelFromDate;
        private System.Windows.Forms.DateTimePicker CalToDate;
    }
}