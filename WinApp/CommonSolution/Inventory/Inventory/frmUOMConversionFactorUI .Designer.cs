namespace Semnox.Parafait.Inventory
{
    partial class UOMConversionFactorUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UOMConversionFactorUI));
            this.dgvUOMConversionFactor = new System.Windows.Forms.DataGridView();
            this.UOMConversionFactorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lnkPublishToSite = new System.Windows.Forms.LinkLabel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbBaseUOM = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.conversionFactorDataGridViewTextBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbUOM = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UOMConversionFactorId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUOMConversionFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UOMConversionFactorBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvUOMConversionFactor
            // 
            this.dgvUOMConversionFactor.AutoGenerateColumns = false;
            this.dgvUOMConversionFactor.BackgroundColor = System.Drawing.Color.DarkSeaGreen;
            this.dgvUOMConversionFactor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUOMConversionFactor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUOMConversionFactor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cmbBaseUOM,
            this.conversionFactorDataGridViewTextBox,
            this.cmbUOM,
            this.isActiveDataGridViewCheckBoxColumn,
            this.UOMConversionFactorId,
            this.CreatedBy,
            this.CreationDate,
            this.LastUpdatedBy,
            this.LastUpdateDate});
            this.dgvUOMConversionFactor.DataSource = this.UOMConversionFactorBindingSource;
            this.dgvUOMConversionFactor.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dgvUOMConversionFactor.Location = new System.Drawing.Point(12, 26);
            this.dgvUOMConversionFactor.Name = "dgvUOMConversionFactor";
            this.dgvUOMConversionFactor.Size = new System.Drawing.Size(1073, 488);
            this.dgvUOMConversionFactor.TabIndex = 42;
            this.dgvUOMConversionFactor.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUOMConversionFactor_CellEnter);
            this.dgvUOMConversionFactor.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvUOMConversionFactor_DataError);
            // 
            // UOMConversionFactorBindingSource
            // 
            this.UOMConversionFactorBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.UOMConversionFactorDTO);
            // 
            // lnkPublishToSite
            // 
            this.lnkPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublishToSite.AutoSize = true;
            this.lnkPublishToSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublishToSite.Location = new System.Drawing.Point(974, 542);
            this.lnkPublishToSite.Name = "lnkPublishToSite";
            this.lnkPublishToSite.Size = new System.Drawing.Size(99, 13);
            this.lnkPublishToSite.TabIndex = 51;
            this.lnkPublishToSite.TabStop = true;
            this.lnkPublishToSite.Text = "Publish To Sites";
            this.lnkPublishToSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublishToSite_LinkClicked);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(143, 535);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 23);
            this.btnRefresh.TabIndex = 48;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(271, 535);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 23);
            this.btnDelete.TabIndex = 49;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(397, 535);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 23);
            this.btnClose.TabIndex = 50;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(17, 535);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 47;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbBaseUOM
            // 
            this.cmbBaseUOM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cmbBaseUOM.DataPropertyName = "BaseUOMId";
            this.cmbBaseUOM.HeaderText = "Base UOM";
            this.cmbBaseUOM.MinimumWidth = 20;
            this.cmbBaseUOM.Name = "cmbBaseUOM";
            this.cmbBaseUOM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cmbBaseUOM.Width = 81;
            // 
            // conversionFactorDataGridViewTextBox
            // 
            this.conversionFactorDataGridViewTextBox.DataPropertyName = "ConversionFactor";
            this.conversionFactorDataGridViewTextBox.HeaderText = "Factor";
            this.conversionFactorDataGridViewTextBox.MaxInputLength = 18;
            this.conversionFactorDataGridViewTextBox.MinimumWidth = 15;
            this.conversionFactorDataGridViewTextBox.Name = "conversionFactorDataGridViewTextBox";
            // 
            // cmbUOM
            // 
            this.cmbUOM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cmbUOM.DataPropertyName = "UOMId";
            this.cmbUOM.HeaderText = "UOM";
            this.cmbUOM.MinimumWidth = 20;
            this.cmbUOM.Name = "cmbUOM";
            this.cmbUOM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cmbUOM.Width = 80;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // UOMConversionFactorId
            // 
            this.UOMConversionFactorId.DataPropertyName = "UOMConversionFactorId";
            this.UOMConversionFactorId.HeaderText = "UOMConversionFactorId";
            this.UOMConversionFactorId.Name = "UOMConversionFactorId";
            this.UOMConversionFactorId.Visible = false;
            // 
            // CreatedBy
            // 
            this.CreatedBy.DataPropertyName = "CreatedBy";
            this.CreatedBy.HeaderText = "Created By";
            this.CreatedBy.Name = "CreatedBy";
            this.CreatedBy.ReadOnly = true;
            // 
            // CreationDate
            // 
            this.CreationDate.DataPropertyName = "CreationDate";
            this.CreationDate.HeaderText = "Creation Date";
            this.CreationDate.Name = "CreationDate";
            this.CreationDate.ReadOnly = true;
            // 
            // LastUpdatedBy
            // 
            this.LastUpdatedBy.DataPropertyName = "LastUpdatedBy";
            this.LastUpdatedBy.HeaderText = "Last Updated By";
            this.LastUpdatedBy.Name = "LastUpdatedBy";
            this.LastUpdatedBy.ReadOnly = true;
            // 
            // LastUpdateDate
            // 
            this.LastUpdateDate.DataPropertyName = "LastUpdateDate";
            this.LastUpdateDate.HeaderText = "Last Update Date";
            this.LastUpdateDate.Name = "LastUpdateDate";
            this.LastUpdateDate.ReadOnly = true;
            // 
            // UOMConversionFactorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1174, 591);
            this.Controls.Add(this.lnkPublishToSite);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvUOMConversionFactor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "UOMConversionFactorUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "UOM Conversion Factor";
            this.Load += new System.EventHandler(this.UOMConversionFactorUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUOMConversionFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UOMConversionFactorBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvUOMConversionFactor;
        private System.Windows.Forms.LinkLabel lnkPublishToSite;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.BindingSource UOMConversionFactorBindingSource;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbBaseUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn conversionFactorDataGridViewTextBox;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbUOM;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOMConversionFactorId;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdateDate;
    }
}