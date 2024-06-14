using System;

namespace Parafait_Kiosk
{
    partial class frmKioskCart
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
            this.lblGreeting1 = new System.Windows.Forms.Label();
            this.flpCartProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlSummaryInfo = new System.Windows.Forms.Panel();
            this.lblDiscountInfo = new System.Windows.Forms.Label();
            this.lblGrandTotal = new System.Windows.Forms.Label();
            this.lblChargeAmountHeader = new System.Windows.Forms.Label();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.lblTax = new System.Windows.Forms.Label();
            this.lblCharges = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblGrandTotalHeader = new System.Windows.Forms.Label();
            this.lblTaxesHeader = new System.Windows.Forms.Label();
            this.lblDiscountAmountHeader = new System.Windows.Forms.Label();
            this.lblTotalHeader = new System.Windows.Forms.Label();
            this.btnProceed = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnDiscount = new System.Windows.Forms.Button();
            this.bigVerticalScrollCartProducts = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.txtMessage = new System.Windows.Forms.Button();
            this.pnlProductSummary = new System.Windows.Forms.Panel();
            this.lblCartProductName = new System.Windows.Forms.Label();
            this.lblCartProductTotal = new System.Windows.Forms.Label();
            this.pnlSummaryInfo.SuspendLayout();
            this.pnlProductSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.Margin = new System.Windows.Forms.Padding(0);
            this.btnHome.Size = new System.Drawing.Size(167, 145);
            this.btnHome.TabIndex = 146;
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
            this.btnCancel.Location = new System.Drawing.Point(834, 882);
            // 
            // lblGreeting1
            // 
            this.lblGreeting1.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting1.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.White;
            this.lblGreeting1.Location = new System.Drawing.Point(188, 12);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(1531, 89);
            this.lblGreeting1.TabIndex = 1079;
            this.lblGreeting1.Text = "Cart Items";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpCartProducts
            // 
            this.flpCartProducts.AutoScroll = true;
            this.flpCartProducts.BackColor = System.Drawing.Color.Transparent;
            this.flpCartProducts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flpCartProducts.Location = new System.Drawing.Point(20, 45);
            this.flpCartProducts.Margin = new System.Windows.Forms.Padding(0);
            this.flpCartProducts.Name = "flpCartProducts";
            this.flpCartProducts.Size = new System.Drawing.Size(862, 300);
            this.flpCartProducts.TabIndex = 1080;
            // 
            // pnlSummaryInfo
            // 
            this.pnlSummaryInfo.BackColor = System.Drawing.Color.Transparent;
            this.pnlSummaryInfo.Controls.Add(this.lblDiscountInfo);
            this.pnlSummaryInfo.Controls.Add(this.lblGrandTotal);
            this.pnlSummaryInfo.Controls.Add(this.lblChargeAmountHeader);
            this.pnlSummaryInfo.Controls.Add(this.lblDiscount);
            this.pnlSummaryInfo.Controls.Add(this.lblTax);
            this.pnlSummaryInfo.Controls.Add(this.lblCharges);
            this.pnlSummaryInfo.Controls.Add(this.lblTotal);
            this.pnlSummaryInfo.Controls.Add(this.lblGrandTotalHeader);
            this.pnlSummaryInfo.Controls.Add(this.lblTaxesHeader);
            this.pnlSummaryInfo.Controls.Add(this.lblDiscountAmountHeader);
            this.pnlSummaryInfo.Controls.Add(this.lblTotalHeader);
            this.pnlSummaryInfo.Location = new System.Drawing.Point(216, 511);
            this.pnlSummaryInfo.Name = "pnlSummaryInfo";
            this.pnlSummaryInfo.Size = new System.Drawing.Size(1491, 232);
            this.pnlSummaryInfo.TabIndex = 1082;
            // 
            // lblDiscountInfo
            // 
            this.lblDiscountInfo.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscountInfo.ForeColor = System.Drawing.Color.White;
            this.lblDiscountInfo.Location = new System.Drawing.Point(3, 10);
            this.lblDiscountInfo.Name = "lblDiscountInfo";
            this.lblDiscountInfo.Size = new System.Drawing.Size(1485, 57);
            this.lblDiscountInfo.TabIndex = 1078;
            this.lblDiscountInfo.Text = "Discount Info";
            this.lblDiscountInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.Font = new System.Drawing.Font("Gotham Rounded Bold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrandTotal.ForeColor = System.Drawing.Color.White;
            this.lblGrandTotal.Location = new System.Drawing.Point(1147, 143);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Size = new System.Drawing.Size(266, 53);
            this.lblGrandTotal.TabIndex = 1077;
            this.lblGrandTotal.Text = "$ 1,00,000.00";
            this.lblGrandTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblChargeAmountHeader
            // 
            this.lblChargeAmountHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChargeAmountHeader.ForeColor = System.Drawing.Color.White;
            this.lblChargeAmountHeader.Location = new System.Drawing.Point(320, 86);
            this.lblChargeAmountHeader.Name = "lblChargeAmountHeader";
            this.lblChargeAmountHeader.Size = new System.Drawing.Size(266, 57);
            this.lblChargeAmountHeader.TabIndex = 1076;
            this.lblChargeAmountHeader.Text = "Charges";
            this.lblChargeAmountHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDiscount
            // 
            this.lblDiscount.Font = new System.Drawing.Font("Gotham Rounded Bold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscount.ForeColor = System.Drawing.Color.White;
            this.lblDiscount.Location = new System.Drawing.Point(864, 143);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(266, 53);
            this.lblDiscount.TabIndex = 8;
            this.lblDiscount.Text = "$ 1,00,000.00";
            this.lblDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTax
            // 
            this.lblTax.Font = new System.Drawing.Font("Gotham Rounded Bold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTax.ForeColor = System.Drawing.Color.White;
            this.lblTax.Location = new System.Drawing.Point(592, 143);
            this.lblTax.Name = "lblTax";
            this.lblTax.Size = new System.Drawing.Size(266, 53);
            this.lblTax.TabIndex = 7;
            this.lblTax.Text = "$ 1,00,000.00";
            this.lblTax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCharges
            // 
            this.lblCharges.Font = new System.Drawing.Font("Gotham Rounded Bold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCharges.ForeColor = System.Drawing.Color.White;
            this.lblCharges.Location = new System.Drawing.Point(320, 143);
            this.lblCharges.Name = "lblCharges";
            this.lblCharges.Size = new System.Drawing.Size(266, 53);
            this.lblCharges.TabIndex = 6;
            this.lblCharges.Text = "$ 1,00,000.00";
            this.lblCharges.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Gotham Rounded Bold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.White;
            this.lblTotal.Location = new System.Drawing.Point(38, 143);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(266, 53);
            this.lblTotal.TabIndex = 5;
            this.lblTotal.Text = "$ 1,00,000.00";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGrandTotalHeader
            // 
            this.lblGrandTotalHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrandTotalHeader.ForeColor = System.Drawing.Color.White;
            this.lblGrandTotalHeader.Location = new System.Drawing.Point(1147, 86);
            this.lblGrandTotalHeader.Name = "lblGrandTotalHeader";
            this.lblGrandTotalHeader.Size = new System.Drawing.Size(266, 57);
            this.lblGrandTotalHeader.TabIndex = 4;
            this.lblGrandTotalHeader.Text = "Grand Total";
            this.lblGrandTotalHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTaxesHeader
            // 
            this.lblTaxesHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxesHeader.ForeColor = System.Drawing.Color.White;
            this.lblTaxesHeader.Location = new System.Drawing.Point(592, 86);
            this.lblTaxesHeader.Name = "lblTaxesHeader";
            this.lblTaxesHeader.Size = new System.Drawing.Size(266, 57);
            this.lblTaxesHeader.TabIndex = 3;
            this.lblTaxesHeader.Text = "Taxes";
            this.lblTaxesHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDiscountAmountHeader
            // 
            this.lblDiscountAmountHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscountAmountHeader.ForeColor = System.Drawing.Color.White;
            this.lblDiscountAmountHeader.Location = new System.Drawing.Point(864, 86);
            this.lblDiscountAmountHeader.Name = "lblDiscountAmountHeader";
            this.lblDiscountAmountHeader.Size = new System.Drawing.Size(266, 57);
            this.lblDiscountAmountHeader.TabIndex = 2;
            this.lblDiscountAmountHeader.Text = "Discount";
            this.lblDiscountAmountHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotalHeader
            // 
            this.lblTotalHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalHeader.ForeColor = System.Drawing.Color.White;
            this.lblTotalHeader.Location = new System.Drawing.Point(38, 86);
            this.lblTotalHeader.Name = "lblTotalHeader";
            this.lblTotalHeader.Size = new System.Drawing.Size(266, 57);
            this.lblTotalHeader.TabIndex = 1;
            this.lblTotalHeader.Text = "Total";
            this.lblTotalHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnProceed
            // 
            this.btnProceed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnProceed.BackColor = System.Drawing.Color.Transparent;
            this.btnProceed.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnProceed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProceed.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnProceed.FlatAppearance.BorderSize = 0;
            this.btnProceed.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProceed.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.Location = new System.Drawing.Point(1203, 882);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(250, 125);
            this.btnProceed.TabIndex = 1075;
            this.btnProceed.Tag = "applyDiscount";
            this.btnProceed.Text = "Checkout";
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnContinue.BackColor = System.Drawing.Color.Transparent;
            this.btnContinue.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnContinue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnContinue.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnContinue.FlatAppearance.BorderSize = 0;
            this.btnContinue.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnContinue.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinue.Font = new System.Drawing.Font("Gotham Rounded Bold", 23F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinue.ForeColor = System.Drawing.Color.White;
            this.btnContinue.Location = new System.Drawing.Point(465, 882);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(250, 125);
            this.btnContinue.TabIndex = 1074;
            this.btnContinue.Tag = "applyDiscount";
            this.btnContinue.Text = "Continue Shopping";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnDiscount
            // 
            this.btnDiscount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDiscount.BackColor = System.Drawing.Color.Transparent;
            this.btnDiscount.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnDiscount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDiscount.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnDiscount.FlatAppearance.BorderSize = 0;
            this.btnDiscount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDiscount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDiscount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiscount.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDiscount.ForeColor = System.Drawing.Color.White;
            this.btnDiscount.Location = new System.Drawing.Point(1493, 360);
            this.btnDiscount.Name = "btnDiscount";
            this.btnDiscount.Size = new System.Drawing.Size(250, 125);
            this.btnDiscount.TabIndex = 1083;
            this.btnDiscount.Tag = "applyDiscount";
            this.btnDiscount.Text = "Apply Coupon";
            this.btnDiscount.UseVisualStyleBackColor = false;
            this.btnDiscount.Click += new System.EventHandler(this.btnDiscount_Click);
            // 
            // bigVerticalScrollCartProducts
            // 
            this.bigVerticalScrollCartProducts.AutoHide = true;
            this.bigVerticalScrollCartProducts.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollCartProducts.DataGridView = null;
            this.bigVerticalScrollCartProducts.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollCartProducts.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollCartProducts.Location = new System.Drawing.Point(866, 45);
            this.bigVerticalScrollCartProducts.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollCartProducts.Name = "bigVerticalScrollCartProducts";
            this.bigVerticalScrollCartProducts.ScrollableControl = this.flpCartProducts;
            this.bigVerticalScrollCartProducts.ScrollViewer = null;
            this.bigVerticalScrollCartProducts.Size = new System.Drawing.Size(66, 300);
            this.bigVerticalScrollCartProducts.TabIndex = 1084;
            this.bigVerticalScrollCartProducts.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollCartProducts.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollCartProducts.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollCartProducts.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1012);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 49);
            this.txtMessage.TabIndex = 149;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // pnlProductSummary
            // 
            this.pnlProductSummary.BackColor = System.Drawing.Color.Transparent;
            this.pnlProductSummary.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.TablePurchaseSummary;
            this.pnlProductSummary.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlProductSummary.Controls.Add(this.lblCartProductName);
            this.pnlProductSummary.Controls.Add(this.lblCartProductTotal);
            this.pnlProductSummary.Controls.Add(this.bigVerticalScrollCartProducts);
            this.pnlProductSummary.Controls.Add(this.flpCartProducts);
            this.pnlProductSummary.Location = new System.Drawing.Point(485, 101);
            this.pnlProductSummary.Name = "pnlProductSummary";
            this.pnlProductSummary.Size = new System.Drawing.Size(953, 385);
            this.pnlProductSummary.TabIndex = 1085;
            // 
            // lblCartProductName
            // 
            this.lblCartProductName.BackColor = System.Drawing.Color.Transparent;
            this.lblCartProductName.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCartProductName.ForeColor = System.Drawing.Color.White;
            this.lblCartProductName.Location = new System.Drawing.Point(0, 4);
            this.lblCartProductName.Name = "lblCartProductName";
            this.lblCartProductName.Size = new System.Drawing.Size(562, 36);
            this.lblCartProductName.TabIndex = 1082;
            this.lblCartProductName.Text = "Product";
            this.lblCartProductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCartProductTotal
            // 
            this.lblCartProductTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblCartProductTotal.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCartProductTotal.ForeColor = System.Drawing.Color.White;
            this.lblCartProductTotal.Location = new System.Drawing.Point(557, 4);
            this.lblCartProductTotal.Name = "lblCartProductTotal";
            this.lblCartProductTotal.Size = new System.Drawing.Size(276, 36);
            this.lblCartProductTotal.TabIndex = 1081;
            this.lblCartProductTotal.Text = "Total";
            this.lblCartProductTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmKioskCart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.btnProceed);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.btnDiscount);
            this.Controls.Add(this.pnlProductSummary);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.pnlSummaryInfo);
            this.Controls.Add(this.lblGreeting1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Gotham Rounded Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmKioskCart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmKioskCart";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmKioskCart_Load);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.lblGreeting1, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.pnlSummaryInfo, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.pnlProductSummary, 0);
            this.Controls.SetChildIndex(this.btnDiscount, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnContinue, 0);
            this.Controls.SetChildIndex(this.btnProceed, 0);
            this.pnlSummaryInfo.ResumeLayout(false);
            this.pnlProductSummary.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        

        #endregion
        internal System.Windows.Forms.Label lblGreeting1;
        private System.Windows.Forms.FlowLayoutPanel flpCartProducts;
        private System.Windows.Forms.Panel pnlSummaryInfo;
        private System.Windows.Forms.Label lblTotalHeader;
        private System.Windows.Forms.Label lblDiscountAmountHeader;
        private System.Windows.Forms.Label lblChargeAmountHeader;
        private System.Windows.Forms.Label lblGrandTotalHeader;
        private System.Windows.Forms.Label lblTaxesHeader;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.Label lblCharges;
        private System.Windows.Forms.Button btnProceed;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblGrandTotal;
        private System.Windows.Forms.Button btnDiscount;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollCartProducts;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Panel pnlProductSummary;
        private System.Windows.Forms.Label lblCartProductName;
        private System.Windows.Forms.Label lblCartProductTotal;
        //private System.Windows.Forms.Panel pnlAction;
        private System.Windows.Forms.Label lblDiscountInfo;
    }
}