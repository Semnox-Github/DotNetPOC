using System;

namespace Parafait_Kiosk
{
    partial class UsrCtrlWaiverAlert
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnlUsrCtrl = new System.Windows.Forms.Panel();
            this.lblParticipants = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.pbxWaiverIcon = new System.Windows.Forms.PictureBox();
            this.pnlUsrCtrl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxWaiverIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // pnlUsrCtrl
            // 
            this.pnlUsrCtrl.BackColor = System.Drawing.Color.Transparent;
            this.pnlUsrCtrl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlUsrCtrl.Controls.Add(this.lblParticipants);
            this.pnlUsrCtrl.Controls.Add(this.lblProductName);
            this.pnlUsrCtrl.Location = new System.Drawing.Point(102, 3);
            this.pnlUsrCtrl.Name = "pnlUsrCtrl";
            this.pnlUsrCtrl.Size = new System.Drawing.Size(795, 98);
            this.pnlUsrCtrl.TabIndex = 20035;
            // 
            // lblParticipants
            // 
            this.lblParticipants.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParticipants.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblParticipants.Location = new System.Drawing.Point(3, 55);
            this.lblParticipants.Name = "lblParticipants";
            this.lblParticipants.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblParticipants.Size = new System.Drawing.Size(789, 34);
            this.lblParticipants.TabIndex = 5;
            this.lblParticipants.Text = "0 participants";
            this.lblParticipants.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblProductName
            // 
            this.lblProductName.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblProductName.Location = new System.Drawing.Point(3, 8);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(789, 45);
            this.lblProductName.TabIndex = 3;
            this.lblProductName.Text = "Product Name";
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbxWaiverIcon
            // 
            this.pbxWaiverIcon.BackColor = System.Drawing.Color.Transparent;
            this.pbxWaiverIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxWaiverIcon.Image = global::Parafait_Kiosk.Properties.Resources.WaiverIcon;
            this.pbxWaiverIcon.Location = new System.Drawing.Point(22, 17);
            this.pbxWaiverIcon.Name = "pbxWaiverIcon";
            this.pbxWaiverIcon.Size = new System.Drawing.Size(61, 68);
            this.pbxWaiverIcon.TabIndex = 6;
            this.pbxWaiverIcon.TabStop = false;
            //this.pbxWaiverIcon.Visible = false;
            // 
            // UsrCtrlWaiverAlert
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.WaiverAlertBackgroundImage;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.pnlUsrCtrl);
            this.Controls.Add(this.pbxWaiverIcon);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(1,1,1,8);
            this.Name = "UsrCtrlWaiverAlert";
            this.Size = new System.Drawing.Size(900, 104);
            this.pnlUsrCtrl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxWaiverIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label lblParticipants;
        private System.Windows.Forms.Panel pnlUsrCtrl;
        private System.Windows.Forms.PictureBox pbxWaiverIcon;
    }
}
