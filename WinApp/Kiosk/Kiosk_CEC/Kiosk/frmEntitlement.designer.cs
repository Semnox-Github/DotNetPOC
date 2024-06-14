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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEntitlement));
            this.lblmsg = new System.Windows.Forms.Button();
            this.btnTime = new System.Windows.Forms.Button();
            this.btnPoints = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
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
            this.lblmsg.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmsg.ForeColor = System.Drawing.Color.White;
            this.lblmsg.Location = new System.Drawing.Point(4, 91);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(968, 96);
            this.lblmsg.TabIndex = 13;
            this.lblmsg.Text = "Choose Entitlement";
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
            this.btnTime.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTime.ForeColor = System.Drawing.Color.Black;
            this.btnTime.Location = new System.Drawing.Point(102, 225);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(329, 119);
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
            this.btnPoints.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPoints.Location = new System.Drawing.Point(553, 225);
            this.btnPoints.Name = "btnPoints";
            this.btnPoints.Size = new System.Drawing.Size(329, 119);
            this.btnPoints.TabIndex = 169;
            this.btnPoints.Text = "Play Points";
            this.btnPoints.UseVisualStyleBackColor = false;
            this.btnPoints.Click += new System.EventHandler(this.btnPoints_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOk.BackgroundImage")));
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnOk.Location = new System.Drawing.Point(352, 369);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(326, 72);
            this.btnOk.TabIndex = 170;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Visible = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmEntitlement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(984, 547);
            this.Controls.Add(this.btnOk);
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
            this.TransparencyKey = System.Drawing.Color.Black;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button lblmsg;
        private System.Windows.Forms.Button btnTime;
        private System.Windows.Forms.Button btnPoints;
        private System.Windows.Forms.Button btnOk;
    }
}
