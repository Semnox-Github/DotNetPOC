namespace Parafait_Kiosk
{
    partial class frmSignWaivers
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
            this.txtMessage = new System.Windows.Forms.Button();
            this.lblGreeting1 = new System.Windows.Forms.Label();
            this.lblSelection = new System.Windows.Forms.Label();
            this.lblWaiver = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bigVerticalScrollWaiverSet = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
            this.fpnlWaiverSet = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
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
            this.btnHome.Margin = new System.Windows.Forms.Padding(0);
            this.btnHome.TabIndex = 146;
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(1242, 1354);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(1242, 1354);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.btnCancel.TabIndex = 1028;
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
            this.txtMessage.Location = new System.Drawing.Point(0, 1571);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(2880, 75);
            this.txtMessage.TabIndex = 3;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // lblGreeting1
            // 
            this.lblGreeting1.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting1.Font = new System.Drawing.Font("Gotham Rounded Bold", 41.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.White;
            this.lblGreeting1.Location = new System.Drawing.Point(280, 38);
            this.lblGreeting1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(2298, 122);
            this.lblGreeting1.TabIndex = 132;
            this.lblGreeting1.Text = "Sign Waivers";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSelection
            // 
            this.lblSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblSelection.Font = new System.Drawing.Font("Gotham Rounded Bold", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelection.ForeColor = System.Drawing.Color.Thistle;
            this.lblSelection.Location = new System.Drawing.Point(698, 0);
            this.lblSelection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(543, 95);
            this.lblSelection.TabIndex = 164;
            this.lblSelection.Text = "Waiver Name";
            this.lblSelection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWaiver
            // 
            this.lblWaiver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWaiver.BackColor = System.Drawing.Color.Transparent;
            this.lblWaiver.Font = new System.Drawing.Font("Gotham Rounded Bold", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaiver.ForeColor = System.Drawing.Color.Thistle;
            this.lblWaiver.Location = new System.Drawing.Point(80, 0);
            this.lblWaiver.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWaiver.Name = "lblWaiver";
            this.lblWaiver.Size = new System.Drawing.Size(592, 95);
            this.lblWaiver.TabIndex = 163;
            this.lblWaiver.Text = "Waiver Set";
            this.lblWaiver.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Table1;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.bigVerticalScrollWaiverSet);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblSelection);
            this.panel1.Controls.Add(this.lblWaiver);
            this.panel1.Controls.Add(this.fpnlWaiverSet);
            this.panel1.Location = new System.Drawing.Point(460, 331);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2006, 946);
            this.panel1.TabIndex = 163;
            // 
            // bigVerticalScrollWaiverSet
            // 
            this.bigVerticalScrollWaiverSet.AutoHide = true;
            this.bigVerticalScrollWaiverSet.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollWaiverSet.DataGridView = null;
            this.bigVerticalScrollWaiverSet.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollWaiverSet.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollWaiverSet.Location = new System.Drawing.Point(1827, 97);
            this.bigVerticalScrollWaiverSet.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollWaiverSet.Name = "bigVerticalScrollWaiverSet";
            this.bigVerticalScrollWaiverSet.ScrollableControl = this.fpnlWaiverSet;
            this.bigVerticalScrollWaiverSet.ScrollViewer = null;
            this.bigVerticalScrollWaiverSet.Size = new System.Drawing.Size(100, 765);
            this.bigVerticalScrollWaiverSet.TabIndex = 166;
            this.bigVerticalScrollWaiverSet.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollWaiverSet.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            this.bigVerticalScrollWaiverSet.UpButtonClick += new System.EventHandler(this.UpButtonClick);
            this.bigVerticalScrollWaiverSet.DownButtonClick += new System.EventHandler(this.DownButtonClick);
            // 
            // fpnlWaiverSet
            // 
            this.fpnlWaiverSet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpnlWaiverSet.AutoScroll = true;
            this.fpnlWaiverSet.BackColor = this.panel1.BackColor;
            this.fpnlWaiverSet.Location = new System.Drawing.Point(80, 97);
            this.fpnlWaiverSet.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.fpnlWaiverSet.Name = "fpnlWaiverSet";
            this.fpnlWaiverSet.Size = new System.Drawing.Size(1818, 765);
            this.fpnlWaiverSet.TabIndex = 163;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Gotham Rounded Bold", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Thistle;
            this.label1.Location = new System.Drawing.Point(1425, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(592, 95);
            this.label1.TabIndex = 165;
            this.label1.Text = "Validity (Days)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomer
            // 
            this.lblCustomer.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomer.Font = new System.Drawing.Font("Gotham Rounded Bold", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.ForeColor = System.Drawing.Color.White;
            this.lblCustomer.Location = new System.Drawing.Point(480, 200);
            this.lblCustomer.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(2260, 95);
            this.lblCustomer.TabIndex = 1029;
            this.lblCustomer.Text = "Customer : ";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmSignWaivers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(2880, 1646);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblGreeting1);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(9, 12, 9, 12);
            this.Name = "frmSignWaivers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sign Waivers";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSignWaivers_Load);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.lblGreeting1, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblCustomer, 0);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion 
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Label lblGreeting1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel fpnlWaiverSet;
        private System.Windows.Forms.Label lblSelection;
        private System.Windows.Forms.Label lblWaiver;
        private System.Windows.Forms.Label label1;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollWaiverSet;
        //private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCustomer;
    }
}