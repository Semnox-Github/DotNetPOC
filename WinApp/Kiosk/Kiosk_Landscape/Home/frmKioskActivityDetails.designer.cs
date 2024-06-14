namespace Parafait_Kiosk
{
    partial class KioskActivityDetails
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KioskActivityDetails));
            this.btnPrint = new System.Windows.Forms.Button();
            this.panelKioskActivity = new System.Windows.Forms.Panel();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.hScroll = new System.Windows.Forms.HScrollBar();
            this.vScrollBarGp = new System.Windows.Forms.VScrollBar();
            this.dgvKioskActivity = new System.Windows.Forms.DataGridView();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblActivity = new System.Windows.Forms.Label();
            this.lblGreeting = new System.Windows.Forms.Label();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.btnPrintTrx = new System.Windows.Forms.DataGridViewImageColumn();
            this.panelKioskActivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKioskActivity)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(646, 630);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(332, 130);
            this.btnPrint.TabIndex = 159;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // panelKioskActivity
            // 
            this.panelKioskActivity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelKioskActivity.BackColor = System.Drawing.Color.Transparent;
            this.panelKioskActivity.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.KioskActivityTable;
            this.panelKioskActivity.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelKioskActivity.Controls.Add(this.btnRight);
            this.panelKioskActivity.Controls.Add(this.btnLeft);
            this.panelKioskActivity.Controls.Add(this.hScroll);
            this.panelKioskActivity.Controls.Add(this.vScrollBarGp);
            this.panelKioskActivity.Controls.Add(this.dgvKioskActivity);
            this.panelKioskActivity.Location = new System.Drawing.Point(79, 130);
            this.panelKioskActivity.Name = "panelKioskActivity";
            this.panelKioskActivity.Size = new System.Drawing.Size(873, 494);
            this.panelKioskActivity.TabIndex = 158;
            // 
            // btnRight
            // 
            this.btnRight.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Right_Button;
            this.btnRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRight.FlatAppearance.BorderSize = 0;
            this.btnRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRight.ForeColor = System.Drawing.Color.Transparent;
            this.btnRight.Location = new System.Drawing.Point(1023, 662);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(68, 66);
            this.btnRight.TabIndex = 164;
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnLeft.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Left_Button;
            this.btnLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLeft.FlatAppearance.BorderSize = 0;
            this.btnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeft.ForeColor = System.Drawing.Color.Transparent;
            this.btnLeft.Location = new System.Drawing.Point(54, 662);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(68, 66);
            this.btnLeft.TabIndex = 163;
            this.btnLeft.UseVisualStyleBackColor = false;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // hScroll
            // 
            this.hScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScroll.Location = new System.Drawing.Point(3, 369);
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size(867, 37);
            this.hScroll.TabIndex = 162;
            this.hScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScroll_Scroll);
            // 
            // vScrollBarGp
            // 
            this.vScrollBarGp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBarGp.Location = new System.Drawing.Point(831, 29);
            this.vScrollBarGp.Name = "vScrollBarGp";
            this.vScrollBarGp.Size = new System.Drawing.Size(41, 377);
            this.vScrollBarGp.TabIndex = 161;
            this.vScrollBarGp.Visible = false;
            this.vScrollBarGp.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBarGp_Scroll);
            // 
            // dgvKioskActivity
            // 
            this.dgvKioskActivity.AllowUserToAddRows = false;
            this.dgvKioskActivity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) ));
            this.dgvKioskActivity.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvKioskActivity.BackgroundColor = System.Drawing.Color.White;
            this.dgvKioskActivity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvKioskActivity.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKioskActivity.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvKioskActivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKioskActivity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnPrintTrx});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKioskActivity.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvKioskActivity.EnableHeadersVisualStyles = false;
            this.dgvKioskActivity.GridColor = System.Drawing.SystemColors.Control;
            this.dgvKioskActivity.Location = new System.Drawing.Point(3, 29);
            this.dgvKioskActivity.Name = "dgvKioskActivity";
            this.dgvKioskActivity.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKioskActivity.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvKioskActivity.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvKioskActivity.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvKioskActivity.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvKioskActivity.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dgvKioskActivity.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.dgvKioskActivity.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvKioskActivity.RowTemplate.Height = 93;
            this.dgvKioskActivity.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvKioskActivity.Size = new System.Drawing.Size(1123, 598);
            this.dgvKioskActivity.TabIndex = 36;
            this.dgvKioskActivity.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKioskActivity_CellContentClick);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ForeColor = System.Drawing.Color.White;
            this.btnPrev.Location = new System.Drawing.Point(82, 630);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(332, 130);
            this.btnPrev.TabIndex = 152;
            this.btnPrev.Text = "BACK";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            this.btnPrev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseDown);
            this.btnPrev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseUp);
            // 
            // lblActivity
            // 
            this.lblActivity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblActivity.BackColor = System.Drawing.Color.Transparent;
            this.lblActivity.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActivity.ForeColor = System.Drawing.Color.White;
            this.lblActivity.Location = new System.Drawing.Point(75, 93);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(867, 34);
            this.lblActivity.TabIndex = 35;
            this.lblActivity.Text = "Kiosk Activity Details";
            this.lblActivity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblActivity.Visible = false;
            // 
            // lblGreeting
            // 
            this.lblGreeting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreeting.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting.ForeColor = System.Drawing.Color.White;
            this.lblGreeting.Location = new System.Drawing.Point(6, 12);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(1017, 108);
            this.lblGreeting.TabIndex = 143;
            this.lblGreeting.Text = "Kiosk Activity Details of Current Day";
            this.lblGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSiteName
            // 
            this.lblSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSiteName.BackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.BorderSize = 0;
            this.lblSiteName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSiteName.Font = new System.Drawing.Font("Verdana", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(0, 0);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1023, 77);
            this.lblSiteName.TabIndex = 142;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            this.lblSiteName.Visible = false;
            // 
            // btnPrintTrx
            // 
            this.btnPrintTrx.HeaderText = "S";
            this.btnPrintTrx.Image = global::Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
            this.btnPrintTrx.Name = "btnPrintTrx";
            this.btnPrintTrx.ReadOnly = true;
            this.btnPrintTrx.Width = 34;
            // 
            // KioskActivityDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_Screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1024, 772);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.panelKioskActivity);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblActivity);
            this.Controls.Add(this.lblGreeting);
            this.Controls.Add(this.lblSiteName);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "KioskActivityDetails";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parafait Customer Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.KioskActivityDetails_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KioskActivityDetails_KeyPress);
            this.panelKioskActivity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKioskActivity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvKioskActivity;
        private System.Windows.Forms.Label lblActivity;
        internal System.Windows.Forms.Label lblGreeting;
        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Panel panelKioskActivity;
        private System.Windows.Forms.VScrollBar vScrollBarGp;
        private System.Windows.Forms.HScrollBar hScroll;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.DataGridViewImageColumn btnPrintTrx;
    }
}