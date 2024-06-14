namespace Semnox.Parafait.Transaction
{
    partial class frmLockerManagementUI
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.tpZones = new System.Windows.Forms.TabPage();
            this.btnCardManagement = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panelTree = new System.Windows.Forms.Panel();
            this.tvCategory = new System.Windows.Forms.TreeView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvLockerZones = new System.Windows.Forms.DataGridView();
            this.zoneIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zoneNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parentZoneIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.zoneCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LockerMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LockerMake = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.activeFlagDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsZones = new System.Windows.Forms.BindingSource(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.tcLockers = new System.Windows.Forms.TabControl();
            this.tpPanels = new System.Windows.Forms.TabPage();
            this.btnDeletePanel = new System.Windows.Forms.Button();
            this.btnClosePanel = new System.Windows.Forms.Button();
            this.btnRefreshPanel = new System.Windows.Forms.Button();
            this.btnSavePanel = new System.Windows.Forms.Button();
            this.dgvPanels = new System.Windows.Forms.DataGridView();
            this.panelIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zoneIdPanelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.sequencePrefixDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numRowsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numColsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dcGoToPanelLockers = new System.Windows.Forms.DataGridViewButtonColumn();
            this.bsPanels = new System.Windows.Forms.BindingSource(this.components);
            this.tpLockers = new System.Windows.Forms.TabPage();
            this.btnSaveLockers = new System.Windows.Forms.Button();
            this.panelLockers = new System.Windows.Forms.Panel();
            this.tblLockers = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPanel = new System.Windows.Forms.ComboBox();
            this.tpZones.SuspendLayout();
            this.panelTree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLockerZones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsZones)).BeginInit();
            this.tcLockers.SuspendLayout();
            this.tpPanels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPanels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPanels)).BeginInit();
            this.tpLockers.SuspendLayout();
            this.panelLockers.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpZones
            // 
            this.tpZones.Controls.Add(this.btnCardManagement);
            this.tpZones.Controls.Add(this.btnDelete);
            this.tpZones.Controls.Add(this.panelTree);
            this.tpZones.Controls.Add(this.btnCancel);
            this.tpZones.Controls.Add(this.btnRefresh);
            this.tpZones.Controls.Add(this.dgvLockerZones);
            this.tpZones.Controls.Add(this.btnSave);
            this.tpZones.Location = new System.Drawing.Point(4, 34);
            this.tpZones.Name = "tpZones";
            this.tpZones.Padding = new System.Windows.Forms.Padding(3);
            this.tpZones.Size = new System.Drawing.Size(1030, 453);
            this.tpZones.TabIndex = 0;
            this.tpZones.Text = "Zones";
            this.tpZones.UseVisualStyleBackColor = true;
            // 
            // btnCardManagement
            // 
            this.btnCardManagement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCardManagement.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCardManagement.Location = new System.Drawing.Point(580, 417);
            this.btnCardManagement.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCardManagement.Name = "btnCardManagement";
            this.btnCardManagement.Size = new System.Drawing.Size(130, 23);
            this.btnCardManagement.TabIndex = 36;
            this.btnCardManagement.Text = "Card Management";
            this.btnCardManagement.UseVisualStyleBackColor = true;
            this.btnCardManagement.Click += new System.EventHandler(this.btnCardManagement_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.CausesValidation = false;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(297, 417);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 23);
            this.btnDelete.TabIndex = 34;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panelTree
            // 
            this.panelTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTree.Controls.Add(this.tvCategory);
            this.panelTree.Location = new System.Drawing.Point(730, 7);
            this.panelTree.Name = "panelTree";
            this.panelTree.Size = new System.Drawing.Size(298, 398);
            this.panelTree.TabIndex = 35;
            // 
            // tvCategory
            // 
            this.tvCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvCategory.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvCategory.Location = new System.Drawing.Point(0, 0);
            this.tvCategory.Name = "tvCategory";
            treeNode1.Name = "Node1";
            treeNode1.Text = "Node1";
            treeNode2.Name = "Node2";
            treeNode2.Text = "Node2";
            treeNode3.Name = "Node0";
            treeNode3.Text = "Node0";
            this.tvCategory.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.tvCategory.Size = new System.Drawing.Size(298, 398);
            this.tvCategory.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(441, 417);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 23);
            this.btnCancel.TabIndex = 33;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(153, 417);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 23);
            this.btnRefresh.TabIndex = 31;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dgvLockerZones
            // 
            this.dgvLockerZones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLockerZones.AutoGenerateColumns = false;
            this.dgvLockerZones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLockerZones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.zoneIdDataGridViewTextBoxColumn,
            this.zoneNameDataGridViewTextBoxColumn,
            this.parentZoneIdDataGridViewTextBoxColumn,
            this.zoneCodeDataGridViewTextBoxColumn,
            this.LockerMode,
            this.LockerMake,
            this.activeFlagDataGridViewCheckBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdatedDateDataGridViewTextBoxColumn});
            this.dgvLockerZones.DataSource = this.bsZones;
            this.dgvLockerZones.Location = new System.Drawing.Point(9, 7);
            this.dgvLockerZones.Name = "dgvLockerZones";
            this.dgvLockerZones.Size = new System.Drawing.Size(717, 398);
            this.dgvLockerZones.TabIndex = 0;
            this.dgvLockerZones.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvLockerZones_DataError);
            this.dgvLockerZones.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvLockerZones_DefaultValuesNeeded);
            this.dgvLockerZones.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLockerZones_RowEnter);
            // 
            // zoneIdDataGridViewTextBoxColumn
            // 
            this.zoneIdDataGridViewTextBoxColumn.DataPropertyName = "ZoneId";
            this.zoneIdDataGridViewTextBoxColumn.HeaderText = "Zone Id";
            this.zoneIdDataGridViewTextBoxColumn.Name = "zoneIdDataGridViewTextBoxColumn";
            this.zoneIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // zoneNameDataGridViewTextBoxColumn
            // 
            this.zoneNameDataGridViewTextBoxColumn.DataPropertyName = "ZoneName";
            this.zoneNameDataGridViewTextBoxColumn.HeaderText = "Zone Name";
            this.zoneNameDataGridViewTextBoxColumn.Name = "zoneNameDataGridViewTextBoxColumn";
            // 
            // parentZoneIdDataGridViewTextBoxColumn
            // 
            this.parentZoneIdDataGridViewTextBoxColumn.DataPropertyName = "ParentZoneId";
            this.parentZoneIdDataGridViewTextBoxColumn.HeaderText = "Parent Zone";
            this.parentZoneIdDataGridViewTextBoxColumn.Name = "parentZoneIdDataGridViewTextBoxColumn";
            this.parentZoneIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.parentZoneIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // zoneCodeDataGridViewTextBoxColumn
            // 
            this.zoneCodeDataGridViewTextBoxColumn.DataPropertyName = "ZoneCode";
            this.zoneCodeDataGridViewTextBoxColumn.HeaderText = "Zone Code";
            this.zoneCodeDataGridViewTextBoxColumn.Name = "zoneCodeDataGridViewTextBoxColumn";
            this.zoneCodeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.zoneCodeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // LockerMode
            // 
            this.LockerMode.DataPropertyName = "LockerMode";
            this.LockerMode.HeaderText = "LockerMode";
            this.LockerMode.Name = "LockerMode";
            // 
            // LockerMake
            // 
            this.LockerMake.DataPropertyName = "LockerMake";
            this.LockerMake.HeaderText = "LockerMake";
            this.LockerMake.Name = "LockerMake";
            // 
            // activeFlagDataGridViewCheckBoxColumn
            // 
            this.activeFlagDataGridViewCheckBoxColumn.DataPropertyName = "ActiveFlag";
            this.activeFlagDataGridViewCheckBoxColumn.HeaderText = "Active Flag";
            this.activeFlagDataGridViewCheckBoxColumn.Name = "activeFlagDataGridViewCheckBoxColumn";
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Last Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn.HeaderText = "Last Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn.Name = "lastUpdatedDateDataGridViewTextBoxColumn";
            this.lastUpdatedDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bsZones
            // 
            this.bsZones.DataSource = typeof(Semnox.Parafait.Device.Lockers.LockerZonesDTO);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(9, 417);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 32;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tcLockers
            // 
            this.tcLockers.Controls.Add(this.tpZones);
            this.tcLockers.Controls.Add(this.tpPanels);
            this.tcLockers.Controls.Add(this.tpLockers);
            this.tcLockers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcLockers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcLockers.ItemSize = new System.Drawing.Size(58, 30);
            this.tcLockers.Location = new System.Drawing.Point(0, 0);
            this.tcLockers.Name = "tcLockers";
            this.tcLockers.SelectedIndex = 0;
            this.tcLockers.Size = new System.Drawing.Size(1038, 491);
            this.tcLockers.TabIndex = 37;
            this.tcLockers.SelectedIndexChanged += new System.EventHandler(this.tcLockers_SelectedIndexChanged);
            // 
            // tpPanels
            // 
            this.tpPanels.Controls.Add(this.btnDeletePanel);
            this.tpPanels.Controls.Add(this.btnClosePanel);
            this.tpPanels.Controls.Add(this.btnRefreshPanel);
            this.tpPanels.Controls.Add(this.btnSavePanel);
            this.tpPanels.Controls.Add(this.dgvPanels);
            this.tpPanels.Location = new System.Drawing.Point(4, 34);
            this.tpPanels.Name = "tpPanels";
            this.tpPanels.Padding = new System.Windows.Forms.Padding(3);
            this.tpPanels.Size = new System.Drawing.Size(1030, 453);
            this.tpPanels.TabIndex = 1;
            this.tpPanels.Text = "Panels";
            this.tpPanels.UseVisualStyleBackColor = true;
            // 
            // btnDeletePanel
            // 
            this.btnDeletePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeletePanel.CausesValidation = false;
            this.btnDeletePanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeletePanel.Location = new System.Drawing.Point(302, 417);
            this.btnDeletePanel.Name = "btnDeletePanel";
            this.btnDeletePanel.Size = new System.Drawing.Size(90, 23);
            this.btnDeletePanel.TabIndex = 38;
            this.btnDeletePanel.Text = "Delete";
            this.btnDeletePanel.UseVisualStyleBackColor = true;
            this.btnDeletePanel.Click += new System.EventHandler(this.btnDeletePanel_Click);
            // 
            // btnClosePanel
            // 
            this.btnClosePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClosePanel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClosePanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClosePanel.Location = new System.Drawing.Point(441, 417);
            this.btnClosePanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClosePanel.Name = "btnClosePanel";
            this.btnClosePanel.Size = new System.Drawing.Size(90, 23);
            this.btnClosePanel.TabIndex = 37;
            this.btnClosePanel.Text = "Close";
            this.btnClosePanel.UseVisualStyleBackColor = true;
            this.btnClosePanel.Click += new System.EventHandler(this.btnClosePanel_Click);
            // 
            // btnRefreshPanel
            // 
            this.btnRefreshPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshPanel.Location = new System.Drawing.Point(155, 417);
            this.btnRefreshPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefreshPanel.Name = "btnRefreshPanel";
            this.btnRefreshPanel.Size = new System.Drawing.Size(90, 23);
            this.btnRefreshPanel.TabIndex = 35;
            this.btnRefreshPanel.Text = "Refresh";
            this.btnRefreshPanel.UseVisualStyleBackColor = true;
            this.btnRefreshPanel.Click += new System.EventHandler(this.btnRefreshPanel_Click);
            // 
            // btnSavePanel
            // 
            this.btnSavePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSavePanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSavePanel.Location = new System.Drawing.Point(4, 417);
            this.btnSavePanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSavePanel.Name = "btnSavePanel";
            this.btnSavePanel.Size = new System.Drawing.Size(90, 23);
            this.btnSavePanel.TabIndex = 36;
            this.btnSavePanel.Text = "Save";
            this.btnSavePanel.UseVisualStyleBackColor = true;
            this.btnSavePanel.Click += new System.EventHandler(this.btnSavePanel_Click);
            // 
            // dgvPanels
            // 
            this.dgvPanels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPanels.AutoGenerateColumns = false;
            this.dgvPanels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPanels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.panelIdDataGridViewTextBoxColumn,
            this.panelNameDataGridViewTextBoxColumn,
            this.zoneIdPanelDataGridViewTextBoxColumn,
            this.sequencePrefixDataGridViewTextBoxColumn,
            this.numRowsDataGridViewTextBoxColumn,
            this.numColsDataGridViewTextBoxColumn,
            this.IsActive,
            this.dcGoToPanelLockers});
            this.dgvPanels.DataSource = this.bsPanels;
            this.dgvPanels.Location = new System.Drawing.Point(9, 7);
            this.dgvPanels.Name = "dgvPanels";
            this.dgvPanels.Size = new System.Drawing.Size(1013, 392);
            this.dgvPanels.TabIndex = 0;
            this.dgvPanels.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPanels_CellClick);
            this.dgvPanels.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvLockerPanels_DataError);
            // 
            // panelIdDataGridViewTextBoxColumn
            // 
            this.panelIdDataGridViewTextBoxColumn.DataPropertyName = "PanelId";
            this.panelIdDataGridViewTextBoxColumn.HeaderText = "Panel Id";
            this.panelIdDataGridViewTextBoxColumn.Name = "panelIdDataGridViewTextBoxColumn";
            this.panelIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // panelNameDataGridViewTextBoxColumn
            // 
            this.panelNameDataGridViewTextBoxColumn.DataPropertyName = "PanelName";
            this.panelNameDataGridViewTextBoxColumn.HeaderText = "Panel Name";
            this.panelNameDataGridViewTextBoxColumn.Name = "panelNameDataGridViewTextBoxColumn";
            // 
            // zoneIdPanelDataGridViewTextBoxColumn
            // 
            this.zoneIdPanelDataGridViewTextBoxColumn.DataPropertyName = "ZoneId";
            this.zoneIdPanelDataGridViewTextBoxColumn.HeaderText = "Zone";
            this.zoneIdPanelDataGridViewTextBoxColumn.Name = "zoneIdPanelDataGridViewTextBoxColumn";
            this.zoneIdPanelDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.zoneIdPanelDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // sequencePrefixDataGridViewTextBoxColumn
            // 
            this.sequencePrefixDataGridViewTextBoxColumn.DataPropertyName = "SequencePrefix";
            this.sequencePrefixDataGridViewTextBoxColumn.HeaderText = "Sequence Prefix";
            this.sequencePrefixDataGridViewTextBoxColumn.Name = "sequencePrefixDataGridViewTextBoxColumn";
            // 
            // numRowsDataGridViewTextBoxColumn
            // 
            this.numRowsDataGridViewTextBoxColumn.DataPropertyName = "NumRows";
            this.numRowsDataGridViewTextBoxColumn.HeaderText = "Rows";
            this.numRowsDataGridViewTextBoxColumn.Name = "numRowsDataGridViewTextBoxColumn";
            // 
            // numColsDataGridViewTextBoxColumn
            // 
            this.numColsDataGridViewTextBoxColumn.DataPropertyName = "NumCols";
            this.numColsDataGridViewTextBoxColumn.HeaderText = "Columns";
            this.numColsDataGridViewTextBoxColumn.Name = "numColsDataGridViewTextBoxColumn";
            // 
            // IsActive
            // 
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.HeaderText = "IsActive";
            this.IsActive.Name = "IsActive";
            // 
            // dcGoToPanelLockers
            // 
            this.dcGoToPanelLockers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dcGoToPanelLockers.HeaderText = "Lockers";
            this.dcGoToPanelLockers.Name = "dcGoToPanelLockers";
            this.dcGoToPanelLockers.ReadOnly = true;
            this.dcGoToPanelLockers.Text = "....";
            this.dcGoToPanelLockers.Width = 50;
            // 
            // bsPanels
            // 
            this.bsPanels.DataSource = typeof(Semnox.Parafait.Device.Lockers.LockerPanelDTO);
            // 
            // tpLockers
            // 
            this.tpLockers.Controls.Add(this.btnSaveLockers);
            this.tpLockers.Controls.Add(this.panelLockers);
            this.tpLockers.Controls.Add(this.label1);
            this.tpLockers.Controls.Add(this.cmbPanel);
            this.tpLockers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tpLockers.Location = new System.Drawing.Point(4, 34);
            this.tpLockers.Name = "tpLockers";
            this.tpLockers.Padding = new System.Windows.Forms.Padding(3);
            this.tpLockers.Size = new System.Drawing.Size(1030, 453);
            this.tpLockers.TabIndex = 2;
            this.tpLockers.Text = "Lockers";
            this.tpLockers.UseVisualStyleBackColor = true;
            // 
            // btnSaveLockers
            // 
            this.btnSaveLockers.Location = new System.Drawing.Point(227, 6);
            this.btnSaveLockers.Name = "btnSaveLockers";
            this.btnSaveLockers.Size = new System.Drawing.Size(83, 23);
            this.btnSaveLockers.TabIndex = 3;
            this.btnSaveLockers.Text = "Save";
            this.btnSaveLockers.UseVisualStyleBackColor = true;
            this.btnSaveLockers.Click += new System.EventHandler(this.btnSaveLockers_Click);
            // 
            // panelLockers
            // 
            this.panelLockers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLockers.AutoScroll = true;
            this.panelLockers.Controls.Add(this.tblLockers);
            this.panelLockers.Location = new System.Drawing.Point(0, 36);
            this.panelLockers.Name = "panelLockers";
            this.panelLockers.Size = new System.Drawing.Size(1030, 417);
            this.panelLockers.TabIndex = 2;
            // 
            // tblLockers
            // 
            this.tblLockers.AutoSize = true;
            this.tblLockers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblLockers.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tblLockers.ColumnCount = 1;
            this.tblLockers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tblLockers.Location = new System.Drawing.Point(3, 3);
            this.tblLockers.Name = "tblLockers";
            this.tblLockers.RowCount = 1;
            this.tblLockers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tblLockers.Size = new System.Drawing.Size(171, 152);
            this.tblLockers.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Panel:";
            // 
            // cmbPanel
            // 
            this.cmbPanel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPanel.FormattingEnabled = true;
            this.cmbPanel.Location = new System.Drawing.Point(71, 6);
            this.cmbPanel.Name = "cmbPanel";
            this.cmbPanel.Size = new System.Drawing.Size(132, 24);
            this.cmbPanel.TabIndex = 0;
            this.cmbPanel.SelectedIndexChanged += new System.EventHandler(this.cmbPanel_SelectedIndexChanged);
            // 
            // frmLockerManagementUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 491);
            this.Controls.Add(this.tcLockers);
            this.Name = "frmLockerManagementUI";
            this.Text = "Locker Setup";
            this.Load += new System.EventHandler(this.frmLockerSetupUI_Load);
            this.tpZones.ResumeLayout(false);
            this.panelTree.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLockerZones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsZones)).EndInit();
            this.tcLockers.ResumeLayout(false);
            this.tpPanels.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPanels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPanels)).EndInit();
            this.tpLockers.ResumeLayout(false);
            this.tpLockers.PerformLayout();
            this.panelLockers.ResumeLayout(false);
            this.panelLockers.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tpZones;
        private System.Windows.Forms.Button btnCardManagement;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel panelTree;
        private System.Windows.Forms.TreeView tvCategory;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvLockerZones;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabControl tcLockers;
        private System.Windows.Forms.TabPage tpPanels;
        private System.Windows.Forms.Button btnDeletePanel;
        private System.Windows.Forms.Button btnClosePanel;
        private System.Windows.Forms.Button btnRefreshPanel;
        private System.Windows.Forms.Button btnSavePanel;
        private System.Windows.Forms.DataGridView dgvPanels;
        private System.Windows.Forms.TabPage tpLockers;
        private System.Windows.Forms.Button btnSaveLockers;
        private System.Windows.Forms.Panel panelLockers;
        private System.Windows.Forms.TableLayoutPanel tblLockers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPanel;
        private System.Windows.Forms.BindingSource bsZones;
        private System.Windows.Forms.BindingSource bsPanels;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeFlagDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByPanelsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn panelIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn panelNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn zoneIdPanelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sequencePrefixDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numRowsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numColsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActive;
        private System.Windows.Forms.DataGridViewButtonColumn dcGoToPanelLockers;
        private System.Windows.Forms.DataGridViewTextBoxColumn zoneIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn zoneNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn parentZoneIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn zoneCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn LockerMode;
        private System.Windows.Forms.DataGridViewComboBoxColumn LockerMake;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeFlagDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
    }
}

