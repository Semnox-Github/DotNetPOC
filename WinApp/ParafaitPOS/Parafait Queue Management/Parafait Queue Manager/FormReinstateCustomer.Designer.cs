namespace ParafaitQueueManagement
{
    partial class frmReinstateCustomer
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
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtFromdate = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtToDate = new System.Windows.Forms.DateTimePicker();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.txtCustName = new System.Windows.Forms.TextBox();
            this.chkRemoved = new System.Windows.Forms.CheckBox();
            this.chkExpired = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgReinstateCust = new System.Windows.Forms.DataGridView();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblSrchResult = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgReinstateCust)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.Location = new System.Drawing.Point(19, 21);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(64, 13);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "From Date: ";
            // 
            // dtFromdate
            // 
            this.dtFromdate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.dtFromdate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFromdate.Location = new System.Drawing.Point(110, 15);
            this.dtFromdate.Name = "dtFromdate";
            this.dtFromdate.Size = new System.Drawing.Size(172, 21);
            this.dtFromdate.TabIndex = 1;
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDate.Location = new System.Drawing.Point(309, 21);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(52, 13);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "To Date: ";
            // 
            // dtToDate
            // 
            this.dtToDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.dtToDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtToDate.Location = new System.Drawing.Point(381, 15);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(172, 21);
            this.dtToDate.TabIndex = 3;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.Location = new System.Drawing.Point(19, 83);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(90, 13);
            this.lblCustomerName.TabIndex = 4;
            this.lblCustomerName.Text = "Customer Name: ";
            // 
            // txtCustName
            // 
            this.txtCustName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustName.Location = new System.Drawing.Point(110, 79);
            this.txtCustName.Name = "txtCustName";
            this.txtCustName.Size = new System.Drawing.Size(172, 21);
            this.txtCustName.TabIndex = 5;
            // 
            // chkRemoved
            // 
            this.chkRemoved.AutoSize = true;
            this.chkRemoved.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemoved.Location = new System.Drawing.Point(310, 47);
            this.chkRemoved.Name = "chkRemoved";
            this.chkRemoved.Size = new System.Drawing.Size(71, 17);
            this.chkRemoved.TabIndex = 6;
            this.chkRemoved.Text = "Removed";
            this.chkRemoved.UseVisualStyleBackColor = true;
            // 
            // chkExpired
            // 
            this.chkExpired.AutoSize = true;
            this.chkExpired.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkExpired.Location = new System.Drawing.Point(310, 79);
            this.chkExpired.Name = "chkExpired";
            this.chkExpired.Size = new System.Drawing.Size(62, 17);
            this.chkExpired.TabIndex = 7;
            this.chkExpired.Text = "Expired";
            this.chkExpired.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(489, 75);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(64, 21);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgReinstateCust
            // 
            this.dgReinstateCust.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgReinstateCust.Location = new System.Drawing.Point(22, 119);
            this.dgReinstateCust.Name = "dgReinstateCust";
            this.dgReinstateCust.Size = new System.Drawing.Size(531, 198);
            this.dgReinstateCust.TabIndex = 9;
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.AutoSize = true;
            this.lblCardNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.Location = new System.Drawing.Point(19, 51);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(77, 13);
            this.lblCardNumber.TabIndex = 10;
            this.lblCardNumber.Text = "Card Number: ";
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.Location = new System.Drawing.Point(110, 47);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(172, 21);
            this.txtCardNumber.TabIndex = 11;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(105, 333);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 21);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(205, 333);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 21);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(317, 333);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(64, 21);
            this.btnRefresh.TabIndex = 14;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblSrchResult
            // 
            this.lblSrchResult.AutoSize = true;
            this.lblSrchResult.Location = new System.Drawing.Point(109, 361);
            this.lblSrchResult.Name = "lblSrchResult";
            this.lblSrchResult.Size = new System.Drawing.Size(35, 13);
            this.lblSrchResult.TabIndex = 15;
            this.lblSrchResult.Text = "label1";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(418, 333);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 16;
            this.btnExport.Text = "Get Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // frmReinstateCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 382);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lblSrchResult);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.lblCardNumber);
            this.Controls.Add(this.dgReinstateCust);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.chkExpired);
            this.Controls.Add(this.chkRemoved);
            this.Controls.Add(this.txtCustName);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.dtToDate);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.dtFromdate);
            this.Controls.Add(this.lblFromDate);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReinstateCustomer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Re-instate Customer";
            this.Load += new System.EventHandler(this.frmReinstateCustomer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgReinstateCust)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dtFromdate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtToDate;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.TextBox txtCustName;
        private System.Windows.Forms.CheckBox chkRemoved;
        private System.Windows.Forms.CheckBox chkExpired;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgReinstateCust;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblSrchResult;
        private System.Windows.Forms.Button btnExport;
    }
}