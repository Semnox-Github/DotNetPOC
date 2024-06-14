namespace Semnox.Parafait.Device.PaymentGateway
{
    partial class frmTransactionTypeUI
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
            this.grpboxTipEntry = new System.Windows.Forms.GroupBox();
            this.btnShowNumPad = new System.Windows.Forms.Button();
            this.txtTip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gpTransactionType = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnsettlement = new System.Windows.Forms.Button();
            this.btnAuth = new System.Windows.Forms.Button();
            this.btnPreauth = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpboxTipEntry.SuspendLayout();
            this.gpTransactionType.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpboxTipEntry
            // 
            this.grpboxTipEntry.Controls.Add(this.btnShowNumPad);
            this.grpboxTipEntry.Controls.Add(this.txtTip);
            this.grpboxTipEntry.Controls.Add(this.label1);
            this.grpboxTipEntry.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpboxTipEntry.Location = new System.Drawing.Point(7, 120);
            this.grpboxTipEntry.Name = "grpboxTipEntry";
            this.grpboxTipEntry.Size = new System.Drawing.Size(587, 68);
            this.grpboxTipEntry.TabIndex = 6;
            this.grpboxTipEntry.TabStop = false;
            this.grpboxTipEntry.Text = "Tip Entry";
            // 
            // btnShowNumPad
            // 
            this.btnShowNumPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowNumPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.BackgroundImage = global::Semnox.Parafait.Device.Properties.Resources.keypadGrey;
            this.btnShowNumPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnShowNumPad.CausesValidation = false;
            this.btnShowNumPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowNumPad.FlatAppearance.BorderSize = 0;
            this.btnShowNumPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowNumPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowNumPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnShowNumPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowNumPad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnShowNumPad.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnShowNumPad.Location = new System.Drawing.Point(538, 20);
            this.btnShowNumPad.Name = "btnShowNumPad";
            this.btnShowNumPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowNumPad.TabIndex = 25;
            this.btnShowNumPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowNumPad.UseVisualStyleBackColor = false;
            this.btnShowNumPad.Click += new System.EventHandler(this.btnShowNumPad_Click);
            // 
            // txtTip
            // 
            this.txtTip.Location = new System.Drawing.Point(335, 28);
            this.txtTip.Name = "txtTip";
            this.txtTip.Size = new System.Drawing.Size(75, 21);
            this.txtTip.TabIndex = 1;
            this.txtTip.Text = "0.00";
            this.txtTip.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTip.Click += new System.EventHandler(this.txtTip_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(177, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tip Amount :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gpTransactionType
            // 
            this.gpTransactionType.Controls.Add(this.label2);
            this.gpTransactionType.Controls.Add(this.btnsettlement);
            this.gpTransactionType.Controls.Add(this.btnAuth);
            this.gpTransactionType.Controls.Add(this.btnPreauth);
            this.gpTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpTransactionType.Location = new System.Drawing.Point(7, 6);
            this.gpTransactionType.Name = "gpTransactionType";
            this.gpTransactionType.Size = new System.Drawing.Size(587, 112);
            this.gpTransactionType.TabIndex = 5;
            this.gpTransactionType.TabStop = false;
            this.gpTransactionType.Text = "Transaction Types";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(566, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = "Pre-Authorization and Authorization transactions are not allowed for debit cards." +
    "";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnsettlement
            // 
            this.btnsettlement.BackColor = System.Drawing.Color.DimGray;
            this.btnsettlement.ForeColor = System.Drawing.Color.White;
            this.btnsettlement.Location = new System.Drawing.Point(431, 36);
            this.btnsettlement.Margin = new System.Windows.Forms.Padding(4);
            this.btnsettlement.Name = "btnsettlement";
            this.btnsettlement.Size = new System.Drawing.Size(144, 40);
            this.btnsettlement.TabIndex = 2;
            this.btnsettlement.Text = "Settlement";
            this.btnsettlement.UseVisualStyleBackColor = false;
            this.btnsettlement.Click += new System.EventHandler(this.btnsettlement_Click);
            // 
            // btnAuth
            // 
            this.btnAuth.BackColor = System.Drawing.Color.DimGray;
            this.btnAuth.ForeColor = System.Drawing.Color.White;
            this.btnAuth.Location = new System.Drawing.Point(221, 36);
            this.btnAuth.Margin = new System.Windows.Forms.Padding(4);
            this.btnAuth.Name = "btnAuth";
            this.btnAuth.Size = new System.Drawing.Size(144, 40);
            this.btnAuth.TabIndex = 1;
            this.btnAuth.Text = "Authorization";
            this.btnAuth.UseVisualStyleBackColor = false;
            this.btnAuth.Click += new System.EventHandler(this.btnAuth_Click);
            // 
            // btnPreauth
            // 
            this.btnPreauth.BackColor = System.Drawing.Color.DimGray;
            this.btnPreauth.ForeColor = System.Drawing.Color.White;
            this.btnPreauth.Location = new System.Drawing.Point(11, 37);
            this.btnPreauth.Margin = new System.Windows.Forms.Padding(4);
            this.btnPreauth.Name = "btnPreauth";
            this.btnPreauth.Size = new System.Drawing.Size(144, 39);
            this.btnPreauth.TabIndex = 0;
            this.btnPreauth.Text = "Pre-Authorization";
            this.btnPreauth.UseVisualStyleBackColor = false;
            this.btnPreauth.Click += new System.EventHandler(this.btnPreauth_Click);
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
            this.btnCancel.Location = new System.Drawing.Point(249, 199);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 37);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmTransactionTypeUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 249);
            this.Controls.Add(this.grpboxTipEntry);
            this.Controls.Add(this.gpTransactionType);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTransactionTypeUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transaction Type";
            this.Load += new System.EventHandler(this.frmTransactionTypeUI_Load);
            this.grpboxTipEntry.ResumeLayout(false);
            this.grpboxTipEntry.PerformLayout();
            this.gpTransactionType.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpboxTipEntry;
        private System.Windows.Forms.TextBox txtTip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gpTransactionType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnsettlement;
        private System.Windows.Forms.Button btnAuth;
        private System.Windows.Forms.Button btnPreauth;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShowNumPad;
    }
}