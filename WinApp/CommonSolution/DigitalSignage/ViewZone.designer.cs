namespace Semnox.Parafait.DigitalSignage
{
    partial class ViewZone
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
            this.grpZoneDetails = new System.Windows.Forms.GroupBox();
            this.panelZone = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.grpZoneDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpZoneDetails
            // 
            this.grpZoneDetails.Controls.Add(this.panelZone);
            this.grpZoneDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpZoneDetails.Location = new System.Drawing.Point(51, 32);
            this.grpZoneDetails.Name = "grpZoneDetails";
            this.grpZoneDetails.Size = new System.Drawing.Size(799, 452);
            this.grpZoneDetails.TabIndex = 14;
            this.grpZoneDetails.TabStop = false;
            // 
            // panelZone
            // 
            this.panelZone.Location = new System.Drawing.Point(27, 20);
            this.panelZone.Name = "panelZone";
            this.panelZone.Size = new System.Drawing.Size(754, 415);
            this.panelZone.TabIndex = 12;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(399, 509);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.Close_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(48, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(37, 16);
            this.lblName.TabIndex = 15;
            this.lblName.Text = "Name";
            // 
            // ViewZone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 560);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.grpZoneDetails);
            this.Controls.Add(this.btnClose);
            this.Name = "ViewZone";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ViewZone";
            this.Load += new System.EventHandler(this.ViewZone_Load);
            this.grpZoneDetails.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpZoneDetails;
        private System.Windows.Forms.Panel panelZone;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblName;
    }
}