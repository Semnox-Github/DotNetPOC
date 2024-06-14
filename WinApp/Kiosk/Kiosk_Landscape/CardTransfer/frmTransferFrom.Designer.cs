namespace Parafait_Kiosk.CardTransfer
{
    partial class frmTransferFrom
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
            this.lblSiteName = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTapMsg = new System.Windows.Forms.Label();
            this.txtCardNo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAvlblTokens = new System.Windows.Forms.Label();
            this.lblAvlCredits = new System.Windows.Forms.Label();
            this.buttonNext = new System.Windows.Forms.Button();
            this.txtTransfer = new System.Windows.Forms.Label();
            this.lblCreditsToTransfer = new System.Windows.Forms.Label();
            this.panelCardDetails = new System.Windows.Forms.Panel();
            this.lblTimeRemaining = new System.Windows.Forms.Button();
            this.lblHeading = new System.Windows.Forms.Label();
            this.textBoxMessageLine = new System.Windows.Forms.Button();
            this.panelCardDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.home_button;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.TabIndex = 20012;
            // 
            // lblSiteName
            // 
            this.lblSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSiteName.BackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.BorderSize = 0;
            this.lblSiteName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSiteName.Font = new System.Drawing.Font("Verdana", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(0, 0);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1268, 69);
            this.lblSiteName.TabIndex = 144;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.back_btn;
            this.btnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnPrev.Location = new System.Drawing.Point(12, 557);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(142, 110);
            this.btnPrev.TabIndex = 2;
            this.btnPrev.Text = "BACK";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Visible = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTapMsg
            // 
            this.lblTapMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTapMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblTapMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTapMsg.ForeColor = System.Drawing.Color.White;
            this.lblTapMsg.Location = new System.Drawing.Point(12, 182);
            this.lblTapMsg.Name = "lblTapMsg";
            this.lblTapMsg.Size = new System.Drawing.Size(1255, 113);
            this.lblTapMsg.TabIndex = 1045;
            this.lblTapMsg.Text = "Transfer From";
            this.lblTapMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCardNo
            // 
            this.txtCardNo.BackColor = System.Drawing.Color.Transparent;
            this.txtCardNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNo.ForeColor = System.Drawing.Color.White;
            this.txtCardNo.Location = new System.Drawing.Point(444, 50);
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.Size = new System.Drawing.Size(220, 49);
            this.txtCardNo.TabIndex = 1046;
            this.txtCardNo.Text = "9";
            this.txtCardNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(14, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(424, 49);
            this.label1.TabIndex = 1047;
            this.label1.Text = "Card #:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAvlblTokens
            // 
            this.txtAvlblTokens.BackColor = System.Drawing.Color.Transparent;
            this.txtAvlblTokens.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAvlblTokens.ForeColor = System.Drawing.Color.White;
            this.txtAvlblTokens.Location = new System.Drawing.Point(444, 120);
            this.txtAvlblTokens.Name = "txtAvlblTokens";
            this.txtAvlblTokens.Size = new System.Drawing.Size(220, 49);
            this.txtAvlblTokens.TabIndex = 1048;
            this.txtAvlblTokens.Text = "9";
            this.txtAvlblTokens.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAvlCredits
            // 
            this.lblAvlCredits.BackColor = System.Drawing.Color.Transparent;
            this.lblAvlCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvlCredits.ForeColor = System.Drawing.Color.White;
            this.lblAvlCredits.Location = new System.Drawing.Point(14, 120);
            this.lblAvlCredits.Name = "lblAvlCredits";
            this.lblAvlCredits.Size = new System.Drawing.Size(424, 49);
            this.lblAvlCredits.TabIndex = 1049;
            this.lblAvlCredits.Text = "Available Credits:";
            this.lblAvlCredits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonNext.BackColor = System.Drawing.Color.Transparent;
            this.buttonNext.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.buttonNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonNext.FlatAppearance.BorderSize = 0;
            this.buttonNext.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNext.ForeColor = System.Drawing.Color.White;
            this.buttonNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonNext.Location = new System.Drawing.Point(488, 819);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(315, 116);
            this.buttonNext.TabIndex = 1050;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = false;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // txtTransfer
            // 
            this.txtTransfer.BackColor = System.Drawing.Color.Transparent;
            this.txtTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransfer.ForeColor = System.Drawing.Color.White;
            this.txtTransfer.Location = new System.Drawing.Point(444, 190);
            this.txtTransfer.Name = "txtTransfer";
            this.txtTransfer.Size = new System.Drawing.Size(220, 49);
            this.txtTransfer.TabIndex = 1050;
            this.txtTransfer.Text = "9";
            this.txtTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtTransfer.TextChanged += new System.EventHandler(this.txtTransfer_TextChanged);
            this.txtTransfer.Click += new System.EventHandler(this.txtTransfer_Click);
            // 
            // lblCreditsToTransfer
            // 
            this.lblCreditsToTransfer.BackColor = System.Drawing.Color.Transparent;
            this.lblCreditsToTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreditsToTransfer.ForeColor = System.Drawing.Color.White;
            this.lblCreditsToTransfer.Location = new System.Drawing.Point(3, 190);
            this.lblCreditsToTransfer.Name = "lblCreditsToTransfer";
            this.lblCreditsToTransfer.Size = new System.Drawing.Size(435, 49);
            this.lblCreditsToTransfer.TabIndex = 1051;
            this.lblCreditsToTransfer.Text = "Credits To Transfer:";
            this.lblCreditsToTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelCardDetails
            // 
            this.panelCardDetails.BackColor = System.Drawing.Color.Transparent;
            this.panelCardDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelCardDetails.Controls.Add(this.txtTransfer);
            this.panelCardDetails.Controls.Add(this.txtCardNo);
            this.panelCardDetails.Controls.Add(this.lblCreditsToTransfer);
            this.panelCardDetails.Controls.Add(this.lblAvlCredits);
            this.panelCardDetails.Controls.Add(this.label1);
            this.panelCardDetails.Controls.Add(this.txtAvlblTokens);
            this.panelCardDetails.Location = new System.Drawing.Point(192, 289);
            this.panelCardDetails.Name = "panelCardDetails";
            this.panelCardDetails.Size = new System.Drawing.Size(681, 281);
            this.panelCardDetails.TabIndex = 1;
            // 
            // lblTimeRemaining
            // 
            this.lblTimeRemaining.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeRemaining.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemaining.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.timer_SmallBox;
            this.lblTimeRemaining.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lblTimeRemaining.FlatAppearance.BorderSize = 0;
            this.lblTimeRemaining.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemaining.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemaining.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeRemaining.ForeColor = System.Drawing.Color.White;
            this.lblTimeRemaining.Location = new System.Drawing.Point(1126, 28);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(142, 110);
            this.lblTimeRemaining.TabIndex = 20007;
            this.lblTimeRemaining.Text = "1:45";
            this.lblTimeRemaining.UseVisualStyleBackColor = false;
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeading.BackColor = System.Drawing.Color.Transparent;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.ForeColor = System.Drawing.Color.White;
            this.lblHeading.Location = new System.Drawing.Point(6, 12);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(1322, 82);
            this.lblHeading.TabIndex = 20008;
            this.lblHeading.Text = "Transfer Points";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxMessageLine
            // 
            this.textBoxMessageLine.BackColor = System.Drawing.Color.Transparent;
            this.textBoxMessageLine.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.bottom_bar;
            this.textBoxMessageLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.textBoxMessageLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxMessageLine.FlatAppearance.BorderSize = 0;
            this.textBoxMessageLine.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.textBoxMessageLine.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.textBoxMessageLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.textBoxMessageLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessageLine.ForeColor = System.Drawing.Color.White;
            this.textBoxMessageLine.Location = new System.Drawing.Point(0, 963);
            this.textBoxMessageLine.Name = "textBoxMessageLine";
            this.textBoxMessageLine.Size = new System.Drawing.Size(1280, 61);
            this.textBoxMessageLine.TabIndex = 20013;
            this.textBoxMessageLine.Text = "Message";
            this.textBoxMessageLine.UseVisualStyleBackColor = false;
            // 
            // frmTransferFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_Screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.Controls.Add(this.textBoxMessageLine);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.lblTimeRemaining);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblTapMsg);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.panelCardDetails);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmTransferFrom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTransferTokenFrom";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTransferFrom_FormClosed);
            this.Load += new System.EventHandler(this.frmTransferTokenFrom_Load);
            this.Controls.SetChildIndex(this.panelCardDetails, 0);
            this.Controls.SetChildIndex(this.lblSiteName, 0);
            this.Controls.SetChildIndex(this.lblTapMsg, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.lblHeading, 0);
            this.Controls.SetChildIndex(this.lblTimeRemaining, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.buttonNext, 0);
            this.Controls.SetChildIndex(this.textBoxMessageLine, 0);
            this.panelCardDetails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblTapMsg;
        private System.Windows.Forms.Label txtCardNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label txtAvlblTokens;
        private System.Windows.Forms.Label lblAvlCredits;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Label txtTransfer;
        private System.Windows.Forms.Label lblCreditsToTransfer;
        private System.Windows.Forms.Panel panelCardDetails;
        private System.Windows.Forms.Button lblTimeRemaining;
        private System.Windows.Forms.Label lblHeading;
        //private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button textBoxMessageLine;
    }
}
