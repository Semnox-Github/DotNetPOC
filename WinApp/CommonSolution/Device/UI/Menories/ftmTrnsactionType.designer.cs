namespace Semnox.Parafait.Device.PaymentGateway.Menories
{
    partial class ftmTransactionType
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
            this.gpTransactionType = new System.Windows.Forms.GroupBox();
            this.btnPurchase = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPreauth = new System.Windows.Forms.Button();
            this.gpTransactionType.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.DimGray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(206, 123);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 37);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gpTransactionType
            // 
            this.gpTransactionType.Controls.Add(this.btnPurchase);
            this.gpTransactionType.Controls.Add(this.label2);
            this.gpTransactionType.Controls.Add(this.btnPreauth);
            this.gpTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpTransactionType.Location = new System.Drawing.Point(3, 4);
            this.gpTransactionType.Name = "gpTransactionType";
            this.gpTransactionType.Size = new System.Drawing.Size(534, 112);
            this.gpTransactionType.TabIndex = 2;
            this.gpTransactionType.TabStop = false;
            this.gpTransactionType.Text = "Transaction Types";
            // 
            // btnPurchase
            // 
            this.btnPurchase.BackColor = System.Drawing.Color.DimGray;
            this.btnPurchase.ForeColor = System.Drawing.Color.White;
            this.btnPurchase.Location = new System.Drawing.Point(76, 37);
            this.btnPurchase.Margin = new System.Windows.Forms.Padding(4);
            this.btnPurchase.Name = "btnPurchase";
            this.btnPurchase.Size = new System.Drawing.Size(144, 40);
            this.btnPurchase.TabIndex = 4;
            this.btnPurchase.Text = "Purchase";
            this.btnPurchase.UseVisualStyleBackColor = false;
            this.btnPurchase.Click += new System.EventHandler(this.btnPurchase_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(520, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = "Pre-Authorization and Authorization transactions are not allowed for debit cards." +
    "";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPreauth
            // 
            this.btnPreauth.BackColor = System.Drawing.Color.DimGray;
            this.btnPreauth.ForeColor = System.Drawing.Color.White;
            this.btnPreauth.Location = new System.Drawing.Point(313, 37);
            this.btnPreauth.Margin = new System.Windows.Forms.Padding(4);
            this.btnPreauth.Name = "btnPreauth";
            this.btnPreauth.Size = new System.Drawing.Size(144, 39);
            this.btnPreauth.TabIndex = 0;
            this.btnPreauth.Text = "Pre-Authorization";
            this.btnPreauth.UseVisualStyleBackColor = false;
            this.btnPreauth.Click += new System.EventHandler(this.btnPreauth_Click);
            // 
            // ftmTransactionType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 172);
            this.ControlBox = false;
            this.Controls.Add(this.gpTransactionType);
            this.Controls.Add(this.btnCancel);
            this.Name = "ftmTransactionType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transaction Types";
            this.gpTransactionType.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gpTransactionType;
        private System.Windows.Forms.Button btnPreauth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPurchase;
    }
}