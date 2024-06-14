namespace Parafait_POS
{
    partial class frmParentChildCards
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.dgvParentCard = new System.Windows.Forms.DataGridView();
            this.ParentCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Credits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreditPlus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParentCardId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvChildCards = new System.Windows.Forms.DataGridView();
            this.ParentChildCardId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChildCardId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChildCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChildCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcCreditPlus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActiveFlag = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DayLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSplitEqual = new System.Windows.Forms.Button();
            this.btnFirstCardHalf = new System.Windows.Forms.Button();
            this.btnCustomFirstCard = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.lblCustomFirstCard = new System.Windows.Forms.Label();
            this.panelCustomFirstCard = new System.Windows.Forms.Panel();
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
            this.btnClear_Perc = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParentCard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChildCards)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.NavajoWhite;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 426);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(814, 26);
            this.txtMessage.TabIndex = 0;
            // 
            // dgvParentCard
            // 
            this.dgvParentCard.AllowUserToAddRows = false;
            this.dgvParentCard.AllowUserToDeleteRows = false;
            this.dgvParentCard.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvParentCard.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvParentCard.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvParentCard.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Aquamarine;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvParentCard.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvParentCard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParentCard.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParentCardNumber,
            this.Customer,
            this.Credits,
            this.CreditPlus,
            this.ParentCardId});
            this.dgvParentCard.EnableHeadersVisualStyles = false;
            this.dgvParentCard.Location = new System.Drawing.Point(13, 33);
            this.dgvParentCard.Name = "dgvParentCard";
            this.dgvParentCard.ReadOnly = true;
            this.dgvParentCard.RowHeadersVisible = false;
            this.dgvParentCard.Size = new System.Drawing.Size(582, 45);
            this.dgvParentCard.TabIndex = 1;
            // 
            // ParentCardNumber
            // 
            this.ParentCardNumber.HeaderText = "Card Number";
            this.ParentCardNumber.Name = "ParentCardNumber";
            this.ParentCardNumber.ReadOnly = true;
            // 
            // Customer
            // 
            this.Customer.HeaderText = "Customer";
            this.Customer.Name = "Customer";
            this.Customer.ReadOnly = true;
            // 
            // Credits
            // 
            this.Credits.HeaderText = "Credits";
            this.Credits.Name = "Credits";
            this.Credits.ReadOnly = true;
            // 
            // CreditPlus
            // 
            this.CreditPlus.HeaderText = "Credit Plus";
            this.CreditPlus.Name = "CreditPlus";
            this.CreditPlus.ReadOnly = true;
            // 
            // ParentCardId
            // 
            this.ParentCardId.HeaderText = "ParentCardId";
            this.ParentCardId.Name = "ParentCardId";
            this.ParentCardId.ReadOnly = true;
            this.ParentCardId.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Parent Card:";
            // 
            // dgvChildCards
            // 
            this.dgvChildCards.AllowUserToAddRows = false;
            this.dgvChildCards.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChildCards.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvChildCards.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvChildCards.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Aquamarine;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvChildCards.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvChildCards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChildCards.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParentChildCardId,
            this.ChildCardId,
            this.ChildCardNumber,
            this.ChildCustomer,
            this.dcCredits,
            this.dcCreditPlus,
            this.ActiveFlag,
            this.DayLimit});
            this.dgvChildCards.EnableHeadersVisualStyles = false;
            this.dgvChildCards.Location = new System.Drawing.Point(13, 114);
            this.dgvChildCards.Name = "dgvChildCards";
            this.dgvChildCards.RowHeadersVisible = false;
            this.dgvChildCards.Size = new System.Drawing.Size(582, 264);
            this.dgvChildCards.TabIndex = 3;
            this.dgvChildCards.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvChildCards_CellMouseUp);
            this.dgvChildCards.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChildCards_CellValueChanged);
            // 
            // ParentChildCardId
            // 
            this.ParentChildCardId.HeaderText = "ParentChildCardId";
            this.ParentChildCardId.Name = "ParentChildCardId";
            this.ParentChildCardId.Visible = false;
            // 
            // ChildCardId
            // 
            this.ChildCardId.HeaderText = "ChildCardId";
            this.ChildCardId.Name = "ChildCardId";
            this.ChildCardId.Visible = false;
            // 
            // ChildCardNumber
            // 
            this.ChildCardNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ChildCardNumber.FillWeight = 106.599F;
            this.ChildCardNumber.HeaderText = "Child Card Number";
            this.ChildCardNumber.Name = "ChildCardNumber";
            this.ChildCardNumber.ReadOnly = true;
            this.ChildCardNumber.Width = 160;
            // 
            // ChildCustomer
            // 
            this.ChildCustomer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ChildCustomer.FillWeight = 96.70051F;
            this.ChildCustomer.HeaderText = "Customer";
            this.ChildCustomer.Name = "ChildCustomer";
            this.ChildCustomer.ReadOnly = true;
            // 
            // dcCredits
            // 
            this.dcCredits.HeaderText = "Credits";
            this.dcCredits.Name = "dcCredits";
            this.dcCredits.ReadOnly = true;
            // 
            // dcCreditPlus
            // 
            this.dcCreditPlus.HeaderText = "Credit Plus";
            this.dcCreditPlus.Name = "dcCreditPlus";
            this.dcCreditPlus.ReadOnly = true;
            // 
            // ActiveFlag
            // 
            this.ActiveFlag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ActiveFlag.FalseValue = "false";
            this.ActiveFlag.FillWeight = 96.70051F;
            this.ActiveFlag.HeaderText = "Associated?";
            this.ActiveFlag.Name = "ActiveFlag";
            this.ActiveFlag.TrueValue = "true";
            this.ActiveFlag.Width = 90;
            // 
            // DayLimit
            // 
            this.DayLimit.HeaderText = "Day Limit % ";
            this.DayLimit.Name = "DayLimit";
            this.DayLimit.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Child Cards:";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(456, 390);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(139, 32);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(13, 390);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(139, 32);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(234, 390);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(139, 32);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSplitEqual
            // 
            this.btnSplitEqual.BackColor = System.Drawing.Color.White;
            this.btnSplitEqual.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnSplitEqual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSplitEqual.FlatAppearance.BorderSize = 0;
            this.btnSplitEqual.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSplitEqual.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSplitEqual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSplitEqual.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSplitEqual.ForeColor = System.Drawing.Color.White;
            this.btnSplitEqual.Location = new System.Drawing.Point(627, 114);
            this.btnSplitEqual.Name = "btnSplitEqual";
            this.btnSplitEqual.Size = new System.Drawing.Size(160, 50);
            this.btnSplitEqual.TabIndex = 11;
            this.btnSplitEqual.Text = "Split Equal";
            this.btnSplitEqual.UseVisualStyleBackColor = false;
            this.btnSplitEqual.Click += new System.EventHandler(this.btnSplitEqual_Click);
            // 
            // btnFirstCardHalf
            // 
            this.btnFirstCardHalf.BackColor = System.Drawing.Color.White;
            this.btnFirstCardHalf.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnFirstCardHalf.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFirstCardHalf.FlatAppearance.BorderSize = 0;
            this.btnFirstCardHalf.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFirstCardHalf.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFirstCardHalf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFirstCardHalf.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirstCardHalf.ForeColor = System.Drawing.Color.White;
            this.btnFirstCardHalf.Location = new System.Drawing.Point(627, 172);
            this.btnFirstCardHalf.Name = "btnFirstCardHalf";
            this.btnFirstCardHalf.Size = new System.Drawing.Size(160, 50);
            this.btnFirstCardHalf.TabIndex = 12;
            this.btnFirstCardHalf.Text = "50% First Card";
            this.btnFirstCardHalf.UseVisualStyleBackColor = false;
            this.btnFirstCardHalf.Click += new System.EventHandler(this.btnFirstCardHalf_Click);
            // 
            // btnCustomFirstCard
            // 
            this.btnCustomFirstCard.BackColor = System.Drawing.Color.White;
            this.btnCustomFirstCard.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnCustomFirstCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCustomFirstCard.FlatAppearance.BorderSize = 0;
            this.btnCustomFirstCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCustomFirstCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCustomFirstCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomFirstCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomFirstCard.ForeColor = System.Drawing.Color.White;
            this.btnCustomFirstCard.Location = new System.Drawing.Point(627, 231);
            this.btnCustomFirstCard.Name = "btnCustomFirstCard";
            this.btnCustomFirstCard.Size = new System.Drawing.Size(160, 50);
            this.btnCustomFirstCard.TabIndex = 13;
            this.btnCustomFirstCard.Text = "Custom First Card";
            this.btnCustomFirstCard.UseVisualStyleBackColor = false;
            this.btnCustomFirstCard.Click += new System.EventHandler(this.btnCustomFirstCard_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.BackColor = System.Drawing.Color.White;
            this.btnPlus.BackgroundImage = global::Parafait_POS.Properties.Resources.button_normal;
            this.btnPlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlus.FlatAppearance.BorderSize = 0;
            this.btnPlus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPlus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.btnPlus.ForeColor = System.Drawing.Color.White;
            this.btnPlus.Location = new System.Drawing.Point(743, 323);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(50, 48);
            this.btnPlus.TabIndex = 14;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = false;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.BackColor = System.Drawing.Color.White;
            this.btnMinus.BackgroundImage = global::Parafait_POS.Properties.Resources.button_normal;
            this.btnMinus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMinus.FlatAppearance.BorderSize = 0;
            this.btnMinus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMinus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.btnMinus.ForeColor = System.Drawing.Color.White;
            this.btnMinus.Location = new System.Drawing.Point(620, 323);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(50, 48);
            this.btnMinus.TabIndex = 15;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = false;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // lblCustomFirstCard
            // 
            this.lblCustomFirstCard.BackColor = System.Drawing.Color.White;
            this.lblCustomFirstCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomFirstCard.Location = new System.Drawing.Point(673, 323);
            this.lblCustomFirstCard.Name = "lblCustomFirstCard";
            this.lblCustomFirstCard.Size = new System.Drawing.Size(68, 48);
            this.lblCustomFirstCard.TabIndex = 16;
            this.lblCustomFirstCard.Text = "10";
            this.lblCustomFirstCard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelCustomFirstCard
            // 
            this.panelCustomFirstCard.Location = new System.Drawing.Point(615, 316);
            this.panelCustomFirstCard.Name = "panelCustomFirstCard";
            this.panelCustomFirstCard.Size = new System.Drawing.Size(184, 61);
            this.panelCustomFirstCard.TabIndex = 17;
            this.panelCustomFirstCard.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Card Number";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 144;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Customer";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 145;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Credits";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 145;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Credit Plus";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 145;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "ParentCardId";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "ChildCardId";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn7.FillWeight = 106.599F;
            this.dataGridViewTextBoxColumn7.HeaderText = "Child Card Number";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 160;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.FillWeight = 96.70051F;
            this.dataGridViewTextBoxColumn8.HeaderText = "Customer";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Credits";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 83;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Credit Plus";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Width = 83;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "Day Limit % ";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Width = 83;
            // 
            // btnClear_Perc
            // 
            this.btnClear_Perc.BackColor = System.Drawing.Color.White;
            this.btnClear_Perc.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnClear_Perc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClear_Perc.FlatAppearance.BorderSize = 0;
            this.btnClear_Perc.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClear_Perc.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClear_Perc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear_Perc.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear_Perc.ForeColor = System.Drawing.Color.White;
            this.btnClear_Perc.Location = new System.Drawing.Point(627, 58);
            this.btnClear_Perc.Name = "btnClear_Perc";
            this.btnClear_Perc.Size = new System.Drawing.Size(160, 50);
            this.btnClear_Perc.TabIndex = 18;
            this.btnClear_Perc.Text = "Clear Split";
            this.btnClear_Perc.UseVisualStyleBackColor = false;
            this.btnClear_Perc.Click += new System.EventHandler(this.btnClear_Perc_Click);
            // 
            // frmParentChildCards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(814, 452);
            this.Controls.Add(this.btnClear_Perc);
            this.Controls.Add(this.btnPlus);
            this.Controls.Add(this.lblCustomFirstCard);
            this.Controls.Add(this.btnMinus);
            this.Controls.Add(this.panelCustomFirstCard);
            this.Controls.Add(this.btnCustomFirstCard);
            this.Controls.Add(this.btnFirstCardHalf);
            this.Controls.Add(this.btnSplitEqual);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvChildCards);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvParentCard);
            this.Controls.Add(this.txtMessage);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmParentChildCards";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parent-Child Cards";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmParentChildCards_FormClosed);
            this.Load += new System.EventHandler(this.frmParentChildCards_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvParentCard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChildCards)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.DataGridView dgvParentCard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentCardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Credits;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreditPlus;
        private System.Windows.Forms.DataGridView dgvChildCards;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentCardId;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSplitEqual;
        private System.Windows.Forms.Button btnFirstCardHalf;
        private System.Windows.Forms.Button btnCustomFirstCard;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Label lblCustomFirstCard;
        private System.Windows.Forms.Panel panelCustomFirstCard;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentChildCardId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChildCardId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChildCardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChildCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcCredits;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcCreditPlus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ActiveFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn DayLimit;
        private System.Windows.Forms.Button btnClear_Perc;
    }
}