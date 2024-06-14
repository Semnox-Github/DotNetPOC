using Semnox.Core.GenericUtilities;

namespace Parafait_POS.Waivers
{
    partial class frmMapWaiversToTransaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMapWaiversToTransaction));
            this.lblSample = new System.Windows.Forms.Label();
            this.fpnlCustomerSelection = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSigned = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblSelectCustomers = new System.Windows.Forms.Label();
            this.fPnlProductWaiverMap = new System.Windows.Forms.FlowLayoutPanel();
            this.lblWaiverCode = new System.Windows.Forms.Label();
            this.txtWaiverCode = new System.Windows.Forms.TextBox();
            this.btnGetWaiverCodeCustomer = new System.Windows.Forms.Button();
            this.btnNewCustomer = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblWaiverAssignment = new System.Windows.Forms.Label();
            this.pnlBase = new System.Windows.Forms.Panel();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.gbxCustomers = new System.Windows.Forms.GroupBox();
            this.pnlCustSearchTab = new System.Windows.Forms.Panel();
            this.btnSearchCustomer = new System.Windows.Forms.Button();
            this.pbxRelatedCustomerIcon = new System.Windows.Forms.PictureBox();
            this.pbxPhoneIcon = new System.Windows.Forms.PictureBox();
            this.btnEmailSearchOption = new System.Windows.Forms.Button();
            this.btnPhoneSearchOption = new System.Windows.Forms.Button();
            this.btnClearSearchFilter = new System.Windows.Forms.Button();
            this.pbxChannelIcon = new System.Windows.Forms.PictureBox();
            this.cmbChannels = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.vScrollBarCustomerSearch = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.fpnlCustomerSearch = new System.Windows.Forms.FlowLayoutPanel();
            this.cbxGetRelatedCustomers = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.txtPhoneSearch = new Semnox.Parafait.User.CueTextBox();
            this.pbxEmailIcon = new System.Windows.Forms.PictureBox();
            this.txtEmailSearch = new Semnox.Parafait.User.CueTextBox();
            this.txtLastNameSearch = new Semnox.Parafait.User.CueTextBox();
            this.pbxNameIcon = new System.Windows.Forms.PictureBox();
            this.txtFirstNameSearch = new Semnox.Parafait.User.CueTextBox();
            this.pnlSelectedCustHeader = new System.Windows.Forms.Panel();
            this.lblSelectCustHFillerA = new System.Windows.Forms.Label();
            this.pbxRemove = new System.Windows.Forms.PictureBox();
            this.chkSelectAll = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.btnExpandCollapseSelected = new System.Windows.Forms.Button();
            this.lblSelectCustHFillerB = new System.Windows.Forms.Label();
            this.pnlCustSearchHeader = new System.Windows.Forms.Panel();
            this.btnLatestSignedCustomers = new System.Windows.Forms.Button();
            this.lblSearchCustomer = new System.Windows.Forms.Label();
            this.lblCustSearchFillerA = new System.Windows.Forms.Label();
            this.btnExpandCollapseSearch = new System.Windows.Forms.Button();
            this.lblCustSearchFiller = new System.Windows.Forms.Label();
            this.pnlSelectedCustTab = new System.Windows.Forms.Panel();
            this.vScrollBarCustomerSelection = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.lblDivider = new System.Windows.Forms.Label();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.gbxWaivers = new System.Windows.Forms.GroupBox();
            this.vScrollBarProductCustomerMap = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.btnOverridePending = new System.Windows.Forms.Button();
            this.hScrollBarProductCustomerMap = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.bgwCustomerPanel = new System.ComponentModel.BackgroundWorker();
            this.bgwCustomerSearchPanel = new System.ComponentModel.BackgroundWorker();
            this.bgwTransactionPanel = new System.ComponentModel.BackgroundWorker();
            this.bgwDoDefaultMap = new System.ComponentModel.BackgroundWorker();
            this.pnlBase.SuspendLayout();
            this.gbxCustomers.SuspendLayout();
            this.pnlCustSearchTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxRelatedCustomerIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPhoneIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxChannelIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxEmailIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNameIcon)).BeginInit();
            this.pnlSelectedCustHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxRemove)).BeginInit();
            this.pnlCustSearchHeader.SuspendLayout();
            this.pnlSelectedCustTab.SuspendLayout();
            this.gbxWaivers.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSample
            // 
            this.lblSample.AutoSize = true;
            this.lblSample.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSample.Location = new System.Drawing.Point(386, 541);
            this.lblSample.Name = "lblSample";
            this.lblSample.Size = new System.Drawing.Size(49, 15);
            this.lblSample.TabIndex = 39;
            this.lblSample.Text = "sample";
            this.lblSample.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSample.Visible = false;
            // 
            // fpnlCustomerSelection
            // 
            this.fpnlCustomerSelection.AutoScroll = true;
            this.fpnlCustomerSelection.Location = new System.Drawing.Point(3, 0);
            this.fpnlCustomerSelection.Name = "fpnlCustomerSelection";
            this.fpnlCustomerSelection.Size = new System.Drawing.Size(436, 440);
            this.fpnlCustomerSelection.TabIndex = 0;
            // 
            // lblSigned
            // 
            this.lblSigned.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSigned.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblSigned.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblSigned.Location = new System.Drawing.Point(312, 0);
            this.lblSigned.Name = "lblSigned";
            this.lblSigned.Size = new System.Drawing.Size(106, 24);
            this.lblSigned.TabIndex = 2;
            this.lblSigned.Text = "Signed";
            this.lblSigned.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSigned.Visible = false;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCustomerName.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblCustomerName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerName.Location = new System.Drawing.Point(2, 0);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(262, 24);
            this.lblCustomerName.TabIndex = 1;
            this.lblCustomerName.Text = "Customers";
            this.lblCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCustomerName.Visible = false;
            // 
            // lblSelectCustomers
            // 
            this.lblSelectCustomers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectCustomers.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblSelectCustomers.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblSelectCustomers.Location = new System.Drawing.Point(82, 0);
            this.lblSelectCustomers.Name = "lblSelectCustomers";
            this.lblSelectCustomers.Size = new System.Drawing.Size(180, 33);
            this.lblSelectCustomers.TabIndex = 0;
            this.lblSelectCustomers.Text = "Select Customers";
            this.lblSelectCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fPnlProductWaiverMap
            // 
            this.fPnlProductWaiverMap.AutoScroll = true;
            this.fPnlProductWaiverMap.Location = new System.Drawing.Point(2, 43);
            this.fPnlProductWaiverMap.Name = "fPnlProductWaiverMap";
            this.fPnlProductWaiverMap.Size = new System.Drawing.Size(749, 456);
            this.fPnlProductWaiverMap.TabIndex = 1;
            this.fPnlProductWaiverMap.WrapContents = false;
            // 
            // lblWaiverCode
            // 
            this.lblWaiverCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWaiverCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblWaiverCode.Location = new System.Drawing.Point(3, 556);
            this.lblWaiverCode.Name = "lblWaiverCode";
            this.lblWaiverCode.Size = new System.Drawing.Size(85, 24);
            this.lblWaiverCode.TabIndex = 2;
            this.lblWaiverCode.Text = "Waiver Code: ";
            this.lblWaiverCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtWaiverCode
            // 
            this.txtWaiverCode.Font = new System.Drawing.Font("Arial", 15F);
            this.txtWaiverCode.Location = new System.Drawing.Point(88, 553);
            this.txtWaiverCode.MaxLength = 10;
            this.txtWaiverCode.Name = "txtWaiverCode";
            this.txtWaiverCode.Size = new System.Drawing.Size(118, 30);
            this.txtWaiverCode.TabIndex = 3;
            // 
            // btnGetWaiverCodeCustomer
            // 
            this.btnGetWaiverCodeCustomer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGetWaiverCodeCustomer.BackgroundImage")));
            this.btnGetWaiverCodeCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGetWaiverCodeCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetWaiverCodeCustomer.ForeColor = System.Drawing.Color.White;
            this.btnGetWaiverCodeCustomer.Location = new System.Drawing.Point(212, 550);
            this.btnGetWaiverCodeCustomer.Name = "btnGetWaiverCodeCustomer";
            this.btnGetWaiverCodeCustomer.Size = new System.Drawing.Size(82, 36);
            this.btnGetWaiverCodeCustomer.TabIndex = 5;
            this.btnGetWaiverCodeCustomer.Text = "Get";
            this.btnGetWaiverCodeCustomer.UseVisualStyleBackColor = true;
            this.btnGetWaiverCodeCustomer.Click += new System.EventHandler(this.btnGetWaiverCodeCustomer_Click);
            // 
            // btnNewCustomer
            // 
            this.btnNewCustomer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNewCustomer.BackgroundImage")));
            this.btnNewCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNewCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewCustomer.ForeColor = System.Drawing.Color.White;
            this.btnNewCustomer.Location = new System.Drawing.Point(296, 550);
            this.btnNewCustomer.Name = "btnNewCustomer";
            this.btnNewCustomer.Size = new System.Drawing.Size(130, 36);
            this.btnNewCustomer.TabIndex = 7;
            this.btnNewCustomer.Text = "Register Customer";
            this.btnNewCustomer.UseVisualStyleBackColor = true;
            this.btnNewCustomer.Click += new System.EventHandler(this.btnNewCustomer_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOkay.BackgroundImage")));
            this.btnOkay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOkay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkay.ForeColor = System.Drawing.Color.White;
            this.btnOkay.Location = new System.Drawing.Point(166, 550);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(104, 36);
            this.btnOkay.TabIndex = 8;
            this.btnOkay.Text = "Ok";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(486, 550);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 36);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblWaiverAssignment
            // 
            this.lblWaiverAssignment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWaiverAssignment.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblWaiverAssignment.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblWaiverAssignment.Location = new System.Drawing.Point(2, 10);
            this.lblWaiverAssignment.Name = "lblWaiverAssignment";
            this.lblWaiverAssignment.Size = new System.Drawing.Size(758, 33);
            this.lblWaiverAssignment.TabIndex = 12;
            this.lblWaiverAssignment.Text = "Waiver Assignment";
            this.lblWaiverAssignment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlBase
            // 
            this.pnlBase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBase.AutoScroll = true;
            this.pnlBase.Controls.Add(this.txtMessage);
            this.pnlBase.Controls.Add(this.gbxCustomers);
            this.pnlBase.Controls.Add(this.gbxWaivers);
            this.pnlBase.Controls.Add(this.lblSample);
            this.pnlBase.Location = new System.Drawing.Point(1, 2);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(1241, 615);
            this.pnlBase.TabIndex = 40;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.MistyRose;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 593);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(1241, 22);
            this.txtMessage.TabIndex = 42;
            // 
            // gbxCustomers
            // 
            this.gbxCustomers.Controls.Add(this.pnlCustSearchTab);
            this.gbxCustomers.Controls.Add(this.pnlSelectedCustHeader);
            this.gbxCustomers.Controls.Add(this.pnlCustSearchHeader);
            this.gbxCustomers.Controls.Add(this.pnlSelectedCustTab);
            this.gbxCustomers.Controls.Add(this.lblWaiverCode);
            this.gbxCustomers.Controls.Add(this.btnShowKeyPad);
            this.gbxCustomers.Controls.Add(this.btnGetWaiverCodeCustomer);
            this.gbxCustomers.Controls.Add(this.txtWaiverCode);
            this.gbxCustomers.Controls.Add(this.btnNewCustomer);
            this.gbxCustomers.Location = new System.Drawing.Point(3, -7);
            this.gbxCustomers.Name = "gbxCustomers";
            this.gbxCustomers.Size = new System.Drawing.Size(469, 594);
            this.gbxCustomers.TabIndex = 40;
            this.gbxCustomers.TabStop = false;
            // 
            // pnlCustSearchTab
            // 
            this.pnlCustSearchTab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCustSearchTab.Controls.Add(this.btnSearchCustomer);
            this.pnlCustSearchTab.Controls.Add(this.pbxRelatedCustomerIcon);
            this.pnlCustSearchTab.Controls.Add(this.pbxPhoneIcon);
            this.pnlCustSearchTab.Controls.Add(this.btnEmailSearchOption);
            this.pnlCustSearchTab.Controls.Add(this.btnPhoneSearchOption);
            this.pnlCustSearchTab.Controls.Add(this.btnClearSearchFilter);
            this.pnlCustSearchTab.Controls.Add(this.pbxChannelIcon);
            this.pnlCustSearchTab.Controls.Add(this.cmbChannels);
            this.pnlCustSearchTab.Controls.Add(this.vScrollBarCustomerSearch);
            this.pnlCustSearchTab.Controls.Add(this.fpnlCustomerSearch);
            this.pnlCustSearchTab.Controls.Add(this.cbxGetRelatedCustomers);
            this.pnlCustSearchTab.Controls.Add(this.txtPhoneSearch);
            this.pnlCustSearchTab.Controls.Add(this.pbxEmailIcon);
            this.pnlCustSearchTab.Controls.Add(this.txtEmailSearch);
            this.pnlCustSearchTab.Controls.Add(this.txtLastNameSearch);
            this.pnlCustSearchTab.Controls.Add(this.pbxNameIcon);
            this.pnlCustSearchTab.Controls.Add(this.txtFirstNameSearch);
            this.pnlCustSearchTab.Location = new System.Drawing.Point(3, 44);
            this.pnlCustSearchTab.Name = "pnlCustSearchTab";
            this.pnlCustSearchTab.Size = new System.Drawing.Size(462, 458);
            this.pnlCustSearchTab.TabIndex = 28;
            // 
            // btnSearchCustomer
            // 
            this.btnSearchCustomer.BackgroundImage = global::Parafait_POS.Properties.Resources.SmallSearchBtn;
            this.btnSearchCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearchCustomer.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSearchCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchCustomer.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnSearchCustomer.ForeColor = System.Drawing.Color.White;
            this.btnSearchCustomer.Location = new System.Drawing.Point(334, 73);
            this.btnSearchCustomer.Name = "btnSearchCustomer";
            this.btnSearchCustomer.Size = new System.Drawing.Size(55, 32);
            this.btnSearchCustomer.TabIndex = 20;
            this.btnSearchCustomer.Tag = "Search";
            this.btnSearchCustomer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearchCustomer.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnSearchCustomer.UseVisualStyleBackColor = true;
            this.btnSearchCustomer.Click += new System.EventHandler(this.btnSearchCustomer_Click);
            // 
            // pbxRelatedCustomerIcon
            // 
            this.pbxRelatedCustomerIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbxRelatedCustomerIcon.BackgroundImage = global::Parafait_POS.Properties.Resources.RelatedCustomerIcon;
            this.pbxRelatedCustomerIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxRelatedCustomerIcon.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.pbxRelatedCustomerIcon.Location = new System.Drawing.Point(399, 5);
            this.pbxRelatedCustomerIcon.Name = "pbxRelatedCustomerIcon";
            this.pbxRelatedCustomerIcon.Size = new System.Drawing.Size(30, 28);
            this.pbxRelatedCustomerIcon.TabIndex = 53;
            this.pbxRelatedCustomerIcon.TabStop = false;
            this.pbxRelatedCustomerIcon.Tag = "Include Related Customers";
            this.pbxRelatedCustomerIcon.Click += new System.EventHandler(this.IconObject_Click);
            // 
            // pbxPhoneIcon
            // 
            this.pbxPhoneIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbxPhoneIcon.BackgroundImage = global::Parafait_POS.Properties.Resources.PhoneIcon;
            this.pbxPhoneIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxPhoneIcon.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.pbxPhoneIcon.Location = new System.Drawing.Point(5, 75);
            this.pbxPhoneIcon.Name = "pbxPhoneIcon";
            this.pbxPhoneIcon.Size = new System.Drawing.Size(30, 28);
            this.pbxPhoneIcon.TabIndex = 52;
            this.pbxPhoneIcon.TabStop = false;
            this.pbxPhoneIcon.Tag = "Phone Number";
            this.pbxPhoneIcon.Click += new System.EventHandler(this.IconObject_Click);
            this.pbxPhoneIcon.MouseEnter += new System.EventHandler(this.IconObjectMouseEnter);
            this.pbxPhoneIcon.MouseHover += new System.EventHandler(this.IconObjectMouseHover);
            // 
            // btnEmailSearchOption
            // 
            this.btnEmailSearchOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEmailSearchOption.AutoEllipsis = true;
            this.btnEmailSearchOption.BackColor = System.Drawing.Color.Transparent;
            this.btnEmailSearchOption.BackgroundImage = global::Parafait_POS.Properties.Resources.BlueToggleOn;
            this.btnEmailSearchOption.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEmailSearchOption.FlatAppearance.BorderSize = 0;
            this.btnEmailSearchOption.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEmailSearchOption.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEmailSearchOption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmailSearchOption.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmailSearchOption.ForeColor = System.Drawing.Color.White;
            this.btnEmailSearchOption.Location = new System.Drawing.Point(218, 38);
            this.btnEmailSearchOption.Margin = new System.Windows.Forms.Padding(0);
            this.btnEmailSearchOption.Name = "btnEmailSearchOption";
            this.btnEmailSearchOption.Size = new System.Drawing.Size(72, 30);
            this.btnEmailSearchOption.TabIndex = 51;
            this.btnEmailSearchOption.TabStop = false;
            this.btnEmailSearchOption.Tag = "E";
            this.btnEmailSearchOption.Text = "Exact";
            this.btnEmailSearchOption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmailSearchOption.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnEmailSearchOption.UseVisualStyleBackColor = false;
            this.btnEmailSearchOption.Click += new System.EventHandler(this.btnEmailSearchOption_Click);
            // 
            // btnPhoneSearchOption
            // 
            this.btnPhoneSearchOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPhoneSearchOption.AutoEllipsis = true;
            this.btnPhoneSearchOption.BackColor = System.Drawing.Color.Transparent;
            this.btnPhoneSearchOption.BackgroundImage = global::Parafait_POS.Properties.Resources.BlueToggleOn;
            this.btnPhoneSearchOption.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPhoneSearchOption.FlatAppearance.BorderSize = 0;
            this.btnPhoneSearchOption.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPhoneSearchOption.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPhoneSearchOption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPhoneSearchOption.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhoneSearchOption.ForeColor = System.Drawing.Color.White;
            this.btnPhoneSearchOption.Location = new System.Drawing.Point(218, 74);
            this.btnPhoneSearchOption.Margin = new System.Windows.Forms.Padding(0);
            this.btnPhoneSearchOption.Name = "btnPhoneSearchOption";
            this.btnPhoneSearchOption.Size = new System.Drawing.Size(72, 30);
            this.btnPhoneSearchOption.TabIndex = 50;
            this.btnPhoneSearchOption.TabStop = false;
            this.btnPhoneSearchOption.Tag = "E";
            this.btnPhoneSearchOption.Text = "Exact";
            this.btnPhoneSearchOption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPhoneSearchOption.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnPhoneSearchOption.UseVisualStyleBackColor = false;
            this.btnPhoneSearchOption.Click += new System.EventHandler(this.btnPhoneSearchOption_Click);
            // 
            // btnClearSearchFilter
            // 
            this.btnClearSearchFilter.BackgroundImage = global::Parafait_POS.Properties.Resources.SmallClearSearchBtn;
            this.btnClearSearchFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClearSearchFilter.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnClearSearchFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearSearchFilter.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnClearSearchFilter.ForeColor = System.Drawing.Color.White;
            this.btnClearSearchFilter.Location = new System.Drawing.Point(400, 73);
            this.btnClearSearchFilter.Name = "btnClearSearchFilter";
            this.btnClearSearchFilter.Size = new System.Drawing.Size(55, 32);
            this.btnClearSearchFilter.TabIndex = 23;
            this.btnClearSearchFilter.Tag = "Clear";
            this.btnClearSearchFilter.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClearSearchFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnClearSearchFilter.UseVisualStyleBackColor = true;
            this.btnClearSearchFilter.Click += new System.EventHandler(this.btnClearSearchFilter_Click);
            // 
            // pbxChannelIcon
            // 
            this.pbxChannelIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbxChannelIcon.BackgroundImage = global::Parafait_POS.Properties.Resources.Channel;
            this.pbxChannelIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxChannelIcon.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.pbxChannelIcon.Location = new System.Drawing.Point(300, 39);
            this.pbxChannelIcon.Name = "pbxChannelIcon";
            this.pbxChannelIcon.Size = new System.Drawing.Size(30, 28);
            this.pbxChannelIcon.TabIndex = 25;
            this.pbxChannelIcon.TabStop = false;
            this.pbxChannelIcon.Tag = "Waiver Channels";
            this.pbxChannelIcon.Click += new System.EventHandler(this.IconObject_Click);
            this.pbxChannelIcon.MouseEnter += new System.EventHandler(this.IconObjectMouseEnter);
            this.pbxChannelIcon.MouseHover += new System.EventHandler(this.IconObjectMouseHover);
            // 
            // cmbChannels
            // 
            this.cmbChannels.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbChannels.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbChannels.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbChannels.FormattingEnabled = true;
            this.cmbChannels.Location = new System.Drawing.Point(334, 38);
            this.cmbChannels.Name = "cmbChannels";
            this.cmbChannels.Size = new System.Drawing.Size(121, 31);
            this.cmbChannels.TabIndex = 24;
            // 
            // vScrollBarCustomerSearch
            // 
            this.vScrollBarCustomerSearch.AutoHide = false;
            this.vScrollBarCustomerSearch.DataGridView = null;
            this.vScrollBarCustomerSearch.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBarCustomerSearch.DownButtonBackgroundImage")));
            this.vScrollBarCustomerSearch.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBarCustomerSearch.DownButtonDisabledBackgroundImage")));
            this.vScrollBarCustomerSearch.Location = new System.Drawing.Point(415, 113);
            this.vScrollBarCustomerSearch.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollBarCustomerSearch.Name = "vScrollBarCustomerSearch";
            this.vScrollBarCustomerSearch.ScrollableControl = this.fpnlCustomerSearch;
            this.vScrollBarCustomerSearch.ScrollViewer = null;
            this.vScrollBarCustomerSearch.Size = new System.Drawing.Size(40, 340);
            this.vScrollBarCustomerSearch.TabIndex = 22;
            this.vScrollBarCustomerSearch.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBarCustomerSearch.UpButtonBackgroundImage")));
            this.vScrollBarCustomerSearch.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBarCustomerSearch.UpButtonDisabledBackgroundImage")));
            // 
            // fpnlCustomerSearch
            // 
            this.fpnlCustomerSearch.AutoScroll = true;
            this.fpnlCustomerSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpnlCustomerSearch.Location = new System.Drawing.Point(3, 112);
            this.fpnlCustomerSearch.Name = "fpnlCustomerSearch";
            this.fpnlCustomerSearch.Size = new System.Drawing.Size(454, 341);
            this.fpnlCustomerSearch.TabIndex = 21;
            // 
            // cbxGetRelatedCustomers
            // 
            this.cbxGetRelatedCustomers.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxGetRelatedCustomers.BackColor = System.Drawing.Color.Transparent;
            this.cbxGetRelatedCustomers.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxGetRelatedCustomers.FlatAppearance.BorderSize = 0;
            this.cbxGetRelatedCustomers.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cbxGetRelatedCustomers.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbxGetRelatedCustomers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbxGetRelatedCustomers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxGetRelatedCustomers.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.cbxGetRelatedCustomers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxGetRelatedCustomers.ImageIndex = 1;
            this.cbxGetRelatedCustomers.Location = new System.Drawing.Point(427, 4);
            this.cbxGetRelatedCustomers.Margin = new System.Windows.Forms.Padding(0);
            this.cbxGetRelatedCustomers.Name = "cbxGetRelatedCustomers";
            this.cbxGetRelatedCustomers.Size = new System.Drawing.Size(30, 30);
            this.cbxGetRelatedCustomers.TabIndex = 18;
            this.cbxGetRelatedCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxGetRelatedCustomers.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.cbxGetRelatedCustomers.UseVisualStyleBackColor = false;
            // 
            // txtPhoneSearch
            // 
            this.txtPhoneSearch.Cue = null;
            this.txtPhoneSearch.Font = new System.Drawing.Font("Arial", 15F);
            this.txtPhoneSearch.Location = new System.Drawing.Point(39, 74);
            this.txtPhoneSearch.MaxLength = 14;
            this.txtPhoneSearch.Name = "txtPhoneSearch";
            this.txtPhoneSearch.Size = new System.Drawing.Size(175, 30);
            this.txtPhoneSearch.TabIndex = 9;
            // 
            // pbxEmailIcon
            // 
            this.pbxEmailIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbxEmailIcon.BackgroundImage = global::Parafait_POS.Properties.Resources.EmailIcon;
            this.pbxEmailIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxEmailIcon.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.pbxEmailIcon.Location = new System.Drawing.Point(5, 39);
            this.pbxEmailIcon.Name = "pbxEmailIcon";
            this.pbxEmailIcon.Size = new System.Drawing.Size(30, 28);
            this.pbxEmailIcon.TabIndex = 7;
            this.pbxEmailIcon.TabStop = false;
            this.pbxEmailIcon.Tag = "Email Id";
            this.pbxEmailIcon.Click += new System.EventHandler(this.IconObject_Click);
            this.pbxEmailIcon.MouseEnter += new System.EventHandler(this.IconObjectMouseEnter);
            this.pbxEmailIcon.MouseHover += new System.EventHandler(this.IconObjectMouseHover);
            // 
            // txtEmailSearch
            // 
            this.txtEmailSearch.Cue = null;
            this.txtEmailSearch.Font = new System.Drawing.Font("Arial", 15F);
            this.txtEmailSearch.Location = new System.Drawing.Point(39, 38);
            this.txtEmailSearch.MaxLength = 100;
            this.txtEmailSearch.Name = "txtEmailSearch";
            this.txtEmailSearch.Size = new System.Drawing.Size(175, 30);
            this.txtEmailSearch.TabIndex = 8;
            // 
            // txtLastNameSearch
            // 
            this.txtLastNameSearch.Cue = null;
            this.txtLastNameSearch.Font = new System.Drawing.Font("Arial", 15F);
            this.txtLastNameSearch.Location = new System.Drawing.Point(218, 4);
            this.txtLastNameSearch.MaxLength = 50;
            this.txtLastNameSearch.Name = "txtLastNameSearch";
            this.txtLastNameSearch.Size = new System.Drawing.Size(175, 30);
            this.txtLastNameSearch.TabIndex = 6;
            // 
            // pbxNameIcon
            // 
            this.pbxNameIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbxNameIcon.BackgroundImage = global::Parafait_POS.Properties.Resources.ContactName;
            this.pbxNameIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxNameIcon.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.pbxNameIcon.Location = new System.Drawing.Point(5, 5);
            this.pbxNameIcon.Name = "pbxNameIcon";
            this.pbxNameIcon.Size = new System.Drawing.Size(30, 28);
            this.pbxNameIcon.TabIndex = 4;
            this.pbxNameIcon.TabStop = false;
            this.pbxNameIcon.Tag = "Customer Name";
            this.pbxNameIcon.Click += new System.EventHandler(this.IconObject_Click);
            this.pbxNameIcon.MouseEnter += new System.EventHandler(this.IconObjectMouseEnter);
            this.pbxNameIcon.MouseHover += new System.EventHandler(this.IconObjectMouseHover);
            // 
            // txtFirstNameSearch
            // 
            this.txtFirstNameSearch.Cue = null;
            this.txtFirstNameSearch.Font = new System.Drawing.Font("Arial", 15F);
            this.txtFirstNameSearch.Location = new System.Drawing.Point(39, 4);
            this.txtFirstNameSearch.MaxLength = 50;
            this.txtFirstNameSearch.Name = "txtFirstNameSearch";
            this.txtFirstNameSearch.Size = new System.Drawing.Size(175, 30);
            this.txtFirstNameSearch.TabIndex = 5;
            // 
            // pnlSelectedCustHeader
            // 
            this.pnlSelectedCustHeader.Controls.Add(this.lblSelectCustomers);
            this.pnlSelectedCustHeader.Controls.Add(this.lblSelectCustHFillerA);
            this.pnlSelectedCustHeader.Controls.Add(this.pbxRemove);
            this.pnlSelectedCustHeader.Controls.Add(this.chkSelectAll);
            this.pnlSelectedCustHeader.Controls.Add(this.btnExpandCollapseSelected);
            this.pnlSelectedCustHeader.Controls.Add(this.lblSelectCustHFillerB);
            this.pnlSelectedCustHeader.Location = new System.Drawing.Point(3, 44);
            this.pnlSelectedCustHeader.Name = "pnlSelectedCustHeader";
            this.pnlSelectedCustHeader.Size = new System.Drawing.Size(462, 33);
            this.pnlSelectedCustHeader.TabIndex = 0;
            // 
            // lblSelectCustHFillerA
            // 
            this.lblSelectCustHFillerA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectCustHFillerA.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblSelectCustHFillerA.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblSelectCustHFillerA.Location = new System.Drawing.Point(2, 0);
            this.lblSelectCustHFillerA.Name = "lblSelectCustHFillerA";
            this.lblSelectCustHFillerA.Size = new System.Drawing.Size(80, 33);
            this.lblSelectCustHFillerA.TabIndex = 134;
            this.lblSelectCustHFillerA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbxRemove
            // 
            this.pbxRemove.BackgroundImage = global::Parafait_POS.Properties.Resources.R_Remove_Btn_Normal;
            this.pbxRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxRemove.Location = new System.Drawing.Point(265, 1);
            this.pbxRemove.Name = "pbxRemove";
            this.pbxRemove.Size = new System.Drawing.Size(30, 30);
            this.pbxRemove.TabIndex = 132;
            this.pbxRemove.TabStop = false;
            this.pbxRemove.Click += new System.EventHandler(this.pbxRemove_Click);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSelectAll.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.chkSelectAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chkSelectAll.FlatAppearance.BorderSize = 0;
            this.chkSelectAll.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.chkSelectAll.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.chkSelectAll.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.chkSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSelectAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkSelectAll.ImageIndex = 1;
            this.chkSelectAll.Location = new System.Drawing.Point(306, 0);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(98, 33);
            this.chkSelectAll.TabIndex = 16;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkSelectAll.UseVisualStyleBackColor = false;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            this.chkSelectAll.Click += new System.EventHandler(this.chkSelectAll_Click);
            // 
            // btnExpandCollapseSelected
            // 
            this.btnExpandCollapseSelected.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnExpandCollapseSelected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExpandCollapseSelected.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnExpandCollapseSelected.FlatAppearance.BorderSize = 0;
            this.btnExpandCollapseSelected.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapseSelected.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnExpandCollapseSelected.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnExpandCollapseSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandCollapseSelected.ForeColor = System.Drawing.Color.White;
            this.btnExpandCollapseSelected.Image = global::Parafait_POS.Properties.Resources.CollapseArrow;
            this.btnExpandCollapseSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExpandCollapseSelected.Location = new System.Drawing.Point(400, 0);
            this.btnExpandCollapseSelected.Name = "btnExpandCollapseSelected";
            this.btnExpandCollapseSelected.Size = new System.Drawing.Size(60, 32);
            this.btnExpandCollapseSelected.TabIndex = 130;
            this.btnExpandCollapseSelected.Tag = "";
            this.btnExpandCollapseSelected.UseVisualStyleBackColor = false;
            this.btnExpandCollapseSelected.Click += new System.EventHandler(this.btnExpandCollapse_Click);
            // 
            // lblSelectCustHFillerB
            // 
            this.lblSelectCustHFillerB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectCustHFillerB.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblSelectCustHFillerB.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblSelectCustHFillerB.Location = new System.Drawing.Point(218, 0);
            this.lblSelectCustHFillerB.Name = "lblSelectCustHFillerB";
            this.lblSelectCustHFillerB.Size = new System.Drawing.Size(243, 33);
            this.lblSelectCustHFillerB.TabIndex = 131;
            this.lblSelectCustHFillerB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlCustSearchHeader
            // 
            this.pnlCustSearchHeader.Controls.Add(this.btnLatestSignedCustomers);
            this.pnlCustSearchHeader.Controls.Add(this.lblSearchCustomer);
            this.pnlCustSearchHeader.Controls.Add(this.lblCustSearchFillerA);
            this.pnlCustSearchHeader.Controls.Add(this.btnExpandCollapseSearch);
            this.pnlCustSearchHeader.Controls.Add(this.lblCustSearchFiller);
            this.pnlCustSearchHeader.Location = new System.Drawing.Point(3, 10);
            this.pnlCustSearchHeader.Name = "pnlCustSearchHeader";
            this.pnlCustSearchHeader.Size = new System.Drawing.Size(462, 33);
            this.pnlCustSearchHeader.TabIndex = 27;
            // 
            // btnLatestSignedCustomers
            // 
            this.btnLatestSignedCustomers.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnLatestSignedCustomers.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLatestSignedCustomers.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnLatestSignedCustomers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLatestSignedCustomers.ForeColor = System.Drawing.Color.White;
            this.btnLatestSignedCustomers.Location = new System.Drawing.Point(272, 1);
            this.btnLatestSignedCustomers.Name = "btnLatestSignedCustomers";
            this.btnLatestSignedCustomers.Size = new System.Drawing.Size(124, 32);
            this.btnLatestSignedCustomers.TabIndex = 19;
            this.btnLatestSignedCustomers.Text = "Get Latest Signed";
            this.btnLatestSignedCustomers.UseVisualStyleBackColor = true;
            this.btnLatestSignedCustomers.Click += new System.EventHandler(this.btnLatestSignedCustomers_Click);
            // 
            // lblSearchCustomer
            // 
            this.lblSearchCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSearchCustomer.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblSearchCustomer.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblSearchCustomer.Location = new System.Drawing.Point(82, 0);
            this.lblSearchCustomer.Name = "lblSearchCustomer";
            this.lblSearchCustomer.Size = new System.Drawing.Size(180, 33);
            this.lblSearchCustomer.TabIndex = 1;
            this.lblSearchCustomer.Text = "Search Customers";
            this.lblSearchCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustSearchFillerA
            // 
            this.lblCustSearchFillerA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCustSearchFillerA.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblCustSearchFillerA.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustSearchFillerA.Location = new System.Drawing.Point(2, 0);
            this.lblCustSearchFillerA.Name = "lblCustSearchFillerA";
            this.lblCustSearchFillerA.Size = new System.Drawing.Size(80, 33);
            this.lblCustSearchFillerA.TabIndex = 133;
            this.lblCustSearchFillerA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExpandCollapseSearch
            // 
            this.btnExpandCollapseSearch.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnExpandCollapseSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExpandCollapseSearch.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnExpandCollapseSearch.FlatAppearance.BorderSize = 0;
            this.btnExpandCollapseSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExpandCollapseSearch.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnExpandCollapseSearch.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnExpandCollapseSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandCollapseSearch.ForeColor = System.Drawing.Color.White;
            this.btnExpandCollapseSearch.Image = global::Parafait_POS.Properties.Resources.ExpandArrow;
            this.btnExpandCollapseSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExpandCollapseSearch.Location = new System.Drawing.Point(400, 0);
            this.btnExpandCollapseSearch.Name = "btnExpandCollapseSearch";
            this.btnExpandCollapseSearch.Size = new System.Drawing.Size(60, 32);
            this.btnExpandCollapseSearch.TabIndex = 131;
            this.btnExpandCollapseSearch.Tag = "";
            this.btnExpandCollapseSearch.UseVisualStyleBackColor = false;
            this.btnExpandCollapseSearch.Click += new System.EventHandler(this.btnExpandCollapse_Click);
            // 
            // lblCustSearchFiller
            // 
            this.lblCustSearchFiller.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCustSearchFiller.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblCustSearchFiller.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustSearchFiller.Location = new System.Drawing.Point(206, 0);
            this.lblCustSearchFiller.Name = "lblCustSearchFiller";
            this.lblCustSearchFiller.Size = new System.Drawing.Size(256, 33);
            this.lblCustSearchFiller.TabIndex = 132;
            this.lblCustSearchFiller.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlSelectedCustTab
            // 
            this.pnlSelectedCustTab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSelectedCustTab.Controls.Add(this.vScrollBarCustomerSelection);
            this.pnlSelectedCustTab.Controls.Add(this.lblSigned);
            this.pnlSelectedCustTab.Controls.Add(this.lblCustomerName);
            this.pnlSelectedCustTab.Controls.Add(this.lblDivider);
            this.pnlSelectedCustTab.Controls.Add(this.fpnlCustomerSelection);
            this.pnlSelectedCustTab.Location = new System.Drawing.Point(2, 80);
            this.pnlSelectedCustTab.Name = "pnlSelectedCustTab";
            this.pnlSelectedCustTab.Size = new System.Drawing.Size(462, 443);
            this.pnlSelectedCustTab.TabIndex = 0;
            // 
            // vScrollBarCustomerSelection
            // 
            this.vScrollBarCustomerSelection.AutoHide = false;
            this.vScrollBarCustomerSelection.DataGridView = null;
            this.vScrollBarCustomerSelection.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBarCustomerSelection.DownButtonBackgroundImage")));
            this.vScrollBarCustomerSelection.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBarCustomerSelection.DownButtonDisabledBackgroundImage")));
            this.vScrollBarCustomerSelection.Location = new System.Drawing.Point(414, 0);
            this.vScrollBarCustomerSelection.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollBarCustomerSelection.Name = "vScrollBarCustomerSelection";
            this.vScrollBarCustomerSelection.ScrollableControl = this.fpnlCustomerSelection;
            this.vScrollBarCustomerSelection.ScrollViewer = null;
            this.vScrollBarCustomerSelection.Size = new System.Drawing.Size(40, 440);
            this.vScrollBarCustomerSelection.TabIndex = 11;
            this.vScrollBarCustomerSelection.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBarCustomerSelection.UpButtonBackgroundImage")));
            this.vScrollBarCustomerSelection.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBarCustomerSelection.UpButtonDisabledBackgroundImage")));
            // 
            // lblDivider
            // 
            this.lblDivider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDivider.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblDivider.Location = new System.Drawing.Point(150, 0);
            this.lblDivider.Name = "lblDivider";
            this.lblDivider.Size = new System.Drawing.Size(191, 24);
            this.lblDivider.TabIndex = 12;
            this.lblDivider.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDivider.Visible = false;
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnShowKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Location = new System.Drawing.Point(433, 552);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(36, 35);
            this.btnShowKeyPad.TabIndex = 26;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            // 
            // gbxWaivers
            // 
            this.gbxWaivers.Controls.Add(this.vScrollBarProductCustomerMap);
            this.gbxWaivers.Controls.Add(this.btnOverridePending);
            this.gbxWaivers.Controls.Add(this.lblWaiverAssignment);
            this.gbxWaivers.Controls.Add(this.hScrollBarProductCustomerMap);
            this.gbxWaivers.Controls.Add(this.btnCancel);
            this.gbxWaivers.Controls.Add(this.btnOkay);
            this.gbxWaivers.Controls.Add(this.fPnlProductWaiverMap);
            this.gbxWaivers.Location = new System.Drawing.Point(472, -7);
            this.gbxWaivers.Name = "gbxWaivers";
            this.gbxWaivers.Size = new System.Drawing.Size(765, 594);
            this.gbxWaivers.TabIndex = 41;
            this.gbxWaivers.TabStop = false;
            // 
            // vScrollBarProductCustomerMap
            // 
            this.vScrollBarProductCustomerMap.AutoHide = false;
            this.vScrollBarProductCustomerMap.DataGridView = null;
            this.vScrollBarProductCustomerMap.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBarProductCustomerMap.DownButtonBackgroundImage")));
            this.vScrollBarProductCustomerMap.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBarProductCustomerMap.DownButtonDisabledBackgroundImage")));
            this.vScrollBarProductCustomerMap.Location = new System.Drawing.Point(727, 43);
            this.vScrollBarProductCustomerMap.Margin = new System.Windows.Forms.Padding(0);
            this.vScrollBarProductCustomerMap.Name = "vScrollBarProductCustomerMap";
            this.vScrollBarProductCustomerMap.ScrollableControl = this.fPnlProductWaiverMap;
            this.vScrollBarProductCustomerMap.ScrollViewer = null;
            this.vScrollBarProductCustomerMap.Size = new System.Drawing.Size(40, 480);
            this.vScrollBarProductCustomerMap.TabIndex = 0;
            this.vScrollBarProductCustomerMap.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBarProductCustomerMap.UpButtonBackgroundImage")));
            this.vScrollBarProductCustomerMap.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBarProductCustomerMap.UpButtonDisabledBackgroundImage")));
            // 
            // btnOverridePending
            // 
            this.btnOverridePending.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOverridePending.BackgroundImage")));
            this.btnOverridePending.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOverridePending.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOverridePending.ForeColor = System.Drawing.Color.White;
            this.btnOverridePending.Location = new System.Drawing.Point(317, 550);
            this.btnOverridePending.Name = "btnOverridePending";
            this.btnOverridePending.Size = new System.Drawing.Size(125, 36);
            this.btnOverridePending.TabIndex = 13;
            this.btnOverridePending.Tag = "O";
            this.btnOverridePending.Text = "Override Pending";
            this.btnOverridePending.UseVisualStyleBackColor = true;
            this.btnOverridePending.Click += new System.EventHandler(this.btnOverridePending_Click);
            // 
            // hScrollBarProductCustomerMap
            // 
            this.hScrollBarProductCustomerMap.AutoHide = false;
            this.hScrollBarProductCustomerMap.DataGridView = null;
            this.hScrollBarProductCustomerMap.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollBarProductCustomerMap.LeftButtonBackgroundImage")));
            this.hScrollBarProductCustomerMap.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollBarProductCustomerMap.LeftButtonDisabledBackgroundImage")));
            this.hScrollBarProductCustomerMap.Location = new System.Drawing.Point(3, 483);
            this.hScrollBarProductCustomerMap.Margin = new System.Windows.Forms.Padding(0);
            this.hScrollBarProductCustomerMap.Name = "hScrollBarProductCustomerMap";
            this.hScrollBarProductCustomerMap.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollBarProductCustomerMap.RightButtonBackgroundImage")));
            this.hScrollBarProductCustomerMap.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("hScrollBarProductCustomerMap.RightButtonDisabledBackgroundImage")));
            this.hScrollBarProductCustomerMap.ScrollableControl = this.fPnlProductWaiverMap;
            this.hScrollBarProductCustomerMap.ScrollViewer = null;
            this.hScrollBarProductCustomerMap.Size = new System.Drawing.Size(724, 40);
            this.hScrollBarProductCustomerMap.TabIndex = 10;
            // 
            // bgwCustomerPanel
            // 
            this.bgwCustomerPanel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwCustomerPanel_DoWork);
            this.bgwCustomerPanel.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwCustomerPanel_ProgressChanged);
            this.bgwCustomerPanel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwCustomerPanel_RunWorkerCompleted);
            // 
            // bgwCustomerSearchPanel
            // 
            this.bgwCustomerSearchPanel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwCustomerSearchPanel_DoWork);
            this.bgwCustomerSearchPanel.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwCustomerSearchPanel_ProgressChanged);
            this.bgwCustomerSearchPanel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwCustomerSearchPanel_RunWorkerCompleted);
            // 
            // bgwTransactionPanel
            // 
            this.bgwTransactionPanel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwTransactionPanel_DoWork);
            this.bgwTransactionPanel.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwTransactionPanel_ProgressChanged);
            this.bgwTransactionPanel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwTransactionPanel_RunWorkerCompleted);
            // 
            // bgwDoDefaultMap
            // 
            this.bgwDoDefaultMap.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwDoDefaultMap_DoWork);
            this.bgwDoDefaultMap.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwDoDefaultMap_ProgressChanged);
            this.bgwDoDefaultMap.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwDoDefaultMap_RunWorkerCompleted);
            // 
            // frmMapWaiversToTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1244, 617);
            this.Controls.Add(this.pnlBase);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMapWaiversToTransaction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Map Customer Waivers To Transaction";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMapWaiversToTransaction_FormClosing);
            this.Load += new System.EventHandler(this.frmMapWaiversToTransaction_Load);
            this.pnlBase.ResumeLayout(false);
            this.pnlBase.PerformLayout();
            this.gbxCustomers.ResumeLayout(false);
            this.gbxCustomers.PerformLayout();
            this.pnlCustSearchTab.ResumeLayout(false);
            this.pnlCustSearchTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxRelatedCustomerIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPhoneIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxChannelIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxEmailIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNameIcon)).EndInit();
            this.pnlSelectedCustHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxRemove)).EndInit();
            this.pnlCustSearchHeader.ResumeLayout(false);
            this.pnlSelectedCustTab.ResumeLayout(false);
            this.gbxWaivers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel fpnlCustomerSelection;
        private System.Windows.Forms.Label lblSigned;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblSelectCustomers;
        private System.Windows.Forms.FlowLayoutPanel fPnlProductWaiverMap;
        private System.Windows.Forms.Label lblWaiverCode;
        private System.Windows.Forms.TextBox txtWaiverCode;
        private System.Windows.Forms.Button btnGetWaiverCodeCustomer;
        private System.Windows.Forms.Button btnNewCustomer;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnCancel;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vScrollBarProductCustomerMap;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView hScrollBarProductCustomerMap;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vScrollBarCustomerSelection;
        private System.Windows.Forms.Label lblWaiverAssignment;
        private System.Windows.Forms.Label lblSample;
        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.GroupBox gbxCustomers;
        private System.Windows.Forms.GroupBox gbxWaivers;
        private System.Windows.Forms.Label lblDivider;
        private CustomCheckBox chkSelectAll;
        private System.Windows.Forms.Button btnOverridePending;
        private System.ComponentModel.BackgroundWorker bgwCustomerSearchPanel;
        private System.ComponentModel.BackgroundWorker bgwCustomerPanel;
        private System.ComponentModel.BackgroundWorker bgwTransactionPanel;
        private System.ComponentModel.BackgroundWorker bgwDoDefaultMap;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Panel pnlCustSearchHeader;
        private System.Windows.Forms.Panel pnlCustSearchTab;
        private System.Windows.Forms.Panel pnlSelectedCustHeader;
        private System.Windows.Forms.Panel pnlSelectedCustTab;
        private System.Windows.Forms.Button btnExpandCollapseSelected;
        private System.Windows.Forms.Label lblSearchCustomer;
        private System.Windows.Forms.Button btnExpandCollapseSearch;
        private Semnox.Parafait.User.CueTextBox txtLastNameSearch;
        private System.Windows.Forms.PictureBox pbxNameIcon;
        private Semnox.Parafait.User.CueTextBox txtFirstNameSearch;
        private Semnox.Parafait.User.CueTextBox txtPhoneSearch;
        private System.Windows.Forms.PictureBox pbxEmailIcon;
        private Semnox.Parafait.User.CueTextBox txtEmailSearch;
        private System.Windows.Forms.Button btnLatestSignedCustomers;
        private CustomCheckBox cbxGetRelatedCustomers;
        private System.Windows.Forms.Button btnSearchCustomer;
        private System.Windows.Forms.FlowLayoutPanel fpnlCustomerSearch;
        private VerticalScrollBarView vScrollBarCustomerSearch;
        private System.Windows.Forms.Button btnClearSearchFilter;
        private System.Windows.Forms.Label lblSelectCustHFillerB;
        private System.Windows.Forms.PictureBox pbxRemove;
        private System.Windows.Forms.PictureBox pbxChannelIcon;
        private AutoCompleteComboBox cmbChannels;
        private System.Windows.Forms.Label lblCustSearchFillerA;
        private System.Windows.Forms.Label lblCustSearchFiller;
        private System.Windows.Forms.Label lblSelectCustHFillerA;
        private System.Windows.Forms.TextBox txtMessage;
        protected System.Windows.Forms.Button btnEmailSearchOption;
        protected System.Windows.Forms.Button btnPhoneSearchOption;
        private System.Windows.Forms.PictureBox pbxPhoneIcon;
        private System.Windows.Forms.PictureBox pbxRelatedCustomerIcon;
    }
}