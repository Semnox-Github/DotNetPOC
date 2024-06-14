namespace Parafait_Kiosk
{
    partial class frmFundRaiserAndDonation
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
            this.lblGreeting1 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tlpFundDonationProducts = new System.Windows.Forms.TableLayoutPanel();
            this.pbFundDonationLogo = new System.Windows.Forms.PictureBox();
            this.btnNoThanks = new System.Windows.Forms.Button();
            this.bigVerticalScrollFundDonationProducts = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.tlpFundDonationProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbFundDonationLogo)).BeginInit();
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
            // lblGreeting1
            // 
            this.lblGreeting1.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting1.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.White;
            this.lblGreeting1.Location = new System.Drawing.Point(183, 9);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(1537, 135);
            this.lblGreeting1.TabIndex = 132;
            this.lblGreeting1.Text = "Are You Here For Fund Raiser Event?";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Bango Pro", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1011);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 50);
            this.txtMessage.TabIndex = 147;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Bango Pro", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(993, 845);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(250, 125);
            this.btnOK.TabIndex = 1076;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tlpFundDonationProducts
            // 
            this.tlpFundDonationProducts.AutoScroll = true;
            this.tlpFundDonationProducts.BackColor = System.Drawing.Color.Transparent;
            this.tlpFundDonationProducts.ColumnCount = 1;
            this.tlpFundDonationProducts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFundDonationProducts.Controls.Add(this.pbFundDonationLogo, 0, 0);
            this.tlpFundDonationProducts.Location = new System.Drawing.Point(215, 163);
            this.tlpFundDonationProducts.Name = "tlpFundDonationProducts";
            this.tlpFundDonationProducts.RowCount = 2;
            this.tlpFundDonationProducts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFundDonationProducts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFundDonationProducts.Size = new System.Drawing.Size(1500, 630);
            this.tlpFundDonationProducts.TabIndex = 1077;
            // 
            // pbFundDonationLogo
            // 
            this.pbFundDonationLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbFundDonationLogo.ErrorImage = null;
            this.pbFundDonationLogo.Location = new System.Drawing.Point(0, 3);
            this.pbFundDonationLogo.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.pbFundDonationLogo.Name = "pbFundDonationLogo";
            this.pbFundDonationLogo.Size = new System.Drawing.Size(1442, 199);
            this.pbFundDonationLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbFundDonationLogo.TabIndex = 1078;
            this.pbFundDonationLogo.TabStop = false;
            // 
            // btnNoThanks
            // 
            this.btnNoThanks.BackColor = System.Drawing.Color.Transparent;
            this.btnNoThanks.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnNoThanks.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNoThanks.FlatAppearance.BorderSize = 0;
            this.btnNoThanks.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNoThanks.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNoThanks.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNoThanks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNoThanks.Font = new System.Drawing.Font("Bango Pro", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNoThanks.ForeColor = System.Drawing.Color.White;
            this.btnNoThanks.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNoThanks.Location = new System.Drawing.Point(678, 845);
            this.btnNoThanks.Name = "btnNoThanks";
            this.btnNoThanks.Size = new System.Drawing.Size(250, 125);
            this.btnNoThanks.TabIndex = 1078;
            this.btnNoThanks.Text = "No Thanks";
            this.btnNoThanks.UseVisualStyleBackColor = false;
            this.btnNoThanks.Click += new System.EventHandler(this.btnNoThanks_Click);
            // 
            // bigVerticalScrollFundDonationProducts
            // 
            this.bigVerticalScrollFundDonationProducts.AutoHide = true;
            this.bigVerticalScrollFundDonationProducts.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollFundDonationProducts.DataGridView = null;
            this.bigVerticalScrollFundDonationProducts.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollFundDonationProducts.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollFundDonationProducts.Location = new System.Drawing.Point(1657, 163);
            this.bigVerticalScrollFundDonationProducts.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollFundDonationProducts.Name = "bigVerticalScrollFundDonationProducts";
            this.bigVerticalScrollFundDonationProducts.ScrollableControl = this.tlpFundDonationProducts;
            this.bigVerticalScrollFundDonationProducts.ScrollViewer = null;
            this.bigVerticalScrollFundDonationProducts.Size = new System.Drawing.Size(63, 630);
            this.bigVerticalScrollFundDonationProducts.TabIndex = 1085;
            this.bigVerticalScrollFundDonationProducts.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollFundDonationProducts.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollFundDonationProducts.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollFundDonationProducts.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // frmFundRaiserAndDonation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.bigVerticalScrollFundDonationProducts);
            this.Controls.Add(this.btnNoThanks);
            this.Controls.Add(this.tlpFundDonationProducts);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblGreeting1);
            this.Controls.Add(this.txtMessage);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Bango Pro", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmFundRaiserAndDonation";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmChooseProduct";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDonationAndFundRaiser_FormClosed);
            this.Load += new System.EventHandler(this.frmFundRaiserAndDonation_Load);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.lblGreeting1, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.tlpFundDonationProducts, 0);
            this.Controls.SetChildIndex(this.btnNoThanks, 0);
            this.Controls.SetChildIndex(this.bigVerticalScrollFundDonationProducts, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.tlpFundDonationProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbFundDonationLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.Label lblGreeting1;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TableLayoutPanel tlpFundDonationProducts;
        private System.Windows.Forms.PictureBox pbFundDonationLogo;
        private System.Windows.Forms.Button btnNoThanks;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollFundDonationProducts;
    }
}