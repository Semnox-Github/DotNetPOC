using Semnox.Core.GenericUtilities;

namespace Parafait_POS.Reservation
{
    partial class frmReservations
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReservations));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dtpfromDate = new System.Windows.Forms.DateTimePicker();
            this.lblBookingFromDate = new System.Windows.Forms.Label();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtReservationCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.dgvBookingDetails = new System.Windows.Forms.DataGridView();
            this.btnNewReservation = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabCalendar = new System.Windows.Forms.TabControl();
            this.tabPageDay = new System.Windows.Forms.TabPage();
            this.calendarDayVScrollBarView = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.panelDay = new System.Windows.Forms.Panel();
            this.dgvDay = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDayHeader = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageWeek = new System.Windows.Forms.TabPage();
            this.calendarWeekVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.panelDGV = new System.Windows.Forms.Panel();
            this.dgvWeek = new System.Windows.Forms.DataGridView();
            this.dayOne = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayTwo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayThree = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayFour = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dayFive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.daySix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.daySeven = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvWeekHeader = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageList = new System.Windows.Forms.TabPage();
            this.calendarAllHScrollBar = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.dgvAll = new System.Windows.Forms.DataGridView();
            this.bookingIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookingClassIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookingNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reservationCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxNetAmountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recurFlagDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recurFrequencyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recurEndDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expiryTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.channelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contactNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alternateContactNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isEmailSentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.genderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.postalAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookingProductIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attractionScheduleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extraGuestsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastupdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reservationDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.calendarAllVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.lnkExcelDownload = new System.Windows.Forms.LinkLabel();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuNewBooking = new System.Windows.Forms.ToolStripMenuItem();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbBookingProducts = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.btnPrevWeek = new System.Windows.Forms.Button();
            this.btnNextWeek = new System.Windows.Forms.Button();
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.lblComplete = new System.Windows.Forms.Label();
            this.lblCancelled = new System.Windows.Forms.Label();
            this.lblConfirmed = new System.Windows.Forms.Label();
            this.lblBooked = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCompleteColor = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblBookingToDate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookingDetailsScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.lbxBookingStatus = new System.Windows.Forms.ListBox();
            this.lblFacility = new System.Windows.Forms.Label();
            this.cmbFacility = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookingDetails)).BeginInit();
            this.tabCalendar.SuspendLayout();
            this.tabPageDay.SuspendLayout();
            this.panelDay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDayHeader)).BeginInit();
            this.tabPageWeek.SuspendLayout();
            this.panelDGV.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeek)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeekHeader)).BeginInit();
            this.tabPageList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reservationDTOBindingSource)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(989, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(70, 50);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "Search";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnRefresh.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseDown);
            this.btnRefresh.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseUp);
            // 
            // dtpfromDate
            // 
            this.dtpfromDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpfromDate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpfromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpfromDate.Location = new System.Drawing.Point(50, 10);
            this.dtpfromDate.Name = "dtpfromDate";
            this.dtpfromDate.Size = new System.Drawing.Size(214, 30);
            this.dtpfromDate.TabIndex = 2;
            this.dtpfromDate.ValueChanged += new System.EventHandler(this.DatePicker_ValueChanged);
            this.dtpfromDate.Enter += new System.EventHandler(this.dtpfromDate_Enter);
            // 
            // lblBookingFromDate
            // 
            this.lblBookingFromDate.Location = new System.Drawing.Point(9, 10);
            this.lblBookingFromDate.Name = "lblBookingFromDate";
            this.lblBookingFromDate.Size = new System.Drawing.Size(39, 30);
            this.lblBookingFromDate.TabIndex = 3;
            this.lblBookingFromDate.Text = "From:";
            this.lblBookingFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.Location = new System.Drawing.Point(389, 55);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(107, 30);
            this.txtCardNumber.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(302, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 30);
            this.label3.TabIndex = 7;
            this.label3.Text = "Card Number:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(278, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 30);
            this.label4.TabIndex = 9;
            this.label4.Text = "Reservation Code:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReservationCode
            // 
            this.txtReservationCode.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReservationCode.Location = new System.Drawing.Point(389, 10);
            this.txtReservationCode.Name = "txtReservationCode";
            this.txtReservationCode.Size = new System.Drawing.Size(107, 30);
            this.txtReservationCode.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(796, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 30);
            this.label5.TabIndex = 11;
            this.label5.Text = "Status:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(546, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 30);
            this.label6.TabIndex = 13;
            this.label6.Text = "Customer:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerName.Location = new System.Drawing.Point(612, 55);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(111, 30);
            this.txtCustomerName.TabIndex = 7;
            // 
            // dgvBookingDetails
            // 
            this.dgvBookingDetails.AllowUserToAddRows = false;
            this.dgvBookingDetails.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Transparent;
            this.dgvBookingDetails.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBookingDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvBookingDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBookingDetails.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBookingDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBookingDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBookingDetails.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvBookingDetails.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvBookingDetails.Location = new System.Drawing.Point(937, 136);
            this.dgvBookingDetails.MultiSelect = false;
            this.dgvBookingDetails.Name = "dgvBookingDetails";
            this.dgvBookingDetails.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBookingDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvBookingDetails.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Silver;
            this.dgvBookingDetails.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvBookingDetails.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvBookingDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBookingDetails.Size = new System.Drawing.Size(246, 420);
            this.dgvBookingDetails.TabIndex = 22;
            this.dgvBookingDetails.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBookingDetails_CellDoubleClick);
            this.dgvBookingDetails.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvBookingDetails_CellFormatting);
            this.dgvBookingDetails.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBookingDetails_ColumnHeaderMouseDoubleClick);
            // 
            // btnNewReservation
            // 
            this.btnNewReservation.BackColor = System.Drawing.Color.Transparent;
            this.btnNewReservation.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnNewReservation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNewReservation.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnNewReservation.FlatAppearance.BorderSize = 0;
            this.btnNewReservation.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNewReservation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNewReservation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNewReservation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewReservation.ForeColor = System.Drawing.Color.White;
            this.btnNewReservation.Location = new System.Drawing.Point(1076, 10);
            this.btnNewReservation.Name = "btnNewReservation";
            this.btnNewReservation.Size = new System.Drawing.Size(70, 50);
            this.btnNewReservation.TabIndex = 12;
            this.btnNewReservation.Text = "New";
            this.btnNewReservation.UseVisualStyleBackColor = false;
            this.btnNewReservation.Click += new System.EventHandler(this.btnNewReservation_Click);
            this.btnNewReservation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseDown);
            this.btnNewReservation.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlackButtonMouseUp);
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
            this.btnClose.Location = new System.Drawing.Point(1163, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 50);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseDown);
            this.btnClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueButtonMouseUp);
            // 
            // tabCalendar
            // 
            this.tabCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabCalendar.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabCalendar.Controls.Add(this.tabPageDay);
            this.tabCalendar.Controls.Add(this.tabPageWeek);
            this.tabCalendar.Controls.Add(this.tabPageList);
            this.tabCalendar.Location = new System.Drawing.Point(-2, 109);
            this.tabCalendar.Name = "tabCalendar";
            this.tabCalendar.SelectedIndex = 0;
            this.tabCalendar.Size = new System.Drawing.Size(933, 451);
            this.tabCalendar.TabIndex = 17;
            this.tabCalendar.SelectedIndexChanged += new System.EventHandler(this.tabCalendar_SelectedIndexChanged);
            // 
            // tabPageDay
            // 
            this.tabPageDay.Controls.Add(this.calendarDayVScrollBarView);
            this.tabPageDay.Controls.Add(this.dgvDayHeader);
            this.tabPageDay.Controls.Add(this.panelDay);
            this.tabPageDay.Location = new System.Drawing.Point(4, 27);
            this.tabPageDay.Name = "tabPageDay";
            this.tabPageDay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDay.Size = new System.Drawing.Size(925, 420);
            this.tabPageDay.TabIndex = 2;
            this.tabPageDay.Text = "Day";
            this.tabPageDay.UseVisualStyleBackColor = true;
            // 
            // calendarDayVScrollBarView
            // 
            this.calendarDayVScrollBarView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.calendarDayVScrollBarView.AutoHide = false;
            this.calendarDayVScrollBarView.AutoScroll = true;
            this.calendarDayVScrollBarView.DataGridView = null;
            this.calendarDayVScrollBarView.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarDayVScrollBarView.DownButtonBackgroundImage")));
            this.calendarDayVScrollBarView.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarDayVScrollBarView.DownButtonDisabledBackgroundImage")));
            this.calendarDayVScrollBarView.Location = new System.Drawing.Point(880, 4);
            this.calendarDayVScrollBarView.Margin = new System.Windows.Forms.Padding(0);
            this.calendarDayVScrollBarView.Name = "calendarDayVScrollBarView";
            this.calendarDayVScrollBarView.ScrollableControl = this.panelDay;
            this.calendarDayVScrollBarView.ScrollViewer = null;
            this.calendarDayVScrollBarView.Size = new System.Drawing.Size(40, 409);
            this.calendarDayVScrollBarView.TabIndex = 2;
            this.calendarDayVScrollBarView.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarDayVScrollBarView.UpButtonBackgroundImage")));
            this.calendarDayVScrollBarView.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarDayVScrollBarView.UpButtonDisabledBackgroundImage")));
            // 
            // panelDay
            // 
            this.panelDay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelDay.AutoScroll = true;
            this.panelDay.Controls.Add(this.dgvDay);
            this.panelDay.Location = new System.Drawing.Point(0, 23);
            this.panelDay.Name = "panelDay";
            this.panelDay.Size = new System.Drawing.Size(910, 394);
            this.panelDay.TabIndex = 4;
            // 
            // dgvDay
            // 
            this.dgvDay.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvDay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.dgvDay.Location = new System.Drawing.Point(0, 0);
            this.dgvDay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvDay.Name = "dgvDay";
            this.dgvDay.RowHeadersWidth = 50;
            this.dgvDay.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvDay.Size = new System.Drawing.Size(870, 392);
            this.dgvDay.TabIndex = 1;
            this.dgvDay.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDay_CellDoubleClick);
            this.dgvDay.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvDay_CellPainting);
            this.dgvDay.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvDay_MouseClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "dayOne";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dgvDayHeader
            // 
            this.dgvDayHeader.AllowUserToAddRows = false;
            this.dgvDayHeader.AllowUserToDeleteRows = false;
            this.dgvDayHeader.AllowUserToOrderColumns = true;
            this.dgvDayHeader.AllowUserToResizeColumns = false;
            this.dgvDayHeader.AllowUserToResizeRows = false;
            this.dgvDayHeader.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvDayHeader.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDayHeader.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDayHeader.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDayHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDayHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            this.dgvDayHeader.EnableHeadersVisualStyles = false;
            this.dgvDayHeader.Location = new System.Drawing.Point(0, 4);
            this.dgvDayHeader.Name = "dgvDayHeader";
            this.dgvDayHeader.RowHeadersWidth = 50;
            this.dgvDayHeader.RowTemplate.Height = 12;
            this.dgvDayHeader.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvDayHeader.Size = new System.Drawing.Size(870, 20);
            this.dgvDayHeader.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Column1";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // tabPageWeek
            // 
            this.tabPageWeek.Controls.Add(this.calendarWeekVScrollBar);
            this.tabPageWeek.Controls.Add(this.dgvWeekHeader);
            this.tabPageWeek.Controls.Add(this.panelDGV);
            this.tabPageWeek.Location = new System.Drawing.Point(4, 27);
            this.tabPageWeek.Name = "tabPageWeek";
            this.tabPageWeek.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWeek.Size = new System.Drawing.Size(925, 420);
            this.tabPageWeek.TabIndex = 1;
            this.tabPageWeek.Text = "Week";
            this.tabPageWeek.UseVisualStyleBackColor = true;
            // 
            // calendarWeekVScrollBar
            // 
            this.calendarWeekVScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.calendarWeekVScrollBar.AutoHide = false;
            this.calendarWeekVScrollBar.AutoScroll = true;
            this.calendarWeekVScrollBar.DataGridView = null;
            this.calendarWeekVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarWeekVScrollBar.DownButtonBackgroundImage")));
            this.calendarWeekVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarWeekVScrollBar.DownButtonDisabledBackgroundImage")));
            this.calendarWeekVScrollBar.Location = new System.Drawing.Point(881, 4);
            this.calendarWeekVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.calendarWeekVScrollBar.Name = "calendarWeekVScrollBar";
            this.calendarWeekVScrollBar.ScrollableControl = this.panelDGV;
            this.calendarWeekVScrollBar.ScrollViewer = null;
            this.calendarWeekVScrollBar.Size = new System.Drawing.Size(40, 412);
            this.calendarWeekVScrollBar.TabIndex = 2;
            this.calendarWeekVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarWeekVScrollBar.UpButtonBackgroundImage")));
            this.calendarWeekVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarWeekVScrollBar.UpButtonDisabledBackgroundImage")));
            // 
            // panelDGV
            // 
            this.panelDGV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelDGV.AutoScroll = true;
            this.panelDGV.Controls.Add(this.dgvWeek);
            this.panelDGV.Location = new System.Drawing.Point(0, 23);
            this.panelDGV.Name = "panelDGV";
            this.panelDGV.Size = new System.Drawing.Size(910, 397);
            this.panelDGV.TabIndex = 3;
            // 
            // dgvWeek
            // 
            this.dgvWeek.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvWeek.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWeek.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dayOne,
            this.dayTwo,
            this.dayThree,
            this.dayFour,
            this.dayFive,
            this.daySix,
            this.daySeven});
            this.dgvWeek.Location = new System.Drawing.Point(0, 0);
            this.dgvWeek.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvWeek.Name = "dgvWeek";
            this.dgvWeek.RowHeadersWidth = 50;
            this.dgvWeek.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvWeek.Size = new System.Drawing.Size(870, 393);
            this.dgvWeek.TabIndex = 1;
            this.dgvWeek.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWeek_CellDoubleClick);
            this.dgvWeek.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvWeek_CellPainting);
            this.dgvWeek.SelectionChanged += new System.EventHandler(this.dgvWeek_SelectionChanged);
            this.dgvWeek.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvWeek_MouseClick);
            // 
            // dayOne
            // 
            this.dayOne.HeaderText = "dayOne";
            this.dayOne.Name = "dayOne";
            this.dayOne.Width = 116;
            // 
            // dayTwo
            // 
            this.dayTwo.HeaderText = "dayTwo";
            this.dayTwo.Name = "dayTwo";
            this.dayTwo.Width = 116;
            // 
            // dayThree
            // 
            this.dayThree.HeaderText = "dayThree";
            this.dayThree.Name = "dayThree";
            this.dayThree.Width = 116;
            // 
            // dayFour
            // 
            this.dayFour.HeaderText = "dayFour";
            this.dayFour.Name = "dayFour";
            this.dayFour.Width = 116;
            // 
            // dayFive
            // 
            this.dayFive.HeaderText = "dayFive";
            this.dayFive.Name = "dayFive";
            this.dayFive.Width = 116;
            // 
            // daySix
            // 
            this.daySix.HeaderText = "daySix";
            this.daySix.Name = "daySix";
            this.daySix.Width = 116;
            // 
            // daySeven
            // 
            this.daySeven.HeaderText = "daySeven";
            this.daySeven.Name = "daySeven";
            this.daySeven.Width = 116;
            // 
            // dgvWeekHeader
            // 
            this.dgvWeekHeader.AllowUserToAddRows = false;
            this.dgvWeekHeader.AllowUserToDeleteRows = false;
            this.dgvWeekHeader.AllowUserToOrderColumns = true;
            this.dgvWeekHeader.AllowUserToResizeColumns = false;
            this.dgvWeekHeader.AllowUserToResizeRows = false;
            this.dgvWeekHeader.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvWeekHeader.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvWeekHeader.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvWeekHeader.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvWeekHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWeekHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7});
            this.dgvWeekHeader.EnableHeadersVisualStyles = false;
            this.dgvWeekHeader.Location = new System.Drawing.Point(0, 4);
            this.dgvWeekHeader.Name = "dgvWeekHeader";
            this.dgvWeekHeader.RowHeadersWidth = 50;
            this.dgvWeekHeader.RowTemplate.Height = 12;
            this.dgvWeekHeader.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvWeekHeader.Size = new System.Drawing.Size(870, 20);
            this.dgvWeekHeader.TabIndex = 2;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.Width = 116;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.Width = 116;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            this.Column3.Width = 116;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            this.Column4.Width = 116;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Column5";
            this.Column5.Name = "Column5";
            this.Column5.Width = 116;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Column6";
            this.Column6.Name = "Column6";
            this.Column6.Width = 116;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Column7";
            this.Column7.Name = "Column7";
            this.Column7.Width = 116;
            // 
            // tabPageList
            // 
            this.tabPageList.Controls.Add(this.calendarAllHScrollBar);
            this.tabPageList.Controls.Add(this.calendarAllVScrollBar);
            this.tabPageList.Controls.Add(this.dgvAll);
            this.tabPageList.Location = new System.Drawing.Point(4, 27);
            this.tabPageList.Name = "tabPageList";
            this.tabPageList.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageList.Size = new System.Drawing.Size(925, 420);
            this.tabPageList.TabIndex = 0;
            this.tabPageList.Text = "List";
            this.tabPageList.UseVisualStyleBackColor = true;
            // 
            // calendarAllHScrollBar
            // 
            this.calendarAllHScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calendarAllHScrollBar.AutoHide = false;
            this.calendarAllHScrollBar.DataGridView = this.dgvAll;
            this.calendarAllHScrollBar.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarAllHScrollBar.LeftButtonBackgroundImage")));
            this.calendarAllHScrollBar.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarAllHScrollBar.LeftButtonDisabledBackgroundImage")));
            this.calendarAllHScrollBar.Location = new System.Drawing.Point(3, 377);
            this.calendarAllHScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.calendarAllHScrollBar.Name = "calendarAllHScrollBar";
            this.calendarAllHScrollBar.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarAllHScrollBar.RightButtonBackgroundImage")));
            this.calendarAllHScrollBar.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarAllHScrollBar.RightButtonDisabledBackgroundImage")));
            this.calendarAllHScrollBar.ScrollableControl = null;
            this.calendarAllHScrollBar.ScrollViewer = null;
            this.calendarAllHScrollBar.Size = new System.Drawing.Size(875, 40);
            this.calendarAllHScrollBar.TabIndex = 3;
            // 
            // dgvAll
            // 
            this.dgvAll.AllowUserToAddRows = false;
            this.dgvAll.AllowUserToDeleteRows = false;
            this.dgvAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAll.AutoGenerateColumns = false;
            this.dgvAll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAll.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.bookingIdDataGridViewTextBoxColumn,
            this.bookingClassIdDataGridViewTextBoxColumn,
            this.bookingNameDataGridViewTextBoxColumn,
            this.fromDateDataGridViewTextBoxColumn,
            this.toDateDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.reservationCodeDataGridViewTextBoxColumn,
            this.customerNameDataGridViewTextBoxColumn,
            this.cardNumberDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.trxNetAmountDataGridViewTextBoxColumn,
            this.trxStatusDataGridViewTextBoxColumn,
            this.trxIdDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.recurFlagDataGridViewTextBoxColumn,
            this.recurFrequencyDataGridViewTextBoxColumn,
            this.recurEndDateDataGridViewTextBoxColumn,
            this.cardIdDataGridViewTextBoxColumn,
            this.customerIdDataGridViewTextBoxColumn,
            this.expiryTimeDataGridViewTextBoxColumn,
            this.channelDataGridViewTextBoxColumn,
            this.contactNoDataGridViewTextBoxColumn,
            this.alternateContactNoDataGridViewTextBoxColumn,
            this.emailDataGridViewTextBoxColumn,
            this.isEmailSentDataGridViewTextBoxColumn,
            this.trxNumberDataGridViewTextBoxColumn,
            this.ageDataGridViewTextBoxColumn,
            this.genderDataGridViewTextBoxColumn,
            this.postalAddressDataGridViewTextBoxColumn,
            this.bookingProductIdDataGridViewTextBoxColumn,
            this.attractionScheduleIdDataGridViewTextBoxColumn,
            this.extraGuestsDataGridViewTextBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastupdateDateDataGridViewTextBoxColumn,
            this.guidDataGridViewTextBoxColumn,
            this.siteIdDataGridViewTextBoxColumn,
            this.synchStatusDataGridViewCheckBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn});
            this.dgvAll.DataSource = this.reservationDTOBindingSource;
            this.dgvAll.Location = new System.Drawing.Point(3, 3);
            this.dgvAll.Name = "dgvAll";
            this.dgvAll.ReadOnly = true;
            this.dgvAll.RowTemplate.Height = 30;
            this.dgvAll.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvAll.Size = new System.Drawing.Size(875, 373);
            this.dgvAll.TabIndex = 20;
            this.dgvAll.SelectionChanged += new System.EventHandler(this.dgvAll_SelectionChanged);
            this.dgvAll.DoubleClick += new System.EventHandler(this.dgvAll_DoubleClick);
            // 
            // bookingIdDataGridViewTextBoxColumn
            // 
            this.bookingIdDataGridViewTextBoxColumn.DataPropertyName = "BookingId";
            this.bookingIdDataGridViewTextBoxColumn.HeaderText = "Booking Id";
            this.bookingIdDataGridViewTextBoxColumn.Name = "bookingIdDataGridViewTextBoxColumn";
            this.bookingIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bookingClassIdDataGridViewTextBoxColumn
            // 
            this.bookingClassIdDataGridViewTextBoxColumn.DataPropertyName = "BookingClassId";
            this.bookingClassIdDataGridViewTextBoxColumn.HeaderText = "BookingClassId";
            this.bookingClassIdDataGridViewTextBoxColumn.Name = "bookingClassIdDataGridViewTextBoxColumn";
            this.bookingClassIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.bookingClassIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // bookingNameDataGridViewTextBoxColumn
            // 
            this.bookingNameDataGridViewTextBoxColumn.DataPropertyName = "BookingName";
            this.bookingNameDataGridViewTextBoxColumn.HeaderText = "Booking Name";
            this.bookingNameDataGridViewTextBoxColumn.Name = "bookingNameDataGridViewTextBoxColumn";
            this.bookingNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fromDateDataGridViewTextBoxColumn
            // 
            this.fromDateDataGridViewTextBoxColumn.DataPropertyName = "FromDate";
            this.fromDateDataGridViewTextBoxColumn.HeaderText = "From Date";
            this.fromDateDataGridViewTextBoxColumn.Name = "fromDateDataGridViewTextBoxColumn";
            this.fromDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // toDateDataGridViewTextBoxColumn
            // 
            this.toDateDataGridViewTextBoxColumn.DataPropertyName = "ToDate";
            this.toDateDataGridViewTextBoxColumn.HeaderText = "To Date";
            this.toDateDataGridViewTextBoxColumn.Name = "toDateDataGridViewTextBoxColumn";
            this.toDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // reservationCodeDataGridViewTextBoxColumn
            // 
            this.reservationCodeDataGridViewTextBoxColumn.DataPropertyName = "ReservationCode";
            this.reservationCodeDataGridViewTextBoxColumn.HeaderText = "Reservation Code";
            this.reservationCodeDataGridViewTextBoxColumn.Name = "reservationCodeDataGridViewTextBoxColumn";
            this.reservationCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // customerNameDataGridViewTextBoxColumn
            // 
            this.customerNameDataGridViewTextBoxColumn.DataPropertyName = "CustomerName";
            this.customerNameDataGridViewTextBoxColumn.HeaderText = "Customer Name";
            this.customerNameDataGridViewTextBoxColumn.Name = "customerNameDataGridViewTextBoxColumn";
            this.customerNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cardNumberDataGridViewTextBoxColumn
            // 
            this.cardNumberDataGridViewTextBoxColumn.DataPropertyName = "CardNumber";
            this.cardNumberDataGridViewTextBoxColumn.HeaderText = "Card Number";
            this.cardNumberDataGridViewTextBoxColumn.Name = "cardNumberDataGridViewTextBoxColumn";
            this.cardNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // trxNetAmountDataGridViewTextBoxColumn
            // 
            this.trxNetAmountDataGridViewTextBoxColumn.DataPropertyName = "TrxNetAmount";
            this.trxNetAmountDataGridViewTextBoxColumn.HeaderText = "Trx Net Amount";
            this.trxNetAmountDataGridViewTextBoxColumn.Name = "trxNetAmountDataGridViewTextBoxColumn";
            this.trxNetAmountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // trxStatusDataGridViewTextBoxColumn
            // 
            this.trxStatusDataGridViewTextBoxColumn.DataPropertyName = "TrxStatus";
            this.trxStatusDataGridViewTextBoxColumn.HeaderText = "Trx Status";
            this.trxStatusDataGridViewTextBoxColumn.Name = "trxStatusDataGridViewTextBoxColumn";
            this.trxStatusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // trxIdDataGridViewTextBoxColumn
            // 
            this.trxIdDataGridViewTextBoxColumn.DataPropertyName = "TrxId";
            this.trxIdDataGridViewTextBoxColumn.HeaderText = "TrxId";
            this.trxIdDataGridViewTextBoxColumn.Name = "trxIdDataGridViewTextBoxColumn";
            this.trxIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            this.remarksDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // recurFlagDataGridViewTextBoxColumn
            // 
            this.recurFlagDataGridViewTextBoxColumn.DataPropertyName = "RecurFlag";
            this.recurFlagDataGridViewTextBoxColumn.HeaderText = "Recurring?";
            this.recurFlagDataGridViewTextBoxColumn.Name = "recurFlagDataGridViewTextBoxColumn";
            this.recurFlagDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // recurFrequencyDataGridViewTextBoxColumn
            // 
            this.recurFrequencyDataGridViewTextBoxColumn.DataPropertyName = "RecurFrequency";
            this.recurFrequencyDataGridViewTextBoxColumn.HeaderText = "Recur Frequency";
            this.recurFrequencyDataGridViewTextBoxColumn.Name = "recurFrequencyDataGridViewTextBoxColumn";
            this.recurFrequencyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // recurEndDateDataGridViewTextBoxColumn
            // 
            this.recurEndDateDataGridViewTextBoxColumn.DataPropertyName = "RecurEndDate";
            this.recurEndDateDataGridViewTextBoxColumn.HeaderText = "Recur EndDate";
            this.recurEndDateDataGridViewTextBoxColumn.Name = "recurEndDateDataGridViewTextBoxColumn";
            this.recurEndDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cardIdDataGridViewTextBoxColumn
            // 
            this.cardIdDataGridViewTextBoxColumn.DataPropertyName = "CardId";
            this.cardIdDataGridViewTextBoxColumn.HeaderText = "CardId";
            this.cardIdDataGridViewTextBoxColumn.Name = "cardIdDataGridViewTextBoxColumn";
            this.cardIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.cardIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // customerIdDataGridViewTextBoxColumn
            // 
            this.customerIdDataGridViewTextBoxColumn.DataPropertyName = "CustomerId";
            this.customerIdDataGridViewTextBoxColumn.HeaderText = "Customer Id";
            this.customerIdDataGridViewTextBoxColumn.Name = "customerIdDataGridViewTextBoxColumn";
            this.customerIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // expiryTimeDataGridViewTextBoxColumn
            // 
            this.expiryTimeDataGridViewTextBoxColumn.DataPropertyName = "ExpiryTime";
            this.expiryTimeDataGridViewTextBoxColumn.HeaderText = "Expiry Time";
            this.expiryTimeDataGridViewTextBoxColumn.Name = "expiryTimeDataGridViewTextBoxColumn";
            this.expiryTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.expiryTimeDataGridViewTextBoxColumn.Visible = false;
            // 
            // channelDataGridViewTextBoxColumn
            // 
            this.channelDataGridViewTextBoxColumn.DataPropertyName = "Channel";
            this.channelDataGridViewTextBoxColumn.HeaderText = "Channel";
            this.channelDataGridViewTextBoxColumn.Name = "channelDataGridViewTextBoxColumn";
            this.channelDataGridViewTextBoxColumn.ReadOnly = true;
            this.channelDataGridViewTextBoxColumn.Visible = false;
            // 
            // contactNoDataGridViewTextBoxColumn
            // 
            this.contactNoDataGridViewTextBoxColumn.DataPropertyName = "ContactNo";
            this.contactNoDataGridViewTextBoxColumn.HeaderText = "ContactNo";
            this.contactNoDataGridViewTextBoxColumn.Name = "contactNoDataGridViewTextBoxColumn";
            this.contactNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.contactNoDataGridViewTextBoxColumn.Visible = false;
            // 
            // alternateContactNoDataGridViewTextBoxColumn
            // 
            this.alternateContactNoDataGridViewTextBoxColumn.DataPropertyName = "AlternateContactNo";
            this.alternateContactNoDataGridViewTextBoxColumn.HeaderText = "AlternateContactNo";
            this.alternateContactNoDataGridViewTextBoxColumn.Name = "alternateContactNoDataGridViewTextBoxColumn";
            this.alternateContactNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.alternateContactNoDataGridViewTextBoxColumn.Visible = false;
            // 
            // emailDataGridViewTextBoxColumn
            // 
            this.emailDataGridViewTextBoxColumn.DataPropertyName = "Email";
            this.emailDataGridViewTextBoxColumn.HeaderText = "Email";
            this.emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";
            this.emailDataGridViewTextBoxColumn.ReadOnly = true;
            this.emailDataGridViewTextBoxColumn.Visible = false;
            // 
            // isEmailSentDataGridViewTextBoxColumn
            // 
            this.isEmailSentDataGridViewTextBoxColumn.DataPropertyName = "IsEmailSent";
            this.isEmailSentDataGridViewTextBoxColumn.HeaderText = "IsEmailSent";
            this.isEmailSentDataGridViewTextBoxColumn.Name = "isEmailSentDataGridViewTextBoxColumn";
            this.isEmailSentDataGridViewTextBoxColumn.ReadOnly = true;
            this.isEmailSentDataGridViewTextBoxColumn.Visible = false;
            // 
            // trxNumberDataGridViewTextBoxColumn
            // 
            this.trxNumberDataGridViewTextBoxColumn.DataPropertyName = "TrxNumber";
            this.trxNumberDataGridViewTextBoxColumn.HeaderText = "TrxNumber";
            this.trxNumberDataGridViewTextBoxColumn.Name = "trxNumberDataGridViewTextBoxColumn";
            this.trxNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.trxNumberDataGridViewTextBoxColumn.Visible = false;
            // 
            // ageDataGridViewTextBoxColumn
            // 
            this.ageDataGridViewTextBoxColumn.DataPropertyName = "Age";
            this.ageDataGridViewTextBoxColumn.HeaderText = "Age";
            this.ageDataGridViewTextBoxColumn.Name = "ageDataGridViewTextBoxColumn";
            this.ageDataGridViewTextBoxColumn.ReadOnly = true;
            this.ageDataGridViewTextBoxColumn.Visible = false;
            // 
            // genderDataGridViewTextBoxColumn
            // 
            this.genderDataGridViewTextBoxColumn.DataPropertyName = "Gender";
            this.genderDataGridViewTextBoxColumn.HeaderText = "Gender";
            this.genderDataGridViewTextBoxColumn.Name = "genderDataGridViewTextBoxColumn";
            this.genderDataGridViewTextBoxColumn.ReadOnly = true;
            this.genderDataGridViewTextBoxColumn.Visible = false;
            // 
            // postalAddressDataGridViewTextBoxColumn
            // 
            this.postalAddressDataGridViewTextBoxColumn.DataPropertyName = "PostalAddress";
            this.postalAddressDataGridViewTextBoxColumn.HeaderText = "PostalAddress";
            this.postalAddressDataGridViewTextBoxColumn.Name = "postalAddressDataGridViewTextBoxColumn";
            this.postalAddressDataGridViewTextBoxColumn.ReadOnly = true;
            this.postalAddressDataGridViewTextBoxColumn.Visible = false;
            // 
            // bookingProductIdDataGridViewTextBoxColumn
            // 
            this.bookingProductIdDataGridViewTextBoxColumn.DataPropertyName = "BookingProductId";
            this.bookingProductIdDataGridViewTextBoxColumn.HeaderText = "BookingProductId";
            this.bookingProductIdDataGridViewTextBoxColumn.Name = "bookingProductIdDataGridViewTextBoxColumn";
            this.bookingProductIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.bookingProductIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // attractionScheduleIdDataGridViewTextBoxColumn
            // 
            this.attractionScheduleIdDataGridViewTextBoxColumn.DataPropertyName = "AttractionScheduleId";
            this.attractionScheduleIdDataGridViewTextBoxColumn.HeaderText = "AttractionScheduleId";
            this.attractionScheduleIdDataGridViewTextBoxColumn.Name = "attractionScheduleIdDataGridViewTextBoxColumn";
            this.attractionScheduleIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.attractionScheduleIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // extraGuestsDataGridViewTextBoxColumn
            // 
            this.extraGuestsDataGridViewTextBoxColumn.DataPropertyName = "ExtraGuests";
            this.extraGuestsDataGridViewTextBoxColumn.HeaderText = "ExtraGuests";
            this.extraGuestsDataGridViewTextBoxColumn.Name = "extraGuestsDataGridViewTextBoxColumn";
            this.extraGuestsDataGridViewTextBoxColumn.ReadOnly = true;
            this.extraGuestsDataGridViewTextBoxColumn.Visible = false;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "Created By";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdByDataGridViewTextBoxColumn.Visible = false;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "Created Date";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.creationDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastUpdatedByDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastupdateDateDataGridViewTextBoxColumn
            // 
            this.lastupdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastupdateDate";
            this.lastupdateDateDataGridViewTextBoxColumn.HeaderText = "Updated Date";
            this.lastupdateDateDataGridViewTextBoxColumn.Name = "lastupdateDateDataGridViewTextBoxColumn";
            this.lastupdateDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastupdateDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn
            // 
            this.guidDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn.Name = "guidDataGridViewTextBoxColumn";
            this.guidDataGridViewTextBoxColumn.ReadOnly = true;
            this.guidDataGridViewTextBoxColumn.Visible = false;
            // 
            // siteIdDataGridViewTextBoxColumn
            // 
            this.siteIdDataGridViewTextBoxColumn.DataPropertyName = "SiteId";
            this.siteIdDataGridViewTextBoxColumn.HeaderText = "SiteId";
            this.siteIdDataGridViewTextBoxColumn.Name = "siteIdDataGridViewTextBoxColumn";
            this.siteIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.siteIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn
            // 
            this.synchStatusDataGridViewCheckBoxColumn.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.Name = "synchStatusDataGridViewCheckBoxColumn";
            this.synchStatusDataGridViewCheckBoxColumn.ReadOnly = true;
            this.synchStatusDataGridViewCheckBoxColumn.Visible = false;
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // reservationDTOBindingSource
            // 
            this.reservationDTOBindingSource.DataSource = typeof(Semnox.Parafait.Transaction.ReservationDTO);
            // 
            // calendarAllVScrollBar
            // 
            this.calendarAllVScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.calendarAllVScrollBar.AutoHide = false;
            this.calendarAllVScrollBar.AutoScroll = true;
            this.calendarAllVScrollBar.DataGridView = this.dgvAll;
            this.calendarAllVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarAllVScrollBar.DownButtonBackgroundImage")));
            this.calendarAllVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarAllVScrollBar.DownButtonDisabledBackgroundImage")));
            this.calendarAllVScrollBar.Location = new System.Drawing.Point(881, 3);
            this.calendarAllVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.calendarAllVScrollBar.Name = "calendarAllVScrollBar";
            this.calendarAllVScrollBar.ScrollableControl = null;
            this.calendarAllVScrollBar.ScrollViewer = null;
            this.calendarAllVScrollBar.Size = new System.Drawing.Size(40, 414);
            this.calendarAllVScrollBar.TabIndex = 21;
            this.calendarAllVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarAllVScrollBar.UpButtonBackgroundImage")));
            this.calendarAllVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("calendarAllVScrollBar.UpButtonDisabledBackgroundImage")));
            // 
            // lnkExcelDownload
            // 
            this.lnkExcelDownload.AutoSize = true;
            this.lnkExcelDownload.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExcelDownload.LinkColor = System.Drawing.Color.Maroon;
            this.lnkExcelDownload.Location = new System.Drawing.Point(292, 111);
            this.lnkExcelDownload.Name = "lnkExcelDownload";
            this.lnkExcelDownload.Size = new System.Drawing.Size(146, 22);
            this.lnkExcelDownload.TabIndex = 16;
            this.lnkExcelDownload.TabStop = true;
            this.lnkExcelDownload.Text = "Excel Download";
            this.lnkExcelDownload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lnkExcelDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExcelDownload_LinkClicked);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNewBooking});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(146, 26);
            this.contextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_ItemClicked);
            // 
            // menuNewBooking
            // 
            this.menuNewBooking.Image = ((System.Drawing.Image)(resources.GetObject("menuNewBooking.Image")));
            this.menuNewBooking.Name = "menuNewBooking";
            this.menuNewBooking.Size = new System.Drawing.Size(145, 22);
            this.menuNewBooking.Text = "New Booking";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(498, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 30);
            this.label7.TabIndex = 19;
            this.label7.Text = "Booking Product:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbBookingProducts
            // 
            this.cmbBookingProducts.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBookingProducts.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBookingProducts.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBookingProducts.FormattingEnabled = true;
            this.cmbBookingProducts.Items.AddRange(new object[] {
            "BOOKED",
            "CONFIRMED",
            "CANCELLED"});
            this.cmbBookingProducts.Location = new System.Drawing.Point(612, 10);
            this.cmbBookingProducts.Name = "cmbBookingProducts";
            this.cmbBookingProducts.Size = new System.Drawing.Size(180, 31);
            this.cmbBookingProducts.TabIndex = 6;
            // 
            // btnPrevWeek
            // 
            this.btnPrevWeek.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevWeek.BackgroundImage = global::Parafait_POS.Properties.Resources.BackwardBtn;
            this.btnPrevWeek.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrevWeek.FlatAppearance.BorderSize = 0;
            this.btnPrevWeek.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrevWeek.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrevWeek.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrevWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevWeek.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevWeek.ForeColor = System.Drawing.Color.White;
            this.btnPrevWeek.Location = new System.Drawing.Point(169, 102);
            this.btnPrevWeek.Name = "btnPrevWeek";
            this.btnPrevWeek.Size = new System.Drawing.Size(40, 36);
            this.btnPrevWeek.TabIndex = 14;
            this.btnPrevWeek.UseVisualStyleBackColor = false;
            this.btnPrevWeek.Click += new System.EventHandler(this.btnPrevWeek_Click);
            this.btnPrevWeek.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBackwardMouseDown);
            this.btnPrevWeek.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnBackwardMouseUp);
            // 
            // btnNextWeek
            // 
            this.btnNextWeek.BackColor = System.Drawing.Color.Transparent;
            this.btnNextWeek.BackgroundImage = global::Parafait_POS.Properties.Resources.ForwardBtn;
            this.btnNextWeek.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextWeek.FlatAppearance.BorderSize = 0;
            this.btnNextWeek.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNextWeek.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextWeek.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextWeek.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextWeek.ForeColor = System.Drawing.Color.White;
            this.btnNextWeek.Location = new System.Drawing.Point(227, 102);
            this.btnNextWeek.Name = "btnNextWeek";
            this.btnNextWeek.Size = new System.Drawing.Size(40, 36);
            this.btnNextWeek.TabIndex = 15;
            this.btnNextWeek.UseVisualStyleBackColor = false;
            this.btnNextWeek.Click += new System.EventHandler(this.btnNextWeek_Click);
            this.btnNextWeek.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnForwardMouseDown);
            this.btnNextWeek.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnForwardMouseUp);
            // 
            // lnkClearSearch
            // 
            this.lnkClearSearch.AutoSize = true;
            this.lnkClearSearch.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkClearSearch.Location = new System.Drawing.Point(989, 70);
            this.lnkClearSearch.Name = "lnkClearSearch";
            this.lnkClearSearch.Size = new System.Drawing.Size(55, 22);
            this.lnkClearSearch.TabIndex = 11;
            this.lnkClearSearch.TabStop = true;
            this.lnkClearSearch.Text = "Clear";
            this.lnkClearSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lnkClearSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearSearch_LinkClicked);
            // 
            // lblComplete
            // 
            this.lblComplete.AutoSize = true;
            this.lblComplete.Location = new System.Drawing.Point(1073, 67);
            this.lblComplete.Name = "lblComplete";
            this.lblComplete.Size = new System.Drawing.Size(72, 15);
            this.lblComplete.TabIndex = 3;
            this.lblComplete.Text = "COMPLETE";
            this.lblComplete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCancelled
            // 
            this.lblCancelled.AutoSize = true;
            this.lblCancelled.Location = new System.Drawing.Point(1073, 88);
            this.lblCancelled.Name = "lblCancelled";
            this.lblCancelled.Size = new System.Drawing.Size(80, 15);
            this.lblCancelled.TabIndex = 3;
            this.lblCancelled.Text = "CANCELLED";
            this.lblCancelled.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConfirmed
            // 
            this.lblConfirmed.AutoSize = true;
            this.lblConfirmed.Location = new System.Drawing.Point(1073, 109);
            this.lblConfirmed.Name = "lblConfirmed";
            this.lblConfirmed.Size = new System.Drawing.Size(79, 15);
            this.lblConfirmed.TabIndex = 3;
            this.lblConfirmed.Text = "CONFIRMED";
            this.lblConfirmed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBooked
            // 
            this.lblBooked.AutoSize = true;
            this.lblBooked.Location = new System.Drawing.Point(1180, 67);
            this.lblBooked.Name = "lblBooked";
            this.lblBooked.Size = new System.Drawing.Size(58, 15);
            this.lblBooked.TabIndex = 3;
            this.lblBooked.Text = "BOOKED";
            this.lblBooked.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1181, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "WIP";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCompleteColor
            // 
            this.lblCompleteColor.BackColor = System.Drawing.Color.Aquamarine;
            this.lblCompleteColor.Location = new System.Drawing.Point(1159, 90);
            this.lblCompleteColor.Name = "lblCompleteColor";
            this.lblCompleteColor.Size = new System.Drawing.Size(16, 13);
            this.lblCompleteColor.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.LightGreen;
            this.label8.Location = new System.Drawing.Point(1052, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 13);
            this.label8.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.LightCoral;
            this.label9.Location = new System.Drawing.Point(1052, 89);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 13);
            this.label9.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.LightBlue;
            this.label10.Location = new System.Drawing.Point(1052, 110);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(16, 13);
            this.label10.TabIndex = 23;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.LightPink;
            this.label11.Location = new System.Drawing.Point(1159, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(16, 13);
            this.label11.TabIndex = 23;
            // 
            // lblBookingToDate
            // 
            this.lblBookingToDate.Location = new System.Drawing.Point(11, 55);
            this.lblBookingToDate.Name = "lblBookingToDate";
            this.lblBookingToDate.Size = new System.Drawing.Size(39, 30);
            this.lblBookingToDate.TabIndex = 24;
            this.lblBookingToDate.Text = "To:";
            this.lblBookingToDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpToDate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(50, 55);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(214, 30);
            this.dtpToDate.TabIndex = 3;
            this.dtpToDate.Enter += new System.EventHandler(this.dtpToDate_Enter);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Column1";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 118;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Column2";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 118;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Column3";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 118;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Column4";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 118;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Column5";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 118;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Column6";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 118;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Column7";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 118;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "dayOne";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Width = 118;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "dayTwo";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Width = 118;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "dayThree";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.Width = 118;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "dayFour";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.Width = 118;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.HeaderText = "dayFive";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.Width = 118;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.HeaderText = "daySix";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.Width = 118;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.HeaderText = "daySeven";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.Width = 118;
            // 
            // bookingDetailsScrollBar
            // 
            this.bookingDetailsScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.bookingDetailsScrollBar.AutoHide = false;
            this.bookingDetailsScrollBar.AutoScroll = true;
            this.bookingDetailsScrollBar.DataGridView = this.dgvBookingDetails;
            this.bookingDetailsScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("bookingDetailsScrollBar.DownButtonBackgroundImage")));
            this.bookingDetailsScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("bookingDetailsScrollBar.DownButtonDisabledBackgroundImage")));
            this.bookingDetailsScrollBar.Location = new System.Drawing.Point(1186, 136);
            this.bookingDetailsScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.bookingDetailsScrollBar.Name = "bookingDetailsScrollBar";
            this.bookingDetailsScrollBar.ScrollableControl = null;
            this.bookingDetailsScrollBar.ScrollViewer = null;
            this.bookingDetailsScrollBar.Size = new System.Drawing.Size(40, 416);
            this.bookingDetailsScrollBar.TabIndex = 23;
            this.bookingDetailsScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("bookingDetailsScrollBar.UpButtonBackgroundImage")));
            this.bookingDetailsScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("bookingDetailsScrollBar.UpButtonDisabledBackgroundImage")));
            // 
            // lbxBookingStatus
            // 
            this.lbxBookingStatus.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbxBookingStatus.FormattingEnabled = true;
            this.lbxBookingStatus.ItemHeight = 23;
            this.lbxBookingStatus.Location = new System.Drawing.Point(854, 10);
            this.lbxBookingStatus.Name = "lbxBookingStatus";
            this.lbxBookingStatus.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbxBookingStatus.Size = new System.Drawing.Size(120, 119);
            this.lbxBookingStatus.TabIndex = 9;
            // 
            // lblFacility
            // 
            this.lblFacility.Location = new System.Drawing.Point(498, 98);
            this.lblFacility.Name = "lblFacility";
            this.lblFacility.Size = new System.Drawing.Size(112, 30);
            this.lblFacility.TabIndex = 29;
            this.lblFacility.Text = "Facility:";
            this.lblFacility.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbFacility
            // 
            this.cmbFacility.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbFacility.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFacility.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFacility.FormattingEnabled = true;
            this.cmbFacility.Items.AddRange(new object[] {
            "BOOKED",
            "CONFIRMED",
            "CANCELLED"});
            this.cmbFacility.Location = new System.Drawing.Point(612, 98);
            this.cmbFacility.Name = "cmbFacility";
            this.cmbFacility.Size = new System.Drawing.Size(180, 31);
            this.cmbFacility.TabIndex = 8;
            // 
            // frmReservations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1242, 561);
            this.Controls.Add(this.lblFacility);
            this.Controls.Add(this.cmbFacility);
            this.Controls.Add(this.lbxBookingStatus);
            this.Controls.Add(this.lnkExcelDownload);
            this.Controls.Add(this.bookingDetailsScrollBar);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.lblBookingToDate);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblCompleteColor);
            this.Controls.Add(this.lblBooked);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblConfirmed);
            this.Controls.Add(this.lblCancelled);
            this.Controls.Add(this.lblComplete);
            this.Controls.Add(this.lnkClearSearch);
            this.Controls.Add(this.btnNextWeek);
            this.Controls.Add(this.btnPrevWeek);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbBookingProducts);
            this.Controls.Add(this.tabCalendar);
            this.Controls.Add(this.btnNewReservation);
            this.Controls.Add(this.dgvBookingDetails);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCustomerName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtReservationCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.lblBookingFromDate);
            this.Controls.Add(this.dtpfromDate);
            this.Controls.Add(this.btnRefresh);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmReservations";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Reservations";
            this.Deactivate += new System.EventHandler(this.frmReservations_Deactivate);
            this.Load += new System.EventHandler(this.frmReservations_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookingDetails)).EndInit();
            this.tabCalendar.ResumeLayout(false);
            this.tabPageDay.ResumeLayout(false);
            this.panelDay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDayHeader)).EndInit();
            this.tabPageWeek.ResumeLayout(false);
            this.panelDGV.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeek)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeekHeader)).EndInit();
            this.tabPageList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reservationDTOBindingSource)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DateTimePicker dtpfromDate;
        private System.Windows.Forms.Label lblBookingFromDate;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtReservationCode; 
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.DataGridView dgvBookingDetails;
        private System.Windows.Forms.Button btnNewReservation;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabCalendar;
        private System.Windows.Forms.TabPage tabPageList;
        private System.Windows.Forms.TabPage tabPageWeek;
        private System.Windows.Forms.DataGridView dgvWeek;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuNewBooking;
        private System.Windows.Forms.DataGridView dgvAll;
        private System.Windows.Forms.Label label7;
        private AutoCompleteComboBox cmbBookingProducts;
        private System.Windows.Forms.Button btnPrevWeek;
        private System.Windows.Forms.Button btnNextWeek;
        private System.Windows.Forms.LinkLabel lnkClearSearch;
        private System.Windows.Forms.DataGridView dgvWeekHeader;
        private System.Windows.Forms.Panel panelDGV;
        private System.Windows.Forms.LinkLabel lnkExcelDownload;
        private System.Windows.Forms.TabPage tabPageDay;
        private System.Windows.Forms.Panel panelDay;
        private System.Windows.Forms.DataGridView dgvDay;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridView dgvDayHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Label lblComplete;
        private System.Windows.Forms.Label lblCancelled;
        private System.Windows.Forms.Label lblConfirmed;
        private System.Windows.Forms.Label lblBooked;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCompleteColor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblBookingToDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView bookingDetailsScrollBar;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView calendarWeekVScrollBar;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView calendarAllVScrollBar;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView calendarDayVScrollBarView;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView calendarAllHScrollBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dayOne;
        private System.Windows.Forms.DataGridViewTextBoxColumn dayTwo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dayThree;
        private System.Windows.Forms.DataGridViewTextBoxColumn dayFour;
        private System.Windows.Forms.DataGridViewTextBoxColumn dayFive;
        private System.Windows.Forms.DataGridViewTextBoxColumn daySix;
        private System.Windows.Forms.DataGridViewTextBoxColumn daySeven;
        private System.Windows.Forms.ListBox lbxBookingStatus;
        private System.Windows.Forms.Label lblFacility;
        private AutoCompleteComboBox cmbFacility;
        private System.Windows.Forms.BindingSource reservationDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookingIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookingClassIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookingNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reservationCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxNetAmountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recurFlagDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recurFrequencyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recurEndDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expiryTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn channelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn contactNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn alternateContactNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn isEmailSentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn genderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn postalAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookingProductIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attractionScheduleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn extraGuestsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastupdateDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
    }
}