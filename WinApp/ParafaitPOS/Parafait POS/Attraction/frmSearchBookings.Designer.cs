namespace Parafait_POS.Attraction
{
    partial class frmSearchBookings
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCard = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtTrxId = new System.Windows.Forms.TextBox();
            this.cmbFacilityMap = new System.Windows.Forms.ComboBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCustomerLookup = new System.Windows.Forms.Button();
            this.txtMessageLine = new System.Windows.Forms.TextBox();
            this.cmbFromHour = new System.Windows.Forms.ComboBox();
            this.cmbFromMin = new System.Windows.Forms.ComboBox();
            this.cmbFromAM = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbToAM = new System.Windows.Forms.ComboBox();
            this.cmbToMin = new System.Windows.Forms.ComboBox();
            this.cmbToHour = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnKeyBoard = new System.Windows.Forms.Button();
            this.cmbFacilityMapSchedules = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(86, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Card:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(53, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "First Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(83, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Trx Id:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(47, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "From Time:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(67, 205);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "To Time:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(42, 247);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "Facility Map:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCard
            // 
            this.txtCard.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCard.Location = new System.Drawing.Point(141, 33);
            this.txtCard.Name = "txtCard";
            this.txtCard.Size = new System.Drawing.Size(199, 25);
            this.txtCard.TabIndex = 0;
            this.txtCard.Leave += new System.EventHandler(this.txtCard_Leave);
            // 
            // txtFirstName
            // 
            this.txtFirstName.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.Location = new System.Drawing.Point(140, 75);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(200, 25);
            this.txtFirstName.TabIndex = 1;
            this.txtFirstName.Leave += new System.EventHandler(this.txtFirstName_Leave);
            // 
            // txtTrxId
            // 
            this.txtTrxId.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTrxId.Location = new System.Drawing.Point(140, 117);
            this.txtTrxId.Name = "txtTrxId";
            this.txtTrxId.Size = new System.Drawing.Size(200, 25);
            this.txtTrxId.TabIndex = 3;
            this.txtTrxId.Enter += new System.EventHandler(this.txt_Enter);
            // 
            // cmbFacilityMap
            // 
            this.cmbFacilityMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFacilityMap.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFacilityMap.FormattingEnabled = true;
            this.cmbFacilityMap.ItemHeight = 17;
            this.cmbFacilityMap.Location = new System.Drawing.Point(140, 245);
            this.cmbFacilityMap.Name = "cmbFacilityMap";
            this.cmbFacilityMap.Size = new System.Drawing.Size(200, 25);
            this.cmbFacilityMap.TabIndex = 16;
            this.cmbFacilityMap.SelectedIndexChanged += new System.EventHandler(this.cmbFacilityMap_SelectedIndexChanged);
            // 
            // dtpToDate
            // 
            this.dtpToDate.CalendarFont = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDate.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDate.Location = new System.Drawing.Point(140, 203);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(200, 25);
            this.dtpToDate.TabIndex = 10;
            this.dtpToDate.Value = new System.DateTime(2019, 8, 14, 13, 29, 51, 0);
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CalendarFont = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFromDate.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFromDate.Location = new System.Drawing.Point(141, 159);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(200, 25);
            this.dtpFromDate.TabIndex = 4;
            this.dtpFromDate.Value = new System.DateTime(2019, 8, 14, 13, 29, 43, 0);
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            // 
            // btnOk
            // 
            this.btnOk.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(176, 298);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 36);
            this.btnOk.TabIndex = 17;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.btnOk.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnOk_MouseDown);
            this.btnOk.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnOk_MouseUp);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(317, 298);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 36);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCancel_MouseDown);
            this.btnCancel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCancel_MouseUp);
            // 
            // btnCustomerLookup
            // 
            this.btnCustomerLookup.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCustomerLookup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCustomerLookup.FlatAppearance.BorderSize = 0;
            this.btnCustomerLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerLookup.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerLookup.ForeColor = System.Drawing.Color.White;
            this.btnCustomerLookup.Location = new System.Drawing.Point(363, 75);
            this.btnCustomerLookup.Name = "btnCustomerLookup";
            this.btnCustomerLookup.Size = new System.Drawing.Size(104, 36);
            this.btnCustomerLookup.TabIndex = 2;
            this.btnCustomerLookup.Text = "Lookup";
            this.btnCustomerLookup.UseVisualStyleBackColor = true;
            this.btnCustomerLookup.Click += new System.EventHandler(this.btnCustomerLookup_Click);
            this.btnCustomerLookup.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCustomerLookup_MouseDown);
            this.btnCustomerLookup.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCustomerLookup_MouseUp);
            // 
            // txtMessageLine
            // 
            this.txtMessageLine.BackColor = System.Drawing.Color.Ivory;
            this.txtMessageLine.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessageLine.Location = new System.Drawing.Point(0, 342);
            this.txtMessageLine.Name = "txtMessageLine";
            this.txtMessageLine.Size = new System.Drawing.Size(583, 25);
            this.txtMessageLine.TabIndex = 17;
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
            this.cmbFromHour.Location = new System.Drawing.Point(349, 159);
            this.cmbFromHour.Name = "cmbFromHour";
            this.cmbFromHour.Size = new System.Drawing.Size(50, 25);
            this.cmbFromHour.TabIndex = 5;
            this.cmbFromHour.SelectedIndexChanged += new System.EventHandler(this.cmbFromHour_SelectedIndexChanged);
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
            this.cmbFromMin.Location = new System.Drawing.Point(406, 159);
            this.cmbFromMin.Name = "cmbFromMin";
            this.cmbFromMin.Size = new System.Drawing.Size(50, 25);
            this.cmbFromMin.TabIndex = 6;
            this.cmbFromMin.SelectedIndexChanged += new System.EventHandler(this.cmbFromMin_SelectedIndexChanged);
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
            this.cmbFromAM.Location = new System.Drawing.Point(461, 159);
            this.cmbFromAM.Name = "cmbFromAM";
            this.cmbFromAM.Size = new System.Drawing.Size(50, 25);
            this.cmbFromAM.TabIndex = 7;
            this.cmbFromAM.SelectedIndexChanged += new System.EventHandler(this.cmbFromAM_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(397, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 18);
            this.label7.TabIndex = 27;
            this.label7.Text = ":";
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
            this.cmbToAM.Location = new System.Drawing.Point(461, 203);
            this.cmbToAM.Name = "cmbToAM";
            this.cmbToAM.Size = new System.Drawing.Size(50, 25);
            this.cmbToAM.TabIndex = 13;
            this.cmbToAM.SelectedIndexChanged += new System.EventHandler(this.cmbToAM_SelectedIndexChanged);
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
            this.cmbToMin.Location = new System.Drawing.Point(406, 203);
            this.cmbToMin.Name = "cmbToMin";
            this.cmbToMin.Size = new System.Drawing.Size(50, 25);
            this.cmbToMin.TabIndex = 12;
            this.cmbToMin.SelectedIndexChanged += new System.EventHandler(this.cmbToMin_SelectedIndexChanged);
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
            this.cmbToHour.Location = new System.Drawing.Point(349, 203);
            this.cmbToHour.Name = "cmbToHour";
            this.cmbToHour.Size = new System.Drawing.Size(50, 25);
            this.cmbToHour.TabIndex = 11;
            this.cmbToHour.SelectedIndexChanged += new System.EventHandler(this.cmbToHour_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(397, 206);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(12, 18);
            this.label8.TabIndex = 31;
            this.label8.Text = ":";
            // 
            // btnKeyBoard
            // 
            this.btnKeyBoard.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnKeyBoard.FlatAppearance.BorderSize = 0;
            this.btnKeyBoard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyBoard.Location = new System.Drawing.Point(488, 300);
            this.btnKeyBoard.Name = "btnKeyBoard";
            this.btnKeyBoard.Size = new System.Drawing.Size(32, 32);
            this.btnKeyBoard.TabIndex = 32;
            this.btnKeyBoard.UseVisualStyleBackColor = true;
            this.btnKeyBoard.Click += new System.EventHandler(this.btnKeyBoard_Click);
            // 
            // cmbFacilityMapSchedules
            // 
            this.cmbFacilityMapSchedules.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFacilityMapSchedules.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFacilityMapSchedules.FormattingEnabled = true;
            this.cmbFacilityMapSchedules.ItemHeight = 17;
            this.cmbFacilityMapSchedules.Location = new System.Drawing.Point(349, 245);
            this.cmbFacilityMapSchedules.Name = "cmbFacilityMapSchedules";
            this.cmbFacilityMapSchedules.Size = new System.Drawing.Size(162, 25);
            this.cmbFacilityMapSchedules.TabIndex = 33;
            // 
            // frmSearchBookings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(583, 367);
            this.Controls.Add(this.cmbFacilityMapSchedules);
            this.Controls.Add(this.btnKeyBoard);
            this.Controls.Add(this.cmbToAM);
            this.Controls.Add(this.cmbToMin);
            this.Controls.Add(this.cmbToHour);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbFromAM);
            this.Controls.Add(this.cmbFromMin);
            this.Controls.Add(this.cmbFromHour);
            this.Controls.Add(this.txtMessageLine);
            this.Controls.Add(this.btnCustomerLookup);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.cmbFacilityMap);
            this.Controls.Add(this.txtTrxId);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.txtCard);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSearchBookings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search Attraction Bookings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSearchBookings_FormClosed);
            this.Load += new System.EventHandler(this.frmSearchBookings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCard;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtTrxId;
        private System.Windows.Forms.ComboBox cmbFacilityMap;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCustomerLookup;
        private System.Windows.Forms.TextBox txtMessageLine;
        private System.Windows.Forms.ComboBox cmbFromHour;
        private System.Windows.Forms.ComboBox cmbFromMin;
        private System.Windows.Forms.ComboBox cmbFromAM;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbToAM;
        private System.Windows.Forms.ComboBox cmbToMin;
        private System.Windows.Forms.ComboBox cmbToHour;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnKeyBoard;
        private System.Windows.Forms.ComboBox cmbFacilityMapSchedules;
    }
}