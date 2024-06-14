namespace Semnox.Parafait.Customer
{
    partial class AddressView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddressView));
            this.lblAddressType = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblAddressType
            // 
            this.lblAddressType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(53)))), ((int)(((byte)(77)))));
            this.lblAddressType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblAddressType.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddressType.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblAddressType.Location = new System.Drawing.Point(-1, 0);
            this.lblAddressType.Name = "lblAddressType";
            this.lblAddressType.Size = new System.Drawing.Size(347, 32);
            this.lblAddressType.TabIndex = 0;
            this.lblAddressType.Text = "Work";
            this.lblAddressType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoEllipsis = true;
            this.lblAddress.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblAddress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblAddress.Location = new System.Drawing.Point(4, 32);
            this.lblAddress.Margin = new System.Windows.Forms.Padding(3);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblAddress.Size = new System.Drawing.Size(330, 121);
            this.lblAddress.TabIndex = 1;
            this.lblAddress.Text = "Semnox Solutions";
            this.lblAddress.Click += new System.EventHandler(this.LblAddress_Click);
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnClose.Location = new System.Drawing.Point(305, -1);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(41, 32);
            this.btnClose.TabIndex = 3;
            this.btnClose.UseCompatibleTextRendering = true;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // AddressView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.lblAddressType);
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Name = "AddressView";
            this.Size = new System.Drawing.Size(346, 157);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAddressType;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblAddress;
    }
}
