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
            //this.btnPrev = new System.Windows.Forms.Button();
            this.lblSiteName = new System.Windows.Forms.Button();
            //this.inactivityTimer = new System.Windows.Forms.Timer(this.components);
            this.btnVariable = new System.Windows.Forms.Button();
            //this.btnCancel = new System.Windows.Forms.Button();
            this.panelProducts = new System.Windows.Forms.Panel();
            this.vScrollBarProducts = new System.Windows.Forms.VScrollBar();
            this.txtMessage = new System.Windows.Forms.Button();
            this.flpCardProducts.SuspendLayout();
            this.panelProducts.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpCardProducts
            // 
            this.flpCardProducts.AutoSize = true;
            this.flpCardProducts.BackColor = System.Drawing.Color.Transparent;
            this.flpCardProducts.Controls.Add(this.btnSampleName);
            this.flpCardProducts.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpCardProducts.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.flpCardProducts.Location = new System.Drawing.Point(0, 0);
            this.flpCardProducts.Name = "flpCardProducts";
            this.flpCardProducts.Size = new System.Drawing.Size(1013, 1237);
            this.flpCardProducts.TabIndex = 1;
            // 
            // btnSampleName
            // 
            this.btnSampleName.BackColor = System.Drawing.Color.Transparent;
            this.btnSampleName.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Button1;
            this.btnSampleName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSampleName.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnSampleName.FlatAppearance.BorderSize = 0;
            this.btnSampleName.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSampleName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSampleName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSampleName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSampleName.Font = new System.Drawing.Font("Bango Pro", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSampleName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.btnSampleName.Location = new System.Drawing.Point(6, 6);
            this.btnSampleName.Margin = new System.Windows.Forms.Padding(6);
            this.btnSampleName.Name = "btnSampleName";
            this.btnSampleName.Size = new System.Drawing.Size(598, 192);
            this.btnSampleName.TabIndex = 1;
            this.btnSampleName.Text = "Sample Product";
            this.btnSampleName.UseVisualStyleBackColor = false;
            this.btnSampleName.Visible = false;
            // 
            // lblGreeting1
            // 
            this.lblGreeting1.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting1.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.White;
            this.lblGreeting1.Location = new System.Drawing.Point(137, 228);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(791, 168);
            this.lblGreeting1.TabIndex = 132;
            this.lblGreeting1.Text = "HOW MANY POINTS WOULD YOU LIKE?";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            //this.btnPrev.BackColor = System.Drawing.Color.Transparent;
            //this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            //this.btnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            //this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            //this.btnPrev.FlatAppearance.BorderSize = 0;
            //this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            //this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            //this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.btnPrev.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.btnPrev.ForeColor = System.Drawing.Color.White;
            this.btnPrev.Location = new System.Drawing.Point(24, 1646);
            //this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(354, 177);
            this.btnPrev.TabIndex = 2;
            //this.btnPrev.Text = "Back";
            //this.btnPrev.UseVisualStyleBackColor = false;
            //this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            this.btnPrev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseDown);
            this.btnPrev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseUp);
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
            this.lblSiteName.Size = new System.Drawing.Size(1192, 77);
            this.lblSiteName.TabIndex = 136;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            // 
            // inactivityTimer
            // 
            //this.inactivityTimer.Enabled = true;
            //this.inactivityTimer.Interval = 1000;
            //this.inactivityTimer.Tick += new System.EventHandler(this.inactivityTimer_Tick);
            // 
            // btnVariable
            // 
            this.btnVariable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVariable.BackColor = System.Drawing.Color.Transparent;
            this.btnVariable.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnVariable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnVariable.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnVariable.FlatAppearance.BorderSize = 0;
            this.btnVariable.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnVariable.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnVariable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVariable.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVariable.ForeColor = System.Drawing.Color.White;
            this.btnVariable.Location = new System.Drawing.Point(720, 1646);
            this.btnVariable.Name = "btnVariable";
            this.btnVariable.Size = new System.Drawing.Size(354, 177);
            this.btnVariable.TabIndex = 144;
            this.btnVariable.Text = "Other Amount";
            this.btnVariable.UseVisualStyleBackColor = false;
            this.btnVariable.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            //this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            //this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            //this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            //this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            //this.btnCancel.FlatAppearance.BorderSize = 0;
            //this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            //this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            //this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.btnCancel.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(373, 1646);
            //this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(354, 177);
            this.btnCancel.TabIndex = 145;
            //this.btnCancel.Text = "Cancel";
            //this.btnCancel.UseVisualStyleBackColor = false;
            //this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // panelProducts
            // 
            this.panelProducts.BackColor = System.Drawing.Color.Transparent;
            this.panelProducts.Controls.Add(this.vScrollBarProducts);
            this.panelProducts.Controls.Add(this.flpCardProducts);
            this.panelProducts.Location = new System.Drawing.Point(0, 401);
            this.panelProducts.Name = "panelProducts";
            this.panelProducts.Size = new System.Drawing.Size(1076, 1237);
            this.panelProducts.TabIndex = 146;
            // 
            // vScrollBarProducts
            // 
            this.vScrollBarProducts.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBarProducts.LargeChange = 50;
            this.vScrollBarProducts.Location = new System.Drawing.Point(1016, 0);
            this.vScrollBarProducts.Name = "vScrollBarProducts";
            this.vScrollBarProducts.Size = new System.Drawing.Size(60, 1237);
            this.vScrollBarProducts.SmallChange = 10;
            this.vScrollBarProducts.TabIndex = 2;
            this.vScrollBarProducts.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBarProducts_Scroll);
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
            this.txtMessage.Location = new System.Drawing.Point(0, 1855);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1076, 38);
            this.txtMessage.TabIndex = 147;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // frmChooseProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1076, 1893);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.panelProducts);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnVariable);
            this.Controls.Add(this.lblGreeting1);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblSiteName);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Bango Pro", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.panelProducts.ResumeLayout(false);
            this.panelProducts.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpCardProducts;
        //private System.Windows.Forms.Button btnPrev;
        internal System.Windows.Forms.Label lblGreeting1;
        //private System.Windows.Forms.Timer inactivityTimer;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button btnSampleName;
        private System.Windows.Forms.Button btnVariable;
        //private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panelProducts;
        private System.Windows.Forms.VScrollBar vScrollBarProducts;
        private System.Windows.Forms.Button txtMessage;
    }
}