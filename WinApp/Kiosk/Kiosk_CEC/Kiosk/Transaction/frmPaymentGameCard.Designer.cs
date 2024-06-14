namespace Parafait_Kiosk.Transaction
{
    partial class frmPaymentGameCard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPaymentGameCard));
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblPaymentText = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTapCardText = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lblBalanceCredits = new System.Windows.Forms.Label();
            this.butttonCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.lblPurchaseValue = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblAvailableCredits = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.lblBalanceCreditsText = new System.Windows.Forms.Label();
            this.lblPurchaseValueText = new System.Windows.Forms.Label();
            this.lblAvailableCreditsText = new System.Windows.Forms.Label();
            this.lblCardNumberText = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtMessage = new System.Windows.Forms.Button();
            this.panelSummary.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(83, 552);
            this.btnPrev.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(85, 555);
            this.btnCancel.Size = new System.Drawing.Size(75, 22);
            // 
            // panelSummary
            // 
            this.panelSummary.BackColor = System.Drawing.Color.Transparent;
            this.panelSummary.Controls.Add(this.lblPaymentText);
            this.panelSummary.Controls.Add(this.panel1);
            this.panelSummary.Location = new System.Drawing.Point(77, 434);
            this.panelSummary.Margin = new System.Windows.Forms.Padding(0);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(938, 1013);
            this.panelSummary.TabIndex = 1071;
            // 
            // lblPaymentText
            // 
            this.lblPaymentText.BackColor = System.Drawing.Color.Transparent;
            this.lblPaymentText.Font = new System.Drawing.Font("Bango Pro", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentText.ForeColor = System.Drawing.Color.White;
            this.lblPaymentText.Location = new System.Drawing.Point(-77, 12);
            this.lblPaymentText.Margin = new System.Windows.Forms.Padding(0);
            this.lblPaymentText.Name = "lblPaymentText";
            this.lblPaymentText.Size = new System.Drawing.Size(1080, 67);
            this.lblPaymentText.TabIndex = 1059;
            this.lblPaymentText.Text = "Payment - Gamecard";
            this.lblPaymentText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.lblTapCardText);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Location = new System.Drawing.Point(17, 100);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(890, 913);
            this.panel1.TabIndex = 0;
            // 
            // lblTapCardText
            // 
            this.lblTapCardText.BackColor = System.Drawing.Color.Transparent;
            this.lblTapCardText.Font = new System.Drawing.Font("Bango Pro", 33F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTapCardText.ForeColor = System.Drawing.Color.White;
            this.lblTapCardText.Location = new System.Drawing.Point(-94, 43);
            this.lblTapCardText.Margin = new System.Windows.Forms.Padding(0);
            this.lblTapCardText.Name = "lblTapCardText";
            this.lblTapCardText.Size = new System.Drawing.Size(1080, 58);
            this.lblTapCardText.TabIndex = 1060;
            this.lblTapCardText.Text = "Tap card to pay for the purchase";
            this.lblTapCardText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Controls.Add(this.butttonCancel);
            this.panel3.Controls.Add(this.btnApply);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.lblBalanceCreditsText);
            this.panel3.Controls.Add(this.lblPurchaseValueText);
            this.panel3.Controls.Add(this.lblAvailableCreditsText);
            this.panel3.Controls.Add(this.lblCardNumberText);
            this.panel3.Font = new System.Drawing.Font("Bango Pro", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel3.Location = new System.Drawing.Point(3, 139);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(726, 771);
            this.panel3.TabIndex = 1069;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Transparent;
            this.panel7.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Value_add_box;
            this.panel7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel7.Controls.Add(this.lblBalanceCredits);
            this.panel7.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel7.Location = new System.Drawing.Point(372, 310);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(303, 75);
            this.panel7.TabIndex = 20007;
            // 
            // lblBalanceCredits
            // 
            this.lblBalanceCredits.BackColor = System.Drawing.Color.Transparent;
            this.lblBalanceCredits.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalanceCredits.ForeColor = System.Drawing.Color.White;
            this.lblBalanceCredits.Location = new System.Drawing.Point(33, 19);
            this.lblBalanceCredits.Margin = new System.Windows.Forms.Padding(0);
            this.lblBalanceCredits.Name = "lblBalanceCredits";
            this.lblBalanceCredits.Size = new System.Drawing.Size(241, 34);
            this.lblBalanceCredits.TabIndex = 1078;
            this.lblBalanceCredits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // butttonCancel
            // 
            this.butttonCancel.BackColor = System.Drawing.Color.Transparent;
            this.butttonCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.butttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.butttonCancel.FlatAppearance.BorderSize = 0;
            this.butttonCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.butttonCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.butttonCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.butttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butttonCancel.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butttonCancel.ForeColor = System.Drawing.Color.White;
            this.butttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butttonCancel.Location = new System.Drawing.Point(372, 494);
            this.butttonCancel.Name = "butttonCancel";
            this.butttonCancel.Size = new System.Drawing.Size(333, 170);
            this.butttonCancel.TabIndex = 1072;
            this.butttonCancel.Text = "Cancel";
            this.butttonCancel.UseVisualStyleBackColor = false;
            this.butttonCancel.Click += new System.EventHandler(this.butttonCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.Transparent;
            this.btnApply.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnApply.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnApply.FlatAppearance.BorderSize = 0;
            this.btnApply.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnApply.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnApply.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApply.Location = new System.Drawing.Point(24, 494);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(333, 170);
            this.btnApply.TabIndex = 1073;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Transparent;
            this.panel6.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Value_add_box;
            this.panel6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel6.Controls.Add(this.label8);
            this.panel6.Controls.Add(this.lblPurchaseValue);
            this.panel6.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel6.Location = new System.Drawing.Point(372, 229);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(303, 75);
            this.panel6.TabIndex = 20007;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(31, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(358, 58);
            this.label8.TabIndex = 1046;
            // 
            // lblPurchaseValue
            // 
            this.lblPurchaseValue.BackColor = System.Drawing.Color.Transparent;
            this.lblPurchaseValue.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPurchaseValue.ForeColor = System.Drawing.Color.White;
            this.lblPurchaseValue.Location = new System.Drawing.Point(33, 15);
            this.lblPurchaseValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblPurchaseValue.Name = "lblPurchaseValue";
            this.lblPurchaseValue.Size = new System.Drawing.Size(241, 34);
            this.lblPurchaseValue.TabIndex = 1077;
            this.lblPurchaseValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Value_add_box;
            this.panel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel5.Controls.Add(this.lblAvailableCredits);
            this.panel5.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel5.Location = new System.Drawing.Point(372, 148);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(303, 75);
            this.panel5.TabIndex = 20006;
            // 
            // lblAvailableCredits
            // 
            this.lblAvailableCredits.BackColor = System.Drawing.Color.Transparent;
            this.lblAvailableCredits.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailableCredits.ForeColor = System.Drawing.Color.White;
            this.lblAvailableCredits.Location = new System.Drawing.Point(33, 20);
            this.lblAvailableCredits.Margin = new System.Windows.Forms.Padding(0);
            this.lblAvailableCredits.Name = "lblAvailableCredits";
            this.lblAvailableCredits.Size = new System.Drawing.Size(241, 34);
            this.lblAvailableCredits.TabIndex = 1076;
            this.lblAvailableCredits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Value_add_box;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Controls.Add(this.lblCardNumber);
            this.panel4.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel4.Location = new System.Drawing.Point(372, 67);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(303, 75);
            this.panel4.TabIndex = 20005;
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblCardNumber.Font = new System.Drawing.Font("Bango Pro", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.ForeColor = System.Drawing.Color.White;
            this.lblCardNumber.Location = new System.Drawing.Point(34, 19);
            this.lblCardNumber.Margin = new System.Windows.Forms.Padding(0);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(241, 34);
            this.lblCardNumber.TabIndex = 1075;
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBalanceCreditsText
            // 
            this.lblBalanceCreditsText.BackColor = System.Drawing.Color.Transparent;
            this.lblBalanceCreditsText.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalanceCreditsText.ForeColor = System.Drawing.Color.White;
            this.lblBalanceCreditsText.Location = new System.Drawing.Point(28, 322);
            this.lblBalanceCreditsText.Margin = new System.Windows.Forms.Padding(0);
            this.lblBalanceCreditsText.Name = "lblBalanceCreditsText";
            this.lblBalanceCreditsText.Size = new System.Drawing.Size(330, 48);
            this.lblBalanceCreditsText.TabIndex = 1074;
            this.lblBalanceCreditsText.Text = "Balance Credits  :";
            this.lblBalanceCreditsText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPurchaseValueText
            // 
            this.lblPurchaseValueText.BackColor = System.Drawing.Color.Transparent;
            this.lblPurchaseValueText.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPurchaseValueText.ForeColor = System.Drawing.Color.White;
            this.lblPurchaseValueText.Location = new System.Drawing.Point(39, 229);
            this.lblPurchaseValueText.Margin = new System.Windows.Forms.Padding(0);
            this.lblPurchaseValueText.Name = "lblPurchaseValueText";
            this.lblPurchaseValueText.Size = new System.Drawing.Size(320, 65);
            this.lblPurchaseValueText.TabIndex = 1073;
            this.lblPurchaseValueText.Text = "Purchase Value  :";
            this.lblPurchaseValueText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAvailableCreditsText
            // 
            this.lblAvailableCreditsText.BackColor = System.Drawing.Color.Transparent;
            this.lblAvailableCreditsText.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailableCreditsText.ForeColor = System.Drawing.Color.White;
            this.lblAvailableCreditsText.Location = new System.Drawing.Point(13, 154);
            this.lblAvailableCreditsText.Margin = new System.Windows.Forms.Padding(0);
            this.lblAvailableCreditsText.Name = "lblAvailableCreditsText";
            this.lblAvailableCreditsText.Size = new System.Drawing.Size(346, 62);
            this.lblAvailableCreditsText.TabIndex = 1072;
            this.lblAvailableCreditsText.Text = "Available Credits  :";
            this.lblAvailableCreditsText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCardNumberText
            // 
            this.lblCardNumberText.BackColor = System.Drawing.Color.Transparent;
            this.lblCardNumberText.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumberText.ForeColor = System.Drawing.Color.White;
            this.lblCardNumberText.Location = new System.Drawing.Point(62, 74);
            this.lblCardNumberText.Margin = new System.Windows.Forms.Padding(0);
            this.lblCardNumberText.Name = "lblCardNumberText";
            this.lblCardNumberText.Size = new System.Drawing.Size(297, 54);
            this.lblCardNumberText.TabIndex = 1071;
            this.lblCardNumberText.Text = "Card Number  :";
            this.lblCardNumberText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(786, 732);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(235, 329);
            this.panel2.TabIndex = 1064;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Bango Pro", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1882);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 38);
            this.txtMessage.TabIndex = 1072;
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // frmPaymentGameCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelSummary);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPaymentGameCard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPaymentGameCard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPaymentGameCard_FormClosed);
            this.Load += new System.EventHandler(this.frmPaymentGameCard_Load);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.panelSummary, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.panelSummary.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblPaymentText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblBalanceCredits;
        private System.Windows.Forms.Label lblPurchaseValue;
        private System.Windows.Forms.Label lblAvailableCredits;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.Label lblBalanceCreditsText;
        private System.Windows.Forms.Label lblPurchaseValueText;
        private System.Windows.Forms.Label lblAvailableCreditsText;
        private System.Windows.Forms.Label lblCardNumberText;
        private System.Windows.Forms.Label lblTapCardText;
        private System.Windows.Forms.Button butttonCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button txtMessage;
    }
}