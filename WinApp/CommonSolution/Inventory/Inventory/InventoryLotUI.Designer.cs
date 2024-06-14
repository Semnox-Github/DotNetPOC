namespace Semnox.Parafait.Inventory
{
    partial class InventoryLotUI
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
            this.dgvInventoryLot = new System.Windows.Forms.DataGridView();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lotIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.originalQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtUOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balanceQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbUOM = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.receivePriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expiryInDaysDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expirydateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selectExpiryDate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.inventoryLotDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSplit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dtpExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventoryLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryLotDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvInventoryLot
            // 
            this.dgvInventoryLot.AllowUserToAddRows = false;
            this.dgvInventoryLot.AllowUserToDeleteRows = false;
            this.dgvInventoryLot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvInventoryLot.AutoGenerateColumns = false;
            this.dgvInventoryLot.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventoryLot.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.isActiveDataGridViewCheckBoxColumn,
            this.lotIdDataGridViewTextBoxColumn,
            this.lotNumberDataGridViewTextBoxColumn,
            this.originalQuantityDataGridViewTextBoxColumn,
            this.txtUOM,
            this.balanceQuantityDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.cmbUOM,
            this.receivePriceDataGridViewTextBoxColumn,
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn,
            this.expiryInDaysDataGridViewTextBoxColumn,
            this.expirydateDataGridViewTextBoxColumn,
            this.selectExpiryDate});
            this.dgvInventoryLot.DataSource = this.inventoryLotDTOBindingSource;
            this.dgvInventoryLot.Location = new System.Drawing.Point(2, 0);
            this.dgvInventoryLot.Name = "dgvInventoryLot";
            this.dgvInventoryLot.Size = new System.Drawing.Size(655, 260);
            this.dgvInventoryLot.TabIndex = 0;
            this.dgvInventoryLot.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInventoryLot_CellContentClick);
            this.dgvInventoryLot.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvInventoryLot_CellFormatting);
            this.dgvInventoryLot.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvInventoryLot_ColumnHeaderMouseClick);
            this.dgvInventoryLot.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvInventoryLot_DataError);
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewCheckBoxColumn.Visible = false;
            // 
            // lotIdDataGridViewTextBoxColumn
            // 
            this.lotIdDataGridViewTextBoxColumn.DataPropertyName = "LotId";
            this.lotIdDataGridViewTextBoxColumn.HeaderText = "Lot Id";
            this.lotIdDataGridViewTextBoxColumn.Name = "lotIdDataGridViewTextBoxColumn";
            this.lotIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lotNumberDataGridViewTextBoxColumn
            // 
            this.lotNumberDataGridViewTextBoxColumn.DataPropertyName = "LotNumber";
            this.lotNumberDataGridViewTextBoxColumn.HeaderText = "Lot Number";
            this.lotNumberDataGridViewTextBoxColumn.Name = "lotNumberDataGridViewTextBoxColumn";
            this.lotNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // originalQuantityDataGridViewTextBoxColumn
            // 
            this.originalQuantityDataGridViewTextBoxColumn.DataPropertyName = "OriginalQuantity";
            this.originalQuantityDataGridViewTextBoxColumn.HeaderText = "Original Quantity";
            this.originalQuantityDataGridViewTextBoxColumn.Name = "originalQuantityDataGridViewTextBoxColumn";
            this.originalQuantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // txtUOM
            // 
            this.txtUOM.DataPropertyName = "UOM";
            this.txtUOM.HeaderText = "Stock UOM";
            this.txtUOM.Name = "txtUOM";
            this.txtUOM.ReadOnly = true;
            // 
            // balanceQuantityDataGridViewTextBoxColumn
            // 
            this.balanceQuantityDataGridViewTextBoxColumn.DataPropertyName = "BalanceQuantity";
            this.balanceQuantityDataGridViewTextBoxColumn.HeaderText = "Balance Quantity";
            this.balanceQuantityDataGridViewTextBoxColumn.Name = "balanceQuantityDataGridViewTextBoxColumn";
            this.balanceQuantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            this.quantityDataGridViewTextBoxColumn.Visible = false;
            // 
            // cmbUOM
            // 
            this.cmbUOM.HeaderText = "UOM";
            this.cmbUOM.Name = "cmbUOM";
            this.cmbUOM.ReadOnly = true;
            this.cmbUOM.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // receivePriceDataGridViewTextBoxColumn
            // 
            this.receivePriceDataGridViewTextBoxColumn.DataPropertyName = "ReceivePrice";
            this.receivePriceDataGridViewTextBoxColumn.HeaderText = "Receive Price";
            this.receivePriceDataGridViewTextBoxColumn.Name = "receivePriceDataGridViewTextBoxColumn";
            this.receivePriceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // purchaseOrderReceiveLineIdDataGridViewTextBoxColumn
            // 
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.DataPropertyName = "PurchaseOrderReceiveLineId";
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.HeaderText = "Receive Line Id";
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.Name = "purchaseOrderReceiveLineIdDataGridViewTextBoxColumn";
            this.purchaseOrderReceiveLineIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // expiryInDaysDataGridViewTextBoxColumn
            // 
            this.expiryInDaysDataGridViewTextBoxColumn.DataPropertyName = "ExpiryInDays";
            this.expiryInDaysDataGridViewTextBoxColumn.HeaderText = "Expiry In Days";
            this.expiryInDaysDataGridViewTextBoxColumn.Name = "expiryInDaysDataGridViewTextBoxColumn";
            this.expiryInDaysDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // expirydateDataGridViewTextBoxColumn
            // 
            this.expirydateDataGridViewTextBoxColumn.DataPropertyName = "Expirydate";
            this.expirydateDataGridViewTextBoxColumn.HeaderText = "Expiry date";
            this.expirydateDataGridViewTextBoxColumn.Name = "expirydateDataGridViewTextBoxColumn";
            this.expirydateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // selectExpiryDate
            // 
            this.selectExpiryDate.HeaderText = "...";
            this.selectExpiryDate.Name = "selectExpiryDate";
            this.selectExpiryDate.UseColumnTextForButtonValue = true;
            // 
            // inventoryLotDTOBindingSource
            // 
            this.inventoryLotDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.InventoryLotDTO);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(49, 218);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(164, 218);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSplit
            // 
            this.btnSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSplit.Location = new System.Drawing.Point(289, 218);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(75, 23);
            this.btnSplit.TabIndex = 3;
            this.btnSplit.Text = "Split";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Visible = false;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(399, 218);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // dtpExpiryDate
            // 
            this.dtpExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpExpiryDate.Location = new System.Drawing.Point(527, 217);
            this.dtpExpiryDate.Name = "dtpExpiryDate";
            this.dtpExpiryDate.Size = new System.Drawing.Size(21, 20);
            this.dtpExpiryDate.TabIndex = 5;
            this.dtpExpiryDate.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "LotId";
            this.dataGridViewTextBoxColumn1.HeaderText = "Lot Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "LotNumber";
            this.dataGridViewTextBoxColumn2.HeaderText = "Lot Number";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "OriginalQuantity";
            this.dataGridViewTextBoxColumn3.HeaderText = "Original Quantity";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "BalanceQuantity";
            this.dataGridViewTextBoxColumn4.HeaderText = "Balance Quantity";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Quantity";
            this.dataGridViewTextBoxColumn5.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "ReceivePrice";
            this.dataGridViewTextBoxColumn6.HeaderText = "Receive Price";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "PurchaseOrderReceiveLineId";
            this.dataGridViewTextBoxColumn7.HeaderText = "Receive Line Id";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "ExpiryInDays";
            this.dataGridViewTextBoxColumn8.HeaderText = "Expiry In Days";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Expirydate";
            this.dataGridViewTextBoxColumn9.HeaderText = "Expiry date";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // InventoryLotUI
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(657, 261);
            this.Controls.Add(this.dtpExpiryDate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSplit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgvInventoryLot);
            this.Name = "InventoryLotUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Inventory Lot";
            this.Activated += new System.EventHandler(this.InventoryLotUI_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventoryLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryLotDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvInventoryLot;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.BindingSource inventoryLotDTOBindingSource;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DateTimePicker dtpExpiryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn originalQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn balanceQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivePriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchaseOrderReceiveLineIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expiryInDaysDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expirydateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn selectExpiryDate;
    }
}

