namespace Redemption_Kiosk
{
    partial class frmRemoteDebugInput
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
            this.txtTicketReceiptNo = new System.Windows.Forms.TextBox();
            this.lblTicketReceiptNo = new System.Windows.Forms.Label();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnKeyPad = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtTicketReceiptNo
            // 
            this.txtTicketReceiptNo.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTicketReceiptNo.Location = new System.Drawing.Point(263, 96);
            this.txtTicketReceiptNo.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.txtTicketReceiptNo.Name = "txtTicketReceiptNo";
            this.txtTicketReceiptNo.Size = new System.Drawing.Size(359, 29);
            this.txtTicketReceiptNo.TabIndex = 4;
            this.txtTicketReceiptNo.Enter += new System.EventHandler(this.TextBoxEnter);
            this.txtTicketReceiptNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxKeyPress);
            this.txtTicketReceiptNo.Leave += new System.EventHandler(this.TextBoxLeave);
            this.txtTicketReceiptNo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBoxMouseDown);
            this.txtTicketReceiptNo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TextBoxMouseDown);
            // 
            // lblTicketReceiptNo
            // 
            this.lblTicketReceiptNo.BackColor = System.Drawing.Color.Transparent;
            this.lblTicketReceiptNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketReceiptNo.ForeColor = System.Drawing.Color.Black;
            this.lblTicketReceiptNo.Location = new System.Drawing.Point(12, 81);
            this.lblTicketReceiptNo.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblTicketReceiptNo.Name = "lblTicketReceiptNo";
            this.lblTicketReceiptNo.Size = new System.Drawing.Size(246, 58);
            this.lblTicketReceiptNo.TabIndex = 3;
            this.lblTicketReceiptNo.Text = "Ticket Receipt Number:";
            this.lblTicketReceiptNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.Location = new System.Drawing.Point(263, 27);
            this.txtCardNumber.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.txtCardNumber.MaxLength = 20;
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(359, 29);
            this.txtCardNumber.TabIndex = 2;
            this.txtCardNumber.Enter += new System.EventHandler(this.TextBoxEnter);
            this.txtCardNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxKeyPress);
            this.txtCardNumber.Leave += new System.EventHandler(this.TextBoxLeave);
            this.txtCardNumber.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBoxMouseDown);
            this.txtCardNumber.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TextBoxMouseDown);
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblCardNumber.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.ForeColor = System.Drawing.Color.Black;
            this.lblCardNumber.Location = new System.Drawing.Point(12, 11);
            this.lblCardNumber.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(246, 58);
            this.lblCardNumber.TabIndex = 1;
            this.lblCardNumber.Text = "Card Number:";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOkay
            // 
            this.btnOkay.BackColor = System.Drawing.Color.Transparent;
            this.btnOkay.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Search;
            this.btnOkay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnOkay.FlatAppearance.BorderSize = 0;
            this.btnOkay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOkay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkay.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOkay.ForeColor = System.Drawing.Color.GhostWhite;
            this.btnOkay.Location = new System.Drawing.Point(225, 178);
            this.btnOkay.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(191, 69);
            this.btnOkay.TabIndex = 5;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = false;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnKeyPad
            // 
            this.btnKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.keyboard;
            this.btnKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnKeyPad.FlatAppearance.BorderSize = 0;
            this.btnKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyPad.Location = new System.Drawing.Point(671, 175);
            this.btnKeyPad.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.btnKeyPad.Name = "btnKeyPad";
            this.btnKeyPad.Size = new System.Drawing.Size(150, 79);
            this.btnKeyPad.TabIndex = 7;
            this.btnKeyPad.UseVisualStyleBackColor = false;
            this.btnKeyPad.Click += new System.EventHandler(this.btnKeyPad_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Search;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.GhostWhite;
            this.btnClose.Location = new System.Drawing.Point(460, 178);
            this.btnClose.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(191, 69);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.MistyRose;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 292);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(821, 22);
            this.txtMessage.TabIndex = 8;
            // 
            // frmRemoteDebugInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(822, 315);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtTicketReceiptNo);
            this.Controls.Add(this.lblTicketReceiptNo);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.lblCardNumber);
            this.Controls.Add(this.btnKeyPad);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.txtMessage);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(15, 13, 15, 13);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(838, 354);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(838, 354);
            this.Name = "frmRemoteDebugInput";
            this.Text = "Remote Input";
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRemoteDebugInput_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTicketReceiptNo;
        private System.Windows.Forms.Label lblTicketReceiptNo;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnKeyPad;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtMessage;
    }
}