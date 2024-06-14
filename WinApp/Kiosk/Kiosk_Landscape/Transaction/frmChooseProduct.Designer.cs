namespace Parafait_Kiosk
{
    partial class frmChooseProduct
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
            this.flpCardProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSampleName = new System.Windows.Forms.Button();
            this.lblGreeting2 = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblGreeting1 = new System.Windows.Forms.Label();
            this.lblGreetingDeposit = new System.Windows.Forms.Label();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.inactivityTimer = new System.Windows.Forms.Timer(this.components);
            this.txtMessage = new System.Windows.Forms.Button();
            //this.btnHome = new System.Windows.Forms.Button();
            this.btnScrollLeft = new System.Windows.Forms.Button();
            this.btnScrollRight = new System.Windows.Forms.Button();
            this.flpCardProducts.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpCardProducts
            // 
            this.flpCardProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpCardProducts.BackColor = System.Drawing.Color.Transparent;
            this.flpCardProducts.Controls.Add(this.btnSampleName);
            this.flpCardProducts.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpCardProducts.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.flpCardProducts.Location = new System.Drawing.Point(142, 235);
            this.flpCardProducts.Margin = new System.Windows.Forms.Padding(5);
            this.flpCardProducts.Name = "flpCardProducts";
            this.flpCardProducts.Size = new System.Drawing.Size(974, 526);
            this.flpCardProducts.TabIndex = 1;
            // 
            // btnSampleName
            // 
            this.btnSampleName.BackColor = System.Drawing.Color.Transparent;
            this.btnSampleName.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.plain_product_button;
            this.btnSampleName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSampleName.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnSampleName.FlatAppearance.BorderSize = 0;
            this.btnSampleName.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSampleName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSampleName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSampleName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSampleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSampleName.ForeColor = System.Drawing.Color.White;
            this.btnSampleName.Location = new System.Drawing.Point(6, 6);
            this.btnSampleName.Margin = new System.Windows.Forms.Padding(6);
            this.btnSampleName.Name = "btnSampleName";
            this.btnSampleName.Size = new System.Drawing.Size(314, 242);
            this.btnSampleName.TabIndex = 0;
            this.btnSampleName.Text = "VARIABLE";
            this.btnSampleName.UseVisualStyleBackColor = false;
            this.btnSampleName.Visible = false;
            // 
            // lblGreeting2
            // 
            this.lblGreeting2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreeting2.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting2.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F);
            this.lblGreeting2.ForeColor = System.Drawing.Color.White;
            this.lblGreeting2.Location = new System.Drawing.Point(6, 12);
            this.lblGreeting2.Name = "lblGreeting2";
            this.lblGreeting2.Size = new System.Drawing.Size(1248, 106);
            this.lblGreeting2.TabIndex = 132;
            this.lblGreeting2.Text = "Select Amount";
            this.lblGreeting2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.btnPrev.Location = new System.Drawing.Point(20, 85);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(142, 110);
            this.btnPrev.TabIndex = 2;
            this.btnPrev.Text = "BACK";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Visible = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            this.btnPrev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseDown);
            this.btnPrev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseUp);
            // 
            // lblGreeting1
            // 
            this.lblGreeting1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreeting1.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting1.Font = new System.Drawing.Font("Verdana", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.White;
            this.lblGreeting1.Location = new System.Drawing.Point(142, 134);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(1014, 51);
            this.lblGreeting1.TabIndex = 142;
            this.lblGreeting1.Text = "Customer Greeting";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGreeting1.Visible = false;
            // 
            // lblGreetingDeposit
            // 
            this.lblGreetingDeposit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreetingDeposit.BackColor = System.Drawing.Color.Transparent;
            this.lblGreetingDeposit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreetingDeposit.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreetingDeposit.ForeColor = System.Drawing.Color.DimGray;
            this.lblGreetingDeposit.Location = new System.Drawing.Point(137, 192);
            this.lblGreetingDeposit.Name = "lblGreetingDeposit";
            this.lblGreetingDeposit.Size = new System.Drawing.Size(1116, 29);
            this.lblGreetingDeposit.TabIndex = 141;
            this.lblGreetingDeposit.Text = "@CardDeposit Non-refundable Card Deposit will be levied";
            this.lblGreetingDeposit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGreetingDeposit.Visible = false;
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
            this.lblSiteName.Font = new System.Drawing.Font("Verdana", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(0, 0);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1260, 77);
            this.lblSiteName.TabIndex = 136;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            this.lblSiteName.Visible = false;
            // 
            // inactivityTimer
            // 
            //this.inactivityTimer.Enabled = true;
            //this.inactivityTimer.Interval = 1000;
            //this.inactivityTimer.Tick += new System.EventHandler(this.inactivityTimer_Tick);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.bottom_bar;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 940);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1276, 57);
            this.txtMessage.TabIndex = 2;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // btnHome
            // 
            //this.btnHome.BackColor = System.Drawing.Color.Transparent;
            this.btnHome.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.home_button;
            //this.btnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            //this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            //this.btnHome.FlatAppearance.BorderSize = 0;
            //this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            //this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            //this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            //this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.btnHome.ForeColor = System.Drawing.Color.White;
            //this.btnHome.Location = new System.Drawing.Point(31, 28);
            this.btnHome.Margin = new System.Windows.Forms.Padding(0);
            //this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(153, 151);
            this.btnHome.TabIndex = 20013;
            //this.btnHome.Text = "GO HOME";
            //this.btnHome.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            //this.btnHome.UseVisualStyleBackColor = false;
            //this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnScrollLeft
            // 
            this.btnScrollLeft.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnScrollLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnScrollLeft.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Left_Button;
            this.btnScrollLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnScrollLeft.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnScrollLeft.FlatAppearance.BorderSize = 0;
            this.btnScrollLeft.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnScrollLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnScrollLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnScrollLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScrollLeft.Location = new System.Drawing.Point(26, 486);
            this.btnScrollLeft.Margin = new System.Windows.Forms.Padding(0);
            this.btnScrollLeft.Name = "btnScrollLeft";
            this.btnScrollLeft.Size = new System.Drawing.Size(67, 66);
            this.btnScrollLeft.TabIndex = 20014;
            this.btnScrollLeft.UseVisualStyleBackColor = false;
            this.btnScrollLeft.Visible = false;
            this.btnScrollLeft.Click += new System.EventHandler(this.btnScrollLeft_Click);
            // 
            // btnScrollRight
            // 
            this.btnScrollRight.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnScrollRight.BackColor = System.Drawing.Color.Transparent;
            this.btnScrollRight.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Right_Button;
            this.btnScrollRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnScrollRight.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnScrollRight.FlatAppearance.BorderSize = 0;
            this.btnScrollRight.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnScrollRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnScrollRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnScrollRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScrollRight.Location = new System.Drawing.Point(1187, 486);
            this.btnScrollRight.Margin = new System.Windows.Forms.Padding(0);
            this.btnScrollRight.Name = "btnScrollRight";
            this.btnScrollRight.Size = new System.Drawing.Size(67, 66);
            this.btnScrollRight.TabIndex = 20015;
            this.btnScrollRight.UseVisualStyleBackColor = false;
            this.btnScrollRight.Visible = false;
            this.btnScrollRight.Click += new System.EventHandler(this.btnScrollRight_Click);
            // 
            // frmChooseProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_Screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1276, 997);
            this.Controls.Add(this.btnScrollRight);
            this.Controls.Add(this.btnScrollLeft);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.flpCardProducts);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblGreeting2);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.lblGreetingDeposit);
            this.Controls.Add(this.lblGreeting1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmChooseProduct";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmChooseProduct";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.frmChooseProduct_Activated);
            this.Deactivate += new System.EventHandler(this.frmChooseProduct_Deactivate);
            this.Load += new System.EventHandler(this.frmChooseProduct_Load);
            this.flpCardProducts.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpCardProducts;
        private System.Windows.Forms.Button btnPrev;
        internal System.Windows.Forms.Label lblGreeting2;
        private System.Windows.Forms.Timer inactivityTimer;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button lblSiteName;
        internal System.Windows.Forms.Label lblGreetingDeposit;
        internal System.Windows.Forms.Label lblGreeting1;
        private System.Windows.Forms.Button btnSampleName;
        //private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnScrollLeft;
        private System.Windows.Forms.Button btnScrollRight;
    }
}
