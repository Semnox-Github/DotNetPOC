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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrintTransactionLines));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.lblGreeting = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.btnPrintPending = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelPurchase = new System.Windows.Forms.Panel();
            this.bigVerticalScrollPrintedTrxLines = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.dgvPrintedTransactionLines = new System.Windows.Forms.DataGridView();
            this.printedProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printedLineQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printedLineCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printedLineProcessed = new System.Windows.Forms.DataGridViewImageColumn();
            this.printedLineDBLineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bigVerticalScrollPendingPrintTrxLines = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.dgvPendingPrintTransactionLines = new System.Windows.Forms.DataGridView();
            this.productName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineCardNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineProcessed = new System.Windows.Forms.DataGridViewImageColumn();
            this.cmbPrintReason = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblPrintReason = new System.Windows.Forms.Label();
            this.lblPrintedCards = new System.Windows.Forms.Label();
            this.lblCardsPendingPrint = new System.Windows.Forms.Label();
            this.dgvTransactionHeader = new System.Windows.Forms.DataGridView();
            this.dgvTransactionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTransactionDetails = new System.Windows.Forms.Label();
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
            this.lblSiteName.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(12, 10);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1896, 66);
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
            this.lblGreeting.Font = new System.Drawing.Font("Gotham Rounded Bold", 33F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting.ForeColor = System.Drawing.Color.White;
            this.lblGreeting.Location = new System.Drawing.Point(2, 89);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(1899, 52);
            this.lblGreeting.TabIndex = 143;
            this.lblGreeting.Text = "Print pending cards";
            this.lblGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGreeting.Visible = false;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.Transparent;
            this.mainPanel.Controls.Add(this.btnPrintPending);
            this.mainPanel.Controls.Add(this.btnCancel);
            this.mainPanel.Location = new System.Drawing.Point(310, 89);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1300, 927);
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
            this.btnPrintPending.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F);
            this.btnPrintPending.ForeColor = System.Drawing.Color.White;
            this.btnPrintPending.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrintPending.Location = new System.Drawing.Point(685, 777);
            this.btnPrintPending.Name = "btnPrintPending";
            this.btnPrintPending.Size = new System.Drawing.Size(250, 125);
            this.btnPrintPending.TabIndex = 162;
            this.btnPrintPending.Text = "Print";
            this.btnPrintPending.UseVisualStyleBackColor = false;
            this.btnPrintPending.Click += new System.EventHandler(this.btnPrintPending_Click);
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
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(360, 777);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(250, 125);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Back";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // panelPurchase
            // 
            this.panelPurchase.BackColor = System.Drawing.Color.Transparent;
            this.panelPurchase.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.product_table;
            this.panelPurchase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelPurchase.Controls.Add(this.bigVerticalScrollPrintedTrxLines);
            this.panelPurchase.Controls.Add(this.bigVerticalScrollPendingPrintTrxLines);
            this.panelPurchase.Controls.Add(this.cmbPrintReason);
            this.panelPurchase.Controls.Add(this.lblPrintReason);
            this.panelPurchase.Controls.Add(this.lblPrintedCards);
            this.panelPurchase.Controls.Add(this.lblCardsPendingPrint);
            this.panelPurchase.Controls.Add(this.dgvPrintedTransactionLines);
            this.panelPurchase.Controls.Add(this.dgvPendingPrintTransactionLines);
            this.panelPurchase.Controls.Add(this.dgvTransactionHeader);
            this.panelPurchase.Controls.Add(this.lblTransactionDetails);
            this.panelPurchase.Location = new System.Drawing.Point(320, 93);
            this.panelPurchase.Name = "panelPurchase";
            this.panelPurchase.Size = new System.Drawing.Size(1280, 752);
            this.panelPurchase.TabIndex = 161;
            // 
            // bigVerticalScrollPrintedTrxLines
            // 
            this.bigVerticalScrollPrintedTrxLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bigVerticalScrollPrintedTrxLines.AutoHide = false;
            this.bigVerticalScrollPrintedTrxLines.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollPrintedTrxLines.DataGridView = this.dgvPrintedTransactionLines;
            this.bigVerticalScrollPrintedTrxLines.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollPrintedTrxLines.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollPrintedTrxLines.Location = new System.Drawing.Point(1200, 435);
            this.bigVerticalScrollPrintedTrxLines.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.bigVerticalScrollPrintedTrxLines.Name = "bigVerticalScrollPrintedTrxLines";
            this.bigVerticalScrollPrintedTrxLines.ScrollableControl = null;
            this.bigVerticalScrollPrintedTrxLines.ScrollViewer = null;
            this.bigVerticalScrollPrintedTrxLines.Size = new System.Drawing.Size(63, 213);
            this.bigVerticalScrollPrintedTrxLines.TabIndex = 20024;
            this.bigVerticalScrollPrintedTrxLines.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollPrintedTrxLines.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollPrintedTrxLines.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollPrintedTrxLines.DownButtonClick += new System.EventHandler(this.DownButtonClick);
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
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPrintedTransactionLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dgvPrintedTransactionLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrintedTransactionLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.printedProductName,
            this.printedLineQuantity,
            this.printedLineCardNumber,
            this.printedLineProcessed,
            this.printedLineDBLineId});
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPrintedTransactionLines.DefaultCellStyle = dataGridViewCellStyle20;
            this.dgvPrintedTransactionLines.EnableHeadersVisualStyles = false;
            this.dgvPrintedTransactionLines.GridColor = System.Drawing.SystemColors.Control;
            this.dgvPrintedTransactionLines.Location = new System.Drawing.Point(23, 435);
            this.dgvPrintedTransactionLines.Name = "dgvPrintedTransactionLines";
            this.dgvPrintedTransactionLines.ReadOnly = true;
            this.dgvPrintedTransactionLines.RowHeadersVisible = false;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPrintedTransactionLines.RowsDefaultCellStyle = dataGridViewCellStyle21;
            this.dgvPrintedTransactionLines.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPrintedTransactionLines.RowTemplate.Height = 60;
            this.dgvPrintedTransactionLines.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPrintedTransactionLines.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPrintedTransactionLines.Size = new System.Drawing.Size(1177, 213);
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
            this.printedLineQuantity.Width = 150;
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
            // bigVerticalScrollPendingPrintTrxLines
            // 
            this.bigVerticalScrollPendingPrintTrxLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bigVerticalScrollPendingPrintTrxLines.AutoHide = false;
            this.bigVerticalScrollPendingPrintTrxLines.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollPendingPrintTrxLines.DataGridView = this.dgvPendingPrintTransactionLines;
            this.bigVerticalScrollPendingPrintTrxLines.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("bigVerticalScrollPendingPrintTrxLines.DownButtonBackgroundImage")));
            this.bigVerticalScrollPendingPrintTrxLines.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("bigVerticalScrollPendingPrintTrxLines.DownButtonDisabledBackgroundImage")));
            this.bigVerticalScrollPendingPrintTrxLines.Location = new System.Drawing.Point(1200, 180);
            this.bigVerticalScrollPendingPrintTrxLines.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.bigVerticalScrollPendingPrintTrxLines.Name = "bigVerticalScrollPendingPrintTrxLines";
            this.bigVerticalScrollPendingPrintTrxLines.ScrollableControl = null;
            this.bigVerticalScrollPendingPrintTrxLines.ScrollViewer = null;
            this.bigVerticalScrollPendingPrintTrxLines.Size = new System.Drawing.Size(63, 213);
            this.bigVerticalScrollPendingPrintTrxLines.TabIndex = 20023;
            this.bigVerticalScrollPendingPrintTrxLines.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("bigVerticalScrollPendingPrintTrxLines.UpButtonBackgroundImage")));
            this.bigVerticalScrollPendingPrintTrxLines.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("bigVerticalScrollPendingPrintTrxLines.UpButtonDisabledBackgroundImage")));
            this.bigVerticalScrollPendingPrintTrxLines.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollPendingPrintTrxLines.DownButtonClick += new System.EventHandler(this.DownButtonClick);
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
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPendingPrintTransactionLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.dgvPendingPrintTransactionLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPendingPrintTransactionLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productName,
            this.lineQty,
            this.lineCardNo,
            this.lineProcessed});
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle23.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPendingPrintTransactionLines.DefaultCellStyle = dataGridViewCellStyle23;
            this.dgvPendingPrintTransactionLines.EnableHeadersVisualStyles = false;
            this.dgvPendingPrintTransactionLines.GridColor = System.Drawing.SystemColors.Control;
            this.dgvPendingPrintTransactionLines.Location = new System.Drawing.Point(23, 180);
            this.dgvPendingPrintTransactionLines.Name = "dgvPendingPrintTransactionLines";
            this.dgvPendingPrintTransactionLines.ReadOnly = true;
            this.dgvPendingPrintTransactionLines.RowHeadersVisible = false;
            dataGridViewCellStyle24.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPendingPrintTransactionLines.RowsDefaultCellStyle = dataGridViewCellStyle24;
            this.dgvPendingPrintTransactionLines.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPendingPrintTransactionLines.RowTemplate.Height = 60;
            this.dgvPendingPrintTransactionLines.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPendingPrintTransactionLines.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPendingPrintTransactionLines.Size = new System.Drawing.Size(1177, 213);
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
            this.lineQty.Width = 150;
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
            // cmbPrintReason
            // 
            this.cmbPrintReason.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbPrintReason.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbPrintReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrintReason.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F);
            this.cmbPrintReason.ForeColor = System.Drawing.Color.DarkOrchid;
            this.cmbPrintReason.FormattingEnabled = true;
            this.cmbPrintReason.Location = new System.Drawing.Point(484, 674);
            this.cmbPrintReason.Name = "cmbPrintReason";
            this.cmbPrintReason.Size = new System.Drawing.Size(532, 47);
            this.cmbPrintReason.TabIndex = 20009;
            // 
            // lblPrintReason
            // 
            this.lblPrintReason.BackColor = System.Drawing.Color.Transparent;
            this.lblPrintReason.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPrintReason.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintReason.ForeColor = System.Drawing.Color.White;
            this.lblPrintReason.Location = new System.Drawing.Point(30, 678);
            this.lblPrintReason.Name = "lblPrintReason";
            this.lblPrintReason.Size = new System.Drawing.Size(437, 43);
            this.lblPrintReason.TabIndex = 20010;
            this.lblPrintReason.Text = "Print Reason:";
            this.lblPrintReason.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPrintedCards
            // 
            this.lblPrintedCards.AutoSize = true;
            this.lblPrintedCards.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblPrintedCards.ForeColor = System.Drawing.Color.Thistle;
            this.lblPrintedCards.Location = new System.Drawing.Point(18, 403);
            this.lblPrintedCards.Name = "lblPrintedCards";
            this.lblPrintedCards.Size = new System.Drawing.Size(179, 29);
            this.lblPrintedCards.TabIndex = 178;
            this.lblPrintedCards.Text = "Printed Cards";
            // 
            // lblCardsPendingPrint
            // 
            this.lblCardsPendingPrint.AutoSize = true;
            this.lblCardsPendingPrint.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblCardsPendingPrint.ForeColor = System.Drawing.Color.Thistle;
            this.lblCardsPendingPrint.Location = new System.Drawing.Point(18, 145);
            this.lblCardsPendingPrint.Name = "lblCardsPendingPrint";
            this.lblCardsPendingPrint.Size = new System.Drawing.Size(255, 29);
            this.lblCardsPendingPrint.TabIndex = 177;
            this.lblCardsPendingPrint.Text = "Cards Pending Print";
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
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle25.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle25.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransactionHeader.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle25;
            this.dgvTransactionHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransactionHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTransactionId,
            this.trxNo,
            this.trxDate});
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransactionHeader.DefaultCellStyle = dataGridViewCellStyle26;
            this.dgvTransactionHeader.EnableHeadersVisualStyles = false;
            this.dgvTransactionHeader.GridColor = System.Drawing.SystemColors.Control;
            this.dgvTransactionHeader.Location = new System.Drawing.Point(23, 52);
            this.dgvTransactionHeader.Name = "dgvTransactionHeader";
            this.dgvTransactionHeader.ReadOnly = true;
            this.dgvTransactionHeader.RowHeadersVisible = false;
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTransactionHeader.RowsDefaultCellStyle = dataGridViewCellStyle27;
            this.dgvTransactionHeader.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTransactionHeader.RowTemplate.Height = 60;
            this.dgvTransactionHeader.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTransactionHeader.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvTransactionHeader.Size = new System.Drawing.Size(1240, 90);
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
            this.trxNo.Width = 220;
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
            this.lblTransactionDetails.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionDetails.ForeColor = System.Drawing.Color.White;
            this.lblTransactionDetails.Location = new System.Drawing.Point(2, 3);
            this.lblTransactionDetails.Name = "lblTransactionDetails";
            this.lblTransactionDetails.Size = new System.Drawing.Size(1274, 41);
            this.lblTransactionDetails.TabIndex = 159;
            this.lblTransactionDetails.Text = "Transaction Details";
            this.lblTransactionDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.txtMessage.Location = new System.Drawing.Point(0, 1031);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 49);
            this.txtMessage.TabIndex = 136;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // frmPrintTransactionLines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.panelPurchase);
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
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollPendingPrintTrxLines;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollPrintedTrxLines;
        //private System.Windows.Forms.DataGridViewTextBoxColumn customerName;
    }
}
