using System.Windows.Forms;

namespace Parafait_Kiosk
{
    partial class frmCardTransaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCardTransaction));
            this.TimerMoney = new System.Windows.Forms.Timer(this.components);
            this.lblTotalToPay = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.lblBal = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.lblPaid = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.panelAmountPaid = new System.Windows.Forms.Panel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.txtMessage = new System.Windows.Forms.Button();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.lblTimeOut = new System.Windows.Forms.Button();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblTax = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPackage = new System.Windows.Forms.Label();
            this.lblCardnumber = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblTaxAmount = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panelDeposit = new System.Windows.Forms.Panel();
            this.lblDHeadTax = new System.Windows.Forms.Label();
            this.flpDepositLine = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDepositName = new System.Windows.Forms.Label();
            this.lblDepositQty = new System.Windows.Forms.Label();
            this.lblDepositPrice = new System.Windows.Forms.Label();
            this.lblDepositTax = new System.Windows.Forms.Label();
            this.lblDepositTotal = new System.Windows.Forms.Label();
            this.lblDHeadTotal = new System.Windows.Forms.Label();
            this.lblDHeadName = new System.Windows.Forms.Label();
            this.lblDHeadPrice = new System.Windows.Forms.Label();
            this.lblDHeadQty = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panelDiscount = new System.Windows.Forms.Panel();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelDiscountInfo = new System.Windows.Forms.Panel();
            this.txtDiscAmount = new System.Windows.Forms.Label();
            this.txtDiscPerc = new System.Windows.Forms.Label();
            this.txtDiscountName = new System.Windows.Forms.Label();
            this.lblDiscAmount = new System.Windows.Forms.Label();
            this.lblDiscPerc = new System.Windows.Forms.Label();
            this.lblDiscountName = new System.Windows.Forms.Label();
            this.lblProductCPValidityMsg = new System.Windows.Forms.Label();
            this.fLPPModes = new System.Windows.Forms.FlowLayoutPanel();
            this.verticalScrollBarView2 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelFund = new System.Windows.Forms.Panel();
            this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelFundInfo = new System.Windows.Forms.Panel();
            this.txtFundName = new System.Windows.Forms.Label();
            this.lblFundName = new System.Windows.Forms.Label();
            this.panelDonation = new System.Windows.Forms.Panel();
            this.flowLayoutPanel8 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelDonationInfo = new System.Windows.Forms.Panel();
            this.txtDonationAmount = new System.Windows.Forms.Label();
            this.txtDonationName = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblDonationName = new System.Windows.Forms.Label();
            this.lblDonationTax = new System.Windows.Forms.Label();
            this.txtDonationTax = new System.Windows.Forms.Label();
            this.panelAmountPaid.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.panelDeposit.SuspendLayout();
            this.flpDepositLine.SuspendLayout();
            this.panelDiscount.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.panelDiscountInfo.SuspendLayout();
            this.flowLayoutPanel6.SuspendLayout();
            this.panelFund.SuspendLayout();
            this.flowLayoutPanel7.SuspendLayout();
            this.panelFundInfo.SuspendLayout();
            this.panelDonation.SuspendLayout();
            this.flowLayoutPanel8.SuspendLayout();
            this.panelDonationInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(15, 226);
            this.btnPrev.Size = new System.Drawing.Size(4, 10);
            this.btnPrev.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(316, 1642);
            this.btnCancel.Size = new System.Drawing.Size(437, 167);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.TabStop = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // TimerMoney
            // 
            this.TimerMoney.Tick += new System.EventHandler(this.TimerMoney_Tick);
            // 
            // lblTotalToPay
            // 
            this.lblTotalToPay.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalToPay.Font = new System.Drawing.Font("Bango Pro", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalToPay.ForeColor = System.Drawing.Color.White;
            this.lblTotalToPay.Location = new System.Drawing.Point(512, 0);
            this.lblTotalToPay.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblTotalToPay.Name = "lblTotalToPay";
            this.lblTotalToPay.Size = new System.Drawing.Size(496, 116);
            this.lblTotalToPay.TabIndex = 116;
            this.lblTotalToPay.Text = "$ 30000.00";
            this.lblTotalToPay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Font = new System.Drawing.Font("Bango Pro", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.Color.White;
            this.Label1.Location = new System.Drawing.Point(3, 0);
            this.Label1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(506, 112);
            this.Label1.TabIndex = 115;
            this.Label1.Text = "Total to pay:";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBal
            // 
            this.lblBal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBal.BackColor = System.Drawing.Color.Transparent;
            this.lblBal.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBal.ForeColor = System.Drawing.Color.White;
            this.lblBal.Location = new System.Drawing.Point(509, 0);
            this.lblBal.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.lblBal.Name = "lblBal";
            this.lblBal.Size = new System.Drawing.Size(421, 43);
            this.lblBal.TabIndex = 122;
            this.lblBal.Text = "30.00";
            this.lblBal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label9
            // 
            this.Label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label9.BackColor = System.Drawing.Color.Transparent;
            this.Label9.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.ForeColor = System.Drawing.Color.White;
            this.Label9.Location = new System.Drawing.Point(3, 0);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(493, 43);
            this.Label9.TabIndex = 121;
            this.Label9.Text = "Balance to pay:";
            this.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPaid
            // 
            this.lblPaid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPaid.BackColor = System.Drawing.Color.Transparent;
            this.lblPaid.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaid.ForeColor = System.Drawing.Color.White;
            this.lblPaid.Location = new System.Drawing.Point(509, 0);
            this.lblPaid.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.lblPaid.Name = "lblPaid";
            this.lblPaid.Size = new System.Drawing.Size(420, 43);
            this.lblPaid.TabIndex = 120;
            this.lblPaid.Text = "0.00";
            this.lblPaid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label8
            // 
            this.Label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.White;
            this.Label8.Location = new System.Drawing.Point(3, 0);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(493, 43);
            this.Label8.TabIndex = 119;
            this.Label8.Text = "Amount paid:";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelAmountPaid
            // 
            this.panelAmountPaid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAmountPaid.BackColor = System.Drawing.Color.Transparent;
            this.panelAmountPaid.Controls.Add(this.flowLayoutPanel3);
            this.panelAmountPaid.Controls.Add(this.flowLayoutPanel2);
            this.panelAmountPaid.Controls.Add(this.flowLayoutPanel1);
            this.panelAmountPaid.ForeColor = System.Drawing.Color.White;
            this.panelAmountPaid.Location = new System.Drawing.Point(31, 855);
            this.panelAmountPaid.Name = "panelAmountPaid";
            this.panelAmountPaid.Size = new System.Drawing.Size(1024, 266);
            this.panelAmountPaid.TabIndex = 124;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowLayoutPanel3.Controls.Add(this.Label1);
            this.flowLayoutPanel3.Controls.Add(this.lblTotalToPay);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(4, 7);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(1016, 123);
            this.flowLayoutPanel3.TabIndex = 125;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowLayoutPanel2.Controls.Add(this.Label8);
            this.flowLayoutPanel2.Controls.Add(this.lblPaid);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(4, 141);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1013, 52);
            this.flowLayoutPanel2.TabIndex = 124;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowLayoutPanel1.Controls.Add(this.Label9);
            this.flowLayoutPanel1.Controls.Add(this.lblBal);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 201);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1012, 56);
            this.flowLayoutPanel1.TabIndex = 123;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Bango Pro", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1817);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1068, 68);
            this.txtMessage.TabIndex = 134;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // lblSiteName
            // 
            this.lblSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSiteName.BackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.BorderSize = 0;
            this.lblSiteName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSiteName.Font = new System.Drawing.Font("Verdana", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(98, 0);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(992, 82);
            this.lblSiteName.TabIndex = 137;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
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
            // lblTimeOut
            // 
            this.lblTimeOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeOut.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.timer_btn1;
            this.lblTimeOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lblTimeOut.FlatAppearance.BorderSize = 0;
            this.lblTimeOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeOut.Font = new System.Drawing.Font("Verdana", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOut.ForeColor = System.Drawing.Color.White;
            this.lblTimeOut.Location = new System.Drawing.Point(883, 132);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(142, 110);
            this.lblTimeOut.TabIndex = 141;
            this.lblTimeOut.Text = "Top-Up in Progress";
            this.lblTimeOut.UseVisualStyleBackColor = false;
            // 
            // panelSummary
            // 
            this.panelSummary.BackColor = System.Drawing.Color.Transparent;
            this.panelSummary.Controls.Add(this.lblTax);
            this.panelSummary.Controls.Add(this.label5);
            this.panelSummary.Controls.Add(this.flowLayoutPanel4);
            this.panelSummary.Controls.Add(this.label10);
            this.panelSummary.Controls.Add(this.label6);
            this.panelSummary.Controls.Add(this.panelDeposit);
            this.panelSummary.Controls.Add(this.label11);
            this.panelSummary.Controls.Add(this.label12);
            this.panelSummary.Location = new System.Drawing.Point(3, 3);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(1012, 318);
            this.panelSummary.TabIndex = 1067;
            // 
            // lblTax
            // 
            this.lblTax.BackColor = System.Drawing.Color.Transparent;
            this.lblTax.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTax.ForeColor = System.Drawing.Color.White;
            this.lblTax.Location = new System.Drawing.Point(744, 16);
            this.lblTax.Name = "lblTax";
            this.lblTax.Size = new System.Drawing.Size(94, 36);
            this.lblTax.TabIndex = 1064;
            this.lblTax.Text = "Tax";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(263, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 36);
            this.label5.TabIndex = 1060;
            this.label5.Text = "Card #";
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel4.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.MoneySummaryPanel;
            this.flowLayoutPanel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowLayoutPanel4.Controls.Add(this.lblPackage);
            this.flowLayoutPanel4.Controls.Add(this.lblCardnumber);
            this.flowLayoutPanel4.Controls.Add(this.lblQuantity);
            this.flowLayoutPanel4.Controls.Add(this.lblPrice);
            this.flowLayoutPanel4.Controls.Add(this.lblTaxAmount);
            this.flowLayoutPanel4.Controls.Add(this.lblTotal);
            this.flowLayoutPanel4.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flowLayoutPanel4.ForeColor = System.Drawing.Color.Black;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(17, 56);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(992, 259);
            this.flowLayoutPanel4.TabIndex = 1058;
            // 
            // lblPackage
            // 
            this.lblPackage.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackage.Location = new System.Drawing.Point(20, 20);
            this.lblPackage.Margin = new System.Windows.Forms.Padding(20, 20, 3, 0);
            this.lblPackage.Name = "lblPackage";
            this.lblPackage.Size = new System.Drawing.Size(199, 219);
            this.lblPackage.TabIndex = 0;
            this.lblPackage.Text = "$30\r\n(120 Points\r\nIncludes 30 Free)";
            // 
            // lblCardnumber
            // 
            this.lblCardnumber.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardnumber.Location = new System.Drawing.Point(225, 20);
            this.lblCardnumber.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblCardnumber.Name = "lblCardnumber";
            this.lblCardnumber.Size = new System.Drawing.Size(183, 219);
            this.lblCardnumber.TabIndex = 1;
            this.lblCardnumber.Text = "4A0056AE";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoEllipsis = true;
            this.lblQuantity.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantity.Location = new System.Drawing.Point(414, 20);
            this.lblQuantity.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(174, 219);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "2 Cards\r\n(60 Points per card)";
            this.lblQuantity.Click += new System.EventHandler(this.lblQuantity_Click);
            // 
            // lblPrice
            // 
            this.lblPrice.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.Location = new System.Drawing.Point(594, 20);
            this.lblPrice.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(124, 219);
            this.lblPrice.TabIndex = 3;
            this.lblPrice.Text = "80.00";
            // 
            // lblTaxAmount
            // 
            this.lblTaxAmount.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxAmount.Location = new System.Drawing.Point(724, 20);
            this.lblTaxAmount.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblTaxAmount.Name = "lblTaxAmount";
            this.lblTaxAmount.Size = new System.Drawing.Size(112, 219);
            this.lblTaxAmount.TabIndex = 6;
            this.lblTaxAmount.Text = "80.00";
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(842, 20);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(138, 219);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "80.00";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(865, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 36);
            this.label10.TabIndex = 1063;
            this.label10.Text = "Total";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(37, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(213, 36);
            this.label6.TabIndex = 1059;
            this.label6.Text = "Package";
            // 
            // panelDeposit
            // 
            this.panelDeposit.BackColor = System.Drawing.Color.Transparent;
            this.panelDeposit.Controls.Add(this.lblDHeadTax);
            this.panelDeposit.Controls.Add(this.flpDepositLine);
            this.panelDeposit.Controls.Add(this.lblDHeadTotal);
            this.panelDeposit.Controls.Add(this.lblDHeadName);
            this.panelDeposit.Controls.Add(this.lblDHeadPrice);
            this.panelDeposit.Controls.Add(this.lblDHeadQty);
            this.panelDeposit.Location = new System.Drawing.Point(0, 298);
            this.panelDeposit.Name = "panelDeposit";
            this.panelDeposit.Size = new System.Drawing.Size(1012, 160);
            this.panelDeposit.TabIndex = 1068;
            // 
            // lblDHeadTax
            // 
            this.lblDHeadTax.BackColor = System.Drawing.Color.Transparent;
            this.lblDHeadTax.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDHeadTax.ForeColor = System.Drawing.Color.White;
            this.lblDHeadTax.Location = new System.Drawing.Point(744, 16);
            this.lblDHeadTax.Name = "lblDHeadTax";
            this.lblDHeadTax.Size = new System.Drawing.Size(94, 36);
            this.lblDHeadTax.TabIndex = 1064;
            this.lblDHeadTax.Text = "Tax";
            // 
            // flpDepositLine
            // 
            this.flpDepositLine.BackColor = System.Drawing.Color.Transparent;
            this.flpDepositLine.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.discountPanel;
            this.flpDepositLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flpDepositLine.Controls.Add(this.lblDepositName);
            this.flpDepositLine.Controls.Add(this.lblDepositQty);
            this.flpDepositLine.Controls.Add(this.lblDepositPrice);
            this.flpDepositLine.Controls.Add(this.lblDepositTax);
            this.flpDepositLine.Controls.Add(this.lblDepositTotal);
            this.flpDepositLine.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpDepositLine.ForeColor = System.Drawing.Color.Black;
            this.flpDepositLine.Location = new System.Drawing.Point(17, 56);
            this.flpDepositLine.Margin = new System.Windows.Forms.Padding(1);
            this.flpDepositLine.Name = "flpDepositLine";
            this.flpDepositLine.Size = new System.Drawing.Size(992, 92);
            this.flpDepositLine.TabIndex = 1058;
            // 
            // lblDepositName
            // 
            this.lblDepositName.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositName.Location = new System.Drawing.Point(20, 10);
            this.lblDepositName.Margin = new System.Windows.Forms.Padding(20, 10, 3, 0);
            this.lblDepositName.Name = "lblDepositName";
            this.lblDepositName.Size = new System.Drawing.Size(380, 52);
            this.lblDepositName.TabIndex = 0;
            this.lblDepositName.Text = "Deposit";
            // 
            // lblDepositQty
            // 
            this.lblDepositQty.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositQty.Location = new System.Drawing.Point(406, 10);
            this.lblDepositQty.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblDepositQty.Name = "lblDepositQty";
            this.lblDepositQty.Size = new System.Drawing.Size(180, 72);
            this.lblDepositQty.TabIndex = 1;
            this.lblDepositQty.Text = "0";
            this.lblDepositQty.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDepositPrice
            // 
            this.lblDepositPrice.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositPrice.Location = new System.Drawing.Point(592, 10);
            this.lblDepositPrice.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblDepositPrice.Name = "lblDepositPrice";
            this.lblDepositPrice.Size = new System.Drawing.Size(124, 72);
            this.lblDepositPrice.TabIndex = 2;
            this.lblDepositPrice.Text = "80.00";
            // 
            // lblDepositTax
            // 
            this.lblDepositTax.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositTax.Location = new System.Drawing.Point(722, 10);
            this.lblDepositTax.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblDepositTax.Name = "lblDepositTax";
            this.lblDepositTax.Size = new System.Drawing.Size(110, 72);
            this.lblDepositTax.TabIndex = 3;
            this.lblDepositTax.Text = "80.00";
            // 
            // lblDepositTotal
            // 
            this.lblDepositTotal.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositTotal.Location = new System.Drawing.Point(838, 10);
            this.lblDepositTotal.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblDepositTotal.Name = "lblDepositTotal";
            this.lblDepositTotal.Size = new System.Drawing.Size(138, 72);
            this.lblDepositTotal.TabIndex = 6;
            this.lblDepositTotal.Text = "80.00";
            // 
            // lblDHeadTotal
            // 
            this.lblDHeadTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblDHeadTotal.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDHeadTotal.ForeColor = System.Drawing.Color.White;
            this.lblDHeadTotal.Location = new System.Drawing.Point(865, 16);
            this.lblDHeadTotal.Name = "lblDHeadTotal";
            this.lblDHeadTotal.Size = new System.Drawing.Size(105, 36);
            this.lblDHeadTotal.TabIndex = 1063;
            this.lblDHeadTotal.Text = "Total";
            // 
            // lblDHeadName
            // 
            this.lblDHeadName.BackColor = System.Drawing.Color.Transparent;
            this.lblDHeadName.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDHeadName.ForeColor = System.Drawing.Color.White;
            this.lblDHeadName.Location = new System.Drawing.Point(37, 16);
            this.lblDHeadName.Name = "lblDHeadName";
            this.lblDHeadName.Size = new System.Drawing.Size(383, 36);
            this.lblDHeadName.TabIndex = 1059;
            this.lblDHeadName.Text = "Card Deposits";
            // 
            // lblDHeadPrice
            // 
            this.lblDHeadPrice.BackColor = System.Drawing.Color.Transparent;
            this.lblDHeadPrice.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDHeadPrice.ForeColor = System.Drawing.Color.White;
            this.lblDHeadPrice.Location = new System.Drawing.Point(606, 16);
            this.lblDHeadPrice.Name = "lblDHeadPrice";
            this.lblDHeadPrice.Size = new System.Drawing.Size(114, 36);
            this.lblDHeadPrice.TabIndex = 1062;
            this.lblDHeadPrice.Text = "Price";
            // 
            // lblDHeadQty
            // 
            this.lblDHeadQty.BackColor = System.Drawing.Color.Transparent;
            this.lblDHeadQty.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDHeadQty.ForeColor = System.Drawing.Color.White;
            this.lblDHeadQty.Location = new System.Drawing.Point(473, 16);
            this.lblDHeadQty.Name = "lblDHeadQty";
            this.lblDHeadQty.Size = new System.Drawing.Size(170, 36);
            this.lblDHeadQty.TabIndex = 1061;
            this.lblDHeadQty.Text = "Qty";
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(606, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(114, 36);
            this.label11.TabIndex = 1062;
            this.label11.Text = "Price";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(473, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(170, 36);
            this.label12.TabIndex = 1061;
            this.label12.Text = "Qty";
            // 
            // panelDiscount
            // 
            this.panelDiscount.BackColor = System.Drawing.Color.Transparent;
            this.panelDiscount.Controls.Add(this.flowLayoutPanel5);
            this.panelDiscount.Controls.Add(this.lblDiscAmount);
            this.panelDiscount.Controls.Add(this.lblDiscPerc);
            this.panelDiscount.Controls.Add(this.lblDiscountName);
            this.panelDiscount.Location = new System.Drawing.Point(3, 327);
            this.panelDiscount.Name = "panelDiscount";
            this.panelDiscount.Size = new System.Drawing.Size(1027, 97);
            this.panelDiscount.TabIndex = 1074;
            this.panelDiscount.Visible = false;
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.discountPanel;
            this.flowLayoutPanel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowLayoutPanel5.Controls.Add(this.panelDiscountInfo);
            this.flowLayoutPanel5.Location = new System.Drawing.Point(17, 40);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(989, 37);
            this.flowLayoutPanel5.TabIndex = 1063;
            // 
            // panelDiscountInfo
            // 
            this.panelDiscountInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelDiscountInfo.Controls.Add(this.txtDiscAmount);
            this.panelDiscountInfo.Controls.Add(this.txtDiscPerc);
            this.panelDiscountInfo.Controls.Add(this.txtDiscountName);
            this.panelDiscountInfo.Location = new System.Drawing.Point(3, 3);
            this.panelDiscountInfo.Name = "panelDiscountInfo";
            this.panelDiscountInfo.Size = new System.Drawing.Size(986, 62);
            this.panelDiscountInfo.TabIndex = 0;
            // 
            // txtDiscAmount
            // 
            this.txtDiscAmount.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiscAmount.Location = new System.Drawing.Point(834, 9);
            this.txtDiscAmount.Name = "txtDiscAmount";
            this.txtDiscAmount.Size = new System.Drawing.Size(80, 18);
            this.txtDiscAmount.TabIndex = 2;
            this.txtDiscAmount.Text = "10";
            // 
            // txtDiscPerc
            // 
            this.txtDiscPerc.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiscPerc.Location = new System.Drawing.Point(404, 9);
            this.txtDiscPerc.Name = "txtDiscPerc";
            this.txtDiscPerc.Size = new System.Drawing.Size(243, 18);
            this.txtDiscPerc.TabIndex = 1;
            this.txtDiscPerc.Text = "10%";
            // 
            // txtDiscountName
            // 
            this.txtDiscountName.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiscountName.Location = new System.Drawing.Point(2, 9);
            this.txtDiscountName.Name = "txtDiscountName";
            this.txtDiscountName.Size = new System.Drawing.Size(389, 17);
            this.txtDiscountName.TabIndex = 0;
            this.txtDiscountName.Text = "Coupon Discount";
            // 
            // lblDiscAmount
            // 
            this.lblDiscAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblDiscAmount.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscAmount.ForeColor = System.Drawing.Color.White;
            this.lblDiscAmount.Location = new System.Drawing.Point(857, 18);
            this.lblDiscAmount.Name = "lblDiscAmount";
            this.lblDiscAmount.Size = new System.Drawing.Size(89, 22);
            this.lblDiscAmount.TabIndex = 1062;
            this.lblDiscAmount.Text = "Amount";
            // 
            // lblDiscPerc
            // 
            this.lblDiscPerc.BackColor = System.Drawing.Color.Transparent;
            this.lblDiscPerc.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscPerc.ForeColor = System.Drawing.Color.White;
            this.lblDiscPerc.Location = new System.Drawing.Point(398, 13);
            this.lblDiscPerc.Name = "lblDiscPerc";
            this.lblDiscPerc.Size = new System.Drawing.Size(204, 24);
            this.lblDiscPerc.TabIndex = 1061;
            this.lblDiscPerc.Text = "Percentage(%)";
            // 
            // lblDiscountName
            // 
            this.lblDiscountName.BackColor = System.Drawing.Color.Transparent;
            this.lblDiscountName.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscountName.ForeColor = System.Drawing.Color.White;
            this.lblDiscountName.Location = new System.Drawing.Point(19, 13);
            this.lblDiscountName.Name = "lblDiscountName";
            this.lblDiscountName.Size = new System.Drawing.Size(297, 20);
            this.lblDiscountName.TabIndex = 1060;
            this.lblDiscountName.Text = "Discount Name";
            // 
            // lblProductCPValidityMsg
            // 
            this.lblProductCPValidityMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProductCPValidityMsg.AutoEllipsis = true;
            this.lblProductCPValidityMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblProductCPValidityMsg.Font = new System.Drawing.Font("Bango Pro", 22F);
            this.lblProductCPValidityMsg.ForeColor = System.Drawing.Color.White;
            this.lblProductCPValidityMsg.Location = new System.Drawing.Point(13, 1216);
            this.lblProductCPValidityMsg.Name = "lblProductCPValidityMsg";
            this.lblProductCPValidityMsg.Size = new System.Drawing.Size(1030, 118);
            this.lblProductCPValidityMsg.TabIndex = 20015;
            this.lblProductCPValidityMsg.Text = "0.00";
            this.lblProductCPValidityMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProductCPValidityMsg.Click += new System.EventHandler(this.lblProductCPValidityMsg_Click);
            // 
            // fLPPModes
            // 
            this.fLPPModes.AutoScroll = true;
            this.fLPPModes.BackColor = System.Drawing.Color.Transparent;
            this.fLPPModes.Location = new System.Drawing.Point(0, 1204);
            this.fLPPModes.Name = "fLPPModes";
            this.fLPPModes.Size = new System.Drawing.Size(1062, 430);
            this.fLPPModes.TabIndex = 20016;
            // 
            // verticalScrollBarView2
            // 
            this.verticalScrollBarView2.AutoHide = true;
            this.verticalScrollBarView2.AutoScroll = true;
            this.verticalScrollBarView2.BackColor = System.Drawing.Color.Transparent;
            this.verticalScrollBarView2.DataGridView = null;
            this.verticalScrollBarView2.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.DownButtonBackgroundImage")));
            this.verticalScrollBarView2.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarView2.Location = new System.Drawing.Point(1034, 1203);
            this.verticalScrollBarView2.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView2.Name = "verticalScrollBarView2";
            this.verticalScrollBarView2.ScrollableControl = this.fLPPModes;
            this.verticalScrollBarView2.ScrollViewer = null;
            this.verticalScrollBarView2.Size = new System.Drawing.Size(52, 433);
            this.verticalScrollBarView2.TabIndex = 20019;
            this.verticalScrollBarView2.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.UpButtonBackgroundImage")));
            this.verticalScrollBarView2.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.UpButtonDisabledBackgroundImage")));
            // 
            // flowLayoutPanel6
            // 
            this.flowLayoutPanel6.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel6.Controls.Add(this.panelSummary);
            this.flowLayoutPanel6.Controls.Add(this.panelDiscount);
            this.flowLayoutPanel6.Controls.Add(this.panelFund);
            this.flowLayoutPanel6.Controls.Add(this.panelDonation);
            this.flowLayoutPanel6.Location = new System.Drawing.Point(19, 245);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Size = new System.Drawing.Size(1049, 632);
            this.flowLayoutPanel6.TabIndex = 20020;
            // 
            // panelFund
            // 
            this.panelFund.BackColor = System.Drawing.Color.Transparent;
            this.panelFund.Controls.Add(this.flowLayoutPanel7);
            this.panelFund.Controls.Add(this.lblFundName);
            this.panelFund.Location = new System.Drawing.Point(3, 430);
            this.panelFund.Name = "panelFund";
            this.panelFund.Size = new System.Drawing.Size(1027, 97);
            this.panelFund.TabIndex = 1075;
            this.panelFund.Visible = false;
            // 
            // flowLayoutPanel7
            // 
            this.flowLayoutPanel7.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.discountPanel;
            this.flowLayoutPanel7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowLayoutPanel7.Controls.Add(this.panelFundInfo);
            this.flowLayoutPanel7.Location = new System.Drawing.Point(17, 40);
            this.flowLayoutPanel7.Name = "flowLayoutPanel7";
            this.flowLayoutPanel7.Size = new System.Drawing.Size(989, 37);
            this.flowLayoutPanel7.TabIndex = 1063;
            // 
            // panelFundInfo
            // 
            this.panelFundInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelFundInfo.Controls.Add(this.txtFundName);
            this.panelFundInfo.Location = new System.Drawing.Point(3, 3);
            this.panelFundInfo.Name = "panelFundInfo";
            this.panelFundInfo.Size = new System.Drawing.Size(986, 62);
            this.panelFundInfo.TabIndex = 0;
            // 
            // txtFundName
            // 
            this.txtFundName.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFundName.Location = new System.Drawing.Point(2, 9);
            this.txtFundName.Name = "txtFundName";
            this.txtFundName.Size = new System.Drawing.Size(389, 17);
            this.txtFundName.TabIndex = 0;
            this.txtFundName.Text = "Fund 1";
            // 
            // lblFundName
            // 
            this.lblFundName.BackColor = System.Drawing.Color.Transparent;
            this.lblFundName.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFundName.ForeColor = System.Drawing.Color.White;
            this.lblFundName.Location = new System.Drawing.Point(19, 13);
            this.lblFundName.Name = "lblFundName";
            this.lblFundName.Size = new System.Drawing.Size(297, 20);
            this.lblFundName.TabIndex = 1060;
            this.lblFundName.Text = "Fund Name";
            // 
            // panelDonation
            // 
            this.panelDonation.BackColor = System.Drawing.Color.Transparent;
            this.panelDonation.Controls.Add(this.lblDonationTax);
            this.panelDonation.Controls.Add(this.flowLayoutPanel8);
            this.panelDonation.Controls.Add(this.label15);
            this.panelDonation.Controls.Add(this.lblDonationName);
            this.panelDonation.Location = new System.Drawing.Point(3, 533);
            this.panelDonation.Name = "panelDonation";
            this.panelDonation.Size = new System.Drawing.Size(1027, 97);
            this.panelDonation.TabIndex = 1076;
            this.panelDonation.Visible = false;
            // 
            // flowLayoutPanel8
            // 
            this.flowLayoutPanel8.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.discountPanel;
            this.flowLayoutPanel8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowLayoutPanel8.Controls.Add(this.panelDonationInfo);
            this.flowLayoutPanel8.Location = new System.Drawing.Point(17, 40);
            this.flowLayoutPanel8.Name = "flowLayoutPanel8";
            this.flowLayoutPanel8.Size = new System.Drawing.Size(989, 37);
            this.flowLayoutPanel8.TabIndex = 1063;
            // 
            // panelDonationInfo
            // 
            this.panelDonationInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelDonationInfo.Controls.Add(this.txtDonationTax);
            this.panelDonationInfo.Controls.Add(this.txtDonationAmount);
            this.panelDonationInfo.Controls.Add(this.txtDonationName);
            this.panelDonationInfo.Location = new System.Drawing.Point(3, 3);
            this.panelDonationInfo.Name = "panelDonationInfo";
            this.panelDonationInfo.Size = new System.Drawing.Size(986, 62);
            this.panelDonationInfo.TabIndex = 0;
            // 
            // txtDonationAmount
            // 
            this.txtDonationAmount.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDonationAmount.Location = new System.Drawing.Point(832, 9);
            this.txtDonationAmount.Name = "txtDonationAmount";
            this.txtDonationAmount.Size = new System.Drawing.Size(49, 18);
            this.txtDonationAmount.TabIndex = 2;
            this.txtDonationAmount.Text = "05";
            // 
            // txtDonationName
            // 
            this.txtDonationName.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDonationName.Location = new System.Drawing.Point(2, 9);
            this.txtDonationName.Name = "txtDonationName";
            this.txtDonationName.Size = new System.Drawing.Size(389, 17);
            this.txtDonationName.TabIndex = 0;
            this.txtDonationName.Text = "Donation 1";
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(855, 16);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(92, 22);
            this.label15.TabIndex = 1062;
            this.label15.Text = "Amount";
            // 
            // lblDonationName
            // 
            this.lblDonationName.BackColor = System.Drawing.Color.Transparent;
            this.lblDonationName.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDonationName.ForeColor = System.Drawing.Color.White;
            this.lblDonationName.Location = new System.Drawing.Point(19, 13);
            this.lblDonationName.Name = "lblDonationName";
            this.lblDonationName.Size = new System.Drawing.Size(297, 20);
            this.lblDonationName.TabIndex = 1060;
            this.lblDonationName.Text = "Your Donations : ";
            // 
            // lblDonationTax
            // 
            this.lblDonationTax.BackColor = System.Drawing.Color.Transparent;
            this.lblDonationTax.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDonationTax.ForeColor = System.Drawing.Color.White;
            this.lblDonationTax.Location = new System.Drawing.Point(401, 13);
            this.lblDonationTax.Name = "lblDonationTax";
            this.lblDonationTax.Size = new System.Drawing.Size(204, 24);
            this.lblDonationTax.TabIndex = 1064;
            this.lblDonationTax.Text = "Tax (Amount)";
            // 
            // txtDonationTax
            // 
            this.txtDonationTax.Font = new System.Drawing.Font("Bango Pro", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDonationTax.Location = new System.Drawing.Point(406, 9);
            this.txtDonationTax.Name = "txtDonationTax";
            this.txtDonationTax.Size = new System.Drawing.Size(150, 18);
            this.txtDonationTax.TabIndex = 3;
            this.txtDonationTax.Text = "0";
            // 
            // frmCardTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1068, 1885);
            this.Controls.Add(this.flowLayoutPanel6);
            this.Controls.Add(this.verticalScrollBarView2);
            this.Controls.Add(this.fLPPModes);
            this.Controls.Add(this.lblProductCPValidityMsg);
            this.Controls.Add(this.lblTimeOut);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.panelAmountPaid);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmCardTransaction";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmNewCard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNewcard_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCardTransaction_FormClosed);
            this.Load += new System.EventHandler(this.frmNewcard_Load);
            this.Controls.SetChildIndex(this.panelAmountPaid, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.lblSiteName, 0);
            this.Controls.SetChildIndex(this.lblTimeOut, 0);
            this.Controls.SetChildIndex(this.lblProductCPValidityMsg, 0);
            this.Controls.SetChildIndex(this.fLPPModes, 0);
            this.Controls.SetChildIndex(this.verticalScrollBarView2, 0);
            this.Controls.SetChildIndex(this.flowLayoutPanel6, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.panelAmountPaid.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.flowLayoutPanel4.ResumeLayout(false);
            this.panelDeposit.ResumeLayout(false);
            this.flpDepositLine.ResumeLayout(false);
            this.panelDiscount.ResumeLayout(false);
            this.flowLayoutPanel5.ResumeLayout(false);
            this.panelDiscountInfo.ResumeLayout(false);
            this.flowLayoutPanel6.ResumeLayout(false);
            this.panelFund.ResumeLayout(false);
            this.flowLayoutPanel7.ResumeLayout(false);
            this.panelFundInfo.ResumeLayout(false);
            this.panelDonation.ResumeLayout(false);
            this.flowLayoutPanel8.ResumeLayout(false);
            this.panelDonationInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Timer TimerMoney;
        internal System.Windows.Forms.Label lblTotalToPay;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label lblBal;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.Label lblPaid;
        internal System.Windows.Forms.Label Label8;
        private System.Windows.Forms.Panel panelAmountPaid;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        //private System.Windows.Forms.Timer exitTimer;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button lblTimeOut;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Label lblPackage;
        private System.Windows.Forms.Label lblCardnumber;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panelDiscount;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.Panel panelDiscountInfo;
        private System.Windows.Forms.Label txtDiscAmount;
        private System.Windows.Forms.Label txtDiscPerc;
        private System.Windows.Forms.Label txtDiscountName;
        private System.Windows.Forms.Label lblDiscAmount;
        private System.Windows.Forms.Label lblDiscPerc;
        private System.Windows.Forms.Label lblDiscountName;
        internal System.Windows.Forms.Label lblProductCPValidityMsg;
        private Label lblTax;
        private Label lblTaxAmount;
        private Panel panelDeposit;
        private Label lblDHeadTax;
        private FlowLayoutPanel flpDepositLine;
        private Label lblDepositName;
        private Label lblDepositQty;
        private Label lblDepositPrice;
        private Label lblDepositTax;
        private Label lblDepositTotal;
        private Label lblDHeadTotal;
        private Label lblDHeadName;
        private Label lblDHeadPrice;
        private Label lblDHeadQty;
        private FlowLayoutPanel fLPPModes;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView2;
        private FlowLayoutPanel flowLayoutPanel6;
        private Panel panelFund;
        private FlowLayoutPanel flowLayoutPanel7;
        private Panel panelFundInfo;
        private Label txtFundName;
        private Label lblFundName;
        private Panel panelDonation;
        private FlowLayoutPanel flowLayoutPanel8;
        private Panel panelDonationInfo;
        private Label txtDonationAmount;
        private Label txtDonationName;
        private Label label15;
        private Label lblDonationName;
        private Label lblDonationTax;
        private Label txtDonationTax;
    }
}