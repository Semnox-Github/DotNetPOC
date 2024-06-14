using System;

namespace Parafait_Kiosk
{
    partial class UsrCtrlCustomer 
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
            this.pnlExpandCollapse = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblCustomer
            // 
            this.lblCustomer.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.ForeColor = System.Drawing.Color.DarkOrchid;
            this.lblCustomer.Location = new System.Drawing.Point(11, 0);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(726, 86);
            this.lblCustomer.TabIndex = 7;
            this.lblCustomer.Text = "Sathyavathi Saligrama";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCustomer.Click += new System.EventHandler(this.pnlExpandCollapse_Click);
            // 
            // pnlExpandCollapse
            // 
            this.pnlExpandCollapse.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Collapse;
            this.pnlExpandCollapse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlExpandCollapse.Location = new System.Drawing.Point(757, 10);
            this.pnlExpandCollapse.Name = "pnlExpandCollapse";
            this.pnlExpandCollapse.Size = new System.Drawing.Size(67, 68);
            this.pnlExpandCollapse.TabIndex = 9;
            this.pnlExpandCollapse.Click += new System.EventHandler(this.pnlExpandCollapse_Click);
            // 
            // UsrCtrlCustomer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Slot;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.pnlExpandCollapse);
            this.Controls.Add(this.lblCustomer);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.Name = "UsrCtrlCustomer";
            this.Size = new System.Drawing.Size(840, 90);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Panel pnlExpandCollapse;
    }
}
