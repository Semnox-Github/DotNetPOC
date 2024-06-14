namespace Parafait_POS.Reservation
{
    partial class frmReservationScheduleDecommission
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
            this.grpSchedules = new System.Windows.Forms.GroupBox();
            this.dgvSchedules = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnScheduleOk = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpAttractionDate = new System.Windows.Forms.DateTimePicker();
            this.grpSchedules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedules)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSchedules
            // 
            this.grpSchedules.Controls.Add(this.dgvSchedules);
            this.grpSchedules.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSchedules.Location = new System.Drawing.Point(6, 41);
            this.grpSchedules.Name = "grpSchedules";
            this.grpSchedules.Size = new System.Drawing.Size(787, 279);
            this.grpSchedules.TabIndex = 1;
            this.grpSchedules.TabStop = false;
            this.grpSchedules.Text = "Schedules";
            // 
            // dgvSchedules
            // 
            this.dgvSchedules.AllowUserToAddRows = false;
            this.dgvSchedules.AllowUserToDeleteRows = false;
            this.dgvSchedules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSchedules.Location = new System.Drawing.Point(6, 19);
            this.dgvSchedules.Name = "dgvSchedules";
            this.dgvSchedules.ReadOnly = true;
            this.dgvSchedules.Size = new System.Drawing.Size(775, 254);
            this.dgvSchedules.TabIndex = 2;
            this.dgvSchedules.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSchedules_CellClick);
            //this.dgvSchedules.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvSchedules_UserDeletingRow);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(487, 331);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(116, 34);
            this.btnClose.TabIndex = 75;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // btnScheduleOk
            // 
            this.btnScheduleOk.BackColor = System.Drawing.Color.Transparent;
            this.btnScheduleOk.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnScheduleOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnScheduleOk.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnScheduleOk.FlatAppearance.BorderSize = 0;
            this.btnScheduleOk.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnScheduleOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnScheduleOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnScheduleOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScheduleOk.ForeColor = System.Drawing.Color.White;
            this.btnScheduleOk.Location = new System.Drawing.Point(187, 331);
            this.btnScheduleOk.Name = "btnScheduleOk";
            this.btnScheduleOk.Size = new System.Drawing.Size(116, 34);
            this.btnScheduleOk.TabIndex = 74;
            this.btnScheduleOk.Text = "OK";
            this.btnScheduleOk.UseVisualStyleBackColor = false;
            this.btnScheduleOk.Click += new System.EventHandler(this.btnScheduleOk_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNext.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Location = new System.Drawing.Point(227, 12);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(30, 25);
            this.btnNext.TabIndex = 78;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 15);
            this.label1.TabIndex = 77;
            this.label1.Text = "For Date:";
            // 
            // dtpAttractionDate
            // 
            this.dtpAttractionDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpAttractionDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpAttractionDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAttractionDate.Location = new System.Drawing.Point(78, 13);
            this.dtpAttractionDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpAttractionDate.Name = "dtpAttractionDate";
            this.dtpAttractionDate.Size = new System.Drawing.Size(143, 21);
            this.dtpAttractionDate.TabIndex = 76;
            this.dtpAttractionDate.ValueChanged += new System.EventHandler(this.dtpAttractionDate_ValueChanged);
            // 
            // frmReservationSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 377);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpAttractionDate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnScheduleOk);
            this.Controls.Add(this.grpSchedules);
            this.Name = "frmReservationSchedule";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reservation Schedules";
            this.grpSchedules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedules)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSchedules;
        private System.Windows.Forms.DataGridView dgvSchedules;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnScheduleOk;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpAttractionDate;

    }
}