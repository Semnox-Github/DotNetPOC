namespace Parafait_Kiosk
{
    partial class frmIssueTempCards
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
            this.btnIssuePending = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelPurchase = new System.Windows.Forms.Panel();
            this.bigVerticalScrollIssuedTrxLines = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.dgvIssuedTransactionLines = new System.Windows.Forms.DataGridView();
            this.issuedProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.issuedLineQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.issuedLineCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.issuedLineDBLineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bigVerticalScrollPendingIssueTrxLines = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.dgvPendingIssueTransactionLines = new System.Windows.Forms.DataGridView();
            this.productName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineCardNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineProcessed = new System.Windows.Forms.DataGridViewImageColumn();
            this.lblIssuedCards = new System.Windows.Forms.Label();
            this.lblCardsPending = new System.Windows.Forms.Label();
            this.dgvTransactionHeader = new System.Windows.Forms.DataGridView();
            this.dgvTransactionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTransactionDetails = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            this.panelPurchase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIssuedTransactionLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendingIssueTransactionLines)).BeginInit();
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
            this.lblSiteName.Size = new System.Drawing.Size(1896, 63);
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
            this.lblGreeting.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting.ForeColor = System.Drawing.Color.White;
            this.lblGreeting.Location = new System.Drawing.Point(2, 97);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(1906, 60);
            this.lblGreeting.TabIndex = 143;
            this.lblGreeting.Text = "Issue pending cards";
            this.lblGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGreeting.Visible = false;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.Transparent;
            this.mainPanel.Controls.Add(this.btnIssuePending);
            this.mainPanel.Controls.Add(this.btnCancel);
            this.mainPanel.Location = new System.Drawing.Point(323, 144);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1300, 868);
            this.mainPanel.TabIndex = 3;
            // 
            // btnIssuePending
            // 
            this.btnIssuePending.BackColor = System.Drawing.Color.Transparent;
            this.btnIssuePending.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnIssuePending.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIssuePending.FlatAppearance.BorderSize = 0;
            this.btnIssuePending.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnIssuePending.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnIssuePending.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnIssuePending.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIssuePending.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIssuePending.ForeColor = System.Drawing.Color.White;
            this.btnIssuePending.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIssuePending.Location = new System.Drawing.Point(673, 718);
            this.btnIssuePending.Name = "btnIssuePending";
            this.btnIssuePending.Size = new System.Drawing.Size(250, 125);
            this.btnIssuePending.TabIndex = 162;
            this.btnIssuePending.Text = "Issue";
            this.btnIssuePending.UseVisualStyleBackColor = false;
            this.btnIssuePending.Click += new System.EventHandler(this.btnIssuePending_Click);
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
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(353, 718);
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
            this.panelPurchase.Controls.Add(this.bigVerticalScrollIssuedTrxLines);
            this.panelPurchase.Controls.Add(this.bigVerticalScrollPendingIssueTrxLines);
            this.panelPurchase.Controls.Add(this.lblIssuedCards);
            this.panelPurchase.Controls.Add(this.lblCardsPending);
            this.panelPurchase.Controls.Add(this.dgvIssuedTransactionLines);
            this.panelPurchase.Controls.Add(this.dgvPendingIssueTransactionLines);
            this.panelPurchase.Controls.Add(this.dgvTransactionHeader);
            this.panelPurchase.Controls.Add(this.lblTransactionDetails);
            this.panelPurchase.Location = new System.Drawing.Point(320, 87);
            this.panelPurchase.Name = "panelPurchase";
            this.panelPurchase.Size = new System.Drawing.Size(1280, 741);
            this.panelPurchase.TabIndex = 161;
            // 
            // bigVerticalScrollIssuedTrxLines
            // 
            this.bigVerticalScrollIssuedTrxLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bigVerticalScrollIssuedTrxLines.AutoHide = false;
            this.bigVerticalScrollIssuedTrxLines.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollIssuedTrxLines.DataGridView = this.dgvIssuedTransactionLines;
            this.bigVerticalScrollIssuedTrxLines.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollIssuedTrxLines.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollIssuedTrxLines.Location = new System.Drawing.Point(1197, 462);
            this.bigVerticalScrollIssuedTrxLines.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.bigVerticalScrollIssuedTrxLines.Name = "bigVerticalScrollIssuedTrxLines";
            this.bigVerticalScrollIssuedTrxLines.ScrollableControl = null;
            this.bigVerticalScrollIssuedTrxLines.ScrollViewer = null;
            this.bigVerticalScrollIssuedTrxLines.Size = new System.Drawing.Size(63, 213);
            this.bigVerticalScrollIssuedTrxLines.TabIndex = 20019;
            this.bigVerticalScrollIssuedTrxLines.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollIssuedTrxLines.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollIssuedTrxLines.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollIssuedTrxLines.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // dgvIssuedTransactionLines
            // 
            this.dgvIssuedTransactionLines.AllowUserToAddRows = false;
            this.dgvIssuedTransactionLines.AllowUserToDeleteRows = false;
            this.dgvIssuedTransactionLines.AllowUserToResizeColumns = false;
            this.dgvIssuedTransactionLines.AllowUserToResizeRows = false;
            this.dgvIssuedTransactionLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvIssuedTransactionLines.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(204)))), ((int)(((byte)(208)))));
            this.dgvIssuedTransactionLines.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvIssuedTransactionLines.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvIssuedTransactionLines.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvIssuedTransactionLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvIssuedTransactionLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIssuedTransactionLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.issuedProductName,
            this.issuedLineQuantity,
            this.issuedLineCardNumber,
            this.issuedLineDBLineId});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvIssuedTransactionLines.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvIssuedTransactionLines.EnableHeadersVisualStyles = false;
            this.dgvIssuedTransactionLines.GridColor = System.Drawing.SystemColors.Control;
            this.dgvIssuedTransactionLines.Location = new System.Drawing.Point(23, 462);
            this.dgvIssuedTransactionLines.Name = "dgvIssuedTransactionLines";
            this.dgvIssuedTransactionLines.ReadOnly = true;
            this.dgvIssuedTransactionLines.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvIssuedTransactionLines.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvIssuedTransactionLines.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvIssuedTransactionLines.RowTemplate.Height = 60;
            this.dgvIssuedTransactionLines.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvIssuedTransactionLines.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvIssuedTransactionLines.Size = new System.Drawing.Size(1175, 213);
            this.dgvIssuedTransactionLines.TabIndex = 172;
            this.dgvIssuedTransactionLines.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvIssuedTransactionLines_CellFormatting);
            // 
            // issuedProductName
            // 
            this.issuedProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.issuedProductName.HeaderText = "Product";
            this.issuedProductName.MinimumWidth = 320;
            this.issuedProductName.Name = "issuedProductName";
            this.issuedProductName.ReadOnly = true;
            this.issuedProductName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.issuedProductName.Width = 320;
            // 
            // issuedLineQuantity
            // 
            this.issuedLineQuantity.HeaderText = "Quantity";
            this.issuedLineQuantity.MinimumWidth = 150;
            this.issuedLineQuantity.Name = "issuedLineQuantity";
            this.issuedLineQuantity.ReadOnly = true;
            this.issuedLineQuantity.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.issuedLineQuantity.Width = 150;
            // 
            // issuedLineCardNumber
            // 
            this.issuedLineCardNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.issuedLineCardNumber.HeaderText = "Card Number";
            this.issuedLineCardNumber.MinimumWidth = 280;
            this.issuedLineCardNumber.Name = "issuedLineCardNumber";
            this.issuedLineCardNumber.ReadOnly = true;
            this.issuedLineCardNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // issuedLineDBLineId
            // 
            this.issuedLineDBLineId.HeaderText = "Line Id";
            this.issuedLineDBLineId.MinimumWidth = 250;
            this.issuedLineDBLineId.Name = "issuedLineDBLineId";
            this.issuedLineDBLineId.ReadOnly = true;
            this.issuedLineDBLineId.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.issuedLineDBLineId.Visible = false;
            this.issuedLineDBLineId.Width = 250;
            // 
            // bigVerticalScrollPendingIssueTrxLines
            // 
            this.bigVerticalScrollPendingIssueTrxLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bigVerticalScrollPendingIssueTrxLines.AutoHide = false;
            this.bigVerticalScrollPendingIssueTrxLines.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollPendingIssueTrxLines.DataGridView = this.dgvPendingIssueTransactionLines;
            this.bigVerticalScrollPendingIssueTrxLines.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollPendingIssueTrxLines.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollPendingIssueTrxLines.Location = new System.Drawing.Point(1197, 180);
            this.bigVerticalScrollPendingIssueTrxLines.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.bigVerticalScrollPendingIssueTrxLines.Name = "bigVerticalScrollPendingIssueTrxLines";
            this.bigVerticalScrollPendingIssueTrxLines.ScrollableControl = null;
            this.bigVerticalScrollPendingIssueTrxLines.ScrollViewer = null;
            this.bigVerticalScrollPendingIssueTrxLines.Size = new System.Drawing.Size(63, 213);
            this.bigVerticalScrollPendingIssueTrxLines.TabIndex = 20018;
            this.bigVerticalScrollPendingIssueTrxLines.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollPendingIssueTrxLines.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollPendingIssueTrxLines.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollPendingIssueTrxLines.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // dgvPendingIssueTransactionLines
            // 
            this.dgvPendingIssueTransactionLines.AllowUserToAddRows = false;
            this.dgvPendingIssueTransactionLines.AllowUserToDeleteRows = false;
            this.dgvPendingIssueTransactionLines.AllowUserToResizeColumns = false;
            this.dgvPendingIssueTransactionLines.AllowUserToResizeRows = false;
            this.dgvPendingIssueTransactionLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPendingIssueTransactionLines.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(204)))), ((int)(((byte)(208)))));
            this.dgvPendingIssueTransactionLines.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPendingIssueTransactionLines.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvPendingIssueTransactionLines.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPendingIssueTransactionLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPendingIssueTransactionLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPendingIssueTransactionLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productName,
            this.lineQty,
            this.lineCardNo,
            this.lineProcessed});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPendingIssueTransactionLines.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPendingIssueTransactionLines.EnableHeadersVisualStyles = false;
            this.dgvPendingIssueTransactionLines.GridColor = System.Drawing.SystemColors.Control;
            this.dgvPendingIssueTransactionLines.Location = new System.Drawing.Point(23, 180);
            this.dgvPendingIssueTransactionLines.Name = "dgvPendingIssueTransactionLines";
            this.dgvPendingIssueTransactionLines.ReadOnly = true;
            this.dgvPendingIssueTransactionLines.RowHeadersVisible = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPendingIssueTransactionLines.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPendingIssueTransactionLines.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPendingIssueTransactionLines.RowTemplate.Height = 60;
            this.dgvPendingIssueTransactionLines.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPendingIssueTransactionLines.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPendingIssueTransactionLines.Size = new System.Drawing.Size(1175, 213);
            this.dgvPendingIssueTransactionLines.TabIndex = 167;
            this.dgvPendingIssueTransactionLines.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPendingIssueTransactionLines_CellFormatting);
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
            // lblIssuedCards
            // 
            this.lblIssuedCards.AutoSize = true;
            this.lblIssuedCards.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblIssuedCards.ForeColor = System.Drawing.Color.Thistle;
            this.lblIssuedCards.Location = new System.Drawing.Point(18, 426);
            this.lblIssuedCards.Name = "lblIssuedCards";
            this.lblIssuedCards.Size = new System.Drawing.Size(168, 29);
            this.lblIssuedCards.TabIndex = 178;
            this.lblIssuedCards.Text = "Issued Cards";
            // 
            // lblCardsPending
            // 
            this.lblCardsPending.AutoSize = true;
            this.lblCardsPending.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblCardsPending.ForeColor = System.Drawing.Color.Thistle;
            this.lblCardsPending.Location = new System.Drawing.Point(18, 145);
            this.lblCardsPending.Name = "lblCardsPending";
            this.lblCardsPending.Size = new System.Drawing.Size(239, 29);
            this.lblCardsPending.TabIndex = 177;
            this.lblCardsPending.Text = "Cards to be Issued";
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
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransactionHeader.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvTransactionHeader.EnableHeadersVisualStyles = false;
            this.dgvTransactionHeader.GridColor = System.Drawing.SystemColors.Control;
            this.dgvTransactionHeader.Location = new System.Drawing.Point(23, 44);
            this.dgvTransactionHeader.Name = "dgvTransactionHeader";
            this.dgvTransactionHeader.ReadOnly = true;
            this.dgvTransactionHeader.RowHeadersVisible = false;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTransactionHeader.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvTransactionHeader.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTransactionHeader.RowTemplate.Height = 60;
            this.dgvTransactionHeader.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTransactionHeader.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvTransactionHeader.Size = new System.Drawing.Size(1237, 90);
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
            this.lblTransactionDetails.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F);
            this.lblTransactionDetails.ForeColor = System.Drawing.Color.White;
            this.lblTransactionDetails.Location = new System.Drawing.Point(2, 6);
            this.lblTransactionDetails.Name = "lblTransactionDetails";
            this.lblTransactionDetails.Size = new System.Drawing.Size(1274, 35);
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
            // frmIssueTempCards
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
            this.Name = "frmIssueTempCards";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print Transaction Lines";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmIssueTempCards_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmIssueTempCards_KeyPress);
            this.mainPanel.ResumeLayout(false);
            this.panelPurchase.ResumeLayout(false);
            this.panelPurchase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIssuedTransactionLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendingIssueTransactionLines)).EndInit();
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
        private System.Windows.Forms.DataGridView dgvPendingIssueTransactionLines;
        private System.Windows.Forms.Label lblIssuedCards;
        private System.Windows.Forms.Label lblCardsPending;
        private System.Windows.Forms.DataGridView dgvIssuedTransactionLines;
        private System.Windows.Forms.Button btnIssuePending;
        internal System.Windows.Forms.Label lblGreeting;
        private System.Windows.Forms.DataGridViewTextBoxColumn productName;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineCardNo;
        private System.Windows.Forms.DataGridViewImageColumn lineProcessed;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTransactionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn issuedProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn issuedLineQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn issuedLineCardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn issuedLineDBLineId;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollIssuedTrxLines;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollPendingIssueTrxLines;
        //private System.Windows.Forms.DataGridViewTextBoxColumn customerName;
    }
}
