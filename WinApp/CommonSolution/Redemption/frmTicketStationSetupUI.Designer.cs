namespace Semnox.Parafait.Redemption
{
    partial class frmTicketStationSetupUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTicketStationSetupUI));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnPanel = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTicketStation = new System.Windows.Forms.DataGridView();
            this.ticketStationListBS = new System.Windows.Forms.BindingSource(this.components);
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketStationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stationTypeDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.voucherLengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkDigitDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ticketLengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkBitAlgorithmDataGridViewComboBox = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTicketStation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketStationListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPanel
            // 
            this.btnPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPanel.Controls.Add(this.btnRefresh);
            this.btnPanel.Controls.Add(this.btnDelete);
            this.btnPanel.Controls.Add(this.btnClose);
            this.btnPanel.Controls.Add(this.btnSave);
            this.btnPanel.Location = new System.Drawing.Point(15, 435);
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Size = new System.Drawing.Size(691, 48);
            this.btnPanel.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(139, 7);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 34);
            this.btnRefresh.TabIndex = 48;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(286, 7);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 34);
            this.btnDelete.TabIndex = 49;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(437, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 34);
            this.btnClose.TabIndex = 50;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(4, 7);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 34);
            this.btnSave.TabIndex = 47;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn1.HeaderText = "Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 120;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TicketStationId";
            this.dataGridViewTextBoxColumn2.HeaderText = "Ticket Station Id";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 120;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "VoucherLength";
            this.dataGridViewTextBoxColumn3.HeaderText = "Voucher Length";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 120;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "TicketLength";
            this.dataGridViewTextBoxColumn4.HeaderText = "Ticket Length";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Width = 120;
            // 
            // dgvTicketStation
            // 
            this.dgvTicketStation.AllowUserToResizeColumns = false;
            this.dgvTicketStation.AllowUserToResizeRows = false;
            this.dgvTicketStation.AutoGenerateColumns = false;
            this.dgvTicketStation.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTicketStation.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTicketStation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTicketStation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.ticketStationIdDataGridViewTextBoxColumn,
            this.stationTypeDataGridViewComboBoxColumn,
            this.voucherLengthDataGridViewTextBoxColumn,
            this.checkDigitDataGridViewCheckBoxColumn,
            this.ticketLengthDataGridViewTextBoxColumn,
            this.checkBitAlgorithmDataGridViewComboBox,
            this.isActiveDataGridViewCheckBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdatedDateDataGridViewTextBoxColumn});
            this.dgvTicketStation.DataSource = this.ticketStationListBS;
            this.dgvTicketStation.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvTicketStation.Location = new System.Drawing.Point(15, 25);
            this.dgvTicketStation.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.dgvTicketStation.Name = "dgvTicketStation";
            this.dgvTicketStation.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTicketStation.Size = new System.Drawing.Size(1087, 368);
            this.dgvTicketStation.TabIndex = 0;
            this.dgvTicketStation.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTicketStation_DataBindingComplete);
            this.dgvTicketStation.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvTicketStation_DataError);
            this.dgvTicketStation.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvTicketStation_DefaultValuesNeeded);
            // 
            // ticketStationListBS
            // 
            this.ticketStationListBS.DataSource = typeof(Semnox.Parafait.Redemption.TicketStationDTO);
            this.ticketStationListBS.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.ticketStationListBS_AddingNew);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.FillWeight = 37.36537F;
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ticketStationIdDataGridViewTextBoxColumn
            // 
            this.ticketStationIdDataGridViewTextBoxColumn.DataPropertyName = "TicketStationId";
            this.ticketStationIdDataGridViewTextBoxColumn.FillWeight = 111.3982F;
            this.ticketStationIdDataGridViewTextBoxColumn.HeaderText = "Ticket Station Id";
            this.ticketStationIdDataGridViewTextBoxColumn.Name = "ticketStationIdDataGridViewTextBoxColumn";
            // 
            // stationTypeDataGridViewComboBoxColumn
            // 
            this.stationTypeDataGridViewComboBoxColumn.DataPropertyName = "TicketStationType";
            this.stationTypeDataGridViewComboBoxColumn.FillWeight = 111.59F;
            this.stationTypeDataGridViewComboBoxColumn.HeaderText = "Ticket Station Type";
            this.stationTypeDataGridViewComboBoxColumn.Name = "stationTypeDataGridViewComboBoxColumn";
            // 
            // voucherLengthDataGridViewTextBoxColumn
            // 
            this.voucherLengthDataGridViewTextBoxColumn.DataPropertyName = "VoucherLength";
            this.voucherLengthDataGridViewTextBoxColumn.FillWeight = 118.001F;
            this.voucherLengthDataGridViewTextBoxColumn.HeaderText = "Voucher Length";
            this.voucherLengthDataGridViewTextBoxColumn.Name = "voucherLengthDataGridViewTextBoxColumn";
            // 
            // checkDigitDataGridViewCheckBoxColumn
            // 
            this.checkDigitDataGridViewCheckBoxColumn.DataPropertyName = "CheckDigit";
            this.checkDigitDataGridViewCheckBoxColumn.FillWeight = 73.26517F;
            this.checkDigitDataGridViewCheckBoxColumn.HeaderText = "Check Digit";
            this.checkDigitDataGridViewCheckBoxColumn.Name = "checkDigitDataGridViewCheckBoxColumn";
            // 
            // ticketLengthDataGridViewTextBoxColumn
            // 
            this.ticketLengthDataGridViewTextBoxColumn.DataPropertyName = "TicketLength";
            this.ticketLengthDataGridViewTextBoxColumn.FillWeight = 94.70319F;
            this.ticketLengthDataGridViewTextBoxColumn.HeaderText = "Ticket Length";
            this.ticketLengthDataGridViewTextBoxColumn.Name = "ticketLengthDataGridViewTextBoxColumn";
            this.ticketLengthDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ticketLengthDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // checkBitAlgorithmDataGridViewComboBox
            // 
            this.checkBitAlgorithmDataGridViewComboBox.DataPropertyName = "CheckBitAlgorithm";
            this.checkBitAlgorithmDataGridViewComboBox.HeaderText = "Check Bit Algorithm";
            this.checkBitAlgorithmDataGridViewComboBox.Name = "checkBitAlgorithmDataGridViewComboBox";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FillWeight = 74.79826F;
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.FillWeight = 154.1944F;
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Last Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn.FillWeight = 170.0322F;
            this.lastUpdatedDateDataGridViewTextBoxColumn.HeaderText = "Last Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn.Name = "lastUpdatedDateDataGridViewTextBoxColumn";
            this.lastUpdatedDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // frmTicketStationSetupUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1131, 492);
            this.Controls.Add(this.dgvTicketStation);
            this.Controls.Add(this.btnPanel);
            this.Name = "frmTicketStationSetupUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ticket Station Setup ";
            this.btnPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTicketStation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketStationListBS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource ticketStationListBS;
        private System.Windows.Forms.Panel btnPanel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridView dgvTicketStation;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticketStationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn stationTypeDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn voucherLengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn checkDigitDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticketLengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn checkBitAlgorithmDataGridViewComboBox;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
    }
}