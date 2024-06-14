using Semnox.Parafait.Category;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Product;
using Semnox.Parafait.Vendor;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Inventory
{
    partial class frmProduct
    {

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
            this.view_dgv = new System.Windows.Forms.DataGridView();
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
            this.isRedeemableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outboundLocationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.salePriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taxInclusiveCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lowerLimitCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.upperLimitCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costVariancePercentageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.turnInPriceInTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.segmentCategoryIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uOMValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.autoUpdateMarkupDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.itemMarkupPercentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.fillByToolStrip = new System.Windows.Forms.ToolStrip();
            this.productCodeToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.productCodeToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.DescriptiontoolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.cmbCategory = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmbProductType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.BarcodetoolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.cmbActive = new System.Windows.Forms.ToolStripComboBox();
            this.fillByToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbsAdvancedSearched = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.gb_view = new System.Windows.Forms.GroupBox();
            this.taxBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnTabularView = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnDuplicate = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.tvBOM = new System.Windows.Forms.TreeView();
            this.EditProduct = new System.Windows.Forms.ToolStripMenuItem();
            this.Add = new System.Windows.Forms.ToolStripMenuItem();
            this.Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.EditBOM = new System.Windows.Forms.ToolStripMenuItem();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnPrintBOM = new System.Windows.Forms.Button();
            this.productBarcodebindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.RichTextBox();
            this.lblProductid = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txtYieldPer = new System.Windows.Forms.TextBox();
            this.cbxCostHasTax = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_error_sale_price = new System.Windows.Forms.Label();
            this.txtSalePrice = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.txtCostVariancePer = new System.Windows.Forms.TextBox();
            this.txtUpperCostLimit = new System.Windows.Forms.TextBox();
            this.lnkBuildCostfromBOM = new System.Windows.Forms.LinkLabel();
            this.txtLowerCostLimit = new System.Windows.Forms.TextBox();
            this.txtInnerPackQty = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtCost = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLastpurchaseprice = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtItemMarkupPercent = new System.Windows.Forms.TextBox();
            this.lblItemMarkupPercent = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPreparationTime = new System.Windows.Forms.TextBox();
            this.lblPreparationTime = new System.Windows.Forms.Label();
            this.cbxIncludeInPlan = new System.Windows.Forms.CheckBox();
            this.label26 = new System.Windows.Forms.Label();
            this.txtIndays = new System.Windows.Forms.TextBox();
            this.cbxMarketListItem = new System.Windows.Forms.CheckBox();
            this.cbxLotControlled = new System.Windows.Forms.CheckBox();
            this.ddlIssueApproach = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.ddlExpiryType = new System.Windows.Forms.ComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.btnSKUSegments = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.cbxIsRedeemable = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cbxIsSellable = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cbxPurchaseable = new System.Windows.Forms.CheckBox();
            this.gb_add = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lnkrecipeDescription = new System.Windows.Forms.LinkLabel();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbInventoryUOM = new System.Windows.Forms.ComboBox();
            this.uOMDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnShowDisplayGroups = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbItemType = new System.Windows.Forms.ComboBox();
            this.txtDisplayGroup = new System.Windows.Forms.TextBox();
            this.lblDisplayGrp = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.cbxAutoUpdateMarkup = new System.Windows.Forms.CheckBox();
            this.lnkStock = new System.Windows.Forms.LinkLabel();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.btnUPCBarcode = new System.Windows.Forms.Button();
            this.btnCustom = new System.Windows.Forms.Button();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbxTaxInclusiveCost = new System.Windows.Forms.CheckBox();
            this.txtTurnInPIT = new System.Windows.Forms.TextBox();
            this.lblImageFileName = new System.Windows.Forms.Label();
            this.lnkClearImage = new System.Windows.Forms.LinkLabel();
            this.lnkProductImage = new System.Windows.Forms.LinkLabel();
            this.pbProductImage = new System.Windows.Forms.PictureBox();
            this.lblActive = new System.Windows.Forms.Label();
            this.cbxActive = new System.Windows.Forms.CheckBox();
            this.btnAddUOM = new System.Windows.Forms.Button();
            this.ddlUOM = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.btnAddTax = new System.Windows.Forms.Button();
            this.ddlTax = new System.Windows.Forms.ComboBox();
            this.purchaseTaxDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ddlOutboundLocation = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.outBoundLocationDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblOutboundLocation = new System.Windows.Forms.Label();
            this.btnGenerateBarCode = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnAddvendor = new System.Windows.Forms.Button();
            this.btnAddlocation = new System.Windows.Forms.Button();
            this.btnAddcategory = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPit = new System.Windows.Forms.TextBox();
            this.ddlPreferredvendor = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.vendorDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.ddlDefaultlocation = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.inBoundLocationDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblInboundLocation = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtReorderquantity = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtReorderpoint = new System.Windows.Forms.TextBox();
            this.ddlCategory = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.categoryDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblCategory = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkPublishToSite = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.view_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productDTOBindingSource)).BeginInit();
            this.fillByToolStrip.SuspendLayout();
            this.gb_view.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taxBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productBarcodebindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gb_add.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uOMDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProductImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseTaxDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outBoundLocationDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inBoundLocationDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.categoryDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // view_dgv
            // 
            this.view_dgv.AllowUserToAddRows = false;
            this.view_dgv.AllowUserToDeleteRows = false;
            this.view_dgv.AutoGenerateColumns = false;
            this.view_dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.view_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.view_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14,
            this.isRedeemableDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn15,
            this.dataGridViewTextBoxColumn16,
            this.dataGridViewTextBoxColumn17,
            this.dataGridViewTextBoxColumn18,
            this.outboundLocationIdDataGridViewTextBoxColumn,
            this.salePriceDataGridViewTextBoxColumn,
            this.taxInclusiveCostDataGridViewTextBoxColumn,
            this.imageFileNameDataGridViewTextBoxColumn,
            this.lowerLimitCostDataGridViewTextBoxColumn,
            this.upperLimitCostDataGridViewTextBoxColumn,
            this.costVariancePercentageDataGridViewTextBoxColumn,
            this.turnInPriceInTicketsDataGridViewTextBoxColumn,
            this.segmentCategoryIdDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn19,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewCheckBoxColumn2,
            this.dataGridViewTextBoxColumn20,
            this.dataGridViewTextBoxColumn21,
            this.uOMValueDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn22,
            this.dataGridViewTextBoxColumn23,
            this.productNameDataGridViewTextBoxColumn,
            this.autoUpdateMarkupDataGridViewCheckBoxColumn,
            this.itemMarkupPercentDataGridViewTextBoxColumn});
            this.view_dgv.DataSource = this.productDTOBindingSource;
            this.view_dgv.GridColor = System.Drawing.Color.DarkSeaGreen;
            this.view_dgv.Location = new System.Drawing.Point(6, 19);
            this.view_dgv.MultiSelect = false;
            this.view_dgv.Name = "view_dgv";
            this.view_dgv.ReadOnly = true;
            this.view_dgv.RowHeadersVisible = false;
            this.view_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.view_dgv.Size = new System.Drawing.Size(216, 467);
            this.view_dgv.TabIndex = 3;
            this.view_dgv.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.view_dgv_RowStateChanged);
            this.view_dgv.SelectionChanged += new System.EventHandler(this.view_dgv_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn1.HeaderText = "Product Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Code";
            this.dataGridViewTextBoxColumn2.HeaderText = "Code";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn3.HeaderText = "Description";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Remarks";
            this.dataGridViewTextBoxColumn4.HeaderText = "Remarks";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "CategoryId";
            this.dataGridViewTextBoxColumn5.HeaderText = "Category Id";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "DefaultLocationId";
            this.dataGridViewTextBoxColumn6.HeaderText = "Default Location Id";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "ReorderPoint";
            this.dataGridViewTextBoxColumn7.HeaderText = "Reorder Point";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "ReorderQuantity";
            this.dataGridViewTextBoxColumn8.HeaderText = "Reorder Quantity";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "UomId";
            this.dataGridViewTextBoxColumn9.HeaderText = "UomId";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "MasterPackQty";
            this.dataGridViewTextBoxColumn10.HeaderText = "Master Pack Qty";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Visible = false;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "InnerPackQty";
            this.dataGridViewTextBoxColumn11.HeaderText = "Inner Pack Qty";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Visible = false;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "DefaultVendorId";
            this.dataGridViewTextBoxColumn12.HeaderText = "Default Vendor Id";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Visible = false;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "Cost";
            this.dataGridViewTextBoxColumn13.HeaderText = "Cost";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Visible = false;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "LastPurchasePrice";
            this.dataGridViewTextBoxColumn14.HeaderText = "Last Purchase Price";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Visible = false;
            // 
            // isRedeemableDataGridViewTextBoxColumn
            // 
            this.isRedeemableDataGridViewTextBoxColumn.DataPropertyName = "IsRedeemable";
            this.isRedeemableDataGridViewTextBoxColumn.HeaderText = "Redeemable?";
            this.isRedeemableDataGridViewTextBoxColumn.Name = "isRedeemableDataGridViewTextBoxColumn";
            this.isRedeemableDataGridViewTextBoxColumn.ReadOnly = true;
            this.isRedeemableDataGridViewTextBoxColumn.Visible = false;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "IsSellable";
            this.dataGridViewTextBoxColumn15.HeaderText = "Sellable?";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            this.dataGridViewTextBoxColumn15.Visible = false;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "IsPurchaseable";
            this.dataGridViewTextBoxColumn16.HeaderText = "Purchaseable?";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            this.dataGridViewTextBoxColumn16.Visible = false;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.DataPropertyName = "IsActive";
            this.dataGridViewTextBoxColumn17.HeaderText = "Active?";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.ReadOnly = true;
            this.dataGridViewTextBoxColumn17.Visible = false;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.DataPropertyName = "PriceInTickets";
            this.dataGridViewTextBoxColumn18.HeaderText = "Price In Tickets";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.ReadOnly = true;
            this.dataGridViewTextBoxColumn18.Visible = false;
            // 
            // outboundLocationIdDataGridViewTextBoxColumn
            // 
            this.outboundLocationIdDataGridViewTextBoxColumn.DataPropertyName = "OutboundLocationId";
            this.outboundLocationIdDataGridViewTextBoxColumn.HeaderText = "Outbound Location Id";
            this.outboundLocationIdDataGridViewTextBoxColumn.Name = "outboundLocationIdDataGridViewTextBoxColumn";
            this.outboundLocationIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.outboundLocationIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // salePriceDataGridViewTextBoxColumn
            // 
            this.salePriceDataGridViewTextBoxColumn.DataPropertyName = "SalePrice";
            this.salePriceDataGridViewTextBoxColumn.HeaderText = "Sale Price";
            this.salePriceDataGridViewTextBoxColumn.Name = "salePriceDataGridViewTextBoxColumn";
            this.salePriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.salePriceDataGridViewTextBoxColumn.Visible = false;
            // 
            // taxInclusiveCostDataGridViewTextBoxColumn
            // 
            this.taxInclusiveCostDataGridViewTextBoxColumn.DataPropertyName = "TaxInclusiveCost";
            this.taxInclusiveCostDataGridViewTextBoxColumn.HeaderText = "Tax Inclusive Cost";
            this.taxInclusiveCostDataGridViewTextBoxColumn.Name = "taxInclusiveCostDataGridViewTextBoxColumn";
            this.taxInclusiveCostDataGridViewTextBoxColumn.ReadOnly = true;
            this.taxInclusiveCostDataGridViewTextBoxColumn.Visible = false;
            // 
            // imageFileNameDataGridViewTextBoxColumn
            // 
            this.imageFileNameDataGridViewTextBoxColumn.DataPropertyName = "ImageFileName";
            this.imageFileNameDataGridViewTextBoxColumn.HeaderText = "Image File Name";
            this.imageFileNameDataGridViewTextBoxColumn.Name = "imageFileNameDataGridViewTextBoxColumn";
            this.imageFileNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.imageFileNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // lowerLimitCostDataGridViewTextBoxColumn
            // 
            this.lowerLimitCostDataGridViewTextBoxColumn.DataPropertyName = "LowerLimitCost";
            this.lowerLimitCostDataGridViewTextBoxColumn.HeaderText = "Lower Limit Cost";
            this.lowerLimitCostDataGridViewTextBoxColumn.Name = "lowerLimitCostDataGridViewTextBoxColumn";
            this.lowerLimitCostDataGridViewTextBoxColumn.ReadOnly = true;
            this.lowerLimitCostDataGridViewTextBoxColumn.Visible = false;
            // 
            // upperLimitCostDataGridViewTextBoxColumn
            // 
            this.upperLimitCostDataGridViewTextBoxColumn.DataPropertyName = "UpperLimitCost";
            this.upperLimitCostDataGridViewTextBoxColumn.HeaderText = "Upper Limit Cost";
            this.upperLimitCostDataGridViewTextBoxColumn.Name = "upperLimitCostDataGridViewTextBoxColumn";
            this.upperLimitCostDataGridViewTextBoxColumn.ReadOnly = true;
            this.upperLimitCostDataGridViewTextBoxColumn.Visible = false;
            // 
            // costVariancePercentageDataGridViewTextBoxColumn
            // 
            this.costVariancePercentageDataGridViewTextBoxColumn.DataPropertyName = "CostVariancePercentage";
            this.costVariancePercentageDataGridViewTextBoxColumn.HeaderText = "Cost Variance Percentage";
            this.costVariancePercentageDataGridViewTextBoxColumn.Name = "costVariancePercentageDataGridViewTextBoxColumn";
            this.costVariancePercentageDataGridViewTextBoxColumn.ReadOnly = true;
            this.costVariancePercentageDataGridViewTextBoxColumn.Visible = false;
            // 
            // turnInPriceInTicketsDataGridViewTextBoxColumn
            // 
            this.turnInPriceInTicketsDataGridViewTextBoxColumn.DataPropertyName = "TurnInPriceInTickets";
            this.turnInPriceInTicketsDataGridViewTextBoxColumn.HeaderText = "TurnInPriceInTickets";
            this.turnInPriceInTicketsDataGridViewTextBoxColumn.Name = "turnInPriceInTicketsDataGridViewTextBoxColumn";
            this.turnInPriceInTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            this.turnInPriceInTicketsDataGridViewTextBoxColumn.Visible = false;
            // 
            // segmentCategoryIdDataGridViewTextBoxColumn
            // 
            this.segmentCategoryIdDataGridViewTextBoxColumn.DataPropertyName = "SegmentCategoryId";
            this.segmentCategoryIdDataGridViewTextBoxColumn.HeaderText = "Segment Category Id";
            this.segmentCategoryIdDataGridViewTextBoxColumn.Name = "segmentCategoryIdDataGridViewTextBoxColumn";
            this.segmentCategoryIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.segmentCategoryIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.DataPropertyName = "CustomDataSetId";
            this.dataGridViewTextBoxColumn19.HeaderText = "CustomDataSetId";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.ReadOnly = true;
            this.dataGridViewTextBoxColumn19.Visible = false;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "LotControlled";
            this.dataGridViewCheckBoxColumn1.HeaderText = "LotControlled";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            this.dataGridViewCheckBoxColumn1.Visible = false;
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.DataPropertyName = "MarketListItem";
            this.dataGridViewCheckBoxColumn2.HeaderText = "MarketListItem";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.ReadOnly = true;
            this.dataGridViewCheckBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn20
            // 
            this.dataGridViewTextBoxColumn20.DataPropertyName = "ExpiryType";
            this.dataGridViewTextBoxColumn20.HeaderText = "ExpiryType";
            this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
            this.dataGridViewTextBoxColumn20.ReadOnly = true;
            this.dataGridViewTextBoxColumn20.Visible = false;
            // 
            // dataGridViewTextBoxColumn21
            // 
            this.dataGridViewTextBoxColumn21.DataPropertyName = "IssuingApproach";
            this.dataGridViewTextBoxColumn21.HeaderText = "IssuingApproach";
            this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
            this.dataGridViewTextBoxColumn21.ReadOnly = true;
            this.dataGridViewTextBoxColumn21.Visible = false;
            // 
            // uOMValueDataGridViewTextBoxColumn
            // 
            this.uOMValueDataGridViewTextBoxColumn.DataPropertyName = "UOMValue";
            this.uOMValueDataGridViewTextBoxColumn.HeaderText = "Unit of Measure";
            this.uOMValueDataGridViewTextBoxColumn.Name = "uOMValueDataGridViewTextBoxColumn";
            this.uOMValueDataGridViewTextBoxColumn.ReadOnly = true;
            this.uOMValueDataGridViewTextBoxColumn.Visible = false;
            // 
            // dataGridViewTextBoxColumn22
            // 
            this.dataGridViewTextBoxColumn22.DataPropertyName = "ExpiryDays";
            this.dataGridViewTextBoxColumn22.HeaderText = "Expiry Days";
            this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
            this.dataGridViewTextBoxColumn22.ReadOnly = true;
            this.dataGridViewTextBoxColumn22.Visible = false;
            // 
            // dataGridViewTextBoxColumn23
            // 
            this.dataGridViewTextBoxColumn23.DataPropertyName = "BarCode";
            this.dataGridViewTextBoxColumn23.HeaderText = "BarCode";
            this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
            this.dataGridViewTextBoxColumn23.ReadOnly = true;
            this.dataGridViewTextBoxColumn23.Visible = false;
            // 
            // productNameDataGridViewTextBoxColumn
            // 
            this.productNameDataGridViewTextBoxColumn.DataPropertyName = "ProductName";
            this.productNameDataGridViewTextBoxColumn.HeaderText = "ProductName";
            this.productNameDataGridViewTextBoxColumn.Name = "productNameDataGridViewTextBoxColumn";
            this.productNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.productNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // autoUpdateMarkupDataGridViewCheckBoxColumn
            // 
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.DataPropertyName = "AutoUpdateMarkup";
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.HeaderText = "AutoUpdateMarkup";
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.Name = "autoUpdateMarkupDataGridViewCheckBoxColumn";
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.ReadOnly = true;
            this.autoUpdateMarkupDataGridViewCheckBoxColumn.Visible = false;
            // 
            // itemMarkupPercentDataGridViewTextBoxColumn
            // 
            this.itemMarkupPercentDataGridViewTextBoxColumn.DataPropertyName = "ItemMarkupPercent";
            this.itemMarkupPercentDataGridViewTextBoxColumn.HeaderText = "ItemMarkupPercent";
            this.itemMarkupPercentDataGridViewTextBoxColumn.Name = "itemMarkupPercentDataGridViewTextBoxColumn";
            this.itemMarkupPercentDataGridViewTextBoxColumn.ReadOnly = true;
            this.itemMarkupPercentDataGridViewTextBoxColumn.Visible = false;
            // 
            // productDTOBindingSource
            // 
            this.productDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.ProductDTO);
            // 
            // fillByToolStrip
            // 
            this.fillByToolStrip.CanOverflow = false;
            this.fillByToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.productCodeToolStripLabel,
            this.productCodeToolStripTextBox,
            this.toolStripLabel3,
            this.DescriptiontoolStripTextBox,
            this.toolStripLabel2,
            this.cmbCategory,
            this.toolStripLabel1,
            this.cmbProductType,
            this.toolStripLabel4,
            this.BarcodetoolStripTextBox,
            this.toolStripLabel5,
            this.cmbActive,
            this.fillByToolStripButton,
            this.toolStripSeparator2,
            this.tbsAdvancedSearched,
            this.toolStripSeparator3,
            this.tsbClear});
            this.fillByToolStrip.Location = new System.Drawing.Point(0, 0);
            this.fillByToolStrip.Name = "fillByToolStrip";
            this.fillByToolStrip.Size = new System.Drawing.Size(1175, 25);
            this.fillByToolStrip.TabIndex = 1;
            this.fillByToolStrip.Text = "fillByToolStrip";
            // 
            // productCodeToolStripLabel
            // 
            this.productCodeToolStripLabel.Name = "productCodeToolStripLabel";
            this.productCodeToolStripLabel.Size = new System.Drawing.Size(80, 22);
            this.productCodeToolStripLabel.Text = "ProductCode:";
            // 
            // productCodeToolStripTextBox
            // 
            this.productCodeToolStripTextBox.Name = "productCodeToolStripTextBox";
            this.productCodeToolStripTextBox.Size = new System.Drawing.Size(100, 25);
            this.productCodeToolStripTextBox.Leave += new System.EventHandler(this.productCodeToolStrip_TextLeave);
            this.productCodeToolStripTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.productCodeToolStripTextBox_KeyPress);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(38, 22);
            this.toolStripLabel3.Text = "Desc.:";
            // 
            // DescriptiontoolStripTextBox
            // 
            this.DescriptiontoolStripTextBox.Name = "DescriptiontoolStripTextBox";
            this.DescriptiontoolStripTextBox.Size = new System.Drawing.Size(100, 25);
            this.DescriptiontoolStripTextBox.Leave += new System.EventHandler(this.productCodeToolStrip_TextLeave);
            this.DescriptiontoolStripTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DescriptiontoolStripTextBox_KeyPress);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(58, 22);
            this.toolStripLabel2.Text = "Category:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(95, 25);
            this.cmbCategory.Leave += new System.EventHandler(this.productCodeToolStrip_TextLeave);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(80, 22);
            this.toolStripLabel1.Text = "Product Type:";
            // 
            // cmbProductType
            // 
            this.cmbProductType.AutoSize = false;
            this.cmbProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductType.Items.AddRange(new object[] {
            " - All -",
            "Redeemable",
            "Sellable"});
            this.cmbProductType.Name = "cmbProductType";
            this.cmbProductType.Size = new System.Drawing.Size(85, 23);
            this.cmbProductType.Leave += new System.EventHandler(this.productCodeToolStrip_TextLeave);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(98, 22);
            this.toolStripLabel4.Text = "Product Barcode:";
            // 
            // BarcodetoolStripTextBox
            // 
            this.BarcodetoolStripTextBox.Name = "BarcodetoolStripTextBox";
            this.BarcodetoolStripTextBox.Size = new System.Drawing.Size(100, 25);
            this.BarcodetoolStripTextBox.Leave += new System.EventHandler(this.productCodeToolStrip_TextLeave);
            this.BarcodetoolStripTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BarcodetoolStripTextBox_KeyPress);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel5.Text = "Active:";
            // 
            // cmbActive
            // 
            this.cmbActive.AutoSize = false;
            this.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbActive.Items.AddRange(new object[] {
            " - All -",
            "Active",
            "InActive"});
            this.cmbActive.Name = "cmbActive";
            this.cmbActive.Size = new System.Drawing.Size(65, 23);
            this.cmbActive.Leave += new System.EventHandler(this.productCodeToolStrip_TextLeave);
            // 
            // fillByToolStripButton
            // 
            this.fillByToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fillByToolStripButton.Name = "fillByToolStripButton";
            this.fillByToolStripButton.Size = new System.Drawing.Size(46, 22);
            this.fillByToolStripButton.Text = "Search";
            this.fillByToolStripButton.Click += new System.EventHandler(this.fillByToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tbsAdvancedSearched
            // 
            this.tbsAdvancedSearched.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbsAdvancedSearched.Name = "tbsAdvancedSearched";
            this.tbsAdvancedSearched.Size = new System.Drawing.Size(70, 22);
            this.tbsAdvancedSearched.Text = "Adv Search";
            this.tbsAdvancedSearched.Click += new System.EventHandler(this.tbsAdvancedSearched_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbClear
            // 
            this.tsbClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(38, 22);
            this.tsbClear.Text = "Clear";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // gb_view
            // 
            this.gb_view.BackColor = System.Drawing.Color.Transparent;
            this.gb_view.Controls.Add(this.view_dgv);
            this.gb_view.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gb_view.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gb_view.Location = new System.Drawing.Point(12, 46);
            this.gb_view.Name = "gb_view";
            this.gb_view.Size = new System.Drawing.Size(233, 494);
            this.gb_view.TabIndex = 2;
            this.gb_view.TabStop = false;
            this.gb_view.Text = "View Products";
            // 
            // taxBindingSource
            // 
            this.taxBindingSource.DataMember = "TaxLookupWithNull";
            // 
            // btnTabularView
            // 
            this.btnTabularView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTabularView.Image = global::Semnox.Parafait.Inventory.Properties.Resources.autoorder;
            this.btnTabularView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTabularView.Location = new System.Drawing.Point(110, 548);
            this.btnTabularView.Name = "btnTabularView";
            this.btnTabularView.Size = new System.Drawing.Size(110, 23);
            this.btnTabularView.TabIndex = 120;
            this.btnTabularView.Text = "List View";
            this.btnTabularView.UseVisualStyleBackColor = true;
            this.btnTabularView.Click += new System.EventHandler(this.btnTabularView_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Image = global::Semnox.Parafait.Inventory.Properties.Resources.cancel;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(352, 558);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 122;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpload.Image = global::Semnox.Parafait.Inventory.Properties.Resources.XLS_Icon;
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpload.Location = new System.Drawing.Point(239, 558);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(91, 23);
            this.btnUpload.TabIndex = 121;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.CausesValidation = false;
            this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDelete.Image = global::Semnox.Parafait.Inventory.Properties.Resources.removegift;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(633, 558);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(86, 23);
            this.btnDelete.TabIndex = 125;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDuplicate
            // 
            this.btnDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDuplicate.Image = global::Semnox.Parafait.Inventory.Properties.Resources.duplicate;
            this.btnDuplicate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDuplicate.Location = new System.Drawing.Point(442, 558);
            this.btnDuplicate.Name = "btnDuplicate";
            this.btnDuplicate.Size = new System.Drawing.Size(84, 23);
            this.btnDuplicate.TabIndex = 123;
            this.btnDuplicate.Text = "Duplicate";
            this.btnDuplicate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDuplicate.UseVisualStyleBackColor = true;
            this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::Semnox.Parafait.Inventory.Properties.Resources.parsmallest;
            this.pictureBox1.Location = new System.Drawing.Point(9, 549);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(81, 38);
            this.pictureBox1.TabIndex = 35;
            this.pictureBox1.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Image = global::Semnox.Parafait.Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(737, 558);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 23);
            this.btnSave.TabIndex = 119;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNew.Image = global::Semnox.Parafait.Inventory.Properties.Resources.add1;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(542, 558);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 124;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // tvBOM
            // 
            this.tvBOM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvBOM.Location = new System.Drawing.Point(963, 37);
            this.tvBOM.Name = "tvBOM";
            this.tvBOM.Size = new System.Drawing.Size(209, 503);
            this.tvBOM.TabIndex = 131;
            // 
            // EditProduct
            // 
            this.EditProduct.Enabled = false;
            this.EditProduct.Name = "EditProduct";
            this.EditProduct.Size = new System.Drawing.Size(139, 22);
            this.EditProduct.Text = "Edit Product";
            this.EditProduct.Click += new System.EventHandler(this.EditProduct_Click);
            // 
            // Add
            // 
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(139, 22);
            this.Add.Text = "Add Child";
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Remove
            // 
            this.Remove.Enabled = false;
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(139, 22);
            this.Remove.Text = "Remove";
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
            this.toolStripSeparator1.Visible = false;
            // 
            // menuRefresh
            // 
            this.menuRefresh.Enabled = false;
            this.menuRefresh.Name = "menuRefresh";
            this.menuRefresh.Size = new System.Drawing.Size(139, 22);
            this.menuRefresh.Text = "Refresh";
            this.menuRefresh.Click += new System.EventHandler(this.menuRefresh_Click);
            // 
            // EditBOM
            // 
            this.EditBOM.Name = "EditBOM";
            this.EditBOM.Size = new System.Drawing.Size(180, 22);
            this.EditBOM.Text = "Edit BOM";
            this.EditBOM.Click += new System.EventHandler(this.EditBOM_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEdit.CausesValidation = false;
            this.btnEdit.Enabled = false;
            this.btnEdit.Image = global::Semnox.Parafait.Inventory.Properties.Resources.autoorder;
            this.btnEdit.Location = new System.Drawing.Point(972, 552);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(30, 25);
            this.btnEdit.TabIndex = 127;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.CausesValidation = false;
            this.btnAdd.Image = global::Semnox.Parafait.Inventory.Properties.Resources.add1;
            this.btnAdd.Location = new System.Drawing.Point(1025, 552);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(30, 25);
            this.btnAdd.TabIndex = 128;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.CausesValidation = false;
            this.btnRemove.Enabled = false;
            this.btnRemove.Image = global::Semnox.Parafait.Inventory.Properties.Resources.ordercancel1;
            this.btnRemove.Location = new System.Drawing.Point(1078, 552);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(30, 25);
            this.btnRemove.TabIndex = 129;
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnPrintBOM
            // 
            this.btnPrintBOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrintBOM.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPrintBOM.Image = global::Semnox.Parafait.Inventory.Properties.Resources.printer;
            this.btnPrintBOM.Location = new System.Drawing.Point(1132, 552);
            this.btnPrintBOM.Name = "btnPrintBOM";
            this.btnPrintBOM.Size = new System.Drawing.Size(30, 25);
            this.btnPrintBOM.TabIndex = 130;
            this.btnPrintBOM.UseVisualStyleBackColor = true;
            this.btnPrintBOM.Click += new System.EventHandler(this.btnPrintBOM_Click);
            // 
            // productBarcodebindingSource
            // 
            this.productBarcodebindingSource.DataMember = "ProductBarcode";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Image = global::Semnox.Parafait.Inventory.Properties.Resources.Refresh;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(841, 558);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(97, 23);
            this.btnRefresh.TabIndex = 126;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(14, 480);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 17);
            this.label12.TabIndex = 29;
            this.label12.Text = "Remarks:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRemarks
            // 
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.Location = new System.Drawing.Point(105, 477);
            this.txtRemarks.Multiline = false;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(590, 20);
            this.txtRemarks.TabIndex = 118;
            this.txtRemarks.Text = "";
            // 
            // lblProductid
            // 
            this.lblProductid.AutoSize = true;
            this.lblProductid.Location = new System.Drawing.Point(115, 22);
            this.lblProductid.Name = "lblProductid";
            this.lblProductid.Size = new System.Drawing.Size(0, 13);
            this.lblProductid.TabIndex = 36;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label29);
            this.groupBox1.Controls.Add(this.txtYieldPer);
            this.groupBox1.Controls.Add(this.cbxCostHasTax);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lbl_error_sale_price);
            this.groupBox1.Controls.Add(this.txtSalePrice);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.txtCostVariancePer);
            this.groupBox1.Controls.Add(this.txtUpperCostLimit);
            this.groupBox1.Controls.Add(this.lnkBuildCostfromBOM);
            this.groupBox1.Controls.Add(this.txtLowerCostLimit);
            this.groupBox1.Controls.Add(this.txtInnerPackQty);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.txtCost);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtLastpurchaseprice);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtItemMarkupPercent);
            this.groupBox1.Controls.Add(this.lblItemMarkupPercent);
            this.groupBox1.Location = new System.Drawing.Point(8, 384);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(692, 87);
            this.groupBox1.TabIndex = 109;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Costing";
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(343, 64);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(88, 15);
            this.label29.TabIndex = 136;
            this.label29.Text = "Yield %";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtYieldPer
            // 
            this.txtYieldPer.Location = new System.Drawing.Point(437, 61);
            this.txtYieldPer.Name = "txtYieldPer";
            this.txtYieldPer.Size = new System.Drawing.Size(66, 20);
            this.txtYieldPer.TabIndex = 137;
            this.txtYieldPer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbxCostHasTax
            // 
            this.cbxCostHasTax.Checked = true;
            this.cbxCostHasTax.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxCostHasTax.Location = new System.Drawing.Point(266, 59);
            this.cbxCostHasTax.Name = "cbxCostHasTax";
            this.cbxCostHasTax.Size = new System.Drawing.Size(16, 24);
            this.cbxCostHasTax.TabIndex = 134;
            this.cbxCostHasTax.Text = "Active?:";
            this.cbxCostHasTax.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(151, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 135;
            this.label3.Text = "Cost Includes Tax:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_error_sale_price
            // 
            this.lbl_error_sale_price.AutoSize = true;
            this.lbl_error_sale_price.ForeColor = System.Drawing.Color.Yellow;
            this.lbl_error_sale_price.Location = new System.Drawing.Point(553, 65);
            this.lbl_error_sale_price.Name = "lbl_error_sale_price";
            this.lbl_error_sale_price.Size = new System.Drawing.Size(129, 13);
            this.lbl_error_sale_price.TabIndex = 72;
            this.lbl_error_sale_price.Text = "Please enter valid amount";
            this.lbl_error_sale_price.Visible = false;
            // 
            // txtSalePrice
            // 
            this.txtSalePrice.Location = new System.Drawing.Point(616, 12);
            this.txtSalePrice.Name = "txtSalePrice";
            this.txtSalePrice.Size = new System.Drawing.Size(66, 20);
            this.txtSalePrice.TabIndex = 115;
            this.txtSalePrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(540, 16);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(72, 13);
            this.label18.TabIndex = 71;
            this.label18.Text = "Sale Price:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(177, 41);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(87, 13);
            this.label24.TabIndex = 69;
            this.label24.Text = "Upper Cost Limit:";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(177, 16);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(87, 13);
            this.label23.TabIndex = 68;
            this.label23.Text = "Lower Cost Limit:";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(343, 15);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(88, 15);
            this.label22.TabIndex = 67;
            this.label22.Text = "Variance %";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCostVariancePer
            // 
            this.txtCostVariancePer.Location = new System.Drawing.Point(437, 12);
            this.txtCostVariancePer.Name = "txtCostVariancePer";
            this.txtCostVariancePer.Size = new System.Drawing.Size(66, 20);
            this.txtCostVariancePer.TabIndex = 113;
            this.txtCostVariancePer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCostVariancePer.Validating += new System.ComponentModel.CancelEventHandler(this.txtCostVariancePer_Validating);
            // 
            // txtUpperCostLimit
            // 
            this.txtUpperCostLimit.Location = new System.Drawing.Point(266, 37);
            this.txtUpperCostLimit.Name = "txtUpperCostLimit";
            this.txtUpperCostLimit.Size = new System.Drawing.Size(66, 20);
            this.txtUpperCostLimit.TabIndex = 112;
            this.txtUpperCostLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtUpperCostLimit.Validating += new System.ComponentModel.CancelEventHandler(this.txtUpperCostLimit_Validating);
            // 
            // lnkBuildCostfromBOM
            // 
            this.lnkBuildCostfromBOM.AutoSize = true;
            this.lnkBuildCostfromBOM.Location = new System.Drawing.Point(11, 61);
            this.lnkBuildCostfromBOM.Name = "lnkBuildCostfromBOM";
            this.lnkBuildCostfromBOM.Size = new System.Drawing.Size(80, 13);
            this.lnkBuildCostfromBOM.TabIndex = 117;
            this.lnkBuildCostfromBOM.TabStop = true;
            this.lnkBuildCostfromBOM.Text = "Build from BOM";
            this.lnkBuildCostfromBOM.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkBuildCostfromBOM_LinkClicked);
            // 
            // txtLowerCostLimit
            // 
            this.txtLowerCostLimit.Location = new System.Drawing.Point(266, 12);
            this.txtLowerCostLimit.Name = "txtLowerCostLimit";
            this.txtLowerCostLimit.Size = new System.Drawing.Size(66, 20);
            this.txtLowerCostLimit.TabIndex = 111;
            this.txtLowerCostLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLowerCostLimit.Validating += new System.ComponentModel.CancelEventHandler(this.txtLowerCostLimit_Validating);
            // 
            // txtInnerPackQty
            // 
            this.txtInnerPackQty.Location = new System.Drawing.Point(97, 12);
            this.txtInnerPackQty.Name = "txtInnerPackQty";
            this.txtInnerPackQty.Size = new System.Drawing.Size(66, 20);
            this.txtInnerPackQty.TabIndex = 109;
            this.txtInnerPackQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtInnerPackQty.Validating += new System.ComponentModel.CancelEventHandler(this.txtInnerPackQty_Validating);
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(14, 16);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(80, 13);
            this.label21.TabIndex = 63;
            this.label21.Text = "Inner Cost Unit:";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCost
            // 
            this.txtCost.Location = new System.Drawing.Point(97, 37);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(66, 20);
            this.txtCost.TabIndex = 110;
            this.txtCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCost.Validating += new System.ComponentModel.CancelEventHandler(this.txtCost_Validating);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(63, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Cost:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLastpurchaseprice
            // 
            this.txtLastpurchaseprice.Location = new System.Drawing.Point(437, 37);
            this.txtLastpurchaseprice.Name = "txtLastpurchaseprice";
            this.txtLastpurchaseprice.ReadOnly = true;
            this.txtLastpurchaseprice.Size = new System.Drawing.Size(66, 20);
            this.txtLastpurchaseprice.TabIndex = 114;
            this.txtLastpurchaseprice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(343, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 15);
            this.label6.TabIndex = 17;
            this.label6.Text = "Last Purch Price:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtItemMarkupPercent
            // 
            this.txtItemMarkupPercent.Location = new System.Drawing.Point(616, 39);
            this.txtItemMarkupPercent.Name = "txtItemMarkupPercent";
            this.txtItemMarkupPercent.Size = new System.Drawing.Size(66, 20);
            this.txtItemMarkupPercent.TabIndex = 116;
            this.txtItemMarkupPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtItemMarkupPercent.Validating += new System.ComponentModel.CancelEventHandler(this.TxtItemMarkupPercent_Validating);
            // 
            // lblItemMarkupPercent
            // 
            this.lblItemMarkupPercent.Location = new System.Drawing.Point(540, 43);
            this.lblItemMarkupPercent.Name = "lblItemMarkupPercent";
            this.lblItemMarkupPercent.Size = new System.Drawing.Size(72, 13);
            this.lblItemMarkupPercent.TabIndex = 19;
            this.lblItemMarkupPercent.Text = "Markup %:";
            this.lblItemMarkupPercent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPreparationTime);
            this.groupBox2.Controls.Add(this.lblPreparationTime);
            this.groupBox2.Controls.Add(this.cbxIncludeInPlan);
            this.groupBox2.Controls.Add(this.label26);
            this.groupBox2.Controls.Add(this.txtIndays);
            this.groupBox2.Controls.Add(this.cbxMarketListItem);
            this.groupBox2.Controls.Add(this.cbxLotControlled);
            this.groupBox2.Controls.Add(this.ddlIssueApproach);
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.ddlExpiryType);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.btnSKUSegments);
            this.groupBox2.Controls.Add(this.label25);
            this.groupBox2.Controls.Add(this.cbxIsRedeemable);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.cbxIsSellable);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.cbxPurchaseable);
            this.groupBox2.Location = new System.Drawing.Point(8, 297);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(692, 78);
            this.groupBox2.TabIndex = 100;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Item Attributes";
            // 
            // txtPreparationTime
            // 
            this.txtPreparationTime.Location = new System.Drawing.Point(510, 47);
            this.txtPreparationTime.Name = "txtPreparationTime";
            this.txtPreparationTime.Size = new System.Drawing.Size(41, 20);
            this.txtPreparationTime.TabIndex = 138;
            this.txtPreparationTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblPreparationTime
            // 
            this.lblPreparationTime.Location = new System.Drawing.Point(379, 45);
            this.lblPreparationTime.Name = "lblPreparationTime";
            this.lblPreparationTime.Size = new System.Drawing.Size(129, 24);
            this.lblPreparationTime.TabIndex = 112;
            this.lblPreparationTime.Text = "Preparation Time (Mins):";
            this.lblPreparationTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxIncludeInPlan
            // 
            this.cbxIncludeInPlan.Location = new System.Drawing.Point(539, 18);
            this.cbxIncludeInPlan.Name = "cbxIncludeInPlan";
            this.cbxIncludeInPlan.Size = new System.Drawing.Size(16, 21);
            this.cbxIncludeInPlan.TabIndex = 110;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(449, 22);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(86, 13);
            this.label26.TabIndex = 109;
            this.label26.Text = "Include in Plan?:";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtIndays
            // 
            this.txtIndays.Location = new System.Drawing.Point(166, 47);
            this.txtIndays.Name = "txtIndays";
            this.txtIndays.Size = new System.Drawing.Size(39, 20);
            this.txtIndays.TabIndex = 106;
            this.txtIndays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtIndays.Visible = false;
            // 
            // cbxMarketListItem
            // 
            this.cbxMarketListItem.AutoSize = true;
            this.cbxMarketListItem.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxMarketListItem.Location = new System.Drawing.Point(568, 20);
            this.cbxMarketListItem.Name = "cbxMarketListItem";
            this.cbxMarketListItem.Size = new System.Drawing.Size(110, 17);
            this.cbxMarketListItem.TabIndex = 104;
            this.cbxMarketListItem.Text = "Market List Item?:";
            this.cbxMarketListItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxMarketListItem.UseVisualStyleBackColor = true;
            this.cbxMarketListItem.CheckedChanged += new System.EventHandler(this.cbxMarketListItem_CheckedChanged);
            // 
            // cbxLotControlled
            // 
            this.cbxLotControlled.AutoSize = true;
            this.cbxLotControlled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxLotControlled.Location = new System.Drawing.Point(344, 20);
            this.cbxLotControlled.Name = "cbxLotControlled";
            this.cbxLotControlled.Size = new System.Drawing.Size(100, 17);
            this.cbxLotControlled.TabIndex = 103;
            this.cbxLotControlled.Text = "Lot Controlled?:";
            this.cbxLotControlled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxLotControlled.UseVisualStyleBackColor = true;
            this.cbxLotControlled.CheckedChanged += new System.EventHandler(this.cbxLotControlled_CheckedChanged);
            // 
            // ddlIssueApproach
            // 
            this.ddlIssueApproach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlIssueApproach.FormattingEnabled = true;
            this.ddlIssueApproach.Items.AddRange(new object[] {
            "None",
            "FIFO",
            "FEFO"});
            this.ddlIssueApproach.Location = new System.Drawing.Point(314, 47);
            this.ddlIssueApproach.Name = "ddlIssueApproach";
            this.ddlIssueApproach.Size = new System.Drawing.Size(65, 21);
            this.ddlIssueApproach.TabIndex = 107;
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(211, 51);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(99, 13);
            this.label27.TabIndex = 87;
            this.label27.Text = "Issuing Approach:*";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlExpiryType
            // 
            this.ddlExpiryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlExpiryType.FormattingEnabled = true;
            this.ddlExpiryType.Items.AddRange(new object[] {
            "None",
            "In Days",
            "Expiry Date"});
            this.ddlExpiryType.Location = new System.Drawing.Point(94, 47);
            this.ddlExpiryType.Name = "ddlExpiryType";
            this.ddlExpiryType.Size = new System.Drawing.Size(66, 21);
            this.ddlExpiryType.TabIndex = 105;
            this.ddlExpiryType.SelectedIndexChanged += new System.EventHandler(this.ddlExpiryType_SelectedIndexChanged);
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(21, 51);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(69, 13);
            this.label28.TabIndex = 85;
            this.label28.Text = "Expiry Type:*";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSKUSegments
            // 
            this.btnSKUSegments.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSKUSegments.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSKUSegments.Location = new System.Drawing.Point(635, 47);
            this.btnSKUSegments.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnSKUSegments.Name = "btnSKUSegments";
            this.btnSKUSegments.Size = new System.Drawing.Size(47, 20);
            this.btnSKUSegments.TabIndex = 108;
            this.btnSKUSegments.Text = "....";
            this.btnSKUSegments.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSKUSegments.UseVisualStyleBackColor = true;
            this.btnSKUSegments.Click += new System.EventHandler(this.btnSKUSegments_Click);
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(524, 51);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(112, 13);
            this.label25.TabIndex = 67;
            this.label25.Text = "SKU Segments:";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxIsRedeemable
            // 
            this.cbxIsRedeemable.Checked = true;
            this.cbxIsRedeemable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxIsRedeemable.Location = new System.Drawing.Point(309, 18);
            this.cbxIsRedeemable.Name = "cbxIsRedeemable";
            this.cbxIsRedeemable.Size = new System.Drawing.Size(28, 21);
            this.cbxIsRedeemable.TabIndex = 102;
            this.cbxIsRedeemable.CheckedChanged += new System.EventHandler(this.CbxIsRedeemable_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(227, 22);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(76, 13);
            this.label16.TabIndex = 46;
            this.label16.Text = "Redeemable?:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxIsSellable
            // 
            this.cbxIsSellable.Location = new System.Drawing.Point(193, 18);
            this.cbxIsSellable.Name = "cbxIsSellable";
            this.cbxIsSellable.Size = new System.Drawing.Size(16, 21);
            this.cbxIsSellable.TabIndex = 101;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(137, 22);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 13);
            this.label17.TabIndex = 44;
            this.label17.Text = "Sellable?:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxPurchaseable
            // 
            this.cbxPurchaseable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxPurchaseable.Checked = true;
            this.cbxPurchaseable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxPurchaseable.Location = new System.Drawing.Point(10, 19);
            this.cbxPurchaseable.Name = "cbxPurchaseable";
            this.cbxPurchaseable.Size = new System.Drawing.Size(115, 19);
            this.cbxPurchaseable.TabIndex = 100;
            this.cbxPurchaseable.Text = "Inventory Item?:";
            this.cbxPurchaseable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxPurchaseable.UseVisualStyleBackColor = true;
            // 
            // gb_add
            // 
            this.gb_add.BackColor = System.Drawing.Color.Transparent;
            this.gb_add.Controls.Add(this.groupBox3);
            this.gb_add.Controls.Add(this.groupBox2);
            this.gb_add.Controls.Add(this.groupBox1);
            this.gb_add.Controls.Add(this.txtRemarks);
            this.gb_add.Controls.Add(this.label12);
            this.gb_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gb_add.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gb_add.Location = new System.Drawing.Point(251, 28);
            this.gb_add.Name = "gb_add";
            this.gb_add.Size = new System.Drawing.Size(706, 512);
            this.gb_add.TabIndex = 4;
            this.gb_add.TabStop = false;
            this.gb_add.Text = "Maintain Products";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lnkrecipeDescription);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.cmbInventoryUOM);
            this.groupBox3.Controls.Add(this.btnShowDisplayGroups);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.cmbItemType);
            this.groupBox3.Controls.Add(this.txtDisplayGroup);
            this.groupBox3.Controls.Add(this.lblDisplayGrp);
            this.groupBox3.Controls.Add(this.txtCode);
            this.groupBox3.Controls.Add(this.cbxAutoUpdateMarkup);
            this.groupBox3.Controls.Add(this.lnkStock);
            this.groupBox3.Controls.Add(this.txtProductName);
            this.groupBox3.Controls.Add(this.lblProductName);
            this.groupBox3.Controls.Add(this.btnUPCBarcode);
            this.groupBox3.Controls.Add(this.btnCustom);
            this.groupBox3.Controls.Add(this.lblProductid);
            this.groupBox3.Controls.Add(this.txtBarcode);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.cbxTaxInclusiveCost);
            this.groupBox3.Controls.Add(this.txtTurnInPIT);
            this.groupBox3.Controls.Add(this.lblImageFileName);
            this.groupBox3.Controls.Add(this.lnkClearImage);
            this.groupBox3.Controls.Add(this.lnkProductImage);
            this.groupBox3.Controls.Add(this.pbProductImage);
            this.groupBox3.Controls.Add(this.lblActive);
            this.groupBox3.Controls.Add(this.cbxActive);
            this.groupBox3.Controls.Add(this.btnAddUOM);
            this.groupBox3.Controls.Add(this.ddlUOM);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.btnAddTax);
            this.groupBox3.Controls.Add(this.ddlTax);
            this.groupBox3.Controls.Add(this.ddlOutboundLocation);
            this.groupBox3.Controls.Add(this.lblOutboundLocation);
            this.groupBox3.Controls.Add(this.btnGenerateBarCode);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.btnAddvendor);
            this.groupBox3.Controls.Add(this.btnAddlocation);
            this.groupBox3.Controls.Add(this.btnAddcategory);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.txtPit);
            this.groupBox3.Controls.Add(this.ddlPreferredvendor);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.ddlDefaultlocation);
            this.groupBox3.Controls.Add(this.lblInboundLocation);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtReorderquantity);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtReorderpoint);
            this.groupBox3.Controls.Add(this.ddlCategory);
            this.groupBox3.Controls.Add(this.lblCategory);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtDescription);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(8, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(692, 270);
            this.groupBox3.TabIndex = 72;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Item Details";
            // 
            // lnkrecipeDescription
            // 
            this.lnkrecipeDescription.AutoSize = true;
            this.lnkrecipeDescription.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkrecipeDescription.Location = new System.Drawing.Point(397, 69);
            this.lnkrecipeDescription.Name = "lnkrecipeDescription";
            this.lnkrecipeDescription.Size = new System.Drawing.Size(114, 15);
            this.lnkrecipeDescription.TabIndex = 137;
            this.lnkrecipeDescription.TabStop = true;
            this.lnkrecipeDescription.Text = "Recipe Description";
            this.lnkrecipeDescription.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkrecipeDescription_LinkClicked);
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(351, 126);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(83, 15);
            this.label15.TabIndex = 136;
            this.label15.Text = "Inventory UOM:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbInventoryUOM
            // 
            this.cmbInventoryUOM.DataSource = this.uOMDTOBindingSource;
            this.cmbInventoryUOM.DisplayMember = "UOM";
            this.cmbInventoryUOM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInventoryUOM.FormattingEnabled = true;
            this.cmbInventoryUOM.Location = new System.Drawing.Point(435, 123);
            this.cmbInventoryUOM.Name = "cmbInventoryUOM";
            this.cmbInventoryUOM.Size = new System.Drawing.Size(73, 21);
            this.cmbInventoryUOM.TabIndex = 135;
            this.cmbInventoryUOM.ValueMember = "UOMId";
            // 
            // uOMDTOBindingSource
            // 
            this.uOMDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.UOMDTO);
            // 
            // btnShowDisplayGroups
            // 
            this.btnShowDisplayGroups.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnShowDisplayGroups.BackColor = System.Drawing.Color.Gainsboro;
            this.btnShowDisplayGroups.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnShowDisplayGroups.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowDisplayGroups.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowDisplayGroups.Location = new System.Drawing.Point(511, 153);
            this.btnShowDisplayGroups.Name = "btnShowDisplayGroups";
            this.btnShowDisplayGroups.Size = new System.Drawing.Size(30, 20);
            this.btnShowDisplayGroups.TabIndex = 92;
            this.btnShowDisplayGroups.Text = "+";
            this.btnShowDisplayGroups.UseVisualStyleBackColor = false;
            this.btnShowDisplayGroups.Click += new System.EventHandler(this.btnShowDisplayGroups_Click);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(361, 237);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 25);
            this.label9.TabIndex = 134;
            this.label9.Text = "Item Type:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbItemType
            // 
            this.cmbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemType.FormattingEnabled = true;
            this.cmbItemType.Location = new System.Drawing.Point(436, 239);
            this.cmbItemType.Name = "cmbItemType";
            this.cmbItemType.Size = new System.Drawing.Size(121, 21);
            this.cmbItemType.TabIndex = 133;
            // 
            // txtDisplayGroup
            // 
            this.txtDisplayGroup.Location = new System.Drawing.Point(435, 153);
            this.txtDisplayGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDisplayGroup.Name = "txtDisplayGroup";
            this.txtDisplayGroup.ReadOnly = true;
            this.txtDisplayGroup.Size = new System.Drawing.Size(73, 20);
            this.txtDisplayGroup.TabIndex = 91;
            // 
            // lblDisplayGrp
            // 
            this.lblDisplayGrp.Location = new System.Drawing.Point(351, 156);
            this.lblDisplayGrp.Name = "lblDisplayGrp";
            this.lblDisplayGrp.Size = new System.Drawing.Size(82, 15);
            this.lblDisplayGrp.TabIndex = 91;
            this.lblDisplayGrp.Text = "Display Group:";
            this.lblDisplayGrp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(111, 42);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(121, 20);
            this.txtCode.TabIndex = 72;
            this.txtCode.Leave += new System.EventHandler(this.TxtCode_Leave);
            // 
            // cbxAutoUpdateMarkup
            // 
            this.cbxAutoUpdateMarkup.AutoSize = true;
            this.cbxAutoUpdateMarkup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAutoUpdateMarkup.Location = new System.Drawing.Point(570, 213);
            this.cbxAutoUpdateMarkup.Name = "cbxAutoUpdateMarkup";
            this.cbxAutoUpdateMarkup.Size = new System.Drawing.Size(112, 17);
            this.cbxAutoUpdateMarkup.TabIndex = 93;
            this.cbxAutoUpdateMarkup.Text = "Auto Update PIT?";
            this.cbxAutoUpdateMarkup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAutoUpdateMarkup.UseVisualStyleBackColor = true;
            this.cbxAutoUpdateMarkup.CheckedChanged += new System.EventHandler(this.CbxAutoUpdateMarkup_CheckedChanged);
            // 
            // lnkStock
            // 
            this.lnkStock.AutoSize = true;
            this.lnkStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkStock.Location = new System.Drawing.Point(254, 22);
            this.lnkStock.Name = "lnkStock";
            this.lnkStock.Size = new System.Drawing.Size(37, 15);
            this.lnkStock.TabIndex = 133;
            this.lnkStock.TabStop = true;
            this.lnkStock.Text = "Stock";
            this.lnkStock.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkStock_LinkClicked);
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(111, 68);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(121, 20);
            this.txtProductName.TabIndex = 73;
            // 
            // lblProductName
            // 
            this.lblProductName.Location = new System.Drawing.Point(5, 71);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(102, 15);
            this.lblProductName.TabIndex = 118;
            this.lblProductName.Text = "Product Name:";
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnUPCBarcode
            // 
            this.btnUPCBarcode.AutoSize = true;
            this.btnUPCBarcode.FlatAppearance.BorderSize = 0;
            this.btnUPCBarcode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SeaGreen;
            this.btnUPCBarcode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.btnUPCBarcode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUPCBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUPCBarcode.ForeColor = System.Drawing.SystemColors.Control;
            this.btnUPCBarcode.Location = new System.Drawing.Point(111, 180);
            this.btnUPCBarcode.Name = "btnUPCBarcode";
            this.btnUPCBarcode.Size = new System.Drawing.Size(133, 23);
            this.btnUPCBarcode.TabIndex = 90;
            this.btnUPCBarcode.Text = "Generate UPC Bar Code";
            this.btnUPCBarcode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUPCBarcode.UseVisualStyleBackColor = true;
            this.btnUPCBarcode.Click += new System.EventHandler(this.btnUPCBarcode_Click);
            // 
            // btnCustom
            // 
            this.btnCustom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustom.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCustom.Location = new System.Drawing.Point(599, 238);
            this.btnCustom.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnCustom.Name = "btnCustom";
            this.btnCustom.Size = new System.Drawing.Size(66, 23);
            this.btnCustom.TabIndex = 97;
            this.btnCustom.Text = "Custom..";
            this.btnCustom.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCustom.UseVisualStyleBackColor = true;
            this.btnCustom.Click += new System.EventHandler(this.btnCustom_Click);
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(111, 154);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.ReadOnly = true;
            this.txtBarcode.Size = new System.Drawing.Size(121, 20);
            this.txtBarcode.TabIndex = 87;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(358, 184);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 15);
            this.label11.TabIndex = 111;
            this.label11.Text = "Turn-in PIT:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxTaxInclusiveCost
            // 
            this.cbxTaxInclusiveCost.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxTaxInclusiveCost.Checked = true;
            this.cbxTaxInclusiveCost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxTaxInclusiveCost.Location = new System.Drawing.Point(583, 91);
            this.cbxTaxInclusiveCost.Name = "cbxTaxInclusiveCost";
            this.cbxTaxInclusiveCost.Size = new System.Drawing.Size(100, 24);
            this.cbxTaxInclusiveCost.TabIndex = 86;
            this.cbxTaxInclusiveCost.Text = "Tax Inclusive?:";
            this.cbxTaxInclusiveCost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxTaxInclusiveCost.UseVisualStyleBackColor = true;
            // 
            // txtTurnInPIT
            // 
            this.txtTurnInPIT.Location = new System.Drawing.Point(435, 181);
            this.txtTurnInPIT.Name = "txtTurnInPIT";
            this.txtTurnInPIT.Size = new System.Drawing.Size(73, 20);
            this.txtTurnInPIT.TabIndex = 88;
            this.txtTurnInPIT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblImageFileName
            // 
            this.lblImageFileName.Location = new System.Drawing.Point(511, 42);
            this.lblImageFileName.Name = "lblImageFileName";
            this.lblImageFileName.Size = new System.Drawing.Size(82, 23);
            this.lblImageFileName.TabIndex = 109;
            this.lblImageFileName.Text = "ImageFileName";
            this.lblImageFileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lnkClearImage
            // 
            this.lnkClearImage.AutoSize = true;
            this.lnkClearImage.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lnkClearImage.Location = new System.Drawing.Point(562, 65);
            this.lnkClearImage.Name = "lnkClearImage";
            this.lnkClearImage.Size = new System.Drawing.Size(31, 13);
            this.lnkClearImage.TabIndex = 77;
            this.lnkClearImage.TabStop = true;
            this.lnkClearImage.Text = "Clear";
            this.lnkClearImage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearImage_LinkClicked);
            // 
            // lnkProductImage
            // 
            this.lnkProductImage.AutoSize = true;
            this.lnkProductImage.Location = new System.Drawing.Point(517, 21);
            this.lnkProductImage.Name = "lnkProductImage";
            this.lnkProductImage.Size = new System.Drawing.Size(76, 13);
            this.lnkProductImage.TabIndex = 76;
            this.lnkProductImage.TabStop = true;
            this.lnkProductImage.Text = "Product Image";
            this.lnkProductImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkProductImage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkProductImage_LinkClicked);
            // 
            // pbProductImage
            // 
            this.pbProductImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbProductImage.Location = new System.Drawing.Point(602, 22);
            this.pbProductImage.Name = "pbProductImage";
            this.pbProductImage.Size = new System.Drawing.Size(80, 60);
            this.pbProductImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbProductImage.TabIndex = 108;
            this.pbProductImage.TabStop = false;
            // 
            // lblActive
            // 
            this.lblActive.Location = new System.Drawing.Point(318, 21);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(113, 13);
            this.lblActive.TabIndex = 107;
            this.lblActive.Text = "Active?:";
            this.lblActive.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxActive
            // 
            this.cbxActive.Checked = true;
            this.cbxActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxActive.Location = new System.Drawing.Point(432, 15);
            this.cbxActive.Name = "cbxActive";
            this.cbxActive.Size = new System.Drawing.Size(16, 24);
            this.cbxActive.TabIndex = 74;
            this.cbxActive.Text = "Active?:";
            this.cbxActive.UseVisualStyleBackColor = true;
            // 
            // btnAddUOM
            // 
            this.btnAddUOM.FlatAppearance.BorderSize = 0;
            this.btnAddUOM.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SeaGreen;
            this.btnAddUOM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.btnAddUOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddUOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddUOM.ForeColor = System.Drawing.SystemColors.Control;
            this.btnAddUOM.Location = new System.Drawing.Point(237, 123);
            this.btnAddUOM.Name = "btnAddUOM";
            this.btnAddUOM.Size = new System.Drawing.Size(95, 23);
            this.btnAddUOM.TabIndex = 83;
            this.btnAddUOM.Text = "Add UOM";
            this.btnAddUOM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddUOM.UseVisualStyleBackColor = true;
            this.btnAddUOM.Click += new System.EventHandler(this.btnAddUOM_Click);
            // 
            // ddlUOM
            // 
            this.ddlUOM.DataSource = this.uOMDTOBindingSource;
            this.ddlUOM.DisplayMember = "UOMId";
            this.ddlUOM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlUOM.FormattingEnabled = true;
            this.ddlUOM.Location = new System.Drawing.Point(112, 125);
            this.ddlUOM.Name = "ddlUOM";
            this.ddlUOM.Size = new System.Drawing.Size(121, 21);
            this.ddlUOM.TabIndex = 82;
            this.ddlUOM.ValueMember = "UOMId";
            this.ddlUOM.SelectedIndexChanged += new System.EventHandler(this.ddlUOM_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(7, 128);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(102, 15);
            this.label20.TabIndex = 106;
            this.label20.Text = "Unit of Measure:*";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(361, 96);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(73, 15);
            this.label19.TabIndex = 105;
            this.label19.Text = "Tax:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnAddTax
            // 
            this.btnAddTax.FlatAppearance.BorderSize = 0;
            this.btnAddTax.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SeaGreen;
            this.btnAddTax.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.btnAddTax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTax.ForeColor = System.Drawing.SystemColors.Control;
            this.btnAddTax.Location = new System.Drawing.Point(511, 92);
            this.btnAddTax.Name = "btnAddTax";
            this.btnAddTax.Size = new System.Drawing.Size(66, 23);
            this.btnAddTax.TabIndex = 85;
            this.btnAddTax.Text = "Add Tax..";
            this.btnAddTax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddTax.UseVisualStyleBackColor = true;
            this.btnAddTax.Click += new System.EventHandler(this.btnAddTax_Click);
            // 
            // ddlTax
            // 
            this.ddlTax.DataSource = this.purchaseTaxDTOBindingSource;
            this.ddlTax.DisplayMember = "TaxName";
            this.ddlTax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTax.FormattingEnabled = true;
            this.ddlTax.Location = new System.Drawing.Point(435, 93);
            this.ddlTax.Name = "ddlTax";
            this.ddlTax.Size = new System.Drawing.Size(73, 21);
            this.ddlTax.TabIndex = 84;
            this.ddlTax.ValueMember = "TaxId";
            // 
            // purchaseTaxDTOBindingSource
            // 
            this.purchaseTaxDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.TaxDTO);
            // 
            // ddlOutboundLocation
            // 
            this.ddlOutboundLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ddlOutboundLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ddlOutboundLocation.DataSource = this.outBoundLocationDTOBindingSource;
            this.ddlOutboundLocation.DisplayMember = "LocationId";
            this.ddlOutboundLocation.FormattingEnabled = true;
            this.ddlOutboundLocation.Location = new System.Drawing.Point(436, 211);
            this.ddlOutboundLocation.Name = "ddlOutboundLocation";
            this.ddlOutboundLocation.Size = new System.Drawing.Size(121, 21);
            this.ddlOutboundLocation.TabIndex = 96;
            this.ddlOutboundLocation.ValueMember = "LocationId";
            // 
            // outBoundLocationDTOBindingSource
            // 
            this.outBoundLocationDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.LocationDTO);
            // 
            // lblOutboundLocation
            // 
            this.lblOutboundLocation.Location = new System.Drawing.Point(334, 207);
            this.lblOutboundLocation.Name = "lblOutboundLocation";
            this.lblOutboundLocation.Size = new System.Drawing.Size(105, 29);
            this.lblOutboundLocation.TabIndex = 101;
            this.lblOutboundLocation.Tag = "Outbound Location";
            this.lblOutboundLocation.Text = "Outbound Location:*";
            this.lblOutboundLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnGenerateBarCode
            // 
            this.btnGenerateBarCode.FlatAppearance.BorderSize = 0;
            this.btnGenerateBarCode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SeaGreen;
            this.btnGenerateBarCode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.btnGenerateBarCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateBarCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateBarCode.ForeColor = System.Drawing.SystemColors.Control;
            this.btnGenerateBarCode.Location = new System.Drawing.Point(237, 152);
            this.btnGenerateBarCode.Name = "btnGenerateBarCode";
            this.btnGenerateBarCode.Size = new System.Drawing.Size(95, 23);
            this.btnGenerateBarCode.TabIndex = 87;
            this.btnGenerateBarCode.Text = "Edit Bar Codes..";
            this.btnGenerateBarCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerateBarCode.UseVisualStyleBackColor = true;
            this.btnGenerateBarCode.Click += new System.EventHandler(this.btnGenerateBarCode_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 15);
            this.label5.TabIndex = 97;
            this.label5.Text = "Bar Code:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(7, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(102, 15);
            this.label14.TabIndex = 94;
            this.label14.Text = "Product Id:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnAddvendor
            // 
            this.btnAddvendor.FlatAppearance.BorderSize = 0;
            this.btnAddvendor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SeaGreen;
            this.btnAddvendor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.btnAddvendor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddvendor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddvendor.ForeColor = System.Drawing.SystemColors.Control;
            this.btnAddvendor.Location = new System.Drawing.Point(237, 238);
            this.btnAddvendor.Name = "btnAddvendor";
            this.btnAddvendor.Size = new System.Drawing.Size(95, 23);
            this.btnAddvendor.TabIndex = 99;
            this.btnAddvendor.Text = "Add Vendor..";
            this.btnAddvendor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddvendor.UseVisualStyleBackColor = true;
            this.btnAddvendor.Click += new System.EventHandler(this.btnAddvendor_Click);
            // 
            // btnAddlocation
            // 
            this.btnAddlocation.FlatAppearance.BorderSize = 0;
            this.btnAddlocation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SeaGreen;
            this.btnAddlocation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.btnAddlocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddlocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddlocation.ForeColor = System.Drawing.SystemColors.Control;
            this.btnAddlocation.Location = new System.Drawing.Point(237, 210);
            this.btnAddlocation.Name = "btnAddlocation";
            this.btnAddlocation.Size = new System.Drawing.Size(95, 23);
            this.btnAddlocation.TabIndex = 95;
            this.btnAddlocation.Text = "Add Location..";
            this.btnAddlocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddlocation.UseVisualStyleBackColor = true;
            this.btnAddlocation.Click += new System.EventHandler(this.btnAddlocation_Click);
            // 
            // btnAddcategory
            // 
            this.btnAddcategory.FlatAppearance.BorderSize = 0;
            this.btnAddcategory.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SeaGreen;
            this.btnAddcategory.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.btnAddcategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddcategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddcategory.ForeColor = System.Drawing.SystemColors.Control;
            this.btnAddcategory.Location = new System.Drawing.Point(237, 96);
            this.btnAddcategory.Name = "btnAddcategory";
            this.btnAddcategory.Size = new System.Drawing.Size(95, 23);
            this.btnAddcategory.TabIndex = 79;
            this.btnAddcategory.Text = "Add Category..";
            this.btnAddcategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddcategory.UseVisualStyleBackColor = true;
            this.btnAddcategory.Click += new System.EventHandler(this.btnAddcategory_Click);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(527, 184);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 15);
            this.label13.TabIndex = 83;
            this.label13.Text = "Price in Tickets:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPit
            // 
            this.txtPit.Location = new System.Drawing.Point(630, 181);
            this.txtPit.Name = "txtPit";
            this.txtPit.Size = new System.Drawing.Size(52, 20);
            this.txtPit.TabIndex = 89;
            this.txtPit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ddlPreferredvendor
            // 
            this.ddlPreferredvendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ddlPreferredvendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ddlPreferredvendor.DataSource = this.vendorDTOBindingSource;
            this.ddlPreferredvendor.DisplayMember = "VendorId";
            this.ddlPreferredvendor.FormattingEnabled = true;
            this.ddlPreferredvendor.Location = new System.Drawing.Point(111, 240);
            this.ddlPreferredvendor.Name = "ddlPreferredvendor";
            this.ddlPreferredvendor.Size = new System.Drawing.Size(121, 21);
            this.ddlPreferredvendor.TabIndex = 98;
            this.ddlPreferredvendor.ValueMember = "VendorId";
            this.ddlPreferredvendor.SelectedIndexChanged += new System.EventHandler(this.ddlPreferredvendor_SelectedIndexChanged);
            this.ddlPreferredvendor.Enter += new System.EventHandler(this.ddlPreferredvendor_SelectedIndexChanged);
            // 
            // vendorDTOBindingSource
            // 
            this.vendorDTOBindingSource.DataSource = typeof(Semnox.Parafait.Vendor.VendorDTO);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(7, 246);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 15);
            this.label10.TabIndex = 98;
            this.label10.Text = "Preferred Vendor:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlDefaultlocation
            // 
            this.ddlDefaultlocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ddlDefaultlocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ddlDefaultlocation.DataSource = this.inBoundLocationDTOBindingSource;
            this.ddlDefaultlocation.DisplayMember = "Name";
            this.ddlDefaultlocation.FormattingEnabled = true;
            this.ddlDefaultlocation.Location = new System.Drawing.Point(111, 211);
            this.ddlDefaultlocation.Name = "ddlDefaultlocation";
            this.ddlDefaultlocation.Size = new System.Drawing.Size(121, 21);
            this.ddlDefaultlocation.TabIndex = 94;
            this.ddlDefaultlocation.ValueMember = "LocationId";
            // 
            // inBoundLocationDTOBindingSource
            // 
            this.inBoundLocationDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.LocationDTO);
            // 
            // lblInboundLocation
            // 
            this.lblInboundLocation.Location = new System.Drawing.Point(7, 214);
            this.lblInboundLocation.Name = "lblInboundLocation";
            this.lblInboundLocation.Size = new System.Drawing.Size(102, 15);
            this.lblInboundLocation.TabIndex = 94;
            this.lblInboundLocation.Tag = "Inbound Location";
            this.lblInboundLocation.Text = "Inbound Location:*";
            this.lblInboundLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(515, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 15);
            this.label8.TabIndex = 91;
            this.label8.Text = "Reorder Quantity:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReorderquantity
            // 
            this.txtReorderquantity.Location = new System.Drawing.Point(629, 123);
            this.txtReorderquantity.Name = "txtReorderquantity";
            this.txtReorderquantity.Size = new System.Drawing.Size(52, 20);
            this.txtReorderquantity.TabIndex = 81;
            this.txtReorderquantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(547, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 15);
            this.label7.TabIndex = 90;
            this.label7.Text = "Reorder Point:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReorderpoint
            // 
            this.txtReorderpoint.Location = new System.Drawing.Point(629, 153);
            this.txtReorderpoint.Name = "txtReorderpoint";
            this.txtReorderpoint.Size = new System.Drawing.Size(52, 20);
            this.txtReorderpoint.TabIndex = 80;
            this.txtReorderpoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ddlCategory
            // 
            this.ddlCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ddlCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ddlCategory.DataSource = this.categoryDTOBindingSource;
            this.ddlCategory.DisplayMember = "CategoryId";
            this.ddlCategory.FormattingEnabled = true;
            this.ddlCategory.Location = new System.Drawing.Point(112, 97);
            this.ddlCategory.Name = "ddlCategory";
            this.ddlCategory.Size = new System.Drawing.Size(121, 21);
            this.ddlCategory.TabIndex = 78;
            this.ddlCategory.ValueMember = "CategoryId";
            // 
            // categoryDTOBindingSource
            // 
            this.categoryDTOBindingSource.DataSource = typeof(Semnox.Parafait.Category.CategoryDTO);
            // 
            // lblCategory
            // 
            this.lblCategory.Location = new System.Drawing.Point(7, 100);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(102, 15);
            this.lblCategory.TabIndex = 80;
            this.lblCategory.Tag = "Category";
            this.lblCategory.Text = "Category:*";
            this.lblCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(240, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 18);
            this.label2.TabIndex = 77;
            this.label2.Text = "Description:*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(316, 42);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(192, 20);
            this.txtDescription.TabIndex = 75;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 15);
            this.label1.TabIndex = 75;
            this.label1.Text = "Code:*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lnkPublishToSite
            // 
            this.lnkPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublishToSite.AutoSize = true;
            this.lnkPublishToSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublishToSite.Location = new System.Drawing.Point(838, 543);
            this.lnkPublishToSite.Name = "lnkPublishToSite";
            this.lnkPublishToSite.Size = new System.Drawing.Size(99, 13);
            this.lnkPublishToSite.TabIndex = 132;
            this.lnkPublishToSite.TabStop = true;
            this.lnkPublishToSite.Text = "Publish To Sites";
            this.lnkPublishToSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublishToSite_LinkClicked);
            // 
            // frmProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1175, 591);
            this.Controls.Add(this.lnkPublishToSite);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnPrintBOM);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnTabularView);
            this.Controls.Add(this.tvBOM);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnDuplicate);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gb_add);
            this.Controls.Add(this.fillByToolStrip);
            this.Controls.Add(this.gb_view);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmProduct";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Product";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmProduct_FormClosed);
            this.Load += new System.EventHandler(this.frmProduct_Load);
            ((System.ComponentModel.ISupportInitialize)(this.view_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productDTOBindingSource)).EndInit();
            this.fillByToolStrip.ResumeLayout(false);
            this.fillByToolStrip.PerformLayout();
            this.gb_view.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.taxBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productBarcodebindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gb_add.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uOMDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProductImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseTaxDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outBoundLocationDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inBoundLocationDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.categoryDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView view_dgv;
        //  private Redemption.RedemptionDataSetTableAdapters.ProductTableAdapter productTableAdapter;
        private System.Windows.Forms.ToolStrip fillByToolStrip;
        private System.Windows.Forms.ToolStripLabel productCodeToolStripLabel;
        private System.Windows.Forms.ToolStripTextBox productCodeToolStripTextBox;
        private System.Windows.Forms.ToolStripButton fillByToolStripButton;
        private System.Windows.Forms.GroupBox gb_view;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnDuplicate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.BindingSource taxBindingSource;
        private System.Windows.Forms.ToolStripComboBox cmbProductType;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        // private RedemptionDataSetTableAdapters.UOMTableAdapter uOMTableAdapter;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox cmbCategory;
        private System.Windows.Forms.Button btnTabularView;
        // private RedemptionDataSetTableAdapters.TaxLookupWithNullTableAdapter taxLookupWithNullTableAdapter;
        //  private RedemptionDataSetTableAdapters.VendorLookupWithNullTableAdapter vendorLookupWithNullTableAdapter;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox DescriptiontoolStripTextBox;
        private System.Windows.Forms.TreeView tvBOM;
        //private System.Windows.Forms.ContextMenuStrip BOMEditMenu;
        private System.Windows.Forms.ToolStripMenuItem EditBOM;
        private System.Windows.Forms.ToolStripMenuItem Add;
        private System.Windows.Forms.ToolStripMenuItem Remove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuRefresh;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripMenuItem EditProduct;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripComboBox cmbActive;
        private System.Windows.Forms.Button btnPrintBOM;
        private System.Windows.Forms.ToolStripTextBox BarcodetoolStripTextBox;
        private System.Windows.Forms.BindingSource productBarcodebindingSource;
        //  private RedemptionDataSetTableAdapters.ProductBarcodeTableAdapter productBarcodeTableAdapter;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ToolStripButton tbsAdvancedSearched;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RichTextBox txtRemarks;
        private System.Windows.Forms.Label lblProductid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_error_sale_price;
        private System.Windows.Forms.TextBox txtSalePrice;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtCostVariancePer;
        private System.Windows.Forms.TextBox txtUpperCostLimit;
        private System.Windows.Forms.LinkLabel lnkBuildCostfromBOM;
        private System.Windows.Forms.TextBox txtLowerCostLimit;
        private System.Windows.Forms.TextBox txtInnerPackQty;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtCost;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLastpurchaseprice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSKUSegments;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox cbxIsRedeemable;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox cbxIsSellable;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox cbxPurchaseable;
        private System.Windows.Forms.GroupBox gb_add;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnUPCBarcode;
        private System.Windows.Forms.Button btnCustom;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTurnInPIT;
        private System.Windows.Forms.Label lblImageFileName;
        private System.Windows.Forms.LinkLabel lnkClearImage;
        private System.Windows.Forms.LinkLabel lnkProductImage;
        private System.Windows.Forms.PictureBox pbProductImage;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.CheckBox cbxActive;
        private System.Windows.Forms.Button btnAddUOM;
        private System.Windows.Forms.ComboBox ddlUOM;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox cbxTaxInclusiveCost;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnAddTax;
        private System.Windows.Forms.ComboBox ddlTax;
        private AutoCompleteComboBox ddlOutboundLocation;
        private System.Windows.Forms.Label lblOutboundLocation;
        private System.Windows.Forms.Button btnGenerateBarCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnAddvendor;
        private System.Windows.Forms.Button btnAddlocation;
        private System.Windows.Forms.Button btnAddcategory;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtPit;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox ddlPreferredvendor;
        private System.Windows.Forms.Label label10;
        private AutoCompleteComboBox ddlDefaultlocation;
        private System.Windows.Forms.Label lblInboundLocation;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtReorderquantity;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtReorderpoint;
        private AutoCompleteComboBox ddlCategory;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlIssueApproach;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.ComboBox ddlExpiryType;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.CheckBox cbxMarketListItem;
        private System.Windows.Forms.CheckBox cbxLotControlled;
        private System.Windows.Forms.TextBox txtIndays;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.BindingSource productDTOBindingSource;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn isRedeemableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn outboundLocationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn salePriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxInclusiveCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lowerLimitCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn upperLimitCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costVariancePercentageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn turnInPriceInTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn segmentCategoryIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
        private System.Windows.Forms.DataGridViewTextBoxColumn uOMValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemMarkupPercentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn autoUpdateMarkupDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource outBoundLocationDTOBindingSource;
        private System.Windows.Forms.BindingSource vendorDTOBindingSource;
        private System.Windows.Forms.BindingSource inBoundLocationDTOBindingSource;
        private System.Windows.Forms.BindingSource categoryDTOBindingSource;
        private System.Windows.Forms.BindingSource uOMDTOBindingSource;
        private System.Windows.Forms.BindingSource purchaseTaxDTOBindingSource;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.TextBox txtItemMarkupPercent;
        private System.Windows.Forms.Label lblItemMarkupPercent;
        private System.Windows.Forms.CheckBox cbxAutoUpdateMarkup;
        private System.Windows.Forms.LinkLabel lnkPublishToSite;
        private System.Windows.Forms.LinkLabel lnkStock;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Button btnShowDisplayGroups;
        private System.Windows.Forms.TextBox txtDisplayGroup;
        private System.Windows.Forms.Label lblDisplayGrp;
        private System.Windows.Forms.CheckBox cbxCostHasTax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbItemType;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbInventoryUOM;
        private System.Windows.Forms.LinkLabel lnkrecipeDescription;
        private System.Windows.Forms.CheckBox cbxIncludeInPlan;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtYieldPer;
        private System.Windows.Forms.Label lblPreparationTime;
        private System.Windows.Forms.TextBox txtPreparationTime;
    }
}
