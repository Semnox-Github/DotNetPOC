namespace Semnox.Parafait.Device.Turnstile
{
    partial class frmTurnstileUI
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtTurnstileName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.dgvTurnstile = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnControlPanel = new System.Windows.Forms.Button();
            this.searchByParametersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PassageAAlias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PassageBAlias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblMessage = new System.Windows.Forms.Label();
            this.turnstileIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.turnstileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iPAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gameProfileIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.portNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.useRSProtocolDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.turnstileTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.turnstileMakeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.turnstileModelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.activeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TurnstileSetupDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.turnstileDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnstile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchByParametersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TurnstileSetupDTOListBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.turnstileDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(443, 495);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 24);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(110, 495);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 23;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(11, 494);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.txtTurnstileName);
            this.gpFilter.Controls.Add(this.lblName);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(11, 12);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(1071, 45);
            this.gpFilter.TabIndex = 21;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(411, 14);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtTurnstileName
            // 
            this.txtTurnstileName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtTurnstileName.Location = new System.Drawing.Point(269, 16);
            this.txtTurnstileName.Name = "txtTurnstileName";
            this.txtTurnstileName.Size = new System.Drawing.Size(136, 21);
            this.txtTurnstileName.TabIndex = 2;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(154, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(109, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Turnstile Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(12, 18);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 0;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            this.chbShowActiveEntries.CheckedChanged += new System.EventHandler(this.chbShowActiveEntries_CheckedChanged);
            // 
            // dgvTurnstile
            // 
            this.dgvTurnstile.AllowUserToDeleteRows = false;
            this.dgvTurnstile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTurnstile.AutoGenerateColumns = false;
            this.dgvTurnstile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTurnstile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.turnstileIdDataGridViewTextBoxColumn,
            this.turnstileNameDataGridViewTextBoxColumn,
            this.PassageAAlias,
            this.PassageBAlias,
            this.iPAddressDataGridViewTextBoxColumn,
            this.gameProfileIdDataGridViewComboBoxColumn,
            this.portNumberDataGridViewTextBoxColumn,
            this.useRSProtocolDataGridViewCheckBoxColumn,
            this.turnstileTypeDataGridViewTextBoxColumn,
            this.turnstileMakeDataGridViewTextBoxColumn,
            this.turnstileModelDataGridViewTextBoxColumn,
            this.activeDataGridViewCheckBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn});
            this.dgvTurnstile.DataSource = this.TurnstileSetupDTOListBS;
            this.dgvTurnstile.EnableHeadersVisualStyles = false;
            this.dgvTurnstile.Location = new System.Drawing.Point(13, 71);
            this.dgvTurnstile.MultiSelect = false;
            this.dgvTurnstile.Name = "dgvTurnstile";
            this.dgvTurnstile.ShowCellErrors = false;
            this.dgvTurnstile.Size = new System.Drawing.Size(1069, 393);
            this.dgvTurnstile.TabIndex = 20;
            this.dgvTurnstile.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvTurnstile_DataError);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(210, 495);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnControlPanel
            // 
            this.btnControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnControlPanel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnControlPanel.Location = new System.Drawing.Point(309, 495);
            this.btnControlPanel.Name = "btnControlPanel";
            this.btnControlPanel.Size = new System.Drawing.Size(114, 24);
            this.btnControlPanel.TabIndex = 26;
            this.btnControlPanel.Text = "Control Panel";
            this.btnControlPanel.UseVisualStyleBackColor = true;
            this.btnControlPanel.Click += new System.EventHandler(this.btnControlPanel_Click);
            // 
            // searchByParametersBindingSource
            // 
            this.searchByParametersBindingSource.DataSource = typeof(Semnox.Parafait.Device.Turnstile.TurnstileDTO.SearchByParameters);
            // 
            // PassageAAlias
            // 
            this.PassageAAlias.DataPropertyName = "PassageAAlias";
            this.PassageAAlias.HeaderText = "Passage A Alias";
            this.PassageAAlias.Name = "PassageAAlias";
            // 
            // PassageBAlias
            // 
            this.PassageBAlias.DataPropertyName = "PassageBAlias";
            this.PassageBAlias.HeaderText = "Passage B Alias";
            this.PassageBAlias.Name = "PassageBAlias";
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblMessage.Location = new System.Drawing.Point(16, 471);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 19);
            this.lblMessage.TabIndex = 74;
            // 
            // turnstileIdDataGridViewTextBoxColumn
            // 
            this.turnstileIdDataGridViewTextBoxColumn.DataPropertyName = "TurnstileId";
            this.turnstileIdDataGridViewTextBoxColumn.HeaderText = "Sl#";
            this.turnstileIdDataGridViewTextBoxColumn.Name = "turnstileIdDataGridViewTextBoxColumn";
            this.turnstileIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // turnstileNameDataGridViewTextBoxColumn
            // 
            this.turnstileNameDataGridViewTextBoxColumn.DataPropertyName = "TurnstileName";
            this.turnstileNameDataGridViewTextBoxColumn.HeaderText = "Turnstile Name";
            this.turnstileNameDataGridViewTextBoxColumn.Name = "turnstileNameDataGridViewTextBoxColumn";
            // 
            // iPAddressDataGridViewTextBoxColumn
            // 
            this.iPAddressDataGridViewTextBoxColumn.DataPropertyName = "IPAddress";
            this.iPAddressDataGridViewTextBoxColumn.HeaderText = "IP Address";
            this.iPAddressDataGridViewTextBoxColumn.Name = "iPAddressDataGridViewTextBoxColumn";
            // 
            // gameProfileIdDataGridViewComboBoxColumn
            // 
            this.gameProfileIdDataGridViewComboBoxColumn.DataPropertyName = "GameProfileId";
            this.gameProfileIdDataGridViewComboBoxColumn.HeaderText = "Game Profile";
            this.gameProfileIdDataGridViewComboBoxColumn.Name = "gameProfileIdDataGridViewComboBoxColumn";
            this.gameProfileIdDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gameProfileIdDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // portNumberDataGridViewTextBoxColumn
            // 
            this.portNumberDataGridViewTextBoxColumn.DataPropertyName = "PortNumber";
            this.portNumberDataGridViewTextBoxColumn.HeaderText = "Port Number";
            this.portNumberDataGridViewTextBoxColumn.Name = "portNumberDataGridViewTextBoxColumn";
            // 
            // useRSProtocolDataGridViewCheckBoxColumn
            // 
            this.useRSProtocolDataGridViewCheckBoxColumn.DataPropertyName = "UseRSProtocol";
            this.useRSProtocolDataGridViewCheckBoxColumn.FalseValue = "false";
            this.useRSProtocolDataGridViewCheckBoxColumn.HeaderText = "Use RS Protocol";
            this.useRSProtocolDataGridViewCheckBoxColumn.IndeterminateValue = "false";
            this.useRSProtocolDataGridViewCheckBoxColumn.Name = "useRSProtocolDataGridViewCheckBoxColumn";
            this.useRSProtocolDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // turnstileTypeDataGridViewTextBoxColumn
            // 
            this.turnstileTypeDataGridViewTextBoxColumn.DataPropertyName = "Type";
            this.turnstileTypeDataGridViewTextBoxColumn.HeaderText = "Turnstile Type";
            this.turnstileTypeDataGridViewTextBoxColumn.Name = "turnstileTypeDataGridViewTextBoxColumn";
            this.turnstileTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.turnstileTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // turnstileMakeDataGridViewTextBoxColumn
            // 
            this.turnstileMakeDataGridViewTextBoxColumn.DataPropertyName = "Make";
            this.turnstileMakeDataGridViewTextBoxColumn.HeaderText = "Turnstile Make";
            this.turnstileMakeDataGridViewTextBoxColumn.Name = "turnstileMakeDataGridViewTextBoxColumn";
            this.turnstileMakeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.turnstileMakeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // turnstileModelDataGridViewTextBoxColumn
            // 
            this.turnstileModelDataGridViewTextBoxColumn.DataPropertyName = "Model";
            this.turnstileModelDataGridViewTextBoxColumn.HeaderText = "Turnstile Model";
            this.turnstileModelDataGridViewTextBoxColumn.Name = "turnstileModelDataGridViewTextBoxColumn";
            this.turnstileModelDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.turnstileModelDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // activeDataGridViewCheckBoxColumn
            // 
            this.activeDataGridViewCheckBoxColumn.DataPropertyName = "Active";
            this.activeDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.activeDataGridViewCheckBoxColumn.Name = "activeDataGridViewCheckBoxColumn";
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // TurnstileSetupDTOListBS
            // 
            this.TurnstileSetupDTOListBS.DataSource = typeof(Semnox.Parafait.Device.Turnstile.TurnstileDTO);
            // 
            // turnstileDTOBindingSource
            // 
            this.turnstileDTOBindingSource.DataSource = typeof(Semnox.Parafait.Device.Turnstile.TurnstileDTO);
            // 
            // frmTurnstileUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1093, 528);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnControlPanel);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gpFilter);
            this.Controls.Add(this.dgvTurnstile);
            this.Name = "frmTurnstileUI";
            this.Text = "Turnstile Setup";
            this.Load += new System.EventHandler(this.frmTurnstileUI_Load);
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnstile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchByParametersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TurnstileSetupDTOListBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.turnstileDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtTurnstileName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.DataGridView dgvTurnstile;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnControlPanel;
        private System.Windows.Forms.BindingSource TurnstileSetupDTOListBS;
        private System.Windows.Forms.BindingSource searchByParametersBindingSource;
        private System.Windows.Forms.BindingSource turnstileDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn turnstileIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn turnstileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PassageAAlias;
        private System.Windows.Forms.DataGridViewTextBoxColumn PassageBAlias;
        private System.Windows.Forms.DataGridViewTextBoxColumn iPAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn gameProfileIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn portNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn useRSProtocolDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn turnstileTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn turnstileMakeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn turnstileModelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label lblMessage;
    }
}