namespace Redemption_Kiosk
{
    partial class frmRedemptionKioskTapCard
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
            this.pbDownArrow = new System.Windows.Forms.PictureBox();
            this.lblmsg = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbDownArrow)).BeginInit();
            this.SuspendLayout();
            // 
            // pbDownArrow
            // 
            this.pbDownArrow.BackColor = System.Drawing.Color.Transparent;
            this.pbDownArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbDownArrow.Image = global::Redemption_Kiosk.Properties.Resources.Arrow_scroll_down;
            this.pbDownArrow.Location = new System.Drawing.Point(342, 474);
            this.pbDownArrow.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.pbDownArrow.Name = "pbDownArrow";
            this.pbDownArrow.Size = new System.Drawing.Size(173, 158);
            this.pbDownArrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDownArrow.TabIndex = 22;
            this.pbDownArrow.TabStop = false;
            // 
            // lblmsg
            // 
            this.lblmsg.BackColor = System.Drawing.Color.Transparent;
            this.lblmsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblmsg.FlatAppearance.BorderSize = 0;
            this.lblmsg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblmsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmsg.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblmsg.Location = new System.Drawing.Point(27, 178);
            this.lblmsg.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(762, 272);
            this.lblmsg.TabIndex = 21;
            this.lblmsg.Text = "Please Tap Your Card At The Reader";
            this.lblmsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblmsg.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Bango Pro", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Image = global::Redemption_Kiosk.Properties.Resources.Close_Btn1;
            this.btnClose.Location = new System.Drawing.Point(642, 47);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(111, 84);
            this.btnClose.TabIndex = 23;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // frmTapCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Redemption_Kiosk.Properties.Resources.Pop_up;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(817, 676);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pbDownArrow);
            this.Controls.Add(this.lblmsg);
            this.DoubleBuffered = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(15, 13, 15, 13);
            this.Name = "frmTapCard";
            this.Text = "frmTapCard";
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTapCard_FormClosing);
            this.Load += new System.EventHandler(this.FrmTapCard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbDownArrow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbDownArrow;
        private System.Windows.Forms.Button lblmsg;
        private System.Windows.Forms.Button btnClose;
    }
}