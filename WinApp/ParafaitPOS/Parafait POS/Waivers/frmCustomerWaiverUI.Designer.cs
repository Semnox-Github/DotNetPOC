using Semnox.Core.GenericUtilities;

namespace Parafait_POS.Waivers
{
    partial class frmCustomerWaiverUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomerWaiverUI));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlCustomer = new System.Windows.Forms.Panel();
            this.grpbxCustomer = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.lblSelectCustomerMsg = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvCustomers = new System.Windows.Forms.DataGridView();
            this.customerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerRelationType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signFor = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RelationshipTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSignWaiver = new System.Windows.Forms.Button();
            this.vScrollCustomers = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.pnlWaivers = new System.Windows.Forms.Panel();
            this.grpBxSignWaiver = new System.Windows.Forms.GroupBox();
            this.tcWaivers = new System.Windows.Forms.TabControl();
            this.tpSignWaivers = new System.Windows.Forms.TabPage();
            this.hScrollSignWaivers = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.pnlSignWaivers = new System.Windows.Forms.Panel();
            this.tPnlSignWaiverHeader = new System.Windows.Forms.TableLayoutPanel();
            this.lblCustomerHasSigned = new System.Windows.Forms.Label();
            this.lblWaiverSetName = new System.Windows.Forms.Label();
            this.lblViewWaiver = new System.Windows.Forms.Label();
            this.lblValidityDays = new System.Windows.Forms.Label();
            this.lblWaiveName = new System.Windows.Forms.Label();
            this.vScrollSignWaivers = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.tpViewSignedWaivers = new System.Windows.Forms.TabPage();
            this.btnEmailWaiver = new System.Windows.Forms.Button();
            this.btnPrintWaiver = new System.Windows.Forms.Button();
            this.btnViewSignedWaiver = new System.Windows.Forms.Button();
            this.btnDeactivate = new System.Windows.Forms.Button();
            this.hScrollCustSignedWaivers = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.dgvCustomerSignedWaivers = new System.Windows.Forms.DataGridView();
            this.customerSignedWaiverDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.verticalScrollBarView2 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.pnlBase = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selectRecord = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.customerSignedWaiverIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WaiverSetDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waiverSetDetailId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WaiverName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signedWaiverFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signedForNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signedByNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waiverCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expiryDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.deactivationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deactivatedBy = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.deactivationApprovedBy = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.guidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signedFor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlCustomer.SuspendLayout();
            this.grpbxCustomer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            this.pnlWaivers.SuspendLayout();
            this.grpBxSignWaiver.SuspendLayout();
            this.tcWaivers.SuspendLayout();
            this.tpSignWaivers.SuspendLayout();
            this.pnlSignWaivers.SuspendLayout();
            this.tPnlSignWaiverHeader.SuspendLayout();
            this.tpViewSignedWaivers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerSignedWaivers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerSignedWaiverDTOBindingSource)).BeginInit();
            this.pnlBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCustomer
            // 
            this.pnlCustomer.Controls.Add(this.grpbxCustomer);
            this.pnlCustomer.Location = new System.Drawing.Point(6, 0);
            this.pnlCustomer.Name = "pnlCustomer";
            this.pnlCustomer.Size = new System.Drawing.Size(550, 550);
            this.pnlCustomer.TabIndex = 1;
            // 
            // grpbxCustomer
            // 
            this.grpbxCustomer.Controls.Add(this.chkSelectAll);
            this.grpbxCustomer.Controls.Add(this.lblSelectCustomerMsg);
            this.grpbxCustomer.Controls.Add(this.btnCancel);
            this.grpbxCustomer.Controls.Add(this.dgvCustomers);
            this.grpbxCustomer.Controls.Add(this.btnSignWaiver);
            this.grpbxCustomer.Controls.Add(this.vScrollCustomers);
            this.grpbxCustomer.Location = new System.Drawing.Point(0, 0);
            this.grpbxCustomer.Name = "grpbxCustomer";
            this.grpbxCustomer.Size = new System.Drawing.Size(550, 550);
            this.grpbxCustomer.TabIndex = 7;
            this.grpbxCustomer.TabStop = false;
            this.grpbxCustomer.Text = "Customer: ";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSelectAll.BackColor = System.Drawing.SystemColors.Control;
            this.chkSelectAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chkSelectAll.FlatAppearance.BorderSize = 0;
            this.chkSelectAll.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.chkSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSelectAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSelectAll.ImageIndex = 1;
            this.chkSelectAll.Location = new System.Drawing.Point(437, 12);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(103, 33);
            this.chkSelectAll.TabIndex = 17;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkSelectAll.UseVisualStyleBackColor = false;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // lblSelectCustomerMsg
            // 
            this.lblSelectCustomerMsg.Location = new System.Drawing.Point(8, 18);
            this.lblSelectCustomerMsg.Name = "lblSelectCustomerMsg";
            this.lblSelectCustomerMsg.Size = new System.Drawing.Size(428, 27);
            this.lblSelectCustomerMsg.TabIndex = 2;
            this.lblSelectCustomerMsg.Text = "If you want to sign for dependents (Minors only). Please include them ";
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(297, 484);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 36);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvCustomers
            // 
            this.dgvCustomers.AllowUserToAddRows = false;
            this.dgvCustomers.AllowUserToDeleteRows = false;
            this.dgvCustomers.AllowUserToResizeColumns = false;
            this.dgvCustomers.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Transparent;
            this.dgvCustomers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.customerName,
            this.customerRelationType,
            this.signFor,
            this.RelationshipTypeId});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCustomers.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvCustomers.Location = new System.Drawing.Point(9, 45);
            this.dgvCustomers.MultiSelect = false;
            this.dgvCustomers.Name = "dgvCustomers";
            this.dgvCustomers.ReadOnly = true;
            this.dgvCustomers.RowHeadersVisible = false;
            this.dgvCustomers.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dgvCustomers.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvCustomers.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvCustomers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvCustomers.Size = new System.Drawing.Size(490, 411);
            this.dgvCustomers.TabIndex = 0;
            this.dgvCustomers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomers_CellClick);
            this.dgvCustomers.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvCustomers_DataError);
            // 
            // customerName
            // 
            this.customerName.HeaderText = "Customer Name";
            this.customerName.MinimumWidth = 250;
            this.customerName.Name = "customerName";
            this.customerName.ReadOnly = true;
            this.customerName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.customerName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.customerName.Width = 250;
            // 
            // customerRelationType
            // 
            this.customerRelationType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.customerRelationType.HeaderText = "Relation";
            this.customerRelationType.MinimumWidth = 150;
            this.customerRelationType.Name = "customerRelationType";
            this.customerRelationType.ReadOnly = true;
            this.customerRelationType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.customerRelationType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.customerRelationType.Width = 150;
            // 
            // signFor
            // 
            this.signFor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.signFor.FalseValue = "false";
            this.signFor.HeaderText = "Sign For";
            this.signFor.MinimumWidth = 87;
            this.signFor.Name = "signFor";
            this.signFor.ReadOnly = true;
            this.signFor.TrueValue = "true";
            // 
            // RelationshipTypeId
            // 
            this.RelationshipTypeId.HeaderText = "Relation Id";
            this.RelationshipTypeId.Name = "RelationshipTypeId";
            this.RelationshipTypeId.ReadOnly = true;
            this.RelationshipTypeId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RelationshipTypeId.Visible = false;
            // 
            // btnSignWaiver
            // 
            this.btnSignWaiver.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSignWaiver.BackgroundImage")));
            this.btnSignWaiver.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSignWaiver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignWaiver.ForeColor = System.Drawing.Color.White;
            this.btnSignWaiver.Location = new System.Drawing.Point(125, 484);
            this.btnSignWaiver.Name = "btnSignWaiver";
            this.btnSignWaiver.Size = new System.Drawing.Size(104, 36);
            this.btnSignWaiver.TabIndex = 4;
            this.btnSignWaiver.Text = "Sign";
            this.btnSignWaiver.UseVisualStyleBackColor = true;
            this.btnSignWaiver.Click += new System.EventHandler(this.btnSignWaiver_Click);
            // 
            // vScrollCustomers
            // 
            this.vScrollCustomers.AutoHide = false;
            this.vScrollCustomers.DataGridView = this.dgvCustomers;
            this.vScrollCustomers.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollCustomers.DownButtonBackgroundImage")));
            this.vScrollCustomers.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollCustomers.DownButtonDisabledBackgroundImage")));
            this.vScrollCustomers.Location = new System.Drawing.Point(500, 45);
            this.vScrollCustomers.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollCustomers.Name = "vScrollCustomers";
            this.vScrollCustomers.ScrollableControl = null;
            this.vScrollCustomers.ScrollViewer = null;
            this.vScrollCustomers.Size = new System.Drawing.Size(47, 411);
            this.vScrollCustomers.TabIndex = 6;
            this.vScrollCustomers.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollCustomers.UpButtonBackgroundImage")));
            this.vScrollCustomers.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollCustomers.UpButtonDisabledBackgroundImage")));
            // 
            // pnlWaivers
            // 
            this.pnlWaivers.Controls.Add(this.grpBxSignWaiver);
            this.pnlWaivers.Location = new System.Drawing.Point(556, 2);
            this.pnlWaivers.Name = "pnlWaivers";
            this.pnlWaivers.Size = new System.Drawing.Size(750, 550);
            this.pnlWaivers.TabIndex = 2;
            // 
            // grpBxSignWaiver
            // 
            this.grpBxSignWaiver.Controls.Add(this.tcWaivers);
            this.grpBxSignWaiver.Location = new System.Drawing.Point(0, -2);
            this.grpBxSignWaiver.Name = "grpBxSignWaiver";
            this.grpBxSignWaiver.Size = new System.Drawing.Size(747, 550);
            this.grpBxSignWaiver.TabIndex = 3;
            this.grpBxSignWaiver.TabStop = false;
            this.grpBxSignWaiver.Text = "Waiver details for Customer: ";
            // 
            // tcWaivers
            // 
            this.tcWaivers.Controls.Add(this.tpSignWaivers);
            this.tcWaivers.Controls.Add(this.tpViewSignedWaivers);
            this.tcWaivers.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcWaivers.Location = new System.Drawing.Point(3, 21);
            this.tcWaivers.Name = "tcWaivers";
            this.tcWaivers.SelectedIndex = 0;
            this.tcWaivers.Size = new System.Drawing.Size(740, 526);
            this.tcWaivers.TabIndex = 2;
            // 
            // tpSignWaivers
            // 
            this.tpSignWaivers.Controls.Add(this.hScrollSignWaivers);
            this.tpSignWaivers.Controls.Add(this.vScrollSignWaivers);
            this.tpSignWaivers.Controls.Add(this.pnlSignWaivers);
            this.tpSignWaivers.Location = new System.Drawing.Point(4, 24);
            this.tpSignWaivers.Name = "tpSignWaivers";
            this.tpSignWaivers.Padding = new System.Windows.Forms.Padding(3);
            this.tpSignWaivers.Size = new System.Drawing.Size(732, 498);
            this.tpSignWaivers.TabIndex = 0;
            this.tpSignWaivers.Text = "Sign Waivers";
            this.tpSignWaivers.UseVisualStyleBackColor = true;
            this.tpSignWaivers.Enter += new System.EventHandler(this.tpSignWaivers_Enter);
            // 
            // hScrollSignWaivers
            // 
            this.hScrollSignWaivers.AutoHide = false;
            this.hScrollSignWaivers.DataGridView = null;
            this.hScrollSignWaivers.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollSignWaivers.LeftButtonBackgroundImage")));
            this.hScrollSignWaivers.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollSignWaivers.LeftButtonDisabledBackgroundImage")));
            this.hScrollSignWaivers.Location = new System.Drawing.Point(7, 411);
            this.hScrollSignWaivers.Margin = new System.Windows.Forms.Padding(0);
            this.hScrollSignWaivers.Name = "hScrollSignWaivers";
            this.hScrollSignWaivers.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollSignWaivers.RightButtonBackgroundImage")));
            this.hScrollSignWaivers.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollSignWaivers.RightButtonDisabledBackgroundImage")));
            this.hScrollSignWaivers.ScrollableControl = this.pnlSignWaivers;
            this.hScrollSignWaivers.ScrollViewer = null;
            this.hScrollSignWaivers.Size = new System.Drawing.Size(718, 46);
            this.hScrollSignWaivers.TabIndex = 2;
            // 
            // pnlSignWaivers
            // 
            //this.pnlSignWaivers.AutoScroll = true;
            this.pnlSignWaivers.Controls.Add(this.tPnlSignWaiverHeader);
            this.pnlSignWaivers.Location = new System.Drawing.Point(7, 0);
            this.pnlSignWaivers.Name = "pnlSignWaivers";
            this.pnlSignWaivers.Size = new System.Drawing.Size(694, 428);
            this.pnlSignWaivers.TabIndex = 0;
            // 
            // tPnlSignWaiverHeader
            // 
            this.tPnlSignWaiverHeader.ColumnCount = 5;
            this.tPnlSignWaiverHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 261F));
            this.tPnlSignWaiverHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 257F));
            this.tPnlSignWaiverHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tPnlSignWaiverHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tPnlSignWaiverHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tPnlSignWaiverHeader.Controls.Add(this.lblCustomerHasSigned, 4, 0);
            this.tPnlSignWaiverHeader.Controls.Add(this.lblWaiverSetName, 0, 0);
            this.tPnlSignWaiverHeader.Controls.Add(this.lblViewWaiver, 3, 0);
            this.tPnlSignWaiverHeader.Controls.Add(this.lblValidityDays, 2, 0);
            this.tPnlSignWaiverHeader.Controls.Add(this.lblWaiveName, 1, 0);
            this.tPnlSignWaiverHeader.Location = new System.Drawing.Point(3, -3);
            this.tPnlSignWaiverHeader.Name = "tPnlSignWaiverHeader";
            this.tPnlSignWaiverHeader.RowCount = 1;
            this.tPnlSignWaiverHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tPnlSignWaiverHeader.Size = new System.Drawing.Size(676, 32);
            this.tPnlSignWaiverHeader.TabIndex = 0;
            // 
            // lblCustomerHasSigned
            // 
            this.lblCustomerHasSigned.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerHasSigned.Location = new System.Drawing.Point(615, 0);
            this.lblCustomerHasSigned.Name = "lblCustomerHasSigned";
            this.lblCustomerHasSigned.Size = new System.Drawing.Size(55, 27);
            this.lblCustomerHasSigned.TabIndex = 6;
            this.lblCustomerHasSigned.Text = "Signed?";
            this.lblCustomerHasSigned.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWaiverSetName
            // 
            this.lblWaiverSetName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblWaiverSetName.Location = new System.Drawing.Point(3, 0);
            this.lblWaiverSetName.Name = "lblWaiverSetName";
            this.lblWaiverSetName.Size = new System.Drawing.Size(189, 27);
            this.lblWaiverSetName.TabIndex = 2;
            this.lblWaiverSetName.Text = "Waiver Set Description";
            this.lblWaiverSetName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblViewWaiver
            // 
            this.lblViewWaiver.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblViewWaiver.Location = new System.Drawing.Point(588, 0);
            this.lblViewWaiver.Name = "lblViewWaiver";
            this.lblViewWaiver.Size = new System.Drawing.Size(21, 27);
            this.lblViewWaiver.TabIndex = 4;
            this.lblViewWaiver.Text = "...";
            this.lblViewWaiver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValidityDays
            // 
            this.lblValidityDays.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblValidityDays.Location = new System.Drawing.Point(521, 0);
            this.lblValidityDays.Name = "lblValidityDays";
            this.lblValidityDays.Size = new System.Drawing.Size(53, 27);
            this.lblValidityDays.TabIndex = 5;
            this.lblValidityDays.Text = "Validity";
            this.lblValidityDays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWaiveName
            // 
            this.lblWaiveName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblWaiveName.Location = new System.Drawing.Point(264, 0);
            this.lblWaiveName.Name = "lblWaiveName";
            this.lblWaiveName.Size = new System.Drawing.Size(191, 27);
            this.lblWaiveName.TabIndex = 3;
            this.lblWaiveName.Text = "Waivers Name";
            this.lblWaiveName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // vScrollSignWaivers
            // 
            this.vScrollSignWaivers.AutoHide = false;
            this.vScrollSignWaivers.DataGridView = null;
            this.vScrollSignWaivers.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollSignWaivers.DownButtonBackgroundImage")));
            this.vScrollSignWaivers.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollSignWaivers.DownButtonDisabledBackgroundImage")));
            this.vScrollSignWaivers.Location = new System.Drawing.Point(683, 0);
            this.vScrollSignWaivers.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollSignWaivers.Name = "vScrollSignWaivers";
            this.vScrollSignWaivers.ScrollableControl = this.pnlSignWaivers;
            this.vScrollSignWaivers.ScrollViewer = null;
            this.vScrollSignWaivers.Size = new System.Drawing.Size(47, 411);
            this.vScrollSignWaivers.TabIndex = 1;
            this.vScrollSignWaivers.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollSignWaivers.UpButtonBackgroundImage")));
            this.vScrollSignWaivers.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollSignWaivers.UpButtonDisabledBackgroundImage")));
            // 
            // tpViewSignedWaivers
            // 
            this.tpViewSignedWaivers.Controls.Add(this.btnEmailWaiver);
            this.tpViewSignedWaivers.Controls.Add(this.btnPrintWaiver);
            this.tpViewSignedWaivers.Controls.Add(this.btnViewSignedWaiver);
            this.tpViewSignedWaivers.Controls.Add(this.btnDeactivate);
            this.tpViewSignedWaivers.Controls.Add(this.hScrollCustSignedWaivers);
            this.tpViewSignedWaivers.Controls.Add(this.verticalScrollBarView2);
            this.tpViewSignedWaivers.Controls.Add(this.dgvCustomerSignedWaivers);
            this.tpViewSignedWaivers.Location = new System.Drawing.Point(4, 24);
            this.tpViewSignedWaivers.Name = "tpViewSignedWaivers";
            this.tpViewSignedWaivers.Padding = new System.Windows.Forms.Padding(3);
            this.tpViewSignedWaivers.Size = new System.Drawing.Size(732, 498);
            this.tpViewSignedWaivers.TabIndex = 1;
            this.tpViewSignedWaivers.Text = "View Signed Waivers";
            this.tpViewSignedWaivers.UseVisualStyleBackColor = true;
            this.tpViewSignedWaivers.Enter += new System.EventHandler(this.tpViewSignedWaivers_Enter);
            // 
            // btnEmailWaiver
            // 
            this.btnEmailWaiver.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEmailWaiver.BackgroundImage")));
            this.btnEmailWaiver.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEmailWaiver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmailWaiver.ForeColor = System.Drawing.Color.White;
            this.btnEmailWaiver.Location = new System.Drawing.Point(530, 434);
            this.btnEmailWaiver.Name = "btnEmailWaiver";
            this.btnEmailWaiver.Size = new System.Drawing.Size(104, 36);
            this.btnEmailWaiver.TabIndex = 12;
            this.btnEmailWaiver.Text = "Email";
            this.btnEmailWaiver.UseVisualStyleBackColor = true;
            this.btnEmailWaiver.Click += new System.EventHandler(this.btnEmailWaiver_Click);
            // 
            // btnPrintWaiver
            // 
            this.btnPrintWaiver.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPrintWaiver.BackgroundImage")));
            this.btnPrintWaiver.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrintWaiver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintWaiver.ForeColor = System.Drawing.Color.White;
            this.btnPrintWaiver.Location = new System.Drawing.Point(367, 434);
            this.btnPrintWaiver.Name = "btnPrintWaiver";
            this.btnPrintWaiver.Size = new System.Drawing.Size(104, 36);
            this.btnPrintWaiver.TabIndex = 11;
            this.btnPrintWaiver.Text = "Print";
            this.btnPrintWaiver.UseVisualStyleBackColor = true;
            this.btnPrintWaiver.Click += new System.EventHandler(this.btnPrintWaiver_Click);
            // 
            // btnViewSignedWaiver
            // 
            this.btnViewSignedWaiver.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnViewSignedWaiver.BackgroundImage")));
            this.btnViewSignedWaiver.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnViewSignedWaiver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewSignedWaiver.ForeColor = System.Drawing.Color.White;
            this.btnViewSignedWaiver.Location = new System.Drawing.Point(204, 434);
            this.btnViewSignedWaiver.Name = "btnViewSignedWaiver";
            this.btnViewSignedWaiver.Size = new System.Drawing.Size(104, 36);
            this.btnViewSignedWaiver.TabIndex = 10;
            this.btnViewSignedWaiver.Text = "View";
            this.btnViewSignedWaiver.UseVisualStyleBackColor = true;
            this.btnViewSignedWaiver.Click += new System.EventHandler(this.btnViewSignedWaiver_Click);
            // 
            // btnDeactivate
            // 
            this.btnDeactivate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeactivate.BackgroundImage")));
            this.btnDeactivate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDeactivate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeactivate.ForeColor = System.Drawing.Color.White;
            this.btnDeactivate.Location = new System.Drawing.Point(41, 434);
            this.btnDeactivate.Name = "btnDeactivate";
            this.btnDeactivate.Size = new System.Drawing.Size(104, 36);
            this.btnDeactivate.TabIndex = 9;
            this.btnDeactivate.Text = "Deactivate";
            this.btnDeactivate.UseVisualStyleBackColor = true;
            this.btnDeactivate.Click += new System.EventHandler(this.btnDeactivate_Click);
            // 
            // hScrollCustSignedWaivers
            // 
            this.hScrollCustSignedWaivers.AutoHide = false;
            this.hScrollCustSignedWaivers.DataGridView = this.dgvCustomerSignedWaivers;
            this.hScrollCustSignedWaivers.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollCustSignedWaivers.LeftButtonBackgroundImage")));
            this.hScrollCustSignedWaivers.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollCustSignedWaivers.LeftButtonDisabledBackgroundImage")));
            this.hScrollCustSignedWaivers.Location = new System.Drawing.Point(3, 365);
            this.hScrollCustSignedWaivers.Margin = new System.Windows.Forms.Padding(0);
            this.hScrollCustSignedWaivers.Name = "hScrollCustSignedWaivers";
            this.hScrollCustSignedWaivers.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollCustSignedWaivers.RightButtonBackgroundImage")));
            this.hScrollCustSignedWaivers.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollCustSignedWaivers.RightButtonDisabledBackgroundImage")));
            this.hScrollCustSignedWaivers.ScrollableControl = null;
            this.hScrollCustSignedWaivers.ScrollViewer = null;
            this.hScrollCustSignedWaivers.Size = new System.Drawing.Size(680, 46);
            this.hScrollCustSignedWaivers.TabIndex = 8;
            // 
            // dgvCustomerSignedWaivers
            // 
            this.dgvCustomerSignedWaivers.AllowUserToAddRows = false;
            this.dgvCustomerSignedWaivers.AllowUserToDeleteRows = false;
            this.dgvCustomerSignedWaivers.AllowUserToResizeColumns = false;
            this.dgvCustomerSignedWaivers.AllowUserToResizeRows = false;
            this.dgvCustomerSignedWaivers.AutoGenerateColumns = false;
            this.dgvCustomerSignedWaivers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomerSignedWaivers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectRecord,
            this.customerSignedWaiverIdDataGridViewTextBoxColumn,
            this.WaiverSetDescription,
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn,
            this.waiverSetDetailId,
            this.WaiverName,
            this.signedWaiverFileNameDataGridViewTextBoxColumn,
            this.signedForNameDataGridViewTextBoxColumn,
            this.signedByNameDataGridViewTextBoxColumn,
            this.signedDateDataGridViewTextBoxColumn,
            this.waiverCodeDataGridViewTextBoxColumn,
            this.expiryDateDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.deactivationDateDataGridViewTextBoxColumn,
            this.deactivatedBy,
            this.deactivationApprovedBy,
            this.guidDataGridViewTextBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn,
            this.signedByDataGridViewTextBoxColumn,
            this.signedFor});
            this.dgvCustomerSignedWaivers.DataSource = this.customerSignedWaiverDTOBindingSource;
            this.dgvCustomerSignedWaivers.Location = new System.Drawing.Point(3, 0);
            this.dgvCustomerSignedWaivers.MultiSelect = false;
            this.dgvCustomerSignedWaivers.Name = "dgvCustomerSignedWaivers";
            this.dgvCustomerSignedWaivers.ReadOnly = true;
            this.dgvCustomerSignedWaivers.RowHeadersVisible = false;
            this.dgvCustomerSignedWaivers.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvCustomerSignedWaivers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvCustomerSignedWaivers.Size = new System.Drawing.Size(680, 365);
            this.dgvCustomerSignedWaivers.TabIndex = 0;
            this.dgvCustomerSignedWaivers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomerSignedWaivers_CellClick);
            this.dgvCustomerSignedWaivers.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvCustomerSignedWaivers_DataError);
            // 
            // customerSignedWaiverDTOBindingSource
            // 
            this.customerSignedWaiverDTOBindingSource.DataSource = typeof(Semnox.Parafait.Customer.Waivers.CustomerSignedWaiverDTO);
            // 
            // verticalScrollBarView2
            // 
            this.verticalScrollBarView2.AutoHide = false;
            this.verticalScrollBarView2.DataGridView = this.dgvCustomerSignedWaivers;
            this.verticalScrollBarView2.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.DownButtonBackgroundImage")));
            this.verticalScrollBarView2.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarView2.Location = new System.Drawing.Point(683, 0);
            this.verticalScrollBarView2.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView2.Name = "verticalScrollBarView2";
            this.verticalScrollBarView2.ScrollableControl = null;
            this.verticalScrollBarView2.ScrollViewer = null;
            this.verticalScrollBarView2.Size = new System.Drawing.Size(47, 365);
            this.verticalScrollBarView2.TabIndex = 7;
            this.verticalScrollBarView2.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.UpButtonBackgroundImage")));
            this.verticalScrollBarView2.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.UpButtonDisabledBackgroundImage")));
            // 
            // pnlBase
            // 
            //this.pnlBase.AutoScroll = true;
            this.pnlBase.Controls.Add(this.pnlCustomer);
            this.pnlBase.Controls.Add(this.pnlWaivers);
            this.pnlBase.Location = new System.Drawing.Point(2, 0);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(1310, 555);
            this.pnlBase.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Customer Name";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 250;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 250;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.HeaderText = "Relation";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Relation Id";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "CustomerSignedWaiverId";
            this.dataGridViewTextBoxColumn4.HeaderText = "Id";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "WaiverSetDescription";
            this.dataGridViewTextBoxColumn5.HeaderText = "Waiver Set Description";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 200;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "CustomerSignedWaiverHeaderId";
            this.dataGridViewTextBoxColumn6.HeaderText = "CustomerSignedWaiverHeaderId";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "WaiverSetDetailId";
            this.dataGridViewTextBoxColumn7.HeaderText = "Waiver Id";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn7.Visible = false;
            this.dataGridViewTextBoxColumn7.Width = 200;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "WaiverName";
            this.dataGridViewTextBoxColumn8.HeaderText = "Waiver Name";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "SignedWaiverFileName";
            this.dataGridViewTextBoxColumn9.HeaderText = "SignedWaiverFileName";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "SignedFor";
            this.dataGridViewTextBoxColumn10.HeaderText = "SignedFor";
            this.dataGridViewTextBoxColumn10.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn10.Visible = false;
            this.dataGridViewTextBoxColumn10.Width = 200;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "ExpiryDate";
            this.dataGridViewTextBoxColumn11.HeaderText = "Expiry Date";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "SignedForName";
            this.dataGridViewTextBoxColumn12.HeaderText = "Signed For";
            this.dataGridViewTextBoxColumn12.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Width = 200;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "SignedByName";
            this.dataGridViewTextBoxColumn13.HeaderText = "Signed By";
            this.dataGridViewTextBoxColumn13.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Width = 200;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "SignedDate";
            this.dataGridViewTextBoxColumn14.HeaderText = "Signed Date";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "WaiverCode";
            this.dataGridViewTextBoxColumn15.HeaderText = "Waiver Code";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "DeactivationDate";
            this.dataGridViewTextBoxColumn16.HeaderText = "Deactivation Date";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.DataPropertyName = "Guid";
            this.dataGridViewTextBoxColumn17.HeaderText = "Guid";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.ReadOnly = true;
            this.dataGridViewTextBoxColumn17.Visible = false;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.DataPropertyName = "MasterEntityId";
            this.dataGridViewTextBoxColumn18.HeaderText = "Master Entity Id";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.ReadOnly = true;
            this.dataGridViewTextBoxColumn18.Visible = false;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.DataPropertyName = "CreatedBy";
            this.dataGridViewTextBoxColumn19.HeaderText = "CreatedBy";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn20
            // 
            this.dataGridViewTextBoxColumn20.DataPropertyName = "CreationDate";
            this.dataGridViewTextBoxColumn20.HeaderText = "CreationDate";
            this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
            this.dataGridViewTextBoxColumn20.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn21
            // 
            this.dataGridViewTextBoxColumn21.DataPropertyName = "LastUpdatedBy";
            this.dataGridViewTextBoxColumn21.HeaderText = "LastUpdatedBy";
            this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
            this.dataGridViewTextBoxColumn21.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn22
            // 
            this.dataGridViewTextBoxColumn22.DataPropertyName = "LastUpdateDate";
            this.dataGridViewTextBoxColumn22.HeaderText = "LastUpdateDate";
            this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
            this.dataGridViewTextBoxColumn22.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn23
            // 
            this.dataGridViewTextBoxColumn23.DataPropertyName = "SignedBy";
            this.dataGridViewTextBoxColumn23.HeaderText = "SignedBy";
            this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
            this.dataGridViewTextBoxColumn23.ReadOnly = true;
            this.dataGridViewTextBoxColumn23.Visible = false;
            // 
            // selectRecord
            // 
            this.selectRecord.FalseValue = "false";
            this.selectRecord.Frozen = true;
            this.selectRecord.HeaderText = "";
            this.selectRecord.MinimumWidth = 60;
            this.selectRecord.Name = "selectRecord";
            this.selectRecord.ReadOnly = true;
            this.selectRecord.TrueValue = "true";
            this.selectRecord.Width = 60;
            // 
            // customerSignedWaiverIdDataGridViewTextBoxColumn
            // 
            this.customerSignedWaiverIdDataGridViewTextBoxColumn.DataPropertyName = "CustomerSignedWaiverId";
            this.customerSignedWaiverIdDataGridViewTextBoxColumn.HeaderText = "Id";
            this.customerSignedWaiverIdDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.customerSignedWaiverIdDataGridViewTextBoxColumn.Name = "customerSignedWaiverIdDataGridViewTextBoxColumn";
            this.customerSignedWaiverIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // WaiverSetDescription
            // 
            this.WaiverSetDescription.DataPropertyName = "WaiverSetDescription";
            this.WaiverSetDescription.HeaderText = "Waiver Set Description";
            this.WaiverSetDescription.MinimumWidth = 200;
            this.WaiverSetDescription.Name = "WaiverSetDescription";
            this.WaiverSetDescription.ReadOnly = true;
            this.WaiverSetDescription.Width = 200;
            // 
            // customerSignedWaiverHeaderIdDataGridViewTextBoxColumn
            // 
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn.DataPropertyName = "CustomerSignedWaiverHeaderId";
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn.HeaderText = "CustomerSignedWaiverHeaderId";
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn.Name = "customerSignedWaiverHeaderIdDataGridViewTextBoxColumn";
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerSignedWaiverHeaderIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // waiverSetDetailId
            // 
            this.waiverSetDetailId.DataPropertyName = "WaiverSetDetailId";
            this.waiverSetDetailId.HeaderText = "Waiver Id";
            this.waiverSetDetailId.MinimumWidth = 200;
            this.waiverSetDetailId.Name = "waiverSetDetailId";
            this.waiverSetDetailId.ReadOnly = true;
            this.waiverSetDetailId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.waiverSetDetailId.Visible = false;
            this.waiverSetDetailId.Width = 200;
            // 
            // WaiverName
            // 
            this.WaiverName.DataPropertyName = "WaiverName";
            this.WaiverName.HeaderText = "Waiver Name";
            this.WaiverName.Name = "WaiverName";
            this.WaiverName.ReadOnly = true;
            // 
            // signedWaiverFileNameDataGridViewTextBoxColumn
            // 
            this.signedWaiverFileNameDataGridViewTextBoxColumn.DataPropertyName = "SignedWaiverFileName";
            this.signedWaiverFileNameDataGridViewTextBoxColumn.HeaderText = "SignedWaiverFileName";
            this.signedWaiverFileNameDataGridViewTextBoxColumn.Name = "signedWaiverFileNameDataGridViewTextBoxColumn";
            this.signedWaiverFileNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.signedWaiverFileNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // signedForNameDataGridViewTextBoxColumn
            // 
            this.signedForNameDataGridViewTextBoxColumn.DataPropertyName = "SignedForName";
            this.signedForNameDataGridViewTextBoxColumn.HeaderText = "Signed For";
            this.signedForNameDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.signedForNameDataGridViewTextBoxColumn.Name = "signedForNameDataGridViewTextBoxColumn";
            this.signedForNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.signedForNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // signedByNameDataGridViewTextBoxColumn
            // 
            this.signedByNameDataGridViewTextBoxColumn.DataPropertyName = "SignedByName";
            this.signedByNameDataGridViewTextBoxColumn.HeaderText = "Signed By";
            this.signedByNameDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.signedByNameDataGridViewTextBoxColumn.Name = "signedByNameDataGridViewTextBoxColumn";
            this.signedByNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.signedByNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // signedDateDataGridViewTextBoxColumn
            // 
            this.signedDateDataGridViewTextBoxColumn.DataPropertyName = "SignedDate";
            this.signedDateDataGridViewTextBoxColumn.HeaderText = "Signed Date";
            this.signedDateDataGridViewTextBoxColumn.Name = "signedDateDataGridViewTextBoxColumn";
            this.signedDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // waiverCodeDataGridViewTextBoxColumn
            // 
            this.waiverCodeDataGridViewTextBoxColumn.DataPropertyName = "WaiverCode";
            this.waiverCodeDataGridViewTextBoxColumn.HeaderText = "Waiver Code";
            this.waiverCodeDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.waiverCodeDataGridViewTextBoxColumn.Name = "waiverCodeDataGridViewTextBoxColumn";
            this.waiverCodeDataGridViewTextBoxColumn.ReadOnly = true;
            this.waiverCodeDataGridViewTextBoxColumn.Width = 150;
            // 
            // expiryDateDataGridViewTextBoxColumn
            // 
            this.expiryDateDataGridViewTextBoxColumn.DataPropertyName = "ExpiryDate";
            this.expiryDateDataGridViewTextBoxColumn.HeaderText = "Expiry Date";
            this.expiryDateDataGridViewTextBoxColumn.Name = "expiryDateDataGridViewTextBoxColumn";
            this.expiryDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "false";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // deactivationDateDataGridViewTextBoxColumn
            // 
            this.deactivationDateDataGridViewTextBoxColumn.DataPropertyName = "DeactivationDate";
            this.deactivationDateDataGridViewTextBoxColumn.HeaderText = "Deactivation Date";
            this.deactivationDateDataGridViewTextBoxColumn.Name = "deactivationDateDataGridViewTextBoxColumn";
            this.deactivationDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.deactivationDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // deactivatedBy
            // 
            this.deactivatedBy.DataPropertyName = "DeactivatedBy";
            this.deactivatedBy.HeaderText = "Deactivated By";
            this.deactivatedBy.MinimumWidth = 200;
            this.deactivatedBy.Name = "deactivatedBy";
            this.deactivatedBy.ReadOnly = true;
            this.deactivatedBy.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.deactivatedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.deactivatedBy.Visible = false;
            this.deactivatedBy.Width = 200;
            // 
            // deactivationApprovedBy
            // 
            this.deactivationApprovedBy.DataPropertyName = "DeactivationApprovedBy";
            this.deactivationApprovedBy.HeaderText = "Deactivation Approved By";
            this.deactivationApprovedBy.MinimumWidth = 200;
            this.deactivationApprovedBy.Name = "deactivationApprovedBy";
            this.deactivationApprovedBy.ReadOnly = true;
            this.deactivationApprovedBy.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.deactivationApprovedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.deactivationApprovedBy.Visible = false;
            this.deactivationApprovedBy.Width = 200;
            // 
            // guidDataGridViewTextBoxColumn
            // 
            this.guidDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn.Name = "guidDataGridViewTextBoxColumn";
            this.guidDataGridViewTextBoxColumn.ReadOnly = true;
            this.guidDataGridViewTextBoxColumn.Visible = false;
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "Master Entity Id";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "Created By";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "Creation Date";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Last Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "Last Update Date";
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // signedByDataGridViewTextBoxColumn
            // 
            this.signedByDataGridViewTextBoxColumn.DataPropertyName = "SignedBy";
            this.signedByDataGridViewTextBoxColumn.HeaderText = "SignedBy";
            this.signedByDataGridViewTextBoxColumn.Name = "signedByDataGridViewTextBoxColumn";
            this.signedByDataGridViewTextBoxColumn.ReadOnly = true;
            this.signedByDataGridViewTextBoxColumn.Visible = false;
            // 
            // signedFor
            // 
            this.signedFor.DataPropertyName = "SignedFor";
            this.signedFor.HeaderText = "SignedFor";
            this.signedFor.MinimumWidth = 200;
            this.signedFor.Name = "signedFor";
            this.signedFor.ReadOnly = true;
            this.signedFor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.signedFor.Visible = false;
            this.signedFor.Width = 200;
            // 
            // frmCustomerWaiverUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1313, 560);
            this.Controls.Add(this.pnlBase);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCustomerWaiverUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Waiver";
            this.Load += new System.EventHandler(this.frmCustomerWaiverUI_Load);
            this.pnlCustomer.ResumeLayout(false);
            this.grpbxCustomer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            this.pnlWaivers.ResumeLayout(false);
            this.grpBxSignWaiver.ResumeLayout(false);
            this.tcWaivers.ResumeLayout(false);
            this.tpSignWaivers.ResumeLayout(false);
            this.pnlSignWaivers.ResumeLayout(false);
            this.tPnlSignWaiverHeader.ResumeLayout(false);
            this.tpViewSignedWaivers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerSignedWaivers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerSignedWaiverDTOBindingSource)).EndInit();
            this.pnlBase.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlCustomer;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSignWaiver;
        private System.Windows.Forms.Label lblSelectCustomerMsg;
        private System.Windows.Forms.DataGridView dgvCustomers;
        private VerticalScrollBarView vScrollCustomers;
        private System.Windows.Forms.Panel pnlWaivers;
        private System.Windows.Forms.TabControl tcWaivers;
        private System.Windows.Forms.TabPage tpSignWaivers;
        private System.Windows.Forms.TabPage tpViewSignedWaivers;
        private HorizontalScrollBarView hScrollCustSignedWaivers;
        private VerticalScrollBarView verticalScrollBarView2;
        private System.Windows.Forms.DataGridView dgvCustomerSignedWaivers;
        private VerticalScrollBarView vScrollSignWaivers;
        private System.Windows.Forms.Panel pnlSignWaivers;
        private System.Windows.Forms.BindingSource customerSignedWaiverDTOBindingSource;
        private System.Windows.Forms.Button btnEmailWaiver;
        private System.Windows.Forms.Button btnPrintWaiver;
        private System.Windows.Forms.Button btnViewSignedWaiver;
        private System.Windows.Forms.Button btnDeactivate;
        private System.Windows.Forms.Label lblCustomerHasSigned;
        private System.Windows.Forms.Label lblValidityDays;
        private System.Windows.Forms.Label lblViewWaiver;
        private System.Windows.Forms.Label lblWaiveName;
        private System.Windows.Forms.Label lblWaiverSetName;
        private System.Windows.Forms.GroupBox grpBxSignWaiver;
        private System.Windows.Forms.GroupBox grpbxCustomer;
        private System.Windows.Forms.TableLayoutPanel tPnlSignWaiverHeader;
        private HorizontalScrollBarView hScrollSignWaivers;
        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.DataGridViewCheckBoxColumn signFor;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerRelationType;
        private System.Windows.Forms.DataGridViewTextBoxColumn RelationshipTypeId;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private CustomCheckBox chkSelectAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectRecord;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerSignedWaiverIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn WaiverSetDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerSignedWaiverHeaderIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverSetDetailId;
        private System.Windows.Forms.DataGridViewTextBoxColumn WaiverName;
        private System.Windows.Forms.DataGridViewTextBoxColumn signedWaiverFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signedForNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signedByNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expiryDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deactivationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn deactivatedBy;
        private System.Windows.Forms.DataGridViewComboBoxColumn deactivationApprovedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signedFor;
    }
}