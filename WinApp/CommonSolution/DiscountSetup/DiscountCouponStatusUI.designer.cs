namespace Semnox.Parafait.DiscountSetup
{
    partial class DiscountCouponStatusUI
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
            this.txtCouponNumber = new System.Windows.Forms.TextBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAlphaKeypad = new System.Windows.Forms.Button();
            this.lblCouponStatus = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvDiscountsDTOList = new System.Windows.Forms.DataGridView();
            this.discountNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discountPercentageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discountAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minimumSaleAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discountnsDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvDiscountsPurchaseCriteriaDTOList = new System.Windows.Forms.DataGridView();
            this.dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.minQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discountPurchaseCriteriaDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgvDiscountedProductsDTOList = new System.Windows.Forms.DataGridView();
            this.dgvDiscountedProductsDTOListCategoryIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discountPercentageDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discountAmountDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discountedProductsDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblEffectiveDate = new System.Windows.Forms.Label();
            this.lblExpiryDate = new System.Windows.Forms.Label();
            this.discountedPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountsDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountnsDTOListBS)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountsPurchaseCriteriaDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountPurchaseCriteriaDTOListBS)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountedProductsDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountedProductsDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCouponNumber
            // 
            this.txtCouponNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCouponNumber.Location = new System.Drawing.Point(118, 28);
            this.txtCouponNumber.Name = "txtCouponNumber";
            this.txtCouponNumber.Size = new System.Drawing.Size(205, 20);
            this.txtCouponNumber.TabIndex = 3;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblFrom.Location = new System.Drawing.Point(12, 31);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(107, 15);
            this.lblFrom.TabIndex = 2;
            this.lblFrom.Text = "Coupon Number : ";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundImage = global::Semnox.Parafait.DiscountSetup.Properties.Resources.normal2;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(356, 13);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(155, 55);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(19, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Coupon Status : ";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Semnox.Parafait.DiscountSetup.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(379, 399);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(155, 55);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAlphaKeypad
            // 
            this.btnAlphaKeypad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAlphaKeypad.BackColor = System.Drawing.Color.Transparent;
            this.btnAlphaKeypad.BackgroundImage = global::Semnox.Parafait.DiscountSetup.Properties.Resources.keyboard;
            this.btnAlphaKeypad.CausesValidation = false;
            this.btnAlphaKeypad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAlphaKeypad.FlatAppearance.BorderSize = 0;
            this.btnAlphaKeypad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAlphaKeypad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAlphaKeypad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlphaKeypad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlphaKeypad.ForeColor = System.Drawing.Color.Black;
            this.btnAlphaKeypad.Location = new System.Drawing.Point(946, 421);
            this.btnAlphaKeypad.Name = "btnAlphaKeypad";
            this.btnAlphaKeypad.Size = new System.Drawing.Size(36, 33);
            this.btnAlphaKeypad.TabIndex = 27;
            this.btnAlphaKeypad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAlphaKeypad.UseVisualStyleBackColor = false;
            this.btnAlphaKeypad.Click += new System.EventHandler(this.btnAlphaKeypad_Click);
            // 
            // lblCouponStatus
            // 
            this.lblCouponStatus.AutoSize = true;
            this.lblCouponStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCouponStatus.Location = new System.Drawing.Point(116, 90);
            this.lblCouponStatus.Name = "lblCouponStatus";
            this.lblCouponStatus.Size = new System.Drawing.Size(0, 15);
            this.lblCouponStatus.TabIndex = 28;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgvDiscountsDTOList);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(12, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(970, 130);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Discount";
            // 
            // dgvDiscountsDTOList
            // 
            this.dgvDiscountsDTOList.AllowUserToAddRows = false;
            this.dgvDiscountsDTOList.AllowUserToDeleteRows = false;
            this.dgvDiscountsDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDiscountsDTOList.AutoGenerateColumns = false;
            this.dgvDiscountsDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiscountsDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.discountNameDataGridViewTextBoxColumn,
            this.discountPercentageDataGridViewTextBoxColumn,
            this.discountAmountDataGridViewTextBoxColumn,
            this.minimumSaleAmountDataGridViewTextBoxColumn});
            this.dgvDiscountsDTOList.DataSource = this.discountnsDTOListBS;
            this.dgvDiscountsDTOList.Location = new System.Drawing.Point(9, 19);
            this.dgvDiscountsDTOList.Name = "dgvDiscountsDTOList";
            this.dgvDiscountsDTOList.ReadOnly = true;
            this.dgvDiscountsDTOList.Size = new System.Drawing.Size(953, 94);
            this.dgvDiscountsDTOList.TabIndex = 0;
            // 
            // discountNameDataGridViewTextBoxColumn
            // 
            this.discountNameDataGridViewTextBoxColumn.DataPropertyName = "DiscountName";
            this.discountNameDataGridViewTextBoxColumn.HeaderText = "Discount Name";
            this.discountNameDataGridViewTextBoxColumn.Name = "discountNameDataGridViewTextBoxColumn";
            this.discountNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // discountPercentageDataGridViewTextBoxColumn
            // 
            this.discountPercentageDataGridViewTextBoxColumn.DataPropertyName = "DiscountPercentage";
            this.discountPercentageDataGridViewTextBoxColumn.HeaderText = "Discount Percentage";
            this.discountPercentageDataGridViewTextBoxColumn.Name = "discountPercentageDataGridViewTextBoxColumn";
            this.discountPercentageDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // discountAmountDataGridViewTextBoxColumn
            // 
            this.discountAmountDataGridViewTextBoxColumn.DataPropertyName = "DiscountAmount";
            this.discountAmountDataGridViewTextBoxColumn.HeaderText = "Discount Amount";
            this.discountAmountDataGridViewTextBoxColumn.Name = "discountAmountDataGridViewTextBoxColumn";
            this.discountAmountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // minimumSaleAmountDataGridViewTextBoxColumn
            // 
            this.minimumSaleAmountDataGridViewTextBoxColumn.DataPropertyName = "MinimumSaleAmount";
            this.minimumSaleAmountDataGridViewTextBoxColumn.HeaderText = "Minimum Sale Amount";
            this.minimumSaleAmountDataGridViewTextBoxColumn.Name = "minimumSaleAmountDataGridViewTextBoxColumn";
            this.minimumSaleAmountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // discountnsDTOListBS
            // 
            this.discountnsDTOListBS.DataSource = typeof(Semnox.Parafait.Discounts.DiscountsDTO);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.dgvDiscountsPurchaseCriteriaDTOList);
            this.groupBox3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(12, 251);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(379, 136);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Product Purchase Criteria";
            // 
            // dgvDiscountsPurchaseCriteriaDTOList
            // 
            this.dgvDiscountsPurchaseCriteriaDTOList.AllowUserToAddRows = false;
            this.dgvDiscountsPurchaseCriteriaDTOList.AllowUserToDeleteRows = false;
            this.dgvDiscountsPurchaseCriteriaDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDiscountsPurchaseCriteriaDTOList.AutoGenerateColumns = false;
            this.dgvDiscountsPurchaseCriteriaDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiscountsPurchaseCriteriaDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn,
            this.dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn,
            this.minQuantityDataGridViewTextBoxColumn});
            this.dgvDiscountsPurchaseCriteriaDTOList.DataSource = this.discountPurchaseCriteriaDTOListBS;
            this.dgvDiscountsPurchaseCriteriaDTOList.Location = new System.Drawing.Point(6, 19);
            this.dgvDiscountsPurchaseCriteriaDTOList.Name = "dgvDiscountsPurchaseCriteriaDTOList";
            this.dgvDiscountsPurchaseCriteriaDTOList.ReadOnly = true;
            this.dgvDiscountsPurchaseCriteriaDTOList.Size = new System.Drawing.Size(367, 111);
            this.dgvDiscountsPurchaseCriteriaDTOList.TabIndex = 0;
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
            // minQuantityDataGridViewTextBoxColumn
            // 
            this.minQuantityDataGridViewTextBoxColumn.DataPropertyName = "MinQuantity";
            this.minQuantityDataGridViewTextBoxColumn.HeaderText = "Min. Quantity";
            this.minQuantityDataGridViewTextBoxColumn.Name = "minQuantityDataGridViewTextBoxColumn";
            this.minQuantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // discountPurchaseCriteriaDTOListBS
            // 
            this.discountPurchaseCriteriaDTOListBS.DataSource = typeof(Semnox.Parafait.Discounts.DiscountPurchaseCriteriaDTO);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.dgvDiscountedProductsDTOList);
            this.groupBox4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox4.Location = new System.Drawing.Point(397, 251);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(585, 136);
            this.groupBox4.TabIndex = 32;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Discounted Products";
            // 
            // dgvDiscountedProductsDTOList
            // 
            this.dgvDiscountedProductsDTOList.AllowUserToAddRows = false;
            this.dgvDiscountedProductsDTOList.AllowUserToDeleteRows = false;
            this.dgvDiscountedProductsDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDiscountedProductsDTOList.AutoGenerateColumns = false;
            this.dgvDiscountedProductsDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiscountedProductsDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvDiscountedProductsDTOListCategoryIdComboBoxColumn,
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.discountPercentageDataGridViewTextBoxColumn1,
            this.discountAmountDataGridViewTextBoxColumn1,
            this.discountedPriceDataGridViewTextBoxColumn});
            this.dgvDiscountedProductsDTOList.DataSource = this.discountedProductsDTOListBS;
            this.dgvDiscountedProductsDTOList.Location = new System.Drawing.Point(20, 19);
            this.dgvDiscountedProductsDTOList.Name = "dgvDiscountedProductsDTOList";
            this.dgvDiscountedProductsDTOList.ReadOnly = true;
            this.dgvDiscountedProductsDTOList.Size = new System.Drawing.Size(557, 111);
            this.dgvDiscountedProductsDTOList.TabIndex = 0;
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
            // dgvDiscountedProductsDTOListProductIdComboBoxColumn
            // 
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.DataPropertyName = "ProductId";
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.HeaderText = "Product";
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.Name = "dgvDiscountedProductsDTOListProductIdComboBoxColumn";
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.ReadOnly = true;
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDiscountedProductsDTOListProductIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // discountPercentageDataGridViewTextBoxColumn1
            // 
            this.discountPercentageDataGridViewTextBoxColumn1.DataPropertyName = "DiscountPercentage";
            this.discountPercentageDataGridViewTextBoxColumn1.HeaderText = "Discount Percentage";
            this.discountPercentageDataGridViewTextBoxColumn1.Name = "discountPercentageDataGridViewTextBoxColumn1";
            this.discountPercentageDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // discountAmountDataGridViewTextBoxColumn1
            // 
            this.discountAmountDataGridViewTextBoxColumn1.DataPropertyName = "DiscountAmount";
            this.discountAmountDataGridViewTextBoxColumn1.HeaderText = "Discount Amount";
            this.discountAmountDataGridViewTextBoxColumn1.Name = "discountAmountDataGridViewTextBoxColumn1";
            this.discountAmountDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // discountedProductsDTOListBS
            // 
            this.discountedProductsDTOListBS.DataSource = typeof(Semnox.Parafait.Discounts.DiscountedProductsDTO);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(331, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 15);
            this.label2.TabIndex = 33;
            this.label2.Text = "Effective Date :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(646, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Expiry Date : ";
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.AutoSize = true;
            this.lblEffectiveDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblEffectiveDate.Location = new System.Drawing.Point(423, 90);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.Size = new System.Drawing.Size(0, 15);
            this.lblEffectiveDate.TabIndex = 34;
            // 
            // lblExpiryDate
            // 
            this.lblExpiryDate.AutoSize = true;
            this.lblExpiryDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblExpiryDate.Location = new System.Drawing.Point(727, 90);
            this.lblExpiryDate.Name = "lblExpiryDate";
            this.lblExpiryDate.Size = new System.Drawing.Size(0, 15);
            this.lblExpiryDate.TabIndex = 35;
            // 
            // discountedPriceDataGridViewTextBoxColumn
            // 
            this.discountedPriceDataGridViewTextBoxColumn.DataPropertyName = "DiscountedPrice";
            this.discountedPriceDataGridViewTextBoxColumn.HeaderText = "Discounted Price";
            this.discountedPriceDataGridViewTextBoxColumn.Name = "discountedPriceDataGridViewTextBoxColumn";
            this.discountedPriceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // DiscountCouponStatusUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 466);
            this.Controls.Add(this.lblExpiryDate);
            this.Controls.Add(this.lblEffectiveDate);
            this.Controls.Add(this.lblCouponStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnAlphaKeypad);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtCouponNumber);
            this.Controls.Add(this.lblFrom);
            this.Name = "DiscountCouponStatusUI";
            this.Text = "Coupon Status";
            this.Load += new System.EventHandler(this.DiscountCouponStatusUI_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountsDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountnsDTOListBS)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountsPurchaseCriteriaDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountPurchaseCriteriaDTOListBS)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountedProductsDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountedProductsDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCouponNumber;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAlphaKeypad;
        private System.Windows.Forms.Label lblCouponStatus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvDiscountsDTOList;
        private System.Windows.Forms.BindingSource discountnsDTOListBS;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dgvDiscountsPurchaseCriteriaDTOList;
        private System.Windows.Forms.DataGridView dgvDiscountedProductsDTOList;
        private System.Windows.Forms.BindingSource discountPurchaseCriteriaDTOListBS;
        private System.Windows.Forms.BindingSource discountedProductsDTOListBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn discountNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn discountPercentageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn discountAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minimumSaleAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblEffectiveDate;
        private System.Windows.Forms.Label lblExpiryDate;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountedProductsDTOListCategoryIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvDiscountedProductsDTOListProductIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn discountPercentageDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn discountAmountDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn discountedPriceDataGridViewTextBoxColumn;
    }
}