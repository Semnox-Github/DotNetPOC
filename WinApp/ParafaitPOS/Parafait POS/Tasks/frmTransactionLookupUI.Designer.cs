namespace Parafait_POS
{
    partial class frmTransactionLookupUI
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
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.lblReference = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtOTP = new System.Windows.Forms.TextBox();
            this.lblOTP = new System.Windows.Forms.Label();
            this.txtTrxId = new System.Windows.Forms.TextBox();
            this.txtTrxNumber = new System.Windows.Forms.TextBox();
            this.lblTrxNumber = new System.Windows.Forms.Label();
            this.lblTrxId = new System.Windows.Forms.Label();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.btnKeyBoard = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCustomerLookup = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbToAM = new System.Windows.Forms.ComboBox();
            this.cmbToMin = new System.Windows.Forms.ComboBox();
            this.cmbToHour = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbFromAM = new System.Windows.Forms.ComboBox();
            this.cmbFromMin = new System.Windows.Forms.ComboBox();
            this.cmbFromHour = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.cmbUser = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.SuspendLayout();
            // 
            // txtCustomer
            // 
            this.txtCustomer.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.txtCustomer.Location = new System.Drawing.Point(513, 136);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(199, 26);
            this.txtCustomer.TabIndex = 4;
            this.txtCustomer.Enter += new System.EventHandler(this.SetCurrentTextBox_Enter);
            this.txtCustomer.Leave += new System.EventHandler(this.txtCustomer_Leave);
            // 
            // lblCustomer
            // 
            this.lblCustomer.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomer.Location = new System.Drawing.Point(400, 141);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(102, 15);
            this.lblCustomer.TabIndex = 42;
            this.lblCustomer.Text = "Customer Name:";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReference
            // 
            this.txtReference.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.txtReference.Location = new System.Drawing.Point(145, 177);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(199, 26);
            this.txtReference.TabIndex = 6;
            this.txtReference.Enter += new System.EventHandler(this.SetCurrentTextBox_Enter);
            // 
            // lblReference
            // 
            this.lblReference.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblReference.Location = new System.Drawing.Point(65, 181);
            this.lblReference.Name = "lblReference";
            this.lblReference.Size = new System.Drawing.Size(69, 15);
            this.lblReference.TabIndex = 39;
            this.lblReference.Text = "Reference:";
            this.lblReference.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(513, 176);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(199, 27);
            this.cmbStatus.TabIndex = 9;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(453, 181);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 15);
            this.lblStatus.TabIndex = 34;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOTP
            // 
            this.txtOTP.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.txtOTP.Location = new System.Drawing.Point(145, 136);
            this.txtOTP.Name = "txtOTP";
            this.txtOTP.Size = new System.Drawing.Size(199, 26);
            this.txtOTP.TabIndex = 8;
            this.txtOTP.Enter += new System.EventHandler(this.SetCurrentTextBox_Enter);
            // 
            // lblOTP
            // 
            this.lblOTP.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblOTP.Location = new System.Drawing.Point(100, 142);
            this.lblOTP.Name = "lblOTP";
            this.lblOTP.Size = new System.Drawing.Size(34, 15);
            this.lblOTP.TabIndex = 32;
            this.lblOTP.Text = "OTP:";
            this.lblOTP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTrxId
            // 
            this.txtTrxId.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.txtTrxId.Location = new System.Drawing.Point(146, 95);
            this.txtTrxId.Name = "txtTrxId";
            this.txtTrxId.Size = new System.Drawing.Size(199, 26);
            this.txtTrxId.TabIndex = 3;
            this.txtTrxId.Enter += new System.EventHandler(this.SetCurrentTextBox_Enter);
            // 
            // txtTrxNumber
            // 
            this.txtTrxNumber.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.txtTrxNumber.Location = new System.Drawing.Point(513, 95);
            this.txtTrxNumber.Name = "txtTrxNumber";
            this.txtTrxNumber.Size = new System.Drawing.Size(199, 26);
            this.txtTrxNumber.TabIndex = 5;
            this.txtTrxNumber.Enter += new System.EventHandler(this.SetCurrentTextBox_Enter);
            // 
            // lblTrxNumber
            // 
            this.lblTrxNumber.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblTrxNumber.Location = new System.Drawing.Point(456, 99);
            this.lblTrxNumber.Name = "lblTrxNumber";
            this.lblTrxNumber.Size = new System.Drawing.Size(46, 15);
            this.lblTrxNumber.TabIndex = 29;
            this.lblTrxNumber.Text = "Trx No:";
            this.lblTrxNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTrxId
            // 
            this.lblTrxId.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblTrxId.Location = new System.Drawing.Point(95, 100);
            this.lblTrxId.Name = "lblTrxId";
            this.lblTrxId.Size = new System.Drawing.Size(41, 15);
            this.lblTrxId.TabIndex = 28;
            this.lblTrxId.Text = "Trx Id:";
            this.lblTrxId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblToDate
            // 
            this.lblToDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblToDate.Location = new System.Drawing.Point(70, 53);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(65, 27);
            this.lblToDate.TabIndex = 26;
            this.lblToDate.Text = "To Date:";
            this.lblToDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtpToDate.Location = new System.Drawing.Point(147, 53);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(199, 25);
            this.dtpToDate.TabIndex = 2;
            this.dtpToDate.ValueChanged += new System.EventHandler(this.IsDateTimeChanged);
            // 
            // lblFromDate
            // 
            this.lblFromDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblFromDate.Location = new System.Drawing.Point(69, 16);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(68, 15);
            this.lblFromDate.TabIndex = 24;
            this.lblFromDate.Text = "From Date:";
            this.lblFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dtpFromDate.Location = new System.Drawing.Point(147, 12);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(199, 25);
            this.dtpFromDate.TabIndex = 1;
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.IsDateTimeChanged);
            // 
            // btnKeyBoard
            // 
            this.btnKeyBoard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnKeyBoard.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnKeyBoard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnKeyBoard.FlatAppearance.BorderSize = 0;
            this.btnKeyBoard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyBoard.Location = new System.Drawing.Point(654, 264);
            this.btnKeyBoard.Name = "btnKeyBoard";
            this.btnKeyBoard.Size = new System.Drawing.Size(57, 60);
            this.btnKeyBoard.TabIndex = 13;
            this.btnKeyBoard.UseVisualStyleBackColor = true;
            this.btnKeyBoard.Click += new System.EventHandler(this.btnKeyBoard_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(401, 278);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(108, 42);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCustomerLookup
            // 
            this.btnCustomerLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCustomerLookup.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCustomerLookup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCustomerLookup.FlatAppearance.BorderSize = 0;
            this.btnCustomerLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerLookup.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerLookup.ForeColor = System.Drawing.Color.White;
            this.btnCustomerLookup.Location = new System.Drawing.Point(737, 136);
            this.btnCustomerLookup.Name = "btnCustomerLookup";
            this.btnCustomerLookup.Size = new System.Drawing.Size(104, 36);
            this.btnCustomerLookup.TabIndex = 7;
            this.btnCustomerLookup.Text = "Lookup";
            this.btnCustomerLookup.UseVisualStyleBackColor = true;
            this.btnCustomerLookup.Click += new System.EventHandler(this.btnCustomerLookup_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(272, 278);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(108, 42);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(141, 278);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(108, 42);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbToAM
            // 
            this.cmbToAM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToAM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbToAM.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbToAM.FormattingEnabled = true;
            this.cmbToAM.Items.AddRange(new object[] {
            "AM",
            "PM"});
            this.cmbToAM.Location = new System.Drawing.Point(479, 53);
            this.cmbToAM.Name = "cmbToAM";
            this.cmbToAM.Size = new System.Drawing.Size(50, 25);
            this.cmbToAM.TabIndex = 48;
            this.cmbToAM.SelectionChangeCommitted += new System.EventHandler(this.IsDateTimeChanged);
            // 
            // cmbToMin
            // 
            this.cmbToMin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbToMin.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbToMin.FormattingEnabled = true;
            this.cmbToMin.Items.AddRange(new object[] {
            "00",
            "15",
            "30",
            "45"});
            this.cmbToMin.Location = new System.Drawing.Point(424, 53);
            this.cmbToMin.Name = "cmbToMin";
            this.cmbToMin.Size = new System.Drawing.Size(50, 25);
            this.cmbToMin.TabIndex = 47;
            this.cmbToMin.SelectionChangeCommitted += new System.EventHandler(this.IsDateTimeChanged);
            // 
            // cmbToHour
            // 
            this.cmbToHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToHour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbToHour.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbToHour.FormattingEnabled = true;
            this.cmbToHour.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cmbToHour.Location = new System.Drawing.Point(367, 53);
            this.cmbToHour.Name = "cmbToHour";
            this.cmbToHour.Size = new System.Drawing.Size(50, 25);
            this.cmbToHour.TabIndex = 46;
            this.cmbToHour.SelectionChangeCommitted += new System.EventHandler(this.IsDateTimeChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(415, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(12, 18);
            this.label8.TabIndex = 50;
            this.label8.Text = ":";
            // 
            // cmbFromAM
            // 
            this.cmbFromAM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromAM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFromAM.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFromAM.FormattingEnabled = true;
            this.cmbFromAM.Items.AddRange(new object[] {
            "AM",
            "PM"});
            this.cmbFromAM.Location = new System.Drawing.Point(479, 12);
            this.cmbFromAM.Name = "cmbFromAM";
            this.cmbFromAM.Size = new System.Drawing.Size(50, 25);
            this.cmbFromAM.TabIndex = 45;
            this.cmbFromAM.SelectionChangeCommitted += new System.EventHandler(this.IsDateTimeChanged);
            // 
            // cmbFromMin
            // 
            this.cmbFromMin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFromMin.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFromMin.FormattingEnabled = true;
            this.cmbFromMin.Items.AddRange(new object[] {
            "00",
            "15",
            "30",
            "45"});
            this.cmbFromMin.Location = new System.Drawing.Point(424, 12);
            this.cmbFromMin.Name = "cmbFromMin";
            this.cmbFromMin.Size = new System.Drawing.Size(50, 25);
            this.cmbFromMin.TabIndex = 44;
            this.cmbFromMin.SelectionChangeCommitted += new System.EventHandler(this.IsDateTimeChanged);
            // 
            // cmbFromHour
            // 
            this.cmbFromHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromHour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFromHour.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFromHour.FormattingEnabled = true;
            this.cmbFromHour.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cmbFromHour.Location = new System.Drawing.Point(367, 12);
            this.cmbFromHour.Name = "cmbFromHour";
            this.cmbFromHour.Size = new System.Drawing.Size(50, 25);
            this.cmbFromHour.TabIndex = 43;
            this.cmbFromHour.SelectionChangeCommitted += new System.EventHandler(this.IsDateTimeChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(415, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 18);
            this.label7.TabIndex = 49;
            this.label7.Text = ":";
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblErrorMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblErrorMessage.ForeColor = System.Drawing.Color.Black;
            this.lblErrorMessage.Location = new System.Drawing.Point(144, 324);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(568, 30);
            this.lblErrorMessage.TabIndex = 51;
            // 
            // lblUser
            // 
            this.lblUser.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblUser.Location = new System.Drawing.Point(79, 224);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(54, 15);
            this.lblUser.TabIndex = 53;
            this.lblUser.Text = "Cashier:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbUser
            // 
            this.cmbUser.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbUser.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbUser.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.Location = new System.Drawing.Point(145, 219);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(199, 27);
            this.cmbUser.TabIndex = 54;
            // 
            // frmTransactionLookupUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(896, 358);
            this.ControlBox = false;
            this.Controls.Add(this.cmbUser);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.lblErrorMessage);
            this.Controls.Add(this.cmbToAM);
            this.Controls.Add(this.cmbToMin);
            this.Controls.Add(this.cmbToHour);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbFromAM);
            this.Controls.Add(this.cmbFromMin);
            this.Controls.Add(this.cmbFromHour);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCustomerLookup);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnKeyBoard);
            this.Controls.Add(this.txtCustomer);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.txtReference);
            this.Controls.Add(this.lblReference);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtOTP);
            this.Controls.Add(this.lblOTP);
            this.Controls.Add(this.txtTrxId);
            this.Controls.Add(this.txtTrxNumber);
            this.Controls.Add(this.lblTrxNumber);
            this.Controls.Add(this.lblTrxId);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.lblFromDate);
            this.Controls.Add(this.dtpFromDate);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTransactionLookupUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction Lookup ";
            this.Load += new System.EventHandler(this.frmTransactionLookupUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.Label lblReference;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtOTP;
        private System.Windows.Forms.Label lblOTP;
        private System.Windows.Forms.TextBox txtTrxId;
        private System.Windows.Forms.TextBox txtTrxNumber;
        private System.Windows.Forms.Label lblTrxNumber;
        private System.Windows.Forms.Label lblTrxId;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Button btnKeyBoard;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCustomerLookup;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbToAM;
        private System.Windows.Forms.ComboBox cmbToMin;
        private System.Windows.Forms.ComboBox cmbToHour;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbFromAM;
        private System.Windows.Forms.ComboBox cmbFromMin;
        private System.Windows.Forms.ComboBox cmbFromHour;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblErrorMessage;
        private System.Windows.Forms.Label lblUser;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbUser;
    }
}