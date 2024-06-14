using Semnox.Parafait.Product;

namespace Semnox.Parafait.Redemption
{
    partial class RedemptionCurrencyUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RedemptionCurrencyUI));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvRedemptionCurrency = new System.Windows.Forms.DataGridView();
            this.currencyIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.productDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.valueInTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GenerateBarcode = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ShowQtyPrompt = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ManagerApproval = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ShortCutKeys = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lastModifiedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redemptionCurrencyDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lnkPublishToSite = new System.Windows.Forms.LinkLabel();
            this.btnCurrencyRule = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptionCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionCurrencyDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(597, 546);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 23);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(169, 546);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(109, 23);
            this.btnRefresh.TabIndex = 22;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(315, 546);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(102, 23);
            this.btnDelete.TabIndex = 23;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(26, 546);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvRedemptionCurrency
            // 
            this.dgvRedemptionCurrency.AutoGenerateColumns = false;
            this.dgvRedemptionCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRedemptionCurrency.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRedemptionCurrency.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.currencyIdDataGridViewTextBoxColumn,
            this.currencyNameDataGridViewTextBoxColumn,
            this.productIdDataGridViewTextBoxColumn,
            this.valueInTicketsDataGridViewTextBoxColumn,
            this.barCodeDataGridViewTextBoxColumn,
            this.GenerateBarcode,
            this.ShowQtyPrompt,
            this.ManagerApproval,
            this.ShortCutKeys,
            this.IsActive,
            this.lastModifiedByDataGridViewTextBoxColumn,
            this.lastUpdatedDateDataGridViewTextBoxColumn});
            this.dgvRedemptionCurrency.DataSource = this.redemptionCurrencyDTOBindingSource;
            this.dgvRedemptionCurrency.GridColor = System.Drawing.Color.DarkOliveGreen;
            this.dgvRedemptionCurrency.Location = new System.Drawing.Point(28, 22);
            this.dgvRedemptionCurrency.Name = "dgvRedemptionCurrency";
            this.dgvRedemptionCurrency.RowHeadersVisible = false;
            this.dgvRedemptionCurrency.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRedemptionCurrency.Size = new System.Drawing.Size(1127, 506);
            this.dgvRedemptionCurrency.TabIndex = 20;
            this.dgvRedemptionCurrency.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRedemptionCurrency_CellContentClick);
            // 
            // currencyIdDataGridViewTextBoxColumn
            // 
            this.currencyIdDataGridViewTextBoxColumn.DataPropertyName = "CurrencyId";
            this.currencyIdDataGridViewTextBoxColumn.HeaderText = "Currency Id";
            this.currencyIdDataGridViewTextBoxColumn.Name = "currencyIdDataGridViewTextBoxColumn";
            this.currencyIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // currencyNameDataGridViewTextBoxColumn
            // 
            this.currencyNameDataGridViewTextBoxColumn.DataPropertyName = "CurrencyName";
            this.currencyNameDataGridViewTextBoxColumn.HeaderText = "Currency Name";
            this.currencyNameDataGridViewTextBoxColumn.Name = "currencyNameDataGridViewTextBoxColumn";
            // 
            // productIdDataGridViewTextBoxColumn
            // 
            this.productIdDataGridViewTextBoxColumn.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn.DataSource = this.productDTOBindingSource;
            this.productIdDataGridViewTextBoxColumn.DisplayMember = "Code";
            this.productIdDataGridViewTextBoxColumn.HeaderText = "Inv. Product";
            this.productIdDataGridViewTextBoxColumn.Name = "productIdDataGridViewTextBoxColumn";
            this.productIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.productIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.productIdDataGridViewTextBoxColumn.ValueMember = "ProductId";
            // 
            // productDTOBindingSource
            // 
            this.productDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.ProductDTO);
            // 
            // valueInTicketsDataGridViewTextBoxColumn
            // 
            this.valueInTicketsDataGridViewTextBoxColumn.DataPropertyName = "ValueInTickets";
            this.valueInTicketsDataGridViewTextBoxColumn.HeaderText = "Value In Tickets";
            this.valueInTicketsDataGridViewTextBoxColumn.Name = "valueInTicketsDataGridViewTextBoxColumn";
            this.valueInTicketsDataGridViewTextBoxColumn.Width = 60;
            // 
            // barCodeDataGridViewTextBoxColumn
            // 
            this.barCodeDataGridViewTextBoxColumn.DataPropertyName = "BarCode";
            this.barCodeDataGridViewTextBoxColumn.HeaderText = "Bar Code";
            this.barCodeDataGridViewTextBoxColumn.Name = "barCodeDataGridViewTextBoxColumn";
            // 
            // GenerateBarcode
            // 
            this.GenerateBarcode.HeaderText = "...";
            this.GenerateBarcode.Name = "GenerateBarcode";
            this.GenerateBarcode.UseColumnTextForButtonValue = true;
            this.GenerateBarcode.Width = 50;
            // 
            // ShowQtyPrompt
            // 
            this.ShowQtyPrompt.DataPropertyName = "ShowQtyPrompt";
            this.ShowQtyPrompt.FalseValue = "false";
            this.ShowQtyPrompt.HeaderText = "Show Quantity Prompt?";
            this.ShowQtyPrompt.Name = "ShowQtyPrompt";
            this.ShowQtyPrompt.TrueValue = "true";
            this.ShowQtyPrompt.Width = 90;
            // 
            // ManagerApproval
            // 
            this.ManagerApproval.DataPropertyName = "ManagerApproval";
            this.ManagerApproval.FalseValue = "false";
            this.ManagerApproval.HeaderText = "Requires Manager Approval";
            this.ManagerApproval.Name = "ManagerApproval";
            this.ManagerApproval.TrueValue = "true";
            // 
            // ShortCutKeys
            // 
            this.ShortCutKeys.DataPropertyName = "ShortCutKeys";
            this.ShortCutKeys.HeaderText = "Shortcut Keys";
            this.ShortCutKeys.Name = "ShortCutKeys";
            // 
            // IsActive
            // 
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.FalseValue = "false";
            this.IsActive.HeaderText = "Is Active";
            this.IsActive.Name = "IsActive";
            this.IsActive.TrueValue = "true";
            // 
            // lastModifiedByDataGridViewTextBoxColumn
            // 
            this.lastModifiedByDataGridViewTextBoxColumn.DataPropertyName = "LastModifiedBy";
            this.lastModifiedByDataGridViewTextBoxColumn.HeaderText = "Last Updated By";
            this.lastModifiedByDataGridViewTextBoxColumn.Name = "lastModifiedByDataGridViewTextBoxColumn";
            this.lastModifiedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn.HeaderText = "Last Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn.Name = "lastUpdatedDateDataGridViewTextBoxColumn";
            this.lastUpdatedDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // redemptionCurrencyDTOBindingSource
            // 
            this.redemptionCurrencyDTOBindingSource.DataSource = typeof(Semnox.Parafait.Redemption.RedemptionCurrencyDTO);
            // 
            // lnkPublishToSite
            // 
            this.lnkPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublishToSite.AutoSize = true;
            this.lnkPublishToSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublishToSite.Location = new System.Drawing.Point(1056, 556);
            this.lnkPublishToSite.Name = "lnkPublishToSite";
            this.lnkPublishToSite.Size = new System.Drawing.Size(99, 13);
            this.lnkPublishToSite.TabIndex = 51;
            this.lnkPublishToSite.TabStop = true;
            this.lnkPublishToSite.Text = "Publish To Sites";
            this.lnkPublishToSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublishToSite_LinkClicked);
            // 
            // btnCurrencyRule
            // 
            this.btnCurrencyRule.Location = new System.Drawing.Point(462, 546);
            this.btnCurrencyRule.Name = "btnCurrencyRule";
            this.btnCurrencyRule.Size = new System.Drawing.Size(91, 23);
            this.btnCurrencyRule.TabIndex = 52;
            this.btnCurrencyRule.Text = "Currency rule";
            this.btnCurrencyRule.UseVisualStyleBackColor = true;
            this.btnCurrencyRule.Click += new System.EventHandler(this.btnCurrencyRule_Click);
            // 
            // RedemptionCurrencyUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 591);
            this.Controls.Add(this.btnCurrencyRule);
            this.Controls.Add(this.lnkPublishToSite);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvRedemptionCurrency);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "RedemptionCurrencyUI";
            this.Text = "Redemption Currency";
            this.Load += new System.EventHandler(this.RedemptionCurrencyUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptionCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionCurrencyDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvRedemptionCurrency;
        private System.Windows.Forms.BindingSource redemptionCurrencyDTOBindingSource;
        private System.Windows.Forms.BindingSource productDTOBindingSource;
        private System.Windows.Forms.LinkLabel lnkPublishToSite;
        private System.Windows.Forms.Button btnCurrencyRule;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn productIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueInTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn barCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn GenerateBarcode;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ShowQtyPrompt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ManagerApproval;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShortCutKeys;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastModifiedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
    }
}

