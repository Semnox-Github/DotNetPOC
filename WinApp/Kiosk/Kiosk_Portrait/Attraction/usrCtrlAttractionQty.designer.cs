using System;

namespace Parafait_Kiosk
{
    partial class usrCtrlAttractionQty
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
            this.pcbIncreaseQty = new System.Windows.Forms.PictureBox();
            this.pcbDecreaseQty = new System.Windows.Forms.PictureBox();
            this.usrControlPanel = new System.Windows.Forms.Panel();
            this.btnSampleProduct = new System.Windows.Forms.Button();
            this.panelAgeCriteria = new System.Windows.Forms.Panel();
            this.lblAgeCriteriaHeader = new System.Windows.Forms.Label();
            this.lblAgeCriteria = new System.Windows.Forms.Label();
            this.pnlQtyTxtBox = new System.Windows.Forms.Panel();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.pnlQty = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pcbIncreaseQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbDecreaseQty)).BeginInit();
            this.usrControlPanel.SuspendLayout();
            this.panelAgeCriteria.SuspendLayout();
            this.pnlQtyTxtBox.SuspendLayout();
            this.pnlQty.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // pcbIncreaseQty
            // 
            this.pcbIncreaseQty.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.IncreaseQty;
            this.pcbIncreaseQty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcbIncreaseQty.Location = new System.Drawing.Point(271, 6);
            this.pcbIncreaseQty.Name = "pcbIncreaseQty";
            this.pcbIncreaseQty.Size = new System.Drawing.Size(116, 116);
            this.pcbIncreaseQty.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbIncreaseQty.TabIndex = 75;
            this.pcbIncreaseQty.TabStop = false;
            this.pcbIncreaseQty.Click += new System.EventHandler(this.pcbIncreaseQty_Click);
            // 
            // pcbDecreaseQty
            // 
            this.pcbDecreaseQty.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.DecreaseQty;
            this.pcbDecreaseQty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcbDecreaseQty.Location = new System.Drawing.Point(3, 6);
            this.pcbDecreaseQty.Name = "pcbDecreaseQty";
            this.pcbDecreaseQty.Size = new System.Drawing.Size(116, 116);
            this.pcbDecreaseQty.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pcbDecreaseQty.TabIndex = 76;
            this.pcbDecreaseQty.TabStop = false;
            this.pcbDecreaseQty.Click += new System.EventHandler(this.pcbDecreaseQty_Click);
            // 
            // usrControlPanel
            // 
            this.usrControlPanel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Button1;
            this.usrControlPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.usrControlPanel.Controls.Add(this.btnSampleProduct);
            this.usrControlPanel.Location = new System.Drawing.Point(-11, 0);
            this.usrControlPanel.Name = "usrControlPanel";
            this.usrControlPanel.Size = new System.Drawing.Size(606, 202);
            this.usrControlPanel.TabIndex = 5;
            // 
            // btnSampleProduct
            // 
            this.btnSampleProduct.BackColor = System.Drawing.Color.Transparent;
            this.btnSampleProduct.FlatAppearance.BorderSize = 0;
            this.btnSampleProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSampleProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSampleProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSampleProduct.Font = new System.Drawing.Font("Bango Pro", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSampleProduct.ForeColor = System.Drawing.Color.DarkOrchid;
            this.btnSampleProduct.Location = new System.Drawing.Point(11, 0);
            this.btnSampleProduct.Margin = new System.Windows.Forms.Padding(6);
            this.btnSampleProduct.MinimumSize = new System.Drawing.Size(250, 28);
            this.btnSampleProduct.Name = "btnSampleProduct";
            this.btnSampleProduct.Size = new System.Drawing.Size(576, 195);
            this.btnSampleProduct.TabIndex = 1;
            this.btnSampleProduct.Text = "Sample Button = RM 0.00";
            this.btnSampleProduct.UseVisualStyleBackColor = false;
            // 
            // panelAgeCriteria
            // 
            this.panelAgeCriteria.Controls.Add(this.lblAgeCriteriaHeader);
            this.panelAgeCriteria.Controls.Add(this.lblAgeCriteria);
            this.panelAgeCriteria.Location = new System.Drawing.Point(601, 0);
            this.panelAgeCriteria.Name = "panelAgeCriteria";
            this.panelAgeCriteria.Size = new System.Drawing.Size(387, 35);
            this.panelAgeCriteria.TabIndex = 20032;
            // 
            // lblAgeCriteriaHeader
            // 
            this.lblAgeCriteriaHeader.AutoSize = true;
            this.lblAgeCriteriaHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAgeCriteriaHeader.ForeColor = System.Drawing.Color.White;
            this.lblAgeCriteriaHeader.Location = new System.Drawing.Point(2, 3);
            this.lblAgeCriteriaHeader.MaximumSize = new System.Drawing.Size(186, 0);
            this.lblAgeCriteriaHeader.MinimumSize = new System.Drawing.Size(186, 0);
            this.lblAgeCriteriaHeader.Name = "lblAgeCriteriaHeader";
            this.lblAgeCriteriaHeader.Size = new System.Drawing.Size(186, 29);
            this.lblAgeCriteriaHeader.TabIndex = 20018;
            this.lblAgeCriteriaHeader.Text = "Age Criteria :";
            this.lblAgeCriteriaHeader.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAgeCriteria
            // 
            this.lblAgeCriteria.AutoSize = true;
            this.lblAgeCriteria.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAgeCriteria.ForeColor = System.Drawing.Color.White;
            this.lblAgeCriteria.Location = new System.Drawing.Point(193, 4);
            this.lblAgeCriteria.Name = "lblAgeCriteria";
            this.lblAgeCriteria.Size = new System.Drawing.Size(168, 29);
            this.lblAgeCriteria.TabIndex = 20026;
            this.lblAgeCriteria.Text = "above 25 yrs";
            this.lblAgeCriteria.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlQtyTxtBox
            // 
            this.pnlQtyTxtBox.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.textbox_quantity;
            this.pnlQtyTxtBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlQtyTxtBox.Controls.Add(this.txtQty);
            this.pnlQtyTxtBox.Location = new System.Drawing.Point(127, 0);
            this.pnlQtyTxtBox.Name = "pnlQtyTxtBox";
            this.pnlQtyTxtBox.Size = new System.Drawing.Size(136, 124);
            this.pnlQtyTxtBox.TabIndex = 1040;
            this.pnlQtyTxtBox.Click += new System.EventHandler(this.txtQty_Click);
            // 
            // txtQty
            // 
            this.txtQty.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtQty.Font = new System.Drawing.Font("Gotham Rounded Bold", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQty.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtQty.Location = new System.Drawing.Point(4, 30);
            this.txtQty.Name = "txtQty";
            this.txtQty.ReadOnly = true;
            this.txtQty.Size = new System.Drawing.Size(129, 64);
            this.txtQty.TabIndex = 1;
            this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtQty.Click += new System.EventHandler(this.txtQty_Click);
            // 
            // pnlQty
            // 
            this.pnlQty.Controls.Add(this.pcbDecreaseQty);
            this.pnlQty.Controls.Add(this.pcbIncreaseQty);
            this.pnlQty.Controls.Add(this.pnlQtyTxtBox);
            this.pnlQty.Location = new System.Drawing.Point(598, 44);
            this.pnlQty.Name = "pnlQty";
            this.pnlQty.Size = new System.Drawing.Size(394, 127);
            this.pnlQty.TabIndex = 20033;
            // 
            // usrCtrlQuantity
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.pnlQty);
            this.Controls.Add(this.panelAgeCriteria);
            this.Controls.Add(this.usrControlPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "usrCtrlQuantity";
            this.Size = new System.Drawing.Size(995, 206);
            ((System.ComponentModel.ISupportInitialize)(this.pcbIncreaseQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbDecreaseQty)).EndInit();
            this.usrControlPanel.ResumeLayout(false);
            this.panelAgeCriteria.ResumeLayout(false);
            this.panelAgeCriteria.PerformLayout();
            this.pnlQtyTxtBox.ResumeLayout(false);
            this.pnlQtyTxtBox.PerformLayout();
            this.pnlQty.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel usrControlPanel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.PictureBox pcbDecreaseQty;
        private System.Windows.Forms.PictureBox pcbIncreaseQty;
        private System.Windows.Forms.Panel pnlQtyTxtBox;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Button btnSampleProduct;
        private System.Windows.Forms.Panel panelAgeCriteria;
        private System.Windows.Forms.Label lblAgeCriteriaHeader;
        private System.Windows.Forms.Label lblAgeCriteria;
        private System.Windows.Forms.Panel pnlQty;
    }
}
