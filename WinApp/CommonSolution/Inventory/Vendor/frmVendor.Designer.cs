using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory
{
    partial class frmVendor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVendor));
            this.vendorFillByToolStrip = new System.Windows.Forms.ToolStrip();
            this.vendorNameToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.vendorNameToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.vendorFillByToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtAddressline1 = new System.Windows.Forms.TextBox();
            this.txtaddressline2 = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.txtZip = new System.Windows.Forms.TextBox();
            this.rtbAddressremarks = new System.Windows.Forms.RichTextBox();
            this.dgvVendor = new System.Windows.Forms.DataGridView();
            this.VendorId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vendorname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vendorisactive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxRegistrationNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.website = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContactName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.City = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Country = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PostalCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddressRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastModUserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VendorMarkupPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountryId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StateId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vendorDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gb_vendor = new System.Windows.Forms.GroupBox();
            this.gb_address = new System.Windows.Forms.GroupBox();
            this.ddlCountry = new System.Windows.Forms.ComboBox();
            this.countryDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ddlState = new System.Windows.Forms.ComboBox();
            this.stateDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblErrorEmail = new System.Windows.Forms.Label();
            this.txtContactname = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtFax = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtWebsite = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.rtbRemarks = new System.Windows.Forms.RichTextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.ddlIsactive = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.lblVendorid = new System.Windows.Forms.Label();
            this.lblErrorWebsite = new System.Windows.Forms.Label();
            this.btnDuplicate = new System.Windows.Forms.Button();
            this.txtTaxRegistrationNo = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtVendorCode = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.ddlPurchaseTax = new System.Windows.Forms.ComboBox();
            this.purchaseTaxDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtVendorMarkup = new System.Windows.Forms.TextBox();
            this.lblVendorMarkup = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lnkPublishToSite = new System.Windows.Forms.LinkLabel();
            this.vendorFillByToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorDTOBindingSource)).BeginInit();
            this.gb_vendor.SuspendLayout();
            this.gb_address.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.countryDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stateDTOBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseTaxDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // vendorFillByToolStrip
            // 
            this.vendorFillByToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vendorNameToolStripLabel,
            this.vendorNameToolStripTextBox,
            this.vendorFillByToolStripButton});
            this.vendorFillByToolStrip.Location = new System.Drawing.Point(0, 0);
            this.vendorFillByToolStrip.Name = "vendorFillByToolStrip";
            this.vendorFillByToolStrip.Size = new System.Drawing.Size(896, 25);
            this.vendorFillByToolStrip.TabIndex = 1;
            this.vendorFillByToolStrip.Text = "vendorFillByToolStrip";
            // 
            // vendorNameToolStripLabel
            // 
            this.vendorNameToolStripLabel.Name = "vendorNameToolStripLabel";
            this.vendorNameToolStripLabel.Size = new System.Drawing.Size(79, 22);
            this.vendorNameToolStripLabel.Text = "VendorName:";
            // 
            // vendorNameToolStripTextBox
            // 
            this.vendorNameToolStripTextBox.Name = "vendorNameToolStripTextBox";
            this.vendorNameToolStripTextBox.Size = new System.Drawing.Size(100, 25);
            this.vendorNameToolStripTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.vendorNameToolStripTextBox_KeyPress);
            // 
            // vendorFillByToolStripButton
            // 
            this.vendorFillByToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.vendorFillByToolStripButton.Name = "vendorFillByToolStripButton";
            this.vendorFillByToolStripButton.Size = new System.Drawing.Size(46, 22);
            this.vendorFillByToolStripButton.Text = "Search";
            this.vendorFillByToolStripButton.Click += new System.EventHandler(this.vendorFillByToolStripButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(263, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name:*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(566, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Address Line 1:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(566, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Address Line 2:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(566, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "City:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(566, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "State:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(566, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Zip Code:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(566, 209);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Country:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(566, 232);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Address Remarks:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(322, 88);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(201, 20);
            this.txtName.TabIndex = 4;
            // 
            // txtAddressline1
            // 
            this.txtAddressline1.Location = new System.Drawing.Point(656, 84);
            this.txtAddressline1.Name = "txtAddressline1";
            this.txtAddressline1.Size = new System.Drawing.Size(209, 20);
            this.txtAddressline1.TabIndex = 20;
            // 
            // txtaddressline2
            // 
            this.txtaddressline2.Location = new System.Drawing.Point(656, 108);
            this.txtaddressline2.Name = "txtaddressline2";
            this.txtaddressline2.Size = new System.Drawing.Size(209, 20);
            this.txtaddressline2.TabIndex = 22;
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(656, 133);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(145, 20);
            this.txtCity.TabIndex = 24;
            // 
            // txtZip
            // 
            this.txtZip.Location = new System.Drawing.Point(656, 182);
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(145, 20);
            this.txtZip.TabIndex = 28;
            this.txtZip.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtZip_KeyPress);
            // 
            // rtbAddressremarks
            // 
            this.rtbAddressremarks.Location = new System.Drawing.Point(569, 249);
            this.rtbAddressremarks.Name = "rtbAddressremarks";
            this.rtbAddressremarks.Size = new System.Drawing.Size(296, 63);
            this.rtbAddressremarks.TabIndex = 32;
            this.rtbAddressremarks.Text = "";
            // 
            // dgvVendor
            // 
            this.dgvVendor.AllowUserToAddRows = false;
            this.dgvVendor.AllowUserToDeleteRows = false;
            this.dgvVendor.AutoGenerateColumns = false;
            this.dgvVendor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvVendor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVendor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VendorId,
            this.vendorname,
            this.vendorisactive,
            this.Remarks,
            this.TaxRegistrationNumber,
            this.website,
            this.VendorCode,
            this.ContactName,
            this.Phone,
            this.Fax,
            this.Email,
            this.Address1,
            this.Address2,
            this.City,
            this.State,
            this.Country,
            this.PostalCode,
            this.AddressRemarks,
            this.VendorRemarks,
            this.LastModUserId,
            this.VendorMarkupPercent,
            this.CountryId,
            this.StateId});
            this.dgvVendor.DataSource = this.vendorDTOBindingSource;
            this.dgvVendor.GridColor = System.Drawing.Color.DarkOliveGreen;
            this.dgvVendor.Location = new System.Drawing.Point(12, 19);
            this.dgvVendor.Name = "dgvVendor";
            this.dgvVendor.ReadOnly = true;
            this.dgvVendor.RowHeadersVisible = false;
            this.dgvVendor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVendor.Size = new System.Drawing.Size(202, 410);
            this.dgvVendor.TabIndex = 2;
            // 
            // VendorId
            // 
            this.VendorId.DataPropertyName = "VendorId";
            this.VendorId.HeaderText = "VendorId";
            this.VendorId.Name = "VendorId";
            this.VendorId.ReadOnly = true;
            this.VendorId.Visible = false;
            // 
            // vendorname
            // 
            this.vendorname.DataPropertyName = "Name";
            this.vendorname.HeaderText = "Name";
            this.vendorname.Name = "vendorname";
            this.vendorname.ReadOnly = true;
            // 
            // vendorisactive
            // 
            this.vendorisactive.DataPropertyName = "IsActive";
            this.vendorisactive.HeaderText = "IsActive";
            this.vendorisactive.Name = "vendorisactive";
            this.vendorisactive.ReadOnly = true;
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            this.Remarks.ReadOnly = true;
            this.Remarks.Visible = false;
            // 
            // TaxRegistrationNumber
            // 
            this.TaxRegistrationNumber.DataPropertyName = "TaxRegistrationNumber";
            this.TaxRegistrationNumber.HeaderText = "TaxRegistrationNumber";
            this.TaxRegistrationNumber.Name = "TaxRegistrationNumber";
            this.TaxRegistrationNumber.ReadOnly = true;
            this.TaxRegistrationNumber.Visible = false;
            // 
            // website
            // 
            this.website.DataPropertyName = "Website";
            this.website.HeaderText = "Website";
            this.website.Name = "website";
            this.website.ReadOnly = true;
            this.website.Visible = false;
            // 
            // VendorCode
            // 
            this.VendorCode.DataPropertyName = "VendorCode";
            this.VendorCode.HeaderText = "VendorCode";
            this.VendorCode.Name = "VendorCode";
            this.VendorCode.ReadOnly = true;
            this.VendorCode.Visible = false;
            // 
            // ContactName
            // 
            this.ContactName.DataPropertyName = "ContactName";
            this.ContactName.HeaderText = "Contact Name";
            this.ContactName.Name = "ContactName";
            this.ContactName.ReadOnly = true;
            this.ContactName.Visible = false;
            // 
            // Phone
            // 
            this.Phone.DataPropertyName = "Phone";
            this.Phone.HeaderText = "Phone";
            this.Phone.Name = "Phone";
            this.Phone.ReadOnly = true;
            this.Phone.Visible = false;
            // 
            // Fax
            // 
            this.Fax.DataPropertyName = "Fax";
            this.Fax.HeaderText = "Fax";
            this.Fax.Name = "Fax";
            this.Fax.ReadOnly = true;
            this.Fax.Visible = false;
            // 
            // Email
            // 
            this.Email.DataPropertyName = "Email";
            this.Email.HeaderText = "Email";
            this.Email.Name = "Email";
            this.Email.ReadOnly = true;
            this.Email.Visible = false;
            // 
            // Address1
            // 
            this.Address1.DataPropertyName = "Address1";
            this.Address1.HeaderText = "Address1";
            this.Address1.Name = "Address1";
            this.Address1.ReadOnly = true;
            this.Address1.Visible = false;
            // 
            // Address2
            // 
            this.Address2.DataPropertyName = "Address2";
            this.Address2.HeaderText = "Address2";
            this.Address2.Name = "Address2";
            this.Address2.ReadOnly = true;
            this.Address2.Visible = false;
            // 
            // City
            // 
            this.City.DataPropertyName = "City";
            this.City.HeaderText = "City";
            this.City.Name = "City";
            this.City.ReadOnly = true;
            this.City.Visible = false;
            // 
            // State
            // 
            this.State.DataPropertyName = "State";
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            this.State.Visible = false;
            // 
            // Country
            // 
            this.Country.DataPropertyName = "Country";
            this.Country.HeaderText = "Country";
            this.Country.Name = "Country";
            this.Country.ReadOnly = true;
            this.Country.Visible = false;
            // 
            // PostalCode
            // 
            this.PostalCode.DataPropertyName = "PostalCode";
            this.PostalCode.HeaderText = "PostalCode";
            this.PostalCode.Name = "PostalCode";
            this.PostalCode.ReadOnly = true;
            this.PostalCode.Visible = false;
            // 
            // AddressRemarks
            // 
            this.AddressRemarks.DataPropertyName = "AddressRemarks";
            this.AddressRemarks.HeaderText = "AddressRemarks";
            this.AddressRemarks.Name = "AddressRemarks";
            this.AddressRemarks.ReadOnly = true;
            this.AddressRemarks.Visible = false;
            // 
            // VendorRemarks
            // 
            this.VendorRemarks.DataPropertyName = "Remarks";
            this.VendorRemarks.HeaderText = "Remarks";
            this.VendorRemarks.Name = "VendorRemarks";
            this.VendorRemarks.ReadOnly = true;
            this.VendorRemarks.Visible = false;
            // 
            // LastModUserId
            // 
            this.LastModUserId.DataPropertyName = "LastModUserId";
            this.LastModUserId.HeaderText = "LastModUserId";
            this.LastModUserId.Name = "LastModUserId";
            this.LastModUserId.ReadOnly = true;
            this.LastModUserId.Visible = false;
            // 
            // VendorMarkupPercent
            // 
            this.VendorMarkupPercent.DataPropertyName = "VendorMarkupPercent";
            this.VendorMarkupPercent.HeaderText = "VendorMarkupPercent";
            this.VendorMarkupPercent.Name = "VendorMarkupPercent";
            this.VendorMarkupPercent.ReadOnly = true;
            this.VendorMarkupPercent.Visible = false;
            // 
            // CountryId
            // 
            this.CountryId.DataPropertyName = "CountryId";
            this.CountryId.HeaderText = "CountryId";
            this.CountryId.Name = "CountryId";
            this.CountryId.ReadOnly = true;
            this.CountryId.Visible = false;
            // 
            // StateId
            // 
            this.StateId.DataPropertyName = "StateId";
            this.StateId.HeaderText = "StateId";
            this.StateId.Name = "StateId";
            this.StateId.ReadOnly = true;
            this.StateId.Visible = false;
            // 
            // vendorDTOBindingSource
            // 
            this.vendorDTOBindingSource.DataSource = typeof(Semnox.Parafait.Vendor.VendorDTO);
            this.vendorDTOBindingSource.CurrentChanged += new System.EventHandler(this.vendorDTOBindingSource_CurrentChanged);
            // 
            // gb_vendor
            // 
            this.gb_vendor.BackColor = System.Drawing.Color.Transparent;
            this.gb_vendor.Controls.Add(this.dgvVendor);
            this.gb_vendor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gb_vendor.Location = new System.Drawing.Point(12, 40);
            this.gb_vendor.Name = "gb_vendor";
            this.gb_vendor.Size = new System.Drawing.Size(226, 435);
            this.gb_vendor.TabIndex = 1;
            this.gb_vendor.TabStop = false;
            this.gb_vendor.Text = "View Vendors";
            // 
            // gb_address
            // 
            this.gb_address.BackColor = System.Drawing.Color.Transparent;
            this.gb_address.Controls.Add(this.ddlCountry);
            this.gb_address.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gb_address.Location = new System.Drawing.Point(543, 59);
            this.gb_address.Name = "gb_address";
            this.gb_address.Size = new System.Drawing.Size(341, 273);
            this.gb_address.TabIndex = 18;
            this.gb_address.TabStop = false;
            this.gb_address.Text = "Vendor Address";
            // 
            // ddlCountry
            // 
            this.ddlCountry.DataSource = this.countryDTOBindingSource;
            this.ddlCountry.DisplayMember = "CountryName";
            this.ddlCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCountry.FormattingEnabled = true;
            this.ddlCountry.Location = new System.Drawing.Point(113, 147);
            this.ddlCountry.Name = "ddlCountry";
            this.ddlCountry.Size = new System.Drawing.Size(121, 21);
            this.ddlCountry.TabIndex = 19;
            this.ddlCountry.ValueMember = "CountryId";
            this.ddlCountry.SelectedIndexChanged += new System.EventHandler(this.DdlCountry_SelectedIndexChanged);
            // 
            // countryDTOBindingSource
            // 
            this.countryDTOBindingSource.DataSource = typeof(Semnox.Core.GenericUtilities.CountryDTO);
            // 
            // ddlState
            // 
            this.ddlState.DataSource = this.stateDTOBindingSource;
            this.ddlState.DisplayMember = "State";
            this.ddlState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlState.FormattingEnabled = true;
            this.ddlState.Location = new System.Drawing.Point(656, 157);
            this.ddlState.Name = "ddlState";
            this.ddlState.Size = new System.Drawing.Size(121, 21);
            this.ddlState.TabIndex = 26;
            this.ddlState.ValueMember = "StateId";
            // 
            // stateDTOBindingSource
            // 
            this.stateDTOBindingSource.DataSource = typeof(Semnox.Core.GenericUtilities.StateDTO);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.lblErrorEmail);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(262, 193);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 158);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Contact Person";
            // 
            // lblErrorEmail
            // 
            this.lblErrorEmail.AutoSize = true;
            this.lblErrorEmail.ForeColor = System.Drawing.Color.Red;
            this.lblErrorEmail.Location = new System.Drawing.Point(139, 135);
            this.lblErrorEmail.Name = "lblErrorEmail";
            this.lblErrorEmail.Size = new System.Drawing.Size(100, 13);
            this.lblErrorEmail.TabIndex = 42;
            this.lblErrorEmail.Text = "Pls enter valid email";
            // 
            // txtContactname
            // 
            this.txtContactname.Location = new System.Drawing.Point(367, 209);
            this.txtContactname.Name = "txtContactname";
            this.txtContactname.Size = new System.Drawing.Size(130, 20);
            this.txtContactname.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(279, 212);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Contact Name:";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(367, 234);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(130, 20);
            this.txtPhone.TabIndex = 13;
            this.txtPhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPhone_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(279, 237);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Phone:";
            // 
            // txtFax
            // 
            this.txtFax.Location = new System.Drawing.Point(367, 260);
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(130, 20);
            this.txtFax.TabIndex = 15;
            this.txtFax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFax_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(279, 263);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(27, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Fax:";
            // 
            // txtMail
            // 
            this.txtMail.Location = new System.Drawing.Point(367, 286);
            this.txtMail.Name = "txtMail";
            this.txtMail.Size = new System.Drawing.Size(130, 20);
            this.txtMail.TabIndex = 17;
            this.txtMail.Validating += new System.ComponentModel.CancelEventHandler(this.txtMail_Validating);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(279, 289);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "email:";
            // 
            // txtWebsite
            // 
            this.txtWebsite.Location = new System.Drawing.Point(322, 113);
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(201, 20);
            this.txtWebsite.TabIndex = 6;
            this.txtWebsite.Validating += new System.ComponentModel.CancelEventHandler(this.txtWebsite_Validating);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(263, 116);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "Website:";
            // 
            // rtbRemarks
            // 
            this.rtbRemarks.Location = new System.Drawing.Point(261, 398);
            this.rtbRemarks.Name = "rtbRemarks";
            this.rtbRemarks.Size = new System.Drawing.Size(623, 50);
            this.rtbRemarks.TabIndex = 36;
            this.rtbRemarks.Text = "";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(258, 383);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "Remarks:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(263, 145);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(50, 13);
            this.label15.TabIndex = 7;
            this.label15.Text = "Active?:*";
            // 
            // ddlIsactive
            // 
            this.ddlIsactive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlIsactive.FormattingEnabled = true;
            this.ddlIsactive.Items.AddRange(new object[] {
            "Y",
            "N"});
            this.ddlIsactive.Location = new System.Drawing.Point(322, 139);
            this.ddlIsactive.Name = "ddlIsactive";
            this.ddlIsactive.Size = new System.Drawing.Size(66, 21);
            this.ddlIsactive.TabIndex = 8;
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(789, 457);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 37;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Semnox.Parafait.Inventory.Properties.Resources.add1;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(687, 457);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 40;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Image = global::Semnox.Parafait.Inventory.Properties.Resources.cancel;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(478, 457);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 38;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(263, 60);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(56, 13);
            this.label16.TabIndex = 39;
            this.label16.Text = "Vendor Id:";
            // 
            // lblVendorid
            // 
            this.lblVendorid.AutoSize = true;
            this.lblVendorid.Location = new System.Drawing.Point(319, 59);
            this.lblVendorid.Name = "lblVendorid";
            this.lblVendorid.Size = new System.Drawing.Size(0, 13);
            this.lblVendorid.TabIndex = 40;
            // 
            // lblErrorWebsite
            // 
            this.lblErrorWebsite.AutoSize = true;
            this.lblErrorWebsite.ForeColor = System.Drawing.Color.Red;
            this.lblErrorWebsite.Location = new System.Drawing.Point(417, 136);
            this.lblErrorWebsite.Name = "lblErrorWebsite";
            this.lblErrorWebsite.Size = new System.Drawing.Size(112, 13);
            this.lblErrorWebsite.TabIndex = 41;
            this.lblErrorWebsite.Text = "Pls enter valid website";
            // 
            // btnDuplicate
            // 
            this.btnDuplicate.Image = global::Semnox.Parafait.Inventory.Properties.Resources.duplicate;
            this.btnDuplicate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDuplicate.Location = new System.Drawing.Point(581, 457);
            this.btnDuplicate.Name = "btnDuplicate";
            this.btnDuplicate.Size = new System.Drawing.Size(82, 23);
            this.btnDuplicate.TabIndex = 39;
            this.btnDuplicate.Text = "Duplicate";
            this.btnDuplicate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDuplicate.UseVisualStyleBackColor = true;
            this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
            // 
            // txtTaxRegistrationNo
            // 
            this.txtTaxRegistrationNo.Location = new System.Drawing.Point(367, 358);
            this.txtTaxRegistrationNo.Name = "txtTaxRegistrationNo";
            this.txtTaxRegistrationNo.Size = new System.Drawing.Size(156, 20);
            this.txtTaxRegistrationNo.TabIndex = 33;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(263, 362);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(77, 13);
            this.label17.TabIndex = 43;
            this.label17.Text = "VAT / Tax No:";
            // 
            // txtVendorCode
            // 
            this.txtVendorCode.Location = new System.Drawing.Point(322, 165);
            this.txtVendorCode.Name = "txtVendorCode";
            this.txtVendorCode.Size = new System.Drawing.Size(201, 20);
            this.txtVendorCode.TabIndex = 9;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(263, 167);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 13);
            this.label18.TabIndex = 45;
            this.label18.Text = "Code:";
            // 
            // ddlPurchaseTax
            // 
            this.ddlPurchaseTax.DataSource = this.purchaseTaxDTOBindingSource;
            this.ddlPurchaseTax.DisplayMember = "TaxName";
            this.ddlPurchaseTax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPurchaseTax.FormattingEnabled = true;
            this.ddlPurchaseTax.Location = new System.Drawing.Point(569, 357);
            this.ddlPurchaseTax.Name = "ddlPurchaseTax";
            this.ddlPurchaseTax.Size = new System.Drawing.Size(121, 21);
            this.ddlPurchaseTax.TabIndex = 34;
            this.ddlPurchaseTax.ValueMember = "TaxId";
            // 
            // purchaseTaxDTOBindingSource
            // 
            this.purchaseTaxDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.TaxDTO);
            // 
            // txtVendorMarkup
            // 
            this.txtVendorMarkup.Location = new System.Drawing.Point(812, 358);
            this.txtVendorMarkup.Name = "txtVendorMarkup";
            this.txtVendorMarkup.Size = new System.Drawing.Size(50, 20);
            this.txtVendorMarkup.TabIndex = 35;
            // 
            // lblVendorMarkup
            // 
            this.lblVendorMarkup.AutoSize = true;
            this.lblVendorMarkup.Location = new System.Drawing.Point(749, 361);
            this.lblVendorMarkup.Name = "lblVendorMarkup";
            this.lblVendorMarkup.Size = new System.Drawing.Size(57, 13);
            this.lblVendorMarkup.TabIndex = 48;
            this.lblVendorMarkup.Text = "Markup %:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(540, 361);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(28, 13);
            this.label19.TabIndex = 47;
            this.label19.Text = "Tax:";
            // 
            // lnkPublishToSite
            // 
            this.lnkPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublishToSite.AutoSize = true;
            this.lnkPublishToSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublishToSite.Location = new System.Drawing.Point(763, 483);
            this.lnkPublishToSite.Name = "lnkPublishToSite";
            this.lnkPublishToSite.Size = new System.Drawing.Size(99, 13);
            this.lnkPublishToSite.TabIndex = 41;
            this.lnkPublishToSite.TabStop = true;
            this.lnkPublishToSite.Text = "Publish To Sites";
            this.lnkPublishToSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublishToSite_LinkClicked);
            // 
            // frmVendor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(896, 499);
            this.Controls.Add(this.ddlPurchaseTax);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.txtVendorMarkup);
            this.Controls.Add(this.lblVendorMarkup);
            this.Controls.Add(this.txtVendorCode);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.txtTaxRegistrationNo);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.btnDuplicate);
            this.Controls.Add(this.lblErrorWebsite);
            this.Controls.Add(this.lblVendorid);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.ddlIsactive);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.rtbRemarks);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtWebsite);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtMail);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtFax);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtContactname);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rtbAddressremarks);
            this.Controls.Add(this.txtZip);
            this.Controls.Add(this.ddlState);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.txtaddressline2);
            this.Controls.Add(this.txtAddressline1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.vendorFillByToolStrip);
            this.Controls.Add(this.gb_vendor);
            this.Controls.Add(this.gb_address);
            this.Controls.Add(this.lnkPublishToSite);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmVendor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vendor";
            this.Load += new System.EventHandler(this.frmVendor_Load);
            this.vendorFillByToolStrip.ResumeLayout(false);
            this.vendorFillByToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorDTOBindingSource)).EndInit();
            this.gb_vendor.ResumeLayout(false);
            this.gb_address.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.countryDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stateDTOBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseTaxDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion

        private System.Windows.Forms.ToolStrip vendorFillByToolStrip;
        private System.Windows.Forms.ToolStripLabel vendorNameToolStripLabel;
        private System.Windows.Forms.ToolStripTextBox vendorNameToolStripTextBox;
        private System.Windows.Forms.ToolStripButton vendorFillByToolStripButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtAddressline1;
        private System.Windows.Forms.TextBox txtaddressline2;
        private System.Windows.Forms.TextBox txtCity;
        // private System.Windows.Forms.TextBox txtState;
        private System.Windows.Forms.ComboBox ddlState;
        private System.Windows.Forms.TextBox txtZip;
        private System.Windows.Forms.RichTextBox rtbAddressremarks;
        private System.Windows.Forms.DataGridView dgvVendor;
        private System.Windows.Forms.GroupBox gb_vendor;
        private System.Windows.Forms.GroupBox gb_address;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtContactname;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtFax;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtWebsite;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.RichTextBox rtbRemarks;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox ddlIsactive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblVendorid;
        private System.Windows.Forms.Label lblErrorEmail;
        private System.Windows.Forms.Label lblErrorWebsite;
        private System.Windows.Forms.ComboBox ddlCountry;
        private System.Windows.Forms.Button btnDuplicate;
        private System.Windows.Forms.TextBox txtTaxRegistrationNo;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtVendorCode;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.BindingSource vendorDTOBindingSource;
        private System.Windows.Forms.ComboBox ddlPurchaseTax;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.BindingSource purchaseTaxDTOBindingSource;
        private System.Windows.Forms.BindingSource countryDTOBindingSource;
        private System.Windows.Forms.BindingSource stateDTOBindingSource;
        private System.Windows.Forms.TextBox txtVendorMarkup;
        private System.Windows.Forms.Label lblVendorMarkup;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastModDttm;
        private System.Windows.Forms.LinkLabel lnkPublishToSite;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorId;
        private System.Windows.Forms.DataGridViewTextBoxColumn vendorname;
        private System.Windows.Forms.DataGridViewTextBoxColumn vendorisactive;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxRegistrationNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn website;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContactName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fax;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address2;
        private System.Windows.Forms.DataGridViewTextBoxColumn City;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn Country;
        private System.Windows.Forms.DataGridViewTextBoxColumn PostalCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddressRemarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorRemarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastModUserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxId;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorMarkupPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountryId;
        private System.Windows.Forms.DataGridViewTextBoxColumn StateId;
    }
}