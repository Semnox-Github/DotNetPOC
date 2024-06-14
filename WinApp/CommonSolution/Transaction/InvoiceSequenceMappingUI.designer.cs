namespace Semnox.Parafait.Transaction
{
    partial class InvoiceSequenceMappingUI
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
            this.dgvInvoiceSequenceMapping = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sequenceIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.effectiveDateDataGridViewTextBoxColumn = new Semnox.Parafait.Transaction.CalendarColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.InvoiceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SeriesSatrtNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SeriesEndNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpiryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdatedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoiceSequenceMappingDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.calendarColumn1 = new Semnox.Parafait.Transaction.CalendarColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoiceSequenceMapping)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceSequenceMappingDTOListBS)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvInvoiceSequenceMapping
            // 
            this.dgvInvoiceSequenceMapping.AutoGenerateColumns = false;
            this.dgvInvoiceSequenceMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInvoiceSequenceMapping.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.sequenceIdDataGridViewTextBoxColumn,
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn,
            this.effectiveDateDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.InvoiceType,
            this.CurrentValue,
            this.SeriesSatrtNumber,
            this.SeriesEndNumber,
            this.ExpiryDate,
            this.LastUpdatedBy,
            this.LastUpdatedDate});
            this.dgvInvoiceSequenceMapping.DataSource = this.invoiceSequenceMappingDTOListBS;
            this.dgvInvoiceSequenceMapping.Location = new System.Drawing.Point(13, 71);
            this.dgvInvoiceSequenceMapping.Name = "dgvInvoiceSequenceMapping";
            this.dgvInvoiceSequenceMapping.Size = new System.Drawing.Size(562, 334);
            this.dgvInvoiceSequenceMapping.TabIndex = 28;
            this.dgvInvoiceSequenceMapping.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInvoiceSequenceMapping_CellValueChanged);
            this.dgvInvoiceSequenceMapping.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvInvoiceSequenceMapping_DataBindingComplete);
            this.dgvInvoiceSequenceMapping.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvInvoiceSequenceMapping_DataError);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "ID";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sequenceIdDataGridViewTextBoxColumn
            // 
            this.sequenceIdDataGridViewTextBoxColumn.DataPropertyName = "SequenceId";
            this.sequenceIdDataGridViewTextBoxColumn.HeaderText = "Sequence Id";
            this.sequenceIdDataGridViewTextBoxColumn.Name = "sequenceIdDataGridViewTextBoxColumn";
            this.sequenceIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // invoiceSequenceSetupIdDataGridViewTextBoxColumn
            // 
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn.DataPropertyName = "InvoiceSequenceSetupId";
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn.HeaderText = "Invoice Sequence";
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn.Name = "invoiceSequenceSetupIdDataGridViewTextBoxColumn";
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // effectiveDateDataGridViewTextBoxColumn
            // 
            this.effectiveDateDataGridViewTextBoxColumn.DataPropertyName = "EffectiveDate";
            this.effectiveDateDataGridViewTextBoxColumn.HeaderText = "Effective Date";
            this.effectiveDateDataGridViewTextBoxColumn.Name = "effectiveDateDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // InvoiceType
            // 
            this.InvoiceType.HeaderText = "Invoice Type";
            this.InvoiceType.Name = "InvoiceType";
            this.InvoiceType.ReadOnly = true;
            // 
            // CurrentValue
            // 
            this.CurrentValue.HeaderText = "Current Value";
            this.CurrentValue.Name = "CurrentValue";
            this.CurrentValue.ReadOnly = true;
            // 
            // SeriesSatrtNumber
            // 
            this.SeriesSatrtNumber.HeaderText = "Series Start Number";
            this.SeriesSatrtNumber.Name = "SeriesSatrtNumber";
            this.SeriesSatrtNumber.ReadOnly = true;
            // 
            // SeriesEndNumber
            // 
            this.SeriesEndNumber.HeaderText = "Series End Number";
            this.SeriesEndNumber.Name = "SeriesEndNumber";
            this.SeriesEndNumber.ReadOnly = true;
            // 
            // ExpiryDate
            // 
            this.ExpiryDate.HeaderText = "Expiry Date";
            this.ExpiryDate.Name = "ExpiryDate";
            this.ExpiryDate.ReadOnly = true;
            // 
            // LastUpdatedBy
            // 
            this.LastUpdatedBy.DataPropertyName = "LastUpdatedBy";
            this.LastUpdatedBy.HeaderText = "Last Updated User";
            this.LastUpdatedBy.Name = "LastUpdatedBy";
            this.LastUpdatedBy.ReadOnly = true;
            // 
            // LastUpdatedDate
            // 
            this.LastUpdatedDate.DataPropertyName = "LastUpdatedDate";
            this.LastUpdatedDate.HeaderText = "Last Updated Date";
            this.LastUpdatedDate.Name = "LastUpdatedDate";
            this.LastUpdatedDate.ReadOnly = true;
            // 
            // invoiceSequenceMappingDTOListBS
            // 
            this.invoiceSequenceMappingDTOListBS.DataSource = typeof(Semnox.Parafait.Transaction.InvoiceSequenceMappingDTO);
            // 
            // btnDelete
            // 
            this.btnDelete.CausesValidation = false;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(263, 421);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 23);
            this.btnDelete.TabIndex = 32;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(388, 421);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 25);
            this.btnClose.TabIndex = 31;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(138, 421);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 23);
            this.btnRefresh.TabIndex = 29;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(13, 421);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.chbShowActiveEntries);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 53);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(200, 15);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 25;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(6, 18);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 22;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "SequenceId";
            this.dataGridViewTextBoxColumn2.HeaderText = "Sequence Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // calendarColumn1
            // 
            this.calendarColumn1.DataPropertyName = "EffectiveDate";
            this.calendarColumn1.HeaderText = "Effective Date";
            this.calendarColumn1.Name = "calendarColumn1";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "InvoiceSequenceSetupId";
            this.dataGridViewTextBoxColumn3.HeaderText = "Invoice Sequence";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "InvoiceType";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "SeriesSatrtNumber";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "SeriesEndNumber";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // InvoiceSequenceMappingUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 457);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvInvoiceSequenceMapping);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InvoiceSequenceMappingUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Invoice Sequence Mapping";
            this.Load += new System.EventHandler(this.InvoiceSequenceMappingUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoiceSequenceMapping)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceSequenceMappingDTOListBS)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvInvoiceSequenceMapping;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.BindingSource invoiceSequenceMappingDTOListBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private CalendarColumn calendarColumn1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sequenceIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn invoiceSequenceSetupIdDataGridViewTextBoxColumn;
        private CalendarColumn effectiveDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeriesSatrtNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeriesEndNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpiryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdatedDate;
    }
}