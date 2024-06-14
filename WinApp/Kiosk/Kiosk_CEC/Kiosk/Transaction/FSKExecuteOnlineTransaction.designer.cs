namespace Parafait_Kiosk
{
    partial class FSKExecuteOnlineTransaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSKExecuteOnlineTransaction));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pbClientLogo = new System.Windows.Forms.PictureBox();
            this.pbSemnox = new System.Windows.Forms.PictureBox();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.btnExecute = new System.Windows.Forms.Button();
            this.pnlTrasactionId = new System.Windows.Forms.Panel();
            this.txtTransactionId = new System.Windows.Forms.TextBox();
            this.pnlTransactionOTP = new System.Windows.Forms.Panel();
            this.txtTransactionOTP = new System.Windows.Forms.TextBox();
            this.panelPurchase = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.vScrollTransactionLines = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.dgvTransactionLines = new System.Windows.Forms.DataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewImageColumn();
            this.productName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineCardNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineProductDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionLineDTOBS = new System.Windows.Forms.BindingSource(this.components);
            this.pbxSelectAll = new System.Windows.Forms.PictureBox();
            this.lblProductDetail = new System.Windows.Forms.Label();
            this.lblLineCardNo = new System.Windows.Forms.Label();
            this.lblLineAmount = new System.Windows.Forms.Label();
            this.lblLineQuantity = new System.Windows.Forms.Label();
            this.lblLineProduct = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblTrxDate = new System.Windows.Forms.Label();
            this.dgvTransactionHeader = new System.Windows.Forms.DataGridView();
            this.transactionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTransactionDetails = new System.Windows.Forms.Label();
            this.lblTrxNo = new System.Windows.Forms.Label();
            this.lblTrxReferenceNo = new System.Windows.Forms.Label();
            this.lblTransactionId = new System.Windows.Forms.Label();
            this.lblTransactionOTP = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGetTransactionDetails = new System.Windows.Forms.Button();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.lblAppVersion = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbClientLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSemnox)).BeginInit();
            this.mainPanel.SuspendLayout();
            this.pnlTrasactionId.SuspendLayout();
            this.pnlTransactionOTP.SuspendLayout();
            this.panelPurchase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionLineDTOBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // pbClientLogo
            // 
            this.pbClientLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbClientLogo.Location = new System.Drawing.Point(309, 107);
            this.pbClientLogo.Name = "pbClientLogo";
            this.pbClientLogo.Size = new System.Drawing.Size(465, 186);
            this.pbClientLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbClientLogo.TabIndex = 141;
            this.pbClientLogo.TabStop = false;
            // 
            // pbSemnox
            // 
            this.pbSemnox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSemnox.BackColor = System.Drawing.Color.Transparent;
            this.pbSemnox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbSemnox.Image = global::Parafait_Kiosk.Properties.Resources.semnox_logo;
            this.pbSemnox.Location = new System.Drawing.Point(780, 1725);
            this.pbSemnox.Name = "pbSemnox";
            this.pbSemnox.Size = new System.Drawing.Size(215, 36);
            this.pbSemnox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbSemnox.TabIndex = 5;
            this.pbSemnox.TabStop = false;
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
            this.lblSiteName.Font = new System.Drawing.Font("Gotham Rounded Bold", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(12, 10);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1056, 82);
            this.lblSiteName.TabIndex = 135;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.Transparent;
            this.mainPanel.Controls.Add(this.btnExecute);
            this.mainPanel.Controls.Add(this.pnlTrasactionId);
            this.mainPanel.Controls.Add(this.pnlTransactionOTP);
            this.mainPanel.Controls.Add(this.panelPurchase);
            this.mainPanel.Controls.Add(this.lblTransactionId);
            this.mainPanel.Controls.Add(this.lblTransactionOTP);
            this.mainPanel.Controls.Add(this.btnCancel);
            this.mainPanel.Controls.Add(this.btnGetTransactionDetails);
            this.mainPanel.Controls.Add(this.btnShowKeyPad);
            this.mainPanel.Location = new System.Drawing.Point(31, 293);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1037, 1524);
            this.mainPanel.TabIndex = 3;
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecute.BackColor = System.Drawing.Color.Transparent;
            this.btnExecute.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnExecute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExecute.FlatAppearance.BorderSize = 0;
            this.btnExecute.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecute.Font = new System.Drawing.Font("Gotham Rounded Bold", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecute.ForeColor = System.Drawing.Color.White;
            this.btnExecute.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExecute.Location = new System.Drawing.Point(542, 890);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(197, 100);
            this.btnExecute.TabIndex = 20004;
            this.btnExecute.Text = "Issue";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // pnlTrasactionId
            // 
            this.pnlTrasactionId.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Value_add_box;
            this.pnlTrasactionId.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlTrasactionId.Controls.Add(this.txtTransactionId);
            this.pnlTrasactionId.ForeColor = System.Drawing.Color.White;
            this.pnlTrasactionId.Location = new System.Drawing.Point(651, 12);
            this.pnlTrasactionId.Name = "pnlTrasactionId";
            this.pnlTrasactionId.Size = new System.Drawing.Size(148, 59);
            this.pnlTrasactionId.TabIndex = 20003;
            // 
            // txtTransactionId
            // 
            this.txtTransactionId.BackColor = System.Drawing.Color.White;
            this.txtTransactionId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTransactionId.Font = new System.Drawing.Font("Gotham Rounded Bold", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransactionId.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtTransactionId.Location = new System.Drawing.Point(5, 11);
            this.txtTransactionId.MaxLength = 18;
            this.txtTransactionId.Name = "txtTransactionId";
            this.txtTransactionId.Size = new System.Drawing.Size(138, 36);
            this.txtTransactionId.TabIndex = 5;
            this.txtTransactionId.Enter += new System.EventHandler(this.TxtTrasactionId_Enter);
            // 
            // pnlTransactionOTP
            // 
            this.pnlTransactionOTP.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Value_add_box;
            this.pnlTransactionOTP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlTransactionOTP.Controls.Add(this.txtTransactionOTP);
            this.pnlTransactionOTP.ForeColor = System.Drawing.Color.White;
            this.pnlTransactionOTP.Location = new System.Drawing.Point(274, 12);
            this.pnlTransactionOTP.Name = "pnlTransactionOTP";
            this.pnlTransactionOTP.Size = new System.Drawing.Size(148, 59);
            this.pnlTransactionOTP.TabIndex = 20002;
            // 
            // txtTransactionOTP
            // 
            this.txtTransactionOTP.BackColor = System.Drawing.Color.White;
            this.txtTransactionOTP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTransactionOTP.Font = new System.Drawing.Font("Gotham Rounded Bold", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransactionOTP.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtTransactionOTP.Location = new System.Drawing.Point(5, 11);
            this.txtTransactionOTP.MaxLength = 10;
            this.txtTransactionOTP.Name = "txtTransactionOTP";
            this.txtTransactionOTP.Size = new System.Drawing.Size(138, 36);
            this.txtTransactionOTP.TabIndex = 3;
            this.txtTransactionOTP.Text = "12345678";
            this.txtTransactionOTP.Enter += new System.EventHandler(this.TxtTrasactionOTP_Enter);
            // 
            // panelPurchase
            // 
            this.panelPurchase.BackColor = System.Drawing.Color.Transparent;
            this.panelPurchase.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.product_table_ExOn;
            this.panelPurchase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelPurchase.Controls.Add(this.lblStatus);
            this.panelPurchase.Controls.Add(this.vScrollTransactionLines);
            this.panelPurchase.Controls.Add(this.pbxSelectAll);
            this.panelPurchase.Controls.Add(this.lblProductDetail);
            this.panelPurchase.Controls.Add(this.lblLineCardNo);
            this.panelPurchase.Controls.Add(this.lblLineAmount);
            this.panelPurchase.Controls.Add(this.dgvTransactionLines);
            this.panelPurchase.Controls.Add(this.lblLineQuantity);
            this.panelPurchase.Controls.Add(this.lblLineProduct);
            this.panelPurchase.Controls.Add(this.lblCustomerName);
            this.panelPurchase.Controls.Add(this.lblTrxDate);
            this.panelPurchase.Controls.Add(this.dgvTransactionHeader);
            this.panelPurchase.Controls.Add(this.lblTransactionDetails);
            this.panelPurchase.Controls.Add(this.lblTrxNo);
            this.panelPurchase.Controls.Add(this.lblTrxReferenceNo);
            this.panelPurchase.Location = new System.Drawing.Point(3, 107);
            this.panelPurchase.Name = "panelPurchase";
            this.panelPurchase.Size = new System.Drawing.Size(987, 740);
            this.panelPurchase.TabIndex = 161;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblStatus.Font = new System.Drawing.Font("Gotham Rounded Bold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(20, 673);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(924, 60);
            this.lblStatus.TabIndex = 176;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vScrollTransactionLines
            // 
            this.vScrollTransactionLines.AutoHide = false;
            this.vScrollTransactionLines.DataGridView = this.dgvTransactionLines;
            this.vScrollTransactionLines.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollTransactionLines.DownButtonBackgroundImage")));
            this.vScrollTransactionLines.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollTransactionLines.DownButtonDisabledBackgroundImage")));
            this.vScrollTransactionLines.Location = new System.Drawing.Point(943, 242);
            this.vScrollTransactionLines.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollTransactionLines.Name = "vScrollTransactionLines";
            this.vScrollTransactionLines.ScrollableControl = null;
            this.vScrollTransactionLines.ScrollViewer = null;
            this.vScrollTransactionLines.Size = new System.Drawing.Size(40, 430);
            this.vScrollTransactionLines.TabIndex = 174;
            this.vScrollTransactionLines.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollTransactionLines.UpButtonBackgroundImage")));
            this.vScrollTransactionLines.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollTransactionLines.UpButtonDisabledBackgroundImage")));
            this.vScrollTransactionLines.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollTransactionLines_Scroll);
            this.vScrollTransactionLines.Click += new System.EventHandler(this.vScrollTransactionLines_Click);
            // 
            // dgvTransactionLines
            // 
            this.dgvTransactionLines.AllowUserToAddRows = false;
            this.dgvTransactionLines.AllowUserToDeleteRows = false;
            this.dgvTransactionLines.AllowUserToResizeColumns = false;
            this.dgvTransactionLines.AllowUserToResizeRows = false;
            this.dgvTransactionLines.AutoGenerateColumns = false;
            this.dgvTransactionLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTransactionLines.BackgroundColor = System.Drawing.Color.White;
            this.dgvTransactionLines.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTransactionLines.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvTransactionLines.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTransactionLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTransactionLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransactionLines.ColumnHeadersVisible = false;
            this.dgvTransactionLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.productName,
            this.lineQty,
            this.lineAmount,
            this.lineCardNo,
            this.lineProductDetails,
            this.lineProductId});
            this.dgvTransactionLines.DataSource = this.transactionLineDTOBS;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransactionLines.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTransactionLines.EnableHeadersVisualStyles = false;
            this.dgvTransactionLines.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.dgvTransactionLines.Location = new System.Drawing.Point(20, 242);
            this.dgvTransactionLines.Name = "dgvTransactionLines";
            this.dgvTransactionLines.ReadOnly = true;
            this.dgvTransactionLines.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTransactionLines.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTransactionLines.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTransactionLines.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvTransactionLines.Size = new System.Drawing.Size(943, 430);
            this.dgvTransactionLines.TabIndex = 167;
            this.dgvTransactionLines.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTransactionLines_CellClick);
            // 
            // Selected
            // 
            this.Selected.HeaderText = "Select";
            this.Selected.Image = global::Parafait_Kiosk.Properties.Resources.NewUnTickedCheckBox;
            this.Selected.MinimumWidth = 40;
            this.Selected.Name = "Selected";
            this.Selected.ReadOnly = true;
            this.Selected.Width = 40;
            // 
            // productName
            // 
            this.productName.DataPropertyName = "ProductName";
            this.productName.HeaderText = "Product";
            this.productName.MinimumWidth = 240;
            this.productName.Name = "productName";
            this.productName.ReadOnly = true;
            this.productName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.productName.Width = 240;
            // 
            // lineQty
            // 
            this.lineQty.DataPropertyName = "Quantity";
            this.lineQty.HeaderText = "Quantity";
            this.lineQty.MinimumWidth = 80;
            this.lineQty.Name = "lineQty";
            this.lineQty.ReadOnly = true;
            this.lineQty.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.lineQty.Width = 80;
            // 
            // lineAmount
            // 
            this.lineAmount.DataPropertyName = "Amount";
            this.lineAmount.HeaderText = "Amount";
            this.lineAmount.MinimumWidth = 110;
            this.lineAmount.Name = "lineAmount";
            this.lineAmount.ReadOnly = true;
            this.lineAmount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.lineAmount.Width = 110;
            // 
            // lineCardNo
            // 
            this.lineCardNo.DataPropertyName = "CardNumber";
            this.lineCardNo.HeaderText = "Card Number";
            this.lineCardNo.MinimumWidth = 180;
            this.lineCardNo.Name = "lineCardNo";
            this.lineCardNo.ReadOnly = true;
            this.lineCardNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.lineCardNo.Width = 180;
            // 
            // lineProductDetails
            // 
            this.lineProductDetails.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lineProductDetails.DataPropertyName = "ProductDetail";
            this.lineProductDetails.HeaderText = "Product Detail";
            this.lineProductDetails.MinimumWidth = 260;
            this.lineProductDetails.Name = "lineProductDetails";
            this.lineProductDetails.ReadOnly = true;
            // 
            // lineProductId
            // 
            this.lineProductId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.lineProductId.DataPropertyName = "ProductId";
            this.lineProductId.HeaderText = "Product Id";
            this.lineProductId.MinimumWidth = 30;
            this.lineProductId.Name = "lineProductId";
            this.lineProductId.ReadOnly = true;
            this.lineProductId.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.lineProductId.Visible = false;
            this.lineProductId.Width = 30;
            // 
            // transactionLineDTOBS
            // 
            this.transactionLineDTOBS.DataSource = typeof(Semnox.Parafait.Transaction.TransactionLineDTO);
            // 
            // pbxSelectAll
            // 
            this.pbxSelectAll.BackColor = System.Drawing.Color.White;
            this.pbxSelectAll.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.NewUnTickedCheckBox;
            this.pbxSelectAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxSelectAll.Location = new System.Drawing.Point(25, 199);
            this.pbxSelectAll.Name = "pbxSelectAll";
            this.pbxSelectAll.Size = new System.Drawing.Size(40, 40);
            this.pbxSelectAll.TabIndex = 173;
            this.pbxSelectAll.TabStop = false;
            this.pbxSelectAll.Click += new System.EventHandler(this.pbxSelectAll_Click);
            // 
            // lblProductDetail
            // 
            this.lblProductDetail.AutoSize = true;
            this.lblProductDetail.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblProductDetail.ForeColor = System.Drawing.Color.Thistle;
            this.lblProductDetail.Location = new System.Drawing.Point(764, 210);
            this.lblProductDetail.Name = "lblProductDetail";
            this.lblProductDetail.Size = new System.Drawing.Size(85, 29);
            this.lblProductDetail.TabIndex = 172;
            this.lblProductDetail.Text = "Detail";
            // 
            // lblLineCardNo
            // 
            this.lblLineCardNo.AutoSize = true;
            this.lblLineCardNo.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblLineCardNo.ForeColor = System.Drawing.Color.Thistle;
            this.lblLineCardNo.Location = new System.Drawing.Point(516, 210);
            this.lblLineCardNo.Name = "lblLineCardNo";
            this.lblLineCardNo.Size = new System.Drawing.Size(176, 29);
            this.lblLineCardNo.TabIndex = 171;
            this.lblLineCardNo.Text = "Card Number";
            // 
            // lblLineAmount
            // 
            this.lblLineAmount.AutoSize = true;
            this.lblLineAmount.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblLineAmount.ForeColor = System.Drawing.Color.Thistle;
            this.lblLineAmount.Location = new System.Drawing.Point(392, 210);
            this.lblLineAmount.Name = "lblLineAmount";
            this.lblLineAmount.Size = new System.Drawing.Size(110, 29);
            this.lblLineAmount.TabIndex = 170;
            this.lblLineAmount.Text = "Amount";
            // 
            // lblLineQuantity
            // 
            this.lblLineQuantity.AutoSize = true;
            this.lblLineQuantity.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblLineQuantity.ForeColor = System.Drawing.Color.Thistle;
            this.lblLineQuantity.Location = new System.Drawing.Point(320, 210);
            this.lblLineQuantity.Name = "lblLineQuantity";
            this.lblLineQuantity.Size = new System.Drawing.Size(57, 29);
            this.lblLineQuantity.TabIndex = 169;
            this.lblLineQuantity.Text = "Qty";
            // 
            // lblLineProduct
            // 
            this.lblLineProduct.AutoSize = true;
            this.lblLineProduct.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblLineProduct.ForeColor = System.Drawing.Color.Thistle;
            this.lblLineProduct.Location = new System.Drawing.Point(137, 210);
            this.lblLineProduct.Name = "lblLineProduct";
            this.lblLineProduct.Size = new System.Drawing.Size(110, 29);
            this.lblLineProduct.TabIndex = 168;
            this.lblLineProduct.Text = "Product";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblCustomerName.ForeColor = System.Drawing.Color.Thistle;
            this.lblCustomerName.Location = new System.Drawing.Point(751, 57);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(132, 29);
            this.lblCustomerName.TabIndex = 166;
            this.lblCustomerName.Text = "Customer";
            // 
            // lblTrxDate
            // 
            this.lblTrxDate.AutoSize = true;
            this.lblTrxDate.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblTrxDate.ForeColor = System.Drawing.Color.Thistle;
            this.lblTrxDate.Location = new System.Drawing.Point(536, 57);
            this.lblTrxDate.Name = "lblTrxDate";
            this.lblTrxDate.Size = new System.Drawing.Size(71, 29);
            this.lblTrxDate.TabIndex = 165;
            this.lblTrxDate.Text = "Date";
            // 
            // dgvTransactionHeader
            // 
            this.dgvTransactionHeader.AllowUserToAddRows = false;
            this.dgvTransactionHeader.AllowUserToDeleteRows = false;
            this.dgvTransactionHeader.AllowUserToResizeColumns = false;
            this.dgvTransactionHeader.AllowUserToResizeRows = false;
            this.dgvTransactionHeader.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTransactionHeader.BackgroundColor = System.Drawing.Color.White;
            this.dgvTransactionHeader.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTransactionHeader.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvTransactionHeader.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTransactionHeader.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTransactionHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransactionHeader.ColumnHeadersVisible = false;
            this.dgvTransactionHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.transactionId,
            this.trxNo,
            this.trxDate,
            this.customerName});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransactionHeader.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvTransactionHeader.EnableHeadersVisualStyles = false;
            this.dgvTransactionHeader.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.dgvTransactionHeader.Location = new System.Drawing.Point(20, 89);
            this.dgvTransactionHeader.Name = "dgvTransactionHeader";
            this.dgvTransactionHeader.ReadOnly = true;
            this.dgvTransactionHeader.RowHeadersVisible = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTransactionHeader.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvTransactionHeader.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTransactionHeader.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvTransactionHeader.Size = new System.Drawing.Size(943, 100);
            this.dgvTransactionHeader.TabIndex = 6;
            // 
            // transactionId
            // 
            this.transactionId.HeaderText = "Transaction Id";
            this.transactionId.MinimumWidth = 220;
            this.transactionId.Name = "transactionId";
            this.transactionId.ReadOnly = true;
            this.transactionId.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.transactionId.Width = 220;
            // 
            // trxNo
            // 
            this.trxNo.HeaderText = "TrxNo";
            this.trxNo.MinimumWidth = 220;
            this.trxNo.Name = "trxNo";
            this.trxNo.ReadOnly = true;
            this.trxNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.trxNo.Width = 220;
            // 
            // trxDate
            // 
            this.trxDate.HeaderText = "Date";
            this.trxDate.MinimumWidth = 220;
            this.trxDate.Name = "trxDate";
            this.trxDate.ReadOnly = true;
            this.trxDate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.trxDate.Width = 220;
            // 
            // customerName
            // 
            this.customerName.HeaderText = "Customer";
            this.customerName.MinimumWidth = 280;
            this.customerName.Name = "customerName";
            this.customerName.ReadOnly = true;
            this.customerName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.customerName.Width = 280;
            // 
            // lblTransactionDetails
            // 
            this.lblTransactionDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTransactionDetails.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionDetails.ForeColor = System.Drawing.Color.White;
            this.lblTransactionDetails.Location = new System.Drawing.Point(2, 12);
            this.lblTransactionDetails.Name = "lblTransactionDetails";
            this.lblTransactionDetails.Size = new System.Drawing.Size(981, 35);
            this.lblTransactionDetails.TabIndex = 159;
            this.lblTransactionDetails.Text = "Transaction Details";
            this.lblTransactionDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTrxNo
            // 
            this.lblTrxNo.AutoSize = true;
            this.lblTrxNo.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblTrxNo.ForeColor = System.Drawing.Color.Thistle;
            this.lblTrxNo.Location = new System.Drawing.Point(253, 57);
            this.lblTrxNo.Name = "lblTrxNo";
            this.lblTrxNo.Size = new System.Drawing.Size(196, 29);
            this.lblTrxNo.TabIndex = 164;
            this.lblTrxNo.Text = "Transaction No";
            // 
            // lblTrxReferenceNo
            // 
            this.lblTrxReferenceNo.AutoSize = true;
            this.lblTrxReferenceNo.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.lblTrxReferenceNo.ForeColor = System.Drawing.Color.Thistle;
            this.lblTrxReferenceNo.Location = new System.Drawing.Point(36, 57);
            this.lblTrxReferenceNo.Name = "lblTrxReferenceNo";
            this.lblTrxReferenceNo.Size = new System.Drawing.Size(185, 29);
            this.lblTrxReferenceNo.TabIndex = 163;
            this.lblTrxReferenceNo.Text = "Transaction Id";
            // 
            // lblTransactionId
            // 
            this.lblTransactionId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTransactionId.BackColor = System.Drawing.Color.Transparent;
            this.lblTransactionId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTransactionId.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionId.ForeColor = System.Drawing.Color.White;
            this.lblTransactionId.Location = new System.Drawing.Point(405, 0);
            this.lblTransactionId.Name = "lblTransactionId";
            this.lblTransactionId.Size = new System.Drawing.Size(246, 87);
            this.lblTransactionId.TabIndex = 4;
            this.lblTransactionId.Text = "Transaction Id:";
            this.lblTransactionId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTransactionOTP
            // 
            this.lblTransactionOTP.AllowDrop = true;
            this.lblTransactionOTP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTransactionOTP.BackColor = System.Drawing.Color.Transparent;
            this.lblTransactionOTP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTransactionOTP.Font = new System.Drawing.Font("Gotham Rounded Bold", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionOTP.ForeColor = System.Drawing.Color.White;
            this.lblTransactionOTP.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionOTP.Name = "lblTransactionOTP";
            this.lblTransactionOTP.Size = new System.Drawing.Size(272, 87);
            this.lblTransactionOTP.TabIndex = 2;
            this.lblTransactionOTP.Text = "Transaction OTP:";
            this.lblTransactionOTP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(260, 890);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(197, 100);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Back";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnGetTransactionDetails
            // 
            this.btnGetTransactionDetails.BackColor = System.Drawing.Color.Transparent;
            this.btnGetTransactionDetails.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnGetTransactionDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGetTransactionDetails.FlatAppearance.BorderSize = 0;
            this.btnGetTransactionDetails.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnGetTransactionDetails.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGetTransactionDetails.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGetTransactionDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetTransactionDetails.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetTransactionDetails.ForeColor = System.Drawing.Color.White;
            this.btnGetTransactionDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGetTransactionDetails.Location = new System.Drawing.Point(820, 12);
            this.btnGetTransactionDetails.Name = "btnGetTransactionDetails";
            this.btnGetTransactionDetails.Size = new System.Drawing.Size(137, 60);
            this.btnGetTransactionDetails.TabIndex = 0;
            this.btnGetTransactionDetails.Text = "Search";
            this.btnGetTransactionDetails.UseVisualStyleBackColor = false;
            this.btnGetTransactionDetails.Click += new System.EventHandler(this.BtnGetTransactionDetails_Click);
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
            this.btnShowKeyPad.Image = global::Parafait_Kiosk.Properties.Resources.keyboard;
            this.btnShowKeyPad.Location = new System.Drawing.Point(967, 12);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(66, 60);
            this.btnShowKeyPad.TabIndex = 20001;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.BtnShowKeyPad_Click);
            // 
            // lblAppVersion
            // 
            this.lblAppVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAppVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblAppVersion.Font = new System.Drawing.Font("Gotham Rounded Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppVersion.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblAppVersion.Location = new System.Drawing.Point(793, 1840);
            this.lblAppVersion.Margin = new System.Windows.Forms.Padding(3);
            this.lblAppVersion.Name = "lblAppVersion";
            this.lblAppVersion.Size = new System.Drawing.Size(204, 20);
            this.lblAppVersion.TabIndex = 20004;
            this.lblAppVersion.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1871);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 49);
            this.txtMessage.TabIndex = 136;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // FSKExecuteOnlineTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.lblAppVersion);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.pbClientLogo);
            this.Controls.Add(this.pbSemnox);
            this.Controls.Add(this.lblSiteName);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FSKExecuteOnlineTransaction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Semnox Retrieve My Purchase";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.FSKExecuteOnlineTransaction_Activated);
            this.Deactivate += new System.EventHandler(this.FSKExecuteOnlineTransaction_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FSKExecuteOnlineTransaction_FormClosed);
            this.Load += new System.EventHandler(this.FSKExecuteOnlineTransaction_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FSKExecuteOnlineTransaction_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pbClientLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSemnox)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.pnlTrasactionId.ResumeLayout(false);
            this.pnlTrasactionId.PerformLayout();
            this.pnlTransactionOTP.ResumeLayout(false);
            this.pnlTransactionOTP.PerformLayout();
            this.panelPurchase.ResumeLayout(false);
            this.panelPurchase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionLineDTOBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactionHeader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetTransactionDetails;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.PictureBox pbSemnox;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.PictureBox pbClientLogo;
        private System.Windows.Forms.Label lblTransactionOTP;
        private System.Windows.Forms.DataGridView dgvTransactionHeader;
        private System.Windows.Forms.TextBox txtTransactionId;
        private System.Windows.Forms.Label lblTransactionId;
        private System.Windows.Forms.TextBox txtTransactionOTP;
        private System.Windows.Forms.Panel panelPurchase;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblTrxDate;
        private System.Windows.Forms.Label lblTrxNo;
        private System.Windows.Forms.Label lblTrxReferenceNo;
        private System.Windows.Forms.Label lblTransactionDetails;
        private System.Windows.Forms.Label lblLineCardNo;
        private System.Windows.Forms.Label lblLineAmount;
        private System.Windows.Forms.DataGridView dgvTransactionLines;
        private System.Windows.Forms.Label lblLineQuantity;
        private System.Windows.Forms.Label lblLineProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerName;
        private System.Windows.Forms.Panel pnlTransactionOTP;
        private System.Windows.Forms.Panel pnlTrasactionId;
        private System.Windows.Forms.Button btnExecute;
        //private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DataGridViewTextBoxColumn productName;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineCardNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineProductDetails;
        //private System.Windows.Forms.DataGridViewImageColumn lineProcessed;
        private System.Windows.Forms.DataGridViewImageColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineProductId;
        private System.Windows.Forms.BindingSource transactionLineDTOBS;
        private System.Windows.Forms.Label lblProductDetail;
        private System.Windows.Forms.PictureBox pbxSelectAll;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vScrollTransactionLines;
        private System.Windows.Forms.Label lblStatus; 
        private System.Windows.Forms.Label lblAppVersion;
    }
}
