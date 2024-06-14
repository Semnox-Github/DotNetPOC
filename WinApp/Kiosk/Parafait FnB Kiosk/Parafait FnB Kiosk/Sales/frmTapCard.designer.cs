namespace Parafait_FnB_Kiosk
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
            this.btnClose = new System.Windows.Forms.Button();
            this.lblmsg = new System.Windows.Forms.Button();
            this.pbDownArrow = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbDownArrow)).BeginInit();
            this.SuspendLayout();
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
            this.btnClose.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Close_Btn;
            this.btnClose.Location = new System.Drawing.Point(813, 35);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(111, 84);
            this.btnClose.TabIndex = 11;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblmsg
            // 
            this.lblmsg.BackColor = System.Drawing.Color.Transparent;
            this.lblmsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblmsg.FlatAppearance.BorderSize = 0;
            this.lblmsg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblmsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblmsg.Font = new System.Drawing.Font("Bango Pro", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmsg.ForeColor = System.Drawing.Color.White;
            this.lblmsg.Location = new System.Drawing.Point(4, 154);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(948, 336);
            this.lblmsg.TabIndex = 12;
            this.lblmsg.Text = "Please Tap Your Card At The Reader";
            this.lblmsg.UseVisualStyleBackColor = false;
            // 
            // pbDownArrow
            // 
            this.pbDownArrow.BackColor = System.Drawing.Color.Transparent;
            this.pbDownArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbDownArrow.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Arrow_scroll_down;
            this.pbDownArrow.Location = new System.Drawing.Point(388, 496);
            this.pbDownArrow.Name = "pbDownArrow";
            this.pbDownArrow.Size = new System.Drawing.Size(181, 157);
            this.pbDownArrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDownArrow.TabIndex = 19;
            this.pbDownArrow.TabStop = false;
            // 
            // frmTapCard
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.ProductSalePopUp1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(955, 665);
            this.Controls.Add(this.pbDownArrow);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblmsg);
            this.DoubleBuffered = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "frmTapCard";
            this.ShowIcon = false;
            this.Text = "frmTapCard";
            this.TransparencyKey = System.Drawing.Color.SlateGray;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTapCard_FormClosing);
            this.Load += new System.EventHandler(this.frmTapCard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbDownArrow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button lblmsg;
        private System.Windows.Forms.PictureBox pbDownArrow;
    }
}