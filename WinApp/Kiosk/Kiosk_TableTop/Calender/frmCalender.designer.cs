namespace Parafait_Kiosk
{
    partial class frmCalender
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flpDatePicker = new System.Windows.Forms.FlowLayoutPanel();
            this.flpDay = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDay = new System.Windows.Forms.Label();
            this.flpMonth = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMonth = new System.Windows.Forms.Label();
            this.flpYear = new System.Windows.Forms.FlowLayoutPanel();
            this.lblYear = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flpUsrCtrlDay = new System.Windows.Forms.FlowLayoutPanel();
            this.flpUsrCtrlMonth = new System.Windows.Forms.FlowLayoutPanel();
            this.flpUsrCtrlYear = new System.Windows.Forms.FlowLayoutPanel();
            this.flpTimePicker = new System.Windows.Forms.FlowLayoutPanel();
            this.flpHour = new System.Windows.Forms.FlowLayoutPanel();
            this.lblHour = new System.Windows.Forms.Label();
            this.lblDelimeter = new System.Windows.Forms.Label();
            this.flpMinute = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMinute = new System.Windows.Forms.Label();
            this.flpAMPM = new System.Windows.Forms.FlowLayoutPanel();
            this.lblAMPM = new System.Windows.Forms.Label();
            this.flpUsrCtrlHour = new System.Windows.Forms.FlowLayoutPanel();
            this.flpUsrCtrlMinute = new System.Windows.Forms.FlowLayoutPanel();
            this.flpUsrCtrlAMPM = new System.Windows.Forms.FlowLayoutPanel();
            this.flpDatePicker.SuspendLayout();
            this.flpDay.SuspendLayout();
            this.flpMonth.SuspendLayout();
            this.flpYear.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.flpTimePicker.SuspendLayout();
            this.flpHour.SuspendLayout();
            this.flpMinute.SuspendLayout();
            this.flpAMPM.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.Visible = false;
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(87, 449);
            this.btnPrev.Size = new System.Drawing.Size(365, 135);
            this.btnPrev.Text = "Cancel";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(31, 226);
            this.btnCancel.Size = new System.Drawing.Size(43, 21);
            // 
            // btnCart
            // 
            this.btnCart.Location = new System.Drawing.Point(798, 28);
            // 
            // flpDatePicker
            // 
            this.flpDatePicker.AutoSize = true;
            this.flpDatePicker.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpDatePicker.BackColor = System.Drawing.Color.Transparent;
            this.flpDatePicker.Controls.Add(this.flpDay);
            this.flpDatePicker.Controls.Add(this.flpMonth);
            this.flpDatePicker.Controls.Add(this.flpYear);
            this.flpDatePicker.Location = new System.Drawing.Point(50, 54);
            this.flpDatePicker.Name = "flpDatePicker";
            this.flpDatePicker.Size = new System.Drawing.Size(429, 86);
            this.flpDatePicker.TabIndex = 20016;
            // 
            // flpDay
            // 
            this.flpDay.AutoSize = true;
            this.flpDay.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpDay.BackColor = System.Drawing.Color.Transparent;
            this.flpDay.Controls.Add(this.lblDay);
            this.flpDay.Location = new System.Drawing.Point(3, 3);
            this.flpDay.Name = "flpDay";
            this.flpDay.Size = new System.Drawing.Size(110, 80);
            this.flpDay.TabIndex = 20021;
            // 
            // lblDay
            // 
            this.lblDay.BackColor = System.Drawing.Color.White;
            this.lblDay.CausesValidation = false;
            this.lblDay.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDay.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblDay.Location = new System.Drawing.Point(0, 0);
            this.lblDay.Margin = new System.Windows.Forms.Padding(0);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(110, 80);
            this.lblDay.TabIndex = 0;
            this.lblDay.Text = "01";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDay.Click += new System.EventHandler(this.lblDay_Click);
            // 
            // flpMonth
            // 
            this.flpMonth.AutoSize = true;
            this.flpMonth.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpMonth.BackColor = System.Drawing.Color.Transparent;
            this.flpMonth.Controls.Add(this.lblMonth);
            this.flpMonth.Location = new System.Drawing.Point(119, 3);
            this.flpMonth.Name = "flpMonth";
            this.flpMonth.Size = new System.Drawing.Size(140, 80);
            this.flpMonth.TabIndex = 20022;
            // 
            // lblMonth
            // 
            this.lblMonth.BackColor = System.Drawing.Color.White;
            this.lblMonth.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonth.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblMonth.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMonth.Location = new System.Drawing.Point(0, 0);
            this.lblMonth.Margin = new System.Windows.Forms.Padding(0);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(140, 80);
            this.lblMonth.TabIndex = 1;
            this.lblMonth.Text = "Jan";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMonth.Click += new System.EventHandler(this.lblMonth_Click);
            // 
            // flpYear
            // 
            this.flpYear.AutoSize = true;
            this.flpYear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpYear.BackColor = System.Drawing.Color.Transparent;
            this.flpYear.Controls.Add(this.lblYear);
            this.flpYear.Location = new System.Drawing.Point(265, 3);
            this.flpYear.Name = "flpYear";
            this.flpYear.Size = new System.Drawing.Size(161, 80);
            this.flpYear.TabIndex = 20023;
            // 
            // lblYear
            // 
            this.lblYear.BackColor = System.Drawing.Color.White;
            this.lblYear.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYear.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblYear.Location = new System.Drawing.Point(0, 0);
            this.lblYear.Margin = new System.Windows.Forms.Padding(0);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(161, 80);
            this.lblYear.TabIndex = 2;
            this.lblYear.Text = "1901";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblYear.Click += new System.EventHandler(this.lblYear_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(84, 446);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(831, 170);
            this.panelButtons.TabIndex = 20018;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(449, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(365, 135);
            this.btnSave.TabIndex = 1025;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Day";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Month";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Year";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // flpUsrCtrlDay
            // 
            this.flpUsrCtrlDay.AutoSize = true;
            this.flpUsrCtrlDay.Location = new System.Drawing.Point(31, 333);
            this.flpUsrCtrlDay.Margin = new System.Windows.Forms.Padding(0);
            this.flpUsrCtrlDay.Name = "flpUsrCtrlDay";
            this.flpUsrCtrlDay.Size = new System.Drawing.Size(30, 35);
            this.flpUsrCtrlDay.TabIndex = 20025;
            // 
            // flpUsrCtrlMonth
            // 
            this.flpUsrCtrlMonth.AutoSize = true;
            this.flpUsrCtrlMonth.Location = new System.Drawing.Point(84, 333);
            this.flpUsrCtrlMonth.Margin = new System.Windows.Forms.Padding(0);
            this.flpUsrCtrlMonth.Name = "flpUsrCtrlMonth";
            this.flpUsrCtrlMonth.Size = new System.Drawing.Size(33, 35);
            this.flpUsrCtrlMonth.TabIndex = 20026;
            // 
            // flpUsrCtrlYear
            // 
            this.flpUsrCtrlYear.AutoSize = true;
            this.flpUsrCtrlYear.Location = new System.Drawing.Point(31, 288);
            this.flpUsrCtrlYear.Margin = new System.Windows.Forms.Padding(0);
            this.flpUsrCtrlYear.Name = "flpUsrCtrlYear";
            this.flpUsrCtrlYear.Size = new System.Drawing.Size(27, 35);
            this.flpUsrCtrlYear.TabIndex = 20026;
            // 
            // flpTimePicker
            // 
            this.flpTimePicker.AutoSize = true;
            this.flpTimePicker.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpTimePicker.BackColor = System.Drawing.Color.Transparent;
            this.flpTimePicker.Controls.Add(this.flpHour);
            this.flpTimePicker.Controls.Add(this.lblDelimeter);
            this.flpTimePicker.Controls.Add(this.flpMinute);
            this.flpTimePicker.Controls.Add(this.flpAMPM);
            this.flpTimePicker.Location = new System.Drawing.Point(542, 54);
            this.flpTimePicker.Name = "flpTimePicker";
            this.flpTimePicker.Size = new System.Drawing.Size(367, 86);
            this.flpTimePicker.TabIndex = 20028;
            // 
            // flpHour
            // 
            this.flpHour.AutoSize = true;
            this.flpHour.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpHour.BackColor = System.Drawing.Color.Transparent;
            this.flpHour.Controls.Add(this.lblHour);
            this.flpHour.Location = new System.Drawing.Point(3, 3);
            this.flpHour.Name = "flpHour";
            this.flpHour.Size = new System.Drawing.Size(100, 80);
            this.flpHour.TabIndex = 20021;
            // 
            // lblHour
            // 
            this.lblHour.BackColor = System.Drawing.Color.White;
            this.lblHour.CausesValidation = false;
            this.lblHour.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHour.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblHour.Location = new System.Drawing.Point(0, 0);
            this.lblHour.Margin = new System.Windows.Forms.Padding(0);
            this.lblHour.Name = "lblHour";
            this.lblHour.Size = new System.Drawing.Size(100, 80);
            this.lblHour.TabIndex = 0;
            this.lblHour.Text = "00";
            this.lblHour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHour.Click += new System.EventHandler(this.lblHour_Click);
            // 
            // lblDelimeter
            // 
            this.lblDelimeter.AutoSize = true;
            this.lblDelimeter.BackColor = System.Drawing.Color.White;
            this.lblDelimeter.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDelimeter.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblDelimeter.Location = new System.Drawing.Point(106, 3);
            this.lblDelimeter.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.lblDelimeter.MaximumSize = new System.Drawing.Size(0, 80);
            this.lblDelimeter.MinimumSize = new System.Drawing.Size(0, 80);
            this.lblDelimeter.Name = "lblDelimeter";
            this.lblDelimeter.Size = new System.Drawing.Size(39, 80);
            this.lblDelimeter.TabIndex = 20032;
            this.lblDelimeter.Text = ":";
            this.lblDelimeter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpMinute
            // 
            this.flpMinute.AutoSize = true;
            this.flpMinute.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpMinute.BackColor = System.Drawing.Color.Transparent;
            this.flpMinute.Controls.Add(this.lblMinute);
            this.flpMinute.Location = new System.Drawing.Point(148, 3);
            this.flpMinute.Name = "flpMinute";
            this.flpMinute.Size = new System.Drawing.Size(100, 80);
            this.flpMinute.TabIndex = 20022;
            // 
            // lblMinute
            // 
            this.lblMinute.BackColor = System.Drawing.Color.White;
            this.lblMinute.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinute.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblMinute.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMinute.Location = new System.Drawing.Point(0, 0);
            this.lblMinute.Margin = new System.Windows.Forms.Padding(0);
            this.lblMinute.Name = "lblMinute";
            this.lblMinute.Size = new System.Drawing.Size(100, 80);
            this.lblMinute.TabIndex = 1;
            this.lblMinute.Text = "00";
            this.lblMinute.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMinute.Click += new System.EventHandler(this.lblMinute_Click);
            // 
            // flpAMPM
            // 
            this.flpAMPM.AutoSize = true;
            this.flpAMPM.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpAMPM.BackColor = System.Drawing.Color.Transparent;
            this.flpAMPM.Controls.Add(this.lblAMPM);
            this.flpAMPM.Location = new System.Drawing.Point(254, 3);
            this.flpAMPM.Name = "flpAMPM";
            this.flpAMPM.Size = new System.Drawing.Size(110, 80);
            this.flpAMPM.TabIndex = 20023;
            // 
            // lblAMPM
            // 
            this.lblAMPM.BackColor = System.Drawing.Color.White;
            this.lblAMPM.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAMPM.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblAMPM.Location = new System.Drawing.Point(0, 0);
            this.lblAMPM.Margin = new System.Windows.Forms.Padding(0);
            this.lblAMPM.Name = "lblAMPM";
            this.lblAMPM.Size = new System.Drawing.Size(110, 80);
            this.lblAMPM.TabIndex = 2;
            this.lblAMPM.Text = "AM";
            this.lblAMPM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAMPM.Click += new System.EventHandler(this.lblAMPM_Click);
            // 
            // flpUsrCtrlHour
            // 
            this.flpUsrCtrlHour.AutoSize = true;
            this.flpUsrCtrlHour.Location = new System.Drawing.Point(31, 377);
            this.flpUsrCtrlHour.Margin = new System.Windows.Forms.Padding(0);
            this.flpUsrCtrlHour.Name = "flpUsrCtrlHour";
            this.flpUsrCtrlHour.Size = new System.Drawing.Size(30, 35);
            this.flpUsrCtrlHour.TabIndex = 20029;
            // 
            // flpUsrCtrlMinute
            // 
            this.flpUsrCtrlMinute.AutoSize = true;
            this.flpUsrCtrlMinute.Location = new System.Drawing.Point(84, 377);
            this.flpUsrCtrlMinute.Margin = new System.Windows.Forms.Padding(0);
            this.flpUsrCtrlMinute.Name = "flpUsrCtrlMinute";
            this.flpUsrCtrlMinute.Size = new System.Drawing.Size(33, 35);
            this.flpUsrCtrlMinute.TabIndex = 20030;
            // 
            // flpUsrCtrlAMPM
            // 
            this.flpUsrCtrlAMPM.AutoSize = true;
            this.flpUsrCtrlAMPM.Location = new System.Drawing.Point(63, 288);
            this.flpUsrCtrlAMPM.Margin = new System.Windows.Forms.Padding(0);
            this.flpUsrCtrlAMPM.Name = "flpUsrCtrlAMPM";
            this.flpUsrCtrlAMPM.Size = new System.Drawing.Size(27, 35);
            this.flpUsrCtrlAMPM.TabIndex = 20031;
            // 
            // frmCalender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(984, 637);
            this.Controls.Add(this.flpTimePicker);
            this.Controls.Add(this.flpDatePicker);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.flpUsrCtrlMonth);
            this.Controls.Add(this.flpUsrCtrlMinute);
            this.Controls.Add(this.flpUsrCtrlAMPM);
            this.Controls.Add(this.flpUsrCtrlYear);
            this.Controls.Add(this.flpUsrCtrlDay);
            this.Controls.Add(this.flpUsrCtrlHour);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCalender";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmYesNo";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCalender_FormClosing);
            this.Load += new System.EventHandler(this.frmCalender_Load);
            this.Click += new System.EventHandler(this.frmCalender_Click);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.flpUsrCtrlHour, 0);
            this.Controls.SetChildIndex(this.flpUsrCtrlDay, 0);
            this.Controls.SetChildIndex(this.flpUsrCtrlYear, 0);
            this.Controls.SetChildIndex(this.flpUsrCtrlAMPM, 0);
            this.Controls.SetChildIndex(this.flpUsrCtrlMinute, 0);
            this.Controls.SetChildIndex(this.flpUsrCtrlMonth, 0);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.flpDatePicker, 0);
            this.Controls.SetChildIndex(this.flpTimePicker, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.flpDatePicker.ResumeLayout(false);
            this.flpDatePicker.PerformLayout();
            this.flpDay.ResumeLayout(false);
            this.flpMonth.ResumeLayout(false);
            this.flpYear.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.flpTimePicker.ResumeLayout(false);
            this.flpTimePicker.PerformLayout();
            this.flpHour.ResumeLayout(false);
            this.flpMinute.ResumeLayout(false);
            this.flpAMPM.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.FlowLayoutPanel flpDatePicker;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.FlowLayoutPanel flpDay;
        private System.Windows.Forms.FlowLayoutPanel flpMonth;
        private System.Windows.Forms.FlowLayoutPanel flpYear;
        private System.Windows.Forms.FlowLayoutPanel flpUsrCtrlDay;
        private System.Windows.Forms.FlowLayoutPanel flpUsrCtrlMonth;
        private System.Windows.Forms.FlowLayoutPanel flpUsrCtrlYear;
        private System.Windows.Forms.FlowLayoutPanel flpTimePicker;
        private System.Windows.Forms.FlowLayoutPanel flpHour;
        private System.Windows.Forms.Label lblHour;
        private System.Windows.Forms.FlowLayoutPanel flpMinute;
        private System.Windows.Forms.Label lblMinute;
        private System.Windows.Forms.FlowLayoutPanel flpAMPM;
        private System.Windows.Forms.Label lblAMPM;
        private System.Windows.Forms.FlowLayoutPanel flpUsrCtrlHour;
        private System.Windows.Forms.FlowLayoutPanel flpUsrCtrlMinute;
        private System.Windows.Forms.FlowLayoutPanel flpUsrCtrlAMPM;
        private System.Windows.Forms.Label lblDelimeter;
    }
}
