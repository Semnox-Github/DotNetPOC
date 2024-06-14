namespace Parafait_POS.OnlineFunctions
{
    partial class frmRescheduleTrx
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
            this.lblCurrentDate = new System.Windows.Forms.Label();
            this.dtpChangeDate = new System.Windows.Forms.DateTimePicker();
            this.btnReschedule = new System.Windows.Forms.Button();
            this.btnSendEmail = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 65);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter New Date:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Current Trx Date:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrentDate
            // 
            this.lblCurrentDate.AutoSize = true;
            this.lblCurrentDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentDate.Location = new System.Drawing.Point(173, 40);
            this.lblCurrentDate.Name = "lblCurrentDate";
            this.lblCurrentDate.Size = new System.Drawing.Size(51, 16);
            this.lblCurrentDate.TabIndex = 2;
            this.lblCurrentDate.Text = "label3";
            this.lblCurrentDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpChangeDate
            // 
            this.dtpChangeDate.CustomFormat = "dd-MMM-yyyy";
            this.dtpChangeDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpChangeDate.Location = new System.Drawing.Point(176, 65);
            this.dtpChangeDate.Name = "dtpChangeDate";
            this.dtpChangeDate.Size = new System.Drawing.Size(135, 22);
            this.dtpChangeDate.TabIndex = 3;
            // 
            // btnReschedule
            // 
            this.btnReschedule.BackColor = System.Drawing.Color.Transparent;
            this.btnReschedule.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnReschedule.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReschedule.FlatAppearance.BorderSize = 0;
            this.btnReschedule.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnReschedule.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnReschedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReschedule.ForeColor = System.Drawing.Color.White;
            this.btnReschedule.Location = new System.Drawing.Point(34, 138);
            this.btnReschedule.Name = "btnReschedule";
            this.btnReschedule.Size = new System.Drawing.Size(128, 32);
            this.btnReschedule.TabIndex = 12;
            this.btnReschedule.Text = "Reschedule";
            this.btnReschedule.UseVisualStyleBackColor = false;
            this.btnReschedule.Click += new System.EventHandler(this.btnReschedule_Click);
            // 
            // btnSendEmail
            // 
            this.btnSendEmail.BackColor = System.Drawing.Color.Transparent;
            this.btnSendEmail.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSendEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSendEmail.FlatAppearance.BorderSize = 0;
            this.btnSendEmail.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSendEmail.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSendEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendEmail.ForeColor = System.Drawing.Color.White;
            this.btnSendEmail.Location = new System.Drawing.Point(181, 138);
            this.btnSendEmail.Name = "btnSendEmail";
            this.btnSendEmail.Size = new System.Drawing.Size(128, 32);
            this.btnSendEmail.TabIndex = 13;
            this.btnSendEmail.Text = "Send Email";
            this.btnSendEmail.UseVisualStyleBackColor = false; 
            this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(328, 138);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(128, 32);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // frmRescheduleTrx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(490, 196);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSendEmail);
            this.Controls.Add(this.btnReschedule);
            this.Controls.Add(this.dtpChangeDate);
            this.Controls.Add(this.lblCurrentDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRescheduleTrx";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Reschedule Transaction";
            this.Load += new System.EventHandler(this.frmRescheduleTrx_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCurrentDate;
        private System.Windows.Forms.DateTimePicker dtpChangeDate;
        private System.Windows.Forms.Button btnReschedule;
        private System.Windows.Forms.Button btnSendEmail;
        private System.Windows.Forms.Button btnClose;
    }
}