using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory
{
    partial class PurchaseTaxUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseTaxUI));
            this.dgvTax = new System.Windows.Forms.DataGridView();
            this.taxIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taxNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taxPercentageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeFlagDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.purchaseTaxDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lnkPublishToSite = new System.Windows.Forms.LinkLabel();
            this.groupBoxStructure = new System.Windows.Forms.GroupBox();
            this.dgvTaxStructure = new System.Windows.Forms.DataGridView();
            this.taxStructureIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structureNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.percentageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parentStructureIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.taxStructureDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TaxGroup = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseTaxDTOBindingSource)).BeginInit();
            this.groupBoxStructure.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxStructure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.taxStructureDTOBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTax
            // 
            this.dgvTax.AllowUserToResizeColumns = false;
            this.dgvTax.AllowUserToResizeRows = false;
            this.dgvTax.AutoGenerateColumns = false;
            this.dgvTax.BackgroundColor = System.Drawing.Color.SlateGray;
            this.dgvTax.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.taxIdDataGridViewTextBoxColumn,
            this.taxNameDataGridViewTextBoxColumn,
            this.taxPercentageDataGridViewTextBoxColumn,
            this.activeFlagDataGridViewTextBoxColumn});
            this.dgvTax.DataSource = this.purchaseTaxDTOBindingSource;
            this.dgvTax.Location = new System.Drawing.Point(53, 48);
            this.dgvTax.Name = "dgvTax";
            this.dgvTax.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvTax.Size = new System.Drawing.Size(1047, 216);
            this.dgvTax.TabIndex = 42;
            this.dgvTax.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTax_CellClick);
            this.dgvTax.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvTax_DataError);
            this.dgvTax.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewTax_DefaultValuesNeeded);
            this.dgvTax.Enter += new System.EventHandler(this.dgvTax_Enter);
            // 
            // taxIdDataGridViewTextBoxColumn
            // 
            this.taxIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.taxIdDataGridViewTextBoxColumn.DataPropertyName = "TaxId";
            this.taxIdDataGridViewTextBoxColumn.FillWeight = 160F;
            this.taxIdDataGridViewTextBoxColumn.HeaderText = "TaxId";
            this.taxIdDataGridViewTextBoxColumn.Name = "taxIdDataGridViewTextBoxColumn";
            this.taxIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.taxIdDataGridViewTextBoxColumn.Width = 160;
            // 
            // taxNameDataGridViewTextBoxColumn
            // 
            this.taxNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.taxNameDataGridViewTextBoxColumn.DataPropertyName = "TaxName";
            this.taxNameDataGridViewTextBoxColumn.FillWeight = 160F;
            this.taxNameDataGridViewTextBoxColumn.HeaderText = "Tax Name";
            this.taxNameDataGridViewTextBoxColumn.Name = "taxNameDataGridViewTextBoxColumn";
            this.taxNameDataGridViewTextBoxColumn.Width = 160;
            // 
            // taxPercentageDataGridViewTextBoxColumn
            // 
            this.taxPercentageDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.taxPercentageDataGridViewTextBoxColumn.DataPropertyName = "TaxPercentage";
            this.taxPercentageDataGridViewTextBoxColumn.FillWeight = 160F;
            this.taxPercentageDataGridViewTextBoxColumn.HeaderText = "Tax   Percentage";
            this.taxPercentageDataGridViewTextBoxColumn.Name = "taxPercentageDataGridViewTextBoxColumn";
            this.taxPercentageDataGridViewTextBoxColumn.Width = 160;
            // 
            // activeFlagDataGridViewTextBoxColumn
            // 
            this.activeFlagDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.activeFlagDataGridViewTextBoxColumn.DataPropertyName = "ActiveFlag";
            this.activeFlagDataGridViewTextBoxColumn.FalseValue = "false";
            this.activeFlagDataGridViewTextBoxColumn.FillWeight = 160F;
            this.activeFlagDataGridViewTextBoxColumn.HeaderText = "Active  Flag";
            this.activeFlagDataGridViewTextBoxColumn.Name = "activeFlagDataGridViewTextBoxColumn";
            this.activeFlagDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.activeFlagDataGridViewTextBoxColumn.TrueValue = "true";
            this.activeFlagDataGridViewTextBoxColumn.Width = 160;
            // 
            // purchaseTaxDTOBindingSource
            // 
            this.purchaseTaxDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.TaxDTO);
            this.purchaseTaxDTOBindingSource.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.taxDTOBindingSource_AddingNew);
            this.purchaseTaxDTOBindingSource.CurrentChanged += new System.EventHandler(this.dgvTax_CurrentChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(176, 562);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 34);
            this.btnRefresh.TabIndex = 44;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(316, 562);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 34);
            this.btnDelete.TabIndex = 45;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(459, 562);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 34);
            this.btnClose.TabIndex = 46;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(41, 562);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 34);
            this.btnSave.TabIndex = 43;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lnkPublishToSite
            // 
            this.lnkPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublishToSite.AutoSize = true;
            this.lnkPublishToSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublishToSite.Location = new System.Drawing.Point(989, 569);
            this.lnkPublishToSite.Name = "lnkPublishToSite";
            this.lnkPublishToSite.Size = new System.Drawing.Size(99, 13);
            this.lnkPublishToSite.TabIndex = 47;
            this.lnkPublishToSite.TabStop = true;
            this.lnkPublishToSite.Text = "Publish To Sites";
            this.lnkPublishToSite.Visible = false;
            this.lnkPublishToSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublishToSite_LinkClicked);
            // 
            // groupBoxStructure
            // 
            this.groupBoxStructure.Controls.Add(this.dgvTaxStructure);
            this.groupBoxStructure.Location = new System.Drawing.Point(39, 304);
            this.groupBoxStructure.Name = "groupBoxStructure";
            this.groupBoxStructure.Size = new System.Drawing.Size(1086, 228);
            this.groupBoxStructure.TabIndex = 48;
            this.groupBoxStructure.TabStop = false;
            this.groupBoxStructure.Text = "Tax Structure";
            // 
            // dgvTaxStructure
            // 
            this.dgvTaxStructure.AllowUserToResizeColumns = false;
            this.dgvTaxStructure.AllowUserToResizeRows = false;
            this.dgvTaxStructure.AutoGenerateColumns = false;
            this.dgvTaxStructure.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTaxStructure.BackgroundColor = System.Drawing.Color.SlateGray;
            this.dgvTaxStructure.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTaxStructure.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.taxStructureIdDataGridViewTextBoxColumn,
            this.structureNameDataGridViewTextBoxColumn,
            this.percentageDataGridViewTextBoxColumn,
            this.parentStructureIdDataGridViewComboBoxColumn});
            this.dgvTaxStructure.DataSource = this.taxStructureDTOBindingSource;
            this.dgvTaxStructure.Location = new System.Drawing.Point(14, 29);
            this.dgvTaxStructure.MultiSelect = false;
            this.dgvTaxStructure.Name = "dgvTaxStructure";
            this.dgvTaxStructure.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvTaxStructure.Size = new System.Drawing.Size(1047, 182);
            this.dgvTaxStructure.TabIndex = 0;
            this.dgvTaxStructure.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvTaxStructure_DataError);
            this.dgvTaxStructure.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewTaxStructure_DefaultValuesNeeded);
            this.dgvTaxStructure.Enter += new System.EventHandler(this.dgvTaxStructure_Enter);
            // 
            // taxStructureIdDataGridViewTextBoxColumn
            // 
            this.taxStructureIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.taxStructureIdDataGridViewTextBoxColumn.DataPropertyName = "TaxStructureId";
            this.taxStructureIdDataGridViewTextBoxColumn.HeaderText = "Tax Structure Id";
            this.taxStructureIdDataGridViewTextBoxColumn.Name = "taxStructureIdDataGridViewTextBoxColumn";
            this.taxStructureIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.taxStructureIdDataGridViewTextBoxColumn.Width = 160;
            // 
            // structureNameDataGridViewTextBoxColumn
            // 
            this.structureNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.structureNameDataGridViewTextBoxColumn.DataPropertyName = "StructureName";
            this.structureNameDataGridViewTextBoxColumn.HeaderText = "Structure Name";
            this.structureNameDataGridViewTextBoxColumn.Name = "structureNameDataGridViewTextBoxColumn";
            this.structureNameDataGridViewTextBoxColumn.Width = 160;
            // 
            // percentageDataGridViewTextBoxColumn
            // 
            this.percentageDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.percentageDataGridViewTextBoxColumn.DataPropertyName = "Percentage";
            this.percentageDataGridViewTextBoxColumn.HeaderText = "Percentage";
            this.percentageDataGridViewTextBoxColumn.Name = "percentageDataGridViewTextBoxColumn";
            this.percentageDataGridViewTextBoxColumn.Width = 160;
            // 
            // parentStructureIdDataGridViewComboBoxColumn
            // 
            this.parentStructureIdDataGridViewComboBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.parentStructureIdDataGridViewComboBoxColumn.DataPropertyName = "ParentStructureId";
            this.parentStructureIdDataGridViewComboBoxColumn.HeaderText = "ParentStructureId";
            this.parentStructureIdDataGridViewComboBoxColumn.Name = "parentStructureIdDataGridViewComboBoxColumn";
            this.parentStructureIdDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.parentStructureIdDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.parentStructureIdDataGridViewComboBoxColumn.Width = 160;
            // 
            // taxStructureDTOBindingSource
            // 
            this.taxStructureDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.TaxStructureDTO);
            // 
            // TaxGroup
            // 
            this.TaxGroup.Location = new System.Drawing.Point(39, 25);
            this.TaxGroup.Name = "TaxGroup";
            this.TaxGroup.Size = new System.Drawing.Size(1086, 260);
            this.TaxGroup.TabIndex = 49;
            this.TaxGroup.TabStop = false;
            this.TaxGroup.Text = "Tax Setup";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lnkPublishToSite);
            this.groupBox1.Location = new System.Drawing.Point(12, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1138, 600);
            this.groupBox1.TabIndex = 50;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tax";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // PurchaseTaxUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 591);
            this.Controls.Add(this.groupBoxStructure);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvTax);
            this.Controls.Add(this.TaxGroup);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "PurchaseTaxUI";
            this.Text = "Purchase Tax";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseTaxDTOBindingSource)).EndInit();
            this.groupBoxStructure.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxStructure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.taxStructureDTOBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTax;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.BindingSource purchaseTaxDTOBindingSource;       
        private System.Windows.Forms.LinkLabel lnkPublishToSite;
        private System.Windows.Forms.GroupBox groupBoxStructure;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvTaxStructure;
        private System.Windows.Forms.BindingSource taxStructureDTOBindingSource;
        private System.Windows.Forms.GroupBox TaxGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxPercentageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeFlagDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxStructureIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn structureNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn percentageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn parentStructureIdDataGridViewComboBoxColumn;
    }
}

