using System;

namespace Parafait_Kiosk
{
    partial class usrControlFundsDonationsProduct
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
            this.usrControlPanel = new System.Windows.Forms.Panel();
            this.pbxSelectd = new System.Windows.Forms.PictureBox();
            this.btnSampleProduct = new System.Windows.Forms.Button();
            this.usrControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectd)).BeginInit();
            this.SuspendLayout();
            // 
            // usrControlPanel
            // 
            this.usrControlPanel.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Button1;
            this.usrControlPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.usrControlPanel.Controls.Add(this.pbxSelectd);
            this.usrControlPanel.Controls.Add(this.btnSampleProduct);
            this.usrControlPanel.Location = new System.Drawing.Point(152, 0);
            this.usrControlPanel.Name = "usrControlPanel";
            this.usrControlPanel.Size = new System.Drawing.Size(672, 202);
            this.usrControlPanel.TabIndex = 5;
            this.usrControlPanel.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // pbxSelectd
            // 
            this.pbxSelectd.BackColor = System.Drawing.Color.Transparent;
            this.pbxSelectd.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.selected_tick;
            this.pbxSelectd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbxSelectd.Location = new System.Drawing.Point(35, 68);
            this.pbxSelectd.Name = "pbxSelectd";
            this.pbxSelectd.Size = new System.Drawing.Size(68, 65);
            this.pbxSelectd.TabIndex = 4;
            this.pbxSelectd.TabStop = false;
            this.pbxSelectd.Visible = false;
            this.pbxSelectd.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // btnSampleProduct
            // 
            this.btnSampleProduct.BackColor = System.Drawing.Color.Transparent;
            this.btnSampleProduct.FlatAppearance.BorderSize = 0;
            this.btnSampleProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSampleProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSampleProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSampleProduct.Font = new System.Drawing.Font("Bango Pro", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSampleProduct.ForeColor = System.Drawing.Color.DarkOrchid;
            this.btnSampleProduct.Location = new System.Drawing.Point(0, 4);
            this.btnSampleProduct.Margin = new System.Windows.Forms.Padding(6);
            this.btnSampleProduct.MinimumSize = new System.Drawing.Size(250, 28);
            this.btnSampleProduct.Name = "btnSampleProduct";
            this.btnSampleProduct.Size = new System.Drawing.Size(672, 195);
            this.btnSampleProduct.TabIndex = 1;
            this.btnSampleProduct.UseVisualStyleBackColor = false;
            this.btnSampleProduct.Click += new System.EventHandler(this.usrControl_Click);
            // 
            // usrControlFundsDonationsProduct
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.usrControlPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "usrControlFundsDonationsProduct";
            this.Size = new System.Drawing.Size(983, 205);
            this.usrControlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxSelectd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pbxSelectd;
        private System.Windows.Forms.Button btnSampleProduct;
        private System.Windows.Forms.Panel usrControlPanel;
    }
}
