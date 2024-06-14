namespace Parafait_Kiosk
{
    partial class frmTapCard : BaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTapCard));
            this.btnClose = new System.Windows.Forms.Button();
            this.lblmsg = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnCustom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClose.Location = new System.Drawing.Point(330, 379);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(325, 115);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnClose_MouseDown);
            this.btnClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnClose_MouseUp);
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
            this.lblmsg.Location = new System.Drawing.Point(4, 150);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(968, 236);
            this.lblmsg.TabIndex = 12;
            this.lblmsg.Text = "Please Tap Your Card At The Reader";
            this.lblmsg.UseVisualStyleBackColor = false;
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.Color.Transparent;
            this.btnYes.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnYes.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnYes.Location = new System.Drawing.Point(87, 385);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(328, 119);
            this.btnYes.TabIndex = 15;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            this.btnYes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseDown);
            this.btnYes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseUp);
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.Transparent;
            this.btnNo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNo.BackgroundImage")));
            this.btnNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNo.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNo.Location = new System.Drawing.Point(580, 385);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(328, 119);
            this.btnNo.TabIndex = 16;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            this.btnNo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseDown);
            this.btnNo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseUp);
            // 
            // btnCustom
            // 
            this.btnCustom.BackColor = System.Drawing.Color.Transparent;
            this.btnCustom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCustom.BackgroundImage")));
            this.btnCustom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCustom.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCustom.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnCustom.FlatAppearance.BorderSize = 0;
            this.btnCustom.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCustom.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCustom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustom.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustom.ForeColor = System.Drawing.Color.White;
            this.btnCustom.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCustom.Location = new System.Drawing.Point(635, 269);
            this.btnCustom.Name = "btnCustom";
            this.btnCustom.Size = new System.Drawing.Size(325, 115);
            this.btnCustom.TabIndex = 17;
            this.btnCustom.Text = "New Card";
            this.btnCustom.UseVisualStyleBackColor = false;
            this.btnCustom.Visible = false;
            this.btnCustom.Click += new System.EventHandler(this.btnCustom_Click);
            this.btnCustom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCustom_MouseDown);
            this.btnCustom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCustom_MouseUp);
            // 
            // frmTapCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(984, 667);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCustom);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.lblmsg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmTapCard";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmYesNo";
            this.TransparencyKey = System.Drawing.Color.Black;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTapCard_FormClosing);
            this.Load += new System.EventHandler(this.frmTapCard_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        //private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button lblmsg;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnCustom;
    }
}