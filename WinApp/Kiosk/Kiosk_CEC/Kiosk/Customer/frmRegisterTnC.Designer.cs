namespace Parafait_Kiosk
{
    partial class frmRegisterTnC
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
            this.btnYes = new System.Windows.Forms.Button();
            this.lblmsg = new System.Windows.Forms.Button();
            this.wbTerms = new System.Windows.Forms.WebBrowser();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.panelBrowser = new System.Windows.Forms.Panel();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.panelButtons.SuspendLayout();
            this.panelBrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrev
            // 
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Size = new System.Drawing.Size(326, 127);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancel.Location = new System.Drawing.Point(456, 3);
            this.btnCancel.Size = new System.Drawing.Size(326, 127);
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnYes.BackColor = System.Drawing.Color.Transparent;
            this.btnYes.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnYes.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnYes.Location = new System.Drawing.Point(4, 3);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(326, 127);
            this.btnYes.TabIndex = 10;
            this.btnYes.Text = "Agree";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // lblmsg
            // 
            this.lblmsg.BackColor = System.Drawing.Color.Transparent;
            this.lblmsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblmsg.FlatAppearance.BorderSize = 0;
            this.lblmsg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblmsg.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmsg.ForeColor = System.Drawing.Color.White;
            this.lblmsg.Location = new System.Drawing.Point(5, 21);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(974, 183);
            this.lblmsg.TabIndex = 13;
            this.lblmsg.Text = "<<Terms and Conditions>>";
            this.lblmsg.UseVisualStyleBackColor = false;
            // 
            // wbTerms
            // 
            this.wbTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wbTerms.Location = new System.Drawing.Point(0, 0);
            this.wbTerms.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbTerms.Name = "wbTerms";
            this.wbTerms.ScrollBarsEnabled = false;
            this.wbTerms.Size = new System.Drawing.Size(212, 123);
            this.wbTerms.TabIndex = 15;
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Controls.Add(this.btnYes);
            this.panelButtons.Location = new System.Drawing.Point(99, 222);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(786, 132);
            this.panelButtons.TabIndex = 16;
            this.panelButtons.Controls.SetChildIndex(this.btnYes, 0);
            this.panelButtons.Controls.SetChildIndex(this.btnCancel, 0);
            // 
            // panelBrowser
            // 
            this.panelBrowser.BackColor = System.Drawing.Color.Transparent;
            this.panelBrowser.Controls.Add(this.vScrollBar);
            this.panelBrowser.Controls.Add(this.wbTerms);
            this.panelBrowser.Location = new System.Drawing.Point(441, 65);
            this.panelBrowser.Name = "panelBrowser";
            this.panelBrowser.Size = new System.Drawing.Size(266, 123);
            this.panelBrowser.TabIndex = 17;
            this.panelBrowser.Visible = false;
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.LargeChange = 650;
            this.vScrollBar.Location = new System.Drawing.Point(211, 0);
            this.vScrollBar.Maximum = 13000;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(55, 123);
            this.vScrollBar.SmallChange = 50;
            this.vScrollBar.TabIndex = 16;
            this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
            // 
            // frmRegisterTnC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(984, 379);
            this.Controls.Add(this.panelBrowser);
            this.Controls.Add(this.lblmsg);
            this.Controls.Add(this.panelButtons);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmRegisterTnC";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmYesNo";
            this.TransparencyKey = System.Drawing.Color.CornflowerBlue;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRegisterTnC_FormClosing);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.lblmsg, 0);
            this.Controls.SetChildIndex(this.panelBrowser, 0);
            this.panelButtons.ResumeLayout(false);
            this.panelBrowser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        
        #endregion

        private System.Windows.Forms.Button btnYes;
        //private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button lblmsg;
        //private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.WebBrowser wbTerms;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Panel panelBrowser;
        private System.Windows.Forms.VScrollBar vScrollBar;
    }
}
