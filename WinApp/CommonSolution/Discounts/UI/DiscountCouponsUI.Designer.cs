namespace Semnox.Parafait.Discounts
{
    partial class DiscountCouponsUI
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
            if(disposing && (components != null))
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvDiscountCouponsDTOList = new System.Windows.Forms.DataGridView();
            this.discountCouponsDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTransactionId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbSequential = new System.Windows.Forms.CheckBox();
            this.nudExpiresInDays = new System.Windows.Forms.NumericUpDown();
            this.nudCouponCount = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.chbPrintCoupons = new System.Windows.Forms.CheckBox();
            this.lblScheduleName = new System.Windows.Forms.Label();
            this.dtpExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCouponsUsed = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.couponSetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.countDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsedCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expiryDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountCouponsDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountCouponsDTOListBS)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpiresInDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCouponCount)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(13, 444);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(343, 444);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(233, 444);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 19;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(123, 444);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 18;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dgvDiscountCouponsDTOList
            // 
            this.dgvDiscountCouponsDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDiscountCouponsDTOList.AutoGenerateColumns = false;
            this.dgvDiscountCouponsDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiscountCouponsDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.couponSetIdDataGridViewTextBoxColumn,
            this.fromNumberDataGridViewTextBoxColumn,
            this.toNumberDataGridViewTextBoxColumn,
            this.countDataGridViewTextBoxColumn,
            this.UsedCount,
            this.startDateDataGridViewTextBoxColumn,
            this.expiryDateDataGridViewTextBoxColumn,
            this.transactionIdDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvDiscountCouponsDTOList.DataSource = this.discountCouponsDTOListBS;
            this.dgvDiscountCouponsDTOList.Location = new System.Drawing.Point(13, 169);
            this.dgvDiscountCouponsDTOList.Name = "dgvDiscountCouponsDTOList";
            this.dgvDiscountCouponsDTOList.Size = new System.Drawing.Size(766, 269);
            this.dgvDiscountCouponsDTOList.TabIndex = 17;
            this.dgvDiscountCouponsDTOList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDiscountCouponsDTOList_CellFormatting);
            this.dgvDiscountCouponsDTOList.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvDiscountCouponsDTOList_CellValidating);
            this.dgvDiscountCouponsDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDiscountCouponsDTOList_DataError);
            this.dgvDiscountCouponsDTOList.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvDiscountCouponsDTOList_EditingControlShowing);
            // 
            // discountCouponsDTOListBS
            // 
            this.discountCouponsDTOListBS.DataSource = typeof(Semnox.Parafait.Discounts.DiscountCouponsDTO);
            this.discountCouponsDTOListBS.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.discountCouponsDTOListBS_AddingNew);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtTransactionId);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.chbShowActiveEntries);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(769, 48);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // txtTransactionId
            // 
            this.txtTransactionId.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtTransactionId.Location = new System.Drawing.Point(524, 18);
            this.txtTransactionId.Name = "txtTransactionId";
            this.txtTransactionId.Size = new System.Drawing.Size(136, 21);
            this.txtTransactionId.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(411, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Transaction Id :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(677, 17);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(269, 18);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(156, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(107, 20);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Coupon Number :";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(6, 20);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(124, 19);
            this.chbShowActiveEntries.TabIndex = 1;
            this.chbShowActiveEntries.Text = "Show Active Only";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbSequential);
            this.groupBox2.Controls.Add(this.nudExpiresInDays);
            this.groupBox2.Controls.Add(this.nudCouponCount);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.chbPrintCoupons);
            this.groupBox2.Controls.Add(this.lblScheduleName);
            this.groupBox2.Controls.Add(this.dtpExpiryDate);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dtpEffectiveDate);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(10, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(769, 96);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Coupons Header";
            // 
            // chbSequential
            // 
            this.chbSequential.AutoSize = true;
            this.chbSequential.Checked = true;
            this.chbSequential.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbSequential.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbSequential.Location = new System.Drawing.Point(585, 57);
            this.chbSequential.Name = "chbSequential";
            this.chbSequential.Size = new System.Drawing.Size(86, 19);
            this.chbSequential.TabIndex = 24;
            this.chbSequential.Text = "Sequential";
            this.chbSequential.UseVisualStyleBackColor = true;
            // 
            // nudExpiresInDays
            // 
            this.nudExpiresInDays.Location = new System.Drawing.Point(428, 57);
            this.nudExpiresInDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudExpiresInDays.Name = "nudExpiresInDays";
            this.nudExpiresInDays.Size = new System.Drawing.Size(120, 21);
            this.nudExpiresInDays.TabIndex = 23;
            this.nudExpiresInDays.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudCouponCount
            // 
            this.nudCouponCount.Location = new System.Drawing.Point(428, 26);
            this.nudCouponCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCouponCount.Name = "nudCouponCount";
            this.nudCouponCount.Size = new System.Drawing.Size(120, 21);
            this.nudCouponCount.TabIndex = 22;
            this.nudCouponCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(322, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 15);
            this.label3.TabIndex = 20;
            this.label3.Text = "Expires in Days :";
            // 
            // chbPrintCoupons
            // 
            this.chbPrintCoupons.AutoSize = true;
            this.chbPrintCoupons.Checked = true;
            this.chbPrintCoupons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbPrintCoupons.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbPrintCoupons.Location = new System.Drawing.Point(585, 28);
            this.chbPrintCoupons.Name = "chbPrintCoupons";
            this.chbPrintCoupons.Size = new System.Drawing.Size(106, 19);
            this.chbPrintCoupons.TabIndex = 19;
            this.chbPrintCoupons.Text = "Print Coupons";
            this.chbPrintCoupons.UseVisualStyleBackColor = true;
            // 
            // lblScheduleName
            // 
            this.lblScheduleName.AutoSize = true;
            this.lblScheduleName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblScheduleName.Location = new System.Drawing.Point(330, 30);
            this.lblScheduleName.Name = "lblScheduleName";
            this.lblScheduleName.Size = new System.Drawing.Size(92, 15);
            this.lblScheduleName.TabIndex = 16;
            this.lblScheduleName.Text = "Coupon Count :";
            // 
            // dtpExpiryDate
            // 
            this.dtpExpiryDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.dtpExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpiryDate.Location = new System.Drawing.Point(104, 61);
            this.dtpExpiryDate.Name = "dtpExpiryDate";
            this.dtpExpiryDate.Size = new System.Drawing.Size(194, 21);
            this.dtpExpiryDate.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(20, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 15);
            this.label2.TabIndex = 14;
            this.label2.Text = "Expiry Date :";
            // 
            // dtpEffectiveDate
            // 
            this.dtpEffectiveDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.dtpEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEffectiveDate.Location = new System.Drawing.Point(103, 26);
            this.dtpEffectiveDate.Name = "dtpEffectiveDate";
            this.dtpEffectiveDate.Size = new System.Drawing.Size(195, 21);
            this.dtpEffectiveDate.TabIndex = 13;
            this.dtpEffectiveDate.ValueChanged += new System.EventHandler(this.dtpEffectiveDate_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Effective Date :";
            // 
            // btnCouponsUsed
            // 
            this.btnCouponsUsed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCouponsUsed.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnCouponsUsed.Location = new System.Drawing.Point(453, 444);
            this.btnCouponsUsed.Name = "btnCouponsUsed";
            this.btnCouponsUsed.Size = new System.Drawing.Size(105, 23);
            this.btnCouponsUsed.TabIndex = 23;
            this.btnCouponsUsed.Text = "Used Coupons";
            this.btnCouponsUsed.UseVisualStyleBackColor = true;
            this.btnCouponsUsed.Click += new System.EventHandler(this.btnCouponsUsed_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnImport.Location = new System.Drawing.Point(596, 444);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 24;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // couponSetIdDataGridViewTextBoxColumn
            // 
            this.couponSetIdDataGridViewTextBoxColumn.DataPropertyName = "CouponSetId";
            this.couponSetIdDataGridViewTextBoxColumn.HeaderText = "Coupon Set Id";
            this.couponSetIdDataGridViewTextBoxColumn.Name = "couponSetIdDataGridViewTextBoxColumn";
            this.couponSetIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fromNumberDataGridViewTextBoxColumn
            // 
            this.fromNumberDataGridViewTextBoxColumn.DataPropertyName = "FromNumber";
            this.fromNumberDataGridViewTextBoxColumn.HeaderText = "From Number";
            this.fromNumberDataGridViewTextBoxColumn.Name = "fromNumberDataGridViewTextBoxColumn";
            // 
            // toNumberDataGridViewTextBoxColumn
            // 
            this.toNumberDataGridViewTextBoxColumn.DataPropertyName = "ToNumber";
            this.toNumberDataGridViewTextBoxColumn.HeaderText = "To Number";
            this.toNumberDataGridViewTextBoxColumn.Name = "toNumberDataGridViewTextBoxColumn";
            // 
            // countDataGridViewTextBoxColumn
            // 
            this.countDataGridViewTextBoxColumn.DataPropertyName = "Count";
            this.countDataGridViewTextBoxColumn.HeaderText = "Count";
            this.countDataGridViewTextBoxColumn.Name = "countDataGridViewTextBoxColumn";
            // 
            // UsedCount
            // 
            this.UsedCount.DataPropertyName = "UsedCount";
            this.UsedCount.HeaderText = "Used Count";
            this.UsedCount.Name = "UsedCount";
            this.UsedCount.ReadOnly = true;
            // 
            // startDateDataGridViewTextBoxColumn
            // 
            this.startDateDataGridViewTextBoxColumn.DataPropertyName = "StartDate";
            this.startDateDataGridViewTextBoxColumn.HeaderText = "Effective Date";
            this.startDateDataGridViewTextBoxColumn.Name = "startDateDataGridViewTextBoxColumn";
            // 
            // expiryDateDataGridViewTextBoxColumn
            // 
            this.expiryDateDataGridViewTextBoxColumn.DataPropertyName = "ExpiryDate";
            this.expiryDateDataGridViewTextBoxColumn.HeaderText = "Coupon Expiry Date";
            this.expiryDateDataGridViewTextBoxColumn.Name = "expiryDateDataGridViewTextBoxColumn";
            // 
            // transactionIdDataGridViewTextBoxColumn
            // 
            this.transactionIdDataGridViewTextBoxColumn.DataPropertyName = "TransactionId";
            this.transactionIdDataGridViewTextBoxColumn.HeaderText = "TransactionId";
            this.transactionIdDataGridViewTextBoxColumn.Name = "transactionIdDataGridViewTextBoxColumn";
            this.transactionIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.FalseValue = "False";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.isActiveDataGridViewCheckBoxColumn.IndeterminateValue = "False";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isActiveDataGridViewCheckBoxColumn.TrueValue = "True";
            // 
            // DiscountCouponsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 480);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnCouponsUsed);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvDiscountCouponsDTOList);
            this.Controls.Add(this.groupBox1);
            this.Name = "DiscountCouponsUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Discount Coupons";
            this.Load += new System.EventHandler(this.DiscountCouponsUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountCouponsDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountCouponsDTOListBS)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpiresInDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCouponCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvDiscountCouponsDTOList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtpEffectiveDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpExpiryDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblScheduleName;
        private System.Windows.Forms.CheckBox chbPrintCoupons;
        private System.Windows.Forms.BindingSource discountCouponsDTOListBS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCouponsUsed;
        private System.Windows.Forms.NumericUpDown nudExpiresInDays;
        private System.Windows.Forms.NumericUpDown nudCouponCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTransactionId;
        private System.Windows.Forms.CheckBox chbSequential;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.DataGridViewTextBoxColumn couponSetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn countDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsedCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expiryDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}