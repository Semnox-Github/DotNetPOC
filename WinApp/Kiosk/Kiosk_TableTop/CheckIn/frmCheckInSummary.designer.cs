namespace Parafait_Kiosk
{
    partial class frmCheckInSummary
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
            this.txtMessage = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.panelDiscount = new System.Windows.Forms.Panel();
            this.panelDiscountLine = new System.Windows.Forms.Panel();
            this.txtDiscAmount = new System.Windows.Forms.Label();
            this.txtDiscPerc = new System.Windows.Forms.Label();
            this.txtDiscountName = new System.Windows.Forms.Label();
            this.lblHeaderDiscNameAmount = new System.Windows.Forms.Label();
            this.lblHeaderDiscNamePercentage = new System.Windows.Forms.Label();
            this.lblHeaderDiscountName = new System.Windows.Forms.Label();
            this.panelDeposit = new System.Windows.Forms.Panel();
            this.panelDepositLine = new System.Windows.Forms.Panel();
            this.lblCardDepositTax = new System.Windows.Forms.Label();
            this.lblDepositPrice = new System.Windows.Forms.Label();
            this.lblDepositAmount = new System.Windows.Forms.Label();
            this.lblDepositQty = new System.Windows.Forms.Label();
            this.lblDepositName = new System.Windows.Forms.Label();
            this.lblHeaderCardDepositTax = new System.Windows.Forms.Label();
            this.lblHeaderCardDepositPrice = new System.Windows.Forms.Label();
            this.lblHeaderCardDepositTotal = new System.Windows.Forms.Label();
            this.lblHeaderCardDepositQty = new System.Windows.Forms.Label();
            this.lblHeaderCardDeposit = new System.Windows.Forms.Label();
            this.flpCheckInSummary = new System.Windows.Forms.FlowLayoutPanel();
            this.flpSummaryDetails = new System.Windows.Forms.FlowLayoutPanel();
            this.panelSummaryHeader = new System.Windows.Forms.Panel();
            this.lblTaxHeader = new System.Windows.Forms.Label();
            this.lblCardNumHeader = new System.Windows.Forms.Label();
            this.lblTotalHeader = new System.Windows.Forms.Label();
            this.lblPackageHeader = new System.Windows.Forms.Label();
            this.lblPriceHeader = new System.Windows.Forms.Label();
            this.lblQtyHeader = new System.Windows.Forms.Label();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblPackageTax = new System.Windows.Forms.Label();
            this.lblPackagePrice = new System.Windows.Forms.Label();
            this.lblPackageQty = new System.Windows.Forms.Label();
            this.lblPackageTotal = new System.Windows.Forms.Label();
            this.lblPackageCardnumber = new System.Windows.Forms.Label();
            this.lblPackageName = new System.Windows.Forms.Label();
            this.flpPassportCoupon = new System.Windows.Forms.FlowLayoutPanel();
            this.panelPassportCoupon = new System.Windows.Forms.Panel();
            this.lblStar = new System.Windows.Forms.Label();
            this.lblPassportCoupon = new System.Windows.Forms.Label();
            this.passportCouponHeader = new System.Windows.Forms.Panel();
            this.CouponTotal = new System.Windows.Forms.Label();
            this.CouponName = new System.Windows.Forms.Label();
            this.CouponDiscount = new System.Windows.Forms.Label();
            this.CouponTax = new System.Windows.Forms.Label();
            this.CouponQty = new System.Windows.Forms.Label();
            this.CouponPrice = new System.Windows.Forms.Label();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.panelTotalToPay = new System.Windows.Forms.Panel();
            this.lblTotalToPayLowerBar = new System.Windows.Forms.Label();
            this.lblTotalToPayUpperBar = new System.Windows.Forms.Label();
            this.lblTotalToPayValue = new System.Windows.Forms.Label();
            this.lblTotalToPayText = new System.Windows.Forms.Label();
            this.panelApplyCoupon = new System.Windows.Forms.Panel();
            this.btnApplyCoupon = new System.Windows.Forms.Button();
            this.lblProductCPValidityMsg = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnProceed = new System.Windows.Forms.Button();
            this.lblGreeting = new System.Windows.Forms.Label();
            this.panelDiscount.SuspendLayout();
            this.panelDiscountLine.SuspendLayout();
            this.panelDeposit.SuspendLayout();
            this.panelDepositLine.SuspendLayout();
            this.flpCheckInSummary.SuspendLayout();
            this.flpSummaryDetails.SuspendLayout();
            this.panelSummaryHeader.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.flpPassportCoupon.SuspendLayout();
            this.panelPassportCoupon.SuspendLayout();
            this.passportCouponHeader.SuspendLayout();
            this.panelTotalToPay.SuspendLayout();
            this.panelApplyCoupon.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.home_button;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.Margin = new System.Windows.Forms.Padding(0);
            this.btnHome.TabIndex = 20013;
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(465, 882);
            this.btnPrev.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(834, 882);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1014);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 47);
            this.txtMessage.TabIndex = 134;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::Parafait_Kiosk.Properties.Resources.Generic_Coin_Note;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewImageColumn1.Width = 142;
            // 
            // panelDiscount
            // 
            this.panelDiscount.AutoSize = true;
            this.panelDiscount.BackColor = System.Drawing.Color.Transparent;
            this.panelDiscount.Controls.Add(this.panelDiscountLine);
            this.panelDiscount.Controls.Add(this.lblHeaderDiscNameAmount);
            this.panelDiscount.Controls.Add(this.lblHeaderDiscNamePercentage);
            this.panelDiscount.Controls.Add(this.lblHeaderDiscountName);
            this.panelDiscount.Location = new System.Drawing.Point(3, 313);
            this.panelDiscount.Name = "panelDiscount";
            this.panelDiscount.Size = new System.Drawing.Size(1528, 100);
            this.panelDiscount.TabIndex = 20014;
            // 
            // panelDiscountLine
            // 
            this.panelDiscountLine.AutoSize = true;
            this.panelDiscountLine.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.TablePurchaseSummary;
            this.panelDiscountLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelDiscountLine.Controls.Add(this.txtDiscAmount);
            this.panelDiscountLine.Controls.Add(this.txtDiscPerc);
            this.panelDiscountLine.Controls.Add(this.txtDiscountName);
            this.panelDiscountLine.Location = new System.Drawing.Point(3, 39);
            this.panelDiscountLine.Name = "panelDiscountLine";
            this.panelDiscountLine.Size = new System.Drawing.Size(1522, 58);
            this.panelDiscountLine.TabIndex = 1063;
            // 
            // txtDiscAmount
            // 
            this.txtDiscAmount.AutoEllipsis = true;
            this.txtDiscAmount.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiscAmount.ForeColor = System.Drawing.Color.Thistle;
            this.txtDiscAmount.Location = new System.Drawing.Point(1101, 3);
            this.txtDiscAmount.Name = "txtDiscAmount";
            this.txtDiscAmount.Size = new System.Drawing.Size(141, 45);
            this.txtDiscAmount.TabIndex = 3;
            this.txtDiscAmount.Text = "100";
            // 
            // txtDiscPerc
            // 
            this.txtDiscPerc.AutoSize = true;
            this.txtDiscPerc.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiscPerc.ForeColor = System.Drawing.Color.Thistle;
            this.txtDiscPerc.Location = new System.Drawing.Point(754, 3);
            this.txtDiscPerc.Name = "txtDiscPerc";
            this.txtDiscPerc.Size = new System.Drawing.Size(82, 39);
            this.txtDiscPerc.TabIndex = 2;
            this.txtDiscPerc.Text = "10%";
            // 
            // txtDiscountName
            // 
            this.txtDiscountName.AutoSize = true;
            this.txtDiscountName.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiscountName.ForeColor = System.Drawing.Color.Thistle;
            this.txtDiscountName.Location = new System.Drawing.Point(6, 3);
            this.txtDiscountName.Name = "txtDiscountName";
            this.txtDiscountName.Size = new System.Drawing.Size(111, 39);
            this.txtDiscountName.TabIndex = 0;
            this.txtDiscountName.Text = "Name";
            // 
            // lblHeaderDiscNameAmount
            // 
            this.lblHeaderDiscNameAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderDiscNameAmount.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderDiscNameAmount.ForeColor = System.Drawing.Color.White;
            this.lblHeaderDiscNameAmount.Location = new System.Drawing.Point(1105, -3);
            this.lblHeaderDiscNameAmount.Name = "lblHeaderDiscNameAmount";
            this.lblHeaderDiscNameAmount.Size = new System.Drawing.Size(311, 36);
            this.lblHeaderDiscNameAmount.TabIndex = 1062;
            this.lblHeaderDiscNameAmount.Text = "Amount";
            // 
            // lblHeaderDiscNamePercentage
            // 
            this.lblHeaderDiscNamePercentage.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderDiscNamePercentage.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderDiscNamePercentage.ForeColor = System.Drawing.Color.White;
            this.lblHeaderDiscNamePercentage.Location = new System.Drawing.Point(758, -3);
            this.lblHeaderDiscNamePercentage.Name = "lblHeaderDiscNamePercentage";
            this.lblHeaderDiscNamePercentage.Size = new System.Drawing.Size(328, 36);
            this.lblHeaderDiscNamePercentage.TabIndex = 1061;
            this.lblHeaderDiscNamePercentage.Text = "Percentage";
            // 
            // lblHeaderDiscountName
            // 
            this.lblHeaderDiscountName.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderDiscountName.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderDiscountName.ForeColor = System.Drawing.Color.White;
            this.lblHeaderDiscountName.Location = new System.Drawing.Point(6, 0);
            this.lblHeaderDiscountName.Name = "lblHeaderDiscountName";
            this.lblHeaderDiscountName.Size = new System.Drawing.Size(532, 36);
            this.lblHeaderDiscountName.TabIndex = 1060;
            this.lblHeaderDiscountName.Text = "Discount Name";
            // 
            // panelDeposit
            // 
            this.panelDeposit.AutoSize = true;
            this.panelDeposit.BackColor = System.Drawing.Color.Transparent;
            this.panelDeposit.Controls.Add(this.panelDepositLine);
            this.panelDeposit.Controls.Add(this.lblHeaderCardDepositTax);
            this.panelDeposit.Controls.Add(this.lblHeaderCardDepositPrice);
            this.panelDeposit.Controls.Add(this.lblHeaderCardDepositTotal);
            this.panelDeposit.Controls.Add(this.lblHeaderCardDepositQty);
            this.panelDeposit.Controls.Add(this.lblHeaderCardDeposit);
            this.panelDeposit.Location = new System.Drawing.Point(3, 202);
            this.panelDeposit.Name = "panelDeposit";
            this.panelDeposit.Size = new System.Drawing.Size(1528, 105);
            this.panelDeposit.TabIndex = 20016;
            this.panelDeposit.Visible = false;
            // 
            // panelDepositLine
            // 
            this.panelDepositLine.AutoSize = true;
            this.panelDepositLine.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.TablePurchaseSummary;
            this.panelDepositLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelDepositLine.Controls.Add(this.lblCardDepositTax);
            this.panelDepositLine.Controls.Add(this.lblDepositPrice);
            this.panelDepositLine.Controls.Add(this.lblDepositAmount);
            this.panelDepositLine.Controls.Add(this.lblDepositQty);
            this.panelDepositLine.Controls.Add(this.lblDepositName);
            this.panelDepositLine.Location = new System.Drawing.Point(3, 44);
            this.panelDepositLine.Name = "panelDepositLine";
            this.panelDepositLine.Size = new System.Drawing.Size(1522, 58);
            this.panelDepositLine.TabIndex = 1063;
            // 
            // lblCardDepositTax
            // 
            this.lblCardDepositTax.AutoSize = true;
            this.lblCardDepositTax.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardDepositTax.ForeColor = System.Drawing.Color.Thistle;
            this.lblCardDepositTax.Location = new System.Drawing.Point(1101, 3);
            this.lblCardDepositTax.Name = "lblCardDepositTax";
            this.lblCardDepositTax.Size = new System.Drawing.Size(113, 39);
            this.lblCardDepositTax.TabIndex = 5;
            this.lblCardDepositTax.Text = "80.00";
            // 
            // lblDepositPrice
            // 
            this.lblDepositPrice.AutoSize = true;
            this.lblDepositPrice.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositPrice.ForeColor = System.Drawing.Color.Thistle;
            this.lblDepositPrice.Location = new System.Drawing.Point(886, 3);
            this.lblDepositPrice.Name = "lblDepositPrice";
            this.lblDepositPrice.Size = new System.Drawing.Size(107, 39);
            this.lblDepositPrice.TabIndex = 4;
            this.lblDepositPrice.Text = "10.00";
            // 
            // lblDepositAmount
            // 
            this.lblDepositAmount.AutoSize = true;
            this.lblDepositAmount.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositAmount.ForeColor = System.Drawing.Color.Thistle;
            this.lblDepositAmount.Location = new System.Drawing.Point(1315, 3);
            this.lblDepositAmount.Name = "lblDepositAmount";
            this.lblDepositAmount.Size = new System.Drawing.Size(113, 39);
            this.lblDepositAmount.TabIndex = 3;
            this.lblDepositAmount.Text = "80.00";
            // 
            // lblDepositQty
            // 
            this.lblDepositQty.AutoSize = true;
            this.lblDepositQty.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositQty.ForeColor = System.Drawing.Color.Thistle;
            this.lblDepositQty.Location = new System.Drawing.Point(754, 3);
            this.lblDepositQty.Name = "lblDepositQty";
            this.lblDepositQty.Size = new System.Drawing.Size(54, 39);
            this.lblDepositQty.TabIndex = 2;
            this.lblDepositQty.Text = "10";
            // 
            // lblDepositName
            // 
            this.lblDepositName.AutoSize = true;
            this.lblDepositName.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositName.ForeColor = System.Drawing.Color.Thistle;
            this.lblDepositName.Location = new System.Drawing.Point(3, 3);
            this.lblDepositName.Name = "lblDepositName";
            this.lblDepositName.Size = new System.Drawing.Size(111, 39);
            this.lblDepositName.TabIndex = 0;
            this.lblDepositName.Text = "Name";
            // 
            // lblHeaderCardDepositTax
            // 
            this.lblHeaderCardDepositTax.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderCardDepositTax.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderCardDepositTax.ForeColor = System.Drawing.Color.White;
            this.lblHeaderCardDepositTax.Location = new System.Drawing.Point(1105, 0);
            this.lblHeaderCardDepositTax.Name = "lblHeaderCardDepositTax";
            this.lblHeaderCardDepositTax.Size = new System.Drawing.Size(144, 36);
            this.lblHeaderCardDepositTax.TabIndex = 20017;
            this.lblHeaderCardDepositTax.Text = "Tax";
            // 
            // lblHeaderCardDepositPrice
            // 
            this.lblHeaderCardDepositPrice.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderCardDepositPrice.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderCardDepositPrice.ForeColor = System.Drawing.Color.White;
            this.lblHeaderCardDepositPrice.Location = new System.Drawing.Point(890, 0);
            this.lblHeaderCardDepositPrice.Name = "lblHeaderCardDepositPrice";
            this.lblHeaderCardDepositPrice.Size = new System.Drawing.Size(156, 36);
            this.lblHeaderCardDepositPrice.TabIndex = 1064;
            this.lblHeaderCardDepositPrice.Text = "Price";
            // 
            // lblHeaderCardDepositTotal
            // 
            this.lblHeaderCardDepositTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderCardDepositTotal.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderCardDepositTotal.ForeColor = System.Drawing.Color.White;
            this.lblHeaderCardDepositTotal.Location = new System.Drawing.Point(1317, 5);
            this.lblHeaderCardDepositTotal.Name = "lblHeaderCardDepositTotal";
            this.lblHeaderCardDepositTotal.Size = new System.Drawing.Size(171, 36);
            this.lblHeaderCardDepositTotal.TabIndex = 1062;
            this.lblHeaderCardDepositTotal.Text = "Total";
            // 
            // lblHeaderCardDepositQty
            // 
            this.lblHeaderCardDepositQty.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderCardDepositQty.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderCardDepositQty.ForeColor = System.Drawing.Color.White;
            this.lblHeaderCardDepositQty.Location = new System.Drawing.Point(758, 0);
            this.lblHeaderCardDepositQty.Name = "lblHeaderCardDepositQty";
            this.lblHeaderCardDepositQty.Size = new System.Drawing.Size(146, 36);
            this.lblHeaderCardDepositQty.TabIndex = 1061;
            this.lblHeaderCardDepositQty.Text = "Qty";
            // 
            // lblHeaderCardDeposit
            // 
            this.lblHeaderCardDeposit.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderCardDeposit.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderCardDeposit.ForeColor = System.Drawing.Color.White;
            this.lblHeaderCardDeposit.Location = new System.Drawing.Point(5, 0);
            this.lblHeaderCardDeposit.Name = "lblHeaderCardDeposit";
            this.lblHeaderCardDeposit.Size = new System.Drawing.Size(509, 36);
            this.lblHeaderCardDeposit.TabIndex = 1060;
            this.lblHeaderCardDeposit.Text = "Card Deposit";
            // 
            // flpCheckInSummary
            // 
            this.flpCheckInSummary.BackColor = System.Drawing.Color.Transparent;
            this.flpCheckInSummary.Controls.Add(this.flpSummaryDetails);
            this.flpCheckInSummary.Controls.Add(this.flpPassportCoupon);
            this.flpCheckInSummary.Controls.Add(this.panelDeposit);
            this.flpCheckInSummary.Controls.Add(this.panelDiscount);
            this.flpCheckInSummary.Controls.Add(this.lblDiscount);
            this.flpCheckInSummary.Controls.Add(this.panelTotalToPay);
            this.flpCheckInSummary.Location = new System.Drawing.Point(190, 175);
            this.flpCheckInSummary.Name = "flpCheckInSummary";
            this.flpCheckInSummary.Size = new System.Drawing.Size(1530, 683);
            this.flpCheckInSummary.TabIndex = 20025;
            // 
            // flpSummaryDetails
            // 
            this.flpSummaryDetails.AutoSize = true;
            this.flpSummaryDetails.BackColor = System.Drawing.Color.Transparent;
            this.flpSummaryDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flpSummaryDetails.Controls.Add(this.panelSummaryHeader);
            this.flpSummaryDetails.Controls.Add(this.panelSummary);
            this.flpSummaryDetails.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSummaryDetails.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpSummaryDetails.ForeColor = System.Drawing.Color.Black;
            this.flpSummaryDetails.Location = new System.Drawing.Point(3, 3);
            this.flpSummaryDetails.MaximumSize = new System.Drawing.Size(1522, 0);
            this.flpSummaryDetails.MinimumSize = new System.Drawing.Size(1522, 0);
            this.flpSummaryDetails.Name = "flpSummaryDetails";
            this.flpSummaryDetails.Size = new System.Drawing.Size(1522, 93);
            this.flpSummaryDetails.TabIndex = 1058;
            // 
            // panelSummaryHeader
            // 
            this.panelSummaryHeader.AutoSize = true;
            this.panelSummaryHeader.BackColor = System.Drawing.Color.Transparent;
            this.panelSummaryHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelSummaryHeader.Controls.Add(this.lblTaxHeader);
            this.panelSummaryHeader.Controls.Add(this.lblCardNumHeader);
            this.panelSummaryHeader.Controls.Add(this.lblTotalHeader);
            this.panelSummaryHeader.Controls.Add(this.lblPackageHeader);
            this.panelSummaryHeader.Controls.Add(this.lblPriceHeader);
            this.panelSummaryHeader.Controls.Add(this.lblQtyHeader);
            this.panelSummaryHeader.Location = new System.Drawing.Point(3, 3);
            this.panelSummaryHeader.MaximumSize = new System.Drawing.Size(1522, 0);
            this.panelSummaryHeader.MinimumSize = new System.Drawing.Size(1522, 0);
            this.panelSummaryHeader.Name = "panelSummaryHeader";
            this.panelSummaryHeader.Size = new System.Drawing.Size(1522, 42);
            this.panelSummaryHeader.TabIndex = 20036;
            // 
            // lblTaxHeader
            // 
            this.lblTaxHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblTaxHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxHeader.ForeColor = System.Drawing.Color.White;
            this.lblTaxHeader.Location = new System.Drawing.Point(1101, 0);
            this.lblTaxHeader.Name = "lblTaxHeader";
            this.lblTaxHeader.Size = new System.Drawing.Size(206, 36);
            this.lblTaxHeader.TabIndex = 1064;
            this.lblTaxHeader.Text = "Tax";
            // 
            // lblCardNumHeader
            // 
            this.lblCardNumHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblCardNumHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumHeader.ForeColor = System.Drawing.Color.White;
            this.lblCardNumHeader.Location = new System.Drawing.Point(556, 6);
            this.lblCardNumHeader.Name = "lblCardNumHeader";
            this.lblCardNumHeader.Size = new System.Drawing.Size(193, 36);
            this.lblCardNumHeader.TabIndex = 1060;
            this.lblCardNumHeader.Text = "Card #";
            // 
            // lblTotalHeader
            // 
            this.lblTotalHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalHeader.ForeColor = System.Drawing.Color.White;
            this.lblTotalHeader.Location = new System.Drawing.Point(1313, 0);
            this.lblTotalHeader.Name = "lblTotalHeader";
            this.lblTotalHeader.Size = new System.Drawing.Size(206, 36);
            this.lblTotalHeader.TabIndex = 1063;
            this.lblTotalHeader.Text = "Total";
            // 
            // lblPackageHeader
            // 
            this.lblPackageHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblPackageHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackageHeader.ForeColor = System.Drawing.Color.White;
            this.lblPackageHeader.Location = new System.Drawing.Point(2, 6);
            this.lblPackageHeader.Name = "lblPackageHeader";
            this.lblPackageHeader.Size = new System.Drawing.Size(556, 36);
            this.lblPackageHeader.TabIndex = 1059;
            this.lblPackageHeader.Text = "Package";
            // 
            // lblPriceHeader
            // 
            this.lblPriceHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblPriceHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriceHeader.ForeColor = System.Drawing.Color.White;
            this.lblPriceHeader.Location = new System.Drawing.Point(888, 6);
            this.lblPriceHeader.Name = "lblPriceHeader";
            this.lblPriceHeader.Size = new System.Drawing.Size(206, 36);
            this.lblPriceHeader.TabIndex = 1062;
            this.lblPriceHeader.Text = "Price";
            // 
            // lblQtyHeader
            // 
            this.lblQtyHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblQtyHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQtyHeader.ForeColor = System.Drawing.Color.White;
            this.lblQtyHeader.Location = new System.Drawing.Point(757, 6);
            this.lblQtyHeader.Name = "lblQtyHeader";
            this.lblQtyHeader.Size = new System.Drawing.Size(119, 36);
            this.lblQtyHeader.TabIndex = 1061;
            this.lblQtyHeader.Text = "Qty";
            // 
            // panelSummary
            // 
            this.panelSummary.AutoSize = true;
            this.panelSummary.BackColor = System.Drawing.Color.Transparent;
            this.panelSummary.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.TablePurchaseSummary;
            this.panelSummary.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelSummary.Controls.Add(this.lblPackageTax);
            this.panelSummary.Controls.Add(this.lblPackagePrice);
            this.panelSummary.Controls.Add(this.lblPackageQty);
            this.panelSummary.Controls.Add(this.lblPackageTotal);
            this.panelSummary.Controls.Add(this.lblPackageCardnumber);
            this.panelSummary.Controls.Add(this.lblPackageName);
            this.panelSummary.Location = new System.Drawing.Point(3, 51);
            this.panelSummary.MaximumSize = new System.Drawing.Size(1522, 0);
            this.panelSummary.MinimumSize = new System.Drawing.Size(1522, 0);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(1006, 0, 0, 0);
            this.panelSummary.Size = new System.Drawing.Size(1522, 39);
            this.panelSummary.TabIndex = 20035;
            // 
            // lblPackageTax
            // 
            this.lblPackageTax.AutoSize = true;
            this.lblPackageTax.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackageTax.ForeColor = System.Drawing.Color.Thistle;
            this.lblPackageTax.Location = new System.Drawing.Point(1101, -2);
            this.lblPackageTax.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblPackageTax.Name = "lblPackageTax";
            this.lblPackageTax.Size = new System.Drawing.Size(113, 39);
            this.lblPackageTax.TabIndex = 8;
            this.lblPackageTax.Text = "80.00";
            // 
            // lblPackagePrice
            // 
            this.lblPackagePrice.AutoSize = true;
            this.lblPackagePrice.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackagePrice.ForeColor = System.Drawing.Color.Thistle;
            this.lblPackagePrice.Location = new System.Drawing.Point(887, -2);
            this.lblPackagePrice.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblPackagePrice.Name = "lblPackagePrice";
            this.lblPackagePrice.Size = new System.Drawing.Size(113, 39);
            this.lblPackagePrice.TabIndex = 7;
            this.lblPackagePrice.Text = "80.00";
            // 
            // lblPackageQty
            // 
            this.lblPackageQty.AutoSize = true;
            this.lblPackageQty.Font = new System.Drawing.Font("Gotham Rounded Bold", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackageQty.ForeColor = System.Drawing.Color.Thistle;
            this.lblPackageQty.Location = new System.Drawing.Point(762, 3);
            this.lblPackageQty.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblPackageQty.Name = "lblPackageQty";
            this.lblPackageQty.Size = new System.Drawing.Size(131, 36);
            this.lblPackageQty.TabIndex = 6;
            this.lblPackageQty.Text = "2 Guest";
            // 
            // lblPackageTotal
            // 
            this.lblPackageTotal.AutoSize = true;
            this.lblPackageTotal.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackageTotal.ForeColor = System.Drawing.Color.Thistle;
            this.lblPackageTotal.Location = new System.Drawing.Point(1315, -2);
            this.lblPackageTotal.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblPackageTotal.Name = "lblPackageTotal";
            this.lblPackageTotal.Size = new System.Drawing.Size(167, 39);
            this.lblPackageTotal.TabIndex = 4;
            this.lblPackageTotal.Text = "8,000.00";
            // 
            // lblPackageCardnumber
            // 
            this.lblPackageCardnumber.AutoSize = true;
            this.lblPackageCardnumber.Font = new System.Drawing.Font("Gotham Rounded Bold", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackageCardnumber.ForeColor = System.Drawing.Color.Thistle;
            this.lblPackageCardnumber.Location = new System.Drawing.Point(557, 3);
            this.lblPackageCardnumber.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblPackageCardnumber.MaximumSize = new System.Drawing.Size(200, 0);
            this.lblPackageCardnumber.Name = "lblPackageCardnumber";
            this.lblPackageCardnumber.Size = new System.Drawing.Size(185, 36);
            this.lblPackageCardnumber.TabIndex = 1;
            this.lblPackageCardnumber.Text = "4A0056AE";
            // 
            // lblPackageName
            // 
            this.lblPackageName.Font = new System.Drawing.Font("Gotham Rounded Bold", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackageName.ForeColor = System.Drawing.Color.Thistle;
            this.lblPackageName.Location = new System.Drawing.Point(2, 3);
            this.lblPackageName.Margin = new System.Windows.Forms.Padding(20, 20, 3, 0);
            this.lblPackageName.MaximumSize = new System.Drawing.Size(556, 84);
            this.lblPackageName.Name = "lblPackageName";
            this.lblPackageName.Size = new System.Drawing.Size(556, 36);
            this.lblPackageName.TabIndex = 0;
            this.lblPackageName.Text = "$30\r\n(All Day Entry)";
            this.lblPackageName.AutoEllipsis = true;
            // 
            // flpPassportCoupon
            // 
            this.flpPassportCoupon.AutoSize = true;
            this.flpPassportCoupon.Controls.Add(this.panelPassportCoupon);
            this.flpPassportCoupon.Controls.Add(this.passportCouponHeader);
            this.flpPassportCoupon.Location = new System.Drawing.Point(3, 102);
            this.flpPassportCoupon.Name = "flpPassportCoupon";
            this.flpPassportCoupon.Size = new System.Drawing.Size(1528, 94);
            this.flpPassportCoupon.TabIndex = 20019;
            // 
            // panelPassportCoupon
            // 
            this.panelPassportCoupon.AutoSize = true;
            this.panelPassportCoupon.BackColor = System.Drawing.Color.Transparent;
            this.panelPassportCoupon.Controls.Add(this.lblStar);
            this.panelPassportCoupon.Controls.Add(this.lblPassportCoupon);
            this.panelPassportCoupon.Location = new System.Drawing.Point(3, 3);
            this.panelPassportCoupon.Name = "panelPassportCoupon";
            this.panelPassportCoupon.Size = new System.Drawing.Size(1522, 40);
            this.panelPassportCoupon.TabIndex = 20036;
            // 
            // lblStar
            // 
            this.lblStar.BackColor = System.Drawing.Color.Transparent;
            this.lblStar.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStar.ForeColor = System.Drawing.Color.White;
            this.lblStar.Location = new System.Drawing.Point(5, 4);
            this.lblStar.Name = "lblStar";
            this.lblStar.Size = new System.Drawing.Size(18, 36);
            this.lblStar.TabIndex = 20019;
            this.lblStar.Text = "*";
            // 
            // lblPassportCoupon
            // 
            this.lblPassportCoupon.BackColor = System.Drawing.Color.Transparent;
            this.lblPassportCoupon.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassportCoupon.ForeColor = System.Drawing.Color.White;
            this.lblPassportCoupon.Location = new System.Drawing.Point(21, 0);
            this.lblPassportCoupon.Name = "lblPassportCoupon";
            this.lblPassportCoupon.Size = new System.Drawing.Size(1498, 36);
            this.lblPassportCoupon.TabIndex = 20018;
            this.lblPassportCoupon.Text = "Passport Coupon/s Applied";
            // 
            // passportCouponHeader
            // 
            this.passportCouponHeader.AutoSize = true;
            this.passportCouponHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.passportCouponHeader.Controls.Add(this.CouponTotal);
            this.passportCouponHeader.Controls.Add(this.CouponName);
            this.passportCouponHeader.Controls.Add(this.CouponDiscount);
            this.passportCouponHeader.Controls.Add(this.CouponTax);
            this.passportCouponHeader.Controls.Add(this.CouponQty);
            this.passportCouponHeader.Controls.Add(this.CouponPrice);
            this.passportCouponHeader.Location = new System.Drawing.Point(3, 49);
            this.passportCouponHeader.Name = "passportCouponHeader";
            this.passportCouponHeader.Size = new System.Drawing.Size(1522, 42);
            this.passportCouponHeader.TabIndex = 1063;
            // 
            // CouponTotal
            // 
            this.CouponTotal.BackColor = System.Drawing.Color.Transparent;
            this.CouponTotal.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CouponTotal.ForeColor = System.Drawing.Color.White;
            this.CouponTotal.Location = new System.Drawing.Point(1314, 5);
            this.CouponTotal.Name = "CouponTotal";
            this.CouponTotal.Size = new System.Drawing.Size(205, 36);
            this.CouponTotal.TabIndex = 20020;
            this.CouponTotal.Text = "Total";
            // 
            // CouponName
            // 
            this.CouponName.BackColor = System.Drawing.Color.Transparent;
            this.CouponName.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CouponName.ForeColor = System.Drawing.Color.White;
            this.CouponName.Location = new System.Drawing.Point(-5, 5);
            this.CouponName.Name = "CouponName";
            this.CouponName.Size = new System.Drawing.Size(556, 36);
            this.CouponName.TabIndex = 1060;
            this.CouponName.Text = "Product Name";
            // 
            // CouponDiscount
            // 
            this.CouponDiscount.BackColor = System.Drawing.Color.Transparent;
            this.CouponDiscount.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CouponDiscount.ForeColor = System.Drawing.Color.White;
            this.CouponDiscount.Location = new System.Drawing.Point(557, 6);
            this.CouponDiscount.Name = "CouponDiscount";
            this.CouponDiscount.Size = new System.Drawing.Size(185, 36);
            this.CouponDiscount.TabIndex = 20019;
            this.CouponDiscount.Text = "Discount%";
            // 
            // CouponTax
            // 
            this.CouponTax.BackColor = System.Drawing.Color.Transparent;
            this.CouponTax.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CouponTax.ForeColor = System.Drawing.Color.White;
            this.CouponTax.Location = new System.Drawing.Point(1102, 6);
            this.CouponTax.Name = "CouponTax";
            this.CouponTax.Size = new System.Drawing.Size(205, 36);
            this.CouponTax.TabIndex = 20017;
            this.CouponTax.Text = "Tax";
            // 
            // CouponQty
            // 
            this.CouponQty.BackColor = System.Drawing.Color.Transparent;
            this.CouponQty.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CouponQty.ForeColor = System.Drawing.Color.White;
            this.CouponQty.Location = new System.Drawing.Point(755, 5);
            this.CouponQty.Name = "CouponQty";
            this.CouponQty.Size = new System.Drawing.Size(124, 36);
            this.CouponQty.TabIndex = 1061;
            this.CouponQty.Text = "Qty";
            // 
            // CouponPrice
            // 
            this.CouponPrice.BackColor = System.Drawing.Color.Transparent;
            this.CouponPrice.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CouponPrice.ForeColor = System.Drawing.Color.White;
            this.CouponPrice.Location = new System.Drawing.Point(887, 5);
            this.CouponPrice.Name = "CouponPrice";
            this.CouponPrice.Size = new System.Drawing.Size(209, 36);
            this.CouponPrice.TabIndex = 1064;
            this.CouponPrice.Text = "Price";
            // 
            // lblDiscount
            // 
            this.lblDiscount.BackColor = System.Drawing.Color.Transparent;
            this.lblDiscount.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscount.ForeColor = System.Drawing.Color.White;
            this.lblDiscount.Location = new System.Drawing.Point(3, 436);
            this.lblDiscount.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(1538, 43);
            this.lblDiscount.TabIndex = 20019;
            this.lblDiscount.Text = "$10 (10% Discount)";
            this.lblDiscount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panelTotalToPay
            // 
            this.panelTotalToPay.BackColor = System.Drawing.Color.Transparent;
            this.panelTotalToPay.Controls.Add(this.lblTotalToPayLowerBar);
            this.panelTotalToPay.Controls.Add(this.lblTotalToPayUpperBar);
            this.panelTotalToPay.Controls.Add(this.lblTotalToPayValue);
            this.panelTotalToPay.Controls.Add(this.lblTotalToPayText);
            this.panelTotalToPay.Location = new System.Drawing.Point(3, 482);
            this.panelTotalToPay.Name = "panelTotalToPay";
            this.panelTotalToPay.Size = new System.Drawing.Size(1265, 160);
            this.panelTotalToPay.TabIndex = 20030;
            // 
            // lblTotalToPayLowerBar
            // 
            this.lblTotalToPayLowerBar.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalToPayLowerBar.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalToPayLowerBar.ForeColor = System.Drawing.Color.White;
            this.lblTotalToPayLowerBar.Location = new System.Drawing.Point(379, 114);
            this.lblTotalToPayLowerBar.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblTotalToPayLowerBar.Name = "lblTotalToPayLowerBar";
            this.lblTotalToPayLowerBar.Size = new System.Drawing.Size(813, 46);
            this.lblTotalToPayLowerBar.TabIndex = 20033;
            this.lblTotalToPayLowerBar.Text = "________________________________________";
            this.lblTotalToPayLowerBar.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblTotalToPayUpperBar
            // 
            this.lblTotalToPayUpperBar.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalToPayUpperBar.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalToPayUpperBar.ForeColor = System.Drawing.Color.White;
            this.lblTotalToPayUpperBar.Location = new System.Drawing.Point(379, 0);
            this.lblTotalToPayUpperBar.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblTotalToPayUpperBar.Name = "lblTotalToPayUpperBar";
            this.lblTotalToPayUpperBar.Size = new System.Drawing.Size(813, 42);
            this.lblTotalToPayUpperBar.TabIndex = 20029;
            this.lblTotalToPayUpperBar.Text = "________________________________________";
            this.lblTotalToPayUpperBar.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblTotalToPayValue
            // 
            this.lblTotalToPayValue.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalToPayValue.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalToPayValue.ForeColor = System.Drawing.Color.White;
            this.lblTotalToPayValue.Location = new System.Drawing.Point(764, 66);
            this.lblTotalToPayValue.Name = "lblTotalToPayValue";
            this.lblTotalToPayValue.Size = new System.Drawing.Size(448, 39);
            this.lblTotalToPayValue.TabIndex = 20031;
            this.lblTotalToPayValue.Text = "80.00";
            this.lblTotalToPayValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalToPayText
            // 
            this.lblTotalToPayText.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalToPayText.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalToPayText.ForeColor = System.Drawing.Color.White;
            this.lblTotalToPayText.Location = new System.Drawing.Point(40, 66);
            this.lblTotalToPayText.Name = "lblTotalToPayText";
            this.lblTotalToPayText.Size = new System.Drawing.Size(718, 39);
            this.lblTotalToPayText.TabIndex = 20030;
            this.lblTotalToPayText.Text = "Total to Pay";
            this.lblTotalToPayText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelApplyCoupon
            // 
            this.panelApplyCoupon.AutoScroll = true;
            this.panelApplyCoupon.BackColor = System.Drawing.Color.Transparent;
            this.panelApplyCoupon.Controls.Add(this.btnApplyCoupon);
            this.panelApplyCoupon.Location = new System.Drawing.Point(1600, 687);
            this.panelApplyCoupon.Name = "panelApplyCoupon";
            this.panelApplyCoupon.Size = new System.Drawing.Size(308, 285);
            this.panelApplyCoupon.TabIndex = 20034;
            // 
            // btnApplyCoupon
            // 
            this.btnApplyCoupon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApplyCoupon.BackColor = System.Drawing.Color.Transparent;
            this.btnApplyCoupon.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnApplyCoupon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnApplyCoupon.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnApplyCoupon.FlatAppearance.BorderSize = 0;
            this.btnApplyCoupon.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnApplyCoupon.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnApplyCoupon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyCoupon.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplyCoupon.ForeColor = System.Drawing.Color.White;
            this.btnApplyCoupon.Location = new System.Drawing.Point(17, 40);
            this.btnApplyCoupon.Name = "btnApplyCoupon";
            this.btnApplyCoupon.Size = new System.Drawing.Size(250, 125);
            this.btnApplyCoupon.TabIndex = 20027;
            this.btnApplyCoupon.Tag = "applyDiscount";
            this.btnApplyCoupon.Text = "Apply Coupon";
            this.btnApplyCoupon.UseVisualStyleBackColor = false;
            this.btnApplyCoupon.Click += new System.EventHandler(this.btnDiscount_Click);
            // 
            // lblProductCPValidityMsg
            // 
            this.lblProductCPValidityMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProductCPValidityMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblProductCPValidityMsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductCPValidityMsg.ForeColor = System.Drawing.Color.White;
            this.lblProductCPValidityMsg.Location = new System.Drawing.Point(12, 1344);
            this.lblProductCPValidityMsg.Name = "lblProductCPValidityMsg";
            this.lblProductCPValidityMsg.Size = new System.Drawing.Size(1894, 80);
            this.lblProductCPValidityMsg.TabIndex = 20035;
            this.lblProductCPValidityMsg.Text = "0.00";
            this.lblProductCPValidityMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(333, 860);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1261, 131);
            this.panelButtons.TabIndex = 20026;
            // 
            // btnProceed
            // 
            this.btnProceed.BackColor = System.Drawing.Color.Transparent;
            this.btnProceed.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnProceed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProceed.FlatAppearance.BorderSize = 0;
            this.btnProceed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProceed.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProceed.Location = new System.Drawing.Point(1203, 882);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(250, 125);
            this.btnProceed.TabIndex = 1025;
            this.btnProceed.Text = "Proceed to Payment";
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // lblGreeting
            // 
            this.lblGreeting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreeting.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting.ForeColor = System.Drawing.Color.White;
            this.lblGreeting.Location = new System.Drawing.Point(187, 9);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(1532, 124);
            this.lblGreeting.TabIndex = 20036;
            this.lblGreeting.Text = "Summary";
            this.lblGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmCheckInSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.btnProceed);
            this.Controls.Add(this.panelApplyCoupon);
            this.Controls.Add(this.lblGreeting);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.flpCheckInSummary);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblProductCPValidityMsg);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmCheckInSummary";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmNewCard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCheckInSummary_FormClosed);
            this.Load += new System.EventHandler(this.frmCheckInSummary_Load);
            this.Controls.SetChildIndex(this.lblProductCPValidityMsg, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.flpCheckInSummary, 0);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.lblGreeting, 0);
            this.Controls.SetChildIndex(this.panelApplyCoupon, 0);
            this.Controls.SetChildIndex(this.btnProceed, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.panelDiscount.ResumeLayout(false);
            this.panelDiscount.PerformLayout();
            this.panelDiscountLine.ResumeLayout(false);
            this.panelDiscountLine.PerformLayout();
            this.panelDeposit.ResumeLayout(false);
            this.panelDeposit.PerformLayout();
            this.panelDepositLine.ResumeLayout(false);
            this.panelDepositLine.PerformLayout();
            this.flpCheckInSummary.ResumeLayout(false);
            this.flpCheckInSummary.PerformLayout();
            this.flpSummaryDetails.ResumeLayout(false);
            this.flpSummaryDetails.PerformLayout();
            this.panelSummaryHeader.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.panelSummary.PerformLayout();
            this.flpPassportCoupon.ResumeLayout(false);
            this.flpPassportCoupon.PerformLayout();
            this.panelPassportCoupon.ResumeLayout(false);
            this.passportCouponHeader.ResumeLayout(false);
            this.panelTotalToPay.ResumeLayout(false);
            this.panelApplyCoupon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Panel panelDiscount;
        private System.Windows.Forms.Panel panelDiscountLine;
        private System.Windows.Forms.Label txtDiscAmount;
        private System.Windows.Forms.Label txtDiscPerc;
        private System.Windows.Forms.Label txtDiscountName;
        private System.Windows.Forms.Label lblHeaderDiscNameAmount;
        private System.Windows.Forms.Label lblHeaderDiscNamePercentage;
        private System.Windows.Forms.Label lblHeaderDiscountName;
        private System.Windows.Forms.Panel panelDeposit;
        private System.Windows.Forms.Panel panelDepositLine;
        private System.Windows.Forms.Label lblDepositAmount;
        private System.Windows.Forms.Label lblDepositQty;
        private System.Windows.Forms.Label lblDepositName;
        private System.Windows.Forms.Label lblHeaderCardDepositTotal;
        private System.Windows.Forms.Label lblHeaderCardDepositQty;
        private System.Windows.Forms.Label lblHeaderCardDeposit;
        private System.Windows.Forms.Label lblHeaderCardDepositPrice;
        private System.Windows.Forms.Label lblDepositPrice;
        private System.Windows.Forms.Label lblHeaderCardDepositTax;
        private System.Windows.Forms.Label lblCardDepositTax;
        private System.Windows.Forms.FlowLayoutPanel flpCheckInSummary;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnProceed;
        private System.Windows.Forms.Button btnApplyCoupon;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblTotalToPayText;
        private System.Windows.Forms.Label lblTotalToPayValue;
        private System.Windows.Forms.Panel panelTotalToPay;
        private System.Windows.Forms.Label lblTotalToPayLowerBar;
        private System.Windows.Forms.Label lblTotalToPayUpperBar;
        private System.Windows.Forms.Panel panelApplyCoupon;
        internal System.Windows.Forms.Label lblProductCPValidityMsg;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.FlowLayoutPanel flpSummaryDetails;
        private System.Windows.Forms.Label lblPackageName;
        private System.Windows.Forms.Label lblPackageCardnumber;
        private System.Windows.Forms.Label lblPackageTotal;
        private System.Windows.Forms.Panel panelSummaryHeader;
        private System.Windows.Forms.Label lblTaxHeader;
        private System.Windows.Forms.Label lblCardNumHeader;
        private System.Windows.Forms.Label lblTotalHeader;
        private System.Windows.Forms.Label lblPackageHeader;
        private System.Windows.Forms.Label lblPriceHeader;
        private System.Windows.Forms.Label lblQtyHeader;
        internal System.Windows.Forms.Label lblGreeting;
        private System.Windows.Forms.Label lblPackageQty;
        private System.Windows.Forms.Label lblPackagePrice;
        private System.Windows.Forms.Label lblPackageTax;
        private System.Windows.Forms.Panel panelPassportCoupon;
        private System.Windows.Forms.Label lblPassportCoupon;
        private System.Windows.Forms.FlowLayoutPanel flpPassportCoupon;
        private System.Windows.Forms.Panel passportCouponHeader;
        private System.Windows.Forms.Label CouponTotal;
        private System.Windows.Forms.Label CouponName;
        private System.Windows.Forms.Label CouponDiscount;
        private System.Windows.Forms.Label CouponTax;
        private System.Windows.Forms.Label CouponQty;
        private System.Windows.Forms.Label CouponPrice;
        private System.Windows.Forms.Label lblStar;
        //private System.Windows.Forms.Button btnHome;
    }
}
