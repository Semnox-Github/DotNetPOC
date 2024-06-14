namespace Semnox.Parafait.DiscountSetup
{
    partial class DiscountsSetupUI
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
            if(disposing && (components != null))
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
            this.dgvDiscountsDTOList = new System.Windows.Forms.DataGridView();
            this.dgvDiscountsDTOListDiscountIdTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountsDTOListDiscountNameTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountsDTOListAutomaticApplyCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDiscountsDTOListDiscountPercentageTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountsDTOListDiscountAmountTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountsDTOListMinimumSaleAmountTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountsDTOListMinimumCreditsTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountsDTOListDisplayInPOSCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDiscountsDTOListIsActiveCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDiscountsDTOListSortOrderTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDiscountsDTOListVariableDiscountsCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDiscountsDTOListCouponMandatoryCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.AllowMultipleApplication = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDiscountsDTOListApplicationLimitTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountCriteriaLines = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDiscountsDTOListTransactionProfileIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvDiscountsDTOListLastUpdatedDateTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountsDTOListLastUpdatedByTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountsDTOListCouponsButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvDiscountsDTOListScheduleIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.discountsDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.tcDiscountsDTOList = new System.Windows.Forms.TabControl();
            this.transactionDiscountsTab = new System.Windows.Forms.TabPage();
            this.gamePlayDiscountsTab = new System.Windows.Forms.TabPage();
            this.loyaltyDiscountsTab = new System.Windows.Forms.TabPage();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlDiscountChildren = new System.Windows.Forms.Panel();
            this.tlpDiscountsPurchaseCriteriaAndProducts = new System.Windows.Forms.TableLayoutPanel();
            this.grpbxDiscountedProducts = new System.Windows.Forms.GroupBox();
            this.lnkSelectAll = new System.Windows.Forms.LinkLabel();
            this.lnkClearDiscountedProducts = new System.Windows.Forms.LinkLabel();
            this.dgvDiscountedProductsDTOList = new System.Windows.Forms.DataGridView();
            this.dgvDiscountedProductsDTOListIdTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvDiscountedProductsDTOListProductIdButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvDiscountedProductsDTOListCategoryIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvDiscountedProductsDTOListCategoryIdButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvDiscountedProductsDTOListProductGroupIdButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvDiscountedProductsDTOListDiscountedCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDiscountedProductsDTOListQuantityTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountedProductsDTOListDiscountPercentageTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountedProductsDTOListDiscountAmountTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountedProductsDTOListDiscountedPriceTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountedProductsDTOListIsActiveCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.discountedProductsDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.grpbxProductPurchaseCriteria = new System.Windows.Forms.GroupBox();
            this.dgvDiscountPurchaseCriteriaDTOList = new System.Windows.Forms.DataGridView();
            this.dgvDiscountPurchaseCriteriaDTOListCriteriaIdTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvDiscountPurchaseCriteriaDTOListMinQuantityTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.discountPurchaseCriteriaDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.grpDiscountedGames = new System.Windows.Forms.GroupBox();
            this.dgvDiscountedGamesDTOList = new System.Windows.Forms.DataGridView();
            this.dgvDiscountedGamesDTOListIdTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDiscountedGamesDTOListGameIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvDiscountedGamesDTOListGameIdButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvDiscountedGamesDTOListDiscountedCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDiscountedGamesDTOListIsActiveCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.discountedGamesDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.lnkClearDiscountedGameplayGames = new System.Windows.Forms.LinkLabel();
            this.btnSchedule = new System.Windows.Forms.Button();
            this.btnPublishToSite = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountsDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountsDTOListBS)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tcDiscountsDTOList.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.pnlDiscountChildren.SuspendLayout();
            this.tlpDiscountsPurchaseCriteriaAndProducts.SuspendLayout();
            this.grpbxDiscountedProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountedProductsDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountedProductsDTOListBS)).BeginInit();
            this.grpbxProductPurchaseCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountPurchaseCriteriaDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountPurchaseCriteriaDTOListBS)).BeginInit();
            this.grpDiscountedGames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountedGamesDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountedGamesDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDiscountsDTOList
            // 
            this.dgvDiscountsDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDiscountsDTOList.AutoGenerateColumns = false;
            this.dgvDiscountsDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiscountsDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvDiscountsDTOListDiscountIdTextBoxColumn,
            this.dgvDiscountsDTOListDiscountNameTextBoxColumn,
            this.dgvDiscountsDTOListAutomaticApplyCheckBoxColumn,
            this.dgvDiscountsDTOListDiscountPercentageTextBoxColumn,
            this.dgvDiscountsDTOListDiscountAmountTextBoxColumn,
            this.dgvDiscountsDTOListMinimumSaleAmountTextBoxColumn,
            this.dgvDiscountsDTOListMinimumCreditsTextBoxColumn,
            this.dgvDiscountsDTOListDisplayInPOSCheckBoxColumn,
            this.dgvDiscountsDTOListIsActiveCheckBoxColumn,
            this.dgvDiscountsDTOListSortOrderTextBoxColumn,
            this.dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn,
            this.dgvDiscountsDTOListVariableDiscountsCheckBoxColumn,
            this.dgvDiscountsDTOListCouponMandatoryCheckBoxColumn,
            this.dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn,
            this.AllowMultipleApplication,
            this.dgvDiscountsDTOListApplicationLimitTextBoxColumn,
            this.DiscountCriteriaLines,
            this.dgvDiscountsDTOListTransactionProfileIdComboBoxColumn,
            this.dgvDiscountsDTOListLastUpdatedDateTextBoxColumn,
            this.dgvDiscountsDTOListLastUpdatedByTextBoxColumn,
            this.dgvDiscountsDTOListCouponsButtonColumn,
            this.dgvDiscountsDTOListScheduleIdComboBoxColumn});
            this.dgvDiscountsDTOList.DataSource = this.discountsDTOListBS;
            this.dgvDiscountsDTOList.Location = new System.Drawing.Point(3, 3);
            this.dgvDiscountsDTOList.Name = "dgvDiscountsDTOList";
            this.dgvDiscountsDTOList.Size = new System.Drawing.Size(808, 163);
            this.dgvDiscountsDTOList.TabIndex = 0;
            this.dgvDiscountsDTOList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiscountsDTOList_CellContentClick);
            this.dgvDiscountsDTOList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiscountsDTOList_CellEnter);
            this.dgvDiscountsDTOList.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvDiscountsDTOList_CellValidating);
            this.dgvDiscountsDTOList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiscountsDTOList_CellValueChanged);
            this.dgvDiscountsDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDiscountsDTOList_DataError);
            // 
            // dgvDiscountsDTOListDiscountIdTextBoxColumn
            // 
            this.dgvDiscountsDTOListDiscountIdTextBoxColumn.DataPropertyName = "DiscountId";
            this.dgvDiscountsDTOListDiscountIdTextBoxColumn.Frozen = true;
            this.dgvDiscountsDTOListDiscountIdTextBoxColumn.HeaderText = "Discount Id";
            this.dgvDiscountsDTOListDiscountIdTextBoxColumn.Name = "dgvDiscountsDTOListDiscountIdTextBoxColumn";
            this.dgvDiscountsDTOListDiscountIdTextBoxColumn.ReadOnly = true;
            // 
            // dgvDiscountsDTOListDiscountNameTextBoxColumn
            // 
            this.dgvDiscountsDTOListDiscountNameTextBoxColumn.DataPropertyName = "DiscountName";
            this.dgvDiscountsDTOListDiscountNameTextBoxColumn.Frozen = true;
            this.dgvDiscountsDTOListDiscountNameTextBoxColumn.HeaderText = "Discount Name";
            this.dgvDiscountsDTOListDiscountNameTextBoxColumn.Name = "dgvDiscountsDTOListDiscountNameTextBoxColumn";
            // 
            // dgvDiscountsDTOListAutomaticApplyCheckBoxColumn
            // 
            this.dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.DataPropertyName = "AutomaticApply";
            this.dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.FalseValue = "N";
            this.dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.HeaderText = "Automatic Apply";
            this.dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.IndeterminateValue = "N";
            this.dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Name = "dgvDiscountsDTOListAutomaticApplyCheckBoxColumn";
            this.dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.TrueValue = "Y";
            // 
            // dgvDiscountsDTOListDiscountPercentageTextBoxColumn
            // 
            this.dgvDiscountsDTOListDiscountPercentageTextBoxColumn.DataPropertyName = "DiscountPercentage";
            this.dgvDiscountsDTOListDiscountPercentageTextBoxColumn.HeaderText = "Discount Percentage";
            this.dgvDiscountsDTOListDiscountPercentageTextBoxColumn.Name = "dgvDiscountsDTOListDiscountPercentageTextBoxColumn";
            // 
            // dgvDiscountsDTOListDiscountAmountTextBoxColumn
            // 
            this.dgvDiscountsDTOListDiscountAmountTextBoxColumn.DataPropertyName = "DiscountAmount";
            this.dgvDiscountsDTOListDiscountAmountTextBoxColumn.HeaderText = "Discount Amount";
            this.dgvDiscountsDTOListDiscountAmountTextBoxColumn.Name = "dgvDiscountsDTOListDiscountAmountTextBoxColumn";
            // 
            // dgvDiscountsDTOListMinimumSaleAmountTextBoxColumn
            // 
            this.dgvDiscountsDTOListMinimumSaleAmountTextBoxColumn.DataPropertyName = "MinimumSaleAmount";
            this.dgvDiscountsDTOListMinimumSaleAmountTextBoxColumn.HeaderText = "Minimum Sale Amount";
            this.dgvDiscountsDTOListMinimumSaleAmountTextBoxColumn.Name = "dgvDiscountsDTOListMinimumSaleAmountTextBoxColumn";
            // 
            // dgvDiscountsDTOListMinimumCreditsTextBoxColumn
            // 
            this.dgvDiscountsDTOListMinimumCreditsTextBoxColumn.DataPropertyName = "MinimumCredits";
            this.dgvDiscountsDTOListMinimumCreditsTextBoxColumn.HeaderText = "Minimum Used Credits";
            this.dgvDiscountsDTOListMinimumCreditsTextBoxColumn.Name = "dgvDiscountsDTOListMinimumCreditsTextBoxColumn";
            // 
            // dgvDiscountsDTOListDisplayInPOSCheckBoxColumn
            // 
            this.dgvDiscountsDTOListDisplayInPOSCheckBoxColumn.DataPropertyName = "DisplayInPOS";
            this.dgvDiscountsDTOListDisplayInPOSCheckBoxColumn.FalseValue = "N";
            this.dgvDiscountsDTOListDisplayInPOSCheckBoxColumn.HeaderText = "Display In POS";
            this.dgvDiscountsDTOListDisplayInPOSCheckBoxColumn.IndeterminateValue = "N";
            this.dgvDiscountsDTOListDisplayInPOSCheckBoxColumn.Name = "dgvDiscountsDTOListDisplayInPOSCheckBoxColumn";
            this.dgvDiscountsDTOListDisplayInPOSCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountsDTOListDisplayInPOSCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvDiscountsDTOListDisplayInPOSCheckBoxColumn.TrueValue = "Y";
            // 
            // dgvDiscountsDTOListIsActiveCheckBoxColumn
            // 
            this.dgvDiscountsDTOListIsActiveCheckBoxColumn.DataPropertyName = "IsActive";
            this.dgvDiscountsDTOListIsActiveCheckBoxColumn.FalseValue = "False";
            this.dgvDiscountsDTOListIsActiveCheckBoxColumn.HeaderText = "Active Flag";
            this.dgvDiscountsDTOListIsActiveCheckBoxColumn.IndeterminateValue = "False";
            this.dgvDiscountsDTOListIsActiveCheckBoxColumn.Name = "dgvDiscountsDTOListIsActiveCheckBoxColumn";
            this.dgvDiscountsDTOListIsActiveCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountsDTOListIsActiveCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvDiscountsDTOListIsActiveCheckBoxColumn.TrueValue = "True";
            // 
            // dgvDiscountsDTOListSortOrderTextBoxColumn
            // 
            this.dgvDiscountsDTOListSortOrderTextBoxColumn.DataPropertyName = "SortOrder";
            this.dgvDiscountsDTOListSortOrderTextBoxColumn.HeaderText = "Display Order";
            this.dgvDiscountsDTOListSortOrderTextBoxColumn.Name = "dgvDiscountsDTOListSortOrderTextBoxColumn";
            // 
            // dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn
            // 
            this.dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn.DataPropertyName = "ManagerApprovalRequired";
            this.dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn.FalseValue = "N";
            this.dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn.HeaderText = "Manager Approval Required";
            this.dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn.IndeterminateValue = "N";
            this.dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn.Name = "dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn";
            this.dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn.TrueValue = "Y";
            // 
            // dgvDiscountsDTOListVariableDiscountsCheckBoxColumn
            // 
            this.dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.DataPropertyName = "VariableDiscounts";
            this.dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.FalseValue = "N";
            this.dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.HeaderText = "Variable Discounts";
            this.dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.IndeterminateValue = "N";
            this.dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Name = "dgvDiscountsDTOListVariableDiscountsCheckBoxColumn";
            this.dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.TrueValue = "Y";
            // 
            // dgvDiscountsDTOListCouponMandatoryCheckBoxColumn
            // 
            this.dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.DataPropertyName = "CouponMandatory";
            this.dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.FalseValue = "N";
            this.dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.HeaderText = "Coupon Mandatory?";
            this.dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.IndeterminateValue = "N";
            this.dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.Name = "dgvDiscountsDTOListCouponMandatoryCheckBoxColumn";
            this.dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.TrueValue = "Y";
            // 
            // dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn
            // 
            this.dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn.DataPropertyName = "RemarksMandatory";
            this.dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn.FalseValue = "N";
            this.dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn.HeaderText = "Remarks Mandatory";
            this.dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn.IndeterminateValue = "N";
            this.dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn.Name = "dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn";
            this.dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn.TrueValue = "Y";
            // 
            // AllowMultipleApplication
            // 
            this.AllowMultipleApplication.DataPropertyName = "AllowMultipleApplication";
            this.AllowMultipleApplication.HeaderText = "Allow Multiple Application";
            this.AllowMultipleApplication.Name = "AllowMultipleApplication";
            // 
            // dgvDiscountsDTOListApplicationLimitTextBoxColumn
            // 
            this.dgvDiscountsDTOListApplicationLimitTextBoxColumn.DataPropertyName = "ApplicationLimit";
            this.dgvDiscountsDTOListApplicationLimitTextBoxColumn.HeaderText = "Application Limit";
            this.dgvDiscountsDTOListApplicationLimitTextBoxColumn.Name = "dgvDiscountsDTOListApplicationLimitTextBoxColumn";
            // 
            // DiscountCriteriaLines
            // 
            this.DiscountCriteriaLines.DataPropertyName = "DiscountCriteriaLines";
            this.DiscountCriteriaLines.HeaderText = "Discount Criteria Lines";
            this.DiscountCriteriaLines.Name = "DiscountCriteriaLines";
            // 
            // dgvDiscountsDTOListTransactionProfileIdComboBoxColumn
            // 
            this.dgvDiscountsDTOListTransactionProfileIdComboBoxColumn.DataPropertyName = "TransactionProfileId";
            this.dgvDiscountsDTOListTransactionProfileIdComboBoxColumn.HeaderText = "Transaction Profile";
            this.dgvDiscountsDTOListTransactionProfileIdComboBoxColumn.Name = "dgvDiscountsDTOListTransactionProfileIdComboBoxColumn";
            this.dgvDiscountsDTOListTransactionProfileIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountsDTOListTransactionProfileIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvDiscountsDTOListLastUpdatedDateTextBoxColumn
            // 
            this.dgvDiscountsDTOListLastUpdatedDateTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.dgvDiscountsDTOListLastUpdatedDateTextBoxColumn.HeaderText = "Last Updated Date";
            this.dgvDiscountsDTOListLastUpdatedDateTextBoxColumn.Name = "dgvDiscountsDTOListLastUpdatedDateTextBoxColumn";
            this.dgvDiscountsDTOListLastUpdatedDateTextBoxColumn.ReadOnly = true;
            // 
            // dgvDiscountsDTOListLastUpdatedByTextBoxColumn
            // 
            this.dgvDiscountsDTOListLastUpdatedByTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.dgvDiscountsDTOListLastUpdatedByTextBoxColumn.HeaderText = "Last Updated User";
            this.dgvDiscountsDTOListLastUpdatedByTextBoxColumn.Name = "dgvDiscountsDTOListLastUpdatedByTextBoxColumn";
            this.dgvDiscountsDTOListLastUpdatedByTextBoxColumn.ReadOnly = true;
            // 
            // dgvDiscountsDTOListCouponsButtonColumn
            // 
            this.dgvDiscountsDTOListCouponsButtonColumn.HeaderText = "Coupons";
            this.dgvDiscountsDTOListCouponsButtonColumn.Name = "dgvDiscountsDTOListCouponsButtonColumn";
            this.dgvDiscountsDTOListCouponsButtonColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountsDTOListCouponsButtonColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvDiscountsDTOListCouponsButtonColumn.Text = "...";
            // 
            // dgvDiscountsDTOListScheduleIdComboBoxColumn
            // 
            this.dgvDiscountsDTOListScheduleIdComboBoxColumn.DataPropertyName = "ScheduleId";
            this.dgvDiscountsDTOListScheduleIdComboBoxColumn.HeaderText = "Schedule";
            this.dgvDiscountsDTOListScheduleIdComboBoxColumn.Name = "dgvDiscountsDTOListScheduleIdComboBoxColumn";
            this.dgvDiscountsDTOListScheduleIdComboBoxColumn.ReadOnly = true;
            // 
            // discountsDTOListBS
            // 
            this.discountsDTOListBS.DataSource = typeof(Semnox.Parafait.Discounts.DiscountsDTO);
            this.discountsDTOListBS.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.discountsDTOListBS_AddingNew);
            this.discountsDTOListBS.CurrentChanged += new System.EventHandler(this.discountsDTOListBS_CurrentChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.chbShowActiveEntries);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(814, 47);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(456, 17);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(269, 18);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(167, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(96, 20);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name :";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(6, 20);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(124, 19);
            this.chbShowActiveEntries.TabIndex = 1;
            this.chbShowActiveEntries.Text = "Show Active Only";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // tcDiscountsDTOList
            // 
            this.tcDiscountsDTOList.Controls.Add(this.transactionDiscountsTab);
            this.tcDiscountsDTOList.Controls.Add(this.gamePlayDiscountsTab);
            this.tcDiscountsDTOList.Controls.Add(this.loyaltyDiscountsTab);
            this.tcDiscountsDTOList.Location = new System.Drawing.Point(10, 63);
            this.tcDiscountsDTOList.Name = "tcDiscountsDTOList";
            this.tcDiscountsDTOList.SelectedIndex = 0;
            this.tcDiscountsDTOList.Size = new System.Drawing.Size(565, 18);
            this.tcDiscountsDTOList.TabIndex = 2;
            this.tcDiscountsDTOList.Selected += new System.Windows.Forms.TabControlEventHandler(this.tcDiscountsDTOList_Selected);
            // 
            // transactionDiscountsTab
            // 
            this.transactionDiscountsTab.Location = new System.Drawing.Point(4, 22);
            this.transactionDiscountsTab.Name = "transactionDiscountsTab";
            this.transactionDiscountsTab.Padding = new System.Windows.Forms.Padding(3);
            this.transactionDiscountsTab.Size = new System.Drawing.Size(557, 0);
            this.transactionDiscountsTab.TabIndex = 0;
            this.transactionDiscountsTab.Text = "Transaction Discounts";
            this.transactionDiscountsTab.UseVisualStyleBackColor = true;
            // 
            // gamePlayDiscountsTab
            // 
            this.gamePlayDiscountsTab.Location = new System.Drawing.Point(4, 22);
            this.gamePlayDiscountsTab.Name = "gamePlayDiscountsTab";
            this.gamePlayDiscountsTab.Padding = new System.Windows.Forms.Padding(3);
            this.gamePlayDiscountsTab.Size = new System.Drawing.Size(557, 0);
            this.gamePlayDiscountsTab.TabIndex = 1;
            this.gamePlayDiscountsTab.Text = "Game Play Discounts (Loaded on Cards)";
            this.gamePlayDiscountsTab.UseVisualStyleBackColor = true;
            // 
            // loyaltyDiscountsTab
            // 
            this.loyaltyDiscountsTab.Location = new System.Drawing.Point(4, 22);
            this.loyaltyDiscountsTab.Name = "loyaltyDiscountsTab";
            this.loyaltyDiscountsTab.Size = new System.Drawing.Size(557, 0);
            this.loyaltyDiscountsTab.TabIndex = 2;
            this.loyaltyDiscountsTab.Text = "Loyalty Discounts(Automatic on Game Play)";
            this.loyaltyDiscountsTab.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(10, 534);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 27;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(340, 534);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(230, 534);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(120, 534);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 24;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tlpMain
            // 
            this.tlpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.Controls.Add(this.dgvDiscountsDTOList, 0, 0);
            this.tlpMain.Controls.Add(this.pnlDiscountChildren, 0, 1);
            this.tlpMain.Location = new System.Drawing.Point(10, 81);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(814, 423);
            this.tlpMain.TabIndex = 28;
            // 
            // pnlDiscountChildren
            // 
            this.pnlDiscountChildren.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDiscountChildren.Controls.Add(this.tlpDiscountsPurchaseCriteriaAndProducts);
            this.pnlDiscountChildren.Controls.Add(this.grpDiscountedGames);
            this.pnlDiscountChildren.Location = new System.Drawing.Point(3, 172);
            this.pnlDiscountChildren.Name = "pnlDiscountChildren";
            this.pnlDiscountChildren.Size = new System.Drawing.Size(808, 248);
            this.pnlDiscountChildren.TabIndex = 3;
            // 
            // tlpDiscountsPurchaseCriteriaAndProducts
            // 
            this.tlpDiscountsPurchaseCriteriaAndProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpDiscountsPurchaseCriteriaAndProducts.ColumnCount = 2;
            this.tlpDiscountsPurchaseCriteriaAndProducts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDiscountsPurchaseCriteriaAndProducts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDiscountsPurchaseCriteriaAndProducts.Controls.Add(this.grpbxDiscountedProducts, 1, 0);
            this.tlpDiscountsPurchaseCriteriaAndProducts.Controls.Add(this.grpbxProductPurchaseCriteria, 0, 0);
            this.tlpDiscountsPurchaseCriteriaAndProducts.Location = new System.Drawing.Point(-3, 0);
            this.tlpDiscountsPurchaseCriteriaAndProducts.Margin = new System.Windows.Forms.Padding(0);
            this.tlpDiscountsPurchaseCriteriaAndProducts.Name = "tlpDiscountsPurchaseCriteriaAndProducts";
            this.tlpDiscountsPurchaseCriteriaAndProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tlpDiscountsPurchaseCriteriaAndProducts.RowCount = 1;
            this.tlpDiscountsPurchaseCriteriaAndProducts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDiscountsPurchaseCriteriaAndProducts.Size = new System.Drawing.Size(811, 248);
            this.tlpDiscountsPurchaseCriteriaAndProducts.TabIndex = 2;
            // 
            // grpbxDiscountedProducts
            // 
            this.grpbxDiscountedProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpbxDiscountedProducts.Controls.Add(this.lnkSelectAll);
            this.grpbxDiscountedProducts.Controls.Add(this.lnkClearDiscountedProducts);
            this.grpbxDiscountedProducts.Controls.Add(this.dgvDiscountedProductsDTOList);
            this.grpbxDiscountedProducts.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.grpbxDiscountedProducts.Location = new System.Drawing.Point(408, 6);
            this.grpbxDiscountedProducts.Name = "grpbxDiscountedProducts";
            this.grpbxDiscountedProducts.Size = new System.Drawing.Size(397, 236);
            this.grpbxDiscountedProducts.TabIndex = 1;
            this.grpbxDiscountedProducts.TabStop = false;
            this.grpbxDiscountedProducts.Text = "Discounted Products";
            // 
            // lnkSelectAll
            // 
            this.lnkSelectAll.AutoSize = true;
            this.lnkSelectAll.Location = new System.Drawing.Point(51, 22);
            this.lnkSelectAll.Name = "lnkSelectAll";
            this.lnkSelectAll.Size = new System.Drawing.Size(79, 15);
            this.lnkSelectAll.TabIndex = 13;
            this.lnkSelectAll.TabStop = true;
            this.lnkSelectAll.Text = "De-Select All";
            this.lnkSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectAll_LinkClicked);
            // 
            // lnkClearDiscountedProducts
            // 
            this.lnkClearDiscountedProducts.AutoSize = true;
            this.lnkClearDiscountedProducts.Location = new System.Drawing.Point(9, 22);
            this.lnkClearDiscountedProducts.Name = "lnkClearDiscountedProducts";
            this.lnkClearDiscountedProducts.Size = new System.Drawing.Size(37, 15);
            this.lnkClearDiscountedProducts.TabIndex = 12;
            this.lnkClearDiscountedProducts.TabStop = true;
            this.lnkClearDiscountedProducts.Text = "Clear";
            this.lnkClearDiscountedProducts.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearDiscountedProducts_LinkClicked);
            // 
            // dgvDiscountedProductsDTOList
            // 
            this.dgvDiscountedProductsDTOList.AllowUserToDeleteRows = false;
            this.dgvDiscountedProductsDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDiscountedProductsDTOList.AutoGenerateColumns = false;
            this.dgvDiscountedProductsDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiscountedProductsDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvDiscountedProductsDTOListIdTextBoxColumn,
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn,
            this.dgvDiscountedProductsDTOListProductIdButtonColumn,
            this.dgvDiscountedProductsDTOListCategoryIdComboBoxColumn,
            this.dgvDiscountedProductsDTOListCategoryIdButtonColumn,
            this.dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn,
            this.dgvDiscountedProductsDTOListProductGroupIdButtonColumn,
            this.dgvDiscountedProductsDTOListDiscountedCheckBoxColumn,
            this.dgvDiscountedProductsDTOListQuantityTextBoxColumn,
            this.dgvDiscountedProductsDTOListDiscountPercentageTextBoxColumn,
            this.dgvDiscountedProductsDTOListDiscountAmountTextBoxColumn,
            this.dgvDiscountedProductsDTOListDiscountedPriceTextBoxColumn,
            this.dgvDiscountedProductsDTOListIsActiveCheckBoxColumn});
            this.dgvDiscountedProductsDTOList.DataSource = this.discountedProductsDTOListBS;
            this.dgvDiscountedProductsDTOList.Location = new System.Drawing.Point(6, 43);
            this.dgvDiscountedProductsDTOList.Name = "dgvDiscountedProductsDTOList";
            this.dgvDiscountedProductsDTOList.Size = new System.Drawing.Size(385, 170);
            this.dgvDiscountedProductsDTOList.TabIndex = 0;
            this.dgvDiscountedProductsDTOList.VirtualMode = true;
            this.dgvDiscountedProductsDTOList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiscountedProductsDTOList_CellClick);
            this.dgvDiscountedProductsDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDiscountedProductsDTOList_DataError);
            // 
            // dgvDiscountedProductsDTOListIdTextBoxColumn
            // 
            this.dgvDiscountedProductsDTOListIdTextBoxColumn.DataPropertyName = "Id";
            this.dgvDiscountedProductsDTOListIdTextBoxColumn.HeaderText = "Id";
            this.dgvDiscountedProductsDTOListIdTextBoxColumn.Name = "dgvDiscountedProductsDTOListIdTextBoxColumn";
            this.dgvDiscountedProductsDTOListIdTextBoxColumn.ReadOnly = true;
            // 
            // dgvDiscountedProductsDTOListProductIdComboBoxColumn
            // 
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.DataPropertyName = "ProductId";
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.HeaderText = "Product";
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.Name = "dgvDiscountedProductsDTOListProductIdComboBoxColumn";
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.ReadOnly = true;
            // 
            // dgvDiscountedProductsDTOListProductIdButtonColumn
            // 
            this.dgvDiscountedProductsDTOListProductIdButtonColumn.HeaderText = "";
            this.dgvDiscountedProductsDTOListProductIdButtonColumn.Name = "dgvDiscountedProductsDTOListProductIdButtonColumn";
            // 
            // dgvDiscountedProductsDTOListCategoryIdComboBoxColumn
            // 
            this.dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.DataPropertyName = "CategoryId";
            this.dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.HeaderText = "Category";
            this.dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.Name = "dgvDiscountedProductsDTOListCategoryIdComboBoxColumn";
            this.dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.ReadOnly = true;
            this.dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvDiscountedProductsDTOListCategoryIdButtonColumn
            // 
            this.dgvDiscountedProductsDTOListCategoryIdButtonColumn.HeaderText = "";
            this.dgvDiscountedProductsDTOListCategoryIdButtonColumn.Name = "dgvDiscountedProductsDTOListCategoryIdButtonColumn";
            // 
            // dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn
            // 
            this.dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn.DataPropertyName = "ProductGroupId";
            this.dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn.HeaderText = "Product Group";
            this.dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn.Name = "dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn";
            this.dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn.ReadOnly = true;
            // 
            // dgvDiscountedProductsDTOListProductGroupIdButtonColumn
            // 
            this.dgvDiscountedProductsDTOListProductGroupIdButtonColumn.HeaderText = "";
            this.dgvDiscountedProductsDTOListProductGroupIdButtonColumn.Name = "dgvDiscountedProductsDTOListProductGroupIdButtonColumn";
            // 
            // dgvDiscountedProductsDTOListDiscountedCheckBoxColumn
            // 
            this.dgvDiscountedProductsDTOListDiscountedCheckBoxColumn.DataPropertyName = "Discounted";
            this.dgvDiscountedProductsDTOListDiscountedCheckBoxColumn.FalseValue = "N";
            this.dgvDiscountedProductsDTOListDiscountedCheckBoxColumn.HeaderText = "Discounted";
            this.dgvDiscountedProductsDTOListDiscountedCheckBoxColumn.IndeterminateValue = "N";
            this.dgvDiscountedProductsDTOListDiscountedCheckBoxColumn.Name = "dgvDiscountedProductsDTOListDiscountedCheckBoxColumn";
            this.dgvDiscountedProductsDTOListDiscountedCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountedProductsDTOListDiscountedCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvDiscountedProductsDTOListDiscountedCheckBoxColumn.TrueValue = "Y";
            // 
            // dgvDiscountedProductsDTOListQuantityTextBoxColumn
            // 
            this.dgvDiscountedProductsDTOListQuantityTextBoxColumn.DataPropertyName = "Quantity";
            this.dgvDiscountedProductsDTOListQuantityTextBoxColumn.HeaderText = "Quantity";
            this.dgvDiscountedProductsDTOListQuantityTextBoxColumn.Name = "dgvDiscountedProductsDTOListQuantityTextBoxColumn";
            // 
            // dgvDiscountedProductsDTOListDiscountPercentageTextBoxColumn
            // 
            this.dgvDiscountedProductsDTOListDiscountPercentageTextBoxColumn.DataPropertyName = "DiscountPercentage";
            this.dgvDiscountedProductsDTOListDiscountPercentageTextBoxColumn.HeaderText = "Discount Percentage";
            this.dgvDiscountedProductsDTOListDiscountPercentageTextBoxColumn.Name = "dgvDiscountedProductsDTOListDiscountPercentageTextBoxColumn";
            // 
            // dgvDiscountedProductsDTOListDiscountAmountTextBoxColumn
            // 
            this.dgvDiscountedProductsDTOListDiscountAmountTextBoxColumn.DataPropertyName = "DiscountAmount";
            this.dgvDiscountedProductsDTOListDiscountAmountTextBoxColumn.HeaderText = "Discount Amount";
            this.dgvDiscountedProductsDTOListDiscountAmountTextBoxColumn.Name = "dgvDiscountedProductsDTOListDiscountAmountTextBoxColumn";
            // 
            // dgvDiscountedProductsDTOListDiscountedPriceTextBoxColumn
            // 
            this.dgvDiscountedProductsDTOListDiscountedPriceTextBoxColumn.DataPropertyName = "DiscountedPrice";
            this.dgvDiscountedProductsDTOListDiscountedPriceTextBoxColumn.HeaderText = "Discounted Price";
            this.dgvDiscountedProductsDTOListDiscountedPriceTextBoxColumn.Name = "dgvDiscountedProductsDTOListDiscountedPriceTextBoxColumn";
            // 
            // dgvDiscountedProductsDTOListIsActiveCheckBoxColumn
            // 
            this.dgvDiscountedProductsDTOListIsActiveCheckBoxColumn.DataPropertyName = "IsActive";
            this.dgvDiscountedProductsDTOListIsActiveCheckBoxColumn.HeaderText = "Is Active?";
            this.dgvDiscountedProductsDTOListIsActiveCheckBoxColumn.Name = "dgvDiscountedProductsDTOListIsActiveCheckBoxColumn";
            // 
            // discountedProductsDTOListBS
            // 
            this.discountedProductsDTOListBS.DataSource = typeof(Semnox.Parafait.Discounts.DiscountedProductsDTO);
            // 
            // grpbxProductPurchaseCriteria
            // 
            this.grpbxProductPurchaseCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpbxProductPurchaseCriteria.Controls.Add(this.dgvDiscountPurchaseCriteriaDTOList);
            this.grpbxProductPurchaseCriteria.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.grpbxProductPurchaseCriteria.Location = new System.Drawing.Point(6, 6);
            this.grpbxProductPurchaseCriteria.Name = "grpbxProductPurchaseCriteria";
            this.grpbxProductPurchaseCriteria.Size = new System.Drawing.Size(396, 236);
            this.grpbxProductPurchaseCriteria.TabIndex = 0;
            this.grpbxProductPurchaseCriteria.TabStop = false;
            this.grpbxProductPurchaseCriteria.Text = "Product Purchase Criteria";
            // 
            // dgvDiscountPurchaseCriteriaDTOList
            // 
            this.dgvDiscountPurchaseCriteriaDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDiscountPurchaseCriteriaDTOList.AutoGenerateColumns = false;
            this.dgvDiscountPurchaseCriteriaDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiscountPurchaseCriteriaDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvDiscountPurchaseCriteriaDTOListCriteriaIdTextBoxColumn,
            this.dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn,
            this.dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn,
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn,
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn,
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn,
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdButtonColumn,
            this.dgvDiscountPurchaseCriteriaDTOListMinQuantityTextBoxColumn,
            this.dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn});
            this.dgvDiscountPurchaseCriteriaDTOList.DataSource = this.discountPurchaseCriteriaDTOListBS;
            this.dgvDiscountPurchaseCriteriaDTOList.Location = new System.Drawing.Point(6, 43);
            this.dgvDiscountPurchaseCriteriaDTOList.Name = "dgvDiscountPurchaseCriteriaDTOList";
            this.dgvDiscountPurchaseCriteriaDTOList.Size = new System.Drawing.Size(384, 170);
            this.dgvDiscountPurchaseCriteriaDTOList.TabIndex = 0;
            this.dgvDiscountPurchaseCriteriaDTOList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiscountPurchaseCriteriaDTOList_CellClick);
            this.dgvDiscountPurchaseCriteriaDTOList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiscountPurchaseCriteriaDTOList_CellValueChanged);
            this.dgvDiscountPurchaseCriteriaDTOList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvDiscountPurchaseCriteriaDTOList_DataBindingComplete);
            this.dgvDiscountPurchaseCriteriaDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDiscountPurchaseCriteriaDTOList_DataError);
            // 
            // dgvDiscountPurchaseCriteriaDTOListCriteriaIdTextBoxColumn
            // 
            this.dgvDiscountPurchaseCriteriaDTOListCriteriaIdTextBoxColumn.DataPropertyName = "CriteriaId";
            this.dgvDiscountPurchaseCriteriaDTOListCriteriaIdTextBoxColumn.HeaderText = "Criteria Id";
            this.dgvDiscountPurchaseCriteriaDTOListCriteriaIdTextBoxColumn.Name = "dgvDiscountPurchaseCriteriaDTOListCriteriaIdTextBoxColumn";
            this.dgvDiscountPurchaseCriteriaDTOListCriteriaIdTextBoxColumn.ReadOnly = true;
            // 
            // dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn
            // 
            this.dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.DataPropertyName = "ProductId";
            this.dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.HeaderText = "Product";
            this.dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.Name = "dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn";
            this.dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.ReadOnly = true;
            this.dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn
            // 
            this.dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn.HeaderText = "";
            this.dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn.Name = "dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn";
            // 
            // dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn
            // 
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.DataPropertyName = "CategoryId";
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.HeaderText = "Category";
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.Name = "dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn";
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.ReadOnly = true;
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn
            // 
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn.HeaderText = "";
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn.Name = "dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn";
            // 
            // dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn
            // 
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn.DataPropertyName = "ProductGroupId";
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn.HeaderText = "Product Group";
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn.Name = "dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn";
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvDiscountPurchaseCriteriaDTOListProductGroupIdButtonColumn
            // 
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdButtonColumn.HeaderText = "";
            this.dgvDiscountPurchaseCriteriaDTOListProductGroupIdButtonColumn.Name = "dgvDiscountPurchaseCriteriaDTOListProductGroupIdButtonColumn";
            // 
            // dgvDiscountPurchaseCriteriaDTOListMinQuantityTextBoxColumn
            // 
            this.dgvDiscountPurchaseCriteriaDTOListMinQuantityTextBoxColumn.DataPropertyName = "MinQuantity";
            this.dgvDiscountPurchaseCriteriaDTOListMinQuantityTextBoxColumn.HeaderText = "Min. Quantity";
            this.dgvDiscountPurchaseCriteriaDTOListMinQuantityTextBoxColumn.Name = "dgvDiscountPurchaseCriteriaDTOListMinQuantityTextBoxColumn";
            // 
            // dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn
            // 
            this.dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn.DataPropertyName = "IsActive";
            this.dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn.FalseValue = "False";
            this.dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn.HeaderText = "Is Active?";
            this.dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn.IndeterminateValue = "False";
            this.dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn.Name = "dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn";
            this.dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn.TrueValue = "True";
            // 
            // discountPurchaseCriteriaDTOListBS
            // 
            this.discountPurchaseCriteriaDTOListBS.DataSource = typeof(Semnox.Parafait.Discounts.DiscountPurchaseCriteriaDTO);
            // 
            // grpDiscountedGames
            // 
            this.grpDiscountedGames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDiscountedGames.Controls.Add(this.dgvDiscountedGamesDTOList);
            this.grpDiscountedGames.Controls.Add(this.lnkClearDiscountedGameplayGames);
            this.grpDiscountedGames.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.grpDiscountedGames.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grpDiscountedGames.Location = new System.Drawing.Point(5, 9);
            this.grpDiscountedGames.Name = "grpDiscountedGames";
            this.grpDiscountedGames.Size = new System.Drawing.Size(799, 240);
            this.grpDiscountedGames.TabIndex = 0;
            this.grpDiscountedGames.TabStop = false;
            this.grpDiscountedGames.Text = "Discounted Games";
            // 
            // dgvDiscountedGamesDTOList
            // 
            this.dgvDiscountedGamesDTOList.AllowUserToAddRows = false;
            this.dgvDiscountedGamesDTOList.AllowUserToDeleteRows = false;
            this.dgvDiscountedGamesDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDiscountedGamesDTOList.AutoGenerateColumns = false;
            this.dgvDiscountedGamesDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiscountedGamesDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvDiscountedGamesDTOListIdTextBoxColumn,
            this.dgvDiscountedGamesDTOListGameIdComboBoxColumn,
            this.dgvDiscountedGamesDTOListGameIdButtonColumn,
            this.dgvDiscountedGamesDTOListDiscountedCheckBoxColumn,
            this.dgvDiscountedGamesDTOListIsActiveCheckBoxColumn});
            this.dgvDiscountedGamesDTOList.DataSource = this.discountedGamesDTOListBS;
            this.dgvDiscountedGamesDTOList.Location = new System.Drawing.Point(6, 34);
            this.dgvDiscountedGamesDTOList.Name = "dgvDiscountedGamesDTOList";
            this.dgvDiscountedGamesDTOList.Size = new System.Drawing.Size(787, 200);
            this.dgvDiscountedGamesDTOList.TabIndex = 10;
            this.dgvDiscountedGamesDTOList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiscountedGamesDTOList_CellClick);
            this.dgvDiscountedGamesDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDiscountedGamesDTOList_DataError);
            // 
            // dgvDiscountedGamesDTOListIdTextBoxColumn
            // 
            this.dgvDiscountedGamesDTOListIdTextBoxColumn.DataPropertyName = "Id";
            this.dgvDiscountedGamesDTOListIdTextBoxColumn.HeaderText = "Id";
            this.dgvDiscountedGamesDTOListIdTextBoxColumn.Name = "dgvDiscountedGamesDTOListIdTextBoxColumn";
            this.dgvDiscountedGamesDTOListIdTextBoxColumn.ReadOnly = true;
            // 
            // dgvDiscountedGamesDTOListGameIdComboBoxColumn
            // 
            this.dgvDiscountedGamesDTOListGameIdComboBoxColumn.DataPropertyName = "GameId";
            this.dgvDiscountedGamesDTOListGameIdComboBoxColumn.HeaderText = "Game";
            this.dgvDiscountedGamesDTOListGameIdComboBoxColumn.Name = "dgvDiscountedGamesDTOListGameIdComboBoxColumn";
            this.dgvDiscountedGamesDTOListGameIdComboBoxColumn.ReadOnly = true;
            // 
            // dgvDiscountedGamesDTOListGameIdButtonColumn
            // 
            this.dgvDiscountedGamesDTOListGameIdButtonColumn.HeaderText = "";
            this.dgvDiscountedGamesDTOListGameIdButtonColumn.Name = "dgvDiscountedGamesDTOListGameIdButtonColumn";
            this.dgvDiscountedGamesDTOListGameIdButtonColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountedGamesDTOListGameIdButtonColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvDiscountedGamesDTOListDiscountedCheckBoxColumn
            // 
            this.dgvDiscountedGamesDTOListDiscountedCheckBoxColumn.DataPropertyName = "Discounted";
            this.dgvDiscountedGamesDTOListDiscountedCheckBoxColumn.FalseValue = "N";
            this.dgvDiscountedGamesDTOListDiscountedCheckBoxColumn.HeaderText = "Discounted";
            this.dgvDiscountedGamesDTOListDiscountedCheckBoxColumn.IndeterminateValue = "N";
            this.dgvDiscountedGamesDTOListDiscountedCheckBoxColumn.Name = "dgvDiscountedGamesDTOListDiscountedCheckBoxColumn";
            this.dgvDiscountedGamesDTOListDiscountedCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountedGamesDTOListDiscountedCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvDiscountedGamesDTOListDiscountedCheckBoxColumn.TrueValue = "Y";
            // 
            // dgvDiscountedGamesDTOListIsActiveCheckBoxColumn
            // 
            this.dgvDiscountedGamesDTOListIsActiveCheckBoxColumn.DataPropertyName = "IsActive";
            this.dgvDiscountedGamesDTOListIsActiveCheckBoxColumn.HeaderText = "Is Active?";
            this.dgvDiscountedGamesDTOListIsActiveCheckBoxColumn.Name = "dgvDiscountedGamesDTOListIsActiveCheckBoxColumn";
            // 
            // discountedGamesDTOListBS
            // 
            this.discountedGamesDTOListBS.DataSource = typeof(Semnox.Parafait.Discounts.DiscountedGamesDTO);
            // 
            // lnkClearDiscountedGameplayGames
            // 
            this.lnkClearDiscountedGameplayGames.AutoSize = true;
            this.lnkClearDiscountedGameplayGames.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lnkClearDiscountedGameplayGames.Location = new System.Drawing.Point(5, 16);
            this.lnkClearDiscountedGameplayGames.Name = "lnkClearDiscountedGameplayGames";
            this.lnkClearDiscountedGameplayGames.Size = new System.Drawing.Size(37, 15);
            this.lnkClearDiscountedGameplayGames.TabIndex = 9;
            this.lnkClearDiscountedGameplayGames.TabStop = true;
            this.lnkClearDiscountedGameplayGames.Text = "Clear";
            this.lnkClearDiscountedGameplayGames.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearDiscountedGameplayGames_LinkClicked);
            // 
            // btnSchedule
            // 
            this.btnSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSchedule.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSchedule.Location = new System.Drawing.Point(450, 534);
            this.btnSchedule.Name = "btnSchedule";
            this.btnSchedule.Size = new System.Drawing.Size(75, 23);
            this.btnSchedule.TabIndex = 29;
            this.btnSchedule.Text = "Schedule";
            this.btnSchedule.UseVisualStyleBackColor = true;
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);
            // 
            // btnPublishToSite
            // 
            this.btnPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPublishToSite.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPublishToSite.Location = new System.Drawing.Point(560, 534);
            this.btnPublishToSite.Name = "btnPublishToSite";
            this.btnPublishToSite.Size = new System.Drawing.Size(109, 23);
            this.btnPublishToSite.TabIndex = 55;
            this.btnPublishToSite.Text = "Publish To Sites";
            this.btnPublishToSite.UseVisualStyleBackColor = true;
            this.btnPublishToSite.Click += new System.EventHandler(this.btnPublishToSite_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(10, 516);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(133, 15);
            this.lblStatus.TabIndex = 56;
            this.lblStatus.Text = "Loading.. Please wait..";
            // 
            // DiscountsSetupUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 570);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnPublishToSite);
            this.Controls.Add(this.btnSchedule);
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.tcDiscountsDTOList);
            this.Controls.Add(this.groupBox1);
            this.Name = "DiscountsSetupUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Discounts Setup";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DiscountsSetupUI_FormClosed);
            this.Load += new System.EventHandler(this.DiscountsSetupUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountsDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountsDTOListBS)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tcDiscountsDTOList.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.pnlDiscountChildren.ResumeLayout(false);
            this.tlpDiscountsPurchaseCriteriaAndProducts.ResumeLayout(false);
            this.grpbxDiscountedProducts.ResumeLayout(false);
            this.grpbxDiscountedProducts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountedProductsDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountedProductsDTOListBS)).EndInit();
            this.grpbxProductPurchaseCriteria.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountPurchaseCriteriaDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountPurchaseCriteriaDTOListBS)).EndInit();
            this.grpDiscountedGames.ResumeLayout(false);
            this.grpDiscountedGames.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountedGamesDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountedGamesDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDiscountsDTOList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.BindingSource discountsDTOListBS;
        private System.Windows.Forms.TabControl tcDiscountsDTOList;
        private System.Windows.Forms.TabPage transactionDiscountsTab;
        private System.Windows.Forms.TabPage gamePlayDiscountsTab;
        private System.Windows.Forms.TabPage loyaltyDiscountsTab;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.BindingSource discountPurchaseCriteriaDTOListBS;
        private System.Windows.Forms.BindingSource discountedProductsDTOListBS;
        private System.Windows.Forms.BindingSource discountedGamesDTOListBS;
        private System.Windows.Forms.TableLayoutPanel tlpDiscountsPurchaseCriteriaAndProducts;
        private System.Windows.Forms.GroupBox grpbxDiscountedProducts;
        private System.Windows.Forms.LinkLabel lnkSelectAll;
        private System.Windows.Forms.LinkLabel lnkClearDiscountedProducts;
        private System.Windows.Forms.DataGridView dgvDiscountedProductsDTOList;
        private System.Windows.Forms.GroupBox grpbxProductPurchaseCriteria;
        private System.Windows.Forms.DataGridView dgvDiscountPurchaseCriteriaDTOList;
        private System.Windows.Forms.GroupBox grpDiscountedGames;
        private System.Windows.Forms.DataGridView dgvDiscountedGamesDTOList;
        private System.Windows.Forms.LinkLabel lnkClearDiscountedGameplayGames;
        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.Button btnPublishToSite;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountsDTOListDiscountIdTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountsDTOListDiscountNameTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDiscountsDTOListAutomaticApplyCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountsDTOListDiscountPercentageTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountsDTOListDiscountAmountTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountsDTOListMinimumSaleAmountTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountsDTOListMinimumCreditsTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDiscountsDTOListDisplayInPOSCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDiscountsDTOListIsActiveCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountsDTOListSortOrderTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDiscountsDTOListVariableDiscountsCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDiscountsDTOListCouponMandatoryCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AllowMultipleApplication;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountsDTOListApplicationLimitTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DiscountCriteriaLines;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountsDTOListTransactionProfileIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountsDTOListLastUpdatedDateTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountsDTOListLastUpdatedByTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDiscountsDTOListCouponsButtonColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountsDTOListScheduleIdComboBoxColumn;
        private System.Windows.Forms.Panel pnlDiscountChildren;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountPurchaseCriteriaDTOListCriteriaIdTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDiscountPurchaseCriteriaDTOListProductGroupIdButtonColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountPurchaseCriteriaDTOListMinQuantityTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountedGamesDTOListIdTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountedGamesDTOListGameIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDiscountedGamesDTOListGameIdButtonColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDiscountedGamesDTOListDiscountedCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDiscountedGamesDTOListIsActiveCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountedProductsDTOListIdTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountedProductsDTOListProductIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDiscountedProductsDTOListProductIdButtonColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountedProductsDTOListCategoryIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDiscountedProductsDTOListCategoryIdButtonColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDiscountedProductsDTOListProductGroupIdButtonColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDiscountedProductsDTOListDiscountedCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountedProductsDTOListQuantityTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountedProductsDTOListDiscountPercentageTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountedProductsDTOListDiscountAmountTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDiscountedProductsDTOListDiscountedPriceTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvDiscountedProductsDTOListIsActiveCheckBoxColumn;
    }
}