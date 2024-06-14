namespace Semnox.Parafait.Inventory
{
    partial class LocationUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationUI));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvLocation = new System.Windows.Forms.DataGridView();
            this.locationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksMandatoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isAvailableToSellDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isStoreDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isTurnInLocationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.massUpdatedAllowedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.locationTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.barcodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GenerateBarcode = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Custom = new System.Windows.Forms.DataGridViewButtonColumn();
            this.customDataSetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.externalSystemReferenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnImportMachines = new System.Windows.Forms.LinkLabel();
            this.lnkPublishToSite = new System.Windows.Forms.LinkLabel();
            this.locationTypeDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtLocationName = new System.Windows.Forms.TextBox();
            this.lblLocationName = new System.Windows.Forms.Label();
            this.lblLocationType = new System.Windows.Forms.Label();
            this.cbxActive = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbLocationType = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.filterGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationTypeDTOBindingSource)).BeginInit();
            this.filterGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(470, 552);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 23);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(178, 552);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 23);
            this.btnRefresh.TabIndex = 17;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(322, 552);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 23);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(34, 552);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvLocation
            // 
            this.dgvLocation.AutoGenerateColumns = false;
            this.dgvLocation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLocation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLocation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.locationIdDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.remarksMandatoryDataGridViewTextBoxColumn,
            this.isAvailableToSellDataGridViewTextBoxColumn,
            this.isStoreDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn,
            this.isTurnInLocationDataGridViewTextBoxColumn,
            this.massUpdatedAllowedDataGridViewTextBoxColumn,
            this.locationTypeIdDataGridViewTextBoxColumn,
            this.barcodeDataGridViewTextBoxColumn,
            this.GenerateBarcode,
            this.Custom,
            this.customDataSetIdDataGridViewTextBoxColumn,
            this.externalSystemReferenceDataGridViewTextBoxColumn});
            this.dgvLocation.DataSource = this.locationDTOBindingSource;
            this.dgvLocation.GridColor = System.Drawing.Color.DarkOliveGreen;
            this.dgvLocation.Location = new System.Drawing.Point(33, 65);
            this.dgvLocation.Name = "dgvLocation";
            this.dgvLocation.RowHeadersVisible = false;
            this.dgvLocation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLocation.Size = new System.Drawing.Size(1107, 470);
            this.dgvLocation.TabIndex = 15;
            this.dgvLocation.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLocation_CellContentClick);
            this.dgvLocation.SelectionChanged += new System.EventHandler(this.dgvLocation_SelectionChanged);
            // 
            // locationIdDataGridViewTextBoxColumn
            // 
            this.locationIdDataGridViewTextBoxColumn.DataPropertyName = "LocationId";
            this.locationIdDataGridViewTextBoxColumn.HeaderText = "Location Id";
            this.locationIdDataGridViewTextBoxColumn.Name = "locationIdDataGridViewTextBoxColumn";
            this.locationIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // remarksMandatoryDataGridViewTextBoxColumn
            // 
            this.remarksMandatoryDataGridViewTextBoxColumn.DataPropertyName = "RemarksMandatory";
            this.remarksMandatoryDataGridViewTextBoxColumn.FalseValue = "N";
            this.remarksMandatoryDataGridViewTextBoxColumn.HeaderText = "Remarks Mandatory";
            this.remarksMandatoryDataGridViewTextBoxColumn.Name = "remarksMandatoryDataGridViewTextBoxColumn";
            this.remarksMandatoryDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.remarksMandatoryDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.remarksMandatoryDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // isAvailableToSellDataGridViewTextBoxColumn
            // 
            this.isAvailableToSellDataGridViewTextBoxColumn.DataPropertyName = "IsAvailableToSell";
            this.isAvailableToSellDataGridViewTextBoxColumn.FalseValue = "N";
            this.isAvailableToSellDataGridViewTextBoxColumn.HeaderText = "Available To Sell";
            this.isAvailableToSellDataGridViewTextBoxColumn.Name = "isAvailableToSellDataGridViewTextBoxColumn";
            this.isAvailableToSellDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isAvailableToSellDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isAvailableToSellDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // isStoreDataGridViewTextBoxColumn
            // 
            this.isStoreDataGridViewTextBoxColumn.DataPropertyName = "IsStore";
            this.isStoreDataGridViewTextBoxColumn.FalseValue = "N";
            this.isStoreDataGridViewTextBoxColumn.HeaderText = "Is Store";
            this.isStoreDataGridViewTextBoxColumn.Name = "isStoreDataGridViewTextBoxColumn";
            this.isStoreDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isStoreDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isStoreDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // isActiveDataGridViewTextBoxColumn
            // 
            this.isActiveDataGridViewTextBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn.FalseValue = "false";
            this.isActiveDataGridViewTextBoxColumn.HeaderText = "Is Active";
            this.isActiveDataGridViewTextBoxColumn.Name = "isActiveDataGridViewTextBoxColumn";
            this.isActiveDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isActiveDataGridViewTextBoxColumn.TrueValue = "true";
            // 
            // isTurnInLocationDataGridViewTextBoxColumn
            // 
            this.isTurnInLocationDataGridViewTextBoxColumn.DataPropertyName = "IsTurnInLocation";
            this.isTurnInLocationDataGridViewTextBoxColumn.FalseValue = "N";
            this.isTurnInLocationDataGridViewTextBoxColumn.HeaderText = "Turn In Location";
            this.isTurnInLocationDataGridViewTextBoxColumn.Name = "isTurnInLocationDataGridViewTextBoxColumn";
            this.isTurnInLocationDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isTurnInLocationDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isTurnInLocationDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // massUpdatedAllowedDataGridViewTextBoxColumn
            // 
            this.massUpdatedAllowedDataGridViewTextBoxColumn.DataPropertyName = "MassUpdatedAllowed";
            this.massUpdatedAllowedDataGridViewTextBoxColumn.FalseValue = "N";
            this.massUpdatedAllowedDataGridViewTextBoxColumn.HeaderText = "Allow Mass Update";
            this.massUpdatedAllowedDataGridViewTextBoxColumn.Name = "massUpdatedAllowedDataGridViewTextBoxColumn";
            this.massUpdatedAllowedDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.massUpdatedAllowedDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.massUpdatedAllowedDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // locationTypeIdDataGridViewTextBoxColumn
            // 
            this.locationTypeIdDataGridViewTextBoxColumn.DataPropertyName = "LocationTypeId";
            this.locationTypeIdDataGridViewTextBoxColumn.HeaderText = "Location Type Id";
            this.locationTypeIdDataGridViewTextBoxColumn.Name = "locationTypeIdDataGridViewTextBoxColumn";
            this.locationTypeIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.locationTypeIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // barcodeDataGridViewTextBoxColumn
            // 
            this.barcodeDataGridViewTextBoxColumn.DataPropertyName = "Barcode";
            this.barcodeDataGridViewTextBoxColumn.HeaderText = "Barcode";
            this.barcodeDataGridViewTextBoxColumn.Name = "barcodeDataGridViewTextBoxColumn";
            // 
            // GenerateBarcode
            // 
            this.GenerateBarcode.HeaderText = "";
            this.GenerateBarcode.Name = "GenerateBarcode";
            this.GenerateBarcode.Text = "...";
            this.GenerateBarcode.UseColumnTextForButtonValue = true;
            // 
            // Custom
            // 
            this.Custom.HeaderText = "Custom";
            this.Custom.Name = "Custom";
            this.Custom.Text = "...";
            this.Custom.UseColumnTextForButtonValue = true;
            // 
            // customDataSetIdDataGridViewTextBoxColumn
            // 
            this.customDataSetIdDataGridViewTextBoxColumn.DataPropertyName = "CustomDataSetId";
            this.customDataSetIdDataGridViewTextBoxColumn.HeaderText = "CustomDataSetId";
            this.customDataSetIdDataGridViewTextBoxColumn.Name = "customDataSetIdDataGridViewTextBoxColumn";
            this.customDataSetIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.customDataSetIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // externalSystemReferenceDataGridViewTextBoxColumn
            // 
            this.externalSystemReferenceDataGridViewTextBoxColumn.DataPropertyName = "ExternalSystemReference";
            this.externalSystemReferenceDataGridViewTextBoxColumn.HeaderText = "External System Reference";
            this.externalSystemReferenceDataGridViewTextBoxColumn.Name = "externalSystemReferenceDataGridViewTextBoxColumn";
            // 
            // locationDTOBindingSource
            // 
            this.locationDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.LocationDTO);
            // 
            // btnImportMachines
            // 
            this.btnImportMachines.AutoSize = true;
            this.btnImportMachines.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportMachines.Location = new System.Drawing.Point(1026, 562);
            this.btnImportMachines.Name = "btnImportMachines";
            this.btnImportMachines.Size = new System.Drawing.Size(100, 13);
            this.btnImportMachines.TabIndex = 20;
            this.btnImportMachines.TabStop = true;
            this.btnImportMachines.Text = "Import Machines";
            this.btnImportMachines.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnImportMachines_LinkClicked);
            // 
            // lnkPublishToSite
            // 
            this.lnkPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublishToSite.AutoSize = true;
            this.lnkPublishToSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublishToSite.Location = new System.Drawing.Point(911, 562);
            this.lnkPublishToSite.Name = "lnkPublishToSite";
            this.lnkPublishToSite.Size = new System.Drawing.Size(99, 13);
            this.lnkPublishToSite.TabIndex = 50;
            this.lnkPublishToSite.TabStop = true;
            this.lnkPublishToSite.Text = "Publish To Sites";
            this.lnkPublishToSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublishToSite_LinkClicked);
            // 
            // locationTypeDTOBindingSource
            // 
            this.locationTypeDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.LocationTypeDTO);
            // 
            // txtLocationName
            // 
            this.txtLocationName.Location = new System.Drawing.Point(308, 18);
            this.txtLocationName.Name = "txtLocationName";
            this.txtLocationName.Size = new System.Drawing.Size(108, 20);
            this.txtLocationName.TabIndex = 55;
            // 
            // lblLocationName
            // 
            this.lblLocationName.AutoSize = true;
            this.lblLocationName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblLocationName.Location = new System.Drawing.Point(207, 20);
            this.lblLocationName.Name = "lblLocationName";
            this.lblLocationName.Size = new System.Drawing.Size(95, 15);
            this.lblLocationName.TabIndex = 51;
            this.lblLocationName.Text = "Location Name:";
            // 
            // lblLocationType
            // 
            this.lblLocationType.AutoSize = true;
            this.lblLocationType.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblLocationType.Location = new System.Drawing.Point(449, 23);
            this.lblLocationType.Name = "lblLocationType";
            this.lblLocationType.Size = new System.Drawing.Size(88, 15);
            this.lblLocationType.TabIndex = 53;
            this.lblLocationType.Text = "Location Type:";
            // 
            // cbxActive
            // 
            this.cbxActive.AutoSize = true;
            this.cbxActive.Checked = true;
            this.cbxActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxActive.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cbxActive.Location = new System.Drawing.Point(18, 19);
            this.cbxActive.Name = "cbxActive";
            this.cbxActive.Size = new System.Drawing.Size(139, 19);
            this.cbxActive.TabIndex = 54;
            this.cbxActive.Text = "Show Active Entries";
            this.cbxActive.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(731, 19);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 23);
            this.btnSearch.TabIndex = 57;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbLocationType
            // 
            this.cbLocationType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbLocationType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbLocationType.FormattingEnabled = true;
            this.cbLocationType.Location = new System.Drawing.Point(543, 19);
            this.cbLocationType.Name = "cbLocationType";
            this.cbLocationType.Size = new System.Drawing.Size(121, 21);
            this.cbLocationType.TabIndex = 56;
            // 
            // filterGroupBox
            // 
            this.filterGroupBox.Controls.Add(this.cbxActive);
            this.filterGroupBox.Controls.Add(this.btnSearch);
            this.filterGroupBox.Controls.Add(this.cbLocationType);
            this.filterGroupBox.Controls.Add(this.lblLocationName);
            this.filterGroupBox.Controls.Add(this.txtLocationName);
            this.filterGroupBox.Controls.Add(this.lblLocationType);
            this.filterGroupBox.Location = new System.Drawing.Point(33, 12);
            this.filterGroupBox.Name = "filterGroupBox";
            this.filterGroupBox.Size = new System.Drawing.Size(1107, 47);
            this.filterGroupBox.TabIndex = 57;
            this.filterGroupBox.TabStop = false;
            this.filterGroupBox.Text = "Filter";
            // 
            // LocationUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 591);
            this.Controls.Add(this.filterGroupBox);
            this.Controls.Add(this.lnkPublishToSite);
            this.Controls.Add(this.btnImportMachines);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvLocation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "LocationUI";
            this.Text = "Location";
            this.Load += new System.EventHandler(this.LocationUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationTypeDTOBindingSource)).EndInit();
            this.filterGroupBox.ResumeLayout(false);
            this.filterGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvLocation;
        private System.Windows.Forms.LinkLabel btnImportMachines;
        private System.Windows.Forms.BindingSource locationTypeDTOBindingSource;
        private System.Windows.Forms.BindingSource locationDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn locationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn remarksMandatoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isAvailableToSellDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isStoreDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isTurnInLocationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn massUpdatedAllowedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn locationTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn barcodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn GenerateBarcode;
        private System.Windows.Forms.DataGridViewButtonColumn Custom;
        private System.Windows.Forms.DataGridViewTextBoxColumn customDataSetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn externalSystemReferenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.LinkLabel lnkPublishToSite;
        private System.Windows.Forms.TextBox txtLocationName;
        private System.Windows.Forms.Label lblLocationName;
        private System.Windows.Forms.Label lblLocationType;
        private System.Windows.Forms.CheckBox cbxActive;
        private System.Windows.Forms.Button btnSearch;
        private Core.GenericUtilities.AutoCompleteComboBox cbLocationType;
        private System.Windows.Forms.GroupBox filterGroupBox;
    }
}

