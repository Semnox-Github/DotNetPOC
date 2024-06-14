using System;

namespace Semnox.Parafait.Device.PaymentGateway
{
    partial class frmKioskStatusUI
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
            this.statusUITimer = new System.Windows.Forms.Timer(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCheckNow = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCardCharged = new System.Windows.Forms.Label();
            this.pnlbackGroundPanel = new System.Windows.Forms.Panel();
            this.pBxbackGroundPicureBox = new System.Windows.Forms.PictureBox();
            this.lblTimeOut = new System.Windows.Forms.Button();
            this.pnlbackGroundPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBxbackGroundPicureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // statusUITimer
            // 
            this.statusUITimer.Interval = 1000;
            this.statusUITimer.Tick += new System.EventHandler(this.StatusUITimer_Tick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Semnox.Parafait.Device.Properties.Resources.CancelButton;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.DarkOrchid;
            this.btnCancel.Location = new System.Drawing.Point(592, 1238);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(441, 109);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);


            this.btnCheckNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheckNow.BackColor = System.Drawing.Color.Transparent;
            this.btnCheckNow.BackgroundImage = global::Semnox.Parafait.Device.Properties.Resources.CancelButton;
            this.btnCheckNow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCheckNow.FlatAppearance.BorderSize = 0;
            this.btnCheckNow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCheckNow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCheckNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckNow.ForeColor = System.Drawing.Color.DarkOrchid;
            this.btnCheckNow.Location = new System.Drawing.Point(348, 805);
            this.btnCheckNow.Name = "btnCheckNow";
            this.btnCheckNow.Size = new System.Drawing.Size(294, 71);
            this.btnCheckNow.TabIndex = 0;
            this.btnCheckNow.Text = "Check Now";
            this.btnCheckNow.Visible = false;
            this.btnCheckNow.UseVisualStyleBackColor = false;
            this.btnCheckNow.Click += new System.EventHandler(this.btnCheckNow_Click);
            // 



            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(92, 665);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(858, 518);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(3, 37);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1080, 194);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.AutoEllipsis = true;
            // 
            // lblCardCharged
            // 
            this.lblCardCharged.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCardCharged.BackColor = System.Drawing.Color.Transparent;
            this.lblCardCharged.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardCharged.ForeColor = System.Drawing.Color.Yellow;
            this.lblCardCharged.Location = new System.Drawing.Point(18, 535);
            this.lblCardCharged.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCardCharged.Name = "lblCardCharged";
            this.lblCardCharged.Size = new System.Drawing.Size(966, 106);
            this.lblCardCharged.TabIndex = 3;
            this.lblCardCharged.Text = "Card will be charged: $10";
            this.lblCardCharged.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlbackGroundPanel
            // 
            this.pnlbackGroundPanel.BackColor = System.Drawing.Color.Transparent;
            this.pnlbackGroundPanel.Controls.Add(this.pBxbackGroundPicureBox);
            this.pnlbackGroundPanel.Location = new System.Drawing.Point(0, 0);
            this.pnlbackGroundPanel.Margin = new System.Windows.Forms.Padding(0, 46, 0, 0);
            this.pnlbackGroundPanel.Name = "pnlbackGroundPanel";
            this.pnlbackGroundPanel.Size = new System.Drawing.Size(1482, 1418);
            this.pnlbackGroundPanel.TabIndex = 0;
            // 
            // pBxbackGroundPicureBox
            // 
            this.pBxbackGroundPicureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pBxbackGroundPicureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pBxbackGroundPicureBox.Image = global::Semnox.Parafait.Device.Properties.Resources.PaymentScreen;
            this.pBxbackGroundPicureBox.Location = new System.Drawing.Point(0, 0);
            this.pBxbackGroundPicureBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pBxbackGroundPicureBox.Name = "pBxbackGroundPicureBox";
            this.pBxbackGroundPicureBox.Size = new System.Drawing.Size(1479, 1415);
            this.pBxbackGroundPicureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBxbackGroundPicureBox.TabIndex = 0;
            this.pBxbackGroundPicureBox.TabStop = false;
            // 
            // lblTimeOut
            // 
            this.lblTimeOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeOut.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.BackgroundImage = global::Semnox.Parafait.Device.Properties.Resources.timer_SmallBox;
            this.lblTimeOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lblTimeOut.FlatAppearance.BorderSize = 0;
            //this.lblTimeOut.FlatAppearance.BorderColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeOut.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOut.ForeColor = System.Drawing.Color.White;
            this.lblTimeOut.Location = new System.Drawing.Point(891, 24);
            this.lblTimeOut.Margin = new System.Windows.Forms.Padding(2);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(197, 137);
            this.lblTimeOut.TabIndex = 141;
            this.lblTimeOut.UseVisualStyleBackColor = false;
            this.lblTimeOut.Visible = false;
            // 
            // frmKioskStatusUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImage = global::Semnox.Parafait.Device.Properties.Resources.PaymentScreen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1485, 1422);
            this.ControlBox = false;
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblCardCharged);
            this.Controls.Add(this.lblTimeOut);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlbackGroundPanel);
            this.DoubleBuffered = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmKioskStatusUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmKioskStatusUI";
            this.TransparencyKey = System.Drawing.Color.PaleGreen;
            this.Load += new System.EventHandler(this.frmKioskStatusUI_Load);
            this.pnlbackGroundPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pBxbackGroundPicureBox)).EndInit();
            this.ResumeLayout(false);

        }        

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCheckNow;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCardCharged;
        internal System.Windows.Forms.PictureBox pBxbackGroundPicureBox;
        internal System.Windows.Forms.Panel pnlbackGroundPanel;
        private System.Windows.Forms.Button lblTimeOut;
        private System.Windows.Forms.Timer statusUITimer;
    }
}