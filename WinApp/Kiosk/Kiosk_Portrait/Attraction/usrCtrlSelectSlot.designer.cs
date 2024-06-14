using System;

namespace Parafait_Kiosk
{
    partial class usrCtrlSlot 
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
            this.pbxSelectd = new System.Windows.Forms.PictureBox();
            this.lblScheduleTime = new System.Windows.Forms.Label();
            this.lblSlotInfo = new System.Windows.Forms.Label();
            this.usrControlPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectd)).BeginInit();
            this.usrControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbxSelectd
            // 
            this.pbxSelectd.BackColor = System.Drawing.Color.Transparent;
            this.pbxSelectd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxSelectd.Image = global::Parafait_Kiosk.Properties.Resources.selected_tick;
            this.pbxSelectd.Location = new System.Drawing.Point(597, 30);
            this.pbxSelectd.Name = "pbxSelectd";
            this.pbxSelectd.Size = new System.Drawing.Size(68, 65);
            this.pbxSelectd.TabIndex = 4;
            this.pbxSelectd.TabStop = false;
            this.pbxSelectd.Visible = false;
            this.pbxSelectd.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // lblScheduleTime
            // 
            this.lblScheduleTime.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScheduleTime.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblScheduleTime.Location = new System.Drawing.Point(19, 0);
            this.lblScheduleTime.Name = "lblScheduleTime";
            this.lblScheduleTime.Size = new System.Drawing.Size(572, 61);
            this.lblScheduleTime.TabIndex = 7;
            this.lblScheduleTime.Text = "11:00 AM - 11:30 AM";
            this.lblScheduleTime.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblScheduleTime.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // lblSlotInfo
            // 
            this.lblSlotInfo.Font = new System.Drawing.Font("Gotham Rounded Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSlotInfo.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblSlotInfo.Location = new System.Drawing.Point(27, 61);
            this.lblSlotInfo.Name = "lblSlotInfo";
            this.lblSlotInfo.Size = new System.Drawing.Size(564, 65);
            this.lblSlotInfo.TabIndex = 5;
            this.lblSlotInfo.Text = "Morning slot 1, 00 available";
            this.lblSlotInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSlotInfo.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // usrControlPanel
            // 
            this.usrControlPanel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Slot;
            this.usrControlPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.usrControlPanel.Controls.Add(this.pbxSelectd);
            this.usrControlPanel.Controls.Add(this.lblScheduleTime);
            this.usrControlPanel.Controls.Add(this.lblSlotInfo);
            this.usrControlPanel.Location = new System.Drawing.Point(0, 0);
            this.usrControlPanel.Name = "usrControlPanel";
            this.usrControlPanel.Size = new System.Drawing.Size(700, 126);
            this.usrControlPanel.TabIndex = 5;
            this.usrControlPanel.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // usrCtrlSlot
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.usrControlPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.Name = "usrCtrlSlot";
            this.Size = new System.Drawing.Size(700, 126);
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectd)).EndInit();
            this.usrControlPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pbxSelectd;
        private System.Windows.Forms.Label lblSlotInfo;
        private System.Windows.Forms.Label lblScheduleTime;
        private System.Windows.Forms.Panel usrControlPanel;
    }
}
