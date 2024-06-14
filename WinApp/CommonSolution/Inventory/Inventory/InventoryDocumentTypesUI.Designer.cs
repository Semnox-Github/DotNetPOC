namespace Semnox.Parafait.Inventory
{
    partial class InventoryDocumentTypesUI
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
            this.dgvDocumentTypes = new System.Windows.Forms.DataGridView();
            this.inventoryDocumentTypeDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpDocumentType = new System.Windows.Forms.GroupBox();
            this.documentTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.applicabilityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocumentTypes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryDocumentTypeDTOBindingSource)).BeginInit();
            this.grpDocumentType.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDocumentTypes
            // 
            this.dgvDocumentTypes.AllowUserToAddRows = false;
            this.dgvDocumentTypes.AllowUserToDeleteRows = false;
            this.dgvDocumentTypes.AutoGenerateColumns = false;
            this.dgvDocumentTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDocumentTypes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.documentTypeIdDataGridViewTextBoxColumn,
            this.codeDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.applicabilityDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn});
            this.dgvDocumentTypes.DataSource = this.inventoryDocumentTypeDTOBindingSource;
            this.dgvDocumentTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDocumentTypes.Location = new System.Drawing.Point(3, 17);
            this.dgvDocumentTypes.Name = "dgvDocumentTypes";
            this.dgvDocumentTypes.ReadOnly = true;
            this.dgvDocumentTypes.Size = new System.Drawing.Size(646, 232);
            this.dgvDocumentTypes.TabIndex = 0;
            // 
            // inventoryDocumentTypeDTOBindingSource
            // 
            this.inventoryDocumentTypeDTOBindingSource.DataSource = typeof(InventoryDocumentTypeDTO);
            // 
            // grpDocumentType
            // 
            this.grpDocumentType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDocumentType.Controls.Add(this.dgvDocumentTypes);
            this.grpDocumentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpDocumentType.Location = new System.Drawing.Point(6, 9);
            this.grpDocumentType.Name = "grpDocumentType";
            this.grpDocumentType.Size = new System.Drawing.Size(652, 252);
            this.grpDocumentType.TabIndex = 1;
            this.grpDocumentType.TabStop = false;
            this.grpDocumentType.Text = "Document Types";
            // 
            // documentTypeIdDataGridViewTextBoxColumn
            // 
            this.documentTypeIdDataGridViewTextBoxColumn.DataPropertyName = "DocumentTypeId";
            this.documentTypeIdDataGridViewTextBoxColumn.HeaderText = "Document Type Id";
            this.documentTypeIdDataGridViewTextBoxColumn.Name = "documentTypeIdDataGridViewTextBoxColumn";
            this.documentTypeIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // codeDataGridViewTextBoxColumn
            // 
            this.codeDataGridViewTextBoxColumn.DataPropertyName = "Code";
            this.codeDataGridViewTextBoxColumn.HeaderText = "Code";
            this.codeDataGridViewTextBoxColumn.Name = "codeDataGridViewTextBoxColumn";
            this.codeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // applicabilityDataGridViewTextBoxColumn
            // 
            this.applicabilityDataGridViewTextBoxColumn.DataPropertyName = "Applicability";
            this.applicabilityDataGridViewTextBoxColumn.HeaderText = "Applicability";
            this.applicabilityDataGridViewTextBoxColumn.Name = "applicabilityDataGridViewTextBoxColumn";
            this.applicabilityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isActiveDataGridViewTextBoxColumn
            // 
            this.isActiveDataGridViewTextBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn.FalseValue = "N";
            this.isActiveDataGridViewTextBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewTextBoxColumn.Name = "isActiveDataGridViewTextBoxColumn";
            this.isActiveDataGridViewTextBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isActiveDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // InventoryDocumentTypesUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 263);
            this.Controls.Add(this.grpDocumentType);
            this.Name = "InventoryDocumentTypesUI";
            this.Text = "Document Types";
            this.Load += new System.EventHandler(this.InventoryDocumentTypesUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocumentTypes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryDocumentTypeDTOBindingSource)).EndInit();
            this.grpDocumentType.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDocumentTypes;
        private System.Windows.Forms.GroupBox grpDocumentType;
        private System.Windows.Forms.BindingSource inventoryDocumentTypeDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn documentTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicabilityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn;
    }
}

