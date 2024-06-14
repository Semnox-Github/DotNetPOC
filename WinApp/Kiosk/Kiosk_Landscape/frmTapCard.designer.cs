namespace Parafait_Kiosk
{
    partial class frmTapCard
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.lblmsg = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnCustom = new System.Windows.Forms.Button();
            this.pbDownArrow = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbDownArrow)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClose.Location = new System.Drawing.Point(68, 191);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(269, 80);
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
            this.lblmsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmsg.ForeColor = System.Drawing.Color.White;
            this.lblmsg.Location = new System.Drawing.Point(2, 12);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(765, 132);
            this.lblmsg.TabIndex = 12;
            this.lblmsg.Text = "Please Tap Your Card At The Reader";
            this.lblmsg.UseVisualStyleBackColor = false;
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.Color.Transparent;
            this.btnYes.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.button_normal;
            this.btnYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnYes.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnYes.Location = new System.Drawing.Point(97, 190);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(210, 80);
            this.btnYes.TabIndex = 15;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Visible = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            this.btnYes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseDown);
            this.btnYes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseUp);
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.Transparent;
            this.btnNo.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.button_normal;
            this.btnNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNo.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNo.Location = new System.Drawing.Point(448, 190);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(210, 80);
            this.btnNo.TabIndex = 16;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Visible = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            this.btnNo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseDown);
            this.btnNo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnYes_MouseUp);
            // 
            // btnCustom
            // 
            this.btnCustom.BackColor = System.Drawing.Color.Transparent;
            this.btnCustom.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.close_button;
            this.btnCustom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCustom.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCustom.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnCustom.FlatAppearance.BorderSize = 0;
            this.btnCustom.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCustom.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCustom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustom.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustom.ForeColor = System.Drawing.Color.White;
            this.btnCustom.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCustom.Location = new System.Drawing.Point(433, 191);
            this.btnCustom.Name = "btnCustom";
            this.btnCustom.Size = new System.Drawing.Size(269, 80);
            this.btnCustom.TabIndex = 17;
            this.btnCustom.Text = "New Card";
            this.btnCustom.UseVisualStyleBackColor = false;
            this.btnCustom.Visible = false;
            this.btnCustom.Click += new System.EventHandler(this.btnCustom_Click);
            this.btnCustom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCustom_MouseDown);
            this.btnCustom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCustom_MouseUp);
            // 
            // pbDownArrow
            // 
            this.pbDownArrow.BackColor = System.Drawing.Color.Transparent;
            this.pbDownArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbDownArrow.Image = global::Parafait_Kiosk.Properties.Resources.card_reader;
            this.pbDownArrow.Location = new System.Drawing.Point(402, 145);
            this.pbDownArrow.Name = "pbDownArrow";
            this.pbDownArrow.Size = new System.Drawing.Size(272, 253);
            this.pbDownArrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDownArrow.TabIndex = 18;
            this.pbDownArrow.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(12, 276);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(446, 117);
            this.button1.TabIndex = 20;
            this.button1.Text = "Card Reader Below";
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // frmTapCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(779, 410);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pbDownArrow);
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
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTapCard_FormClosing);
            this.Load += new System.EventHandler(this.frmTapCard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbDownArrow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button lblmsg;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnCustom;
        private System.Windows.Forms.PictureBox pbDownArrow;
        private System.Windows.Forms.Button button1;
    }
}
