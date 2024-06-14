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
            this.flpCustomerMain = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.cmbTitle = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxAddress1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxAddress2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxCity = new System.Windows.Forms.TextBox();
            this.lblState = new System.Windows.Forms.Label();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.lblPostaCode = new System.Windows.Forms.Label();
            this.textBoxPin = new System.Windows.Forms.TextBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.cmbCountry = new System.Windows.Forms.ComboBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.textBoxContactPhone1 = new System.Windows.Forms.TextBox();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.txtBirthDate = new System.Windows.Forms.TextBox();
            this.lblAnniversary = new System.Windows.Forms.Label();
            this.textBoxAnniversaryDate = new System.Windows.Forms.TextBox();
            this.lblUniqueId = new System.Windows.Forms.Label();
            this.txtUniqueId = new System.Windows.Forms.TextBox();
            this.lblComments = new System.Windows.Forms.Label();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.lblFBUserId = new System.Windows.Forms.Label();
            this.txtFBUserId = new System.Windows.Forms.TextBox();
            this.lblFBToken = new System.Windows.Forms.Label();
            this.txtFBAccessToken = new System.Windows.Forms.TextBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.comboBoxGender = new System.Windows.Forms.ComboBox();
            this.pnlOptinPromotions = new System.Windows.Forms.Panel();
            this.lblOptinPromotions = new System.Windows.Forms.Label();
            this.btnOptinPromotions = new System.Windows.Forms.Button();
            this.lblPromotionMode = new System.Windows.Forms.Label();
            this.pnlPromotionMode = new System.Windows.Forms.Panel();
            this.cmbPromotionMode = new System.Windows.Forms.ComboBox();
            this.flpCustomAttributes = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPhoto = new System.Windows.Forms.Label();
            this.pbCapture = new System.Windows.Forms.PictureBox();
            this.pnlTermsAndConditions = new System.Windows.Forms.Panel();
            this.lblTermsAndConditions = new System.Windows.Forms.Label();
            this.btnTermsAndCondition = new System.Windows.Forms.Button();
            this.lblRegistration = new System.Windows.Forms.Label();
            this.lblMessage1 = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.buttonCustomerSave = new System.Windows.Forms.Button();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.lblMessage2 = new System.Windows.Forms.Label();
            this.lblTimeRemaining = new System.Windows.Forms.Button();
            this.textBoxMessageLine = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.flpCustomerMain.SuspendLayout();
            this.pnlOptinPromotions.SuspendLayout();
            this.pnlPromotionMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCapture)).BeginInit();
            this.pnlTermsAndConditions.SuspendLayout();
            this.panelButtons.SuspendLayout();
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
            // flpCustomerMain
            // 
            this.flpCustomerMain.AutoScroll = true;
            this.flpCustomerMain.BackColor = System.Drawing.Color.Transparent;
            this.flpCustomerMain.Controls.Add(this.lblCardNumber);
            this.flpCustomerMain.Controls.Add(this.txtCardNumber);
            this.flpCustomerMain.Controls.Add(this.lblTitle);
            this.flpCustomerMain.Controls.Add(this.cmbTitle);
            this.flpCustomerMain.Controls.Add(this.label1);
            this.flpCustomerMain.Controls.Add(this.txtFirstName);
            this.flpCustomerMain.Controls.Add(this.label4);
            this.flpCustomerMain.Controls.Add(this.txtLastName);
            this.flpCustomerMain.Controls.Add(this.label2);
            this.flpCustomerMain.Controls.Add(this.textBoxAddress1);
            this.flpCustomerMain.Controls.Add(this.label3);
            this.flpCustomerMain.Controls.Add(this.textBoxAddress2);
            this.flpCustomerMain.Controls.Add(this.label9);
            this.flpCustomerMain.Controls.Add(this.textBoxCity);
            this.flpCustomerMain.Controls.Add(this.lblState);
            this.flpCustomerMain.Controls.Add(this.cmbState);
            this.flpCustomerMain.Controls.Add(this.lblPostaCode);
            this.flpCustomerMain.Controls.Add(this.textBoxPin);
            this.flpCustomerMain.Controls.Add(this.lblCountry);
            this.flpCustomerMain.Controls.Add(this.cmbCountry);
            this.flpCustomerMain.Controls.Add(this.lblEmail);
            this.flpCustomerMain.Controls.Add(this.textBoxEmail);
            this.flpCustomerMain.Controls.Add(this.lblPhone);
            this.flpCustomerMain.Controls.Add(this.textBoxContactPhone1);
            this.flpCustomerMain.Controls.Add(this.lblBirthDate);
            this.flpCustomerMain.Controls.Add(this.txtBirthDate);
            this.flpCustomerMain.Controls.Add(this.lblAnniversary);
            this.flpCustomerMain.Controls.Add(this.textBoxAnniversaryDate);
            this.flpCustomerMain.Controls.Add(this.lblUniqueId);
            this.flpCustomerMain.Controls.Add(this.txtUniqueId);
            this.flpCustomerMain.Controls.Add(this.lblComments);
            this.flpCustomerMain.Controls.Add(this.textBoxNotes);
            this.flpCustomerMain.Controls.Add(this.lblFBUserId);
            this.flpCustomerMain.Controls.Add(this.txtFBUserId);
            this.flpCustomerMain.Controls.Add(this.lblFBToken);
            this.flpCustomerMain.Controls.Add(this.txtFBAccessToken);
            this.flpCustomerMain.Controls.Add(this.lblGender);
            this.flpCustomerMain.Controls.Add(this.comboBoxGender);
            this.flpCustomerMain.Controls.Add(this.pnlOptinPromotions);
            this.flpCustomerMain.Controls.Add(this.lblPromotionMode);
            this.flpCustomerMain.Controls.Add(this.pnlPromotionMode);
            this.flpCustomerMain.Controls.Add(this.flpCustomAttributes);
            this.flpCustomerMain.Controls.Add(this.lblPhoto);
            this.flpCustomerMain.Controls.Add(this.pbCapture);
            this.flpCustomerMain.Controls.Add(this.pnlTermsAndConditions);
            this.flpCustomerMain.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpCustomerMain.Location = new System.Drawing.Point(6, 252);
            this.flpCustomerMain.Name = "flpCustomerMain";
            this.flpCustomerMain.Size = new System.Drawing.Size(1065, 420);
            this.flpCustomerMain.TabIndex = 1601;
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.ForeColor = System.Drawing.Color.White;
            this.lblCardNumber.Location = new System.Drawing.Point(3, 0);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(200, 46);
            this.lblCardNumber.TabIndex = 1;
            this.lblCardNumber.Text = "Card #:";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.BackColor = System.Drawing.Color.LightGray;
            this.txtCardNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCardNumber.Enabled = false;
            this.txtCardNumber.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.ForeColor = System.Drawing.Color.Black;
            this.txtCardNumber.Location = new System.Drawing.Point(209, 3);
            this.txtCardNumber.Multiline = true;
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.ReadOnly = true;
            this.txtCardNumber.Size = new System.Drawing.Size(310, 49);
            this.txtCardNumber.TabIndex = 2;
            this.txtCardNumber.Text = "543F4A34";
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(525, 6);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 46);
            this.lblTitle.TabIndex = 1036;
            this.lblTitle.Text = "Title:";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbTitle
            // 
            this.cmbTitle.BackColor = System.Drawing.Color.White;
            this.cmbTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTitle.Font = new System.Drawing.Font("Verdana", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTitle.ForeColor = System.Drawing.Color.Black;
            this.cmbTitle.FormattingEnabled = true;
            this.cmbTitle.Location = new System.Drawing.Point(731, 6);
            this.cmbTitle.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbTitle.Name = "cmbTitle";
            this.cmbTitle.Size = new System.Drawing.Size(310, 52);
            this.cmbTitle.TabIndex = 1037;
            this.cmbTitle.SelectedIndexChanged += new System.EventHandler(this.cmbTitle_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 70);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 46);
            this.label1.TabIndex = 3;
            this.label1.Text = "First Name:*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFirstName
            // 
            this.txtFirstName.BackColor = System.Drawing.Color.White;
            this.txtFirstName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFirstName.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.ForeColor = System.Drawing.Color.Black;
            this.txtFirstName.Location = new System.Drawing.Point(209, 70);
            this.txtFirstName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(310, 49);
            this.txtFirstName.TabIndex = 4;
            this.txtFirstName.Text = "SANJEEV";
            this.txtFirstName.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(525, 70);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 46);
            this.label4.TabIndex = 5;
            this.label4.Text = "Last Name:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLastName
            // 
            this.txtLastName.BackColor = System.Drawing.Color.White;
            this.txtLastName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLastName.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.ForeColor = System.Drawing.Color.Black;
            this.txtLastName.Location = new System.Drawing.Point(731, 70);
            this.txtLastName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(310, 49);
            this.txtLastName.TabIndex = 6;
            this.txtLastName.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 131);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 46);
            this.label2.TabIndex = 11;
            this.label2.Text = "Address 1:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxAddress1
            // 
            this.textBoxAddress1.BackColor = System.Drawing.Color.White;
            this.textBoxAddress1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxAddress1.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAddress1.ForeColor = System.Drawing.Color.Black;
            this.textBoxAddress1.Location = new System.Drawing.Point(209, 131);
            this.textBoxAddress1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxAddress1.Name = "textBoxAddress1";
            this.textBoxAddress1.Size = new System.Drawing.Size(310, 49);
            this.textBoxAddress1.TabIndex = 12;
            this.textBoxAddress1.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(525, 131);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 46);
            this.label3.TabIndex = 13;
            this.label3.Text = "Address 2:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxAddress2
            // 
            this.textBoxAddress2.AcceptsReturn = true;
            this.textBoxAddress2.BackColor = System.Drawing.Color.White;
            this.textBoxAddress2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxAddress2.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAddress2.ForeColor = System.Drawing.Color.Black;
            this.textBoxAddress2.Location = new System.Drawing.Point(731, 131);
            this.textBoxAddress2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxAddress2.Name = "textBoxAddress2";
            this.textBoxAddress2.Size = new System.Drawing.Size(310, 49);
            this.textBoxAddress2.TabIndex = 14;
            this.textBoxAddress2.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(3, 192);
            this.label9.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(200, 46);
            this.label9.TabIndex = 15;
            this.label9.Text = "City:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCity
            // 
            this.textBoxCity.BackColor = System.Drawing.Color.White;
            this.textBoxCity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCity.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCity.ForeColor = System.Drawing.Color.Black;
            this.textBoxCity.Location = new System.Drawing.Point(209, 192);
            this.textBoxCity.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxCity.Name = "textBoxCity";
            this.textBoxCity.Size = new System.Drawing.Size(310, 49);
            this.textBoxCity.TabIndex = 16;
            this.textBoxCity.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // lblState
            // 
            this.lblState.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblState.ForeColor = System.Drawing.Color.White;
            this.lblState.Location = new System.Drawing.Point(525, 192);
            this.lblState.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(200, 46);
            this.lblState.TabIndex = 19;
            this.lblState.Text = "State:";
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbState
            // 
            this.cmbState.BackColor = System.Drawing.Color.White;
            this.cmbState.DropDownHeight = 1040;
            this.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbState.Font = new System.Drawing.Font("Verdana", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbState.ForeColor = System.Drawing.Color.Black;
            this.cmbState.FormattingEnabled = true;
            this.cmbState.IntegralHeight = false;
            this.cmbState.Location = new System.Drawing.Point(731, 192);
            this.cmbState.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(310, 52);
            this.cmbState.TabIndex = 20;
            // 
            // lblPostaCode
            // 
            this.lblPostaCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPostaCode.ForeColor = System.Drawing.Color.White;
            this.lblPostaCode.Location = new System.Drawing.Point(3, 256);
            this.lblPostaCode.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblPostaCode.Name = "lblPostaCode";
            this.lblPostaCode.Size = new System.Drawing.Size(201, 46);
            this.lblPostaCode.TabIndex = 23;
            this.lblPostaCode.Text = "Postal Code:";
            this.lblPostaCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPin
            // 
            this.textBoxPin.BackColor = System.Drawing.Color.White;
            this.textBoxPin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPin.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPin.ForeColor = System.Drawing.Color.Black;
            this.textBoxPin.Location = new System.Drawing.Point(210, 256);
            this.textBoxPin.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxPin.Name = "textBoxPin";
            this.textBoxPin.Size = new System.Drawing.Size(310, 49);
            this.textBoxPin.TabIndex = 24;
            this.textBoxPin.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // lblCountry
            // 
            this.lblCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountry.ForeColor = System.Drawing.Color.White;
            this.lblCountry.Location = new System.Drawing.Point(526, 256);
            this.lblCountry.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(200, 46);
            this.lblCountry.TabIndex = 21;
            this.lblCountry.Text = "Country:";
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbCountry
            // 
            this.cmbCountry.BackColor = System.Drawing.Color.White;
            this.cmbCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCountry.Font = new System.Drawing.Font("Verdana", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCountry.ForeColor = System.Drawing.Color.Black;
            this.cmbCountry.FormattingEnabled = true;
            this.cmbCountry.Items.AddRange(new object[] {
            "Female",
            "Male",
            "Not Set"});
            this.cmbCountry.Location = new System.Drawing.Point(732, 256);
            this.cmbCountry.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbCountry.Name = "cmbCountry";
            this.cmbCountry.Size = new System.Drawing.Size(310, 52);
            this.cmbCountry.TabIndex = 20016;
            // 
            // lblEmail
            // 
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.ForeColor = System.Drawing.Color.White;
            this.lblEmail.Location = new System.Drawing.Point(3, 320);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(200, 46);
            this.lblEmail.TabIndex = 7;
            this.lblEmail.Text = "E-Mail:";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.BackColor = System.Drawing.Color.White;
            this.textBoxEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxEmail.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEmail.ForeColor = System.Drawing.Color.Black;
            this.textBoxEmail.Location = new System.Drawing.Point(209, 320);
            this.textBoxEmail.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(310, 49);
            this.textBoxEmail.TabIndex = 8;
            this.textBoxEmail.Text = "this@thatgrouse.com";
            this.textBoxEmail.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // lblPhone
            // 
            this.lblPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhone.ForeColor = System.Drawing.Color.White;
            this.lblPhone.Location = new System.Drawing.Point(525, 320);
            this.lblPhone.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(200, 46);
            this.lblPhone.TabIndex = 9;
            this.lblPhone.Text = "Phone:";
            this.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxContactPhone1
            // 
            this.textBoxContactPhone1.BackColor = System.Drawing.Color.White;
            this.textBoxContactPhone1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxContactPhone1.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxContactPhone1.ForeColor = System.Drawing.Color.Black;
            this.textBoxContactPhone1.Location = new System.Drawing.Point(731, 320);
            this.textBoxContactPhone1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxContactPhone1.Name = "textBoxContactPhone1";
            this.textBoxContactPhone1.Size = new System.Drawing.Size(310, 49);
            this.textBoxContactPhone1.TabIndex = 10;
            this.textBoxContactPhone1.Text = "9845085780";
            this.textBoxContactPhone1.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBoxContactPhone1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxContactPhone1_KeyPress);
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBirthDate.ForeColor = System.Drawing.Color.White;
            this.lblBirthDate.Location = new System.Drawing.Point(3, 381);
            this.lblBirthDate.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(200, 46);
            this.lblBirthDate.TabIndex = 25;
            this.lblBirthDate.Text = "Birth Date:";
            this.lblBirthDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBirthDate
            // 
            this.txtBirthDate.BackColor = System.Drawing.Color.White;
            this.txtBirthDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBirthDate.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBirthDate.ForeColor = System.Drawing.Color.Black;
            this.txtBirthDate.Location = new System.Drawing.Point(209, 381);
            this.txtBirthDate.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtBirthDate.Name = "txtBirthDate";
            this.txtBirthDate.Size = new System.Drawing.Size(310, 49);
            this.txtBirthDate.TabIndex = 26;
            this.txtBirthDate.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // lblAnniversary
            // 
            this.lblAnniversary.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnniversary.ForeColor = System.Drawing.Color.White;
            this.lblAnniversary.Location = new System.Drawing.Point(525, 381);
            this.lblAnniversary.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblAnniversary.Name = "lblAnniversary";
            this.lblAnniversary.Size = new System.Drawing.Size(200, 46);
            this.lblAnniversary.TabIndex = 27;
            this.lblAnniversary.Text = "Anniversary:";
            this.lblAnniversary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxAnniversaryDate
            // 
            this.textBoxAnniversaryDate.BackColor = System.Drawing.Color.White;
            this.textBoxAnniversaryDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxAnniversaryDate.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAnniversaryDate.ForeColor = System.Drawing.Color.Black;
            this.textBoxAnniversaryDate.Location = new System.Drawing.Point(731, 381);
            this.textBoxAnniversaryDate.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxAnniversaryDate.Name = "textBoxAnniversaryDate";
            this.textBoxAnniversaryDate.Size = new System.Drawing.Size(310, 49);
            this.textBoxAnniversaryDate.TabIndex = 28;
            this.textBoxAnniversaryDate.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // lblUniqueId
            // 
            this.lblUniqueId.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUniqueId.ForeColor = System.Drawing.Color.White;
            this.lblUniqueId.Location = new System.Drawing.Point(3, 442);
            this.lblUniqueId.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblUniqueId.Name = "lblUniqueId";
            this.lblUniqueId.Size = new System.Drawing.Size(200, 46);
            this.lblUniqueId.TabIndex = 29;
            this.lblUniqueId.Text = "Unique Id:";
            this.lblUniqueId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUniqueId
            // 
            this.txtUniqueId.BackColor = System.Drawing.Color.White;
            this.txtUniqueId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUniqueId.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUniqueId.ForeColor = System.Drawing.Color.Black;
            this.txtUniqueId.Location = new System.Drawing.Point(209, 442);
            this.txtUniqueId.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtUniqueId.Name = "txtUniqueId";
            this.txtUniqueId.Size = new System.Drawing.Size(310, 49);
            this.txtUniqueId.TabIndex = 30;
            this.txtUniqueId.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // lblComments
            // 
            this.lblComments.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComments.ForeColor = System.Drawing.Color.White;
            this.lblComments.Location = new System.Drawing.Point(525, 442);
            this.lblComments.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblComments.Name = "lblComments";
            this.lblComments.Size = new System.Drawing.Size(200, 46);
            this.lblComments.TabIndex = 31;
            this.lblComments.Text = "Comments:";
            this.lblComments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNotes
            // 
            this.textBoxNotes.AcceptsReturn = true;
            this.textBoxNotes.BackColor = System.Drawing.Color.White;
            this.textBoxNotes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxNotes.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNotes.ForeColor = System.Drawing.Color.Black;
            this.textBoxNotes.Location = new System.Drawing.Point(731, 442);
            this.textBoxNotes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.Size = new System.Drawing.Size(310, 49);
            this.textBoxNotes.TabIndex = 32;
            this.textBoxNotes.Text = "Nice";
            this.textBoxNotes.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // lblFBUserId
            // 
            this.lblFBUserId.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFBUserId.ForeColor = System.Drawing.Color.White;
            this.lblFBUserId.Location = new System.Drawing.Point(3, 503);
            this.lblFBUserId.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblFBUserId.Name = "lblFBUserId";
            this.lblFBUserId.Size = new System.Drawing.Size(200, 46);
            this.lblFBUserId.TabIndex = 33;
            this.lblFBUserId.Text = "FB UserId:";
            this.lblFBUserId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFBUserId
            // 
            this.txtFBUserId.BackColor = System.Drawing.Color.White;
            this.txtFBUserId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFBUserId.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFBUserId.ForeColor = System.Drawing.Color.Black;
            this.txtFBUserId.Location = new System.Drawing.Point(209, 503);
            this.txtFBUserId.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtFBUserId.Name = "txtFBUserId";
            this.txtFBUserId.Size = new System.Drawing.Size(310, 49);
            this.txtFBUserId.TabIndex = 34;
            this.txtFBUserId.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // lblFBToken
            // 
            this.lblFBToken.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFBToken.ForeColor = System.Drawing.Color.White;
            this.lblFBToken.Location = new System.Drawing.Point(525, 503);
            this.lblFBToken.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblFBToken.Name = "lblFBToken";
            this.lblFBToken.Size = new System.Drawing.Size(200, 46);
            this.lblFBToken.TabIndex = 35;
            this.lblFBToken.Text = "FB Token:";
            this.lblFBToken.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFBAccessToken
            // 
            this.txtFBAccessToken.BackColor = System.Drawing.Color.White;
            this.txtFBAccessToken.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFBAccessToken.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFBAccessToken.ForeColor = System.Drawing.Color.Black;
            this.txtFBAccessToken.Location = new System.Drawing.Point(731, 503);
            this.txtFBAccessToken.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtFBAccessToken.Name = "txtFBAccessToken";
            this.txtFBAccessToken.PasswordChar = '*';
            this.txtFBAccessToken.Size = new System.Drawing.Size(310, 49);
            this.txtFBAccessToken.TabIndex = 36;
            this.txtFBAccessToken.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // lblGender
            // 
            this.lblGender.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGender.ForeColor = System.Drawing.Color.White;
            this.lblGender.Location = new System.Drawing.Point(3, 564);
            this.lblGender.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(200, 46);
            this.lblGender.TabIndex = 37;
            this.lblGender.Text = "Gender:";
            this.lblGender.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxGender
            // 
            this.comboBoxGender.BackColor = System.Drawing.Color.White;
            this.comboBoxGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGender.Font = new System.Drawing.Font("Verdana", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxGender.ForeColor = System.Drawing.Color.Black;
            this.comboBoxGender.FormattingEnabled = true;
            this.comboBoxGender.Items.AddRange(new object[] {
            "Female",
            "Male",
            "Not Set"});
            this.comboBoxGender.Location = new System.Drawing.Point(209, 564);
            this.comboBoxGender.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.comboBoxGender.Name = "comboBoxGender";
            this.comboBoxGender.Size = new System.Drawing.Size(310, 52);
            this.comboBoxGender.TabIndex = 38;
            // 
            // pnlOptinPromotions
            // 
            this.pnlOptinPromotions.Controls.Add(this.lblOptinPromotions);
            this.pnlOptinPromotions.Controls.Add(this.btnOptinPromotions);
            this.pnlOptinPromotions.Location = new System.Drawing.Point(3, 625);
            this.pnlOptinPromotions.Name = "pnlOptinPromotions";
            this.pnlOptinPromotions.Size = new System.Drawing.Size(1038, 65);
            this.pnlOptinPromotions.TabIndex = 20020;
            this.pnlOptinPromotions.Tag = "false";
            this.pnlOptinPromotions.Click += new System.EventHandler(this.pnlOptinPromotions_Click);
            // 
            // lblOptinPromotions
            // 
            this.lblOptinPromotions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOptinPromotions.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOptinPromotions.ForeColor = System.Drawing.Color.White;
            this.lblOptinPromotions.Location = new System.Drawing.Point(73, 8);
            this.lblOptinPromotions.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblOptinPromotions.Name = "lblOptinPromotions";
            this.lblOptinPromotions.Size = new System.Drawing.Size(962, 46);
            this.lblOptinPromotions.TabIndex = 20020;
            this.lblOptinPromotions.Text = "Opt in Promotions";
            this.lblOptinPromotions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOptinPromotions.Click += new System.EventHandler(this.pnlOptinPromotions_Click);
            // 
            // btnOptinPromotions
            // 
            this.btnOptinPromotions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOptinPromotions.FlatAppearance.BorderSize = 0;
            this.btnOptinPromotions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOptinPromotions.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.btnOptinPromotions.ForeColor = System.Drawing.Color.White;
            this.btnOptinPromotions.Image = global::Parafait_Kiosk.Properties.Resources.checkbox_unchecked;
            this.btnOptinPromotions.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOptinPromotions.Location = new System.Drawing.Point(11, 4);
            this.btnOptinPromotions.Name = "btnOptinPromotions";
            this.btnOptinPromotions.Size = new System.Drawing.Size(56, 55);
            this.btnOptinPromotions.TabIndex = 20019;
            this.btnOptinPromotions.Tag = "";
            this.btnOptinPromotions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOptinPromotions.UseVisualStyleBackColor = true;
            this.btnOptinPromotions.Click += new System.EventHandler(this.pnlOptinPromotions_Click);
            // 
            // lblPromotionMode
            // 
            this.lblPromotionMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPromotionMode.ForeColor = System.Drawing.Color.White;
            this.lblPromotionMode.Location = new System.Drawing.Point(3, 699);
            this.lblPromotionMode.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblPromotionMode.Name = "lblPromotionMode";
            this.lblPromotionMode.Size = new System.Drawing.Size(200, 62);
            this.lblPromotionMode.TabIndex = 20017;
            this.lblPromotionMode.Text = "Promotion Mode:";
            this.lblPromotionMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlPromotionMode
            // 
            this.pnlPromotionMode.Controls.Add(this.cmbPromotionMode);
            this.pnlPromotionMode.Location = new System.Drawing.Point(209, 696);
            this.pnlPromotionMode.Name = "pnlPromotionMode";
            this.pnlPromotionMode.Size = new System.Drawing.Size(328, 68);
            this.pnlPromotionMode.TabIndex = 0;
            this.pnlPromotionMode.Tag = "OptInPromotionsMode";
            // 
            // cmbPromotionMode
            // 
            this.cmbPromotionMode.BackColor = System.Drawing.Color.White;
            this.cmbPromotionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPromotionMode.Font = new System.Drawing.Font("Verdana", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPromotionMode.ForeColor = System.Drawing.Color.Black;
            this.cmbPromotionMode.FormattingEnabled = true;
            this.cmbPromotionMode.Items.AddRange(new object[] {
            "Female",
            "Male",
            "Not Set"});
            this.cmbPromotionMode.Location = new System.Drawing.Point(9, 7);
            this.cmbPromotionMode.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbPromotionMode.Name = "cmbPromotionMode";
            this.cmbPromotionMode.Size = new System.Drawing.Size(310, 52);
            this.cmbPromotionMode.TabIndex = 20018;
            this.cmbPromotionMode.Tag = "";
            // 
            // flpCustomAttributes
            // 
            this.flpCustomAttributes.Location = new System.Drawing.Point(0, 767);
            this.flpCustomAttributes.Margin = new System.Windows.Forms.Padding(0);
            this.flpCustomAttributes.MinimumSize = new System.Drawing.Size(300, 60);
            this.flpCustomAttributes.Name = "flpCustomAttributes";
            this.flpCustomAttributes.Size = new System.Drawing.Size(1047, 80);
            this.flpCustomAttributes.TabIndex = 39;
            // 
            // lblPhoto
            // 
            this.lblPhoto.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhoto.ForeColor = System.Drawing.Color.White;
            this.lblPhoto.Location = new System.Drawing.Point(3, 847);
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
            this.pbCapture.Location = new System.Drawing.Point(209, 850);
            this.pbCapture.Name = "pbCapture";
            this.pbCapture.Size = new System.Drawing.Size(200, 120);
            this.pbCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCapture.TabIndex = 1035;
            this.pbCapture.TabStop = false;
            this.pbCapture.Click += new System.EventHandler(this.pbCapture_Click);
            // 
            // pnlTermsAndConditions
            // 
            this.pnlTermsAndConditions.Controls.Add(this.lblTermsAndConditions);
            this.pnlTermsAndConditions.Controls.Add(this.btnTermsAndCondition);
            this.pnlTermsAndConditions.Location = new System.Drawing.Point(3, 976);
            this.pnlTermsAndConditions.Name = "pnlTermsAndConditions";
            this.pnlTermsAndConditions.Size = new System.Drawing.Size(1038, 65);
            this.pnlTermsAndConditions.TabIndex = 20021;
            this.pnlTermsAndConditions.Tag = "false";
            this.pnlTermsAndConditions.Visible = false;
            this.pnlTermsAndConditions.Click += new System.EventHandler(this.pnlTermsAndConditions_Click);
            // 
            // lblTermsAndConditions
            // 
            this.lblTermsAndConditions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTermsAndConditions.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTermsAndConditions.ForeColor = System.Drawing.Color.White;
            this.lblTermsAndConditions.Location = new System.Drawing.Point(72, 8);
            this.lblTermsAndConditions.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lblTermsAndConditions.Name = "lblTermsAndConditions";
            this.lblTermsAndConditions.Size = new System.Drawing.Size(962, 46);
            this.lblTermsAndConditions.TabIndex = 20020;
            this.lblTermsAndConditions.Text = "Terms and Conditions";
            this.lblTermsAndConditions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTermsAndConditions.Click += new System.EventHandler(this.pnlTermsAndConditions_Click);
            // 
            // btnTermsAndCondition
            // 
            this.btnTermsAndCondition.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnTermsAndCondition.FlatAppearance.BorderSize = 0;
            this.btnTermsAndCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTermsAndCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.btnTermsAndCondition.ForeColor = System.Drawing.Color.White;
            this.btnTermsAndCondition.Image = global::Parafait_Kiosk.Properties.Resources.checkbox_unchecked;
            this.btnTermsAndCondition.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTermsAndCondition.Location = new System.Drawing.Point(10, 4);
            this.btnTermsAndCondition.Name = "btnTermsAndCondition";
            this.btnTermsAndCondition.Size = new System.Drawing.Size(56, 55);
            this.btnTermsAndCondition.TabIndex = 20019;
            this.btnTermsAndCondition.Tag = "";
            this.btnTermsAndCondition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTermsAndCondition.UseVisualStyleBackColor = true;
            this.btnTermsAndCondition.Click += new System.EventHandler(this.pnlTermsAndConditions_Click);
            // 
            // lblRegistration
            // 
            this.lblRegistration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRegistration.BackColor = System.Drawing.Color.Transparent;
            this.lblRegistration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRegistration.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegistration.ForeColor = System.Drawing.Color.White;
            this.lblRegistration.Location = new System.Drawing.Point(47, 116);
            this.lblRegistration.Name = "lblRegistration";
            this.lblRegistration.Size = new System.Drawing.Size(1353, 66);
            this.lblRegistration.TabIndex = 1037;
            this.lblRegistration.Text = "Customer Registration";
            this.lblRegistration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRegistration.Visible = false;
            // 
            // lblMessage1
            // 
            this.lblMessage1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage1.BackColor = System.Drawing.Color.Transparent;
            this.lblMessage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage1.ForeColor = System.Drawing.Color.White;
            this.lblMessage1.Location = new System.Drawing.Point(52, 170);
            this.lblMessage1.Name = "lblMessage1";
            this.lblMessage1.Size = new System.Drawing.Size(1348, 42);
            this.lblMessage1.TabIndex = 1038;
            this.lblMessage1.Text = "(Kindly note this is not for MIP registration.";
            this.lblMessage1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.btnClose);
            this.panelButtons.Controls.Add(this.buttonCustomerSave);
            this.panelButtons.Controls.Add(this.btnShowKeyPad);
            this.panelButtons.Location = new System.Drawing.Point(1077, 250);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(207, 279);
            this.panelButtons.TabIndex = 1039;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(3, 92);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(197, 79);
            this.btnClose.TabIndex = 1026;
            this.btnClose.Text = "Cancel";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonCustomerSave_MouseDown);
            this.btnClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonCustomerSave_MouseUp);
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
            this.buttonCustomerSave.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCustomerSave.ForeColor = System.Drawing.Color.White;
            this.buttonCustomerSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCustomerSave.Location = new System.Drawing.Point(3, 2);
            this.buttonCustomerSave.Name = "buttonCustomerSave";
            this.buttonCustomerSave.Size = new System.Drawing.Size(197, 79);
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
            this.btnShowKeyPad.Location = new System.Drawing.Point(124, 191);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(83, 73);
            this.btnShowKeyPad.TabIndex = 20001;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.btnShowKeyPad_Click);
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
            this.lblSiteName.Font = new System.Drawing.Font("Verdana", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(120, 0);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1227, 82);
            this.lblSiteName.TabIndex = 1040;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            this.lblSiteName.Visible = false;
            // 
            // lblMessage2
            // 
            this.lblMessage2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage2.BackColor = System.Drawing.Color.Transparent;
            this.lblMessage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage2.ForeColor = System.Drawing.Color.White;
            this.lblMessage2.Location = new System.Drawing.Point(52, 212);
            this.lblMessage2.Name = "lblMessage2";
            this.lblMessage2.Size = new System.Drawing.Size(1348, 35);
            this.lblMessage2.TabIndex = 1602;
            this.lblMessage2.Text = "Fill up your personal details below to receive Zone X updates.)";
            this.lblMessage2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.lblTimeRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeRemaining.ForeColor = System.Drawing.Color.White;
            this.lblTimeRemaining.Location = new System.Drawing.Point(1126, 28);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(142, 110);
            this.lblTimeRemaining.TabIndex = 1603;
            this.lblTimeRemaining.Text = "1:45";
            this.lblTimeRemaining.UseVisualStyleBackColor = false;
            // 
            // textBoxMessageLine
            // 
            this.textBoxMessageLine.BackColor = System.Drawing.Color.Transparent;
            this.textBoxMessageLine.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.bottom_bar;
            this.textBoxMessageLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.textBoxMessageLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxMessageLine.FlatAppearance.BorderSize = 0;
            this.textBoxMessageLine.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.textBoxMessageLine.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.textBoxMessageLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.textBoxMessageLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessageLine.ForeColor = System.Drawing.Color.White;
            this.textBoxMessageLine.Location = new System.Drawing.Point(0, 739);
            this.textBoxMessageLine.Name = "textBoxMessageLine";
            this.textBoxMessageLine.Size = new System.Drawing.Size(1280, 49);
            this.textBoxMessageLine.TabIndex = 20014;
            this.textBoxMessageLine.Text = "Message";
            this.textBoxMessageLine.UseVisualStyleBackColor = false;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F);
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(6, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(1261, 66);
            this.label10.TabIndex = 20015;
            this.label10.Text = "Register";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Customer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_Screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1280, 788);
            this.Controls.Add(this.flpCustomerMain);
            this.Controls.Add(this.textBoxMessageLine);
            this.Controls.Add(this.lblTimeRemaining);
            this.Controls.Add(this.lblMessage2);
            this.Controls.Add(this.lblMessage1);
            this.Controls.Add(this.lblRegistration);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblSiteName);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Customer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Details";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Customer_FormClosing);
            this.Load += new System.EventHandler(this.Customer_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Customer_KeyPress);
            this.Controls.SetChildIndex(this.lblSiteName, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.lblRegistration, 0);
            this.Controls.SetChildIndex(this.lblMessage1, 0);
            this.Controls.SetChildIndex(this.lblMessage2, 0);
            this.Controls.SetChildIndex(this.lblTimeRemaining, 0);
            this.Controls.SetChildIndex(this.textBoxMessageLine, 0);
            this.Controls.SetChildIndex(this.flpCustomerMain, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.flpCustomerMain.ResumeLayout(false);
            this.flpCustomerMain.PerformLayout();
            this.pnlOptinPromotions.ResumeLayout(false);
            this.pnlPromotionMode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCapture)).EndInit();
            this.pnlTermsAndConditions.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblComments;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.TextBox textBoxPin;
        private System.Windows.Forms.Label lblPostaCode;
        private System.Windows.Forms.Button buttonCustomerSave;
        private System.Windows.Forms.ComboBox comboBoxGender;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.TextBox textBoxAnniversaryDate;
        private System.Windows.Forms.TextBox textBoxContactPhone1;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.TextBox textBoxCity;
        private System.Windows.Forms.TextBox textBoxAddress2;
        private System.Windows.Forms.TextBox textBoxAddress1;
        private System.Windows.Forms.Label lblAnniversary;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtUniqueId;
        private System.Windows.Forms.Label lblUniqueId;
        internal System.Windows.Forms.Label lblRegistration;
        private System.Windows.Forms.Label lblMessage1;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.TextBox txtBirthDate;
        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.FlowLayoutPanel flpCustomerMain;
        private System.Windows.Forms.Label lblFBUserId;
        private System.Windows.Forms.TextBox txtFBUserId;
        private System.Windows.Forms.Label lblFBToken;
        private System.Windows.Forms.TextBox txtFBAccessToken;
        private System.Windows.Forms.FlowLayoutPanel flpCustomAttributes;
        private System.Windows.Forms.PictureBox pbCapture;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPhoto;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label lblMessage2;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.ComboBox cmbState;
        private System.Windows.Forms.Button lblTimeRemaining;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox cmbTitle;
        //private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button textBoxMessageLine;
        internal System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbCountry;
        private Panel pnlOptinPromotions;
        private Label lblOptinPromotions;
        private Button btnOptinPromotions;
        private Label lblPromotionMode;
        private ComboBox cmbPromotionMode;
        private Panel pnlTermsAndConditions;
        private Label lblTermsAndConditions;
        private Button btnTermsAndCondition;
        private Panel pnlPromotionMode;
    }
}
