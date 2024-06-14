using System;

namespace Parafait_Kiosk
{
    partial class usrCtrlSelectCustomer
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
            this.lblCustomerFirstName = new System.Windows.Forms.Label();
            this.usrControlPanel = new System.Windows.Forms.Panel();
            this.lblCustomerLastName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectd)).BeginInit();
            this.usrControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbxSelectd
            // 
            this.pbxSelectd.BackColor = System.Drawing.Color.Transparent;
            this.pbxSelectd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxSelectd.Image = global::Parafait_Kiosk.Properties.Resources.selected_tick;
            this.pbxSelectd.Location = new System.Drawing.Point(513, 30);
            this.pbxSelectd.Name = "pbxSelectd";
            this.pbxSelectd.Size = new System.Drawing.Size(68, 65);
            this.pbxSelectd.TabIndex = 4;
            this.pbxSelectd.TabStop = false;
            this.pbxSelectd.Visible = false;
            this.pbxSelectd.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // lblCustomerFirstName
            // 
            this.lblCustomerFirstName.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerFirstName.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblCustomerFirstName.Location = new System.Drawing.Point(19, 32);
            this.lblCustomerFirstName.Name = "lblCustomerFirstName";
            this.lblCustomerFirstName.Size = new System.Drawing.Size(230, 61);
            this.lblCustomerFirstName.TabIndex = 7;
            this.lblCustomerFirstName.Text = "Sath**** ";
            this.lblCustomerFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomerFirstName.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // usrControlPanel
            // 
            this.usrControlPanel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Slot;
            this.usrControlPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.usrControlPanel.Controls.Add(this.lblCustomerLastName);
            this.usrControlPanel.Controls.Add(this.pbxSelectd);
            this.usrControlPanel.Controls.Add(this.lblCustomerFirstName);
            this.usrControlPanel.Location = new System.Drawing.Point(0, 0);
            this.usrControlPanel.Name = "usrControlPanel";
            this.usrControlPanel.Size = new System.Drawing.Size(603, 126);
            this.usrControlPanel.TabIndex = 5;
            this.usrControlPanel.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // lblCustomerLastName
            // 
            this.lblCustomerLastName.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerLastName.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblCustomerLastName.Location = new System.Drawing.Point(259, 32);
            this.lblCustomerLastName.Name = "lblCustomerLastName";
            this.lblCustomerLastName.Size = new System.Drawing.Size(230, 61);
            this.lblCustomerLastName.TabIndex = 8;
            this.lblCustomerLastName.Text = "****ngh";
            this.lblCustomerLastName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomerLastName.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // usrCtrlSelectCustomer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.usrControlPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.Name = "usrCtrlSelectCustomer";
            this.Size = new System.Drawing.Size(610, 126);
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectd)).EndInit();
            this.usrControlPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pbxSelectd;
        private System.Windows.Forms.Label lblCustomerFirstName;
        private System.Windows.Forms.Panel usrControlPanel;
        private System.Windows.Forms.Label lblCustomerLastName;
    }
}
