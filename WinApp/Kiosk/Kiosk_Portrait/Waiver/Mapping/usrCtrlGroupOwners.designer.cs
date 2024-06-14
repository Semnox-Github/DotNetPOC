using System;

namespace Parafait_Kiosk
{
    partial class UsrCtrlGroupOwners
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
            this.lblCustomer = new System.Windows.Forms.Label();
            this.pbxSelectd = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectd)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCustomer
            // 
            this.lblCustomer.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblCustomer.Location = new System.Drawing.Point(14, 0);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(708, 102);
            this.lblCustomer.TabIndex = 7;
            this.lblCustomer.Text = "Sathyavathi Saligrama";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomer.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // pbxSelectd
            // 
            this.pbxSelectd.BackColor = System.Drawing.Color.Transparent;
            this.pbxSelectd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxSelectd.Image = global::Parafait_Kiosk.Properties.Resources.NewUnTickedCheckBox;
            this.pbxSelectd.Location = new System.Drawing.Point(728, 21);
            this.pbxSelectd.Name = "pbxSelectd";
            this.pbxSelectd.Size = new System.Drawing.Size(50, 50);
            this.pbxSelectd.TabIndex = 10;
            this.pbxSelectd.TabStop = false;
            this.pbxSelectd.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // UsrCtrlGroupOwners
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Slot;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.pbxSelectd);
            this.Controls.Add(this.lblCustomer);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.Name = "UsrCtrlGroupOwners";
            this.Size = new System.Drawing.Size(805, 102);
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.PictureBox pbxSelectd;
    }
}
