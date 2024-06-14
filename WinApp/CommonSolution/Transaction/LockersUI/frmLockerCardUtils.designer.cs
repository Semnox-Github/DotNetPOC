namespace Semnox.Parafait.Transaction
{
    partial class frmLockerCardUtils
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
            this.btnClear = new System.Windows.Forms.Button();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSystemCard = new System.Windows.Forms.Button();
            this.btnCreateSettingCard = new System.Windows.Forms.Button();
            this.btnMasterCard = new System.Windows.Forms.Button();
            this.btnClockCard = new System.Windows.Forms.Button();
            this.btnParameterCard = new System.Windows.Forms.Button();
            this.btnBlockCard = new System.Windows.Forms.Button();
            this.btnGuestCard = new System.Windows.Forms.Button();
            this.dtPickerFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtPickerToDate = new System.Windows.Forms.DateTimePicker();
            this.lockerModeFixedMode = new System.Windows.Forms.RadioButton();
            this.lockerModeFreeMode = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblLockerNo = new System.Windows.Forms.Label();
            this.txtLockerNo = new System.Windows.Forms.TextBox();
            this.groupBoxLockerMode = new System.Windows.Forms.GroupBox();
            this.chbEnableInAllZone = new System.Windows.Forms.CheckBox();
            this.cmbZone = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbLockers = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnEraseCard = new System.Windows.Forms.Button();
            this.lnkResetMifareReader = new System.Windows.Forms.LinkLabel();
            this.txtCardInfo = new System.Windows.Forms.TextBox();
            this.txtLockerName = new System.Windows.Forms.TextBox();
            this.txtLockerId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLockerNumberDisp = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPanelName = new System.Windows.Forms.TextBox();
            this.lblLockerMake = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBoxLockerMode.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.SteelBlue;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(485, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 30);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtCardNumber.Location = new System.Drawing.Point(188, 12);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.ReadOnly = true;
            this.txtCardNumber.Size = new System.Drawing.Size(77, 20);
            this.txtCardNumber.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Card#:";
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 494);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(675, 22);
            this.txtMessage.TabIndex = 4;
            // 
            // btnSystemCard
            // 
            this.btnSystemCard.BackColor = System.Drawing.Color.Wheat;
            this.btnSystemCard.FlatAppearance.BorderSize = 0;
            this.btnSystemCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSystemCard.Location = new System.Drawing.Point(37, 86);
            this.btnSystemCard.Name = "btnSystemCard";
            this.btnSystemCard.Size = new System.Drawing.Size(131, 46);
            this.btnSystemCard.TabIndex = 19;
            this.btnSystemCard.Text = "Create System Card";
            this.btnSystemCard.UseVisualStyleBackColor = false;
            this.btnSystemCard.Click += new System.EventHandler(this.btnSystemCard_Click);
            // 
            // btnCreateSettingCard
            // 
            this.btnCreateSettingCard.BackColor = System.Drawing.Color.Wheat;
            this.btnCreateSettingCard.FlatAppearance.BorderSize = 0;
            this.btnCreateSettingCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateSettingCard.Location = new System.Drawing.Point(203, 86);
            this.btnCreateSettingCard.Name = "btnCreateSettingCard";
            this.btnCreateSettingCard.Size = new System.Drawing.Size(131, 46);
            this.btnCreateSettingCard.TabIndex = 20;
            this.btnCreateSettingCard.Text = "Create Setting Card";
            this.btnCreateSettingCard.UseVisualStyleBackColor = false;
            this.btnCreateSettingCard.Click += new System.EventHandler(this.btnCreateSettingCard_Click);
            // 
            // btnMasterCard
            // 
            this.btnMasterCard.BackColor = System.Drawing.Color.Wheat;
            this.btnMasterCard.FlatAppearance.BorderSize = 0;
            this.btnMasterCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMasterCard.Location = new System.Drawing.Point(37, 144);
            this.btnMasterCard.Name = "btnMasterCard";
            this.btnMasterCard.Size = new System.Drawing.Size(131, 46);
            this.btnMasterCard.TabIndex = 21;
            this.btnMasterCard.Text = "Create Master Card";
            this.btnMasterCard.UseVisualStyleBackColor = false;
            this.btnMasterCard.Click += new System.EventHandler(this.btnMasterCard_Click);
            // 
            // btnClockCard
            // 
            this.btnClockCard.BackColor = System.Drawing.Color.Wheat;
            this.btnClockCard.FlatAppearance.BorderSize = 0;
            this.btnClockCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClockCard.Location = new System.Drawing.Point(203, 144);
            this.btnClockCard.Name = "btnClockCard";
            this.btnClockCard.Size = new System.Drawing.Size(131, 46);
            this.btnClockCard.TabIndex = 22;
            this.btnClockCard.Text = "Clock Card";
            this.btnClockCard.UseVisualStyleBackColor = false;
            this.btnClockCard.Click += new System.EventHandler(this.btnClockCard_Click);
            // 
            // btnParameterCard
            // 
            this.btnParameterCard.BackColor = System.Drawing.Color.Goldenrod;
            this.btnParameterCard.FlatAppearance.BorderSize = 0;
            this.btnParameterCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParameterCard.ForeColor = System.Drawing.Color.White;
            this.btnParameterCard.Location = new System.Drawing.Point(34, 75);
            this.btnParameterCard.Name = "btnParameterCard";
            this.btnParameterCard.Size = new System.Drawing.Size(131, 52);
            this.btnParameterCard.TabIndex = 23;
            this.btnParameterCard.Text = "5. Parameter Card";
            this.btnParameterCard.UseVisualStyleBackColor = false;
            this.btnParameterCard.Click += new System.EventHandler(this.btnParameterCard_Click);
            // 
            // btnBlockCard
            // 
            this.btnBlockCard.BackColor = System.Drawing.Color.DarkOrange;
            this.btnBlockCard.FlatAppearance.BorderSize = 0;
            this.btnBlockCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlockCard.ForeColor = System.Drawing.Color.White;
            this.btnBlockCard.Location = new System.Drawing.Point(361, 75);
            this.btnBlockCard.Name = "btnBlockCard";
            this.btnBlockCard.Size = new System.Drawing.Size(131, 52);
            this.btnBlockCard.TabIndex = 24;
            this.btnBlockCard.Text = "Inhibit Card";
            this.btnBlockCard.UseVisualStyleBackColor = false;
            this.btnBlockCard.Click += new System.EventHandler(this.btnBlockCard_Click);
            // 
            // btnGuestCard
            // 
            this.btnGuestCard.BackColor = System.Drawing.Color.SeaGreen;
            this.btnGuestCard.FlatAppearance.BorderSize = 0;
            this.btnGuestCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuestCard.ForeColor = System.Drawing.Color.White;
            this.btnGuestCard.Location = new System.Drawing.Point(200, 75);
            this.btnGuestCard.Name = "btnGuestCard";
            this.btnGuestCard.Size = new System.Drawing.Size(131, 52);
            this.btnGuestCard.TabIndex = 25;
            this.btnGuestCard.Text = "Guest Card";
            this.btnGuestCard.UseVisualStyleBackColor = false;
            this.btnGuestCard.Click += new System.EventHandler(this.btnGuestCard_Click);
            // 
            // dtPickerFromDate
            // 
            this.dtPickerFromDate.CustomFormat = "MMM dd, yyyy - HH:mm:ss";
            this.dtPickerFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerFromDate.Location = new System.Drawing.Point(124, 21);
            this.dtPickerFromDate.Name = "dtPickerFromDate";
            this.dtPickerFromDate.Size = new System.Drawing.Size(177, 20);
            this.dtPickerFromDate.TabIndex = 26;
            // 
            // dtPickerToDate
            // 
            this.dtPickerToDate.CustomFormat = "MMM dd, yyyy - HH:mm:ss";
            this.dtPickerToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerToDate.Location = new System.Drawing.Point(124, 50);
            this.dtPickerToDate.Name = "dtPickerToDate";
            this.dtPickerToDate.Size = new System.Drawing.Size(177, 20);
            this.dtPickerToDate.TabIndex = 27;
            // 
            // lockerModeFixedMode
            // 
            this.lockerModeFixedMode.AutoSize = true;
            this.lockerModeFixedMode.Location = new System.Drawing.Point(34, 19);
            this.lockerModeFixedMode.Name = "lockerModeFixedMode";
            this.lockerModeFixedMode.Size = new System.Drawing.Size(80, 17);
            this.lockerModeFixedMode.TabIndex = 28;
            this.lockerModeFixedMode.TabStop = true;
            this.lockerModeFixedMode.Text = "Fixed Mode";
            this.lockerModeFixedMode.UseVisualStyleBackColor = true;
            // 
            // lockerModeFreeMode
            // 
            this.lockerModeFreeMode.AutoSize = true;
            this.lockerModeFreeMode.Location = new System.Drawing.Point(124, 19);
            this.lockerModeFreeMode.Name = "lockerModeFreeMode";
            this.lockerModeFreeMode.Size = new System.Drawing.Size(76, 17);
            this.lockerModeFreeMode.TabIndex = 29;
            this.lockerModeFreeMode.TabStop = true;
            this.lockerModeFreeMode.Text = "Free Mode";
            this.lockerModeFreeMode.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(65, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "From Time:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(74, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "To Time:";
            // 
            // lblLockerNo
            // 
            this.lblLockerNo.AutoSize = true;
            this.lblLockerNo.Location = new System.Drawing.Point(415, 51);
            this.lblLockerNo.Name = "lblLockerNo";
            this.lblLockerNo.Size = new System.Drawing.Size(24, 13);
            this.lblLockerNo.TabIndex = 33;
            this.lblLockerNo.Text = "No:";
            // 
            // txtLockerNo
            // 
            this.txtLockerNo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtLockerNo.Location = new System.Drawing.Point(445, 47);
            this.txtLockerNo.MaxLength = 6;
            this.txtLockerNo.Name = "txtLockerNo";
            this.txtLockerNo.Size = new System.Drawing.Size(46, 20);
            this.txtLockerNo.TabIndex = 32;
            this.txtLockerNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBoxLockerMode
            // 
            this.groupBoxLockerMode.Controls.Add(this.chbEnableInAllZone);
            this.groupBoxLockerMode.Controls.Add(this.cmbZone);
            this.groupBoxLockerMode.Controls.Add(this.label9);
            this.groupBoxLockerMode.Controls.Add(this.cmbLockers);
            this.groupBoxLockerMode.Controls.Add(this.label6);
            this.groupBoxLockerMode.Controls.Add(this.lockerModeFixedMode);
            this.groupBoxLockerMode.Controls.Add(this.lockerModeFreeMode);
            this.groupBoxLockerMode.Controls.Add(this.txtLockerNo);
            this.groupBoxLockerMode.Controls.Add(this.lblLockerNo);
            this.groupBoxLockerMode.Controls.Add(this.btnBlockCard);
            this.groupBoxLockerMode.Controls.Add(this.btnParameterCard);
            this.groupBoxLockerMode.Controls.Add(this.btnGuestCard);
            this.groupBoxLockerMode.Location = new System.Drawing.Point(64, 330);
            this.groupBoxLockerMode.Name = "groupBoxLockerMode";
            this.groupBoxLockerMode.Size = new System.Drawing.Size(524, 140);
            this.groupBoxLockerMode.TabIndex = 34;
            this.groupBoxLockerMode.TabStop = false;
            this.groupBoxLockerMode.Text = "Locker Mode";
            // 
            // chbEnableInAllZone
            // 
            this.chbEnableInAllZone.AutoSize = true;
            this.chbEnableInAllZone.Location = new System.Drawing.Point(212, 19);
            this.chbEnableInAllZone.Name = "chbEnableInAllZone";
            this.chbEnableInAllZone.Size = new System.Drawing.Size(143, 17);
            this.chbEnableInAllZone.TabIndex = 39;
            this.chbEnableInAllZone.Text = "Enable Card In All Zones";
            this.chbEnableInAllZone.UseVisualStyleBackColor = true;
            this.chbEnableInAllZone.Visible = false;
            // 
            // cmbZone
            // 
            this.cmbZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZone.FormattingEnabled = true;
            this.cmbZone.Location = new System.Drawing.Point(111, 47);
            this.cmbZone.Name = "cmbZone";
            this.cmbZone.Size = new System.Drawing.Size(99, 21);
            this.cmbZone.TabIndex = 38;
            this.cmbZone.SelectedIndexChanged += new System.EventHandler(this.cmbZone_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(34, 49);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Locker Zone:";
            // 
            // cmbLockers
            // 
            this.cmbLockers.FormattingEnabled = true;
            this.cmbLockers.Location = new System.Drawing.Point(303, 47);
            this.cmbLockers.Name = "cmbLockers";
            this.cmbLockers.Size = new System.Drawing.Size(99, 21);
            this.cmbLockers.TabIndex = 36;
            this.cmbLockers.SelectedIndexChanged += new System.EventHandler(this.cmbLockers_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(226, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Locker Name:";
            // 
            // btnEraseCard
            // 
            this.btnEraseCard.BackColor = System.Drawing.Color.Red;
            this.btnEraseCard.FlatAppearance.BorderSize = 0;
            this.btnEraseCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEraseCard.Location = new System.Drawing.Point(364, 144);
            this.btnEraseCard.Name = "btnEraseCard";
            this.btnEraseCard.Size = new System.Drawing.Size(131, 46);
            this.btnEraseCard.TabIndex = 35;
            this.btnEraseCard.Text = "Erase Card";
            this.btnEraseCard.UseVisualStyleBackColor = false;
            this.btnEraseCard.Click += new System.EventHandler(this.btnEraseCard_Click);
            // 
            // lnkResetMifareReader
            // 
            this.lnkResetMifareReader.AutoSize = true;
            this.lnkResetMifareReader.Location = new System.Drawing.Point(61, 473);
            this.lnkResetMifareReader.Name = "lnkResetMifareReader";
            this.lnkResetMifareReader.Size = new System.Drawing.Size(108, 13);
            this.lnkResetMifareReader.TabIndex = 36;
            this.lnkResetMifareReader.TabStop = true;
            this.lnkResetMifareReader.Text = "Reset MiFare Reader";
            this.lnkResetMifareReader.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkResetMifareReader_LinkClicked);
            // 
            // txtCardInfo
            // 
            this.txtCardInfo.BackColor = System.Drawing.Color.White;
            this.txtCardInfo.Location = new System.Drawing.Point(276, 12);
            this.txtCardInfo.Multiline = true;
            this.txtCardInfo.Name = "txtCardInfo";
            this.txtCardInfo.ReadOnly = true;
            this.txtCardInfo.Size = new System.Drawing.Size(203, 66);
            this.txtCardInfo.TabIndex = 37;
            // 
            // txtLockerName
            // 
            this.txtLockerName.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtLockerName.Location = new System.Drawing.Point(188, 35);
            this.txtLockerName.Name = "txtLockerName";
            this.txtLockerName.ReadOnly = true;
            this.txtLockerName.Size = new System.Drawing.Size(77, 20);
            this.txtLockerName.TabIndex = 38;
            // 
            // txtLockerId
            // 
            this.txtLockerId.Enabled = false;
            this.txtLockerId.Location = new System.Drawing.Point(276, 81);
            this.txtLockerId.Name = "txtLockerId";
            this.txtLockerId.ReadOnly = true;
            this.txtLockerId.Size = new System.Drawing.Size(52, 20);
            this.txtLockerId.TabIndex = 39;
            this.txtLockerId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Locker Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(253, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 41;
            this.label3.Text = "ID:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSystemCard);
            this.groupBox1.Controls.Add(this.btnCreateSettingCard);
            this.groupBox1.Controls.Add(this.btnMasterCard);
            this.groupBox1.Controls.Add(this.btnClockCard);
            this.groupBox1.Controls.Add(this.dtPickerFromDate);
            this.groupBox1.Controls.Add(this.dtPickerToDate);
            this.groupBox1.Controls.Add(this.btnEraseCard);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(64, 117);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(524, 201);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Setup";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(139, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 44;
            this.label4.Text = "Number:";
            // 
            // txtLockerNumberDisp
            // 
            this.txtLockerNumberDisp.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtLockerNumberDisp.Location = new System.Drawing.Point(188, 81);
            this.txtLockerNumberDisp.Name = "txtLockerNumberDisp";
            this.txtLockerNumberDisp.ReadOnly = true;
            this.txtLockerNumberDisp.Size = new System.Drawing.Size(52, 20);
            this.txtLockerNumberDisp.TabIndex = 43;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(150, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 46;
            this.label5.Text = "Panel:";
            // 
            // txtPanelName
            // 
            this.txtPanelName.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtPanelName.Location = new System.Drawing.Point(188, 58);
            this.txtPanelName.Name = "txtPanelName";
            this.txtPanelName.ReadOnly = true;
            this.txtPanelName.Size = new System.Drawing.Size(77, 20);
            this.txtPanelName.TabIndex = 45;
            // 
            // lblLockerMake
            // 
            this.lblLockerMake.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLockerMake.Location = new System.Drawing.Point(546, 478);
            this.lblLockerMake.Name = "lblLockerMake";
            this.lblLockerMake.Size = new System.Drawing.Size(127, 13);
            this.lblLockerMake.TabIndex = 47;
            this.lblLockerMake.Text = "INNOVATE";
            this.lblLockerMake.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.SteelBlue;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(485, 48);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 48;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmLockerCardUtils
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 516);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblLockerMake);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPanelName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLockerNumberDisp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLockerId);
            this.Controls.Add(this.txtLockerName);
            this.Controls.Add(this.txtCardInfo);
            this.Controls.Add(this.lnkResetMifareReader);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.groupBoxLockerMode);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmLockerCardUtils";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MiFare Locker - Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLockerCardUtils_FormClosing);
            this.Load += new System.EventHandler(this.frmMifareUtils_Load);
            this.groupBoxLockerMode.ResumeLayout(false);
            this.groupBoxLockerMode.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSystemCard;
        private System.Windows.Forms.Button btnCreateSettingCard;
        private System.Windows.Forms.Button btnMasterCard;
        private System.Windows.Forms.Button btnClockCard;
        private System.Windows.Forms.Button btnParameterCard;
        private System.Windows.Forms.Button btnBlockCard;
        private System.Windows.Forms.Button btnGuestCard;
        private System.Windows.Forms.DateTimePicker dtPickerFromDate;
        private System.Windows.Forms.DateTimePicker dtPickerToDate;
        private System.Windows.Forms.RadioButton lockerModeFixedMode;
        private System.Windows.Forms.RadioButton lockerModeFreeMode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblLockerNo;
        private System.Windows.Forms.TextBox txtLockerNo;
        private System.Windows.Forms.GroupBox groupBoxLockerMode;
        private System.Windows.Forms.Button btnEraseCard;
        private System.Windows.Forms.LinkLabel lnkResetMifareReader;
        private System.Windows.Forms.TextBox txtCardInfo;
        private System.Windows.Forms.TextBox txtLockerName;
        private System.Windows.Forms.TextBox txtLockerId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLockerNumberDisp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPanelName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbLockers;
        private System.Windows.Forms.Label lblLockerMake;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cmbZone;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chbEnableInAllZone;
    }
}

