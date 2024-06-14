namespace Semnox.Parafait.Device.Turnstile
{
    partial class frmShowTurnstiles
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel4 = new System.Windows.Forms.GroupBox();
            this.cmbProfile = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tcTurnstiles = new System.Windows.Forms.TabControl();
            this.tpStatus = new System.Windows.Forms.TabPage();
            this.dgvTurnstiles = new System.Windows.Forms.DataGridView();
            this.textBoxEvents = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.GroupBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTypeMake = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.labelTurn = new System.Windows.Forms.Label();
            this.txtTurnPort = new System.Windows.Forms.TextBox();
            this.txtTurnIp = new System.Windows.Forms.TextBox();
            this.buttonPanic = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.GroupBox();
            this.lblPassageBAlias = new System.Windows.Forms.Label();
            this.lblPassageAAlias = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_lockB = new System.Windows.Forms.Button();
            this.button_lockA = new System.Windows.Forms.Button();
            this.button_FreeB = new System.Windows.Forms.Button();
            this.button_FreeA = new System.Windows.Forms.Button();
            this.button_singleB = new System.Windows.Forms.Button();
            this.button_singleA = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.lnkClearLog = new System.Windows.Forms.LinkLabel();
            this.panel4.SuspendLayout();
            this.tcTurnstiles.SuspendLayout();
            this.tpStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnstiles)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.cmbProfile);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.tcTurnstiles);
            this.panel4.Location = new System.Drawing.Point(2, 6);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(613, 680);
            this.panel4.TabIndex = 0;
            this.panel4.TabStop = false;
            this.panel4.Text = "Turnstiles";
            // 
            // cmbProfile
            // 
            this.cmbProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProfile.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbProfile.FormattingEnabled = true;
            this.cmbProfile.Location = new System.Drawing.Point(216, 20);
            this.cmbProfile.Name = "cmbProfile";
            this.cmbProfile.Size = new System.Drawing.Size(392, 40);
            this.cmbProfile.TabIndex = 6;
            this.cmbProfile.SelectedIndexChanged += new System.EventHandler(this.cmbProfile_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(163, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Profile:";
            // 
            // tcTurnstiles
            // 
            this.tcTurnstiles.Controls.Add(this.tpStatus);
            this.tcTurnstiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTurnstiles.ItemSize = new System.Drawing.Size(82, 40);
            this.tcTurnstiles.Location = new System.Drawing.Point(3, 18);
            this.tcTurnstiles.Name = "tcTurnstiles";
            this.tcTurnstiles.SelectedIndex = 0;
            this.tcTurnstiles.Size = new System.Drawing.Size(607, 659);
            this.tcTurnstiles.TabIndex = 7;
            // 
            // tpStatus
            // 
            this.tpStatus.Controls.Add(this.dgvTurnstiles);
            this.tpStatus.Location = new System.Drawing.Point(4, 44);
            this.tpStatus.Name = "tpStatus";
            this.tpStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tpStatus.Size = new System.Drawing.Size(599, 611);
            this.tpStatus.TabIndex = 0;
            this.tpStatus.Text = "View Status";
            this.tpStatus.UseVisualStyleBackColor = true;
            // 
            // dgvTurnstiles
            // 
            this.dgvTurnstiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTurnstiles.BackgroundColor = System.Drawing.Color.White;
            this.dgvTurnstiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTurnstiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTurnstiles.Location = new System.Drawing.Point(2, 3);
            this.dgvTurnstiles.Name = "dgvTurnstiles";
            this.dgvTurnstiles.Size = new System.Drawing.Size(593, 583);
            this.dgvTurnstiles.TabIndex = 0;
            // 
            // textBoxEvents
            // 
            this.textBoxEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEvents.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEvents.Location = new System.Drawing.Point(621, 492);
            this.textBoxEvents.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxEvents.Multiline = true;
            this.textBoxEvents.Name = "textBoxEvents";
            this.textBoxEvents.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxEvents.Size = new System.Drawing.Size(451, 191);
            this.textBoxEvents.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.chkActive);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtTypeMake);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.labelTurn);
            this.panel1.Controls.Add(this.txtTurnPort);
            this.panel1.Controls.Add(this.txtTurnIp);
            this.panel1.Location = new System.Drawing.Point(621, 6);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(451, 116);
            this.panel1.TabIndex = 2;
            this.panel1.TabStop = false;
            this.panel1.Text = "Properties";
            // 
            // chkActive
            // 
            this.chkActive.AutoCheck = false;
            this.chkActive.AutoSize = true;
            this.chkActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(362, 78);
            this.chkActive.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(62, 20);
            this.chkActive.TabIndex = 13;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "Type / Make:";
            // 
            // txtTypeMake
            // 
            this.txtTypeMake.BackColor = System.Drawing.Color.LightGray;
            this.txtTypeMake.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTypeMake.Location = new System.Drawing.Point(115, 47);
            this.txtTypeMake.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTypeMake.Name = "txtTypeMake";
            this.txtTypeMake.ReadOnly = true;
            this.txtTypeMake.Size = new System.Drawing.Size(309, 22);
            this.txtTypeMake.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.LightGray;
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Location = new System.Drawing.Point(115, 19);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(309, 22);
            this.txtName.TabIndex = 9;
            // 
            // labelTurn
            // 
            this.labelTurn.AutoSize = true;
            this.labelTurn.Location = new System.Drawing.Point(4, 78);
            this.labelTurn.Name = "labelTurn";
            this.labelTurn.Size = new System.Drawing.Size(109, 16);
            this.labelTurn.TabIndex = 4;
            this.labelTurn.Text = "Turnsile IP / Port:";
            // 
            // txtTurnPort
            // 
            this.txtTurnPort.BackColor = System.Drawing.Color.LightGray;
            this.txtTurnPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTurnPort.Location = new System.Drawing.Point(228, 75);
            this.txtTurnPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTurnPort.Name = "txtTurnPort";
            this.txtTurnPort.ReadOnly = true;
            this.txtTurnPort.Size = new System.Drawing.Size(43, 22);
            this.txtTurnPort.TabIndex = 1;
            this.txtTurnPort.Text = "8888";
            // 
            // txtTurnIp
            // 
            this.txtTurnIp.BackColor = System.Drawing.Color.LightGray;
            this.txtTurnIp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTurnIp.Location = new System.Drawing.Point(115, 75);
            this.txtTurnIp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTurnIp.Name = "txtTurnIp";
            this.txtTurnIp.ReadOnly = true;
            this.txtTurnIp.Size = new System.Drawing.Size(112, 22);
            this.txtTurnIp.TabIndex = 0;
            this.txtTurnIp.Text = "192.168.189.988";
            // 
            // buttonPanic
            // 
            this.buttonPanic.BackColor = System.Drawing.Color.Transparent;
            this.buttonPanic.BackgroundImage = global::Semnox.Parafait.Device.Turnstile.TurnstileResource.YellowgreenMenuButton;
            this.buttonPanic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonPanic.FlatAppearance.BorderSize = 0;
            this.buttonPanic.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.buttonPanic.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPanic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPanic.ForeColor = System.Drawing.Color.White;
            this.buttonPanic.Location = new System.Drawing.Point(139, 249);
            this.buttonPanic.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonPanic.Name = "buttonPanic";
            this.buttonPanic.Size = new System.Drawing.Size(177, 75);
            this.buttonPanic.TabIndex = 0;
            this.buttonPanic.Text = "Panic";
            this.buttonPanic.UseVisualStyleBackColor = false;
            this.buttonPanic.Click += new System.EventHandler(this.buttonPanic_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.buttonPanic);
            this.panel2.Controls.Add(this.lblPassageBAlias);
            this.panel2.Controls.Add(this.lblPassageAAlias);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.button_lockB);
            this.panel2.Controls.Add(this.button_lockA);
            this.panel2.Controls.Add(this.button_FreeB);
            this.panel2.Controls.Add(this.button_FreeA);
            this.panel2.Controls.Add(this.button_singleB);
            this.panel2.Controls.Add(this.button_singleA);
            this.panel2.Location = new System.Drawing.Point(621, 130);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(450, 337);
            this.panel2.TabIndex = 8;
            this.panel2.TabStop = false;
            this.panel2.Text = "Control";
            // 
            // lblPassageBAlias
            // 
            this.lblPassageBAlias.AutoSize = true;
            this.lblPassageBAlias.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassageBAlias.Location = new System.Drawing.Point(243, 37);
            this.lblPassageBAlias.Name = "lblPassageBAlias";
            this.lblPassageBAlias.Size = new System.Drawing.Size(131, 19);
            this.lblPassageBAlias.TabIndex = 16;
            this.lblPassageBAlias.Text = "Passage B Alias";
            // 
            // lblPassageAAlias
            // 
            this.lblPassageAAlias.AutoSize = true;
            this.lblPassageAAlias.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassageAAlias.Location = new System.Drawing.Point(20, 37);
            this.lblPassageAAlias.Name = "lblPassageAAlias";
            this.lblPassageAAlias.Size = new System.Drawing.Size(128, 19);
            this.lblPassageAAlias.TabIndex = 15;
            this.lblPassageAAlias.Text = "Passage A Alias";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "Passage B";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "Passage A";
            // 
            // button_lockB
            // 
            this.button_lockB.BackColor = System.Drawing.Color.Transparent;
            this.button_lockB.BackgroundImage = global::Semnox.Parafait.Device.Turnstile.TurnstileResource.YellowgreenMenuButton;
            this.button_lockB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_lockB.FlatAppearance.BorderSize = 0;
            this.button_lockB.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_lockB.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_lockB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_lockB.ForeColor = System.Drawing.Color.White;
            this.button_lockB.Location = new System.Drawing.Point(247, 186);
            this.button_lockB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_lockB.Name = "button_lockB";
            this.button_lockB.Size = new System.Drawing.Size(178, 44);
            this.button_lockB.TabIndex = 7;
            this.button_lockB.Text = "Lock";
            this.button_lockB.UseVisualStyleBackColor = false;
            this.button_lockB.Click += new System.EventHandler(this.button_lockB_Click);
            // 
            // button_lockA
            // 
            this.button_lockA.BackColor = System.Drawing.Color.Transparent;
            this.button_lockA.BackgroundImage = global::Semnox.Parafait.Device.Turnstile.TurnstileResource.YellowgreenMenuButton;
            this.button_lockA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_lockA.FlatAppearance.BorderSize = 0;
            this.button_lockA.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_lockA.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_lockA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_lockA.ForeColor = System.Drawing.Color.White;
            this.button_lockA.Location = new System.Drawing.Point(24, 186);
            this.button_lockA.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_lockA.Name = "button_lockA";
            this.button_lockA.Size = new System.Drawing.Size(177, 44);
            this.button_lockA.TabIndex = 6;
            this.button_lockA.Text = "Lock";
            this.button_lockA.UseVisualStyleBackColor = false;
            this.button_lockA.Click += new System.EventHandler(this.button_lockA_Click);
            // 
            // button_FreeB
            // 
            this.button_FreeB.BackColor = System.Drawing.Color.Transparent;
            this.button_FreeB.BackgroundImage = global::Semnox.Parafait.Device.Turnstile.TurnstileResource.YellowgreenMenuButton;
            this.button_FreeB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_FreeB.FlatAppearance.BorderSize = 0;
            this.button_FreeB.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_FreeB.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_FreeB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_FreeB.ForeColor = System.Drawing.Color.White;
            this.button_FreeB.Location = new System.Drawing.Point(246, 123);
            this.button_FreeB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_FreeB.Name = "button_FreeB";
            this.button_FreeB.Size = new System.Drawing.Size(178, 44);
            this.button_FreeB.TabIndex = 5;
            this.button_FreeB.Text = "Free Access";
            this.button_FreeB.UseVisualStyleBackColor = false;
            this.button_FreeB.Click += new System.EventHandler(this.button_FreeB_Click);
            // 
            // button_FreeA
            // 
            this.button_FreeA.BackColor = System.Drawing.Color.Transparent;
            this.button_FreeA.BackgroundImage = global::Semnox.Parafait.Device.Turnstile.TurnstileResource.YellowgreenMenuButton;
            this.button_FreeA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_FreeA.FlatAppearance.BorderSize = 0;
            this.button_FreeA.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_FreeA.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_FreeA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_FreeA.ForeColor = System.Drawing.Color.White;
            this.button_FreeA.Location = new System.Drawing.Point(24, 123);
            this.button_FreeA.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_FreeA.Name = "button_FreeA";
            this.button_FreeA.Size = new System.Drawing.Size(177, 44);
            this.button_FreeA.TabIndex = 4;
            this.button_FreeA.Text = "Free Access";
            this.button_FreeA.UseVisualStyleBackColor = false;
            this.button_FreeA.Click += new System.EventHandler(this.button_FreeA_Click);
            // 
            // button_singleB
            // 
            this.button_singleB.BackColor = System.Drawing.Color.Transparent;
            this.button_singleB.BackgroundImage = global::Semnox.Parafait.Device.Turnstile.TurnstileResource.YellowgreenMenuButton;
            this.button_singleB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_singleB.FlatAppearance.BorderSize = 0;
            this.button_singleB.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_singleB.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_singleB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_singleB.ForeColor = System.Drawing.Color.White;
            this.button_singleB.Location = new System.Drawing.Point(246, 60);
            this.button_singleB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_singleB.Name = "button_singleB";
            this.button_singleB.Size = new System.Drawing.Size(178, 44);
            this.button_singleB.TabIndex = 2;
            this.button_singleB.Text = "Single Access";
            this.button_singleB.UseVisualStyleBackColor = false;
            this.button_singleB.Click += new System.EventHandler(this.button_singleB_Click);
            // 
            // button_singleA
            // 
            this.button_singleA.BackColor = System.Drawing.Color.Transparent;
            this.button_singleA.BackgroundImage = global::Semnox.Parafait.Device.Turnstile.TurnstileResource.YellowgreenMenuButton;
            this.button_singleA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_singleA.FlatAppearance.BorderSize = 0;
            this.button_singleA.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.button_singleA.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_singleA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_singleA.ForeColor = System.Drawing.Color.White;
            this.button_singleA.Location = new System.Drawing.Point(23, 60);
            this.button_singleA.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_singleA.Name = "button_singleA";
            this.button_singleA.Size = new System.Drawing.Size(177, 44);
            this.button_singleA.TabIndex = 0;
            this.button_singleA.Text = "Single Access";
            this.button_singleA.UseVisualStyleBackColor = false;
            this.button_singleA.Click += new System.EventHandler(this.button_singleA_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(618, 472);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(28, 16);
            this.labelInfo.TabIndex = 11;
            this.labelInfo.Text = "Info";
            // 
            // lnkClearLog
            // 
            this.lnkClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkClearLog.AutoSize = true;
            this.lnkClearLog.Location = new System.Drawing.Point(654, 472);
            this.lnkClearLog.Name = "lnkClearLog";
            this.lnkClearLog.Size = new System.Drawing.Size(38, 16);
            this.lnkClearLog.TabIndex = 13;
            this.lnkClearLog.TabStop = true;
            this.lnkClearLog.Text = "Clear";
            this.lnkClearLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearLog_LinkClicked);
            // 
            // frmShowTurnstiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 686);
            this.Controls.Add(this.lnkClearLog);
            this.Controls.Add(this.textBoxEvents);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmShowTurnstiles";
            this.Text = "Site Turnstiles";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmShowTurnstiles_FormClosing);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tcTurnstiles.ResumeLayout(false);
            this.tpStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnstiles)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox panel4;
        private System.Windows.Forms.TextBox textBoxEvents;
        private System.Windows.Forms.GroupBox panel1;
        private System.Windows.Forms.TextBox txtTurnPort;
        private System.Windows.Forms.TextBox txtTurnIp;
        private System.Windows.Forms.Label labelTurn;
        private System.Windows.Forms.GroupBox panel2;
        private System.Windows.Forms.Button buttonPanic;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button button_singleB;
        private System.Windows.Forms.Button button_singleA;
        private System.Windows.Forms.Button button_FreeB;
        private System.Windows.Forms.Button button_FreeA;
        private System.Windows.Forms.Button button_lockB;
        private System.Windows.Forms.Button button_lockA;
        private System.Windows.Forms.LinkLabel lnkClearLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbProfile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPassageBAlias;
        private System.Windows.Forms.Label lblPassageAAlias;
        private System.Windows.Forms.TabControl tcTurnstiles;
        private System.Windows.Forms.TabPage tpStatus;
        private System.Windows.Forms.DataGridView dgvTurnstiles;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTypeMake;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox chkActive;
    }
}

