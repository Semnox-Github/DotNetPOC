namespace Parafait_POS
{
    partial class TransactionWaiverList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.verticalScrollBarView2 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.dgvTrxDetails = new System.Windows.Forms.DataGridView();
            this.horizontalScrollBarView2 = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCustomFields = new System.Windows.Forms.ComboBox();
            this.lblCustomFields = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.lblPhoneNumber = new System.Windows.Forms.Label();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTrxId = new System.Windows.Forms.TextBox();
            this.lblTrxId = new System.Windows.Forms.Label();
            this.lblToDate = new System.Windows.Forms.Label();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxDetails)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnShowKeyPad);
            this.panel1.Controls.Add(this.verticalScrollBarView2);
            this.panel1.Controls.Add(this.horizontalScrollBarView2);
            this.panel1.Controls.Add(this.dgvTrxDetails);
            this.panel1.Location = new System.Drawing.Point(4, 192);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(765, 380);
            this.panel1.TabIndex = 1;
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnShowKeyPad.Location = new System.Drawing.Point(713, 330);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(38, 39);
            this.btnShowKeyPad.TabIndex = 33;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.BtnShowKeyPad_Click);
            // 
            // verticalScrollBarView2
            // 
            this.verticalScrollBarView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView2.AutoHide = false;
            this.verticalScrollBarView2.DataGridView = this.dgvTrxDetails;
            this.verticalScrollBarView2.Location = new System.Drawing.Point(713, 3);
            this.verticalScrollBarView2.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView2.Name = "verticalScrollBarView2";
            this.verticalScrollBarView2.ScrollableControl = null;
            this.verticalScrollBarView2.Size = new System.Drawing.Size(49, 324);
            this.verticalScrollBarView2.TabIndex = 8;
            // 
            // dgvTrxDetails
            // 
            this.dgvTrxDetails.AllowUserToAddRows = false;
            this.dgvTrxDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTrxDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTrxDetails.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTrxDetails.Location = new System.Drawing.Point(11, 3);
            this.dgvTrxDetails.Name = "dgvTrxDetails";
            this.dgvTrxDetails.ReadOnly = true;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTrxDetails.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTrxDetails.RowTemplate.Height = 30;
            this.dgvTrxDetails.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvTrxDetails.Size = new System.Drawing.Size(699, 324);
            this.dgvTrxDetails.TabIndex = 6;
            this.dgvTrxDetails.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrxDetails_CellClick);
            this.dgvTrxDetails.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvTrxDetails_CellMouseEnter);
            // 
            // horizontalScrollBarView2
            // 
            this.horizontalScrollBarView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBarView2.AutoHide = false;
            this.horizontalScrollBarView2.DataGridView = this.dgvTrxDetails;
            this.horizontalScrollBarView2.Location = new System.Drawing.Point(11, 330);
            this.horizontalScrollBarView2.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarView2.Name = "horizontalScrollBarView2";
            this.horizontalScrollBarView2.ScrollableControl = null;
            this.horizontalScrollBarView2.Size = new System.Drawing.Size(699, 40);
            this.horizontalScrollBarView2.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(4, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(765, 183);
            this.panel2.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpFrom);
            this.groupBox1.Controls.Add(this.dtpTo);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtCardNumber);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbCustomFields);
            this.groupBox1.Controls.Add(this.lblCustomFields);
            this.groupBox1.Controls.Add(this.txtValue);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtPhoneNumber);
            this.groupBox1.Controls.Add(this.lblPhoneNumber);
            this.groupBox1.Controls.Add(this.txtCustomerName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtTrxId);
            this.groupBox1.Controls.Add(this.lblTrxId);
            this.groupBox1.Controls.Add(this.lblToDate);
            this.groupBox1.Controls.Add(this.lblFromDate);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(11, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(751, 173);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Filters";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(116, 39);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(156, 22);
            this.dtpFrom.TabIndex = 20;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(361, 39);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(156, 22);
            this.dtpTo.TabIndex = 19;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Beige;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(632, 118);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 45);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.Beige;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(526, 118);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 45);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.Location = new System.Drawing.Point(622, 78);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(120, 22);
            this.txtCardNumber.TabIndex = 17;
            this.txtCardNumber.Enter += new System.EventHandler(this.TxtCardNumber_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(523, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "Card Number";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbCustomFields
            // 
            this.cmbCustomFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCustomFields.FormattingEnabled = true;
            this.cmbCustomFields.Location = new System.Drawing.Point(116, 129);
            this.cmbCustomFields.Name = "cmbCustomFields";
            this.cmbCustomFields.Size = new System.Drawing.Size(156, 24);
            this.cmbCustomFields.TabIndex = 15;
            // 
            // lblCustomFields
            // 
            this.lblCustomFields.AutoSize = true;
            this.lblCustomFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomFields.Location = new System.Drawing.Point(6, 133);
            this.lblCustomFields.Name = "lblCustomFields";
            this.lblCustomFields.Size = new System.Drawing.Size(93, 16);
            this.lblCustomFields.TabIndex = 14;
            this.lblCustomFields.Text = "Custom Fields";
            this.lblCustomFields.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtValue
            // 
            this.txtValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValue.Location = new System.Drawing.Point(361, 131);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(156, 22);
            this.txtValue.TabIndex = 13;
            this.txtValue.Enter += new System.EventHandler(this.TxtValue_Enter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(287, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 32);
            this.label7.TabIndex = 12;
            this.label7.Text = "Value\r\n\r\n";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhoneNumber.Location = new System.Drawing.Point(361, 81);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(156, 22);
            this.txtPhoneNumber.TabIndex = 9;
            this.txtPhoneNumber.Enter += new System.EventHandler(this.TxtPhoneNumber_Enter);
            this.txtPhoneNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPhoneNumber_KeyPress);
            // 
            // lblPhoneNumber
            // 
            this.lblPhoneNumber.AutoSize = true;
            this.lblPhoneNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhoneNumber.Location = new System.Drawing.Point(287, 84);
            this.lblPhoneNumber.Name = "lblPhoneNumber";
            this.lblPhoneNumber.Size = new System.Drawing.Size(68, 16);
            this.lblPhoneNumber.TabIndex = 8;
            this.lblPhoneNumber.Text = "Phone No";
            this.lblPhoneNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerName.Location = new System.Drawing.Point(116, 81);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(156, 22);
            this.txtCustomerName.TabIndex = 7;
            this.txtCustomerName.Enter += new System.EventHandler(this.TxtCustomerName_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Customer Name";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTrxId
            // 
            this.txtTrxId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTrxId.Location = new System.Drawing.Point(622, 39);
            this.txtTrxId.Name = "txtTrxId";
            this.txtTrxId.Size = new System.Drawing.Size(120, 22);
            this.txtTrxId.TabIndex = 5;
            this.txtTrxId.Enter += new System.EventHandler(this.TxtTrxId_Enter);
            // 
            // lblTrxId
            // 
            this.lblTrxId.AutoSize = true;
            this.lblTrxId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrxId.Location = new System.Drawing.Point(523, 42);
            this.lblTrxId.Name = "lblTrxId";
            this.lblTrxId.Size = new System.Drawing.Size(93, 16);
            this.lblTrxId.TabIndex = 4;
            this.lblTrxId.Text = "Transaction Id";
            this.lblTrxId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDate.Location = new System.Drawing.Point(287, 44);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(57, 16);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "To Date";
            this.lblToDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.Location = new System.Drawing.Point(6, 42);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(71, 16);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "From Date";
            this.lblFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TransactionWaiverList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 584);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransactionWaiverList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction Waiver List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TransactionWaiverList_FormClosing);
            this.Load += new System.EventHandler(this.SignedWaiversUI_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrxDetails)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView2;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarView2;
        private System.Windows.Forms.DataGridView dgvTrxDetails;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCustomFields;
        private System.Windows.Forms.Label lblCustomFields;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.Label lblPhoneNumber;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTrxId;
        private System.Windows.Forms.Label lblTrxId;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.DateTimePicker dtpFrom;
    }
}