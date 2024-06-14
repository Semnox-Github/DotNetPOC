namespace Redemption_Kiosk
{
    partial class ProductUserControl
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
            this.lblProductName = new System.Windows.Forms.Label();
            this.lblTicketCount = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.pbProductImage = new System.Windows.Forms.PictureBox();
            this.panelAddProduct = new System.Windows.Forms.Panel();
            this.lblProductCount = new System.Windows.Forms.Label();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btnInfo = new System.Windows.Forms.Button();
            this.lblTicketLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbProductImage)).BeginInit();
            this.panelAddProduct.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblProductName
            // 
            this.lblProductName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProductName.AutoEllipsis = true;
            this.lblProductName.BackColor = System.Drawing.Color.Transparent;
            this.lblProductName.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName.ForeColor = System.Drawing.Color.White;
            this.lblProductName.Location = new System.Drawing.Point(42, 17);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(233, 29);
            this.lblProductName.TabIndex = 0;
            // 
            // lblTicketCount
            // 
            this.lblTicketCount.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblTicketCount.AutoSize = true;
            this.lblTicketCount.BackColor = System.Drawing.Color.White;
            this.lblTicketCount.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketCount.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblTicketCount.Location = new System.Drawing.Point(140, 368);
            this.lblTicketCount.Name = "lblTicketCount";
            this.lblTicketCount.Size = new System.Drawing.Size(0, 32);
            this.lblTicketCount.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAdd.BackColor = System.Drawing.Color.White;
            this.btnAdd.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Product_Add_Btn;
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(243, 362);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(93, 46);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Tag = "Add";
            this.btnAdd.Text = "Update";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // pbProductImage
            // 
            this.pbProductImage.BackColor = System.Drawing.Color.White;
            this.pbProductImage.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.default_product_image;
            this.pbProductImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbProductImage.Location = new System.Drawing.Point(30, 84);
            this.pbProductImage.Name = "pbProductImage";
            this.pbProductImage.Size = new System.Drawing.Size(203, 221);
            this.pbProductImage.TabIndex = 4;
            this.pbProductImage.TabStop = false;
            // 
            // panelAddProduct
            // 
            this.panelAddProduct.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panelAddProduct.BackColor = System.Drawing.Color.Transparent;
            this.panelAddProduct.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Panel_AddProduct;
            this.panelAddProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelAddProduct.Controls.Add(this.lblProductCount);
            this.panelAddProduct.Controls.Add(this.btnMinus);
            this.panelAddProduct.Controls.Add(this.btnPlus);
            this.panelAddProduct.Location = new System.Drawing.Point(252, 99);
            this.panelAddProduct.Name = "panelAddProduct";
            this.panelAddProduct.Size = new System.Drawing.Size(84, 192);
            this.panelAddProduct.TabIndex = 5;
            // 
            // lblProductCount
            // 
            this.lblProductCount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProductCount.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductCount.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblProductCount.Location = new System.Drawing.Point(3, 68);
            this.lblProductCount.Name = "lblProductCount";
            this.lblProductCount.Size = new System.Drawing.Size(78, 49);
            this.lblProductCount.TabIndex = 2;
            this.lblProductCount.Text = "0";
            this.lblProductCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMinus
            // 
            this.btnMinus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMinus.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Minus_Btn;
            this.btnMinus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMinus.FlatAppearance.BorderSize = 0;
            this.btnMinus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMinus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinus.ForeColor = System.Drawing.Color.White;
            this.btnMinus.Location = new System.Drawing.Point(20, 120);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(42, 50);
            this.btnMinus.TabIndex = 1;
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.BtnMinus_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPlus.BackColor = System.Drawing.Color.Transparent;
            this.btnPlus.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Plus_Btn;
            this.btnPlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPlus.FlatAppearance.BorderSize = 0;
            this.btnPlus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPlus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlus.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlus.ForeColor = System.Drawing.Color.White;
            this.btnPlus.Location = new System.Drawing.Point(19, 23);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(44, 44);
            this.btnPlus.TabIndex = 0;
            this.btnPlus.UseVisualStyleBackColor = false;
            this.btnPlus.Click += new System.EventHandler(this.BtnPlus_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnInfo.BackColor = System.Drawing.Color.Transparent;
            this.btnInfo.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Info_Icon;
            this.btnInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnInfo.FlatAppearance.BorderSize = 0;
            this.btnInfo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnInfo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInfo.ForeColor = System.Drawing.Color.Blue;
            this.btnInfo.Location = new System.Drawing.Point(259, 5);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(70, 47);
            this.btnInfo.TabIndex = 6;
            this.btnInfo.UseVisualStyleBackColor = false;
            this.btnInfo.Click += new System.EventHandler(this.BtnInfo_Click);
            // 
            // lblTicketLabel
            // 
            this.lblTicketLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblTicketLabel.AutoSize = true;
            this.lblTicketLabel.BackColor = System.Drawing.Color.White;
            this.lblTicketLabel.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketLabel.ForeColor = System.Drawing.Color.Black;
            this.lblTicketLabel.Location = new System.Drawing.Point(24, 368);
            this.lblTicketLabel.Name = "lblTicketLabel";
            this.lblTicketLabel.Size = new System.Drawing.Size(124, 32);
            this.lblTicketLabel.TabIndex = 7;
            this.lblTicketLabel.Text = "Tickets :";
            // 
            // ProductUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Product_box_normal;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.lblTicketCount);
            this.Controls.Add(this.lblTicketLabel);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.panelAddProduct);
            this.Controls.Add(this.pbProductImage);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblProductName);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximumSize = new System.Drawing.Size(350, 423);
            this.MinimumSize = new System.Drawing.Size(350, 423);
            this.Name = "ProductUserControl";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(350, 423);
            this.Load += new System.EventHandler(this.ProductUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbProductImage)).EndInit();
            this.panelAddProduct.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label lblTicketCount;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.PictureBox pbProductImage;
        private System.Windows.Forms.Panel panelAddProduct;
        private System.Windows.Forms.Label lblProductCount;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Label lblTicketLabel;
    }
}
