namespace Semnox.Parafait.Customer.Accounts
{
    partial class frmTokenCardInventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTokenCardInventory));
            this.tc_cards = new System.Windows.Forms.TabControl();
            this.tp_add = new System.Windows.Forms.TabPage();
            this.btnCardSave = new System.Windows.Forms.Button();
            this.BtnCardClose = new System.Windows.Forms.Button();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblAddCards = new System.Windows.Forms.Label();
            this.txtNoOfCards = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtToSerial = new System.Windows.Forms.TextBox();
            this.lblfrmSerNo = new System.Windows.Forms.Label();
            this.txtFromSerial = new System.Windows.Forms.TextBox();
            this.lblRecDate = new System.Windows.Forms.Label();
            this.tp_delete = new System.Windows.Forms.TabPage();
            this.lb_del_date = new System.Windows.Forms.Label();
            this.lb_del_user = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDelNumber = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnDelSave = new System.Windows.Forms.Button();
            this.tp_reports = new System.Windows.Forms.TabPage();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.fromSerialNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toserialNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actiondateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actionByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machineTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activityTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tokenCardInventoryDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.BtnGenerate = new System.Windows.Forms.Button();
            this.dtp_toDate = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.dtp_fromDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.rboPeriod = new System.Windows.Forms.RadioButton();
            this.rboTillDate = new System.Windows.Forms.RadioButton();
            this.BtnDisk = new System.Windows.Forms.Button();
            this.tp_CardInventory = new System.Windows.Forms.TabPage();
            this.btnTokenCancel = new System.Windows.Forms.Button();
            this.btnTokenSave = new System.Windows.Forms.Button();
            this.grp1 = new System.Windows.Forms.GroupBox();
            this.txtCardPurchased = new System.Windows.Forms.TextBox();
            this.txtCardsOnHand = new System.Windows.Forms.TextBox();
            this.txtTokenPOS = new System.Windows.Forms.TextBox();
            this.txtTransferredToken = new System.Windows.Forms.TextBox();
            this.txtTokenHand = new System.Windows.Forms.TextBox();
            this.txtTokenKiosk = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.lblCardsPurchased = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.lblTechName = new System.Windows.Forms.Label();
            this.lblWeekendDate = new System.Windows.Forms.Label();
            this.lblSiteName = new System.Windows.Forms.Label();
            this.lblWeekend = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblCardBalance = new System.Windows.Forms.Label();
            this.lb_cards = new System.Windows.Forms.Label();
            this.lblCardInventory = new System.Windows.Forms.Label();
            this.lb_inventory = new System.Windows.Forms.Label();
            this.lb_issued_td = new System.Windows.Forms.Label();
            this.lblCardIssued = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.tc_cards.SuspendLayout();
            this.tp_add.SuspendLayout();
            this.tp_delete.SuspendLayout();
            this.tp_reports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tokenCardInventoryDTOBindingSource)).BeginInit();
            this.tp_CardInventory.SuspendLayout();
            this.grp1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tc_cards
            // 
            this.tc_cards.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tc_cards.Controls.Add(this.tp_add);
            this.tc_cards.Controls.Add(this.tp_delete);
            this.tc_cards.Controls.Add(this.tp_reports);
            this.tc_cards.Controls.Add(this.tp_CardInventory);
            this.tc_cards.Dock = System.Windows.Forms.DockStyle.Top;
            this.tc_cards.Location = new System.Drawing.Point(0, 0);
            this.tc_cards.Name = "tc_cards";
            this.tc_cards.SelectedIndex = 0;
            this.tc_cards.Size = new System.Drawing.Size(759, 457);
            this.tc_cards.TabIndex = 0;
            this.tc_cards.SelectedIndexChanged += new System.EventHandler(this.tc_cards_SelectedIndexChanged);
            // 
            // tp_add
            // 
            this.tp_add.BackColor = System.Drawing.Color.White;
            this.tp_add.Controls.Add(this.btnCardSave);
            this.tp_add.Controls.Add(this.BtnCardClose);
            this.tp_add.Controls.Add(this.lblUser);
            this.tp_add.Controls.Add(this.lblDate);
            this.tp_add.Controls.Add(this.lblUserName);
            this.tp_add.Controls.Add(this.lblAddCards);
            this.tp_add.Controls.Add(this.txtNoOfCards);
            this.tp_add.Controls.Add(this.label3);
            this.tp_add.Controls.Add(this.txtToSerial);
            this.tp_add.Controls.Add(this.lblfrmSerNo);
            this.tp_add.Controls.Add(this.txtFromSerial);
            this.tp_add.Controls.Add(this.lblRecDate);
            this.tp_add.Location = new System.Drawing.Point(4, 25);
            this.tp_add.Name = "tp_add";
            this.tp_add.Padding = new System.Windows.Forms.Padding(3);
            this.tp_add.Size = new System.Drawing.Size(751, 428);
            this.tp_add.TabIndex = 0;
            this.tp_add.Text = "Add Cards";
            // 
            // btnCardSave
            // 
            this.btnCardSave.Location = new System.Drawing.Point(306, 255);
            this.btnCardSave.Name = "btnCardSave";
            this.btnCardSave.Size = new System.Drawing.Size(75, 23);
            this.btnCardSave.TabIndex = 12;
            this.btnCardSave.Text = "Save";
            this.btnCardSave.UseVisualStyleBackColor = true;
            this.btnCardSave.Click += new System.EventHandler(this.BtnCardSave_Click);
            // 
            // BtnCardClose
            // 
            this.BtnCardClose.Location = new System.Drawing.Point(476, 255);
            this.BtnCardClose.Name = "BtnCardClose";
            this.BtnCardClose.Size = new System.Drawing.Size(75, 23);
            this.BtnCardClose.TabIndex = 11;
            this.BtnCardClose.Text = "Close";
            this.BtnCardClose.UseVisualStyleBackColor = true;
            this.BtnCardClose.Click += new System.EventHandler(this.BtnCardClose_Click);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.ForeColor = System.Drawing.Color.DimGray;
            this.lblUser.Location = new System.Drawing.Point(303, 118);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(27, 13);
            this.lblUser.TabIndex = 2;
            this.lblUser.Text = "user";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.ForeColor = System.Drawing.Color.DimGray;
            this.lblDate.Location = new System.Drawing.Point(303, 141);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(28, 13);
            this.lblDate.TabIndex = 4;
            this.lblDate.Text = "date";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Location = new System.Drawing.Point(263, 118);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(37, 13);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "User:";
            // 
            // lblAddCards
            // 
            this.lblAddCards.AutoSize = true;
            this.lblAddCards.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddCards.Location = new System.Drawing.Point(196, 193);
            this.lblAddCards.Name = "lblAddCards";
            this.lblAddCards.Size = new System.Drawing.Size(104, 13);
            this.lblAddCards.TabIndex = 9;
            this.lblAddCards.Text = "Number of cards:";
            // 
            // txtNoOfCards
            // 
            this.txtNoOfCards.Location = new System.Drawing.Point(306, 190);
            this.txtNoOfCards.Name = "txtNoOfCards";
            this.txtNoOfCards.Size = new System.Drawing.Size(57, 20);
            this.txtNoOfCards.TabIndex = 10;
            this.txtNoOfCards.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNoOfCards_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(412, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "To Ser #:";
            // 
            // txtToSerial
            // 
            this.txtToSerial.Location = new System.Drawing.Point(476, 164);
            this.txtToSerial.Name = "txtToSerial";
            this.txtToSerial.Size = new System.Drawing.Size(100, 20);
            this.txtToSerial.TabIndex = 8;
            // 
            // lblfrmSerNo
            // 
            this.lblfrmSerNo.AutoSize = true;
            this.lblfrmSerNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblfrmSerNo.Location = new System.Drawing.Point(227, 167);
            this.lblfrmSerNo.Name = "lblfrmSerNo";
            this.lblfrmSerNo.Size = new System.Drawing.Size(73, 13);
            this.lblfrmSerNo.TabIndex = 5;
            this.lblfrmSerNo.Text = "From Ser #:";
            // 
            // txtFromSerial
            // 
            this.txtFromSerial.Location = new System.Drawing.Point(306, 164);
            this.txtFromSerial.Name = "txtFromSerial";
            this.txtFromSerial.Size = new System.Drawing.Size(100, 20);
            this.txtFromSerial.TabIndex = 6;
            // 
            // lblRecDate
            // 
            this.lblRecDate.AutoSize = true;
            this.lblRecDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecDate.Location = new System.Drawing.Point(204, 141);
            this.lblRecDate.Name = "lblRecDate";
            this.lblRecDate.Size = new System.Drawing.Size(96, 13);
            this.lblRecDate.TabIndex = 3;
            this.lblRecDate.Text = "Received Date:";
            // 
            // tp_delete
            // 
            this.tp_delete.BackColor = System.Drawing.Color.White;
            this.tp_delete.Controls.Add(this.lb_del_date);
            this.tp_delete.Controls.Add(this.lb_del_user);
            this.tp_delete.Controls.Add(this.label10);
            this.tp_delete.Controls.Add(this.label9);
            this.tp_delete.Controls.Add(this.txtDelNumber);
            this.tp_delete.Controls.Add(this.label8);
            this.tp_delete.Controls.Add(this.btnExit);
            this.tp_delete.Controls.Add(this.btnDelSave);
            this.tp_delete.Location = new System.Drawing.Point(4, 25);
            this.tp_delete.Name = "tp_delete";
            this.tp_delete.Padding = new System.Windows.Forms.Padding(3);
            this.tp_delete.Size = new System.Drawing.Size(751, 428);
            this.tp_delete.TabIndex = 1;
            this.tp_delete.Text = "Delete Cards";
            // 
            // lb_del_date
            // 
            this.lb_del_date.AutoSize = true;
            this.lb_del_date.ForeColor = System.Drawing.Color.DimGray;
            this.lb_del_date.Location = new System.Drawing.Point(368, 160);
            this.lb_del_date.Name = "lb_del_date";
            this.lb_del_date.Size = new System.Drawing.Size(41, 13);
            this.lb_del_date.TabIndex = 4;
            this.lb_del_date.Text = "label12";
            // 
            // lb_del_user
            // 
            this.lb_del_user.AutoSize = true;
            this.lb_del_user.ForeColor = System.Drawing.Color.DimGray;
            this.lb_del_user.Location = new System.Drawing.Point(368, 130);
            this.lb_del_user.Name = "lb_del_user";
            this.lb_del_user.Size = new System.Drawing.Size(41, 13);
            this.lb_del_user.TabIndex = 2;
            this.lb_del_user.Text = "label11";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(260, 190);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Number of cards:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(326, 160);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Date:";
            // 
            // txtDelNumber
            // 
            this.txtDelNumber.Location = new System.Drawing.Point(371, 186);
            this.txtDelNumber.Name = "txtDelNumber";
            this.txtDelNumber.Size = new System.Drawing.Size(57, 20);
            this.txtDelNumber.TabIndex = 6;
            this.txtDelNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNoOfCards_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(327, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "User:";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(433, 253);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(90, 23);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Close";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDelSave
            // 
            this.btnDelSave.Location = new System.Drawing.Point(263, 253);
            this.btnDelSave.Name = "btnDelSave";
            this.btnDelSave.Size = new System.Drawing.Size(75, 23);
            this.btnDelSave.TabIndex = 8;
            this.btnDelSave.Text = "Save";
            this.btnDelSave.UseVisualStyleBackColor = true;
            this.btnDelSave.Click += new System.EventHandler(this.btnDelSave_Click);
            // 
            // tp_reports
            // 
            this.tp_reports.BackColor = System.Drawing.Color.White;
            this.tp_reports.Controls.Add(this.dgvStock);
            this.tp_reports.Controls.Add(this.btnClose);
            this.tp_reports.Controls.Add(this.BtnGenerate);
            this.tp_reports.Controls.Add(this.dtp_toDate);
            this.tp_reports.Controls.Add(this.label12);
            this.tp_reports.Controls.Add(this.dtp_fromDate);
            this.tp_reports.Controls.Add(this.label7);
            this.tp_reports.Controls.Add(this.rboPeriod);
            this.tp_reports.Controls.Add(this.rboTillDate);
            this.tp_reports.Controls.Add(this.BtnDisk);
            this.tp_reports.Location = new System.Drawing.Point(4, 25);
            this.tp_reports.Name = "tp_reports";
            this.tp_reports.Padding = new System.Windows.Forms.Padding(3);
            this.tp_reports.Size = new System.Drawing.Size(751, 428);
            this.tp_reports.TabIndex = 2;
            this.tp_reports.Text = "View Inventory";
            // 
            // dgvStock
            // 
            this.dgvStock.AllowUserToAddRows = false;
            this.dgvStock.AutoGenerateColumns = false;
            this.dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fromSerialNumberDataGridViewTextBoxColumn,
            this.toserialNumberDataGridViewTextBoxColumn,
            this.numberDataGridViewTextBoxColumn,
            this.actionDataGridViewTextBoxColumn,
            this.actiondateDataGridViewTextBoxColumn,
            this.actionByDataGridViewTextBoxColumn,
            this.tagTypeDataGridViewTextBoxColumn,
            this.machineTypeDataGridViewTextBoxColumn,
            this.activityTypeDataGridViewTextBoxColumn,
            this.lastUpdatedDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn});
            this.dgvStock.DataSource = this.tokenCardInventoryDTOBindingSource;
            this.dgvStock.Location = new System.Drawing.Point(8, 86);
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.ReadOnly = true;
            this.dgvStock.Size = new System.Drawing.Size(725, 324);
            this.dgvStock.TabIndex = 25;
            // 
            // fromSerialNumberDataGridViewTextBoxColumn
            // 
            this.fromSerialNumberDataGridViewTextBoxColumn.DataPropertyName = "FromSerialNumber";
            this.fromSerialNumberDataGridViewTextBoxColumn.HeaderText = "From Serial Number";
            this.fromSerialNumberDataGridViewTextBoxColumn.Name = "fromSerialNumberDataGridViewTextBoxColumn";
            this.fromSerialNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // toserialNumberDataGridViewTextBoxColumn
            // 
            this.toserialNumberDataGridViewTextBoxColumn.DataPropertyName = "ToserialNumber";
            this.toserialNumberDataGridViewTextBoxColumn.HeaderText = "To Serial Number";
            this.toserialNumberDataGridViewTextBoxColumn.Name = "toserialNumberDataGridViewTextBoxColumn";
            this.toserialNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // numberDataGridViewTextBoxColumn
            // 
            this.numberDataGridViewTextBoxColumn.DataPropertyName = "Number";
            this.numberDataGridViewTextBoxColumn.HeaderText = "Number of Cards";
            this.numberDataGridViewTextBoxColumn.Name = "numberDataGridViewTextBoxColumn";
            this.numberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // actionDataGridViewTextBoxColumn
            // 
            this.actionDataGridViewTextBoxColumn.DataPropertyName = "Action";
            this.actionDataGridViewTextBoxColumn.HeaderText = "Action";
            this.actionDataGridViewTextBoxColumn.Name = "actionDataGridViewTextBoxColumn";
            this.actionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // actiondateDataGridViewTextBoxColumn
            // 
            this.actiondateDataGridViewTextBoxColumn.DataPropertyName = "Actiondate";
            this.actiondateDataGridViewTextBoxColumn.HeaderText = "Action Date";
            this.actiondateDataGridViewTextBoxColumn.Name = "actiondateDataGridViewTextBoxColumn";
            this.actiondateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // actionByDataGridViewTextBoxColumn
            // 
            this.actionByDataGridViewTextBoxColumn.DataPropertyName = "ActionBy";
            this.actionByDataGridViewTextBoxColumn.HeaderText = "Action By";
            this.actionByDataGridViewTextBoxColumn.Name = "actionByDataGridViewTextBoxColumn";
            this.actionByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tagTypeDataGridViewTextBoxColumn
            // 
            this.tagTypeDataGridViewTextBoxColumn.DataPropertyName = "TagType";
            this.tagTypeDataGridViewTextBoxColumn.HeaderText = "TagType";
            this.tagTypeDataGridViewTextBoxColumn.Name = "tagTypeDataGridViewTextBoxColumn";
            this.tagTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.tagTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // machineTypeDataGridViewTextBoxColumn
            // 
            this.machineTypeDataGridViewTextBoxColumn.DataPropertyName = "MachineType";
            this.machineTypeDataGridViewTextBoxColumn.HeaderText = "MachineType";
            this.machineTypeDataGridViewTextBoxColumn.Name = "machineTypeDataGridViewTextBoxColumn";
            this.machineTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.machineTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // activityTypeDataGridViewTextBoxColumn
            // 
            this.activityTypeDataGridViewTextBoxColumn.DataPropertyName = "ActivityType";
            this.activityTypeDataGridViewTextBoxColumn.HeaderText = "ActivityType";
            this.activityTypeDataGridViewTextBoxColumn.Name = "activityTypeDataGridViewTextBoxColumn";
            this.activityTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.activityTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn.HeaderText = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn.Name = "lastUpdatedDateDataGridViewTextBoxColumn";
            this.lastUpdatedDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdatedDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdatedByDataGridViewTextBoxColumn.Visible = false;
            // 
            // tokenCardInventoryDTOBindingSource
            // 
            this.tokenCardInventoryDTOBindingSource.DataSource = typeof(Semnox.Parafait.Customer.Accounts.TokenCardInventoryDTO);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(254, 40);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // BtnGenerate
            // 
            this.BtnGenerate.Location = new System.Drawing.Point(8, 40);
            this.BtnGenerate.Name = "BtnGenerate";
            this.BtnGenerate.Size = new System.Drawing.Size(75, 25);
            this.BtnGenerate.TabIndex = 23;
            this.BtnGenerate.Text = "Generate";
            this.BtnGenerate.UseVisualStyleBackColor = true;
            this.BtnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // dtp_toDate
            // 
            this.dtp_toDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_toDate.Location = new System.Drawing.Point(321, 11);
            this.dtp_toDate.Name = "dtp_toDate";
            this.dtp_toDate.Size = new System.Drawing.Size(86, 20);
            this.dtp_toDate.TabIndex = 22;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(295, 14);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 13);
            this.label12.TabIndex = 21;
            this.label12.Text = "To:";
            // 
            // dtp_fromDate
            // 
            this.dtp_fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_fromDate.Location = new System.Drawing.Point(203, 11);
            this.dtp_fromDate.Name = "dtp_fromDate";
            this.dtp_fromDate.Size = new System.Drawing.Size(86, 20);
            this.dtp_fromDate.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(168, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "From:";
            // 
            // rboPeriod
            // 
            this.rboPeriod.AutoSize = true;
            this.rboPeriod.Checked = true;
            this.rboPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rboPeriod.Location = new System.Drawing.Point(82, 12);
            this.rboPeriod.Name = "rboPeriod";
            this.rboPeriod.Size = new System.Drawing.Size(83, 17);
            this.rboPeriod.TabIndex = 18;
            this.rboPeriod.TabStop = true;
            this.rboPeriod.Text = "For Period";
            this.rboPeriod.UseVisualStyleBackColor = true;
            // 
            // rboTillDate
            // 
            this.rboTillDate.AutoSize = true;
            this.rboTillDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rboTillDate.Location = new System.Drawing.Point(7, 12);
            this.rboTillDate.Name = "rboTillDate";
            this.rboTillDate.Size = new System.Drawing.Size(73, 17);
            this.rboTillDate.TabIndex = 17;
            this.rboTillDate.Text = "Till Date";
            this.rboTillDate.UseVisualStyleBackColor = true;
            // 
            // BtnDisk
            // 
            this.BtnDisk.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnDisk.FlatAppearance.BorderSize = 0;
            this.BtnDisk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDisk.ForeColor = System.Drawing.Color.Black;
            this.BtnDisk.Location = new System.Drawing.Point(113, 40);
            this.BtnDisk.Name = "BtnDisk";
            this.BtnDisk.Size = new System.Drawing.Size(114, 25);
            this.BtnDisk.TabIndex = 16;
            this.BtnDisk.Text = "Save to Disk";
            this.BtnDisk.UseVisualStyleBackColor = true;
            this.BtnDisk.Click += new System.EventHandler(this.BtnDisk_Click);
            // 
            // tp_CardInventory
            // 
            this.tp_CardInventory.BackColor = System.Drawing.Color.White;
            this.tp_CardInventory.Controls.Add(this.btnTokenCancel);
            this.tp_CardInventory.Controls.Add(this.btnTokenSave);
            this.tp_CardInventory.Controls.Add(this.grp1);
            this.tp_CardInventory.Controls.Add(this.lblTechName);
            this.tp_CardInventory.Controls.Add(this.lblWeekendDate);
            this.tp_CardInventory.Controls.Add(this.lblSiteName);
            this.tp_CardInventory.Controls.Add(this.lblWeekend);
            this.tp_CardInventory.Controls.Add(this.label14);
            this.tp_CardInventory.Controls.Add(this.lblLocation);
            this.tp_CardInventory.Location = new System.Drawing.Point(4, 25);
            this.tp_CardInventory.Name = "tp_CardInventory";
            this.tp_CardInventory.Padding = new System.Windows.Forms.Padding(3);
            this.tp_CardInventory.Size = new System.Drawing.Size(751, 428);
            this.tp_CardInventory.TabIndex = 3;
            this.tp_CardInventory.Text = "Token/Card Inventory";
            // 
            // btnTokenCancel
            // 
            this.btnTokenCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTokenCancel.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.normal2;
            this.btnTokenCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTokenCancel.FlatAppearance.BorderSize = 0;
            this.btnTokenCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTokenCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTokenCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTokenCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnTokenCancel.ForeColor = System.Drawing.Color.White;
            this.btnTokenCancel.Location = new System.Drawing.Point(435, 371);
            this.btnTokenCancel.Name = "btnTokenCancel";
            this.btnTokenCancel.Size = new System.Drawing.Size(133, 48);
            this.btnTokenCancel.TabIndex = 8;
            this.btnTokenCancel.Text = "Cancel";
            this.btnTokenCancel.UseVisualStyleBackColor = true;
            this.btnTokenCancel.Click += new System.EventHandler(this.btnTokenCancel_Click);
            // 
            // btnTokenSave
            // 
            this.btnTokenSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTokenSave.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.normal2;
            this.btnTokenSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTokenSave.FlatAppearance.BorderSize = 0;
            this.btnTokenSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTokenSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTokenSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTokenSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnTokenSave.ForeColor = System.Drawing.Color.White;
            this.btnTokenSave.Location = new System.Drawing.Point(214, 371);
            this.btnTokenSave.Name = "btnTokenSave";
            this.btnTokenSave.Size = new System.Drawing.Size(133, 48);
            this.btnTokenSave.TabIndex = 7;
            this.btnTokenSave.Text = "Save";
            this.btnTokenSave.UseVisualStyleBackColor = true;
            this.btnTokenSave.Click += new System.EventHandler(this.btnTokenSave_Click);
            // 
            // grp1
            // 
            this.grp1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grp1.Controls.Add(this.txtCardPurchased);
            this.grp1.Controls.Add(this.txtCardsOnHand);
            this.grp1.Controls.Add(this.txtTokenPOS);
            this.grp1.Controls.Add(this.txtTransferredToken);
            this.grp1.Controls.Add(this.txtTokenHand);
            this.grp1.Controls.Add(this.txtTokenKiosk);
            this.grp1.Controls.Add(this.label15);
            this.grp1.Controls.Add(this.lblCardsPurchased);
            this.grp1.Controls.Add(this.label18);
            this.grp1.Controls.Add(this.label23);
            this.grp1.Controls.Add(this.label24);
            this.grp1.Controls.Add(this.label25);
            this.grp1.Location = new System.Drawing.Point(19, 97);
            this.grp1.Name = "grp1";
            this.grp1.Size = new System.Drawing.Size(715, 263);
            this.grp1.TabIndex = 9;
            this.grp1.TabStop = false;
            // 
            // txtCardPurchased
            // 
            this.txtCardPurchased.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtCardPurchased.Location = new System.Drawing.Point(283, 210);
            this.txtCardPurchased.Name = "txtCardPurchased";
            this.txtCardPurchased.Size = new System.Drawing.Size(190, 26);
            this.txtCardPurchased.TabIndex = 6;
            this.txtCardPurchased.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNoOfCards_KeyPress);
            this.txtCardPurchased.Validating += new System.ComponentModel.CancelEventHandler(this.txtCardPurchased_Validating);
            // 
            // txtCardsOnHand
            // 
            this.txtCardsOnHand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtCardsOnHand.Location = new System.Drawing.Point(283, 172);
            this.txtCardsOnHand.Name = "txtCardsOnHand";
            this.txtCardsOnHand.Size = new System.Drawing.Size(190, 26);
            this.txtCardsOnHand.TabIndex = 5;
            this.txtCardsOnHand.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNoOfCards_KeyPress);
            this.txtCardsOnHand.Validating += new System.ComponentModel.CancelEventHandler(this.txtCardsOnHand_Validating);
            // 
            // txtTokenPOS
            // 
            this.txtTokenPOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtTokenPOS.Location = new System.Drawing.Point(283, 62);
            this.txtTokenPOS.Name = "txtTokenPOS";
            this.txtTokenPOS.Size = new System.Drawing.Size(190, 26);
            this.txtTokenPOS.TabIndex = 2;
            this.txtTokenPOS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNoOfCards_KeyPress);
            this.txtTokenPOS.Validating += new System.ComponentModel.CancelEventHandler(this.txtTokenPOS_Validating);
            // 
            // txtTransferredToken
            // 
            this.txtTransferredToken.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtTransferredToken.Location = new System.Drawing.Point(283, 136);
            this.txtTransferredToken.Name = "txtTransferredToken";
            this.txtTransferredToken.Size = new System.Drawing.Size(190, 26);
            this.txtTransferredToken.TabIndex = 4;
            this.txtTransferredToken.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTransferredToken_KeyPress);
            this.txtTransferredToken.Validating += new System.ComponentModel.CancelEventHandler(this.txtTransferredToken_Validating);
            // 
            // txtTokenHand
            // 
            this.txtTokenHand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtTokenHand.Location = new System.Drawing.Point(283, 98);
            this.txtTokenHand.Name = "txtTokenHand";
            this.txtTokenHand.Size = new System.Drawing.Size(190, 26);
            this.txtTokenHand.TabIndex = 3;
            this.txtTokenHand.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNoOfCards_KeyPress);
            this.txtTokenHand.Validating += new System.ComponentModel.CancelEventHandler(this.txtTokenHand_Validating);
            // 
            // txtTokenKiosk
            // 
            this.txtTokenKiosk.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtTokenKiosk.Location = new System.Drawing.Point(283, 26);
            this.txtTokenKiosk.Name = "txtTokenKiosk";
            this.txtTokenKiosk.Size = new System.Drawing.Size(190, 26);
            this.txtTokenKiosk.TabIndex = 1;
            this.txtTokenKiosk.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNoOfCards_KeyPress);
            this.txtTokenKiosk.Validating += new System.ComponentModel.CancelEventHandler(this.txtTokenKiosk_Validating);
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label15.Location = new System.Drawing.Point(6, 174);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(274, 20);
            this.label15.TabIndex = 14;
            this.label15.Text = "Total Cards on Hand:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCardsPurchased
            // 
            this.lblCardsPurchased.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblCardsPurchased.Location = new System.Drawing.Point(6, 212);
            this.lblCardsPurchased.Name = "lblCardsPurchased";
            this.lblCardsPurchased.Size = new System.Drawing.Size(274, 20);
            this.lblCardsPurchased.TabIndex = 13;
            this.lblCardsPurchased.Text = "Cards Purchased:";
            this.lblCardsPurchased.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label18.Location = new System.Drawing.Point(6, 138);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(274, 20);
            this.label18.TabIndex = 12;
            this.label18.Text = "Transferred Tokens:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label23.Location = new System.Drawing.Point(10, 64);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(270, 20);
            this.label23.TabIndex = 11;
            this.label23.Text = "Token Collected from Pos:";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label24.Location = new System.Drawing.Point(6, 100);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(274, 20);
            this.label24.TabIndex = 10;
            this.label24.Text = "Remaining on Hand Token:";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label25.Location = new System.Drawing.Point(6, 27);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(274, 20);
            this.label25.TabIndex = 9;
            this.label25.Text = "Token Collected from Kiosk:";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTechName
            // 
            this.lblTechName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblTechName.ForeColor = System.Drawing.Color.DimGray;
            this.lblTechName.Location = new System.Drawing.Point(297, 79);
            this.lblTechName.Name = "lblTechName";
            this.lblTechName.Size = new System.Drawing.Size(448, 20);
            this.lblTechName.TabIndex = 8;
            this.lblTechName.Text = "Username";
            this.lblTechName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWeekendDate
            // 
            this.lblWeekendDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblWeekendDate.ForeColor = System.Drawing.Color.DimGray;
            this.lblWeekendDate.Location = new System.Drawing.Point(297, 53);
            this.lblWeekendDate.Name = "lblWeekendDate";
            this.lblWeekendDate.Size = new System.Drawing.Size(446, 20);
            this.lblWeekendDate.TabIndex = 7;
            this.lblWeekendDate.Text = "Weekend Date";
            this.lblWeekendDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSiteName
            // 
            this.lblSiteName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.DimGray;
            this.lblSiteName.Location = new System.Drawing.Point(297, 26);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(443, 20);
            this.lblSiteName.TabIndex = 6;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWeekend
            // 
            this.lblWeekend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeekend.Location = new System.Drawing.Point(19, 52);
            this.lblWeekend.Name = "lblWeekend";
            this.lblWeekend.Size = new System.Drawing.Size(279, 20);
            this.lblWeekend.TabIndex = 4;
            this.lblWeekend.Text = "Week Ending:";
            this.lblWeekend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(19, 79);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(279, 20);
            this.label14.TabIndex = 3;
            this.label14.Text = "Technician Name:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLocation
            // 
            this.lblLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.Location = new System.Drawing.Point(19, 25);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(279, 20);
            this.lblLocation.TabIndex = 2;
            this.lblLocation.Text = "Location:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCardBalance
            // 
            this.lblCardBalance.AutoSize = true;
            this.lblCardBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardBalance.Location = new System.Drawing.Point(486, 460);
            this.lblCardBalance.Name = "lblCardBalance";
            this.lblCardBalance.Size = new System.Drawing.Size(138, 13);
            this.lblCardBalance.TabIndex = 14;
            this.lblCardBalance.Text = "Card Balance in Stock:";
            // 
            // lb_cards
            // 
            this.lb_cards.AutoSize = true;
            this.lb_cards.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_cards.Location = new System.Drawing.Point(622, 460);
            this.lb_cards.Name = "lb_cards";
            this.lb_cards.Size = new System.Drawing.Size(48, 13);
            this.lb_cards.TabIndex = 14;
            this.lb_cards.Text = "label14";
            // 
            // lblCardInventory
            // 
            this.lblCardInventory.AutoSize = true;
            this.lblCardInventory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardInventory.Location = new System.Drawing.Point(7, 459);
            this.lblCardInventory.Name = "lblCardInventory";
            this.lblCardInventory.Size = new System.Drawing.Size(139, 13);
            this.lblCardInventory.TabIndex = 15;
            this.lblCardInventory.Text = "Card inventory till date:";
            // 
            // lb_inventory
            // 
            this.lb_inventory.AutoSize = true;
            this.lb_inventory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_inventory.Location = new System.Drawing.Point(146, 460);
            this.lb_inventory.Name = "lb_inventory";
            this.lb_inventory.Size = new System.Drawing.Size(64, 13);
            this.lb_inventory.TabIndex = 16;
            this.lb_inventory.Text = "lb_inventory";
            // 
            // lb_issued_td
            // 
            this.lb_issued_td.AutoSize = true;
            this.lb_issued_td.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_issued_td.Location = new System.Drawing.Point(376, 460);
            this.lb_issued_td.Name = "lb_issued_td";
            this.lb_issued_td.Size = new System.Drawing.Size(66, 13);
            this.lb_issued_td.TabIndex = 18;
            this.lb_issued_td.Text = "lb_issued_td";
            // 
            // lblCardIssued
            // 
            this.lblCardIssued.AutoSize = true;
            this.lblCardIssued.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardIssued.Location = new System.Drawing.Point(245, 460);
            this.lblCardIssued.Name = "lblCardIssued";
            this.lblCardIssued.Size = new System.Drawing.Size(129, 13);
            this.lblCardIssued.TabIndex = 17;
            this.lblCardIssued.Text = "Cards issued till date:";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(8, 458);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 16);
            this.lblMessage.TabIndex = 16;
            // 
            // frmTokenCardInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 479);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lb_issued_td);
            this.Controls.Add(this.lblCardIssued);
            this.Controls.Add(this.lb_inventory);
            this.Controls.Add(this.lblCardInventory);
            this.Controls.Add(this.lb_cards);
            this.Controls.Add(this.lblCardBalance);
            this.Controls.Add(this.tc_cards);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTokenCardInventory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cards Management";
            this.Load += new System.EventHandler(this.frm_cardMaintenance_Load);
            this.tc_cards.ResumeLayout(false);
            this.tp_add.ResumeLayout(false);
            this.tp_add.PerformLayout();
            this.tp_delete.ResumeLayout(false);
            this.tp_delete.PerformLayout();
            this.tp_reports.ResumeLayout(false);
            this.tp_reports.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tokenCardInventoryDTOBindingSource)).EndInit();
            this.tp_CardInventory.ResumeLayout(false);
            this.grp1.ResumeLayout(false);
            this.grp1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tc_cards;
        private System.Windows.Forms.TabPage tp_add;
        private System.Windows.Forms.TabPage tp_delete;
        private System.Windows.Forms.TabPage tp_reports;
        private System.Windows.Forms.Label lblAddCards;
        private System.Windows.Forms.TextBox txtNoOfCards;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtToSerial;
        private System.Windows.Forms.Label lblfrmSerNo;
        private System.Windows.Forms.TextBox txtFromSerial;
        private System.Windows.Forms.Label lblRecDate;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lb_del_date;
        private System.Windows.Forms.Label lb_del_user;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDelNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnDelSave;
        private System.Windows.Forms.Label lblCardBalance;
        private System.Windows.Forms.Label lb_cards;
        private System.Windows.Forms.Button BtnDisk;
        private System.Windows.Forms.Button btnCardSave;
        private System.Windows.Forms.Button BtnCardClose;
        private System.Windows.Forms.Label lblCardInventory;
        private System.Windows.Forms.Label lb_inventory;
        private System.Windows.Forms.Label lb_issued_td;
        private System.Windows.Forms.Label lblCardIssued;
        private System.Windows.Forms.Button BtnGenerate;
        private System.Windows.Forms.DateTimePicker dtp_toDate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker dtp_fromDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton rboPeriod;
        private System.Windows.Forms.RadioButton rboTillDate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabPage tp_CardInventory;
        private System.Windows.Forms.Label lblTechName;
        private System.Windows.Forms.Label lblWeekendDate;
        private System.Windows.Forms.Label lblSiteName;
        private System.Windows.Forms.Label lblWeekend;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.GroupBox grp1;
        private System.Windows.Forms.TextBox txtCardsOnHand;
        private System.Windows.Forms.TextBox txtTokenPOS;
        private System.Windows.Forms.TextBox txtTransferredToken;
        private System.Windows.Forms.TextBox txtTokenHand;
        private System.Windows.Forms.TextBox txtTokenKiosk;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblCardsPurchased;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txtCardPurchased;
        private System.Windows.Forms.Button btnTokenSave;
        private System.Windows.Forms.Button btnTokenCancel;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.BindingSource tokenCardInventoryDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromSerialNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toserialNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn actionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn actiondateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn actionByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn machineTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn activityTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label lblMessage;
    }
}