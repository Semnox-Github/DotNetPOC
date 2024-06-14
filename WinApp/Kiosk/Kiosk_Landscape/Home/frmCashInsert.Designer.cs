namespace Parafait_Kiosk
{
    partial class frmCashInsert
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.TimerMoney = new System.Windows.Forms.Timer(this.components);
            this.exitTimer = new System.Windows.Forms.Timer(this.components);
            this.panelGrid = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvCashInserted = new System.Windows.Forms.DataGridView();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.Denomination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnNewCard = new System.Windows.Forms.Button();
            this.lblGreeting1 = new System.Windows.Forms.Label();
            this.btnRecharge = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblCashInserted = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnHome = new System.Windows.Forms.Button();
            this.panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCashInserted)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSiteName
            // 
            this.lblSiteName.BackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSiteName.FlatAppearance.BorderSize = 0;
            this.lblSiteName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblSiteName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSiteName.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(0, 0);
            this.lblSiteName.Margin = new System.Windows.Forms.Padding(6);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1280, 82);
            this.lblSiteName.TabIndex = 137;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            this.lblSiteName.Visible = false;
            // 
            // panelGrid
            // 
            this.panelGrid.BackColor = System.Drawing.Color.Transparent;
            this.panelGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelGrid.Controls.Add(this.label4);
            this.panelGrid.Controls.Add(this.label3);
            this.panelGrid.Controls.Add(this.label2);
            this.panelGrid.Controls.Add(this.dgvCashInserted);
            this.panelGrid.Location = new System.Drawing.Point(410, 934);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(661, 343);
            this.panelGrid.TabIndex = 145;
            this.panelGrid.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(518, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 24);
            this.label4.TabIndex = 4;
            this.label4.Text = "Points";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(326, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 24);
            this.label3.TabIndex = 3;
            this.label3.Text = "Quantity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(73, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "Denomination";
            // 
            // dgvCashInserted
            // 
            this.dgvCashInserted.AllowUserToAddRows = false;
            this.dgvCashInserted.AllowUserToDeleteRows = false;
            this.dgvCashInserted.AllowUserToResizeColumns = false;
            this.dgvCashInserted.AllowUserToResizeRows = false;
            this.dgvCashInserted.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCashInserted.BackgroundColor = System.Drawing.Color.DarkGray;
            this.dgvCashInserted.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCashInserted.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DarkKhaki;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCashInserted.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCashInserted.ColumnHeadersVisible = false;
            this.dgvCashInserted.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Image,
            this.Denomination,
            this.Quantity,
            this.Amount});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Khaki;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Khaki;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCashInserted.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCashInserted.EnableHeadersVisualStyles = false;
            this.dgvCashInserted.GridColor = System.Drawing.Color.White;
            this.dgvCashInserted.Location = new System.Drawing.Point(0, 42);
            this.dgvCashInserted.Name = "dgvCashInserted";
            this.dgvCashInserted.ReadOnly = true;
            this.dgvCashInserted.RowHeadersVisible = false;
            this.dgvCashInserted.RowTemplate.Height = 40;
            this.dgvCashInserted.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvCashInserted.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCashInserted.Size = new System.Drawing.Size(660, 260);
            this.dgvCashInserted.TabIndex = 1;
            // 
            // Image
            // 
            this.Image.HeaderText = "";
            this.Image.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Image.Name = "Image";
            this.Image.ReadOnly = true;
            this.Image.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Image.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Image.Visible = false;
            // 
            // Denomination
            // 
            this.Denomination.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Denomination.HeaderText = "Denomination";
            this.Denomination.Name = "Denomination";
            this.Denomination.ReadOnly = true;
            this.Denomination.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Denomination.Width = 269;
            // 
            // Quantity
            // 
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Quantity.Width = 196;
            // 
            // Amount
            // 
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            this.Amount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Amount.Width = 195;
            // 
            // btnNewCard
            // 
            this.btnNewCard.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNewCard.BackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.New_Play_Pass_Button;
            this.btnNewCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnNewCard.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnNewCard.FlatAppearance.BorderSize = 0;
            this.btnNewCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNewCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 35.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewCard.ForeColor = System.Drawing.Color.White;
            this.btnNewCard.Location = new System.Drawing.Point(166, 370);
            this.btnNewCard.Name = "btnNewCard";
            this.btnNewCard.Size = new System.Drawing.Size(447, 290);
            this.btnNewCard.TabIndex = 147;
            this.btnNewCard.Text = "Buy NEW Card";
            this.btnNewCard.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNewCard.UseVisualStyleBackColor = false;
            this.btnNewCard.Click += new System.EventHandler(this.btnNewCard_Click);
            // 
            // lblGreeting1
            // 
            this.lblGreeting1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreeting1.BackColor = System.Drawing.Color.Transparent;
            this.lblGreeting1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGreeting1.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting1.ForeColor = System.Drawing.Color.White;
            this.lblGreeting1.Location = new System.Drawing.Point(6, 12);
            this.lblGreeting1.Name = "lblGreeting1";
            this.lblGreeting1.Size = new System.Drawing.Size(1265, 82);
            this.lblGreeting1.TabIndex = 149;
            this.lblGreeting1.Text = "Insert more cash";
            this.lblGreeting1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRecharge
            // 
            this.btnRecharge.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRecharge.BackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.LoadExisting;
            this.btnRecharge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRecharge.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnRecharge.FlatAppearance.BorderSize = 0;
            this.btnRecharge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRecharge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecharge.Font = new System.Drawing.Font("Microsoft Sans Serif", 35.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecharge.ForeColor = System.Drawing.Color.White;
            this.btnRecharge.Location = new System.Drawing.Point(673, 370);
            this.btnRecharge.Name = "btnRecharge";
            this.btnRecharge.Size = new System.Drawing.Size(447, 290);
            this.btnRecharge.TabIndex = 151;
            this.btnRecharge.Text = "Recharge Card";
            this.btnRecharge.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRecharge.UseVisualStyleBackColor = false;
            this.btnRecharge.Click += new System.EventHandler(this.btnRecharge_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.Controls.Add(this.lblCashInserted);
            this.panel2.Location = new System.Drawing.Point(653, 232);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(366, 107);
            this.panel2.TabIndex = 1055;
            // 
            // lblCashInserted
            // 
            this.lblCashInserted.BackColor = System.Drawing.Color.Transparent;
            this.lblCashInserted.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCashInserted.ForeColor = System.Drawing.Color.Black;
            this.lblCashInserted.Location = new System.Drawing.Point(60, 18);
            this.lblCashInserted.Name = "lblCashInserted";
            this.lblCashInserted.Size = new System.Drawing.Size(248, 68);
            this.lblCashInserted.TabIndex = 1048;
            this.lblCashInserted.Text = "9";
            this.lblCashInserted.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(134, 260);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(514, 52);
            this.label1.TabIndex = 1054;
            this.label1.Text = "Cash Inserted:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(0, 689);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1280, 52);
            this.label5.TabIndex = 1056;
            this.label5.Text = "Insert more cash at any time";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.Transparent;
            this.btnHome.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.home_button;
            this.btnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Location = new System.Drawing.Point(31, 28);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(153, 151);
            this.btnHome.TabIndex = 20011;
            this.btnHome.Text = "GO HOME";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Visible = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // frmCashInsert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_Screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1280, 741);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRecharge);
            this.Controls.Add(this.lblGreeting1);
            this.Controls.Add(this.btnNewCard);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.panelGrid);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmCashInsert";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmRedeemTokens";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCashInsert_FormClosing);
            this.Load += new System.EventHandler(this.frmCashInsert_Load);
            this.panelGrid.ResumeLayout(false);
            this.panelGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCashInserted)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button lblSiteName;
        private System.Windows.Forms.Timer TimerMoney;
        private System.Windows.Forms.Timer exitTimer;
        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvCashInserted;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn Denomination;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.Button btnNewCard;
        internal System.Windows.Forms.Label lblGreeting1;
        private System.Windows.Forms.Button btnRecharge;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblCashInserted;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnHome;
    }
}