namespace Semnox.Parafait.Report.Reports
{
    partial class CustomReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomReports));
            this.label1 = new System.Windows.Forms.Label();
            this.txtReportName = new System.Windows.Forms.TextBox();
            this.txtDBQuery = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbOutputFormat = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBreakColumn = new System.Windows.Forms.TextBox();
            this.chkHideBreakColumn = new System.Windows.Forms.CheckBox();
            this.txtReportGroup = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAggregateColumns = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkHideGridLines = new System.Windows.Forms.CheckBox();
            this.btnParameters = new System.Windows.Forms.Button();
            this.chkShowGrandTotal = new System.Windows.Forms.CheckBox();
            this.chkPrintContinuous = new System.Windows.Forms.CheckBox();
            this.chkRepeatBreakColumns = new System.Windows.Forms.CheckBox();
            this.lblMaxdateRange = new System.Windows.Forms.Label();
            this.txtMaxDateRange = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Report Name:";
            // 
            // txtReportName
            // 
            this.txtReportName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReportName.Location = new System.Drawing.Point(99, 23);
            this.txtReportName.Name = "txtReportName";
            this.txtReportName.Size = new System.Drawing.Size(508, 22);
            this.txtReportName.TabIndex = 1;
            // 
            // txtDBQuery
            // 
            this.txtDBQuery.AcceptsTab = true;
            this.txtDBQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDBQuery.BackColor = System.Drawing.Color.AliceBlue;
            this.txtDBQuery.DetectUrls = false;
            this.txtDBQuery.Location = new System.Drawing.Point(99, 51);
            this.txtDBQuery.Name = "txtDBQuery";
            this.txtDBQuery.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtDBQuery.Size = new System.Drawing.Size(799, 253);
            this.txtDBQuery.TabIndex = 3;
            this.txtDBQuery.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "DB Query:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(419, 369);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Output Format:";
            // 
            // cmbOutputFormat
            // 
            this.cmbOutputFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbOutputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOutputFormat.FormattingEnabled = true;
            this.cmbOutputFormat.Items.AddRange(new object[] {
            "PDF",
            "HTML",
            "Excel",
            "Chart"});
            this.cmbOutputFormat.Location = new System.Drawing.Point(521, 367);
            this.cmbOutputFormat.Name = "cmbOutputFormat";
            this.cmbOutputFormat.Size = new System.Drawing.Size(121, 24);
            this.cmbOutputFormat.TabIndex = 8;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(99, 447);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(651, 447);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPreview.Location = new System.Drawing.Point(375, 447);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(90, 23);
            this.btnPreview.TabIndex = 10;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(513, 447);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Break Column:";
            // 
            // txtBreakColumn
            // 
            this.txtBreakColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBreakColumn.Location = new System.Drawing.Point(115, 332);
            this.txtBreakColumn.Name = "txtBreakColumn";
            this.txtBreakColumn.Size = new System.Drawing.Size(126, 22);
            this.txtBreakColumn.TabIndex = 4;
            // 
            // chkHideBreakColumn
            // 
            this.chkHideBreakColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkHideBreakColumn.AutoSize = true;
            this.chkHideBreakColumn.Location = new System.Drawing.Point(259, 334);
            this.chkHideBreakColumn.Name = "chkHideBreakColumn";
            this.chkHideBreakColumn.Size = new System.Drawing.Size(139, 20);
            this.chkHideBreakColumn.TabIndex = 5;
            this.chkHideBreakColumn.Text = "Hide Break Column";
            this.chkHideBreakColumn.UseVisualStyleBackColor = true;
            // 
            // txtReportGroup
            // 
            this.txtReportGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReportGroup.Location = new System.Drawing.Point(704, 23);
            this.txtReportGroup.Name = "txtReportGroup";
            this.txtReportGroup.Size = new System.Drawing.Size(194, 22);
            this.txtReportGroup.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(613, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 16);
            this.label5.TabIndex = 15;
            this.label5.Text = "Report Group:";
            // 
            // txtAggregateColumns
            // 
            this.txtAggregateColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtAggregateColumns.Location = new System.Drawing.Point(799, 335);
            this.txtAggregateColumns.Name = "txtAggregateColumns";
            this.txtAggregateColumns.Size = new System.Drawing.Size(154, 22);
            this.txtAggregateColumns.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(673, 338);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Aggregate Columns:";
            // 
            // chkHideGridLines
            // 
            this.chkHideGridLines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkHideGridLines.AutoSize = true;
            this.chkHideGridLines.Location = new System.Drawing.Point(13, 366);
            this.chkHideGridLines.Name = "chkHideGridLines";
            this.chkHideGridLines.Size = new System.Drawing.Size(116, 20);
            this.chkHideGridLines.TabIndex = 7;
            this.chkHideGridLines.Text = "Hide Grid Lines";
            this.chkHideGridLines.UseVisualStyleBackColor = true;
            // 
            // btnParameters
            // 
            this.btnParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnParameters.Location = new System.Drawing.Point(237, 447);
            this.btnParameters.Name = "btnParameters";
            this.btnParameters.Size = new System.Drawing.Size(90, 23);
            this.btnParameters.TabIndex = 18;
            this.btnParameters.Text = "Parameters";
            this.btnParameters.UseVisualStyleBackColor = true;
            this.btnParameters.Click += new System.EventHandler(this.btnParameters_Click);
            // 
            // chkShowGrandTotal
            // 
            this.chkShowGrandTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowGrandTotal.AutoSize = true;
            this.chkShowGrandTotal.Checked = true;
            this.chkShowGrandTotal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowGrandTotal.Location = new System.Drawing.Point(259, 369);
            this.chkShowGrandTotal.Name = "chkShowGrandTotal";
            this.chkShowGrandTotal.Size = new System.Drawing.Size(129, 20);
            this.chkShowGrandTotal.TabIndex = 19;
            this.chkShowGrandTotal.Text = "Show Grand Total";
            this.chkShowGrandTotal.UseVisualStyleBackColor = true;
            // 
            // chkPrintContinuous
            // 
            this.chkPrintContinuous.AutoSize = true;
            this.chkPrintContinuous.Location = new System.Drawing.Point(676, 369);
            this.chkPrintContinuous.Name = "chkPrintContinuous";
            this.chkPrintContinuous.Size = new System.Drawing.Size(220, 20);
            this.chkPrintContinuous.TabIndex = 20;
            this.chkPrintContinuous.Text = "Print Continuous(Receipt Format)";
            this.chkPrintContinuous.UseVisualStyleBackColor = true;
            // 
            // chkRepeatBreakColumns
            // 
            this.chkRepeatBreakColumns.AutoSize = true;
            this.chkRepeatBreakColumns.Location = new System.Drawing.Point(422, 335);
            this.chkRepeatBreakColumns.Name = "chkRepeatBreakColumns";
            this.chkRepeatBreakColumns.Size = new System.Drawing.Size(181, 20);
            this.chkRepeatBreakColumns.TabIndex = 23;
            this.chkRepeatBreakColumns.Text = "Repeat BreakColumn Data";
            this.chkRepeatBreakColumns.UseVisualStyleBackColor = true;
            // 
            // lblMaxdateRange
            // 
            this.lblMaxdateRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMaxdateRange.AutoSize = true;
            this.lblMaxdateRange.Location = new System.Drawing.Point(10, 401);
            this.lblMaxdateRange.Name = "lblMaxdateRange";
            this.lblMaxdateRange.Size = new System.Drawing.Size(161, 16);
            this.lblMaxdateRange.TabIndex = 24;
            this.lblMaxdateRange.Text = "Max Date Range(In Days):";
            // 
            // txtMaxDateRange
            // 
            this.txtMaxDateRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtMaxDateRange.Location = new System.Drawing.Point(177, 401);
            this.txtMaxDateRange.Name = "txtMaxDateRange";
            this.txtMaxDateRange.Size = new System.Drawing.Size(107, 22);
            this.txtMaxDateRange.TabIndex = 25;
            // 
            // CustomReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(976, 481);
            this.Controls.Add(this.txtMaxDateRange);
            this.Controls.Add(this.lblMaxdateRange);
            this.Controls.Add(this.chkRepeatBreakColumns);
            this.Controls.Add(this.chkPrintContinuous);
            this.Controls.Add(this.chkShowGrandTotal);
            this.Controls.Add(this.btnParameters);
            this.Controls.Add(this.chkHideGridLines);
            this.Controls.Add(this.txtAggregateColumns);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtReportGroup);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkHideBreakColumn);
            this.Controls.Add(this.txtBreakColumn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cmbOutputFormat);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDBQuery);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtReportName);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CustomReports";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Custom Reports";
            this.Load += new System.EventHandler(this.CustomReports_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReportName;
        private System.Windows.Forms.RichTextBox txtDBQuery;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbOutputFormat;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBreakColumn;
        private System.Windows.Forms.CheckBox chkHideBreakColumn;
        private System.Windows.Forms.TextBox txtReportGroup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAggregateColumns;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkHideGridLines;
        private System.Windows.Forms.Button btnParameters;
        private System.Windows.Forms.CheckBox chkShowGrandTotal;
        private System.Windows.Forms.CheckBox chkPrintContinuous;
        private System.Windows.Forms.CheckBox chkRepeatBreakColumns;
        private System.Windows.Forms.Label lblMaxdateRange;
        private System.Windows.Forms.TextBox txtMaxDateRange;
    }
}