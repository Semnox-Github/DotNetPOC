using System;

namespace Parafait_Kiosk
{
    partial class UsrCtrlComboDetails
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
            this.lblChildPackageInfo = new System.Windows.Forms.Label();
            this.pnlNum = new System.Windows.Forms.Panel();
            this.lblNum = new System.Windows.Forms.Label();
            this.usrControlPanel.SuspendLayout();
            this.pnlNum.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // usrControlPanel
            // 
            this.usrControlPanel.BackColor = System.Drawing.Color.Transparent;
            this.usrControlPanel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.ComboProductBackground;
            this.usrControlPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.usrControlPanel.Controls.Add(this.lblChildPackageInfo);
            this.usrControlPanel.Controls.Add(this.pnlNum);
            this.usrControlPanel.Location = new System.Drawing.Point(2, 0);
            this.usrControlPanel.Name = "usrControlPanel";
            this.usrControlPanel.Size = new System.Drawing.Size(810, 93);
            this.usrControlPanel.TabIndex = 5;
            // 
            // lblChildPackageInfo
            // 
            this.lblChildPackageInfo.AutoEllipsis = true;
            this.lblChildPackageInfo.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChildPackageInfo.ForeColor = System.Drawing.Color.White;
            this.lblChildPackageInfo.Location = new System.Drawing.Point(70, 29);
            this.lblChildPackageInfo.Name = "lblChildPackageInfo";
            this.lblChildPackageInfo.Size = new System.Drawing.Size(735, 40);
            this.lblChildPackageInfo.TabIndex = 3;
            this.lblChildPackageInfo.Text = "Child Package Info";
            this.lblChildPackageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlNum
            // 
            this.pnlNum.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlNum.Controls.Add(this.lblNum);
            this.pnlNum.Location = new System.Drawing.Point(8, 22);
            this.pnlNum.Name = "pnlNum";
            this.pnlNum.Size = new System.Drawing.Size(60, 46);
            this.pnlNum.TabIndex = 4;
            // 
            // lblNum
            // 
            this.lblNum.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblNum.Location = new System.Drawing.Point(1, 1);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(59, 45);
            this.lblNum.TabIndex = 0;
            this.lblNum.Text = "1";
            this.lblNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UsrCtrlComboDetails
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.usrControlPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(1, 1, 1, 5);
            this.Name = "UsrCtrlComboDetails";
            this.Size = new System.Drawing.Size(1183, 1140);
            this.usrControlPanel.ResumeLayout(false);
            this.pnlNum.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel usrControlPanel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label lblChildPackageInfo;
        private System.Windows.Forms.Panel pnlNum;
        private System.Windows.Forms.Label lblNum;
    }
}
