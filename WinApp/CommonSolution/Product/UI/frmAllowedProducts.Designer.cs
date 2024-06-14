namespace Semnox.Parafait.Product.UI
{
    partial class frmAllowedProducts
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvAllowedProducts = new System.Windows.Forms.DataGridView();
            this.productsAllowedInFacilityDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.defaultRentalProductDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.productTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productsIdDGV = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.facilityMapIdDGV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productsAllowedInFacilityMapId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllowedProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productsAllowedInFacilityDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.CausesValidation = false;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(234, 378);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 28);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(343, 378);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(123, 378);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 28);
            this.btnRefresh.TabIndex = 15;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(12, 378);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvAllowedProducts
            // 
            this.dgvAllowedProducts.AllowUserToDeleteRows = false;
            this.dgvAllowedProducts.AllowUserToResizeColumns = false;
            this.dgvAllowedProducts.AllowUserToResizeRows = false;
            this.dgvAllowedProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvAllowedProducts.AutoGenerateColumns = false;
            this.dgvAllowedProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllowedProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productsAllowedInFacilityMapId,
            this.facilityMapIdDGV,
            this.productsIdDGV,
            this.productTypeDataGridViewTextBoxColumn,
            this.defaultRentalProductDataGridViewCheckBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.CreatedBy,
            this.CreationDate,
            this.LastUpdatedBy,
            this.LastUpdateDate,
            this.masterEntityIdDataGridViewTextBoxColumn});
            this.dgvAllowedProducts.DataSource = this.productsAllowedInFacilityDTOBindingSource;
            this.dgvAllowedProducts.Location = new System.Drawing.Point(12, 12);
            this.dgvAllowedProducts.MultiSelect = false;
            this.dgvAllowedProducts.Name = "dgvAllowedProducts";
            this.dgvAllowedProducts.Size = new System.Drawing.Size(1068, 347);
            this.dgvAllowedProducts.TabIndex = 0;
            this.dgvAllowedProducts.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAllowedProducts_CellEnter);
            this.dgvAllowedProducts.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAllowedProducts_CellValueChanged);
            this.dgvAllowedProducts.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAllowedProducts_DataError);
            this.dgvAllowedProducts.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvAllowedProducts_DefaultValuesNeeded);
            // 
            // productsAllowedInFacilityDTOBindingSource
            // 
            this.productsAllowedInFacilityDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.ProductsAllowedInFacilityMapDTO);
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "Master EntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // LastUpdateDate
            // 
            this.LastUpdateDate.DataPropertyName = "LastUpdateDate";
            this.LastUpdateDate.HeaderText = "Last Update Date";
            this.LastUpdateDate.MinimumWidth = 120;
            this.LastUpdateDate.Name = "LastUpdateDate";
            this.LastUpdateDate.ReadOnly = true;
            this.LastUpdateDate.Width = 120;
            // 
            // LastUpdatedBy
            // 
            this.LastUpdatedBy.DataPropertyName = "LastUpdatedBy";
            this.LastUpdatedBy.HeaderText = "Last Updated By";
            this.LastUpdatedBy.MinimumWidth = 110;
            this.LastUpdatedBy.Name = "LastUpdatedBy";
            this.LastUpdatedBy.ReadOnly = true;
            this.LastUpdatedBy.Width = 120;
            // 
            // CreationDate
            // 
            this.CreationDate.DataPropertyName = "CreationDate";
            this.CreationDate.HeaderText = "Creation Date";
            this.CreationDate.Name = "CreationDate";
            this.CreationDate.ReadOnly = true;
            // 
            // CreatedBy
            // 
            this.CreatedBy.DataPropertyName = "CreatedBy";
            this.CreatedBy.HeaderText = "Created By";
            this.CreatedBy.Name = "CreatedBy";
            this.CreatedBy.ReadOnly = true;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "False";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // defaultRentalProductDataGridViewCheckBoxColumn
            // 
            this.defaultRentalProductDataGridViewCheckBoxColumn.DataPropertyName = "DefaultRentalProduct";
            this.defaultRentalProductDataGridViewCheckBoxColumn.FalseValue = "False";
            this.defaultRentalProductDataGridViewCheckBoxColumn.HeaderText = "Default Rental Product?";
            this.defaultRentalProductDataGridViewCheckBoxColumn.Name = "defaultRentalProductDataGridViewCheckBoxColumn";
            this.defaultRentalProductDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // productTypeDataGridViewTextBoxColumn
            // 
            this.productTypeDataGridViewTextBoxColumn.DataPropertyName = "ProductType";
            this.productTypeDataGridViewTextBoxColumn.HeaderText = "Product Type";
            this.productTypeDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.productTypeDataGridViewTextBoxColumn.Name = "productTypeDataGridViewTextBoxColumn";
            this.productTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.productTypeDataGridViewTextBoxColumn.Width = 150;
            // 
            // productsIdDGV
            // 
            this.productsIdDGV.DataPropertyName = "ProductsId";
            this.productsIdDGV.HeaderText = "Allowed Product";
            this.productsIdDGV.MinimumWidth = 200;
            this.productsIdDGV.Name = "productsIdDGV";
            this.productsIdDGV.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.productsIdDGV.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.productsIdDGV.Width = 200;
            // 
            // facilityMapIdDGV
            // 
            this.facilityMapIdDGV.DataPropertyName = "FacilityMapId";
            this.facilityMapIdDGV.HeaderText = "Facility Map";
            this.facilityMapIdDGV.Name = "facilityMapIdDGV";
            this.facilityMapIdDGV.Visible = false;

            // 
            // productsAllowedInFacilityMapId
            // 
            this.productsAllowedInFacilityMapId.DataPropertyName = "productsAllowedInFacilityMapId";
            this.productsAllowedInFacilityMapId.HeaderText = "Id";
            this.productsAllowedInFacilityMapId.Name = "productsAllowedInFacilityMapId";
            this.productsAllowedInFacilityMapId.Visible = true;
            this.productsAllowedInFacilityMapId.ReadOnly = true;

            // 
            // frmAllowedProducts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 419);
            this.Controls.Add(this.dgvAllowedProducts);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "frmAllowedProducts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Allowed Products";
            this.Load += new System.EventHandler(this.frmAllowedProducts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllowedProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productsAllowedInFacilityDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource productsAllowedInFacilityDTOBindingSource;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvAllowedProducts;  
        private System.Windows.Forms.DataGridViewTextBoxColumn facilityMapIdDGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn productsAllowedInFacilityMapId;
        private System.Windows.Forms.DataGridViewComboBoxColumn productsIdDGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn productTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn defaultRentalProductDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
    }
}