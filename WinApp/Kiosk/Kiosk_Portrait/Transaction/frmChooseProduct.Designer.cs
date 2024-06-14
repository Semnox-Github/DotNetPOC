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
            this.lblSiteName = new System.Windows.Forms.Button();
            this.inactivityTimer = new System.Windows.Forms.Timer(this.components);
            this.btnVariable = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            this.bigVerticalScrollCardProducts = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
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
            this.btnPrev.Location = new System.Drawing.Point(26, 1670);
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
            this.btnCancel.Location = new System.Drawing.Point(376, 1670);
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
            this.btnProceed.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.Location = new System.Drawing.Point(725, 1670);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(325, 160);
            this.btnProceed.TabIndex = 1075;
            this.btnProceed.Tag = "applyDiscount";
            this.btnProceed.Text = "Checkout";
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);

            // 
            // flpCardProducts
            // 
            this.flpCardProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.flpCardProducts.AutoScroll = true;
            this.flpCardProducts.BackColor = System.Drawing.Color.Transparent;
            this.flpCardProducts.Controls.Add(this.btnSampleName);
            this.flpCardProducts.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.flpCardProducts.Location = new System.Drawing.Point(97, 355);
            this.flpCardProducts.Margin = new System.Windows.Forms.Padding(0);
            this.flpCardProducts.Name = "flpCardProducts";
            this.flpCardProducts.Size = new System.Drawing.Size(917, 1286);
            this.flpCardProducts.TabIndex = 1;
            // 
            // btnSampleName
            // 
            this.btnSampleName.BackColor = System.Drawing.Color.Transparent;
            this.btnSampleName.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.plain_product_button;
            this.btnSampleName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSampleName.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnSampleName.FlatAppearance.BorderSize = 0;
            this.btnSampleName.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSampleName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSampleName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSampleName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSampleName.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSampleName.ForeColor = System.Drawing.Color.White;
            this.btnSampleName.Location = new System.Drawing.Point(6, 10);
            this.btnSampleName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnSampleName.Name = "btnSampleName";
            this.btnSampleName.Size = new System.Drawing.Size(846, 181);
            this.btnSampleName.TabIndex = 1;
            this.btnSampleName.Text = "Sample Product";
            this.btnSampleName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSampleName.UseVisualStyleBackColor = false;
            this.btnSampleName.Visible = false;
            // 
            // lblGreeting1
            // 
            this.lblGreeting1.AutoEllipsis = true;
            this.lblGreeting1.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting1.Font = new System.Drawing.Font("Gotham Rounded Bold", 41.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.White;
            this.lblGreeting1.Location = new System.Drawing.Point(1, 197);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(1078, 136);
            //this.lblGreeting1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGreeting1.TabIndex = 132;
            this.lblGreeting1.Text = "Choose Product\r\n";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.lblSiteName.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(12, 10);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1068, 82);
            this.lblSiteName.TabIndex = 136;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            this.lblSiteName.Visible = false;
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
            this.btnVariable.Font = new System.Drawing.Font("Gotham Rounded Bold", 33F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVariable.ForeColor = System.Drawing.Color.White;
            this.btnVariable.Location = new System.Drawing.Point(725, 1670);
            this.btnVariable.Name = "btnVariable";
            this.btnVariable.Size = new System.Drawing.Size(325, 160);
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
            this.txtMessage.Location = new System.Drawing.Point(0, 1851);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 50);
            this.txtMessage.TabIndex = 149;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // bigVerticalScrollCardProducts
            // 
            this.bigVerticalScrollCardProducts.AutoHide = true;
            this.bigVerticalScrollCardProducts.BackColor = System.Drawing.Color.White;
            this.bigVerticalScrollCardProducts.DataGridView = null;
            this.bigVerticalScrollCardProducts.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollCardProducts.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollCardProducts.Location = new System.Drawing.Point(983, 355);
            this.bigVerticalScrollCardProducts.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollCardProducts.Name = "bigVerticalScrollCardProducts";
            this.bigVerticalScrollCardProducts.ScrollableControl = this.flpCardProducts;
            this.bigVerticalScrollCardProducts.ScrollViewer = null;
            this.bigVerticalScrollCardProducts.Size = new System.Drawing.Size(63, 1286);
            this.bigVerticalScrollCardProducts.TabIndex = 166;
            this.bigVerticalScrollCardProducts.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollCardProducts.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollCardProducts.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollCardProducts.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // frmChooseProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.bigVerticalScrollCardProducts);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnVariable);
            this.Controls.Add(this.flpCardProducts);
            this.Controls.Add(this.lblGreeting1);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.btnProceed);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Gotham Rounded Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.Font = new System.Drawing.Font("Bango Pro", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmChooseProduct";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmChooseProduct";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmChooseProduct_Load);
            this.Controls.SetChildIndex(this.lblSiteName, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.lblGreeting1, 0);
            this.Controls.SetChildIndex(this.flpCardProducts, 0);
            this.Controls.SetChildIndex(this.btnVariable, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.bigVerticalScrollCardProducts, 0);
            this.Controls.SetChildIndex(this.btnProceed, 0);
            this.flpCardProducts.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpCardProducts;
        //private System.Windows.Forms.Button btnPrev;
        internal System.Windows.Forms.Label lblGreeting1;
        private System.Windows.Forms.Timer inactivityTimer;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button btnSampleName;
        private System.Windows.Forms.Button btnVariable;
        private System.Windows.Forms.Button txtMessage;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollCardProducts;
        private System.Windows.Forms.Button btnProceed;
    }
}
