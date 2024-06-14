using System;

namespace Parafait_Kiosk
{
    partial class frmPreSelectPaymentMode
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.lblGreeting = new System.Windows.Forms.Button();
            this.fLPPaymentModes = new System.Windows.Forms.FlowLayoutPanel();
            this.bigVerticalScrollPaymentModes = new Semnox.Core.GenericUtilities.BigVerticalScrollBarView();
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
            this.btnHome.Location = new System.Drawing.Point(22, 26);
            this.btnHome.Margin = new System.Windows.Forms.Padding(0);
            this.btnHome.TabIndex = 143;
            this.btnHome.Visible = false;
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
            this.btnCart.Location = new System.Drawing.Point(828, 28);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClose.Location = new System.Drawing.Point(307, 527);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(365, 135);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblGreeting
            // 
            this.lblGreeting.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblGreeting.FlatAppearance.BorderSize = 0;
            this.lblGreeting.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblGreeting.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblGreeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting.ForeColor = System.Drawing.Color.White;
            this.lblGreeting.Location = new System.Drawing.Point(17, 25);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(964, 158);
            this.lblGreeting.TabIndex = 12;
            this.lblGreeting.Text = "Select Payment Option";
            this.lblGreeting.UseVisualStyleBackColor = false;
            // 
            // fLPPaymentModes
            // 
            this.fLPPaymentModes.AutoScroll = true;
            this.fLPPaymentModes.BackColor = System.Drawing.Color.Transparent;
            this.fLPPaymentModes.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.fLPPaymentModes.Location = new System.Drawing.Point(92, 212);
            this.fLPPaymentModes.Name = "fLPPaymentModes";
            this.fLPPaymentModes.Size = new System.Drawing.Size(860, 280);
            this.fLPPaymentModes.TabIndex = 20020;
            // 
            // bigVerticalScrollPaymentModes
            // 
            this.bigVerticalScrollPaymentModes.AutoHide = true;
            this.bigVerticalScrollPaymentModes.BackColor = System.Drawing.SystemColors.Control;
            this.bigVerticalScrollPaymentModes.DataGridView = null;
            this.bigVerticalScrollPaymentModes.DownButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button;
            this.bigVerticalScrollPaymentModes.DownButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Down_Button_Disabled;
            this.bigVerticalScrollPaymentModes.Location = new System.Drawing.Point(905, 212);
            this.bigVerticalScrollPaymentModes.Margin = new System.Windows.Forms.Padding(0);
            this.bigVerticalScrollPaymentModes.Name = "bigVerticalScrollPaymentModes";
            this.bigVerticalScrollPaymentModes.ScrollableControl = this.fLPPaymentModes;
            this.bigVerticalScrollPaymentModes.ScrollViewer = null;
            this.bigVerticalScrollPaymentModes.Size = new System.Drawing.Size(76, 280);
            this.bigVerticalScrollPaymentModes.DownButtonClick += new System.EventHandler(ScrollBtnClick);
            this.bigVerticalScrollPaymentModes.UpButtonClick += new System.EventHandler(ScrollBtnClick);
            this.bigVerticalScrollPaymentModes.TabIndex = 20021;
            this.bigVerticalScrollPaymentModes.UpButtonBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button;
            this.bigVerticalScrollPaymentModes.UpButtonDisabledBackgroundImage = global::Parafait_Kiosk.Properties.Resources.Scroll_Up_Button_Disabled;
            // 
            // frmPreSelectPaymentMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = false;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.PreSelectPaymentBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1001, 697);
            this.Controls.Add(this.bigVerticalScrollPaymentModes);
            this.Controls.Add(this.fLPPaymentModes);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblGreeting);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPreSelectPaymentMode";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPreSelectPaymnet";
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.Load += new System.EventHandler(this.frmPreSelectPaymentMode_Load);
            this.Controls.SetChildIndex(this.lblGreeting, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.fLPPaymentModes, 0);
            this.Controls.SetChildIndex(this.bigVerticalScrollPaymentModes, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button lblGreeting;
        private System.Windows.Forms.FlowLayoutPanel fLPPaymentModes;
        private Semnox.Core.GenericUtilities.BigVerticalScrollBarView bigVerticalScrollPaymentModes;
        //private System.Windows.Forms.Button btnHome;
    }
}
