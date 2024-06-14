namespace Semnox.Parafait.Product
{
    partial class SaleGroupProductMapUI
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
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.txtProductSearch = new System.Windows.Forms.TextBox();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGroupSearch = new System.Windows.Forms.TextBox();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvDisplayData = new System.Windows.Forms.DataGridView();
            this.typeMapIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saleGroupIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.productIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.sequenceIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.saleGroupProductMapDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dgvGroupSearch = new System.Windows.Forms.DataGridView();
            this.dgvProductSearch = new System.Windows.Forms.DataGridView();
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.saleGroupProductMapDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.txtProductSearch);
            this.gpFilter.Controls.Add(this.cmbProduct);
            this.gpFilter.Controls.Add(this.label1);
            this.gpFilter.Controls.Add(this.txtGroupSearch);
            this.gpFilter.Controls.Add(this.cmbGroup);
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.lblName);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(12, 8);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(743, 47);
            this.gpFilter.TabIndex = 24;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // txtProductSearch
            // 
            this.txtProductSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.txtProductSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtProductSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtProductSearch.Location = new System.Drawing.Point(501, 19);
            this.txtProductSearch.Name = "txtProductSearch";
            this.txtProductSearch.Size = new System.Drawing.Size(115, 14);
            this.txtProductSearch.TabIndex = 24;
            this.txtProductSearch.TextChanged += new System.EventHandler(this.txtProductSearch_TextChanged);
            // 
            // cmbProduct
            // 
            this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduct.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(496, 15);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(136, 23);
            this.cmbProduct.TabIndex = 23;
            this.cmbProduct.SelectedValueChanged += new System.EventHandler(this.cmbProduct_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(396, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 20);
            this.label1.TabIndex = 22;
            this.label1.Text = "Product :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGroupSearch
            // 
            this.txtGroupSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.txtGroupSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGroupSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtGroupSearch.Location = new System.Drawing.Point(256, 21);
            this.txtGroupSearch.Name = "txtGroupSearch";
            this.txtGroupSearch.Size = new System.Drawing.Size(115, 14);
            this.txtGroupSearch.TabIndex = 20;
            this.txtGroupSearch.TextChanged += new System.EventHandler(this.txtGroupSearch_TextChanged);
            // 
            // cmbGroup
            // 
            this.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroup.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbGroup.FormattingEnabled = true;
            this.cmbGroup.Location = new System.Drawing.Point(251, 17);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Size = new System.Drawing.Size(136, 23);
            this.cmbGroup.TabIndex = 19;
            this.cmbGroup.SelectedValueChanged += new System.EventHandler(this.cmbGroup_SelectedValueChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(638, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(151, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(96, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Group :";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(12, 18);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 0;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(269, 296);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 23;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(397, 296);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(141, 296);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 21;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(13, 296);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvDisplayData
            // 
            this.dgvDisplayData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDisplayData.AutoGenerateColumns = false;
            this.dgvDisplayData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDisplayData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.typeMapIdDataGridViewTextBoxColumn,
            this.saleGroupIdDataGridViewTextBoxColumn,
            this.productIdDataGridViewTextBoxColumn,
            this.sequenceIdDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvDisplayData.DataSource = this.saleGroupProductMapDTOBindingSource;
            this.dgvDisplayData.Location = new System.Drawing.Point(12, 59);
            this.dgvDisplayData.Name = "dgvDisplayData";
            this.dgvDisplayData.Size = new System.Drawing.Size(743, 220);
            this.dgvDisplayData.TabIndex = 19;
            // 
            // typeMapIdDataGridViewTextBoxColumn
            // 
            this.typeMapIdDataGridViewTextBoxColumn.DataPropertyName = "TypeMapId";
            this.typeMapIdDataGridViewTextBoxColumn.HeaderText = "Map Id";
            this.typeMapIdDataGridViewTextBoxColumn.Name = "typeMapIdDataGridViewTextBoxColumn";
            this.typeMapIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // saleGroupIdDataGridViewTextBoxColumn
            // 
            this.saleGroupIdDataGridViewTextBoxColumn.DataPropertyName = "SaleGroupId";
            this.saleGroupIdDataGridViewTextBoxColumn.HeaderText = "Sale Group";
            this.saleGroupIdDataGridViewTextBoxColumn.Name = "saleGroupIdDataGridViewTextBoxColumn";
            this.saleGroupIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.saleGroupIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // productIdDataGridViewTextBoxColumn
            // 
            this.productIdDataGridViewTextBoxColumn.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn.HeaderText = "Product";
            this.productIdDataGridViewTextBoxColumn.Name = "productIdDataGridViewTextBoxColumn";
            this.productIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.productIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // sequenceIdDataGridViewTextBoxColumn
            // 
            this.sequenceIdDataGridViewTextBoxColumn.DataPropertyName = "SequenceId";
            this.sequenceIdDataGridViewTextBoxColumn.HeaderText = "Display Order";
            this.sequenceIdDataGridViewTextBoxColumn.Name = "sequenceIdDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // saleGroupProductMapDTOBindingSource
            // 
            this.saleGroupProductMapDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.SaleGroupProductMapDTO);
            // 
            // dgvGroupSearch
            // 
            this.dgvGroupSearch.AllowUserToAddRows = false;
            this.dgvGroupSearch.AllowUserToDeleteRows = false;
            this.dgvGroupSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvGroupSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGroupSearch.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvGroupSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGroupSearch.ColumnHeadersVisible = false;
            this.dgvGroupSearch.Location = new System.Drawing.Point(264, 50);
            this.dgvGroupSearch.Name = "dgvGroupSearch";
            this.dgvGroupSearch.ReadOnly = true;
            this.dgvGroupSearch.RowHeadersVisible = false;
            this.dgvGroupSearch.RowTemplate.Height = 35;
            this.dgvGroupSearch.Size = new System.Drawing.Size(134, 181);
            this.dgvGroupSearch.TabIndex = 21;
            this.dgvGroupSearch.Visible = false;
            this.dgvGroupSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGroupSearch_CellClick);
            // 
            // dgvProductSearch
            // 
            this.dgvProductSearch.AllowUserToAddRows = false;
            this.dgvProductSearch.AllowUserToDeleteRows = false;
            this.dgvProductSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvProductSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProductSearch.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvProductSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductSearch.ColumnHeadersVisible = false;
            this.dgvProductSearch.Location = new System.Drawing.Point(510, 47);
            this.dgvProductSearch.Name = "dgvProductSearch";
            this.dgvProductSearch.ReadOnly = true;
            this.dgvProductSearch.RowHeadersVisible = false;
            this.dgvProductSearch.RowTemplate.Height = 35;
            this.dgvProductSearch.Size = new System.Drawing.Size(134, 181);
            this.dgvProductSearch.TabIndex = 25;
            this.dgvProductSearch.Visible = false;
            this.dgvProductSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductSearch_CellClick);
            // 
            // SaleGroupProductMapUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 343);
            this.Controls.Add(this.dgvProductSearch);
            this.Controls.Add(this.dgvGroupSearch);
            this.Controls.Add(this.gpFilter);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvDisplayData);
            this.Name = "SaleGroupProductMapUI";
            this.Text = "Offer Group Product Map";
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.saleGroupProductMapDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSearch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvDisplayData;
        private System.Windows.Forms.TextBox txtProductSearch;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGroupSearch;
        private System.Windows.Forms.ComboBox cmbGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn squenceIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView dgvGroupSearch;
        private System.Windows.Forms.DataGridView dgvProductSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeMapIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn saleGroupIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn productIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sequenceIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource saleGroupProductMapDTOBindingSource;
    }
}