namespace Parafait_POS
{
    partial class frmRentalReturn
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.dgvRentalReturn = new System.Windows.Forms.DataGridView();
            this.RefundCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblCardNo = new System.Windows.Forms.Label();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.lblTransactionId = new System.Windows.Forms.Label();
            this.txtTransactionId = new System.Windows.Forms.TextBox();
            this.btnRentalTrx = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblTrxNo = new System.Windows.Forms.Label();
            this.txtTransactionNumber = new System.Windows.Forms.TextBox();
            this.btnCustomerSearch = new System.Windows.Forms.Button();
            this.cbxIncludeReturned = new Semnox.Core.GenericUtilities.CustomCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRentalReturn)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(499, 359);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(134, 32);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReturn.BackColor = System.Drawing.Color.Transparent;
            this.btnReturn.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnReturn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReturn.ForeColor = System.Drawing.Color.White;
            this.btnReturn.Location = new System.Drawing.Point(255, 359);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(133, 32);
            this.btnReturn.TabIndex = 18;
            this.btnReturn.Text = "Return";
            this.btnReturn.UseVisualStyleBackColor = false;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.Transparent;
            this.btnGo.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnGo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.ForeColor = System.Drawing.Color.White;
            this.btnGo.Location = new System.Drawing.Point(752, 23);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(57, 27);
            this.btnGo.TabIndex = 17;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // dgvRentalReturn
            // 
            this.dgvRentalReturn.AllowDrop = true;
            this.dgvRentalReturn.AllowUserToAddRows = false;
            this.dgvRentalReturn.AllowUserToDeleteRows = false;
            this.dgvRentalReturn.AllowUserToResizeColumns = false;
            this.dgvRentalReturn.AllowUserToResizeRows = false;
            this.dgvRentalReturn.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRentalReturn.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvRentalReturn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvRentalReturn.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRentalReturn.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRentalReturn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRentalReturn.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RefundCount});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRentalReturn.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRentalReturn.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvRentalReturn.EnableHeadersVisualStyles = false;
            this.dgvRentalReturn.Location = new System.Drawing.Point(12, 63);
            this.dgvRentalReturn.Name = "dgvRentalReturn";
            this.dgvRentalReturn.RowHeadersVisible = false;
            this.dgvRentalReturn.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRentalReturn.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvRentalReturn.Size = new System.Drawing.Size(860, 290);
            this.dgvRentalReturn.TabIndex = 4;
            this.dgvRentalReturn.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRentalReturn_CellValidated);
            this.dgvRentalReturn.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvRentalReturn_DataError);
            this.dgvRentalReturn.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvRentalReturn_EditingControlShowing);
            this.dgvRentalReturn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvRentalReturn_KeyDown);
            this.dgvRentalReturn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvRentalReturn_KeyPress);
            this.dgvRentalReturn.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvRentalReturn_PreviewKeyDown);
            // 
            // RefundCount
            // 
            this.RefundCount.HeaderText = "Enter Return Count";
            this.RefundCount.Name = "RefundCount";
            this.RefundCount.ToolTipText = "Enter Rental return quantity";
            this.RefundCount.Width = 130;
            // 
            // lblCardNo
            // 
            this.lblCardNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNo.Location = new System.Drawing.Point(182, 27);
            this.lblCardNo.Name = "lblCardNo";
            this.lblCardNo.Size = new System.Drawing.Size(72, 21);
            this.lblCardNo.TabIndex = 13;
            this.lblCardNo.Text = "Card No:";
            this.lblCardNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.Location = new System.Drawing.Point(256, 25);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.ReadOnly = true;
            this.txtCardNumber.Size = new System.Drawing.Size(90, 22);
            this.txtCardNumber.TabIndex = 23;
            // 
            // lblTransactionId
            // 
            this.lblTransactionId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionId.Location = new System.Drawing.Point(8, 27);
            this.lblTransactionId.Name = "lblTransactionId";
            this.lblTransactionId.Size = new System.Drawing.Size(52, 20);
            this.lblTransactionId.TabIndex = 11;
            this.lblTransactionId.Text = "Trx Id:";
            this.lblTransactionId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtTransactionId
            // 
            this.txtTransactionId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransactionId.Location = new System.Drawing.Point(63, 25);
            this.txtTransactionId.Name = "txtTransactionId";
            this.txtTransactionId.Size = new System.Drawing.Size(90, 22);
            this.txtTransactionId.TabIndex = 0;
            this.txtTransactionId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTransactionId_KeyDown);
            this.txtTransactionId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTransactionId_KeyPress);
            // 
            // btnRentalTrx
            // 
            this.btnRentalTrx.BackColor = System.Drawing.Color.Transparent;
            this.btnRentalTrx.BackgroundImage = global::Parafait_POS.Properties.Resources.Keypad;
            this.btnRentalTrx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRentalTrx.CausesValidation = false;
            this.btnRentalTrx.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnRentalTrx.FlatAppearance.BorderSize = 0;
            this.btnRentalTrx.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRentalTrx.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRentalTrx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRentalTrx.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRentalTrx.ForeColor = System.Drawing.Color.White;
            this.btnRentalTrx.Location = new System.Drawing.Point(157, 24);
            this.btnRentalTrx.Name = "btnRentalTrx";
            this.btnRentalTrx.Size = new System.Drawing.Size(21, 22);
            this.btnRentalTrx.TabIndex = 20;
            this.btnRentalTrx.UseVisualStyleBackColor = false;
            this.btnRentalTrx.Click += new System.EventHandler(this.btnRentalTrx_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(815, 23);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(57, 27);
            this.btnClear.TabIndex = 21;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblTrxNo
            // 
            this.lblTrxNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrxNo.Location = new System.Drawing.Point(352, 27);
            this.lblTrxNo.Name = "lblTrxNo";
            this.lblTrxNo.Size = new System.Drawing.Size(60, 21);
            this.lblTrxNo.TabIndex = 22;
            this.lblTrxNo.Text = "Trx No:";
            this.lblTrxNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTransactionNumber
            // 
            this.txtTransactionNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransactionNumber.Location = new System.Drawing.Point(416, 25);
            this.txtTransactionNumber.Name = "txtTransactionNumber";
            this.txtTransactionNumber.Size = new System.Drawing.Size(90, 22);
            this.txtTransactionNumber.TabIndex = 1;
            this.txtTransactionNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTransactionNumber_KeyDown);
            // 
            // btnCustomerSearch
            // 
            this.btnCustomerSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnCustomerSearch.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCustomerSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCustomerSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnCustomerSearch.ForeColor = System.Drawing.Color.White;
            this.btnCustomerSearch.Location = new System.Drawing.Point(671, 23);
            this.btnCustomerSearch.Name = "btnCustomerSearch";
            this.btnCustomerSearch.Size = new System.Drawing.Size(75, 27);
            this.btnCustomerSearch.TabIndex = 26;
            this.btnCustomerSearch.Text = "Customer";
            this.btnCustomerSearch.UseVisualStyleBackColor = false;
            this.btnCustomerSearch.Click += new System.EventHandler(this.btnCustomerSearch_Click);
            // 
            // cbxIncludeReturned
            // 
            this.cbxIncludeReturned.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxIncludeReturned.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxIncludeReturned.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxIncludeReturned.FlatAppearance.BorderSize = 0;
            this.cbxIncludeReturned.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.cbxIncludeReturned.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.cbxIncludeReturned.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.cbxIncludeReturned.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxIncludeReturned.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cbxIncludeReturned.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxIncludeReturned.ImageIndex = 1;
            this.cbxIncludeReturned.Location = new System.Drawing.Point(508, 22);
            this.cbxIncludeReturned.Name = "cbxIncludeReturned";
            this.cbxIncludeReturned.Size = new System.Drawing.Size(155, 28);
            this.cbxIncludeReturned.TabIndex = 98;
            this.cbxIncludeReturned.Text = "Include Returned:";
            this.cbxIncludeReturned.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxIncludeReturned.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.cbxIncludeReturned.UseVisualStyleBackColor = true;
            // 
            // frmRentalReturn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 400);
            this.Controls.Add(this.btnCustomerSearch);
            this.Controls.Add(this.txtTransactionNumber);
            this.Controls.Add(this.lblTrxNo);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnRentalTrx);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.dgvRentalReturn);
            this.Controls.Add(this.lblCardNo);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.lblTransactionId);
            this.Controls.Add(this.txtTransactionId);
            this.Controls.Add(this.cbxIncludeReturned);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRentalReturn";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rental Return";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRentalReturn_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRentalReturn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label lblCardNo;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label lblTransactionId;
        private System.Windows.Forms.TextBox txtTransactionId;
        public System.Windows.Forms.DataGridView dgvRentalReturn;
        private System.Windows.Forms.Button btnRentalTrx;
        private System.Windows.Forms.DataGridViewTextBoxColumn RefundCount;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblTrxNo;
        private System.Windows.Forms.TextBox txtTransactionNumber;
        private System.Windows.Forms.Button btnCustomerSearch;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxIncludeReturned;
    }
}