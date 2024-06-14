namespace Parafait_Kiosk
{
    partial class frmMapAttendees
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
            this.lblMsg = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblAssignParticipants = new System.Windows.Forms.Label();
            this.bigVerticalScrollView = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.flpUsrCtrls = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlMapAttendees = new System.Windows.Forms.Panel();
            this.lblProductName = new System.Windows.Forms.Label();
            this.flpProductIndex = new System.Windows.Forms.FlowLayoutPanel();
            this.flpScreenDetails = new System.Windows.Forms.FlowLayoutPanel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnComplete = new System.Windows.Forms.Button();
            this.pnlMapAttendees.SuspendLayout();
            this.flpScreenDetails.SuspendLayout();
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
            this.btnCancel.Location = new System.Drawing.Point(380, 1670);
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
            this.lblGreetingMsg.Size = new System.Drawing.Size(1080, 138);
            this.lblGreetingMsg.TabIndex = 136;
            this.lblGreetingMsg.Text = "Assign Participants for your 1st waiver product";
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
            // lblMsg
            // 
            this.lblMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblMsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.White;
            this.lblMsg.Location = new System.Drawing.Point(3, 129);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(989, 43);
            this.lblMsg.TabIndex = 132;
            this.lblMsg.Text = "Assign participant for each quantity of the product";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoEllipsis = true;
            this.lblQuantity.BackColor = System.Drawing.Color.Transparent;
            this.lblQuantity.Font = new System.Drawing.Font("Gotham Rounded Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantity.ForeColor = System.Drawing.Color.White;
            this.lblQuantity.Location = new System.Drawing.Point(35, 34);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(193, 51);
            this.lblQuantity.TabIndex = 20021;
            this.lblQuantity.Text = "Quantity";
            this.lblQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblQuantity.Visible = false;
            // 
            // lblAssignParticipants
            // 
            this.lblAssignParticipants.BackColor = System.Drawing.Color.Transparent;
            this.lblAssignParticipants.Font = new System.Drawing.Font("Gotham Rounded Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAssignParticipants.ForeColor = System.Drawing.Color.White;
            this.lblAssignParticipants.Location = new System.Drawing.Point(232, 34);
            this.lblAssignParticipants.Name = "lblAssignParticipants";
            this.lblAssignParticipants.Size = new System.Drawing.Size(667, 51);
            this.lblAssignParticipants.TabIndex = 20022;
            this.lblAssignParticipants.Text = "Assign Participants";
            this.lblAssignParticipants.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAssignParticipants.Visible = false;
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
            this.bigVerticalScrollView.Location = new System.Drawing.Point(1351, 297);
            this.bigVerticalScrollView.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollView.Name = "bigVerticalScrollView";
            this.bigVerticalScrollView.ScrollableControl = this.flpUsrCtrls;
            this.bigVerticalScrollView.ScrollViewer = null;
            this.bigVerticalScrollView.Size = new System.Drawing.Size(60, 1355);
            this.bigVerticalScrollView.TabIndex = 20019;
            this.bigVerticalScrollView.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollView.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollView.UpButtonClick += new System.EventHandler(this.bigVerticalScrollView_ButtonClick);
            this.bigVerticalScrollView.DownButtonClick += new System.EventHandler(this.bigVerticalScrollView_ButtonClick);
            // 
            // flpUsrCtrls
            // 
            this.flpUsrCtrls.AutoScroll = true;
            this.flpUsrCtrls.BackColor = System.Drawing.Color.Transparent;
            this.flpUsrCtrls.Location = new System.Drawing.Point(44, 101);
            this.flpUsrCtrls.Name = "flpUsrCtrls";
            this.flpUsrCtrls.Size = new System.Drawing.Size(875, 933);
            this.flpUsrCtrls.TabIndex = 20006;
            // 
            // pnlMapAttendees
            // 
            this.pnlMapAttendees.BackColor = System.Drawing.Color.Transparent;
            this.pnlMapAttendees.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.TimePanelBackground;
            this.pnlMapAttendees.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlMapAttendees.Controls.Add(this.lblQuantity);
            this.pnlMapAttendees.Controls.Add(this.lblAssignParticipants);
            this.pnlMapAttendees.Controls.Add(this.flpUsrCtrls);
            this.pnlMapAttendees.Location = new System.Drawing.Point(54, 522);
            this.pnlMapAttendees.Name = "pnlMapAttendees";
            this.pnlMapAttendees.Size = new System.Drawing.Size(989, 1068);
            this.pnlMapAttendees.TabIndex = 20026;
            // 
            // lblProductName
            // 
            this.lblProductName.BackColor = System.Drawing.Color.Transparent;
            this.lblProductName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblProductName.Font = new System.Drawing.Font("Gotham Rounded Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName.ForeColor = System.Drawing.Color.White;
            this.lblProductName.Location = new System.Drawing.Point(0, 76);
            this.lblProductName.Margin = new System.Windows.Forms.Padding(0);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(991, 53);
            this.lblProductName.TabIndex = 20027;
            this.lblProductName.Text = " Product Name";
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // flpProductIndex
            // 
            this.flpProductIndex.BackColor = System.Drawing.Color.Transparent;
            this.flpProductIndex.Location = new System.Drawing.Point(3, 3);
            this.flpProductIndex.Name = "flpProductIndex";
            this.flpProductIndex.Size = new System.Drawing.Size(988, 70);
            this.flpProductIndex.TabIndex = 20028;
            // 
            // flpScreenDetails
            // 
            this.flpScreenDetails.BackColor = System.Drawing.Color.Transparent;
            this.flpScreenDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flpScreenDetails.Controls.Add(this.flpProductIndex);
            this.flpScreenDetails.Controls.Add(this.lblProductName);
            this.flpScreenDetails.Controls.Add(this.lblMsg);
            this.flpScreenDetails.Location = new System.Drawing.Point(54, 311);
            this.flpScreenDetails.Name = "flpScreenDetails";
            this.flpScreenDetails.Size = new System.Drawing.Size(992, 177);
            this.flpScreenDetails.TabIndex = 20029;
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.btnComplete);
            this.panelButtons.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(26, 1670);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1038, 164);
            this.panelButtons.TabIndex = 20030;
            // 
            // btnComplete
            // 
            this.btnComplete.BackColor = System.Drawing.Color.Transparent;
            this.btnComplete.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnComplete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnComplete.FlatAppearance.BorderSize = 0;
            this.btnComplete.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnComplete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnComplete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnComplete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComplete.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F);
            this.btnComplete.ForeColor = System.Drawing.Color.White;
            this.btnComplete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnComplete.Location = new System.Drawing.Point(708, 0);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(325, 164);
            this.btnComplete.TabIndex = 1025;
            this.btnComplete.Text = "Complete";
            this.btnComplete.UseVisualStyleBackColor = false;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // frmMapAttendees
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1084, 1901);
            this.Controls.Add(this.bigVerticalScrollView);
            this.Controls.Add(this.flpScreenDetails);
            this.Controls.Add(this.pnlMapAttendees);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblGreetingMsg);
            this.Controls.Add(this.panelButtons);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Bango Pro", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmMapAttendees";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMapAttendees";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMapAttendees_FormClosed);
            this.Load += new System.EventHandler(this.frmMapAttendees_Load);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.lblGreetingMsg, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.pnlMapAttendees, 0);
            this.Controls.SetChildIndex(this.flpScreenDetails, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.bigVerticalScrollView, 0);
            this.pnlMapAttendees.ResumeLayout(false);
            this.flpScreenDetails.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button lblGreetingMsg;
        private System.Windows.Forms.Button txtMessage;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollView;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblAssignParticipants;
        internal System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Panel pnlMapAttendees;
        private System.Windows.Forms.FlowLayoutPanel flpUsrCtrls;
        internal System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.FlowLayoutPanel flpProductIndex;
        private System.Windows.Forms.FlowLayoutPanel flpScreenDetails;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnComplete;
    }
}