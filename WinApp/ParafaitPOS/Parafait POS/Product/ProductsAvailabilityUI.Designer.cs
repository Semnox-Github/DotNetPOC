using Semnox.Core.GenericUtilities;
namespace Parafait_POS
{
    partial class ProductsAvailabilityUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductsAvailabilityUI));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.avlProductsVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.dgvAvailableProducts = new System.Windows.Forms.DataGridView();
            this.cbAutoCompleteAvailableProducts = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.unavlProductsVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.dgvUnavailableProducts = new System.Windows.Forms.DataGridView();
            this.cbAutoCompleteUnavailableProducts = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.cbProductDisplayGroup = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMakeAvailable = new System.Windows.Forms.Button();
            this.btnMakeUnavailable = new System.Windows.Forms.Button();
            this.unavailableProductsDTOBS = new System.Windows.Forms.BindingSource(this.components);
            this.availableProductsDTOBS = new System.Windows.Forms.BindingSource(this.components);
            this.selAvl = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isAvailableDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.availableQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.initialAvailableQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unavailableTillDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approvedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.guidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isChangedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.selUnavl = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btndgvUpdate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.productNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.availableQtyDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unavailableTillDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isAvailableDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.initialAvailableQtyDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approvedByDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.createdByDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.guidDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isChangedDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableProducts)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnavailableProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unavailableProductsDTOBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.availableProductsDTOBS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.avlProductsVScrollBar);
            this.groupBox1.Controls.Add(this.cbAutoCompleteAvailableProducts);
            this.groupBox1.Controls.Add(this.dgvAvailableProducts);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(4, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(520, 418);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available Products";
            // 
            // avlProductsVScrollBar
            // 
            this.avlProductsVScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.avlProductsVScrollBar.AutoHide = false;
            this.avlProductsVScrollBar.AutoScroll = true;
            this.avlProductsVScrollBar.DataGridView = this.dgvAvailableProducts;
            this.avlProductsVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("avlProductsVScrollBar.DownButtonBackgroundImage")));
            this.avlProductsVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("avlProductsVScrollBar.DownButtonDisabledBackgroundImage")));
            this.avlProductsVScrollBar.Location = new System.Drawing.Point(475, 56);
            this.avlProductsVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.avlProductsVScrollBar.Name = "avlProductsVScrollBar";
            this.avlProductsVScrollBar.ScrollableControl = null;
            this.avlProductsVScrollBar.ScrollViewer = null;
            this.avlProductsVScrollBar.Size = new System.Drawing.Size(40, 356);
            this.avlProductsVScrollBar.TabIndex = 22;
            this.avlProductsVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("avlProductsVScrollBar.UpButtonBackgroundImage")));
            this.avlProductsVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("avlProductsVScrollBar.UpButtonDisabledBackgroundImage")));
            // 
            // dgvAvailableProducts
            // 
            this.dgvAvailableProducts.AllowUserToAddRows = false;
            this.dgvAvailableProducts.AllowUserToDeleteRows = false;
            this.dgvAvailableProducts.AllowUserToResizeColumns = false;
            this.dgvAvailableProducts.AllowUserToResizeRows = false;
            this.dgvAvailableProducts.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAvailableProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAvailableProducts.ColumnHeadersHeight = 32;
            this.dgvAvailableProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAvailableProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selAvl,
            this.ProductName,
            this.idDataGridViewTextBoxColumn,
            this.productIdDataGridViewTextBoxColumn,
            this.isAvailableDataGridViewCheckBoxColumn,
            this.availableQtyDataGridViewTextBoxColumn,
            this.initialAvailableQtyDataGridViewTextBoxColumn,
            this.unavailableTillDataGridViewTextBoxColumn,
            this.approvedByDataGridViewTextBoxColumn,
            this.commentsDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn,
            this.siteIdDataGridViewTextBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn,
            this.synchStatusDataGridViewCheckBoxColumn,
            this.guidDataGridViewTextBoxColumn,
            this.isChangedDataGridViewCheckBoxColumn});
            this.dgvAvailableProducts.DataSource = this.availableProductsDTOBS;
            this.dgvAvailableProducts.Location = new System.Drawing.Point(8, 56);
            this.dgvAvailableProducts.Name = "dgvAvailableProducts";
            this.dgvAvailableProducts.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(2);
            this.dgvAvailableProducts.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvAvailableProducts.RowTemplate.Height = 32;
            this.dgvAvailableProducts.RowTemplate.ReadOnly = true;
            this.dgvAvailableProducts.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvAvailableProducts.Size = new System.Drawing.Size(464, 356);
            this.dgvAvailableProducts.TabIndex = 1;
            this.dgvAvailableProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAvailableProducts_CellClick);
            // 
            // cbAutoCompleteAvailableProducts
            // 
            this.cbAutoCompleteAvailableProducts.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbAutoCompleteAvailableProducts.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbAutoCompleteAvailableProducts.FormattingEnabled = true;
            this.cbAutoCompleteAvailableProducts.Location = new System.Drawing.Point(9, 20);
            this.cbAutoCompleteAvailableProducts.Name = "cbAutoCompleteAvailableProducts";
            this.cbAutoCompleteAvailableProducts.Size = new System.Drawing.Size(463, 26);
            this.cbAutoCompleteAvailableProducts.TabIndex = 0;
            this.cbAutoCompleteAvailableProducts.SelectedIndexChanged += new System.EventHandler(this.cbAutoCompleteAvailableProducts_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.unavlProductsVScrollBar);
            this.groupBox2.Controls.Add(this.cbAutoCompleteUnavailableProducts);
            this.groupBox2.Controls.Add(this.dgvUnavailableProducts);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(591, 49);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(530, 418);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Unavailable Products";
            // 
            // unavlProductsVScrollBar
            // 
            this.unavlProductsVScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unavlProductsVScrollBar.AutoHide = false;
            this.unavlProductsVScrollBar.AutoScroll = true;
            this.unavlProductsVScrollBar.DataGridView = this.dgvUnavailableProducts;
            this.unavlProductsVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("unavlProductsVScrollBar.DownButtonBackgroundImage")));
            this.unavlProductsVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("unavlProductsVScrollBar.DownButtonDisabledBackgroundImage")));
            this.unavlProductsVScrollBar.Location = new System.Drawing.Point(482, 56);
            this.unavlProductsVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.unavlProductsVScrollBar.Name = "unavlProductsVScrollBar";
            this.unavlProductsVScrollBar.ScrollableControl = null;
            this.unavlProductsVScrollBar.ScrollViewer = null;
            this.unavlProductsVScrollBar.Size = new System.Drawing.Size(40, 356);
            this.unavlProductsVScrollBar.TabIndex = 3;
            this.unavlProductsVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("unavlProductsVScrollBar.UpButtonBackgroundImage")));
            this.unavlProductsVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("unavlProductsVScrollBar.UpButtonDisabledBackgroundImage")));
            // 
            // dgvUnavailableProducts
            // 
            this.dgvUnavailableProducts.AutoGenerateColumns = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUnavailableProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvUnavailableProducts.ColumnHeadersHeight = 32;
            this.dgvUnavailableProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvUnavailableProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selUnavl,
            this.btndgvUpdate,
            this.productNameDataGridViewTextBoxColumn1,
            this.availableQtyDataGridViewTextBoxColumn1,
            this.unavailableTillDataGridViewTextBoxColumn1,
            this.idDataGridViewTextBoxColumn1,
            this.productIdDataGridViewTextBoxColumn1,
            this.isAvailableDataGridViewCheckBoxColumn1,
            this.initialAvailableQtyDataGridViewTextBoxColumn1,
            this.approvedByDataGridViewTextBoxColumn1,
            this.commentsDataGridViewTextBoxColumn1,
            this.isActiveDataGridViewCheckBoxColumn1,
            this.createdByDataGridViewTextBoxColumn1,
            this.creationDateDataGridViewTextBoxColumn1,
            this.lastUpdatedByDataGridViewTextBoxColumn1,
            this.lastUpdateDateDataGridViewTextBoxColumn1,
            this.siteIdDataGridViewTextBoxColumn1,
            this.masterEntityIdDataGridViewTextBoxColumn1,
            this.synchStatusDataGridViewCheckBoxColumn1,
            this.guidDataGridViewTextBoxColumn1,
            this.isChangedDataGridViewCheckBoxColumn1});
            this.dgvUnavailableProducts.DataSource = this.unavailableProductsDTOBS;
            this.dgvUnavailableProducts.Location = new System.Drawing.Point(8, 56);
            this.dgvUnavailableProducts.Name = "dgvUnavailableProducts";
            this.dgvUnavailableProducts.RowHeadersVisible = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvUnavailableProducts.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvUnavailableProducts.RowTemplate.Height = 32;
            this.dgvUnavailableProducts.RowTemplate.ReadOnly = true;
            this.dgvUnavailableProducts.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvUnavailableProducts.Size = new System.Drawing.Size(471, 356);
            this.dgvUnavailableProducts.TabIndex = 2;
            this.dgvUnavailableProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnavailableProducts_CellClick);
            // 
            // cbAutoCompleteUnavailableProducts
            // 
            this.cbAutoCompleteUnavailableProducts.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbAutoCompleteUnavailableProducts.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbAutoCompleteUnavailableProducts.FormattingEnabled = true;
            this.cbAutoCompleteUnavailableProducts.Location = new System.Drawing.Point(8, 20);
            this.cbAutoCompleteUnavailableProducts.Name = "cbAutoCompleteUnavailableProducts";
            this.cbAutoCompleteUnavailableProducts.Size = new System.Drawing.Size(471, 26);
            this.cbAutoCompleteUnavailableProducts.TabIndex = 0;
            this.cbAutoCompleteUnavailableProducts.SelectedIndexChanged += new System.EventHandler(this.cbAutoCompleteUnavailableProducts_SelectedIndexChanged);
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblMessage.Location = new System.Drawing.Point(1, 523);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(1120, 25);
            this.lblMessage.TabIndex = 19;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbProductDisplayGroup
            // 
            this.cbProductDisplayGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProductDisplayGroup.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbProductDisplayGroup.FormattingEnabled = true;
            this.cbProductDisplayGroup.Location = new System.Drawing.Point(141, 13);
            this.cbProductDisplayGroup.Name = "cbProductDisplayGroup";
            this.cbProductDisplayGroup.Size = new System.Drawing.Size(335, 26);
            this.cbProductDisplayGroup.TabIndex = 0;
            this.cbProductDisplayGroup.SelectedIndexChanged += new System.EventHandler(this.cbProductDisplayGroup_SelectionChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 25);
            this.label1.TabIndex = 21;
            this.label1.Text = "Display Group";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BackgroundImage = global::Parafait_POS.Properties.Resources.customer_button_normal;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(608, 475);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(154, 45);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.customer_button_normal;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(353, 475);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(154, 45);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnMakeAvailable
            // 
            this.btnMakeAvailable.BackgroundImage = global::Parafait_POS.Properties.Resources.left_arrow_normal;
            this.btnMakeAvailable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMakeAvailable.Location = new System.Drawing.Point(530, 291);
            this.btnMakeAvailable.Name = "btnMakeAvailable";
            this.btnMakeAvailable.Size = new System.Drawing.Size(55, 55);
            this.btnMakeAvailable.TabIndex = 2;
            this.btnMakeAvailable.UseVisualStyleBackColor = true;
            this.btnMakeAvailable.Click += new System.EventHandler(this.btnMakeAvailable_Click);
            // 
            // btnMakeUnavailable
            // 
            this.btnMakeUnavailable.BackgroundImage = global::Parafait_POS.Properties.Resources.right_arrow_normal;
            this.btnMakeUnavailable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMakeUnavailable.Location = new System.Drawing.Point(530, 172);
            this.btnMakeUnavailable.Name = "btnMakeUnavailable";
            this.btnMakeUnavailable.Size = new System.Drawing.Size(55, 55);
            this.btnMakeUnavailable.TabIndex = 1;
            this.btnMakeUnavailable.UseVisualStyleBackColor = true;
            this.btnMakeUnavailable.Click += new System.EventHandler(this.btnMakeUnavailable_Click);
            // 
            // unavailableProductsDTOBS
            // 
            this.unavailableProductsDTOBS.DataSource = typeof(Semnox.Parafait.Product.ProductsAvailabilityDTO);
            // 
            // availableProductsDTOBS
            // 
            this.availableProductsDTOBS.DataSource = typeof(Semnox.Parafait.Product.ProductsAvailabilityDTO);
            // 
            // selAvl
            // 
            this.selAvl.HeaderText = "";
            this.selAvl.Name = "selAvl";
            this.selAvl.ReadOnly = true;
            this.selAvl.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.selAvl.Width = 35;
            // 
            // ProductName
            // 
            this.ProductName.DataPropertyName = "ProductName";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ProductName.DefaultCellStyle = dataGridViewCellStyle2;
            this.ProductName.HeaderText = "Product Name";
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            this.ProductName.Width = 401;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // productIdDataGridViewTextBoxColumn
            // 
            this.productIdDataGridViewTextBoxColumn.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn.HeaderText = "Product Id";
            this.productIdDataGridViewTextBoxColumn.Name = "productIdDataGridViewTextBoxColumn";
            this.productIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // isAvailableDataGridViewCheckBoxColumn
            // 
            this.isAvailableDataGridViewCheckBoxColumn.DataPropertyName = "IsAvailable";
            this.isAvailableDataGridViewCheckBoxColumn.HeaderText = "Is Available";
            this.isAvailableDataGridViewCheckBoxColumn.Name = "isAvailableDataGridViewCheckBoxColumn";
            this.isAvailableDataGridViewCheckBoxColumn.Visible = false;
            // 
            // availableQtyDataGridViewTextBoxColumn
            // 
            this.availableQtyDataGridViewTextBoxColumn.DataPropertyName = "AvailableQty";
            this.availableQtyDataGridViewTextBoxColumn.HeaderText = "Available Qty";
            this.availableQtyDataGridViewTextBoxColumn.Name = "availableQtyDataGridViewTextBoxColumn";
            this.availableQtyDataGridViewTextBoxColumn.Visible = false;
            // 
            // initialAvailableQtyDataGridViewTextBoxColumn
            // 
            this.initialAvailableQtyDataGridViewTextBoxColumn.DataPropertyName = "InitialAvailableQty";
            this.initialAvailableQtyDataGridViewTextBoxColumn.HeaderText = "Initial Available Qty";
            this.initialAvailableQtyDataGridViewTextBoxColumn.Name = "initialAvailableQtyDataGridViewTextBoxColumn";
            this.initialAvailableQtyDataGridViewTextBoxColumn.Visible = false;
            // 
            // unavailableTillDataGridViewTextBoxColumn
            // 
            this.unavailableTillDataGridViewTextBoxColumn.DataPropertyName = "UnavailableTill";
            this.unavailableTillDataGridViewTextBoxColumn.HeaderText = "Unavailable Till";
            this.unavailableTillDataGridViewTextBoxColumn.Name = "unavailableTillDataGridViewTextBoxColumn";
            this.unavailableTillDataGridViewTextBoxColumn.Visible = false;
            // 
            // approvedByDataGridViewTextBoxColumn
            // 
            this.approvedByDataGridViewTextBoxColumn.DataPropertyName = "ApprovedBy";
            this.approvedByDataGridViewTextBoxColumn.HeaderText = "Approved By";
            this.approvedByDataGridViewTextBoxColumn.Name = "approvedByDataGridViewTextBoxColumn";
            this.approvedByDataGridViewTextBoxColumn.Visible = false;
            // 
            // commentsDataGridViewTextBoxColumn
            // 
            this.commentsDataGridViewTextBoxColumn.DataPropertyName = "Comments";
            this.commentsDataGridViewTextBoxColumn.HeaderText = "Comments";
            this.commentsDataGridViewTextBoxColumn.Name = "commentsDataGridViewTextBoxColumn";
            this.commentsDataGridViewTextBoxColumn.Visible = false;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.Visible = false;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.Visible = false;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // siteIdDataGridViewTextBoxColumn
            // 
            this.siteIdDataGridViewTextBoxColumn.DataPropertyName = "SiteId";
            this.siteIdDataGridViewTextBoxColumn.HeaderText = "SiteId";
            this.siteIdDataGridViewTextBoxColumn.Name = "siteIdDataGridViewTextBoxColumn";
            this.siteIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn
            // 
            this.synchStatusDataGridViewCheckBoxColumn.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.Name = "synchStatusDataGridViewCheckBoxColumn";
            this.synchStatusDataGridViewCheckBoxColumn.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn
            // 
            this.guidDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn.Name = "guidDataGridViewTextBoxColumn";
            this.guidDataGridViewTextBoxColumn.Visible = false;
            // 
            // isChangedDataGridViewCheckBoxColumn
            // 
            this.isChangedDataGridViewCheckBoxColumn.DataPropertyName = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.HeaderText = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.Name = "isChangedDataGridViewCheckBoxColumn";
            this.isChangedDataGridViewCheckBoxColumn.Visible = false;
            // 
            // selUnavl
            // 
            this.selUnavl.HeaderText = "";
            this.selUnavl.Name = "selUnavl";
            this.selUnavl.ReadOnly = true;
            this.selUnavl.Width = 35;
            // 
            // btndgvUpdate
            // 
            this.btndgvUpdate.HeaderText = "Edit";
            this.btndgvUpdate.Name = "btndgvUpdate";
            this.btndgvUpdate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.btndgvUpdate.Text = "...";
            this.btndgvUpdate.UseColumnTextForButtonValue = true;
            // 
            // productNameDataGridViewTextBoxColumn1
            // 
            this.productNameDataGridViewTextBoxColumn1.DataPropertyName = "ProductName";
            this.productNameDataGridViewTextBoxColumn1.HeaderText = "Product Name";
            this.productNameDataGridViewTextBoxColumn1.Name = "productNameDataGridViewTextBoxColumn1";
            this.productNameDataGridViewTextBoxColumn1.ReadOnly = true;
            this.productNameDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.productNameDataGridViewTextBoxColumn1.Width = 201;
            // 
            // availableQtyDataGridViewTextBoxColumn1
            // 
            this.availableQtyDataGridViewTextBoxColumn1.DataPropertyName = "AvailableQty";
            this.availableQtyDataGridViewTextBoxColumn1.HeaderText = "Available Qty";
            this.availableQtyDataGridViewTextBoxColumn1.Name = "availableQtyDataGridViewTextBoxColumn1";
            this.availableQtyDataGridViewTextBoxColumn1.ReadOnly = true;
            this.availableQtyDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // unavailableTillDataGridViewTextBoxColumn1
            // 
            this.unavailableTillDataGridViewTextBoxColumn1.DataPropertyName = "UnavailableTill";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.unavailableTillDataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle5;
            this.unavailableTillDataGridViewTextBoxColumn1.HeaderText = "Unavailable Till";
            this.unavailableTillDataGridViewTextBoxColumn1.Name = "unavailableTillDataGridViewTextBoxColumn1";
            this.unavailableTillDataGridViewTextBoxColumn1.ReadOnly = true;
            this.unavailableTillDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // idDataGridViewTextBoxColumn1
            // 
            this.idDataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn1.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn1.Name = "idDataGridViewTextBoxColumn1";
            this.idDataGridViewTextBoxColumn1.Visible = false;
            // 
            // productIdDataGridViewTextBoxColumn1
            // 
            this.productIdDataGridViewTextBoxColumn1.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn1.HeaderText = "Product Id";
            this.productIdDataGridViewTextBoxColumn1.Name = "productIdDataGridViewTextBoxColumn1";
            this.productIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // isAvailableDataGridViewCheckBoxColumn1
            // 
            this.isAvailableDataGridViewCheckBoxColumn1.DataPropertyName = "IsAvailable";
            this.isAvailableDataGridViewCheckBoxColumn1.HeaderText = "Is Available";
            this.isAvailableDataGridViewCheckBoxColumn1.Name = "isAvailableDataGridViewCheckBoxColumn1";
            this.isAvailableDataGridViewCheckBoxColumn1.Visible = false;
            // 
            // initialAvailableQtyDataGridViewTextBoxColumn1
            // 
            this.initialAvailableQtyDataGridViewTextBoxColumn1.DataPropertyName = "InitialAvailableQty";
            this.initialAvailableQtyDataGridViewTextBoxColumn1.HeaderText = "Initial Available Qty";
            this.initialAvailableQtyDataGridViewTextBoxColumn1.Name = "initialAvailableQtyDataGridViewTextBoxColumn1";
            this.initialAvailableQtyDataGridViewTextBoxColumn1.Visible = false;
            // 
            // approvedByDataGridViewTextBoxColumn1
            // 
            this.approvedByDataGridViewTextBoxColumn1.DataPropertyName = "ApprovedBy";
            this.approvedByDataGridViewTextBoxColumn1.HeaderText = "Approved By";
            this.approvedByDataGridViewTextBoxColumn1.Name = "approvedByDataGridViewTextBoxColumn1";
            this.approvedByDataGridViewTextBoxColumn1.Visible = false;
            // 
            // commentsDataGridViewTextBoxColumn1
            // 
            this.commentsDataGridViewTextBoxColumn1.DataPropertyName = "Comments";
            this.commentsDataGridViewTextBoxColumn1.HeaderText = "Comments";
            this.commentsDataGridViewTextBoxColumn1.Name = "commentsDataGridViewTextBoxColumn1";
            this.commentsDataGridViewTextBoxColumn1.Visible = false;
            // 
            // isActiveDataGridViewCheckBoxColumn1
            // 
            this.isActiveDataGridViewCheckBoxColumn1.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn1.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn1.Name = "isActiveDataGridViewCheckBoxColumn1";
            this.isActiveDataGridViewCheckBoxColumn1.Visible = false;
            // 
            // createdByDataGridViewTextBoxColumn1
            // 
            this.createdByDataGridViewTextBoxColumn1.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn1.HeaderText = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn1.Name = "createdByDataGridViewTextBoxColumn1";
            this.createdByDataGridViewTextBoxColumn1.Visible = false;
            // 
            // creationDateDataGridViewTextBoxColumn1
            // 
            this.creationDateDataGridViewTextBoxColumn1.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn1.HeaderText = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn1.Name = "creationDateDataGridViewTextBoxColumn1";
            this.creationDateDataGridViewTextBoxColumn1.Visible = false;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn1
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn1.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn1.HeaderText = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn1.Name = "lastUpdatedByDataGridViewTextBoxColumn1";
            this.lastUpdatedByDataGridViewTextBoxColumn1.Visible = false;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn1
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn1.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn1.HeaderText = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn1.Name = "lastUpdateDateDataGridViewTextBoxColumn1";
            this.lastUpdateDateDataGridViewTextBoxColumn1.Visible = false;
            // 
            // siteIdDataGridViewTextBoxColumn1
            // 
            this.siteIdDataGridViewTextBoxColumn1.DataPropertyName = "SiteId";
            this.siteIdDataGridViewTextBoxColumn1.HeaderText = "SiteId";
            this.siteIdDataGridViewTextBoxColumn1.Name = "siteIdDataGridViewTextBoxColumn1";
            this.siteIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // masterEntityIdDataGridViewTextBoxColumn1
            // 
            this.masterEntityIdDataGridViewTextBoxColumn1.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn1.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn1.Name = "masterEntityIdDataGridViewTextBoxColumn1";
            this.masterEntityIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn1
            // 
            this.synchStatusDataGridViewCheckBoxColumn1.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn1.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn1.Name = "synchStatusDataGridViewCheckBoxColumn1";
            this.synchStatusDataGridViewCheckBoxColumn1.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn1
            // 
            this.guidDataGridViewTextBoxColumn1.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn1.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn1.Name = "guidDataGridViewTextBoxColumn1";
            this.guidDataGridViewTextBoxColumn1.Visible = false;
            // 
            // isChangedDataGridViewCheckBoxColumn1
            // 
            this.isChangedDataGridViewCheckBoxColumn1.DataPropertyName = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn1.HeaderText = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn1.Name = "isChangedDataGridViewCheckBoxColumn1";
            this.isChangedDataGridViewCheckBoxColumn1.Visible = false;
            // 
            // ProductsAvailabilityUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1129, 549);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbProductDisplayGroup);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnMakeAvailable);
            this.Controls.Add(this.btnMakeUnavailable);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ProductsAvailabilityUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Products Availability";
            this.Load += new System.EventHandler(this.ProductsAvailabilityUI_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableProducts)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnavailableProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unavailableProductsDTOBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.availableProductsDTOBS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnMakeUnavailable;
        private System.Windows.Forms.Button btnMakeAvailable;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvUnavailableProducts;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ComboBox cbProductDisplayGroup;
        private System.Windows.Forms.Label label1;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cbAutoCompleteAvailableProducts;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cbAutoCompleteUnavailableProducts;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView avlProductsVScrollBar;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView unavlProductsVScrollBar;
        private System.Windows.Forms.BindingSource availableProductsDTOBS;
        private System.Windows.Forms.BindingSource unavailableProductsDTOBS;
        private System.Windows.Forms.DataGridView dgvAvailableProducts;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selAvl;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isAvailableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn availableQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn initialAvailableQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unavailableTillDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approvedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selUnavl;
        private System.Windows.Forms.DataGridViewButtonColumn btndgvUpdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn availableQtyDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn unavailableTillDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isAvailableDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn initialAvailableQtyDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn approvedByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedDataGridViewCheckBoxColumn1;
    }
}