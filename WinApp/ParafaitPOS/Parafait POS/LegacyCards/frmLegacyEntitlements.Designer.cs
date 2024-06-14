using System.Windows.Forms;

namespace Parafait_POS
{
    partial class frmLegacyEntitlements
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
            this.dgvLegacyEntitlements = new System.Windows.Forms.DataGridView();
            this.LegacyCardCreditPlusDataseBS = new System.Windows.Forms.BindingSource(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.legacyCardGameExtendedDTOListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.legacyCardGamesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bookingAttendeesTableAdapter1 = new Parafait_POS.Reservation.ParafaitDataSetTableAdapters.BookingAttendeesTableAdapter();
            this.DGVChildLines = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblParentHeader = new System.Windows.Forms.Label();
            this.lblChildHeader = new System.Windows.Forms.Label();
            this.legacyCardDiscountsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.LegacyCardCreditPlusId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LegacyCreditPlus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RevisedLegacyCreditPlus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreditPlusType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LegacyCard_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLegacyEntitlements)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LegacyCardCreditPlusDataseBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.legacyCardGameExtendedDTOListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.legacyCardGamesDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGVChildLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.legacyCardDiscountsDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvLegacyEntitlements
            // 
            this.dgvLegacyEntitlements.AllowUserToAddRows = false;
            this.dgvLegacyEntitlements.AllowUserToDeleteRows = false;
            this.dgvLegacyEntitlements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLegacyEntitlements.AutoGenerateColumns = false;
            this.dgvLegacyEntitlements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLegacyEntitlements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LegacyCardCreditPlusId,
            this.LegacyCreditPlus,
            this.RevisedLegacyCreditPlus,
            this.CreditPlusType,
            this.Remarks,
            this.LegacyCard_id,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvLegacyEntitlements.DataSource = this.LegacyCardCreditPlusDataseBS;
            this.dgvLegacyEntitlements.Location = new System.Drawing.Point(31, 38);
            this.dgvLegacyEntitlements.Name = "dgvLegacyEntitlements";
            this.dgvLegacyEntitlements.ReadOnly = true;
            this.dgvLegacyEntitlements.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLegacyEntitlements.Size = new System.Drawing.Size(683, 170);
            this.dgvLegacyEntitlements.TabIndex = 1;
            this.dgvLegacyEntitlements.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvLegacyEntitlements_DataError);
            this.dgvLegacyEntitlements.SelectionChanged += new System.EventHandler(this.dgvLegacyEntitlements_SelectionChanged);
            // 
            // LegacyCardCreditPlusDataseBS
            // 
            this.LegacyCardCreditPlusDataseBS.DataSource = typeof(Parafait_POS.LegacyCardCreditPlusDTO);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Chocolate;
            this.btnOk.Location = new System.Drawing.Point(330, 219);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(89, 31);
            this.btnOk.TabIndex = 46;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // legacyCardGameExtendedDTOListBindingSource
            // 
            this.legacyCardGameExtendedDTOListBindingSource.DataMember = "LegacyCardGameExtendedDTOList";
            this.legacyCardGameExtendedDTOListBindingSource.DataSource = this.legacyCardGamesDTOBindingSource;
            // 
            // legacyCardGamesDTOBindingSource
            // 
            this.legacyCardGamesDTOBindingSource.DataSource = typeof(Parafait_POS.LegacyCardGamesDTO);
            // 
            // bookingAttendeesTableAdapter1
            // 
            this.bookingAttendeesTableAdapter1.ClearBeforeFill = true;
            // 
            // DGVChildLines
            // 
            this.DGVChildLines.AllowUserToAddRows = false;
            this.DGVChildLines.AllowUserToDeleteRows = false;
            this.DGVChildLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVChildLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVChildLines.Location = new System.Drawing.Point(31, 263);
            this.DGVChildLines.Name = "DGVChildLines";
            this.DGVChildLines.ReadOnly = true;
            this.DGVChildLines.Size = new System.Drawing.Size(683, 171);
            this.DGVChildLines.TabIndex = 47;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Chocolate;
            this.btnClose.Location = new System.Drawing.Point(330, 456);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(89, 31);
            this.btnClose.TabIndex = 48;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblParentHeader
            // 
            this.lblParentHeader.Location = new System.Drawing.Point(31, 18);
            this.lblParentHeader.Name = "lblParentHeader";
            this.lblParentHeader.Size = new System.Drawing.Size(306, 16);
            this.lblParentHeader.TabIndex = 49;
            this.lblParentHeader.Text = "Header1";
            this.lblParentHeader.Visible = false;
            // 
            // lblChildHeader
            // 
            this.lblChildHeader.Location = new System.Drawing.Point(31, 242);
            this.lblChildHeader.Name = "lblChildHeader";
            this.lblChildHeader.Size = new System.Drawing.Size(292, 16);
            this.lblChildHeader.TabIndex = 51;
            this.lblChildHeader.Text = "Header2";
            this.lblChildHeader.Visible = false;
            // 
            // legacyCardDiscountsDTOBindingSource
            // 
            this.legacyCardDiscountsDTOBindingSource.DataSource = typeof(Parafait_POS.LegacyCardDiscountsDTO);
            // 
            // LegacyCardCreditPlusId
            // 
            this.LegacyCardCreditPlusId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.LegacyCardCreditPlusId.DataPropertyName = "LegacyCardCreditPlusId";
            this.LegacyCardCreditPlusId.HeaderText = "Id";
            this.LegacyCardCreditPlusId.Name = "LegacyCardCreditPlusId";
            this.LegacyCardCreditPlusId.ReadOnly = true;
            this.LegacyCardCreditPlusId.Width = 41;
            // 
            // LegacyCreditPlus
            // 
            this.LegacyCreditPlus.DataPropertyName = "LegacyCreditPlus";
            this.LegacyCreditPlus.HeaderText = "Legacy Credit Plus";
            this.LegacyCreditPlus.Name = "LegacyCreditPlus";
            this.LegacyCreditPlus.ReadOnly = true;
            this.LegacyCreditPlus.Width = 114;
            // 
            // RevisedLegacyCreditPlus
            // 
            this.RevisedLegacyCreditPlus.DataPropertyName = "RevisedLegacyCreditPlus";
            this.RevisedLegacyCreditPlus.HeaderText = "Revised Legacy Credit Plus";
            this.RevisedLegacyCreditPlus.Name = "RevisedLegacyCreditPlus";
            this.RevisedLegacyCreditPlus.ReadOnly = true;
            // 
            // CreditPlusType
            // 
            this.CreditPlusType.DataPropertyName = "CreditPlusType";
            this.CreditPlusType.HeaderText = "Credit Plus Type";
            this.CreditPlusType.Name = "CreditPlusType";
            this.CreditPlusType.ReadOnly = true;
            this.CreditPlusType.Width = 103;
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            this.Remarks.ReadOnly = true;
            this.Remarks.Width = 74;
            // 
            // LegacyCard_id
            // 
            this.LegacyCard_id.DataPropertyName = "LegacyCard_id";
            this.LegacyCard_id.HeaderText = "LegacyCard Id";
            this.LegacyCard_id.Name = "LegacyCard_id";
            this.LegacyCard_id.ReadOnly = true;
            this.LegacyCard_id.Width = 103;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Is Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // frmLegacyEntitlements
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 500);
            this.Controls.Add(this.lblChildHeader);
            this.Controls.Add(this.lblParentHeader);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.DGVChildLines);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dgvLegacyEntitlements);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLegacyEntitlements";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LegacyEntitlements";
            this.Load += new System.EventHandler(this.frmLegacyEntitlements_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLegacyEntitlements)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LegacyCardCreditPlusDataseBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.legacyCardGameExtendedDTOListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.legacyCardGamesDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGVChildLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.legacyCardDiscountsDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLegacyEntitlements;
        private System.Windows.Forms.BindingSource legacyCardDiscountsDTOBindingSource;
        private System.Windows.Forms.BindingSource LegacyCardCreditPlusDataseBS;
        private System.Windows.Forms.BindingSource legacyCardGamesDTOBindingSource;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.BindingSource legacyCardGameExtendedDTOListBindingSource;
        private Reservation.ParafaitDataSetTableAdapters.BookingAttendeesTableAdapter bookingAttendeesTableAdapter1;
        private System.Windows.Forms.DataGridView DGVChildLines;
        private Button btnClose;
        private Label lblParentHeader;
        private Label lblChildHeader;
        private DataGridViewTextBoxColumn LegacyCardCreditPlusId;
        private DataGridViewTextBoxColumn LegacyCreditPlus;
        private DataGridViewTextBoxColumn RevisedLegacyCreditPlus;
        private DataGridViewTextBoxColumn CreditPlusType;
        private DataGridViewTextBoxColumn Remarks;
        private DataGridViewTextBoxColumn LegacyCard_id;
        private DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}