using System;

namespace Parafait_Kiosk
{
    partial class UsrCtrlCustomersAndRelationsList
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
            this.lblRelatedCustomerName = new System.Windows.Forms.Label();
            this.lblRelationship = new System.Windows.Forms.Label();
            this.lblSignStatus = new System.Windows.Forms.Label();
            this.pBInfo = new System.Windows.Forms.PictureBox();
            this.lblValidity = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxSelectd
            // 
            this.pbxSelectd.BackColor = System.Drawing.Color.Transparent;
            this.pbxSelectd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxSelectd.Image = global::Parafait_Kiosk.Properties.Resources.NewUnTickedCheckBox;
            this.pbxSelectd.Location = new System.Drawing.Point(12, 40);
            this.pbxSelectd.Name = "pbxSelectd";
            this.pbxSelectd.Size = new System.Drawing.Size(50, 50);
            this.pbxSelectd.TabIndex = 4;
            this.pbxSelectd.TabStop = false;
            this.pbxSelectd.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // lblRelatedCustomerName
            // 
            this.lblRelatedCustomerName.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRelatedCustomerName.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblRelatedCustomerName.Location = new System.Drawing.Point(68, 0);
            this.lblRelatedCustomerName.Name = "lblRelatedCustomerName";
            this.lblRelatedCustomerName.Size = new System.Drawing.Size(464, 85);
            this.lblRelatedCustomerName.TabIndex = 7;
            this.lblRelatedCustomerName.Text = "Sathyavathi Saligrama";
            this.lblRelatedCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblRelatedCustomerName.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // lblRelationship
            // 
            this.lblRelationship.Font = new System.Drawing.Font("Gotham Rounded Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRelationship.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblRelationship.Location = new System.Drawing.Point(75, 89);
            this.lblRelationship.Name = "lblRelationship";
            this.lblRelationship.Size = new System.Drawing.Size(504, 34);
            this.lblRelationship.TabIndex = 5;
            this.lblRelationship.Text = "You";
            this.lblRelationship.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // lblSignStatus
            // 
            this.lblSignStatus.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignStatus.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblSignStatus.Location = new System.Drawing.Point(579, 0);
            this.lblSignStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblSignStatus.Name = "lblSignStatus";
            this.lblSignStatus.Size = new System.Drawing.Size(171, 85);
            this.lblSignStatus.TabIndex = 8;
            this.lblSignStatus.Text = "PENDING";
            this.lblSignStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSignStatus.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // pBInfo
            // 
            this.pBInfo.BackColor = System.Drawing.Color.Transparent;
            this.pBInfo.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.ComboInformationIcon;
            this.pBInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pBInfo.Location = new System.Drawing.Point(538, 28);
            this.pBInfo.Name = "pBInfo";
            this.pBInfo.Size = new System.Drawing.Size(41, 32);
            this.pBInfo.TabIndex = 9;
            this.pBInfo.TabStop = false;
            this.pBInfo.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // lblValidity
            // 
            this.lblValidity.Font = new System.Drawing.Font("Gotham Rounded Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValidity.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblValidity.Location = new System.Drawing.Point(585, 90);
            this.lblValidity.Name = "lblValidity";
            this.lblValidity.Size = new System.Drawing.Size(165, 34);
            this.lblValidity.TabIndex = 10;
            this.lblValidity.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblValidity.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // UsrCtrlCustomerRelations
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Slot;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.lblRelationship);
            this.Controls.Add(this.lblRelatedCustomerName);
            this.Controls.Add(this.pbxSelectd);
            this.Controls.Add(this.lblValidity);
            this.Controls.Add(this.pBInfo);
            this.Controls.Add(this.lblSignStatus);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(30, 0, 0, 10);
            this.Name = "UsrCtrlCustomerRelations";
            this.Size = new System.Drawing.Size(772, 126);
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pbxSelectd;
        private System.Windows.Forms.Label lblRelationship;
        private System.Windows.Forms.Label lblRelatedCustomerName;
        private System.Windows.Forms.PictureBox pBInfo;
        private System.Windows.Forms.Label lblSignStatus;
        private System.Windows.Forms.Label lblValidity;
    }
}
