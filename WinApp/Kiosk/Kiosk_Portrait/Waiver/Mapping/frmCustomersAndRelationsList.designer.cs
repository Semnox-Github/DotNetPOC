namespace Parafait_Kiosk
{
    partial class frmCustomersAndRelationsList
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
            this.lblGreetingMsg = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            this.flpUsrCtrls = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCustomers = new System.Windows.Forms.Panel();
            this.bigVerticalScrollView = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnSearchAnotherCustomer = new System.Windows.Forms.Button();
            this.btnProceed = new System.Windows.Forms.Button();
            this.btnAddNewMember = new System.Windows.Forms.Button();
            this.pnlCustomers.SuspendLayout();
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
            this.btnPrev.Location = new System.Drawing.Point(26, 1670);
            this.btnPrev.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(745, 872);
            // 
            // lblGreetingMsg
            // 
            this.lblGreetingMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreetingMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblGreetingMsg.FlatAppearance.BorderSize = 0;
            this.lblGreetingMsg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblGreetingMsg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblGreetingMsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreetingMsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreetingMsg.ForeColor = System.Drawing.Color.White;
            this.lblGreetingMsg.Location = new System.Drawing.Point(0, 167);
            this.lblGreetingMsg.Name = "lblGreetingMsg";
            this.lblGreetingMsg.Size = new System.Drawing.Size(1078, 210);
            this.lblGreetingMsg.TabIndex = 136;
            this.lblGreetingMsg.Text = "Selecting Participant for ProductName";
            this.lblGreetingMsg.UseVisualStyleBackColor = false;
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
            this.txtMessage.Size = new System.Drawing.Size(1084, 50);
            this.txtMessage.TabIndex = 147;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // flpUsrCtrls
            // 
            this.flpUsrCtrls.AutoScroll = true;
            this.flpUsrCtrls.BackColor = System.Drawing.Color.Transparent;
            this.flpUsrCtrls.Location = new System.Drawing.Point(35, 53);
            this.flpUsrCtrls.Name = "flpUsrCtrls";
            this.flpUsrCtrls.Size = new System.Drawing.Size(891, 967);
            this.flpUsrCtrls.TabIndex = 20020;
            // 
            // pnlCustomers
            // 
            this.pnlCustomers.BackColor = System.Drawing.Color.Transparent;
            this.pnlCustomers.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.TimePanelBackground;
            this.pnlCustomers.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlCustomers.Controls.Add(this.bigVerticalScrollView);
            this.pnlCustomers.Controls.Add(this.flpUsrCtrls);
            this.pnlCustomers.Location = new System.Drawing.Point(44, 395);
            this.pnlCustomers.Name = "pnlCustomers";
            this.pnlCustomers.Size = new System.Drawing.Size(999, 1054);
            this.pnlCustomers.TabIndex = 20026;
            // 
            // bigVerticalScrollView
            // 
            this.bigVerticalScrollView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bigVerticalScrollView.AutoHide = false;
            this.bigVerticalScrollView.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollView.DataGridView = null;
            this.bigVerticalScrollView.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollView.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollView.Location = new System.Drawing.Point(903, 53);
            this.bigVerticalScrollView.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollView.Name = "bigVerticalScrollView";
            this.bigVerticalScrollView.ScrollableControl = this.flpUsrCtrls;
            this.bigVerticalScrollView.ScrollViewer = null;
            this.bigVerticalScrollView.Size = new System.Drawing.Size(60, 967);
            this.bigVerticalScrollView.TabIndex = 20019;
            this.bigVerticalScrollView.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollView.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollView.UpButtonClick += new System.EventHandler(this.bigVerticalScrollView_ButtonClick);
            this.bigVerticalScrollView.DownButtonClick += new System.EventHandler(this.bigVerticalScrollView_ButtonClick);
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.btnSearchAnotherCustomer);
            this.panelButtons.Controls.Add(this.btnProceed);
            this.panelButtons.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(26, 1670);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1108, 164);
            this.panelButtons.TabIndex = 20030;
            // 
            // btnSearchAnotherCustomer
            // 
            this.btnSearchAnotherCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchAnotherCustomer.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnSearchAnotherCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearchAnotherCustomer.FlatAppearance.BorderSize = 0;
            this.btnSearchAnotherCustomer.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSearchAnotherCustomer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSearchAnotherCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSearchAnotherCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchAnotherCustomer.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F);
            this.btnSearchAnotherCustomer.ForeColor = System.Drawing.Color.White;
            this.btnSearchAnotherCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearchAnotherCustomer.Location = new System.Drawing.Point(355, 0);
            this.btnSearchAnotherCustomer.Name = "btnSearchAnotherCustomer";
            this.btnSearchAnotherCustomer.Size = new System.Drawing.Size(325, 164);
            this.btnSearchAnotherCustomer.TabIndex = 1027;
            this.btnSearchAnotherCustomer.Text = "Search Customer";
            this.btnSearchAnotherCustomer.UseVisualStyleBackColor = false;
            this.btnSearchAnotherCustomer.Click += new System.EventHandler(this.btnSearchAnotherCustomer_Click);
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
            this.btnProceed.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F);
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProceed.Location = new System.Drawing.Point(710, 0);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(325, 164);
            this.btnProceed.TabIndex = 1025;
            this.btnProceed.Text = "Proceed";
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // btnAddNewMember
            // 
            this.btnAddNewMember.BackColor = System.Drawing.Color.Transparent;
            this.btnAddNewMember.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnAddNewMember.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddNewMember.FlatAppearance.BorderSize = 0;
            this.btnAddNewMember.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAddNewMember.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddNewMember.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddNewMember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNewMember.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F);
            this.btnAddNewMember.ForeColor = System.Drawing.Color.White;
            this.btnAddNewMember.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddNewMember.Location = new System.Drawing.Point(717, 1455);
            this.btnAddNewMember.Name = "btnAddNewMember";
            this.btnAddNewMember.Size = new System.Drawing.Size(325, 164);
            this.btnAddNewMember.TabIndex = 1026;
            this.btnAddNewMember.Text = "Add New Member";
            this.btnAddNewMember.UseVisualStyleBackColor = false;
            this.btnAddNewMember.Click += new System.EventHandler(this.BtnAddNewMember_Click);
            // 
            // frmCustomersAndRelationsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1084, 1901);
            this.Controls.Add(this.btnAddNewMember);
            this.Controls.Add(this.pnlCustomers);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblGreetingMsg);
            this.Controls.Add(this.panelButtons);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Bango Pro", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmCustomersAndRelationsList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmChooseProduct";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCustomersAndRelationsList_FormClosed);
            this.Load += new System.EventHandler(this.frmCustomersAndRelationsList_Load);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.lblGreetingMsg, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.pnlCustomers, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnAddNewMember, 0);
            this.pnlCustomers.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button lblGreetingMsg;
        private System.Windows.Forms.Button txtMessage;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollView;
        private System.Windows.Forms.Panel pnlCustomers;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnProceed;
        private System.Windows.Forms.FlowLayoutPanel flpUsrCtrls;
        private System.Windows.Forms.Button btnAddNewMember;
        private System.Windows.Forms.Button btnSearchAnotherCustomer;
    }
}