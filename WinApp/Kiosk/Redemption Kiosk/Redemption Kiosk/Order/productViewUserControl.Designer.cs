namespace Redemption_Kiosk
{
    partial class productViewUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbProductImage = new System.Windows.Forms.PictureBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.btnInfo = new System.Windows.Forms.Button();
            this.lblTickets = new System.Windows.Forms.Label();
            this.lblProductTicket = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblProductCount = new System.Windows.Forms.Label();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.lblTicketCount = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbProductImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbProductImage
            // 
            this.pbProductImage.BackColor = System.Drawing.Color.Transparent;
            this.pbProductImage.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.default_product_image;
            this.pbProductImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbProductImage.Location = new System.Drawing.Point(9, 41);
            this.pbProductImage.Name = "pbProductImage";
            this.pbProductImage.Size = new System.Drawing.Size(192, 87);
            this.pbProductImage.TabIndex = 0;
            this.pbProductImage.TabStop = false;
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.BackColor = System.Drawing.Color.Transparent;
            this.lblProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblProductName.Location = new System.Drawing.Point(50, 9);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(79, 29);
            this.lblProductName.TabIndex = 1;
            this.lblProductName.Text = "label1";
            // 
            // btnInfo
            // 
            this.btnInfo.BackColor = System.Drawing.Color.Transparent;
            this.btnInfo.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Info_Icon1;
            this.btnInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnInfo.FlatAppearance.BorderSize = 0;
            this.btnInfo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnInfo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInfo.Location = new System.Drawing.Point(9, 9);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(36, 29);
            this.btnInfo.TabIndex = 2;
            this.btnInfo.UseVisualStyleBackColor = false;
            this.btnInfo.Click += new System.EventHandler(this.BtnInfo_Click);
            // 
            // lblTickets
            // 
            this.lblTickets.AutoSize = true;
            this.lblTickets.BackColor = System.Drawing.Color.Transparent;
            this.lblTickets.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTickets.ForeColor = System.Drawing.Color.Red;
            this.lblTickets.Location = new System.Drawing.Point(15, 129);
            this.lblTickets.Name = "lblTickets";
            this.lblTickets.Size = new System.Drawing.Size(128, 29);
            this.lblTickets.TabIndex = 3;
            this.lblTickets.Text = "TICKETS :";
            // 
            // lblProductTicket
            // 
            this.lblProductTicket.AutoSize = true;
            this.lblProductTicket.BackColor = System.Drawing.Color.Transparent;
            this.lblProductTicket.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductTicket.ForeColor = System.Drawing.Color.Red;
            this.lblProductTicket.Location = new System.Drawing.Point(149, 129);
            this.lblProductTicket.Name = "lblProductTicket";
            this.lblProductTicket.Size = new System.Drawing.Size(52, 29);
            this.lblProductTicket.TabIndex = 4;
            this.lblProductTicket.Text = "100";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Panel_AddProduct;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.lblProductCount);
            this.panel1.Controls.Add(this.btnMinus);
            this.panel1.Controls.Add(this.btnPlus);
            this.panel1.Location = new System.Drawing.Point(391, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(63, 147);
            this.panel1.TabIndex = 5;
            // 
            // lblProductCount
            // 
            this.lblProductCount.BackColor = System.Drawing.Color.Transparent;
            this.lblProductCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblProductCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductCount.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblProductCount.Location = new System.Drawing.Point(3, 57);
            this.lblProductCount.Name = "lblProductCount";
            this.lblProductCount.Size = new System.Drawing.Size(57, 39);
            this.lblProductCount.TabIndex = 2;
            this.lblProductCount.Text = "100";
            this.lblProductCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMinus
            // 
            this.btnMinus.BackColor = System.Drawing.Color.Transparent;
            this.btnMinus.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Minus_Btn;
            this.btnMinus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMinus.FlatAppearance.BorderSize = 0;
            this.btnMinus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMinus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinus.Location = new System.Drawing.Point(9, 99);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(46, 42);
            this.btnMinus.TabIndex = 1;
            this.btnMinus.UseVisualStyleBackColor = false;
            this.btnMinus.Click += new System.EventHandler(this.BtnMinus_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.BackColor = System.Drawing.Color.Transparent;
            this.btnPlus.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Plus_Btn;
            this.btnPlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPlus.FlatAppearance.BorderSize = 0;
            this.btnPlus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPlus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlus.Location = new System.Drawing.Point(9, 12);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(46, 42);
            this.btnPlus.TabIndex = 0;
            this.btnPlus.UseVisualStyleBackColor = false;
            this.btnPlus.Click += new System.EventHandler(this.BtnPlus_Click);
            // 
            // lblTicketCount
            // 
            this.lblTicketCount.AutoSize = true;
            this.lblTicketCount.BackColor = System.Drawing.Color.Transparent;
            this.lblTicketCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketCount.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblTicketCount.Location = new System.Drawing.Point(648, 68);
            this.lblTicketCount.Name = "lblTicketCount";
            this.lblTicketCount.Size = new System.Drawing.Size(66, 36);
            this.lblTicketCount.TabIndex = 6;
            this.lblTicketCount.Text = "200";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Close_Btn;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(921, 53);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 62);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // productViewUserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblTicketCount);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblProductTicket);
            this.Controls.Add(this.lblTickets);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.pbProductImage);
            this.Name = "productViewUserControl";
            this.Size = new System.Drawing.Size(1087, 158);
            this.Load += new System.EventHandler(this.ProductViewUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbProductImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbProductImage;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Label lblTickets;
        private System.Windows.Forms.Label lblProductTicket;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblProductCount;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Label lblTicketCount;
        private System.Windows.Forms.Button btnCancel;
    }
}
