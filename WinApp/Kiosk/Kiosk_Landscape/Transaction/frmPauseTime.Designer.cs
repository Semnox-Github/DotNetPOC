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
            this.lblmsg = new System.Windows.Forms.Label();
            this.txtCardNo = new System.Windows.Forms.TextBox();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.lblTimeRemaining = new System.Windows.Forms.Label();
            this.txtTimeRemaining = new System.Windows.Forms.TextBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblmsg
            // 
            this.lblmsg.BackColor = System.Drawing.Color.Transparent;
            this.lblmsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmsg.ForeColor = System.Drawing.Color.White;
            this.lblmsg.Location = new System.Drawing.Point(12, 9);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(758, 119);
            this.lblmsg.TabIndex = 0;
            this.lblmsg.Text = "Below Time will be paused";
            this.lblmsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCardNo
            // 
            this.txtCardNo.BackColor = System.Drawing.Color.LightGray;
            this.txtCardNo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCardNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNo.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtCardNo.Location = new System.Drawing.Point(331, 135);
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.ReadOnly = true;
            this.txtCardNo.Size = new System.Drawing.Size(376, 33);
            this.txtCardNo.TabIndex = 600;
            this.txtCardNo.Text = "543F4A34";
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.ForeColor = System.Drawing.Color.White;
            this.lblCardNumber.Location = new System.Drawing.Point(112, 128);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(200, 46);
            this.lblCardNumber.TabIndex = 4;
            this.lblCardNumber.Text = "Card #:";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTimeRemaining
            // 
            this.lblTimeRemaining.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeRemaining.ForeColor = System.Drawing.Color.White;
            this.lblTimeRemaining.Location = new System.Drawing.Point(40, 208);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(272, 46);
            this.lblTimeRemaining.TabIndex = 5;
            this.lblTimeRemaining.Text = "Time Remaining:";
            this.lblTimeRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTimeRemaining
            // 
            this.txtTimeRemaining.BackColor = System.Drawing.Color.LightGray;
            this.txtTimeRemaining.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTimeRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimeRemaining.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtTimeRemaining.Location = new System.Drawing.Point(331, 214);
            this.txtTimeRemaining.Name = "txtTimeRemaining";
            this.txtTimeRemaining.ReadOnly = true;
            this.txtTimeRemaining.Size = new System.Drawing.Size(376, 33);
            this.txtTimeRemaining.TabIndex = 800;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBack.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnBack.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBack.Location = new System.Drawing.Point(157, 301);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(191, 80);
            this.btnBack.TabIndex = 12;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnOk.Location = new System.Drawing.Point(422, 301);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(191, 80);
            this.btnOk.TabIndex = 13;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmPauseTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(782, 410);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.txtTimeRemaining);
            this.Controls.Add(this.lblTimeRemaining);
            this.Controls.Add(this.lblCardNumber);
            this.Controls.Add(this.txtCardNo);
            this.Controls.Add(this.lblmsg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPauseTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPauseTime";
            this.Load += new System.EventHandler(this.frmPauseTime_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblmsg;
        private System.Windows.Forms.TextBox txtCardNo;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.Label lblTimeRemaining;
        private System.Windows.Forms.TextBox txtTimeRemaining;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnOk;
    }
}