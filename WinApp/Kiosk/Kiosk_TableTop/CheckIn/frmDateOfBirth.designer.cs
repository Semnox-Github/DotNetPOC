namespace Parafait_Kiosk
{
    partial class frmDateOfBirth
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDateOfBirth));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flpCalender = new System.Windows.Forms.FlowLayoutPanel();
            this.flpDay = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDay = new System.Windows.Forms.Label();
            this.lblScrollImageDay = new System.Windows.Forms.Label();
            this.flpMonth = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMonth = new System.Windows.Forms.Label();
            this.lblScrollImageMonth = new System.Windows.Forms.Label();
            this.flpYear = new System.Windows.Forms.FlowLayoutPanel();
            this.lblYear = new System.Windows.Forms.Label();
            this.lblScrollImageYear = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvYear = new System.Windows.Forms.DataGridView();
            this.dgvTextBoxColumnYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDay = new System.Windows.Forms.DataGridView();
            this.dgvTextBoxColumnDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelCalenderContents = new System.Windows.Forms.Panel();
            this.vScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.dgvMonth = new System.Windows.Forms.DataGridView();
            this.dgvTextBoxColumnMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelCalenderHeader = new System.Windows.Forms.Panel();
            this.lblYearHeader = new System.Windows.Forms.Label();
            this.lblMonthHeader = new System.Windows.Forms.Label();
            this.lblDayHeader = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Button();
            this.lblMsg = new System.Windows.Forms.Button();
            this.flpCalender.SuspendLayout();
            this.flpDay.SuspendLayout();
            this.flpMonth.SuspendLayout();
            this.flpYear.SuspendLayout();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDay)).BeginInit();
            this.panelCalenderContents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonth)).BeginInit();
            this.panelCalenderHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHome.Visible = false;
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(190, 440);
            this.btnPrev.Size = new System.Drawing.Size(330, 167);
            this.btnPrev.Text = "Cancel";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.PowderBlue;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(31, 226);
            this.btnCancel.Size = new System.Drawing.Size(43, 21);
            // 
            // flpCalender
            // 
            this.flpCalender.AutoSize = true;
            this.flpCalender.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpCalender.BackColor = System.Drawing.Color.Transparent;
            this.flpCalender.Controls.Add(this.flpDay);
            this.flpCalender.Controls.Add(this.flpMonth);
            this.flpCalender.Controls.Add(this.flpYear);
            this.flpCalender.Location = new System.Drawing.Point(223, 299);
            this.flpCalender.Name = "flpCalender";
            this.flpCalender.Size = new System.Drawing.Size(624, 51);
            this.flpCalender.TabIndex = 20016;
            // 
            // flpDay
            // 
            this.flpDay.AutoSize = true;
            this.flpDay.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpDay.BackColor = System.Drawing.Color.Transparent;
            this.flpDay.Controls.Add(this.lblDay);
            this.flpDay.Controls.Add(this.lblScrollImageDay);
            this.flpDay.Location = new System.Drawing.Point(3, 3);
            this.flpDay.Name = "flpDay";
            this.flpDay.Size = new System.Drawing.Size(202, 45);
            this.flpDay.TabIndex = 20021;
            // 
            // lblDay
            // 
            this.lblDay.BackColor = System.Drawing.Color.White;
            this.lblDay.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDay.ForeColor = System.Drawing.Color.White;
            this.lblDay.Location = new System.Drawing.Point(3, 0);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(137, 45);
            this.lblDay.TabIndex = 0;
            this.lblDay.Text = "Select";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDay.Click += new System.EventHandler(this.lblDay_Click);
            // 
            // lblScrollImageDay
            // 
            this.lblScrollImageDay.AutoSize = true;
            this.lblScrollImageDay.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScrollImageDay.Image = global::Parafait_Kiosk.Properties.Resources.down_button;
            this.lblScrollImageDay.Location = new System.Drawing.Point(146, 0);
            this.lblScrollImageDay.Name = "lblScrollImageDay";
            this.lblScrollImageDay.Size = new System.Drawing.Size(53, 45);
            this.lblScrollImageDay.TabIndex = 1;
            this.lblScrollImageDay.Text = "   ";
            this.lblScrollImageDay.Click += new System.EventHandler(this.lblDay_Click);
            // 
            // flpMonth
            // 
            this.flpMonth.AutoSize = true;
            this.flpMonth.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpMonth.BackColor = System.Drawing.Color.Transparent;
            this.flpMonth.Controls.Add(this.lblMonth);
            this.flpMonth.Controls.Add(this.lblScrollImageMonth);
            this.flpMonth.Location = new System.Drawing.Point(211, 3);
            this.flpMonth.Name = "flpMonth";
            this.flpMonth.Size = new System.Drawing.Size(202, 45);
            this.flpMonth.TabIndex = 20022;
            // 
            // lblMonth
            // 
            this.lblMonth.BackColor = System.Drawing.Color.White;
            this.lblMonth.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonth.ForeColor = System.Drawing.Color.White;
            this.lblMonth.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMonth.Location = new System.Drawing.Point(3, 0);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(137, 45);
            this.lblMonth.TabIndex = 1;
            this.lblMonth.Text = "Select";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMonth.Click += new System.EventHandler(this.lblMonth_Click);
            // 
            // lblScrollImageMonth
            // 
            this.lblScrollImageMonth.AutoSize = true;
            this.lblScrollImageMonth.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScrollImageMonth.Image = global::Parafait_Kiosk.Properties.Resources.down_button;
            this.lblScrollImageMonth.Location = new System.Drawing.Point(146, 0);
            this.lblScrollImageMonth.Name = "lblScrollImageMonth";
            this.lblScrollImageMonth.Size = new System.Drawing.Size(53, 45);
            this.lblScrollImageMonth.TabIndex = 1;
            this.lblScrollImageMonth.Text = "   ";
            this.lblScrollImageMonth.Click += new System.EventHandler(this.lblMonth_Click);
            // 
            // flpYear
            // 
            this.flpYear.AutoSize = true;
            this.flpYear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpYear.BackColor = System.Drawing.Color.Transparent;
            this.flpYear.Controls.Add(this.lblYear);
            this.flpYear.Controls.Add(this.lblScrollImageYear);
            this.flpYear.Location = new System.Drawing.Point(419, 3);
            this.flpYear.Name = "flpYear";
            this.flpYear.Size = new System.Drawing.Size(202, 45);
            this.flpYear.TabIndex = 20023;
            // 
            // lblYear
            // 
            this.lblYear.BackColor = System.Drawing.Color.White;
            this.lblYear.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYear.ForeColor = System.Drawing.Color.White;
            this.lblYear.Location = new System.Drawing.Point(3, 0);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(137, 45);
            this.lblYear.TabIndex = 2;
            this.lblYear.Text = "Select";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblYear.Click += new System.EventHandler(this.lblYear_Click);
            // 
            // lblScrollImageYear
            // 
            this.lblScrollImageYear.AutoSize = true;
            this.lblScrollImageYear.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScrollImageYear.Image = global::Parafait_Kiosk.Properties.Resources.down_button;
            this.lblScrollImageYear.Location = new System.Drawing.Point(146, 0);
            this.lblScrollImageYear.Name = "lblScrollImageYear";
            this.lblScrollImageYear.Size = new System.Drawing.Size(53, 45);
            this.lblScrollImageYear.TabIndex = 1;
            this.lblScrollImageYear.Text = "   ";
            this.lblScrollImageYear.Click += new System.EventHandler(this.lblYear_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(186, 440);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(701, 170);
            this.panelButtons.TabIndex = 20018;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.Back_button_box;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Gotham Rounded Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(365, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(333, 167);
            this.btnSave.TabIndex = 1025;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvYear
            // 
            this.dgvYear.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvYear.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvYear.ColumnHeadersVisible = false;
            this.dgvYear.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTextBoxColumnYear});
            this.dgvYear.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvYear.Location = new System.Drawing.Point(-1, 0);
            this.dgvYear.MultiSelect = false;
            this.dgvYear.Name = "dgvYear";
            this.dgvYear.RowHeadersVisible = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvYear.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvYear.RowTemplate.Height = 60;
            this.dgvYear.RowTemplate.ReadOnly = true;
            this.dgvYear.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvYear.Size = new System.Drawing.Size(128, 246);
            this.dgvYear.TabIndex = 20022;
            this.dgvYear.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvYear_CellClick);
            // 
            // dgvTextBoxColumnYear
            // 
            this.dgvTextBoxColumnYear.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvTextBoxColumnYear.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTextBoxColumnYear.HeaderText = "Year";
            this.dgvTextBoxColumnYear.Name = "dgvTextBoxColumnYear";
            this.dgvTextBoxColumnYear.ReadOnly = true;
            this.dgvTextBoxColumnYear.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dgvDay
            // 
            this.dgvDay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDay.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvDay.ColumnHeadersVisible = false;
            this.dgvDay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTextBoxColumnDay});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDay.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDay.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvDay.Location = new System.Drawing.Point(0, 0);
            this.dgvDay.MultiSelect = false;
            this.dgvDay.Name = "dgvDay";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDay.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDay.RowHeadersVisible = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvDay.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDay.RowTemplate.Height = 60;
            this.dgvDay.RowTemplate.ReadOnly = true;
            this.dgvDay.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvDay.Size = new System.Drawing.Size(115, 246);
            this.dgvDay.TabIndex = 132;
            this.dgvDay.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDay_CellClick);
            // 
            // dgvTextBoxColumnDay
            // 
            this.dgvTextBoxColumnDay.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvTextBoxColumnDay.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTextBoxColumnDay.HeaderText = "Day";
            this.dgvTextBoxColumnDay.Name = "dgvTextBoxColumnDay";
            this.dgvTextBoxColumnDay.ReadOnly = true;
            this.dgvTextBoxColumnDay.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // panelCalenderContents
            // 
            this.panelCalenderContents.BackColor = System.Drawing.Color.Transparent;
            this.panelCalenderContents.Controls.Add(this.dgvYear);
            this.panelCalenderContents.Controls.Add(this.vScrollBar);
            this.panelCalenderContents.Controls.Add(this.dgvMonth);
            this.panelCalenderContents.Controls.Add(this.dgvDay);
            this.panelCalenderContents.Location = new System.Drawing.Point(282, 258);
            this.panelCalenderContents.Name = "panelCalenderContents";
            this.panelCalenderContents.Size = new System.Drawing.Size(199, 246);
            this.panelCalenderContents.TabIndex = 20020;
            // 
            // vScrollBar
            // 
            this.vScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar.AutoHide = true;
            this.vScrollBar.BackColor = System.Drawing.SystemColors.Control;
            this.vScrollBar.DataGridView = null;
            this.vScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBar.DownButtonBackgroundImage")));
            this.vScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBar.DownButtonDisabledBackgroundImage")));
            this.vScrollBar.Location = new System.Drawing.Point(128, 0);
            this.vScrollBar.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.ScrollableControl = null;
            this.vScrollBar.ScrollViewer = null;
            this.vScrollBar.Size = new System.Drawing.Size(68, 246);
            this.vScrollBar.TabIndex = 20021;
            this.vScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBar.UpButtonBackgroundImage")));
            this.vScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("vScrollBar.UpButtonDisabledBackgroundImage")));
            this.vScrollBar.Click += new System.EventHandler(this.vScrollBar_Click);
            // 
            // dgvMonth
            // 
            this.dgvMonth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMonth.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvMonth.ColumnHeadersVisible = false;
            this.dgvMonth.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTextBoxColumnMonth});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Gotham Rounded Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMonth.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvMonth.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvMonth.Location = new System.Drawing.Point(-1, 0);
            this.dgvMonth.MultiSelect = false;
            this.dgvMonth.Name = "dgvMonth";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMonth.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvMonth.RowHeadersVisible = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Gotham Rounded Bold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvMonth.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvMonth.RowTemplate.Height = 60;
            this.dgvMonth.RowTemplate.ReadOnly = true;
            this.dgvMonth.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvMonth.Size = new System.Drawing.Size(116, 246);
            this.dgvMonth.TabIndex = 20021;
            this.dgvMonth.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMonth_CellClick);
            // 
            // dgvTextBoxColumnMonth
            // 
            this.dgvTextBoxColumnMonth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvTextBoxColumnMonth.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvTextBoxColumnMonth.HeaderText = "Month";
            this.dgvTextBoxColumnMonth.Name = "dgvTextBoxColumnMonth";
            this.dgvTextBoxColumnMonth.ReadOnly = true;
            this.dgvTextBoxColumnMonth.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Day";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Month";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Year";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // panelCalenderHeader
            // 
            this.panelCalenderHeader.BackColor = System.Drawing.Color.Transparent;
            this.panelCalenderHeader.Controls.Add(this.lblYearHeader);
            this.panelCalenderHeader.Controls.Add(this.lblMonthHeader);
            this.panelCalenderHeader.Controls.Add(this.lblDayHeader);
            this.panelCalenderHeader.Location = new System.Drawing.Point(225, 247);
            this.panelCalenderHeader.Name = "panelCalenderHeader";
            this.panelCalenderHeader.Size = new System.Drawing.Size(624, 54);
            this.panelCalenderHeader.TabIndex = 20021;
            // 
            // lblYearHeader
            // 
            this.lblYearHeader.AutoSize = true;
            this.lblYearHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYearHeader.ForeColor = System.Drawing.Color.White;
            this.lblYearHeader.Location = new System.Drawing.Point(423, 3);
            this.lblYearHeader.Name = "lblYearHeader";
            this.lblYearHeader.Size = new System.Drawing.Size(103, 45);
            this.lblYearHeader.TabIndex = 2;
            this.lblYearHeader.Text = "Year";
            // 
            // lblMonthHeader
            // 
            this.lblMonthHeader.AutoSize = true;
            this.lblMonthHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonthHeader.ForeColor = System.Drawing.Color.White;
            this.lblMonthHeader.Location = new System.Drawing.Point(215, 3);
            this.lblMonthHeader.Name = "lblMonthHeader";
            this.lblMonthHeader.Size = new System.Drawing.Size(138, 45);
            this.lblMonthHeader.TabIndex = 1;
            this.lblMonthHeader.Text = "Month";
            // 
            // lblDayHeader
            // 
            this.lblDayHeader.AutoSize = true;
            this.lblDayHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDayHeader.ForeColor = System.Drawing.Color.White;
            this.lblDayHeader.Location = new System.Drawing.Point(3, 3);
            this.lblDayHeader.Name = "lblDayHeader";
            this.lblDayHeader.Size = new System.Drawing.Size(93, 45);
            this.lblDayHeader.TabIndex = 0;
            this.lblDayHeader.Text = "Day";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoEllipsis = true;
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblHeader.FlatAppearance.BorderSize = 0;
            this.lblHeader.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblHeader.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblHeader.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(31, 56);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(966, 91);
            this.lblHeader.TabIndex = 20022;
            this.lblHeader.Text = "Heading";
            this.lblHeader.UseVisualStyleBackColor = false;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoEllipsis = true;
            this.lblMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblMsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblMsg.FlatAppearance.BorderSize = 0;
            this.lblMsg.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.lblMsg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.lblMsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMsg.Font = new System.Drawing.Font("Gotham Rounded Bold", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.White;
            this.lblMsg.Location = new System.Drawing.Point(31, 153);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(966, 91);
            this.lblMsg.TabIndex = 13;
            this.lblMsg.Text = "Message";
            this.lblMsg.UseVisualStyleBackColor = false;
            // 
            // frmDateOfBirth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.tap_card_box;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(984, 637);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.panelCalenderContents);
            this.Controls.Add(this.panelCalenderHeader);
            this.Controls.Add(this.flpCalender);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.lblMsg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmDateOfBirth";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmYesNo";
            this.TransparencyKey = System.Drawing.Color.Blue;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEdirBirthYear_FormClosing);
            this.Load += new System.EventHandler(this.frmEditBirthYear_Load);
            this.Controls.SetChildIndex(this.lblMsg, 0);
            this.Controls.SetChildIndex(this.panelButtons, 0);
            this.Controls.SetChildIndex(this.flpCalender, 0);
            this.Controls.SetChildIndex(this.panelCalenderHeader, 0);
            this.Controls.SetChildIndex(this.panelCalenderContents, 0);
            this.Controls.SetChildIndex(this.lblHeader, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnHome, 0);
            this.Controls.SetChildIndex(this.btnPrev, 0);
            this.flpCalender.ResumeLayout(false);
            this.flpCalender.PerformLayout();
            this.flpDay.ResumeLayout(false);
            this.flpDay.PerformLayout();
            this.flpMonth.ResumeLayout(false);
            this.flpMonth.PerformLayout();
            this.flpYear.ResumeLayout(false);
            this.flpYear.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDay)).EndInit();
            this.panelCalenderContents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonth)).EndInit();
            this.panelCalenderHeader.ResumeLayout(false);
            this.panelCalenderHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.FlowLayoutPanel flpCalender;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.DataGridView dgvDay;
        private System.Windows.Forms.Panel panelCalenderContents;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridView dgvMonth;
        private System.Windows.Forms.DataGridView dgvYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTextBoxColumnDay;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.FlowLayoutPanel flpDay;
        private System.Windows.Forms.Label lblScrollImageDay;
        private System.Windows.Forms.FlowLayoutPanel flpMonth;
        private System.Windows.Forms.Label lblScrollImageMonth;
        private System.Windows.Forms.FlowLayoutPanel flpYear;
        private System.Windows.Forms.Label lblScrollImageYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTextBoxColumnMonth;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView vScrollBar;
        private System.Windows.Forms.Panel panelCalenderHeader;
        private System.Windows.Forms.Label lblDayHeader;
        private System.Windows.Forms.Label lblMonthHeader;
        private System.Windows.Forms.Label lblYearHeader;
        private System.Windows.Forms.Button lblHeader;
        private System.Windows.Forms.Button lblMsg;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTextBoxColumnYear;
    }
}
