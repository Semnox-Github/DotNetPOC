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
            this.lblGreeting1 = new System.Windows.Forms.Label();
            this.inactivityTimer = new System.Windows.Forms.Timer(this.components);
            this.btnVariable = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            this.btnScrollLeft = new System.Windows.Forms.Button();
            this.btnScrollRight = new System.Windows.Forms.Button();
            this.btnProceed = new System.Windows.Forms.Button();
            this.flpCardProducts.SuspendLayout();
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
            this.btnHome.Location = new System.Drawing.Point(34, 27);
            this.btnHome.Margin = new System.Windows.Forms.Padding(0);
            this.btnHome.Size = new System.Drawing.Size(167, 145);
            this.btnHome.TabIndex = 146;
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(674, 882);
            this.btnPrev.TabIndex = 2;
            this.btnPrev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseDown);
            this.btnPrev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseUp);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(990, 882);
            this.btnCancel.TabIndex = 145;
            //
            // btnProceed
            // 
            this.btnProceed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnProceed.BackColor = System.Drawing.Color.Transparent;
            this.btnProceed.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnProceed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProceed.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnProceed.FlatAppearance.BorderSize = 0;
            this.btnProceed.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProceed.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.Location = new System.Drawing.Point(1159, 882);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(250, 125);
            this.btnProceed.TabIndex = 1075;
            this.btnProceed.Tag = "applyDiscount";
            this.btnProceed.Text = "Checkout";
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // flpCardProducts
            // 
            this.flpCardProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpCardProducts.BackColor = System.Drawing.Color.Transparent;
            this.flpCardProducts.Controls.Add(this.btnSampleName);
            this.flpCardProducts.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpCardProducts.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.flpCardProducts.Location = new System.Drawing.Point(246, 235);
            this.flpCardProducts.Margin = new System.Windows.Forms.Padding(20);
            this.flpCardProducts.Name = "flpCardProducts";
            this.flpCardProducts.Size = new System.Drawing.Size(1390, 575);
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
            this.btnSampleName.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSampleName.ForeColor = System.Drawing.Color.White;
            this.btnSampleName.Location = new System.Drawing.Point(6, 6);
            this.btnSampleName.Margin = new System.Windows.Forms.Padding(6);
            this.btnSampleName.Name = "btnSampleName";
            this.btnSampleName.Size = new System.Drawing.Size(314, 242);
            this.btnSampleName.TabIndex = 1;
            this.btnSampleName.Text = "Sample Product";
            this.btnSampleName.UseVisualStyleBackColor = false;
            this.btnSampleName.Visible = false;
            // 
            // lblGreeting1
            // 
            this.lblGreeting1.AutoEllipsis = true;
            this.lblGreeting1.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting1.Font = new System.Drawing.Font("Gotham Rounded Bold", 39F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.White;
            this.lblGreeting1.Location = new System.Drawing.Point(194, 28);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(1525, 136);
            this.lblGreeting1.TabIndex = 132;
            this.lblGreeting1.Text = "Choose Product\r\n";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnVariable
            // 
            this.btnVariable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVariable.BackColor = System.Drawing.Color.Transparent;
            this.btnVariable.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnVariable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVariable.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnVariable.FlatAppearance.BorderSize = 0;
            this.btnVariable.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnVariable.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnVariable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVariable.Font = new System.Drawing.Font("Gotham Rounded Bold", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVariable.ForeColor = System.Drawing.Color.White;
            this.btnVariable.Location = new System.Drawing.Point(728, 882);
            this.btnVariable.Name = "btnVariable";
            this.btnVariable.Size = new System.Drawing.Size(250, 125);
            this.btnVariable.TabIndex = 144;
            this.btnVariable.Text = "Other Amount";
            this.btnVariable.UseVisualStyleBackColor = false;
            this.btnVariable.Visible = false;
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
            this.txtMessage.Location = new System.Drawing.Point(0, 1011);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1920, 50);
            this.txtMessage.TabIndex = 149;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // btnScrollLeft
            // 
            this.btnScrollLeft.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnScrollLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnScrollLeft.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.left_arrow;
            this.btnScrollLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnScrollLeft.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnScrollLeft.FlatAppearance.BorderSize = 0;
            this.btnScrollLeft.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnScrollLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnScrollLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnScrollLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScrollLeft.Location = new System.Drawing.Point(152, 447);
            this.btnScrollLeft.Margin = new System.Windows.Forms.Padding(0);
            this.btnScrollLeft.Name = "btnScrollLeft";
            this.btnScrollLeft.Size = new System.Drawing.Size(80, 80);
            this.btnScrollLeft.TabIndex = 20015;
            this.btnScrollLeft.UseVisualStyleBackColor = false;
            this.btnScrollLeft.Visible = false;
            this.btnScrollLeft.Click += new System.EventHandler(this.btnScrollLeft_Click);
            // 
            // btnScrollRight
            // 
            this.btnScrollRight.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnScrollRight.BackColor = System.Drawing.Color.Transparent;
            this.btnScrollRight.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.right_arrow;
            this.btnScrollRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnScrollRight.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnScrollRight.FlatAppearance.BorderSize = 0;
            this.btnScrollRight.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnScrollRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnScrollRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnScrollRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScrollRight.Location = new System.Drawing.Point(1685, 447);
            this.btnScrollRight.Margin = new System.Windows.Forms.Padding(0);
            this.btnScrollRight.Name = "btnScrollRight";
            this.btnScrollRight.Size = new System.Drawing.Size(80, 80);
            this.btnScrollRight.TabIndex = 20016;
            this.btnScrollRight.UseVisualStyleBackColor = false;
            this.btnScrollRight.Visible = false;
            this.btnScrollRight.Click += new System.EventHandler(this.btnScrollRight_Click);
            // 
            // frmChooseProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.btnScrollLeft);
            this.Controls.Add(this.btnScrollRight);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnVariable);
            this.Controls.Add(this.flpCardProducts);
            this.Controls.Add(this.lblGreeting1);
			this.Controls.Add(this.btnProceed);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Gotham Rounded Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmChooseProduct";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmChooseProduct";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmChooseProduct_Load);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.lblGreeting1, 0);
            this.Controls.SetChildIndex(this.flpCardProducts, 0);
            this.Controls.SetChildIndex(this.btnVariable, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.btnScrollRight, 0);
            this.Controls.SetChildIndex(this.btnScrollLeft, 0);
            this.Controls.SetChildIndex(this.btnProceed, 0);
            this.flpCardProducts.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpCardProducts;
        //private System.Windows.Forms.Button btnPrev;
        internal System.Windows.Forms.Label lblGreeting1;
        private System.Windows.Forms.Timer inactivityTimer;
        private System.Windows.Forms.Button btnSampleName;
        private System.Windows.Forms.Button btnVariable;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Button btnScrollLeft;
        private System.Windows.Forms.Button btnScrollRight;
        private System.Windows.Forms.Button btnProceed;
    }
}
