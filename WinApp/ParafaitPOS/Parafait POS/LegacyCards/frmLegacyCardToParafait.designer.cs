namespace Parafait_POS
{
    partial class frmLegacyCardToParafait
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLegacyCardToParafait));
            this.label1 = new System.Windows.Forms.Label();
            this.txtLegacyCardNumber = new System.Windows.Forms.TextBox();
            this.btnGetMCASHDetails = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCredits = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBonus = new System.Windows.Forms.TextBox();
            this.txtTickets = new System.Windows.Forms.TextBox();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEditLoyaltyPoints = new System.Windows.Forms.Button();
            this.btnEditTickets = new System.Windows.Forms.Button();
            this.btnEditTime = new System.Windows.Forms.Button();
            this.btnEditBonus = new System.Windows.Forms.Button();
            this.btnEditCredits = new System.Windows.Forms.Button();
            this.dtpLastPlayed = new System.Windows.Forms.DateTimePicker();
            this.label16 = new System.Windows.Forms.Label();
            this.txtFaceValue = new System.Windows.Forms.TextBox();
            this.chkIncludePackages = new System.Windows.Forms.CheckBox();
            this.txtLastPlayedTime = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtLoyalty = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.chkTransferred = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTransferDate = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCourtesy = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCreditsPlayed = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtParafaitCardNumber = new System.Windows.Forms.TextBox();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.lnkXRef = new System.Windows.Forms.LinkLabel();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.txtTranslatedCardNumber = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnInitiateTransfer = new System.Windows.Forms.Button();
            this.btnEditGames = new System.Windows.Forms.Button();
            this.txtGames = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Legacy Card Number:";
            // 
            // txtLegacyCardNumber
            // 
            this.txtLegacyCardNumber.Location = new System.Drawing.Point(197, 12);
            this.txtLegacyCardNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtLegacyCardNumber.Name = "txtLegacyCardNumber";
            this.txtLegacyCardNumber.Size = new System.Drawing.Size(228, 22);
            this.txtLegacyCardNumber.TabIndex = 1;
            this.txtLegacyCardNumber.Text = ";1160000059385";
            this.txtLegacyCardNumber.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // btnGetMCASHDetails
            // 
            this.btnGetMCASHDetails.CausesValidation = false;
            this.btnGetMCASHDetails.Location = new System.Drawing.Point(433, 9);
            this.btnGetMCASHDetails.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetMCASHDetails.Name = "btnGetMCASHDetails";
            this.btnGetMCASHDetails.Size = new System.Drawing.Size(100, 28);
            this.btnGetMCASHDetails.TabIndex = 3;
            this.btnGetMCASHDetails.Text = "Get Details";
            this.btnGetMCASHDetails.UseVisualStyleBackColor = true;
            this.btnGetMCASHDetails.Click += new System.EventHandler(this.btnGetDetails_ClickAsync);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(18, 48);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "Credits:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCredits
            // 
            this.txtCredits.BackColor = System.Drawing.Color.Gainsboro;
            this.txtCredits.Location = new System.Drawing.Point(123, 45);
            this.txtCredits.Margin = new System.Windows.Forms.Padding(4);
            this.txtCredits.Name = "txtCredits";
            this.txtCredits.ReadOnly = true;
            this.txtCredits.Size = new System.Drawing.Size(226, 22);
            this.txtCredits.TabIndex = 14;
            this.txtCredits.Text = "1234";
            this.txtCredits.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(18, 110);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "Bonus:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(18, 175);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 16);
            this.label6.TabIndex = 21;
            this.label6.Text = "Tickets:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(18, 388);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 16);
            this.label7.TabIndex = 36;
            this.label7.Text = "Customer Info:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBonus
            // 
            this.txtBonus.BackColor = System.Drawing.Color.Gainsboro;
            this.txtBonus.Location = new System.Drawing.Point(123, 107);
            this.txtBonus.Margin = new System.Windows.Forms.Padding(4);
            this.txtBonus.Name = "txtBonus";
            this.txtBonus.ReadOnly = true;
            this.txtBonus.Size = new System.Drawing.Size(226, 22);
            this.txtBonus.TabIndex = 18;
            this.txtBonus.Text = "1234";
            this.txtBonus.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // txtTickets
            // 
            this.txtTickets.BackColor = System.Drawing.Color.Gainsboro;
            this.txtTickets.Location = new System.Drawing.Point(123, 172);
            this.txtTickets.Margin = new System.Windows.Forms.Padding(4);
            this.txtTickets.Name = "txtTickets";
            this.txtTickets.ReadOnly = true;
            this.txtTickets.Size = new System.Drawing.Size(226, 22);
            this.txtTickets.TabIndex = 22;
            this.txtTickets.Text = "1234";
            this.txtTickets.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // txtCustomer
            // 
            this.txtCustomer.BackColor = System.Drawing.Color.Gainsboro;
            this.txtCustomer.Location = new System.Drawing.Point(123, 385);
            this.txtCustomer.Margin = new System.Windows.Forms.Padding(4);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.ReadOnly = true;
            this.txtCustomer.Size = new System.Drawing.Size(226, 22);
            this.txtCustomer.TabIndex = 37;
            this.txtCustomer.Text = "Mr Ramakrishna Aithal";
            this.txtCustomer.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEditGames);
            this.groupBox1.Controls.Add(this.txtGames);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.btnEditLoyaltyPoints);
            this.groupBox1.Controls.Add(this.btnEditTickets);
            this.groupBox1.Controls.Add(this.btnEditTime);
            this.groupBox1.Controls.Add(this.btnEditBonus);
            this.groupBox1.Controls.Add(this.btnEditCredits);
            this.groupBox1.Controls.Add(this.dtpLastPlayed);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txtFaceValue);
            this.groupBox1.Controls.Add(this.chkIncludePackages);
            this.groupBox1.Controls.Add(this.txtLastPlayedTime);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtLoyalty);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.chkTransferred);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtTransferDate);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtTime);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtCourtesy);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtCreditsPlayed);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtCustomer);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtTickets);
            this.groupBox1.Controls.Add(this.txtCredits);
            this.groupBox1.Controls.Add(this.txtBonus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(153, 63);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(540, 437);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // btnEditLoyaltyPoints
            // 
            this.btnEditLoyaltyPoints.BackColor = System.Drawing.Color.Chocolate;
            this.btnEditLoyaltyPoints.Location = new System.Drawing.Point(367, 238);
            this.btnEditLoyaltyPoints.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditLoyaltyPoints.Name = "btnEditLoyaltyPoints";
            this.btnEditLoyaltyPoints.Size = new System.Drawing.Size(126, 27);
            this.btnEditLoyaltyPoints.TabIndex = 20008;
            this.btnEditLoyaltyPoints.Text = "Edit Loyalty Points";
            this.btnEditLoyaltyPoints.UseVisualStyleBackColor = false;
            this.btnEditLoyaltyPoints.Click += new System.EventHandler(this.btnEditLoyaltyPoints_Click);
            // 
            // btnEditTickets
            // 
            this.btnEditTickets.BackColor = System.Drawing.Color.Chocolate;
            this.btnEditTickets.Location = new System.Drawing.Point(367, 170);
            this.btnEditTickets.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditTickets.Name = "btnEditTickets";
            this.btnEditTickets.Size = new System.Drawing.Size(126, 27);
            this.btnEditTickets.TabIndex = 20007;
            this.btnEditTickets.Text = "Edit Tickets";
            this.btnEditTickets.UseVisualStyleBackColor = false;
            this.btnEditTickets.Click += new System.EventHandler(this.btnEditTickets_Click);
            // 
            // btnEditTime
            // 
            this.btnEditTime.BackColor = System.Drawing.Color.Chocolate;
            this.btnEditTime.Location = new System.Drawing.Point(367, 136);
            this.btnEditTime.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditTime.Name = "btnEditTime";
            this.btnEditTime.Size = new System.Drawing.Size(126, 27);
            this.btnEditTime.TabIndex = 20006;
            this.btnEditTime.Text = "Edit Time";
            this.btnEditTime.UseVisualStyleBackColor = false;
            this.btnEditTime.Click += new System.EventHandler(this.btnEditTime_Click);
            // 
            // btnEditBonus
            // 
            this.btnEditBonus.BackColor = System.Drawing.Color.Chocolate;
            this.btnEditBonus.Location = new System.Drawing.Point(367, 103);
            this.btnEditBonus.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditBonus.Name = "btnEditBonus";
            this.btnEditBonus.Size = new System.Drawing.Size(126, 27);
            this.btnEditBonus.TabIndex = 20005;
            this.btnEditBonus.Text = "Edit Bonus";
            this.btnEditBonus.UseVisualStyleBackColor = false;
            this.btnEditBonus.Click += new System.EventHandler(this.btnEditBonus_Click);
            // 
            // btnEditCredits
            // 
            this.btnEditCredits.BackColor = System.Drawing.Color.Chocolate;
            this.btnEditCredits.Location = new System.Drawing.Point(367, 42);
            this.btnEditCredits.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditCredits.Name = "btnEditCredits";
            this.btnEditCredits.Size = new System.Drawing.Size(126, 27);
            this.btnEditCredits.TabIndex = 20004;
            this.btnEditCredits.Text = "Edit Credits";
            this.btnEditCredits.UseVisualStyleBackColor = false;
            this.btnEditCredits.Click += new System.EventHandler(this.btnEditCredits_Click);
            // 
            // dtpLastPlayed
            // 
            this.dtpLastPlayed.Enabled = false;
            this.dtpLastPlayed.Location = new System.Drawing.Point(350, 305);
            this.dtpLastPlayed.Name = "dtpLastPlayed";
            this.dtpLastPlayed.Size = new System.Drawing.Size(18, 22);
            this.dtpLastPlayed.TabIndex = 30;
            this.dtpLastPlayed.ValueChanged += new System.EventHandler(this.dtpLastPlayed_ValueChanged);
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(18, 19);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(103, 16);
            this.label16.TabIndex = 10;
            this.label16.Text = "Face Value:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFaceValue
            // 
            this.txtFaceValue.BackColor = System.Drawing.Color.Gainsboro;
            this.txtFaceValue.Location = new System.Drawing.Point(123, 16);
            this.txtFaceValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtFaceValue.Name = "txtFaceValue";
            this.txtFaceValue.ReadOnly = true;
            this.txtFaceValue.Size = new System.Drawing.Size(226, 22);
            this.txtFaceValue.TabIndex = 11;
            this.txtFaceValue.Text = "1234";
            this.txtFaceValue.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // chkIncludePackages
            // 
            this.chkIncludePackages.AutoSize = true;
            this.chkIncludePackages.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIncludePackages.Checked = true;
            this.chkIncludePackages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludePackages.Location = new System.Drawing.Point(3, 411);
            this.chkIncludePackages.Name = "chkIncludePackages";
            this.chkIncludePackages.Size = new System.Drawing.Size(135, 20);
            this.chkIncludePackages.TabIndex = 38;
            this.chkIncludePackages.Text = "Include Packages";
            this.chkIncludePackages.UseVisualStyleBackColor = true;
            // 
            // txtLastPlayedTime
            // 
            this.txtLastPlayedTime.BackColor = System.Drawing.Color.Gainsboro;
            this.txtLastPlayedTime.Location = new System.Drawing.Point(123, 304);
            this.txtLastPlayedTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtLastPlayedTime.Name = "txtLastPlayedTime";
            this.txtLastPlayedTime.ReadOnly = true;
            this.txtLastPlayedTime.Size = new System.Drawing.Size(226, 22);
            this.txtLastPlayedTime.TabIndex = 29;
            this.txtLastPlayedTime.Text = "01-Aug-2014 11:10 AM";
            this.txtLastPlayedTime.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(5, 307);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(116, 16);
            this.label14.TabIndex = 28;
            this.label14.Text = "Last Played Time:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLoyalty
            // 
            this.txtLoyalty.BackColor = System.Drawing.Color.Gainsboro;
            this.txtLoyalty.Enabled = false;
            this.txtLoyalty.Location = new System.Drawing.Point(123, 241);
            this.txtLoyalty.Margin = new System.Windows.Forms.Padding(4);
            this.txtLoyalty.Name = "txtLoyalty";
            this.txtLoyalty.ReadOnly = true;
            this.txtLoyalty.Size = new System.Drawing.Size(226, 22);
            this.txtLoyalty.TabIndex = 24;
            this.txtLoyalty.Text = "1234";
            this.txtLoyalty.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(18, 244);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(103, 16);
            this.label13.TabIndex = 23;
            this.label13.Text = "Loyalty Points:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkTransferred
            // 
            this.chkTransferred.AutoSize = true;
            this.chkTransferred.Enabled = false;
            this.chkTransferred.Location = new System.Drawing.Point(123, 335);
            this.chkTransferred.Name = "chkTransferred";
            this.chkTransferred.Size = new System.Drawing.Size(15, 14);
            this.chkTransferred.TabIndex = 32;
            this.chkTransferred.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(18, 332);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(103, 16);
            this.label11.TabIndex = 31;
            this.label11.Text = "Transferred:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTransferDate
            // 
            this.txtTransferDate.BackColor = System.Drawing.Color.Gainsboro;
            this.txtTransferDate.Enabled = false;
            this.txtTransferDate.Location = new System.Drawing.Point(123, 355);
            this.txtTransferDate.Margin = new System.Windows.Forms.Padding(4);
            this.txtTransferDate.Name = "txtTransferDate";
            this.txtTransferDate.ReadOnly = true;
            this.txtTransferDate.Size = new System.Drawing.Size(226, 22);
            this.txtTransferDate.TabIndex = 34;
            this.txtTransferDate.Text = "15-Aug-2014 01:54PM";
            this.txtTransferDate.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(18, 358);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(103, 16);
            this.label10.TabIndex = 33;
            this.label10.Text = "Transfer Date:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTime
            // 
            this.txtTime.BackColor = System.Drawing.Color.Gainsboro;
            this.txtTime.Location = new System.Drawing.Point(123, 139);
            this.txtTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtTime.Name = "txtTime";
            this.txtTime.ReadOnly = true;
            this.txtTime.Size = new System.Drawing.Size(226, 22);
            this.txtTime.TabIndex = 20;
            this.txtTime.Text = "1234";
            this.txtTime.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(18, 142);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 16);
            this.label9.TabIndex = 19;
            this.label9.Text = "Time:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCourtesy
            // 
            this.txtCourtesy.BackColor = System.Drawing.Color.Gainsboro;
            this.txtCourtesy.Location = new System.Drawing.Point(123, 76);
            this.txtCourtesy.Margin = new System.Windows.Forms.Padding(4);
            this.txtCourtesy.Name = "txtCourtesy";
            this.txtCourtesy.ReadOnly = true;
            this.txtCourtesy.Size = new System.Drawing.Size(226, 22);
            this.txtCourtesy.TabIndex = 16;
            this.txtCourtesy.Text = "1234";
            this.txtCourtesy.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(18, 77);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "Courtesy:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCreditsPlayed
            // 
            this.txtCreditsPlayed.BackColor = System.Drawing.Color.Gainsboro;
            this.txtCreditsPlayed.Location = new System.Drawing.Point(123, 272);
            this.txtCreditsPlayed.Margin = new System.Windows.Forms.Padding(4);
            this.txtCreditsPlayed.Name = "txtCreditsPlayed";
            this.txtCreditsPlayed.ReadOnly = true;
            this.txtCreditsPlayed.Size = new System.Drawing.Size(226, 22);
            this.txtCreditsPlayed.TabIndex = 26;
            this.txtCreditsPlayed.Text = "1234";
            this.txtCreditsPlayed.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(18, 275);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 16);
            this.label2.TabIndex = 25;
            this.label2.Text = "Credits Played:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(178, 558);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 16);
            this.label8.TabIndex = 42;
            this.label8.Text = "Card Number:";
            // 
            // txtParafaitCardNumber
            // 
            this.txtParafaitCardNumber.BackColor = System.Drawing.Color.Gainsboro;
            this.txtParafaitCardNumber.Location = new System.Drawing.Point(277, 555);
            this.txtParafaitCardNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtParafaitCardNumber.Name = "txtParafaitCardNumber";
            this.txtParafaitCardNumber.ReadOnly = true;
            this.txtParafaitCardNumber.Size = new System.Drawing.Size(139, 22);
            this.txtParafaitCardNumber.TabIndex = 43;
            this.txtParafaitCardNumber.Text = "1234";
            this.txtParafaitCardNumber.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // btnTransfer
            // 
            this.btnTransfer.BackColor = System.Drawing.Color.Chocolate;
            this.btnTransfer.Location = new System.Drawing.Point(264, 584);
            this.btnTransfer.Margin = new System.Windows.Forms.Padding(4);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(100, 54);
            this.btnTransfer.TabIndex = 45;
            this.btnTransfer.Text = "Transfer";
            this.btnTransfer.UseVisualStyleBackColor = false;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_ClickAsync);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Chocolate;
            this.btnCancel.Location = new System.Drawing.Point(412, 584);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 54);
            this.btnCancel.TabIndex = 46;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.White;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.Black;
            this.txtMessage.Location = new System.Drawing.Point(0, 648);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(705, 26);
            this.txtMessage.TabIndex = 48;
            this.txtMessage.Text = "Tap Card";
            // 
            // btnClear
            // 
            this.btnClear.CausesValidation = false;
            this.btnClear.Location = new System.Drawing.Point(546, 9);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 28);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lnkXRef
            // 
            this.lnkXRef.AutoSize = true;
            this.lnkXRef.Location = new System.Drawing.Point(544, 622);
            this.lnkXRef.Name = "lnkXRef";
            this.lnkXRef.Size = new System.Drawing.Size(162, 16);
            this.lnkXRef.TabIndex = 47;
            this.lnkXRef.TabStop = true;
            this.lnkXRef.Text = "View Legacy Card Details";
            this.lnkXRef.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkXRef_LinkClicked);
            // 
            // txtNotes
            // 
            this.txtNotes.BackColor = System.Drawing.Color.Bisque;
            this.txtNotes.Location = new System.Drawing.Point(277, 502);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(4);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(282, 46);
            this.txtNotes.TabIndex = 41;
            this.txtNotes.Text = "Transferring to Parafait card ABCDEF1234.";
            this.txtNotes.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(163, 504);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(111, 16);
            this.label12.TabIndex = 40;
            this.label12.Text = "Remarks:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnShowKeyPad.Location = new System.Drawing.Point(657, 4);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowKeyPad.TabIndex = 20002;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.btnShowKeyPad_Click);
            // 
            // txtTranslatedCardNumber
            // 
            this.txtTranslatedCardNumber.BackColor = System.Drawing.Color.Coral;
            this.txtTranslatedCardNumber.Location = new System.Drawing.Point(196, 37);
            this.txtTranslatedCardNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtTranslatedCardNumber.Name = "txtTranslatedCardNumber";
            this.txtTranslatedCardNumber.ReadOnly = true;
            this.txtTranslatedCardNumber.Size = new System.Drawing.Size(110, 22);
            this.txtTranslatedCardNumber.TabIndex = 6;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(103, 39);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(91, 16);
            this.label15.TabIndex = 20003;
            this.label15.Text = "Card Number:";
            // 
            // btnInitiateTransfer
            // 
            this.btnInitiateTransfer.BackColor = System.Drawing.Color.Chocolate;
            this.btnInitiateTransfer.Location = new System.Drawing.Point(116, 584);
            this.btnInitiateTransfer.Margin = new System.Windows.Forms.Padding(4);
            this.btnInitiateTransfer.Name = "btnInitiateTransfer";
            this.btnInitiateTransfer.Size = new System.Drawing.Size(100, 54);
            this.btnInitiateTransfer.TabIndex = 44;
            this.btnInitiateTransfer.Text = "Initiate Transfer";
            this.btnInitiateTransfer.UseVisualStyleBackColor = false;
            this.btnInitiateTransfer.Click += new System.EventHandler(this.btnInitiateTransfer_Click);
            // 
            // btnEditGames
            // 
            this.btnEditGames.BackColor = System.Drawing.Color.Chocolate;
            this.btnEditGames.Location = new System.Drawing.Point(367, 204);
            this.btnEditGames.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditGames.Name = "btnEditGames";
            this.btnEditGames.Size = new System.Drawing.Size(126, 27);
            this.btnEditGames.TabIndex = 20011;
            this.btnEditGames.Text = "Edit Games";
            this.btnEditGames.UseVisualStyleBackColor = false;
            this.btnEditGames.Click += new System.EventHandler(this.btnEditGames_Click);
            // 
            // txtGames
            // 
            this.txtGames.BackColor = System.Drawing.Color.Gainsboro;
            this.txtGames.Location = new System.Drawing.Point(123, 207);
            this.txtGames.Margin = new System.Windows.Forms.Padding(4);
            this.txtGames.Name = "txtGames";
            this.txtGames.ReadOnly = true;
            this.txtGames.Size = new System.Drawing.Size(226, 22);
            this.txtGames.TabIndex = 20010;
            this.txtGames.Text = "1234";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(18, 210);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(103, 16);
            this.label17.TabIndex = 20009;
            this.label17.Text = "Games:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmLegacyCardToParafait
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 674);
            this.Controls.Add(this.btnInitiateTransfer);
            this.Controls.Add(this.txtTranslatedCardNumber);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnShowKeyPad);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lnkXRef);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnTransfer);
            this.Controls.Add(this.txtParafaitCardNumber);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGetMCASHDetails);
            this.Controls.Add(this.txtLegacyCardNumber);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmLegacyCardToParafait";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transfer Legacy Card";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLegacyCashToParafait_FormClosed);
            this.Load += new System.EventHandler(this.frmLegacyCashToParafait_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLegacyCardNumber;
        private System.Windows.Forms.Button btnGetMCASHDetails;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCredits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBonus;
        private System.Windows.Forms.TextBox txtTickets;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtParafaitCardNumber;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.LinkLabel lnkXRef;
        private System.Windows.Forms.TextBox txtCourtesy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTransferDate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtCreditsPlayed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkTransferred;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtLoyalty;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtLastPlayedTime;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.CheckBox chkIncludePackages;
        private System.Windows.Forms.TextBox txtTranslatedCardNumber;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnInitiateTransfer;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtFaceValue;
        private System.Windows.Forms.DateTimePicker dtpLastPlayed;
        private System.Windows.Forms.Button btnEditLoyaltyPoints;
        private System.Windows.Forms.Button btnEditTickets;
        private System.Windows.Forms.Button btnEditTime;
        private System.Windows.Forms.Button btnEditBonus;
        private System.Windows.Forms.Button btnEditCredits;
        private System.Windows.Forms.Button btnEditGames;
        private System.Windows.Forms.TextBox txtGames;
        private System.Windows.Forms.Label label17;
    }
}

