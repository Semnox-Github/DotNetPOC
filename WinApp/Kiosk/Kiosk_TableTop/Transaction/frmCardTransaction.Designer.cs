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
            //this.TimerMoney = new System.Windows.Forms.Timer(this.components);
            this.exitTimer = new System.Windows.Forms.Timer(this.components);
            this.txtMessage = new System.Windows.Forms.Button();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.lblTimeOut = new System.Windows.Forms.Button();
            this.lblProductCPValidityMsg = new System.Windows.Forms.Label();
            this.pbxPaymentModeAnimation = new System.Windows.Forms.PictureBox();
            this.lblCartProductHeader = new System.Windows.Forms.Label();
            this.lblCartProductAmount = new System.Windows.Forms.Label();
            this.bigVerticalScrollCartProducts = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.flpCartProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlProductSummary = new System.Windows.Forms.Panel();
            this.lblCartProductName = new System.Windows.Forms.Label();
            this.lblCartProductTotal = new System.Windows.Forms.Label();
            this.pnlSummaryInfo = new System.Windows.Forms.Panel();
            this.panelAmountPaid = new System.Windows.Forms.Panel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPaidText = new System.Windows.Forms.Label();
            this.lblPaid = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblBalanceToPayText = new System.Windows.Forms.Label();
            this.lblBal = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.pbxPaymentModeAnimation)).BeginInit();
            this.pnlProductSummary.SuspendLayout();
            this.pnlSummaryInfo.SuspendLayout();
            this.panelAmountPaid.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
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
            this.btnPrev.Location = new System.Drawing.Point(0, 182);
            this.btnPrev.Size = new System.Drawing.Size(80, 14);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(831, 857);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.TabStop = false;
            // 
            // TimerMoney
            // 
            //this.TimerMoney.Tick += new System.EventHandler(this.TimerMoney_Tick);
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
            this.txtMessage.Location = new System.Drawing.Point(0, 1023);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 38);
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
            this.lblSiteName.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(187, 12);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1532, 91);
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
            this.lblTimeOut.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.timer_SmallBox;
            this.lblTimeOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lblTimeOut.FlatAppearance.BorderSize = 0;
            this.lblTimeOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeOut.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOut.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblTimeOut.Location = new System.Drawing.Point(1724, 38);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(152, 117);
            this.lblTimeOut.TabIndex = 141;
            this.lblTimeOut.Text = "Top-Up in Progress";
            this.lblTimeOut.UseVisualStyleBackColor = false;
            // 
            // lblProductCPValidityMsg
            // 
            this.lblProductCPValidityMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProductCPValidityMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblProductCPValidityMsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductCPValidityMsg.ForeColor = System.Drawing.Color.White;
            this.lblProductCPValidityMsg.Location = new System.Drawing.Point(12, 446);
            this.lblProductCPValidityMsg.Name = "lblProductCPValidityMsg";
            this.lblProductCPValidityMsg.Size = new System.Drawing.Size(1896, 73);
            this.lblProductCPValidityMsg.TabIndex = 20015;
            this.lblProductCPValidityMsg.Text = "0.00";
            this.lblProductCPValidityMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbxPaymentModeAnimation
            // 
            this.pbxPaymentModeAnimation.BackColor = System.Drawing.Color.Transparent;
            this.pbxPaymentModeAnimation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxPaymentModeAnimation.Image = global::Parafait_Kiosk.Properties.Resources.InsertCash_Animation;
            this.pbxPaymentModeAnimation.Location = new System.Drawing.Point(1457, 176);
            this.pbxPaymentModeAnimation.Name = "pbxPaymentModeAnimation";
            this.pbxPaymentModeAnimation.Size = new System.Drawing.Size(262, 247);
            this.pbxPaymentModeAnimation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxPaymentModeAnimation.TabIndex = 20026;
            this.pbxPaymentModeAnimation.TabStop = false;
            this.pbxPaymentModeAnimation.Visible = false;
            // 
            // lblCartProductHeader
            // 
            this.lblCartProductHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblCartProductHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCartProductHeader.ForeColor = System.Drawing.Color.White;
            this.lblCartProductHeader.Location = new System.Drawing.Point(3, 3);
            this.lblCartProductHeader.Name = "lblCartProductHeader";
            this.lblCartProductHeader.Size = new System.Drawing.Size(562, 36);
            this.lblCartProductHeader.TabIndex = 1059;
            this.lblCartProductHeader.Text = "Product";
            this.lblCartProductHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCartProductHeader.Visible = false;
            // 
            // lblCartProductAmount
            // 
            this.lblCartProductAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblCartProductAmount.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCartProductAmount.ForeColor = System.Drawing.Color.White;
            this.lblCartProductAmount.Location = new System.Drawing.Point(557, 3);
            this.lblCartProductAmount.Name = "lblCartProductAmount";
            this.lblCartProductAmount.Size = new System.Drawing.Size(276, 36);
            this.lblCartProductAmount.TabIndex = 1063;
            this.lblCartProductAmount.Text = "Total";
            this.lblCartProductAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCartProductAmount.Visible = false;
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
            this.bigVerticalScrollCartProducts.Size = new System.Drawing.Size(63, 216);
            this.bigVerticalScrollCartProducts.TabIndex = 1084;
            this.bigVerticalScrollCartProducts.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollCartProducts.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollCartProducts.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollCartProducts.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // flpCartProducts
            // 
            this.flpCartProducts.AutoScroll = true;
            this.flpCartProducts.BackColor = System.Drawing.Color.Transparent;
            this.flpCartProducts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flpCartProducts.Location = new System.Drawing.Point(20, 45);
            this.flpCartProducts.Margin = new System.Windows.Forms.Padding(0);
            this.flpCartProducts.Name = "flpCartProducts";
            this.flpCartProducts.Size = new System.Drawing.Size(862, 216);
            this.flpCartProducts.TabIndex = 1080;
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
            this.pnlProductSummary.Location = new System.Drawing.Point(485, 131);
            this.pnlProductSummary.Name = "pnlProductSummary";
            this.pnlProductSummary.Size = new System.Drawing.Size(953, 307);
            this.pnlProductSummary.TabIndex = 20027;
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
            // pnlSummaryInfo
            // 
            this.pnlSummaryInfo.BackColor = System.Drawing.Color.Transparent;
            this.pnlSummaryInfo.Controls.Add(this.panelAmountPaid);
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
            this.pnlSummaryInfo.Location = new System.Drawing.Point(216, 522);
            this.pnlSummaryInfo.Name = "pnlSummaryInfo";
            this.pnlSummaryInfo.Size = new System.Drawing.Size(1491, 306);
            this.pnlSummaryInfo.TabIndex = 20028;
            // 
            // panelAmountPaid
            // 
            this.panelAmountPaid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAmountPaid.BackColor = System.Drawing.Color.Transparent;
            this.panelAmountPaid.Controls.Add(this.flowLayoutPanel2);
            this.panelAmountPaid.Controls.Add(this.flowLayoutPanel1);
            this.panelAmountPaid.ForeColor = System.Drawing.Color.White;
            this.panelAmountPaid.Location = new System.Drawing.Point(391, 5);
            this.panelAmountPaid.Name = "panelAmountPaid";
            this.panelAmountPaid.Size = new System.Drawing.Size(1044, 114);
            this.panelAmountPaid.TabIndex = 1079;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowLayoutPanel2.Controls.Add(this.lblPaidText);
            this.flowLayoutPanel2.Controls.Add(this.lblPaid);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(4, 4);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(730, 52);
            this.flowLayoutPanel2.TabIndex = 124;
            // 
            // lblPaidText
            // 
            this.lblPaidText.BackColor = System.Drawing.Color.Transparent;
            this.lblPaidText.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaidText.ForeColor = System.Drawing.Color.White;
            this.lblPaidText.Location = new System.Drawing.Point(3, 0);
            this.lblPaidText.Name = "lblPaidText";
            this.lblPaidText.Size = new System.Drawing.Size(377, 43);
            this.lblPaidText.TabIndex = 119;
            this.lblPaidText.Text = "Amount paid:";
            this.lblPaidText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPaid
            // 
            this.lblPaid.BackColor = System.Drawing.Color.Transparent;
            this.lblPaid.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaid.ForeColor = System.Drawing.Color.White;
            this.lblPaid.Location = new System.Drawing.Point(386, 0);
            this.lblPaid.Name = "lblPaid";
            this.lblPaid.Size = new System.Drawing.Size(340, 43);
            this.lblPaid.TabIndex = 120;
            this.lblPaid.Text = "0.00";
            this.lblPaid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowLayoutPanel1.Controls.Add(this.lblBalanceToPayText);
            this.flowLayoutPanel1.Controls.Add(this.lblBal);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 58);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(730, 52);
            this.flowLayoutPanel1.TabIndex = 123;
            // 
            // lblBalanceToPayText
            // 
            this.lblBalanceToPayText.BackColor = System.Drawing.Color.Transparent;
            this.lblBalanceToPayText.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalanceToPayText.ForeColor = System.Drawing.Color.White;
            this.lblBalanceToPayText.Location = new System.Drawing.Point(3, 0);
            this.lblBalanceToPayText.Name = "lblBalanceToPayText";
            this.lblBalanceToPayText.Size = new System.Drawing.Size(377, 43);
            this.lblBalanceToPayText.TabIndex = 121;
            this.lblBalanceToPayText.Text = "Balance to pay:";
            this.lblBalanceToPayText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBal
            // 
            this.lblBal.BackColor = System.Drawing.Color.Transparent;
            this.lblBal.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBal.ForeColor = System.Drawing.Color.White;
            this.lblBal.Location = new System.Drawing.Point(386, 0);
            this.lblBal.Name = "lblBal";
            this.lblBal.Size = new System.Drawing.Size(340, 43);
            this.lblBal.TabIndex = 122;
            this.lblBal.Text = "30.00";
            this.lblBal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDiscountInfo
            // 
            this.lblDiscountInfo.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscountInfo.ForeColor = System.Drawing.Color.White;
            this.lblDiscountInfo.Location = new System.Drawing.Point(2, 122);
            this.lblDiscountInfo.Name = "lblDiscountInfo";
            this.lblDiscountInfo.Size = new System.Drawing.Size(1486, 57);
            this.lblDiscountInfo.TabIndex = 1078;
            this.lblDiscountInfo.Text = "Discount Info";
            this.lblDiscountInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.Font = new System.Drawing.Font("Gotham Rounded Bold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrandTotal.ForeColor = System.Drawing.Color.White;
            this.lblGrandTotal.Location = new System.Drawing.Point(1153, 247);
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
            this.lblChargeAmountHeader.Location = new System.Drawing.Point(337, 181);
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
            this.lblDiscount.Location = new System.Drawing.Point(881, 247);
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
            this.lblTax.Location = new System.Drawing.Point(609, 247);
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
            this.lblCharges.Location = new System.Drawing.Point(337, 247);
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
            this.lblTotal.Location = new System.Drawing.Point(65, 247);
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
            this.lblGrandTotalHeader.Location = new System.Drawing.Point(1153, 181);
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
            this.lblTaxesHeader.Location = new System.Drawing.Point(609, 181);
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
            this.lblDiscountAmountHeader.Location = new System.Drawing.Point(881, 181);
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
            this.lblTotalHeader.Location = new System.Drawing.Point(65, 181);
            this.lblTotalHeader.Name = "lblTotalHeader";
            this.lblTotalHeader.Size = new System.Drawing.Size(266, 57);
            this.lblTotalHeader.TabIndex = 1;
            this.lblTotalHeader.Text = "Total";
            this.lblTotalHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmCardTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.lblTimeOut);
            this.Controls.Add(this.pnlSummaryInfo);
            this.Controls.Add(this.lblProductCPValidityMsg);
            this.Controls.Add(this.pnlProductSummary);
            this.Controls.Add(this.pbxPaymentModeAnimation);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.txtMessage);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmCardTransaction";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCardTransaction";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNewcard_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCardTransaction_FormClosed);
            this.Load += new System.EventHandler(this.frmCardTransaction_Load);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.lblSiteName, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.pbxPaymentModeAnimation, 0);
            this.Controls.SetChildIndex(this.pnlProductSummary, 0);
            this.Controls.SetChildIndex(this.lblProductCPValidityMsg, 0);
            this.Controls.SetChildIndex(this.pnlSummaryInfo, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.lblTimeOut, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbxPaymentModeAnimation)).EndInit();
            this.pnlProductSummary.ResumeLayout(false);
            this.pnlSummaryInfo.ResumeLayout(false);
            this.panelAmountPaid.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.Timer TimerMoney;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Timer exitTimer;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button lblTimeOut;
        internal System.Windows.Forms.Label lblProductCPValidityMsg;
        private System.Windows.Forms.PictureBox pbxPaymentModeAnimation;
        private System.Windows.Forms.Label lblCartProductHeader;
        private System.Windows.Forms.Label lblCartProductAmount;
        //private System.Windows.Forms.FlowLayoutPanel flpCartSummaryTable;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollCartProducts;
        private System.Windows.Forms.Panel pnlProductSummary;
        private System.Windows.Forms.Label lblCartProductName;
        private System.Windows.Forms.Label lblCartProductTotal;
        //private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollBarView1;
        private System.Windows.Forms.FlowLayoutPanel flpCartProducts;
        private System.Windows.Forms.Panel pnlSummaryInfo;
        private System.Windows.Forms.Panel panelAmountPaid;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        internal System.Windows.Forms.Label lblPaidText;
        internal System.Windows.Forms.Label lblPaid;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        internal System.Windows.Forms.Label lblBalanceToPayText;
        internal System.Windows.Forms.Label lblBal;
        private System.Windows.Forms.Label lblDiscountInfo;
        private System.Windows.Forms.Label lblGrandTotal;
        private System.Windows.Forms.Label lblChargeAmountHeader;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.Label lblCharges;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblGrandTotalHeader;
        private System.Windows.Forms.Label lblTaxesHeader;
        private System.Windows.Forms.Label lblDiscountAmountHeader;
        private System.Windows.Forms.Label lblTotalHeader;
        //private System.Windows.Forms.Button btnHome;
    }
}
