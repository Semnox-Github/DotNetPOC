namespace Parafait_POS.Reservation
{
    partial class usrCtrlProductDetails
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
            this.components = new System.ComponentModel.Container();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.pcbDecreaseQty = new System.Windows.Forms.PictureBox();
            this.pcbIncreaseQty = new System.Windows.Forms.PictureBox();
            this.cbxSelectedProduct = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.btnLineMenu = new System.Windows.Forms.Button();
            this.ctxLineMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.remarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyDiscountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyTransactionProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtInclusiveQty = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pcbDecreaseQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbIncreaseQty)).BeginInit();
            this.ctxLineMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtProductName
            // 
            this.txtProductName.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductName.Location = new System.Drawing.Point(30, 4);
            this.txtProductName.MaxLength = 50;
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.ReadOnly = true;
            this.txtProductName.Size = new System.Drawing.Size(226, 30);
            this.txtProductName.TabIndex = 69;
            // 
            // txtPrice
            // 
            this.txtPrice.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrice.Location = new System.Drawing.Point(262, 4);
            this.txtPrice.MaxLength = 50;
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.ReadOnly = true;
            this.txtPrice.Size = new System.Drawing.Size(102, 30);
            this.txtPrice.TabIndex = 70;
            // 
            // txtQty
            // 
            this.txtQty.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQty.Location = new System.Drawing.Point(406, 4);
            this.txtQty.MaxLength = 50;
            this.txtQty.Name = "txtQty";
            this.txtQty.ReadOnly = true;
            this.txtQty.Size = new System.Drawing.Size(102, 30);
            this.txtQty.TabIndex = 71;
            this.txtQty.Enter += new System.EventHandler(this.txtQty_Enter);
            this.txtQty.Validating += new System.ComponentModel.CancelEventHandler(this.txtQty_Validating);
            // 
            // pcbDecreaseQty
            // 
            this.pcbDecreaseQty.Image = global::Parafait_POS.Properties.Resources.R_Decrease_Qty_Normal;
            this.pcbDecreaseQty.Location = new System.Drawing.Point(370, 4);
            this.pcbDecreaseQty.Name = "pcbDecreaseQty";
            this.pcbDecreaseQty.Size = new System.Drawing.Size(30, 30);
            this.pcbDecreaseQty.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbDecreaseQty.TabIndex = 73;
            this.pcbDecreaseQty.TabStop = false;
            this.pcbDecreaseQty.Click += new System.EventHandler(this.pcbDecreaseQty_Click);
            this.pcbDecreaseQty.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pcbDecreaseQty_MouseDown);
            this.pcbDecreaseQty.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pcbDecreaseQty_MouseUp);
            // 
            // pcbIncreaseQty
            // 
            this.pcbIncreaseQty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pcbIncreaseQty.Image = global::Parafait_POS.Properties.Resources.R_Increase_Qty_Normal;
            this.pcbIncreaseQty.Location = new System.Drawing.Point(514, 4);
            this.pcbIncreaseQty.Name = "pcbIncreaseQty";
            this.pcbIncreaseQty.Size = new System.Drawing.Size(30, 30);
            this.pcbIncreaseQty.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbIncreaseQty.TabIndex = 72;
            this.pcbIncreaseQty.TabStop = false;
            this.pcbIncreaseQty.Click += new System.EventHandler(this.pcbIncreaseQty_Click);
            this.pcbIncreaseQty.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pcbIncreaseQty_MouseDown);
            this.pcbIncreaseQty.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pcbIncreaseQty_MouseUp);
            // 
            // cbxSelectedProduct
            // 
            this.cbxSelectedProduct.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxSelectedProduct.AutoSize = true;
            this.cbxSelectedProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxSelectedProduct.FlatAppearance.BorderSize = 0;
            this.cbxSelectedProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxSelectedProduct.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxSelectedProduct.ImageIndex = 1;
            this.cbxSelectedProduct.Location = new System.Drawing.Point(0, 6);
            this.cbxSelectedProduct.Name = "cbxSelectedProduct";
            this.cbxSelectedProduct.Size = new System.Drawing.Size(26, 26);
            this.cbxSelectedProduct.TabIndex = 77;
            this.cbxSelectedProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxSelectedProduct.UseVisualStyleBackColor = true;
            this.cbxSelectedProduct.CheckedChanged += new System.EventHandler(this.cbxSelectedProduct_CheckedChanged);
            // 
            // btnLineMenu
            // 
            this.btnLineMenu.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnLineMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLineMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLineMenu.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLineMenu.ForeColor = System.Drawing.Color.White;
            this.btnLineMenu.Location = new System.Drawing.Point(564, 4);
            this.btnLineMenu.Name = "btnLineMenu";
            this.btnLineMenu.Size = new System.Drawing.Size(30, 30);
            this.btnLineMenu.TabIndex = 74;
            this.btnLineMenu.TabStop = false;
            this.btnLineMenu.Text = "...";
            this.btnLineMenu.Click += new System.EventHandler(this.btnRemarks_Click);
            this.btnLineMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLineMenu_MouseDown);
            this.btnLineMenu.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLineMenu_MouseUp);
            // 
            // ctxLineMenu
            // 
            this.ctxLineMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remarksToolStripMenuItem,
            this.detailsToolStripMenuItem,
            this.applyDiscountToolStripMenuItem,
            this.applyTransactionProfileToolStripMenuItem});
            this.ctxLineMenu.Name = "ctxLineMenu";
            this.ctxLineMenu.Size = new System.Drawing.Size(207, 92);
            this.ctxLineMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ctxLineMenu_ItemClicked);
            // 
            // remarksToolStripMenuItem
            // 
            this.remarksToolStripMenuItem.Name = "remarksToolStripMenuItem";
            this.remarksToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.remarksToolStripMenuItem.Text = "Remarks";
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.detailsToolStripMenuItem.Text = "Details";
            // 
            // applyDiscountToolStripMenuItem
            // 
            this.applyDiscountToolStripMenuItem.Name = "applyDiscountToolStripMenuItem";
            this.applyDiscountToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.applyDiscountToolStripMenuItem.Text = "Apply Discount";
            // 
            // applyTransactionProfileToolStripMenuItem
            // 
            this.applyTransactionProfileToolStripMenuItem.Name = "applyTransactionProfileToolStripMenuItem";
            this.applyTransactionProfileToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.applyTransactionProfileToolStripMenuItem.Text = "Apply Transaction Profile";
            // 
            // txtInclusiveQty
            // 
            this.txtInclusiveQty.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInclusiveQty.Location = new System.Drawing.Point(600, 4);
            this.txtInclusiveQty.MaxLength = 50;
            this.txtInclusiveQty.Name = "txtInclusiveQty";
            this.txtInclusiveQty.ReadOnly = true;
            this.txtInclusiveQty.Size = new System.Drawing.Size(102, 30);
            this.txtInclusiveQty.TabIndex = 78;
            this.txtInclusiveQty.Visible = false;
            // 
            // usrCtrlProductDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.txtInclusiveQty);
            this.Controls.Add(this.cbxSelectedProduct);
            this.Controls.Add(this.btnLineMenu);
            this.Controls.Add(this.pcbDecreaseQty);
            this.Controls.Add(this.pcbIncreaseQty);
            this.Controls.Add(this.txtQty);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.txtProductName);
            this.Name = "usrCtrlProductDetails";
            this.Size = new System.Drawing.Size(748, 40);
            ((System.ComponentModel.ISupportInitialize)(this.pcbDecreaseQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbIncreaseQty)).EndInit();
            this.ctxLineMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.PictureBox pcbIncreaseQty;
        private System.Windows.Forms.PictureBox pcbDecreaseQty;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxSelectedProduct;
        private System.Windows.Forms.Button btnLineMenu;
        private System.Windows.Forms.ContextMenuStrip ctxLineMenu;
        private System.Windows.Forms.ToolStripMenuItem remarksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyDiscountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyTransactionProfileToolStripMenuItem;
        private System.Windows.Forms.TextBox txtInclusiveQty;
    }
}
