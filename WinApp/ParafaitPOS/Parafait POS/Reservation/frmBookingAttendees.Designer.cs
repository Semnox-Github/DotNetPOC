using System.Drawing;

namespace Parafait_POS.Reservation
{
    partial class frmBookingAttendees
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBookingAttendees));
            this.dgvAttendees = new System.Windows.Forms.DataGridView();
            this.selectRecord = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TrxId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookingIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateofBirth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.genderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Party_In_Name_Of = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PhoneNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WaiversSigned = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.specialRequestDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signWaiverEmailLastSentOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signWaiverEmailSentCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookingAttendeeDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.bgwSetWaiverSigned = new System.ComponentModel.BackgroundWorker();
            this.btnSendWaiverEmail = new System.Windows.Forms.Button();
            this.horizontalScrollBarAttendee = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.verticalScrollBarAttendee = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.lblWaiverStatusMessageOne = new System.Windows.Forms.Label();
            this.lblWaiverStatusMessageTwo = new System.Windows.Forms.Label();
            this.lblWaiverStatusMessageOneVal = new System.Windows.Forms.Label();
            this.lblWaiverStatusMessageTwoVal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendees)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookingAttendeeDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAttendees
            // 
            this.dgvAttendees.AutoGenerateColumns = false;
            this.dgvAttendees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttendees.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectRecord,
            this.idDataGridViewTextBoxColumn,
            this.CustomerId,
            this.TrxId,
            this.bookingIdDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.ageDataGridViewTextBoxColumn,
            this.DateofBirth,
            this.genderDataGridViewTextBoxColumn,
            this.Party_In_Name_Of,
            this.PhoneNumber,
            this.Email,
            this.WaiversSigned,
            this.IsActive,
            this.specialRequestDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn,
            this.signWaiverEmailLastSentOn,
            this.signWaiverEmailSentCount});
            this.dgvAttendees.DataSource = this.bookingAttendeeDTOBindingSource;
            this.dgvAttendees.Location = new System.Drawing.Point(12, 32);
            this.dgvAttendees.Name = "dgvAttendees";
            this.dgvAttendees.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvAttendees.Size = new System.Drawing.Size(1193, 387);
            this.dgvAttendees.TabIndex = 0;
            this.dgvAttendees.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAttendees_CellClick);
            this.dgvAttendees.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAttendees_CellEnter);
            this.dgvAttendees.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvAttendees_CellValidating);
            this.dgvAttendees.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAttendees_DataError);
            this.dgvAttendees.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvAttendees_DefaultValuesNeeded);
            // 
            // selectRecord
            // 
            this.selectRecord.FalseValue = "false";
            this.selectRecord.Frozen = true;
            this.selectRecord.HeaderText = "";
            this.selectRecord.MinimumWidth = 30;
            this.selectRecord.Name = "selectRecord";
            this.selectRecord.ReadOnly = true;
            this.selectRecord.TrueValue = "true";
            this.selectRecord.Width = 40;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.Frozen = true;
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Width = 50;
            // 
            // CustomerId
            // 
            this.CustomerId.DataPropertyName = "CustomerId";
            this.CustomerId.HeaderText = "CustomerId";
            this.CustomerId.Name = "CustomerId";
            this.CustomerId.ReadOnly = true;
            this.CustomerId.Visible = false;
            // 
            // TrxId
            // 
            this.TrxId.DataPropertyName = "TrxId";
            this.TrxId.HeaderText = "TrxId";
            this.TrxId.Name = "TrxId";
            this.TrxId.ReadOnly = true;
            this.TrxId.Visible = false;
            // 
            // bookingIdDataGridViewTextBoxColumn
            // 
            this.bookingIdDataGridViewTextBoxColumn.DataPropertyName = "BookingId";
            this.bookingIdDataGridViewTextBoxColumn.HeaderText = "BookingId";
            this.bookingIdDataGridViewTextBoxColumn.Name = "bookingIdDataGridViewTextBoxColumn";
            this.bookingIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // ageDataGridViewTextBoxColumn
            // 
            this.ageDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ageDataGridViewTextBoxColumn.DataPropertyName = "Age";
            this.ageDataGridViewTextBoxColumn.HeaderText = "Age";
            this.ageDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.ageDataGridViewTextBoxColumn.Name = "ageDataGridViewTextBoxColumn";
            this.ageDataGridViewTextBoxColumn.Width = 51;
            // 
            // DateofBirth
            // 
            this.DateofBirth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DateofBirth.DataPropertyName = "DateofBirth";
            this.DateofBirth.HeaderText = "Date Of Birth";
            this.DateofBirth.MinimumWidth = 120;
            this.DateofBirth.Name = "DateofBirth";
            this.DateofBirth.Width = 120;
            // 
            // genderDataGridViewTextBoxColumn
            // 
            this.genderDataGridViewTextBoxColumn.DataPropertyName = "Gender";
            this.genderDataGridViewTextBoxColumn.HeaderText = "Gender";
            this.genderDataGridViewTextBoxColumn.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.genderDataGridViewTextBoxColumn.Name = "genderDataGridViewTextBoxColumn";
            this.genderDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.genderDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Party_In_Name_Of
            // 
            this.Party_In_Name_Of.DataPropertyName = "PartyInNameOf";
            this.Party_In_Name_Of.FalseValue = "false";
            this.Party_In_Name_Of.HeaderText = "Party In Name Of";
            this.Party_In_Name_Of.MinimumWidth = 60;
            this.Party_In_Name_Of.Name = "Party_In_Name_Of";
            this.Party_In_Name_Of.TrueValue = "true";
            // 
            // PhoneNumber
            // 
            this.PhoneNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.PhoneNumber.DataPropertyName = "PhoneNumber";
            this.PhoneNumber.HeaderText = "Phone Number";
            this.PhoneNumber.MinimumWidth = 120;
            this.PhoneNumber.Name = "PhoneNumber";
            this.PhoneNumber.Width = 120;
            // 
            // Email
            // 
            this.Email.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Email.DataPropertyName = "Email";
            this.Email.HeaderText = "Email";
            this.Email.MinimumWidth = 100;
            this.Email.Name = "Email";
            // 
            // WaiversSigned
            // 
            this.WaiversSigned.HeaderText = "Waivers Signed?";
            this.WaiversSigned.MinimumWidth = 60;
            this.WaiversSigned.Name = "WaiversSigned";
            this.WaiversSigned.ReadOnly = true;
            // 
            // IsActive
            // 
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.FalseValue = "false";
            this.IsActive.HeaderText = "Active?";
            this.IsActive.MinimumWidth = 60;
            this.IsActive.Name = "IsActive";
            this.IsActive.TrueValue = "true";
            // 
            // specialRequestDataGridViewTextBoxColumn
            // 
            this.specialRequestDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.specialRequestDataGridViewTextBoxColumn.DataPropertyName = "SpecialRequest";
            this.specialRequestDataGridViewTextBoxColumn.HeaderText = "Special Request";
            this.specialRequestDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.specialRequestDataGridViewTextBoxColumn.Name = "specialRequestDataGridViewTextBoxColumn";
            this.specialRequestDataGridViewTextBoxColumn.Width = 101;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            this.remarksDataGridViewTextBoxColumn.Width = 74;
            // 
            // signWaiverEmailLastSentOn
            // 
            this.signWaiverEmailLastSentOn.DataPropertyName = "SignWaiverEmailLastSentOn";
            this.signWaiverEmailLastSentOn.HeaderText = "Waiver Email Last Sent On";
            this.signWaiverEmailLastSentOn.MinimumWidth = 100;
            this.signWaiverEmailLastSentOn.Name = "signWaiverEmailLastSentOn";
            // 
            // signWaiverEmailSentCount
            // 
            this.signWaiverEmailSentCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.signWaiverEmailSentCount.DataPropertyName = "SignWaiverEmailSentCount";
            this.signWaiverEmailSentCount.HeaderText = "Waiver Email Sent Count";
            this.signWaiverEmailSentCount.Name = "signWaiverEmailSentCount";
            this.signWaiverEmailSentCount.Width = 112;
            // 
            // bookingAttendeeDTOBindingSource
            // 
            this.bookingAttendeeDTOBindingSource.DataSource = typeof(Semnox.Parafait.Transaction.BookingAttendeeDTO);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(119, 480);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 34);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Add / Update";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(419, 480);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(91, 34);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(569, 480);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(91, 34);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(269, 480);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(91, 34);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExcel.BackColor = System.Drawing.Color.Transparent;
            this.btnExcel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExcel.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnExcel.FlatAppearance.BorderSize = 0;
            this.btnExcel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExcel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExcel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExcel.ForeColor = System.Drawing.Color.White;
            this.btnExcel.Location = new System.Drawing.Point(855, 480);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(91, 34);
            this.btnExcel.TabIndex = 5;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // bgwSetWaiverSigned
            // 
            this.bgwSetWaiverSigned.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSetWaiverSigned_DoWork);
            this.bgwSetWaiverSigned.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwSetWaiverSigned_ProgressChanged);
            this.bgwSetWaiverSigned.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSetWaiverSigned_RunWorkerCompleted);
            // 
            // btnSendWaiverEmail
            // 
            this.btnSendWaiverEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSendWaiverEmail.BackColor = System.Drawing.Color.Transparent;
            this.btnSendWaiverEmail.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSendWaiverEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSendWaiverEmail.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnSendWaiverEmail.FlatAppearance.BorderSize = 0;
            this.btnSendWaiverEmail.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSendWaiverEmail.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSendWaiverEmail.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSendWaiverEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendWaiverEmail.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendWaiverEmail.ForeColor = System.Drawing.Color.White;
            this.btnSendWaiverEmail.Location = new System.Drawing.Point(714, 480);
            this.btnSendWaiverEmail.Name = "btnSendWaiverEmail";
            this.btnSendWaiverEmail.Size = new System.Drawing.Size(91, 34);
            this.btnSendWaiverEmail.TabIndex = 6;
            this.btnSendWaiverEmail.Text = "Waiver Email";
            this.btnSendWaiverEmail.UseVisualStyleBackColor = false;
            this.btnSendWaiverEmail.Click += new System.EventHandler(this.btnSendWaiverEmail_Click);
            // 
            // horizontalScrollBarAttendee
            // 
            this.horizontalScrollBarAttendee.AutoHide = false;
            this.horizontalScrollBarAttendee.DataGridView = this.dgvAttendees;
            this.horizontalScrollBarAttendee.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarAttendee.LeftButtonBackgroundImage")));
            this.horizontalScrollBarAttendee.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarAttendee.LeftButtonDisabledBackgroundImage")));
            this.horizontalScrollBarAttendee.Location = new System.Drawing.Point(12, 421);
            this.horizontalScrollBarAttendee.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarAttendee.Name = "horizontalScrollBarAttendee";
            this.horizontalScrollBarAttendee.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarAttendee.RightButtonBackgroundImage")));
            this.horizontalScrollBarAttendee.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarAttendee.RightButtonDisabledBackgroundImage")));
            this.horizontalScrollBarAttendee.ScrollableControl = null;
            this.horizontalScrollBarAttendee.ScrollViewer = null;
            this.horizontalScrollBarAttendee.Size = new System.Drawing.Size(1193, 40);
            this.horizontalScrollBarAttendee.TabIndex = 7;
            this.horizontalScrollBarAttendee.LeftButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            this.horizontalScrollBarAttendee.RightButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            // 
            // verticalScrollBarAttendee
            // 
            this.verticalScrollBarAttendee.AutoHide = false;
            this.verticalScrollBarAttendee.DataGridView = this.dgvAttendees;
            this.verticalScrollBarAttendee.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarAttendee.DownButtonBackgroundImage")));
            this.verticalScrollBarAttendee.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarAttendee.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarAttendee.Location = new System.Drawing.Point(1206, 32);
            this.verticalScrollBarAttendee.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarAttendee.Name = "verticalScrollBarAttendee";
            this.verticalScrollBarAttendee.ScrollableControl = null;
            this.verticalScrollBarAttendee.ScrollViewer = null;
            this.verticalScrollBarAttendee.Size = new System.Drawing.Size(40, 387);
            this.verticalScrollBarAttendee.TabIndex = 8;
            this.verticalScrollBarAttendee.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarAttendee.UpButtonBackgroundImage")));
            this.verticalScrollBarAttendee.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarAttendee.UpButtonDisabledBackgroundImage")));
            this.verticalScrollBarAttendee.UpButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            this.verticalScrollBarAttendee.DownButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            // 
            // lblWaiverStatusMessageOne
            // 
            this.lblWaiverStatusMessageOne.BackColor = System.Drawing.Color.Transparent;
            this.lblWaiverStatusMessageOne.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaiverStatusMessageOne.Location = new System.Drawing.Point(12, 4);
            this.lblWaiverStatusMessageOne.Name = "lblWaiverStatusMessageOne";
            this.lblWaiverStatusMessageOne.Size = new System.Drawing.Size(200, 25);
            this.lblWaiverStatusMessageOne.TabIndex = 9;
            this.lblWaiverStatusMessageOne.Text = "Reservation Guest Count:";
            this.lblWaiverStatusMessageOne.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWaiverStatusMessageTwo
            // 
            this.lblWaiverStatusMessageTwo.BackColor = System.Drawing.Color.Transparent;
            this.lblWaiverStatusMessageTwo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaiverStatusMessageTwo.Location = new System.Drawing.Point(295, 4);
            this.lblWaiverStatusMessageTwo.Name = "lblWaiverStatusMessageTwo";
            this.lblWaiverStatusMessageTwo.Size = new System.Drawing.Size(221, 25);
            this.lblWaiverStatusMessageTwo.TabIndex = 10;
            this.lblWaiverStatusMessageTwo.Text = "Waiver Signed Attendee Count:";
            this.lblWaiverStatusMessageTwo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWaiverStatusMessageOneVal
            // 
            this.lblWaiverStatusMessageOneVal.BackColor = System.Drawing.Color.Transparent;
            this.lblWaiverStatusMessageOneVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWaiverStatusMessageOneVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaiverStatusMessageOneVal.Location = new System.Drawing.Point(213, 4);
            this.lblWaiverStatusMessageOneVal.Name = "lblWaiverStatusMessageOneVal";
            this.lblWaiverStatusMessageOneVal.Size = new System.Drawing.Size(60, 25);
            this.lblWaiverStatusMessageOneVal.TabIndex = 11;
            this.lblWaiverStatusMessageOneVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWaiverStatusMessageTwoVal
            // 
            this.lblWaiverStatusMessageTwoVal.BackColor = System.Drawing.Color.Transparent;
            this.lblWaiverStatusMessageTwoVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWaiverStatusMessageTwoVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaiverStatusMessageTwoVal.Location = new System.Drawing.Point(517, 4);
            this.lblWaiverStatusMessageTwoVal.Name = "lblWaiverStatusMessageTwoVal";
            this.lblWaiverStatusMessageTwoVal.Size = new System.Drawing.Size(60, 25);
            this.lblWaiverStatusMessageTwoVal.TabIndex = 12;
            this.lblWaiverStatusMessageTwoVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmBookingAttendees
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1260, 523);
            this.Controls.Add(this.lblWaiverStatusMessageTwoVal);
            this.Controls.Add(this.lblWaiverStatusMessageOneVal);
            this.Controls.Add(this.lblWaiverStatusMessageTwo);
            this.Controls.Add(this.lblWaiverStatusMessageOne);
            this.Controls.Add(this.verticalScrollBarAttendee);
            this.Controls.Add(this.horizontalScrollBarAttendee);
            this.Controls.Add(this.btnSendWaiverEmail);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvAttendees);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmBookingAttendees";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Attendees";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBookingAttendees_FormClosing);
            this.Load += new System.EventHandler(this.frmBookingAttendees_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendees)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookingAttendeeDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAttendees;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.BindingSource bookingAttendeeDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrxId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateofBirth;  
        private System.ComponentModel.BackgroundWorker bgwSetWaiverSigned;
        private System.Windows.Forms.Button btnSendWaiverEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn signWaiverEmailSentCount;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectRecord;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn; 
        private System.Windows.Forms.DataGridViewTextBoxColumn bookingIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ageDataGridViewTextBoxColumn; 
        private System.Windows.Forms.DataGridViewComboBoxColumn genderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Party_In_Name_Of;
        private System.Windows.Forms.DataGridViewTextBoxColumn PhoneNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewCheckBoxColumn WaiversSigned;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn specialRequestDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signWaiverEmailLastSentOn;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarAttendee;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarAttendee;
        private System.Windows.Forms.Label lblWaiverStatusMessageOne;
        private System.Windows.Forms.Label lblWaiverStatusMessageTwo;
        private System.Windows.Forms.Label lblWaiverStatusMessageOneVal;
        private System.Windows.Forms.Label lblWaiverStatusMessageTwoVal;
    }
}