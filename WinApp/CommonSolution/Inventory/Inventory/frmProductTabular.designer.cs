using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory
{
    partial class frmProductTabular
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
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.SetProducts = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.productIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BarCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductBarCode = new System.Windows.Forms.DataGridViewButtonColumn();
            this.categoryIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TurnInPriceInTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LowerLimitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uomIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cmbItemType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.btnRecipeDescription = new System.Windows.Forms.DataGridViewButtonColumn();
            this.priceInTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.autoUpdateMarkupDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CostVariancePercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.defaultLocationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.outboundLocationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.defaultVendorIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.reorderPointDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpperLimitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SegmentCategoryId = new System.Windows.Forms.DataGridViewButtonColumn();
            this.CustomDataSetId = new System.Windows.Forms.DataGridViewButtonColumn();
            this.reorderQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.innerPackQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.salePriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastPurchasePriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemMarkupPercentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isRedeemableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isSellableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isPurchaseableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cbxIncludeInPlan = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.taxIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.taxInclusiveCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pictureDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.upperLimitCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterPackQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lowerLimitCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costVariancePercentageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YieldPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.turnInPriceInTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lotControlledDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.marketListItemDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.expiryTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.issuingApproachDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.uOMValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expiryDaysDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customDataSetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.segmentCategoryIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostIncludeTaxDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cmbUOM = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.PreparationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.cmbProductType = new System.Windows.Forms.ComboBox();
            this.cmbDisplayGroup = new System.Windows.Forms.ComboBox();
            this.cmbProductCategory = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.lblVendor = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblDisplayGroup = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblProductType = new System.Windows.Forms.Label();
            this.lnkPublishToSite = new System.Windows.Forms.LinkLabel();
            this.cmbVendor = new System.Windows.Forms.ComboBox();
            this.btnAdvancedSearch = new System.Windows.Forms.Button();
            this.lblActive = new System.Windows.Forms.Label();
            this.cmbActive = new System.Windows.Forms.ComboBox();
            this.cbxLot = new System.Windows.Forms.CheckBox();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productDTOBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToResizeRows = false;
            this.dgvProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProducts.AutoGenerateColumns = false;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SetProducts,
            this.productIdDataGridViewTextBoxColumn,
            this.codeDataGridViewTextBoxColumn,
            this.productNameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.BarCode,
            this.ProductBarCode,
            this.categoryIdDataGridViewTextBoxColumn,
            this.TurnInPriceInTickets,
            this.LowerLimitCost,
            this.uomIdDataGridViewTextBoxColumn,
            this.cmbItemType,
            this.btnRecipeDescription,
            this.priceInTicketsDataGridViewTextBoxColumn,
            this.autoUpdateMarkupDataGridViewCheckBoxColumn,
            this.CostVariancePercentage,
            this.defaultLocationIdDataGridViewTextBoxColumn,
            this.outboundLocationIdDataGridViewTextBoxColumn,
            this.defaultVendorIdDataGridViewTextBoxColumn,
            this.reorderPointDataGridViewTextBoxColumn,
            this.UpperLimitCost,
            this.SegmentCategoryId,
            this.CustomDataSetId,
            this.reorderQuantityDataGridViewTextBoxColumn,
            this.innerPackQtyDataGridViewTextBoxColumn,
            this.salePriceDataGridViewTextBoxColumn,
            this.costDataGridViewTextBoxColumn,
            this.lastPurchasePriceDataGridViewTextBoxColumn,
            this.itemMarkupPercentDataGridViewTextBoxColumn,
            this.isRedeemableDataGridViewTextBoxColumn,
            this.isSellableDataGridViewTextBoxColumn,
            this.isPurchaseableDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn,
            this.cbxIncludeInPlan,
            this.taxIdDataGridViewTextBoxColumn,
            this.taxInclusiveCostDataGridViewTextBoxColumn,
            this.pictureDataGridViewImageColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.upperLimitCostDataGridViewTextBoxColumn,
            this.masterPackQtyDataGridViewTextBoxColumn,
            this.imageFileNameDataGridViewTextBoxColumn,
            this.lowerLimitCostDataGridViewTextBoxColumn,
            this.costVariancePercentageDataGridViewTextBoxColumn,
            this.YieldPercentage,
            this.turnInPriceInTicketsDataGridViewTextBoxColumn,
            this.lotControlledDataGridViewCheckBoxColumn,
            this.marketListItemDataGridViewCheckBoxColumn,
            this.expiryTypeDataGridViewTextBoxColumn,
            this.issuingApproachDataGridViewTextBoxColumn,
            this.uOMValueDataGridViewTextBoxColumn,
            this.expiryDaysDataGridViewTextBoxColumn,
            this.customDataSetIdDataGridViewTextBoxColumn,
            this.segmentCategoryIdDataGridViewTextBoxColumn,
            this.CostIncludeTaxDataGridViewCheckBoxColumn,
            this.cmbUOM,
            this.PreparationTime});
            this.dgvProducts.DataSource = this.productDTOBindingSource;
            this.dgvProducts.Location = new System.Drawing.Point(12, 125);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.Size = new System.Drawing.Size(1015, 334);
            this.dgvProducts.TabIndex = 0;
            this.dgvProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellClick);
            this.dgvProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellContentClick);
            this.dgvProducts.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellEndEdit);
            this.dgvProducts.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellEnter);
            this.dgvProducts.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProducts_CellFormat);
            this.dgvProducts.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellLeave);
            this.dgvProducts.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProducts_ColumnHeaderMouseClick);
            this.dgvProducts.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvProducts_DataError);
            this.dgvProducts.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvProducts_DefaultValuesNeeded);
            this.dgvProducts.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dgvProducts_RowStateChanged);
            this.dgvProducts.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvProducts_RowValidating);
            this.dgvProducts.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridViewProduct_Scroll);
            // 
            // SetProducts
            // 
            this.SetProducts.Frozen = true;
            this.SetProducts.HeaderText = "";
            this.SetProducts.MinimumWidth = 50;
            this.SetProducts.Name = "SetProducts";
            this.SetProducts.ReadOnly = true;
            this.SetProducts.Width = 60;
            // 
            // productIdDataGridViewTextBoxColumn
            // 
            this.productIdDataGridViewTextBoxColumn.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn.Frozen = true;
            this.productIdDataGridViewTextBoxColumn.HeaderText = "Id";
            this.productIdDataGridViewTextBoxColumn.Name = "productIdDataGridViewTextBoxColumn";
            this.productIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // codeDataGridViewTextBoxColumn
            // 
            this.codeDataGridViewTextBoxColumn.DataPropertyName = "Code";
            this.codeDataGridViewTextBoxColumn.Frozen = true;
            this.codeDataGridViewTextBoxColumn.HeaderText = "Code";
            this.codeDataGridViewTextBoxColumn.Name = "codeDataGridViewTextBoxColumn";
            // 
            // productNameDataGridViewTextBoxColumn
            // 
            this.productNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.productNameDataGridViewTextBoxColumn.DataPropertyName = "ProductName";
            this.productNameDataGridViewTextBoxColumn.Frozen = true;
            this.productNameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.productNameDataGridViewTextBoxColumn.Name = "productNameDataGridViewTextBoxColumn";
            this.productNameDataGridViewTextBoxColumn.Width = 60;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.Frozen = true;
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.Width = 85;
            // 
            // BarCode
            // 
            this.BarCode.Frozen = true;
            this.BarCode.HeaderText = "BarCode";
            this.BarCode.Name = "BarCode";
            this.BarCode.ReadOnly = true;
            // 
            // ProductBarCode
            // 
            this.ProductBarCode.HeaderText = "";
            this.ProductBarCode.Name = "ProductBarCode";
            this.ProductBarCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ProductBarCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ProductBarCode.Text = "...";
            this.ProductBarCode.UseColumnTextForButtonValue = true;
            // 
            // categoryIdDataGridViewTextBoxColumn
            // 
            this.categoryIdDataGridViewTextBoxColumn.DataPropertyName = "CategoryId";
            this.categoryIdDataGridViewTextBoxColumn.HeaderText = "Category";
            this.categoryIdDataGridViewTextBoxColumn.Name = "categoryIdDataGridViewTextBoxColumn";
            this.categoryIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.categoryIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // TurnInPriceInTickets
            // 
            this.TurnInPriceInTickets.DataPropertyName = "TurnInPriceInTickets";
            this.TurnInPriceInTickets.HeaderText = "Turn-In PIT";
            this.TurnInPriceInTickets.Name = "TurnInPriceInTickets";
            // 
            // LowerLimitCost
            // 
            this.LowerLimitCost.DataPropertyName = "LowerLimitCost";
            this.LowerLimitCost.HeaderText = "Lower Limit Cost";
            this.LowerLimitCost.Name = "LowerLimitCost";
            // 
            // uomIdDataGridViewTextBoxColumn
            // 
            this.uomIdDataGridViewTextBoxColumn.DataPropertyName = "UomId";
            this.uomIdDataGridViewTextBoxColumn.HeaderText = "Uom";
            this.uomIdDataGridViewTextBoxColumn.Name = "uomIdDataGridViewTextBoxColumn";
            this.uomIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.uomIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // cmbItemType
            // 
            this.cmbItemType.DataPropertyName = "ItemType";
            this.cmbItemType.HeaderText = "ItemType";
            this.cmbItemType.Name = "cmbItemType";
            // 
            // btnRecipeDescription
            // 
            this.btnRecipeDescription.DataPropertyName = "RecipeDescription";
            this.btnRecipeDescription.HeaderText = "RecipeDescription";
            this.btnRecipeDescription.Name = "btnRecipeDescription";
            this.btnRecipeDescription.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // priceInTicketsDataGridViewTextBoxColumn
            // 
            this.priceInTicketsDataGridViewTextBoxColumn.DataPropertyName = "PriceInTickets";
            this.priceInTicketsDataGridViewTextBoxColumn.HeaderText = "Price In Tickets";
            this.priceInTicketsDataGridViewTextBoxColumn.Name = "priceInTicketsDataGridViewTextBoxColumn";
            // 
            // autoUpdateMarkupDataGridViewCheckBoxColumn
            // 
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.DataPropertyName = "AutoUpdateMarkup";
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.FalseValue = "false";
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.HeaderText = "AutoUpdatePIT?";
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.Name = "autoUpdateMarkupDataGridViewCheckBoxColumn";
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.ReadOnly = true;
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // CostVariancePercentage
            // 
            this.CostVariancePercentage.DataPropertyName = "CostVariancePercentage";
            this.CostVariancePercentage.HeaderText = "Cost Variance %";
            this.CostVariancePercentage.Name = "CostVariancePercentage";
            // 
            // defaultLocationIdDataGridViewTextBoxColumn
            // 
            this.defaultLocationIdDataGridViewTextBoxColumn.DataPropertyName = "DefaultLocationId";
            this.defaultLocationIdDataGridViewTextBoxColumn.HeaderText = "Inbound Location";
            this.defaultLocationIdDataGridViewTextBoxColumn.Name = "defaultLocationIdDataGridViewTextBoxColumn";
            this.defaultLocationIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.defaultLocationIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // outboundLocationIdDataGridViewTextBoxColumn
            // 
            this.outboundLocationIdDataGridViewTextBoxColumn.DataPropertyName = "OutboundLocationId";
            this.outboundLocationIdDataGridViewTextBoxColumn.HeaderText = "Outbound Location";
            this.outboundLocationIdDataGridViewTextBoxColumn.Name = "outboundLocationIdDataGridViewTextBoxColumn";
            this.outboundLocationIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.outboundLocationIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // defaultVendorIdDataGridViewTextBoxColumn
            // 
            this.defaultVendorIdDataGridViewTextBoxColumn.DataPropertyName = "DefaultVendorId";
            this.defaultVendorIdDataGridViewTextBoxColumn.HeaderText = "Default Vendor";
            this.defaultVendorIdDataGridViewTextBoxColumn.Name = "defaultVendorIdDataGridViewTextBoxColumn";
            this.defaultVendorIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.defaultVendorIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // reorderPointDataGridViewTextBoxColumn
            // 
            this.reorderPointDataGridViewTextBoxColumn.DataPropertyName = "ReorderPoint";
            this.reorderPointDataGridViewTextBoxColumn.HeaderText = "Reorder Point";
            this.reorderPointDataGridViewTextBoxColumn.Name = "reorderPointDataGridViewTextBoxColumn";
            // 
            // UpperLimitCost
            // 
            this.UpperLimitCost.DataPropertyName = "UpperLimitCost";
            this.UpperLimitCost.HeaderText = "Upper Limit Cost";
            this.UpperLimitCost.Name = "UpperLimitCost";
            // 
            // SegmentCategoryId
            // 
            this.SegmentCategoryId.DataPropertyName = "SegmentCategoryId";
            this.SegmentCategoryId.HeaderText = "SKU Segment";
            this.SegmentCategoryId.Name = "SegmentCategoryId";
            this.SegmentCategoryId.Text = ".....";
            // 
            // CustomDataSetId
            // 
            this.CustomDataSetId.HeaderText = "Custom";
            this.CustomDataSetId.Name = "CustomDataSetId";
            this.CustomDataSetId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CustomDataSetId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CustomDataSetId.Text = "...";
            // 
            // reorderQuantityDataGridViewTextBoxColumn
            // 
            this.reorderQuantityDataGridViewTextBoxColumn.DataPropertyName = "ReorderQuantity";
            this.reorderQuantityDataGridViewTextBoxColumn.HeaderText = "Reorder Quantity";
            this.reorderQuantityDataGridViewTextBoxColumn.Name = "reorderQuantityDataGridViewTextBoxColumn";
            // 
            // innerPackQtyDataGridViewTextBoxColumn
            // 
            this.innerPackQtyDataGridViewTextBoxColumn.DataPropertyName = "InnerPackQty";
            this.innerPackQtyDataGridViewTextBoxColumn.HeaderText = "Inner Cost Unit";
            this.innerPackQtyDataGridViewTextBoxColumn.Name = "innerPackQtyDataGridViewTextBoxColumn";
            // 
            // salePriceDataGridViewTextBoxColumn
            // 
            this.salePriceDataGridViewTextBoxColumn.DataPropertyName = "SalePrice";
            this.salePriceDataGridViewTextBoxColumn.HeaderText = "Sale Price";
            this.salePriceDataGridViewTextBoxColumn.Name = "salePriceDataGridViewTextBoxColumn";
            // 
            // costDataGridViewTextBoxColumn
            // 
            this.costDataGridViewTextBoxColumn.DataPropertyName = "Cost";
            this.costDataGridViewTextBoxColumn.HeaderText = "Cost";
            this.costDataGridViewTextBoxColumn.Name = "costDataGridViewTextBoxColumn";
            // 
            // lastPurchasePriceDataGridViewTextBoxColumn
            // 
            this.lastPurchasePriceDataGridViewTextBoxColumn.DataPropertyName = "LastPurchasePrice";
            this.lastPurchasePriceDataGridViewTextBoxColumn.HeaderText = "Last Purchase Price";
            this.lastPurchasePriceDataGridViewTextBoxColumn.Name = "lastPurchasePriceDataGridViewTextBoxColumn";
            this.lastPurchasePriceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // itemMarkupPercentDataGridViewTextBoxColumn
            // 
            this.itemMarkupPercentDataGridViewTextBoxColumn.DataPropertyName = "ItemMarkupPercent";
            this.itemMarkupPercentDataGridViewTextBoxColumn.HeaderText = "Item Markup Percent";
            this.itemMarkupPercentDataGridViewTextBoxColumn.Name = "itemMarkupPercentDataGridViewTextBoxColumn";
            // 
            // isRedeemableDataGridViewTextBoxColumn
            // 
            this.isRedeemableDataGridViewTextBoxColumn.DataPropertyName = "IsRedeemable";
            this.isRedeemableDataGridViewTextBoxColumn.FalseValue = "N";
            this.isRedeemableDataGridViewTextBoxColumn.HeaderText = "Redeemable?";
            this.isRedeemableDataGridViewTextBoxColumn.Name = "isRedeemableDataGridViewTextBoxColumn";
            this.isRedeemableDataGridViewTextBoxColumn.ReadOnly = true;
            this.isRedeemableDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isRedeemableDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isRedeemableDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // isSellableDataGridViewTextBoxColumn
            // 
            this.isSellableDataGridViewTextBoxColumn.DataPropertyName = "IsSellable";
            this.isSellableDataGridViewTextBoxColumn.FalseValue = "N";
            this.isSellableDataGridViewTextBoxColumn.HeaderText = "Sellable?";
            this.isSellableDataGridViewTextBoxColumn.Name = "isSellableDataGridViewTextBoxColumn";
            this.isSellableDataGridViewTextBoxColumn.ReadOnly = true;
            this.isSellableDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isSellableDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isSellableDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // isPurchaseableDataGridViewTextBoxColumn
            // 
            this.isPurchaseableDataGridViewTextBoxColumn.DataPropertyName = "IsPurchaseable";
            this.isPurchaseableDataGridViewTextBoxColumn.FalseValue = "N";
            this.isPurchaseableDataGridViewTextBoxColumn.HeaderText = "Inv. Item?";
            this.isPurchaseableDataGridViewTextBoxColumn.Name = "isPurchaseableDataGridViewTextBoxColumn";
            this.isPurchaseableDataGridViewTextBoxColumn.ReadOnly = true;
            this.isPurchaseableDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isPurchaseableDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isPurchaseableDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // isActiveDataGridViewTextBoxColumn
            // 
            this.isActiveDataGridViewTextBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn.FalseValue = "false";
            this.isActiveDataGridViewTextBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewTextBoxColumn.Name = "isActiveDataGridViewTextBoxColumn";
            this.isActiveDataGridViewTextBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isActiveDataGridViewTextBoxColumn.TrueValue = "true";
            // 
            // cbxIncludeInPlan
            // 
            this.cbxIncludeInPlan.DataPropertyName = "IncludeInPlan";
            this.cbxIncludeInPlan.HeaderText = "IncludeInPlan";
            this.cbxIncludeInPlan.Name = "cbxIncludeInPlan";
            // 
            // taxIdDataGridViewTextBoxColumn
            // 
            this.taxIdDataGridViewTextBoxColumn.DataPropertyName = "PurchaseTaxId";
            this.taxIdDataGridViewTextBoxColumn.HeaderText = "Tax";
            this.taxIdDataGridViewTextBoxColumn.Name = "taxIdDataGridViewTextBoxColumn";
            this.taxIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.taxIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // taxInclusiveCostDataGridViewTextBoxColumn
            // 
            this.taxInclusiveCostDataGridViewTextBoxColumn.DataPropertyName = "TaxInclusiveCost";
            this.taxInclusiveCostDataGridViewTextBoxColumn.FalseValue = "N";
            this.taxInclusiveCostDataGridViewTextBoxColumn.HeaderText = "Tax Incl. Cost";
            this.taxInclusiveCostDataGridViewTextBoxColumn.Name = "taxInclusiveCostDataGridViewTextBoxColumn";
            this.taxInclusiveCostDataGridViewTextBoxColumn.ReadOnly = true;
            this.taxInclusiveCostDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.taxInclusiveCostDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.taxInclusiveCostDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // pictureDataGridViewImageColumn
            // 
            this.pictureDataGridViewImageColumn.HeaderText = "Picture";
            this.pictureDataGridViewImageColumn.Name = "pictureDataGridViewImageColumn";
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            // 
            // upperLimitCostDataGridViewTextBoxColumn
            // 
            this.upperLimitCostDataGridViewTextBoxColumn.DataPropertyName = "UpperLimitCost";
            this.upperLimitCostDataGridViewTextBoxColumn.HeaderText = "Upper Limit Cost";
            this.upperLimitCostDataGridViewTextBoxColumn.Name = "upperLimitCostDataGridViewTextBoxColumn";
            this.upperLimitCostDataGridViewTextBoxColumn.Visible = false;
            // 
            // masterPackQtyDataGridViewTextBoxColumn
            // 
            this.masterPackQtyDataGridViewTextBoxColumn.DataPropertyName = "MasterPackQty";
            this.masterPackQtyDataGridViewTextBoxColumn.HeaderText = "Master Pack Qty";
            this.masterPackQtyDataGridViewTextBoxColumn.Name = "masterPackQtyDataGridViewTextBoxColumn";
            this.masterPackQtyDataGridViewTextBoxColumn.Visible = false;
            // 
            // imageFileNameDataGridViewTextBoxColumn
            // 
            this.imageFileNameDataGridViewTextBoxColumn.DataPropertyName = "ImageFileName";
            this.imageFileNameDataGridViewTextBoxColumn.HeaderText = "Image File Name";
            this.imageFileNameDataGridViewTextBoxColumn.Name = "imageFileNameDataGridViewTextBoxColumn";
            this.imageFileNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // lowerLimitCostDataGridViewTextBoxColumn
            // 
            this.lowerLimitCostDataGridViewTextBoxColumn.DataPropertyName = "LowerLimitCost";
            this.lowerLimitCostDataGridViewTextBoxColumn.HeaderText = "Lower Limit Cost";
            this.lowerLimitCostDataGridViewTextBoxColumn.Name = "lowerLimitCostDataGridViewTextBoxColumn";
            this.lowerLimitCostDataGridViewTextBoxColumn.Visible = false;
            // 
            // costVariancePercentageDataGridViewTextBoxColumn
            // 
            this.costVariancePercentageDataGridViewTextBoxColumn.DataPropertyName = "CostVariancePercentage";
            this.costVariancePercentageDataGridViewTextBoxColumn.HeaderText = "Cost Variance Percentage";
            this.costVariancePercentageDataGridViewTextBoxColumn.Name = "costVariancePercentageDataGridViewTextBoxColumn";
            this.costVariancePercentageDataGridViewTextBoxColumn.Visible = false;
            // 
            // YieldPercentage
            // 
            this.YieldPercentage.DataPropertyName = "YieldPercentage";
            this.YieldPercentage.HeaderText = "YieldPercentage";
            this.YieldPercentage.Name = "YieldPercentage";
            // 
            // turnInPriceInTicketsDataGridViewTextBoxColumn
            // 
            this.turnInPriceInTicketsDataGridViewTextBoxColumn.DataPropertyName = "TurnInPriceInTickets";
            this.turnInPriceInTicketsDataGridViewTextBoxColumn.HeaderText = "TurnIn PriceIn Tickets";
            this.turnInPriceInTicketsDataGridViewTextBoxColumn.Name = "turnInPriceInTicketsDataGridViewTextBoxColumn";
            this.turnInPriceInTicketsDataGridViewTextBoxColumn.Visible = false;
            // 
            // lotControlledDataGridViewCheckBoxColumn
            // 
            this.lotControlledDataGridViewCheckBoxColumn.DataPropertyName = "LotControlled";
            this.lotControlledDataGridViewCheckBoxColumn.FalseValue = "false";
            this.lotControlledDataGridViewCheckBoxColumn.HeaderText = "Lot Controlled";
            this.lotControlledDataGridViewCheckBoxColumn.Name = "lotControlledDataGridViewCheckBoxColumn";
            this.lotControlledDataGridViewCheckBoxColumn.ReadOnly = true;
            this.lotControlledDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // marketListItemDataGridViewCheckBoxColumn
            // 
            this.marketListItemDataGridViewCheckBoxColumn.DataPropertyName = "MarketListItem";
            this.marketListItemDataGridViewCheckBoxColumn.FalseValue = "false";
            this.marketListItemDataGridViewCheckBoxColumn.HeaderText = "Market List Item";
            this.marketListItemDataGridViewCheckBoxColumn.Name = "marketListItemDataGridViewCheckBoxColumn";
            this.marketListItemDataGridViewCheckBoxColumn.ReadOnly = true;
            this.marketListItemDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // expiryTypeDataGridViewTextBoxColumn
            // 
            this.expiryTypeDataGridViewTextBoxColumn.DataPropertyName = "ExpiryType";
            this.expiryTypeDataGridViewTextBoxColumn.HeaderText = "Expiry Type";
            this.expiryTypeDataGridViewTextBoxColumn.Name = "expiryTypeDataGridViewTextBoxColumn";
            this.expiryTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.expiryTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // issuingApproachDataGridViewTextBoxColumn
            // 
            this.issuingApproachDataGridViewTextBoxColumn.DataPropertyName = "IssuingApproach";
            this.issuingApproachDataGridViewTextBoxColumn.HeaderText = "Issuing Approach";
            this.issuingApproachDataGridViewTextBoxColumn.Items.AddRange(new object[] {
            "FIFO",
            "FEFO",
            "None"});
            this.issuingApproachDataGridViewTextBoxColumn.Name = "issuingApproachDataGridViewTextBoxColumn";
            this.issuingApproachDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.issuingApproachDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // uOMValueDataGridViewTextBoxColumn
            // 
            this.uOMValueDataGridViewTextBoxColumn.DataPropertyName = "UOMValue";
            this.uOMValueDataGridViewTextBoxColumn.HeaderText = "Unit of Measure";
            this.uOMValueDataGridViewTextBoxColumn.Name = "uOMValueDataGridViewTextBoxColumn";
            this.uOMValueDataGridViewTextBoxColumn.Visible = false;
            // 
            // expiryDaysDataGridViewTextBoxColumn
            // 
            this.expiryDaysDataGridViewTextBoxColumn.DataPropertyName = "ExpiryDays";
            this.expiryDaysDataGridViewTextBoxColumn.HeaderText = "Expiry Days";
            this.expiryDaysDataGridViewTextBoxColumn.Name = "expiryDaysDataGridViewTextBoxColumn";
            this.expiryDaysDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.expiryDaysDataGridViewTextBoxColumn.Visible = false;
            // 
            // customDataSetIdDataGridViewTextBoxColumn
            // 
            this.customDataSetIdDataGridViewTextBoxColumn.DataPropertyName = "CustomDataSetId";
            this.customDataSetIdDataGridViewTextBoxColumn.HeaderText = "CustomDataSetId";
            this.customDataSetIdDataGridViewTextBoxColumn.Name = "customDataSetIdDataGridViewTextBoxColumn";
            this.customDataSetIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // segmentCategoryIdDataGridViewTextBoxColumn
            // 
            this.segmentCategoryIdDataGridViewTextBoxColumn.DataPropertyName = "SegmentCategoryId";
            this.segmentCategoryIdDataGridViewTextBoxColumn.HeaderText = "Segment Category Id";
            this.segmentCategoryIdDataGridViewTextBoxColumn.Name = "segmentCategoryIdDataGridViewTextBoxColumn";
            this.segmentCategoryIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // CostIncludeTaxDataGridViewCheckBoxColumn
            // 
            this.CostIncludeTaxDataGridViewCheckBoxColumn.DataPropertyName = "CostIncludesTax";
            this.CostIncludeTaxDataGridViewCheckBoxColumn.FalseValue = "false";
            this.CostIncludeTaxDataGridViewCheckBoxColumn.HeaderText = "CostIncludesTax";
            this.CostIncludeTaxDataGridViewCheckBoxColumn.Name = "CostIncludeTaxDataGridViewCheckBoxColumn";
            this.CostIncludeTaxDataGridViewCheckBoxColumn.ReadOnly = true;
            this.CostIncludeTaxDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // cmbUOM
            // 
            this.cmbUOM.DataPropertyName = "InventoryUOMId";
            this.cmbUOM.HeaderText = "Inventory UOM";
            this.cmbUOM.Name = "cmbUOM";
            // 
            // PreparationTime
            // 
            this.PreparationTime.DataPropertyName = "PreparationTime";
            this.PreparationTime.HeaderText = "Preparation Time";
            this.PreparationTime.Name = "PreparationTime";
            // 
            // productDTOBindingSource
            // 
            this.productDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.ProductDTO);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Image = global::Semnox.Parafait.Inventory.Properties.Resources.Refresh;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(219, 473);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(97, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Image = global::Semnox.Parafait.Inventory.Properties.Resources.cancel;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(328, 473);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(81, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Image = global::Semnox.Parafait.Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(15, 473);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Image = global::Semnox.Parafait.Inventory.Properties.Resources.removegift;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(117, 473);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(91, 23);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // cmbProductType
            // 
            this.cmbProductType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbProductType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cmbProductType.FormattingEnabled = true;
            this.cmbProductType.Items.AddRange(new object[] {
            " - All -",
            "Redeemable",
            "Sellable"});
            this.cmbProductType.Location = new System.Drawing.Point(698, 31);
            this.cmbProductType.Name = "cmbProductType";
            this.cmbProductType.Size = new System.Drawing.Size(106, 21);
            this.cmbProductType.TabIndex = 62;
            // 
            // cmbDisplayGroup
            // 
            this.cmbDisplayGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbDisplayGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDisplayGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDisplayGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cmbDisplayGroup.FormattingEnabled = true;
            this.cmbDisplayGroup.Location = new System.Drawing.Point(279, 66);
            this.cmbDisplayGroup.Name = "cmbDisplayGroup";
            this.cmbDisplayGroup.Size = new System.Drawing.Size(106, 21);
            this.cmbDisplayGroup.TabIndex = 63;
            // 
            // cmbProductCategory
            // 
            this.cmbProductCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbProductCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbProductCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cmbProductCategory.FormattingEnabled = true;
            this.cmbProductCategory.Location = new System.Drawing.Point(496, 31);
            this.cmbProductCategory.Name = "cmbProductCategory";
            this.cmbProductCategory.Size = new System.Drawing.Size(106, 21);
            this.cmbProductCategory.TabIndex = 64;
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtDescription.Location = new System.Drawing.Point(279, 31);
            this.txtDescription.MaxLength = 200;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(106, 20);
            this.txtDescription.TabIndex = 69;
            // 
            // txtProductName
            // 
            this.txtProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtProductName.Location = new System.Drawing.Point(91, 31);
            this.txtProductName.MaxLength = 100;
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(106, 20);
            this.txtProductName.TabIndex = 70;
            // 
            // lblProductName
            // 
            this.lblProductName.Location = new System.Drawing.Point(6, 26);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(85, 30);
            this.lblProductName.TabIndex = 71;
            this.lblProductName.Text = "Product Name:";
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(194, 26);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(85, 30);
            this.lblDescription.TabIndex = 73;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProductCode
            // 
            this.lblProductCode.Location = new System.Drawing.Point(6, 61);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(85, 30);
            this.lblProductCode.TabIndex = 74;
            this.lblProductCode.Text = "Product Code:";
            this.lblProductCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVendor
            // 
            this.lblVendor.Location = new System.Drawing.Point(401, 61);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(93, 30);
            this.lblVendor.TabIndex = 75;
            this.lblVendor.Text = "Vendor:";
            this.lblVendor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtCode.Location = new System.Drawing.Point(91, 66);
            this.txtCode.MaxLength = 50;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(106, 20);
            this.txtCode.TabIndex = 76;
            // 
            // lblCategory
            // 
            this.lblCategory.Location = new System.Drawing.Point(393, 26);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(101, 30);
            this.lblCategory.TabIndex = 78;
            this.lblCategory.Text = " Product Category:";
            this.lblCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDisplayGroup
            // 
            this.lblDisplayGroup.Location = new System.Drawing.Point(197, 61);
            this.lblDisplayGroup.Name = "lblDisplayGroup";
            this.lblDisplayGroup.Size = new System.Drawing.Size(82, 30);
            this.lblDisplayGroup.TabIndex = 79;
            this.lblDisplayGroup.Text = "Display group:";
            this.lblDisplayGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Image = global::Semnox.Parafait.Inventory.Properties.Resources.search;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(1024, 10);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(107, 29);
            this.btnSearch.TabIndex = 80;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.Location = new System.Drawing.Point(1024, 72);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(107, 29);
            this.btnClear.TabIndex = 81;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblProductType
            // 
            this.lblProductType.Location = new System.Drawing.Point(610, 26);
            this.lblProductType.Name = "lblProductType";
            this.lblProductType.Size = new System.Drawing.Size(88, 30);
            this.lblProductType.TabIndex = 82;
            this.lblProductType.Text = "Product Type:";
            this.lblProductType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lnkPublishToSite
            // 
            this.lnkPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublishToSite.AutoSize = true;
            this.lnkPublishToSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublishToSite.Location = new System.Drawing.Point(890, 478);
            this.lnkPublishToSite.Name = "lnkPublishToSite";
            this.lnkPublishToSite.Size = new System.Drawing.Size(99, 13);
            this.lnkPublishToSite.TabIndex = 131;
            this.lnkPublishToSite.TabStop = true;
            this.lnkPublishToSite.Text = "Publish To Sites";
            this.lnkPublishToSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublishToSite_LinkClicked);
            // 
            // cmbVendor
            // 
            this.cmbVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVendor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cmbVendor.FormattingEnabled = true;
            this.cmbVendor.Location = new System.Drawing.Point(496, 66);
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.Size = new System.Drawing.Size(106, 21);
            this.cmbVendor.TabIndex = 132;
            // 
            // btnAdvancedSearch
            // 
            this.btnAdvancedSearch.Image = global::Semnox.Parafait.Inventory.Properties.Resources.search;
            this.btnAdvancedSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdvancedSearch.Location = new System.Drawing.Point(1024, 41);
            this.btnAdvancedSearch.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.btnAdvancedSearch.Name = "btnAdvancedSearch";
            this.btnAdvancedSearch.Size = new System.Drawing.Size(107, 29);
            this.btnAdvancedSearch.TabIndex = 133;
            this.btnAdvancedSearch.Text = "Adv Search";
            this.btnAdvancedSearch.UseVisualStyleBackColor = true;
            this.btnAdvancedSearch.Click += new System.EventHandler(this.btnAdvancedSearch_Click);
            // 
            // lblActive
            // 
            this.lblActive.Location = new System.Drawing.Point(617, 61);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(81, 30);
            this.lblActive.TabIndex = 135;
            this.lblActive.Text = "Active:";
            this.lblActive.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbActive
            // 
            this.cmbActive.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbActive.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cmbActive.FormattingEnabled = true;
            this.cmbActive.Items.AddRange(new object[] {
            " - All -",
            "Active",
            "InActive"});
            this.cmbActive.Location = new System.Drawing.Point(698, 66);
            this.cmbActive.Name = "cmbActive";
            this.cmbActive.Size = new System.Drawing.Size(106, 21);
            this.cmbActive.TabIndex = 134;
            // 
            // cbxLot
            // 
            this.cbxLot.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxLot.Location = new System.Drawing.Point(823, 66);
            this.cbxLot.Name = "cbxLot";
            this.cbxLot.Size = new System.Drawing.Size(99, 21);
            this.cbxLot.TabIndex = 139;
            this.cbxLot.Text = "Lot Product:";
            this.cbxLot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxLot.UseVisualStyleBackColor = true;
            // 
            // lblBarcode
            // 
            this.lblBarcode.Location = new System.Drawing.Point(809, 26);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(99, 30);
            this.lblBarcode.TabIndex = 137;
            this.lblBarcode.Text = "Product Barcode:";
            this.lblBarcode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtBarcode.Location = new System.Drawing.Point(909, 31);
            this.txtBarcode.MaxLength = 50;
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(106, 20);
            this.txtBarcode.TabIndex = 138;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxLot);
            this.groupBox1.Controls.Add(this.lblProductName);
            this.groupBox1.Controls.Add(this.lblBarcode);
            this.groupBox1.Controls.Add(this.cmbProductCategory);
            this.groupBox1.Controls.Add(this.txtBarcode);
            this.groupBox1.Controls.Add(this.lblDisplayGroup);
            this.groupBox1.Controls.Add(this.txtProductName);
            this.groupBox1.Controls.Add(this.cmbDisplayGroup);
            this.groupBox1.Controls.Add(this.btnAdvancedSearch);
            this.groupBox1.Controls.Add(this.lblCategory);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.lblVendor);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.cmbProductType);
            this.groupBox1.Controls.Add(this.lblDescription);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.lblProductType);
            this.groupBox1.Controls.Add(this.cmbActive);
            this.groupBox1.Controls.Add(this.cmbVendor);
            this.groupBox1.Controls.Add(this.lblActive);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.lblProductCode);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1146, 107);
            this.groupBox1.TabIndex = 138;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Product Search";
            // 
            // frmProductTabular
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1044, 534);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lnkPublishToSite);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvProducts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmProductTabular";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Products";
            this.Load += new System.EventHandler(this.frmProductTabular_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productDTOBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.BindingSource productDTOBindingSource;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DisplayInPOS;
        private System.Windows.Forms.ComboBox cmbProductType;
        private System.Windows.Forms.ComboBox cmbDisplayGroup;
        private System.Windows.Forms.ComboBox cmbProductCategory;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.Label lblVendor;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblDisplayGroup;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblProductType;
        private System.Windows.Forms.LinkLabel lnkPublishToSite;
        private System.Windows.Forms.ComboBox cmbVendor;
        private System.Windows.Forms.Button btnAdvancedSearch;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.ComboBox cmbActive;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.CheckBox cbxLot;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SetProducts;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BarCode;
        private System.Windows.Forms.DataGridViewButtonColumn ProductBarCode;
        private System.Windows.Forms.DataGridViewComboBoxColumn categoryIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TurnInPriceInTickets;
        private System.Windows.Forms.DataGridViewTextBoxColumn LowerLimitCost;
        private System.Windows.Forms.DataGridViewComboBoxColumn uomIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbItemType;
        private System.Windows.Forms.DataGridViewButtonColumn btnRecipeDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceInTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn autoUpdateMarkupDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostVariancePercentage;
        private System.Windows.Forms.DataGridViewComboBoxColumn defaultLocationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn outboundLocationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn defaultVendorIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reorderPointDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpperLimitCost;
        private System.Windows.Forms.DataGridViewButtonColumn SegmentCategoryId;
        private System.Windows.Forms.DataGridViewButtonColumn CustomDataSetId;
        private System.Windows.Forms.DataGridViewTextBoxColumn reorderQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn innerPackQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn salePriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastPurchasePriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemMarkupPercentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isRedeemableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isSellableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isPurchaseableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cbxIncludeInPlan;
        private System.Windows.Forms.DataGridViewComboBoxColumn taxIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn taxInclusiveCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn pictureDataGridViewImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn upperLimitCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterPackQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lowerLimitCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costVariancePercentageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn YieldPercentage;
        private System.Windows.Forms.DataGridViewTextBoxColumn turnInPriceInTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn lotControlledDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn marketListItemDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn expiryTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn issuingApproachDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uOMValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expiryDaysDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customDataSetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn segmentCategoryIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CostIncludeTaxDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn PreparationTime;
    }
}