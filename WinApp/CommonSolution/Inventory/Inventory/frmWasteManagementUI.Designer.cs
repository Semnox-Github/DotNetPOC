namespace Semnox.Parafait.Inventory
{
    partial class frmWasteManagementUI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWasteManagementUI));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvInventoryWastage = new System.Windows.Forms.DataGridView();
            this.inventoryWastageSummaryDTOBindingSource = new System.Windows.Forms.BindingSource();
            this.btnPanel = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uOMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.availableQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TodaysWastageQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbUOM = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.wastageQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbAdjustmentType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventoryId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AdjustmentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LotId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UomId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventoryWastage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryWastageSummaryDTOBindingSource)).BeginInit();
            this.btnPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvInventoryWastage
            // 
            this.dgvInventoryWastage.AllowUserToResizeColumns = false;
            this.dgvInventoryWastage.AllowUserToResizeRows = false;
            this.dgvInventoryWastage.AutoGenerateColumns = false;
            this.dgvInventoryWastage.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInventoryWastage.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvInventoryWastage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventoryWastage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productCodeDataGridViewTextBoxColumn,
            this.productDescriptionDataGridViewTextBoxColumn,
            this.productNameDataGridViewTextBoxColumn,
            this.categoryDataGridViewTextBoxColumn,
            this.uOMDataGridViewTextBoxColumn,
            this.locationIdDataGridViewComboBoxColumn,
            this.availableQuantityDataGridViewTextBoxColumn,
            this.TodaysWastageQuantityDataGridViewTextBoxColumn,
            this.cmbUOM,
            this.wastageQuantityDataGridViewTextBoxColumn,
            this.cmbAdjustmentType,
            this.remarksDataGridViewTextBoxColumn,
            this.ProductId,
            this.InventoryId,
            this.AdjustmentId,
            this.OriginalQuantity,
            this.LotId,
            this.UomId});
            this.dgvInventoryWastage.DataSource = this.inventoryWastageSummaryDTOBindingSource;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInventoryWastage.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvInventoryWastage.Location = new System.Drawing.Point(28, 38);
            this.dgvInventoryWastage.Name = "dgvInventoryWastage";
            this.dgvInventoryWastage.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvInventoryWastage.Size = new System.Drawing.Size(1085, 480);
            this.dgvInventoryWastage.TabIndex = 0;
            this.dgvInventoryWastage.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInventoryWastage_CellValueChanged);
            this.dgvInventoryWastage.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvInventoryWastage_DataError);
            this.dgvInventoryWastage.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvInventoryWastage_DefaultValuesNeeded);
            this.dgvInventoryWastage.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvInventoryWastage_RowPrePaint);
            this.dgvInventoryWastage.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvInventoryWastage_Paint);
            // 
            // inventoryWastageSummaryDTOBindingSource
            // 
            this.inventoryWastageSummaryDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.InventoryWastageSummaryDTO);
            // 
            // btnPanel
            // 
            this.btnPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPanel.Controls.Add(this.btnLoad);
            this.btnPanel.Controls.Add(this.btnRefresh);
            this.btnPanel.Controls.Add(this.btnClose);
            this.btnPanel.Controls.Add(this.btnSave);
            this.btnPanel.Location = new System.Drawing.Point(24, 543);
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Size = new System.Drawing.Size(777, 48);
            this.btnPanel.TabIndex = 2;
            // 
            // btnLoad
            // 
            this.btnLoad.Image = ((System.Drawing.Image)(resources.GetObject("btnLoad.Image")));
            this.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoad.Location = new System.Drawing.Point(395, 8);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(125, 34);
            this.btnLoad.TabIndex = 51;
            this.btnLoad.Text = "Load History";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(134, 7);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 34);
            this.btnRefresh.TabIndex = 48;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(264, 8);
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
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(28, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(634, 23);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Wastage entry details:";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn1.HeaderText = "ProductId";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 78;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ProductCode";
            this.dataGridViewTextBoxColumn2.HeaderText = "Product Code";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ProductDescription";
            this.dataGridViewTextBoxColumn3.HeaderText = "Product Description";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "WastageQuantity";
            this.dataGridViewTextBoxColumn4.HeaderText = "Wastage Quantity";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            this.dataGridViewTextBoxColumn4.Width = 120;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "AvailableQuantity";
            this.dataGridViewTextBoxColumn5.HeaderText = "Available Quantity";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            this.dataGridViewTextBoxColumn5.Width = 200;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Remarks";
            this.dataGridViewTextBoxColumn6.HeaderText = "Remarks";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn6.Visible = false;
            this.dataGridViewTextBoxColumn6.Width = 200;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn7.DataPropertyName = "LastUpdatedBy";
            this.dataGridViewTextBoxColumn7.HeaderText = "Last Updated By";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Visible = false;
            this.dataGridViewTextBoxColumn7.Width = 120;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn8.DataPropertyName = "LastUpdateDate";
            this.dataGridViewTextBoxColumn8.HeaderText = "Last Update Date";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Visible = false;
            this.dataGridViewTextBoxColumn8.Width = 150;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn9.DataPropertyName = "WastageQuantity";
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.dataGridViewTextBoxColumn9.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn9.HeaderText = "Add Wastage Quantity";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 80;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn10.DataPropertyName = "Remarks";
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn10.HeaderText = "Add Remarks";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Width = 150;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn11.HeaderText = "ProductId";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Visible = false;
            this.dataGridViewTextBoxColumn11.Width = 78;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "InventoryId";
            this.dataGridViewTextBoxColumn12.HeaderText = "InventoryId";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.Visible = false;
            this.dataGridViewTextBoxColumn12.Width = 85;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "AdjustmentId";
            this.dataGridViewTextBoxColumn13.HeaderText = "AdjustmentId";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.Visible = false;
            this.dataGridViewTextBoxColumn13.Width = 93;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "OriginalQuantity";
            this.dataGridViewTextBoxColumn14.HeaderText = "OriginalQuantity";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.Visible = false;
            this.dataGridViewTextBoxColumn14.Width = 106;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "LotId";
            this.dataGridViewTextBoxColumn15.HeaderText = "LotId";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.Visible = false;
            this.dataGridViewTextBoxColumn15.Width = 56;
            // 
            // productCodeDataGridViewTextBoxColumn
            // 
            this.productCodeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.productCodeDataGridViewTextBoxColumn.DataPropertyName = "ProductCode";
            this.productCodeDataGridViewTextBoxColumn.HeaderText = "Product Code";
            this.productCodeDataGridViewTextBoxColumn.Name = "productCodeDataGridViewTextBoxColumn";
            // 
            // productDescriptionDataGridViewTextBoxColumn
            // 
            this.productDescriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.productDescriptionDataGridViewTextBoxColumn.DataPropertyName = "ProductDescription";
            this.productDescriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.productDescriptionDataGridViewTextBoxColumn.Name = "productDescriptionDataGridViewTextBoxColumn";
            this.productDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.productDescriptionDataGridViewTextBoxColumn.Width = 85;
            // 
            // productNameDataGridViewTextBoxColumn
            // 
            this.productNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.productNameDataGridViewTextBoxColumn.DataPropertyName = "ProductName";
            this.productNameDataGridViewTextBoxColumn.HeaderText = "Product Name";
            this.productNameDataGridViewTextBoxColumn.Name = "productNameDataGridViewTextBoxColumn";
            this.productNameDataGridViewTextBoxColumn.Visible = false;
            this.productNameDataGridViewTextBoxColumn.Width = 60;
            // 
            // categoryDataGridViewTextBoxColumn
            // 
            this.categoryDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.categoryDataGridViewTextBoxColumn.DataPropertyName = "Category";
            this.categoryDataGridViewTextBoxColumn.HeaderText = "Category";
            this.categoryDataGridViewTextBoxColumn.Name = "categoryDataGridViewTextBoxColumn";
            this.categoryDataGridViewTextBoxColumn.ReadOnly = true;
            this.categoryDataGridViewTextBoxColumn.Width = 120;
            // 
            // uOMDataGridViewTextBoxColumn
            // 
            this.uOMDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.uOMDataGridViewTextBoxColumn.DataPropertyName = "UOM";
            this.uOMDataGridViewTextBoxColumn.HeaderText = "Stock UOM";
            this.uOMDataGridViewTextBoxColumn.Name = "uOMDataGridViewTextBoxColumn";
            this.uOMDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // locationIdDataGridViewComboBoxColumn
            // 
            this.locationIdDataGridViewComboBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.locationIdDataGridViewComboBoxColumn.DataPropertyName = "LocationName";
            this.locationIdDataGridViewComboBoxColumn.HeaderText = "Location Name";
            this.locationIdDataGridViewComboBoxColumn.Name = "locationIdDataGridViewComboBoxColumn";
            this.locationIdDataGridViewComboBoxColumn.ReadOnly = true;
            this.locationIdDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.locationIdDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.locationIdDataGridViewComboBoxColumn.Width = 120;
            // 
            // availableQuantityDataGridViewTextBoxColumn
            // 
            this.availableQuantityDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.availableQuantityDataGridViewTextBoxColumn.DataPropertyName = "AvailableQuantity";
            this.availableQuantityDataGridViewTextBoxColumn.HeaderText = "Available Quantity";
            this.availableQuantityDataGridViewTextBoxColumn.Name = "availableQuantityDataGridViewTextBoxColumn";
            this.availableQuantityDataGridViewTextBoxColumn.ReadOnly = true;
            this.availableQuantityDataGridViewTextBoxColumn.Width = 80;
            // 
            // TodaysWastageQuantityDataGridViewTextBoxColumn
            // 
            this.TodaysWastageQuantityDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TodaysWastageQuantityDataGridViewTextBoxColumn.DataPropertyName = "TodaysWastageQuantity";
            this.TodaysWastageQuantityDataGridViewTextBoxColumn.HeaderText = "Updated Wastage Quantity";
            this.TodaysWastageQuantityDataGridViewTextBoxColumn.Name = "TodaysWastageQuantityDataGridViewTextBoxColumn";
            this.TodaysWastageQuantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cmbUOM
            // 
            this.cmbUOM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cmbUOM.HeaderText = "UOM";
            this.cmbUOM.Name = "cmbUOM";
            this.cmbUOM.Width = 80;
            // 
            // wastageQuantityDataGridViewTextBoxColumn
            // 
            this.wastageQuantityDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.wastageQuantityDataGridViewTextBoxColumn.DataPropertyName = "WastageQuantity";
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.wastageQuantityDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.wastageQuantityDataGridViewTextBoxColumn.HeaderText = "Add Wastage Quantity";
            this.wastageQuantityDataGridViewTextBoxColumn.Name = "wastageQuantityDataGridViewTextBoxColumn";
            this.wastageQuantityDataGridViewTextBoxColumn.Width = 80;
            // 
            // cmbAdjustmentType
            // 
            this.cmbAdjustmentType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.cmbAdjustmentType.DataPropertyName = "AdjustementTypeId";
            this.cmbAdjustmentType.HeaderText = "Wastage Type";
            this.cmbAdjustmentType.Name = "cmbAdjustmentType";
            this.cmbAdjustmentType.Width = 75;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.remarksDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Add Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            this.remarksDataGridViewTextBoxColumn.Width = 150;
            // 
            // ProductId
            // 
            this.ProductId.DataPropertyName = "ProductId";
            this.ProductId.HeaderText = "ProductId";
            this.ProductId.Name = "ProductId";
            this.ProductId.Visible = false;
            this.ProductId.Width = 78;
            // 
            // InventoryId
            // 
            this.InventoryId.DataPropertyName = "InventoryId";
            this.InventoryId.HeaderText = "InventoryId";
            this.InventoryId.Name = "InventoryId";
            this.InventoryId.Visible = false;
            this.InventoryId.Width = 85;
            // 
            // AdjustmentId
            // 
            this.AdjustmentId.DataPropertyName = "AdjustmentId";
            this.AdjustmentId.HeaderText = "AdjustmentId";
            this.AdjustmentId.Name = "AdjustmentId";
            this.AdjustmentId.Visible = false;
            this.AdjustmentId.Width = 93;
            // 
            // OriginalQuantity
            // 
            this.OriginalQuantity.DataPropertyName = "OriginalQuantity";
            this.OriginalQuantity.HeaderText = "OriginalQuantity";
            this.OriginalQuantity.Name = "OriginalQuantity";
            this.OriginalQuantity.Visible = false;
            this.OriginalQuantity.Width = 106;
            // 
            // LotId
            // 
            this.LotId.DataPropertyName = "LotId";
            this.LotId.HeaderText = "LotId";
            this.LotId.Name = "LotId";
            this.LotId.Visible = false;
            this.LotId.Width = 56;
            // 
            // UomId
            // 
            this.UomId.DataPropertyName = "UomId";
            this.UomId.HeaderText = "UomId";
            this.UomId.Name = "UomId";
            this.UomId.Visible = false;
            this.UomId.Width = 63;
            // 
            // frmWasteManagementUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1146, 594);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnPanel);
            this.Controls.Add(this.dgvInventoryWastage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWasteManagementUI";
            this.Text = "Wastage Management";
            this.Activated += new System.EventHandler(this.frmWasteManagementUI_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventoryWastage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryWastageSummaryDTOBindingSource)).EndInit();
            this.btnPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvInventoryWastage;
        private System.Windows.Forms.Panel btnPanel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.BindingSource inventoryWastageSummaryDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn productCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uOMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn locationIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn availableQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TodaysWastageQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn wastageQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbAdjustmentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn InventoryId;
        private System.Windows.Forms.DataGridViewTextBoxColumn AdjustmentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn LotId;
        private System.Windows.Forms.DataGridViewTextBoxColumn UomId;
    }
}