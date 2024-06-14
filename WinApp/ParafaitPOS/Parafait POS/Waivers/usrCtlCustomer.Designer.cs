using System;

namespace Parafait_POS.Waivers
{
    partial class usrCtlCustomer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblEmailId = new System.Windows.Forms.Label();
            this.pbxSigned = new System.Windows.Forms.PictureBox();
            this.btnSignWaiver = new System.Windows.Forms.Button();
            this.lblCustomerName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSigned)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEmailId
            // 
            this.lblEmailId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))));
            this.lblEmailId.AutoEllipsis = true;
            this.lblEmailId.AutoSize = false;
            this.lblEmailId.BackColor = System.Drawing.Color.Transparent;
            this.lblEmailId.ForeColor = System.Drawing.Color.Black;
            this.lblEmailId.Location = new System.Drawing.Point(56, 3);
            this.lblEmailId.Name = "lblEmailId";
            this.lblEmailId.Size = new System.Drawing.Size(250, 28);
            this.lblEmailId.MinimumSize = new System.Drawing.Size(250, 28);
            this.lblEmailId.TabIndex = 5;
            this.lblEmailId.Text = "Email";
            this.lblEmailId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblEmailId.Click += new System.EventHandler(this.lblCustomerName_Click);
            // 
            // pbxSigned
            // 
            this.pbxSigned.BackgroundImage = global::Parafait_POS.Properties.Resources.OK;
            this.pbxSigned.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxSigned.Location = new System.Drawing.Point(13, 3);
            this.pbxSigned.Name = "pbxSigned";
            this.pbxSigned.Size = new System.Drawing.Size(30, 30);
            this.pbxSigned.TabIndex = 4;
            this.pbxSigned.TabStop = false;
            this.pbxSigned.Click += new System.EventHandler(this.btnSignWaiver_Click);
            // 
            // btnSignWaiver
            // 
            this.btnSignWaiver.BackgroundImage = global::Parafait_POS.Properties.Resources.pressed1;
            this.btnSignWaiver.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSignWaiver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignWaiver.Location = new System.Drawing.Point(3, 3);
            this.btnSignWaiver.Name = "btnSignWaiver";
            this.btnSignWaiver.Size = new System.Drawing.Size(49, 30);
            this.btnSignWaiver.TabIndex = 3;
            this.btnSignWaiver.Text = "Sign";
            this.btnSignWaiver.UseVisualStyleBackColor = true;
            this.btnSignWaiver.Click += new System.EventHandler(this.btnSignWaiver_Click);
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom))));
            this.lblCustomerName.AutoEllipsis = true;
            this.lblCustomerName.AutoSize = false;
            this.lblCustomerName.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomerName.ForeColor = System.Drawing.Color.Black;
            this.lblCustomerName.Location = new System.Drawing.Point(307, 3);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(120, 28);
            this.lblCustomerName.MinimumSize = new System.Drawing.Size(120, 28);
            this.lblCustomerName.MaximumSize = new System.Drawing.Size(120, 28);
            this.lblCustomerName.TabIndex = 2;
            this.lblCustomerName.Text = "Customers";
            this.lblCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomerName.Click += new System.EventHandler(this.lblCustomerName_Click);
            // 
            // usrCtlCustomer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Controls.Add(this.lblEmailId);
            this.Controls.Add(this.pbxSigned);
            this.Controls.Add(this.btnSignWaiver);
            this.Controls.Add(this.lblCustomerName);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "usrCtlCustomer";
            this.Size = new System.Drawing.Size(432, 35);
            this.Leave += new System.EventHandler(this.usrCtlCustomer_Leave);
            this.MouseEnter += new System.EventHandler(this.usrCtlCustomer_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.usrCtlCustomer_MouseLeave);
            this.MouseHover += new System.EventHandler(this.usrCtlCustomer_MouseHover);
            ((System.ComponentModel.ISupportInitialize)(this.pbxSigned)).EndInit();
            this.ResumeLayout(false);

        }
        
        #endregion
        private System.Windows.Forms.Button btnSignWaiver;
        private System.Windows.Forms.PictureBox pbxSigned;
        private System.Windows.Forms.Label lblEmailId;
        private System.Windows.Forms.Label lblCustomerName;
    }
}
