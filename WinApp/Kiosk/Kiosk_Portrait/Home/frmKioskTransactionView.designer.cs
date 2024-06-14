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
            this.bigVerticalScrollKioskTransactions = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.bigHorizontalScrollKioskTransactions = new Semnox.Core.GenericUtilities.BigHorizontalScrollBarView();
            this.lblTransaction = new System.Windows.Forms.Label();
            this.dgvKioskTransactions = new System.Windows.Forms.DataGridView();
            this.TransactionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransactionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransactionNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTransactionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnPrintReceipt = new System.Windows.Forms.Button();
            this.btnRefund = new System.Windows.Forms.Button();
            this.btnPrintPending = new System.Windows.Forms.Button();
            this.btnIssueTempCard = new System.Windows.Forms.Button();
            this.lblGreeting = new System.Windows.Forms.Label();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTrxId = new System.Windows.Forms.TextBox();
            this.lblTrxId = new System.Windows.Forms.Label();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtFromTimeHrs = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtFromTimeMins = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.cmbFromTimeTT = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cmbToTimeTT = new System.Windows.Forms.ComboBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.txtToTimeMins = new System.Windows.Forms.TextBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.txtToTimeHrs = new System.Windows.Forms.TextBox();
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
            this.panelKioskTransaction.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Table1;
            this.panelKioskTransaction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelKioskTransaction.Controls.Add(this.bigVerticalScrollKioskTransactions);
            this.panelKioskTransaction.Controls.Add(this.bigHorizontalScrollKioskTransactions);
            this.panelKioskTransaction.Controls.Add(this.lblTransaction);
            this.panelKioskTransaction.Controls.Add(this.dgvKioskTransactions);
            this.panelKioskTransaction.Location = new System.Drawing.Point(40, 465);
            this.panelKioskTransaction.Name = "panelKioskTransaction";
            this.panelKioskTransaction.Size = new System.Drawing.Size(1004, 1080);
            this.panelKioskTransaction.TabIndex = 158;
            // 
            // bigVerticalScrollKioskTransactions
            // 
            this.bigVerticalScrollKioskTransactions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bigVerticalScrollKioskTransactions.AutoHide = false;
            this.bigVerticalScrollKioskTransactions.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollKioskTransactions.DataGridView = this.dgvKioskTransactions;
            this.bigVerticalScrollKioskTransactions.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollKioskTransactions.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollKioskTransactions.Location = new System.Drawing.Point(930, 60);
            this.bigVerticalScrollKioskTransactions.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.bigVerticalScrollKioskTransactions.Name = "bigVerticalScrollKioskTransactions";
            this.bigVerticalScrollKioskTransactions.ScrollableControl = null;
            this.bigVerticalScrollKioskTransactions.ScrollViewer = null;
            this.bigVerticalScrollKioskTransactions.Size = new System.Drawing.Size(63, 953);
            this.bigVerticalScrollKioskTransactions.TabIndex = 20018;
            this.bigVerticalScrollKioskTransactions.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollKioskTransactions.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollKioskTransactions.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollKioskTransactions.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // bigHorizontalScrollKioskActivity
            // 
            this.bigHorizontalScrollKioskTransactions.AutoHide = false;
            this.bigHorizontalScrollKioskTransactions.BackColor = System.Drawing.SystemColors.Control;
            this.bigHorizontalScrollKioskTransactions.DataGridView = this.dgvKioskTransactions;
            this.bigHorizontalScrollKioskTransactions.LeftButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Left_Button;
            this.bigHorizontalScrollKioskTransactions.LeftButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Left_Button_Disabled;
            this.bigHorizontalScrollKioskTransactions.Location = new System.Drawing.Point(10, 950);
            this.bigHorizontalScrollKioskTransactions.Margin = new System.Windows.Forms.Padding(0);
            this.bigHorizontalScrollKioskTransactions.Name = "bigHorizontalScrollKioskTransactions";
            this.bigHorizontalScrollKioskTransactions.RightButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Right_Button;
            this.bigHorizontalScrollKioskTransactions.RightButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Right_Button_Disabled;
            this.bigHorizontalScrollKioskTransactions.ScrollableControl = null;
            this.bigHorizontalScrollKioskTransactions.ScrollViewer = null;
            this.bigHorizontalScrollKioskTransactions.Size = new System.Drawing.Size(920, 63);
            this.bigHorizontalScrollKioskTransactions.TabIndex = 1059;
            this.bigHorizontalScrollKioskTransactions.LeftButtonClick += new System.EventHandler(this.LeftButtonClick);
            this.bigHorizontalScrollKioskTransactions.RightButtonClick += new System.EventHandler(this.RightButtonClick);
            // 
            // lblTransaction
            // 
            this.lblTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTransaction.BackColor = System.Drawing.Color.Transparent;
            this.lblTransaction.Font = new System.Drawing.Font("Gotham Rounded Bold", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransaction.ForeColor = System.Drawing.Color.White;
            this.lblTransaction.Location = new System.Drawing.Point(3, 0);
            this.lblTransaction.Name = "lblTransaction";
            this.lblTransaction.Size = new System.Drawing.Size(998, 57);
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
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKioskTransactions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvKioskTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKioskTransactions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TransactionDate,
            this.TransactionId,
            this.TransactionNumber,
            this.POSName,
            this.OriginalTransactionId});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKioskTransactions.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvKioskTransactions.EnableHeadersVisualStyles = false;
            this.dgvKioskTransactions.GridColor = System.Drawing.SystemColors.Control;
            this.dgvKioskTransactions.Location = new System.Drawing.Point(10, 60);
            this.dgvKioskTransactions.Name = "dgvKioskTransactions";
            this.dgvKioskTransactions.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKioskTransactions.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvKioskTransactions.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvKioskTransactions.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvKioskTransactions.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvKioskTransactions.RowTemplate.Height = 93;
            this.dgvKioskTransactions.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKioskTransactions.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvKioskTransactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvKioskTransactions.Size = new System.Drawing.Size(920, 953);
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
            // TransactionNumber
            // 
            this.TransactionNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TransactionNumber.HeaderText = "Transaction No";
            this.TransactionNumber.MinimumWidth = 230;
            this.TransactionNumber.Name = "TransactionNumber";
            this.TransactionNumber.ReadOnly = true;
            this.TransactionNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TransactionNumber.Width = 232;
            // 
            // POSName
            // 
            this.POSName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.POSName.HeaderText = "POS Name";
            this.POSName.MinimumWidth = 280;
            this.POSName.Name = "POSName";
            this.POSName.ReadOnly = true;
            // 
            // OriginalTransactionId
            // 
            this.OriginalTransactionId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.OriginalTransactionId.HeaderText = "Original Transaction Id";
            this.OriginalTransactionId.MinimumWidth = 230;
            this.OriginalTransactionId.Name = "OriginalTransactionId";
            this.OriginalTransactionId.ReadOnly = true;
            this.OriginalTransactionId.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.OriginalTransactionId.Visible = false;
            // 
            // btnPrintReceipt
            // 
            this.btnPrintReceipt.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintReceipt.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrintReceipt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrintReceipt.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrintReceipt.FlatAppearance.BorderSize = 0;
            this.btnPrintReceipt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintReceipt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintReceipt.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F);
            this.btnPrintReceipt.ForeColor = System.Drawing.Color.White;
            this.btnPrintReceipt.Location = new System.Drawing.Point(26, 1565);
            this.btnPrintReceipt.Name = "btnPrintReceipt";
            this.btnPrintReceipt.Size = new System.Drawing.Size(325, 140);
            this.btnPrintReceipt.TabIndex = 159;
            this.btnPrintReceipt.Text = "Print Receipt";
            this.btnPrintReceipt.UseVisualStyleBackColor = false;
            this.btnPrintReceipt.Click += new System.EventHandler(this.btnPrintReceipt_Click);
            // 
            // btnPrintPending
            // 
            this.btnPrintPending.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrintPending.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintPending.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrintPending.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrintPending.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrintPending.FlatAppearance.BorderSize = 0;
            this.btnPrintPending.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintPending.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintPending.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintPending.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F);
            this.btnPrintPending.ForeColor = System.Drawing.Color.White;
            this.btnPrintPending.Location = new System.Drawing.Point(377, 1565);
            this.btnPrintPending.Name = "btnPrintPending";
            this.btnPrintPending.Size = new System.Drawing.Size(325, 140);
            this.btnPrintPending.TabIndex = 160;
            this.btnPrintPending.Text = "Print Pending";
            this.btnPrintPending.UseVisualStyleBackColor = false;
            this.btnPrintPending.Click += new System.EventHandler(this.btnPrintPending_Click);
            // 
            // btnIssueTempCard
            // 
            this.btnIssueTempCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIssueTempCard.BackColor = System.Drawing.Color.Transparent;
            this.btnIssueTempCard.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnIssueTempCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIssueTempCard.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnIssueTempCard.FlatAppearance.BorderSize = 0;
            this.btnIssueTempCard.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnIssueTempCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnIssueTempCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnIssueTempCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIssueTempCard.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F);
            this.btnIssueTempCard.ForeColor = System.Drawing.Color.White;
            this.btnIssueTempCard.Location = new System.Drawing.Point(727, 1565);
            this.btnIssueTempCard.Name = "btnIssueTempCard";
            this.btnIssueTempCard.Size = new System.Drawing.Size(325, 140);
            this.btnIssueTempCard.TabIndex = 160;
            this.btnIssueTempCard.Text = "Issue Pending Cards";
            this.btnIssueTempCard.UseVisualStyleBackColor = false;
            this.btnIssueTempCard.Click += new System.EventHandler(this.btnIssueTempCard_Click);
            // 
            // btnRefund
            // 
            this.btnRefund.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefund.BackColor = System.Drawing.Color.Transparent;
            this.btnRefund.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnRefund.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefund.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnRefund.FlatAppearance.BorderSize = 0;
            this.btnRefund.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRefund.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefund.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefund.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefund.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F);
            this.btnRefund.ForeColor = System.Drawing.Color.White;
            this.btnRefund.Location = new System.Drawing.Point(555, 1715);
            this.btnRefund.Name = "btnRefund";
            this.btnRefund.Size = new System.Drawing.Size(325, 140);
            this.btnRefund.TabIndex = 160;
            this.btnRefund.Text = "Refund";
            this.btnRefund.UseVisualStyleBackColor = false;
            this.btnRefund.Click += new System.EventHandler(this.btnRefund_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ForeColor = System.Drawing.Color.White;
            this.btnPrev.Location = new System.Drawing.Point(176, 1715);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(325, 140);
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
            this.lblGreeting.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting.ForeColor = System.Drawing.Color.White;
            this.lblGreeting.Location = new System.Drawing.Point(2, 95);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(1059, 87);
            this.lblGreeting.TabIndex = 143;
            this.lblGreeting.Text = "Kiosk Transaction View";
            this.lblGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.lblSiteName.Location = new System.Drawing.Point(12, 10);
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
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1870);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 50);
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
            this.panel1.Location = new System.Drawing.Point(275, 361);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(263, 62);
            this.panel1.TabIndex = 1058;
            // 
            // txtTrxId
            // 
            this.txtTrxId.BackColor = System.Drawing.Color.White;
            this.txtTrxId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTrxId.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTrxId.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtTrxId.Location = new System.Drawing.Point(15, 8);
            this.txtTrxId.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtTrxId.Name = "txtTrxId";
            this.txtTrxId.Size = new System.Drawing.Size(237, 48);
            this.txtTrxId.TabIndex = 5;
            this.txtTrxId.Text = "12345678";
            this.txtTrxId.Enter += new System.EventHandler(this.txtTrxId_Enter);
            this.txtTrxId.TextChanged += new System.EventHandler(this.txtTrxId_TextChanged);
            // 
            // lblTrxId
            // 
            this.lblTrxId.BackColor = System.Drawing.Color.Transparent;
            this.lblTrxId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTrxId.Font = new System.Drawing.Font("Gotham Rounded Bold", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrxId.ForeColor = System.Drawing.Color.White;
            this.lblTrxId.Location = new System.Drawing.Point(7, 363);
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
            this.lblFromDate.Font = new System.Drawing.Font("Gotham Rounded Bold", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.ForeColor = System.Drawing.Color.White;
            this.lblFromDate.Location = new System.Drawing.Point(7, 198);
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
            this.panel3.Location = new System.Drawing.Point(275, 196);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(74, 62);
            this.panel3.TabIndex = 1061;
            // 
            // txtFromTimeHrs
            // 
            this.txtFromTimeHrs.BackColor = System.Drawing.Color.White;
            this.txtFromTimeHrs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFromTimeHrs.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFromTimeHrs.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtFromTimeHrs.Location = new System.Drawing.Point(8, 8);
            this.txtFromTimeHrs.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtFromTimeHrs.MaxLength = 2;
            this.txtFromTimeHrs.Name = "txtFromTimeHrs";
            this.txtFromTimeHrs.Size = new System.Drawing.Size(56, 48);
            this.txtFromTimeHrs.TabIndex = 6;
            this.txtFromTimeHrs.Text = "12";
            this.txtFromTimeHrs.TextChanged += new System.EventHandler(this.txtFromTimeHrs_TextChanged);
            this.txtFromTimeHrs.Enter += new System.EventHandler(this.txtFromTimeHrs_Enter);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Gotham Rounded Bold", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(533, 198);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 58);
            this.label1.TabIndex = 1062;
            this.label1.Text = "To #:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(563, 357);
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
            this.btnClear.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.Location = new System.Drawing.Point(747, 357);
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
            this.btnShowKeyPad.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Image = global::Parafait_Kiosk.Properties.Resources.Keyboard_1;
            this.btnShowKeyPad.Location = new System.Drawing.Point(933, 341);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(87, 83);
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
            this.panel6.Location = new System.Drawing.Point(351, 196);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(74, 62);
            this.panel6.TabIndex = 20003;
            // 
            // txtFromTimeMins
            // 
            this.txtFromTimeMins.BackColor = System.Drawing.Color.White;
            this.txtFromTimeMins.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFromTimeMins.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFromTimeMins.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtFromTimeMins.Location = new System.Drawing.Point(8, 7);
            this.txtFromTimeMins.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtFromTimeMins.MaxLength = 2;
            this.txtFromTimeMins.Name = "txtFromTimeMins";
            this.txtFromTimeMins.Size = new System.Drawing.Size(56, 48);
            this.txtFromTimeMins.TabIndex = 6;
            this.txtFromTimeMins.Text = "00";
            this.txtFromTimeMins.TextChanged += new System.EventHandler(this.txtFromTimeMins_TextChanged);
            this.txtFromTimeMins.Enter += new System.EventHandler(this.txtFromTimeMins_Enter);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Transparent;
            this.panel7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel7.Controls.Add(this.cmbFromTimeTT);
            this.panel7.Location = new System.Drawing.Point(426, 194);
            this.panel7.Margin = new System.Windows.Forms.Padding(0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(90, 65);
            this.panel7.TabIndex = 20004;
            // 
            // cmbFromTimeTT
            // 
            this.cmbFromTimeTT.BackColor = System.Drawing.Color.White;
            this.cmbFromTimeTT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromTimeTT.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFromTimeTT.ForeColor = System.Drawing.Color.DarkOrchid;
            this.cmbFromTimeTT.Items.AddRange(new object[] {
            "AM",
            "PM"});
            this.cmbFromTimeTT.Location = new System.Drawing.Point(1, 5);
            this.cmbFromTimeTT.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbFromTimeTT.MaxLength = 2;
            this.cmbFromTimeTT.Name = "cmbFromTimeTT";
            this.cmbFromTimeTT.Size = new System.Drawing.Size(88, 56);
            this.cmbFromTimeTT.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Controls.Add(this.cmbToTimeTT);
            this.panel4.Location = new System.Drawing.Point(900, 194);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(90, 65);
            this.panel4.TabIndex = 20007;
            // 
            // cmbToTimeTT
            // 
            this.cmbToTimeTT.BackColor = System.Drawing.Color.White;
            this.cmbToTimeTT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToTimeTT.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbToTimeTT.ForeColor = System.Drawing.Color.DarkOrchid;
            this.cmbToTimeTT.Items.AddRange(new object[] {
            "AM",
            "PM"});
            this.cmbToTimeTT.Location = new System.Drawing.Point(1, 5);
            this.cmbToTimeTT.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbToTimeTT.MaxLength = 2;
            this.cmbToTimeTT.Name = "cmbToTimeTT";
            this.cmbToTimeTT.Size = new System.Drawing.Size(88, 56);
            this.cmbToTimeTT.TabIndex = 6;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.Transparent;
            this.panel8.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel8.Controls.Add(this.txtToTimeMins);
            this.panel8.Location = new System.Drawing.Point(825, 196);
            this.panel8.Margin = new System.Windows.Forms.Padding(0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(74, 62);
            this.panel8.TabIndex = 20006;
            // 
            // txtToTimeMins
            // 
            this.txtToTimeMins.BackColor = System.Drawing.Color.White;
            this.txtToTimeMins.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtToTimeMins.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToTimeMins.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtToTimeMins.Location = new System.Drawing.Point(8, 8);
            this.txtToTimeMins.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtToTimeMins.MaxLength = 2;
            this.txtToTimeMins.Name = "txtToTimeMins";
            this.txtToTimeMins.Size = new System.Drawing.Size(56, 48);
            this.txtToTimeMins.TabIndex = 6;
            this.txtToTimeMins.Text = "00";
            this.txtToTimeMins.TextChanged += new System.EventHandler(this.txtToTimeMins_TextChanged);
            this.txtToTimeMins.Enter += new System.EventHandler(this.txtToTimeMins_Enter);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Transparent;
            this.panel9.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel9.Controls.Add(this.txtToTimeHrs);
            this.panel9.Location = new System.Drawing.Point(750, 196);
            this.panel9.Margin = new System.Windows.Forms.Padding(0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(74, 62);
            this.panel9.TabIndex = 20005;
            // 
            // txtToTimeHrs
            // 
            this.txtToTimeHrs.BackColor = System.Drawing.Color.White;
            this.txtToTimeHrs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtToTimeHrs.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToTimeHrs.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtToTimeHrs.Location = new System.Drawing.Point(8, 8);
            this.txtToTimeHrs.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtToTimeHrs.MaxLength = 2;
            this.txtToTimeHrs.Name = "txtToTimeHrs";
            this.txtToTimeHrs.Size = new System.Drawing.Size(56, 48);
            this.txtToTimeHrs.TabIndex = 6;
            this.txtToTimeHrs.Text = "12";
            this.txtToTimeHrs.TextChanged += new System.EventHandler(this.txtToTimeHrs_TextChanged);
            this.txtToTimeHrs.Enter += new System.EventHandler(this.txtToTimeHrs_Enter);
            // 
            // lblPosMachines
            // 
            this.lblPosMachines.BackColor = System.Drawing.Color.Transparent;
            this.lblPosMachines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPosMachines.Font = new System.Drawing.Font("Gotham Rounded Bold", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPosMachines.ForeColor = System.Drawing.Color.White;
            this.lblPosMachines.Location = new System.Drawing.Point(7, 281);
            this.lblPosMachines.Name = "lblPosMachines";
            this.lblPosMachines.Size = new System.Drawing.Size(268, 58);
            this.lblPosMachines.TabIndex = 20008;
            this.lblPosMachines.Text = "POS Name #:";
            this.lblPosMachines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlPosMachines
            // 
            this.pnlPosMachines.BackColor = System.Drawing.Color.Transparent;
            this.pnlPosMachines.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlPosMachines.Controls.Add(this.cmbPosMachines);
            this.pnlPosMachines.Location = new System.Drawing.Point(275, 278);
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
            this.cmbPosMachines.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F);
            this.cmbPosMachines.ForeColor = System.Drawing.Color.DarkOrchid;
            this.cmbPosMachines.FormattingEnabled = true;
            this.cmbPosMachines.Location = new System.Drawing.Point(2, 5);
            this.cmbPosMachines.Name = "cmbPosMachines";
            this.cmbPosMachines.Size = new System.Drawing.Size(446, 56);
            this.cmbPosMachines.TabIndex = 0;
            // 
            // frmKioskTransactionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
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
            this.Controls.Add(this.btnPrintReceipt);
            this.Controls.Add(this.btnRefund);
            this.Controls.Add(this.btnPrintPending);
            this.Controls.Add(this.btnIssueTempCard);
            this.Controls.Add(this.lblGreeting);
            this.Controls.Add(this.lblSiteName);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmKioskTransactionView";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kiosk Transaction View";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.KioskTransactionView_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KioskActivityDetails_KeyPress);
            this.panelKioskTransaction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKioskTransactions)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.pnlPosMachines.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvKioskTransactions;
        private System.Windows.Forms.Label lblTransaction;
        internal System.Windows.Forms.Label lblGreeting;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnPrintReceipt;
        private System.Windows.Forms.Button btnRefund;
        private System.Windows.Forms.Button btnPrintPending;
        private System.Windows.Forms.Button btnIssueTempCard;
        private System.Windows.Forms.Panel panelKioskTransaction;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label lblTrxId;
        internal System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Panel panel3;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtTrxId;
        private System.Windows.Forms.TextBox txtFromTimeHrs;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox txtFromTimeMins;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.ComboBox cmbFromTimeTT;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cmbToTimeTT;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TextBox txtToTimeMins;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.TextBox txtToTimeHrs;
        internal System.Windows.Forms.Label lblPosMachines;
        private System.Windows.Forms.Panel pnlPosMachines;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbPosMachines;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn POSName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTransactionId;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollKioskTransactions;
        private Semnox.Core.GenericUtilities.BigHorizontalScrollBarView bigHorizontalScrollKioskTransactions;
    }
}