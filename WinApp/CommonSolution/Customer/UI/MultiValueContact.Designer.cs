namespace Semnox.Parafait.Customer
{
    partial class MultiValueContact
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiValueContact));
            this.pnlMultiValueContact = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.pnlMultiValueContact.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMultiValueContact
            // 
            this.pnlMultiValueContact.AutoSize = true;
            this.pnlMultiValueContact.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.pnlMultiValueContact.Controls.Add(this.txtPassword);
            this.pnlMultiValueContact.Controls.Add(this.btnDelete);
            this.pnlMultiValueContact.Controls.Add(this.txtUsername);
            this.pnlMultiValueContact.Location = new System.Drawing.Point(3, 3);
            this.pnlMultiValueContact.Name = "pnlMultiValueContact";
            this.pnlMultiValueContact.Size = new System.Drawing.Size(345, 68);
            this.pnlMultiValueContact.TabIndex = 8;
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtPassword.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(14, 36);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(280, 26);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Text = "PassWord";
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Location = new System.Drawing.Point(302, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(34, 30);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtUsername.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(14, 5);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(280, 26);
            this.txtUsername.TabIndex = 2;
            this.txtUsername.Text = "UserName";
            this.txtUsername.Leave += new System.EventHandler(this.txtUsername_Leave);
            // 
            // MultiValueContact
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlMultiValueContact);
            this.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.Name = "MultiValueContact";
            this.Size = new System.Drawing.Size(349, 75);
            this.pnlMultiValueContact.ResumeLayout(false);
            this.pnlMultiValueContact.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlMultiValueContact;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtUsername;
    }
}
