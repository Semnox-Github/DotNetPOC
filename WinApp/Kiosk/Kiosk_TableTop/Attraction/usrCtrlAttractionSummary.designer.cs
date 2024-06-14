using System;

namespace Parafait_Kiosk
{
    partial class UsrCtrlAttractionSummary
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
            this.usrControlPanel = new System.Windows.Forms.Panel();
            this.pnlUsrCtrl = new System.Windows.Forms.Panel();
            this.lblSlotDetails = new System.Windows.Forms.Label();
            this.pbxSelectd = new System.Windows.Forms.PictureBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.usrControlPanel.SuspendLayout();
            this.pnlUsrCtrl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectd)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // usrControlPanel
            // 
            this.usrControlPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.usrControlPanel.Controls.Add(this.pnlUsrCtrl);
            this.usrControlPanel.Location = new System.Drawing.Point(2, 0);
            this.usrControlPanel.Name = "usrControlPanel";
            this.usrControlPanel.Size = new System.Drawing.Size(780, 126);
            this.usrControlPanel.TabIndex = 5;
            // 
            // pnlUsrCtrl
            // 
            this.pnlUsrCtrl.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.ComboProductBackground;
            this.pnlUsrCtrl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlUsrCtrl.Controls.Add(this.lblSlotDetails);
            this.pnlUsrCtrl.Controls.Add(this.pbxSelectd);
            this.pnlUsrCtrl.Controls.Add(this.lblProductName);
            this.pnlUsrCtrl.Location = new System.Drawing.Point(0, 0);
            this.pnlUsrCtrl.Name = "pnlUsrCtrl";
            this.pnlUsrCtrl.Size = new System.Drawing.Size(780, 126);
            this.pnlUsrCtrl.TabIndex = 20035;
            // 
            // lblSlotDetails
            // 
            this.lblSlotDetails.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSlotDetails.Location = new System.Drawing.Point(4, 76);
            this.lblSlotDetails.Name = "lblSlotDetails";
            this.lblSlotDetails.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblSlotDetails.Size = new System.Drawing.Size(496, 47);
            this.lblSlotDetails.TabIndex = 5;
            this.lblSlotDetails.Text = "Slot Details";
            this.lblSlotDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbxSelectd
            // 
            this.pbxSelectd.BackColor = System.Drawing.Color.Transparent;
            this.pbxSelectd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxSelectd.Image = global::Parafait_Kiosk.Properties.Resources.selected_tick;
            this.pbxSelectd.Location = new System.Drawing.Point(683, 32);
            this.pbxSelectd.Name = "pbxSelectd";
            this.pbxSelectd.Size = new System.Drawing.Size(68, 65);
            this.pbxSelectd.TabIndex = 6;
            this.pbxSelectd.TabStop = false;
            // 
            // lblProductName
            // 
            this.lblProductName.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName.Location = new System.Drawing.Point(3, 4);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(497, 68);
            this.lblProductName.TabIndex = 3;
            this.lblProductName.Text = "Product Name";
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UsrCtrlAttractionSummary
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.usrControlPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 21);
            this.Name = "UsrCtrlAttractionSummary";
            this.Size = new System.Drawing.Size(1183, 1140);
            this.usrControlPanel.ResumeLayout(false);
            this.pnlUsrCtrl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel usrControlPanel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label lblSlotDetails;
        private System.Windows.Forms.PictureBox pbxSelectd;
        private System.Windows.Forms.Panel pnlUsrCtrl;
    }
}
