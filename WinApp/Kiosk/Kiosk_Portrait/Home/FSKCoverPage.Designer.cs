namespace Parafait_Kiosk
{
    partial class FSKCoverPage
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
            this.pbClientLogo = new System.Windows.Forms.PictureBox();
            this.pbSemnox = new System.Windows.Forms.PictureBox();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.flpOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnFSKSales = new System.Windows.Forms.Button();
            this.btnExecuteOnlineTransaction = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            this.lblAppVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbClientLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSemnox)).BeginInit();
            this.flpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbClientLogo
            // 
            this.pbClientLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbClientLogo.Location = new System.Drawing.Point(309, 107);
            this.pbClientLogo.Name = "pbClientLogo";
            this.pbClientLogo.Size = new System.Drawing.Size(465, 186);
            this.pbClientLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbClientLogo.TabIndex = 141;
            this.pbClientLogo.TabStop = false;
            // 
            // pbSemnox
            // 
            this.pbSemnox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSemnox.BackColor = System.Drawing.Color.Transparent;
            this.pbSemnox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbSemnox.Image = global::Parafait_Kiosk.Properties.Resources.semnox_logo;
            this.pbSemnox.Location = new System.Drawing.Point(780, 1725);
            this.pbSemnox.Name = "pbSemnox";
            this.pbSemnox.Size = new System.Drawing.Size(215, 36);
            this.pbSemnox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbSemnox.TabIndex = 5;
            this.pbSemnox.TabStop = false;
            // 
            // lblSiteName
            // 
            this.lblSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSiteName.BackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.BorderSize = 0;
            this.lblSiteName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSiteName.Font = new System.Drawing.Font("Gotham Rounded Bold", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(12, 10);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1056, 82);
            this.lblSiteName.TabIndex = 135;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            // 
            // flpOptions
            // 
            this.flpOptions.BackColor = System.Drawing.Color.Transparent;
            this.flpOptions.Controls.Add(this.btnFSKSales);
            this.flpOptions.Controls.Add(this.btnExecuteOnlineTransaction);
            this.flpOptions.Location = new System.Drawing.Point(31, 293);
            this.flpOptions.Name = "flpOptions";
            this.flpOptions.Size = new System.Drawing.Size(1037, 1524);
            this.flpOptions.TabIndex = 3;
            // 
            // btnFSKSales
            // 
            this.btnFSKSales.BackColor = System.Drawing.Color.Transparent;
            this.btnFSKSales.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.New_Play_Pass_Button_big;
            this.btnFSKSales.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFSKSales.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnFSKSales.FlatAppearance.BorderSize = 0;
            this.btnFSKSales.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnFSKSales.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFSKSales.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFSKSales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFSKSales.Font = new System.Drawing.Font("Gotham Rounded Bold", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFSKSales.ForeColor = System.Drawing.Color.White;
            this.btnFSKSales.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFSKSales.Location = new System.Drawing.Point(75, 20);
            this.btnFSKSales.Margin = new System.Windows.Forms.Padding(75, 20, 3, 3);
            this.btnFSKSales.Name = "btnFSKSales";
            this.btnFSKSales.Size = new System.Drawing.Size(863, 369);
            this.btnFSKSales.TabIndex = 0;
            this.btnFSKSales.Text = "New Purchase/Topups";
            this.btnFSKSales.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFSKSales.UseVisualStyleBackColor = false;
            this.btnFSKSales.Click += new System.EventHandler(this.BtnFSKSales_Click);
            // 
            // btnExecuteOnlineTransaction
            // 
            this.btnExecuteOnlineTransaction.BackColor = System.Drawing.Color.Transparent;
            this.btnExecuteOnlineTransaction.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Recharge_Play_Pass_Big;
            this.btnExecuteOnlineTransaction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExecuteOnlineTransaction.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnExecuteOnlineTransaction.FlatAppearance.BorderSize = 0;
            this.btnExecuteOnlineTransaction.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExecuteOnlineTransaction.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExecuteOnlineTransaction.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExecuteOnlineTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecuteOnlineTransaction.Font = new System.Drawing.Font("Gotham Rounded Bold", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecuteOnlineTransaction.ForeColor = System.Drawing.Color.White;
            this.btnExecuteOnlineTransaction.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExecuteOnlineTransaction.Location = new System.Drawing.Point(75, 412);
            this.btnExecuteOnlineTransaction.Margin = new System.Windows.Forms.Padding(75, 20, 3, 3);
            this.btnExecuteOnlineTransaction.Name = "btnExecuteOnlineTransaction";
            this.btnExecuteOnlineTransaction.Size = new System.Drawing.Size(863, 369);
            this.btnExecuteOnlineTransaction.TabIndex = 1;
            this.btnExecuteOnlineTransaction.Text = "Retrieve My Purchase";
            this.btnExecuteOnlineTransaction.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExecuteOnlineTransaction.UseVisualStyleBackColor = false;
            this.btnExecuteOnlineTransaction.Click += new System.EventHandler(this.BtnExecuteOnlineTransaction_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1871);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 49);
            this.txtMessage.TabIndex = 136;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // lblAppVersion
            // 
            this.lblAppVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAppVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblAppVersion.Font = new System.Drawing.Font("Gotham Rounded Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppVersion.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblAppVersion.Location = new System.Drawing.Point(791, 1838);
            this.lblAppVersion.Margin = new System.Windows.Forms.Padding(3);
            this.lblAppVersion.Name = "lblAppVersion";
            this.lblAppVersion.Size = new System.Drawing.Size(204, 20);
            this.lblAppVersion.TabIndex = 149;
            this.lblAppVersion.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // FSKCoverPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.pbClientLogo);
            this.Controls.Add(this.lblAppVersion);
            this.Controls.Add(this.pbSemnox);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.flpOptions);
            this.DoubleBuffered = true;
            this.ShowInTaskbar = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FSKCoverPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Semnox Parafait Self-Service Kiosk";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            //this.Activated += new System.EventHandler(this.FrmFSKCoverPage_Activated);
            //this.Deactivate += new System.EventHandler(this.FrmFSKCoverPage_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmFSKCoverPage_FormClosing);
            this.Load += new System.EventHandler(this.FrmFSKCoverPage_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmFSKCoverPage_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pbClientLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSemnox)).EndInit();
            this.flpOptions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFSKSales;
        private System.Windows.Forms.Button btnExecuteOnlineTransaction; 
        private System.Windows.Forms.FlowLayoutPanel flpOptions; 
        private System.Windows.Forms.PictureBox pbSemnox; 
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button txtMessage; 
        private System.Windows.Forms.PictureBox pbClientLogo;
        private System.Windows.Forms.Label lblAppVersion;
    }
}
