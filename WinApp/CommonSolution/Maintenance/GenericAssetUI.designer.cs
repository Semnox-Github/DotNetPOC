namespace Semnox.Parafait.Maintenance
{
    partial class GenericAssetUI
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
            this.genericAssetDataGridView = new System.Windows.Forms.DataGridView();
            this.genericAssetDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.assetDeleteBtn = new System.Windows.Forms.Button();
            this.assetCloseBtn = new System.Windows.Forms.Button();
            this.assetRefreshBtn = new System.Windows.Forms.Button();
            this.assetSaveBtn = new System.Windows.Forms.Button();
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.cmbAssetType = new System.Windows.Forms.ComboBox();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblAssetStatus = new System.Windows.Forms.Label();
            this.txtURN = new System.Windows.Forms.TextBox();
            this.lblURN = new System.Windows.Forms.Label();
            this.lblAssetType = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.txtAssetTypeSearch = new System.Windows.Forms.TextBox();
            this.dgvAssetTypeSearch = new System.Windows.Forms.DataGridView();
            this.dgvNameSearch = new System.Windows.Forms.DataGridView();
            this.btnImportMachine = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPublishToSite = new System.Windows.Forms.Button();
            this.assetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machineidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetTypeIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.locationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.uRNDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchaseDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saleDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scrapDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetTaxTypeIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.purchaseValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saleValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scrapValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.genericAssetDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.genericAssetDTOBindingSource)).BeginInit();
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssetTypeSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNameSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // genericAssetDataGridView
            // 
            this.genericAssetDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.genericAssetDataGridView.AutoGenerateColumns = false;
            this.genericAssetDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.genericAssetDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.assetIdDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.machineidDataGridViewTextBoxColumn,
            this.assetTypeIdDataGridViewComboBoxColumn,
            this.locationDataGridViewTextBoxColumn,
            this.assetStatusDataGridViewTextBoxColumn,
            this.uRNDataGridViewTextBoxColumn,
            this.purchaseDateDataGridViewTextBoxColumn,
            this.saleDateDataGridViewTextBoxColumn,
            this.scrapDateDataGridViewTextBoxColumn,
            this.assetTaxTypeIdDataGridViewComboBoxColumn,
            this.purchaseValueDataGridViewTextBoxColumn,
            this.saleValueDataGridViewTextBoxColumn,
            this.scrapValueDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.genericAssetDataGridView.DataSource = this.genericAssetDTOBindingSource;
            this.genericAssetDataGridView.Location = new System.Drawing.Point(12, 84);
            this.genericAssetDataGridView.Name = "genericAssetDataGridView";
            this.genericAssetDataGridView.Size = new System.Drawing.Size(1021, 180);
            this.genericAssetDataGridView.TabIndex = 0;
            this.genericAssetDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.genericAssetDataGridView_DataError);
            // 
            // genericAssetDTOBindingSource
            // 
            this.genericAssetDTOBindingSource.DataSource = typeof(Semnox.Parafait.Maintenance.GenericAssetDTO);
            // 
            // assetDeleteBtn
            // 
            this.assetDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetDeleteBtn.Location = new System.Drawing.Point(269, 279);
            this.assetDeleteBtn.Name = "assetDeleteBtn";
            this.assetDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.assetDeleteBtn.TabIndex = 16;
            this.assetDeleteBtn.Text = "Delete";
            this.assetDeleteBtn.UseVisualStyleBackColor = true;
            this.assetDeleteBtn.Click += new System.EventHandler(this.assetDeleteBtn_Click);
            // 
            // assetCloseBtn
            // 
            this.assetCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetCloseBtn.Location = new System.Drawing.Point(397, 279);
            this.assetCloseBtn.Name = "assetCloseBtn";
            this.assetCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.assetCloseBtn.TabIndex = 15;
            this.assetCloseBtn.Text = "Close";
            this.assetCloseBtn.UseVisualStyleBackColor = true;
            this.assetCloseBtn.Click += new System.EventHandler(this.assetCloseBtn_Click);
            // 
            // assetRefreshBtn
            // 
            this.assetRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetRefreshBtn.Location = new System.Drawing.Point(141, 279);
            this.assetRefreshBtn.Name = "assetRefreshBtn";
            this.assetRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.assetRefreshBtn.TabIndex = 14;
            this.assetRefreshBtn.Text = "Refresh";
            this.assetRefreshBtn.UseVisualStyleBackColor = true;
            this.assetRefreshBtn.Click += new System.EventHandler(this.assetRefreshBtn_Click);
            // 
            // assetSaveBtn
            // 
            this.assetSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assetSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.assetSaveBtn.Location = new System.Drawing.Point(13, 279);
            this.assetSaveBtn.Name = "assetSaveBtn";
            this.assetSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.assetSaveBtn.TabIndex = 13;
            this.assetSaveBtn.Text = "Save";
            this.assetSaveBtn.UseVisualStyleBackColor = true;
            this.assetSaveBtn.Click += new System.EventHandler(this.assetSaveBtn_Click);
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.cmbStatus);
            this.gpFilter.Controls.Add(this.cmbAssetType);
            this.gpFilter.Controls.Add(this.cmbLocation);
            this.gpFilter.Controls.Add(this.lblLocation);
            this.gpFilter.Controls.Add(this.lblAssetStatus);
            this.gpFilter.Controls.Add(this.txtURN);
            this.gpFilter.Controls.Add(this.lblURN);
            this.gpFilter.Controls.Add(this.lblAssetType);
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.txtName);
            this.gpFilter.Controls.Add(this.lblName);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(12, 2);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(1022, 77);
            this.gpFilter.TabIndex = 17;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(128, 47);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(136, 23);
            this.cmbStatus.TabIndex = 13;
            // 
            // cmbAssetType
            // 
            this.cmbAssetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssetType.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbAssetType.FormattingEnabled = true;
            this.cmbAssetType.Location = new System.Drawing.Point(128, 15);
            this.cmbAssetType.Name = "cmbAssetType";
            this.cmbAssetType.Size = new System.Drawing.Size(136, 23);
            this.cmbAssetType.TabIndex = 12;
            this.cmbAssetType.SelectedValueChanged += new System.EventHandler(this.cmbAssetType_SelectedValueChanged);
            // 
            // cmbLocation
            // 
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(375, 47);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(136, 23);
            this.cmbLocation.TabIndex = 11;
            // 
            // lblLocation
            // 
            this.lblLocation.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblLocation.Location = new System.Drawing.Point(277, 48);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(92, 20);
            this.lblLocation.TabIndex = 10;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAssetStatus
            // 
            this.lblAssetStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssetStatus.Location = new System.Drawing.Point(12, 48);
            this.lblAssetStatus.Name = "lblAssetStatus";
            this.lblAssetStatus.Size = new System.Drawing.Size(104, 20);
            this.lblAssetStatus.TabIndex = 8;
            this.lblAssetStatus.Text = "Asset Status:";
            this.lblAssetStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtURN
            // 
            this.txtURN.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtURN.Location = new System.Drawing.Point(603, 16);
            this.txtURN.Name = "txtURN";
            this.txtURN.Size = new System.Drawing.Size(136, 21);
            this.txtURN.TabIndex = 7;
            // 
            // lblURN
            // 
            this.lblURN.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblURN.Location = new System.Drawing.Point(521, 16);
            this.lblURN.Name = "lblURN";
            this.lblURN.Size = new System.Drawing.Size(76, 20);
            this.lblURN.TabIndex = 6;
            this.lblURN.Text = "URN:";
            this.lblURN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAssetType
            // 
            this.lblAssetType.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssetType.Location = new System.Drawing.Point(9, 16);
            this.lblAssetType.Name = "lblAssetType";
            this.lblAssetType.Size = new System.Drawing.Size(107, 20);
            this.lblAssetType.TabIndex = 4;
            this.lblAssetType.Text = "Asset Type:";
            this.lblAssetType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(759, 47);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(375, 16);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 2;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(280, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(89, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(603, 49);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 0;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // txtAssetTypeSearch
            // 
            this.txtAssetTypeSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.txtAssetTypeSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAssetTypeSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtAssetTypeSearch.Location = new System.Drawing.Point(143, 21);
            this.txtAssetTypeSearch.Name = "txtAssetTypeSearch";
            this.txtAssetTypeSearch.Size = new System.Drawing.Size(115, 14);
            this.txtAssetTypeSearch.TabIndex = 17;
            this.txtAssetTypeSearch.TextChanged += new System.EventHandler(this.txtAssetTypeSearch_TextChanged);
            this.txtAssetTypeSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAssetTypeSearch_KeyDown);
            // 
            // dgvAssetTypeSearch
            // 
            this.dgvAssetTypeSearch.AllowUserToAddRows = false;
            this.dgvAssetTypeSearch.AllowUserToDeleteRows = false;
            this.dgvAssetTypeSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvAssetTypeSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAssetTypeSearch.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvAssetTypeSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssetTypeSearch.ColumnHeadersVisible = false;
            this.dgvAssetTypeSearch.Location = new System.Drawing.Point(142, 40);
            this.dgvAssetTypeSearch.Name = "dgvAssetTypeSearch";
            this.dgvAssetTypeSearch.ReadOnly = true;
            this.dgvAssetTypeSearch.RowHeadersVisible = false;
            this.dgvAssetTypeSearch.RowTemplate.Height = 35;
            this.dgvAssetTypeSearch.Size = new System.Drawing.Size(134, 181);
            this.dgvAssetTypeSearch.TabIndex = 18;
            this.dgvAssetTypeSearch.Visible = false;
            this.dgvAssetTypeSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAssetTypeSearch_CellClick);
            this.dgvAssetTypeSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvAssetTypeSearch_KeyDown);
            // 
            // dgvNameSearch
            // 
            this.dgvNameSearch.AllowUserToAddRows = false;
            this.dgvNameSearch.AllowUserToDeleteRows = false;
            this.dgvNameSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvNameSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvNameSearch.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvNameSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNameSearch.ColumnHeadersVisible = false;
            this.dgvNameSearch.Location = new System.Drawing.Point(389, 40);
            this.dgvNameSearch.Name = "dgvNameSearch";
            this.dgvNameSearch.ReadOnly = true;
            this.dgvNameSearch.RowHeadersVisible = false;
            this.dgvNameSearch.RowTemplate.Height = 35;
            this.dgvNameSearch.Size = new System.Drawing.Size(134, 181);
            this.dgvNameSearch.TabIndex = 19;
            this.dgvNameSearch.Visible = false;
            this.dgvNameSearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNameSearch_CellClick);
            this.dgvNameSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvNameSearch_KeyDown);
            // 
            // btnImportMachine
            // 
            this.btnImportMachine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImportMachine.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnImportMachine.Location = new System.Drawing.Point(723, 279);
            this.btnImportMachine.Name = "btnImportMachine";
            this.btnImportMachine.Size = new System.Drawing.Size(127, 23);
            this.btnImportMachine.TabIndex = 20;
            this.btnImportMachine.Text = "Import Machines";
            this.btnImportMachine.UseVisualStyleBackColor = true;
            this.btnImportMachine.Click += new System.EventHandler(this.btnImportMachine_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(525, 279);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(121, 23);
            this.btnExport.TabIndex = 21;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPublishToSite
            // 
            this.btnPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPublishToSite.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPublishToSite.Location = new System.Drawing.Point(909, 279);
            this.btnPublishToSite.Name = "btnPublishToSite";
            this.btnPublishToSite.Size = new System.Drawing.Size(109, 23);
            this.btnPublishToSite.TabIndex = 22;
            this.btnPublishToSite.Text = "Publish To Sites";
            this.btnPublishToSite.UseVisualStyleBackColor = true;
            this.btnPublishToSite.Click += new System.EventHandler(this.btnPublishToSite_Click);
            // 
            // assetIdDataGridViewTextBoxColumn
            // 
            this.assetIdDataGridViewTextBoxColumn.DataPropertyName = "AssetId";
            this.assetIdDataGridViewTextBoxColumn.HeaderText = "Asset Id";
            this.assetIdDataGridViewTextBoxColumn.Name = "assetIdDataGridViewTextBoxColumn";
            this.assetIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Asset Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // machineidDataGridViewTextBoxColumn
            // 
            this.machineidDataGridViewTextBoxColumn.DataPropertyName = "Machineid";
            this.machineidDataGridViewTextBoxColumn.HeaderText = "Machine Id";
            this.machineidDataGridViewTextBoxColumn.Name = "machineidDataGridViewTextBoxColumn";
            this.machineidDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // assetTypeIdDataGridViewComboBoxColumn
            // 
            this.assetTypeIdDataGridViewComboBoxColumn.DataPropertyName = "AssetTypeId";
            this.assetTypeIdDataGridViewComboBoxColumn.HeaderText = "Asset Type";
            this.assetTypeIdDataGridViewComboBoxColumn.Name = "assetTypeIdDataGridViewComboBoxColumn";
            this.assetTypeIdDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assetTypeIdDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // locationDataGridViewTextBoxColumn
            // 
            this.locationDataGridViewTextBoxColumn.DataPropertyName = "Location";
            this.locationDataGridViewTextBoxColumn.HeaderText = "Location";
            this.locationDataGridViewTextBoxColumn.Name = "locationDataGridViewTextBoxColumn";
            // 
            // assetStatusDataGridViewTextBoxColumn
            // 
            this.assetStatusDataGridViewTextBoxColumn.DataPropertyName = "AssetStatus";
            this.assetStatusDataGridViewTextBoxColumn.HeaderText = "Asset Status";
            this.assetStatusDataGridViewTextBoxColumn.Name = "assetStatusDataGridViewTextBoxColumn";
            this.assetStatusDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assetStatusDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // uRNDataGridViewTextBoxColumn
            // 
            this.uRNDataGridViewTextBoxColumn.DataPropertyName = "URN";
            this.uRNDataGridViewTextBoxColumn.HeaderText = "URN";
            this.uRNDataGridViewTextBoxColumn.Name = "uRNDataGridViewTextBoxColumn";
            // 
            // purchaseDateDataGridViewTextBoxColumn
            // 
            this.purchaseDateDataGridViewTextBoxColumn.DataPropertyName = "PurchaseDate";
            this.purchaseDateDataGridViewTextBoxColumn.HeaderText = "Purchase Date";
            this.purchaseDateDataGridViewTextBoxColumn.Name = "purchaseDateDataGridViewTextBoxColumn";
            // 
            // saleDateDataGridViewTextBoxColumn
            // 
            this.saleDateDataGridViewTextBoxColumn.DataPropertyName = "SaleDate";
            this.saleDateDataGridViewTextBoxColumn.HeaderText = "Sale Date";
            this.saleDateDataGridViewTextBoxColumn.Name = "saleDateDataGridViewTextBoxColumn";
            // 
            // scrapDateDataGridViewTextBoxColumn
            // 
            this.scrapDateDataGridViewTextBoxColumn.DataPropertyName = "ScrapDate";
            this.scrapDateDataGridViewTextBoxColumn.HeaderText = "Scrap Date";
            this.scrapDateDataGridViewTextBoxColumn.Name = "scrapDateDataGridViewTextBoxColumn";
            // 
            // assetTaxTypeIdDataGridViewComboBoxColumn
            // 
            this.assetTaxTypeIdDataGridViewComboBoxColumn.DataPropertyName = "AssetTaxTypeId";
            this.assetTaxTypeIdDataGridViewComboBoxColumn.HeaderText = "Tax Type";
            this.assetTaxTypeIdDataGridViewComboBoxColumn.Name = "assetTaxTypeIdDataGridViewComboBoxColumn";
            this.assetTaxTypeIdDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assetTaxTypeIdDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // purchaseValueDataGridViewTextBoxColumn
            // 
            this.purchaseValueDataGridViewTextBoxColumn.DataPropertyName = "PurchaseValue";
            this.purchaseValueDataGridViewTextBoxColumn.HeaderText = "Purchase Value";
            this.purchaseValueDataGridViewTextBoxColumn.Name = "purchaseValueDataGridViewTextBoxColumn";
            // 
            // saleValueDataGridViewTextBoxColumn
            // 
            this.saleValueDataGridViewTextBoxColumn.DataPropertyName = "SaleValue";
            this.saleValueDataGridViewTextBoxColumn.HeaderText = "Sale Value";
            this.saleValueDataGridViewTextBoxColumn.Name = "saleValueDataGridViewTextBoxColumn";
            // 
            // scrapValueDataGridViewTextBoxColumn
            // 
            this.scrapValueDataGridViewTextBoxColumn.DataPropertyName = "ScrapValue";
            this.scrapValueDataGridViewTextBoxColumn.HeaderText = "Scrap Value";
            this.scrapValueDataGridViewTextBoxColumn.Name = "scrapValueDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "false";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // GenericAssetUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 319);
            this.Controls.Add(this.btnPublishToSite);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnImportMachine);
            this.Controls.Add(this.dgvNameSearch);
            this.Controls.Add(this.dgvAssetTypeSearch);
            this.Controls.Add(this.txtAssetTypeSearch);
            this.Controls.Add(this.gpFilter);
            this.Controls.Add(this.assetDeleteBtn);
            this.Controls.Add(this.assetCloseBtn);
            this.Controls.Add(this.assetRefreshBtn);
            this.Controls.Add(this.assetSaveBtn);
            this.Controls.Add(this.genericAssetDataGridView);
            this.Name = "GenericAssetUI";
            this.Text = "Generic Asset";
            this.Load += new System.EventHandler(this.GenericAssetUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.genericAssetDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.genericAssetDTOBindingSource)).EndInit();
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssetTypeSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNameSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView genericAssetDataGridView;
        private System.Windows.Forms.Button assetDeleteBtn;
        private System.Windows.Forms.Button assetCloseBtn;
        private System.Windows.Forms.Button assetRefreshBtn;
        private System.Windows.Forms.Button assetSaveBtn;
        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblAssetStatus;
        private System.Windows.Forms.TextBox txtURN;
        private System.Windows.Forms.Label lblURN;
        private System.Windows.Forms.Label lblAssetType;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.ComboBox cmbAssetType;
        private System.Windows.Forms.BindingSource genericAssetDTOBindingSource;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.TextBox txtAssetTypeSearch;
        private System.Windows.Forms.DataGridView dgvAssetTypeSearch;
        private System.Windows.Forms.DataGridView dgvNameSearch;
        private System.Windows.Forms.Button btnImportMachine;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPublishToSite;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn machineidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetTypeIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn locationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uRNDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchaseDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn saleDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scrapDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetTaxTypeIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchaseValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn saleValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scrapValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}