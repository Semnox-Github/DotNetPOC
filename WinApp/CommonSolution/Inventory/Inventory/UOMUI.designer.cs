using Semnox.Parafait.Inventory;

namespace Semnox.Parafait.Inventory
{
    partial class UOMUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UOMUI));
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lnkPublishToSite = new System.Windows.Forms.LinkLabel();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvUOM = new System.Windows.Forms.DataGridView();
            this.uOMDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uomConversionDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uOMIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uOMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOMConversion = new System.Windows.Forms.DataGridViewButtonColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uOMDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uomConversionDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(161, 527);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 23);
            this.btnRefresh.TabIndex = 43;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(289, 527);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 23);
            this.btnDelete.TabIndex = 44;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(415, 527);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 23);
            this.btnClose.TabIndex = 45;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(35, 527);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 42;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lnkPublishToSite
            // 
            this.lnkPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublishToSite.AutoSize = true;
            this.lnkPublishToSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublishToSite.Location = new System.Drawing.Point(977, 534);
            this.lnkPublishToSite.Name = "lnkPublishToSite";
            this.lnkPublishToSite.Size = new System.Drawing.Size(99, 13);
            this.lnkPublishToSite.TabIndex = 46;
            this.lnkPublishToSite.TabStop = true;
            this.lnkPublishToSite.Text = "Publish To Sites";
            this.lnkPublishToSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublishToSite_LinkClicked);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "UOMId";
            this.dataGridViewTextBoxColumn1.HeaderText = "UOMId";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "UOM";
            this.dataGridViewTextBoxColumn2.HeaderText = "UOM";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Remarks";
            this.dataGridViewTextBoxColumn3.HeaderText = "Remarks";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "UOMConversionFactorId";
            this.dataGridViewTextBoxColumn4.HeaderText = "UOMConversionFactorId";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "UOMId";
            this.dataGridViewTextBoxColumn5.HeaderText = "UOMId";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "BaseUOMId";
            this.dataGridViewTextBoxColumn6.HeaderText = "BaseUOMId";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "ConversionFactor";
            this.dataGridViewTextBoxColumn7.HeaderText = "ConversionFactor";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dgvUOM
            // 
            this.dgvUOM.AutoGenerateColumns = false;
            this.dgvUOM.BackgroundColor = System.Drawing.Color.DarkSeaGreen;
            this.dgvUOM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUOM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.uOMIdDataGridViewTextBoxColumn,
            this.uOMDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.UOMConversion,
            this.IsActive,
            this.CreatedBy,
            this.CreationDate,
            this.LastUpdatedBy,
            this.LastUpdateDate});
            this.dgvUOM.DataSource = this.uOMDTOBindingSource;
            this.dgvUOM.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dgvUOM.Location = new System.Drawing.Point(28, 37);
            this.dgvUOM.Name = "dgvUOM";
            this.dgvUOM.Size = new System.Drawing.Size(1057, 468);
            this.dgvUOM.TabIndex = 41;
            this.dgvUOM.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUOM_CellContentClick);
            this.dgvUOM.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvUOM_DataError);
            // 
            // uOMDTOBindingSource
            // 
            this.uOMDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.UOMDTO);
            // 
            // uomConversionDTOBindingSource
            // 
            this.uomConversionDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.UOMConversionFactorDTO);
            // 
            // uOMIdDataGridViewTextBoxColumn
            // 
            this.uOMIdDataGridViewTextBoxColumn.DataPropertyName = "UOMId";
            this.uOMIdDataGridViewTextBoxColumn.HeaderText = "UOMId";
            this.uOMIdDataGridViewTextBoxColumn.MinimumWidth = 15;
            this.uOMIdDataGridViewTextBoxColumn.Name = "uOMIdDataGridViewTextBoxColumn";
            this.uOMIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // uOMDataGridViewTextBoxColumn
            // 
            this.uOMDataGridViewTextBoxColumn.DataPropertyName = "UOM";
            this.uOMDataGridViewTextBoxColumn.HeaderText = "UOM";
            this.uOMDataGridViewTextBoxColumn.MinimumWidth = 15;
            this.uOMDataGridViewTextBoxColumn.Name = "uOMDataGridViewTextBoxColumn";
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.MinimumWidth = 15;
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            // 
            // UOMConversion
            // 
            this.UOMConversion.HeaderText = "Conversion";
            this.UOMConversion.Name = "UOMConversion";
            this.UOMConversion.Text = "....";
            this.UOMConversion.UseColumnTextForButtonValue = true;
            this.UOMConversion.Width = 40;
            // 
            // IsActive
            // 
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.HeaderText = "Is Active";
            this.IsActive.Name = "IsActive";
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
            // UOMUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1174, 591);
            this.Controls.Add(this.dgvUOM);
            this.Controls.Add(this.lnkPublishToSite);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "UOMUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Units of Measure";
            this.Load += new System.EventHandler(this.UOMUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uOMDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uomConversionDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isactiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource uOMDTOBindingSource;
        private System.Windows.Forms.LinkLabel lnkPublishToSite;
        private System.Windows.Forms.BindingSource uomConversionDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridView dgvUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn uOMIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uOMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn UOMConversion;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdateDate;
    }
}