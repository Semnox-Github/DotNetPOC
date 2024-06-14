namespace Parafait_POS
{
    partial class frmTechCard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTechCard));
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblStaffName = new System.Windows.Forms.Label();
            this.txtTechnicianName = new System.Windows.Forms.TextBox();
            this.dtpValidTill = new System.Windows.Forms.DateTimePicker();
            this.cmbTechCards = new System.Windows.Forms.ComboBox();
            this.lblSelectStaff = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lnkAdvanced = new System.Windows.Forms.LinkLabel();
            this.gbProducts = new System.Windows.Forms.GroupBox();
            this.flpStaffCardProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSample = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lnkDeactivate = new System.Windows.Forms.LinkLabel();
            this.txtTechnicianLastName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.gbCardInfo = new System.Windows.Forms.GroupBox();
            this.txtCredits = new System.Windows.Forms.TextBox();
            this.txtTickets = new System.Windows.Forms.TextBox();
            this.txtBalanceGames = new System.Windows.Forms.TextBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.lnkSelectStaff = new System.Windows.Forms.LinkLabel();
            this.lblGamebalance = new System.Windows.Forms.Label();
            this.gbProducts.SuspendLayout();
            this.flpStaffCardProducts.SuspendLayout();
            this.gbCardInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtCardNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCardNumber.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.Location = new System.Drawing.Point(116, 74);
            this.txtCardNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.ReadOnly = true;
            this.txtCardNumber.Size = new System.Drawing.Size(197, 23);
            this.txtCardNumber.TabIndex = 2;
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.Location = new System.Drawing.Point(19, 74);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(95, 17);
            this.lblCardNumber.TabIndex = 1;
            this.lblCardNumber.Text = "Card Number:";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(54, 220);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 17);
            this.label9.TabIndex = 54;
            this.label9.Text = "Tickets:sadasda";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStaffName
            // 
            this.lblStaffName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStaffName.Location = new System.Drawing.Point(16, 110);
            this.lblStaffName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStaffName.Name = "lblStaffName";
            this.lblStaffName.Size = new System.Drawing.Size(100, 17);
            this.lblStaffName.TabIndex = 57;
            this.lblStaffName.Text = "First Name: ";
            this.lblStaffName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTechnicianName
            // 
            this.txtTechnicianName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtTechnicianName.Enabled = false;
            this.txtTechnicianName.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTechnicianName.Location = new System.Drawing.Point(116, 109);
            this.txtTechnicianName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtTechnicianName.Name = "txtTechnicianName";
            this.txtTechnicianName.Size = new System.Drawing.Size(197, 23);
            this.txtTechnicianName.TabIndex = 4;
            // 
            // dtpValidTill
            // 
            this.dtpValidTill.CalendarFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpValidTill.CalendarMonthBackground = System.Drawing.Color.WhiteSmoke;
            this.dtpValidTill.CustomFormat = "dd-MMM-yyyy";
            this.dtpValidTill.Enabled = false;
            this.dtpValidTill.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpValidTill.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpValidTill.Location = new System.Drawing.Point(116, 255);
            this.dtpValidTill.Name = "dtpValidTill";
            this.dtpValidTill.Size = new System.Drawing.Size(278, 23);
            this.dtpValidTill.TabIndex = 10;
            this.dtpValidTill.Value = new System.DateTime(2017, 4, 10, 19, 44, 18, 0);
            // 
            // cmbTechCards
            // 
            this.cmbTechCards.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTechCards.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTechCards.FormattingEnabled = true;
            this.cmbTechCards.Location = new System.Drawing.Point(116, 38);
            this.cmbTechCards.Name = "cmbTechCards";
            this.cmbTechCards.Size = new System.Drawing.Size(278, 24);
            this.cmbTechCards.TabIndex = 1;
            // 
            // lblSelectStaff
            // 
            this.lblSelectStaff.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectStaff.Location = new System.Drawing.Point(19, 38);
            this.lblSelectStaff.Name = "lblSelectStaff";
            this.lblSelectStaff.Size = new System.Drawing.Size(95, 20);
            this.lblSelectStaff.TabIndex = 73;
            this.lblSelectStaff.Text = "Staff Cards:";
            this.lblSelectStaff.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(36, 322);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 17);
            this.label10.TabIndex = 75;
            this.label10.Text = "Notes:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNotes
            // 
            this.txtNotes.BackColor = System.Drawing.Color.White;
            this.txtNotes.Enabled = false;
            this.txtNotes.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNotes.Location = new System.Drawing.Point(116, 322);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(278, 61);
            this.txtNotes.TabIndex = 12;
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(309, 419);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(113, 32);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(450, 420);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 32);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.White;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 459);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(900, 22);
            this.txtMessage.TabIndex = 79;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 255);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 23);
            this.label6.TabIndex = 80;
            this.label6.Text = "Games Valid Till:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lnkAdvanced
            // 
            this.lnkAdvanced.AutoSize = true;
            this.lnkAdvanced.Location = new System.Drawing.Point(630, 428);
            this.lnkAdvanced.Name = "lnkAdvanced";
            this.lnkAdvanced.Size = new System.Drawing.Size(64, 16);
            this.lnkAdvanced.TabIndex = 13;
            this.lnkAdvanced.TabStop = true;
            this.lnkAdvanced.Text = "Advanced";
            this.lnkAdvanced.Visible = false;
            this.lnkAdvanced.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAdvanced_LinkClicked);
            // 
            // gbProducts
            // 
            this.gbProducts.Controls.Add(this.flpStaffCardProducts);
            this.gbProducts.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbProducts.Location = new System.Drawing.Point(444, 12);
            this.gbProducts.Name = "gbProducts";
            this.gbProducts.Size = new System.Drawing.Size(444, 401);
            this.gbProducts.TabIndex = 5;
            this.gbProducts.TabStop = false;
            this.gbProducts.Text = "Select Products ";
            // 
            // flpStaffCardProducts
            // 
            this.flpStaffCardProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.flpStaffCardProducts.AutoScroll = true;
            this.flpStaffCardProducts.BackColor = System.Drawing.Color.White;
            this.flpStaffCardProducts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpStaffCardProducts.Controls.Add(this.btnSample);
            this.flpStaffCardProducts.Enabled = false;
            this.flpStaffCardProducts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.flpStaffCardProducts.ForeColor = System.Drawing.SystemColors.MenuText;
            this.flpStaffCardProducts.Location = new System.Drawing.Point(6, 21);
            this.flpStaffCardProducts.Name = "flpStaffCardProducts";
            this.flpStaffCardProducts.Size = new System.Drawing.Size(432, 374);
            this.flpStaffCardProducts.TabIndex = 1;
            // 
            // btnSample
            // 
            this.btnSample.BackColor = System.Drawing.Color.Transparent;
            this.btnSample.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSample.BackgroundImage")));
            this.btnSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSample.FlatAppearance.BorderSize = 0;
            this.btnSample.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSample.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSample.ForeColor = System.Drawing.Color.White;
            this.btnSample.Location = new System.Drawing.Point(3, 3);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(102, 83);
            this.btnSample.TabIndex = 0;
            this.btnSample.Text = "sample";
            this.btnSample.UseVisualStyleBackColor = false;
            this.btnSample.Visible = false;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(229, 220);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 17);
            this.label5.TabIndex = 87;
            this.label5.Text = "Credits:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lnkDeactivate
            // 
            this.lnkDeactivate.AutoSize = true;
            this.lnkDeactivate.Enabled = false;
            this.lnkDeactivate.Location = new System.Drawing.Point(319, 76);
            this.lnkDeactivate.Name = "lnkDeactivate";
            this.lnkDeactivate.Size = new System.Drawing.Size(75, 16);
            this.lnkDeactivate.TabIndex = 3;
            this.lnkDeactivate.TabStop = true;
            this.lnkDeactivate.Text = "Deactivate";
            this.lnkDeactivate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDeactivate_LinkClicked);
            // 
            // txtTechnicianLastName
            // 
            this.txtTechnicianLastName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtTechnicianLastName.Enabled = false;
            this.txtTechnicianLastName.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTechnicianLastName.Location = new System.Drawing.Point(116, 143);
            this.txtTechnicianLastName.Name = "txtTechnicianLastName";
            this.txtTechnicianLastName.Size = new System.Drawing.Size(278, 23);
            this.txtTechnicianLastName.TabIndex = 6;
            // 
            // lblLastName
            // 
            this.lblLastName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastName.Location = new System.Drawing.Point(16, 146);
            this.lblLastName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(98, 17);
            this.lblLastName.TabIndex = 86;
            this.lblLastName.Text = "Last Name: ";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbCardInfo
            // 
            this.gbCardInfo.Controls.Add(this.txtCredits);
            this.gbCardInfo.Controls.Add(this.txtTickets);
            this.gbCardInfo.Controls.Add(this.txtBalanceGames);
            this.gbCardInfo.Controls.Add(this.lblProduct);
            this.gbCardInfo.Controls.Add(this.txtProduct);
            this.gbCardInfo.Controls.Add(this.dtpValidTill);
            this.gbCardInfo.Controls.Add(this.label6);
            this.gbCardInfo.Controls.Add(this.label5);
            this.gbCardInfo.Controls.Add(this.lnkSelectStaff);
            this.gbCardInfo.Controls.Add(this.lblGamebalance);
            this.gbCardInfo.Controls.Add(this.label9);
            this.gbCardInfo.Controls.Add(this.txtTechnicianName);
            this.gbCardInfo.Controls.Add(this.lblLastName);
            this.gbCardInfo.Controls.Add(this.txtCardNumber);
            this.gbCardInfo.Controls.Add(this.txtTechnicianLastName);
            this.gbCardInfo.Controls.Add(this.lblCardNumber);
            this.gbCardInfo.Controls.Add(this.lnkDeactivate);
            this.gbCardInfo.Controls.Add(this.lblStaffName);
            this.gbCardInfo.Controls.Add(this.cmbTechCards);
            this.gbCardInfo.Controls.Add(this.lblSelectStaff);
            this.gbCardInfo.Controls.Add(this.txtNotes);
            this.gbCardInfo.Controls.Add(this.label10);
            this.gbCardInfo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCardInfo.Location = new System.Drawing.Point(12, 12);
            this.gbCardInfo.Name = "gbCardInfo";
            this.gbCardInfo.Size = new System.Drawing.Size(426, 401);
            this.gbCardInfo.TabIndex = 88;
            this.gbCardInfo.TabStop = false;
            this.gbCardInfo.Text = "Create/Update Staff Card";
            // 
            // txtCredits
            // 
            this.txtCredits.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtCredits.Enabled = false;
            this.txtCredits.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCredits.Location = new System.Drawing.Point(294, 216);
            this.txtCredits.Name = "txtCredits";
            this.txtCredits.Size = new System.Drawing.Size(99, 25);
            this.txtCredits.TabIndex = 104;
            this.txtCredits.Text = "0";
            // 
            // txtTickets
            // 
            this.txtTickets.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtTickets.Enabled = false;
            this.txtTickets.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTickets.Location = new System.Drawing.Point(115, 216);
            this.txtTickets.Name = "txtTickets";
            this.txtTickets.Size = new System.Drawing.Size(99, 25);
            this.txtTickets.TabIndex = 103;
            this.txtTickets.Text = "0";
            // 
            // txtBalanceGames
            // 
            this.txtBalanceGames.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtBalanceGames.Enabled = false;
            this.txtBalanceGames.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBalanceGames.Location = new System.Drawing.Point(115, 177);
            this.txtBalanceGames.Name = "txtBalanceGames";
            this.txtBalanceGames.Size = new System.Drawing.Size(278, 25);
            this.txtBalanceGames.TabIndex = 102;
            this.txtBalanceGames.Text = "0";
            // 
            // lblProduct
            // 
            this.lblProduct.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProduct.Location = new System.Drawing.Point(15, 290);
            this.lblProduct.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(98, 17);
            this.lblProduct.TabIndex = 101;
            this.lblProduct.Text = "Product Name: ";
            this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProduct
            // 
            this.txtProduct.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtProduct.Enabled = false;
            this.txtProduct.Location = new System.Drawing.Point(115, 289);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(278, 22);
            this.txtProduct.TabIndex = 11;
            // 
            // lnkSelectStaff
            // 
            this.lnkSelectStaff.AutoSize = true;
            this.lnkSelectStaff.Enabled = false;
            this.lnkSelectStaff.Location = new System.Drawing.Point(318, 112);
            this.lnkSelectStaff.Name = "lnkSelectStaff";
            this.lnkSelectStaff.Size = new System.Drawing.Size(81, 16);
            this.lnkSelectStaff.TabIndex = 5;
            this.lnkSelectStaff.TabStop = true;
            this.lnkSelectStaff.Text = "Select Staff";
            this.lnkSelectStaff.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectStaff_LinkClicked);
            // 
            // lblGamebalance
            // 
            this.lblGamebalance.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGamebalance.Location = new System.Drawing.Point(5, 178);
            this.lblGamebalance.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblGamebalance.Name = "lblGamebalance";
            this.lblGamebalance.Size = new System.Drawing.Size(109, 20);
            this.lblGamebalance.TabIndex = 88;
            this.lblGamebalance.Text = "Balance Games:";
            this.lblGamebalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmTechCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Ivory;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(900, 481);
            this.Controls.Add(this.gbCardInfo);
            this.Controls.Add(this.gbProducts);
            this.Controls.Add(this.lnkAdvanced);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmTechCard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Staff Card Management";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTechCard_FormClosed);
            this.gbProducts.ResumeLayout(false);
            this.flpStaffCardProducts.ResumeLayout(false);
            this.gbCardInfo.ResumeLayout(false);
            this.gbCardInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblStaffName;
        private System.Windows.Forms.TextBox txtTechnicianName;
        private System.Windows.Forms.DateTimePicker dtpValidTill;
        private System.Windows.Forms.ComboBox cmbTechCards;
        private System.Windows.Forms.Label lblSelectStaff;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel lnkAdvanced;
        private System.Windows.Forms.GroupBox gbProducts;
        private System.Windows.Forms.LinkLabel lnkDeactivate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTechnicianLastName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.GroupBox gbCardInfo;
        private System.Windows.Forms.LinkLabel lnkSelectStaff;
        private System.Windows.Forms.Label lblGamebalance;
        private System.Windows.Forms.FlowLayoutPanel flpStaffCardProducts;
        private System.Windows.Forms.Button btnSample;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.TextBox txtBalanceGames;
        private System.Windows.Forms.TextBox txtTickets;
        private System.Windows.Forms.TextBox txtCredits;
    }
}