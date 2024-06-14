namespace Parafait_POS
{
    partial class frmTransferLegacyTickets
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
            this.lblTicketReceiptNo = new System.Windows.Forms.Label();
            this.txtTicketReceiptNo = new System.Windows.Forms.TextBox();
            this.lblTickets = new System.Windows.Forms.Label();
            this.txtTickets = new System.Windows.Forms.TextBox();
            this.lblIssueDate = new System.Windows.Forms.Label();
            this.dtpPicker = new System.Windows.Forms.DateTimePicker();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnPrintTicket = new System.Windows.Forms.Button();
            this.btnLoadTicket = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblCardNo = new System.Windows.Forms.Label();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.lblCardTickets = new System.Windows.Forms.Label();
            this.txtCardTickets = new System.Windows.Forms.TextBox();
            this.btnGetReceipt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTicketReceiptNo
            // 
            this.lblTicketReceiptNo.AutoSize = true;
            this.lblTicketReceiptNo.Location = new System.Drawing.Point(19, 29);
            this.lblTicketReceiptNo.Name = "lblTicketReceiptNo";
            this.lblTicketReceiptNo.Size = new System.Drawing.Size(106, 15);
            this.lblTicketReceiptNo.TabIndex = 0;
            this.lblTicketReceiptNo.Text = "Ticket Receipt No";
            this.lblTicketReceiptNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTicketReceiptNo
            // 
            this.txtTicketReceiptNo.Location = new System.Drawing.Point(132, 26);
            this.txtTicketReceiptNo.Name = "txtTicketReceiptNo";
            this.txtTicketReceiptNo.Size = new System.Drawing.Size(179, 21);
            this.txtTicketReceiptNo.TabIndex = 1;
            this.txtTicketReceiptNo.Validating += new System.ComponentModel.CancelEventHandler(this.txtTicketReceiptNo_Validating);
            // 
            // lblTickets
            // 
            this.lblTickets.AutoSize = true;
            this.lblTickets.Location = new System.Drawing.Point(22, 60);
            this.lblTickets.Name = "lblTickets";
            this.lblTickets.Size = new System.Drawing.Size(103, 15);
            this.lblTickets.TabIndex = 2;
            this.lblTickets.Text = "Available Tickets";
            this.lblTickets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTickets
            // 
            this.txtTickets.Location = new System.Drawing.Point(132, 56);
            this.txtTickets.Name = "txtTickets";
            this.txtTickets.ReadOnly = true;
            this.txtTickets.Size = new System.Drawing.Size(179, 21);
            this.txtTickets.TabIndex = 3;
            // 
            // lblIssueDate
            // 
            this.lblIssueDate.AutoSize = true;
            this.lblIssueDate.Location = new System.Drawing.Point(58, 91);
            this.lblIssueDate.Name = "lblIssueDate";
            this.lblIssueDate.Size = new System.Drawing.Size(67, 15);
            this.lblIssueDate.TabIndex = 4;
            this.lblIssueDate.Text = "Issue Date";
            this.lblIssueDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpPicker
            // 
            this.dtpPicker.CustomFormat = "dddd,dd-MMM-yyyy";
            this.dtpPicker.Enabled = false;
            this.dtpPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPicker.Location = new System.Drawing.Point(132, 87);
            this.dtpPicker.Name = "dtpPicker";
            this.dtpPicker.Size = new System.Drawing.Size(179, 21);
            this.dtpPicker.TabIndex = 6;
            this.dtpPicker.Value = new System.DateTime(2018, 4, 5, 18, 15, 39, 0);
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMessage.AutoEllipsis = true;
            this.lblMessage.Location = new System.Drawing.Point(3, 240);
            this.lblMessage.MaximumSize = new System.Drawing.Size(495, 30);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(495, 30);
            this.lblMessage.TabIndex = 7;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // btnPrintTicket
            // 
            this.btnPrintTicket.BackgroundImage = global::Parafait_POS.Properties.Resources.PrintTrx;
            this.btnPrintTicket.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrintTicket.FlatAppearance.BorderSize = 0;
            this.btnPrintTicket.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintTicket.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintTicket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintTicket.ForeColor = System.Drawing.Color.White;
            this.btnPrintTicket.Location = new System.Drawing.Point(22, 195);
            this.btnPrintTicket.Name = "btnPrintTicket";
            this.btnPrintTicket.Size = new System.Drawing.Size(89, 40);
            this.btnPrintTicket.TabIndex = 8;
            this.btnPrintTicket.Text = "Print Receipt";
            this.btnPrintTicket.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrintTicket.UseVisualStyleBackColor = true;
            this.btnPrintTicket.Click += new System.EventHandler(this.btnPrintTicket_Click);
            this.btnPrintTicket.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPrintTicket_MouseDown);
            this.btnPrintTicket.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPrintTicket_MouseUp);
            // 
            // btnLoadTicket
            // 
            this.btnLoadTicket.BackgroundImage = global::Parafait_POS.Properties.Resources.LoadTicket_Normal;
            this.btnLoadTicket.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLoadTicket.FlatAppearance.BorderSize = 0;
            this.btnLoadTicket.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLoadTicket.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLoadTicket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadTicket.ForeColor = System.Drawing.Color.White;
            this.btnLoadTicket.Location = new System.Drawing.Point(140, 195);
            this.btnLoadTicket.Name = "btnLoadTicket";
            this.btnLoadTicket.Size = new System.Drawing.Size(89, 40);
            this.btnLoadTicket.TabIndex = 9;
            this.btnLoadTicket.Text = "Load To Card";
            this.btnLoadTicket.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoadTicket.UseVisualStyleBackColor = true;
            this.btnLoadTicket.Click += new System.EventHandler(this.btnLoadTicket_Click);
            this.btnLoadTicket.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLoadTicket_MouseDown);
            this.btnLoadTicket.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLoadTicket_MouseUp);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackgroundImage = global::Parafait_POS.Properties.Resources.ClearTrx;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(258, 195);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(89, 40);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "Clear";
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.btnRefresh.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRefresh_MouseDown);
            this.btnRefresh.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRefresh_MouseUp);
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.CancelLine;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(376, 195);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(89, 40);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnClose_MouseDown);
            this.btnClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnClose_MouseUp);
            // 
            // lblCardNo
            // 
            this.lblCardNo.AutoSize = true;
            this.lblCardNo.Location = new System.Drawing.Point(43, 122);
            this.lblCardNo.Name = "lblCardNo";
            this.lblCardNo.Size = new System.Drawing.Size(82, 15);
            this.lblCardNo.TabIndex = 12;
            this.lblCardNo.Text = "Card Number";
            this.lblCardNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Location = new System.Drawing.Point(132, 118);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.ReadOnly = true;
            this.txtCardNumber.Size = new System.Drawing.Size(179, 21);
            this.txtCardNumber.TabIndex = 13;
            // 
            // lblCardTickets
            // 
            this.lblCardTickets.AutoSize = true;
            this.lblCardTickets.Location = new System.Drawing.Point(46, 151);
            this.lblCardTickets.Name = "lblCardTickets";
            this.lblCardTickets.Size = new System.Drawing.Size(79, 15);
            this.lblCardTickets.TabIndex = 14;
            this.lblCardTickets.Text = "Card Tickets";
            this.lblCardTickets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCardTickets
            // 
            this.txtCardTickets.Location = new System.Drawing.Point(132, 151);
            this.txtCardTickets.Name = "txtCardTickets";
            this.txtCardTickets.ReadOnly = true;
            this.txtCardTickets.Size = new System.Drawing.Size(179, 21);
            this.txtCardTickets.TabIndex = 15;
            // 
            // btnGetReceipt
            // 
            this.btnGetReceipt.BackgroundImage = global::Parafait_POS.Properties.Resources.Search_Btn_Normal;
            this.btnGetReceipt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGetReceipt.FlatAppearance.BorderSize = 0;
            this.btnGetReceipt.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGetReceipt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGetReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetReceipt.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetReceipt.ForeColor = System.Drawing.Color.White;
            this.btnGetReceipt.Location = new System.Drawing.Point(317, 21);
            this.btnGetReceipt.Name = "btnGetReceipt";
            this.btnGetReceipt.Size = new System.Drawing.Size(61, 30);
            this.btnGetReceipt.TabIndex = 16;
            this.btnGetReceipt.Text = "Receipt";
            this.btnGetReceipt.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGetReceipt.UseVisualStyleBackColor = true;
            this.btnGetReceipt.Click += new System.EventHandler(this.btnGetReceipt_Click);
            this.btnGetReceipt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnGetReceipt_MouseDown);
            this.btnGetReceipt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnGetReceipt_MouseUp);
            // 
            // frmTransferLegacyTickets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 271);
            this.Controls.Add(this.btnGetReceipt);
            this.Controls.Add(this.txtCardTickets);
            this.Controls.Add(this.lblCardTickets);
            this.Controls.Add(this.txtCardNumber);
            this.Controls.Add(this.lblCardNo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnLoadTicket);
            this.Controls.Add(this.btnPrintTicket);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.dtpPicker);
            this.Controls.Add(this.lblIssueDate);
            this.Controls.Add(this.txtTickets);
            this.Controls.Add(this.lblTickets);
            this.Controls.Add(this.txtTicketReceiptNo);
            this.Controls.Add(this.lblTicketReceiptNo);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTransferLegacyTickets";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transfer Legacy Tickets";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTransferLegacyTickets_FormClosing);
            this.Load += new System.EventHandler(this.frmTransferLegacyTickets_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTicketReceiptNo;
        private System.Windows.Forms.TextBox txtTicketReceiptNo;
        private System.Windows.Forms.Label lblTickets;
        private System.Windows.Forms.TextBox txtTickets;
        private System.Windows.Forms.Label lblIssueDate;
        private System.Windows.Forms.DateTimePicker dtpPicker;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnPrintTicket;
        private System.Windows.Forms.Button btnLoadTicket;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblCardNo;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label lblCardTickets;
        private System.Windows.Forms.TextBox txtCardTickets;
        private System.Windows.Forms.Button btnGetReceipt;
    }
}

