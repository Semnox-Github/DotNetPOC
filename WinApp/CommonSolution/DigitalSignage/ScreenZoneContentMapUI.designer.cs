namespace Semnox.Parafait.DigitalSignage
{
    partial class ScreenZoneContentMapUI
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
            this.btnRefersh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvContentSetupGrid = new System.Windows.Forms.DataGridView();
            this.gpContent = new System.Windows.Forms.GroupBox();
            this.gpZones = new System.Windows.Forms.GroupBox();
            this.dgvZones = new System.Windows.Forms.DataGridView();
            this.TopLeft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BottomRight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GetZone = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ScreenContentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContentTypeId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ContentId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.zoneIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.screenIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayOrderDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.screenZoneDefSetupDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.displayOrderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.backImageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.backColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.borderSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.borderColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imgSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.imgAlignmentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imgRefreshSecsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.videoRepeatDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lookupRefreshSecsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lookupHeaderDisplayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tickerScrollDirectionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tickerSpeedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tickerRefreshSecsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.screenZoneContentMapDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContentSetupGrid)).BeginInit();
            this.gpContent.SuspendLayout();
            this.gpZones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvZones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.screenZoneDefSetupDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.screenZoneContentMapDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(269, 365);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 24;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(397, 365);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefersh
            // 
            this.btnRefersh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefersh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefersh.Location = new System.Drawing.Point(141, 365);
            this.btnRefersh.Name = "btnRefersh";
            this.btnRefersh.Size = new System.Drawing.Size(75, 23);
            this.btnRefersh.TabIndex = 22;
            this.btnRefersh.Text = "Refresh";
            this.btnRefersh.UseVisualStyleBackColor = true;
            this.btnRefersh.Click += new System.EventHandler(this.btnRefersh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(13, 365);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvContentSetupGrid
            // 
            this.dgvContentSetupGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvContentSetupGrid.AutoGenerateColumns = false;
            this.dgvContentSetupGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContentSetupGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ScreenContentId,
            this.isActiveDataGridViewTextBoxColumn,
            this.ContentTypeId,
            this.ContentId,
            this.displayOrderDataGridViewTextBoxColumn,
            this.backImageDataGridViewTextBoxColumn,
            this.backColorDataGridViewTextBoxColumn,
            this.borderSizeDataGridViewTextBoxColumn,
            this.borderColorDataGridViewTextBoxColumn,
            this.imgSizeDataGridViewTextBoxColumn,
            this.imgAlignmentDataGridViewTextBoxColumn,
            this.imgRefreshSecsDataGridViewTextBoxColumn,
            this.videoRepeatDataGridViewTextBoxColumn,
            this.lookupRefreshSecsDataGridViewTextBoxColumn,
            this.lookupHeaderDisplayDataGridViewTextBoxColumn,
            this.tickerScrollDirectionDataGridViewTextBoxColumn,
            this.tickerSpeedDataGridViewTextBoxColumn,
            this.tickerRefreshSecsDataGridViewTextBoxColumn});
            this.dgvContentSetupGrid.DataSource = this.screenZoneContentMapDTOBindingSource;
            this.dgvContentSetupGrid.Location = new System.Drawing.Point(6, 19);
            this.dgvContentSetupGrid.Name = "dgvContentSetupGrid";
            this.dgvContentSetupGrid.Size = new System.Drawing.Size(739, 171);
            this.dgvContentSetupGrid.TabIndex = 20;
            this.dgvContentSetupGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContentSetupGrid_CellDoubleClick);
            this.dgvContentSetupGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContentSetupGrid_CellValueChanged);
            this.dgvContentSetupGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvContentSetupGrid_CurrentCellDirtyStateChanged);
            this.dgvContentSetupGrid.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvContentSetupGrid_DataBindingComplete);
            this.dgvContentSetupGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvContentSetupGrid_DataError);
            // 
            // gpContent
            // 
            this.gpContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpContent.Controls.Add(this.dgvContentSetupGrid);
            this.gpContent.Cursor = System.Windows.Forms.Cursors.Default;
            this.gpContent.Location = new System.Drawing.Point(8, 152);
            this.gpContent.Name = "gpContent";
            this.gpContent.Size = new System.Drawing.Size(751, 196);
            this.gpContent.TabIndex = 25;
            this.gpContent.TabStop = false;
            this.gpContent.Text = "Content";
            // 
            // gpZones
            // 
            this.gpZones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpZones.Controls.Add(this.dgvZones);
            this.gpZones.Location = new System.Drawing.Point(12, 12);
            this.gpZones.Name = "gpZones";
            this.gpZones.Size = new System.Drawing.Size(741, 134);
            this.gpZones.TabIndex = 26;
            this.gpZones.TabStop = false;
            this.gpZones.Text = "Zones";
            // 
            // dgvZones
            // 
            this.dgvZones.AllowUserToAddRows = false;
            this.dgvZones.AllowUserToDeleteRows = false;
            this.dgvZones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvZones.AutoGenerateColumns = false;
            this.dgvZones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvZones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.zoneIdDataGridViewTextBoxColumn,
            this.screenIdDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.displayOrderDataGridViewTextBoxColumn1,
            this.descriptionDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn1,
            this.TopLeft,
            this.BottomRight,
            this.GetZone});
            this.dgvZones.DataSource = this.screenZoneDefSetupDTOBindingSource;
            this.dgvZones.Location = new System.Drawing.Point(6, 19);
            this.dgvZones.Name = "dgvZones";
            this.dgvZones.ReadOnly = true;
            this.dgvZones.Size = new System.Drawing.Size(729, 109);
            this.dgvZones.TabIndex = 0;
            this.dgvZones.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvZones_CellContentClick);
            // 
            // TopLeft
            // 
            this.TopLeft.DataPropertyName = "TopLeft";
            this.TopLeft.HeaderText = "TopLeft";
            this.TopLeft.Name = "TopLeft";
            this.TopLeft.ReadOnly = true;
            this.TopLeft.Visible = false;
            // 
            // BottomRight
            // 
            this.BottomRight.DataPropertyName = "BottomRight";
            this.BottomRight.HeaderText = "BottomRight";
            this.BottomRight.Name = "BottomRight";
            this.BottomRight.ReadOnly = true;
            this.BottomRight.Visible = false;
            // 
            // GetZone
            // 
            this.GetZone.HeaderText = "GetZone";
            this.GetZone.Name = "GetZone";
            this.GetZone.ReadOnly = true;
            this.GetZone.Text = "GetZone";
            this.GetZone.UseColumnTextForButtonValue = true;
            // 
            // ScreenContentId
            // 
            this.ScreenContentId.DataPropertyName = "ScreenContentId";
            this.ScreenContentId.HeaderText = "ScreenContentId";
            this.ScreenContentId.Name = "ScreenContentId";
            this.ScreenContentId.ReadOnly = true;
            this.ScreenContentId.Visible = false;
            // 
            // ContentTypeId
            // 
            this.ContentTypeId.DataPropertyName = "ContentTypeId";
            this.ContentTypeId.HeaderText = "Content Type";
            this.ContentTypeId.Name = "ContentTypeId";
            // 
            // ContentId
            // 
            this.ContentId.DataPropertyName = "ContentGuid";
            this.ContentId.HeaderText = "Content";
            this.ContentId.Name = "ContentId";
            this.ContentId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ContentId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // zoneIdDataGridViewTextBoxColumn
            // 
            this.zoneIdDataGridViewTextBoxColumn.DataPropertyName = "ZoneId";
            this.zoneIdDataGridViewTextBoxColumn.HeaderText = "ID";
            this.zoneIdDataGridViewTextBoxColumn.Name = "zoneIdDataGridViewTextBoxColumn";
            this.zoneIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // screenIdDataGridViewTextBoxColumn
            // 
            this.screenIdDataGridViewTextBoxColumn.DataPropertyName = "ScreenId";
            this.screenIdDataGridViewTextBoxColumn.HeaderText = "Screen ID";
            this.screenIdDataGridViewTextBoxColumn.Name = "screenIdDataGridViewTextBoxColumn";
            this.screenIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // displayOrderDataGridViewTextBoxColumn1
            // 
            this.displayOrderDataGridViewTextBoxColumn1.DataPropertyName = "DisplayOrder";
            this.displayOrderDataGridViewTextBoxColumn1.HeaderText = "Display Order";
            this.displayOrderDataGridViewTextBoxColumn1.Name = "displayOrderDataGridViewTextBoxColumn1";
            this.displayOrderDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isActiveDataGridViewTextBoxColumn1
            // 
            this.isActiveDataGridViewTextBoxColumn1.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn1.HeaderText = "Active?";
            this.isActiveDataGridViewTextBoxColumn1.Name = "isActiveDataGridViewTextBoxColumn1";
            this.isActiveDataGridViewTextBoxColumn1.ReadOnly = true;
            this.isActiveDataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // screenZoneDefSetupDTOBindingSource
            // 
            this.screenZoneDefSetupDTOBindingSource.DataSource = typeof(Semnox.Parafait.DigitalSignage.ScreenZoneDefSetupDTO);
            this.screenZoneDefSetupDTOBindingSource.CurrentChanged += new System.EventHandler(this.screenZoneDefSetupDTOBindingSource_CurrentChanged);
            // 
            // isActiveDataGridViewTextBoxColumn
            // 
            this.isActiveDataGridViewTextBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewTextBoxColumn.Name = "isActiveDataGridViewTextBoxColumn";
            this.isActiveDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // displayOrderDataGridViewTextBoxColumn
            // 
            this.displayOrderDataGridViewTextBoxColumn.DataPropertyName = "DisplayOrder";
            this.displayOrderDataGridViewTextBoxColumn.HeaderText = "Display Order";
            this.displayOrderDataGridViewTextBoxColumn.Name = "displayOrderDataGridViewTextBoxColumn";
            // 
            // backImageDataGridViewTextBoxColumn
            // 
            this.backImageDataGridViewTextBoxColumn.DataPropertyName = "BackImage";
            this.backImageDataGridViewTextBoxColumn.HeaderText = "Background Image";
            this.backImageDataGridViewTextBoxColumn.Name = "backImageDataGridViewTextBoxColumn";
            this.backImageDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.backImageDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // backColorDataGridViewTextBoxColumn
            // 
            this.backColorDataGridViewTextBoxColumn.DataPropertyName = "BackColor";
            this.backColorDataGridViewTextBoxColumn.HeaderText = "Back Color";
            this.backColorDataGridViewTextBoxColumn.Name = "backColorDataGridViewTextBoxColumn";
            // 
            // borderSizeDataGridViewTextBoxColumn
            // 
            this.borderSizeDataGridViewTextBoxColumn.DataPropertyName = "BorderSize";
            this.borderSizeDataGridViewTextBoxColumn.HeaderText = "Border Size";
            this.borderSizeDataGridViewTextBoxColumn.Name = "borderSizeDataGridViewTextBoxColumn";
            this.borderSizeDataGridViewTextBoxColumn.Visible = false;
            // 
            // borderColorDataGridViewTextBoxColumn
            // 
            this.borderColorDataGridViewTextBoxColumn.DataPropertyName = "BorderColor";
            this.borderColorDataGridViewTextBoxColumn.HeaderText = "Border Color";
            this.borderColorDataGridViewTextBoxColumn.Name = "borderColorDataGridViewTextBoxColumn";
            this.borderColorDataGridViewTextBoxColumn.Visible = false;
            // 
            // imgSizeDataGridViewTextBoxColumn
            // 
            this.imgSizeDataGridViewTextBoxColumn.DataPropertyName = "ImgSize";
            this.imgSizeDataGridViewTextBoxColumn.HeaderText = "Image Size";
            this.imgSizeDataGridViewTextBoxColumn.Name = "imgSizeDataGridViewTextBoxColumn";
            this.imgSizeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.imgSizeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.imgSizeDataGridViewTextBoxColumn.Visible = false;
            // 
            // imgAlignmentDataGridViewTextBoxColumn
            // 
            this.imgAlignmentDataGridViewTextBoxColumn.DataPropertyName = "ImgAlignment";
            this.imgAlignmentDataGridViewTextBoxColumn.HeaderText = "Image Alignment";
            this.imgAlignmentDataGridViewTextBoxColumn.Name = "imgAlignmentDataGridViewTextBoxColumn";
            this.imgAlignmentDataGridViewTextBoxColumn.Visible = false;
            // 
            // imgRefreshSecsDataGridViewTextBoxColumn
            // 
            this.imgRefreshSecsDataGridViewTextBoxColumn.DataPropertyName = "ImgRefreshSecs";
            this.imgRefreshSecsDataGridViewTextBoxColumn.HeaderText = "Refresh(Secs)";
            this.imgRefreshSecsDataGridViewTextBoxColumn.Name = "imgRefreshSecsDataGridViewTextBoxColumn";
            // 
            // videoRepeatDataGridViewTextBoxColumn
            // 
            this.videoRepeatDataGridViewTextBoxColumn.DataPropertyName = "VideoRepeat";
            this.videoRepeatDataGridViewTextBoxColumn.FalseValue = "N";
            this.videoRepeatDataGridViewTextBoxColumn.HeaderText = "Video Repeat";
            this.videoRepeatDataGridViewTextBoxColumn.Name = "videoRepeatDataGridViewTextBoxColumn";
            this.videoRepeatDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.videoRepeatDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.videoRepeatDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // lookupRefreshSecsDataGridViewTextBoxColumn
            // 
            this.lookupRefreshSecsDataGridViewTextBoxColumn.DataPropertyName = "LookupRefreshSecs";
            this.lookupRefreshSecsDataGridViewTextBoxColumn.HeaderText = "Lookup Refresh (Secs)";
            this.lookupRefreshSecsDataGridViewTextBoxColumn.Name = "lookupRefreshSecsDataGridViewTextBoxColumn";
            // 
            // lookupHeaderDisplayDataGridViewTextBoxColumn
            // 
            this.lookupHeaderDisplayDataGridViewTextBoxColumn.DataPropertyName = "LookupHeaderDisplay";
            this.lookupHeaderDisplayDataGridViewTextBoxColumn.FalseValue = "N";
            this.lookupHeaderDisplayDataGridViewTextBoxColumn.HeaderText = "Lookup Header Display";
            this.lookupHeaderDisplayDataGridViewTextBoxColumn.Name = "lookupHeaderDisplayDataGridViewTextBoxColumn";
            this.lookupHeaderDisplayDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.lookupHeaderDisplayDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.lookupHeaderDisplayDataGridViewTextBoxColumn.TrueValue = "Y";
            this.lookupHeaderDisplayDataGridViewTextBoxColumn.Visible = false;
            // 
            // tickerScrollDirectionDataGridViewTextBoxColumn
            // 
            this.tickerScrollDirectionDataGridViewTextBoxColumn.DataPropertyName = "TickerScrollDirection";
            this.tickerScrollDirectionDataGridViewTextBoxColumn.HeaderText = "Ticker Scroll Direction";
            this.tickerScrollDirectionDataGridViewTextBoxColumn.Name = "tickerScrollDirectionDataGridViewTextBoxColumn";
            this.tickerScrollDirectionDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tickerScrollDirectionDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // tickerSpeedDataGridViewTextBoxColumn
            // 
            this.tickerSpeedDataGridViewTextBoxColumn.DataPropertyName = "TickerSpeed";
            this.tickerSpeedDataGridViewTextBoxColumn.HeaderText = "Ticker Speed";
            this.tickerSpeedDataGridViewTextBoxColumn.Name = "tickerSpeedDataGridViewTextBoxColumn";
            // 
            // tickerRefreshSecsDataGridViewTextBoxColumn
            // 
            this.tickerRefreshSecsDataGridViewTextBoxColumn.DataPropertyName = "TickerRefreshSecs";
            this.tickerRefreshSecsDataGridViewTextBoxColumn.HeaderText = "Ticker Refresh (Secs)";
            this.tickerRefreshSecsDataGridViewTextBoxColumn.Name = "tickerRefreshSecsDataGridViewTextBoxColumn";
            // 
            // screenZoneContentMapDTOBindingSource
            // 
            this.screenZoneContentMapDTOBindingSource.DataSource = typeof(Semnox.Parafait.DigitalSignage.ScreenZoneContentMapDTO);
            // 
            // ScreenZoneContentMapUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 398);
            this.Controls.Add(this.gpZones);
            this.Controls.Add(this.gpContent);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefersh);
            this.Controls.Add(this.btnSave);
            this.Name = "ScreenZoneContentMapUI";
            this.Text = "Screen Content Map";
            this.Load += new System.EventHandler(this.ScreenZoneContentMapUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContentSetupGrid)).EndInit();
            this.gpContent.ResumeLayout(false);
            this.gpZones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvZones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.screenZoneDefSetupDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.screenZoneContentMapDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefersh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvContentSetupGrid;
        private System.Windows.Forms.BindingSource screenZoneContentMapDTOBindingSource;
        private System.Windows.Forms.GroupBox gpContent;
        private System.Windows.Forms.GroupBox gpZones;
        private System.Windows.Forms.DataGridView dgvZones;
        private System.Windows.Forms.BindingSource screenZoneDefSetupDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn zoneIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn screenIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayOrderDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewButtonColumn GetZone;
        private System.Windows.Forms.DataGridViewTextBoxColumn TopLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn BottomRight;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScreenContentId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ContentTypeId;
        private System.Windows.Forms.DataGridViewComboBoxColumn ContentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayOrderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn backImageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn backColorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn borderSizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn borderColorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn imgSizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imgAlignmentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imgRefreshSecsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn videoRepeatDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lookupRefreshSecsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn lookupHeaderDisplayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn tickerScrollDirectionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tickerSpeedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tickerRefreshSecsDataGridViewTextBoxColumn;
    }
}

