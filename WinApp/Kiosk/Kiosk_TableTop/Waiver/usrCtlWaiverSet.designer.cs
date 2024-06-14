using System.Drawing;

namespace Parafait_Kiosk
{
    partial class usrCtlWaiverSet
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
            this.lblWaiverSetName = new System.Windows.Forms.Label();
            this.tPnlWaiverSet = new System.Windows.Forms.TableLayoutPanel();
            this.tPnlWaivers = new System.Windows.Forms.TableLayoutPanel();
            this.btnSignWaiverSet = new System.Windows.Forms.Button();
            this.tPnlWaiverSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWaiverSetName
            // 
            this.lblWaiverSetName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWaiverSetName.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaiverSetName.Location = new System.Drawing.Point(3, 0);
            this.lblWaiverSetName.MaximumSize = new System.Drawing.Size(350, 0);
            this.lblWaiverSetName.MinimumSize = new System.Drawing.Size(350, 43);
            this.lblWaiverSetName.Name = "lblWaiverSetName";
            this.lblWaiverSetName.Size = new System.Drawing.Size(350, 43);
            this.lblWaiverSetName.TabIndex = 2;
            this.lblWaiverSetName.Text = "label1";
            this.lblWaiverSetName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tPnlWaiverSet
            // 
            this.tPnlWaiverSet.ColumnCount = 4;
            this.tPnlWaiverSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 365F));
            this.tPnlWaiverSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 632F));
            this.tPnlWaiverSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121F));
            this.tPnlWaiverSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tPnlWaiverSet.Controls.Add(this.lblWaiverSetName, 0, 0);
            this.tPnlWaiverSet.Controls.Add(this.tPnlWaivers, 1, 0);
            this.tPnlWaiverSet.Controls.Add(this.btnSignWaiverSet, 2, 0);
            this.tPnlWaiverSet.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.tPnlWaiverSet.Location = new System.Drawing.Point(3, 3);
            this.tPnlWaiverSet.Name = "tPnlWaiverSet";
            this.tPnlWaiverSet.RowCount = 1;
            this.tPnlWaiverSet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 680F));
            this.tPnlWaiverSet.Size = new System.Drawing.Size(1268, 137);
            this.tPnlWaiverSet.TabIndex = 3;
            // 
            // tPnlWaivers
            // 
            this.tPnlWaivers.ColumnCount = 3;
            this.tPnlWaivers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tPnlWaivers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tPnlWaivers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tPnlWaivers.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.tPnlWaivers.Location = new System.Drawing.Point(368, 3);
            this.tPnlWaivers.MaximumSize = new System.Drawing.Size(609, 0);
            this.tPnlWaivers.MinimumSize = new System.Drawing.Size(609, 50);
            this.tPnlWaivers.Name = "tPnlWaivers";
            this.tPnlWaivers.RowCount = 1;
            this.tPnlWaivers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tPnlWaivers.Size = new System.Drawing.Size(609, 50);
            this.tPnlWaivers.TabIndex = 3;
            // 
            // btnSignWaiverSet
            // 
            this.btnSignWaiverSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSignWaiverSet.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.WaiverButtonBorder;
            this.btnSignWaiverSet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSignWaiverSet.FlatAppearance.BorderSize = 0;
            this.btnSignWaiverSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignWaiverSet.Font = new System.Drawing.Font("Gotham Rounded Bold", 14F);
            this.btnSignWaiverSet.ForeColor = System.Drawing.Color.White;
            this.btnSignWaiverSet.Image = global::Parafait_Kiosk.Properties.Resources.SignWaiver_Btn;
            this.btnSignWaiverSet.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSignWaiverSet.Location = new System.Drawing.Point(1000, 3);
            this.btnSignWaiverSet.Name = "btnSignWaiverSet";
            this.btnSignWaiverSet.Size = new System.Drawing.Size(115, 130);
            this.btnSignWaiverSet.TabIndex = 0;
            this.btnSignWaiverSet.Text = "Sign";
            this.btnSignWaiverSet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSignWaiverSet.UseVisualStyleBackColor = false;
            this.btnSignWaiverSet.Click += new System.EventHandler(this.btnSignWaiverSet_Click);
            // 
            // usrCtlWaiverSet
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tPnlWaiverSet);
            this.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F);
            this.Name = "usrCtlWaiverSet";
            this.Size = new System.Drawing.Size(1185, 141);
            this.tPnlWaiverSet.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSignWaiverSet;
        private System.Windows.Forms.Label lblWaiverSetName;
        private System.Windows.Forms.TableLayoutPanel tPnlWaiverSet; 
        private System.Windows.Forms.TableLayoutPanel tPnlWaivers;
        //private Color btnColor;
    }
}
