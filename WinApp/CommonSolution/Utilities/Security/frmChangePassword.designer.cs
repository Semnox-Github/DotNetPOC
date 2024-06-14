namespace Semnox.Core.Utilities
{
    partial class frmChangePassword
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtLoginId = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtReenterPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnChange = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Login ID:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLoginId
            // 
            this.txtLoginId.Location = new System.Drawing.Point(115, 33);
            this.txtLoginId.Name = "txtLoginId";
            this.txtLoginId.Size = new System.Drawing.Size(155, 22);
            this.txtLoginId.TabIndex = 1;
            this.txtLoginId.Enter += new System.EventHandler(this.txtLoginId_Enter);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(115, 61);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = 'x';
            this.txtPassword.Size = new System.Drawing.Size(155, 22);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.Enter += new System.EventHandler(this.txtLoginId_Enter);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Location = new System.Drawing.Point(115, 89);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = 'x';
            this.txtNewPassword.Size = new System.Drawing.Size(155, 22);
            this.txtNewPassword.TabIndex = 5;
            this.txtNewPassword.Enter += new System.EventHandler(this.txtLoginId_Enter);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 92);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "New Password:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReenterPassword
            // 
            this.txtReenterPassword.Location = new System.Drawing.Point(115, 117);
            this.txtReenterPassword.Name = "txtReenterPassword";
            this.txtReenterPassword.PasswordChar = 'x';
            this.txtReenterPassword.Size = new System.Drawing.Size(155, 22);
            this.txtReenterPassword.TabIndex = 7;
            this.txtReenterPassword.Enter += new System.EventHandler(this.txtLoginId_Enter);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 120);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Re-enter:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnChange
            // 
            this.btnChange.BackgroundImage = global::Semnox.Core.Utilities.Properties.Resources.pressed1;
            this.btnChange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnChange.FlatAppearance.BorderSize = 0;
            this.btnChange.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnChange.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChange.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChange.ForeColor = System.Drawing.Color.White;
            this.btnChange.Location = new System.Drawing.Point(115, 159);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(75, 32);
            this.btnChange.TabIndex = 8;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::Semnox.Core.Utilities.Properties.Resources.pressed1;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(195, 159);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 32);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.BackgroundImage = global::Semnox.Core.Utilities.Properties.Resources.keyboard;
            this.btnShowKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Location = new System.Drawing.Point(274, 157);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(73, 34);
            this.btnShowKeyPad.TabIndex = 22;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.btnShowKeyPad_Click);
            // 
            // frmChangePassword
            // 
            this.AcceptButton = this.btnChange;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(369, 203);
            this.Controls.Add(this.btnShowKeyPad);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.txtReenterPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLoginId);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChangePassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Password";
            this.Load += new System.EventHandler(this.frmChangePassword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLoginId;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtReenterPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShowKeyPad;
    }
}