namespace Parafait_POS.Login
{
    partial class frmAttendanceRoles
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
            this.cmbAttendancRoles = new System.Windows.Forms.ComboBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblRecordAttendance = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.btnClockOut = new System.Windows.Forms.Button();
            this.btnBreak = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnClockIn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbAttendancRoles
            // 
            this.cmbAttendancRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttendancRoles.Font = new System.Drawing.Font("Arial", 16F);
            this.cmbAttendancRoles.FormattingEnabled = true;
            this.cmbAttendancRoles.Location = new System.Drawing.Point(283, 114);
            this.cmbAttendancRoles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbAttendancRoles.Name = "cmbAttendancRoles";
            this.cmbAttendancRoles.Size = new System.Drawing.Size(214, 32);
            this.cmbAttendancRoles.TabIndex = 0;
            // 
            // lblRole
            // 
            this.lblRole.Font = new System.Drawing.Font("Arial", 15.75F);
            this.lblRole.Location = new System.Drawing.Point(32, 111);
            this.lblRole.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(251, 38);
            this.lblRole.TabIndex = 1;
            this.lblRole.Text = "Choose Role:";
            this.lblRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 222);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(635, 26);
            this.txtMessage.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Arial", 15.75F);
            this.lblStatus.Location = new System.Drawing.Point(284, 50);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(274, 58);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRecordAttendance
            // 
            this.lblRecordAttendance.Font = new System.Drawing.Font("Arial", 15.75F);
            this.lblRecordAttendance.Location = new System.Drawing.Point(32, 50);
            this.lblRecordAttendance.Name = "lblRecordAttendance";
            this.lblRecordAttendance.Size = new System.Drawing.Size(251, 58);
            this.lblRecordAttendance.TabIndex = 9;
            this.lblRecordAttendance.Text = "Record Attendance:";
            this.lblRecordAttendance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUser
            // 
            this.lblUser.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(32, 9);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(251, 38);
            this.lblUser.TabIndex = 37;
            this.lblUser.Text = "User:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lblUserName
            // 
            this.lblUserName.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Location = new System.Drawing.Point(284, 9);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(274, 38);
            this.lblUserName.TabIndex = 38;
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // btnClockOut
            // 
            this.btnClockOut.BackgroundImage = global::Parafait_POS.Properties.Resources.red_button_normal;
            this.btnClockOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClockOut.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnClockOut.FlatAppearance.BorderSize = 0;
            this.btnClockOut.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClockOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClockOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClockOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClockOut.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClockOut.ForeColor = System.Drawing.Color.Black;
            this.btnClockOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClockOut.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClockOut.Location = new System.Drawing.Point(177, 172);
            this.btnClockOut.Name = "btnClockOut";
            this.btnClockOut.Size = new System.Drawing.Size(139, 40);
            this.btnClockOut.TabIndex = 3;
            this.btnClockOut.Text = "Clock Out";
            this.btnClockOut.UseVisualStyleBackColor = false;
            this.btnClockOut.Click += new System.EventHandler(this.btnClockOut_Click);
            this.btnClockOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            this.btnClockOut.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            // 
            // btnBreak
            // 
            this.btnBreak.BackgroundImage = global::Parafait_POS.Properties.Resources.blue_button_normal;
            this.btnBreak.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBreak.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnBreak.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnBreak.FlatAppearance.BorderSize = 0;
            this.btnBreak.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBreak.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBreak.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBreak.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBreak.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBreak.ForeColor = System.Drawing.Color.Black;
            this.btnBreak.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBreak.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBreak.Location = new System.Drawing.Point(330, 172);
            this.btnBreak.Name = "btnBreak";
            this.btnBreak.Size = new System.Drawing.Size(139, 40);
            this.btnBreak.TabIndex = 4;
            this.btnBreak.Text = "On Break";
            this.btnBreak.UseVisualStyleBackColor = false;
            this.btnBreak.Click += new System.EventHandler(this.btnBreak_Click);
            this.btnBreak.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            this.btnBreak.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(483, 172);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(139, 40);
            this.btnClose.TabIndex = 36;
            this.btnClose.Text = "Cancel";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnClockIn
            // 
            this.btnClockIn.BackgroundImage = global::Parafait_POS.Properties.Resources.green_button_normal;
            this.btnClockIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClockIn.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnClockIn.FlatAppearance.BorderSize = 0;
            this.btnClockIn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClockIn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClockIn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClockIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClockIn.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClockIn.ForeColor = System.Drawing.Color.Black;
            this.btnClockIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClockIn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClockIn.Location = new System.Drawing.Point(24, 172);
            this.btnClockIn.Name = "btnClockIn";
            this.btnClockIn.Size = new System.Drawing.Size(139, 40);
            this.btnClockIn.TabIndex = 6;
            this.btnClockIn.Text = "Clock In";
            this.btnClockIn.UseVisualStyleBackColor = false;
            this.btnClockIn.Click += new System.EventHandler(this.btnClockIn_Click);
            this.btnClockIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            this.btnClockIn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            // 
            // frmAttendanceRoles
            // 
            this.AcceptButton = this.btnClockOut;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnBreak;
            this.ClientSize = new System.Drawing.Size(635, 248);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblRecordAttendance);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnClockIn);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnBreak);
            this.Controls.Add(this.btnClockOut);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.cmbAttendancRoles);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmAttendanceRoles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Attendance Roles";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAttendanceRoles_FormClosed);
            this.Load += new System.EventHandler(this.frmAttendanceRoles_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbAttendancRoles;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Button btnBreak;
        private System.Windows.Forms.Button btnClockOut;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnClockIn;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblRecordAttendance;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblUserName;
    }
}