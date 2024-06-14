namespace Semnox.Parafait.DigitalSignage
{
    partial class DisplayPanelListUI
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
            if(disposing && (components != null))
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtDisplayGroup = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnStartPC = new System.Windows.Forms.Button();
            this.btnShutdownPC = new System.Windows.Forms.Button();
            this.dgvDisplayPanelDTOList = new System.Windows.Forms.DataGridView();
            this.panelIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mACAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pCNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shutdownSecDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resolutionXDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.resolutionYDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.displayGroupDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.startTimeDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.endTimeDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.restartFlagDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.localFolderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayPanelDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.btnRestartPC = new System.Windows.Forms.Button();
            this.lnkPublish = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayPanelDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.displayPanelDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtDisplayGroup);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.chbShowActiveEntries);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(964, 47);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(501, 17);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtDisplayGroup
            // 
            this.txtDisplayGroup.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtDisplayGroup.Location = new System.Drawing.Point(330, 18);
            this.txtDisplayGroup.Name = "txtDisplayGroup";
            this.txtDisplayGroup.Size = new System.Drawing.Size(136, 21);
            this.txtDisplayGroup.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(167, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(157, 20);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Display Group Filter :";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(6, 20);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(175, 19);
            this.chbShowActiveEntries.TabIndex = 1;
            this.chbShowActiveEntries.Text = "Show Only Active Displays";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(10, 264);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(294, 264);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(196, 264);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(103, 264);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 16;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPreview.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPreview.Location = new System.Drawing.Point(401, 264);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 20;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnStartPC
            // 
            this.btnStartPC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartPC.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartPC.Location = new System.Drawing.Point(511, 264);
            this.btnStartPC.Name = "btnStartPC";
            this.btnStartPC.Size = new System.Drawing.Size(75, 23);
            this.btnStartPC.TabIndex = 21;
            this.btnStartPC.Text = "Start PC";
            this.btnStartPC.UseVisualStyleBackColor = true;
            this.btnStartPC.Click += new System.EventHandler(this.btnStartPC_Click);
            // 
            // btnShutdownPC
            // 
            this.btnShutdownPC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShutdownPC.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnShutdownPC.Location = new System.Drawing.Point(610, 264);
            this.btnShutdownPC.Name = "btnShutdownPC";
            this.btnShutdownPC.Size = new System.Drawing.Size(98, 23);
            this.btnShutdownPC.TabIndex = 22;
            this.btnShutdownPC.Text = "Shutdown PC";
            this.btnShutdownPC.UseVisualStyleBackColor = true;
            this.btnShutdownPC.Click += new System.EventHandler(this.btnShutDown_Click);
            // 
            // dgvDisplayPanelDTOList
            // 
            this.dgvDisplayPanelDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDisplayPanelDTOList.AutoGenerateColumns = false;
            this.dgvDisplayPanelDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDisplayPanelDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.panelIdDataGridViewTextBoxColumn,
            this.panelNameDataGridViewTextBoxColumn,
            this.locationDataGridViewTextBoxColumn,
            this.mACAddressDataGridViewTextBoxColumn,
            this.pCNameDataGridViewTextBoxColumn,
            this.shutdownSecDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.resolutionXDataGridViewComboBoxColumn,
            this.resolutionYDataGridViewComboBoxColumn,
            this.displayGroupDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.startTimeDataGridViewComboBoxColumn,
            this.endTimeDataGridViewComboBoxColumn,
            this.restartFlagDataGridViewCheckBoxColumn,
            this.localFolderDataGridViewTextBoxColumn});
            this.dgvDisplayPanelDTOList.DataSource = this.displayPanelDTOListBS;
            this.dgvDisplayPanelDTOList.Location = new System.Drawing.Point(10, 63);
            this.dgvDisplayPanelDTOList.Name = "dgvDisplayPanelDTOList";
            this.dgvDisplayPanelDTOList.Size = new System.Drawing.Size(964, 195);
            this.dgvDisplayPanelDTOList.TabIndex = 23;
            this.dgvDisplayPanelDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDisplayPanelDTOList_DataError);
            // 
            // panelIdDataGridViewTextBoxColumn
            // 
            this.panelIdDataGridViewTextBoxColumn.DataPropertyName = "PanelId";
            this.panelIdDataGridViewTextBoxColumn.Frozen = true;
            this.panelIdDataGridViewTextBoxColumn.HeaderText = "ID";
            this.panelIdDataGridViewTextBoxColumn.Name = "panelIdDataGridViewTextBoxColumn";
            this.panelIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // panelNameDataGridViewTextBoxColumn
            // 
            this.panelNameDataGridViewTextBoxColumn.DataPropertyName = "PanelName";
            this.panelNameDataGridViewTextBoxColumn.Frozen = true;
            this.panelNameDataGridViewTextBoxColumn.HeaderText = "Panel Name";
            this.panelNameDataGridViewTextBoxColumn.Name = "panelNameDataGridViewTextBoxColumn";
            // 
            // locationDataGridViewTextBoxColumn
            // 
            this.locationDataGridViewTextBoxColumn.DataPropertyName = "Location";
            this.locationDataGridViewTextBoxColumn.Frozen = true;
            this.locationDataGridViewTextBoxColumn.HeaderText = "Location";
            this.locationDataGridViewTextBoxColumn.Name = "locationDataGridViewTextBoxColumn";
            // 
            // mACAddressDataGridViewTextBoxColumn
            // 
            this.mACAddressDataGridViewTextBoxColumn.DataPropertyName = "MACAddress";
            this.mACAddressDataGridViewTextBoxColumn.HeaderText = "MAC Address";
            this.mACAddressDataGridViewTextBoxColumn.Name = "mACAddressDataGridViewTextBoxColumn";
            // 
            // pCNameDataGridViewTextBoxColumn
            // 
            this.pCNameDataGridViewTextBoxColumn.DataPropertyName = "PCName";
            this.pCNameDataGridViewTextBoxColumn.HeaderText = "PC Name";
            this.pCNameDataGridViewTextBoxColumn.Name = "pCNameDataGridViewTextBoxColumn";
            // 
            // shutdownSecDataGridViewTextBoxColumn
            // 
            this.shutdownSecDataGridViewTextBoxColumn.DataPropertyName = "ShutdownSec";
            this.shutdownSecDataGridViewTextBoxColumn.HeaderText = "Shutdown Sec";
            this.shutdownSecDataGridViewTextBoxColumn.Name = "shutdownSecDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // resolutionXDataGridViewComboBoxColumn
            // 
            this.resolutionXDataGridViewComboBoxColumn.DataPropertyName = "ResolutionX";
            this.resolutionXDataGridViewComboBoxColumn.HeaderText = "Horizontal (Pixels)";
            this.resolutionXDataGridViewComboBoxColumn.Name = "resolutionXDataGridViewComboBoxColumn";
            this.resolutionXDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.resolutionXDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // resolutionYDataGridViewComboBoxColumn
            // 
            this.resolutionYDataGridViewComboBoxColumn.DataPropertyName = "ResolutionY";
            this.resolutionYDataGridViewComboBoxColumn.HeaderText = "Vertical (Pixels)";
            this.resolutionYDataGridViewComboBoxColumn.Name = "resolutionYDataGridViewComboBoxColumn";
            this.resolutionYDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.resolutionYDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // displayGroupDataGridViewTextBoxColumn
            // 
            this.displayGroupDataGridViewTextBoxColumn.DataPropertyName = "DisplayGroup";
            this.displayGroupDataGridViewTextBoxColumn.HeaderText = "Display Group";
            this.displayGroupDataGridViewTextBoxColumn.Name = "displayGroupDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // startTimeDataGridViewComboBoxColumn
            // 
            this.startTimeDataGridViewComboBoxColumn.DataPropertyName = "StartTime";
            this.startTimeDataGridViewComboBoxColumn.HeaderText = "Start Time";
            this.startTimeDataGridViewComboBoxColumn.Name = "startTimeDataGridViewComboBoxColumn";
            this.startTimeDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.startTimeDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // endTimeDataGridViewComboBoxColumn
            // 
            this.endTimeDataGridViewComboBoxColumn.DataPropertyName = "EndTime";
            this.endTimeDataGridViewComboBoxColumn.HeaderText = "End Time";
            this.endTimeDataGridViewComboBoxColumn.Name = "endTimeDataGridViewComboBoxColumn";
            this.endTimeDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.endTimeDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // restartFlagDataGridViewCheckBoxColumn
            // 
            this.restartFlagDataGridViewCheckBoxColumn.DataPropertyName = "RestartFlag";
            this.restartFlagDataGridViewCheckBoxColumn.FalseValue = "N";
            this.restartFlagDataGridViewCheckBoxColumn.HeaderText = "Restart Flag";
            this.restartFlagDataGridViewCheckBoxColumn.IndeterminateValue = "N";
            this.restartFlagDataGridViewCheckBoxColumn.Name = "restartFlagDataGridViewCheckBoxColumn";
            this.restartFlagDataGridViewCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.restartFlagDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.restartFlagDataGridViewCheckBoxColumn.TrueValue = "Y";
            this.restartFlagDataGridViewCheckBoxColumn.Visible = false;
            // 
            // localFolderDataGridViewTextBoxColumn
            // 
            this.localFolderDataGridViewTextBoxColumn.DataPropertyName = "LocalFolder";
            this.localFolderDataGridViewTextBoxColumn.HeaderText = "Local Folder";
            this.localFolderDataGridViewTextBoxColumn.Name = "localFolderDataGridViewTextBoxColumn";
            // 
            // displayPanelDTOListBS
            // 
            this.displayPanelDTOListBS.DataSource = typeof(Semnox.Parafait.DigitalSignage.DisplayPanelDTO);
            // 
            // btnRestartPC
            // 
            this.btnRestartPC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRestartPC.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRestartPC.Location = new System.Drawing.Point(734, 264);
            this.btnRestartPC.Name = "btnRestartPC";
            this.btnRestartPC.Size = new System.Drawing.Size(98, 23);
            this.btnRestartPC.TabIndex = 24;
            this.btnRestartPC.Text = "Restart PC";
            this.btnRestartPC.UseVisualStyleBackColor = true;
            this.btnRestartPC.Click += new System.EventHandler(this.btnRestartPC_Click);
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
            this.lnkPublish.Location = new System.Drawing.Point(878, 268);
            this.lnkPublish.Name = "lnkPublish";
            this.lnkPublish.Size = new System.Drawing.Size(96, 15);
            this.lnkPublish.TabIndex = 41;
            this.lnkPublish.TabStop = true;
            this.lnkPublish.Text = "Publish To Sites";
            this.lnkPublish.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublish_LinkClicked);
            // 
            // DisplayPanelListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 300);
            this.Controls.Add(this.lnkPublish);
            this.Controls.Add(this.btnRestartPC);
            this.Controls.Add(this.dgvDisplayPanelDTOList);
            this.Controls.Add(this.btnShutdownPC);
            this.Controls.Add(this.btnStartPC);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.groupBox1);
            this.Name = "DisplayPanelListUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Panels";
            this.Load += new System.EventHandler(this.DisplayPanelListUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayPanelDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.displayPanelDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtDisplayGroup;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnStartPC;
        private System.Windows.Forms.Button btnShutdownPC;
        private System.Windows.Forms.DataGridView dgvDisplayPanelDTOList;
        private System.Windows.Forms.BindingSource displayPanelDTOListBS;
        private System.Windows.Forms.Button btnRestartPC;
        private System.Windows.Forms.DataGridViewTextBoxColumn panelIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn panelNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn locationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mACAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pCNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn shutdownSecDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn resolutionXDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn resolutionYDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayGroupDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn startTimeDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn endTimeDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn restartFlagDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn localFolderDataGridViewTextBoxColumn;
        private System.Windows.Forms.LinkLabel lnkPublish;
    }
}