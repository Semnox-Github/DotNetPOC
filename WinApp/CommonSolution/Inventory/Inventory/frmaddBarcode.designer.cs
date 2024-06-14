using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory
{
    partial class frmAddBarcode
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
            this.lblBarcodeid = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.ddlactive = new System.Windows.Forms.ComboBox();
            this.txtbarcode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_generateBarcode = new System.Windows.Forms.Button();
            this.lblHeading = new System.Windows.Forms.Label();
            this.productBarcode_dgv = new System.Windows.Forms.DataGridView();
            this.barCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productBarcodeDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.productBarcode_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productBarcodeDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBarcodeid
            // 
            this.lblBarcodeid.AutoSize = true;
            this.lblBarcodeid.Location = new System.Drawing.Point(154, 199);
            this.lblBarcodeid.Name = "lblBarcodeid";
            this.lblBarcodeid.Size = new System.Drawing.Size(0, 13);
            this.lblBarcodeid.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(86, 199);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Barcode Id:";
            // 
            // btnNew
            // 
            this.btnNew.Image = global::Semnox.Parafait.Inventory.Properties.Resources.add1;
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(169, 299);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 20;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Semnox.Parafait.Inventory.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(298, 299);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Image = global::Semnox.Parafait.Inventory.Properties.Resources.cancel;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(44, 299);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 19;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // ddlactive
            // 
            this.ddlactive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlactive.FormattingEnabled = true;
            this.ddlactive.Items.AddRange(new object[] {
            "Y",
            "N"});
            this.ddlactive.Location = new System.Drawing.Point(154, 248);
            this.ddlactive.Name = "ddlactive";
            this.ddlactive.Size = new System.Drawing.Size(57, 21);
            this.ddlactive.TabIndex = 17;
            // 
            // txtbarcode
            // 
            this.txtbarcode.Location = new System.Drawing.Point(154, 221);
            this.txtbarcode.Name = "txtbarcode";
            this.txtbarcode.Size = new System.Drawing.Size(114, 20);
            this.txtbarcode.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(107, 251);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Active?";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 223);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Barcode:";
            // 
            // btn_generateBarcode
            // 
            this.btn_generateBarcode.FlatAppearance.BorderSize = 0;
            this.btn_generateBarcode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SeaGreen;
            this.btn_generateBarcode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.btn_generateBarcode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_generateBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_generateBarcode.ForeColor = System.Drawing.SystemColors.Control;
            this.btn_generateBarcode.Location = new System.Drawing.Point(271, 222);
            this.btn_generateBarcode.Name = "btn_generateBarcode";
            this.btn_generateBarcode.Size = new System.Drawing.Size(113, 23);
            this.btn_generateBarcode.TabIndex = 36;
            this.btn_generateBarcode.Text = "Generate Barcode..";
            this.btn_generateBarcode.UseVisualStyleBackColor = true;
            this.btn_generateBarcode.Click += new System.EventHandler(this.btn_generateBarcode_Click);
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.Location = new System.Drawing.Point(38, 12);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(84, 16);
            this.lblHeading.TabIndex = 37;
            this.lblHeading.Text = "lblHeading";
            // 
            // productBarcode_dgv
            // 
            this.productBarcode_dgv.AllowUserToAddRows = false;
            this.productBarcode_dgv.AllowUserToDeleteRows = false;
            this.productBarcode_dgv.AutoGenerateColumns = false;
            this.productBarcode_dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.productBarcode_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productBarcode_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.barCodeDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn});
            this.productBarcode_dgv.DataSource = this.productBarcodeDTOListBS;
            this.productBarcode_dgv.GridColor = System.Drawing.Color.DarkOliveGreen;
            this.productBarcode_dgv.Location = new System.Drawing.Point(44, 40);
            this.productBarcode_dgv.Name = "productBarcode_dgv";
            this.productBarcode_dgv.ReadOnly = true;
            this.productBarcode_dgv.RowHeadersVisible = false;
            this.productBarcode_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.productBarcode_dgv.Size = new System.Drawing.Size(329, 154);
            this.productBarcode_dgv.TabIndex = 15;
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
            // productBarcodeDTOListBS
            // 
            this.productBarcodeDTOListBS.DataSource = typeof(ProductBarcodeDTO);
            this.productBarcodeDTOListBS.CurrentChanged += new System.EventHandler(this.productBarcodeDTOListBS_CurrentChanged);
            // 
            // frmaddBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(416, 338);
            this.Controls.Add(this.productBarcode_dgv);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.btn_generateBarcode);
            this.Controls.Add(this.lblBarcodeid);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.ddlactive);
            this.Controls.Add(this.txtbarcode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmaddBarcode";
            this.Text = "Add Barcode";
            this.Load += new System.EventHandler(this.frm_addBarcode_Load);
            this.Activated += new System.EventHandler(this.frm_addBarcode_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.productBarcode_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productBarcodeDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBarcodeid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ComboBox ddlactive;
        private System.Windows.Forms.TextBox txtbarcode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_generateBarcode;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.DataGridView productBarcode_dgv;
        private System.Windows.Forms.BindingSource productBarcodeDTOListBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn barCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn isActiveDataGridViewTextBoxColumn;
    }


}
