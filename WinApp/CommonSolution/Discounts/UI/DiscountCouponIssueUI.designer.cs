namespace Semnox.Parafait.Discounts
{
    partial class DiscountCouponIssueUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiscountCouponIssueUI));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.btnGO = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvDiscountCouponsDTOList = new System.Windows.Forms.DataGridView();
            this.serialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAlphaKeypad = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnIssueCoupons = new System.Windows.Forms.Button();
            this.lblCouponCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDiscountValue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblDiscountName = new System.Windows.Forms.Label();
            this.dtpExpiresOn = new System.Windows.Forms.DateTimePicker();
            this.dtpEffectiveFrom = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.verticalScrollBarView1 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.fromNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discountCouponsDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountCouponsDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountCouponsDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(19, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Coupon Count :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtFrom);
            this.groupBox1.Controls.Add(this.lblFrom);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(13, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(343, 79);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Coupon Range";
            // 
            // txtFrom
            // 
            this.txtFrom.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFrom.Location = new System.Drawing.Point(61, 29);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(205, 22);
            this.txtFrom.TabIndex = 1;
            this.txtFrom.Enter += new System.EventHandler(this.textbox_Enter);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(6, 32);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(53, 16);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From : ";
            // 
            // btnGO
            // 
            this.btnGO.BackColor = System.Drawing.Color.Transparent;
            this.btnGO.BackgroundImage = global::Semnox.Parafait.Discounts.Properties.Resources.normal2;
            this.btnGO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGO.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnGO.FlatAppearance.BorderSize = 0;
            this.btnGO.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnGO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGO.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGO.ForeColor = System.Drawing.Color.White;
            this.btnGO.Location = new System.Drawing.Point(448, 121);
            this.btnGO.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(155, 55);
            this.btnGO.TabIndex = 5;
            this.btnGO.Text = "GO";
            this.btnGO.UseVisualStyleBackColor = false;
            this.btnGO.Click += new System.EventHandler(this.btnGO_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.verticalScrollBarView1);
            this.groupBox2.Controls.Add(this.dgvDiscountCouponsDTOList);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(13, 195);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(647, 185);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Coupons List";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // dgvDiscountCouponsDTOList
            // 
            this.dgvDiscountCouponsDTOList.AllowUserToAddRows = false;
            this.dgvDiscountCouponsDTOList.AllowUserToDeleteRows = false;
            this.dgvDiscountCouponsDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDiscountCouponsDTOList.AutoGenerateColumns = false;
            this.dgvDiscountCouponsDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiscountCouponsDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fromNumberDataGridViewTextBoxColumn,
            this.serialNumber});
            this.dgvDiscountCouponsDTOList.DataSource = this.discountCouponsDTOListBS;
            this.dgvDiscountCouponsDTOList.Location = new System.Drawing.Point(12, 27);
            this.dgvDiscountCouponsDTOList.Name = "dgvDiscountCouponsDTOList";
            this.dgvDiscountCouponsDTOList.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvDiscountCouponsDTOList.Size = new System.Drawing.Size(578, 152);
            this.dgvDiscountCouponsDTOList.TabIndex = 0;
            this.dgvDiscountCouponsDTOList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvDiscountCouponsDTOList_DataBindingComplete);
            this.dgvDiscountCouponsDTOList.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvDiscountCouponsDTOList_EditingControlShowing);
            // 
            // serialNumber
            // 
            this.serialNumber.HeaderText = "Sl No";
            this.serialNumber.Name = "serialNumber";
            this.serialNumber.ReadOnly = true;
            this.serialNumber.Width = 75;
            // 
            // btnAlphaKeypad
            // 
            this.btnAlphaKeypad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAlphaKeypad.BackColor = System.Drawing.Color.Transparent;
            this.btnAlphaKeypad.BackgroundImage = global::Semnox.Parafait.Discounts.Properties.Resources.keyboard;
            this.btnAlphaKeypad.CausesValidation = false;
            this.btnAlphaKeypad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAlphaKeypad.FlatAppearance.BorderSize = 0;
            this.btnAlphaKeypad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAlphaKeypad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAlphaKeypad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlphaKeypad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlphaKeypad.ForeColor = System.Drawing.Color.Black;
            this.btnAlphaKeypad.Location = new System.Drawing.Point(617, 422);
            this.btnAlphaKeypad.Name = "btnAlphaKeypad";
            this.btnAlphaKeypad.Size = new System.Drawing.Size(36, 33);
            this.btnAlphaKeypad.TabIndex = 26;
            this.btnAlphaKeypad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAlphaKeypad.UseVisualStyleBackColor = false;
            this.btnAlphaKeypad.Click += new System.EventHandler(this.btnAlphaKeypad_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Semnox.Parafait.Discounts.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(174, 387);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(155, 55);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnIssueCoupons
            // 
            this.btnIssueCoupons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIssueCoupons.BackColor = System.Drawing.Color.Transparent;
            this.btnIssueCoupons.BackgroundImage = global::Semnox.Parafait.Discounts.Properties.Resources.normal2;
            this.btnIssueCoupons.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIssueCoupons.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnIssueCoupons.FlatAppearance.BorderSize = 0;
            this.btnIssueCoupons.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnIssueCoupons.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnIssueCoupons.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnIssueCoupons.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIssueCoupons.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIssueCoupons.ForeColor = System.Drawing.Color.White;
            this.btnIssueCoupons.Location = new System.Drawing.Point(13, 387);
            this.btnIssueCoupons.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnIssueCoupons.Name = "btnIssueCoupons";
            this.btnIssueCoupons.Size = new System.Drawing.Size(155, 55);
            this.btnIssueCoupons.TabIndex = 6;
            this.btnIssueCoupons.Text = "Issue Coupon";
            this.btnIssueCoupons.UseVisualStyleBackColor = false;
            this.btnIssueCoupons.Click += new System.EventHandler(this.btnIssueCoupons_Click);
            // 
            // lblCouponCount
            // 
            this.lblCouponCount.AutoSize = true;
            this.lblCouponCount.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblCouponCount.Location = new System.Drawing.Point(132, 40);
            this.lblCouponCount.Name = "lblCouponCount";
            this.lblCouponCount.Size = new System.Drawing.Size(15, 16);
            this.lblCouponCount.TabIndex = 27;
            this.lblCouponCount.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(20, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 16);
            this.label2.TabIndex = 28;
            this.label2.Text = "Discount Value :";
            // 
            // lblDiscountValue
            // 
            this.lblDiscountValue.AutoSize = true;
            this.lblDiscountValue.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblDiscountValue.Location = new System.Drawing.Point(132, 70);
            this.lblDiscountValue.Name = "lblDiscountValue";
            this.lblDiscountValue.Size = new System.Drawing.Size(15, 16);
            this.lblDiscountValue.TabIndex = 29;
            this.lblDiscountValue.Text = "0";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(300, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 16);
            this.label3.TabIndex = 30;
            this.label3.Text = "Effective From : ";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(300, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 16);
            this.label4.TabIndex = 31;
            this.label4.Text = "Expires On :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(19, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 16);
            this.label5.TabIndex = 32;
            this.label5.Text = "Discount Name :";
            // 
            // lblDiscountName
            // 
            this.lblDiscountName.AutoSize = true;
            this.lblDiscountName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblDiscountName.Location = new System.Drawing.Point(132, 10);
            this.lblDiscountName.Name = "lblDiscountName";
            this.lblDiscountName.Size = new System.Drawing.Size(115, 16);
            this.lblDiscountName.TabIndex = 33;
            this.lblDiscountName.Text = "Sample Discount";
            // 
            // dtpExpiresOn
            // 
            this.dtpExpiresOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpExpiresOn.CustomFormat = "dd-MMM-yyyy";
            this.dtpExpiresOn.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpiresOn.Location = new System.Drawing.Point(438, 66);
            this.dtpExpiresOn.Name = "dtpExpiresOn";
            this.dtpExpiresOn.Size = new System.Drawing.Size(106, 20);
            this.dtpExpiresOn.TabIndex = 71;
            // 
            // dtpEffectiveFrom
            // 
            this.dtpEffectiveFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpEffectiveFrom.CustomFormat = "dd-MMM-yyyy";
            this.dtpEffectiveFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEffectiveFrom.Location = new System.Drawing.Point(438, 39);
            this.dtpEffectiveFrom.Name = "dtpEffectiveFrom";
            this.dtpEffectiveFrom.Size = new System.Drawing.Size(106, 20);
            this.dtpEffectiveFrom.TabIndex = 71;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Sl No";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 75;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "FromNumber";
            this.dataGridViewTextBoxColumn2.HeaderText = "Coupon Number";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // verticalScrollBarView1
            // 
            this.verticalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView1.AutoHide = false;
            this.verticalScrollBarView1.DataGridView = this.dgvDiscountCouponsDTOList;
            this.verticalScrollBarView1.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.DownButtonBackgroundImage")));
            this.verticalScrollBarView1.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarView1.Location = new System.Drawing.Point(600, 27);
            this.verticalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView1.Name = "verticalScrollBarView1";
            this.verticalScrollBarView1.ScrollableControl = null;
            this.verticalScrollBarView1.ScrollViewer = null;
            this.verticalScrollBarView1.Size = new System.Drawing.Size(40, 152);
            this.verticalScrollBarView1.TabIndex = 1;
            this.verticalScrollBarView1.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.UpButtonBackgroundImage")));
            this.verticalScrollBarView1.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.UpButtonDisabledBackgroundImage")));
            // 
            // fromNumberDataGridViewTextBoxColumn
            // 
            this.fromNumberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fromNumberDataGridViewTextBoxColumn.DataPropertyName = "FromNumber";
            this.fromNumberDataGridViewTextBoxColumn.HeaderText = "Coupon Number";
            this.fromNumberDataGridViewTextBoxColumn.Name = "fromNumberDataGridViewTextBoxColumn";
            // 
            // discountCouponsDTOListBS
            // 
            this.discountCouponsDTOListBS.DataSource = typeof(Semnox.Parafait.Discounts.DiscountCouponsDTO);
            // 
            // DiscountCouponIssueUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 456);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.dtpEffectiveFrom);
            this.Controls.Add(this.dtpExpiresOn);
            this.Controls.Add(this.lblDiscountName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblDiscountValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCouponCount);
            this.Controls.Add(this.btnAlphaKeypad);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnIssueCoupons);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "DiscountCouponIssueUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Issue Coupons";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiscountCouponIssueUI_FormClosing);
            this.Load += new System.EventHandler(this.DiscountCouponIssueUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscountCouponsDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountCouponsDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.Button btnIssueCoupons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvDiscountCouponsDTOList;
        private System.Windows.Forms.BindingSource discountCouponsDTOListBS;
        private System.Windows.Forms.Button btnAlphaKeypad;
        private System.Windows.Forms.Label lblCouponCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDiscountValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblDiscountName;
        private System.Windows.Forms.DataGridViewTextBoxColumn serialNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DateTimePicker dtpExpiresOn;
        private System.Windows.Forms.DateTimePicker dtpEffectiveFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView1;
    }
}