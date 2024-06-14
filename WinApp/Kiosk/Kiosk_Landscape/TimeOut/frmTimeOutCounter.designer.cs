namespace Parafait_Kiosk
{
    partial class frmTimeOutCounter
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTimeOut = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 608);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1026, 170);
            this.label2.TabIndex = 5;
            this.label2.Text = "Touch anywhere on screen to continue";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.frmTimeOutCounter_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1026, 81);
            this.label1.TabIndex = 4;
            this.label1.Text = "Do you need more time?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.frmTimeOutCounter_Click);
            // 
            // lblTimeOut
            // 
            this.lblTimeOut.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 360F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOut.ForeColor = System.Drawing.Color.White;
            this.lblTimeOut.Location = new System.Drawing.Point(12, 70);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(1026, 538);
            this.lblTimeOut.TabIndex = 3;
            this.lblTimeOut.Text = "8";
            this.lblTimeOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTimeOut.Click += new System.EventHandler(this.frmTimeOutCounter_Click);
            // 
            // frmTimeOutCounter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.countdown_timer;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1050, 780);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTimeOut);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmTimeOutCounter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTimeOutCounter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTimeOutCounter_FormClosed);
            this.Load += new System.EventHandler(this.frmTimeOutCounter_Load);
            this.Click += new System.EventHandler(this.frmTimeOutCounter_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTimeOut;
    }
}