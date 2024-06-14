namespace Parafait_FnB_Kiosk
{
    partial class frmCustomerDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomerDashboard));
            this.dgvPurchases = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvGamePlay = new System.Windows.Forms.DataGridView();
            this.Column0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label21 = new System.Windows.Forms.Label();
            this.grpActivities = new System.Windows.Forms.Panel();
            this.panelPurchase = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.vScrollBarPur = new System.Windows.Forms.VScrollBar();
            this.label13 = new System.Windows.Forms.Label();
            this.panelGameplay = new System.Windows.Forms.Panel();
            this.lblTicketsCourtesy = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.vScrollBarGp = new System.Windows.Forms.VScrollBar();
            this.lblScreenTitle = new System.Windows.Forms.Button();
            this.lblPlayValueLabel = new System.Windows.Forms.Label();
            this.lblCredits = new System.Windows.Forms.Label();
            this.panelBalance = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.lblCard = new System.Windows.Forms.Label();
            this.panelCredits = new System.Windows.Forms.Panel();
            this.lblTimeRemainingText = new System.Windows.Forms.Label();
            this.lblTimeRemainingValue = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblTimeOut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchases)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGamePlay)).BeginInit();
            this.grpActivities.SuspendLayout();
            this.panelPurchase.SuspendLayout();
            this.panelGameplay.SuspendLayout();
            this.panelBalance.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelCredits.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPurchases
            // 
            this.dgvPurchases.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPurchases.BackgroundColor = System.Drawing.Color.White;
            this.dgvPurchases.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPurchases.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvPurchases.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchases.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPurchases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPurchases.ColumnHeadersVisible = false;
            this.dgvPurchases.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9});
            this.dgvPurchases.EnableHeadersVisualStyles = false;
            this.dgvPurchases.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.dgvPurchases.Location = new System.Drawing.Point(4, 113);
            this.dgvPurchases.Name = "dgvPurchases";
            this.dgvPurchases.ReadOnly = true;
            this.dgvPurchases.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPurchases.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPurchases.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPurchases.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPurchases.Size = new System.Drawing.Size(936, 401);
            this.dgvPurchases.TabIndex = 35;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column5.HeaderText = "Column5";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 193;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column6.HeaderText = "Column6";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 162;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column7.HeaderText = "Column7";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 190;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column8.HeaderText = "Column8";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 200;
            // 
            // Column9
            // 
            this.Column9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column9.HeaderText = "Column9";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Width = 192;
            // 
            // dgvGamePlay
            // 
            this.dgvGamePlay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvGamePlay.BackgroundColor = System.Drawing.Color.White;
            this.dgvGamePlay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGamePlay.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvGamePlay.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGamePlay.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvGamePlay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGamePlay.ColumnHeadersVisible = false;
            this.dgvGamePlay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column0,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dgvGamePlay.EnableHeadersVisualStyles = false;
            this.dgvGamePlay.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.dgvGamePlay.Location = new System.Drawing.Point(6, 132);
            this.dgvGamePlay.Name = "dgvGamePlay";
            this.dgvGamePlay.ReadOnly = true;
            this.dgvGamePlay.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvGamePlay.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvGamePlay.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvGamePlay.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvGamePlay.Size = new System.Drawing.Size(932, 385);
            this.dgvGamePlay.TabIndex = 36;
            // 
            // Column0
            // 
            this.Column0.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column0.HeaderText = "Column0";
            this.Column0.Name = "Column0";
            this.Column0.ReadOnly = true;
            this.Column0.Width = 163;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 150;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 234;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 198;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 188;
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(189)))), ((int)(((byte)(34)))));
            this.label21.Location = new System.Drawing.Point(3, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(930, 43);
            this.label21.TabIndex = 35;
            this.label21.Text = "Game Play Details";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpActivities
            // 
            this.grpActivities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpActivities.BackColor = System.Drawing.Color.Transparent;
            this.grpActivities.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.grpActivities.Controls.Add(this.panelPurchase);
            this.grpActivities.Controls.Add(this.panelGameplay);
            this.grpActivities.Location = new System.Drawing.Point(48, 438);
            this.grpActivities.Name = "grpActivities";
            this.grpActivities.Size = new System.Drawing.Size(989, 1179);
            this.grpActivities.TabIndex = 39;
            this.grpActivities.Visible = false;
            // 
            // panelPurchase
            // 
            this.panelPurchase.BackColor = System.Drawing.Color.Transparent;
            this.panelPurchase.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.product_table;
            this.panelPurchase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelPurchase.Controls.Add(this.label1);
            this.panelPurchase.Controls.Add(this.label11);
            this.panelPurchase.Controls.Add(this.label10);
            this.panelPurchase.Controls.Add(this.label9);
            this.panelPurchase.Controls.Add(this.label8);
            this.panelPurchase.Controls.Add(this.vScrollBarPur);
            this.panelPurchase.Controls.Add(this.label13);
            this.panelPurchase.Controls.Add(this.dgvPurchases);
            this.panelPurchase.Location = new System.Drawing.Point(4, 600);
            this.panelPurchase.Name = "panelPurchase";
            this.panelPurchase.Size = new System.Drawing.Size(981, 592);
            this.panelPurchase.TabIndex = 160;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label1.Location = new System.Drawing.Point(800, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 32);
            this.label1.TabIndex = 167;
            this.label1.Text = "Time";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label11.Location = new System.Drawing.Point(576, 66);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(162, 32);
            this.label11.TabIndex = 166;
            this.label11.Text = "Play Points";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label10.Location = new System.Drawing.Point(394, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(122, 32);
            this.label10.TabIndex = 165;
            this.label10.Text = "Amount";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label9.Location = new System.Drawing.Point(222, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 32);
            this.label9.TabIndex = 164;
            this.label9.Text = "Product";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label8.Location = new System.Drawing.Point(54, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 32);
            this.label8.TabIndex = 163;
            this.label8.Text = "Date";
            // 
            // vScrollBarPur
            // 
            this.vScrollBarPur.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBarPur.Location = new System.Drawing.Point(939, 113);
            this.vScrollBarPur.Name = "vScrollBarPur";
            this.vScrollBarPur.Size = new System.Drawing.Size(41, 401);
            this.vScrollBarPur.TabIndex = 160;
            this.vScrollBarPur.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBarPur_Scroll);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(189)))), ((int)(((byte)(34)))));
            this.label13.Location = new System.Drawing.Point(3, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(975, 35);
            this.label13.TabIndex = 159;
            this.label13.Text = "Purchases and Tasks";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelGameplay
            // 
            this.panelGameplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGameplay.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.game_price_table;
            this.panelGameplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelGameplay.Controls.Add(this.lblTicketsCourtesy);
            this.panelGameplay.Controls.Add(this.label6);
            this.panelGameplay.Controls.Add(this.label5);
            this.panelGameplay.Controls.Add(this.label4);
            this.panelGameplay.Controls.Add(this.label3);
            this.panelGameplay.Controls.Add(this.vScrollBarGp);
            this.panelGameplay.Controls.Add(this.label21);
            this.panelGameplay.Controls.Add(this.dgvGamePlay);
            this.panelGameplay.Location = new System.Drawing.Point(4, 8);
            this.panelGameplay.Name = "panelGameplay";
            this.panelGameplay.Size = new System.Drawing.Size(985, 589);
            this.panelGameplay.TabIndex = 158;
            // 
            // lblTicketsCourtesy
            // 
            this.lblTicketsCourtesy.AutoSize = true;
            this.lblTicketsCourtesy.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketsCourtesy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.lblTicketsCourtesy.Location = new System.Drawing.Point(800, 55);
            this.lblTicketsCourtesy.Name = "lblTicketsCourtesy";
            this.lblTicketsCourtesy.Size = new System.Drawing.Size(133, 32);
            this.lblTicketsCourtesy.TabIndex = 166;
            this.lblTicketsCourtesy.Text = "Courtesy";
            this.lblTicketsCourtesy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label6.Location = new System.Drawing.Point(565, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(182, 74);
            this.label6.TabIndex = 165;
            this.label6.Text = "Game Price (Bonus)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label5.Location = new System.Drawing.Point(354, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(182, 74);
            this.label5.TabIndex = 164;
            this.label5.Text = "Game Price (Play Points)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label4.Location = new System.Drawing.Point(209, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 32);
            this.label4.TabIndex = 163;
            this.label4.Text = "Game";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bango Pro", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label3.Location = new System.Drawing.Point(38, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 32);
            this.label3.TabIndex = 162;
            this.label3.Text = "Date";
            // 
            // vScrollBarGp
            // 
            this.vScrollBarGp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBarGp.Location = new System.Drawing.Point(940, 132);
            this.vScrollBarGp.Name = "vScrollBarGp";
            this.vScrollBarGp.Size = new System.Drawing.Size(41, 385);
            this.vScrollBarGp.TabIndex = 161;
            this.vScrollBarGp.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBarGp_Scroll);
            // 
            // lblScreenTitle
            // 
            this.lblScreenTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScreenTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblScreenTitle.FlatAppearance.BorderSize = 0;
            this.lblScreenTitle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblScreenTitle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblScreenTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblScreenTitle.Font = new System.Drawing.Font("Bango Pro", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenTitle.ForeColor = System.Drawing.Color.White;
            this.lblScreenTitle.Location = new System.Drawing.Point(0, 48);
            this.lblScreenTitle.Name = "lblScreenTitle";
            this.lblScreenTitle.Size = new System.Drawing.Size(1079, 119);
            this.lblScreenTitle.TabIndex = 142;
            this.lblScreenTitle.Text = "Card Balance / Activity";
            this.lblScreenTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblScreenTitle.UseVisualStyleBackColor = false;
            // 
            // lblPlayValueLabel
            // 
            this.lblPlayValueLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPlayValueLabel.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(189)))), ((int)(((byte)(34)))));
            this.lblPlayValueLabel.Location = new System.Drawing.Point(3, -6);
            this.lblPlayValueLabel.Name = "lblPlayValueLabel";
            this.lblPlayValueLabel.Size = new System.Drawing.Size(469, 58);
            this.lblPlayValueLabel.TabIndex = 144;
            this.lblPlayValueLabel.Text = "Play Value:";
            this.lblPlayValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCredits
            // 
            this.lblCredits.BackColor = System.Drawing.Color.Transparent;
            this.lblCredits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCredits.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredits.ForeColor = System.Drawing.Color.White;
            this.lblCredits.Location = new System.Drawing.Point(473, -6);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new System.Drawing.Size(444, 58);
            this.lblCredits.TabIndex = 148;
            this.lblCredits.Text = "00000";
            this.lblCredits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelBalance
            // 
            this.panelBalance.BackColor = System.Drawing.Color.Transparent;
            this.panelBalance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBalance.Controls.Add(this.flowLayoutPanel1);
            this.panelBalance.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelBalance.ForeColor = System.Drawing.Color.White;
            this.panelBalance.Location = new System.Drawing.Point(48, 221);
            this.panelBalance.Name = "panelBalance";
            this.panelBalance.Size = new System.Drawing.Size(993, 202);
            this.panelBalance.TabIndex = 153;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panelCredits);
            this.flowLayoutPanel1.Controls.Add(this.lblTimeRemainingText);
            this.flowLayoutPanel1.Controls.Add(this.lblTimeRemainingValue);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(993, 202);
            this.flowLayoutPanel1.TabIndex = 158;
            // 
            // panel1
            // 
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.lblCardNumber);
            this.panel1.Controls.Add(this.lblCard);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(987, 62);
            this.panel1.TabIndex = 154;
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblCardNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCardNumber.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(189)))), ((int)(((byte)(34)))));
            this.lblCardNumber.Location = new System.Drawing.Point(3, -6);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(469, 58);
            this.lblCardNumber.TabIndex = 147;
            this.lblCardNumber.Text = "Card#:";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCard
            // 
            this.lblCard.BackColor = System.Drawing.Color.Transparent;
            this.lblCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCard.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCard.ForeColor = System.Drawing.Color.White;
            this.lblCard.Location = new System.Drawing.Point(473, -6);
            this.lblCard.Name = "lblCard";
            this.lblCard.Size = new System.Drawing.Size(444, 58);
            this.lblCard.TabIndex = 151;
            this.lblCard.Text = "00000";
            this.lblCard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelCredits
            // 
            this.panelCredits.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelCredits.Controls.Add(this.lblPlayValueLabel);
            this.panelCredits.Controls.Add(this.lblCredits);
            this.panelCredits.Location = new System.Drawing.Point(0, 62);
            this.panelCredits.Margin = new System.Windows.Forms.Padding(0);
            this.panelCredits.Name = "panelCredits";
            this.panelCredits.Size = new System.Drawing.Size(991, 60);
            this.panelCredits.TabIndex = 0;
            // 
            // lblTimeRemainingText
            // 
            this.lblTimeRemainingText.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemainingText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeRemainingText.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeRemainingText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(189)))), ((int)(((byte)(34)))));
            this.lblTimeRemainingText.Location = new System.Drawing.Point(3, 122);
            this.lblTimeRemainingText.Name = "lblTimeRemainingText";
            this.lblTimeRemainingText.Size = new System.Drawing.Size(469, 58);
            this.lblTimeRemainingText.TabIndex = 155;
            this.lblTimeRemainingText.Text = "Time Remaining:";
            this.lblTimeRemainingText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTimeRemainingValue
            // 
            this.lblTimeRemainingValue.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemainingValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeRemainingValue.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeRemainingValue.ForeColor = System.Drawing.Color.White;
            this.lblTimeRemainingValue.Location = new System.Drawing.Point(478, 122);
            this.lblTimeRemainingValue.Name = "lblTimeRemainingValue";
            this.lblTimeRemainingValue.Size = new System.Drawing.Size(444, 58);
            this.lblTimeRemainingValue.TabIndex = 156;
            this.lblTimeRemainingValue.Text = "00000";
            this.lblTimeRemainingValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Close_Btn;
            this.btnPrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ForeColor = System.Drawing.Color.White;
            this.btnPrev.Location = new System.Drawing.Point(935, 48);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(81, 84);
            this.btnPrev.TabIndex = 152;
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblTimeOut
            // 
            this.lblTimeOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeOut.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lblTimeOut.FlatAppearance.BorderSize = 0;
            this.lblTimeOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeOut.Font = new System.Drawing.Font("Verdana", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOut.ForeColor = System.Drawing.Color.White;
            this.lblTimeOut.Location = new System.Drawing.Point(891, 150);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(142, 110);
            this.lblTimeOut.TabIndex = 159;
            this.lblTimeOut.UseVisualStyleBackColor = false;
            // 
            // frmCustomerDashboard
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Pizza_Screen_BYO_Popup;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1640);
            this.Controls.Add(this.lblTimeOut);
            this.Controls.Add(this.grpActivities);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblScreenTitle);
            this.Controls.Add(this.panelBalance);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "frmCustomerDashboard";
            this.ShowIcon = false;
            this.Text = "Parafait Customer Dashboard";
            this.TransparencyKey = System.Drawing.Color.LightSlateGray;
            this.Load += new System.EventHandler(this.CustomerDashboard_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Dashboard_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchases)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGamePlay)).EndInit();
            this.grpActivities.ResumeLayout(false);
            this.panelPurchase.ResumeLayout(false);
            this.panelPurchase.PerformLayout();
            this.panelGameplay.ResumeLayout(false);
            this.panelGameplay.PerformLayout();
            this.panelBalance.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelCredits.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPurchases;
        private System.Windows.Forms.DataGridView dgvGamePlay;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Panel grpActivities;
        private System.Windows.Forms.Button lblScreenTitle;
        internal System.Windows.Forms.Label lblPlayValueLabel;
        internal System.Windows.Forms.Label lblCredits;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Panel panelBalance;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panelGameplay;
        private System.Windows.Forms.Panel panelPurchase;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panelCredits;
        private System.Windows.Forms.VScrollBar vScrollBarPur;
        private System.Windows.Forms.VScrollBar vScrollBarGp;
        private System.Windows.Forms.Button lblTimeOut;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label lblCardNumber;
        internal System.Windows.Forms.Label lblCard;
        private System.Windows.Forms.Label lblTicketsCourtesy;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column0;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        internal System.Windows.Forms.Label lblTimeRemainingText;
        internal System.Windows.Forms.Label lblTimeRemainingValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
    }
}