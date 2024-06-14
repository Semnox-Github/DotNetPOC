namespace Parafait_Kiosk
{
    partial class CustomerDashboard
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerDashboard));
            this.dgvPurchases = new System.Windows.Forms.DataGridView();
            this.dateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creditsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountActivityDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dgvGamePlay = new System.Windows.Forms.DataGridView();
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
            this.lblEaterTickets = new System.Windows.Forms.Label();
            this.lblTicketsCourtesy = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.vScrollBarGp = new System.Windows.Forms.VScrollBar();
            this.lblTicketMode = new System.Windows.Forms.Label();
            this.lblSiteName = new System.Windows.Forms.Button();
            this.panelBalance = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelcard = new System.Windows.Forms.Panel();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.lblCard = new System.Windows.Forms.Label();
            this.panelTicket = new System.Windows.Forms.Panel();
            this.lblRealTicketMode = new System.Windows.Forms.Label();
            this.btnChangeTicketMode = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblTimeOut = new System.Windows.Forms.Button();
            this.btnTopUp = new System.Windows.Forms.Button();
            this.btnRegisterNew = new System.Windows.Forms.Button();
            this.GamePlayDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GameName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GamePricePlayPoints = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GamePriceBonus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TicketOrCourtesy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchases)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountActivityDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGamePlay)).BeginInit();
            this.grpActivities.SuspendLayout();
            this.panelPurchase.SuspendLayout();
            this.panelGameplay.SuspendLayout();
            this.panelBalance.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelcard.SuspendLayout();
            this.panelTicket.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrev
            // 
            this.btnPrev.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Font = new System.Drawing.Font("Bango Pro", 33F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(47, 1636);
            this.btnPrev.Size = new System.Drawing.Size(323, 167);
            this.btnPrev.TabIndex = 152;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // dgvPurchases
            // 
            this.dgvPurchases.AutoGenerateColumns = false;
            this.dgvPurchases.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPurchases.BackgroundColor = System.Drawing.Color.White;
            this.dgvPurchases.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPurchases.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.dgvPurchases.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchases.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPurchases.ColumnHeadersVisible = false;
            this.dgvPurchases.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dateDataGridViewTextBoxColumn,
            this.productDataGridViewTextBoxColumn,
            this.amountDataGridViewTextBoxColumn,
            this.creditsDataGridViewTextBoxColumn,
            this.timeDataGridViewTextBoxColumn});
            this.dgvPurchases.DataSource = this.accountActivityDTOBindingSource;
            this.dgvPurchases.EnableHeadersVisualStyles = false;
            this.dgvPurchases.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.dgvPurchases.Location = new System.Drawing.Point(4, 113);
            this.dgvPurchases.Name = "dgvPurchases";
            this.dgvPurchases.ReadOnly = true;
            this.dgvPurchases.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPurchases.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPurchases.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPurchases.RowTemplate.Height = 35;
            this.dgvPurchases.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPurchases.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvPurchases.Size = new System.Drawing.Size(936, 401);
            this.dgvPurchases.TabIndex = 35;
            // 
            // dateDataGridViewTextBoxColumn
            // 
            this.dateDataGridViewTextBoxColumn.DataPropertyName = "Date";
            this.dateDataGridViewTextBoxColumn.HeaderText = "Date";
            this.dateDataGridViewTextBoxColumn.MinimumWidth = 220;
            this.dateDataGridViewTextBoxColumn.Name = "dateDataGridViewTextBoxColumn";
            this.dateDataGridViewTextBoxColumn.ReadOnly = true;
            this.dateDataGridViewTextBoxColumn.Width = 220;
            // 
            // productDataGridViewTextBoxColumn
            // 
            this.productDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.productDataGridViewTextBoxColumn.DataPropertyName = "Product";
            this.productDataGridViewTextBoxColumn.HeaderText = "Product";
            this.productDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.productDataGridViewTextBoxColumn.Name = "productDataGridViewTextBoxColumn";
            this.productDataGridViewTextBoxColumn.ReadOnly = true;
            this.productDataGridViewTextBoxColumn.Width = 200;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            this.amountDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.amountDataGridViewTextBoxColumn.MinimumWidth = 169;
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            this.amountDataGridViewTextBoxColumn.ReadOnly = true;
            this.amountDataGridViewTextBoxColumn.Width = 169;
            // 
            // creditsDataGridViewTextBoxColumn
            // 
            this.creditsDataGridViewTextBoxColumn.DataPropertyName = "Credits";
            this.creditsDataGridViewTextBoxColumn.HeaderText = "Credits";
            this.creditsDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.creditsDataGridViewTextBoxColumn.Name = "creditsDataGridViewTextBoxColumn";
            this.creditsDataGridViewTextBoxColumn.ReadOnly = true;
            this.creditsDataGridViewTextBoxColumn.Width = 200;
            // 
            // timeDataGridViewTextBoxColumn
            // 
            this.timeDataGridViewTextBoxColumn.DataPropertyName = "Time";
            this.timeDataGridViewTextBoxColumn.HeaderText = "Time";
            this.timeDataGridViewTextBoxColumn.MinimumWidth = 152;
            this.timeDataGridViewTextBoxColumn.Name = "timeDataGridViewTextBoxColumn";
            this.timeDataGridViewTextBoxColumn.ReadOnly = true;
            this.timeDataGridViewTextBoxColumn.Width = 152;
            // 
            // accountActivityDTOBindingSource
            // 
            this.accountActivityDTOBindingSource.DataSource = typeof(Semnox.Parafait.Customer.Accounts.AccountActivityDTO);
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
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGamePlay.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvGamePlay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGamePlay.ColumnHeadersVisible = false;
            this.dgvGamePlay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GamePlayDate,
            this.GameName,
            this.GamePricePlayPoints,
            this.GamePriceBonus,
            this.TicketOrCourtesy});
            this.dgvGamePlay.EnableHeadersVisualStyles = false;
            this.dgvGamePlay.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.dgvGamePlay.Location = new System.Drawing.Point(6, 120);
            this.dgvGamePlay.Name = "dgvGamePlay";
            this.dgvGamePlay.ReadOnly = true;
            this.dgvGamePlay.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvGamePlay.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvGamePlay.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvGamePlay.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvGamePlay.Size = new System.Drawing.Size(932, 377);
            this.dgvGamePlay.TabIndex = 36;
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.Font = new System.Drawing.Font("Bango Pro", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(3, -10);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(930, 46);
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
            this.grpActivities.Location = new System.Drawing.Point(48, 407);
            this.grpActivities.Name = "grpActivities";
            this.grpActivities.Size = new System.Drawing.Size(989, 1206);
            this.grpActivities.TabIndex = 39;
            this.grpActivities.Visible = false;
            // 
            // panelPurchase
            // 
            this.panelPurchase.BackColor = System.Drawing.Color.Transparent;
            this.panelPurchase.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.product_table;
            this.panelPurchase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelPurchase.Controls.Add(this.label1);
            this.panelPurchase.Controls.Add(this.label11);
            this.panelPurchase.Controls.Add(this.label10);
            this.panelPurchase.Controls.Add(this.label9);
            this.panelPurchase.Controls.Add(this.label8);
            this.panelPurchase.Controls.Add(this.vScrollBarPur);
            this.panelPurchase.Controls.Add(this.label13);
            this.panelPurchase.Controls.Add(this.dgvPurchases);
            this.panelPurchase.Location = new System.Drawing.Point(4, 613);
            this.panelPurchase.Name = "panelPurchase";
            this.panelPurchase.Size = new System.Drawing.Size(981, 592);
            this.panelPurchase.TabIndex = 160;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bango Pro", 18F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label1.Location = new System.Drawing.Point(796, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 29);
            this.label1.TabIndex = 167;
            this.label1.Text = "Time";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Bango Pro", 18F);
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label11.Location = new System.Drawing.Point(605, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(178, 60);
            this.label11.TabIndex = 166;
            this.label11.Text = "Play Points";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Bango Pro", 18F);
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label10.Location = new System.Drawing.Point(443, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 29);
            this.label10.TabIndex = 165;
            this.label10.Text = "Amount";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label9.Location = new System.Drawing.Point(244, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 46);
            this.label9.TabIndex = 164;
            this.label9.Text = "Product";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label8.Location = new System.Drawing.Point(54, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 29);
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
            this.label13.Font = new System.Drawing.Font("Bango Pro", 21.75F);
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(3, 1);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(975, 40);
            this.label13.TabIndex = 159;
            this.label13.Text = "Purchases and Tasks";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelGameplay
            // 
            this.panelGameplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGameplay.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.game_price_table_plain;
            this.panelGameplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelGameplay.Controls.Add(this.lblEaterTickets);
            this.panelGameplay.Controls.Add(this.lblTicketsCourtesy);
            this.panelGameplay.Controls.Add(this.label6);
            this.panelGameplay.Controls.Add(this.label5);
            this.panelGameplay.Controls.Add(this.label4);
            this.panelGameplay.Controls.Add(this.label3);
            this.panelGameplay.Controls.Add(this.vScrollBarGp);
            this.panelGameplay.Controls.Add(this.label21);
            this.panelGameplay.Controls.Add(this.dgvGamePlay);
            this.panelGameplay.Location = new System.Drawing.Point(4, 19);
            this.panelGameplay.Name = "panelGameplay";
            this.panelGameplay.Size = new System.Drawing.Size(985, 589);
            this.panelGameplay.TabIndex = 158;
            // 
            // lblEaterTickets
            // 
            this.lblEaterTickets.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEaterTickets.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.lblEaterTickets.Location = new System.Drawing.Point(880, 55);
            this.lblEaterTickets.Name = "lblEaterTickets";
            this.lblEaterTickets.Size = new System.Drawing.Size(158, 74);
            this.lblEaterTickets.TabIndex = 168;
            this.lblEaterTickets.Text = "T.Eater Tickets";
            this.lblEaterTickets.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblEaterTickets.Visible = false;
            // 
            // lblTicketsCourtesy
            // 
            this.lblTicketsCourtesy.AutoSize = true;
            this.lblTicketsCourtesy.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketsCourtesy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.lblTicketsCourtesy.Location = new System.Drawing.Point(794, 65);
            this.lblTicketsCourtesy.Name = "lblTicketsCourtesy";
            this.lblTicketsCourtesy.Size = new System.Drawing.Size(118, 29);
            this.lblTicketsCourtesy.TabIndex = 166;
            this.lblTicketsCourtesy.Text = "Courtesy";
            this.lblTicketsCourtesy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label6.Location = new System.Drawing.Point(586, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(182, 74);
            this.label6.TabIndex = 165;
            this.label6.Text = "Game Price (Bonus)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label5.Location = new System.Drawing.Point(412, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(182, 74);
            this.label5.TabIndex = 164;
            this.label5.Text = "Game Price (Play Points)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label4.Location = new System.Drawing.Point(259, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 29);
            this.label4.TabIndex = 163;
            this.label4.Text = "Game";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bango Pro", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            this.label3.Location = new System.Drawing.Point(54, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 29);
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
            // lblTicketMode
            // 
            this.lblTicketMode.BackColor = System.Drawing.Color.Transparent;
            this.lblTicketMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTicketMode.Font = new System.Drawing.Font("Bango Pro", 33F);
            this.lblTicketMode.ForeColor = System.Drawing.Color.White;
            this.lblTicketMode.Location = new System.Drawing.Point(0, 4);
            this.lblTicketMode.Name = "lblTicketMode";
            this.lblTicketMode.Size = new System.Drawing.Size(476, 58);
            this.lblTicketMode.TabIndex = 155;
            this.lblTicketMode.Text = "Ticket Mode:";
            this.lblTicketMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.lblSiteName.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.White;
            this.lblSiteName.Location = new System.Drawing.Point(0, 0);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(1079, 77);
            this.lblSiteName.TabIndex = 142;
            this.lblSiteName.Text = "Site Name";
            this.lblSiteName.UseVisualStyleBackColor = false;
            // 
            // panelBalance
            // 
            this.panelBalance.BackColor = System.Drawing.Color.Transparent;
            this.panelBalance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBalance.Controls.Add(this.flowLayoutPanel1);
            this.panelBalance.Font = new System.Drawing.Font("Bango Pro", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelBalance.ForeColor = System.Drawing.Color.White;
            this.panelBalance.Location = new System.Drawing.Point(13, 267);
            this.panelBalance.Name = "panelBalance";
            this.panelBalance.Size = new System.Drawing.Size(1028, 137);
            this.panelBalance.TabIndex = 153;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panelcard);
            this.flowLayoutPanel1.Controls.Add(this.panelTicket);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(13, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(980, 134);
            this.flowLayoutPanel1.TabIndex = 158;
            // 
            // panelcard
            // 
            this.panelcard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelcard.Controls.Add(this.lblCardNumber);
            this.panelcard.Controls.Add(this.lblCard);
            this.panelcard.Location = new System.Drawing.Point(0, 0);
            this.panelcard.Margin = new System.Windows.Forms.Padding(0);
            this.panelcard.Name = "panelcard";
            this.panelcard.Size = new System.Drawing.Size(991, 62);
            this.panelcard.TabIndex = 154;
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblCardNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCardNumber.Font = new System.Drawing.Font("Bango Pro", 33F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNumber.ForeColor = System.Drawing.Color.White;
            this.lblCardNumber.Location = new System.Drawing.Point(3, 2);
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
            this.lblCard.Font = new System.Drawing.Font("Bango Pro", 33F);
            this.lblCard.ForeColor = System.Drawing.Color.White;
            this.lblCard.Location = new System.Drawing.Point(473, 6);
            this.lblCard.Name = "lblCard";
            this.lblCard.Size = new System.Drawing.Size(444, 54);
            this.lblCard.TabIndex = 151;
            this.lblCard.Text = "00000";
            this.lblCard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelTicket
            // 
            this.panelTicket.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelTicket.Controls.Add(this.lblRealTicketMode);
            this.panelTicket.Controls.Add(this.btnChangeTicketMode);
            this.panelTicket.Controls.Add(this.panel6);
            this.panelTicket.Controls.Add(this.lblTicketMode);
            this.panelTicket.Location = new System.Drawing.Point(0, 62);
            this.panelTicket.Margin = new System.Windows.Forms.Padding(0);
            this.panelTicket.Name = "panelTicket";
            this.panelTicket.Size = new System.Drawing.Size(991, 65);
            this.panelTicket.TabIndex = 158;
            // 
            // lblRealTicketMode
            // 
            this.lblRealTicketMode.BackColor = System.Drawing.Color.Transparent;
            this.lblRealTicketMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRealTicketMode.Font = new System.Drawing.Font("Bango Pro", 33F);
            this.lblRealTicketMode.ForeColor = System.Drawing.Color.White;
            this.lblRealTicketMode.Location = new System.Drawing.Point(476, 1);
            this.lblRealTicketMode.Name = "lblRealTicketMode";
            this.lblRealTicketMode.Size = new System.Drawing.Size(296, 54);
            this.lblRealTicketMode.TabIndex = 169;
            this.lblRealTicketMode.Text = "Real Ticket";
            this.lblRealTicketMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnChangeTicketMode
            // 
            this.btnChangeTicketMode.BackColor = System.Drawing.Color.Transparent;
            this.btnChangeTicketMode.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.BtnChange;
            this.btnChangeTicketMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnChangeTicketMode.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
            this.btnChangeTicketMode.FlatAppearance.BorderSize = 0;
            this.btnChangeTicketMode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChangeTicketMode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChangeTicketMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeTicketMode.Font = new System.Drawing.Font("Bango Pro", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeTicketMode.ForeColor = System.Drawing.Color.White;
            this.btnChangeTicketMode.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnChangeTicketMode.Location = new System.Drawing.Point(773, 4);
            this.btnChangeTicketMode.Name = "btnChangeTicketMode";
            this.btnChangeTicketMode.Size = new System.Drawing.Size(180, 56);
            this.btnChangeTicketMode.TabIndex = 168;
            this.btnChangeTicketMode.Text = "Change";
            this.btnChangeTicketMode.UseVisualStyleBackColor = false;
            this.btnChangeTicketMode.Click += new System.EventHandler(this.btnChangeTicketMode_Click);
            // 
            // panel6
            // 
            this.panel6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel6.Controls.Add(this.label12);
            this.panel6.Controls.Add(this.label14);
            this.panel6.Location = new System.Drawing.Point(0, 64);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(991, 59);
            this.panel6.TabIndex = 158;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label12.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(4, -3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(469, 58);
            this.label12.TabIndex = 155;
            this.label12.Text = "Time Remaining:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label14.Font = new System.Drawing.Font("Bango Pro", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(473, 4);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(444, 52);
            this.label14.TabIndex = 156;
            this.label14.Text = "00000";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTimeOut
            // 
            this.lblTimeOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeOut.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.timer_btn1;
            this.lblTimeOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lblTimeOut.FlatAppearance.BorderSize = 0;
            this.lblTimeOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblTimeOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTimeOut.Font = new System.Drawing.Font("Verdana", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeOut.ForeColor = System.Drawing.Color.White;
            this.lblTimeOut.Location = new System.Drawing.Point(853, 150);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(157, 118);
            this.lblTimeOut.TabIndex = 159;
            this.lblTimeOut.UseVisualStyleBackColor = false;
            // 
            // btnTopUp
            // 
            this.btnTopUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTopUp.BackColor = System.Drawing.Color.Transparent;
            this.btnTopUp.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnTopUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnTopUp.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnTopUp.FlatAppearance.BorderSize = 0;
            this.btnTopUp.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTopUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTopUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTopUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTopUp.Font = new System.Drawing.Font("Bango Pro", 32F);
            this.btnTopUp.ForeColor = System.Drawing.Color.White;
            this.btnTopUp.Location = new System.Drawing.Point(382, 1636);
            this.btnTopUp.Name = "btnTopUp";
            this.btnTopUp.Size = new System.Drawing.Size(323, 167);
            this.btnTopUp.TabIndex = 154;
            this.btnTopUp.Text = "Top Up";
            this.btnTopUp.UseVisualStyleBackColor = false;
            this.btnTopUp.Visible = false;
            this.btnTopUp.Click += new System.EventHandler(this.btnRecharge_Click);
            // 
            // btnRegisterNew
            // 
            this.btnRegisterNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegisterNew.BackColor = System.Drawing.Color.Transparent;
            this.btnRegisterNew.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnRegisterNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRegisterNew.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnRegisterNew.FlatAppearance.BorderSize = 0;
            this.btnRegisterNew.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRegisterNew.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRegisterNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRegisterNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegisterNew.Font = new System.Drawing.Font("Bango Pro", 33F);
            this.btnRegisterNew.ForeColor = System.Drawing.Color.White;
            this.btnRegisterNew.Location = new System.Drawing.Point(717, 1636);
            this.btnRegisterNew.Name = "btnRegisterNew";
            this.btnRegisterNew.Size = new System.Drawing.Size(323, 167);
            this.btnRegisterNew.TabIndex = 155;
            this.btnRegisterNew.Text = "Register";
            this.btnRegisterNew.UseVisualStyleBackColor = false;
            this.btnRegisterNew.Visible = false;
            this.btnRegisterNew.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // GamePlayDate
            // 
            this.GamePlayDate.HeaderText = "GamePlayDate";
            this.GamePlayDate.MinimumWidth = 205;
            this.GamePlayDate.Name = "GamePlayDate";
            this.GamePlayDate.ReadOnly = true;
            this.GamePlayDate.Width = 205;
            // 
            // GameName
            // 
            this.GameName.HeaderText = "Game Name";
            this.GameName.MinimumWidth = 197;
            this.GameName.Name = "GameName";
            this.GameName.ReadOnly = true;
            this.GameName.Width = 197;
            // 
            // GamePricePlayPoints
            // 
            this.GamePricePlayPoints.HeaderText = "Game Price(PlayPoints)";
            this.GamePricePlayPoints.MinimumWidth = 177;
            this.GamePricePlayPoints.Name = "GamePricePlayPoints";
            this.GamePricePlayPoints.ReadOnly = true;
            this.GamePricePlayPoints.Width = 177;
            // 
            // GamePriceBonus
            // 
            this.GamePriceBonus.HeaderText = "Game Price(Bonus)";
            this.GamePriceBonus.MinimumWidth = 177;
            this.GamePriceBonus.Name = "GamePriceBonus";
            this.GamePriceBonus.ReadOnly = true;
            this.GamePriceBonus.Width = 177;
            // 
            // TicketOrCourtesy
            // 
            this.TicketOrCourtesy.HeaderText = "Ticket/Courtesy";
            this.TicketOrCourtesy.MinimumWidth = 177;
            this.TicketOrCourtesy.Name = "TicketOrCourtesy";
            this.TicketOrCourtesy.ReadOnly = true;
            this.TicketOrCourtesy.Width = 177;
            // 
            // CustomerDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Home_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 1920);
            this.Controls.Add(this.lblTimeOut);
            this.Controls.Add(this.grpActivities);
            this.Controls.Add(this.btnTopUp);
            this.Controls.Add(this.btnRegisterNew);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.panelBalance);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "CustomerDashboard";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parafait Customer Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CustomerDashboard_FormClosed);
            this.Load += new System.EventHandler(this.CustomerDashboard_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Dashboard_KeyPress);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.panelBalance, 0);
            this.Controls.SetChildIndex(this.lblSiteName, 0);
            this.Controls.SetChildIndex(this.btnRegisterNew, 0);
            this.Controls.SetChildIndex(this.btnTopUp, 0);
            this.Controls.SetChildIndex(this.grpActivities, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.Controls.SetChildIndex(this.lblTimeOut, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchases)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountActivityDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGamePlay)).EndInit();
            this.grpActivities.ResumeLayout(false);
            this.panelPurchase.ResumeLayout(false);
            this.panelPurchase.PerformLayout();
            this.panelGameplay.ResumeLayout(false);
            this.panelGameplay.PerformLayout();
            this.panelBalance.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panelcard.ResumeLayout(false);
            this.panelTicket.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPurchases;
        private System.Windows.Forms.DataGridView dgvGamePlay;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Panel grpActivities;
        private System.Windows.Forms.Button lblSiteName;
        //private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Panel panelBalance;
        private System.Windows.Forms.Button btnTopUp;
        private System.Windows.Forms.Button btnRegisterNew;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panelGameplay;
        private System.Windows.Forms.Panel panelPurchase;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.VScrollBar vScrollBarPur;
        private System.Windows.Forms.VScrollBar vScrollBarGp;
        private System.Windows.Forms.Button lblTimeOut;
        private System.Windows.Forms.Panel panelcard;
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblEaterTickets;
        internal System.Windows.Forms.Label lblTicketMode;
        private System.Windows.Forms.Panel panelTicket;
        private System.Windows.Forms.Panel panel6;
        internal System.Windows.Forms.Label label12;
        internal System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnChangeTicketMode;
        internal System.Windows.Forms.Label lblRealTicketMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creditsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource accountActivityDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn GamePlayDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn GameName;
        private System.Windows.Forms.DataGridViewTextBoxColumn GamePricePlayPoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn GamePriceBonus;
        private System.Windows.Forms.DataGridViewTextBoxColumn TicketOrCourtesy;
    }
}
