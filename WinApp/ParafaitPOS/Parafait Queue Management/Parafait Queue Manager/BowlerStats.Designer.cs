namespace ParafaitQueueManagement
{
    partial class BowlerStats
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
            this.lblFromdate = new System.Windows.Forms.Label();
            this.dtFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtTodate = new System.Windows.Forms.DateTimePicker();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.dgvBowlerStatistics = new System.Windows.Forms.DataGridView();
            this.cmbFromDate = new System.Windows.Forms.ComboBox();
            this.cmbToDate = new System.Windows.Forms.ComboBox();
            this.btnStatExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBowlerStatistics)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFromdate
            // 
            this.lblFromdate.AutoSize = true;
            this.lblFromdate.Location = new System.Drawing.Point(56, 40);
            this.lblFromdate.Name = "lblFromdate";
            this.lblFromdate.Size = new System.Drawing.Size(57, 13);
            this.lblFromdate.TabIndex = 0;
            this.lblFromdate.Text = "From Date";
            // 
            // dtFromDate
            // 
            this.dtFromDate.CustomFormat = "dd-MM-yyyy";
            this.dtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFromDate.Location = new System.Drawing.Point(140, 32);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(133, 21);
            this.dtFromDate.TabIndex = 1;
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(56, 77);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(45, 13);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "To Date";
            // 
            // dtTodate
            // 
            this.dtTodate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTodate.Location = new System.Drawing.Point(140, 71);
            this.dtTodate.Name = "dtTodate";
            this.dtTodate.Size = new System.Drawing.Size(133, 21);
            this.dtTodate.TabIndex = 3;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(451, 33);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 4;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // dgvBowlerStatistics
            // 
            this.dgvBowlerStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBowlerStatistics.Location = new System.Drawing.Point(140, 117);
            this.dgvBowlerStatistics.Name = "dgvBowlerStatistics";
            this.dgvBowlerStatistics.Size = new System.Drawing.Size(275, 222);
            this.dgvBowlerStatistics.TabIndex = 5;
            // 
            // cmbFromDate
            // 
            this.cmbFromDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromDate.FormattingEnabled = true;
            this.cmbFromDate.Location = new System.Drawing.Point(294, 32);
            this.cmbFromDate.Name = "cmbFromDate";
            this.cmbFromDate.Size = new System.Drawing.Size(121, 21);
            this.cmbFromDate.TabIndex = 6;
            // 
            // cmbToDate
            // 
            this.cmbToDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToDate.FormattingEnabled = true;
            this.cmbToDate.Location = new System.Drawing.Point(294, 70);
            this.cmbToDate.Name = "cmbToDate";
            this.cmbToDate.Size = new System.Drawing.Size(121, 21);
            this.cmbToDate.TabIndex = 7;
            // 
            // btnStatExport
            // 
            this.btnStatExport.Location = new System.Drawing.Point(198, 345);
            this.btnStatExport.Name = "btnStatExport";
            this.btnStatExport.Size = new System.Drawing.Size(102, 23);
            this.btnStatExport.TabIndex = 8;
            this.btnStatExport.Text = "Export To Excel";
            this.btnStatExport.UseVisualStyleBackColor = true;
            this.btnStatExport.Click += new System.EventHandler(this.btnStatExport_Click);
            // 
            // BowlerStats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 389);
            this.Controls.Add(this.btnStatExport);
            this.Controls.Add(this.cmbToDate);
            this.Controls.Add(this.cmbFromDate);
            this.Controls.Add(this.dgvBowlerStatistics);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.dtTodate);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.dtFromDate);
            this.Controls.Add(this.lblFromdate);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BowlerStats";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bowler Statistics";
            this.Load += new System.EventHandler(this.BowlerStats_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBowlerStatistics)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFromdate;
        private System.Windows.Forms.DateTimePicker dtFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtTodate;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.DataGridView dgvBowlerStatistics;
        private System.Windows.Forms.ComboBox cmbFromDate;
        private System.Windows.Forms.ComboBox cmbToDate;
        private System.Windows.Forms.Button btnStatExport;
    }
}