namespace Redemption_Kiosk
{
    partial class frmRedemptionKioskGiftInfo
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
            this.lblproductName = new System.Windows.Forms.Label();
            this.pbProductImage = new System.Windows.Forms.PictureBox();
            this.lblTickets = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblproductTicket = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtDescription = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbProductImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblproductName
            // 
            this.lblproductName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblproductName.AutoEllipsis = true;
            this.lblproductName.BackColor = System.Drawing.Color.Transparent;
            this.lblproductName.Font = new System.Drawing.Font("Bango Pro", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblproductName.ForeColor = System.Drawing.Color.White;
            this.lblproductName.Location = new System.Drawing.Point(0, 57);
            this.lblproductName.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblproductName.Name = "lblproductName";
            this.lblproductName.Size = new System.Drawing.Size(932, 48);
            this.lblproductName.TabIndex = 0;
            this.lblproductName.Text = "PRODUCT NAME";
            this.lblproductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbProductImage
            // 
            this.pbProductImage.BackColor = System.Drawing.Color.Transparent;
            this.pbProductImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbProductImage.Location = new System.Drawing.Point(272, 173);
            this.pbProductImage.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.pbProductImage.Name = "pbProductImage";
            this.pbProductImage.Size = new System.Drawing.Size(406, 442);
            this.pbProductImage.TabIndex = 1;
            this.pbProductImage.TabStop = false;
            // 
            // lblTickets
            // 
            this.lblTickets.AutoSize = true;
            this.lblTickets.BackColor = System.Drawing.Color.Transparent;
            this.lblTickets.Font = new System.Drawing.Font("Bango Pro", 30F);
            this.lblTickets.ForeColor = System.Drawing.Color.Crimson;
            this.lblTickets.Location = new System.Drawing.Point(330, 642);
            this.lblTickets.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblTickets.Name = "lblTickets";
            this.lblTickets.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTickets.Size = new System.Drawing.Size(182, 48);
            this.lblTickets.TabIndex = 2;
            this.lblTickets.Text = ": Tickets";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Popup_Close_Btn;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("DIN", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Transparent;
            this.btnClose.Location = new System.Drawing.Point(236, 845);
            this.btnClose.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(486, 82);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // lblproductTicket
            // 
            this.lblproductTicket.AutoSize = true;
            this.lblproductTicket.BackColor = System.Drawing.Color.White;
            this.lblproductTicket.Font = new System.Drawing.Font("Bango Pro", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblproductTicket.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblproductTicket.Location = new System.Drawing.Point(515, 642);
            this.lblproductTicket.Name = "lblproductTicket";
            this.lblproductTicket.Size = new System.Drawing.Size(96, 48);
            this.lblproductTicket.TabIndex = 5;
            this.lblproductTicket.Text = "300";
            this.lblproductTicket.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.txtDescription);
            this.panel1.Location = new System.Drawing.Point(128, 714);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(699, 101);
            this.panel1.TabIndex = 7;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescription.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.txtDescription.Location = new System.Drawing.Point(3, 3);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(693, 99);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.Text = "";
            // 
            // frmRedemptionKioskGiftInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Product_Box_3;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(934, 1004);
            this.Controls.Add(this.lblproductName);
            this.Controls.Add(this.pbProductImage);
            this.Controls.Add(this.lblproductTicket);
            this.Controls.Add(this.lblTickets);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.DoubleBuffered = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(15, 13, 15, 13);
            this.Name = "frmRedemptionKioskGiftInfo";
            this.Text = "RedemptionKioskGiftInfo";
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.Load += new System.EventHandler(this.FrmGiftInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbProductImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblproductName;
        private System.Windows.Forms.PictureBox pbProductImage;
        private System.Windows.Forms.Label lblTickets;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblproductTicket;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox txtDescription;
    }
}