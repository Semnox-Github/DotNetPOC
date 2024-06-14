using Semnox.Parafait.Category;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Product;
using Semnox.Parafait.Vendor;

namespace Semnox.Parafait.Inventory
{
    partial class frmUploadProduct
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
            this.btnFormat = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbInboundLocation = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.loactionDTObindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbOutboundLocation = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.locationDTOBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.cmbCategory = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.CategoryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbVendor = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.VendorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblMessage = new System.Windows.Forms.Label();
            this.lnkError = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbUOM = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.UOMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnCreateProduct = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbTax = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.TaxBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnDownload = new System.Windows.Forms.Button();
            this.cbxIsPurchasable = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.loactionDTObindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationDTOBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VendorBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UOMBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaxBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFormat
            // 
            this.btnFormat.Location = new System.Drawing.Point(8, 225);
            this.btnFormat.Name = "btnFormat";
            this.btnFormat.Size = new System.Drawing.Size(75, 23);
            this.btnFormat.TabIndex = 0;
            this.btnFormat.Text = "File Format";
            this.btnFormat.UseVisualStyleBackColor = true;
            this.btnFormat.Click += new System.EventHandler(this.btnFormat_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(256, 225);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(359, 225);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbInboundLocation
            // 
            this.cmbInboundLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbInboundLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbInboundLocation.DataSource = this.loactionDTObindingSource1;
            this.cmbInboundLocation.DisplayMember = "Name";
            this.cmbInboundLocation.FormattingEnabled = true;
            this.cmbInboundLocation.Location = new System.Drawing.Point(201, 54);
            this.cmbInboundLocation.Name = "cmbInboundLocation";
            this.cmbInboundLocation.Size = new System.Drawing.Size(121, 21);
            this.cmbInboundLocation.TabIndex = 3;
            this.cmbInboundLocation.ValueMember = "LocationId";
            // 
            // loactionDTObindingSource1
            // 
            this.loactionDTObindingSource1.DataSource = typeof(Semnox.Parafait.Inventory.LocationDTO);
            this.loactionDTObindingSource1.Filter = "ISActive=\'Y\'";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Inbound Location:";
            // 
            // cmbOutboundLocation
            // 
            this.cmbOutboundLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbOutboundLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbOutboundLocation.DataSource = this.locationDTOBindingSource2;
            this.cmbOutboundLocation.DisplayMember = "Name";
            this.cmbOutboundLocation.FormattingEnabled = true;
            this.cmbOutboundLocation.Location = new System.Drawing.Point(201, 83);
            this.cmbOutboundLocation.Name = "cmbOutboundLocation";
            this.cmbOutboundLocation.Size = new System.Drawing.Size(121, 21);
            this.cmbOutboundLocation.TabIndex = 5;
            this.cmbOutboundLocation.ValueMember = "LocationId";
            // 
            // locationDTOBindingSource2
            // 
            this.locationDTOBindingSource2.DataSource = typeof(Semnox.Parafait.Inventory.LocationDTO);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(96, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Outbound Location:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCategory.DataSource = this.CategoryBindingSource;
            this.cmbCategory.DisplayMember = "Name";
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(201, 25);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(121, 21);
            this.cmbCategory.TabIndex = 7;
            this.cmbCategory.ValueMember = "CategoryId";
            // 
            // CategoryBindingSource
            // 
            this.CategoryBindingSource.DataSource = typeof(Semnox.Parafait.Category.CategoryDTO);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Category:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "Defaults";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(153, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Vendor:";
            // 
            // cmbVendor
            // 
            this.cmbVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbVendor.DataSource = this.VendorBindingSource;
            this.cmbVendor.DisplayMember = "Name";
            this.cmbVendor.FormattingEnabled = true;
            this.cmbVendor.Location = new System.Drawing.Point(201, 112);
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.Size = new System.Drawing.Size(121, 21);
            this.cmbVendor.TabIndex = 10;
            this.cmbVendor.ValueMember = "VendorId";
            // 
            // VendorBindingSource
            // 
            this.VendorBindingSource.DataSource = typeof(Semnox.Parafait.Vendor.VendorDTO);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(13, 255);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(50, 13);
            this.lblMessage.TabIndex = 12;
            this.lblMessage.Text = "Message";
            // 
            // lnkError
            // 
            this.lnkError.AutoSize = true;
            this.lnkError.Location = new System.Drawing.Point(276, 255);
            this.lnkError.Name = "lnkError";
            this.lnkError.Size = new System.Drawing.Size(34, 13);
            this.lnkError.TabIndex = 13;
            this.lnkError.TabStop = true;
            this.lnkError.Text = "Errors";
            this.lnkError.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkError_LinkClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(162, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "UOM:";
            // 
            // cmbUOM
            // 
            this.cmbUOM.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbUOM.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbUOM.DataSource = this.UOMBindingSource;
            this.cmbUOM.DisplayMember = "UOM";
            this.cmbUOM.FormattingEnabled = true;
            this.cmbUOM.Location = new System.Drawing.Point(201, 141);
            this.cmbUOM.Name = "cmbUOM";
            this.cmbUOM.Size = new System.Drawing.Size(121, 21);
            this.cmbUOM.TabIndex = 14;
            this.cmbUOM.ValueMember = "UOMId";
            // 
            // UOMBindingSource
            // 
            this.UOMBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.UOMDTO);
            // 
            // btnCreateProduct
            // 
            this.btnCreateProduct.Location = new System.Drawing.Point(63, 225);
            this.btnCreateProduct.Name = "btnCreateProduct";
            this.btnCreateProduct.Size = new System.Drawing.Size(75, 23);
            this.btnCreateProduct.TabIndex = 16;
            this.btnCreateProduct.Text = "Create";
            this.btnCreateProduct.UseVisualStyleBackColor = true;
            this.btnCreateProduct.Visible = false;
            this.btnCreateProduct.Click += new System.EventHandler(this.btnCreateProduct_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(169, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Tax:";
            // 
            // cmbTax
            // 
            this.cmbTax.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbTax.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTax.DataSource = this.TaxBindingSource;
            this.cmbTax.DisplayMember = "TaxName";
            this.cmbTax.FormattingEnabled = true;
            this.cmbTax.Location = new System.Drawing.Point(201, 171);
            this.cmbTax.Name = "cmbTax";
            this.cmbTax.Size = new System.Drawing.Size(121, 21);
            this.cmbTax.TabIndex = 17;
            this.cmbTax.ValueMember = "TaxId";
            // 
            // TaxBindingSource
            // 
            this.TaxBindingSource.DataSource = typeof(Semnox.Parafait.Product.TaxDTO);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(156, 225);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 19;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // cbxIsPurchasable
            // 
            this.cbxIsPurchasable.AutoSize = true;
            this.cbxIsPurchasable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxIsPurchasable.Checked = true;
            this.cbxIsPurchasable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxIsPurchasable.Location = new System.Drawing.Point(113, 200);
            this.cbxIsPurchasable.Name = "cbxIsPurchasable";
            this.cbxIsPurchasable.Size = new System.Drawing.Size(102, 17);
            this.cbxIsPurchasable.TabIndex = 20;
            this.cbxIsPurchasable.Text = "Inventory Item?:";
            this.cbxIsPurchasable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxIsPurchasable.UseVisualStyleBackColor = true;
            // 
            // frmUploadProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(456, 276);
            this.Controls.Add(this.cbxIsPurchasable);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbTax);
            this.Controls.Add(this.btnCreateProduct);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbUOM);
            this.Controls.Add(this.lnkError);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbVendor);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbOutboundLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbInboundLocation);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnFormat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmUploadProduct";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Upload Products";
            this.Load += new System.EventHandler(this.UploadProducts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.loactionDTObindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationDTOBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CategoryBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VendorBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UOMBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaxBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFormat;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnCancel;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbInboundLocation;
        private System.Windows.Forms.Label label1;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbOutboundLocation;
        private System.Windows.Forms.Label label2;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbCategory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbVendor;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.LinkLabel lnkError;
        private System.Windows.Forms.Label label6;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbUOM;
        private System.Windows.Forms.Button btnCreateProduct;
        private System.Windows.Forms.Label label7;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbTax;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.BindingSource CategoryBindingSource;
        private System.Windows.Forms.BindingSource loactionDTObindingSource1;
        private System.Windows.Forms.BindingSource locationDTOBindingSource2;
        private System.Windows.Forms.BindingSource VendorBindingSource;
        private System.Windows.Forms.BindingSource UOMBindingSource;
        private System.Windows.Forms.BindingSource TaxBindingSource;
        private System.Windows.Forms.CheckBox cbxIsPurchasable;
    }
}