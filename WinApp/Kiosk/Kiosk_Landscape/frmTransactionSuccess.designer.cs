namespace Parafait_Kiosk
{
    partial class frmTransactionSuccess
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
            this.lblmsg = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblHeading = new System.Windows.Forms.Button();
            this.pbSuccess = new System.Windows.Forms.PictureBox();
            this.lblTrxNumber = new System.Windows.Forms.Button();
            this.lblPoint = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbSuccess)).BeginInit();
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
            this.btnHome.TabIndex = 144;
            // 
            // lblmsg
            // 
            this.lblmsg.BackColor = System.Drawing.Color.Transparent;
            this.lblmsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblmsg.FlatAppearance.BorderSize = 0;
            this.lblmsg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblmsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.75F);
            this.lblmsg.ForeColor = System.Drawing.Color.White;
            this.lblmsg.Location = new System.Drawing.Point(12, 215);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(737, 227);
            this.lblmsg.TabIndex = 13;
            this.lblmsg.Text = "Message";
            this.lblmsg.UseVisualStyleBackColor = false;
            this.lblmsg.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.done_button;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 33F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClose.Location = new System.Drawing.Point(12, 901);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(1256, 100);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "I\'m Done";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblHeading
            // 
            this.lblHeading.BackColor = System.Drawing.Color.Transparent;
            this.lblHeading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblHeading.FlatAppearance.BorderSize = 0;
            this.lblHeading.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblHeading.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblHeading.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F);
            this.lblHeading.ForeColor = System.Drawing.Color.White;
            this.lblHeading.Location = new System.Drawing.Point(310, 16);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(450, 82);
            this.lblHeading.TabIndex = 145;
            this.lblHeading.Text = "Success";
            this.lblHeading.UseVisualStyleBackColor = false;
            // 
            // pbSuccess
            // 
            this.pbSuccess.BackColor = System.Drawing.Color.Transparent;
            this.pbSuccess.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbSuccess.Image = global::Parafait_Kiosk.Properties.Resources.sucess_Add;
            this.pbSuccess.Location = new System.Drawing.Point(755, 210);
            this.pbSuccess.Name = "pbSuccess";
            this.pbSuccess.Size = new System.Drawing.Size(513, 632);
            this.pbSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbSuccess.TabIndex = 147;
            this.pbSuccess.TabStop = false;
            this.pbSuccess.Visible = false;
            // 
            // lblTrxNumber
            // 
            this.lblTrxNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblTrxNumber.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblTrxNumber.FlatAppearance.BorderSize = 0;
            this.lblTrxNumber.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblTrxNumber.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblTrxNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTrxNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F);
            this.lblTrxNumber.ForeColor = System.Drawing.Color.White;
            this.lblTrxNumber.Location = new System.Drawing.Point(187, 104);
            this.lblTrxNumber.Name = "lblTrxNumber";
            this.lblTrxNumber.Size = new System.Drawing.Size(699, 104);
            this.lblTrxNumber.TabIndex = 151;
            this.lblTrxNumber.Text = "Trx # : ";
            this.lblTrxNumber.UseVisualStyleBackColor = false;
            this.lblTrxNumber.Visible = false;
            // 
            // lblPoint
            // 
            this.lblPoint.BackColor = System.Drawing.Color.Transparent;
            this.lblPoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblPoint.FlatAppearance.BorderSize = 0;
            this.lblPoint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblPoint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F);
            this.lblPoint.ForeColor = System.Drawing.Color.White;
            this.lblPoint.Location = new System.Drawing.Point(12, 450);
            this.lblPoint.Name = "lblPoint";
            this.lblPoint.Size = new System.Drawing.Size(737, 162);
            this.lblPoint.TabIndex = 152;
            this.lblPoint.Text = "Message";
            this.lblPoint.UseVisualStyleBackColor = false;
            this.lblPoint.Visible = false;
            // 
            // frmTransactionSuccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_Screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.Controls.Add(this.lblPoint);
            this.Controls.Add(this.lblTrxNumber);
            this.Controls.Add(this.pbSuccess);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblmsg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmTransactionSuccess";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Text = "frmYesNo";
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSuccessWaiverTransaction_FormClosing);
            this.Load += new System.EventHandler(this.frmSuccessWaiverTransaction_Load);
            this.Controls.SetChildIndex(this.lblmsg, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.lblHeading, 0);
            this.Controls.SetChildIndex(this.pbSuccess, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.lblTrxNumber, 0);
            this.Controls.SetChildIndex(this.lblPoint, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbSuccess)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button lblmsg;
        private System.Windows.Forms.Button btnClose;
        //private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button lblHeading;
        private System.Windows.Forms.PictureBox pbSuccess;
        private System.Windows.Forms.Button lblTrxNumber;
        private System.Windows.Forms.Button lblPoint;
    }
}
