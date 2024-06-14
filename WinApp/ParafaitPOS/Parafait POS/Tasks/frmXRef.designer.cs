namespace Parafait_POS
{
    partial class frmXRef
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
            this.dgvXRef = new System.Windows.Forms.DataGridView();
            this.card_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parafaitcardnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transfer_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.credits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.courtesy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bonus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticket_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.last_played_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loyalty_points = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.legacyToParafaitDataset1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtParafaitCardNumber = new System.Windows.Forms.TextBox();
            this.txtMCASHCardNumber = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.btnShowCredits = new System.Windows.Forms.Button();
            this.btnShowGames = new System.Windows.Forms.Button();
            this.btnShowDiscounts = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvXRef)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.legacyToParafaitDataset1BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvXRef
            // 
            this.dgvXRef.AllowUserToAddRows = false;
            this.dgvXRef.AllowUserToDeleteRows = false;
            this.dgvXRef.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvXRef.AutoGenerateColumns = false;
            this.dgvXRef.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvXRef.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvXRef.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvXRef.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.card_number,
            this.parafaitcardnumber,
            this.transfer_date,
            this.credits,
            this.courtesy,
            this.bonus,
            this.time,
            this.ticket_count,
            this.last_played_time,
            this.loyalty_points});
            this.dgvXRef.DataSource = this.legacyToParafaitDataset1BindingSource;
            this.dgvXRef.Location = new System.Drawing.Point(12, 74);
            this.dgvXRef.Name = "dgvXRef";
            this.dgvXRef.ReadOnly = true;
            this.dgvXRef.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvXRef.Size = new System.Drawing.Size(884, 382);
            this.dgvXRef.TabIndex = 0;
            this.dgvXRef.SelectionChanged += new System.EventHandler(this.dgvXRef_SelectionChanged);
            // 
            // card_number
            // 
            this.card_number.DataPropertyName = "card_number";
            this.card_number.HeaderText = "Legacy Card Number";
            this.card_number.Name = "card_number";
            this.card_number.ReadOnly = true;
            this.card_number.Width = 121;
            // 
            // parafaitcardnumber
            // 
            this.parafaitcardnumber.DataPropertyName = "parafaitcardnumber";
            this.parafaitcardnumber.HeaderText = "Parafait Card Number";
            this.parafaitcardnumber.Name = "parafaitcardnumber";
            this.parafaitcardnumber.ReadOnly = true;
            this.parafaitcardnumber.Width = 122;
            // 
            // transfer_date
            // 
            this.transfer_date.DataPropertyName = "transfer_date";
            this.transfer_date.HeaderText = "Transfer Date";
            this.transfer_date.Name = "transfer_date";
            this.transfer_date.ReadOnly = true;
            this.transfer_date.Width = 89;
            // 
            // credits
            // 
            this.credits.DataPropertyName = "credits";
            this.credits.HeaderText = "Credits";
            this.credits.Name = "credits";
            this.credits.ReadOnly = true;
            this.credits.Width = 64;
            // 
            // courtesy
            // 
            this.courtesy.DataPropertyName = "courtesy";
            this.courtesy.HeaderText = "courtesy";
            this.courtesy.Name = "courtesy";
            this.courtesy.ReadOnly = true;
            this.courtesy.Width = 72;
            // 
            // bonus
            // 
            this.bonus.DataPropertyName = "bonus";
            this.bonus.HeaderText = "Bonus";
            this.bonus.Name = "bonus";
            this.bonus.ReadOnly = true;
            this.bonus.Width = 62;
            // 
            // time
            // 
            this.time.DataPropertyName = "time";
            this.time.HeaderText = "Time";
            this.time.Name = "time";
            this.time.ReadOnly = true;
            this.time.Width = 55;
            // 
            // ticket_count
            // 
            this.ticket_count.DataPropertyName = "ticket_count";
            this.ticket_count.HeaderText = "Tickets";
            this.ticket_count.Name = "ticket_count";
            this.ticket_count.ReadOnly = true;
            this.ticket_count.Width = 67;
            // 
            // last_played_time
            // 
            this.last_played_time.DataPropertyName = "last_played_time";
            this.last_played_time.HeaderText = "Last Played Time";
            this.last_played_time.Name = "last_played_time";
            this.last_played_time.ReadOnly = true;
            this.last_played_time.Width = 104;
            // 
            // loyalty_points
            // 
            this.loyalty_points.DataPropertyName = "loyalty_points";
            this.loyalty_points.HeaderText = "Loyalty Points";
            this.loyalty_points.Name = "loyalty_points";
            this.loyalty_points.ReadOnly = true;
            this.loyalty_points.Width = 89;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(313, 43);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(497, 43);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtParafaitCardNumber
            // 
            this.txtParafaitCardNumber.Location = new System.Drawing.Point(186, 45);
            this.txtParafaitCardNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtParafaitCardNumber.Name = "txtParafaitCardNumber";
            this.txtParafaitCardNumber.Size = new System.Drawing.Size(108, 20);
            this.txtParafaitCardNumber.TabIndex = 18;
            this.txtParafaitCardNumber.TabStop = false;
            this.txtParafaitCardNumber.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // txtMCASHCardNumber
            // 
            this.txtMCASHCardNumber.Location = new System.Drawing.Point(13, 45);
            this.txtMCASHCardNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtMCASHCardNumber.Name = "txtMCASHCardNumber";
            this.txtMCASHCardNumber.Size = new System.Drawing.Size(143, 20);
            this.txtMCASHCardNumber.TabIndex = 17;
            this.txtMCASHCardNumber.Text = "%";
            this.txtMCASHCardNumber.Enter += new System.EventHandler(this.textBox_Enter);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(183, 28);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Parafait Card Number:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Legacy Card Number:";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(405, 43);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 21;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Image = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnShowKeyPad.Location = new System.Drawing.Point(857, 28);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(36, 36);
            this.btnShowKeyPad.TabIndex = 20003;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            this.btnShowKeyPad.Click += new System.EventHandler(this.btnShowKeyPad_Click);
            // 
            // btnShowCredits
            // 
            this.btnShowCredits.BackColor = System.Drawing.Color.Chocolate;
            this.btnShowCredits.Location = new System.Drawing.Point(54, 475);
            this.btnShowCredits.Margin = new System.Windows.Forms.Padding(4);
            this.btnShowCredits.Name = "btnShowCredits";
            this.btnShowCredits.Size = new System.Drawing.Size(102, 38);
            this.btnShowCredits.TabIndex = 20006;
            this.btnShowCredits.Text = "Credit Plus";
            this.btnShowCredits.UseVisualStyleBackColor = false;
            this.btnShowCredits.Click += new System.EventHandler(this.btnShowCredits_Click);
            // 
            // btnShowGames
            // 
            this.btnShowGames.BackColor = System.Drawing.Color.Chocolate;
            this.btnShowGames.Location = new System.Drawing.Point(180, 475);
            this.btnShowGames.Margin = new System.Windows.Forms.Padding(4);
            this.btnShowGames.Name = "btnShowGames";
            this.btnShowGames.Size = new System.Drawing.Size(78, 38);
            this.btnShowGames.TabIndex = 20007;
            this.btnShowGames.Text = "Games";
            this.btnShowGames.UseVisualStyleBackColor = false;
            this.btnShowGames.Click += new System.EventHandler(this.btnShowGames_Click);
            // 
            // btnShowDiscounts
            // 
            this.btnShowDiscounts.BackColor = System.Drawing.Color.Chocolate;
            this.btnShowDiscounts.Location = new System.Drawing.Point(284, 475);
            this.btnShowDiscounts.Margin = new System.Windows.Forms.Padding(4);
            this.btnShowDiscounts.Name = "btnShowDiscounts";
            this.btnShowDiscounts.Size = new System.Drawing.Size(75, 38);
            this.btnShowDiscounts.TabIndex = 20008;
            this.btnShowDiscounts.Text = "Discounts";
            this.btnShowDiscounts.UseVisualStyleBackColor = false;
            this.btnShowDiscounts.Click += new System.EventHandler(this.btnShowDiscounts_Click);
            // 
            // frmXRef
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(911, 526);
            this.Controls.Add(this.btnShowDiscounts);
            this.Controls.Add(this.btnShowGames);
            this.Controls.Add(this.btnShowCredits);
            this.Controls.Add(this.btnShowKeyPad);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtParafaitCardNumber);
            this.Controls.Add(this.txtMCASHCardNumber);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgvXRef);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmXRef";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XRef";
            this.Load += new System.EventHandler(this.XRef_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvXRef)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.legacyToParafaitDataset1BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvXRef;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtParafaitCardNumber;
        private System.Windows.Forms.TextBox txtMCASHCardNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.BindingSource legacyToParafaitDataset1BindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn card_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn parafaitcardnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn transfer_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn credits;
        private System.Windows.Forms.DataGridViewTextBoxColumn courtesy;
        private System.Windows.Forms.DataGridViewTextBoxColumn bonus;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticket_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn last_played_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn loyalty_points;
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Button btnShowCredits;
        private System.Windows.Forms.Button btnShowGames;
        private System.Windows.Forms.Button btnShowDiscounts;
    }
}