namespace Parafait_POS.Redemption
{
    partial class frmRedemptionCurrency
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
            this.redemptionCurrencyListBS = new System.Windows.Forms.BindingSource(this.components);
            this.dgvRedemptionCurrency = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.currencyIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redemptionCurrencyRuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueInTicketsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalTickets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.redemptionCurrencyListBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptionCurrency)).BeginInit();
            this.SuspendLayout();
            // 
            // redemptionCurrencyListBS
            // 
            this.redemptionCurrencyListBS.DataSource = typeof(Parafait_POS.clsRedemption.clsRedemptionCurrency);
            // 
            // dgvRedemptionCurrency
            // 
            this.dgvRedemptionCurrency.AllowUserToResizeColumns = false;
            this.dgvRedemptionCurrency.AutoGenerateColumns = false;
            this.dgvRedemptionCurrency.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRedemptionCurrency.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(235)))), ((int)(((byte)(252)))));
            this.dgvRedemptionCurrency.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deleteButton,
            this.currencyIdDataGridViewTextBoxColumn,
            this.currencyNameDataGridViewTextBoxColumn,
            this.redemptionCurrencyRuleName,
            this.barCodeDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.valueInTicketsDataGridViewTextBoxColumn,
            this.TotalTickets});
            this.dgvRedemptionCurrency.DataSource = this.redemptionCurrencyListBS;
            this.dgvRedemptionCurrency.Location = new System.Drawing.Point(23, 24);
            this.dgvRedemptionCurrency.Name = "dgvRedemptionCurrency";
            this.dgvRedemptionCurrency.ReadOnly = true;
            this.dgvRedemptionCurrency.RowHeadersVisible = false;
            this.dgvRedemptionCurrency.Size = new System.Drawing.Size(603, 258);
            this.dgvRedemptionCurrency.TabIndex = 15;
            this.dgvRedemptionCurrency.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRedemptionCurrency_CellClick);
            this.dgvRedemptionCurrency.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRedemptionCurrency_CellEnter);
            this.dgvRedemptionCurrency.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvRedemptionCurrency_DataBindingComplete);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.CausesValidation = false;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(267, 300);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(116, 30);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.FillWeight = 63.95939F;
            this.deleteButton.HeaderText = "     X";
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.ReadOnly = true;
            this.deleteButton.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.deleteButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.deleteButton.Text = "X";
            this.deleteButton.ToolTipText = "X";
            this.deleteButton.UseColumnTextForButtonValue = true;
            // 
            // currencyIdDataGridViewTextBoxColumn
            // 
            this.currencyIdDataGridViewTextBoxColumn.DataPropertyName = "currencyId";
            this.currencyIdDataGridViewTextBoxColumn.HeaderText = "Currency Id";
            this.currencyIdDataGridViewTextBoxColumn.Name = "currencyIdDataGridViewTextBoxColumn";
            this.currencyIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.currencyIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // currencyNameDataGridViewTextBoxColumn
            // 
            this.currencyNameDataGridViewTextBoxColumn.DataPropertyName = "currencyName";
            this.currencyNameDataGridViewTextBoxColumn.FillWeight = 107.2081F;
            this.currencyNameDataGridViewTextBoxColumn.HeaderText = "Currency Name";
            this.currencyNameDataGridViewTextBoxColumn.Name = "currencyNameDataGridViewTextBoxColumn";
            this.currencyNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // redemptionCurrencyRuleName
            // 
            this.redemptionCurrencyRuleName.DataPropertyName = "redemptionCurrencyRuleName";
            this.redemptionCurrencyRuleName.FillWeight = 107.2081F;
            this.redemptionCurrencyRuleName.HeaderText = "Rule";
            this.redemptionCurrencyRuleName.Name = "redemptionCurrencyRuleName";
            this.redemptionCurrencyRuleName.ReadOnly = true;
            // 
            // barCodeDataGridViewTextBoxColumn
            // 
            this.barCodeDataGridViewTextBoxColumn.DataPropertyName = "barCode";
            this.barCodeDataGridViewTextBoxColumn.HeaderText = "Bar Code";
            this.barCodeDataGridViewTextBoxColumn.Name = "barCodeDataGridViewTextBoxColumn";
            this.barCodeDataGridViewTextBoxColumn.ReadOnly = true;
            this.barCodeDataGridViewTextBoxColumn.Visible = false;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "quantity";
            this.quantityDataGridViewTextBoxColumn.FillWeight = 107.2081F;
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // valueInTicketsDataGridViewTextBoxColumn
            // 
            this.valueInTicketsDataGridViewTextBoxColumn.FillWeight = 107.2081F;
            this.valueInTicketsDataGridViewTextBoxColumn.HeaderText = "Ticket Value";
            this.valueInTicketsDataGridViewTextBoxColumn.Name = "valueInTicketsDataGridViewTextBoxColumn";
            this.valueInTicketsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // TotalTickets
            // 
            this.TotalTickets.FillWeight = 107.2081F;
            this.TotalTickets.HeaderText = "Total Tickets";
            this.TotalTickets.Name = "TotalTickets";
            this.TotalTickets.ReadOnly = true;
            // 
            // frmRedemptionCurrency
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.ClientSize = new System.Drawing.Size(651, 342);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvRedemptionCurrency);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "frmRedemptionCurrency";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Redemption Currency Details";
            this.Activated += new System.EventHandler(this.frmRedemptionCurrency_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRedemptionCurrency_FormClosed);
            this.Load += new System.EventHandler(this.frmRedemptionCurrency_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmRedemptionCurrency_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.redemptionCurrencyListBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRedemptionCurrency)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource redemptionCurrencyListBS;
        private System.Windows.Forms.DataGridView dgvRedemptionCurrency;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewButtonColumn deleteButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn redemptionCurrencyRuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn barCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueInTicketsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalTickets;
    }
}