namespace Redemption_Kiosk
{
    partial class SetUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetUp));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDBMachine = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDBInstance = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.flpLabels1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flpValues1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flpLabels2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flpValues2 = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(254, 696);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 31);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            //this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(721, 696);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 31);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(255, 90);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(209, 23);
            this.txtPassword.TabIndex = 21;
            // 
            // txtUserId
            // 
            this.txtUserId.Location = new System.Drawing.Point(255, 65);
            this.txtUserId.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.ReadOnly = true;
            this.txtUserId.Size = new System.Drawing.Size(209, 23);
            this.txtUserId.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(119, 90);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 23);
            this.label7.TabIndex = 23;
            this.label7.Text = "Password:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(119, 65);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 23);
            this.label6.TabIndex = 22;
            this.label6.Text = "DB UserID:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDBMachine
            // 
            this.txtDBMachine.Location = new System.Drawing.Point(255, 16);
            this.txtDBMachine.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDBMachine.Name = "txtDBMachine";
            this.txtDBMachine.Size = new System.Drawing.Size(209, 23);
            this.txtDBMachine.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(119, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "DB Machine Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDBInstance
            // 
            this.txtDBInstance.Location = new System.Drawing.Point(255, 41);
            this.txtDBInstance.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDBInstance.Name = "txtDBInstance";
            this.txtDBInstance.Size = new System.Drawing.Size(209, 23);
            this.txtDBInstance.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(119, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 23);
            this.label2.TabIndex = 26;
            this.label2.Text = "Instance Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTest.Location = new System.Drawing.Point(488, 696);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(100, 31);
            this.btnTest.TabIndex = 28;
            this.btnTest.Text = "Test DB";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // flpLabels1
            // 
            this.flpLabels1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flpLabels1.BackColor = System.Drawing.Color.DarkKhaki;
            this.flpLabels1.Location = new System.Drawing.Point(12, 117);
            this.flpLabels1.Margin = new System.Windows.Forms.Padding(4);
            this.flpLabels1.Name = "flpLabels1";
            this.flpLabels1.Size = new System.Drawing.Size(243, 572);
            this.flpLabels1.TabIndex = 29;
            // 
            // flpValues1
            // 
            this.flpValues1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flpValues1.BackColor = System.Drawing.Color.DarkKhaki;
            this.flpValues1.Location = new System.Drawing.Point(252, 117);
            this.flpValues1.Margin = new System.Windows.Forms.Padding(4);
            this.flpValues1.Name = "flpValues1";
            this.flpValues1.Size = new System.Drawing.Size(339, 572);
            this.flpValues1.TabIndex = 30;
            // 
            // flpLabels2
            // 
            this.flpLabels2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flpLabels2.BackColor = System.Drawing.Color.DarkKhaki;
            this.flpLabels2.Location = new System.Drawing.Point(590, 117);
            this.flpLabels2.Margin = new System.Windows.Forms.Padding(4);
            this.flpLabels2.Name = "flpLabels2";
            this.flpLabels2.Size = new System.Drawing.Size(245, 572);
            this.flpLabels2.TabIndex = 31;
            // 
            // flpValues2
            // 
            this.flpValues2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flpValues2.BackColor = System.Drawing.Color.DarkKhaki;
            this.flpValues2.Location = new System.Drawing.Point(834, 117);
            this.flpValues2.Margin = new System.Windows.Forms.Padding(4);
            this.flpValues2.Name = "flpValues2";
            this.flpValues2.Size = new System.Drawing.Size(229, 572);
            this.flpValues2.TabIndex = 32;
            // 
            // SetUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Beige;
            //this.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Pop_up_Bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1076, 733);
            this.Controls.Add(this.flpValues2);
            this.Controls.Add(this.flpLabels2);
            this.Controls.Add(this.flpValues1);
            this.Controls.Add(this.flpLabels1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtDBInstance);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserId);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtDBMachine);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SetUp";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kiosk Setup";
            this.TransparencyKey = System.Drawing.Color.Beige;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetUp_FormClosing);
            this.Load += new System.EventHandler(this.SetUp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDBMachine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDBInstance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.FlowLayoutPanel flpLabels1;
        private System.Windows.Forms.FlowLayoutPanel flpValues1;
        private System.Windows.Forms.FlowLayoutPanel flpLabels2;
        private System.Windows.Forms.FlowLayoutPanel flpValues2;
    }
}