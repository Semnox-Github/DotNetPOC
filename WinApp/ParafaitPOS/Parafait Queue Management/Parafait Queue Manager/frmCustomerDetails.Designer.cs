namespace ParafaitQueueManagement
{
    partial class frmCustomerDetails
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
            this.picCustomer = new System.Windows.Forms.PictureBox();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblCustomerNameValue = new System.Windows.Forms.Label();
            this.lblCardNo = new System.Windows.Forms.Label();
            this.lblCardNoValue = new System.Windows.Forms.Label();
            this.lblMobNo = new System.Windows.Forms.Label();
            this.lblMobNoValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picCustomer)).BeginInit();
            this.SuspendLayout();
            // 
            // picCustomer
            // 
            this.picCustomer.BackColor = System.Drawing.Color.Transparent;
            this.picCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picCustomer.Location = new System.Drawing.Point(12, 23);
            this.picCustomer.Name = "picCustomer";
            this.picCustomer.Size = new System.Drawing.Size(106, 77);
            this.picCustomer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCustomer.TabIndex = 0;
            this.picCustomer.TabStop = false;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Location = new System.Drawing.Point(128, 24);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(87, 13);
            this.lblCustomerName.TabIndex = 1;
            this.lblCustomerName.Text = "Customer Name:";
            // 
            // lblCustomerNameValue
            // 
            this.lblCustomerNameValue.AutoSize = true;
            this.lblCustomerNameValue.Location = new System.Drawing.Point(211, 24);
            this.lblCustomerNameValue.Name = "lblCustomerNameValue";
            this.lblCustomerNameValue.Size = new System.Drawing.Size(35, 13);
            this.lblCustomerNameValue.TabIndex = 2;
            this.lblCustomerNameValue.Text = "label2";
            // 
            // lblCardNo
            // 
            this.lblCardNo.AutoSize = true;
            this.lblCardNo.Location = new System.Drawing.Point(128, 49);
            this.lblCardNo.Name = "lblCardNo";
            this.lblCardNo.Size = new System.Drawing.Size(50, 13);
            this.lblCardNo.TabIndex = 3;
            this.lblCardNo.Text = "Card No:";
            // 
            // lblCardNoValue
            // 
            this.lblCardNoValue.AutoSize = true;
            this.lblCardNoValue.Location = new System.Drawing.Point(211, 49);
            this.lblCardNoValue.Name = "lblCardNoValue";
            this.lblCardNoValue.Size = new System.Drawing.Size(35, 13);
            this.lblCardNoValue.TabIndex = 4;
            this.lblCardNoValue.Text = "label4";
            // 
            // lblMobNo
            // 
            this.lblMobNo.AutoSize = true;
            this.lblMobNo.Location = new System.Drawing.Point(128, 72);
            this.lblMobNo.Name = "lblMobNo";
            this.lblMobNo.Size = new System.Drawing.Size(57, 13);
            this.lblMobNo.TabIndex = 5;
            this.lblMobNo.Text = "Mobile No:";
            // 
            // lblMobNoValue
            // 
            this.lblMobNoValue.AutoSize = true;
            this.lblMobNoValue.Location = new System.Drawing.Point(211, 72);
            this.lblMobNoValue.Name = "lblMobNoValue";
            this.lblMobNoValue.Size = new System.Drawing.Size(35, 13);
            this.lblMobNoValue.TabIndex = 6;
            this.lblMobNoValue.Text = "label6";
            // 
            // frmCustomerDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 122);
            this.Controls.Add(this.lblMobNoValue);
            this.Controls.Add(this.lblMobNo);
            this.Controls.Add(this.lblCardNoValue);
            this.Controls.Add(this.lblCardNo);
            this.Controls.Add(this.lblCustomerNameValue);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.picCustomer);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmCustomerDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Details";
            this.Load += new System.EventHandler(this.frmCustomerDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCustomer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picCustomer;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblCustomerNameValue;
        private System.Windows.Forms.Label lblCardNo;
        private System.Windows.Forms.Label lblCardNoValue;
        private System.Windows.Forms.Label lblMobNo;
        private System.Windows.Forms.Label lblMobNoValue;
    }
}