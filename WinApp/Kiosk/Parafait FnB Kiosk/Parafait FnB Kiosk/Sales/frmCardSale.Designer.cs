namespace Parafait_FnB_Kiosk
{
    partial class frmCardSale
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
            this.panelBG = new System.Windows.Forms.Panel();
            this.flpCardCount = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDigit1 = new System.Windows.Forms.Label();
            this.lblDigit2 = new System.Windows.Forms.Label();
            this.lblDigit3 = new System.Windows.Forms.Label();
            this.lblDigit4 = new System.Windows.Forms.Label();
            this.lblDigit5 = new System.Windows.Forms.Label();
            this.lblDigit6 = new System.Windows.Forms.Label();
            this.panelOtherNumber = new System.Windows.Forms.Panel();
            this.lblOtherNumber = new System.Windows.Forms.Label();
            this.cmbOtherNumber = new System.Windows.Forms.ComboBox();
            this.lblExistingCard = new System.Windows.Forms.Label();
            this.pnlsuggestiveSale = new System.Windows.Forms.Panel();
            this.lblEachWristBand = new System.Windows.Forms.Label();
            this.panelQty = new System.Windows.Forms.Panel();
            this.lblComboWBCount = new System.Windows.Forms.Label();
            this.cmbWBCount = new System.Windows.Forms.ComboBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblScreenTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelConfirm = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.lblEachCardMessage = new System.Windows.Forms.Label();
            this.panelBG.SuspendLayout();
            this.flpCardCount.SuspendLayout();
            this.panelOtherNumber.SuspendLayout();
            this.panelQty.SuspendLayout();
            this.panelConfirm.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBG
            // 
            this.panelBG.BackColor = System.Drawing.Color.Transparent;
            this.panelBG.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Pop_up_Purple;
            this.panelBG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelBG.Controls.Add(this.flpCardCount);
            this.panelBG.Controls.Add(this.lblScreenTitle);
            this.panelBG.Controls.Add(this.btnClose);
            this.panelBG.Controls.Add(this.panelConfirm);
            this.panelBG.Controls.Add(this.lblEachCardMessage);
            this.panelBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBG.Location = new System.Drawing.Point(0, 0);
            this.panelBG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelBG.Name = "panelBG";
            this.panelBG.Size = new System.Drawing.Size(955, 1676);
            this.panelBG.TabIndex = 5;
            // 
            // flpCardCount
            // 
            this.flpCardCount.Controls.Add(this.lblDigit1);
            this.flpCardCount.Controls.Add(this.lblDigit2);
            this.flpCardCount.Controls.Add(this.lblDigit3);
            this.flpCardCount.Controls.Add(this.lblDigit4);
            this.flpCardCount.Controls.Add(this.lblDigit5);
            this.flpCardCount.Controls.Add(this.lblDigit6);
            this.flpCardCount.Controls.Add(this.panelOtherNumber);
            this.flpCardCount.Controls.Add(this.lblExistingCard);
            this.flpCardCount.Controls.Add(this.pnlsuggestiveSale);
            this.flpCardCount.Controls.Add(this.lblEachWristBand);
            this.flpCardCount.Controls.Add(this.panelQty);
            this.flpCardCount.Controls.Add(this.lblTotal);
            this.flpCardCount.Controls.Add(this.lblAmount);
            this.flpCardCount.Location = new System.Drawing.Point(110, 190);
            this.flpCardCount.Margin = new System.Windows.Forms.Padding(260, 3, 3, 3);
            this.flpCardCount.Name = "flpCardCount";
            this.flpCardCount.Size = new System.Drawing.Size(738, 1313);
            this.flpCardCount.TabIndex = 10;
            // 
            // lblDigit1
            // 
            this.lblDigit1.Font = new System.Drawing.Font("Bango Pro", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit1.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Keypad__Key;
            this.lblDigit1.Location = new System.Drawing.Point(15, 15);
            this.lblDigit1.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit1.Name = "lblDigit1";
            this.lblDigit1.Size = new System.Drawing.Size(213, 246);
            this.lblDigit1.TabIndex = 8;
            this.lblDigit1.Text = "1";
            this.lblDigit1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit1.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit2
            // 
            this.lblDigit2.Font = new System.Drawing.Font("Bango Pro", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit2.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Keypad__Key;
            this.lblDigit2.Location = new System.Drawing.Point(258, 15);
            this.lblDigit2.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit2.Name = "lblDigit2";
            this.lblDigit2.Size = new System.Drawing.Size(213, 246);
            this.lblDigit2.TabIndex = 9;
            this.lblDigit2.Text = "2";
            this.lblDigit2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit2.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit3
            // 
            this.lblDigit3.Font = new System.Drawing.Font("Bango Pro", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit3.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Keypad__Key;
            this.lblDigit3.Location = new System.Drawing.Point(501, 15);
            this.lblDigit3.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit3.Name = "lblDigit3";
            this.lblDigit3.Size = new System.Drawing.Size(213, 246);
            this.lblDigit3.TabIndex = 10;
            this.lblDigit3.Text = "3";
            this.lblDigit3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit3.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit4
            // 
            this.lblDigit4.Font = new System.Drawing.Font("Bango Pro", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit4.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Keypad__Key;
            this.lblDigit4.Location = new System.Drawing.Point(15, 291);
            this.lblDigit4.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit4.Name = "lblDigit4";
            this.lblDigit4.Size = new System.Drawing.Size(213, 246);
            this.lblDigit4.TabIndex = 11;
            this.lblDigit4.Text = "4";
            this.lblDigit4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit4.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit5
            // 
            this.lblDigit5.Font = new System.Drawing.Font("Bango Pro", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit5.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Keypad__Key;
            this.lblDigit5.Location = new System.Drawing.Point(258, 291);
            this.lblDigit5.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit5.Name = "lblDigit5";
            this.lblDigit5.Size = new System.Drawing.Size(213, 246);
            this.lblDigit5.TabIndex = 12;
            this.lblDigit5.Text = "5";
            this.lblDigit5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit5.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit6
            // 
            this.lblDigit6.Font = new System.Drawing.Font("Bango Pro", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit6.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Keypad__Key;
            this.lblDigit6.Location = new System.Drawing.Point(501, 291);
            this.lblDigit6.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit6.Name = "lblDigit6";
            this.lblDigit6.Size = new System.Drawing.Size(213, 246);
            this.lblDigit6.TabIndex = 13;
            this.lblDigit6.Text = "6";
            this.lblDigit6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit6.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // panelOtherNumber
            // 
            this.panelOtherNumber.Controls.Add(this.lblOtherNumber);
            this.panelOtherNumber.Controls.Add(this.cmbOtherNumber);
            this.panelOtherNumber.Location = new System.Drawing.Point(3, 555);
            this.panelOtherNumber.Name = "panelOtherNumber";
            this.panelOtherNumber.Size = new System.Drawing.Size(711, 120);
            this.panelOtherNumber.TabIndex = 22;
            // 
            // lblOtherNumber
            // 
            this.lblOtherNumber.Font = new System.Drawing.Font("Bango Pro", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOtherNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblOtherNumber.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Any_amount;
            this.lblOtherNumber.Location = new System.Drawing.Point(15, 0);
            this.lblOtherNumber.Margin = new System.Windows.Forms.Padding(15);
            this.lblOtherNumber.Name = "lblOtherNumber";
            this.lblOtherNumber.Size = new System.Drawing.Size(699, 112);
            this.lblOtherNumber.TabIndex = 21;
            this.lblOtherNumber.Text = "Other number of Cards";
            this.lblOtherNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblOtherNumber.Click += new System.EventHandler(this.lblOtherNumber_Click);
            // 
            // cmbOtherNumber
            // 
            this.cmbOtherNumber.BackColor = System.Drawing.Color.White;
            this.cmbOtherNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbOtherNumber.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOtherNumber.FormattingEnabled = true;
            this.cmbOtherNumber.Location = new System.Drawing.Point(132, 11);
            this.cmbOtherNumber.MaxDropDownItems = 12;
            this.cmbOtherNumber.Name = "cmbOtherNumber";
            this.cmbOtherNumber.Size = new System.Drawing.Size(446, 85);
            this.cmbOtherNumber.TabIndex = 22;
            this.cmbOtherNumber.SelectedIndexChanged += new System.EventHandler(this.cmbOtherNumber_SelectedIndexChanged);
            // 
            // lblExistingCard
            // 
            this.lblExistingCard.Font = new System.Drawing.Font("Bango Pro", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExistingCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblExistingCard.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Any_amount;
            this.lblExistingCard.Location = new System.Drawing.Point(15, 693);
            this.lblExistingCard.Margin = new System.Windows.Forms.Padding(15);
            this.lblExistingCard.Name = "lblExistingCard";
            this.lblExistingCard.Size = new System.Drawing.Size(699, 112);
            this.lblExistingCard.TabIndex = 20;
            this.lblExistingCard.Text = "Add points to an existing card";
            this.lblExistingCard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblExistingCard.Click += new System.EventHandler(this.lblExistingCard_Click);
            // 
            // pnlsuggestiveSale
            // 
            this.pnlsuggestiveSale.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlsuggestiveSale.Location = new System.Drawing.Point(3, 823);
            this.pnlsuggestiveSale.Name = "pnlsuggestiveSale";
            this.pnlsuggestiveSale.Size = new System.Drawing.Size(735, 337);
            this.pnlsuggestiveSale.TabIndex = 23;
            // 
            // lblEachWristBand
            // 
            this.lblEachWristBand.Font = new System.Drawing.Font("Bango Pro", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEachWristBand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(189)))), ((int)(((byte)(34)))));
            this.lblEachWristBand.Location = new System.Drawing.Point(0, 1163);
            this.lblEachWristBand.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblEachWristBand.Name = "lblEachWristBand";
            this.lblEachWristBand.Size = new System.Drawing.Size(733, 46);
            this.lblEachWristBand.TabIndex = 24;
            this.lblEachWristBand.Text = "Add a wristband for $1 each.";
            this.lblEachWristBand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelQty
            // 
            this.panelQty.Controls.Add(this.lblComboWBCount);
            this.panelQty.Controls.Add(this.cmbWBCount);
            this.panelQty.Location = new System.Drawing.Point(3, 1212);
            this.panelQty.Name = "panelQty";
            this.panelQty.Size = new System.Drawing.Size(730, 56);
            this.panelQty.TabIndex = 27;
            // 
            // lblComboWBCount
            // 
            this.lblComboWBCount.Font = new System.Drawing.Font("VAG Rounded", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComboWBCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblComboWBCount.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Dropdown_Btn_11;
            this.lblComboWBCount.Location = new System.Drawing.Point(243, 5);
            this.lblComboWBCount.Name = "lblComboWBCount";
            this.lblComboWBCount.Size = new System.Drawing.Size(214, 47);
            this.lblComboWBCount.TabIndex = 25;
            this.lblComboWBCount.Text = "0";
            this.lblComboWBCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblComboWBCount.Click += new System.EventHandler(this.lblComboWBCount_Click);
            // 
            // cmbWBCount
            // 
            this.cmbWBCount.BackColor = System.Drawing.Color.White;
            this.cmbWBCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWBCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbWBCount.Font = new System.Drawing.Font("VAG Rounded", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbWBCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.cmbWBCount.FormattingEnabled = true;
            this.cmbWBCount.IntegralHeight = false;
            this.cmbWBCount.ItemHeight = 33;
            this.cmbWBCount.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbWBCount.Location = new System.Drawing.Point(332, 9);
            this.cmbWBCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbWBCount.MaxDropDownItems = 11;
            this.cmbWBCount.Name = "cmbWBCount";
            this.cmbWBCount.Size = new System.Drawing.Size(96, 41);
            this.cmbWBCount.TabIndex = 26;
            this.cmbWBCount.SelectedIndexChanged += new System.EventHandler(this.cmbWBCount_SelectedIndexChanged);
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Bango Pro", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.White;
            this.lblTotal.Location = new System.Drawing.Point(0, 1271);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(368, 46);
            this.lblTotal.TabIndex = 28;
            this.lblTotal.Text = "Total: ";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAmount
            // 
            this.lblAmount.Font = new System.Drawing.Font("Bango Pro", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmount.ForeColor = System.Drawing.Color.White;
            this.lblAmount.Location = new System.Drawing.Point(371, 1271);
            this.lblAmount.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(269, 46);
            this.lblAmount.TabIndex = 29;
            this.lblAmount.Text = "$0.0";
            this.lblAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScreenTitle
            // 
            this.lblScreenTitle.Font = new System.Drawing.Font("Bango Pro", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenTitle.ForeColor = System.Drawing.Color.White;
            this.lblScreenTitle.Location = new System.Drawing.Point(145, 68);
            this.lblScreenTitle.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblScreenTitle.Name = "lblScreenTitle";
            this.lblScreenTitle.Size = new System.Drawing.Size(664, 61);
            this.lblScreenTitle.TabIndex = 5;
            this.lblScreenTitle.Text = "How Many Cards?";
            this.lblScreenTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Close_Btn;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(819, 54);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(76, 76);
            this.btnClose.TabIndex = 0;
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // panelConfirm
            // 
            this.panelConfirm.Controls.Add(this.btnCancel);
            this.panelConfirm.Controls.Add(this.btnConfirm);
            this.panelConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelConfirm.Location = new System.Drawing.Point(0, 1511);
            this.panelConfirm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelConfirm.Name = "panelConfirm";
            this.panelConfirm.Size = new System.Drawing.Size(955, 165);
            this.panelConfirm.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Green_Btn;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.btnCancel.Location = new System.Drawing.Point(71, 40);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(346, 68);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Green_Btn;
            this.btnConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.btnConfirm.Location = new System.Drawing.Point(549, 40);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(346, 68);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // lblEachCardMessage
            // 
            this.lblEachCardMessage.Font = new System.Drawing.Font("Bango Pro", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEachCardMessage.ForeColor = System.Drawing.Color.White;
            this.lblEachCardMessage.Location = new System.Drawing.Point(0, 132);
            this.lblEachCardMessage.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblEachCardMessage.Name = "lblEachCardMessage";
            this.lblEachCardMessage.Size = new System.Drawing.Size(955, 46);
            this.lblEachCardMessage.TabIndex = 19;
            this.lblEachCardMessage.Text = "Points will be equally divided onto each card.";
            this.lblEachCardMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmCardSale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Turquoise;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(955, 1676);
            this.Controls.Add(this.panelBG);
            this.DoubleBuffered = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Name = "frmCardSale";
            this.Text = "BaseFormProductSale";
            this.TransparencyKey = System.Drawing.Color.Turquoise;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.Load += new System.EventHandler(this.frmCardSale_Load);
            this.panelBG.ResumeLayout(false);
            this.flpCardCount.ResumeLayout(false);
            this.panelOtherNumber.ResumeLayout(false);
            this.panelQty.ResumeLayout(false);
            this.panelConfirm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panelBG;
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Panel panelConfirm;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label lblScreenTitle;
        private System.Windows.Forms.FlowLayoutPanel flpCardCount;
        private System.Windows.Forms.Label lblDigit1;
        private System.Windows.Forms.Label lblDigit2;
        private System.Windows.Forms.Label lblDigit3;
        private System.Windows.Forms.Label lblDigit4;
        private System.Windows.Forms.Label lblDigit5;
        private System.Windows.Forms.Label lblDigit6;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblEachCardMessage;
        private System.Windows.Forms.Label lblExistingCard;
        private System.Windows.Forms.Label lblOtherNumber;
        private System.Windows.Forms.Panel panelOtherNumber;
        private System.Windows.Forms.ComboBox cmbOtherNumber;
        private System.Windows.Forms.Panel pnlsuggestiveSale;
        private System.Windows.Forms.Label lblEachWristBand;
        private System.Windows.Forms.Label lblComboWBCount;
        private System.Windows.Forms.Panel panelQty;
        private System.Windows.Forms.ComboBox cmbWBCount;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblAmount;
    }
}