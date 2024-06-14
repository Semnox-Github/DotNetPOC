namespace Parafait_POS.Redemption
{
    partial class frmLoginCheckDecommission
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.pictureBoxUser = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.pictureBoxPassword = new System.Windows.Forms.PictureBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUser)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPassword)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Parafait_POS.Properties.Resources.password_button;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.txtLogin);
            this.panel1.Controls.Add(this.pictureBoxUser);
            this.panel1.Location = new System.Drawing.Point(32, 54);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(335, 55);
            this.panel1.TabIndex = 0;
            // 
            // txtLogin
            // 
            this.txtLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(126)))), ((int)(((byte)(222)))));
            this.txtLogin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLogin.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(252)))), ((int)(((byte)(216)))));
            this.txtLogin.Location = new System.Drawing.Point(42, 16);
            this.txtLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(290, 25);
            this.txtLogin.TabIndex = 0;
            this.txtLogin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLogin_KeyDown);
            // 
            // pictureBoxUser
            // 
            this.pictureBoxUser.BackgroundImage = global::Parafait_POS.Properties.Resources.username_icon;
            this.pictureBoxUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxUser.Location = new System.Drawing.Point(6, 0);
            this.pictureBoxUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBoxUser.Name = "pictureBoxUser";
            this.pictureBoxUser.Size = new System.Drawing.Size(32, 55);
            this.pictureBoxUser.TabIndex = 1;
            this.pictureBoxUser.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::Parafait_POS.Properties.Resources.password_button;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.txtPassword);
            this.panel2.Controls.Add(this.pictureBoxPassword);
            this.panel2.Location = new System.Drawing.Point(32, 132);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(335, 55);
            this.panel2.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(126)))), ((int)(((byte)(222)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(252)))), ((int)(((byte)(216)))));
            this.txtPassword.Location = new System.Drawing.Point(42, 16);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(290, 25);
            this.txtPassword.TabIndex = 0;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // pictureBoxPassword
            // 
            this.pictureBoxPassword.BackgroundImage = global::Parafait_POS.Properties.Resources.password_icon;
            this.pictureBoxPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxPassword.Location = new System.Drawing.Point(6, 0);
            this.pictureBoxPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBoxPassword.Name = "pictureBoxPassword";
            this.pictureBoxPassword.Size = new System.Drawing.Size(32, 55);
            this.pictureBoxPassword.TabIndex = 1;
            this.pictureBoxPassword.TabStop = false;
            // 
            // OKButton
            // 
            this.OKButton.BackColor = System.Drawing.Color.Transparent;
            this.OKButton.BackgroundImage = global::Parafait_POS.Properties.Resources.login_button_normal;
            this.OKButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.OKButton.FlatAppearance.BorderSize = 0;
            this.OKButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.OKButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.OKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OKButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OKButton.ForeColor = System.Drawing.Color.White;
            this.OKButton.Location = new System.Drawing.Point(192, 205);
            this.OKButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(193, 51);
            this.OKButton.TabIndex = 3;
            this.OKButton.Text = "     Login";
            this.OKButton.UseVisualStyleBackColor = false;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            this.OKButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OKButton_MouseDown);
            this.OKButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OKButton_MouseUp);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.Black;
            this.cancelButton.Location = new System.Drawing.Point(339, 1);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(28, 37);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "X";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(57)))), ((int)(((byte)(57)))));
            this.lblHeader.Location = new System.Drawing.Point(47, 12);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(286, 20);
            this.lblHeader.TabIndex = 2;
            this.lblHeader.Text = "Parafait User Login";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmLoginCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_POS.Properties.Resources.login_bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(393, 270);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLoginCheck";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmLoginCheck";
            this.Load += new System.EventHandler(this.frmLoginCheck_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUser)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPassword)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.PictureBox pictureBoxUser;
        private System.Windows.Forms.PictureBox pictureBoxPassword;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtPassword;
    }
}