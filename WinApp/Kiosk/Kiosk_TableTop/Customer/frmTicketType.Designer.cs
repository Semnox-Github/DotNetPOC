namespace Parafait_Kiosk
{
    partial class frmTicketType 
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
            this.panelTicketMode = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEticket = new System.Windows.Forms.Button();
            this.lblChangeTicketMode = new System.Windows.Forms.Label();
            this.BtnRealTicket = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.panelTicketMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTicketMode
            // 
            this.panelTicketMode.BackColor = System.Drawing.Color.Transparent;
            this.panelTicketMode.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Bg_Panel;
            this.panelTicketMode.Controls.Add(this.btnCancel);
            this.panelTicketMode.Controls.Add(this.btnEticket);
            this.panelTicketMode.Controls.Add(this.lblChangeTicketMode);
            this.panelTicketMode.Controls.Add(this.BtnRealTicket);
            this.panelTicketMode.Controls.Add(this.btnOk);
            this.panelTicketMode.Location = new System.Drawing.Point(163, 153);
            this.panelTicketMode.Name = "panelTicketMode";
            this.panelTicketMode.Size = new System.Drawing.Size(659, 358);
            this.panelTicketMode.TabIndex = 171;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancel.Location = new System.Drawing.Point(153, 271);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(178, 56);
            this.btnCancel.TabIndex = 168;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEticket
            // 
            this.btnEticket.BackColor = System.Drawing.Color.Transparent;
            this.btnEticket.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.ImageUnchecked;
            this.btnEticket.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEticket.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnEticket.FlatAppearance.BorderSize = 0;
            this.btnEticket.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEticket.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEticket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEticket.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEticket.Location = new System.Drawing.Point(171, 189);
            this.btnEticket.Name = "btnEticket";
            this.btnEticket.Size = new System.Drawing.Size(339, 57);
            this.btnEticket.TabIndex = 167;
            this.btnEticket.Text = "     E-Ticket";
            this.btnEticket.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEticket.UseVisualStyleBackColor = false;
            this.btnEticket.Click += new System.EventHandler(this.btnEticket_Click);
            // 
            // lblChangeTicketMode
            // 
            this.lblChangeTicketMode.BackColor = System.Drawing.Color.Transparent;
            this.lblChangeTicketMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblChangeTicketMode.Font = new System.Drawing.Font("Gotham Rounded Bold", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChangeTicketMode.ForeColor = System.Drawing.Color.White;
            this.lblChangeTicketMode.Location = new System.Drawing.Point(18, 26);
            this.lblChangeTicketMode.MinimumSize = new System.Drawing.Size(659, 0);
            this.lblChangeTicketMode.Name = "lblChangeTicketMode";
            this.lblChangeTicketMode.Size = new System.Drawing.Size(659, 52);
            this.lblChangeTicketMode.TabIndex = 163;
            this.lblChangeTicketMode.Text = "Change Ticket Mode";
            this.lblChangeTicketMode.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // BtnRealTicket
            // 
            this.BtnRealTicket.BackColor = System.Drawing.Color.Transparent;
            this.BtnRealTicket.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.ImageChecked;
            this.BtnRealTicket.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnRealTicket.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.BtnRealTicket.FlatAppearance.BorderSize = 0;
            this.BtnRealTicket.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnRealTicket.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnRealTicket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRealTicket.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnRealTicket.ForeColor = System.Drawing.Color.Black;
            this.BtnRealTicket.Location = new System.Drawing.Point(171, 105);
            this.BtnRealTicket.Name = "BtnRealTicket";
            this.BtnRealTicket.Size = new System.Drawing.Size(339, 57);
            this.BtnRealTicket.TabIndex = 166;
            this.BtnRealTicket.Text = "     Real Ticket";
            this.BtnRealTicket.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnRealTicket.UseVisualStyleBackColor = false;
            this.BtnRealTicket.Click += new System.EventHandler(this.BtnRealTicket_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Gotham Rounded Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnOk.Location = new System.Drawing.Point(379, 271);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(178, 56);
            this.btnOk.TabIndex = 164;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmTicketType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(984, 667);
            this.Controls.Add(this.panelTicketMode);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmTicketType";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmYesNo";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.panelTicketMode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTicketMode;
        private System.Windows.Forms.Button btnEticket;
        private System.Windows.Forms.Label lblChangeTicketMode;
        private System.Windows.Forms.Button BtnRealTicket;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}