namespace Semnox.Parafait.Inventory.Recipe
{
    partial class frmCopyRecipeUI
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpSourceToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpSrcFromDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtpDestToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpDestFromDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnConfirmCopy = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(291, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Source To Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(36, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Source From Date:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpSourceToDate);
            this.groupBox1.Controls.Add(this.dtpSrcFromDate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(34, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(536, 100);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source";
            // 
            // dtpSourceToDate
            // 
            this.dtpSourceToDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpSourceToDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.dtpSourceToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSourceToDate.Location = new System.Drawing.Point(294, 50);
            this.dtpSourceToDate.Name = "dtpSourceToDate";
            this.dtpSourceToDate.Size = new System.Drawing.Size(177, 26);
            this.dtpSourceToDate.TabIndex = 65;
            this.dtpSourceToDate.Value = new System.DateTime(2020, 8, 21, 0, 0, 0, 0);
            this.dtpSourceToDate.ValueChanged += new System.EventHandler(this.dtpSourceToDate_ValueChanged);
            // 
            // dtpSrcFromDate
            // 
            this.dtpSrcFromDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpSrcFromDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.dtpSrcFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSrcFromDate.Location = new System.Drawing.Point(39, 50);
            this.dtpSrcFromDate.Name = "dtpSrcFromDate";
            this.dtpSrcFromDate.Size = new System.Drawing.Size(177, 26);
            this.dtpSrcFromDate.TabIndex = 64;
            this.dtpSrcFromDate.Value = new System.DateTime(2020, 8, 21, 0, 0, 0, 0);
            this.dtpSrcFromDate.ValueChanged += new System.EventHandler(this.dtpSrcFromDate_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtpDestToDate);
            this.groupBox2.Controls.Add(this.dtpDestFromDate);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(34, 149);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(536, 100);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Destination";
            // 
            // dtpDestToDate
            // 
            this.dtpDestToDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpDestToDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.dtpDestToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDestToDate.Location = new System.Drawing.Point(294, 56);
            this.dtpDestToDate.Name = "dtpDestToDate";
            this.dtpDestToDate.Size = new System.Drawing.Size(177, 26);
            this.dtpDestToDate.TabIndex = 67;
            this.dtpDestToDate.Value = new System.DateTime(2020, 8, 21, 0, 0, 0, 0);
            // 
            // dtpDestFromDate
            // 
            this.dtpDestFromDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpDestFromDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.dtpDestFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDestFromDate.Location = new System.Drawing.Point(39, 56);
            this.dtpDestFromDate.Name = "dtpDestFromDate";
            this.dtpDestFromDate.Size = new System.Drawing.Size(177, 26);
            this.dtpDestFromDate.TabIndex = 66;
            this.dtpDestFromDate.Value = new System.DateTime(2020, 8, 21, 0, 0, 0, 0);
            this.dtpDestFromDate.ValueChanged += new System.EventHandler(this.dtpDestFromDate_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(291, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 15);
            this.label1.TabIndex = 16;
            this.label1.Text = "Destination To Date:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(36, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "Destination From Date:";
            // 
            // btnConfirmCopy
            // 
            this.btnConfirmCopy.Location = new System.Drawing.Point(34, 255);
            this.btnConfirmCopy.Name = "btnConfirmCopy";
            this.btnConfirmCopy.Size = new System.Drawing.Size(101, 42);
            this.btnConfirmCopy.TabIndex = 13;
            this.btnConfirmCopy.Text = "Confirm Copy";
            this.btnConfirmCopy.UseVisualStyleBackColor = true;
            this.btnConfirmCopy.Click += new System.EventHandler(this.btnConfirmCopy_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(178, 255);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 42);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblMessage.Location = new System.Drawing.Point(-1, 311);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(747, 35);
            this.lblMessage.TabIndex = 15;
            // 
            // frmCopyRecipeUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(616, 346);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirmCopy);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmCopyRecipeUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copy Recipe";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnConfirmCopy;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.DateTimePicker dtpSrcFromDate;
        private System.Windows.Forms.DateTimePicker dtpSourceToDate;
        private System.Windows.Forms.DateTimePicker dtpDestFromDate;
        private System.Windows.Forms.DateTimePicker dtpDestToDate;
    }
}