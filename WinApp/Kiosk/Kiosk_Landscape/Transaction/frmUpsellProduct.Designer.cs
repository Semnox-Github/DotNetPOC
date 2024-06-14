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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpsellProduct));
            this.lblSiteName = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.exitTimer = new System.Windows.Forms.Timer(this.components);
            this.txtMessage = new System.Windows.Forms.Button();
            this.lblGreeting1 = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.lblOffer = new System.Windows.Forms.Label();
            this.lblTimeOut = new System.Windows.Forms.Button();
            this.btnSelectedProd = new System.Windows.Forms.Button();
            this.btnSelectedProdDesc = new System.Windows.Forms.Button();
            this.btnUpsellProduct = new System.Windows.Forms.Button();
            this.btnUpsellProductDesc = new System.Windows.Forms.Button();
            this.panelSelected = new System.Windows.Forms.Panel();
            this.panelOffer = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            //this.btnHome = new System.Windows.Forms.Button();
            this.panelSelected.SuspendLayout();
            this.panelOffer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSiteName
            // 
            this.lblSiteName.BackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSiteName.FlatAppearance.BorderSize = 0;
            this.lblSiteName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSiteName.Font = new System.Drawing.Font("Verdana", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(0, 0);
            this.lblSiteName.Margin = new System.Windows.Forms.Padding(6);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1276, 82);
            this.lblSiteName.TabIndex = 137;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            this.lblSiteName.Visible = false;
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNo.BackColor = System.Drawing.Color.Transparent;
            this.btnNo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNo.BackgroundImage")));
            this.btnNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNo.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.Location = new System.Drawing.Point(180, 854);
            this.btnNo.Margin = new System.Windows.Forms.Padding(6);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(268, 84);
            this.btnNo.TabIndex = 138;
            this.btnNo.Text = "No Thanks!";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            this.btnNo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNo_MouseDown);
            this.btnNo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnNo_MouseUp);
            // 
            // exitTimer
            // 
            //this.exitTimer.Tick += new System.EventHandler(this.exitTimer_Tick);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.bottom_bar;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F);
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 957);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1276, 40);
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
            this.lblGreeting1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.White;
            this.lblGreeting1.Location = new System.Drawing.Point(58, 144);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(1185, 76);
            this.lblGreeting1.TabIndex = 149;
            this.lblGreeting1.Text = "You have selected $20 Top up";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYes.BackColor = System.Drawing.Color.Transparent;
            this.btnYes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnYes.BackgroundImage")));
            this.btnYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnYes.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.Location = new System.Drawing.Point(838, 854);
            this.btnYes.Margin = new System.Windows.Forms.Padding(6);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(268, 84);
            this.btnYes.TabIndex = 150;
            this.btnYes.Text = "Yes Please!";
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
            this.lblOffer.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOffer.ForeColor = System.Drawing.Color.White;
            this.lblOffer.Location = new System.Drawing.Point(12, 214);
            this.lblOffer.Name = "lblOffer";
            this.lblOffer.Size = new System.Drawing.Size(1256, 62);
            this.lblOffer.TabIndex = 151;
            this.lblOffer.Text = "We Have a Special Offer for you";
            this.lblOffer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblOffer.Visible = false;
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
            this.lblTimeOut.Font = new System.Drawing.Font("Verdana", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOut.ForeColor = System.Drawing.Color.White;
            this.lblTimeOut.Location = new System.Drawing.Point(1126, 28);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(142, 110);
            this.lblTimeOut.TabIndex = 153;
            this.lblTimeOut.UseVisualStyleBackColor = false;
            // 
            // btnSelectedProd
            // 
            this.btnSelectedProd.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectedProd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSelectedProd.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnSelectedProd.FlatAppearance.BorderSize = 0;
            this.btnSelectedProd.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSelectedProd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSelectedProd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSelectedProd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectedProd.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectedProd.ForeColor = System.Drawing.Color.White;
            this.btnSelectedProd.Location = new System.Drawing.Point(6, 9);
            this.btnSelectedProd.Margin = new System.Windows.Forms.Padding(6);
            this.btnSelectedProd.Name = "btnSelectedProd";
            this.btnSelectedProd.Size = new System.Drawing.Size(221, 77);
            this.btnSelectedProd.TabIndex = 154;
            this.btnSelectedProd.Text = "$100";
            this.btnSelectedProd.UseVisualStyleBackColor = false;
            // 
            // btnSelectedProdDesc
            // 
            this.btnSelectedProdDesc.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectedProdDesc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSelectedProdDesc.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnSelectedProdDesc.FlatAppearance.BorderSize = 0;
            this.btnSelectedProdDesc.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSelectedProdDesc.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSelectedProdDesc.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSelectedProdDesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectedProdDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectedProdDesc.ForeColor = System.Drawing.Color.White;
            this.btnSelectedProdDesc.Location = new System.Drawing.Point(6, 98);
            this.btnSelectedProdDesc.Margin = new System.Windows.Forms.Padding(6);
            this.btnSelectedProdDesc.Name = "btnSelectedProdDesc";
            this.btnSelectedProdDesc.Size = new System.Drawing.Size(221, 132);
            this.btnSelectedProdDesc.TabIndex = 155;
            this.btnSelectedProdDesc.Text = "Sample Product";
            this.btnSelectedProdDesc.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSelectedProdDesc.UseVisualStyleBackColor = false;
            // 
            // btnUpsellProduct
            // 
            this.btnUpsellProduct.BackColor = System.Drawing.Color.Transparent;
            this.btnUpsellProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUpsellProduct.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnUpsellProduct.FlatAppearance.BorderSize = 0;
            this.btnUpsellProduct.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnUpsellProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnUpsellProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnUpsellProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpsellProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpsellProduct.ForeColor = System.Drawing.Color.White;
            this.btnUpsellProduct.Location = new System.Drawing.Point(6, 9);
            this.btnUpsellProduct.Margin = new System.Windows.Forms.Padding(6);
            this.btnUpsellProduct.Name = "btnUpsellProduct";
            this.btnUpsellProduct.Size = new System.Drawing.Size(276, 77);
            this.btnUpsellProduct.TabIndex = 156;
            this.btnUpsellProduct.Text = "$100";
            this.btnUpsellProduct.UseVisualStyleBackColor = false;
            // 
            // btnUpsellProductDesc
            // 
            this.btnUpsellProductDesc.BackColor = System.Drawing.Color.Transparent;
            this.btnUpsellProductDesc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUpsellProductDesc.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnUpsellProductDesc.FlatAppearance.BorderSize = 0;
            this.btnUpsellProductDesc.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnUpsellProductDesc.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnUpsellProductDesc.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnUpsellProductDesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpsellProductDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpsellProductDesc.ForeColor = System.Drawing.Color.White;
            this.btnUpsellProductDesc.Location = new System.Drawing.Point(6, 98);
            this.btnUpsellProductDesc.Margin = new System.Windows.Forms.Padding(6);
            this.btnUpsellProductDesc.Name = "btnUpsellProductDesc";
            this.btnUpsellProductDesc.Size = new System.Drawing.Size(276, 152);
            this.btnUpsellProductDesc.TabIndex = 157;
            this.btnUpsellProductDesc.Text = "Sample Product";
            this.btnUpsellProductDesc.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUpsellProductDesc.UseVisualStyleBackColor = false;
            // 
            // panelSelected
            // 
            this.panelSelected.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelSelected.BackColor = System.Drawing.Color.Transparent;
            this.panelSelected.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.plain_product_button;
            this.panelSelected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelSelected.Controls.Add(this.btnSelectedProd);
            this.panelSelected.Controls.Add(this.btnSelectedProdDesc);
            this.panelSelected.Location = new System.Drawing.Point(913, 321);
            this.panelSelected.Name = "panelSelected";
            this.panelSelected.Size = new System.Drawing.Size(233, 236);
            this.panelSelected.TabIndex = 158;
            this.panelSelected.Visible = false;
            // 
            // panelOffer
            // 
            this.panelOffer.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelOffer.BackColor = System.Drawing.Color.Transparent;
            this.panelOffer.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.plain_product_button;
            this.panelOffer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelOffer.Controls.Add(this.btnUpsellProduct);
            this.panelOffer.Controls.Add(this.btnUpsellProductDesc);
            this.panelOffer.Location = new System.Drawing.Point(496, 384);
            this.panelOffer.Name = "panelOffer";
            this.panelOffer.Size = new System.Drawing.Size(288, 256);
            this.panelOffer.TabIndex = 159;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 41.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(6, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1262, 82);
            this.button1.TabIndex = 161;
            this.button1.Text = "Special Offer";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // btnHome
            // 
            //this.btnHome.BackColor = System.Drawing.Color.Transparent;
            this.btnHome.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.home_button;
            //this.btnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            //this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            //this.btnHome.FlatAppearance.BorderSize = 0;
            //this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            //this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            //this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            //this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.btnHome.ForeColor = System.Drawing.Color.White;
            //this.btnHome.Location = new System.Drawing.Point(31, 28);
            //this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(153, 151);
            this.btnHome.TabIndex = 162;
            //this.btnHome.Text = "GO HOME";
            //this.btnHome.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            //this.btnHome.UseVisualStyleBackColor = false;
            //this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // frmUpsellProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_Screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1276, 997);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.lblTimeOut);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panelOffer);
            this.Controls.Add(this.panelSelected);
            this.Controls.Add(this.lblOffer);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblGreeting1);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.lblSiteName);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmUpsellProduct";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Upsell Offers";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpsellProduct_FormClosing);
            this.Load += new System.EventHandler(this.frmRedeemTokens_Load);
            this.panelSelected.ResumeLayout(false);
            this.panelOffer.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnSelectedProd;
        private System.Windows.Forms.Button btnSelectedProdDesc;
        private System.Windows.Forms.Button btnUpsellProduct;
        private System.Windows.Forms.Button btnUpsellProductDesc;
        private System.Windows.Forms.Panel panelSelected;
        private System.Windows.Forms.Panel panelOffer;
        private System.Windows.Forms.Button button1;
        //private System.Windows.Forms.Button btnHome;
    }
}
