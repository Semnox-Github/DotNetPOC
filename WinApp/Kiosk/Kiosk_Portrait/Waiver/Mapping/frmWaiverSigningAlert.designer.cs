using System;

namespace Parafait_Kiosk
{
    partial class frmWaiverSigningAlert
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
            this.lblGreeting = new System.Windows.Forms.Button();
            this.fLPProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnStartSigning = new System.Windows.Forms.Button();
            this.pnlProducts = new System.Windows.Forms.Panel();
            this.panelButtons.SuspendLayout();
            this.pnlProducts.SuspendLayout();
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
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(26, 1670);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(373, 1670);
            // 
            // btnCart
            // 
            this.btnCart.Location = new System.Drawing.Point(828, 28);
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
            this.lblGreeting.Location = new System.Drawing.Point(17, 154);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(1050, 92);
            this.lblGreeting.TabIndex = 12;
            this.lblGreeting.Text = "Waiver Signing Required";
            this.lblGreeting.UseVisualStyleBackColor = false;
            // 
            // fLPProducts
            // 
            this.fLPProducts.AutoScroll = true;
            this.fLPProducts.BackColor = System.Drawing.Color.Transparent;
            this.fLPProducts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.fLPProducts.Location = new System.Drawing.Point(35, 42);
            this.fLPProducts.Name = "fLPProducts";
            this.fLPProducts.Size = new System.Drawing.Size(913, 1040);
            this.fLPProducts.TabIndex = 20020;
            // 
            // lblMsg
            // 
            this.lblMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblMsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.White;
            this.lblMsg.Location = new System.Drawing.Point(18, 257);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(1050, 160);
            this.lblMsg.TabIndex = 20023;
            this.lblMsg.Text = "The items below in your cart require a waiver to be signed by each participant";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMessage
            // 
            this.txtMessage.AutoEllipsis = true;
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1870);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 50);
            this.txtMessage.TabIndex = 20025;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.btnStartSigning);
            this.panelButtons.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(26, 1670);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1028, 164);
            this.panelButtons.TabIndex = 20026;
            // 
            // btnStartSigning
            // 
            this.btnStartSigning.BackColor = System.Drawing.Color.Transparent;
            this.btnStartSigning.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnStartSigning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStartSigning.FlatAppearance.BorderSize = 0;
            this.btnStartSigning.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnStartSigning.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStartSigning.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStartSigning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartSigning.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F);
            this.btnStartSigning.ForeColor = System.Drawing.Color.White;
            this.btnStartSigning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStartSigning.Location = new System.Drawing.Point(694, 0);
            this.btnStartSigning.Name = "btnStartSigning";
            this.btnStartSigning.Size = new System.Drawing.Size(325, 164);
            this.btnStartSigning.TabIndex = 1025;
            this.btnStartSigning.Text = "Start Signing";
            this.btnStartSigning.UseVisualStyleBackColor = false;
            this.btnStartSigning.Click += new System.EventHandler(this.btnStartSigning_Click);
            // 
            // pnlProducts
            // 
            this.pnlProducts.BackColor = System.Drawing.Color.Transparent;
            this.pnlProducts.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.TimePanelBackground;
            this.pnlProducts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlProducts.Controls.Add(this.fLPProducts);
            this.pnlProducts.Location = new System.Drawing.Point(54, 450);
            this.pnlProducts.Name = "pnlProducts";
            this.pnlProducts.Size = new System.Drawing.Size(973, 1089);
            this.pnlProducts.TabIndex = 20027;
            // 
            // frmWaiverSigningAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pnlProducts);
            this.Controls.Add(this.lblGreeting);
            this.Controls.Add(this.panelButtons);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmWaiverSigningAlert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPreSelectPaymnet";
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.Load += new System.EventHandler(this.frmWaiverSigningAlert_Load);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.lblGreeting, 0);
            this.Controls.SetChildIndex(this.pnlProducts, 0);
            this.Controls.SetChildIndex(this.lblMsg, 0);
            this.Controls.SetChildIndex(this.txtMessage, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCart, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.panelButtons.ResumeLayout(false);
            this.pnlProducts.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button lblGreeting;
        private System.Windows.Forms.FlowLayoutPanel fLPProducts;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button txtMessage;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnStartSigning;
        private System.Windows.Forms.Panel pnlProducts;
        //private System.Windows.Forms.Button btnHome;
    }
}
