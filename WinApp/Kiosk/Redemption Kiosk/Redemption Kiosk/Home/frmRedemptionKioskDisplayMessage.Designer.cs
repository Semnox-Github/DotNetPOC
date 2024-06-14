namespace Redemption_Kiosk
{
        partial class frmRedemptionKioskDisplayMessage
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
            this.panelBG = new System.Windows.Forms.Panel();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.lblDisplayText1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelBG.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBG
            // 
            this.panelBG.BackColor = System.Drawing.Color.White;
            this.panelBG.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Pop_up;
            this.panelBG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBG.Controls.Add(this.btnNo);
            this.panelBG.Controls.Add(this.btnYes);
            this.panelBG.Controls.Add(this.lblDisplayText1);
            this.panelBG.Controls.Add(this.btnClose);
            this.panelBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBG.Location = new System.Drawing.Point(0, 0);
            this.panelBG.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.panelBG.Name = "panelBG";
            this.panelBG.Size = new System.Drawing.Size(955, 586);
            this.panelBG.TabIndex = 6;
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNo.BackColor = System.Drawing.Color.Transparent;
            this.btnNo.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Blue_Btn;
            this.btnNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.Location = new System.Drawing.Point(540, 456);
            this.btnNo.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(348, 69);
            this.btnNo.TabIndex = 4;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Visible = false;
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnYes.BackColor = System.Drawing.Color.Transparent;
            this.btnYes.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Blue_Btn;
            this.btnYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.Location = new System.Drawing.Point(69, 456);
            this.btnYes.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(348, 69);
            this.btnYes.TabIndex = 3;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Visible = false;
            // 
            // lblDisplayText1
            // 
            this.lblDisplayText1.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayText1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblDisplayText1.Location = new System.Drawing.Point(27, 71);
            this.lblDisplayText1.Margin = new System.Windows.Forms.Padding(0, 0, 9, 0);
            this.lblDisplayText1.Name = "lblDisplayText1";
            this.lblDisplayText1.Size = new System.Drawing.Size(896, 364);
            this.lblDisplayText1.TabIndex = 2;
            this.lblDisplayText1.Text = "Parameter Display Text 1";
            this.lblDisplayText1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Popup_Close_Btn;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(303, 456);
            this.btnClose.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(348, 69);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // frmRedemptionKioskDisplayMessage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Gray;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(955, 586);
            this.Controls.Add(this.panelBG);
            this.Font = new System.Drawing.Font("Bango Pro", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "frmRedemptionKioskDisplayMessage";
            this.Text = "frmMessage";
            this.TransparencyKey = System.Drawing.Color.Gray;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.panelBG.ResumeLayout(false);
            this.ResumeLayout(false);

            }

            #endregion

            internal System.Windows.Forms.Label lblDisplayText1;
            private System.Windows.Forms.Button btnClose;
            public System.Windows.Forms.Panel panelBG;
            private System.Windows.Forms.Button btnNo;
            private System.Windows.Forms.Button btnYes;
        }
    }