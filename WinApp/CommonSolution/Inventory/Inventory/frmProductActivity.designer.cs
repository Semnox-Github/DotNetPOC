namespace Semnox.Parafait.Inventory
{
    partial class frmProductActivity
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
            this.dgvProductActivity = new System.Windows.Forms.DataGridView();
            this.productActivityViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.locationIdLocationDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.transferLocationLocationDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.lblHeading = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.transferLocationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.trxTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Trx_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtUOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductActivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productActivityViewBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationIdLocationDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transferLocationLocationDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvProductActivity
            // 
            this.dgvProductActivity.AllowUserToAddRows = false;
            this.dgvProductActivity.AllowUserToDeleteRows = false;
            this.dgvProductActivity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProductActivity.AutoGenerateColumns = false;
            this.dgvProductActivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductActivity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.locationIdDataGridViewTextBoxColumn,
            this.transferLocationIdDataGridViewTextBoxColumn,
            this.trxTypeDataGridViewTextBoxColumn,
            this.Trx_Date,
            this.productIdDataGridViewTextBoxColumn,
            this.Quantity,
            this.txtUOM,
            this.userNameDataGridViewTextBoxColumn});
            this.dgvProductActivity.DataSource = this.productActivityViewBindingSource;
            this.dgvProductActivity.Location = new System.Drawing.Point(12, 39);
            this.dgvProductActivity.Name = "dgvProductActivity";
            this.dgvProductActivity.ReadOnly = true;
            this.dgvProductActivity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductActivity.Size = new System.Drawing.Size(647, 419);
            this.dgvProductActivity.TabIndex = 0;
            this.dgvProductActivity.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProductActivity_CellFormatting);
            // 
            // productActivityViewBindingSource
            // 
            this.productActivityViewBindingSource.DataSource = typeof(Semnox.Parafait.Product.ProductActivityViewDTO);
            // 
            // locationIdLocationDTOBindingSource
            // 
            this.locationIdLocationDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.LocationDTO);
            // 
            // transferLocationLocationDTOBindingSource
            // 
            this.transferLocationLocationDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.LocationDTO);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(435, 470);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(94, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.Location = new System.Drawing.Point(11, 13);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(84, 16);
            this.lblHeading.TabIndex = 2;
            this.lblHeading.Text = "lblHeading";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(291, 470);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(94, 23);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(147, 470);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(94, 23);
            this.btnExportExcel.TabIndex = 4;
            this.btnExportExcel.Text = "Export To Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Trx_Type";
            this.dataGridViewTextBoxColumn1.HeaderText = "Trx Type";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TimeStamp";
            this.dataGridViewTextBoxColumn2.HeaderText = "Trx_Date";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn3.HeaderText = "Product Id";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Quantity";
            this.dataGridViewTextBoxColumn4.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "UOM";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "UserName";
            this.dataGridViewTextBoxColumn6.HeaderText = "User Name";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // locationIdDataGridViewTextBoxColumn
            // 
            this.locationIdDataGridViewTextBoxColumn.DataPropertyName = "LocationId";
            this.locationIdDataGridViewTextBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.locationIdDataGridViewTextBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.locationIdDataGridViewTextBoxColumn.HeaderText = "Location";
            this.locationIdDataGridViewTextBoxColumn.Name = "locationIdDataGridViewTextBoxColumn";
            this.locationIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.locationIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.locationIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // transferLocationIdDataGridViewTextBoxColumn
            // 
            this.transferLocationIdDataGridViewTextBoxColumn.DataPropertyName = "TransferLocationId";
            this.transferLocationIdDataGridViewTextBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.transferLocationIdDataGridViewTextBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.transferLocationIdDataGridViewTextBoxColumn.HeaderText = "Transfer Location";
            this.transferLocationIdDataGridViewTextBoxColumn.Name = "transferLocationIdDataGridViewTextBoxColumn";
            this.transferLocationIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.transferLocationIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.transferLocationIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // trxTypeDataGridViewTextBoxColumn
            // 
            this.trxTypeDataGridViewTextBoxColumn.DataPropertyName = "Trx_Type";
            this.trxTypeDataGridViewTextBoxColumn.HeaderText = "Trx Type";
            this.trxTypeDataGridViewTextBoxColumn.Name = "trxTypeDataGridViewTextBoxColumn";
            this.trxTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Trx_Date
            // 
            this.Trx_Date.DataPropertyName = "TimeStamp";
            this.Trx_Date.HeaderText = "Trx_Date";
            this.Trx_Date.Name = "Trx_Date";
            this.Trx_Date.ReadOnly = true;
            // 
            // productIdDataGridViewTextBoxColumn
            // 
            this.productIdDataGridViewTextBoxColumn.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn.HeaderText = "Product Id";
            this.productIdDataGridViewTextBoxColumn.Name = "productIdDataGridViewTextBoxColumn";
            this.productIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.productIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "Quantity";
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            // 
            // txtUOM
            // 
            this.txtUOM.HeaderText = "UOM";
            this.txtUOM.Name = "txtUOM";
            this.txtUOM.ReadOnly = true;
            this.txtUOM.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // userNameDataGridViewTextBoxColumn
            // 
            this.userNameDataGridViewTextBoxColumn.DataPropertyName = "UserName";
            this.userNameDataGridViewTextBoxColumn.HeaderText = "User Name";
            this.userNameDataGridViewTextBoxColumn.Name = "userNameDataGridViewTextBoxColumn";
            this.userNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // frmProductActivity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(671, 505);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvProductActivity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmProductActivity";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Product Activity";
            this.Load += new System.EventHandler(this.frmProductActivity_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductActivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productActivityViewBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationIdLocationDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transferLocationLocationDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvProductActivity;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.BindingSource productActivityViewBindingSource;
        private System.Windows.Forms.BindingSource locationIdLocationDTOBindingSource;
        private System.Windows.Forms.BindingSource transferLocationLocationDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewComboBoxColumn locationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn transferLocationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Trx_Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn;
    }
}