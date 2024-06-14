namespace Semnox.Parafait.Product
{
    partial class frmlistBarCodes
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
            this.lblHeading = new System.Windows.Forms.Label();
            this.barcode_dgv = new System.Windows.Forms.DataGridView();
            this.productBarcodeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.barCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.barcode_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productBarcodeBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.Location = new System.Drawing.Point(24, 14);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(85, 17);
            this.lblHeading.TabIndex = 33;
            this.lblHeading.Text = "lblHeading";
            // 
            // barcode_dgv
            // 
            this.barcode_dgv.AllowUserToAddRows = false;
            this.barcode_dgv.AllowUserToDeleteRows = false;
            this.barcode_dgv.AutoGenerateColumns = false;
            this.barcode_dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.barcode_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.barcode_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.barCodeDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn});
            this.barcode_dgv.DataSource = this.productBarcodeBindingSource;
            this.barcode_dgv.GridColor = System.Drawing.Color.DarkOliveGreen;
            this.barcode_dgv.Location = new System.Drawing.Point(27, 46);
            this.barcode_dgv.Name = "barcode_dgv";
            this.barcode_dgv.ReadOnly = true;
            this.barcode_dgv.RowHeadersVisible = false;
            this.barcode_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.barcode_dgv.Size = new System.Drawing.Size(321, 201);
            this.barcode_dgv.TabIndex = 34;
            // 
            // productBarcodeBindingSource
            // 
            this.productBarcodeBindingSource.DataSource = typeof(Semnox.Parafait.Product.ProductBarcodeDTO);
            // 
            // barCodeDataGridViewTextBoxColumn
            // 
            this.barCodeDataGridViewTextBoxColumn.DataPropertyName = "BarCode";
            this.barCodeDataGridViewTextBoxColumn.HeaderText = "BarCode";
            this.barCodeDataGridViewTextBoxColumn.Name = "barCodeDataGridViewTextBoxColumn";
            this.barCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isActiveDataGridViewTextBoxColumn
            // 
            this.isActiveDataGridViewTextBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn.HeaderText = "IsActive";
            this.isActiveDataGridViewTextBoxColumn.Name = "isActiveDataGridViewTextBoxColumn";
            this.isActiveDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // frmlistBarCodes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 279);
            this.Controls.Add(this.barcode_dgv);
            this.Controls.Add(this.lblHeading);
            this.Name = "frmlistBarCodes";
            this.Text = "Bar Code";
            this.Load += new System.EventHandler(this.frmlistBarCodes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barcode_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productBarcodeBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.DataGridView barcode_dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn barCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn isActiveDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource productBarcodeBindingSource;
    }
}