using System.Drawing;

namespace Parafait_POS.Redemption
{
    partial class frmRedemptionScreenBanner
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
            this.pnlBase = new System.Windows.Forms.Panel();
            this.fpnlUserTiles = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.pnlBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.AutoSize = true;
            this.pnlBase.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlBase.Controls.Add(this.fpnlUserTiles);
            this.pnlBase.Controls.Add(this.btnAddUser);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBase.Location = new System.Drawing.Point(0, 0);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(1268, 75);
            this.pnlBase.TabIndex = 0;
            // 
            // fpnlUserTiles
            // 
            this.fpnlUserTiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.fpnlUserTiles.AutoSize = true;
            this.fpnlUserTiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpnlUserTiles.Location = new System.Drawing.Point(12, 3);
            this.fpnlUserTiles.Name = "fpnlUserTiles";
            this.fpnlUserTiles.Size = new System.Drawing.Size(200, 65);
            this.fpnlUserTiles.TabIndex = 1;
            this.fpnlUserTiles.WrapContents = false;
            // 
            // btnAddUser
            // 
            this.btnAddUser.BackgroundImage = global::Parafait_POS.Properties.Resources.AddRedemptionUser;
            this.btnAddUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddUser.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddUser.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddUser.FlatAppearance.BorderSize = 0;
            this.btnAddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddUser.ForeColor = System.Drawing.Color.Transparent;
            this.btnAddUser.Location = new System.Drawing.Point(1146, 6);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(70, 60);
            this.btnAddUser.TabIndex = 0;
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            this.btnAddUser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAddUser_MouseDown);
            this.btnAddUser.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAddUser_MouseUp);
            // 
            // frmRedemptionScreenBanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 75);
            this.Controls.Add(this.pnlBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmRedemptionScreenBanner";
            this.Text = "Redemption";
            this.Load += new System.EventHandler(this.frmRedemptionScreenBanner_Load);
            this.pnlBase.ResumeLayout(false);
            this.pnlBase.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.FlowLayoutPanel fpnlUserTiles;
       // private System.Windows.Forms.Button btnExit;
    }
}