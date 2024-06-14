using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.GenericUtilities
{
    partial class frmEntityOverride
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvEntityExclusion = new System.Windows.Forms.DataGridView();
            this.entityExclusionDatesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblExclude = new System.Windows.Forms.Label();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entityNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entityGuidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overrideDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.includeExcludeFlagDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntityExclusion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityExclusionDatesDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(214, 283);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 57;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(308, 283);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 56;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(120, 283);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 55;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(26, 283);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 54;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvEntityExclusion
            // 
            this.dgvEntityExclusion.AutoGenerateColumns = false;
            this.dgvEntityExclusion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEntityExclusion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.entityNameDataGridViewTextBoxColumn,
            this.entityGuidDataGridViewTextBoxColumn,
            this.overrideDateDataGridViewTextBoxColumn,
            this.includeExcludeFlagDataGridViewCheckBoxColumn,
            this.dayDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.lastUpdatedDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn});
            this.dgvEntityExclusion.DataSource = this.entityExclusionDatesDTOBindingSource;
            this.dgvEntityExclusion.Location = new System.Drawing.Point(26, 53);
            this.dgvEntityExclusion.Name = "dgvEntityExclusion";
            this.dgvEntityExclusion.Size = new System.Drawing.Size(747, 211);
            this.dgvEntityExclusion.TabIndex = 53;
            this.dgvEntityExclusion.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvEntityExclusion_CellValidating);
            this.dgvEntityExclusion.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvEntityExclusion_DataError);
            // 
            // entityExclusionDatesDTOBindingSource
            // 
            this.entityExclusionDatesDTOBindingSource.DataSource = typeof(EntityOverrideDatesDTO);
            // 
            // lblExclude
            // 
            this.lblExclude.AutoSize = true;
            this.lblExclude.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblExclude.Location = new System.Drawing.Point(25, 22);
            this.lblExclude.Name = "lblExclude";
            this.lblExclude.Size = new System.Drawing.Size(185, 16);
            this.lblExclude.TabIndex = 59;
            this.lblExclude.Text = "Include / Exclude Days from";
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "Id";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // entityNameDataGridViewTextBoxColumn
            // 
            this.entityNameDataGridViewTextBoxColumn.DataPropertyName = "EntityName";
            this.entityNameDataGridViewTextBoxColumn.HeaderText = "Entity Name";
            this.entityNameDataGridViewTextBoxColumn.Name = "entityNameDataGridViewTextBoxColumn";
            this.entityNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // entityGuidDataGridViewTextBoxColumn
            // 
            this.entityGuidDataGridViewTextBoxColumn.DataPropertyName = "EntityGuid";
            this.entityGuidDataGridViewTextBoxColumn.HeaderText = "Entity Guid";
            this.entityGuidDataGridViewTextBoxColumn.Name = "entityGuidDataGridViewTextBoxColumn";
            this.entityGuidDataGridViewTextBoxColumn.Visible = false;
            // 
            // overrideDateDataGridViewTextBoxColumn
            // 
            this.overrideDateDataGridViewTextBoxColumn.DataPropertyName = "OverrideDate";
            this.overrideDateDataGridViewTextBoxColumn.HeaderText = "Override Date";
            this.overrideDateDataGridViewTextBoxColumn.Name = "overrideDateDataGridViewTextBoxColumn";
            // 
            // includeExcludeFlagDataGridViewCheckBoxColumn
            // 
            this.includeExcludeFlagDataGridViewCheckBoxColumn.DataPropertyName = "IncludeExcludeFlag";
            this.includeExcludeFlagDataGridViewCheckBoxColumn.HeaderText = "Include This Day? ";
            this.includeExcludeFlagDataGridViewCheckBoxColumn.Name = "includeExcludeFlagDataGridViewCheckBoxColumn";
            // 
            // dayDataGridViewTextBoxColumn
            // 
            this.dayDataGridViewTextBoxColumn.DataPropertyName = "Day";
            this.dayDataGridViewTextBoxColumn.HeaderText = "Day";
            this.dayDataGridViewTextBoxColumn.Name = "dayDataGridViewTextBoxColumn";
            this.dayDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dayDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn.HeaderText = "Last Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn.Name = "lastUpdatedDateDataGridViewTextBoxColumn";
            this.lastUpdatedDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // frmEntityOverride
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 327);
            this.Controls.Add(this.dgvEntityExclusion);
            this.Controls.Add(this.lblExclude);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEntityOverride";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Entity Exclusion";
            this.Load += new System.EventHandler(this.frmEntityOverride_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntityExclusion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entityExclusionDatesDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvEntityExclusion;
        private System.Windows.Forms.Label lblExclude;
        private System.Windows.Forms.BindingSource entityExclusionDatesDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn entityNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn entityGuidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overrideDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn includeExcludeFlagDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
    }
}