namespace Parafait_Kiosk
{
    partial class frmCashTransaction
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
            this.TimerMoney = new System.Windows.Forms.Timer(this.components);
            this.panelAmountPaid = new System.Windows.Forms.Panel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCreditCard = new System.Windows.Forms.Button();
            this.exitTimer = new System.Windows.Forms.Timer(this.components);
            this.txtMessage = new System.Windows.Forms.Button();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.lblTimeOut = new System.Windows.Forms.Button();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPackage = new System.Windows.Forms.Label();
            this.lblCardnumber = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblTax = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnDebitCard = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Label9 = new System.Windows.Forms.Label();
            this.lblBal = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.lblPaid = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.lblTotalToPay = new System.Windows.Forms.Label();
            this.pbDownArrow = new System.Windows.Forms.PictureBox();
            this.lblNoChange = new System.Windows.Forms.Label();
            this.lblProductCPValidityMsg = new System.Windows.Forms.Label();
            this.panelAmountPaid.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDownArrow)).BeginInit();
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
            this.btnHome.TabIndex = 1070;
            // 
            // TimerMoney
            // 
            this.TimerMoney.Tick += new System.EventHandler(this.TimerMoney_Tick);
            // 
            // panelAmountPaid
            // 
            this.panelAmountPaid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAmountPaid.BackColor = System.Drawing.Color.Transparent;
            this.panelAmountPaid.Controls.Add(this.flowLayoutPanel3);
            this.panelAmountPaid.Controls.Add(this.flowLayoutPanel2);
            this.panelAmountPaid.Controls.Add(this.flowLayoutPanel1);
            this.panelAmountPaid.ForeColor = System.Drawing.Color.White;
            this.panelAmountPaid.Location = new System.Drawing.Point(256, 480);
            this.panelAmountPaid.Name = "panelAmountPaid";
            this.panelAmountPaid.Size = new System.Drawing.Size(64, 52);
            this.panelAmountPaid.TabIndex = 124;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(4, 16);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(641, 50);
            this.flowLayoutPanel3.TabIndex = 125;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(4, 68);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(641, 52);
            this.flowLayoutPanel2.TabIndex = 124;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 122);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(641, 56);
            this.flowLayoutPanel1.TabIndex = 123;
            // 
            // btnCreditCard
            // 
            this.btnCreditCard.BackColor = System.Drawing.Color.Transparent;
            this.btnCreditCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCreditCard.FlatAppearance.BorderSize = 0;
            this.btnCreditCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCreditCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCreditCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreditCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreditCard.ForeColor = System.Drawing.Color.White;
            this.btnCreditCard.Location = new System.Drawing.Point(337, 1540);
            this.btnCreditCard.Name = "btnCreditCard";
            this.btnCreditCard.Size = new System.Drawing.Size(343, 194);
            this.btnCreditCard.TabIndex = 143;
            this.btnCreditCard.TabStop = false;
            this.btnCreditCard.Text = "Credit";
            this.btnCreditCard.UseVisualStyleBackColor = false;
            this.btnCreditCard.Visible = false;
            this.btnCreditCard.Click += new System.EventHandler(this.btnCreditCard_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.bottom_bar;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 703);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1280, 38);
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
            this.lblSiteName.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(12, 10);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1262, 82);
            this.lblSiteName.TabIndex = 137;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            this.lblSiteName.Visible = false;
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
            this.lblTimeOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOut.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblTimeOut.Location = new System.Drawing.Point(1114, 28);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(142, 110);
            this.lblTimeOut.TabIndex = 141;
            this.lblTimeOut.Text = "Top-Up in Progress";
            this.lblTimeOut.UseVisualStyleBackColor = false;
            // 
            // panelSummary
            // 
            this.panelSummary.BackColor = System.Drawing.Color.Transparent;
            this.panelSummary.Controls.Add(this.label5);
            this.panelSummary.Controls.Add(this.flowLayoutPanel4);
            this.panelSummary.Controls.Add(this.label10);
            this.panelSummary.Controls.Add(this.label6);
            this.panelSummary.Controls.Add(this.label11);
            this.panelSummary.Controls.Add(this.label12);
            this.panelSummary.Location = new System.Drawing.Point(56, 394);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(115, 80);
            this.panelSummary.TabIndex = 1067;
            this.panelSummary.Visible = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(269, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 36);
            this.label5.TabIndex = 1060;
            this.label5.Text = "Card #";
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.flowLayoutPanel4.Controls.Add(this.lblPackage);
            this.flowLayoutPanel4.Controls.Add(this.lblCardnumber);
            this.flowLayoutPanel4.Controls.Add(this.lblQuantity);
            this.flowLayoutPanel4.Controls.Add(this.lblPrice);
            this.flowLayoutPanel4.Controls.Add(this.lblTax);
            this.flowLayoutPanel4.Controls.Add(this.lblTotal);
            this.flowLayoutPanel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flowLayoutPanel4.ForeColor = System.Drawing.Color.Black;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(17, 56);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(992, 350);
            this.flowLayoutPanel4.TabIndex = 1058;
            // 
            // lblPackage
            // 
            this.lblPackage.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackage.ForeColor = System.Drawing.Color.Thistle;
            this.lblPackage.Location = new System.Drawing.Point(20, 20);
            this.lblPackage.Margin = new System.Windows.Forms.Padding(20, 20, 3, 0);
            this.lblPackage.Name = "lblPackage";
            this.lblPackage.Size = new System.Drawing.Size(226, 313);
            this.lblPackage.TabIndex = 0;
            this.lblPackage.Text = "$30\r\n(120 Points\r\nIncludes 30 Free)";
            // 
            // lblCardnumber
            // 
            this.lblCardnumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardnumber.ForeColor = System.Drawing.Color.Thistle;
            this.lblCardnumber.Location = new System.Drawing.Point(252, 20);
            this.lblCardnumber.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblCardnumber.Name = "lblCardnumber";
            this.lblCardnumber.Size = new System.Drawing.Size(190, 313);
            this.lblCardnumber.TabIndex = 1;
            this.lblCardnumber.Text = "4A0056AE";
            // 
            // lblQuantity
            // 
            this.lblQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantity.ForeColor = System.Drawing.Color.Thistle;
            this.lblQuantity.Location = new System.Drawing.Point(448, 20);
            this.lblQuantity.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(184, 313);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "2 Cards\r\n(60 Points per card)";
            // 
            // lblPrice
            // 
            this.lblPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.ForeColor = System.Drawing.Color.Thistle;
            this.lblPrice.Location = new System.Drawing.Point(638, 20);
            this.lblPrice.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(136, 313);
            this.lblPrice.TabIndex = 3;
            this.lblPrice.Text = "80.00";
            // 
            // lblTax
            // 
            this.lblTax.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTax.ForeColor = System.Drawing.Color.Thistle;
            this.lblTax.Location = new System.Drawing.Point(780, 20);
            this.lblTax.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblTax.Name = "lblTax";
            this.lblTax.Size = new System.Drawing.Size(136, 313);
            this.lblTax.TabIndex = 3;
            this.lblTax.Text = "80.00";
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.Thistle;
            this.lblTotal.Location = new System.Drawing.Point(3, 353);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(132, 313);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "80.00";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(770, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(242, 36);
            this.label10.TabIndex = 1063;
            this.label10.Text = "Total (Inc Tax)";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(37, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(213, 36);
            this.label6.TabIndex = 1059;
            this.label6.Text = "Package";
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(654, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(114, 36);
            this.label11.TabIndex = 1062;
            this.label11.Text = "Price";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(465, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(170, 36);
            this.label12.TabIndex = 1061;
            this.label12.Text = "Qty";
            // 
            // btnDebitCard
            // 
            this.btnDebitCard.BackColor = System.Drawing.Color.Transparent;
            this.btnDebitCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDebitCard.FlatAppearance.BorderSize = 0;
            this.btnDebitCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDebitCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDebitCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDebitCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDebitCard.ForeColor = System.Drawing.Color.White;
            this.btnDebitCard.Location = new System.Drawing.Point(672, 1540);
            this.btnDebitCard.Name = "btnDebitCard";
            this.btnDebitCard.Size = new System.Drawing.Size(343, 194);
            this.btnDebitCard.TabIndex = 1068;
            this.btnDebitCard.TabStop = false;
            this.btnDebitCard.Text = "Debit";
            this.btnDebitCard.UseVisualStyleBackColor = false;
            this.btnDebitCard.Visible = false;
            this.btnDebitCard.Click += new System.EventHandler(this.btnCreditCard_Click);
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
            this.button1.Size = new System.Drawing.Size(1265, 82);
            this.button1.TabIndex = 1071;
            this.button1.Text = "Insert Cash";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // Label9
            // 
            this.Label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Label9.BackColor = System.Drawing.Color.Transparent;
            this.Label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.ForeColor = System.Drawing.Color.White;
            this.Label9.Location = new System.Drawing.Point(723, 282);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(545, 43);
            this.Label9.TabIndex = 1072;
            this.Label9.Text = "Balance to pay";
            this.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBal
            // 
            this.lblBal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBal.BackColor = System.Drawing.Color.Transparent;
            this.lblBal.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBal.ForeColor = System.Drawing.Color.White;
            this.lblBal.Location = new System.Drawing.Point(725, 344);
            this.lblBal.Name = "lblBal";
            this.lblBal.Size = new System.Drawing.Size(351, 43);
            this.lblBal.TabIndex = 1073;
            this.lblBal.Text = "30.00";
            this.lblBal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label8
            // 
            this.Label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.White;
            this.Label8.Location = new System.Drawing.Point(6, 282);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(593, 43);
            this.Label8.TabIndex = 1074;
            this.Label8.Text = "Amount paid";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPaid
            // 
            this.lblPaid.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPaid.BackColor = System.Drawing.Color.Transparent;
            this.lblPaid.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaid.ForeColor = System.Drawing.Color.White;
            this.lblPaid.Location = new System.Drawing.Point(248, 344);
            this.lblPaid.Name = "lblPaid";
            this.lblPaid.Size = new System.Drawing.Size(351, 43);
            this.lblPaid.TabIndex = 1075;
            this.lblPaid.Text = "0.00";
            this.lblPaid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label1
            // 
            this.Label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 35.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(26)))));
            this.Label1.Location = new System.Drawing.Point(212, 152);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(468, 58);
            this.Label1.TabIndex = 1076;
            this.Label1.Text = "Please Insert";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalToPay
            // 
            this.lblTotalToPay.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTotalToPay.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalToPay.Font = new System.Drawing.Font("Microsoft Sans Serif", 35.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalToPay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(26)))));
            this.lblTotalToPay.Location = new System.Drawing.Point(693, 152);
            this.lblTotalToPay.Name = "lblTotalToPay";
            this.lblTotalToPay.Size = new System.Drawing.Size(440, 58);
            this.lblTotalToPay.TabIndex = 1077;
            this.lblTotalToPay.Text = "30.00";
            this.lblTotalToPay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbDownArrow
            // 
            this.pbDownArrow.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pbDownArrow.BackColor = System.Drawing.Color.Transparent;
            this.pbDownArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbDownArrow.Image = global::Parafait_Kiosk.Properties.Resources.InsertCash_Animation;
            this.pbDownArrow.Location = new System.Drawing.Point(496, 413);
            this.pbDownArrow.Name = "pbDownArrow";
            this.pbDownArrow.Size = new System.Drawing.Size(327, 331);
            this.pbDownArrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDownArrow.TabIndex = 1078;
            this.pbDownArrow.TabStop = false;
            // 
            // lblNoChange
            // 
            this.lblNoChange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoChange.BackColor = System.Drawing.Color.Transparent;
            this.lblNoChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoChange.ForeColor = System.Drawing.Color.White;
            this.lblNoChange.Location = new System.Drawing.Point(12, 783);
            this.lblNoChange.Name = "lblNoChange";
            this.lblNoChange.Size = new System.Drawing.Size(1259, 104);
            this.lblNoChange.TabIndex = 1079;
            this.lblNoChange.Text = "No Change or Refund will be given machine only accept $1,$5,$10,$20 bills";
            this.lblNoChange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProductCPValidityMsg
            // 
            this.lblProductCPValidityMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProductCPValidityMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblProductCPValidityMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductCPValidityMsg.ForeColor = System.Drawing.Color.White;
            this.lblProductCPValidityMsg.Location = new System.Drawing.Point(124, 214);
            this.lblProductCPValidityMsg.Name = "lblProductCPValidityMsg";
            this.lblProductCPValidityMsg.Size = new System.Drawing.Size(1009, 70);
            this.lblProductCPValidityMsg.TabIndex = 20015;
            this.lblProductCPValidityMsg.Text = "0.00";
            this.lblProductCPValidityMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmCashTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_Screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1280, 741);
            this.Controls.Add(this.lblProductCPValidityMsg);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.pbDownArrow);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lblTotalToPay);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.lblPaid);
            this.Controls.Add(this.Label9);
            this.Controls.Add(this.lblBal);
            this.Controls.Add(this.btnDebitCard);
            this.Controls.Add(this.btnCreditCard);
            this.Controls.Add(this.lblTimeOut);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panelAmountPaid);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.lblNoChange);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmCashTransaction";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmNewCard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNewcard_FormClosing);
            this.Load += new System.EventHandler(this.frmNewcard_Load);
            this.Controls.SetChildIndex(this.lblNoChange, 0);
            this.Controls.SetChildIndex(this.lblSiteName, 0);
            this.Controls.SetChildIndex(this.panelAmountPaid, 0);
            this.Controls.SetChildIndex(this.button1, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.lblTimeOut, 0);
            this.Controls.SetChildIndex(this.btnCreditCard, 0);
            this.Controls.SetChildIndex(this.btnDebitCard, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.lblBal, 0);
            this.Controls.SetChildIndex(this.Label9, 0);
            this.Controls.SetChildIndex(this.lblPaid, 0);
            this.Controls.SetChildIndex(this.Label8, 0);
            this.Controls.SetChildIndex(this.lblTotalToPay, 0);
            this.Controls.SetChildIndex(this.Label1, 0);
            this.Controls.SetChildIndex(this.pbDownArrow, 0);
            this.Controls.SetChildIndex(this.panelSummary, 0);
            this.Controls.SetChildIndex(this.lblProductCPValidityMsg, 0);
            this.panelAmountPaid.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.flowLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbDownArrow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer TimerMoney;
        private System.Windows.Forms.Panel panelAmountPaid;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Timer exitTimer;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button lblTimeOut;
        private System.Windows.Forms.Button btnCreditCard;
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
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnDebitCard;
        //private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button button1;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.Label lblBal;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label lblPaid;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label lblTotalToPay;
        private System.Windows.Forms.PictureBox pbDownArrow;
        internal System.Windows.Forms.Label lblNoChange;
        internal System.Windows.Forms.Label lblProductCPValidityMsg;
    }
}
