namespace Semnox.Parafait.Customer
{
    partial class frmCustomerAddress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomerAddress));
            this.flpAddress = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlActive = new System.Windows.Forms.Panel();
            this.lblActive = new System.Windows.Forms.Label();
            this.chkActive = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.pnlType = new System.Windows.Forms.Panel();
            this.lblType = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.pnlLine1 = new System.Windows.Forms.Panel();
            this.txtLine1 = new System.Windows.Forms.TextBox();
            this.lblAddress1 = new System.Windows.Forms.Label();
            this.pnlLine2 = new System.Windows.Forms.Panel();
            this.txtLine2 = new System.Windows.Forms.TextBox();
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.pnlLine3 = new System.Windows.Forms.Panel();
            this.txtLine3 = new System.Windows.Forms.TextBox();
            this.lblAddress3 = new System.Windows.Forms.Label();
            this.pnlCity = new System.Windows.Forms.Panel();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.pnlPostalCode = new System.Windows.Forms.Panel();
            this.txtPostalCode = new System.Windows.Forms.TextBox();
            this.lblPostalCode = new System.Windows.Forms.Label();
            this.pnlState = new System.Windows.Forms.Panel();
            this.btnStateView = new System.Windows.Forms.Button();
            this.cbState = new System.Windows.Forms.ComboBox();
            this.lblState = new System.Windows.Forms.Label();
            this.pnlCountry = new System.Windows.Forms.Panel();
            this.btnCountryView = new System.Windows.Forms.Button();
            this.cbCountry = new System.Windows.Forms.ComboBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.pnlMessage = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.flpAddress.SuspendLayout();
            this.pnlActive.SuspendLayout();
            this.pnlType.SuspendLayout();
            this.pnlLine1.SuspendLayout();
            this.pnlLine2.SuspendLayout();
            this.pnlLine3.SuspendLayout();
            this.pnlCity.SuspendLayout();
            this.pnlPostalCode.SuspendLayout();
            this.pnlState.SuspendLayout();
            this.pnlCountry.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.pnlMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpAddress
            // 
            this.flpAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpAddress.AutoScroll = true;
            this.flpAddress.AutoSize = true;
            this.flpAddress.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpAddress.Controls.Add(this.pnlActive);
            this.flpAddress.Controls.Add(this.pnlType);
            this.flpAddress.Controls.Add(this.pnlLine1);
            this.flpAddress.Controls.Add(this.pnlLine2);
            this.flpAddress.Controls.Add(this.pnlLine3);
            this.flpAddress.Controls.Add(this.pnlCity);
            this.flpAddress.Controls.Add(this.pnlPostalCode);
            this.flpAddress.Controls.Add(this.pnlState);
            this.flpAddress.Controls.Add(this.pnlCountry);
            this.flpAddress.Controls.Add(this.pnlButtons);
            this.flpAddress.Controls.Add(this.pnlMessage);
            this.flpAddress.Location = new System.Drawing.Point(8, 5);
            this.flpAddress.Name = "flpAddress";
            this.flpAddress.Size = new System.Drawing.Size(502, 636);
            this.flpAddress.TabIndex = 0;
            this.flpAddress.SizeChanged += new System.EventHandler(this.flpAddress_SizeChanged);
            // 
            // pnlActive
            // 
            this.pnlActive.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlActive.Controls.Add(this.lblActive);
            this.pnlActive.Controls.Add(this.chkActive);
            this.pnlActive.Location = new System.Drawing.Point(3, 3);
            this.pnlActive.Name = "pnlActive";
            this.pnlActive.Size = new System.Drawing.Size(496, 40);
            this.pnlActive.TabIndex = 0;
            // 
            // lblActive
            // 
            this.lblActive.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblActive.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActive.Location = new System.Drawing.Point(40, 12);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(75, 19);
            this.lblActive.TabIndex = 0;
            this.lblActive.Text = "Active :";
            // 
            // chkActive
            // 
            this.chkActive.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkActive.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkActive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.FlatAppearance.BorderColor = System.Drawing.Color.SlateGray;
            this.chkActive.FlatAppearance.BorderSize = 0;
            this.chkActive.FlatAppearance.CheckedBackColor = System.Drawing.Color.SlateGray;
            this.chkActive.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateGray;
            this.chkActive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SlateGray;
            this.chkActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkActive.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkActive.ImageIndex = 0;
            this.chkActive.Location = new System.Drawing.Point(110, 4);
            this.chkActive.Margin = new System.Windows.Forms.Padding(2);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(120, 35);
            this.chkActive.TabIndex = 0;
            this.chkActive.Tag = "TeamUser";
            this.chkActive.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // pnlType
            // 
            this.pnlType.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlType.Controls.Add(this.lblType);
            this.pnlType.Controls.Add(this.cbType);
            this.pnlType.Location = new System.Drawing.Point(3, 49);
            this.pnlType.Name = "pnlType";
            this.pnlType.Size = new System.Drawing.Size(496, 35);
            this.pnlType.TabIndex = 5;
            // 
            // lblType
            // 
            this.lblType.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblType.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.Location = new System.Drawing.Point(49, 11);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(61, 19);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Type :";
            // 
            // cbType
            // 
            this.cbType.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(110, 1);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(362, 32);
            this.cbType.TabIndex = 1;
            // 
            // pnlLine1
            // 
            this.pnlLine1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlLine1.Controls.Add(this.txtLine1);
            this.pnlLine1.Controls.Add(this.lblAddress1);
            this.pnlLine1.Location = new System.Drawing.Point(3, 90);
            this.pnlLine1.Name = "pnlLine1";
            this.pnlLine1.Size = new System.Drawing.Size(496, 80);
            this.pnlLine1.TabIndex = 2;
            // 
            // txtLine1
            // 
            this.txtLine1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLine1.Location = new System.Drawing.Point(109, 2);
            this.txtLine1.MaxLength = 100;
            this.txtLine1.Multiline = true;
            this.txtLine1.Name = "txtLine1";
            this.txtLine1.Size = new System.Drawing.Size(363, 74);
            this.txtLine1.TabIndex = 2;
            // 
            // lblAddress1
            // 
            this.lblAddress1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblAddress1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress1.Location = new System.Drawing.Point(42, 33);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(61, 19);
            this.lblAddress1.TabIndex = 0;
            this.lblAddress1.Text = "Line1 :";
            // 
            // pnlLine2
            // 
            this.pnlLine2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlLine2.Controls.Add(this.txtLine2);
            this.pnlLine2.Controls.Add(this.lblAddress2);
            this.pnlLine2.Location = new System.Drawing.Point(3, 176);
            this.pnlLine2.Name = "pnlLine2";
            this.pnlLine2.Size = new System.Drawing.Size(496, 80);
            this.pnlLine2.TabIndex = 2;
            // 
            // txtLine2
            // 
            this.txtLine2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLine2.Location = new System.Drawing.Point(109, 3);
            this.txtLine2.MaxLength = 100;
            this.txtLine2.Multiline = true;
            this.txtLine2.Name = "txtLine2";
            this.txtLine2.Size = new System.Drawing.Size(363, 74);
            this.txtLine2.TabIndex = 3;
            // 
            // lblAddress2
            // 
            this.lblAddress2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblAddress2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress2.Location = new System.Drawing.Point(48, 31);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(61, 19);
            this.lblAddress2.TabIndex = 0;
            this.lblAddress2.Text = "Line2 :";
            // 
            // pnlLine3
            // 
            this.pnlLine3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlLine3.Controls.Add(this.txtLine3);
            this.pnlLine3.Controls.Add(this.lblAddress3);
            this.pnlLine3.Location = new System.Drawing.Point(3, 262);
            this.pnlLine3.Name = "pnlLine3";
            this.pnlLine3.Size = new System.Drawing.Size(496, 80);
            this.pnlLine3.TabIndex = 4;
            // 
            // txtLine3
            // 
            this.txtLine3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLine3.Location = new System.Drawing.Point(109, 3);
            this.txtLine3.MaxLength = 100;
            this.txtLine3.Multiline = true;
            this.txtLine3.Name = "txtLine3";
            this.txtLine3.Size = new System.Drawing.Size(363, 74);
            this.txtLine3.TabIndex = 4;
            // 
            // lblAddress3
            // 
            this.lblAddress3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblAddress3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress3.Location = new System.Drawing.Point(49, 31);
            this.lblAddress3.Name = "lblAddress3";
            this.lblAddress3.Size = new System.Drawing.Size(65, 19);
            this.lblAddress3.TabIndex = 0;
            this.lblAddress3.Text = "Line3 :";
            // 
            // pnlCity
            // 
            this.pnlCity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlCity.Controls.Add(this.txtCity);
            this.pnlCity.Controls.Add(this.lblCity);
            this.pnlCity.Location = new System.Drawing.Point(3, 348);
            this.pnlCity.Name = "pnlCity";
            this.pnlCity.Size = new System.Drawing.Size(496, 40);
            this.pnlCity.TabIndex = 3;
            // 
            // txtCity
            // 
            this.txtCity.Font = new System.Drawing.Font("Arial", 15F);
            this.txtCity.Location = new System.Drawing.Point(109, 4);
            this.txtCity.MaxLength = 100;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(363, 30);
            this.txtCity.TabIndex = 5;
            // 
            // lblCity
            // 
            this.lblCity.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblCity.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCity.Location = new System.Drawing.Point(56, 11);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(58, 19);
            this.lblCity.TabIndex = 0;
            this.lblCity.Text = "City  :";
            // 
            // pnlPostalCode
            // 
            this.pnlPostalCode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlPostalCode.Controls.Add(this.txtPostalCode);
            this.pnlPostalCode.Controls.Add(this.lblPostalCode);
            this.pnlPostalCode.Location = new System.Drawing.Point(3, 394);
            this.pnlPostalCode.Name = "pnlPostalCode";
            this.pnlPostalCode.Size = new System.Drawing.Size(496, 40);
            this.pnlPostalCode.TabIndex = 4;
            // 
            // txtPostalCode
            // 
            this.txtPostalCode.Font = new System.Drawing.Font("Arial", 15F);
            this.txtPostalCode.Location = new System.Drawing.Point(109, 2);
            this.txtPostalCode.MaxLength = 100;
            this.txtPostalCode.Name = "txtPostalCode";
            this.txtPostalCode.Size = new System.Drawing.Size(363, 30);
            this.txtPostalCode.TabIndex = 6;
            // 
            // lblPostalCode
            // 
            this.lblPostalCode.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblPostalCode.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPostalCode.Location = new System.Drawing.Point(2, 8);
            this.lblPostalCode.Name = "lblPostalCode";
            this.lblPostalCode.Size = new System.Drawing.Size(114, 19);
            this.lblPostalCode.TabIndex = 0;
            this.lblPostalCode.Text = "Postal Code :";
            // 
            // pnlState
            // 
            this.pnlState.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlState.Controls.Add(this.btnStateView);
            this.pnlState.Controls.Add(this.cbState);
            this.pnlState.Controls.Add(this.lblState);
            this.pnlState.Location = new System.Drawing.Point(3, 440);
            this.pnlState.Name = "pnlState";
            this.pnlState.Size = new System.Drawing.Size(496, 40);
            this.pnlState.TabIndex = 2;
            // 
            // btnStateView
            // 
            this.btnStateView.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnStateView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStateView.FlatAppearance.BorderSize = 0;
            this.btnStateView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStateView.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStateView.ForeColor = System.Drawing.Color.White;
            this.btnStateView.Location = new System.Drawing.Point(389, 6);
            this.btnStateView.Name = "btnStateView";
            this.btnStateView.Size = new System.Drawing.Size(83, 33);
            this.btnStateView.TabIndex = 8;
            this.btnStateView.Text = "State";
            this.btnStateView.UseVisualStyleBackColor = true;
            this.btnStateView.Click += new System.EventHandler(this.btnStateView_Click);
            // 
            // cbState
            // 
            this.cbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbState.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbState.FormattingEnabled = true;
            this.cbState.Location = new System.Drawing.Point(109, 6);
            this.cbState.Name = "cbState";
            this.cbState.Size = new System.Drawing.Size(274, 32);
            this.cbState.TabIndex = 7;
            // 
            // lblState
            // 
            this.lblState.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblState.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblState.Location = new System.Drawing.Point(49, 12);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(61, 19);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "State :";
            // 
            // pnlCountry
            // 
            this.pnlCountry.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlCountry.Controls.Add(this.btnCountryView);
            this.pnlCountry.Controls.Add(this.cbCountry);
            this.pnlCountry.Controls.Add(this.lblCountry);
            this.pnlCountry.Location = new System.Drawing.Point(3, 486);
            this.pnlCountry.Name = "pnlCountry";
            this.pnlCountry.Size = new System.Drawing.Size(496, 40);
            this.pnlCountry.TabIndex = 2;
            // 
            // btnCountryView
            // 
            this.btnCountryView.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnCountryView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCountryView.FlatAppearance.BorderSize = 0;
            this.btnCountryView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCountryView.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCountryView.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnCountryView.Location = new System.Drawing.Point(389, 5);
            this.btnCountryView.Name = "btnCountryView";
            this.btnCountryView.Size = new System.Drawing.Size(83, 33);
            this.btnCountryView.TabIndex = 10;
            this.btnCountryView.Text = "Country";
            this.btnCountryView.UseVisualStyleBackColor = true;
            this.btnCountryView.Click += new System.EventHandler(this.btnCountryView_Click);
            // 
            // cbCountry
            // 
            this.cbCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCountry.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold);
            this.cbCountry.FormattingEnabled = true;
            this.cbCountry.Location = new System.Drawing.Point(109, 5);
            this.cbCountry.Name = "cbCountry";
            this.cbCountry.Size = new System.Drawing.Size(274, 32);
            this.cbCountry.TabIndex = 9;
            // 
            // lblCountry
            // 
            this.lblCountry.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblCountry.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountry.Location = new System.Drawing.Point(29, 9);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(81, 19);
            this.lblCountry.TabIndex = 0;
            this.lblCountry.Text = "Country :";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnShowKeyPad);
            this.pnlButtons.Location = new System.Drawing.Point(3, 532);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(496, 55);
            this.pnlButtons.TabIndex = 6;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(278, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 50);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(111, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 50);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowKeyPad.BackColor = System.Drawing.Color.SlateGray;
            this.btnShowKeyPad.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.keyboard;
            this.btnShowKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Location = new System.Drawing.Point(417, 1);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(53, 52);
            this.btnShowKeyPad.TabIndex = 13;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            // 
            // pnlMessage
            // 
            this.pnlMessage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlMessage.Controls.Add(this.lblMessage);
            this.pnlMessage.Location = new System.Drawing.Point(3, 593);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Size = new System.Drawing.Size(496, 40);
            this.pnlMessage.TabIndex = 7;
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMessage.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(109, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(363, 23);
            this.lblMessage.TabIndex = 7;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(518, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(45, 45);
            this.btnClose.TabIndex = 14;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmCustomerAddress
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(572, 657);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.flpAddress);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCustomerAddress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.frmCustomerAddress_Load);
            this.flpAddress.ResumeLayout(false);
            this.pnlActive.ResumeLayout(false);
            this.pnlType.ResumeLayout(false);
            this.pnlLine1.ResumeLayout(false);
            this.pnlLine1.PerformLayout();
            this.pnlLine2.ResumeLayout(false);
            this.pnlLine2.PerformLayout();
            this.pnlLine3.ResumeLayout(false);
            this.pnlLine3.PerformLayout();
            this.pnlCity.ResumeLayout(false);
            this.pnlCity.PerformLayout();
            this.pnlPostalCode.ResumeLayout(false);
            this.pnlPostalCode.PerformLayout();
            this.pnlState.ResumeLayout(false);
            this.pnlCountry.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlMessage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flpAddress;
        private System.Windows.Forms.Panel pnlLine1;
        private System.Windows.Forms.Label lblAddress1;
        private System.Windows.Forms.Panel pnlLine2;
        private System.Windows.Forms.TextBox txtLine2;
        private System.Windows.Forms.Label lblAddress2;
        private System.Windows.Forms.Panel pnlCity;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Panel pnlLine3;
        private System.Windows.Forms.TextBox txtLine3;
        private System.Windows.Forms.Label lblAddress3;
        private System.Windows.Forms.Panel pnlPostalCode;
        private System.Windows.Forms.TextBox txtPostalCode;
        private System.Windows.Forms.Label lblPostalCode;
        private System.Windows.Forms.Panel pnlType;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.Panel pnlState;
        private System.Windows.Forms.ComboBox cbState;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Panel pnlCountry;
        private System.Windows.Forms.ComboBox cbCountry;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private Semnox.Core.GenericUtilities.CustomCheckBox chkActive;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Panel pnlMessage;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Button btnStateView;
        private System.Windows.Forms.Button btnCountryView;
        private System.Windows.Forms.TextBox txtLine1;
        private System.Windows.Forms.Panel pnlActive;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cbType;
    }
}