namespace Parafait_POS.Redemption
{
    partial class frmSuspendedRedemptions
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
            this.label1 = new System.Windows.Forms.Label();
            this.dgvSuspended = new System.Windows.Forms.DataGridView();
            this.dcData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcETickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcVoucherTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcManualTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcCurrencyTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcRedeemed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcGiftCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcTimeSuspended = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkShowAll = new System.Windows.Forms.CheckBox();
            this.btnRetrieve = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuspended)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(475, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Parked Redemptions";
            // 
            // dgvSuspended
            // 
            this.dgvSuspended.AllowUserToAddRows = false;
            this.dgvSuspended.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSuspended.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.dgvSuspended.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSuspended.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSuspended.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcData,
            this.dcCardNumber,
            this.dcETickets,
            this.dcVoucherTickets,
            this.dcManualTickets,
            this.dcCurrencyTickets,
            this.dcRedeemed,
            this.dcGiftCount,
            this.dcTimeSuspended,
            this.dcRemarks});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSuspended.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSuspended.EnableHeadersVisualStyles = false;
            this.dgvSuspended.GridColor = System.Drawing.Color.White;
            this.dgvSuspended.Location = new System.Drawing.Point(3, 45);
            this.dgvSuspended.Margin = new System.Windows.Forms.Padding(4);
            this.dgvSuspended.Name = "dgvSuspended";
            this.dgvSuspended.ReadOnly = true;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgvSuspended.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSuspended.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgvSuspended.RowTemplate.Height = 30;
            this.dgvSuspended.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSuspended.Size = new System.Drawing.Size(979, 346);
            this.dgvSuspended.TabIndex = 0;
            this.dgvSuspended.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSuspended_CellClick);
            this.dgvSuspended.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvSuspended_Scroll);
            // 
            // dcData
            // 
            this.dcData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dcData.HeaderText = "Id";
            this.dcData.Name = "dcData";
            this.dcData.ReadOnly = true;
            this.dcData.Width = 44;
            // 
            // dcCardNumber
            // 
            this.dcCardNumber.HeaderText = "Card Number";
            this.dcCardNumber.Name = "dcCardNumber";
            this.dcCardNumber.ReadOnly = true;
            // 
            // dcETickets
            // 
            this.dcETickets.HeaderText = "eTickets";
            this.dcETickets.Name = "dcETickets";
            this.dcETickets.ReadOnly = true;
            // 
            // dcVoucherTickets
            // 
            this.dcVoucherTickets.HeaderText = "Voucher Tickets";
            this.dcVoucherTickets.Name = "dcVoucherTickets";
            this.dcVoucherTickets.ReadOnly = true;
            // 
            // dcManualTickets
            // 
            this.dcManualTickets.HeaderText = "Manual Tickets";
            this.dcManualTickets.Name = "dcManualTickets";
            this.dcManualTickets.ReadOnly = true;
            // 
            // dcCurrencyTickets
            // 
            this.dcCurrencyTickets.HeaderText = "Currency Tickets";
            this.dcCurrencyTickets.Name = "dcCurrencyTickets";
            this.dcCurrencyTickets.ReadOnly = true;
            // 
            // dcRedeemed
            // 
            this.dcRedeemed.HeaderText = "Redeemed";
            this.dcRedeemed.Name = "dcRedeemed";
            this.dcRedeemed.ReadOnly = true;
            // 
            // dcGiftCount
            // 
            this.dcGiftCount.HeaderText = "Gifts";
            this.dcGiftCount.Name = "dcGiftCount";
            this.dcGiftCount.ReadOnly = true;
            // 
            // dcTimeSuspended
            // 
            this.dcTimeSuspended.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dcTimeSuspended.HeaderText = "Time Parked";
            this.dcTimeSuspended.Name = "dcTimeSuspended";
            this.dcTimeSuspended.ReadOnly = true;
            this.dcTimeSuspended.Width = 104;
            // 
            // dcRemarks
            // 
            this.dcRemarks.HeaderText = "Remarks";
            this.dcRemarks.Name = "dcRemarks";
            this.dcRemarks.ReadOnly = true;
            // 
            // chkShowAll
            // 
            this.chkShowAll.AutoSize = true;
            this.chkShowAll.Location = new System.Drawing.Point(3, 16);
            this.chkShowAll.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowAll.Name = "chkShowAll";
            this.chkShowAll.Size = new System.Drawing.Size(80, 21);
            this.chkShowAll.TabIndex = 1;
            this.chkShowAll.Text = "Show All";
            this.chkShowAll.UseVisualStyleBackColor = true;
            this.chkShowAll.CheckedChanged += new System.EventHandler(this.chkShowAll_CheckedChanged);
            // 
            // btnRetrieve
            // 
            this.btnRetrieve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRetrieve.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRetrieve.FlatAppearance.BorderSize = 0;
            this.btnRetrieve.FlatAppearance.CheckedBackColor = System.Drawing.Color.RoyalBlue;
            this.btnRetrieve.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnRetrieve.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnRetrieve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRetrieve.ForeColor = System.Drawing.Color.White;
            this.btnRetrieve.Location = new System.Drawing.Point(259, 401);
            this.btnRetrieve.Name = "btnRetrieve";
            this.btnRetrieve.Size = new System.Drawing.Size(101, 44);
            this.btnRetrieve.TabIndex = 2;
            this.btnRetrieve.Text = "Retrieve";
            this.btnRetrieve.UseVisualStyleBackColor = false;
            this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatAppearance.CheckedBackColor = System.Drawing.Color.RoyalBlue;
            this.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(441, 401);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(101, 44);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.RoyalBlue;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(623, 401);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 44);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmSuspendedRedemptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(984, 455);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRetrieve);
            this.Controls.Add(this.chkShowAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvSuspended);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmSuspendedRedemptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Parked Redemptions";
            this.Load += new System.EventHandler(this.frmSuspendedRedemptions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuspended)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSuspended;
        private System.Windows.Forms.CheckBox chkShowAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRetrieve;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcData;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcCardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcETickets;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcVoucherTickets;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcManualTickets;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcCurrencyTickets;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcRedeemed;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcGiftCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcTimeSuspended;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcRemarks;
    }
}