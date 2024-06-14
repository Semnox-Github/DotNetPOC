namespace Parafait_Kiosk.Transaction
{
    partial class frmPauseTime
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
            this.lblmsg = new System.Windows.Forms.Button();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.txtCardNo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTimeRemaining = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTimeRemainingText = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblmsg
            // 
            this.lblmsg.BackColor = System.Drawing.Color.Transparent;
            this.lblmsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblmsg.FlatAppearance.BorderSize = 0;
            this.lblmsg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblmsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmsg.ForeColor = System.Drawing.Color.White;
            this.lblmsg.Location = new System.Drawing.Point(12, 32);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(944, 147);
            this.lblmsg.TabIndex = 1060;
            this.lblmsg.Text = "Balance Time will be paused";
            this.lblmsg.UseVisualStyleBackColor = false;
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblCardNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCardNumber.Font = new System.Drawing.Font("Gotham Rounded Bold", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.ForeColor = System.Drawing.Color.White;
            this.lblCardNumber.Location = new System.Drawing.Point(47, 211);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(408, 58);
            this.lblCardNumber.TabIndex = 1054;
            this.lblCardNumber.Text = "Card#:";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCardNo
            // 
            this.txtCardNo.BackColor = System.Drawing.Color.Transparent;
            this.txtCardNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtCardNo.Font = new System.Drawing.Font("Gotham Rounded Bold", 28F);
            this.txtCardNo.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtCardNo.Location = new System.Drawing.Point(27, 13);
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.Size = new System.Drawing.Size(349, 54);
            this.txtCardNo.TabIndex = 158;
            this.txtCardNo.Text = "662D2F24";
            this.txtCardNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Controls.Add(this.txtCardNo);
            this.panel1.Location = new System.Drawing.Point(461, 200);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(426, 90);
            this.panel1.TabIndex = 1056;
            // 
            // txtTimeRemaining
            // 
            this.txtTimeRemaining.BackColor = System.Drawing.Color.Transparent;
            this.txtTimeRemaining.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtTimeRemaining.Font = new System.Drawing.Font("Gotham Rounded Bold", 28F);
            this.txtTimeRemaining.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtTimeRemaining.Location = new System.Drawing.Point(27, 19);
            this.txtTimeRemaining.Name = "txtTimeRemaining";
            this.txtTimeRemaining.Size = new System.Drawing.Size(365, 54);
            this.txtTimeRemaining.TabIndex = 160;
            this.txtTimeRemaining.Text = "15 Minutes";
            this.txtTimeRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBack.Location = new System.Drawing.Point(86, 429);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(365, 135);
            this.btnBack.TabIndex = 1059;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(549, 429);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(365, 135);
            this.btnOk.TabIndex = 1058;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.Controls.Add(this.txtTimeRemaining);
            this.panel2.Location = new System.Drawing.Point(461, 306);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(426, 92);
            this.panel2.TabIndex = 1057;
            // 
            // lblTimeRemainingText
            // 
            this.lblTimeRemainingText.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemainingText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeRemainingText.Font = new System.Drawing.Font("Gotham Rounded Bold", 28F);
            this.lblTimeRemainingText.ForeColor = System.Drawing.Color.White;
            this.lblTimeRemainingText.Location = new System.Drawing.Point(27, 320);
            this.lblTimeRemainingText.Name = "lblTimeRemainingText";
            this.lblTimeRemainingText.Size = new System.Drawing.Size(428, 58);
            this.lblTimeRemainingText.TabIndex = 1055;
            this.lblTimeRemainingText.Text = "Time Remaining:";
            this.lblTimeRemainingText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmPauseTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(993, 632);
            this.Controls.Add(this.lblmsg);
            this.Controls.Add(this.lblCardNumber);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblTimeRemainingText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPauseTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPauseTime";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.Load += new System.EventHandler(this.frmPauseTime_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button lblmsg;
        internal System.Windows.Forms.Label lblCardNumber;
        internal System.Windows.Forms.Label txtCardNo;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label txtTimeRemaining;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.Label lblTimeRemainingText;
    }
}