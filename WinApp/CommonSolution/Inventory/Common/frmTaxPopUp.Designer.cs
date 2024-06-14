namespace Semnox.Parafait.Inventory
{
    partial class frmTaxPopUp
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
            this.dgv_purchaseTax = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_purchaseTax)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_purchaseTax
            // 
            this.dgv_purchaseTax.AllowUserToAddRows = false;
            this.dgv_purchaseTax.AllowUserToDeleteRows = false;
            this.dgv_purchaseTax.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_purchaseTax.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductCode,
            this.TaxName,
            this.TaxDetails,
            this.TaxAmount});
            this.dgv_purchaseTax.Location = new System.Drawing.Point(12, 12);
            this.dgv_purchaseTax.Name = "dgv_purchaseTax";
            this.dgv_purchaseTax.Size = new System.Drawing.Size(638, 227);
            this.dgv_purchaseTax.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(300, 270);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(94, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ProductCode
            // 
            this.ProductCode.HeaderText = "Product Description";
            this.ProductCode.Name = "ProductCode";
            this.ProductCode.ReadOnly = true;
            this.ProductCode.Width = 170;
            // 
            // TaxName
            // 
            this.TaxName.HeaderText = "Tax Name";
            this.TaxName.Name = "TaxName";
            this.TaxName.ReadOnly = true;
            // 
            // TaxDetails
            // 
            this.TaxDetails.HeaderText = "Tax Details";
            this.TaxDetails.Name = "TaxDetails";
            this.TaxDetails.ReadOnly = true;
            this.TaxDetails.Width = 175;
            // 
            // TaxAmount
            // 
            this.TaxAmount.HeaderText = "TaxAmount";
            this.TaxAmount.Name = "TaxAmount";
            this.TaxAmount.ReadOnly = true;
            this.TaxAmount.Width = 150;
            // 
            // frmTaxPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 338);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgv_purchaseTax);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTaxPopUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TaxPopUp";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_purchaseTax)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_purchaseTax;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxAmount;
    }
}