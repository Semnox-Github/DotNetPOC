namespace Parafait_Kiosk
{
    partial class frmKioskTransactionView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKioskTransactionView));
            this.panelKioskTransaction = new System.Windows.Forms.Panel();
            this.vScrollBarGp = new System.Windows.Forms.VScrollBar();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.hScroll = new System.Windows.Forms.HScrollBar();
            this.lblTransaction = new System.Windows.Forms.Label();
            this.dgvKioskTransactions = new System.Windows.Forms.DataGridView();
            this.TransactionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransactionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblGreeting = new System.Windows.Forms.Label();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTrxId = new System.Windows.Forms.Label();
            this.lblTrxId = new System.Windows.Forms.Label();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtFromTimeHrs = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtFromTimeMins = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.cmbFromTimeTT = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cmbToTimeTT = new System.Windows.Forms.ComboBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.txtToTimeMins = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.txtToTimeHrs = new System.Windows.Forms.Label();
            this.lblPosMachines = new System.Windows.Forms.Label();
            this.pnlPosMachines = new System.Windows.Forms.Panel();
            this.cmbPosMachines = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.panelKioskTransaction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKioskTransactions)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.pnlPosMachines.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelKioskTransaction
            // 
            this.panelKioskTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelKioskTransaction.BackColor = System.Drawing.Color.Transparent;
            this.panelKioskTransaction.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.table;
            this.panelKioskTransaction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelKioskTransaction.Controls.Add(this.vScrollBarGp);
            this.panelKioskTransaction.Controls.Add(this.buttonPrev);
            this.panelKioskTransaction.Controls.Add(this.buttonNext);
            this.panelKioskTransaction.Controls.Add(this.hScroll);
            this.panelKioskTransaction.Controls.Add(this.lblTransaction);
            this.panelKioskTransaction.Controls.Add(this.dgvKioskTransactions);
            this.panelKioskTransaction.Location = new System.Drawing.Point(50, 465);
            this.panelKioskTransaction.Name = "panelKioskTransaction";
            this.panelKioskTransaction.Size = new System.Drawing.Size(983, 1159);
            this.panelKioskTransaction.TabIndex = 158;
            // 
            // vScrollBarGp
            // 
            this.vScrollBarGp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBarGp.Location = new System.Drawing.Point(930, 60);
            this.vScrollBarGp.Name = "vScrollBarGp";
            this.vScrollBarGp.Size = new System.Drawing.Size(51, 1005);
            this.vScrollBarGp.TabIndex = 161;
            this.vScrollBarGp.Visible = false;
            this.vScrollBarGp.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBarGp_Scroll);
            // 
            // buttonPrev
            // 
            this.buttonPrev.BackColor = System.Drawing.Color.Transparent;
            this.buttonPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.left_arrow;
            this.buttonPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonPrev.FlatAppearance.BorderSize = 0;
            this.buttonPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPrev.Font = new System.Drawing.Font("Bango Pro", 46F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPrev.ForeColor = System.Drawing.Color.White;
            this.buttonPrev.Location = new System.Drawing.Point(65, 1077);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(70, 68);
            this.buttonPrev.TabIndex = 168;
            this.buttonPrev.UseVisualStyleBackColor = false;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.BackColor = System.Drawing.Color.Transparent;
            this.buttonNext.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.right_arrow;
            this.buttonNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonNext.FlatAppearance.BorderSize = 0;
            this.buttonNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNext.Font = new System.Drawing.Font("Bango Pro", 46F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNext.ForeColor = System.Drawing.Color.White;
            this.buttonNext.Location = new System.Drawing.Point(841, 1077);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(72, 68);
            this.buttonNext.TabIndex = 167;
            this.buttonNext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonNext.UseVisualStyleBackColor = false;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // hScroll
            // 
            this.hScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScroll.Location = new System.Drawing.Point(3, 1013);
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size(977, 52);
            this.hScroll.TabIndex = 162;
            this.hScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScroll_Scroll);
            // 
            // lblTransaction
            // 
            this.lblTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTransaction.BackColor = System.Drawing.Color.Transparent;
            this.lblTransaction.Font = new System.Drawing.Font("Bango Pro", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransaction.ForeColor = System.Drawing.Color.White;
            this.lblTransaction.Location = new System.Drawing.Point(3, 13);
            this.lblTransaction.Name = "lblTransaction";
            this.lblTransaction.Size = new System.Drawing.Size(977, 34);
            this.lblTransaction.TabIndex = 35;
            this.lblTransaction.Text = "Transaction Details";
            this.lblTransaction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvKioskTransactions
            // 
            this.dgvKioskTransactions.AllowUserToAddRows = false;
            this.dgvKioskTransactions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvKioskTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvKioskTransactions.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(204)))), ((int)(((byte)(208)))));
            this.dgvKioskTransactions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvKioskTransactions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKioskTransactions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvKioskTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKioskTransactions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TransactionDate,
            this.TransactionId,
            this.POSName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Bango Pro", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKioskTransactions.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvKioskTransactions.EnableHeadersVisualStyles = false;
            this.dgvKioskTransactions.GridColor = System.Drawing.SystemColors.Control;
            this.dgvKioskTransactions.Location = new System.Drawing.Point(3, 60);
            this.dgvKioskTransactions.Name = "dgvKioskTransactions";
            this.dgvKioskTransactions.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKioskTransactions.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvKioskTransactions.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvKioskTransactions.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvKioskTransactions.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvKioskTransactions.RowTemplate.Height = 93;
            this.dgvKioskTransactions.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKioskTransactions.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvKioskTransactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvKioskTransactions.Size = new System.Drawing.Size(977, 960);
            this.dgvKioskTransactions.TabIndex = 36;
            this.dgvKioskTransactions.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKioskTransactions_CellContentClick);
            // 
            // TransactionDate
            // 
            this.TransactionDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TransactionDate.HeaderText = "Date";
            this.TransactionDate.MinimumWidth = 255;
            this.TransactionDate.Name = "TransactionDate";
            this.TransactionDate.ReadOnly = true;
            this.TransactionDate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TransactionDate.Width = 255;
            // 
            // TransactionId
            // 
            this.TransactionId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TransactionId.HeaderText = "Transaction Id";
            this.TransactionId.MinimumWidth = 230;
            this.TransactionId.Name = "TransactionId";
            this.TransactionId.ReadOnly = true;
            this.TransactionId.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TransactionId.Width = 232;
            // 
            // POSName
            // 
            this.POSName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.POSName.HeaderText = "POS Name";
            this.POSName.MinimumWidth = 280;
            this.POSName.Name = "POSName";
            this.POSName.ReadOnly = true;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Bango Pro", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ForeColor = System.Drawing.Color.White;
            this.btnPrev.Location = new System.Drawing.Point(402, 1648);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(288, 163);
            this.btnPrev.TabIndex = 152;
            this.btnPrev.Text = "Back";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblGreeting
            // 
            this.lblGreeting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreeting.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting.ForeColor = System.Drawing.Color.White;
            this.lblGreeting.Location = new System.Drawing.Point(2, 95);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(1059, 87);
            this.lblGreeting.TabIndex = 143;
            this.lblGreeting.Text = "Kiosk Transaction View";
            this.lblGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGreeting.Visible = false;
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
            this.lblSiteName.Font = new System.Drawing.Font("Verdana", 26.25F, System.Drawing.FontStyle.Bold);
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(12, 2);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1056, 82);
            this.lblSiteName.TabIndex = 142;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            // 
            // txtMessage
            // 
            this.txtMessage.AutoEllipsis = true;
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Bango Pro", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1871);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 49);
            this.txtMessage.TabIndex = 136;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.txtTrxId);
            this.panel1.Location = new System.Drawing.Point(275, 370);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(263, 62);
            this.panel1.TabIndex = 1058;
            // 
            // txtTrxId
            // 
            this.txtTrxId.BackColor = System.Drawing.Color.Transparent;
            this.txtTrxId.Font = new System.Drawing.Font("Bango Pro", 28F);
            this.txtTrxId.ForeColor = System.Drawing.Color.White;
            this.txtTrxId.Location = new System.Drawing.Point(15, 8);
            this.txtTrxId.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtTrxId.Name = "txtTrxId";
            this.txtTrxId.Size = new System.Drawing.Size(237, 48);
            this.txtTrxId.TabIndex = 5;
            this.txtTrxId.Text = "12345678";
            this.txtTrxId.Click += new System.EventHandler(this.txtTrxId_Enter);
            this.txtTrxId.Enter += new System.EventHandler(this.txtTrxId_Enter);
            // 
            // lblTrxId
            // 
            this.lblTrxId.BackColor = System.Drawing.Color.Transparent;
            this.lblTrxId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTrxId.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrxId.ForeColor = System.Drawing.Color.White;
            this.lblTrxId.Location = new System.Drawing.Point(7, 372);
            this.lblTrxId.Name = "lblTrxId";
            this.lblTrxId.Size = new System.Drawing.Size(268, 58);
            this.lblTrxId.TabIndex = 1057;
            this.lblTrxId.Text = "Trx Id#:";
            this.lblTrxId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFromDate
            // 
            this.lblFromDate.BackColor = System.Drawing.Color.Transparent;
            this.lblFromDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFromDate.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.ForeColor = System.Drawing.Color.White;
            this.lblFromDate.Location = new System.Drawing.Point(7, 207);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(268, 58);
            this.lblFromDate.TabIndex = 1059;
            this.lblFromDate.Text = "From #:";
            this.lblFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Controls.Add(this.txtFromTimeHrs);
            this.panel3.Location = new System.Drawing.Point(275, 205);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(82, 62);
            this.panel3.TabIndex = 1061;
            // 
            // txtFromTimeHrs
            // 
            this.txtFromTimeHrs.BackColor = System.Drawing.Color.Transparent;
            this.txtFromTimeHrs.Font = new System.Drawing.Font("Bango Pro", 28F);
            this.txtFromTimeHrs.ForeColor = System.Drawing.Color.White;
            this.txtFromTimeHrs.Location = new System.Drawing.Point(5, 8);
            this.txtFromTimeHrs.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtFromTimeHrs.Name = "txtFromTimeHrs";
            this.txtFromTimeHrs.Size = new System.Drawing.Size(74, 48);
            this.txtFromTimeHrs.TabIndex = 6;
            this.txtFromTimeHrs.Text = "00";
            this.txtFromTimeHrs.TextChanged += new System.EventHandler(this.txtFromTimeHrs_TextChanged);
            this.txtFromTimeHrs.Click += new System.EventHandler(this.txtFromTimeHrs_Enter);
            this.txtFromTimeHrs.Enter += new System.EventHandler(this.txtFromTimeHrs_Enter);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(540, 207);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 58);
            this.label1.TabIndex = 1062;
            this.label1.Text = "To #:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Bango Pro", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(563, 366);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(160, 70);
            this.btnSearch.TabIndex = 1065;
            this.btnSearch.Text = "Get";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Bango Pro", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.Location = new System.Drawing.Point(747, 366);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(160, 70);
            this.btnClear.TabIndex = 1066;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Bango Pro", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Image = global::Parafait_Kiosk.Properties.Resources.keyboard;
            this.btnShowKeyPad.Location = new System.Drawing.Point(943, 371);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(66, 60);
            this.btnShowKeyPad.TabIndex = 20002;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.btnShowKeyPad_Click);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Transparent;
            this.panel6.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel6.Controls.Add(this.txtFromTimeMins);
            this.panel6.Location = new System.Drawing.Point(358, 205);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(82, 62);
            this.panel6.TabIndex = 20003;
            // 
            // txtFromTimeMins
            // 
            this.txtFromTimeMins.BackColor = System.Drawing.Color.Transparent;
            this.txtFromTimeMins.Font = new System.Drawing.Font("Bango Pro", 28F);
            this.txtFromTimeMins.ForeColor = System.Drawing.Color.White;
            this.txtFromTimeMins.Location = new System.Drawing.Point(5, 7);
            this.txtFromTimeMins.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtFromTimeMins.Name = "txtFromTimeMins";
            this.txtFromTimeMins.Size = new System.Drawing.Size(74, 48);
            this.txtFromTimeMins.TabIndex = 6;
            this.txtFromTimeMins.Text = "00";
            this.txtFromTimeMins.TextChanged += new System.EventHandler(this.txtFromTimeMins_TextChanged);
            this.txtFromTimeMins.Click += new System.EventHandler(this.txtFromTimeMins_Enter);
            this.txtFromTimeMins.Enter += new System.EventHandler(this.txtFromTimeMins_Enter);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Transparent;
            this.panel7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel7.Controls.Add(this.cmbFromTimeTT);
            this.panel7.Location = new System.Drawing.Point(443, 203);
            this.panel7.Margin = new System.Windows.Forms.Padding(0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(90, 65);
            this.panel7.TabIndex = 20004;
            // 
            // cmbFromTimeTT
            // 
            this.cmbFromTimeTT.BackColor = System.Drawing.Color.White;
            this.cmbFromTimeTT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromTimeTT.Font = new System.Drawing.Font("Bango Pro", 28F);
            this.cmbFromTimeTT.ForeColor = System.Drawing.Color.White;
            this.cmbFromTimeTT.Items.AddRange(new object[] {
            "AM",
            "PM"});
            this.cmbFromTimeTT.Location = new System.Drawing.Point(3, 6);
            this.cmbFromTimeTT.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbFromTimeTT.MaxLength = 2;
            this.cmbFromTimeTT.Name = "cmbFromTimeTT";
            this.cmbFromTimeTT.Size = new System.Drawing.Size(88, 52);
            this.cmbFromTimeTT.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Controls.Add(this.cmbToTimeTT);
            this.panel4.Location = new System.Drawing.Point(920, 203);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(90, 65);
            this.panel4.TabIndex = 20007;
            // 
            // cmbToTimeTT
            // 
            this.cmbToTimeTT.BackColor = System.Drawing.Color.White;
            this.cmbToTimeTT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToTimeTT.Font = new System.Drawing.Font("Bango Pro", 28F);
            this.cmbToTimeTT.ForeColor = System.Drawing.Color.White;
            this.cmbToTimeTT.Items.AddRange(new object[] {
            "AM",
            "PM"});
            this.cmbToTimeTT.Location = new System.Drawing.Point(7, 5);
            this.cmbToTimeTT.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbToTimeTT.MaxLength = 2;
            this.cmbToTimeTT.Name = "cmbToTimeTT";
            this.cmbToTimeTT.Size = new System.Drawing.Size(88, 52);
            this.cmbToTimeTT.TabIndex = 6;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.Transparent;
            this.panel8.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel8.Controls.Add(this.txtToTimeMins);
            this.panel8.Location = new System.Drawing.Point(836, 205);
            this.panel8.Margin = new System.Windows.Forms.Padding(0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(84, 62);
            this.panel8.TabIndex = 20006;
            // 
            // txtToTimeMins
            // 
            this.txtToTimeMins.BackColor = System.Drawing.Color.Transparent;
            this.txtToTimeMins.Font = new System.Drawing.Font("Bango Pro", 28F);
            this.txtToTimeMins.ForeColor = System.Drawing.Color.White;
            this.txtToTimeMins.Location = new System.Drawing.Point(6, 8);
            this.txtToTimeMins.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtToTimeMins.Name = "txtToTimeMins";
            this.txtToTimeMins.Size = new System.Drawing.Size(74, 48);
            this.txtToTimeMins.TabIndex = 6;
            this.txtToTimeMins.Text = "00";
            this.txtToTimeMins.TextChanged += new System.EventHandler(this.txtToTimeMins_TextChanged);
            this.txtToTimeMins.Click += new System.EventHandler(this.txtToTimeMins_Enter);
            this.txtToTimeMins.Enter += new System.EventHandler(this.txtToTimeMins_Enter);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Transparent;
            this.panel9.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel9.Controls.Add(this.txtToTimeHrs);
            this.panel9.Location = new System.Drawing.Point(750, 205);
            this.panel9.Margin = new System.Windows.Forms.Padding(0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(84, 62);
            this.panel9.TabIndex = 20005;
            // 
            // txtToTimeHrs
            // 
            this.txtToTimeHrs.BackColor = System.Drawing.Color.Transparent;
            this.txtToTimeHrs.Font = new System.Drawing.Font("Bango Pro", 28F);
            this.txtToTimeHrs.ForeColor = System.Drawing.Color.White;
            this.txtToTimeHrs.Location = new System.Drawing.Point(7, 8);
            this.txtToTimeHrs.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtToTimeHrs.Name = "txtToTimeHrs";
            this.txtToTimeHrs.Size = new System.Drawing.Size(74, 48);
            this.txtToTimeHrs.TabIndex = 6;
            this.txtToTimeHrs.Text = "12";
            this.txtToTimeHrs.TextChanged += new System.EventHandler(this.txtToTimeHrs_TextChanged);
            this.txtToTimeHrs.Click += new System.EventHandler(this.txtToTimeHrs_Enter);
            this.txtToTimeHrs.Enter += new System.EventHandler(this.txtToTimeHrs_Enter);
            // 
            // lblPosMachines
            // 
            this.lblPosMachines.BackColor = System.Drawing.Color.Transparent;
            this.lblPosMachines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPosMachines.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPosMachines.ForeColor = System.Drawing.Color.White;
            this.lblPosMachines.Location = new System.Drawing.Point(7, 290);
            this.lblPosMachines.Name = "lblPosMachines";
            this.lblPosMachines.Size = new System.Drawing.Size(268, 58);
            this.lblPosMachines.TabIndex = 20008;
            this.lblPosMachines.Text = "Machine #:";
            this.lblPosMachines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlPosMachines
            // 
            this.pnlPosMachines.BackColor = System.Drawing.Color.Transparent;
            this.pnlPosMachines.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlPosMachines.Controls.Add(this.cmbPosMachines);
            this.pnlPosMachines.Location = new System.Drawing.Point(275, 287);
            this.pnlPosMachines.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPosMachines.Name = "pnlPosMachines";
            this.pnlPosMachines.Size = new System.Drawing.Size(449, 65);
            this.pnlPosMachines.TabIndex = 20009;
            // 
            // cmbPosMachines
            // 
            this.cmbPosMachines.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbPosMachines.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbPosMachines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPosMachines.Font = new System.Drawing.Font("Bango Pro", 28F);
            this.cmbPosMachines.ForeColor = System.Drawing.Color.White;
            this.cmbPosMachines.FormattingEnabled = true;
            this.cmbPosMachines.Location = new System.Drawing.Point(2, 5);
            this.cmbPosMachines.Name = "cmbPosMachines";
            this.cmbPosMachines.Size = new System.Drawing.Size(446, 52);
            this.cmbPosMachines.TabIndex = 0;
            // 
            // frmKioskTransactionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.pnlPosMachines);
            this.Controls.Add(this.lblPosMachines);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.btnShowKeyPad);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lblFromDate);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTrxId);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.panelKioskTransaction);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblGreeting);
            this.Controls.Add(this.lblSiteName);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmKioskTransactionView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kiosk Transaction View";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.KioskTransactionView_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KioskActivityDetails_KeyPress);
            this.panelKioskTransaction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKioskTransactions)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.pnlPosMachines.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvKioskTransactions;
        private System.Windows.Forms.Label lblTransaction;
        internal System.Windows.Forms.Label lblGreeting;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Panel panelKioskTransaction;
        private System.Windows.Forms.VScrollBar vScrollBarGp;
        private System.Windows.Forms.HScrollBar hScroll;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonPrev;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label lblTrxId;
        internal System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Panel panel3;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label txtTrxId;
        private System.Windows.Forms.Label txtFromTimeHrs;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label txtFromTimeMins;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.ComboBox cmbFromTimeTT;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cmbToTimeTT;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label txtToTimeMins;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label txtToTimeHrs;
        internal System.Windows.Forms.Label lblPosMachines;
        private System.Windows.Forms.Panel pnlPosMachines;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbPosMachines;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn POSName;
    }
}