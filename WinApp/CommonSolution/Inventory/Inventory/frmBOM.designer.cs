using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Required Designer Variables
    /// </summary>
    partial class frmBOM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBOM));
            this.BOMEditMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.Add = new System.Windows.Forms.ToolStripMenuItem();
            this.Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblEditLabel = new System.Windows.Forms.Label();
            this.lblProduct = new System.Windows.Forms.Label();
            this.txtTotalRecipeCost = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvBOM = new System.Windows.Forms.DataGridView();
            this.BOMDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
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
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.productDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.itemNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbUOM = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.costDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecipeCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stockDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Isactive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.preparationRemarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.preparationOffsetMinsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bOMIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.childProductIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uOMIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BOMEditMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BOMDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // BOMEditMenu
            // 
            this.BOMEditMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Edit,
            this.Add,
            this.Remove,
            this.toolStripSeparator1,
            this.menuRefresh});
            this.BOMEditMenu.Name = "BOMEditMenu";
            this.BOMEditMenu.Size = new System.Drawing.Size(140, 98);
            // 
            // Edit
            // 
            this.Edit.Name = "Edit";
            this.Edit.Size = new System.Drawing.Size(139, 22);
            this.Edit.Text = "Edit Product";
            // 
            // Add
            // 
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(139, 22);
            this.Add.Text = "Add Child";
            // 
            // Remove
            // 
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(139, 22);
            this.Remove.Text = "Remove";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
            // 
            // menuRefresh
            // 
            this.menuRefresh.Name = "menuRefresh";
            this.menuRefresh.Size = new System.Drawing.Size(139, 22);
            this.menuRefresh.Text = "Refresh";
            // 
            // btnExit
            // 
            this.btnExit.CausesValidation = false;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(343, 522);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(78, 23);
            this.btnExit.TabIndex = 49;
            this.btnExit.Text = "Close";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(20, 522);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 23);
            this.btnSave.TabIndex = 50;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblEditLabel
            // 
            this.lblEditLabel.AutoSize = true;
            this.lblEditLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEditLabel.ForeColor = System.Drawing.Color.DimGray;
            this.lblEditLabel.Location = new System.Drawing.Point(17, 9);
            this.lblEditLabel.Name = "lblEditLabel";
            this.lblEditLabel.Size = new System.Drawing.Size(133, 16);
            this.lblEditLabel.TabIndex = 53;
            this.lblEditLabel.Text = "Bill of Material for:";
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProduct.Location = new System.Drawing.Point(184, 9);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(61, 16);
            this.lblProduct.TabIndex = 54;
            this.lblProduct.Text = "Product";
            this.lblProduct.Visible = false;
            // 
            // txtTotalRecipeCost
            // 
            this.txtTotalRecipeCost.Location = new System.Drawing.Point(603, 9);
            this.txtTotalRecipeCost.Name = "txtTotalRecipeCost";
            this.txtTotalRecipeCost.ReadOnly = true;
            this.txtTotalRecipeCost.Size = new System.Drawing.Size(92, 20);
            this.txtTotalRecipeCost.TabIndex = 112;
            this.txtTotalRecipeCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(511, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 111;
            this.label4.Text = "Recipe Cost:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvBOM
            // 
            this.dgvBOM.AutoGenerateColumns = false;
            this.dgvBOM.BackgroundColor = System.Drawing.Color.DarkSeaGreen;
            this.dgvBOM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBOM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemNameDataGridViewTextBoxColumn,
            this.itemTypeDataGridViewTextBoxColumn,
            this.UOM,
            this.quantityDataGridViewTextBoxColumn1,
            this.cmbUOM,
            this.costDataGridViewTextBoxColumn,
            this.RecipeCost,
            this.stockDataGridViewTextBoxColumn,
            this.Isactive,
            this.preparationRemarksDataGridViewTextBoxColumn,
            this.preparationOffsetMinsDataGridViewTextBoxColumn,
            this.CreatedBy,
            this.CreationDate,
            this.LastUpdatedBy,
            this.LastUpdateDate,
            this.bOMIdDataGridViewTextBoxColumn1,
            this.productIdDataGridViewTextBoxColumn1,
            this.childProductIdDataGridViewTextBoxColumn1,
            this.uOMIdDataGridViewTextBoxColumn});
            this.dgvBOM.DataSource = this.BOMDTOBindingSource;
            this.dgvBOM.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dgvBOM.Location = new System.Drawing.Point(20, 44);
            this.dgvBOM.Name = "dgvBOM";
            this.dgvBOM.Size = new System.Drawing.Size(957, 461);
            this.dgvBOM.TabIndex = 55;
            this.dgvBOM.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBOM_CellClick);
            this.dgvBOM.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBOM_CellValueChanged);
            this.dgvBOM.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBOM_ColumnHeaderMouseClick);
            this.dgvBOM.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvBOM_DataError);
            // 
            // BOMDTOBindingSource
            // 
            this.BOMDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.BOMDTO);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(228, 522);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 23);
            this.btnDelete.TabIndex = 113;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(9, 562);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(977, 20);
            this.lblMessage.TabIndex = 114;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "BOMId";
            this.dataGridViewTextBoxColumn1.HeaderText = "BOMId";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 20;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ItemName";
            this.dataGridViewTextBoxColumn2.HeaderText = "Description";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 125;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Quantity";
            this.dataGridViewTextBoxColumn3.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 50;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Quantity";
            this.dataGridViewTextBoxColumn4.HeaderText = "UOM";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 50;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn5.HeaderText = "ProductId";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "RecipeCost";
            this.dataGridViewTextBoxColumn6.HeaderText = "Item Cost";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "Stock";
            this.dataGridViewTextBoxColumn7.HeaderText = "Recipe Cost";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "BOMId";
            this.dataGridViewTextBoxColumn8.HeaderText = "Stock";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn9.HeaderText = "ProductId";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "BOMId";
            this.dataGridViewTextBoxColumn10.HeaderText = "Description";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Visible = false;
            this.dataGridViewTextBoxColumn10.Width = 125;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn11.HeaderText = "Recipe Description";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "PreparationOffsetMins";
            this.dataGridViewTextBoxColumn12.HeaderText = "PreparationOffsetMinutes";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "UOMId";
            this.dataGridViewTextBoxColumn13.HeaderText = "UOMId";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Visible = false;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn14.HeaderText = "ProductId";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.Visible = false;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "ChildProductId";
            this.dataGridViewTextBoxColumn15.HeaderText = "ChildProductId";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.Visible = false;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "UOMId";
            this.dataGridViewTextBoxColumn16.HeaderText = "UOMId";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            this.dataGridViewTextBoxColumn16.Visible = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(112, 522);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 23);
            this.btnRefresh.TabIndex = 115;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // productDTOBindingSource
            // 
            this.productDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.ProductDTO);
            // 
            // itemNameDataGridViewTextBoxColumn
            // 
            this.itemNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.itemNameDataGridViewTextBoxColumn.DataPropertyName = "ItemName";
            this.itemNameDataGridViewTextBoxColumn.HeaderText = "Item Name";
            this.itemNameDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.itemNameDataGridViewTextBoxColumn.Name = "itemNameDataGridViewTextBoxColumn";
            this.itemNameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // itemTypeDataGridViewTextBoxColumn
            // 
            this.itemTypeDataGridViewTextBoxColumn.DataPropertyName = "ItemType";
            this.itemTypeDataGridViewTextBoxColumn.HeaderText = "Item Type";
            this.itemTypeDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.itemTypeDataGridViewTextBoxColumn.Name = "itemTypeDataGridViewTextBoxColumn";
            this.itemTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // UOM
            // 
            this.UOM.DataPropertyName = "UOM";
            this.UOM.HeaderText = "Stock UOM";
            this.UOM.MinimumWidth = 100;
            this.UOM.Name = "UOM";
            this.UOM.ReadOnly = true;
            // 
            // quantityDataGridViewTextBoxColumn1
            // 
            this.quantityDataGridViewTextBoxColumn1.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn1.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn1.MinimumWidth = 100;
            this.quantityDataGridViewTextBoxColumn1.Name = "quantityDataGridViewTextBoxColumn1";
            // 
            // cmbUOM
            // 
            this.cmbUOM.HeaderText = "BOM UOM";
            this.cmbUOM.MinimumWidth = 100;
            this.cmbUOM.Name = "cmbUOM";
            // 
            // costDataGridViewTextBoxColumn
            // 
            this.costDataGridViewTextBoxColumn.DataPropertyName = "Cost";
            this.costDataGridViewTextBoxColumn.HeaderText = "Item Cost";
            this.costDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.costDataGridViewTextBoxColumn.Name = "costDataGridViewTextBoxColumn";
            this.costDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // RecipeCost
            // 
            this.RecipeCost.DataPropertyName = "RecipeCost";
            this.RecipeCost.HeaderText = "Recipe Cost";
            this.RecipeCost.MinimumWidth = 100;
            this.RecipeCost.Name = "RecipeCost";
            this.RecipeCost.ReadOnly = true;
            // 
            // stockDataGridViewTextBoxColumn
            // 
            this.stockDataGridViewTextBoxColumn.DataPropertyName = "Stock";
            this.stockDataGridViewTextBoxColumn.HeaderText = "Stock";
            this.stockDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.stockDataGridViewTextBoxColumn.Name = "stockDataGridViewTextBoxColumn";
            this.stockDataGridViewTextBoxColumn.ReadOnly = true;
            this.stockDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.stockDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Isactive
            // 
            this.Isactive.DataPropertyName = "Isactive";
            this.Isactive.HeaderText = "Is Active";
            this.Isactive.Name = "Isactive";
            // 
            // preparationRemarksDataGridViewTextBoxColumn
            // 
            this.preparationRemarksDataGridViewTextBoxColumn.DataPropertyName = "PreparationRemarks";
            this.preparationRemarksDataGridViewTextBoxColumn.HeaderText = "Preparation Remarks";
            this.preparationRemarksDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.preparationRemarksDataGridViewTextBoxColumn.Name = "preparationRemarksDataGridViewTextBoxColumn";
            // 
            // preparationOffsetMinsDataGridViewTextBoxColumn
            // 
            this.preparationOffsetMinsDataGridViewTextBoxColumn.DataPropertyName = "PreparationOffsetMins";
            this.preparationOffsetMinsDataGridViewTextBoxColumn.HeaderText = "Preparation Offset (Minutes)";
            this.preparationOffsetMinsDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.preparationOffsetMinsDataGridViewTextBoxColumn.Name = "preparationOffsetMinsDataGridViewTextBoxColumn";
            // 
            // CreatedBy
            // 
            this.CreatedBy.DataPropertyName = "CreatedBy";
            this.CreatedBy.HeaderText = "Created By";
            this.CreatedBy.Name = "CreatedBy";
            this.CreatedBy.ReadOnly = true;
            // 
            // CreationDate
            // 
            this.CreationDate.DataPropertyName = "CreationDate";
            this.CreationDate.HeaderText = "Creation Date";
            this.CreationDate.Name = "CreationDate";
            this.CreationDate.ReadOnly = true;
            // 
            // LastUpdatedBy
            // 
            this.LastUpdatedBy.DataPropertyName = "LastUpdatedBy";
            this.LastUpdatedBy.HeaderText = "Last Updated By";
            this.LastUpdatedBy.Name = "LastUpdatedBy";
            this.LastUpdatedBy.ReadOnly = true;
            // 
            // LastUpdateDate
            // 
            this.LastUpdateDate.DataPropertyName = "LastUpdateDate";
            this.LastUpdateDate.HeaderText = "Last Updated Date";
            this.LastUpdateDate.Name = "LastUpdateDate";
            this.LastUpdateDate.ReadOnly = true;
            // 
            // bOMIdDataGridViewTextBoxColumn1
            // 
            this.bOMIdDataGridViewTextBoxColumn1.DataPropertyName = "BOMId";
            this.bOMIdDataGridViewTextBoxColumn1.HeaderText = "BOMId";
            this.bOMIdDataGridViewTextBoxColumn1.Name = "bOMIdDataGridViewTextBoxColumn1";
            this.bOMIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.bOMIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // productIdDataGridViewTextBoxColumn1
            // 
            this.productIdDataGridViewTextBoxColumn1.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn1.HeaderText = "ProductId";
            this.productIdDataGridViewTextBoxColumn1.Name = "productIdDataGridViewTextBoxColumn1";
            this.productIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // childProductIdDataGridViewTextBoxColumn1
            // 
            this.childProductIdDataGridViewTextBoxColumn1.DataPropertyName = "ChildProductId";
            this.childProductIdDataGridViewTextBoxColumn1.HeaderText = "ChildProductId";
            this.childProductIdDataGridViewTextBoxColumn1.Name = "childProductIdDataGridViewTextBoxColumn1";
            this.childProductIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // uOMIdDataGridViewTextBoxColumn
            // 
            this.uOMIdDataGridViewTextBoxColumn.DataPropertyName = "UOMId";
            this.uOMIdDataGridViewTextBoxColumn.HeaderText = "UOMId";
            this.uOMIdDataGridViewTextBoxColumn.Name = "uOMIdDataGridViewTextBoxColumn";
            this.uOMIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.uOMIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // frmBOM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(998, 591);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.txtTotalRecipeCost);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvBOM);
            this.Controls.Add(this.lblEditLabel);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmBOM";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bill of Material";
            this.Activated += new System.EventHandler(this.frmBOM_Activated);
            this.Load += new System.EventHandler(this.frmBOM_Load);
            this.BOMEditMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BOMDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip BOMEditMenu;
        private System.Windows.Forms.ToolStripMenuItem Add;
        private System.Windows.Forms.ToolStripMenuItem Edit;
        private System.Windows.Forms.ToolStripMenuItem Remove;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblEditLabel;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuRefresh;
        private System.Windows.Forms.BindingSource productDTOBindingSource;
        private System.Windows.Forms.BindingSource BOMDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.TextBox txtTotalRecipeCost;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridView dgvBOM;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn costDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecipeCost;
        private System.Windows.Forms.DataGridViewLinkColumn stockDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Isactive;
        private System.Windows.Forms.DataGridViewTextBoxColumn preparationRemarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn preparationOffsetMinsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn bOMIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn childProductIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn uOMIdDataGridViewTextBoxColumn;
    }

}