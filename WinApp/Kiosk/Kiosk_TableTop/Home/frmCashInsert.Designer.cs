namespace Parafait_Kiosk
{
    partial class frmCashInsert
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
            this.components = new System.ComponentModel.Container();
            this.TimerMoney = new System.Windows.Forms.Timer(this.components);
            this.exitTimer = new System.Windows.Forms.Timer(this.components);
            this.txtMessage = new System.Windows.Forms.Button();
            this.btnNewCard = new System.Windows.Forms.Button();
            this.lblGreeting1 = new System.Windows.Forms.Label();
            this.btnRecharge = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblCashInserted = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
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
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1012);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 49);
            this.txtMessage.TabIndex = 136;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // btnNewCard
            // 
            this.btnNewCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewCard.BackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.New_Play_Pass_Button_big;
            this.btnNewCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNewCard.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnNewCard.FlatAppearance.BorderSize = 0;
            this.btnNewCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewCard.Font = new System.Drawing.Font("Gotham Rounded Bold", 29F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewCard.ForeColor = System.Drawing.Color.White;
            this.btnNewCard.Location = new System.Drawing.Point(379, 282);
            this.btnNewCard.Name = "btnNewCard";
            this.btnNewCard.Size = new System.Drawing.Size(550, 410);
            this.btnNewCard.TabIndex = 147;
            this.btnNewCard.Text = "Buy NEW Card";
            this.btnNewCard.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNewCard.UseVisualStyleBackColor = false;
            this.btnNewCard.Click += new System.EventHandler(this.btnNewCard_Click);
            // 
            // lblGreeting1
            // 
            this.lblGreeting1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreeting1.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting1.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.White;
            this.lblGreeting1.Location = new System.Drawing.Point(190, 18);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(1485, 91);
            this.lblGreeting1.TabIndex = 149;
            this.lblGreeting1.Text = "Insert more cash";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRecharge
            // 
            this.btnRecharge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRecharge.BackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Recharge_Play_Pass_Big;
            this.btnRecharge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRecharge.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnRecharge.FlatAppearance.BorderSize = 0;
            this.btnRecharge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecharge.Font = new System.Drawing.Font("Gotham Rounded Bold", 29F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecharge.ForeColor = System.Drawing.Color.White;
            this.btnRecharge.Location = new System.Drawing.Point(998, 282);
            this.btnRecharge.Name = "btnRecharge";
            this.btnRecharge.Size = new System.Drawing.Size(550, 410);
            this.btnRecharge.TabIndex = 151;
            this.btnRecharge.Text = "Recharge Card";
            this.btnRecharge.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRecharge.UseVisualStyleBackColor = false;
            this.btnRecharge.Click += new System.EventHandler(this.btnRecharge_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.lblCashInserted);
            this.panel2.Location = new System.Drawing.Point(956, 138);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(274, 113);
            this.panel2.TabIndex = 1055;
            // 
            // lblCashInserted
            // 
            this.lblCashInserted.BackColor = System.Drawing.Color.Transparent;
            this.lblCashInserted.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCashInserted.ForeColor = System.Drawing.Color.White;
            this.lblCashInserted.Location = new System.Drawing.Point(15, 20);
            this.lblCashInserted.Name = "lblCashInserted";
            this.lblCashInserted.Size = new System.Drawing.Size(241, 68);
            this.lblCashInserted.TabIndex = 1048;
            this.lblCashInserted.Text = "9";
            this.lblCashInserted.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(215, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(724, 52);
            this.label1.TabIndex = 1054;
            this.label1.Text = "Cash Inserted:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.btnHome.Font = new System.Drawing.Font("Gotham Rounded Medium", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Location = new System.Drawing.Point(31, 28);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(153, 151);
            this.btnHome.TabIndex = 20011;
            this.btnHome.Text = "GO HOME";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Visible = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(824, 847);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(250, 125);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(10, 733);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1910, 85);
            this.label5.TabIndex = 1056;
            this.label5.Text = "Insert more cash at any time";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmCashInsert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRecharge);
            this.Controls.Add(this.lblGreeting1);
            this.Controls.Add(this.btnNewCard);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmCashInsert";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmRedeemTokens";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCashInsert_FormClosing);
            this.Load += new System.EventHandler(this.frmCashInsert_Load);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer TimerMoney;
        private System.Windows.Forms.Timer exitTimer;
        private System.Windows.Forms.Button btnNewCard;
        internal System.Windows.Forms.Label lblGreeting1;
        private System.Windows.Forms.Button btnRecharge;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblCashInserted;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label5;
    }
}