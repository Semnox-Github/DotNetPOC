namespace Semnox.Parafait.Device.PaymentGateway
{
    partial class frmFinalizeTransaction
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
            this.pnlFinalizingTransaction = new System.Windows.Forms.Panel();
            this.btnComplete = new System.Windows.Forms.Button();
            this.btnEditTip = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblTipAmount = new System.Windows.Forms.Label();
            this.lblTrxnAmount = new System.Windows.Forms.Label();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlFinalizingTransaction.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFinalizingTransaction
            // 
            this.pnlFinalizingTransaction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnlFinalizingTransaction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFinalizingTransaction.Controls.Add(this.btnComplete);
            this.pnlFinalizingTransaction.Controls.Add(this.btnEditTip);
            this.pnlFinalizingTransaction.Controls.Add(this.btnClose);
            this.pnlFinalizingTransaction.Controls.Add(this.lblTotalAmount);
            this.pnlFinalizingTransaction.Controls.Add(this.lblTipAmount);
            this.pnlFinalizingTransaction.Controls.Add(this.lblTrxnAmount);
            this.pnlFinalizingTransaction.Controls.Add(this.lblCardNumber);
            this.pnlFinalizingTransaction.Controls.Add(this.label5);
            this.pnlFinalizingTransaction.Controls.Add(this.label4);
            this.pnlFinalizingTransaction.Controls.Add(this.label3);
            this.pnlFinalizingTransaction.Controls.Add(this.label2);
            this.pnlFinalizingTransaction.Controls.Add(this.label1);
            this.pnlFinalizingTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFinalizingTransaction.Location = new System.Drawing.Point(0, 0);
            this.pnlFinalizingTransaction.Name = "pnlFinalizingTransaction";
            this.pnlFinalizingTransaction.Size = new System.Drawing.Size(400, 278);
            this.pnlFinalizingTransaction.TabIndex = 1;
            // 
            // btnComplete
            // 
            this.btnComplete.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnComplete.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnComplete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComplete.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnComplete.Location = new System.Drawing.Point(290, 225);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(96, 42);
            this.btnComplete.TabIndex = 12;
            this.btnComplete.Text = "Complete";
            this.btnComplete.UseVisualStyleBackColor = false;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // btnEditTip
            // 
            this.btnEditTip.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnEditTip.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnEditTip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditTip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditTip.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnEditTip.Location = new System.Drawing.Point(149, 225);
            this.btnEditTip.Name = "btnEditTip";
            this.btnEditTip.Size = new System.Drawing.Size(96, 42);
            this.btnEditTip.TabIndex = 11;
            this.btnEditTip.Text = "Edit Tip";
            this.btnEditTip.UseVisualStyleBackColor = false;
            this.btnEditTip.Click += new System.EventHandler(this.btnEditTip_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnClose.Location = new System.Drawing.Point(8, 225);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 42);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.BackColor = System.Drawing.Color.White;
            this.lblTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotalAmount.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(183, 180);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(203, 23);
            this.lblTotalAmount.TabIndex = 9;
            this.lblTotalAmount.Text = "$0.00";
            this.lblTotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTipAmount
            // 
            this.lblTipAmount.BackColor = System.Drawing.Color.White;
            this.lblTipAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTipAmount.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipAmount.Location = new System.Drawing.Point(183, 138);
            this.lblTipAmount.Name = "lblTipAmount";
            this.lblTipAmount.Size = new System.Drawing.Size(203, 23);
            this.lblTipAmount.TabIndex = 8;
            this.lblTipAmount.Text = "$0.00";
            this.lblTipAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTrxnAmount
            // 
            this.lblTrxnAmount.BackColor = System.Drawing.Color.White;
            this.lblTrxnAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTrxnAmount.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrxnAmount.Location = new System.Drawing.Point(183, 96);
            this.lblTrxnAmount.Name = "lblTrxnAmount";
            this.lblTrxnAmount.Size = new System.Drawing.Size(203, 23);
            this.lblTrxnAmount.TabIndex = 7;
            this.lblTrxnAmount.Text = "$0.00";
            this.lblTrxnAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.BackColor = System.Drawing.Color.White;
            this.lblCardNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCardNumber.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.Location = new System.Drawing.Point(183, 54);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(203, 23);
            this.lblCardNumber.TabIndex = 6;
            this.lblCardNumber.Text = "XXXXXXXXXXXXXXXX";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(11, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 23);
            this.label5.TabIndex = 5;
            this.label5.Text = "Transaction Total:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(170, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Entered Tip:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Transaction Amount:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Card Number:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.SteelBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(396, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "Finalizing Transaction";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmFinalizeTransaction
            // 
            this.AcceptButton = this.btnComplete;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(400, 278);
            this.ControlBox = false;
            this.Controls.Add(this.pnlFinalizingTransaction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFinalizeTransaction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Finalize Credit Card Transaction";
            this.pnlFinalizingTransaction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlFinalizingTransaction;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Button btnEditTip;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label lblTipAmount;
        private System.Windows.Forms.Label lblTrxnAmount;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}