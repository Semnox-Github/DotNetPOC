namespace Semnox.Parafait.Inventory
{
    partial class frmRequisitionTemplate
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnNew = new System.Windows.Forms.Button();
            this.gbItemDetails = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_clear = new System.Windows.Forms.Button();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvProductList = new System.Windows.Forms.DataGridView();
            this.requisitionTemplateLinesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gb_Requisition = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lnkLblAddRemarks = new System.Windows.Forms.LinkLabel();
            this.txtTemplateName = new System.Windows.Forms.TextBox();
            this.cmbTemplateStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblTemplateNames = new System.Windows.Forms.Label();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.lblActive = new System.Windows.Forms.Label();
            this.cmbToDept = new System.Windows.Forms.ComboBox();
            this.lblToDept = new System.Windows.Forms.Label();
            this.cmbFromDept = new System.Windows.Forms.ComboBox();
            this.lblFromDept = new System.Windows.Forms.Label();
            this.cmbRequestingDept = new System.Windows.Forms.ComboBox();
            this.lblRequestingDept = new System.Windows.Forms.Label();
            this.cmbReqType = new System.Windows.Forms.ComboBox();
            this.lblReqType = new System.Windows.Forms.Label();
            this.lblReqTemplateId = new System.Windows.Forms.Label();
            this.txtReqTemplateId = new System.Windows.Forms.TextBox();
            this.gb_search = new System.Windows.Forms.GroupBox();
            this.cmbSearchStatus = new System.Windows.Forms.ComboBox();
            this.lblTemplateStatus = new System.Windows.Forms.Label();
            this.chkSearchIsActive = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTemplateId = new System.Windows.Forms.TextBox();
            this.cmbReqTypeSearch = new System.Windows.Forms.ComboBox();
            this.lblReqTypes = new System.Windows.Forms.Label();
            this.txtTemplateNamesearch = new System.Windows.Forms.TextBox();
            this.lblTemplateName = new System.Windows.Forms.Label();
            this.btnSearchTemplates = new System.Windows.Forms.Button();
            this.lblRequisitionType = new System.Windows.Forms.Label();
            this.dgvSearchRequisitions = new System.Windows.Forms.DataGridView();
            this.TemplateId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemplateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requisitionTemplatesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnClear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnProdSearch = new System.Windows.Forms.Button();
            this.lblCategory = new System.Windows.Forms.Label();
            this.txtProdcode = new System.Windows.Forms.TextBox();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAdvSearch = new System.Windows.Forms.Button();
            this.txtProdBarcode = new System.Windows.Forms.TextBox();
            this.cmbProdCategory = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOMId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbUOM = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Product_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemoveLine = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbItemDetails.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.requisitionTemplateLinesDTOBindingSource)).BeginInit();
            this.gb_Requisition.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gb_search.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchRequisitions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.requisitionTemplatesDTOBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnNew.Image = global::Semnox.Parafait.Inventory.Properties.Resources.add1;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(302, 558);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(88, 23);
            this.btnNew.TabIndex = 33;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // gbItemDetails
            // 
            this.gbItemDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbItemDetails.BackColor = System.Drawing.Color.Transparent;
            this.gbItemDetails.Controls.Add(this.panel1);
            this.gbItemDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbItemDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbItemDetails.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gbItemDetails.Location = new System.Drawing.Point(296, 171);
            this.gbItemDetails.Name = "gbItemDetails";
            this.gbItemDetails.Size = new System.Drawing.Size(863, 353);
            this.gbItemDetails.TabIndex = 31;
            this.gbItemDetails.TabStop = false;
            this.gbItemDetails.Text = "Item Details";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btn_clear);
            this.panel1.Controls.Add(this.cmbCategory);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.dgvProductList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(857, 334);
            this.panel1.TabIndex = 67;
            this.panel1.TabStop = true;
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(269, 1);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(101, 23);
            this.btn_clear.TabIndex = 32;
            this.btn_clear.Text = "Clear Category";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(130, 2);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(121, 21);
            this.cmbCategory.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "Filter By Category:";
            // 
            // dgvProductList
            // 
            this.dgvProductList.AllowUserToOrderColumns = true;
            this.dgvProductList.AllowUserToResizeColumns = false;
            this.dgvProductList.AllowUserToResizeRows = false;
            this.dgvProductList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProductList.AutoGenerateColumns = false;
            this.dgvProductList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProductList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProductList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.UOMId,
            this.Description,
            this.Category,
            this.cmbUOM,
            this.Product_ID,
            this.RemoveLine,
            this.CreatedBy,
            this.CreatedDate,
            this.LastUpdatedBy,
            this.LastUpdateDate});
            this.dgvProductList.DataSource = this.requisitionTemplateLinesDTOBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProductList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProductList.GridColor = System.Drawing.Color.LightSlateGray;
            this.dgvProductList.Location = new System.Drawing.Point(25, 32);
            this.dgvProductList.Name = "dgvProductList";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProductList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvProductList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductList.Size = new System.Drawing.Size(792, 274);
            this.dgvProductList.TabIndex = 32;
            this.dgvProductList.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvProductList_CellValidating);
            this.dgvProductList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductList_CellValueChanged);
            this.dgvProductList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvProductList_ColumnHeaderMouseClick);
            this.dgvProductList.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvProductList_DefaultValuesNeeded);
            // 
            // requisitionTemplateLinesDTOBindingSource
            // 
            this.requisitionTemplateLinesDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.RequisitionTemplateLinesDTO);
            // 
            // gb_Requisition
            // 
            this.gb_Requisition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Requisition.Controls.Add(this.panel2);
            this.gb_Requisition.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gb_Requisition.Location = new System.Drawing.Point(298, 4);
            this.gb_Requisition.Name = "gb_Requisition";
            this.gb_Requisition.Size = new System.Drawing.Size(863, 163);
            this.gb_Requisition.TabIndex = 18;
            this.gb_Requisition.TabStop = false;
            this.gb_Requisition.Text = "Requisition Template";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.lnkLblAddRemarks);
            this.panel2.Controls.Add(this.txtTemplateName);
            this.panel2.Controls.Add(this.cmbTemplateStatus);
            this.panel2.Controls.Add(this.lblStatus);
            this.panel2.Controls.Add(this.lblTemplateNames);
            this.panel2.Controls.Add(this.chkIsActive);
            this.panel2.Controls.Add(this.lblActive);
            this.panel2.Controls.Add(this.cmbToDept);
            this.panel2.Controls.Add(this.lblToDept);
            this.panel2.Controls.Add(this.cmbFromDept);
            this.panel2.Controls.Add(this.lblFromDept);
            this.panel2.Controls.Add(this.cmbRequestingDept);
            this.panel2.Controls.Add(this.lblRequestingDept);
            this.panel2.Controls.Add(this.cmbReqType);
            this.panel2.Controls.Add(this.lblReqType);
            this.panel2.Controls.Add(this.lblReqTemplateId);
            this.panel2.Controls.Add(this.txtReqTemplateId);
            this.panel2.Location = new System.Drawing.Point(6, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(851, 138);
            this.panel2.TabIndex = 18;
            this.panel2.TabStop = true;
            // 
            // lnkLblAddRemarks
            // 
            this.lnkLblAddRemarks.AutoSize = true;
            this.lnkLblAddRemarks.Location = new System.Drawing.Point(175, 123);
            this.lnkLblAddRemarks.Name = "lnkLblAddRemarks";
            this.lnkLblAddRemarks.Size = new System.Drawing.Size(49, 13);
            this.lnkLblAddRemarks.TabIndex = 30;
            this.lnkLblAddRemarks.TabStop = true;
            this.lnkLblAddRemarks.Text = "Remarks";
            this.lnkLblAddRemarks.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblAddRemarks_LinkClicked);
            // 
            // txtTemplateName
            // 
            this.txtTemplateName.Location = new System.Drawing.Point(178, 38);
            this.txtTemplateName.Name = "txtTemplateName";
            this.txtTemplateName.Size = new System.Drawing.Size(189, 20);
            this.txtTemplateName.TabIndex = 24;
            // 
            // cmbTemplateStatus
            // 
            this.cmbTemplateStatus.AutoCompleteCustomSource.AddRange(new string[] {
            "Open",
            "Submitted",
            "Cancellled",
            "Closed"});
            this.cmbTemplateStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbTemplateStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTemplateStatus.DisplayMember = "Name";
            this.cmbTemplateStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTemplateStatus.FormattingEnabled = true;
            this.cmbTemplateStatus.Items.AddRange(new object[] {
            "Open",
            "Closed"});
            this.cmbTemplateStatus.Location = new System.Drawing.Point(577, 99);
            this.cmbTemplateStatus.Name = "cmbTemplateStatus";
            this.cmbTemplateStatus.Size = new System.Drawing.Size(189, 21);
            this.cmbTemplateStatus.TabIndex = 29;
            this.cmbTemplateStatus.ValueMember = "VendorId";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(421, 104);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(153, 17);
            this.lblStatus.TabIndex = 73;
            this.lblStatus.Text = "Status:*";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTemplateNames
            // 
            this.lblTemplateNames.BackColor = System.Drawing.Color.White;
            this.lblTemplateNames.Location = new System.Drawing.Point(28, 33);
            this.lblTemplateNames.Name = "lblTemplateNames";
            this.lblTemplateNames.Size = new System.Drawing.Size(150, 17);
            this.lblTemplateNames.TabIndex = 71;
            this.lblTemplateNames.Text = "Template Name:*";
            this.lblTemplateNames.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Location = new System.Drawing.Point(577, 10);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(15, 14);
            this.chkIsActive.TabIndex = 23;
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // lblActive
            // 
            this.lblActive.BackColor = System.Drawing.Color.White;
            this.lblActive.Location = new System.Drawing.Point(416, 7);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(155, 19);
            this.lblActive.TabIndex = 69;
            this.lblActive.Text = "Is Active:";
            this.lblActive.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbToDept
            // 
            this.cmbToDept.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbToDept.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbToDept.DisplayMember = "Name";
            this.cmbToDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToDept.FormattingEnabled = true;
            this.cmbToDept.Location = new System.Drawing.Point(577, 68);
            this.cmbToDept.Name = "cmbToDept";
            this.cmbToDept.Size = new System.Drawing.Size(189, 21);
            this.cmbToDept.TabIndex = 28;
            this.cmbToDept.ValueMember = "VendorId";
            // 
            // lblToDept
            // 
            this.lblToDept.BackColor = System.Drawing.Color.White;
            this.lblToDept.Location = new System.Drawing.Point(425, 68);
            this.lblToDept.Name = "lblToDept";
            this.lblToDept.Size = new System.Drawing.Size(152, 20);
            this.lblToDept.TabIndex = 59;
            this.lblToDept.Text = "To Department/Store:*";
            this.lblToDept.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbFromDept
            // 
            this.cmbFromDept.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbFromDept.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFromDept.DisplayMember = "Name";
            this.cmbFromDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromDept.FormattingEnabled = true;
            this.cmbFromDept.Location = new System.Drawing.Point(577, 37);
            this.cmbFromDept.Name = "cmbFromDept";
            this.cmbFromDept.Size = new System.Drawing.Size(189, 21);
            this.cmbFromDept.TabIndex = 27;
            this.cmbFromDept.ValueMember = "VendorId";
            // 
            // lblFromDept
            // 
            this.lblFromDept.BackColor = System.Drawing.Color.White;
            this.lblFromDept.Location = new System.Drawing.Point(422, 36);
            this.lblFromDept.Name = "lblFromDept";
            this.lblFromDept.Size = new System.Drawing.Size(155, 19);
            this.lblFromDept.TabIndex = 57;
            this.lblFromDept.Text = "From Department/Store:*";
            this.lblFromDept.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbRequestingDept
            // 
            this.cmbRequestingDept.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbRequestingDept.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbRequestingDept.DisplayMember = "Name";
            this.cmbRequestingDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRequestingDept.FormattingEnabled = true;
            this.cmbRequestingDept.Location = new System.Drawing.Point(178, 100);
            this.cmbRequestingDept.Name = "cmbRequestingDept";
            this.cmbRequestingDept.Size = new System.Drawing.Size(189, 21);
            this.cmbRequestingDept.TabIndex = 26;
            this.cmbRequestingDept.ValueMember = "VendorId";
            // 
            // lblRequestingDept
            // 
            this.lblRequestingDept.BackColor = System.Drawing.Color.White;
            this.lblRequestingDept.Location = new System.Drawing.Point(20, 102);
            this.lblRequestingDept.Name = "lblRequestingDept";
            this.lblRequestingDept.Size = new System.Drawing.Size(155, 15);
            this.lblRequestingDept.TabIndex = 55;
            this.lblRequestingDept.Text = "Requesting Department:";
            this.lblRequestingDept.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbReqType
            // 
            this.cmbReqType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbReqType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbReqType.DisplayMember = "Name";
            this.cmbReqType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReqType.FormattingEnabled = true;
            this.cmbReqType.Location = new System.Drawing.Point(178, 68);
            this.cmbReqType.Name = "cmbReqType";
            this.cmbReqType.Size = new System.Drawing.Size(189, 21);
            this.cmbReqType.TabIndex = 25;
            this.cmbReqType.ValueMember = "VendorId";
            this.cmbReqType.SelectedIndexChanged += new System.EventHandler(this.cmbReqType_SelectedIndexChanged);
            // 
            // lblReqType
            // 
            this.lblReqType.BackColor = System.Drawing.Color.White;
            this.lblReqType.Location = new System.Drawing.Point(29, 67);
            this.lblReqType.Name = "lblReqType";
            this.lblReqType.Size = new System.Drawing.Size(149, 19);
            this.lblReqType.TabIndex = 53;
            this.lblReqType.Text = "Requisition Type:*";
            this.lblReqType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblReqTemplateId
            // 
            this.lblReqTemplateId.BackColor = System.Drawing.Color.White;
            this.lblReqTemplateId.Location = new System.Drawing.Point(30, 7);
            this.lblReqTemplateId.Name = "lblReqTemplateId";
            this.lblReqTemplateId.Size = new System.Drawing.Size(146, 18);
            this.lblReqTemplateId.TabIndex = 21;
            this.lblReqTemplateId.Text = "Template Id.:";
            this.lblReqTemplateId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReqTemplateId
            // 
            this.txtReqTemplateId.Enabled = false;
            this.txtReqTemplateId.Location = new System.Drawing.Point(178, 8);
            this.txtReqTemplateId.Name = "txtReqTemplateId";
            this.txtReqTemplateId.ReadOnly = true;
            this.txtReqTemplateId.Size = new System.Drawing.Size(189, 20);
            this.txtReqTemplateId.TabIndex = 22;
            // 
            // gb_search
            // 
            this.gb_search.Controls.Add(this.cmbSearchStatus);
            this.gb_search.Controls.Add(this.lblTemplateStatus);
            this.gb_search.Controls.Add(this.chkSearchIsActive);
            this.gb_search.Controls.Add(this.label3);
            this.gb_search.Controls.Add(this.txtTemplateId);
            this.gb_search.Controls.Add(this.cmbReqTypeSearch);
            this.gb_search.Controls.Add(this.lblReqTypes);
            this.gb_search.Controls.Add(this.txtTemplateNamesearch);
            this.gb_search.Controls.Add(this.lblTemplateName);
            this.gb_search.Controls.Add(this.btnSearchTemplates);
            this.gb_search.Controls.Add(this.lblRequisitionType);
            this.gb_search.Controls.Add(this.dgvSearchRequisitions);
            this.gb_search.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gb_search.Location = new System.Drawing.Point(10, 4);
            this.gb_search.Name = "gb_search";
            this.gb_search.Size = new System.Drawing.Size(282, 522);
            this.gb_search.TabIndex = 1;
            this.gb_search.TabStop = false;
            this.gb_search.Text = "Template Search";
            // 
            // cmbSearchStatus
            // 
            this.cmbSearchStatus.DisplayMember = "Name";
            this.cmbSearchStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchStatus.FormattingEnabled = true;
            this.cmbSearchStatus.Items.AddRange(new object[] {
            "Open",
            "Closed"});
            this.cmbSearchStatus.Location = new System.Drawing.Point(113, 110);
            this.cmbSearchStatus.Name = "cmbSearchStatus";
            this.cmbSearchStatus.Size = new System.Drawing.Size(153, 21);
            this.cmbSearchStatus.TabIndex = 14;
            this.cmbSearchStatus.ValueMember = "VendorId";
            // 
            // lblTemplateStatus
            // 
            this.lblTemplateStatus.Location = new System.Drawing.Point(3, 112);
            this.lblTemplateStatus.Name = "lblTemplateStatus";
            this.lblTemplateStatus.Size = new System.Drawing.Size(108, 18);
            this.lblTemplateStatus.TabIndex = 72;
            this.lblTemplateStatus.Text = "Status:";
            this.lblTemplateStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkSearchIsActive
            // 
            this.chkSearchIsActive.AutoSize = true;
            this.chkSearchIsActive.Checked = true;
            this.chkSearchIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSearchIsActive.Location = new System.Drawing.Point(113, 142);
            this.chkSearchIsActive.Name = "chkSearchIsActive";
            this.chkSearchIsActive.Size = new System.Drawing.Size(15, 14);
            this.chkSearchIsActive.TabIndex = 15;
            this.chkSearchIsActive.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(22, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 18);
            this.label3.TabIndex = 13;
            this.label3.Text = "Is Active:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTemplateId
            // 
            this.txtTemplateId.Location = new System.Drawing.Point(113, 52);
            this.txtTemplateId.Name = "txtTemplateId";
            this.txtTemplateId.Size = new System.Drawing.Size(153, 20);
            this.txtTemplateId.TabIndex = 12;
            // 
            // cmbReqTypeSearch
            // 
            this.cmbReqTypeSearch.DisplayMember = "Name";
            this.cmbReqTypeSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReqTypeSearch.FormattingEnabled = true;
            this.cmbReqTypeSearch.Location = new System.Drawing.Point(113, 79);
            this.cmbReqTypeSearch.Name = "cmbReqTypeSearch";
            this.cmbReqTypeSearch.Size = new System.Drawing.Size(153, 21);
            this.cmbReqTypeSearch.TabIndex = 13;
            this.cmbReqTypeSearch.ValueMember = "VendorId";
            // 
            // lblReqTypes
            // 
            this.lblReqTypes.Location = new System.Drawing.Point(3, 81);
            this.lblReqTypes.Name = "lblReqTypes";
            this.lblReqTypes.Size = new System.Drawing.Size(108, 18);
            this.lblReqTypes.TabIndex = 10;
            this.lblReqTypes.Text = "Requisition Type:";
            this.lblReqTypes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTemplateNamesearch
            // 
            this.txtTemplateNamesearch.Location = new System.Drawing.Point(113, 23);
            this.txtTemplateNamesearch.Name = "txtTemplateNamesearch";
            this.txtTemplateNamesearch.Size = new System.Drawing.Size(153, 20);
            this.txtTemplateNamesearch.TabIndex = 7;
            // 
            // lblTemplateName
            // 
            this.lblTemplateName.Location = new System.Drawing.Point(22, 24);
            this.lblTemplateName.Name = "lblTemplateName";
            this.lblTemplateName.Size = new System.Drawing.Size(88, 19);
            this.lblTemplateName.TabIndex = 9;
            this.lblTemplateName.Text = "Template Name:";
            this.lblTemplateName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearchTemplates
            // 
            this.btnSearchTemplates.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearchTemplates.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearchTemplates.Location = new System.Drawing.Point(191, 140);
            this.btnSearchTemplates.Name = "btnSearchTemplates";
            this.btnSearchTemplates.Size = new System.Drawing.Size(75, 23);
            this.btnSearchTemplates.TabIndex = 16;
            this.btnSearchTemplates.Text = "Search";
            this.btnSearchTemplates.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchTemplates.UseVisualStyleBackColor = true;
            this.btnSearchTemplates.Click += new System.EventHandler(this.btnSearchTemplates_Click);
            // 
            // lblRequisitionType
            // 
            this.lblRequisitionType.Location = new System.Drawing.Point(19, 50);
            this.lblRequisitionType.Name = "lblRequisitionType";
            this.lblRequisitionType.Size = new System.Drawing.Size(91, 23);
            this.lblRequisitionType.TabIndex = 5;
            this.lblRequisitionType.Text = "Template Id:";
            this.lblRequisitionType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvSearchRequisitions
            // 
            this.dgvSearchRequisitions.AllowUserToAddRows = false;
            this.dgvSearchRequisitions.AllowUserToDeleteRows = false;
            this.dgvSearchRequisitions.AutoGenerateColumns = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSearchRequisitions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSearchRequisitions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearchRequisitions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TemplateId,
            this.TemplateName,
            this.Status});
            this.dgvSearchRequisitions.DataSource = this.requisitionTemplatesDTOBindingSource;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSearchRequisitions.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSearchRequisitions.GridColor = System.Drawing.Color.LightSlateGray;
            this.dgvSearchRequisitions.Location = new System.Drawing.Point(6, 167);
            this.dgvSearchRequisitions.MultiSelect = false;
            this.dgvSearchRequisitions.Name = "dgvSearchRequisitions";
            this.dgvSearchRequisitions.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSearchRequisitions.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvSearchRequisitions.RowHeadersVisible = false;
            this.dgvSearchRequisitions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSearchRequisitions.Size = new System.Drawing.Size(270, 346);
            this.dgvSearchRequisitions.TabIndex = 17;
            this.dgvSearchRequisitions.SelectionChanged += new System.EventHandler(this.dgvSearchRequisitions_SelectionChanged);
            // 
            // TemplateId
            // 
            this.TemplateId.DataPropertyName = "TemplateId";
            this.TemplateId.HeaderText = "Id";
            this.TemplateId.Name = "TemplateId";
            this.TemplateId.ReadOnly = true;
            // 
            // TemplateName
            // 
            this.TemplateName.DataPropertyName = "TemplateName";
            this.TemplateName.HeaderText = "Name";
            this.TemplateName.Name = "TemplateName";
            this.TemplateName.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // requisitionTemplatesDTOBindingSource
            // 
            this.requisitionTemplatesDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.RequisitionTemplatesDTO);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.Location = new System.Drawing.Point(154, 159);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 20;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(28, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 18);
            this.label2.TabIndex = 18;
            this.label2.Text = "Product Barcode:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnProdSearch
            // 
            this.btnProdSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProdSearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnProdSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProdSearch.Location = new System.Drawing.Point(66, 159);
            this.btnProdSearch.Name = "btnProdSearch";
            this.btnProdSearch.Size = new System.Drawing.Size(75, 23);
            this.btnProdSearch.TabIndex = 15;
            this.btnProdSearch.Text = "Search";
            this.btnProdSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProdSearch.UseVisualStyleBackColor = true;
            this.btnProdSearch.Click += new System.EventHandler(this.btnProdSearch_Click);
            // 
            // lblCategory
            // 
            this.lblCategory.Location = new System.Drawing.Point(28, 59);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(91, 18);
            this.lblCategory.TabIndex = 12;
            this.lblCategory.Text = "Category:";
            this.lblCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProdcode
            // 
            this.txtProdcode.Location = new System.Drawing.Point(125, 26);
            this.txtProdcode.Name = "txtProdcode";
            this.txtProdcode.Size = new System.Drawing.Size(141, 20);
            this.txtProdcode.TabIndex = 11;
            // 
            // lblProductCode
            // 
            this.lblProductCode.Location = new System.Drawing.Point(28, 29);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(91, 17);
            this.lblProductCode.TabIndex = 10;
            this.lblProductCode.Text = "Product Code:";
            this.lblProductCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnAdvSearch);
            this.groupBox1.Controls.Add(this.txtProdBarcode);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbProdCategory);
            this.groupBox1.Controls.Add(this.btnProdSearch);
            this.groupBox1.Controls.Add(this.lblCategory);
            this.groupBox1.Controls.Add(this.txtProdcode);
            this.groupBox1.Controls.Add(this.lblProductCode);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(10, 335);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 191);
            this.groupBox1.TabIndex = 85;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Product Search";
            this.groupBox1.Visible = false;
            // 
            // btnAdvSearch
            // 
            this.btnAdvSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdvSearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAdvSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdvSearch.Location = new System.Drawing.Point(155, 123);
            this.btnAdvSearch.Name = "btnAdvSearch";
            this.btnAdvSearch.Size = new System.Drawing.Size(109, 23);
            this.btnAdvSearch.TabIndex = 96;
            this.btnAdvSearch.Text = "Advance Search";
            this.btnAdvSearch.UseVisualStyleBackColor = true;
            this.btnAdvSearch.Click += new System.EventHandler(this.btnAdvSearch_Click);
            // 
            // txtProdBarcode
            // 
            this.txtProdBarcode.Location = new System.Drawing.Point(125, 92);
            this.txtProdBarcode.Name = "txtProdBarcode";
            this.txtProdBarcode.Size = new System.Drawing.Size(141, 20);
            this.txtProdBarcode.TabIndex = 21;
            // 
            // cmbProdCategory
            // 
            this.cmbProdCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProdCategory.FormattingEnabled = true;
            this.cmbProdCategory.Location = new System.Drawing.Point(125, 56);
            this.cmbProdCategory.Name = "cmbProdCategory";
            this.cmbProdCategory.Size = new System.Drawing.Size(141, 21);
            this.cmbProdCategory.TabIndex = 13;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClose.Image = global::Semnox.Parafait.Inventory.Properties.Resources.cancel;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(686, 558);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 23);
            this.btnClose.TabIndex = 36;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRefresh.Image = global::Semnox.Parafait.Inventory.Properties.Resources.Refresh;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(550, 558);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(97, 23);
            this.btnRefresh.TabIndex = 35;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSave.Image = global::Semnox.Parafait.Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(425, 558);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 23);
            this.btnSave.TabIndex = 34;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblMessage.Location = new System.Drawing.Point(298, 532);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 19);
            this.lblMessage.TabIndex = 95;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Semnox.Parafait.Inventory.Properties.Resources.parsmallest;
            this.pictureBox1.Location = new System.Drawing.Point(9, 549);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(81, 38);
            this.pictureBox1.TabIndex = 87;
            this.pictureBox1.TabStop = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "TemplateId";
            this.dataGridViewTextBoxColumn1.HeaderText = "Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TemplateName";
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Status";
            this.dataGridViewTextBoxColumn3.HeaderText = "Status";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Code";
            this.dataGridViewTextBoxColumn4.HeaderText = "Product Code";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 97;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn5.HeaderText = "Description";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "CategoryName";
            this.dataGridViewTextBoxColumn6.HeaderText = "Category";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn6.Width = 55;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "UOM";
            this.dataGridViewTextBoxColumn7.HeaderText = "UOM";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 57;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn8.HeaderText = "Product ID";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Visible = false;
            this.dataGridViewTextBoxColumn8.Width = 83;
            // 
            // Code
            // 
            this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Code.DataPropertyName = "Code";
            this.Code.HeaderText = "Product Code";
            this.Code.Name = "Code";
            // 
            // UOMId
            // 
            this.UOMId.DataPropertyName = "UOMId";
            this.UOMId.HeaderText = "UOMId";
            this.UOMId.Name = "UOMId";
            this.UOMId.Visible = false;
            this.UOMId.Width = 66;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Description.DataPropertyName = "Description";
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // Category
            // 
            this.Category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Category.DataPropertyName = "CategoryName";
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            this.Category.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Category.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cmbUOM
            // 
            this.cmbUOM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cmbUOM.HeaderText = "UOM";
            this.cmbUOM.Name = "cmbUOM";
            this.cmbUOM.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cmbUOM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cmbUOM.Width = 80;
            // 
            // Product_ID
            // 
            this.Product_ID.DataPropertyName = "ProductId";
            this.Product_ID.HeaderText = "Product ID";
            this.Product_ID.Name = "Product_ID";
            this.Product_ID.Visible = false;
            this.Product_ID.Width = 83;
            // 
            // RemoveLine
            // 
            this.RemoveLine.DataPropertyName = "IsActive";
            this.RemoveLine.FalseValue = "false";
            this.RemoveLine.HeaderText = "IsActive";
            this.RemoveLine.Name = "RemoveLine";
            this.RemoveLine.TrueValue = "true";
            this.RemoveLine.Width = 51;
            // 
            // CreatedBy
            // 
            this.CreatedBy.DataPropertyName = "CreatedBy";
            this.CreatedBy.HeaderText = "CreatedBy";
            this.CreatedBy.Name = "CreatedBy";
            this.CreatedBy.Width = 81;
            // 
            // CreatedDate
            // 
            this.CreatedDate.DataPropertyName = "CreatedDate";
            this.CreatedDate.HeaderText = "CreatedDate";
            this.CreatedDate.Name = "CreatedDate";
            this.CreatedDate.Width = 92;
            // 
            // LastUpdatedBy
            // 
            this.LastUpdatedBy.DataPropertyName = "LastUpdatedBy";
            this.LastUpdatedBy.HeaderText = "LastUpdatedBy";
            this.LastUpdatedBy.Name = "LastUpdatedBy";
            this.LastUpdatedBy.Width = 105;
            // 
            // LastUpdateDate
            // 
            this.LastUpdateDate.DataPropertyName = "LastUpdateDate";
            this.LastUpdateDate.HeaderText = "LastUpdateDate";
            this.LastUpdateDate.Name = "LastUpdateDate";
            this.LastUpdateDate.Width = 110;
            // 
            // frmRequisitionTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 588);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.gbItemDetails);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gb_Requisition);
            this.Controls.Add(this.gb_search);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmRequisitionTemplate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Requisition Template";
            this.Activated += new System.EventHandler(this.frmRequisitionTemplate_Activated);
            this.Load += new System.EventHandler(this.frmRequisitionTemplate_Load);
            this.gbItemDetails.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.requisitionTemplateLinesDTOBindingSource)).EndInit();
            this.gb_Requisition.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.gb_search.ResumeLayout(false);
            this.gb_search.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchRequisitions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.requisitionTemplatesDTOBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.GroupBox gbItemDetails;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvProductList;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox gb_Requisition;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTemplateNames;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.ComboBox cmbToDept;
        private System.Windows.Forms.Label lblToDept;
        private System.Windows.Forms.ComboBox cmbFromDept;
        private System.Windows.Forms.Label lblFromDept;
        private System.Windows.Forms.ComboBox cmbRequestingDept;
        private System.Windows.Forms.Label lblRequestingDept;
        private System.Windows.Forms.ComboBox cmbReqType;
        private System.Windows.Forms.Label lblReqType;
        private System.Windows.Forms.Label lblReqTemplateId;
        private System.Windows.Forms.TextBox txtReqTemplateId;
        private System.Windows.Forms.GroupBox gb_search;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTemplateId;
        private System.Windows.Forms.ComboBox cmbReqTypeSearch;
        private System.Windows.Forms.Label lblReqTypes;
        private System.Windows.Forms.TextBox txtTemplateNamesearch;
        private System.Windows.Forms.Label lblTemplateName;
        private System.Windows.Forms.Button btnSearchTemplates;
        private System.Windows.Forms.Label lblRequisitionType;
        private System.Windows.Forms.DataGridView dgvSearchRequisitions;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnProdSearch;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.TextBox txtProdcode;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbProdCategory;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkSearchIsActive;
        private System.Windows.Forms.ComboBox cmbTemplateStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbSearchStatus;
        private System.Windows.Forms.Label lblTemplateStatus;
        private System.Windows.Forms.TextBox txtTemplateName;
        private System.Windows.Forms.TextBox txtProdBarcode;
        //private System.Windows.Forms.DataGridViewTextBoxColumn ProductId;
        private System.Windows.Forms.LinkLabel lnkLblAddRemarks;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnAdvSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemplateId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TemplateName;
        //private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.BindingSource requisitionTemplatesDTOBindingSource;
        private System.Windows.Forms.BindingSource requisitionTemplateLinesDTOBindingSource;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOMId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Product_ID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RemoveLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdateDate;
    }
}

