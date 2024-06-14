namespace Parafait_POS.Redemption
{
    partial class frmRedemptionAdmin
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblManualTickets = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lnkPrintTotalPhysial = new System.Windows.Forms.LinkLabel();
            this.lblTotalPhysicalTickets = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtManualTickets = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lnkPrintReal = new System.Windows.Forms.LinkLabel();
            this.dgvCurrencies = new System.Windows.Forms.DataGridView();
            this.dataGridViewButtonColumn1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.CurrencyRule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcCurrencyId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcCurQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcReduceCurrency = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dcIIncreaseCurrency = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvCards = new System.Windows.Forms.DataGridView();
            this.dcRemoveCard = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dcCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcCardTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lnkPrintVoucher = new System.Windows.Forms.LinkLabel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvVouchers = new System.Windows.Forms.DataGridView();
            this.dcRemoveVoucher = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dcVoucher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lnkPrintCurrencies = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnReprintTicket = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrencies)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCards)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVouchers)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.BackColor = System.Drawing.Color.Transparent;
            this.btnAdvanced.BackgroundImage = global::Parafait_POS.Properties.Resources.CheckInCheckOut;
            this.btnAdvanced.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAdvanced.FlatAppearance.BorderSize = 0;
            this.btnAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdvanced.ForeColor = System.Drawing.Color.White;
            this.btnAdvanced.Location = new System.Drawing.Point(5, 5);
            this.btnAdvanced.Margin = new System.Windows.Forms.Padding(5);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(92, 57);
            this.btnAdvanced.TabIndex = 0;
            this.btnAdvanced.Text = "Advanced";
            this.btnAdvanced.UseVisualStyleBackColor = false;
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lblManualTickets);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.txtManualTickets);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1138, 436);
            this.panel1.TabIndex = 1;
            // 
            // lblManualTickets
            // 
            this.lblManualTickets.Location = new System.Drawing.Point(22, 356);
            this.lblManualTickets.Name = "lblManualTickets";
            this.lblManualTickets.Size = new System.Drawing.Size(106, 18);
            this.lblManualTickets.TabIndex = 4;
            this.lblManualTickets.Text = "Manual Tickets";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.lnkPrintTotalPhysial);
            this.panel2.Controls.Add(this.lblTotalPhysicalTickets);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Location = new System.Drawing.Point(21, 381);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(363, 30);
            this.panel2.TabIndex = 41;
            // 
            // lnkPrintTotalPhysial
            // 
            this.lnkPrintTotalPhysial.Location = new System.Drawing.Point(272, 8);
            this.lnkPrintTotalPhysial.Name = "lnkPrintTotalPhysial";
            this.lnkPrintTotalPhysial.Size = new System.Drawing.Size(87, 18);
            this.lnkPrintTotalPhysial.TabIndex = 42;
            this.lnkPrintTotalPhysial.TabStop = true;
            this.lnkPrintTotalPhysial.Text = "Print Receipt";
            this.lnkPrintTotalPhysial.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPrintTotalPhysial_LinkClicked);
            // 
            // lblTotalPhysicalTickets
            // 
            this.lblTotalPhysicalTickets.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPhysicalTickets.Location = new System.Drawing.Point(204, -2);
            this.lblTotalPhysicalTickets.Name = "lblTotalPhysicalTickets";
            this.lblTotalPhysicalTickets.Size = new System.Drawing.Size(65, 34);
            this.lblTotalPhysicalTickets.TabIndex = 41;
            this.lblTotalPhysicalTickets.Text = "9999";
            this.lblTotalPhysicalTickets.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, -3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(199, 34);
            this.label5.TabIndex = 40;
            this.label5.Text = "Total Physical Tickets:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtManualTickets
            // 
            this.txtManualTickets.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtManualTickets.Location = new System.Drawing.Point(134, 343);
            this.txtManualTickets.Name = "txtManualTickets";
            this.txtManualTickets.Size = new System.Drawing.Size(80, 31);
            this.txtManualTickets.TabIndex = 5;
            this.txtManualTickets.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtManualTickets.Enter += new System.EventHandler(this.txtManualTickets_Enter);
            this.txtManualTickets.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtManualTickets_KeyPress);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(595, 334);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(633, 331);
            this.panel3.TabIndex = 42;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 18);
            this.label4.TabIndex = 37;
            this.label4.Text = "Currencies";
            // 
            // panel6
            // 
            this.panel6.AutoScroll = true;
            this.panel6.Controls.Add(this.lnkPrintReal);
            this.panel6.Controls.Add(this.dgvCurrencies);
            this.panel6.Location = new System.Drawing.Point(4, 189);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(585, 142);
            this.panel6.TabIndex = 44;
            // 
            // lnkPrintReal
            // 
            this.lnkPrintReal.Location = new System.Drawing.Point(15, 115);
            this.lnkPrintReal.Name = "lnkPrintReal";
            this.lnkPrintReal.Size = new System.Drawing.Size(233, 18);
            this.lnkPrintReal.TabIndex = 35;
            this.lnkPrintReal.TabStop = true;
            this.lnkPrintReal.Text = "Print Receipt";
            this.lnkPrintReal.Visible = false;
            // 
            // dgvCurrencies
            // 
            this.dgvCurrencies.AllowUserToAddRows = false;
            this.dgvCurrencies.AllowUserToDeleteRows = false;
            this.dgvCurrencies.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvCurrencies.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCurrencies.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Salmon;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCurrencies.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCurrencies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCurrencies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewButtonColumn1,
            this.CurrencyRule,
            this.dcCurrencyId,
            this.dataGridViewTextBoxColumn1,
            this.dcRate,
            this.dcCurQuantity,
            this.dataGridViewTextBoxColumn3,
            this.dcReduceCurrency,
            this.dcIIncreaseCurrency});
            this.dgvCurrencies.EnableHeadersVisualStyles = false;
            this.dgvCurrencies.Location = new System.Drawing.Point(15, 3);
            this.dgvCurrencies.Name = "dgvCurrencies";
            this.dgvCurrencies.RowHeadersVisible = false;
            this.dgvCurrencies.Size = new System.Drawing.Size(484, 27);
            this.dgvCurrencies.TabIndex = 38;
            this.dgvCurrencies.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCurrencies_CellClick);
            this.dgvCurrencies.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCurrencies_CellEnter);
            this.dgvCurrencies.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvCurrencies_Scroll);
            // 
            // dataGridViewButtonColumn1
            // 
            this.dataGridViewButtonColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewButtonColumn1.HeaderText = "X";
            this.dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            this.dataGridViewButtonColumn1.Text = "X";
            this.dataGridViewButtonColumn1.ToolTipText = "Remove";
            this.dataGridViewButtonColumn1.UseColumnTextForButtonValue = true;
            this.dataGridViewButtonColumn1.Width = 21;
            // 
            // CurrencyRule
            // 
            this.CurrencyRule.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CurrencyRule.HeaderText = "CurrencyRule";
            this.CurrencyRule.Name = "CurrencyRule";
            this.CurrencyRule.Width = 95;
            // 
            // dcCurrencyId
            // 
            this.dcCurrencyId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcCurrencyId.HeaderText = "curId";
            this.dcCurrencyId.Name = "dcCurrencyId";
            this.dcCurrencyId.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 95;
            // 
            // dcRate
            // 
            this.dcRate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcRate.HeaderText = "Rate";
            this.dcRate.Name = "dcRate";
            this.dcRate.Width = 60;
            // 
            // dcCurQuantity
            // 
            this.dcCurQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcCurQuantity.HeaderText = "Quantity";
            this.dcCurQuantity.Name = "dcCurQuantity";
            this.dcCurQuantity.Width = 60;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.HeaderText = "Tickets";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // dcReduceCurrency
            // 
            this.dcReduceCurrency.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcReduceCurrency.HeaderText = "-";
            this.dcReduceCurrency.Name = "dcReduceCurrency";
            this.dcReduceCurrency.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dcReduceCurrency.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dcReduceCurrency.Text = "-";
            this.dcReduceCurrency.ToolTipText = "Reduce";
            this.dcReduceCurrency.Width = 35;
            // 
            // dcIIncreaseCurrency
            // 
            this.dcIIncreaseCurrency.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcIIncreaseCurrency.HeaderText = "+";
            this.dcIIncreaseCurrency.Name = "dcIIncreaseCurrency";
            this.dcIIncreaseCurrency.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dcIIncreaseCurrency.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dcIIncreaseCurrency.Text = "+";
            this.dcIIncreaseCurrency.ToolTipText = "Increase";
            this.dcIIncreaseCurrency.UseColumnTextForButtonValue = true;
            this.dcIIncreaseCurrency.Width = 35;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Vouchers";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cards";
            // 
            // panel4
            // 
            this.panel4.AutoScroll = true;
            this.panel4.Controls.Add(this.dgvCards);
            this.panel4.Controls.Add(this.lnkPrintVoucher);
            this.panel4.Location = new System.Drawing.Point(4, 25);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(585, 57);
            this.panel4.TabIndex = 42;
            // 
            // dgvCards
            // 
            this.dgvCards.AllowUserToAddRows = false;
            this.dgvCards.AllowUserToDeleteRows = false;
            this.dgvCards.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvCards.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCards.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Salmon;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCards.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCards.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcRemoveCard,
            this.dcCardNumber,
            this.dcCardTickets});
            this.dgvCards.EnableHeadersVisualStyles = false;
            this.dgvCards.Location = new System.Drawing.Point(14, 3);
            this.dgvCards.Name = "dgvCards";
            this.dgvCards.RowHeadersVisible = false;
            this.dgvCards.Size = new System.Drawing.Size(469, 30);
            this.dgvCards.TabIndex = 1;
            this.dgvCards.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCards_CellClick);
            this.dgvCards.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvCards_Scroll);
            // 
            // dcRemoveCard
            // 
            this.dcRemoveCard.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcRemoveCard.HeaderText = "X";
            this.dcRemoveCard.Name = "dcRemoveCard";
            this.dcRemoveCard.Text = "X";
            this.dcRemoveCard.ToolTipText = "Remove";
            this.dcRemoveCard.UseColumnTextForButtonValue = true;
            this.dcRemoveCard.Width = 21;
            // 
            // dcCardNumber
            // 
            this.dcCardNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcCardNumber.HeaderText = "Card Number";
            this.dcCardNumber.Name = "dcCardNumber";
            this.dcCardNumber.Width = 364;
            // 
            // dcCardTickets
            // 
            this.dcCardTickets.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcCardTickets.HeaderText = "Tickets";
            this.dcCardTickets.Name = "dcCardTickets";
            this.dcCardTickets.Width = 76;
            // 
            // lnkPrintVoucher
            // 
            this.lnkPrintVoucher.Location = new System.Drawing.Point(11, 50);
            this.lnkPrintVoucher.Name = "lnkPrintVoucher";
            this.lnkPrintVoucher.Size = new System.Drawing.Size(238, 18);
            this.lnkPrintVoucher.TabIndex = 36;
            this.lnkPrintVoucher.TabStop = true;
            this.lnkPrintVoucher.Text = "Print Receipt";
            this.lnkPrintVoucher.Visible = false;
            // 
            // panel5
            // 
            this.panel5.AutoScroll = true;
            this.panel5.Controls.Add(this.dgvVouchers);
            this.panel5.Controls.Add(this.lnkPrintCurrencies);
            this.panel5.Location = new System.Drawing.Point(4, 106);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(585, 57);
            this.panel5.TabIndex = 43;
            // 
            // dgvVouchers
            // 
            this.dgvVouchers.AllowUserToAddRows = false;
            this.dgvVouchers.AllowUserToDeleteRows = false;
            this.dgvVouchers.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvVouchers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvVouchers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Salmon;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvVouchers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvVouchers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVouchers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcRemoveVoucher,
            this.dcVoucher,
            this.dataGridViewTextBoxColumn2});
            this.dgvVouchers.EnableHeadersVisualStyles = false;
            this.dgvVouchers.Location = new System.Drawing.Point(15, 3);
            this.dgvVouchers.Name = "dgvVouchers";
            this.dgvVouchers.RowHeadersVisible = false;
            this.dgvVouchers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvVouchers.Size = new System.Drawing.Size(469, 29);
            this.dgvVouchers.TabIndex = 3;
            this.dgvVouchers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVouchers_CellClick);
            this.dgvVouchers.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvVouchers_Scroll);
            // 
            // dcRemoveVoucher
            // 
            this.dcRemoveVoucher.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcRemoveVoucher.HeaderText = "X";
            this.dcRemoveVoucher.Name = "dcRemoveVoucher";
            this.dcRemoveVoucher.Text = "X";
            this.dcRemoveVoucher.ToolTipText = "Remove";
            this.dcRemoveVoucher.UseColumnTextForButtonValue = true;
            this.dcRemoveVoucher.Width = 21;
            // 
            // dcVoucher
            // 
            this.dcVoucher.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dcVoucher.HeaderText = "Voucher";
            this.dcVoucher.Name = "dcVoucher";
            this.dcVoucher.Width = 364;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.HeaderText = "Tickets";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 76;
            // 
            // lnkPrintCurrencies
            // 
            this.lnkPrintCurrencies.Location = new System.Drawing.Point(10, 53);
            this.lnkPrintCurrencies.Name = "lnkPrintCurrencies";
            this.lnkPrintCurrencies.Size = new System.Drawing.Size(238, 18);
            this.lnkPrintCurrencies.TabIndex = 39;
            this.lnkPrintCurrencies.TabStop = true;
            this.lnkPrintCurrencies.Text = "Print Receipt";
            this.lnkPrintCurrencies.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.CheckInCheckOut;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(308, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 57);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BackgroundImage = global::Parafait_POS.Properties.Resources.CheckInCheckOut;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(208, 4);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(92, 57);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnReprintTicket
            // 
            this.btnReprintTicket.BackColor = System.Drawing.Color.Transparent;
            this.btnReprintTicket.BackgroundImage = global::Parafait_POS.Properties.Resources.CheckInCheckOut;
            this.btnReprintTicket.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReprintTicket.FlatAppearance.BorderSize = 0;
            this.btnReprintTicket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReprintTicket.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReprintTicket.ForeColor = System.Drawing.Color.White;
            this.btnReprintTicket.Location = new System.Drawing.Point(107, 5);
            this.btnReprintTicket.Margin = new System.Windows.Forms.Padding(5);
            this.btnReprintTicket.Name = "btnReprintTicket";
            this.btnReprintTicket.Size = new System.Drawing.Size(92, 57);
            this.btnReprintTicket.TabIndex = 4;
            this.btnReprintTicket.Text = "Reprint Receipt";
            this.btnReprintTicket.UseVisualStyleBackColor = false;
            this.btnReprintTicket.Click += new System.EventHandler(this.btnReprintTicket_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnAdvanced);
            this.flowLayoutPanel2.Controls.Add(this.btnReprintTicket);
            this.flowLayoutPanel2.Controls.Add(this.btnRefresh);
            this.flowLayoutPanel2.Controls.Add(this.btnClose);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 2);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(422, 65);
            this.flowLayoutPanel2.TabIndex = 2;
            // 
            // frmRedemptionAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1138, 504);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmRedemptionAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Redemption Admin";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRedemptionAdmin_FormClosed);
            this.Load += new System.EventHandler(this.frmRedemptionAdmin_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrencies)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCards)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVouchers)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdvanced;
        private System.Windows.Forms.Panel panel1;
        //private System.Windows.Forms.NumericUpDown nudManualTickets;
        private System.Windows.Forms.TextBox txtManualTickets;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel lnkPrintTotalPhysial;
        private System.Windows.Forms.Label lblTotalPhysicalTickets;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblManualTickets;
        private System.Windows.Forms.LinkLabel lnkPrintReal;
        private System.Windows.Forms.DataGridView dgvCurrencies;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrencyRule;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcCurrencyId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcCurQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewButtonColumn dcReduceCurrency;
        private System.Windows.Forms.DataGridViewButtonColumn dcIIncreaseCurrency;
        private System.Windows.Forms.LinkLabel lnkPrintCurrencies;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvVouchers;
        private System.Windows.Forms.DataGridViewButtonColumn dcRemoveVoucher;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcVoucher;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.LinkLabel lnkPrintVoucher;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvCards;
        private System.Windows.Forms.DataGridViewButtonColumn dcRemoveCard;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcCardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcCardTickets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnReprintTicket;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    }
}