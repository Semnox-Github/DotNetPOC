namespace Semnox.Parafait.Transaction
{
    partial class InvoiceSequenceSetupUI
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSysAuthorization = new System.Windows.Forms.TextBox();
            this.lblSysAuthorization = new System.Windows.Forms.Label();
            this.ddlInvoiceType = new System.Windows.Forms.ComboBox();
            this.lblInvoiceType = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvInvoiceSequenceSetupDTOList = new System.Windows.Forms.DataGridView();
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoiceTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.prefixDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seriesStartNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seriesEndNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approvedDateDataGridViewTextBoxColumn = new Semnox.Parafait.Transaction.CalendarColumn();
            this.expiryDateDataGridViewTextBoxColumn = new Semnox.Parafait.Transaction.CalendarColumn();
            this.resolutionNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resolutionDateDataGridViewTextBoxColumn = new Semnox.Parafait.Transaction.CalendarColumn();
            this.LastUpdatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdatedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.InvoiceSequenceSetupDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.calendarColumn1 = new Semnox.Parafait.Transaction.CalendarColumn();
            this.calendarColumn2 = new Semnox.Parafait.Transaction.CalendarColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.calendarColumn3 = new Semnox.Parafait.Transaction.CalendarColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoiceSequenceSetupDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InvoiceSequenceSetupDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSysAuthorization);
            this.groupBox1.Controls.Add(this.lblSysAuthorization);
            this.groupBox1.Controls.Add(this.ddlInvoiceType);
            this.groupBox1.Controls.Add(this.lblInvoiceType);
            this.groupBox1.Controls.Add(this.chbShowActiveEntries);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(964, 47);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // txtSysAuthorization
            // 
            this.txtSysAuthorization.Location = new System.Drawing.Point(886, 16);
            this.txtSysAuthorization.Multiline = true;
            this.txtSysAuthorization.Name = "txtSysAuthorization";
            this.txtSysAuthorization.Size = new System.Drawing.Size(203, 23);
            this.txtSysAuthorization.TabIndex = 28;
            // 
            // lblSysAuthorization
            // 
            this.lblSysAuthorization.Location = new System.Drawing.Point(603, 16);
            this.lblSysAuthorization.Name = "lblSysAuthorization";
            this.lblSysAuthorization.Size = new System.Drawing.Size(277, 22);
            this.lblSysAuthorization.TabIndex = 27;
            this.lblSysAuthorization.Text = "System Resolution Authorization No :";
            this.lblSysAuthorization.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlInvoiceType
            // 
            this.ddlInvoiceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlInvoiceType.FormattingEnabled = true;
            this.ddlInvoiceType.Location = new System.Drawing.Point(356, 15);
            this.ddlInvoiceType.Name = "ddlInvoiceType";
            this.ddlInvoiceType.Size = new System.Drawing.Size(121, 23);
            this.ddlInvoiceType.TabIndex = 26;
            // 
            // lblInvoiceType
            // 
            this.lblInvoiceType.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblInvoiceType.Location = new System.Drawing.Point(249, 18);
            this.lblInvoiceType.Name = "lblInvoiceType";
            this.lblInvoiceType.Size = new System.Drawing.Size(101, 20);
            this.lblInvoiceType.TabIndex = 25;
            this.lblInvoiceType.Text = "Invoice Type:";
            this.lblInvoiceType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(6, 20);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 22;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(490, 15);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 24;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(10, 445);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(340, 445);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(230, 445);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 21;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(120, 445);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 20;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dgvInvoiceSequenceSetupDTOList
            // 
            this.dgvInvoiceSequenceSetupDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvInvoiceSequenceSetupDTOList.AutoGenerateColumns = false;
            this.dgvInvoiceSequenceSetupDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInvoiceSequenceSetupDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn,
            this.invoiceTypeIdDataGridViewTextBoxColumn,
            this.prefixDataGridViewTextBoxColumn,
            this.currentValueDataGridViewTextBoxColumn,
            this.seriesStartNumberDataGridViewTextBoxColumn,
            this.seriesEndNumberDataGridViewTextBoxColumn,
            this.approvedDateDataGridViewTextBoxColumn,
            this.expiryDateDataGridViewTextBoxColumn,
            this.resolutionNumberDataGridViewTextBoxColumn,
            this.resolutionDateDataGridViewTextBoxColumn,
            this.LastUpdatedBy,
            this.LastUpdatedDate,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvInvoiceSequenceSetupDTOList.DataSource = this.InvoiceSequenceSetupDTOListBS;
            this.dgvInvoiceSequenceSetupDTOList.Location = new System.Drawing.Point(10, 65);
            this.dgvInvoiceSequenceSetupDTOList.Margin = new System.Windows.Forms.Padding(5);
            this.dgvInvoiceSequenceSetupDTOList.Name = "dgvInvoiceSequenceSetupDTOList";
            this.dgvInvoiceSequenceSetupDTOList.Size = new System.Drawing.Size(964, 372);
            this.dgvInvoiceSequenceSetupDTOList.TabIndex = 24;
            this.dgvInvoiceSequenceSetupDTOList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInvoiceSequenceSetupDTOList_CellDoubleClick);
            this.dgvInvoiceSequenceSetupDTOList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.InvoiceSequenceSetupDTOListBS_BindingComplete);
            this.dgvInvoiceSequenceSetupDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvInvoiceSequenceSetupDTOList_DataError);
            // 
            // invoiceSequenceSetupIdDataGridViewTextBoxColumn
            // 
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn.DataPropertyName = "InvoiceSequenceSetupId";
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn.HeaderText = "SI#";
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn.Name = "invoiceSequenceSetupIdDataGridViewTextBoxColumn";
            this.invoiceSequenceSetupIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // invoiceTypeIdDataGridViewTextBoxColumn
            // 
            this.invoiceTypeIdDataGridViewTextBoxColumn.DataPropertyName = "InvoiceTypeId";
            this.invoiceTypeIdDataGridViewTextBoxColumn.HeaderText = "Invoice Type";
            this.invoiceTypeIdDataGridViewTextBoxColumn.Name = "invoiceTypeIdDataGridViewTextBoxColumn";
            this.invoiceTypeIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.invoiceTypeIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // prefixDataGridViewTextBoxColumn
            // 
            this.prefixDataGridViewTextBoxColumn.DataPropertyName = "Prefix";
            this.prefixDataGridViewTextBoxColumn.HeaderText = "Prefix";
            this.prefixDataGridViewTextBoxColumn.Name = "prefixDataGridViewTextBoxColumn";
            // 
            // currentValueDataGridViewTextBoxColumn
            // 
            this.currentValueDataGridViewTextBoxColumn.DataPropertyName = "CurrentValue";
            this.currentValueDataGridViewTextBoxColumn.HeaderText = "Current Value";
            this.currentValueDataGridViewTextBoxColumn.Name = "currentValueDataGridViewTextBoxColumn";
            this.currentValueDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // seriesStartNumberDataGridViewTextBoxColumn
            // 
            this.seriesStartNumberDataGridViewTextBoxColumn.DataPropertyName = "SeriesStartNumber";
            this.seriesStartNumberDataGridViewTextBoxColumn.HeaderText = "Series Start Number";
            this.seriesStartNumberDataGridViewTextBoxColumn.Name = "seriesStartNumberDataGridViewTextBoxColumn";
            // 
            // seriesEndNumberDataGridViewTextBoxColumn
            // 
            this.seriesEndNumberDataGridViewTextBoxColumn.DataPropertyName = "SeriesEndNumber";
            this.seriesEndNumberDataGridViewTextBoxColumn.HeaderText = "Series End Number";
            this.seriesEndNumberDataGridViewTextBoxColumn.Name = "seriesEndNumberDataGridViewTextBoxColumn";
            // 
            // approvedDateDataGridViewTextBoxColumn
            // 
            this.approvedDateDataGridViewTextBoxColumn.DataPropertyName = "ApprovedDate";
            this.approvedDateDataGridViewTextBoxColumn.HeaderText = "Approved Date";
            this.approvedDateDataGridViewTextBoxColumn.Name = "approvedDateDataGridViewTextBoxColumn";
            // 
            // expiryDateDataGridViewTextBoxColumn
            // 
            this.expiryDateDataGridViewTextBoxColumn.DataPropertyName = "ExpiryDate";
            this.expiryDateDataGridViewTextBoxColumn.HeaderText = "Expiry Date";
            this.expiryDateDataGridViewTextBoxColumn.Name = "expiryDateDataGridViewTextBoxColumn";
            // 
            // resolutionNumberDataGridViewTextBoxColumn
            // 
            this.resolutionNumberDataGridViewTextBoxColumn.DataPropertyName = "ResolutionNumber";
            this.resolutionNumberDataGridViewTextBoxColumn.HeaderText = "Resolution Number";
            this.resolutionNumberDataGridViewTextBoxColumn.Name = "resolutionNumberDataGridViewTextBoxColumn";
            // 
            // resolutionDateDataGridViewTextBoxColumn
            // 
            this.resolutionDateDataGridViewTextBoxColumn.DataPropertyName = "ResolutionDate";
            this.resolutionDateDataGridViewTextBoxColumn.HeaderText = "Resolution Date";
            this.resolutionDateDataGridViewTextBoxColumn.Name = "resolutionDateDataGridViewTextBoxColumn";
            // 
            // LastUpdatedBy
            // 
            this.LastUpdatedBy.DataPropertyName = "LastUpdatedBy";
            this.LastUpdatedBy.HeaderText = "Last Updated By";
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
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // InvoiceSequenceSetupDTOListBS
            // 
            this.InvoiceSequenceSetupDTOListBS.DataSource = typeof(Semnox.Parafait.Transaction.InvoiceSequenceSetupDTO);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "InvoiceSequenceSetupId";
            this.dataGridViewTextBoxColumn1.HeaderText = "SI#";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Prefix";
            this.dataGridViewTextBoxColumn2.HeaderText = "Prefix";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "CurrentValue";
            this.dataGridViewTextBoxColumn3.HeaderText = "Current Value";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "SeriesStartNumber";
            this.dataGridViewTextBoxColumn4.HeaderText = "Series Start Number";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "SeriesEndNumber";
            this.dataGridViewTextBoxColumn5.HeaderText = "Series End Number";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // calendarColumn1
            // 
            this.calendarColumn1.DataPropertyName = "ApprovedDate";
            this.calendarColumn1.HeaderText = "Approved Date";
            this.calendarColumn1.Name = "calendarColumn1";
            // 
            // calendarColumn2
            // 
            this.calendarColumn2.DataPropertyName = "ExpiryDate";
            this.calendarColumn2.HeaderText = "Expiry Date";
            this.calendarColumn2.Name = "calendarColumn2";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "ResolutionNumber";
            this.dataGridViewTextBoxColumn6.HeaderText = "Resolution Number";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // calendarColumn3
            // 
            this.calendarColumn3.DataPropertyName = "ResolutionDate";
            this.calendarColumn3.HeaderText = "Resolution Date";
            this.calendarColumn3.Name = "calendarColumn3";
            // 
            // InvoiceSequenceSetupUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 481);
            this.Controls.Add(this.dgvInvoiceSequenceSetupDTOList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Name = "InvoiceSequenceSetupUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Invoice Sequence Setup";
            this.Load += new System.EventHandler(this.InvoiceSequenceSetupUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvoiceSequenceSetupDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InvoiceSequenceSetupDTOListBS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblInvoiceType;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox ddlInvoiceType;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvInvoiceSequenceSetupDTOList;
        private System.Windows.Forms.BindingSource InvoiceSequenceSetupDTOListBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private CalendarColumn calendarColumn1;
        private CalendarColumn calendarColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private CalendarColumn calendarColumn3;
        private System.Windows.Forms.TextBox txtSysAuthorization;
        private System.Windows.Forms.Label lblSysAuthorization;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoiceSequenceSetupIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn invoiceTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn prefixDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seriesStartNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seriesEndNumberDataGridViewTextBoxColumn;
        private CalendarColumn approvedDateDataGridViewTextBoxColumn;
        private CalendarColumn expiryDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resolutionNumberDataGridViewTextBoxColumn;
        private CalendarColumn resolutionDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdatedDate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}

