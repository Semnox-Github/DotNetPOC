namespace Semnox.Parafait.Device.Lockers
{
    partial class LockerAccessPointUI
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvDisplayData = new System.Windows.Forms.DataGridView();
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.lockerAccessPointDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accessPointIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iPAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.portNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.channelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gatewayIPDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.baudRateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupCode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lockerIDFromDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockerIDToDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataPackingTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataPackingSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isAliveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayData)).BeginInit();
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lockerAccessPointDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(269, 296);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(397, 296);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(141, 296);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 15;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(13, 296);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvDisplayData
            // 
            this.dgvDisplayData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDisplayData.AutoGenerateColumns = false;
            this.dgvDisplayData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDisplayData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.accessPointIdDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn1,
            this.iPAddressDataGridViewTextBoxColumn,
            this.portNoDataGridViewTextBoxColumn,
            this.channelDataGridViewTextBoxColumn,
            this.gatewayIPDataGridViewTextBoxColumn,
            this.baudRateDataGridViewTextBoxColumn,
            this.GroupCode,
            this.lockerIDFromDataGridViewTextBoxColumn,
            this.lockerIDToDataGridViewTextBoxColumn,
            this.dataPackingTimeDataGridViewTextBoxColumn,
            this.dataPackingSizeDataGridViewTextBoxColumn,
            this.isAliveDataGridViewCheckBoxColumn,
            this.dataGridViewCheckBoxColumn1});
            this.dgvDisplayData.DataSource = this.lockerAccessPointDTOBindingSource;
            this.dgvDisplayData.Location = new System.Drawing.Point(12, 61);
            this.dgvDisplayData.Name = "dgvDisplayData";
            this.dgvDisplayData.Size = new System.Drawing.Size(743, 220);
            this.dgvDisplayData.TabIndex = 13;
            this.dgvDisplayData.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDisplayData_DataError);
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.txtName);
            this.gpFilter.Controls.Add(this.lblName);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(12, 8);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(743, 47);
            this.gpFilter.TabIndex = 18;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(395, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(253, 17);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 2;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(151, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(96, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(12, 18);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 0;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // lockerAccessPointDTOBindingSource
            // 
            this.lockerAccessPointDTOBindingSource.DataSource = typeof(LockerAccessPointDTO);
            // 
            // accessPointIdDataGridViewTextBoxColumn
            // 
            this.accessPointIdDataGridViewTextBoxColumn.DataPropertyName = "AccessPointId";
            this.accessPointIdDataGridViewTextBoxColumn.HeaderText = "AccessPointId";
            this.accessPointIdDataGridViewTextBoxColumn.Name = "accessPointIdDataGridViewTextBoxColumn";
            this.accessPointIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // iPAddressDataGridViewTextBoxColumn
            // 
            this.iPAddressDataGridViewTextBoxColumn.DataPropertyName = "IPAddress";
            this.iPAddressDataGridViewTextBoxColumn.HeaderText = "IPAddress";
            this.iPAddressDataGridViewTextBoxColumn.Name = "iPAddressDataGridViewTextBoxColumn";
            // 
            // portNoDataGridViewTextBoxColumn
            // 
            this.portNoDataGridViewTextBoxColumn.DataPropertyName = "PortNo";
            this.portNoDataGridViewTextBoxColumn.HeaderText = "PortNo";
            this.portNoDataGridViewTextBoxColumn.Name = "portNoDataGridViewTextBoxColumn";
            // 
            // channelDataGridViewTextBoxColumn
            // 
            this.channelDataGridViewTextBoxColumn.DataPropertyName = "Channel";
            this.channelDataGridViewTextBoxColumn.HeaderText = "Channel No";
            this.channelDataGridViewTextBoxColumn.Name = "channelDataGridViewTextBoxColumn";
            // 
            // gatewayIPDataGridViewTextBoxColumn
            // 
            this.gatewayIPDataGridViewTextBoxColumn.DataPropertyName = "GatewayIP";
            this.gatewayIPDataGridViewTextBoxColumn.HeaderText = "GatewayIP";
            this.gatewayIPDataGridViewTextBoxColumn.Name = "gatewayIPDataGridViewTextBoxColumn";
            // 
            // baudRateDataGridViewTextBoxColumn
            // 
            this.baudRateDataGridViewTextBoxColumn.DataPropertyName = "BaudRate";
            this.baudRateDataGridViewTextBoxColumn.HeaderText = "Baud Rate";
            this.baudRateDataGridViewTextBoxColumn.Name = "baudRateDataGridViewTextBoxColumn";
            // 
            // GroupCode
            // 
            this.GroupCode.DataPropertyName = "GroupCode";
            this.GroupCode.HeaderText = "Group Code";
            this.GroupCode.Name = "GroupCode";
            // 
            // lockerIDFromDataGridViewTextBoxColumn
            // 
            this.lockerIDFromDataGridViewTextBoxColumn.DataPropertyName = "LockerIDFrom";
            this.lockerIDFromDataGridViewTextBoxColumn.HeaderText = "Locker From";
            this.lockerIDFromDataGridViewTextBoxColumn.Name = "lockerIDFromDataGridViewTextBoxColumn";
            // 
            // lockerIDToDataGridViewTextBoxColumn
            // 
            this.lockerIDToDataGridViewTextBoxColumn.DataPropertyName = "LockerIDTo";
            this.lockerIDToDataGridViewTextBoxColumn.HeaderText = "Locker To";
            this.lockerIDToDataGridViewTextBoxColumn.Name = "lockerIDToDataGridViewTextBoxColumn";
            // 
            // dataPackingTimeDataGridViewTextBoxColumn
            // 
            this.dataPackingTimeDataGridViewTextBoxColumn.DataPropertyName = "DataPackingTime";
            this.dataPackingTimeDataGridViewTextBoxColumn.HeaderText = "Data Packing Time";
            this.dataPackingTimeDataGridViewTextBoxColumn.Name = "dataPackingTimeDataGridViewTextBoxColumn";
            // 
            // dataPackingSizeDataGridViewTextBoxColumn
            // 
            this.dataPackingSizeDataGridViewTextBoxColumn.DataPropertyName = "DataPackingSize";
            this.dataPackingSizeDataGridViewTextBoxColumn.HeaderText = "Data Packing Size";
            this.dataPackingSizeDataGridViewTextBoxColumn.Name = "dataPackingSizeDataGridViewTextBoxColumn";
            // 
            // isAliveDataGridViewCheckBoxColumn
            // 
            this.isAliveDataGridViewCheckBoxColumn.DataPropertyName = "IsAlive";
            this.isAliveDataGridViewCheckBoxColumn.HeaderText = "IsAlive?";
            this.isAliveDataGridViewCheckBoxColumn.Name = "isAliveDataGridViewCheckBoxColumn";
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "IsActive";
            this.dataGridViewCheckBoxColumn1.HeaderText = "Active?";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            // 
            // LockerAccessPointUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 343);
            this.Controls.Add(this.gpFilter);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvDisplayData);
            this.Name = "LockerAccessPointUI";
            this.Text = "Locker Access Point";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayData)).EndInit();
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lockerAccessPointDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvDisplayData; 
        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.BindingSource lockerAccessPointDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn accessPointIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn iPAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn portNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn channelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gatewayIPDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn baudRateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn GroupCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockerIDFromDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockerIDToDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataPackingTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataPackingSizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isAliveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    }
}

