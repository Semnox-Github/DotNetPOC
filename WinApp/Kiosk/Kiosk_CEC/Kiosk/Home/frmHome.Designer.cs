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
            this.btnNewCard = new System.Windows.Forms.Button();
            this.btnRecharge = new System.Windows.Forms.Button();
            this.btnPauseTime = new System.Windows.Forms.Button();
            this.btnPointsToTime = new System.Windows.Forms.Button();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.btnRedeemTokens = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnCheckBalance = new System.Windows.Forms.Button();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.lblDebug = new System.Windows.Forms.Label();
            this.btnLanguage = new System.Windows.Forms.Button();
            this.btnFAQ = new System.Windows.Forms.Button();
            this.lblAppVersion = new System.Windows.Forms.Label();
            this.pbSemnox = new System.Windows.Forms.PictureBox();
            this.flpOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSemnox)).BeginInit();
            this.SuspendLayout();
            // 
            // flpOptions
            // 
            this.flpOptions.BackColor = System.Drawing.Color.Transparent;
            this.flpOptions.Controls.Add(this.btnNewCard);
            this.flpOptions.Controls.Add(this.btnRecharge);
            this.flpOptions.Controls.Add(this.btnPauseTime);
            this.flpOptions.Controls.Add(this.btnPointsToTime);
            this.flpOptions.Controls.Add(this.btnTransfer);
            this.flpOptions.Controls.Add(this.btnRedeemTokens);
            this.flpOptions.Controls.Add(this.btnRegister);
            this.flpOptions.Controls.Add(this.btnCheckBalance);
            this.flpOptions.Location = new System.Drawing.Point(31, 285);
            this.flpOptions.Name = "flpOptions";
            this.flpOptions.Size = new System.Drawing.Size(1037, 1396);
            this.flpOptions.TabIndex = 3;
            // 
            // btnNewCard
            // 
            this.btnNewCard.BackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.New_Play_Pass_Button;
            this.btnNewCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNewCard.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(254)))));
            this.btnNewCard.FlatAppearance.BorderSize = 0;
            this.btnNewCard.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewCard.Font = new System.Drawing.Font("Bango Pro", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnNewCard.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNewCard.Location = new System.Drawing.Point(3, 3);
            this.btnNewCard.Name = "btnNewCard";
            this.btnNewCard.Size = new System.Drawing.Size(495, 313);
            this.btnNewCard.TabIndex = 0;
            this.btnNewCard.Text = "Buy A New Card";
            this.btnNewCard.UseVisualStyleBackColor = false;
            this.btnNewCard.Click += new System.EventHandler(this.btnNewCard_Click);
            this.btnNewCard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNewCard_MouseDown);
            this.btnNewCard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnNewCard_MouseUp);
            // 
            // btnRecharge
            // 
            this.btnRecharge.BackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.New_Play_Pass_Button;
            this.btnRecharge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRecharge.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(254)))));
            this.btnRecharge.FlatAppearance.BorderSize = 0;
            this.btnRecharge.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecharge.Font = new System.Drawing.Font("Bango Pro", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecharge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnRecharge.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRecharge.Location = new System.Drawing.Point(504, 3);
            this.btnRecharge.Name = "btnRecharge";
            this.btnRecharge.Size = new System.Drawing.Size(495, 313);
            this.btnRecharge.TabIndex = 1;
            this.btnRecharge.Text = "Top Up";
            this.btnRecharge.UseVisualStyleBackColor = false;
            this.btnRecharge.Click += new System.EventHandler(this.btnRecharge_Click);
            this.btnRecharge.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRecharge_MouseDown);
            this.btnRecharge.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRecharge_MouseUp);
            // 
            // btnPauseTime
            // 
            this.btnPauseTime.BackColor = System.Drawing.Color.Transparent;
            this.btnPauseTime.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Exchange_tokens_Button;
            this.btnPauseTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPauseTime.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(254)))));
            this.btnPauseTime.FlatAppearance.BorderSize = 0;
            this.btnPauseTime.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPauseTime.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPauseTime.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPauseTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPauseTime.Font = new System.Drawing.Font("Bango Pro", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPauseTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnPauseTime.Location = new System.Drawing.Point(3, 322);
            this.btnPauseTime.Name = "btnPauseTime";
            this.btnPauseTime.Size = new System.Drawing.Size(1008, 192);
            this.btnPauseTime.TabIndex = 7;
            this.btnPauseTime.Text = "Pause Card";
            this.btnPauseTime.UseVisualStyleBackColor = false;
            this.btnPauseTime.Click += new System.EventHandler(this.btnPauseTime_Click);
            // 
            // btnPointsToTime
            // 
            this.btnPointsToTime.BackColor = System.Drawing.Color.Transparent;
            this.btnPointsToTime.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Exchange_tokens_Button;
            this.btnPointsToTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPointsToTime.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(254)))));
            this.btnPointsToTime.FlatAppearance.BorderSize = 0;
            this.btnPointsToTime.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPointsToTime.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPointsToTime.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPointsToTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPointsToTime.Font = new System.Drawing.Font("Bango Pro", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPointsToTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnPointsToTime.Location = new System.Drawing.Point(3, 520);
            this.btnPointsToTime.Name = "btnPointsToTime";
            this.btnPointsToTime.Size = new System.Drawing.Size(1008, 192);
            this.btnPointsToTime.TabIndex = 6;
            this.btnPointsToTime.Text = "Convert Points To Time";
            this.btnPointsToTime.UseVisualStyleBackColor = false;
            this.btnPointsToTime.Click += new System.EventHandler(this.btnPointsToTime_Click);
            // 
            // btnTransfer
            // 
            this.btnTransfer.BackColor = System.Drawing.Color.Transparent;
            this.btnTransfer.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Exchange_tokens_Button;
            this.btnTransfer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnTransfer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(254)))));
            this.btnTransfer.FlatAppearance.BorderSize = 0;
            this.btnTransfer.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTransfer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTransfer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransfer.Font = new System.Drawing.Font("Bango Pro", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransfer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnTransfer.Location = new System.Drawing.Point(3, 718);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(1008, 190);
            this.btnTransfer.TabIndex = 4;
            this.btnTransfer.Text = "Transfer";
            this.btnTransfer.UseVisualStyleBackColor = false;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            this.btnTransfer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTransfer_MouseDown);
            this.btnTransfer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTransfer_MouseUp);
            // 
            // btnRedeemTokens
            // 
            this.btnRedeemTokens.BackColor = System.Drawing.Color.Transparent;
            this.btnRedeemTokens.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Exchange_tokens_Button;
            this.btnRedeemTokens.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRedeemTokens.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(254)))));
            this.btnRedeemTokens.FlatAppearance.BorderSize = 0;
            this.btnRedeemTokens.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemTokens.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemTokens.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRedeemTokens.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedeemTokens.Font = new System.Drawing.Font("Bango Pro", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRedeemTokens.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnRedeemTokens.Location = new System.Drawing.Point(3, 914);
            this.btnRedeemTokens.Name = "btnRedeemTokens";
            this.btnRedeemTokens.Size = new System.Drawing.Size(1008, 191);
            this.btnRedeemTokens.TabIndex = 5;
            this.btnRedeemTokens.Text = "Redeem Tokens";
            this.btnRedeemTokens.UseVisualStyleBackColor = false;
            this.btnRedeemTokens.Click += new System.EventHandler(this.btnRedeemTokens_Click);
            this.btnRedeemTokens.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRedeemTokens_MouseDown);
            this.btnRedeemTokens.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRedeemTokens_MouseUp);
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.Transparent;
            this.btnRegister.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.check_Baalnce_image;
            this.btnRegister.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRegister.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(254)))));
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRegister.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRegister.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("Bango Pro", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(10, 1111);
            this.btnRegister.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(486, 260);
            this.btnRegister.TabIndex = 2;
            this.btnRegister.TabStop = false;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            this.btnRegister.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRegister_MouseDown);
            this.btnRegister.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRegister_MouseUp);
            // 
            // btnCheckBalance
            // 
            this.btnCheckBalance.BackColor = System.Drawing.Color.Transparent;
            this.btnCheckBalance.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.check_Baalnce_image;
            this.btnCheckBalance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCheckBalance.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(254)))));
            this.btnCheckBalance.FlatAppearance.BorderSize = 0;
            this.btnCheckBalance.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCheckBalance.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCheckBalance.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCheckBalance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckBalance.Font = new System.Drawing.Font("Bango Pro", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckBalance.ForeColor = System.Drawing.Color.White;
            this.btnCheckBalance.Location = new System.Drawing.Point(502, 1111);
            this.btnCheckBalance.Name = "btnCheckBalance";
            this.btnCheckBalance.Size = new System.Drawing.Size(486, 260);
            this.btnCheckBalance.TabIndex = 5;
            this.btnCheckBalance.Text = "Check Balance or Activity";
            this.btnCheckBalance.UseVisualStyleBackColor = false;
            this.btnCheckBalance.Click += new System.EventHandler(this.btnCheckBalance_Click);
            this.btnCheckBalance.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCheckBalance_MouseDown);
            this.btnCheckBalance.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCheckBalance_MouseUp);
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
            this.lblSiteName.Location = new System.Drawing.Point(0, 1);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1080, 82);
            this.lblSiteName.TabIndex = 135;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
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
            this.txtMessage.Font = new System.Drawing.Font("Bango Pro", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1871);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 49);
            this.txtMessage.TabIndex = 136;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.Transparent;
            this.btnHome.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.home_button;
            this.btnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Location = new System.Drawing.Point(31, 107);
            this.btnHome.Margin = new System.Windows.Forms.Padding(0);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(153, 151);
            this.btnHome.TabIndex = 146;
            this.btnHome.Text = "GO HOME";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.BtnHome_Click);
            //  
            // lblDebug
            // 
            this.lblDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDebug.AutoSize = true;
            this.lblDebug.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDebug.Location = new System.Drawing.Point(130, 1827);
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(58, 18);
            this.lblDebug.TabIndex = 138;
            this.lblDebug.Text = "debug";
            // 
            // btnLanguage
            // 
            this.btnLanguage.BackColor = System.Drawing.Color.Transparent;
            this.btnLanguage.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Language_Btn;
            this.btnLanguage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLanguage.FlatAppearance.BorderSize = 0;
            this.btnLanguage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLanguage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLanguage.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnLanguage.Location = new System.Drawing.Point(688, 165);
            this.btnLanguage.Name = "btnLanguage";
            this.btnLanguage.Size = new System.Drawing.Size(329, 87);
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
            this.btnFAQ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFAQ.FlatAppearance.BorderSize = 0;
            this.btnFAQ.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFAQ.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFAQ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFAQ.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFAQ.ForeColor = System.Drawing.Color.White;
            this.btnFAQ.Location = new System.Drawing.Point(55, 1763);
            this.btnFAQ.Name = "btnFAQ";
            this.btnFAQ.Size = new System.Drawing.Size(340, 91);
            this.btnFAQ.TabIndex = 140;
            this.btnFAQ.Text = "FAQ";
            this.btnFAQ.UseVisualStyleBackColor = false;
            this.btnFAQ.Click += new System.EventHandler(this.btnFAQ_Click);
            // 
            // lblAppVersion
            // 
            this.lblAppVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAppVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblAppVersion.Font = new System.Drawing.Font("Gotham Rounded Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppVersion.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblAppVersion.Location = new System.Drawing.Point(826, 1841);
            this.lblAppVersion.Margin = new System.Windows.Forms.Padding(3);
            this.lblAppVersion.Name = "lblAppVersion";
            this.lblAppVersion.Size = new System.Drawing.Size(204, 20);
            this.lblAppVersion.TabIndex = 148;
            this.lblAppVersion.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // pbSemnox
            // 
            this.pbSemnox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSemnox.BackColor = System.Drawing.Color.Transparent;
            this.pbSemnox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbSemnox.Image = global::Parafait_Kiosk.Properties.Resources.semnox_logo;
            this.pbSemnox.Location = new System.Drawing.Point(831, 1793);
            this.pbSemnox.Name = "pbSemnox";
            this.pbSemnox.Size = new System.Drawing.Size(249, 42);
            this.pbSemnox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbSemnox.TabIndex = 5;
            this.pbSemnox.TabStop = false;
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.lblAppVersion);
            this.Controls.Add(this.pbSemnox);
            this.Controls.Add(this.btnFAQ);
            this.Controls.Add(this.btnLanguage);
            this.Controls.Add(this.lblDebug);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.flpOptions);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnHome);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmHome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Semnox Parafait Self-Service Kiosk-Sales";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.frmHome_Activated);
            this.Deactivate += new System.EventHandler(this.frmHome_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmHome_FormClosing);
            this.Load += new System.EventHandler(this.frmHome_Load);
            this.Click += new System.EventHandler(this.frmHome_Click);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmHome_KeyPress);
            this.flpOptions.ResumeLayout(false);
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
        //private System.Windows.Forms.Timer statusTimer;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Label lblDebug;
        private System.Windows.Forms.Button btnLanguage;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnRedeemTokens;
        private System.Windows.Forms.Button btnFAQ;
        private System.Windows.Forms.Button btnPointsToTime;
        private System.Windows.Forms.Button btnPauseTime;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Label lblAppVersion;
        private System.Windows.Forms.PictureBox pbSemnox;
    }
}
