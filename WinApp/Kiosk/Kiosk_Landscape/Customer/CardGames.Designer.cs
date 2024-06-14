namespace Parafait_Kiosk
{
    partial class CardGames
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
            this.btnPrev = new System.Windows.Forms.Button();
            this.dgvCardGames = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardGames)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.alert_close_btn;
            this.btnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnPrev.Location = new System.Drawing.Point(194, 315);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(142, 110);
            this.btnPrev.TabIndex = 153;
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            this.btnPrev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseDown);
            this.btnPrev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseUp);
            // 
            // dgvCardGames
            // 
            this.dgvCardGames.AllowUserToAddRows = false;
            this.dgvCardGames.AllowUserToDeleteRows = false;
            this.dgvCardGames.AllowUserToResizeColumns = false;
            this.dgvCardGames.AllowUserToResizeRows = false;
            this.dgvCardGames.BackgroundColor = System.Drawing.Color.Gold;
            this.dgvCardGames.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCardGames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCardGames.Location = new System.Drawing.Point(0, 1);
            this.dgvCardGames.Name = "dgvCardGames";
            this.dgvCardGames.ReadOnly = true;
            this.dgvCardGames.Size = new System.Drawing.Size(532, 308);
            this.dgvCardGames.TabIndex = 154;
            // 
            // timer1
            // 
            //this.timer1.Interval = 1000;
            //this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // CardGames
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gold;
            this.ClientSize = new System.Drawing.Size(532, 426);
            this.Controls.Add(this.dgvCardGames);
            this.Controls.Add(this.btnPrev);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CardGames";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.CardGames_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardGames)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.DataGridView dgvCardGames;
        private System.Windows.Forms.Timer timer1;

    }
}
