namespace Semnox.Parafait.Device.PaymentGateway
{
    partial class frmPOSPaymentStatusUI
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCheckNow = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCardCharged = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.DimGray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(244, 80);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(151, 35);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCancel
            // 
            this.btnCheckNow.BackColor = System.Drawing.Color.DimGray;
            this.btnCheckNow.FlatAppearance.BorderSize = 0;
            this.btnCheckNow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCheckNow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCheckNow.ForeColor = System.Drawing.Color.White;
            this.btnCheckNow.Location = new System.Drawing.Point(244, 80);
            this.btnCheckNow.Margin = new System.Windows.Forms.Padding(4);
            this.btnCheckNow.Name = "btnCheckNow";
            this.btnCheckNow.Size = new System.Drawing.Size(151, 35);
            this.btnCheckNow.TabIndex = 26;
            this.btnCheckNow.Text = "CHECK NOW";
            this.btnCheckNow.UseVisualStyleBackColor = false;
            this.btnCheckNow.Visible = false;
            this.btnCheckNow.Click += new System.EventHandler(this.btnCheckNow_Click);
            //
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.White;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(7, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(652, 51);
            this.lblStatus.TabIndex = 27;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCardCharged
            // 
            this.lblCardCharged.BackColor = System.Drawing.SystemColors.Control;
            this.lblCardCharged.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Bold);
            this.lblCardCharged.ForeColor = System.Drawing.Color.Red;
            this.lblCardCharged.Location = new System.Drawing.Point(7, -1);
            this.lblCardCharged.Name = "lblCardCharged";
            this.lblCardCharged.Size = new System.Drawing.Size(652, 30);
            this.lblCardCharged.TabIndex = 28;
            this.lblCardCharged.Text = "Card will be charged: $10";
            this.lblCardCharged.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmPOSPaymentStatusUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 117);
            this.ControlBox = false;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblCardCharged);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCheckNow);
            this.Name = "frmPOSPaymentStatusUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Payment Gateway";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCheckNow;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCardCharged;
    }
}