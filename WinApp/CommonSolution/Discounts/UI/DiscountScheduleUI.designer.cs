namespace Semnox.Parafait.Discounts
{
    partial class DiscountScheduleUI
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
            if(disposing && (components != null))
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
            this.dtpScheduleEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpScheduleEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblScheduleEndDate = new System.Windows.Forms.Label();
            this.dtpScheduleTime = new System.Windows.Forms.DateTimePicker();
            this.dtpScheduleDate = new System.Windows.Forms.DateTimePicker();
            this.lblScheduleDate = new System.Windows.Forms.Label();
            this.chbActive = new System.Windows.Forms.CheckBox();
            this.txtScheduleName = new System.Windows.Forms.TextBox();
            this.lblScheduleName = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdbWeekDay = new System.Windows.Forms.RadioButton();
            this.rdbDay = new System.Windows.Forms.RadioButton();
            this.dtpRecurEndDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.rdbMonthly = new System.Windows.Forms.RadioButton();
            this.rdbWeekly = new System.Windows.Forms.RadioButton();
            this.rdbDaily = new System.Windows.Forms.RadioButton();
            this.chbRecurFlag = new System.Windows.Forms.CheckBox();
            this.btnInclExclDays = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpScheduleEndTime
            // 
            this.dtpScheduleEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpScheduleEndTime.Location = new System.Drawing.Point(233, 71);
            this.dtpScheduleEndTime.Name = "dtpScheduleEndTime";
            this.dtpScheduleEndTime.ShowUpDown = true;
            this.dtpScheduleEndTime.Size = new System.Drawing.Size(100, 20);
            this.dtpScheduleEndTime.TabIndex = 19;
            // 
            // dtpScheduleEndDate
            // 
            this.dtpScheduleEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpScheduleEndDate.Location = new System.Drawing.Point(119, 71);
            this.dtpScheduleEndDate.Name = "dtpScheduleEndDate";
            this.dtpScheduleEndDate.Size = new System.Drawing.Size(100, 20);
            this.dtpScheduleEndDate.TabIndex = 18;
            // 
            // lblScheduleEndDate
            // 
            this.lblScheduleEndDate.AutoSize = true;
            this.lblScheduleEndDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblScheduleEndDate.Location = new System.Drawing.Point(50, 74);
            this.lblScheduleEndDate.Name = "lblScheduleEndDate";
            this.lblScheduleEndDate.Size = new System.Drawing.Size(65, 15);
            this.lblScheduleEndDate.TabIndex = 17;
            this.lblScheduleEndDate.Text = "End Time :";
            // 
            // dtpScheduleTime
            // 
            this.dtpScheduleTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpScheduleTime.Location = new System.Drawing.Point(233, 38);
            this.dtpScheduleTime.Name = "dtpScheduleTime";
            this.dtpScheduleTime.ShowUpDown = true;
            this.dtpScheduleTime.Size = new System.Drawing.Size(100, 20);
            this.dtpScheduleTime.TabIndex = 16;
            // 
            // dtpScheduleDate
            // 
            this.dtpScheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpScheduleDate.Location = new System.Drawing.Point(119, 38);
            this.dtpScheduleDate.Name = "dtpScheduleDate";
            this.dtpScheduleDate.Size = new System.Drawing.Size(100, 20);
            this.dtpScheduleDate.TabIndex = 15;
            this.dtpScheduleDate.ValueChanged += new System.EventHandler(this.dtpScheduleDate_ValueChanged);
            // 
            // lblScheduleDate
            // 
            this.lblScheduleDate.AutoSize = true;
            this.lblScheduleDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblScheduleDate.Location = new System.Drawing.Point(43, 41);
            this.lblScheduleDate.Name = "lblScheduleDate";
            this.lblScheduleDate.Size = new System.Drawing.Size(72, 15);
            this.lblScheduleDate.TabIndex = 14;
            this.lblScheduleDate.Text = "Start Time :";
            // 
            // chbActive
            // 
            this.chbActive.AutoSize = true;
            this.chbActive.Checked = true;
            this.chbActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbActive.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbActive.Location = new System.Drawing.Point(235, 8);
            this.chbActive.Name = "chbActive";
            this.chbActive.Size = new System.Drawing.Size(61, 19);
            this.chbActive.TabIndex = 13;
            this.chbActive.Text = "Active";
            this.chbActive.UseVisualStyleBackColor = true;
            // 
            // txtScheduleName
            // 
            this.txtScheduleName.Location = new System.Drawing.Point(119, 7);
            this.txtScheduleName.Name = "txtScheduleName";
            this.txtScheduleName.Size = new System.Drawing.Size(100, 20);
            this.txtScheduleName.TabIndex = 12;
            // 
            // lblScheduleName
            // 
            this.lblScheduleName.AutoSize = true;
            this.lblScheduleName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblScheduleName.Location = new System.Drawing.Point(13, 10);
            this.lblScheduleName.Name = "lblScheduleName";
            this.lblScheduleName.Size = new System.Drawing.Size(102, 15);
            this.lblScheduleName.TabIndex = 11;
            this.lblScheduleName.Text = "Schedule Name :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.dtpRecurEndDate);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.rdbMonthly);
            this.groupBox2.Controls.Add(this.rdbWeekly);
            this.groupBox2.Controls.Add(this.rdbDaily);
            this.groupBox2.Controls.Add(this.chbRecurFlag);
            this.groupBox2.Location = new System.Drawing.Point(16, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(332, 164);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recurrence";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdbWeekDay);
            this.groupBox3.Controls.Add(this.rdbDay);
            this.groupBox3.Location = new System.Drawing.Point(14, 84);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(145, 50);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Recurrence Type";
            // 
            // rdbWeekDay
            // 
            this.rdbWeekDay.AutoSize = true;
            this.rdbWeekDay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rdbWeekDay.Location = new System.Drawing.Point(56, 19);
            this.rdbWeekDay.Name = "rdbWeekDay";
            this.rdbWeekDay.Size = new System.Drawing.Size(79, 19);
            this.rdbWeekDay.TabIndex = 1;
            this.rdbWeekDay.TabStop = true;
            this.rdbWeekDay.Text = "WeekDay";
            this.rdbWeekDay.UseVisualStyleBackColor = true;
            // 
            // rdbDay
            // 
            this.rdbDay.AutoSize = true;
            this.rdbDay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rdbDay.Location = new System.Drawing.Point(6, 19);
            this.rdbDay.Name = "rdbDay";
            this.rdbDay.Size = new System.Drawing.Size(46, 19);
            this.rdbDay.TabIndex = 0;
            this.rdbDay.TabStop = true;
            this.rdbDay.Text = "Day";
            this.rdbDay.UseVisualStyleBackColor = true;
            // 
            // dtpRecurEndDate
            // 
            this.dtpRecurEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpRecurEndDate.Location = new System.Drawing.Point(113, 46);
            this.dtpRecurEndDate.Name = "dtpRecurEndDate";
            this.dtpRecurEndDate.Size = new System.Drawing.Size(100, 20);
            this.dtpRecurEndDate.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(11, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Recur End Date :";
            // 
            // rdbMonthly
            // 
            this.rdbMonthly.AutoSize = true;
            this.rdbMonthly.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rdbMonthly.Location = new System.Drawing.Point(234, 19);
            this.rdbMonthly.Name = "rdbMonthly";
            this.rdbMonthly.Size = new System.Drawing.Size(69, 19);
            this.rdbMonthly.TabIndex = 9;
            this.rdbMonthly.TabStop = true;
            this.rdbMonthly.Text = "Monthly";
            this.rdbMonthly.UseVisualStyleBackColor = true;
            this.rdbMonthly.CheckedChanged += new System.EventHandler(this.rdbMonthly_CheckedChanged);
            // 
            // rdbWeekly
            // 
            this.rdbWeekly.AutoSize = true;
            this.rdbWeekly.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rdbWeekly.Location = new System.Drawing.Point(167, 19);
            this.rdbWeekly.Name = "rdbWeekly";
            this.rdbWeekly.Size = new System.Drawing.Size(67, 19);
            this.rdbWeekly.TabIndex = 8;
            this.rdbWeekly.TabStop = true;
            this.rdbWeekly.Text = "Weekly";
            this.rdbWeekly.UseVisualStyleBackColor = true;
            this.rdbWeekly.CheckedChanged += new System.EventHandler(this.rdbWeekly_CheckedChanged);
            // 
            // rdbDaily
            // 
            this.rdbDaily.AutoSize = true;
            this.rdbDaily.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.rdbDaily.Location = new System.Drawing.Point(113, 19);
            this.rdbDaily.Name = "rdbDaily";
            this.rdbDaily.Size = new System.Drawing.Size(52, 19);
            this.rdbDaily.TabIndex = 7;
            this.rdbDaily.TabStop = true;
            this.rdbDaily.Text = "Daily";
            this.rdbDaily.UseVisualStyleBackColor = true;
            this.rdbDaily.CheckedChanged += new System.EventHandler(this.rdbDaily_CheckedChanged);
            // 
            // chbRecurFlag
            // 
            this.chbRecurFlag.AutoSize = true;
            this.chbRecurFlag.Checked = true;
            this.chbRecurFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbRecurFlag.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbRecurFlag.Location = new System.Drawing.Point(14, 19);
            this.chbRecurFlag.Name = "chbRecurFlag";
            this.chbRecurFlag.Size = new System.Drawing.Size(86, 19);
            this.chbRecurFlag.TabIndex = 6;
            this.chbRecurFlag.Text = "Recur Flag";
            this.chbRecurFlag.UseVisualStyleBackColor = true;
            this.chbRecurFlag.CheckStateChanged += new System.EventHandler(this.chbRecurFlag_CheckStateChanged);
            // 
            // btnInclExclDays
            // 
            this.btnInclExclDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInclExclDays.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnInclExclDays.Location = new System.Drawing.Point(233, 267);
            this.btnInclExclDays.Name = "btnInclExclDays";
            this.btnInclExclDays.Size = new System.Drawing.Size(75, 23);
            this.btnInclExclDays.TabIndex = 28;
            this.btnInclExclDays.Text = "Incl/Excl Days";
            this.btnInclExclDays.UseVisualStyleBackColor = true;
            this.btnInclExclDays.Click += new System.EventHandler(this.btnInclExclDays_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(13, 267);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 27;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(123, 267);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DiscountScheduleUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 303);
            this.Controls.Add(this.btnInclExclDays);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dtpScheduleEndTime);
            this.Controls.Add(this.dtpScheduleEndDate);
            this.Controls.Add(this.lblScheduleEndDate);
            this.Controls.Add(this.dtpScheduleTime);
            this.Controls.Add(this.dtpScheduleDate);
            this.Controls.Add(this.lblScheduleDate);
            this.Controls.Add(this.chbActive);
            this.Controls.Add(this.txtScheduleName);
            this.Controls.Add(this.lblScheduleName);
            this.Name = "DiscountScheduleUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Discount Schedule";
            this.Load += new System.EventHandler(this.DiscountScheduleUI_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpScheduleEndTime;
        private System.Windows.Forms.DateTimePicker dtpScheduleEndDate;
        private System.Windows.Forms.Label lblScheduleEndDate;
        private System.Windows.Forms.DateTimePicker dtpScheduleTime;
        private System.Windows.Forms.DateTimePicker dtpScheduleDate;
        private System.Windows.Forms.Label lblScheduleDate;
        private System.Windows.Forms.CheckBox chbActive;
        private System.Windows.Forms.TextBox txtScheduleName;
        private System.Windows.Forms.Label lblScheduleName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdbWeekDay;
        private System.Windows.Forms.RadioButton rdbDay;
        private System.Windows.Forms.DateTimePicker dtpRecurEndDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdbMonthly;
        private System.Windows.Forms.RadioButton rdbWeekly;
        private System.Windows.Forms.RadioButton rdbDaily;
        private System.Windows.Forms.CheckBox chbRecurFlag;
        private System.Windows.Forms.Button btnInclExclDays;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
    }
}