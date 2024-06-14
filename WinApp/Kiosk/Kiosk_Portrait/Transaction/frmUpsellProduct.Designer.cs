namespace Parafait_Kiosk
{
    partial class frmUpsellProduct
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
            this.lblSiteName = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.exitTimer = new System.Windows.Forms.Timer(this.components);
            this.txtMessage = new System.Windows.Forms.Button();
            this.lblGreeting1 = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.lblOffer = new System.Windows.Forms.Label();
            this.lblTimeOut = new System.Windows.Forms.Button();
            this.panelUpsell = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDesc1 = new System.Windows.Forms.Label();
            this.lblDesc2 = new System.Windows.Forms.Label();
            this.lblDesc3 = new System.Windows.Forms.Label();
            this.pbOfferLogo = new System.Windows.Forms.PictureBox();
            this.panelUpsell.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOfferLogo)).BeginInit();
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
            this.btnHome.TabIndex = 160;
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
            // lblSiteName
            // 
            this.lblSiteName.BackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSiteName.FlatAppearance.BorderSize = 0;
            this.lblSiteName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSiteName.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(0, 0);
            this.lblSiteName.Margin = new System.Windows.Forms.Padding(6);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1076, 82);
            this.lblSiteName.TabIndex = 137;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            this.lblSiteName.Visible = false;
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNo.BackColor = System.Drawing.Color.Transparent;
            this.btnNo.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.done_button;
            this.btnNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNo.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Gotham Rounded Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.Location = new System.Drawing.Point(12, 1745);
            this.btnNo.Margin = new System.Windows.Forms.Padding(6);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(1056, 100);
            this.btnNo.TabIndex = 138;
            this.btnNo.Text = "No Thanks";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            this.btnNo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo_MouseDown);
            this.btnNo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnNo_MouseUp);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1870);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1076, 50);
            this.txtMessage.TabIndex = 146;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // lblGreeting1
            // 
            this.lblGreeting1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreeting1.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting1.Font = new System.Drawing.Font("Gotham Rounded Bold", 35.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(26)))));
            this.lblGreeting1.Location = new System.Drawing.Point(9, 942);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(1061, 56);
            this.lblGreeting1.TabIndex = 149;
            this.lblGreeting1.Text = "You selected $20 (60 Points)";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYes.BackColor = System.Drawing.Color.Transparent;
            this.btnYes.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.sure_button;
            this.btnYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnYes.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.ForeColor = System.Drawing.Color.DarkOrchid;
            this.btnYes.Location = new System.Drawing.Point(310, 1460);
            this.btnYes.Margin = new System.Windows.Forms.Padding(6, 20, 6, 6);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(461, 162);
            this.btnYes.TabIndex = 150;
            this.btnYes.Text = "SURE, WHY NOT";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            this.btnYes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseDown);
            this.btnYes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseUp);
            // 
            // lblOffer
            // 
            this.lblOffer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOffer.BackColor = System.Drawing.Color.Transparent;
            this.lblOffer.Font = new System.Drawing.Font("Gotham Rounded Bold", 41.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOffer.ForeColor = System.Drawing.Color.White;
            this.lblOffer.Location = new System.Drawing.Point(9, 259);
            this.lblOffer.Name = "lblOffer";
            this.lblOffer.Size = new System.Drawing.Size(1061, 82);
            this.lblOffer.TabIndex = 151;
            this.lblOffer.Text = "SPECIAL OFFER";
            this.lblOffer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTimeOut
            // 
            this.lblTimeOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeOut.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.timer_SmallBox;
            this.lblTimeOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lblTimeOut.FlatAppearance.BorderSize = 0;
            this.lblTimeOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeOut.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOut.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblTimeOut.Location = new System.Drawing.Point(929, 76);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(142, 110);
            this.lblTimeOut.TabIndex = 153;
            this.lblTimeOut.UseVisualStyleBackColor = false;
            // 
            // panelUpsell
            // 
            this.panelUpsell.AutoScroll = true;
            this.panelUpsell.BackColor = System.Drawing.Color.Transparent;
            this.panelUpsell.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUpsell.Controls.Add(this.lblDesc1);
            this.panelUpsell.Controls.Add(this.lblDesc2);
            this.panelUpsell.Controls.Add(this.lblDesc3);
            this.panelUpsell.Location = new System.Drawing.Point(135, 1040);
            this.panelUpsell.Name = "panelUpsell";
            this.panelUpsell.Size = new System.Drawing.Size(808, 226);
            this.panelUpsell.TabIndex = 158;
            // 
            // lblDesc1
            // 
            this.lblDesc1.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesc1.ForeColor = System.Drawing.Color.White;
            this.lblDesc1.Location = new System.Drawing.Point(3, 0);
            this.lblDesc1.Name = "lblDesc1";
            this.lblDesc1.Size = new System.Drawing.Size(778, 62);
            this.lblDesc1.TabIndex = 152;
            this.lblDesc1.Text = "label1";
            this.lblDesc1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDesc2
            // 
            this.lblDesc2.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesc2.ForeColor = System.Drawing.Color.White;
            this.lblDesc2.Location = new System.Drawing.Point(3, 62);
            this.lblDesc2.Name = "lblDesc2";
            this.lblDesc2.Size = new System.Drawing.Size(778, 71);
            this.lblDesc2.TabIndex = 153;
            this.lblDesc2.Text = "label2";
            this.lblDesc2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDesc3
            // 
            this.lblDesc3.Font = new System.Drawing.Font("Gotham Rounded Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesc3.ForeColor = System.Drawing.Color.White;
            this.lblDesc3.Location = new System.Drawing.Point(3, 133);
            this.lblDesc3.Name = "lblDesc3";
            this.lblDesc3.Size = new System.Drawing.Size(778, 59);
            this.lblDesc3.TabIndex = 154;
            this.lblDesc3.Text = "label3";
            this.lblDesc3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbOfferLogo
            // 
            this.pbOfferLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbOfferLogo.ErrorImage = global::Parafait_Kiosk.Properties.Resources.special_offer_semnox_logo;
            this.pbOfferLogo.Image = global::Parafait_Kiosk.Properties.Resources.special_offer_semnox_logo;
            this.pbOfferLogo.Location = new System.Drawing.Point(221, 349);
            this.pbOfferLogo.Name = "pbOfferLogo";
            this.pbOfferLogo.Size = new System.Drawing.Size(639, 539);
            this.pbOfferLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbOfferLogo.TabIndex = 159;
            this.pbOfferLogo.TabStop = false;
            this.pbOfferLogo.Visible = false;
            // 
            // frmUpsellProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.lblGreeting1);
            this.Controls.Add(this.lblTimeOut);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblOffer);
            this.Controls.Add(this.pbOfferLogo);
            this.Controls.Add(this.panelUpsell);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblSiteName);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmUpsellProduct";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Upsell Offers";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpsellProduct_FormClosing);
            this.Load += new System.EventHandler(this.frmRedeemTokens_Load);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblSiteName, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.btnNo, 0);
            this.Controls.SetChildIndex(this.panelUpsell, 0);
            this.Controls.SetChildIndex(this.pbOfferLogo, 0);
            this.Controls.SetChildIndex(this.lblOffer, 0);
            this.Controls.SetChildIndex(this.btnYes, 0);
            this.Controls.SetChildIndex(this.lblTimeOut, 0);
            this.Controls.SetChildIndex(this.lblGreeting1, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.panelUpsell.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbOfferLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Timer exitTimer;
        private System.Windows.Forms.Button txtMessage;
        internal System.Windows.Forms.Label lblGreeting1;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Label lblOffer;
        private System.Windows.Forms.Button lblTimeOut;
        private System.Windows.Forms.FlowLayoutPanel panelUpsell;
        private System.Windows.Forms.Label lblDesc1;
        private System.Windows.Forms.Label lblDesc2;
        private System.Windows.Forms.Label lblDesc3;
        private System.Windows.Forms.PictureBox pbOfferLogo;
        //private System.Windows.Forms.Button btnHome;
    }
}
