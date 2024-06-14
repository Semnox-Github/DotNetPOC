namespace Parafait_POS.Reservation
{
    partial class frmInputPhysicalCards
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInputPhysicalCards));
            this.dgvMultipleCards = new System.Windows.Forms.DataGridView();
            this.SerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Card_Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxMessageLine = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCardCount = new System.Windows.Forms.Label();
            this.lblEntitlement = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLoadCardSerialMapping = new System.Windows.Forms.Button();
            this.txtToCardSerialNumber = new System.Windows.Forms.TextBox();
            this.lblToSerialNo = new System.Windows.Forms.Label();
            this.txtFromCardSerialNumber = new System.Windows.Forms.TextBox();
            this.lblFromSerialNo = new System.Windows.Forms.Label();
            this.lblMapCardsTo = new System.Windows.Forms.Label();
            this.btnRemoveCardFromLine = new System.Windows.Forms.Button();
            this.cmbCardProducts = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.btnSwap = new System.Windows.Forms.Button();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMultipleCards)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMultipleCards
            // 
            this.dgvMultipleCards.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvMultipleCards.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMultipleCards.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMultipleCards.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvMultipleCards.ColumnHeadersHeight = 31;
            this.dgvMultipleCards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMultipleCards.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SerialNumber,
            this.dcProductName,
            this.dcQuantity,
            this.Card_Number});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMultipleCards.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvMultipleCards.EnableHeadersVisualStyles = false;
            this.dgvMultipleCards.Location = new System.Drawing.Point(34, 147);
            this.dgvMultipleCards.Name = "dgvMultipleCards";
            this.dgvMultipleCards.ReadOnly = true;
            this.dgvMultipleCards.RowHeadersVisible = false;
            this.dgvMultipleCards.RowTemplate.Height = 31;
            this.dgvMultipleCards.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvMultipleCards.Size = new System.Drawing.Size(446, 322);
            this.dgvMultipleCards.TabIndex = 1;
            this.dgvMultipleCards.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMultipleCards_CellClick);
            // 
            // SerialNumber
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.SerialNumber.DefaultCellStyle = dataGridViewCellStyle7;
            this.SerialNumber.HeaderText = "Serial#";
            this.SerialNumber.Name = "SerialNumber";
            this.SerialNumber.ReadOnly = true;
            this.SerialNumber.Width = 50;
            // 
            // dcProductName
            // 
            this.dcProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dcProductName.HeaderText = "Product Name";
            this.dcProductName.Name = "dcProductName";
            this.dcProductName.ReadOnly = true;
            // 
            // dcQuantity
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.dcQuantity.DefaultCellStyle = dataGridViewCellStyle8;
            this.dcQuantity.HeaderText = "Quantity";
            this.dcQuantity.Name = "dcQuantity";
            this.dcQuantity.ReadOnly = true;
            this.dcQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dcQuantity.Width = 80;
            // 
            // Card_Number
            // 
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Card_Number.DefaultCellStyle = dataGridViewCellStyle9;
            this.Card_Number.HeaderText = "Card Number";
            this.Card_Number.Name = "Card_Number";
            this.Card_Number.ReadOnly = true;
            this.Card_Number.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Card_Number.Width = 120;
            // 
            // textBoxMessageLine
            // 
            this.textBoxMessageLine.BackColor = System.Drawing.Color.PapayaWhip;
            this.textBoxMessageLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxMessageLine.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessageLine.ForeColor = System.Drawing.Color.Firebrick;
            this.textBoxMessageLine.Location = new System.Drawing.Point(0, 517);
            this.textBoxMessageLine.Name = "textBoxMessageLine";
            this.textBoxMessageLine.Size = new System.Drawing.Size(512, 26);
            this.textBoxMessageLine.TabIndex = 4;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.Location = new System.Drawing.Point(357, 475);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(82, 35);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.BackColor = System.Drawing.Color.Transparent;
            this.buttonOK.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.buttonOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonOK.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.buttonOK.FlatAppearance.BorderSize = 0;
            this.buttonOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.Color.White;
            this.buttonOK.Location = new System.Drawing.Point(39, 475);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(82, 35);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Card Count:";
            // 
            // lblCardCount
            // 
            this.lblCardCount.AutoSize = true;
            this.lblCardCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardCount.Location = new System.Drawing.Point(104, 38);
            this.lblCardCount.Name = "lblCardCount";
            this.lblCardCount.Size = new System.Drawing.Size(71, 15);
            this.lblCardCount.TabIndex = 8;
            this.lblCardCount.Text = "Card Count:";
            // 
            // lblEntitlement
            // 
            this.lblEntitlement.AutoSize = true;
            this.lblEntitlement.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntitlement.ForeColor = System.Drawing.Color.Red;
            this.lblEntitlement.Location = new System.Drawing.Point(15, 5);
            this.lblEntitlement.Name = "lblEntitlement";
            this.lblEntitlement.Size = new System.Drawing.Size(0, 31);
            this.lblEntitlement.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLoadCardSerialMapping);
            this.groupBox1.Controls.Add(this.txtToCardSerialNumber);
            this.groupBox1.Controls.Add(this.lblToSerialNo);
            this.groupBox1.Controls.Add(this.txtFromCardSerialNumber);
            this.groupBox1.Controls.Add(this.lblFromSerialNo);
            this.groupBox1.Location = new System.Drawing.Point(34, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(446, 47);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Numbers";
            // 
            // btnLoadCardSerialMapping
            // 
            this.btnLoadCardSerialMapping.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadCardSerialMapping.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLoadCardSerialMapping.BackgroundImage")));
            this.btnLoadCardSerialMapping.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLoadCardSerialMapping.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnLoadCardSerialMapping.FlatAppearance.BorderSize = 0;
            this.btnLoadCardSerialMapping.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnLoadCardSerialMapping.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLoadCardSerialMapping.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLoadCardSerialMapping.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadCardSerialMapping.ForeColor = System.Drawing.Color.White;
            this.btnLoadCardSerialMapping.Location = new System.Drawing.Point(357, 11);
            this.btnLoadCardSerialMapping.Name = "btnLoadCardSerialMapping";
            this.btnLoadCardSerialMapping.Size = new System.Drawing.Size(76, 34);
            this.btnLoadCardSerialMapping.TabIndex = 12;
            this.btnLoadCardSerialMapping.Text = "Load";
            this.btnLoadCardSerialMapping.UseVisualStyleBackColor = false;
            this.btnLoadCardSerialMapping.Click += new System.EventHandler(this.btnLoadCardSerialMapping_Click);
            // 
            // txtToCardSerialNumber
            // 
            this.txtToCardSerialNumber.Location = new System.Drawing.Point(220, 20);
            this.txtToCardSerialNumber.Name = "txtToCardSerialNumber";
            this.txtToCardSerialNumber.Size = new System.Drawing.Size(117, 20);
            this.txtToCardSerialNumber.TabIndex = 11;
            // 
            // lblToSerialNo
            // 
            this.lblToSerialNo.AutoSize = true;
            this.lblToSerialNo.Location = new System.Drawing.Point(194, 23);
            this.lblToSerialNo.Name = "lblToSerialNo";
            this.lblToSerialNo.Size = new System.Drawing.Size(23, 13);
            this.lblToSerialNo.TabIndex = 10;
            this.lblToSerialNo.Text = "To:";
            // 
            // txtFromCardSerialNumber
            // 
            this.txtFromCardSerialNumber.Location = new System.Drawing.Point(43, 20);
            this.txtFromCardSerialNumber.Name = "txtFromCardSerialNumber";
            this.txtFromCardSerialNumber.Size = new System.Drawing.Size(117, 20);
            this.txtFromCardSerialNumber.TabIndex = 9;
            // 
            // lblFromSerialNo
            // 
            this.lblFromSerialNo.AutoSize = true;
            this.lblFromSerialNo.Location = new System.Drawing.Point(7, 23);
            this.lblFromSerialNo.Name = "lblFromSerialNo";
            this.lblFromSerialNo.Size = new System.Drawing.Size(33, 13);
            this.lblFromSerialNo.TabIndex = 8;
            this.lblFromSerialNo.Text = "From:";
            // 
            // lblMapCardsTo
            // 
            this.lblMapCardsTo.Location = new System.Drawing.Point(4, 57);
            this.lblMapCardsTo.Name = "lblMapCardsTo";
            this.lblMapCardsTo.Size = new System.Drawing.Size(100, 30);
            this.lblMapCardsTo.TabIndex = 110;
            this.lblMapCardsTo.Text = "Map Card To:";
            this.lblMapCardsTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRemoveCardFromLine
            // 
            this.btnRemoveCardFromLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveCardFromLine.BackColor = System.Drawing.Color.Transparent;
            this.btnRemoveCardFromLine.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnRemoveCardFromLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRemoveCardFromLine.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnRemoveCardFromLine.FlatAppearance.BorderSize = 0;
            this.btnRemoveCardFromLine.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRemoveCardFromLine.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRemoveCardFromLine.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRemoveCardFromLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveCardFromLine.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveCardFromLine.ForeColor = System.Drawing.Color.White;
            this.btnRemoveCardFromLine.Location = new System.Drawing.Point(145, 475);
            this.btnRemoveCardFromLine.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRemoveCardFromLine.Name = "btnRemoveCardFromLine";
            this.btnRemoveCardFromLine.Size = new System.Drawing.Size(82, 35);
            this.btnRemoveCardFromLine.TabIndex = 112;
            this.btnRemoveCardFromLine.Text = "Remove";
            this.btnRemoveCardFromLine.UseVisualStyleBackColor = false;
            this.btnRemoveCardFromLine.Click += new System.EventHandler(this.btnRemoveCardFromLine_Click);
            // 
            // cmbCardProducts
            // 
            this.cmbCardProducts.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCardProducts.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCardProducts.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCardProducts.FormattingEnabled = true;
            this.cmbCardProducts.Items.AddRange(new object[] {
            "Facility",
            "Game"});
            this.cmbCardProducts.Location = new System.Drawing.Point(107, 57);
            this.cmbCardProducts.Name = "cmbCardProducts";
            this.cmbCardProducts.Size = new System.Drawing.Size(373, 31);
            this.cmbCardProducts.TabIndex = 111;
            this.cmbCardProducts.SelectedIndexChanged += new System.EventHandler(this.cmbCardProducts_SelectedIndexChanged);
            // 
            // btnSwap
            // 
            this.btnSwap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSwap.BackColor = System.Drawing.Color.Transparent;
            this.btnSwap.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSwap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSwap.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnSwap.FlatAppearance.BorderSize = 0;
            this.btnSwap.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSwap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSwap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSwap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwap.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwap.ForeColor = System.Drawing.Color.White;
            this.btnSwap.Location = new System.Drawing.Point(251, 475);
            this.btnSwap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSwap.Name = "btnSwap";
            this.btnSwap.Size = new System.Drawing.Size(82, 35);
            this.btnSwap.TabIndex = 113;
            this.btnSwap.Text = "Swap";
            this.btnSwap.UseVisualStyleBackColor = false;
            this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnShowKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Location = new System.Drawing.Point(463, 472);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(36, 41);
            this.btnShowKeyPad.TabIndex = 114;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            // 
            // frmInputPhysicalCards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(512, 543);
            this.Controls.Add(this.btnShowKeyPad);
            this.Controls.Add(this.btnSwap);
            this.Controls.Add(this.btnRemoveCardFromLine);
            this.Controls.Add(this.cmbCardProducts);
            this.Controls.Add(this.lblMapCardsTo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblEntitlement);
            this.Controls.Add(this.lblCardCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxMessageLine);
            this.Controls.Add(this.dgvMultipleCards);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmInputPhysicalCards";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Input Cards";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInputPhysicalCards_FormClosed);
            this.Load += new System.EventHandler(this.frmInputPhysicalCards_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMultipleCards)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMultipleCards;
        private System.Windows.Forms.TextBox textBoxMessageLine;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCardCount;
        private System.Windows.Forms.Label lblEntitlement;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLoadCardSerialMapping;
        private System.Windows.Forms.TextBox txtToCardSerialNumber;
        private System.Windows.Forms.Label lblToSerialNo;
        private System.Windows.Forms.TextBox txtFromCardSerialNumber;
        private System.Windows.Forms.Label lblFromSerialNo;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbCardProducts;
        private System.Windows.Forms.Label lblMapCardsTo;
        private System.Windows.Forms.Button btnRemoveCardFromLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Card_Number;
        private System.Windows.Forms.Button btnSwap;
        private System.Windows.Forms.Button btnShowKeyPad;
    }
}