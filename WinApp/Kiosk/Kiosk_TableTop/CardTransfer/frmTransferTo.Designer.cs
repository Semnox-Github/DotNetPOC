namespace Parafait_Kiosk.CardTransfer
{
    partial class frmTransferTo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTransferTo));
            this.txtMessage = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.lblAvilCredits = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTapMsg = new System.Windows.Forms.Label();
            this.txtTransfrdTokens = new System.Windows.Forms.Label();
            this.lblNewPoints = new System.Windows.Forms.Label();
            this.txtAvlblTokens = new System.Windows.Forms.Label();
            this.txtCardNo = new System.Windows.Forms.Label();
            this.panelCardDetails = new System.Windows.Forms.Panel();
            this.panelTrnsferredTokens = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelTransferer = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtTransferFromCredits = new System.Windows.Forms.Label();
            this.txtFromCard = new System.Windows.Forms.Label();
            this.lblAvilCredits2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTransfererDetails = new System.Windows.Forms.Label();
            this.lblTransfereeDetails = new System.Windows.Forms.Label();
            this.lblTimeRemaining = new System.Windows.Forms.Button();
            this.pbSuccess = new System.Windows.Forms.PictureBox();
            this.lblPointTransferd = new System.Windows.Forms.Label();
            this.panelCardDetails.SuspendLayout();
            this.panelTransferer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSuccess)).BeginInit();
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
            // btnPrev
            // 
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(681, 859);
            this.btnPrev.TabIndex = 1051;
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 1030);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 50);
            this.txtMessage.TabIndex = 147;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // buttonNext
            // 
            this.buttonNext.BackColor = System.Drawing.Color.Transparent;
            this.buttonNext.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.buttonNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonNext.FlatAppearance.BorderSize = 0;
            this.buttonNext.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNext.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNext.ForeColor = System.Drawing.Color.White;
            this.buttonNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonNext.Location = new System.Drawing.Point(993, 859);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.buttonNext.Size = new System.Drawing.Size(250, 125);
            this.buttonNext.TabIndex = 1057;
            this.buttonNext.Text = "Transfer";
            this.buttonNext.UseVisualStyleBackColor = false;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // lblAvilCredits
            // 
            this.lblAvilCredits.BackColor = System.Drawing.Color.Transparent;
            this.lblAvilCredits.Font = new System.Drawing.Font("Gotham Rounded Medium", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvilCredits.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(26)))));
            this.lblAvilCredits.Location = new System.Drawing.Point(372, 440);
            this.lblAvilCredits.Name = "lblAvilCredits";
            this.lblAvilCredits.Size = new System.Drawing.Size(613, 52);
            this.lblAvilCredits.TabIndex = 1056;
            this.lblAvilCredits.Text = "Available Credits:";
            this.lblAvilCredits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(26)))));
            this.label1.Location = new System.Drawing.Point(364, 364);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(621, 52);
            this.label1.TabIndex = 1054;
            this.label1.Text = "Card No:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTapMsg
            // 
            this.lblTapMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTapMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblTapMsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTapMsg.ForeColor = System.Drawing.Color.White;
            this.lblTapMsg.Location = new System.Drawing.Point(190, 9);
            this.lblTapMsg.Name = "lblTapMsg";
            this.lblTapMsg.Size = new System.Drawing.Size(1529, 139);
            this.lblTapMsg.TabIndex = 1052;
            this.lblTapMsg.Text = "Success";
            this.lblTapMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTransfrdTokens
            // 
            this.txtTransfrdTokens.BackColor = System.Drawing.Color.Transparent;
            this.txtTransfrdTokens.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransfrdTokens.ForeColor = System.Drawing.Color.White;
            this.txtTransfrdTokens.Location = new System.Drawing.Point(991, 514);
            this.txtTransfrdTokens.Name = "txtTransfrdTokens";
            this.txtTransfrdTokens.Size = new System.Drawing.Size(262, 55);
            this.txtTransfrdTokens.TabIndex = 1060;
            this.txtTransfrdTokens.Text = "9";
            this.txtTransfrdTokens.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNewPoints
            // 
            this.lblNewPoints.BackColor = System.Drawing.Color.Transparent;
            this.lblNewPoints.Font = new System.Drawing.Font("Gotham Rounded Medium", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewPoints.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(26)))));
            this.lblNewPoints.Location = new System.Drawing.Point(372, 515);
            this.lblNewPoints.Name = "lblNewPoints";
            this.lblNewPoints.Size = new System.Drawing.Size(613, 52);
            this.lblNewPoints.TabIndex = 1059;
            this.lblNewPoints.Text = "New Credits:";
            this.lblNewPoints.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAvlblTokens
            // 
            this.txtAvlblTokens.BackColor = System.Drawing.Color.Transparent;
            this.txtAvlblTokens.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAvlblTokens.ForeColor = System.Drawing.Color.White;
            this.txtAvlblTokens.Location = new System.Drawing.Point(991, 439);
            this.txtAvlblTokens.Name = "txtAvlblTokens";
            this.txtAvlblTokens.Size = new System.Drawing.Size(243, 55);
            this.txtAvlblTokens.TabIndex = 1058;
            this.txtAvlblTokens.Text = "9";
            this.txtAvlblTokens.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCardNo
            // 
            this.txtCardNo.BackColor = System.Drawing.Color.Transparent;
            this.txtCardNo.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(26)))));
            this.txtCardNo.Location = new System.Drawing.Point(991, 363);
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.Size = new System.Drawing.Size(243, 55);
            this.txtCardNo.TabIndex = 1057;
            this.txtCardNo.Text = "9";
            this.txtCardNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelCardDetails
            // 
            this.panelCardDetails.BackColor = System.Drawing.Color.Black;
            this.panelCardDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelCardDetails.Controls.Add(this.panelTrnsferredTokens);
            this.panelCardDetails.Controls.Add(this.panel2);
            this.panelCardDetails.Controls.Add(this.panel1);
            this.panelCardDetails.Location = new System.Drawing.Point(20, 1399);
            this.panelCardDetails.Name = "panelCardDetails";
            this.panelCardDetails.Size = new System.Drawing.Size(56, 72);
            this.panelCardDetails.TabIndex = 20006;
            this.panelCardDetails.Visible = false;
            // 
            // panelTrnsferredTokens
            // 
            this.panelTrnsferredTokens.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelTrnsferredTokens.BackgroundImage")));
            this.panelTrnsferredTokens.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelTrnsferredTokens.Location = new System.Drawing.Point(541, 356);
            this.panelTrnsferredTokens.Name = "panelTrnsferredTokens";
            this.panelTrnsferredTokens.Size = new System.Drawing.Size(432, 168);
            this.panelTrnsferredTokens.TabIndex = 1062;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.Location = new System.Drawing.Point(541, 182);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(432, 168);
            this.panel2.TabIndex = 1062;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Location = new System.Drawing.Point(541, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(432, 168);
            this.panel1.TabIndex = 1061;
            // 
            // panelTransferer
            // 
            this.panelTransferer.BackColor = System.Drawing.Color.Black;
            this.panelTransferer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelTransferer.Controls.Add(this.panel5);
            this.panelTransferer.Controls.Add(this.panel4);
            this.panelTransferer.Location = new System.Drawing.Point(12, 1497);
            this.panelTransferer.Name = "panelTransferer";
            this.panelTransferer.Size = new System.Drawing.Size(76, 56);
            this.panelTransferer.TabIndex = 20007;
            this.panelTransferer.Visible = false;
            // 
            // panel5
            // 
            this.panel5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel5.BackgroundImage")));
            this.panel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel5.Location = new System.Drawing.Point(541, 188);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(432, 168);
            this.panel5.TabIndex = 1064;
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel4.BackgroundImage")));
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel4.Location = new System.Drawing.Point(541, 14);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(432, 168);
            this.panel4.TabIndex = 1063;
            // 
            // txtTransferFromCredits
            // 
            this.txtTransferFromCredits.BackColor = System.Drawing.Color.Transparent;
            this.txtTransferFromCredits.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransferFromCredits.ForeColor = System.Drawing.Color.White;
            this.txtTransferFromCredits.Location = new System.Drawing.Point(991, 766);
            this.txtTransferFromCredits.Name = "txtTransferFromCredits";
            this.txtTransferFromCredits.Size = new System.Drawing.Size(262, 55);
            this.txtTransferFromCredits.TabIndex = 1058;
            this.txtTransferFromCredits.Text = "9";
            this.txtTransferFromCredits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtTransferFromCredits.Visible = false;
            // 
            // txtFromCard
            // 
            this.txtFromCard.BackColor = System.Drawing.Color.Transparent;
            this.txtFromCard.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFromCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(26)))));
            this.txtFromCard.Location = new System.Drawing.Point(991, 691);
            this.txtFromCard.Name = "txtFromCard";
            this.txtFromCard.Size = new System.Drawing.Size(243, 55);
            this.txtFromCard.TabIndex = 1057;
            this.txtFromCard.Text = "9";
            this.txtFromCard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtFromCard.Visible = false;
            // 
            // lblAvilCredits2
            // 
            this.lblAvilCredits2.BackColor = System.Drawing.Color.Transparent;
            this.lblAvilCredits2.Font = new System.Drawing.Font("Gotham Rounded Medium", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvilCredits2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(26)))));
            this.lblAvilCredits2.Location = new System.Drawing.Point(272, 765);
            this.lblAvilCredits2.Name = "lblAvilCredits2";
            this.lblAvilCredits2.Size = new System.Drawing.Size(713, 55);
            this.lblAvilCredits2.TabIndex = 1056;
            this.lblAvilCredits2.Text = "Available Credits:";
            this.lblAvilCredits2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAvilCredits2.Visible = false;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(26)))));
            this.label6.Location = new System.Drawing.Point(433, 691);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(552, 55);
            this.label6.TabIndex = 1054;
            this.label6.Text = "Card No:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.Visible = false;
            // 
            // lblTransfererDetails
            // 
            this.lblTransfererDetails.BackColor = System.Drawing.Color.Transparent;
            this.lblTransfererDetails.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransfererDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(26)))));
            this.lblTransfererDetails.Location = new System.Drawing.Point(570, 285);
            this.lblTransfererDetails.Name = "lblTransfererDetails";
            this.lblTransfererDetails.Size = new System.Drawing.Size(796, 59);
            this.lblTransfererDetails.TabIndex = 20008;
            this.lblTransfererDetails.Text = "Transfer To";
            this.lblTransfererDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTransfereeDetails
            // 
            this.lblTransfereeDetails.BackColor = System.Drawing.Color.Transparent;
            this.lblTransfereeDetails.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransfereeDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(26)))));
            this.lblTransfereeDetails.Location = new System.Drawing.Point(570, 602);
            this.lblTransfereeDetails.Name = "lblTransfereeDetails";
            this.lblTransfereeDetails.Size = new System.Drawing.Size(814, 59);
            this.lblTransfereeDetails.TabIndex = 20009;
            this.lblTransfereeDetails.Text = "From ";
            this.lblTransfereeDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTransfereeDetails.Visible = false;
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
            this.lblTimeRemaining.Font = new System.Drawing.Font("Verdana", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeRemaining.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblTimeRemaining.Location = new System.Drawing.Point(1740, 38);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(142, 110);
            this.lblTimeRemaining.TabIndex = 20011;
            this.lblTimeRemaining.Text = "1:45";
            this.lblTimeRemaining.UseVisualStyleBackColor = false;
            // 
            // pbSuccess
            // 
            this.pbSuccess.BackColor = System.Drawing.Color.Transparent;
            this.pbSuccess.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbSuccess.Image = global::Parafait_Kiosk.Properties.Resources.sucessTransfer;
            this.pbSuccess.Location = new System.Drawing.Point(1350, 363);
            this.pbSuccess.Name = "pbSuccess";
            this.pbSuccess.Size = new System.Drawing.Size(421, 340);
            this.pbSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSuccess.TabIndex = 20013;
            this.pbSuccess.TabStop = false;
            this.pbSuccess.Visible = false;
            // 
            // lblPointTransferd
            // 
            this.lblPointTransferd.BackColor = System.Drawing.Color.Transparent;
            this.lblPointTransferd.Font = new System.Drawing.Font("Gotham Rounded Medium", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPointTransferd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(26)))));
            this.lblPointTransferd.Location = new System.Drawing.Point(189, 210);
            this.lblPointTransferd.Name = "lblPointTransferd";
            this.lblPointTransferd.Size = new System.Drawing.Size(1530, 52);
            this.lblPointTransferd.TabIndex = 20014;
            this.lblPointTransferd.Text = "points have been transferred";
            this.lblPointTransferd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPointTransferd.Visible = false;
            // 
            // frmTransferTo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.lblTimeRemaining);
            this.Controls.Add(this.lblPointTransferd);
            this.Controls.Add(this.pbSuccess);
            this.Controls.Add(this.txtTransferFromCredits);
            this.Controls.Add(this.txtFromCard);
            this.Controls.Add(this.lblAvilCredits2);
            this.Controls.Add(this.txtTransfrdTokens);
            this.Controls.Add(this.txtAvlblTokens);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCardNo);
            this.Controls.Add(this.lblNewPoints);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.lblAvilCredits);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTransfereeDetails);
            this.Controls.Add(this.lblTransfererDetails);
            this.Controls.Add(this.panelTransferer);
            this.Controls.Add(this.lblTapMsg);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.panelCardDetails);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmTransferTo";
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTransferTokenTo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTransferTo_FormClosed);
            this.Load += new System.EventHandler(this.frmTransferTokenTo_Load);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.panelCardDetails, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.lblTapMsg, 0);
            this.Controls.SetChildIndex(this.panelTransferer, 0);
            this.Controls.SetChildIndex(this.lblTransfererDetails, 0);
            this.Controls.SetChildIndex(this.lblTransfereeDetails, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.lblAvilCredits, 0);
            this.Controls.SetChildIndex(this.buttonNext, 0);
            this.Controls.SetChildIndex(this.lblNewPoints, 0);
            this.Controls.SetChildIndex(this.txtCardNo, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtAvlblTokens, 0);
            this.Controls.SetChildIndex(this.txtTransfrdTokens, 0);
            this.Controls.SetChildIndex(this.lblAvilCredits2, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.txtFromCard, 0);
            this.Controls.SetChildIndex(this.txtTransferFromCredits, 0);
            this.Controls.SetChildIndex(this.pbSuccess, 0);
            this.Controls.SetChildIndex(this.lblPointTransferd, 0);
            this.Controls.SetChildIndex(this.lblTimeRemaining, 0);
            this.panelCardDetails.ResumeLayout(false);
            this.panelTransferer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSuccess)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Label lblAvilCredits;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTapMsg;
        //private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label txtCardNo;
        private System.Windows.Forms.Label txtAvlblTokens;
        private System.Windows.Forms.Label txtTransfrdTokens;
        private System.Windows.Forms.Label lblNewPoints;
        private System.Windows.Forms.Panel panelCardDetails;
        private System.Windows.Forms.Panel panelTransferer;
        private System.Windows.Forms.Label txtFromCard;
        private System.Windows.Forms.Label lblAvilCredits2;
        private System.Windows.Forms.Label txtTransferFromCredits;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblTransfererDetails;
        private System.Windows.Forms.Label lblTransfereeDetails;
        private System.Windows.Forms.Button lblTimeRemaining;
        private System.Windows.Forms.Panel panelTrnsferredTokens;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        //private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.PictureBox pbSuccess;
        private System.Windows.Forms.Label lblPointTransferd;
    }
}
