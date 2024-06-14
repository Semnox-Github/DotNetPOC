namespace Parafait_POS
{
    partial class CreditPlusDetails
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
            this.panelPanel = new System.Windows.Forms.Panel();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbNonZeroBalance = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvConsumption = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvPurchaseCriteria = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvCardCreditPlus = new System.Windows.Forms.DataGridView();
            this.tcGamesAndCreditPlus = new System.Windows.Forms.TabControl();
            this.tpCreditPlus = new System.Windows.Forms.TabPage();
            this.tpGames = new System.Windows.Forms.TabPage();
            this.dgvCardGames = new System.Windows.Forms.DataGridView();
            this.btnCloseGames = new System.Windows.Forms.Button();
            this.dgvCardGameExtended = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.panelPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConsumption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchaseCriteria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardCreditPlus)).BeginInit();
            this.tcGamesAndCreditPlus.SuspendLayout();
            this.tpCreditPlus.SuspendLayout();
            this.tpGames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardGames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardGameExtended)).BeginInit();
            this.SuspendLayout();
            // 
            // panelPanel
            // 
            this.panelPanel.BackColor = System.Drawing.Color.GhostWhite;
            this.panelPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPanel.Controls.Add(this.rbAll);
            this.panelPanel.Controls.Add(this.rbNonZeroBalance);
            this.panelPanel.Controls.Add(this.label2);
            this.panelPanel.Controls.Add(this.dgvConsumption);
            this.panelPanel.Controls.Add(this.label1);
            this.panelPanel.Controls.Add(this.dgvPurchaseCriteria);
            this.panelPanel.Controls.Add(this.btnClose);
            this.panelPanel.Controls.Add(this.dgvCardCreditPlus);
            this.panelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPanel.Location = new System.Drawing.Point(3, 3);
            this.panelPanel.Name = "panelPanel";
            this.panelPanel.Size = new System.Drawing.Size(994, 490);
            this.panelPanel.TabIndex = 0;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(141, 3);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(36, 17);
            this.rbAll.TabIndex = 7;
            this.rbAll.Text = "All";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rbNonZeroBalance
            // 
            this.rbNonZeroBalance.AutoSize = true;
            this.rbNonZeroBalance.Checked = true;
            this.rbNonZeroBalance.Location = new System.Drawing.Point(6, 3);
            this.rbNonZeroBalance.Name = "rbNonZeroBalance";
            this.rbNonZeroBalance.Size = new System.Drawing.Size(112, 17);
            this.rbNonZeroBalance.TabIndex = 6;
            this.rbNonZeroBalance.TabStop = true;
            this.rbNonZeroBalance.Text = "Non-Zero Balance";
            this.rbNonZeroBalance.UseVisualStyleBackColor = true;
            this.rbNonZeroBalance.CheckedChanged += new System.EventHandler(this.rbNonZeroBalance_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(206, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Redemption / Consumption Details";
            // 
            // dgvConsumption
            // 
            this.dgvConsumption.AllowUserToAddRows = false;
            this.dgvConsumption.AllowUserToDeleteRows = false;
            this.dgvConsumption.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConsumption.Location = new System.Drawing.Point(207, 242);
            this.dgvConsumption.Name = "dgvConsumption";
            this.dgvConsumption.ReadOnly = true;
            this.dgvConsumption.Size = new System.Drawing.Size(781, 209);
            this.dgvConsumption.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 226);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "On Purchase Of";
            // 
            // dgvPurchaseCriteria
            // 
            this.dgvPurchaseCriteria.AllowUserToAddRows = false;
            this.dgvPurchaseCriteria.AllowUserToDeleteRows = false;
            this.dgvPurchaseCriteria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPurchaseCriteria.Location = new System.Drawing.Point(8, 242);
            this.dgvPurchaseCriteria.Name = "dgvPurchaseCriteria";
            this.dgvPurchaseCriteria.ReadOnly = true;
            this.dgvPurchaseCriteria.Size = new System.Drawing.Size(182, 209);
            this.dgvPurchaseCriteria.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(454, 457);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(86, 28);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvCardCreditPlus
            // 
            this.dgvCardCreditPlus.AllowUserToAddRows = false;
            this.dgvCardCreditPlus.AllowUserToDeleteRows = false;
            this.dgvCardCreditPlus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCardCreditPlus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCardCreditPlus.Location = new System.Drawing.Point(4, 23);
            this.dgvCardCreditPlus.Name = "dgvCardCreditPlus";
            this.dgvCardCreditPlus.ReadOnly = true;
            this.dgvCardCreditPlus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCardCreditPlus.Size = new System.Drawing.Size(985, 190);
            this.dgvCardCreditPlus.TabIndex = 0;
            this.dgvCardCreditPlus.SelectionChanged += new System.EventHandler(this.dgvCardCreditPlus_SelectionChanged);
            // 
            // tcGamesAndCreditPlus
            // 
            this.tcGamesAndCreditPlus.Controls.Add(this.tpCreditPlus);
            this.tcGamesAndCreditPlus.Controls.Add(this.tpGames);
            this.tcGamesAndCreditPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcGamesAndCreditPlus.ItemSize = new System.Drawing.Size(59, 26);
            this.tcGamesAndCreditPlus.Location = new System.Drawing.Point(0, 0);
            this.tcGamesAndCreditPlus.Name = "tcGamesAndCreditPlus";
            this.tcGamesAndCreditPlus.SelectedIndex = 0;
            this.tcGamesAndCreditPlus.Size = new System.Drawing.Size(1008, 530);
            this.tcGamesAndCreditPlus.TabIndex = 1;
            this.tcGamesAndCreditPlus.SelectedIndexChanged += new System.EventHandler(this.tcGamesAndCreditPlus_SelectedIndexChanged);
            // 
            // tpCreditPlus
            // 
            this.tpCreditPlus.Controls.Add(this.panelPanel);
            this.tpCreditPlus.Location = new System.Drawing.Point(4, 30);
            this.tpCreditPlus.Name = "tpCreditPlus";
            this.tpCreditPlus.Padding = new System.Windows.Forms.Padding(3);
            this.tpCreditPlus.Size = new System.Drawing.Size(1000, 496);
            this.tpCreditPlus.TabIndex = 0;
            this.tpCreditPlus.Text = "CreditPlus / Promotions";
            this.tpCreditPlus.UseVisualStyleBackColor = true;
            // 
            // tpGames
            // 
            this.tpGames.Controls.Add(this.label3);
            this.tpGames.Controls.Add(this.dgvCardGameExtended);
            this.tpGames.Controls.Add(this.dgvCardGames);
            this.tpGames.Controls.Add(this.btnCloseGames);
            this.tpGames.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tpGames.Location = new System.Drawing.Point(4, 30);
            this.tpGames.Name = "tpGames";
            this.tpGames.Padding = new System.Windows.Forms.Padding(3);
            this.tpGames.Size = new System.Drawing.Size(1000, 496);
            this.tpGames.TabIndex = 1;
            this.tpGames.Text = "Packages / Games";
            this.tpGames.UseVisualStyleBackColor = true;
            // 
            // dgvCardGames
            // 
            this.dgvCardGames.AllowUserToAddRows = false;
            this.dgvCardGames.AllowUserToDeleteRows = false;
            this.dgvCardGames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCardGames.Location = new System.Drawing.Point(7, 7);
            this.dgvCardGames.Name = "dgvCardGames";
            this.dgvCardGames.ReadOnly = true;
            this.dgvCardGames.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCardGames.Size = new System.Drawing.Size(987, 252);
            this.dgvCardGames.TabIndex = 3;
            this.dgvCardGames.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCardGames_RowEnter);
            // 
            // btnCloseGames
            // 
            this.btnCloseGames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCloseGames.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseGames.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCloseGames.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCloseGames.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCloseGames.FlatAppearance.BorderSize = 0;
            this.btnCloseGames.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCloseGames.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCloseGames.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseGames.ForeColor = System.Drawing.Color.White;
            this.btnCloseGames.Location = new System.Drawing.Point(459, 462);
            this.btnCloseGames.Name = "btnCloseGames";
            this.btnCloseGames.Size = new System.Drawing.Size(86, 28);
            this.btnCloseGames.TabIndex = 2;
            this.btnCloseGames.Text = "Close";
            this.btnCloseGames.UseVisualStyleBackColor = false;
            this.btnCloseGames.Click += new System.EventHandler(this.btnCloseGames_Click);
            // 
            // dgvCardGameExtended
            // 
            this.dgvCardGameExtended.AllowUserToAddRows = false;
            this.dgvCardGameExtended.AllowUserToDeleteRows = false;
            this.dgvCardGameExtended.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCardGameExtended.Location = new System.Drawing.Point(6, 280);
            this.dgvCardGameExtended.Name = "dgvCardGameExtended";
            this.dgvCardGameExtended.ReadOnly = true;
            this.dgvCardGameExtended.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCardGameExtended.Size = new System.Drawing.Size(987, 176);
            this.dgvCardGameExtended.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 265);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(189, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Extended Inclusion / Exclusions";
            // 
            // CreditPlusDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1008, 530);
            this.Controls.Add(this.tcGamesAndCreditPlus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CreditPlusDetails";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Credit Plus / Promotions / Packages / Games on Card";
            this.Load += new System.EventHandler(this.CreditPlusDetails_Load);
            this.panelPanel.ResumeLayout(false);
            this.panelPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConsumption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchaseCriteria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardCreditPlus)).EndInit();
            this.tcGamesAndCreditPlus.ResumeLayout(false);
            this.tpCreditPlus.ResumeLayout(false);
            this.tpGames.ResumeLayout(false);
            this.tpGames.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardGames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCardGameExtended)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPanel;
        private System.Windows.Forms.DataGridView dgvCardCreditPlus;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvPurchaseCriteria;
        private System.Windows.Forms.DataGridView dgvConsumption;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbNonZeroBalance;
        private System.Windows.Forms.TabControl tcGamesAndCreditPlus;
        private System.Windows.Forms.TabPage tpCreditPlus;
        private System.Windows.Forms.TabPage tpGames;
        private System.Windows.Forms.Button btnCloseGames;
        private System.Windows.Forms.DataGridView dgvCardGames;
        private System.Windows.Forms.DataGridView dgvCardGameExtended;
        private System.Windows.Forms.Label label3;

    }
}