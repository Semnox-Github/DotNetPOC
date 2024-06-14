
namespace Semnox.Parafait.KioskCore
{
    partial class BtnKioskCart
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
            this.lblItemQty = new System.Windows.Forms.Label();
            this.lblCartText = new System.Windows.Forms.Label();
            this.pnlKioskCart = new System.Windows.Forms.Panel();
            this.pnlKioskCart.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblItemQty
            // 
            this.lblItemQty.AutoEllipsis = true;
            this.lblItemQty.BackColor = System.Drawing.Color.Transparent;
            this.lblItemQty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblItemQty.Font = new System.Drawing.Font("Gotham Rounded Bold", 17F);
            this.lblItemQty.ForeColor = System.Drawing.Color.White;
            this.lblItemQty.Location = new System.Drawing.Point(31, 47);
            this.lblItemQty.Margin = new System.Windows.Forms.Padding(0);
            this.lblItemQty.Name = "lblItemQty";
            this.lblItemQty.Size = new System.Drawing.Size(99, 34);
            this.lblItemQty.TabIndex = 1;
            this.lblItemQty.Text = "99+";
            this.lblItemQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblItemQty.Click += new System.EventHandler(this.btnCart_Click);
            // 
            // lblCartText
            // 
            this.lblCartText.BackColor = System.Drawing.Color.Transparent;
            this.lblCartText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCartText.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCartText.ForeColor = System.Drawing.Color.White;
            this.lblCartText.Location = new System.Drawing.Point(5, 105);
            this.lblCartText.Margin = new System.Windows.Forms.Padding(0);
            this.lblCartText.Name = "lblCartText";
            this.lblCartText.Size = new System.Drawing.Size(153, 42);
            this.lblCartText.TabIndex = 0;
            this.lblCartText.Text = "CART";
            this.lblCartText.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblCartText.Click += new System.EventHandler(this.btnCart_Click);
            // 
            // pnlKioskCart
            // 
            this.pnlKioskCart.Controls.Add(this.lblItemQty);
            this.pnlKioskCart.Controls.Add(this.lblCartText);
            this.pnlKioskCart.Location = new System.Drawing.Point(0, 0);
            this.pnlKioskCart.Margin = new System.Windows.Forms.Padding(0);
            this.pnlKioskCart.Name = "pnlKioskCart";
            this.pnlKioskCart.Size = new System.Drawing.Size(153, 151);
            this.pnlKioskCart.TabIndex = 2;
            this.pnlKioskCart.Click += new System.EventHandler(this.btnCart_Click);
            // 
            // BtnKioskCart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::Semnox.Parafait.KioskCore.Properties.Resources.KioskCartIcon;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.pnlKioskCart);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BtnKioskCart";
            this.Click += new System.EventHandler(this.btnCart_Click);
            this.pnlKioskCart.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblCartText;
        private System.Windows.Forms.Label lblItemQty;
        //private System.Windows.Forms.Timer glowTimer;
        private System.Windows.Forms.Panel pnlKioskCart;
    }
}
