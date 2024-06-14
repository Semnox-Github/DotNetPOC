namespace Parafait_Kiosk
{
    partial class frmHome
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
            this.flpOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.flpBigButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNewCard = new System.Windows.Forms.Button();
            this.btnRecharge = new System.Windows.Forms.Button();
            this.btnPointsToTime = new System.Windows.Forms.Button();
            this.btnPauseTime = new System.Windows.Forms.Button();
            this.flpSmallButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCheckBalance = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.btnRedeemTokens = new System.Windows.Forms.Button();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            this.lblDebug = new System.Windows.Forms.Label();
            this.pbSemnox = new System.Windows.Forms.PictureBox();
            this.btnLanguage = new System.Windows.Forms.Button();
            this.btnFAQ = new System.Windows.Forms.Button();
            this.flpOptions.SuspendLayout();
            this.flpBigButtons.SuspendLayout();
            this.flpSmallButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSemnox)).BeginInit();
            this.SuspendLayout();
            // 
            // flpOptions
            // 
            this.flpOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpOptions.BackColor = System.Drawing.Color.Transparent;
            this.flpOptions.Controls.Add(this.flpBigButtons);
            this.flpOptions.Controls.Add(this.flpSmallButtons);
            this.flpOptions.Location = new System.Drawing.Point(62, 166);
            this.flpOptions.Name = "flpOptions";
            this.flpOptions.Size = new System.Drawing.Size(1157, 718);
            this.flpOptions.TabIndex = 3;
            // 
            // flpBigButtons
            // 
            this.flpBigButtons.Controls.Add(this.btnNewCard);
            this.flpBigButtons.Controls.Add(this.btnRecharge);
            this.flpBigButtons.Controls.Add(this.btnPointsToTime);
            this.flpBigButtons.Controls.Add(this.btnPauseTime);
            this.flpBigButtons.Location = new System.Drawing.Point(0, 0);
            this.flpBigButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flpBigButtons.Name = "flpBigButtons";
            this.flpBigButtons.Size = new System.Drawing.Size(1197, 448);
            this.flpBigButtons.TabIndex = 9;
            // 
            // btnNewCard
            // 
            this.btnNewCard.BackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.New_Card_Big;
            this.btnNewCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnNewCard.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnNewCard.FlatAppearance.BorderSize = 0;
            this.btnNewCard.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewCard.ForeColor = System.Drawing.Color.White;
            this.btnNewCard.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNewCard.Location = new System.Drawing.Point(10, 10);
            this.btnNewCard.Margin = new System.Windows.Forms.Padding(10);
            this.btnNewCard.Name = "btnNewCard";
            this.btnNewCard.Size = new System.Drawing.Size(550, 410);
            this.btnNewCard.TabIndex = 0;
            this.btnNewCard.Text = "Buy A New Card";
            this.btnNewCard.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNewCard.UseVisualStyleBackColor = false;
            this.btnNewCard.Click += new System.EventHandler(this.btnNewCard_Click);
            this.btnNewCard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNewCard_MouseDown);
            this.btnNewCard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnNewCard_MouseUp);
            // 
            // btnRecharge
            // 
            this.btnRecharge.BackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Recharge_Card_Big;
            this.btnRecharge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRecharge.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnRecharge.FlatAppearance.BorderSize = 0;
            this.btnRecharge.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecharge.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecharge.ForeColor = System.Drawing.Color.White;
            this.btnRecharge.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRecharge.Location = new System.Drawing.Point(580, 10);
            this.btnRecharge.Margin = new System.Windows.Forms.Padding(10);
            this.btnRecharge.Name = "btnRecharge";
            this.btnRecharge.Size = new System.Drawing.Size(550, 410);
            this.btnRecharge.TabIndex = 1;
            this.btnRecharge.Text = "Top Up";
            this.btnRecharge.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRecharge.UseVisualStyleBackColor = false;
            this.btnRecharge.Click += new System.EventHandler(this.btnRecharge_Click);
            this.btnRecharge.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRecharge_MouseDown);
            this.btnRecharge.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRecharge_MouseUp);
            // 
            // btnPointsToTime
            // 
            this.btnPointsToTime.BackColor = System.Drawing.Color.Transparent;
            this.btnPointsToTime.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Points_To_Time;
            this.btnPointsToTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPointsToTime.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPointsToTime.FlatAppearance.BorderSize = 0;
            this.btnPointsToTime.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPointsToTime.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPointsToTime.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPointsToTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPointsToTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPointsToTime.ForeColor = System.Drawing.Color.White;
            this.btnPointsToTime.Location = new System.Drawing.Point(10, 440);
            this.btnPointsToTime.Margin = new System.Windows.Forms.Padding(10);
            this.btnPointsToTime.Name = "btnPointsToTime";
            this.btnPointsToTime.Size = new System.Drawing.Size(265, 252);
            this.btnPointsToTime.TabIndex = 8;
            this.btnPointsToTime.Text = "Convert Points to Time";
            this.btnPointsToTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPointsToTime.UseVisualStyleBackColor = false;
            this.btnPointsToTime.Click += new System.EventHandler(this.btnPointsToTime_Click);
            // 
            // btnPauseTime
            // 
            this.btnPauseTime.BackColor = System.Drawing.Color.Transparent;
            this.btnPauseTime.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Pause_Card;
            this.btnPauseTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPauseTime.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPauseTime.FlatAppearance.BorderSize = 0;
            this.btnPauseTime.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPauseTime.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPauseTime.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPauseTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPauseTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPauseTime.ForeColor = System.Drawing.Color.White;
            this.btnPauseTime.Location = new System.Drawing.Point(295, 440);
            this.btnPauseTime.Margin = new System.Windows.Forms.Padding(10);
            this.btnPauseTime.Name = "btnPauseTime";
            this.btnPauseTime.Size = new System.Drawing.Size(265, 252);
            this.btnPauseTime.TabIndex = 7;
            this.btnPauseTime.Text = "Pause Card";
            this.btnPauseTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPauseTime.UseVisualStyleBackColor = false;
            this.btnPauseTime.Click += new System.EventHandler(this.btnPauseTime_Click);
            // 
            // flpSmallButtons
            // 
            this.flpSmallButtons.Controls.Add(this.btnCheckBalance);
            this.flpSmallButtons.Controls.Add(this.btnRegister);
            this.flpSmallButtons.Controls.Add(this.btnTransfer);
            this.flpSmallButtons.Controls.Add(this.btnRedeemTokens);
            this.flpSmallButtons.Location = new System.Drawing.Point(0, 448);
            this.flpSmallButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flpSmallButtons.Name = "flpSmallButtons";
            this.flpSmallButtons.Size = new System.Drawing.Size(1153, 266);
            this.flpSmallButtons.TabIndex = 6;
            // 
            // btnCheckBalance
            // 
            this.btnCheckBalance.BackColor = System.Drawing.Color.Transparent;
            this.btnCheckBalance.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Check_Balance;
            this.btnCheckBalance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCheckBalance.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCheckBalance.FlatAppearance.BorderSize = 0;
            this.btnCheckBalance.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCheckBalance.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCheckBalance.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCheckBalance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckBalance.ForeColor = System.Drawing.Color.White;
            this.btnCheckBalance.Location = new System.Drawing.Point(10, 10);
            this.btnCheckBalance.Margin = new System.Windows.Forms.Padding(10);
            this.btnCheckBalance.Name = "btnCheckBalance";
            this.btnCheckBalance.Size = new System.Drawing.Size(265, 245);
            this.btnCheckBalance.TabIndex = 3;
            this.btnCheckBalance.Text = "Balance / Activity";
            this.btnCheckBalance.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCheckBalance.UseVisualStyleBackColor = false;
            this.btnCheckBalance.Click += new System.EventHandler(this.btnCheckBalance_Click);
            this.btnCheckBalance.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCheckBalance_MouseDown);
            this.btnCheckBalance.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCheckBalance_MouseUp);
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.Transparent;
            this.btnRegister.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Register_pass;
            this.btnRegister.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRegister.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRegister.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRegister.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(295, 10);
            this.btnRegister.Margin = new System.Windows.Forms.Padding(10);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(265, 245);
            this.btnRegister.TabIndex = 2;
            this.btnRegister.Text = "Register";
            this.btnRegister.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            this.btnRegister.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRegister_MouseDown);
            this.btnRegister.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRegister_MouseUp);
            // 
            // btnTransfer
            // 
            this.btnTransfer.BackColor = System.Drawing.Color.Transparent;
            this.btnTransfer.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Transfer_Points;
            this.btnTransfer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnTransfer.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnTransfer.FlatAppearance.BorderSize = 0;
            this.btnTransfer.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTransfer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTransfer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransfer.ForeColor = System.Drawing.Color.White;
            this.btnTransfer.Location = new System.Drawing.Point(580, 10);
            this.btnTransfer.Margin = new System.Windows.Forms.Padding(10);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(265, 245);
            this.btnTransfer.TabIndex = 4;
            this.btnTransfer.Text = "Transfer";
            this.btnTransfer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTransfer.UseVisualStyleBackColor = false;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            this.btnTransfer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTransfer_MouseDown);
            this.btnTransfer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTransfer_MouseUp);
            // 
            // btnRedeemTokens
            // 
            this.btnRedeemTokens.BackColor = System.Drawing.Color.Transparent;
            this.btnRedeemTokens.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Redeem_Tokens;
            this.btnRedeemTokens.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRedeemTokens.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnRedeemTokens.FlatAppearance.BorderSize = 0;
            this.btnRedeemTokens.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemTokens.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemTokens.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemTokens.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedeemTokens.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRedeemTokens.ForeColor = System.Drawing.Color.White;
            this.btnRedeemTokens.Location = new System.Drawing.Point(865, 10);
            this.btnRedeemTokens.Margin = new System.Windows.Forms.Padding(10);
            this.btnRedeemTokens.Name = "btnRedeemTokens";
            this.btnRedeemTokens.Size = new System.Drawing.Size(265, 245);
            this.btnRedeemTokens.TabIndex = 5;
            this.btnRedeemTokens.Text = "Redeem Tokens";
            this.btnRedeemTokens.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRedeemTokens.UseVisualStyleBackColor = false;
            this.btnRedeemTokens.Click += new System.EventHandler(this.btnRedeemTokens_Click);
            this.btnRedeemTokens.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRedeemTokens_MouseDown);
            this.btnRedeemTokens.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRedeemTokens_MouseUp);
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
            this.lblSiteName.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(12, 0);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1256, 80);
            this.lblSiteName.TabIndex = 135;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.bottom_bar;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 975);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1280, 49);
            this.txtMessage.TabIndex = 136;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // lblDebug
            // 
            this.lblDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDebug.AutoSize = true;
            this.lblDebug.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDebug.Location = new System.Drawing.Point(337, 931);
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(58, 18);
            this.lblDebug.TabIndex = 138;
            this.lblDebug.Text = "debug";
            // 
            // pbSemnox
            // 
            this.pbSemnox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSemnox.BackColor = System.Drawing.Color.Transparent;
            this.pbSemnox.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Semnox;
            this.pbSemnox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbSemnox.Location = new System.Drawing.Point(1139, 925);
            this.pbSemnox.Name = "pbSemnox";
            this.pbSemnox.Size = new System.Drawing.Size(135, 47);
            this.pbSemnox.TabIndex = 5;
            this.pbSemnox.TabStop = false;
            this.pbSemnox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbSemnox_MouseClick);
            // 
            // btnLanguage
            // 
            this.btnLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLanguage.BackColor = System.Drawing.Color.Transparent;
            this.btnLanguage.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.terms_button;
            this.btnLanguage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLanguage.FlatAppearance.BorderSize = 0;
            this.btnLanguage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLanguage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLanguage.ForeColor = System.Drawing.Color.White;
            this.btnLanguage.Location = new System.Drawing.Point(1125, 87);
            this.btnLanguage.Name = "btnLanguage";
            this.btnLanguage.Size = new System.Drawing.Size(151, 56);
            this.btnLanguage.TabIndex = 139;
            this.btnLanguage.Text = "English";
            this.btnLanguage.UseVisualStyleBackColor = false;
            this.btnLanguage.Click += new System.EventHandler(this.btnLanguage_Click);
            // 
            // btnFAQ
            // 
            this.btnFAQ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFAQ.BackColor = System.Drawing.Color.Transparent;
            this.btnFAQ.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.terms_button;
            this.btnFAQ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFAQ.FlatAppearance.BorderSize = 0;
            this.btnFAQ.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFAQ.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFAQ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFAQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFAQ.ForeColor = System.Drawing.Color.White;
            this.btnFAQ.Location = new System.Drawing.Point(12, 902);
            this.btnFAQ.Name = "btnFAQ";
            this.btnFAQ.Size = new System.Drawing.Size(151, 56);
            this.btnFAQ.TabIndex = 140;
            this.btnFAQ.Text = "FAQ";
            this.btnFAQ.UseVisualStyleBackColor = false;
            this.btnFAQ.Click += new System.EventHandler(this.btnFAQ_Click);
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_Screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.Controls.Add(this.btnFAQ);
            this.Controls.Add(this.btnLanguage);
            this.Controls.Add(this.lblDebug);
            this.Controls.Add(this.pbSemnox);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.flpOptions);
            this.Controls.Add(this.txtMessage);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmHome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Semnox Parafait Self-Service Kiosk";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.frmHome_Activated);
            this.Deactivate += new System.EventHandler(this.frmHome_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmHome_FormClosing);
            this.Load += new System.EventHandler(this.frmHome_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmHome_KeyPress);
            this.flpOptions.ResumeLayout(false);
            this.flpBigButtons.ResumeLayout(false);
            this.flpSmallButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSemnox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNewCard;
        private System.Windows.Forms.Button btnRecharge;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.FlowLayoutPanel flpOptions;
        private System.Windows.Forms.Button btnCheckBalance;
        private System.Windows.Forms.PictureBox pbSemnox;
        //private System.Windows.Forms.Timer statusTimer;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Label lblDebug;
        private System.Windows.Forms.Button btnLanguage;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnRedeemTokens;
        private System.Windows.Forms.Button btnFAQ;
        private System.Windows.Forms.FlowLayoutPanel flpSmallButtons;
        private System.Windows.Forms.Button btnPauseTime;
        private System.Windows.Forms.Button btnPointsToTime;
        private System.Windows.Forms.FlowLayoutPanel flpBigButtons;
    }
}
