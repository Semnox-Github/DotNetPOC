namespace Semnox.Parafait.Inventory
{
    partial class frmInventoryStockDetails
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
            this.dgvItemInfo = new System.Windows.Forms.DataGridView();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.lblProdDescription = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.txtProdDescription = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblLastReceiveDate = new System.Windows.Forms.Label();
            this.txtLastReceivedDate = new System.Windows.Forms.TextBox();
            this.DataGridViewLocationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewQuantityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewLotIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewLotNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtUOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataGridViewOriginalQuantityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewBalanceQtyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewExpiryDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvItemInfo
            // 
            this.dgvItemInfo.AllowUserToDeleteRows = false;
            this.dgvItemInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItemInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvItemInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DataGridViewLocationColumn,
            this.DataGridViewQuantityColumn,
            this.dataGridViewLotIdColumn,
            this.DataGridViewLotNumberColumn,
            this.txtUOM,
            this.DataGridViewOriginalQuantityColumn,
            this.dataGridViewBalanceQtyColumn,
            this.dataGridViewExpiryDateColumn});
            this.dgvItemInfo.Location = new System.Drawing.Point(12, 77);
            this.dgvItemInfo.Name = "dgvItemInfo";
            this.dgvItemInfo.ReadOnly = true;
            this.dgvItemInfo.RowHeadersVisible = false;
            this.dgvItemInfo.Size = new System.Drawing.Size(631, 302);
            this.dgvItemInfo.TabIndex = 0;
            // 
            // lblProductCode
            // 
            this.lblProductCode.AutoSize = true;
            this.lblProductCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductCode.Location = new System.Drawing.Point(12, 9);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(102, 17);
            this.lblProductCode.TabIndex = 1;
            this.lblProductCode.Text = "Product Code :";
            // 
            // lblProdDescription
            // 
            this.lblProdDescription.AutoSize = true;
            this.lblProdDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProdDescription.Location = new System.Drawing.Point(27, 43);
            this.lblProdDescription.Name = "lblProdDescription";
            this.lblProdDescription.Size = new System.Drawing.Size(87, 17);
            this.lblProdDescription.TabIndex = 2;
            this.lblProdDescription.Text = "Description :";
            // 
            // txtProductCode
            // 
            this.txtProductCode.BackColor = System.Drawing.Color.White;
            this.txtProductCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductCode.Location = new System.Drawing.Point(120, 9);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.ReadOnly = true;
            this.txtProductCode.Size = new System.Drawing.Size(229, 21);
            this.txtProductCode.TabIndex = 3;
            // 
            // txtProdDescription
            // 
            this.txtProdDescription.BackColor = System.Drawing.Color.White;
            this.txtProdDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProdDescription.Location = new System.Drawing.Point(120, 43);
            this.txtProdDescription.Name = "txtProdDescription";
            this.txtProdDescription.ReadOnly = true;
            this.txtProdDescription.Size = new System.Drawing.Size(229, 21);
            this.txtProdDescription.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(274, 385);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 36);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblLastReceiveDate
            // 
            this.lblLastReceiveDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastReceiveDate.Location = new System.Drawing.Point(355, 42);
            this.lblLastReceiveDate.Name = "lblLastReceiveDate";
            this.lblLastReceiveDate.Size = new System.Drawing.Size(142, 18);
            this.lblLastReceiveDate.TabIndex = 135;
            this.lblLastReceiveDate.Text = "Last Received Date :";
            this.lblLastReceiveDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLastReceivedDate
            // 
            this.txtLastReceivedDate.BackColor = System.Drawing.Color.White;
            this.txtLastReceivedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastReceivedDate.Location = new System.Drawing.Point(498, 43);
            this.txtLastReceivedDate.Name = "txtLastReceivedDate";
            this.txtLastReceivedDate.ReadOnly = true;
            this.txtLastReceivedDate.Size = new System.Drawing.Size(129, 21);
            this.txtLastReceivedDate.TabIndex = 134;
            // 
            // DataGridViewLocationColumn
            // 
            this.DataGridViewLocationColumn.HeaderText = "Location";
            this.DataGridViewLocationColumn.Name = "DataGridViewLocationColumn";
            this.DataGridViewLocationColumn.ReadOnly = true;
            // 
            // DataGridViewQuantityColumn
            // 
            this.DataGridViewQuantityColumn.HeaderText = "Quantity";
            this.DataGridViewQuantityColumn.Name = "DataGridViewQuantityColumn";
            this.DataGridViewQuantityColumn.ReadOnly = true;
            // 
            // dataGridViewLotIdColumn
            // 
            this.dataGridViewLotIdColumn.HeaderText = "Lot Id";
            this.dataGridViewLotIdColumn.Name = "dataGridViewLotIdColumn";
            this.dataGridViewLotIdColumn.ReadOnly = true;
            // 
            // DataGridViewLotNumberColumn
            // 
            this.DataGridViewLotNumberColumn.HeaderText = "Lot Number";
            this.DataGridViewLotNumberColumn.Name = "DataGridViewLotNumberColumn";
            this.DataGridViewLotNumberColumn.ReadOnly = true;
            // 
            // txtUOM
            // 
            this.txtUOM.HeaderText = "UOM";
            this.txtUOM.Name = "txtUOM";
            this.txtUOM.ReadOnly = true;
            // 
            // DataGridViewOriginalQuantityColumn
            // 
            this.DataGridViewOriginalQuantityColumn.HeaderText = "Original Quantity";
            this.DataGridViewOriginalQuantityColumn.Name = "DataGridViewOriginalQuantityColumn";
            this.DataGridViewOriginalQuantityColumn.ReadOnly = true;
            // 
            // dataGridViewBalanceQtyColumn
            // 
            this.dataGridViewBalanceQtyColumn.HeaderText = "Balance Quantity";
            this.dataGridViewBalanceQtyColumn.Name = "dataGridViewBalanceQtyColumn";
            this.dataGridViewBalanceQtyColumn.ReadOnly = true;
            // 
            // dataGridViewExpiryDateColumn
            // 
            this.dataGridViewExpiryDateColumn.HeaderText = "Expiry Date";
            this.dataGridViewExpiryDateColumn.Name = "dataGridViewExpiryDateColumn";
            this.dataGridViewExpiryDateColumn.ReadOnly = true;
            // 
            // frmInventoryStockDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(644, 429);
            this.Controls.Add(this.lblLastReceiveDate);
            this.Controls.Add(this.txtLastReceivedDate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtProdDescription);
            this.Controls.Add(this.txtProductCode);
            this.Controls.Add(this.lblProdDescription);
            this.Controls.Add(this.lblProductCode);
            this.Controls.Add(this.dgvItemInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmInventoryStockDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stock Details";
            this.Load += new System.EventHandler(this.frmInventoryStockDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvItemInfo;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.Label lblProdDescription;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.TextBox txtProdDescription;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblLastReceiveDate;
        private System.Windows.Forms.TextBox txtLastReceivedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewLocationColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewQuantityColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewLotIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewLotNumberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataGridViewOriginalQuantityColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewBalanceQtyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewExpiryDateColumn;
    }
}