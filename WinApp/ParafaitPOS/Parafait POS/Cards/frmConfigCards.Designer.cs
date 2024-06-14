namespace Parafait_POS.Cards
{
    partial class frmConfigCards
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
            this.tcConfigCards = new System.Windows.Forms.TabControl();
            this.tpConfigCards = new System.Windows.Forms.TabPage();
            this.rbExitFreePlayMode = new System.Windows.Forms.RadioButton();
            this.lblMessage = new System.Windows.Forms.Label();
            this.rbChangeSSID = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.rbInvalidateFreePlayCards = new System.Windows.Forms.RadioButton();
            this.grpCardNumber = new System.Windows.Forms.GroupBox();
            this.nudSSID = new System.Windows.Forms.NumericUpDown();
            this.lblSSID = new System.Windows.Forms.Label();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbExitFreePlay = new System.Windows.Forms.RadioButton();
            this.rbFreePlayCard = new System.Windows.Forms.RadioButton();
            this.rbEnterFreePlayMode = new System.Windows.Forms.RadioButton();
            this.tcConfigCards.SuspendLayout();
            this.tpConfigCards.SuspendLayout();
            this.grpCardNumber.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSSID)).BeginInit();
            this.SuspendLayout();
            // 
            // tcConfigCards
            // 
            this.tcConfigCards.Controls.Add(this.tpConfigCards);
            this.tcConfigCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcConfigCards.Location = new System.Drawing.Point(0, 0);
            this.tcConfigCards.Margin = new System.Windows.Forms.Padding(4);
            this.tcConfigCards.Name = "tcConfigCards";
            this.tcConfigCards.SelectedIndex = 0;
            this.tcConfigCards.Size = new System.Drawing.Size(659, 279);
            this.tcConfigCards.TabIndex = 0;
            // 
            // tpConfigCards
            // 
            this.tpConfigCards.Controls.Add(this.rbEnterFreePlayMode);
            this.tpConfigCards.Controls.Add(this.rbExitFreePlayMode);
            this.tpConfigCards.Controls.Add(this.lblMessage);
            this.tpConfigCards.Controls.Add(this.rbChangeSSID);
            this.tpConfigCards.Controls.Add(this.rbInvalidateFreePlayCards);
            this.tpConfigCards.Controls.Add(this.btnCancel);
            this.tpConfigCards.Controls.Add(this.btnSubmit);
            this.tpConfigCards.Controls.Add(this.grpCardNumber);
            this.tpConfigCards.Controls.Add(this.rbExitFreePlay);
            this.tpConfigCards.Controls.Add(this.rbFreePlayCard);
            this.tpConfigCards.Location = new System.Drawing.Point(4, 25);
            this.tpConfigCards.Margin = new System.Windows.Forms.Padding(4);
            this.tpConfigCards.Name = "tpConfigCards";
            this.tpConfigCards.Padding = new System.Windows.Forms.Padding(4);
            this.tpConfigCards.Size = new System.Drawing.Size(651, 250);
            this.tpConfigCards.TabIndex = 0;
            this.tpConfigCards.Text = "Create Master Cards";
            this.tpConfigCards.UseVisualStyleBackColor = true;
            // 
            // rbExitFreePlayMode
            // 
            this.rbExitFreePlayMode.AutoSize = true;
            this.rbExitFreePlayMode.Location = new System.Drawing.Point(233, 54);
            this.rbExitFreePlayMode.Name = "rbExitFreePlayMode";
            this.rbExitFreePlayMode.Size = new System.Drawing.Size(146, 20);
            this.rbExitFreePlayMode.TabIndex = 9;
            this.rbExitFreePlayMode.Text = "Exit Free Play Mode";
            this.rbExitFreePlayMode.UseVisualStyleBackColor = true;
            this.rbExitFreePlayMode.CheckedChanged += new System.EventHandler(this.rbExitFreePlayMode_CheckedChanged);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(3, 231);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(78, 16);
            this.lblMessage.TabIndex = 8;
            this.lblMessage.Text = "card details";
            // 
            // rbChangeSSID
            // 
            this.rbChangeSSID.AutoSize = true;
            this.rbChangeSSID.Location = new System.Drawing.Point(445, 19);
            this.rbChangeSSID.Name = "rbChangeSSID";
            this.rbChangeSSID.Size = new System.Drawing.Size(145, 20);
            this.rbChangeSSID.TabIndex = 7;
            this.rbChangeSSID.Text = "\'Change SSID\' Card";
            this.rbChangeSSID.UseVisualStyleBackColor = true;
            this.rbChangeSSID.CheckedChanged += new System.EventHandler(this.rbChangeSSID_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(401, 181);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(172, 51);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCancel_MouseDown);
            this.btnCancel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCancel_MouseUp);
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSubmit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(96, 181);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(172, 51);
            this.btnSubmit.TabIndex = 5;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.btnSubmit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSubmit_MouseDown);
            this.btnSubmit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSubmit_MouseUp);
            // 
            // rbInvalidateFreePlayCards
            // 
            this.rbInvalidateFreePlayCards.AutoSize = true;
            this.rbInvalidateFreePlayCards.Location = new System.Drawing.Point(445, 54);
            this.rbInvalidateFreePlayCards.Name = "rbInvalidateFreePlayCards";
            this.rbInvalidateFreePlayCards.Size = new System.Drawing.Size(190, 20);
            this.rbInvalidateFreePlayCards.TabIndex = 4;
            this.rbInvalidateFreePlayCards.Text = "Invalidate \'Free Play\' Cards";
            this.rbInvalidateFreePlayCards.UseVisualStyleBackColor = true;
            this.rbInvalidateFreePlayCards.CheckedChanged += new System.EventHandler(this.rbInvalidateFreePlayCards_CheckedChanged);
            // 
            // grpCardNumber
            // 
            this.grpCardNumber.Controls.Add(this.nudSSID);
            this.grpCardNumber.Controls.Add(this.lblSSID);
            this.grpCardNumber.Controls.Add(this.txtCardNumber);
            this.grpCardNumber.Controls.Add(this.label1);
            this.grpCardNumber.Location = new System.Drawing.Point(24, 91);
            this.grpCardNumber.Name = "grpCardNumber";
            this.grpCardNumber.Size = new System.Drawing.Size(601, 62);
            this.grpCardNumber.TabIndex = 2;
            this.grpCardNumber.TabStop = false;
            // 
            // nudSSID
            // 
            this.nudSSID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSSID.Location = new System.Drawing.Point(493, 20);
            this.nudSSID.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudSSID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSSID.Name = "nudSSID";
            this.nudSSID.Size = new System.Drawing.Size(56, 29);
            this.nudSSID.TabIndex = 3;
            this.nudSSID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSSID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblSSID
            // 
            this.lblSSID.AutoSize = true;
            this.lblSSID.Location = new System.Drawing.Point(441, 25);
            this.lblSSID.Name = "lblSSID";
            this.lblSSID.Size = new System.Drawing.Size(49, 16);
            this.lblSSID.TabIndex = 2;
            this.lblSSID.Text = "SSID#:";
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Location = new System.Drawing.Point(248, 22);
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.ReadOnly = true;
            this.txtCardNumber.Size = new System.Drawing.Size(183, 22);
            this.txtCardNumber.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Card Number:";
            // 
            // rbExitFreePlay
            // 
            this.rbExitFreePlay.AutoSize = true;
            this.rbExitFreePlay.Location = new System.Drawing.Point(233, 19);
            this.rbExitFreePlay.Name = "rbExitFreePlay";
            this.rbExitFreePlay.Size = new System.Drawing.Size(146, 20);
            this.rbExitFreePlay.TabIndex = 1;
            this.rbExitFreePlay.Text = "\'Exit Free Play\' Card";
            this.rbExitFreePlay.UseVisualStyleBackColor = true;
            // 
            // rbFreePlayCard
            // 
            this.rbFreePlayCard.AutoSize = true;
            this.rbFreePlayCard.Checked = true;
            this.rbFreePlayCard.Location = new System.Drawing.Point(24, 19);
            this.rbFreePlayCard.Name = "rbFreePlayCard";
            this.rbFreePlayCard.Size = new System.Drawing.Size(122, 20);
            this.rbFreePlayCard.TabIndex = 0;
            this.rbFreePlayCard.TabStop = true;
            this.rbFreePlayCard.Text = "\'Free Play\' Card";
            this.rbFreePlayCard.UseVisualStyleBackColor = true;
            // 
            // rbEnterFreePlayMode
            // 
            this.rbEnterFreePlayMode.AutoSize = true;
            this.rbEnterFreePlayMode.Location = new System.Drawing.Point(24, 54);
            this.rbEnterFreePlayMode.Name = "rbEnterFreePlayMode";
            this.rbEnterFreePlayMode.Size = new System.Drawing.Size(156, 20);
            this.rbEnterFreePlayMode.TabIndex = 10;
            this.rbEnterFreePlayMode.Text = "Enter Free Play Mode";
            this.rbEnterFreePlayMode.UseVisualStyleBackColor = true;
            this.rbEnterFreePlayMode.CheckedChanged += new System.EventHandler(this.rbEnterFreePlayMode_CheckedChanged);
            // 
            // frmConfigCards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(659, 279);
            this.Controls.Add(this.tcConfigCards);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConfigCards";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Config Cards";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmConfigCards_FormClosed);
            this.Load += new System.EventHandler(this.frmConfigCards_Load);
            this.tcConfigCards.ResumeLayout(false);
            this.tpConfigCards.ResumeLayout(false);
            this.tpConfigCards.PerformLayout();
            this.grpCardNumber.ResumeLayout(false);
            this.grpCardNumber.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSSID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcConfigCards;
        private System.Windows.Forms.TabPage tpConfigCards;
        private System.Windows.Forms.RadioButton rbExitFreePlay;
        private System.Windows.Forms.RadioButton rbFreePlayCard;
        private System.Windows.Forms.GroupBox grpCardNumber;
        private System.Windows.Forms.RadioButton rbInvalidateFreePlayCards;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rbChangeSSID;
        private System.Windows.Forms.NumericUpDown nudSSID;
        private System.Windows.Forms.Label lblSSID;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.RadioButton rbExitFreePlayMode;
        private System.Windows.Forms.RadioButton rbEnterFreePlayMode;
    }
}