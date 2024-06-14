namespace Parafait_Kiosk
{
    partial class frmCardSaleOption
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
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.panelExistingCard = new System.Windows.Forms.Panel();
            this.pbCheckBoxExistingCard = new System.Windows.Forms.PictureBox();
            this.pbExistingcard = new System.Windows.Forms.PictureBox();
            this.lblNewCard = new System.Windows.Forms.Label();
            this.lblExistingCard = new System.Windows.Forms.Label();
            this.panelNewCard = new System.Windows.Forms.Panel();
            this.pbCheckBoxNewCard = new System.Windows.Forms.PictureBox();
            this.pbNewCard = new System.Windows.Forms.PictureBox();
            this.panelExistingCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheckBoxExistingCard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExistingcard)).BeginInit();
            this.panelNewCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheckBoxNewCard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewCard)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(12, 20);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(969, 116);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "New Card?";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancel.Location = new System.Drawing.Point(100, 450);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(365, 125);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConfirm.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfirm.Location = new System.Drawing.Point(520, 450);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(365, 125);
            this.btnConfirm.TabIndex = 15;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // panelExistingCard
            // 
            this.panelExistingCard.BackColor = System.Drawing.Color.Transparent;
            this.panelExistingCard.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.WhiteBackground;
            this.panelExistingCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelExistingCard.Controls.Add(this.pbCheckBoxExistingCard);
            this.panelExistingCard.Controls.Add(this.pbExistingcard);
            this.panelExistingCard.Controls.Add(this.lblNewCard);
            this.panelExistingCard.Location = new System.Drawing.Point(180, 140);
            this.panelExistingCard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelExistingCard.Name = "panelExistingCard";
            this.panelExistingCard.Size = new System.Drawing.Size(630, 125);
            this.panelExistingCard.TabIndex = 16;
            // 
            // pbCheckBoxExistingCard
            // 
            this.pbCheckBoxExistingCard.BackColor = System.Drawing.Color.Transparent;
            this.pbCheckBoxExistingCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbCheckBoxExistingCard.Image = global::Parafait_Kiosk.Properties.Resources.NewUnTickedCheckBox;
            this.pbCheckBoxExistingCard.Location = new System.Drawing.Point(550, 40);
            this.pbCheckBoxExistingCard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbCheckBoxExistingCard.Name = "pbCheckBoxExistingCard";
            this.pbCheckBoxExistingCard.Size = new System.Drawing.Size(50, 50);
            this.pbCheckBoxExistingCard.TabIndex = 1;
            this.pbCheckBoxExistingCard.TabStop = false;
            this.pbCheckBoxExistingCard.Click += new System.EventHandler(this.pbCheckBoxExistingCard_Click);
            // 
            // pbExistingcard
            // 
            this.pbExistingcard.BackColor = System.Drawing.Color.Transparent;
            this.pbExistingcard.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.AttractionExistingCard;
            this.pbExistingcard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbExistingcard.Location = new System.Drawing.Point(0, 0);
            this.pbExistingcard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbExistingcard.Name = "pbExistingcard";
            this.pbExistingcard.Size = new System.Drawing.Size(150, 125);
            this.pbExistingcard.TabIndex = 0;
            this.pbExistingcard.TabStop = false;
            // 
            // lblNewCard
            // 
            this.lblNewCard.BackColor = System.Drawing.Color.Transparent;
            this.lblNewCard.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewCard.ForeColor = System.Drawing.Color.Black;
            this.lblNewCard.Location = new System.Drawing.Point(45, 3);
            this.lblNewCard.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNewCard.Name = "lblNewCard";
            this.lblNewCard.Size = new System.Drawing.Size(500, 125);
            this.lblNewCard.TabIndex = 18;
            this.lblNewCard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNewCard.Click += new System.EventHandler(this.pbCheckBoxExistingCard_Click);
            // 
            // lblExistingCard
            // 
            this.lblExistingCard.BackColor = System.Drawing.Color.Transparent;
            this.lblExistingCard.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExistingCard.ForeColor = System.Drawing.Color.Black;
            this.lblExistingCard.Location = new System.Drawing.Point(70, 3);
            this.lblExistingCard.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExistingCard.Name = "lblExistingCard";
            this.lblExistingCard.Size = new System.Drawing.Size(500, 125);
            this.lblExistingCard.TabIndex = 18;
            this.lblExistingCard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblExistingCard.Click += new System.EventHandler(this.pbCheckBoxNewCard_Click);
            // 
            // panelNewCard
            // 
            this.panelNewCard.BackColor = System.Drawing.Color.Transparent;
            this.panelNewCard.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.WhiteBackground;
            this.panelNewCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelNewCard.Controls.Add(this.pbCheckBoxNewCard);
            this.panelNewCard.Controls.Add(this.pbNewCard);
            this.panelNewCard.Controls.Add(this.lblExistingCard);
            this.panelNewCard.Location = new System.Drawing.Point(180, 290);
            this.panelNewCard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelNewCard.Name = "panelNewCard";
            this.panelNewCard.Size = new System.Drawing.Size(630, 125);
            this.panelNewCard.TabIndex = 16;
            // 
            // pbCheckBoxNewCard
            // 
            this.pbCheckBoxNewCard.BackColor = System.Drawing.Color.Transparent;
            this.pbCheckBoxNewCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbCheckBoxNewCard.Image = global::Parafait_Kiosk.Properties.Resources.NewUnTickedCheckBox;
            this.pbCheckBoxNewCard.Location = new System.Drawing.Point(550, 40);
            this.pbCheckBoxNewCard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbCheckBoxNewCard.Name = "pbCheckBoxNewCard";
            this.pbCheckBoxNewCard.Size = new System.Drawing.Size(50, 50);
            this.pbCheckBoxNewCard.TabIndex = 1;
            this.pbCheckBoxNewCard.TabStop = false;
            this.pbCheckBoxNewCard.Click += new System.EventHandler(this.pbCheckBoxNewCard_Click);
            // 
            // pbNewCard
            // 
            this.pbNewCard.BackColor = System.Drawing.Color.Transparent;
            this.pbNewCard.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.AttractionNewCard;
            this.pbNewCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbNewCard.Location = new System.Drawing.Point(0, 0);
            this.pbNewCard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbNewCard.Name = "pbNewCard";
            this.pbNewCard.Size = new System.Drawing.Size(150, 125);
            this.pbNewCard.TabIndex = 0;
            this.pbNewCard.TabStop = false;
            // 
            // frmCardSaleOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(993, 625);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.panelExistingCard);
            this.Controls.Add(this.panelNewCard);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCardSaleOption";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTicketOption";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCardSaleOption_FormClosed);
            this.Load += new System.EventHandler(this.frmCardSaleOption_Load);
            this.panelExistingCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCheckBoxExistingCard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExistingcard)).EndInit();
            this.panelNewCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCheckBoxNewCard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewCard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Panel panelExistingCard;
        private System.Windows.Forms.Panel panelNewCard;
        private System.Windows.Forms.PictureBox pbExistingcard;
        private System.Windows.Forms.PictureBox pbNewCard;
        private System.Windows.Forms.Label lblNewCard;
        private System.Windows.Forms.Label lblExistingCard;
        private System.Windows.Forms.PictureBox pbCheckBoxExistingCard;
        private System.Windows.Forms.PictureBox pbCheckBoxNewCard;
    }
}