namespace Parafait_POS
{
    partial class frmPOSFingerPrintLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOSFingerPrintLogin));
            this.pbPrint = new System.Windows.Forms.PictureBox();
            this.lblSwipeTo = new System.Windows.Forms.Label();
            this.PictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.lnkLoginUsername = new System.Windows.Forms.LinkLabel();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.pbStatusBar = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPrint
            // 
            this.pbPrint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPrint.Location = new System.Drawing.Point(95, 79);
            this.pbPrint.Name = "pbPrint";
            this.pbPrint.Size = new System.Drawing.Size(137, 138);
            this.pbPrint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPrint.TabIndex = 0;
            this.pbPrint.TabStop = false;
            // 
            // lblSwipeTo
            // 
            this.lblSwipeTo.AutoSize = true;
            this.lblSwipeTo.Location = new System.Drawing.Point(68, 237);
            this.lblSwipeTo.Name = "lblSwipeTo";
            this.lblSwipeTo.Size = new System.Drawing.Size(195, 14);
            this.lblSwipeTo.TabIndex = 1;
            this.lblSwipeTo.Text = "Please Swipe Your Finger to Login";
            // 
            // PictureBoxLogo
            // 
            this.PictureBoxLogo.Image = ((System.Drawing.Image)(resources.GetObject("PictureBoxLogo.Image")));
            this.PictureBoxLogo.Location = new System.Drawing.Point(2, 2);
            this.PictureBoxLogo.Name = "PictureBoxLogo";
            this.PictureBoxLogo.Size = new System.Drawing.Size(320, 54);
            this.PictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBoxLogo.TabIndex = 9;
            this.PictureBoxLogo.TabStop = false;
            // 
            // lnkLoginUsername
            // 
            this.lnkLoginUsername.AutoSize = true;
            this.lnkLoginUsername.Location = new System.Drawing.Point(186, 298);
            this.lnkLoginUsername.Name = "lnkLoginUsername";
            this.lnkLoginUsername.Size = new System.Drawing.Size(134, 14);
            this.lnkLoginUsername.TabIndex = 10;
            this.lnkLoginUsername.TabStop = true;
            this.lnkLoginUsername.Text = "Use LoginId Password ";
            this.lnkLoginUsername.Visible = false;
            this.lnkLoginUsername.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLoginUsername_LinkClicked);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(189, 13);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(100, 20);
            this.txtMsg.TabIndex = 12;
            this.txtMsg.Visible = false;
            // 
            // pbStatusBar
            // 
            this.pbStatusBar.Location = new System.Drawing.Point(50, 12);
            this.pbStatusBar.Name = "pbStatusBar";
            this.pbStatusBar.Size = new System.Drawing.Size(100, 10);
            this.pbStatusBar.TabIndex = 13;
            this.pbStatusBar.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(130, 266);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmPOSFingerPrintLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 321);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lnkLoginUsername);
            this.Controls.Add(this.PictureBoxLogo);
            this.Controls.Add(this.lblSwipeTo);
            this.Controls.Add(this.pbPrint);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.pbStatusBar);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPOSFingerPrintLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parafait POS - Finger Print Login";
            this.Shown += new System.EventHandler(this.frmPOSFingerPrintLogin_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbPrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPrint;
        private System.Windows.Forms.Label lblSwipeTo;
        private System.Windows.Forms.PictureBox PictureBoxLogo;
        private System.Windows.Forms.LinkLabel lnkLoginUsername;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.ProgressBar pbStatusBar;
        private System.Windows.Forms.Button btnCancel;
    }
}