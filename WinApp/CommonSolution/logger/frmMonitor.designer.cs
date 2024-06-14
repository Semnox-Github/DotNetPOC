namespace Semnox.Parafait.logger
{
    partial class frmMonitor
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Sites");
            this.dgvMonitor = new System.Windows.Forms.DataGridView();
            this.monitorIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.monitorAssetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.monitorDataSet = new Parafait.DataSet.monitorDataSet(); // onitorDataSet();
            this.monitorTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.monitorTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.applicationIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.monitorApplicationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.appModuleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.monitorAppModuleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PriorityId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.monitorPriorityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.intervalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.monitorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            //this.monitorTableAdapter = new Parafait.Tools.Monitor.monitorDataSetTableAdapters.MonitorTableAdapter();
            //this.monitorAssetTableAdapter = new Parafait.Tools.Monitor.monitorDataSetTableAdapters.MonitorAssetTableAdapter();
            //this.monitorTypeTableAdapter = new Parafait.Tools.Monitor.monitorDataSetTableAdapters.MonitorTypeTableAdapter();
            //this.monitorApplicationTableAdapter = new Parafait.Tools.Monitor.monitorDataSetTableAdapters.MonitorApplicationTableAdapter();
            //this.monitorAppModuleTableAdapter = new Parafait.Tools.Monitor.monitorDataSetTableAdapters.MonitorAppModuleTableAdapter();

            this.monitorTableAdapter = new Parafait.DataSet.monitorDataSetTableAdapters.MonitorTableAdapter();
            this.monitorAssetTableAdapter = new Parafait.DataSet.monitorDataSetTableAdapters.MonitorAssetTableAdapter();
            this.monitorTypeTableAdapter = new Parafait.DataSet.monitorDataSetTableAdapters.MonitorTypeTableAdapter();
            this.monitorApplicationTableAdapter = new Parafait.DataSet.monitorDataSetTableAdapters.MonitorApplicationTableAdapter();
            this.monitorAppModuleTableAdapter = new Parafait.DataSet.monitorDataSetTableAdapters.MonitorAppModuleTableAdapter();



            this.tcMonitor = new System.Windows.Forms.TabControl();
            this.tpDashboard = new System.Windows.Forms.TabPage();
            this.splitContainerMonitorLog = new System.Windows.Forms.SplitContainer();
            this.lnkRefresh = new System.Windows.Forms.LinkLabel();
            this.tvSites = new System.Windows.Forms.TreeView();
            this.lnkOpenLog = new System.Windows.Forms.LinkLabel();
            this.lblSiteName = new System.Windows.Forms.Label();
            this.lblMonitorSite = new System.Windows.Forms.Label();
            this.dgvMonitorLog = new System.Windows.Forms.DataGridView();
            this.tpAsset = new System.Windows.Forms.TabPage();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvMonitorAsset = new System.Windows.Forms.DataGridView();
            this.assetIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.monitorAssetTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.hostnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iPAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.macAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteidDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guidDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.monitorAssetBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tpMonitor = new System.Windows.Forms.TabPage();
            this.lnkPublish = new System.Windows.Forms.LinkLabel();
            this.btnViewLog = new System.Windows.Forms.Button();
            this.btnDeleteMonitor = new System.Windows.Forms.Button();
            this.btnCloseMonitor = new System.Windows.Forms.Button();
            this.btnRefreshMonitor = new System.Windows.Forms.Button();
            this.btnSaveMonitor = new System.Windows.Forms.Button();
            this.tpMasters = new System.Windows.Forms.TabPage();
            this.lnkPublishPriority = new System.Windows.Forms.LinkLabel();
            this.btnDeleteMaserData = new System.Windows.Forms.Button();
            this.btnCloseMasterData = new System.Windows.Forms.Button();
            this.btnRefreshMasterData = new System.Windows.Forms.Button();
            this.btnSaveMasterData = new System.Windows.Forms.Button();
            this.dgvMonitorPriority = new System.Windows.Forms.DataGridView();
            this.priorityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteidDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guidDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.monitorPriorityBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.monitorAssetTypeTableAdapter = new Parafait.Tools.Monitor.monitorDataSetTableAdapters.MonitorAssetTypeTableAdapter();
            this.monitorPriorityTableAdapter = new Parafait.Tools.Monitor.monitorDataSetTableAdapters.MonitorPriorityTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonitor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorAssetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorApplicationBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorAppModuleBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorPriorityBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorBindingSource)).BeginInit();
            this.tcMonitor.SuspendLayout();
            this.tpDashboard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMonitorLog)).BeginInit();
            this.splitContainerMonitorLog.Panel1.SuspendLayout();
            this.splitContainerMonitorLog.Panel2.SuspendLayout();
            this.splitContainerMonitorLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonitorLog)).BeginInit();
            this.tpAsset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonitorAsset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorAssetTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorAssetBindingSource1)).BeginInit();
            this.tpMonitor.SuspendLayout();
            this.tpMasters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonitorPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorPriorityBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMonitor
            // 
            this.dgvMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMonitor.AutoGenerateColumns = false;
            this.dgvMonitor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMonitor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.monitorIdDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.assetIdDataGridViewTextBoxColumn,
            this.monitorTypeIdDataGridViewTextBoxColumn,
            this.applicationIdDataGridViewTextBoxColumn,
            this.appModuleIdDataGridViewTextBoxColumn,
            this.PriorityId,
            this.intervalDataGridViewTextBoxColumn,
            this.activeDataGridViewCheckBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdatedDateDataGridViewTextBoxColumn,
            this.siteidDataGridViewTextBoxColumn,
            this.guidDataGridViewTextBoxColumn,
            this.synchStatusDataGridViewCheckBoxColumn});
            this.dgvMonitor.DataSource = this.monitorBindingSource;
            this.dgvMonitor.Location = new System.Drawing.Point(7, 7);
            this.dgvMonitor.Name = "dgvMonitor";
            this.dgvMonitor.Size = new System.Drawing.Size(830, 273);
            this.dgvMonitor.TabIndex = 0;
            this.dgvMonitor.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvMonitor_DataError);
            this.dgvMonitor.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvMonitor_DefaultValuesNeeded);
            // 
            // monitorIdDataGridViewTextBoxColumn
            // 
            this.monitorIdDataGridViewTextBoxColumn.DataPropertyName = "MonitorId";
            this.monitorIdDataGridViewTextBoxColumn.HeaderText = "Monitor Id";
            this.monitorIdDataGridViewTextBoxColumn.Name = "monitorIdDataGridViewTextBoxColumn";
            this.monitorIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // assetIdDataGridViewTextBoxColumn
            // 
            this.assetIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.assetIdDataGridViewTextBoxColumn.DataPropertyName = "AssetId";
            this.assetIdDataGridViewTextBoxColumn.DataSource = this.monitorAssetBindingSource;
            this.assetIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            this.assetIdDataGridViewTextBoxColumn.HeaderText = "Asset Name";
            this.assetIdDataGridViewTextBoxColumn.Name = "assetIdDataGridViewTextBoxColumn";
            this.assetIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assetIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.assetIdDataGridViewTextBoxColumn.ValueMember = "AssetId";
            this.assetIdDataGridViewTextBoxColumn.Width = 125;
            // 
            // monitorAssetBindingSource
            // 
            this.monitorAssetBindingSource.DataMember = "MonitorAsset";
            this.monitorAssetBindingSource.DataSource = this.monitorDataSet;
            // 
            // monitorDataSet
            // 
            this.monitorDataSet.DataSetName = "monitorDataSet";
            this.monitorDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // monitorTypeIdDataGridViewTextBoxColumn
            // 
            this.monitorTypeIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.monitorTypeIdDataGridViewTextBoxColumn.DataPropertyName = "MonitorTypeId";
            this.monitorTypeIdDataGridViewTextBoxColumn.DataSource = this.monitorTypeBindingSource;
            this.monitorTypeIdDataGridViewTextBoxColumn.DisplayMember = "MonitorType";
            this.monitorTypeIdDataGridViewTextBoxColumn.HeaderText = "Monitor Type";
            this.monitorTypeIdDataGridViewTextBoxColumn.Name = "monitorTypeIdDataGridViewTextBoxColumn";
            this.monitorTypeIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.monitorTypeIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.monitorTypeIdDataGridViewTextBoxColumn.ValueMember = "MonitorTypeId";
            // 
            // monitorTypeBindingSource
            // 
            this.monitorTypeBindingSource.DataMember = "MonitorType";
            this.monitorTypeBindingSource.DataSource = this.monitorDataSet;
            // 
            // applicationIdDataGridViewTextBoxColumn
            // 
            this.applicationIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.applicationIdDataGridViewTextBoxColumn.DataPropertyName = "ApplicationId";
            this.applicationIdDataGridViewTextBoxColumn.DataSource = this.monitorApplicationBindingSource;
            this.applicationIdDataGridViewTextBoxColumn.DisplayMember = "ApplicationName";
            this.applicationIdDataGridViewTextBoxColumn.HeaderText = "Application";
            this.applicationIdDataGridViewTextBoxColumn.Name = "applicationIdDataGridViewTextBoxColumn";
            this.applicationIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.applicationIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.applicationIdDataGridViewTextBoxColumn.ValueMember = "ApplicationId";
            this.applicationIdDataGridViewTextBoxColumn.Width = 150;
            // 
            // monitorApplicationBindingSource
            // 
            this.monitorApplicationBindingSource.DataMember = "MonitorApplication";
            this.monitorApplicationBindingSource.DataSource = this.monitorDataSet;
            // 
            // appModuleIdDataGridViewTextBoxColumn
            // 
            this.appModuleIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.appModuleIdDataGridViewTextBoxColumn.DataPropertyName = "AppModuleId";
            this.appModuleIdDataGridViewTextBoxColumn.DataSource = this.monitorAppModuleBindingSource;
            this.appModuleIdDataGridViewTextBoxColumn.DisplayMember = "ModuleName";
            this.appModuleIdDataGridViewTextBoxColumn.HeaderText = "App. Module";
            this.appModuleIdDataGridViewTextBoxColumn.Name = "appModuleIdDataGridViewTextBoxColumn";
            this.appModuleIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.appModuleIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.appModuleIdDataGridViewTextBoxColumn.ValueMember = "ModuleId";
            this.appModuleIdDataGridViewTextBoxColumn.Width = 120;
            // 
            // monitorAppModuleBindingSource
            // 
            this.monitorAppModuleBindingSource.DataMember = "MonitorAppModule";
            this.monitorAppModuleBindingSource.DataSource = this.monitorDataSet;
            // 
            // PriorityId
            // 
            this.PriorityId.DataPropertyName = "PriorityId";
            this.PriorityId.DataSource = this.monitorPriorityBindingSource;
            this.PriorityId.DisplayMember = "Name";
            this.PriorityId.HeaderText = "Priority";
            this.PriorityId.Name = "PriorityId";
            this.PriorityId.ValueMember = "PriorityId";
            // 
            // monitorPriorityBindingSource
            // 
            this.monitorPriorityBindingSource.DataMember = "MonitorPriority";
            this.monitorPriorityBindingSource.DataSource = this.monitorDataSet;
            // 
            // intervalDataGridViewTextBoxColumn
            // 
            this.intervalDataGridViewTextBoxColumn.DataPropertyName = "Interval";
            this.intervalDataGridViewTextBoxColumn.HeaderText = "Interval (Mins)";
            this.intervalDataGridViewTextBoxColumn.Name = "intervalDataGridViewTextBoxColumn";
            // 
            // activeDataGridViewCheckBoxColumn
            // 
            this.activeDataGridViewCheckBoxColumn.DataPropertyName = "Active";
            this.activeDataGridViewCheckBoxColumn.FalseValue = "false";
            this.activeDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.activeDataGridViewCheckBoxColumn.Name = "activeDataGridViewCheckBoxColumn";
            this.activeDataGridViewCheckBoxColumn.TrueValue = "true";
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
            // siteidDataGridViewTextBoxColumn
            // 
            this.siteidDataGridViewTextBoxColumn.DataPropertyName = "site_id";
            this.siteidDataGridViewTextBoxColumn.HeaderText = "site_id";
            this.siteidDataGridViewTextBoxColumn.Name = "siteidDataGridViewTextBoxColumn";
            this.siteidDataGridViewTextBoxColumn.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn
            // 
            this.guidDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn.Name = "guidDataGridViewTextBoxColumn";
            this.guidDataGridViewTextBoxColumn.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn
            // 
            this.synchStatusDataGridViewCheckBoxColumn.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.Name = "synchStatusDataGridViewCheckBoxColumn";
            this.synchStatusDataGridViewCheckBoxColumn.Visible = false;
            // 
            // monitorBindingSource
            // 
            this.monitorBindingSource.DataMember = "Monitor";
            this.monitorBindingSource.DataSource = this.monitorDataSet;
            // 
            // monitorTableAdapter
            // 
            this.monitorTableAdapter.ClearBeforeFill = true;
            // 
            // monitorAssetTableAdapter
            // 
            this.monitorAssetTableAdapter.ClearBeforeFill = true;
            // 
            // monitorTypeTableAdapter
            // 
            this.monitorTypeTableAdapter.ClearBeforeFill = true;
            // 
            // monitorApplicationTableAdapter
            // 
            this.monitorApplicationTableAdapter.ClearBeforeFill = true;
            // 
            // monitorAppModuleTableAdapter
            // 
            this.monitorAppModuleTableAdapter.ClearBeforeFill = true;
            // 
            // tcMonitor
            // 
            this.tcMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMonitor.Controls.Add(this.tpDashboard);
            this.tcMonitor.Controls.Add(this.tpAsset);
            this.tcMonitor.Controls.Add(this.tpMonitor);
            this.tcMonitor.Controls.Add(this.tpMasters);
            this.tcMonitor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcMonitor.Location = new System.Drawing.Point(4, 3);
            this.tcMonitor.Name = "tcMonitor";
            this.tcMonitor.SelectedIndex = 0;
            this.tcMonitor.Size = new System.Drawing.Size(848, 350);
            this.tcMonitor.TabIndex = 1;
            this.tcMonitor.SelectedIndexChanged += new System.EventHandler(this.tcMonitor_SelectedIndexChanged);
            // 
            // tpDashboard
            // 
            this.tpDashboard.Controls.Add(this.splitContainerMonitorLog);
            this.tpDashboard.Location = new System.Drawing.Point(4, 25);
            this.tpDashboard.Name = "tpDashboard";
            this.tpDashboard.Padding = new System.Windows.Forms.Padding(3);
            this.tpDashboard.Size = new System.Drawing.Size(840, 321);
            this.tpDashboard.TabIndex = 2;
            this.tpDashboard.Text = "Dashboard";
            this.tpDashboard.UseVisualStyleBackColor = true;
            // 
            // splitContainerMonitorLog
            // 
            this.splitContainerMonitorLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMonitorLog.Location = new System.Drawing.Point(3, 3);
            this.splitContainerMonitorLog.Name = "splitContainerMonitorLog";
            // 
            // splitContainerMonitorLog.Panel1
            // 
            this.splitContainerMonitorLog.Panel1.Controls.Add(this.lnkRefresh);
            this.splitContainerMonitorLog.Panel1.Controls.Add(this.tvSites);
            // 
            // splitContainerMonitorLog.Panel2
            // 
            this.splitContainerMonitorLog.Panel2.Controls.Add(this.lnkOpenLog);
            this.splitContainerMonitorLog.Panel2.Controls.Add(this.lblSiteName);
            this.splitContainerMonitorLog.Panel2.Controls.Add(this.lblMonitorSite);
            this.splitContainerMonitorLog.Panel2.Controls.Add(this.dgvMonitorLog);
            this.splitContainerMonitorLog.Size = new System.Drawing.Size(834, 315);
            this.splitContainerMonitorLog.SplitterDistance = 193;
            this.splitContainerMonitorLog.TabIndex = 0;
            // 
            // lnkRefresh
            // 
            this.lnkRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkRefresh.AutoSize = true;
            this.lnkRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkRefresh.Location = new System.Drawing.Point(1, 299);
            this.lnkRefresh.Name = "lnkRefresh";
            this.lnkRefresh.Size = new System.Drawing.Size(55, 16);
            this.lnkRefresh.TabIndex = 1;
            this.lnkRefresh.TabStop = true;
            this.lnkRefresh.Text = "Refresh";
            this.lnkRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRefresh_LinkClicked);
            // 
            // tvSites
            // 
            this.tvSites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvSites.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvSites.HideSelection = false;
            this.tvSites.Location = new System.Drawing.Point(0, 0);
            this.tvSites.Name = "tvSites";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Sites";
            this.tvSites.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tvSites.Size = new System.Drawing.Size(190, 296);
            this.tvSites.TabIndex = 0;
            this.tvSites.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSites_AfterSelect);
            // 
            // lnkOpenLog
            // 
            this.lnkOpenLog.AutoSize = true;
            this.lnkOpenLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkOpenLog.Location = new System.Drawing.Point(83, 5);
            this.lnkOpenLog.Name = "lnkOpenLog";
            this.lnkOpenLog.Size = new System.Drawing.Size(67, 16);
            this.lnkOpenLog.TabIndex = 2;
            this.lnkOpenLog.TabStop = true;
            this.lnkOpenLog.Text = "Open Log";
            this.lnkOpenLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOpenLog_LinkClicked);
            // 
            // lblSiteName
            // 
            this.lblSiteName.AutoSize = true;
            this.lblSiteName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.Location = new System.Drawing.Point(559, 5);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(75, 16);
            this.lblSiteName.TabIndex = 2;
            this.lblSiteName.Text = "site name";
            this.lblSiteName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMonitorSite
            // 
            this.lblMonitorSite.AutoSize = true;
            this.lblMonitorSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonitorSite.Location = new System.Drawing.Point(3, 5);
            this.lblMonitorSite.Name = "lblMonitorSite";
            this.lblMonitorSite.Size = new System.Drawing.Size(59, 16);
            this.lblMonitorSite.TabIndex = 1;
            this.lblMonitorSite.Text = "Monitors";
            // 
            // dgvMonitorLog
            // 
            this.dgvMonitorLog.AllowUserToAddRows = false;
            this.dgvMonitorLog.AllowUserToDeleteRows = false;
            this.dgvMonitorLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMonitorLog.BackgroundColor = System.Drawing.Color.White;
            this.dgvMonitorLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMonitorLog.Location = new System.Drawing.Point(0, 24);
            this.dgvMonitorLog.Name = "dgvMonitorLog";
            this.dgvMonitorLog.ReadOnly = true;
            this.dgvMonitorLog.Size = new System.Drawing.Size(637, 291);
            this.dgvMonitorLog.TabIndex = 0;
            // 
            // tpAsset
            // 
            this.tpAsset.Controls.Add(this.btnDelete);
            this.tpAsset.Controls.Add(this.btnCancel);
            this.tpAsset.Controls.Add(this.btnRefresh);
            this.tpAsset.Controls.Add(this.btnSave);
            this.tpAsset.Controls.Add(this.dgvMonitorAsset);
            this.tpAsset.Location = new System.Drawing.Point(4, 25);
            this.tpAsset.Name = "tpAsset";
            this.tpAsset.Padding = new System.Windows.Forms.Padding(3);
            this.tpAsset.Size = new System.Drawing.Size(840, 321);
            this.tpAsset.TabIndex = 1;
            this.tpAsset.Text = "Assets";
            this.tpAsset.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.CausesValidation = false;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(266, 287);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 23);
            this.btnDelete.TabIndex = 38;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(395, 287);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 23);
            this.btnCancel.TabIndex = 37;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(137, 287);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 23);
            this.btnRefresh.TabIndex = 35;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(8, 287);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 36;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvMonitorAsset
            // 
            this.dgvMonitorAsset.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMonitorAsset.AutoGenerateColumns = false;
            this.dgvMonitorAsset.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMonitorAsset.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.assetIdDataGridViewTextBoxColumn1,
            this.nameDataGridViewTextBoxColumn1,
            this.assetTypeIdDataGridViewTextBoxColumn,
            this.hostnameDataGridViewTextBoxColumn,
            this.iPAddressDataGridViewTextBoxColumn,
            this.macAddressDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn1,
            this.lastUpdatedDateDataGridViewTextBoxColumn1,
            this.siteidDataGridViewTextBoxColumn1,
            this.guidDataGridViewTextBoxColumn1,
            this.synchStatusDataGridViewCheckBoxColumn1});
            this.dgvMonitorAsset.DataSource = this.monitorAssetBindingSource1;
            this.dgvMonitorAsset.Location = new System.Drawing.Point(7, 7);
            this.dgvMonitorAsset.Name = "dgvMonitorAsset";
            this.dgvMonitorAsset.Size = new System.Drawing.Size(727, 268);
            this.dgvMonitorAsset.TabIndex = 0;
            this.dgvMonitorAsset.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvMonitorAsset_DataError);
            // 
            // assetIdDataGridViewTextBoxColumn1
            // 
            this.assetIdDataGridViewTextBoxColumn1.DataPropertyName = "AssetId";
            this.assetIdDataGridViewTextBoxColumn1.HeaderText = "Asset Id";
            this.assetIdDataGridViewTextBoxColumn1.Name = "assetIdDataGridViewTextBoxColumn1";
            this.assetIdDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn1
            // 
            this.nameDataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn1.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn1.Name = "nameDataGridViewTextBoxColumn1";
            // 
            // assetTypeIdDataGridViewTextBoxColumn
            // 
            this.assetTypeIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.assetTypeIdDataGridViewTextBoxColumn.DataPropertyName = "AssetTypeId";
            this.assetTypeIdDataGridViewTextBoxColumn.DataSource = this.monitorAssetTypeBindingSource;
            this.assetTypeIdDataGridViewTextBoxColumn.DisplayMember = "AssetType";
            this.assetTypeIdDataGridViewTextBoxColumn.HeaderText = "Asset Type";
            this.assetTypeIdDataGridViewTextBoxColumn.Name = "assetTypeIdDataGridViewTextBoxColumn";
            this.assetTypeIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assetTypeIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.assetTypeIdDataGridViewTextBoxColumn.ValueMember = "AssetTypeId";
            this.assetTypeIdDataGridViewTextBoxColumn.Width = 150;
            // 
            // monitorAssetTypeBindingSource
            // 
            this.monitorAssetTypeBindingSource.DataMember = "MonitorAssetType";
            this.monitorAssetTypeBindingSource.DataSource = this.monitorDataSet;
            // 
            // hostnameDataGridViewTextBoxColumn
            // 
            this.hostnameDataGridViewTextBoxColumn.DataPropertyName = "Hostname";
            this.hostnameDataGridViewTextBoxColumn.HeaderText = "Hostname";
            this.hostnameDataGridViewTextBoxColumn.Name = "hostnameDataGridViewTextBoxColumn";
            // 
            // iPAddressDataGridViewTextBoxColumn
            // 
            this.iPAddressDataGridViewTextBoxColumn.DataPropertyName = "IPAddress";
            this.iPAddressDataGridViewTextBoxColumn.HeaderText = "IP Address";
            this.iPAddressDataGridViewTextBoxColumn.Name = "iPAddressDataGridViewTextBoxColumn";
            // 
            // macAddressDataGridViewTextBoxColumn
            // 
            this.macAddressDataGridViewTextBoxColumn.DataPropertyName = "MacAddress";
            this.macAddressDataGridViewTextBoxColumn.HeaderText = "Mac Address";
            this.macAddressDataGridViewTextBoxColumn.Name = "macAddressDataGridViewTextBoxColumn";
            // 
            // lastUpdatedByDataGridViewTextBoxColumn1
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn1.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn1.HeaderText = "Last Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn1.Name = "lastUpdatedByDataGridViewTextBoxColumn1";
            this.lastUpdatedByDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn1
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn1.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn1.HeaderText = "Last Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn1.Name = "lastUpdatedDateDataGridViewTextBoxColumn1";
            this.lastUpdatedDateDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // siteidDataGridViewTextBoxColumn1
            // 
            this.siteidDataGridViewTextBoxColumn1.DataPropertyName = "site_id";
            this.siteidDataGridViewTextBoxColumn1.HeaderText = "site_id";
            this.siteidDataGridViewTextBoxColumn1.Name = "siteidDataGridViewTextBoxColumn1";
            this.siteidDataGridViewTextBoxColumn1.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn1
            // 
            this.guidDataGridViewTextBoxColumn1.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn1.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn1.Name = "guidDataGridViewTextBoxColumn1";
            this.guidDataGridViewTextBoxColumn1.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn1
            // 
            this.synchStatusDataGridViewCheckBoxColumn1.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn1.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn1.Name = "synchStatusDataGridViewCheckBoxColumn1";
            this.synchStatusDataGridViewCheckBoxColumn1.Visible = false;
            // 
            // monitorAssetBindingSource1
            // 
            this.monitorAssetBindingSource1.DataMember = "MonitorAsset";
            this.monitorAssetBindingSource1.DataSource = this.monitorDataSet;
            // 
            // tpMonitor
            // 
            this.tpMonitor.Controls.Add(this.lnkPublish);
            this.tpMonitor.Controls.Add(this.btnViewLog);
            this.tpMonitor.Controls.Add(this.btnDeleteMonitor);
            this.tpMonitor.Controls.Add(this.btnCloseMonitor);
            this.tpMonitor.Controls.Add(this.btnRefreshMonitor);
            this.tpMonitor.Controls.Add(this.btnSaveMonitor);
            this.tpMonitor.Controls.Add(this.dgvMonitor);
            this.tpMonitor.Location = new System.Drawing.Point(4, 25);
            this.tpMonitor.Name = "tpMonitor";
            this.tpMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.tpMonitor.Size = new System.Drawing.Size(840, 321);
            this.tpMonitor.TabIndex = 0;
            this.tpMonitor.Text = "Monitors";
            this.tpMonitor.UseVisualStyleBackColor = true;
            // 
            // lnkPublish
            // 
            this.lnkPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublish.AutoSize = true;
            this.lnkPublish.BackColor = System.Drawing.Color.Blue;
            this.lnkPublish.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublish.ForeColor = System.Drawing.Color.White;
            this.lnkPublish.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkPublish.LinkColor = System.Drawing.Color.White;
            this.lnkPublish.Location = new System.Drawing.Point(650, 295);
            this.lnkPublish.Name = "lnkPublish";
            this.lnkPublish.Size = new System.Drawing.Size(96, 15);
            this.lnkPublish.TabIndex = 44;
            this.lnkPublish.TabStop = true;
            this.lnkPublish.Text = "Publish To Sites";
            this.lnkPublish.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublish_LinkClicked);
            // 
            // btnViewLog
            // 
            this.btnViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnViewLog.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewLog.Location = new System.Drawing.Point(524, 287);
            this.btnViewLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnViewLog.Name = "btnViewLog";
            this.btnViewLog.Size = new System.Drawing.Size(90, 23);
            this.btnViewLog.TabIndex = 43;
            this.btnViewLog.Text = "View Log";
            this.btnViewLog.UseVisualStyleBackColor = true;
            this.btnViewLog.Click += new System.EventHandler(this.btnViewLog_Click);
            // 
            // btnDeleteMonitor
            // 
            this.btnDeleteMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteMonitor.CausesValidation = false;
            this.btnDeleteMonitor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteMonitor.Location = new System.Drawing.Point(266, 287);
            this.btnDeleteMonitor.Name = "btnDeleteMonitor";
            this.btnDeleteMonitor.Size = new System.Drawing.Size(90, 23);
            this.btnDeleteMonitor.TabIndex = 42;
            this.btnDeleteMonitor.Text = "Delete";
            this.btnDeleteMonitor.UseVisualStyleBackColor = true;
            this.btnDeleteMonitor.Click += new System.EventHandler(this.btnDeleteMonitor_Click);
            // 
            // btnCloseMonitor
            // 
            this.btnCloseMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCloseMonitor.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCloseMonitor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseMonitor.Location = new System.Drawing.Point(395, 287);
            this.btnCloseMonitor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCloseMonitor.Name = "btnCloseMonitor";
            this.btnCloseMonitor.Size = new System.Drawing.Size(90, 23);
            this.btnCloseMonitor.TabIndex = 41;
            this.btnCloseMonitor.Text = "Close";
            this.btnCloseMonitor.UseVisualStyleBackColor = true;
            this.btnCloseMonitor.Click += new System.EventHandler(this.btnCloseMonitor_Click);
            // 
            // btnRefreshMonitor
            // 
            this.btnRefreshMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshMonitor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshMonitor.Location = new System.Drawing.Point(137, 287);
            this.btnRefreshMonitor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefreshMonitor.Name = "btnRefreshMonitor";
            this.btnRefreshMonitor.Size = new System.Drawing.Size(90, 23);
            this.btnRefreshMonitor.TabIndex = 39;
            this.btnRefreshMonitor.Text = "Refresh";
            this.btnRefreshMonitor.UseVisualStyleBackColor = true;
            this.btnRefreshMonitor.Click += new System.EventHandler(this.btnRefreshMonitor_Click);
            // 
            // btnSaveMonitor
            // 
            this.btnSaveMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveMonitor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveMonitor.Location = new System.Drawing.Point(8, 287);
            this.btnSaveMonitor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveMonitor.Name = "btnSaveMonitor";
            this.btnSaveMonitor.Size = new System.Drawing.Size(90, 23);
            this.btnSaveMonitor.TabIndex = 40;
            this.btnSaveMonitor.Text = "Save";
            this.btnSaveMonitor.UseVisualStyleBackColor = true;
            this.btnSaveMonitor.Click += new System.EventHandler(this.btnSaveMonitor_Click);
            // 
            // tpMasters
            // 
            this.tpMasters.Controls.Add(this.lnkPublishPriority);
            this.tpMasters.Controls.Add(this.btnDeleteMaserData);
            this.tpMasters.Controls.Add(this.btnCloseMasterData);
            this.tpMasters.Controls.Add(this.btnRefreshMasterData);
            this.tpMasters.Controls.Add(this.btnSaveMasterData);
            this.tpMasters.Controls.Add(this.dgvMonitorPriority);
            this.tpMasters.Location = new System.Drawing.Point(4, 25);
            this.tpMasters.Name = "tpMasters";
            this.tpMasters.Padding = new System.Windows.Forms.Padding(3);
            this.tpMasters.Size = new System.Drawing.Size(840, 321);
            this.tpMasters.TabIndex = 3;
            this.tpMasters.Text = "MasterData";
            this.tpMasters.UseVisualStyleBackColor = true;
            // 
            // lnkPublishPriority
            // 
            this.lnkPublishPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublishPriority.AutoSize = true;
            this.lnkPublishPriority.BackColor = System.Drawing.Color.Blue;
            this.lnkPublishPriority.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublishPriority.ForeColor = System.Drawing.Color.White;
            this.lnkPublishPriority.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkPublishPriority.LinkColor = System.Drawing.Color.White;
            this.lnkPublishPriority.Location = new System.Drawing.Point(524, 295);
            this.lnkPublishPriority.Name = "lnkPublishPriority";
            this.lnkPublishPriority.Size = new System.Drawing.Size(96, 15);
            this.lnkPublishPriority.TabIndex = 47;
            this.lnkPublishPriority.TabStop = true;
            this.lnkPublishPriority.Text = "Publish To Sites";
            this.lnkPublishPriority.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublishPriority_LinkClicked);
            // 
            // btnDeleteMaserData
            // 
            this.btnDeleteMaserData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteMaserData.CausesValidation = false;
            this.btnDeleteMaserData.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteMaserData.Location = new System.Drawing.Point(266, 287);
            this.btnDeleteMaserData.Name = "btnDeleteMaserData";
            this.btnDeleteMaserData.Size = new System.Drawing.Size(90, 23);
            this.btnDeleteMaserData.TabIndex = 46;
            this.btnDeleteMaserData.Text = "Delete";
            this.btnDeleteMaserData.UseVisualStyleBackColor = true;
            this.btnDeleteMaserData.Click += new System.EventHandler(this.btnDeleteMaserData_Click);
            // 
            // btnCloseMasterData
            // 
            this.btnCloseMasterData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCloseMasterData.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCloseMasterData.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseMasterData.Location = new System.Drawing.Point(395, 287);
            this.btnCloseMasterData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCloseMasterData.Name = "btnCloseMasterData";
            this.btnCloseMasterData.Size = new System.Drawing.Size(90, 23);
            this.btnCloseMasterData.TabIndex = 45;
            this.btnCloseMasterData.Text = "Close";
            this.btnCloseMasterData.UseVisualStyleBackColor = true;
            this.btnCloseMasterData.Click += new System.EventHandler(this.btnCloseMasterData_Click);
            // 
            // btnRefreshMasterData
            // 
            this.btnRefreshMasterData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshMasterData.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshMasterData.Location = new System.Drawing.Point(137, 287);
            this.btnRefreshMasterData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefreshMasterData.Name = "btnRefreshMasterData";
            this.btnRefreshMasterData.Size = new System.Drawing.Size(90, 23);
            this.btnRefreshMasterData.TabIndex = 43;
            this.btnRefreshMasterData.Text = "Refresh";
            this.btnRefreshMasterData.UseVisualStyleBackColor = true;
            this.btnRefreshMasterData.Click += new System.EventHandler(this.btnRefreshMasterData_Click);
            // 
            // btnSaveMasterData
            // 
            this.btnSaveMasterData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveMasterData.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveMasterData.Location = new System.Drawing.Point(8, 287);
            this.btnSaveMasterData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveMasterData.Name = "btnSaveMasterData";
            this.btnSaveMasterData.Size = new System.Drawing.Size(90, 23);
            this.btnSaveMasterData.TabIndex = 44;
            this.btnSaveMasterData.Text = "Save";
            this.btnSaveMasterData.UseVisualStyleBackColor = true;
            this.btnSaveMasterData.Click += new System.EventHandler(this.btnSaveMasterData_Click);
            // 
            // dgvMonitorPriority
            // 
            this.dgvMonitorPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMonitorPriority.AutoGenerateColumns = false;
            this.dgvMonitorPriority.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMonitorPriority.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.priorityIdDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn2,
            this.descriptionDataGridViewTextBoxColumn,
            this.siteidDataGridViewTextBoxColumn2,
            this.guidDataGridViewTextBoxColumn2,
            this.synchStatusDataGridViewCheckBoxColumn2});
            this.dgvMonitorPriority.DataSource = this.monitorPriorityBindingSource1;
            this.dgvMonitorPriority.Location = new System.Drawing.Point(7, 7);
            this.dgvMonitorPriority.Name = "dgvMonitorPriority";
            this.dgvMonitorPriority.Size = new System.Drawing.Size(795, 273);
            this.dgvMonitorPriority.TabIndex = 0;
            this.dgvMonitorPriority.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvMonitorPriority_DataError);
            // 
            // priorityIdDataGridViewTextBoxColumn
            // 
            this.priorityIdDataGridViewTextBoxColumn.DataPropertyName = "PriorityId";
            this.priorityIdDataGridViewTextBoxColumn.HeaderText = "PriorityId";
            this.priorityIdDataGridViewTextBoxColumn.Name = "priorityIdDataGridViewTextBoxColumn";
            this.priorityIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn2
            // 
            this.nameDataGridViewTextBoxColumn2.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn2.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn2.Name = "nameDataGridViewTextBoxColumn2";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // siteidDataGridViewTextBoxColumn2
            // 
            this.siteidDataGridViewTextBoxColumn2.DataPropertyName = "site_id";
            this.siteidDataGridViewTextBoxColumn2.HeaderText = "site_id";
            this.siteidDataGridViewTextBoxColumn2.Name = "siteidDataGridViewTextBoxColumn2";
            this.siteidDataGridViewTextBoxColumn2.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn2
            // 
            this.guidDataGridViewTextBoxColumn2.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn2.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn2.Name = "guidDataGridViewTextBoxColumn2";
            this.guidDataGridViewTextBoxColumn2.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn2
            // 
            this.synchStatusDataGridViewCheckBoxColumn2.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn2.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn2.Name = "synchStatusDataGridViewCheckBoxColumn2";
            this.synchStatusDataGridViewCheckBoxColumn2.Visible = false;
            // 
            // monitorPriorityBindingSource1
            // 
            this.monitorPriorityBindingSource1.DataMember = "MonitorPriority";
            this.monitorPriorityBindingSource1.DataSource = this.monitorDataSet;
            // 
            // monitorAssetTypeTableAdapter
            // 
            this.monitorAssetTypeTableAdapter.ClearBeforeFill = true;
            // 
            // monitorPriorityTableAdapter
            // 
            this.monitorPriorityTableAdapter.ClearBeforeFill = true;
            // 
            // frmMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 355);
            this.Controls.Add(this.tcMonitor);
            this.Name = "frmMonitor";
            this.Text = "Monitor";
            this.Load += new System.EventHandler(this.frmMonitor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonitor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorAssetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorApplicationBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorAppModuleBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorPriorityBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorBindingSource)).EndInit();
            this.tcMonitor.ResumeLayout(false);
            this.tpDashboard.ResumeLayout(false);
            this.splitContainerMonitorLog.Panel1.ResumeLayout(false);
            this.splitContainerMonitorLog.Panel1.PerformLayout();
            this.splitContainerMonitorLog.Panel2.ResumeLayout(false);
            this.splitContainerMonitorLog.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMonitorLog)).EndInit();
            this.splitContainerMonitorLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonitorLog)).EndInit();
            this.tpAsset.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonitorAsset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorAssetTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorAssetBindingSource1)).EndInit();
            this.tpMonitor.ResumeLayout(false);
            this.tpMonitor.PerformLayout();
            this.tpMasters.ResumeLayout(false);
            this.tpMasters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonitorPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monitorPriorityBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMonitor;
        //private monitorDataSet monitorDataSet;
        private Semnox.Parafait.DataSet.monitorDataSet monitorDataSet;
        private System.Windows.Forms.BindingSource monitorBindingSource;
        private Semnox.Parafait.DataSet.monitorDataSetTableAdapters.MonitorTableAdapter monitorTableAdapter;
        private System.Windows.Forms.BindingSource monitorAssetBindingSource;
        private Semnox.Parafait.DataSet.monitorDataSetTableAdapters.MonitorAssetTableAdapter monitorAssetTableAdapter;
        private System.Windows.Forms.BindingSource monitorTypeBindingSource;
        private Semnox.Parafait.DataSet.monitorDataSetTableAdapters.MonitorTypeTableAdapter monitorTypeTableAdapter;
        private System.Windows.Forms.BindingSource monitorApplicationBindingSource;
        private Semnox.Parafait.DataSet.monitorDataSetTableAdapters.MonitorApplicationTableAdapter monitorApplicationTableAdapter;
        private System.Windows.Forms.BindingSource monitorAppModuleBindingSource;
        private Semnox.Parafait.DataSet.monitorDataSetTableAdapters.MonitorAppModuleTableAdapter monitorAppModuleTableAdapter;
        private System.Windows.Forms.TabControl tcMonitor;
        private System.Windows.Forms.TabPage tpAsset;
        private System.Windows.Forms.DataGridView dgvMonitorAsset;
        private System.Windows.Forms.BindingSource monitorAssetBindingSource1;
        private System.Windows.Forms.TabPage tpMonitor;
        private System.Windows.Forms.BindingSource monitorAssetTypeBindingSource;
        private Semnox.Parafait.DataSet.monitorDataSetTableAdapters.MonitorAssetTypeTableAdapter monitorAssetTypeTableAdapter;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDeleteMonitor;
        private System.Windows.Forms.Button btnCloseMonitor;
        private System.Windows.Forms.Button btnRefreshMonitor;
        private System.Windows.Forms.Button btnSaveMonitor;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iPAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn macAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteidDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.Button btnViewLog;
        private System.Windows.Forms.TabPage tpDashboard;
        private System.Windows.Forms.SplitContainer splitContainerMonitorLog;
        private System.Windows.Forms.TreeView tvSites;
        private System.Windows.Forms.DataGridView dgvMonitorLog;
        private System.Windows.Forms.LinkLabel lnkRefresh;
        private System.Windows.Forms.Label lblMonitorSite;
        private System.Windows.Forms.Label lblSiteName;
        private System.Windows.Forms.LinkLabel lnkOpenLog;
        private System.Windows.Forms.BindingSource monitorPriorityBindingSource;
        private Semnox.Parafait.DataSet.monitorDataSetTableAdapters.MonitorPriorityTableAdapter monitorPriorityTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn monitorIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn monitorTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn applicationIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn appModuleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn PriorityId;
        private System.Windows.Forms.DataGridViewTextBoxColumn intervalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn;
        private System.Windows.Forms.TabPage tpMasters;
        private System.Windows.Forms.DataGridView dgvMonitorPriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn priorityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteidDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn2;
        private System.Windows.Forms.BindingSource monitorPriorityBindingSource1;
        private System.Windows.Forms.Button btnDeleteMaserData;
        private System.Windows.Forms.Button btnCloseMasterData;
        private System.Windows.Forms.Button btnRefreshMasterData;
        private System.Windows.Forms.Button btnSaveMasterData;
        private System.Windows.Forms.LinkLabel lnkPublish;
        private System.Windows.Forms.LinkLabel lnkPublishPriority;
    }
}