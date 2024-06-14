using System;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    partial class Customer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flpCustomerMain = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.panelCardNumber = new System.Windows.Forms.Panel();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.cmbTitle = new System.Windows.Forms.ComboBox();
            this.lblFirstname = new System.Windows.Forms.Label();
            this.panelFirstName = new System.Windows.Forms.Panel();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.panelLastName = new System.Windows.Forms.Panel();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.lblBirthDateFormat = new System.Windows.Forms.Label();
            this.lblAnniversaryDateFormat = new System.Windows.Forms.Label();
            this.panelBirthDate = new System.Windows.Forms.Panel();
            this.txtBirthDate = new System.Windows.Forms.MaskedTextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.panelContactPhone1 = new System.Windows.Forms.Panel();
            this.textBoxContactPhone1 = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.panelEmail = new System.Windows.Forms.Panel();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.lblUniqueId = new System.Windows.Forms.Label();
            this.panelUniqueId = new System.Windows.Forms.Panel();
            this.txtUniqueId = new System.Windows.Forms.TextBox();
            this.lblAddres1 = new System.Windows.Forms.Label();
            this.panelAddress1 = new System.Windows.Forms.Panel();
            this.textBoxAddress1 = new System.Windows.Forms.TextBox();
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.panelAddress2 = new System.Windows.Forms.Panel();
            this.textBoxAddress2 = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.panelCity = new System.Windows.Forms.Panel();
            this.textBoxCity = new System.Windows.Forms.TextBox();
            this.lblStateCombo = new System.Windows.Forms.Label();
            this.panelState = new System.Windows.Forms.Panel();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.lblPostalCode = new System.Windows.Forms.Label();
            this.panelPin = new System.Windows.Forms.Panel();
            this.textBoxPin = new System.Windows.Forms.TextBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.panelCountry = new System.Windows.Forms.Panel();
            this.cmbCountry = new System.Windows.Forms.ComboBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.panelGender = new System.Windows.Forms.Panel();
            this.comboBoxGender = new System.Windows.Forms.ComboBox();
            this.lblPromotionMode = new System.Windows.Forms.Label();
            this.panelOptInPromotionsMode = new System.Windows.Forms.Panel();
            this.cmbPromotionMode = new System.Windows.Forms.ComboBox();
            this.lblAnniversary = new System.Windows.Forms.Label();
            this.panelAnniversary = new System.Windows.Forms.Panel();
            this.txtAnniversary = new System.Windows.Forms.MaskedTextBox();
            this.pnlOptinPromotions = new System.Windows.Forms.Panel();
            this.btnOpinPromotions = new System.Windows.Forms.Button();
            this.lblOptinPromotions = new System.Windows.Forms.Label();
            this.flpCustomAttributes = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPhoto = new System.Windows.Forms.Label();
            this.pbCapture = new System.Windows.Forms.PictureBox();
            this.pnlTermsandConditions = new System.Windows.Forms.Panel();
            this.btnTermsandConditions = new System.Windows.Forms.Button();
            this.lblTermsAndConditions = new System.Windows.Forms.Label();
            this.pnlWhatsAppOptOut = new System.Windows.Forms.Panel();
            this.btnWhatsAppOptOut = new System.Windows.Forms.Button();
            this.lblWhatsAppOptOut = new System.Windows.Forms.Label();
            this.Label = new System.Windows.Forms.Label();
            this.textBoxMessageLine = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonCustomerSave = new System.Windows.Forms.Button();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.btnAddRelation = new System.Windows.Forms.Button();
            this.lblRegistrationMessage = new System.Windows.Forms.Label();
            this.lblTimeRemaining = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.PanelLinkedRelations = new System.Windows.Forms.Panel();
            this.bigVerticalScrollLinkedRelations = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.dgvLinkedRelations = new System.Windows.Forms.DataGridView();
            this.relatedCustomerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.relationshipTypeNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dOBDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contactPhoneDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uniqueIdentifierDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.genderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.address1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.address2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.address3DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.countryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.postalCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.anniversaryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.membershipIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.middleNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parentCustomerIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.relatedCustomerIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.optInPromotionsModeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.photoURLDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerRelationshipTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addressTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customCustomerDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblLinkedRelations = new System.Windows.Forms.Label();
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
            this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bigVerticalScrollCustomer = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.flpCustomerMain.SuspendLayout();
            this.panelCardNumber.SuspendLayout();
            this.panelTitle.SuspendLayout();
            this.panelFirstName.SuspendLayout();
            this.panelLastName.SuspendLayout();
            this.lblBirthDate.SuspendLayout();
            this.panelBirthDate.SuspendLayout();
            this.panelContactPhone1.SuspendLayout();
            this.panelEmail.SuspendLayout();
            this.panelUniqueId.SuspendLayout();
            this.panelAddress1.SuspendLayout();
            this.panelAddress2.SuspendLayout();
            this.panelCity.SuspendLayout();
            this.panelState.SuspendLayout();
            this.panelPin.SuspendLayout();
            this.panelCountry.SuspendLayout();
            this.panelGender.SuspendLayout();
            this.panelOptInPromotionsMode.SuspendLayout();
            this.panelAnniversary.SuspendLayout();
            this.pnlOptinPromotions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCapture)).BeginInit();
            this.pnlTermsandConditions.SuspendLayout();
            this.pnlWhatsAppOptOut.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.PanelLinkedRelations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLinkedRelations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customCustomerDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.home_button;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.TabIndex = 20013;
            // 
            // btnPrev
            // 
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrev.Location = new System.Drawing.Point(3, 14);
            this.btnPrev.TabIndex = 1026;
            this.btnPrev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonCustomerSave_MouseDown);
            this.btnPrev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonCustomerSave_MouseUp);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // flpCustomerMain
            // 
            this.flpCustomerMain.AutoScroll = true;
            this.flpCustomerMain.BackColor = System.Drawing.Color.Transparent;
            this.flpCustomerMain.Controls.Add(this.lblCardNumber);
            this.flpCustomerMain.Controls.Add(this.panelCardNumber);
            this.flpCustomerMain.Controls.Add(this.lblTitle);
            this.flpCustomerMain.Controls.Add(this.panelTitle);
            this.flpCustomerMain.Controls.Add(this.lblFirstname);
            this.flpCustomerMain.Controls.Add(this.panelFirstName);
            this.flpCustomerMain.Controls.Add(this.lblLastName);
            this.flpCustomerMain.Controls.Add(this.panelLastName);
            this.flpCustomerMain.Controls.Add(this.lblBirthDate);
            this.flpCustomerMain.Controls.Add(this.panelBirthDate);
            this.flpCustomerMain.Controls.Add(this.lblPhone);
            this.flpCustomerMain.Controls.Add(this.panelContactPhone1);
            this.flpCustomerMain.Controls.Add(this.lblEmail);
            this.flpCustomerMain.Controls.Add(this.panelEmail);
            this.flpCustomerMain.Controls.Add(this.lblUniqueId);
            this.flpCustomerMain.Controls.Add(this.panelUniqueId);
            this.flpCustomerMain.Controls.Add(this.lblAddres1);
            this.flpCustomerMain.Controls.Add(this.panelAddress1);
            this.flpCustomerMain.Controls.Add(this.lblAddress2);
            this.flpCustomerMain.Controls.Add(this.panelAddress2);
            this.flpCustomerMain.Controls.Add(this.lblCity);
            this.flpCustomerMain.Controls.Add(this.panelCity);
            this.flpCustomerMain.Controls.Add(this.lblStateCombo);
            this.flpCustomerMain.Controls.Add(this.panelState);
            this.flpCustomerMain.Controls.Add(this.lblPostalCode);
            this.flpCustomerMain.Controls.Add(this.panelPin);
            this.flpCustomerMain.Controls.Add(this.lblCountry);
            this.flpCustomerMain.Controls.Add(this.panelCountry);
            this.flpCustomerMain.Controls.Add(this.lblGender);
            this.flpCustomerMain.Controls.Add(this.panelGender);
            this.flpCustomerMain.Controls.Add(this.lblPromotionMode);
            this.flpCustomerMain.Controls.Add(this.panelOptInPromotionsMode);
            this.flpCustomerMain.Controls.Add(this.lblAnniversary);
            this.flpCustomerMain.Controls.Add(this.panelAnniversary);
            this.flpCustomerMain.Controls.Add(this.pnlOptinPromotions);
            this.flpCustomerMain.Controls.Add(this.flpCustomAttributes);
            this.flpCustomerMain.Controls.Add(this.lblPhoto);
            this.flpCustomerMain.Controls.Add(this.pbCapture);
            this.flpCustomerMain.Controls.Add(this.pnlTermsandConditions);
            this.flpCustomerMain.Controls.Add(this.pnlWhatsAppOptOut);
            this.flpCustomerMain.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpCustomerMain.ForeColor = System.Drawing.Color.White;
            this.flpCustomerMain.Location = new System.Drawing.Point(170, 166);
            this.flpCustomerMain.Name = "flpCustomerMain";
            this.flpCustomerMain.Size = new System.Drawing.Size(1375, 413);
            this.flpCustomerMain.TabIndex = 1601;
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblCardNumber.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.Location = new System.Drawing.Point(2, 2);
            this.lblCardNumber.Margin = new System.Windows.Forms.Padding(2);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(317, 66);
            this.lblCardNumber.TabIndex = 1;
            this.lblCardNumber.Text = "Card #:";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelCardNumber
            // 
            this.panelCardNumber.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelCardNumber.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelCardNumber.Controls.Add(this.txtCardNumber);
            this.panelCardNumber.Location = new System.Drawing.Point(323, 2);
            this.panelCardNumber.Margin = new System.Windows.Forms.Padding(2);
            this.panelCardNumber.Name = "panelCardNumber";
            this.panelCardNumber.Size = new System.Drawing.Size(310, 63);
            this.panelCardNumber.TabIndex = 1038;
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.BackColor = System.Drawing.Color.White;
            this.txtCardNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCardNumber.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtCardNumber.Location = new System.Drawing.Point(16, 16);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.ReadOnly = true;
            this.txtCardNumber.Size = new System.Drawing.Size(280, 39);
            this.txtCardNumber.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(637, 2);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(316, 66);
            this.lblTitle.TabIndex = 1036;
            this.lblTitle.Text = "Title:";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelTitle
            // 
            this.panelTitle.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelTitle.Controls.Add(this.cmbTitle);
            this.panelTitle.Location = new System.Drawing.Point(957, 2);
            this.panelTitle.Margin = new System.Windows.Forms.Padding(2);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(310, 63);
            this.panelTitle.TabIndex = 1039;
            // 
            // cmbTitle
            // 
            this.cmbTitle.BackColor = System.Drawing.Color.White;
            this.cmbTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbTitle.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTitle.ForeColor = System.Drawing.Color.DarkOrchid;
            this.cmbTitle.FormattingEnabled = true;
            this.cmbTitle.Location = new System.Drawing.Point(17, 17);
            this.cmbTitle.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbTitle.Name = "cmbTitle";
            this.cmbTitle.Size = new System.Drawing.Size(280, 40);
            this.cmbTitle.TabIndex = 2;
            this.cmbTitle.SelectedIndexChanged += new System.EventHandler(this.cmbTitle_SelectedIndexChanged);
            this.cmbTitle.Click += new System.EventHandler(this.cmbbox_Click);
            // 
            // lblFirstname
            // 
            this.lblFirstname.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstname.Location = new System.Drawing.Point(2, 72);
            this.lblFirstname.Margin = new System.Windows.Forms.Padding(2);
            this.lblFirstname.Name = "lblFirstname";
            this.lblFirstname.Size = new System.Drawing.Size(315, 66);
            this.lblFirstname.TabIndex = 3;
            this.lblFirstname.Text = "First Name:*";
            this.lblFirstname.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelFirstName
            // 
            this.panelFirstName.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelFirstName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelFirstName.Controls.Add(this.txtFirstName);
            this.panelFirstName.Location = new System.Drawing.Point(322, 73);
            this.panelFirstName.Name = "panelFirstName";
            this.panelFirstName.Padding = new System.Windows.Forms.Padding(2);
            this.panelFirstName.Size = new System.Drawing.Size(310, 63);
            this.panelFirstName.TabIndex = 1040;
            // 
            // txtFirstName
            // 
            this.txtFirstName.BackColor = System.Drawing.Color.White;
            this.txtFirstName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFirstName.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtFirstName.Location = new System.Drawing.Point(16, 14);
            this.txtFirstName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(280, 39);
            this.txtFirstName.TabIndex = 3;
            this.txtFirstName.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtFirstName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
            // 
            // lblLastName
            // 
            this.lblLastName.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastName.Location = new System.Drawing.Point(637, 72);
            this.lblLastName.Margin = new System.Windows.Forms.Padding(2);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(316, 66);
            this.lblLastName.TabIndex = 5;
            this.lblLastName.Text = "Last Name:";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelLastName
            // 
            this.panelLastName.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelLastName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelLastName.Controls.Add(this.txtLastName);
            this.panelLastName.Location = new System.Drawing.Point(957, 72);
            this.panelLastName.Margin = new System.Windows.Forms.Padding(2);
            this.panelLastName.Name = "panelLastName";
            this.panelLastName.Size = new System.Drawing.Size(310, 63);
            this.panelLastName.TabIndex = 1041;
            // 
            // txtLastName
            // 
            this.txtLastName.BackColor = System.Drawing.Color.White;
            this.txtLastName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLastName.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtLastName.Location = new System.Drawing.Point(17, 17);
            this.txtLastName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(280, 39);
            this.txtLastName.TabIndex = 4;
            this.txtLastName.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtLastName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.Controls.Add(this.lblBirthDateFormat);
            this.lblBirthDate.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBirthDate.Location = new System.Drawing.Point(2, 142);
            this.lblBirthDate.Margin = new System.Windows.Forms.Padding(2);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(316, 66);
            this.lblBirthDate.TabIndex = 25;
            this.lblBirthDate.Text = "Birth Date:";
            this.lblBirthDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDobFormat
            // 
            this.lblBirthDateFormat.Font = new System.Drawing.Font("Gotham Rounded Bold", 12.25F);
            this.lblBirthDateFormat.Location = new System.Drawing.Point(0, 48);
            this.lblBirthDateFormat.Margin = new System.Windows.Forms.Padding(2, 50, 2, 2);
            this.lblBirthDateFormat.Name = "lblBirthDateFormat";
            this.lblBirthDateFormat.Size = new System.Drawing.Size(310, 18);
            this.lblBirthDateFormat.TabIndex = 25;
            this.lblBirthDateFormat.Text = "(2024-Jan-01)";
            this.lblBirthDateFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAnniversaryDateFormat
            // 
            this.lblAnniversaryDateFormat.Font = new System.Drawing.Font("Gotham Rounded Bold", 12.25F);
            this.lblAnniversaryDateFormat.Location = new System.Drawing.Point(0, 48);
            this.lblAnniversaryDateFormat.Margin = new System.Windows.Forms.Padding(2, 50, 2, 2);
            this.lblAnniversaryDateFormat.Name = "lblBirthDateFormat";
            this.lblAnniversaryDateFormat.Size = new System.Drawing.Size(310, 18);
            this.lblAnniversaryDateFormat.TabIndex = 25;
            this.lblAnniversaryDateFormat.Text = "(2024-Jan-01)";
            this.lblAnniversaryDateFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelBirthDate
            // 
            this.panelBirthDate.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelBirthDate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBirthDate.Controls.Add(this.txtBirthDate);
            this.panelBirthDate.Location = new System.Drawing.Point(322, 142);
            this.panelBirthDate.Margin = new System.Windows.Forms.Padding(2);
            this.panelBirthDate.Name = "panelBirthDate";
            this.panelBirthDate.Size = new System.Drawing.Size(310, 63);
            this.panelBirthDate.TabIndex = 1604;
            // 
            // txtBirthDate
            // 
            this.txtBirthDate.BackColor = System.Drawing.Color.White;
            this.txtBirthDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBirthDate.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBirthDate.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtBirthDate.Location = new System.Drawing.Point(17, 17);
            this.txtBirthDate.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtBirthDate.Name = "txtBirthDate";
            this.txtBirthDate.Size = new System.Drawing.Size(280, 39);
            this.txtBirthDate.TabIndex = 5;
            this.txtBirthDate.Click += new System.EventHandler(this.txtBirthDate_Click);
            this.txtBirthDate.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // lblPhone
            // 
            this.lblPhone.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhone.Location = new System.Drawing.Point(636, 142);
            this.lblPhone.Margin = new System.Windows.Forms.Padding(2);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(316, 66);
            this.lblPhone.TabIndex = 3;
            this.lblPhone.Text = "Phone:";
            this.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelContactPhone1
            // 
            this.panelContactPhone1.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelContactPhone1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelContactPhone1.Controls.Add(this.textBoxContactPhone1);
            this.panelContactPhone1.Location = new System.Drawing.Point(956, 142);
            this.panelContactPhone1.Margin = new System.Windows.Forms.Padding(2);
            this.panelContactPhone1.Name = "panelContactPhone1";
            this.panelContactPhone1.Size = new System.Drawing.Size(310, 63);
            this.panelContactPhone1.TabIndex = 1604;
            // 
            // textBoxContactPhone1
            // 
            this.textBoxContactPhone1.BackColor = System.Drawing.Color.White;
            this.textBoxContactPhone1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxContactPhone1.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxContactPhone1.ForeColor = System.Drawing.Color.DarkOrchid;
            this.textBoxContactPhone1.Location = new System.Drawing.Point(17, 17);
            this.textBoxContactPhone1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxContactPhone1.Name = "textBoxContactPhone1";
            this.textBoxContactPhone1.Size = new System.Drawing.Size(280, 39);
            this.textBoxContactPhone1.TabIndex = 6;
            this.textBoxContactPhone1.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBoxContactPhone1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxContactPhone1_KeyPress);
            // 
            // lblEmail
            // 
            this.lblEmail.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(2, 212);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(2);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(316, 66);
            this.lblEmail.TabIndex = 7;
            this.lblEmail.Text = "E-Mail:";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelEmail
            // 
            this.panelEmail.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelEmail.Controls.Add(this.textBoxEmail);
            this.panelEmail.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelEmail.Location = new System.Drawing.Point(322, 212);
            this.panelEmail.Margin = new System.Windows.Forms.Padding(2);
            this.panelEmail.Name = "panelEmail";
            this.panelEmail.Size = new System.Drawing.Size(310, 63);
            this.panelEmail.TabIndex = 1604;
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.BackColor = System.Drawing.Color.White;
            this.textBoxEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxEmail.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEmail.ForeColor = System.Drawing.Color.DarkOrchid;
            this.textBoxEmail.Location = new System.Drawing.Point(17, 17);
            this.textBoxEmail.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(280, 39);
            this.textBoxEmail.TabIndex = 7;
            this.textBoxEmail.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBoxEmail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
            // 
            // lblUniqueId
            // 
            this.lblUniqueId.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUniqueId.Location = new System.Drawing.Point(636, 212);
            this.lblUniqueId.Margin = new System.Windows.Forms.Padding(2);
            this.lblUniqueId.Name = "lblUniqueId";
            this.lblUniqueId.Size = new System.Drawing.Size(316, 66);
            this.lblUniqueId.TabIndex = 3;
            this.lblUniqueId.Text = "Unique Id:";
            this.lblUniqueId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelUniqueId
            // 
            this.panelUniqueId.AutoScroll = true;
            this.panelUniqueId.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelUniqueId.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUniqueId.Controls.Add(this.txtUniqueId);
            this.panelUniqueId.Location = new System.Drawing.Point(956, 212);
            this.panelUniqueId.Margin = new System.Windows.Forms.Padding(2);
            this.panelUniqueId.Name = "panelUniqueId";
            this.panelUniqueId.Size = new System.Drawing.Size(310, 63);
            this.panelUniqueId.TabIndex = 1040;
            // 
            // txtUniqueId
            // 
            this.txtUniqueId.BackColor = System.Drawing.Color.White;
            this.txtUniqueId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUniqueId.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUniqueId.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtUniqueId.Location = new System.Drawing.Point(17, 17);
            this.txtUniqueId.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtUniqueId.Name = "txtUniqueId";
            this.txtUniqueId.Size = new System.Drawing.Size(280, 39);
            this.txtUniqueId.TabIndex = 8;
            this.txtUniqueId.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtUniqueId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
            this.txtUniqueId.Validating += new System.ComponentModel.CancelEventHandler(this.txtUniqueId_Validating);
            // 
            // lblAddres1
            // 
            this.lblAddres1.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddres1.Location = new System.Drawing.Point(2, 282);
            this.lblAddres1.Margin = new System.Windows.Forms.Padding(2);
            this.lblAddres1.Name = "lblAddres1";
            this.lblAddres1.Size = new System.Drawing.Size(316, 66);
            this.lblAddres1.TabIndex = 11;
            this.lblAddres1.Text = "Address 1:";
            this.lblAddres1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelAddress1
            // 
            this.panelAddress1.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelAddress1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelAddress1.Controls.Add(this.textBoxAddress1);
            this.panelAddress1.Location = new System.Drawing.Point(322, 282);
            this.panelAddress1.Margin = new System.Windows.Forms.Padding(2);
            this.panelAddress1.Name = "panelAddress1";
            this.panelAddress1.Size = new System.Drawing.Size(310, 63);
            this.panelAddress1.TabIndex = 1605;
            // 
            // textBoxAddress1
            // 
            this.textBoxAddress1.BackColor = System.Drawing.Color.White;
            this.textBoxAddress1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxAddress1.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAddress1.ForeColor = System.Drawing.Color.DarkOrchid;
            this.textBoxAddress1.Location = new System.Drawing.Point(17, 17);
            this.textBoxAddress1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxAddress1.Name = "textBoxAddress1";
            this.textBoxAddress1.Size = new System.Drawing.Size(280, 39);
            this.textBoxAddress1.TabIndex = 9;
            this.textBoxAddress1.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBoxAddress1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
            // 
            // lblAddress2
            // 
            this.lblAddress2.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress2.Location = new System.Drawing.Point(637, 280);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(315, 66);
            this.lblAddress2.TabIndex = 13;
            this.lblAddress2.Text = "Address 2:";
            this.lblAddress2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelAddress2
            // 
            this.panelAddress2.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelAddress2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelAddress2.Controls.Add(this.textBoxAddress2);
            this.panelAddress2.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelAddress2.Location = new System.Drawing.Point(957, 282);
            this.panelAddress2.Margin = new System.Windows.Forms.Padding(2);
            this.panelAddress2.Name = "panelAddress2";
            this.panelAddress2.Size = new System.Drawing.Size(310, 63);
            this.panelAddress2.TabIndex = 1605;
            // 
            // textBoxAddress2
            // 
            this.textBoxAddress2.AcceptsReturn = true;
            this.textBoxAddress2.BackColor = System.Drawing.Color.White;
            this.textBoxAddress2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxAddress2.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAddress2.ForeColor = System.Drawing.Color.DarkOrchid;
            this.textBoxAddress2.Location = new System.Drawing.Point(17, 17);
            this.textBoxAddress2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxAddress2.Name = "textBoxAddress2";
            this.textBoxAddress2.Size = new System.Drawing.Size(280, 39);
            this.textBoxAddress2.TabIndex = 10;
            this.textBoxAddress2.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBoxAddress2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
            // 
            // lblCity
            // 
            this.lblCity.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCity.Location = new System.Drawing.Point(2, 352);
            this.lblCity.Margin = new System.Windows.Forms.Padding(2);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(316, 66);
            this.lblCity.TabIndex = 15;
            this.lblCity.Text = "City:";
            this.lblCity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelCity
            // 
            this.panelCity.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelCity.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelCity.Controls.Add(this.textBoxCity);
            this.panelCity.Location = new System.Drawing.Point(322, 352);
            this.panelCity.Margin = new System.Windows.Forms.Padding(2);
            this.panelCity.Name = "panelCity";
            this.panelCity.Size = new System.Drawing.Size(310, 63);
            this.panelCity.TabIndex = 1605;
            // 
            // textBoxCity
            // 
            this.textBoxCity.BackColor = System.Drawing.Color.White;
            this.textBoxCity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCity.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCity.ForeColor = System.Drawing.Color.DarkOrchid;
            this.textBoxCity.Location = new System.Drawing.Point(17, 17);
            this.textBoxCity.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxCity.Name = "textBoxCity";
            this.textBoxCity.Size = new System.Drawing.Size(280, 39);
            this.textBoxCity.TabIndex = 11;
            this.textBoxCity.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBoxCity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
            // 
            // lblStateCombo
            // 
            this.lblStateCombo.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStateCombo.Location = new System.Drawing.Point(636, 352);
            this.lblStateCombo.Margin = new System.Windows.Forms.Padding(2);
            this.lblStateCombo.Name = "lblStateCombo";
            this.lblStateCombo.Size = new System.Drawing.Size(316, 66);
            this.lblStateCombo.TabIndex = 19;
            this.lblStateCombo.Text = "State:";
            this.lblStateCombo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelState
            // 
            this.panelState.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelState.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelState.Controls.Add(this.cmbState);
            this.panelState.Location = new System.Drawing.Point(956, 352);
            this.panelState.Margin = new System.Windows.Forms.Padding(2);
            this.panelState.Name = "panelState";
            this.panelState.Size = new System.Drawing.Size(310, 63);
            this.panelState.TabIndex = 1605;
            // 
            // cmbState
            // 
            this.cmbState.BackColor = System.Drawing.Color.White;
            this.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbState.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbState.ForeColor = System.Drawing.Color.DarkOrchid;
            this.cmbState.FormattingEnabled = true;
            this.cmbState.Items.AddRange(new object[] {
            "Female",
            "Male",
            "Not Set"});
            this.cmbState.Location = new System.Drawing.Point(17, 11);
            this.cmbState.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(280, 47);
            this.cmbState.TabIndex = 12;
            this.cmbState.Click += new System.EventHandler(this.cmbbox_Click);
            // 
            // lblPostalCode
            // 
            this.lblPostalCode.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPostalCode.Location = new System.Drawing.Point(2, 422);
            this.lblPostalCode.Margin = new System.Windows.Forms.Padding(2);
            this.lblPostalCode.Name = "lblPostalCode";
            this.lblPostalCode.Size = new System.Drawing.Size(316, 66);
            this.lblPostalCode.TabIndex = 23;
            this.lblPostalCode.Text = "Postal Code:";
            this.lblPostalCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelPin
            // 
            this.panelPin.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelPin.Controls.Add(this.textBoxPin);
            this.panelPin.Location = new System.Drawing.Point(322, 422);
            this.panelPin.Margin = new System.Windows.Forms.Padding(2);
            this.panelPin.Name = "panelPin";
            this.panelPin.Size = new System.Drawing.Size(310, 63);
            this.panelPin.TabIndex = 1605;
            // 
            // textBoxPin
            // 
            this.textBoxPin.BackColor = System.Drawing.Color.White;
            this.textBoxPin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPin.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPin.ForeColor = System.Drawing.Color.DarkOrchid;
            this.textBoxPin.Location = new System.Drawing.Point(17, 17);
            this.textBoxPin.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxPin.Name = "textBoxPin";
            this.textBoxPin.Size = new System.Drawing.Size(280, 39);
            this.textBoxPin.TabIndex = 13;
            this.textBoxPin.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBoxPin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
            // 
            // lblCountry
            // 
            this.lblCountry.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountry.Location = new System.Drawing.Point(636, 422);
            this.lblCountry.Margin = new System.Windows.Forms.Padding(2);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(316, 66);
            this.lblCountry.TabIndex = 21;
            this.lblCountry.Text = "Country:";
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelCountry
            // 
            this.panelCountry.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelCountry.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelCountry.Controls.Add(this.cmbCountry);
            this.panelCountry.Location = new System.Drawing.Point(956, 422);
            this.panelCountry.Margin = new System.Windows.Forms.Padding(2);
            this.panelCountry.Name = "panelCountry";
            this.panelCountry.Size = new System.Drawing.Size(310, 63);
            this.panelCountry.TabIndex = 1605;
            // 
            // cmbCountry
            // 
            this.cmbCountry.BackColor = System.Drawing.Color.White;
            this.cmbCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCountry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCountry.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCountry.ForeColor = System.Drawing.Color.DarkOrchid;
            this.cmbCountry.FormattingEnabled = true;
            this.cmbCountry.Location = new System.Drawing.Point(17, 8);
            this.cmbCountry.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbCountry.Name = "cmbCountry";
            this.cmbCountry.Size = new System.Drawing.Size(280, 47);
            this.cmbCountry.TabIndex = 14;
            this.cmbCountry.Click += new System.EventHandler(this.cmbbox_Click);
            // 
            // lblGender
            // 
            this.lblGender.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGender.Location = new System.Drawing.Point(2, 492);
            this.lblGender.Margin = new System.Windows.Forms.Padding(2);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(316, 66);
            this.lblGender.TabIndex = 37;
            this.lblGender.Text = "Gender:";
            this.lblGender.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelGender
            // 
            this.panelGender.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelGender.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelGender.Controls.Add(this.comboBoxGender);
            this.panelGender.Location = new System.Drawing.Point(322, 492);
            this.panelGender.Margin = new System.Windows.Forms.Padding(2);
            this.panelGender.Name = "panelGender";
            this.panelGender.Size = new System.Drawing.Size(310, 63);
            this.panelGender.TabIndex = 1604;
            // 
            // comboBoxGender
            // 
            this.comboBoxGender.BackColor = System.Drawing.Color.White;
            this.comboBoxGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxGender.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxGender.ForeColor = System.Drawing.Color.DarkOrchid;
            this.comboBoxGender.FormattingEnabled = true;
            this.comboBoxGender.Items.AddRange(new object[] {
            "Female",
            "Male",
            "Not Set"});
            this.comboBoxGender.Location = new System.Drawing.Point(17, 11);
            this.comboBoxGender.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.comboBoxGender.Name = "comboBoxGender";
            this.comboBoxGender.Size = new System.Drawing.Size(280, 47);
            this.comboBoxGender.TabIndex = 15;
            this.comboBoxGender.Click += new System.EventHandler(this.cmbbox_Click);
            // 
            // lblPromotionMode
            // 
            this.lblPromotionMode.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPromotionMode.Location = new System.Drawing.Point(636, 492);
            this.lblPromotionMode.Margin = new System.Windows.Forms.Padding(2);
            this.lblPromotionMode.Name = "lblPromotionMode";
            this.lblPromotionMode.Size = new System.Drawing.Size(316, 66);
            this.lblPromotionMode.TabIndex = 1606;
            this.lblPromotionMode.Text = "Promotion Mode:";
            this.lblPromotionMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelOptInPromotionsMode
            // 
            this.panelOptInPromotionsMode.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelOptInPromotionsMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelOptInPromotionsMode.Controls.Add(this.cmbPromotionMode);
            this.panelOptInPromotionsMode.Location = new System.Drawing.Point(956, 492);
            this.panelOptInPromotionsMode.Margin = new System.Windows.Forms.Padding(2);
            this.panelOptInPromotionsMode.Name = "panelOptInPromotionsMode";
            this.panelOptInPromotionsMode.Size = new System.Drawing.Size(310, 63);
            this.panelOptInPromotionsMode.TabIndex = 1607;
            this.panelOptInPromotionsMode.Tag = "OptInPromotionsMode";
            // 
            // cmbPromotionMode
            // 
            this.cmbPromotionMode.BackColor = System.Drawing.Color.White;
            this.cmbPromotionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPromotionMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPromotionMode.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPromotionMode.ForeColor = System.Drawing.Color.DarkOrchid;
            this.cmbPromotionMode.FormattingEnabled = true;
            this.cmbPromotionMode.Location = new System.Drawing.Point(17, 8);
            this.cmbPromotionMode.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbPromotionMode.Name = "cmbPromotionMode";
            this.cmbPromotionMode.Size = new System.Drawing.Size(280, 47);
            this.cmbPromotionMode.TabIndex = 16;
            this.cmbPromotionMode.Click += new System.EventHandler(this.cmbbox_Click);
            // 
            // lblAnniversary
            // 
            this.lblAnniversary.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnniversary.Controls.Add(this.lblAnniversaryDateFormat);
            this.lblAnniversary.Location = new System.Drawing.Point(2, 562);
            this.lblAnniversary.Margin = new System.Windows.Forms.Padding(2);
            this.lblAnniversary.Name = "lblAnniversary";
            this.lblAnniversary.Size = new System.Drawing.Size(316, 66);
            this.lblAnniversary.TabIndex = 25;
            this.lblAnniversary.Text = "Anniversary Date:";
            this.lblAnniversary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelAnniversary
            // 
            this.panelAnniversary.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panelAnniversary.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelAnniversary.Controls.Add(this.txtAnniversary);
            this.panelAnniversary.Location = new System.Drawing.Point(322, 562);
            this.panelAnniversary.Margin = new System.Windows.Forms.Padding(2);
            this.panelAnniversary.Name = "panelAnniversary";
            this.panelAnniversary.Size = new System.Drawing.Size(310, 63);
            this.panelAnniversary.TabIndex = 1604;
            // 
            // txtAnniversary
            // 
            this.txtAnniversary.BackColor = System.Drawing.Color.White;
            this.txtAnniversary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAnniversary.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAnniversary.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtAnniversary.Location = new System.Drawing.Point(16, 16);
            this.txtAnniversary.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtAnniversary.Name = "txtAnniversary";
            this.txtAnniversary.Size = new System.Drawing.Size(280, 39);
            this.txtAnniversary.TabIndex = 17;
            this.txtAnniversary.Click += new System.EventHandler(this.txtAnniversary_Click);
            this.txtAnniversary.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // pnlOptinPromotions
            // 
            this.pnlOptinPromotions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlOptinPromotions.Controls.Add(this.btnOpinPromotions);
            this.pnlOptinPromotions.Controls.Add(this.lblOptinPromotions);
            this.pnlOptinPromotions.Location = new System.Drawing.Point(3, 633);
            this.pnlOptinPromotions.Name = "pnlOptinPromotions";
            this.pnlOptinPromotions.Size = new System.Drawing.Size(982, 94);
            this.pnlOptinPromotions.TabIndex = 1608;
            this.pnlOptinPromotions.Click += new System.EventHandler(this.pnlOptinPromotions_Click);
            // 
            // btnOpinPromotions
            // 
            this.btnOpinPromotions.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOpinPromotions.Image = global::Parafait_Kiosk.Properties.Resources.tick_box_unchecked;
            this.btnOpinPromotions.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpinPromotions.Location = new System.Drawing.Point(8, 3);
            this.btnOpinPromotions.Name = "btnOpinPromotions";
            this.btnOpinPromotions.Size = new System.Drawing.Size(118, 78);
            this.btnOpinPromotions.TabIndex = 18;
            this.btnOpinPromotions.UseVisualStyleBackColor = true;
            this.btnOpinPromotions.Click += new System.EventHandler(this.pnlOptinPromotions_Click);
            // 
            // lblOptinPromotions
            // 
            this.lblOptinPromotions.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOptinPromotions.Location = new System.Drawing.Point(125, -1);
            this.lblOptinPromotions.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblOptinPromotions.Name = "lblOptinPromotions";
            this.lblOptinPromotions.Size = new System.Drawing.Size(854, 87);
            this.lblOptinPromotions.TabIndex = 1607;
            this.lblOptinPromotions.Text = "Opt in Promotions";
            this.lblOptinPromotions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOptinPromotions.Click += new System.EventHandler(this.pnlOptinPromotions_Click);
            // 
            // flpCustomAttributes
            // 
            this.flpCustomAttributes.AutoSize = true;
            this.flpCustomAttributes.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpCustomAttributes.Location = new System.Drawing.Point(988, 630);
            this.flpCustomAttributes.Margin = new System.Windows.Forms.Padding(0);
            this.flpCustomAttributes.MinimumSize = new System.Drawing.Size(300, 60);
            this.flpCustomAttributes.Name = "flpCustomAttributes";
            this.flpCustomAttributes.Size = new System.Drawing.Size(300, 60);
            this.flpCustomAttributes.TabIndex = 39;
            // 
            // lblPhoto
            // 
            this.lblPhoto.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhoto.ForeColor = System.Drawing.Color.White;
            this.lblPhoto.Location = new System.Drawing.Point(3, 730);
            this.lblPhoto.Name = "lblPhoto";
            this.lblPhoto.Size = new System.Drawing.Size(200, 46);
            this.lblPhoto.TabIndex = 38;
            this.lblPhoto.Text = "Photo:";
            this.lblPhoto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbCapture
            // 
            this.pbCapture.BackColor = System.Drawing.Color.Transparent;
            this.pbCapture.Image = global::Parafait_Kiosk.Properties.Resources.profile_picture_placeholder;
            this.pbCapture.Location = new System.Drawing.Point(209, 733);
            this.pbCapture.Name = "pbCapture";
            this.pbCapture.Size = new System.Drawing.Size(200, 120);
            this.pbCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCapture.TabIndex = 1035;
            this.pbCapture.TabStop = false;
            this.pbCapture.Click += new System.EventHandler(this.pbCapture_Click);
            // 
            // pnlTermsandConditions
            // 
            this.pnlTermsandConditions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlTermsandConditions.Controls.Add(this.btnTermsandConditions);
            this.pnlTermsandConditions.Controls.Add(this.lblTermsAndConditions);
            this.pnlTermsandConditions.Location = new System.Drawing.Point(3, 859);
            this.pnlTermsandConditions.Name = "pnlTermsandConditions";
            this.pnlTermsandConditions.Size = new System.Drawing.Size(982, 100);
            this.pnlTermsandConditions.TabIndex = 1609;
            this.pnlTermsandConditions.Tag = "false";
            this.pnlTermsandConditions.Visible = false;
            this.pnlTermsandConditions.Click += new System.EventHandler(this.pnlTermsandConditions_Click);
            // 
            // btnTermsandConditions
            // 
            this.btnTermsandConditions.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTermsandConditions.Image = global::Parafait_Kiosk.Properties.Resources.tick_box_unchecked;
            this.btnTermsandConditions.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTermsandConditions.Location = new System.Drawing.Point(8, 3);
            this.btnTermsandConditions.Name = "btnTermsandConditions";
            this.btnTermsandConditions.Size = new System.Drawing.Size(118, 78);
            this.btnTermsandConditions.TabIndex = 19;
            this.btnTermsandConditions.UseVisualStyleBackColor = true;
            this.btnTermsandConditions.Click += new System.EventHandler(this.pnlTermsandConditions_Click);
            // 
            // lblTermsAndConditions
            // 
            this.lblTermsAndConditions.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTermsAndConditions.Location = new System.Drawing.Point(133, 0);
            this.lblTermsAndConditions.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblTermsAndConditions.Name = "lblTermsAndConditions";
            this.lblTermsAndConditions.Size = new System.Drawing.Size(846, 98);
            this.lblTermsAndConditions.TabIndex = 1607;
            this.lblTermsAndConditions.Text = "Terms and Conditions";
            this.lblTermsAndConditions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTermsAndConditions.Click += new System.EventHandler(this.pnlTermsandConditions_Click);
            // 
            // pnlWhatsAppOptOut
            // 
            this.pnlWhatsAppOptOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlWhatsAppOptOut.Controls.Add(this.btnWhatsAppOptOut);
            this.pnlWhatsAppOptOut.Controls.Add(this.lblWhatsAppOptOut);
            this.pnlWhatsAppOptOut.Location = new System.Drawing.Point(3, 965);
            this.pnlWhatsAppOptOut.Name = "pnlWhatsAppOptOut";
            this.pnlWhatsAppOptOut.Size = new System.Drawing.Size(500, 94);
            this.pnlWhatsAppOptOut.TabIndex = 1608;
            this.pnlWhatsAppOptOut.Click += new System.EventHandler(this.pnlWhatsAppOptin_Click);
            // 
            // btnWhatsAppOptOut
            // 
            this.btnWhatsAppOptOut.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnWhatsAppOptOut.Image = global::Parafait_Kiosk.Properties.Resources.tick_box_unchecked;
            this.btnWhatsAppOptOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWhatsAppOptOut.Location = new System.Drawing.Point(8, 3);
            this.btnWhatsAppOptOut.Name = "btnWhatsAppOptOut";
            this.btnWhatsAppOptOut.Size = new System.Drawing.Size(118, 78);
            this.btnWhatsAppOptOut.TabIndex = 20;
            this.btnWhatsAppOptOut.UseVisualStyleBackColor = true;
            this.btnWhatsAppOptOut.Click += new System.EventHandler(this.pnlWhatsAppOptin_Click);
            // 
            // lblWhatsAppOptOut
            // 
            this.lblWhatsAppOptOut.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWhatsAppOptOut.Location = new System.Drawing.Point(133, 0);
            this.lblWhatsAppOptOut.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblWhatsAppOptOut.Name = "lblWhatsAppOptOut";
            this.lblWhatsAppOptOut.Size = new System.Drawing.Size(854, 87);
            this.lblWhatsAppOptOut.TabIndex = 1607;
            this.lblWhatsAppOptOut.Text = "Opt Out WhatsApp";
            this.lblWhatsAppOptOut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblWhatsAppOptOut.Click += new System.EventHandler(this.pnlWhatsAppOptin_Click);
            // 
            // Label
            // 
            this.Label.BackColor = System.Drawing.Color.White;
            this.Label.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label.ForeColor = System.Drawing.Color.DarkOrchid;
            this.Label.Location = new System.Drawing.Point(27, 12);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(369, 48);
            this.Label.TabIndex = 6;
            this.Label.Click += new System.EventHandler(this.txtBirthDate_Click);
            this.Label.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // textBoxMessageLine
            // 
            this.textBoxMessageLine.BackColor = System.Drawing.Color.Transparent;
            this.textBoxMessageLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxMessageLine.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessageLine.ForeColor = System.Drawing.Color.White;
            this.textBoxMessageLine.Location = new System.Drawing.Point(0, 1048);
            this.textBoxMessageLine.Name = "textBoxMessageLine";
            this.textBoxMessageLine.Size = new System.Drawing.Size(1920, 32);
            this.textBoxMessageLine.TabIndex = 1;
            this.textBoxMessageLine.Text = "Message";
            this.textBoxMessageLine.UseVisualStyleBackColor = false;
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.btnPrev);
            this.panelButtons.Controls.Add(this.buttonCustomerSave);
            this.panelButtons.Controls.Add(this.btnShowKeyPad);
            this.panelButtons.Controls.Add(this.btnAddRelation);
            this.panelButtons.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(469, 612);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1130, 155);
            this.panelButtons.TabIndex = 1039;
            this.panelButtons.Controls.SetChildIndex(this.btnAddRelation, 0);
            this.panelButtons.Controls.SetChildIndex(this.btnShowKeyPad, 0);
            this.panelButtons.Controls.SetChildIndex(this.buttonCustomerSave, 0);
            this.panelButtons.Controls.SetChildIndex(this.btnPrev, 0);
            // 
            // buttonCustomerSave
            // 
            this.buttonCustomerSave.BackColor = System.Drawing.Color.Transparent;
            this.buttonCustomerSave.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.buttonCustomerSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCustomerSave.FlatAppearance.BorderSize = 0;
            this.buttonCustomerSave.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonCustomerSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonCustomerSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonCustomerSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCustomerSave.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCustomerSave.ForeColor = System.Drawing.Color.White;
            this.buttonCustomerSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCustomerSave.Location = new System.Drawing.Point(372, 14);
            this.buttonCustomerSave.Name = "buttonCustomerSave";
            this.buttonCustomerSave.Size = new System.Drawing.Size(250, 125);
            this.buttonCustomerSave.TabIndex = 1025;
            this.buttonCustomerSave.Text = "Save";
            this.buttonCustomerSave.UseVisualStyleBackColor = false;
            this.buttonCustomerSave.Click += new System.EventHandler(this.buttonCustomerSave_Click);
            this.buttonCustomerSave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonCustomerSave_MouseDown);
            this.buttonCustomerSave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonCustomerSave_MouseUp);
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Image = global::Parafait_Kiosk.Properties.Resources.Keyboard_1;
            this.btnShowKeyPad.Location = new System.Drawing.Point(1040, 40);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(87, 83);
            this.btnShowKeyPad.TabIndex = 20001;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.btnShowKeyPad_Click);
            // 
            // btnAddRelation
            // 
            this.btnAddRelation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddRelation.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRelation.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnAddRelation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddRelation.FlatAppearance.BorderSize = 0;
            this.btnAddRelation.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAddRelation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddRelation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddRelation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddRelation.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRelation.ForeColor = System.Drawing.Color.White;
            this.btnAddRelation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddRelation.Location = new System.Drawing.Point(741, 14);
            this.btnAddRelation.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddRelation.Name = "btnAddRelation";
            this.btnAddRelation.Size = new System.Drawing.Size(250, 125);
            this.btnAddRelation.TabIndex = 20018;
            this.btnAddRelation.Text = "Add Family";
            this.btnAddRelation.UseVisualStyleBackColor = false;
            this.btnAddRelation.Click += new System.EventHandler(this.btnAddRelation_Click);
            // 
            // lblRegistrationMessage
            // 
            this.lblRegistrationMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRegistrationMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblRegistrationMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegistrationMessage.ForeColor = System.Drawing.Color.White;
            this.lblRegistrationMessage.Location = new System.Drawing.Point(183, 91);
            this.lblRegistrationMessage.Name = "lblRegistrationMessage";
            this.lblRegistrationMessage.Size = new System.Drawing.Size(1536, 57);
            this.lblRegistrationMessage.TabIndex = 1602;
            this.lblRegistrationMessage.Text = "Fill out the Information to Register Today!";
            this.lblRegistrationMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRegistrationMessage.Visible = false;
            // 
            // lblTimeRemaining
            // 
            this.lblTimeRemaining.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeRemaining.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemaining.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.timer_SmallBox;
            this.lblTimeRemaining.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lblTimeRemaining.FlatAppearance.BorderSize = 0;
            this.lblTimeRemaining.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemaining.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemaining.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeRemaining.Font = new System.Drawing.Font("Verdana", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeRemaining.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblTimeRemaining.Location = new System.Drawing.Point(1749, 38);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(142, 110);
            this.lblTimeRemaining.TabIndex = 1603;
            this.lblTimeRemaining.Text = "1:45";
            this.lblTimeRemaining.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Gotham Rounded Bold", 39F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(195, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1530, 76);
            this.button1.TabIndex = 20014;
            this.button1.Text = "Register";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // PanelLinkedRelations
            // 
            this.PanelLinkedRelations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelLinkedRelations.AutoScroll = true;
            this.PanelLinkedRelations.BackColor = System.Drawing.Color.Transparent;
            this.PanelLinkedRelations.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Table1;
            this.PanelLinkedRelations.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PanelLinkedRelations.Controls.Add(this.bigVerticalScrollLinkedRelations);
            this.PanelLinkedRelations.Controls.Add(this.lblLinkedRelations);
            this.PanelLinkedRelations.Controls.Add(this.dgvLinkedRelations);
            this.PanelLinkedRelations.Location = new System.Drawing.Point(515, 795);
            this.PanelLinkedRelations.Name = "PanelLinkedRelations";
            this.PanelLinkedRelations.Size = new System.Drawing.Size(902, 247);
            this.PanelLinkedRelations.TabIndex = 20020;
            // 
            // bigVerticalScrollLinkedRelations
            // 
            this.bigVerticalScrollLinkedRelations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bigVerticalScrollLinkedRelations.AutoHide = false;
            this.bigVerticalScrollLinkedRelations.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollLinkedRelations.DataGridView = this.dgvLinkedRelations;
            this.bigVerticalScrollLinkedRelations.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollLinkedRelations.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollLinkedRelations.Location = new System.Drawing.Point(835, 63);
            this.bigVerticalScrollLinkedRelations.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.bigVerticalScrollLinkedRelations.Name = "bigVerticalScrollLinkedRelations";
            this.bigVerticalScrollLinkedRelations.ScrollableControl = null;
            this.bigVerticalScrollLinkedRelations.ScrollViewer = null;
            this.bigVerticalScrollLinkedRelations.Size = new System.Drawing.Size(63, 160);
            this.bigVerticalScrollLinkedRelations.TabIndex = 20018;
            this.bigVerticalScrollLinkedRelations.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollLinkedRelations.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollLinkedRelations.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollLinkedRelations.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // dgvLinkedRelations
            // 
            this.dgvLinkedRelations.AllowUserToAddRows = false;
            this.dgvLinkedRelations.AllowUserToDeleteRows = false;
            this.dgvLinkedRelations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLinkedRelations.AutoGenerateColumns = false;
            this.dgvLinkedRelations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLinkedRelations.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(204)))), ((int)(((byte)(208)))));
            this.dgvLinkedRelations.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLinkedRelations.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLinkedRelations.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvLinkedRelations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLinkedRelations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.relatedCustomerNameDataGridViewTextBoxColumn,
            this.relationshipTypeNameDataGridViewTextBoxColumn,
            this.dOBDataGridViewTextBoxColumn,
            this.contactPhoneDataGridViewTextBoxColumn,
            this.emailAddressDataGridViewTextBoxColumn,
            this.uniqueIdentifierDataGridViewTextBoxColumn,
            this.genderDataGridViewTextBoxColumn,
            this.address1DataGridViewTextBoxColumn,
            this.address2DataGridViewTextBoxColumn,
            this.address3DataGridViewTextBoxColumn,
            this.cityDataGridViewTextBoxColumn,
            this.stateDataGridViewTextBoxColumn,
            this.countryDataGridViewTextBoxColumn,
            this.postalCodeDataGridViewTextBoxColumn,
            this.anniversaryDataGridViewTextBoxColumn,
            this.lastNameDataGridViewTextBoxColumn,
            this.titleDataGridViewTextBoxColumn,
            this.membershipIdDataGridViewTextBoxColumn,
            this.middleNameDataGridViewTextBoxColumn,
            this.parentCustomerIdDataGridViewTextBoxColumn,
            this.relatedCustomerIdDataGridViewTextBoxColumn,
            this.optInPromotionsModeDataGridViewTextBoxColumn,
            this.photoURLDataGridViewTextBoxColumn,
            this.customerRelationshipTypeIdDataGridViewTextBoxColumn,
            this.addressTypeDataGridViewTextBoxColumn,
            this.customerTypeDataGridViewTextBoxColumn});
            this.dgvLinkedRelations.DataSource = this.customCustomerDTOBindingSource;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Gotham Rounded Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLinkedRelations.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvLinkedRelations.EnableHeadersVisualStyles = false;
            this.dgvLinkedRelations.GridColor = System.Drawing.SystemColors.Control;
            this.dgvLinkedRelations.Location = new System.Drawing.Point(3, 63);
            this.dgvLinkedRelations.Name = "dgvLinkedRelations";
            this.dgvLinkedRelations.ReadOnly = true;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLinkedRelations.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvLinkedRelations.RowHeadersVisible = false;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLinkedRelations.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvLinkedRelations.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLinkedRelations.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvLinkedRelations.RowTemplate.Height = 60;
            this.dgvLinkedRelations.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLinkedRelations.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvLinkedRelations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLinkedRelations.Size = new System.Drawing.Size(833, 160);
            this.dgvLinkedRelations.TabIndex = 36;
            this.dgvLinkedRelations.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvLinkedRelations_CellFormatting);
            // 
            // relatedCustomerNameDataGridViewTextBoxColumn
            // 
            this.relatedCustomerNameDataGridViewTextBoxColumn.DataPropertyName = "RelatedCustomerName";
            this.relatedCustomerNameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.relatedCustomerNameDataGridViewTextBoxColumn.Name = "relatedCustomerNameDataGridViewTextBoxColumn";
            this.relatedCustomerNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // relationshipTypeNameDataGridViewTextBoxColumn
            // 
            this.relationshipTypeNameDataGridViewTextBoxColumn.DataPropertyName = "RelationshipTypeName";
            this.relationshipTypeNameDataGridViewTextBoxColumn.HeaderText = "Relationship";
            this.relationshipTypeNameDataGridViewTextBoxColumn.Name = "relationshipTypeNameDataGridViewTextBoxColumn";
            this.relationshipTypeNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dOBDataGridViewTextBoxColumn
            // 
            this.dOBDataGridViewTextBoxColumn.DataPropertyName = "DOB";
            this.dOBDataGridViewTextBoxColumn.HeaderText = "DOB";
            this.dOBDataGridViewTextBoxColumn.Name = "dOBDataGridViewTextBoxColumn";
            this.dOBDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // contactPhoneDataGridViewTextBoxColumn
            // 
            this.contactPhoneDataGridViewTextBoxColumn.DataPropertyName = "ContactPhone";
            this.contactPhoneDataGridViewTextBoxColumn.HeaderText = "Phone Num";
            this.contactPhoneDataGridViewTextBoxColumn.Name = "contactPhoneDataGridViewTextBoxColumn";
            this.contactPhoneDataGridViewTextBoxColumn.ReadOnly = true;
            this.contactPhoneDataGridViewTextBoxColumn.Visible = false;
            // 
            // emailAddressDataGridViewTextBoxColumn
            // 
            this.emailAddressDataGridViewTextBoxColumn.DataPropertyName = "EmailAddress";
            this.emailAddressDataGridViewTextBoxColumn.HeaderText = "Email";
            this.emailAddressDataGridViewTextBoxColumn.Name = "emailAddressDataGridViewTextBoxColumn";
            this.emailAddressDataGridViewTextBoxColumn.ReadOnly = true;
            this.emailAddressDataGridViewTextBoxColumn.Visible = false;
            // 
            // uniqueIdentifierDataGridViewTextBoxColumn
            // 
            this.uniqueIdentifierDataGridViewTextBoxColumn.DataPropertyName = "UniqueIdentifier";
            this.uniqueIdentifierDataGridViewTextBoxColumn.HeaderText = "Unique Id";
            this.uniqueIdentifierDataGridViewTextBoxColumn.Name = "uniqueIdentifierDataGridViewTextBoxColumn";
            this.uniqueIdentifierDataGridViewTextBoxColumn.ReadOnly = true;
            this.uniqueIdentifierDataGridViewTextBoxColumn.Visible = false;
            // 
            // genderDataGridViewTextBoxColumn
            // 
            this.genderDataGridViewTextBoxColumn.DataPropertyName = "Gender";
            this.genderDataGridViewTextBoxColumn.HeaderText = "Gender";
            this.genderDataGridViewTextBoxColumn.Name = "genderDataGridViewTextBoxColumn";
            this.genderDataGridViewTextBoxColumn.ReadOnly = true;
            this.genderDataGridViewTextBoxColumn.Visible = false;
            // 
            // address1DataGridViewTextBoxColumn
            // 
            this.address1DataGridViewTextBoxColumn.DataPropertyName = "Address1";
            this.address1DataGridViewTextBoxColumn.HeaderText = "Address1";
            this.address1DataGridViewTextBoxColumn.Name = "address1DataGridViewTextBoxColumn";
            this.address1DataGridViewTextBoxColumn.ReadOnly = true;
            this.address1DataGridViewTextBoxColumn.Visible = false;
            // 
            // address2DataGridViewTextBoxColumn
            // 
            this.address2DataGridViewTextBoxColumn.DataPropertyName = "Address2";
            this.address2DataGridViewTextBoxColumn.HeaderText = "Address2";
            this.address2DataGridViewTextBoxColumn.Name = "address2DataGridViewTextBoxColumn";
            this.address2DataGridViewTextBoxColumn.ReadOnly = true;
            this.address2DataGridViewTextBoxColumn.Visible = false;
            // 
            // address3DataGridViewTextBoxColumn
            // 
            this.address3DataGridViewTextBoxColumn.DataPropertyName = "Address3";
            this.address3DataGridViewTextBoxColumn.HeaderText = "Address3";
            this.address3DataGridViewTextBoxColumn.Name = "address3DataGridViewTextBoxColumn";
            this.address3DataGridViewTextBoxColumn.ReadOnly = true;
            this.address3DataGridViewTextBoxColumn.Visible = false;
            // 
            // cityDataGridViewTextBoxColumn
            // 
            this.cityDataGridViewTextBoxColumn.DataPropertyName = "City";
            this.cityDataGridViewTextBoxColumn.HeaderText = "City";
            this.cityDataGridViewTextBoxColumn.Name = "cityDataGridViewTextBoxColumn";
            this.cityDataGridViewTextBoxColumn.ReadOnly = true;
            this.cityDataGridViewTextBoxColumn.Visible = false;
            // 
            // stateDataGridViewTextBoxColumn
            // 
            this.stateDataGridViewTextBoxColumn.DataPropertyName = "State";
            this.stateDataGridViewTextBoxColumn.HeaderText = "State";
            this.stateDataGridViewTextBoxColumn.Name = "stateDataGridViewTextBoxColumn";
            this.stateDataGridViewTextBoxColumn.ReadOnly = true;
            this.stateDataGridViewTextBoxColumn.Visible = false;
            // 
            // countryDataGridViewTextBoxColumn
            // 
            this.countryDataGridViewTextBoxColumn.DataPropertyName = "Country";
            this.countryDataGridViewTextBoxColumn.HeaderText = "Country";
            this.countryDataGridViewTextBoxColumn.Name = "countryDataGridViewTextBoxColumn";
            this.countryDataGridViewTextBoxColumn.ReadOnly = true;
            this.countryDataGridViewTextBoxColumn.Visible = false;
            // 
            // postalCodeDataGridViewTextBoxColumn
            // 
            this.postalCodeDataGridViewTextBoxColumn.DataPropertyName = "PostalCode";
            this.postalCodeDataGridViewTextBoxColumn.HeaderText = "Postal Code";
            this.postalCodeDataGridViewTextBoxColumn.Name = "postalCodeDataGridViewTextBoxColumn";
            this.postalCodeDataGridViewTextBoxColumn.ReadOnly = true;
            this.postalCodeDataGridViewTextBoxColumn.Visible = false;
            // 
            // anniversaryDataGridViewTextBoxColumn
            // 
            this.anniversaryDataGridViewTextBoxColumn.DataPropertyName = "Anniversary";
            this.anniversaryDataGridViewTextBoxColumn.HeaderText = "Anniversary";
            this.anniversaryDataGridViewTextBoxColumn.Name = "anniversaryDataGridViewTextBoxColumn";
            this.anniversaryDataGridViewTextBoxColumn.ReadOnly = true;
            this.anniversaryDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastNameDataGridViewTextBoxColumn
            // 
            this.lastNameDataGridViewTextBoxColumn.DataPropertyName = "LastName";
            this.lastNameDataGridViewTextBoxColumn.HeaderText = "LastName";
            this.lastNameDataGridViewTextBoxColumn.Name = "lastNameDataGridViewTextBoxColumn";
            this.lastNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // titleDataGridViewTextBoxColumn
            // 
            this.titleDataGridViewTextBoxColumn.DataPropertyName = "Title";
            this.titleDataGridViewTextBoxColumn.HeaderText = "Title";
            this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
            this.titleDataGridViewTextBoxColumn.ReadOnly = true;
            this.titleDataGridViewTextBoxColumn.Visible = false;
            // 
            // membershipIdDataGridViewTextBoxColumn
            // 
            this.membershipIdDataGridViewTextBoxColumn.DataPropertyName = "MembershipId";
            this.membershipIdDataGridViewTextBoxColumn.HeaderText = "Membership Id";
            this.membershipIdDataGridViewTextBoxColumn.Name = "membershipIdDataGridViewTextBoxColumn";
            this.membershipIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.membershipIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // middleNameDataGridViewTextBoxColumn
            // 
            this.middleNameDataGridViewTextBoxColumn.DataPropertyName = "MiddleName";
            this.middleNameDataGridViewTextBoxColumn.HeaderText = "Middle Name";
            this.middleNameDataGridViewTextBoxColumn.Name = "middleNameDataGridViewTextBoxColumn";
            this.middleNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.middleNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // parentCustomerIdDataGridViewTextBoxColumn
            // 
            this.parentCustomerIdDataGridViewTextBoxColumn.DataPropertyName = "ParentCustomerId";
            this.parentCustomerIdDataGridViewTextBoxColumn.HeaderText = "Parent Customer id";
            this.parentCustomerIdDataGridViewTextBoxColumn.Name = "parentCustomerIdDataGridViewTextBoxColumn";
            this.parentCustomerIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.parentCustomerIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // relatedCustomerIdDataGridViewTextBoxColumn
            // 
            this.relatedCustomerIdDataGridViewTextBoxColumn.DataPropertyName = "RelatedCustomerId";
            this.relatedCustomerIdDataGridViewTextBoxColumn.HeaderText = "Related Customer id";
            this.relatedCustomerIdDataGridViewTextBoxColumn.Name = "relatedCustomerIdDataGridViewTextBoxColumn";
            this.relatedCustomerIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.relatedCustomerIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // optInPromotionsModeDataGridViewTextBoxColumn
            // 
            this.optInPromotionsModeDataGridViewTextBoxColumn.DataPropertyName = "OptInPromotionsMode";
            this.optInPromotionsModeDataGridViewTextBoxColumn.HeaderText = "Promotion mode";
            this.optInPromotionsModeDataGridViewTextBoxColumn.Name = "optInPromotionsModeDataGridViewTextBoxColumn";
            this.optInPromotionsModeDataGridViewTextBoxColumn.ReadOnly = true;
            this.optInPromotionsModeDataGridViewTextBoxColumn.Visible = false;
            // 
            // photoURLDataGridViewTextBoxColumn
            // 
            this.photoURLDataGridViewTextBoxColumn.DataPropertyName = "PhotoURL";
            this.photoURLDataGridViewTextBoxColumn.HeaderText = "Customer Photo";
            this.photoURLDataGridViewTextBoxColumn.Name = "photoURLDataGridViewTextBoxColumn";
            this.photoURLDataGridViewTextBoxColumn.ReadOnly = true;
            this.photoURLDataGridViewTextBoxColumn.Visible = false;
            // 
            // customerRelationshipTypeIdDataGridViewTextBoxColumn
            // 
            this.customerRelationshipTypeIdDataGridViewTextBoxColumn.DataPropertyName = "CustomerRelationshipTypeId";
            this.customerRelationshipTypeIdDataGridViewTextBoxColumn.HeaderText = "Relationship Type Id";
            this.customerRelationshipTypeIdDataGridViewTextBoxColumn.Name = "customerRelationshipTypeIdDataGridViewTextBoxColumn";
            this.customerRelationshipTypeIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerRelationshipTypeIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // addressTypeDataGridViewTextBoxColumn
            // 
            this.addressTypeDataGridViewTextBoxColumn.DataPropertyName = "AddressType";
            this.addressTypeDataGridViewTextBoxColumn.HeaderText = "Address Type";
            this.addressTypeDataGridViewTextBoxColumn.Name = "addressTypeDataGridViewTextBoxColumn";
            this.addressTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.addressTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // customerTypeDataGridViewTextBoxColumn
            // 
            this.customerTypeDataGridViewTextBoxColumn.DataPropertyName = "CustomerType";
            this.customerTypeDataGridViewTextBoxColumn.HeaderText = "Type";
            this.customerTypeDataGridViewTextBoxColumn.Name = "customerTypeDataGridViewTextBoxColumn";
            this.customerTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // customCustomerDTOBindingSource
            // 
            this.customCustomerDTOBindingSource.DataSource = typeof(Semnox.Parafait.Customer.CustomCustomerDTO);
            // 
            // lblLinkedRelations
            // 
            this.lblLinkedRelations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLinkedRelations.BackColor = System.Drawing.Color.Transparent;
            this.lblLinkedRelations.Font = new System.Drawing.Font("Gotham Rounded Bold", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLinkedRelations.ForeColor = System.Drawing.Color.White;
            this.lblLinkedRelations.Location = new System.Drawing.Point(3, 0);
            this.lblLinkedRelations.Name = "lblLinkedRelations";
            this.lblLinkedRelations.Size = new System.Drawing.Size(895, 57);
            this.lblLinkedRelations.TabIndex = 35;
            this.lblLinkedRelations.Text = "Linked Relations";
            this.lblLinkedRelations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "AddressType";
            this.dataGridViewTextBoxColumn1.HeaderText = "Address Type";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 325;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "CustomerType";
            this.dataGridViewTextBoxColumn2.HeaderText = "Type";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            this.dataGridViewTextBoxColumn2.Width = 326;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "DOB";
            this.dataGridViewTextBoxColumn3.HeaderText = "DOB";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 325;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "ContactPhone";
            this.dataGridViewTextBoxColumn4.HeaderText = "Phone Num";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "EmailAddress";
            this.dataGridViewTextBoxColumn5.HeaderText = "Email";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "UniqueIdentifier";
            this.dataGridViewTextBoxColumn6.HeaderText = "Unique Id";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "Gender";
            this.dataGridViewTextBoxColumn7.HeaderText = "Gender";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "Address1";
            this.dataGridViewTextBoxColumn8.HeaderText = "Address1";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Address2";
            this.dataGridViewTextBoxColumn9.HeaderText = "Address2";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "Address3";
            this.dataGridViewTextBoxColumn10.HeaderText = "Address3";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Visible = false;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "City";
            this.dataGridViewTextBoxColumn11.HeaderText = "City";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Visible = false;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "State";
            this.dataGridViewTextBoxColumn12.HeaderText = "State";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Visible = false;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "Country";
            this.dataGridViewTextBoxColumn13.HeaderText = "Country";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Visible = false;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "PostalCode";
            this.dataGridViewTextBoxColumn14.HeaderText = "Postal Code";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Visible = false;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "Anniversary";
            this.dataGridViewTextBoxColumn15.HeaderText = "Anniversary";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            this.dataGridViewTextBoxColumn15.Visible = false;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "LastName";
            this.dataGridViewTextBoxColumn16.HeaderText = "LastName";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            this.dataGridViewTextBoxColumn16.Visible = false;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.DataPropertyName = "Title";
            this.dataGridViewTextBoxColumn17.HeaderText = "Title";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.ReadOnly = true;
            this.dataGridViewTextBoxColumn17.Visible = false;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.DataPropertyName = "MembershipId";
            this.dataGridViewTextBoxColumn18.HeaderText = "Membership Id";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.ReadOnly = true;
            this.dataGridViewTextBoxColumn18.Visible = false;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.DataPropertyName = "MiddleName";
            this.dataGridViewTextBoxColumn19.HeaderText = "Middle Name";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.ReadOnly = true;
            this.dataGridViewTextBoxColumn19.Visible = false;
            // 
            // dataGridViewTextBoxColumn20
            // 
            this.dataGridViewTextBoxColumn20.DataPropertyName = "ParentCustomerId";
            this.dataGridViewTextBoxColumn20.HeaderText = "Parent Customer id";
            this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
            this.dataGridViewTextBoxColumn20.ReadOnly = true;
            this.dataGridViewTextBoxColumn20.Visible = false;
            // 
            // dataGridViewTextBoxColumn21
            // 
            this.dataGridViewTextBoxColumn21.DataPropertyName = "RelatedCustomerId";
            this.dataGridViewTextBoxColumn21.HeaderText = "Related Customer id";
            this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
            this.dataGridViewTextBoxColumn21.ReadOnly = true;
            this.dataGridViewTextBoxColumn21.Visible = false;
            // 
            // dataGridViewTextBoxColumn22
            // 
            this.dataGridViewTextBoxColumn22.DataPropertyName = "OptInPromotionsMode";
            this.dataGridViewTextBoxColumn22.HeaderText = "Promotion mode";
            this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
            this.dataGridViewTextBoxColumn22.ReadOnly = true;
            this.dataGridViewTextBoxColumn22.Visible = false;
            // 
            // dataGridViewTextBoxColumn23
            // 
            this.dataGridViewTextBoxColumn23.DataPropertyName = "PhotoURL";
            this.dataGridViewTextBoxColumn23.HeaderText = "Customer Photo";
            this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
            this.dataGridViewTextBoxColumn23.ReadOnly = true;
            this.dataGridViewTextBoxColumn23.Visible = false;
            // 
            // dataGridViewTextBoxColumn24
            // 
            this.dataGridViewTextBoxColumn24.DataPropertyName = "CustomerRelationshipTypeId";
            this.dataGridViewTextBoxColumn24.HeaderText = "Relationship Type Id";
            this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
            this.dataGridViewTextBoxColumn24.ReadOnly = true;
            this.dataGridViewTextBoxColumn24.Visible = false;
            // 
            // dataGridViewTextBoxColumn25
            // 
            this.dataGridViewTextBoxColumn25.DataPropertyName = "AddressType";
            this.dataGridViewTextBoxColumn25.HeaderText = "Address Type";
            this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
            this.dataGridViewTextBoxColumn25.ReadOnly = true;
            this.dataGridViewTextBoxColumn25.Visible = false;
            // 
            // dataGridViewTextBoxColumn26
            // 
            this.dataGridViewTextBoxColumn26.DataPropertyName = "CustomerType";
            this.dataGridViewTextBoxColumn26.HeaderText = "Type";
            this.dataGridViewTextBoxColumn26.Name = "dataGridViewTextBoxColumn26";
            this.dataGridViewTextBoxColumn26.ReadOnly = true;
            this.dataGridViewTextBoxColumn26.Visible = false;
            // 
            // bigVerticalScrollCustomer
            // 
            this.bigVerticalScrollCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bigVerticalScrollCustomer.AutoHide = true;
            this.bigVerticalScrollCustomer.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollCustomer.DataGridView = null;
            this.bigVerticalScrollCustomer.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollCustomer.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollCustomer.Location = new System.Drawing.Point(1520, 166);
            this.bigVerticalScrollCustomer.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.bigVerticalScrollCustomer.Name = "bigVerticalScrollCustomer";
            this.bigVerticalScrollCustomer.ScrollableControl = this.flpCustomerMain;
            this.bigVerticalScrollCustomer.ScrollViewer = null;
            this.bigVerticalScrollCustomer.Size = new System.Drawing.Size(63, 413);
            this.bigVerticalScrollCustomer.TabIndex = 20017;
            this.bigVerticalScrollCustomer.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollCustomer.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollCustomer.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollCustomer.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // Customer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.bigVerticalScrollCustomer);
            this.Controls.Add(this.lblTimeRemaining);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblRegistrationMessage);
            this.Controls.Add(this.flpCustomerMain);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.textBoxMessageLine);
            this.Controls.Add(this.PanelLinkedRelations);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Customer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Details";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Customer_FormClosed);
            this.Load += new System.EventHandler(this.Customer_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormOnKeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormOnKeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormOnKeyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FormOnMouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormOnMouseClick);
            this.Controls.SetChildIndex(this.PanelLinkedRelations, 0);
            this.Controls.SetChildIndex(this.textBoxMessageLine, 0);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.flpCustomerMain, 0);
            this.Controls.SetChildIndex(this.lblRegistrationMessage, 0);
            this.Controls.SetChildIndex(this.button1, 0);
            this.Controls.SetChildIndex(this.lblTimeRemaining, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.bigVerticalScrollCustomer, 0);
            this.flpCustomerMain.ResumeLayout(false);
            this.flpCustomerMain.PerformLayout();
            this.panelCardNumber.ResumeLayout(false);
            this.panelCardNumber.PerformLayout();
            this.panelTitle.ResumeLayout(false);
            this.panelFirstName.ResumeLayout(false);
            this.panelFirstName.PerformLayout();
            this.panelLastName.ResumeLayout(false);
            this.panelLastName.PerformLayout();
            this.lblBirthDate.ResumeLayout(false);
            this.panelBirthDate.ResumeLayout(false);
            this.panelBirthDate.PerformLayout();
            this.panelContactPhone1.ResumeLayout(false);
            this.panelContactPhone1.PerformLayout();
            this.panelEmail.ResumeLayout(false);
            this.panelEmail.PerformLayout();
            this.panelUniqueId.ResumeLayout(false);
            this.panelUniqueId.PerformLayout();
            this.panelAddress1.ResumeLayout(false);
            this.panelAddress1.PerformLayout();
            this.panelAddress2.ResumeLayout(false);
            this.panelAddress2.PerformLayout();
            this.panelCity.ResumeLayout(false);
            this.panelCity.PerformLayout();
            this.panelState.ResumeLayout(false);
            this.panelPin.ResumeLayout(false);
            this.panelPin.PerformLayout();
            this.panelCountry.ResumeLayout(false);
            this.panelGender.ResumeLayout(false);
            this.panelOptInPromotionsMode.ResumeLayout(false);
            this.panelAnniversary.ResumeLayout(false);
            this.panelAnniversary.PerformLayout();
            this.pnlOptinPromotions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCapture)).EndInit();
            this.pnlTermsandConditions.ResumeLayout(false);
            this.pnlWhatsAppOptOut.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.PanelLinkedRelations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLinkedRelations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customCustomerDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        } 

        #endregion

        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox textBoxPin;
        private System.Windows.Forms.Label lblPostalCode;
        private System.Windows.Forms.Button buttonCustomerSave;
        private System.Windows.Forms.ComboBox comboBoxGender;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.TextBox textBoxContactPhone1;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.TextBox textBoxCity;
        private System.Windows.Forms.TextBox textBoxAddress2;
        private System.Windows.Forms.TextBox textBoxAddress1;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.Label lblAddress2;
        private System.Windows.Forms.Label lblAddres1;
        private System.Windows.Forms.Label lblFirstname;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Button textBoxMessageLine;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.Label lblBirthDateFormat;
        private System.Windows.Forms.Label lblAnniversaryDateFormat;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.FlowLayoutPanel flpCustomerMain;
        private System.Windows.Forms.FlowLayoutPanel flpCustomAttributes;
        private System.Windows.Forms.PictureBox pbCapture;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblPhoto;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label lblAnniversary;
        private System.Windows.Forms.MaskedTextBox txtAnniversary;
        private System.Windows.Forms.Label lblUniqueId;
        private System.Windows.Forms.TextBox txtUniqueId;
        private System.Windows.Forms.Label lblRegistrationMessage;
        private System.Windows.Forms.Label lblStateCombo;
        private System.Windows.Forms.ComboBox cmbState;
        private System.Windows.Forms.Button lblTimeRemaining;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox cmbTitle;
        private System.Windows.Forms.Panel panelCardNumber;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Panel panelFirstName;
        private System.Windows.Forms.Panel panelLastName;
        private System.Windows.Forms.Panel panelAddress1;
        private System.Windows.Forms.Panel panelAddress2;
        private System.Windows.Forms.Panel panelCity;
        private System.Windows.Forms.Panel panelState;
        private System.Windows.Forms.Panel panelPin;
        private System.Windows.Forms.Panel panelCountry;
        private System.Windows.Forms.Panel panelEmail;
        private System.Windows.Forms.Panel panelContactPhone1;
        private System.Windows.Forms.Panel panelBirthDate;
        private System.Windows.Forms.Panel panelGender;
        private System.Windows.Forms.Panel panelAnniversary;
        private System.Windows.Forms.Panel panelUniqueId;
        //private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbCountry;
        private System.Windows.Forms.Label lblPromotionMode;
        private System.Windows.Forms.Panel panelOptInPromotionsMode;
        private System.Windows.Forms.ComboBox cmbPromotionMode;
        private System.Windows.Forms.Panel pnlOptinPromotions;
        private System.Windows.Forms.Button btnOpinPromotions;
        private System.Windows.Forms.Label lblOptinPromotions;
        private System.Windows.Forms.Panel pnlTermsandConditions;
        private System.Windows.Forms.Button btnTermsandConditions;
        private System.Windows.Forms.Label lblTermsAndConditions;
        private System.Windows.Forms.Panel pnlWhatsAppOptOut;
        private System.Windows.Forms.Button btnWhatsAppOptOut;
        private System.Windows.Forms.Label lblWhatsAppOptOut;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollCustomer;
        private System.Windows.Forms.Button btnAddRelation;
        private System.Windows.Forms.Panel PanelLinkedRelations;
        private System.Windows.Forms.Label lblLinkedRelations;
        private System.Windows.Forms.DataGridView dgvLinkedRelations;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollLinkedRelations;
        private System.Windows.Forms.BindingSource customCustomerDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn relatedCustomerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn relationshipTypeNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dOBDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn contactPhoneDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uniqueIdentifierDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn genderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn address1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn address2DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn address3DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn countryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn postalCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn anniversaryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn membershipIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn middleNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn addressTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn parentCustomerIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn relatedCustomerIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn optInPromotionsModeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn photoURLDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerRelationshipTypeIdDataGridViewTextBoxColumn;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn26;
        private MaskedTextBox txtBirthDate;
        private Label Label;
    }
}
