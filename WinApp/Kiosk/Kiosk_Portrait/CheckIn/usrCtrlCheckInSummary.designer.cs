using System;

namespace Parafait_Kiosk
{
    partial class UsrCtrlCheckInSummary
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panelUsrCtrl = new System.Windows.Forms.Panel();
            this.lblDiscountPerc = new System.Windows.Forms.Label();
            this.lblTax = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblPackage = new System.Windows.Forms.Label();
            this.panelUsrCtrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // panelUsrCtrl
            // 
            this.panelUsrCtrl.AutoSize = true;
            this.panelUsrCtrl.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.TablePurchaseSummary;
            this.panelUsrCtrl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUsrCtrl.Controls.Add(this.lblDiscountPerc);
            this.panelUsrCtrl.Controls.Add(this.lblTax);
            this.panelUsrCtrl.Controls.Add(this.lblPrice);
            this.panelUsrCtrl.Controls.Add(this.lblTotal);
            this.panelUsrCtrl.Controls.Add(this.lblQuantity);
            this.panelUsrCtrl.Controls.Add(this.lblPackage);
            this.panelUsrCtrl.ForeColor = System.Drawing.Color.Black;
            this.panelUsrCtrl.Location = new System.Drawing.Point(0, 3);
            this.panelUsrCtrl.MaximumSize = new System.Drawing.Size(1013, 0);
            this.panelUsrCtrl.Name = "panelUsrCtrl";
            this.panelUsrCtrl.Size = new System.Drawing.Size(1015, 54);
            this.panelUsrCtrl.TabIndex = 1063;
            // 
            // lblDiscPerc
            // 
            this.lblDiscountPerc.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscountPerc.ForeColor = System.Drawing.Color.Thistle;
            this.lblDiscountPerc.Location = new System.Drawing.Point(259, 9);
            this.lblDiscountPerc.Name = "lblDiscPerc";
            this.lblDiscountPerc.Size = new System.Drawing.Size(98, 42);
            this.lblDiscountPerc.TabIndex = 6;
            this.lblDiscountPerc.Text = "000";
            // 
            // lblTax
            // 
            this.lblTax.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTax.ForeColor = System.Drawing.Color.Thistle;
            this.lblTax.Location = new System.Drawing.Point(763, 8);
            this.lblTax.Name = "lblTax";
            this.lblTax.Size = new System.Drawing.Size(104, 45);
            this.lblTax.TabIndex = 5;
            this.lblTax.Text = "00.00";
            // 
            // lblPrice
            // 
            this.lblPrice.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.ForeColor = System.Drawing.Color.Thistle;
            this.lblPrice.Location = new System.Drawing.Point(592, 9);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(110, 42);
            this.lblPrice.TabIndex = 4;
            this.lblPrice.Text = "00.00";
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.Thistle;
            this.lblTotal.Location = new System.Drawing.Point(885, 7);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(137, 45);
            this.lblTotal.TabIndex = 3;
            this.lblTotal.Text = "00.00";
            // 
            // lblQuantity
            // 
            this.lblQuantity.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantity.ForeColor = System.Drawing.Color.Thistle;
            this.lblQuantity.Location = new System.Drawing.Point(455, 9);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(110, 42);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "00";
            // 
            // lblPackage
            // 
            this.lblPackage.AutoEllipsis = true;
            this.lblPackage.AutoSize = true;
            this.lblPackage.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackage.ForeColor = System.Drawing.Color.Thistle;
            this.lblPackage.Location = new System.Drawing.Point(44, 6);
            this.lblPackage.MaximumSize = new System.Drawing.Size(200, 0);
            this.lblPackage.Name = "lblPackage";
            this.lblPackage.Size = new System.Drawing.Size(111, 39);
            this.lblPackage.TabIndex = 0;
            this.lblPackage.Text = "Name";
            // 
            // usrCtrlCheckInSummary
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.panelUsrCtrl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "usrCtrlCheckInSummary";
            this.Size = new System.Drawing.Size(1016, 65);
            this.panelUsrCtrl.ResumeLayout(false);
            this.panelUsrCtrl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Panel panelUsrCtrl;
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblPackage;
        private System.Windows.Forms.Label lblDiscountPerc;
    }
}
