namespace Semnox.Parafait.DigitalSignage
{
    partial class PreviewUI
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
            if(disposing && (components != null))
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
            this.lblAsOn = new System.Windows.Forms.Label();
            this.dtpAsOnDate = new System.Windows.Forms.DateTimePicker();
            this.lblTime = new System.Windows.Forms.Label();
            this.dtpAsOnTime = new System.Windows.Forms.DateTimePicker();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblAsOn
            // 
            this.lblAsOn.AutoSize = true;
            this.lblAsOn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblAsOn.Location = new System.Drawing.Point(51, 38);
            this.lblAsOn.Name = "lblAsOn";
            this.lblAsOn.Size = new System.Drawing.Size(47, 15);
            this.lblAsOn.TabIndex = 0;
            this.lblAsOn.Text = "As On :";
            // 
            // dtpAsOnDate
            // 
            this.dtpAsOnDate.CustomFormat = "dddd, dd-MMM-yyyy";
            this.dtpAsOnDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAsOnDate.Location = new System.Drawing.Point(104, 36);
            this.dtpAsOnDate.Name = "dtpAsOnDate";
            this.dtpAsOnDate.Size = new System.Drawing.Size(169, 20);
            this.dtpAsOnDate.TabIndex = 1;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblTime.Location = new System.Drawing.Point(279, 38);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(41, 15);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "Time :";
            // 
            // dtpAsOnTime
            // 
            this.dtpAsOnTime.CustomFormat = "hh:mm tt";
            this.dtpAsOnTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpAsOnTime.Location = new System.Drawing.Point(326, 36);
            this.dtpAsOnTime.Name = "dtpAsOnTime";
            this.dtpAsOnTime.ShowUpDown = true;
            this.dtpAsOnTime.Size = new System.Drawing.Size(70, 20);
            this.dtpAsOnTime.TabIndex = 3;
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPreview.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnPreview.Location = new System.Drawing.Point(130, 81);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 22;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(235, 81);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // PreviewUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 146);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dtpAsOnTime);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.dtpAsOnDate);
            this.Controls.Add(this.lblAsOn);
            this.Name = "PreviewUI";
            this.Text = "Preview";
            this.Load += new System.EventHandler(this.PreviewUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAsOn;
        private System.Windows.Forms.DateTimePicker dtpAsOnDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.DateTimePicker dtpAsOnTime;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnClose;
    }
}