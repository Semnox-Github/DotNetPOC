namespace Parafait_Kiosk
{
    partial class frmEntitlement
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
            this.lblmsg = new System.Windows.Forms.Button();
            this.btnTime = new System.Windows.Forms.Button();
            this.btnPoints = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblmsg
            // 
            this.lblmsg.BackColor = System.Drawing.Color.Transparent;
            this.lblmsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblmsg.FlatAppearance.BorderSize = 0;
            this.lblmsg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblmsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmsg.ForeColor = System.Drawing.Color.White;
            this.lblmsg.Location = new System.Drawing.Point(2, 12);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(765, 132);
            this.lblmsg.TabIndex = 13;
            this.lblmsg.Text = "What would yo like to add?";
            this.lblmsg.UseVisualStyleBackColor = false;
            // 
            // btnTime
            // 
            this.btnTime.BackColor = System.Drawing.Color.Transparent;
            this.btnTime.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.credit_debit_button;
            this.btnTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTime.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnTime.FlatAppearance.BorderSize = 0;
            this.btnTime.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTime.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTime.ForeColor = System.Drawing.Color.Black;
            this.btnTime.Location = new System.Drawing.Point(60, 171);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(268, 119);
            this.btnTime.TabIndex = 167;
            this.btnTime.Text = "Play Time";
            this.btnTime.UseVisualStyleBackColor = false;
            this.btnTime.Click += new System.EventHandler(this.btnTime_Click);
            // 
            // btnPoints
            // 
            this.btnPoints.BackColor = System.Drawing.Color.Transparent;
            this.btnPoints.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.credit_debit_button;
            this.btnPoints.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPoints.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnPoints.FlatAppearance.BorderSize = 0;
            this.btnPoints.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPoints.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPoints.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPoints.ForeColor = System.Drawing.Color.Black;
            this.btnPoints.Location = new System.Drawing.Point(427, 171);
            this.btnPoints.Name = "btnPoints";
            this.btnPoints.Size = new System.Drawing.Size(268, 119);
            this.btnPoints.TabIndex = 168;
            this.btnPoints.Text = "Play Point";
            this.btnPoints.UseVisualStyleBackColor = false;
            this.btnPoints.Click += new System.EventHandler(this.btnPoints_Click);
            // 
            // frmEntitlement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(779, 410);
            this.Controls.Add(this.btnPoints);
            this.Controls.Add(this.btnTime);
            this.Controls.Add(this.lblmsg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmEntitlement";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmEntitlement";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button lblmsg;
        private System.Windows.Forms.Button btnTime;
        private System.Windows.Forms.Button btnPoints;
    }
}