namespace Semnox.Parafait.Customer
{
    partial class CustomerListUI
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
            this.grpFilter = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAdvancedSearch = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.flpSearchFields = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCustNameSearchFields = new System.Windows.Forms.Panel();
            this.txtLastName = new Semnox.Parafait.User.CueTextBox();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.txtFirstName = new Semnox.Parafait.User.CueTextBox();
            this.txtMiddleName = new Semnox.Parafait.User.CueTextBox();
            this.pnlCustEmailSearchField = new System.Windows.Forms.Panel();
            this.btnEmailSearchOption = new System.Windows.Forms.Button();
            this.txtEmail = new Semnox.Parafait.User.CueTextBox();
            this.lblContactEmail = new System.Windows.Forms.Label();
            this.pnlCustPhSearchField = new System.Windows.Forms.Panel();
            this.lblContactPh = new System.Windows.Forms.Label();
            this.txtPhone = new Semnox.Parafait.User.CueTextBox();
            this.btnPhoneSearchOption = new System.Windows.Forms.Button();
            this.pnlCustUniqIdSearchField = new System.Windows.Forms.Panel();
            this.txtUniqueIdentifier = new System.Windows.Forms.TextBox();
            this.lblUID = new System.Windows.Forms.Label();
            this.pnlCustMembershipSearchField = new System.Windows.Forms.Panel();
            this.cmbMembership = new System.Windows.Forms.ComboBox();
            this.lblMembership = new System.Windows.Forms.Label();
            this.pnlCustSiteSearchField = new System.Windows.Forms.Panel();
            this.lblSite = new System.Windows.Forms.Label();
            this.cmbSite = new System.Windows.Forms.ComboBox();
            this.dgvCustomerDTOList = new System.Windows.Forms.DataGridView();
            this.selectDataGridViewButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.customerIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titleDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.firstNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.middleNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.membershipIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.uniqueIdentifierDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taxCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateOfBirthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.genderDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.anniversaryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teamUserDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.rightHandedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.verifiedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.companyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.designationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.externalSystemReferenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customDataSetDataGridViewButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.channelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerTypeDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.grpContact = new System.Windows.Forms.GroupBox();
            this.chbShowActiveContactEntries = new System.Windows.Forms.CheckBox();
            this.dgvContactDTOList = new System.Windows.Forms.DataGridView();
            this.contactTypeIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.attribute1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attribute2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.contactDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.grpCustomer = new System.Windows.Forms.GroupBox();
            this.lblPage = new System.Windows.Forms.Label();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tlpAddressAndContact = new System.Windows.Forms.TableLayoutPanel();
            this.grpAddress = new System.Windows.Forms.GroupBox();
            this.chbShowActiveAddressEntries = new System.Windows.Forms.CheckBox();
            this.dgvAddressDTOList = new System.Windows.Forms.DataGridView();
            this.addressTypeIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.line1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.line2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.line3DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stateIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.countryIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.postalCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.addressDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.btnRelationship = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.flpButtonControls = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAssociatedCards = new System.Windows.Forms.Button();
            this.btnUpdateMembership = new System.Windows.Forms.Button();
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
            this.cardCoreDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.grpFilter.SuspendLayout();
            this.flpSearchFields.SuspendLayout();
            this.pnlCustNameSearchFields.SuspendLayout();
            this.pnlCustEmailSearchField.SuspendLayout();
            this.pnlCustPhSearchField.SuspendLayout();
            this.pnlCustUniqIdSearchField.SuspendLayout();
            this.pnlCustMembershipSearchField.SuspendLayout();
            this.pnlCustSiteSearchField.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerDTOListBS)).BeginInit();
            this.grpContact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContactDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contactDTOListBS)).BeginInit();
            this.grpCustomer.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.tlpAddressAndContact.SuspendLayout();
            this.grpAddress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddressDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.addressDTOListBS)).BeginInit();
            this.flpButtonControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cardCoreDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // grpFilter
            // 
            this.grpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFilter.Controls.Add(this.btnClear);
            this.grpFilter.Controls.Add(this.btnAdvancedSearch);
            this.grpFilter.Controls.Add(this.btnSearch);
            this.grpFilter.Controls.Add(this.flpSearchFields);
            this.grpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpFilter.Location = new System.Drawing.Point(12, 12);
            this.grpFilter.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.Size = new System.Drawing.Size(1128, 110);
            this.grpFilter.TabIndex = 4;
            this.grpFilter.TabStop = false;
            this.grpFilter.Text = "Filter";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(1031, 76);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(87, 30);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAdvancedSearch
            // 
            this.btnAdvancedSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdvancedSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdvancedSearch.Location = new System.Drawing.Point(878, 76);
            this.btnAdvancedSearch.Name = "btnAdvancedSearch";
            this.btnAdvancedSearch.Size = new System.Drawing.Size(147, 30);
            this.btnAdvancedSearch.TabIndex = 15;
            this.btnAdvancedSearch.Text = "Advanced Search";
            this.btnAdvancedSearch.UseVisualStyleBackColor = true;
            this.btnAdvancedSearch.Click += new System.EventHandler(this.btnAdvancedSearch_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(785, 76);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(87, 30);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // flpSearchFields
            // 
            this.flpSearchFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpSearchFields.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpSearchFields.BackColor = System.Drawing.Color.Transparent;
            this.flpSearchFields.Controls.Add(this.pnlCustNameSearchFields);
            this.flpSearchFields.Controls.Add(this.pnlCustEmailSearchField);
            this.flpSearchFields.Controls.Add(this.pnlCustPhSearchField);
            this.flpSearchFields.Controls.Add(this.pnlCustUniqIdSearchField);
            this.flpSearchFields.Controls.Add(this.pnlCustMembershipSearchField);
            this.flpSearchFields.Controls.Add(this.pnlCustSiteSearchField);
            this.flpSearchFields.Location = new System.Drawing.Point(3, 8);
            this.flpSearchFields.Margin = new System.Windows.Forms.Padding(1);
            this.flpSearchFields.Name = "flpSearchFields";
            this.flpSearchFields.Size = new System.Drawing.Size(1122, 101);
            this.flpSearchFields.TabIndex = 1;
            // 
            // pnlCustNameSearchFields
            // 
            this.pnlCustNameSearchFields.Controls.Add(this.txtLastName);
            this.pnlCustNameSearchFields.Controls.Add(this.lblCustomerName);
            this.pnlCustNameSearchFields.Controls.Add(this.txtFirstName);
            this.pnlCustNameSearchFields.Controls.Add(this.txtMiddleName);
            this.pnlCustNameSearchFields.Location = new System.Drawing.Point(1, 1);
            this.pnlCustNameSearchFields.Margin = new System.Windows.Forms.Padding(1);
            this.pnlCustNameSearchFields.Name = "pnlCustNameSearchFields";
            this.pnlCustNameSearchFields.Size = new System.Drawing.Size(478, 31);
            this.pnlCustNameSearchFields.TabIndex = 23;
            // 
            // txtLastName
            // 
            this.txtLastName.Cue = "Last Name";
            this.txtLastName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtLastName.Location = new System.Drawing.Point(335, 5);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(139, 21);
            this.txtLastName.TabIndex = 5;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerName.Location = new System.Drawing.Point(4, 4);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(114, 23);
            this.lblCustomerName.TabIndex = 2;
            this.lblCustomerName.Text = "Customer Name: ";
            this.lblCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Cue = "First Name";
            this.txtFirstName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtFirstName.Location = new System.Drawing.Point(119, 5);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(125, 21);
            this.txtFirstName.TabIndex = 3;
            // 
            // txtMiddleName
            // 
            this.txtMiddleName.Cue = "Middle Name";
            this.txtMiddleName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtMiddleName.Location = new System.Drawing.Point(246, 5);
            this.txtMiddleName.Name = "txtMiddleName";
            this.txtMiddleName.Size = new System.Drawing.Size(87, 21);
            this.txtMiddleName.TabIndex = 4;
            // 
            // pnlCustEmailSearchField
            // 
            this.pnlCustEmailSearchField.Controls.Add(this.btnEmailSearchOption);
            this.pnlCustEmailSearchField.Controls.Add(this.txtEmail);
            this.pnlCustEmailSearchField.Controls.Add(this.lblContactEmail);
            this.pnlCustEmailSearchField.Location = new System.Drawing.Point(481, 1);
            this.pnlCustEmailSearchField.Margin = new System.Windows.Forms.Padding(1);
            this.pnlCustEmailSearchField.Name = "pnlCustEmailSearchField";
            this.pnlCustEmailSearchField.Size = new System.Drawing.Size(310, 31);
            this.pnlCustEmailSearchField.TabIndex = 24;
            // 
            // btnEmailSearchOption
            // 
            this.btnEmailSearchOption.AutoEllipsis = true;
            this.btnEmailSearchOption.BackColor = System.Drawing.Color.Transparent;
            this.btnEmailSearchOption.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.ToggleOn;
            this.btnEmailSearchOption.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEmailSearchOption.FlatAppearance.BorderSize = 0;
            this.btnEmailSearchOption.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEmailSearchOption.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEmailSearchOption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmailSearchOption.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmailSearchOption.ForeColor = System.Drawing.Color.White;
            this.btnEmailSearchOption.Location = new System.Drawing.Point(235, 0);
            this.btnEmailSearchOption.Margin = new System.Windows.Forms.Padding(0);
            this.btnEmailSearchOption.Name = "btnEmailSearchOption";
            this.btnEmailSearchOption.Size = new System.Drawing.Size(72, 30);
            this.btnEmailSearchOption.TabIndex = 8;
            this.btnEmailSearchOption.TabStop = false;
            this.btnEmailSearchOption.Tag = "E";
            this.btnEmailSearchOption.Text = "Exact";
            this.btnEmailSearchOption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmailSearchOption.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnEmailSearchOption.UseVisualStyleBackColor = false;
            this.btnEmailSearchOption.Click += new System.EventHandler(this.btnSearchOption_Click);
            // 
            // txtEmail
            // 
            this.txtEmail.Cue = "Email Id";
            this.txtEmail.Location = new System.Drawing.Point(85, 5);
            this.txtEmail.MaxLength = 400;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(150, 21);
            this.txtEmail.TabIndex = 7;
            // 
            // lblContactEmail
            // 
            this.lblContactEmail.AutoSize = true;
            this.lblContactEmail.Location = new System.Drawing.Point(38, 8);
            this.lblContactEmail.Name = "lblContactEmail";
            this.lblContactEmail.Size = new System.Drawing.Size(44, 15);
            this.lblContactEmail.TabIndex = 16;
            this.lblContactEmail.Text = "Email: ";
            this.lblContactEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlCustPhSearchField
            // 
            this.pnlCustPhSearchField.Controls.Add(this.lblContactPh);
            this.pnlCustPhSearchField.Controls.Add(this.txtPhone);
            this.pnlCustPhSearchField.Controls.Add(this.btnPhoneSearchOption);
            this.pnlCustPhSearchField.Location = new System.Drawing.Point(793, 1);
            this.pnlCustPhSearchField.Margin = new System.Windows.Forms.Padding(1);
            this.pnlCustPhSearchField.Name = "pnlCustPhSearchField";
            this.pnlCustPhSearchField.Size = new System.Drawing.Size(322, 31);
            this.pnlCustPhSearchField.TabIndex = 24;
            // 
            // lblContactPh
            // 
            this.lblContactPh.Location = new System.Drawing.Point(4, 4);
            this.lblContactPh.Name = "lblContactPh";
            this.lblContactPh.Size = new System.Drawing.Size(114, 23);
            this.lblContactPh.TabIndex = 16;
            this.lblContactPh.Text = "Phone: ";
            this.lblContactPh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPhone
            // 
            this.txtPhone.Cue = "Phone Number";
            this.txtPhone.Location = new System.Drawing.Point(119, 5);
            this.txtPhone.MaxLength = 15;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(130, 21);
            this.txtPhone.TabIndex = 9;
            // 
            // btnPhoneSearchOption
            // 
            this.btnPhoneSearchOption.AutoEllipsis = true;
            this.btnPhoneSearchOption.BackColor = System.Drawing.Color.Transparent;
            this.btnPhoneSearchOption.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.ToggleOn;
            this.btnPhoneSearchOption.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPhoneSearchOption.FlatAppearance.BorderSize = 0;
            this.btnPhoneSearchOption.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPhoneSearchOption.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPhoneSearchOption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPhoneSearchOption.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhoneSearchOption.ForeColor = System.Drawing.Color.White;
            this.btnPhoneSearchOption.Location = new System.Drawing.Point(249, 0);
            this.btnPhoneSearchOption.Margin = new System.Windows.Forms.Padding(0);
            this.btnPhoneSearchOption.Name = "btnPhoneSearchOption";
            this.btnPhoneSearchOption.Size = new System.Drawing.Size(72, 30);
            this.btnPhoneSearchOption.TabIndex = 10;
            this.btnPhoneSearchOption.TabStop = false;
            this.btnPhoneSearchOption.Tag = "E";
            this.btnPhoneSearchOption.Text = "Exact";
            this.btnPhoneSearchOption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPhoneSearchOption.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnPhoneSearchOption.UseVisualStyleBackColor = false;
            this.btnPhoneSearchOption.Click += new System.EventHandler(this.btnSearchOption_Click);
            // 
            // pnlCustUniqIdSearchField
            // 
            this.pnlCustUniqIdSearchField.Controls.Add(this.txtUniqueIdentifier);
            this.pnlCustUniqIdSearchField.Controls.Add(this.lblUID);
            this.pnlCustUniqIdSearchField.Location = new System.Drawing.Point(1, 34);
            this.pnlCustUniqIdSearchField.Margin = new System.Windows.Forms.Padding(1);
            this.pnlCustUniqIdSearchField.Name = "pnlCustUniqIdSearchField";
            this.pnlCustUniqIdSearchField.Size = new System.Drawing.Size(310, 31);
            this.pnlCustUniqIdSearchField.TabIndex = 47;
            // 
            // txtUniqueIdentifier
            // 
            this.txtUniqueIdentifier.Location = new System.Drawing.Point(119, 5);
            this.txtUniqueIdentifier.Name = "txtUniqueIdentifier";
            this.txtUniqueIdentifier.Size = new System.Drawing.Size(150, 21);
            this.txtUniqueIdentifier.TabIndex = 11;
            // 
            // lblUID
            // 
            this.lblUID.AutoSize = true;
            this.lblUID.Location = new System.Drawing.Point(13, 8);
            this.lblUID.Name = "lblUID";
            this.lblUID.Size = new System.Drawing.Size(105, 15);
            this.lblUID.TabIndex = 18;
            this.lblUID.Text = "Unique Identifier: ";
            this.lblUID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlCustMembershipSearchField
            // 
            this.pnlCustMembershipSearchField.Controls.Add(this.cmbMembership);
            this.pnlCustMembershipSearchField.Controls.Add(this.lblMembership);
            this.pnlCustMembershipSearchField.Location = new System.Drawing.Point(313, 34);
            this.pnlCustMembershipSearchField.Margin = new System.Windows.Forms.Padding(1);
            this.pnlCustMembershipSearchField.Name = "pnlCustMembershipSearchField";
            this.pnlCustMembershipSearchField.Size = new System.Drawing.Size(310, 31);
            this.pnlCustMembershipSearchField.TabIndex = 48;
            // 
            // cmbMembership
            // 
            this.cmbMembership.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMembership.FormattingEnabled = true;
            this.cmbMembership.Location = new System.Drawing.Point(122, 4);
            this.cmbMembership.Name = "cmbMembership";
            this.cmbMembership.Size = new System.Drawing.Size(149, 23);
            this.cmbMembership.TabIndex = 12;
            // 
            // lblMembership
            // 
            this.lblMembership.AutoSize = true;
            this.lblMembership.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMembership.Location = new System.Drawing.Point(36, 8);
            this.lblMembership.Name = "lblMembership";
            this.lblMembership.Size = new System.Drawing.Size(84, 15);
            this.lblMembership.TabIndex = 21;
            this.lblMembership.Text = "Membership: ";
            this.lblMembership.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlCustSiteSearchField
            // 
            this.pnlCustSiteSearchField.Controls.Add(this.lblSite);
            this.pnlCustSiteSearchField.Controls.Add(this.cmbSite);
            this.pnlCustSiteSearchField.Location = new System.Drawing.Point(625, 34);
            this.pnlCustSiteSearchField.Margin = new System.Windows.Forms.Padding(1);
            this.pnlCustSiteSearchField.Name = "pnlCustSiteSearchField";
            this.pnlCustSiteSearchField.Size = new System.Drawing.Size(310, 31);
            this.pnlCustSiteSearchField.TabIndex = 49;
            // 
            // lblSite
            // 
            this.lblSite.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSite.Location = new System.Drawing.Point(2, 4);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(114, 23);
            this.lblSite.TabIndex = 12;
            this.lblSite.Text = "Site:";
            this.lblSite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbSite
            // 
            this.cmbSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSite.FormattingEnabled = true;
            this.cmbSite.Location = new System.Drawing.Point(119, 4);
            this.cmbSite.Name = "cmbSite";
            this.cmbSite.Size = new System.Drawing.Size(149, 23);
            this.cmbSite.TabIndex = 13;
            // 
            // dgvCustomerDTOList
            // 
            this.dgvCustomerDTOList.AllowUserToAddRows = false;
            this.dgvCustomerDTOList.AllowUserToDeleteRows = false;
            this.dgvCustomerDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCustomerDTOList.AutoGenerateColumns = false;
            this.dgvCustomerDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomerDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectDataGridViewButtonColumn,
            this.customerIdDataGridViewTextBoxColumn,
            this.titleDataGridViewComboBoxColumn,
            this.firstNameDataGridViewTextBoxColumn,
            this.middleNameDataGridViewTextBoxColumn,
            this.lastNameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.membershipIdDataGridViewComboBoxColumn,
            this.uniqueIdentifierDataGridViewTextBoxColumn,
            this.taxCodeDataGridViewTextBoxColumn,
            this.dateOfBirthDataGridViewTextBoxColumn,
            this.genderDataGridViewComboBoxColumn,
            this.anniversaryDataGridViewTextBoxColumn,
            this.teamUserDataGridViewCheckBoxColumn,
            this.rightHandedDataGridViewCheckBoxColumn,
            this.verifiedDataGridViewCheckBoxColumn,
            this.companyDataGridViewTextBoxColumn,
            this.designationDataGridViewTextBoxColumn,
            this.externalSystemReferenceDataGridViewTextBoxColumn,
            this.customDataSetDataGridViewButtonColumn,
            this.channelDataGridViewTextBoxColumn,
            this.notesDataGridViewTextBoxColumn,
            this.userNameDataGridViewTextBoxColumn,
            this.customerTypeDataGridViewComboBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn});
            this.dgvCustomerDTOList.DataSource = this.customerDTOListBS;
            this.dgvCustomerDTOList.Location = new System.Drawing.Point(10, 22);
            this.dgvCustomerDTOList.Name = "dgvCustomerDTOList";
            this.dgvCustomerDTOList.RowTemplate.Height = 30;
            this.dgvCustomerDTOList.Size = new System.Drawing.Size(1102, 175);
            this.dgvCustomerDTOList.TabIndex = 42;
            this.dgvCustomerDTOList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomerDTOList_CellContentClick);
            this.dgvCustomerDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvCustomerDTOList_DataError);
            // 
            // selectDataGridViewButtonColumn
            // 
            this.selectDataGridViewButtonColumn.HeaderText = "Select";
            this.selectDataGridViewButtonColumn.Name = "selectDataGridViewButtonColumn";
            this.selectDataGridViewButtonColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.selectDataGridViewButtonColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.selectDataGridViewButtonColumn.Width = 30;
            // 
            // customerIdDataGridViewTextBoxColumn
            // 
            this.customerIdDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.customerIdDataGridViewTextBoxColumn.HeaderText = "Customer Id";
            this.customerIdDataGridViewTextBoxColumn.Name = "customerIdDataGridViewTextBoxColumn";
            this.customerIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // titleDataGridViewComboBoxColumn
            // 
            this.titleDataGridViewComboBoxColumn.DataPropertyName = "Title";
            this.titleDataGridViewComboBoxColumn.HeaderText = "Title";
            this.titleDataGridViewComboBoxColumn.Name = "titleDataGridViewComboBoxColumn";
            // 
            // firstNameDataGridViewTextBoxColumn
            // 
            this.firstNameDataGridViewTextBoxColumn.DataPropertyName = "FirstName";
            this.firstNameDataGridViewTextBoxColumn.HeaderText = "First Name";
            this.firstNameDataGridViewTextBoxColumn.Name = "firstNameDataGridViewTextBoxColumn";
            // 
            // middleNameDataGridViewTextBoxColumn
            // 
            this.middleNameDataGridViewTextBoxColumn.DataPropertyName = "MiddleName";
            this.middleNameDataGridViewTextBoxColumn.HeaderText = "Middle Name";
            this.middleNameDataGridViewTextBoxColumn.Name = "middleNameDataGridViewTextBoxColumn";
            // 
            // lastNameDataGridViewTextBoxColumn
            // 
            this.lastNameDataGridViewTextBoxColumn.DataPropertyName = "LastName";
            this.lastNameDataGridViewTextBoxColumn.HeaderText = "Last Name";
            this.lastNameDataGridViewTextBoxColumn.Name = "lastNameDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "CardNumber";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Card Number";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // membershipIdDataGridViewComboBoxColumn
            // 
            this.membershipIdDataGridViewComboBoxColumn.DataPropertyName = "MembershipId";
            this.membershipIdDataGridViewComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.membershipIdDataGridViewComboBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.membershipIdDataGridViewComboBoxColumn.HeaderText = "Membership";
            this.membershipIdDataGridViewComboBoxColumn.Name = "membershipIdDataGridViewComboBoxColumn";
            this.membershipIdDataGridViewComboBoxColumn.ReadOnly = true;
            // 
            // uniqueIdentifierDataGridViewTextBoxColumn
            // 
            this.uniqueIdentifierDataGridViewTextBoxColumn.DataPropertyName = "UniqueIdentifier";
            this.uniqueIdentifierDataGridViewTextBoxColumn.HeaderText = "Unique ID";
            this.uniqueIdentifierDataGridViewTextBoxColumn.Name = "uniqueIdentifierDataGridViewTextBoxColumn";
            // 
            // taxCodeDataGridViewTextBoxColumn
            // 
            this.taxCodeDataGridViewTextBoxColumn.DataPropertyName = "TaxCode";
            this.taxCodeDataGridViewTextBoxColumn.HeaderText = "Tax Code";
            this.taxCodeDataGridViewTextBoxColumn.Name = "taxCodeDataGridViewTextBoxColumn";
            // 
            // dateOfBirthDataGridViewTextBoxColumn
            // 
            this.dateOfBirthDataGridViewTextBoxColumn.DataPropertyName = "DateOfBirth";
            this.dateOfBirthDataGridViewTextBoxColumn.HeaderText = "Date of Birth";
            this.dateOfBirthDataGridViewTextBoxColumn.Name = "dateOfBirthDataGridViewTextBoxColumn";
            // 
            // genderDataGridViewComboBoxColumn
            // 
            this.genderDataGridViewComboBoxColumn.DataPropertyName = "Gender";
            this.genderDataGridViewComboBoxColumn.HeaderText = "Gender";
            this.genderDataGridViewComboBoxColumn.Name = "genderDataGridViewComboBoxColumn";
            this.genderDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.genderDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // anniversaryDataGridViewTextBoxColumn
            // 
            this.anniversaryDataGridViewTextBoxColumn.DataPropertyName = "Anniversary";
            this.anniversaryDataGridViewTextBoxColumn.HeaderText = "Anniversary";
            this.anniversaryDataGridViewTextBoxColumn.Name = "anniversaryDataGridViewTextBoxColumn";
            // 
            // teamUserDataGridViewCheckBoxColumn
            // 
            this.teamUserDataGridViewCheckBoxColumn.DataPropertyName = "TeamUser";
            this.teamUserDataGridViewCheckBoxColumn.HeaderText = "Team User";
            this.teamUserDataGridViewCheckBoxColumn.Name = "teamUserDataGridViewCheckBoxColumn";
            // 
            // rightHandedDataGridViewCheckBoxColumn
            // 
            this.rightHandedDataGridViewCheckBoxColumn.DataPropertyName = "RightHanded";
            this.rightHandedDataGridViewCheckBoxColumn.HeaderText = "Right Handed";
            this.rightHandedDataGridViewCheckBoxColumn.Name = "rightHandedDataGridViewCheckBoxColumn";
            // 
            // verifiedDataGridViewCheckBoxColumn
            // 
            this.verifiedDataGridViewCheckBoxColumn.DataPropertyName = "Verified";
            this.verifiedDataGridViewCheckBoxColumn.HeaderText = "Verified";
            this.verifiedDataGridViewCheckBoxColumn.Name = "verifiedDataGridViewCheckBoxColumn";
            this.verifiedDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // companyDataGridViewTextBoxColumn
            // 
            this.companyDataGridViewTextBoxColumn.DataPropertyName = "Company";
            this.companyDataGridViewTextBoxColumn.HeaderText = "Company";
            this.companyDataGridViewTextBoxColumn.Name = "companyDataGridViewTextBoxColumn";
            // 
            // designationDataGridViewTextBoxColumn
            // 
            this.designationDataGridViewTextBoxColumn.DataPropertyName = "Designation";
            this.designationDataGridViewTextBoxColumn.HeaderText = "Designation";
            this.designationDataGridViewTextBoxColumn.Name = "designationDataGridViewTextBoxColumn";
            // 
            // externalSystemReferenceDataGridViewTextBoxColumn
            // 
            this.externalSystemReferenceDataGridViewTextBoxColumn.DataPropertyName = "ExternalSystemReference";
            this.externalSystemReferenceDataGridViewTextBoxColumn.HeaderText = "External System Reference";
            this.externalSystemReferenceDataGridViewTextBoxColumn.Name = "externalSystemReferenceDataGridViewTextBoxColumn";
            // 
            // customDataSetDataGridViewButtonColumn
            // 
            this.customDataSetDataGridViewButtonColumn.HeaderText = "Custom";
            this.customDataSetDataGridViewButtonColumn.Name = "customDataSetDataGridViewButtonColumn";
            this.customDataSetDataGridViewButtonColumn.Text = "...";
            // 
            // channelDataGridViewTextBoxColumn
            // 
            this.channelDataGridViewTextBoxColumn.DataPropertyName = "Channel";
            this.channelDataGridViewTextBoxColumn.HeaderText = "Channel";
            this.channelDataGridViewTextBoxColumn.Name = "channelDataGridViewTextBoxColumn";
            this.channelDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // notesDataGridViewTextBoxColumn
            // 
            this.notesDataGridViewTextBoxColumn.DataPropertyName = "Notes";
            this.notesDataGridViewTextBoxColumn.HeaderText = "Notes";
            this.notesDataGridViewTextBoxColumn.Name = "notesDataGridViewTextBoxColumn";
            // 
            // userNameDataGridViewTextBoxColumn
            // 
            this.userNameDataGridViewTextBoxColumn.DataPropertyName = "UserName";
            this.userNameDataGridViewTextBoxColumn.HeaderText = "Username";
            this.userNameDataGridViewTextBoxColumn.Name = "userNameDataGridViewTextBoxColumn";
            // 
            // customerTypeDataGridViewComboBoxColumn
            // 
            this.customerTypeDataGridViewComboBoxColumn.DataPropertyName = "CustomerType";
            this.customerTypeDataGridViewComboBoxColumn.HeaderText = "Type";
            this.customerTypeDataGridViewComboBoxColumn.Name = "customerTypeDataGridViewComboBoxColumn";
            this.customerTypeDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.customerTypeDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "Last Update Date";
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Last Updated User";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // customerDTOListBS
            // 
            this.customerDTOListBS.DataSource = typeof(Semnox.Parafait.Customer.CustomerDTO);
            this.customerDTOListBS.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.customerDTOListBS_AddingNew);
            this.customerDTOListBS.CurrentChanged += new System.EventHandler(this.customerDTOListBS_CurrentChanged);
            // 
            // grpContact
            // 
            this.grpContact.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpContact.Controls.Add(this.chbShowActiveContactEntries);
            this.grpContact.Controls.Add(this.dgvContactDTOList);
            this.grpContact.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.grpContact.Location = new System.Drawing.Point(734, 3);
            this.grpContact.Name = "grpContact";
            this.grpContact.Size = new System.Drawing.Size(388, 153);
            this.grpContact.TabIndex = 7;
            this.grpContact.TabStop = false;
            this.grpContact.Text = "Contacts";
            // 
            // chbShowActiveContactEntries
            // 
            this.chbShowActiveContactEntries.AutoSize = true;
            this.chbShowActiveContactEntries.Checked = true;
            this.chbShowActiveContactEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveContactEntries.Location = new System.Drawing.Point(11, 16);
            this.chbShowActiveContactEntries.Name = "chbShowActiveContactEntries";
            this.chbShowActiveContactEntries.Size = new System.Drawing.Size(124, 19);
            this.chbShowActiveContactEntries.TabIndex = 5;
            this.chbShowActiveContactEntries.Text = "Show Active Only";
            this.chbShowActiveContactEntries.UseVisualStyleBackColor = true;
            this.chbShowActiveContactEntries.CheckedChanged += new System.EventHandler(this.chbShowActiveContactEntries_CheckedChanged);
            // 
            // dgvContactDTOList
            // 
            this.dgvContactDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvContactDTOList.AutoGenerateColumns = false;
            this.dgvContactDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContactDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.contactTypeIdDataGridViewComboBoxColumn,
            this.attribute1DataGridViewTextBoxColumn,
            this.attribute2DataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn1});
            this.dgvContactDTOList.DataSource = this.contactDTOListBS;
            this.dgvContactDTOList.Location = new System.Drawing.Point(7, 39);
            this.dgvContactDTOList.Name = "dgvContactDTOList";
            this.dgvContactDTOList.RowTemplate.Height = 30;
            this.dgvContactDTOList.Size = new System.Drawing.Size(374, 107);
            this.dgvContactDTOList.TabIndex = 0;
            this.dgvContactDTOList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContactDTOList_CellEnter);
            this.dgvContactDTOList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvContactDTOList_CellFormatting);
            this.dgvContactDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvContactDTOList_DataError);
            this.dgvContactDTOList.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvContactDTOList_EditingControlShowing);
            // 
            // contactTypeIdDataGridViewComboBoxColumn
            // 
            this.contactTypeIdDataGridViewComboBoxColumn.DataPropertyName = "ContactType";
            this.contactTypeIdDataGridViewComboBoxColumn.HeaderText = "Contact Type";
            this.contactTypeIdDataGridViewComboBoxColumn.Name = "contactTypeIdDataGridViewComboBoxColumn";
            // 
            // attribute1DataGridViewTextBoxColumn
            // 
            this.attribute1DataGridViewTextBoxColumn.DataPropertyName = "Attribute1";
            this.attribute1DataGridViewTextBoxColumn.HeaderText = "Attribute1";
            this.attribute1DataGridViewTextBoxColumn.Name = "attribute1DataGridViewTextBoxColumn";
            // 
            // attribute2DataGridViewTextBoxColumn
            // 
            this.attribute2DataGridViewTextBoxColumn.DataPropertyName = "Attribute2";
            this.attribute2DataGridViewTextBoxColumn.HeaderText = "Attribute2";
            this.attribute2DataGridViewTextBoxColumn.Name = "attribute2DataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn1
            // 
            this.isActiveDataGridViewCheckBoxColumn1.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn1.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn1.Name = "isActiveDataGridViewCheckBoxColumn1";
            // 
            // contactDTOListBS
            // 
            this.contactDTOListBS.DataSource = typeof(Semnox.Parafait.Customer.ContactDTO);
            // 
            // grpCustomer
            // 
            this.grpCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCustomer.Controls.Add(this.dgvCustomerDTOList);
            this.grpCustomer.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.grpCustomer.Location = new System.Drawing.Point(3, 3);
            this.grpCustomer.Name = "grpCustomer";
            this.grpCustomer.Size = new System.Drawing.Size(1119, 204);
            this.grpCustomer.TabIndex = 35;
            this.grpCustomer.TabStop = false;
            this.grpCustomer.Text = "Customer";
            // 
            // lblPage
            // 
            this.lblPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPage.AutoSize = true;
            this.lblPage.Location = new System.Drawing.Point(639, 138);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(84, 15);
            this.lblPage.TabIndex = 36;
            this.lblPage.Text = "Page 1 of 100";
            this.lblPage.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnFirst
            // 
            this.btnFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFirst.Location = new System.Drawing.Point(765, 128);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(87, 30);
            this.btnFirst.TabIndex = 37;
            this.btnFirst.Text = "First";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevious.Location = new System.Drawing.Point(859, 128);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(87, 30);
            this.btnPrevious.TabIndex = 38;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(951, 128);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(87, 30);
            this.btnNext.TabIndex = 39;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnLast
            // 
            this.btnLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLast.Location = new System.Drawing.Point(1043, 128);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(87, 30);
            this.btnLast.TabIndex = 40;
            this.btnLast.Text = "Last";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(16, 534);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(133, 15);
            this.lblStatus.TabIndex = 41;
            this.lblStatus.Text = "Loading.. Please wait..";
            // 
            // tlpMain
            // 
            this.tlpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.grpCustomer, 0, 0);
            this.tlpMain.Controls.Add(this.tlpAddressAndContact, 0, 1);
            this.tlpMain.Location = new System.Drawing.Point(12, 159);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tlpMain.Size = new System.Drawing.Size(1125, 369);
            this.tlpMain.TabIndex = 45;
            // 
            // tlpAddressAndContact
            // 
            this.tlpAddressAndContact.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpAddressAndContact.AutoSize = true;
            this.tlpAddressAndContact.ColumnCount = 2;
            this.tlpAddressAndContact.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tlpAddressAndContact.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpAddressAndContact.Controls.Add(this.grpAddress, 0, 0);
            this.tlpAddressAndContact.Controls.Add(this.grpContact, 1, 0);
            this.tlpAddressAndContact.Location = new System.Drawing.Point(0, 210);
            this.tlpAddressAndContact.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAddressAndContact.Name = "tlpAddressAndContact";
            this.tlpAddressAndContact.RowCount = 1;
            this.tlpAddressAndContact.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAddressAndContact.Size = new System.Drawing.Size(1125, 159);
            this.tlpAddressAndContact.TabIndex = 46;
            // 
            // grpAddress
            // 
            this.grpAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAddress.Controls.Add(this.chbShowActiveAddressEntries);
            this.grpAddress.Controls.Add(this.dgvAddressDTOList);
            this.grpAddress.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.grpAddress.Location = new System.Drawing.Point(3, 3);
            this.grpAddress.Name = "grpAddress";
            this.grpAddress.Size = new System.Drawing.Size(725, 153);
            this.grpAddress.TabIndex = 8;
            this.grpAddress.TabStop = false;
            this.grpAddress.Text = "Addresses";
            // 
            // chbShowActiveAddressEntries
            // 
            this.chbShowActiveAddressEntries.AutoSize = true;
            this.chbShowActiveAddressEntries.Checked = true;
            this.chbShowActiveAddressEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveAddressEntries.Location = new System.Drawing.Point(11, 16);
            this.chbShowActiveAddressEntries.Name = "chbShowActiveAddressEntries";
            this.chbShowActiveAddressEntries.Size = new System.Drawing.Size(124, 19);
            this.chbShowActiveAddressEntries.TabIndex = 4;
            this.chbShowActiveAddressEntries.Text = "Show Active Only";
            this.chbShowActiveAddressEntries.UseVisualStyleBackColor = true;
            this.chbShowActiveAddressEntries.CheckedChanged += new System.EventHandler(this.chbShowActiveAddressEntries_CheckedChanged);
            // 
            // dgvAddressDTOList
            // 
            this.dgvAddressDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAddressDTOList.AutoGenerateColumns = false;
            this.dgvAddressDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAddressDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.addressTypeIdDataGridViewComboBoxColumn,
            this.line1DataGridViewTextBoxColumn,
            this.line2DataGridViewTextBoxColumn,
            this.line3DataGridViewTextBoxColumn,
            this.cityDataGridViewTextBoxColumn,
            this.stateIdDataGridViewComboBoxColumn,
            this.countryIdDataGridViewComboBoxColumn,
            this.postalCodeDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvAddressDTOList.DataSource = this.addressDTOListBS;
            this.dgvAddressDTOList.Location = new System.Drawing.Point(10, 39);
            this.dgvAddressDTOList.Name = "dgvAddressDTOList";
            this.dgvAddressDTOList.RowTemplate.Height = 30;
            this.dgvAddressDTOList.Size = new System.Drawing.Size(707, 107);
            this.dgvAddressDTOList.TabIndex = 0;
            this.dgvAddressDTOList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAddressDTOList_CellValueChanged);
            this.dgvAddressDTOList.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvAddressDTOList_CurrentCellDirtyStateChanged);
            this.dgvAddressDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAddressDTOList_DataError);
            this.dgvAddressDTOList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAddressDTOList_RowEnter);
            // 
            // addressTypeIdDataGridViewComboBoxColumn
            // 
            this.addressTypeIdDataGridViewComboBoxColumn.DataPropertyName = "AddressType";
            this.addressTypeIdDataGridViewComboBoxColumn.HeaderText = "Address Type";
            this.addressTypeIdDataGridViewComboBoxColumn.Name = "addressTypeIdDataGridViewComboBoxColumn";
            this.addressTypeIdDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.addressTypeIdDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // line1DataGridViewTextBoxColumn
            // 
            this.line1DataGridViewTextBoxColumn.DataPropertyName = "Line1";
            this.line1DataGridViewTextBoxColumn.HeaderText = "Line1";
            this.line1DataGridViewTextBoxColumn.Name = "line1DataGridViewTextBoxColumn";
            // 
            // line2DataGridViewTextBoxColumn
            // 
            this.line2DataGridViewTextBoxColumn.DataPropertyName = "Line2";
            this.line2DataGridViewTextBoxColumn.HeaderText = "Line2";
            this.line2DataGridViewTextBoxColumn.Name = "line2DataGridViewTextBoxColumn";
            // 
            // line3DataGridViewTextBoxColumn
            // 
            this.line3DataGridViewTextBoxColumn.DataPropertyName = "Line3";
            this.line3DataGridViewTextBoxColumn.HeaderText = "Line3";
            this.line3DataGridViewTextBoxColumn.Name = "line3DataGridViewTextBoxColumn";
            // 
            // cityDataGridViewTextBoxColumn
            // 
            this.cityDataGridViewTextBoxColumn.DataPropertyName = "City";
            this.cityDataGridViewTextBoxColumn.HeaderText = "City";
            this.cityDataGridViewTextBoxColumn.Name = "cityDataGridViewTextBoxColumn";
            this.cityDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // stateIdDataGridViewComboBoxColumn
            // 
            this.stateIdDataGridViewComboBoxColumn.DataPropertyName = "StateId";
            this.stateIdDataGridViewComboBoxColumn.HeaderText = "State";
            this.stateIdDataGridViewComboBoxColumn.Name = "stateIdDataGridViewComboBoxColumn";
            this.stateIdDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.stateIdDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // countryIdDataGridViewComboBoxColumn
            // 
            this.countryIdDataGridViewComboBoxColumn.DataPropertyName = "CountryId";
            this.countryIdDataGridViewComboBoxColumn.HeaderText = "Country";
            this.countryIdDataGridViewComboBoxColumn.Name = "countryIdDataGridViewComboBoxColumn";
            this.countryIdDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.countryIdDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // postalCodeDataGridViewTextBoxColumn
            // 
            this.postalCodeDataGridViewTextBoxColumn.DataPropertyName = "PostalCode";
            this.postalCodeDataGridViewTextBoxColumn.HeaderText = "Postal Code";
            this.postalCodeDataGridViewTextBoxColumn.Name = "postalCodeDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // addressDTOListBS
            // 
            this.addressDTOListBS.DataSource = typeof(Semnox.Parafait.Customer.AddressDTO);
            this.addressDTOListBS.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.addressDTOListBS_AddingNew);
            this.addressDTOListBS.DataSourceChanged += new System.EventHandler(this.addressDTOListBS_DataSourceChanged);
            // 
            // btnRelationship
            // 
            this.btnRelationship.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRelationship.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRelationship.Location = new System.Drawing.Point(705, 3);
            this.btnRelationship.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.btnRelationship.Name = "btnRelationship";
            this.btnRelationship.Size = new System.Drawing.Size(169, 45);
            this.btnRelationship.TabIndex = 34;
            this.btnRelationship.Text = "Customer Relationship";
            this.btnRelationship.UseVisualStyleBackColor = true;
            this.btnRelationship.Click += new System.EventHandler(this.btnRelationship_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnImport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnImport.Location = new System.Drawing.Point(595, 3);
            this.btnImport.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(87, 45);
            this.btnImport.TabIndex = 42;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(485, 3);
            this.btnExport.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(87, 45);
            this.btnExport.TabIndex = 43;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(223, 3);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 45);
            this.btnClose.TabIndex = 32;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(113, 4);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 20, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(87, 45);
            this.btnRefresh.TabIndex = 30;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.AutoSize = true;
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 45);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Image = global::Semnox.Parafait.Customer.Properties.Resources.keyboard;
            this.btnShowKeyPad.Location = new System.Drawing.Point(1100, 592);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowKeyPad.TabIndex = 45;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            // 
            // flpButtonControls
            // 
            this.flpButtonControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpButtonControls.AutoSize = true;
            this.flpButtonControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpButtonControls.Controls.Add(this.btnSave);
            this.flpButtonControls.Controls.Add(this.btnRefresh);
            this.flpButtonControls.Controls.Add(this.btnClose);
            this.flpButtonControls.Controls.Add(this.btnAssociatedCards);
            this.flpButtonControls.Controls.Add(this.btnExport);
            this.flpButtonControls.Controls.Add(this.btnImport);
            this.flpButtonControls.Controls.Add(this.btnRelationship);
            this.flpButtonControls.Controls.Add(this.btnUpdateMembership);
            this.flpButtonControls.Location = new System.Drawing.Point(15, 554);
            this.flpButtonControls.Margin = new System.Windows.Forms.Padding(0);
            this.flpButtonControls.Name = "flpButtonControls";
            this.flpButtonControls.Size = new System.Drawing.Size(1050, 52);
            this.flpButtonControls.TabIndex = 46;
            // 
            // btnAssociatedCards
            // 
            this.btnAssociatedCards.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAssociatedCards.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAssociatedCards.Location = new System.Drawing.Point(333, 3);
            this.btnAssociatedCards.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.btnAssociatedCards.Name = "btnAssociatedCards";
            this.btnAssociatedCards.Size = new System.Drawing.Size(129, 45);
            this.btnAssociatedCards.TabIndex = 45;
            this.btnAssociatedCards.Text = "Associated Cards";
            this.btnAssociatedCards.UseVisualStyleBackColor = true;
            this.btnAssociatedCards.Click += new System.EventHandler(this.btnAssociatedCards_Click);
            // 
            // btnUpdateMembership
            // 
            this.btnUpdateMembership.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUpdateMembership.Location = new System.Drawing.Point(897, 3);
            this.btnUpdateMembership.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.btnUpdateMembership.Name = "btnUpdateMembership";
            this.btnUpdateMembership.Size = new System.Drawing.Size(133, 45);
            this.btnUpdateMembership.TabIndex = 44;
            this.btnUpdateMembership.Text = "Update Membership";
            this.btnUpdateMembership.UseVisualStyleBackColor = true;
            this.btnUpdateMembership.Click += new System.EventHandler(this.btnUpdateMembership_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CardId";
            this.dataGridViewTextBoxColumn1.HeaderText = "CardId";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Card_number";
            this.dataGridViewTextBoxColumn2.HeaderText = "Card Number";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Issue_date";
            this.dataGridViewTextBoxColumn3.HeaderText = "Issue Date";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Credits";
            this.dataGridViewTextBoxColumn4.HeaderText = "Credits";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Courtesy";
            this.dataGridViewTextBoxColumn5.HeaderText = "Courtesy";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Bonus";
            this.dataGridViewTextBoxColumn6.HeaderText = "Bonus";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "Ticket_count";
            this.dataGridViewTextBoxColumn7.HeaderText = "Ticket Count";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "Credits_played";
            this.dataGridViewTextBoxColumn8.HeaderText = "Credits Played";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Face_value";
            this.dataGridViewTextBoxColumn9.HeaderText = "Face Value";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "Refund_amount";
            this.dataGridViewTextBoxColumn10.HeaderText = "Refund Amount";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "Refund_date";
            this.dataGridViewTextBoxColumn11.HeaderText = "Refund Date";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "Notes";
            this.dataGridViewTextBoxColumn12.HeaderText = "Notes";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "Last_update_time";
            this.dataGridViewTextBoxColumn13.HeaderText = "Last Update Time";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "Time";
            this.dataGridViewTextBoxColumn14.HeaderText = "Time";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.Visible = false;
            // 
            // cardCoreDTOListBS
            // 
            this.cardCoreDTOListBS.DataSource = typeof(Semnox.Parafait.CardCore.CardCoreDTO);
            // 
            // CustomerListUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1152, 636);
            this.Controls.Add(this.flpButtonControls);
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.grpFilter);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.lblPage);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnShowKeyPad);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Name = "CustomerListUI";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CustomerListUI_FormClosing);
            this.Load += new System.EventHandler(this.CustomerListUI_Load);
            this.Shown += new System.EventHandler(this.CustomerListUI_Shown);
            this.grpFilter.ResumeLayout(false);
            this.flpSearchFields.ResumeLayout(false);
            this.pnlCustNameSearchFields.ResumeLayout(false);
            this.pnlCustNameSearchFields.PerformLayout();
            this.pnlCustEmailSearchField.ResumeLayout(false);
            this.pnlCustEmailSearchField.PerformLayout();
            this.pnlCustPhSearchField.ResumeLayout(false);
            this.pnlCustPhSearchField.PerformLayout();
            this.pnlCustUniqIdSearchField.ResumeLayout(false);
            this.pnlCustUniqIdSearchField.PerformLayout();
            this.pnlCustMembershipSearchField.ResumeLayout(false);
            this.pnlCustMembershipSearchField.PerformLayout();
            this.pnlCustSiteSearchField.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerDTOListBS)).EndInit();
            this.grpContact.ResumeLayout(false);
            this.grpContact.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContactDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contactDTOListBS)).EndInit();
            this.grpCustomer.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tlpAddressAndContact.ResumeLayout(false);
            this.grpAddress.ResumeLayout(false);
            this.grpAddress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddressDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.addressDTOListBS)).EndInit();
            this.flpButtonControls.ResumeLayout(false);
            this.flpButtonControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cardCoreDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFilter;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnSearch;
        /// <summary>
        /// 
        /// </summary>
        protected Semnox.Parafait.User.CueTextBox txtFirstName;
        private System.Windows.Forms.Label lblCustomerName;
        /// <summary>
        /// 
        /// </summary>
        protected Semnox.Parafait.User.CueTextBox txtLastName;
        /// <summary>
        /// 
        /// </summary>
        protected Semnox.Parafait.User.CueTextBox txtMiddleName;
        private System.Windows.Forms.Label lblSite;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.ComboBox cmbSite;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnAdvancedSearch;
        private System.Windows.Forms.GroupBox grpContact;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.DataGridView dgvContactDTOList;
        private System.Windows.Forms.BindingSource addressDTOListBS;
        private System.Windows.Forms.BindingSource contactDTOListBS;
        private System.Windows.Forms.GroupBox grpCustomer;
        private System.Windows.Forms.BindingSource cardCoreDTOListBS;
        private System.Windows.Forms.Label lblPage;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnFirst;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnPrevious;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnNext;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblMembership;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.ComboBox cmbMembership;
        private System.Windows.Forms.Label lblUID;
        /// <summary>
        /// 
        /// </summary>
        protected Semnox.Parafait.User.CueTextBox txtPhone;
        private System.Windows.Forms.Label lblContactPh;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.TextBox txtUniqueIdentifier;
        private System.Windows.Forms.TableLayoutPanel tlpAddressAndContact;
        private System.Windows.Forms.GroupBox grpAddress;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.DataGridView dgvAddressDTOList;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.TableLayoutPanel tlpMain;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.DataGridView dgvCustomerDTOList;

        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.BindingSource customerDTOListBS;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnRelationship;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnImport;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnExport;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnClose;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnRefresh;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewComboBoxColumn contactTypeIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attribute1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attribute2DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn1;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.FlowLayoutPanel flpButtonControls;
        private System.Windows.Forms.DataGridViewComboBoxColumn addressTypeIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn line1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn line2DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn line3DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn stateIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn countryIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn postalCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.CheckBox chbShowActiveAddressEntries;
        private System.Windows.Forms.CheckBox chbShowActiveContactEntries;
        /// <summary>
        /// 
        /// </summary>
        protected System.Windows.Forms.Button btnUpdateMembership;
        protected System.Windows.Forms.Button btnAssociatedCards;
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
        protected System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridViewButtonColumn selectDataGridViewButtonColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn titleDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn middleNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn membershipIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uniqueIdentifierDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateOfBirthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn genderDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn anniversaryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn teamUserDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn rightHandedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn verifiedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn designationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn externalSystemReferenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn customDataSetDataGridViewButtonColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn channelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn customerTypeDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        protected System.Windows.Forms.Button btnPhoneSearchOption;
        private System.Windows.Forms.Panel pnlCustPhSearchField;
        private System.Windows.Forms.Panel pnlCustNameSearchFields;
        private System.Windows.Forms.Panel pnlCustEmailSearchField;
        protected System.Windows.Forms.Button btnEmailSearchOption;
        protected User.CueTextBox txtEmail;
        private System.Windows.Forms.Label lblContactEmail;
        private System.Windows.Forms.Panel pnlCustUniqIdSearchField;
        private System.Windows.Forms.Panel pnlCustMembershipSearchField;
        private System.Windows.Forms.FlowLayoutPanel flpSearchFields;
        private System.Windows.Forms.Panel pnlCustSiteSearchField;
        private System.Windows.Forms.Button btnShowKeyPad;
    }
}