namespace Parafait_FnB_Kiosk
{
    partial class frmAdmin
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
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrintSummary = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.flpOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRebootComputer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.flpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.White_Btn;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Bango Pro", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnExit.Location = new System.Drawing.Point(30, 118);
            this.btnExit.Margin = new System.Windows.Forms.Padding(30, 10, 2, 10);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(346, 68);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Exit Kiosk";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPrintSummary
            // 
            this.btnPrintSummary.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintSummary.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.White_Btn;
            this.btnPrintSummary.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrintSummary.FlatAppearance.BorderSize = 0;
            this.btnPrintSummary.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintSummary.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintSummary.Font = new System.Drawing.Font("Bango Pro", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnPrintSummary.Location = new System.Drawing.Point(30, 294);
            this.btnPrintSummary.Margin = new System.Windows.Forms.Padding(30, 10, 2, 10);
            this.btnPrintSummary.Name = "btnPrintSummary";
            this.btnPrintSummary.Size = new System.Drawing.Size(346, 68);
            this.btnPrintSummary.TabIndex = 2;
            this.btnPrintSummary.Text = "Print Kiosk Summary";
            this.btnPrintSummary.UseVisualStyleBackColor = false;
            this.btnPrintSummary.Click += new System.EventHandler(this.btnPrintSummary_Click);
            // 
            // btnSetup
            // 
            this.btnSetup.BackColor = System.Drawing.Color.Transparent;
            this.btnSetup.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.White_Btn;
            this.btnSetup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSetup.FlatAppearance.BorderSize = 0;
            this.btnSetup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSetup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetup.Font = new System.Drawing.Font("Bango Pro", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnSetup.Location = new System.Drawing.Point(30, 206);
            this.btnSetup.Margin = new System.Windows.Forms.Padding(30, 10, 2, 10);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(346, 68);
            this.btnSetup.TabIndex = 3;
            this.btnSetup.Text = "Set Up";
            this.btnSetup.UseVisualStyleBackColor = false;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // flpOptions
            // 
            this.flpOptions.BackColor = System.Drawing.Color.Transparent;
            this.flpOptions.Controls.Add(this.btnCancel);
            this.flpOptions.Controls.Add(this.btnExit);
            this.flpOptions.Controls.Add(this.btnSetup);
            this.flpOptions.Controls.Add(this.btnPrintSummary);
            this.flpOptions.Controls.Add(this.btnRebootComputer);
            this.flpOptions.Location = new System.Drawing.Point(566, 193);
            this.flpOptions.Name = "flpOptions";
            this.flpOptions.Size = new System.Drawing.Size(408, 478);
            this.flpOptions.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.White_Btn;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Bango Pro", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnCancel.Location = new System.Drawing.Point(30, 30);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(30, 30, 2, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(346, 68);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRebootComputer
            // 
            this.btnRebootComputer.BackColor = System.Drawing.Color.Transparent;
            this.btnRebootComputer.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.White_Btn;
            this.btnRebootComputer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRebootComputer.FlatAppearance.BorderSize = 0;
            this.btnRebootComputer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRebootComputer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRebootComputer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRebootComputer.Font = new System.Drawing.Font("Bango Pro", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRebootComputer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnRebootComputer.Location = new System.Drawing.Point(30, 382);
            this.btnRebootComputer.Margin = new System.Windows.Forms.Padding(30, 10, 2, 10);
            this.btnRebootComputer.Name = "btnRebootComputer";
            this.btnRebootComputer.Size = new System.Drawing.Size(346, 68);
            this.btnRebootComputer.TabIndex = 8;
            this.btnRebootComputer.Text = "Reboot Computer";
            this.btnRebootComputer.UseVisualStyleBackColor = false;
            this.btnRebootComputer.Click += new System.EventHandler(this.btnRebootComputer_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("VAG Rounded", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(558, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(394, 114);
            this.label1.TabIndex = 9;
            this.label1.Text = "Enter password or Tap staff Card";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.ProductSalePopUp1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(990, 800);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flpOptions);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin";
            this.TransparencyKey = System.Drawing.Color.LightSlateGray;
            this.Load += new System.EventHandler(this.frmAdmin_Load);
            this.flpOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPrintSummary;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FlowLayoutPanel flpOptions;
        private System.Windows.Forms.Button btnRebootComputer;
        private System.Windows.Forms.Label label1;
    }
}