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
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KioskActivityDetails));
            this.dgvKioskActivity = new System.Windows.Forms.DataGridView();
            this.btnPrintTrx = new System.Windows.Forms.DataGridViewImageColumn();
            this.lblActivity = new System.Windows.Forms.Label();
            this.panelKioskActivity = new System.Windows.Forms.Panel();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.hScroll = new System.Windows.Forms.HScrollBar();
            this.vScrollBarGp = new System.Windows.Forms.VScrollBar();
            this.lblGreeting = new System.Windows.Forms.Label();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnRefund = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKioskActivity)).BeginInit();
            this.panelKioskActivity.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrev
            // 
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(25, 1561);
            this.btnPrev.Size = new System.Drawing.Size(314, 163);
            this.btnPrev.TabIndex = 152;
            this.btnPrev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseDown);
            this.btnPrev.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPrev_MouseUp);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(28, 233);
            this.btnCancel.Size = new System.Drawing.Size(17, 23);
            // 
            // dgvKioskActivity
            // 
            this.dgvKioskActivity.AllowUserToAddRows = false;
            this.dgvKioskActivity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvKioskActivity.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvKioskActivity.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(204)))), ((int)(((byte)(208)))));
            this.dgvKioskActivity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvKioskActivity.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKioskActivity.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvKioskActivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKioskActivity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnPrintTrx});
            this.dgvKioskActivity.EnableHeadersVisualStyles = false;
            this.dgvKioskActivity.GridColor = System.Drawing.SystemColors.Control;
            this.dgvKioskActivity.Location = new System.Drawing.Point(3, 60);
            this.dgvKioskActivity.Name = "dgvKioskActivity";
            this.dgvKioskActivity.ReadOnly = true;
            this.dgvKioskActivity.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvKioskActivity.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvKioskActivity.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvKioskActivity.RowTemplate.Height = 93;
            this.dgvKioskActivity.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKioskActivity.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvKioskActivity.Size = new System.Drawing.Size(954, 965);
            this.dgvKioskActivity.TabIndex = 36;
            this.dgvKioskActivity.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKioskActivity_CellContentClick);
            this.dgvKioskActivity.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvKioskActivity_CellFormatting);

            // 
            // btnPrintTrx
            // 
            this.btnPrintTrx.HeaderText = "S";
            this.btnPrintTrx.Image = global::Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
            this.btnPrintTrx.Name = "btnPrintTrx";
            this.btnPrintTrx.ReadOnly = true;
            this.btnPrintTrx.Width = 35;
            // 
            // lblActivity
            // 
            this.lblActivity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblActivity.BackColor = System.Drawing.Color.Transparent;
            this.lblActivity.Font = new System.Drawing.Font("Bango Pro", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActivity.ForeColor = System.Drawing.Color.White;
            this.lblActivity.Location = new System.Drawing.Point(3, 13);
            this.lblActivity.Name = "lblActivity";
            this.lblActivity.Size = new System.Drawing.Size(954, 34);
            this.lblActivity.TabIndex = 35;
            this.lblActivity.Text = "Kiosk Activity Details";
            this.lblActivity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelKioskActivity
            // 
            this.panelKioskActivity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelKioskActivity.BackColor = System.Drawing.Color.Transparent;
            this.panelKioskActivity.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.table;
            this.panelKioskActivity.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelKioskActivity.Controls.Add(this.buttonPrev);
            this.panelKioskActivity.Controls.Add(this.buttonNext);
            this.panelKioskActivity.Controls.Add(this.hScroll);
            this.panelKioskActivity.Controls.Add(this.vScrollBarGp);
            this.panelKioskActivity.Controls.Add(this.lblActivity);
            this.panelKioskActivity.Controls.Add(this.dgvKioskActivity);
            this.panelKioskActivity.Location = new System.Drawing.Point(50, 282);
            this.panelKioskActivity.Name = "panelKioskActivity";
            this.panelKioskActivity.Size = new System.Drawing.Size(960, 1215);
            this.panelKioskActivity.TabIndex = 158;
            // 
            // buttonPrev
            // 
            this.buttonPrev.BackColor = System.Drawing.Color.Transparent;
            this.buttonPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.left_arrow;
            this.buttonPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonPrev.FlatAppearance.BorderSize = 0;
            this.buttonPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPrev.Font = new System.Drawing.Font("Bango Pro", 46F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPrev.ForeColor = System.Drawing.Color.White;
            this.buttonPrev.Location = new System.Drawing.Point(77, 1105);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(70, 68);
            this.buttonPrev.TabIndex = 167;
            this.buttonPrev.UseVisualStyleBackColor = false;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.BackColor = System.Drawing.Color.Transparent;
            this.buttonNext.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.right_arrow;
            this.buttonNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonNext.FlatAppearance.BorderSize = 0;
            this.buttonNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNext.Font = new System.Drawing.Font("Bango Pro", 46F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNext.ForeColor = System.Drawing.Color.White;
            this.buttonNext.Location = new System.Drawing.Point(829, 1105);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(72, 68);
            this.buttonNext.TabIndex = 166;
            this.buttonNext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonNext.UseVisualStyleBackColor = false;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // hScroll
            // 
            this.hScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScroll.Location = new System.Drawing.Point(3, 1025);
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new System.Drawing.Size(954, 59);
            this.hScroll.TabIndex = 162;
            this.hScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScroll_Scroll);
            // 
            // vScrollBarGp
            // 
            this.vScrollBarGp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBarGp.Location = new System.Drawing.Point(917, 60);
            this.vScrollBarGp.Name = "vScrollBarGp";
            this.vScrollBarGp.Size = new System.Drawing.Size(41, 965);
            this.vScrollBarGp.TabIndex = 161;
            this.vScrollBarGp.Visible = false;
            this.vScrollBarGp.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBarGp_Scroll);
            // 
            // lblGreeting
            // 
            this.lblGreeting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreeting.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting.ForeColor = System.Drawing.Color.White;
            this.lblGreeting.Location = new System.Drawing.Point(0, 179);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(1079, 51);
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
            this.lblSiteName.Size = new System.Drawing.Size(1079, 77);
            this.lblSiteName.TabIndex = 142;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Bango Pro", 36F);
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(364, 1561);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(323, 167);
            this.btnPrint.TabIndex = 159;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnRefund
            // 
            this.btnRefund.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefund.BackColor = System.Drawing.Color.Transparent;
            this.btnRefund.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnRefund.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRefund.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnRefund.FlatAppearance.BorderSize = 0;
            this.btnRefund.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRefund.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefund.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefund.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefund.Font = new System.Drawing.Font("Bango Pro", 36F);
            this.btnRefund.ForeColor = System.Drawing.Color.White;
            this.btnRefund.Location = new System.Drawing.Point(708, 1563);
            this.btnRefund.Name = "btnRefund";
            this.btnRefund.Size = new System.Drawing.Size(323, 167);
            this.btnRefund.TabIndex = 160;
            this.btnRefund.Text = "Refund";
            this.btnRefund.UseVisualStyleBackColor = false;
            this.btnRefund.Click += new System.EventHandler(this.btnRefund_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.AutoEllipsis = true;
            this.txtMessage.BackColor = System.Drawing.Color.Transparent;
            this.txtMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.FlatAppearance.BorderSize = 0;
            this.txtMessage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.txtMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.txtMessage.Font = new System.Drawing.Font("Bango Pro", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(0, 1871);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(1080, 49);
            this.txtMessage.TabIndex = 136;
            this.txtMessage.Text = "Message";
            this.txtMessage.UseVisualStyleBackColor = false;

            //
            // KioskActivityDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnRefund);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.panelKioskActivity);
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
            this.Controls.SetChildIndex(this.lblSiteName, 0);
            this.Controls.SetChildIndex(this.lblGreeting, 0);
            this.Controls.SetChildIndex(this.panelKioskActivity, 0);
            this.Controls.SetChildIndex(this.btnPrint, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnRefund, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKioskActivity)).EndInit();
            this.panelKioskActivity.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvKioskActivity;
        private System.Windows.Forms.Label lblActivity;
        internal System.Windows.Forms.Label lblGreeting;
        private System.Windows.Forms.Button lblSiteName;
        //private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Panel panelKioskActivity;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.HScrollBar hScroll;
        private System.Windows.Forms.VScrollBar vScrollBarGp;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonPrev;
        private System.Windows.Forms.DataGridViewImageColumn btnPrintTrx;
        private System.Windows.Forms.Button btnRefund;
        private System.Windows.Forms.Button txtMessage;
    }
}