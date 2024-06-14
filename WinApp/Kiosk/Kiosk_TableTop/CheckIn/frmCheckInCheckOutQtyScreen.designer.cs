namespace Parafait_Kiosk
{
    partial class frmCheckInCheckOutQtyScreen
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
            this.lblGreeting = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.Button();
            this.flpComboProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.bigVerticalScrollComboProducts = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.btnProceed = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(465, 882);
            this.btnPrev.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(834, 882);
            // 
            // lblGreeting
            // 
            this.lblGreeting.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting.ForeColor = System.Drawing.Color.White;
            this.lblGreeting.Location = new System.Drawing.Point(195, 9);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(1529, 122);
            this.lblGreeting.TabIndex = 132;
            this.lblGreeting.Text = "Please Select the Quantity for Each Product \r\n\r\n\r\n";
            this.lblGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.txtMessage.TabIndex = 147;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // flpComboProducts
            // 
            this.flpComboProducts.AutoScroll = true;
            this.flpComboProducts.BackColor = System.Drawing.Color.Transparent;
            this.flpComboProducts.Location = new System.Drawing.Point(356, 173);
            this.flpComboProducts.Name = "flpComboProducts";
            this.flpComboProducts.Size = new System.Drawing.Size(1208, 620);
            this.flpComboProducts.TabIndex = 1077;
            // 
            // bigVerticalScrollComboProducts
            // 
            this.bigVerticalScrollComboProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bigVerticalScrollComboProducts.AutoHide = true;
            this.bigVerticalScrollComboProducts.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollComboProducts.DataGridView = null;
            this.bigVerticalScrollComboProducts.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollComboProducts.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollComboProducts.Location = new System.Drawing.Point(1510, 171);
            this.bigVerticalScrollComboProducts.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.bigVerticalScrollComboProducts.Name = "bigVerticalScrollComboProducts";
            this.bigVerticalScrollComboProducts.ScrollableControl = this.flpComboProducts;
            this.bigVerticalScrollComboProducts.ScrollViewer = null;
            this.bigVerticalScrollComboProducts.Size = new System.Drawing.Size(63, 622);
            this.bigVerticalScrollComboProducts.TabIndex = 20020;
            this.bigVerticalScrollComboProducts.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollComboProducts.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollComboProducts.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollComboProducts.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // btnProceed
            // 
            this.btnProceed.BackColor = System.Drawing.Color.Transparent;
            this.btnProceed.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnProceed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProceed.FlatAppearance.BorderSize = 0;
            this.btnProceed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnProceed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProceed.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F);
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProceed.Location = new System.Drawing.Point(1203, 882);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(250, 125);
            this.btnProceed.TabIndex = 1078;
            this.btnProceed.Text = "Proceed";
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(365, 825);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1241, 180);
            this.panelButtons.TabIndex = 20005;
            // 
            // frmCheckInCheckOutQtyScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.bigVerticalScrollComboProducts);
            this.Controls.Add(this.btnProceed);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.lblGreeting);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.flpComboProducts);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Bango Pro", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmCheckInCheckOutQtyScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmChooseProduct";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCheckInCheckOutQtyScreen_FormClosed);
            this.Load += new System.EventHandler(this.frmCheckInCheckOutQtyScreen_Load);
            this.Controls.SetChildIndex(this.flpComboProducts, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.lblGreeting, 0);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnProceed, 0);
            this.Controls.SetChildIndex(this.bigVerticalScrollComboProducts, 0);
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.Label lblGreeting;
        private System.Windows.Forms.Button txtMessage;
        //private System.Windows.Forms.TableLayoutPanel tlpComboProducts;
        private System.Windows.Forms.FlowLayoutPanel flpComboProducts;
        private System.Windows.Forms.Button btnProceed;
        private System.Windows.Forms.Panel panelButtons;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollComboProducts;
    }
}