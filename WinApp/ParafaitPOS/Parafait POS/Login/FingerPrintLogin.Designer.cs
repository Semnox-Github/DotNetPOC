namespace Parafait_POS
{
    partial class FingerPrintLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FingerPrintLogin));
            this.pbPrint = new System.Windows.Forms.PictureBox();
            this.lblSwipeTo = new System.Windows.Forms.Label();
            this.lblNotRegistered = new System.Windows.Forms.Label();
            this.btnRegisterFingerPrint = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLogingId = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnSaveFingerPrint = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lnkLoginUsername = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPrint
            // 
            this.pbPrint.Location = new System.Drawing.Point(107, 71);
            this.pbPrint.Name = "pbPrint";
            this.pbPrint.Size = new System.Drawing.Size(117, 110);
            this.pbPrint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPrint.TabIndex = 0;
            this.pbPrint.TabStop = false;
            // 
            // lblSwipeTo
            // 
            this.lblSwipeTo.AutoSize = true;
            this.lblSwipeTo.Location = new System.Drawing.Point(70, 198);
            this.lblSwipeTo.Name = "lblSwipeTo";
            this.lblSwipeTo.Size = new System.Drawing.Size(195, 14);
            this.lblSwipeTo.TabIndex = 1;
            this.lblSwipeTo.Text = "Please Swipe Your Finger to Login";
            // 
            // lblNotRegistered
            // 
            this.lblNotRegistered.AutoSize = true;
            this.lblNotRegistered.Location = new System.Drawing.Point(22, 227);
            this.lblNotRegistered.Name = "lblNotRegistered";
            this.lblNotRegistered.Size = new System.Drawing.Size(96, 14);
            this.lblNotRegistered.TabIndex = 2;
            this.lblNotRegistered.Text = "Not Registered?";
            // 
            // btnRegisterFingerPrint
            // 
            this.btnRegisterFingerPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnRegisterFingerPrint.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            this.btnRegisterFingerPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRegisterFingerPrint.FlatAppearance.BorderSize = 0;
            this.btnRegisterFingerPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRegisterFingerPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRegisterFingerPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegisterFingerPrint.ForeColor = System.Drawing.Color.White;
            this.btnRegisterFingerPrint.Location = new System.Drawing.Point(124, 223);
            this.btnRegisterFingerPrint.Name = "btnRegisterFingerPrint";
            this.btnRegisterFingerPrint.Size = new System.Drawing.Size(75, 23);
            this.btnRegisterFingerPrint.TabIndex = 3;
            this.btnRegisterFingerPrint.Text = "Register";
            this.btnRegisterFingerPrint.UseVisualStyleBackColor = false;
            this.btnRegisterFingerPrint.Click += new System.EventHandler(this.btnRegisterFingerPrint_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 255);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "Login Id:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 288);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 14);
            this.label4.TabIndex = 5;
            this.label4.Text = "Password:";
            // 
            // txtLogingId
            // 
            this.txtLogingId.Location = new System.Drawing.Point(124, 252);
            this.txtLogingId.Name = "txtLogingId";
            this.txtLogingId.Size = new System.Drawing.Size(127, 20);
            this.txtLogingId.TabIndex = 6;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(124, 285);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(127, 20);
            this.txtPassword.TabIndex = 7;
            // 
            // btnSaveFingerPrint
            // 
            this.btnSaveFingerPrint.Location = new System.Drawing.Point(124, 311);
            this.btnSaveFingerPrint.Name = "btnSaveFingerPrint";
            this.btnSaveFingerPrint.Size = new System.Drawing.Size(75, 23);
            this.btnSaveFingerPrint.TabIndex = 8;
            this.btnSaveFingerPrint.Text = "Register";
            this.btnSaveFingerPrint.UseVisualStyleBackColor = true;
            this.btnSaveFingerPrint.Click += new System.EventHandler(this.btnSaveFingerPrint_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(326, 57);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // lnkLoginUsername
            // 
            this.lnkLoginUsername.AutoSize = true;
            this.lnkLoginUsername.Location = new System.Drawing.Point(246, 232);
            this.lnkLoginUsername.Name = "lnkLoginUsername";
            this.lnkLoginUsername.Size = new System.Drawing.Size(75, 14);
            this.lnkLoginUsername.TabIndex = 10;
            this.lnkLoginUsername.TabStop = true;
            this.lnkLoginUsername.Text = "Use Login Id";
            this.lnkLoginUsername.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLoginUsername_LinkClicked);
            // 
            // FingerPrintLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 250);
            this.Controls.Add(this.lnkLoginUsername);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnSaveFingerPrint);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtLogingId);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnRegisterFingerPrint);
            this.Controls.Add(this.lblNotRegistered);
            this.Controls.Add(this.lblSwipeTo);
            this.Controls.Add(this.pbPrint);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FingerPrintLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parafait POS - Finger Print Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FingerPrintLogin_FormClosed);
            this.Load += new System.EventHandler(this.FingerPrintLogin_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FingerPrintLogin_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pbPrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPrint;
        private System.Windows.Forms.Label lblSwipeTo;
        private System.Windows.Forms.Label lblNotRegistered;
        private System.Windows.Forms.Button btnRegisterFingerPrint;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLogingId;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnSaveFingerPrint;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel lnkLoginUsername;
    }
}