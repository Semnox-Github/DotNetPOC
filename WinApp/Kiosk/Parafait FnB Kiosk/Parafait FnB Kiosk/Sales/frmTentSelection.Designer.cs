namespace Parafait_FnB_Kiosk
{
    partial class frmTentSelection
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
            this.panelBG = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.flpChoiceParameter = new System.Windows.Forms.FlowLayoutPanel();
            this.lblScreenTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblOrderNumber = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblOrderDigit1 = new System.Windows.Forms.Label();
            this.lblOrderDigit2 = new System.Windows.Forms.Label();
            this.lblOrderDigit3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDigit1 = new System.Windows.Forms.Label();
            this.lblDigit2 = new System.Windows.Forms.Label();
            this.lblDigit3 = new System.Windows.Forms.Label();
            this.lblDigit4 = new System.Windows.Forms.Label();
            this.lblDigit5 = new System.Windows.Forms.Label();
            this.lblDigit6 = new System.Windows.Forms.Label();
            this.lblDigit7 = new System.Windows.Forms.Label();
            this.lblDigit8 = new System.Windows.Forms.Label();
            this.lblDigit9 = new System.Windows.Forms.Label();
            this.lblDigit0 = new System.Windows.Forms.Label();
            this.lblBackspace = new System.Windows.Forms.Label();
            this.panelConfirm = new System.Windows.Forms.Panel();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.panelTentNumber = new System.Windows.Forms.Panel();
            this.panelBG.SuspendLayout();
            this.flpChoiceParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.panelConfirm.SuspendLayout();
            this.panelTentNumber.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBG
            // 
            this.panelBG.BackColor = System.Drawing.Color.Transparent;
            this.panelBG.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Popup_955x1845;
            this.panelBG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelBG.Controls.Add(this.btnClose);
            this.panelBG.Controls.Add(this.flpChoiceParameter);
            this.panelBG.Controls.Add(this.panelConfirm);
            this.panelBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBG.Location = new System.Drawing.Point(0, 0);
            this.panelBG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelBG.Name = "panelBG";
            this.panelBG.Size = new System.Drawing.Size(955, 1845);
            this.panelBG.TabIndex = 5;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Close_Btn;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(819, 54);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(76, 76);
            this.btnClose.TabIndex = 0;
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // flpChoiceParameter
            // 
            this.flpChoiceParameter.Controls.Add(this.lblScreenTitle);
            this.flpChoiceParameter.Controls.Add(this.panelTentNumber);
            this.flpChoiceParameter.Controls.Add(this.lblOrderNumber);
            this.flpChoiceParameter.Controls.Add(this.flowLayoutPanel1);
            this.flpChoiceParameter.Controls.Add(this.flowLayoutPanel2);
            this.flpChoiceParameter.Location = new System.Drawing.Point(0, 103);
            this.flpChoiceParameter.Margin = new System.Windows.Forms.Padding(0);
            this.flpChoiceParameter.Name = "flpChoiceParameter";
            this.flpChoiceParameter.Size = new System.Drawing.Size(955, 1575);
            this.flpChoiceParameter.TabIndex = 2;
            // 
            // lblScreenTitle
            // 
            this.lblScreenTitle.Font = new System.Drawing.Font("Bango Pro", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenTitle.ForeColor = System.Drawing.Color.White;
            this.lblScreenTitle.Location = new System.Drawing.Point(125, 0);
            this.lblScreenTitle.Margin = new System.Windows.Forms.Padding(125, 0, 3, 0);
            this.lblScreenTitle.Name = "lblScreenTitle";
            this.lblScreenTitle.Size = new System.Drawing.Size(692, 159);
            this.lblScreenTitle.TabIndex = 5;
            this.lblScreenTitle.Text = "Grab a plastic number located on the front of this kiosk.";
            this.lblScreenTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Parafait_FnB_Kiosk.Properties.Resources.OrderNumber;
            this.pictureBox1.Location = new System.Drawing.Point(42, 111);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(49, 35);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // lblOrderNumber
            // 
            this.lblOrderNumber.Font = new System.Drawing.Font("Bango Pro", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderNumber.ForeColor = System.Drawing.Color.White;
            this.lblOrderNumber.Location = new System.Drawing.Point(125, 643);
            this.lblOrderNumber.Margin = new System.Windows.Forms.Padding(125, 25, 3, 0);
            this.lblOrderNumber.Name = "lblOrderNumber";
            this.lblOrderNumber.Size = new System.Drawing.Size(705, 163);
            this.lblOrderNumber.TabIndex = 7;
            this.lblOrderNumber.Text = "Enter your order number below and place on the order stand at your table";
            this.lblOrderNumber.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.lblOrderDigit1);
            this.flowLayoutPanel1.Controls.Add(this.lblOrderDigit2);
            this.flowLayoutPanel1.Controls.Add(this.lblOrderDigit3);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(217, 821);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(217, 15, 3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(520, 149);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // lblOrderDigit1
            // 
            this.lblOrderDigit1.Font = new System.Drawing.Font("Bango Pro", 71.99999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderDigit1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblOrderDigit1.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Number_Display;
            this.lblOrderDigit1.Location = new System.Drawing.Point(20, 0);
            this.lblOrderDigit1.Margin = new System.Windows.Forms.Padding(20, 0, 3, 0);
            this.lblOrderDigit1.Name = "lblOrderDigit1";
            this.lblOrderDigit1.Size = new System.Drawing.Size(144, 144);
            this.lblOrderDigit1.TabIndex = 8;
            this.lblOrderDigit1.Text = "1";
            this.lblOrderDigit1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOrderDigit2
            // 
            this.lblOrderDigit2.Font = new System.Drawing.Font("Bango Pro", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderDigit2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblOrderDigit2.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Number_Display;
            this.lblOrderDigit2.Location = new System.Drawing.Point(187, 0);
            this.lblOrderDigit2.Margin = new System.Windows.Forms.Padding(20, 0, 3, 0);
            this.lblOrderDigit2.Name = "lblOrderDigit2";
            this.lblOrderDigit2.Size = new System.Drawing.Size(144, 144);
            this.lblOrderDigit2.TabIndex = 9;
            this.lblOrderDigit2.Text = "2";
            this.lblOrderDigit2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOrderDigit3
            // 
            this.lblOrderDigit3.Font = new System.Drawing.Font("Bango Pro", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderDigit3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblOrderDigit3.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Number_Display;
            this.lblOrderDigit3.Location = new System.Drawing.Point(354, 0);
            this.lblOrderDigit3.Margin = new System.Windows.Forms.Padding(20, 0, 3, 0);
            this.lblOrderDigit3.Name = "lblOrderDigit3";
            this.lblOrderDigit3.Size = new System.Drawing.Size(144, 144);
            this.lblOrderDigit3.TabIndex = 10;
            this.lblOrderDigit3.Text = "3";
            this.lblOrderDigit3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.lblDigit1);
            this.flowLayoutPanel2.Controls.Add(this.lblDigit2);
            this.flowLayoutPanel2.Controls.Add(this.lblDigit3);
            this.flowLayoutPanel2.Controls.Add(this.lblDigit4);
            this.flowLayoutPanel2.Controls.Add(this.lblDigit5);
            this.flowLayoutPanel2.Controls.Add(this.lblDigit6);
            this.flowLayoutPanel2.Controls.Add(this.lblDigit7);
            this.flowLayoutPanel2.Controls.Add(this.lblDigit8);
            this.flowLayoutPanel2.Controls.Add(this.lblDigit9);
            this.flowLayoutPanel2.Controls.Add(this.lblDigit0);
            this.flowLayoutPanel2.Controls.Add(this.lblBackspace);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(260, 976);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(260, 3, 3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(435, 572);
            this.flowLayoutPanel2.TabIndex = 9;
            // 
            // lblDigit1
            // 
            this.lblDigit1.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit1.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Key_Pad_Key;
            this.lblDigit1.Location = new System.Drawing.Point(15, 15);
            this.lblDigit1.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit1.Name = "lblDigit1";
            this.lblDigit1.Size = new System.Drawing.Size(114, 114);
            this.lblDigit1.TabIndex = 8;
            this.lblDigit1.Text = "1";
            this.lblDigit1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit1.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit2
            // 
            this.lblDigit2.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit2.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Key_Pad_Key;
            this.lblDigit2.Location = new System.Drawing.Point(159, 15);
            this.lblDigit2.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit2.Name = "lblDigit2";
            this.lblDigit2.Size = new System.Drawing.Size(114, 114);
            this.lblDigit2.TabIndex = 9;
            this.lblDigit2.Text = "2";
            this.lblDigit2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit2.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit3
            // 
            this.lblDigit3.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit3.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Key_Pad_Key;
            this.lblDigit3.Location = new System.Drawing.Point(303, 15);
            this.lblDigit3.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit3.Name = "lblDigit3";
            this.lblDigit3.Size = new System.Drawing.Size(114, 114);
            this.lblDigit3.TabIndex = 10;
            this.lblDigit3.Text = "3";
            this.lblDigit3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit3.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit4
            // 
            this.lblDigit4.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit4.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Key_Pad_Key;
            this.lblDigit4.Location = new System.Drawing.Point(15, 159);
            this.lblDigit4.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit4.Name = "lblDigit4";
            this.lblDigit4.Size = new System.Drawing.Size(114, 114);
            this.lblDigit4.TabIndex = 11;
            this.lblDigit4.Text = "4";
            this.lblDigit4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit4.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit5
            // 
            this.lblDigit5.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit5.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Key_Pad_Key;
            this.lblDigit5.Location = new System.Drawing.Point(159, 159);
            this.lblDigit5.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit5.Name = "lblDigit5";
            this.lblDigit5.Size = new System.Drawing.Size(114, 114);
            this.lblDigit5.TabIndex = 12;
            this.lblDigit5.Text = "5";
            this.lblDigit5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit5.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit6
            // 
            this.lblDigit6.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit6.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Key_Pad_Key;
            this.lblDigit6.Location = new System.Drawing.Point(303, 159);
            this.lblDigit6.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit6.Name = "lblDigit6";
            this.lblDigit6.Size = new System.Drawing.Size(114, 114);
            this.lblDigit6.TabIndex = 13;
            this.lblDigit6.Text = "6";
            this.lblDigit6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit6.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit7
            // 
            this.lblDigit7.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit7.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Key_Pad_Key;
            this.lblDigit7.Location = new System.Drawing.Point(15, 303);
            this.lblDigit7.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit7.Name = "lblDigit7";
            this.lblDigit7.Size = new System.Drawing.Size(114, 114);
            this.lblDigit7.TabIndex = 14;
            this.lblDigit7.Text = "7";
            this.lblDigit7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit7.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit8
            // 
            this.lblDigit8.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit8.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Key_Pad_Key;
            this.lblDigit8.Location = new System.Drawing.Point(159, 303);
            this.lblDigit8.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit8.Name = "lblDigit8";
            this.lblDigit8.Size = new System.Drawing.Size(114, 114);
            this.lblDigit8.TabIndex = 15;
            this.lblDigit8.Text = "8";
            this.lblDigit8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit8.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit9
            // 
            this.lblDigit9.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit9.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Key_Pad_Key;
            this.lblDigit9.Location = new System.Drawing.Point(303, 303);
            this.lblDigit9.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit9.Name = "lblDigit9";
            this.lblDigit9.Size = new System.Drawing.Size(114, 114);
            this.lblDigit9.TabIndex = 16;
            this.lblDigit9.Text = "9";
            this.lblDigit9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit9.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblDigit0
            // 
            this.lblDigit0.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDigit0.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblDigit0.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Key_Pad_Key;
            this.lblDigit0.Location = new System.Drawing.Point(15, 447);
            this.lblDigit0.Margin = new System.Windows.Forms.Padding(15);
            this.lblDigit0.Name = "lblDigit0";
            this.lblDigit0.Size = new System.Drawing.Size(114, 114);
            this.lblDigit0.TabIndex = 18;
            this.lblDigit0.Text = "0";
            this.lblDigit0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDigit0.Click += new System.EventHandler(this.lblDigit_Click);
            // 
            // lblBackspace
            // 
            this.lblBackspace.Font = new System.Drawing.Font("Bango Pro", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBackspace.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.lblBackspace.Image = global::Parafait_FnB_Kiosk.Properties.Resources.Backspace_Btn;
            this.lblBackspace.Location = new System.Drawing.Point(159, 447);
            this.lblBackspace.Margin = new System.Windows.Forms.Padding(15);
            this.lblBackspace.Name = "lblBackspace";
            this.lblBackspace.Size = new System.Drawing.Size(258, 114);
            this.lblBackspace.TabIndex = 19;
            this.lblBackspace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblBackspace.Click += new System.EventHandler(this.lblBackspace_Click);
            // 
            // panelConfirm
            // 
            this.panelConfirm.Controls.Add(this.btnConfirm);
            this.panelConfirm.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelConfirm.Location = new System.Drawing.Point(0, 1680);
            this.panelConfirm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelConfirm.Name = "panelConfirm";
            this.panelConfirm.Size = new System.Drawing.Size(955, 165);
            this.panelConfirm.TabIndex = 4;
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.Green_Btn;
            this.btnConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Bango Pro", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
            this.btnConfirm.Location = new System.Drawing.Point(304, 40);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(346, 68);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "Continue";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // panelTentNumber
            // 
            this.panelTentNumber.BackgroundImage = global::Parafait_FnB_Kiosk.Properties.Resources.OrderNumber;
            this.panelTentNumber.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelTentNumber.Controls.Add(this.pictureBox1);
            this.panelTentNumber.Location = new System.Drawing.Point(0, 159);
            this.panelTentNumber.Margin = new System.Windows.Forms.Padding(0);
            this.panelTentNumber.Name = "panelTentNumber";
            this.panelTentNumber.Size = new System.Drawing.Size(955, 459);
            this.panelTentNumber.TabIndex = 10;
            // 
            // frmTentSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Turquoise;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(955, 1845);
            this.Controls.Add(this.panelBG);
            this.DoubleBuffered = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Name = "frmTentSelection";
            this.Text = "BaseFormProductSale";
            this.TransparencyKey = System.Drawing.Color.Turquoise;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.Load += new System.EventHandler(this.frmTentSelection_Load);
            this.panelBG.ResumeLayout(false);
            this.flpChoiceParameter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.panelConfirm.ResumeLayout(false);
            this.panelTentNumber.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panelBG;
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.FlowLayoutPanel flpChoiceParameter;
        public System.Windows.Forms.Panel panelConfirm;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label lblScreenTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblOrderNumber;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblOrderDigit1;
        private System.Windows.Forms.Label lblOrderDigit2;
        private System.Windows.Forms.Label lblOrderDigit3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label lblDigit1;
        private System.Windows.Forms.Label lblDigit2;
        private System.Windows.Forms.Label lblDigit3;
        private System.Windows.Forms.Label lblDigit4;
        private System.Windows.Forms.Label lblDigit5;
        private System.Windows.Forms.Label lblDigit6;
        private System.Windows.Forms.Label lblDigit7;
        private System.Windows.Forms.Label lblDigit8;
        private System.Windows.Forms.Label lblDigit9;
        private System.Windows.Forms.Label lblDigit0;
        private System.Windows.Forms.Label lblBackspace;
        private System.Windows.Forms.Panel panelTentNumber;
    }
}