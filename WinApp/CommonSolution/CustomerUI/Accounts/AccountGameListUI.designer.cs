namespace Semnox.Parafait.Customer.Accounts
{
    partial class AccountGameListUI
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
            this.dgvAccountGameDTOList = new System.Windows.Forms.DataGridView();
            this.accountGameDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.dgvAccountGameExtendedDTOList = new System.Windows.Forms.DataGridView();
            this.accountGameExtendedDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.lblStatus = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountGameIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gameProfileIdAccountGameDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gameIdAccountGameDataGridComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.frequencyDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balanceGamesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketAllowedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.fromDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expiryDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mondayDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tuesdayDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.wednesdayDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.thursdayDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.fridayDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.saturdayDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.sundayDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.expireWithMembershipDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.entitlementTypeDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lastPlayedTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.optionalAttributeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customDataDataGridViewButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.accountGameExtendedIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gameIdAccountGameExtendedDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.excludeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PlayLimitPerGame = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountGameDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountGameDTOListBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountGameExtendedDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountGameExtendedDTOListBS)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.CausesValidation = false;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(250, 3);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0, 3, 20, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(105, 25);
            this.btnDelete.TabIndex = 26;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(375, 3);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0, 3, 20, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(105, 25);
            this.btnClose.TabIndex = 25;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(125, 3);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(0, 3, 20, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(105, 25);
            this.btnRefresh.TabIndex = 23;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(0, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(0, 3, 20, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 25);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvAccountGameDTOList
            // 
            this.dgvAccountGameDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAccountGameDTOList.AutoGenerateColumns = false;
            this.dgvAccountGameDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccountGameDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.accountGameIdDataGridViewTextBoxColumn,
            this.gameProfileIdAccountGameDataGridViewComboBoxColumn,
            this.gameIdAccountGameDataGridComboBoxColumn,
            this.frequencyDataGridViewComboBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.balanceGamesDataGridViewTextBoxColumn,
            this.ticketAllowedDataGridViewCheckBoxColumn,
            this.fromDateDataGridViewTextBoxColumn,
            this.expiryDateDataGridViewTextBoxColumn,
            this.mondayDataGridViewCheckBoxColumn,
            this.tuesdayDataGridViewCheckBoxColumn,
            this.wednesdayDataGridViewCheckBoxColumn,
            this.thursdayDataGridViewCheckBoxColumn,
            this.fridayDataGridViewCheckBoxColumn,
            this.saturdayDataGridViewCheckBoxColumn,
            this.sundayDataGridViewCheckBoxColumn,
            this.expireWithMembershipDataGridViewCheckBoxColumn,
            this.entitlementTypeDataGridViewComboBoxColumn,
            this.lastPlayedTimeDataGridViewTextBoxColumn,
            this.optionalAttributeDataGridViewTextBoxColumn,
            this.customDataDataGridViewButtonColumn});
            this.dgvAccountGameDTOList.DataSource = this.accountGameDTOListBS;
            this.dgvAccountGameDTOList.Location = new System.Drawing.Point(7, 22);
            this.dgvAccountGameDTOList.Name = "dgvAccountGameDTOList";
            this.dgvAccountGameDTOList.Size = new System.Drawing.Size(1212, 270);
            this.dgvAccountGameDTOList.TabIndex = 22;
            this.dgvAccountGameDTOList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccountGameDTOList_CellContentClick);
            this.dgvAccountGameDTOList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccountGameDTOList_CellEnter);
            this.dgvAccountGameDTOList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccountGameDTOList_CellValueChanged);
            this.dgvAccountGameDTOList.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvAccountGameDTOList_CurrentCellDirtyStateChanged);
            this.dgvAccountGameDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAccountGameDTOList_DataError);
            // 
            // accountGameDTOListBS
            // 
            this.accountGameDTOListBS.DataSource = typeof(Semnox.Parafait.Customer.Accounts.AccountGameDTO);
            this.accountGameDTOListBS.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.accountGameDTOListBS_AddingNew);
            this.accountGameDTOListBS.DataSourceChanged += new System.EventHandler(this.accountGameDTOListBS_DataSourceChanged);
            this.accountGameDTOListBS.CurrentChanged += new System.EventHandler(this.accountGameDTOListBS_CurrentChanged);
            // 
            // dgvAccountGameExtendedDTOList
            // 
            this.dgvAccountGameExtendedDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAccountGameExtendedDTOList.AutoGenerateColumns = false;
            this.dgvAccountGameExtendedDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccountGameExtendedDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.accountGameExtendedIdDataGridViewTextBoxColumn,
            this.gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn,
            this.gameIdAccountGameExtendedDataGridViewComboBoxColumn,
            this.excludeDataGridViewCheckBoxColumn,
            this.PlayLimitPerGame});
            this.dgvAccountGameExtendedDTOList.DataSource = this.accountGameExtendedDTOListBS;
            this.dgvAccountGameExtendedDTOList.Location = new System.Drawing.Point(7, 20);
            this.dgvAccountGameExtendedDTOList.Name = "dgvAccountGameExtendedDTOList";
            this.dgvAccountGameExtendedDTOList.Size = new System.Drawing.Size(1210, 269);
            this.dgvAccountGameExtendedDTOList.TabIndex = 28;
            this.dgvAccountGameExtendedDTOList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccountGameExtendedDTOList_CellEnter);
            this.dgvAccountGameExtendedDTOList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccountGameExtendedDTOList_CellValueChanged);
            this.dgvAccountGameExtendedDTOList.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvAccountGameExtendedDTOList_CurrentCellDirtyStateChanged);
            this.dgvAccountGameExtendedDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAccountGameExtendedDTOList_DataError);
            // 
            // accountGameExtendedDTOListBS
            // 
            this.accountGameExtendedDTOListBS.DataSource = typeof(Semnox.Parafait.Customer.Accounts.AccountGameExtendedDTO);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(9, 617);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(133, 15);
            this.lblStatus.TabIndex = 43;
            this.lblStatus.Text = "Loading.. Please wait..";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Controls.Add(this.btnRefresh);
            this.flowLayoutPanel1.Controls.Add(this.btnDelete);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(7, 638);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1150, 31);
            this.flowLayoutPanel1.TabIndex = 44;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgvAccountGameDTOList);
            this.groupBox1.Location = new System.Drawing.Point(7, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1226, 299);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add or Remove Games / Entitlements from Card";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgvAccountGameExtendedDTOList);
            this.groupBox2.Location = new System.Drawing.Point(7, 319);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1226, 295);
            this.groupBox2.TabIndex = 46;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Extended Inclusion / Exclusions";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "AccountGameId";
            this.dataGridViewTextBoxColumn1.HeaderText = "Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Quantity";
            this.dataGridViewTextBoxColumn2.HeaderText = "Play Count / Entt. Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "BalanceGames";
            this.dataGridViewTextBoxColumn3.HeaderText = "Balance Games";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "LastPlayedTime";
            this.dataGridViewTextBoxColumn4.HeaderText = "Last Played Time";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "FromDate";
            this.dataGridViewTextBoxColumn5.HeaderText = "From Date";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "ExpiryDate";
            this.dataGridViewTextBoxColumn6.HeaderText = "Expiry Date";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "OptionalAttribute";
            this.dataGridViewTextBoxColumn7.HeaderText = "Optional Attribute";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "AccountGameExtendedId";
            this.dataGridViewTextBoxColumn8.HeaderText = "Id";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // accountGameIdDataGridViewTextBoxColumn
            // 
            this.accountGameIdDataGridViewTextBoxColumn.DataPropertyName = "AccountGameId";
            this.accountGameIdDataGridViewTextBoxColumn.HeaderText = "Id";
            this.accountGameIdDataGridViewTextBoxColumn.Name = "accountGameIdDataGridViewTextBoxColumn";
            this.accountGameIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // gameProfileIdAccountGameDataGridViewComboBoxColumn
            // 
            this.gameProfileIdAccountGameDataGridViewComboBoxColumn.DataPropertyName = "GameProfileId";
            this.gameProfileIdAccountGameDataGridViewComboBoxColumn.HeaderText = "Game Profile";
            this.gameProfileIdAccountGameDataGridViewComboBoxColumn.Name = "gameProfileIdAccountGameDataGridViewComboBoxColumn";
            this.gameProfileIdAccountGameDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gameProfileIdAccountGameDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // gameIdAccountGameDataGridComboBoxColumn
            // 
            this.gameIdAccountGameDataGridComboBoxColumn.DataPropertyName = "GameId";
            this.gameIdAccountGameDataGridComboBoxColumn.HeaderText = "Game";
            this.gameIdAccountGameDataGridComboBoxColumn.Name = "gameIdAccountGameDataGridComboBoxColumn";
            this.gameIdAccountGameDataGridComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gameIdAccountGameDataGridComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // frequencyDataGridViewComboBoxColumn
            // 
            this.frequencyDataGridViewComboBoxColumn.DataPropertyName = "Frequency";
            this.frequencyDataGridViewComboBoxColumn.HeaderText = "Frequency";
            this.frequencyDataGridViewComboBoxColumn.Name = "frequencyDataGridViewComboBoxColumn";
            this.frequencyDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.frequencyDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Play Count / Entt. Value";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            // 
            // balanceGamesDataGridViewTextBoxColumn
            // 
            this.balanceGamesDataGridViewTextBoxColumn.DataPropertyName = "BalanceGames";
            this.balanceGamesDataGridViewTextBoxColumn.HeaderText = "Balance Games";
            this.balanceGamesDataGridViewTextBoxColumn.Name = "balanceGamesDataGridViewTextBoxColumn";
            this.balanceGamesDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ticketAllowedDataGridViewCheckBoxColumn
            // 
            this.ticketAllowedDataGridViewCheckBoxColumn.DataPropertyName = "TicketAllowed";
            this.ticketAllowedDataGridViewCheckBoxColumn.HeaderText = "Ticket Allowed";
            this.ticketAllowedDataGridViewCheckBoxColumn.Name = "ticketAllowedDataGridViewCheckBoxColumn";
            // 
            // fromDateDataGridViewTextBoxColumn
            // 
            this.fromDateDataGridViewTextBoxColumn.DataPropertyName = "FromDate";
            this.fromDateDataGridViewTextBoxColumn.HeaderText = "From Date";
            this.fromDateDataGridViewTextBoxColumn.Name = "fromDateDataGridViewTextBoxColumn";
            // 
            // expiryDateDataGridViewTextBoxColumn
            // 
            this.expiryDateDataGridViewTextBoxColumn.DataPropertyName = "ExpiryDate";
            this.expiryDateDataGridViewTextBoxColumn.HeaderText = "Expiry Date";
            this.expiryDateDataGridViewTextBoxColumn.Name = "expiryDateDataGridViewTextBoxColumn";
            // 
            // mondayDataGridViewCheckBoxColumn
            // 
            this.mondayDataGridViewCheckBoxColumn.DataPropertyName = "Monday";
            this.mondayDataGridViewCheckBoxColumn.HeaderText = "Monday";
            this.mondayDataGridViewCheckBoxColumn.Name = "mondayDataGridViewCheckBoxColumn";
            // 
            // tuesdayDataGridViewCheckBoxColumn
            // 
            this.tuesdayDataGridViewCheckBoxColumn.DataPropertyName = "Tuesday";
            this.tuesdayDataGridViewCheckBoxColumn.HeaderText = "Tuesday";
            this.tuesdayDataGridViewCheckBoxColumn.Name = "tuesdayDataGridViewCheckBoxColumn";
            // 
            // wednesdayDataGridViewCheckBoxColumn
            // 
            this.wednesdayDataGridViewCheckBoxColumn.DataPropertyName = "Wednesday";
            this.wednesdayDataGridViewCheckBoxColumn.HeaderText = "Wednesday";
            this.wednesdayDataGridViewCheckBoxColumn.Name = "wednesdayDataGridViewCheckBoxColumn";
            // 
            // thursdayDataGridViewCheckBoxColumn
            // 
            this.thursdayDataGridViewCheckBoxColumn.DataPropertyName = "Thursday";
            this.thursdayDataGridViewCheckBoxColumn.HeaderText = "Thursday";
            this.thursdayDataGridViewCheckBoxColumn.Name = "thursdayDataGridViewCheckBoxColumn";
            // 
            // fridayDataGridViewCheckBoxColumn
            // 
            this.fridayDataGridViewCheckBoxColumn.DataPropertyName = "Friday";
            this.fridayDataGridViewCheckBoxColumn.HeaderText = "Friday";
            this.fridayDataGridViewCheckBoxColumn.Name = "fridayDataGridViewCheckBoxColumn";
            // 
            // saturdayDataGridViewCheckBoxColumn
            // 
            this.saturdayDataGridViewCheckBoxColumn.DataPropertyName = "Saturday";
            this.saturdayDataGridViewCheckBoxColumn.HeaderText = "Saturday";
            this.saturdayDataGridViewCheckBoxColumn.Name = "saturdayDataGridViewCheckBoxColumn";
            // 
            // sundayDataGridViewCheckBoxColumn
            // 
            this.sundayDataGridViewCheckBoxColumn.DataPropertyName = "Sunday";
            this.sundayDataGridViewCheckBoxColumn.HeaderText = "Sunday";
            this.sundayDataGridViewCheckBoxColumn.Name = "sundayDataGridViewCheckBoxColumn";
            // 
            // expireWithMembershipDataGridViewCheckBoxColumn
            // 
            this.expireWithMembershipDataGridViewCheckBoxColumn.DataPropertyName = "ExpireWithMembership";
            this.expireWithMembershipDataGridViewCheckBoxColumn.HeaderText = "Expire With Membership";
            this.expireWithMembershipDataGridViewCheckBoxColumn.Name = "expireWithMembershipDataGridViewCheckBoxColumn";
            // 
            // entitlementTypeDataGridViewComboBoxColumn
            // 
            this.entitlementTypeDataGridViewComboBoxColumn.DataPropertyName = "EntitlementType";
            this.entitlementTypeDataGridViewComboBoxColumn.HeaderText = "Entitlement Type";
            this.entitlementTypeDataGridViewComboBoxColumn.Name = "entitlementTypeDataGridViewComboBoxColumn";
            this.entitlementTypeDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.entitlementTypeDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // lastPlayedTimeDataGridViewTextBoxColumn
            // 
            this.lastPlayedTimeDataGridViewTextBoxColumn.DataPropertyName = "LastPlayedTime";
            this.lastPlayedTimeDataGridViewTextBoxColumn.HeaderText = "Last Played Time";
            this.lastPlayedTimeDataGridViewTextBoxColumn.Name = "lastPlayedTimeDataGridViewTextBoxColumn";
            this.lastPlayedTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // optionalAttributeDataGridViewTextBoxColumn
            // 
            this.optionalAttributeDataGridViewTextBoxColumn.DataPropertyName = "OptionalAttribute";
            this.optionalAttributeDataGridViewTextBoxColumn.HeaderText = "Optional Attribute";
            this.optionalAttributeDataGridViewTextBoxColumn.Name = "optionalAttributeDataGridViewTextBoxColumn";
            // 
            // customDataDataGridViewButtonColumn
            // 
            this.customDataDataGridViewButtonColumn.HeaderText = "Custom";
            this.customDataDataGridViewButtonColumn.Name = "customDataDataGridViewButtonColumn";
            // 
            // accountGameExtendedIdDataGridViewTextBoxColumn
            // 
            this.accountGameExtendedIdDataGridViewTextBoxColumn.DataPropertyName = "AccountGameExtendedId";
            this.accountGameExtendedIdDataGridViewTextBoxColumn.HeaderText = "Id";
            this.accountGameExtendedIdDataGridViewTextBoxColumn.Name = "accountGameExtendedIdDataGridViewTextBoxColumn";
            this.accountGameExtendedIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn
            // 
            this.gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn.DataPropertyName = "GameProfileId";
            this.gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn.HeaderText = "Game Profile";
            this.gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn.Name = "gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn";
            this.gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // gameIdAccountGameExtendedDataGridViewComboBoxColumn
            // 
            this.gameIdAccountGameExtendedDataGridViewComboBoxColumn.DataPropertyName = "GameId";
            this.gameIdAccountGameExtendedDataGridViewComboBoxColumn.HeaderText = "Game";
            this.gameIdAccountGameExtendedDataGridViewComboBoxColumn.Name = "gameIdAccountGameExtendedDataGridViewComboBoxColumn";
            this.gameIdAccountGameExtendedDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gameIdAccountGameExtendedDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // excludeDataGridViewCheckBoxColumn
            // 
            this.excludeDataGridViewCheckBoxColumn.DataPropertyName = "Exclude";
            this.excludeDataGridViewCheckBoxColumn.HeaderText = "Exclude?";
            this.excludeDataGridViewCheckBoxColumn.Name = "excludeDataGridViewCheckBoxColumn";
            // 
            // PlayLimitPerGame
            // 
            this.PlayLimitPerGame.DataPropertyName = "PlayLimitPerGame";
            this.PlayLimitPerGame.HeaderText = "Play Limit Per Game";
            this.PlayLimitPerGame.Name = "PlayLimitPerGame";
            // 
            // AccountGameListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1245, 681);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.lblStatus);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AccountGameListUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Card Games";
            this.Load += new System.EventHandler(this.AccountGameListUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountGameDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountGameDTOListBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountGameExtendedDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountGameExtendedDTOListBS)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvAccountGameDTOList;
        private System.Windows.Forms.DataGridView dgvAccountGameExtendedDTOList;
        private System.Windows.Forms.BindingSource accountGameDTOListBS;
        private System.Windows.Forms.BindingSource accountGameExtendedDTOListBS;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountGameIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn gameProfileIdAccountGameDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn gameIdAccountGameDataGridComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn frequencyDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn balanceGamesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ticketAllowedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expiryDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn mondayDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn tuesdayDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn wednesdayDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn thursdayDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn fridayDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn saturdayDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sundayDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn expireWithMembershipDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn entitlementTypeDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastPlayedTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn optionalAttributeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn customDataDataGridViewButtonColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountGameExtendedIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn gameProfileIdAccountGameExtendedDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn gameIdAccountGameExtendedDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn excludeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlayLimitPerGame;
    }
}