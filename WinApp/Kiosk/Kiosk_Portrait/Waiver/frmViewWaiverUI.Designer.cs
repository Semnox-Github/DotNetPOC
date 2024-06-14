using System;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    partial class frmViewWaiverUI
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
            this.lblWaiverName = new System.Windows.Forms.Label();
            this.pnlWaiverDisplay = new System.Windows.Forms.Panel();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlWaiverDisplay.SuspendLayout();
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
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // btnCart
            // 
            //this.btnCart.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            //this.btnCart.FlatAppearance.BorderSize = 0;
            //this.btnCart.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            //this.btnCart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            //this.btnCart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // lblWaiverName
            // 
            this.lblWaiverName.BackColor = System.Drawing.Color.Transparent;
            this.lblWaiverName.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaiverName.ForeColor = System.Drawing.Color.White;
            this.lblWaiverName.Location = new System.Drawing.Point(34, 198);
            this.lblWaiverName.MaximumSize = new System.Drawing.Size(1015, 0);
            this.lblWaiverName.MinimumSize = new System.Drawing.Size(1015, 86);
            this.lblWaiverName.Name = "lblWaiverName";
            this.lblWaiverName.Size = new System.Drawing.Size(1015, 86);
            this.lblWaiverName.TabIndex = 2;
            this.lblWaiverName.Text = "Waiver Name";
            // 
            // pnlWaiverDisplay
            // 
            this.pnlWaiverDisplay.AutoScroll = true;
            this.pnlWaiverDisplay.BackColor = System.Drawing.Color.White;
            this.pnlWaiverDisplay.Controls.Add(this.webBrowser);
            this.pnlWaiverDisplay.Controls.Add(this.tabControl);
            this.pnlWaiverDisplay.Location = new System.Drawing.Point(46, 290);
            this.pnlWaiverDisplay.Name = "pnlWaiverDisplay";
            this.pnlWaiverDisplay.Size = new System.Drawing.Size(986, 1205);
            this.pnlWaiverDisplay.TabIndex = 3;
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(969, 1189);
            this.webBrowser.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(200, 100);
            this.tabControl.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(377, 1670);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(325, 164);
            this.btnClose.TabIndex = 1028;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmViewWaiverUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.lblWaiverName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlWaiverDisplay);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmViewWaiverUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Waiver UI";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmViewWaiverUI_FormClosing);
            this.Load += new System.EventHandler(this.frmViewWaiverUI_Load);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.pnlWaiverDisplay, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.lblWaiverName, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.pnlWaiverDisplay.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        

        #endregion

        private System.Windows.Forms.Label lblWaiverName;
        private System.Windows.Forms.Panel pnlWaiverDisplay;
        private System.Windows.Forms.Button btnClose;
        private WebBrowser webBrowser;
        private TabControl tabControl;
    }
}