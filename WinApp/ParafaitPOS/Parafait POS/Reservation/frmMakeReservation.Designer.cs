using Semnox.Core.GenericUtilities;

namespace Parafait_POS.Reservation
{
    partial class frmMakeReservation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMakeReservation));
            this.cmbBookingClass = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpforDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.cmbFromTime = new System.Windows.Forms.ComboBox();
            this.nudQuantity = new System.Windows.Forms.NumericUpDown();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.btnBook = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.Panel();
            this.lblAppliedTrxProfileName = new System.Windows.Forms.Label();
            this.lblAppliedTrxProfileID = new System.Windows.Forms.Label();
            this.btnTrxProfiles = new System.Windows.Forms.Button();
            this.btnAddAttendeesInSummary = new System.Windows.Forms.Button();
            this.btnDiscounts = new System.Windows.Forms.Button();
            this.txtBalanceAmount = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txtTransactionAmount = new System.Windows.Forms.TextBox();
            this.lblTransactionAmount = new System.Windows.Forms.Label();
            this.txtDiscount = new System.Windows.Forms.TextBox();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.txtAdvanceAmount = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnPayment = new System.Windows.Forms.Button();
            this.txtPaidAmount = new System.Windows.Forms.TextBox();
            this.cbxEmailSent = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblExpires = new System.Windows.Forms.Label();
            this.txtBookingEstimate = new System.Windows.Forms.TextBox();
            this.lblReservationCode = new System.Windows.Forms.Label();
            this.btnEmail = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnCancelBooking = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvPackageDetails = new System.Windows.Forms.DataGridView();
            this.btnAttendeeDetails = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbToTime = new System.Windows.Forms.ComboBox();
            this.cmbRecurFrequency = new System.Windows.Forms.ComboBox();
            this.dtpRecurUntil = new System.Windows.Forms.DateTimePicker();
            this.label18 = new System.Windows.Forms.Label();
            this.dgvBookingProducts = new System.Windows.Forms.DataGridView();
            this.dcDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dcProduct = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dcQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcBookingProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AdditionalDiscountName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.additionalTransactionProfileId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dcRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tcBooking = new System.Windows.Forms.TabControl();
            this.tpDateTime = new System.Windows.Forms.TabPage();
            this.cmbToTimeForSearch = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.cmbFromTimeForSearch = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblFromTimeSearch = new System.Windows.Forms.Label();
            this.lblToTimeSearch = new System.Windows.Forms.Label();
            this.attrScheduleHScrollBar = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.dgvAttractionSchedules = new System.Windows.Forms.DataGridView();
            this.lblTimeSlots = new System.Windows.Forms.Label();
            this.btnIncreaseGuestCount = new System.Windows.Forms.Button();
            this.btnReduceGuestCount = new System.Windows.Forms.Button();
            this.txtGuests = new System.Windows.Forms.TextBox();
            this.cbxNight = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cbxAfternoon = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cbxMorning = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cbxEarlyMorning = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.btnBlockSchedule = new System.Windows.Forms.Button();
            this.cmbFacility = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblFacility = new System.Windows.Forms.Label();
            this.attrScheduleVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.lblSchedule = new System.Windows.Forms.Label();
            this.cmbChannel = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBookingName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.btnNextDateTime = new System.Windows.Forms.Button();
            this.cbxRecur = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.tpCustomer = new System.Windows.Forms.TabPage();
            this.custPanelVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.pnlCustomerDetail = new System.Windows.Forms.Panel();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.btnCustomerLookup = new System.Windows.Forms.Button();
            this.lnkUpdate = new System.Windows.Forms.LinkLabel();
            this.btnPrevCustomer = new System.Windows.Forms.Button();
            this.btnNextCustomer = new System.Windows.Forms.Button();
            this.tpPackage = new System.Windows.Forms.TabPage();
            this.packageListHScrollBar = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.dgvPackageList = new System.Windows.Forms.DataGridView();
            this.SelectProduct = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.packageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Product_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.package_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discountName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.transactionProfileId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SelectedStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.packageListVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.btnPrevPackage = new System.Windows.Forms.Button();
            this.btnNextPackage = new System.Windows.Forms.Button();
            this.cmbPackage = new System.Windows.Forms.ComboBox();
            this.tpAdditional = new System.Windows.Forms.TabPage();
            this.additionalProdListHScrollbar = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.additionProdListVScrollbar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.btnAddServiceCharge = new System.Windows.Forms.Button();
            this.btnAddProducts = new System.Windows.Forms.Button();
            this.btnPrevAdditionalProducts = new System.Windows.Forms.Button();
            this.btnNextAdditionalProducts = new System.Windows.Forms.Button();
            this.tpConfirm = new System.Windows.Forms.TabPage();
            this.btnEditBooking = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrevConfirm = new System.Windows.Forms.Button();
            this.tpAuditTrail = new System.Windows.Forms.TabPage();
            this.auditTrailVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.dgvAuditTrail = new System.Windows.Forms.DataGridView();
            this.lblAuditTrail = new System.Windows.Forms.Label();
            this.eventLogBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackageDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookingProducts)).BeginInit();
            this.tcBooking.SuspendLayout();
            this.tpDateTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttractionSchedules)).BeginInit();
            this.tpCustomer.SuspendLayout();
            this.tpPackage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackageList)).BeginInit();
            this.tpAdditional.SuspendLayout();
            this.tpConfirm.SuspendLayout();
            this.tpAuditTrail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditTrail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventLogBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbBookingClass
            // 
            this.cmbBookingClass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBookingClass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBookingClass.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBookingClass.FormattingEnabled = true;
            this.cmbBookingClass.Items.AddRange(new object[] {
            "Facility",
            "Game"});
            this.cmbBookingClass.Location = new System.Drawing.Point(128, 144);
            this.cmbBookingClass.Name = "cmbBookingClass";
            this.cmbBookingClass.Size = new System.Drawing.Size(179, 31);
            this.cmbBookingClass.TabIndex = 14;
            this.cmbBookingClass.SelectedIndexChanged += new System.EventHandler(this.cmbPackage_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(68, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 30);
            this.label1.TabIndex = 7;
            this.label1.Text = "For Date:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpforDate
            // 
            this.dtpforDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpforDate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpforDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpforDate.Location = new System.Drawing.Point(128, 58);
            this.dtpforDate.Name = "dtpforDate";
            this.dtpforDate.Size = new System.Drawing.Size(214, 30);
            this.dtpforDate.TabIndex = 6;
            this.dtpforDate.ValueChanged += new System.EventHandler(this.dtpforDate_ValueChanged);
            this.dtpforDate.Enter += new System.EventHandler(this.dtpforDate_Enter);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(25, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 30);
            this.label3.TabIndex = 10;
            this.label3.Text = "Booking Product:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.Color.Transparent;
            this.btnFind.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFind.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnFind.FlatAppearance.BorderSize = 0;
            this.btnFind.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnFind.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFind.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.ForeColor = System.Drawing.Color.White;
            this.btnFind.Location = new System.Drawing.Point(313, 141);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(40, 36);
            this.btnFind.TabIndex = 15;
            this.btnFind.Text = "Info";
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            this.btnFind.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseDown);
            this.btnFind.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseUp);
            // 
            // cmbFromTime
            // 
            this.cmbFromTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromTime.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFromTime.FormattingEnabled = true;
            this.cmbFromTime.Location = new System.Drawing.Point(471, 144);
            this.cmbFromTime.Name = "cmbFromTime";
            this.cmbFromTime.Size = new System.Drawing.Size(116, 31);
            this.cmbFromTime.TabIndex = 16;
            this.cmbFromTime.SelectedIndexChanged += new System.EventHandler(this.cmbFromTime_SelectedIndexChanged);
            // 
            // nudQuantity
            // 
            this.nudQuantity.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudQuantity.Location = new System.Drawing.Point(139, 441);
            this.nudQuantity.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQuantity.Name = "nudQuantity";
            this.nudQuantity.Size = new System.Drawing.Size(62, 30);
            this.nudQuantity.TabIndex = 9;
            this.nudQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQuantity.Visible = false;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(138, 478);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(414, 56);
            this.txtRemarks.TabIndex = 29;
            // 
            // btnBook
            // 
            this.btnBook.BackColor = System.Drawing.Color.Transparent;
            this.btnBook.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnBook.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBook.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnBook.FlatAppearance.BorderSize = 0;
            this.btnBook.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBook.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBook.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBook.ForeColor = System.Drawing.Color.White;
            this.btnBook.Location = new System.Drawing.Point(318, 585);
            this.btnBook.Name = "btnBook";
            this.btnBook.Size = new System.Drawing.Size(116, 34);
            this.btnBook.TabIndex = 54;
            this.btnBook.Text = "Book";
            this.btnBook.UseVisualStyleBackColor = false;
            this.btnBook.Click += new System.EventHandler(this.btnBook_Click);
            this.btnBook.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnBook.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.MistyRose;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 669);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(1240, 22);
            this.txtMessage.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(359, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 30);
            this.label5.TabIndex = 22;
            this.label5.Text = "Reserved From:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(85, 441);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 30);
            this.label7.TabIndex = 25;
            this.label7.Text = "Guests:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label7.Visible = false;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(74, 476);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 15);
            this.label9.TabIndex = 27;
            this.label9.Text = "Remarks:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 30;
            this.label2.Text = "Select Package:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(279, 585);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 34);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnCancel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.DarkKhaki;
            this.groupBox1.Controls.Add(this.lblAppliedTrxProfileName);
            this.groupBox1.Controls.Add(this.lblAppliedTrxProfileID);
            this.groupBox1.Controls.Add(this.btnTrxProfiles);
            this.groupBox1.Controls.Add(this.btnAddAttendeesInSummary);
            this.groupBox1.Controls.Add(this.btnDiscounts);
            this.groupBox1.Controls.Add(this.txtBalanceAmount);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.txtTransactionAmount);
            this.groupBox1.Controls.Add(this.lblTransactionAmount);
            this.groupBox1.Controls.Add(this.txtDiscount);
            this.groupBox1.Controls.Add(this.lblDiscount);
            this.groupBox1.Controls.Add(this.btnPrint);
            this.groupBox1.Controls.Add(this.btnExecute);
            this.groupBox1.Controls.Add(this.txtAdvanceAmount);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.btnPayment);
            this.groupBox1.Controls.Add(this.txtPaidAmount);
            this.groupBox1.Controls.Add(this.cbxEmailSent);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.lblExpires);
            this.groupBox1.Controls.Add(this.txtBookingEstimate);
            this.groupBox1.Controls.Add(this.lblReservationCode);
            this.groupBox1.Controls.Add(this.btnEmail);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.btnConfirm);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Location = new System.Drawing.Point(283, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(476, 477);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.Text = "Status";
            // 
            // lblAppliedTrxProfileName
            // 
            this.lblAppliedTrxProfileName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppliedTrxProfileName.Location = new System.Drawing.Point(166, 323);
            this.lblAppliedTrxProfileName.Name = "lblAppliedTrxProfileName";
            this.lblAppliedTrxProfileName.Size = new System.Drawing.Size(238, 26);
            this.lblAppliedTrxProfileName.TabIndex = 70;
            this.lblAppliedTrxProfileName.Text = "Applied";
            this.lblAppliedTrxProfileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAppliedTrxProfileID
            // 
            this.lblAppliedTrxProfileID.AutoSize = true;
            this.lblAppliedTrxProfileID.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppliedTrxProfileID.Location = new System.Drawing.Point(28, 328);
            this.lblAppliedTrxProfileID.Name = "lblAppliedTrxProfileID";
            this.lblAppliedTrxProfileID.Size = new System.Drawing.Size(138, 18);
            this.lblAppliedTrxProfileID.TabIndex = 69;
            this.lblAppliedTrxProfileID.Text = "Trx Profile Applied:";
            this.lblAppliedTrxProfileID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTrxProfiles
            // 
            this.btnTrxProfiles.BackColor = System.Drawing.Color.Transparent;
            this.btnTrxProfiles.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnTrxProfiles.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTrxProfiles.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnTrxProfiles.FlatAppearance.BorderSize = 0;
            this.btnTrxProfiles.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTrxProfiles.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTrxProfiles.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTrxProfiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrxProfiles.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrxProfiles.ForeColor = System.Drawing.Color.White;
            this.btnTrxProfiles.Location = new System.Drawing.Point(31, 363);
            this.btnTrxProfiles.Name = "btnTrxProfiles";
            this.btnTrxProfiles.Size = new System.Drawing.Size(80, 41);
            this.btnTrxProfiles.TabIndex = 44;
            this.btnTrxProfiles.Text = "Transaction Profile";
            this.btnTrxProfiles.UseVisualStyleBackColor = false;
            this.btnTrxProfiles.Click += new System.EventHandler(this.btnTrxProfiles_Click);
            // 
            // btnAddAttendeesInSummary
            // 
            this.btnAddAttendeesInSummary.BackColor = System.Drawing.Color.Transparent;
            this.btnAddAttendeesInSummary.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnAddAttendeesInSummary.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddAttendeesInSummary.Enabled = false;
            this.btnAddAttendeesInSummary.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnAddAttendeesInSummary.FlatAppearance.BorderSize = 0;
            this.btnAddAttendeesInSummary.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAddAttendeesInSummary.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddAttendeesInSummary.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddAttendeesInSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddAttendeesInSummary.ForeColor = System.Drawing.Color.White;
            this.btnAddAttendeesInSummary.Location = new System.Drawing.Point(116, 412);
            this.btnAddAttendeesInSummary.Name = "btnAddAttendeesInSummary";
            this.btnAddAttendeesInSummary.Size = new System.Drawing.Size(80, 41);
            this.btnAddAttendeesInSummary.TabIndex = 49;
            this.btnAddAttendeesInSummary.Text = "Add Attendees";
            this.btnAddAttendeesInSummary.UseVisualStyleBackColor = false;
            this.btnAddAttendeesInSummary.Click += new System.EventHandler(this.btnAddAttendeesInSummary_Click);
            // 
            // btnDiscounts
            // 
            this.btnDiscounts.BackColor = System.Drawing.Color.Transparent;
            this.btnDiscounts.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnDiscounts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDiscounts.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnDiscounts.FlatAppearance.BorderSize = 0;
            this.btnDiscounts.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnDiscounts.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDiscounts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDiscounts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiscounts.ForeColor = System.Drawing.Color.White;
            this.btnDiscounts.Location = new System.Drawing.Point(31, 412);
            this.btnDiscounts.Name = "btnDiscounts";
            this.btnDiscounts.Size = new System.Drawing.Size(80, 41);
            this.btnDiscounts.TabIndex = 48;
            this.btnDiscounts.Text = "Discount";
            this.btnDiscounts.UseVisualStyleBackColor = false;
            this.btnDiscounts.Click += new System.EventHandler(this.btnDiscounts_Click);
            this.btnDiscounts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseDown);
            this.btnDiscounts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseUp);
            // 
            // txtBalanceAmount
            // 
            this.txtBalanceAmount.BackColor = System.Drawing.Color.Salmon;
            this.txtBalanceAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBalanceAmount.Location = new System.Drawing.Point(175, 186);
            this.txtBalanceAmount.Name = "txtBalanceAmount";
            this.txtBalanceAmount.ReadOnly = true;
            this.txtBalanceAmount.Size = new System.Drawing.Size(145, 30);
            this.txtBalanceAmount.TabIndex = 41;
            this.txtBalanceAmount.Text = "0.00";
            this.txtBalanceAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(73, 186);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(99, 30);
            this.label24.TabIndex = 64;
            this.label24.Text = "Balance Amount:";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTransactionAmount
            // 
            this.txtTransactionAmount.BackColor = System.Drawing.Color.Salmon;
            this.txtTransactionAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransactionAmount.Location = new System.Drawing.Point(175, 11);
            this.txtTransactionAmount.Name = "txtTransactionAmount";
            this.txtTransactionAmount.ReadOnly = true;
            this.txtTransactionAmount.Size = new System.Drawing.Size(145, 30);
            this.txtTransactionAmount.TabIndex = 36;
            this.txtTransactionAmount.Text = "0.00";
            this.txtTransactionAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTransactionAmount
            // 
            this.lblTransactionAmount.Location = new System.Drawing.Point(74, 11);
            this.lblTransactionAmount.Name = "lblTransactionAmount";
            this.lblTransactionAmount.Size = new System.Drawing.Size(98, 30);
            this.lblTransactionAmount.TabIndex = 62;
            this.lblTransactionAmount.Text = "Transaction Amt:";
            this.lblTransactionAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDiscount
            // 
            this.txtDiscount.BackColor = System.Drawing.Color.Salmon;
            this.txtDiscount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiscount.Location = new System.Drawing.Point(175, 116);
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.ReadOnly = true;
            this.txtDiscount.Size = new System.Drawing.Size(145, 30);
            this.txtDiscount.TabIndex = 39;
            this.txtDiscount.Text = "0.00";
            this.txtDiscount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblDiscount
            // 
            this.lblDiscount.Location = new System.Drawing.Point(69, 116);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(103, 30);
            this.lblDiscount.TabIndex = 60;
            this.lblDiscount.Text = "Discount Amount:";
            this.lblDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(199, 363);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 41);
            this.btnPrint.TabIndex = 46;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.btnPrint.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseDown);
            this.btnPrint.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseUp);
            // 
            // btnExecute
            // 
            this.btnExecute.BackColor = System.Drawing.Color.Transparent;
            this.btnExecute.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnExecute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExecute.Enabled = false;
            this.btnExecute.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnExecute.FlatAppearance.BorderSize = 0;
            this.btnExecute.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecute.ForeColor = System.Drawing.Color.White;
            this.btnExecute.Location = new System.Drawing.Point(371, 412);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(80, 41);
            this.btnExecute.TabIndex = 52;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            this.btnExecute.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseDown);
            this.btnExecute.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseUp);
            // 
            // txtAdvanceAmount
            // 
            this.txtAdvanceAmount.BackColor = System.Drawing.Color.Salmon;
            this.txtAdvanceAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdvanceAmount.Location = new System.Drawing.Point(175, 46);
            this.txtAdvanceAmount.Name = "txtAdvanceAmount";
            this.txtAdvanceAmount.ReadOnly = true;
            this.txtAdvanceAmount.Size = new System.Drawing.Size(145, 30);
            this.txtAdvanceAmount.TabIndex = 37;
            this.txtAdvanceAmount.Text = "0.00";
            this.txtAdvanceAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(65, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 30);
            this.label8.TabIndex = 58;
            this.label8.Text = "Minimum Adv. Amt:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPayment
            // 
            this.btnPayment.BackColor = System.Drawing.Color.Transparent;
            this.btnPayment.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnPayment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPayment.Enabled = false;
            this.btnPayment.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPayment.FlatAppearance.BorderSize = 0;
            this.btnPayment.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPayment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPayment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayment.ForeColor = System.Drawing.Color.White;
            this.btnPayment.Location = new System.Drawing.Point(201, 412);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(80, 41);
            this.btnPayment.TabIndex = 50;
            this.btnPayment.Text = "Payment";
            this.btnPayment.UseVisualStyleBackColor = false;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            // 
            // txtPaidAmount
            // 
            this.txtPaidAmount.BackColor = System.Drawing.Color.Salmon;
            this.txtPaidAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPaidAmount.Location = new System.Drawing.Point(175, 151);
            this.txtPaidAmount.Name = "txtPaidAmount";
            this.txtPaidAmount.ReadOnly = true;
            this.txtPaidAmount.Size = new System.Drawing.Size(145, 30);
            this.txtPaidAmount.TabIndex = 40;
            this.txtPaidAmount.Text = "0.00";
            this.txtPaidAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbxEmailSent
            // 
            this.cbxEmailSent.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxEmailSent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxEmailSent.Enabled = false;
            this.cbxEmailSent.FlatAppearance.BorderSize = 0;
            this.cbxEmailSent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxEmailSent.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxEmailSent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxEmailSent.ImageIndex = 1;
            this.cbxEmailSent.Location = new System.Drawing.Point(283, 368);
            this.cbxEmailSent.Name = "cbxEmailSent";
            this.cbxEmailSent.Size = new System.Drawing.Size(141, 30);
            this.cbxEmailSent.TabIndex = 47;
            this.cbxEmailSent.Text = "Email Sent";
            this.cbxEmailSent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxEmailSent.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(122, 151);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(50, 30);
            this.label20.TabIndex = 55;
            this.label20.Text = "Paid:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(28, 299);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 18);
            this.label13.TabIndex = 35;
            this.label13.Text = "Expires:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblExpires
            // 
            this.lblExpires.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExpires.Location = new System.Drawing.Point(163, 297);
            this.lblExpires.Name = "lblExpires";
            this.lblExpires.Size = new System.Drawing.Size(241, 26);
            this.lblExpires.TabIndex = 34;
            this.lblExpires.Text = "Expires:";
            this.lblExpires.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBookingEstimate
            // 
            this.txtBookingEstimate.BackColor = System.Drawing.Color.Salmon;
            this.txtBookingEstimate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBookingEstimate.Location = new System.Drawing.Point(175, 81);
            this.txtBookingEstimate.Name = "txtBookingEstimate";
            this.txtBookingEstimate.ReadOnly = true;
            this.txtBookingEstimate.Size = new System.Drawing.Size(145, 30);
            this.txtBookingEstimate.TabIndex = 38;
            this.txtBookingEstimate.Text = "0.00";
            this.txtBookingEstimate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblReservationCode
            // 
            this.lblReservationCode.BackColor = System.Drawing.Color.Wheat;
            this.lblReservationCode.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReservationCode.Location = new System.Drawing.Point(78, 227);
            this.lblReservationCode.Name = "lblReservationCode";
            this.lblReservationCode.Size = new System.Drawing.Size(326, 28);
            this.lblReservationCode.TabIndex = 42;
            this.lblReservationCode.Text = "Code";
            this.lblReservationCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEmail
            // 
            this.btnEmail.BackColor = System.Drawing.Color.Transparent;
            this.btnEmail.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEmail.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnEmail.FlatAppearance.BorderSize = 0;
            this.btnEmail.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnEmail.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEmail.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmail.ForeColor = System.Drawing.Color.White;
            this.btnEmail.Location = new System.Drawing.Point(115, 363);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(80, 41);
            this.btnEmail.TabIndex = 45;
            this.btnEmail.Text = "Email";
            this.btnEmail.UseVisualStyleBackColor = false;
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            this.btnEmail.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseDown);
            this.btnEmail.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseUp);
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(69, 81);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(103, 30);
            this.label17.TabIndex = 37;
            this.label17.Text = "Estimate Amount:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConfirm.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(286, 412);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 41);
            this.btnConfirm.TabIndex = 51;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            this.btnConfirm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseDown);
            this.btnConfirm.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseUp);
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.LemonChiffon;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(78, 263);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(326, 28);
            this.lblStatus.TabIndex = 43;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancelBooking
            // 
            this.btnCancelBooking.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelBooking.BackgroundImage = global::Parafait_POS.Properties.Resources.discount_button;
            this.btnCancelBooking.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelBooking.FlatAppearance.BorderColor = System.Drawing.Color.LightSalmon;
            this.btnCancelBooking.FlatAppearance.BorderSize = 0;
            this.btnCancelBooking.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancelBooking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancelBooking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancelBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelBooking.ForeColor = System.Drawing.Color.Black;
            this.btnCancelBooking.Location = new System.Drawing.Point(574, 585);
            this.btnCancelBooking.Name = "btnCancelBooking";
            this.btnCancelBooking.Size = new System.Drawing.Size(116, 34);
            this.btnCancelBooking.TabIndex = 56;
            this.btnCancelBooking.Text = "Cancel Booking";
            this.btnCancelBooking.UseVisualStyleBackColor = false;
            this.btnCancelBooking.Click += new System.EventHandler(this.btnCancelBooking_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 313);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 15);
            this.label4.TabIndex = 34;
            this.label4.Text = "Package Contents:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvPackageDetails
            // 
            this.dgvPackageDetails.AllowUserToAddRows = false;
            this.dgvPackageDetails.AllowUserToDeleteRows = false;
            this.dgvPackageDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPackageDetails.Location = new System.Drawing.Point(138, 313);
            this.dgvPackageDetails.Name = "dgvPackageDetails";
            this.dgvPackageDetails.ReadOnly = true;
            this.dgvPackageDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPackageDetails.Size = new System.Drawing.Size(414, 120);
            this.dgvPackageDetails.TabIndex = 28;
            // 
            // btnAttendeeDetails
            // 
            this.btnAttendeeDetails.BackColor = System.Drawing.Color.Transparent;
            this.btnAttendeeDetails.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnAttendeeDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAttendeeDetails.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnAttendeeDetails.FlatAppearance.BorderSize = 0;
            this.btnAttendeeDetails.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAttendeeDetails.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAttendeeDetails.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAttendeeDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAttendeeDetails.ForeColor = System.Drawing.Color.White;
            this.btnAttendeeDetails.Location = new System.Drawing.Point(463, 302);
            this.btnAttendeeDetails.Name = "btnAttendeeDetails";
            this.btnAttendeeDetails.Size = new System.Drawing.Size(116, 48);
            this.btnAttendeeDetails.TabIndex = 34;
            this.btnAttendeeDetails.Text = "Add Attendee Details";
            this.btnAttendeeDetails.UseVisualStyleBackColor = false;
            this.btnAttendeeDetails.Click += new System.EventHandler(this.btnAttendeeDetails_Click);
            this.btnAttendeeDetails.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseDown);
            this.btnAttendeeDetails.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseUp);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(593, 144);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(100, 30);
            this.label14.TabIndex = 40;
            this.label14.Text = "Reserved To";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbToTime
            // 
            this.cmbToTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToTime.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbToTime.FormattingEnabled = true;
            this.cmbToTime.Location = new System.Drawing.Point(696, 144);
            this.cmbToTime.Name = "cmbToTime";
            this.cmbToTime.Size = new System.Drawing.Size(116, 31);
            this.cmbToTime.TabIndex = 17;
            // 
            // cmbRecurFrequency
            // 
            this.cmbRecurFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRecurFrequency.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRecurFrequency.FormattingEnabled = true;
            this.cmbRecurFrequency.Items.AddRange(new object[] {
            "Daily",
            "Weekly"});
            this.cmbRecurFrequency.Location = new System.Drawing.Point(692, 587);
            this.cmbRecurFrequency.Name = "cmbRecurFrequency";
            this.cmbRecurFrequency.Size = new System.Drawing.Size(85, 31);
            this.cmbRecurFrequency.TabIndex = 7;
            this.cmbRecurFrequency.Visible = false;
            // 
            // dtpRecurUntil
            // 
            this.dtpRecurUntil.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpRecurUntil.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpRecurUntil.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpRecurUntil.Location = new System.Drawing.Point(831, 589);
            this.dtpRecurUntil.Name = "dtpRecurUntil";
            this.dtpRecurUntil.Size = new System.Drawing.Size(212, 30);
            this.dtpRecurUntil.TabIndex = 8;
            this.dtpRecurUntil.Visible = false;
            this.dtpRecurUntil.Enter += new System.EventHandler(this.dtpRecurUntil_Enter);
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(790, 589);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 30);
            this.label18.TabIndex = 51;
            this.label18.Text = "Until:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label18.Visible = false;
            // 
            // dgvBookingProducts
            // 
            this.dgvBookingProducts.AllowUserToDeleteRows = false;
            this.dgvBookingProducts.AllowUserToResizeRows = false;
            this.dgvBookingProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBookingProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcDelete,
            this.dcProduct,
            this.dcQuantity,
            this.dcPrice,
            this.type,
            this.dcBookingProductId,
            this.AdditionalDiscountName,
            this.additionalTransactionProfileId,
            this.dcRemarks,
            this.LineId});
            this.dgvBookingProducts.Location = new System.Drawing.Point(7, 24);
            this.dgvBookingProducts.Name = "dgvBookingProducts";
            this.dgvBookingProducts.RowHeadersVisible = false;
            this.dgvBookingProducts.RowTemplate.Height = 30;
            this.dgvBookingProducts.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvBookingProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBookingProducts.Size = new System.Drawing.Size(1148, 204);
            this.dgvBookingProducts.TabIndex = 32;
            this.dgvBookingProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBookingProducts_CellClick);
            this.dgvBookingProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBookingProducts_CellContentClick);
            this.dgvBookingProducts.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBookingProducts_CellEnter);
            this.dgvBookingProducts.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBookingProducts_CellValidated);
            this.dgvBookingProducts.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvBookingProducts_DataError);
            // 
            // dcDelete
            // 
            this.dcDelete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dcDelete.HeaderText = "X";
            this.dcDelete.Name = "dcDelete";
            this.dcDelete.Text = "X";
            this.dcDelete.UseColumnTextForButtonValue = true;
            this.dcDelete.Width = 22;
            // 
            // dcProduct
            // 
            this.dcProduct.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dcProduct.HeaderText = "Product";
            this.dcProduct.MinimumWidth = 180;
            this.dcProduct.Name = "dcProduct";
            this.dcProduct.Width = 180;
            // 
            // dcQuantity
            // 
            this.dcQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcQuantity.HeaderText = "Qty.";
            this.dcQuantity.Name = "dcQuantity";
            this.dcQuantity.Width = 70;
            // 
            // dcPrice
            // 
            this.dcPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcPrice.HeaderText = "Price";
            this.dcPrice.Name = "dcPrice";
            this.dcPrice.Width = 70;
            // 
            // type
            // 
            this.type.HeaderText = "Product Type";
            this.type.MinimumWidth = 150;
            this.type.Name = "type";
            this.type.Width = 150;
            // 
            // dcBookingProductId
            // 
            this.dcBookingProductId.HeaderText = "BookingProductId";
            this.dcBookingProductId.Name = "dcBookingProductId";
            this.dcBookingProductId.Visible = false;
            // 
            // AdditionalDiscountName
            // 
            this.AdditionalDiscountName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AdditionalDiscountName.HeaderText = "Discount Name";
            this.AdditionalDiscountName.Items.AddRange(new object[] {
            "NULL"});
            this.AdditionalDiscountName.MinimumWidth = 180;
            this.AdditionalDiscountName.Name = "AdditionalDiscountName";
            this.AdditionalDiscountName.Width = 180;
            // 
            // additionalTransactionProfileId
            // 
            this.additionalTransactionProfileId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.additionalTransactionProfileId.HeaderText = "Transaction Profile";
            this.additionalTransactionProfileId.MinimumWidth = 180;
            this.additionalTransactionProfileId.Name = "additionalTransactionProfileId";
            this.additionalTransactionProfileId.Width = 180;
            // 
            // dcRemarks
            // 
            this.dcRemarks.HeaderText = "Remarks";
            this.dcRemarks.MinimumWidth = 305;
            this.dcRemarks.Name = "dcRemarks";
            this.dcRemarks.Width = 305;
            // 
            // LineId
            // 
            this.LineId.HeaderText = "LineId";
            this.LineId.Name = "LineId";
            this.LineId.Visible = false;
            // 
            // tcBooking
            // 
            this.tcBooking.Controls.Add(this.tpDateTime);
            this.tcBooking.Controls.Add(this.tpCustomer);
            this.tcBooking.Controls.Add(this.tpPackage);
            this.tcBooking.Controls.Add(this.tpAdditional);
            this.tcBooking.Controls.Add(this.tpConfirm);
            this.tcBooking.Controls.Add(this.tpAuditTrail);
            this.tcBooking.ItemSize = new System.Drawing.Size(93, 40);
            this.tcBooking.Location = new System.Drawing.Point(1, -10);
            this.tcBooking.Name = "tcBooking";
            this.tcBooking.SelectedIndex = 0;
            this.tcBooking.Size = new System.Drawing.Size(1239, 673);
            this.tcBooking.TabIndex = 66;
            this.tcBooking.SelectedIndexChanged += new System.EventHandler(this.tcBooking_SelectedIndexChanged);
            // 
            // tpDateTime
            // 
            this.tpDateTime.Controls.Add(this.cmbToTimeForSearch);
            this.tpDateTime.Controls.Add(this.cmbFromTimeForSearch);
            this.tpDateTime.Controls.Add(this.lblFromTimeSearch);
            this.tpDateTime.Controls.Add(this.lblToTimeSearch);
            this.tpDateTime.Controls.Add(this.attrScheduleHScrollBar);
            this.tpDateTime.Controls.Add(this.lblTimeSlots);
            this.tpDateTime.Controls.Add(this.btnIncreaseGuestCount);
            this.tpDateTime.Controls.Add(this.btnReduceGuestCount);
            this.tpDateTime.Controls.Add(this.txtGuests);
            this.tpDateTime.Controls.Add(this.cbxNight);
            this.tpDateTime.Controls.Add(this.cbxAfternoon);
            this.tpDateTime.Controls.Add(this.cbxMorning);
            this.tpDateTime.Controls.Add(this.cbxEarlyMorning);
            this.tpDateTime.Controls.Add(this.btnBlockSchedule);
            this.tpDateTime.Controls.Add(this.cmbFacility);
            this.tpDateTime.Controls.Add(this.lblFacility);
            this.tpDateTime.Controls.Add(this.attrScheduleVScrollBar);
            this.tpDateTime.Controls.Add(this.dgvAttractionSchedules);
            this.tpDateTime.Controls.Add(this.lblSchedule);
            this.tpDateTime.Controls.Add(this.cmbChannel);
            this.tpDateTime.Controls.Add(this.label11);
            this.tpDateTime.Controls.Add(this.txtBookingName);
            this.tpDateTime.Controls.Add(this.label10);
            this.tpDateTime.Controls.Add(this.label19);
            this.tpDateTime.Controls.Add(this.btnCancel);
            this.tpDateTime.Controls.Add(this.btnNextDateTime);
            this.tpDateTime.Controls.Add(this.cmbRecurFrequency);
            this.tpDateTime.Controls.Add(this.dtpforDate);
            this.tpDateTime.Controls.Add(this.cbxRecur);
            this.tpDateTime.Controls.Add(this.label1);
            this.tpDateTime.Controls.Add(this.cmbBookingClass);
            this.tpDateTime.Controls.Add(this.dtpRecurUntil);
            this.tpDateTime.Controls.Add(this.label3);
            this.tpDateTime.Controls.Add(this.btnFind);
            this.tpDateTime.Controls.Add(this.label18);
            this.tpDateTime.Controls.Add(this.cmbToTime);
            this.tpDateTime.Controls.Add(this.cmbFromTime);
            this.tpDateTime.Controls.Add(this.label5);
            this.tpDateTime.Controls.Add(this.label14);
            this.tpDateTime.Location = new System.Drawing.Point(4, 44);
            this.tpDateTime.Name = "tpDateTime";
            this.tpDateTime.Padding = new System.Windows.Forms.Padding(3);
            this.tpDateTime.Size = new System.Drawing.Size(1231, 625);
            this.tpDateTime.TabIndex = 0;
            this.tpDateTime.Text = "Date and Time";
            this.tpDateTime.UseVisualStyleBackColor = true;
            // 
            // cmbToTimeForSearch
            // 
            this.cmbToTimeForSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbToTimeForSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbToTimeForSearch.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbToTimeForSearch.FormattingEnabled = true;
            this.cmbToTimeForSearch.Location = new System.Drawing.Point(696, 61);
            this.cmbToTimeForSearch.Name = "cmbToTimeForSearch";
            this.cmbToTimeForSearch.Size = new System.Drawing.Size(116, 31);
            this.cmbToTimeForSearch.TabIndex = 8;
            this.cmbToTimeForSearch.SelectedIndexChanged += new System.EventHandler(this.cmbFromOrTOTimeForSearch_SelectedIndexChanged);
            // 
            // cmbFromTimeForSearch
            // 
            this.cmbFromTimeForSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbFromTimeForSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFromTimeForSearch.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFromTimeForSearch.FormattingEnabled = true;
            this.cmbFromTimeForSearch.Location = new System.Drawing.Point(471, 61);
            this.cmbFromTimeForSearch.Name = "cmbFromTimeForSearch";
            this.cmbFromTimeForSearch.Size = new System.Drawing.Size(116, 31);
            this.cmbFromTimeForSearch.TabIndex = 7;
            this.cmbFromTimeForSearch.SelectedIndexChanged += new System.EventHandler(this.cmbFromOrTOTimeForSearch_SelectedIndexChanged);
            // 
            // lblFromTimeSearch
            // 
            this.lblFromTimeSearch.Location = new System.Drawing.Point(396, 61);
            this.lblFromTimeSearch.Name = "lblFromTimeSearch";
            this.lblFromTimeSearch.Size = new System.Drawing.Size(70, 30);
            this.lblFromTimeSearch.TabIndex = 90;
            this.lblFromTimeSearch.Text = "From Time:";
            this.lblFromTimeSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblToTimeSearch
            // 
            this.lblToTimeSearch.Location = new System.Drawing.Point(639, 61);
            this.lblToTimeSearch.Name = "lblToTimeSearch";
            this.lblToTimeSearch.Size = new System.Drawing.Size(54, 30);
            this.lblToTimeSearch.TabIndex = 91;
            this.lblToTimeSearch.Text = "To Time:";
            this.lblToTimeSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // attrScheduleHScrollBar
            // 
            this.attrScheduleHScrollBar.AutoHide = false;
            this.attrScheduleHScrollBar.DataGridView = this.dgvAttractionSchedules;
            this.attrScheduleHScrollBar.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("attrScheduleHScrollBar.LeftButtonBackgroundImage")));
            this.attrScheduleHScrollBar.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("attrScheduleHScrollBar.LeftButtonDisabledBackgroundImage")));
            this.attrScheduleHScrollBar.Location = new System.Drawing.Point(128, 534);
            this.attrScheduleHScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.attrScheduleHScrollBar.Name = "attrScheduleHScrollBar";
            this.attrScheduleHScrollBar.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("attrScheduleHScrollBar.RightButtonBackgroundImage")));
            this.attrScheduleHScrollBar.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("attrScheduleHScrollBar.RightButtonDisabledBackgroundImage")));
            this.attrScheduleHScrollBar.ScrollableControl = null;
            this.attrScheduleHScrollBar.ScrollViewer = null;
            this.attrScheduleHScrollBar.Size = new System.Drawing.Size(1054, 40);
            this.attrScheduleHScrollBar.TabIndex = 87;
            // 
            // dgvAttractionSchedules
            // 
            this.dgvAttractionSchedules.AllowUserToAddRows = false;
            this.dgvAttractionSchedules.AllowUserToDeleteRows = false;
            this.dgvAttractionSchedules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttractionSchedules.Location = new System.Drawing.Point(128, 187);
            this.dgvAttractionSchedules.MultiSelect = false;
            this.dgvAttractionSchedules.Name = "dgvAttractionSchedules";
            this.dgvAttractionSchedules.RowTemplate.Height = 30;
            this.dgvAttractionSchedules.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvAttractionSchedules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAttractionSchedules.Size = new System.Drawing.Size(1054, 344);
            this.dgvAttractionSchedules.TabIndex = 19;
            this.dgvAttractionSchedules.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAttractionSchedules_CellClick);
            this.dgvAttractionSchedules.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAttractionSchedules_CellEnter);
            // 
            // lblTimeSlots
            // 
            this.lblTimeSlots.Location = new System.Drawing.Point(328, 101);
            this.lblTimeSlots.Name = "lblTimeSlots";
            this.lblTimeSlots.Size = new System.Drawing.Size(138, 30);
            this.lblTimeSlots.TabIndex = 86;
            this.lblTimeSlots.Text = "Prefered Time Slots:";
            this.lblTimeSlots.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnIncreaseGuestCount
            // 
            this.btnIncreaseGuestCount.BackColor = System.Drawing.Color.Transparent;
            this.btnIncreaseGuestCount.BackgroundImage = global::Parafait_POS.Properties.Resources.Plus_Btn;
            this.btnIncreaseGuestCount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIncreaseGuestCount.FlatAppearance.BorderSize = 0;
            this.btnIncreaseGuestCount.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnIncreaseGuestCount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnIncreaseGuestCount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnIncreaseGuestCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIncreaseGuestCount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIncreaseGuestCount.ForeColor = System.Drawing.Color.White;
            this.btnIncreaseGuestCount.Location = new System.Drawing.Point(789, 13);
            this.btnIncreaseGuestCount.Name = "btnIncreaseGuestCount";
            this.btnIncreaseGuestCount.Size = new System.Drawing.Size(40, 36);
            this.btnIncreaseGuestCount.TabIndex = 5;
            this.btnIncreaseGuestCount.UseVisualStyleBackColor = false;
            this.btnIncreaseGuestCount.Click += new System.EventHandler(this.btnIncreaseGuestCount_Click);
            this.btnIncreaseGuestCount.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnIncreaseGuestMouseDown);
            this.btnIncreaseGuestCount.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnIncreaseGuestMouseUp);
            // 
            // btnReduceGuestCount
            // 
            this.btnReduceGuestCount.BackColor = System.Drawing.Color.Transparent;
            this.btnReduceGuestCount.BackgroundImage = global::Parafait_POS.Properties.Resources.Minus_Btn;
            this.btnReduceGuestCount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReduceGuestCount.FlatAppearance.BorderSize = 0;
            this.btnReduceGuestCount.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnReduceGuestCount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnReduceGuestCount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnReduceGuestCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReduceGuestCount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReduceGuestCount.ForeColor = System.Drawing.Color.White;
            this.btnReduceGuestCount.Location = new System.Drawing.Point(696, 13);
            this.btnReduceGuestCount.Name = "btnReduceGuestCount";
            this.btnReduceGuestCount.Size = new System.Drawing.Size(40, 36);
            this.btnReduceGuestCount.TabIndex = 4;
            this.btnReduceGuestCount.UseVisualStyleBackColor = false;
            this.btnReduceGuestCount.Click += new System.EventHandler(this.btnRedueceGuestCount_Click);
            this.btnReduceGuestCount.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnReduceGuestMouseDown);
            this.btnReduceGuestCount.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnReduceGuestMouseUp);
            // 
            // txtGuests
            // 
            this.txtGuests.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGuests.Location = new System.Drawing.Point(737, 16);
            this.txtGuests.MaxLength = 4;
            this.txtGuests.Name = "txtGuests";
            this.txtGuests.Size = new System.Drawing.Size(52, 30);
            this.txtGuests.TabIndex = 3;
            this.txtGuests.Text = "1";
            this.txtGuests.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtGuests.TextChanged += new System.EventHandler(this.txtGuests_TextChanged);
            this.txtGuests.Enter += new System.EventHandler(this.txtGuests_Enter);
            this.txtGuests.Validating += new System.ComponentModel.CancelEventHandler(this.txtGuests_Validating);
            // 
            // cbxNight
            // 
            this.cbxNight.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxNight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxNight.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxNight.FlatAppearance.BorderSize = 0;
            this.cbxNight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxNight.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxNight.ImageIndex = 1;
            this.cbxNight.Location = new System.Drawing.Point(729, 98);
            this.cbxNight.Name = "cbxNight";
            this.cbxNight.Size = new System.Drawing.Size(80, 36);
            this.cbxNight.TabIndex = 13;
            this.cbxNight.Text = "18 - 00";
            this.cbxNight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxNight.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.cbxNight.UseVisualStyleBackColor = true;
            this.cbxNight.CheckedChanged += new System.EventHandler(this.cbxTimeSlots_CheckedChanged);
            // 
            // cbxAfternoon
            // 
            this.cbxAfternoon.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxAfternoon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxAfternoon.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAfternoon.FlatAppearance.BorderSize = 0;
            this.cbxAfternoon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxAfternoon.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAfternoon.ImageIndex = 1;
            this.cbxAfternoon.Location = new System.Drawing.Point(643, 98);
            this.cbxAfternoon.Name = "cbxAfternoon";
            this.cbxAfternoon.Size = new System.Drawing.Size(80, 36);
            this.cbxAfternoon.TabIndex = 12;
            this.cbxAfternoon.Text = "12 - 18";
            this.cbxAfternoon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAfternoon.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.cbxAfternoon.UseVisualStyleBackColor = true;
            this.cbxAfternoon.CheckedChanged += new System.EventHandler(this.cbxTimeSlots_CheckedChanged);
            // 
            // cbxMorning
            // 
            this.cbxMorning.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxMorning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxMorning.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxMorning.FlatAppearance.BorderSize = 0;
            this.cbxMorning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxMorning.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxMorning.ImageIndex = 1;
            this.cbxMorning.Location = new System.Drawing.Point(557, 98);
            this.cbxMorning.Name = "cbxMorning";
            this.cbxMorning.Size = new System.Drawing.Size(80, 36);
            this.cbxMorning.TabIndex = 11;
            this.cbxMorning.Text = "06 - 12";
            this.cbxMorning.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxMorning.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.cbxMorning.UseVisualStyleBackColor = true;
            this.cbxMorning.CheckedChanged += new System.EventHandler(this.cbxTimeSlots_CheckedChanged);
            // 
            // cbxEarlyMorning
            // 
            this.cbxEarlyMorning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxEarlyMorning.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxEarlyMorning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxEarlyMorning.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxEarlyMorning.FlatAppearance.BorderSize = 0;
            this.cbxEarlyMorning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxEarlyMorning.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxEarlyMorning.ImageIndex = 1;
            this.cbxEarlyMorning.Location = new System.Drawing.Point(471, 98);
            this.cbxEarlyMorning.Name = "cbxEarlyMorning";
            this.cbxEarlyMorning.Size = new System.Drawing.Size(80, 36);
            this.cbxEarlyMorning.TabIndex = 10;
            this.cbxEarlyMorning.Text = "00 - 06";
            this.cbxEarlyMorning.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxEarlyMorning.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.cbxEarlyMorning.UseVisualStyleBackColor = true;
            this.cbxEarlyMorning.CheckedChanged += new System.EventHandler(this.cbxTimeSlots_CheckedChanged);
            // 
            // btnBlockSchedule
            // 
            this.btnBlockSchedule.BackColor = System.Drawing.Color.Transparent;
            this.btnBlockSchedule.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnBlockSchedule.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBlockSchedule.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnBlockSchedule.FlatAppearance.BorderSize = 0;
            this.btnBlockSchedule.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBlockSchedule.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBlockSchedule.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBlockSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlockSchedule.ForeColor = System.Drawing.Color.White;
            this.btnBlockSchedule.Location = new System.Drawing.Point(873, 141);
            this.btnBlockSchedule.Name = "btnBlockSchedule";
            this.btnBlockSchedule.Size = new System.Drawing.Size(104, 36);
            this.btnBlockSchedule.TabIndex = 18;
            this.btnBlockSchedule.Text = "Block Schedule";
            this.btnBlockSchedule.UseVisualStyleBackColor = false;
            this.btnBlockSchedule.Click += new System.EventHandler(this.btnBlockSchedule_Click);
            // 
            // cmbFacility
            // 
            this.cmbFacility.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbFacility.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFacility.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFacility.FormattingEnabled = true;
            this.cmbFacility.Location = new System.Drawing.Point(128, 101);
            this.cmbFacility.Name = "cmbFacility";
            this.cmbFacility.Size = new System.Drawing.Size(179, 31);
            this.cmbFacility.TabIndex = 9;
            this.cmbFacility.SelectedIndexChanged += new System.EventHandler(this.cmbFacility_SelectedIndexChanged);
            // 
            // lblFacility
            // 
            this.lblFacility.Location = new System.Drawing.Point(25, 101);
            this.lblFacility.Name = "lblFacility";
            this.lblFacility.Size = new System.Drawing.Size(100, 30);
            this.lblFacility.TabIndex = 72;
            this.lblFacility.Text = "Facility:";
            this.lblFacility.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // attrScheduleVScrollBar
            // 
            this.attrScheduleVScrollBar.AutoHide = false;
            this.attrScheduleVScrollBar.DataGridView = this.dgvAttractionSchedules;
            this.attrScheduleVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("attrScheduleVScrollBar.DownButtonBackgroundImage")));
            this.attrScheduleVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("attrScheduleVScrollBar.DownButtonDisabledBackgroundImage")));
            this.attrScheduleVScrollBar.Location = new System.Drawing.Point(1185, 187);
            this.attrScheduleVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.attrScheduleVScrollBar.Name = "attrScheduleVScrollBar";
            this.attrScheduleVScrollBar.ScrollableControl = null;
            this.attrScheduleVScrollBar.ScrollViewer = null;
            this.attrScheduleVScrollBar.Size = new System.Drawing.Size(40, 344);
            this.attrScheduleVScrollBar.TabIndex = 70;
            this.attrScheduleVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("attrScheduleVScrollBar.UpButtonBackgroundImage")));
            this.attrScheduleVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("attrScheduleVScrollBar.UpButtonDisabledBackgroundImage")));
            // 
            // lblSchedule
            // 
            this.lblSchedule.Location = new System.Drawing.Point(24, 187);
            this.lblSchedule.Name = "lblSchedule";
            this.lblSchedule.Size = new System.Drawing.Size(101, 30);
            this.lblSchedule.TabIndex = 68;
            this.lblSchedule.Text = "Schedule:";
            this.lblSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbChannel
            // 
            this.cmbChannel.AutoCompleteCustomSource.AddRange(new string[] {
            "InPerson",
            "Phone",
            "Email",
            "Web",
            "Gateway"});
            this.cmbChannel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbChannel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbChannel.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbChannel.FormattingEnabled = true;
            this.cmbChannel.Items.AddRange(new object[] {
            "InPerson",
            "Phone",
            "Email",
            "Web",
            "Gateway"});
            this.cmbChannel.Location = new System.Drawing.Point(471, 16);
            this.cmbChannel.Name = "cmbChannel";
            this.cmbChannel.Size = new System.Drawing.Size(116, 31);
            this.cmbChannel.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(364, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(102, 30);
            this.label11.TabIndex = 67;
            this.label11.Text = "Channel:*";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBookingName
            // 
            this.txtBookingName.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBookingName.Location = new System.Drawing.Point(128, 16);
            this.txtBookingName.MaxLength = 50;
            this.txtBookingName.Name = "txtBookingName";
            this.txtBookingName.Size = new System.Drawing.Size(226, 30);
            this.txtBookingName.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(23, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 30);
            this.label10.TabIndex = 63;
            this.label10.Text = "Booking Name:*";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(643, 16);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(50, 30);
            this.label19.TabIndex = 60;
            this.label19.Text = "Guests:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnNextDateTime
            // 
            this.btnNextDateTime.BackColor = System.Drawing.Color.Transparent;
            this.btnNextDateTime.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnNextDateTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextDateTime.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnNextDateTime.FlatAppearance.BorderSize = 0;
            this.btnNextDateTime.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNextDateTime.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextDateTime.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextDateTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextDateTime.ForeColor = System.Drawing.Color.White;
            this.btnNextDateTime.Location = new System.Drawing.Point(466, 585);
            this.btnNextDateTime.Name = "btnNextDateTime";
            this.btnNextDateTime.Size = new System.Drawing.Size(116, 34);
            this.btnNextDateTime.TabIndex = 21;
            this.btnNextDateTime.Text = "Next";
            this.btnNextDateTime.UseVisualStyleBackColor = false;
            this.btnNextDateTime.Click += new System.EventHandler(this.btnNextDateTime_Click);
            this.btnNextDateTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnNextDateTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // cbxRecur
            // 
            this.cbxRecur.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxRecur.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxRecur.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxRecur.FlatAppearance.BorderSize = 0;
            this.cbxRecur.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxRecur.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxRecur.ImageIndex = 1;
            this.cbxRecur.Location = new System.Drawing.Point(607, 587);
            this.cbxRecur.Name = "cbxRecur";
            this.cbxRecur.Size = new System.Drawing.Size(76, 30);
            this.cbxRecur.TabIndex = 6;
            this.cbxRecur.Text = "Recur:";
            this.cbxRecur.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxRecur.UseVisualStyleBackColor = true;
            this.cbxRecur.Visible = false;
            this.cbxRecur.CheckedChanged += new System.EventHandler(this.cbxRecur_CheckedChanged);
            // 
            // tpCustomer
            // 
            this.tpCustomer.Controls.Add(this.custPanelVScrollBar);
            this.tpCustomer.Controls.Add(this.txtCardNumber);
            this.tpCustomer.Controls.Add(this.lblCardNumber);
            this.tpCustomer.Controls.Add(this.pnlCustomerDetail);
            this.tpCustomer.Controls.Add(this.btnCustomerLookup);
            this.tpCustomer.Controls.Add(this.lnkUpdate);
            this.tpCustomer.Controls.Add(this.btnPrevCustomer);
            this.tpCustomer.Controls.Add(this.btnNextCustomer);
            this.tpCustomer.Location = new System.Drawing.Point(4, 44);
            this.tpCustomer.Name = "tpCustomer";
            this.tpCustomer.Padding = new System.Windows.Forms.Padding(3);
            this.tpCustomer.Size = new System.Drawing.Size(1231, 625);
            this.tpCustomer.TabIndex = 1;
            this.tpCustomer.Text = "Customer";
            this.tpCustomer.UseVisualStyleBackColor = true;
            // 
            // custPanelVScrollBar
            // 
            this.custPanelVScrollBar.AutoHide = false;
            this.custPanelVScrollBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.custPanelVScrollBar.DataGridView = null;
            this.custPanelVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("custPanelVScrollBar.DownButtonBackgroundImage")));
            this.custPanelVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("custPanelVScrollBar.DownButtonDisabledBackgroundImage")));
            this.custPanelVScrollBar.Location = new System.Drawing.Point(1182, 38);
            this.custPanelVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.custPanelVScrollBar.Name = "custPanelVScrollBar";
            this.custPanelVScrollBar.ScrollableControl = this.pnlCustomerDetail;
            this.custPanelVScrollBar.ScrollViewer = null;
            this.custPanelVScrollBar.Size = new System.Drawing.Size(40, 533);
            this.custPanelVScrollBar.TabIndex = 71;
            this.custPanelVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("custPanelVScrollBar.UpButtonBackgroundImage")));
            this.custPanelVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("custPanelVScrollBar.UpButtonDisabledBackgroundImage")));
            // 
            // pnlCustomerDetail
            // 
            this.pnlCustomerDetail.AutoScroll = true;
            this.pnlCustomerDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCustomerDetail.Location = new System.Drawing.Point(7, 38);
            this.pnlCustomerDetail.Name = "pnlCustomerDetail";
            this.pnlCustomerDetail.Size = new System.Drawing.Size(1204, 533);
            this.pnlCustomerDetail.TabIndex = 23;
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.Location = new System.Drawing.Point(127, 4);
            this.txtCardNumber.MaxLength = 12;
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(145, 30);
            this.txtCardNumber.TabIndex = 22;
            this.txtCardNumber.TextChanged += new System.EventHandler(this.txtCardNumber_TextChanged);
            this.txtCardNumber.Validating += new System.ComponentModel.CancelEventHandler(this.txtCardNumber_Validating);
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.Location = new System.Drawing.Point(58, 3);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(67, 30);
            this.lblCardNumber.TabIndex = 70;
            this.lblCardNumber.Text = "Card:";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCustomerLookup
            // 
            this.btnCustomerLookup.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCustomerLookup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCustomerLookup.FlatAppearance.BorderSize = 0;
            this.btnCustomerLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerLookup.ForeColor = System.Drawing.Color.White;
            this.btnCustomerLookup.Location = new System.Drawing.Point(241, 585);
            this.btnCustomerLookup.Name = "btnCustomerLookup";
            this.btnCustomerLookup.Size = new System.Drawing.Size(150, 34);
            this.btnCustomerLookup.TabIndex = 24;
            this.btnCustomerLookup.Text = "Customer Lookup";
            this.btnCustomerLookup.UseVisualStyleBackColor = true;
            this.btnCustomerLookup.Click += new System.EventHandler(this.btnCustomerLookup_Click);
            this.btnCustomerLookup.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnCustomerLookup.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // lnkUpdate
            // 
            this.lnkUpdate.AutoSize = true;
            this.lnkUpdate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkUpdate.Location = new System.Drawing.Point(709, 601);
            this.lnkUpdate.Name = "lnkUpdate";
            this.lnkUpdate.Size = new System.Drawing.Size(59, 18);
            this.lnkUpdate.TabIndex = 66;
            this.lnkUpdate.TabStop = true;
            this.lnkUpdate.Text = "Update";
            this.lnkUpdate.Visible = false;
            this.lnkUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUpdate_LinkClicked);
            // 
            // btnPrevCustomer
            // 
            this.btnPrevCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevCustomer.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrevCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrevCustomer.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrevCustomer.FlatAppearance.BorderSize = 0;
            this.btnPrevCustomer.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrevCustomer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrevCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrevCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevCustomer.ForeColor = System.Drawing.Color.White;
            this.btnPrevCustomer.Location = new System.Drawing.Point(397, 585);
            this.btnPrevCustomer.Name = "btnPrevCustomer";
            this.btnPrevCustomer.Size = new System.Drawing.Size(116, 34);
            this.btnPrevCustomer.TabIndex = 25;
            this.btnPrevCustomer.Text = "Prev";
            this.btnPrevCustomer.UseVisualStyleBackColor = false;
            this.btnPrevCustomer.Click += new System.EventHandler(this.btnPrevCustomer_Click);
            this.btnPrevCustomer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnPrevCustomer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // btnNextCustomer
            // 
            this.btnNextCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnNextCustomer.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnNextCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextCustomer.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnNextCustomer.FlatAppearance.BorderSize = 0;
            this.btnNextCustomer.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNextCustomer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextCustomer.ForeColor = System.Drawing.Color.White;
            this.btnNextCustomer.Location = new System.Drawing.Point(519, 585);
            this.btnNextCustomer.Name = "btnNextCustomer";
            this.btnNextCustomer.Size = new System.Drawing.Size(116, 34);
            this.btnNextCustomer.TabIndex = 26;
            this.btnNextCustomer.Text = "Next";
            this.btnNextCustomer.UseVisualStyleBackColor = false;
            this.btnNextCustomer.Click += new System.EventHandler(this.btnNextCustomer_Click);
            this.btnNextCustomer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnNextCustomer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // tpPackage
            // 
            this.tpPackage.Controls.Add(this.packageListHScrollBar);
            this.tpPackage.Controls.Add(this.packageListVScrollBar);
            this.tpPackage.Controls.Add(this.btnPrevPackage);
            this.tpPackage.Controls.Add(this.btnNextPackage);
            this.tpPackage.Controls.Add(this.label2);
            this.tpPackage.Controls.Add(this.dgvPackageDetails);
            this.tpPackage.Controls.Add(this.label4);
            this.tpPackage.Controls.Add(this.nudQuantity);
            this.tpPackage.Controls.Add(this.label7);
            this.tpPackage.Controls.Add(this.txtRemarks);
            this.tpPackage.Controls.Add(this.label9);
            this.tpPackage.Controls.Add(this.dgvPackageList);
            this.tpPackage.Controls.Add(this.cmbPackage);
            this.tpPackage.Location = new System.Drawing.Point(4, 44);
            this.tpPackage.Name = "tpPackage";
            this.tpPackage.Padding = new System.Windows.Forms.Padding(3);
            this.tpPackage.Size = new System.Drawing.Size(1231, 625);
            this.tpPackage.TabIndex = 2;
            this.tpPackage.Text = "Package";
            this.tpPackage.UseVisualStyleBackColor = true;
            // 
            // packageListHScrollBar
            // 
            this.packageListHScrollBar.AutoHide = false;
            this.packageListHScrollBar.DataGridView = this.dgvPackageList;
            this.packageListHScrollBar.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("packageListHScrollBar.LeftButtonBackgroundImage")));
            this.packageListHScrollBar.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("packageListHScrollBar.LeftButtonDisabledBackgroundImage")));
            this.packageListHScrollBar.Location = new System.Drawing.Point(138, 261);
            this.packageListHScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.packageListHScrollBar.Name = "packageListHScrollBar";
            this.packageListHScrollBar.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("packageListHScrollBar.RightButtonBackgroundImage")));
            this.packageListHScrollBar.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("packageListHScrollBar.RightButtonDisabledBackgroundImage")));
            this.packageListHScrollBar.ScrollableControl = null;
            this.packageListHScrollBar.ScrollViewer = null;
            this.packageListHScrollBar.Size = new System.Drawing.Size(1022, 40);
            this.packageListHScrollBar.TabIndex = 74;
            // 
            // dgvPackageList
            // 
            this.dgvPackageList.AllowUserToResizeRows = false;
            this.dgvPackageList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPackageList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectProduct,
            this.packageName,
            this.Product_Id,
            this.package_Id,
            this.discountName,
            this.transactionProfileId,
            this.Remarks,
            this.SelectedStatus});
            this.dgvPackageList.Location = new System.Drawing.Point(138, 22);
            this.dgvPackageList.Name = "dgvPackageList";
            this.dgvPackageList.RowTemplate.Height = 30;
            this.dgvPackageList.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPackageList.Size = new System.Drawing.Size(1022, 236);
            this.dgvPackageList.TabIndex = 27;
            this.dgvPackageList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPackageList_CellClick);
            this.dgvPackageList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPackageList_CellEnter);
            this.dgvPackageList.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvPackageList_CurrentCellDirtyStateChanged);
            this.dgvPackageList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvPackageList_DataError);
            this.dgvPackageList.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvPackageList_EditingControlShowing);
            this.dgvPackageList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvPackageList_KeyPress);
            // 
            // SelectProduct
            // 
            this.SelectProduct.FalseValue = "N";
            this.SelectProduct.HeaderText = "";
            this.SelectProduct.MinimumWidth = 50;
            this.SelectProduct.Name = "SelectProduct";
            this.SelectProduct.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SelectProduct.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SelectProduct.TrueValue = "Y";
            this.SelectProduct.Width = 50;
            // 
            // packageName
            // 
            this.packageName.HeaderText = "Product Name";
            this.packageName.MinimumWidth = 200;
            this.packageName.Name = "packageName";
            this.packageName.ReadOnly = true;
            this.packageName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.packageName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.packageName.Width = 200;
            // 
            // Product_Id
            // 
            this.Product_Id.HeaderText = "Product Id";
            this.Product_Id.Name = "Product_Id";
            this.Product_Id.Visible = false;
            // 
            // package_Id
            // 
            this.package_Id.HeaderText = "Package Id";
            this.package_Id.Name = "package_Id";
            this.package_Id.Visible = false;
            this.package_Id.Width = 5;
            // 
            // discountName
            // 
            this.discountName.HeaderText = "Discount ";
            this.discountName.Items.AddRange(new object[] {
            "NULL"});
            this.discountName.MinimumWidth = 180;
            this.discountName.Name = "discountName";
            this.discountName.Width = 180;
            // 
            // transactionProfileId
            // 
            this.transactionProfileId.HeaderText = "Transaction Profile";
            this.transactionProfileId.Items.AddRange(new object[] {
            "NULL"});
            this.transactionProfileId.MinimumWidth = 180;
            this.transactionProfileId.Name = "transactionProfileId";
            this.transactionProfileId.Width = 180;
            // 
            // Remarks
            // 
            this.Remarks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.MinimumWidth = 300;
            this.Remarks.Name = "Remarks";
            // 
            // SelectedStatus
            // 
            this.SelectedStatus.HeaderText = "SelectedStatus";
            this.SelectedStatus.Name = "SelectedStatus";
            this.SelectedStatus.Visible = false;
            // 
            // packageListVScrollBar
            // 
            this.packageListVScrollBar.AutoHide = false;
            this.packageListVScrollBar.DataGridView = this.dgvPackageList;
            this.packageListVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("packageListVScrollBar.DownButtonBackgroundImage")));
            this.packageListVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("packageListVScrollBar.DownButtonDisabledBackgroundImage")));
            this.packageListVScrollBar.Location = new System.Drawing.Point(1163, 22);
            this.packageListVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.packageListVScrollBar.Name = "packageListVScrollBar";
            this.packageListVScrollBar.ScrollableControl = null;
            this.packageListVScrollBar.ScrollViewer = null;
            this.packageListVScrollBar.Size = new System.Drawing.Size(40, 236);
            this.packageListVScrollBar.TabIndex = 73;
            this.packageListVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("packageListVScrollBar.UpButtonBackgroundImage")));
            this.packageListVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("packageListVScrollBar.UpButtonDisabledBackgroundImage")));
            // 
            // btnPrevPackage
            // 
            this.btnPrevPackage.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevPackage.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrevPackage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrevPackage.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrevPackage.FlatAppearance.BorderSize = 0;
            this.btnPrevPackage.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrevPackage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrevPackage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrevPackage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevPackage.ForeColor = System.Drawing.Color.White;
            this.btnPrevPackage.Location = new System.Drawing.Point(280, 585);
            this.btnPrevPackage.Name = "btnPrevPackage";
            this.btnPrevPackage.Size = new System.Drawing.Size(116, 34);
            this.btnPrevPackage.TabIndex = 30;
            this.btnPrevPackage.Text = "Prev";
            this.btnPrevPackage.UseVisualStyleBackColor = false;
            this.btnPrevPackage.Click += new System.EventHandler(this.btnPrevPackage_Click);
            this.btnPrevPackage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnPrevPackage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // btnNextPackage
            // 
            this.btnNextPackage.BackColor = System.Drawing.Color.Transparent;
            this.btnNextPackage.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnNextPackage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextPackage.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnNextPackage.FlatAppearance.BorderSize = 0;
            this.btnNextPackage.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNextPackage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextPackage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextPackage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextPackage.ForeColor = System.Drawing.Color.White;
            this.btnNextPackage.Location = new System.Drawing.Point(477, 585);
            this.btnNextPackage.Name = "btnNextPackage";
            this.btnNextPackage.Size = new System.Drawing.Size(116, 34);
            this.btnNextPackage.TabIndex = 31;
            this.btnNextPackage.Text = "Next";
            this.btnNextPackage.UseVisualStyleBackColor = false;
            this.btnNextPackage.Click += new System.EventHandler(this.btnNextPackage_Click);
            // 
            // cmbPackage
            // 
            this.cmbPackage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPackage.FormattingEnabled = true;
            this.cmbPackage.Location = new System.Drawing.Point(246, 86);
            this.cmbPackage.Name = "cmbPackage";
            this.cmbPackage.Size = new System.Drawing.Size(303, 23);
            this.cmbPackage.TabIndex = 70;
            this.cmbPackage.Visible = false;
            this.cmbPackage.SelectedIndexChanged += new System.EventHandler(this.cmbPackage_SelectedIndexChanged);
            // 
            // tpAdditional
            // 
            this.tpAdditional.Controls.Add(this.additionalProdListHScrollbar);
            this.tpAdditional.Controls.Add(this.additionProdListVScrollbar);
            this.tpAdditional.Controls.Add(this.btnAddServiceCharge);
            this.tpAdditional.Controls.Add(this.btnAddProducts);
            this.tpAdditional.Controls.Add(this.btnPrevAdditionalProducts);
            this.tpAdditional.Controls.Add(this.btnNextAdditionalProducts);
            this.tpAdditional.Controls.Add(this.dgvBookingProducts);
            this.tpAdditional.Controls.Add(this.btnAttendeeDetails);
            this.tpAdditional.Location = new System.Drawing.Point(4, 44);
            this.tpAdditional.Name = "tpAdditional";
            this.tpAdditional.Padding = new System.Windows.Forms.Padding(3);
            this.tpAdditional.Size = new System.Drawing.Size(1231, 625);
            this.tpAdditional.TabIndex = 3;
            this.tpAdditional.Text = "Additional Products";
            this.tpAdditional.UseVisualStyleBackColor = true;
            // 
            // additionalProdListHScrollbar
            // 
            this.additionalProdListHScrollbar.AutoHide = false;
            this.additionalProdListHScrollbar.DataGridView = this.dgvBookingProducts;
            this.additionalProdListHScrollbar.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("additionalProdListHScrollbar.LeftButtonBackgroundImage")));
            this.additionalProdListHScrollbar.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("additionalProdListHScrollbar.LeftButtonDisabledBackgroundImage")));
            this.additionalProdListHScrollbar.Location = new System.Drawing.Point(7, 230);
            this.additionalProdListHScrollbar.Margin = new System.Windows.Forms.Padding(0);
            this.additionalProdListHScrollbar.Name = "additionalProdListHScrollbar";
            this.additionalProdListHScrollbar.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("additionalProdListHScrollbar.RightButtonBackgroundImage")));
            this.additionalProdListHScrollbar.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("additionalProdListHScrollbar.RightButtonDisabledBackgroundImage")));
            this.additionalProdListHScrollbar.ScrollableControl = null;
            this.additionalProdListHScrollbar.ScrollViewer = null;
            this.additionalProdListHScrollbar.Size = new System.Drawing.Size(1148, 40);
            this.additionalProdListHScrollbar.TabIndex = 75;
            // 
            // additionProdListVScrollbar
            // 
            this.additionProdListVScrollbar.AutoHide = false;
            this.additionProdListVScrollbar.DataGridView = this.dgvBookingProducts;
            this.additionProdListVScrollbar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("additionProdListVScrollbar.DownButtonBackgroundImage")));
            this.additionProdListVScrollbar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("additionProdListVScrollbar.DownButtonDisabledBackgroundImage")));
            this.additionProdListVScrollbar.Location = new System.Drawing.Point(1158, 24);
            this.additionProdListVScrollbar.Margin = new System.Windows.Forms.Padding(0);
            this.additionProdListVScrollbar.Name = "additionProdListVScrollbar";
            this.additionProdListVScrollbar.ScrollableControl = null;
            this.additionProdListVScrollbar.ScrollViewer = null;
            this.additionProdListVScrollbar.Size = new System.Drawing.Size(40, 204);
            this.additionProdListVScrollbar.TabIndex = 74;
            this.additionProdListVScrollbar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("additionProdListVScrollbar.UpButtonBackgroundImage")));
            this.additionProdListVScrollbar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("additionProdListVScrollbar.UpButtonDisabledBackgroundImage")));
            // 
            // btnAddServiceCharge
            // 
            this.btnAddServiceCharge.BackColor = System.Drawing.Color.Transparent;
            this.btnAddServiceCharge.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnAddServiceCharge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddServiceCharge.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnAddServiceCharge.FlatAppearance.BorderSize = 0;
            this.btnAddServiceCharge.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAddServiceCharge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddServiceCharge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddServiceCharge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddServiceCharge.ForeColor = System.Drawing.Color.White;
            this.btnAddServiceCharge.Location = new System.Drawing.Point(650, 302);
            this.btnAddServiceCharge.Name = "btnAddServiceCharge";
            this.btnAddServiceCharge.Size = new System.Drawing.Size(116, 48);
            this.btnAddServiceCharge.TabIndex = 35;
            this.btnAddServiceCharge.Text = "Add Service Charge";
            this.btnAddServiceCharge.UseVisualStyleBackColor = false;
            this.btnAddServiceCharge.Click += new System.EventHandler(this.btnAddServiceCharge_Click);
            this.btnAddServiceCharge.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseDown);
            this.btnAddServiceCharge.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseUp);
            // 
            // btnAddProducts
            // 
            this.btnAddProducts.BackColor = System.Drawing.Color.Transparent;
            this.btnAddProducts.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnAddProducts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddProducts.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnAddProducts.FlatAppearance.BorderSize = 0;
            this.btnAddProducts.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAddProducts.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddProducts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddProducts.ForeColor = System.Drawing.Color.White;
            this.btnAddProducts.Location = new System.Drawing.Point(276, 302);
            this.btnAddProducts.Name = "btnAddProducts";
            this.btnAddProducts.Size = new System.Drawing.Size(116, 48);
            this.btnAddProducts.TabIndex = 33;
            this.btnAddProducts.Text = "Add Products";
            this.btnAddProducts.UseVisualStyleBackColor = false;
            this.btnAddProducts.Click += new System.EventHandler(this.btnAddProducts_Click);
            this.btnAddProducts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseDown);
            this.btnAddProducts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseUp);
            // 
            // btnPrevAdditionalProducts
            // 
            this.btnPrevAdditionalProducts.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevAdditionalProducts.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrevAdditionalProducts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrevAdditionalProducts.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrevAdditionalProducts.FlatAppearance.BorderSize = 0;
            this.btnPrevAdditionalProducts.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrevAdditionalProducts.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrevAdditionalProducts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrevAdditionalProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevAdditionalProducts.ForeColor = System.Drawing.Color.White;
            this.btnPrevAdditionalProducts.Location = new System.Drawing.Point(255, 585);
            this.btnPrevAdditionalProducts.Name = "btnPrevAdditionalProducts";
            this.btnPrevAdditionalProducts.Size = new System.Drawing.Size(116, 34);
            this.btnPrevAdditionalProducts.TabIndex = 36;
            this.btnPrevAdditionalProducts.Text = "Prev";
            this.btnPrevAdditionalProducts.UseVisualStyleBackColor = false;
            this.btnPrevAdditionalProducts.Click += new System.EventHandler(this.btnPrevAdditionalProducts_Click);
            this.btnPrevAdditionalProducts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnPrevAdditionalProducts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // btnNextAdditionalProducts
            // 
            this.btnNextAdditionalProducts.BackColor = System.Drawing.Color.Transparent;
            this.btnNextAdditionalProducts.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnNextAdditionalProducts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextAdditionalProducts.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnNextAdditionalProducts.FlatAppearance.BorderSize = 0;
            this.btnNextAdditionalProducts.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNextAdditionalProducts.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextAdditionalProducts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextAdditionalProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextAdditionalProducts.ForeColor = System.Drawing.Color.White;
            this.btnNextAdditionalProducts.Location = new System.Drawing.Point(500, 585);
            this.btnNextAdditionalProducts.Name = "btnNextAdditionalProducts";
            this.btnNextAdditionalProducts.Size = new System.Drawing.Size(116, 34);
            this.btnNextAdditionalProducts.TabIndex = 37;
            this.btnNextAdditionalProducts.Text = "Next";
            this.btnNextAdditionalProducts.UseVisualStyleBackColor = false;
            this.btnNextAdditionalProducts.Click += new System.EventHandler(this.btnNextAdditionalProducts_Click);
            this.btnNextAdditionalProducts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnNextAdditionalProducts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // tpConfirm
            // 
            this.tpConfirm.Controls.Add(this.btnEditBooking);
            this.tpConfirm.Controls.Add(this.btnClose);
            this.tpConfirm.Controls.Add(this.btnPrevConfirm);
            this.tpConfirm.Controls.Add(this.groupBox1);
            this.tpConfirm.Controls.Add(this.btnBook);
            this.tpConfirm.Controls.Add(this.btnCancelBooking);
            this.tpConfirm.Location = new System.Drawing.Point(4, 44);
            this.tpConfirm.Name = "tpConfirm";
            this.tpConfirm.Padding = new System.Windows.Forms.Padding(3);
            this.tpConfirm.Size = new System.Drawing.Size(1231, 625);
            this.tpConfirm.TabIndex = 4;
            this.tpConfirm.Text = "Summary";
            this.tpConfirm.UseVisualStyleBackColor = true;
            // 
            // btnEditBooking
            // 
            this.btnEditBooking.BackColor = System.Drawing.Color.Transparent;
            this.btnEditBooking.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnEditBooking.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEditBooking.Enabled = false;
            this.btnEditBooking.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnEditBooking.FlatAppearance.BorderSize = 0;
            this.btnEditBooking.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnEditBooking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEditBooking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEditBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditBooking.ForeColor = System.Drawing.Color.White;
            this.btnEditBooking.Location = new System.Drawing.Point(446, 585);
            this.btnEditBooking.Name = "btnEditBooking";
            this.btnEditBooking.Size = new System.Drawing.Size(116, 34);
            this.btnEditBooking.TabIndex = 55;
            this.btnEditBooking.Text = "Edit Booking";
            this.btnEditBooking.UseVisualStyleBackColor = false;
            this.btnEditBooking.Click += new System.EventHandler(this.btnEditBooking_Click);
            this.btnEditBooking.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnEditBooking.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(702, 585);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(116, 34);
            this.btnClose.TabIndex = 57;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // btnPrevConfirm
            // 
            this.btnPrevConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevConfirm.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrevConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrevConfirm.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrevConfirm.FlatAppearance.BorderSize = 0;
            this.btnPrevConfirm.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrevConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrevConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrevConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevConfirm.ForeColor = System.Drawing.Color.White;
            this.btnPrevConfirm.Location = new System.Drawing.Point(190, 585);
            this.btnPrevConfirm.Name = "btnPrevConfirm";
            this.btnPrevConfirm.Size = new System.Drawing.Size(116, 34);
            this.btnPrevConfirm.TabIndex = 53;
            this.btnPrevConfirm.Text = "Prev";
            this.btnPrevConfirm.UseVisualStyleBackColor = false;
            this.btnPrevConfirm.Click += new System.EventHandler(this.btnPrevConfirm_Click);
            this.btnPrevConfirm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnPrevConfirm.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // tpAuditTrail
            // 
            this.tpAuditTrail.Controls.Add(this.auditTrailVScrollBar);
            this.tpAuditTrail.Controls.Add(this.lblAuditTrail);
            this.tpAuditTrail.Controls.Add(this.dgvAuditTrail);
            this.tpAuditTrail.Location = new System.Drawing.Point(4, 44);
            this.tpAuditTrail.Name = "tpAuditTrail";
            this.tpAuditTrail.Padding = new System.Windows.Forms.Padding(3);
            this.tpAuditTrail.Size = new System.Drawing.Size(1231, 625);
            this.tpAuditTrail.TabIndex = 5;
            this.tpAuditTrail.Text = "Audit Trail";
            this.tpAuditTrail.UseVisualStyleBackColor = true;
            // 
            // auditTrailVScrollBar
            // 
            this.auditTrailVScrollBar.AutoHide = false;
            this.auditTrailVScrollBar.DataGridView = this.dgvAuditTrail;
            this.auditTrailVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("auditTrailVScrollBar.DownButtonBackgroundImage")));
            this.auditTrailVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("auditTrailVScrollBar.DownButtonDisabledBackgroundImage")));
            this.auditTrailVScrollBar.Location = new System.Drawing.Point(1021, 51);
            this.auditTrailVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.auditTrailVScrollBar.Name = "auditTrailVScrollBar";
            this.auditTrailVScrollBar.ScrollableControl = null;
            this.auditTrailVScrollBar.ScrollViewer = null;
            this.auditTrailVScrollBar.Size = new System.Drawing.Size(40, 428);
            this.auditTrailVScrollBar.TabIndex = 2;
            this.auditTrailVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("auditTrailVScrollBar.UpButtonBackgroundImage")));
            this.auditTrailVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("auditTrailVScrollBar.UpButtonDisabledBackgroundImage")));
            // 
            // dgvAuditTrail
            // 
            this.dgvAuditTrail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAuditTrail.Location = new System.Drawing.Point(93, 51);
            this.dgvAuditTrail.Name = "dgvAuditTrail";
            this.dgvAuditTrail.ReadOnly = true;
            this.dgvAuditTrail.RowTemplate.Height = 30;
            this.dgvAuditTrail.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvAuditTrail.Size = new System.Drawing.Size(925, 428);
            this.dgvAuditTrail.TabIndex = 58;
            // 
            // lblAuditTrail
            // 
            this.lblAuditTrail.AutoSize = true;
            this.lblAuditTrail.Location = new System.Drawing.Point(90, 22);
            this.lblAuditTrail.Name = "lblAuditTrail";
            this.lblAuditTrail.Size = new System.Drawing.Size(81, 15);
            this.lblAuditTrail.TabIndex = 1;
            this.lblAuditTrail.Text = "Booking Audit";
            // 
            // eventLogBindingSource
            // 
            this.eventLogBindingSource.DataMember = "EventLog";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Product Name";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 200;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Product Id";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Package Id";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Visible = false;
            this.dataGridViewTextBoxColumn3.Width = 5;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Remarks";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 300;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 300;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "SelectedStatus";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn6.HeaderText = "Qty.";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 70;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn7.HeaderText = "Price";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 70;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Product Type";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "BookingProductId";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Remarks";
            this.dataGridViewTextBoxColumn10.MinimumWidth = 305;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Width = 305;
            // 
            // frmMakeReservation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1240, 691);
            this.Controls.Add(this.tcBooking);
            this.Controls.Add(this.txtMessage);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMakeReservation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Make a Booking";
            this.Deactivate += new System.EventHandler(this.frmMakeReservation_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMakeReservation_FormClosing);
            this.Load += new System.EventHandler(this.FrmMakeReservation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackageDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookingProducts)).EndInit();
            this.tcBooking.ResumeLayout(false);
            this.tpDateTime.ResumeLayout(false);
            this.tpDateTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttractionSchedules)).EndInit();
            this.tpCustomer.ResumeLayout(false);
            this.tpCustomer.PerformLayout();
            this.tpPackage.ResumeLayout(false);
            this.tpPackage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackageList)).EndInit();
            this.tpAdditional.ResumeLayout(false);
            this.tpConfirm.ResumeLayout(false);
            this.tpAuditTrail.ResumeLayout(false);
            this.tpAuditTrail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditTrail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventLogBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AutoCompleteComboBox cmbBookingClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpforDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.ComboBox cmbFromTime;
        private System.Windows.Forms.NumericUpDown nudQuantity;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Button btnBook;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel groupBox1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnEmail;
        private System.Windows.Forms.Button btnCancelBooking;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label lblReservationCode;
        private System.Windows.Forms.Label lblExpires;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvPackageDetails;
        private System.Windows.Forms.Button btnAttendeeDetails;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbToTime; 
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtBookingEstimate; 
        private System.Windows.Forms.ComboBox cmbRecurFrequency;
        private System.Windows.Forms.DateTimePicker dtpRecurUntil;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.DataGridView dgvBookingProducts;
        private System.Windows.Forms.Button btnPayment;
        private System.Windows.Forms.TextBox txtPaidAmount;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox txtAdvanceAmount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabControl tcBooking;
        private System.Windows.Forms.TabPage tpDateTime;
        private System.Windows.Forms.TabPage tpCustomer;
        private System.Windows.Forms.TabPage tpPackage;
        private System.Windows.Forms.TabPage tpAdditional;
        private System.Windows.Forms.TabPage tpConfirm;
        private System.Windows.Forms.Button btnNextDateTime;
        private System.Windows.Forms.Button btnPrevCustomer;
        private System.Windows.Forms.Button btnNextCustomer;
        private System.Windows.Forms.Button btnPrevPackage;
        private System.Windows.Forms.Button btnNextPackage;
        private System.Windows.Forms.Button btnPrevAdditionalProducts;
        private System.Windows.Forms.Button btnNextAdditionalProducts;
        private System.Windows.Forms.Button btnPrevConfirm;
        private System.Windows.Forms.Button btnAddProducts;
        private System.Windows.Forms.ComboBox cmbPackage;
        private System.Windows.Forms.LinkLabel lnkUpdate;
        private System.Windows.Forms.Button btnCustomerLookup;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnEditBooking;
        private System.Windows.Forms.DataGridView dgvPackageList;
        //private System.Windows.Forms.Button btnCheckAvailability;
        private System.Windows.Forms.TabPage tpAuditTrail;
        private System.Windows.Forms.Label lblAuditTrail;
        private System.Windows.Forms.DataGridView dgvAuditTrail;
        private System.Windows.Forms.TextBox txtDiscount;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.TextBox txtTransactionAmount;
        private System.Windows.Forms.Label lblTransactionAmount;
       // private parafaitEvenDataSet parafaitEventDataSet;
        private System.Windows.Forms.BindingSource eventLogBindingSource;
        private System.Windows.Forms.Label label19;
        //private System.Windows.Forms.Button btnChooseSchedules;
        private System.Windows.Forms.TextBox txtBalanceAmount;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Panel pnlCustomerDetail;
        private System.Windows.Forms.TextBox txtBookingName;
        private System.Windows.Forms.Label label10;
        private AutoCompleteComboBox cmbChannel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnAddServiceCharge;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView packageListHScrollBar;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView packageListVScrollBar;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView additionProdListVScrollbar;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView additionalProdListHScrollbar;
        private System.Windows.Forms.Button btnDiscounts;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxEmailSent;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxRecur;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView auditTrailVScrollBar;
        private System.Windows.Forms.Label lblSchedule;
        private System.Windows.Forms.DataGridView dgvAttractionSchedules;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView attrScheduleVScrollBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label lblCardNumber;
        private AutoCompleteComboBox cmbFacility;
        private System.Windows.Forms.Label lblFacility;
        private System.Windows.Forms.Button btnBlockSchedule;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxMorning;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxNight;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxAfternoon;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxEarlyMorning;
        private System.Windows.Forms.TextBox txtGuests;
        private System.Windows.Forms.Button btnIncreaseGuestCount;
        private System.Windows.Forms.Button btnReduceGuestCount;
        private System.Windows.Forms.Label lblTimeSlots;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView attrScheduleHScrollBar;
        private AutoCompleteComboBox cmbToTimeForSearch;
        private AutoCompleteComboBox cmbFromTimeForSearch;
        private System.Windows.Forms.Label lblFromTimeSearch;
        private System.Windows.Forms.Label lblToTimeSearch;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView custPanelVScrollBar;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn packageName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Product_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn package_Id;
        private System.Windows.Forms.DataGridViewComboBoxColumn discountName;
        private System.Windows.Forms.DataGridViewComboBoxColumn transactionProfileId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn SelectedStatus;
        private System.Windows.Forms.Button btnAddAttendeesInSummary;
        private System.Windows.Forms.Button btnTrxProfiles;
        private System.Windows.Forms.Label lblAppliedTrxProfileName;
        private System.Windows.Forms.Label lblAppliedTrxProfileID;
        private System.Windows.Forms.DataGridViewButtonColumn dcDelete;
        private System.Windows.Forms.DataGridViewComboBoxColumn dcProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcBookingProductId;
        private System.Windows.Forms.DataGridViewComboBoxColumn AdditionalDiscountName;
        private System.Windows.Forms.DataGridViewComboBoxColumn additionalTransactionProfileId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcRemarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineId;
    }
}