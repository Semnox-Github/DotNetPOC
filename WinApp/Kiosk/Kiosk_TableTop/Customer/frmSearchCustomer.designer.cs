namespace Parafait_Kiosk
{
    partial class frmSearchCustomer
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
            this.txtMessage = new System.Windows.Forms.Button();
            this.pnlEmailId = new System.Windows.Forms.Panel();
            this.txtEmailId = new System.Windows.Forms.TextBox();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.lblNewCustomerHeader = new System.Windows.Forms.Label();
            this.lblEnterPhoneHeader = new System.Windows.Forms.Label();
            this.lblEnterEmailHeader = new System.Windows.Forms.Label();
            this.lblORField = new System.Windows.Forms.Label();
            this.pnlPhoneNum = new System.Windows.Forms.Panel();
            this.txtPhoneNum = new System.Windows.Forms.TextBox();
            this.flpSearchOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlSearchViaPhone = new System.Windows.Forms.Panel();
            this.pnlSearchViaEmail = new System.Windows.Forms.Panel();
            this.lblGreeting = new System.Windows.Forms.Label();
            this.lblExistingCustomerHeader = new System.Windows.Forms.Label();
            this.lblExistingCustomerMsg = new System.Windows.Forms.Label();
            this.lblNewCustomerMsg = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnNewRegistration = new System.Windows.Forms.Button();
            this.pnlEmailId.SuspendLayout();
            this.pnlPhoneNum.SuspendLayout();
            this.flpSearchOptions.SuspendLayout();
            this.pnlSearchViaPhone.SuspendLayout();
            this.pnlSearchViaEmail.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(465, 882);
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(30, 1684);
            this.btnCancel.Click += new System.EventHandler(this.btnPrev_Click);
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
            this.txtMessage.Location = new System.Drawing.Point(0, 1030);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 50);
            this.txtMessage.TabIndex = 1053;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // pnlEmailId
            // 
            this.pnlEmailId.AutoSize = true;
            this.pnlEmailId.BackColor = System.Drawing.Color.Transparent;
            this.pnlEmailId.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.pnlEmailId.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlEmailId.Controls.Add(this.txtEmailId);
            this.pnlEmailId.Location = new System.Drawing.Point(0, 75);
            this.pnlEmailId.Name = "pnlEmailId";
            this.pnlEmailId.Size = new System.Drawing.Size(955, 90);
            this.pnlEmailId.TabIndex = 1039;
            // 
            // txtEmailId
            // 
            this.txtEmailId.BackColor = System.Drawing.Color.White;
            this.txtEmailId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEmailId.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmailId.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtEmailId.Location = new System.Drawing.Point(31, 20);
            this.txtEmailId.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtEmailId.MaxLength = 100;
            this.txtEmailId.Name = "txtEmailId";
            this.txtEmailId.Size = new System.Drawing.Size(900, 48);
            this.txtEmailId.TabIndex = 1041;
            this.txtEmailId.Text = "sathyavathi.saligrama@semnox.com";
            this.txtEmailId.Click += new System.EventHandler(this.textBox_Enter);
            this.txtEmailId.Enter += new System.EventHandler(this.textBox_Enter);
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
            this.btnShowKeyPad.Location = new System.Drawing.Point(1347, 756);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(87, 83);
            this.btnShowKeyPad.TabIndex = 20002;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.btnShowKeyPad_Click);
            this.btnShowKeyPad.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // lblNewCustomerHeader
            // 
            this.lblNewCustomerHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblNewCustomerHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 33.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewCustomerHeader.Location = new System.Drawing.Point(9, 105);
            this.lblNewCustomerHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblNewCustomerHeader.Name = "lblNewCustomerHeader";
            this.lblNewCustomerHeader.Size = new System.Drawing.Size(890, 63);
            this.lblNewCustomerHeader.TabIndex = 20003;
            this.lblNewCustomerHeader.Text = "New Customer,";
            this.lblNewCustomerHeader.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblEnterPhoneHeader
            // 
            this.lblEnterPhoneHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblEnterPhoneHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnterPhoneHeader.Location = new System.Drawing.Point(0, 0);
            this.lblEnterPhoneHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblEnterPhoneHeader.Name = "lblEnterPhoneHeader";
            this.lblEnterPhoneHeader.Size = new System.Drawing.Size(820, 70);
            this.lblEnterPhoneHeader.TabIndex = 1607;
            this.lblEnterPhoneHeader.Text = "Enter Phone #";
            this.lblEnterPhoneHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEnterEmailHeader
            // 
            this.lblEnterEmailHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblEnterEmailHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnterEmailHeader.Location = new System.Drawing.Point(0, 2);
            this.lblEnterEmailHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblEnterEmailHeader.Name = "lblEnterEmailHeader";
            this.lblEnterEmailHeader.Size = new System.Drawing.Size(820, 70);
            this.lblEnterEmailHeader.TabIndex = 1607;
            this.lblEnterEmailHeader.Text = "Enter Email Id";
            this.lblEnterEmailHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblORField
            // 
            this.lblORField.BackColor = System.Drawing.Color.Transparent;
            this.lblORField.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblORField.Location = new System.Drawing.Point(3, 170);
            this.lblORField.Margin = new System.Windows.Forms.Padding(3);
            this.lblORField.Name = "lblORField";
            this.lblORField.Size = new System.Drawing.Size(958, 70);
            this.lblORField.TabIndex = 20014;
            this.lblORField.Text = "OR";
            this.lblORField.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlPhoneNum
            // 
            this.pnlPhoneNum.AutoSize = true;
            this.pnlPhoneNum.BackColor = System.Drawing.Color.Transparent;
            this.pnlPhoneNum.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.pnlPhoneNum.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlPhoneNum.Controls.Add(this.txtPhoneNum);
            this.pnlPhoneNum.Location = new System.Drawing.Point(0, 74);
            this.pnlPhoneNum.Name = "pnlPhoneNum";
            this.pnlPhoneNum.Size = new System.Drawing.Size(955, 90);
            this.pnlPhoneNum.TabIndex = 20015;
            // 
            // txtPhoneNum
            // 
            this.txtPhoneNum.BackColor = System.Drawing.Color.White;
            this.txtPhoneNum.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPhoneNum.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhoneNum.ForeColor = System.Drawing.Color.DarkOrchid;
            this.txtPhoneNum.Location = new System.Drawing.Point(31, 19);
            this.txtPhoneNum.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtPhoneNum.MaxLength = 100;
            this.txtPhoneNum.Name = "txtPhoneNum";
            this.txtPhoneNum.Size = new System.Drawing.Size(900, 48);
            this.txtPhoneNum.TabIndex = 1041;
            this.txtPhoneNum.Text = "9876543210";
            this.txtPhoneNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPhoneNum_KeyPress);
            // 
            // flpSearchOptions
            // 
            this.flpSearchOptions.BackColor = System.Drawing.Color.Transparent;
            this.flpSearchOptions.Controls.Add(this.pnlSearchViaPhone);
            this.flpSearchOptions.Controls.Add(this.lblORField);
            this.flpSearchOptions.Controls.Add(this.pnlSearchViaEmail);
            this.flpSearchOptions.Location = new System.Drawing.Point(485, 328);
            this.flpSearchOptions.Name = "flpSearchOptions";
            this.flpSearchOptions.Size = new System.Drawing.Size(958, 415);
            this.flpSearchOptions.TabIndex = 20025;
            // 
            // pnlSearchViaPhone
            // 
            this.pnlSearchViaPhone.AutoSize = true;
            this.pnlSearchViaPhone.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlSearchViaPhone.BackColor = System.Drawing.Color.Transparent;
            this.pnlSearchViaPhone.Controls.Add(this.lblEnterPhoneHeader);
            this.pnlSearchViaPhone.Controls.Add(this.pnlPhoneNum);
            this.pnlSearchViaPhone.Location = new System.Drawing.Point(0, 0);
            this.pnlSearchViaPhone.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSearchViaPhone.Name = "pnlSearchViaPhone";
            this.pnlSearchViaPhone.Size = new System.Drawing.Size(958, 167);
            this.pnlSearchViaPhone.TabIndex = 20027;
            // 
            // pnlSearchViaEmail
            // 
            this.pnlSearchViaEmail.AutoSize = true;
            this.pnlSearchViaEmail.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlSearchViaEmail.BackColor = System.Drawing.Color.Transparent;
            this.pnlSearchViaEmail.Controls.Add(this.lblEnterEmailHeader);
            this.pnlSearchViaEmail.Controls.Add(this.pnlEmailId);
            this.pnlSearchViaEmail.Location = new System.Drawing.Point(0, 243);
            this.pnlSearchViaEmail.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSearchViaEmail.Name = "pnlSearchViaEmail";
            this.pnlSearchViaEmail.Size = new System.Drawing.Size(958, 168);
            this.pnlSearchViaEmail.TabIndex = 20026;
            // 
            // lblGreeting
            // 
            this.lblGreeting.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting.Location = new System.Drawing.Point(9, 35);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(1899, 60);
            this.lblGreeting.TabIndex = 4;
            this.lblGreeting.Text = "The transaction requires a customer association";
            this.lblGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExistingCustomerHeader
            // 
            this.lblExistingCustomerHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblExistingCustomerHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 33.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExistingCustomerHeader.Location = new System.Drawing.Point(9, 175);
            this.lblExistingCustomerHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblExistingCustomerHeader.Name = "lblExistingCustomerHeader";
            this.lblExistingCustomerHeader.Size = new System.Drawing.Size(890, 63);
            this.lblExistingCustomerHeader.TabIndex = 20026;
            this.lblExistingCustomerHeader.Text = "Existing Customer,";
            this.lblExistingCustomerHeader.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblExistingCustomerMsg
            // 
            this.lblExistingCustomerMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblExistingCustomerMsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 33.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExistingCustomerMsg.Location = new System.Drawing.Point(899, 175);
            this.lblExistingCustomerMsg.Margin = new System.Windows.Forms.Padding(0);
            this.lblExistingCustomerMsg.Name = "lblExistingCustomerMsg";
            this.lblExistingCustomerMsg.Size = new System.Drawing.Size(1009, 63);
            this.lblExistingCustomerMsg.TabIndex = 20028;
            this.lblExistingCustomerMsg.Text = "Tap your card or Search";
            // 
            // lblNewCustomerMsg
            // 
            this.lblNewCustomerMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblNewCustomerMsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 33.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewCustomerMsg.Location = new System.Drawing.Point(899, 105);
            this.lblNewCustomerMsg.Margin = new System.Windows.Forms.Padding(0);
            this.lblNewCustomerMsg.Name = "lblNewCustomerMsg";
            this.lblNewCustomerMsg.Size = new System.Drawing.Size(1009, 63);
            this.lblNewCustomerMsg.TabIndex = 20029;
            this.lblNewCustomerMsg.Text = "Press New Registration";
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(315, 878);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1295, 127);
            this.panelButtons.TabIndex = 20030;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(834, 882);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(250, 125);
            this.btnSearch.TabIndex = 1027;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnNewRegistration
            // 
            this.btnNewRegistration.BackColor = System.Drawing.Color.Transparent;
            this.btnNewRegistration.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnNewRegistration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNewRegistration.FlatAppearance.BorderSize = 0;
            this.btnNewRegistration.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNewRegistration.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNewRegistration.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNewRegistration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewRegistration.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewRegistration.ForeColor = System.Drawing.Color.White;
            this.btnNewRegistration.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewRegistration.Location = new System.Drawing.Point(1203, 882);
            this.btnNewRegistration.Name = "btnNewRegistration";
            this.btnNewRegistration.Size = new System.Drawing.Size(250, 125);
            this.btnNewRegistration.TabIndex = 1025;
            this.btnNewRegistration.Text = "New Registration";
            this.btnNewRegistration.UseVisualStyleBackColor = false;
            this.btnNewRegistration.Click += new System.EventHandler(this.btnNewRegistration_Click);
            // 
            // frmSearchCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.lblNewCustomerMsg);
            this.Controls.Add(this.lblExistingCustomerMsg);
            this.Controls.Add(this.lblExistingCustomerHeader);
            this.Controls.Add(this.lblGreeting);
            this.Controls.Add(this.flpSearchOptions);
            this.Controls.Add(this.lblNewCustomerHeader);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnNewRegistration);
            this.Controls.Add(this.btnShowKeyPad);
            this.Controls.Add(this.txtMessage);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSearchCustomer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmSearchCustomer";
            this.TransparencyKey = System.Drawing.Color.White;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmFetchCustomer_Closed);
            this.Load += new System.EventHandler(this.frmSearchCustomer_Load);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.btnShowKeyPad, 0);
            this.Controls.SetChildIndex(this.lblNewCustomerHeader, 0);
            this.Controls.SetChildIndex(this.flpSearchOptions, 0);
            this.Controls.SetChildIndex(this.lblGreeting, 0);
            this.Controls.SetChildIndex(this.lblExistingCustomerHeader, 0);
            this.Controls.SetChildIndex(this.lblExistingCustomerMsg, 0);
            this.Controls.SetChildIndex(this.lblNewCustomerMsg, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnSearch, 0);
            this.Controls.SetChildIndex(this.btnNewRegistration, 0);
            this.pnlEmailId.ResumeLayout(false);
            this.pnlEmailId.PerformLayout();
            this.pnlPhoneNum.ResumeLayout(false);
            this.pnlPhoneNum.PerformLayout();
            this.flpSearchOptions.ResumeLayout(false);
            this.flpSearchOptions.PerformLayout();
            this.pnlSearchViaPhone.ResumeLayout(false);
            this.pnlSearchViaPhone.PerformLayout();
            this.pnlSearchViaEmail.ResumeLayout(false);
            this.pnlSearchViaEmail.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlEmailId;
        private System.Windows.Forms.TextBox txtEmailId;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button btnShowKeyPad;
        public System.Windows.Forms.Label lblNewCustomerHeader;
        private System.Windows.Forms.Label lblEnterPhoneHeader;
        private System.Windows.Forms.Label lblEnterEmailHeader;
        private System.Windows.Forms.Label lblORField;
        private System.Windows.Forms.Panel pnlPhoneNum;
        private System.Windows.Forms.TextBox txtPhoneNum;
        private System.Windows.Forms.FlowLayoutPanel flpSearchOptions;
        private System.Windows.Forms.Panel pnlSearchViaEmail;
        private System.Windows.Forms.Panel pnlSearchViaPhone;
        public System.Windows.Forms.Label lblGreeting;
        public System.Windows.Forms.Label lblExistingCustomerHeader;
        public System.Windows.Forms.Label lblExistingCustomerMsg;
        public System.Windows.Forms.Label lblNewCustomerMsg;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnNewRegistration;
    }
}