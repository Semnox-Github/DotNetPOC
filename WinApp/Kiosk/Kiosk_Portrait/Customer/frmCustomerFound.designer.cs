namespace Parafait_Kiosk
{
    partial class frmCustomerFound
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
            this.lblCustomerName = new System.Windows.Forms.Button();
            this.btnProceed = new System.Windows.Forms.Button();
            this.pBCustomerFound = new System.Windows.Forms.PictureBox();
            this.lblWelcomeMsg = new System.Windows.Forms.Label();
            this.lblClickOKMsg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pBCustomerFound)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // btnPrev
            // 
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(89, 440);
            this.btnPrev.Size = new System.Drawing.Size(365, 135);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(89, 440);
            this.btnCancel.Size = new System.Drawing.Size(365, 135);
            this.btnCancel.Visible = true;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoEllipsis = true;
            this.lblCustomerName.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomerName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.lblCustomerName.FlatAppearance.BorderSize = 0;
            this.lblCustomerName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblCustomerName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblCustomerName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCustomerName.Font = new System.Drawing.Font("Gotham Rounded Bold", 45.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.ForeColor = System.Drawing.Color.White;
            this.lblCustomerName.Location = new System.Drawing.Point(8, 28);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(970, 94);
            this.lblCustomerName.TabIndex = 13;
            this.lblCustomerName.Text = "Dear Sathyavathi Saligrama";
            this.lblCustomerName.UseVisualStyleBackColor = false;
            // 
            // btnProceed
            // 
            this.btnProceed.BackColor = System.Drawing.Color.Transparent;
            this.btnProceed.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnProceed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProceed.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnProceed.FlatAppearance.BorderSize = 0;
            this.btnProceed.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProceed.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProceed.Location = new System.Drawing.Point(529, 440);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(365, 135);
            this.btnProceed.TabIndex = 14;
            this.btnProceed.Text = "Confirm";
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // pBCustomerFound
            // 
            this.pBCustomerFound.BackColor = System.Drawing.Color.Transparent;
            this.pBCustomerFound.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.CustomerFound;
            this.pBCustomerFound.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pBCustomerFound.Location = new System.Drawing.Point(400, 203);
            this.pBCustomerFound.Name = "pBCustomerFound";
            this.pBCustomerFound.Size = new System.Drawing.Size(186, 147);
            this.pBCustomerFound.TabIndex = 1613;
            this.pBCustomerFound.TabStop = false;
            // 
            // lblWelcomeMsg
            // 
            this.lblWelcomeMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblWelcomeMsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcomeMsg.ForeColor = System.Drawing.Color.White;
            this.lblWelcomeMsg.Location = new System.Drawing.Point(5, 128);
            this.lblWelcomeMsg.Margin = new System.Windows.Forms.Padding(0);
            this.lblWelcomeMsg.Name = "lblWelcomeMsg";
            this.lblWelcomeMsg.Size = new System.Drawing.Size(977, 65);
            this.lblWelcomeMsg.TabIndex = 1616;
            this.lblWelcomeMsg.Text = "Welcome Back!!";
            this.lblWelcomeMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblClickOKMsg
            // 
            this.lblClickOKMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblClickOKMsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClickOKMsg.ForeColor = System.Drawing.Color.White;
            this.lblClickOKMsg.Location = new System.Drawing.Point(9, 355);
            this.lblClickOKMsg.Margin = new System.Windows.Forms.Padding(0);
            this.lblClickOKMsg.Name = "lblClickOKMsg";
            this.lblClickOKMsg.Size = new System.Drawing.Size(973, 70);
            this.lblClickOKMsg.TabIndex = 1617;
            this.lblClickOKMsg.Text = "Click Confirm to Proceed";
            this.lblClickOKMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmCustomerFound
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(984, 637);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.pBCustomerFound);
            this.Controls.Add(this.lblClickOKMsg);
            this.Controls.Add(this.lblWelcomeMsg);
            this.Controls.Add(this.btnProceed);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCustomerFound";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmYesNo";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCustomerFound_FormClosing);
            this.Load += new System.EventHandler(this.frmCustomerFound_Load);
            this.Controls.SetChildIndex(this.btnProceed, 0);
            this.Controls.SetChildIndex(this.lblWelcomeMsg, 0);
            this.Controls.SetChildIndex(this.lblClickOKMsg, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.pBCustomerFound, 0);
            this.Controls.SetChildIndex(this.lblCustomerName, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pBCustomerFound)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button lblCustomerName;
        private System.Windows.Forms.Button btnProceed;
        private System.Windows.Forms.PictureBox pBCustomerFound;
        private System.Windows.Forms.Label lblWelcomeMsg;
        private System.Windows.Forms.Label lblClickOKMsg;
    }
}
