namespace Parafait_Kiosk
{
    partial class frmPrintTransactionLines
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.lblGreeting = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.btnPrintPending = new System.Windows.Forms.Button();
            this.panelPurchase = new System.Windows.Forms.Panel();
            this.cmbPrintReason = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblPrintReason = new System.Windows.Forms.Label();
            this.vPrintedScrollBarGp = new System.Windows.Forms.VScrollBar();
            this.btnPrintedPrev = new System.Windows.Forms.Button();
            this.btnPrintedNext = new System.Windows.Forms.Button();
            this.vPendingScrollBarGp = new System.Windows.Forms.VScrollBar();
            this.btnPendingPrev = new System.Windows.Forms.Button();
            this.btnPendingNext = new System.Windows.Forms.Button();
            this.lblPrintedCards = new System.Windows.Forms.Label();
            this.lblCardsPendingPrint = new System.Windows.Forms.Label();
            this.dgvPrintedTransactionLines = new System.Windows.Forms.DataGridView();
            this.printedProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printedLineQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printedLineCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printedLineProcessed = new System.Windows.Forms.DataGridViewImageColumn();
            this.printedLineDBLineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPendingPrintTransactionLines = new System.Windows.Forms.DataGridView();
            this.productName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineCardNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineProcessed = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgvTransactionHeader = new System.Windows.Forms.DataGridView();
            this.dgvTransactionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTransactionDetails = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            this.panelPurchase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrintedTransactionLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendingPrintTransactionLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionHeader)).BeginInit();
            this.SuspendLayout();
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
            this.lblSiteName.TabIndex = 135;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
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
            this.lblGreeting.Text = "Print pending cards";
            this.lblGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGreeting.Visible = false;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.Transparent;
            this.mainPanel.Controls.Add(this.btnPrintPending);
            this.mainPanel.Controls.Add(this.panelPurchase);
            this.mainPanel.Controls.Add(this.btnCancel);
            this.mainPanel.Location = new System.Drawing.Point(31, 198);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1037, 1630);
            this.mainPanel.TabIndex = 3;
            // 
            // btnPrintPending
            // 
            this.btnPrintPending.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintPending.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrintPending.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrintPending.FlatAppearance.BorderSize = 0;
            this.btnPrintPending.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrintPending.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintPending.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintPending.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintPending.Font = new System.Drawing.Font("Bango Pro", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintPending.ForeColor = System.Drawing.Color.White;
            this.btnPrintPending.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPending.Location = new System.Drawing.Point(546, 1452);
            this.btnPrintPending.Name = "btnPrintPending";
            this.btnPrintPending.Size = new System.Drawing.Size(288, 163);
            this.btnPrintPending.TabIndex = 162;
            this.btnPrintPending.Text = "Print";
            this.btnPrintPending.UseVisualStyleBackColor = false;
            this.btnPrintPending.Click += new System.EventHandler(this.btnPrintPending_Click);
            // 
            // panelPurchase
            // 
            this.panelPurchase.BackColor = System.Drawing.Color.Transparent;
            this.panelPurchase.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Table1;
            this.panelPurchase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelPurchase.Controls.Add(this.cmbPrintReason);
            this.panelPurchase.Controls.Add(this.lblPrintReason);
            this.panelPurchase.Controls.Add(this.vPrintedScrollBarGp);
            this.panelPurchase.Controls.Add(this.btnPrintedPrev);
            this.panelPurchase.Controls.Add(this.btnPrintedNext);
            this.panelPurchase.Controls.Add(this.vPendingScrollBarGp);
            this.panelPurchase.Controls.Add(this.btnPendingPrev);
            this.panelPurchase.Controls.Add(this.btnPendingNext);
            this.panelPurchase.Controls.Add(this.lblPrintedCards);
            this.panelPurchase.Controls.Add(this.lblCardsPendingPrint);
            this.panelPurchase.Controls.Add(this.dgvPrintedTransactionLines);
            this.panelPurchase.Controls.Add(this.dgvPendingPrintTransactionLines);
            this.panelPurchase.Controls.Add(this.dgvTransactionHeader);
            this.panelPurchase.Controls.Add(this.lblTransactionDetails);
            this.panelPurchase.Location = new System.Drawing.Point(14, 3);
            this.panelPurchase.Name = "panelPurchase";
            this.panelPurchase.Size = new System.Drawing.Size(987, 1427);
            this.panelPurchase.TabIndex = 161;
            // 
            // cmbPrintReason
            // 
            this.cmbPrintReason.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbPrintReason.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbPrintReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrintReason.Font = new System.Drawing.Font("Bango Pro", 28F);
            this.cmbPrintReason.ForeColor = System.Drawing.Color.White;
            this.cmbPrintReason.FormattingEnabled = true;
            this.cmbPrintReason.Location = new System.Drawing.Point(371, 1314);
            this.cmbPrintReason.Name = "cmbPrintReason";
            this.cmbPrintReason.Size = new System.Drawing.Size(532, 52);
            this.cmbPrintReason.TabIndex = 20009;
            // 
            // lblPrintReason
            // 
            this.lblPrintReason.BackColor = System.Drawing.Color.Transparent;
            this.lblPrintReason.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPrintReason.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintReason.ForeColor = System.Drawing.Color.White;
            this.lblPrintReason.Location = new System.Drawing.Point(20, 1313);
            this.lblPrintReason.Name = "lblPrintReason";
            this.lblPrintReason.Size = new System.Drawing.Size(345, 58);
            this.lblPrintReason.TabIndex = 20010;
            this.lblPrintReason.Text = "Print Reason:";
            this.lblPrintReason.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // vPrintedScrollBarGp
            // 
            this.vPrintedScrollBarGp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vPrintedScrollBarGp.Location = new System.Drawing.Point(911, 668);
            this.vPrintedScrollBarGp.Name = "vPrintedScrollBarGp";
            this.vPrintedScrollBarGp.Size = new System.Drawing.Size(51, 556);
            this.vPrintedScrollBarGp.TabIndex = 183;
            this.vPrintedScrollBarGp.Visible = false;
            this.vPrintedScrollBarGp.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vPrintedScrollBarGp_Scroll);
            // 
            // btnPrintedPrev
            // 
            this.btnPrintedPrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintedPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.left_arrow;
            this.btnPrintedPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrintedPrev.FlatAppearance.BorderSize = 0;
            this.btnPrintedPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintedPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintedPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintedPrev.Font = new System.Drawing.Font("Bango Pro", 46F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintedPrev.ForeColor = System.Drawing.Color.White;
            this.btnPrintedPrev.Location = new System.Drawing.Point(20, 1225);
            this.btnPrintedPrev.Name = "btnPrintedPrev";
            this.btnPrintedPrev.Size = new System.Drawing.Size(70, 68);
            this.btnPrintedPrev.TabIndex = 185;
            this.btnPrintedPrev.UseVisualStyleBackColor = false;
            this.btnPrintedPrev.Click += new System.EventHandler(this.btnPrintedPrev_Click);
            // 
            // btnPrintedNext
            // 
            this.btnPrintedNext.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintedNext.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.right_arrow;
            this.btnPrintedNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrintedNext.FlatAppearance.BorderSize = 0;
            this.btnPrintedNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintedNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintedNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintedNext.Font = new System.Drawing.Font("Bango Pro", 46F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintedNext.ForeColor = System.Drawing.Color.White;
            this.btnPrintedNext.Location = new System.Drawing.Point(881, 1225);
            this.btnPrintedNext.Name = "btnPrintedNext";
            this.btnPrintedNext.Size = new System.Drawing.Size(72, 68);
            this.btnPrintedNext.TabIndex = 184;
            this.btnPrintedNext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrintedNext.UseVisualStyleBackColor = false;
            this.btnPrintedNext.Click += new System.EventHandler(this.btnPrintedNext_Click);
            // 
            // vPendingScrollBarGp
            // 
            this.vPendingScrollBarGp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vPendingScrollBarGp.Location = new System.Drawing.Point(911, 180);
            this.vPendingScrollBarGp.Name = "vPendingScrollBarGp";
            this.vPendingScrollBarGp.Size = new System.Drawing.Size(51, 369);
            this.vPendingScrollBarGp.TabIndex = 182;
            this.vPendingScrollBarGp.Visible = false;
            this.vPendingScrollBarGp.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vPendingScrollBarGp_Scroll);
            // 
            // btnPendingPrev
            // 
            this.btnPendingPrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPendingPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.left_arrow;
            this.btnPendingPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPendingPrev.FlatAppearance.BorderSize = 0;
            this.btnPendingPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPendingPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPendingPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPendingPrev.Font = new System.Drawing.Font("Bango Pro", 46F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPendingPrev.ForeColor = System.Drawing.Color.White;
            this.btnPendingPrev.Location = new System.Drawing.Point(20, 554);
            this.btnPendingPrev.Name = "btnPendingPrev";
            this.btnPendingPrev.Size = new System.Drawing.Size(70, 68);
            this.btnPendingPrev.TabIndex = 181;
            this.btnPendingPrev.UseVisualStyleBackColor = false;
            this.btnPendingPrev.Click += new System.EventHandler(this.btnPendingPrev_Click);
            // 
            // btnPendingNext
            // 
            this.btnPendingNext.BackColor = System.Drawing.Color.Transparent;
            this.btnPendingNext.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.right_arrow;
            this.btnPendingNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPendingNext.FlatAppearance.BorderSize = 0;
            this.btnPendingNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPendingNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPendingNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPendingNext.Font = new System.Drawing.Font("Bango Pro", 46F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPendingNext.ForeColor = System.Drawing.Color.White;
            this.btnPendingNext.Location = new System.Drawing.Point(891, 554);
            this.btnPendingNext.Name = "btnPendingNext";
            this.btnPendingNext.Size = new System.Drawing.Size(72, 68);
            this.btnPendingNext.TabIndex = 180;
            this.btnPendingNext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPendingNext.UseVisualStyleBackColor = false;
            this.btnPendingNext.Click += new System.EventHandler(this.btnPendingNext_Click);
            // 
            // lblPrintedCards
            // 
            this.lblPrintedCards.AutoSize = true;
            this.lblPrintedCards.Font = new System.Drawing.Font("Bango Pro", 18F);
            this.lblPrintedCards.ForeColor = System.Drawing.Color.White;
            this.lblPrintedCards.Location = new System.Drawing.Point(31, 629);
            this.lblPrintedCards.Name = "lblPrintedCards";
            this.lblPrintedCards.Size = new System.Drawing.Size(175, 29);
            this.lblPrintedCards.TabIndex = 178;
            this.lblPrintedCards.Text = "Printed Cards";
            // 
            // lblCardsPendingPrint
            // 
            this.lblCardsPendingPrint.AutoSize = true;
            this.lblCardsPendingPrint.Font = new System.Drawing.Font("Bango Pro", 18F);
            this.lblCardsPendingPrint.ForeColor = System.Drawing.Color.White;
            this.lblCardsPendingPrint.Location = new System.Drawing.Point(31, 145);
            this.lblCardsPendingPrint.Name = "lblCardsPendingPrint";
            this.lblCardsPendingPrint.Size = new System.Drawing.Size(250, 29);
            this.lblCardsPendingPrint.TabIndex = 177;
            this.lblCardsPendingPrint.Text = "Cards Pending Print";
            // 
            // dgvPrintedTransactionLines
            // 
            this.dgvPrintedTransactionLines.AllowUserToAddRows = false;
            this.dgvPrintedTransactionLines.AllowUserToDeleteRows = false;
            this.dgvPrintedTransactionLines.AllowUserToResizeColumns = false;
            this.dgvPrintedTransactionLines.AllowUserToResizeRows = false;
            this.dgvPrintedTransactionLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPrintedTransactionLines.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(204)))), ((int)(((byte)(208)))));
            this.dgvPrintedTransactionLines.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPrintedTransactionLines.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvPrintedTransactionLines.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPrintedTransactionLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPrintedTransactionLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrintedTransactionLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.printedProductName,
            this.printedLineQuantity,
            this.printedLineCardNumber,
            this.printedLineProcessed,
            this.printedLineDBLineId});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Bango Pro", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPrintedTransactionLines.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPrintedTransactionLines.EnableHeadersVisualStyles = false;
            this.dgvPrintedTransactionLines.GridColor = System.Drawing.SystemColors.Control;
            this.dgvPrintedTransactionLines.Location = new System.Drawing.Point(20, 668);
            this.dgvPrintedTransactionLines.Name = "dgvPrintedTransactionLines";
            this.dgvPrintedTransactionLines.ReadOnly = true;
            this.dgvPrintedTransactionLines.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPrintedTransactionLines.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvPrintedTransactionLines.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPrintedTransactionLines.RowTemplate.Height = 60;
            this.dgvPrintedTransactionLines.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPrintedTransactionLines.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPrintedTransactionLines.Size = new System.Drawing.Size(943, 556);
            this.dgvPrintedTransactionLines.TabIndex = 172;
            this.dgvPrintedTransactionLines.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPrintedTransactionLines_CellClick);
            this.dgvPrintedTransactionLines.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPrintedTransactionLines_CellFormatting);
            // 
            // printedProductName
            // 
            this.printedProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.printedProductName.HeaderText = "Product";
            this.printedProductName.MinimumWidth = 320;
            this.printedProductName.Name = "printedProductName";
            this.printedProductName.ReadOnly = true;
            this.printedProductName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.printedProductName.Width = 320;
            // 
            // printedLineQuantity
            // 
            this.printedLineQuantity.HeaderText = "Quantity";
            this.printedLineQuantity.MinimumWidth = 150;
            this.printedLineQuantity.Name = "printedLineQuantity";
            this.printedLineQuantity.ReadOnly = true;
            this.printedLineQuantity.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.printedLineQuantity.Width = 153;
            // 
            // printedLineCardNumber
            // 
            this.printedLineCardNumber.HeaderText = "Card Number";
            this.printedLineCardNumber.MinimumWidth = 280;
            this.printedLineCardNumber.Name = "printedLineCardNumber";
            this.printedLineCardNumber.ReadOnly = true;
            this.printedLineCardNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.printedLineCardNumber.Width = 280;
            // 
            // printedLineProcessed
            // 
            this.printedLineProcessed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.printedLineProcessed.HeaderText = "Reprint?";
            this.printedLineProcessed.Image = global::Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
            this.printedLineProcessed.MinimumWidth = 100;
            this.printedLineProcessed.Name = "printedLineProcessed";
            this.printedLineProcessed.ReadOnly = true;
            this.printedLineProcessed.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // printedLineDBLineId
            // 
            this.printedLineDBLineId.HeaderText = "Line Id";
            this.printedLineDBLineId.MinimumWidth = 250;
            this.printedLineDBLineId.Name = "printedLineDBLineId";
            this.printedLineDBLineId.ReadOnly = true;
            this.printedLineDBLineId.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.printedLineDBLineId.Visible = false;
            this.printedLineDBLineId.Width = 250;
            // 
            // dgvPendingPrintTransactionLines
            // 
            this.dgvPendingPrintTransactionLines.AllowUserToAddRows = false;
            this.dgvPendingPrintTransactionLines.AllowUserToDeleteRows = false;
            this.dgvPendingPrintTransactionLines.AllowUserToResizeColumns = false;
            this.dgvPendingPrintTransactionLines.AllowUserToResizeRows = false;
            this.dgvPendingPrintTransactionLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPendingPrintTransactionLines.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(204)))), ((int)(((byte)(208)))));
            this.dgvPendingPrintTransactionLines.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPendingPrintTransactionLines.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvPendingPrintTransactionLines.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPendingPrintTransactionLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPendingPrintTransactionLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPendingPrintTransactionLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productName,
            this.lineQty,
            this.lineCardNo,
            this.lineProcessed});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Bango Pro", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPendingPrintTransactionLines.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPendingPrintTransactionLines.EnableHeadersVisualStyles = false;
            this.dgvPendingPrintTransactionLines.GridColor = System.Drawing.SystemColors.Control;
            this.dgvPendingPrintTransactionLines.Location = new System.Drawing.Point(20, 180);
            this.dgvPendingPrintTransactionLines.Name = "dgvPendingPrintTransactionLines";
            this.dgvPendingPrintTransactionLines.ReadOnly = true;
            this.dgvPendingPrintTransactionLines.RowHeadersVisible = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPendingPrintTransactionLines.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPendingPrintTransactionLines.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPendingPrintTransactionLines.RowTemplate.Height = 60;
            this.dgvPendingPrintTransactionLines.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPendingPrintTransactionLines.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPendingPrintTransactionLines.Size = new System.Drawing.Size(943, 369);
            this.dgvPendingPrintTransactionLines.TabIndex = 167;
            this.dgvPendingPrintTransactionLines.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPendingPrintTransactionLines_CellFormatting);
            // 
            // productName
            // 
            this.productName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.productName.HeaderText = "Product";
            this.productName.MinimumWidth = 320;
            this.productName.Name = "productName";
            this.productName.ReadOnly = true;
            this.productName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.productName.Width = 320;
            // 
            // lineQty
            // 
            this.lineQty.HeaderText = "Quantity";
            this.lineQty.MinimumWidth = 150;
            this.lineQty.Name = "lineQty";
            this.lineQty.ReadOnly = true;
            this.lineQty.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.lineQty.Width = 153;
            // 
            // lineCardNo
            // 
            this.lineCardNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lineCardNo.HeaderText = "Card Number";
            this.lineCardNo.MinimumWidth = 250;
            this.lineCardNo.Name = "lineCardNo";
            this.lineCardNo.ReadOnly = true;
            this.lineCardNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // lineProcessed
            // 
            this.lineProcessed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.lineProcessed.HeaderText = "Processed";
            this.lineProcessed.MinimumWidth = 30;
            this.lineProcessed.Name = "lineProcessed";
            this.lineProcessed.ReadOnly = true;
            this.lineProcessed.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.lineProcessed.Visible = false;
            this.lineProcessed.Width = 30;
            // 
            // dgvTransactionHeader
            // 
            this.dgvTransactionHeader.AllowUserToAddRows = false;
            this.dgvTransactionHeader.AllowUserToDeleteRows = false;
            this.dgvTransactionHeader.AllowUserToResizeColumns = false;
            this.dgvTransactionHeader.AllowUserToResizeRows = false;
            this.dgvTransactionHeader.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTransactionHeader.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(204)))), ((int)(((byte)(208)))));
            this.dgvTransactionHeader.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTransactionHeader.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvTransactionHeader.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransactionHeader.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvTransactionHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransactionHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTransactionId,
            this.trxNo,
            this.trxDate});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Bango Pro", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransactionHeader.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvTransactionHeader.EnableHeadersVisualStyles = false;
            this.dgvTransactionHeader.GridColor = System.Drawing.SystemColors.Control;
            this.dgvTransactionHeader.Location = new System.Drawing.Point(20, 45);
            this.dgvTransactionHeader.Name = "dgvTransactionHeader";
            this.dgvTransactionHeader.ReadOnly = true;
            this.dgvTransactionHeader.RowHeadersVisible = false;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTransactionHeader.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvTransactionHeader.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTransactionHeader.RowTemplate.Height = 60;
            this.dgvTransactionHeader.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTransactionHeader.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvTransactionHeader.Size = new System.Drawing.Size(943, 90);
            this.dgvTransactionHeader.TabIndex = 6;
            // 
            // dgvTransactionId
            // 
            this.dgvTransactionId.HeaderText = "Transaction Id";
            this.dgvTransactionId.MinimumWidth = 240;
            this.dgvTransactionId.Name = "dgvTransactionId";
            this.dgvTransactionId.ReadOnly = true;
            this.dgvTransactionId.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransactionId.Width = 240;
            // 
            // trxNo
            // 
            this.trxNo.HeaderText = "Transaction No";
            this.trxNo.MinimumWidth = 220;
            this.trxNo.Name = "trxNo";
            this.trxNo.ReadOnly = true;
            this.trxNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.trxNo.Width = 238;
            // 
            // trxDate
            // 
            this.trxDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.trxDate.HeaderText = "Transaction Date";
            this.trxDate.MinimumWidth = 220;
            this.trxDate.Name = "trxDate";
            this.trxDate.ReadOnly = true;
            this.trxDate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // lblTransactionDetails
            // 
            this.lblTransactionDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTransactionDetails.Font = new System.Drawing.Font("Bango Pro", 15.75F);
            this.lblTransactionDetails.ForeColor = System.Drawing.Color.White;
            this.lblTransactionDetails.Location = new System.Drawing.Point(2, 6);
            this.lblTransactionDetails.Name = "lblTransactionDetails";
            this.lblTransactionDetails.Size = new System.Drawing.Size(981, 35);
            this.lblTransactionDetails.TabIndex = 159;
            this.lblTransactionDetails.Text = "Transaction Details";
            this.lblTransactionDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Bango Pro", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(153, 1452);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(288, 163);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Back";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
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
            // frmPrintTransactionLines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.lblGreeting);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmPrintTransactionLines";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print Transaction Lines";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FSKExecuteOnlineTransaction_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FSKExecuteOnlineTransaction_KeyPress);
            this.mainPanel.ResumeLayout(false);
            this.panelPurchase.ResumeLayout(false);
            this.panelPurchase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrintedTransactionLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendingPrintTransactionLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionHeader)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.DataGridView dgvTransactionHeader;
        private System.Windows.Forms.Panel panelPurchase;
        private System.Windows.Forms.Label lblTransactionDetails;
        private System.Windows.Forms.DataGridView dgvPendingPrintTransactionLines;
        private System.Windows.Forms.Label lblPrintedCards;
        private System.Windows.Forms.Label lblCardsPendingPrint;
        private System.Windows.Forms.DataGridView dgvPrintedTransactionLines;
        private System.Windows.Forms.Button btnPrintPending;
        private System.Windows.Forms.Button btnPendingPrev;
        private System.Windows.Forms.Button btnPendingNext;
        private System.Windows.Forms.VScrollBar vPendingScrollBarGp;
        private System.Windows.Forms.VScrollBar vPrintedScrollBarGp;
        private System.Windows.Forms.Button btnPrintedPrev;
        private System.Windows.Forms.Button btnPrintedNext;
        internal System.Windows.Forms.Label lblGreeting;
        private System.Windows.Forms.DataGridViewTextBoxColumn printedProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn printedLineQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn printedLineCardNumber;
        private System.Windows.Forms.DataGridViewImageColumn printedLineProcessed;
        private System.Windows.Forms.DataGridViewTextBoxColumn printedLineDBLineId;
        private System.Windows.Forms.DataGridViewTextBoxColumn productName;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineCardNo;
        private System.Windows.Forms.DataGridViewImageColumn lineProcessed;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTransactionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxDate;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbPrintReason;
        internal System.Windows.Forms.Label lblPrintReason;
        //private System.Windows.Forms.DataGridViewTextBoxColumn customerName;
    }
}
