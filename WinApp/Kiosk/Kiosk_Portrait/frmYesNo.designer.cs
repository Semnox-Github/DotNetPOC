namespace Parafait_Kiosk
{
    partial class frmYesNo
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
            this.btnYes = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblmsg = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.lblAdditionalMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.home_button;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.TabIndex = 145;
            this.btnHome.Visible = false;
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.Color.Transparent;
            this.btnYes.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnYes.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnYes.Location = new System.Drawing.Point(66, 393);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(365, 135);
            this.btnYes.TabIndex = 10;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            this.btnYes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseDown);
            this.btnYes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseUp);
            // 
            // lblmsg
            // 
            this.lblmsg.AutoEllipsis = true;
            this.lblmsg.BackColor = System.Drawing.Color.Transparent;
            this.lblmsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblmsg.FlatAppearance.BorderSize = 0;
            this.lblmsg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblmsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 33F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmsg.ForeColor = System.Drawing.Color.White;
            this.lblmsg.Location = new System.Drawing.Point(12, 28);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(968, 154);
            this.lblmsg.TabIndex = 13;
            this.lblmsg.Text = "Would you like to register?";
            this.lblmsg.UseVisualStyleBackColor = false;
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.Transparent;
            this.btnNo.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNo.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNo.Location = new System.Drawing.Point(560, 393);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(365, 135);
            this.btnNo.TabIndex = 14;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            this.btnNo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo_MouseDown);
            this.btnNo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnNo_MouseUp);
            // 
            // lblAdditionalMessage
            // 
            this.lblAdditionalMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblAdditionalMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdditionalMessage.ForeColor = System.Drawing.Color.White;
            this.lblAdditionalMessage.Location = new System.Drawing.Point(21, 182);
            this.lblAdditionalMessage.Margin = new System.Windows.Forms.Padding(3);
            this.lblAdditionalMessage.Name = "lblAdditionalMessage";
            this.lblAdditionalMessage.Size = new System.Drawing.Size(950, 196);
            this.lblAdditionalMessage.TabIndex = 18;
            this.lblAdditionalMessage.Text = "Additional\r\nMessage";
            this.lblAdditionalMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmYesNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.CancelButton = this.btnNo;
            this.ClientSize = new System.Drawing.Size(993, 625);
            this.Controls.Add(this.lblAdditionalMessage);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.lblmsg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmYesNo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmYesNo";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.lblmsg, 0);
            this.Controls.SetChildIndex(this.btnNo, 0);
            this.Controls.SetChildIndex(this.btnYes, 0);
            this.Controls.SetChildIndex(this.lblAdditionalMessage, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button lblmsg;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Label lblAdditionalMessage;
        //private System.Windows.Forms.Button btnHome;
    }
}
